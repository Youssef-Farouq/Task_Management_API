using System.Collections.Concurrent;

namespace TaskManager.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimitingMiddleware> _logger;
        private static readonly ConcurrentDictionary<string, TokenBucket> _buckets = new();
        private const int MaxRequestsPerMinute = 60;
        private const int BurstSize = 10;

        public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var key = GetClientKey(context);
            var bucket = _buckets.GetOrAdd(key, _ => new TokenBucket(MaxRequestsPerMinute, BurstSize));

            if (!bucket.TryConsume())
            {
                _logger.LogWarning("Rate limit exceeded for client: {ClientKey}", key);
                context.Response.StatusCode = 429; // Too Many Requests
                await context.Response.WriteAsJsonAsync(new { error = "Rate limit exceeded. Please try again later." });
                return;
            }

            await _next(context);
        }

        private string GetClientKey(HttpContext context)
        {
            var key = context.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? 
                     context.Connection.RemoteIpAddress?.ToString() ?? 
                     "unknown";
            return key;
        }
    }

    public class TokenBucket
    {
        private readonly double _tokensPerSecond;
        private readonly int _burstSize;
        private double _tokens;
        private DateTime _lastRefill;

        public TokenBucket(int requestsPerMinute, int burstSize)
        {
            _tokensPerSecond = requestsPerMinute / 60.0;
            _burstSize = burstSize;
            _tokens = burstSize;
            _lastRefill = DateTime.UtcNow;
        }

        public bool TryConsume()
        {
            RefillTokens();
            if (_tokens >= 1)
            {
                _tokens -= 1;
                return true;
            }
            return false;
        }

        private void RefillTokens()
        {
            var now = DateTime.UtcNow;
            var elapsed = (now - _lastRefill).TotalSeconds;
            _tokens = Math.Min(_burstSize, _tokens + elapsed * _tokensPerSecond);
            _lastRefill = now;
        }
    }
} 