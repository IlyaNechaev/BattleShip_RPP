using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battleship
{
    public partial class BattleshipMainForm : Form
    {
        GFX.DrawBuffer buffer1;

        public BattleshipMainForm()
        {
            InitializeComponent();
            buffer1 = new GFX.DrawBuffer(pictureBox1);

            DrawShips();

            buffer1.Update();
        }

        private void DrawShips()
        {
            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 10; x++)
                    buffer1.DrawCell(x, y);

            buffer1.DrawBattleship(0, 0, Direction.North);
            buffer1.DrawBattleship(1, 0, Direction.East);
            buffer1.DrawBattleship(5, 0, Direction.South);
            buffer1.DrawBattleship(6, 0, Direction.West);

            buffer1.DrawCruiser(0, 4, Direction.North);
            buffer1.DrawCruiser(1, 4, Direction.East);
            buffer1.DrawCruiser(4, 4, Direction.South);
            buffer1.DrawCruiser(5, 4, Direction.West);

            buffer1.DrawDestroyer(0, 7, Direction.North);
            buffer1.DrawDestroyer(1, 7, Direction.East);
            buffer1.DrawDestroyer(3, 7, Direction.South);
            buffer1.DrawDestroyer(4, 7, Direction.West);

            buffer1.DrawPatrol(0, 9, Direction.North);
            buffer1.DrawPatrol(1, 9, Direction.East);
            buffer1.DrawPatrol(2, 9, Direction.South);
            buffer1.DrawPatrol(3, 9, Direction.West);


            buffer1.DrawMine(4, 9);
            buffer1.DrawMine(5, 9);
            buffer1.DrawMine(6, 9);
            buffer1.DrawMine(7, 9);

            buffer1.DrawShootedMark(8, 9);
            buffer1.DrawDestroyedMark(9, 9);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buffer1.StartShipExplosion(0, 0);
        }

        private void gfx_timer_Tick(object sender, EventArgs e)
        {
            DrawShips();
            buffer1.NewFrame();
            buffer1.Update();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            buffer1.StartMineExplosion(5, 5);
        }
    }

    internal enum Direction { North, West, South, East };

    internal static class GFX
    {
        static Image battleship;
        static Image destroyer;
        static Image cruiser;
        static Image patrol;
        static Image mine;
        static Image cell;
        static Image[] ship_explosion;
        static Image[] mine_explosion;

        static int cellsize = 48;

        static public int CellSize { get { return cellsize; } set { cellsize = value; CreateCell(); } }

        static void CreateCell()
        {
            Bitmap ACell = new Bitmap(cellsize, cellsize);
            Graphics gfx = Graphics.FromImage(ACell);
            gfx.FillRectangle(new SolidBrush(Color.FromArgb(0x40, 0x50, 0xFF)), new Rectangle(0, 0, cellsize-1, cellsize-1));
            gfx.DrawRectangle(new Pen(Color.FromArgb(0, 0, 0xC0)), new Rectangle(0, 0, cellsize-1, cellsize-1));
            cell = ACell;
        }

        static GFX()
        {
            battleship = Image.FromFile("battleship.png");
            destroyer = Image.FromFile("destroyer.png");
            cruiser = Image.FromFile("cruiser.png");
            patrol = Image.FromFile("patrol.png");
            mine = Image.FromFile("mine.png");

            List<Image> explosions = new List<Image>();
            int i = 0;
            while (File.Exists(String.Format("expl_ship_{0:D2}.png", i)))
            {
                explosions.Add(Image.FromFile(String.Format("expl_ship_{0:D2}.png", i)));
                i++;
            }
            ship_explosion = explosions.ToArray();

            explosions.Clear();
            i = 0;
            while (File.Exists(String.Format("expl_mine_{0:D2}.png", i)))
            {
                explosions.Add(Image.FromFile(String.Format("expl_mine_{0:D2}.png", i)));
                i++;
            }
            mine_explosion = explosions.ToArray();


            CreateCell();
        }

        internal class DrawBuffer
        {
            Bitmap buffer;
            Graphics gfx;
            PictureBox output;

            List<Effect> effects = new List<Effect>();

            class Effect
            {
                public int frame;
                public Image[] frames;
                public int x, y, w, h;
            }

            internal DrawBuffer(PictureBox output)
            {
                this.output = output;
                buffer = new Bitmap(output.Width, output.Height);
                gfx = Graphics.FromImage(buffer);
            }

            internal void Update()
            {
                output.Image = buffer;
            }

            internal void DrawCell(int x, int y)
            {
                gfx.DrawImage(cell, x*cellsize, y*cellsize, cell.Width, cell.Height);
            }

            void DrawShip(Image ship, int shipsize, int x, int y, Direction direction)
            {
                x *= cellsize;
                y *= cellsize;
                Point[] dest = new Point[3];
                switch (direction)
                {
                    case Direction.North:
                        dest[0] = new Point(x + (cellsize - ship.Width) / 2, y + (cellsize * shipsize - ship.Height) / 2);
                        dest[1] = new Point(x + (cellsize - ship.Width) / 2 + ship.Width, y + (cellsize * shipsize - ship.Height) / 2);
                        dest[2] = new Point(x + (cellsize - ship.Width) / 2, y + (cellsize * shipsize - ship.Height) / 2 + ship.Height);
                        break;
                    case Direction.East:
                        dest[0] = new Point(x + (cellsize * shipsize - ship.Height) / 2 + ship.Height, y + (cellsize - ship.Width) / 2);
                        dest[1] = new Point(x + (cellsize * shipsize - ship.Height) / 2 + ship.Height, y + (cellsize - ship.Width) / 2 + ship.Width);
                        dest[2] = new Point(x + (cellsize * shipsize - ship.Height) / 2, y + (cellsize - ship.Width) / 2);
                        break;
                    case Direction.South:
                        dest[0] = new Point(x + (cellsize - ship.Width) / 2 + ship.Width, y + (cellsize * shipsize - ship.Height) / 2 + ship.Height);
                        dest[1] = new Point(x + (cellsize - ship.Width) / 2, y + (cellsize * shipsize - ship.Height) / 2 + ship.Height);
                        dest[2] = new Point(x + (cellsize - ship.Width) / 2 + ship.Width, y + (cellsize * shipsize - ship.Height) / 2);
                        break;
                    case Direction.West:
                        dest[0] = new Point(x + (cellsize * shipsize - ship.Height) / 2, y + (cellsize - ship.Width) / 2 + ship.Width);
                        dest[1] = new Point(x + (cellsize * shipsize - ship.Height) / 2, y + (cellsize - ship.Width) / 2);
                        dest[2] = new Point(x + (cellsize * shipsize - ship.Height) / 2 + ship.Height, y + (cellsize - ship.Width) / 2 + ship.Width);
                        break;
                }
                gfx.DrawImage(ship, dest);
            }

            internal void DrawBattleship(int x, int y, Direction direction)
            {
                DrawShip(battleship, 4, x, y, direction);
            }

            internal void DrawCruiser(int x, int y, Direction direction)
            {
                DrawShip(cruiser, 3, x, y, direction);
            }

            internal void DrawDestroyer(int x, int y, Direction direction)
            {
                DrawShip(destroyer, 2, x, y, direction);
            }

            internal void DrawPatrol(int x, int y, Direction direction)
            {
                DrawShip(patrol, 1, x, y, direction);
            }

            internal void DrawMine(int x, int y)
            {
                gfx.DrawImage(mine, x*cellsize + 10, y*cellsize + 10, cellsize - 20, cellsize - 20);
            }

            internal void DrawShootedMark(int x, int y)
            {
                gfx.DrawEllipse(new Pen(Color.FromArgb(0, 0, 0x80), 4), new Rectangle(x*cellsize+6, y*cellsize+6, cellsize-12, cellsize-12));
            }

            internal void DrawDestroyedMark(int x, int y)
            {
                Pen red = new Pen(Color.FromArgb(0xD0, 0, 0), 6);
                gfx.DrawLine(red, x * cellsize + 6, y * cellsize + 6, (x + 1) * cellsize - 6, (y + 1) * cellsize - 6);
                gfx.DrawLine(red, (x + 1) * cellsize - 6, y * cellsize + 6, x * cellsize + 6, (y + 1) * cellsize - 6);
            }

            internal void StartShipExplosion(int x, int y)
            {
                Effect ne = new Effect();
                ne.frame = 0;
                ne.frames = ship_explosion;
                ne.x = x*CellSize;
                ne.y = y*CellSize;
                ne.w = CellSize;
                ne.h = CellSize;
                effects.Add(ne);
            }

            internal void StartMineExplosion(int x, int y)
            {
                Effect ne = new Effect();
                ne.frame = 0;
                ne.frames = mine_explosion;
                ne.x = (x-1) * CellSize;
                ne.y = (y-1) * CellSize;
                ne.w = CellSize*3;
                ne.h = CellSize*3;
                effects.Add(ne);
            }

            internal void NewFrame()
            {
                int i = 0;
                while (i < effects.Count)
                {
                    Effect e = effects[i];
                    gfx.DrawImage(e.frames[e.frame], e.x, e.y, e.w, e.h);
                    e.frame++;
                    if (e.frame >= e.frames.Length)
                        effects.RemoveAt(i);
                    else
                        i++;
                }
            }
        }
    }
}
