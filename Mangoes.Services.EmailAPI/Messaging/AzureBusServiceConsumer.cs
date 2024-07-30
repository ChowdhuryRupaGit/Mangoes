using Azure.Messaging.ServiceBus;
using Mangoes.Services.EmailAPI.Model;
using Mangoes.Services.EmailAPI.Model.DTO;
using Mangoes.Services.EmailAPI.Services;
using Newtonsoft.Json;
using System.Text;

namespace Mangoes.Services.EmailAPI.Messaging
{
    public class AzureBusServiceConsumer : IAzureBusServiceConsumer
    {
        private ServiceBusProcessor serviceBusProcessor;
        private ServiceBusProcessor serviceBusProcessorForNewUser;
        private IConfiguration configuration;
        private string connectiontring;
        private string topic_name_queue;
        private string topic_queue_newUser;
        private EmailServices _emailServices;

        public AzureBusServiceConsumer(IConfiguration _configuration, EmailServices emailServices)
        {
            configuration = _configuration;
            connectiontring = _configuration.GetValue<string>("ConnectionStringAccess:Private_Access");
            topic_name_queue = _configuration.GetValue<string>("TopicsAndQueueNames:EmailShoppingCartQueue");
            topic_queue_newUser = _configuration.GetValue<string>("TopicsAndQueueNames:EmailNewUser");
            var client = new ServiceBusClient(connectiontring);
            serviceBusProcessor = client.CreateProcessor(topic_name_queue);
            serviceBusProcessorForNewUser = client.CreateProcessor(topic_queue_newUser);
            _emailServices = emailServices;
        }

        public async Task Start()
        {
            serviceBusProcessorForNewUser.ProcessMessageAsync += ServiceBusProcessorForNewUser_ProcessMessageAsync;
            serviceBusProcessorForNewUser.ProcessErrorAsync += ServiceBusProcessorForNewUser_ProcessErrorAsync;
             await serviceBusProcessorForNewUser.StartProcessingAsync();

            serviceBusProcessor.ProcessMessageAsync += ServiceBusProcessor_ProcessMessageAsync;
            serviceBusProcessor.ProcessErrorAsync += ServiceBusProcessor_ProcessErrorAsync;
            await serviceBusProcessor.StartProcessingAsync();
        }

        private Task ServiceBusProcessorForNewUser_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            Console.WriteLine(arg.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task ServiceBusProcessorForNewUser_ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            var message = arg.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            string user = JsonConvert.DeserializeObject<string>(body);
            try
            {
                await _emailServices.EmailNewUser(user);
                await arg.CompleteMessageAsync(arg.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private Task ServiceBusProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            Console.WriteLine(arg.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task ServiceBusProcessor_ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
           var message = arg.Message;
           var body = Encoding.UTF8.GetString(message.Body);
           CartDTO cartDTO = JsonConvert.DeserializeObject<CartDTO>(body);
            try
            {
                await _emailServices.EmailCartAndLog(cartDTO);
                await arg.CompleteMessageAsync(arg.Message);
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

        public async Task Stop()
        {
            
            await serviceBusProcessor.StopProcessingAsync();
            await serviceBusProcessor.DisposeAsync();
        }
    }
}
