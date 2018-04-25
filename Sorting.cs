using System.Collections.Generic;

namespace AutoComplete
{
    class Sorting
    {
        public static void bubbleSort(List<Query> lq)
        {
            for (int i = 0; i < lq.Count - 1; i++)
            {
                for (int j = 0; j < lq.Count - 1; j++)
                {
                    if (lq[j].weight < lq[j + 1].weight)
                    {
                        Query tmp = lq[j];
                        lq[j] = lq[j + 1];
                        lq[j + 1] = tmp;
                    }
                }
            }
        }
        public static List<Query> mergeSort(List<Query> q)
        {
            List<Query> left = new List<Query>();
            List<Query> right = new List<Query>();
            List<Query> result = new List<Query>();
            if (q.Count <= 1)
                return q;
            int mid = q.Count / 2;
            for (int i = 0; i < mid; i++)
            {
                left.Add(q[i]);
            }
            for (int i = mid; i < q.Count; i++)
            {
                right.Add(q[i]);
            }
            left = mergeSort(left);
            right = mergeSort(right);
            result = merge(left, right);
            return result;
        }
        private static List<Query> merge(List<Query> l, List<Query> r)
        {
            List<Query> res = new List<Query>();
            int li = 0;
            int ri = 0;
            while (l.Count > li && r.Count > ri)
            {
                if (l[li].weight > r[ri].weight)
                {
                    res.Add(l[li]);
                    li++;
                }
                else
                {
                    res.Add(r[ri]);
                    ri++;
                }
            }
            while (l.Count > li)
            {
                res.Add(l[li]);
                li++;
            }
            while (r.Count > ri)
            {
                res.Add(r[ri]);
                ri++;
            }
            return res;
        }
    }
}
