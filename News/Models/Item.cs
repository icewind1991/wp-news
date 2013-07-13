using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Models
{
    public class Item
    {
        public int id { get; set; }
        public string guid { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string pubDate { get; set; }
        public string body { get; set; }
        public string enclosureMime { get; set; }
        public string enclosureLink { get; set; }
        public int feedId { get; set; }
        public bool unread { get; set; }
        public bool starred { get; set; }
        public int lastModified { get; set; }
    }
}
