using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

namespace VerifyTests;

public enum OutputType { Full, Compact }

public static class VerifyDiffPlex
{
    public static void Initialize() => Initialize(OutputType.Full);
    
    public static void Initialize(OutputType outputType)
    {
        Func<string, string, StringBuilder> compareFunc = outputType switch
        {
            OutputType.Compact => CompactCompare,
            _ => VerboseCompare,
        };
        
        VerifierSettings.SetDefaultStringComparer((received, verified, _) =>
        {
            var builder = compareFunc(received, verified);
            
            var compareResult = CompareResult.NotEqual(builder.ToString());
            return Task.FromResult(compareResult);
        });
    }

    private static StringBuilder VerboseCompare(string received, string verified)
    {
        var diff = InlineDiffBuilder.Diff(verified, received);

        var builder = new StringBuilder();
        foreach (var line in diff.Lines)
        {
            switch (line.Type)
            {
                case ChangeType.Inserted:
                    builder.Append("+ ");
                    break;
                case ChangeType.Deleted:
                    builder.Append("- ");
                    break;
                default:
                    builder.Append("  ");
                    break;
            }
            builder.AppendLine(line.Text);
        }

        return builder;
    }

    private static StringBuilder CompactCompare(string received, string verified)
    {
        var diff = InlineDiffBuilder.Diff(verified, received);
        var builder = new StringBuilder();

        var paddingLength = diff.Lines.Max(l => l.Position).ToString().Length;
        var defaultPadding = new String(' ', paddingLength);

        bool IsChanged(DiffPiece? line) => line?.Type == ChangeType.Inserted || line?.Type == ChangeType.Deleted;

        void AddDiffLine(int? lineNumber, string symbol, string text)
        {
            var padding = lineNumber?.ToString("D" + paddingLength) ?? defaultPadding;
            builder.AppendLine($"{padding} {symbol} {text}");
        }

        DiffPiece? prevLine = null;
        int lastIndex = diff.Lines.Count - 1;

        for (int i = 0; i <= lastIndex; i++)
        {
            var currentLine = diff.Lines[i];
            var nextLine = i < lastIndex
                ? diff.Lines[i + 1]
                : null;

            if (IsChanged(currentLine))
            {
                if (i == 0)
                    AddDiffLine(null, " ", "[BOF]");

                var symbol = currentLine.Type == ChangeType.Inserted ? "+" : "-";
                AddDiffLine(null, symbol, currentLine.Text);

                if (i == lastIndex)
                    AddDiffLine(null, " ", "[EOF]");
            }
            else if (IsChanged(prevLine) || IsChanged(nextLine))
            {
                AddDiffLine(currentLine.Position, " ", currentLine.Text);
                if (!IsChanged(nextLine))
                    builder.AppendLine();
            }

            prevLine = currentLine;
        }

        return builder;
    }
}