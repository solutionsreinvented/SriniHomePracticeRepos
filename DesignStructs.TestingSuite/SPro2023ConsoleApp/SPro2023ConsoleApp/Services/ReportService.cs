using Word = Microsoft.Office.Interop.Word;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Word;

namespace SPro2023ConsoleApp.Services
{
    public class ReportService
    {

        public static void GenerateOptimizedSectionsSummary(string reportPath, string fileName)
        {
            // Initialize Word application
            Application wordApp = new Application
            {
                Visible = true // Show Word application
            };

            // Create a new document
            Document doc = wordApp.Documents.Add();

            // Define table parameters
            int numRows = 5;
            int numCols = 5;
            float tableWidth = 400f;
            float[] columnWidths = { 50f, 120f, 80f, 100f, 150f };

            // Insert table into document
            Table table = doc.Tables.Add(doc.Range(), numRows, numCols);
            table.AllowAutoFit = true;
            table.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
            table.PreferredWidth = tableWidth;

            // Set border properties
            table.Borders.Enable = 1; // Enable borders
            table.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle; // Thick border at top and bottom
            table.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle; // Thin border inside cells

            // Set border color (assuming user specified color is red)
            table.Borders.OutsideColor = WdColor.wdColorRed;
            table.Borders.InsideColor = WdColor.wdColorRed;

            // Set column widths
            for (int i = 1; i <= numCols; i++)
            {
                table.Columns[i].Width = columnWidths[i - 1];
            }

            // Set alignment and formatting for headers and cells
            for (int i = 1; i <= numCols; i++)
            {
                table.Cell(1, i).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                table.Cell(1, i).Range.Font.Bold = 1; // Bold headers
                table.Cell(1, i).Range.Shading.BackgroundPatternColor = WdColor.wdColorGray50; // Background color

                // Set alignment for cells under the respective columns
                for (int j = 2; j <= numRows; j++)
                {
                    table.Cell(j, i).Range.ParagraphFormat.Alignment = i == 1 || i == 4 ? WdParagraphAlignment.wdAlignParagraphCenter : WdParagraphAlignment.wdAlignParagraphLeft;
                }
            }

            // Set font
            table.Range.Font.Name = "Aptos";

            // Populate table data
            string[,] data = {
                { "Sl. No.", "Section Property", "Weight (kg/m)", "Utilization Ratio", "Section Database" },
                { "1", "Property 1", "50", "0.5", "Database 1" },
                { "2", "Property 2", "60", "0.6", "Database 2" },
                { "3", "Property 3", "70", "0.7", "Database 3" },
                { "4", "Property 4", "80", "0.8", "Database 4" }
            };

            for (int i = 1; i <= numRows; i++)
            {
                for (int j = 1; j <= numCols; j++)
                {
                    table.Cell(i, j).Range.Text = data[i - 1, j - 1];
                    if (j != 1) // Apply left alignment and tab space for non-Sl. No. columns
                    {
                        table.Cell(i, j).Range.ParagraphFormat.Alignment = i == 1 || i == 4 ? WdParagraphAlignment.wdAlignParagraphCenter : WdParagraphAlignment.wdAlignParagraphLeft;
                        table.Cell(i, j).Range.InsertAfter("\t");
                    }
                }
            }

            // Clean up
            Marshal.ReleaseComObject(doc);
            Marshal.ReleaseComObject(wordApp);
            doc = null;
            wordApp = null;
        }


        public static void GenerateWordReport(string reportPath, string fileName)
        {
            // Create a new instance of Word application
            Application wordApp = new Application();

            // Create a new document
            Document doc = wordApp.Documents.Add();

            // Set the default font for the entire document
            doc.Content.Font.Name = "Adobe Clean UX";

            // Add title
            Paragraph title = doc.Paragraphs.Add();
            title.Range.Text = "Title";
            title.Range.Bold = 1;
            title.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            title.Range.InsertParagraphAfter();

            // Add description
            Paragraph description = doc.Paragraphs.Add();
            description.Range.Text = "Description";
            description.Range.Bold = 1;
            description.Range.InsertParagraphAfter();

            // Add description content
            Paragraph descriptionContent = doc.Paragraphs.Add();
            descriptionContent.Range.Text = "This is the description content.";
            descriptionContent.Range.InsertParagraphAfter();

            // Add table
            Table table = doc.Tables.Add(descriptionContent.Range, 1, 4); // 1 row, 4 columns
            table.Borders.Enable = 1; // Enable borders

            // Apply table style
            table.set_Style("Grid Table 7 Colorful - Accent 5");

            // Set table width to 100% of page width
            table.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
            table.PreferredWidth = 100.0f;

            // Set column headers
            table.Cell(1, 1).Range.Text = "Sl. No.";
            table.Cell(1, 2).Range.Text = "Section Property";
            table.Cell(1, 3).Range.Text = "Weight (kg/m)";
            table.Cell(1, 4).Range.Text = "Utilization Ratio";

            // Autofit table
            table.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitContent);

            // Save the document
            doc.SaveAs2($"{Path.Combine(reportPath, fileName)}.docx");

            // Close the document
            doc.Close();

            // Quit Word application
            wordApp.Quit();
        }
    }
}
