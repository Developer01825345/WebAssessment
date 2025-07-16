namespace UserManagement.Api.Repositories;

using System.Data;
using Microsoft.Data.SqlClient;
using UserManagement.Api.Models;

public class UserRepository
{
    private readonly string _connectionString;
    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public IEnumerable<User> GetAll()
    {
        var users = new List<User>();
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new("sp_GetUsers", conn) { CommandType = CommandType.StoredProcedure };
        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            users.Add(new User
            {
                UserId = (int)reader["Id"],
                FirstName = reader["FirstName"].ToString()!,
                LastName = reader["LastName"].ToString()!,
                Email = reader["Email"].ToString()!,
                PhoneNumber = reader["Phone"].ToString()!,
                Address = reader["Address"].ToString()!
            });
        }
        return users;
    }

    public User? GetById(int id)
    {
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new("sp_GetUserById", conn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@Id", id);
        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new User
            {
                UserId = (int)reader["Id"],
                FirstName = reader["FirstName"].ToString()!,
                LastName = reader["LastName"].ToString()!,
                Email = reader["Email"].ToString()!,
                PhoneNumber = reader["Phone"].ToString()!,
                Address = reader["Address"].ToString()!
            };
        }
        return null;
    }

    public void Add(User user)
    {
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new("sp_CreateUser", conn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
        cmd.Parameters.AddWithValue("@LastName", user.LastName);
        cmd.Parameters.AddWithValue("@Email", user.Email);
        cmd.Parameters.AddWithValue("@Phone", user.PhoneNumber);
        cmd.Parameters.AddWithValue("@Address", user.Address);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void Update(User user)
    {
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new("sp_UpdateUser", conn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@Id", user.UserId);
        cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
        cmd.Parameters.AddWithValue("@LastName", user.LastName);
        cmd.Parameters.AddWithValue("@Email", user.Email);
        cmd.Parameters.AddWithValue("@Phone", user.PhoneNumber);
        cmd.Parameters.AddWithValue("@Address", user.Address);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new("sp_DeleteUser", conn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@Id", id);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public bool EmailExists(string email, int? userId = null)
    {
        var users = GetAll();
        return userId == null
            ? users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
            : users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && u.UserId != userId);
    }
}