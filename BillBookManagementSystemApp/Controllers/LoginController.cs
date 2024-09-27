using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using BillBookManagementSystemApp.Models;

namespace BillBookManagementSystemApp.Controllers
{
    public class LoginController : Controller
    {
        private static string _generatedOtp; // Store OTP in memory for simplicity
        private static string _userEmail;
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult RequestOtp( string email)
        {
            _userEmail = email;
            _generatedOtp = GenerateOtp();

            // Send OTP to user's email
            SendOtpToEmail(_userEmail, _generatedOtp);

            return Json(new { success = true, message = "OTP sent to your email." });
        }

        [HttpPost]
        public JsonResult VerifyOtp(string otp)
        {
            if (otp == _generatedOtp)
            {
                // OTP is correct
                return Json(new { success = true, redirectUrl = Url.Action("Index") });
            }

            // OTP is incorrect
            return Json(new { success = false, message = "Invalid OTP. Please try again." });
        }

        public ActionResult Success()
        {
            return View();
        }

        private string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Generate a 6-digit OTP
        }

        private void SendOtpToEmail(string email, string otp)
        {
            var fromAddress = new MailAddress("prasadmhasal@gmail.com", "Prasad");
            var toAddress = new MailAddress(email);
            const string fromPassword = "qsafikqptsweailz"; // Replace with your email password
            const string subject = "Your OTP Code";

            // HTML email body
            string body = $@"
    <!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">
    <html xmlns=""http://www.w3.org/1999/xhtml"">
    <head>
      <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
      <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
      <title>Verify your login</title>
    </head>
    <body style=""font-family: Helvetica, Arial, sans-serif; margin: 0px; padding: 0px; background-color: #ffffff;"">
      <table role=""presentation"" style=""width: 100%; border-collapse: collapse; border: 0px; border-spacing: 0px; font-family: Arial, Helvetica, sans-serif; background-color: rgb(239, 239, 239);"">
        <tbody>
          <tr>
            <td align=""center"" style=""padding: 1rem 2rem; vertical-align: top; width: 100%;"">
              <table role=""presentation"" style=""max-width: 600px; border-collapse: collapse; border: 0px; border-spacing: 0px; text-align: left;"">
                <tbody>
                  <tr>
                    <td style=""padding: 40px 0px 0px;"">
                      <div style=""text-align: left;"">
                        <div style=""padding-bottom: 20px;""><img src=""https://i.ibb.co/Qbnj4mz/logo.png"" alt=""Company"" style=""width: 56px;""></div>
                      </div>
                      <div style=""padding: 20px; background-color: rgb(255, 255, 255);"">
                        <div style=""color: rgb(0, 0, 0); text-align: left;"">
                          <h1 style=""margin: 1rem 0"">Verification code</h1>
                          <p style=""padding-bottom: 16px"">Please use the verification code below to sign in.</p>
                          <p style=""padding-bottom: 16px""><strong style=""font-size: 130%"">{otp}</strong></p>
                          <p style=""padding-bottom: 16px"">If you didn’t request this, you can ignore this email.</p>
                          <p style=""padding-bottom: 16px"">Thanks,<br> MasstechBillBook Team</p>
                        </div>
                      </div>
                      <div style=""padding-top: 20px; color: rgb(153, 153, 153); text-align: center;"">
                        <p style=""padding-bottom: 16px"">Made with ♥ in India</p>
                      </div>
                    </td>
                  </tr>
                </tbody>
              </table>
            </td>
          </tr>
        </tbody>
      </table>
    </body>
    </html>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com", // Replace with your SMTP server
                Port = 587, // Use appropriate port
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true // Important to set this to true for HTML emails
            })
            {
                smtp.Send(message);
            }
        }

    }
}
