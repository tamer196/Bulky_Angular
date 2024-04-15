using BulkyBook.DataAccess.Repositiry.IRepository;
using BulkyBooks.DataAcces.Data;
using BulkyBooks.DataAccess.Repositiry;
using BulkyBooks.DataAccess.Repositiry.IRepository;
using Microsoft.EntityFrameworkCore;
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
        public IProductRepository product { get; private set; }
        public ICompanyRepository company { get; private set; }
        public IShoppingCartRepository shoppingCart { get; private set; }
        public IApplicationUserRepository applicationUser { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            category = new CategoryRepository(_db);
            product = new ProductRepository(_db);
            company = new CompanyRepository(_db);
            shoppingCart = new ShoppingCartRepository(_db);
            applicationUser = new ApplicationUserRepository(_db);
        }

        public void Detach(object entity)
        {
            var entry = _db.Entry(entity);
            entry.State = EntityState.Detached;
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
