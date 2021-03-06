namespace Atanet.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.ApiResult;
    using Services.Authentication;
    using Services.Scoring;

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
        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(long userId)
        {
            this.userService.DeleteUser(userId);
            return this.apiResultService.Ok("User deleted");
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUser()
        {
            var currentUserId = this.userService.GetCurrentUserId();
            return this.GetUser(currentUserId);
        }

        [Authorize]
        [HttpGet("scoreboard")]
        public IActionResult GetScoreBoard()
        {
            var result = this.scoreService.GetUsersSortedByScore();
            return this.apiResultService.Ok(result);
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
