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
    public async Task Simple()
    {
        VerifyDiffPlex.Initialize();
        var settings = new VerifySettings();
        settings.UseMethodName("Foo");
        settings.DisableDiff();

        await Verifier.ThrowsTask(() =>
                Verifier.Verify(
                    @"The
after
text",
                    settings))
            .ScrubLinesContaining("DiffEngineTray");
    }

    [Test]
    public async Task Sample()
    {
        VerifyDiffPlex.Initialize();
        var target = @"The
after
text";
        await Verifier.Verify(target);
    }

    [Test]
    public async Task Compact()
    {
        VerifyDiffPlex.Initialize(compact: true);
        var settings = new VerifySettings();
        settings.UseMethodName("Bar");
        settings.DisableDiff();

        await Verifier.ThrowsTask(() =>
                Verifier.Verify(
                    @"Line1
Line2 - changed
Line3
Line4
Line5
Line5a - added
Line6
Line7
Line8
Line10",
                    settings))
            .ScrubLinesContaining("DiffEngineTray");

    }
}