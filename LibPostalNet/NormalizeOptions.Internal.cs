using System;
using System.Runtime.InteropServices;

namespace LibPostalNet
{
    public partial class AddressExpansionOptions
    {
        [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 36)]
        protected internal struct UnsafeNativeMethods
        {
            [FieldOffset(0)]
            internal IntPtr languages;

            [FieldOffset(8)]
            internal ulong num_languages;

            [FieldOffset(16)]
            internal ushort address_components;

            [FieldOffset(18)]
            internal byte latin_ascii;

            [FieldOffset(19)]
            internal byte transliterate;

            [FieldOffset(20)]
            internal byte strip_accents;

            [FieldOffset(21)]
            internal byte decompose;

            [FieldOffset(22)]
            internal byte lowercase;

            [FieldOffset(23)]
            internal byte trim_string;

            [FieldOffset(24)]
            internal byte drop_parentheticals;

            [FieldOffset(25)]
            internal byte replace_numeric_hyphens;

            [FieldOffset(26)]
            internal byte delete_numeric_hyphens;

            [FieldOffset(27)]
            internal byte split_alpha_from_numeric;

            [FieldOffset(28)]
            internal byte replace_word_hyphens;

            [FieldOffset(29)]
            internal byte delete_word_hyphens;

            [FieldOffset(30)]
            internal byte delete_final_periods;

            [FieldOffset(31)]
            internal byte delete_acronym_periods;

            [FieldOffset(32)]
            internal byte drop_english_possessives;

            [FieldOffset(33)]
            internal byte delete_apostrophes;

            [FieldOffset(34)]
            internal byte expand_numex;

            [FieldOffset(35)]
            internal byte roman_numerals;

            //[SuppressUnmanagedCodeSecurity]
            //[DllImport("libpostal", CallingConvention = CallingConvention.Cdecl,
            //    EntryPoint = "??0libpostal_normalize_options@@QEAA@AEBU0@@Z")]
            //internal static extern IntPtr cctor(IntPtr instance, IntPtr _0);
        }
    }
}
