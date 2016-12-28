//----------------------------------------------------------------------------//
//               █      █                                                     //
//               ████████                                                     //
//             ██        ██                                                   //
//            ███  █  █  ███        RunOptions.cs                             //
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

//Usinsg
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace com.amazingcow.dummytextures
{
    public class RunOptions
    {               
        // Constants //
        private const String kDefaultOutputPath = "./DummyTextures_Output";


        // iVars //
        private bool       m_verbose;
        private List<Size> m_texturesEntries;
        private String     m_outputPath;

        private Random m_rnd;


        // CTOR //
        public RunOptions()
        {         
            m_verbose         = false;
            m_texturesEntries = new List<Size>();
            m_outputPath      = null;

            m_rnd = new Random();
        }


        // Public Methods //
        public void SetVerbose() { m_verbose = true; }
        public bool isVerbose () { return m_verbose; }

        public void AddTextureEntry(String entry) { ParseTextureEntry(entry); }
        public List<Size> GetTextureEntries()     { return m_texturesEntries; }

        public void SetOutputPath(String path) { m_outputPath = path; }
        public String GetOutputPath()          { return m_outputPath; }


        // Create Textures //
        public void CreateTextures()
        {
            //Get the absolute output path.
            if(m_outputPath == null)
                m_outputPath = kDefaultOutputPath;

            m_outputPath = PathHelper.Canonize(m_outputPath);

            //Create a Directory to put the textures.
            if(!Directory.Exists(m_outputPath))
                Directory.CreateDirectory(m_outputPath);


            //Create the textures.
            var texturesCount = m_texturesEntries.Count;
            for(int i = 0; i < texturesCount; ++i)
            {  
                var size = m_texturesEntries[i];
                var texture = new Texture(
                    size.w, 
                    size.h, 
                    GetRandomColor(), 
                    i
                );

                if(isVerbose())
                {
                    var msg = String.Format(
                        "Generating Texture ({0}) of ({1}) - Width:{2} Height:{3}",
                        i, 
                        texturesCount,
                        size.w,
                        size.h
                    );
                    Console.WriteLine(msg);
                }

                texture.SaveToFile(m_outputPath);
            }

            if(isVerbose())
                Console.WriteLine("Done...");
        }


        // Helper Methods //
        private void ParseTextureEntry(String entry)
        {            
            var components = entry.Replace("\n", "").Split('x');
            if(components.Length != 3)
            {
                var msg = String.Format(
                    "Texture Configuration must be in format of:\n\t{0}\nFound:\n\t{1}",
                    "[Width]x[Height]x[Quantity of Textures]",
                    entry
                );
                throw new InvalidProgramException(msg);
            }

            int w, h, count;
            if(!int.TryParse(components[0], out w))
            {
                var msg = String.Format(
                    "Width is not valid integer: ({0})",
                    components[0]
                );
                throw new InvalidProgramException(msg);
            }

            if(!int.TryParse(components[1], out h))
            {
                var msg = String.Format(
                    "Height is not valid integer: ({0})",
                    components[1]
                );
                throw new InvalidProgramException(msg);
            }

            if(!int.TryParse(components[2], out count))
            {
                var msg = String.Format(
                    "Quantity of Textures is not valid integer: ({0})",
                    components[2]
                );
                throw new InvalidProgramException(msg);
            }

            while(count > 0)
            {
                m_texturesEntries.Add(new Size(){w=w, h=h});
                --count;
            }
        }

        private Color GetRandomColor()
        {
            return Color.FromArgb(
                255, 
                m_rnd.Next(255), 
                m_rnd.Next(255), 
                m_rnd.Next(255)
            );
        }    
    
    }// class RunOptions

}// namespace com.amazingcow.dummytextures

