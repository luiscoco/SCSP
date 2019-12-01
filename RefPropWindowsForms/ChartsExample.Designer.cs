namespace RefPropWindowsForms
{
    partial class ChartsExample
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
            this.chartViewer1 = new ChartDirector.WinChartViewer();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.chartViewer2 = new ChartDirector.WinChartViewer();
            ((System.ComponentModel.ISupportInitialize)(this.chartViewer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartViewer2)).BeginInit();
            this.SuspendLayout();
            // 
            // chartViewer1
            // 
            this.chartViewer1.HotSpotCursor = System.Windows.Forms.Cursors.Hand;
            this.chartViewer1.Location = new System.Drawing.Point(12, 12);
            this.chartViewer1.Name = "chartViewer1";
            this.chartViewer1.Size = new System.Drawing.Size(375, 321);
            this.chartViewer1.TabIndex = 1;
            this.chartViewer1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(133, 339);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 26);
            this.button1.TabIndex = 2;
            this.button1.Text = "Simple_Pie_Example";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(617, 544);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(113, 26);
            this.button2.TabIndex = 3;
            this.button2.Text = "Contour_Example";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // chartViewer2
            // 
            this.chartViewer2.HotSpotCursor = System.Windows.Forms.Cursors.Hand;
            this.chartViewer2.Location = new System.Drawing.Point(403, 12);
            this.chartViewer2.Name = "chartViewer2";
            this.chartViewer2.Size = new System.Drawing.Size(517, 427);
            this.chartViewer2.TabIndex = 4;
            this.chartViewer2.TabStop = false;
            // 
            // ChartsExample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 582);
            this.Controls.Add(this.chartViewer2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chartViewer1);
            this.Name = "ChartsExample";
            this.Text = "ChartsExample";
            ((System.ComponentModel.ISupportInitialize)(this.chartViewer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartViewer2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ChartDirector.WinChartViewer chartViewer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private ChartDirector.WinChartViewer chartViewer2;
    }
}