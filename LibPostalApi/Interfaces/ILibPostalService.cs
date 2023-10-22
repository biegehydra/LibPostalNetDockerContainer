using LibPostalApi.Models;
using LibPostalNet;

namespace LibPostalApi.Interfaces;

public interface ILibPostalService
{
    ParseAddressesResponse? ParseAddress(List<string> addresses, ParseOptions? dtoOptions = null);
    ExpandAddressesResponse? ExpandAddress(List<string> addresses, ExpandOptions? dtoOptions = null);
}