using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mr.Pacman
{
    class Queue
    {
        /*Data*/
        private Node[] node = new Node[1000];
        private int top;
        private int tail;

        /*Method*/
        public Queue()
        {
            top = 0;
            tail = 1;
        }
        public int Empty()
        {
            if ((top + 1) % 1000 == tail)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public int Full()
        {
            if ((tail + 1) % 1000 == top)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public int In(Node n)
        {
            if (Full() == 1)
            {
                return 0;
            }
            else
            {
                node[tail] = n;
                tail = (tail + 1) % 1000;
                return 1;
            }
        }
        public Node Out()
        {
            if (Empty() == 1)
            {
                return null;
            }
            else
            {
                top = (top + 1) % 1000;
                return node[top];
            }
        }
        public void Reset()
        {
            top = 0;
            tail = 1;
        }
    }
}
