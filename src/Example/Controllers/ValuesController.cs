using Example.Domain.Contexts;
using Example.Domain.Services;
using Insql;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly UserDbContext userDbContext;
        private readonly IUserService userService;

        public ValuesController(UserDbContext userDbContext, IUserService userService)
        {
            this.userDbContext = userDbContext;
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            this.userDbContext.DoWithTransaction(() =>
            {
                this.userDbContext.InsertUserSelective(new Domain.UserInfo
                {
                    UserName = "loveW",
                    UserGender = Domain.Gender.M
                });

                this.userDbContext.UpdateUserSelective(new Domain.UserInfo
                {
                    UserId = 1,
                    UserName = "loveWWW",
                });

                this.userDbContext.DeleteUser(2);
            });

            var llist = this.userDbContext.GetUserList("love");

            //----service

            var list = this.userService.GetUserList("11", Domain.Gender.M);

            var lists = this.userService.GetUserList("11", null);

            return new string[] { "value1", "value2" };
        }
    }
}
