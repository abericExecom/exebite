﻿using System.Collections.Generic;
using System.Linq;
using Exebite.DataAccess;
using Exebite.DataAccess.Repositories;
using Exebite.Model;

namespace Exebite.Business
{
    public class RestaurantService : IRestaurantService
    {
        private IRestaurantRepository _restaurantRepository;

        public RestaurantService(IRestaurantRepository restaurantHandler)
        {
            _restaurantRepository = restaurantHandler;
        }

        public List<Restaurant> GetAllRestaurants()
        {
            return _restaurantRepository.GetAll().ToList();
        }

        public Restaurant GetRestaurantById(int id)
        {
            return _restaurantRepository.GetByID(id);
        }

        public Restaurant GetRestaurantByName(string name)
        {
            if (name == string.Empty)
            {
                throw new System.ArgumentException("Name can't be empty string");
            }

            return _restaurantRepository.GetByName(name);
        }

        public Restaurant CreateNewRestaurant(Restaurant restaurant)
        {
            if (restaurant == null)
            {
                throw new System.ArgumentNullException(nameof(restaurant));
            }

            return _restaurantRepository.Insert(restaurant);
        }

        public Restaurant UpdateRestourant(Restaurant restaurant)
        {
            return _restaurantRepository.Update(restaurant);
        }

        public void DeleteRestourant(int restaurantId)
        {
            var restaurant = _restaurantRepository.GetByID(restaurantId);
            if (restaurant != null)
            {
                _restaurantRepository.Delete(restaurantId);
            }
        }
    }
}