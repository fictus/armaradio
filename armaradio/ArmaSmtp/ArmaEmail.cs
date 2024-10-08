using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Net.Mail;

namespace armaradio.ArmaSmtp
{
    public class ArmaEmail : IArmaEmail
    {
        public ArmaEmail()
        {

        }

        public void SendEmail(
            string FromEmail,
            string Subject,
            List<string> To,
            string Body
        )
        {
            if (!Debugger.IsAttached && To != null && To.Count > 0)
            {
                using (var smtpClient = new SmtpClient("localhost", 25))
                {
                    var mailMessage = new MailMessage()
                    {
                        From = new MailAddress(FromEmail, "Arma Radio"),
                        Subject = Subject,
                        Body = (Body + GetEmailFooter(To.First())),
                        IsBodyHtml = true
                    };

                    foreach (var emailTo in To)
                    {
                        mailMessage.To.Add(emailTo);
                    }

                    smtpClient.Send(mailMessage);
                }
            }
        }

        private string GetEmailFooter(string EmailTo)
        {
            string returnItem = @$"
                <br>
                <br>
                <br>
                <br>
                <br>
                <br>
                <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #f7f7f7; padding: 20px 0; font-family: Arial, sans-serif; color: #555555; font-size: 14px; line-height: 1.5;"">
                    <tr>
                        <td align=""center"">
                            <table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #ffffff; padding: 20px; border: 1px solid #e0e0e0; border-radius: 5px;"">
                                <tr>
                                    <td style=""text-align: center; color: #999999; font-size: 12px;"">
                                        <p style=""margin: 0; padding: 0;"">You're receiving this email because you subscribed to <strong>ArmaRad</strong>.</p>
                                        <p style=""margin: 10px 0; padding: 0;"">
                                            If you no longer wish to receive these emails, you can <a href=""https://armarad.com/registration/unsubscribe?e={EmailTo.Trim()}"" style=""color: #007bff; text-decoration: none;"">unsubscribe here</a>.
                                        </p>
                                        <hr style=""border: 0; border-top: 1px solid #e0e0e0; margin: 20px 0;"">
                                        <p style=""margin: 0; padding: 0;"">&copy; {DateTime.Now.Year} Armarad</p>
                                        <p style=""margin: 5px 0 0 0; padding: 0;"">
                                            <a href=""https://armarad.com/privacy"" style=""color: #007bff; text-decoration: none;"">Privacy Policy</a> | 
                                            <a href=""https://armarad.com/terms"" style=""color: #007bff; text-decoration: none;"">Terms of Service</a>
                                        </p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            ";

            return returnItem;
        }
    }
}
