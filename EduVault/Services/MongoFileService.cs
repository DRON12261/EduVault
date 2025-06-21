using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EduVault.Services
{
    public interface IMongoFileService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType = null);
        Task<(Stream Stream, string ContentType, string FileName, string ErrorMessage)> DownloadFileAsync(string fileId);
        Task<(byte[] Data, string FileName, string ContentType)> DownloadFileWithMetadataAsync(string fileId);
        Task DeleteFileAsync(string fileId);
        Task<GridFSFileInfo> GetFileInfoAsync(string fileId);
        Task<string> GetFileNameAsync(string fileId);
    }
    public class MongoFileService: IMongoFileService
    {
        private readonly IGridFSBucket _gridFsBucket;
        private readonly ILogger<MongoFileService> _logger;

        public MongoFileService(IMongoDatabase database)
        {
            _gridFsBucket = new GridFSBucket(database);
        }

        // Загрузка файла в GridFS
        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType = null)
        {
            var options = new GridFSUploadOptions
            {
                Metadata = new BsonDocument
                {
                    { "content_type", contentType },
                    { "upload_date", DateTime.UtcNow }
                }
            };

            return (await _gridFsBucket.UploadFromStreamAsync(fileName, fileStream, options)).ToString();
        }

        // Скачивание файла
        public async Task<(Stream Stream, string ContentType, string FileName, string ErrorMessage)> DownloadFileAsync(string fileId)
        {
            try
            {
                // 1. Валидация ObjectId
                if (!ObjectId.TryParse(fileId, out var objectId))
                {
                    return (null, null, null, "Некорректный идентификатор файла");
                }

                // 2. Проверка существования файла
                var fileExists = await _gridFsBucket
                    .Find(Builders<GridFSFileInfo>.Filter.Eq("_id", objectId))
                    .AnyAsync();

                if (!fileExists)
                {
                    return (null, null, null, "Файл не найден в хранилище");
                }

                // 3. Загрузка файла
                var stream = new MemoryStream();
                await _gridFsBucket.DownloadToStreamAsync(objectId, stream);

                // 4. Проверка, что файл не пустой
                if (stream.Length == 0)
                {
                    stream.Dispose();
                    return (null, null, null, "Файл пустой или поврежден");
                }

                stream.Position = 0;

                // 5. Получение метаданных
                var fileInfo = await _gridFsBucket
                    .Find(Builders<GridFSFileInfo>.Filter.Eq("_id", objectId))
                    .FirstOrDefaultAsync();

                if (fileInfo == null)
                {
                    stream.Dispose();
                    return (null, null, null, "Метаданные файла не найдены");
                }

                // 6. Извлечение информации о файле
                var contentType = fileInfo.Metadata?.GetValue("contentType", "application/octet-stream").AsString;
                var fileName = fileInfo.Filename;

                // 7. Дополнительная валидация
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = $"file_{objectId}";
                }

                return (stream, contentType, fileName, null);
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "Ошибка MongoDB при загрузке файла {FileId}", fileId);
                return (null, null, null, "Ошибка доступа к хранилищу файлов");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неожиданная ошибка при загрузке файла {FileId}", fileId);
                return (null, null, null, "Внутренняя ошибка сервера");
            }
        }

        // Скачивание с получением метаданных
        public async Task<(byte[] Data, string FileName, string ContentType)> DownloadFileWithMetadataAsync(string fileId)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", fileId);
            var fileInfo = (await _gridFsBucket.FindAsync(filter)).FirstOrDefault();

            if (fileInfo == null)
                return (null, null, null);

            var data = await _gridFsBucket.DownloadAsBytesAsync(new ObjectId(fileId));
            return (data, fileInfo.Filename, fileInfo.Metadata?.GetValue("content_type", null)?.AsString);
        }

        // Удаление файла
        public async Task DeleteFileAsync(string fileId)
        {
            await _gridFsBucket.DeleteAsync(new ObjectId(fileId));
        }

        // Получение информации о файле
        public async Task<GridFSFileInfo> GetFileInfoAsync(string fileId)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", fileId);
            return (await _gridFsBucket.FindAsync(filter)).FirstOrDefault();
        }
        public async Task<string> GetFileNameAsync(string fileId)
        {
            if (!ObjectId.TryParse(fileId, out var objectId))
            {
                throw new ArgumentException("Invalid file ID format");
            }

            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", objectId);
            var fileInfo = await _gridFsBucket.Find(filter).FirstOrDefaultAsync();

            return fileInfo?.Filename; // Вернет null если файл не найден
        }
    }
}
