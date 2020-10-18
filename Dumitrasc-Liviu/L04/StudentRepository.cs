using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace L04
{
    public class StudentRepository : IStudentRepository
    {

        private string _connectionString;
        private CloudTableClient _tableClient;

        private CloudTable _studentsTable;

        public StudentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("AzureStorageConnectionString");

            Task.Run(async () => { await InitializeTable(); })
                .GetAwaiter()
                .GetResult();
        }
        public async Task CreateNewStudent(StudentEntity student)
        {
            var insertOperation = TableOperation.Insert(student);

            await _studentsTable.ExecuteAsync(insertOperation);
        }

        public async Task<List<StudentEntity>> GetAllStudents()
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

        public async Task EditStudent(StudentEntity student)
        {
            var editOperation = TableOperation.Merge(student);

            await _studentsTable.ExecuteAsync(editOperation);
        }

        public async Task DeleteStudent(StudentEntity student)
        {
            var editOperation = TableOperation.Delete(student);

            await _studentsTable.ExecuteAsync(editOperation);
        }

        private async Task InitializeTable()
        {

            var account = CloudStorageAccount.Parse(_connectionString);
            _tableClient = account.CreateCloudTableClient();

            _studentsTable = _tableClient.GetTableReference("students");

            await _studentsTable.CreateIfNotExistsAsync();
        }
    }
}