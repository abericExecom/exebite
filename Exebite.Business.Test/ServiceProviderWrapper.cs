﻿using System;
using Exebite.Business.Test.Mocks;
using Exebite.DataAccess.AutoMapper;
using Exebite.DataAccess.Context;
using Exebite.DataAccess.Migrations;
using Exebite.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;


namespace Exebite.Business.Test
{
    public static class ServiceProviderWrapper
    {
        public static IServiceProvider GetContainer()
        {
            var serviceProvider = new ServiceCollection()
                                        .AddLogging()
                                        .AddTransient<IFoodOrderingContextFactory, InMemoryDBFactory>()

                                        .AddTransient<IRestaurantRepository, RestaurantRepository>()
                                        .AddTransient<IFoodRepository, FoodRepository>()
                                        .AddTransient<IRecipeRepository, RecipeRepository>()
                                        .AddTransient<ICustomerRepository, CustomerRepository>()
                                        .AddTransient<ILocationRepository, LocationRepository>()
                                        .AddTransient<IOrderRepository, OrderRepository>()

                                        .AddTransient<IFoodService, FoodService>()
                                        .AddTransient<IMenuService, MenuService>()
                                        .AddTransient<IRestaurantService, RestaurantService>()
                                        .AddTransient<ICustomerService, CustomerService>()
                                        .AddTransient<ILocationService, LocationService>()
                                        .AddTransient<IOrderService, OrderService>()

                                        .AddTransient<IExebiteDbContextOptionsFactory, ExebiteDbContextOptionsFactory>()
                                        .AddTransient<IExebiteMapper, ExebiteMapper>()
                                        .BuildServiceProvider();
            return serviceProvider;
        }

        public static T Resolve<T>(this IServiceProvider provider)
        {
            var res = (T)provider.GetService(typeof(T));
            return res;
        }
    }
}