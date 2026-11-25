using MySqlConnector;
namespace ITHelpDeskSystem.Data
{
    public static class DatabaseConnection
    {
        public static MySqlConnection GetConnection()
        {
            string connectionString =
                Environment.GetEnvironmentVariable(
                    "HELPDESK_DB_CONNECTION"
                );
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Database connection settings are missing."
                );
            }
            return new MySqlConnection(connectionString);
        }
    }
}