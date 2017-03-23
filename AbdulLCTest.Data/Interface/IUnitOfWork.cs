using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbdulLCTest.Data
{
    public interface IUnitOfWork
    {
        #region Properties
        GenericRepository<Posts> PostRepository { get; }
        #endregion

        #region Public methods
        /// <summary>
        /// Save method.
        /// </summary>
        void Save();
        #endregion
    }
}
