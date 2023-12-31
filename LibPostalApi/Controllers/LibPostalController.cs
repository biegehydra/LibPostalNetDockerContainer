using LibPostalApi.Interfaces;
using LibPostalApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        [ProducesResponseType(typeof(ExpandAddressesResponse), 200, "application/json")]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult ExpandAddresses([FromBody] ExpandAddressesRequest? request)
        {
            if (request?.Addresses is not {Count: > 0})
            {
                return BadRequest("No addresses in payload. Nothing to expand.");
            }

            if (request.Addresses.Count > 1000)
            {
                return BadRequest($"Too many addresses. Request had {request.Addresses.Count} addresses. Only 1000 addresses are allowed per request.");
            }

            var response = _libPostal.ExpandAddress(request.Addresses, request.ExpandOptions);
            if (response?.ExpandResults == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An internal server error occurred.");
            }

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ParseAddressesResponse), 200, "application/json")]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult ParseAddresses([FromBody] ParseAddressesRequest? request)
        {
            if (request?.Addresses is not { Count: > 0 })
            {
                return BadRequest("No addresses in payload. Nothing to parse.");
            }

            if (request.Addresses.Count > 1000)
            {
                return BadRequest($"Too many addresses. Request had {request.Addresses.Count} addresses. Only 1000 addresses are allowed per request.");
            }

            var response = _libPostal.ParseAddress(request.Addresses, request.ParseOptions);
            if (response?.ParseResults == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An internal server error occurred.");
            }

            return Ok(response);
        }
    }
}