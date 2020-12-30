using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using FluentAssertions;
using Moq;

using Pipes;

namespace Tests
{
    [TestClass]
    public class AdaptorPipeShould
    {
        [TestMethod]
        public void Pipe_FromArray_ReturnsInstanceOfAdaptorPipe()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4, 5});

            pipe.Should().BeOfType(typeof(AdaptorPipe<int>));
        }

        [TestMethod]
        public void Count_NonEmptyArray_ReturnsElementCount()
        {
            var pipe = new AdaptorPipe<int>(new [] {1, 2, 3, 4, 5});

            pipe.Count().Should().Be(5);
        }

        [TestMethod]
        public void Count_EmptyArray_ReturnsZero()
        {
            var pipe = new AdaptorPipe<int>(new int[0]);

            pipe.Count().Should().Be(0);
        }

        [TestMethod]
        public void Count_ICollection_CallsICollectionCount()
        {
            var collection = GetMockCollection(new[] {1, 2, 3, 4});

            var pipe = new AdaptorPipe<int>(collection.Object);

            var count = pipe.Count();

            collection.VerifyGet(c => c.Count, Times.Once);
        }

        [TestMethod]
        public void GetEnumerator_ReturnsThis() {
            var pipe = new AdaptorPipe<string>(new [] {"a", "b", "c"});
            
            pipe.GetEnumerator().Should().Be(pipe);
        }

        [TestMethod]
        public void GetEnumerator_ReturnsCorrectElements() {
            var pipe = new AdaptorPipe<string>(new [] {"a", "b", "c"});

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
