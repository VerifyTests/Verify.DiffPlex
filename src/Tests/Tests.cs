[TestFixture]
public class Tests
{
    static Tests()
    {
        VerifierSettings.ScrubLinesContaining("DiffEngineTray");
        VerifierSettings.IgnoreStackTrace();
    }

    [Test]
    public Task Simple()
    {
        VerifyDiffPlex.Initialize();
        return ThrowsTask(() =>
                Verify(
                    @"The
after
text"))
            .UseMethodName("Foo")
            .DisableDiff();
    }

    [Test]
    public Task Sample()
    {
        VerifyDiffPlex.Initialize();
        var target = @"The
after
text";
        return Verify(target);
    }

    [Test]
    public Task Compact()
    {
        VerifyDiffPlex.Initialize(OutputType.Compact);
        return ThrowsTask(() =>
                Verify(
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
Line 12 changed"))
            .UseMethodName("Bar")
            .DisableDiff();
    }
}