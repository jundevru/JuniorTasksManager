using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetworkCore;

namespace TasksManagerServer
{
    public partial class MainForm : Form
    {

        private Server server;


        public MainForm()
        {
            InitializeComponent();
            StartServer();
            this.bt_start.Click += (s, e) =>
            {
                StartServer();
                bt_start.Enabled = false;
                bt_stop.Enabled = true;
            };
            this.bt_stop.Click += (s, e) =>
            {
                if (server.IsWork)
                    server.TryStop();
            };
            this.FormClosed += (s, e) => {
                if (server.IsWork)
                    server.TryStop();
            };
        }
        void StartServer()
        {
            server = new Server();
            server.ClientLoggedInEvent += (s) =>
            {
                Action action = () => {
                    lb_log.Items.Add(s + " connected");
                };
                this.InvokeEx(action);
            };
            server.ClientLoggedOutEvent += (s) =>
            {
                this.InvokeEx(new Action(delegate () {
                    lb_log.Items.Add(s + " disconnected");
                }));
            };
            server.ServerStarted += () =>
            {
                Action action = () => {
                    lb_log.Items.Add("Server started");
                };
                this.InvokeEx(action);
            };
            server.ServerStopped += () =>
            {
                Action action = () => {
                    lb_log.Items.Add("Server stopped");
                    bt_start.Enabled = true;
                    bt_stop.Enabled = false;
                };
                this.InvokeEx(action);
            };
            server.ConnectionsChange += (c) =>
            {
                Action action = () =>
                {
                    label_count.Text = c.ToString();
                };
                this.InvokeEx(action);
            };
        }





    }
}
