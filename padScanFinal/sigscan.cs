using System;
using System.Runtime.InteropServices;

public class sigscan
{
    // REQUIRED CONSTS

    const int PROCESS_QUERY_INFORMATION = 0x0400;
    const int MEM_COMMIT = 0x00001000;
    const int PAGE_READWRITE = 0x04;
    const int PROCESS_WM_READ = 0x0010;

    // REQUIRED METHODS
    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
    [DllImport("kernel32.dll")]
    public static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern int VirtualQueryEx(IntPtr hProcess,IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);
    // REQUIRED STRUCTS
    public struct MEMORY_BASIC_INFORMATION
    {
        public int BaseAddress;
        public int AllocationBase;
        public int AllocationProtect;
        public int RegionSize;
        public int State;
        public int Protect;
        public int lType;
    }

    public struct SYSTEM_INFO
    {
        public ushort processorArchitecture;
        ushort reserved;
        public uint pageSize;
        public IntPtr minimumApplicationAddress;
        public IntPtr maximumApplicationAddress;
        public IntPtr activeProcessorMask;
        public uint numberOfProcessors;
        public uint processorType;
        public uint allocationGranularity;
        public ushort processorLevel;
        public ushort processorRevision;
    }

    public static byte[] ToByteArray(string value)
    {
        char[] charArr = value.ToCharArray();
        byte[] bytes = new byte[charArr.Length];
        for (int i = 0; i < charArr.Length; i++)
        {
            byte current = Convert.ToByte(charArr[i]);
            bytes[i] = current;
        }

        return bytes;
    }
}
