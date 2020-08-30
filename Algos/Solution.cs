using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;
using System.Xml.Linq;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using LeetSolutions.Algos;
using System.Text;
using static LeetSolutions.Algos.Solution;
using System.Security.Cryptography;

namespace LeetSolutions.Algos{
    public static partial class Solution
    {
        /*
            Compute all distict permutations of an array of distict integers
            Example: [1,2,3] --> [[1,2,3],[1,3,2],[2,1,3],[3,1,2],[2,3,1],[3,2,1]]
        */
        public static List<List<int>> GetListOfPermutationsFor(int[] ip)
        {
            var retval = new List<List<int>>();
            var set = new List<int>();
            permute(set, ref retval, new List<int>(ip));
            return retval;
        }

        private static void permute(List<int> set, ref List<List<int>> result, List<int> suffix)
        {
            if (suffix.Count == 0)
            {
                result.Add(set);
                return;
            }
            for (int i = 0; i < suffix.Count; i++)
            {
                var newSet = new List<int>(set);
                newSet.Add(suffix[i]);
                var newSuffix = new List<int>(suffix);
                newSuffix.RemoveAt(i);
                permute(newSet, ref result, newSuffix);
            }

        }

        /*
            Return top k frequently occuring elements in ascending order of freq
            example: given [5, 6, 7, 7, 1, 1, 1, 3, 3, 1, 7], K = 2
            o/p: [7, 1] explanation: 7  occurs three times, 1 occurs four times
        */
        public static int[] TopKElementsByFreq(int[] nums, int k)
        {
            //freq bucket for top k, this cannot be more than the no. of elements in the array.
            //i.e. maximum frequency of an element in the array is not greater than the size of the input array
            var freq = new Dictionary<int, int>();

            for (int q = 0; q < nums.Length; q++)
            {
                if (!freq.TryAdd(nums[q], 1))
                {
                    freq[nums[q]]++;
                }
            }

            //buckets starts from index 1 as we will atleast have one item in nums, 
            //thus bucket length is 1 more than length of nums.
            //here bucket index is the freq of items in nums i.e. value of each key in freq.
            List<int>[] buckets = new List<int>[nums.Length + 1];
            foreach (var key in freq.Keys)
            {
                var v = freq[key];
                if (buckets[v] != null) buckets[v].Add(key);
                else buckets[v] = new List<int> { key };
            }
            int i = buckets.Length - 1;
            int[] topK = new int[k];
            int p = 0;
            while (i > 0 && p < k)
            {
                int j = 0;
                while (buckets[i] != null && j < buckets[i].Count && p < k)
                {
                    topK[p] = buckets[i][j];
                    j++;
                    p++;
                }
                i--;
            }
            return topK;
        }
        



        public static void FindWordsinSentences()
        {
            string text = @"Historically, the world of data and the world of objects " +
        @"have not been well integrated. Programmers work in C# or Visual Basic " +
        @"and also in SQL or XQuery. On the one side are concepts such as classes, " +
        @"objects, fields, inheritance, and .NET Framework APIs. On the other side " +
        @"are tables, columns, rows, nodes, and separate languages for dealing with " +
        @"them. Data types often require translation between the two worlds; there are " +
        @"different standard functions. Because the object world has no notion of query, a " +
        @"query can only be represented as a string without compile-time type checking or " +
        @"IntelliSense support in the IDE. Transferring data from SQL tables or XML trees to " +
        @"objects in memory is often tedious and error-prone.";

            // Split the text block into an array of sentences.  
            string[] sentences = text.Split(new char[] { '.', '?', '!' });

            // Define the search terms. This list could also be dynamically populated at runtime.  
            string[] wordsToMatch = { "Historically", "data", "integrated" };

            // Find sentences that contain all the terms in the wordsToMatch array.  
            // Note that the number of terms to match is not specified at compile time.  
            var sentenceQuery = from sentence in sentences
                                let w = sentence.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' },
                                                        StringSplitOptions.RemoveEmptyEntries)
                                where w.Distinct().Intersect(wordsToMatch).Count() == wordsToMatch.Count()
                                select sentence;

            // Execute the query. Note that you can explicitly type  
            // the iteration variable here even though sentenceQuery  
            // was implicitly typed.
            foreach (string str in sentenceQuery)
            {
                Console.WriteLine(str);
            }

            // Keep the console window open in debug mode.  
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        /*
Given a list of reviews, a list of keywords and an integer k. Find the most popular k keywords in order of most to least frequently mentioned.

The comparison of strings is case-insensitive.
Multiple occurances of a keyword in a review should be considred as a single mention.
If keywords are mentioned an equal number of times in reviews, sort alphabetically.

Example 1:

Input:
k = 2
keywords = ["anacell", "cetracular", "betacellular"]
reviews = [
  "Anacell provides the best services in the city",
  "betacellular has awesome services",
  "Best services provided by anacell, everyone should use anacell",
]

Output:
["anacell", "betacellular"]

Explanation:
"anacell" is occuring in 2 different reviews and "betacellular" is only occuring in 1 review.
Example 2:

Input:
k = 2
keywords = ["anacell", "betacellular", "cetracular", "deltacellular", "eurocell"]
reviews = [
  "I love anacell Best services; Best services provided by anacell",
  "betacellular has great services",
  "deltacellular provides much better services than betacellular",
  "cetracular is worse than anacell",
  "Betacellular is better than deltacellular.",
]

Output:
["betacellular", "anacell"]

Explanation:
"betacellular" is occuring in 3 different reviews. "anacell" and "deltacellular" are occuring in 2 reviews, but "anacell" is lexicographically smaller.
**/

        public static string[] TopKFrequentWords(List<string> reviews, List<string> keywords, int k)
        {
            var keywordsDict = keywords.ToDictionary(o => o, o => 0);
            foreach (var review in reviews)
            {
                UpdateWordFreq(review, ref keywordsDict);
            }

            return GetTopKWordsByFreq(keywordsDict, k, reviews.Count);
        }

        private static void UpdateWordFreq(string review, ref Dictionary<string, int> keywords)
        {
            var words = review.Split(new char[] { ',', '.', ';', '!', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words.Distinct())
            {
                var cword = word.ToLower();
                if (keywords.ContainsKey(cword))
                {
                    keywords[cword]++;
                }
            }
        }

        private static string[] GetTopKWordsByFreq(Dictionary<string, int> freqs, int k, int maxFreqSize)
        {
            List<string>[] freqBuckets = new List<string>[maxFreqSize + 1];
            foreach (var w in freqs.Keys)
            {
                if (freqBuckets[freqs[w]] == null)
                {
                    freqBuckets[freqs[w]] = new List<string>();
                }
                freqBuckets[freqs[w]].Add(w);
            }

            string[] topKwords = new string[k];
            k--;
            int i = maxFreqSize;
            while (k >= 0)
            {
                if (freqBuckets[i] != null)
                {
                    int j = freqBuckets[i].Count - 1;
                    while (j >= 0)
                    {
                        topKwords[k] = freqBuckets[i][j];
                        j--;
                        k--;

                        if (k < 0)
                        {
                            break;
                        }
                    }
                }
                i--;
            }
            return topKwords;
        }
        /*
            Determine if a 9x9 Sudoku board is valid. Only the filled cells need to be validated according to the following rules:

            Each row must contain the digits 1-9 without repetition.
            Each column must contain the digits 1-9 without repetition.
            Each of the 9 3x3 sub-boxes of the grid must contain the digits 1-9 without repetition

            Input:
                [
                  ["5","3",".",".","7",".",".",".","."],
                  ["6",".",".","1","9","5",".",".","."],
                  [".","9","8",".",".",".",".","6","."],
                  ["8",".",".",".","6",".",".",".","3"],
                  ["4",".",".","8",".","3",".",".","1"],
                  ["7",".",".",".","2",".",".",".","6"],
                  [".","6",".",".",".",".","2","8","."],
                  [".",".",".","4","1","9",".",".","5"],
                  [".",".",".",".","8",".",".","7","9"]
                ]
            Output: true

            Input:
                [
                  ["8","3",".",".","7",".",".",".","."],
                  ["6",".",".","1","9","5",".",".","."],
                  [".","9","8",".",".",".",".","6","."],
                  ["8",".",".",".","6",".",".",".","3"],
                  ["4",".",".","8",".","3",".",".","1"],
                  ["7",".",".",".","2",".",".",".","6"],
                  [".","6",".",".",".",".","2","8","."],
                  [".",".",".","4","1","9",".",".","5"],
                  [".",".",".",".","8",".",".","7","9"]
                ]
            Output: false
            Explanation: Same as Example 1, except with the 5 in the top left corner being 
                modified to 8. Since there are two 8's in the top left 3x3 sub-box, it is invalid.
        */
        public static bool IsValidSudoku(char[,] board)
        {
            HashSet<int>[] col = new HashSet<int>[9];
            HashSet<int>[] row = new HashSet<int>[9];
            HashSet<int>[] unit = new HashSet<int>[9];
            bool retval = true;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    // this formula works as we are dividing ints
                    // so any fraction or decimal part will get truncated
                    char valChar = board[i, j];
                    if (valChar != '.')
                    {
                        var val = (int)Char.GetNumericValue(valChar);
                        int unitIdx = ((i / 3) * 3 + j / 3);
                        retval = IsBoardUnitValid(ref col, j, val) &&
                        IsBoardUnitValid(ref row, i, val) &&
                        IsBoardUnitValid(ref unit, unitIdx, val);
                        //if validation fails return immediately i.e. retval is false
                        if (!retval) return retval;
                    }
                }
            }
            return retval;
        }

        private static bool IsBoardUnitValid(ref HashSet<int>[] boardUnit, int idx, int val)
        {
            if (boardUnit[idx] == null)
            {
                boardUnit[idx] = new HashSet<int> { val };
            }
            else
            {
                if (boardUnit[idx].Contains(val))
                {
                    return false;
                }
                boardUnit[idx].Add(val);
            }
            return true;
        }

        /********************************************************************************
        You are given a m x n 2D grid initialized with these three possible values.

        -1 - A wall or an obstacle.
        0 - A gate.
        INF - Infinity means an empty room. We use the value 231 - 1 = 2147483647 to represent INF as you may assume that the distance to a gate is less than 2147483647.
        Fill each empty room with the distance to its nearest gate. If it is impossible to reach a gate, it should be filled with INF.

        Example: 

        Given the 2D grid:

        INF  -1  0  INF
        INF INF INF  -1
        INF  -1 INF  -1
        0  -1 INF INF
        After running your function, the 2D grid should be:

        3  -1   0   1
        2   2   1  -1
        1  -1   2  -1
        0  -1   3   4

        ***********************************************************************************/
        public static void WallsAndGates(int[][] rooms)
        {
            for (int i = 0; i < rooms.Length; i++)
            {
                for (int j = 0; j < rooms[i].Length; j++)
                {
                    if (rooms[i][j] == 0)
                    {
                        UpdateRooms(rooms, i, j, 0);
                    }
                }
            }
        }

        private static void UpdateRooms(int[][] rooms, int i, int j, int hops)
        {
            if (i >= rooms.Length || i < 0 || j >= rooms[i].Length || j < 0 || rooms[i][j] < hops)
            {
                return;
            }

            rooms[i][j] = hops;
            UpdateRooms(rooms, i + 1, j, hops + 1);
            UpdateRooms(rooms, i - 1, j, hops + 1);
            UpdateRooms(rooms, i, j + 1, hops + 1);
            UpdateRooms(rooms, i, j - 1, hops + 1);
        }
        /************************************************************************************************************************
                Given a 2D grid, each cell is either a zombie 1 or a human 0. Zombies can turn adjacent (up/down/left/right)
                human beings into zombies every hour. Find out how many hours does it take to infect all humans?
                https://leetcode.com/problems/rotting-oranges/
                https://leetcode.com/problems/walls-and-gates/ (premium)


                Example:

                    Input:
                    [[0, 1, 1, 0, 1],
                    [0, 1, 0, 1, 0],
                    [0, 0, 0, 0, 1],
                    [0, 1, 0, 0, 0]]

                    Output: 2

                    Explanation:
                    At the end of the 1st hour, the status of the grid:
                    [[1, 1, 1, 1, 1],
                    [1, 1, 1, 1, 1],
                    [0, 1, 0, 1, 1],
                    [1, 1, 1, 0, 1]]

                    At the end of the 2nd hour, the status of the grid:
                    [[1, 1, 1, 1, 1],
                    [1, 1, 1, 1, 1],
                    [1, 1, 1, 1, 1],
                    [1, 1, 1, 1, 1]]
       **************************************************************************************************************************/
        public static int MinHours(int[,] grid)
        {
            HashSet<string> zombies = new HashSet<string>();
            HashSet<string> humans = new HashSet<string>();

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    var idx = $"{i}{j}";
                    if (grid[i, j] == 0)
                    {
                        humans.Add(idx);
                    }
                    else
                    {
                        zombies.Add(idx);
                    }
                }
            }
            return Zombify(grid, humans, zombies);
        }

        private static int Zombify(int[,] grid, HashSet<string> humans, HashSet<string> zombies)
        {
            int thrs = 0;
            int[,] direction = new int[,] { { 0, 1 }, { 1, 0 }, { -1, 0 }, { 0, -1 } };
            while (humans.Count > 0)
            {
                HashSet<string> infected = new HashSet<string>();
                foreach (var zombie in zombies)
                {
                    for (int d = 0; d < direction.GetLength(0); d++)
                    {
                        int i = (zombie[0] - '0') + direction[d, 0];
                        int j = (zombie[1] - '0') + direction[d, 1];
                        if (humans.Contains($"{i}{j}"))
                        {
                            infected.Add($"{i}{j}");
                            humans.Remove($"{i}{j}");
                        }

                    }
                }
                if (infected.Count == 0)
                {
                    return thrs;
                }
                zombies = infected;
                thrs++;
            }
            return thrs;
        }
        /*
            In a given grid, each cell can have one of three values:

            the value 0 representing an empty cell;
            the value 1 representing a fresh orange;
            the value 2 representing a rotten orange.
            Every minute, any fresh orange that is adjacent (4-directionally) to a rotten orange becomes rotten.

            Return the minimum number of minutes that must elapse until no cell has a fresh orange. 
             If this is impossible, return -1 instead.
            
            Example 1:

            Input: [[2,1,1],
                    [1,1,0],
                    [0,1,1]]
            Output: 4
            Example 2:

            Input: [[2,1,1],
                    [0,1,1],
                    [1,0,1]]
            Output: -1
            Explanation:  The orange in the bottom left corner (row 2, column 0) is never rotten, because rotting only happens 4-directionally.
            Example 3:

            Input: [[0,2]]
            Output: 0
            Explanation:  Since there are already no fresh oranges at minute 0, the answer is just 0.
            

            Note:

            1 <= grid.length <= 10
            1 <= grid[0].length <= 10
            grid[i][j] is only 0, 1, or 2.
        */

        public static int OrangesRotting(int[,] grid)
        {
            int retval = 0;
            //{"00"}
            var q = new Queue<string>();
            //{"01", "02", "10", "11", "21", "22"}
            var fresh = new HashSet<string>();
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == 2)
                        q.Enqueue($"{i}{j}");
                    else if (grid[i, j] == 1)
                        fresh.Add($"{i}{j}");
                }
            }

            var d = new int[,] { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

            while (fresh.Count > 0)
            {
                var rotting = new Queue<string>();
                while (q.Count > 0)
                {

                    var curr = q.Dequeue();
                    var i = curr[0] - '0';
                    var j = curr[1] - '0';
                    for (int k = 0; k < d.GetLength(0); k++)
                    {
                        var di = i + d[k, 0];
                        var dj = j + d[k, 1];
                        if (di < 0 || di >= grid.GetLength(0) || dj < 0 || dj >= grid.GetLength(1) || grid[di, dj] == 0)
                        {
                            continue;
                        }
                        if (fresh.Contains($"{di}{dj}"))
                        {
                            fresh.Remove($"{di}{dj}");
                            grid[di, dj] = 2;
                            rotting.Enqueue($"{di}{dj}");
                        }


                    } 
                }
                if (rotting.Count == 0)
                {
                    break;
                }
                retval++;//2
                q = rotting;
            }
            return fresh.Count > 0 ? -1 : retval;
        }

        // public static int OrangesRottingWithoutQ(int[][] grid)
        // {
        //     var retval = 0;
        //     var freshOranges = 0;

        //     for(int i = 0; i < grid.Length; i++)
        //     {
        //         for(int j =0; j < grid[1].Length; j++)
        //         {
        //             if(grid[i][j] == 1) freshOranges++;
        //         }
        //     }


        //     return retval;
        // }

        // private static void UpdateBasket(int[][] grid, int i, int j, int freshOranges, int count) 
        // {
        //     var d = new int[,] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
        //     if(freshOranges == 0)
        //     {
        //         return;
        //     }
        //     if(grid[i][j] >= 2)
        //     for (int k = 0; k< d.GetLength(0); k++)
        //     {
        //         var di = i + d[k, 0];
        //         var dj = j + d[k, 1];
        //         if((di > 0 || di < grid.Length) && (dj > 0 || dj < grid[i].Length))
        //         {
        //             if(grid[di][dj] == 1)
        //             {
        //                 grid[di][dj] = grid[i][j] + 1; 
        //             }
        //         }
        //     }

        // }

        /*
        Critical Router Connection
        You are given an undirected connected graph. 
        An articulation point (or cut vertex) is defined as a vertex which, 
        when removed along with associated edges, makes the graph disconnected (or more precisely, 
        increases the number of connected components in the graph). 
        The task is to find all articulation points in the given graph.

        Input:
        The input to the function/method consists of three arguments:

        numNodes, an integer representing the number of nodes in the graph.
        numEdges, an integer representing the number of edges in the graph.
        edges, the list of pair of integers - A, B representing an edge between the nodes A and B.
        Output:
        Return a list of integers representing the critical nodes.

        Example:

        Input: numNodes = 7, numEdges = 7, edges = [[0, 1], [0, 2], [1, 3], [2, 3], [2, 5], [5, 6], [3, 4]]
        Output: [2, 3, 5]

            {0} --- {1}
             |       |
            {2} --- {3} --- {4}
             |
            {5} --- {6}
            
        */

        //Brute Force - TimeComplexity O(E + V2) is check when removing a vertex other vertex are still connected. 
        public static List<int> CriticalRouter(int numNodes, int numEdges, int[,] edges)
        {
            var retval = new List<int>();
            var edgeList = new Dictionary<int, HashSet<int>>();
            if (numEdges < numNodes - 1) return retval;
            for (int i = 0; i < numEdges; i++)
            {
                //As the graph is undirected
                UpdateEdgeList(edges[i, 0], edges[i, 1], ref edgeList);
                UpdateEdgeList(edges[i, 1], edges[i, 0], ref edgeList);
            }

            //perform check for articulation point for all nodes
            for (int i = 0; i < numNodes; i++)
            {
                var q = new Queue<int>();
                q.Enqueue(edgeList[i].FirstOrDefault());
                var visited = new HashSet<int>();
                //perform bfs for the current node tested for articulation point.
                while (q.Count > 0)
                {
                    var curr = q.Dequeue();
                    if (visited.Contains(curr)) continue;
                    visited.Add(curr);
                    foreach (int n in edgeList[curr])
                    {
                        if (n != i && !visited.Contains(n))
                        {
                            q.Enqueue(n);
                        }
                    }
                }
                //All the nodes should have been visited except
                //the one which is been currently tested for articulation point
                //hence numNodes - 1 should be visited otherwise there is a
                //disconnect in the graph because of the current articulation point.
                if (visited.Count < numNodes - 1)
                {
                    retval.Add(i);
                }
            }
            return retval;
        }

        private static void UpdateEdgeList(int i, int j, ref Dictionary<int, HashSet<int>> edgeList)
        {
            if (!edgeList.ContainsKey(i))
            {
                edgeList.Add(i, new HashSet<int> { j });
            }
            else
            {
                edgeList[i].Add(j);
            }
        }

        /******************************************************************
        Given an undirected connected graph with n nodes labeled 1..n. 
        A bridge (cut edge) is defined as an edge which, when removed, 
        makes the graph disconnected (or more precisely, increases the
        number of connected components in the graph). Equivalently, 
        an edge is a bridge if and only if it is not contained in any cycle. 
        The task is to find all bridges in the given graph. 
        Output an empty list if there are no bridges.

        Input Param:
        n, an int representing the total number of nodes.
        edges, a list of pairs of integers representing the nodes connected by an edge.

        Example 1:
        Input: n = 5, edges = [[1, 2], [1, 3], [3, 4], [1, 4], [4, 5]]
        Output: [[1, 2], [4, 5]]
        Explanation:
        There are 2 bridges:
        1. Between node 1 and 2
        2. Between node 4 and 5
        If we remove these edges, then the graph will be disconnected.
        If we remove any of the remaining edges, the graph will remain connected.

        Example 2:
        Input: n = 6, edges = [[1, 2], [1, 3], [2, 3], [2, 4], [2, 5], [4, 6], [5, 6]]
        Output: []
        Explanation:
        We can remove any edge, the graph will remain connected.

        Example 3:
        Input: n = 9, edges = [[1, 2], [1, 3], [2, 3], [3, 4], [3, 6], [4, 5], [6, 7], [6, 9], [7, 8], [8, 9]]
        Output: [[3, 4], [3, 6], [4, 5]]
        ***********************************************************************/

        //Applying Tarjan's Algorithm
        /*
        
                ids[] - contains unique id for each node or vertex
                low[] - contains the lowlink values for each node or vertex.
                lowlink - is a value of a current node which is equal to the id of a node
                         reachable from the current node whose id value is lowest. 
                         N2{id: 2, low: 2} -->N3{id: 7, low: 5} --> .... -->Ni{id: i, low: 5} --> ... -->N5{id:5, low: 5} ...
                         here N3 has an id 7 which is connected to some node Ni(where i > 5)
                         which in turn is connected to a node N5 which has the lowest id connected in this chain, 
                         hence N3's low value is 5.  
        */

        public static List<List<int>> CriticalConnections(int numNodes, int numEdges, int[,] edges)
        {
            var retval = new List<List<int>>();
            var adjMatrix = new Dictionary<int, HashSet<int>>();
            if (numEdges < numNodes - 1) return retval;
            //update the adjacency matrix list
            for (int i = 0; i < numEdges; i++)
            {
                UpdateEdgeList(edges[i, 0], edges[i, 1], ref adjMatrix);
                UpdateEdgeList(edges[i, 1], edges[i, 0], ref adjMatrix);
            }
            var visited = new HashSet<int>();
            var ids = new Dictionary<int, int>();
            var lows = new int[numNodes];
            ResetSeqCount();
            CriticalConnectionsDFS(0, adjMatrix.Keys.FirstOrDefault(), -1, ids, lows, visited, adjMatrix, retval);
            return retval;
        }

        private static void CriticalConnectionsDFS(int currNodeId, int currNodeVal, int parentNodeVal, Dictionary<int, int> ids,
        int[] lows, HashSet<int> visited, Dictionary<int, HashSet<int>> adjMatrix, List<List<int>> bridges)
        {
            visited.Add(currNodeVal);
            ids.Add(currNodeVal, currNodeId);
            lows[currNodeId] = currNodeId;
            foreach (var neigbour in adjMatrix[currNodeVal])
            {
                if (neigbour == parentNodeVal) continue;
                if (!visited.Contains(neigbour))
                {
                    var nextId = GetNextInSeq();
                    CriticalConnectionsDFS(nextId, neigbour, currNodeVal, ids, lows, visited, adjMatrix, bridges);
                    lows[currNodeId] = Math.Min(lows[currNodeId], lows[nextId]);
                    if (currNodeId < lows[nextId])
                    {
                        bridges.Add(new List<int> { currNodeVal, neigbour });
                    }
                }
                else
                {
                    //this is a backedge so assign curr low to min between curr low and next id
                    lows[currNodeId] = Math.Min(lows[currNodeId], ids[neigbour]);
                }
            }
        }


        public static int[] DFSSeq(int numNodes, int numEdges, int[,] edges, int startNode)
        {
            var retval = new int[numNodes];
            var edgeList = new Dictionary<int, HashSet<int>>();
            if (numEdges < numNodes - 1) return retval;
            for (int i = 0; i < numEdges; i++)
            {
                //As the graph is undirected
                UpdateEdgeList(edges[i, 0], edges[i, 1], ref edgeList);
                UpdateEdgeList(edges[i, 1], edges[i, 0], ref edgeList);
            }
            DFSSeqRec(startNode, SeqCount, new HashSet<int>(), edgeList, retval);
            ResetSeqCount();
            return retval;
        }

        public static int SeqCount = 0;

        public static void ResetSeqCount()
        {
            SeqCount = 0;
        }
        public static int GetNextInSeq()
        {
            return ++SeqCount;
        }
        private static void DFSSeqRec(int currNode, int curNodeId, HashSet<int> visited, Dictionary<int, HashSet<int>> adjMatrix, int[] dfsSeq)
        {
            dfsSeq[curNodeId] = currNode;
            visited.Add(currNode);
            foreach (var node in adjMatrix[currNode])
            {
                if (visited.Contains(node)) continue;
                DFSSeqRec(node, GetNextInSeq(), visited, adjMatrix, dfsSeq);
            }
        }


        /*
        1268. Search Suggestions System
        Given an array of strings products and a string searchWord. 
        We want to design a system that suggests at most three product names 
        from products after each character of searchWord is typed. 
        Suggested products should have common prefix with the searchWord. 
        If there are more than three products with a common prefix return the three lexicographically minimums products.
        Return list of lists of the suggested products after each character of searchWord is typed. 

        Example 1:

            Input: products = ["mobile","mouse","moneypot","monitor","mousepad"], searchWord = "mouse"
            Output: [
            ["mobile","moneypot","monitor"],
            ["mobile","moneypot","monitor"],
            ["mouse","mousepad"],
            ["mouse","mousepad"],
            ["mouse","mousepad"]
            ]
            Explanation: products sorted lexicographically = ["mobile","moneypot","monitor","mouse","mousepad"]
            After typing m and mo all products match and we show user ["mobile","moneypot","monitor"]
            After typing mou, mous and mouse the system suggests ["mouse","mousepad"]
        Example 2:

            Input: products = ["havana"], searchWord = "havana"
            Output: [["havana"],["havana"],["havana"],["havana"],["havana"],["havana"]]
            Example 3:

            Input: products = ["bags","baggage","banner","box","cloths"], searchWord = "bags"
            Output: [["baggage","bags","banner"],["baggage","bags","banner"],["baggage","bags"],["bags"]]
        Example 4:

            Input: products = ["havana"], searchWord = "tatiana"
            Output: [[],[],[],[],[],[],[]]


        Constraints:
            1 <= products.length <= 1000
            There are no repeated elements in products.
            1 <= Σ products[i].length <= 2 * 10^4
            All characters of products[i] are lower-case English letters.
            1 <= searchWord.length <= 1000
            All characters of searchWord are lower-case English letters.
        */
        public static IList<IList<string>> SuggestedProducts(string[] products, string searchWord)
        {
            var retval = new List<IList<string>>();
            if (searchWord.Length == 0 || products.Length == 0) return retval;
            Dictionary<string, IList<string>> prefixMatchDict = new Dictionary<string, IList<string>>();
            var prefix = searchWord[0].ToString();
            var initList = SearchForMatchingPrefix(prefix, 0, products);
            prefixMatchDict.Add(prefix, initList);
            UpdateTop3MatchingProducts(initList, retval);
            for (int i = 1; i < searchWord.Length; i++)
            {
                var curPrefix = prefix + searchWord[i].ToString();
                var list = SearchForMatchingPrefix(curPrefix, i, prefixMatchDict[prefix]);
                prefix = curPrefix;
                prefixMatchDict.Add(prefix, list);
                UpdateTop3MatchingProducts(list, retval);
            }
            return retval;
        }

        private static void UpdateTop3MatchingProducts(List<string> products, List<IList<string>> result)
        {
            products.Sort();
            if (products.Count > 3)
            {
                result.Add(products.Take(3).ToList());
            }
            else result.Add(products);
        }
        private static List<string> SearchForMatchingPrefix(string prefix, int i, IEnumerable<string> products)
        {
            var retval = new List<string>();
            foreach (var p in products)
            {
                if (p.Length >= i + 1 && p.Substring(0, i + 1) == prefix)
                {
                    retval.Add(p);
                }
            }
            return retval;
        }

        public static int NumDistinctIslands(int[][] grid)
        {
            int noi = 0;
            if (grid.Length == 0) return noi;
            bool[,] visited = new bool[grid.Length, grid[0].Length];
            var uniqueIslandShapes = new HashSet<string>();
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    if (grid[i][j] == 1 && !visited[i, j])
                    {
                        var shape = "";
                        NumDistinctIslandsDFS(i, j, i, j, visited, grid, ref shape);
                        if (!uniqueIslandShapes.Contains(shape))
                        {
                            uniqueIslandShapes.Add(shape);
                            noi++;
                        }
                    }
                }
            }
            return noi;
        }

        private static void NumDistinctIslandsDFS(int i, int j, int normI, int normJ, bool[,] visited, int[][] grid, ref string shape)
        {
            if (i < 0 || i >= grid.Length || j < 0 || j >= grid[i].Length || visited[i, j])
                return;

            visited[i, j] = true;

            if (grid[i][j] == 0) return;
            shape += $"{i - normI}{j - normJ}";
            NumDistinctIslandsDFS(i + 1, j, normI, normJ, visited, grid, ref shape);
            NumDistinctIslandsDFS(i - 1, j, normI, normJ, visited, grid, ref shape);
            NumDistinctIslandsDFS(i, j + 1, normI, normJ, visited, grid, ref shape);
            NumDistinctIslandsDFS(i, j - 1, normI, normJ, visited, grid, ref shape);
        }


        public static IList<int> NumIslands2(int m, int n, int[][] positions)
        {
            HashSet<string> posiSet = new HashSet<string>();
            HashSet<string> visited = new HashSet<string>();
            IList<int> retval = new List<int>();

            if (m == 0 || n == 0) return retval;
            if (positions.Length == 0)
            {
                retval.Add(0);
                return retval;
            }
            foreach (var p in positions)
            {
                posiSet.Add(string.Join("", p));
            }

            int noi = 0;

            for (int x = 0; x < positions.Length; x++)
            {
                var i = positions[x][0];
                var j = positions[x][1];

                //check up
                if (i - 1 >= 0 && posiSet.Contains($"{i - 1}{j}") && !visited.Contains($"{i - 1}{j}"))
                {
                    visited.Add($"{i - 1}{j}");
                }
                //check down
                if (i + 1 < m && posiSet.Contains($"{i + 1}{j}") && !visited.Contains($"{i + 1}{j}"))
                {
                    visited.Add($"{i + 1}{j}");
                }
                //check left
                if (j - 1 >= 0 && posiSet.Contains($"{i}{j - 1}") && !visited.Contains($"{i}{j - 1}"))
                {
                    visited.Add($"{i}{j - 1}");
                }
                //check right
                if (j + 1 < n && posiSet.Contains($"{i}{j + 1}") && !visited.Contains($"{i}{j + 1}"))
                {
                    visited.Add($"{i}{j + 1}");
                }

                if (!visited.Contains($"{i}{j}"))
                {
                    noi++;
                    visited.Add($"{i}{j}");
                }
                retval.Add(noi);
            }
            return retval;
        }

        /**Definition for a binary tree node.**/
        public class TreeNode
        {
            public int val;
            public TreeNode left;
            public TreeNode right;
            public TreeNode(int x) { val = x; }
        }

        public static string serialize(TreeNode root)
        {
            var q = new Queue<TreeNode>();
            var s = "";
            q.Enqueue(root);
            while (q.Count > 0)
            {
                var curNode = q.Dequeue();
                if (curNode == null)
                {
                    s += "#,";
                    continue;
                }
                s += $"{curNode.val},";
                q.Enqueue(curNode.left);
                q.Enqueue(curNode.right);
            }
            return s;
        }

        // Decodes your encoded data to tree.
        public static TreeNode deserialize(string data)
        {
            if (data.Length == 0 || data == "#,") return null;
            string[] s = data.Split(",");
            var q = new Queue<TreeNode>();
            var root = new TreeNode(Int32.Parse(s[0]));
            q.Enqueue(root);
            int i = 1;
            while (q.Count > 0 && i < s.Length - 1)
            {
                var cur = q.Dequeue();
                if (s[i] != "#")
                {
                    cur.left = new TreeNode(Int32.Parse(s[i]));
                    q.Enqueue(cur.left);
                }
                i++;

                if (s[i] != "#")
                {
                    cur.right = new TreeNode(Int32.Parse(s[i]));
                    q.Enqueue(cur.right);
                }
                i++;
            }

            return root;
        }

        /*
         * You are given an array coordinates, coordinates[i] = [x, y], where [x, y]
         * represents the coordinate of a point. Check if these points make a straight line in the XY plane.

            Input: coordinates = [[1,2],[2,3],[3,4],[4,5],[5,6],[6,7]]
            Output: true

            Input: coordinates = [[1,1],[2,2],[3,4],[4,5],[5,6],[7,7]]
            Output: false

            Constraints:

                2 <= coordinates.length <= 1000
                coordinates[i].length == 2
                -10^4 <= coordinates[i][0], coordinates[i][1] <= 10^4
                coordinates contains no duplicate point.
        */
        public static bool CheckStraightLine(int[][] coordinates)
        {
            int npts = coordinates.Length;
            if (npts == 0 || npts == 1) return false;
            int dy = coordinates[npts - 1][1] - coordinates[0][1];
            int dx = coordinates[npts - 1][0] - coordinates[0][0];
            int x2 = coordinates[npts - 1][0];
            int y2 = coordinates[npts - 1][1];
            /*
                two point line formula should satisfy for every 
                point in the array for it to be a straight line.

                i.e. (y - y2)/(x - x2) == dy/dx => dx(y-y2) = dy(x-x2)
            */
            for (int i = 1; i < npts - 1; i++)
            {
                int y = coordinates[i][1] - y2;
                int x = coordinates[i][0] - x2;
                if (y * dx == x * dy)
                {
                    continue;
                }
                else
                    return false;
            }

            return true;
        }


        public static int WidthOfBinaryTree(TreeNode root)
        {
            var q = new Queue<Tuple<TreeNode, int, int>>();
            q.Enqueue(new Tuple<TreeNode, int, int>(root, 0, 0));
            int currentDepth = 0;
            int left = 0;
            int width = 0;
            while (q.Count > 0)
            {
                var t = q.Dequeue();
                var node = t.Item1;
                var depth = t.Item2;
                var position = t.Item3;
                if (node != null)
                {
                    q.Enqueue(new Tuple<TreeNode, int, int>(node.left, depth + 1, position * 2 + 1));
                    q.Enqueue(new Tuple<TreeNode, int, int>(node.right, depth + 1, position * 2 + 2));
                    if (currentDepth != depth)
                    {
                        currentDepth = depth;
                        left = position;
                    }
                    width = Math.Max(width, position - left + 1);
                }
            }
            return width;
        }

        public static int SearchRotatedBST(int[] nums, int target)
        {
            //check if rotated

            return SearchBSTRec(nums, 0, nums.Length - 1, target);
        }
        private static int SearchBSTRec(int[] nums, int s, int e, int target)
        {
            if (s > e)
            {
                return -1;
            }
            int mid = (e + s) / 2;
            if (nums[mid] == target) return mid;

            //check if left half is strictly increasing
            if (nums[mid] >= nums[s])
            {
                if (target >= nums[s] && target < nums[mid])
                    return SearchBSTRec(nums, s, mid - 1, target);
                else
                    return SearchBSTRec(nums, mid + 1, e, target);
            }
            //else happens when right half is strictly increasing
            else
            {
                if (target <= nums[e] && target > nums[mid])
                    return SearchBSTRec(nums, mid + 1, e, target);
                else
                    return SearchBSTRec(nums, s, mid - 1, target);
            }

        }

        /*
         * 443 String Comnpression
         * **************************
           Given an array of characters, compress it in-place.

            The length after compression must always be smaller than or equal to the original array.

            Every element of the array should be a character (not int) of length 1.

            After you are done modifying the input array in-place, return the new length of the array.

 
            Follow up:
            Could you solve it using only O(1) extra space?

 
            Example 1:

            Input:
            ["a","a","b","b","c","c","c"]

            Output:
            Return 6, and the first 6 characters of the input array should be: ["a","2","b","2","c","3"]

            Explanation:
            "aa" is replaced by "a2". "bb" is replaced by "b2". "ccc" is replaced by "c3".
 

            Example 2:

            Input:
            ["a"]

            Output:
            Return 1, and the first 1 characters of the input array should be: ["a"]

            Explanation:
            Nothing is replaced.
 

            Example 3:

            Input:
            ["a","b","b","b","b","b","b","b","b","b","b","b","b"]

            Output:
            Return 4, and the first 4 characters of the input array should be: ["a","b","1","2"].

            Explanation:
            Since the character "a" does not repeat, it is not compressed. "bbbbbbbbbbbb" is replaced by "b12".
            Notice each digit has it's own entry in the array.
 

            Note:

            All characters have an ASCII value in [35, 126].
            1 <= len(chars) <= 1000.
             */
        public static int StringCompress(char[] chars)
        {
            //length of the compressed string
            int length = 0;
            //Keeps track of the char in chars array
            int i = 0;

            while (i < chars.Length)
            {
                //Keeps track of count of char in chars array
                int j = i;
                while (j < chars.Length && chars[i] == chars[j])
                {
                    j++;
                }
                chars[length++] = chars[i];

                //update the count
                if (j - i > 1)
                {
                    var countStr = $"{j - i}";
                    foreach (var n in countStr)
                    {
                        chars[length++] = n;
                    }
                }

                i = j;
            }

            return length;
        }

        /*

            The count-and-say sequence is the sequence of integers with the first five terms as following:

                1.     1
                2.     11
                3.     21
                4.     1211
                5.     111221
                1 is read off as "one 1" or 11.
                11 is read off as "two 1s" or 21.
                21 is read off as "one 2, then one 1" or 1211.

                Given an integer n where 1 ≤ n ≤ 30, generate the nth term of the count-and-say sequence. You can do so recursively, in other words from the previous member read off the digits, counting the number of digits in groups of the same digit.

                Note: Each term of the sequence of integers will be represented as a string.

 

                Example 1:

                Input: 1
                Output: "1"
                Explanation: This is the base case.
                Example 2:

                Input: 4
                Output: "1211"
                Explanation: For n = 3 the term was "21" in which we have two groups "2" and "1", "2" can be read as "12" which means frequency = 1 and value = 2, the same way "1" is read as "11", so the answer is the concatenation of "12" and "11" which is "1211".

        */
        public static string CountAndSay(int n)
        {
            var retval = "1";
            for (int i = 1; i < n; i++)
            {
                retval = CountAndSayRec(retval);
            }
            return retval;
        }

        private static string CountAndSayRec(string str)
        {
            string retval = "";
            int i = 0;
            int j = 0;
            while (i < str.Length)
            {
                j = i;

                while (j < str.Length && str[i] == str[j])
                {

                    j++;
                }
                retval = retval + $"{j - i}{str[i]}";
                i = j;
            }
            return retval;
        }

        /*

            545. Boundary of Binary Tree
            Input:
                ____1_____
               /          \
              2            3
             / \          / 
            4   5        6   
               / \      / \
              7   8    9  10  
       
            Ouput:
            [1,2,4,7,8,9,10,6,3]

            Explanation:
            The left boundary are node 1,2,4. (4 is the left-most node according to definition)
            The leaves are node 4,7,8,9,10.
            The right boundary are node 1,3,6,10. (10 is the right-most node).
            So order them in anti-clockwise without duplicate nodes we have [1,2,4,7,8,9,10,6,3].

         */
        public static IList<int> BoundaryOfBinaryTree(TreeNode root)
        {
            var boundary = new List<int>();
            if (root == null) return boundary;
            boundary.Add(root.val);
            if (root.left == null && root.right == null) return boundary;
            ComputeBoundaryLeft(root.left, boundary);
            ComputeBoundaryLeaf(root, boundary);
            ComputeBoundaryRight(root.right, boundary, false);
            return boundary;
        }


        private static void ComputeBoundaryLeft(TreeNode root, List<int> b)
        {
            if (root == null || root.left == null && root.right == null)
            {
                return;
            }

            b.Add(root.val);
            if (root.left != null)
            {
                ComputeBoundaryLeft(root.left, b);
            }
            else if (root.right != null)
            {
                ComputeBoundaryLeft(root.right, b);
            }

        }
        private static void ComputeBoundaryLeaf(TreeNode root, List<int> b)
        {
            if (root == null)
            {
                return;
            }
            if (root.left == null && root.right == null)
            {
                b.Add(root.val);
            }
            ComputeBoundaryLeaf(root.left, b);
            ComputeBoundaryLeaf(root.right, b);
        }
        private static void ComputeBoundaryRight(TreeNode root, List<int> b, bool isRootComputed)
        {
            if (root == null || root.left == null && root.right == null)
            {
                return;
            }

            if (root.right != null)
            {
                ComputeBoundaryRight(root.right, b, false);
            }
            else if (root.left != null)
            {
                ComputeBoundaryRight(root.left, b, false);
            }
            b.Add(root.val);
        }


        /*
        Given a binary tree, count the number of uni-value subtrees.

        A Uni-value subtree means all nodes of the subtree have the same value.

        Example :

        Input:  root = [5,1,5,5,5,null,5]

                      5
                     / \
                    1   5
                   / \   \
                  5   5   5

        Output: 4
        */
        public static int CountUnivalSubtrees(TreeNode root)
        {
            int count = 0;
            UniValDFS(root, ref count, root);
            return count;
        }

        private static bool UniValDFS(TreeNode root, ref int count, TreeNode parent)
        {
            if (root == null)
            {
                return true;
            }

            var left = UniValDFS(root.left, ref count, root);
            var right = UniValDFS(root.right, ref count, root);
            if (left && right)
            {
                count++;
            }
            else return false;
            return root.val == parent.val;
        }

        //[1,3,0,1] 

        public static int FindLoopSizeInArray(int[] arr)
        {
            int i = 0;
            while (i < arr.Length)
            {
                if (arr[i] < i) break;
                i = arr[i];
            }


            return i - arr[i] + 1;
        }

        /*
            Given an array with n objects colored red, white or blue, sort them in-place so that objects of the same color are adjacent, with the colors in the order red, white and blue.

            Here, we will use the integers 0, 1, and 2 to represent the color red, white, and blue respectively.

            Note: You are not suppose to use the library's sort function for this problem.

            Example:

            Input: [2,0,2,1,1,0]
            Output: [0,0,1,1,2,2]
            Follow up:

            A rather straight forward solution is a two-pass algorithm using counting sort.
            First, iterate the array counting number of 0's, 1's, and 2's, then overwrite array with total number of 0's, then 1's and followed by 2's.
            Could you come up with a one-pass algorithm using only constant space?
        */
        public static void SortColors(int[] nums)
        {
            int h = nums.Length - 1;
            int l = 0;
            int m = 0;
            while (m <= h)
            {
                if (nums[m] == 0)
                {
                    Swap(nums, m, l);
                    m++;
                    l++;
                }
                if (m <= h && nums[m] == 1)
                {
                    m++;
                }
                if (m <= h && nums[m] == 2)
                {
                    Swap(nums, m, h);
                    h--;
                }
            }
        }

        private static void Swap(int[] nums, int i, int j)
        {
            int s = nums[i];
            nums[i] = nums[j];
            nums[j] = s;
        }


        /*
        A string S of lowercase English letters is given.
        We want to partition this string into as many parts as
        possible so that each letter appears in at most one part,and
        return a list of integers representing the size of these parts.

            Example 1:

            Input: S = "ababcbacadefegdehijhklij"
            Output: [9,7,8]
            Explanation:
            The partition is "ababcbaca", "defegde", "hijhklij".
            This is a partition so that each letter appears in at most one part.
            A partition like "ababcbacadefegde", "hijhklij" is incorrect, because it splits S into less parts.
 

            Note:

            S will have length in range [1, 500].
            S will consist of lowercase English letters ('a' to 'z') only.
        */
        public static IList<int> PartitionLabels(string S)
        {
            int[] LastSeen = new int[26];
            for (int k = 0; k < S.Length; k++)
            {
                LastSeen[S[k] - 'a'] = k;
            }

            int i = 0;
            var partitions = new List<int>();
            while (i < S.Length)
            {
                int curPartitionLength = LastSeen[S[i] - 'a'];
                int j = i;
                while (j != curPartitionLength)
                {
                    curPartitionLength = Math.Max(curPartitionLength, LastSeen[S[j] - 'a']);
                    j++;
                }
                partitions.Add(j - i + 1);
                i = j + 1;
            }
            return partitions;
        }

        /*
         * Amazon - Min Cost to Repair Edges
        There's an undirected connected graph with n nodes labeled 1..n.
        But some of the edges has been broken disconnecting the graph.
        Find the minimum cost to repair the edges so that all the nodes are once again accessible from each other.

            Input:

            n, an int representing the total number of nodes.
            edges, a list of integer pair representing the nodes connected by an edge.
            edgesToRepair, a list where each element is a triplet representing the pair
            of nodes between which an edge is currently broken and the cost of repearing
            that edge, respectively (e.g. [1, 2, 12] means to repear an edge between nodes 1 and 2, the cost would be 12).
            Example 1:

            Input: n = 5, edges = [[1, 2], [2, 3], [3, 4], [4, 5], [1, 5]], edgesToRepair = [[1, 2, 12], [3, 4, 30], [1, 5, 8]]
            Output: 20
            Explanation:
            There are 3 connected components due to broken edges: [1], [2, 3] and [4, 5].
            We can connect these components into a single component by repearing the edges
            between nodes 1 and 2, and nodes 1 and 5 at a minimum cost 12 + 8 = 20.
         */

        //TODO: this solution is incomplete, need to use priority queue for prims..
        public static int MinimumRepairCost(int[][] edges, int[][] edgesToRepair, int n)
        {
            int minCost = 0;
            Dictionary<string, int> edgeList = new Dictionary<string, int>();
            HashSet<int> visited = new HashSet<int>();
            foreach (var e in edges)
            {
                edgeList.Add($"{e[0]}{e[1]}", 0);
            }

            foreach (var e in edgesToRepair)
            {
                edgeList[$"{e[0]}{e[1]}"] = e[2];
            }

            var sortedEdgeList = from entry in edgeList orderby entry.Value ascending select entry;

            foreach (var e in sortedEdgeList)
            {
                int v1 = e.Key[0] - '0';
                int v2 = e.Key[1] - '0';
                if (visited.Contains(v1) && visited.Contains(v2))
                    continue;
                minCost += e.Value;
                visited.Add(v1);
                visited.Add(v2);
            }
            // using prims algo fing min spanning tree


            return minCost;
        }


        /* Amazon | OA 2019 | Optimal Utilization
        Given 2 lists a and b. Each element is a pair of integers where the first integer represents the unique id and
        the second integer represents a value. Your task is to find an element from a and an element form b such that the
        sum of their values is less or equal to target and as close to target as possible. Return a list of ids of selected elements.
        If no pair is possible, return an empty list.

            Example 1:

            Input:
            a = [[1, 2], [2, 4], [3, 6]]
            b = [[1, 2]]
            target = 7

            Output: [[2, 1]]

            Explanation:
            There are only three combinations [1, 1], [2, 1], and [3, 1], which have a total sum of 4, 6 and 8, respectively.
            Since 6 is the largest sum that does not exceed 7, [2, 1] is the optimal pair.
            Example 2:

            Input:
            a = [[1, 3], [2, 5], [3, 7], [4, 10]]
            b = [[1, 2], [2, 3], [3, 4], [4, 5]]
            target = 10

            Output: [[2, 4], [3, 2]]

            Explanation:
            There are two pairs possible. Element with id = 2 from the list `a` has a value 5, and element with id = 4 from the list `b` also has a value 5.
            Combined, they add up to 10. Similarily, element with id = 3 from `a` has a value 7, and element with id = 2 from `b` has a value 3.
            These also add up to 10. Therefore, the optimal pairs are [2, 4] and [3, 2].
            Example 3:

            Input:
            a = [[1, 8], [2, 7], [3, 14]]
            b = [[1, 5], [2, 10], [3, 14]]
            target = 20

            Output: [[3, 1]]
            Example 4:

            Input:
            a = [[1, 8], [2, 15], [3, 9]]
            b = [[1, 8], [2, 11], [3, 12]]
            target = 20

            Output: [[1, 3], [3, 2]]
         */
        //This is a bruteforce solution
        public static List<List<int>> OptimalUtilization(int[,] a, int[,] b, int target)
        {
            Dictionary<int, List<List<int>>> sums = new Dictionary<int, List<List<int>>>();
            int max = 0;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(0); j++)
                {
                    int curSum = a[i, 1] + b[j, 1];
                    if (max <= curSum && curSum <= target)
                    {
                        if (!sums.ContainsKey(curSum))
                        {
                            sums.Add(curSum, new List<List<int>>() { new List<int> { a[i, 0], b[j, 0] } });
                        }
                        else
                        {
                            sums[curSum].Add(new List<int> { a[i, 0], b[j, 0] });
                        }
                        max = Math.Max(curSum, max);
                    }
                }
            }
            return sums[max];
        }

        /*
        Amazon | OA 2019 | Find Pair With Given Sum
        Given a list of positive integers nums and an int target, return indices of the two numbers such that they add up to a target - 30.

        Conditions:

        You will pick exactly 2 numbers.
        You cannot pick the same element twice.
        If you have muliple pairs, select the pair with the largest number.
        Example 1:

        Input: nums = [1, 10, 25, 35, 60], target = 90
        Output: [2, 3]
        Explanation:
        nums[2] + nums[3] = 25 + 35 = 60 = 90 - 30
        Example 2:

        Input: nums = [20, 50, 40, 25, 30, 10], target = 90
        Output: [1, 5]
        Explanation:
        nums[0] + nums[2] = 20 + 40 = 60 = 90 - 30
        nums[1] + nums[5] = 50 + 10 = 60 = 90 - 30
        You should return the pair with the largest number.
        */
        public static int[] TargetPairSum(int[] arr, int target)
        {
            int[] retval = new int[2];
            int max = 0;
            HashSet<int> set = new HashSet<int>();
            if (arr.Length < 2) return retval;
            target -= 30;
            for (int i = 0; i < arr.Length; i++)
            {
                if (set.Contains(target - arr[i]))
                {
                    var cur = target - arr[i] > arr[i] ? target - arr[i] : arr[i];
                    max = Math.Max(max, cur);
                }
                else
                {
                    set.Add(arr[i]);
                }
            }
            retval[0] = max;
            retval[1] = target - max;
            return retval;
        }

        /*
          Find all unique Three Sum for zero
            Given array nums = [-1, 0, 1, 2, -1, -4],

            A solution set is:
            [
              [-1, 0, 1],
              [-1, -1, 2]
            ]
         */
        public static IList<IList<int>> ThreeSumZero(int[] nums)
        {
            var ret = new List<IList<int>>();
            if (nums.Length < 3) return ret;
            Array.Sort(nums);
            for (int i = 0; i < nums.Length - 2; i++)
            {
                if (i != 0 && nums[i - 1] == nums[i]) continue;
                var low = i + 1;
                var hi = nums.Length - 1;
                while (low < hi)
                {
                    if (nums[i] + nums[low] + nums[hi] == 0)
                    {
                        ret.Add(new List<int> { nums[i], nums[low], nums[hi] });
                        low++;
                        hi--;
                        while (low < hi && nums[low - 1] == nums[low])
                        {
                            ++low;
                        }
                    }
                    else if (nums[i] + nums[low] + nums[hi] < 0)
                    {
                        low++;
                    }
                    else
                    {
                        hi--;
                    }

                }
            }
            return ret;
        }

        /*
         Given an array nums of n integers and an integer target,
        find three integers in nums such that the sum is closest to target.
        Return the sum of the three integers. You may assume that each input would have exactly one solution.

 

            Example 1:

            Input: nums = [-1,2,1,-4], target = 1
            Output: 2
            Explanation: The sum that is closest to the target is 2. (-1 + 2 + 1 = 2).
 

            Constraints:

            3 <= nums.length <= 10^3
            -10^3 <= nums[i] <= 10^3
            -10^4 <= target <= 10^4
         */
        public static int ThreeSumClosest(int[] nums, int target)
        {
            Array.Sort(nums);
            int closeSum = nums[0] + nums[1] + nums[2];
            for (int i = 0; i < nums.Length - 2; i++)
            {
                int low = i + 1;
                int hi = nums.Length - 1;
                while (low < hi)
                {
                    int sum = nums[i] + nums[low] + nums[hi];
                    if (Math.Abs(sum - target) < Math.Abs(closeSum - target))
                    {
                        closeSum = sum;
                    }
                    else if (sum <= target)
                    {
                        low++;
                    }
                    else
                    {
                        hi--;
                    }
                }
            }

            return closeSum;
        }
        /*
         
            Input: ropes = [8, 4, 6, 12]
            Output: 58
            Explanation: The optimal way to connect ropes is as follows
            1. Connect the ropes of length 4 and 6 (cost is 10). Ropes after connecting: [8, 10, 12]
            2. Connect the ropes of length 8 and 10 (cost is 18). Ropes after connecting: [18, 12]
            3. Connect the ropes of length 18 and 12 (cost is 30).
            Total cost to connect the ropes is 10 + 18 + 30 = 58
            Example 2:

            Input: ropes = [20, 4, 8, 2]
            Output: 54
            Example 3:

            Input: ropes = [1, 2, 5, 10, 35, 89]
            Output: 224
            Example 4:

            Input: ropes = [2, 2, 3, 3]
            Output: 20
        */
        //O(NLogN) using sorted dictionary
        private class PriorityQueue
        {
            private SortedDictionary<int, int> Q;
            int _size;
            public PriorityQueue()
            {
                Q = new SortedDictionary<int, int>();
                _size = 0;
            }
            public int Size => _size;
            public int Dequeue()
            {
                var first = Q.FirstOrDefault();
                if (first.Value == 1) Q.Remove(first.Key);
                else Q[first.Key]--;
                _size--;
                return first.Key;
            }
            public void Enqueue(int key)
            {
                if (Q.ContainsKey(key))
                {
                    Q[key]++;
                }
                else
                {
                    Q.Add(key, 1);
                }
                _size++;
            }
        }
        public static int ConnectRopesOptimally(int[] arr)
        {
            int ropeLength = 0;

            //using sorted dictionary as priority queue
            PriorityQueue ropes = new PriorityQueue();
            int p = 0;
            while (p < arr.Length)
            {

                ropes.Enqueue(arr[p]);
                p++;
            }
            while (ropes.Size > 1)
            {
                int curRopeLength = ropes.Dequeue() + ropes.Dequeue();
                ropeLength += curRopeLength;
                ropes.Enqueue(curRopeLength);
            }
            return ropeLength;
        }

        /*  Amazon | OA 2019 | Treasure Island
         
            You have a map that marks the location of a treasure island. Some of the map area has jagged rocks and dangerous reefs.
            Other areas are safe to sail in. There are other explorers trying to find the treasure. So you must figure out a shortest route to the treasure island.

            Assume the map area is a two dimensional grid, represented by a matrix of characters. You must start from the top-left corner of the map and can move one block up, down,
            left or right at a time. The treasure island is marked as X in a block of the matrix. X will not be at the top-left corner.
            Any block with dangerous rocks or reefs will be marked as D. You must not enter dangerous blocks. You cannot leave the map area.
            Other areas O are safe to sail in. The top-left corner is always safe. Output the minimum number of steps to get to the treasure.
            Example:

            Input:
            [['O', 'O', 'O', 'O'],
             ['D', 'O', 'D', 'O'],
             ['O', 'O', 'O', 'O'],
             ['X', 'D', 'D', 'O']]

            Output: 5
            Explanation: Route is (0, 0), (0, 1), (1, 1), (2, 1), (2, 0), (3, 0) The minimum route takes 5 steps.
         */

        public static int TreasureIsland(int[,] grid)
        {
            bool[,] visited = new bool[grid.GetLength(0), grid.GetLength(1)];
            int size = 0;
            Queue<int> Qi = new Queue<int>();
            Queue<int> Qj = new Queue<int>();
            int i = 0;
            int j = 0;
            Qi.Enqueue(i);
            Qj.Enqueue(j);
            visited[i, j] = true;
            while (Qi.Count > 0)
            {
                i = Qi.Dequeue();
                j = Qj.Dequeue();
                if (i < 0 || i >= grid.GetLength(0) || j < 0 || j >= grid.GetLength(1) || visited[i, j] || grid[i, j] == 'D')
                {
                    continue;
                }

                size++;
            }
            return size;
        }

        private static void TreasureIsland(int[,] grid, bool[,] visited, ref int size, int i, int j)
        {
            if (i < 0 || i >= grid.GetLength(0) || j < 0 || j >= grid.GetLength(1) || visited[i, j] || grid[i, j] == 'D')
            {
                return;
            }
            visited[i, j] = true;
            size++;
            if (grid[i, j] == 'X')
            {
                return;
            }
            TreasureIsland(grid, visited, ref size, i, j + 1);
            TreasureIsland(grid, visited, ref size, i + 1, j);
            TreasureIsland(grid, visited, ref size, i - 1, j);
            TreasureIsland(grid, visited, ref size, i, j - 1);

        }

        /*
         * 22. Generate Parentheses
        Given n pairs of parentheses, write a function to generate all combinations of well-formed parentheses.

        For example, given n = 3, a solution set is:

        [
          "((()))",
          "(()())",
          "(())()",
          "()(())",
          "()()()"
        ]
             */
        public static IList<string> GenerateParenthesis(int n)
        {
            var retval = new List<string>();
            if (n == 0)
                return new List<string>() { "" };
            Backtrack(n, "", n, n, ref retval);
            return retval;
        }

        private static void Backtrack(int n, string paren, int open, int close, ref List<string> retval)
        {
            if (paren.Length == 2 * n)
            {
                retval.Add(paren);
                return;
            }
            if (open <= close && open >= 0)
            {
                if (open > 0)
                    Backtrack(n, paren + "(", open - 1, close, ref retval);
                if (close > 0)
                    Backtrack(n, paren + ")", open, close - 1, ref retval);
            }
        }


        /*
        Facebook phone interview: Change Directory

        given a string cwd(current working directory) and string args(desired directory), change the current directory
        to the given directory and output the complete path as a string.

        Example 1:

        cwd: /c
        args: a
        o/p: /c/a

        Example 2:

        cwd: /c/d
        args: ../q/../p
        o/p: /c/p

        Example 3:
        cwd: /foo/bar
        args: ../../../c/d
        o/p: /c/d
         */


        public static string ChangeDirectory(string cwd, string targetDir)
        {
            if (targetDir == "" || targetDir == "/") return cwd;

            string retval = "";

            Stack<string> cwdStack = new Stack<string>(cwd.Split("/", StringSplitOptions.RemoveEmptyEntries));
            Queue<string> cmdStack = new Queue<string>(targetDir.Split("/", StringSplitOptions.RemoveEmptyEntries));
            while (cmdStack.Count > 0)
            {
                var currentCmd = cmdStack.Dequeue();
                if (currentCmd == "..")
                {
                    if (cwdStack.Count > 0)
                        cwdStack.Pop();
                }
                else
                {
                    cwdStack.Push(currentCmd);
                }
            }
            while (cwdStack.Count > 0)
            {
                retval = "/" + cwdStack.Pop() + retval;
            }

            return retval == "" ? "/" : retval;
        }

        /*
            Three Sum in an unsorted array, given an input unsorted array of unique integers and a tagrget sum, find all the matching subsets/triplets,
            the output should be sorted in ascending order, also the integers in the triplets should be in arranged in ascending order.

            Example:
            array: [12, 3, 1, 2, -6, 5, -8, 6]
            targetSum: 0
            output: [[-8, 2, 6], [-8, 3, 5], [-6, 1, 5]]

         */
        public static List<int[]> ThreeNumberSum(int[] array, int targetSum)
        {
            Array.Sort(array);
            var retval = new List<int[]>();
            for (int i = 0; i < array.Length - 2; i++)
            {
                int left = i + 1;
                int right = array.Length - 1;
                var current = array[i];
                while (left < right)
                {
                    var currentSum = current + array[left] + array[right];
                    if (currentSum == targetSum)
                    {
                        retval.Add(new int[] { current, array[left], array[right] });
                        left++;
                    }
                    else if (currentSum < targetSum)
                    {
                        left++;
                    }
                    else
                    {
                        right--;
                    }
                }

            }
            return retval;
        }

        /*
        Word Ladder II
         */

        public static IList<IList<string>> FindLadders(string beginWord, string endWord, IList<string> wordList)
        {
            var retval = new List<IList<string>>();
            var wordSet = wordList.ToHashSet();
            var q = new Queue<string>();
            q.Enqueue(beginWord);
            var wordGraph = new Dictionary<string, List<string>>();
            wordGraph.Add(beginWord, new List<string>());
            while (q.Count > 0)
            {
                var qCount = q.Count;
                for (int i = 0; i < qCount; i++)
                {
                    var currentWord = q.Dequeue();
                    wordSet.Remove(currentWord);
                    foreach (var w in wordSet)
                    {
                        if (WordDiff(w, currentWord) == 1)
                        {
                            wordGraph[currentWord].Add(w);
                            if (wordGraph.ContainsKey(w) || w == endWord) continue;
                            q.Enqueue(w);
                            wordGraph.Add(w, new List<string>());
                        }
                    }
                }
            }

            int minLen = Int32.MaxValue;
            //dfs on wordGraph
            dfs(wordGraph, beginWord, ref retval, endWord, new List<string> { beginWord }, ref minLen);

            return retval;
        }

        private static void dfs(Dictionary<string, List<string>> wordGraph,
                     string currentWord,
                     ref List<IList<string>> retval,
                     string endWord, List<string> currentWordList, ref int minLen)
        {
            if (currentWord == endWord)
            {
                if (retval.Count == 0 || minLen > currentWordList.Count)
                {
                    retval = new List<IList<string>> { new List<string>(currentWordList) };
                    minLen = currentWordList.Count;
                }
                else if (minLen == currentWordList.Count)
                {
                    retval.Add(new List<string>(currentWordList));
                }
                return;
            }

            if (currentWordList.Count >= minLen) return;
            foreach (var w in wordGraph[currentWord])
            {
                if (w == currentWord) continue;
                currentWordList.Add(w);
                dfs(wordGraph, w, ref retval, endWord, currentWordList, ref minLen);
                currentWordList.RemoveAt(currentWordList.Count - 1);
            }

        }

        private static int WordDiff(string a, string b)
        {
            int diff = 0;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    diff++;
            }
            return diff;
        }


        /* Course Schedule
         * 
            There are a total of numCourses courses you have to take, labeled from 0 to numCourses-1.
            Some courses may have prerequisites, for example to take course 0 you have to first take course 1,
            which is expressed as a pair: [0,1]

            Given the total number of courses and a list of prerequisite pairs, is it possible for you to finish all courses?

 

            Example 1:

            Input: numCourses = 2, prerequisites = [[1,0]]
            Output: true
            Explanation: There are a total of 2 courses to take. 
                            To take course 1 you should have finished course 0. So it is possible.
            Example 2:

            Input: numCourses = 2, prerequisites = [[1,0],[0,1]]
            Output: false
            Explanation: There are a total of 2 courses to take. 
                            To take course 1 you should have finished course 0, and to take course 0 you should
                            also have finished course 1. So it is impossible.
 

            Constraints:

            The input prerequisites is a graph represented by a list of edges, not adjacency matrices.
            Read more about how a graph is represented. You may assume that there are no duplicate edges in the input prerequisites.
            1 <= numCourses <= 10^5
         
         */

        public static bool CanFinish(int numCourses, int[][] prerequisites)
        {
            var courseG = GetAdjMatrix(prerequisites);
            int unTakenCourses = numCourses;
            bool hasCycle = false;
            foreach (var c in courseG.Keys)
            {
                var visited = new HashSet<int>();
                CanFinishdfs(courseG, ref visited, ref hasCycle, c);
                if (hasCycle) break;
                unTakenCourses--;
            }
            return !hasCycle && unTakenCourses >= 0;
        }
        private static void CanFinishdfs(Dictionary<int, List<int>> courseG, ref HashSet<int> visited, ref bool hasCycle, int currentCourse)
        {
            if (visited.Contains(currentCourse))
            {
                hasCycle = true;
                return;
            }
            visited.Add(currentCourse);
            foreach (var p in courseG[currentCourse])
            {
                CanFinishdfs(courseG, ref visited, ref hasCycle, p);
                if (hasCycle) break;
            }
            visited.Remove(currentCourse);
        }
        private static Dictionary<int, List<int>> GetAdjMatrix(int[][] edges)
        {
            var retval = new Dictionary<int, List<int>>();
            for (int i = 0; i < edges.Length; i++)
            {
                if (retval.ContainsKey(edges[i][0]))
                {
                    retval[edges[i][0]].Add(edges[i][1]);
                }
                else
                {
                    retval.Add(edges[i][0], new List<int> { edges[i][1] });
                }

                if (!retval.ContainsKey(edges[i][1]))
                    retval.Add(edges[i][1], new List<int>());
            }
            return retval;
        }

        /*
         Given a binary tree, find the lowest common ancestor (LCA) of two given nodes in the tree.

         Given the following binary tree:  root = [3,5,1,6,2,0,8,null,null,7,4]


 

        Example 1:

        Input: root = [3,5,1,6,2,0,8,null,null,7,4], p = 5, q = 1
        Output: 3
        Explanation: The LCA of nodes 5 and 1 is 3.
        Example 2:

        Input: root = [3,5,1,6,2,0,8,null,null,7,4], p = 5, q = 4
        Output: 5
        Explanation: The LCA of nodes 5 and 4 is 5, since a node can be a descendant of itself according to the LCA definition.
 

        Note:

        All of the nodes' values will be unique.
        p and q are different and both values will exist in the binary tree.
         */

        public static TreeNode LowestCommonAncestor(TreeNode root, TreeNode p, TreeNode q)
        {
            var sp = new Stack<TreeNode>();
            bool f = false;
            GetAncestors(root, p.val, ref sp, ref f);
            var sq = new Stack<TreeNode>();
            f = false;
            GetAncestors(root, q.val, ref sq, ref f);
            var lowestChild = sp.Count > sq.Count ? sp : sq;
            var d = Math.Abs(sp.Count - sq.Count);
            while (d > 0)
            {
                lowestChild.Pop();
                d--;
            }

            while (sp.Peek().val != sq.Peek().val)
            {
                sp.Pop();
                sq.Pop();
            }

            return sp.Pop();
        }

        private static void GetAncestors(TreeNode node, int target, ref Stack<TreeNode> s, ref bool found)
        {
            if (node == null)
            {
                return;
            }

            s.Push(node);
            if (node.val == target)
            {
                found = true;
                return;
            }
            GetAncestors(node.left, target, ref s, ref found);
            if (found) return;
            GetAncestors(node.right, target, ref s, ref found);
            if (found) return;
            s.Pop();
        }

        /*
        Given a binary tree, you need to compute the length of the diameter of the tree.
        The diameter of a binary tree is the length of the longest path between any two nodes in a tree.
        This path may or may not pass through the root.

        Example:
        Given a binary tree

                     1
                    / \
                   2   3
                  / \     
                 4   5

        Return 3, which is the length of the path [4,2,1,3] or [5,2,1,3].

        Note: The length of path between two nodes is represented by the number of edges between them.
         */
        public static int DiameterOfBinaryTree(TreeNode root)
        {
            if (root == null) return 0;
            var result = DiameterOfBinaryTreeRec(root);
            return (result.Item1 > result.Item2 ? result.Item1 : result.Item2) - 1;
        }

        private static Tuple<int, int> DiameterOfBinaryTreeRec(TreeNode node)
        {
            if (node == null)
            {
                return new Tuple<int, int>(0, 0);
            }

            var left = DiameterOfBinaryTreeRec(node.left);
            var right = DiameterOfBinaryTreeRec(node.right);
            var linearBranchLeft = left.Item1;
            var triangularPathLeft = left.Item2;
            var linearBranchRight = right.Item1;
            var triangularPathRight = right.Item2;
            var maxLinearBranch = Math.Max(linearBranchLeft, linearBranchRight);
            var currentMaxTriangular = Math.Max(triangularPathLeft, triangularPathRight);
            currentMaxTriangular = Math.Max(currentMaxTriangular, linearBranchLeft + linearBranchRight + 1);
            return new Tuple<int, int>(maxLinearBranch + 1, currentMaxTriangular);
        }


        /*
         * 
         * Cut Off Trees for Golf Event
         * 
            You are asked to cut off trees in a forest for a golf event. The forest is represented as a non-negative 2D map, in this map:

            0 represents the obstacle can't be reached.
            1 represents the ground can be walked through.
            The place with number bigger than 1 represents a tree can be walked through, and this positive number represents the tree's height.
            In one step you can walk in any of the four directions top, bottom, left and right also when standing in a point which is a tree you can decide whether or not to cut off the tree.

            You are asked to cut off all the trees in this forest in the order of tree's height - always cut off the tree with lowest height first. And after cutting, the original place has the tree will become a grass (value 1).

            You will start from the point (0, 0) and you should output the minimum steps you need to walk to cut off all the trees. If you can't cut off all the trees, output -1 in that situation.

            You are guaranteed that no two trees have the same height and there is at least one tree needs to be cut off.

            Example 1:

            Input: 
            [
             [1,2,3],
             [0,0,4],
             [7,6,5]
            ]
            Output: 6
 

            Example 2:

            Input: 
            [
             [1,2,3],
             [0,0,0],
             [7,6,5]
            ]
            Output: -1
 

            Example 3:

            Input: 
            [
             [2,3,4],
             [0,0,5],
             [8,7,6]
            ]
            Output: 6
            Explanation: You started from the point (0,0) and you can cut off the tree in (0,0) directly without walking.
         */

        //Brute force solution O(N2)
        public static int CutOffTree(IList<IList<int>> forest)
        {
            var numOfTrees = GetNumOfTrees(forest);
            if (numOfTrees.Count == 0) return 0;
            int steps = 0;
            int i = 0;
            int j = 0;
            var q = new Queue<int[]>();
            q.Enqueue(new int[] { i, j, 0 });
            foreach (var treeHeight in numOfTrees.Keys)
            {
                int r = CutOffTreebfs(ref forest, treeHeight, q);
                if (r < 0) return -1;
                steps += r;
                q = new Queue<int[]>();
                i = numOfTrees[treeHeight][0];
                j = numOfTrees[treeHeight][1];
                q.Enqueue(new int[] { i, j, 0 });
            }
            return steps;
        }

        private static int CutOffTreebfs(ref IList<IList<int>> forest, int tHeight, Queue<int[]> q)
        {
            var direction = new int[4, 2] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
            int m = forest.Count;
            int n = forest[0].Count;
            bool[,] visited = new bool[m, n];
            while (q.Count > 0)
            {

                var t = q.Dequeue();
                int i = t[0];
                int j = t[1];
                int l = t[2];

                visited[i, j] = true;
                if (forest[i][j] == tHeight)
                {
                    forest[i][j] = 1;
                    return l;
                }
                for (int k = 0; k < 4; k++)
                {
                    int di = i + direction[k, 0];
                    int dj = j + direction[k, 1];
                    if ((di >= 0 && di < m) &&
                       (dj >= 0 && dj < n) &&
                       forest[di][dj] != 0 && !visited[di, dj])
                    {
                        visited[di, dj] = true;
                        q.Enqueue(new int[] { di, dj, l + 1 });
                    }
                }

            }

            return -1;
        }


        private static SortedDictionary<int, int[]> GetNumOfTrees(IList<IList<int>> forest)
        {
            var ret = new SortedDictionary<int, int[]>();
            for (int i = 0; i < forest.Count; i++)
            {
                for (int j = 0; j < forest[i].Count; j++)
                {
                    if (forest[i][j] > 1)
                        ret.Add(forest[i][j], new int[] { i, j });
                }
            }
            return ret;
        }

        /*
         * LeetCode 79 WOrd Search

        Given a 2D board and a word, find if the word exists in the grid.

        The word can be constructed from letters of sequentially adjacent cell, where "adjacent" cells are those horizontally or vertically neighboring. The same letter cell may not be used more than once.

        Example:

        board =
        [
          ['A','B','C','E'],
          ['S','F','C','S'],
          ['A','D','E','E']
        ]

        Given word = "ABCCED", return true.
        Given word = "SEE", return true.
        Given word = "ABCB", return false.
 

        Constraints:

        board and word consists only of lowercase and uppercase English letters.
        1 <= board.length <= 200
        1 <= board[i].length <= 200
        1 <= word.length <= 10^3
         */

        public static bool Exist(char[][] board, string word)
        {
            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board[i].Length; j++)
                {
                    if (board[i][j] == word[0] && wordSearch(board, i, j, 0, word))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool wordSearch(char[][] board, int r, int c, int m, string word)
        {
            if (m == word.Length)
                return true;
            if (r < 0 || r >= board.Length || c < 0 || c >= board[r].Length || board[r][c] != word[m])
            {
                return false;
            }
            char temp = board[r][c];
            board[r][c] = ' ';
            bool found = wordSearch(board, r - 1, c, m + 1, word) || wordSearch(board, r, c + 1, m + 1, word) ||
                         wordSearch(board, r + 1, c, m + 1, word) || wordSearch(board, r, c - 1, m + 1, word);
            board[r][c] = temp;

            return found;
        }

        /*980. Unique Paths III
         
         On a 2-dimensional grid, there are 4 types of squares:

        1 represents the starting square.  There is exactly one starting square.
        2 represents the ending square.  There is exactly one ending square.
        0 represents empty squares we can walk over.
        -1 represents obstacles that we cannot walk over.
        Return the number of 4-directional walks from the starting square to the ending square,
        that walk over every non-obstacle square exactly once.

 

        Example 1:

        Input: [[1,0,0,0],[0,0,0,0],[0,0,2,-1]]
        Output: 2
        Explanation: We have the following two paths: 
        1. (0,0),(0,1),(0,2),(0,3),(1,3),(1,2),(1,1),(1,0),(2,0),(2,1),(2,2)
        2. (0,0),(1,0),(2,0),(2,1),(1,1),(0,1),(0,2),(0,3),(1,3),(1,2),(2,2)
        Example 2:

        Input: [[1,0,0,0],[0,0,0,0],[0,0,0,2]]
        Output: 4
        Explanation: We have the following four paths: 
        1. (0,0),(0,1),(0,2),(0,3),(1,3),(1,2),(1,1),(1,0),(2,0),(2,1),(2,2),(2,3)
        2. (0,0),(0,1),(1,1),(1,0),(2,0),(2,1),(2,2),(1,2),(0,2),(0,3),(1,3),(2,3)
        3. (0,0),(1,0),(2,0),(2,1),(2,2),(1,2),(1,1),(0,1),(0,2),(0,3),(1,3),(2,3)
        4. (0,0),(1,0),(2,0),(2,1),(1,1),(0,1),(0,2),(0,3),(1,3),(1,2),(2,2),(2,3)
        Example 3:

        Input: [[0,1],[2,0]]
        Output: 0
        Explanation: 
        There is no path that walks over every empty square exactly once.
        Note that the starting and ending square can be anywhere in the grid.
 

        Note:

        1 <= grid.length * grid[0].length <= 20
                 */

        public static int UniquePathsIII(int[][] grid)
        {
            int emptyCells = 0;
            int si = 0;
            int sj = 0;
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    if (grid[i][j] == 0)
                    {
                        emptyCells++;
                    }
                    else if (grid[i][j] == 1)
                    {
                        si = i;
                        sj = j;
                    }
                }
            }

            int ret = 0;
            bool[,] v = new bool[grid.Length, grid[0].Length];
            UniquePathsIIIdfs(grid, si, sj, v, emptyCells + 1, ref ret);
            return ret;
        }

        private static void UniquePathsIIIdfs(int[][] grid, int i, int j, bool[,] visited, int noe, ref int ret)
        {
            if (i < 0 || i >= grid.Length || j < 0 || j >= grid[i].Length || visited[i, j] || grid[i][j] == -1)
            {
                return;
            }
            if (noe > 0 && grid[i][j] == 2)
            {
                return;
            }
            else if (noe == 0 && grid[i][j] == 2)
            {
                ret++;
                return;
            }
            visited[i, j] = true;
            UniquePathsIIIdfs(grid, i - 1, j, visited, noe - 1, ref ret);
            UniquePathsIIIdfs(grid, i, j + 1, visited, noe - 1, ref ret);
            UniquePathsIIIdfs(grid, i + 1, j, visited, noe - 1, ref ret);
            UniquePathsIIIdfs(grid, i, j - 1, visited, noe - 1, ref ret);
            visited[i, j] = false;
        }

        /*Unique Paths II
         A robot is located at the top-left corner of a m x n grid (marked 'Start' in the diagram below).

        The robot can only move either down or right at any point in time. The robot is trying to reach the bottom-right corner of the grid (marked 'Finish' in the diagram below).

        Now consider if some obstacles are added to the grids. How many unique paths would there be?



        An obstacle and empty space is marked as 1 and 0 respectively in the grid.

        Note: m and n will be at most 100.

        Example 1:

        Input:
        [
          [0,0,0],
          [0,1,0],
          [0,0,0]
        ]
        Output: 2
        Explanation:
        There is one obstacle in the middle of the 3x3 grid above.
        There are two ways to reach the bottom-right corner:
        1. Right -> Right -> Down -> Down
        2. Down -> Down -> Right -> Right
         */
        /*
         Intuition

            The robot can only move either down or right. Hence any cell in the first row can only be reached from the cell left to it.
            And, any cell in the first column can only be reached from the cell above it.
            For any other cell in the grid, we can reach it either from the cell to left of it or the cell above it.

            If any cell has an obstacle, we won't let that cell contribute to any path.

            We will be iterating the array from left-to-right and top-to-bottom. Thus, before reaching any cell we would have the number of ways of reaching the predecessor cells. This is what makes it a Dynamic Programming problem. We will be using the obstacleGrid array as the DP array thus not utilizing any additional space.

            Note: As per the question, cell with an obstacle has a value 1. We would use this value to make sure if a cell needs to be included in the path or not. After that we can use the same cell to store the number of ways to reach that cell.

            Algorithm

            If the first cell i.e. obstacleGrid[0,0] contains 1, this means there is an obstacle in the first cell. Hence the robot won't be able to make any move and we would return the number of ways as 0.
            Otherwise, if obstacleGrid[0,0] has a 0 originally we set it to 1 and move ahead.
            Iterate the first row. If a cell originally contains a 1, this means the current cell has an obstacle and shouldn't contribute to any path. Hence, set the value of that cell to 0. Otherwise, set it to the value of previous cell i.e. obstacleGrid[i,j] = obstacleGrid[i,j-1]
            Iterate the first column. If a cell originally contains a 1, this means the current cell has an obstacle and shouldn't contribute to any path. Hence, set the value of that cell to 0. Otherwise, set it to the value of previous cell i.e. obstacleGrid[i,j] = obstacleGrid[i-1,j]
            Now, iterate through the array starting from cell obstacleGrid[1,1]. If a cell originally doesn't contain any obstacle then the number of ways of reaching that cell would be the sum of number of ways of reaching the cell above it and number of ways of reaching the cell to the left of it.
             obstacleGrid[i,j] = obstacleGrid[i-1,j] + obstacleGrid[i,j-1]
            If a cell contains an obstacle set it to 0 and continue. This is done to make sure it doesn't contribute to any other path.
         */
        public static int UniquePathsII(int[][] obstacleGrid)
        {
            if (obstacleGrid[0][0] == 1) return 0;
            obstacleGrid[0][0] = 1;
            int m = obstacleGrid.Length - 1;
            int n = obstacleGrid[0].Length - 1;
            for (int i = 1; i <= n; i++)
            {
                if (obstacleGrid[0][i] == 1)
                    obstacleGrid[0][i] = 0;
                else
                    obstacleGrid[0][i] = obstacleGrid[0][i - 1];
            }
            for (int i = 1; i <= m; i++)
            {
                if (obstacleGrid[i][0] == 1)
                    obstacleGrid[i][0] = 0;
                else
                    obstacleGrid[i][0] = obstacleGrid[i - 1][0];
            }
            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    if (obstacleGrid[i][j] == 1)
                        obstacleGrid[i][j] = 0;
                    else
                    {
                        obstacleGrid[i][j] = obstacleGrid[i][j - 1] + obstacleGrid[i - 1][j];
                    }
                }
            }

            return obstacleGrid[m][n];
        }

        /*
         * MinSum Path
         * 
         * 
         Given a m x n grid filled with non-negative numbers,
         find a path from top left to bottom right which minimizes
         the sum of all numbers along its path.

            Note: You can only move either down or right at any point in time.

            Example:

            Input:
            [
              [1,3,1],
              [1,5,1],
              [4,2,1]
            ]
            Output: 7
            Explanation: Because the path 1→3→1→1→1 minimizes the sum.
         * 
         */
        public static int MinPathSum(int[][] grid)
        {
            int m = grid.Length;
            int n = grid[0].Length;
            for (int i = 1; i < m; i++)
            {
                grid[i][0] = grid[i - 1][0] + grid[i][0];
            }
            for (int i = 1; i < n; i++)
            {
                grid[0][i] = grid[0][i - 1] + grid[0][i];
            }
            for (int i = 1; i < m; i++)
            {
                for (int j = 1; j < n; j++)
                {
                    var minV = Math.Min(grid[i - 1][j], grid[i][j - 1]);
                    grid[i][j] = grid[i][j] + minV;
                }
            }
            return grid[m - 1][n - 1];
        }
        /*
         * Same BST - check if the two input arrays represent the same BST
         * Note: Function is not allow to create a BST to do this validation.
         * 
         * input:
         *  arrayOne = [10, 15, 8, 12, 94, 81, 5, 2, 11]
         *  arrayTwo = [10, 8, 5, 15, 2, 12, 11, 94, 81]
         *  
         *  output: true
         */
        public static bool SameBsts(List<int> arrayOne, List<int> arrayTwo)
        {
            return isSameBst(arrayOne, arrayTwo);
        }

        private static bool isSameBst(List<int> aOne, List<int> aTwo)
        {
            if (aOne.Count == 0 && aTwo.Count == 0)
            {
                return true;
            }
            if (aOne.Count != aTwo.Count || aOne[0] != aTwo[0])
            {
                return false;
            }
            var oneResult = GetLeftAndRight(aOne, aOne[0]);
            var twoResult = GetLeftAndRight(aTwo, aTwo[0]);
            bool ret = true;
            ret = isSameBst(oneResult[0], twoResult[0]) && isSameBst(oneResult[1], twoResult[1]);
            return ret;
        }

        private static List<List<int>> GetLeftAndRight(List<int> array, int root)
        {
            var ret = new List<List<int>> { new List<int>(), new List<int>() };
            for (int i = 1; i < array.Count; i++)
            {
                if (array[i] >= root)
                {
                    ret[1].Add(array[i]);
                }
                else
                {
                    ret[0].Add(array[i]);
                }
            }
            return ret;
        }

        /*
         AlgoExpert Difficult: Largest Range
         */
        public static int[] LargestRange(int[] array)
        {
            var set = new HashSet<int>(array);
            int k = set.Count;
            int maxLen = 0;
            int[] ret = new int[2];
            while (k > 0)
            {
                int first = set.FirstOrDefault();
                int last = first;
                int currentLen = 1;
                set.Remove(first);
                while (k > 0 && set.Contains(last + 1))
                {
                    set.Remove(++last);
                    currentLen++;
                    k--;
                }
                while (k > 0 && set.Contains(first - 1))
                {
                    set.Remove(--first);
                    currentLen++;
                    k--;
                }
                if (maxLen < currentLen)
                {
                    ret[0] = first;
                    ret[1] = last;
                    maxLen = currentLen;
                }
                k--;
            }
            return ret;
        }

        /*
         * AlgoExpert Difficult: Minimum Reward
         */

        public static int MinRewards(int[] scores)
        {
            int[] rewards = new int[scores.Length];
            for (int i = 0; i < scores.Length; i++)
            {
                rewards[i] = 1;
            }
            int cur = 1;
            while (cur < scores.Length)
            {
                if (scores[cur] > scores[cur - 1])
                {
                    rewards[cur] = rewards[cur - 1] + 1;
                }
                cur++;
            }
            cur = scores.Length - 2;
            while (cur >= 0)
            {
                if (scores[cur] > scores[cur + 1])
                {
                    rewards[cur] = Math.Max(rewards[cur + 1] + 1, rewards[cur]);
                }
                cur--;
            }


            return rewards.Sum();
        }

        //Word Break
        //brute force
        /*
         Given a non-empty string s and a dictionary wordDict containing a list of non-empty words, determine if s can be segmented into a space-separated sequence of one or more dictionary words.

            Note:

            The same word in the dictionary may be reused multiple times in the segmentation.
            You may assume the dictionary does not contain duplicate words.
            Example 1:

            Input: s = "leetcode", wordDict = ["leet", "code"]
            Output: true
            Explanation: Return true because "leetcode" can be segmented as "leet code".
            Example 2:

            Input: s = "applepenapple", wordDict = ["apple", "pen"]
            Output: true
            Explanation: Return true because "applepenapple" can be segmented as "apple pen apple".
                         Note that you are allowed to reuse a dictionary word.
            Example 3:

            Input: s = "catsandog", wordDict = ["cats", "dog", "sand", "and", "cat"]
            Output: false
         */
        public static bool WordBreak(string s, IList<string> wordDict)
        {
            if (s == "") return true;
            foreach (var w in wordDict)
            {
                if (isPrefix(s, w) && WordBreak(new String(s.Skip(w.Length).ToArray()), wordDict))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool isPrefix(string a, string b)
        {

            if (a.Length < b.Length) return false;
            for (int i = 0; i < b.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }
            return true;
        }
        // Word break
        //using memorization O(N2)
        public static bool WordBreakMem(string s, IList<string> wordDict)
        {
            Dictionary<string, bool> r = new Dictionary<string, bool>();
            return WordBreakRec(s, wordDict, ref r);
        }

        public static bool WordBreakRec(string s, IList<string> wordDict, ref Dictionary<string, bool> r)
        {
            if (s == "") return true;
            if (r.ContainsKey(s)) return r[s];
            foreach (var w in wordDict)
            {
                var prefix = new String(s.Take(w.Length).ToArray());
                if (prefix == w && WordBreakRec(new String(s.Skip(w.Length).ToArray()), wordDict, ref r))
                {
                    r.Add(s, true);
                    return true;
                }
            }

            r.Add(s, false);
            return false;
        }

        //Word Break II
        /*
         Given a non-empty string s and a dictionary wordDict containing a
        list of non-empty words, add spaces in s to construct a sentence where each word is a valid dictionary word. Return all such possible sentences.

            Note:

            The same word in the dictionary may be reused multiple times in the segmentation.
            You may assume the dictionary does not contain duplicate words.
            Example 1:

            Input:
            s = "catsanddog"
            wordDict = ["cat", "cats", "and", "sand", "dog"]
            Output:
            [
              "cats and dog",
              "cat sand dog"
            ]
            Example 2:

            Input:
            s = "pineapplepenapple"
            wordDict = ["apple", "pen", "applepen", "pine", "pineapple"]
            Output:
            [
              "pine apple pen apple",
              "pineapple pen apple",
              "pine applepen apple"
            ]
            Explanation: Note that you are allowed to reuse a dictionary word.
            Example 3:

            Input:
            s = "catsandog"
            wordDict = ["cats", "dog", "sand", "and", "cat"]
            Output:
            []
         */
        public static IList<string> WordBreakII(string s, IList<string> wordDict)
        {
            if (s.Length == 0) return new List<string>();
            Dictionary<string, IList<string>> mem = new Dictionary<string, IList<string>>();
            return WordBreakIIRec(s, ref wordDict, ref mem);
        }

        public static IList<string> WordBreakIIRec(string s, ref IList<string> wordDict,
                                        ref Dictionary<string, IList<string>> mem)
        {
            if (s == "")
            {
                return new List<string>();
            }
            if (mem.ContainsKey(s))
            {
                return mem[s];
            }

            IList<string> ret = new List<string>();
            foreach (var w in wordDict)
            {
                var prefix = new String(s.Take(w.Length).ToArray());
                if (prefix == w)
                {
                    if (s == w)
                    {
                        ret.Add(w);
                    }
                    else
                    {
                        foreach (var v in WordBreakIIRec(new String(s.Skip(w.Length).ToArray()), ref wordDict, ref mem))
                        {
                            ret.Add(w + " " + v);
                        }
                    }

                }
            }

            mem.Add(s, ret);
            return ret;
        }

        public static bool WordBreakTrie(string s, IList<string> wordDict)
        {
            var trie = new Trie();
            trie.Insert(wordDict);
            int i = 0;
            while (i < s.Length)
            {
                i = trie.Search(s, i);
                if (i < 0)
                    return false;
                i++;
            }
            return true;
        }
        /*
         Implement atoi which converts a string to an integer.

        The function first discards as many whitespace characters as necessary until the first non-whitespace character is found. Then, starting from this character, takes an optional initial plus or minus sign followed by as many numerical digits as possible, and interprets them as a numerical value.

        The string can contain additional characters after those that form the integral number, which are ignored and have no effect on the behavior of this function.

        If the first sequence of non-whitespace characters in str is not a valid integral number, or if no such sequence exists because either str is empty or it contains only whitespace characters, no conversion is performed.

        If no valid conversion could be performed, a zero value is returned.

        Note:

        Only the space character ' ' is considered as whitespace character.
        Assume we are dealing with an environment which could only store integers within the 32-bit signed integer range: [−231,  231 − 1]. If the numerical value is out of the range of representable values, INT_MAX (231 − 1) or INT_MIN (−231) is returned.
         */
        public static int MyAtoi(string str)
        {
            const double INTMAX = Int32.MaxValue;
            const double INTMIN = Int32.MinValue;
            /*
                ^ - Represents the start of text
                \s* - Represents zero or more spaces chars
                -? - Represents zero or one '-' chars
                \+? - Represents zero or one '+' chars
                [0-9]+ - Represents one or more number/digits
                .* - Represents any char
                $ - Represents end of text
            */
            var p = @"^\s*-?\+?[0-9]+.*$";
            int r = 0;
            if (Regex.IsMatch(str, p))
            {
                /*
                ^ - Represents the start of text
                \s* - Represents zero or more spaces chars
                -? - Represents zero or one '-' chars
                \+* - Represents zero or more '+' chars
                \d+ - Represents one or more number/digits
                ?<digits> - Represents group name as 'digits'
                (exp) - represents group
                [^0-9]? - Represents zero or one non-digit chars
                .* - Represents any char
                $ - Represents end of text
            */
                p = @"^\s*(?<digits>-*\+*\d+)[^0-9]?.*$";
                Double.TryParse(Regex.Match(str, p).Groups["digits"].ToString(), out var d);
                if (d > INTMAX)
                    r = (int)INTMAX;
                else if (d < INTMIN)
                    r = (int)INTMIN;
                else
                    r = (int)d;
            }
            return r;
        }


        /*
         * Container with most water.
            Given n non-negative integers a1, a2, ..., an , where each represents a point at
            coordinate (i, ai). n vertical lines are drawn such that the two endpoints of line i is at (i, ai) and (i, 0).
            Find two lines, which together with x-axis forms a container, such that the container contains the most water.

            Note: You may not slant the container and n is at least 2.
         */

        //Brute force O(N2)
        public static int MaxArea(int[] height)
        {
            int ret = 0;
            for (int i = 0; i < height.Length - 1; i++)
            {
                int w = 1;
                for (int j = i + 1; j < height.Length; j++)
                {
                    var h = Math.Min(height[i], height[j]);
                    ret = Math.Max(ret, h * w);
                    w++;
                }
            }
            return ret;
        }

        //using Tw0-Pointer technique solve in O(N)
        public static int MaxAreaII(int[] height)
        {
            int ret = 0;
            int i = 0;
            int j = height.Length - 1;
            int w = height.Length - 1;
            while (w > 0)
            {
                if (height[i] < height[j])
                {
                    ret = Math.Max(ret, height[i] * w);
                    i++;
                }
                else
                {
                    ret = Math.Max(ret, height[j] * w);
                    j--;
                }
                w--;
            }
            return ret;
        }

        /*
         Leetcode : Integer to Roman (1 - 3999 given range)
        Roman numerals are represented by seven different symbols: I, V, X, L, C, D and M.

        Symbol       Value
        I             1
        V             5
        X             10
        L             50
        C             100
        D             500
        M             1000
        For example, two is written as II in Roman numeral, just two one's added together. Twelve is written as, XII, which is simply X + II. The number twenty seven is written as XXVII, which is XX + V + II.

        Roman numerals are usually written largest to smallest from left to right. However, the numeral for four is not IIII. Instead, the number four is written as IV. Because the one is before the five we subtract it making four. The same principle applies to the number nine, which is written as IX. There are six instances where subtraction is used:

        I can be placed before V (5) and X (10) to make 4 and 9. 
        X can be placed before L (50) and C (100) to make 40 and 90. 
        C can be placed before D (500) and M (1000) to make 400 and 900.
        Given an integer, convert it to a roman numeral. Input is guaranteed to be within the range from 1 to 3999.

        Example 1:

        Input: 3
        Output: "III"
        Example 2:

        Input: 4
        Output: "IV"
        Example 3:

        Input: 9
        Output: "IX"
        Example 4:

        Input: 58
        Output: "LVIII"
        Explanation: L = 50, V = 5, III = 3.
        Example 5:

        Input: 1994
        Output: "MCMXCIV"
        Explanation: M = 1000, CM = 900, XC = 90 and IV = 4.
         */
        //Beats 98.5% leetcode c# soln
        static List<int> romanKey = new List<int> { 0, 1, 4, 9, 40, 90, 400, 900 };
        static List<string> romanValue = new List<string> { "", "I", "V", "X", "L", "C", "D", "M" };
        public static string IntToRoman(int num)
        {
            string ret = "";
            int c = num.ToString().Length - 1;
            while (num > 0 && c >= 0)
            {
                int p = (int)Math.Pow(10, c);
                int msb = num / p;
                ret += GetRoman(msb, GetRomanIdx(num));
                num = num - (msb * p);
                c--;
            }
            return ret;
        }

        private static int GetRomanIdx(int num)
        {
            int i = 1;
            for (; i < romanKey.Count; i++)
            {
                if (num < romanKey[i])
                {
                    break;
                }
            }
            return i - 1;
        }
        private static string GetRoman(int num, int i)
        {
            string ret = "";
            if (num < 4)
            {
                while (num > 0)
                {
                    ret += romanValue[i];
                    num--;
                }
            }
            else if (num == 4)
            {
                ret = romanValue[i - 1] + romanValue[i];
            }
            else if (num == 9)
            {
                ret = romanValue[i - 2] + romanValue[i];
            }
            else
            {
                ret += romanValue[i];
                num -= 5;
                while (num > 0)
                {
                    ret += romanValue[i - 1];
                    num--;
                }
            }
            return ret;
        }

        //Simple soln beats 78.5% csharp soln
        public static string IntToRomanSimple(int num)
        {
            List<int> romanKey = new List<int> { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
            List<string> romanValue = new List<string> { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
            string ret = "";
            int i = 0;
            while (num > 0)
            {
                if (num >= romanKey[i])
                {
                    num -= romanKey[i];
                    ret += romanValue[i];
                }
                else
                {
                    i++;
                }
            }
            return ret;
        }
        /*
         Roman numerals are represented by seven different symbols: I, V, X, L, C, D and M.

            Symbol       Value
            I             1
            V             5
            X             10
            L             50
            C             100
            D             500
            M             1000
            For example, two is written as II in Roman numeral, just two one's added together. Twelve is written as, XII, which is simply X + II. The number twenty seven is written as XXVII, which is XX + V + II.

            Roman numerals are usually written largest to smallest from left to right. However, the numeral for four is not IIII. Instead, the number four is written as IV. Because the one is before the five we subtract it making four. The same principle applies to the number nine, which is written as IX. There are six instances where subtraction is used:

            I can be placed before V (5) and X (10) to make 4 and 9. 
            X can be placed before L (50) and C (100) to make 40 and 90. 
            C can be placed before D (500) and M (1000) to make 400 and 900.
            Given a roman numeral, convert it to an integer. Input is guaranteed to be within the range from 1 to 3999.

            Example 1:

            Input: "III"
            Output: 3
            Example 2:

            Input: "IV"
            Output: 4
            Example 3:

            Input: "IX"
            Output: 9
            Example 4:

            Input: "LVIII"
            Output: 58
            Explanation: L = 50, V= 5, III = 3.
            Example 5:

            Input: "MCMXCIV"
            Output: 1994
            Explanation: M = 1000, CM = 900, XC = 90 and IV = 4.

        Note*: Additional string/ip validation can be asked 
         */

        private static readonly string romanRegexPattern = @"^(M{0,4}(C[M|D]|D?C{0,3})(X[L|C]|L?X{0,3})(I[V|X]|V?I{0,3}))$";
        private static Dictionary<char, int> roman = new Dictionary<char, int>{
        {'I', 1},
        {'V', 5},
        {'X', 10},
        {'L', 50},
        {'C', 100},
        {'D',500},
        {'M', 1000}
    };
        public static int RomanToInt(string s)
        {
            //s = 'LXXVII'
            if (!Regex.IsMatch(s, romanRegexPattern)) throw new ArgumentException("Invalid roman numeral");
            int ret = roman[s[s.Length - 1]];//roman[s[5]], roman['I'] = 1, ret = 1
            int prev = roman[s[s.Length - 1]]; //prev = 1 
            for (int i = s.Length - 2; i >= 0; i--) // i = -1
            {
                int cur = roman[s[i]]; //s[0] = L, cur = roman[s[0]] , roman['L'] = 50
                if (cur < prev) // cur == prev
                {
                    ret -= cur;
                }
                else
                {
                    ret += cur; //ret += 50 , ret = 77
                }
                prev = cur; // prev = cur = 50
            }
            return ret;//77
        }

        //Pattern match
        /*
         Implement strStr().

            Return the index of the first occurrence of needle in haystack, or -1 if needle is not part of haystack.

            Example 1:

            Input: haystack = "hello", needle = "ll"
            Output: 2
            Example 2:

            Input: haystack = "aaaaa", needle = "bba"
            Output: -1
            Clarification:

            What should we return when needle is an empty string? This is a great question to ask during an interview.

            For the purpose of this problem, we will return 0 when needle is an empty string. This is consistent to C's strstr() and Java's indexOf().

 

            Constraints:

            haystack and needle consist only of lowercase English characters.
         */
        //brute force version
        public static int StrStrBruteForce(string haystack, string needle)
        {
            if (needle == "") return 0;
            return IndexOf(haystack, needle);
        }
        private static int IndexOf(string haystack, string needle)
        {

            List<int> sidxs = new List<int>();
            for (int i = 0; i < haystack.Length; i++)
            {
                if (haystack[i] == needle[0])
                {
                    sidxs.Add(i);
                }
            }

            for (int i = 0; i < sidxs.Count; i++)
            {
                int j = 1;
                int p = sidxs[i] + 1;
                while (j < needle.Length && p < haystack.Length && haystack[p] == needle[j])
                {
                    j++;
                    p++;
                }
                if (j == needle.Length)
                    return sidxs[i];
            }
            return -1;
        }

        //using Rabin Karp method
        public static int StrStrRabinKarp(string haystack, string needle)
        {
            if (needle == "") return 0;
            if (needle.Length > haystack.Length || haystack == "") return -1;
            var rk = new RabinKarp(needle);
            return rk.Search(haystack);
        }


        /*
         * Rotate Image
         
         You are given an n x n 2D matrix representing an image.

                Rotate the image by 90 degrees (clockwise).

                Note:

                You have to rotate the image in-place, which means you have to modify the input 2D matrix directly.
                DO NOT allocate another 2D matrix and do the rotation.

                Example 1:

                Given input matrix = 
                [
                  [1,2,3],
                  [4,5,6],
                  [7,8,9]
                ],

                rotate the input matrix in-place such that it becomes:
                [
                  [7,4,1],
                  [8,5,2],
                  [9,6,3]
                ]
         
         */
        public static void Rotate(int[][] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                //transpose the row
                for (int j = i; j < matrix[0].Length; j++)
                {
                    int temp = matrix[i][j];
                    matrix[i][j] = matrix[j][i];
                    matrix[j][i] = temp;
                }

                //reverse the row
                int k = matrix[0].Length - 1;
                int d = k / 2;
                for (int j = 0; j <= d; j++)
                {
                    int temp = matrix[i][k];
                    matrix[i][k] = matrix[i][j];
                    matrix[i][j] = temp;
                    k--;
                }
            }

        }
        /*
         Group Anagrams
         */

        public static List<List<string>> groupAnagrams(List<string> words)
        {
            // Write your code here.
            var dict = new Dictionary<string, List<string>>();
            foreach (var w in words)
            {
                var key = AnaHash(w);
                if (!dict.TryAdd(key, new List<string> { w }))
                {
                    dict[key].Add(w);
                }
            }
            return dict.Values.ToList();
        }


        private static string AnaHash(string word)
        {
            var alpha = new StringBuilder("#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0");
            foreach (var c in word)
            {
                alpha[2*(c - 'a') + 1] = Convert.ToChar((int)(Char.GetNumericValue(alpha[2 * (c - 'a') + 1]) + 1));
            }
            return alpha.ToString();
        }


        /*
          Minimum Substring Window
         */

        public static string MinWindow(string s, string t)
        {
            //dictionary is required because t may have duplicate chars, hence we have to
            //to know the counts of each char in t
            var tset = new Dictionary<char, int>();
            var tset1 = new Dictionary<char, int>();
            foreach (var ch in t)
            {
                if (!tset.ContainsKey(ch))
                {
                    tset.Add(ch, 0);
                    tset1.Add(ch, 0);
                }
                tset[ch]++;
            }

            int startIdx = 0;
            //set startIdx with first match
            while (!tset.ContainsKey(s[startIdx]))
            {
                startIdx++;
            }

            int c = t.Length;
            int minLen = Int32.MaxValue;
            int sidx = 0;
            int i = startIdx;
            while (i < s.Length)
            {
                var cur = s[i];
                if (tset.ContainsKey(cur))
                {
                    
                    if (tset1[cur] < tset[cur]) c--;
                    tset1[cur]++;
                    if (s[startIdx] == cur && tset1[cur] > tset[cur])
                    {
                        var m = s[startIdx];
                        while ((tset1.ContainsKey(m) && tset1[m] > tset[m]) ||
                              !tset1.ContainsKey(m))
                        {
                            if (tset1.ContainsKey(m)) tset1[m]--;
                            startIdx++;
                            m = s[startIdx];
                        }
                    }
                }
                if (c == 0)
                {
                    int curLen = i - startIdx + 1;
                    if (curLen < minLen)
                    {
                        sidx = startIdx;
                        minLen = curLen;
                    }
                }
                i++;
            }
            return minLen < Int32.MaxValue ? s.Substring(sidx, minLen) : "";
        }

        public static int CompareVersion(string version1, string version2)
        {
            var v1 = version1.Split(".");
            var v2 = version2.Split(".");
            int r = 0;
            int n = Math.Min(v1.Length, v2.Length);
            int i = 0;

            //note we should take this local function outside and mark it static due to performance issues
            Func<string, string, int> Compare = (a, b) => Int32.Parse(a).CompareTo(Int32.Parse(b));
            for (; i < n; i++)
            {
                r = Compare(v1[i], v2[i]);
                if (r != 0) return r;
            }
            if (v1.Length < v2.Length)
            {
                for (; i < v2.Length; i++)
                {
                    r = Compare("0", v2[i]);
                    if (r != 0) return r;
                }

            }
            else if (v1.Length > v2.Length)
            {
                for (; i < v1.Length; i++)
                {
                    r = Compare(v1[i], "0");
                    if (r != 0) return r;
                }

            }

            return r;
        }

        /*
         * Product of Array Except Self
         Given an array nums of n integers where n > 1,  return an array output such that output[i] is
        equal to the product of all the elements of nums except nums[i].

        Example:

        Input:  [1,2,3,4]
        Output: [24,12,8,6]
        Constraint: It's guaranteed that the product of the elements of any prefix or suffix of the
        array (including the whole array) fits in a 32 bit integer.

        Note: Please solve it without division and in O(n).

        Follow up:
        Could you solve it with constant space complexity? (The output array does not count as extra
        space for the purpose of space complexity analysis.)
                 
         */
        public static int[] ProductExceptSelf(int[] nums)
        {
            int n = nums.Length;
            int[] ret = new int[n];
            ret[n - 1] = nums[n - 1];
            for (int i = n - 2; i >= 0; i--)
            {
                ret[i] = ret[i + 1] * nums[i];
            }
            int left = 1;
            int p = nums[0];
            for (int i = 0; i < n - 1; i++)
            {
                ret[i] = left * ret[i + 1];
                left = left * nums[i];
            }
            ret[n - 1] = left;
            return ret;
        }

        /* Parity check brute force O(n) where n is numer of bits in x*/

       public static int Parity(int x)
        {
            int ret = 0;
            while(x > 0)
            {
                ret += (x & 1);
                x >>= 1;
            }

            return ret % 2;
        }

        /*Parity O(s) solution where s is the number of set bits or 1s in x*/
        public static int ParityS(int x)
        {
            int ret = 0;
            while (x > 0)
            {
                ret ^= 1;
                x &= x - 1;
            }

            return ret;
        }

        /* Parity O(log n) solution where n is the number of bits in x */
        //This solution takes advantage of XOR as it gives the parity of a group of bits

        public static long ParityXOR(long x)
        {
            x ^= x >> 32;
            x ^= x >> 16;
            x ^= x >> 8;
            x ^= x >> 4;
            x ^= x >> 2;
            x ^= x >> 1;

            return x & 1;
        }

        public static long ParityRec(long x, int wordSize)
        {
            if(wordSize == 1) x = x & 1;
            wordSize = wordSize / 2;
            ParityRec(x ^ (x >> wordSize), wordSize);
            return x;
        }

        public static int MissingNumber(int[] nums)
        {
            double naturalSum = ((nums.Length + 1) * nums.Length) / 2;
            return (int)(naturalSum - nums.Sum());

        }

        public static int MissingNumberXOR(int[] nums)
        {
            int missing = nums.Length;
            for (int i = 0; i < nums.Length; i++)
            {
                missing ^= i ^ nums[i];
            }
            return missing;
        }

        /*
         Convert a non-negative integer to its english words representation. Given input is guaranteed to be less than 231 - 1.
         */
        private static Dictionary<int, string> numStr = new Dictionary<int, string>{
            {0, ""},
            {1, "One"},
            {2, "Two"},
            {3, "Three"},
            {4,"Four"},
            {5,"Five"},
            {6,"Six"},
            {7,"Seven"},
            {8,"Eight"},
            {9,"Nine"},
            {10,"Ten"},
            {11,"Eleven"},
            {12,"Twelve"},
            {13,"Thirteen"},
            {14,"Fourteen"},
            {15,"Fifteen"},
            {16,"Sixteen"},
            {17,"Seventeen"},
            {18,"Eighteen"},
            {19,"Nineteen"},
            {20,"Twenty"},
            {30,"Thirty"},
            {40,"Forty"},
            {50,"Fifty"},
            {60,"Sixty"},
            {70, "Seventy"},
            {80,"Eighty"},
            {90,"Ninety"}
        };
        public static string NumberToWords(int num)
        {
            if (num == 0) return "Zero";
            int x = num;
            var ret = "";
            if (x >= 1000000000)
            {
                var x1 = x / 1000000000;
                ret = numStr[x1] + " Billion";
                x -= (x1 * 1000000000);
            }
            if (x >= 1000000)
            {
                var x1 = (decimal)(x / 1000000m);
                ret += " " + GetHundredUnitString((int)x1) + " Million";
                x = (int)((x1 - (int)x1) * 1000000);
            }
            if (x >= 1000)
            {
                var x1 = (decimal)(x / 1000m);
                ret += " " + GetHundredUnitString((int)x1) + " Thousand";
                x = (int)((x1 - (int)x1) * 1000);
            }
            return (ret + " " + GetHundredUnitString(x)).Trim();
        }

        private static string GetHundredUnitString(int x)
        {
            string ret = "";
            if (x >= 100)
            {
                var x1 = (decimal)x / 100;
                ret += " " + numStr[(int)x1] + " Hundred";
                x = (int)((x1 - (int)x1) * 100);
            }
            if (x > 20)
            {
                var x1 = (decimal)x / 10;
                ret += " " + numStr[(int)x1 * 10];
                x = (int)((x1 - (int)x1) * 10);

            }

            return (ret + " " + numStr[x]).Trim();
        }

        public static int Trap(int[] height)
        {
            int sum = 0;
            int n = height.Length;
            if (n < 3) return sum;
            int[] leftMax = new int[height.Length];
            int[] rightMax = new int[height.Length];
            leftMax[0] = height[0];
            rightMax[n - 1] = height[n - 1];
            int i = 1;
            int j = n - 2;
            for (; i < n; i++, j--)
            {
                leftMax[i] = Math.Max(leftMax[i - 1], height[i]);
                rightMax[j] = Math.Max(rightMax[j + 1], height[j]);
            }
            for (i = 1; i < n - 1; i++)
            {
                sum += Math.Min(leftMax[i], rightMax[i]) - height[i];
            }
            return sum;
        }

        /*
         * Add Two Numbers represented as linked list
         You are given two non-empty linked lists representing two non-negative integers.
         The digits are stored in reverse order and each of their nodes contain a single digit.
         Add the two numbers and return it as a linked list.

            You may assume the two numbers do not contain any leading zero, except the number 0 itself.

            Example:

            Input: (2 -> 4 -> 3) + (5 -> 6 -> 4)
            Output: 7 -> 0 -> 8
            Explanation: 342 + 465 = 807.
         */
        public static ListNode AddTwoNumbers(ListNode l1, ListNode l2)
        {
            int carry = 0;
            var curNode = new ListNode();
            var ret = curNode;
            while (true)
            {
                var val = l1.val + l2.val + carry;
                if (val > 9)
                {
                    carry = 1;
                    curNode.val = val - 10;
                }
                else
                {
                    carry = 0;
                    curNode.val = val;
                }
                l1 = l1.next;
                l2 = l2.next;
                if (l1 == null || l2 == null)
                {
                    break;
                }
                curNode.next = new ListNode();
                curNode = curNode.next;
            }
            if (l1 != null || l2 != null)
            {
                var node = l1 == null ? l2 : l1;
                do
                {
                    var val = node.val + carry;
                    if (val > 9)
                    {
                        val -= 10;
                        carry = 1;
                    }
                    else
                        carry = 0;
                    curNode.next = new ListNode(val);
                    curNode = curNode.next;
                    node = node.next;
                } while (node != null);
            }

            if (carry == 1)
            {
                curNode.next = new ListNode(1);
            }

            return ret;
        }

        /*
         Merge two sorted linked lists and return it as a new sorted list.
        The new list should be made by splicing together the nodes of the first two lists.

            Example:

            Input: 1->2->4, 1->3->4
            Output: 1->1->2->3->4->4
         */
        public static ListNode MergeTwoLists(ListNode l1, ListNode l2)
        {
            if (l1 == null) return l2;
            if (l2 == null) return l1;
            ListNode ret = UpdateListNode(ref l1, ref l2);
            var curNode = ret;
            while (l1 != null && l2 != null)
            {
                curNode.next = UpdateListNode(ref l1, ref l2);
                curNode = curNode.next;
            }
            curNode.next = l1 == null ? l2 : l1;
            return ret;
        }
        private static ListNode UpdateListNode(ref ListNode l1, ref ListNode l2)
        {
            ListNode ret = null;
            if (l1.val <= l2.val)
            {
                ret = l1;
                l1 = l1.next;
            }
            else
            {
                ret = l2;
                l2 = l2.next;
            }
            return ret;
        }

        /*
         * Reverse Nodes in k-Group
         Given a linked list, reverse the nodes of a linked list k at a time and return its modified list.

            k is a positive integer and is less than or equal to the length of the linked list.
                    If the number of nodes is not a multiple of k then left-out nodes in the end should remain as it is.

            Example:

            Given this linked list: 1->2->3->4->5

            For k = 2, you should return: 2->1->4->3->5

            For k = 3, you should return: 3->2->1->4->5

            Note:

            Only constant extra memory is allowed.
            You may not alter the values in the list's nodes, only nodes itself may be changed.
         */

        public static ListNode ReverseKGroup(ListNode head, int k)
        {
            if (head == null) return head;
            int listLen = 1;
            var cur = head;
            while (cur.next != null)
            {
                cur = cur.next;
                listLen++;
            }

            int groups = listLen / k;
            int i = k;
            var list = head;
            head = null;
            var tail = list;
            if (groups > 0)
            {
                cur = null;
                while (i > 0)
                {
                    Swap(ref cur, ref list);
                    i--;
                }

                groups --;
                head = cur;
                cur = null;
                i = k;
            }

            var prevTail = tail;
            tail = list;
            while (groups > 0)
            {
                while (i > 0)
                {
                    Swap(ref cur, ref list);
                    i--;
                }
                groups --;
                i = k;
                prevTail.next = cur;
                prevTail = tail;
                tail = list;
                cur = null;
            }

            prevTail.next = tail;
            return head;
        }
        // h = null t = 1 -> 2 -> 3 -> 4 -> 5 ...
        public static void Swap(ref ListNode h, ref ListNode t)
        {
            var temp = t.next;
            t.next = h;
            h = t;
            t = temp;
        }

        /*
         Copy List with Random Pointer

        A linked list is given such that each node contains an additional random pointer which could point to any node in the list or null.

        Return a deep copy of the list.

        The Linked List is represented in the input/output as a list of n nodes. Each node is represented as a pair of [val, random_index] where:

        val: an integer representing Node.val
        random_index: the index of the node (range from 0 to n-1) where random pointer points to, or null if it does not point to any node.
 

        Example 1:


        Input: head = [[7,null],[13,0],[11,4],[10,2],[1,0]]
        Output: [[7,null],[13,0],[11,4],[10,2],[1,0]]
        Example 2:


        Input: head = [[1,1],[2,1]]
        Output: [[1,1],[2,1]]
        Example 3:



        Input: head = [[3,null],[3,0],[3,null]]
        Output: [[3,null],[3,0],[3,null]]
        Example 4:

        Input: head = []
        Output: []
        Explanation: Given linked list is empty (null pointer), so return null.
 

        Constraints:

        -10000 <= Node.val <= 10000
        Node.random is null or pointing to a node in the linked list.
        Number of Nodes will not exceed 1000.
         */

        //this solution uses dictionary, other solution without using dictionary
        //intertwinning the new nodes with old i.e. oldnode1 -> newNode1 ->oldNode2 -> newNode2 ...
        public static RandomNode CopyRandomList(RandomNode head)
        {
            if (head == null) return null;
            Dictionary<RandomNode, RandomNode> dict = new Dictionary<RandomNode, RandomNode>();
            RandomNode ret = new RandomNode(head.val);
            var node = ret;
            var cur = head;
            dict.Add(cur, node);
            while (cur.next != null)
            {
                node.next = new RandomNode(cur.next.val);
                dict.Add(cur.next, node.next);
                node = node.next;
                cur = cur.next;
            }
            cur = head;
            node = ret;
            while (cur != null)
            {
                if (cur.random != null)
                    node.random = dict[cur.random];
                cur = cur.next;
                node = node.next;
            }

            return ret;
        }

        //without using hashtable
        public static RandomNode CopyRandomListIISol(RandomNode head)
        {
            if (head == null) return null;
            var oldNode = head;
            while (oldNode != null)
            {
                var newNode = new RandomNode(oldNode.val);
                newNode.next = oldNode.next;
                oldNode.next = newNode;
                oldNode = newNode.next;
            }
            oldNode = head;
            while (oldNode != null)
            {
                var newNode = oldNode.next;
                if (oldNode.random != null)
                    newNode.random = oldNode.random.next;
                oldNode = newNode.next;

            }

            oldNode = head;
            var ret = head.next;
            while (oldNode != null)
            {
                var newNode = oldNode.next;
                var nextOldNode = newNode.next;
                oldNode.next = nextOldNode;
                RandomNode nextNewNode = null;
                if (nextOldNode != null)
                    nextNewNode = nextOldNode.next;
                newNode.next = nextNewNode;
                oldNode = nextOldNode;
            }
            return ret;
        }

        //Merge K sorted lists this soln is O(nlogn)
        public static ListNode MergeKLists(ListNode[] lists)
        {
            SortedDictionary<int, Queue<ListNode>> minH = new SortedDictionary<int, Queue<ListNode>>();
            int i = 0;
            while(i < lists.Length)
            {
                var cur = lists[i];
                while (cur != null)
                {
                    if(minH.ContainsKey(cur.val))
                    {
                        minH[cur.val].Enqueue(cur);
                    }
                    else
                    {
                        Queue<ListNode> q = new Queue<ListNode>();
                        q.Enqueue(cur);
                        minH.Add(cur.val, q);
                    }
                    cur = cur.next;
                }
                i++;
            }

            ListNode dummy = new ListNode(-1);
            var head = dummy;
            foreach(var k in minH.Keys)
            {
                var q = minH[k];
                while(q.Count > 0)
                {
                    head.next = q.Dequeue();
                    head = head.next;
                }
            }

            return dummy.next;

        }
        /*
            Given an array of linked-lists lists, each linked list is sorted in ascending order.

            Merge all the linked-lists into one sort linked-list and return it.

 

            Example 1:

            Input: lists = [[1,4,5],[1,3,4],[2,6]]
            Output: [1,1,2,3,4,4,5,6]
            Explanation: The linked-lists are:
            [
                1->4->5,
                1->3->4,
                2->6
            ]
            merging them into one sorted list:
            1->1->2->3->4->4->5->6
            Example 2:

            Input: lists = []
            Output: []
            Example 3:

            Input: lists = [[]]
            Output: []
 

            Constraints:

            k == lists.length
            0 <= k <= 10^4
            0 <= lists[i].length <= 500
            -10^4 <= lists[i][j] <= 10^4
            lists[i] is sorted in ascending order.
            The sum of lists[i].length won't exceed 10^4.
         */
        //this sol we will use k pointers in a minHeap
        public static ListNode MergeKListsII(ListNode[] lists)
        {
            SortedDictionary<int, Queue<ListNode>> minH = new SortedDictionary<int, Queue<ListNode>>();
            int i = 0;
            while (i < lists.Length)
            {
                var cur = lists[i];

                if (minH.ContainsKey(cur.val))
                {
                    minH[cur.val].Enqueue(cur);
                }
                else
                {
                    Queue<ListNode> q = new Queue<ListNode>();
                    q.Enqueue(cur);
                    minH.Add(cur.val, q);
                }
                   
                i++;
            }

            ListNode dummy = new ListNode(-1);
            var head = dummy;
            while(minH.Count > 0)
            {
                var min = minH.First();
                head.next = min.Value.Dequeue();
                if(min.Value.Count == 0)
                {
                    minH.Remove(min.Key);
                }
                var next = head.next.next;
                if (next != null) {
                    if (minH.ContainsKey(next.val))
                    {
                        minH[next.val].Enqueue(next);
                    }
                    else
                    {
                        Queue<ListNode> q = new Queue<ListNode>();
                        q.Enqueue(next);
                        minH.Add(next.val, q);
                    }
                }
                
                head = head.next;
            }
            return dummy.next;
        }

        /*
         * Letter Combinations of a Phone Number
         Given a string containing digits from 2-9 inclusive, return all possible letter combinations that the number could represent.

        A mapping of digit to letters (just like on the telephone buttons) is given below. Note that 1 does not map to any letters.

        Example:

        Input: "23"
        Output: ["ad", "ae", "af", "bd", "be", "bf", "cd", "ce", "cf"].
        Note:

        Although the above answer is in lexicographical order, your answer could be in any order you want.
         */
        //this is a recursive solution with O(4^n)
        private static Dictionary<char, string> nmeumo = new Dictionary<char, string>{
            {'2', "abc"},
            {'3', "def"},
            {'4', "ghi"},
            {'5', "jkl"},
            {'6', "mno"},
            {'7', "pqrs"},
            {'8', "tuv"},
            {'9', "wxyz"}
        };
        public static IList<string> LetterCombinations(string digits)
        {
            var ret = new List<string>();
            if (digits == "") return ret;
            LetterPermRec("", ref ret, digits, 0);
            return ret;
        }

        private static void LetterPermRec(string cur, ref List<string> ret, string d, int idx)
        {

            if (idx > d.Length - 1)
            {
                ret.Add(cur);
                return;
            }

            //if(!nmeumo.ContainsKey(d[idx])) return;
            var curDigitLetters = nmeumo[d[idx]];
            for (int i = 0; i < curDigitLetters.Length; i++)
            {
                LetterPermRec(cur + curDigitLetters[i].ToString(), ref ret, d, idx + 1);
            }
        }


        public static IList<string> GenerateParenthesisII(int n)
        {
            IList<string> ret = new List<string>();
            if (n == 0) return ret;
            GenerateParenRec(n, n, "", ref ret);
            return ret;
        }

        private static void GenerateParenRec(int open, int close, string cur, ref IList<string> ret)
        {
            if (open == 0 && close == 0)
            {
                ret.Add(cur);
                return;
            }

            if (open == close || open > 0)
            {
                GenerateParenRec(open - 1, close, cur + "(", ref ret);
            }
            if (open < close)
            {
                GenerateParenRec(open, close - 1, cur + ")", ref ret);
            }

        }

        /*
         * Median of Two Sorted Arrays
         
            There are two sorted arrays nums1 and nums2 of size m and n respectively.

            Find the median of the two sorted arrays. The overall run time complexity should be O(log (m+n)).

            You may assume nums1 and nums2 cannot be both empty.

            Example 1:

            nums1 = [1, 3]
            nums2 = [2]

            The median is 2.0
            Example 2:

            nums1 = [1, 2]
            nums2 = [3, 4]

            The median is (2 + 3)/2 = 2.5
         */
        //This is deceptively tough to implement too many egde cases
        public static double FindMedianSortedArrays(int[] nums1, int[] nums2)
        {
            if (nums1.Length == 0 && nums2.Length == 0) return 0;
            var num = Merge(ref nums1, ref nums2);
            int k = num.Length - 1;
            return num.Length % 2 > 0 ? num[k / 2] : (double)(num[k / 2] + num[k / 2 + 1]) / 2;
        }

        private static int[] Merge(ref int[] a, ref int[] b)
        {
            int m = a.Length;
            int n = b.Length;
            int[] ret = new int[m + n];
            int i = 0; int j = 0; int k = 0;
            while (k < ret.Length)
            {
                if (i < m && (j >= n || a[i] <= b[j]))
                {
                    ret[k] = a[i];
                    i++;
                }
                else if (j < n && (i >= m || a[i] > b[j]))
                {
                    ret[k] = b[j];
                    j++;
                }
                k++;
            }

            return ret;
        }

        /*
         * Search in Rotated Sorted Array
         * 
         * Given an integer array nums sorted in ascending order, and an integer target.

            Suppose that nums is rotated at some pivot unknown to you beforehand (i.e., [0,1,2,4,5,6,7] might become [4,5,6,7,0,1,2]).

            You should search for target in nums and if you found return its index, otherwise return -1.
         */

        public static int RotatedSearch(int[] nums, int target)
        {
            return RotatedSearchRec(nums, 0, nums.Length - 1, target);
        }

        private static int RotatedSearchRec(int[] nums, int start, int end, int target)
        {
            if (start > end)
            {
                return -1;
            }

            int mid = (start + end) / 2;
            if (nums[mid] == target) return mid;

            //outer check is if left half is increasing  
            if (nums[mid] >= nums[start])
            {
                //checking if target falls in the left increasing half
                if (target >= nums[start] && target < nums[mid])
                    return RotatedSearchRec(nums, start, mid - 1, target);
                //else it has to be in the right half which is not strictly increasing
                else
                    return RotatedSearchRec(nums, mid + 1, end, target);
            }
            //else right half is increasing 
            else
            {
                //checking if target falls in the right increasing half
                if (target > nums[mid] && target <= nums[end])
                    return RotatedSearchRec(nums, mid + 1, end, target);
                //else it has to be in the left half which is not strictly increasing
                else
                    return RotatedSearchRec(nums, start, mid - 1, target);
            }

        }

        public static int[][] MergeInervals(int[][] intervals)
        {
            if (intervals.Length < 2) return intervals;
            var ret = new List<int[]>();
            int j = 0;
            Array.Sort(intervals, Compare);
            ret.Add(new int[] { intervals[0][0], intervals[0][1] });
            for (int i = 1; i < intervals.Length; i++)
            {
                if (ret[j][1] >= intervals[i][0])
                {
                    ret[j][0] = Math.Min(intervals[i][0], ret[j][0]);
                    ret[j][1] = Math.Max(intervals[i][1], ret[j][1]);

                }
                else
                {
                    ret.Add(new int[] { intervals[i][0], intervals[i][1] });
                    j++;
                }
            }

            return ret.ToArray();
        }

        private static int Compare(int[] a, int[] b)
        {
            if (a[0] > b[0]) return 1;
            else if (a[0] < b[0]) return -1;
            else return 0;
        }

        //Find the Longest Palindromic Substring
        //i:p "abaattaa" o:p "aattaa"  
        //O(N2) avg case O(2MN) where N is the length of the string and M is length of substring
        public static string LongestPalindromicSubstring(string str)
        {
            if (str == "") return str;
            int startIdx = 0;
            int pLen = 1;
            for (int i = 1; i < str.Length; i++)
            {
                var odd = GetLongestPalindromFrom(str, i - 1, i + 1);
                var even = GetLongestPalindromFrom(str, i - 1, i);
                if (odd[1] > even[1] && odd[1] > pLen)
                {
                    startIdx = odd[0];
                    pLen = odd[1];
                }
                else if (even[1] > odd[1] && even[1] > pLen)
                {
                    startIdx = even[0];
                    pLen = even[1];
                }
            }
            return str.Substring(startIdx, pLen);
        }

        private static int[] GetLongestPalindromFrom(string s, int i, int j)
        {

            while (i >= 0 && j < s.Length && s[i] == s[j])
            {
                i--; j++;
            }
            /*
                After the while loop is done, on a match i would be one less than the start index of the palindrom
                and j would be one more than the end index of the palindrom hence i + 1 will give us the
                start index of the palindrom and j - i - 1 will give the length of the palindrom the substring.
                This format is readily accepted by String.Substring method which takes the start index(zero-based)
                of the substring and length of the substring.
             */
            return new int[] { i + 1, j - i - 1 };
        }

        /*
          Find the Maximum Sum of adjacent items in a non empty array
         i/p: [3, 5, -9, 1, 3, -2, 3, 4, 7, 2, -9, 6, 3, 1, -5, 4]
         o/p: 19 i.e. sum of [1, 3, -2, 3, 4, 7, 2, -9, 6, 3, 1]
         */
        //Applying Kadanes Algorithm by computing the running sum and comparing the current value and taking the maximum of both.
        //runs in O(N)
        public static int KadanesAlgorithm(int[] array)
        {
            int maxSum = array[0];
            int currentSum = array[0];
            int i = 1;
            while (i < array.Length)
            {
                currentSum += array[i];
                currentSum = System.Math.Max(currentSum, array[i]);
                maxSum = System.Math.Max(maxSum, currentSum);
                i++;
            }
            return maxSum;
        }

        /*
         * Remove Kth from the last in a LinkedList
         */
        //using 2 pointer technique which is the fastest of all solutions O(N) time & O(1) space
        public static void RemoveKthNodeFromEnd(ListNode head, int k)
        {
            // Write your code here.
            
            var p = head;
            var q = head;
            ListNode pPrev = null;
            while (k > 0)
            {
                q = q.next;
                k--;
            }
            while (q != null)
            {
                pPrev = p;
                p = p.next;
                q = q.next;
            }
            if (p == head)
            {
                var next = head.next;
                head.val = next.val;
                head.next = next.next;
                next.next = null;
            }
            else
            {
                pPrev.next = p.next;
                p.next = null;
            }
        }

        //This code takes O(RlogC)
        public static int[] SearchInSortedMatrix(int[,] matrix, int target)
        {
            // Write your code here.
            var ret = "";
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                ret = BstArr(matrix, target, i, 0, matrix.GetLength(1) - 1);
                if (ret != "")
                    return new int[] { (int)(ret[0] - '0'), (int)(ret[1] - '0') };
            }
            return new int[] { -1, -1 };
        }
        private static string BstArr(int[,] arr, int t, int k, int i, int j)
        {
            if (i > j)
            {
                return "";
            }

            int mid = (i + j) / 2;
            if (arr[k, mid] == t) return $"{k}{mid}";
            if (t < arr[k, mid])
                return BstArr(arr, t, k, i, mid - 1);
            else
            {
                return BstArr(arr, t, k, mid + 1, j);
            }
        }
        //This is solution is the best with O(R + C) Time and O(1) Space 
        public static int[] SearchInSortedMatrixFastest(int[,] matrix, int target)
        {
            // Write your code here.
            int r = 0;
            int c = matrix.GetLength(1) - 1;
            while (r < matrix.GetLength(0) && c > -1)
            {
                if (matrix[r, c] < target)
                    r++;
                else if (matrix[r, c] > target)
                    c--;
                else
                    return new int[] { r, c };
            }
            return new int[] { -1, -1 };
        }


        public static List<List<int>> MaxSumIncreasingSubsequence(int[] array)
        {
            // Write your code here.
            int maxSum = Int32.MinValue;
            List<int> maxSeq = null;
            if (array.Length == 0)
                return new List<List<int>>() { new List<int>() { -1 }, new List<int>() };
            for (int i = 0; i < array.Length; i++)
            {
                var curSum = 0;
                var curSeq = new List<int>();
                for (int j = 0; j < array.Length; j++)
                {
                    if (j == i)
                    {
                        curSum += array[j];
                        curSeq.Add(array[j]);
                    }
                    else if (j < i && array[j] < array[i] && array[j] > 0)
                    {
                        if(curSeq.Count == 0 || curSeq.Count > 0 && curSeq[curSeq.Count - 1] < array[j])
                        curSum += array[j];
                        curSeq.Add(array[j]);
                    }
                    else if (j > i && curSeq[curSeq.Count - 1] < array[j])
                    {
                        curSum += array[j];
                        curSeq.Add(array[j]);
                    }
                }
                if (curSum > maxSum)
                {
                    maxSum = curSum;
                    maxSeq = curSeq;
                }
            }

            return new List<List<int>> { new List<int> { maxSum }, maxSeq };
        }

        /*
         MeetingRoom II
         Given an array of meeting time intervals consisting of start and end times [[s1,e1],[s2,e2],...] (si < ei), find the minimum number of conference rooms required.

            Example 1:

            Input: [[0, 30],[5, 10],[15, 20]]
            Output: 2
            Example 2:

            Input: [[7,10],[2,4]]
            Output: 1
         */
        public static int MinMeetingRooms(int[][] intervals)
        {
            if (intervals == null || intervals.Length == 0) return 0;
            Array.Sort(intervals, (int[] a, int[] b) => { return a[0] - b[0]; });
            var minHeap = new SortedDictionary<int, int>();
            int ret = 1;
            minHeap.Add(intervals[0][1], 1);
            for (int i = 1; i < intervals.Length; i++)
            {
                var cur = intervals[i];
                var prev = minHeap.First().Key;
                if (minHeap[prev] > 1) minHeap[prev]--;
                else minHeap.Remove(prev);
                if (cur[0] >= prev)
                {
                    prev = cur[1];
                }
                else
                {
                    ret++;
                    if (!minHeap.TryAdd(cur[1], 1))
                        minHeap[cur[1]]++;
                }
                if (!minHeap.TryAdd(prev, 1))
                    minHeap[prev]++;
            }

            return ret;
        }

        public static int[][] KClosestPointsFromOrigin(int[][] points, int K)
        {
            var h = new SortedDictionary<double, LinkedList<int[]>>();
            var k = 0;
            while (k < K)
            {
                var edist = EDist(points[k][0], points[k][1]);
                if (h.ContainsKey(edist))
                    h[edist].AddLast(points[k]);
                else
                {
                    var curList = new LinkedList<int[]>();
                    curList.AddLast(points[k]);
                    h.Add(edist, curList);
                }
                k++;
            }
            for (int i = K; i < points.Length; i++)
            {
                var top = h.Last();
                var curEDist = EDist(points[i][0], points[i][1]);
                if (curEDist < top.Key)
                {
                    if (top.Value.Count > 1)
                        top.Value.RemoveLast();
                    else
                        h.Remove(top.Key);

                    var curList = new LinkedList<int[]>();
                    curList.AddLast(points[i]);
                    if (!h.TryAdd(curEDist, curList))
                        h[curEDist].AddLast(points[i]);
                }
                else if(curEDist == top.Key)
                {
                    h[curEDist].AddLast(points[i]);
                }
            }

            int[][] kClosest = new int[K][];
            k = 0;
            while (k < K)
            {
                var last = h.First();
                kClosest[k] = last.Value.First.Value;
                if (last.Value.Count > 1)
                    last.Value.RemoveFirst();
                else
                    h.Remove(last.Key);
                k++;
            }

            return kClosest;
        }

        private static double EDist(int x, int y)
        {
            //Sqrt((x2 - x1)^2 + (y2 - y1)^2)
            return Math.Sqrt((x * x) + (y * y));
        }

        /*Quick Sort*/
        public static int[] QuickSort(int[] array)
        {
            // Write your code here.
            QuickSortHelper(array, 0, array.Length - 1);
            return array;
        }

        private static void QuickSortHelper(int[] arr, int startIdx, int endIdx)
        {
            if (startIdx >= endIdx) return;

            int pivotIdx = startIdx;
            int rightIdx = endIdx;
            int leftIdx = startIdx + 1;
            while (leftIdx <= rightIdx)
            {
                if (arr[leftIdx] > arr[pivotIdx] && arr[pivotIdx] > arr[rightIdx])
                {
                    //swap
                    Swap(arr, leftIdx, rightIdx);
                }
                if (arr[pivotIdx] >= arr[leftIdx])
                {
                    leftIdx++;
                }
                if (arr[pivotIdx] <= arr[rightIdx])
                {
                    rightIdx--;
                }
            }
            Swap(arr, pivotIdx, rightIdx);
            bool leftSubarrayIsSmaller = rightIdx - 1 - startIdx < endIdx - (rightIdx + 1);
            if (leftSubarrayIsSmaller)
            {
                QuickSortHelper(arr, startIdx, rightIdx - 1);
                QuickSortHelper(arr, rightIdx + 1, endIdx);
            }
            else
            {
                QuickSortHelper(arr, rightIdx + 1, endIdx);
                QuickSortHelper(arr, startIdx, rightIdx - 1);
            }
        }

        /*Quick Select Algo: Find the kth smallest*/
        public static int Quickselect(int[] array, int k)
        {
            // Write your code here.
            QuickSelectHelper(array, 0, array.Length - 1, k);
            return array[k - 1];
        }

        private static void QuickSelectHelper(int[] arr, int startIdx, int endIdx, int k)
        {
            if (startIdx >= endIdx) return;
            int rightIdx = endIdx;
            int leftIdx = startIdx + 1;
            int pivotIdx = startIdx;
            while (leftIdx <= rightIdx)
            {
                if (arr[leftIdx] > arr[pivotIdx] && arr[pivotIdx] > arr[rightIdx])
                {
                    Swap(arr, leftIdx, rightIdx);
                }
                if (arr[leftIdx] <= arr[pivotIdx]) leftIdx++;
                if (arr[rightIdx] >= arr[pivotIdx]) rightIdx--;
            }
            Swap(arr, pivotIdx, rightIdx);
            if (rightIdx == k - 1) return;
            if (rightIdx > k - 1)
                QuickSelectHelper(arr, startIdx, rightIdx - 1, k);
            else
                QuickSelectHelper(arr, rightIdx + 1, endIdx, k);
        }

        /*
         Given a string, determine if a permutation of the string could form a palindrome.

            Example 1:

            Input: "code"
            Output: false
            Example 2:

            Input: "aab"
            Output: true
            Example 3:

            Input: "carerac"
            Output: true
         */
        public static bool CanPermutePalindrome(string s)
        {
            var set = new HashSet<int>();
            for (int i = 0; i < s.Length; i++)
            {
                if (set.Contains(s[i])) set.Remove(s[i]);
                else set.Add(s[i]);
            }

            return set.Count <= 1;
        }

        //O(N) time and O(N) space
        public static List<int> SpiralTraverse(int[,] array)
        {
            // Write your code here.
            var retval = new List<int>();
            var rsize = array.GetLength(0);
            var csize = array.GetLength(1);
            int startRow = 0;
            int startCol = 0;
            int endRow = rsize - 1;
            int endCol = csize - 1;
            while (retval.Count < rsize * csize)
            {
                ProcessPerimeter(ref array, ref retval, startRow, endRow, startCol, endCol);
                startRow++;
                startCol++;
                endRow--;
                endCol--;
            }
            return retval;
        }

        private static void ProcessPerimeter(ref int[,] array, ref List<int> retval, int startRow, int endRow, int startCol, int endCol)
        {
            //left
            for (int j = startCol; j <= endCol; j++)
            {
                retval.Add(array[startRow, j]);
            }
            //down
            for (int j = startRow + 1; j <= endRow; j++)
            {
                retval.Add(array[j, endCol]);
            }
            //right
            if (startRow < endRow)
            {
                for (int j = endCol - 1; j >= startCol; j--)
                {
                    retval.Add(array[endRow, j]);
                }
            }

            //up
            if (startCol < endCol)
            {
                for (int j = endRow - 1; j > startRow; j--)
                {
                    retval.Add(array[j, startCol]);
                }
            }

        }
        //end of class
    }
    //end of namespace

}