using System;

namespace Codurance.Domain.Unit.Tests
{
    public static class TestHelper
    {
        public static string NewRow(this string source)
        {
            return source + Environment.NewLine;
        }
    }
}
