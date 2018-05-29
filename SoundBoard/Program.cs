using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SoundBoardTest
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run((Form)new SoundBoard());
        }
    }
    public class HotKeyGesture
    {
        public List<Keys> keyCombo;
        public HotKeyGesture(IEnumerable<Keys> keys, Keys modifiers)
        {
            keyCombo = new List<Keys>(keys);

            if (keyCombo.Count == 0)
            {
                throw new ArgumentException("Specify minimum one key", "keys");
            }
        }

        private int currentindex;
        public bool Matches(KeyEventArgs e)
        {
            if (e.Modifiers != Keys.Control) return false; ;
            if (keyCombo[currentindex] == e.KeyCode)
                //partial match
                currentindex++;
            else
                //No keys match
                currentindex = 0;
            if (currentindex + 1 > keyCombo.Count)
            {
                //Last key match
                currentindex = 0;
                return true;
            }
            return false;
        }
    }
    public class SoundButton : MetroFramework.Controls.MetroButton
    {
        public string name, soundPath, imagePath;
        private List<String> acceptedImageExtensions = new List<String>() {".jpg",".jpeg",".png"}; 
        public SoundButton(string csvLine)
        {
            string[] split = csvLine.Split(',');
            this.name = split[0];
            this.Text = this.name;
            this.soundPath = split[1];
            this.imagePath = split.Length > 2 ? split[2] : "";
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.setImage(this.imagePath);
            
        }
        public void setImage(string path)
        {
            if(String.IsNullOrEmpty(path) || !File.Exists(path) 
                || !acceptedImageExtensions.Contains(Path.GetExtension(path).ToLower()))
            {
                this.BackgroundImage = null;
                return;
            }
            this.BackgroundImage = Image.FromFile(path);
        }
    }
}

