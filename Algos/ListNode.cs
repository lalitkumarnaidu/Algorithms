namespace Algos
{
    public static partial class Solution
    {
        public class ListNode
        {
            public int val;
            public ListNode next;
            public ListNode(int val = 0, ListNode next = null)
            {
                this.val = val;
                this.next = next;
            }

            public static ListNode GetList(int[] arr)
            {
                var head = new ListNode(arr[0]);
                var cur = head;
                for(int i = 1; i < arr.Length; i++)
                {
                    cur.next = new ListNode(arr[i]);
                    cur = cur.next;
                }
                return head;
            }
        }
    }
}
