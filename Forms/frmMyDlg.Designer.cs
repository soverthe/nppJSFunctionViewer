namespace Kbg.NppPluginNET
{
    public partial class frmMyDlg
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.GoToFunction = new System.Windows.Forms.Button();
            this.GoUp = new System.Windows.Forms.Button();
            this.GoDown = new System.Windows.Forms.Button();
            this.GoToX = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.AcceptsTab = true;
            this.richTextBox1.BackColor = System.Drawing.Color.Black;
            this.richTextBox1.CausesValidation = false;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.Window;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(445, 278);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // GoToFunction
            // 
            this.GoToFunction.Location = new System.Drawing.Point(67, 12);
            this.GoToFunction.Name = "GoToFunction";
            this.GoToFunction.Size = new System.Drawing.Size(92, 32);
            this.GoToFunction.TabIndex = 2;
            this.GoToFunction.Text = "GoTo Function";
            this.GoToFunction.UseVisualStyleBackColor = true;
            this.GoToFunction.Click += new System.EventHandler(this.GoToFunctionClick);
            // 
            // GoUp
            // 
            this.GoUp.Location = new System.Drawing.Point(174, 12);
            this.GoUp.Name = "GoUp";
            this.GoUp.Size = new System.Drawing.Size(40, 32);
            this.GoUp.TabIndex = 3;
            this.GoUp.Text = "/\\";
            this.GoUp.UseVisualStyleBackColor = true;
            this.GoUp.Click += new System.EventHandler(this.GoUpClick);
            // 
            // GoDown
            // 
            this.GoDown.Location = new System.Drawing.Point(220, 12);
            this.GoDown.Name = "GoDown";
            this.GoDown.Size = new System.Drawing.Size(40, 32);
            this.GoDown.TabIndex = 4;
            this.GoDown.Text = "\\/";
            this.GoDown.UseVisualStyleBackColor = true;
            this.GoDown.Click += new System.EventHandler(this.GoDownClick);
            // 
            // GoToX
            // 
            this.GoToX.Location = new System.Drawing.Point(12, 12);
            this.GoToX.Name = "GoToX";
            this.GoToX.Size = new System.Drawing.Size(40, 32);
            this.GoToX.TabIndex = 5;
            this.GoToX.Text = "X";
            this.GoToX.UseVisualStyleBackColor = true;
            this.GoToX.Click += new System.EventHandler(this.GoToXClick);
            // 
            // frmMyDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(445, 278);
            this.Controls.Add(this.GoToX);
            this.Controls.Add(this.GoDown);
            this.Controls.Add(this.GoUp);
            this.Controls.Add(this.GoToFunction);
            this.Controls.Add(this.richTextBox1);
            this.Name = "frmMyDlg";
            this.Text = "frmMyDlg";
            this.Load += new System.EventHandler(this.frmMyDlg_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;

        public void ChangeText(string text)
        {
            richTextBox1.Text = text;
        }

        public System.Windows.Forms.Button GoToFunction;
        public System.Windows.Forms.Button GoUp;
        public System.Windows.Forms.Button GoDown;
        public System.Windows.Forms.Button GoToX;

        public static bool isGoToFunctionClicked = false;
        public static bool isGoUpClicked = false;
        public static bool isGoDownClicked = false;
        public static bool isGoToXClicked = false;
    }
}