namespace VolunteerProg.Domain.Shared;

public static class CheckCategory
{
    public static string WhatAType(string? extension, string? path)
    {
        return (extension ?? Path.GetExtension(path)) switch
        {
            ".jpg" => Constants.BUCKET_PHOTOS,
            ".png" => Constants.BUCKET_PHOTOS,
            ".jpeg" => Constants.BUCKET_PHOTOS,
            ".mp4" => Constants.BUCKET_VIDEOS,
            ".mov" => Constants.BUCKET_VIDEOS,
            ".wmv" => Constants.BUCKET_VIDEOS,
            _ => Constants.BUCKET_OTHER
        };
    }
}