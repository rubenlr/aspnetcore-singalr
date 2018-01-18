using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace SiganlR.Core
{
    public class ChatHub : Hub
    {
        private readonly ILogger _logger;
        private static readonly UserDataStore UserData = new UserDataStore();

        public ChatHub(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger<ChatHub>();
        }

        public async Task Send(string message)
        {
            _logger.LogInformation($"{UserData.GetName(Context)}: {message}");

            await SendOthersRaw(message);
        }

        private async Task SendAllRaw(string message)
            => await Clients.All.InvokeAsync("Send", message);

        private async Task SendOthersRaw(string message)
            => await Clients.Others.InvokeAsync("Send", message);

        public async Task Registrar(string nome)
        {
            _logger.LogInformation($"{nome} se registrou");

            UserData.Store(new UserData { Id = Context.ConnectionId, Nome = nome });

            await SendAllRaw($"{nome} entrou no grupo.");
        }
    }
}
