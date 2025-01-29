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
            axestxt = new Label();
            SuspendLayout();
            // 
            // FPSDisplay
            // 
            FPSDisplay.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            FPSDisplay.AutoSize = true;
            FPSDisplay.Location = new Point(710, 401);
            FPSDisplay.Name = "FPSDisplay";
            FPSDisplay.Size = new Size(50, 20);
            FPSDisplay.TabIndex = 0;
            FPSDisplay.Text = "label1";
            // 
            // axestxt
            // 
            axestxt.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            axestxt.AutoSize = true;
            axestxt.Location = new Point(375, 215);
            axestxt.Name = "axestxt";
            axestxt.Size = new Size(40, 20);
            axestxt.TabIndex = 1;
            axestxt.Text = "Axes";
            // 
            // Engine
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(axestxt);
            Controls.Add(FPSDisplay);
            DoubleBuffered = true;
            Name = "Engine";
            Text = "Form1";
            Load += EngineInit;
            Paint += Engine_Paint;
            KeyUp += Engine_KeyUp;
            PreviewKeyDown += Engine_PreviewKeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label FPSDisplay;
        private Label axestxt;
    }
}
