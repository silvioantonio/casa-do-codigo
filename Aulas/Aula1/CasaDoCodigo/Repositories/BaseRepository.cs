using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;

namespace CasaDoCodigo.Repositories
{
    public abstract class BaseRepository<T> where T : BaseModel
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
