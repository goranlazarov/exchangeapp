using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ExchangeApp.Models;
using System.Linq.Expressions;

namespace ExchangeApp.Helpers
{
    public static class DbSetExtended
    {
        /*
         * Wrapper around Find method. If found, entity is returned, otherwise ArgumentNullException is thrown
        */
        public static TEntity Locate<TEntity>( this IDbSet<TEntity> set, params Object[] keyValues ) where TEntity:class 
        {
            TEntity r = set.Find( keyValues );
            if (r == null)
                throw new ArgumentNullException();
            return r;
        }

        public static void AddIfNotFound<TEntity>( this IDbSet<TEntity> set, Expression<Func<TEntity,object>> identifierExpression, params TEntity[] entities) where TEntity:class
        {
            throw new NotImplementedException();
            /*
             * //if (System.Diagnostics.Debugger.IsAttached == false)
                //System.Diagnostics.Debugger.Launch();
            setset.Union(entities);
            var parameter = Expression.Parameter(typeof(TEntity), "x");

            foreach ( var entity in entities)
            {
                foreach ( var param in identifierExpression.Parameters )
                {
                if (set.Union((Expression<Func<TEntity, bool>>)
                    x => x.parametar == entity[parameter]
                Expression.Lambda(
                    Expression.Equal(
                        Expression.Property( parameter , param.Name ),
                        Expression.Constant( "test" ) ),
                    parameter) ).Single() != null)
                    set.Add(entity);
                }
            }
            */
       }
    }
}