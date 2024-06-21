﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Forms;
using ms = MaterialSkin; // alias to ms to avoid conflict with ColorScheme

namespace UltimateBlueScreenSimulator
{
    internal class GlobalSettings
    {
        /*
         * // Example usage
         * GlobalSettings gs = new GlobalSettings();
         * gs.EnableEggs = true; // set value
         * 
         * if (gs.EnableEggs) {  // get value
         *  MessageBox.Show("I stored a setting!");
         * }
         */

        // stored settings (these are saved)
        private bool postponeupdate = false;
        private bool hashverify = true;
        private bool autoupdate = true;
        private bool showcursor = false;
        private bool enableeggs = true;
        private bool randomness = false;
        private bool autodark = true;
        private string update_server = "http://markustegelane.eu/app/";

        // runtime settings (these are not persistent)
        private int errorcode = -1;
        private bool realpostpone = false;
        private bool nighttheme = false;
        private string supporttext = "If this is the first time you've seen this Stop error screen,\nrestart your computer. If this screen appears again, follow\nthese steps:\n\nCheck to make sure any new hardware or software is properly installed.\nIf this is a new installation, ask your hardware or software manufacturer\nfor any Windows updates you might need.\n\nIf problems continue, disable or remove any newly installed hardware\nor software. Disable BIOS memory options such as caching or shadowing.\nIf you need to use Safe mode to remove or disable components, restart\nyour computer, press F8 to select Advanced Startup Options, and then\nselect Safe Mode.";

        // prank mode
        private MessageBoxIcon MsgBoxIcon;
        private MessageBoxButtons MsgBoxType;
        private string MsgBoxTitle;
        private string MsgBoxMessage;
        private string appname;
        private bool lockout;
        private bool closecuzhidden;
        private bool showmsg;
        private string[] usb_device;
        private int[] time = { 0, 5, 0 };
        private bool timecatch = true;



        public List<string> log1 = new List<string>();


        ///<summary>
        ///Multi-monitor display method for fullscreen mode.
        ///</summary>
        public DisplayModes DisplayModeEnum = DisplayModes.None;

        ///<summary>
        ///Scaling mode for legacy OS templates, i.e. Windows 7 and earlier.
        ///</summary>
        public ScaleModes ScaleMode = ScaleModes.HighQualityBicubic;

        ///<summary>
        ///Accent color used by the application
        ///</summary>
        public ColorSchemes ColorScheme = ColorSchemes.Indigo;

        // define enums
        public enum DisplayModes
        {
            None,
            Blank,
            Mirror,
            Freeze
        }

        public enum ColorSchemes
        {
            Amber, Blue, Cyan, DeepOrange, DeepPurple,
            Green, Indigo, LightBlue, LightGreen, Lime,
            Orange, Pink, Purple, Red, Teal, Yellow
        }

        public enum ScaleModes
        {
            HighQualityBicubic,
            HighQualityBilinear,
            Bilinear,
            Bicubic,
            NearestNeighbour
        }

        ///<summary>
        ///Settings stored by the application, no initialization parameters required
        ///</summary>
        public GlobalSettings()
        {
            this.Log("Info", "Log started");
        }

        ///<summary>
        ///Installs updates only when the application is closed
        ///</summary>
        public bool PostponeUpdate {
            get { return postponeupdate; }
            set { postponeupdate = value; }
        }


        ///<summary>
        ///Automatically checks for updates
        ///</summary>
        public bool AutoUpdate {
            get { return autoupdate; }
            set { autoupdate = value; }
        }

        ///<summary>
        ///Checks the hash of the executable and any dependencies when downloading updates
        ///</summary>
        public bool HashVerify {
            get { return hashverify; }
            set { hashverify = value; }
        }

        ///<summary>
        ///Checks the hash of the executable and any dependencies when downloading updates
        ///</summary>
        public string SupportText {
            get { return supporttext; }
            set { supporttext= value; }
        }

        ///<summary>
        ///Shows the cursor during simulation
        ///</summary>
        public bool ShowCursor {
            get { return showcursor; }
            set { showcursor = value; }
        }

        ///<summary>
        ///Toggles easter eggs
        ///</summary>
        public bool EnableEggs {
            get { return enableeggs; }
            set { enableeggs = value; }
        }

        ///<summary>
        ///Toggles whether or not to add randomness to legacy configurations loaded from an old bs2cfg or bscfg file
        ///</summary>
        public bool Randomness {
            get { return randomness; }
            set { randomness = value; }
        }

        ///<summary>
        ///Selected update server, where the updates are downloaded from
        ///</summary>
        public string UpdateServer {
            get { return update_server; }
            set { update_server = value; }
        }

        ///<summary>
        ///Toggles Windows dark mode detection
        ///</summary>
        public bool AutoDark {
            get { return autodark; }
            set { autodark = value; }
        }


        ///<summary>
        ///Sets the error code for current session
        ///</summary>
        public int ErrorCode {
            get { return errorcode; }
            set { errorcode = value; }
        }

        ///<summary>
        ///Icon for the message box that may pop up after prank mode ends
        ///</summary>
        public MessageBoxIcon PM_MsgIcon {
            get { return MsgBoxIcon; }
            set { MsgBoxIcon = value; }
        }

        ///<summary>
        ///Button layout for the message box that may pop up after prank mode ends
        ///</summary>
        public MessageBoxButtons PM_MsgType {
            get { return MsgBoxType; }
            set { MsgBoxType = value; }
        }

        ///<summary>
        ///Title for the message box that may pop up after prank mode ends
        ///</summary>
        public string PM_MsgTitle {
            get { return MsgBoxTitle; }
            set { MsgBoxTitle = value; }
        }

        ///<summary>
        ///Text content for the message box that may pop up after prank mode ends
        ///</summary>
        public string PM_MsgText {
            get { return MsgBoxMessage; }
            set { MsgBoxMessage = value; }
        }

        ///<summary>
        ///Prevent ending the simulation by pressing Alt+F4 or any other key
        ///</summary>
        public bool PM_Lockout {
            get { return lockout; }
            set { lockout = value; }
        }

        ///<summary>
        ///Prevents re-showing the main interface when exiting prank mode
        ///</summary>
        public bool PM_CloseMainUI {
            get { return closecuzhidden; }
            set { closecuzhidden = value; }
        }

        ///<summary>
        ///Displays a message when exiting prank mode
        ///</summary>
        public bool PM_ShowMessage {
            get { return showmsg; }
            set { showmsg = value; }
        }


        ///<summary>
        ///Specifies which process triggers prank mode (if process trigger is selected)
        ///</summary>
        public string PM_AppName {
            get { return appname; }
            set { appname = value; }
        }

        ///<summary>
        ///Specifies which USB device triggers prank mode (if device trigger is selected)
        ///</summary>
        public string[] PM_UsbDevice {
            get { return usb_device; }
            set { usb_device = value; }
        }

        ///<summary>
        ///Specifies the timer, which triggers prank mode (if time based trigger is selected) in [hours, minutes, seconds] format
        ///</summary>
        public int[] PM_Time {
            get { return time; }
            set { time = value; }
        }

        ///<summary>
        ///Determines whether or not the trigger is time based. Device based trigger is only used if the length of PM_UsbDevice is greater than zero, otherwise application based trigger is used.
        ///</summary>
        public bool PM_Timecatch {
            get { return timecatch; }
            set { timecatch = value; }
        }

        ///<summary>
        ///Starts the update process after exiting the program
        ///</summary>
        public bool UpdateAfterExit {
            get { return realpostpone; }
            set { realpostpone = value; }
        }

        ///<summary>
        ///Determines if the interface should have a night theme or not
        ///</summary>
        public bool NightTheme {
            get { return nighttheme; }
            set { nighttheme = value; }
        }


        ///<summary>
        ///Saves application configuration settings
        ///</summary>
        public void SaveSettings()
        {
            this.Log("Info", "Saving settings");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"UpdateClose={PostponeUpdate}");
            sb.AppendLine($"HashVerify={HashVerify}");
            sb.AppendLine($"AutoUpdate={AutoUpdate}");
            sb.AppendLine($"ShowCursor={ShowCursor}");
            sb.AppendLine($"ScaleMode={GetScaleModeAsString()}");
            sb.AppendLine($"MultiMode={DisplayMode}");
            sb.AppendLine($"Seecrets={EnableEggs}");
            sb.AppendLine($"Server={UpdateServer}");
            sb.AppendLine($"Randomness={Randomness}");
            sb.AppendLine($"AutoDark={AutoDark}");
            sb.AppendLine($"ColorScheme={ColorScheme}");
            sb.Replace("\r\n", "\n");
            File.WriteAllText("settings.cfg", sb.ToString().Substring(0, sb.Length - 1));
        }

        ///<summary>
        ///Loads application configuration settings
        ///</summary>
        public void LoadSettings()
        {
            if (File.Exists("settings.cfg"))
            {
                this.Log("Info", $"Loading settings");
                string[] fc = File.ReadAllText("settings.cfg").Split('\n');
                foreach (string element in fc)
                {
                    //Other configurations
                    if (element.StartsWith("UpdateClose="))
                    {
                        PostponeUpdate = Convert.ToBoolean(element.Replace("UpdateClose=", ""));
                    }
                    else if (element.StartsWith("HashVerify="))
                    {
                        HashVerify = Convert.ToBoolean(element.Replace("HashVerify=", ""));
                    }
                    else if (element.StartsWith("AutoUpdate="))
                    {
                        AutoUpdate = Convert.ToBoolean(element.Replace("AutoUpdate=", ""));
                    }
                    else if (element.StartsWith("ShowCursor="))
                    {
                        ShowCursor = Convert.ToBoolean(element.Replace("ShowCursor=", ""));
                    }
                    else if (element.StartsWith("MultiMode="))
                    {
                        DisplayMode = element.Replace("MultiMode=", "");
                    }
                    else if (element.StartsWith("Seecrets="))
                    {
                        EnableEggs = Convert.ToBoolean(element.Replace("Seecrets=", ""));
                    }
                    else if (element.StartsWith("ScaleMode="))
                    {
                        SetScalingFromString(element.Replace("ScaleMode=", ""));
                    }
                    else if (element.StartsWith("Server="))
                    {
                        UpdateServer = element.Replace("Server=", "");
                    }
                    else if (element.StartsWith("Randomness="))
                    {
                        Randomness = Convert.ToBoolean(element.Replace("Randomness=", ""));
                    }
                    else if (element.StartsWith("AutoDark="))
                    {
                        AutoDark = Convert.ToBoolean(element.Replace("AutoDark=", ""));
                    }
                    else if (element.StartsWith("ColorScheme="))
                    {
                        Enum.TryParse(element.Replace("ColorScheme=", ""), out ColorScheme);
                    }
                    // this skips checking hidden/visible OS-s
                    // this is a feature that was exclusive to 1.x
                    else if (element.Contains("\""))
                    {
                        break;
                    }
                }
            } else
            {
                this.Log("Warning", "Settings file does not exist, using defaults");
            }
        }

        ///<summary>
        ///Returns current scale mode as a string (e.g. ScaleModes.Bilinear would return "Bilinear" etc.)
        ///</summary>
        private string GetScaleModeAsString()
        {
            this.Log("Info", $"Requested scale mode as string");
            switch (this.ScaleMode)
            {
                case ScaleModes.HighQualityBicubic:
                    return "HighQualityBicubic";
                case ScaleModes.HighQualityBilinear:
                    return "HighQualityBilinear";
                case ScaleModes.Bicubic:
                    return "Bicubic";
                case ScaleModes.Bilinear:
                    return "Bilinear";
                case ScaleModes.NearestNeighbour:
                    return "NearestNeighbour";
                default:
                    throw new NotImplementedException();
            }
        }

        ///<summary>
        ///Converts current interpolation mode to System.Drawing.Drawing2D.InterpolationMode. This should be used when setting scaling mode for System.Drawing.Graphics (to avoid a bunch of unnecessary conditional statements).
        ///</summary>
        public System.Drawing.Drawing2D.InterpolationMode GetInterpolationMode()
        {
            this.Log("Info", $"Requested interpolation mode");
            switch (ScaleMode)
            {
                case ScaleModes.HighQualityBicubic:
                    return System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                case ScaleModes.HighQualityBilinear:
                    return System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                case ScaleModes.Bilinear:
                    return System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                case ScaleModes.Bicubic:
                    return System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                case ScaleModes.NearestNeighbour:
                    return System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                default:
                    throw new NotImplementedException();
            }
        }

        ///<summary>
        ///Sets current scaling mode based on a string. Please note that passing an invalid value will throw NotImplementedException.
        ///</summary>
        private void SetScalingFromString(string algorithm)
        {
            this.Log("Info", $"Changed scaling mode to {algorithm}");
            switch (algorithm)
            {
                case "HighQualityBicubic":
                    ScaleMode = ScaleModes.HighQualityBicubic;
                    break;
                case "HighQualityBilinear":
                    ScaleMode = ScaleModes.HighQualityBilinear;
                    break;
                case "Bicubic":
                    ScaleMode = ScaleModes.Bicubic;
                    break;
                case "Bilinear":
                    ScaleMode = ScaleModes.Bilinear;
                    break;
                case "NearestNeighbour":
                    ScaleMode = ScaleModes.NearestNeighbour;
                    break;
            }
        }

        ///<summary>
        ///Multi-monitor display method (string input/output). The value must be one of the following: none, mirror, blank or freeze, in any other case NotImplementedException is thrown.
        ///</summary>
        public string DisplayMode {
            get {
                this.Log("Info", "Requested display mode");
                switch (DisplayModeEnum)
                {
                    case DisplayModes.None:
                        return "none";
                    case DisplayModes.Mirror:
                        return "mirror";
                    case DisplayModes.Blank:
                        return "blank";
                    case DisplayModes.Freeze:
                        return "freeze";
                    default:
                        throw new NotImplementedException();
                }
            }
            set {
                this.Log("Info", $"Changed display mode to {value}");
                switch (value)
                {
                    case "none":
                        DisplayModeEnum = DisplayModes.None;
                        break;
                    case "mirror":
                        DisplayModeEnum = DisplayModes.Mirror;
                        break;
                    case "blank":
                        DisplayModeEnum = DisplayModes.Blank;
                        break;
                    case "freeze":
                        DisplayModeEnum = DisplayModes.Freeze;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }


        ///<summary>
        ///Creates color scheme from accent color
        ///</summary>
        private ms.ColorScheme MakeScheme(ms.Accent accent)
        {
            return new ms.ColorScheme(ms.Primary.Blue800, ms.Primary.Blue900, ms.Primary.Blue400, accent, ms.TextShade.WHITE);
        }

        ///<summary>
        ///Applies accent color
        ///</summary>
        public void ApplyScheme()
        {
            this.Log("Info", $"Applying color scheme {ColorScheme}");
            switch (ColorScheme)
            {
                case ColorSchemes.Indigo:
                    Program.f1.materialSkinManager.ColorScheme = MakeScheme(ms.Accent.Indigo700);
                    break;
                case ColorSchemes.Lime:
                    Program.f1.materialSkinManager.ColorScheme = MakeScheme(ms.Accent.Lime700);
                    break;
                case ColorSchemes.Red:
                    Program.f1.materialSkinManager.ColorScheme = MakeScheme(ms.Accent.Red700);
                    break;
                case ColorSchemes.Pink:
                    Program.f1.materialSkinManager.ColorScheme = MakeScheme(ms.Accent.Pink700);
                    break;
                case ColorSchemes.Orange:
                    Program.f1.materialSkinManager.ColorScheme = MakeScheme(ms.Accent.Orange700);
                    break;
                case ColorSchemes.Amber:
                    Program.f1.materialSkinManager.ColorScheme = MakeScheme(ms.Accent.Amber700);
                    break;
                case ColorSchemes.Blue:
                    Program.f1.materialSkinManager.ColorScheme = MakeScheme(ms.Accent.Blue700);
                    break;
                case ColorSchemes.Cyan:
                    Program.f1.materialSkinManager.ColorScheme = MakeScheme(ms.Accent.Cyan700);
                    break;
                case ColorSchemes.DeepOrange:
                    Program.f1.materialSkinManager.ColorScheme = MakeScheme(ms.Accent.DeepOrange700);
                    break;
                case ColorSchemes.DeepPurple:
                    Program.f1.materialSkinManager.ColorScheme = MakeScheme(ms.Accent.DeepPurple700);
                    break;
                case ColorSchemes.Green:
                    Program.f1.materialSkinManager.ColorScheme = MakeScheme(ms.Accent.Green700);
                    break;
                case ColorSchemes.LightBlue:
                    Program.f1.materialSkinManager.ColorScheme = MakeScheme(ms.Accent.LightBlue700);
                    break;
                case ColorSchemes.LightGreen:
                    Program.f1.materialSkinManager.ColorScheme = MakeScheme(ms.Accent.LightGreen700);
                    break;
                case ColorSchemes.Purple:
                    Program.f1.materialSkinManager.ColorScheme = MakeScheme(ms.Accent.Purple700);
                    break;
                case ColorSchemes.Teal:
                    Program.f1.materialSkinManager.ColorScheme = MakeScheme(ms.Accent.Teal700);
                    break;
                case ColorSchemes.Yellow:
                    Program.f1.materialSkinManager.ColorScheme = MakeScheme(ms.Accent.Yellow700);
                    break;
            }
        }

        ///<summary>
        ///Logs an event
        ///</summary>
        ///<param name="e">Desired event type</param>
        ///<param name="message">Description of the event</param>
        public void Log(string e, string message, string source = "")
        {
            string msg = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + e + " - " + message;
            if (source != "")
            {
                msg += $" @{source}";
            }
            log1.Add(msg);
        }

        ///<summary>
        ///Gets all logged events
        ///</summary>
        ///<param name="reverse">Allows the log to be reversed, making the recent events show up first (default behaviour). If you don't want the log to be reverse, set this value to false.</param>
        ///<returns>A string containing all logged events</returns>
        public string GetLog(bool reverse = true)
        {
            if (reverse) { log1.Reverse(); }
            string ret = string.Join("\n", log1.ToArray());
            if (reverse) { log1.Reverse(); }
            return ret;
        }


    }
}
