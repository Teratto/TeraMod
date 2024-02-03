namespace Tera
{
    partial class DemoAddinPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Dock = DockStyle.Top;
            button1.Location = new Point(0, 0);
            button1.Name = "button1";
            button1.Size = new Size(751, 47);
            button1.TabIndex = 3;
            button1.Text = "Dump Color Keys";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Dock = DockStyle.Fill;
            textBox1.Location = new Point(0, 47);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.ScrollBars = ScrollBars.Both;
            textBox1.Size = new Size(751, 686);
            textBox1.TabIndex = 4;
            // 
            // DemoAddinPanel
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = System.Drawing.Color.Green;
            Controls.Add(textBox1);
            Controls.Add(button1);
            Margin = new Padding(4, 5, 4, 5);
            Name = "DemoAddinPanel";
            Size = new Size(751, 733);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button1;
        private TextBox textBox1;
    }
}
