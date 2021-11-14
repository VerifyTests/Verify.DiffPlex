using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

namespace VerifyTests;

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
            builder.TrimEnd();
            var message = builder.ToString();
            var compareResult = CompareResult.NotEqual(message);
            return Task.FromResult(compareResult);
        });
    }

    static StringBuilder VerboseCompare(string received, string verified)
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

    static StringBuilder CompactCompare(string received, string verified)
    {
        var diff = InlineDiffBuilder.Diff(verified, received);
        var builder = new StringBuilder();

        var prefixLength = diff.Lines.Max(l => l.Position).ToString().Length;
        var spacePrefix = new string(' ', prefixLength - 1);

        static bool IsChanged(DiffPiece? line) => line?.Type is ChangeType.Inserted or ChangeType.Deleted;

        void AddDiffLine(int? lineNumber, string symbol, string text)
        {
            var prefix = lineNumber?.ToString("D" + prefixLength) ?? spacePrefix + symbol;
            builder.AppendLine($"{prefix} {text}");
        }

        DiffPiece? prevLine = null;
        var lastIndex = diff.Lines.Count - 1;

        for (var i = 0; i <= lastIndex; i++)
        {
            var currentLine = diff.Lines[i];
            var nextLine = i < lastIndex
                ? diff.Lines[i + 1]
                : null;

            if (IsChanged(currentLine))
            {
                if (i == 0)
                {
                    AddDiffLine(null, " ", "[BOF]");
                }

                var symbol = currentLine.Type == ChangeType.Inserted ? "+" : "-";
                AddDiffLine(null, symbol, currentLine.Text);

                if (i == lastIndex)
                {
                    AddDiffLine(null, " ", "[EOF]");
                }
            }
            else if (IsChanged(prevLine) || IsChanged(nextLine))
            {
                AddDiffLine(currentLine.Position, " ", currentLine.Text);
                if (!IsChanged(nextLine))
                {
                    builder.AppendLine();
                }
            }

            prevLine = currentLine;
        }
        
        return builder;
    }
}