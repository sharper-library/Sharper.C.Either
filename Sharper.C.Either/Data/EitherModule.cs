using System;

namespace Sharper.C.Data
{

public static class EitherModule
{
    public sealed class Either<A, B>
    {
        public bool IsLeft { get; }
        public bool IsRight => !IsLeft;

        private readonly A leftValue;
        private readonly B rightValue;

        internal Either(bool isLeft, A left, B right)
        {
            IsLeft = isLeft;
            leftValue = left;
            rightValue = right;
        }

        public Either<A, C> FlatMap<C>(Func<B, Either<A, C>> f)
        =>
            IsLeft
            ? new Either<A, C>(true, leftValue, default(C))
            : f(rightValue);

        public Either<A, C> Map<C>(Func<B, C> f)
        =>
            IsLeft
            ? new Either<A, C>(true, leftValue, default(C))
            : new Either<A, C>(false, default(A), f(rightValue));

        public C Match<C>(Func<A, C> left, Func<B, C> right)
        =>
            IsLeft ? left(leftValue) : right(rightValue);
    }

    public static Either<A, B> Left<A, B>(A a)
    =>
        new Either<A, B>(true, a, default(B));

    public static Either<A, B> Right<A, B>(B b)
    =>
        new Either<A, B>(false, default(A), b);

    public struct LeftModule<A>
    {
        public Either<A, B> Left<B>(A a)
        =>
            Left<A, B>(a);

        public Either<A, B> Right<B>(B b)
        =>
            Right<A, B>(b);
    }
}

}
