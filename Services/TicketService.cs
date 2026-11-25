using System;
using System.Collections.Generic;
using MySqlConnector;
using ITHelpDeskSystem.Data;
using ITHelpDeskSystem.Models;
namespace ITHelpDeskSystem.Services
{
    public class TicketService
    {
        private void ValidateRole(User user)
        {
            if (user == null || !user.IsActive)
                throw new UnauthorizedAccessException(
                    "This account is not authorised."
                );
            if (user.Role != "Employee" &&
                user.Role != "Technician" &&
                user.Role != "Admin")
            {
                throw new UnauthorizedAccessException(
                    "Invalid user role."
                );
            }
        }
        private void RequireStaff(User user)
        {
            ValidateRole(user);
            if (user.Role != "Technician" &&
                user.Role != "Admin")
            {
                throw new UnauthorizedAccessException(
                    "Only technicians and administrators can perform this action."
                );
            }
        }
        public List<Ticket> GetTickets(User user)
        {
            ValidateRole(user);
            List<Ticket> tickets = new();
            using var connection =
                DatabaseConnection.GetConnection();
            connection.Open();
            string sql = @"
               SELECT t.ticket_id, t.title, t.description,
                      t.category, t.priority, t.status,
                      t.created_by, t.assigned_to,
                      t.resolution_notes, t.created_at,
                      u.full_name AS creator_name,
                      a.full_name AS assignee_name
               FROM tickets t
               JOIN users u ON t.created_by = u.user_id
               LEFT JOIN users a ON t.assigned_to = a.user_id";
            if (user.Role == "Employee")
                sql += " WHERE t.created_by = @userId";
            sql += " ORDER BY t.created_at DESC";
            using var command = new MySqlCommand(sql, connection);
            if (user.Role == "Employee")
            {
                command.Parameters.AddWithValue(
                    "@userId", user.UserId
                );
            }
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                tickets.Add(new Ticket
                {
                    TicketId = reader.GetInt32("ticket_id"),
                    Title = reader.GetString("title"),
                    Description = reader.GetString("description"),
                    Category = reader.GetString("category"),
                    Priority = reader.GetString("priority"),
                    Status = reader.GetString("status"),
                    CreatedBy = reader.GetInt32("created_by"),
                    AssignedTo = reader.IsDBNull(
                        reader.GetOrdinal("assigned_to"))
                        ? null
                        : reader.GetInt32("assigned_to"),
                    ResolutionNotes = reader.IsDBNull(
                        reader.GetOrdinal("resolution_notes"))
                        ? ""
                        : reader.GetString("resolution_notes"),
                    CreatedAt = reader.GetDateTime("created_at"),
                    CreatedByName = reader.GetString("creator_name"),
                    AssignedToName = reader.IsDBNull(
                        reader.GetOrdinal("assignee_name"))
                        ? "Unassigned"
                        : reader.GetString("assignee_name")
                });
            }
            return tickets;
        }
        public void CreateTicket(
            User user,
            string title,
            string description,
            string category,
            string priority)
        {
            ValidateRole(user);
            if (string.IsNullOrWhiteSpace(title) ||
                string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException(
                    "Ticket title and description are required."
                );
            }
            string[] allowedPriorities =
            {
               "Low", "Medium", "High", "Critical"
           };
            string[] allowedCategories =
            {
               "Hardware", "Software", "Network",
               "Printer", "Email", "Other"
           };
            if (Array.IndexOf(allowedPriorities, priority) < 0 ||
                Array.IndexOf(allowedCategories, category) < 0)
            {
                throw new ArgumentException(
                    "Invalid ticket category or priority."
                );
            }
            using var connection =
                DatabaseConnection.GetConnection();
            connection.Open();
            using var command = new MySqlCommand(
                @"INSERT INTO tickets
                 (title, description, category, priority,
                  status, created_by)
                 VALUES
                 (@title, @description, @category, @priority,
                  'Open', @createdBy)",
                connection
            );
            command.Parameters.AddWithValue("@title", title.Trim());
            command.Parameters.AddWithValue("@description", description.Trim());
            command.Parameters.AddWithValue("@category", category);
            command.Parameters.AddWithValue("@priority", priority);
            command.Parameters.AddWithValue("@createdBy", user.UserId);
            command.ExecuteNonQuery();
        }
        public void AssignToMe(int ticketId, User user)
        {
            RequireStaff(user);
            using var connection =
                DatabaseConnection.GetConnection();
            connection.Open();
            string sql = @"
               UPDATE tickets
               SET assigned_to = @technician,
                   status = 'In Progress'
               WHERE ticket_id = @ticket
                 AND status <> 'Resolved'";
            if (user.Role == "Technician")
            {
                sql += @"
                 AND (assigned_to IS NULL
                      OR assigned_to = @technician)";
            }
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue(
                "@technician", user.UserId
            );
            command.Parameters.AddWithValue("@ticket", ticketId);
            if (command.ExecuteNonQuery() == 0)
            {
                throw new InvalidOperationException(
                    "The ticket could not be assigned. It may already be resolved or assigned to another technician."
                );
            }
        }
        public void ResolveTicket(
            int ticketId,
            string notes,
            User user)
        {
            RequireStaff(user);
            if (string.IsNullOrWhiteSpace(notes))
            {
                throw new ArgumentException(
                    "Resolution notes are required."
                );
            }
            using var connection =
                DatabaseConnection.GetConnection();
            connection.Open();
            string sql = @"
               UPDATE tickets
               SET status = 'Resolved',
                   resolution_notes = @notes
               WHERE ticket_id = @ticket
                 AND status <> 'Resolved'";
            if (user.Role == "Technician")
            {
                sql += " AND assigned_to = @technician";
            }
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@notes", notes.Trim());
            command.Parameters.AddWithValue("@ticket", ticketId);
            if (user.Role == "Technician")
            {
                command.Parameters.AddWithValue(
                    "@technician", user.UserId
                );
            }
            if (command.ExecuteNonQuery() == 0)
            {
                throw new UnauthorizedAccessException(
                    "You cannot resolve this ticket. It may not be assigned to you, or it may already be resolved."
                );
            }
        }
    }
}