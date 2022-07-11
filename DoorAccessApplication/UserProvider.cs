//using DoorAccessApplication.Core.Models;
//using Newtonsoft.Json;

//namespace DoorAccessApplication.Api
//{
//    public class UserProvider : IUserProvider
//    {
//        private readonly IHttpClientFactory httpClientFactory;
//        private readonly ILogger<UserProvider> logger;

//        public OrderDetailsProvider(IHttpClientFactory httpClientFactory,
//            ILogger<UserProvider> logger)
//        {
//            this.httpClientFactory = httpClientFactory;
//            this.logger = logger;
//        }

//        public async Task<User> Get()
//        {
//            try
//            {
//                using var client = httpClientFactory.CreateClient("identity");
//                var response = await client.GetAsync("/api/identity");
//                var data = await response.Content.ReadAsStringAsync();
//                return JsonConvert.DeserializeObject<User>(data);
//            }
//            catch (System.Exception exc)
//            {
//                // Log the exception
//                logger.LogError($"Error getting user {exc}");
//                return null;
//            }
//        }
//    }
//}
