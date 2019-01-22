using System.IO;
using System.Threading.Tasks;
using HelloServerless.DTO;
using HelloServerless.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HelloServerless
{
    public static class MakeApplication
    {
        [FunctionName("MakeApplication")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Queue("loan-applications")]IAsyncCollector<LoanApplicationReceived> messageQueue,
            ILogger log)
        {
            log.LogInformation($"{nameof(MakeApplication)} started.");

            var readDataOperation = await GetDataFromRequestBodyAsync<LoanApplicationRequest>(req).ConfigureAwait(false);
            if (!readDataOperation.Status)
            {
                log.LogInformation("Invalid loan application received");
                return new BadRequestObjectResult("Please pass a valid request");
            }

            var loanApplication = readDataOperation.Data;
            //
            // Send the loan application request to the queue
            //
            var loanApplicationReceivedEvent = new LoanApplicationReceived
            {
                Name = loanApplication.Name,
                Age = loanApplication.Age
            };

            await messageQueue.AddAsync(loanApplicationReceivedEvent).ConfigureAwait(false);

            return new OkObjectResult($"Hi {loanApplication.Name}! Your loan application is in progress.");
        }

        public static async Task<ResultStatus<TData>> GetDataFromRequestBodyAsync<TData>(HttpRequest request) where TData : class
        {
            if (request == null || request.Body == null)
            {
                return ResultStatus<TData>.Error();
            }

            try
            {
                var content = string.Empty;
                using (var streamReader = new StreamReader(request.Body))
                {
                    content = await streamReader.ReadToEndAsync().ConfigureAwait(false);
                }

                if (string.IsNullOrWhiteSpace(content))
                {
                    return ResultStatus<TData>.Error();
                }

                var data = JsonConvert.DeserializeObject<TData>(content);

                return ResultStatus<TData>.Success(data);
            }
            catch
            {
                return ResultStatus<TData>.Error();
            }
        }
    }
}
