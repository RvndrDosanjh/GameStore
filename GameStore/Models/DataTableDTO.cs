using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models
{
    public class DataTableDTO
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string ReleaseDate { get; set; }
        public string Category { get; set; }
        public string Company { get; set; }
        public int DownloadCount { get; set; }
        public int Age { get; set; }
    }
}
