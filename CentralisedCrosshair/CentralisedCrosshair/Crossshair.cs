using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CentralisedCrosshair
{
    public partial class Crossshair : Form
    {
        public Crossshair()
        {
            InitializeComponent();
        }

        #region Events
        private void Crossshair_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = this.TransparencyColor;
            this.TransparencyKey = this.TransparencyColor;

            this.BackgroundImage = this.BackgroundImage = global::CentralisedCrosshair.Properties.Resources.precision;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

            this.ClientSize = this.BackgroundImage.Size;

            centraliseWindow();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (ctrlDown)
            {
                Point newPosition = new Point()
                {
                    X = this.Location.X,
                    Y = this.Location.Y
                };

                switch (e.KeyCode)
                {
                    case (Keys.W):
                        newPosition.Y = this.Location.Y - 1;
                        break;
                    case (Keys.S):
                        newPosition.Y = this.Location.Y + 1;
                        break;
                    case (Keys.A):
                        newPosition.X = this.Location.X - 1;
                        break;
                    case (Keys.D):
                        newPosition.X = this.Location.X + 1;
                        break;
                }

                this.Location = newPosition;
            }
            else if (e.KeyCode == Keys.ControlKey)
            {
                ctrlDown = true;
            }

        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                ctrlDown = false;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            int wl = GetWindowLong(this.Handle, GWL.ExStyle);
            wl = wl | 0x80000 | 0x20;
            SetWindowLong(this.Handle, GWL.ExStyle, wl);

        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (ctrlDown)
            {
                switch (e.KeyChar)
                {
                    // 'C'
                    case ('\u0003'):
                        centraliseWindow();
                        break;

                    // 'F'
                    case ('\u0006'):
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Filter = "Portable Network Graphics (*.png)|*.PNG";

                        if (fileDialog.ShowDialog() == DialogResult.OK)
                        {
                            this.BackgroundImage = Image.FromFile(fileDialog.FileName);
                            this.Size = this.BackgroundImage.Size;
                            this.TopMost = true;
                        }

                        break;

                    // 'R'
                    case ('\u0012'):
                        this.BackgroundImage = global::CentralisedCrosshair.Properties.Resources.precision;
                        this.Size = this.BackgroundImage.Size;
                        this.TopMost = true;
                        break;

                }
            }
        }
        #endregion

        #region Enums
        public enum GWL
        {
            ExStyle = -20
        }

        public enum WS_EX
        {
            Transparent = 0x20,
            Layered = 0x80000
        }

        public enum LWA
        {
            ColorKey = 0x1,
            Alpha = 0x2
        }
        #endregion

        #region DLL Imports
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern int GetWindowLong(IntPtr hWnd, GWL nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong(IntPtr hWnd, GWL nIndex, int dwNewLong);
        #endregion

        #region Private Methods
        private void centraliseWindow()
        {
            Screen mainScreen = Screen.PrimaryScreen;

            this.SetBounds(Convert.ToInt32(mainScreen.Bounds.Width / 2 - this.BackgroundImage.Width / 2),
                Convert.ToInt32(mainScreen.Bounds.Height / 2 - this.BackgroundImage.Height / 2),
                this.BackgroundImage.Width,
                this.BackgroundImage.Height);
        }
        #endregion

        #region Properties
        private bool ctrlDown = false;
        #endregion
    }
}
