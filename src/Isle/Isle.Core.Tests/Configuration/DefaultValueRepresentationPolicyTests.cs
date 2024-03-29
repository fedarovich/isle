﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using Isle.Configuration;
using NUnit.Framework;

namespace Isle.Core.Tests.Configuration;

[TestFixture(typeof(byte))]
[TestFixture(typeof(ushort))]
[TestFixture(typeof(uint))]
[TestFixture(typeof(ulong))]
[TestFixture(typeof(nuint))]
[TestFixture(typeof(sbyte))]
[TestFixture(typeof(short))]
[TestFixture(typeof(int))]
[TestFixture(typeof(long))]
[TestFixture(typeof(nint))]
[TestFixture(typeof(float))]
[TestFixture(typeof(double))]
[TestFixture(typeof(decimal))]
[TestFixture(typeof(bool))]
[TestFixture(typeof(char))]
[TestFixture(typeof(string))]
[TestFixture(typeof(DateTime))]
[TestFixture(typeof(DateTimeOffset))]
[TestFixture(typeof(TimeSpan))]
#if NET6_0_OR_GREATER
[TestFixture(typeof(DateOnly))]
[TestFixture(typeof(TimeOnly))]
#endif
#if NET7_0_OR_GREATER
[TestFixture(typeof(Int128))]
[TestFixture(typeof(UInt128))]
#endif
[TestFixture(typeof(Guid))]
[TestFixture(typeof(Uri))]
[TestFixture(typeof(UriKind))] // Enum
[TestFixture(typeof(TestStruct))] // Struct
[TestFixture(typeof(EventArgs))] // Class
[TestFixture(typeof(int[]))] // Array
[TestFixture(typeof(List<int>))] // Collection
[TestFixture(typeof(object))]
public class DefaultValueRepresentationPolicyTests<T>
{
    [Test]
    public void GetRepresentationOfType()
    {
        DefaultValueRepresentationPolicy.Instance.GetRepresentationOfType<T>().Should().Be(ValueRepresentation.Default);
    }
}