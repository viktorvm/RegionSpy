namespace RegionSpy
{
    partial class fMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbHex = new System.Windows.Forms.TextBox();
            this.tbDec = new System.Windows.Forms.TextBox();
            this.tbToSend = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tbRegion = new System.Windows.Forms.TextBox();
            this.tbTerminal = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbHex);
            this.panel1.Controls.Add(this.tbDec);
            this.panel1.Controls.Add(this.tbToSend);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btnStop);
            this.panel1.Controls.Add(this.btnGo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(829, 56);
            this.panel1.TabIndex = 0;
            // 
            // tbHex
            // 
            this.tbHex.Location = new System.Drawing.Point(636, 31);
            this.tbHex.Name = "tbHex";
            this.tbHex.Size = new System.Drawing.Size(181, 20);
            this.tbHex.TabIndex = 7;
            this.tbHex.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbHex_KeyPress);
            // 
            // tbDec
            // 
            this.tbDec.Location = new System.Drawing.Point(511, 31);
            this.tbDec.Name = "tbDec";
            this.tbDec.Size = new System.Drawing.Size(100, 20);
            this.tbDec.TabIndex = 6;
            this.tbDec.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbDec_KeyPress);
            // 
            // tbToSend
            // 
            this.tbToSend.Location = new System.Drawing.Point(8, 7);
            this.tbToSend.Name = "tbToSend";
            this.tbToSend.Size = new System.Drawing.Size(728, 20);
            this.tbToSend.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(742, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(221, 29);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(140, 29);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 3;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(327, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Ответ терминала:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Запрос с Region2000:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tbRegion);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 56);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(320, 290);
            this.panel2.TabIndex = 1;
            // 
            // tbRegion
            // 
            this.tbRegion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbRegion.Location = new System.Drawing.Point(0, 0);
            this.tbRegion.Multiline = true;
            this.tbRegion.Name = "tbRegion";
            this.tbRegion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbRegion.Size = new System.Drawing.Size(320, 290);
            this.tbRegion.TabIndex = 0;
            this.tbRegion.TextChanged += new System.EventHandler(this.tbRegion_TextChanged);
            // 
            // tbTerminal
            // 
            this.tbTerminal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbTerminal.Location = new System.Drawing.Point(320, 56);
            this.tbTerminal.Multiline = true;
            this.tbTerminal.Name = "tbTerminal";
            this.tbTerminal.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbTerminal.Size = new System.Drawing.Size(509, 290);
            this.tbTerminal.TabIndex = 2;
            this.tbTerminal.TextChanged += new System.EventHandler(this.tbTerminal_TextChanged);
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 346);
            this.Controls.Add(this.tbTerminal);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "fMain";
            this.Text = "RegionSpy 1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fMain_FormClosing);
            this.Load += new System.EventHandler(this.fMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tbRegion;
        private System.Windows.Forms.TextBox tbTerminal;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbToSend;
        private System.Windows.Forms.TextBox tbHex;
        private System.Windows.Forms.TextBox tbDec;
    }
}

