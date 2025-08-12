using Microsoft.AspNetCore.StaticFiles;
using WebSocket.Contracts.User;
using WebSocket.db;
using WebSocket.Domain.Entity;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using WebSocket.Contracts.Extensions;

namespace WebSocket.Features.Photos;

public class PhotoService
{
    private readonly AppDbContext _context;
    
    public PhotoService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<ErrorOr<List<Photo>>> SavePhotoInMemoryAsync(Guid userId, ICollection<IFormFile> files)
    {
        if (files.Count < 1)
            return Error.Validation(description: "No photo uploaded");
        
        string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos");
        var userPhotos = new List<Photo>();
        foreach (var file in files)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            await using var stream1 = file.OpenReadStream();
            byte[] hash = await md5.ComputeHashAsync(stream1);
            var level0 = Guid.NewGuid();
            int level1 = hash[0] % 256 % 10;
            int level2 = hash[1] % 256 % 10;
            int level3 = hash[2] % 256 % 10;
            string nestedDirectory = Path.Combine(basePath
                , level1.ToString()
                , level2.ToString()
                , level3.ToString());
            if (!Directory.Exists(nestedDirectory))
            {
                Directory.CreateDirectory(nestedDirectory);
            }
            string fileName = $"{level0.ToString()
                                 +level3.ToString()
                                 +level2.ToString()
                                 +level1.ToString()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(nestedDirectory, fileName);
            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            string photoUrl = $"/photos/{level1}/{level2}/{level3}/{fileName}";
            userPhotos.Add(new Photo()
            {
                Url = photoUrl
                , UserId = userId
                , ContentType = file.ContentType
                
            });
        }
        return userPhotos;
    }
    
    public void DeletePhotoFromMemoryAsync(ICollection<Photo> deletingPhotos)
    {
        foreach (var filePath in deletingPhotos
                     .Select(photo => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",photo.Url.TrimStart('/')))
                     .Where(File.Exists))
            File.Delete(filePath);
    }

    // public async Task<ErrorOr<List<Photo>>> SavePhoto(Guid userId, ICollection<Photo> oldPhotos, ICollection<IFormFile> files, CancellationToken ct)
    // {
    //     var res = await SavePhotoInMemoryAsync(userId, files);
    //     if (res.IsError) return Error.Validation(description: "Error saving photo");
    //     
    //     await _context.Photos.AddRangeAsync(res.Value,ct);
    //     //
    //     // await _context.Photos
    //     //     .Select(x=>x.Id)
    //     //     .Where(x=> oldPhotos
    //     //         .Select(i=>i.Id)
    //     //         .Contains(x))
    //     //     .ExecuteDeleteAsync(ct);
    //     DeletePhotoFromMemoryAsync(oldPhotos);
    //     await _context.SaveChangesAsync(ct);
    //
    //     return res.Value;
    // }
    
    private string GetMimeType(string filePath)
    {
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(filePath, out var contentType))
        {
            contentType = "application/octet-stream";
        }
        return contentType;
    }
}