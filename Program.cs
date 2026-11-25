using System;
using System.Windows.Forms;
using ITHelpDeskSystem.Forms;

namespace ITHelpDeskSystem
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new LoginForm());
        }
    }
}