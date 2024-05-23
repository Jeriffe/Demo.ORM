using AutoMapper;
using Demo.Data.Models;
using Demo.DTOs;
using Demo.Infrastructure;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application
{
    public class GetPatientQuery : IRequest<List<Patient>>
    {
        public PageFilter PageFilter { get; set; }
    }

    public class GetPatientQueryHandler : IRequestHandler<GetPatientQuery, List<Patient>>
    {
        private IMapper mapper;
        private IUnitOfWork unitOfWork;
        private IRepository<TPatient> entityRepository;
    
        public GetPatientQueryHandler(IUnitOfWork uow, IRepository<TPatient> repository, IMapper mapper)
        {
            unitOfWork = uow;
            entityRepository = repository;
            this.mapper = mapper;
        }

        public async Task<List<Patient>> Handle(GetPatientQuery request, CancellationToken cancellationToken)
        {
            var models = entityRepository.GetList();

            var dtos = mapper.Map<List<Patient>>(models);

            return dtos;
        }
    }
}
