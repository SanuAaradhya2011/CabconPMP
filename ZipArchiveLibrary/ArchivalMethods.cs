using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zip;

namespace ZipArchiveLibrary
{
    public class ArchivalMethods
    {
        private static string archiveDir = AppDomain.CurrentDomain.BaseDirectory + "Configuration\\ResultFilesToSync\\Archive\\";    //@"C:\ResultFilesToSync\";
       
        /// <summary>
        /// Archive file to provided zip filepath, appending if archive exists 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool ArchiveAppendFile(string sourceFilePath, string archiveFilePath)
        {

            try
            {
                if (File.Exists(archiveFilePath))
                {
                    ZipFile zip = ZipFile.Read(archiveFilePath);

                    if (zip.ContainsEntry(sourceFilePath.Substring(sourceFilePath.LastIndexOf("\\") + 1)))
                        zip.RemoveEntry(sourceFilePath.Substring(sourceFilePath.LastIndexOf("\\") + 1));

                    zip.AddFile(sourceFilePath, "");
                    zip.Save();
                }
                else
                {
                    ZipFile zip = new ZipFile();
                    zip.AddFile(sourceFilePath, "");
                    zip.Save(archiveFilePath);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
