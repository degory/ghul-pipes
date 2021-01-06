using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using FluentAssertions;
using Moq;

using Pipes;

namespace Tests
{
    [TestClass]
    public class TakePipeShould
    {

       [TestMethod]
        public void Take_NegativeTakeCount_ReturnsEmptySequence()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Take(-27)
                .Should()
                .Equal(new int[0]);
        }

        [TestMethod]
        public void Take_ZeroTakeCount_ReturnsEmptySequence()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Take(0)
                .Should()
                .Equal(new int[0]);
        }

        [TestMethod]
        public void Take_TakeCountEqualToLength_ReturnsWholeInput()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Take(10)
                .Should()
                .Equal(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});
        }

        [TestMethod]
        public void Take_TakeCountOneGreaterThanLength_ReturnsWholeInput()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Take(11)
                .Should()
                .Equal(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});
        }

        [TestMethod]
        public void Take_TakeCountSomewhatGreaterThanLength_ReturnsWholeInput()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Take(15)
                .Should()
                .Equal(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});
        }

        [TestMethod]
        public void Take_TakeCountMuchGreaterThanLength_ReturnsWholeInput()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Take(1000)
                .Should()
                .Equal(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});
        }

        [TestMethod]
        public void Take_TakeOne_ReturnsFirstElementAsSequence()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Take(1)
                .Should()
                .Equal(new [] {3});
        }

        [TestMethod]
        public void Take_TakeMultiple_ReturnsCorrectHeadSequence()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Take(4)
                .Should()
                .Equal(new [] {3, 4, 5, 1});
        }

        [TestMethod]
        public void Take_TakeOneLessThanLength_ReturnsCorrectHeadSequence()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Take(9)
                .Should()
                .Equal(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4});
        }

        [TestMethod]
        public void Take_ChainedCalls_TakesMinimumOfTakeCountsElements()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Take(7)
                .Take(2)
                .Take(4)
                .Should()
                .Equal(new [] {3, 4});
        }

        [TestMethod]
        public void Take_ChainedCallsWithCountGreaterThanInputLength_TakesMiniumnOfTakeCountsElements()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Take(7)
                .Take(27)
                .Take(5)
                .Take(1000)
                .Should()
                .Equal(new [] {3, 4, 5, 1, 2});
        }
 
        [TestMethod]
        public void Take_TakeAfterPriorEnumerate_PipeIsReset()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Take(7)
                .Collect();

            pipe
                .Take(6)
                .Collect();

            pipe
                .Take(5)
                .Should()
                .Equal(new [] { 3, 4, 5, 1, 2 });
        }
    }
}
