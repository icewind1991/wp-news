﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Models
{
    public class OCSFolderBlock : OCSBlock
    {
        public OCSFolderBlock()
        {
            meta = new OCSMeta();
            data = new OCSFolderData();
        }
        new public OCSFolderData data { get; set; }
    }
}