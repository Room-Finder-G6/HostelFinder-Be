﻿using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories
{
    public interface IImageRepository : IBaseGenericRepository<Image>
    {
        Task<Image> GetImageUrlByRoomId(Guid roomId);
    }
}
