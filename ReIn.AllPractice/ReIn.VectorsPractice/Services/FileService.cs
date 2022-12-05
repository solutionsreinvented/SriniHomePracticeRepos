using System.IO;

namespace ReIn.VectorsPractice.Services
{
    public class FileService
    {


        public static void WriteContentsToFile(string filePath, string contents)
        {
            File.WriteAllText(filePath, contents);
        }
    }
}
