using FsCheck;
using Sharper.C.Data;

namespace Sharper.C.Testing.Arbitraries
{

public static class SumArbitrariesModule
{
    public static Arbitrary<Or<A, B>> AnySum<A, B>
      ( Arbitrary<A> arbA
      , Arbitrary<B> arbB
      )
    =>
        Arb.From
          ( Gen.OneOf
              ( new[]
                { arbA.Generator.Select(Or.Left<A, B>)
                , arbB.Generator.Select(Or.Right<A, B>)
                }
              )
          );
}

}
