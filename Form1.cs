using System;
using System.Drawing;
using System.Windows.Forms;

namespace ITHelpDeskSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            BuildLoginScreen();
        }
        private void BuildLoginScreen()
        {
            this.Text = "IT Help Desk System";
            this.Size = new Size(650, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            Label heading = new Label();
            heading.Text = "IT HELP DESK SYSTEM";
            heading.Font = new Font("Arial", 20, FontStyle.Bold);
            heading.AutoSize = true;
            heading.Location = new Point(170, 45);

            Label usernameLabel = new Label();
            usernameLabel.Text = "Username";
            usernameLabel.Location = new Point(150, 140);
            usernameLabel.AutoSize = true;

            TextBox usernameTextBox = new TextBox();
            usernameTextBox.Location = new Point(150, 165);
            usernameTextBox.Width = 330;

            Label passwordLabel = new Label();
            passwordLabel.Text = "Password";
            passwordLabel.Location = new Point(150, 215);
            passwordLabel.AutoSize = true;

            TextBox passwordTextBox = new TextBox();
            passwordTextBox.Location = new Point(150, 240);
            passwordTextBox.Width = 330;
            passwordTextBox.PasswordChar = '*';

            Button loginButton = new Button();
            loginButton.Text = "LOGIN";
            loginButton.Location = new Point(150, 300);
            loginButton.Width = 330;
            loginButton.Height = 40;

            loginButton.Click += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(usernameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(passwordTextBox.Text))
                {
                    MessageBox.Show("Please enter your username and password.");
                }
                else
                {
                    MessageBox.Show("Login interface is working!");
                }
            };

            this.Controls.Add(heading);
            this.Controls.Add(usernameLabel);
            this.Controls.Add(usernameTextBox);
            this.Controls.Add(passwordLabel);
            this.Controls.Add(passwordTextBox);
            this.Controls.Add(loginButton);
        }
    }
}