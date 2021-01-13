using System;
using System.Collections.Generic;

namespace Pipes
{
    // FIXME: should dispose of all enumerators - ideally somehow check if they really need disposing
    // and so only pay cost of try/finally handler if genuinely needed. For now, in practice, none
    // of the enumerators we want to use actually have any non-managed state that needs to be disposed
    public class Pipe {
        public static Pipe<T> From<T>(IEnumerable<T> source) => Pipe<T>.From(source);
    }

    public class Pipe<T>: IEnumerable<T>, IEnumerator<T>
    {
        protected IEnumerable<T> source;

        public Pipe(IEnumerable<T> source) {
            this.source = source;
        }

        public static Pipe<T> From(IEnumerable<T> source) {
            var result = source as Pipe<T>;

            if (result != null) {
                return result;
            }

            return new AdaptorPipe<T>(source);
        }

        public Pipe<T> Filter(Func<T,bool> predicate) =>
            new FilterPipe<T>(GetEnumerator(), predicate);

        public Pipe<U> Map<U>(Func<T,U> mapper) =>
            new MapPipe<T,U>(GetEnumerator(), mapper);

        public Pipe<T> Skip(int skipCount) => 
            new SkipPipe<T>(GetEnumerator(), skipCount);

        public Pipe<T> Take(int takeCount) => 
            new TakePipe<T>(GetEnumerator(), takeCount);

        public Pipe<T> Cat(IEnumerable<T> right) =>
            new CatPipe<T>(GetEnumerator(), right.GetEnumerator());

        public Pipe<IndexedValue<T>> Index() =>
            new IndexPipe<T>(GetEnumerator());

        public Pipe<(T,T2)> Zip<T2>(IEnumerable<T2> other) =>
            new ZipPipe<T,T2>(GetEnumerator(), other.GetEnumerator());

        public Pipe<TOut> Zip<T2,TOut>(IEnumerable<T2> other, Func<T,T2,TOut> mapper) =>
            new ZipMapPipe<T,T2,TOut>(GetEnumerator(), other.GetEnumerator(), mapper);

        public TRunning Reduce<TRunning> (TRunning seed, Func<TRunning,T,TRunning> accumulator) {
            TRunning running = seed;

            foreach (var element in this) {
                running = accumulator(running, element);
            }

            return running;
        }

        public TOut Reduce<TRunning,TOut> (TRunning seed, Func<TRunning,T,TRunning> accumulator, Func<TRunning,TOut> mapper) {
            TRunning running = seed;

            foreach (var element in this) {
                running = accumulator(running, element);
            }

            return mapper(running);            
        }

        public IReadOnlyList<T> Collect() => CollectList();

        public T[] CollectArray() => CollectList().ToArray();

        public List<T> CollectList() {
            var result = new List<T>();

            while (MoveNext()) {
                result.Add(Current);
            }

            Reset();

            return result;
        }

        // Must be overridden if pipe can change the source length - e.g. filter
        public virtual int Count() {
            var collection = source as ICollection<T>;

            if (collection != null) {
                return collection.Count;
            }

            var count = 0;                

            while(MoveNext()){
                count++;
            }

            Reset();

            return count;                
        }

        public virtual T First() {
            var result = default(T);

            if (MoveNext()) {
                result = Current;

                Reset();
            }

            return result;
        }

        public virtual T Only() {
            if (MoveNext()) {
                var result = Current;

                Reset();

                return result;
            }

            throw new System.InvalidOperationException("no element found");
        }

        public virtual Pipe<T> Sort() {
            var list = CollectList();

            list.Sort();

            Reset();

            return Pipe.From(list);
        }

        public virtual Pipe<T> Sort(IComparer<T> comparer) {
            var list = CollectList();

            list.Sort(comparer);

            Reset();

            return Pipe.From(list);
        }

        public virtual Pipe<T> Sort(Func<T,T,int> compare) => Sort(new FunctionComparer<T>(compare));

        // Deprecated - reserve 'Join' for joining two pipes on a key:
        public virtual string Join(string separator) {
            var result = new System.Text.StringBuilder();
            var seen_any = false;

            while(MoveNext()) {
                if (seen_any) {
                    result.Append(separator);
                }

                result.Append(Current);

                seen_any = true;
            }

            Reset();

            return result.ToString();
        }

        // Deprecated - reserve 'Join' for joining two pipes on a key:
        public virtual string Join() => Join(", ");

        public virtual string ToString(string separator) {
            var result = new System.Text.StringBuilder();
            var seen_any = false;

            while(MoveNext()) {
                if (seen_any) {
                    result.Append(separator);
                }

                result.Append(Current);

                seen_any = true;
            }

            Reset();

            return result.ToString();
        }

        public override string ToString() => ToString(", ");

        #region function comparer
        private class FunctionComparer<U>: IComparer<U> {
            private Func<U,U,int> comparer;

            public FunctionComparer(Func<U,U,int> comparer) {
                this.comparer = comparer;
            }

            public int Compare(U x, U y) => comparer(x, y);
        }
        #endregion

        #region boilerplate
        public IEnumerator<T> GetEnumerator() => this;

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        // ghūl doesn't have abstract classes:
        public virtual bool MoveNext() => throw new NotImplementedException();
        public virtual T Current { get => throw new NotImplementedException(); }

        public virtual void Reset() => throw new NotImplementedException();

        object System.Collections.IEnumerator.Current { get => Current; }

        public virtual void Dispose() { }
        #endregion
    }
}
