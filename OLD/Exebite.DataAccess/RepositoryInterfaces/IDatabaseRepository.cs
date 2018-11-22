﻿using System.Collections.Generic;

namespace Exebite.DataAccess
{
    public interface IDatabaseRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetByID(int Id);
        T Insert(T entity);
        void Delete(int Id);
        T Update(T entity);
    }
}