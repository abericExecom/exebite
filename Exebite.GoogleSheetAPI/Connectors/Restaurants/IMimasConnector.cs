﻿using Exebite.GoogleSheetAPI.Connectors.Restaurants.Base;

namespace Exebite.GoogleSheetAPI.Connectors.Restaurants
{
    public interface IMimasConnector : IRestaurantConnector
    {
        /// <summary>
        /// Order Daily menu sheet so first column is today and place correct dates
        /// </summary>
        void DnevniMenuSheetSetup();
    }
}