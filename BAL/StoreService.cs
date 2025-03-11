using MaterialGatePassTacker;
using MaterialGatePassTacker.Models;
using MaterialGatePassTracker.DAL;
using MaterialGatePassTracker.Models;
using MaterialGatePassTracker.Services;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace MaterialGatePassTracker.BAL
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepo _repository;
        private readonly EmailService _emailService;
        private readonly MaterialDbContext _context;

        public StoreService(IStoreRepo repository, EmailService emailService, MaterialDbContext context)
        {
            _repository = repository;
            _emailService = emailService;
            _context = context;
        }

        public async Task<List<T_Gate_Pass>> GetGatePassesByUserIdAsync(int userId)
        {
            try 
            {
                return await _repository.GetGatePassesByUserIdAsync(userId);
            }
            
            catch (Exception ex)
            {
                Console.WriteLine($"[Business] Error getting userdetails {userId}: {ex.Message}");
                return null; // Return null to indicate failure
            }
        }

        public async Task UpdateGatePassStatusAsync(GatePassStatusRequest request)
        {
            await _repository.UpdateGatePassStatusAsync(request);
            try 
            {
                if (request.Action.ToLower() == "reassign")
                {
                    var reassignedUserEmail = await _repository.GetUserEmailByUserNameAsync(request.Store);

                    if (!string.IsNullOrEmpty(reassignedUserEmail))
                    {
                        await _emailService.SendStoreEmailAsync( 
                           "Gate Pass Reassigned",
                           $@"
                            <p>Dear Manager,</p>                            
                            <p>Gate Pass {request.GatePassId} has been reassigned to you.</p>
                            <p>Best regards,<br/>IT Support</p>",
                            reassignedUserEmail);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Business] Error getting UserdateGatePass Status {request}: {ex.Message}");
            }

        }
    }
}
