using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Models
{
    class OCSItemBlock : OCSBlock
    {
        public OCSItemBlock()
        {
            meta = new OCSMeta();
            data = new OCSItemData();
        }
        new public OCSItemData data { get; set; }
    }
}
