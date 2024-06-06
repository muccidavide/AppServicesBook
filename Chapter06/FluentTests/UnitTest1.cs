using FluentAssertions;
using FluentAssertions.Extensions;

namespace FluentTests
{
    public class FluentExamples
    {
        [Fact]
        public void TestString()
        {
            string city = "London";
            string expectedCity = "London";

            city.Should().StartWith("Lo").And.EndWith("on").And.Contain("nd").And.HaveLength(6);
            city.Should().NotBeNull().And.Be("London").And.BeSameAs(expectedCity).And.BeOfType<string>();

            city.Length.Should().Be(6);
        }

        [Fact]
        public void TestsCollections()
        {
            string[] names = { "Alice", "Bob", "Charlie" };

            names.Should().OnlyContain(name => name.Length <= 8);


        }

        [Fact]
        public void TestsDateTimes()
        {
            DateTime when = new(2024, 3, 25, 9, 30, 0);
            when.Should().Be(25.March(2024).At(9, 30));
            when.Should().BeOnOrAfter(23.March(2024));
            when.Should().NotBeSameDateAs(12.March(1999));
            when.Should().HaveYear(2024);

            DateTime due = new(2024, 3, 25, 11, 30, 0);

            when.Should().BeAtLeast(2.Hours()).Before(due);
        }
    }
}