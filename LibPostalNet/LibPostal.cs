using System;
using System.IO;
using System.Runtime.InteropServices;

namespace LibPostalNet
{
    public unsafe partial class LibPostal : IDisposable
    {
        // Instance Logic
        private static LibPostal s_instance;
        public static LibPostal GetInstance() { return GetInstance(null); }
        public static LibPostal GetInstance(string dataDir) 
        { 
            return s_instance ?? (s_instance = new LibPostal(dataDir));
        }

        // Library Logic
        private IntPtr _DataDirPtr;
        private string _DataDirStr;
        private bool _PrintFeatures;

        public string DataDir
        {
            get { return _DataDirStr; }
            set
            {
                if (!string.IsNullOrEmpty(_DataDirStr = value))
                {
                    if (_DataDirPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(_DataDirPtr);
                        _DataDirPtr = IntPtr.Zero;
                    }
                    _DataDirPtr = MarshalUTF8.StringToPtr(_DataDirStr);
                }
            }
        }
        public bool IsLoaded { get; private set; }
        public bool IsParserLoaded { get; private set; }
        public bool IsLanguageClassifierLoaded { get; private set; }
        public bool PrintFeatures
        {
            get { return _PrintFeatures; }
            set
            {
                if (IsParserLoaded)
                {
                    _PrintFeatures = value;
                    UnsafeNativeMethods.ParserPrintFeatures(_PrintFeatures);
                }
                else
                {
                    throw new InvalidOperationException("The LibPostal Parser must be loaded first.");
                }
            }
        }

        static LibPostal()
        {
            try
            {
                UriBuilder uri = new UriBuilder(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                string path = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
                if (!string.IsNullOrEmpty(path))
                {
                    path = Path.Combine(path, Environment.Is64BitProcess ? "x64" : "x86");
                    UnsafeNativeMethods.SetDllDirectory(path);
                }
            }
            catch (Exception) { }
        }
        private LibPostal(string dataDir)
        {
            IsLoaded = string.IsNullOrEmpty(DataDir = dataDir) ?
                UnsafeNativeMethods.Setup() :
                UnsafeNativeMethods.SetupDatadir(_DataDirPtr);
        }
        ~LibPostal() { Dispose(false); }
        public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
        protected virtual void Dispose(bool disposing)
        {
            s_instance = null;
            if (IsLoaded)
            {
                UnsafeNativeMethods.Teardown();
                IsLoaded = false;
            }
            if (IsParserLoaded)
            {
                UnsafeNativeMethods.TeardownParser();
                IsParserLoaded = false;
            }
            if (IsLanguageClassifierLoaded)
            {
                UnsafeNativeMethods.TeardownLanguageClassifier();
                IsLanguageClassifierLoaded = false;
            }
            if (_DataDirPtr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_DataDirPtr);
                _DataDirPtr = IntPtr.Zero;
            }
        }

        public void LoadParser()
        {
            if (!IsParserLoaded)
            {
                IsParserLoaded = string.IsNullOrEmpty(DataDir) ?
                    UnsafeNativeMethods.SetupParser() :
                    UnsafeNativeMethods.SetupParserDatadir(_DataDirPtr);
            }
        }
        public void LoadLanguageClassifier()
        {
            if (!IsLanguageClassifierLoaded)
            {
                IsLanguageClassifierLoaded = string.IsNullOrEmpty(DataDir) ?
                    UnsafeNativeMethods.SetupLanguageClassifier() :
                    UnsafeNativeMethods.SetupLanguageClassifierDatadir(_DataDirPtr);
            }
        }

        public AddressExpansionOptions GetAddressExpansionDefaultOptions()
        {
            return new AddressExpansionOptions();
        }
        public AddressExpansionResponse ExpandAddress(string input, AddressExpansionOptions options)
        {
            if (!IsLanguageClassifierLoaded) { LoadLanguageClassifier(); }
            return new AddressExpansionResponse(input, options);
        }

        public AddressParserOptions GetAddressParserDefaultOptions()
        {
            return new AddressParserOptions();
        }
        public AddressParserResponse ParseAddress(string address, AddressParserOptions options)
        {
            if (!IsParserLoaded) { LoadParser(); }
            return new AddressParserResponse(address, options);
        }
    }
}
