/************************************************************************************
 * 描述：
 * 通讯主程序。根据消息，执行数据读写。
 * ==================================================================================
 * 修改标记
 * 修改时间			                修改人			版本号			描述
 * 2021/12/9        16:42:46        郝正强			V1.0.0.0
 * 2021/12/12                       郝正强          串口每次读写时打开，更改为一次性打开，退出时关闭。
 * 2023/01/23       18:00:00		郝正强			V2.0.0.0    根据单风机修改
************************************************************************************/

using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Modbus.Device;
using MQDFJ_MB.Model;
using MQDFJ_MB.Model.DEV;
using static MQDFJ_MB.Model.MQZH_Enums;

namespace MQDFJ_MB.Communication
{
    public class MQZH_Communication : ObservableObject
    {
        public MQZH_Communication()
        {
            //订阅定时发送指令数据消息
            Messenger.Default.Register<ushort[]>(this, "CKCMDToComMessage", CKCMDToComMessage);

            //订阅三合一通信指令消息
            Messenger.Default.Register<string>(this, "THPComMessage", THPComMessage);
            //订阅单次发送指令消息
            Messenger.Default.Register<ushort[]>(this, "OnceOrderToComMessage", OnceOrderMessage);
            //订阅软件退出消息（关闭串口）
            Messenger.Default.Register<string>(this, "MQZH_Quit", AppQuitCMDMessage);
        }


        #region 通讯配置属性


        /// <summary>
        /// 公共数据
        /// </summary>
        private PublicDatas _publicData = PublicDatas.GetInstance();
        /// <summary>
        /// 公共数据
        /// </summary>
        public PublicDatas PublicData
        {
            get { return _publicData; }
            set
            {
                _publicData = value;
                RaisePropertyChanged(() => _publicData);
            }
        }

        /// <summary>
        /// 通讯用串行端口1
        /// </summary>
        private SerialPort _serialPort1 = new SerialPort("COM1");
        /// <summary>
        /// 通讯用串行端口1
        /// </summary>
        private SerialPort SerialPort1
        {
            get { return _serialPort1; }
            set
            {
                _serialPort1 = value;
                RaisePropertyChanged(() => SerialPort1);
            }
        }

        #endregion


        #region 通讯状态属性

        /// <summary>
        /// 串口通讯测试成功
        /// </summary>
        private bool _testOK = false;
        /// <summary>
        /// 串口通讯测试成功
        /// </summary>
        public bool TestOK
        {
            get { return _testOK; }
            set
            {
                _testOK = value;
                RaisePropertyChanged(() => TestOK);
            }
        }


        /// <summary>
        /// 串口通讯占用中状态标志
        /// </summary>
        private bool _isBusy = false;
        /// <summary>
        /// 串口通讯占用中状态标志
        /// </summary>
        private bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }


        /// <summary>
        /// 存在三合一读取指令
        /// </summary>
        private bool _existTHPOrder = false;
        /// <summary>
        /// 存在三合一读取指令
        /// </summary>
        private bool ExistTHPOrder
        {
            get { return _existTHPOrder; }
            set
            {
                _existTHPOrder = value;
                RaisePropertyChanged(() => ExistTHPOrder);
            }
        }


        /// <summary>
        /// 存在单次发送指令
        /// </summary>
        private bool _existOnceOrder = false;
        /// <summary>
        /// 存在单次发送指令
        /// </summary>
        private bool ExistOnceOrder
        {
            get { return _existOnceOrder; }
            set
            {
                _existOnceOrder = value;
                RaisePropertyChanged(() => ExistOnceOrder);
            }
        }

        /// <summary>
        /// 上次通讯读写模式（0为上次读参数，1为上次写下发指令）
        /// </summary>
        private bool _isWModeBefore = false;
        /// <summary>
        /// 上次通讯读写模式（1为读参数，0为下发指令）
        /// </summary>
        private bool IsWModeBefore
        {
            get { return _isWModeBefore; }
            set
            {
                _isWModeBefore = value;
                RaisePropertyChanged(() => IsWModeBefore);
            }
        }

        /// <summary>
        /// 退出软件指令
        /// </summary>
        private bool _appQuit = false;
        /// <summary>
        /// 退出软件指令
        /// </summary>
        private bool AppQuit
        {
            get { return _appQuit; }
            set
            {
                _appQuit = value;
                RaisePropertyChanged(() => AppQuit);
            }
        }

        #endregion


        #region 传输地址、数据属性

        /// <summary>
        /// PLC读取数据寄存器数量
        /// </summary>
        static readonly int DataLenNeedRead = 40;                                            //参数组长度D300-D339

        /// <summary>
        /// PLC程控指令数据长度
        /// </summary>
        static readonly int DataLenCKWrite = 20;                                            //参数组长度D8000-D8019

        /// <summary>
        /// PLC程控指令数组
        /// </summary>
        private ushort[] _plcCMDCK = Enumerable.Repeat((ushort)0, DataLenCKWrite).ToArray();
        /// <summary>
        /// PLC程控指令数组
        /// </summary>
        public ushort[] PLCCKCK
        {
            get { return _plcCMDCK; }
            set
            {
                _plcCMDCK = value;
                RaisePropertyChanged(() => PLCCKCK);
            }
        }

        //单次发送相关参数
        public static int _onceOrderStartAddr = (int)MQZH_Enums.CommuRegAddr.DCFS;        //单次发送目标地址40868（D8100）
        static readonly int _onceOrderDataLen = 24;                                             //单次发送数组长度

        /// <summary>
        /// 单次发送指令值数组
        /// </summary>
        private ushort[] _onceOrderUshortValues = Enumerable.Repeat((ushort)0, _onceOrderDataLen).ToArray();
        /// <summary>
        /// 单次发送指令值数组
        /// </summary>
        public ushort[] OnceOrderUshortValues
        {
            get { return _onceOrderUshortValues; }
            set
            {
                _onceOrderUshortValues = value;
                RaisePropertyChanged(() => OnceOrderUshortValues);
            }
        }


        /// <summary>
        /// 三合一读取指令总长度（最大）
        /// </summary>
        static int OrderLenTHP_R = 20;

        /// <summary>
        /// 三合一返回数据总长度（最大）
        /// </summary>
        static int DataLenTHPRet_R = 3;


        /// <summary>
        /// 三合一采集返回数值（ushort）
        /// </summary>
        private ushort[] _dataTHP = new ushort[DataLenTHPRet_R];
        /// <summary>
        /// 三合一采集返回数值（ushort）
        /// </summary>
        public ushort[] DataTHP
        {
            get { return _dataTHP; }
            set
            {
                _dataTHP = value;
                RaisePropertyChanged(() => DataTHP);
            }
        }


        #endregion


        #region 三合一读取

        /// <summary>
        /// 三合一通讯端口检测
        /// </summary>
        private bool TestTHPPort()
        {
            bool isSuccess = true;
            String[] Portname = SerialPort.GetPortNames();
            bool exist = false;
            foreach (string str in Portname)
            {
                if (str == PublicData.Dev.THPCom.PhyPortNO)
                    exist = true;
            }
            if (exist)
            {
                SerialPort1.PortName = PublicData.Dev.THPCom.PhyPortNO;
                SerialPort1.BaudRate = PublicData.Dev.THPCom.BoundRate;
                SerialPort1.DataBits = PublicData.Dev.THPCom.DataBits;
                SerialPort1.Parity = PublicData.Dev.THPCom.Parity;
                SerialPort1.StopBits = PublicData.Dev.THPCom.StopBits;
                SerialPort1.ReadTimeout = PublicData.Dev.THPCom.Timeout;
                try
                {
                    if (SerialPort1.IsOpen)
                        SerialPort1.Close();
                    SerialPort1.Open();
                    SerialPort1.Close();

                    switch (PublicData.Dev.THPType)
                    {
                        //赛诺三合一
                        case 1:
                            isSuccess = GetTHP1Data();
                            break;

                        //森诺中天物联三合一
                        case 2:
                            isSuccess = GetTHP2Data();
                            break;

                        //建大仁科
                        case 3:
                            isSuccess = GetTHP3Data();
                            break;

                        //旧型号赛通三合一
                        default:
                            ;
                            isSuccess = GetTHP1Data();
                            break;
                    }
                }
                catch (Exception e)
                {
                    isSuccess = false;
                    MessageBox.Show("三合一通讯" + e.Message);
                    Messenger.Default.Send<string>("三合一通讯异常" + e.Message, "ComError1");
                }
            }
            else
            {
                isSuccess = false;
                MessageBox.Show("三合一通讯" + PublicData.Dev.THPCom.PhyPortNO + "不可用！");
                Messenger.Default.Send<string>("三合一通讯端口不可用", "ComError1");
            }
            if (!isSuccess)
                PublicData.Dev.THPComStatus.IsFailure = true;
            return isSuccess;
        }

        /// <summary>
        /// 三合一通讯端口设定
        /// </summary>
        private void SetTHPPort()
        {
            SerialPort1.Close();
            SerialPort1.PortName = PublicData.Dev.THPCom.PhyPortNO;
            SerialPort1.BaudRate = PublicData.Dev.THPCom.BoundRate;
            SerialPort1.DataBits = PublicData.Dev.THPCom.DataBits;
            SerialPort1.Parity = PublicData.Dev.THPCom.Parity;
            SerialPort1.StopBits = PublicData.Dev.THPCom.StopBits;
            SerialPort1.ReadTimeout = PublicData.Dev.THPCom.Timeout;
        }


        /// <summary>
        /// 读取三合一数据（旧型号）
        /// </summary>
        private bool GetTHP1Data()
        {
            PublicData.Dev.THPComStatus.IsBusy = true;
            bool isRWSuccess = true;

            //读取温度、湿度
            ushort[] statusTransDataTH = new ushort[DataLenTHPRet_R - 1];                   //装置参数读取传输数组，温度、湿度
            try
            {
                SetTHPPort();
                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPort1);
                master.Transport.ReadTimeout = PublicData.Dev.THPCom.Timeout;
                //命令数据处理
                byte slaveId = Convert.ToByte(PublicData.Dev.THPCom.Addr);
                ushort startAddress = Convert.ToUInt16(THP1RegAddr.TH);
                ushort numRegisters = Convert.ToUInt16(THP1DataQty.TH);
                if (!SerialPort1.IsOpen)
                    SerialPort1.Open();
                statusTransDataTH = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);
            }
            catch (Exception e)
            {
                isRWSuccess = false;
                //   MessageBox.Show(e.Message);
                Messenger.Default.Send<string>("三合一模块通讯异常" + e.Message, "ComError1");
            }

            //读取大气压力
            ushort[] statusTransDataP = new ushort[1] { 0 };                   //压力读取数据
            try
            {
                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPort1);
                master.Transport.ReadTimeout = PublicData.Dev.THPCom.Timeout;
                //命令数据处理
                byte slaveId = Convert.ToByte(PublicData.Dev.THPCom.Addr);
                ushort startAddress = Convert.ToUInt16(THP1RegAddr.P);
                ushort numRegisters = Convert.ToUInt16(THP1DataQty.P);
                if (!SerialPort1.IsOpen)
                    SerialPort1.Open();
                statusTransDataP = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);
            }
            catch (Exception e)
            {
                isRWSuccess = false;
                //   MessageBox.Show(e.Message);
                Messenger.Default.Send<string>("三合一模块通讯异常" + e.Message, "ComError1");
            }

            //数据传送
            if (isRWSuccess)
            {
                //发送状态数据更新消息
                DataTHP = new ushort[statusTransDataTH.Length + 1];
                DataTHP[0] = statusTransDataTH[1];      //温度
                DataTHP[1] = statusTransDataTH[0];      //湿度
                DataTHP[2] = statusTransDataP[0];       //大气压力
                Messenger.Default.Send<ushort[]>(DataTHP, "THPDataUpdated");
                PublicData.Dev.THPComStatus.IsFailure = false;
            }
            else
                PublicData.Dev.THPComStatus.IsFailure = true;

            SerialPort1.Close();
            PublicData.Dev.THPComStatus.IsBusy = false;
            //   PublicData.Dev.NeedTHP = false;

            return isRWSuccess;
        }

        /// <summary>
        /// 读取三合一数据（森诺中天物联）
        /// </summary>
        private bool GetTHP2Data()
        {
            PublicData.Dev.THPComStatus.IsBusy = true;
            bool isRWSuccess = true;

            //读取温度、湿度
            ushort[] statusTransDataTH = new ushort[DataLenTHPRet_R - 1];                   //装置参数读取传输数组，温度、湿度
            try
            {
                SetTHPPort();
                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPort1);
                master.Transport.ReadTimeout = PublicData.Dev.THPCom.Timeout;
                //命令数据处理
                byte slaveId = Convert.ToByte(PublicData.Dev.THPCom.Addr);
                ushort startAddress = Convert.ToUInt16(THP2RegAddr.TH);
                ushort numRegisters = Convert.ToUInt16(THP2DataQty.TH);
                if (!SerialPort1.IsOpen)
                    SerialPort1.Open();
                statusTransDataTH = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);
            }
            catch (Exception e)
            {
                isRWSuccess = false;
                //   MessageBox.Show(e.Message);
                Messenger.Default.Send<string>("三合一模块通讯异常" + e.Message, "ComError1");
            }

            //读取大气压力
            ushort[] statusTransDataP = new ushort[1] { 0 };                   //压力读取数据
            try
            {
                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPort1);
                master.Transport.ReadTimeout = PublicData.Dev.THPCom.Timeout;
                //命令数据处理
                byte slaveId = Convert.ToByte(PublicData.Dev.THPCom.Addr);
                ushort startAddress = Convert.ToUInt16(THP2RegAddr.P);
                ushort numRegisters = Convert.ToUInt16(THP2DataQty.P);
                if (!SerialPort1.IsOpen)
                    SerialPort1.Open();
                statusTransDataP = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);
            }
            catch (Exception e)
            {
                isRWSuccess = false;
                //   MessageBox.Show(e.Message);
                Messenger.Default.Send<string>("三合一模块通讯异常" + e.Message, "ComError1");
            }

            //数据传送
            if (isRWSuccess)
            {
                //发送状态数据更新消息
                DataTHP = new ushort[statusTransDataTH.Length + 1];
                DataTHP[0] = statusTransDataTH[0];      //温度
                DataTHP[1] = statusTransDataTH[1];      //湿度
                DataTHP[2] = statusTransDataP[0];       //大气压力
                Messenger.Default.Send<ushort[]>(DataTHP, "THPDatasUpdated");
                PublicData.Dev.THPComStatus.IsFailure = false;
            }
            else
                PublicData.Dev.THPComStatus.IsFailure = true;

            SerialPort1.Close();
            PublicData.Dev.THPComStatus.IsBusy = false;
            //   PublicData.Dev.NeedTHP = false;

            return isRWSuccess;
        }

        /// <summary>
        /// 读取三合一数据（建大仁科）
        /// </summary>
        private bool GetTHP3Data()
        {
            PublicData.Dev.THPComStatus.IsBusy = true;
            bool isRWSuccess = true;

            //读取温度、湿度
            ushort[] statusTransDataTH = new ushort[DataLenTHPRet_R - 1];                   //装置参数读取传输数组，温度、湿度
            try
            {
                SetTHPPort();
                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPort1);
                master.Transport.ReadTimeout = PublicData.Dev.THPCom.Timeout;
                //命令数据处理
                byte slaveId = Convert.ToByte(PublicData.Dev.THPCom.Addr);
                ushort startAddress = Convert.ToUInt16(THP3RegAddr.TH);
                ushort numRegisters = Convert.ToUInt16(THP3DataQty.TH);
                if (!SerialPort1.IsOpen)
                    SerialPort1.Open();
                statusTransDataTH = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);
            }
            catch (Exception e)
            {
                isRWSuccess = false;
                //   MessageBox.Show(e.Message);
                Messenger.Default.Send<string>("三合一模块通讯异常" + e.Message, "ComError1");
            }

            //读取大气压力
            ushort[] statusTransDataP = new ushort[1] { 0 };                   //压力读取数据
            try
            {
                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPort1);
                master.Transport.ReadTimeout = PublicData.Dev.THPCom.Timeout;
                //命令数据处理
                byte slaveId = Convert.ToByte(PublicData.Dev.THPCom.Addr);
                ushort startAddress = Convert.ToUInt16(THP3RegAddr.P);
                ushort numRegisters = Convert.ToUInt16(THP3DataQty.P);
                if (!SerialPort1.IsOpen)
                    SerialPort1.Open();
                statusTransDataP = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);
            }
            catch (Exception e)
            {
                isRWSuccess = false;
                //   MessageBox.Show(e.Message);
                Messenger.Default.Send<string>("三合一模块通讯异常" + e.Message, "ComError1");
            }

            //数据传送
            if (isRWSuccess)
            {
                //发送状态数据更新消息
                DataTHP = new ushort[statusTransDataTH.Length + 1];
                DataTHP[0] = statusTransDataTH[1];      //温度
                DataTHP[1] = statusTransDataTH[0];      //湿度
                DataTHP[2] = statusTransDataP[0];       //大气压力
                Messenger.Default.Send<ushort[]>(DataTHP, "THPDataUpdated");
                PublicData.Dev.THPComStatus.IsFailure = false;
            }
            else
                PublicData.Dev.THPComStatus.IsFailure = true;

            SerialPort1.Close();
            PublicData.Dev.THPComStatus.IsBusy = false;
            //   PublicData.Dev.NeedTHP = false;

            return isRWSuccess;
        }
        #endregion
       
        
        #region 通讯收发定时器及回调

        /// <summary>
        ///  串口读写定时器定时器
        /// </summary>
        DispatcherTimer ComRW_Timer = new DispatcherTimer();

        /// <summary>
        /// 串口读写定时器回调函数。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComRW_Timer_Tick(object sender, EventArgs e)
        {
            bool isRWSuccess = true;
            ushort[] statusTransData = new ushort[DataLenNeedRead];     //装置参数读取传输数组

            lock (this)
            {
                if (IsBusy)
                    return;
                IsBusy = true;
            }

            try
            {
                //软件退出指令
                if (AppQuit)
                {
                    try
                    {
                        if (!SerialPort1.IsOpen)
                            SerialPort1.Open();
                        IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPort1);
                        master.Transport.ReadTimeout = PublicData.Dev.DVPCom.Timeout;

                        PLCCKCK = Enumerable.Repeat((ushort)0, DataLenCKWrite).ToArray();
                        PLCCKCK[0] = 1;  //PLC为程控模式
                        if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[14].IsOn)   //风阀无故障且自检完成时
                            PLCCKCK[5] = 5;  //换向阀为正压模式
                        byte slaveId = Convert.ToByte(PublicData.Dev.DVPCom.Addr);
                        ushort startAddress = Convert.ToUInt16(CommuRegAddr.CKCMD); //定时指令发送至8000
                        ushort[] registerValues = PLCCKCK;
                        master.WriteMultipleRegisters(slaveId, startAddress, registerValues);

                        SerialPort1.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }


                //读三合一数据
                if (ExistTHPOrder&&PublicData.Dev.IsUseTHP)
                {
                    switch (PublicData.Dev.THPType)
                    {
                        //赛通
                        case 1:
                            GetTHP1Data();
                            break;

                        //森诺中天物联三合一
                        case 2:
                            GetTHP2Data();
                            break;

                        //建大仁科
                        case 3:
                            GetTHP3Data();
                            break;

                        //旧型号赛通三合一
                        default:
                            GetTHP1Data();
                            break;
                    }
                    SerialPort1.Close();
                    ExistTHPOrder = false;
                }


                //单次收发
                if (ExistOnceOrder)
                {
                    //创建RTU连接并发送数据
                    try
                    {
                        IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPort1);
                        master.Transport.ReadTimeout = PublicData.Dev.DVPCom.Timeout;
                        master.Transport.WriteTimeout = PublicData.Dev.DVPCom.Timeout;
                        master.Transport.Retries = 0;
                        master.Transport.RetryOnOldResponseThreshold = 0;
                        master.Transport.SlaveBusyUsesRetryCount = false;
                        master.Transport.WaitToRetryMilliseconds = 0;

                        //命令数据处理
                        byte slaveId = Convert.ToByte(PublicData.Dev.DVPCom.Addr);
                        ushort startAddress = Convert.ToUInt16(_onceOrderStartAddr); //单次指令发送至8100
                        ushort[] registerValues = OnceOrderUshortValues;
                        master.WriteMultipleRegisters(slaveId, startAddress, registerValues);

                        // Trace.Write("0:"+ registerValues[0]+",3:"+registerValues[3] + ",12:"+registerValues[12] + ",15:" + registerValues[15]+"\r\n");
                    }
                    catch (Exception)
                    {
                        isRWSuccess = false;
                        Messenger.Default.Send<string>("发送单次指令异常！", "ComError1");
                    }

                    //状态复位
                    if (isRWSuccess)
                    {
                        ExistOnceOrder = false;
                        PublicData.Dev.DvpComStatus.IsFailure = false;
                    }
                    else
                        PublicData.Dev.DvpComStatus.IsFailure = true;
                }

                //上次写，此次读
                else if (IsWModeBefore)
                {
                    try
                    {
                        IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPort1);
                        master.Transport.ReadTimeout = PublicData.Dev.DVPCom.Timeout;
                        master.Transport.WriteTimeout = PublicData.Dev.DVPCom.Timeout;
                        master.Transport.Retries = 0;
                        master.Transport.RetryOnOldResponseThreshold = 0;
                        master.Transport.SlaveBusyUsesRetryCount = false;
                        master.Transport.WaitToRetryMilliseconds = 0;

                        //命令准备
                        byte slaveId = Convert.ToByte(PublicData.Dev.DVPCom.Addr);
                        ushort startAddress = Convert.ToUInt16(CommuRegAddr.YXZT);
                        ushort numRegisters = Convert.ToUInt16(DataLenNeedRead);
                        statusTransData = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);

                        //  Trace.Write(statusTransData[36]  + "\r\n");

                    }
                    catch (Exception ex)
                    {
                        isRWSuccess = false;
                        Messenger.Default.Send<string>("定时读数据异常！", "ComError1");
                    }

                    //解除通讯占用
                    IsBusy = false;

                    //数据解析计算
                    if (isRWSuccess)
                    {
                        //发送状态数据更新消息
                        Messenger.Default.Send<ushort[]>(statusTransData, "PLCDataUpdated");
                        PublicData.Dev.DvpComStatus.IsFailure = false;
                    }
                    else
                        PublicData.Dev.DvpComStatus.IsFailure = true;

                    //修改上次读写状态为“上次读”
                    IsWModeBefore = false;
                }

                //上次读参数，这次写指令
                else
                {
                    //发送程控指令00工作模式，01启停指令1，02启停指令2，03风机频率给定，04水泵频率给定，05阀门模式，6、7低压准备频率，
                    //8、9低压准备脉冲个数，10设定压力，11压力通道，12、13波动脉冲频率，14、15波动脉冲修正，16波动次数
                    try
                    {
                        IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPort1);
                        master.Transport.ReadTimeout = PublicData.Dev.DVPCom.Timeout;
                        master.Transport.WriteTimeout = PublicData.Dev.DVPCom.Timeout;
                        master.Transport.Retries = 0;
                        master.Transport.RetryOnOldResponseThreshold = 0;
                        master.Transport.SlaveBusyUsesRetryCount = false;
                        master.Transport.WaitToRetryMilliseconds = 0;

                        //命令准备
                        byte slaveId = Convert.ToByte(PublicData.Dev.DVPCom.Addr);
                        ushort startAddress = Convert.ToUInt16(CommuRegAddr.CKCMD); //定时指令发送至8000
                        ushort[] registerValues = PLCCKCK;

                      //  Trace.Write(registerValues[0] + ",  " + registerValues[1] + ",  " + registerValues[2] + ",  " + registerValues[3] + "\r\n");
                        master.WriteMultipleRegisters(slaveId, startAddress, registerValues);
                        PublicData.Dev.DvpComStatus.IsFailure = false;
                    }
                    catch (Exception)
                    {
                        Messenger.Default.Send<string>("定时写数据异常！", "ComError1");
                        PublicData.Dev.DvpComStatus.IsFailure = true;
                    }

                    //修改上次读写状态为“上次写”
                    IsWModeBefore = true;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
            finally
            {
                //解除通讯占用
                IsBusy = false;
            }
        }

        #endregion


        #region 消息处理

        /// <summary>
        /// 程控指令消息
        /// </summary>
        /// <param name="msg"></param>
        private void CKCMDToComMessage(ushort[] msg)
        {
            try
            {
                PLCCKCK = (ushort[])msg.Clone();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// 根据消息指令三合一通信数据
        /// </summary>
        /// <param name="msg"></param>
        private void THPComMessage(string msg)
        {
            try
            {
                ExistTHPOrder = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// 根据消息指令单次收发数据
        /// </summary>
        /// <param name="msg"></param>
        private void OnceOrderMessage(ushort[] msg)
        {
            try
            {
                OnceOrderUshortValues = msg;
                ExistOnceOrder = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        /// <summary>
        /// 软件退出消息处理
        /// </summary>
        /// <param name="msg"></param>
        public void AppQuitCMDMessage(string msg)
        {
            try
            {
                if (msg == "Quit")
                    AppQuit = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        #endregion


        #region 通用方法

        /// <summary>
        /// 通讯模块初始化
        /// </summary>
        public void CommunicationInit()
        {
            try
            {
                SerialPortInit();                           //串口初始化
                TestOK = CommunicationTest();
                //读写定时器初始化
                if (TestOK)
                {
                    ComRW_Timer.Tick += new EventHandler(ComRW_Timer_Tick);
                    ComRW_Timer.Interval = TimeSpan.FromTicks(PublicData.Dev.DVPCom.PeriodRW);
                    ComRW_Timer.Start();
                }
                else
                {
                    if (SerialPort1.IsOpen)
                        SerialPort1.Close();
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// 串口初始化
        /// </summary>
        public void SerialPortInit()
        {
            try
            {
                IsBusy = false;
                //创建串口并配置参数
                SerialPort1 = new SerialPort(PublicData.Dev.DVPCom.PhyPortNO)
                {
                    BaudRate = PublicData.Dev.DVPCom.BoundRate,
                    DataBits = PublicData.Dev.DVPCom.DataBits,
                    Parity = PublicData.Dev.DVPCom.Parity,
                    StopBits = PublicData.Dev.DVPCom.StopBits
                };
                //打开端口、创建RTU连接
                try
                {
                    if (SerialPort1.IsOpen)
                        SerialPort1.Close();
                    SerialPort1.Open();
                }
                catch
                {
                    Messenger.Default.Send<string>("串口初始化异常！", "ComError1");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// 通讯测试
        /// </summary>
        /// <returns></returns>
        private bool CommunicationTest()
        {            
            try
            {
                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPort1);
                master.Transport.ReadTimeout = PublicData.Dev.DVPCom.Timeout;
                master.Transport.WriteTimeout = PublicData.Dev.DVPCom.Timeout;
                master.Transport.Retries = 0;
                master.Transport.RetryOnOldResponseThreshold = 0;
                master.Transport.SlaveBusyUsesRetryCount = false;
                master.Transport.WaitToRetryMilliseconds = 0;

                //命令准备
                byte slaveId = Convert.ToByte(PublicData.Dev.DVPCom.Addr);
                ushort startAddress = Convert.ToUInt16(CommuRegAddr.YXZT);
                ushort numRegisters = Convert.ToUInt16(DataLenNeedRead);
                master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);

                return true;
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<string>("串口测试异常，通讯未启动！", "ComError1");
                return false;
            }

        }

        #endregion
    }
}
