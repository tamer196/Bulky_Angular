using BulkyBook.DataAccess.Repositiry.IRepository;
using BulkyBooks.DataAcces.Data;
using BulkyBooks.DataAccess.Repositiry;
using BulkyBooks.DataAccess.Repositiry.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repositiry
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository category { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            category = new CategoryRepository(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
