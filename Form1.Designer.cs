namespace ParentLock
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblPassword = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnSetPassword = new System.Windows.Forms.Button();
            this.btnAddTime = new System.Windows.Forms.Button();
            this.btnLock = new System.Windows.Forms.Button();
            this.cboTime = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Impact", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.Location = new System.Drawing.Point(121, 36);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(1015, 98);
            this.lblPassword.TabIndex = 0;
            this.lblPassword.Text = "PLEASE ENTER YOUR PASSWORD";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(1334, 181);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(105, 28);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "EXIT";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Font = new System.Drawing.Font("Impact", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(148, 183);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(519, 154);
            this.txtPassword.TabIndex = 0;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // btnSetPassword
            // 
            this.btnSetPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetPassword.Location = new System.Drawing.Point(679, 181);
            this.btnSetPassword.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetPassword.Name = "btnSetPassword";
            this.btnSetPassword.Size = new System.Drawing.Size(180, 28);
            this.btnSetPassword.TabIndex = 1;
            this.btnSetPassword.Text = "SET PASSWORD";
            this.btnSetPassword.UseVisualStyleBackColor = true;
            this.btnSetPassword.Click += new System.EventHandler(this.btnSetPassword_Click);
            // 
            // btnAddTime
            // 
            this.btnAddTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddTime.Location = new System.Drawing.Point(1051, 181);
            this.btnAddTime.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddTime.Name = "btnAddTime";
            this.btnAddTime.Size = new System.Drawing.Size(105, 28);
            this.btnAddTime.TabIndex = 1;
            this.btnAddTime.Text = "Add Time";
            this.btnAddTime.UseVisualStyleBackColor = true;
            this.btnAddTime.Click += new System.EventHandler(this.btnAddTime_Click);
            // 
            // btnLock
            // 
            this.btnLock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLock.Location = new System.Drawing.Point(1164, 181);
            this.btnLock.Margin = new System.Windows.Forms.Padding(4);
            this.btnLock.Name = "btnLock";
            this.btnLock.Size = new System.Drawing.Size(105, 28);
            this.btnLock.TabIndex = 1;
            this.btnLock.Text = "LOCK";
            this.btnLock.UseVisualStyleBackColor = true;
            this.btnLock.Click += new System.EventHandler(this.btnLock_Click);
            // 
            // cboTime
            // 
            this.cboTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cboTime.FormattingEnabled = true;
            this.cboTime.Location = new System.Drawing.Point(923, 181);
            this.cboTime.Name = "cboTime";
            this.cboTime.Size = new System.Drawing.Size(121, 24);
            this.cboTime.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1477, 354);
            this.ControlBox = false;
            this.Controls.Add(this.cboTime);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnSetPassword);
            this.Controls.Add(this.btnLock);
            this.Controls.Add(this.btnAddTime);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lblPassword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Parent Lock";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnSetPassword;
        private System.Windows.Forms.Button btnAddTime;
        private System.Windows.Forms.Button btnLock;
        private System.Windows.Forms.ComboBox cboTime;
    }
}

