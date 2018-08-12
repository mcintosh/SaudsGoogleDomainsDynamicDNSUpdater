﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;

namespace SaudsGoogleDomainsDynamicDNSUpdater
{
    public partial class Form1 : Form
    {
        private static readonly Timer Timer = new Timer();
        public Form1()
        {
            InitializeComponent();

            username.Text = Properties.Settings.Default.usernamesave;
            password.Text = Properties.Settings.Default.passwordsave;
            subdomain.Text = Properties.Settings.Default.subdomainsave;
            interval.Text = Properties.Settings.Default.intervalsave;

            MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.SizeGripStyle = SizeGripStyle.Hide;
            // When window state changed, trigger state update.
            this.Resize += SetMinimizeState;

            // When tray icon clicked, trigger window state change.       
            notifyIcon1.Click += ToggleMinimizeState;

            CallDDNS();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (username.Text == "")
            {
                MessageBox.Show("Please enter the username");
                username.Focus();
                return;
            }
            if (password.Text == "")
            {
                MessageBox.Show("Please enter the password");
                password.Focus();
                return;
            }
            if (subdomain.Text == "")
            {
                MessageBox.Show("Please enter the subdomain");
                subdomain.Focus();
                return;
            }
            if (interval.Text == "")
            {
                MessageBox.Show("Please enter the interval in minutes");
                interval.Focus();
                return;
            }
            int val = 0;
            bool res = Int32.TryParse(interval.Text, out val);
            if (res == true && val > 0 && val < 35792)
            {
                // add record
            }
            else
            {
                MessageBox.Show("Please input 1 to 35791 only.");
                interval.Focus();
                return;
            }
            Timer.Stop();
            CallDDNSOnce();
            CallDDNS();
            

            Properties.Settings.Default.usernamesave = username.Text;
            Properties.Settings.Default.passwordsave = password.Text;
            Properties.Settings.Default.subdomainsave = subdomain.Text;
            Properties.Settings.Default.intervalsave = interval.Text;
            Properties.Settings.Default.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            username.Text = Properties.Settings.Default.usernamesave;
            password.Text = Properties.Settings.Default.passwordsave;
            subdomain.Text = Properties.Settings.Default.subdomainsave;
            interval.Text = Properties.Settings.Default.intervalsave;

            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("http://www.saudiqbal.com/goto.php?link=21");
            Process.Start(sInfo);
        }

        // Toggle state between Normal and Minimized.
        private void ToggleMinimizeState(object sender, EventArgs e)
        {
            bool isMinimized = this.WindowState == FormWindowState.Minimized;
            this.WindowState = (isMinimized) ? FormWindowState.Normal : FormWindowState.Minimized;
        }

        // Show/Hide window and tray icon to match window state.
        private void SetMinimizeState(object sender, EventArgs e)
        {
            bool isMinimized = this.WindowState == FormWindowState.Minimized;

            this.ShowInTaskbar = !isMinimized;
            notifyIcon1.Visible = isMinimized;
            //if (isMinimized) notifyIcon1.ShowBalloonTip(100, "Saud's Google Domains Dynamic DNS Updater", "Application minimized to tray.", ToolTipIcon.Info);
        }

        private void CallDDNSOnce()
        {
            // Timer
            try
            {
                var client = new WebClient { Credentials = new NetworkCredential(username.Text, password.Text) };
                var response = client.DownloadString("https://domains.google.com/nic/update?hostname=" + subdomain.Text);
                //MessageBox.Show(response);
                toolStripStatusLabel2.Text = "Status: " + response + DateTime.Now.ToString(" - yyyy-MM-dd h:mm:ss tt");
                //notifyIcon1.Text = "Status: " + response + DateTime.Now.ToString(" - yyyy-MM-dd h:mm tt");
                //responseddns.Content = response;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ReceiveFailure || ex.Status == WebExceptionStatus.ConnectFailure || ex.Status == WebExceptionStatus.KeepAliveFailure)
                {
                    toolStripStatusLabel2.Text = "Status: Connection Failed" + DateTime.Now.ToString(" - yyyy-MM-dd h:mm:ss tt");
                    //notifyIcon1.Text = "Status: Connection Failed" + DateTime.Now.ToString(" - yyyy-MM-dd h:mm tt");
                }
            }
            int intervaldigit = Convert.ToInt32(interval.Text);
            Timer.Tick += TimerEventProcessor;
            Timer.Interval = (intervaldigit * 60000);
            Timer.Start();
        }

        private void CallDDNS()
        {
            // Timer
            int intervaldigit = Convert.ToInt32(interval.Text);
            Timer.Tick += TimerEventProcessor;
            Timer.Interval = (intervaldigit * 60000);
            Timer.Start();
        }

        private void TimerEventProcessor(object myObject, EventArgs myEventArgs)
        {
            //Timer.Stop();
            //CallDDNS();
            try
            {
                var client = new WebClient { Credentials = new NetworkCredential(username.Text, password.Text) };
                var response = client.DownloadString("https://domains.google.com/nic/update?hostname=" + subdomain.Text);
                //MessageBox.Show(response);
                toolStripStatusLabel2.Text = "Status: " + response + DateTime.Now.ToString(" - yyyy-MM-dd h:mm:ss tt");
                //notifyIcon1.Text = "Status: " + response + DateTime.Now.ToString(" - yyyy-MM-dd h:mm tt");
                //responseddns.Content = response;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ReceiveFailure || ex.Status == WebExceptionStatus.ConnectFailure || ex.Status == WebExceptionStatus.KeepAliveFailure)
                {
                    toolStripStatusLabel2.Text = "Status: Connection Failed" + DateTime.Now.ToString(" - yyyy-MM-dd h:mm:ss tt");
                    //notifyIcon1.Text = "Status: Connection Failed" + DateTime.Now.ToString(" - yyyy-MM-dd h:mm tt");
                }
            }

            //Timer.Start();
        }
    }
}
