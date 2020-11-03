using System;
using L06;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
    public static class QueueTrigger_L06
    {
        [FunctionName("QueueTrigger_L06")]
        [return: Table("students")]
        public static StudentEntity Run([QueueTrigger("students-queue", Connection = "storageaccountdatc28201_STORAGE")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var student = JsonConvert.DeserializeObject<StudentEntity>(myQueueItem);

            return student;
        }
    }
}
