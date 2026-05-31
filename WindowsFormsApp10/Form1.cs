using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp10.Controllers;
using WindowsFormsApp10.Forms;
using WindowsFormsApp10.Models;

namespace WindowsFormsApp10
{
    public partial class Form1 : Form
    {
        private DocumentController _controller;
        private ToolStripStatusLabel _statusLabel;
        private User _currentUser;

        public Form1(User user)
        {
            _currentUser = user;
            _controller = new DocumentController();

            InitializeComponentProgrammatically();

            _controller.DocumentCreated += Controller_DocumentCreated;
            _controller.StatusChanged += Controller_StatusChanged;
        }

        private void InitializeComponentProgrammatically()
        {
            // Название окна зависит от того, кто вошел
            this.Text = $"MDI Система [{_currentUser.Role}] - {_currentUser.Login}";
            this.Size = new Size(1024, 768);
            this.IsMdiContainer = true;
            this.StartPosition = FormStartPosition.CenterScreen;

            var menuStrip = new MenuStrip();

            // Меню Файл
            var fileMenu = new ToolStripMenuItem("Файл");
            fileMenu.DropDownItems.Add("Создать документ (Ctrl+N)", null, (s, e) => _controller.CreateNewDocument());
            fileMenu.DropDownItems.Add("Сохранить всё (Ctrl+S)", null, (s, e) => SaveAll());

            // Продвинутый уровень: Экспорт доступен только Админу
            if (_currentUser.Role == UserRole.Admin)
            {
                fileMenu.DropDownItems.Add("Экспорт всех данных в PDF...", null, (s, e) => ExportToPdf());
            }

            fileMenu.DropDownItems.Add(new ToolStripSeparator());
            fileMenu.DropDownItems.Add("Выход", null, (s, e) => Application.Exit());

            // Меню Окна
            var windowMenu = new ToolStripMenuItem("Окна");
            windowMenu.DropDownItems.Add("Каскадом", null, (s, e) => this.LayoutMdi(MdiLayout.Cascade));
            windowMenu.DropDownItems.Add("Слева направо", null, (s, e) => this.LayoutMdi(MdiLayout.TileVertical));

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(windowMenu);
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            // Статус бар
            var statusStrip = new StatusStrip();
            _statusLabel = new ToolStripStatusLabel("Готово");
            statusStrip.Items.Add(_statusLabel);
            this.Controls.Add(statusStrip);

            // Глобальные горячие клавиши формы
            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.Control && e.KeyCode == Keys.N) { _controller.CreateNewDocument(); e.Handled = true; }
                if (e.Control && e.KeyCode == Keys.S) { SaveAll(); e.Handled = true; }
            };
        }

        private void SaveAll()
        {
            _controller.SaveData(); // Пишем в JSON
            // Сбрасываем звездочки со всех открытых окон
            foreach (Form child in this.MdiChildren)
            {
                if (child is DocumentChildForm docForm)
                    docForm.MarkAsSaved();
            }
        }

        private void ExportToPdf()
        {
            using (var saveDialog = new SaveFileDialog { Filter = "PDF файлы (*.pdf)|*.pdf", FileName = "Report.pdf" })
            {
                if (saveDialog.ShowDialog() == DialogResult.OK)
                    _controller.ExportToPdf(saveDialog.FileName);
            }
        }

        private void Controller_DocumentCreated(object sender, WindowsFormsApp10.Services.DocumentModel newDoc)
        {
            var childForm = new DocumentChildForm(newDoc) { MdiParent = this };
            childForm.Show();
        }

        private void Controller_StatusChanged(object sender, string message)
        {
            _statusLabel.Text = $"{message} | Всего документов: {_controller.GetTotalDocuments()}";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _controller.SaveData(); // Автосохранение при закрытии всей программы
            base.OnFormClosing(e);
        }
    }
}