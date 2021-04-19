﻿using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionConvertFailDemo.Entities
{
    public class Student : Entity
    {
        [Column(CanBeNull = false)]
        public string Name { get; set; }
        [Column]
        public int Age { get; set; }
    }
}
