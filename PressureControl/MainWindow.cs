using System;
using System.Windows.Forms;

namespace PressureControl
{
    public partial class MainWindow : Form
    {
        public DecompressieKamer _DecompressieKamer;

        public MainWindow()
        {
            InitializeComponent();
            _DecompressieKamer = null;
        }

        private void SearchDevices()
        {
            //set _DecompressieKamer if found to != null
            Searcher searcher = new Searcher(this);

            if (_DecompressieKamer != null)
            {
                _DecompressieKamer.Dispose();
                _DecompressieKamer = null;
                searcher.Text = "Connection lost, reconnecting...";
                //reset all local stats here and wait for remote stats then update here
            }

            searcher.ShowDialog();
            if (_DecompressieKamer == null)
            {
                Application.Exit();
            }
            else
            {
                //success, connected with Decompression chamber
                _DecompressieKamer.OnEvent += _DecompressieKamer_OnEvent;
                _DecompressieKamer.StartProcessing();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SearchDevices();
        }

        private void UpdateControl()
        {
            GlobalPressureControl.Enabled = (RemoteControlStatusViewer.Checked && CheckBox_TakeControl.Checked);
            if (_DecompressieKamer != null)
            {
                _DecompressieKamer.ControlFromPC = CheckBox_TakeControl.Checked;
            }
        }

        private void _DecompressieKamer_OnEvent(object sender, Actions action)
        {
            switch (action)
            {
                case Actions.TIMEOUT_OCCURED:
                case Actions.CONNECTION_CORRUPTED:

                    SearchDevices();
                    UpdateControl();

                    GlobalPressureControl.BackColor = System.Drawing.Color.Red;
                    MeasuredPressureViewer.BackColor = System.Drawing.Color.Red;
                    VentValveStateViewer.BackColor = System.Drawing.Color.Red;
                    AirPumpStateViewer.BackColor = System.Drawing.Color.Red;

                    GlobalPressureControl.Value = 0;
                    MeasuredPressureViewer.Value = 0;

                    VentValveStateViewer.Text = "";
                    AirPumpStateViewer.Text = "";

                    break;

                case Actions.INCORRECT_PRESSURE:

                    GlobalPressureControl.BackColor = System.Drawing.Color.Red;

                    break;

                case Actions.CORRECT_PRESSURE:

                    GlobalPressureControl.BackColor = System.Drawing.Color.White;

                    break;

                case Actions.UPDATE_SETTINGS:

                    MeasuredPressureViewer.BackColor = System.Drawing.Color.White;
                    VentValveStateViewer.BackColor = System.Drawing.Color.White;
                    AirPumpStateViewer.BackColor = System.Drawing.Color.White;

                    if (!CheckBox_TakeControl.Checked)
                    {
                        GlobalPressureControl.Value = Clamper.Clamp(_DecompressieKamer.PressureGlobalBar, 0, 6);
                    }

                    MeasuredPressureViewer.Value = Clamper.Clamp(((UInt16)_DecompressieKamer.PressureMilliBar), 0, 60000);

                    VentValveStateViewer.Text = _DecompressieKamer.VentValveOpen ? "OPEN" : "CLOSED";
                    AirPumpStateViewer.Text = _DecompressieKamer.AirPumpOn ? "ON" : "OFF";

                    break;

                case Actions.PC_AQUIRE_CONTROL_SUCCESS:

                    RemoteControlStatusViewer.Checked = true;
                    UpdateControl();

                    break;

                case Actions.PC_RELEASE_CONTROL_SUCCESS:

                    RemoteControlStatusViewer.Checked = false;
                    UpdateControl();

                    break;

            }
        }

        private void CheckBox_TakeControl_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControl();
        }

        private void GlobalPressureControl_ValueChanged(object sender, EventArgs e)
        {
            if (RemoteControlStatusViewer.Checked && CheckBox_TakeControl.Checked && _DecompressieKamer != null)
            {
                _DecompressieKamer.PressureGlobalBar = (byte)GlobalPressureControl.Value;
            }
        }
    }
}
