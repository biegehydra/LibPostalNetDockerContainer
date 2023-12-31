﻿using LibPostalApi.ExtensionMethods;
using LibPostalApi.Interfaces;
using LibPostalApi.Models;
using LibPostalNet;

namespace LibPostalApi.Services;

public class LibPostalService : ILibPostalService
{
    public static bool Initialized = false;
    private readonly LibPostal _libPostal;
    public LibPostalService(IConfiguration config)
    {
        var dataDir = config["libpostal:datadir"];
        Console.WriteLine($"DataDir: {dataDir}");
        var currentDir = Directory.GetCurrentDirectory();
        Console.WriteLine($"CurrentDir: {currentDir}");
        dataDir = Path.Combine(Directory.GetParent(currentDir)!.FullName, dataDir!);
        Console.WriteLine($"CombinedDir: {dataDir}");     

        _libPostal = LibPostal.GetInstance(dataDir);
        _libPostal.LoadParser();
        _libPostal.LoadLanguageClassifier();
        _libPostal.PrintFeatures = true;
        Initialized = true;
    }

    public ParseAddressesResponse? ParseAddress(List<string> addresses, ParseOptions? dtoOptions)
    {
        try
        {
            var defaultOptions = _libPostal.GetAddressParserDefaultOptions();

            dtoOptions?.MapValuesTo(defaultOptions);

            AddressParserResponse[] results = new AddressParserResponse[addresses.Count];

            var successes = 0;
            var failures = 0;

            var i = 0;
            foreach (var address in addresses)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(address))
                    {
                        failures++;
                        continue;
                    }
                    var result = _libPostal.ParseAddress(address, defaultOptions);
                    results[i] = result;
                    successes++;
                    i++;
                }
                catch (Exception ex)
                {
                    failures++;
                    Console.WriteLine($"Error Inside Parse. Input: {addresses} : {ex}");
                }
            }
            return new (results, results.Length, successes, failures);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Outside Parse: {ex}");
            return default;
        }
    }

    public ExpandAddressesResponse? ExpandAddress(List<string> addresses, ExpandOptions? dtoOptions)
    {
        try
        {
            var defaultOptions = _libPostal.GetAddressExpansionDefaultOptions();

            dtoOptions?.MapValuesTo(defaultOptions);

            AddressExpansionResponse[] results = new AddressExpansionResponse[addresses.Count];

            var successes = 0;
            var failures = 0;

            var i = 0;

            foreach (var address in addresses)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(address))
                    {
                        failures++;
                        continue;
                    }
                    var result = _libPostal.ExpandAddress(address, defaultOptions);
                    results[i] = result;
                    successes++;
                    i++;
                }
                catch (Exception ex)
                {
                    failures++;
                    Console.WriteLine($"Error Inside Expand. Input: {addresses} : {ex}");
                }
            }
            return new (results, results.Length, successes, failures);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Outside Expand: {ex}");
            return default;
        }
    }
}