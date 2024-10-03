using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TestFunc
{
    public class Function1(ILogger<Function1> logger)
    {
        private readonly ILogger<Function1> _logger = logger;

        [Function("Function1")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req, [FromQuery]string foo = "21", [FromQuery] LogLevel bar = LogLevel.None)
        {
            if (!req.Query.TryGet<long>("bar", out var bar2))
            {
                return new BadRequestResult();  
            }

            _logger.LogInformation($"C# HTTP trigger function processed a request. {foo}");
            _logger.LogInformation(req.QueryString.ToString());


            return new OkObjectResult($"Welcome to Azure Functions! {foo} {bar} {bar2}");
        }
    }

    public static class QueryExtensions {
        public static bool TryGet<T>(this IQueryCollection query, string name, out T bar)
        {
            bar = default;

            var stringValue = query[name];
            if (!string.IsNullOrEmpty(stringValue)) 
            { 
                bar = (T)Convert.ChangeType(stringValue, typeof(T));
                return bar != null;
            }

            return false;
        }
    }
}
