namespace armaradio.ArmaSmtp
{
    public interface IArmaEmail
    {
        public void SendEmail(
            string FromEmail,
            string Subject,
            List<string> To,
            string Body,
            string EmailToken
        );
    }
}
