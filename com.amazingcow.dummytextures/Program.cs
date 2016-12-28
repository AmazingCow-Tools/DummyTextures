//----------------------------------------------------------------------------//
//               █      █                                                     //
//               ████████                                                     //
//             ██        ██                                                   //
//            ███  █  █  ███        Program                                   //
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
using System.IO;

namespace com.amazingcow.dummytextures
{
    class MainClass
    {
        // Constants //
        private const String kAppVersion = "0.0.1";


        // Entry Point //
        public static void Main(String[] args)
        {
            try {
                Run(args);    
            } 
            catch(Exception ex) {
                var msg = String.Format("[Fatal] {0}", ex.Message);
                Console.WriteLine(msg);
            }
        }            
            
        // Run //
        private static void Run(String[] args)
        {               
            if(args.Length == 0)
                ShowHelp();
            
            //Setup the CMDParser and Flags.
            var infileFlag  = new CmdParser.Flag("i", "infile" );
            var outpathFlag = new CmdParser.Flag("o", "outpath");
            var helpFlag    = new CmdParser.Flag("h", "help"   );
            var versionFlag = new CmdParser.Flag("v", "version");
            var verboseFlag = new CmdParser.Flag("V", "verbose");

            var cmdParser = new CmdParser()
                .AddFlag(infileFlag )
                .AddFlag(outpathFlag)
                .AddFlag(helpFlag   )
                .AddFlag(versionFlag)
                .AddFlag(verboseFlag);

            var texturesConfigArgs = cmdParser.Parse(args);

            //Exclusive Flags - Run and Exit... 
            if(helpFlag.WasFound   ()) ShowHelp   ();
            if(versionFlag.WasFound()) ShowVersion(); 


            //Setup the RunOptions.
            var runOptions = new RunOptions();

            //Verbose.
            if(verboseFlag.WasFound()) 
                runOptions.SetVerbose();

            //Output path.
            if(outpathFlag.WasFound())
                runOptions.SetOutputPath(outpathFlag.GetValue());

            //Add the Textures Config from file (if any).
            if(infileFlag.WasFound())
            {
                var infileFullpath = PathHelper.Canonize(infileFlag.GetValue());
                var lines          = File.ReadLines(infileFullpath);

                texturesConfigArgs.AddRange(lines);
            }

            //Add the Textures Config from command line (if any).
            foreach(var arg in texturesConfigArgs)
                runOptions.AddTextureEntry(arg);


            runOptions.CreateTextures();
        }


        // Show Help / Version //
        private static void ShowHelp()
        {
            var helpMsg = @"Usage:
    dummy-textures [-hv] [-V] [-i<path>] [-o<path>] <textures-configs>

Options:
    *-h --help     : Show this screen.
    *-v --version  : Show app version and copyright.

    -V --verbose : Verbose mode, helps to see what it's doing.
    
    -i --infile  <path> : The textures configuration file path.
    -o --outpath <path> : The path of folder that the Textures will be saved.

Notes:
    Texture config is in format of: 
        [width]x[height]x[count of textures] 
    Example:
        128x128x25 (Will create 25 textures of 128x128 pixels).   
    All components separated by the 'x' char with no withespaces.

    If --outpath <path> is blank a default output directory will be 
    created in the current directory.
    
    If --outpath <path> doesn't exists it'll be created automatically.

    Options marked with * are exclusive, i.e. the dummy-textures will run that
    and exit after the operation.
";
            Console.WriteLine(helpMsg);
            Environment.Exit(0);
        }

        private static void ShowVersion()
        {
            var versionMsg = @"dummy-textures - {0} - N2OMatt <n2omatt@amazingcow.com>
Copyright (c) 2016 - Amazing Cow
This is a free software (GPLv3) - Share/Hack it
Check opensource.amazingcow.com for more :)";
                
            Console.WriteLine(String.Format(versionMsg, kAppVersion));
            Environment.Exit(0);
        }

    }//class MainClass

} // namespace com.amazingcow.dummytextures

