using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;

using Pipes;

namespace Tests
{
    [TestClass]
    public class PipeShould
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

        [TestMethod]
        public void Pipe_FilterThenFilter_ReturnsOnlyElementsThatMatchBothPredicates()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10});

            pipe
                .Filter(i => i > 3 && i < 9)
                .Filter(i => (i & 1) == 0)
                .Should()
                .Equal(new [] { 4, 6, 8 });
        }

        [TestMethod]
        public void Pipe_MapThenMap_AppliesBothFunctionsToEveryElementInCorrectOrder()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6});

            pipe
                .Map(i => i + "-first")
                .Map(i => i + "-second")
                .Should()
                .Equal(new [] { "1-first-second", "2-first-second", "3-first-second", "4-first-second", "5-first-second", "6-first-second" });
        }

        [TestMethod]
        public void Sort_NoComparer_SortsInDefaultOrder()
        {
            var pipe = Pipe.From(new [] {2, 1, 4, 3, 6, 5});

            pipe
                .Sort()
                .Should()
                .Equal(new [] { 1, 2, 3, 4, 5, 6 });
        }

        [TestMethod]
        public void Sort_GivenComparer_SortsInComparerOrder()
        {
            var pipe = Pipe.From(new [] {2, 1, 4, 3, 6, 5});

            pipe
                .Sort(new ReverseIntComparer())
                .Should()
                .Equal(new [] { 6, 5, 4, 3, 2, 1 });
        }

        [TestMethod]
        public void Sort_GivenCompareFunction_SortsInCompareFunctionOrder()
        {
            var pipe = Pipe.From(new [] {2, 1, 4, 3, 6, 5});

            pipe
                .Sort((int x, int y) => y - x)
                .Should()
                .Equal(new [] { 6, 5, 4, 3, 2, 1 });
        }

        [TestMethod]
        public void Sort_CalledTwice_SortsInLastComparerOrder()
        {
            var pipe = Pipe.From(new [] {2, 1, 4, 3, 6, 5});

            pipe
                .Sort()
                .Sort(new ReverseIntComparer())
                .Should()
                .Equal(new [] { 6, 5, 4, 3, 2, 1 });
        }

        [TestMethod]
        public void Sort_EnumeratedTwice_ReturnsSameResultBothTimes()
        {
            var pipe = Pipe.From(new [] {2, 1, 4, 3, 6, 5});

            var sorted = 
                pipe
                    .Sort(new ReverseIntComparer());

            sorted.Collect();

            sorted
                .Should()
                .Equal(new [] { 6, 5, 4, 3, 2, 1 });            
        }

        [TestMethod]
        public void Join_CalledTwice_JoinsAllElements()
        {
            var pipe = Pipe.From(new [] {2, 1, 4, 3, 6, 5});

            pipe
                .Join();

            pipe
                .Join(", ")
                .Should()
                .Be("2, 1, 4, 3, 6, 5");
        }


        class ReverseIntComparer: System.Collections.Generic.IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return y - x;
            }
        }
    }
}
