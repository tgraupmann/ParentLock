﻿using System;
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
            this.btnSetPassword.Enabled = true;
            this.btnExit.Enabled = true;
        }

        private void Lock()
        {
            this.TopMost = true;
            this.MinimizeBox = false;
            this.btnSetPassword.Enabled = false;
            this.btnExit.Enabled = false;
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
            Mute();

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
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            _mCancelExit = false;
            this.Close();
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!_mUnlocked)
            {
                VolDown();
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text == _mPassword)
            {
                lblPassword.Text = "UNLOCKED";
                _mUnlocked = true;
                Unlock();
            }
            else
            {
                lblPassword.Text = "PLEASE ENTER YOUR PASSWORD";
                _mUnlocked = false;
                Lock();
            }
        }
    }
}