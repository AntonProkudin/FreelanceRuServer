using FreelanceRuHub;
using Microsoft.AspNetCore.SignalR;
using ServiceApi.Helpers;
using ServiceApi.Wrappers;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MauiSignalRSample.Server
{
    public class ChatHub : Hub
    {
        public JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public HttpClientHandler handler = new HttpClientHandler()
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            }
        };
        public async System.Threading.Tasks.Task SendMessageToApi(Message req)
        {
            try
            {
                var client = new HttpClient(handler);
                client.BaseAddress = new Uri($"https://{ServerInfo.GetLocalIPAddress()}:7124");
                
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HubWrapper.tokenMap[Context.ConnectionId]);
                string json = System.Text.Json.JsonSerializer.Serialize(req, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await client.PostAsync("/api/Message", content);
                result.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {

            }
        }
        public async Task SendMessage(Message m)
        {
            await SendMessageToApi(m);
            if(HubWrapper.connectionMap.ContainsKey(m.ToUserId))
               await Clients.Clients(HubWrapper.connectionMap[m.ToUserId]).SendAsync("ReceiveMessage", m).ConfigureAwait(false);
            
        }
        public override async Task OnConnectedAsync()
        {
            var httpCtx = Context.GetHttpContext();
            var someHeaderValue = httpCtx.Request.Headers["Authorization"].ToString();
            var UserId = int.Parse(FindId(someHeaderValue.Replace("Bearer ", "")));
            HubWrapper.connectionMap.TryAdd(UserId, Context.ConnectionId);
            HubWrapper.tokenMap.TryAdd(Context.ConnectionId, someHeaderValue.Replace("Bearer ", ""));
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            foreach (var user in HubWrapper.connectionMap.Where(x => x.Value == Context.ConnectionId))
            {
                HubWrapper.connectionMap.TryRemove(user);
            }
            await base.OnDisconnectedAsync(exception);
        }
        public string FindId(string stream) 
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;
            return tokenS.Claims.First(claim => claim.Type == "nameid").Value;
        }

    }
}
