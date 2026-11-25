using System;
using System.Drawing;
using System.Windows.Forms;
using ITHelpDeskSystem.Models;
using ITHelpDeskSystem.Services;

namespace ITHelpDeskSystem.Forms
{
    public partial class LoginForm : Form
    {
        private readonly TextBox usernameBox = new();
        private readonly TextBox passwordBox = new();
        private readonly AuthenticationService auth = new();

        private readonly Panel loginCard = new();
        private readonly Button loginButton = new();
        public LoginForm()
        {
            Text = "IT Help Desk System | Sign In";
            Size = new Size(900, 650);
            MinimumSize = new Size(700, 600);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(241, 245, 249);
            Font = new Font("Segoe UI", 10);
            AutoScaleMode = AutoScaleMode.Font;

            // Login card
            loginCard.Size = new Size(460, 440);
            loginCard.BackColor = Color.White;
            loginCard.BorderStyle = BorderStyle.FixedSingle;

            // Blue accent at the top
            Panel accentBar = new()
            {
                BackColor = Color.FromArgb(10, 102, 194),
                Location = new Point(0, 0),
                Size = new Size(460, 7)
            };

            // Heading
            Label heading = new()
            {
                Text = "IT HELP DESK",
                Font = new Font("Segoe UI", 23, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(30, 45),
                Size = new Size(400, 45)
            };

            Label subtitle = new()
            {
                Text = "Welcome Back to MDW Industries",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(25, 95),
                Size = new Size(410, 30)
            };

            // Username
            AddField("USERNAME", usernameBox, 155);
            usernameBox.PlaceholderText = "Enter your username";
            // Password
            AddField("PASSWORD", passwordBox, 245);
            passwordBox.PlaceholderText = "Enter your password";
            passwordBox.UseSystemPasswordChar = true;

            // Login button
            loginButton.Text = "SIGN IN";
            loginButton.Location = new Point(50, 340);
            loginButton.Size = new Size(360, 45);
            loginButton.BackColor = Color.FromArgb(10, 102, 194);
            loginButton.ForeColor = Color.White;
            loginButton.FlatStyle = FlatStyle.Flat;
            loginButton.FlatAppearance.BorderSize = 0;
            loginButton.Font = new Font(
                "Segoe UI", 10, FontStyle.Bold);
            loginButton.Cursor = Cursors.Hand;
            loginButton.Click += Login;
            // Footer
            Label footer = new()
            {
                Text = "Lindokuhle Nkosinathi Nkosi HelpDesk",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(148, 163, 184),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(30, 400),
                Size = new Size(400, 25)
            };
            loginCard.Controls.Add(accentBar);
            loginCard.Controls.Add(heading);
            loginCard.Controls.Add(subtitle);
            loginCard.Controls.Add(loginButton);
            loginCard.Controls.Add(footer);
            Controls.Add(loginCard);

            // Centre the card whenever the window resizes
            Resize += (sender, e) => LayoutLoginCard();
            LayoutLoginCard();
            AcceptButton = loginButton;
        }
        private void LayoutLoginCard()
        {
            loginCard.Location = new Point(
                Math.Max(0, (ClientSize.Width - loginCard.Width) / 2),
                Math.Max(0, (ClientSize.Height - loginCard.Height) / 2)
            );
        }
        private void AddField(
            string label,
            TextBox box,
            int y)
        {
            Label fieldLabel = new()
            {
                Text = label,
                Location = new Point(50, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 65, 85)
            };
            box.Location = new Point(50, y + 27);
            box.Size = new Size(360, 35);
            box.Font = new Font("Segoe UI", 11);
            box.BorderStyle = BorderStyle.FixedSingle;
            loginCard.Controls.Add(fieldLabel);
            loginCard.Controls.Add(box);
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            try
            {
                if (auth.GetUserCount() == 0)
                {
                    using AdminSetupForm setup = new();
                    if (setup.ShowDialog() != DialogResult.OK)
                    {
                        Close();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Database connection failed: " + ex.Message);
                Close();
            }
        }
        private void Login(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(usernameBox.Text) ||
                string.IsNullOrWhiteSpace(passwordBox.Text))
            {
                MessageBox.Show(
                    "Enter your username and password.");
                return;
            }
            try
            {
                User? user = auth.Login(
                    usernameBox.Text.Trim(),
                    passwordBox.Text
                );
                if (user == null)
                {
                    MessageBox.Show("Invalid login details.");
                    return;
                }
                // Hide login while dashboard is open
                Hide();
                using DashboardForm dashboard = new(user);
                dashboard.ShowDialog();
                // Clear fields when returning to login
                passwordBox.Clear();
                usernameBox.Clear();
                usernameBox.Focus();
                Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Login error: " + ex.Message);
                Show();
            }
        }
    }
}