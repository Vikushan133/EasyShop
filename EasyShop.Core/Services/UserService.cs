using EasyShop.Core.Contracts;
using EasyShop.Core.Models;

namespace EasyShop.Core.Services;

public class UserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void CreateUser(User user)
    {
        _unitOfWork.Users.Add(user);
        _unitOfWork.Complete();
    }

    public User? GetUserById(int id)
    {
        return _unitOfWork.Users.GetById(id);
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _unitOfWork.Users.GetAll();
    }

    public void UpdateUser(User user)
    {
        _unitOfWork.Users.Update(user);
        _unitOfWork.Complete();
    }

    public void DeleteUser(int id)
    {
        var user = _unitOfWork.Users.GetById(id);
        if (user != null)
        {
            _unitOfWork.Users.Delete(user);
            _unitOfWork.Complete();
        }
    }
}
