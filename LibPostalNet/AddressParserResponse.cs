using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LibPostalNet
{
    public unsafe partial class AddressParserResponse : IDisposable
    {
        private IntPtr _Instance;
        private IntPtr _InputString;

        internal AddressParserResponse(string address, AddressParserOptions options)
        {
            if (ReferenceEquals(options, null)) throw new NullReferenceException();
            _InputString = MarshalUTF8.StringToPtr(address);
            var native = LibPostal.UnsafeNativeMethods.ParseAddress(_InputString, options._Native);
            if (native == IntPtr.Zero || native.ToPointer() == null)
                return;
            _Instance = native;
        }

        ~AddressParserResponse() { Dispose(false); }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_Instance != IntPtr.Zero)
            {
                LibPostal.UnsafeNativeMethods.AddressParserResponseDestroy(_Instance);
                _Instance = IntPtr.Zero;
            }
            if (_InputString != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_InputString);
                _InputString = IntPtr.Zero;
            }
        }

        public string Input => MarshalUTF8.PtrToString(_InputString);

        public long NumComponents
        {
            get
            {
                return (long)((UnsafeNativeMethods*)_Instance)->num_components;
            }
        }

        private string[] Components
        {
            get
            {
                long n = NumComponents;
                IntPtr components = ((UnsafeNativeMethods*)_Instance)->components;
                string[] ret = new string[n];
                for (int x = 0; x < n; x++)
                {
                    int offset = x * Marshal.SizeOf(typeof(IntPtr));
                    ret[x] = MarshalUTF8.PtrToString(Marshal.ReadIntPtr(components, offset));
                }
                return ret;
            }
        }

        private string[] Labels
        {
            get
            {
                long n = NumComponents;
                IntPtr labels = ((UnsafeNativeMethods*)_Instance)->labels;
                string[] ret = new string[n];
                for (int x = 0; x < n; x++)
                {
                    int offset = x * Marshal.SizeOf(typeof(IntPtr));
                    ret[x] = MarshalUTF8.PtrToString(Marshal.ReadIntPtr(labels, offset));
                }
                return ret;
            }
        }

        public List<KeyValuePair<string, string>> Results
        {
            get
            {
                var _results = new List<KeyValuePair<string, string>>();

                IntPtr
                    labels = ((UnsafeNativeMethods*) _Instance)->labels,
                    components = ((UnsafeNativeMethods*) _Instance)->components
                    ;

                long n = NumComponents;
                for (int x = 0; x < n; x++)
                {
                    int offset = x * Marshal.SizeOf(typeof(IntPtr));
                    _results.Add(new KeyValuePair<string, string>(
                        MarshalUTF8.PtrToString(Marshal.ReadIntPtr(labels, offset)),
                        MarshalUTF8.PtrToString(Marshal.ReadIntPtr(components, offset))
                    ));
                }

                return _results;
            }
        }
    }
}
