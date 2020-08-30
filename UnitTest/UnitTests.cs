using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeetSolutions.Algos.LRU;
using LeetSolutions.Algos.MapSum;
using NUnit.Framework;
using LeetSolutions.Algos;
using static LeetSolutions.Algos.Solution;
using System.Diagnostics;

namespace LeetSolutions.UnitTest
{
    public class Tests
    {
        // [SetUp]
        // public void Setup()
        // {
        // }

        [Test]
        public void PermutationTest()
        {
            var ip = new int[] { 1, 2, 3 };
            var result = Solution.GetListOfPermutationsFor(ip);
            Assert.True(result.Count == 6);
        }

        // [Test]
        // public void TopKFreqTest()
        // {
        //     var ip = new int[]{5, 6, 7, 7, 1, 1, 1, 3, 3, 1, 7};
        //     var result = Solution.TopKElementsByFreq(ip, 2);
        //     Assert.Equals(result[0], 7);
        //     Assert.Equals(result[1], 1);
        // }

        /*
        Input:
            k = 2
            keywords = ["anacell", "betacellular", "cetracular", "deltacellular", "eurocell"]
            reviews = [
              "I love anacell Best services; Best services provided by anacell",
              "betacellular has great services",
              "deltacellular provides much better services than betacellular",
              "cetracular is worse than anacell",
              "Betacellular is better than deltacellular."
            ]

         Output:
            ["betacellular", "anacell"]

            Explanation:
            "betacellular" is occuring in 3 different reviews. "anacell" and "deltacellular" are
            occuring in 2 reviews, but "anacell" is lexicographically smaller.
         */
        // [Test]
        //  public void TopKFreqWords()
        // {
        //     int k = 2;
        //     var keywords = new List<string> { "anacell", "betacellular", "cetracular", "deltacellular", "eurocell" };
        //     var reviews = new List<string> { "I love anacell Best services; Best services provided by anacell",
        //       "betacellular has great services",
        //       "deltacellular provides much better services than betacellular",
        //       "cetracular is worse than anacell",
        //       "Betacellular is better than deltacellular."};
        //     var result = Solution.TopKFrequentWords(reviews, keywords, k);
        //     var expected = new string[] { "betacellular", "anacell" };
        //     Assert.AreEqual(expected.Length, result.Length);
        //     Assert.True(expected[0] == result[0]);
        //     Assert.True(expected[1] == result[1]);
        // }

        [Test]
        public void IsValidSudokoTest()
        {
            char[,] invalidip = new char[9, 9]{
                  {'8','3','.','.','7','.','.','.','.'},
                  {'6','.','.','1','9','5','.','.','.'},
                  {'.','9','8','.','.','.','.','6','.'},
                  {'8','.','.','.','6','.','.','.','3'},
                  {'4','.','.','8','.','3','.','.','1'},
                  {'7','.','.','.','2','.','.','.','6'},
                  {'.','6','.','.','.','.','2','8','.'},
                  {'.','.','.','4','1','9','.','.','5'},
                  {'.','.','.','.','8','.','.','7','9'}
                };
            Assert.False(Solution.IsValidSudoku(invalidip));

            char[,] validip = new char[9, 9]{
                  {'5','3','.','.','7','.','.','.','.'},
                  {'6','.','.','1','9','5','.','.','.'},
                  {'.','9','8','.','.','.','.','6','.'},
                  {'8','.','.','.','6','.','.','.','3'},
                  {'4','.','.','8','.','3','.','.','1'},
                  {'7','.','.','.','2','.','.','.','6'},
                  {'.','6','.','.','.','.','2','8','.'},
                  {'.','.','.','4','1','9','.','.','5'},
                  {'.','.','.','.','8','.','.','7','9'}
                };
            Assert.True(Solution.IsValidSudoku(validip));
        }

        [Test]
        public void ZombifyTest()
        {
            var ip = new int[,]{{0, 1, 1, 0, 1},
                                {0, 1, 0, 1, 0},
                                {0, 0, 0, 0, 1},
                                {0, 1, 0, 0, 0}};
            var result = Solution.MinHours(ip);
            Assert.AreEqual(2, result);
            ip = new int[,]{{1, 0, 0},
                            {0, 0, 0},
                            {0, 0, 0}};
            result = Solution.MinHours(ip);

            Assert.AreEqual(4, result);

            ip = new int[,]{{0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0}};
            result = Solution.MinHours(ip);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void RottenOrangesTest()
        {
            var ip = new int[,]{{2, 1, 1},
                                {1, 1, 0},
                                {0, 1, 1}};
            var result = Solution.OrangesRotting(ip);
            Assert.AreEqual(4, result);
            ip = new int[,]{{2, 1, 1},
                            {0, 1, 1},
                            {1, 0, 1}};
            result = Solution.OrangesRotting(ip);

            Assert.AreEqual(-1, result);

            ip = new int[,]{{0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0}};
            result = Solution.OrangesRotting(ip);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void ConnectedRoutersTest()
        {
            int[,] ip = new int[,] { { 0, 1 }, { 0, 2 }, { 1, 3 }, { 2, 3 }, { 2, 5 }, { 5, 6 }, { 3, 4 } };
            var result = Solution.CriticalRouter(7, 7, ip);
            var expectedSet = new HashSet<int> { 2, 3, 5 };
            Assert.True(result.Count == 3);
            foreach (var r in result)
            {
                Assert.True(expectedSet.Contains(r));
            }

            ip = new int[,] { { 0, 1 } };
            result = Solution.CriticalRouter(2, 1, ip);
            Assert.True(result.Count == 0);
        }

        [Test]
        public void ConnectedConnectionTest()
        {
            int[,] ip = new int[,] { { 0, 1 }, { 0, 2 }, { 1, 3 }, { 2, 3 }, { 2, 5 }, { 5, 6 }, { 3, 4 } };
            var result = Solution.CriticalConnections(7, 7, ip);
            //var expectedSet = new List<HashSet<int>> { new HashSet<int>{3, 4 }, new HashSet<int> { 2, 5} };
            Assert.True(result.Count == 3);

            ip = new int[,] { { 0, 1 }, { 1, 2 }, { 2, 0 }, { 1, 3 }, { 3, 4 }, { 4, 5 }, { 5, 3 } };
            result = Solution.CriticalConnections(6, 7, ip);
        }

        // [Test]
        // public void TestDFSSeq()
        // {
        //     int[,] ip = new int[,] { { 0, 1 }, { 0, 2 }, { 1, 3 }, { 2, 3 }, { 2, 5 }, { 5, 6 }, { 3, 4 } };
        //     var result = Solution.DFSSeq(7, 7, ip, 0);
        //     var expectedSet = new List<int> { 0, 1, 3, 2, 5, 6, 4 };
        //     Assert.True(result.Length == 7);
        //     for(var i = 0; i < result.Length; i++)
        //     {
        //         Assert.True(expectedSet[i] == result[i]);
        //     }
        // }

        [Test]
        public void NumDistinctIslandsTest()
        {
            int[][] ip = new int[][] { new int[] { 1, 1, 0 }, new int[] { 0, 1, 1 }, new int[] { 0, 0, 0 }, new int[] { 1, 1, 1 }, new int[] { 0, 1, 0 } };
            var result = Solution.NumDistinctIslands(ip);
            Assert.AreEqual(2, result);
        }

        [Test]
        public void NumIslandIITest()
        {
            int[][] ip = new int[][] { new int[] { 0, 0 }, new int[] { 0, 1 }, new int[] { 1, 2 }, new int[] { 2, 1 } };
            var result = Solution.NumIslands2(3, 3, ip);
        }

        [Test]
        public void serializeBinaryTreeTest()
        {
            var root = new Solution.TreeNode(5);
            root.left = new Solution.TreeNode(2);
            root.right = new Solution.TreeNode(3);
            root.right.left = new Solution.TreeNode(2);
            root.right.right = new Solution.TreeNode(4);
            root.right.right.left = new Solution.TreeNode(3);
            root.right.right.right = new Solution.TreeNode(1);
            var result = Solution.serialize(root);
            var result2 = Solution.deserialize(result);
        }

        [Test]
        public void TestWidthOfTheBinaryTree()
        {
            //[1,3,2,5,null,null,9,6,null,null,7,null, null, null, null]
            var root = new Solution.TreeNode(1);
            root.left = new Solution.TreeNode(3);
            root.right = new Solution.TreeNode(2);
            root.left.left = new Solution.TreeNode(5);
            root.right.right = new Solution.TreeNode(9);
            root.left.left.left = new Solution.TreeNode(6);
            root.right.right.right = new Solution.TreeNode(7);
            var result = Solution.WidthOfBinaryTree(root);
            Assert.True(result == 8);
        }

        [Test]
        public void TestSearchRotatedBST()
        {
            int[] ip = new int[] { 4, 5, 6, 7, 0, 1, 2 };
            int target = 0;
            var result = Solution.SearchRotatedBST(ip, target);
            Assert.AreEqual(4, result);

            ip = new int[] { 1, 2, 3, 4, 5 };
            target = 2;
            result = Solution.SearchRotatedBST(ip, target);
            Assert.AreEqual(1, result);

            ip = new int[] { 1, 3 };
            target = 1;
            result = Solution.SearchRotatedBST(ip, target);
            Assert.AreEqual(0, result);

            ip = new int[] { 1, 3 };
            target = 3;
            result = Solution.SearchRotatedBST(ip, target);
            Assert.AreEqual(1, result);

            ip = new int[] { 3, 1 };
            target = 1;
            result = Solution.SearchRotatedBST(ip, target);
            Assert.AreEqual(1, result);

            ip = new int[] { 2, 3, 1 };
            target = 1;
            result = Solution.SearchRotatedBST(ip, target);
            Assert.AreEqual(2, result);
        }

        [Test]
        public void StringCompressTest()
        {
            char[] ip = new char[] { 'a', 'a', 'd', 'b', 'b', 'c', 'c', 'c', 'c', 'c', 'b' };
            var len = Solution.StringCompress(ip);
            var result = ip.Take(len).ToArray();
        }

        [Test]
        public void FindLoopSizeInArray()
        {
            int[] ip = new int[] { 1, 3, 0, 1 };
            var result = Solution.FindLoopSizeInArray(ip);
            Assert.AreEqual(3, result);

            ip = new int[] { 1, 3, 2, 5, 2, 1, 6 };
            result = Solution.FindLoopSizeInArray(ip);
            Assert.AreEqual(5, result);

            ip = new int[] { 1, 3, 2, 5, 2, 0, 6 };
            result = Solution.FindLoopSizeInArray(ip);
            Assert.AreEqual(6, result);
        }

        [Test]
        public void TestSortColors()
        {
            int[] ip = new int[] { 2, 0, 2, 1, 1, 0 };
            Solution.SortColors(ip);
        }

        [Test]
        public void PartitionLabelsTest()
        {
            string S = "ababcbacadefegdehijhklij";
            var r = Solution.PartitionLabels(S);
            Assert.AreEqual(3, r.Count);
            Assert.AreEqual(9, r[0]);
            Assert.AreEqual(7, r[1]);
            Assert.AreEqual(8, r[2]);
        }

        [Test]
        public void MinRepairCost()
        {
            //TODO solution is incorrect
            int[][] ip = new int[][] { new int[] { 1, 2 }, new int[] { 2, 3 }, new int[] { 3, 4 }, new int[] { 4, 5 }, new int[] { 1, 5 } };
            int[][] repairs = new int[][] { new int[] { 1, 2, 12 }, new int[] { 3, 4, 30 }, new int[] { 1, 5, 8 } };
            var result = Solution.MinimumRepairCost(ip, repairs, 5);
            Assert.IsTrue(result == 20);
        }

        [Test]
        public void TargetPairSumTest()
        {
            int[] ip = new int[] { 20, 50, 40, 25, 30, 10 };
            int t = 90;
            var r = Solution.TargetPairSum(ip, t);
            Assert.IsTrue(r[0] == 50);
            Assert.IsTrue(r[1] == 10);
        }
        [Test]
        public void OptimalUtilizationTest()
        {
            /*
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

            int[,] a = new int[,] { { 1, 2 }, { 2, 4 }, { 3, 6 } };
            int[,] b = new int[,] { { 1, 2 } };
            var r = Solution.OptimalUtilization(a, b, 7);
            Assert.IsTrue(r.Count == 1);
            Assert.AreEqual(2, r[0][0]);
            Assert.AreEqual(1, r[0][1]);

            a = new int[,] { { 1, 3 }, { 2, 5 }, { 3, 7 }, { 4, 10 } };
            b = new int[,] { { 1, 2 }, { 2, 3 }, { 3, 4 }, { 4, 5 } };
            r = Solution.OptimalUtilization(a, b, 10);
            Assert.IsTrue(r.Count == 2);
            Assert.AreEqual(2, r[0][0]);
            Assert.AreEqual(4, r[0][1]);
            Assert.AreEqual(3, r[1][0]);
            Assert.AreEqual(2, r[1][1]);
        }

        [Test]
        public void ConnectRopesOptimally()
        {
            /*
            Example 2:

                Input: ropes = [20, 4, 8, 2]
                Output: 54
                Example 3:

                Input: ropes = [1, 2, 5, 10, 35, 89]
                Output: 224
                Example 4:

                Input: ropes = [2, 2, 3, 3]
                Output: 21
             */
            int[] ip = new int[] { 8, 4, 6, 12 };
            Assert.AreEqual(58, Solution.ConnectRopesOptimally(ip));
            ip = new int[] { 20, 4, 8, 2 };
            Assert.AreEqual(54, Solution.ConnectRopesOptimally(ip));
            ip = new int[] { 1, 2, 5, 10, 35, 89 };
            Assert.AreEqual(224, Solution.ConnectRopesOptimally(ip));
            ip = new int[] { 2, 2, 3, 3 };
            Assert.AreEqual(20, Solution.ConnectRopesOptimally(ip));
            ip = new int[] { 2, 2, 2, 2 };
            Assert.AreEqual(16, Solution.ConnectRopesOptimally(ip));
        }

        [Test]
        public void TreasureIsland()
        {
            int[,] ip = new int[,]{{'O', 'O', 'O', 'O'},
                                   {'D', 'O', 'D', 'O'},
                                   {'O', 'O', 'O', 'O'},
                                   {'X', 'D', 'D', 'O'}};
            var r = Solution.TreasureIsland(ip);
        }

        [Test]
        public void ChangeDirTest()
        {
            var result = Solution.ChangeDirectory("/c/d", "../q/../p");
            Assert.AreEqual("/c/p", result);
            result = Solution.ChangeDirectory("/", "a");
            Assert.AreEqual("/a", result);
            result = Solution.ChangeDirectory("/c", "q/p");
            Assert.AreEqual("/c/q/p", result);
            result = Solution.ChangeDirectory("/", "../../");
            Assert.AreEqual("/", result);
            result = Solution.ChangeDirectory("/", "");
            Assert.AreEqual("/", result);
            result = Solution.ChangeDirectory("/foo/bar", "../q/r/bar");
            Assert.AreEqual("/foo/q/r/bar", result);
        }

        [Test]
        public void WordLadderIITest()
        {
            var result = Solution.FindLadders("hit", "cog", new List<string> { "hot", "dot", "dog", "lot", "log", "cog" });
            Assert.True(result.Count == 2);

            result = Solution.FindLadders("red", "tax", new List<string> { "ted", "tex", "red", "tax", "tad", "den", "rex", "pee" });
            Assert.True(result.Count == 3);

            result = Solution.FindLadders("leet", "code", new List<string> { "lest", "leet", "lose", "code", "lode", "robe", "lost" });
            Assert.True(result.Count == 1);


            result = Solution.FindLadders("cet", "ism", new List<string> { "get", "gee", "gte", "ate", "ats", "its", "ito", "ibo", "ibm", "ism" });
            Assert.True(result.Count == 1);

            /*
            [["cet","get","gee","gte","ate","ats","its","ito","ibo","ibm","ism"],
            ["cet","cat","can","ian","inn","ins","its","ito","ibo","ibm","ism"],
            ["cet","cot","con","ion","inn","ins","its","ito","ibo","ibm","ism"]]
             */

            result = Solution.FindLadders("cet", "ism", new List<string> { "kid","tag","pup","ail","tun","woo","erg","luz","brr","gay","sip",
                                                                            "kay","per","val","mes","ohs","now","boa","cet","pal","bar","die",
                                                                            "war","hay","eco","pub","lob","rue","fry","lit","rex","jan","cot",
                                                                            "bid","ali","pay","col","gum","ger","row","won","dan","rum","fad",
                                                                            "tut","sag","yip","sui","ark","has","zip","fez","own","ump","dis",
                                                                            "ads","max","jaw","out","btu","ana","gap","cry","led","abe","box",
                                                                            "ore","pig","fie","toy","fat","cal","lie","noh","sew","ono","tam",
                                                                            "flu","mgm","ply","awe","pry","tit","tie","yet","too","tax","jim",
                                                                            "san","pan","map","ski","ova","wed","non","wac","nut","why","bye",
                                                                            "lye","oct","old","fin","feb","chi","sap","owl","log","tod","dot",
                                                                            "bow","fob","for","joe","ivy","fan","age","fax","hip","jib","mel",
                                                                            "hus","sob","ifs","tab","ara","dab","jag","jar","arm","lot","tom",
                                                                            "sax","tex","yum","pei","wen","wry","ire","irk","far","mew","wit",
                                                                            "doe","gas","rte","ian","pot","ask","wag","hag","amy","nag","ron",
                                                                            "soy","gin","don","tug","fay","vic","boo","nam","ave","buy","sop",
                                                                            "but","orb","fen","paw","his","sub","bob","yea","oft","inn","rod",
                                                                            "yam","pew","web","hod","hun","gyp","wei","wis","rob","gad","pie",
                                                                            "mon","dog","bib","rub","ere","dig","era","cat","fox","bee","mod",
                                                                            "day","apr","vie","nev","jam","pam","new","aye","ani","and","ibm",
                                                                            "yap","can","pyx","tar","kin","fog","hum","pip","cup","dye","lyx",
                                                                            "jog","nun","par","wan","fey","bus","oak","bad","ats","set","qom",
                                                                            "vat","eat","pus","rev","axe","ion","six","ila","lao","mom","mas",
                                                                            "pro","few","opt","poe","art","ash","oar","cap","lop","may","shy",
                                                                            "rid","bat","sum","rim","fee","bmw","sky","maj","hue","thy","ava",
                                                                            "rap","den","fla","auk","cox","ibo","hey","saw","vim","sec","ltd",
                                                                            "you","its","tat","dew","eva","tog","ram","let","see","zit","maw",
                                                                            "nix","ate","gig","rep","owe","ind","hog","eve","sam","zoo","any",
                                                                            "dow","cod","bed","vet","ham","sis","hex","via","fir","nod","mao",
                                                                            "aug","mum","hoe","bah","hal","keg","hew","zed","tow","gog","ass",
                                                                            "dem","who","bet","gos","son","ear","spy","kit","boy","due","sen",
                                                                            "oaf","mix","hep","fur","ada","bin","nil","mia","ewe","hit","fix",
                                                                            "sad","rib","eye","hop","haw","wax","mid","tad","ken","wad","rye",
                                                                            "pap","bog","gut","ito","woe","our","ado","sin","mad","ray","hon",
                                                                            "roy","dip","hen","iva","lug","asp","hui","yak","bay","poi","yep",
                                                                            "bun","try","lad","elm","nat","wyo","gym","dug","toe","dee","wig",
                                                                            "sly","rip","geo","cog","pas","zen","odd","nan","lay","pod","fit",
                                                                            "hem","joy","bum","rio","yon","dec","leg","put","sue","dim","pet",
                                                                            "yaw","nub","bit","bur","sid","sun","oil","red","doc","moe","caw",
                                                                            "eel","dix","cub","end","gem","off","yew","hug","pop","tub","sgt",
                                                                            "lid","pun","ton","sol","din","yup","jab","pea","bug","gag","mil",
                                                                            "jig","hub","low","did","tin","get","gte","sox","lei","mig","fig",
                                                                            "lon","use","ban","flo","nov","jut","bag","mir","sty","lap","two",
                                                                            "ins","con","ant","net","tux","ode","stu","mug","cad","nap","gun",
                                                                            "fop","tot","sow","sal","sic","ted","wot","del","imp","cob","way",
                                                                            "ann","tan","mci","job","wet","ism","err","him","all","pad","hah",
                                                                            "hie","aim","ike","jed","ego","mac","baa","min","com","ill","was",
                                                                            "cab","ago","ina","big","ilk","gal","tap","duh","ola","ran","lab",
                                                                            "top","gob","hot","ora","tia","kip","han","met","hut","she","sac",
                                                                            "fed","goo","tee","ell","not","act","gil","rut","ala","ape","rig",
                                                                            "cid","god","duo","lin","aid","gel","awl","lag","elf","liz","ref",
                                                                            "aha","fib","oho","tho","her","nor","ace","adz","fun","ned","coo",
                                                                            "win","tao","coy","van","man","pit","guy","foe","hid","mai","sup",
                                                                            "jay","hob","mow","jot","are","pol","arc","lax","aft","alb","len",
                                                                            "air","pug","pox","vow","got","meg","zoe","amp","ale","bud","gee",
                                                                            "pin","dun","pat","ten","mob"});
            Assert.True(result.Count == 3);
    }

        [Test]
        public void CourseScheduleTest()
        {
            Assert.True(Solution.CanFinish(3, new int[][] { new int[] { 0, 1 }, new int[] { 0, 2 }, new int[] { 1, 2 } }));
            Assert.False(Solution.CanFinish(3, new int[][] { new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 1, 2 } }));
            Assert.True(Solution.CanFinish(3, new int[][] { }));
            Assert.False(Solution.CanFinish(3, new int[][] { new int[] { 0, 1 }, new int[] { 2, 0 }, new int[] { 1, 2 } }));
            Assert.True(Solution.CanFinish(3, new int[][] { new int[] { 0, 1 }, new int[] { 2, 1 } }));
        }

        [Test]
        public void LowestCommonAncestorTest()
        {
            var root = new Solution.TreeNode(3);
            var p = new Solution.TreeNode(5);
            root.left = p;
            root.right = new Solution.TreeNode(1);
            root.left.left = new Solution.TreeNode(6);
            root.left.right = new Solution.TreeNode(2);
            root.left.right.left = new Solution.TreeNode(7);
            var q = new Solution.TreeNode(4);
            root.left.right.right = q;
            root.right.left = new Solution.TreeNode(0);
            root.right.right = new Solution.TreeNode(8);

            var result = Solution.LowestCommonAncestor(root, p, q);
            Assert.True(result.val == 5);
        }

        [Test]
        public void CutOffTreeTest()
        {
            int[][] ip = new int[][] { new int[] { 1, 2, 3 }, new int[] { 0, 0, 0 }, new int[] { 7, 10, 5 } };
            Assert.True(Solution.CutOffTree(ip)== -1);
            ip = new int[][] { new int[] { 1, 1, 1 }, new int[] { 0, 6, 0 }, new int[] { 1, 1, 1 } };
            Assert.True(Solution.CutOffTree(ip) == 2);
            ip = new int[][] { new int[] { 1, 9, 2 }, new int[] { 0, 1, 1 }, new int[] { 6, 3, 4 } };
            Assert.True(Solution.CutOffTree(ip) == 11);
            ip = new int[][] { new int[] { 54581641, 64080174, 24346381, 69107959 },
                new int[] { 86374198, 61363882, 68783324, 79706116 },
                new int[] { 668150, 92178815, 89819108, 94701471 },
                new int[] { 83920491, 22724204, 46281641, 47531096 },
                new int[] { 89078499, 18904913, 25462145, 60813308 } };
            Assert.True(Solution.CutOffTree(ip) == 57);
            ip = new int[][] { new int[] { 69438, 55243, 0, 43779, 5241, 93591, 73380 },
                new int[] { 847, 49990, 53242, 21837, 89404, 63929, 48214 },
                new int[] { 90332, 49751, 0, 3088, 16374, 70121, 25385 },
                new int[] { 14694, 4338, 87873, 86281, 5204, 84169, 5024 },
                new int[] { 31711, 47313, 1885, 28332, 11646, 42583, 31460 },
                new int[] { 59845, 94855, 29286, 53221, 9803, 41305, 60749 },
                new int[] { 95077, 50343, 27947, 92852, 0, 0, 19731 },
                new int[] { 86158, 63553, 56822, 90251, 0, 23826, 17478 },
                new int[] { 60387, 23279, 78048, 78835, 5310, 99720, 0 },
                new int[] { 74799, 48845, 60658, 29773, 96129, 90443, 14391 },
                new int[] { 65448, 63358, 78089, 93914, 7931, 68804, 72633 },
                new int[] { 93431, 90868, 55280, 30860, 59354, 62083, 47669 },
                new int[] { 81064, 93220, 22386, 22341, 95485, 20696, 13436 },
                new int[] { 50083, 0, 89399, 43882, 0, 13593, 27847 },
                new int[] { 0, 12256, 33652, 69301, 73395, 93440, 0 },
                new int[] { 42818, 87197, 81249, 33936, 7027, 5744, 64710 },
                new int[] { 35843, 0, 99746, 52442, 17494, 49407, 63016 },
                new int[] { 86042, 44524, 0, 0, 26787, 97651, 28572 },
                new int[] { 54183, 83466, 96754, 89861, 84143, 13413, 72921 },
                new int[] { 89405, 52305, 39907, 27366, 14603, 0, 14104 },
                new int[] { 70909, 61104, 70236, 30365, 0, 30944, 98378 },
                new int[] { 20124, 87188, 6515, 98319, 78146, 99325, 88919 },
                new int[] { 89669, 0, 64218, 85795, 2449, 48939, 12869 }, new int[] { 93539, 28909, 90973, 77642, 0, 72170, 98359 },
                new int[] { 88628, 16422, 80512, 0, 38651, 50854, 55768 }, new int[] { 13639, 2889, 74835, 80416, 26051, 78859, 25721 },
                new int[] { 90182, 23154, 16586, 0, 27459, 3272, 84893 }, new int[] { 2480, 33654, 87321, 93272, 93079, 0, 38394 },
                new int[] { 34676, 72427, 95024, 12240, 72012, 0, 57763 }, new int[] { 97957, 56, 83817, 45472, 0, 24087, 90245 },
                new int[] { 32056, 0, 92049, 21380, 4980, 38458, 3490 }, new int[] { 21509, 76628, 0, 90430, 10113, 76264, 45840 },
                new int[] { 97192, 58807, 74165, 65921, 45726, 47265, 56084 }, new int[] { 16276, 27751, 37985, 47944, 54895, 80706, 2372 },
                new int[] { 28438, 53073, 0, 67255, 38416, 63354, 69262 }, new int[] { 23926, 75497, 91347, 58436, 73946, 39565, 10841 },
                new int[] { 34372, 69647, 44093, 62680, 32424, 69858, 68719 }, new int[] { 24425, 4014, 94871, 1031, 99852, 88692, 31503 },
                new int[] { 24475, 12295, 33326, 37771, 37883, 74568, 25163 }, new int[] { 0, 18411, 88185, 60924, 29028, 69789, 0 },
                new int[] { 34697, 75631, 7636, 16190, 60178, 39082, 7052 }, new int[] { 24876, 9570, 53630, 98605, 22331, 79320, 88317 },
                new int[] { 27204, 89103, 15221, 91346, 35428, 94251, 62745 }, new int[] { 26636, 28759, 12998, 58412, 38113, 14678, 0 },
                new int[] { 80871, 79706, 45325, 3861, 12504, 0, 4872 }, new int[] { 79662, 15626, 995, 80546, 64775, 0, 68820 },
                new int[] { 25160, 82123, 81706, 21494, 92958, 33594, 5243 } };
            Assert.True(Solution.CutOffTree(ip) == 5637);
        }


        [Test]
        public void WordSearchTest()
        {
            char[][] board = new char[][]{
          new char[]{'A','B','C','E'},
          new char[]{'S','F','C','S'},
          new char[]{'A','D','E','E'}};

            Assert.True(Solution.Exist(board, "ABCCED"));
            Assert.True(Solution.Exist(board, "SEE"));
            Assert.False(Solution.Exist(board, "ABCB"));
        }

        [Test]
        public void TrieTest()
        {
            /*
             ["Trie","insert","search","search","startsWith","insert","search"]
             [[],["apple"],["apple"],["app"],["app"],["app"],["app"]]
             */

            var root = new Trie();
            root.Insert("apple");
            Assert.True(root.Search("apple"));
            Assert.True(root.StartsWith("app"));
            Assert.IsFalse(root.Search("app"));
            root.Insert("app");
            Assert.True(root.Search("app"));
        }

        [Test]
        public void UniquePathsIIITest()
        {
            var ip = new int[][] { new int[] { 1, -1, 2 } };
            Assert.True(Solution.UniquePathsIII(ip) == 0);
            ip = new int[][]{new int[]{1, 0, 0, 0},new int[]{0,0,0,0}, new int[]{0,0,2,-1} };
            Assert.True(Solution.UniquePathsIII(ip) == 2);
        }

        [Test]
        public void MinimumRewardTest()
        {
            int[] ip = new int[] { 8, 4, 2, 1, 3, 6, 7, 9, 5 };
            Assert.True(25 == Solution.MinRewards(ip));
            ip = new int[] { 0, 4, 2, 1, 3 };
            Assert.True(9 == Solution.MinRewards(ip));
        }

        [Test]
        public void WordBreakTest()
        {
            var s = "aaaaaaa";
            var wordDict = new List<string>{"aaaa", "aaa"};
            Assert.True(Solution.WordBreak(s, wordDict));
        }

        [Test]
        public void WordBreakTrie()
        {
            var s = "LeetCode";
            var wordDict = new List<string> { "Leet", "Code" };
            Assert.True(Solution.WordBreakTrie(s, wordDict));
            s = "aaaaaaa";
            wordDict = new List<string> { "aaaa", "aaa" };
            Assert.True(Solution.WordBreakTrie(s, wordDict));
        }

        [Test]
        public void WordBreakUsingMemorizationTest()
        {
            var s = "aaaaaaa";
            var wordDict = new List<string> { "aaaa", "aaa" };
            Assert.True(Solution.WordBreak(s, wordDict));

            s = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab";
            wordDict = new List<string> { "a", "aa", "aaa", "aaaa", "aaaaa", "aaaaaa", "aaaaaaa", "aaaaaaaa", "aaaaaaaaa", "aaaaaaaaaa" };
            Assert.False(Solution.WordBreak(s, wordDict));
        }

        [Test]
        public void WordBreakII()
        {
            var s = "pineapplepenapple";
            var wordDict = new List<string> { "apple", "pen", "applepen", "pine", "pineapple" };
            var r = Solution.WordBreakII(s, wordDict);
            Assert.True(r.Count == 1);
        }

        [Test]
        public void ThreeSumZero()
        {
            int[] nums = new int[] { -4, -2, -2, -2, 0, 1, 2, 2, 2, 3, 3, 4, 4, 6, 6 };
            var r = Solution.ThreeSumZero(nums);
            Assert.True(r.Count == 6);
        }


        [Test]
        public void RotateImage()
        {
            int[][] grid = new int[][] { new int[] { 4, 8, 12, 16 }, new int[] { 3, 7, 11, 15 }, new int[] { 2, 6, 10, 14 }, new int[] { 1, 5, 9, 13 } };
            Solution.Rotate(grid);

        }

        [Test]
        public void GroupAnagrams()
        {
            var words = new List<string>() { "yo", "act", "flop", "tac", "foo", "cat", "oy", "olfp" };
            var r = Solution.groupAnagrams(words);
        }


        [Test]
        public void MinSubstringWindow()
        {
            var s = "aabaacaabca";
            var t = "abc";
            var r = Solution.MinWindow(s, t);
            s = "ADOBECODEBANC";
            t = "ABC";
            r = Solution.MinWindow(s, t);
        }
        private bool bSearch(int[] nums, int i, int j, int target, ref int ret)
        {
            if (i > j) return false;

            int mid = (i + j) / 2;
            if (nums[mid] == target)
            {
                ret = mid;
                return true;
            }

            return target > nums[mid] ? bSearch(nums, mid + 1, j, target, ref ret) :
                        bSearch(nums, i, mid - 1, target, ref ret);

        }

        [Test]
        public void Parity()
        {
            int r = Solution.Parity(82);
            Assert.True(r == 1);
            r = Solution.Parity(53);
            Assert.True(r == 0);
        }

        [Test]
        public void ParityS()
        {
            int r = Solution.ParityS(82);
            Assert.True(r == 1);
            r = Solution.ParityS(53);
            Assert.True(r == 0);
        }

        [Test]
        public void ParityXOR()
        {
            long r = Solution.ParityXOR(82);
            Assert.True(r == 1);
            r = Solution.ParityXOR(53);
            Assert.True(r == 0);
            r = Solution.ParityXOR(8);

        }

        [Test]
        public void WaterTrap()
        {
            int[] h = new int[] { 0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1 };
            var r = Solution.Trap(h);
            Assert.True( r == 6);
            h = new int[] { 2, 0, 1 };
            r = Solution.Trap(h);
            Assert.True(r == 1);
        }

        [Test]
        public void ReverseListOfSizeK()
        {
            var h = ListNode.GetList(new int[] { 4, 3, 2, 1, 8, 7, 6, 5, 12, 11, 10, 9});
            var r = Solution.ReverseKGroup(h, 4);
            int expected = 1;
            while (r != null)
            {
                Assert.AreEqual(expected, r.val);
                r = r.next;
                expected++;
            }
        }


        [Test]
        public void CopyRandomList()
        {
            var h = new RandomNode(4);
            var t = new RandomNode(7);
            h.random = t;
            t.random = h;
            h.next = new RandomNode(5);
            h.next.random = t;
            h.next.next = new RandomNode(6);
            h.next.next.next = t;

            // 4 -> 5-> 6-> 7
            // |    |   |   |
            // 7    7   N   4

            var result = Solution.CopyRandomListIISol(h);
           
        }

        //[Test]
        //public void LRU()
        //{
        //    var lruCache = new LRUCache(3);
        //    lruCache.InsertKeyValuePair("b", 2);
        //    lruCache.InsertKeyValuePair("a", 1);
        //    lruCache.InsertKeyValuePair("c", 3);
        //    Assert.True(lruCache.GetMostRecentKey() == "c");
        //    Assert.True(lruCache.GetValueFromKey("a").value == 1);
        //    Assert.True(lruCache.GetMostRecentKey() == "a");
        //    lruCache.InsertKeyValuePair("d", 4);
        //    var evictedValue = lruCache.GetValueFromKey("b");
        //    Assert.True(evictedValue == null || evictedValue.found == false);
        //    lruCache.InsertKeyValuePair("a", 5);
        //    Assert.True(lruCache.GetValueFromKey("a").value == 5);

        //    /*
             
        //     {"arguments": ["a"], "method": "getValueFromKey"},
        //    {"arguments": ["a", 1], "method": "insertKeyValuePair"},
        //    {"arguments": ["a"], "method": "getValueFromKey"},
        //    {"arguments": ["a", 9001], "method": "insertKeyValuePair"},
        //    {"arguments": ["a"], "method": "getValueFromKey"},
        //    {"arguments": ["b", 2], "method": "insertKeyValuePair"},
        //    {"arguments": ["a"], "method": "getValueFromKey"},
        //    {"arguments": ["b"], "method": "getValueFromKey"},
        //    {"arguments": ["c", 3], "method": "insertKeyValuePair"},
        //    {"arguments": ["a"], "method": "getValueFromKey"},
        //    {"arguments": ["b"], "method": "getValueFromKey"},
        //    {"arguments": ["c"], "method": "getValueFromKey"}
        //     */
        //    lruCache = new LRUCache(1);
        //    var r = lruCache.GetValueFromKey("a");
        //    Assert.True(r.found == false);
        //    lruCache.InsertKeyValuePair("a", 1);
        //    r = lruCache.GetValueFromKey("a");
        //    Assert.True(r.found == true && r.value == 1);
        //    lruCache.InsertKeyValuePair("a", 9001);
        //    r = lruCache.GetValueFromKey("a");
        //    Assert.True(r.found == true && r.value == 9001);
        //    lruCache.InsertKeyValuePair("b", 2);
        //    r = lruCache.GetValueFromKey("a");
        //    Assert.True(r.found == false);
        //    r = lruCache.GetValueFromKey("b");
        //    Assert.True(r.found == true && r.value == 2);
        //    lruCache.InsertKeyValuePair("c", 3);
        //    r = lruCache.GetValueFromKey("a");
        //    Assert.True(r.found == false);
        //    r = lruCache.GetValueFromKey("b");
        //    Assert.True(r.found == false);
        //    r = lruCache.GetValueFromKey("c");
        //    Assert.True(r.found == true && r.value == 3);



        //    lruCache = new LRUCache(4);
        //    lruCache.InsertKeyValuePair("a", 1);
        //    lruCache.InsertKeyValuePair("b", 2);
        //    lruCache.InsertKeyValuePair("c", 3);
            
        //    Assert.True(lruCache.GetMostRecentKey() == "c");
        //    Assert.True(lruCache.GetValueFromKey("b").value == 1);
        //    Assert.True(lruCache.GetMostRecentKey() == "b");
        //    Assert.True(lruCache.GetValueFromKey("a").value == 1);
        //    Assert.True(lruCache.GetMostRecentKey() == "a");
        //    lruCache.InsertKeyValuePair("d", 4);
        //    Assert.True(lruCache.GetMostRecentKey() == "d");
        //    lruCache.InsertKeyValuePair("e", 5);
        //    Assert.True(lruCache.GetMostRecentKey() == "e");
        //}


        [Test]
        public void StringtoBase64()
        {
            var s = "https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding.getbytes?view=netcore-3.1#System_Text_Encoding_GetBytes_System_String_";
            Encoding bs64 = Encoding.ASCII;
            var r = Convert.ToBase64String(bs64.GetBytes(s));
            var r2 = Convert.ToBase64String(bs64.GetBytes(r));
        }


        [Test]
        public void HashKeyCollisions()
        {
            var hash1 = "93357".GetHashCode();
            var hash2 = "109024".GetHashCode();
            Assert.True(hash1 == hash2);
            Dictionary<string, string> demo = new Dictionary<string, string>() {
                                                           {"109024", "abc"},
                                                           {"93357", "def"},
                                                         };
            var r1 = demo["109024"];
            Assert.True(!demo.ContainsKey("93357"));
        }

        [Test]
        public void HashCollision()
        {
            Dictionary<int, string> hashes = new Dictionary<int, string>();
            string collision = null;
            string st = "";
            for (int i = 0; i < 10000; ++i)
            {
                st = i.ToString();
                int hash = st.GetHashCode();
                if (hashes.TryGetValue(hash, out collision))
                {
                    Console.Write($"Collision: \"{collision}\" and \"{st}\" hash {hash}");

                    break;
                }
                else
                    hashes.Add(hash, st);
            }

        }

        [Test]
        public void SearchInSortedMatrix()
        {

            var ip = new int[,]{
                    {1, 4, 7, 12, 15, 1000},
                    {2, 5, 19, 31, 32, 1001},
                    {3, 8, 24, 33, 35, 1002},
                    {40, 41, 42, 44, 45, 1003},
                    {99, 100, 103, 106, 128, 1004}
                  };
            var r = Solution.SearchInSortedMatrix(ip, 105);
        }

        [Test]
        public void MaxSumIncreasingSubSequence()
        {
            var ip = new int[]{ 8, 12, 2, 3, 15, 5, 7 };
            var r = Solution.MaxSumIncreasingSubsequence(ip);

        }

        /*
         Example 1:

        Input: points = [[1,3],[-2,2]], K = 1
        Output: [[-2,2]]
        Explanation: 
        The distance between (1, 3) and the origin is sqrt(10).
        The distance between (-2, 2) and the origin is sqrt(8).
        Since sqrt(8) < sqrt(10), (-2, 2) is closer to the origin.
        We only want the closest K = 1 points from the origin, so the answer is just [[-2,2]].


        Example 2:

        Input: points = [[3,3],[5,-1],[-2,4]], K = 2
        Output: [[3,3],[-2,4]]
        (The answer [[-2,4],[3,3]] would also be accepted.)
         */
        [Test]
        public void KClosestPointsFromOrigin()
        {
            int[][] p = new int[][] { new int[] { 3, 3 }, new int[] { 5, -1 }, new int[] { -2, 4 } };
            var r = Solution.KClosestPointsFromOrigin(p, 2);
            p = new int[][] { new int[] { 2, 2 }, new int[] { 2, 2 }, new int[] { 1, 1 } };
            r = Solution.KClosestPointsFromOrigin(p, 1);
        }

        [Test]
        public void TestRangesAndIndexes()
        {
            int[] k = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 23, 24, 25 };
            var r = k[6..];
            var _3rd = k[^3]; 
        }

        [Test]
        public void MapSumPairs()
        {
            var msRoot = new MapSum();
            msRoot.Insert("apple", 3);
            var s = msRoot.Sum("ap");
            msRoot.Insert("app", 2);
            s = msRoot.Sum("ap");
        }

        [Test]
        public void HashSetAsQ()
        {
            int[] arr = new int[] { 1, 2, 3 };
            var hs = new HashSet<int>(arr);
            var sw = new Stopwatch();
            sw.Start();
            while (hs.Count > 0)
            {
                hs.Remove(hs.First());
            }
            sw.Stop();
            var t1 = sw.ElapsedMilliseconds;
            sw.Reset();
            var q = new Queue<int>(hs);
            sw.Start();
            while(q.Count > 0)
            {
                q.Dequeue();
            }
            sw.Stop();
            var t2 = sw.ElapsedMilliseconds;
            var d = (double)(t1 - t2)/ 100;
        }
    }
        
}