namespace Demo.WebApi.Common.ResultPackage
{
    public class Result<T>
    {
        public T Data { get; set; }
        
        public string Message { get; set; }

        public int ResultCode { get; set; }
    }
}