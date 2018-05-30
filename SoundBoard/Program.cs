using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;

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
        public Keys modifier;
        public HotKeyGesture(IEnumerable<Keys> keys, Keys modifiers)
        {
            keyCombo = new List<Keys>(keys);

            if (keyCombo.Count == 0)
            {
                throw new ArgumentException("Specify minimum one key", "keys");
            }
            modifier = modifiers;
        }

        private int currentindex;
        public bool Matches(KeyEventArgs e)
        {
            if ((e.Modifiers != this.modifier)) return false;
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
        private List<String> acceptedImageExtensions = new List<String>() { ".jpg", ".jpeg", ".png" };
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
            if (String.IsNullOrEmpty(path) || !File.Exists(path)
                || !acceptedImageExtensions.Contains(Path.GetExtension(path).ToLower()))
            {
                this.BackgroundImage = null;
                return;
            }
            this.BackgroundImage = Image.FromFile(path);
        }
    }

    public class GlobalHotKey
    {
        private int modifier;
        public Keys key;
        private IntPtr hWnd;
        private int id;

        public const int NOMOD = 0x0000;
        public const int ALT = 0x0001;
        public const int CTRL = 0x0002;
        public const int SHIFT = 0x0004;
        public const int WIN = 0x0008;
        public const int WM_HOTKEY_MSG_ID = 0x0312;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public GlobalHotKey(int modifier, Keys key, Form form)
        {
            this.modifier = modifier;
            this.key = key;
            hWnd = form.Handle;
            id = GetHashCode();
        }
        public bool Register()
        {
            return RegisterHotKey(hWnd, id, modifier, (int)key);
        }
        public bool Unregister()
        {
            return UnregisterHotKey(hWnd, id);
        }
        public override int GetHashCode()
        {
            return modifier ^ (int)key ^ hWnd.ToInt32();
        }
    }
}

