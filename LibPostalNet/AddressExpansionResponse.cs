using System;
using System.Runtime.InteropServices;

namespace LibPostalNet
{
    public unsafe class AddressExpansionResponse : IDisposable
    {
        private IntPtr _Instance;
        private IntPtr _InputString;
        private ulong _NumExpansions;

        public string Input => MarshalUTF8.PtrToString(_InputString);
        public string[] Expansions { get; private set; }

        internal AddressExpansionResponse(string input, AddressExpansionOptions options)
        {
            if (ReferenceEquals(options, null)) throw new NullReferenceException();
            _InputString = MarshalUTF8.StringToPtr(input);
            var native = LibPostal.UnsafeNativeMethods.ExpandAddress(_InputString, options._Native, ref _NumExpansions);
            if (native == IntPtr.Zero || native.ToPointer() == null)
                return;
            _Instance = native;

            Expansions = new string[_NumExpansions];
            for (int x = 0; x < (int)_NumExpansions; x++)
            {
                int offset = x * Marshal.SizeOf(typeof(IntPtr));
                Expansions[x] = MarshalUTF8.PtrToString(Marshal.ReadIntPtr(native, offset));
            }
        }

        ~AddressExpansionResponse() { Dispose(false); }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_Instance != IntPtr.Zero)
            {
                LibPostal.UnsafeNativeMethods.ExpansionArrayDestroy(_Instance, _NumExpansions);
                _Instance = IntPtr.Zero;
            }
            if (_InputString != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_InputString);
                _InputString = IntPtr.Zero;
            }
        }
    }
}
