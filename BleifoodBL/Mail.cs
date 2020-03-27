using AspNetCore.Identity.LiteDB.Models;
using Bleifood.BL.Interfaces;

using CoronaEntities;

using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Bleifood.BL
{
    public class Mail : IMail
    {
        private const string MailHeaderOrderFoodtruck = "Bleifood.de: Es ist eine Bestellung eingegangen:\n\n";
        private const string MailHeaderOrderCustomer = "Bleifood.de: Du hast etwas bestellt:\n\n";
        private const string MailFooterCustomer = "(!)PRÜFE BITTE, OB DEINE ZAHLUNG BEI PAYPAL ANGEKOMMEN IST(!)";

        public string SmtpHost { get { return "Smtp:Host".FromConfig(); } }
        public string SmtpPort { get { return "Smtp:Port".FromConfig(); } }
        public string SmtpUser { get { return "Smtp:User".FromConfig(); } }
        public string SmtpPassword { get { return "Smtp:Password".FromConfig(); } }
        public string SmtpSender { get { return "Smtp:Sender".FromConfig(); } }
        public string SmtpBCC { get { return "Smtp:BCC".FromConfig(); } }
        public string SmtpCC { get { return "Smtp:CC".FromConfig(); } }

        public void OrderCustomer(Order order)
        {
            string subject = "Deine Bestellung wurde aufgegeben";
            string body = MailHeaderOrderCustomer;
            body += $"BestellId: {order.UniqueKey}\n\n ";
            body += CreateOrderPositions(order);
            body += GetShopAddress(order);
            body += MailFooterCustomer;
            SendMail(order.Customer.PostAddress.Mail, subject, body, order.Truck.PostAddress.Mail);
        }

        public void OrderFoodTruck(Order order)
        {
            string subject = "Eine Bestellung wurde aufgegeben";
            string body = MailHeaderOrderFoodtruck;
            body += $"BestellId (Paypal!): {order.UniqueKey}\n\n ";
            body += CreateOrderPositions(order);
            body += GetCustomerAddress(order);
            SendMail(order.Truck.PostAddress.Mail, subject, body, order.Customer.PostAddress.Mail);
        }

        private string GetShopAddress(Order order)
        {
            return $"Anbieter:\n{GetAddress(order.Truck.PostAddress)}";
        }

        private string GetCustomerAddress(Order order)
        {
            return $"Besteller:\n{GetAddress(order.Customer.PostAddress)}";
        }

        private string GetAddress(Address address)
        {
            return $"{address.Name}\n{address}\n{address.Mail}\n{address.Phone}\n\n";
        }

        private string CreateOrderPositions(Order order)
        {
            string body = "Bestellpositionen:\n"
                        + "-----------------\n";
            body += GetOrderPositionHeader();
            foreach (var item in order.Positions)
            {
                body += GetOrderPositionText(item);
            }
            body += GetOrderSumText(order);
            return body;
        }

        private string GetOrderPositionHeader()
        {
            return string.Format("{0:80}\t{1}\t{2:10}\t{3}\n", "Bezeichnung", "Menge", "Preis", "Summe");
        }

        private string GetOrderPositionText(OrderPosition position)
        {
            return $"{position.Position.Name:80}\t{position.Amount}\t{position.Position.Price:10}\t{position.Position.Price * position.Amount}\n";
        }

        private string GetOrderSumText(Order order)
        {
            string footer = string.Format("{0:80}\t1\t{1:10}\t{1:10}\n", "Versand", order.Shipping);
            if (order.Tipp > 0)
            {
                footer += string.Format("{0:80}\t1\t{1:10}\t{1:10}\n", "Trinkgeld", order.Tipp);
            }
            decimal fullprice = order.Shipping + order.Tipp;
            fullprice += order.Positions.Sum(q => (q.Amount * q.Position.Price));
            footer += "-----------------------------------------------------------------------\n";
            footer += string.Format("{0:90}\t\t{1:10}\n\n", "Total", fullprice);
            return footer;
        }


        private string GetHostName()
        {
            return "Hostname".FromConfig();
        }

        private void SendMail(string recipient, string subject, string body, string replyTo)
        {
            SmtpClient client = new SmtpClient(SmtpHost, int.Parse(SmtpPort));
            if (SmtpUser != null)
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(SmtpUser, SmtpPassword);
            }

            var message = new MailMessage(SmtpSender, recipient)
            {
                BodyEncoding = Encoding.UTF8,
                Body = body,
                IsBodyHtml = false,
                Subject = subject,
            };
            if (replyTo != null) message.ReplyToList.Add(replyTo);
            if (SmtpBCC != null) message.Bcc.Add(SmtpBCC);
            if (SmtpCC != null) message.Bcc.Add(SmtpCC);

            client.Send(message);
        }

        public void SendMail(ApplicationUser user, string subject, string body)
        {
            SendMail(user.Email.Address, subject, body, null);
        }

        public void Validate(ApplicationUser user)
        {
            string subject = "Bitte bestätige Deine Anmeldung";
            string body = "Du hast Dich bei bleifood.de angemeldet. Bitte klicke auf den folgenden Link um die Registrierung abzuschließen:\n\n";
            body += $"https://{GetHostName()}/manage/validate/?userId={user.Id}&auth={user.AuthenticationKey} \n";
            SendMail(user, subject, body);
        }
    }
}