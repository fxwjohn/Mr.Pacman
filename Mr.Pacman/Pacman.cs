using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Mr.Pacman
{
    class Pacman
    {
        /*Data*/
        private int m_X;                            //Current location on X axis
        private int m_Y;                            //Current location on Y axis
        private int m_PillCnt = 0;                   //Record the number of pills eaten

        private Node.Direction m_Direction;         //Current moving direction
        private const int PerceptionThreshold = 6;  //Lower bound for searching depth
        private int m_PerceptionDepth;              //Searching depth
        private int[,] flag;                        //Record places which have been visited

        private int ClockCounter = 0;               //Record current frame for drawing
        private Image m_Character = null;           //Image for all characters

        /*Method*/
        public Pacman()
        {
            m_X = 14;
            m_Y = 17;
            m_Direction = Node.Direction.NONE;
            m_PerceptionDepth = 20;
            flag = new int[28, 31];
            m_Character = Image.FromFile(System.IO.Directory.GetCurrentDirectory()  + "\\character.png");
        }   
        public int GetX()
        {
            return m_X;
        }
        public int GetY()
        {
            return m_Y;
        }
        public int GetPillCnt()
        {
            return m_PillCnt;
        }
        public void Move(Map map, Ghost[] ghost)
        {
            //Change perception depth adaptly
            m_PerceptionDepth += 2;
            //If it's a pill or a powerpill, then eat it
            if (map.Nodes[m_X, m_Y].GetNodeType() == Node.NodeType.PILL)
            {
                map.Nodes[m_X, m_Y].SetNodeType(Node.NodeType.EATEN);
                if (m_PerceptionDepth > PerceptionThreshold)
                {
                    m_PerceptionDepth -= 2;
                }
                m_PillCnt++;
            }
            else if (map.Nodes[m_X, m_Y].GetNodeType() == Node.NodeType.POWERPILL)
            {
                map.Nodes[m_X, m_Y].SetNodeType(Node.NodeType.EATEN);
                Ghost.Hunted = 40;
                if (m_PerceptionDepth > PerceptionThreshold)
                {
                    m_PerceptionDepth -= 2;
                }
                m_PillCnt++;
            }
            //Search next step
            AI_WFS(map, ghost);
            //check whether has been eaten or eaten a ghost
            for (int i = 0; i < PacmanForm.GhostNum; i++)
            {
                if (Math.Abs(m_X - ghost[i].GetPoint().X) <= 1 && Math.Abs(m_Y - ghost[i].GetPoint().Y) <= 1 && Ghost.Hunted == 0)
                {
                    //If eaten, return to born place
                    m_X = 14;
                    m_Y = 11;
                    break;
                }
                else if (Ghost.Hunted > 0)
                {
                    for (int j = 0; j < PacmanForm.GhostNum; j++)
                    {
                        if (Math.Abs(m_X - ghost[j].GetPoint().X) <= 1 && Math.Abs(m_Y - ghost[j].GetPoint().Y) <= 1)
                        {
                            ghost[j].Reset();
                        }
                    }
                }
            }   
        }
        private void AI_WFS(Map map, Ghost[] ghost)
        {
            //Evaluate left step
            double score_left = 0;
            if (map.Nodes[m_X == 0 ? map.GetSize().Width - 1 : m_X - 1, m_Y].GetNodeType() != Node.NodeType.NONE &&
                map.Nodes[m_X == 0 ? map.GetSize().Width - 1 : m_X - 1, m_Y].GetNodeType() != Node.NodeType.WALL)
            {
                //Clear record matrix
                for (int i = 0; i < map.GetSize().Width; i++)
                {
                    for (int j = 0; j < map.GetSize().Height; j++)
                    {
                        flag[i, j] = 0;
                    }
                }
                flag[m_X, m_Y] = 1;
                score_left = PredictScore(map, ghost, map.Nodes[m_X, m_Y], Node.Direction.LEFT);
            }
            else
            {
                score_left = -100000;
            }
            
            //Evaluate right step
            double score_right = 0;
            if (map.Nodes[m_X == map.GetSize().Width - 1 ? 0 : m_X + 1, m_Y].GetNodeType() != Node.NodeType.NONE &&
                map.Nodes[m_X == map.GetSize().Width - 1 ? 0 : m_X + 1, m_Y].GetNodeType() != Node.NodeType.WALL)
            {
                for (int i = 0; i < map.GetSize().Width; i++)
                {
                    for (int j = 0; j < map.GetSize().Height; j++)
                    {
                        flag[i, j] = 0;
                    }
                }
                flag[m_X, m_Y] = 1;
                score_right = PredictScore(map, ghost, map.Nodes[m_X, m_Y], Node.Direction.RIGHT);
            }
            else
            {
                score_right = -100000;
            }
            
            //Evaluate up step
            double score_up = 0;
            if (map.Nodes[m_X, m_Y == 0 ? map.GetSize().Height - 1 : m_Y - 1].GetNodeType() != Node.NodeType.NONE &&
                map.Nodes[m_X, m_Y == 0 ? map.GetSize().Height - 1 : m_Y - 1].GetNodeType() != Node.NodeType.WALL)
            {
                for (int i = 0; i < map.GetSize().Width; i++)
                {
                    for (int j = 0; j < map.GetSize().Height; j++)
                    {
                        flag[i, j] = 0;
                    }
                }
                flag[m_X, m_Y] = 1;
                score_up = PredictScore(map, ghost, map.Nodes[m_X, m_Y], Node.Direction.UP);
            }
            else
            {
                score_up = -100000;
            }
            
            //Evaluate down step
            double score_down = 0;
            if (map.Nodes[m_X, m_Y == map.GetSize().Height - 1 ? 0 : m_Y + 1].GetNodeType() != Node.NodeType.NONE &&
               map.Nodes[m_X, m_Y == map.GetSize().Height - 1 ? 0 : m_Y + 1].GetNodeType() != Node.NodeType.WALL)
            {
                for (int i = 0; i < map.GetSize().Width; i++)
                {
                    for (int j = 0; j < map.GetSize().Height; j++)
                    {
                        flag[i, j] = 0;
                    }
                }
                flag[m_X, m_Y] = 1;
                score_down = PredictScore(map, ghost, map.Nodes[m_X, m_Y], Node.Direction.DOWN);
            }
            else
            {
                score_down = -100000;
            }

            //Select next step
            if (score_left >= score_right && score_left >= score_up && score_left >= score_down)
            {
                m_X = (m_X == 0 ? map.GetSize().Width - 1 : m_X - 1);
                m_Direction = Node.Direction.LEFT;
            }
            else if (score_right >= score_left && score_right >= score_up && score_right >= score_down)
            {
                m_X = (m_X == map.GetSize().Width - 1 ? 0 : m_X + 1);
                m_Direction = Node.Direction.RIGHT;
            }
            else if (score_up >= score_left && score_up >= score_down && score_up >= score_right)
            {
                m_Y = (m_Y == 0 ? map.GetSize().Height - 1 : m_Y - 1);
                m_Direction = Node.Direction.UP;
            }
            else if (score_down >= score_left && score_down >= score_up && score_down >= score_right)
            {
                m_Y = (m_Y == map.GetSize().Height - 1 ? 0 : m_Y + 1);
                m_Direction = Node.Direction.DOWN;
            }
        }
        private double PredictScore(Map map, Ghost[] ghost, Node node, Node.Direction direction)
        {
            double score = 0;

            Queue queue = new Queue();
            int x = node.GetPoint().X;
            int y = node.GetPoint().Y;
            int mapwidth = map.GetSize().Width;
            int mapheight = map.GetSize().Height;

            for (int i = 0; i < map.GetSize().Width; i++)
            {
                for (int j = 0; j < map.GetSize().Height; j++)
                {
                    flag[i, j] = 0;
                }
            }
            flag[x, y] = 1;

            switch (direction)
            {
                case Node.Direction.LEFT: queue.In(map.Nodes[x == 0 ? mapwidth - 1 : x - 1, y]);  break;
                case Node.Direction.RIGHT: queue.In(map.Nodes[x == mapwidth - 1 ? 0 : x + 1, y]); break;
                case Node.Direction.UP: queue.In(map.Nodes[x, y == 0 ? mapheight - 1 : y - 1]); break;
                case Node.Direction.DOWN: queue.In(map.Nodes[x, y == mapheight - 1 ? 0 : y + 1]); break;
                default: break;
            }

            int cnt = 1;    //Record search depth
            while (cnt < m_PerceptionDepth)
            {
                //Choose an unvisited node
                Node current = null;
                while (queue.Empty() == 0)
                {
                    current = queue.Out();
                    if (flag[current.GetPoint().X, current.GetPoint().Y] == 0)
                    {
                        break;
                    }
                }
                if(current == null)
                {
                    break;
                }

                int current_x = current.GetPoint().X;
                int current_y = current.GetPoint().Y;

                //Change flag
                flag[current_x, current_y] = 1;
                //Add score when it is a pill or a powerpill
                if (current.GetNodeType() == Node.NodeType.PILL)
                {
                    score += Math.Exp(-cnt);
                }
                else if (current.GetNodeType() == Node.NodeType.POWERPILL)
                {
                    if (Ghost.Hunted > 0)       //If it is hunting a ghost, don't eat another powerpill
                    {
                        score -= Math.Exp(8) * Math.Exp(-2 * cnt);
                    }
                    else                        //Head to a powerpill for safety
                    {
                        score += Math.Exp(8) * Math.Exp(-2 * cnt);
                    }
                }
                //Reduce score when there is a ghost
                for (int i = 0; i < ghost.Length; i++)
                {
                    if (current_x == ghost[i].GetPoint().X &&
                        current_y == ghost[i].GetPoint().Y)
                    {
                        if (Ghost.Hunted > 4)   //Hunt the ghost and stop hunting when time is running out
                        {
                            score += Math.Exp(4) * m_PerceptionDepth * Math.Exp(-cnt);
                        }
                        else
                        {
                            score -= Math.Exp(6) * m_PerceptionDepth * Math.Exp(-cnt);
                        }
                        break;
                    }
                }

                //Get neighours in queue
                if (map.Nodes[current_x == 0 ? mapwidth - 1 : current_x - 1, current_y].GetNodeType() != Node.NodeType.WALL
                    && flag[current_x == 0 ? mapwidth - 1 : current_x - 1, current_y] == 0)
                {
                    queue.In(map.Nodes[current_x == 0 ? mapwidth - 1 : current_x - 1, current_y]);
                }
                if (map.Nodes[current_x == mapwidth - 1 ? 0 : current_x + 1, current_y].GetNodeType() != Node.NodeType.WALL
                    && flag[current_x == mapwidth - 1 ? 0 : current_x + 1, current_y] == 0)
                {
                    queue.In(map.Nodes[current_x == mapwidth - 1 ? 0 : current_x + 1, current_y]);
                }
                if (map.Nodes[current_x, current_y == 0 ? mapheight - 1 : current_y - 1].GetNodeType() != Node.NodeType.WALL
                    && flag[current_x, current_y == 0 ? mapheight - 1 : current_y - 1] == 0)
                {
                    queue.In(map.Nodes[current_x, current_y == 0 ? mapheight - 1 : current_y - 1]);
                }
                if (map.Nodes[current_x, current_y == mapheight - 1 ? 0 : current_y + 1].GetNodeType() != Node.NodeType.WALL
                    && flag[current_x, current_y == mapheight - 1 ? 0 : current_y + 1] == 0)
                {
                    queue.In(map.Nodes[current_x, current_y == mapheight - 1 ? 0 : current_y + 1]);
                }

                // add cnt
                cnt++;
            }

            return score;
        }
        public void Draw(Graphics g)
        {
            Rectangle position = new Rectangle();
            position.X = m_X * Node.m_Width - 3;
            position.Y = m_Y * Node.m_Height - 1;
            position.Width = 14;
            position.Height = 14;

            int offset = 0;
            if (ClockCounter < 2)
            {
                offset = position.Width;
            }

            switch (m_Direction)
            {
                case (Node.Direction.LEFT):
                    offset += position.Width * 4;
                    position.X = position.X + (3 - PacmanForm.DrawClock) * 2;
                    break;
                case (Node.Direction.RIGHT):
                    offset += position.Width * 6;
                    position.X = position.X - (3 - PacmanForm.DrawClock) * 2;
                    break;
                case (Node.Direction.DOWN):
                    offset += position.Width * 2;
                    position.Y = position.Y - (3 - PacmanForm.DrawClock) * 2;
                    break;
                default:
                    position.Y = position.Y + (3 - PacmanForm.DrawClock) * 2;
                    break;
            }

            ClockCounter = (ClockCounter + 1) % 4;      //Update clockcounter
            g.DrawImage(m_Character, position, new Rectangle(offset, 0, 14, 14), GraphicsUnit.Pixel);
        }
    }
}
