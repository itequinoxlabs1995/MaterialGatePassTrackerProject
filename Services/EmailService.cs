using MaterialGatePassTracker.Models;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MaterialGatePassTacker.Models;
using System.Collections.Generic;
using System.Net.Http.Json;
using MaterialGatePassTracker.Interfaces;
using MaterialGatePassTacker;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs.Models;


namespace MaterialGatePassTracker.Services
{
    public class EmailService : IEmailService
    {
        private readonly HttpClient _httpClient;
        private readonly EmailSettings _emailSettings;
        private readonly MaterialDbContext _context;
        private string createdBy;

        public EmailService(HttpClient httpClient, IOptions<EmailSettings> emailSettings, MaterialDbContext context)
        {
            _httpClient = httpClient;
            _emailSettings = emailSettings.Value;
            _context = context;
        }

        public async Task<(string ToEmail, List<string> CcEmails)> GetStorekeeperEmailsAsync(string createdBy)
        {
            // Find the employee who raised the request
            var employee = await _context.Users
                .Where(d => d.User_Name == createdBy)
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                return (null, new List<string>());
            }

            // Find the project of the employee
            var project = await _context.UsersAttributes
                .Where(ua => ua.UID == employee.UID && ua.IsActive)
                .Select(ua => ua.PID)
                .FirstOrDefaultAsync();

            if (project == 0)
            {
                return (null, new List<string>());
            }

            // Find all employees mapped to the storekeeper role in the same project
            var storekeeperRole = await _context.Roles
                .Where(r => r.Role_Name == "Storekeeper")
                .Select(r => r.RID)
                .FirstOrDefaultAsync();

            if (storekeeperRole == 0)
            {
                return (null, new List<string>());
            }

            var storekeepers = await _context.UsersAttributes
                .Where(ua => ua.PID == project && ua.RID == storekeeperRole && ua.IsActive)
                .Select(ua => ua.UID)
                .ToListAsync();

            var storekeeperEmails = await _context.Users
                .Where(u => storekeepers.Contains(u.UID))
                .Select(u => u.Email_ID)
                .ToListAsync();

            return (employee.Email_ID, storekeeperEmails);
        }

        public async Task SendEmailAsync(string subject, string htmlBody,T_Gate_Pass request, List<string> filePaths)
        {
            var (toEmail, ccEmails) = await GetStorekeeperEmailsAsync(request.CreatedBy);

            if (string.IsNullOrEmpty(toEmail))
            {
                Console.WriteLine("No valid recipient found for the email.");
                return;
            }

            var emailData = new
            {
                to = new List<string> { toEmail },
                cc = ccEmails,
                bcc = _emailSettings.BCCRecipients ?? new List<string>(),
                subject = subject,
                html = htmlBody
            };

            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(emailData), Encoding.UTF8, "application/json");

                if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", _emailSettings.AuthKey);
                }

                var response = await _httpClient.PostAsync(_emailSettings.ApiUrl, jsonContent);

                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Body: {responseBody}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Email API error: {response.StatusCode} - {responseBody}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email Notification Failed: {ex.Message}");
            }
        }

    }


}
