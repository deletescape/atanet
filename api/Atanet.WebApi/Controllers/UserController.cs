namespace Atanet.WebApi.Controllers
{
    using Atanet.Services.ApiResult;
    using Atanet.Services.Authentication;
    using Atanet.Services.Scoring;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IApiResultService apiResultService;

        private readonly IUserService userService;

        private readonly IScoreService scoreService;

        public UserController(IApiResultService apiResultService, IUserService userService, IScoreService scoreService)
        {
            this.apiResultService = apiResultService;
            this.userService = userService;
            this.scoreService = scoreService;
        }

        [Authorize]
        [HttpGet("{userId}")]
        public IActionResult GetUser(long userId)
        {
            var info = this.userService.GetUserInfo(userId);
            return this.apiResultService.Ok(info);
        }

        [Authorize]
        [HttpGet("picture")]
        public IActionResult GetPicture(long? id)
        {
            var userId = id ?? this.userService.GetCurrentUserId();
            var picture = this.userService.GetUserProfilePicture(userId);
            return File(picture.Data, picture.ContentType);
        }

        [Authorize]
        [HttpGet("score")]
        public IActionResult GetScore()
        {
            var userId = this.userService.GetCurrentUserId();
            var score = this.scoreService.CalculateUserScore(userId);
            return this.apiResultService.Ok(new { Score = score });
        }
    }
}
