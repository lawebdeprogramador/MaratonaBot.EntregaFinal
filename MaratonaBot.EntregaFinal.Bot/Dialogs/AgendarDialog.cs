using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MaratonaBot.EntregaFinal.API.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace MaratonaBot.EntregaFinal.Bot.Dialogs
{
    [Serializable]
    [LuisModel("****", "******")] //Remover
    public class AgendarDialog : LuisDialog<object>
    {
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Desculpe, não consegui entender a frase { result.Query }");
        }

        [LuisIntent("Cumprimento")]
        public async Task Cumprimento(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Olá, é um prazer falar com você!");
            await context.PostAsync($"Como posso ajudar?");
        }

        [LuisIntent("Agendar")]
        public async Task Agendar(IDialogContext context, LuisResult result)
        {

            Agenda agenda = new Agenda();
            agenda.Ocupado = true;
            agenda.Horario = DateTime.Now.AddHours(4);

            var endpoint = $"https://maratonabotentregafinalapi20180307011907.azurewebsites.net/api/";

            await context.PostAsync("Aguarde enquanto eu verifico algumas coisas, por favor.");

            //Validar as entidades da conversa se estiver OK fazer o agendamento.
            List<string> servicos = result.Entities.Where(e => e.Type == "Servicos").Select(e => e.Entity).ToList();

            //Validar serviço.
            if (servicos.Count == 0)
            {
                await context.PostAsync("Gostaria de agendar qual serviço?");
                return;
            }
            using (var client = new HttpClient())
            {
                string servico = servicos.First();
                var response = await client.GetAsync(endpoint + $"servicos/{servico}");
                if (!response.IsSuccessStatusCode)
                {
                    await context.PostAsync("Aconteceu algum problema, tente mais tarde!");
                    return;
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<Servico>(json);
                    if(resultado == null)
                    {
                        await context.PostAsync("Desculpe, não fazemos esse tipo de serviço!");
                        return;
                    }
                    else
                    {
                        agenda.Servico = resultado;
                    }
                }
            }

            //Verificar se o cliente existe se não existir criar.
            List<string> nomes = result.Entities.Where(e => e.Type == "Nome").Select(e => e.Entity).ToList();
            List<string> telefones = result.Entities.Where(e => e.Type == "Telefone").Select(e => e.Entity).ToList();

            if (nomes.Count == 0 || telefones.Count == 0)
            {
                await context.PostAsync("Qual o seu nome e telefone?");
                return;
            }

            Cliente cliente = new Cliente();
            using (var client = new HttpClient())
            {
                string nome = nomes.First();
                string telefone = String.Join(string.Empty,telefones.ToArray());

                var response = await client.GetAsync(endpoint + $"clientes/{nome}");
                if (!response.IsSuccessStatusCode)
                {
                    await context.PostAsync("Aconteceu algum problema, tente mais tarde!");
                    return;
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<Cliente>(json);
                    if (resultado == null)
                    {
                        cliente.Nome = nome;
                        cliente.Telefone = telefone;

                        var content = JsonConvert.SerializeObject(cliente);
                        var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        response = await client.PostAsync(endpoint + $"clientes", byteContent);
                        json = await response.Content.ReadAsStringAsync();
                        resultado = JsonConvert.DeserializeObject<Cliente>(json);

                        cliente = resultado;

                    }
                    else
                    {
                        cliente = resultado;
                    }
                }
            }
            agenda.Cliente = cliente;

            //Agendar
            using (var client = new HttpClient())
            {
                var content = JsonConvert.SerializeObject(agenda);
                var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(endpoint + $"agendas", byteContent);
                var json = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<Agenda>(json);

                await context.PostAsync("Obrigado por aguardar.");
                await context.PostAsync($"Agendado para {resultado.Horario.ToString("dd/MM/yyyy hh:mm")}");
                await context.PostAsync($"Código do agendamento {resultado.AgendaId}");
                return;

            }

        }

        
    }
}