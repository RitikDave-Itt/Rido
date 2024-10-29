using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Data.Entities
{
    public class Image
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FileNAme { get; set; }
        public string Base64String { get; set; }

        public string FileType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    }
}
