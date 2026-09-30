## Deprecated

Verify now shows a text diff in the exception message by default, using [DiffEngine](https://github.com/VerifyTests/DiffEngine)'s `TextDiff`, so this package is no longer required. See [Text Diff Format](https://github.com/VerifyTests/Verify/blob/main/docs/exception-message-format.md#text-diff-format).

To migrate, remove the Verify.DiffPlex package and:

| Verify.DiffPlex | Verify |
|---|---|
| `VerifyDiffPlex.Initialize()` | Remove. Compact is the default. |
| `VerifyDiffPlex.Initialize(OutputType.Full)` | `VerifierSettings.UseTextDiffFormat(TextDiffFormat.Full)` |
| `VerifyDiffPlex.Initialize(OutputType.Compact)` | Remove. Compact is the default. |
| `VerifyDiffPlex.Initialize(OutputType.Minimal)` | `VerifierSettings.UseTextDiffFormat(TextDiffFormat.Minimal)` |
| `settings.UseDiffPlex()` | Remove. The format can only be set globally. |

Differences from Verify.DiffPlex:

 * Whitespace and case count. Verify.DiffPlex ignored whitespace, so a failure caused only by whitespace showed no changed lines.
 * Only trailing line breaks are trimmed from the diff, not trailing spaces.
 * A string comparer's message still takes precedence over the diff.
