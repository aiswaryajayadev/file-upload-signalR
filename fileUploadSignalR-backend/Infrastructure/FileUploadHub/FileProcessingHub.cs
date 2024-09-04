using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.FileUploadHub
{
    public class FileProcessingHub : Hub
    {
        public async Task SendRowUpdate(string row)
        {
            await Clients.All.SendAsync("ReceiveRowUpdate", row);
        }
        public async Task SendHeaderUpdate(string header)
        {
            await Clients.All.SendAsync("ReceiveHeader", header);
        }
    }
}
