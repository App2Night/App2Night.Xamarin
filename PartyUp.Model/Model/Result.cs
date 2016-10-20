namespace PartyUp.Model.Model
{
    public class Result<TExpectedType>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public TExpectedType Data { get; set; }
        public bool IsCached { get; set; }
    }
}