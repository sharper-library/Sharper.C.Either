using System;

namespace Sharper.C.Data
{

public static class EitherModule
{
    public struct Either<A, B>
    {
        public Either<A, C> FlatMap<C>(Func<B, Either<A, C>> f)
        =>
            default(Either<A, C>);

        public Either<A, C> Map<C>(Func<B, C> f)
        =>
            default(Either<A, C>);

        public C Match<C>(Func<A, C> left, Func<B, C> right)
        =>
            default(C);
    }

    public static Either<A, B> Left<A, B>(A a)
    =>
        default(Either<A, B>);

    public static Either<A, B> Right<A, B>(B b)
    =>
        default(Either<A, B>);

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
