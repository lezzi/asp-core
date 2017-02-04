using System;
using System.Threading.Tasks;

namespace DotCore.Portable.DataAccess.EntityFrameworkCore
{
    public class EfUnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DotCoreContext _dotCoreContext;

        public EfUnitOfWork(DotCoreContext dotCoreContext)
        {
            if (dotCoreContext == null)
                throw new ArgumentNullException(nameof(dotCoreContext));
            _dotCoreContext = dotCoreContext;

        }

        public Task CommitAsync()
        {
            return _dotCoreContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dotCoreContext.Dispose();
        }
    }
}