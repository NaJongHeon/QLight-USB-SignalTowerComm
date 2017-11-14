using GalaSoft.MvvmLight.Command;
using SignalTowerComm.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SignalTowerComm.ViewModel
{
    public class TowerControllerViewModel : CustomViewModelBase
    {
        Class.TowerControllerDll cTowerControllerDll;

        #region 프로퍼티
        public string StrCommandLine {
            get { return _strCommandLine; }
            set { _strCommandLine = value; RaisePropertyChanged("StrCommandLine");  } }
        private string _strCommandLine="";

        //버튼 이벤트 류
        public string StrRedCommand
        {
            get { return _strRedCommand; }
            set { _strRedCommand = value; RaisePropertyChanged("StrRedCommand"); }
        }
        private string _strRedCommand = "Red ON";

        public bool BRedOnOff
        {
            get { return _bRedOnOff; }
            set { if (BRedOnOff) { StrRedCommand = "Red ON"; } else { StrRedCommand = "Red OFF"; } _bRedOnOff = value;  RaisePropertyChanged("BRedOnOff"); }
        }
        public bool _bRedOnOff = false;

        public string StrYellowCommand
        {
            get { return _strYellowCommand; }
            set { _strYellowCommand = value; RaisePropertyChanged("StrYellowCommand"); }
        }
        private string _strYellowCommand = "Yellow ON";

        public bool BYellowOnOff
        {
            get { return _bYellowOnOff; }
            set { if (_bYellowOnOff) { StrYellowCommand = "Yellow ON"; } else { StrYellowCommand = "Yellow OFF"; } _bYellowOnOff = value; RaisePropertyChanged("BYellowOnOff"); }
        }
        public bool _bYellowOnOff = false;

        public string StrGreenCommand
        {
            get { return _strGreenCommand; }
            set { _strGreenCommand = value; RaisePropertyChanged("StrGreenCommand"); }
        }
        private string _strGreenCommand = "Green ON";

        public bool BGreenOnOff
        {
            get { return _bGreenOnOff; }
            set { if (_bGreenOnOff) { StrGreenCommand = "Green ON"; } else { StrGreenCommand = "Green OFF"; } _bGreenOnOff = value; RaisePropertyChanged("BGreenOnOff"); }
        }
        public bool _bGreenOnOff = false;

        public string StrBuzzerCommand
        {
            get { return _strBuzzerCommand; }
            set { _strBuzzerCommand = value; RaisePropertyChanged("StrBuzzerCommand"); }
        }
        private string _strBuzzerCommand = "Buzzer ON";

        public bool BBuzzerOnOff
        {
            get { return _bBuzzerOnOff; }
            set { if (_bBuzzerOnOff) { StrBuzzerCommand = "Buzzer ON"; } else { StrBuzzerCommand = "Buzzer OFF"; } _bBuzzerOnOff = value; RaisePropertyChanged("BBuzzerOnOff"); }
        }
        public bool _bBuzzerOnOff = false;

        public string StrLogLine
        {
            get { return _strLogLine; }
            set { _strLogLine = value + Environment.NewLine + _strLogLine; RaisePropertyChanged("StrLogLine"); }
        }
        private string _strLogLine = "";
        #endregion

        #region 커맨드
        public RelayCommand<object> SendCommand { get; private set; }
        public RelayCommand<object> ResetCommand { get; private set; }
        public RelayCommand<object> RedCommand { get; private set; }
        public RelayCommand<object> YellowCommand { get; private set; }
        public RelayCommand<object> GreenCommand { get; private set; }
        public RelayCommand<object> BuzzerCommand { get; private set; }
        #endregion

        #region 초기화
        public TowerControllerViewModel()
        {
            InitData();
            InitCommand();
        }

        private void InitData()
        {
            cTowerControllerDll = new Class.TowerControllerDll();
            cTowerControllerDll.readUsbConnectList();
        }

        private void InitCommand()
        {
            SendCommand = new RelayCommand<object>((param) => OnSendCommand(param));
            ResetCommand = new RelayCommand<object>((param) => OnResetCommand(param));
            RedCommand = new RelayCommand<object>((param) => OnRedCommand(param));
            YellowCommand = new RelayCommand<object>((param) => OnYellowCommand(param));
            GreenCommand = new RelayCommand<object>((param) => OnGreenCommand(param));
            BuzzerCommand = new RelayCommand<object>((param) => OnBuzzerCommand(param));
        }



        #endregion

        #region 이벤트
        private void OnSendCommand(object param)
        {
            byte[] bCommandArr = new byte[8];

            if (StrCommandLine.Length != 8)
            {
                MessageBox.Show("8개로 구성된 커맨드가 필요합니다");
            }
            else
            {
                for (int iLoofCount = 0; iLoofCount < 8; iLoofCount++)
                {
                    bCommandArr[iLoofCount] = byte.Parse(StrCommandLine.Substring(iLoofCount, 1));
                }

                if (cTowerControllerDll.sendCommandSignalTower(bCommandArr))
                {
                    StrLogLine = "SendCommand OK";
                }
                else
                {
                    StrLogLine = "SendCommand Fail";
                }
            }
        }

        private void OnResetCommand(object param)
        {
            byte[] bCommandArr = new byte[8];

            for (int iLoofCount = 0; iLoofCount < 8; iLoofCount++)
            {
                bCommandArr[iLoofCount] = byte.Parse("0");
            }

            if (cTowerControllerDll.sendCommandSignalTower(bCommandArr))
            {
                StrLogLine = "SendReset OK";
                if (BRedOnOff) { BRedOnOff = false; } else {  }
                if (BYellowOnOff) { BYellowOnOff = false; } else { }
                if (BGreenOnOff) { BGreenOnOff = false; } else { }
                if (BBuzzerOnOff) { BBuzzerOnOff = false; } else { }
            }
            else
            {
                StrLogLine = "SendReset Fail";
            }
        }

        private void OnRedCommand(object param)
        {
            byte[] bCommandArr = new byte[8];

            for (int iLoofCount = 0; iLoofCount < 8; iLoofCount++)
            {
                bCommandArr[iLoofCount] = byte.Parse("9");
            }

            bCommandArr[0] = bCommandArr[1] = 0;

            if(BRedOnOff)
            {
                bCommandArr[2] = byte.Parse("0");
                BRedOnOff = false;
            }
            else
            {
                bCommandArr[2] = byte.Parse("1");
                BRedOnOff = true;
            }
            

            if (cTowerControllerDll.sendCommandSignalTower(bCommandArr))
            {
                StrLogLine = "Red Toggle OK";
                
            }
            else
            {
                StrLogLine = "Send Fail";
            }
        }

        private void OnYellowCommand(object param)
        {
            byte[] bCommandArr = new byte[8];

            for (int iLoofCount = 0; iLoofCount < 8; iLoofCount++)
            {
                bCommandArr[iLoofCount] = byte.Parse("9");
            }

            bCommandArr[0] = bCommandArr[1] = 0;

            if (BYellowOnOff)
            {
                bCommandArr[3] = byte.Parse("0");
                BYellowOnOff = false;
            }
            else
            {
                bCommandArr[3] = byte.Parse("1");
                BYellowOnOff = true;
            }


            if (cTowerControllerDll.sendCommandSignalTower(bCommandArr))
            {
                StrLogLine = "Yellow Toggle OK";

            }
            else
            {
                StrLogLine = "Send Fail";
            }
        }

        private void OnGreenCommand(object param)
        {
            byte[] bCommandArr = new byte[8];

            for (int iLoofCount = 0; iLoofCount < 8; iLoofCount++)
            {
                bCommandArr[iLoofCount] = byte.Parse("9");
            }

            bCommandArr[0] = bCommandArr[1] = 0;

            if (BGreenOnOff)
            {
                bCommandArr[4] = byte.Parse("0");
                BGreenOnOff = false;
            }
            else
            {
                bCommandArr[4] = byte.Parse("1");
                BGreenOnOff = true;
            }


            if (cTowerControllerDll.sendCommandSignalTower(bCommandArr))
            {
                StrLogLine = "Green Toggle OK";

            }
            else
            {
                StrLogLine = "Send Fail";
            }
        }

        private void OnBuzzerCommand(object param)
        {
            byte[] bCommandArr = new byte[8];

            for (int iLoofCount = 0; iLoofCount < 8; iLoofCount++)
            {
                bCommandArr[iLoofCount] = byte.Parse("9");
            }

            bCommandArr[0] = bCommandArr[1] = 0;

            if (BBuzzerOnOff)
            {
                bCommandArr[7] = byte.Parse("0");
                BBuzzerOnOff = false;
            }
            else
            {
                bCommandArr[7] = byte.Parse("1");
                BBuzzerOnOff = true;
            }


            if (cTowerControllerDll.sendCommandSignalTower(bCommandArr))
            {
                StrLogLine = "Buzzer Toggle OK";

            }
            else
            {
                StrLogLine = "Send Fail";
            }
        }
        #endregion
    }
}
