using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Entities;
using server.Interface.Repository;

namespace server.Repository
{
    public class ShippingAddressRepository : GenericRepository<ShippingAddress>, IShippingAddressRepository
    {
        private readonly DataContex contex;

        public ShippingAddressRepository(DataContex contex) : base(contex)
        {
            this.contex = contex;
        }

        public async Task<ShippingAddress?> GetShippingAddressByOrderId(int orderId)
        {
            return await contex.shippingAddresses.Where(a=>a.OrderId == orderId).FirstOrDefaultAsync();
        }
    }
}