using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Mr.Pacman
{
    class Node
    {
        /*Data*/
        public static int m_Width = 8;                          //Node's width
        public static int m_Height = 8;                         //Node's height
        private Point m_Point = new Point();                    //Node's position
        private Rectangle m_Position = new Rectangle();         //rectangle for drawing
        private NodeType m_Type;                                //Node's type
        public Direction[,] m_DirMatrix = new Direction[28, 31];//Record shortest path direction to each node
        public int[,] m_DisMatrix = new int[28, 31];            //Record shortest path distance to each node

        public enum NodeType { PILL, POWERPILL, WALL, EATEN, NONE };
        public enum Direction { UP, DOWN, RIGHT, LEFT, NONE };

        /*Method*/
        public Node()
        {
            //Initialize the variables
            m_Type = NodeType.NONE;
        }
        public void SetPosition(int x, int y)
        {
            m_Point.X = x;
            m_Point.Y = y;
            if(m_Type == NodeType.PILL || m_Type == NodeType.WALL || m_Type == NodeType.NONE)
            {
                m_Position.X = x * m_Width + 1;
                m_Position.Y = y * m_Height + 1;
                m_Position.Width = 3;
                m_Position.Height = 3;
            }
            if(m_Type == NodeType.POWERPILL)
            {
                m_Position.X = x * m_Width - 1;
                m_Position.Y = y * m_Height - 1;
                m_Position.Width = 8;
                m_Position.Height = 8;
            }
        }
        public Point GetPoint()     //Get node's position
        {
            return m_Point;
        }   
        public void SetNodeType(NodeType type)
        {
            m_Type = type;
        }
        public NodeType GetNodeType()
        {
            return m_Type;
        }
        public void Draw(Graphics g)
        {
            //Only needed to draw when its type is EATEN
            if (m_Type == NodeType.EATEN)
            {
                g.FillRectangle(Brushes.Black, m_Position);
                g.DrawRectangle(Pens.Black, m_Position);
            }
            //For test only
            /*if (m_Type == NodeType.NONE)
            {
                g.FillRectangle(Brushes.Orange, m_Position);
                g.DrawRectangle(Pens.Black, m_Position);
            }
            if (m_Type == NodeType.WALL)
            {
                g.FillRectangle(Brushes.Green, m_Position);
                g.DrawRectangle(Pens.Black, m_Position);
            }
            if (m_Type == NodeType.PILL)
            {
                g.FillRectangle(Brushes.Yellow, m_Position);
                g.DrawRectangle(Pens.Black, m_Position);
            }
            if (m_Type == NodeType.POWERPILL)
            {
                g.FillRectangle(Brushes.Red, m_Position);
            }*/
        }
    }
}