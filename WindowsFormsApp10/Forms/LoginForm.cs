using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp10.Models;
using WindowsFormsApp10.Services;

namespace WindowsFormsApp10.Forms
{
    public class LoginForm : Form
    {
        private TextBox _txtLogin;
        private TextBox _txtPassword;
        private Button _btnLogin;
        private AuthService _authService;

        // Свойство для хранения того, кто именно вошел в систему
        public User AuthenticatedUser { get; private set; }

        public LoginForm()
        {
            _authService = new AuthService();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Авторизация";
            this.Size = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            var lblLogin = new Label { Text = "Логин (admin/user):", Location = new Point(20, 20), AutoSize = true };
            _txtLogin = new TextBox { Location = new Point(130, 20), Width = 120 };

            var lblPassword = new Label { Text = "Пароль (123):", Location = new Point(20, 60), AutoSize = true };
            _txtPassword = new TextBox { Location = new Point(130, 60), Width = 120, PasswordChar = '*' };

            _btnLogin = new Button { Text = "Войти", Location = new Point(130, 100), Width = 120 };
            _btnLogin.Click += BtnLogin_Click;

            this.Controls.Add(lblLogin);
            this.Controls.Add(_txtLogin);
            this.Controls.Add(lblPassword);
            this.Controls.Add(_txtPassword);
            this.Controls.Add(_btnLogin);
            this.AcceptButton = _btnLogin; // Позволяет нажимать Enter для входа
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            var user = _authService.Authenticate(_txtLogin.Text, _txtPassword.Text);
            if (user != null)
            {
                AuthenticatedUser = user;
                this.DialogResult = DialogResult.OK; // Успешный вход
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}