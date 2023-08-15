using LibPostalApi.Models;
using LibPostalNet;

namespace LibPostalApi.ExtensionMethods;

public static class MappingExtensions
{
    public static void MapValuesTo(this ExpandOptions dto, AddressExpansionOptions domain)
    {
        if (dto.Languages is { Length: > 0 })
        {
            domain.Languages = dto.Languages;
        }
        if (dto.AddressComponents != null)
        {
            domain.AddressComponents = (AddressComponents)dto.AddressComponents;
        }
        if (dto.LatinAscii.HasValue)
        {
            domain.LatinAscii = dto.LatinAscii.Value;
        }
        if (dto.Transliterate.HasValue)
        {
            domain.Transliterate = dto.Transliterate.Value;
        }
        if (dto.StripAccents.HasValue)
        {
            domain.StripAccents = dto.StripAccents.Value;
        }
        if (dto.Decompose.HasValue)
        {
            domain.Decompose = dto.Decompose.Value;
        }
        if (dto.Lowercase.HasValue)
        {
            domain.Lowercase = dto.Lowercase.Value;
        }
        if (dto.TrimString.HasValue)
        {
            domain.TrimString = dto.TrimString.Value;
        }
        if (dto.DropParentheticals.HasValue)
        {
            domain.DropParentheticals = dto.DropParentheticals.Value;
        }
        if (dto.ReplaceNumericHyphens.HasValue)
        {
            domain.ReplaceNumericHyphens = dto.ReplaceNumericHyphens.Value;
        }
        if (dto.DeleteNumericHyphens.HasValue)
        {
            domain.DeleteNumericHyphens = dto.DeleteNumericHyphens.Value;
        }
        if (dto.SplitAlphaFromNumeric.HasValue)
        {
            domain.SplitAlphaFromNumeric = dto.SplitAlphaFromNumeric.Value;
        }
        if (dto.ReplaceWordHyphens.HasValue)
        {
            domain.ReplaceWordHyphens = dto.ReplaceWordHyphens.Value;
        }
        if (dto.DeleteWordHyphens.HasValue)
        {
            domain.DeleteWordHyphens = dto.DeleteWordHyphens.Value;
        }
        if (dto.DeleteFinalPeriods.HasValue)
        {
            domain.DeleteFinalPeriods = dto.DeleteFinalPeriods.Value;
        }
        if (dto.DeleteAcronymPeriods.HasValue)
        {
            domain.DeleteAcronymPeriods = dto.DeleteAcronymPeriods.Value;
        }
        if (dto.DropEnglishPossessives.HasValue)
        {
            domain.DropEnglishPossessives = dto.DropEnglishPossessives.Value;
        }
        if (dto.DeleteApostrophes.HasValue)
        {
            domain.DeleteApostrophes = dto.DeleteApostrophes.Value;
        }
        if (dto.ExpandNumex.HasValue)
        {
            domain.ExpandNumex = dto.ExpandNumex.Value;
        }
        if (dto.RomanNumerals.HasValue)
        {
            domain.RomanNumerals = dto.RomanNumerals.Value;
        }
    }

    public static void MapValuesTo(this ParseOptions dto, AddressParserOptions domain)
    {
        if (!string.IsNullOrEmpty(dto.Country))
        {
            domain.Country = dto.Country.Trim();
        }

        if (!string.IsNullOrEmpty(dto.Language))
        {
            domain.Language = dto.Language.Trim();
        }
    }
}