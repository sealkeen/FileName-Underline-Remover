using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/// <summary>
/// Программа для замены знака подчёркивания '_' из наименований папок и файлов на ' ', содержащихся в DIRECTORY_FULL_PATH
/// </summary>
namespace UnderlineRemover
{
    class Program
    {
        const string DIRECTORY_FULL_PATH = @"E:\Sealkeen\Music";

        static void Main(string[] args)
        {
            Program pr = new Program();

            pr.HandleDirectoryRecursive(new DirectoryInfo(DIRECTORY_FULL_PATH));

            #region Uncomment this to create lots of files and folders for testing inside the specified directory
            //Console.WriteLine(Directory.GetCurrentDirectory());
            //pr.CreateLotsOfFolders(new DirectoryInfo(DIRECTORY_FULL_PATH));
            //Console.WriteLine(errorCounter);
            #endregion

            Console.WriteLine(pr);

            Console.ReadKey();
        }

        Random rnd = new Random();
        int errorCounter = 0; //I used the var to test how many exceptions might be thrown while creating lots of files and folders with "_"
        int totalFilesRenamed = 0;
        int totalDirectoriesRenamed = 0;


        private void HandleDirectoryRecursive(DirectoryInfo currentDir)
        {
            
            RenameFiles(currentDir);

            DirectoryInfo[] childrenDirectories = currentDir.GetDirectories();

            if (childrenDirectories.Count() == 0)
            { Console.WriteLine($"One more folder was scanned."); return; }
            else
                RenameDirectories(ref childrenDirectories);
            

            foreach (DirectoryInfo aChildDir in childrenDirectories)
            {
                HandleDirectoryRecursive(aChildDir);
            }
        }

        private void RenameFiles(DirectoryInfo currentDir)
        {
            FileInfo[] containedFiles = currentDir.GetFiles();
            if (containedFiles.Length == 0)
                return;

            string fileName;
            foreach (FileInfo currentFile in containedFiles)
            {
                fileName = currentFile.Name;
                if (fileName.Contains('_'))
                {
                    //Rename the file using MoveTo
                    string newName = fileName.Replace('_', ' ');
                    string newFullName = currentDir.FullName + '\\'+newName;

                    currentFile.MoveTo(newFullName);

                    totalFilesRenamed++;
                }
            }
        }

        private void RenameDirectories(ref DirectoryInfo[] childrenDirectories)
        {
            //We have already checked for .Length == 0 in HandleDirectories()

            string dirName;
            foreach (DirectoryInfo currentDir in childrenDirectories)
            {
                dirName = currentDir.Name;
                if (dirName.Contains('_'))
                {
                    //Rename the file using MoveTo
                    string newName = dirName.Replace('_', ' ');
                    string newFullName = currentDir.Parent.FullName + '\\' + newName;

                    currentDir.MoveTo(newFullName);

                    totalDirectoriesRenamed++;
                }
            }
        }

#region TEST THIS TO CREATE LOTS OF FILES AND FOLDERS

        private void CreateLotsOfFolders(DirectoryInfo curDir)
        {
            try
            {
                int dirCount = rnd.Next(50);
                for (int c = 0; c < dirCount; ++c)
                {
                    DirectoryInfo theDir = new DirectoryInfo(curDir.FullName);
                    switch (rnd.Next(3))
                    {
                        case 0:
                            theDir = Directory.CreateDirectory(curDir + @"\1_" + rnd.Next(rnd.Next()).ToString());
                            break;
                        case 1:
                            theDir = Directory.CreateDirectory(curDir + @"\2_" + rnd.Next(rnd.Next()).ToString());
                            break;
                        case 2:
                            theDir = Directory.CreateDirectory(curDir + @"\2_" + rnd.Next(rnd.Next()).ToString());
                            break;
                    }

                    CreateLotsOfFiles(theDir);
                }
            }
            catch { errorCounter++; }
        }
        private void CreateLotsOfFiles(DirectoryInfo curDir)
        {

            try
            {
                int filesCount = rnd.Next(50);
                for (int c = 0; c < filesCount; ++c)
                {
                    switch (rnd.Next(3))
                    {
                        case 0:
                            File.Create(curDir.FullName + @"\1_" + rnd.Next(rnd.Next()).ToString());
                            break;
                        case 1:
                            File.Create(curDir.FullName + @"\2_" + rnd.Next(rnd.Next()).ToString());
                            break;
                        case 2:
                            File.Create(curDir.FullName + @"\3_3" + rnd.Next(rnd.Next()).ToString());
                            break;
                    }
                }
            }
            catch { errorCounter++; }
        }

#endregion


        public override string ToString()
        {
            return $"Directories renamed Total = {totalDirectoriesRenamed}\nFiles renamed Total = {totalFilesRenamed}";
        }
    }
}
