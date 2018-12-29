namespace Atanet.Model.Dto
{
    using System;

    public class UserWithScoreDto : UserDto
    {
        public double Score { get; set; }

        public DateTime Created { get; set; }
    }
}
