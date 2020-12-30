using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using FluentAssertions;
using Moq;

using Pipes;

namespace Tests
{
    [TestClass]
    public class SkipPipeShould
    {
        [TestMethod]
        public void Skip_NegativeSkipCount_ReturnsWholeInput()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Skip(-27)
                .Should()
                .Equal(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});
        }

        [TestMethod]
        public void Skip_ZeroSkipCount_ReturnsWholeInput()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Skip(0)
                .Should()
                .Equal(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});
        }

        [TestMethod]
        public void Skip_SkipCountEqualToLength_ReturnsEmptySequence()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Skip(10)
                .Should()
                .Equal(new int[0]);
        }

        [TestMethod]
        public void Skip_SkipCountGreaterThanLength_ReturnsEmptySequence()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Skip(1000)
                .Should()
                .Equal(new int[0]);
        }

        [TestMethod]
        public void Skip_SkipOne_ReturnsCorrectTailSequence()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Skip(1)
                .Should()
                .Equal(new [] {4, 5, 1, 2, 1, 2, 3, 4, 5});
        }

        [TestMethod]
        public void Skip_SkipMultiple_ReturnsCorrectTailSequence()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Skip(4)
                .Should()
                .Equal(new [] {2, 1, 2, 3, 4, 5});
        }

        [TestMethod]
        public void Skip_SkipOneLessThanLength_ReturnsLastElement()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Skip(9)
                .Should()
                .Equal(new [] {5});
        }

        [TestMethod]
        public void Skip_ChainedCalls_SkipSumOfSkipCountsElements()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Skip(1)
                .Skip(2)
                .Skip(3)
                .Should()
                .Equal(new [] {2, 3, 4, 5});
        }

        [TestMethod]
        public void Skip_ChainedCallsWithCountGreaterThanInputLength_ReturnEmptySequence()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Skip(7)
                .Skip(6)
                .Skip(5)
                .Should()
                .Equal(new int[0]);
        }

        [TestMethod]
        public void Skip_SkipAfterPriorEnumerate_PipeIsReset()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Skip(7);

            // expect pipe reset

            pipe
                .Skip(6);

            // expect pipe reset - skipping 5 elements still leaves 5 elements to consume:

            pipe
                .Skip(5)
                .Should()
                .Equal(new [] { 1, 2, 3, 4, 5 });
        }
    }
}
