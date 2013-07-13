using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Models
{
    public class OCSFolderResponse : OCSResponse
    {
        public OCSFolderResponse() : base()
        {
            ocs = new OCSFolderBlock();
        }
        new public OCSFolderBlock ocs { get; set; }
    }
}
