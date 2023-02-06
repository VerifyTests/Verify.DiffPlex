public static class ModuleInitializer
{
    #region Initialize

    [ModuleInitializer]
    public static void Initialize() =>
        VerifyDiffPlex.Initialize();

    #endregion

    [ModuleInitializer]
    public static void OtherInitialize()
    {
        VerifierSettings.InitializePlugins();
        VerifierSettings.ScrubLinesContaining("DiffEngineTray");
        VerifierSettings.IgnoreStackTrace();
    }
}