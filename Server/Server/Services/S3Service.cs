using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace Server.Services
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3Service(IConfiguration configuration)
        {
            var awsOptions = configuration.GetSection("AWS");
            var accessKeyId = awsOptions["AccessKeyId"];
            var secretAccessKey = awsOptions["SecretAccessKey"];
            var region = awsOptions["Region"];
            _bucketName = "pmsforradix";

            var s3Config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(region)
            };

            _s3Client = new AmazonS3Client(accessKeyId, secretAccessKey, s3Config);
        }


        public async Task<string> UploadFileAsync(string fileName, Stream fileStream)
        {
            try
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    InputStream = fileStream,
                    ContentType = "applications/pdf" // Adjust as needed
                };

                var response = await _s3Client.PutObjectAsync(putRequest);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
                }

                return string.Empty;
            }
            catch (AmazonS3Exception ex)
            {
                throw new Exception("Error uploading file to S3", ex);
            }
        }
    }
}
