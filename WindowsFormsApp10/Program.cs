using System;
using System.Windows.Forms;
using WindowsFormsApp10.Forms;

namespace WindowsFormsApp10
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Запускаем окно авторизации
            LoginForm loginForm = new LoginForm();

            // Если пользователь ввел верные данные
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // Запускаем главную форму, передавая туда информацию о пользователе
                Application.Run(new Form1(loginForm.AuthenticatedUser));
            }
            else
            {
                // Если закрыли окно авторизации крестиком — выходим
                Application.Exit();
            }
        }
    }
}