using FrameMobile.Model;
using SubSonic.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FrameMobile.Domain
{
    public static class DbContextExtension
    {
        public static object Add<T>(this IDbContext context, T model) where T : class, IMySQLModel, new()
        {
            return context.Add<T>(model);
        }

        public static int Delete<T>(this IDbContext context, T model) where T : class, IMySQLModel, new()
        {
            return context.Delete<T>(model);
        }

        public static int Update<T>(this IDbContext context, T model) where T : class, IMySQLModel, new()
        {
            return context.Update<T>(model);
        }

        public static IList<T> Find<T>(this IDbContext context, Expression<Func<T, bool>> expression) where T : class, IMySQLModel, new()
        {
            return context.Find<T>(expression);
        }

        public static bool Exists<T>(this IDbContext context, Expression<Func<T, bool>> expression) where T : class, IMySQLModel, new()
        {
            return context.Exists<T>(expression);
        }

        public static T Single<T>(this IDbContext context, object key) where T : class, IMySQLModel, new()
        {
            return context.Single<T>(key);
        }

        public static T Single<T>(this IDbContext context, Expression<Func<T, bool>> expression) where T : class, IMySQLModel, new()
        {
            return context.Single<T>(expression);
        }

        public static IQueryable<T> All<T>(this IDbContext context) where T : class, IMySQLModel, new()
        {
            return context.All<T>();
        }

        public static PagedList<T> GetPaged<T>(this IDbContext context, int pageIndex, int pageSize) where T : class, IMySQLModel, new()
        {
            return context.GetPaged<T>(pageIndex, pageSize);
        }

        public static PagedList<T> GetPaged<T>(this IDbContext context, string sortBy, int pageIndex, int pageSize) where T : class, IMySQLModel, new()
        {
            return context.GetPaged<T>(sortBy, pageIndex, pageSize);
        }
    }
}
