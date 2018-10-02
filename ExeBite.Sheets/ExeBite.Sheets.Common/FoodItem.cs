﻿namespace ExeBite.Sheets.Common
{
    /// <summary>
    /// Class representing food offered every day.
    /// </summary>
    public class FoodItem : Food
    {
        #region Public properties
        /// <summary>
        /// Food description
        /// </summary>
        public string Description { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Standard constructor that sets all 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="fasting"></param>
        /// <param name="price"></param>
        public FoodItem(string name, double price, string restaurant, string subcategory, string description) :
            base(name, price, restaurant, subcategory)
        {
            Description = description;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Override of default ToString regular methods.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Subcategory} : {Name}: {Price.ToString()} RSD, Description: {Description}";
        } 
        #endregion
    }
}
