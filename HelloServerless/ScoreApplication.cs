using System;
using HelloServerless.Events;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HelloServerless
{
    public static class ScoreApplication
    {
        [FunctionName("ScoreApplication")]
        public static void Run([QueueTrigger("loan-applications", Connection = "")]LoanApplicationReceived request,
            [Blob("accepted-applications/{rand-guid}")]out string acceptedApplication,
            [Blob("rejected-applications/{rand-guid}")]out string rejectedApplication,
            ILogger log)
        {
            acceptedApplication = null;
            rejectedApplication = null;

            log.LogInformation($"{nameof(ScoreApplication)} function started.");

            var isValid = !string.IsNullOrWhiteSpace(request.Name) && request.Age >= 18;

            if (isValid)
            {
                log.LogInformation($"{request.Name} application is valid. Proceed with the loan");
                acceptedApplication = JsonConvert.SerializeObject(new LoanApplicationAccepted
                {
                    Name = request.Name,
                    AcceptedDate = DateTime.UtcNow
                });
            }
            else
            {
                var reason = $"{request.Name} application is invalid. The person seemed to be under aged or do not have a name!";
                log.LogInformation(reason);
                rejectedApplication = JsonConvert.SerializeObject(new LoanApplicationRejected
                {
                    Name = request.Name,
                    Reason = reason
                });
            }
        }
    }
}
