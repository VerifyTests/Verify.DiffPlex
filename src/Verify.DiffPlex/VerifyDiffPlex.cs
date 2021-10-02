using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

namespace VerifyTests;

public static class VerifyDiffPlex
{
    public static void Initialize()
    {
        VerifierSettings.SetDefaultStringComparer((received, verified, _) =>
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

            var compareResult = CompareResult.NotEqual(builder.ToString());
            return Task.FromResult(compareResult);
        });
    }
}