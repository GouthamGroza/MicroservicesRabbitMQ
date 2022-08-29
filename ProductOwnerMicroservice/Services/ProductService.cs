using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductOwnerMicroservice.Data;
using ProductOwnerMicroservice.Models;
using ProductOwnerMicroservice.Utility;
using RabbitMQ.Client;
using System.Text;

namespace ProductOwnerMicroservice.Services
{
    public class ProductService : IProductService
    {
        private readonly DbContextClass _dbContext;
        public ProductService(DbContextClass dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ProductDetails> AddProductAsync(ProductDetails product)
        {
            var result = _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<ProductDetails> GetProductByIdAsync(int id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _dbContext.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<IEnumerable<ProductDetails>> GetProductListAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public bool SendProductOffer(ProductOfferDetail productOfferDetails)
        {
            var RabbitMQServer = "";
            var RabbitMQUserName = "";
            var RabbitMQPassword = "";
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                RabbitMQServer = Environment.GetEnvironmentVariable("RABBIT_MQ_SERVER");
                RabbitMQUserName = Environment.GetEnvironmentVariable("RABBIT_MQ_USERNAME");
                RabbitMQPassword = Environment.GetEnvironmentVariable("RABBIT_MQ_PASSWORD");
                
            }
            else
            {
                RabbitMQServer = StaticConfigurationManager.AppSetting["RabbitMQ:RabbitURL"];
                RabbitMQUserName = StaticConfigurationManager.AppSetting["RabbitMQ:Username"];
                RabbitMQPassword = StaticConfigurationManager.AppSetting["RabbitMQ:Password"];
            }
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = RabbitMQServer,
                    UserName = RabbitMQUserName,
                    Password = RabbitMQPassword
                };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {

                        channel.ExchangeDeclare(StaticConfigurationManager.AppSetting["RabbitMqSettins:ExchangeName"], StaticConfigurationManager.AppSetting["RabbitMqSettings:ExchangeType"]);
                        channel.QueueDeclare(queue: StaticConfigurationManager.AppSetting["RabbitMqSettings:QueueName"],
                            durable: true,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
                        channel.QueueBind(queue: StaticConfigurationManager.AppSetting["RabbitMqSettings:QueueName"], exchange: StaticConfigurationManager.AppSetting["RabbitMqSettings:ExchangeName"], routingKey: StaticConfigurationManager.AppSetting["RabbitMqSettings:RouteKey"]);

                        string productDetail = JsonConvert.SerializeObject(productOfferDetails);
                        var body = Encoding.UTF8.GetBytes(productDetail);

                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;

                        channel.BasicPublish(exchange: StaticConfigurationManager.AppSetting["RabbitMqSettings:ExhangeName"],
                         routingKey: StaticConfigurationManager.AppSetting["RabbitMqSettings:RouteKey"],
                         basicProperties: properties,
                         body: body
                         );
                        return true;
                    }


                }
            }
            catch (Exception)
            {
    
            }
            return false;
            

        }
    }
}
