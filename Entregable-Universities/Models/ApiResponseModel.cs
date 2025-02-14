namespace Entregable_Universities.Models
{
    public class ApiResponseModel<T>
    {
        public string message { get; set; }
        public DateTime timeStamp { get; set; }
        public bool success { get; set; }
        public T data { get; set; }
    }
}
