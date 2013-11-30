using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Mr.Pacman
{
    class Ghost
    {
        /*Data*/
        private int m_Tag;                      //Distinguish ghosts
        private Point m_Position;               //Record ghost's position
        private Node.Direction m_LastDirection; //Record last step's direction
        private Image m_Character;              //Image for all characters                 
        private int HuntedTimer = 0;            //Variable facilitating drawing
        public static int Hunted = 0;           //Left time for being hunted

        public Ghost()
        {
            m_Tag = 0;
            m_Position.X = 1;
            m_Position.Y = 1;
            m_LastDirection = Node.Direction.NONE;
            m_Character = Image.FromFile(System.IO.Directory.GetCurrentDirectory()  + "\\character.png");
        }
        public Point GetPoint() //Get position information
        {
            return m_Position;
        }
        public void SetTag(int tag)
        {
            m_Tag = tag;
        }
        public void SetPosition(int x, int y)
        {
            m_Position.X = x;
            m_Position.Y = y;
        }
        public void Reset() //Send ghosts home when is hunted by pacman
        {
            m_Position.X = 14;
            m_Position.Y = 11;
        }
        public void Move(Map map, Pacman pacman, int iter)
        {
            HuntedTimer = 0;    //Reset huntedtimer

            //Move to pacman according to shortest path
            Node.Direction direction = map.Nodes[m_Position.X, m_Position.Y].m_DirMatrix[pacman.GetX(), pacman.GetY()];
            m_LastDirection = direction;

            Random rnd = new Random(DateTime.Now.Millisecond);  //Introduce random elements
            double randomWalk = 0;
            for (int i = 0; i < iter; i++) //Choose a different random numbe for every ghost
            {
                randomWalk = rnd.NextDouble();
            }
            if (randomWalk <= 0.2)  //Ghost randomly walk under some limited conditions
            {
                if ((map.Nodes[m_Position.X == 0 ? map.GetSize().Width - 1 : m_Position.X - 1, m_Position.Y].GetNodeType() == Node.NodeType.PILL ||
                map.Nodes[m_Position.X == 0 ? map.GetSize().Width - 1 : m_Position.X - 1, m_Position.Y].GetNodeType() == Node.NodeType.POWERPILL) &&
                direction != Node.Direction.LEFT && m_LastDirection != Node.Direction.RIGHT)
                {
                    direction = Node.Direction.LEFT;
                    m_LastDirection = direction;
                }
                else if ((map.Nodes[m_Position.X == map.GetSize().Width - 1 ? 0 : m_Position.X + 1, m_Position.Y].GetNodeType() == Node.NodeType.PILL ||
                map.Nodes[m_Position.X == map.GetSize().Width - 1 ? 0 : m_Position.X + 1, m_Position.Y].GetNodeType() == Node.NodeType.POWERPILL) &&
                direction != Node.Direction.RIGHT && m_LastDirection != Node.Direction.LEFT)
                {
                    direction = Node.Direction.RIGHT;
                    m_LastDirection = direction;
                }
                else if ((map.Nodes[m_Position.X, m_Position.Y == 0 ? map.GetSize().Height - 1 : m_Position.Y - 1].GetNodeType() == Node.NodeType.PILL ||
                map.Nodes[m_Position.X, m_Position.Y == 0 ? map.GetSize().Height - 1 : m_Position.Y - 1].GetNodeType() == Node.NodeType.POWERPILL) &&
                direction != Node.Direction.UP && m_LastDirection != Node.Direction.DOWN)
                {
                    direction = Node.Direction.UP;
                    m_LastDirection = direction;
                }
                else if ((map.Nodes[m_Position.X, m_Position.Y == map.GetSize().Height - 1 ? 0 : m_Position.Y + 1].GetNodeType() == Node.NodeType.PILL ||
                map.Nodes[m_Position.X, m_Position.Y == map.GetSize().Height - 1 ? 0 : m_Position.Y + 1].GetNodeType() == Node.NodeType.POWERPILL) &&
                direction != Node.Direction.DOWN && m_LastDirection != Node.Direction.UP)
                {
                    direction = Node.Direction.DOWN;
                    m_LastDirection = direction;
                }
            }
            if (Hunted > 0)     //Flee to the spot far from pacman
            {
                Hunted--;
                if (pacman.GetX() < 14 && pacman.GetY() < 15)
                {
                    direction = map.Nodes[m_Position.X, m_Position.Y].m_DirMatrix[24, 29];
                    m_LastDirection = direction;
                }
                else if (pacman.GetX() < 14 && pacman.GetY() >= 15)
                {
                    direction = map.Nodes[m_Position.X, m_Position.Y].m_DirMatrix[24, 1];
                    m_LastDirection = direction;
                }
                else if (pacman.GetX() >= 14 && pacman.GetY() < 15)
                {
                    direction = map.Nodes[m_Position.X, m_Position.Y].m_DirMatrix[2, 29];
                    m_LastDirection = direction;
                }
                else
                {
                    direction = map.Nodes[m_Position.X, m_Position.Y].m_DirMatrix[1, 1];
                    m_LastDirection = direction;
                }
            }
            //Move according to choosen direction
            switch (direction)
            {
                case Node.Direction.LEFT : m_Position.X = m_Position.X == 0 ? map.GetSize().Width - 1 : m_Position.X - 1; break;
                case Node.Direction.RIGHT : m_Position.X = m_Position.X == map.GetSize().Width - 1 ? 0 : m_Position.X + 1; break;
                case Node.Direction.UP: m_Position.Y = m_Position.Y == 0 ? map.GetSize().Height - 1 : m_Position.Y - 1; break;
                case Node.Direction.DOWN: m_Position.Y = m_Position.Y == map.GetSize().Height - 1 ? 0 : m_Position.Y + 1; break;
            }       
        }
        public void Draw(Graphics g)
        {
            Rectangle rect = new Rectangle(m_Position.X * Node.m_Width - 3, m_Position.Y * Node.m_Height - 1, 14, 14);
            if (Hunted > 0)
            {
                if (HuntedTimer <= 3)   //Move slowly
                {
                    switch (m_LastDirection)
                    {
                        case Node.Direction.LEFT: rect.X = rect.X + (3 - PacmanForm.DrawClock); g.DrawImage(m_Character, rect, new Rectangle(0, 70, 14, 14), GraphicsUnit.Pixel); break;
                        case Node.Direction.RIGHT: rect.X = rect.X - (3 - PacmanForm.DrawClock); g.DrawImage(m_Character, rect, new Rectangle(0, 70, 14, 14), GraphicsUnit.Pixel); break;
                        case Node.Direction.UP: rect.Y = rect.Y + (3 - PacmanForm.DrawClock); g.DrawImage(m_Character, rect, new Rectangle(0, 70, 14, 14), GraphicsUnit.Pixel); break;
                        case Node.Direction.DOWN: rect.Y = rect.Y - (3 - PacmanForm.DrawClock); g.DrawImage(m_Character, rect, new Rectangle(0, 70, 14, 14), GraphicsUnit.Pixel); break;
                        default: g.DrawImage(m_Character, rect, new Rectangle(0, 70, 14, 14), GraphicsUnit.Pixel); break;
                    }
                }
                else    //Stand still
                {
                    g.DrawImage(m_Character, rect, new Rectangle(0, 70, 14, 14), GraphicsUnit.Pixel); 
                }
                HuntedTimer++;
            }
            else
            {
                switch (m_Tag)
                {
                    case 0:
                        {
                            switch (m_LastDirection)
                            {
                                case Node.Direction.LEFT: rect.X = rect.X + (3 - PacmanForm.DrawClock) * 2; g.DrawImage(m_Character, rect, new Rectangle(28, 14, 14, 14), GraphicsUnit.Pixel); break;
                                case Node.Direction.RIGHT: rect.X = rect.X - (3 - PacmanForm.DrawClock) * 2; g.DrawImage(m_Character, rect, new Rectangle(42, 14, 14, 14), GraphicsUnit.Pixel); break;
                                case Node.Direction.UP: rect.Y = rect.Y + (3 - PacmanForm.DrawClock) * 2; g.DrawImage(m_Character, rect, new Rectangle(0, 14, 14, 14), GraphicsUnit.Pixel); break;
                                case Node.Direction.DOWN: rect.Y = rect.Y - (3 - PacmanForm.DrawClock) * 2; g.DrawImage(m_Character, rect, new Rectangle(14, 14, 14, 14), GraphicsUnit.Pixel); break;
                                default : g.DrawImage(m_Character, rect, new Rectangle(14, 14, 14, 14), GraphicsUnit.Pixel); break;
                            }
                            break;
                        }
                    case 1:
                        {
                            switch (m_LastDirection)
                            {
                                case Node.Direction.LEFT: rect.X = rect.X + (3 - PacmanForm.DrawClock) * 2; g.DrawImage(m_Character, rect, new Rectangle(28, 28, 14, 14), GraphicsUnit.Pixel); break;
                                case Node.Direction.RIGHT: rect.X = rect.X - (3 - PacmanForm.DrawClock) * 2; g.DrawImage(m_Character, rect, new Rectangle(42, 28, 14, 14), GraphicsUnit.Pixel); break;
                                case Node.Direction.UP: rect.Y = rect.Y + (3 - PacmanForm.DrawClock) * 2; g.DrawImage(m_Character, rect, new Rectangle(0, 28, 14, 14), GraphicsUnit.Pixel); break;
                                case Node.Direction.DOWN: rect.Y = rect.Y - (3 - PacmanForm.DrawClock) * 2; g.DrawImage(m_Character, rect, new Rectangle(14, 28, 14, 14), GraphicsUnit.Pixel); break;
                                default: g.DrawImage(m_Character, rect, new Rectangle(14, 28, 14, 14), GraphicsUnit.Pixel); break;
                            }
                            break;
                        }
                    case 2:
                        {
                            switch (m_LastDirection)
                            {
                                case Node.Direction.LEFT: rect.X = rect.X + (3 - PacmanForm.DrawClock) * 2; g.DrawImage(m_Character, rect, new Rectangle(28, 42, 14, 14), GraphicsUnit.Pixel); break;
                                case Node.Direction.RIGHT: rect.X = rect.X - (3 - PacmanForm.DrawClock) * 2; g.DrawImage(m_Character, rect, new Rectangle(42, 42, 14, 14), GraphicsUnit.Pixel); break;
                                case Node.Direction.UP: rect.Y = rect.Y + (3 - PacmanForm.DrawClock) * 2; g.DrawImage(m_Character, rect, new Rectangle(0, 42, 14, 14), GraphicsUnit.Pixel); break;
                                case Node.Direction.DOWN: rect.Y = rect.Y - (3 - PacmanForm.DrawClock) * 2; g.DrawImage(m_Character, rect, new Rectangle(14, 42, 14, 14), GraphicsUnit.Pixel); break;
                                default: g.DrawImage(m_Character, rect, new Rectangle(14, 42, 14, 14), GraphicsUnit.Pixel); break;
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
        }
    }
}
