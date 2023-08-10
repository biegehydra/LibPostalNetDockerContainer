
namespace LibPostalNet
{
    public unsafe partial class AddressParserOptions
    {
        internal UnsafeNativeMethods _Native;

        internal AddressParserOptions()
        {
            _Native = LibPostal.UnsafeNativeMethods.GetAddressParserDefaultOptions();
        }

        public string Language
        {
            get { return MarshalUTF8.PtrToString(_Native.language); }
            set { _Native.language = MarshalUTF8.StringToPtr(value); }
        }

        public string Country
        {
            get { return MarshalUTF8.PtrToString(_Native.country); }
            set { _Native.country = MarshalUTF8.StringToPtr(value); }
        }
    }
}
