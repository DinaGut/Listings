using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ItemOffered
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PhoneNumber { get; set; }
        public DateTime DatePosted { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }

    }

}