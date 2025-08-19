using System.Text;
using DocumentFormat.OpenXml.Packaging;
using WordInterop = Microsoft.Office.Interop.Word;
using OpenXml = DocumentFormat.OpenXml.Wordprocessing;

namespace Service.Converter
{
    public class WordToHtmlConverter
    {

        public static string ConvertToHtml(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                return "";
            }

            string extension = System.IO.Path.GetExtension(filePath).ToLower();
            string htmlContent = "<html><body>";

            try
            {
                switch (extension)
                {
                    case ".docx":
                        htmlContent += ExtractFromDocx(filePath);
                        break;
                    case ".doc":
                        htmlContent += ExtractFromDoc(filePath);
                        break;
                    case ".rtf":
                        htmlContent += ExtractFromRtf(filePath);
                        break;
                    default:
                        htmlContent += "<p>Error: Unsupported file format.</p>";
                        break;
                }
            }
            catch (Exception ex)
            {
                htmlContent += $"<p>Error: {System.Web.HttpUtility.HtmlEncode(ex.Message)}</p>";
            }

            htmlContent += "</body></html>";
            return htmlContent;
        }

        private static string ExtractFromDocx(string filePath)
        {
            StringBuilder html = new StringBuilder();
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
            {
                OpenXml.Body body = wordDoc.MainDocumentPart.Document.Body;
                foreach (OpenXml.Paragraph paragraph in body.Elements<OpenXml.Paragraph>())
                {
                    StringBuilder paragraphText = new StringBuilder();
                    foreach (OpenXml.Run run in paragraph.Elements<OpenXml.Run>())
                    {
                        foreach (OpenXml.Text text in run.Elements<OpenXml.Text>())
                        {
                            paragraphText.Append(System.Web.HttpUtility.HtmlEncode(text.Text));
                        }
                    }
                    if (paragraphText.Length > 0)
                    {
                        html.Append($"<p>{paragraphText}</p>");
                    }
                }
            }
            return html.ToString();
        }

        private static string ExtractFromDoc(string filePath)
        {
            StringBuilder html = new StringBuilder();
            WordInterop.Application wordApp = null;
            WordInterop.Document wordDoc = null;
            try
            {
                wordApp = new WordInterop.Application();
                wordDoc = wordApp.Documents.Open(filePath, ReadOnly: true, Visible: false);
                foreach (WordInterop.Paragraph paragraph in wordDoc.Paragraphs)
                {
                    string text = paragraph.Range.Text?.Trim();
                    if (!string.IsNullOrEmpty(text))
                    {
                        html.Append($"<p>{System.Web.HttpUtility.HtmlEncode(text)}</p>");
                    }
                }
            }
            finally
            {
                if (wordDoc != null)
                {
                    wordDoc.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wordDoc);
                }

                if (wordApp != null)
                {
                    wordApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);
                }
            }
            return html.ToString();
        }

        private static string ExtractFromRtf(string filePath)
        {
            string rtfContent = System.IO.File.ReadAllText(filePath);

            // Basic RTF stripping – not 100% accurate, use for simple content
            string plainText = System.Text.RegularExpressions.Regex.Replace(rtfContent, @"\\[a-z]+\d* ?|{\*\\[^}]+}|[{}]", string.Empty);

            StringBuilder html = new StringBuilder();
            foreach (string line in plainText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                html.Append($"<p>{System.Web.HttpUtility.HtmlEncode(line)}</p>");
            }

            return html.ToString();
        }

    }
}
