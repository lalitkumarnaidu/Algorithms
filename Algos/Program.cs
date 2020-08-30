using System;
using System.Collections.Generic;
using System.Linq;

namespace LeetSolutions.Algos
{
    class Program
    {
        static void Main(string[] args)
        {
            //var input = new int[]{1,2,3,4,5,6,7,8};
            //var result = Solution.GetListOfPermutationsFor(input);
            var input = new int[]{ 5, 6, 7, 7, 1, 1, 1, 3, 3, 1, 7, 5,7,6, 1,7,7,5,5,6,1,1,1,1,1 };
            // foreach(var list in result){
            //     Print(list);
            // }
            //Print(Solution.TopKElementsByFreq(input, 2));

            //Print(TopKSort(new List<int>(input), 2));
            Solution.FindWordsinSentences();
        }

        static void Print(List<int> l){
            Console.WriteLine(string.Join(",", l));
        }
        static void Print(int[] l)
        {
            Console.WriteLine(string.Join(",", l));
        }

        static int[] TopKSort(List<int> arr, int k)
        {
            int[] r = new int[k];
            Dictionary<int, int> freq = new Dictionary<int, int>();
            foreach(var num in arr)
            {
                if(!freq.TryAdd(num, 1))
                {
                    freq[num]++;
                }
            }
            var m = freq.OrderByDescending(o => o.Value).ToList();
            int i = k -1;
            foreach(var kv in m)
            {
                r[i] = kv.Key;
                i--;
                if(i < 0)
                {
                    break;
                }
            }
            return r;
        } 
    }
}
