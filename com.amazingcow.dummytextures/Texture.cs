//----------------------------------------------------------------------------//
//               █      █                                                     //
//               ████████                                                     //
//             ██        ██                                                   //
//            ███  █  █  ███        Texture.cs                                //
//            █ █        █ █        dummy-textures                            //
//             ████████████                                                   //
//           █              █       Copyright (c) 2016                        //
//          █     █    █     █      AmazingCow - www.AmazingCow.com           //
//          █     █    █     █                                                //
//           █              █       N2OMatt - n2omatt@amazingcow.com          //
//             ████████████         www.amazingcow.com/n2omatt                //
//                                                                            //
//                  This software is licensed as GPLv3                        //
//                 CHECK THE COPYING FILE TO MORE DETAILS                     //
//                                                                            //
//    Permission is granted to anyone to use this software for any purpose,   //
//   including commercial applications, and to alter it and redistribute it   //
//               freely, subject to the following restrictions:               //
//                                                                            //
//     0. You **CANNOT** change the type of the license.                      //
//     1. The origin of this software must not be misrepresented;             //
//        you must not claim that you wrote the original software.            //
//     2. If you use this software in a product, an acknowledgment in the     //
//        product IS HIGHLY APPRECIATED, both in source and binary forms.     //
//        (See opensource.AmazingCow.com/acknowledgment.html for details).    //
//        If you will not acknowledge, just send us a email. We'll be         //
//        *VERY* happy to see our work being used by other people. :)         //
//        The email is: acknowledgment_opensource@AmazingCow.com              //
//     3. Altered source versions must be plainly marked as such,             //
//        and must not be misrepresented as being the original software.      //
//     4. This notice may not be removed or altered from any source           //
//        distribution.                                                       //
//     5. Most important, you must have fun. ;)                               //
//                                                                            //
//      Visit opensource.amazingcow.com for more open-source projects.        //
//                                                                            //
//                                  Enjoy :)                                  //
//----------------------------------------------------------------------------//

//Usings
using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;

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
            var fullpath = Path.Combine(
                folderPath,
                m_id.ToString()
            );
            fullpath = Path.ChangeExtension(fullpath, "png");
            fullpath = PathHelper.Canonize(fullpath);

            Font f = new Font(FontFamily.GenericMonospace, 20);

            using(var bitmap = new Bitmap(m_width, m_height))
            {
                using(var graphics = Graphics.FromImage(bitmap))
                {
                    using(var brush = new SolidBrush(m_color))
                    {
                        var rect = new Rectangle(0, 0, m_width, m_height);
                        graphics.FillRectangle(brush, rect);

                        graphics.DrawString(
                            m_id.ToString(),
                            f, 
                            new SolidBrush(Color.Black),
                            new RectangleF(0, 0, m_width, m_height)                            
                        );
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

