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
    }

    [Test]
    public async Task Simple()
    {
        var settings = new VerifySettings();
        settings.UseMethodName("Foo");

        await Verifier.Verify(
                @"The
before
text", settings)
            .AutoVerify();

        FileNameBuilder.ClearPrefixList();

        await Verifier.Verify(
            @"The
after
text",
            settings);
    }
}