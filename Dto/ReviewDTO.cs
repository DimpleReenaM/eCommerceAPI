using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dto
{
    public class ReviewDTO
    {
        public string Review { get; set; }
        public int Rating { get; set; }
        public int ProductId { get; set; }
    }
}