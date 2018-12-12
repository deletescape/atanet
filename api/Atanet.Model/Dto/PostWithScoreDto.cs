namespace Atanet.Model.Dto
{
    using Atanet.Model.Data;

    public class PostWithScoreDto
    {
        public double Score { get; set; }
        
        public Post Post { get; set; }
    }
}
