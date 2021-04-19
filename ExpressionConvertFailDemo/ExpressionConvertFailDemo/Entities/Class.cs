using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionConvertFailDemo.Entities
{
    public class Class: Entity
    {
        [Column(CanBeNull = false)]
        public string ClassName { get; set; }
        [Column(CanBeNull = false)]
        public int ClassTotalHour { get; set; }
    }
}
