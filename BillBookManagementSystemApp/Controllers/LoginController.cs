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
            var fromAddress = new MailAddress("prasadmhasal@gmail.com", "prasad");
            var toAddress = new MailAddress(email);
            const string fromPassword = "qsafikqptsweailz"; // Replace with your email password
            const string subject = "Your OTP Code";
            string body = $"Your OTP code is {otp}. It is valid for 5 minutes.";

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
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
  }
