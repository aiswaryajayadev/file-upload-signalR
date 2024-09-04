using MediatR;
using Microsoft.AspNetCore.Http;



namespace Application.Command
{
    public class ProcessFileCommand : IRequest
    {
        public IFormFile File { get; set; }

        public ProcessFileCommand(IFormFile file)
        {
            File = file;
        }
    }
}
