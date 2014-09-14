using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP_Tokenizer
{
    public class TokenList<T> : IEnumerable<T>
    {
        private class Node<T> 
        {
            public Node<T> Next;
            public T Data;

            public Node(T d)
            {
                Data = d;
                Next = null;
            }

            public Node(T d, Node<T> n)
            {
                Data = d;
                Next = n;
            }
        }

        private int _size;
        private Node<T> head;

        public int Count { get { return _size; } }

        public TokenList()
        {
            _size = 0;
            head  = null;
        }

        public bool isEmpty()
        {
            return (_size == 0);
        }

        public void AddAfter(T data, T afterNode)
        {
            Node<T> current = head;
            bool matched = false;
            while (!(matched = current.Data.Equals(afterNode)) && current.Next != null)
                current = current.Next;

            if (matched)
            {
                Node<T> newNode = new Node<T>(data);
                if (current.Next != null)
                    newNode.Next = current.Next;

                current.Next = newNode;
                _size++;
            }
        }

        public void AddBefore(T data, T beforeData)
        {
            Node<T> current = head;
            bool matched = false;
            while (current.Next != null && !(matched = current.Next.Data.Equals(beforeData)))
                current = current.Next;

            if (matched)
            {
                Node<T> newNode = new Node<T>(data);
                if (current.Next != null)
                    newNode.Next = current.Next;

                current.Next = newNode;
                _size++;
            }
        }

        public void AddFirst(T data)
        {
            Node<T> newNode = new Node<T>(data, head);
            head = newNode;
            _size++;
        }


        public void AddLast(T data)
        {
            if (head == null)
            {
                Node<T> newNode = new Node<T>(data);
                head = newNode;
            }
            else
            {
                Node<T> current = head;
                while (current.Next != null)
                    current = current.Next;

                Node<T> newNode = new Node<T>(data);
                current.Next = newNode;
            }
            _size++;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var node = head;
            while (node != null)
            {
                yield return node.Data;
                node = node.Next;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
