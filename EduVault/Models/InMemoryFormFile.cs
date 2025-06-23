namespace EduVault.Models
{
    public class InMemoryFormFile : IFormFile
    {
        private readonly Stream _stream;
        private readonly string _fileName;
        private readonly string _contentType;

        public InMemoryFormFile(Stream stream, string fileName, string contentType, long length)
        {
            _stream = stream;
            _fileName = fileName;
            _contentType = contentType;
            Length = length;
        }

        public string ContentType => _contentType;
        public string ContentDisposition => $"form-data; name=\"file\"; filename=\"{_fileName}\"";
        public IHeaderDictionary Headers => new HeaderDictionary();
        public long Length { get; }
        public string Name => "file"; // Должно совпадать с именем в форме (UploadedFile)
        public string FileName => _fileName;

        public Stream OpenReadStream() => _stream;

        public void CopyTo(Stream target) => _stream.CopyTo(target);

        public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
            => await _stream.CopyToAsync(target, cancellationToken);
    }
}
