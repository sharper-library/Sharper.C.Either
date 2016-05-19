using FsCheck;

namespace Sharper.C.Testing
{

using static Data.SumModule;

public static class SumTestingModule
{
    public static Arbitrary<Or<A, B>> AnySum<A, B>
      ( Arbitrary<A> arbA
      , Arbitrary<B> arbB
      )
    =>
        Arb.From
          ( Gen.OneOf
              ( new[]
                { arbA.Generator.Select(Left<A, B>)
                , arbB.Generator.Select(Right<A, B>)
                }
              )
          );
}

}
