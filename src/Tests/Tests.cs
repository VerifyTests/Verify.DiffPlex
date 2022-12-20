[TestFixture]
public class Tests
{
    static Tests()
    {
        VerifierSettings.ScrubLinesContaining("DiffEngineTray");
        VerifierSettings.IgnoreStackTrace();
        VerifyDiffPlex.Initialize();
    }

    [Test]
    public Task Simple()
    {
        var settings = new VerifySettings();
        settings.UseMethodName("Foo");
        settings.DisableDiff();

        return ThrowsTask(() =>
            Verify("""
                    The
                    after
                    text
                    """,
                settings));
    }

    [Test]
    public Task Sample()
    {
        var target = """
            The
            after
            text
            """;
        return Verify(target);
    }
}