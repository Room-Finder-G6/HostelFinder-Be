using HostelFinder.Application.DTOs.InVoice.Responses;

namespace HostelFinder.Application.DTOs.Email;

public class SendEmailInvoice
{
    public static string BodyInvoiceEmail(InvoiceResponseDto invoice)
{
    var invoiceDate = DateTime.Now.ToString("dd/MM/yyyy");
    var billingPeriod = $"{invoice.BillingMonth}/{invoice.BillingYear}";
    var invoiceDetailsRows = string.Empty;

    foreach (var detail in invoice.InvoiceDetails)
    {
        invoiceDetailsRows += $@"
            <tr>
                <td>{detail.ServiceName}</td>
                <td style='text-align: right;'>{detail.UnitCost:N0} VND</td>
                <td style='text-align: right;'>{detail.ActualCost:N0} VND</td>
            </tr>";
    }
    
    string statusColor = invoice.IsPaid ? "green" : "red";
    string statusText = invoice.IsPaid ? "Đã Thanh Toán" : "Chưa Thanh Toán";
    string statusHtml = $"<span style='color: {statusColor}; font-weight: bold;'>{statusText}</span>";

    return $@"
        <!DOCTYPE html>
        <html lang='vi'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Hóa Đơn Phòng Trọ</title>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f7f7f7;
                    margin: 0;
                    padding: 0;
                }}
                .email-container {{
                    width: 100%;
                    max-width: 700px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    padding: 20px;
                    border-radius: 8px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                }}
                .header {{
                    background-color: #2c3e50;
                    color: #ffffff;
                    padding: 10px;
                    text-align: center;
                    border-radius: 8px 8px 0 0;
                }}
                .body {{
                    padding: 20px;
                    line-height: 1.6;
                }}
                .footer {{
                    text-align: center;
                    font-size: 12px;
                    color: #777777;
                    margin-top: 20px;
                }}
                table {{
                    width: 100%;
                    border-collapse: collapse;
                    margin-top: 20px;
                }}
                th, td {{
                    padding: 10px;
                    border: 1px solid #dddddd;
                }}
                th {{
                    background-color: #f2f2f2;
                }}
                .total {{
                    font-weight: bold;
                }}
            </style>
        </head>
        <body>
            <div class='email-container'>
                <div class='header'>
                    <h2>PhongTro247 - Hóa Đơn Phòng Trọ</h2>
                </div>
                <div class='body'>
                    <p>Kính gửi quý khách hàng,</p>
                    <p>Dưới đây là hóa đơn cho kỳ thanh toán <strong>{billingPeriod}</strong>:</p>
                    <p><strong>Ngày Lập Hóa Đơn:</strong> {invoiceDate}</p>
                    <table>
                        <thead>
                            <tr>
                                <th>Dịch Vụ</th>
                                <th>Đơn Giá</th>
                                <th>Thành Tiền</th>
                            </tr>
                        </thead>
                        <tbody>
                            {invoiceDetailsRows}
                            <tr>
                                <td colspan='2' class='total'>Tổng Cộng</td>
                                <td style='text-align: right;' class='total'>{invoice.TotalAmount} VND</td>
                            </tr>
                        </tbody>
                    </table>
                    <p><strong>Trạng Thái:</strong> {statusHtml}</p>
                    <p>Vui lòng thanh toán hóa đơn tháng <strong>{billingPeriod}</strong> để tránh bị phạt trễ hạn .</p>
                    <p>Trân trọng cảm ơn bạn đã sử dụng dịch vụ của chúng tôi!</p>
                </div>
                <div class='footer'>
                    <p>© 2024 PhongTro247. All rights reserved.</p>
                    <p>Địa chỉ: Hòa Lạc, Hà Nội</p>
                </div>
            </div>
        </body>
        </html>";
    }

        public static string BodyInvoiceSuccessEmail(InvoiceResponseDto invoice)
    {
    var invoiceDate = DateTime.Now.ToString("dd/MM/yyyy");
    var billingPeriod = $"{invoice.BillingMonth}/{invoice.BillingYear}";
    var invoiceDetailsRows = string.Empty;

    foreach (var detail in invoice.InvoiceDetails)
    {
        invoiceDetailsRows += $@"
            <tr>
                <td>{detail.ServiceName}</td>
                <td style='text-align: right;'>{detail.UnitCost:N0} VND</td>
                <td style='text-align: right;'>{detail.ActualCost:N0} VND</td>
            </tr>";
    }
    
    string statusColor = invoice.IsPaid ? "green" : "red";
    string statusText = invoice.IsPaid ? "Đã Thanh Toán" : "Chưa Thanh Toán";
    string statusHtml = $"<span style='color: {statusColor}; font-weight: bold;'>{statusText}</span>";


    return $@"
        <!DOCTYPE html>
        <html lang='vi'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Hóa Đơn Phòng Trọ</title>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f7f7f7;
                    margin: 0;
                    padding: 0;
                }}
                .email-container {{
                    width: 100%;
                    max-width: 700px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    padding: 20px;
                    border-radius: 8px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                }}
                .header {{
                    background-color: #2c3e50;
                    color: #ffffff;
                    padding: 10px;
                    text-align: center;
                    border-radius: 8px 8px 0 0;
                }}
                .body {{
                    padding: 20px;
                    line-height: 1.6;
                }}
                .footer {{
                    text-align: center;
                    font-size: 12px;
                    color: #777777;
                    margin-top: 20px;
                }}
                table {{
                    width: 100%;
                    border-collapse: collapse;
                    margin-top: 20px;
                }}
                th, td {{
                    padding: 10px;
                    border: 1px solid #dddddd;
                }}
                th {{
                    background-color: #f2f2f2;
                }}
                .total {{
                    font-weight: bold;
                }}
            </style>
        </head>
        <body>
            <div class='email-container'>
                <div class='header'>
                    <h2>PhongTro247 - Thông tin hóa đơn Phòng Trọ</h2>
                </div>
                <div class='body'>
                    <p>Kính gửi quý khách hàng,</p>
                    <p>Thông tin chi tiêt hóa đơn thanh toán <strong>{billingPeriod}</strong>:</p>
                    <p><strong>Ngày Lập Hóa Đơn:</strong> {invoiceDate}</p>
                    <p><strong>Trạng Thái:</strong> {statusHtml}</p>
                    <p><strong>Số tiền:</strong> {invoice.TotalAmount} VNĐ</p>
                    <p><strong> Hình thức thành toán : {invoice.FormOfTransfer} </strong></p>
                    <table>
                        <thead>
                            <tr>
                                <th>Dịch Vụ</th>
                                <th>Đơn Giá</th>
                                <th>Thành Tiền</th>
                            </tr>
                        </thead>
                        <tbody>
                            {invoiceDetailsRows}
                            <tr>
                                <td colspan='2' class='total'>Tổng Cộng</td>
                                <td style='text-align: right;' class='total'>{invoice.TotalAmount:N0} VND</td>
                            </tr>
                        </tbody>
                    </table>
                    <p>Trân trọng cảm ơn bạn đã sử dụng dịch vụ của chúng tôi!</p>
                </div>
                <div class='footer'>
                    <p>© 2024 PhongTro247. All rights reserved.</p>
                    <p>Địa chỉ: Hòa Lạc, Hà Nội</p>
                </div>
            </div>
        </body>
        </html>";
    }
    public const string SUBJECT_INVOICE = $"Hóa Đơn Phòng Trọ Của Bạn Từ PhongTro247";
    public const string SUBJECT_INVOICE_SUCCESS = $"Thông Tin Hóa Đơn Thanh Toán Phòng Trọ Từ PhongTro247";
}