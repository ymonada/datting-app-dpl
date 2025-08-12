using Microsoft.EntityFrameworkCore;
using WebSocket.db;
using WebSocket.Domain.Entity;
using WebSocket.Domain.dto;
using WebSocket.Features.User;
using WebSocket.Service;
using WebSocket.Errors;

namespace WebSocket.Features.Like;

public class LikeService
{
    private readonly AppDbContext _context;
    private readonly UserService _userService;

    public LikeService(AppDbContext context, UserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task Get() => await Task.CompletedTask;
}
