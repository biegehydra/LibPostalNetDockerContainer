using LibPostalNet;

namespace LibPostalApi.Models;
public record ParseAddressesRequest(List<string> Addresses);
public record ExpandAddressesRequest(List<string> Addresses);
public record ExpandAddressesResponse(List<AddressExpansionResponse> ParseResults);
public record ParseAddressesResponse(List<AddressParserResponse> ParseResults);
