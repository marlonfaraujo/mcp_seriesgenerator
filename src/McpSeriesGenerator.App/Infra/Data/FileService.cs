using System.Text;

namespace McpSeriesGenerator.App.Infra.Data
{
    public static class FileService
    {
        public static string GetFilePath(string basePath, string filename)
        {
            if (!Directory.Exists(basePath))
            {
                throw new DirectoryNotFoundException($"folder not found: {basePath}");
            }
            return GetFullPath(filename);

            string GetFullPath(string filename)
            {
                var fullPath = Path.Combine(basePath, filename);
                if (!File.Exists(fullPath))
                {
                    throw new FileNotFoundException($"File not found: {fullPath}");
                }
                return fullPath;
            }
        }

        public static async Task<IEnumerable<T>> ReadLinesAsObjectsAsync<T>(string originalPath, Func<string, T> parser, CancellationToken cancellationToken = default)
        {
            var result = new List<T>();
            var lines = await ReadLinesAsync(originalPath, cancellationToken);
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    result.Add(parser(line));
                }
            }
            return result;
        }

        private static string CreateTemporaryFile(string extension = ".tmp")
        {
            string tempPath = Path.Combine(AppContext.BaseDirectory, "temp_upload");
            string tempFilePath = Path.Combine(tempPath, $"{Guid.NewGuid()}{extension}");
            Directory.CreateDirectory(tempPath);
            return tempFilePath;
        }

        public static async Task<IEnumerable<string>> ReadLinesAsync(string originalPath, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(originalPath))
            {
                throw new FileNotFoundException($"File not found: {originalPath}");
            }
            string tempFilePath = CreateTemporaryFile(".tmp");
            File.Copy(originalPath, tempFilePath, overwrite: true);
            var lines = new List<string>();
            try
            {
                using var stream = new FileStream(
                    tempFilePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite);

                using var reader = new StreamReader(stream);
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        lines.Add(line);
                    }
                }
            }
            finally
            {
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
            return lines;
        }

        public static async Task WriteAllLinesAsync(string filePath, IEnumerable<string> lines, CancellationToken cancellationToken = default)
        {
            await File.WriteAllLinesAsync(filePath,
                lines,
                cancellationToken);
            
        }

        public static bool FileValidate(string fileContent, string fileName, string requiredExtension = ".txt")
        {
            var extension = Path.GetExtension(fileName);
            if (!extension.Equals(requiredExtension, StringComparison.OrdinalIgnoreCase)) return false;
            string tempFile = CreateTemporaryFile(extension);
            try
            {
                using (FileStream fileStream = new(tempFile, FileMode.Create, FileAccess.ReadWrite))
                {
                    using (StreamWriter writer = new(fileStream, Encoding.UTF8, leaveOpen: true))
                    {
                        writer.Write(fileContent);
                        writer.Flush();
                    }
                    fileStream.Seek(0, SeekOrigin.Begin);
                    using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: false))
                    {
                        var lineFirst = reader.ReadLine();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                string tempLogFile = CreateTemporaryFile(extension);
                File.WriteAllText(tempLogFile, ex.Message, Encoding.UTF8);
                return false;
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }
    }
}
