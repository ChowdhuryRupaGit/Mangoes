using Mangoes.Services.EmailAPI.Data;
using Mangoes.Services.EmailAPI.Model;
using Mangoes.Services.EmailAPI.Model.DTO;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mangoes.Services.EmailAPI.Services
{
    public class EmailServices
    {
        private DbContextOptions<AppDBContext> options;

        public EmailServices(DbContextOptions<AppDBContext> options)
        {
            this.options = options;
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emaillogger = new EmailLogger
                {
                    Email = email,
                    Message = message,
                    EmailSent = DateTime.Now
                };
                await using var db = new AppDBContext(options);
                db.EmailLogger.AddAsync(emaillogger);
                await db.SaveChangesAsync();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        
        public async Task EmailCartAndLog(CartDTO cartDTO)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("<br/>Cart Email Requeted");
            message.AppendLine("<br/>Total: "+ cartDTO.CartHeaderDTO.TotalPrice);
            message.Append("<br/>");
            message.Append("<ul>"); 
            foreach(var item in cartDTO.CartDetailsDTOLists)
            {
                message.Append("<li>");
                message.Append(item.Product.Name + " " + item.Quantity);
                message.Append("</li>");
            }
            message.Append("</ul>");
            await LogAndEmail(message.ToString(), cartDTO.CartHeaderDTO.EmailId);
        }

        public async Task EmailNewUser(string user)
        {
            string message = "New User Registered <br/> Email: " + user;
            await LogAndEmail(message, "rups.choton@gmail.com");
        }
    }
}
