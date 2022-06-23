//using AuthApi.Logic;
//using AuthApi.Repositories;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MRA.Identity.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IIdentityServerInteractionService _interactionService;

        //private readonly IUserRepository userRepository;
        //private readonly IUserLogic userLogic;
        public UserController(SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IIdentityServerInteractionService interactionService) =>
            (_signInManager, _userManager, _interactionService) =
            (signInManager, userManager, interactionService);

        [HttpGet]
        //[Authorize(Roles = "User")]
        [Route("GetUsersByIds")]
        public async Task<IActionResult> GetRoomsByIdsAsync(string data)
        {
            var userIds = JsonConvert.DeserializeObject<IEnumerable<Guid>>(data);
            
            var users = new List<AppUser>();
            
            foreach(var id in userIds)
            {
                users.Add(await _userManager.FindByIdAsync(id.ToString()));
            }

            return Ok(JsonConvert.SerializeObject(users));
        }
    }
}
