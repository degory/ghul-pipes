using System;
using System.Collections.Generic;

namespace Pipes
{
    public abstract class FilterPipeBase<T>: Pipe<T>
    {
        protected IEnumerator<T> enumerator;

        public FilterPipeBase(IEnumerator<T> enumerator): base(null) {
            this.enumerator = enumerator;
        }

        public override T Current { get => enumerator.Current; }

        public override bool MoveNext() {
            while (enumerator.MoveNext()) {
                HaveMoved();

                if (!ShouldContinue()) {
                    break;
                }
                
                if (ShouldInclude()) {
                    return true;
                }
            }

            enumerator.Reset();

            return false;
        }

        protected virtual void HaveMoved() { }

        protected virtual bool ShouldContinue() => true;

        protected abstract bool ShouldInclude();

        public override int Count() {
            var count = 0;

            while(MoveNext()){
                count++;
            }

            Reset();

            return count;
        }

        public override void Reset() {
            enumerator.Reset();
        }

        public override void Dispose() {
            enumerator.Dispose();
        }
    }
}
