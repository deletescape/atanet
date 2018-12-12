namespace Atanet.Model.Dto
{
    using System;
    using Atanet.Model.Data;

    public class ShowUserDto
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public DateTime Created { get; set; }

    }
}
