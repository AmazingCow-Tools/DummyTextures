
using System;

namespace com.amazingcow.dummytextures
{
    class MainClass
    {
        public static void Main(String[] args)
        {
            var dir = Environment.GetFolderPath(
                Environment.SpecialFolder.DesktopDirectory
            );

            Random rnd = new Random();
            int id = 0;
            for(int i = 6; i < 10; ++i)
            {
                var size  = (int)(Math.Pow(2, i));
                var count = rnd.Next(1, 5);

                for(int c = 0; c < count; ++c)
                {
                    var color = System.Drawing.Color.FromArgb(
                        rnd.Next(0, 255),
                        rnd.Next(0, 255),
                        rnd.Next(0, 255)
                    );

                    var texture = new Texture(size, size, color, id);
                    texture.SaveToFile(dir);

                    id++;
                }
            }
        }
    
    }//class MainClass

} // namespace com.amazingcow.dummytextures

