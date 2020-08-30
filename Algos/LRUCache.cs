using System;
using System.Collections.Generic;

namespace LeetSolutions.Algos.LRU
{
    public class LRUCache
    {

        private Dictionary<int, DoublyLinkedNode> _kv;
        private readonly int _capacity;
        private int _curSize;
        private DoublyLinkedList _listOfRecent;
        public LRUCache(int capacity)
        {
            _kv = new Dictionary<int, DoublyLinkedNode>();
            _capacity = capacity;
            _curSize = 0;
            _listOfRecent = new DoublyLinkedList();
        }

        public int Get(int key)
        {
            if (!_kv.ContainsKey(key)) return -1;
            _listOfRecent.SetHead(_kv[key]);
            return _kv[key].value;
        }

        public void Put(int key, int value)
        {
            if (_kv.ContainsKey(key))
            {
                _kv[key].value = value;
                _listOfRecent.SetHead(_kv[key]);
                return;
            }

            var newNode = new DoublyLinkedNode(key, value);
            if (_curSize < _capacity)
            {
                _listOfRecent.SetHead(newNode);
                _kv.Add(key, newNode);
                _curSize++;
                return;
            }

            EvictLeastRecent();
            _kv.Add(key, newNode);
            _listOfRecent.SetHead(newNode);
        }

        private void EvictLeastRecent()
        {
            var keyToRemove = _listOfRecent.tail.key;
            _listOfRecent.RemoveTail();
            _kv.Remove(keyToRemove);
        }
    }
    public class DoublyLinkedList
    {
        public DoublyLinkedNode head;
        public DoublyLinkedNode tail;

        public void SetHead(DoublyLinkedNode node)
        {
            if (head == node)
                return;
            else if (head == null)
            {
                head = node;
                tail = node;
            }
            else if (head == tail)
            {
                node.next = tail;
                tail.prev = node;
                head = node;
            }
            else
            {
                if (node == tail)
                {
                    RemoveTail();
                }
                node.RemoveBindings();
                node.next = head;
                head.prev = node;
                head = node;
            }
        }

        public void RemoveTail()
        {
            if (tail == null) return;
            else if (tail == head)
            {
                tail = null;
                head = null;
            }
            else
            {
                tail = tail.prev;
                tail.next = null;
            }
        }
    }
    public class DoublyLinkedNode
    {
        public int value;
        public int key;
        public DoublyLinkedNode prev;
        public DoublyLinkedNode next;
        public DoublyLinkedNode(int key, int value)
        {
            this.key = key;
            this.value = value;
        }

        public void RemoveBindings()
        {
            //if this is head
            if (prev != null)
            {
                prev.next = next;
            }
            //if this is tail
            if (next != null)
            {
                next.prev = prev;
            }

            prev = null;
            next = null;
        }
    }
}
