using System;
using System.Threading.Tasks;
using DiffEngine;
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
    public async Task Sample()
    {
        var target = @"The
after
text";
        await Verifier.Verify(target);
    }
}