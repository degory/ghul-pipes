using System;
using System.Collections.Generic;

namespace Pipes
{
    // ghūl doesn't have great support for tuples yet, so using a bespoke struct here for now. Once ghūl gets either
    // tuple destructuring or named tuple elements, this can be migrated over to an (index, value) tuple:
    public struct IndexedValue<T> {
        public int Index;
        public T Value;

        public IndexedValue(int index, T value) {
            Index = index;
            Value = value;
        }

        public void Deconstruct(out int index, out T value) {
            index = Index;
            value = Value;
        }
    }

    public class IndexPipe<T>: Pipe<IndexedValue<T>>
    {
        private int index;
        private IEnumerator<T> enumerator;

        public IndexPipe(IEnumerator<T> enumerator) : base(null){
            this.enumerator = enumerator;         

            index = -1;   
        }

        public override IndexedValue<T> Current { get => new IndexedValue<T>(index,enumerator.Current); }

        public override bool MoveNext() {
            if (enumerator.MoveNext()) {
                index++;
                return true;
            }

            Reset();

            return false;
        }

        public override void Reset() {
            index = -1;
            enumerator.Reset();
        }

        public override void Dispose() {
            enumerator.Dispose();
        }
    }
}
