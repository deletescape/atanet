namespace Atanet.WebApi.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;
    using Atanet.Model.Validation;
    using Atanet.Services.ApiResult;
    using Atanet.Services.Exceptions;
    using System.Threading.Tasks;

    [Route("api/Accounts")]
    public class AccountsController : Controller
    {
        private readonly UserManager<AppUser> userManager;

        private readonly IApiResultService apiResultService;

        public AccountsController(UserManager<AppUser> userManager, IApiResultService apiResultService)
        {
            this.userManager = userManager;
            this.apiResultService = apiResultService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegisterDto model)
        {
            var userIdentity = Mapper.Map<AppUser>(model);
            var result = await this.userManager.CreateAsync(userIdentity, model.Password);
            if (!result.Succeeded)
            {
                throw new ApiException(x => x.BadRequestResult(result, model.Email, PropertyName.Account.Id));
            }

            return this.apiResultService.Created(AtanetEntityName.Account, 0);
        }
    }
}
