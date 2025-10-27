using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ConsoleApp1.Tests
{
    public class IEnumerableExtensionsTest
    {
        [Fact]
        public void Indexed_EnumeratesWithIndices()
        {
            var input = new[] { "a", "b", "c" };
            var expected = new List<(int index, string value)> { (0, "a"), (1, "b"), (2, "c") };
            var actual = input.Indexed().ToList();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Indexed_EmptySequence_ReturnsEmpty()
        {
            var input = Array.Empty<int>();
            var actual = input.Indexed().ToList();
            Assert.Empty(actual);
        }

        [Fact]
        public void SeparatedBy_InsertsSeparatorBetweenItems()
        {
            var input = new[] { "a", "b", "c" };
            var expected = new[] { "a", "-", "b", "-", "c" };
            var actual = input.SeparatedBy("-").ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SeparatedBy_SingleItem_ReturnsSingle()
        {
            var input = new[] { "only" };
            var actual = input.SeparatedBy("-").ToArray();
            Assert.Single(actual);
            Assert.Equal("only", actual[0]);
        }

        [Fact]
        public void SeparatedBy_EmptySequence_ReturnsEmpty()
        {
            var input = Array.Empty<string>();
            var actual = input.SeparatedBy("-").ToArray();
            Assert.Empty(actual);
        }

        [Fact]
        public void SeparatedBy_WorksWithValueTypes()
        {
            var input = new[] { 1, 2, 3 };
            var expected = new[] { 1, 0, 2, 0, 3 };
            var actual = input.SeparatedBy(0).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SeparatedBy_AllowsNullSeparator()
        {
            string? sep = null;
            var input = new[] { "x", "y" };
            var expected = new string?[] { "x", null, "y" };
            var actual = input.SeparatedBy(sep).ToArray();
            Assert.Equal(expected, actual);
        }
    

        [Fact]
        public void LeftJoin_MatchesItemsCorrectly()
        {
            var left = new[] { 1, 2, 3 };
            var right = new[] { "one", "two", "three" };
            Func<int, string, bool> predicate = (i, s) => (i == 1 && s == "one") || (i == 2 && s == "two") || (i == 3 && s == "three");

            var result = left.LeftJoin<int, string, object>(right, predicate).ToList();

            Assert.Equal((1, "one"), result[0]);
            Assert.Equal((2, "two"), result[1]);
            Assert.Equal((3, "three"), result[2]);
        }

        [Fact]
        public void LeftJoin_NoMatches_ReturnsDefaultForRight()
        {
            var left = new[] { 1, 2 };
            var right = new[] { "a", "b" };
            Func<int, string, bool> predicate = (i, s) => false;

            var result = left.LeftJoin<int, string, object>(right, predicate).ToList();

            Assert.Equal((1, null), result[0]);
            Assert.Equal((2, null), result[1]);
        }

        [Fact]
        public void LeftJoin_PartialMatches_ReturnsMatchedOrDefault()
        {
            var left = new[] { 1, 2, 3 };
            var right = new[] { "a", "b", "c" };
            Func<int, string, bool> predicate = (i, s) => (i == 2 && s == "b");

            var result = left.LeftJoin<int, string, object>(right, predicate).ToList();

            Assert.Equal((1, null), result[0]);
            Assert.Equal((2, "b"), result[1]);
            Assert.Equal((3, null), result[2]);
        }

        [Fact]
        public void LeftJoin_EmptyLeft_ReturnsEmpty()
        {
            var left = Array.Empty<int>();
            var right = new[] { "a", "b" };
            Func<int, string, bool> predicate = (i, s) => true;

            var result = left.LeftJoin<int, string, object>(right, predicate).ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void LeftJoin_EmptyRight_ReturnsDefaultForAll()
        {
            var left = new[] { 1, 2 };
            var right = Array.Empty<string>();
            Func<int, string, bool> predicate = (i, s) => true;

            var result = left.LeftJoin<int, string, object>(right, predicate).ToList();

            Assert.Equal((1, null), result[0]);
            Assert.Equal((2, null), result[1]);
        }
    }
}