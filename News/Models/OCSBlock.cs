using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Models
{
    public class OCSBlock
    {
        public OCSBlock()
        {
            meta = new OCSMeta();
            data = new OCSData();
        }
        public OCSMeta meta { get; set; }
        public OCSData data { get; set; }
    }
}
