namespace dentastu
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gein = new System.Windows.Forms.TextBox();
            this.btCalc = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.t_max = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.timeconstant = new System.Windows.Forms.TextBox();
            this.F_1 = new System.Windows.Forms.Label();
            this.F_2 = new System.Windows.Forms.Label();
            this.S_1 = new System.Windows.Forms.Label();
            this.tc1 = new System.Windows.Forms.TextBox();
            this.S_2 = new System.Windows.Forms.Label();
            this.tc2 = new System.Windows.Forms.TextBox();
            this.S_3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "単位パルス",
            "単位ステップ"});
            this.comboBox1.Location = new System.Drawing.Point(12, 62);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(102, 20);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(103, 413);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            this.label1.Visible = false;
            // 
            // gein
            // 
            this.gein.Font = new System.Drawing.Font("MS UI Gothic", 15F);
            this.gein.Location = new System.Drawing.Point(276, 126);
            this.gein.Name = "gein";
            this.gein.Size = new System.Drawing.Size(64, 27);
            this.gein.TabIndex = 3;
            this.gein.Text = "1.0";
            this.gein.TextChanged += new System.EventHandler(this.gein_TextChanged);
            // 
            // btCalc
            // 
            this.btCalc.Location = new System.Drawing.Point(486, 369);
            this.btCalc.Name = "btCalc";
            this.btCalc.Size = new System.Drawing.Size(75, 23);
            this.btCalc.TabIndex = 4;
            this.btCalc.Text = "Show";
            this.btCalc.UseVisualStyleBackColor = true;
            this.btCalc.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(486, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 19);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "1.0";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // t_max
            // 
            this.t_max.AutoSize = true;
            this.t_max.Location = new System.Drawing.Point(446, 15);
            this.t_max.Name = "t_max";
            this.t_max.Size = new System.Drawing.Size(34, 12);
            this.t_max.TabIndex = 6;
            this.t_max.Text = "t_max";
            this.t_max.Click += new System.EventHandler(this.t_max_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "1次遅れ",
            "2次遅れ"});
            this.comboBox2.Location = new System.Drawing.Point(217, 25);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 20);
            this.comboBox2.TabIndex = 7;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // timeconstant
            // 
            this.timeconstant.Font = new System.Drawing.Font("MS UI Gothic", 15F);
            this.timeconstant.Location = new System.Drawing.Point(319, 190);
            this.timeconstant.Name = "timeconstant";
            this.timeconstant.Size = new System.Drawing.Size(30, 27);
            this.timeconstant.TabIndex = 8;
            this.timeconstant.Text = "1.0";
            this.timeconstant.TextChanged += new System.EventHandler(this.timeconstant_TextChanged_1);
            // 
            // F_1
            // 
            this.F_1.AutoSize = true;
            this.F_1.Font = new System.Drawing.Font("MS UI Gothic", 15F);
            this.F_1.Location = new System.Drawing.Point(272, 193);
            this.F_1.Name = "F_1";
            this.F_1.Size = new System.Drawing.Size(41, 20);
            this.F_1.TabIndex = 9;
            this.F_1.Text = "1 + ";
            // 
            // F_2
            // 
            this.F_2.AutoSize = true;
            this.F_2.Font = new System.Drawing.Font("MS UI Gothic", 15F);
            this.F_2.Location = new System.Drawing.Point(355, 193);
            this.F_2.Name = "F_2";
            this.F_2.Size = new System.Drawing.Size(18, 20);
            this.F_2.TabIndex = 10;
            this.F_2.Text = "s";
            // 
            // S_1
            // 
            this.S_1.AutoSize = true;
            this.S_1.Font = new System.Drawing.Font("MS UI Gothic", 15F);
            this.S_1.Location = new System.Drawing.Point(217, 193);
            this.S_1.Name = "S_1";
            this.S_1.Size = new System.Drawing.Size(29, 20);
            this.S_1.TabIndex = 11;
            this.S_1.Text = "1+";
            this.S_1.Visible = false;
            this.S_1.Click += new System.EventHandler(this.S_1_Click);
            // 
            // tc1
            // 
            this.tc1.Font = new System.Drawing.Font("MS UI Gothic", 15F);
            this.tc1.Location = new System.Drawing.Point(252, 190);
            this.tc1.Name = "tc1";
            this.tc1.Size = new System.Drawing.Size(34, 27);
            this.tc1.TabIndex = 12;
            this.tc1.Text = "1.0";
            this.tc1.Visible = false;
            // 
            // S_2
            // 
            this.S_2.AutoSize = true;
            this.S_2.Font = new System.Drawing.Font("MS UI Gothic", 15F);
            this.S_2.Location = new System.Drawing.Point(288, 193);
            this.S_2.Name = "S_2";
            this.S_2.Size = new System.Drawing.Size(40, 20);
            this.S_2.TabIndex = 13;
            this.S_2.Tag = "";
            this.S_2.Text = "s + ";
            this.S_2.Visible = false;
            // 
            // tc2
            // 
            this.tc2.Font = new System.Drawing.Font("MS UI Gothic", 15F);
            this.tc2.Location = new System.Drawing.Point(334, 190);
            this.tc2.Name = "tc2";
            this.tc2.Size = new System.Drawing.Size(36, 27);
            this.tc2.TabIndex = 14;
            this.tc2.Text = "1.0";
            this.tc2.Visible = false;
            // 
            // S_3
            // 
            this.S_3.AutoSize = true;
            this.S_3.Font = new System.Drawing.Font("MS UI Gothic", 15F);
            this.S_3.Location = new System.Drawing.Point(376, 193);
            this.S_3.Name = "S_3";
            this.S_3.Size = new System.Drawing.Size(36, 20);
            this.S_3.TabIndex = 15;
            this.S_3.Text = "s^2";
            this.S_3.Visible = false;
            this.S_3.Click += new System.EventHandler(this.F_3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::dentastu.Properties.Resources.FOLE;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.S_3);
            this.Controls.Add(this.tc2);
            this.Controls.Add(this.S_2);
            this.Controls.Add(this.tc1);
            this.Controls.Add(this.S_1);
            this.Controls.Add(this.F_2);
            this.Controls.Add(this.F_1);
            this.Controls.Add(this.timeconstant);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.t_max);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btCalc);
            this.Controls.Add(this.gein);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox gein;
        private System.Windows.Forms.Button btCalc;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label t_max;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.TextBox timeconstant;
        private System.Windows.Forms.Label F_1;
        private System.Windows.Forms.Label F_2;
        private System.Windows.Forms.Label S_1;
        private System.Windows.Forms.TextBox tc1;
        private System.Windows.Forms.Label S_2;
        private System.Windows.Forms.TextBox tc2;
        private System.Windows.Forms.Label S_3;
    }
}

