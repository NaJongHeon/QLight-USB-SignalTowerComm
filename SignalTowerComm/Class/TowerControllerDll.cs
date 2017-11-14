using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SignalTowerComm.Class
{
    class TowerControllerDll
    {
        [DllImport("Ux64_dllc.dll")]
        public static extern void Usb_Qu_Open();
        [DllImport("Ux64_dllc.dll")]
        public static extern void Usb_Qu_Close();
        [DllImport("Ux64_dllc.dll")]
        public static unsafe extern bool Usb_Qu_write(byte Q_index, byte Q_type, byte* pQ_data);
        [DllImport("Ux64_dllc.dll")]
        public static extern int Usb_Qu_Getstate();

        public TowerControllerDll()
        {
            Usb_Qu_Open();
        }

        /// <summary>
        /// 0. Index(Port) 1.BuzzerType(0-4) 2. Red 3. Yellow 4. Green 5. Blue 6. White 7. Buzzer(0-5)
        /// </summary>
        /// <param name="command">0: Off    1: On   2: Blink    Else:Don't Care </param>
        unsafe public bool sendCommandSignalTower(byte[] command)
        {
            bool bChk = false;
            byte* byteCommand = stackalloc byte[6];
            int cmdCount = 0;

            //1. Red 2. Yellow 3. Green 4. Blue 5. White 6. Buzzer
            for (int iLoofCount = 2; iLoofCount < 8; iLoofCount++)
            {
                byteCommand[cmdCount] = command[iLoofCount];
                cmdCount++;
            }

            bChk = Usb_Qu_write(command[0], command[1], byteCommand);

            return bChk;
            //if (bChk) //쓰기 성공
            //txt_Log.Text = "OK" + Environment.NewLine + txt_Log.Text;
            //else  //쓰기 실패
            //txt_Log.Text = "Fail" + Environment.NewLine + txt_Log.Text;
        }
        public bool[] readUsbConnectList()
        {
            int i = Usb_Qu_Getstate();
            bool[] isConnectedTower = new bool[4];

            if (i == 0x1) isConnectedTower[0] = true;
            else isConnectedTower[0] = false;
            if (i == 0x2) isConnectedTower[1] = true;
            else isConnectedTower[1] = false;
            if (i == 0x4) isConnectedTower[2] = true;
            else isConnectedTower[2] = false;
            if (i == 0x8) isConnectedTower[3] = true;
            else isConnectedTower[3] = false;

            return isConnectedTower;
        }
    }
}

