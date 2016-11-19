using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace ParentLock
{
    public partial class Form1 : Form
    {
        private bool _mUnlocked = false;
        private bool _mCancelExit = true;
        private string _mPassword = "1111"; //default password
        private DateTime _mTempUnlock = DateTime.MinValue;
        private int _mHourAwake = 6;
        private int _mHourAsleep = 22;

        private const string KEY_PARENT_LOCK = "PARENT_LOCK";
        private const string KEY_PARENT_LOCK_PASSWORD = "PARENT_LOCK_PASSWORD";

        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg,
            IntPtr wParam, IntPtr lParam);

        public Form1()
        {
            InitializeComponent();
        }

        private void Unlock()
        {
            this.TopMost = false;
            this.MinimizeBox = true;
            this.ShowInTaskbar = true;
        }

        private void Lock()
        {
            this.TopMost = true;
            this.MinimizeBox = false;
            this.btnSetPassword.Enabled = false;
            this.btnExit.Enabled = false;
            this.btn30Min.Enabled = false;
            this.btn1Hour.Enabled = false;
            this.btnLock.Enabled = false;
            this.ShowInTaskbar = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Microsoft.Win32.RegistryKey key;
            foreach (string name in Microsoft.Win32.Registry.CurrentUser.GetSubKeyNames())
            {
                if (name == KEY_PARENT_LOCK)
                {
                    key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(KEY_PARENT_LOCK);
                    if (null != key)
                    {
                        _mPassword = (string)key.GetValue(KEY_PARENT_LOCK_PASSWORD);
                    }
                }
            }

            Lock();
            UpdateLayout();

            this.timer1.Start();
        }

        private void UpdateLayout()
        {
            this.Location = new Point(Screen.PrimaryScreen.Bounds.X,
                Screen.PrimaryScreen.Bounds.Y);
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
        }

        private void Mute()
        {
            SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle,
                (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }

        private void VolDown()
        {
            SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle,
                (IntPtr)APPCOMMAND_VOLUME_DOWN);
        }

        private void VolUp()
        {
            SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle,
                (IntPtr)APPCOMMAND_VOLUME_UP);
        }

        protected override void OnMove(EventArgs e)
        {
            UpdateLayout();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = _mCancelExit;
        }

        private void btnSetPassword_Click(object sender, EventArgs e)
        {
            _mPassword = txtPassword.Text;
            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(KEY_PARENT_LOCK);
            key.SetValue(KEY_PARENT_LOCK_PASSWORD, _mPassword);
            key.Close();
            txtPassword.Text = "";
            _mUnlocked = false;
            Lock();
        }

        private void btn1Hour_Click(object sender, EventArgs e)
        {
            if (_mTempUnlock < DateTime.Now)
            {
                _mTempUnlock = DateTime.Now + TimeSpan.FromHours(1);
            }
            else
            {
                _mTempUnlock = _mTempUnlock + TimeSpan.FromHours(1);
            }
            txtPassword.Text = "";
            _mUnlocked = false;
            Lock();
        }

        private void btn30Min_Click(object sender, EventArgs e)
        {
            if (_mTempUnlock < DateTime.Now)
            {
                _mTempUnlock = DateTime.Now + TimeSpan.FromMinutes(30);
            }
            else
            {
                _mTempUnlock = _mTempUnlock + TimeSpan.FromMinutes(30);
            }
            txtPassword.Text = "";
            _mUnlocked = false;
            Lock();
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            _mTempUnlock = DateTime.Now - TimeSpan.FromSeconds(1);
            txtPassword.Text = "";
            _mUnlocked = false;
            Lock();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            _mCancelExit = false;
            this.Close();
            Application.Exit();
        }

        private bool IsWeekDay()
        {
            return (DateTime.Now.DayOfWeek == DayOfWeek.Monday ||
                DateTime.Now.DayOfWeek == DayOfWeek.Tuesday ||
                DateTime.Now.DayOfWeek == DayOfWeek.Wednesday ||
                DateTime.Now.DayOfWeek == DayOfWeek.Thursday ||
                DateTime.Now.DayOfWeek == DayOfWeek.Friday);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (IsWeekDay() &&
                DateTime.Now.Hour >= _mHourAwake &&
                DateTime.Now.Hour < _mHourAsleep)
            {
                Unlock();
                DateTime asleep = DateTime.Now.Date + TimeSpan.FromHours(_mHourAsleep);
                TimeSpan timeleft = asleep - DateTime.Now;
                if (timeleft.Hours > 0)
                {
                    lblPassword.Text = string.Format("NORMAL USE - TIME {0} HOURS {1} MINUTES",
                        timeleft.Hours, timeleft.Minutes);
                }
                else
                {
                    lblPassword.Text = string.Format("NORMAL USE - TIME {0} MINUTES {1} SECONDS",
                        timeleft.Minutes, timeleft.Seconds);
                }
            }

            else if (DateTime.Now < _mTempUnlock)
            {
                Unlock();
                TimeSpan timeleft = _mTempUnlock - DateTime.Now;
                lblPassword.Text = string.Format("TIME LEFT {0} MINUTES {1} SECONDS",
                    timeleft.Minutes,timeleft.Seconds);
            }

            else if (_mTempUnlock != DateTime.MinValue)
            {
                _mTempUnlock = DateTime.MinValue;
                this.TopMost = true;
                txtPassword.Text = "";
                txtPassword.Focus();
                _mUnlocked = false;
                Lock();
            }

            else
            {
                if (!_mUnlocked)
                {
                    VolDown();
                    lblPassword.Text = "PLEASE ENTER YOUR PASSWORD";
                }
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text == _mPassword)
            {
                lblPassword.Text = "UNLOCKED";
                _mUnlocked = true;
                Unlock();
                this.btnSetPassword.Enabled = true;
                this.btnExit.Enabled = true;
                this.btn30Min.Enabled = true;
                this.btn1Hour.Enabled = true;
                this.btnLock.Enabled = true;
            }
        }
    }
}
