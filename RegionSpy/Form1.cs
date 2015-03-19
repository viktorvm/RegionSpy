using System;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace RegionSpy
{
    public partial class fMain : Form
    {
        Listener _listener;
        PumpSU[] _pumpsToIntercept;

        public fMain()
        {
            InitializeComponent();

            _listener = new Listener(502);
            _listener.GotMessage += listener_GotMessage;

            _pumpsToIntercept = new PumpSU[] 
            {
                new PumpSU(9, 1079, 201, PumpSUProtocol.ElectonM),
                new PumpSU(9, 2079, 202, PumpSUProtocol.Универсал),
                new PumpSU(9, 1525, 204, PumpSUProtocol.ElectonM),
                new PumpSU(9, 1515, 210, PumpSUProtocol.ElectonM),
                new PumpSU(9, 1516, 212, PumpSUProtocol.ElectonM),
            };
        }

        void listener_GotMessage(object sender, GotMessageEventArgs args)
        {
            if (args.Window == Window.Region)
            {
                if (tbRegion.InvokeRequired)
                    tbRegion.Invoke(new EventHandler(delegate { tbRegion.Text += args.Message + Environment.NewLine; }));
                else
                    tbRegion.Text += args.Message + Environment.NewLine;
            }
            if (args.Window == Window.Terminal)
            {
                if (tbTerminal.InvokeRequired)
                    tbTerminal.Invoke(new EventHandler(delegate { tbTerminal.Text += args.Message + Environment.NewLine; }));
                else
                    tbTerminal.Text += args.Message + Environment.NewLine;
            }
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            btnGo.Enabled = false;

            _listener.Listen(_pumpsToIntercept);

            btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;

            _listener.Stop();

            btnGo.Enabled = true;
        }

        private void fMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_listener.IsListening)
                _listener.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ////listener.SendMessageFromSocket(new System.Net.IPAddress(0x9024A8C0), 502, StrToByteArray(tbToSend.Text.Replace(" ",string.Empty)));

            //float f = 1008.8435f;
            //byte[] b = BitConverter.GetBytes(f);
            //float fn = BitConverter.ToSingle(b, 0);
        }

        private void tbRegion_TextChanged(object sender, EventArgs e)
        {
            tbRegion.Select(tbRegion.TextLength, 0);
            tbRegion.ScrollToCaret();
            ////защита от переполнения TextBox
            //if (tbRegion.Text.Length > 50000)
            //    tbRegion.Text = string.Empty;
        }

        private void tbTerminal_TextChanged(object sender, EventArgs e)
        {
            tbTerminal.Select(tbTerminal.TextLength, 0);
            tbTerminal.ScrollToCaret();
            ////защита от переполнения TextBox
            //if (tbTerminal.Text.Length > 50000)
            //    tbTerminal.Text = string.Empty;
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

        public static byte[] StrToByteArray(string str)
        {
            Dictionary<string, byte> hexindex = new Dictionary<string, byte>();
            for (int i = 0; i <= 255; i++)
                hexindex.Add(i.ToString("X2"), (byte)i);

            List<byte> hexres = new List<byte>();
            for (int i = 0; i < str.Length; i += 2)
                hexres.Add(hexindex[str.Substring(i, 2)]);

            return hexres.ToArray();
        }

        private void tbHex_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (String.IsNullOrEmpty(tbHex.Text))
                return;

            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                tbDec.Text = BitConverter.ToSingle(StrToByteArray(tbHex.Text.Replace(" ", "")),0).ToString();
            }
        }

        private void tbDec_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (String.IsNullOrEmpty(tbDec.Text))
                return;
            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                float number = Convert.ToSingle(tbDec.Text);
                tbHex.Text = HexToStr(BitConverter.GetBytes(number));
            }
        }
    }
}
