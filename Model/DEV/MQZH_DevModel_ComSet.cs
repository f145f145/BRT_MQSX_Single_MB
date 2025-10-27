/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQZHWL.Model.DEV
 * 文件名：  MQZH_DevModel_ComSet
 * 版本号：  V1.0.0.0
 * 唯一标识：a18beb1a-61e4-43fb-915d-7b1921379f3c
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022-5-25 17:43:24
 * 描述：
  * 装置Model，串口通讯部分
 * ==================================================================================
 * 修改标记
 * 修改时间				修改人			版本号			描述
 * 2022-5-25 17:43:24		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;

namespace MQZHWL.Model.DEV
{
    public partial class MQZH_DevModel_Main : ObservableObject
    {
        #region DVPCom

        /// <summary>
        /// PLC主机通讯串口设定
        /// </summary>
        private ComSetParaModel _dvpCom = new ComSetParaModel()
        {
            ComName = "DVPCom",
            PhyPortNO = "COM7",
            BoundRate = 9600,
            StartBits = 1,
            DataBits = 8,
            StopBits = 0,
            Parity = System.IO.Ports.Parity.Even,

            Addr = 2,
            PeriodRW = 400,
            WatchDogPeriod = 20000,
            Timeout = 200,
            Time_BusyDealy = 50,
            CMDRepeat = 1
        };

        /// <summary>
        /// PLC主机通讯串口设定
        /// </summary>
        public ComSetParaModel DVPCom
        {
            get { return _dvpCom; }
            set
            {
                _dvpCom = value;
                RaisePropertyChanged(() => DVPCom);
            }
        }

        /// <summary>
        /// PLC主机通讯状态
        /// </summary>
        private CommStatusModel _dvpComStatus = new CommStatusModel();
        /// <summary>
        /// PLC主机通讯状态
        /// </summary>
        public CommStatusModel DvpComStatus
        {
            get { return _dvpComStatus; }
            set
            {
                _dvpComStatus = value;
                RaisePropertyChanged(() => DvpComStatus);
            }
        }

        #endregion


        #region THPCom

        /// <summary>
        /// 三合一通讯串口设定
        /// </summary>
        private ComSetParaModel _thpCom = new ComSetParaModel()
        {
            ComName = "THPCom1",
            PhyPortNO = "COM1",
            BoundRate = 9600,
            StartBits = 1,
            DataBits = 8,
            StopBits = 0,
            Parity = System.IO.Ports.Parity.Even,

            Addr = 2,
            PeriodRW = 400,
            WatchDogPeriod = 20000,
            Timeout = 200,
            Time_BusyDealy = 50,
            CMDRepeat = 1
        };
        /// <summary>
        /// 三合一通讯串口设定
        /// </summary>
        public ComSetParaModel THPCom
        {
            get { return _thpCom; }
            set
            {
                _thpCom = value;
                RaisePropertyChanged(() => THPCom);
            }
        }

        /// <summary>
        /// 三合一通讯状态
        /// </summary>
        private CommStatusModel _thpComStatus = new CommStatusModel();
        /// <summary>
        /// 三合一通讯状态
        /// </summary>
        public CommStatusModel THPComStatus
        {
            get { return _thpComStatus; }
            set
            {
                _thpComStatus = value;
                RaisePropertyChanged(() => THPComStatus);
            }
        }

        #endregion


        #region UI选择用列表清单

        /// <summary>
        /// 获取串口列表
        /// </summary>
        public void GetComList()
        {
            SerialPortNames.Clear();
            try
            {
                Microsoft.Win32.RegistryKey hklm = Microsoft.Win32.Registry.LocalMachine;
                Microsoft.Win32.RegistryKey software11 = hklm.OpenSubKey("HARDWARE");
                //打开"HARDWARE"子健
                Microsoft.Win32.RegistryKey software = software11.OpenSubKey("DEVICEMAP");
                Microsoft.Win32.RegistryKey sitekey = software.OpenSubKey("SERIALCOMM");
                //获取当前子健
                string[] Str2;
                int ValueCount;
                if (sitekey == null)
                {
                    Str2 = new string[] { "" };
                    ValueCount = 0;
                }
                else
                {
                    Str2 = sitekey.GetValueNames();
                        //获得当前子健下面所有健组成的字符串数组
                    ValueCount = sitekey.ValueCount;
                    //获得当前子健存在的健值
                    int i;
                    for (i = 0; i < ValueCount; i++)
                    {
                        SerialPortNames.Add((string)sitekey.GetValue(Str2[i]));
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 计算机串口列表
        /// </summary>
        private ObservableCollection<string> _serialPortNames = new ObservableCollection<string>() { "COM1" };
        /// <summary>
        /// 计算机串口列表
        /// </summary>
        public ObservableCollection<string> SerialPortNames
        {
            get { return _serialPortNames; }
            set
            {
                _serialPortNames = value;
                RaisePropertyChanged(() => SerialPortNames);
            }
        }

        /// <summary>
        /// 波特率列表
        /// </summary>
        public ObservableCollection<int> _baudRateList = new ObservableCollection<int>() { 75, 110, 134, 150, 300, 600, 1200, 1800, 7200, 9600, 14400, 19200, 38400, 57600, 115200, 128000 };
        /// <summary>
        /// 波特率列表
        /// </summary>
        public ObservableCollection<int> BaudRateList
        {
            get { return _baudRateList; }
        }

        /// <summary>
        /// 数据位列表
        /// </summary>
        public ObservableCollection<int> _dataBitsList = new ObservableCollection<int>() { 4, 5, 6, 7, 8 };
        /// <summary>
        /// 数据位列表
        /// </summary>
        public ObservableCollection<int> DataBitsList
        {
            get { return _dataBitsList; }
        }

        /// <summary>
        /// 停止位列表
        /// </summary>
        public ObservableCollection<string> _stopBitsList = new ObservableCollection<string>() { "0", "1", "1.5", "2" };
        /// <summary>
        /// 停止位列表
        /// </summary>
        public ObservableCollection<string> StopBitsList
        {
            get { return _stopBitsList; }
        }

        /// <summary>
        /// 校验位列表
        /// </summary>
        public ObservableCollection<string> _parityList = new ObservableCollection<string>() { "奇", "偶", "无", "标志", "空格" };
        /// <summary>
        /// 计算机串口列表
        /// </summary>
        public ObservableCollection<string> ParityList
        {
            get { return _parityList; }
        }

        #endregion
        
    }
}