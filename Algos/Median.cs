using System;
using System.Collections.Generic;
using System.Linq;

namespace LeetSolutions.Algos.Median
{
    public class MedianFinder
    {

        /** initialize your data structure here. */
        private SortedDictionary<int, int> maxheap;
        private SortedDictionary<int, int> minheap;
        private int minhsize;
        private int maxhsize;
        public MedianFinder()
        {
            maxheap = new SortedDictionary<int, int>();
            minheap = new SortedDictionary<int, int>();
            minhsize = 0;
            maxhsize = 0;
        }

        public void AddNum(int num)
        {
            if (maxhsize > minhsize)
            {
                if (num > maxheap.Last().Key)
                {
                    Push(minheap, num);
                }
                else
                {
                    var maxTop = PopFromMaxHeap();
                    Push(minheap, maxTop);
                    Push(maxheap, num);

                }
                minhsize++;
            }
            else
            {
                if (minhsize != 0 && num > minheap.First().Key)
                {
                    var minTop = PopFromMinHeap();
                    Push(maxheap, minTop);
                    Push(minheap, num);
                }
                else
                {
                    Push(maxheap, num);
                }
                maxhsize++;
            }
        }

        public double FindMedian()
        {
            //length is even
            if (minhsize == maxhsize) return (double)(minheap.First().Key + maxheap.Last().Key) / 2;
            //length is odd
            if (minhsize > maxhsize) return minheap.First().Key;
            else return maxheap.Last().Key;

        }

        public void Push(SortedDictionary<int, int> heap, int num)
        {
            if (!heap.TryAdd(num, 1))
                heap[num]++;
        }

        public int PopFromMaxHeap()
        {
            int ret = maxheap.Last().Key;
            if (maxheap[ret] > 1) maxheap[ret]--;
            else maxheap.Remove(ret);
            return ret;
        }

        public int PopFromMinHeap()
        {
            int ret = minheap.First().Key;
            if (minheap[ret] > 1) minheap[ret]--;
            else minheap.Remove(ret);
            return ret;
        }

    }
}
