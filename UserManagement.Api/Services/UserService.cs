namespace UserManagement.Api.Services;

using UserManagement.Api.Constants;
using UserManagement.Api.Models;
using UserManagement.Api.Repositories;


public class UserService
{
    private readonly UserRepository _repository;
    public UserService(UserRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<User> GetAll() => _repository.GetAll();

    public User? GetById(int id) => _repository.GetById(id);

    public void Create(User user)
    {
        ValidateUser(user);

        if (_repository.EmailExists(user.Email))
            throw new ArgumentException(Error.EmailExists);

        _repository.Add(user);
    }

    public void Update(User user)
    {
        ValidateUser(user);

        if (_repository.EmailExists(user.Email, user.UserId))
            throw new ArgumentException(Error.EmailExistsForAnother);

        _repository.Update(user);
    }

    public void Delete(int id)
    {
        var existing = _repository.GetById(id);
        if (existing == null)
            throw new KeyNotFoundException(Error.UserNotFound);
        _repository.Delete(id);
    }

    private void ValidateUser(User user)
    {
        if (string.IsNullOrWhiteSpace(user.FirstName))
            throw new ArgumentException(Validation.FirstNameRequired);

        if (string.IsNullOrWhiteSpace(user.LastName))
            throw new ArgumentException(Validation.LastNameRequired);

        if (string.IsNullOrWhiteSpace(user.Email) || !user.Email.Contains("@"))
            throw new ArgumentException(Validation.InValidEmail);

        if (string.IsNullOrWhiteSpace(user.PhoneNumber) || user.PhoneNumber.Length < 10)
            throw new ArgumentException(Validation.InValidPhoneNumber);
    }
}