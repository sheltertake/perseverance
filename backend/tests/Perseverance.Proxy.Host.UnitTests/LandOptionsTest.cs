using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;
using Perseverance.Proxy.Host.Models;

namespace Perseverance.Proxy.Host.UnitTests
{
    [ExcludeFromCodeCoverage]
    class LandOptionsTest
    {
        [Test]
        public void RandomObstaclesTest()
        {
            var options = new LandOptions();
            var randomObstacles = options.RandomObstacles();
            randomObstacles.Should().HaveCount(options.O);

            var options2 = new LandOptions()
            {
                O = 253,
                X = 50,
                Y = 50,
                H = 254,
                W = 254
            };
            var randomObstacles2 = options2.RandomObstacles();
            randomObstacles2.Should().HaveCount(options2.O);
        }
    }
}
