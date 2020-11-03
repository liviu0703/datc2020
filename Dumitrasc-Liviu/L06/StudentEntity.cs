using Microsoft.WindowsAzure.Storage.Table;

namespace L06
{
    public class StudentEntity : TableEntity
    {
        public StudentEntity(string universitate, string cnp)
        {
            this.PartitionKey = universitate;
            this.RowKey = cnp;
        }
        public StudentEntity() { }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public int AnStudiu { get; set; }
        public string Facultate { get; set; }
        public int Id { get; set; }
    }
}