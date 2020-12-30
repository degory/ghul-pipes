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
    }
}
