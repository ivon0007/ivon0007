using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WindowsFormsApp10.Services
{
    // Паттерн Фабрика (Factory) для создания отчетов
    public class ReportFactory
    {
        public void ExportToPdf(List<DocumentModel> documents, string filePath)
        {
            using (var document = new Document())
            {
                PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();

                // Настраиваем шрифты и заголовки
                var font = new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD);
                document.Add(new Paragraph("System Report", font));
                document.Add(new Paragraph("Date: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm")));
                document.Add(new Paragraph(" "));

                // Создаем таблицу из 3 колонок
                PdfPTable table = new PdfPTable(3);
                table.AddCell("ID");
                table.AddCell("Title");
                table.AddCell("Status");

                foreach (var doc in documents)
                {
                    table.AddCell(doc.Id.ToString());
                    table.AddCell(doc.Title ?? "");
                    table.AddCell(string.IsNullOrEmpty(doc.Content) ? "Empty" : "Has Content");
                }

                document.Add(table);
                document.Close();
            }
        }
    }
}