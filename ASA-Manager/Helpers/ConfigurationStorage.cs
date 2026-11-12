using System;
using System.IO;

namespace ARKServerCreationTool.Helpers
{
    internal static class ConfigurationStorage
    {
        public static string GetPath(string fileName)
        {
            string directory = Path.Combine(AppContext.BaseDirectory, "Configs");
            Directory.CreateDirectory(directory);
            string path = Path.Combine(directory, fileName);

            if (!File.Exists(path))
            {
                string oldPath = Path.Combine(AppContext.BaseDirectory, fileName);
                if (!File.Exists(oldPath))
                {
                    oldPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                }

                if (File.Exists(oldPath))
                {
                    File.Move(oldPath, path);
                }
            }

            return path;
        }
    }
}
