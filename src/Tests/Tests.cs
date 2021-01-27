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
        VerifierSettings.ModifySerialization(
            settings => settings.IgnoreMember<Exception>(_ => _.StackTrace));
    }

    [Test]
    public async Task Simple()
    {
        var settings = new VerifySettings();
        settings.UseMethodName("Foo");
        settings.DisableDiff();
        settings.DisableClipboard();

        await Verifier.Verify(
                @"The
before
text", settings)
            .AutoVerify();

        FileNameBuilder.ClearPrefixList();
        var exception = Assert.ThrowsAsync<Exception>(() =>
            Verifier.Verify(
                @"The
after
text",
                settings));
        await Verifier.Verify(exception!.Message);
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