using BulkyBooks.DataAccess.Repositiry.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repositiry.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository category {  get; }

        void Save();
    }
}
