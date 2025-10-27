using System;
using Xunit;
using ConsoleApp1;

namespace ConsoleApp1.Tests
{
    public class FunctionalExtensionsTest
    {
        [Fact]
        public void Curry_BindsFirstArgument()
        {
            Func<int, int, int> add = (a, b) => a + b;
            var addFive = add.Curry(5);

            Assert.Equal(8, addFive(3));
            Assert.Equal(12, addFive(7));
        }

        [Fact]
        public void Into_Value_CapturesValueImmediately()
        {
            int value = 3;
            var doubled = value.Into(v => v * 2);

            value = 10; // changing original should not affect the captured value
            Assert.Equal(6, doubled());
        }

        [Fact]
        public void Into_Func_DefersEvaluation()
        {
            int value = 3;
            Func<int> valueProvider = () => value;
            var doubled = valueProvider.Into(v => v * 2);

            value = 10; // deferred evaluation should see updated value
            Assert.Equal(20, doubled());
        }

        [Fact]
        public void Memoize_CachesResultsPerArgument()
        {
            int calls = 0;
            Func<int, int> square = n =>
            {
                calls++;
                return n * n;
            };

            var memoized = square.Memoize();

            Assert.Equal(4, memoized(2));
            Assert.Equal(4, memoized(2)); // cached
            Assert.Equal(1, calls);

            Assert.Equal(9, memoized(3)); // new argument -> new computation
            Assert.Equal(2, calls);

            Assert.Equal(9, memoized(3)); // cached again
            Assert.Equal(2, calls);
        }
    }
}