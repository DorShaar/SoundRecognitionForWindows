using System;
using System.Collections.Generic;
using System.IO;

namespace SoundRecognition
{
     public class FilePath
     {
          public string Name { get; }
          public string DirectoryPath { get; }

          public static FilePath CreateFilePathWithPrefix(string directoryPath, string prefix, string nameWithExtension)
          {
               return new FilePath(directoryPath, prefix, nameWithExtension);
          }

          public static FilePath CreateFilePath(string fullPath)
          {
               return new FilePath(fullPath);
          }

          public static FilePath CreateFilePath(string path, string name)
          {
               return new FilePath(path, name);
          }

          public string NameWithoutExtension
          {
               get
               {
                    return Path.GetFileNameWithoutExtension(Name);
               }
          }

          public string FileFullPath
          {
               get
               {
                    return Path.Combine(DirectoryPath, Name);
               }
          }

          /// <summary>
          /// Extension with dot.
          /// </summary>
          public string Extension
          {
               get
               {
                    return Path.GetExtension(Name).ToLower();
               }
          }

          public static List<FilePath> GetFiles(string directoryPath)
          {
               List<FilePath> filesPath = new List<FilePath>();

               if (Directory.Exists(directoryPath))
               {
                    FileAttributes attr = File.GetAttributes(directoryPath);

                    if (attr.HasFlag(FileAttributes.Directory))
                    {
                         foreach(string file in Directory.GetFiles(directoryPath))
                         {
                              filesPath.Add(new FilePath(file));
                         }
                    }
                    else
                    {
                         Console.WriteLine($"{directoryPath} is not a directory. Cannot get files");
                    }
               }
               else
               {
                    Console.WriteLine($"{directoryPath} does not exist. Cannot get files");
               }

               return filesPath;
          }

          private FilePath(string directoryPath, string prefix, string nameWithExtension)
          {
               DirectoryPath = directoryPath;
               Name = $"{prefix}_{Path.GetFileName(nameWithExtension)}";
          }

          private FilePath(string path, string name)
          {
               DirectoryPath = path;
               Name = Path.GetFileName(name);
          }

          private FilePath(string fullPath)
          {
               DirectoryPath = Path.GetDirectoryName(fullPath);
               Name = Path.GetFileName(fullPath);
          }
     }
}
