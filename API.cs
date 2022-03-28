using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokédex
{
    class API
    {
        public int id { get; set; }
        public string url { get; set; }
        public int lastEdit { get; set; }

        public override string ToString()
        {
            return "id: " + this.id +
                "\nurl: " + this.url +
                "\nlastEdit: " + this.lastEdit;
        }
    }
}
