using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionConvertFailDemo.Entities
{
    public class ChooseClass: Entity
    {
        [Column(CanBeNull = false)]
        public long ClassID { get; set; }
        [Column(CanBeNull = false)]
        public long StudentID { get; set; }
    }
}
