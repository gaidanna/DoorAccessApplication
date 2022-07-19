using DoorAccessApplication.Core.Interfaces;
using DoorAccessApplication.Core.Models;
using DoorAccessApplication.Model;
using Newtonsoft.Json;
using Plain.RabbitMQ;

namespace DoorAccessApplication.Api.Listeners
{
    public class UserCreatedListener : IHostedService
    {
        private readonly IPublisher _publisher;
        private readonly ISubscriber _subscriber;
        private readonly IServiceProvider _serviceProvider;
        public UserCreatedListener(IPublisher publisher, ISubscriber subscriber, IServiceProvider serviceProvider)
        {
            this._publisher = publisher;
            this._subscriber = subscriber;
            this._serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _subscriber.Subscribe(Subscribe);
            return Task.CompletedTask;
        }

        private bool Subscribe(string message, IDictionary<string, object> header)
        {
            var response = JsonConvert.DeserializeObject<UserRequest>(message);
            try
            {
                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    var _userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                    var user = _userRepository.CreateAsync(new User() { Id = response.Id, Name = response.Name, LastName = response.LastName, Email = response.Email }).Result;

                    _publisher.Publish(JsonConvert.SerializeObject(
                   new UserResponse 
                   { 
                       Id = response.Id, 
                       IsSuccess = true
                   }),
                   "user.response",
                   null);
                }
            }
            catch (Exception)
            {
                _publisher.Publish(JsonConvert.SerializeObject(
                    new UserResponse { Id = response.Id, IsSuccess = false }
                    ), "user.response", null);
            }

            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
