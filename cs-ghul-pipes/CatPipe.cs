using System;
using System.Collections.Generic;

namespace Pipes
{
    public class CatPipe<T>: Pipe<T>
    {
        private readonly IEnumerator<T> left;
        private readonly IEnumerator<T> right;
        private bool takeFromLeft;

        public CatPipe(IEnumerator<T> left, IEnumerator<T> right) : base(null) {
            this.left = left;
            this.right = right;
            takeFromLeft = true;
        }
        public override bool MoveNext() {
            if (takeFromLeft) {
                takeFromLeft = left.MoveNext();

                if (takeFromLeft) {
                    return true;
                }

                left.Reset();
            }

            var result = right.MoveNext();

            if (result) {
                return true;
            }

            takeFromLeft = true;
            right.Reset();

            return false;
        }

        public override T Current { get => takeFromLeft ? left.Current : right.Current; }

        public override void Reset() {
            left.Reset();
            right.Reset();

            takeFromLeft = true;
        }
    }
}
