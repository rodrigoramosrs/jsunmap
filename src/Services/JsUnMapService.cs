using jsunmap.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Resources;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace jsunmap.Services
{
    public static class JsUnMapService
    {
        public static async Task StartUnMapJS(string FileOrUrl)
        {
            Console.WriteLine($"[i] Trying to unmap file '{FileOrUrl}', please wait ...");
            string MapContent = string.Empty;
            string OutputPath = GlobalSettings.OutputPath;
            Uri mapFileUri = new Uri(FileOrUrl);
            if (!mapFileUri.IsFile)
            {
                MapContent = await HttpClientService.DownloadString(mapFileUri);
                OutputPath = Path.Combine(GlobalSettings.OutputPath, mapFileUri.Host);
            }
            else
            {
                MapContent = File.ReadAllText(FileOrUrl);
            }
            if (!string.IsNullOrEmpty(MapContent))
                ExtractMapFile(Path.Combine(OutputPath, "src"), MapContent);
            Console.WriteLine($"[i] Searching for sensitive data, please wait ...");
            ExtractSensitveData(OutputPath);
            Console.WriteLine($"[!] Done....");
        }

        private static void ExtractSensitveData(string PathToScan)
        {
            if (!Directory.Exists(PathToScan)) return;

            StringBuilder sensitiveOutputContent = new StringBuilder();
            StringBuilder piiOutputContent = new StringBuilder();
            foreach (var filePath in Directory.EnumerateFiles(PathToScan, "*.*", SearchOption.AllDirectories))
            {
                string currentFileContent = File.ReadAllText(filePath);

                string RelativeFilePath = filePath.Replace(GlobalSettings.OutputPath, string.Empty).Remove(0, 1);
                AppendOutputFileWithSensitiveData(Path.Combine(PathToScan, $"sensitive_data.txt"), RelativeFilePath, RegexService.ExtractAllGeneralSensitiveData(currentFileContent));

                AppendOutputFileWithSensitiveData(Path.Combine(PathToScan, $"surface_data.txt"), RelativeFilePath, RegexService.ExtractAllSurfaceData(currentFileContent));
                AppendOutputFileWithSensitiveData(Path.Combine(PathToScan, $"pii_data.txt"), RelativeFilePath, RegexService.ExtractAllPÌIData(currentFileContent));
            }
        }

        private static void AppendOutputFileWithSensitiveData(string OutputFilePath, string SourceFilename, Dictionary<string, List<string>> dictionaryList)
        {
            StringBuilder sensitiveOutputContent = new StringBuilder();
            bool printedFilename = false;
            bool printedCategoryName = false;
            foreach (var dictionaryGroup in dictionaryList.GroupBy(x => x.Key))
            {
                //PRINTS FILENAME ONLY IF FOUND SOMETHING
                foreach (var dictionaryItem in dictionaryGroup)
                {
                    if (dictionaryItem.Value.Count <= 0) continue;

                    printedCategoryName = false;
                    foreach (var value in dictionaryItem.Value.Distinct()) //VAvoiding duplicates
                    {
                        if (string.IsNullOrEmpty(value.Trim())) continue;

                        if (!printedFilename)
                        {
                            sensitiveOutputContent.AppendLine($"== [ FILE: {SourceFilename} ] ==\r\n");
                            printedFilename = true;
                        }

                        if (!printedCategoryName)
                        {
                            sensitiveOutputContent.AppendLine($"== [ CATEGORY: {dictionaryItem.Key} ] ==\r\n");

                            printedCategoryName = true;
                        }
                        sensitiveOutputContent.AppendLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\r\n");
                        sensitiveOutputContent.AppendLine(value + "\r\n");
                        sensitiveOutputContent.AppendLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\r\n");

                    }
                    if (printedFilename || printedCategoryName)
                        sensitiveOutputContent.AppendLine();
                }

                if (printedFilename || printedCategoryName)
                    sensitiveOutputContent.AppendLine();
            }
            File.AppendAllText(OutputFilePath, sensitiveOutputContent.ToString());
        }

        private static void ExtractMapFile(string outputPath, string JsMapContent)
        {

            using JsonDocument document = JsonDocument.Parse(JsMapContent);
            int contentIndex = 0;
            foreach (JsonElement source in document.RootElement.GetProperty("sources").EnumerateArray())
            {
                try
                {
                    string dirName = source.GetString()
                        .Replace("../", string.Empty)
                        .Replace("/..", string.Empty)
                        .Replace("/.", string.Empty)
                        .Replace("webpack:", string.Empty)
                        .Replace(":", string.Empty);

                    string dir = PathUtils.RemoveInvalidPathCharacters(Path.Combine(outputPath, dirName));

                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }


                    string file = PathUtils.RemoveInvalidFilenameCharacters(Path.GetFileName(source.GetString()));
                    string content = document.RootElement.GetProperty("sourcesContent")[contentIndex].GetString();
                    File.WriteAllText(Path.Combine(dir, file), content);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    contentIndex++;
                }


            }
        }
    }
}
