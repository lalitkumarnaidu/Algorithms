using System;
namespace LeetSolutions.Algos
{
    public class RabinKarp
    {
        private long patHash; //pattern hash value
        private int M;
        private const long Q = 900000047053;
        private int R;
        private long RM;
        public RabinKarp(string pat)
        {
            M = pat.Length;
            R = 256;
            RM = 1;

            for (int i = 1; i <= M - 1; i++)
                RM = (R * RM) % Q;

            patHash = Hash(pat, M);
        }

        private long Hash(string pat, int M)
        {
            long h = 0;
            for (int i = 0; i < M; i++)
            {
                h = (R * h + pat[i]) % Q; 
            }
            return h;
        }

        public int Search(string txt) {
            int n = txt.Length;
            long txtHash = Hash(txt, M);
            if (patHash == txtHash) return 0;
            for (int i = M; i < n; i++)
            {
                if (patHash == txtHash)
                    return i - M + 1;
                txtHash = (txtHash + Q - (RM * txt[i - M])%Q) % Q;
                txtHash = (txtHash * R + txt[i]) % Q;
            }

            return -1;
        }
    }
}
