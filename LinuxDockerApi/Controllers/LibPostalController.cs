using LibPostalApi.Interfaces;
using LibPostalApi.Models;
using LibPostalNet;
using Microsoft.AspNetCore.Mvc;

namespace LibPostalApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LibPostalController : ControllerBase
    {
        private readonly ILibPostalService _libPostal;

        public LibPostalController(ILibPostalService libPostal)
        {
            _libPostal = libPostal;
        }

        [HttpPost]
        public ExpandAddressesResponse ExpandAddresses([FromBody] ExpandAddressesRequest request)
        {
            var responses = new List<AddressExpansionResponse>();
            foreach (var addresses in request.Addresses)
            {
                responses.Add(_libPostal.ExpandAddress(addresses));
            }
            return new ExpandAddressesResponse(responses);
        }

        [HttpPost]
        public ParseAddressesResponse ParseAddresses([FromBody] ParseAddressesRequest request)
        {
            var responses = new List<AddressParserResponse>();
            foreach (var addresses in request.Addresses)
            {
                responses.Add(_libPostal.ParseAddress(addresses));
            }
            return new ParseAddressesResponse(responses);
        }
    }
}