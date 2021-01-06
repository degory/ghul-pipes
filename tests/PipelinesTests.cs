using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;

using Pipes;

namespace Tests
{
    [TestClass]
    public class PipelinesShould
    {
        [TestMethod]
        public void Pipe_SkipThenFirst_ReturnsCorrectElement()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10});

            pipe
                .Skip(5)
                .First()
                .Should()
                .Be(6);
        }

        [TestMethod]
        public void Pipe_TakeThenFirst_ReturnsFirstElement()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10});

            pipe
                .Take(5)
                .First()
                .Should()
                .Be(1);
        }

        [TestMethod]
        public void Pipe_SkipThenFilter_ReturnsFilteredTailSequence()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10});

            pipe
                .Skip(5)
                .Filter(i => (i & 1) == 0)
                .Should()
                .Equal(new [] { 6, 8, 10 });
        }

        [TestMethod]
        public void Pipe_FilterThenSkip_ReturnsFilteredTailSequence()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10});

            pipe
                .Filter(i => (i & 1) == 0)
                .Skip(1)
                .Should()
                .Equal(new [] { 4, 6, 8, 10 });
        }

        [TestMethod]
        public void Pipe_TakeThenFilter_ReturnsFilteredHeadSequence()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10});

            pipe
                .Take(5)
                .Filter(i => (i & 1) == 0)
                .Should()
                .Equal(new [] { 2, 4 });
        }

        [TestMethod]
        public void Pipe_FilterThenTake_ReturnsFilteredHeadSequence()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10});

            pipe
                .Filter(i => (i & 1) == 0)
                .Take(3)
                .Should()
                .Equal(new [] { 2, 4, 6 });
        }

        [TestMethod]
        public void Pipe_SkipThenMap_ReturnsMappedTailSequence()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10});

            pipe
                .Skip(5)
                .Map(i => "X" + i)
                .Should()
                .Equal(new [] { "X6", "X7", "X8", "X9", "X10" });
        }

        [TestMethod]
        public void Pipe_MapThenSkip_ReturnsMappedTailSequence()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10});

            pipe
                .Map(i => "X" + i)
                .Skip(5)
                .Should()
                .Equal(new [] { "X6", "X7", "X8", "X9", "X10" });
        }

        [TestMethod]
        public void Pipe_TakeThenMap_ReturnsMappedHeadSequence()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10});

            pipe
                .Take(4)
                .Map(i => "X" + i)
                .Should()
                .Equal(new [] { "X1", "X2", "X3", "X4" });
        }

        [TestMethod]
        public void Pipe_MapThenTake_ReturnsMappedHeadSequence()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10});

            pipe
                .Map(i => "X" + i)
                .Take(6)
                .Should()
                .Equal(new [] { "X1", "X2", "X3", "X4", "X5", "X6" });
        }
    }
}
