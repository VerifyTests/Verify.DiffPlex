using System;
using System.Threading.Tasks;
using VerifyTests;
using VerifyNUnit;
using NUnit.Framework;

[TestFixture]
public class Tests
{
    static Tests()
    {
        VerifyDiffPlex.Initialize();
        VerifierSettings.DisableClipboard();
        VerifierSettings.ModifySerialization(
            settings => settings.IgnoreMember<Exception>(_ => _.StackTrace));
    }

    [Test]
    public async Task Simple()
    {
        var settings = new VerifySettings();
        settings.UseMethodName("Foo");
        settings.DisableDiff();

        await Verifier.Verify(
                @"The
before
text", settings)
            .AutoVerify();

        FileNameBuilder.ClearPrefixList();
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
        var target = @"The
after
text";
        await Verifier.Verify(target);
    }
}