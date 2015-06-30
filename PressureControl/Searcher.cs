using System.Windows.Forms;

namespace PressureControl
{
    public partial class Searcher : Form
    {
        private DecompressieKamer _DecompressieKamer;
        private MainWindow _MainWindow;
        private System.Timers.Timer _SearchThread;
        private Timer _RefreshTimer;

        public Searcher(MainWindow main)
        {
            InitializeComponent();

            _MainWindow = main;

            _DecompressieKamer = null;

            _SearchThread = new System.Timers.Timer();
            _SearchThread.Interval = 10;
            _SearchThread.Elapsed += _SearchThread_Elapsed;
            _SearchThread.Start();

            _RefreshTimer = new Timer();
            _RefreshTimer.Interval = 50;
            _RefreshTimer.Tick += _RefreshTimer_Tick;
            _RefreshTimer.Start();
        }

        private void _RefreshTimer_Tick(object sender, System.EventArgs e)
        {
            if(!_SearchThread.Enabled)
            {
                _RefreshTimer.Stop();
                if (_DecompressieKamer == null)
                {
                    SearchBar.MarqueeAnimationSpeed = 0;
                    if (MessageBox.Show("Could not find a connected Decompression Chamber.\r\n\r\n                                                 Try again or abort?", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) != DialogResult.Retry)
                    {
                        Close();
                    }
                    else
                    {
                        SearchBar.MarqueeAnimationSpeed = 20;
                        _SearchThread.Interval = 10;
                        _SearchThread.Start();
                        _RefreshTimer.Start();
                    }
                }
                else
                {
                    _MainWindow._DecompressieKamer = _DecompressieKamer;
                    Close();
                }
            }
        }

        private void _SearchThread_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_SearchThread.Interval == 10)
            {
                _SearchThread.Interval = 600000;
                try
                {
                    //search for an available decompression chamber, if not found exception is thrown because the class cannot do it's work then.. ask user what to do
                    _DecompressieKamer = new DecompressieKamer(15000);
                }
                catch (NoDeviceFoundException)
                {

                }
                _SearchThread.Stop();
            }
        }
    }
}
