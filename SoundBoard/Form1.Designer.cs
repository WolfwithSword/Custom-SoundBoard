using MetroFramework;
using MetroFramework.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SoundBoardTest
{
    partial class SoundBoard
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
            this.playbackDrop = new MetroFramework.Controls.MetroComboBox();
            this.volumeBar = new MetroFramework.Controls.MetroTrackBar();
            this.stopBtn = new MetroFramework.Controls.MetroButton();
            this.playbackLabel = new MetroFramework.Controls.MetroLabel();
            this.volumeLabel = new MetroFramework.Controls.MetroLabel();
            this.volumeDesc = new MetroFramework.Controls.MetroLabel();
            this.addSoundBtn = new MetroFramework.Controls.MetroButton();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.soundPanel = new MetroFramework.Controls.MetroPanel();
            this.metroToolTip1 = new MetroFramework.Components.MetroToolTip();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // playbackDrop
            // 
            this.playbackDrop.FormattingEnabled = true;
            this.playbackDrop.ItemHeight = 23;
            this.playbackDrop.Location = new System.Drawing.Point(130, 7);
            this.playbackDrop.Name = "playbackDrop";
            this.playbackDrop.Size = new System.Drawing.Size(233, 29);
            this.playbackDrop.TabIndex = 11;
            this.playbackDrop.TabStop = false;
            this.playbackDrop.UseSelectable = true;
            this.playbackDrop.DropDown += new System.EventHandler(this.updatePlayback);
            // 
            // volumeBar
            // 
            this.volumeBar.BackColor = System.Drawing.Color.Transparent;
            this.volumeBar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.volumeBar.Location = new System.Drawing.Point(118, 451);
            this.volumeBar.Name = "volumeBar";
            this.volumeBar.Size = new System.Drawing.Size(237, 19);
            this.volumeBar.TabIndex = 14;
            this.volumeBar.TabStop = false;
            this.volumeBar.Text = "metroTrackBar1";
            this.volumeBar.Value = 100;
            this.volumeBar.ValueChanged += new System.EventHandler(this.volumeBar_Scroll);
            this.volumeBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.volumeBar_Scroll);
            this.volumeBar.DoubleClick += new System.EventHandler(this.vol_doubleClick);
            // 
            // stopBtn
            // 
            this.stopBtn.Location = new System.Drawing.Point(66, 13);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(58, 23);
            this.stopBtn.TabIndex = 17;
            this.stopBtn.TabStop = false;
            this.stopBtn.Text = "Stop";
            this.stopBtn.UseSelectable = true;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // playbackLabel
            // 
            this.playbackLabel.AutoSize = true;
            this.playbackLabel.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.playbackLabel.Location = new System.Drawing.Point(202, 35);
            this.playbackLabel.Name = "playbackLabel";
            this.playbackLabel.Size = new System.Drawing.Size(106, 19);
            this.playbackLabel.TabIndex = 20;
            this.playbackLabel.Text = "Playback Device";
            // 
            // volumeLabel
            // 
            this.volumeLabel.AutoSize = true;
            this.volumeLabel.Location = new System.Drawing.Point(56, 451);
            this.volumeLabel.Name = "volumeLabel";
            this.volumeLabel.Size = new System.Drawing.Size(39, 19);
            this.volumeLabel.TabIndex = 22;
            this.volumeLabel.Text = "100%";
            // 
            // volumeDesc
            // 
            this.volumeDesc.AutoSize = true;
            this.volumeDesc.Location = new System.Drawing.Point(3, 451);
            this.volumeDesc.Name = "volumeDesc";
            this.volumeDesc.Size = new System.Drawing.Size(60, 19);
            this.volumeDesc.TabIndex = 22;
            this.volumeDesc.Text = "Volume: ";
            // 
            // addSoundBtn
            // 
            this.addSoundBtn.BackColor = System.Drawing.Color.White;
            this.addSoundBtn.Location = new System.Drawing.Point(3, 13);
            this.addSoundBtn.Name = "addSoundBtn";
            this.addSoundBtn.Size = new System.Drawing.Size(57, 23);
            this.addSoundBtn.TabIndex = 4;
            this.addSoundBtn.TabStop = false;
            this.addSoundBtn.Text = "Add";
            this.addSoundBtn.UseSelectable = true;
            this.addSoundBtn.Click += new System.EventHandler(this.addSound_Click);
            // 
            // metroPanel1
            // 
            this.metroPanel1.Controls.Add(this.stopBtn);
            this.metroPanel1.Controls.Add(this.soundPanel);
            this.metroPanel1.Controls.Add(this.playbackDrop);
            this.metroPanel1.Controls.Add(this.addSoundBtn);
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(3, 50);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(366, 398);
            this.metroPanel1.TabIndex = 24;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // soundPanel
            // 
            this.soundPanel.AutoScroll = true;
            this.soundPanel.HorizontalScrollbar = true;
            this.soundPanel.HorizontalScrollbarBarColor = false;
            this.soundPanel.HorizontalScrollbarHighlightOnWheel = false;
            this.soundPanel.HorizontalScrollbarSize = 10;
            this.soundPanel.Location = new System.Drawing.Point(3, 42);
            this.soundPanel.Name = "soundPanel";
            this.soundPanel.Size = new System.Drawing.Size(360, 353);
            this.soundPanel.TabIndex = 7;
            this.soundPanel.VerticalScrollbar = true;
            this.soundPanel.VerticalScrollbarBarColor = true;
            this.soundPanel.VerticalScrollbarHighlightOnWheel = true;
            this.soundPanel.VerticalScrollbarSize = 10;
            // 
            // metroToolTip1
            // 
            this.metroToolTip1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroToolTip1.StyleManager = null;
            this.metroToolTip1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(3, 9);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(15, 19);
            this.metroLabel1.TabIndex = 25;
            this.metroLabel1.Text = "?";
            this.metroLabel1.Click += new System.EventHandler(this.metroLabel1_Click);
            // 
            // SoundBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 478);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.volumeLabel);
            this.Controls.Add(this.volumeBar);
            this.Controls.Add(this.volumeDesc);
            this.Controls.Add(this.playbackLabel);
            this.Controls.Add(this.metroPanel1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "SoundBoard";
            this.Resizable = false;
            this.Style = MetroFramework.MetroColorStyle.Default;
            this.Text = "SoundBoard";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += this.Form1_FormClosing;
            this.metroPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroComboBox playbackDrop;
        private MetroFramework.Controls.MetroTrackBar volumeBar;
        private MetroFramework.Controls.MetroButton stopBtn;
        private MetroFramework.Controls.MetroLabel playbackLabel;
        private MetroFramework.Controls.MetroLabel volumeLabel;
        private MetroLabel volumeDesc;
        private MetroButton addSoundBtn;
        private MetroPanel metroPanel1;
        private MetroPanel soundPanel;
        private MetroFramework.Components.MetroToolTip metroToolTip1;
        private MetroLabel metroLabel1;
    }
}

