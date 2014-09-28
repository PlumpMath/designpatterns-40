using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP_Tokenizer
{
    public class TokenList<T> : IEnumerable<T>
    {
        public class Node<T> 
        {
            public Node<T> Next;
            public Node<T> Previous;
            public T Data;

            public Node(T d)
            {
                Data = d;
                Next = null;
                Previous = null;
            }
        }

        private int _size;
        private Node<T> head;
        private Node<T> tail;

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

        public void addFirstItem(T item)
        {
            head = new Node<T>(item);
            tail = head;
            head.Next = tail;
            head.Previous = tail;
        }

        public void AddAfter(T existingData, T newData)
        {
            Node<T> node = Find(existingData);
            if (node == null)
                throw new ArgumentException("existingData doesn't exist in the list");
            AddAfter(node, newData);
        }

        public void AddAfter(Node<T> node, T newData)
        {
            if (node == null)
                throw new ArgumentNullException("node");
            Node<T> temp = findNode(head, node.Data);
            if (temp != node)
                throw new InvalidOperationException("node doesn't exist in the list");

            Node<T> newNode = new Node<T>(newData);
            newNode.Next = node.Next;
            node.Next.Previous = newNode;
            newNode.Previous = node;
            node.Next = newNode;

            // if the node adding is tail node, then repointing tail node
            if (node == tail)
                tail = newNode;
            _size++;
        }

        public void AddBefore(T existingData, T newData)
        {
            Node<T> node = Find(existingData);
            if (node == null)
                throw new ArgumentException("existingData doesn't exist in the list");
            AddBefore(node, newData);
        }

        public void AddBefore(Node<T> node, T newData)
        {
            if (node == null)
                throw new ArgumentNullException("Node");

            Node<T> temp = findNode(head, node.Data);
            if (temp != node)
                throw new InvalidOperationException("Node doesn't exist in the list");

            Node<T> newNode     = new Node<T>(newData);
            node.Previous.Next  = newNode;
            newNode.Previous    = node.Previous;
            newNode.Next        = node;
            node.Previous       = newNode;

            if (node == head)
                head = newNode;
            _size++;
        }

        public void AddFirst(T data)
        {
            if (head == null)
                addFirstItem(data);
            else
            {
                Node<T> newNode     = new Node<T>(data);
                head.Previous       = newNode;
                newNode.Previous    = tail;
                newNode.Next        = head;
                tail.Next           = newNode;
                head                = newNode;
            }

            _size++;
        }


        public void AddLast(T data)
        {
            
            if (head == null)
                addFirstItem(data);
            else
            {
                Node<T> newNode  = new Node<T>(data);
                tail.Next        = newNode;
                newNode.Next     = head;
                newNode.Previous = tail;
                tail             = newNode;
                head.Previous    = tail;
            }
        
            _size++;
        }

        public Node<T> Find(T data)
        {
            Node<T> node = findNode(head, data);
            return node;
        }

        private Node<T> findNode(Node<T> node, T valueToCompare)
        {
            Node<T> result = null;
            if (Comparer.Equals(node.Data, valueToCompare))
                result = node;
            else if (result == null && node.Next != head)
                result = findNode(node.Next, valueToCompare);
            return result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node<T> current = head;
            if (current != null)
            {
                do
                {
                    yield return current.Data;
                    current = current.Next;
                } while (current != head);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
