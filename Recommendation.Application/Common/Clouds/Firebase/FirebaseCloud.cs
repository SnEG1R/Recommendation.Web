﻿using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;

namespace Recommendation.Application.Common.Clouds.Firebase;

public class FirebaseCloud
{
    private readonly StorageClient _storageClient;
    private readonly string _bucket;

    public FirebaseCloud(JsonCredentialParameters credentialParameters, string bucket)
    {
        _bucket = bucket;
        _storageClient = StorageClient
            .Create(GoogleCredential.FromJsonParameters(credentialParameters));
    }

    public async Task<string> UploadFile(IFormFile file, string folderName)
    {
        await CreateFolder(folderName);
        var path = $"{folderName}/{file.FileName}";
        var stream = await GetStreamFile(file);
        var response = await _storageClient
            .UploadObjectAsync(_bucket, path, file.ContentType, stream);

        return response.MediaLink;
    }

    private async Task CreateFolder(string name)
    {
        const string folderCreationContentType = "application/x-directory";
        await _storageClient.UploadObjectAsync(_bucket, $"{name}/",
            folderCreationContentType, new MemoryStream());
    }

    private static async Task<Stream> GetStreamFile(IFormFile file)
    {
        var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        return stream;
    }
}