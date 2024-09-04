using MediatR;

namespace Application.Queries
{
    public class GetProcessedRowsQuery : IRequest<IEnumerable<string>>
    {
    }
}
