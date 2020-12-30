using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using FluentAssertions;
using Moq;

using Pipes;

namespace Tests
{
    [TestClass]
    public class FilterPipeShould
    {
        [TestMethod]
        public void Filter_ConsecutiveNonMatchingValues_AreRemoved()
        {
            var pipe = Pipe.From(new [] {3, 4, 5, 1, 2, 1, 2, 3, 4, 5});

            pipe
                .Filter(i => i > 2)
                .Should()
                .Equal(new [] {3, 4, 5, 3, 4, 5});
        }

        [TestMethod]
        public void Filter_NonConsecutiveNonMatchingValues_AreRemoved()
        {
            var pipe = Pipe.From(new [] {3, 4, 1, 5, 2, 3, 1, 4, 5});

            pipe
                .Filter(i => i > 2)
                .Should()
                .Equal(new [] {3, 4, 5, 3, 4, 5});
        }


        [TestMethod]
        public void Filter_LeadingNonMatchingValues_AreRemoved()
        {
            var pipe = Pipe.From(new [] {1, 2, 1, 3, 4, 5, 6, 7});

            pipe
                .Filter(i => i > 2)
                .Should()
                .Equal(new [] {3, 4, 5, 6, 7});
        }

        [TestMethod]
        public void Filter_TrailingNonMatchingValues_AreRemoved()
        {
            var pipe = Pipe.From(new [] {7, 6, 5, 4, 3, 2, 1});

            pipe
                .Filter(i => i > 2)
                .Should()
                .Equal(new [] {7, 6, 5, 4, 3});
        }

        [TestMethod]
        public void Count_AlwaysTruePredicate_ReturnsElementCount()
        {
            var input = new [] {"a", "b", "c", "d"};
            var pipe = new FilterPipe<string>(((IEnumerable<string>)input).GetEnumerator(), i => true);

            pipe.Count().Should().Be(4);
        }

        [TestMethod]
        public void Count_EmptyList_ReturnsZero()
        {
            var pipe = new FilterPipe<int>(new List<int>().GetEnumerator(), i => true);

            pipe.Count().Should().Be(0);
        }

        [TestMethod]
        public void GetEnumerator_ReturnsThis() {
            var input = new [] {"a", "b", "c"};
            var pipe = new FilterPipe<string>(((IEnumerable<string>)input).GetEnumerator(), i => true);
            
            pipe.GetEnumerator().Should().Be(pipe);
        }

        [TestMethod]
        public void GetEnumerator_AlwaysTruePredicate_ResultEqualsInput() {
            var input = new [] {"a", "b", "c"};
            var pipe = new FilterPipe<string>(((IEnumerable<string>)input).GetEnumerator(), i => true);

            pipe.Should().Equal(new [] {"a", "b", "c"});
            Assert.AreSame(pipe, pipe.GetEnumerator());
        }

        private Mock<ICollection<T>> GetMockCollection<T>(IEnumerable<T> enumerable) {
            var collection = new Moq.Mock<ICollection<T>>();

            collection.Setup(c => c.GetEnumerator()).Returns(() => enumerable.GetEnumerator());

            return collection;
        }
    }
}
