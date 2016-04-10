using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices; 

namespace AsusXonarSW3
{

    public partial class Form1 : Form
    {
        private const UInt32 BM_CLICK = 0x00F5;
        private const UInt32 CB_GETCURSEL = 0x0147;
        private const UInt32 WM_KEYDOWN = 0x0100;
        private const UInt32 WM_KEYUP = 0x0101;
        private const UInt32 WM_LBUTTONDOWN = 0x0201;
        private const UInt32 WM_LBUTTONUP = 0x0202;

        public Form1()
        {
            InitializeComponent();
            notifyIcon1.Visible = true;
            notifyIcon1.ContextMenu = new ContextMenu(InitializeMenu());
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.Show();
        }
        private MenuItem[] InitializeMenu()
        {
            MenuItem[] menu = new MenuItem[] {
                new MenuItem("Switch", button1_Click),
                new MenuItem("Exit", Exit)
            };
            return menu;
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            HeadOn();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            Check();
        }
        private void Exit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowA(IntPtr ZeroOnly, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetDlgItem(IntPtr hDlg, int nIDDlgItem);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SendMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    
        private void HeadOn()
        {        
            IntPtr hwndMain = FindWindowA(IntPtr.Zero, "Xonar D1 Audio Center");
            IntPtr hwndButtonMain = GetDlgItem(hwndMain, 0x000003F2);
            SendMessage(hwndButtonMain, BM_CLICK, 0, 0);
            IntPtr h_1 = GetDlgItem(hwndMain, 0x00002717);
            IntPtr h_2 = GetDlgItem(h_1, 0x00000960);
            IntPtr hwndComdoBox = GetDlgItem(h_2, 0x00000961);
            int stat = SendMessage(hwndComdoBox, CB_GETCURSEL, 0, 0);
            // 0 - наушники
            // 1 - 2 колонки
            // 2 - 4 колонки
            // 3 - 5.1 колонки
            // 4 - 7.1 колонки
            // 5 - FP наушники
            // 6 - FP 2 колонки
            if (stat == 1) //с 2 колонки на FP наушники
            {
                for(int i=0; i < 4; i++)
                {
                    SendMessage(hwndComdoBox, WM_KEYDOWN, 40, 0);
                    SendMessage(hwndComdoBox, WM_KEYUP, 40, 0);
                }
                SendMessage(h_2, WM_LBUTTONDOWN, 0, ((138 << 16) | 29));
                SendMessage(h_2, WM_LBUTTONUP, 0, ((138 << 16) | 29));
                notifyIcon1.Text = "Наушники";
                notifyIcon1.ShowBalloonTip(100, "Наушники", "включены", ToolTipIcon.None);
            }
            if (stat == 5) //с FP наушники на 2 колонки
            {
                for (int i = 0; i < 4; i++)
                {
                    SendMessage(hwndComdoBox, WM_KEYDOWN, 38, 0);
                    SendMessage(hwndComdoBox, WM_KEYUP, 38, 0);
                }
                notifyIcon1.Text = "Колонки";
                notifyIcon1.ShowBalloonTip(100, "Колонки", "включены", ToolTipIcon.None);
            }
            
        }
        private void Check()
        {
            IntPtr hwndMain = FindWindowA(IntPtr.Zero, "Xonar D1 Audio Center");
            IntPtr hwndButtonMain = GetDlgItem(hwndMain, 0x000003F2);
            SendMessage(hwndButtonMain, BM_CLICK, 0, 0);
            IntPtr h_1 = GetDlgItem(hwndMain, 0x00002717);
            IntPtr h_2 = GetDlgItem(h_1, 0x00000960);
            IntPtr hwndComdoBox = GetDlgItem(h_2, 0x00000961);
            int stat = SendMessage(hwndComdoBox, CB_GETCURSEL, 0, 0);
            // 0 - наушники
            // 1 - 2 колонки
            // 2 - 4 колонки
            // 3 - 5.1 колонки
            // 4 - 7.1 колонки
            // 5 - FP наушники
            // 6 - FP 2 колонки
            if (stat == 1) //с 2 колонки на FP наушники
            {
                notifyIcon1.Text = "Колонки";
            }
            if (stat == 5) //с FP наушники на 2 колонки
            {
                notifyIcon1.Text = "Наушники";   
            }

        }
    }
}
