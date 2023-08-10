using System;
using System.Runtime.InteropServices;

namespace LibPostalNet
{
    public partial class AddressParserOptions
    {
        [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 16)]
        protected internal struct UnsafeNativeMethods
        {
            [FieldOffset(0)]
            internal IntPtr language;

            [FieldOffset(8)]
            internal IntPtr country;

            //[SuppressUnmanagedCodeSecurity]
            //[DllImport("libpostal", CallingConvention = CallingConvention.Cdecl,
            //    EntryPoint = "??0libpostal_address_parser_options@@QEAA@AEBU0@@Z")]
            //internal static extern IntPtr cctor(IntPtr instance, IntPtr _0);
        }
    }
}
