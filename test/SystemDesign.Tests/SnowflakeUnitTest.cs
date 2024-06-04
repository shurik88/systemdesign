using FluentAssertions;

namespace SystemDesign.Tests
{
    public class SnowflakeUnitTest
    {
        [Fact]
        public void SnowflakeGeneratorId()
        {
            var generator = new SnowflakeGenerator.SnowflakeGenerator(1, 1);

            var id1 = generator.GetId();
            var id2 = generator.GetId();
            id2.Should().BeGreaterThan(id1, "id1 is older than id2");

            var source = Enumerable.Range(1, 10000).Select(x => generator.GetId());
            var ids = source.AsParallel().ToList();

            ids.Should().OnlyHaveUniqueItems("generator should be thread safe");
        }
    }
}