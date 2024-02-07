using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using FluentAssertions;
using Moq;

using Pipes;

namespace Tests
{
    [TestClass]
    public class IndexPipeShould
    {
        private static IndexedValue<string> ivs(int index, string value) => new IndexedValue<string>(index, value);

        [TestMethod]
        public void Index_EmptyArray_ReturnsEmptySequence()
        {
            var pipe = Pipe.From(new int[0]);

            pipe
                .Map(i => i.ToString())
                .Should()
                .Equal(new string[0]);
        }

        [TestMethod]
        public void Index_MultipleElements_ReturnsExpectedSequence()
        {
            var pipe = Pipe.From(new [] {"3", "4", "1", "5", "2", "3", "1", "4", "5"});

            pipe
                .Index()
                .Should()
                .Equal(new [] {ivs(0, "3"), ivs(1, "4"), ivs(2, "1"), ivs(3, "5"), ivs(4, "2"), ivs(5, "3"), ivs(6, "1"), ivs(7, "4"), ivs(8, "5")});
        
        }

        [TestMethod]
        public void Index_MultipleElementsPositiveInitialIndex_ReturnsExpectedSequence()
        {
            var pipe = Pipe.From(new [] {"3", "4", "1", "5", "2", "3", "1", "4", "5"});

            pipe
                .Index(3)
                .Should()
                .Equal(new [] {ivs(3, "3"), ivs(4, "4"), ivs(5, "1"), ivs(6, "5"), ivs(7, "2"), ivs(8, "3"), ivs(9, "1"), ivs(10, "4"), ivs(11, "5")});
        }

        [TestMethod]
        public void Index_MultipleElementsNegativeInitialIndex_ReturnsExpectedSequence()
        {
            var pipe = Pipe.From(new [] {"3", "4", "1", "5", "2", "3", "1", "4", "5"});

            pipe
                .Index(-3)
                .Should()
                .Equal(new [] {ivs(-3, "3"), ivs(-2, "4"), ivs(-1, "1"), ivs(0, "5"), ivs(1, "2"), ivs(2, "3"), ivs(3, "1"), ivs(4, "4"), ivs(5, "5")});
        }

        [TestMethod]
        public void IndexedValue_ToString_ReturnsIndexAndValueInParentheses() {
            var indexedValue = new IndexedValue<string>(1234,"ABCD");

            // TODO: could mock and verify it actually calls ToString on its Value property
            indexedValue.ToString().Should().Be("(1234,ABCD)");
        }
    }
}
