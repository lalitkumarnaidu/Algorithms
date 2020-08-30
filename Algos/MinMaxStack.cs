using System;
using System.Collections.Generic;

namespace LeetSolutions.Algos.MinMaxStack
{
	public class MinMaxStack
	{
		Stack<int> _minStack = null;
		Stack<int> _maxStack = null;
		Stack<int> _stack = null;
		public MinMaxStack()
		{
			_minStack = new Stack<int>();
			_maxStack = new Stack<int>();
			_stack = new Stack<int>();
		}
		public int Peek()
		{
			if (_stack.Count == 0) throw new ArgumentOutOfRangeException("stack is empty");
			return _stack.Peek();
		}

		public int Pop()
		{
			if (_stack.Count == 0) throw new ArgumentOutOfRangeException("stack is empty");
			int ret = _stack.Pop();
			if (ret == _minStack.Peek())
			{
				_minStack.Pop();
			}
			if (ret == _maxStack.Peek())
			{
				_maxStack.Pop();
			}

			return ret;
		}


		public void Push(int number)
		{
			if (_stack.Count == 0)
			{
				_minStack.Push(number);
				_maxStack.Push(number);
			}

			if (_minStack.Peek() >= number)
			{
				_minStack.Push(number);
			}
			if (_maxStack.Peek() <= number)
			{
				_maxStack.Push(number);
			}
			_stack.Push(number);

		}


		public int GetMin()
		{
			if (_stack.Count == 0) throw new ArgumentOutOfRangeException("stack is empty");
			return _minStack.Peek();
		}


		public int GetMax()
		{
			if (_stack.Count == 0) throw new ArgumentOutOfRangeException("stack is empty");
			return _maxStack.Peek();
		}
	}
}
