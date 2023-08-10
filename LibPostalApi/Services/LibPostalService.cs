using LibPostalApi.Interfaces;
using LibPostalNet;

namespace LibPostalApi.Services;

public class LibPostalService : ILibPostalService
{
    private readonly ILogger<LibPostalService> _logger;
    private readonly LibPostal _libPostal;
    private readonly AddressParserOptions _addressParserOptions;
    private readonly AddressExpansionOptions _addressExpansionOptions;
    public LibPostalService(ILogger<LibPostalService> logger, IConfiguration config)
    {
        _logger = logger;
        var dataDir = config["libpostal:datadir"];
        Console.WriteLine($"DataDir: {dataDir}");
        var currentDir = Directory.GetCurrentDirectory();
        Console.WriteLine($"CurrentDir: {currentDir}");
        dataDir = Path.Combine(Directory.GetParent(currentDir)!.FullName, dataDir!);
        Console.WriteLine($"Combine: {dataDir}");     

        _libPostal = LibPostal.GetInstance(dataDir);
        _libPostal.LoadParser();
        _libPostal.LoadLanguageClassifier();
        _libPostal.PrintFeatures = true;
        _addressParserOptions = _libPostal.GetAddressParserDefaultOptions();
        _addressParserOptions.Country = "us";
        _addressParserOptions.Language = "en";
        _addressExpansionOptions = _libPostal.GetAddressExpansionDefaultOptions();
        _addressExpansionOptions.Languages = new string[] { "en" };
    }

    private void SetAllSettingsTrue()
    {
        _addressExpansionOptions.Lowercase = true;
        _addressExpansionOptions.ExpandNumex = true;
        _addressExpansionOptions.TrimString = true;
        _addressExpansionOptions.Decompose = true;
        _addressExpansionOptions.DropEnglishPossessives = true;


        _addressExpansionOptions.DeleteFinalPeriods = true;
        _addressExpansionOptions.DeleteApostrophes = true;
        _addressExpansionOptions.DeleteNumericHyphens = true;
        _addressExpansionOptions.DeleteAcronymPeriods = true;
        _addressExpansionOptions.DeleteWordHyphens = true;


        _addressExpansionOptions.ReplaceNumericHyphens = true;
        _addressExpansionOptions.RomanNumerals = true;
        _addressExpansionOptions.StripAccents = true;
        _addressExpansionOptions.DropParentheticals = true;
        _addressExpansionOptions.Transliterate = true;
        _addressExpansionOptions.SplitAlphaFromNumeric = true;
        _addressExpansionOptions.LatinAscii = true;
    }

    private void SetAllSettingsFalse()
    {
        _addressExpansionOptions.Lowercase = true;
        _addressExpansionOptions.ExpandNumex = false;
        _addressExpansionOptions.TrimString = true;
        _addressExpansionOptions.Decompose = true;
        _addressExpansionOptions.DropEnglishPossessives = true;


        _addressExpansionOptions.DeleteFinalPeriods = false;
        _addressExpansionOptions.DeleteApostrophes = false;
        _addressExpansionOptions.DeleteNumericHyphens = false;
        _addressExpansionOptions.DeleteAcronymPeriods = false;
        _addressExpansionOptions.DeleteWordHyphens = false;


        _addressExpansionOptions.ReplaceNumericHyphens = false;
        _addressExpansionOptions.RomanNumerals = false;
        _addressExpansionOptions.StripAccents = false;
        _addressExpansionOptions.DropParentheticals = false;
        _addressExpansionOptions.Transliterate = false;
        _addressExpansionOptions.SplitAlphaFromNumeric = false;
        _addressExpansionOptions.LatinAscii = false;
    }

    public AddressParserResponse ParseAddress(string address)
    {
        return _libPostal.ParseAddress(address, _addressParserOptions);
    }

    private static int Count = 0;
    public AddressExpansionResponse ExpandAddress(string address)
    {
        if (Count % 2 == 0)
        {
            SetAllSettingsTrue();
        }
        else
        {
            SetAllSettingsFalse();
        }
        Count++;
        return _libPostal.ExpandAddress(address, _addressExpansionOptions);
    }
}