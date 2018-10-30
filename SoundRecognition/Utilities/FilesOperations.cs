using System.IO;

namespace SoundRecognition
{
    internal class FilesOperations
    {
        public static bool IsFileLocked(FileInfo file)
        {
            bool isFileLocked = false;
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    // File is free and can be accessed.
                }
            }
            catch (IOException)
            {
                // The file is unavailable because it is:
                // still being written to
                // or being processed by another thread
                // or does not exist (has already been processed).
                isFileLocked = true;
            }

            return isFileLocked;
        }
    }
}
