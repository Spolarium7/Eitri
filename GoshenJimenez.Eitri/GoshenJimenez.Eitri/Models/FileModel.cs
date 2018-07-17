using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoshenJimenez.Eitri.Models
{
    public class FileModel
    {
        public string FileData { get; set; }

        public string FileType { get; set; }

        public long FileSize { get; set; }

        public string LastModifiedDate { get; set; }

        public string FileName { get; set; }
    }
}
