using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class BaseRepository<T> where T : BaseModel
    {
        protected readonly Applicationcontext applicationcontext;

        protected readonly DbSet<T> dbSets;

        public BaseRepository(Applicationcontext applicationcontext)
        {
            this.applicationcontext = applicationcontext;
            dbSets = applicationcontext.Set<T>();
        }
    }
}
