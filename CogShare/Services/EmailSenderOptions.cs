namespace CogShare.Services
{
    public class EmailSenderOptions
    {
        public string? SMTPServer { get; set; }

        public int SMTPPort { get; set; }

        public string? SMTPUser { get; set; }

        public string? SMTPPassword { get; set; }
    }
}
