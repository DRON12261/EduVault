using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.IO;
using System.Threading.Tasks;

namespace EduVault.Services
{
    public class MongoFileService
    {
        private readonly IGridFSBucket _gridFsBucket;

        public MongoFileService(IMongoDatabase database)
        {
            _gridFsBucket = new GridFSBucket(database);
        }

        // Загрузка файла в GridFS
        public async Task<ObjectId> UploadFileAsync(Stream fileStream, string fileName, string contentType = null) //ЗАКОНЧИЛ НА ДОБАВЛЕНИИ МОНГО В ПРОЕКТ!!!!!!
        {
            var options = new GridFSUploadOptions
            {
                Metadata = new BsonDocument
                {
                    { "content_type", contentType },
                    { "upload_date", DateTime.UtcNow }
                }
            };

            return await _gridFsBucket.UploadFromStreamAsync(fileName, fileStream, options);
        }

        // Скачивание файла
        public async Task<byte[]> DownloadFileAsync(ObjectId fileId)
        {
            return await _gridFsBucket.DownloadAsBytesAsync(fileId);
        }

        // Скачивание с получением метаданных
        public async Task<(byte[] Data, string FileName, string ContentType)> DownloadFileWithMetadataAsync(ObjectId fileId)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", fileId);
            var fileInfo = (await _gridFsBucket.FindAsync(filter)).FirstOrDefault();

            if (fileInfo == null)
                return (null, null, null);

            var data = await _gridFsBucket.DownloadAsBytesAsync(fileId);
            return (data, fileInfo.Filename, fileInfo.Metadata?.GetValue("content_type", null)?.AsString);
        }

        // Удаление файла
        public async Task DeleteFileAsync(ObjectId fileId)
        {
            await _gridFsBucket.DeleteAsync(fileId);
        }

        // Получение информации о файле
        public async Task<GridFSFileInfo> GetFileInfoAsync(ObjectId fileId)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", fileId);
            return (await _gridFsBucket.FindAsync(filter)).FirstOrDefault();
        }
    }
}
