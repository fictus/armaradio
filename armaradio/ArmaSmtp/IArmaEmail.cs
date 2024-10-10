using armaradio.Models.Email;

namespace armaradio.ArmaSmtp
{
    public interface IArmaEmail
    {
        void SendEmail(
            string FromEmail,
            string Subject,
            List<string> To,
            string Body,
            string EmailToken,
            List<EmailAttachmentDataItem> FileAttachments = null
        );
    }
}
