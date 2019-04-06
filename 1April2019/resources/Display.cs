using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace _1April2019.Resource
{
    /**
     * <summary>
     * Class for Windows display options.
     * </summary>
     **/
    public class Display
    {
        #region Orientations
        /**
         * <summary>
         * List of available display orientations.
         * </summary>
         **/
        public enum Orientations
        {
            DEGREES_CW_0 = 0,
            DEGREES_CW_90 = 3,
            DEGREES_CW_180 = 2,
            DEGREES_CW_270 = 1
        }
        #endregion

        #region Rotate function
        /**
         * <summary>
         * Rotate the selected display.
         * </summary>
         **/
        public static bool Rotate(uint DisplayNumber, Orientations Orientation)
        {
            if (DisplayNumber == 0)
            {
                throw new ArgumentOutOfRangeException("DisplayNumber", DisplayNumber, "First display is 1.");
            }

            bool result = false;
            DISPLAY_DEVICE d = new DISPLAY_DEVICE();
            DEVMODE dm = new DEVMODE();
            d.cb = Marshal.SizeOf(d);

            if (!NativeMethods.EnumDisplayDevices(null, DisplayNumber - 1, ref d, 0))
            {
                throw new ArgumentOutOfRangeException("DisplayNumber", DisplayNumber, "Number is greater than connected displays.");
            }

            if (0 != NativeMethods.EnumDisplaySettings(d.DeviceName, NativeMethods.ENUM_CURRENT_SETTINGS, ref dm))
            {
                if ((dm.dmDisplayOrientation + (int) Orientation) % 2 == 1) // Need to swap height and width?
                {
                    int temp = dm.dmPelsHeight;
                    dm.dmPelsHeight = dm.dmPelsWidth;
                    dm.dmPelsWidth = temp;
                }

                switch (Orientation)
                {
                    case Orientations.DEGREES_CW_90:
                        dm.dmDisplayOrientation = NativeMethods.DMDO_270;
                        break;
                    case Orientations.DEGREES_CW_180:
                        dm.dmDisplayOrientation = NativeMethods.DMDO_180;
                        break;
                    case Orientations.DEGREES_CW_270:
                        dm.dmDisplayOrientation = NativeMethods.DMDO_90;
                        break;
                    case Orientations.DEGREES_CW_0:
                        dm.dmDisplayOrientation = NativeMethods.DMDO_DEFAULT;
                        break;
                    default:
                        break;
                }

                DISP_CHANGE ret = NativeMethods.ChangeDisplaySettingsEx(d.DeviceName, ref dm, IntPtr.Zero, DisplaySettingsFlags.CDS_UPDATEREGISTRY, IntPtr.Zero);

                result = ret == 0;
            }

            return result;
        }
        #endregion

        #region Reset rotations function
        /**
         * <summary>
         * Resets all rotations to their defaults.
         * </summary>
         **/
        public static void ResetAllRotations()
        {
            try
            {
                uint i = 0;
                while (++i <= 64)
                {
                    Rotate(i, Orientations.DEGREES_CW_0);
                }
            }
            #pragma warning disable CS0168
            #pragma warning disable IDE0059
            catch (ArgumentOutOfRangeException ex)
            #pragma warning restore IDE0059
            #pragma warning restore CS0168
            {
                // Reached the final display, no error here.
                return;
            }
        }
        #endregion
    }

    #region Native methods
    /**
     * <summary>
     * References to the native Windows methods.
     * </summary>
     **/
    internal class NativeMethods
    {
        [DllImport("user32.dll")]
        internal static extern DISP_CHANGE ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd, DisplaySettingsFlags dwflags, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        internal static extern int EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        public const int DMDO_DEFAULT = 0;
        public const int DMDO_90 = 1;
        public const int DMDO_180 = 2;
        public const int DMDO_270 = 3;

        public const int ENUM_CURRENT_SETTINGS = -1;
    }
    #endregion

    #region Device mode
    /**
     * <summary>
     * https://msdn.microsoft.com/en-us/library/windows/desktop/dd183565(v=vs.85).aspx
     * </summary>
     **/
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
    internal struct DEVMODE
    {
        public const int CCHDEVICENAME = 32;
        public const int CCHFORMNAME = 32;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
        [FieldOffset(0)]
        public string dmDeviceName;
        [FieldOffset(32)]
        public Int16 dmSpecVersion;
        [FieldOffset(34)]
        public Int16 dmDriverVersion;
        [FieldOffset(36)]
        public Int16 dmSize;
        [FieldOffset(38)]
        public Int16 dmDriverExtra;
        [FieldOffset(40)]
        public DM dmFields;

        [FieldOffset(44)]
        readonly Int16 dmOrientation;
        [FieldOffset(46)]
        readonly Int16 dmPaperSize;
        [FieldOffset(48)]
        readonly Int16 dmPaperLength;
        [FieldOffset(50)]
        readonly Int16 dmPaperWidth;
        [FieldOffset(52)]
        readonly Int16 dmScale;
        [FieldOffset(54)]
        readonly Int16 dmCopies;
        [FieldOffset(56)]
        readonly Int16 dmDefaultSource;
        [FieldOffset(58)]
        readonly Int16 dmPrintQuality;

        [FieldOffset(44)]
        public POINTL dmPosition;
        [FieldOffset(52)]
        public Int32 dmDisplayOrientation;
        [FieldOffset(56)]
        public Int32 dmDisplayFixedOutput;

        [FieldOffset(60)]
        public short dmColor;
        [FieldOffset(62)]
        public short dmDuplex;
        [FieldOffset(64)]
        public short dmYResolution;
        [FieldOffset(66)]
        public short dmTTOption;
        [FieldOffset(68)]
        public short dmCollate;
        [FieldOffset(72)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
        public string dmFormName;
        [FieldOffset(102)]
        public Int16 dmLogPixels;
        [FieldOffset(104)]
        public Int32 dmBitsPerPel;
        [FieldOffset(108)]
        public Int32 dmPelsWidth;
        [FieldOffset(112)]
        public Int32 dmPelsHeight;
        [FieldOffset(116)]
        public Int32 dmDisplayFlags;
        [FieldOffset(116)]
        public Int32 dmNup;
        [FieldOffset(120)]
        public Int32 dmDisplayFrequency;
    }
    #endregion

    #region Display device
    /**
     * <summary>
     * https://msdn.microsoft.com/en-us/library/windows/desktop/dd183569(v=vs.85).aspx
     * </summary>
     **/
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct DISPLAY_DEVICE
    {
        [MarshalAs(UnmanagedType.U4)]
        public int cb;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DeviceName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceString;
        [MarshalAs(UnmanagedType.U4)]
        public DisplayDeviceStateFlags StateFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceKey;
    }
    #endregion

    #region Coordinates
    /**
     * <summary>
     * https://msdn.microsoft.com/en-us/library/windows/desktop/dd162807(v=vs.85).aspx
     * </summary>
     **/
    [StructLayout(LayoutKind.Sequential)]
    internal struct POINTL
    {
        readonly long x;
        readonly long y;
    }
    #endregion

    #region Event status codes
    /**
     * <summary>
     * Display change status codes.
     * </summary>
     **/
    internal enum DISP_CHANGE : int
    {
        Successful = 0,
        Restart = 1,
        Failed = -1,
        BadMode = -2,
        NotUpdated = -3,
        BadFlags = -4,
        BadParam = -5,
        BadDualView = -6
    }
    #endregion

    #region Device flag codes
    /**
     * <summary>
     * http://www.pinvoke.net/default.aspx/Enums/DisplayDeviceStateFlags.html
     * </summary>
     **/
    [Flags()]
    internal enum DisplayDeviceStateFlags : int
    {
        /// <summary>The device is part of the desktop.</summary>
        AttachedToDesktop = 0x1,
        MultiDriver = 0x2,
        /// <summary>The device is part of the desktop.</summary>
        PrimaryDevice = 0x4,
        /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
        MirroringDriver = 0x8,
        /// <summary>The device is VGA compatible.</summary>
        VGACompatible = 0x10,
        /// <summary>The device is removable; it cannot be the primary display.</summary>
        Removable = 0x20,
        /// <summary>The device has more display modes than its output devices support.</summary>
        ModesPruned = 0x8000000,
        Remote = 0x4000000,
        Disconnect = 0x2000000
    }
    #endregion

    #region Display settings flags
    /**
     * <summary>
     * http://www.pinvoke.net/default.aspx/user32/ChangeDisplaySettingsFlags.html
     * </summary>
     **/
    [Flags()]
    internal enum DisplaySettingsFlags : int
    {
        CDS_NONE = 0,
        CDS_UPDATEREGISTRY = 0x00000001,
        CDS_TEST = 0x00000002,
        CDS_FULLSCREEN = 0x00000004,
        CDS_GLOBAL = 0x00000008,
        CDS_SET_PRIMARY = 0x00000010,
        CDS_VIDEOPARAMETERS = 0x00000020,
        CDS_ENABLE_UNSAFE_MODES = 0x00000100,
        CDS_DISABLE_UNSAFE_MODES = 0x00000200,
        CDS_RESET = 0x40000000,
        CDS_RESET_EX = 0x20000000,
        CDS_NORESET = 0x10000000
    }
    #endregion

    #region Flag instruction codes
    /**
     * <summary>
     * Flag instruction codes.
     * </summary>
     **/
    [Flags()]
    internal enum DM : int
    {
        Orientation = 0x00000001,
        PaperSize = 0x00000002,
        PaperLength = 0x00000004,
        PaperWidth = 0x00000008,
        Scale = 0x00000010,
        Position = 0x00000020,
        NUP = 0x00000040,
        DisplayOrientation = 0x00000080,
        Copies = 0x00000100,
        DefaultSource = 0x00000200,
        PrintQuality = 0x00000400,
        Color = 0x00000800,
        Duplex = 0x00001000,
        YResolution = 0x00002000,
        TTOption = 0x00004000,
        Collate = 0x00008000,
        FormName = 0x00010000,
        LogPixels = 0x00020000,
        BitsPerPixel = 0x00040000,
        PelsWidth = 0x00080000,
        PelsHeight = 0x00100000,
        DisplayFlags = 0x00200000,
        DisplayFrequency = 0x00400000,
        ICMMethod = 0x00800000,
        ICMIntent = 0x01000000,
        MediaType = 0x02000000,
        DitherType = 0x04000000,
        PanningWidth = 0x08000000,
        PanningHeight = 0x10000000,
        DisplayFixedOutput = 0x20000000
    }
    #endregion
}
