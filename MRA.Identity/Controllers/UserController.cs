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
        private readonly UserManager<AppUser> _userManager;

        public UserController(SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IIdentityServerInteractionService interactionService) =>
            (_userManager) =
            (userManager);

        [HttpGet]
        [Route("GetUsersByIds")]
        public async Task<IActionResult> GetUsersByIdsAsync(string data)
        {
            var userIds = JsonConvert.DeserializeObject<IEnumerable<Guid>>(data);
            
            var users = new List<AppUser>();
            
            foreach(var id in userIds)
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if(user != null)
                {
                    users.Add(user);
                }
            }


            return Ok(JsonConvert.SerializeObject(users));
        }
    }
}