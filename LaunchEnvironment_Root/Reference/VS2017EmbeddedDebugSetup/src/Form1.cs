// Copyright (C) 2017 Teradyne, Inc. All rights reserved.
//
// This document contains proprietary and confidential information of Teradyne,
// Inc. and is tendered subject to the condition that the information (a) be
// retained in confidence and (b) not be used or incorporated in any product
// except with the express written consent of Teradyne, Inc.
//
// (Place the most recent revision history at the top.)
// Date         Name                Bug #           Notes
// 2017 Dec 21  Tibor Trunk                         Added support for DISA-HP which uses PPC64.
// 2017 Nov 25  Tibor Trunk                         Created.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinSCP;

namespace VS2017EmbeddedDebugSetup
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cbModule.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            int slot;
            if (!int.TryParse(tbSlot.Text, out slot) || slot < 0 || slot > 49)
            {
                MessageBox.Show("Slot must be between 0 and 49", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int[] modules;
            if (cbModule.Text == "All")
            {
                modules = new int[] { 1, 2, 3, 4 };
            }
            else
            {
                modules = new int[] { int.Parse(cbModule.Text) };
            }

            bool errors = false;
            foreach (int module in modules)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    InstallTools(slot, module);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errors = true;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }

            if (!errors)
            {
                MessageBox.Show("Installation successful", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void InstallTools(int slot, int module)
        {
            string ipAddress = GetIPAddress(slot, module);

            // SessionOptions.Timeout doesn't seem to work so we ping first to make sure the target is alive.
            Ping ping = new Ping();
            PingReply reply = ping.Send(ipAddress, 250);
            if (reply.Status != IPStatus.Success)
            {
                throw new ApplicationException("Could not connect to " + ipAddress);
            }

            using (Session session = new Session())
            {
                SessionOptions sessionOptions = new SessionOptions
                {
                    HostName = ipAddress,
                    UserName = "root",
                    Password = tbPassword.Text,
                    Protocol = Protocol.Scp,

                    // DISA
                    SshHostKeyFingerprint = "ssh-rsa 1040 72:e4:60:61:30:a4:2f:ae:cf:80:c6:10:c3:09:29:68",

                    // VM
                    //SshHostKeyFingerprint = "ssh-rsa 2048 30:e3:8b:37:10:37:89:f1:fe:7c:8a:61:8d:bd:34:04",
                };

                session.Open(sessionOptions);

                // Determine if DISA (32-bit cpu) or DISA-HP (64-bit cpu).
                var unameResult = session.ExecuteCommand("uname -m");
                unameResult.Check();

                TransferOptions xferOptions = new TransferOptions();
                xferOptions.FilePermissions = new FilePermissions { Octal = "555" };

                var result = session.PutFiles(@"bin-ppc\gdb", @"/usr/bin/gdb", options: xferOptions);
                result.Check();

                if (unameResult.Output == "ppc64")
                {
                    result = session.PutFiles(@"bin-ppc\sftp-server-64", @"/usr/libexec/sftp-server", options: xferOptions);
                    result.Check();

                    result = session.PutFiles(@"bin-ppc\libcrypto.so.1.0.0", @"/usr/lib64/libcrypto.so.1.0.0");
                    result.Check();
                }
                else if (unameResult.Output == "ppc")
                {
                    result = session.PutFiles(@"bin-ppc\sftp-server-32", @"/usr/libexec/sftp-server", options: xferOptions);
                    result.Check();
                }
                else
                {
                    throw new ApplicationException("Unknown hardware architecture: " + unameResult.Output);
                }
            }
        }

        private string GetIPAddress(int slot, int module)
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.Name.Equals("Teradyne_Private", StringComparison.CurrentCultureIgnoreCase))
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            byte[] address = ip.Address.GetAddressBytes();
                            address[3] = (byte)(20 + slot * 4 + module);
                            return new IPAddress(address).ToString();
                        }
                    }
                }
            }

            throw new ApplicationException("Network interface Teradyne_Private not found.");
        }
    }
}
