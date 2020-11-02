using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace L05
{
    class Program
    {
        private static string _connectionString;
        private static CloudTableClient _tableClient;
        private static CloudTable _studentsTable;
        private static CloudTable _metricsTable;
        static void Main(string[] args)
        {
            _connectionString = "DefaultEndpointsProtocol=https;AccountName=l04;AccountKey=cU9KsT5L/IxsOe0dgjAzjQ7i1fxbRm5BccZJ7QxcTux01GMzeJ+HkLo54gNYo8g7DogQZb50I2LGoAdCSwdXtA==;EndpointSuffix=core.windows.net";

            Task.Run(async () => { await InitializeTables(); })
                .GetAwaiter()
                .GetResult();

            Task.Run(async () => { await ComputeMetrics(); })
                .GetAwaiter()
                .GetResult();
                
        }
        public static async Task ComputeMetrics()
        {
            List<StudentEntity> students = await GetAllStudents();

            int countUPT = 0;
            int countUVT = 0;
            int countGeneral = 0;

            foreach (var student in students)
            {
                if(student.PartitionKey == "UPT")
                    countUPT++;
                else if(student.PartitionKey == "UVT")
                    countUVT++;
            }
            countGeneral = countUPT + countUVT;

            var metricUPT = new MetricEntity("UPT");
            var metricUVT = new MetricEntity("UVT");
            var metricGeneral = new MetricEntity("General");

            metricUPT.Count = countUPT;
            metricUVT.Count = countUVT;
            metricGeneral.Count = countGeneral;
            
            var insertUPT = TableOperation.Insert(metricUPT);
            await _metricsTable.ExecuteAsync(insertUPT);
            var insertUVT = TableOperation.Insert(metricUVT);
            await _metricsTable.ExecuteAsync(insertUVT);
            var insertGeneral = TableOperation.Insert(metricGeneral);
            await _metricsTable.ExecuteAsync(insertGeneral);
        }
        public static async Task<List<StudentEntity>> GetAllStudents()
        {
            var students = new List<StudentEntity>();

            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<StudentEntity> resultSegment = await _studentsTable.ExecuteQuerySegmentedAsync(query, token);

                students.AddRange(resultSegment.Results);
            } while (token != null);

            return students;
        }
        static async Task InitializeTables()
        {

            var account = CloudStorageAccount.Parse(_connectionString);
            _tableClient = account.CreateCloudTableClient();

            _studentsTable = _tableClient.GetTableReference("students");
            _metricsTable = _tableClient.GetTableReference("metrics");

            await _studentsTable.CreateIfNotExistsAsync();
            await _metricsTable.CreateIfNotExistsAsync();
        }

        
    }
}
