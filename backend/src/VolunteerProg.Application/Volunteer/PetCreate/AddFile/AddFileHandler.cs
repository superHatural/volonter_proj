using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using VolunteerProg.Application.Database;
using VolunteerProg.Application.FileProvider;
using VolunteerProg.Application.Providers;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Shared.Ids;

namespace VolunteerProg.Application.Volunteer.PetCreate.AddFile;

public class AddFileHandler
{
    private readonly IFileProvider _provider;
    private readonly ILogger<AddFileHandler> _logger;
    private readonly IVolunteersRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddFileHandler(
        IFileProvider provider,
        ILogger<AddFileHandler> logger,
        IVolunteersRepository repository,
        IUnitOfWork unitOfWork)
    {
        _provider = provider;
        _logger = logger;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IReadOnlyList<FilePath>, Error>> Handle(
        AddFileCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        try
        {
            var volunteer =
                await _repository.GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
            if (volunteer.IsFailure)
                return volunteer.Error;

            List<FilePathData> filesPath = [];
            List<FileData> files = [];
            
            foreach (var file in command.Files)
            {
                var extension = Path.GetExtension(file.FileName);
                
                var filePath =
                    FilePath.Create(NotEmptyVo.Create(Guid.NewGuid().ToString()).Value, extension);
                if (filePath.IsFailure)
                    return filePath.Error;
                
                var filePathData = new FilePathData(filePath.Value, false);
                filesPath.Add(filePathData);
                
                var fileToUpload = new FileData(file.Stream, CheckCategory.WhatAType(extension, null), filePath.Value);
                files.Add(fileToUpload);
            }
            var petResult = volunteer.Value.AddFilePet(PetId.Create(command.PetId), new ValueObjectList<FilePathData>(filesPath));
            if (petResult.IsFailure)
                return petResult.Error;
            
            await _unitOfWork.SaveChanges(cancellationToken);
            
            var fileResult = await _provider.UploadFiles(files, cancellationToken);
            if (fileResult.IsFailure)
                return fileResult.Error;
            
            transaction.Commit();
            
            return fileResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Cannot add file to pet - {id} in transaction", command.PetId);

            transaction.Rollback();
            return Error.Failure("Cannot add file to pet", "pet.file.failure");
        }
    }
}