using LibPostalApi.Models;
using LibPostalNet;

namespace LibPostalApi.Interfaces;

public interface ILibPostalService
{
    (AddressParserResponse[]? Results, int Successes, int Failures) ParseAddress(List<string> addresses, ParseOptions? dtoOptions = null);
    (AddressExpansionResponse[]? Results, int Successes, int Failures) ExpandAddress(List<string> addresses, ExpandOptions? dtoOptions = null);
}