namespace Pronia.Helper
{
    public static class DeleteFile
    {
        public static void Delete(string filePath)
        {
            try
            {
                  

                //create a file sample.txt in current working directory 
                if (!File.Exists(filePath))
                {
                    File.Create(filePath);
                }

                // Delete the file
                File.Delete(filePath);

                if (!File.Exists(filePath))
                {
                    Console.ForegroundColor= ConsoleColor.Green;
                    Console.WriteLine($"File {filePath} is successfully deleted.");
                    Console.ForegroundColor = ConsoleColor.White;

                }
            }
            catch (IOException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"File could not be deleted:");
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine(e.Message);
            }
        }
    }
}
