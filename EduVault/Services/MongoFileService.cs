using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.IO;
using System.Threading.Tasks;

namespace EduVault.Services
{
    public interface IMongoFileService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType = null);
        Task<(Stream Stream, string ContentType, string FileName)> DownloadFileAsync(string fileId);
        Task<(byte[] Data, string FileName, string ContentType)> DownloadFileWithMetadataAsync(string fileId);
        Task DeleteFileAsync(string fileId);
        Task<GridFSFileInfo> GetFileInfoAsync(string fileId);
        Task<string> GetFileNameAsync(string fileId);
    }
    public class MongoFileService: IMongoFileService
    {
        private readonly IGridFSBucket _gridFsBucket;

        public MongoFileService(IMongoDatabase database)
        {
            _gridFsBucket = new GridFSBucket(database);
        }

        // Загрузка файла в GridFS
        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType = null) //ЗАКОНЧИЛ НА ДОБАВЛЕНИИ МОНГО В ПРОЕКТ!!!!!!
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
        public async Task<(Stream Stream, string ContentType, string FileName)> DownloadFileAsync(string fileId)
        {
            if (!ObjectId.TryParse(fileId, out var objectId))
                return (null, null, null);

            var stream = new MemoryStream();

            // Загружаем файл в stream (без возвращаемого значения)
            await _gridFsBucket.DownloadToStreamAsync(objectId, stream);
            stream.Position = 0; // Возвращаем позицию в начало

            // Получаем метаданные файла отдельным запросом
            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", objectId);
            var fileInfo = await _gridFsBucket.Find(filter).FirstOrDefaultAsync();

            if (fileInfo == null)
                return (null, null, null);

            var contentType = fileInfo.Metadata?.GetValue("contentType", "application/octet-stream").AsString;
            var fileName = fileInfo.Filename;

            return (stream, contentType, fileName);
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
