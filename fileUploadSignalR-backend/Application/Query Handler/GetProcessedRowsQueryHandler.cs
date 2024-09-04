using Application.Command_Handler;
using Application.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query_Handler
{
    public class GetProcessedRowsQueryHandler : IRequestHandler<GetProcessedRowsQuery, IEnumerable<string>>
    {
        public Task<IEnumerable<string>> Handle(GetProcessedRowsQuery request, CancellationToken cancellationToken)
        {
            var rows = ProcessFileCommandHandler.GetProcessedRows();
            return Task.FromResult((IEnumerable<string>)rows);
        }
    }
}
