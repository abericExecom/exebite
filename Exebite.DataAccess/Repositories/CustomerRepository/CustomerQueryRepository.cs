﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Either;
using Exebite.DataAccess.Context;
using Exebite.DomainModel;

namespace Exebite.DataAccess.Repositories
{
    public class CustomerQueryRepository : ICustomerQueryRepository
    {
        private readonly IMapper _mapper;
        private readonly IFoodOrderingContextFactory _factory;

        public CustomerQueryRepository(IFoodOrderingContextFactory factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }

        public Either<Error, PagingResult<Customer>> Query(CustomerQueryModel queryModel)
        {
            try
            {
                if (queryModel == null)
                {
                    return new Left<Error, PagingResult<Customer>>(new ArgumentNotSet(nameof(queryModel)));
                }

                using (var context = _factory.Create())
                {
                    var query = context.Customers.AsQueryable();

                    if (queryModel.Id != null)
                    {
                        query = query.Where(x => x.Id == queryModel.Id.Value);
                    }

                    var size = queryModel.Size <= QueryConstants.MaxElements ? queryModel.Size : QueryConstants.MaxElements;

                    var total = query.Count();
                    query = query
                        .Skip((queryModel.Page - 1) * size)
                        .Take(size);

                    var results = query.ToList();
                    var mapped = _mapper.Map<IList<Customer>>(results).ToList();
                    return new Right<Error, PagingResult<Customer>>(new PagingResult<Customer>(mapped, total));
                }
            }
            catch (Exception ex)
            {
                return new Left<Error, PagingResult<Customer>>(new UnknownError(ex.ToString()));
            }
        }
    }
}
