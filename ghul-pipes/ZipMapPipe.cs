using System;
using System.Collections.Generic;

namespace Pipes
{
    public class ZipMapPipe<T1,T2,TOut>: Pipe<TOut>
    {
        private IEnumerator<T1> enumerator1;
        private IEnumerator<T2> enumerator2;

        private Func<T1,T2,TOut> mapper;

        private TOut current;

        public ZipMapPipe(IEnumerator<T1> enumerator1, IEnumerator<T2> enumerator2, Func<T1,T2,TOut> mapper) : base(null){
            this.enumerator1 = enumerator1;
            this.enumerator2 = enumerator2;
            this.mapper = mapper;
        }

        public override TOut Current { get => current; }

        public override bool MoveNext() {
            if (enumerator1.MoveNext() && enumerator2.MoveNext()) {
                current = mapper(enumerator1.Current, enumerator2.Current);
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
