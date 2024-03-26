using BulkyBook.Models;
using BulkyBooks.Models;

namespace BulkyBooks.DataAccess.Repositiry.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);
        void Save();
    }
}
