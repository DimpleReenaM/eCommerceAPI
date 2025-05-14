using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Entities;

namespace server.Interface.Repository
{
    public interface ICartRepository:IGenericRepository<ShoppingCart>
    {
      Task<ShoppingCart?> FindCartByUserId(int userId);
    }
}