using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using FluentAssertions;
using Moq;

using Pipes;

namespace Tests
{
    [TestClass]
    public class ZipMapPipeShould
    {
        private static IndexedValue<string> ivs(int index, string value) => new IndexedValue<string>(index, value);

        [TestMethod]
        public void ZipMap_EmptyLeftArray_ReturnsEmptySequence()
        {
            var pipe = Pipe.From(new int[0]);

            pipe
                .Zip(new [] {1, 2, 3, 4, 5}, (l, r) => (l + r))
                .Should()
                .Equal(new int[0]);
        }

        [TestMethod]
        public void ZipMap_EmptyRightArray_ReturnsEmptySequence()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4});

            pipe
                .Zip(new int[0], (l, r) => (l + r))
                .Should()
                .Equal(new int[0]);
        }

        [TestMethod]
        public void ZipMap_EmptyLeftAndRightArrays_ReturnsEmptySequence()
        {
            var pipe = Pipe.From(new int[0]);

            pipe
                .Zip(new int[0], (l, r) => (l + r))
                .Should()
                .Equal(new int[0]);
        }

        [TestMethod]
        public void ZipMap_ShorterLeftArray_ReturnsZipWithAllLeftAndCorrespondingRightElements()
        {
            var pipe = Pipe.From(new [] {"3", "4", "1", "5"});

            pipe
                .Zip(new[] {1, 2, 3, 10, 9, 8, 7, 6, 5}, (l, r) => (l + r))
                .Should()
                .Equal("31", "42", "13", "510");
        }

        [TestMethod]
        public void ZipMap_ShorterRightArray_ReturnsZipWithAllRightAndCorrespondingLeftElements()
        {
            var pipe = Pipe.From(new [] {"3", "4", "1", "5", "7", "9", "3"});

            pipe
                .Zip(new[] {1, 2, 3, 10, 4}, (l, r) => (l + r))
                .Should()
                .Equal("31", "42", "13", "510", "74");

        }


        [TestMethod]
        public void ZipMap_EqualLengthArrays_ReturnsZipWithAllElements()
        {
            var pipe = Pipe.From(new [] {"3", "4", "1", "5", "11"});

            pipe
                .Zip(new[] {1, 2, 3, 10, 9}, (l, r) => (l + r))
                .Should()
                .Equal("31", "42", "13", "510", "119");
        }

        [TestMethod]
        public void ZipMap_ZipResetZipMap_ReturnsElementsFromHeadsOfBothInputSequences()
        {
            var pipe = 
                Pipe
                    .From(new [] {"3", "4", "1", "5", "7", "9", "3"})
                    .Zip(new[] {1, 2, 3, 10, 4}, (l, r) => (l + r))
                    .Collect();

            pipe
                .Should()                
                .Equal("31", "42", "13", "510", "74");

        }
    }
}
