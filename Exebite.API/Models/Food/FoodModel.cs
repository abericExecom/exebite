﻿using Exebite.DomainModel;

namespace Exebite.API.Models
{
    public class FoodModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public FoodType Type { get; set; }

        public decimal Price { get; set; }

        public int RestaurantId { get; set; }

        public string Description { get; set; }

        public bool IsInactive { get; set; }
    }
}
