using WebSocket.Domain.Entity;

namespace WebSocket.Domain.Validators;

public static class PhotoValidation
{
    public static bool IsValidPhotos(IEnumerable<Photo> photos)
    {
        return true;
    }
}