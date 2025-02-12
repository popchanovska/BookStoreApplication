using System;
using System.IO;
using System.Net;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tiff;

public static class UploadImage
{
    public static string ConvertImageToBase64(string img)
    {
        byte[] imageBytes;

        if (IsUrl(img))
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                imageBytes = webClient.DownloadData(img);
            }
        }
        else if (File.Exists(img))
        {
            imageBytes = File.ReadAllBytes(img);
        }
        else
        {
            return img;
        }

        using (var ms = new MemoryStream(imageBytes))
        {
            IImageFormat format = Image.DetectFormat(ms);
            string mimeType = format != null ? GetMimeType(format) : "image/png";
            string base64Image = Convert.ToBase64String(imageBytes);
            return $"data:{mimeType};base64,{base64Image}";
        }
    }

    private static bool IsUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    private static string GetMimeType(IImageFormat format)
    {
        if (format == JpegFormat.Instance)
            return "image/jpeg";
        if (format == PngFormat.Instance)
            return "image/png";
        if (format == GifFormat.Instance)
            return "image/gif";
        if (format == BmpFormat.Instance)
            return "image/bmp";
        if (format == TiffFormat.Instance)
            return "image/tiff";
        return "image/png";
    }
}