﻿using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Exceptions;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories
{
    public class MeterReadingRepository : BaseGenericRepository<MeterReading>, IMeterReadingRepository
    {
        public MeterReadingRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<MeterReading> GetCurrentMeterReadingAsync(Guid roomId, Guid serviceId, int billingMonth, int billingYear)
        {
            var meterReading = await _dbContext.MeterReadings.FirstOrDefaultAsync(
                                                                mr => mr.RoomId == roomId &&
                                                                mr.ServiceId == serviceId &&
                                                                mr.BillingMonth == billingMonth &&
                                                                mr.BillingYear == billingYear &&
                                                                !mr.IsDeleted);
            //if (meterReading == null)
            //{
            //    throw new NotFoundException($"Không tìm thấy số điện và số nước ở phòng {roomId} ");
            //}

            return meterReading;
        }

        public async Task<MeterReading?> GetPreviousMeterReadingAsync(Guid roomId, Guid serviceId, int billingMonth, int billingYear) 
{
    int previousMonth = billingMonth == 1 ? 12 : billingMonth - 1;
    int previousYear = billingMonth == 1 ? billingYear - 1 : billingYear;

    MeterReading? meterReading = null;

    // Kiểm tra các tháng gần nhất, nếu không có dữ liệu thì lùi về tháng trước nữa
    while (meterReading == null && (previousYear > 0 || previousMonth > 0))
    {
        meterReading = await _dbContext.MeterReadings
            .FirstOrDefaultAsync(mr => 
                mr.RoomId == roomId &&
                mr.ServiceId == serviceId &&
                mr.BillingMonth == previousMonth &&
                mr.BillingYear == previousYear &&
                !mr.IsDeleted);

        if (meterReading == null)
        {
            // Lùi về tháng trước nữa
            previousMonth = previousMonth == 1 ? 12 : previousMonth - 1;
            if (previousMonth == 12)
                previousYear--; // Lùi về năm trước nếu tháng là tháng 12
        }
    }

    return meterReading;
}

    }
}
