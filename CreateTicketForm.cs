using System;
using System.Drawing;
using System.Windows.Forms;
using ITHelpDeskSystem.Models;
using ITHelpDeskSystem.Services;
namespace ITHelpDeskSystem.Forms
{
    public partial class CreateTicketForm : Form
    {
        private User currentUser;
        private TicketService service = new();
        private TextBox titleBox = new();
        private TextBox descriptionBox = new();
        private ComboBox categoryBox = new();
        private ComboBox priorityBox = new();
        public CreateTicketForm(User user)
        {
            currentUser = user;
            Text = "Submit IT Support Ticket";
            Size = new Size(550, 550);
            StartPosition = FormStartPosition.CenterParent;
            Label heading = new()
            {
                Text = "CREATE SUPPORT TICKET",
                Font = new Font("Arial", 17, FontStyle.Bold),
                Location = new Point(35, 25),
                AutoSize = true
            };
            AddLabel("Ticket Title", 90);
            titleBox.Location = new Point(35, 115);
            titleBox.Width = 450;
            AddLabel("Description", 155);
            descriptionBox.Location = new Point(35, 180);
            descriptionBox.Size = new Size(450, 90);
            descriptionBox.Multiline = true;
            AddLabel("Category", 285);
            categoryBox.Location = new Point(35, 310);
            categoryBox.Width = 450;
            categoryBox.DropDownStyle = ComboBoxStyle.DropDownList;
            categoryBox.Items.AddRange(new object[]
            {
               "Hardware", "Software", "Network",
               "Printer", "Email", "Other"
            });
            categoryBox.SelectedIndex = 0;
            AddLabel("Priority", 355);
            priorityBox.Location = new Point(35, 380);
            priorityBox.Width = 450;
            priorityBox.DropDownStyle = ComboBoxStyle.DropDownList;
            priorityBox.Items.AddRange(new object[]
            {
               "Low", "Medium", "High", "Critical"
            });
            priorityBox.SelectedItem = "Medium";
            Button submitButton = new()
            {
                Text = "SUBMIT TICKET",
                Location = new Point(35, 445),
                Size = new Size(450, 40)
            };
            submitButton.Click += SubmitTicket;
            Controls.Add(heading);
            Controls.Add(titleBox);
            Controls.Add(descriptionBox);
            Controls.Add(categoryBox);
            Controls.Add(priorityBox);
            Controls.Add(submitButton);
        }
        private void AddLabel(string text, int y)
        {
            Controls.Add(new Label
            {
                Text = text,
                Location = new Point(35, y),
                AutoSize = true
            });
        }
        private void SubmitTicket(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(titleBox.Text) ||
                string.IsNullOrWhiteSpace(descriptionBox.Text))
            {
                MessageBox.Show(
                    "Please complete the title and description."
                );
                return;
            }
            try
            {
                service.CreateTicket(
                    currentUser,
                    titleBox.Text.Trim(),
                    descriptionBox.Text.Trim(),
                    categoryBox.Text,
                    priorityBox.Text
                );
                MessageBox.Show("Your ticket was submitted!");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ticket submission failed: " + ex.Message
                );
            }
        }
    }
}