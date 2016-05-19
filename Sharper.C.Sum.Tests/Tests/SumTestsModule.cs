using FsCheck;

namespace Sharper.C.Tests
{

using Testing.Laws;
using static Data.SumModule;
using static Testing.Arbitraries.SystemArbitrariesModule;
using static Testing.SumTestingModule;

public static class SumTestsModule
{
    // public static Test SumTests
    // =>
    //     nameof(Or<string, int>)
    //     .All
    //       ( IsMonad
    //       , HashingLaws.For
    //           ( StringIntOrHash
    //           , StringIntOrEq
    //           , StringIntOr
    //           )
    //       );

    // private static Test IsMonad
    // =>
    //     MonadLaws.For
    //       ( Right<string, int>
    //       , f => ma => ma.Map(f)
    //       , f => ma => ma.FlatMap(f)
    //       , StringIntOrEq
    //       , StringIntOr
    //       , AnyFunc1<int, Or<string, int>>(StringIntOr)
    //       , AnyFunc1<int, int>(AnyInt)
    //       , AnyInt
    //       );

    public static Arbitrary<Or<string, int>> StringIntOr { get; } =
        AnySum(AlphaNumString, AnyInt);

    public static bool StringIntOrEq
      ( Or<string, int> x
      , Or<string, int> y
      )
    =>
        x.Eq(y, (a, b) => a == b, (a, b) => a == b);

    public static int StringIntOrHash(Or<string, int> e)
    =>
        e.Cata(s => s.GetHashCode() * 37, i => i);
}

}
