using System;
using System.Collections.Generic;
using System.Linq;
using WindowsFormsApp10.Services; // Здесь лежит DataService и DocumentModel

namespace WindowsFormsApp10.Controllers
{
    public class DocumentController
    {
        private List<DocumentModel> _documents;
        private DataService _dataService;
        private ReportFactory _reportFactory;
        private int _nextId = 1;

        public event EventHandler<DocumentModel> DocumentCreated;
        public event EventHandler<string> StatusChanged;

        public DocumentController()
        {
            _dataService = new DataService();
            _reportFactory = new ReportFactory();
            LoadData();
        }

        public void LoadData()
        {
            _documents = _dataService.LoadDocuments();
            if (_documents.Any())
                _nextId = _documents.Max(d => d.Id) + 1;
            StatusChanged?.Invoke(this, "Данные загружены из JSON");
        }

        public void SaveData()
        {
            _dataService.SaveDocuments(_documents);
            StatusChanged?.Invoke(this, "Все документы сохранены");
        }

        public DocumentModel CreateNewDocument()
        {
            var doc = new DocumentModel
            {
                Id = _nextId++,
                Title = $"Документ {_nextId - 1}",
                Content = "",
                CreatedAt = DateTime.Now
            };
            _documents.Add(doc);
            DocumentCreated?.Invoke(this, doc);
            StatusChanged?.Invoke(this, $"Создан: {doc.Title}");
            return doc;
        }

        public void ExportToPdf(string filePath)
        {
            _reportFactory.ExportToPdf(_documents, filePath);
            StatusChanged?.Invoke(this, "Успешный экспорт в PDF!");
        }

        public int GetTotalDocuments() => _documents.Count;
    }
}