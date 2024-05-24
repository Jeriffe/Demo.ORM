using AutoMapper;
using Demo.Data.Models;
using Demo.DTOs;
using Demo.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application
{
    public class GetPatientQuery : IRequest<Patient>
    {
        public int Id { get; set; }
    }

    public class GetPatientQueryHandler : IRequestHandler<GetPatientQuery, Patient>
    {
        private IMapper mapper;
        private IRepository<TPatient> entityRepository;

        public GetPatientQueryHandler(IRepository<TPatient> repository, IMapper mapper)
        {
            entityRepository = repository;
            this.mapper = mapper;
        }

        public async Task<Patient> Handle(GetPatientQuery request, CancellationToken cancellationToken)
        {
            var model = entityRepository.GetByKey(request.Id);

            var dto = mapper.Map<Patient>(model);

            return dto;
        }
    }
}
