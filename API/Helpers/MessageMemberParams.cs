using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class MessageMemberParams : PaginationParams
    {

        public string currentUsername  { get; set; }

        public string recipientUsername { get; set; }  
    }
}
