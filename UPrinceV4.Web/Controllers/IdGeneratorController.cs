using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UPrinceV4.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdGeneratorController : ControllerBase
{
    //[HttpGet("Read")]
    [HttpGet("Read")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<string> GetId()
    {
        return Guid.NewGuid().ToString();
    }
}