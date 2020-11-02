using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace L05
{
    public class MetricEntity : TableEntity
    {
        public MetricEntity(string universitate)
        {
            this.PartitionKey = universitate;
            this.RowKey = DateTime.Now.ToString().Replace("/", ".");
        }
        public MetricEntity() { }
        public int Count { get; set; }


    }
}