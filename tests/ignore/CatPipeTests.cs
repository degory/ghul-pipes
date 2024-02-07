using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using FluentAssertions;
using Moq;

using Pipes;

namespace Tests
{
    [TestClass]
    public class CatPipeShould
    {

       [TestMethod]
        public void Cat_EmptySequences_ReturnsEmptySequence()
        {
            var pipe = Pipe.From(new int[0]).Cat(new int[0]);

            pipe
                .Count()
                .Should()
                .Be(0);
        }

        [TestMethod]
        public void Cat_EmptyPlusNonEmpty_ReturnsSecondSequence()
        {
            var pipe = Pipe.From(new int[0]).Cat(new [] {1, 2, 3, 4, 5});

            pipe
                .Should()
                .Equal(new []{ 1, 2, 3, 4, 5 });
        }

        [TestMethod]
        public void Cat_NonEmptyPlusEmpty_ReturnsFirstSequence()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5}).Cat(new int[0]);

            pipe
                .Should()
                .Equal(new []{ 1, 2, 3, 4, 5 });
        }


        [TestMethod]
        public void Cat_NonEmptyPlusNonEmpty_ReturnsBothSequencesInOrder()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5}).Cat(new [] {6, 7, 8, 9, 10});

            pipe
                .Should()
                .Equal(new []{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        }

        [TestMethod]
        public void Cat_ChainedCalls_ReturnsConcatenationOfAllInputs()
        {
            var pipe = Pipe.From(new [] {3, 4, 5});

            pipe
                .Cat(new [] {1, 2, 1})
                .Cat(new int[0])
                .Cat(new [] {2, 3, 4})
                .Cat(new [] {5})
                .Should()
                .Equal(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});
        }

        [TestMethod]
        public void Cat_ChainedCalls_CountReturnsSumOfAllCounts()
        {
            var pipe = Pipe.From(new [] {3, 4, 5});

            pipe
                .Cat(new [] {1, 2, 1})
                .Cat(new int[0])
                .Cat(new [] {2, 3, 4})
                .Cat(new [] {5})
                .Count()
                .Should()
                .Be(10);
        }

    }
}
