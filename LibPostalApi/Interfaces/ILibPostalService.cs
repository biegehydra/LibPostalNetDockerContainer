using LibPostalNet;

namespace LibPostalApi.Interfaces;

public interface ILibPostalService
{
    AddressParserResponse ParseAddress(string address);
    AddressExpansionResponse ExpandAddress(string address);
}