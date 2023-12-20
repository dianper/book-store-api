using Prometheus;

namespace Presentation.Middlewares
{
    public class MetricsMiddleware
    {
        private readonly RequestDelegate next;
        private readonly Counter requestsCounter;

        public MetricsMiddleware(RequestDelegate next)
        {
            this.next = next;
            this.requestsCounter = Metrics.CreateCounter("api_books_requests_total", "Total number of requests to the Book Store API");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments(new PathString("/api")))
            {
                this.requestsCounter.Inc();
            }

            await this.next(context);
        }
    }
}
