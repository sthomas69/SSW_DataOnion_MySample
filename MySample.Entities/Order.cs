using System;
using System.Collections.Generic;

namespace MySample.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public DateTime OrderDate { get; set; }

        public virtual ICollection<OrderLineItem> LineItems { get; set; }
    }
}