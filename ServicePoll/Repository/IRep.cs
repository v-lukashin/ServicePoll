using System;
using System.Collections.Generic;
using System.Linq;

namespace ServicePoll.Repository
{
    public interface IRep<T>
    {
        T Get(string id);
        IEnumerable<T> GetAll();
        T Create(T value);
        void Update(string id, T value);
        void Remove(string id);
    }
}
