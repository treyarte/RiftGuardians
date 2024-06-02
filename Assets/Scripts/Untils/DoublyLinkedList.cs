using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Untils
{
    public class DoublyLinkedList<T>
    {
        private Node<T> head;
        private Node<T> tail;
        private int size;

        public DoublyLinkedList()
        {
            head = null;
            tail = null;
            size = 0;
        }

        public Node<T> AddLast(T data)
        {
            Node<T> newNode = new Node<T>(data);

            if (head == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail.Next = newNode;
                newNode.Previous = tail;
                tail = newNode;
            }

            size++;

            return newNode;
        }

        public void AddFirst(T data)
        {
            Node<T> newNode = new Node<T>(data);

            if (head == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                head.Previous = newNode;
                newNode.Next = head;
                head = newNode;
            }

            size++;
        }

        public void RemoveFirst()
        {
            if (head == null)
            {
                throw new InvalidOperationException("Linked List is empty");
            }

            if (head == tail)
            {
                head = null;
                tail = null;
            }
            else
            {
                head = head.Next;
                head.Previous = null;
            }

            size--;
        }

        public void RemoveLast()
        {
            if (head == null)
            {
                throw new InvalidOperationException("Linked List is empty");
            }

            if (head == tail)
            {
                head = null;
                tail = null;
            }
            else
            {
                tail = tail.Previous;
                tail.Next = null;
            }

            size--;
        }
        
        // Print the list elements
        public void PrintList()
        {
            Node<T> current = head;
            while (current != null)
            {
                Console.Write(current.Data + " ");
                current = current.Next;
            }
            Console.WriteLine();
        }

        // Print the list elements in reverse order
        public void PrintListReverse()
        {
            Node<T> current = tail;
            while (current != null)
            {
                Console.Write(current.Data + " ");
                current = current.Previous;
            }
            Console.WriteLine();
        }

        public int Size()
        {
            return size;
        }
        
        public Node<T> Head()
        {
            return head;
        }
        
        public Node<T> Tail()
        {
            return tail;
        }
    }
}
