using ConsoleApp1;
using Xunit;

namespace ConsoleApp1.Tests
{
    public class PersonTest
    {
        [Fact]
        public void Constructor_InitializesProperties()
        {
            var person = new Person("Doe", "Jane", 34, "jane.doe@example.com");

            Assert.Equal("Doe", person.Name);
            Assert.Equal("Jane", person.FirstName);
            Assert.Equal(34, person.Age);
            Assert.Equal("jane.doe@example.com", person.Email);
        }

        [Fact]
        public void ToString_WithEmail_ReturnsFormattedPerson()
        {
            var person = new Person("Doe", "Jane", 34, "jane.doe@example.com");

            var result = person.ToString();

            Assert.Equal("Name: Doe, FirstName: Jane, Age: 34, Email: jane.doe@example.com", result);
        }

        [Fact]
        public void ToString_WithoutEmail_UsesNotAvailableValue()
        {
            var person = new Person("Doe", "Jane", 34, null);

            var result = person.ToString();

            Assert.Equal("Name: Doe, FirstName: Jane, Age: 34, Email: N/A", result);
        }

        [Fact]
        public void ImplicitIntConversion_ReturnsAge()
        {
            var person = new Person("Doe", "Jane", 34, null);

            int age = person;

            Assert.Equal(34, age);
        }

        [Fact]
        public void ImplicitStringConversion_ReturnsFullName()
        {
            var person = new Person("Doe", "Jane", 34, null);

            string fullName = person;

            Assert.Equal("Doe Jane", fullName);
        }

        [Theory]
        [InlineData(-1, "Child")]
        [InlineData(12, "Child")]
        [InlineData(13, "Teenager")]
        [InlineData(19, "Teenager")]
        [InlineData(20, "Adult")]
        [InlineData(64, "Adult")]
        [InlineData(65, "Senior")]
        [InlineData(90, "Senior")]
        public void Category_ForAge_ReturnsExpectedCategory(int age, string expectedCategory)
        {
            var person = new Person("Doe", "Jane", age, null);

            var result = person.Category();

            Assert.Equal(expectedCategory, result);
        }
    }
}