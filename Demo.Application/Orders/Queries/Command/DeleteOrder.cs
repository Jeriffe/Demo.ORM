using AutoMapper;
using Demo.Data.Models;
using Demo.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Orders.Command
{
    public class DeleteOrderCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private IMapper mapper;
        private IRepository<TOrder> entityRepository;

        public DeleteOrderCommandHandler(IRepository<TOrder> repository, IMapper mapper)
        {
            entityRepository = repository;
            this.mapper = mapper;
        }

        public Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var model = entityRepository.GetByKey(request.Id);

            entityRepository.Delete(model);

            return Task.FromResult(Unit.Value);
        }
    }
}
