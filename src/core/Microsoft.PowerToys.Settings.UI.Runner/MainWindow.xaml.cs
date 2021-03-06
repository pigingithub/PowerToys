﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using Microsoft.PowerToys.Settings.UI.Views;
using Microsoft.Toolkit.Wpf.UI.XamlHost;
using Windows.UI.Popups;

namespace Microsoft.PowerToys.Settings.UI.Runner
{
    // Interaction logic for MainWindow.xaml.
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void WindowsXamlHost_ChildChanged(object sender, EventArgs e)
        {
            // Hook up x:Bind source.
            WindowsXamlHost windowsXamlHost = sender as WindowsXamlHost;
            ShellPage shellPage = windowsXamlHost.GetUwpInternalObject() as ShellPage;

            if (shellPage != null)
            {
                // send IPC Message
                shellPage.SetDefaultSndMessageCallback(msg =>
                {
                    //IPC Manager is null when launching runner directly
                    Program.GetTwoWayIPCManager()?.Send(msg);
                });

                // send IPC Message
                shellPage.SetRestartAdminSndMessageCallback(msg =>
                {
                    Program.GetTwoWayIPCManager().Send(msg);
                    System.Windows.Application.Current.Shutdown(); // close application
                });

                shellPage.SetElevationStatus(Program.IsElevated);
                shellPage.Refresh();
            }
        }
    }
}
