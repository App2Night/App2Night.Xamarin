namespace App2Night.Model.Model
{
    public class Result
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public bool IsCached { get; set; }
        public bool RequestFailedToException { get; set; }
    }

    public class Result<TExpectedType> : Result
    {
        public TExpectedType Data { get; set; }
    }
}