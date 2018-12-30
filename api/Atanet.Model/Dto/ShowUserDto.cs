namespace Atanet.Model.Dto
{
    using System;
    using System.Collections.Generic;
    using Atanet.Model.Data;

    public class ShowUserDto
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public DateTime Created { get; set; }

        public double Score { get; set; }

        public PictureDto Picture { get; set; }

        public AtanetAction[] Capabilities { get; set; }
    }
}
