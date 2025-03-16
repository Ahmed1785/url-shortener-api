using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UrlShortenerAPI.Data;
using UrlShortenerAPI.Models;

namespace UrlShortenerAPI.Controllers
{
    [Route("api/urls")]
    [ApiController]
    public class UrlsController : ControllerBase
    {

        private readonly CosmosDbService _cosmosDbService;

        public UrlsController(CosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }
        // Temporary in-memory storage
        private static readonly Dictionary<string, Url> UrlDatabase = new();

        [HttpPost]
        public async Task<IActionResult> CreateUrl([FromBody] Url url)
        {
            await _cosmosDbService.CreateOrUpdateUrlAsync(url);
            return Ok(url);
        }

        // GET api/urls/{shortenedUrl}
        [HttpGet("{shortenedUrl}")]
        public async Task<IActionResult> GetUrl(string shortenedUrl)
        {
            var url = await _cosmosDbService.GetUrlAsync(shortenedUrl);
            if (url == null)
            {
                return NotFound();
            }
            return Ok(url);
        }

        // Test route
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "Hello from the URL Shortener API!" });
        }
    }
}