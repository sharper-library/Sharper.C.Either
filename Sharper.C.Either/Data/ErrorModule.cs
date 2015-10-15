using System;
using static Sharper.C.Data.EitherModule;

namespace Sharper.C.Data
{

public static class ErrorModule
{
    public static Either<Exception, B> Error<B>(Exception e)
    =>
        Left<Exception, B>(e);

    public static Either<Exception, B> Result<B>(B b)
    =>
        Right<Exception, B>(b);

    public static Either<Exception, A> Try<A>(Func<A> a)
    {
        try
        {
            return Result(a());
        }
        catch (Exception e)
        {
            return Error<A>(e);
        }
    }

    public static Either<Exception, A> Handle<A>
      ( Func<Exception, Either<Exception, A>> r
      , Either<Exception, A> ea
      )
    =>
        ea.Match(r, Result);

    public static A ResultOrThrow<A>(Either<Exception, A> ea)
    =>
        ea.Match(e => {throw e;}, a => a);
}

}
