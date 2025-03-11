using MaterialGatePassTacker;
using MaterialGatePassTacker.Models;
using MaterialGatePassTracker.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace MaterialGatePassTracker.DAL
{
    public class StoreRepo : IStoreRepo
    {
        private readonly MaterialDbContext _context;

        public StoreRepo(MaterialDbContext context)
        {
            _context = context;
        }

        public async Task<List<T_Gate_Pass>> GetGatePassesByUserIdAsync(int userId)
        {
            try
            {
                var projectIds = await _context.UsersAttributes
                                               .Where(ua => ua.UID == userId)
                                               .Select(ua => ua.PID)
                                               .ToListAsync();

                return await _context.GatesPasses
                                     .Where(gp => projectIds.Contains(gp.PID))
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Repository] Error fetching gate passes for UserID {userId}: {ex.Message}");
                return new List<T_Gate_Pass>(); // Return an empty list on failure
            }
        }


        public async Task UpdateGatePassStatusAsync(GatePassStatusRequest request)
        {
            try 
            {
                var gatePassStatus = await _context.GatesPassesStatus
                                               .FirstOrDefaultAsync(gps => gps.GPID == request.GatePassId);

                if (gatePassStatus == null)
                {
                    gatePassStatus = new T_Gate_Pass_Status
                    {
                        GPID = request.GatePassId,
                        CreatedOn = DateTime.Now
                    };
                    _context.GatesPassesStatus.Add(gatePassStatus);
                }

                gatePassStatus.ModifiedOn = DateTime.Now;

                switch (request.Action.ToLower())
                {
                    case "assign":
                        // Step 1: Get the Project ID (PID) from the Gate Pass table
                        var projectId = await _context.GatesPasses
                                                      .Where(gp => gp.Gate_ID == request.GatePassId)
                                                      .Select(gp => gp.PID)
                                                      .FirstOrDefaultAsync();

                        if (projectId == 0)
                        {
                            Console.WriteLine($"[Repository] No Project found for GatePassId: {request.GatePassId}");
                            return;
                        }

                        // Step 2: Get the User Name associated with the Project
                        var userName = await (from ua in _context.UsersAttributes
                                              join u in _context.Users on ua.UID equals u.UID
                                              where ua.PID == projectId
                                              select u.User_Name)
                                             .FirstOrDefaultAsync();

                        if (string.IsNullOrEmpty(userName))
                        {
                            Console.WriteLine($"[Repository] No User found for ProjectId: {projectId}");
                            return;
                        }

                        // Step 3: Get the Store based on the Project
                        var storeDescription = await _context.Store
                                      .Where(s => s.PID == projectId)
                                      .Select(s => s.Store_Name_Desc)
                                      .FirstOrDefaultAsync() ?? "Unknown Store";


                        // Assign user and store
                        gatePassStatus.AssignUser = userName;
                        gatePassStatus.AssignStore = storeDescription;
                        gatePassStatus.Status = "Assigned";
                        break;

                    case "reject":
                        gatePassStatus.Status = "Rejected";
                        break;

                    case "reassign":
                        gatePassStatus.ReassignUser = request.Store;
                        // Get UserId based on the given username in request.Store
                        var userId = await _context.Users
                            .Where(u => u.User_Name.ToLower().Trim() == request.Store.ToLower().Trim())
                            .Select(u => u.UID)
                            .FirstOrDefaultAsync();

                        if (userId == 0)
                        {
                            throw new Exception("User not found for the given username.");
                        }

                        // Find the store where this user is assigned (from User_Attribute table)
                        var storeId = await _context.UsersAttributes
                            .Where(ua => ua.UID == userId)
                            .Select(ua => ua.PID) // Assuming PID links to the store
                            .FirstOrDefaultAsync();

                        if (storeId == 0)
                        {
                            throw new Exception("No store found for this user.");
                        }

                        // Get store description instead of store ID
                        gatePassStatus.ReassignStore = await _context.Store
                            .Where(s => s.PID == storeId) // Assuming PID links Store to User_Attribute
                            .Select(s => s.Store_Name_Desc) // Select Store Description
                            .FirstOrDefaultAsync() ?? "Unknown Store";

                        gatePassStatus.Status = "Reassigned";
                        break;
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Repository] Error fetching  UpdateGatePass Status: {ex.Message}");
            }
        }

        public async Task<string?> GetUserEmailByUserNameAsync(string userName)
        {
            try 
            {
                return await _context.Users
               .Where(u => u.User_Name == userName)
               .Select(u => u.Email_ID)
               .FirstOrDefaultAsync();
            }
           
            catch (Exception ex)
            {
                Console.WriteLine($"[Repository] Error fetching email: {ex.Message}");
                return null; // Return null if an error occurs
            }
        }

    }
}
