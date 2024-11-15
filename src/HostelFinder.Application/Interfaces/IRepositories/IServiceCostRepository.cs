﻿using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;
using System.Linq.Expressions;

namespace HostelFinder.Application.Interfaces.IRepositories
{
    public interface IServiceCostRepository : IBaseGenericRepository<ServiceCost>
    {
        Task<ServiceCost> CheckExistingServiceCostAsync(Guid hostelId, Guid serviceId, DateTime effectiveFrom);
        Task<List<ServiceCost>> GetAllServiceCostListAsync();

        Task<List<ServiceCost>> GetAllServiceCostListWithConditionAsync(Expression<Func<ServiceCost, bool>> filter);
    }
}
