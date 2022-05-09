using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobcity.Chat.Domain
{
    public class ChatMessage
    {
        public string UserName { get; set; }
        public string DateTime { get; set; }
        public string Message { get; set; }
    }
}
