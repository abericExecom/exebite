﻿namespace Exebite.API.Models
{
    public class CustomerAliasQueryDto : QueryBaseDto
    {
        public int? Id { get; set; }

        public string GoogleId { get; set; }
    }
}
