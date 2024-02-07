using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using FluentAssertions;
using Moq;

using Pipes;

namespace Tests
{
    [TestClass]
    public class MapPipeShould
    {
        [TestMethod]
        public void Map_EmptyArray_ReturnsEmptySequence()
        {
            var pipe = Pipe.From(new int[0]);

            pipe
                .Map(i => i.ToString())
                .Should()
                .Equal(new string[0]);
        }

        [TestMethod]
        public void Map_IntsToStrings_ReturnsExpectedSequence()
        {
            var pipe = Pipe.From(new [] {3, 4, 1, 5, 2, 3, 1, 4, 5});

            pipe
                .Map(i => i * 2)
                .Should()
                .Equal(new [] {6, 8, 2, 10, 4, 6, 2, 8, 10});
        }

        [TestMethod]
        public void Map_CalledTwice_AppliesBothFunctionsToEveryElementInCorrectOrder()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6});

            pipe
                .Map(i => i + "-first")
                .Map(i => i + "-second")
                .Should()
                .Equal(new [] { "1-first-second", "2-first-second", "3-first-second", "4-first-second", "5-first-second", "6-first-second" });
        }
    }
}
