using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SiganlR.Core;

namespace SiganlR.Console
{
    class MessageHandler
    {
        private readonly IChatSettings _chatSettings;
        private ChatHubClient _clientHub;

        public MessageHandler(IChatSettings chatSettings)
        {
            _chatSettings = chatSettings;
        }

        public async Task Iniciar()
        {
            Start().Wait();
            await Console();
        }

        private async Task Start()
        {
            System.Console.WriteLine("Digite seu nome para cadastrar-se: ");
            var nome = System.Console.ReadLine();

            System.Console.WriteLine("Iniciando chat...");

            _clientHub = new ChatHubClient(new LoggerFactory());

            _clientHub.OnSendEvent += System.Console.WriteLine;

            await _clientHub.Start(_chatSettings.Urls);

            System.Console.WriteLine("Chat Inicializado.");

            System.Console.WriteLine("Bem vindo {0}, seu cadastro foi concluido com sucesso.", nome);
            System.Console.WriteLine(@"Quando quiser sair, basta digitar ""sair"" ");

            await _clientHub.Registrar(nome);
        }

        private async Task Console()
        {
            for (;;)
            {
                var message = System.Console.ReadLine();

                if (message == "sair")
                    break;

                await _clientHub.Send(message);
            }
        }
    }
}