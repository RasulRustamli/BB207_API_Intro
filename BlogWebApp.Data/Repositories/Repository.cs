using BlogWebApp.Core.Entities.Common;
using BlogWebApp.Data.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebApp.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly BlogWebAppContext _context;
        DbSet<T> _table;

        public Repository(BlogWebAppContext context)
        {
            this._context = context;
            _table=_context.Set<T>();
        }
        public async Task<IQueryable<T>> GetAllAsync(
            Expression<Func<T,bool>>? func=null,
            Expression<Func<T,object>>? orderBy=null,
            bool isOrderByDesting =false,
            params string[]? includes)
        {
            IQueryable<T> data = _table;
            if(includes is not null)
            {
                for (int i = 0; i < includes.Length; i++)
                {
                    data = data.Include(includes[i]);
                }
            }
            if(orderBy is not null)
            {
                data = 
                    isOrderByDesting
                    ?data.OrderByDescending(orderBy)
                    : data.OrderBy(orderBy);
            }
           
            return func==null?data:data.Where(func);
        }
    }
}
