using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class ErrorModel
    {
        public string FieldName { get; set; }
        public string Message { get; set; }
    }

    public class ErrorModelResponse
    {
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}
