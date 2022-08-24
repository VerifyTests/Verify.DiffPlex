# <img src="/src/icon.png" height="30px"> Verify.DiffPlex

[![Build status](https://ci.appveyor.com/api/projects/status/9ug1ufa69m4vf4ph?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-DiffPlex)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.DiffPlex.svg)](https://www.nuget.org/packages/Verify.DiffPlex/)

Extends [Verify](https://github.com/VerifyTests/Verify) to allow [comparison](https://github.com/VerifyTests/Verify/blob/master/docs/comparer.md) of text via [DiffPlex](https://github.com/mmanela/diffplex).



## NuGet package

https://nuget.org/packages/Verify.DiffPlex/


## Usage


### Initialize

Call `VerifyDiffPlex.Initialize()` once at assembly load time to get the default (full) diff output. Alternatively, use `VerifyDiffPlex.Initialize(OutputType.Full)` or `VerifyDiffPlex.Initialize(OutputType.Compact)` to specify the type of output (see below).

```
public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifyDiffPlex.Initialize();
    }
}
```


### Verify text

Given an existing verified file:

```
The
before
text
```

And a test:

```cs
[Test]
public async Task Sample()
{
    var target = @"The
after
text";
    await Verifier.Verify(target);
}
```


### Diff results

When the comparison fails, the resulting differences will be included in the test result displayed to the user. This example shows the `Full` style of output.

```txt
Results do not match.
Differences:
Received: Tests.Sample.received.txt
Verified: Tests.Sample.verified.txt
Compare Result:
  The
- before
+ after
  text
```


### Output types

The library currently supports two different types of diff outputs; the desired type can be specified during library initialization.

`OutputType.Full` is the default. It shows the full contents of the received file, with differences with the received file indicated by `+` and `-`. Here's an example of `Full` output.

```
  First line
- Second line
+ Second line changed
  Third line
  Fourth line
  Fifth line
- Sixth line
+ Sixth line changed
  Seventh line
  Eighth line
```

This output type gives the most information, but if verified files are long, it can be difficult to read through and find the actual differences. `OutputType.Compact` will show only the changed lines, with one line of context (with line number) before and after each changed section to help identify where the change is. The `Full` output above would look like this as `Compact`.

```
1 First line
- Second line
+ Second line changed
3 Third line

5 Fifth line
- Sixth line
+ Sixth line changed
7 Seventh line
```
