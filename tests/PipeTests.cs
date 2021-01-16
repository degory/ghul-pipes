using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;

using Pipes;

namespace Tests
{
    [TestClass]
    public class PipeShould
    {
    
        private static IndexedValue<int> ivi(int index, int value) => new IndexedValue<int>(index, value);
        [TestMethod]
        public void Pipe_SkipThenFirst_ReturnsCorrectElement()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10});

            pipe
                .Skip(5)
                .First()
                .Should()
                .Be(Maybe.From(6));
        }

        [TestMethod]
        public void Pipe_TakeThenFirst_ReturnsFirstElement()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10});

            pipe
                .Take(5)
                .First()
                .Should()
                .Be(Maybe.From(1));
        }

        [TestMethod]
        public void Pipe_FirstEmptySequence_ReturnsMaybeNot()
        {
            var pipe = Pipe.From(new int[0]);

            pipe
                .First()
                .Should()
                .Be(default(Maybe<int>));
        }

        [TestMethod]
        public void Pipe_FindEmptySequence_ReturnsMaybeNot()
        {
            var pipe = Pipe.From(new int[0]);

            pipe
                .Find(i => i > 10)
                .Should()
                .Be(default(Maybe<int>));
        }

        [TestMethod]
        public void Pipe_FindNonEmptySequenceButNoMatch_ReturnsMaybeNot()
        {
            var pipe = Pipe.From(new [] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });

            pipe
                .Find(i => i == 10)
                .Should()
                .Be(default(Maybe<int>));
        }

        [TestMethod]
        public void Pipe_FindSingleMatchingElement_ReturnsThatElement()
        {
            var pipe = Pipe.From(new [] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });

            pipe
                .Find(i => i == 4)
                .Should()
                .Be(Maybe.From(4));
        }

        private class Thing {
            public int Key;
            public Thing(int key) {
                Key = key;
            }
        }

        [TestMethod]
        public void Pipe_FindMultipleMatchingElement_ReturnsFirstMatchingElement()
        {
            var array = new [] { new Thing(1), new Thing(2), new Thing(3), new Thing(4), new Thing(3), new Thing(2), new Thing(3), new Thing(8), new Thing(9) };

            var pipe = Pipe.From(array);

            pipe
                .Find(t => t.Key == 3)
                .Should()
                .Be(Maybe.From(array[2]));
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
        public void Pipe_SkipThenIndex_StartsIndexingFromZero()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10});

            pipe
                .Skip(4)
                .Index()
                .Should()
                .Equal(new [] { ivi(0, 5), ivi(1, 6), ivi(2, 7), ivi(3, 8), ivi(4, 9), ivi(5, 10) });
        }

        [TestMethod]
        public void Pipe_IndexThenSkip_StartsIndexingFromCorrectOffset()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10});

            pipe
                .Index()
                .Skip(6)
                .Should()
                .Equal(new [] { ivi(6, 7), ivi(7, 8), ivi(8, 9), ivi(9, 10) });
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
        public void Pipe_IndexThenFilter_RetainsOriginalIndexes()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10});

            pipe
                .Index()
                .Filter(i => (i.Value & 1) == 1)
                .Should()
                .Equal(new [] { ivi(0,1), ivi(2,3), ivi(4,5), ivi(6,7), ivi(8,9) });
        }

        [TestMethod]
        public void Pipe_FilterThenIndex_AppliesNewIndexes()
        {
            var pipe = Pipe.From(new [] {3, 4, 1, 5, 2, 3, 1, 4, 5});

            pipe
                .Filter(i => i > 2)
                .Index()
                .Should()
                .Equal(new [] { ivi(0,3), ivi(1,4), ivi(2,5), ivi(3,3), ivi(4,4), ivi(5,5) });
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
        public void Pipe_TakeThenIndex_StartsIndexingFromZero()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10});

            pipe
                .Take(4)
                .Index()
                .Should()
                .Equal(new [] { ivi(0, 1), ivi(1, 2), ivi(2, 3), ivi(3, 4) });
        }

        [TestMethod]
        public void Pipe_IndexThenTake_StartsIndexingFromZero()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10});

            pipe
                .Index()
                .Take(6)
                .Should()
                .Equal(new [] { ivi(0, 1), ivi(1, 2), ivi(2, 3), ivi(3, 4), ivi(4, 5), ivi(5, 6) });
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
        public void Pipe_Running()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6});

            pipe
                .Reduce(0, (running, element) => running + element)
                .Should()
                .Be(1 + 2 + 3 + 4 + 5 + 6);
        }

        [TestMethod]
        public void Pipe_Mapped_Running()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5, 6});

            pipe
                .Reduce(0, (running, element) => running + element, total => "total: " + total)
                .Should()
                .Be("total: " + (1 + 2 + 3 + 4 + 5 + 6));
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

                [TestMethod]
        public void AppendTo_WithNoSeparatorArgument_AppendsStringRepresentationOfAllElementsSeparatedWithACommaAndSpace()
        {
            var pipe = Pipe.From(new [] {2, 1, 4, 3, 6, 5});

            var result = new System.Text.StringBuilder("XX");

            pipe
                .AppendTo(result);

            result
                .ToString()
                .Should()
                .Be("XX2, 1, 4, 3, 6, 5");
        }

        [TestMethod]
        public void AppendTo_WithSeparatorArgument_AppendsStringRepresentationOfAllElementsSeparatedWithThatSeparator()
        {
            var pipe = Pipe.From(new [] {2, 1, 4, 3, 6, 5});

            var result = new System.Text.StringBuilder("XX");

            pipe
                .AppendTo(result, "///");

            result
                .ToString()
                .Should()
                .Be("XX2///1///4///3///6///5");
        }

        [TestMethod]
        public void AppendTo_CalledTwice_AppendsStringRepresentationOfAllElementsTwice()
        {
            var pipe = Pipe.From(new [] {2, 1, 4, 3, 6, 5});

            var result = new System.Text.StringBuilder("XX");

            pipe
                .AppendTo(result, "-");

            result.Append("YY");

            pipe
                .AppendTo(result, "_");

            result
                .ToString()
                .Should()
                .Be("XX2-1-4-3-6-5YY2_1_4_3_6_5");
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
