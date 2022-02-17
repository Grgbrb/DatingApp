using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        
        void DeleteMessage(Message message);

        Task<Message> GetMessage(int Id);

        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);

        Task<PagedList<MessageDto>> GetMessageThread(MessageMemberParams messageMemberParams);

        Task<bool> SaveAllAsync();
    }
}