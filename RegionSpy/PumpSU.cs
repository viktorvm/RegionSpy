using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegionSpy
{
    public enum PumpSUProtocol { Универсал, ElectonM };

    class PumpSU
    {
        private int _kNum;
        private int _wNum;
        private int _id;
        private PumpSUProtocol _protocol;
        private Dictionary<string, float> _pVals;

        public PumpSU(int kNum, int wNum, int id, PumpSUProtocol protocol)
        {
            if (id > 254)
                throw new Exception("Не удалось создать экземпляр класса PumpSU. Адрес не должен превышать 254.");

            _kNum = kNum;
            _wNum = wNum;
            _id = id;
            _protocol = protocol;
            _pVals = new Dictionary<string, float>()
            {
                {"Ia", 0f},       //1
                {"Ib", 0f},       //2
                {"Ic", 0f},       //3
                {"Idis", 0f},     //4
                {"Uab", 0f},      //5
                {"Ubc", 0f},      //6
                {"Uca", 0f},      //7
                {"Udis", 0f},     //8
                {"Riz", 0f},      //9
                {"Cosf", 0f},     //10
                {"EngLoad", 0f},  //11
                {"P", 0f},        //12
                {"S", 0f},        //13
                {"Twork", 0f},    //14
                {"Nstart", 0f},   //15

                {"Online", 0f}    //16
            };
        }

        public int ID
        {
            get { return _id; }
        }
        public byte IDhex
        {
            get { return BitConverter.GetBytes(_id)[0]; }
        }
        public PumpSUProtocol Protocol
        {
            get { return _protocol; }
        }
        public Dictionary<string, float> PValues
        {
            get { return _pVals; }
        }
        public int Kust
        {
            get { return _kNum; }
        }
        public int Well
        {
            get { return _wNum; }
        }

        public void GetValuesFromResponse(byte[] response)
        {
            byte[] temp = new byte[2];
            int pr = 14;

            if (_protocol == PumpSUProtocol.Универсал)
            {
                #region Парсинг ответа протокола Универсал
                if (response[pr - 1] == 0x00)
                {
                    _pVals["Ia"] = 0f;
                    _pVals["Ib"] = 0f;
                    _pVals["Ic"] = 0f;
                    _pVals["Idis"] = 0f;
                    _pVals["Uab"] = 0f;
                    _pVals["Ubc"] = 0f;
                    _pVals["Uca"] = 0f;
                    _pVals["Udis"] = 0f;
                    _pVals["Riz"] = 0f;
                    _pVals["Cosf"] = 0f;
                    _pVals["EngLoad"] = 0f;
                    _pVals["P"] = 0f;
                    _pVals["S"] = 0f;
                    _pVals["Twork"] = 0f;
                    _pVals["Nstart"] = 0f;

                    _pVals["Online"] = 0;
                }

                if (response[pr - 1] == 0x36)
                {
                    temp[0] = response[5 + pr];
                    temp[1] = response[4 + pr];
                    _pVals["Ia"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString()) / 10;

                    temp[0] = response[7 + pr];
                    temp[1] = response[6 + pr];
                    _pVals["Ib"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString()) / 10;

                    temp[0] = response[9 + pr];
                    temp[1] = response[8 + pr];
                    _pVals["Ic"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString()) / 10;

                    temp[0] = response[11 + pr];
                    temp[1] = response[10 + pr];
                    _pVals["Idis"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString());

                    temp[0] = response[13 + pr];
                    temp[1] = response[12 + pr];
                    _pVals["Uab"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString());

                    temp[0] = response[15 + pr];
                    temp[1] = response[14 + pr];
                    _pVals["Ubc"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString());

                    temp[0] = response[17 + pr];
                    temp[1] = response[16 + pr];
                    _pVals["Uca"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString());

                    temp[0] = response[19 + pr];
                    temp[1] = response[18 + pr];
                    _pVals["Udis"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString());

                    temp[0] = response[21 + pr];
                    temp[1] = response[20 + pr];
                    _pVals["Riz"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString());

                    temp[0] = response[23 + pr];
                    temp[1] = response[22 + pr];
                    _pVals["Cosf"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString()) / 100;

                    temp[0] = response[25 + pr];
                    temp[1] = response[24 + pr];
                    _pVals["EngLoad"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString());

                    temp[0] = response[27 + pr];
                    temp[1] = response[26 + pr];
                    _pVals["P"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString()) / 10;

                    temp[0] = response[29 + pr];
                    temp[1] = response[28 + pr];
                    _pVals["S"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString()) / 10;

                    _pVals["Twork"] = 0f;

                    temp[0] = response[47 + pr];
                    temp[1] = response[46 + pr];
                    _pVals["Nstart"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString());
                }
                #endregion
            }

            if (_protocol == PumpSUProtocol.ElectonM)
            {
                #region Парсинг ответа протокола ElectonM
                if (response[pr - 1] == 0x00)
                {
                    _pVals["Ia"] = 0f;
                    _pVals["Ib"] = 0f;
                    _pVals["Ic"] = 0f;
                    _pVals["Idis"] = 0f;
                    _pVals["Uab"] = 0f;
                    _pVals["Ubc"] = 0f;
                    _pVals["Uca"] = 0f;
                    _pVals["Udis"] = 0f;
                    _pVals["Riz"] = 0f;
                    _pVals["Cosf"] = 0f;
                    _pVals["EngLoad"] = 0f;
                    _pVals["P"] = 0f;
                    _pVals["S"] = 0f;
                    _pVals["Twork"] = 0f;
                    _pVals["Nstart"] = 0f;

                    _pVals["Online"] = 0;
                }

                if (response[pr - 1] == 0x20)
                {
                    temp[0] = response[9 + pr];
                    temp[1] = response[8 + pr];
                    _pVals["Ia"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString()) / 10;

                    temp[0] = response[11 + pr];
                    temp[1] = response[10 + pr];
                    _pVals["Ib"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString()) / 10;

                    temp[0] = response[13 + pr];
                    temp[1] = response[12 + pr];
                    _pVals["Ic"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString()) / 10;

                    temp[0] = response[15 + pr];
                    temp[1] = response[14 + pr];
                    _pVals["Idis"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString());

                    temp[0] = response[1 + pr];
                    temp[1] = response[0 + pr];
                    _pVals["Uab"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString());

                    temp[0] = response[3 + pr];
                    temp[1] = response[2 + pr];
                    _pVals["Ubc"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString());

                    temp[0] = response[5 + pr];
                    temp[1] = response[4 + pr];
                    _pVals["Uca"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString());

                    temp[0] = response[7 + pr];
                    temp[1] = response[6 + pr];
                    _pVals["Udis"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString());

                    temp[0] = response[17 + pr];
                    temp[1] = response[16 + pr];
                    _pVals["Riz"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString());

                    temp[0] = response[19 + pr];
                    temp[1] = response[18 + pr];
                    _pVals["Cosf"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString()) / 100;

                    temp[0] = response[21 + pr];
                    temp[1] = response[20 + pr];
                    _pVals["EngLoad"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString());

                    _pVals["P"] = 0f;

                    _pVals["S"] = 0f;

                    temp[0] = response[27 + pr];
                    temp[1] = response[26 + pr];
                    _pVals["Twork"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString());

                    temp[0] = response[29 + pr];
                    temp[1] = response[28 + pr];
                    _pVals["Nstart"] = float.Parse(BitConverter.ToUInt16(temp, 0).ToString());
                }
                #endregion
            }

            _pVals["Online"] = 1f;
        }
    }
}
