using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Isle.Core.Tests;

public class StringBuilderExtensionsTests
{
    [Test]
    public void EscapeAndAppend([Values("abc", "a{c", "a}c", "{", "}", "", null)] string str)
    {
        var builder = new StringBuilder();
        builder.EscapeAndAppend(str);
        var result = builder.ToString();
        result.Should().Be((str ?? "").Replace("{", "{{").Replace("}", "}}"));
    }
}