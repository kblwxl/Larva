using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain
{
    public delegate void OrderAction<T>(OrderCriteria<T> oc);
    public class OrderCriteria<T>
    {
        public List<OrderByField<T, dynamic>> OrderByFields { get; }
        public OrderCriteria()
        {
            OrderByFields = new List<OrderByField<T, dynamic>>();
        }
        public OrderCriteria<T> Asc(Expression<Func<T, dynamic>> order)
        {
            this.OrderByFields.Add( new OrderByField<T, dynamic> { OrderBy = OrderBy.Asc, Field = order });
            return this;
        }
        public OrderCriteria<T> Desc(Expression<Func<T, dynamic>> order)
        {
            this.OrderByFields.Add( new OrderByField<T, dynamic> { OrderBy = OrderBy.Desc, Field = order });
            return this;
        }
    }
    public class OrderByField<T, FieldType>
    {
        public OrderBy OrderBy { get; set; }
        public Expression<Func<T, dynamic>> Field { get; set; }
    }
    public enum OrderBy
    {
        Asc,
        Desc
    }
}
