using System;
using System.Text;
using System.Collections.Generic;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace RegionSpy
{
    delegate void GotMessageEventHandler(object sender, GotMessageEventArgs args);

    public enum Window { Region, Terminal }

    class GotMessageEventArgs
    {
        private string _message;
        private Window _window;

        public GotMessageEventArgs(string message, Window window)
        {
            _message = message;
            _window = window;
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        public Window Window
        {
            get { return _window; }
            set { _window = value; }
        }
    }

    class Listener
    {
        public event GotMessageEventHandler GotMessage;
        protected virtual void OnGotMessage(GotMessageEventArgs e)
        {
            GotMessageEventHandler handler = GotMessage;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private IPAddress _ip;
        private int _port;
        private IPEndPoint _endPoint;
        private Socket _parentSocket;
        private Thread _lThread;
        private bool _stop;
        private bool _isListening;
        private byte _devId;
        private PumpSU[] _pumpsToIntercept;
        SQLWriter _writer;

        public Listener(int port)
        {
            _port = port;

            // Устанавливаем для сокета локальную конечную точку
            _ip = IPAddress.Parse("::");
            foreach (IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.ToString().StartsWith("192.168."))
                {
                    _ip = ip;
                    break;
                }
            }
            _endPoint = new IPEndPoint(_ip, port);

            _writer = new SQLWriter("localhost", "SQLEXPRESS", "UDM");
        }

        public bool IsListening
        {
            get { return _isListening; }
            set { _isListening = value; }
        }

        public byte DeviceId
        {
            get { return _devId; }
            set { _devId = value; }
        }

       
        public void Listen(PumpSU[] pumpsToIntercept)
        {
            // СУ ЭЦН, данные по которым необходимо перехватывать
            _pumpsToIntercept = pumpsToIntercept;

            // Создаем сокет Tcp/Ip
            _parentSocket = new Socket(_ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // Назначаем сокет локальной конечной точке
            _parentSocket.Bind(_endPoint);

            _lThread = new Thread(ListeningProccess);
            _lThread.Start();

            _isListening = true;
        }

        public void Stop()
        {
            _stop = true;

            if (_lThread.ThreadState == ThreadState.Running || _lThread.ThreadState == ThreadState.WaitSleepJoin)
                _lThread.Abort();

            _parentSocket.Dispose();

            _isListening = false;
        }

        private void ListeningProccess()
        {
            while (!_stop)
            {
                try
                {
                    _parentSocket.Listen(10);
                }
                catch (Exception)
                {
                    break;
                }

                // Начинаем слушать соединения
                while (!_stop)
                {
                    Socket chSocket = null;
                    try
                    {
                        // Принимаем входящие соединения от Region PULT
                        chSocket = _parentSocket.Accept();
                        byte[] incBuffer = new byte[1024];
                        int bytesRec = chSocket.Receive(incBuffer);

                        // Показываем данные на консоли
                        GotMessage(this, new GotMessageEventArgs("Полученно " + bytesRec + " байт : " + Environment.NewLine +
                            HexToStr(CutResponse(incBuffer, 0)) + Environment.NewLine, Window.Region));

                        // Дублируем сообщение на контроллер и ловим ответ
                        byte[] respBytes = SendMessageToSocket(new System.Net.IPAddress(0x9024A8C0), 502, CutResponse(incBuffer, 0));

                        // Перенаправляем ответ в сокет, с котрого получили запрос от Region PULT
                        chSocket.Send(CutResponse(respBytes, 0));

                        // Закрываем сокет
                        chSocket.Shutdown(SocketShutdown.Both);
                        chSocket.Close();

                        //try
                        //{
                        //    // Анализируем запрос -> ответ, выполняем необходимые нам действия
                        //    ProccessQuery(incBuffer, respBytes);
                        //    //// -------------не затормозит ли прослушку порта если воткнуть обработку в этом месте??
                        //}
                        //catch(Exception ex)
                        //{ 
                        //    GotMessage(this, new GotMessageEventArgs("Не удалось обработать ответ терминала. " + ex.Message, Window.Terminal));
                        //}
                    }
                    catch (Exception)
                    {
                        GotMessage(this, new GotMessageEventArgs("Соединение разорвано.", Window.Region));
                        if (chSocket != null)
                        {
                            chSocket.Shutdown(SocketShutdown.Both);
                            chSocket.Close();
                        }
                        break;
                    }

                    chSocket.Dispose();
                }
            }
        }

        /// <summary>
        /// Отправляет массив байт в сокет
        /// </summary>
        /// <param name="ip">IP адрес</param>
        /// <param name="port">Порт</param>
        /// <param name="bytesToSend">Массив данных</param>
        /// <returns>Ответ на запрос (byte[] массив)</returns>
        public byte[] SendMessageToSocket(IPAddress ip, int port, byte[] bytesToSend)
        {
            // Буфер для входящих данных
            byte[] incBuffer = new byte[1024];

            // Устанавливаем удаленную точку для сокета
            IPEndPoint ipEndPoint = new IPEndPoint(ip, port);
            Socket sender = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Соединяем сокет с удаленной точкой
            sender.Connect(ipEndPoint);

            // Отправляем данные через сокет
            int bytesSent = sender.Send(bytesToSend);
            GotMessage(this, new GotMessageEventArgs("Отправлено на терминал " + bytesSent + " байт: " + Environment.NewLine + HexToStr(bytesToSend), Window.Terminal));

            // Получаем ответ от сервера
            int bytesRec = sender.Receive(incBuffer);

            string respStr;
            if (bytesToSend[5] == 0x06 && bytesToSend[7] == 0x04 && bytesToSend[8] == 0x00 && bytesToSend[9] == 0x64 && bytesToSend[10] == 0x00 && bytesToSend[11] == 0x4B)
                respStr = HexToStr(CutResponse(incBuffer, 1));
            else
                respStr = HexToStr(CutResponse(incBuffer, 0));

            GotMessage(this, new GotMessageEventArgs("Ответ от терминала " + bytesRec + " байт: " + Environment.NewLine + respStr + Environment.NewLine + "-----------------------------------", Window.Terminal));

            // Освобождаем сокет
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();

            return incBuffer;
        }

        /// <summary>
        /// Преобразует byte[] массив в соответствующее строковое представление
        /// </summary>
        /// <param name="hexData">byte[] массив данных</param>
        /// <returns>string</returns>
        private string HexToStr(byte[] hexData)
        {
            StringBuilder str = new StringBuilder();
            foreach (byte b in hexData)
            {
                str.Append(string.Format("{0:X2} ", b));
            }
            return str.ToString();
        }

        /// <summary>
        /// Размер буфера приема 1024 байта. Урезает ответ,основываясь на передаваемой в пакете длине сообщения.
        /// </summary>
        /// <param name="longResponse">Буфер ответа (1024 байта)</param>
        /// <param name="rType">
        /// У нас 2 типа ответов:
        ///     0 - ответ MicroLogix на запись данных (берем длину пакета из TCP фрейма (байт 5))
        ///     1 - ответ от подчиненного устройства, дублируемый MicroLogix'ом. В этом случае запрашивается всегда 75 байт.
        ///         А ответ зачастую меньше. Поэтому берем длину пакета из фрейма вложенного ответа (байт 13)
        /// </param>
        /// <returns>Урезанный массив byte[]</returns>
        private byte[] CutResponse(byte[] longResponse, int rType)
        {
            int i = 0;
            byte[] shortResponse;
            int len;
            if (rType == 0)
                len = longResponse[5] + 6;
            else
                len = longResponse[13] + 14;

            shortResponse = new byte[len];

            foreach (byte b in longResponse)
            {
                if (i < len)
                {
                    shortResponse[i] = b;
                    i++;
                }
                else
                    return shortResponse;
            }
            return shortResponse;
        }

        private void ProccessQuery(byte[] m, byte[] r)
        {
            if (r[7] == 0x04 && r[12] == 0x04)
            {
                foreach (PumpSU pumpSU in _pumpsToIntercept)
                {
                    if (r[11] == pumpSU.IDhex)
                    {
                       pumpSU.GetValuesFromResponse(r);

                       try
                       { _writer.UpdateUDMItems(pumpSU); }
                       catch (NothingWrittenException ex)
                       { GotMessage(this, new GotMessageEventArgs(ex.Message, Window.Terminal)); }
                    }
                }
            }
        }
    }
}
