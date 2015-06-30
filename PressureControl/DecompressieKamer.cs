using System.IO.Ports;
using System;
using System.Threading;
using System.Diagnostics;

namespace PressureControl
{
    public class DecompressieKamer
    {
        public delegate void OnEventHandler(object sender, Actions action);
        public event OnEventHandler OnEvent;

        private SerialPort _SerialPort;
        private Message _Message;
        private DateTime _LastTimeAlive;
        private DateTime _LastUpdateTime;
        private DateTime _LastDataSendTime;
        private System.Windows.Forms.Timer _Timer;

        public ushort PressureMilliBar { get; private set; }
        public bool VentValveOpen { get; private set; }
        public bool AirPumpOn { get; private set; }
        public bool AreValuesUpToDate { get; private set; }
        public bool ControlFromPC { get; set; }
        public byte PressureGlobalBar { get; set; }

        private bool IsPCControlStatusOK { get; set; }

        public DecompressieKamer(int maxTries = 3)
        {
            AreValuesUpToDate = false;

            _SerialPort = new SerialPort();

            //RP6 default config 38400,8N1
            _SerialPort.BaudRate = 38400;
            _SerialPort.DataBits = 8;
            _SerialPort.Parity = Parity.None;
            _SerialPort.StopBits = StopBits.One;

            if (!SearchForDecompressionDevice(maxTries))
            {
                _SerialPort.Dispose();
                throw new NoDeviceFoundException();
            }

            _Message = new Message(_SerialPort);

            _LastTimeAlive = DateTime.Now;
            _LastUpdateTime = DateTime.Now;

        }

        public void StartProcessing()
        {
            _Timer = new System.Windows.Forms.Timer();
            _Timer.Interval = 15;
            _Timer.Enabled = true;
            _Timer.Tick += ProcessSerial;
            _Timer.Start();
        }

        public void ProcessSerial(object sender, System.EventArgs e)
        {
            if (!_SerialPort.IsOpen)
            {
                return;
            }

            if ((DateTime.Now - _LastTimeAlive).TotalMilliseconds > 2500 || (DateTime.Now - _LastUpdateTime).TotalMilliseconds > 2500)
            {
                if (OnEvent != null)
                {
                    OnEvent(this, Actions.TIMEOUT_OCCURED);
                }
            }

            bool sendData = false;

            try
            {
                if (_Message.Receive() > 0)
                {
                    _LastTimeAlive = DateTime.Now;
                    if (!_Message.IsCorrupt)
                    {
                        switch ((Actions)_Message.Action)
                        {
                            case Actions.UPDATE_SETTINGS:
                                //VentValveOpen;AirPumpOn;PressureGlobalBar;ControlFromPC;PressureMilliBar
                                if (_Message.DataLength == 6)
                                {
                                    _LastUpdateTime = DateTime.Now;

                                    byte readval = 0;
                                    if (_Message.Read_byte(ref readval))
                                    {
                                        VentValveOpen = readval != 0;
                                        if (_Message.Read_byte(ref readval))
                                        {
                                            AirPumpOn = readval != 0;
                                            if (_Message.Read_byte(ref readval))
                                            {
                                                if (!ControlFromPC)
                                                {   //only read global set pressure
                                                    PressureGlobalBar = readval;
                                                }

                                                if (OnEvent != null)
                                                {
                                                    OnEvent(this, PressureGlobalBar != readval ? Actions.INCORRECT_PRESSURE : Actions.CORRECT_PRESSURE);
                                                }

                                                if (_Message.Read_byte(ref readval))
                                                {
                                                    IsPCControlStatusOK = (ControlFromPC == (readval != 0));

                                                    if (OnEvent != null && IsPCControlStatusOK)
                                                    {
                                                        OnEvent(this, readval == 1 ? Actions.PC_AQUIRE_CONTROL_SUCCESS : Actions.PC_RELEASE_CONTROL_SUCCESS);
                                                    }

                                                    short millibars = 0;
                                                    if (_Message.Read_Int16(ref millibars))
                                                    {
                                                        PressureMilliBar = (ushort)millibars;
                                                        if (!AreValuesUpToDate)
                                                        {
                                                            AreValuesUpToDate = true;
                                                        }

                                                        if (OnEvent != null)
                                                        {
                                                            OnEvent(this, Actions.UPDATE_SETTINGS);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                break;

                            case Actions.PC_AQUIRE_CONTROL_SUCCESS:
                            case Actions.PC_RELEASE_CONTROL_SUCCESS:
                                if (OnEvent != null)
                                {
                                    OnEvent(this, (Actions)_Message.Action);
                                }
                                break;
                        }
                    }
                }

                if ((DateTime.Now - _LastDataSendTime).TotalMilliseconds > 100)
                {
                    _LastDataSendTime = DateTime.Now;

                    if (!IsPCControlStatusOK)
                    {
                        _Message.BeginWrite();
                        _Message.Action = (byte)(ControlFromPC ? Actions.PC_AQUIRE_CONTROL : Actions.PC_RELEASE_CONTROL);
                        _Message.Send();
                        sendData = true;
                    }

                    if (ControlFromPC)
                    {
                        _Message.BeginWrite();
                        _Message.Action = (byte)Actions.UPDATE_GLOBAL_PRESSURE;
                        _Message.Write_byte(PressureGlobalBar);
                        _Message.Send();
                        sendData = true;
                    }

                    if (!sendData)
                    {
                        _Message.BeginWrite();
                        _Message.Action = (byte)Actions.PING;
                        _Message.Send();
                    }
                }
            }
            catch (Exception)
            {   /*State not OK? Connection corrupted? */
                if (OnEvent != null)
                {
                    OnEvent(this, Actions.CONNECTION_CORRUPTED);
                }
            }
        }

        public bool Dispose()
        {
            if(_Timer != null)
            {
                _Timer.Tick -= ProcessSerial;
                _Timer.Stop();
            }
            try
            {
                _SerialPort.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Automatically check existing devices and see if they are a decompression chamber by trial and error
        /// </summary>
        /// <param name="maxTries"></param>
        /// <returns>true if found false if not</returns>
        private bool SearchForDecompressionDevice(int maxTries)
        {
            if (_SerialPort == null)
            {
                return false;
            }

            bool found = false;
            int tries = 0;

            Thread.Sleep(1000);

            while (!found && tries < maxTries)
            {
                foreach (string port in SerialPort.GetPortNames())
                {
                    if (_SerialPort.IsOpen)
                    {
                        try
                        {
                            _SerialPort.Close();
                        }
                        catch (Exception) { /*We just want to disconnect and continue searching*/ }
                    }

                    try
                    {

                        _SerialPort.PortName = port;
                        _SerialPort.Open();

                        Message message = new Message(_SerialPort);

                        message.BeginWrite();
                        message.Action = (byte)Actions.CHECK_FOR_DECOMPRESSION_DEVICE;
                        message.Write_byte((byte)MsgStruct.MS_DataBegin);
                        message.Send();

                        Stopwatch watch = new Stopwatch();
                        watch.Start();

                        while (message.Receive() != 6 && watch.ElapsedMilliseconds < 150) { }

                        if (message.DataLength == 1 && !message.IsCorrupt)
                        {
                            byte output = 0;
                            if (message.Action == (byte)Actions.HERE_IS_A_DECOMPRESSION_DEVICE && message.Read_byte(ref output))
                            {
                                if (output == (byte)MsgStruct.MS_DataEnd)
                                {
                                    //found decompression device
                                    return true;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                ++tries;
                Thread.Sleep(1);
            }
            return false;
        }
    }
}
