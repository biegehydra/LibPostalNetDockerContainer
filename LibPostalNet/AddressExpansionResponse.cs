using Newtonsoft.Json.Linq;
using System;
using System.Runtime.InteropServices;
using System.Xml;

namespace LibPostalNet
{
    public unsafe class AddressExpansionResponse : IDisposable
    {
        private IntPtr _Instance;
        private IntPtr _InputString;
        private ulong _NumExpansions;

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

        public string ToJSON()
        {
            var json = new JArray();
            foreach (var expansion in Expansions)
            {
                json.Add(new JValue(expansion));
            }
            return json.ToString(Newtonsoft.Json.Formatting.None);
        }

        public string ToXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", string.Empty, string.Empty));
            var address = doc.CreateElement("address");
            foreach (var expansion in Expansions)
            {
                var elem = doc.CreateElement("expansion");
                elem.AppendChild(doc.CreateTextNode(expansion));
                address.AppendChild(elem);
            }
            doc.AppendChild(address);
            return doc.OuterXml;
        }
    }
}
