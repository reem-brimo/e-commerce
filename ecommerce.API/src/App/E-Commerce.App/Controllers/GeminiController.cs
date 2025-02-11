using E_Commerce.API.Controllers;
using E_Commerce.Services.DTOs;
using E_Commerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace E_Commerce.App.Controllers
{

    [Route("api/[controller]")]
    public class GeminiController(IGeminiService geminiService) : BaseController
    {
        private readonly IGeminiService _geminiService = geminiService;

        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] string message)
        {
            if (string.IsNullOrEmpty(message))
                return BadRequest(new { message = "Message is required." });

            var result = await _geminiService.ChatAsync(message);
            return GetResult(result.ErrorMessages, HttpStatusCode.OK,result.Result);
        }
    }
}
