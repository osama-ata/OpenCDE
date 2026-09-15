using Dangl.OpenCDE.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dangl.OpenCDE.Data.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CdeDbContext _context;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor,
            CdeDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public Task<bool> UserIsAuthenticatedAsync()
        {
            var isAuthenticated = _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
            return Task.FromResult(isAuthenticated);
        }

        public async Task<Guid> GetCurrentUserIdAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var subClaim = user?.Claims.FirstOrDefault(c => c.Type == "sub");
            if (subClaim == null || !Guid.TryParse(subClaim.Value, out var userId))
            {
                throw new InvalidOperationException("There is no authenticated user with a valid 'sub' claim in the current request.");
            }

            // The Supabase-issued token is the source of truth for identity; we just
            // need a local row to satisfy foreign keys from documents/sessions/files.
            var emailClaim = user.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (existingUser == null)
            {
                _context.Users.Add(new CdeUser
                {
                    Id = userId,
                    Email = emailClaim
                });
                await _context.SaveChangesAsync();
            }
            else if (!string.IsNullOrWhiteSpace(emailClaim) && existingUser.Email != emailClaim)
            {
                existingUser.Email = emailClaim;
                await _context.SaveChangesAsync();
            }

            return userId;
        }

        public async Task<string> GetCurrentUserNameAsync()
        {
            var emailClaim = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            if (!string.IsNullOrWhiteSpace(emailClaim))
            {
                return emailClaim;
            }

            var userId = await GetCurrentUserIdAsync();
            return userId.ToString();
        }
    }
}
