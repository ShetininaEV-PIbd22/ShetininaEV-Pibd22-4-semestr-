using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.Interfaces;
using AbstractRemontBusinessLogic.ViewModels;
using AbstractRemontFileImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractRemontFileImplement.Implements
{
    public class MessageInfoLogic : IMessageInfoLogic
    {
        private readonly FileDataListSingleton source;

        public MessageInfoLogic()
        {
            source = FileDataListSingleton.GetInstance();
        }

        public void Create(MessageInfoBindingModel model)
        {
            MessageInfo element = source.MessageInfoes.FirstOrDefault(rec => rec.MessageId == model.MassageId);

            if (element != null)
            {
                throw new Exception("Уже есть письмо с таким идентификатором");
            }

            int? clientId = source.Clients.FirstOrDefault(rec => rec.Login == model.FromMailAddress)?.Id;

            source.MessageInfoes.Add(new MessageInfo
            {
                MessageId = model.MassageId,
                ClientId = clientId,
                SenderName = model.FromMailAddress,
                DateDelivery = model.DateDelivery,
                Subject = model.Subject,
                Body = model.Body
            });
        }
        public List<MessageInfoViewModel> Read(MessageInfoBindingModel model)
        {
            return source.MessageInfoes
                .Where(rec => model == null || rec.ClientId == model.ClientId)
                .Select(rec => new MessageInfoViewModel
                {
                    MassageId = rec.MessageId,
                    SenderName = rec.SenderName,
                    DateDelivery = rec.DateDelivery,
                    Subject = rec.Subject,
                    Body = rec.Body
                })
               .ToList();
        }
    }
}
