namespace BlockChainCSharp
{
    partial class FormSignUp
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbTenDN = new System.Windows.Forms.TextBox();
            this.tbXacNhanMK = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMatKhau = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bDangKy = new System.Windows.Forms.Button();
            this.bThoat = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tên Đăng Nhập:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // tbTenDN
            // 
            this.tbTenDN.Location = new System.Drawing.Point(129, 24);
            this.tbTenDN.Name = "tbTenDN";
            this.tbTenDN.Size = new System.Drawing.Size(184, 20);
            this.tbTenDN.TabIndex = 1;
            // 
            // tbXacNhanMK
            // 
            this.tbXacNhanMK.Location = new System.Drawing.Point(129, 76);
            this.tbXacNhanMK.Name = "tbXacNhanMK";
            this.tbXacNhanMK.Size = new System.Drawing.Size(184, 20);
            this.tbXacNhanMK.TabIndex = 3;
            this.tbXacNhanMK.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Xác Nhận Mật Khẩu:";
            // 
            // tbMatKhau
            // 
            this.tbMatKhau.Location = new System.Drawing.Point(129, 50);
            this.tbMatKhau.Name = "tbMatKhau";
            this.tbMatKhau.Size = new System.Drawing.Size(184, 20);
            this.tbMatKhau.TabIndex = 5;
            this.tbMatKhau.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Mật Khẩu:";
            // 
            // bDangKy
            // 
            this.bDangKy.Location = new System.Drawing.Point(153, 102);
            this.bDangKy.Name = "bDangKy";
            this.bDangKy.Size = new System.Drawing.Size(75, 23);
            this.bDangKy.TabIndex = 7;
            this.bDangKy.Text = "Đăng Ký";
            this.bDangKy.UseVisualStyleBackColor = true;
            this.bDangKy.Click += new System.EventHandler(this.bDangKy_Click);
            // 
            // bThoat
            // 
            this.bThoat.Location = new System.Drawing.Point(238, 102);
            this.bThoat.Name = "bThoat";
            this.bThoat.Size = new System.Drawing.Size(75, 23);
            this.bThoat.TabIndex = 8;
            this.bThoat.Text = "Thoát";
            this.bThoat.UseVisualStyleBackColor = true;
            this.bThoat.Click += new System.EventHandler(this.bThoat_Click);
            // 
            // FormSignUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 147);
            this.Controls.Add(this.bThoat);
            this.Controls.Add(this.bDangKy);
            this.Controls.Add(this.tbMatKhau);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbXacNhanMK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbTenDN);
            this.Controls.Add(this.label1);
            this.Name = "FormSignUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng Ký";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTenDN;
        private System.Windows.Forms.TextBox tbXacNhanMK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbMatKhau;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bDangKy;
        private System.Windows.Forms.Button bThoat;
    }
}