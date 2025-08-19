using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Admin.DocumentType
{
    public class DocumentTypeDto
    {
        public int Id { get; set; }
        public string DocumentTypeName { get; set; }
        public int UserTypeId { get; set; }
    }
}
