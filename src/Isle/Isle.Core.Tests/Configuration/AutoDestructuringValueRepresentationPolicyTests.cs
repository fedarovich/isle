using System;
using System.Collections.Generic;
using FluentAssertions;
using Isle.Configuration;
using NUnit.Framework;

namespace Isle.Core.Tests.Configuration;

[TestFixture(typeof(byte), ValueRepresentation.Default)]
[TestFixture(typeof(ushort), ValueRepresentation.Default)]
[TestFixture(typeof(uint), ValueRepresentation.Default)]
[TestFixture(typeof(ulong), ValueRepresentation.Default)]
[TestFixture(typeof(nuint), ValueRepresentation.Default)]
[TestFixture(typeof(sbyte), ValueRepresentation.Default)]
[TestFixture(typeof(short), ValueRepresentation.Default)]
[TestFixture(typeof(int), ValueRepresentation.Default)]
[TestFixture(typeof(long), ValueRepresentation.Default)]
[TestFixture(typeof(nint), ValueRepresentation.Default)]
[TestFixture(typeof(float), ValueRepresentation.Default)]
[TestFixture(typeof(double), ValueRepresentation.Default)]
[TestFixture(typeof(decimal), ValueRepresentation.Default)]
[TestFixture(typeof(bool), ValueRepresentation.Default)]
[TestFixture(typeof(char), ValueRepresentation.Default)]
[TestFixture(typeof(string), ValueRepresentation.Default)]
[TestFixture(typeof(DateTime), ValueRepresentation.Default)]
[TestFixture(typeof(DateTimeOffset), ValueRepresentation.Default)]
[TestFixture(typeof(TimeSpan), ValueRepresentation.Default)]
#if NET6_0_OR_GREATER
[TestFixture(typeof(DateOnly), ValueRepresentation.Default)]
[TestFixture(typeof(TimeOnly), ValueRepresentation.Default)]
#endif
#if NET7_0_OR_GREATER
[TestFixture(typeof(Int128), ValueRepresentation.Default)]
[TestFixture(typeof(UInt128), ValueRepresentation.Default)]
#endif
[TestFixture(typeof(Guid), ValueRepresentation.Default)]
[TestFixture(typeof(Uri), ValueRepresentation.Default)]
[TestFixture(typeof(UriKind), ValueRepresentation.Default)] // Enum
[TestFixture(typeof(TestStruct), ValueRepresentation.Destructure)] // Struct
[TestFixture(typeof(EventArgs), ValueRepresentation.Destructure)] // Class
[TestFixture(typeof(int[]), ValueRepresentation.Destructure)] // Array
[TestFixture(typeof(List<int>), ValueRepresentation.Destructure)] // Collection
[TestFixture(typeof(object), ValueRepresentation.Destructure)]
public class AutoDestructuringValueRepresentationPolicyTests<T>
{
    private readonly ValueRepresentation _expectedRepresentation;

    public AutoDestructuringValueRepresentationPolicyTests(ValueRepresentation expectedRepresentation)
    {
        _expectedRepresentation = expectedRepresentation;
    }

    [Test]
    public void GetRepresentationOfType()
    {
        AutoDestructuringValueRepresentationPolicy.Instance.GetRepresentationOfType<T>().Should().Be(_expectedRepresentation);
    }
}