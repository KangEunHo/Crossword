using UnityEngine;
using System.Text;  
using System.Collections;
using System.IO;

namespace HealingJam
{
    public static class FileHelper
    {
        public static bool CheckDirectory(string path)
        {
            var direct = Path.GetDirectoryName(path);
            return Directory.Exists(direct);
        }

        public static bool CheckAndCreateDirectory(string path)
        {
            var direct = Path.GetDirectoryName(path);
            if (Directory.Exists(direct))
            {
                return true;
            }
            else
            {
                Directory.CreateDirectory(direct);
                return false;
            }
        }

        public static void WriteAllBytes(string path, byte[] bytes)
        {
            CheckAndCreateDirectory(path);
            File.WriteAllBytes(path, bytes);
        }

        public static void WriteAllText(string path, string contents)
        {
            CheckAndCreateDirectory(path);
            File.WriteAllText(path, contents);
        }

        public static void WriteAllLines(string path, string[] contents)
        {
            CheckAndCreateDirectory(path);
            File.WriteAllLines(path, contents);
        }
    }
}