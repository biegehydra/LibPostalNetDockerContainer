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
        public IActionResult ExpandAddresses([FromBody] ExpandAddressesRequest? request)
        {
            if (request?.Addresses is not {Count: > 0})
            {
                return BadRequest("No addresses in payload. Nothing to expand.");
            }

            var results = _libPostal.ExpandAddress(request.Addresses, request.ExpandOptions);
            if (results.Results == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An internal server error occurred.");
            }

            return Ok(new ExpandAddressesResponse(results.Results, request.Addresses.Count, results.Successes, results.Failures));
        }

        [HttpPost]
        public IActionResult ParseAddresses([FromBody] ParseAddressesRequest? request)
        {
            if (request?.Addresses is not { Count: > 0 })
            {
                return BadRequest("No addresses in payload. Nothing to expand.");
            }
            var results = _libPostal.ParseAddress(request.Addresses, request.ParseOptions);
            if (results.Results == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An internal server error occurred.");
            }

            return Ok(new ParseAddressesResponse(results.Results, request.Addresses.Count, results.Successes, results.Failures));
        }
    }
}