using System;
using System.Collections.Generic;
using System.Linq;
using Exebite.Converters.Attributes;
using Xunit;

namespace Exebite.Converters.Test
{
    public class ValueSepartedConverterTests
    {
        private readonly IEnumerable<TestClass> _testClasses;

        public ValueSepartedConverterTests()
        {
            _testClasses = Enumerable.Range(1, int.MaxValue).Select(content => new TestClass
            {
                Name = "Test " + content,
                IntValue = content,
                ShortValue = (short)content,
                LongValue = content,
                Date = DateTime.Now,
                BoolValueToInt = true
            });
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Serialize_ElementsInList_HeaderAndBodyExists(int numberOfBodyElements)
        {
            // Arrange
            var sut = new ValueSepartedConverter();
            var expectedSplitCharacters = (typeof(TestClass).GetProperties().Length * (numberOfBodyElements + 1)) - numberOfBodyElements;

            // Act
            var result = sut.Serialize(_testClasses.Take(numberOfBodyElements));

            // Assert
            Assert.Equal(numberOfBodyElements + 2, result.Split("\r\n").Length);
            Assert.Equal(expectedSplitCharacters, result.Split(";").Length);
        }

        [Fact]
        public void Serialize_HeaderNames_FromPropertyAndHeaderAttribute()
        {
            // Arrange
            var sut = new ValueSepartedConverter();

            // Act
            var result = sut.Serialize(_testClasses.Take(1));

            // Assert
            Assert.Contains("Name", result);
            Assert.Contains("Int Value", result);
        }

        [Fact]
        public void Serialize_NoHeaderNames_OnlyPropertyNames()
        {
            // Arrange
            var sut = new ValueSepartedConverter();

            // Act
            var result = sut.Serialize(new List<NoHeaderData> { new NoHeaderData { NoHeader = "test" } });

            // Assert
            Assert.Contains("NoHeader", result);
        }

        [Fact]
        public void Serialize_NoPropertiesInClass_EmptyResult()
        {
            // Arrange
            var sut = new ValueSepartedConverter();

            // Act
            var result = sut.Serialize(new List<NoProperty>());

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Deserialize_TextHasNoValidText_EmptyResult()
        {
            // Arrange
            var sut = new ValueSepartedConverter();

            // Act
            var result = sut.Deserialize<NoProperty>(new[] { "NoHeader" });

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Deserialize_TextHasNoValidText_NoPropertyClassMapped()
        {
            // Arrange
            var sut = new ValueSepartedConverter();

            // Act
            var result = sut.Deserialize<NoProperty>(new[] { "Header", "Body, part 2" });

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void Deserialize_TextHasValidText_PropertyMapped()
        {
            // Arrange
            var sut = new ValueSepartedConverter();
            var expectedValues = new TestClass
            {
                Name = "body name",
                IntValue = 1,
                ShortValue = 2,
                LongValue = 3,
                Date = DateTime.Parse("2018-12-22"),
                BoolValueToInt = true,
                DoubleValue = 3.2,
                FloatValue = 5
            };
            // Act
            var result = sut.Deserialize<TestClass>(new[]
            {
                "Name,IntValue,ShortValue,LongValue,Date,BoolValueToInt,DoubleValue,FloatValue",
                $"{expectedValues.Name}, {expectedValues.IntValue}, {expectedValues.ShortValue}," +
                $"{expectedValues.LongValue}, {expectedValues.Date}, {expectedValues.BoolValueToInt}," +
                $" {expectedValues.DoubleValue}, {expectedValues.FloatValue}"
            });

            // Assert
            Assert.Single(result);
            Assert.Equal(expectedValues.Name, result.ElementAt(0).Name);
            Assert.Equal(expectedValues.IntValue, result.ElementAt(0).IntValue);
            Assert.Equal(expectedValues.ShortValue, result.ElementAt(0).ShortValue);
            Assert.Equal(expectedValues.LongValue, result.ElementAt(0).LongValue);
            Assert.Equal(expectedValues.Date, result.ElementAt(0).Date);
            Assert.Equal(expectedValues.BoolValueToInt, result.ElementAt(0).BoolValueToInt);
            Assert.Equal(expectedValues.DoubleValue, result.ElementAt(0).DoubleValue);
            Assert.Equal(expectedValues.FloatValue, result.ElementAt(0).FloatValue);
        }

        [Fact]
        public void Deserialize_TextHasEmptyStringTypeValue_PropertyMappedToDefault()
        {
            // Arrange
            var sut = new ValueSepartedConverter();
            var expectedValues = new TestClass
            {
                Name = "1",
                IntValue = 0,
                ShortValue = 0,
                LongValue = 0,
                Date = DateTime.Parse("0001-01-01"),
                BoolValueToInt = false,
                DoubleValue = 0,
                FloatValue = 0
            };
            // Act
            var result = sut.Deserialize<TestClass>(new[]
            {
                "Name,IntValue,ShortValue,LongValue,Date,BoolValueToInt,DoubleValue,FloatValue",
                "1,IntValue,ShortValue,LongValue,Date,BoolValueToInt,DoubleValue,FloatValue"
            });

            // Assert
            Assert.Single(result);
            Assert.Equal(expectedValues.Name, result.ElementAt(0).Name);
            Assert.Equal(expectedValues.IntValue, result.ElementAt(0).IntValue);
            Assert.Equal(expectedValues.ShortValue, result.ElementAt(0).ShortValue);
            Assert.Equal(expectedValues.LongValue, result.ElementAt(0).LongValue);
            Assert.Equal(expectedValues.Date, result.ElementAt(0).Date);
            Assert.Equal(expectedValues.BoolValueToInt, result.ElementAt(0).BoolValueToInt);
            Assert.Equal(expectedValues.DoubleValue, result.ElementAt(0).DoubleValue);
            Assert.Equal(expectedValues.FloatValue, result.ElementAt(0).FloatValue);
        }

        [Fact]
        public void Deserialize_TextHasWrongBodyValueTypes_PropertyMappedToNull()
        {
            // Arrange
            var sut = new ValueSepartedConverter();
            var expectedValues = new TestClass
            {
                Name = null,
            };

            // Act
            var result = sut.Deserialize<TestClass>(new[]
            {
                "Name",
                ""
            });

            // Assert
            Assert.Single(result);
            Assert.Equal(expectedValues.Name, result.ElementAt(0).Name);
        }

        private class NoProperty { }

        private class NoHeaderData
        {
            public string NoHeader { get; set; }
        }

        private class TestClass
        {
            public string Name { get; set; }

            [Header(Name = "Int Value")]
            public int IntValue { get; set; }

            public short ShortValue { get; set; }

            public long LongValue { get; set; }

            public double DoubleValue { get; set; }

            public float FloatValue { get; set; }

            [Format(Format = "YYYY-DD-MM")]
            public DateTime Date { get; set; }

            [BoolToInt]
            public bool BoolValueToInt { get; set; }
        }
    }
}
