using EasyShop.Core.Contracts;
using EasyShop.Core.Models;

namespace EasyShop.Core.Services;

public class ProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void AddProduct(Product product)
    {
        _unitOfWork.Products.Add(product);
        _unitOfWork.Complete();
    }

    public IEnumerable<Product> GetAllProducts()
    {
        return _unitOfWork.Products.GetAll();
    }

    public Product? GetProductById(int id)
    {
        return _unitOfWork.Products.GetById(id);
    }

    public void UpdateProduct(Product product)
    {
        _unitOfWork.Products.Update(product);
        _unitOfWork.Complete();
    }

    public void DeleteProduct(int id)
    {
        var product = _unitOfWork.Products.GetById(id);
        if (product != null)
        {
            _unitOfWork.Products.Delete(product);
            _unitOfWork.Complete();
        }
    }
}
