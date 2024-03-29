﻿using AlbumsToBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AlbumsToBuy.Helpers
{
	public class MailHelper
	{
		protected static void SendMail(string recipient, string subject, string body)
		{
			string email = "openautoemails@gmail.com";
			string password = "FTAWWR100";

			using (MailMessage mail = new MailMessage())
			{
				mail.From = new MailAddress(email);
				mail.To.Add(recipient);
				mail.Subject = subject;
				mail.IsBodyHtml = true;
				mail.Body = body;

				using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
				{
					smtp.Credentials = new NetworkCredential(email, password);
					smtp.EnableSsl = true;
					smtp.Send(mail);
				}
			}
		}

		public static void RegistrationConfirmation(User user)
		{
			//string recipent = user.Email;
			string recipent = "dustinjoosen2003@gmail.com";
			string subject = "AlbumsToBuy Registration Confirmation";

			var sb = new StringBuilder();
			sb.AppendLine($"Hello {user.FullName}<br/><br/>");
			sb.AppendLine("You have registered to AlbumsToBuy using this email address.<br/>");
			sb.AppendLine($"To confirm the registration, click <a href='https://localhost:44324/Account/Confirm/{user.Id}'>here</a><br/>");
			sb.AppendLine("If this was not you, you can ignore this email.<br/>");
			sb.AppendLine("\nIf you want to take contact with us, you can safely reply to this email.<br/>");


			SendMail(recipent, subject, sb.ToString());
		}

		public static void OrderConfirmation(Order order)
		{
			if(order.Albums == null)
			{
				return;
			}

			//string recipient = order.User.Email;
			string recipient = "dustinjoosen2003@gmail.com";

			string subject = "Order Confirmation - AlbumsToBuy";

			var sb = new StringBuilder();
			sb.AppendLine("We, at AlbumsToBuy, have recieved your order.<br/>");
			sb.AppendLine($"This order was recieved on {order.OrderDate}.<br/>");
			sb.AppendLine($"Your order is being sent to {order.Street} {order.ZipCode}, {order.City}. In {order.Country}.<br/>");
			
			if(order.SpecialNotes != null)
			{
				sb.AppendLine($"Special notes: {order.SpecialNotes}.<br/>");
			}

			sb.AppendLine("<hr/>Albums ordered:<br/>");
			foreach(var item in order.Albums)
			{
				sb.AppendLine($"\t{item.Quantity} x {item.Album.Name} by {item.Album.Creator}.<br/>");
			}
			sb.AppendLine("<hr/>");

			sb.AppendLine($"Price: {order.Payment.Amount}.<br/>");
			sb.AppendLine($"Your Order Id is {order.Id}. this can be used when taking contact with us.<br/>");

			sb.AppendLine("\nIf you want to take contact with us, you can safely reply to this email.<br/>");

			SendMail(recipient, subject, sb.ToString());
		}

		public static void ForgotPassword(User user)
		{
			//var recipent = user.Email;
			string recipent = "dustinjoosen2003@gmail.com";
			string subject = "Password reset - AlbumsToBuy";

			var sb = new StringBuilder();

			sb.AppendLine("This email has been sent because you want to reset your password<br/>");
			sb.AppendLine("To reset your password, click on the underlying link:<br/>");
			sb.AppendLine($"<a href='https://localhost:44324/Account/ResetPassword/{user.Id}'>Reset Link</a><br/>");
			sb.AppendLine("\nIf you want to take contact with us, you can safely reply to this email.<br/>");

			SendMail(recipent, subject, sb.ToString());
		}
	}
}
