using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpTools.Common
{
    public class IniFileHelp
    {
        public const int MaxSectionSize = 32767 * 100; // 32 KB 


        private string m_path;

        #region P/Invoke declares
        [System.Security.SuppressUnmanagedCodeSecurity]
        private static class NativeMethods
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern int GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer,
                                                                   uint nSize,
                                                                   string lpFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern uint GetPrivateProfileString(string lpAppName,
                                                              string lpKeyName,
                                                              string lpDefault,
                                                              StringBuilder lpReturnedString,
                                                              int nSize,
                                                              string lpFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern uint GetPrivateProfileString(string lpAppName,
                                                              string lpKeyName,
                                                              string lpDefault,
                                                              [In, Out] char[] lpReturnedString,
                                                              int nSize,
                                                              string lpFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern int GetPrivateProfileString(string lpAppName,
                                                             string lpKeyName,
                                                             string lpDefault,
                                                             IntPtr lpReturnedString,
                                                             uint nSize,
                                                             string lpFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern int GetPrivateProfileInt(string lpAppName,
                                                          string lpKeyName,
                                                          int lpDefault,
                                                          string lpFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern int GetPrivateProfileSection(string lpAppName,
                                                              IntPtr lpReturnedString,
                                                              uint nSize,
                                                              string lpFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool WritePrivateProfileString(string lpAppName,
                                                                string lpKeyName,
                                                                string lpString,
                                                                string lpFileName);


        }
        #endregion


        public IniFileHelp()
        {
            try
            {
                string fileName = AppDomain.CurrentDomain.FriendlyName;
                fileName = fileName.Substring(0, fileName.IndexOf("."));
                try
                {
                    string appPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create);
                    appPath = System.IO.Path.Combine(appPath, fileName);
                    if (!Directory.Exists(appPath))
                    {
                        Directory.CreateDirectory(appPath);
                    }
                    m_path = System.IO.Path.Combine(appPath, fileName + ".ini");
                    if (!File.Exists(m_path))
                    {
                        File.AppendAllText(m_path, string.Empty, Encoding.UTF8);
                    }
                }
                catch
                {
                    m_path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), fileName + ".ini");
                    if (!File.Exists(m_path))
                    {
                        File.AppendAllText(m_path, string.Empty, Encoding.UTF8);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IniFileHelp(string path)
        {
            try
            {
                m_path = System.IO.Path.GetFullPath(path);
                if (!File.Exists(m_path))
                {
                    File.AppendAllText(m_path, string.Empty, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public string Path
        {
            get
            {
                return m_path;
            }
        }

        #region Get Value Methods


        public string GetString(string sectionName,
                                string keyName,
                                string defaultValue)
        {
            if (sectionName == null)
                throw new ArgumentNullException("sectionName");

            if (keyName == null)
                throw new ArgumentNullException("keyName");

            StringBuilder retval = new StringBuilder(IniFileHelp.MaxSectionSize);

            NativeMethods.GetPrivateProfileString(sectionName,
                                                  keyName,
                                                  defaultValue,
                                                  retval,
                                                  IniFileHelp.MaxSectionSize,
                                                  m_path);

            return retval.ToString();
        }


        public int GetInt16(string sectionName,
                            string keyName,
                            short defaultValue)
        {
            int retval = GetInt32(sectionName, keyName, defaultValue);
            return Convert.ToInt16(retval);
        }


        public int GetInt32(string sectionName,
                            string keyName,
                            int defaultValue)
        {
            if (sectionName == null)
                throw new ArgumentNullException("sectionName");
            if (keyName == null)
                throw new ArgumentNullException("keyName");
            return NativeMethods.GetPrivateProfileInt(sectionName, keyName, defaultValue, m_path);
        }

        public double GetDouble(string sectionName,
                                string keyName,
                                double defaultValue)
        {
            string retval = GetString(sectionName, keyName, "");

            if (retval == null || retval.Length == 0)
            {
                return defaultValue;
            }
            return Convert.ToDouble(retval, CultureInfo.InvariantCulture);
        }

        #endregion

        #region GetSectionValues Methods


        public List<KeyValuePair<string, string>> GetSectionValuesAsList(string sectionName)
        {
            List<KeyValuePair<string, string>> retval;
            string[] keyValuePairs;
            string key, value;
            int equalSignPos;

            if (sectionName == null)
                throw new ArgumentNullException("sectionName");
            IntPtr ptr = Marshal.AllocCoTaskMem(IniFileHelp.MaxSectionSize);

            try
            {
                int len = NativeMethods.GetPrivateProfileSection(sectionName,
                                                                 ptr,
                                                                 IniFileHelp.MaxSectionSize,
                                                                 m_path);

                keyValuePairs = ConvertNullSeperatedStringToStringArray(ptr, len);
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptr);
            }


            retval = new List<KeyValuePair<string, string>>(keyValuePairs.Length);

            for (int i = 0; i < keyValuePairs.Length; ++i)
            {
                if (!keyValuePairs[i].Trim().StartsWith("#"))
                {
                    equalSignPos = keyValuePairs[i].IndexOf('=');
                    key = keyValuePairs[i].Substring(0, equalSignPos);

                    value = keyValuePairs[i].Substring(equalSignPos + 1,
                                                       keyValuePairs[i].Length - equalSignPos - 1);

                    retval.Add(new KeyValuePair<string, string>(key, value));
                }
            }

            return retval;
        }


        public Dictionary<string, string> GetSectionValues(string sectionName)
        {
            List<KeyValuePair<string, string>> keyValuePairs;
            Dictionary<string, string> retval;
            keyValuePairs = GetSectionValuesAsList(sectionName);
            retval = new Dictionary<string, string>(keyValuePairs.Count);
            foreach (KeyValuePair<string, string> keyValuePair in keyValuePairs)
            {
                if (!retval.ContainsKey(keyValuePair.Key))
                {
                    retval.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            return retval;
        }

        #endregion

        #region Get Key/Section Names


        public string[] GetKeyNames(string sectionName)
        {
            int len;
            string[] retval;

            if (sectionName == null)
                throw new ArgumentNullException("sectionName");

            //Allocate a buffer for the returned section names. 

            IntPtr ptr = Marshal.AllocCoTaskMem(IniFileHelp.MaxSectionSize);



            try
            {
                //Get the section names into the buffer. 

                len = NativeMethods.GetPrivateProfileString(sectionName,

                                                            null,
                                                            null,
                                                            ptr,
                                                            IniFileHelp.MaxSectionSize,
                                                            m_path);

                retval = ConvertNullSeperatedStringToStringArray(ptr, len);
            }
            finally
            {
                //Free the buffer 

                Marshal.FreeCoTaskMem(ptr);

            }

            return retval;
        }


        public string[] GetSectionNames()
        {
            string[] retval;
            int len;

            //Allocate a buffer for the returned section names. 

            IntPtr ptr = Marshal.AllocCoTaskMem(IniFileHelp.MaxSectionSize);



            try
            {
                //Get the section names into the buffer. 

                len = NativeMethods.GetPrivateProfileSectionNames(ptr,

                    IniFileHelp.MaxSectionSize, m_path);

                retval = ConvertNullSeperatedStringToStringArray(ptr, len);
            }
            finally
            {
                //Free the buffer 

                Marshal.FreeCoTaskMem(ptr);

            }

            return retval;
        }


        private static string[] ConvertNullSeperatedStringToStringArray(IntPtr ptr, int valLength)
        {
            string[] retval;

            if (valLength == 0)
            {
                //Return an empty array. 

                retval = new string[0];

            }
            else
            {
                //Convert the buffer into a string.  Decrease the length 

                //by 1 so that we remove the second null off the end. 

                string buff = Marshal.PtrToStringAuto(ptr, valLength - 1);



                //Parse the buffer into an array of strings by searching for nulls. 

                retval = buff.Split('\0');

            }

            return retval;
        }

        #endregion

        #region Write Methods


        private void WriteValueInternal(string sectionName, string keyName, string value)
        {
            if (!NativeMethods.WritePrivateProfileString(sectionName, keyName, value, m_path))
            {
                throw new System.ComponentModel.Win32Exception();
            }
        }

        public void WriteValue(string sectionName, string keyName, string value)
        {
            if (sectionName == null)
                throw new ArgumentNullException("sectionName");

            if (keyName == null)
                throw new ArgumentNullException("keyName");

            if (value == null)
                throw new ArgumentNullException("value");

            WriteValueInternal(sectionName, keyName, value);
        }


        public void WriteValue(string sectionName, string keyName, short value)
        {
            WriteValue(sectionName, keyName, (int)value);
        }


        public void WriteValue(string sectionName, string keyName, int value)
        {
            WriteValue(sectionName, keyName, value.ToString(CultureInfo.InvariantCulture));
        }


        public void WriteValue(string sectionName, string keyName, float value)
        {
            WriteValue(sectionName, keyName, value.ToString(CultureInfo.InvariantCulture));
        }


        public void WriteValue(string sectionName, string keyName, double value)
        {
            WriteValue(sectionName, keyName, value.ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region Delete Methods


        public void DeleteKey(string sectionName, string keyName)
        {
            if (sectionName == null)
                throw new ArgumentNullException("sectionName");

            if (keyName == null)
                throw new ArgumentNullException("keyName");

            WriteValueInternal(sectionName, keyName, null);
        }


        public void DeleteSection(string sectionName)
        {
            if (sectionName == null)
                throw new ArgumentNullException("sectionName");

            WriteValueInternal(sectionName, null, null);
        }

        #endregion
    }
}
