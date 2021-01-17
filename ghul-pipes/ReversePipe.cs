using System;
using System.Collections.Generic;

namespace Pipes
{
    public class ReversePipe<T>: Pipe<T>
    {
        private IReadOnlyList<T> list;
        private int index;

        public ReversePipe(IReadOnlyList<T> list) : base(null) {
            this.list = list;

            Reset();
        }

        public override T Current { get => list[index]; }

        public override bool MoveNext() {
            index = index - 1;

            return index >= 0;
        }

        public override void Reset() {
            index = list.Count;
        }

        public override void Dispose() {
        }
    }
}
