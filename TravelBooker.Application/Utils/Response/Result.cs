namespace TravelBooker.Application.Utils.Response
{

    public class Result
    {
        public Error Error { get; }

        public bool IsSuccess => Error == Error.None;

        protected Result(Error error)
        {
            Error = error;
        }

        public static Result Success() => new(Error.None);

        public static Result Failure(Error error) => new(error);

        public TOut Resolve<TOut>(Func<TOut> onSuccess, Func<Error, TOut> onError) => IsSuccess ? onSuccess() : onError(Error);

    }
    public class Result<T> : Result
    {
        public T? Value { get; }


        private Result(T value) : base(Error.None)
        {
            Value = value;
        }

        private Result(Error error) : base(error)
        {
            Value = default;
        }

        public static Result<T> Success(T value) => new(value);

        public static new Result<T> Failure(Error error) => new(error);

        public TOut Resolve<TOut>(Func<T, TOut> onSuccess, Func<Error, TOut> onError) => IsSuccess ? onSuccess(Value!) : onError(Error);

    }
}
