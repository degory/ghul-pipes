using System;
using System.Collections.Generic;

namespace Pipes
{
    public class SkipPipe<T>: FilterPipeBase<T>
    {
        private int skipCount;

        public SkipPipe(IEnumerator<T> enumerator, int skipCount) : base(enumerator) {
            this.skipCount = skipCount;
        }

        protected override void HaveMoved() =>
            skipCount--;

        protected override bool ShouldContinue() =>
            true;

        protected override bool ShouldInclude() =>
            skipCount < 0;
    }
}
