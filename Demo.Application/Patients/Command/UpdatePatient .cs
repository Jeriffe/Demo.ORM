using AutoMapper;
using Demo.Data.Models;
using Demo.DTOs;
using Demo.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Patients.Command
{
    public class UpdatePatientCommand : IRequest
    {
        public Patient Patient { get; set; }
    }

    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand>
    {
        private IMapper mapper;
        private IRepository<TPatient> entityRepository;

        public UpdatePatientCommandHandler(IRepository<TPatient> repository, IMapper mapper)
        {
            entityRepository = repository;
            this.mapper = mapper;
        }

        public Task<Unit> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var model = mapper.Map<TPatient>(request.Patient);

            entityRepository.Update(model);

            return Task.FromResult(Unit.Value);
        }
    }
}
