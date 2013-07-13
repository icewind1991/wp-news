using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Models
{
    class OCSItemResponse: OCSResponse
    {
        public OCSItemResponse()
            : base()
        {
            ocs = new OCSItemBlock();
        }
        new public OCSItemBlock ocs { get; set; }
    }
}
