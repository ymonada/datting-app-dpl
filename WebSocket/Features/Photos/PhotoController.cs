using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebSocket.Features.User;
using WebSocket.Service;

namespace WebSocket.Controlles;

[ApiController]
[Route("[controller]")]
public class PhotoController : ControllerBase
{
    private readonly UserService _userService;
    private readonly PhotoService _photoService;

    public PhotoController(UserService userService, PhotoService photoService)
    {
        _userService = userService;
        _photoService = photoService;
    }

    [HttpPut("/upload")]
    [Authorize]
    public async Task<IActionResult> UploadPhoto([FromForm] List<IFormFile> files)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _photoService.UpdateMyProfilePhotos(int.Parse(userId), files);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Message);
    }

    [HttpGet("/photo/{path}")]
    public async Task<IActionResult> GetPhoto(string path)
    {
        var result = await _photoService.GetPhotoByUrl(path);
        if (result.IsSuccess)
        {
            return File(result.Data.fileBytes, result.Data.type);
        }
        return BadRequest(result.Message);
    }

    [HttpDelete("/photos")]
    [Authorize]
    public async Task<IActionResult> DeletePhotos()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _photoService.DeletePhotos(int.Parse(userId));
        if (result.IsSuccess)
        {
            return StatusCode(204);
        }
        return BadRequest(result.Message);
    }
    [HttpDelete("/dropDatabase")]
    public async Task<IActionResult> DropDatabase()
    {
        var str = await _userService.DropDatabase();
        return Ok(str);
    }
}
