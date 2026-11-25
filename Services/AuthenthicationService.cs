using System;
using MySqlConnector;
using ITHelpDeskSystem.Data;
using ITHelpDeskSystem.Models;
using ITHelpDeskSystem.Helpers;
namespace ITHelpDeskSystem.Services
{
    public class AuthenticationService
    {
        public int GetUserCount()
        {
            using var connection =
                DatabaseConnection.GetConnection();
            connection.Open();
            using var command = new MySqlCommand(
                "SELECT COUNT(*) FROM users",
                connection
            );
            return Convert.ToInt32(command.ExecuteScalar());
        }
        public bool CreateFirstAdmin(
            string fullName,
            string username,
            string password)
        {
            if (GetUserCount() != 0)
                return false;
            ValidateAccountDetails(
                fullName, username, password
            );
            string hash =
                PasswordHasher.HashPassword(password);
            using var connection =
                DatabaseConnection.GetConnection();
            connection.Open();
            using var command = new MySqlCommand(
                @"INSERT INTO users
                 (full_name, username, password_hash, role)
                 VALUES (@name, @username, @password, 'Admin')",
                connection
            );
            command.Parameters.AddWithValue("@name", fullName.Trim());
            command.Parameters.AddWithValue("@username", username.Trim());
            command.Parameters.AddWithValue("@password", hash);
            return command.ExecuteNonQuery() == 1;
        }
        public User? Login(string username, string password)
        {
            using var connection =
                DatabaseConnection.GetConnection();
            connection.Open();
            using var command = new MySqlCommand(
                @"SELECT user_id, full_name, username,
                        password_hash, role, is_active
                 FROM users
                 WHERE username = @username
                 LIMIT 1",
                connection
            );
            command.Parameters.AddWithValue(
                "@username", username.Trim()
            );
            using var reader = command.ExecuteReader();
            if (!reader.Read())
                return null;
            string storedHash =
                reader.GetString("password_hash");
            bool active =
                reader.GetBoolean("is_active");
            if (!active ||
                !PasswordHasher.VerifyPassword(password, storedHash))
            {
                return null;
            }
            string role = reader.GetString("role");
            if (role != "Employee" &&
                role != "Technician" &&
                role != "Admin")
            {
                return null;
            }
            return new User
            {
                UserId = reader.GetInt32("user_id"),
                FullName = reader.GetString("full_name"),
                Username = reader.GetString("username"),
                Role = role,
                IsActive = active
            };
        }
        public bool CreateUser(
            User requestingUser,
            string fullName,
            string username,
            string password,
            string role)
        {
            if (requestingUser == null ||
                !requestingUser.IsActive ||
                requestingUser.Role != "Admin")
            {
                throw new UnauthorizedAccessException(
                    "Only active administrators can create user accounts."
                );
            }
            ValidateAccountDetails(
                fullName, username, password
            );
            if (role != "Employee" &&
                role != "Technician" &&
                role != "Admin")
            {
                throw new ArgumentException(
                    "Invalid account role."
                );
            }
            string hash =
                PasswordHasher.HashPassword(password);
            using var connection =
                DatabaseConnection.GetConnection();
            connection.Open();
            using var command = new MySqlCommand(
                @"INSERT INTO users
                 (full_name, username, password_hash, role)
                 VALUES (@name, @username, @password, @role)",
                connection
            );
            command.Parameters.AddWithValue("@name", fullName.Trim());
            command.Parameters.AddWithValue("@username", username.Trim());
            command.Parameters.AddWithValue("@password", hash);
            command.Parameters.AddWithValue("@role", role);
            return command.ExecuteNonQuery() == 1;
        }
        private void ValidateAccountDetails(
            string fullName,
            string username,
            string password)
        {
            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(
                    "All account fields are required."
                );
            }
            if (password.Length < 12)
            {
                throw new ArgumentException(
                    "Password must contain at least 12 characters."
                );
            }
        }
    }
}