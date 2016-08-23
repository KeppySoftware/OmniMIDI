using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace KeppySynthWatchdog
{
    class ContextMenus
    {
        KeyboardHook MIDIRes;
        KeyboardHook list1;
        KeyboardHook list2;
        KeyboardHook list3;
        KeyboardHook list4;
        KeyboardHook list5;
        KeyboardHook list6;
        KeyboardHook list7;
        KeyboardHook list8;
        KeyboardHook list9;
        KeyboardHook list10;
        KeyboardHook list11;
        KeyboardHook list12;
        KeyboardHook list13;
        KeyboardHook list14;
        KeyboardHook list15;
        KeyboardHook list16;

        MenuItem itemM;
        MenuItem itemR;
        MenuItem item0;
        MenuItem item1;
        MenuItem item0a;
        MenuItem item1a;
        MenuItem itemline1;
        MenuItem itemline2;
        MenuItem itemline3;
        MenuItem item2;
        MenuItem item3;
        MenuItem item4;
        MenuItem item5;
        MenuItem item6;
        MenuItem item7;
        MenuItem item8;
        MenuItem item9;
        MenuItem item10;
        MenuItem item11;
        MenuItem item12;
        MenuItem item13;
        MenuItem item14;
        MenuItem item15;
        MenuItem item16;
        MenuItem item17;
        MenuItem item18;

        public ContextMenu Create()
        {
            ContextMenu menu = new ContextMenu();
            RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
            RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Watchdog", true);

            if (Convert.ToInt32(Watchdog.GetValue("watchdoghotkeys", 1)) == 1)
                RegisterHotKeys(Settings);

            item0 = new MenuItem();
            item0.Index = 0;
            item0.Text = "Open the configurator";
            item0.DefaultItem = true;
            item0.Click += new EventHandler(OpenConf_Click);

            itemM = new MenuItem();
            itemM.Index = 1;
            itemM.Text = "Open the mixer";
            itemM.Click += new EventHandler(OpenMixer_Click);

            item1 = new MenuItem();
            item1.Index = 2;
            item1.Text = "Open the debug window";
            item1.Click += new EventHandler(OpenDbg_Click);

            itemline1 = new MenuItem();
            itemline1.Index = 3;
            itemline1.Text = "-";

            itemR = new MenuItem();
            itemR.Index = 4;
            itemR.Text = "Send a MIDI reset event to all the channels\tALT+INS";
            itemR.Click += new EventHandler(ResetChannels_Click);

            itemline2 = new MenuItem();
            itemline2.Index = 5;
            itemline2.Text = "-";

            item2 = new MenuItem();
            item2.Index = 6;
            item2.Text = "Hotkeys";
            item2.Click += new EventHandler(ResetChannels_Click);

            item0a = new MenuItem();
            item0a.Index = 1;
            item0a.Text = "Enabled";
            item0a.Click += new EventHandler(EnableHotkeys_Click);

            item1a = new MenuItem();
            item1a.Index = 1;
            item1a.Text = "Disabled";
            item1a.Click += new EventHandler(DisableHotkeys_Click);

            if (Convert.ToInt32(Settings.GetValue("hotkeysenabled", 0)) == 1)
            {
                item0a.Enabled = false;
                item0a.Checked = true;
            }
            else
            {
                item1a.Enabled = false;
                item1a.Checked = true;
            }

            itemline3 = new MenuItem();
            itemline3.Index = 7;
            itemline3.Text = "-";

            item3 = new MenuItem();
            item3.Index = 8;
            item3.Text = "Reload list 1\tALT+1";
            item3.Click += new EventHandler(SoundfontReload1);

            item4 = new MenuItem();
            item4.Index = 9;
            item4.Text = "Reload list 2\tALT+2";
            item4.Click += new EventHandler(SoundfontReload2);

            item5 = new MenuItem();
            item5.Index = 10;
            item5.Text = "Reload list 3\tALT+3";
            item5.Click += new EventHandler(SoundfontReload3);

            item6 = new MenuItem();
            item6.Index = 11;
            item6.Text = "Reload list 4\tALT+4";
            item6.Click += new EventHandler(SoundfontReload4);

            item7 = new MenuItem();
            item7.Index = 12;
            item7.Text = "Reload list 5\tALT+5";
            item7.Click += new EventHandler(SoundfontReload5);

            item8 = new MenuItem();
            item8.Index = 13;
            item8.Text = "Reload list 6\tALT+6";
            item8.Click += new EventHandler(SoundfontReload6);

            item9 = new MenuItem();
            item9.Index = 14;
            item9.Text = "Reload list 7\tALT+7";
            item9.Click += new EventHandler(SoundfontReload7);

            item10 = new MenuItem();
            item10.Index = 15;
            item10.Text = "Reload list 8\tALT+8";
            item10.Click += new EventHandler(SoundfontReload8);

            if (Convert.ToInt32(Settings.GetValue("extra8lists", 0)) == 1)
            {
                item11 = new MenuItem();
                item11.Index = 16;
                item11.Text = "Reload list 9\tALT+9";
                item11.Click += new EventHandler(SoundfontReload9);

                item12 = new MenuItem();
                item12.Index = 17;
                item12.Text = "Reload list 10\tALT+10";
                item12.Click += new EventHandler(SoundfontReload10);

                item13 = new MenuItem();
                item13.Index = 18;
                item13.Text = "Reload list 11\tCTRL+ALT+1";
                item13.Click += new EventHandler(SoundfontReload11);

                item14 = new MenuItem();
                item14.Index = 19;
                item14.Text = "Reload list 12\tCTRL+ALT+2";
                item14.Click += new EventHandler(SoundfontReload12);

                item15 = new MenuItem();
                item15.Index = 20;
                item15.Text = "Reload list 13\tCTRL+ALT+3";
                item15.Click += new EventHandler(SoundfontReload13);

                item16 = new MenuItem();
                item16.Index = 21;
                item16.Text = "Reload list 14\tCTRL+ALT+4";
                item16.Click += new EventHandler(SoundfontReload14);

                item17 = new MenuItem();
                item17.Index = 22;
                item17.Text = "Reload list 15\tCTRL+ALT+5";
                item17.Click += new EventHandler(SoundfontReload15);

                item18 = new MenuItem();
                item18.Index = 23;
                item18.Text = "Reload list 16\tCTRL+ALT+6";
                item18.Click += new EventHandler(SoundfontReload16);

                menu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            itemM,
            item0,
            item1,
            itemline1,
            itemR,
            itemline2,
            item2,
            itemline3,
            item3,
            item4,
            item5,
            item6,
            item7,
            item8,
            item9,
            item10,
            item11,
            item12,
            item13,
            item14,
            item15,
            item16,
            item17,
            item18});
            }
            else
            {
                menu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            itemM,
            item0,
            item1,
            itemline1,
            itemR,
            itemline2,
            item2,
            itemline3,
            item3,
            item4,
            item5,
            item6,
            item7,
            item8,
            item9,
            item10});
            }

            item2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { item0a, item1a });

            Settings.Close();
            Watchdog.Close();
            return menu;
        }

        void RegisterHotKeys(RegistryKey Settings)
        {
            MIDIRes = new KeyboardHook();
            list1 = new KeyboardHook();
            list2 = new KeyboardHook();
            list3 = new KeyboardHook();
            list4 = new KeyboardHook();
            list5 = new KeyboardHook();
            list6 = new KeyboardHook();
            list7 = new KeyboardHook();
            list8 = new KeyboardHook();
            list9 = new KeyboardHook();
            list10 = new KeyboardHook();
            list11 = new KeyboardHook();
            list12 = new KeyboardHook();
            list13 = new KeyboardHook();
            list14 = new KeyboardHook();
            list15 = new KeyboardHook();
            list16 = new KeyboardHook();

            MIDIRes.KeyPressed += new EventHandler<KeyPressedEventArgs>(ResetChannels_Click);
            MIDIRes.RegisterHotKey(ModifierKeys.Alt, Keys.Insert);
            list1.KeyPressed += new EventHandler<KeyPressedEventArgs>(SoundfontReload1);
            list1.RegisterHotKey(ModifierKeys.Alt, Keys.D1);
            list2.KeyPressed += new EventHandler<KeyPressedEventArgs>(SoundfontReload2);
            list2.RegisterHotKey(ModifierKeys.Alt, Keys.D2);
            list3.KeyPressed += new EventHandler<KeyPressedEventArgs>(SoundfontReload3);
            list3.RegisterHotKey(ModifierKeys.Alt, Keys.D3);
            list4.KeyPressed += new EventHandler<KeyPressedEventArgs>(SoundfontReload4);
            list4.RegisterHotKey(ModifierKeys.Alt, Keys.D4);
            list5.KeyPressed += new EventHandler<KeyPressedEventArgs>(SoundfontReload5);
            list5.RegisterHotKey(ModifierKeys.Alt, Keys.D5);
            list6.KeyPressed += new EventHandler<KeyPressedEventArgs>(SoundfontReload6);
            list6.RegisterHotKey(ModifierKeys.Alt, Keys.D6);
            list7.KeyPressed += new EventHandler<KeyPressedEventArgs>(SoundfontReload7);
            list7.RegisterHotKey(ModifierKeys.Alt, Keys.D7);
            list8.KeyPressed += new EventHandler<KeyPressedEventArgs>(SoundfontReload8);
            list8.RegisterHotKey(ModifierKeys.Alt, Keys.D8);
            if (Convert.ToInt32(Settings.GetValue("extra8lists", 0)) == 1)
            {
                list9.KeyPressed += new EventHandler<KeyPressedEventArgs>(SoundfontReload9);
                list9.RegisterHotKey(ModifierKeys.Alt, Keys.D9);
                list10.KeyPressed += new EventHandler<KeyPressedEventArgs>(SoundfontReload10);
                list10.RegisterHotKey(ModifierKeys.Alt, Keys.D0);
                list11.KeyPressed += new EventHandler<KeyPressedEventArgs>(SoundfontReload11);
                list11.RegisterHotKey(ModifierKeys.Control | ModifierKeys.Alt, Keys.D1);
                list12.KeyPressed += new EventHandler<KeyPressedEventArgs>(SoundfontReload12);
                list12.RegisterHotKey(ModifierKeys.Control | ModifierKeys.Alt, Keys.D2);
                list13.KeyPressed += new EventHandler<KeyPressedEventArgs>(SoundfontReload13);
                list13.RegisterHotKey(ModifierKeys.Control | ModifierKeys.Alt, Keys.D3);
                list14.KeyPressed += new EventHandler<KeyPressedEventArgs>(SoundfontReload14);
                list14.RegisterHotKey(ModifierKeys.Control | ModifierKeys.Alt, Keys.D4);
                list15.KeyPressed += new EventHandler<KeyPressedEventArgs>(SoundfontReload15);
                list15.RegisterHotKey(ModifierKeys.Control | ModifierKeys.Alt, Keys.D5);
                list16.KeyPressed += new EventHandler<KeyPressedEventArgs>(SoundfontReload16);
                list16.RegisterHotKey(ModifierKeys.Control | ModifierKeys.Alt, Keys.D6);
            }
        }

        void UnregisterHotKeys()
        {
            MIDIRes.Dispose();
            list1.Dispose();
            list2.Dispose();
            list3.Dispose();
            list4.Dispose();
            list5.Dispose();
            list6.Dispose();
            list7.Dispose();
            list8.Dispose();
            list9.Dispose();
            list10.Dispose();
            list11.Dispose();
            list12.Dispose();
            list13.Dispose();
            list14.Dispose();
            list15.Dispose();
            list16.Dispose();
        }

        void EnableHotkeys_Click(object sender, EventArgs e)
        {
            item0a.Enabled = false;
            item0a.Checked = true;
            item1a.Enabled = true;
            item1a.Checked = false;
            RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
            RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Watchdog", true);
            Watchdog.SetValue("hotkeysenabled", "1", RegistryValueKind.DWord);
            RegisterHotKeys(Settings);
            Settings.Close();
            Watchdog.Close();
        }

        void DisableHotkeys_Click(object sender, EventArgs e)
        {
            item0a.Enabled = true;
            item0a.Checked = false;
            item1a.Enabled = false;
            item1a.Checked = true;
            RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Watchdog", true);
            Watchdog.SetValue("hotkeysenabled", "0", RegistryValueKind.DWord);
            UnregisterHotKeys();
            Watchdog.Close();
        }

        void OpenConf_Click(object sender, EventArgs e)
        {
            string currentpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Process.Start(currentpath + "\\KeppySynthConfigurator.exe", null);
        }

        void OpenMixer_Click(object sender, EventArgs e)
        {
            string currentpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Process.Start(currentpath + "\\KeppySynthConfigurator.exe", "/MIX");
        }

        void OpenDbg_Click(object sender, EventArgs e)
        {
            string currentpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Process.Start(currentpath + "\\KeppySynthDebugWindow.exe", null);
        }

        void ResetChannels_Click(object sender, EventArgs e)
        {
            RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Watchdog", true);
            Watchdog.SetValue("resetchannels", 1, RegistryValueKind.DWord);
            Watchdog.Close();
        }

        void SoundfontReload1(object sender, EventArgs e)
        {
            LoadSoundfont(1);
        }

        void SoundfontReload2(object sender, EventArgs e)
        {
            LoadSoundfont(2);
        }

        void SoundfontReload3(object sender, EventArgs e)
        {
            LoadSoundfont(3);
        }

        void SoundfontReload4(object sender, EventArgs e)
        {
            LoadSoundfont(4);
        }

        void SoundfontReload5(object sender, EventArgs e)
        {
            LoadSoundfont(5);
        }

        void SoundfontReload6(object sender, EventArgs e)
        {
            LoadSoundfont(6);
        }

        void SoundfontReload7(object sender, EventArgs e)
        {
            LoadSoundfont(7);
        }

        void SoundfontReload8(object sender, EventArgs e)
        {
            LoadSoundfont(8);
        }

        void SoundfontReload9(object sender, EventArgs e)
        {
            LoadSoundfont(9);
        }

        void SoundfontReload10(object sender, EventArgs e)
        {
            LoadSoundfont(10);
        }

        void SoundfontReload11(object sender, EventArgs e)
        {
            LoadSoundfont(11);
        }

        void SoundfontReload12(object sender, EventArgs e)
        {
            LoadSoundfont(12);
        }

        void SoundfontReload13(object sender, EventArgs e)
        {
            LoadSoundfont(13);
        }

        void SoundfontReload14(object sender, EventArgs e)
        {
            LoadSoundfont(14);
        }

        void SoundfontReload15(object sender, EventArgs e)
        {
            LoadSoundfont(15);
        }

        void SoundfontReload16(object sender, EventArgs e)
        {
            LoadSoundfont(16);
        }

        public static void CheckPop(object source, EventArgs e)
        {
            RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Watchdog", true);
            if (Convert.ToInt32(Watchdog.GetValue("closewatchdog")) == 1 | Convert.ToInt32(Watchdog.GetValue("wdrun")) == 0)
            {
                Watchdog.SetValue("closewatchdog", "0", RegistryValueKind.DWord);
                Application.Exit();
            }
            Watchdog.Close();
        }

        void LoadSoundfont(int whichone)
        {
            RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Watchdog", true);
            Watchdog.SetValue("rel" + whichone.ToString(), "1", RegistryValueKind.DWord);
            Watchdog.Close();
        }
    }
}

public sealed class KeyboardHook : IDisposable
{
    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    /// <summary>
    /// Represents the window that is used internally to get the messages.
    /// </summary>
    private class Window : NativeWindow, IDisposable
    {
        private static int WM_HOTKEY = 0x0312;

        public Window()
        {
            // create the handle for the window.
            this.CreateHandle(new CreateParams());
        }

        /// <summary>
        /// Overridden to get the notifications.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // check if we got a hot key pressed.
            if (m.Msg == WM_HOTKEY)
            {
                // get the keys.
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                ModifierKeys modifier = (ModifierKeys)((int)m.LParam & 0xFFFF);

                // invoke the event to notify the parent.
                if (KeyPressed != null)
                    KeyPressed(this, new KeyPressedEventArgs(modifier, key));
            }
        }

        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        #region IDisposable Members

        public void Dispose()
        {
            this.DestroyHandle();
        }

        #endregion
    }

    private Window _window = new Window();
    private int _currentId;

    public KeyboardHook()
    {
        // register the event of the inner native window.
        _window.KeyPressed += delegate(object sender, KeyPressedEventArgs args)
        {
            if (KeyPressed != null)
                KeyPressed(this, args);
        };
    }

    /// <summary>
    /// Registers a hot key in the system.
    /// </summary>
    /// <param name="modifier">The modifiers that are associated with the hot key.</param>
    /// <param name="key">The key itself that is associated with the hot key.</param>
    public void RegisterHotKey(ModifierKeys modifier, Keys key)
    {
        // increment the counter.
        _currentId = _currentId + 1;

        // register the hot key.
        if (!RegisterHotKey(_window.Handle, _currentId, (uint)modifier, (uint)key))
            throw new InvalidOperationException("Couldn’t register the hot key.");
    }

    /// <summary>
    /// A hot key has been pressed.
    /// </summary>
    public event EventHandler<KeyPressedEventArgs> KeyPressed;

    #region IDisposable Members

    public void Dispose()
    {
        // unregister all the registered hot keys.
        for (int i = _currentId; i > 0; i--)
        {
            UnregisterHotKey(_window.Handle, i);
        }

        // dispose the inner native window.
        _window.Dispose();
    }

    #endregion
}

/// <summary>
/// Event Args for the event that is fired after the hot key has been pressed.
/// </summary>
public class KeyPressedEventArgs : EventArgs
{
    private ModifierKeys _modifier;
    private Keys _key;

    internal KeyPressedEventArgs(ModifierKeys modifier, Keys key)
    {
        _modifier = modifier;
        _key = key;
    }

    public ModifierKeys Modifier
    {
        get { return _modifier; }
    }

    public Keys Key
    {
        get { return _key; }
    }
}

/// <summary>
/// The enumeration of possible modifiers.
/// </summary>
[Flags]
public enum ModifierKeys : uint
{
    Alt = 1,
    Control = 2,
    Shift = 4,
    Win = 8
}