using AspNetCore.Identity.LiteDB.Models;
using Bleifood.BL.Interfaces;

using Bleifood.Entities;

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

        public string SmtpHost { get { return "Smtp:Host".FromConfig(); } }
        public string SmtpPort { get { return "Smtp:Port".FromConfig(); } }
        public string SmtpUser { get { return "Smtp:User".FromConfig(); } }
        public string SmtpPassword { get { return "Smtp:Password".FromConfig(); } }
        public string SmtpSender { get { return "Smtp:Sender".FromConfig(); } }
        public string SmtpBCC { get { return "Smtp:BCC".FromConfig(); } }
        public string SmtpCC { get { return "Smtp:CC".FromConfig(); } }


      
        private string CreateMainBody(Entities.Order order)
        {
            string body= $"BestellId (Paypal!): {order.UniqueKey}\n ";
            body += $"Gewünschte Uhrzeit: {order.TimeSlot}\n\n ";
            body += CreateOrderPositions(order);
            return body;
        }

        public void OrderCustomer(Entities.Order order)
        {
            string subject = "Deine Bestellung wurde aufgegeben";
            string body = MailHeaderOrderCustomer;
            body += CreateMainBody(order);
           
            body += GetShopAddress(order.Truck);
            body += GetOrderPPFooter(order);
            SendMail(order.CustomerAddress.Mail, subject, body, order.Truck.PostAddress.Mail);
        }



        public void OrderFoodTruck(Entities.Order order)
        {
            string subject = "Eine Bestellung wurde aufgegeben";
            
            string body = MailHeaderOrderFoodtruck;

            if (!order.Truck.Active && order.Truck.TestFinished == null)
            {
                body += "\n DU BEFINDEST DICH IM TESTMODUS!\n";
                body+="Wenn Du mit der Bestellung zufrieden bist, klicke auf den folgenden Link um den Test abzuschließen:\n";
                body += $"{GetHostName()}manage/test/{order.Truck.Url}/{order.Truck.TestToken} \n";
                body += $"====================================================================================================\n\n";
            }
            body += CreateMainBody(order);
            body += GetCustomerAddress(order);
            SendMail(order.Truck.PostAddress.Mail, subject, body, order.CustomerAddress.Mail);
        }

        private string GetShopAddress(Entities.FoodTruck truck)
        {
            return $"Anbieter:\n{GetAddress(truck.PostAddress)}";
        }

        private string GetCustomerAddress(Entities.Order order)
        {
            return $"Besteller:\n{GetAddress(order.CustomerAddress)}";
        }

        private string GetAddress(Address address)
        {
            return $"{address.Name}\n{address}\n{address.Mail}\n{address.Phone}\n\n";
        }

    

        private string CreateOrderPositions(Entities.Order order)
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
            return string.Format("{0}\t{1:0000000000}\t{2:0000000000}\t{3}\n", "Menge", "Preis", "Summe", "Bezeichnung");
        }

        private string GetOrderPositionText(OrderPosition position)
        {
            return $"{position.Amount}\t{position.Position.Price.AsCurrency():0000000000}\t{(position.Position.Price * position.Amount).AsCurrency():0000000000}\t{position.Position.Name}\n";
        }

      
        private string GetOrderSumText(Entities.Order order)
        {
            string footer = string.Format("1\t{0:00000000}\t{0:0000000000}\tVersand\n",  order.ShippingCost().AsCurrency());
            if (order.Tip > 0)
            {
                footer += string.Format("1\t{0:0000000000}\t{0:0000000000}\tTrinkgeld\n",  order.Tip.AsCurrency());
            }
            footer += "-----------------------------------------------------------------------\n";
            footer += string.Format("\t\t{0:0000000000}\tTotal\n\n",  order.Total().AsCurrency());
            return footer;
        }

        private string GetOrderPPFooter(Entities.Order order)
        {
            string footer = $"Bitte begleiche den Betrag von {order.Total().AsCurrency()} (falls noch nicht geschehen) per Paypal über folgenden Link: \n";
            footer += order.PaypalMe();
            footer += $"\n\nGib als Bemerkung bitte den Code {order.UniqueKey} ein, damit Deine Zahlung leichter zugeordnet werden kann.\nGuten Appetit!";
            return footer;
        }
 


        private string GetHostName()
        {
            return "AppSettings:BaseUrl".FromConfig();
        }

        public async void SendMail(string recipient, string subject, string body, string replyTo)
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

            await client.SendMailAsync(message);
      
        }

        public void SendMail(ApplicationUser user, string subject, string body)
        {
            SendMail(user.Email.Address, subject, body, null);
        }

        private string Encode(string urlParam)
        {
            return HttpUtility.UrlEncode(urlParam);
        }

        public void Validate(ApplicationUser user, string token)
        {
            string subject = "Bitte bestätige Deine Anmeldung";
            string body = "Du hast Dich bei bleifood.de angemeldet. Bitte klicke auf den folgenden Link um die Registrierung abzuschließen:\n\n";
            body += $"{GetHostName()}manage/validate/?userId={Encode(user.Id)}&auth={Encode(token)} \n";
            SendMail(user, subject, body);
        }
    }
}