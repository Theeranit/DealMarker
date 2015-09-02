using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.EnterpriseServices;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Helper;
using KK.DealMaker.DataAccess.Repositories;
using KK.DealMaker.Core.SystemFramework;
using System.Configuration;
using System.Linq.Expressions;
using System.Reflection;

namespace KK.DealMaker.Business.Master
{
    public class EntityBusiness : BaseBusiness
    {
        public List<string> GetFieldsByTableName(string tablename)
        {
            LookupFactorTables table = (LookupFactorTables)Enum.Parse(typeof(LookupFactorTables), tablename);
            //GetType(tablename).GetFields

            return null;
        }

        //public IEnumerable<string> GetFields<TEntity>() where TEntity
        //: EntityObject
        //{
        //    var ids = from p in typeof(TEntity).GetProperties()
        //              where (from a in p.GetCustomAttributes(false)
        //                     where a is EdmScalarPropertyAttribute &&
        //                       ((EdmScalarPropertyAttribute)a).EntityKeyProperty
        //                     select true).FirstOrDefault()
        //              select p.Name;
        //    return ids;
        //}
        
    }
}
