using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;

namespace KK.DealMaker.Core.Helper
{
    public static class StringExtensions
    {
        public static MemberExpression ToMemberExpression(this string source, ParameterExpression p)
        {
            if (p == null)
                throw new ArgumentNullException("p");

            string[] properties = source.Split('.');

            Expression expression = p;
            Type type = p.Type;

            foreach (var prop in properties)
            {
                var property = type.GetProperty(prop);
                if (property == null)
                    throw new ArgumentException("Invalid expression", "source");

                expression = Expression.MakeMemberAccess(expression, property);
                type = property.PropertyType;
            }

            return (MemberExpression)expression;
        }

        public static Expression<Func<T, bool>> CreateExpression<T>(string searchField, string searchString, string searchOper)
        {
            Expression exp = null;
            var p = Expression.Parameter(typeof(T), "p");

            try
            {
                Expression propertyAccess = searchField.ToMemberExpression(p);

                switch (searchOper)
                {
                    case "bw":
                        exp = Expression.Call(propertyAccess, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), Expression.Constant(searchString));
                        break;
                    case "cn":
                        exp = Expression.Call(propertyAccess, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), Expression.Constant(searchString));
                        break;
                    case "ew":
                        exp = Expression.Call(propertyAccess, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), Expression.Constant(searchString));
                        break;
                    case "gt":
                        exp = Expression.GreaterThan(propertyAccess, Expression.Constant(searchString, propertyAccess.Type));
                        break;
                    case "ge":
                        exp = Expression.GreaterThanOrEqual(propertyAccess, Expression.Constant(searchString, propertyAccess.Type));
                        break;
                    case "lt":
                        exp = Expression.LessThan(propertyAccess, Expression.Constant(searchString, propertyAccess.Type));
                        break;
                    case "le":
                        exp = Expression.LessThanOrEqual(propertyAccess, Expression.Constant(searchString, propertyAccess.Type));
                        break;
                    case "eq":
                        exp = Expression.Equal(propertyAccess, Expression.Constant(searchString, propertyAccess.Type));
                        break;
                    case "ne":
                        exp = Expression.NotEqual(propertyAccess, Expression.Constant(searchString, propertyAccess.Type));
                        break;
                    default:
                        return null;
                }

                return (Expression<Func<T, bool>>)Expression.Lambda(exp, p);
            }
            catch
            {
                return null;
            }
        }

        public static Func<TInput, bool> CreatePredicate<TInput, TResult>(Func<TInput, TResult> selector, TResult comparisonValue)
        {
            return tInput => selector(tInput).Equals(comparisonValue);
        }
    }
}
