using System;
using System.Reflection;
using NUnit.Framework;
using Shouldly;

namespace Helpful.TextParser.Test.ValueSetter
{
    public class ValueSetterTest
    {
        [Test]
        [TestCase("Boolean", "True", true, true)]
        [TestCase("Byte", "254", true, 254)]
        [TestCase("SByte", "120", true, 120)]
        [TestCase("Char", "M", true, 'M')]
        public void Set_SetValue_ValueIsSet(string property, string value, bool isConverted, object expectedValue)
        {
            var dummy = new DummyWithAllProperties();

            var propertyInfo = typeof(DummyWithAllProperties).GetProperty(property, BindingFlags.Public | BindingFlags.Instance);

            var sut = new Impl.ValueSetter();

            sut.Set(propertyInfo, value, dummy).ShouldBe(isConverted);

            propertyInfo.GetValue(dummy).ShouldBe(expectedValue);
        }
    }

    public class DummyWithAllProperties
    {
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
    }
}
