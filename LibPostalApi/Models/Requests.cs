using LibPostalNet;

namespace LibPostalApi.Models;
public record ParseAddressesRequest(List<string>? Addresses, ParseOptions? ParseOptions);
public record ExpandAddressesRequest(List<string>? Addresses, ExpandOptions? ExpandOptions);
public record ExpandAddressesResponse(AddressExpansionResponse[]? ExpandResults, int AddressesReceived, int SuccessfullyParsedCount, int UnsuccessfullyParsedCount);
public record ParseAddressesResponse(AddressParserResponse[]? ParseResults, int AddressesReceived, int SuccessfullyExpandCount, int UnsuccessfullyExpandedCount);
public record ParseOptions(string? Country, string? Language);

public record ExpandOptions(
    string[]? Languages,
    int? AddressComponents,
    bool? LatinAscii,
    bool? Transliterate,
    bool? StripAccents,
    bool? Decompose,
    bool? Lowercase,
    bool? TrimString,
    bool? DropParentheticals,
    bool? ReplaceNumericHyphens,
    bool? DeleteNumericHyphens,
    bool? SplitAlphaFromNumeric,
    bool? ReplaceWordHyphens,
    bool? DeleteWordHyphens,
    bool? DeleteFinalPeriods,
    bool? DeleteAcronymPeriods,
    bool? DropEnglishPossessives,
    bool? DeleteApostrophes,
    bool? ExpandNumex,
    bool? RomanNumerals
);
