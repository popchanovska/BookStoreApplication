using System.Text;

namespace BookApplication.Service.Implementation;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public static class UploadImage
{
    public static string ConvertImageToBase64(string img)
    {
        byte[] imageBytes = Encoding.UTF8.GetBytes(img); 

        // Load the image from the byte array using a memory stream
        using (var ms = new MemoryStream(imageBytes))
        {
            using (Image image = Image.FromStream(ms))
            {
                // Determine the mime type
                string mimeType = GetMimeType(image);

                // Convert the image bytes to a base64 string
                string base64Image = Convert.ToBase64String(imageBytes);

                // Create the Data URI
                return $"data:{mimeType};base64,{base64Image}";
            }
        }
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