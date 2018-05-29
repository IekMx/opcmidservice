using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using opclibrary.Services;
using OpcSocket.Contracts;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

/*
 * ==============================================================
 * @ID       $Id: MainForm.cs 971 2010-09-30 16:09:30Z ww $
 * @created  2008-07-31
 * ==============================================================
 *
 * The official license for this file is shown next.
 * Unofficially, consider this e-postcardware as well:
 * if you find this module useful, let us know via e-mail, along with
 * where in the world you are and (if applicable) your website address.
 */

/* ***** BEGIN LICENSE BLOCK *****
 * Version: MIT License
 *
 * Copyright (c) 2010 Michael Sorens http://www.simple-talk.com/author/michael-sorens/
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 *
 * ***** END LICENSE BLOCK *****
 */

namespace OpcSocket
{

    /// <summary>
    /// Framework for running application as a tray app.
    /// </summary>
    /// <remarks>
    /// Tray app code adapted from "Creating Applications with NotifyIcon in Windows Forms", Jessica Fosler,
    /// http://windowsclient.net/articles/notifyiconapplications.aspx
    /// </remarks>
    public class CustomApplicationContext : ApplicationContext
    {
        // Icon graphic from http://prothemedesign.com/circular-icons/
        private static readonly string IconFileName = "Assets\\iek.ico";
        private readonly HostManager hostManager;
        private ServiceHost _host;
        private Uri _baseAddress = new Uri("http://localhost:8889/opc");

        /// <summary>
        /// This class should be created and passed into Application.Run( ... )
        /// </summary>
        public CustomApplicationContext()
        {
            InitializeContext();
            hostManager = new HostManager(notifyIcon);
            if (!hostManager.IsDecorated)
            {
                var a = Module1.FestoManager;
                var b = Module1.GaugeXlManager;


                // Create the ServiceHost.
                _host = new ServiceHost(typeof(WebSocketsServer), _baseAddress);
                // Enable metadata publishing.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                _host.Description.Behaviors.Add(smb);

                CustomBinding binding = new CustomBinding();
                binding.Elements.Add(new ByteStreamMessageEncodingBindingElement());
                HttpTransportBindingElement transport = new HttpTransportBindingElement();
                //transport.WebSocketSettings = new WebSocketTransportSettings();
                transport.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
                transport.WebSocketSettings.CreateNotificationOnConnection = true;
                binding.Elements.Add(transport);

                _host.AddServiceEndpoint(typeof(IWebSocketsServer), binding, "");

                _host.Open();

                notifyIcon.Text = _host.State == CommunicationState.Opened ? "Listening at " + _baseAddress : "Closed";
            }
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            hostManager.BuildContextMenu(notifyIcon.ContextMenuStrip);
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            notifyIcon.ContextMenuStrip.Items.Add(hostManager.ToolStripMenuItemWithHandler(_host.State == CommunicationState.Opened ? "Listening at " + _baseAddress : "Closed", null));
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            notifyIcon.ContextMenuStrip.Items.Add(hostManager.ToolStripMenuItemWithHandler("&Exit", exitItem_Click));
        }

        #region the child forms

        private void ShowIntroForm()
        {

        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e) { ShowIntroForm(); }

        // From http://stackoverflow.com/questions/2208690/invoke-notifyicons-context-menu
        private void notifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(notifyIcon, null);
            }
        }


        // attach to context menu items
        private void showHelpItem_Click(object sender, EventArgs e) { ShowIntroForm(); }

        #endregion the child forms

        #region generic code framework

        private System.ComponentModel.IContainer components;	// a list of components to dispose when the context is disposed
        private NotifyIcon notifyIcon;				            // the icon that sits in the system tray

        private void InitializeContext()
        {
            components = new System.ComponentModel.Container();
            notifyIcon = new NotifyIcon(components)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = new Icon(IconFileName),
                Text = "Starting Opc Socket...",
                Visible = true
            };
            notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            notifyIcon.DoubleClick += notifyIcon_DoubleClick;
            notifyIcon.MouseUp += notifyIcon_MouseUp;
        }

        /// <summary>
		/// When the application context is disposed, dispose things like the notify icon.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) { components.Dispose(); }
        }

        /// <summary>
        /// When the exit menu item is clicked, make a call to terminate the ApplicationContext.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitItem_Click(object sender, EventArgs e)
        {
            ExitThread();
        }

        /// <summary>
        /// If we are presently showing a form, clean it up.
        /// </summary>
        protected override void ExitThreadCore()
        {
            notifyIcon.Visible = false; 
            // Close the ServiceHost.
            _host.Close();
            base.ExitThreadCore();
        }

        # endregion generic code framework

    }
}
