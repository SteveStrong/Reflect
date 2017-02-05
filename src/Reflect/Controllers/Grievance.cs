using System;

namespace Reflect.Controllers
{

    public class Testing
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

    public interface IDocument
    {
         string Id { get; set; }
         string Name { get; set; }
         string Version { get; set; }
         string Status { get; set; } 
         string Document { get; set; }
         DateTime DateOfLastChange { get; set; }

         string LastVersion { get; set; }
         string NextVersion { get; set; }

    }

    public class Grievance : IDocument
    {
        public string Id { get; set; } = "UNKNOWN";
        public string Name { get; set; } = "UNKNOWN";
        public string Version { get; set; } = "UNKNOWN";
        public string Status { get; set; } = "unknown";
        public string Document { get; set; } = "{}";
        public DateTime DateFiled { get; set; }
        public DateTime DateOfOccurrence { get; set; }
        public DateTime DateOfLastChange { get; set; }

        public string LastVersion { get; set; } = "UNKNOWN";
        public string NextVersion { get; set; } = "UNKNOWN";

        public string Center { get; set; } = "UNKNOWN";

    }
}
