using VerifyTests.DiffPlex;

[TestFixture]
public class Tests
{
    #region OutputTypeCompact

    [ModuleInitializer]
    public static void Init() =>
        VerifyDiffPlex.Initialize(OutputType.Compact);

    #endregion

    [ModuleInitializer]
    public static void OtherInit()
    {
        VerifierSettings.ScrubLinesContaining("DiffEngineTray");
        VerifierSettings.IgnoreStackTrace();
    }

    [Test]
    public Task Compact()
    {
        var settings = new VerifySettings();
        settings.UseMethodName("Bar");
        settings.DisableDiff();

        return ThrowsTask(() =>
            Verify("""
                    Line 1 changed
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
                    Line 12 changed
                    """,
                settings));
    }
}