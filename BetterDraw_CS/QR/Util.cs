using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace QR.Drawing.Util
{
    class Utils
    {
        static public string ReadFileToString(string path)
        {
            byte[] file_bytes;
            char[] file_chars;
            try
            {
                if (File.Exists(path))
                {
                    FileStream file = new FileStream(path, FileMode.Open);
                    file_bytes = new byte[file.Length];
                    file.Seek(0, SeekOrigin.Begin);
                    file.Read(file_bytes, 0, (int)file.Length);
                    file.Close();

                    Decoder decoder = Encoding.UTF8.GetDecoder();
                    int char_length = decoder.GetCharCount(file_bytes, 0, file_bytes.Length);
                    file_chars = new char[char_length];
                    decoder.GetChars(file_bytes, 0, file_bytes.Length, file_chars, 0);
                    return new string(file_chars);
                }
                else
                {
                    throw (new FileNonExistException(
                        "File \"" + path + "\" is not existing, please check out if the path is correct."));
                }
                
            }catch(FileNonExistException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            
        }
    }


    [Serializable]
    public class FileNonExistException : Exception
    {
        public FileNonExistException() { }
        public FileNonExistException(string message) : base(message) { }
        public FileNonExistException(string message, Exception inner) : base(message, inner) { }
        protected FileNonExistException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}
