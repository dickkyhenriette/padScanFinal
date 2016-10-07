using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace padScanFinal
{
    class Memory
    {
        #region Open and close a process
        /// <summary>
        /// usage to open and close to write!!!
        /// Memory temp = new Memory();
        /// Process[] process = Process.GetProcessesByName("notepad");
        /// temp.Readmemory = proc[0];
        /// label1.Text = temp.ToString();
        /// temp.Open();
        /// label2.Text = proc.ToString();
        /// temp.CloseHandle();
        /// </summary>
        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(UInt32 processAccess, UInt32 bInheritHandle, UInt32 processId);
        ///uint bInheritHandle was bool bInheritHandle and "ünt" UInt32 processId!!!
        public Process Readmemory { get; internal set; }
        private static IntPtr _hProcess = IntPtr.Zero;
        public void Open()
        {
            ProcessAccessFlags accessFlags = ProcessAccessFlags.All;
            _hProcess = OpenProcess((uint) accessFlags, 1, (uint) Readmemory.Id);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int CloseHandle(IntPtr hObject); ////bool CloseHandle to int CloseHandle!!

        public void CloseHandle()
        {
            int tjeck = CloseHandle(_hProcess);
            if (tjeck == 0)
            {
                throw new Exception("error is not closed");
            }
        }

        #endregion

        #region private sets.
        /// <summary>
        /// de dlls and the global vars.
        /// </summary>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize,out IntPtr lpNumberOfBytesRead);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern float ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref float lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern double ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref double lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern Int32 WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize,out IntPtr lpNumberOfBytesWritten);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern float WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref float lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern double WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref double lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);
        private static Int64 BaseAddress;
        private static int dwSize = 4;
        private static int show = 0;
        private static IntPtr writtPtr;
        private static byte[] lpBuffer = new byte[8];
        #endregion

        #region public read memory
        /// <summary> how to use
        /// Memory temp = new Memory();
        /// Process[] proc = Process.GetProcessesByName("notepad");
        /// temp.Readmemory = proc[0];
        /// if (proc.Length != 0)
        /// {
        ///     label1.Text = ("is ONLINE");
        ///     temp.Open();
        ///     int help = Memory.ReadMemory(("0022C138"), 4);
        ///     label2.Text = help.ToString();
        ///     temp.CloseHandle();
        /// }
        /// else
        /// {
        ///     label1.Text = ("is Offline");
        ///  }
        /// </summary>
        public static int ReadMemory(string lpBaseAddress)
        {
            BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
            ReadProcessMemory(_hProcess, (IntPtr) BaseAddress, lpBuffer, dwSize, out writtPtr);
            int show = BitConverter.ToInt32(lpBuffer, 0);
            return show;
        }
        /// <summary> how to use
        /// int read = Memory.ReadMemory(("notepad"), ("0022F720"), 4);
        /// label3.Text = (read.ToString());
        /// </summary>
        public static int ReadMemory(string openProgram, string lpBaseAddress)
        {
            Memory use = new Memory();
            Process[] proc = Process.GetProcessesByName(openProgram);
            if (proc.Length != 0)
            {
                use.Readmemory = proc[0];
                use.Open();
                BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                ReadProcessMemory(_hProcess, (IntPtr) BaseAddress, lpBuffer, dwSize, out writtPtr);
                show = BitConverter.ToInt32(lpBuffer, 0);
                use.CloseHandle();
                return show;
            }
            else
            {
                MessageBox.Show("Program is not running");
                return show;
            }
        }
        public static int ReadMemory(string openProgram, string lpBaseAddress, int dwSize)
        {
            Memory use = new Memory();
            Process[] proc = Process.GetProcessesByName(openProgram);
            if (proc.Length != 0)
            {
                use.Readmemory = proc[0];
                use.Open();
                BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                IntPtr writtPtr;
                ReadProcessMemory(_hProcess, (IntPtr) BaseAddress, lpBuffer, dwSize, out writtPtr);
                show = BitConverter.ToInt32(lpBuffer, 0);
                use.CloseHandle();
                return show;
            }
            else
            {
                MessageBox.Show("Program is not running");
                return show;
            }
        }
        /// <summary> how to use
        /// int[] offsets = { 0x7d0, 0x30, 0x558 };
        /// int read = Memory.ReadMemory(("notepad"), ("1B78FB86848"), offsets);
        /// label4.Text = (read.ToString());
        /// </summary>
        public static int ReadMemory(string openProgram, string lpBaseAddress, int[] offset)
        {
            int Ioffset = offset.Length;
            int tempAddress;
            Memory use = new Memory();
            Process[] proc = Process.GetProcessesByName(openProgram);
            if (proc.Length != 0)
            {
                if (Ioffset == 0)
                {
                    MessageBox.Show("No Offsets");
                }
                else
                {
                    for (int i = 0; i <= Ioffset; i++)
                    {
                        if (i == Ioffset)
                        {
                            use.Readmemory = proc[0];
                            use.Open();
                            BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                            ReadProcessMemory(_hProcess, (IntPtr) BaseAddress, lpBuffer, dwSize, out writtPtr);
                            tempAddress = lpBuffer[0] + offset[0];
                            ReadProcessMemory(_hProcess, (IntPtr) tempAddress, lpBuffer, dwSize, out writtPtr);
                            show = BitConverter.ToInt32(lpBuffer, 0);
                            use.CloseHandle();
                            return show;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Program is not running");
                return show;
            }
            return show;
        }

        /// <summary>
        /// float read = Memory.ReadM_float(("notepad"), ("001E0598"));
        /// label3.Text = (read.ToString());
        /// </summary>
        public static float ReadM_float(string openProgram, string lpBaseAddress)
        {
            Memory use = new Memory();
            Process[] proc = Process.GetProcessesByName(openProgram);
            if (proc.Length != 0)
            {
                use.Readmemory = proc[0];
                use.Open();
                BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                float buffer = new float();
                ReadProcessMemory(_hProcess, (IntPtr)BaseAddress,ref buffer, dwSize, out writtPtr);
                use.CloseHandle();
                return buffer;
            }
            else
            {
                MessageBox.Show("Program is not running");
                return show;
            }
        }
        /// <summary>
        /// float read = Memory.ReadM_float(("notepad"), ("001E0598"),4);
        /// label4.Text = ( read.ToString());
        /// </summary>
        public static float ReadM_float(string openProgram, string lpBaseAddress, int dwSize)
        {
            Memory use = new Memory();
            Process[] proc = Process.GetProcessesByName(openProgram);
            if (proc.Length != 0)
            {
                use.Readmemory = proc[0];
                use.Open();
                BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                float buffer = new float();
                ReadProcessMemory(_hProcess, (IntPtr)BaseAddress, ref buffer, dwSize, out writtPtr);
                use.CloseHandle();
                return buffer;
            }
            else
            {
                MessageBox.Show("Program is not running");
                return show;
            }
        }
        /// <summary>
        /// int[] offsets = { 0x598, 0x318, 0x2d8 };
        /// float read = Memory.ReadM_float(("notepad"), ("001E0598"), offsets);
        /// label2.Text = (read.ToString());
        /// </summary>
        public static float ReadM_float(string openProgram, string lpBaseAddress, int[] offset)
        {
            int Ioffset = offset.Length;
            int tempAddress;
            float buffer = new float();
            Memory use = new Memory();
            Process[] proc = Process.GetProcessesByName(openProgram);
            if (proc.Length != 0)
            {
                if (Ioffset == 0)
                {
                    MessageBox.Show("No Offsets");
                }
                else
                {
                    for (int i = 0; i <= Ioffset; i++)
                    {
                        if (i == Ioffset)
                        {
                            use.Readmemory = proc[0];
                            use.Open();
                            BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                            ReadProcessMemory(_hProcess, (IntPtr) BaseAddress,ref buffer, dwSize, out writtPtr);
                            int ram = Convert.ToInt32(buffer);
                            tempAddress = ram + offset[0];
                            ReadProcessMemory(_hProcess, (IntPtr) tempAddress,ref buffer, dwSize, out writtPtr);
                            use.CloseHandle();
                            return buffer;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Program is not running");
                return show;
            }
            return show;
        }

        /// <summary>
        /// double read = Memory.ReadM_double(("notepad"), ("00300694"));
        /// label3.Text = (read.ToString());
        /// </summary>
        public static double ReadM_double(string openProgram, string lpBaseAddress)
        {
            Memory use = new Memory();
            Process[] proc = Process.GetProcessesByName(openProgram);
            if (proc.Length != 0)
            {
                use.Readmemory = proc[0];
                use.Open();
                BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                double buffer = 0;
                ReadProcessMemory(_hProcess, (IntPtr)BaseAddress,ref buffer, 8, out writtPtr);
                use.CloseHandle();
                return buffer;
            }
            else
            {
                MessageBox.Show("Program is not running");
                return show;
            }
        }
        /// <summary>
        /// double read = Memory.ReadM_double(("notepad"), ("00300694"), 8); dwsize shut be always 8 with reading double
        /// label4.Text = ( read.ToString());
        /// </summary>
        public static double ReadM_double(string openProgram, string lpBaseAddress, int dwSize)
        {
            Memory use = new Memory();
            Process[] proc = Process.GetProcessesByName(openProgram);
            if (proc.Length != 0)
            {
                use.Readmemory = proc[0];
                use.Open();
                BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                double buffer = new double();
                ReadProcessMemory(_hProcess, (IntPtr)BaseAddress, ref buffer, 8, out writtPtr);
                use.CloseHandle();
                return buffer;
            }
            else
            {
                MessageBox.Show("Program is not running");
                return show;
            }
        }
        /// <summary>
        /// int[] offsets = { 0x70, 0x0, 0x104 };
        /// double read = Memory.ReadM_double(("notepad"), ("00300694"), offsets);
        /// label2.Text = (read.ToString());
        /// </summary>
        public static double ReadM_double(string openProgram, string lpBaseAddress, int[] offset)
        {
            Int64 Ioffset = offset.Length;
            Int64 tempAddress;
            double buffer = new double();
            Memory use = new Memory();
            Process[] proc = Process.GetProcessesByName(openProgram);
            if (proc.Length != 0)
            {
                if (Ioffset == 0)
                {
                    MessageBox.Show("No Offsets");
                }
                else
                {
                    for (int i = 0; i <= Ioffset; i++)
                    {
                        if (i == Ioffset)
                        {
                            use.Readmemory = proc[0];
                            use.Open();
                            BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                            ReadProcessMemory(_hProcess, (IntPtr)BaseAddress, ref buffer, 8, out writtPtr);
                            Int64 ram = Convert.ToInt64(buffer);
                            tempAddress = ram + offset[0];
                            ReadProcessMemory(_hProcess, (IntPtr)tempAddress, ref buffer, 8, out writtPtr);
                            use.CloseHandle();
                            return buffer;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Program is not running");
                return show;
            }
            return show;
        }

        /// <summary>
        /// string read = Memory.ReadM_string(("notepad"), ("1FF64D0B811"));
        /// label2.Text = read;
        public static string ReadM_string(string openProgram, string lpBaseAddress)
        {
            Memory use = new Memory();
            Process[] proc = Process.GetProcessesByName(openProgram);
            if (proc.Length != 0)
            {
                use.Readmemory = proc[0];
                use.Open();
                BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                ReadProcessMemory(_hProcess, (IntPtr)BaseAddress, lpBuffer, dwSize, out writtPtr);
                use.CloseHandle();
                return Encoding.Default.GetString(lpBuffer);
            }
            else
            {
                MessageBox.Show("Program is not running");
                return null;
            }
        }
        /// <summary>
        /// string read = Memory.ReadM_string(("notepad"), ("1FF64D0B811"),2);
        /// label2.Text = read;
        /// </summary>
        public static string ReadM_string(string openProgram, string lpBaseAddress, int dwSize)
        {
            Memory use = new Memory();
            Process[] proc = Process.GetProcessesByName(openProgram);
            if (proc.Length != 0)
            {
                use.Readmemory = proc[0];
                use.Open();
                BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                ReadProcessMemory(_hProcess, (IntPtr)BaseAddress, lpBuffer, dwSize, out writtPtr);
                use.CloseHandle();
                return Encoding.Default.GetString(lpBuffer);
            }
            else
            {
                MessageBox.Show("Program is not running");
                return null;
            }
        }
        /// <summary>
        /// int[] offsets = { 0x0, 0x140 };
        /// string read = Memory.ReadM_string(("notepad"), ("1FF64C543F0"), 10, offsets);
        /// label2.Text = read;
        /// </summary>
        public static string ReadM_string(string openProgram, string lpBaseAddress,int dwSize, int[] offset)
        {
            int Ioffset = offset.Length;
            int tempAddress;
            byte[] lpBuffer = new byte[dwSize];
            Memory use = new Memory();
            Process[] proc = Process.GetProcessesByName(openProgram);
            if (proc.Length != 0)
            {
                if (Ioffset == 0)
                {
                    MessageBox.Show("No Offsets");
                }
                else
                {
                    for (int i = 0; i <= Ioffset; i++)
                    {
                        if (i == Ioffset)
                        {
                            use.Readmemory = proc[0];
                            use.Open();
                            BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                            ReadProcessMemory(_hProcess, (IntPtr)BaseAddress, lpBuffer, dwSize, out writtPtr);
                            tempAddress = lpBuffer[0] + offset[0];
                            ReadProcessMemory(_hProcess, (IntPtr)tempAddress, lpBuffer, dwSize, out writtPtr);
                            use.CloseHandle();
                            return Encoding.Default.GetString(lpBuffer);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Program is not running");
                return null;
            }
            return null;
        }
        #endregion

        public static void WriteMemory(string openProgram, string lpBaseAddress,int toWrite)
        {
            byte[] lpBuffer = BitConverter.GetBytes(toWrite);
            Memory write = new Memory();
            Process[] proc = Process.GetProcessesByName(openProgram);
            if (proc.Length != 0)
            {
                write.Readmemory = proc[0];
                write.Open();
                BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                WriteProcessMemory(_hProcess, (IntPtr) BaseAddress,lpBuffer, lpBuffer.Length, out writtPtr);
                write.CloseHandle();
            }
        }
        public static void WriteMemory(string openProgram, string lpBaseAddress, int[] offset, int toWrite)
        {
            int Ioffset = offset.Length;
            int tempAddress;
            Memory use = new Memory();
            Process[] proc = Process.GetProcessesByName(openProgram);
            if (proc.Length != 0)
            {
                if (Ioffset == 0)
                {
                    MessageBox.Show("No Offsets");
                }
                else
                {
                    for (int i = 0; i <= Ioffset; i++)
                    {
                        if (i == Ioffset)
                        {
                            use.Readmemory = proc[0];
                            use.Open();
                            BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                            ReadProcessMemory(_hProcess, (IntPtr) BaseAddress, lpBuffer, dwSize, out writtPtr);
                            tempAddress = lpBuffer[0] + offset[0];
                            ReadProcessMemory(_hProcess, (IntPtr) tempAddress, lpBuffer, dwSize, out writtPtr);
                            show = BitConverter.ToInt32(lpBuffer, 0);
                            use.CloseHandle();
                        }
                        byte[] _lpBuffer = BitConverter.GetBytes(toWrite);
                        BaseAddress = show;
                        Memory write = new Memory();
                        if (proc.Length != 0)
                        {
                            write.Readmemory = proc[0];
                            write.Open();
                            BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                            WriteProcessMemory(_hProcess, (IntPtr) BaseAddress, _lpBuffer, _lpBuffer.Length,out writtPtr);
                            write.CloseHandle();
                        }
                    }
                }
            }
        }
        public static void Writedouble(string openProgram, string lpBaseAddress, double toWrite)
        {
            double lpBuffer = toWrite;
            Memory write = new Memory();
            Process[] proc = Process.GetProcessesByName(openProgram);
            if (proc.Length != 0)
            {
                write.Readmemory = proc[0];
                write.Open();
                BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                WriteProcessMemory(_hProcess, (IntPtr)BaseAddress,ref lpBuffer,8, out writtPtr);
                write.CloseHandle();
            }
        }
        public static void Writedouble(string openProgram, string lpBaseAddress, int[] offset, double toWrite)
        {
            int Ioffset = offset.Length;
            Int64 tempAddress;
            double buffer = new double();
            Memory use = new Memory();
            Process[] proc = Process.GetProcessesByName(openProgram);
            if (proc.Length != 0)
            {
                if (Ioffset == 0)
                {
                    MessageBox.Show("No Offsets");
                }
                else
                {
                    for (int i = 0; i <= Ioffset; i++)
                    {
                        if (i == Ioffset)
                        {
                            use.Readmemory = proc[0];
                            use.Open();
                            BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                            ReadProcessMemory(_hProcess, (IntPtr)BaseAddress, ref buffer, 8, out writtPtr);
                            Int64 ram = Convert.ToInt64(buffer);
                            tempAddress = ram + offset[0];
                            ReadProcessMemory(_hProcess, (IntPtr)tempAddress, ref buffer, 8, out writtPtr);
                            use.CloseHandle();
                        }
                        BaseAddress = Convert.ToInt64(buffer);
                        Memory write = new Memory();
                        if (proc.Length != 0)
                        {
                            write.Readmemory = proc[0];
                            write.Open();
                            BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                            WriteProcessMemory(_hProcess, (IntPtr)BaseAddress,ref toWrite, 8, out writtPtr);
                            write.CloseHandle();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Memory.WriteString(("notepad"), ("1FF64D0B811"),"sup");
        /// </summary>
        public static void WriteString(string openProgram, string lpBaseAddress, string toWrite)
        {
            byte[] lpBuffer = Encoding.Default.GetBytes(toWrite);
            Memory write = new Memory();
            Process[] proc = Process.GetProcessesByName(openProgram);
            if (proc.Length != 0)
            {
                write.Readmemory = proc[0];
                write.Open();
                BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                WriteProcessMemory(_hProcess, (IntPtr)BaseAddress, lpBuffer, lpBuffer.Length, out writtPtr);
                write.CloseHandle();
            }
        }
        /// <summary>
        /// int[] offsets = { 0x0, 0x140 };
        /// Memory.WriteString(("notepad"), ("1FF64C543F0 "), offsets, "bonder");
        /// </summary>
        public static void WriteString(string openProgram, string lpBaseAddress, int[] offset, string toWrite)
        {
            int Ioffset = offset.Length;
            int tempAddress;
            Memory use = new Memory();
            Process[] proc = Process.GetProcessesByName(openProgram);
            if (proc.Length != 0)
            {
                if (Ioffset == 0)
                {
                    MessageBox.Show("No Offsets");
                }
                else
                {
                    for (int i = 0; i <= Ioffset; i++)
                    {
                        if (i == Ioffset)
                        {
                            use.Readmemory = proc[0];
                            use.Open();
                            BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                            ReadProcessMemory(_hProcess, (IntPtr)BaseAddress, lpBuffer, dwSize, out writtPtr);
                            tempAddress = lpBuffer[0] + offset[0];
                            ReadProcessMemory(_hProcess, (IntPtr)tempAddress, lpBuffer, dwSize, out writtPtr);
                            show = BitConverter.ToInt32(lpBuffer, 0);
                            use.CloseHandle();
                        }
                        byte[] _lpBuffer = Encoding.Default.GetBytes(toWrite);
                        BaseAddress = show;
                        Memory write = new Memory();
                        if (proc.Length != 0)
                        {
                            write.Readmemory = proc[0];
                            write.Open();
                            BaseAddress = Int64.Parse(lpBaseAddress, NumberStyles.HexNumber);
                            WriteProcessMemory(_hProcess, (IntPtr)BaseAddress, _lpBuffer, _lpBuffer.Length, out writtPtr);
                            write.CloseHandle();
                        }
                    }
                }
            }
        }




    }

}

