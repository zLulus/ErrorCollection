using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionConvertFailDemo.Entities
{
    public class Entity
    {
        [Column(CanBeNull = false,IsPrimaryKey =true)]
        public int ID { get; set; }
    }
}
