using Application.Command;
using CsvHelper;
using MediatR;
using CsvHelper.Configuration;
using System.Globalization;
using Microsoft.AspNetCore.SignalR;
using Infrastructure.FileUploadHub;



namespace Application.Command_Handler
{
    public class ProcessFileCommandHandler : IRequestHandler<ProcessFileCommand,Unit>
    {
        private static List<string> _processedRows = new List<string>();
        private readonly IHubContext<FileProcessingHub> _hubContext;
        public ProcessFileCommandHandler(IHubContext<FileProcessingHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task<Unit> Handle(ProcessFileCommand request, CancellationToken cancellationToken)
        {
            using (var reader = new StreamReader(request.File.OpenReadStream()))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                // Read the header row
                if (csv.Read())
                {
                    csv.ReadHeader();
                    var headerRow = csv.HeaderRecord;

                    // Convert the header row into a string by concatenating all the fields
                    var headerData = string.Join(", ", headerRow);

                    // Send the header to the frontend
                    await _hubContext.Clients.All.SendAsync("ReceiveHeader", headerData, cancellationToken);
                }

                // Read each data row
                while (csv.Read())
                {
                    var row = csv.GetRecord<dynamic>();

                    // Convert the row into a string by concatenating all the fields
                    var rowData = string.Join(", ", ((IDictionary<string, object>)row).Values);

                    _processedRows.Add(rowData);
                    await Task.Delay(1000);

                    // Send the row data to connected clients via SignalR
                    await _hubContext.Clients.All.SendAsync("ReceiveRowUpdate", rowData, cancellationToken);
                }
            }

            return Unit.Value;
        }

        public static List<string> GetProcessedRows()
        {
            return _processedRows;
        }
    }
}
