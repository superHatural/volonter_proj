using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using VolunteerProg.Application.FileProvider;
using VolunteerProg.Application.Providers;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;
    private const string BUCKET_PHOTOS = "photos";

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<string, Error>> UploadFile(
        FileData fileData,
        CancellationToken cancellationToken)
    {
        try
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(BUCKET_PHOTOS);

            var bucketExists = await _minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);
            if (!bucketExists)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(BUCKET_PHOTOS);
                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(BUCKET_PHOTOS)
                .WithStreamData(fileData.Stream)
                .WithObjectSize(fileData.Stream.Length)
                .WithObject(fileData.ObjectName);
            var result = await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
            _logger.LogInformation("Files successfully created.");
            return result.ObjectName;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail while trying to create a file");
            return Error.Failure("cannot.create.file", "fail while trying to create a file");
        }
    }

    public async Task<Result<List<string>, Error>> GetFiles(
        List<string> fileNames,
        CancellationToken cancellationToken)
    {
        try
        {
            List<string> objectLinks = [];
            foreach (var photo in fileNames)
            {
                var args = new PresignedGetObjectArgs()
                    .WithBucket(BUCKET_PHOTOS)
                    .WithObject(photo)
                    .WithExpiry(1000);
                var presignedUrl = await _minioClient.PresignedGetObjectAsync(args);
                objectLinks.Add(presignedUrl);
            }

            _logger.LogInformation("Files successfully retrieved");
            return objectLinks;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail while trying to find a file");
            return Error.Failure("cannot.find.file", "fail while trying to find a file");
        }
    }

    public async Task<Result<string, Error>> DeleteFile(
        string fileName,
        CancellationToken cancellationToken)
    {
        try
        {
            var args = new RemoveObjectArgs()
                .WithBucket(BUCKET_PHOTOS)
                .WithObject(fileName);
            await _minioClient.RemoveObjectAsync(args, cancellationToken);
            _logger.LogInformation("Removed object {fileName} from bucket photos successfully", fileName);
            return fileName;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "fail while trying to delete a file");
            return Error.Failure("cannot.delete.file", "fail while trying to delete a file");
        }
    }
}