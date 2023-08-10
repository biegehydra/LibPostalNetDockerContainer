using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LibPostalNet
{
    internal static class MarshalUTF8
    {
        public static IntPtr StringToPtr(string input)
        {
            var len = Encoding.UTF8.GetByteCount(input);
            var utf8Bytes = new byte[len + 1];
            Encoding.UTF8.GetBytes(input, 0, input.Length, utf8Bytes, 0);
            utf8Bytes[len] = 0;
            IntPtr retPtr = Marshal.AllocHGlobal(len + 1);
            Marshal.Copy(utf8Bytes, 0, retPtr, len + 1);
            return retPtr;
        }

        public static IntPtr StringArrayToPtr(string[] input)
        {
            int totalLength = input.Length * IntPtr.Size;
            IntPtr[] ptrs = new IntPtr[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                ptrs[i] = StringToPtr(input[i]);
            }
            IntPtr retPtr = Marshal.AllocHGlobal(totalLength);
            Marshal.Copy(ptrs, 0, retPtr, input.Length);
            return retPtr;
        }

        public static string PtrToString(IntPtr ptr)
        {
            int len = -1;
            while (true)
            {
                var ch = Marshal.ReadByte(ptr, ++len);
                if (ch == 0) break;
            }
            var utf8Bytes = new byte[len];
            Marshal.Copy(ptr, utf8Bytes, 0, len);
            return Encoding.UTF8.GetString(utf8Bytes);
        }

        public static string[] PtrToStringArray(IntPtr ptr, int length)
        {
            string[] ret = new string[length];
            for (int x = 0; x < length; x++)
            {
                int offset = x * Marshal.SizeOf(typeof(IntPtr));
                IntPtr strPtr = Marshal.ReadIntPtr(ptr, offset);
                ret[x] = PtrToString(strPtr);
                Marshal.FreeHGlobal(strPtr); // Free the individual strings
            }
            Marshal.FreeHGlobal(ptr); // Free the overall block
            return ret;
        }
    }
}
