using Dangl.Data.Shared;
using Dangl.OpenCDE.Data.Services;
using Dangl.OpenCDE.Shared.Models.Foundations;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Dangl.OpenCDE.Core.Controllers.Foundations
{
    [Route("foundation/1.0/current-user")]
    public class CurrentUserController : CdeAppControllerBase
    {
        private readonly ICurrentUserService _currentUserService;

        public CurrentUserController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(UserGet), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiBehaviorOptions), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCurrentUserDataAsync()
        {
            var userIsAuthenticated = await _currentUserService.UserIsAuthenticatedAsync();
            if (!userIsAuthenticated)
            {
                return BadRequest(new ApiError("No user is authenticated in this request."));
            }

            var user = new UserGet
            {
                Id = (await _currentUserService.GetCurrentUserIdAsync()).ToString(),
                Name = await _currentUserService.GetCurrentUserNameAsync()
            };

            return Ok(user);
        }
    }
}
