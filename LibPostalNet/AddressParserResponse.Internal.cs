using System;
using System.Runtime.InteropServices;

namespace LibPostalNet
{
    public partial class AddressParserResponse
    {
        [StructLayout(LayoutKind.Explicit, Size = 24)]
        protected internal struct UnsafeNativeMethods
        {
            [FieldOffset(0)]
            internal ulong num_components;

            [FieldOffset(8)]
            internal IntPtr components;

            [FieldOffset(16)]
            internal IntPtr labels;

            //[SuppressUnmanagedCodeSecurity]
            //[DllImport("libpostal", CallingConvention = CallingConvention.Cdecl,
            //    EntryPoint = "??0libpostal_address_parser_response@@QEAA@AEBU0@@Z")]
            //internal static extern IntPtr cctor(IntPtr instance, IntPtr _0);
        }
    }
}
