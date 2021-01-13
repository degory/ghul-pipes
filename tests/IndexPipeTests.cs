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
    }
}
