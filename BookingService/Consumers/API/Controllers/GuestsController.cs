using Application.Guest.DTO;
using Application.Guest.Requests;
using Application.Ports;
using Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuestsController : ControllerBase
    {
        private readonly ILogger<GuestsController> _logger;
        private readonly IGuestManager _guestManager;

        public GuestsController(ILogger<GuestsController> logger, IGuestManager guestManager)
        {
            _logger = logger;
            _guestManager = guestManager;
        }

        [HttpPost]
        public async Task
            <ActionResult<GuestDTO>> Post(GuestDTO guest)
        {
            var request = new CreateGuestRequest
            {
                Data = guest
            };

            var res = await _guestManager.CreateGuest(request);

            if (res.Success) return Created("", res.Data);

            if(res.ErrorCode == ErrorCodes.NOT_FOUND)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.INVALID_PERSON_ID)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.MISSING_REQUIRED_INFORMATION)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.INVALID_EMAIL)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.COULD_NOT_STORE_DATA)
            {
                return BadRequest(res);
            }

            _logger.LogError("Response with unknow ErrorCode Returned", res);
            return BadRequest(500);
        }
    }
}
