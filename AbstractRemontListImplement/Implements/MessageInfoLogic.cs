using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.Interfaces;
using AbstractRemontBusinessLogic.ViewModels;
using AbstractRemontListImplement.Models;
using System;
using System.Collections.Generic;

namespace AbstractRemontListImplement.Implements
{
    public class MessageInfoLogic : IMessageInfoLogic
    {
        private readonly DataListSingleton source;

        public MessageInfoLogic()
        {
            source = DataListSingleton.GetInstance();
        }

        public void Create(MessageInfoBindingModel model)
        {
            MessageInfo tempMessageInfo = new MessageInfo { MessageId = "" };

            foreach (var messageInfo in source.MessageInfoes)
            {
                if (model.MassageId == messageInfo.MessageId)
                {
                    throw new Exception("Уже есть письмо с таким идентификатором");
                }
            }

            source.MessageInfoes.Add(CreateModel(model, tempMessageInfo));
        }

        public List<MessageInfoViewModel> Read(MessageInfoBindingModel model)
        {
            List<MessageInfoViewModel> result = new List<MessageInfoViewModel>();

            foreach (var messageInfo in source.MessageInfoes)
            {
                if (model != null)
                {
                    if (model.ClientId.HasValue && messageInfo.ClientId == model.ClientId)
                    {
                        result.Add(CreateViewModel(messageInfo));
                    }

                    continue;
                }

                result.Add(CreateViewModel(messageInfo));
            }

            return result;
        }

        private MessageInfo CreateModel(MessageInfoBindingModel model, MessageInfo MessageInfo)
        {
            int? clientId = null;

            foreach (var client in source.Clients)
            {
                if (model.ClientId.HasValue && model.ClientId == client.Id)
                {
                    clientId = model.ClientId;
                    break;
                }
            }

            MessageInfo.MessageId = model.MassageId;
            MessageInfo.ClientId = clientId;
            MessageInfo.SenderName = model.FromMailAddress;
            MessageInfo.DateDelivery = model.DateDelivery;
            MessageInfo.Subject = model.Subject;
            MessageInfo.Body = model.Body;

            return MessageInfo;
        }

        private MessageInfoViewModel CreateViewModel(MessageInfo MessageInfo)
        {
            return new MessageInfoViewModel
            {
                MassageId = MessageInfo.MessageId,
                SenderName = MessageInfo.SenderName,
                DateDelivery = MessageInfo.DateDelivery,
                Subject = MessageInfo.Subject,
                Body = MessageInfo.Body
            };
        }
    }
}
