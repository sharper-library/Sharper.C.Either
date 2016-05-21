using System;
using System.Collections.Generic;
using System.Linq;

namespace Sharper.C.Data
{

    public sealed class Or<A, B>
    {
        public bool IsLeft { get; }
        public bool IsRight => !IsLeft;

        private readonly A leftValue;
        private readonly B rightValue;

        internal Or(bool isLeft, A left, B right)
        {
            IsLeft = isLeft;
            leftValue = left;
            rightValue = right;
        }

        public C Cata<C>(Func<A, C> left, Func<B, C> right)
        =>  IsLeft ? left(leftValue) : right(rightValue);

        // ---

        public Or<A, C> FlatMap<C>(Func<B, Or<A, C>> f)
        =>  IsLeft
            ? new Or<A, C>(true, leftValue, default(C))
            : f(rightValue);

        public Or<A, C> Map<C>(Func<B, C> f)
        =>  IsLeft
            ? new Or<A, C>(true, leftValue, default(C))
            : new Or<A, C>(false, default(A), f(rightValue));

        public Or<A, B> FlatMapForEffect<C>(Func<B, Or<A, C>> f)
        =>  FlatMap(b => f(b).Map(_ => b));

        public Or<B, A> Swap
        =>  new Or<B, A>(!IsLeft, rightValue, leftValue);

        public Or<C, B> MapLeft<C>(Func<A, C> f)
        =>  IsLeft
            ? new Or<C, B>(true, f(leftValue), default(B))
            : new Or<C, B>(false, default(C), rightValue);

        public Or<C, B> FlatMapLeft<C>(Func<A, Or<C, B>> f)
        =>  IsLeft
            ? f(leftValue)
            : new Or<C, B>(false, default(C), rightValue);

        public Or<A, C> LeftOr<C>(Func<Or<A, C>> x)
        =>  IsLeft
            ? Or.Left<A, C>(leftValue)
            : x();

        public Or<C, B> RightOr<C>(Func<Or<C, B>> x)
        =>  IsRight
            ? Or.Right<C, B>(rightValue)
            : x();

        public A LeftValueOr(Func<B, A> x)
        =>  IsLeft ? leftValue : x(rightValue);

        public B RightValueOr(Func<A, B> x)
        =>  IsRight ? rightValue : x(leftValue);

        public Or<A, D> Zip<C, D>(Or<A, C> x, Func<B, C, D> f)
        =>  IsRight && IsRight
            ? Or.Right<A, D>(f(rightValue, x.rightValue))
            : Or.Left<A, D>(LeftOr(() => x).leftValue);

        public Or<D, B> LeftZip<C, D>(Or<C, B> x, Func<A, C, D> f)
        =>  Swap.Zip(x.Swap, f).Swap;

        public IEnumerable<B> ToEnumerable()
        =>  IsLeft
            ? Enumerable.Empty<B>()
            : new[] {rightValue};

        public IEnumerable<A> LeftEnumerable()
        =>  IsLeft
            ? new[] {leftValue}
            : Enumerable.Empty<A>();

        public Maybe<B> ToMaybe
        =>  IsLeft
            ? Maybe.Nothing<B>()
            : Maybe.Just(rightValue);

        public Maybe<A> LeftMaybe
        =>  IsLeft
            ? Maybe.Just(leftValue)
            : Maybe.Nothing<A>();

        public Or<A, C> Select<C>(Func<B, C> f)
        =>  Map(f);

        public Or<A, D> SelectMany<C, D>
        ( Func<B, Or<A, C>> f
        , Func<B, C, D> g
        )
        =>  FlatMap(b => f(b).Map(c => g(b, c)));

        public bool Eq
        ( Or<A, B> x
        , Func<A, A, bool> aEq
        , Func<B, B, bool> bEq
        )
        =>  IsLeft && x.IsLeft && aEq(leftValue, x.leftValue)
            ||  IsRight && x.IsRight && bEq(rightValue, x.rightValue);
    }

    public static class Or
    {
        public static Or<A, B> Left<A, B>(A a)
        =>  new Or<A, B>(true, a, default(B));

        public static Or<A, B> Right<A, B>(B b)
        =>  new Or<A, B>(false, default(A), b);

        public static Or<A, B> ToOr<A, B>(this Maybe<B> b, Func<A> a)
        =>  b.Cata(() => Left<A, B>(a()), Right<A, B>);

        public static Or<A, B> ToLeftOr<A, B>(this Maybe<A> a, Func<B> b)
        =>  a.ToOr(b).Swap;

        public static IEnumerable<Or<A, B>> Sequence<A, B>
        ( this Or<A, IEnumerable<B>> e
        )
        =>  Traverse(e, a => a);

        public static IEnumerable<Or<A, C>> Traverse<A, B, C>
        ( this Or<A, B> e
        , Func<B, IEnumerable<C>> f
        )
        =>  e.Cata
            ( a => new[] {Left<A, C>(a)}
            , b => f(b).Select(Right<A, C>)
            );

        public struct FixLeft<A>
        {
            public Or<A, B> Left<B>(A a)
            =>  Left<A, B>(a);

            public Or<A, B> Right<B>(B b)
            =>  Right<A, B>(b);
        }
    }
}
