using AutoMapper;
using Demo.Data.Models;
using Demo.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Patients.Command
{
    public class DeletePatientCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand>
    {
        private IMapper mapper;
        private IRepository<TPatient> entityRepository;

        public DeletePatientCommandHandler(IRepository<TPatient> repository, IMapper mapper)
        {
            entityRepository = repository;
            this.mapper = mapper;
        }

        public Task<Unit> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
            var model = entityRepository.GetByKey(request.Id);

            entityRepository.Delete(model);

            return Task.FromResult(Unit.Value);
        }
    }
}
