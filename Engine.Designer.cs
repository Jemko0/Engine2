namespace Engine2
{
    partial class Engine
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            FPSDisplay = new Label();
            Captures = new Label();
            SuspendLayout();
            // 
            // FPSDisplay
            // 
            FPSDisplay.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            FPSDisplay.AutoSize = true;
            FPSDisplay.Location = new Point(1812, 984);
            FPSDisplay.Name = "FPSDisplay";
            FPSDisplay.Size = new Size(50, 20);
            FPSDisplay.TabIndex = 0;
            FPSDisplay.Text = "label1";
            // 
            // Captures
            // 
            Captures.Anchor = AnchorStyles.Left;
            Captures.AutoSize = true;
            Captures.FlatStyle = FlatStyle.Popup;
            Captures.Location = new Point(49, 53);
            Captures.Name = "Captures";
            Captures.Size = new Size(40, 20);
            Captures.TabIndex = 1;
            Captures.Text = "Axes";
            // 
            // Engine
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1902, 1033);
            Controls.Add(Captures);
            Controls.Add(FPSDisplay);
            DoubleBuffered = true;
            Name = "Engine";
            Text = "Engine2";
            Load += EngineInit;
            Paint += Engine_Paint;
            KeyUp += Engine_KeyUp;
            PreviewKeyDown += Engine_PreviewKeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label FPSDisplay;
        private Label Captures;
    }
}
