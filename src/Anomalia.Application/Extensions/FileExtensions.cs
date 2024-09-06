using Microsoft.AspNetCore.Http;

namespace Anomalias.Application.Extensions;

public static class FileExtensions
{
    public static string ConvertFileToBase64(this IFormFile? file)
    {

        if (file is null) return string.Empty;
        if (file.Length <= 0) return string.Empty;
        try
        {
            using var ms = new MemoryStream();
            file.OpenReadStream().CopyTo(ms);
            var bytes = ms.ToArray();
            var bytesToBase64 = Convert.ToBase64String(bytes, 0, bytes.Length);
            string content = "data:" + file.ContentType + ";base64,";
            return bytesToBase64;
        }
        catch
        {
            return string.Empty;
        }
    }

    public static byte[] ConvertFileToByte(this IFormFile? file)
    {

        ArgumentNullException.ThrowIfNull(file);
        if (file.Length <= 0) throw new ArgumentOutOfRangeException(nameof(file));
        try
        {
            using var ms = new MemoryStream();
            file.OpenReadStream().CopyTo(ms);
            var bytes = ms.ToArray();

            return bytes;
        }
        catch
        {
            throw new ArgumentOutOfRangeException(nameof(file));
        }
    }
}

