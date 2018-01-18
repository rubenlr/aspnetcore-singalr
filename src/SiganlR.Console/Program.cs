using System.IO;
using Microsoft.Extensions.Configuration;

namespace SiganlR.Console
{
    public interface IBuilderProvider
    {
        IConfigurationRoot GetProvider { get; }
    }

    class BuilderProvider : IBuilderProvider
    {
        public IConfigurationRoot GetProvider => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
    }

    public interface IChatSettings
    {
        string Urls { get; }
    }

    class ChatSettings : IChatSettings
    {
        private readonly IConfigurationRoot _configuration;

        public ChatSettings(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public string Urls => $"{_configuration["chat:hub-url"]}";
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var chatHandler = new MessageHandler(new ChatSettings(new BuilderProvider().GetProvider));

            chatHandler.Iniciar().Wait();
        }
    }
}
