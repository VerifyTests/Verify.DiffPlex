﻿namespace VerifyTests;

static class Extensions
{
    public static void TrimEnd(this StringBuilder builder)
    {
        if (builder.Length == 0)
        {
            return;
        }

        var i = builder.Length - 1;
        for (; i >= 0; i--)
        {
            if (!char.IsWhiteSpace(builder[i]))
            {
                break;
            }
        }

        if (i < builder.Length - 1)
        {
            builder.Length = i + 1;
        }
    }
}