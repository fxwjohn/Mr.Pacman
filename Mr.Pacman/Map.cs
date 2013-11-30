using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Mr.Pacman
{
    class Map
    {
        /*Data*/
        public Node[,] Nodes = null;    //Data structure for all nodes

        private Size m_Size = new Size();       //map size
        private Image m_ImgMaze = null;         //map background image
        private const int LeftSpace = 3;        //offset
        private const int TopSpace = 3;         //offset

        /*Method*/
        public Map()
        {
            m_Size.Width = 28;
            m_Size.Height = 31;
            m_ImgMaze = Image.FromFile(System.IO.Directory.GetCurrentDirectory()  + "\\maze.jpeg");
            
            Nodes = new Node[m_Size.Width, m_Size.Height];
            for (int i = 0; i < m_Size.Width; i++)
            {
                for (int j = 0; j < m_Size.Height; j++)
                {
                    Nodes[i, j] = new Node();
                }
            }

            AnalyseMap(m_ImgMaze);
        } 
        public void ChangeSize(Size size)
        {
            m_Size = size;
        }
        public Size GetSize()
        {
            return m_Size;
        }
        public void ChangeMap(Image img)
        {
            m_ImgMaze = img;
        }
        public void AnalyseMap(Image img)
        {
            Bitmap bitmap = (Bitmap)img;
            //Choosing the type for every node
            for (int i = 0; i < m_Size.Width; i++)
            {
                for (int j = 0; j < m_Size.Height; j++)
                {
                    Color color = bitmap.GetPixel(LeftSpace + i * Node.m_Width, TopSpace + j * Node.m_Height);
                    //Judging node type by analyzing pixel
                    if (color == Color.FromArgb(255, 242, 0))
                    {
                        if (bitmap.GetPixel(LeftSpace + i * Node.m_Width + 2, TopSpace + j * Node.m_Height)
                           == Color.FromArgb(255,242, 0))
                        {
                            Nodes[i, j].SetNodeType(Node.NodeType.POWERPILL);
                        }
                        else
                        {
                            Nodes[i, j].SetNodeType(Node.NodeType.PILL);
                        }
                    }
                    else if(color == Color.FromArgb(0,0,0))
                    {
                        int flag = 0;
                        for (int k = -2; k < 3; k++)
                        {
                            if (bitmap.GetPixel(LeftSpace + Node.m_Width * i + k, TopSpace - 2 + Node.m_Height * j) != Color.FromArgb(0, 0, 0))
                            {
                                flag = 1;
                                break;
                            }
                            else if (bitmap.GetPixel(LeftSpace + Node.m_Width * i + k, TopSpace + 2 + Node.m_Height * j) != Color.FromArgb(0, 0, 0))
                            {
                                flag = 1;
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        for (int k = -1; k < 2; k++)
                        {
                            if (bitmap.GetPixel(LeftSpace - 2 + Node.m_Width * i, TopSpace + Node.m_Height * j + k) != Color.FromArgb(0, 0, 0))
                            {
                                flag = 1;
                                break;
                            }
                            else if (bitmap.GetPixel(LeftSpace + 2 + Node.m_Width * i, TopSpace + Node.m_Height * j + k) != Color.FromArgb(0, 0, 0))
                            {
                                flag = 1;
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        if (flag == 1)
                        {
                            Nodes[i, j].SetNodeType(Node.NodeType.WALL);
                        }
                        else
                        {
                            Nodes[i, j].SetNodeType(Node.NodeType.PILL);
                        }
                    }
                    else
                    {
                        Nodes[i, j].SetNodeType(Node.NodeType.WALL);
                    }
                    //Set node's position
                    Nodes[i, j].SetPosition(i, j);
                }
            }

            //Calculate shortest path between nodes
            foreach (Node node in Nodes)
            {
                if (node.GetNodeType() == Node.NodeType.PILL || node.GetNodeType() == Node.NodeType.POWERPILL)
                {
                    BFS(node);
                }    
            }
        }
        private void BFS(Node node)
        {
            Queue queue = new Queue();
            int x = node.GetPoint().X;
            int y = node.GetPoint().Y;
            int width = m_Size.Width;
            int height = m_Size.Height;

            //Record visited node
            int[,] flag = new int[28, 31];
            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 31; j++)
                {
                    flag[i, j] = 0;
                }
            }
            flag[x, y] = 1;

            node.m_DisMatrix[x, y] = 0;
            //Get neighbors in queue
            if((Nodes[x == 0 ? width - 1 : x - 1, y].GetNodeType() == Node.NodeType.PILL ||
                Nodes[x == 0 ? width - 1 : x - 1, y].GetNodeType() == Node.NodeType.POWERPILL) &&
                flag[x == 0 ? width - 1 : x - 1, y] != 1)
            {
                node.m_DisMatrix[x == 0 ? width - 1 : x - 1, y] = 1;
                node.m_DirMatrix[x == 0 ? width - 1 : x - 1, y] = Node.Direction.LEFT;
                queue.In(Nodes[x == 0 ? width - 1 : x - 1, y]);
            }
            if ((Nodes[x == width - 1 ? 0 : x + 1, y].GetNodeType() == Node.NodeType.PILL ||
                Nodes[x == width - 1 ? 0 : x + 1, y].GetNodeType() == Node.NodeType.POWERPILL) &&
                flag[x == width - 1 ? 0 : x + 1, y] != 1)
            {
                node.m_DisMatrix[x == width - 1 ? 0 : x + 1, y] = 1;
                node.m_DirMatrix[x == width - 1 ? 0 : x + 1, y] = Node.Direction.RIGHT;
                queue.In(Nodes[x == width - 1 ? 0 : x + 1, y]);
            }
            if ((Nodes[x, y == 0 ? height - 1 : y - 1].GetNodeType() == Node.NodeType.PILL ||
                Nodes[x, y == 0 ? height - 1 : y - 1].GetNodeType() == Node.NodeType.POWERPILL) &&
                flag[x, y == 0 ? height - 1 : y - 1] != 1)
            {
                node.m_DisMatrix[x, y == 0 ? height - 1 : y - 1] = 1;
                node.m_DirMatrix[x, y == 0 ? height - 1 : y - 1] = Node.Direction.UP;
                queue.In(Nodes[x, y == 0 ? height - 1 : y - 1]);
            }
            if ((Nodes[x, y == height - 1 ? 0 : y + 1].GetNodeType() == Node.NodeType.PILL ||
                Nodes[x, y == height - 1 ? 0 : y + 1].GetNodeType() == Node.NodeType.POWERPILL) &&
                flag[x, y == height - 1 ? 0 : y + 1] != 1)
            {
                node.m_DisMatrix[x, y == height - 1 ? 0 : y + 1] = 1;
                node.m_DirMatrix[x, y == height - 1 ? 0 : y + 1] = Node.Direction.DOWN;
                queue.In(Nodes[x, y == height - 1 ? 0 : y + 1]);
            }
            //Breadth first search
            while (queue.Empty() != 1)
            {
                Node current = queue.Out();
                x = current.GetPoint().X;
                y = current.GetPoint().Y;
                flag[x, y] = 1;
                //Expand a node, getting neighbors in queue
                if ((Nodes[x == 0 ? width - 1 : x - 1, y].GetNodeType() == Node.NodeType.PILL ||
                Nodes[x == 0 ? width - 1 : x - 1, y].GetNodeType() == Node.NodeType.POWERPILL) &&
                flag[x == 0 ? width - 1 : x - 1, y] != 1)
                {
                    //Update direction and distance information
                    node.m_DisMatrix[x == 0 ? width - 1 : x - 1, y] = node.m_DisMatrix[x, y] + 1;
                    node.m_DirMatrix[x == 0 ? width - 1 : x - 1, y] = node.m_DirMatrix[x, y];
                    queue.In(Nodes[x == 0 ? width - 1 : x - 1, y]);
                }
                if ((Nodes[x == width - 1 ? 0 : x + 1, y].GetNodeType() == Node.NodeType.PILL ||
                    Nodes[x == width - 1 ? 0 : x + 1, y].GetNodeType() == Node.NodeType.POWERPILL) &&
                    flag[x == width - 1 ? 0 : x + 1, y] != 1)
                {
                    node.m_DisMatrix[x == width - 1 ? 0 : x + 1, y] = node.m_DisMatrix[x, y] + 1;
                    node.m_DirMatrix[x == width - 1 ? 0 : x + 1, y] = node.m_DirMatrix[x, y];
                    queue.In(Nodes[x == width - 1 ? 0 : x + 1, y]);
                }
                if ((Nodes[x, y == 0 ? height - 1 : y - 1].GetNodeType() == Node.NodeType.PILL ||
                    Nodes[x, y == 0 ? height - 1 : y - 1].GetNodeType() == Node.NodeType.POWERPILL) &&
                    flag[x, y == 0 ? height - 1 : y - 1] != 1)
                {
                    node.m_DisMatrix[x, y == 0 ? height - 1 : y - 1] = node.m_DisMatrix[x, y] + 1;
                    node.m_DirMatrix[x, y == 0 ? height - 1 : y - 1] = node.m_DirMatrix[x, y];
                    queue.In(Nodes[x, y == 0 ? height - 1 : y - 1]);
                }
                if ((Nodes[x, y == height - 1 ? 0 : y + 1].GetNodeType() == Node.NodeType.PILL ||
                    Nodes[x, y == height - 1 ? 0 : y + 1].GetNodeType() == Node.NodeType.POWERPILL) &&
                    flag[x, y == height - 1 ? 0 : y + 1] != 1)
                {
                    node.m_DisMatrix[x, y == height - 1 ? 0 : y + 1] = node.m_DisMatrix[x, y] + 1;
                    node.m_DirMatrix[x, y == height - 1 ? 0 : y + 1] = node.m_DirMatrix[x, y];
                    queue.In(Nodes[x, y == height - 1 ? 0 : y + 1]);
                }
            }
        }
        public void Reset() //Reset map to initial state
        {
            foreach (Node node in Nodes)
            {
                if (node.GetNodeType() == Node.NodeType.EATEN)
                {
                    node.SetNodeType(Node.NodeType.PILL);
                }
            }

            Nodes[1, 3].SetNodeType(Node.NodeType.POWERPILL);
            Nodes[26, 3].SetNodeType(Node.NodeType.POWERPILL);
            Nodes[1, 23].SetNodeType(Node.NodeType.POWERPILL);
            Nodes[26, 23].SetNodeType(Node.NodeType.POWERPILL);
        }
        public void Draw(Graphics g)
        {
            Point[] bounds = new Point[3];
            bounds[0] = new Point(0, 0);
            bounds[1] = new Point(m_ImgMaze.Width, 0);
            bounds[2] = new Point(0, m_ImgMaze.Height);

            g.DrawImage(m_ImgMaze, bounds);
            foreach (Node node in Nodes)
            {
                node.Draw(g);
            }
        }
    }
}
