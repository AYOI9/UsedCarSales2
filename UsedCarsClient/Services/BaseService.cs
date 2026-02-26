using System.Collections.Generic;
using System.Threading.Tasks;

namespace UsedCarsClient.Services
{
    public abstract class BaseService<T>
    {
        public abstract Task<List<T>> GetAll();
        public abstract Task Add(T obj);
        public abstract Task Update(T obj);
        public abstract Task Delete(T obj);
        public abstract Task<List<T>> Search(string str);
    }
}
