using System;
using System.Runtime.InteropServices;

namespace LibPostalNet
{
    public unsafe partial class AddressExpansionOptions
    {
        internal UnsafeNativeMethods _Native;

        internal AddressExpansionOptions()
        {
            _Native = LibPostal.UnsafeNativeMethods.GetDefaultOptions();
        }

        public string[] Languages
        {
            get
            {
                long n = NumLanguages;
                IntPtr languages = _Native.languages;
                return MarshalUTF8.PtrToStringArray(languages,(int) n);
            }
            set
            {
                NumLanguages = value.Length;
                _Native.languages = MarshalUTF8.StringArrayToPtr(value);
            }
        }

        public long NumLanguages
        {
            get { return (long)_Native.num_languages; }
            private set { _Native.num_languages = (ulong)value; }
        }

        public AddressComponents AddressComponents
        {
            get { return (AddressComponents)_Native.address_components; }
            set { _Native.address_components = (ushort)value; }
        }

        public bool LatinAscii
        {
            get { return _Native.latin_ascii != 0; }
            set { _Native.latin_ascii = (byte)(value ? 1 : 0); }
        }

        public bool Transliterate
        {
            get { return _Native.transliterate != 0; }
            set { _Native.transliterate = (byte)(value ? 1 : 0); }
        }

        public bool StripAccents
        {
            get { return _Native.strip_accents != 0; }
            set { _Native.strip_accents = (byte)(value ? 1 : 0); }
        }

        public bool Decompose
        {
            get { return _Native.decompose != 0; }
            set { _Native.decompose = (byte)(value ? 1 : 0); }
        }

        public bool Lowercase
        {
            get { return _Native.lowercase != 0; }
            set { _Native.lowercase = (byte)(value ? 1 : 0); }
        }

        public bool TrimString
        {
            get { return _Native.trim_string != 0; }
            set { _Native.trim_string = (byte)(value ? 1 : 0); }
        }

        public bool DropParentheticals
        {
            get { return _Native.drop_parentheticals != 0; }
            set { _Native.drop_parentheticals = (byte)(value ? 1 : 0); }
        }

        public bool ReplaceNumericHyphens
        {
            get { return _Native.replace_numeric_hyphens != 0; }
            set { _Native.replace_numeric_hyphens = (byte)(value ? 1 : 0); }
        }

        public bool DeleteNumericHyphens
        {
            get { return _Native.delete_numeric_hyphens != 0; }
            set { _Native.delete_numeric_hyphens = (byte)(value ? 1 : 0); }
        }

        public bool SplitAlphaFromNumeric
        {
            get { return _Native.split_alpha_from_numeric != 0; }
            set { _Native.split_alpha_from_numeric = (byte)(value ? 1 : 0); }
        }

        public bool ReplaceWordHyphens
        {
            get { return _Native.replace_word_hyphens != 0; }
            set { _Native.replace_word_hyphens = (byte)(value ? 1 : 0); }
        }

        public bool DeleteWordHyphens
        {
            get { return _Native.delete_word_hyphens != 0; }
            set { _Native.delete_word_hyphens = (byte)(value ? 1 : 0); }
        }

        public bool DeleteFinalPeriods
        {
            get { return _Native.delete_final_periods != 0; }
            set { _Native.delete_final_periods = (byte)(value ? 1 : 0); }
        }

        public bool DeleteAcronymPeriods
        {
            get { return _Native.delete_acronym_periods != 0; }
            set { _Native.delete_acronym_periods = (byte)(value ? 1 : 0); }
        }

        public bool DropEnglishPossessives
        {
            get { return _Native.drop_english_possessives != 0; }
            set { _Native.drop_english_possessives = (byte)(value ? 1 : 0); }
        }

        public bool DeleteApostrophes
        {
            get { return _Native.delete_apostrophes != 0; }
            set { _Native.delete_apostrophes = (byte)(value ? 1 : 0); }
        }

        public bool ExpandNumex
        {
            get { return _Native.expand_numex != 0; }
            set { _Native.expand_numex = (byte)(value ? 1 : 0); }
        }

        public bool RomanNumerals
        {
            get { return _Native.roman_numerals != 0; }
            set { _Native.roman_numerals = (byte)(value ? 1 : 0); }
        }
    }
}
