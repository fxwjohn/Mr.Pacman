using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mr.Pacman
{
    public partial class PacmanForm : Form
    {
        private Map m_Map = new Map();
        private Pacman m_Pacman = new Pacman();
        private Ghost[] m_Ghost = new Ghost[GhostNum];
        public const int GhostNum = 3;

        //instrumental variable for drawing
        private int GhostTimer = 0;         
        public static int DrawClock = 0;    

        //double buffering
        private Bitmap dbBitmap = null;
        private Graphics dbGraphics = null;

        public PacmanForm()
        {
            for (int i = 0; i < GhostNum; i++)
            {
                m_Ghost[i] = new Ghost();
            }
            m_Ghost[1].SetPosition(21, 6);
            m_Ghost[1].SetTag(1);
            m_Ghost[2].SetPosition(24, 29);
            m_Ghost[2].SetTag(2);

            InitializeComponent();
            this.SetStyle(ControlStyles.Opaque, true);  
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (dbBitmap != null)
            {
                dbBitmap.Dispose();
            }
            if (dbGraphics != null)
            {
                dbGraphics.Dispose();
            }
            dbBitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
            dbGraphics = Graphics.FromImage(dbBitmap);
            m_Map.Draw(dbGraphics);
            for (int i = 0; i < GhostNum; i++)
            {
                m_Ghost[i].Draw(dbGraphics);
            }
            m_Pacman.Draw(dbGraphics);
            e.Graphics.DrawImageUnscaled(dbBitmap, new Point(0, 0));
        }
        private void PacmanTimer_Tick(object sender, EventArgs e)
        {
            DrawClock++;
            if (DrawClock == 4)
            {
                m_Pacman.Move(m_Map, m_Ghost);
                GhostTimer++;
                int Ghoststop = 1;
                if (Ghost.Hunted > 0)
                {
                    Ghoststop = 2;
                }
                else
                {
                    Ghoststop = 1;
                }
                if (GhostTimer == Ghoststop)
                {
                    GhostTimer = 0;
                    for (int i = 0; i < GhostNum; i++)
                    {
                        m_Ghost[i].Move(m_Map, m_Pacman, (i + 1) * 5);
                    }
                }
                DrawClock = 0;
            }

            Invalidate();

            //Check whether all pills have been eaten
            if (m_Pacman.GetPillCnt() % 300 == 0)
            {
                m_Map.Reset();
            }
        }
        private void PacmanForm_Load(object sender, EventArgs e)
        {
            
        }
    }
}
