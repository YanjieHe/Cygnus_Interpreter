using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Cygnus.Extensions
{
    internal static class UtilityFunctions
    {
        internal static Encoding GetEncoding(string encoding)
        {
            switch (encoding)
            {
                case "default":
                    return Encoding.Default;
                case "utf-8":
                case "utf8":
                    return Encoding.UTF8;
                case "unicode":
                    return Encoding.Unicode;
                case "ascii":
                    return Encoding.ASCII;
                default:
                    return Encoding.GetEncoding(encoding);
            }
        }
        internal static string GetFilePath(string currentDir, string path)
        {
            if (!Path.IsPathRooted(path))
            {
                var file = path;
                path = SearchFile(currentDir, file);
                if (path == null)
                    throw new DirectoryNotFoundException(file);
            }
            return path;
        }
        private static string SearchFile(string path, string file)
        {
            foreach (var f in Directory.EnumerateFiles(path))
                if (Path.GetFileName(f) == file)
                    return f;
            foreach (var folder in Directory.EnumerateDirectories(path))
            {
                var result = SearchFile(folder, file);
                if (result != null) return result;
            }
            return null;
        }
    }
}
