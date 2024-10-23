using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net;
using System.Text.RegularExpressions;
namespace BookApplication.Service.Implementation;

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
        //else if (IsBase64String(img))
        //{
        //    return img;
        //}
        else 
        {
            return img;
        }


        using (var ms = new MemoryStream(imageBytes))
        {
            using (Image image = Image.FromStream(ms))
            {
                string mimeType = GetMimeType(image);
                string base64Image = Convert.ToBase64String(imageBytes);
                return $"data:{mimeType};base64,{base64Image}";
            }
        }
    }

    // Helper function to check if a string is a valid URL
    private static bool IsUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    // Helper function to check if a string is a valid base64 string
    private static bool IsBase64String(string base64)
    {
        Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
        return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
    }

    private static string GetMimeType(Image image)
    {
        if (ImageFormat.Jpeg.Equals(image.RawFormat))
            return "image/jpeg";
        if (ImageFormat.Png.Equals(image.RawFormat))
            return "image/png";
        if (ImageFormat.Gif.Equals(image.RawFormat))
            return "image/gif";
        if (ImageFormat.Bmp.Equals(image.RawFormat))
            return "image/bmp";
        if (ImageFormat.Tiff.Equals(image.RawFormat))
            return "image/tiff";
    
        return "image/png";
    }
}