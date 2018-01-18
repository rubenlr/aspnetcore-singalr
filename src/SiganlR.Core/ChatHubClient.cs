using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace SiganlR.Core
{
    public class ChatHubClient
    {
        private readonly ILogger _logger;
        private HubConnection _connection;

        public delegate void SendEventHandler(string message);
        public event SendEventHandler OnSendEvent;

        public ChatHubClient(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger<ChatHubClient>();
        }

        public async Task<ChatHubClient> Start(string uri = "http://localhost:5000/chat")
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(uri)
                .WithConsoleLogger()
                .Build();

            _connection.On<string>("Send", OnSend);
            await _connection.StartAsync();

            return this;
        }

        private void OnSend(string message) 
            => OnSendEvent?.Invoke(message);

        public async Task<ChatHubClient> Registrar(string nome)
        {
            _logger.LogInformation($"{nome} se registrou");

            await _connection.InvokeAsync<string>("Registrar", nome);

            return this;
        }

        public async Task Send(string message)
        {
            _logger.LogInformation($"Mensagem enviada: {message}");

            await _connection.InvokeAsync<string>("Send", message);
        }
    }
}