using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace task_management.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Base64ImageAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            value = value?.ToString()?.Split(',')[1];
            // Step 1 - Null and Empty Check 
            if (value is null || string.IsNullOrWhiteSpace(value.ToString()) || string.IsNullOrEmpty(value.ToString()))
                return false;

            // Step 2 - Base64-encoded Check
            string base64String = value.ToString();
            if (!Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,2}$", RegexOptions.None))
                return false;

            // Step 3- Check for Image Type Only
            try
            {
                byte[] data = Convert.FromBase64String(base64String);
                using (var stream = new System.IO.MemoryStream(data))
                {
                    var image = System.Drawing.Image.FromStream(stream);
                    return image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid ||
                           image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Png.Guid ||
                           image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Gif.Guid;
                }
            }
            catch
            {
                // An exception occurred during decoding or validation
                return false;
            }


        }
    }
}
