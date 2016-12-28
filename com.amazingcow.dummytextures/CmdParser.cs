//----------------------------------------------------------------------------//
//               █      █                                                     //
//               ████████                                                     //
//             ██        ██                                                   //
//            ███  █  █  ███        CmdParser.cs                              //
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
using System.Collections.Generic;


namespace com.amazingcow.dummytextures
{
    public class CmdParser
    {    
        // Inner Types //
        private interface IFlagValueSetter
        {
            void SetValue(String value);    
        }
        private interface IFlagFoundSetter
        {
            void SetAsFound();    
        }

        public class Flag :
            IFlagValueSetter,
            IFlagFoundSetter
        {            
            // iVars //
            private String m_shortOption;
            private String m_longOption;
            private String m_value;
            private bool   m_wasFound;

            // CTOR //
            public Flag(String shortOption, String longOption = null)
            {
                m_shortOption = shortOption;
                m_longOption  = longOption;
                m_value       = null;
                m_wasFound    = false;

                if(String.IsNullOrEmpty(m_shortOption) && 
                   String.IsNullOrEmpty(m_longOption))
                {
                    var msg = String.Format(
                        "Both short and long options cannot be null or empty."
                    );
                    throw new ArgumentException(msg);
                }
            }

            // Public Methods //
            public bool Match(String str)
            {
                return (str == m_shortOption) || (str == m_longOption);
            }

            public String GetShortOption() { return m_shortOption; }
            public String GetLongOption () { return m_longOption;  }
            public String GetValue      () { return m_value;       }
            public bool   WasFound      () { return m_wasFound;    }

            // IFlagValueSetter Interface // 
            void IFlagValueSetter.SetValue(String value) { m_value = value; }

            // IFlagFoundSetter Interface //
            void IFlagFoundSetter.SetAsFound() { m_wasFound = true; }

        }// class Flag
            

        // Constants //
        private const char kFlagStartChar     = '-';
        private const int  kShortFlagLength   = 2;
        private const int  kLongFlagMinLength = 3;


        // iVars //
        private List<Flag> m_flags;


        // CTOR //
        public CmdParser()
        {
            m_flags = new List<Flag>();            
        }


        // Public Methods //
        public CmdParser AddFlag(Flag flag)
        {                        
            //Already contains this flag.
            //  Notice that Flag only cares about content 
            //  equality. So even two separated instances 
            //  contains the same options it'll be evaluated
            //  as equals.
            if(m_flags.Contains(flag))
            {
                var msg = String.Format("Already contains Flag:({0})", flag);
                throw new ArgumentException(msg);                    
            }

            m_flags.Add(flag);
            return this;
        }

        private bool IsFlagArg(String arg)
        {
            return IsShortFlagArg(arg) || IsLongFlagArg(arg);
        }

        private bool IsShortFlagArg(String arg)
        {            
            //To a flag be ok it must be in format of:
            //  -N : Where the '-' is the hyphen char
            //       followed for exactly ONE other char 
            //       besides the '-';

            //Empty.
            if(String.IsNullOrEmpty(arg)) 
                return false;

            //Different than the valid size.
            if(arg.Length != kShortFlagLength)
                return false;
            
            //Have a '-' followed by only one char that isn't '-'.
            return arg[0] == kFlagStartChar && 
                   arg[1] != kFlagStartChar;
        }

        private bool IsLongFlagArg(String arg)
        {
            //To a flag be ok it must be in format of:
            // --ABC : Where the two first chars must be
            //         hyphens (-) followed by any number
            //         of chars.

            //Empty.
            if(String.IsNullOrEmpty(arg)) 
                return false;

            //Smaller than the minimum valid size.
            if(arg.Length < kLongFlagMinLength)
                return false;

            //The two first chars are '-' followed 
            //by any char besides the '-' itself.
            return arg[0] == kFlagStartChar && 
                   arg[1] == kFlagStartChar &&
                   arg[2] != kFlagStartChar;
        }

        private void TrySplitArgs(
            ref List<String> argsList, 
            int              flagIndex, 
            bool             shortFlag)
        {
            var flagStr = argsList[flagIndex];

            String flagClean;
            String valueClean;

            int flagSubStringStartIndex = -1;
            int flagSubStringLength     = -1;

            int valueSubStringStartIndex = -1;
            int valueSubStringLength     = -1;

            if(shortFlag)
            {
                flagSubStringStartIndex = 1;
                flagSubStringLength     = kShortFlagLength -1;

                valueSubStringStartIndex = 2;
                valueSubStringLength     = flagStr.Length - valueSubStringStartIndex;
            }
            else //!shortFlag
            {
                var indexOfEqual = flagStr.IndexOf('=');

                //Found an '=' char in flag.
                if(indexOfEqual != -1)
                {                   
                    flagSubStringStartIndex = 2;
                    flagSubStringLength     = indexOfEqual - 2;

                    valueSubStringStartIndex = indexOfEqual+1;
                    valueSubStringLength     = flagStr.Length - valueSubStringStartIndex;
                }
                else
                {
                    flagSubStringStartIndex = 2;
                    flagSubStringLength     = flagStr.Length - 2;

                    valueSubStringStartIndex = 0;
                    valueSubStringLength     = 0;
                }
            }
                
            //Clean the strings (Remove the '-' and the spaces).
            flagClean = flagStr.Substring(
                flagSubStringStartIndex, 
                flagSubStringLength
            ).Trim();

            valueClean = flagStr.Substring(
                valueSubStringStartIndex, 
                valueSubStringLength
            ).Trim();

            if(flagClean != flagStr && valueClean != flagStr)
            {
                argsList[flagIndex] = flagClean;
                if(!String.IsNullOrEmpty(valueClean))
                    argsList.Insert(flagIndex + 1, valueClean);
            }
        }

        private void ProcessFlag(
            List<String> argsList, 
            int          flagIndex)
        {
            var flag  = argsList[flagIndex];    
            var value = (flagIndex+1 < argsList.Count) 
                        ? argsList[flagIndex+1] 
                        : "";

            foreach(var flagObj in m_flags)
            {
                if(!flagObj.Match(flag))
                    continue;

                (flagObj as IFlagFoundSetter).SetAsFound();
                (flagObj as IFlagValueSetter).SetValue(value);

                return;
            }

            var msg = String.Format("Invalid flag:({0})", flag);
            throw new InvalidProgramException(msg);
        }

        public List<String> Parse(String[] args)
        {
            var argsList     = new List<String>(args);
            var nonFlagsList = new List<String>(args.Length);

            for(int i = 0; i < argsList.Count; ++i)
            {
                var arg = argsList[i];

                //Short Flag.
                if(IsShortFlagArg(arg))
                {
                    TrySplitArgs(ref argsList, i, true);
                    ProcessFlag (argsList, i);

                    ++i;
                }
                //Long Flag.
                else if(IsLongFlagArg(arg))
                {
                    TrySplitArgs(ref argsList, i, false);
                    ProcessFlag (argsList, i);

                    ++i;
                }
                //Mon Flag Arg.
                else
                {
                    nonFlagsList.Add(arg);
                }
            }                


            return nonFlagsList;
        }       
    
    }//class CmdParser

}//namespace com.amazingcow.dummytextures

