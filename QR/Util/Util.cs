using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace QR.Drawing.Util
{
    public class Utils
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
                    file.Read(file_bytes, 0, (int) file.Length);
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
            }
            catch (FileNonExistException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        static public RectangleF FitImage(Bitmap img, RectangleF target_rect)
        {
            float img_prop = (float) img.Width / img.Height;
            float rect_prop = target_rect.Width / target_rect.Height;
            if (img_prop > rect_prop)
            {
                //fit width
                float scale = target_rect.Width / img.Width;
                float new_width = target_rect.Width;
                float new_height = img.Height * scale;
                float new_x = target_rect.X;
                float new_y = target_rect.Y + (target_rect.Height - new_height) / 2.0f;
                return new RectangleF(new_x, new_y, new_width, new_height);
            }
            else
            {
                //fit height
                float scale = target_rect.Height / img.Height;
                float new_width = img.Width * scale;
                float new_height = target_rect.Height;
                float new_x = target_rect.X + (target_rect.Width - new_width) / 2.0f;
                float new_y = target_rect.Y;
                return new RectangleF(new_x, new_y, new_width, new_height);
            }
        }
    }


    [Serializable]
    public class FileNonExistException : Exception
    {
        public FileNonExistException()
        {
        }

        public FileNonExistException(string message) : base(message)
        {
        }

        public FileNonExistException(string message, Exception inner) : base(message, inner)
        {
        }

        protected FileNonExistException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}