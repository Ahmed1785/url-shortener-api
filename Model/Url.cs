using System;

namespace UrlShortenerAPI.Models
{
    public class Url
    {
        private string _originalUrl;
        private string _shortenedUrl;
        private DateTime _expiryDate;
        private DateTime _createdAt;
        private int _clickCount;

        // Property for OriginalUrl with validation
        public string OriginalUrl
        {
            get { return _originalUrl; }
            set
            {
                // Ensure the Original URL is valid
                if (!Uri.IsWellFormedUriString(value, UriKind.Absolute))
                    throw new ArgumentException("Invalid URL format.", nameof(OriginalUrl));

                _originalUrl = value;
            }
        }

        // Property for ShortenedUrl with validation
        public string ShortenedUrl
        {
            get { return _shortenedUrl; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Shortened URL cannot be empty.", nameof(ShortenedUrl));

                _shortenedUrl = value;
            }
        }

        // Property for ExpiryDate with validation
        public DateTime ExpiryDate
        {
            get { return _expiryDate; }
            set
            {
                // Ensure ExpiryDate is not in the past
                if (value < DateTime.UtcNow)
                    throw new ArgumentException("Expiry date cannot be in the past.", nameof(ExpiryDate));

                _expiryDate = value;
            }
        }

        // Property for CreatedAt with validation
        public DateTime CreatedAt
        {
            get { return _createdAt; }
            private set
            {
                // CreatedAt should be set only once (when the object is created)
                if (_createdAt != default)
                    throw new InvalidOperationException("CreatedAt cannot be modified.");

                _createdAt = value;
            }
        }

        // Property to track number of clicks
        public int ClickCount
        {
            get { return _clickCount; }
            private set
            {
                // Click count should not be negative
                if (value < 0)
                    throw new ArgumentException("Click count cannot be negative.", nameof(ClickCount));

                _clickCount = value;
            }
        }

        // Constructor for the model (sets CreatedAt automatically to the current UTC time)
        public Url(string originalUrl, string shortenedUrl, DateTime expiryDate)
        {
            OriginalUrl = originalUrl;
            ShortenedUrl = shortenedUrl;
            ExpiryDate = expiryDate;
            CreatedAt = DateTime.UtcNow;  // Set CreatedAt to current time
            ClickCount = 0;  // Initialize click count to 0
        }

        // Method to increment the click count
        public void IncrementClickCount()
        {
            ClickCount++;
        }
    }
}
