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

        public string OriginalUrl
        {
            get { return _originalUrl; }
            set
            {
                if (!Uri.IsWellFormedUriString(value, UriKind.Absolute))
                    throw new ArgumentException("Invalid URL format.", nameof(OriginalUrl));

                _originalUrl = value;
            }
        }

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

        public DateTime ExpiryDate
        {
            get { return _expiryDate; }
            set
            {
                if (value < DateTime.UtcNow)
                    throw new ArgumentException("Expiry date cannot be in the past.", nameof(ExpiryDate));

                _expiryDate = value;
            }
        }

        public DateTime CreatedAt
        {
            get { return _createdAt; }
            private set
            {
                if (_createdAt != default)
                    throw new InvalidOperationException("CreatedAt cannot be modified.");

                _createdAt = value;
            }
        }

        public int ClickCount
        {
            get { return _clickCount; }
            private set
            {
                if (value < 0)
                    throw new ArgumentException("Click count cannot be negative.", nameof(ClickCount));

                _clickCount = value;
            }
        }

        public Url(string originalUrl, string shortenedUrl, DateTime expiryDate)
        {
            OriginalUrl = originalUrl;
            ShortenedUrl = shortenedUrl;
            ExpiryDate = expiryDate;
            CreatedAt = DateTime.UtcNow;
            ClickCount = 0;
        }

        public void IncrementClickCount()
        {
            ClickCount++;
        }
    }
}
