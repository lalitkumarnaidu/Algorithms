using System;
namespace LeetSolutions.Algos.DoublyLinkedList
{
	public class DoublyLinkedList
	{
		public Node Head;
		public Node Tail;

		public void SetHead(Node node)
		{
			// Write your code here.
			if (Head == null)
			{
				Head = node;
				Tail = node;
				return;
			}
			InsertBefore(Head, node);
		}

		public void SetTail(Node node)
		{
			// Write your code here.
			if (Tail == null)
			{
				Head = node;
				Tail = node;
				return;
			}
			InsertAfter(Tail, node);
		}

		public void InsertBefore(Node node, Node nodeToInsert)
		{
			// Write your code here.
			if (nodeToInsert == Head && nodeToInsert == Tail) return;
			Remove(nodeToInsert);
			nodeToInsert.Next = node;
			nodeToInsert.Prev = node.Prev;
			if (Head != node)
				node.Prev.Next = nodeToInsert;
			else
				Head = nodeToInsert;
			node.Prev = nodeToInsert;
		}

		public void InsertAfter(Node node, Node nodeToInsert)
		{
			// Write your code here.
			if (nodeToInsert == Head && nodeToInsert == Tail) return;
			Remove(nodeToInsert);
			nodeToInsert.Prev = node;
			nodeToInsert.Next = node.Next;
			if (Tail != node)
			{
				node.Next.Prev = nodeToInsert;
			}
			else
			{
				Tail = nodeToInsert;
			}
			node.Next = nodeToInsert;
		}

		public void InsertAtPosition(int position, Node nodeToInsert)
		{
			// Write your code here.
			if (position == 1)
			{
				SetHead(nodeToInsert);
				return;
			}
			var cur = Head;
			while (cur != null && position > 1)
			{
				position--;
				cur = cur.Next;
			}
			if (cur != null) InsertBefore(cur, nodeToInsert);
			else
				SetTail(nodeToInsert);
		}

		public void RemoveNodesWithValue(int value)
		{
			// Write your code here.
			var cur = Head;
			while (cur != null)
			{
				var next = cur.Next;
				if (cur.Value == value)
				{
					Remove(cur);
				}
				cur = next;
			}

		}

		public void Remove(Node node)
		{
			// Write your code here.
			if (node == Head) Head = Head.Next;
			if (node == Tail) Tail = Tail.Prev;
			if (node.Prev != null)
				node.Prev.Next = node.Next;
			if (node.Next != null)
				node.Next.Prev = node.Prev;

			node.Prev = null;
			node.Next = null;
		}

		public bool ContainsNodeWithValue(int value)
		{
			// Write your code here.
			var curHead = Head;
			var curTail = Tail;
			while (curHead != null)
			{
				if (curHead.Value == value || curTail.Value == value) return true;
				curHead = curHead.Next;
				curTail = curTail.Prev;
			}
			return false;
		}
	}

	// Do not edit the class below.
	public class Node
	{
		public int Value;
		public Node Prev;
		public Node Next;

		public Node(int value)
		{
			this.Value = value;
		}
	}
}
