using System;
using System.Threading.Tasks;

namespace Dangl.OpenCDE.Data.Services
{
    /// <summary>
    /// Resolves the caller's identity from the verified bearer token (a Supabase-issued
    /// JWT) on the current request, replacing the previous Dangl Identity user service.
    /// </summary>
    public interface ICurrentUserService
    {
        Task<bool> UserIsAuthenticatedAsync();

        Task<Guid> GetCurrentUserIdAsync();

        Task<string> GetCurrentUserNameAsync();
    }
}
