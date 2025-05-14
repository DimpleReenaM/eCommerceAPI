using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Entities;

namespace server.Interface.Repository
{
    public interface IShippingAddressRepository:IGenericRepository<ShippingAddress>
    {
        Task<ShippingAddress?> GetShippingAddressByOrderId(int orderId);
    }
}