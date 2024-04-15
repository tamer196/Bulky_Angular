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
        IProductRepository product {  get; }
        ICompanyRepository company {  get; }
        IShoppingCartRepository shoppingCart {  get; }
        IApplicationUserRepository applicationUser {  get; }

        void Detach(object entity);
        void Save();
    }
}
