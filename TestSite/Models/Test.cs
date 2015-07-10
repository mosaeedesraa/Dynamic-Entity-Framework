using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSite.Models
{
   public class Test
    {
       /// <summary>
       /// We will do properties for table's columns (same name)
       /// </summary>
        public Int32 ID { get; set; }
        public string Name { get; set; }
        public Int32 Age { get; set; }
        public string Address { get; set; }
    }
}
