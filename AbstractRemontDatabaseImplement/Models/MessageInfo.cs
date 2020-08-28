using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractRemontDatabaseImplement.Models
{
    /// Сообщения, приходящие на почту
    public class MessageInfo
    {
        [Key]
        public string MassageId { get; set; }
        public int? ClientId { get; set; }
        public string SenderName { get; set; }
        public DateTime DateDelivery { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public virtual Client Client { get; set; }
    }
}
