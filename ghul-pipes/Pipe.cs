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

            return result.ToString();
        }

        #region boilerplate
        public IEnumerator<T> GetEnumerator() => this;

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        public virtual bool MoveNext() => throw new NotImplementedException();
        public virtual T Current { get => throw new NotImplementedException(); }

        public virtual void Reset() => throw new NotImplementedException();

        object System.Collections.IEnumerator.Current { get => Current; }

        public virtual void Dispose() { }
        #endregion
    }
}
