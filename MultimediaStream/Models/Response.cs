namespace MultimediaStream.Models
{
    public class Response
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
    }

    public class ResponseForRegister : Response { 
        public string? UserId { get; set; }

    }
}
