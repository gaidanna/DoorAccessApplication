using DoorAccessApplication.Ids.Models;
using DoorAccessApplication.Model;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Plain.RabbitMQ;

namespace DoorAccessApplication.Ids.Listeners
{
    public class DoorAccessResponseListener : IHostedService
    {
        private readonly ISubscriber _subscriber;
        private readonly UserManager<ApplicationUser> _userManager;
        public DoorAccessResponseListener(ISubscriber subscriber, 
            UserManager<ApplicationUser> userManager)
        {
            _subscriber = subscriber;
            _userManager = userManager;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _subscriber.Subscribe(Subscribe);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private bool Subscribe(string message, IDictionary<string, object> header)
        {
            var response = JsonConvert.DeserializeObject<UserResponse>(message);
            if (!response.IsSuccess)
            {
                //delete user here
            }
            return true;
        }
    }


}
