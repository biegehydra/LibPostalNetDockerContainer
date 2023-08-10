using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;

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

        public long NumComponents
        {
            get
            {
                return (long)((UnsafeNativeMethods*)_Instance)->num_components;
            }
        }

        public string[] Components
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

        public string[] Labels
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
                    labels = ((UnsafeNativeMethods*)_Instance)->labels,
                    components = ((UnsafeNativeMethods*)_Instance)->components
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

        public string ToJSON()
        {
            var json = new JObject();
            var grp = Results.GroupBy(K => K.Key, V => V.Value, (key, g) => new { Key = key, Value = g.ToArray() });
            foreach (var x in grp)
            {
                var values = new JArray();
                foreach (var y in x.Value)
                {
                    values.Add(new JValue(y));
                }
                json.Add(new JProperty(x.Key, values));
            }
            return json.ToString(Newtonsoft.Json.Formatting.None);
        }

        public string ToXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", string.Empty, string.Empty));
            var address = doc.CreateElement("address");
            foreach (var x in Results)
            {
                var elem = doc.CreateElement(x.Key);
                elem.AppendChild(doc.CreateTextNode(x.Value));
                address.AppendChild(elem);
            }
            doc.AppendChild(address);
            return doc.OuterXml;
        }
    }
}
