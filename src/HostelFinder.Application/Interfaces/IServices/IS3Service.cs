﻿using Microsoft.AspNetCore.Http;

public interface IS3Service
{
    public Task<string> UploadFileAsync(IFormFile file);
}