using System.IO.Ports;
using System.Threading;
using System;

namespace PressureControl
{
    /// <summary>
    /// 0 Magic Numbers
    /// </summary>
    public class Message
    {
        /// <summary>
        /// 0, 1 startbytes, 2 action, 3 datalen, 4 CorruptionCheck, 5 ... 30 data, 31 always '0'
        /// </summary>
        private byte[] _Message;
        private byte Offset { get; set; }
        private SerialPort _SerialPort;

        private byte CalculateCorruptionCheck()
        {
            int sum = 0;
            if (DataLength > 0)
            {
                for (int i = 0; i < DataLength; ++i)
                {
                    sum += _Message[i + (int)MsgStruct.MS_DataBegin];
                }
                sum %= (int)MsgStruct.MS_CorrModulo;
                sum ^= DataLength;
            }
            sum += Action;

            return (byte)sum;
        }

        private void UpdateCorruptionCheck(bool forWrite)
        {
            if (forWrite)
            {   //prepare simple hash before sending data to RP6 (ReallyPoorSix bot)
                IsCorrupt = false;
                ExcpectedCorruptionCheck = CalculateCorruptionCheck();
            }
            else
            {   //check if read is corrupted
                IsCorrupt = ExcpectedCorruptionCheck != CalculateCorruptionCheck();
            }
        }

        public bool IsCorrupt { get; private set; }

        public byte Action
        {
            get { return _Message[(int)MsgStruct.MS_Action]; }
            set { _Message[(int)MsgStruct.MS_Action] = value; }
        }

        public byte ExcpectedCorruptionCheck
        {
            get { return _Message[(int)MsgStruct.MS_CorrCheck]; }
            private set { _Message[(int)MsgStruct.MS_CorrCheck] = value; }
        }

        public byte DataLength
        {
            get { return _Message[(int)MsgStruct.MS_DataLen] > (byte)MsgStruct.MS_MaxDataLen ? (byte)MsgStruct.MS_MaxDataLen : _Message[(int)MsgStruct.MS_DataLen]; }
            private set { _Message[(int)MsgStruct.MS_DataLen] = value > (byte)MsgStruct.MS_MaxDataLen ? (byte)MsgStruct.MS_MaxDataLen : value; }
        }

        public Message(SerialPort port)
        {
            if (port == null)
            {
                throw new ArgumentNullException("port");
            }

            _SerialPort = port;

            _Message = new byte[(int)MsgStruct.MS_BufferSize];

            _Message[(int)MsgStruct.MS_StartA] = (byte)MsgStruct.MS_CharA;
            _Message[(int)MsgStruct.MS_StartB] = (byte)MsgStruct.MS_CharB;
            _Message[(int)MsgStruct.MS_AlwaysZero] = 0;

            BeginWrite();
        }

        #region read operations
        public bool Read_Int16(ref Int16 output)
        {
            byte size = sizeof(Int16);
            if (Offset + size > DataLength)
            {
                return false;
            }
            output = BitConverter.ToInt16(_Message, (int)MsgStruct.MS_DataBegin + Offset);
            Offset += size;
            return true;
        }

        public bool Read_byte(ref byte output)
        {
            byte size = sizeof(byte);
            if (Offset + size > DataLength)
            {
                return false;
            }
            output = _Message[(int)MsgStruct.MS_DataBegin + Offset];
            Offset += size;
            return true;
        }
        #endregion

        #region write operations
        public bool Write_Int16(Int16 input)
        {
            byte size = sizeof(Int16);
            if (Offset + size > (byte)MsgStruct.MS_MaxDataLen)
            {
                return false;
            }
            Array.Copy(BitConverter.GetBytes(input), 0, _Message, (int)MsgStruct.MS_DataBegin + Offset, size);
            Offset += size;
            if (Offset > DataLength)
            {
                DataLength = Offset;
            }
            return true;
        }

        public bool Write_byte(byte input)
        {
            byte size = sizeof(byte);
            if (Offset + size > (byte)MsgStruct.MS_MaxDataLen)
            {
                return false;
            }
            _Message[(int)MsgStruct.MS_DataBegin + Offset] = input;
            Offset += size;
            if (Offset > DataLength)
            {
                DataLength = Offset;
            }
            return true;
        }
        #endregion

        #region serial comms
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Amount of bytes read</returns>
        public byte Receive()
        {
            if (!_SerialPort.IsOpen)
            {
                return 0;
            }

            if (_SerialPort.BytesToRead < (byte)MsgStruct.MS_DataBegin)
            {
                return 0;
            }

            if (_SerialPort.ReadTimeout != 0)
            {
                _SerialPort.ReadTimeout = 0;//we dont want to wait for reads because we check the buffer size before reading
            }

            if (_SerialPort.ReadByte() != (byte)MsgStruct.MS_CharA)
            {
                return 0;
            }

            if (_SerialPort.ReadByte() != (byte)MsgStruct.MS_CharB)
            {
                return 0;
            }

            BeginWrite();

            Action = (byte)_SerialPort.ReadByte();
            DataLength = (byte)_SerialPort.ReadByte();
            ExcpectedCorruptionCheck = (byte)_SerialPort.ReadByte();

            if (_SerialPort.BytesToRead < DataLength)
            {
                Thread.Sleep(25);
                if (_SerialPort.BytesToRead < DataLength)
                {
                    IsCorrupt = true;
                    return (byte)MsgStruct.MS_DataBegin;//discart message because we are not receiving anything further
                }
            }

            _SerialPort.Read(_Message, (int)MsgStruct.MS_DataBegin, DataLength);
            UpdateCorruptionCheck(false);

            return (byte)((byte)MsgStruct.MS_DataBegin + DataLength);
        }

        /// <summary>
        /// Sends the message to serial port
        /// </summary>
        /// <returns>Amount of bytes written to serial</returns>
        public byte Send()
        {
            if (!_SerialPort.IsOpen)
            {
                return 0;
            }

            UpdateCorruptionCheck(true);

            try
            {
                _SerialPort.Write(_Message, 0, (int)MsgStruct.MS_DataBegin + DataLength);
                return (byte)((byte)MsgStruct.MS_DataBegin + DataLength);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        public void BeginWrite()
        {
            Offset = 0;
            DataLength = 0;
            Action = 0;
            IsCorrupt = false;
        }
    }
}
