using AutoMapper;
using Demo.Data.Models;
using Demo.DTOs;
using Demo.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Patients.Command
{
    public class CreatePatientCommand : IRequest<Patient>
    {
        public Patient Patient { get; set; }
    }

    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Patient>
    {
        private IMapper mapper;
        private IRepository<TPatient> entityRepository;

        public CreatePatientCommandHandler(IRepository<TPatient> repository, IMapper mapper)
        {
            entityRepository = repository;
            this.mapper = mapper;
        }

        public async Task<Patient> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var model = mapper.Map<TPatient>(request.Patient);

            var dbModel = entityRepository.Create(model);

            var dto = mapper.Map<Patient>(dbModel);

            return dto;
        }
    }
}
