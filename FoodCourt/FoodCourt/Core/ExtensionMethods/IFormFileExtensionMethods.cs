using FoodCourt.Core.Types;

namespace FoodCourt.Core.ExtensionMethods
{
    public static class IFormFileExtensionMethods
    {
        public async static Task<string> SaveFile(this IFormFile file, string path)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(path, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return fileName;
        }
    }
}
