namespace HostelFinder.Domain.Common.Constants
{
    public class EmailConstants
    {
        public static string BodyActivationEmail(string email) =>
              @"
                <!DOCTYPE html>
                <html lang=""en"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>Password Reset</title>
                    <style>
                        /* Reset styles */
                        body, html {
                            margin: 0;
                            padding: 0;
                            font-family: Arial, sans-serif;
                            line-height: 1.6;
                        }
                        /* Container styles */
                        .container {
                            max-width: 600px;
                            margin: 20px auto;
                            padding: 20px;
                            border: 1px solid #ccc;
                            border-radius: 10px;
                            background-color: #f9f9f9;
                        }
                        /* Heading styles */
                        h1 {
                            font-size: 24px;
                            text-align: center;
                            color: #333;
                        }
                        /* Paragraph styles */
                        p {
                            margin-bottom: 20px;
                            color: #666;
                        }
                        /* Button styles */
                        .btn {
                            display: inline-block;
                            padding: 10px 20px;
                            background-color: #007bff;
                            color: #fff;
                            text-decoration: none;
                            border-radius: 5px;
                        }
                        /* Footer styles */
                        .footer {
                            margin-top: 20px;
                            text-align: center;
                            color: #999;
                        }
                    </style>
                </head>
                <body>
                    <div class=""container"">
                        <p>Hello,</p>
                        <p>Welcome to Base Project. Thank you for using our servicesđe</p>
                        <p>To experience the service, please activate your account. Click the button below:</p>
                        <p><a href=""http://localhost:5000/Home/Resetpassword?userId=2}"" class=""btn"">Active Account</a></p>
                        <p>If you have any questions or need assistance, please contact our support team.</p>
                        <p>Thank you,</p>
                        <p>The Support Team</p>
                        <div class=""footer"">
                            <p>This is an automated message. Please do not reply.</p>
                        </div>
                    </div>
                </body>
                </html>
              ";

        public static string BodyResetPasswordEmail(string email, string token)
        {
            string resetLink = $"https://localhost:3000/reset-password?token={token}&email={email}";
            return $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    margin: 0;
                    padding: 0;
                    background-color: #f6f6f6;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    padding: 20px;
                    border-radius: 8px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                }}
                h2 {{
                    color: #333333;
                }}
                p {{
                    color: #666666;
                    line-height: 1.6;
                }}
                a.button {{
                    display: inline-block;
                    padding: 10px 20px;
                    margin-top: 20px;
                    background-color: #28a745;
                    color: #ffffff;
                    text-decoration: none;
                    font-weight: bold;
                    border-radius: 5px;
                }}
                a.button:hover {{
                    background-color: #218838;
                }}
                .footer {{
                    margin-top: 20px;
                    font-size: 12px;
                    color: #999999;
                    text-align: center;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h2>Reset your password</h2>
                <p>Hello, {email}</p>
                <p>We received a request to reset the password for the account associated with <strong>{email}</strong>. Click the button below to reset your password:</p>
                <p>
                    <a href='{resetLink}' class='button'>Reset Password</a>
                </p>
                <p>If you didn't request a password reset, please ignore this email. This link will expire in 1 hour.</p>
                <p>Thank you,<br/>Your Company Team</p>
                <div class='footer'>
                    <p>© 2024 Your Company. All rights reserved.</p>
                </div>
            </div>
        </body>
        </html>";
        }

        public const string SUBJECT_RESET_PASSWORD = "Reset Password";
        public const string SUBJECT_ACTIVE_ACCOUNT = "Active Email";
    }
}