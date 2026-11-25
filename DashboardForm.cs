using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ITHelpDeskSystem.Models;
using ITHelpDeskSystem.Services;
namespace ITHelpDeskSystem.Forms
{
    public partial class DashboardForm : Form
    {
        private readonly User currentUser;
        private readonly TicketService ticketService = new();
        private readonly DataGridView ticketGrid = new();
        private readonly TextBox searchBox = new();
        private readonly ComboBox statusFilter = new();
        private readonly Label welcomeLabel = new();
        private readonly Label statisticsLabel = new();
        private readonly FlowLayoutPanel actionPanel = new();
        private List<Ticket> allTickets = new();
        public DashboardForm(User user)
        {
            currentUser = user;
            Text = "IT Help Desk Ticketing System | Dashboard";
            Size = new Size(1200, 750);
            MinimumSize = new Size(850, 600);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(241, 245, 249);
            Font = new Font("Segoe UI", 10);
            AutoScaleMode = AutoScaleMode.Font;
            // Main layout
            TableLayoutPanel mainLayout = new()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                Padding = new Padding(24),
                BackColor = Color.FromArgb(241, 245, 249)
            };
            mainLayout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100));
            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 65));
            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 65));
            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 45));
            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));
            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.AutoSize));
            // Header
            Panel headerPanel = new()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(15)
            };
            welcomeLabel.Text =
                $"Welcome, {currentUser.FullName} ({currentUser.Role})";
            welcomeLabel.Dock = DockStyle.Fill;
            welcomeLabel.TextAlign = ContentAlignment.MiddleLeft;
            welcomeLabel.Font = new Font(
                "Segoe UI", 15, FontStyle.Bold);
            welcomeLabel.ForeColor =
                Color.FromArgb(30, 41, 59);
            headerPanel.Controls.Add(welcomeLabel);
            // Search and filter section
            FlowLayoutPanel filterPanel = new()
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0, 12, 0, 5),
                BackColor = Color.Transparent
            };
            searchBox.Size = new Size(320, 32);
            searchBox.Font = new Font("Segoe UI", 10);
            searchBox.PlaceholderText = "Search tickets...";
            searchBox.Margin = new Padding(0, 0, 15, 0);
            statusFilter.Size = new Size(180, 32);
            statusFilter.Font = new Font("Segoe UI", 10);
            statusFilter.DropDownStyle =
                ComboBoxStyle.DropDownList;
            statusFilter.Items.AddRange(new object[]
            {
               "All Statuses",
               "Open",
               "In Progress",
               "Resolved"
            });
            statusFilter.SelectedIndex = 0;
            filterPanel.Controls.Add(searchBox);
            filterPanel.Controls.Add(statusFilter);
            searchBox.TextChanged +=
                (sender, e) => ApplyFilters();
            statusFilter.SelectedIndexChanged +=
                (sender, e) => ApplyFilters();
            // Statistics
            statisticsLabel.Dock = DockStyle.Fill;
            statisticsLabel.TextAlign = ContentAlignment.MiddleLeft;
            statisticsLabel.Font = new Font(
                "Segoe UI", 10, FontStyle.Bold);
            statisticsLabel.ForeColor =
                Color.FromArgb(51, 65, 85);
            // Ticket table
            ticketGrid.Dock = DockStyle.Fill;
            ticketGrid.ReadOnly = true;
            ticketGrid.AllowUserToAddRows = false;
            ticketGrid.AllowUserToDeleteRows = false;
            ticketGrid.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            ticketGrid.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            ticketGrid.MultiSelect = false;
            ticketGrid.RowHeadersVisible = false;
            ticketGrid.BackgroundColor = Color.White;
            ticketGrid.BorderStyle = BorderStyle.None;
            ticketGrid.EnableHeadersVisualStyles = false;
            ticketGrid.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(10, 102, 194);
            ticketGrid.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;
            ticketGrid.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);
            ticketGrid.ColumnHeadersHeight = 40;
            ticketGrid.RowTemplate.Height = 35;
            ticketGrid.DefaultCellStyle.Font =
                new Font("Segoe UI", 9);
            ticketGrid.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(219, 234, 254);
            ticketGrid.DefaultCellStyle.SelectionForeColor =
                Color.FromArgb(30, 41, 59);
            // Action buttons
            actionPanel.Dock = DockStyle.Fill;
            actionPanel.FlowDirection =
                FlowDirection.LeftToRight;
            actionPanel.WrapContents = true;
            actionPanel.AutoSize = true;
            actionPanel.Padding = new Padding(0, 12, 0, 0);
            actionPanel.BackColor = Color.Transparent;
            AddButton("Refresh", RefreshTickets);
            AddButton("New Ticket", OpenCreateTicket);
            AddButton("Assign to Me", AssignSelectedTicket);
            AddButton("Resolve Ticket", ResolveSelectedTicket);
            AddButton("Reports", ShowReports);
            AddButton("Create User", OpenRegisterUser);
            AddButton("Logout", Logout);
            // Add everything to the main layout
            mainLayout.Controls.Add(headerPanel, 0, 0);
            mainLayout.Controls.Add(filterPanel, 0, 1);
            mainLayout.Controls.Add(statisticsLabel, 0, 2);
            mainLayout.Controls.Add(ticketGrid, 0, 3);
            mainLayout.Controls.Add(actionPanel, 0, 4);
            Controls.Add(mainLayout);
            UpdateButtonVisibility();
            RefreshTickets();
        }
        // Creates dashboard buttons
        private void AddButton(string text, Action action)
        {
            Button button = new()
            {
                Text = text,
                Size = new Size(125, 38),
                BackColor = Color.FromArgb(10, 102, 194),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 10, 8)
            };
            button.FlatAppearance.BorderSize = 0;
            button.Click += (sender, e) => action();
            actionPanel.Controls.Add(button);
        }
        // Controls button visibility according to user role
        private void UpdateButtonVisibility()
        {
            bool isEmployee = currentUser.Role == "Employee";
            bool isTechnician = currentUser.Role == "Technician";
            bool isAdmin = currentUser.Role == "Admin";
            foreach (Control control in actionPanel.Controls)
            {
                if (control is not Button button)
                    continue;
                switch (button.Text)
                {
                    case "Refresh":
                    case "New Ticket":
                        button.Visible =
                            isEmployee || isTechnician || isAdmin;
                        break;
                    case "Assign to Me":
                    case "Resolve Ticket":
                    case "Reports":
                        button.Visible = isTechnician || isAdmin;
                        break;
                    case "Create User":
                        button.Visible = isAdmin;
                        break;
                    case "Logout":
                        button.Visible = true;
                        break;
                }
            }
        }
        // Logs the current user out
        private void Logout()
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Close();
            }
        }
        // Loads tickets from the database
        private void RefreshTickets()
        {
            try
            {
                allTickets = ticketService.GetTickets(currentUser);
                UpdateStatistics();
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load tickets: " + ex.Message,
                    "Ticket Loading Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        // Searches and filters tickets
        private void ApplyFilters()
        {
            string searchText = searchBox.Text.Trim().ToLower();
            string selectedStatus =
                statusFilter.SelectedItem?.ToString()
                ?? "All Statuses";
            IEnumerable<Ticket> filteredTickets = allTickets;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filteredTickets = filteredTickets.Where(ticket =>
                    (ticket.Title ?? "").ToLower().Contains(searchText) ||
                    (ticket.Category ?? "").ToLower().Contains(searchText) ||
                    (ticket.Description ?? "").ToLower().Contains(searchText));
            }
            if (selectedStatus != "All Statuses")
            {
                filteredTickets = filteredTickets.Where(ticket =>
                    ticket.Status == selectedStatus);
            }
            ticketGrid.DataSource = null;
            ticketGrid.DataSource = filteredTickets.ToList();
        }
        // Updates dashboard statistics
        private void UpdateStatistics()
        {
            int totalTickets = allTickets.Count;
            int openTickets = allTickets.Count(
                ticket => ticket.Status == "Open");
            int inProgressTickets = allTickets.Count(
                ticket => ticket.Status == "In Progress");
            int resolvedTickets = allTickets.Count(
                ticket => ticket.Status == "Resolved");
            statisticsLabel.Text =
                $"Total: {totalTickets}     " +
                $"Open: {openTickets}     " +
                $"In Progress: {inProgressTickets}     " +
                $"Resolved: {resolvedTickets}";
        }
        // Gets the selected ticket ID
        private int? GetSelectedTicketId()
        {
            if (ticketGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a ticket first.");
                return null;
            }
            object? value =
                ticketGrid.SelectedRows[0].Cells["TicketId"].Value;
            if (value == null)
                return null;
            return Convert.ToInt32(value);
        }
        // Opens ticket creation form
        private void OpenCreateTicket()
        {
            using CreateTicketForm form = new(currentUser);
            form.ShowDialog();
            RefreshTickets();
        }
        // Opens user registration form
        private void OpenRegisterUser()
        {
            if (currentUser.Role != "Admin")
            {
                MessageBox.Show(
                    "Only administrators can create users.");
                return;
            }
            using RegisterUserForm form = new(currentUser);
            form.ShowDialog();
        }
        // Assigns selected ticket to current technician
        private void AssignSelectedTicket()
        {
            int? ticketId = GetSelectedTicketId();
            if (ticketId == null)
                return;
            try
            {
                ticketService.AssignToMe(
                    ticketId.Value,
                    currentUser);
                MessageBox.Show("Ticket assigned successfully.");
                RefreshTickets();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to assign ticket: " + ex.Message);
            }
        }
        // Resolves selected ticket
        private void ResolveSelectedTicket()
        {
            int? ticketId = GetSelectedTicketId();
            if (ticketId == null)
                return;
            string notes = PromptForResolutionNotes();
            if (string.IsNullOrWhiteSpace(notes))
            {
                MessageBox.Show("Resolution notes are required.");
                return;
            }
            try
            {
                ticketService.ResolveTicket(
                    ticketId.Value,
                    notes,
                    currentUser);
                MessageBox.Show("Ticket resolved successfully.");
                RefreshTickets();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to resolve ticket: " + ex.Message);
            }
        }
        // Requests ticket resolution notes
        private string PromptForResolutionNotes()
        {
            using Form prompt = new();
            prompt.Text = "Resolution Notes";
            prompt.Size = new Size(450, 250);
            prompt.StartPosition = FormStartPosition.CenterParent;
            Label label = new()
            {
                Text = "Enter the resolution details:",
                Location = new Point(15, 15),
                AutoSize = true
            };
            TextBox notesBox = new()
            {
                Location = new Point(15, 45),
                Size = new Size(400, 100),
                Multiline = true
            };
            Button submitButton = new()
            {
                Text = "Submit",
                Location = new Point(15, 155),
                DialogResult = DialogResult.OK
            };
            Button cancelButton = new()
            {
                Text = "Cancel",
                Location = new Point(110, 155),
                DialogResult = DialogResult.Cancel
            };
            prompt.Controls.Add(label);
            prompt.Controls.Add(notesBox);
            prompt.Controls.Add(submitButton);
            prompt.Controls.Add(cancelButton);
            prompt.AcceptButton = submitButton;
            prompt.CancelButton = cancelButton;
            return prompt.ShowDialog(this) == DialogResult.OK
                ? notesBox.Text.Trim()
                : "";
        }
        // Displays ticket reports
        private void ShowReports()
        {
            if (currentUser.Role != "Admin" &&
                currentUser.Role != "Technician")
            {
                MessageBox.Show(
                    "You are not authorised to view reports.");
                return;
            }
            int total = allTickets.Count;
            int open = allTickets.Count(
                t => t.Status == "Open");
            int inProgress = allTickets.Count(
                t => t.Status == "In Progress");
            int resolved = allTickets.Count(
                t => t.Status == "Resolved");
            MessageBox.Show(
                "HELP DESK TICKET REPORT\n\n" +
                $"Total tickets: {total}\n" +
                $"Open tickets: {open}\n" +
                $"In progress tickets: {inProgress}\n" +
                $"Resolved tickets: {resolved}",
                "Ticket Reports",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}