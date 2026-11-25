using System.Drawing;
using System.Windows.Forms;
using ITHelpDeskSystem.Services;
namespace ITHelpDeskSystem.Forms
{
    public partial class AdminSetupForm : Form
    {
        private TextBox nameBox = new();
        private TextBox usernameBox = new();
        private TextBox passwordBox = new();
        public AdminSetupForm()
        {
            Text = "Initial Administrator Setup";
            Size = new Size(420, 380);
            StartPosition = FormStartPosition.CenterScreen;
            Label heading = new()
            {
                Text = "Create Administrator",
                Font = new Font("Arial", 17, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(80, 25)
            };
            AddField("Full Name", nameBox, 90);
            AddField("Username", usernameBox, 155);
            AddField("Password", passwordBox, 220);
            passwordBox.PasswordChar = '*';
            Button createButton = new()
            {
                Text = "CREATE ADMIN",
                Location = new Point(40, 285),
                Size = new Size(320, 35)
            };
            createButton.Click += CreateAdmin;
            Controls.Add(heading);
            Controls.Add(createButton);
        }
        private void AddField(
            string label,
            TextBox box,
            int y)
        {
            Controls.Add(new Label
            {
                Text = label,
                Location = new Point(40, y),
                AutoSize = true
            });
            box.Location = new Point(40, y + 23);
            box.Size = new Size(320, 25);
            Controls.Add(box);
        }
        private void CreateAdmin(object? sender, EventArgs e)
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
                    "Use a password with at least 12 characters."
                );
                return;
            }
            try
            {
                AuthenticationService service = new();
                bool created = service.CreateFirstAdmin(
                    nameBox.Text.Trim(),
                    usernameBox.Text.Trim(),
                    passwordBox.Text
                );
                if (created)
                {
                    MessageBox.Show("Administrator created!");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(
                        "An administrator already exists."
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Setup failed: " + ex.Message);
            }
        }
    }
}
