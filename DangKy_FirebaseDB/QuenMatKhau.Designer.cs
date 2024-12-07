namespace DangKy_FirebaseDB
{
    partial class QuenMatKhau
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuenMatKhau));
            this.tb_email = new System.Windows.Forms.TextBox();
            this.bt_getVeriCode = new System.Windows.Forms.Button();
            this.bt_confirm = new System.Windows.Forms.Button();
            this.tb_veriCode = new System.Windows.Forms.TextBox();
            this.lb_email = new System.Windows.Forms.Label();
            this.lb_verifyCode = new System.Windows.Forms.Label();
            this.lb_signup = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tb_email
            // 
            this.tb_email.BackColor = System.Drawing.Color.Gainsboro;
            this.tb_email.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_email.Font = new System.Drawing.Font("Cascadia Code", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.tb_email.Location = new System.Drawing.Point(32, 83);
            this.tb_email.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tb_email.Name = "tb_email";
            this.tb_email.Size = new System.Drawing.Size(283, 23);
            this.tb_email.TabIndex = 0;
            // 
            // bt_getVeriCode
            // 
            this.bt_getVeriCode.BackColor = System.Drawing.Color.Gainsboro;
            this.bt_getVeriCode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bt_getVeriCode.Font = new System.Drawing.Font("Segoe UI Black", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.bt_getVeriCode.ForeColor = System.Drawing.Color.Black;
            this.bt_getVeriCode.Location = new System.Drawing.Point(249, 131);
            this.bt_getVeriCode.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bt_getVeriCode.Name = "bt_getVeriCode";
            this.bt_getVeriCode.Size = new System.Drawing.Size(65, 22);
            this.bt_getVeriCode.TabIndex = 1;
            this.bt_getVeriCode.Text = "GỬI MÃ";
            this.bt_getVeriCode.UseVisualStyleBackColor = false;
            this.bt_getVeriCode.Click += new System.EventHandler(this.bt_getVeriCode_Click);
            // 
            // bt_confirm
            // 
            this.bt_confirm.BackColor = System.Drawing.Color.Sienna;
            this.bt_confirm.Font = new System.Drawing.Font("Cascadia Code", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.bt_confirm.ForeColor = System.Drawing.Color.AliceBlue;
            this.bt_confirm.Location = new System.Drawing.Point(122, 162);
            this.bt_confirm.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bt_confirm.Name = "bt_confirm";
            this.bt_confirm.Size = new System.Drawing.Size(90, 33);
            this.bt_confirm.TabIndex = 3;
            this.bt_confirm.Text = "XÁC NHẬN";
            this.bt_confirm.UseVisualStyleBackColor = false;
            this.bt_confirm.Click += new System.EventHandler(this.bt_confirm_Click);
            // 
            // tb_veriCode
            // 
            this.tb_veriCode.BackColor = System.Drawing.Color.Gainsboro;
            this.tb_veriCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_veriCode.Font = new System.Drawing.Font("Cascadia Code", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.tb_veriCode.Location = new System.Drawing.Point(31, 131);
            this.tb_veriCode.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tb_veriCode.Name = "tb_veriCode";
            this.tb_veriCode.Size = new System.Drawing.Size(203, 23);
            this.tb_veriCode.TabIndex = 2;
            // 
            // lb_email
            // 
            this.lb_email.AutoSize = true;
            this.lb_email.Font = new System.Drawing.Font("Cascadia Code", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lb_email.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lb_email.Location = new System.Drawing.Point(28, 63);
            this.lb_email.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_email.Name = "lb_email";
            this.lb_email.Size = new System.Drawing.Size(176, 18);
            this.lb_email.TabIndex = 4;
            this.lb_email.Text = "Vui lòng nhập email: ";
            // 
            // lb_verifyCode
            // 
            this.lb_verifyCode.AutoSize = true;
            this.lb_verifyCode.Font = new System.Drawing.Font("Cascadia Code", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lb_verifyCode.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lb_verifyCode.Location = new System.Drawing.Point(28, 112);
            this.lb_verifyCode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_verifyCode.Name = "lb_verifyCode";
            this.lb_verifyCode.Size = new System.Drawing.Size(152, 18);
            this.lb_verifyCode.TabIndex = 5;
            this.lb_verifyCode.Text = "Nhập mã xác nhận: ";
            // 
            // lb_signup
            // 
            this.lb_signup.AutoSize = true;
            this.lb_signup.Font = new System.Drawing.Font("Segoe UI Black", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lb_signup.ForeColor = System.Drawing.Color.Maroon;
            this.lb_signup.Location = new System.Drawing.Point(27, 25);
            this.lb_signup.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_signup.Name = "lb_signup";
            this.lb_signup.Size = new System.Drawing.Size(216, 28);
            this.lb_signup.TabIndex = 27;
            this.lb_signup.Text = "FORGOT PASSWORD";
            // 
            // QuenMatKhau
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkKhaki;
            this.ClientSize = new System.Drawing.Size(345, 209);
            this.Controls.Add(this.lb_signup);
            this.Controls.Add(this.lb_verifyCode);
            this.Controls.Add(this.lb_email);
            this.Controls.Add(this.bt_confirm);
            this.Controls.Add(this.tb_veriCode);
            this.Controls.Add(this.bt_getVeriCode);
            this.Controls.Add(this.tb_email);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "QuenMatKhau";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BOMB MASTER";
            this.Load += new System.EventHandler(this.QuenMatKhau_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_email;
        private System.Windows.Forms.Button bt_getVeriCode;
        private System.Windows.Forms.Button bt_confirm;
        private System.Windows.Forms.TextBox tb_veriCode;
        private System.Windows.Forms.Label lb_email;
        private System.Windows.Forms.Label lb_verifyCode;
        private System.Windows.Forms.Label lb_signup;
    }
}