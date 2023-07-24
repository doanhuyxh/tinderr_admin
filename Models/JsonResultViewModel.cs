namespace tinderr.Models
{
    public class JsonResultViewModel
    {
        public string Message { set; get; }
        public bool IsSuccess { get; set; }
        public dynamic Data { get; set; }
    }
}
