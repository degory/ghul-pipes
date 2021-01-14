using System;
using System.Collections.Generic;

namespace Pipes
{
    public class AdaptorPipe<T>: Pipe<T>
    {
        private IEnumerator<T> enumerator;

        public AdaptorPipe(IEnumerable<T> enumerable) : base(enumerable) {
            enumerator = enumerable.GetEnumerator();
        }

        public override T Current { get => enumerator.Current; }

        public override bool MoveNext() {
            var result = enumerator.MoveNext();

            if (!result) {
                enumerator.Reset();
            }

            return result;
        }

        public override void Reset() => enumerator.Reset();
    }
}
