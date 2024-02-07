using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using FluentAssertions;
using Moq;

using Pipes;

namespace Tests
{
    [TestClass]
    public class ZipPipeShould
    {
        private static IndexedValue<string> ivs(int index, string value) => new IndexedValue<string>(index, value);

        [TestMethod]
        public void Zip_EmptyLeftArray_ReturnsEmptySequence()
        {
            var pipe = Pipe.From(new int[0]);

            pipe
                .Zip(new [] {1, 2, 3, 4, 5})
                .Should()
                .Equal(new (int,int)[0]);
        }

        [TestMethod]
        public void Zip_EmptyRightArray_ReturnsEmptySequence()
        {
            var pipe = Pipe.From(new [] {1, 2, 3, 4});

            pipe
                .Zip(new int[0])
                .Should()
                .Equal(new (int,int)[0]);
        }

        [TestMethod]
        public void Zip_EmptyLeftAndRightArrays_ReturnsEmptySequence()
        {
            var pipe = Pipe.From(new int[0]);

            pipe
                .Zip(new int[0])
                .Should()
                .Equal(new (int,int)[0]);
        }

        [TestMethod]
        public void Zip_ShorterLeftArray_ReturnsZipWithAllLeftAndCorrespondingRightElements()
        {
            var pipe = Pipe.From(new [] {"3", "4", "1", "5"});

            pipe
                .Zip(new[] {1, 2, 3, 10, 9, 8, 7, 6, 5})
                .Should()
                .Equal(("3", 1), ("4", 2), ("1", 3), ("5", 10));
        }

        [TestMethod]
        public void Zip_ShorterRightArray_ReturnsZipWithAllRightAndCorrespondingLeftElements()
        {
            var pipe = Pipe.From(new [] {"3", "4", "1", "5", "7", "9", "3"});

            pipe
                .Zip(new[] {1, 2, 3, 10, 4})
                .Should()
                .Equal(("3", 1), ("4", 2), ("1", 3), ("5", 10), ("7", 4));

        }


        [TestMethod]
        public void Zip_EqualLengthArrays_ReturnsZipWithAllElements()
        {
            var pipe = Pipe.From(new [] {"3", "4", "1", "5", "11"});

            pipe
                .Zip(new[] {1, 2, 3, 10, 9})
                .Should()
                .Equal(("3", 1), ("4", 2), ("1", 3), ("5", 10), ("11", 9));
        }

        [TestMethod]
        public void Zip_ZipResetZip_ReturnsElementsFromHeadsOfBothInputSequences()
        {
            var pipe = 
                Pipe
                    .From(new [] {"3", "4", "1", "5", "7", "9", "3"})
                    .Zip(new[] {1, 2, 3, 10, 4})
                    .Collect();

            pipe
                .Should()                
                .Equal(("3", 1), ("4", 2), ("1", 3), ("5", 10), ("7", 4));

        }

    }
}
