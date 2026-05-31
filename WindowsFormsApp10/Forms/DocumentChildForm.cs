using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp10.Services;

namespace WindowsFormsApp10.Forms
{
    public class DocumentChildForm : Form
    {
        private DocumentModel _model;
        private TextBox _textBox;
        private UndoManager _undoManager;
        public bool IsDirty { get; private set; } = false;

        public DocumentChildForm(DocumentModel model)
        {
            _model = model;
            _undoManager = new UndoManager();
            SetupUI();
            _undoManager.SaveState(_model.Content); // Сохраняем пустое начальное состояние
        }

        private void SetupUI()
        {
            this.Text = _model.Title;
            this.Size = new Size(400, 300);

            _textBox = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 11f),
                ScrollBars = ScrollBars.Both,
                Text = _model.Content
            };

            // Сохраняем состояние при каждом вводе
            _textBox.TextChanged += (s, e) =>
            {
                if (_textBox.Focused)
                {
                    _undoManager.SaveState(_model.Content);
                    _model.Content = _textBox.Text;
                    IsDirty = true;
                    if (!this.Text.EndsWith("*")) this.Text += " *";
                }
            };

            // Горячие клавиши: Ctrl+Z для отмены
            _textBox.KeyDown += (s, e) =>
            {
                if (e.Control && e.KeyCode == Keys.Z)
                {
                    string previous = _undoManager.Undo();
                    if (previous != null)
                    {
                        _textBox.Text = previous;
                        _model.Content = previous;
                        _textBox.SelectionStart = _textBox.Text.Length;
                    }
                    e.Handled = true;
                }
            };

            this.Controls.Add(_textBox);
        }

        public void MarkAsSaved()
        {
            IsDirty = false;
            this.Text = _model.Title; // Убираем звездочку
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (IsDirty)
            {
                var result = MessageBox.Show(
                    "Есть несохраненные изменения. Закрыть без сохранения?",
                    "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.No) e.Cancel = true;
            }
            base.OnFormClosing(e);
        }
    }
}