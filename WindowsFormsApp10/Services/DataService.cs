using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace WindowsFormsApp10.Services
{
    // Сервис для работы с JSON (Продвинутый уровень)
    public class DataService
    {
        private readonly string _filePath = "documents.json";

        // Метод сохранения данных
        public void SaveDocuments(List<DocumentModel> documents)
        {
            // Сериализуем список в строку JSON с красивым форматированием
            string json = JsonConvert.SerializeObject(documents, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        // Метод загрузки данных
        public List<DocumentModel> LoadDocuments()
        {
            if (!File.Exists(_filePath))
            {
                return new List<DocumentModel>(); // Возвращаем пустой список, если файла нет
            }

            string json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<DocumentModel>>(json) ?? new List<DocumentModel>();
        }
    }

    // Временно разместим модель здесь, позже можно перенести в папку Models
    public class DocumentModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}