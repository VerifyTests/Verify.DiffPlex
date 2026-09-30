[Documentation](https://github.com/VerifyTests/Verify.DiffPlex)

Extends [Verify](https://github.com/VerifyTests/Verify) to allow [comparison](https://github.com/VerifyTests/Verify/blob/master/docs/comparer.md) of text via [DiffPlex](https://github.com/mmanela/diffplex).<!-- singleLineInclude: intro. path: /docs/intro.include.md -->

**See [Milestones](https://github.com/VerifyTests/Verify.DiffPlex/milestones?state=closed) for release notes.**


## Deprecated<!-- include: deprecated. path: /docs/deprecated.include.md -->

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
 * A string comparer's message still takes precedence over the diff.<!-- endInclude -->


## Sponsors


### Entity Framework Extensions<!-- include: sponsors. path: /docs/sponsors.include.md -->

[Entity Framework Extensions](https://entityframework-extensions.net/?utm_source=simoncropp&utm_medium=Verify.DiffPlex) is a major sponsor and is proud to contribute to the development this project.

[![Entity Framework Extensions](https://raw.githubusercontent.com/VerifyTests/Verify.DiffPlex/refs/heads/main/docs/zzz.png)](https://entityframework-extensions.net/?utm_source=simoncropp&utm_medium=Verify.DiffPlex)

### Developed using JetBrains IDEs

[![JetBrains logo.](https://raw.githubusercontent.com/VerifyTests/Verify.DiffPlex/main/docs/jetbrains.png)](https://jb.gg/OpenSourceSupport)<!-- endInclude -->
