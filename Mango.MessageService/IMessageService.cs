using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.MessageService
{
    public interface IMessageService
    {
        Task PublishMessage(object message, string topic_queue_name);

    }
}
