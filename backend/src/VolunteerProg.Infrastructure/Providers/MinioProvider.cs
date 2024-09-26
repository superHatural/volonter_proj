using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using VolunteerProg.Application.FileProvider;
using VolunteerProg.Application.Providers;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    private const int SEMAPHORE_MAX_THREADS = 10;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken)
    {
        var semaphoreSlim = new SemaphoreSlim(SEMAPHORE_MAX_THREADS);
        var files = filesData.ToList();
        try
        {
            await IfBucketsNotExistCreateBucket(files, cancellationToken);

            var tasks = files.Select(async file =>
                await PutObject(file, semaphoreSlim, cancellationToken));

            var pathsResult = await Task.WhenAll(tasks);
            if (pathsResult.Any(p => p.IsFailure))
                return pathsResult.First().Error;
            var results = pathsResult.Select(p => p.Value).ToList();

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload files in minio, files amount: {amount}", files.Count);
            return Error.Failure("file.upload", "Fail to upload files in minio");
        }
    }

    public async Task<Result<IReadOnlyList<string>, Error>> GetFiles(
        IEnumerable<string> fileNames,
        CancellationToken cancellationToken)
    {
        var semaphoreSlim = new SemaphoreSlim(SEMAPHORE_MAX_THREADS);
        var files = fileNames.ToList();
        try
        {
            var tasks = files.Select(async file =>
                await GetObject(file, semaphoreSlim, cancellationToken));
            var linksResult = await Task.WhenAll(tasks);
            if (linksResult.Any(p => p.IsFailure))
                return linksResult.First().Error;
            var results = linksResult.Select(p => p.Value).ToList();
            return results;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail while trying to find a files in minio {amount}", files.Count);
            return Error.Failure("cannot.find.file", "fail while trying to find a file");
        }
    }

    public async Task<UnitResult<Error>> DeleteFile(
        IEnumerable<string> fileNames,
        CancellationToken cancellationToken)
    {
        var semaphoreSlim = new SemaphoreSlim(SEMAPHORE_MAX_THREADS);
        var files = fileNames.ToList();
        try
        {
            List<string> objectLinks = [];

            var tasks = files.Select(async file =>
                await DeleteObject(file, semaphoreSlim, cancellationToken));
            var linksResult = await Task.WhenAll(tasks);
            if (linksResult.Any(p => p.IsFailure))
                return linksResult.First().Error;
            return Result.Success<Error>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail while trying to find a files in minio {amount}", files.Count);
            return Error.Failure("cannot.find.file", "fail while trying to find a file");
        }
    }

    private async Task<Result<FilePath, Error>> PutObject(
        FileData fileData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(fileData.BucketName)
            .WithStreamData(fileData.Stream)
            .WithObjectSize(fileData.Stream.Length)
            .WithObject(fileData.FilePath.Path.Value);

        try
        {
            await _minioClient
                .PutObjectAsync(putObjectArgs, cancellationToken);
            return fileData.FilePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with path {path} in bucket {bucket}",
                fileData.FilePath.Path,
                fileData.BucketName);
            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task<Result<string, Error>> GetObject(
        string fileName,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);
        var bucketName = CheckCategory.WhatAType(null, fileName);
        var getObjectArgs = new PresignedGetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithExpiry(1000);
        try
        {
            return await _minioClient.PresignedGetObjectAsync(getObjectArgs).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to find a files in minio with path {path} in bucket {bucket}",
                fileName,
                bucketName);
            return Error.Failure("file.find", "fail to find file in minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task<UnitResult<Error>> DeleteObject(
        string fileName,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);
        var bucketName = CheckCategory.WhatAType(null, fileName);
        var deleteObjectArgs = new RemoveObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName);
        try
        {
            await _minioClient.RemoveObjectAsync(deleteObjectArgs, cancellationToken);
            _logger.LogInformation("Removed object {fileName} from bucket photos successfully", fileName);
            return Result.Success<Error>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail while trying to delete a file {fileName} in {bucketName}",
                fileName, bucketName);
            return Error.Failure("cannot.delete.file", "fail while trying to delete a file");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task IfBucketsNotExistCreateBucket(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken)
    {
        HashSet<string> bucketNames = [..filesData.Select(file => file.BucketName)];

        foreach (var bucketName in bucketNames)
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(bucketName);
            var bucketExist = await _minioClient
                .BucketExistsAsync(bucketExistArgs, cancellationToken);
            if (bucketExist == false)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);
                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }
        }
    }
}