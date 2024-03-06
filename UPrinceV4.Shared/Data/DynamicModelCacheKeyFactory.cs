//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using System;

//namespace UPrinceV4.Shared.Data
//{
//    public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
//    {
//        public object Create(DbContext context)
//        {
//            var castedContext = context as IMultitenantDbContext;
//            if (castedContext == null)
//            {
//                throw new Exception("Unknown DBContext type");
//            }

//            return new { castedContext.TenantId };
//        }
//    }
//}

