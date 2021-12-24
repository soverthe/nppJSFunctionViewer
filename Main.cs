using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;

namespace Kbg.NppPluginNET
{

    class Main
    {
        internal const string PluginName = "JSFunctionViewer";
        static string iniFilePath = null;
        //static bool someSetting = false;
        static frmMyDlg frmFuncView = null;
        static int idFuncView = -1;
        public static bool isFuncEnabled = false;
        static Bitmap tbBmp = new Bitmap(JSFunctionViewer.Properties.Resources.JSFunctionViewer);
        static Bitmap tbBmp_tbTab = new Bitmap(JSFunctionViewer.Properties.Resources.JSFunctionViewer_bmp);
        static Icon tbIcon = null;
        static bool isShuttingDown = false;
        static string externalFilePath = "";
        static bool isShown = false;

        
        public static void OnNotification(ScNotification notification)
        {
            if (notification.Header.Code == (uint)NppMsg.NPPN_BEFORESHUTDOWN || notification.Header.Code == (uint)NppMsg.NPPN_SHUTDOWN)
            {
                isShuttingDown = true;
            }

            if (!isShuttingDown && notification.Header.Code != (uint)NppMsg.NPPN_FILEBEFORELOAD)
            {
                RefreshFunction();
            }
        }

        internal static void CommandMenuInit()
        {
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            iniFilePath = sbIniFilePath.ToString();
            if (!Directory.Exists(iniFilePath)) Directory.CreateDirectory(iniFilePath);
            iniFilePath = Path.Combine(iniFilePath, PluginName + ".ini");
            //someSetting = (Win32.GetPrivateProfileInt("SomeSection", "SomeKey", 0, iniFilePath) != 0);

            PluginBase.SetCommand(0, "Toggle Function Viewer", FunctionViewer/*, new ShortcutKey(false, true, false, Keys.B)*/);

            idFuncView = 0;
        }

        internal static Bitmap GetCurrenctIcon() //Please tell me if there's a way to re-set an icon, 'till then, this function'll be sad
        {
            if (isFuncEnabled)
            {
                return new Bitmap(JSFunctionViewer.Properties.Resources.JSFunctionViewer);
            }
            else
            {
                return new Bitmap(JSFunctionViewer.Properties.Resources.JSFunctionViewer_Gray);
            }
        }

        internal static void SetToolBarIcon()
        {
            //tbBmp = GetCurrenctIcon();   :(
            toolbarIcons tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = tbBmp.GetHbitmap();
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_ADDTOOLBARICON, PluginBase._funcItems.Items[idFuncView]._cmdID, pTbIcons); //todo: dark mode
            Marshal.FreeHGlobal(pTbIcons);
        }

        internal static void PluginCleanUp()
        {
            //Win32.WritePrivateProfileString("SomeSection", "SomeKey", someSetting ? "1" : "0", iniFilePath);
        }


        internal static void FunctionViewer()
        {
            if (frmFuncView == null)
            {
                frmFuncView = new frmMyDlg();

                using (Bitmap newBmp = new Bitmap(16, 16))
                {
                    Graphics g = Graphics.FromImage(newBmp);
                    ColorMap[] colorMap = new ColorMap[1];

                    colorMap[0] = new ColorMap();
                    colorMap[0].OldColor = Color.Fuchsia;
                    colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);

                    ImageAttributes attr = new ImageAttributes();
                    attr.SetRemapTable(colorMap);

                    g.DrawImage(tbBmp_tbTab, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);

                    tbIcon = Icon.FromHandle(newBmp.GetHicon());
                }

                NppTbData _nppTbData = new NppTbData();

                _nppTbData.hClient = frmFuncView.Handle;
                _nppTbData.pszName = "Function Viewer";
                _nppTbData.dlgID = idFuncView;

                _nppTbData.uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
                _nppTbData.hIconTab = (uint)tbIcon.Handle;
                _nppTbData.pszModuleName = PluginName;

                IntPtr _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
                Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);

                Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);

                isFuncEnabled = true;
            }
            else
            {
                isFuncEnabled = !isFuncEnabled;
            }
        }
        
        public static void ShowDialog()
        {
            if (!isShown)
            {
                isShown = true;
                Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_DMMSHOW, 0, frmFuncView.Handle);
            }
        }
        public static void HideDialog()
        {
            if (!frmFuncView.CheckStayCheckBox() || !isFuncEnabled) 
            {
                isShown = false;

                Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_DMMHIDE, 0, frmFuncView.Handle);
            }
        }


        internal static string ReadFile(string path)
        {
            string result;
            using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }


        internal static string GetSelectedText()
        {
            int length = (int)Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_GETSELTEXT, 0, 0);
            IntPtr ptrToText = Marshal.AllocHGlobal(length + 1);

            Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_GETSELTEXT, length, ptrToText);

            return Marshal.PtrToStringAnsi(ptrToText);
        }

        internal static string GetCurrentFilePath()
        {
            StringBuilder filePath = new StringBuilder(Win32.MAX_PATH);

            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETFULLCURRENTPATH, Win32.MAX_PATH, filePath);

            return filePath.ToString();
        }

        internal static string TrimFunction(string text)
        {
            int layers = 0;

            for (int i = 9; i < text.Length; i++)
            {
                if (text[i] == '{')
                {
                    layers++;
                }
                else if (text[i] == '}')
                {
                    layers--;
                    if (layers == 0)
                    {
                        text = text.Substring(0, i + 1);
                        
                        break;
                    }
                }
            }

            return text;
        }


        internal static List<char> GetIndents(string[] lines){
            List<char> indents = new List<char>();

            for (int i = 0; i < lines[lines.Length - 1].Length; i++)
            {
                char indent = lines[lines.Length - 1][i];

                if (indent == ' ' || indent == '\t')
                {
                    indents.Add(indent);
                }
                else
                {
                    break;
                }
            }
            return indents;
        }

        internal static string RemoveExtraIndents(string[] lines, List<char> indents)
        {
            for (int i = 1; i < lines.Length; i++)
            {
                int indentsNum = 0;

                for (int j = 0; j < indents.Count; j++)
                {
                    if (lines[i].Length > j) {
                        if (lines[i][j] == indents[j])
                        {
                            indentsNum++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                lines[i] = lines[i].Substring(indentsNum);
            }

            return String.Join(Environment.NewLine, lines);
        }

        internal static string FixFunction(string text)
        {
            string[] lines = text.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            );

            List<char> indents = GetIndents(lines);
            
            return RemoveExtraIndents(lines, indents);
        }

        internal static List<string> GetExternalFiles(string currentFilePath)
        {
            List<string> externalFiles = new List<string>();

            string currentFile = ReadFile(currentFilePath);

            string[] sources = currentFile.Split(
                new string[] { "src=\"" },
                StringSplitOptions.None
            );

            

            for (int i = 1; i < sources.Length; i++)
            {
                int index = sources[i].IndexOf("\"");

                if (sources[i].Substring(index + 1, 10) == "></script>")
                {
                    externalFiles.Add(sources[i].Substring(0, index));
                }
            }

            return externalFiles;
        }

        internal static void AddExternalFiles(List<string> list)
        {
            int lastSlash = list[0].LastIndexOf('\\');
            string folder = (lastSlash > -1) ? list[0].Substring(0, lastSlash + 1) : "";

            List<string> externalFiles = GetExternalFiles(list[0]);

            for (int i = 0; i < externalFiles.Count; i++)
            {
                externalFiles[i] = Regex.Replace(externalFiles[i], @"[\/]", "\\");

                if (externalFiles[i][1] == ':')
                {
                    list.Add(externalFiles[i]);
                }
                else
                {
                    list.Add(folder + externalFiles[i]);
                }
            }
        }

        internal static void CheckGoToButtons()
        {
            isShown = false;
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_DMMHIDE, 0, frmFuncView.Handle);

            if (frmFuncView.CheckGoToFunction())
            {
                string selectedText = GetSelectedText();

                string functionName = "function " + selectedText + "(";

                Clipboard.SetText(functionName);

                if (externalFilePath != "")
                {
                    Process.Start("notepad++.exe", "\"" + externalFilePath + "\"");
                }

                //HideDialog();
                
                SendKeys.SendWait("%(S{ENTER})^(V){ENTER}{ESCAPE}");
            }
            else if (frmFuncView.CheckGoUp())
            {
                //HideDialog();
                
                SendKeys.SendWait("%(SSS{ENTER})");
            }
            else if (frmFuncView.CheckGoDown())
            {
                //HideDialog();
                
                SendKeys.SendWait("%(SS{ENTER})");
            }
        }

        internal static void RefreshFunction()
        {
            if (isFuncEnabled)
            {
                string selectedText = GetSelectedText();

                string functionName = "function " + selectedText + "(";

                List<string> filePaths = new List<string>();

                filePaths.Add(GetCurrentFilePath());

                AddExternalFiles(filePaths);

                bool isActive = false;

                for (int i = 0; i < filePaths.Count; i++)
                {
                    bool isReal = true;
                    
                    try
                    {
                        ReadFile(filePaths[i]);
                    }
                    catch
                    {
                        isReal = false;
                    }

                    if (isReal)
                    {
                        string fileContents = ReadFile(filePaths[i]);
                        if (fileContents.Contains(functionName))
                        {
                            string functionText = fileContents.Substring(fileContents.IndexOf(functionName));

                            functionText = TrimFunction(functionText);
                            functionText = FixFunction(functionText);

                            if (i > 0)
                            {
                                frmFuncView.HideGoUpDownButtons();
                                externalFilePath = filePaths[i];
                                functionText = Environment.NewLine + "                                                        (From: " + filePaths[i] + ")" + Environment.NewLine + Environment.NewLine + functionText;
                            }
                            else
                            {
                                frmFuncView.ShowGoUpDownButtons();
                                externalFilePath = "";
                                functionText = Environment.NewLine + Environment.NewLine + Environment.NewLine + functionText;
                            }

                            frmFuncView.ChangeText(functionText);

                            ShowDialog();

                            isActive = true;

                            break;
                        }
                    }
                }
                
                if (!isActive)
                {
                    HideDialog();
                }
            }
        }
    }
}