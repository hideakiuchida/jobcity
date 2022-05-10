namespace Jobcity.Chat.InfraLayer.Dtos.Responses
{
    public class Response<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
