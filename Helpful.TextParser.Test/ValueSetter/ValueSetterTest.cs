using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using Shouldly;

namespace Helpful.TextParser.Test.ValueSetter
{
    public class ValueSetterTest
    {
        [Test]
        [TestCase("NullableDateTime", null, true, null)]
        [TestCase("Boolean", "True", true, true)]
        [TestCase("Byte", "254", true, 254)]
        [TestCase("SByte", "120", true, 120)]
        [TestCase("Char", "M", true, 'M')]
        [TestCase("Decimal", "3546744.35", true, 3546744.35)]
        [TestCase("Double", "224455.221", true, 224455.221)]
        [TestCase("Float", "1243.35", true, 1243.35f)]
        [TestCase("Int", "-352623", true, -352623)]
        [TestCase("UInt", "123534", true, 123534)]
        [TestCase("Long", "-12352326", true, -12352326)]
        [TestCase("ULong", "356475777676", true, 356475777676)]
        [TestCase("Object", "ThisIsObject", true, "ThisIsObject")]
        [TestCase("Short", "-5442", true, -5442)]
        [TestCase("UShort", "46777", true, 46777)]
        [TestCase("String", "ThisIsString", true, "ThisIsString")]
        [TestCase("Int", "M", false, null)]
        [TestCase("Int", null, false, null)]
        [TestCase("Int", "699999999999999999999999", false, null)]
        [TestCase("DateTime", "2017-31-32", false, null)]
        public void Set_SetValue_ValueIsSet(string property, string value, bool isConverted, object expectedValue)
        {
            var dummy = new DummyWithAllProperties();

            var propertyInfo = typeof(DummyWithAllProperties).GetProperty(property, BindingFlags.Public | BindingFlags.Instance);

            var sut = new Impl.ValueSetter();

            sut.Set(propertyInfo, value, dummy).ShouldBe(isConverted);

            if (isConverted)
            {
                propertyInfo.GetValue(dummy).ShouldBe(expectedValue);
            }
        }

        [Test]
        public void Set_SetValueToDateTime_ValueIsSet()
        {
            var dummy = new DummyWithAllProperties();

            var propertyInfo = typeof(DummyWithAllProperties).GetProperty("DateTime", BindingFlags.Public | BindingFlags.Instance);

            var sut = new Impl.ValueSetter();

            sut.Set(propertyInfo, "2017-12-11", dummy).ShouldBe(true);

            propertyInfo.GetValue(dummy).ShouldBe(new DateTime(2017, 12, 11));
        }

        [Test]
        public void Set_SetValueTNullableoDateTime_ValueIsSet()
        {
            var dummy = new DummyWithAllProperties();

            var propertyInfo = typeof(DummyWithAllProperties).GetProperty("NullableDateTime", BindingFlags.Public | BindingFlags.Instance);

            var sut = new Impl.ValueSetter();

            sut.Set(propertyInfo, "2017-12-11", dummy).ShouldBe(true);

            DateTime? dateTime = null;
            dateTime = new DateTime(2017, 12, 11);

            propertyInfo.GetValue(dummy).ShouldBe(dateTime);
        }

        [Test]
        public void Set_SetValueDateTime_ValueIsSet()
        {
            var dummy = new DummyWithAllProperties();

            var propertyInfo = typeof(DummyWithAllProperties).GetProperty("DateTime", BindingFlags.Public | BindingFlags.Instance);

            var sut = new Impl.ValueSetter();

            sut.Set(propertyInfo, "2017-04-02", dummy).ShouldBeTrue();

            propertyInfo.GetValue(dummy).ShouldBe(new DateTime(2017, 04, 02));
        }

        [Test]
        public void Set_SetValueToGenericNotNullable_ValueIsNotSet()
        {
            var dummy = new DummyWithAllProperties();

            var propertyInfo = typeof(DummyWithAllProperties).GetProperty("Strings", BindingFlags.Public | BindingFlags.Instance);

            var sut = new Impl.ValueSetter();

            sut.Set(propertyInfo, null, dummy).ShouldBe(false);
        }
    }

    public class DummyWithAllProperties
    {
        public DateTime? NullableDateTime { get; set; }

        public DateTime DateTime { get; set; }

        public bool Boolean { get; set; }

        public byte Byte { get; set; }

        public sbyte SByte { get; set; }

        public char Char { get; set; }

        public decimal Decimal { get; set; }

        public double Double { get; set; }

        public float Float { get; set; }

        public int Int { get; set; }

        public uint UInt { get; set; }

        public long Long { get; set; }

        public ulong ULong { get; set; }

        public object Object { get; set; }

        public short Short { get; set; }

        public ushort UShort { get; set; }

        public string String { get; set; }

        public List<string> Strings { get; set; }
    }
}
