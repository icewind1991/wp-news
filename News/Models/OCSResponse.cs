﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Models
{
    public class OCSResponse
    {
        public OCSResponse()
        {
            ocs = new OCSBlock();
        }
        public OCSBlock ocs { get; set; }
    }
}
