using Microsoft.EntityFrameworkCore.Diagnostics;
using Northwind.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.EntityModels
{
    public class SetLastRefreshedInterceptor : IMaterializationInterceptor
    {
        public object InitializedInstance(MaterializationInterceptionData materializationData, object entity)
        {
            if(entity is IHasLastRefreshed entityWithLastRefreshed)
            {
                entityWithLastRefreshed.LastRefreshed = DateTimeOffset.Now;
            }
            return entity;
        }
    }
}
