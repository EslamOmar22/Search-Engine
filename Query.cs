using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoComplete
{
    class Query
    {
        public string query;
        public int index;
        public long weight;
        public Query()
        {
            query = string.Empty;
            weight = 0;
            index = 0;
        }
        public Query(string q, int weight)
        {
            query = q;
            this.weight = weight;
        }
    }
}
