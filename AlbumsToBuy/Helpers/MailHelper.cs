using AlbumsToBuy.Models;
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

		public static void OrderConfirmation(Order order)
		{
			if(order.Albums == null)
			{
				return;
			}

			//string recipient = order.User.Email;
			string recipient = "dustinjoosen2003@gmail.com";

			string subject = "Order Confirmation";

			var sb = new StringBuilder();
			sb.AppendLine("We have recieved your order.<br/>");
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
	}
}
