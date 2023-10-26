namespace VerifyTests;

public static class VerifyDiffPlex
{
    public static bool Initialized { get; private set; }

    public static void Initialize() => Initialize(OutputType.Full);

    static Func<string, string, StringBuilder> GetCompareFunc(OutputType outputType) =>
        outputType switch
        {
            OutputType.Compact => CompactCompare,
            OutputType.Minimal => MinimalCompare,
            _ => VerboseCompare
        };

    public static void Initialize(OutputType outputType)
    {
        if (Initialized)
        {
            throw new("Already Initialized");
        }

        Initialized = true;
        InnerVerifier.ThrowIfVerifyHasBeenRun();
        VerifierSettings.SetDefaultStringComparer((received, verified, _) => GetResult(outputType, received, verified));
    }

    static Task<CompareResult> GetResult(OutputType outputType, string received, string verified)
    {
        var compare = GetCompareFunc(outputType);
        var builder = compare(received, verified);
        builder.TrimEnd();
        var message = builder.ToString();
        var result = CompareResult.NotEqual(message);
        return Task.FromResult(result);
    }

    public static void UseDiffPlex(this VerifySettings settings, OutputType outputType = OutputType.Full) =>
        settings.UseStringComparer(
            (received, verified, _) => GetResult(outputType, received, verified));

    public static SettingsTask UseDiffPlex(this SettingsTask settings, OutputType outputType = OutputType.Full) =>
        settings.UseStringComparer(
            (received, verified, _) => GetResult(outputType, received, verified));

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

    static StringBuilder MinimalCompare(string received, string verified)
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
                    // omit unchanged files
                    continue;
            }

            builder.AppendLine(line.Text);
        }

        return builder;
    }

    static StringBuilder CompactCompare(string received, string verified)
    {
        var diff = InlineDiffBuilder.Diff(verified, received);
        var builder = new StringBuilder();

        // ReSharper disable once RedundantSuppressNullableWarningExpression
        var prefixLength = diff.Lines.Max(_ => _.Position).ToString()!.Length;
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