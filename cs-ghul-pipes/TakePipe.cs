using System;
using System.Collections.Generic;

namespace Pipes
{
    public class TakePipe<T>: FilterPipeBase<T>
    {
        private int takeCount;

        public TakePipe(IEnumerator<T> enumerator, int takeCount) : base(enumerator) {
            this.takeCount = takeCount;
        }

        protected override void HaveMoved() =>
            takeCount--;

        protected override bool ShouldContinue() =>
            takeCount >= 0;

        protected override bool ShouldInclude() =>
            true;
    }
}
