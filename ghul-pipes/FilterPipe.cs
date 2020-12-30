using System;
using System.Collections.Generic;

namespace Pipes
{
    public class FilterPipe<T>: FilterPipeBase<T>
    {
        private Func<T,bool> predicate;

        public FilterPipe(IEnumerator<T> enumerator, Func<T, bool> predicate) : base(enumerator) {
            this.enumerator = enumerator;
            this.predicate = predicate;
        }

        protected override bool ShouldInclude() => predicate(Current);
    }
}
