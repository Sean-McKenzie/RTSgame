#region
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;
#endregion
namespace Game
{
    public partial class FrmMain : Form
    {
        Game game;
        int LmouseDownX, LmouseDownY, RmouseDownX, RmouseDownY; //Mouse XY Coords
        int LmouseUpX, LmouseUpY, RmouseUpX, RmouseUpY; // Mouse XY coords
        protected List<ILocatables> superAtlas = new List<ILocatables>();
        
        private void FrmMain_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            UpdateStyles(); //Reduces Flicker
            List<string> teamNames = new List<string>();
            teamNames.Add("marauders");

            game = new Game(teamNames);
            game.Update += Game_Update;

            game.Start();
        }

        private void Game_Update(List<ILocatables> superAtlas, List<Player> player)
        {
            this.superAtlas = superAtlas;
            Invalidate();
        }
        public FrmMain()
        {
            InitializeComponent();
            MouseDown += FrmMain_MouseDown;
            MouseUp += FrmMain_MouseUp;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics pic = e.Graphics;
            pic.Clear(Color.Black);
            foreach (ILocatables loc in superAtlas)
            {
                pic.TranslateTransform((float)loc.Pos.X, (float)loc.Pos.Y);
                pic.RotateTransform((float)loc.Angle);
                pic.TranslateTransform(-loc.Width / 2, -loc.Height / 2);
                pic.DrawImage(loc.Img, new Point());
                pic.ResetTransform();
            }

        }
        #region
        private void FrmMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                LmouseUpX = e.X;
                LmouseUpY = e.Y;
            }
            else if (e.Button == MouseButtons.Right)
            {
                RmouseUpX = e.X;
                RmouseUpY = e.Y;
            }
            if (e.Button == MouseButtons.Left && LmouseDownX == LmouseUpX && LmouseDownY == LmouseUpY) // Single Selection / left click
            {
                UserAction ua = UserAction.Sel;
                Vector Pos = new Vector(LmouseDownX, LmouseDownY); // maybe useless
                Rectangle r = new Rectangle(LmouseDownX, LmouseDownY, 0, 0); // useless
                UserInput ui = new UserInput(Pos, r, ua); //
                game.SetUserInput(ui);
            } // end of single selection, passed into game object using SetUserInput method


            else if (e.Button == MouseButtons.Right) //&& RmouseDownX == RmouseUpX && RmouseDownY == RmouseUpY) // Right click
            {
                UserAction ua = UserAction.Do;
                Vector Pos = new Vector(RmouseDownX, RmouseDownY);
                Rectangle r = new Rectangle();
                UserInput ui = new UserInput(Pos, r, ua);
                Text = "DO Called: POSITION X: " + Pos.X + ", Y: " + Pos.Y;

                game.SetUserInput(ui);
            } // end of right click, passed into game object using SetUserInput method


            else if (LmouseDownX != LmouseUpX || LmouseDownY != LmouseUpY) // Multi Selection / left click and drag over group
            {
                UserAction use = UserAction.SelMult;

                int width = Math.Abs(LmouseDownX - LmouseUpX); // maybe useless
                int height = Math.Abs(LmouseDownY - LmouseUpY); // maybe useless
                Rectangle rec;
                if (LmouseUpX > LmouseDownX) // Dragging to the right
                {
                    if (LmouseUpY > LmouseDownY) // lower right
                    {
                        rec = new Rectangle(LmouseDownX, LmouseDownY, width, height);

                    }
                    else
                    {
                        rec = new Rectangle(LmouseDownX, LmouseUpY, width, height);
                    }
                }
                else // dragging to the left
                {
                    if (LmouseUpY < LmouseDownY) // Upper Left
                    {
                        rec = new Rectangle(LmouseUpX, LmouseUpY, width, height);

                    }
                    else // lower Left
                    {
                        rec = new Rectangle(LmouseUpX, LmouseDownY, width,height);
                    }
                }
                Vector Pos = new Vector(width / 2, height / 2); // maybe useless
                UserInput ui = new UserInput(Pos, rec, use);
                Text = "Rect: " + rec.X + "," + rec.Y + ", Width:" + rec.Width + ", Height:" + rec.Height;
                game.SetUserInput(ui);
            } // end of click and drag over, passed into game object using SetUserInput method
        }
        private void FrmMain_MouseDown(object sender, MouseEventArgs e)
        {
            Text = " ";
            if (e.Button == MouseButtons.Left)
            {
                LmouseDownX = e.X;
                LmouseDownY = e.Y;
            }
            else if (e.Button == MouseButtons.Right)
            {
                RmouseDownX = e.X;
                RmouseDownY = e.Y;
            }
        }
        #endregion
       
    }
}