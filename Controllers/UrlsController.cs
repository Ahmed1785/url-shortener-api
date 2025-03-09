using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UrlShortenerAPI.Controllers
{
    [Route("api/urls")]
    [ApiController]
    public class UrlsController : ControllerBase
    {
        // Temporary in-memory storage
        private static readonly Dictionary<string, Url> UrlDatabase = new();

        // POST api/urls
        [HttpPost]
        public ActionResult<Url> ShortenUrl([FromBody] Url urlRequest)
        {
            if (!Uri.IsWellFormedUriString(urlRequest.OriginalUrl, UriKind.Absolute))
            {
                return BadRequest("Invalid URL format.");
            }

            // Generate a short URL (simple example, can be improved)
            var shortenedUrl = Guid.NewGuid().ToString().Substring(0, 8); // First 8 chars of GUID
            urlRequest.ShortenedUrl = shortenedUrl;
            urlRequest.CreatedAt = DateTime.UtcNow;
            urlRequest.ClickCount = 0;

            // Store the URL in the "database"
            UrlDatabase[shortenedUrl] = urlRequest;

            return CreatedAtAction(nameof(GetOriginalUrl), new { shortenedUrl = shortenedUrl }, urlRequest);
        }

        // GET api/urls/{shortenedUrl}
        [HttpGet("{shortenedUrl}")]
        public ActionResult RedirectToUrl(string shortenedUrl)
        {
            if (!UrlDatabase.ContainsKey(shortenedUrl))
            {
                return NotFound("Shortened URL not found.");
            }

            // Increment the click count
            var url = UrlDatabase[shortenedUrl];
            url.ClickCount++;

            // Redirect to the original URL
            return Redirect(url.OriginalUrl);
        }

        // Test route
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "Hello from the URL Shortener API!" });
        }
    }
}
