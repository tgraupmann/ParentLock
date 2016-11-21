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
        private bool _mDialogOpen = false;
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

        private const string MINUTES_5 = "5 Minutes";
        private const string MINUTES_10 = "10 Minutes";
        private const string MINUTES_15 = "15 Minutes";
        private const string MINUTES_30 = "30 Minutes";
        private const string MINUTES_45 = "45 Minutes";
        private const string MINUTES_60 = "60 Minutes";
        private const string MINUTES_90 = "90 Minutes";

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
            this.BringToFront();
            this.MinimizeBox = false;
            this.btnSetPassword.Enabled = false;
            this.btnExit.Enabled = false;
            this.btnAddTime.Enabled = false;
            this.cboTime.Enabled = false;
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

            this.FormBorderStyle = FormBorderStyle.None;

            cboTime.Items.Add(MINUTES_5);
            cboTime.Items.Add(MINUTES_10);
            cboTime.Items.Add(MINUTES_15);
            cboTime.Items.Add(MINUTES_30);
            cboTime.Items.Add(MINUTES_45);
            cboTime.Items.Add(MINUTES_60);
            cboTime.Items.Add(MINUTES_90);
            cboTime.SelectedIndex = 0;

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

        public static class Prompt
        {
            public static string ShowDialog(string caption, string text)
            {
                Form prompt = new Form()
                {
                    Width = 500,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };
                Label textLabel = new Label() { Left = 50, Top = 20, Width = 400, Text = text };
                TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
                textBox.PasswordChar = '*';
                Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
            }
        }

        private void btnSetPassword_Click(object sender, EventArgs e)
        {
            _mDialogOpen = true;
            string promptValue = Prompt.ShowDialog("Confirm", "Reenter previous password to confirm!");
            _mDialogOpen = false;
            if (promptValue != _mPassword)
            {
                return;
            }
            
            _mPassword = txtPassword.Text;
            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(KEY_PARENT_LOCK);
            key.SetValue(KEY_PARENT_LOCK_PASSWORD, _mPassword);
            key.Close();
            txtPassword.Text = "";
            _mUnlocked = false;
            Lock();
        }

        private void btnAddTime_Click(object sender, EventArgs e)
        {
            if (null != cboTime.SelectedItem)
            {
                int minutes = 0;
                switch ((string)cboTime.SelectedItem)
                {
                    case MINUTES_5:
                        minutes = 5;
                        break;
                    case MINUTES_10:
                        minutes = 10;
                        break;
                    case MINUTES_15:
                        minutes = 15;
                        break;
                    case MINUTES_30:
                        minutes = 30;
                        break;
                    case MINUTES_45:
                        minutes = 45;
                        break;
                    case MINUTES_60:
                        minutes = 60;
                        break;
                    case MINUTES_90:
                        minutes = 90;
                        break;
                    default:
                        return;
                }

                if (_mTempUnlock < DateTime.Now)
                {
                    _mTempUnlock = DateTime.Now + TimeSpan.FromMinutes(minutes);
                }
                else
                {
                    _mTempUnlock = _mTempUnlock + TimeSpan.FromMinutes(minutes);
                }
                txtPassword.Text = "";
                _mUnlocked = false;
                Lock();
            }
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

        private bool IsAllowedTime()
        {
            return (DateTime.Now.DayOfWeek == DayOfWeek.Sunday ||
                DateTime.Now.DayOfWeek == DayOfWeek.Monday ||
                DateTime.Now.DayOfWeek == DayOfWeek.Tuesday ||
                DateTime.Now.DayOfWeek == DayOfWeek.Wednesday ||
                DateTime.Now.DayOfWeek == DayOfWeek.Thursday ||
                DateTime.Now.DayOfWeek == DayOfWeek.Friday ||
                DateTime.Now.DayOfWeek == DayOfWeek.Saturday);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_mDialogOpen)
            {
                return;
            }

            if (DateTime.Now < _mTempUnlock)
            {
                Unlock();
                TimeSpan timeleft = _mTempUnlock - DateTime.Now;
                if (timeleft.Hours > 0)
                {
                    lblPassword.Text = string.Format("UNLOCKED - TIME {0} HOURS {1} MINUTES",
                        timeleft.Hours, timeleft.Minutes);
                }
                else
                {
                    lblPassword.Text = string.Format("UNLOCKED - TIME {0} MINUTES {1} SECONDS",
                        timeleft.Minutes, timeleft.Seconds);
                }
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

            else if (IsAllowedTime() &&
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
                this.btnAddTime.Enabled = true;
                this.cboTime.Enabled = true;
                this.btnLock.Enabled = true;
            }
        }
    }
}
