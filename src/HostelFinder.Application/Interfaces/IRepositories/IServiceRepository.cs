﻿using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories
{
    public interface IServiceRepository : IBaseGenericRepository<Service>
    {
        Task<bool> CheckDuplicateServiceAsync(string serviceName);

        Task<IEnumerable<Service>> GetAllServiceAsync();

        Task<Service> GetServiceByIdAsync(Guid serviceId);

        Task<IEnumerable<Service>> GetServiceByRoomIdAsync(Guid roomId);
       
    }
}