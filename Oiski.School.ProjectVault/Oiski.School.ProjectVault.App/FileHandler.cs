using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oiski.School.ProjectVault.App
{
    /// <summary>
    /// Defines a tool to manipulate the contents of a file
    /// </summary>
    public class FileHandler
    {
        /// <summary>
        /// Initialize a new instance of type <see cref="FileHandler"/> that generates a fully qualified path to the associated file at; <i><paramref name="folderPath"/>/<paramref name="fileName"/>.*</i> if one does not already exist.
        /// </summary>
        /// <param name="folderPath">The path to the folder the handler should place the file in</param>
        /// <param name="fileName">The name of the file the handler should manipulate (<strong>NOTE:</strong> <i><paramref name="fileName"/> must include the file extension</i>)</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public FileHandler(string folderPath, string fileName)
        {
            FilePath = $"{folderPath}\\{fileName}";

            //  Validate if the file already exists. If it doesn't create a new file
            if (!System.IO.File.Exists($"{folderPath}\\{fileName}"))
            {
                if (!System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }

                System.IO.File.Create(FilePath).Close();
            }
        }

        /// <summary>
        /// The fully qualified file path
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Returns the <see cref="DirectoryInfo"/> <see langword="object"/> that contains the associated file at <see cref="FilePath"/>
        /// </summary>
        public DirectoryInfo Directory
        {
            get
            {
                return File.Directory;
            }
        }

        /// <summary>
        /// Returns the <see cref="FileInfo"/> <see langword="object"/> for the associated file at <see cref="FilePath"/>
        /// </summary>
        public FileInfo File
        {
            get
            {
                return new FileInfo(FilePath);
            }
        }

        /// <summary>
        /// Delete the associated file
        /// </summary>
        /// <returns><see langword="True"/> if the file could be deleted; Otherwise, if not, <see langword="false"/></returns>
        public bool Delete()
        {
            if (System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Creates a file at <see cref="FilePath"/>
        /// </summary>
        /// <param name="replace">Whether or not to override an existing file if it exists</param>
        /// <returns><see langword="True"/> if the file does not exist, or <paramref name="replace"/> is <see langword="true"/>, and the file could be created; Otherwise, if not, <see langword="false"/></returns>
        public bool Create(bool replace = false)
        {
            return Create(FilePath, replace);
        }
        /// <summary>
        /// Creates a file at the specified <paramref name="path"/> location
        /// </summary>
        /// <param name="path">The fully qualified path to the file</param>
        /// <param name="replace">Whether or not to override an existing file if it exists</param>
        /// <returns><see langword="True"/> if the file does not exist, or <paramref name="replace"/> is <see langword="true"/>, and the file could be created; Otherwise, if not, <see langword="false"/></returns>
        public bool Create(string path, bool replace = false)
        {
            if (!System.IO.File.Exists(path) || replace)
            {
                System.IO.File.Create(path);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Write <paramref name="text"/> to the linked file at <see cref="FilePath"/>
        /// </summary>
        /// <param name="text">The content to write</param>
        /// <param name="append"><see langword="true"/> to append <paramref name="text"/> to the file; <see langword="false"/> to override file</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public void Write(string text, bool append = false)
        {
            using StreamWriter file = new StreamWriter(FilePath, append);
            file.Write(text);
        }

        /// <summary>
        /// Write <paramref name="text"/> to the file, followed by a <i>newline terminator</i>
        /// </summary>
        /// <param name="text">The content to write</param>
        /// <param name="append"><see langword="true"/> to append <paramref name="text"/> to the file; <see langword="false"/> to override file</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        public void WriteLine(string text, bool append = false)
        {
            using StreamWriter file = new StreamWriter(FilePath, append);
            file.WriteLine(text);
        }

        /// <summary>
        /// Insert <paramref name="text"/> at the specified <paramref name="lineNumber"/> in the linked file at <see cref="FilePath"/>
        /// </summary>
        /// <param name="text">The content to write</param>
        /// <param name="lineNumber">The line index</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public void InsertLine(string text, int lineNumber)
        {
            string[] lines = ReadLines();

            if (lineNumber < lines.Length)
            {
                string updatedFileContent = string.Empty;

                for (int i = 0; i < lines.Length; i++)
                {
                    if (i == lineNumber)
                    {
                        updatedFileContent += $"{text}{((i != lines.Length - 1) ? (Environment.NewLine) : (string.Empty))}";   //  Adding the new line to the updated file
                    }
                    else
                    {
                        updatedFileContent += $"{lines[i]}{((i != lines.Length - 1) ? (Environment.NewLine) : (string.Empty))}";  //  Readding existing lines to the updated file
                    }
                }

                Write(updatedFileContent, false);
            }
            else
            {
                throw new IndexOutOfRangeException("Line Number must be positive and less than the length of the file in lines");
            }
        }

        /// <summary>
        /// Replace a line in the linked file with <paramref name="text"/> at the specified <paramref name="lineNumber"/>
        /// </summary>
        /// <param name="text">The content to write</param>
        /// <param name="lineNumber">The line index</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public void UpdateLine(string text, int lineNumber)
        {
            string[] lines = ReadLines();

            if (lineNumber < lines.Length)
            {
                string updatedFileContent;

                string fileContent = ReadAll();
                updatedFileContent = fileContent.Replace(lines[lineNumber], text);

                Write(updatedFileContent, false);
            }
            else
            {
                throw new IndexOutOfRangeException("Line Number must be positive and less than the length of the file in lines");
            }
        }

        /// <summary>
        /// Delete a line in the linked file at the specified <paramref name="lineNumber"/>
        /// </summary>
        /// <param name="lineNumber">The line index</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public void DeleteLine(int lineNumber)
        {
            string[] lines = ReadLines();

            if (lineNumber < lines.Length)
            {
                string updatedFileContent = string.Empty;

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i] != lines[lineNumber])   //  Readding all lines except from the line that should be deleted
                    {
                        updatedFileContent += $"{lines[i]}{((i != lines.Length - 1) ? (Environment.NewLine) : (string.Empty))}";
                    }
                }

                Write(updatedFileContent, false);
            }
            else
            {
                throw new IndexOutOfRangeException("Line Number must be positive and less than the length of the file in lines");
            }
        }

        /// <summary>
        /// Read the content of the linked file as lines (<i>Determined by a <see cref="Environment.NewLine"/> terminator</i>)
        /// </summary>
        /// <returns>The content of the file as a collection of <see langword="strings"/>, seperated by a <i>newline termintor</i></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        public string[] ReadLines()
        {
            string[] lines;
            lines = ReadAll().Split(Environment.NewLine);

            return lines;
        }

        /// <summary>
        /// Read the content of the linked file at <see cref="FilePath"/>
        /// </summary>
        /// <returns>The content of the file as a <see langword="string"/></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        public string ReadAll()
        {
            StreamReader reader = new StreamReader(FilePath);
            string fileContent = reader.ReadToEnd();
            reader.Close();

            return fileContent;
        }

        /// <summary>
        /// Find a specific line within the linked file at <see cref="FilePath"/>
        /// </summary>
        /// <param name="lineNumber">The line index</param>
        /// <returns>The line at the specified <paramref name="lineNumber"/> as a <see langword="string"/></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        public string FindLine(int lineNumber)
        {
            string[] lines = ReadLines();

            if (lineNumber < lines.Length)
            {
                return lines[lineNumber];
            }

            throw new IndexOutOfRangeException("Line Number must be positive and less than the length of the file in lines");
        }

        /// <summary>
        /// Search for a specific key in the linked file at <see cref="FilePath"/>
        /// </summary>
        /// <param name="searchKey">The key to search for</param>
        /// <returns>The first line that contains the specified <paramref name="searchKey"/></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        public string FindLine(string searchKey)
        {
            if (searchKey != null)
            {
                string[] lines = ReadLines();

                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line) && line.ToLower().Contains(searchKey.ToLower()))
                    {
                        return line;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Search for a specific key in the linked file at <see cref="FilePath"/>
        /// </summary>
        /// <param name="searchKey">The key to search for</param>
        /// <returns>A <see langword="readonly"/> collection of all lines containing the specified <paramref name="searchKey"/></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        public IReadOnlyList<string> FindLines(string searchKey)
        {
            List<string> foundLines = null;

            if (searchKey != null)
            {
                string[] lines = ReadLines();
                foundLines = new List<string>();

                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line) && line.ToLower().Contains(searchKey.ToLower()))
                    {
                        foundLines.Add(line);
                    }
                }
            }

            return foundLines;
        }

        /// <summary>
        /// Find the line number of <paramref name="line"/>
        /// </summary>
        /// <param name="line"></param>
        /// <returns>The zero-based index of the first occurence of <paramref name="line"/> within the file, if found; Otherwise, -1</returns>
        public int GetLineNumber(string line)
        {
            return ReadLines().ToList().IndexOf(line);
        }
    }
}
