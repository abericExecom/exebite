﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Either;
using Exebite.Common;
using Exebite.DataAccess.Context;
using Exebite.DomainModel;

namespace Exebite.DataAccess.Repositories
{
    public class MealQueryRepository : IMealQueryRepository
    {
        private readonly IMapper _mapper;
        private readonly IMealOrderingContextFactory _factory;

        public MealQueryRepository(IMealOrderingContextFactory factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }

        public Either<Error, PagingResult<Meal>> Query(MealQueryModel queryModel)
        {
            try
            {
                if (queryModel == null)
                {
                    return new Left<Error, PagingResult<Meal>>(new ArgumentNotSet(nameof(queryModel)));
                }

                using (var context = _factory.Create())
                {
                    var query = context.Meal.AsQueryable();

                    if (queryModel.Id != null)
                    {
                        query = query.Where(x => x.Id == queryModel.Id.Value);
                    }

                    var total = query.Count();
                    query = query
                        .Skip((queryModel.Page - 1) * queryModel.Size)
                        .Take(queryModel.Size);

                    var results = query.ToList();
                    var mapped = _mapper.Map<IList<Meal>>(results).ToList();
                    return new Right<Error, PagingResult<Meal>>(new PagingResult<Meal>(mapped, total));
                }
            }
            catch (Exception ex)
            {
                return new Left<Error, PagingResult<Meal>>(new UnknownError(ex.ToString()));
            }
        }
    }
}
