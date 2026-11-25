using System;
using System.Drawing;
using System.Windows.Forms;
using ITHelpDeskSystem.Models;
using ITHelpDeskSystem.Services;
namespace ITHelpDeskSystem.Forms
{
    public partial class RegisterUserForm : Form
    {
        private readonly User currentUser;
        private TextBox nameBox = new();
        private TextBox usernameBox = new();
        private TextBox passwordBox = new();
        private ComboBox roleBox = new();
        public RegisterUserForm(User user)
        {
            currentUser = user;
            Text = "Create User Account";
            Size = new Size(450, 440);
            StartPosition = FormStartPosition.CenterParent;
            AddField("Full Name", nameBox, 45);
            AddField("Username", usernameBox, 115);
            AddField("Password", passwordBox, 185);
            passwordBox.PasswordChar = '*';
            Controls.Add(new Label
            {
                Text = "User Role",
                Location = new Point(35, 255),
                AutoSize = true
            });
            roleBox.Location = new Point(35, 280);
            roleBox.Width = 350;
            roleBox.DropDownStyle = ComboBoxStyle.DropDownList;
            roleBox.Items.AddRange(new object[]
            {
               "Employee",
               "Technician",
               "Admin"
            });
            roleBox.SelectedIndex = 0;
            Button createButton = new()
            {
                Text = "CREATE ACCOUNT",
                Location = new Point(35, 340),
                Size = new Size(350, 35)
            };
            createButton.Click += CreateAccount;
            Controls.Add(createButton);
            Controls.Add(roleBox);
        }
        private void AddField(
            string label,
            TextBox box,
            int y)
        {
            Controls.Add(new Label
            {
                Text = label,
                Location = new Point(35, y),
                AutoSize = true
            });
            box.Location = new Point(35, y + 25);
            box.Width = 350;
            Controls.Add(box);
        }
        private void CreateAccount(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameBox.Text) ||
                string.IsNullOrWhiteSpace(usernameBox.Text) ||
                string.IsNullOrWhiteSpace(passwordBox.Text))
            {
                MessageBox.Show("Complete all fields.");
                return;
            }
            if (passwordBox.Text.Length < 12)
            {
                MessageBox.Show(
                    "Password must contain at least 12 characters."
                );
                return;
            }
            try
            {
                AuthenticationService service = new();
                bool created = service.CreateUser(
                    currentUser,
                    nameBox.Text.Trim(),
                    usernameBox.Text.Trim(),
                    passwordBox.Text,
                    roleBox.Text
                );
                if (created)
                {
                    MessageBox.Show("Account created successfully!");
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not create account: " + ex.Message
                );
            }
        }
    }
}