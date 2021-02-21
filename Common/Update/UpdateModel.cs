using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Update
{
    public class UpdateModel
    {
        public bool Published { get; set; }
        public string ProductId { get; set; }
        public string TenantId { get; set; }
        public string Name { get; set; }
        public string Channel { get; set; }
        public Version Version { get; set; }
        public string Platform { get; set; }
        public string Notes { get; set; }
        public int TotalFiles { get; set; }
        public List<UploadFile> Files { get; set; }
        public string Id { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
    public class UploadFile
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public int Size { get; set; }
        public int Downloads { get; set; }
        public string Extension { get; set; }
        public string Checksum { get; set; }
        public bool Secured { get; set; }
        public string ReleaseId { get; set; }
        public string Id { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
