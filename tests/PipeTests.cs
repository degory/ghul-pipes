using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;

using Pipes;

namespace Tests
{
    [TestClass]
    public class PipeShould
    {
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
        public void ToString_WithNoSeparatorArgument_ReturnsStringRepresentationOfAllElementsSeparatedWithACommaAndSpace()
        {
            var pipe = Pipe.From(new [] {2, 1, 4, 3, 6, 5});

            pipe
                .ToString()
                .Should()
                .Be("2, 1, 4, 3, 6, 5");
        }

        [TestMethod]
        public void ToString_WithSeparatorArgument_ReturnsStringRepresentationOfAllElementsSeparatedWithThatSeparator()
        {
            var pipe = Pipe.From(new [] {2, 1, 4, 3, 6, 5});

            pipe
                .ToString("///")
                .Should()
                .Be("2///1///4///3///6///5");
        }

        [TestMethod]
        public void ToString_CalledTwice_ReturnsStringRepresentationOfAllElements()
        {
            var pipe = Pipe.From(new [] {2, 1, 4, 3, 6, 5});

            pipe
                .ToString();

            pipe
                .ToString("\t")
                .Should()
                .Be("2\t1\t4\t3\t6\t5");
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
