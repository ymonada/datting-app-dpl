using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using WebSocket.db;
using WebSocket.dto;
using WebSocket.Service;

namespace WebSocket.Features.Photos;

public class PhotoService
{
    private readonly AppDbContext _context;
    public PhotoService(AppDbContext context)
    {
        _context = context;
    }
    // public async Task<ServiceResult<List<string>>> UpdateMyProfilePhotos(int userId, List<IFormFile> files)
    // {
    //     await DeletePhotos(userId);
    //     string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos");
    //     var userPhotos = new List<Photo>();
    //     foreach (var file in files)
    //     {
    //         using var md5 = System.Security.Cryptography.MD5.Create();
    //         using var stream1 = file.OpenReadStream();
    //         byte[] hash = await md5.ComputeHashAsync(stream1);
    //         var level0 = Guid.NewGuid();
    //         int level1 = hash[0] % 256 % 10;
    //         int level2 = hash[1] % 256 % 10;
    //         int level3 = hash[2] % 256 % 10;
    //         string nestedDirectory = Path.Combine(basePath
    //             , level1.ToString()
    //             , level2.ToString()
    //             , level3.ToString());
    //         if (!Directory.Exists(nestedDirectory))
    //         {
    //             Directory.CreateDirectory(nestedDirectory);
    //         }
    //         string fileName = $"{level0.ToString()
    //                              +level3.ToString()
    //                              +level2.ToString()
    //                              +level1.ToString()}{Path.GetExtension(file.FileName)}";
    //         string filePath = Path.Combine(nestedDirectory, fileName);
    //         await using var stream = new FileStream(filePath, FileMode.Create);
    //         await file.CopyToAsync(stream);
    //         string photoUrl = $"/photos/{level1}/{level2}/{level3}/{fileName}";
    //         userPhotos.Add(new Photo()
    //         {
    //             Url = photoUrl
    //             , UserId = userId
    //             , ContentType = file.ContentType
    //             
    //         });
    //     }
    //
    //     if (userPhotos.Count < 1)
    //     {
    //         return new ServiceResult<List<string>>() { IsSuccess = false, Message = "No photos" };
    //     }
    //     await _context.Photos.AddRangeAsync(userPhotos);
    //     await _context.SaveChangesAsync();
    //     return new ServiceResult<List<string>>()
    //     {
    //         IsSuccess = true
    //         , Message = "Photos updated"
    //         , Data = userPhotos.Select(u => u.Url).ToList()
    //     };
    // }
    //
    // public async Task<ServiceResult<List<PhotoDto>>> GetPhotosUrlByUserId(int userId)
    // {
    //     var userPhotos = await _context.Photos
    //         .Where(u => u.UserId == userId)
    //         .Select(u => new PhotoDto(u.Id, u.Url, u.ContentType))
    //         .ToListAsync();
    //     if (userPhotos.Count < 1)
    //     {
    //         return new ServiceResult<List<PhotoDto>>() { IsSuccess = false, Message = "No photos" };
    //     }
    //     return new ServiceResult<List<PhotoDto>>() { IsSuccess = true, Message = "Ok", Data = userPhotos };
    // }
    //
    // public async Task<ServiceResult<PhotoResultDto>> GetPhotoByUrl(string path)
    // {
    //     string decodedPath = Uri.UnescapeDataString(path);
    //     string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", decodedPath.TrimStart('/'));
    //     if (!File.Exists(filePath))
    //     {
    //         return new ServiceResult<PhotoResultDto> { IsSuccess = false, Message = "File not found" };
    //     }
    //     byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
    //     string mimeType = GetMimeType(filePath);
    //     return new ServiceResult<PhotoResultDto>()
    //     {
    //         IsSuccess = true,
    //         Data =  new PhotoResultDto(fileBytes, mimeType)
    //     };
    // }
    //
    // public async Task<ServiceResult<string>> DeletePhotos(int userId)
    // {
    //     var photos = await _context.Photos.Where(u => u.UserId == userId).ToListAsync();
    //     foreach (var photo in photos)
    //     {
    //         string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",photo.Url.TrimStart('/'));
    //         if (File.Exists(filePath))
    //         {
    //             await _context.Photos.Where(u=>u.Id == photo.Id ).ExecuteDeleteAsync();
    //             File.Delete(filePath);
    //         }
    //     }
    //     await _context.Photos.Where(u => u.UserId == userId).ExecuteDeleteAsync();
    //     await _context.SaveChangesAsync();
    //     return new ServiceResult<string>()
    //     {
    //         IsSuccess = true, Message = "Photos", Data = "OK"
    //     };
    // }
    // private string GetMimeType(string filePath)
    // {
    //     var provider = new FileExtensionContentTypeProvider();
    //     if (!provider.TryGetContentType(filePath, out var contentType))
    //     {
    //         contentType = "application/octet-stream";
    //     }
    //     return contentType;
    // }
}