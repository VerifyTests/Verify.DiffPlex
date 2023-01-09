using VerifyTests.DiffPlex;

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
        settings.UseMethodName("SimpleFake");
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
    public Task AtTestLevel()
    {
        var settings = new VerifySettings();
        settings.UseMethodName("AtTestLevelFake");
        settings.DisableDiff();
        settings.UseDiffPlex();

        return ThrowsTask(() =>
            Verify("""
                    The
                    after
                    text
                    """,
                settings));
    }

    [Test]
    public Task AtTestLevelCompact()
    {
        var settings = new VerifySettings();
        settings.UseMethodName("AtTestLevelCompactFake");
        settings.DisableDiff();
        settings.UseDiffPlex(OutputType.Compact);

        return ThrowsTask(() =>
            Verify("""
                    The
                    The
                    The
                    The
                    The
                    The
                    The
                    The
                    The
                    The
                    The
                    The
                    The
                    after
                    text
                    text
                    text
                    text
                    text
                    text
                    text
                    text
                    text
                    text
                    text
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

    #region TestLevelUsage

    [Test]
    public Task TestLevelUsage()
    {
        var target = "The text";
        var settings = new VerifySettings();
        settings.UseDiffPlex();
        return Verify(target, settings);
    }

    #endregion

    #region TestLevelUsageFluent

    [Test]
    public Task TestLevelUsageFluent()
    {
        var target = "The text";
        return Verify(target)
            .UseDiffPlex();
    }

    #endregion
}