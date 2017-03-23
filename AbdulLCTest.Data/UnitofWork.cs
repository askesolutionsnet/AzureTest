using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbdulLCTest.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly PostDbContext context;

        public UnitOfWork()
        {
            context = new PostDbContext();
        }

        //private GenericRepository<Posts> postRepository;
        private GenericRepository<Posts> postRepository;
        public GenericRepository<Posts> PostRepository
        {
            get
            {

                if (this.postRepository == null)
                {
                    this.postRepository = new GenericRepository<Posts>(context);
                }
                return postRepository;
            }
        }

        #region public methods
        public void Save()
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException entityValidationException)
            {
                throw entityValidationException;

            }

        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
