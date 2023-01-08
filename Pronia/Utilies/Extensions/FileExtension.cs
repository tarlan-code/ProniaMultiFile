namespace Pronia.Utilies.Extensions
{
    public static class FileExtension
    {
        public static bool CheckType(this IFormFile file, string type)
            => file.ContentType.Contains(type);
        public static bool CheckSize(this IFormFile file, int kb)
            => kb*1024 > file.Length;

        public static string SaveFile(this IFormFile file,string path)
        {
            string filename = ChangeName(file.FileName);

            using(FileStream fs = new FileStream(Path.Combine(path, filename), FileMode.Create))
            {
                file.CopyTo(fs);
            }
            return filename;
        }

        static string ChangeName(string oldName)
        {
            string extension = oldName.Substring(oldName.LastIndexOf('.'));

            if (oldName.Length < 32) oldName = oldName.Substring(0, oldName.LastIndexOf('.'));
            
            else oldName = oldName.Substring(0, 31);

            return Guid.NewGuid().ToString() + oldName + extension;
        }
    }
}
