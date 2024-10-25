﻿using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;

namespace HostelFinder.Infrastructure.Repositories
{
    public class ServiceCostRepository : BaseGenericRepository<ServiceCost>, IServiceCostRepository
    {
        public ServiceCostRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }
    }
}
