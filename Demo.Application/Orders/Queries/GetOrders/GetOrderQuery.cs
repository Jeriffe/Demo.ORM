﻿using AutoMapper;
using Demo.Data.Models;
using Demo.DTOs;
using Demo.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application
{
    public class GetOrderQuery : IRequest<Order>
    {
        public long Id { get; set; }
    }

    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, Order>
    {
        private IMapper mapper;
        private IRepository<TOrder> entityRepository;

        public GetOrderQueryHandler(IRepository<TOrder> repository, IMapper mapper)
        {
            entityRepository = repository;
            this.mapper = mapper;
        }

        public async Task<Order> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var model = entityRepository.GetByKey(request.Id);

            var dto = mapper.Map<Order>(model);

            return dto;
        }
    }
}