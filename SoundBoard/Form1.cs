using MetroFramework;
using MetroFramework.Controls;
using MetroFramework.Forms;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;

namespace SoundBoardTest
{
    public partial class SoundBoard : MetroFramework.Forms.MetroForm
    {
        // Data file for soundboard buttons
        // Format: CSV like.
        // [name],[audio_source],[optional_image_source]
        string DATA_FILE = "soundboard.dat";

        //Format:
        // [button index],[HotKey key 0-9]
        string GLOBAL_HOTKEY_FILE = "globalhotkeys.dat";

        WaveOut waveOut;
        WaveStream waveStream;
        SoundButton selected_button;
        
        List<SoundButton> sbButtons = new List<SoundButton>();

        private List<HotKeyGesture> hotkeys = new List<HotKeyGesture>();

        private HotKeyGesture stopSound = new HotKeyGesture(new List<Keys>() { Keys.E }, Keys.Control); // End

        private GlobalHotKey stopSoundGlobal;

        private ContextMenu menu = new ContextMenu();

        private List<GlobalHotKey> globalHotKeys = new List<GlobalHotKey>();
        private Dictionary<Keys, int> globalButtonRltn = new Dictionary<Keys, int>();
        public SoundBoard()
        {
            if (!File.Exists(DATA_FILE))
            {
                File.Create(DATA_FILE).Close();
                File.SetAttributes(DATA_FILE, FileAttributes.Normal);
            }
            if(!File.Exists(GLOBAL_HOTKEY_FILE))
            {
                using (StreamWriter writer = new StreamWriter(GLOBAL_HOTKEY_FILE))
                {
                    for(int i=0; i<10; i++)
                    {
                        writer.WriteLine("-1," + i);
                    }
                }
                    File.SetAttributes(GLOBAL_HOTKEY_FILE, FileAttributes.Normal);
            }
            this.InitializeComponent();
            this.generateButtons();
            this.updateSoundGrid();
            soundPanel.Focus();
            this.generateGlobalKeys();
            stopSoundGlobal = new GlobalHotKey(GlobalHotKey.CTRL, Keys.E, this);
            stopSoundGlobal.Register();
        }

        private void hotkeyPressed(Keys key)
        {
            if (!globalButtonRltn.ContainsKey(key)) return;
            int button_index = globalButtonRltn[key];
            if(button_index < sbButtons.Count && button_index >=0) playSound(sbButtons[button_index].soundPath);  
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == GlobalHotKey.WM_HOTKEY_MSG_ID)
                hotkeyPressed( (Keys) ((m.LParam.ToInt32())>>16) );
            base.WndProc(ref m);
        }

        private void generateGlobalKeys()
        {
            using (StreamReader file = new StreamReader(GLOBAL_HOTKEY_FILE))
            {
                string line;
                Keys k;
                string[] split;
                GlobalHotKey ghk;
                while((line=file.ReadLine()) != null)
                {
                    split = line.Split(',');
                    k = (Keys)Enum.Parse(typeof(Keys), "D" + split[1]);
                    ghk = new GlobalHotKey(GlobalHotKey.CTRL, k, this);
                    ghk.Register();
                    globalHotKeys.Add(ghk);
                    globalButtonRltn[k] = int.Parse(split[0]);
                }
            }
        }
        private void updateGlobalKeyFile()
        {
            using (StreamWriter file = new StreamWriter(GLOBAL_HOTKEY_FILE, false))
            {
                foreach(GlobalHotKey ghk in globalHotKeys)
                {
                    file.WriteLine(globalButtonRltn[ghk.key] + "," + ghk.key.ToString().Replace("D", ""));
                }
            }
        }
        private void changeGlobalHotKey(Keys key, int button_index)
        {
            globalButtonRltn[key] = button_index;
            updateGlobalKeyFile();
        }

        // Create buttons from text file. 
        private void generateButtons()
        {
            using (StreamReader file = new StreamReader(DATA_FILE))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    createSoundButton(line);
                }
            }
        }

        // Update the buttons in the grid.
        private void updateSoundGrid()
        {
            updateDataFile();
            soundPanel.Controls.Clear();
            hotkeys.Clear();
            int count = 0;
            SoundButton temp;
            for (int r = 0; count < sbButtons.Count; r++)
            {
                for (int c = 0; c < 4 && count < sbButtons.Count; c++)
                {
                    temp = sbButtons[count];
                    temp.SetBounds(92 * c, r * 90, 80, 80);
                    temp.Visible = true;
                    soundPanel.Controls.Add(temp);
                    List<Keys> keyindex = new List<Keys>();
                    string number = (count + 1).ToString();

                    foreach (char s in number.ToCharArray())
                    {
                        keyindex.Add((Keys)Enum.Parse(typeof(Keys), "NumPad" + s));
                    }
                    hotkeys.Add(new HotKeyGesture(keyindex, Keys.Control));

                    int id = hotkeys.Count;

                    count++;
                }
            }
        }

        // Add a new sound to the soundboard.
        private void addSound_Click(object sender, EventArgs e)
        {
            string name, path;
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Audio files(*.aiff;*.mp3;*.wav;*.ogg)|*.aiff;*.mp3;*.wav;*.ogg";
            if (file.ShowDialog() == DialogResult.OK)
            {
                path = file.FileName;
            }
            else
            {
                return;
            }
            file.Dispose();
            name = Microsoft.VisualBasic.Interaction.InputBox("Enter name for sound",
                "Name", Path.GetFileNameWithoutExtension(path)).Replace(" ", "_").Replace(",", "");
            if (String.IsNullOrEmpty(name)) name = "Sound-" + sbButtons.Count +
                    "-" + String.Format("#{0:X8}", new Random().Next(0x1000000));
            createSoundButton(name + "," + path);
            updateSoundGrid();
        }

        // Edit the name of a sound.
        private void editSound_Click(object sender, EventArgs e)
        {
            selected_button = ((sender as MenuItem).Parent as ContextMenu).SourceControl as SoundButton;
            if (selected_button == null) return;

            string name;
            name = Microsoft.VisualBasic.Interaction.InputBox("Enter name for sound",
                "Name", selected_button.name).Replace(" ", "_").Replace(",", "");
            if (String.IsNullOrEmpty(name)) name = ""; // Allow manual change to empty
            selected_button.name = name;
            selected_button.Text = name;
            updateDataFile();
        }

        // Edit the source of a sound.
        private void editSoundPath_Click(object sender, EventArgs e)
        {
            selected_button = ((sender as MenuItem).Parent as ContextMenu).SourceControl as SoundButton;
            if (selected_button == null) return;
            string path;
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Audio files(*.aiff;*.mp3;*.wav;*.ogg)|*.aiff;*.mp3;*.wav;*.ogg";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                path = fileDialog.FileName;
            }
            else
            {
                path = selected_button.soundPath;
            }
            fileDialog.Dispose();
            selected_button.soundPath = path;
            updateDataFile();
        }

        // Remove a sound from the soundboard.
        private void removeSound_Click(object sender, EventArgs e)
        {
            selected_button = ((sender as MenuItem).Parent as ContextMenu).SourceControl as SoundButton;
            if (selected_button == null) return;
            sbButtons.Remove(selected_button);
            soundPanel.Controls.Remove(selected_button);
            selected_button.Dispose();
            updateSoundGrid();
        }

        // Creates the button for a sound.
        private void createSoundButton(String line)
        {
            SoundButton button = new SoundButton(line);
            button.ContextMenu = menu;
            if (!File.Exists(button.soundPath)) return;
            button.Click += sbButton_Click;

            button.MouseEnter += sbButton_Enter;
            button.MouseLeave += sbButton_Leave;
            button.AutoSize = false;
            button.TabStop = false;
            sbButtons.Add(button);
            List<Keys> keyindex = new List<Keys>();
            string number = sbButtons.Count.ToString();
            if (!number.Equals("0"))
            {
                foreach (char s in number.ToCharArray())
                {
                    keyindex.Add((Keys)Enum.Parse(typeof(Keys), "NumPad" + s));
                }
            }
            hotkeys.Add(new HotKeyGesture(keyindex, Keys.Control));
            
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (stopSound.Matches(e))
            {
                WaveOut_PlaybackStopped(null, null);
                return;
            }
            else
            {
                foreach (HotKeyGesture hk in hotkeys)
                {
                    if (hk.Matches(e))
                    {
                        string index = "";
                        foreach (Keys k in hk.keyCombo)
                        {
                            // Number keys are enumerated like D0, D1, D2 ... D9
                            index += k.ToString().Replace("NumPad", "");
                        }
                        int i = int.Parse(index) - 1;
                        string path = sbButtons[i].soundPath;
                        if (!String.IsNullOrEmpty(path)) playSound(path);
                    }
                }
            }
        }

        private void sbButton_Enter(object sender, EventArgs e)
        {
            SoundButton button = sender as SoundButton;
            metroToolTip1.Show(button.Text, button);
        }
        private void sbButton_Leave(object sender, EventArgs e)
        {
            metroToolTip1.Hide(sender as SoundButton);
        }

        // Play a sound and select it to edit/remove.
        private void sbButton_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs) e;
            SoundButton button = sender as SoundButton;
            selected_button = button;
            if (me.Button == MouseButtons.Left)
            {
                playSound(button.soundPath);
                foreach (SoundButton b in sbButtons)
                {
                    b.Highlight = false;
                }
                button.Highlight = true;
            }
        }

        // Fill in the available output sources.
        private void fillPlayback()
        {
            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                WaveOutCapabilities WOC = WaveOut.GetCapabilities(i);
                playbackDrop.Items.Insert(i, WOC.ProductName);
            }
        }

        // Update the playback devices dropdown on open event.
        private void updatePlayback(object sender, EventArgs e)
        {
            var current_playback = playbackDrop.SelectedIndex;
            playbackDrop.Items.Clear();
            fillPlayback();
            if (playbackDrop.Items.Contains(current_playback))
                playbackDrop.SelectedIndex = playbackDrop.Items.IndexOf(current_playback);
            else playbackDrop.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fillPlayback();
            playbackDrop.SelectedIndex = 0;
            MenuItem setGlobalHK = new MenuItem("Set Global HotKey");
            setGlobalHK.MenuItems.Add("D1", setGHK);
            setGlobalHK.MenuItems.Add("D2", setGHK);
            setGlobalHK.MenuItems.Add("D3", setGHK);
            setGlobalHK.MenuItems.Add("D4", setGHK);
            setGlobalHK.MenuItems.Add("D5", setGHK);
            setGlobalHK.MenuItems.Add("D6", setGHK);
            setGlobalHK.MenuItems.Add("D7", setGHK);
            setGlobalHK.MenuItems.Add("D8", setGHK);
            setGlobalHK.MenuItems.Add("D9", setGHK);
            setGlobalHK.MenuItems.Add("D0", setGHK);
            menu.MenuItems.Add(setGlobalHK);
            menu.MenuItems.Add("Edit Name", editSound_Click);
            menu.MenuItems.Add("Edit Sound", editSoundPath_Click);
            menu.MenuItems.Add("Edit Image", editImgBtn_Click);
            menu.MenuItems.Add("Remove Image", removeImg_Click);
            menu.MenuItems.Add("Remove", removeSound_Click);
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach(GlobalHotKey k in globalHotKeys)
            {
                k.Unregister();
            }
            stopSoundGlobal.Unregister();
        }
        private void setGHK(object sender, EventArgs e)
        {
            Keys key = (Keys)Enum.Parse(typeof(Keys), (sender as MenuItem).Text);
            selected_button = (((sender as MenuItem).Parent as MenuItem).Parent as ContextMenu).SourceControl as SoundButton;
            globalButtonRltn[key] = sbButtons.IndexOf(selected_button);
            updateGlobalKeyFile();
        }

        // When soundboard sound stops. Cleans up memory.
        private void WaveOut_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (waveStream != null)
            {
                waveStream.Close();
                waveStream.Dispose();
            }
            if (waveOut != null) waveOut.Dispose();
            GC.Collect();
        }

        // Play a sound from a given path. Accepts mp3, wav, ogg, aiff
        private void playSound(String path)
        {
            WaveOut_PlaybackStopped(null, null);
            if (!File.Exists(path)) return;
            String extension = Path.GetExtension(path);
            try
            {
                switch (extension.ToLower())
                {
                    case ".mp3":
                        waveStream = new Mp3FileReader(path);
                        break;
                    case ".wav":
                        waveStream = new WaveFileReader(path);
                        break;
                    case ".ogg":
                        waveStream = new NAudio.Vorbis.VorbisWaveReader(path);
                        break;
                    case ".aiff":
                        waveStream = new AiffFileReader(path);
                        break;
                    default:
                        return;
                }
            }catch (System.IO.InvalidDataException ex)
            {
                return;
            }

            waveOut = new WaveOut();
            waveOut.PlaybackStopped += WaveOut_PlaybackStopped;
            waveOut.DeviceNumber = playbackDrop.SelectedIndex;
            waveOut.Init(waveStream);
            waveOut.Volume =(float) volumeBar.Value / 100;
            waveOut.Play();
        }
        private void stopBtn_Click(object sender, EventArgs e)
        {
            if (waveOut == null) return;
            waveOut.Stop();
        }

        private void vol_doubleClick(object sender, EventArgs e)
        {
            this.volumeBar.Value = 100;
            (this.volumeLabel).Text = this.volumeBar.Value.ToString() + "%";
        }
        
        private void updateDataFile()
        {
            using (StreamWriter writer = new StreamWriter(DATA_FILE, false))
            {
                string line;
                foreach(SoundButton button in sbButtons)
                {
                    if (!File.Exists(button.soundPath)) continue;
                    line = button.name + "," + button.soundPath;
                    if (!String.IsNullOrEmpty(button.imagePath) && File.Exists(button.imagePath))line += "," + button.imagePath;
                    writer.WriteLine(line);
                }
            }
        }


        private void editImgBtn_Click(object sender, EventArgs e)
        {
            selected_button = ((sender as MenuItem).Parent as ContextMenu).SourceControl as SoundButton;
            if (selected_button == null) return;
            string imagePath;
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image files(*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"; // Just common types ¯\_(ツ)_/¯ 
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                imagePath = fileDialog.FileName;
            }
            else
            {
                imagePath = null; // I.E Click cancel to remove image
            }
            fileDialog.Dispose();
            selected_button.imagePath = imagePath;
            selected_button.setImage(imagePath);
            updateDataFile();
        }
        private void removeImg_Click(object sender, EventArgs e)
        {
            selected_button = ((sender as MenuItem).Parent as ContextMenu).SourceControl as SoundButton;
            if (selected_button == null) return;
            selected_button.imagePath = null;
            selected_button.setImage(null);
            updateDataFile();
        }

        private void volumeBar_Scroll(object sender, EventArgs e)
        {
            volumeLabel.Text = volumeBar.Value.ToString() + "%";
            soundPanel.Focus();
        }

        private void metroLabel1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("SoundBoard" + Environment.NewLine +
                            Environment.NewLine + "WolfwithSword | https://github.com/WolfwithSword" +
                            Environment.NewLine + Environment.NewLine+ "HotKeys:" +
                            Environment.NewLine + "· Ctrl + E : Stop current playing sound" +
                            Environment.NewLine + "· Ctrl + ### : Play the ### sound (or closest matching #)",
                            "Info",
                            MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }
}
