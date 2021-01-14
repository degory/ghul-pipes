using System;
using System.Collections.Generic;

namespace Pipes
{
    public class MapPipe<TFrom,TTo>: Pipe<TTo>
    {
        private TTo current;
        private IEnumerator<TFrom> enumerator;

        private Func<TFrom,TTo> mapper;

        public MapPipe(IEnumerator<TFrom> enumerator, Func<TFrom, TTo> mapper) : base(null){
            this.enumerator = enumerator;
            this.mapper = mapper;
        }

        public override TTo Current { get => current; }

        public override bool MoveNext() {
            if (enumerator.MoveNext()) {
                current = mapper(enumerator.Current);
                return true;
            }

            enumerator.Reset();

            return false;
        }

        public override void Reset() {
            enumerator.Reset();
        }

        public override void Dispose() {
            enumerator.Dispose();
        }
    }
}
