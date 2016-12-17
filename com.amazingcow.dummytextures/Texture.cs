using System;
using System.Drawing;

namespace com.amazingcow.dummytextures
{
    public class Texture 
    {
        // iVars //
        private int   m_width;
        private int   m_height;
        private Color m_color;
        private int   m_id;

        // CTOR // 
        public Texture(int width, int height, Color color, int id)
        {            
            m_width  = width;
            m_height = height;
            m_color  = color;
            m_id     = id;
        }


        // Public Methods //
        public void SaveToFile(String folderPath)
        {
            var fullpath = System.IO.Path.Combine(
                folderPath,
                m_id.ToString()
            );
            fullpath = System.IO.Path.ChangeExtension(fullpath, "png");

            using(Bitmap bitmap = new Bitmap(m_width, m_height))
            {
                using(Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using(SolidBrush brush = new SolidBrush(m_color))
                    {
                        var rect = new Rectangle(0, 0, m_width, m_height);
                        graphics.FillRectangle(brush, rect);
                    }
                }

                bitmap.Save(
                    fullpath, 
                    System.Drawing.Imaging.ImageFormat.Png
                );
            }
        }

    }// class Texture

}// namespace com.amazingcow.dummytextures

