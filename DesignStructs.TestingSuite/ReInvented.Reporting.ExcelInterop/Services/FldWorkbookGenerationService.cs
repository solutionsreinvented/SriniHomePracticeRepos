using System;
using System.IO;
using System.Threading.Tasks;

using HtmlAgilityPack;

using ReInvented.DataAccess;
using ReInvented.Domain.Reporting.Models;
using ReInvented.Reporting.ExcelInterop.Models;
using ReInvented.Shared.Models;
using ReInvented.Shared.Services;

namespace ReInvented.Reporting.ExcelInterop.Services
{
    public class FldWorkbookGenerationService
    {
        public static async Task<Result<string>> GenerateExcelDocumentAsync(string htmlFilePath, FLDReport fldReport)
        {
            if (string.IsNullOrWhiteSpace(htmlFilePath) || !File.Exists(htmlFilePath))
            {
                ///TODO: Use a logger instead
                throw new ArgumentException($"Invalid {nameof(htmlFilePath)}. Either the path is invalid or the file doesn't exist.");
            }
            //if (fldReport == null)
            //{
            //    ///TODO: Use a logger instead
            //    throw new ArgumentNullException($"{nameof(fldReport)} shall not be null.");
            //}

            Result<string> result;

            PlaywrightService playwrightService = new PlaywrightService();
            try
            {
                HtmlDocument htmlDocument = await playwrightService.RenderHtmlAsync(htmlFilePath);
                string dirTarget = Path.GetDirectoryName(htmlFilePath);
                string xlFileName = Path.GetFileNameWithoutExtension(htmlFilePath);

                //JsonDataSerializer<FLDReport> serializer = new JsonDataSerializer<FLDReport>();
                //fldReport = serializer.Deserialize(@"C:\Users\masanams\Desktop\FLD.json");

                FldWorkbook fldWorkbook = new FldWorkbook(dirTarget, $"{xlFileName}.xlsm", fldReport, htmlDocument);
                fldWorkbook.Create();

                result = new Result<string>("Successfully completed generating the foundation load data in MS Excel format", false);
            }
            catch (Exception ex)
            {
                result = new Result<string>(ex.Message, false);
            }

            return result;
        }
    }
}
