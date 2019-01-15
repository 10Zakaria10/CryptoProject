using cryptoProject.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace cryptoProject.Raport
{
    public class QuestionReport
    {
        Font fontStyle;
        Document _document;
        PdfPTable pdfTable = new PdfPTable(3);
        PdfPCell pdfCell;

        MemoryStream memoryStream = new MemoryStream();
        List<Question> _questions = new List<Question>();
        
        public byte[] PrepareReport(List<Question> questions)
        {
            _questions = questions;
            _document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _document.SetMargins(20f, 20f, 20f, 20f);
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            fontStyle = FontFactory.GetFont("tahoma", 8f, 1);
            PdfWriter.GetInstance(_document,memoryStream);
            _document.Open();
            pdfTable.SetWidths(new float[] { 20f, 150f, 100f });

            this.reportHeader();
            this.reportBody();
            pdfTable.HeaderRows = 2;
            _document.Add(pdfTable);
            _document.Close();
            return memoryStream.ToArray();
        }
        public void reportHeader()
        {
            fontStyle = FontFactory.GetFont("tahoma",18f,1);
            pdfCell = new PdfPCell(new Phrase("Resume audit", fontStyle));
            pdfCell.Colspan = 3;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.Border = 0;
            pdfCell.BackgroundColor = BaseColor.WHITE;
            pdfCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfCell);
            pdfTable.CompleteRow();


            fontStyle = FontFactory.GetFont("tahoma", 14f, 1);
            pdfCell = new PdfPCell(new Phrase("Solutions ", fontStyle));
            pdfCell.Colspan = 3;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.Border = 0;
            pdfCell.BackgroundColor = BaseColor.WHITE;
            pdfCell.ExtraParagraphSpace = 0;
            pdfTable.AddCell(pdfCell);
            pdfTable.CompleteRow();

        }

        private void reportBody()
        {
            fontStyle = FontFactory.GetFont("tahoma", 8f, 1);
            pdfCell = new PdfPCell(new Phrase("Categorie", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfTable.AddCell(pdfCell);

            fontStyle = FontFactory.GetFont("tahoma", 8f, 1);
            pdfCell = new PdfPCell(new Phrase("Question", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfTable.AddCell(pdfCell);

            fontStyle = FontFactory.GetFont("tahoma", 8f, 1);
            pdfCell = new PdfPCell(new Phrase("Recommendation", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfTable.AddCell(pdfCell);

            fontStyle = FontFactory.GetFont("tahoma", 8f, 0);
            
            foreach(Question qst in _questions)
            {
                pdfCell = new PdfPCell(new Phrase(qst.Categorie.nom, fontStyle));
                pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                pdfCell.BackgroundColor = BaseColor.WHITE;
                pdfTable.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(qst.qst, fontStyle));
                pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                pdfCell.BackgroundColor = BaseColor.WHITE;
                pdfTable.AddCell(pdfCell);


                pdfCell = new PdfPCell(new Phrase(qst.Solution.description, fontStyle));
                pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                pdfCell.BackgroundColor = BaseColor.WHITE;
                pdfTable.AddCell(pdfCell);

                pdfTable.CompleteRow();
            }

        }
    } 
}