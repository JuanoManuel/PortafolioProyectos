using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    class Graph
    {
        private Dictionary<ulong, List<ulong>> nodes;

        public Graph(Dictionary<ulong, List<ulong>> init_nodes)
        {
            nodes = new Dictionary<ulong, List<ulong>>(init_nodes);
        }
        public string getKey()
        {

            Dictionary<ulong, ulong> pre_key = new Dictionary<ulong, ulong>();
            foreach (KeyValuePair<ulong, List<ulong>> item in nodes)
            {

                if (!pre_key.ContainsKey(item.Key))
                    pre_key.Add(item.Key, 1);

                for (int i = 0; i < item.Value.Count; i++)
                {
                    if (pre_key.ContainsKey(item.Value[i]))
                    {
                        pre_key[item.Value[i]]++;
                    }
                    else
                    {
                        pre_key.Add(item.Value[i], 1);
                    }
                }
            }

            string key = "";
            foreach (KeyValuePair<ulong, ulong> item in pre_key)
            {
                key += item.Value + ((pre_key.Last().Key == item.Key) ? "" : ",");
            }
            return key;
        }
        public Dictionary<ulong, List<ulong>> getAllNodes()
        {
            return nodes;
        }
    }
}
