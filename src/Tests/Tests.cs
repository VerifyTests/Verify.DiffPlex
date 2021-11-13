using VerifyTests;
using VerifyNUnit;
using NUnit.Framework;

[TestFixture]
public class Tests
{
    static Tests()
    {
        VerifierSettings.DisableClipboard();
        VerifierSettings.ModifySerialization(
            settings => settings.IgnoreMember<Exception>(_ => _.StackTrace));
    }

    [Test]
    public Task Simple()
    {
        VerifyDiffPlex.Initialize();
        var settings = new VerifySettings();
        settings.UseMethodName("Foo");
        settings.DisableDiff();

        return Verifier.ThrowsTask(() =>
                Verifier.Verify(
                    @"The
after
text",
                    settings))
            .ScrubLinesContaining("DiffEngineTray");
    }

    [Test]
    public Task Sample()
    {
        VerifyDiffPlex.Initialize();
        var target = @"The
after
text";
        return Verifier.Verify(target);
    }

    [Test]
    public Task Compact()
    {
        VerifyDiffPlex.Initialize(OutputType.Compact);
        var settings = new VerifySettings();
        settings.UseMethodName("Bar");
        settings.DisableDiff();

        return Verifier.ThrowsTask(() =>
                Verifier.Verify(
                    @"Line 1 changed
Line 2
Line 3
Line 4
Line 5 changed
Line 6
Added
Line 7 changed
Line 8
Line 9
Line 10
Line 11 changed
Line 12 changed",
                    settings))
            .ScrubLinesContaining("DiffEngineTray");
    }
}