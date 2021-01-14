using System;
using System.Collections.Generic;

namespace Pipes
{
    public class ZipPipe<T1,T2>: Pipe<(T1,T2)>
    {
        private IEnumerator<T1> enumerator1;
        private IEnumerator<T2> enumerator2;

        private (T1,T2) current;

        public ZipPipe(IEnumerator<T1> enumerator1, IEnumerator<T2> enumerator2) : base(null){
            this.enumerator1 = enumerator1;
            this.enumerator2 = enumerator2;
        }

        public override (T1,T2) Current { get => current; }

        public override bool MoveNext() {
            if (enumerator1.MoveNext() && enumerator2.MoveNext()) {
                current = (enumerator1.Current, enumerator2.Current);
                return true;
            }

            Reset();

            return false;
        }

        public override void Reset() {
            enumerator1.Reset();
            enumerator2.Reset();
        }

        public override void Dispose() {
            enumerator1.Dispose();
            enumerator2.Dispose();
        }
    }
}
