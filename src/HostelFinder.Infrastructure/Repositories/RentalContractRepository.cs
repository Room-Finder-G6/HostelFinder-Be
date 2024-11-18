﻿using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;

namespace HostelFinder.Infrastructure.Repositories
{
    public class RentalContractRepository : BaseGenericRepository<RentalContract>, IRentalContractRepository
    {
        public RentalContractRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }
    }
}
