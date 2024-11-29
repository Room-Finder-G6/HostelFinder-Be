﻿using HostelFinder.Application.DTOs.RevenueReport;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Services;

public class RevenueReportService : IRevenueReportService
{
    private readonly IInVoiceRepository _inVoiceRepository;
    private readonly IRoomRepository _roomRepository;
    public RevenueReportService(IInVoiceRepository inVoiceRepository, IRoomRepository roomRepository)
    {
        _inVoiceRepository = inVoiceRepository;
        _roomRepository = roomRepository;
    }
    public Task<Response<decimal>> GetRoomRevenueAsync(Guid roomId, int month, int year)
    {
        throw new NotImplementedException();
    }

    public Task<Response<decimal>> GetRoomRevenueByYearAsync(Guid roomId, int year)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// Báo cáo doanh thu của tất các phòng trong năm
    /// </summary>
    /// <param name="hostelId"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<Response<RoomRevenueReport>> GetYearlyRevenueReportByHostel(Guid hostelId, int year)
    {
        try
        {
            var rooms = await _roomRepository.GetRoomsByHostelAsync(hostelId);

            var reports = new RoomRevenueReport()
            {
                RoomRevenueDetail = new List<RoomRevenueDetailReport>()
            };
            decimal totalAllRevenue = 0;
            foreach (var room in rooms)
            {
                var roomRevenue = await _inVoiceRepository.GetRoomRevenueByYearAsync(room.Id, year);
                if (roomRevenue == 0)
                {
                    roomRevenue = 0;
                }
                reports.RoomRevenueDetail.Add(new RoomRevenueDetailReport()
                {
                    RoomName = room.RoomName,
                    TotalRevenue = roomRevenue,
                    Year = year
                });
                totalAllRevenue += roomRevenue;
            }

            reports.TotalAllRevenue = totalAllRevenue;

            return new Response<RoomRevenueReport> { Data = reports, Succeeded = true };
        }
        catch (Exception ex)
        {
            return new Response<RoomRevenueReport> { Succeeded = false, Message = ex.Message };
        }
    }

    public async Task<Response<RoomRevenueReport>> GetMonthlyRevenueReportByHostel(Guid hostelId, int month, int year)
    {
        try
        {
            var rooms = await _roomRepository.GetRoomsByHostelAsync(hostelId);
            var reports = new RoomRevenueReport()
            {
                RoomRevenueDetail = new List<RoomRevenueDetailReport>()
            };
            decimal totalAllRevenue = 0;
            foreach (var room in rooms)
            {
                var roomRevenue = await _inVoiceRepository.GetRoomRevenueByMonthAsync(room.Id,month, year);
                if (roomRevenue == 0)
                {
                    roomRevenue = 0;
                }
                reports.RoomRevenueDetail.Add(new RoomRevenueDetailReport()
                {
                    RoomName = room.RoomName,
                    TotalRevenue = roomRevenue,
                    Month = month,
                    Year = year
                });
                totalAllRevenue += roomRevenue;
            }

            reports.TotalAllRevenue = totalAllRevenue;

            return new Response<RoomRevenueReport> { Data = reports, Succeeded = true };
            
        }
        catch(Exception ex)
        {
            return new Response<RoomRevenueReport> { Succeeded = false, Message = ex.Message };
        }
    }

    public Task<Response<RoomRevenueReport>> GetEmptyRoomReport(Guid hostelId)
    {
        throw new NotImplementedException();
    }

}