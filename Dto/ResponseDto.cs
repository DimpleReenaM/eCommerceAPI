namespace server.Dto
{
    public class ResponseDto
    {
        public string Message { get; set; } = "Success";
        public bool IsSuccessed { get; set; } = true;
        public object? Data { get; set; } = null;

        public ResponseDto success(string Message, object? Data=null)
        {
            this.IsSuccessed = true;
            this.Message = Message;
            this.Data = Data;
            return this;
        }

        public ResponseDto Error(string Message, object? Data = null)
        {
            this.IsSuccessed = false;
            this.Message = Message;
            this.Data = Data;
            return this;
        }

    }
}
