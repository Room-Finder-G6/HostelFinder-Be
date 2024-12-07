﻿using Azure;
using HostelFinder.Application.DTOs.RevenueReport;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices;

public interface IRevenueReportService
{
    /// <summary>
    /// Doanh thu phòng theo tháng
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="month"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    Task<Wrappers.Response<decimal>> GetRoomRevenueAsync(Guid roomId, int month, int year);
    /// <summary>
    /// Doanh thu phòng theo năm
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    Task<Wrappers.Response<decimal>> GetRoomRevenueByYearAsync(Guid roomId, int year);
    
    Task<Wrappers.Response<RoomRevenueReport>> GetYearlyRevenueReportByHostel(Guid hostelId, int year);
    
    Task<Wrappers.Response<RoomRevenueReport>> GetMonthlyRevenueReportByHostel(Guid hostelId, int month, int year);
    

    // Report những phòng đang trống
    Task<Wrappers.Response<RoomRevenueReport>> GetEmptyRoomReport(Guid hostelId);
    
    

}