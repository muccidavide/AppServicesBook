﻿using Northwind.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.EntityModels
{
    public partial class Employee : IHasLastRefreshed
    {
        [NotMapped]
        public DateTimeOffset LastRefreshed { get; set; }
    }
}
