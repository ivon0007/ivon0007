using System.Collections.Generic;

namespace WindowsFormsApp10.Services
{
    // Снимок состояния (Memento)
    public class DocumentMemento
    {
        public string Content { get; }
        public DocumentMemento(string content) { Content = content; }
    }

    // Управляет историей (Caretaker)
    public class UndoManager
    {
        private Stack<DocumentMemento> _undoStack = new Stack<DocumentMemento>();
        private string _currentState;

        public void SaveState(string content)
        {
            if (_currentState != null && _currentState != content)
                _undoStack.Push(new DocumentMemento(_currentState));
            _currentState = content;
        }

        public string Undo()
        {
            if (_undoStack.Count > 0)
            {
                var memento = _undoStack.Pop();
                _currentState = memento.Content;
                return memento.Content;
            }
            return null;
        }
    }
}