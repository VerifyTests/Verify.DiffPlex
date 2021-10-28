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

        void AddToBuilder(int? lineNumber, string symbol, string text)
        {
            var padding = lineNumber?.ToString("D" + paddingLength) ?? defaultPadding;
            builder.AppendLine($"{padding} {symbol} {text}");
        }

        DiffPiece? prevLine = null;
        for (int i = 0; i < diff.Lines.Count; i++)
        {
            var currentLine = diff.Lines[i];
            var nextLine = i < diff.Lines.Count - 1
                ? diff.Lines[i + 1]
                : null;

            switch (currentLine.Type)
            {
                case ChangeType.Inserted:
                    AddToBuilder(null, "+", currentLine.Text);
                    break;
                case ChangeType.Deleted:
                    AddToBuilder(null, "-", currentLine.Text);
                    break;
                default:
                    if (IsChanged(prevLine) || IsChanged(nextLine))
                    {
                        AddToBuilder(currentLine.Position, " ", currentLine.Text);
                        if (IsChanged(prevLine))
                            builder.AppendLine();
                    }
                    break;
            }

            prevLine = currentLine;
        }

        return builder;
    }
}