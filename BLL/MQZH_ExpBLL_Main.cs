/************************************************************************************
 * 描述：
 * 幕墙四性试验操作业务BLL
 * ==================================================================================
 * 修改标记
 * 修改时间			                修改人			版本号			描述
 * 2024/01/29       19:02:11		郝正强			V1.0.0.0
 * 
 ************************************************************************************/

using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using MQDFJ_MB.Model.DEV;
using MQDFJ_MB.Model.Exp;
using System.Linq;
using System.Windows;
using static MQDFJ_MB.Model.MQZH_Enums;
using GalaSoft.MvvmLight.Messaging;
using MQDFJ_MB.Model;
using CtrlMethod;
using System.Windows.Threading;

namespace MQDFJ_MB.BLL
{
    /// <summary>
    /// 主控类
    /// </summary>
    public partial class MQZH_ExpBLL : ObservableObject
    {
        /// <summary>
        /// 主控类初始化
        /// </summary>
        public MQZH_ExpBLL()
        {
            //通讯、消息用数据
            PublicData.Dev.DeviceRunMode = DevicRunModeType.Wait_Mode;
            OnceOrderValuesFrmBLL = Enumerable.Repeat((ushort)0, _onceOrderDataLenFrmBLL).ToArray();

            //BLL主定时器、手动气密计算定时器、气密计算定时器
            BLL_TimerInit();

            //各状态初始化
            EscStopCompleteExpReset();

            //注册窗口关闭、新增消息
            Messenger.Default.Register<string>(this, "WindowClosed", WindowClosedMessage);
            Messenger.Default.Register<Window>(this, "NewWindow", WindowAddedMessage);

            //注册紧急停机消息
            Messenger.Default.Register<string>(this, "JJTJMessage", JJTJMessage);

            //注册停止试验消息
            Messenger.Default.Register<int>(this, "StopExpMessage", ExpStopMessage);

            //注册模式切换消息
            Messenger.Default.Register<DevicRunModeType>(this, "ModeChangeMessage", ModeChangeMessage);

            //手动单点调试消息
            Messenger.Default.Register<int>(this, "SDDDMessage", SDDDMessage);
            //手动调速消息
            Messenger.Default.Register<ushort[]>(this, "SDTSMessage", SDTSMessage);
            //手动气密PID调试消息
            Messenger.Default.Register<double[]>(this, "SDQMMessage", SDQMMessage);
            //手动水密PID调试消息
            Messenger.Default.Register<double[]>(this, "SDSMMessage", SDSMMessage);
            //手动水流量PID调试消息
            Messenger.Default.Register<double[]>(this, "SLLPIDMessage", SLLPIDMessage);
            //手动位移调试消息
            Messenger.Default.Register<ushort[]>(this, "SDWYMessage", DbgSDWYMessage);

            //注册气密、水密试验控制消息
            Messenger.Default.Register<int>(this, "QMCtrlMessage", QMCtrlMessage);
            Messenger.Default.Register<int>(this, "SMCtrlMessage", SMCtrlMessage);
            Messenger.Default.Register<int>(this, "KFYp1CtrlMessage", KFYp1CtrlMessage);
            Messenger.Default.Register<int>(this, "KFYp2CtrlMessage", KFYp2CtrlMessage);
            Messenger.Default.Register<int>(this, "KFYp3CtrlMessage", KFYp3CtrlMessage);
            Messenger.Default.Register<int>(this, "KFYpmaxCtrlMessage", KFYpmaxCtrlMessage);
            //注册层间变形试验控制消息
            Messenger.Default.Register<int>(this, "CJBXCtrlMessage", CJBXCtrlMessage);
        }


        //====核心主体程序==========================================================
        #region 控制主业务逻辑定时器及回调函数

        /// <summary>
        /// 幕墙四性控制业务定时器
        /// </summary>
        private System.Threading.Timer BLL_Timer;
        /// <summary>
        /// 幕墙四性控制业务定时器初始化。
        /// </summary>
        private void BLL_TimerInit()
        {
            BLL_Timer = new System.Threading.Timer(new System.Threading.TimerCallback(BLL_Timer_Tick), this, 0, 100);
        }

        /// <summary>
        /// 业务控制逻辑定时器回调函数（给通讯模块传递将被定时发送的指令数据）。
        /// </summary>
        /// <remarks> 是否需要试验等，在消息参数里处理。</remarks>
        /// <param name="state"></param>
        private void BLL_Timer_Tick(object state)
        {
            //检查进程锁标志
            lock (this)
            {
                if (TimerThreadLock)
                    return;
                TimerThreadLock = true;
            }

            try
            {
                switch (PublicData.Dev.DeviceRunMode)
                {
                    //紧急停机
                    case DevicRunModeType.JJTJ_Mode:
                        {
                            JJTJFunc();
                            break;
                        }

                    //待机模式
                    case DevicRunModeType.Wait_Mode:
                        {
                            WaitFunc();
                            break;
                        }
                    //各种调试模式=========================================================
                    //手动单点模式
                    case DevicRunModeType.SDDDDbg_Mode:
                        {
                            SDDDFunc();
                            break;
                        }

                    //手动正压调速模式
                    case DevicRunModeType.SDZTSDbg_Mode:
                        {
                            SDZTSFunc();
                            break;
                        }

                    //手动负压调速事务
                    case DevicRunModeType.SDFTSDbg_Mode:
                        {
                            SDFTSFunc();
                            break;
                        }

                    //手动水泵调速事务
                    case DevicRunModeType.SDSBTSDbg_Mode:
                        {
                            SDSBTSFunc();
                            break;
                        }

                    //手动气密蝶阀1正压模式
                    case DevicRunModeType.DF1QMZDbg_Mode:
                        {
                            PublicData.Dev.IsDeviceBusy = true;

                            PublicData.Dev.DOList[0].IsOn = true;   //开主管蝶阀1
                            PublicData.Dev.DOList[1].IsOn = false;   //关粗管蝶阀2
                            PublicData.Dev.DOList[2].IsOn = false;   //关细管蝶阀3
                            PublicData.Dev.DOList[3].IsOn = false;   //关加粗管蝶阀4
                            PublicData.Dev.DOList[4].IsOn = true;   //开差压电磁阀
                            PublicData.Dev.DOList[5].IsOn = true;   //开KM1
                            PublicData.Dev.DOList[7].IsOn = true;   //开气泵
                            PublicData.Dev.DOList[8].IsOn = true;   //开风机变频

                            bool[] order1 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] order2 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] tempArry = Enumerable.Repeat(false, PublicData.Dev.DOList.Count).ToArray();
                            for (int i = 0; i < PublicData.Dev.DOList.Count; i++)
                                tempArry[i] = PublicData.Dev.DOList[i].IsOn;
                            Array.Copy(tempArry, 0, order1, 0, 16);
                            Array.Copy(tempArry, 16, order2, 0, 2);

                            PLCCKCMDFrmBLL[0] = 1;  //PLC为程控模式
                            PLCCKCMDFrmBLL[1] = MQZH_ValueCvtBLL.Bools2Ushort(order1);
                            PLCCKCMDFrmBLL[2] = MQZH_ValueCvtBLL.Bools2Ushort(order2);
                            PLCCKCMDFrmBLL[19] = 1;     //看门狗
                            if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                                PLCCKCMDFrmBLL[5] = 5;  //换向阀为正压模式

                            PLCCKCMDFrmBLL[3] = PublicData.Dev.Valve.DIList[5].IsOn ? SDQMFunc() : (ushort)0;   //正压模式完成后输出计算频率
                            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
                            break;
                        }
                    //手动气密蝶阀1负压模式
                    case DevicRunModeType.DF1QMFDbg_Mode:
                        {
                            PublicData.Dev.IsDeviceBusy = true;

                            PublicData.Dev.DOList[0].IsOn = true;   //开主管蝶阀1
                            PublicData.Dev.DOList[1].IsOn = false;   //关粗管蝶阀2
                            PublicData.Dev.DOList[2].IsOn = false;   //关细管蝶阀3
                            PublicData.Dev.DOList[3].IsOn = false;   //关加粗管蝶阀4
                            PublicData.Dev.DOList[4].IsOn = true;   //开差压电磁阀
                            PublicData.Dev.DOList[5].IsOn = true;   //开KM1
                            PublicData.Dev.DOList[7].IsOn = true;   //开气泵
                            PublicData.Dev.DOList[8].IsOn = true;   //开风机变频

                            bool[] order1 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] order2 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] tempArry = Enumerable.Repeat(false, PublicData.Dev.DOList.Count).ToArray();
                            for (int i = 0; i < PublicData.Dev.DOList.Count; i++)
                                tempArry[i] = PublicData.Dev.DOList[i].IsOn;
                            Array.Copy(tempArry, 0, order1, 0, 16);
                            Array.Copy(tempArry, 16, order2, 0, 2);

                            PLCCKCMDFrmBLL[0] = 1;  //PLC为程控模式
                            PLCCKCMDFrmBLL[1] = MQZH_ValueCvtBLL.Bools2Ushort(order1);
                            PLCCKCMDFrmBLL[2] = MQZH_ValueCvtBLL.Bools2Ushort(order2);
                            PLCCKCMDFrmBLL[19] = 1;     //看门狗
                            if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                                PLCCKCMDFrmBLL[5] = 6;  //换向阀为负压模式

                            PLCCKCMDFrmBLL[3] = PublicData.Dev.Valve.DIList[6].IsOn ? SDQMFunc() : (ushort)0;   //负压模式完成后输出计算频率
                            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
                            break;
                        }

                    //手动气密蝶阀2正压模式
                    case DevicRunModeType.DF2QMZDbg_Mode:
                        {
                            PublicData.Dev.IsDeviceBusy = true;

                            PublicData.Dev.DOList[0].IsOn = false;   //关主管蝶阀1
                            PublicData.Dev.DOList[1].IsOn = true;   //开粗管蝶阀2
                            PublicData.Dev.DOList[2].IsOn = false;   //关细管蝶阀3
                            PublicData.Dev.DOList[3].IsOn = false;   //关加粗管蝶阀4
                            PublicData.Dev.DOList[4].IsOn = true;   //开差压电磁阀
                            PublicData.Dev.DOList[5].IsOn = true;   //开KM1
                            PublicData.Dev.DOList[7].IsOn = true;   //开气泵
                            PublicData.Dev.DOList[8].IsOn = true;   //开风机变频

                            bool[] order1 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] order2 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] tempArry = Enumerable.Repeat(false, PublicData.Dev.DOList.Count).ToArray();
                            for (int i = 0; i < PublicData.Dev.DOList.Count; i++)
                                tempArry[i] = PublicData.Dev.DOList[i].IsOn;
                            Array.Copy(tempArry, 0, order1, 0, 16);
                            Array.Copy(tempArry, 16, order2, 0, 2);

                            PLCCKCMDFrmBLL[0] = 1;  //PLC为程控模式
                            PLCCKCMDFrmBLL[1] = MQZH_ValueCvtBLL.Bools2Ushort(order1);
                            PLCCKCMDFrmBLL[2] = MQZH_ValueCvtBLL.Bools2Ushort(order2);
                            PLCCKCMDFrmBLL[19] = 1;     //看门狗
                            if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                                PLCCKCMDFrmBLL[5] = 5;  //换向阀为正压模式

                            PLCCKCMDFrmBLL[3] = PublicData.Dev.Valve.DIList[5].IsOn ? SDQMFunc() : (ushort)0;   //正压模式完成后输出计算频率
                            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
                            break;
                        }
                    //手动气密蝶阀2负压模式
                    case DevicRunModeType.DF2QMFDbg_Mode:
                        {
                            PublicData.Dev.IsDeviceBusy = true;

                            PublicData.Dev.DOList[0].IsOn = false;   //关主管蝶阀1
                            PublicData.Dev.DOList[1].IsOn = true;   //开粗管蝶阀2
                            PublicData.Dev.DOList[2].IsOn = false;   //关细管蝶阀3
                            PublicData.Dev.DOList[3].IsOn = false;   //关加粗管蝶阀4
                            PublicData.Dev.DOList[4].IsOn = true;   //开差压电磁阀
                            PublicData.Dev.DOList[5].IsOn = true;   //开KM1
                            PublicData.Dev.DOList[7].IsOn = true;   //开气泵
                            PublicData.Dev.DOList[8].IsOn = true;   //开风机变频

                            bool[] order1 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] order2 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] tempArry = Enumerable.Repeat(false, PublicData.Dev.DOList.Count).ToArray();
                            for (int i = 0; i < PublicData.Dev.DOList.Count; i++)
                                tempArry[i] = PublicData.Dev.DOList[i].IsOn;
                            Array.Copy(tempArry, 0, order1, 0, 16);
                            Array.Copy(tempArry, 16, order2, 0, 2);

                            PLCCKCMDFrmBLL[0] = 1;  //PLC为程控模式
                            PLCCKCMDFrmBLL[1] = MQZH_ValueCvtBLL.Bools2Ushort(order1);
                            PLCCKCMDFrmBLL[2] = MQZH_ValueCvtBLL.Bools2Ushort(order2);
                            PLCCKCMDFrmBLL[19] = 1;     //看门狗
                            if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                                PLCCKCMDFrmBLL[5] = 6;  //换向阀为负压模式

                            PLCCKCMDFrmBLL[3] = PublicData.Dev.Valve.DIList[6].IsOn ? SDQMFunc() : (ushort)0;   //负压模式完成后输出计算频率
                            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
                            break;
                        }

                    //手动气密蝶阀3正压模式
                    case DevicRunModeType.DF3QMZDbg_Mode:
                        {
                            PublicData.Dev.IsDeviceBusy = true;

                            PublicData.Dev.DOList[0].IsOn = false;   //关主管蝶阀1
                            PublicData.Dev.DOList[1].IsOn = false;   //关粗管蝶阀2
                            PublicData.Dev.DOList[2].IsOn = true;   //开细管蝶阀3
                            PublicData.Dev.DOList[3].IsOn = false;   //关加粗管蝶阀4
                            PublicData.Dev.DOList[4].IsOn = true;   //开差压电磁阀
                            PublicData.Dev.DOList[5].IsOn = true;   //开KM1
                            PublicData.Dev.DOList[7].IsOn = true;   //开气泵
                            PublicData.Dev.DOList[8].IsOn = true;   //开风机变频

                            bool[] order1 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] order2 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] tempArry = Enumerable.Repeat(false, PublicData.Dev.DOList.Count).ToArray();
                            for (int i = 0; i < PublicData.Dev.DOList.Count; i++)
                                tempArry[i] = PublicData.Dev.DOList[i].IsOn;
                            Array.Copy(tempArry, 0, order1, 0, 16);
                            Array.Copy(tempArry, 16, order2, 0, 2);

                            PLCCKCMDFrmBLL[0] = 1;  //PLC为程控模式
                            PLCCKCMDFrmBLL[1] = MQZH_ValueCvtBLL.Bools2Ushort(order1);
                            PLCCKCMDFrmBLL[2] = MQZH_ValueCvtBLL.Bools2Ushort(order2);
                            PLCCKCMDFrmBLL[19] = 1;     //看门狗
                            if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                                PLCCKCMDFrmBLL[5] = 5;  //换向阀为正压模式

                            PLCCKCMDFrmBLL[3] = PublicData.Dev.Valve.DIList[5].IsOn ? SDQMFunc() : (ushort)0;   //正压模式完成后输出计算频率
                            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
                            break;
                        }
                    //手动气密蝶阀3负压模式
                    case DevicRunModeType.DF3QMFDbg_Mode:
                        {
                            PublicData.Dev.IsDeviceBusy = true;

                            PublicData.Dev.DOList[0].IsOn = false;   //关主管蝶阀1
                            PublicData.Dev.DOList[1].IsOn = false;   //关粗管蝶阀2
                            PublicData.Dev.DOList[2].IsOn = true;   //开细管蝶阀3
                            PublicData.Dev.DOList[3].IsOn = false;   //关加粗管蝶阀4
                            PublicData.Dev.DOList[4].IsOn = true;   //开差压电磁阀
                            PublicData.Dev.DOList[5].IsOn = true;   //开KM1
                            PublicData.Dev.DOList[7].IsOn = true;   //开气泵
                            PublicData.Dev.DOList[8].IsOn = true;   //开风机变频

                            bool[] order1 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] order2 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] tempArry = Enumerable.Repeat(false, PublicData.Dev.DOList.Count).ToArray();
                            for (int i = 0; i < PublicData.Dev.DOList.Count; i++)
                                tempArry[i] = PublicData.Dev.DOList[i].IsOn;
                            Array.Copy(tempArry, 0, order1, 0, 16);
                            Array.Copy(tempArry, 16, order2, 0, 2);

                            PLCCKCMDFrmBLL[0] = 1;  //PLC为程控模式
                            PLCCKCMDFrmBLL[1] = MQZH_ValueCvtBLL.Bools2Ushort(order1);
                            PLCCKCMDFrmBLL[2] = MQZH_ValueCvtBLL.Bools2Ushort(order2);
                            PLCCKCMDFrmBLL[19] = 1;     //看门狗
                            if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                                PLCCKCMDFrmBLL[5] = 6;  //换向阀为负压模式

                            PLCCKCMDFrmBLL[3] = PublicData.Dev.Valve.DIList[6].IsOn ? SDQMFunc() : (ushort)0;   //负压模式完成后输出计算频率
                            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
                            break;
                        }

                    //手动气密蝶阀4正压模式
                    case DevicRunModeType.DF4QMZDbg_Mode:
                        {
                            PublicData.Dev.IsDeviceBusy = true;

                            PublicData.Dev.DOList[0].IsOn = false;   //关主管蝶阀1
                            PublicData.Dev.DOList[1].IsOn = false;   //关粗管蝶阀2
                            PublicData.Dev.DOList[2].IsOn = false;   //关细管蝶阀3
                            PublicData.Dev.DOList[3].IsOn = true;   //开加粗管蝶阀4
                            PublicData.Dev.DOList[4].IsOn = true;   //开差压电磁阀
                            PublicData.Dev.DOList[5].IsOn = true;   //开KM1
                            PublicData.Dev.DOList[7].IsOn = true;   //开气泵
                            PublicData.Dev.DOList[8].IsOn = true;   //开风机变频

                            bool[] order1 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] order2 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] tempArry = Enumerable.Repeat(false, PublicData.Dev.DOList.Count).ToArray();
                            for (int i = 0; i < PublicData.Dev.DOList.Count; i++)
                                tempArry[i] = PublicData.Dev.DOList[i].IsOn;
                            Array.Copy(tempArry, 0, order1, 0, 16);
                            Array.Copy(tempArry, 16, order2, 0, 2);

                            PLCCKCMDFrmBLL[0] = 1;  //PLC为程控模式
                            PLCCKCMDFrmBLL[1] = MQZH_ValueCvtBLL.Bools2Ushort(order1);
                            PLCCKCMDFrmBLL[2] = MQZH_ValueCvtBLL.Bools2Ushort(order2);
                            PLCCKCMDFrmBLL[19] = 1;     //看门狗
                            if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                                PLCCKCMDFrmBLL[5] = 5;  //换向阀为正压模式

                            PLCCKCMDFrmBLL[3] = PublicData.Dev.Valve.DIList[5].IsOn ? SDQMFunc() : (ushort)0;   //正压模式完成后输出计算频率
                            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
                            break;
                        }
                    //手动气密蝶阀4负压模式
                    case DevicRunModeType.DF4QMFDbg_Mode:
                        {
                            PublicData.Dev.IsDeviceBusy = true;

                            PublicData.Dev.DOList[0].IsOn = false;   //关主管蝶阀1
                            PublicData.Dev.DOList[1].IsOn = false;   //关粗管蝶阀2
                            PublicData.Dev.DOList[2].IsOn = false;   //关细管蝶阀3
                            PublicData.Dev.DOList[3].IsOn = true;   //开加粗管蝶阀4
                            PublicData.Dev.DOList[4].IsOn = true;   //开差压电磁阀
                            PublicData.Dev.DOList[5].IsOn = true;   //开KM1
                            PublicData.Dev.DOList[7].IsOn = true;   //开气泵
                            PublicData.Dev.DOList[8].IsOn = true;   //开风机变频

                            bool[] order1 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] order2 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] tempArry = Enumerable.Repeat(false, PublicData.Dev.DOList.Count).ToArray();
                            for (int i = 0; i < PublicData.Dev.DOList.Count; i++)
                                tempArry[i] = PublicData.Dev.DOList[i].IsOn;
                            Array.Copy(tempArry, 0, order1, 0, 16);
                            Array.Copy(tempArry, 16, order2, 0, 2);

                            PLCCKCMDFrmBLL[0] = 1;  //PLC为程控模式
                            PLCCKCMDFrmBLL[1] = MQZH_ValueCvtBLL.Bools2Ushort(order1);
                            PLCCKCMDFrmBLL[2] = MQZH_ValueCvtBLL.Bools2Ushort(order2);
                            PLCCKCMDFrmBLL[19] = 1;     //看门狗
                            if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                                PLCCKCMDFrmBLL[5] = 6;  //换向阀为负压模式

                            PLCCKCMDFrmBLL[3] = PublicData.Dev.Valve.DIList[6].IsOn ? SDQMFunc() : (ushort)0;   //负压模式完成后输出计算频率
                            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
                            break;
                        }

                    //手动水密正压模式
                    case DevicRunModeType.SDSMZDbg_Mode:
                        {
                            PublicData.Dev.IsDeviceBusy = true;

                            PublicData.Dev.DOList[0].IsOn = true;   //开主管蝶阀1
                            PublicData.Dev.DOList[1].IsOn = false;   //关粗管蝶阀2
                            PublicData.Dev.DOList[2].IsOn = false;   //关细管蝶阀3
                            PublicData.Dev.DOList[3].IsOn = false;   //关加粗管蝶阀4
                            PublicData.Dev.DOList[4].IsOn = false;   //开差压电磁阀
                            PublicData.Dev.DOList[5].IsOn = true;   //开KM1
                            PublicData.Dev.DOList[7].IsOn = true;   //开气泵
                            PublicData.Dev.DOList[8].IsOn = true;   //开风机变频

                            bool[] order1 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] order2 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] tempArry = Enumerable.Repeat(false, PublicData.Dev.DOList.Count).ToArray();
                            for (int i = 0; i < PublicData.Dev.DOList.Count; i++)
                                tempArry[i] = PublicData.Dev.DOList[i].IsOn;
                            Array.Copy(tempArry, 0, order1, 0, 16);
                            Array.Copy(tempArry, 16, order2, 0, 2);

                            PLCCKCMDFrmBLL[0] = 1;  //PLC为程控模式
                            PLCCKCMDFrmBLL[1] = MQZH_ValueCvtBLL.Bools2Ushort(order1);
                            PLCCKCMDFrmBLL[2] = MQZH_ValueCvtBLL.Bools2Ushort(order2);
                            PLCCKCMDFrmBLL[19] = 1;     //看门狗
                            if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                                PLCCKCMDFrmBLL[5] = 5;  //换向阀为正压模式

                            PLCCKCMDFrmBLL[3] = PublicData.Dev.Valve.DIList[5].IsOn ? SDSMFunc() : (ushort)0;   //正压模式完成后输出计算频率
                            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
                            break;
                        }
                    //手动水密负压模式
                    case DevicRunModeType.SDSMFDbg_Mode:
                        {
                            PublicData.Dev.IsDeviceBusy = true;

                            PublicData.Dev.DOList[0].IsOn = true;   //开主管蝶阀1
                            PublicData.Dev.DOList[1].IsOn = false;   //关粗管蝶阀2
                            PublicData.Dev.DOList[2].IsOn = false;   //关细管蝶阀3
                            PublicData.Dev.DOList[3].IsOn = false;   //关加粗管蝶阀4
                            PublicData.Dev.DOList[4].IsOn = false;   //开差压电磁阀
                            PublicData.Dev.DOList[5].IsOn = true;   //开KM1
                            PublicData.Dev.DOList[7].IsOn = true;   //开气泵
                            PublicData.Dev.DOList[8].IsOn = true;   //开风机变频

                            bool[] order1 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] order2 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] tempArry = Enumerable.Repeat(false, PublicData.Dev.DOList.Count).ToArray();
                            for (int i = 0; i < PublicData.Dev.DOList.Count; i++)
                                tempArry[i] = PublicData.Dev.DOList[i].IsOn;
                            Array.Copy(tempArry, 0, order1, 0, 16);
                            Array.Copy(tempArry, 16, order2, 0, 2);

                            PLCCKCMDFrmBLL[0] = 1;  //PLC为程控模式
                            PLCCKCMDFrmBLL[1] = MQZH_ValueCvtBLL.Bools2Ushort(order1);
                            PLCCKCMDFrmBLL[2] = MQZH_ValueCvtBLL.Bools2Ushort(order2);
                            PLCCKCMDFrmBLL[19] = 1;     //看门狗
                            if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                                PLCCKCMDFrmBLL[5] = 6;  //换向阀为负压模式

                            PLCCKCMDFrmBLL[3] = PublicData.Dev.Valve.DIList[6].IsOn ? SDSMFunc() : (ushort)0;   //负压模式完成后输出计算频率
                            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
                            break;
                        }

                    //手动水流量PID调试模式
                    case DevicRunModeType.SLLPIDDbg_Mode:
                        {
                            PublicData.Dev.IsDeviceBusy = true;

                            PublicData.Dev.DOList[0].IsOn = false;   //关主管蝶阀1
                            PublicData.Dev.DOList[1].IsOn = false;   //关粗管蝶阀2
                            PublicData.Dev.DOList[2].IsOn = false;   //关细管蝶阀3
                            PublicData.Dev.DOList[3].IsOn = false;   //关加粗管蝶阀4
                            PublicData.Dev.DOList[4].IsOn = false;   //关差压电磁阀
                            PublicData.Dev.DOList[5].IsOn = false;   //关KM1
                            PublicData.Dev.DOList[6].IsOn = true;   //开水泵
                            PublicData.Dev.DOList[7].IsOn = false;   //关气泵
                            PublicData.Dev.DOList[8].IsOn = false;   //关风机变频
                            PublicData.Dev.DOList[9].IsOn = true;   //开水泵变频

                            bool[] order1 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] order2 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] tempArry = Enumerable.Repeat(false, PublicData.Dev.DOList.Count).ToArray();
                            for (int i = 0; i < PublicData.Dev.DOList.Count; i++)
                                tempArry[i] = PublicData.Dev.DOList[i].IsOn;
                            Array.Copy(tempArry, 0, order1, 0, 16);
                            Array.Copy(tempArry, 16, order2, 0, 2);

                            PLCCKCMDFrmBLL[0] = 1;  //PLC为程控模式
                            PLCCKCMDFrmBLL[1] = MQZH_ValueCvtBLL.Bools2Ushort(order1);
                            PLCCKCMDFrmBLL[2] = MQZH_ValueCvtBLL.Bools2Ushort(order2);
                            PLCCKCMDFrmBLL[19] = 1;     //看门狗
                            if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                                PLCCKCMDFrmBLL[5] = 5;  //换向阀为正压模式

                            PLCCKCMDFrmBLL[4] = PublicData.Dev.Valve.DIList[5].IsOn ? SLLPIDFunc() : (ushort)0;   //正压模式完成后输出计算频率
                            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
                            break;
                        }

                    //气密检测模式=========================================================
                    case DevicRunModeType.QM_Mode:
                        {
                            //蝶阀选择
                            switch (PublicData.Dev.DFUse)
                            {
                                case 1:
                                    PublicData.Dev.DOList[0].IsOn = true;   //开主管蝶阀1
                                    PublicData.Dev.DOList[1].IsOn = false;   //关粗管蝶阀2
                                    PublicData.Dev.DOList[2].IsOn = false;   //关细管蝶阀3
                                    PublicData.Dev.DOList[3].IsOn = false;   //关加粗管蝶阀4
                                    break;
                                case 2:
                                    PublicData.Dev.DOList[0].IsOn = false;   //关主管蝶阀1
                                    PublicData.Dev.DOList[1].IsOn = true;   //开粗管蝶阀2
                                    PublicData.Dev.DOList[2].IsOn = false;   //关细管蝶阀3
                                    PublicData.Dev.DOList[3].IsOn = false;   //关加粗管蝶阀4
                                    break;
                                case 3:
                                    PublicData.Dev.DOList[0].IsOn = false;   //关主管蝶阀1
                                    PublicData.Dev.DOList[1].IsOn = false;   //关粗管蝶阀2
                                    PublicData.Dev.DOList[2].IsOn = true;   //开细管蝶阀3
                                    PublicData.Dev.DOList[3].IsOn = false;   //关加粗管蝶阀4
                                    break;
                                case 4:
                                    PublicData.Dev.DOList[0].IsOn = false;   //关主管蝶阀1
                                    PublicData.Dev.DOList[1].IsOn = false;   //关粗管蝶阀2
                                    PublicData.Dev.DOList[2].IsOn = false;   //关细管蝶阀3
                                    PublicData.Dev.DOList[3].IsOn = true;   //开加粗管蝶阀4
                                    break;
                            }

                            PublicData.Dev.DOList[4].IsOn = true;   //开差压电磁阀
                            PublicData.Dev.DOList[5].IsOn = true;   //开KM1
                            PublicData.Dev.DOList[7].IsOn = true;   //开气泵
                            PublicData.Dev.DOList[8].IsOn = true;   //开风机变频

                            bool[] order1 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] order2 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] tempArry = Enumerable.Repeat(false, PublicData.Dev.DOList.Count).ToArray();
                            for (int i = 0; i < PublicData.Dev.DOList.Count; i++)
                                tempArry[i] = PublicData.Dev.DOList[i].IsOn;
                            Array.Copy(tempArry, 0, order1, 0, 16);
                            Array.Copy(tempArry, 16, order2, 0, 2);

                            PLCCKCMDFrmBLL[0] = 1;  //PLC为程控模式
                            PLCCKCMDFrmBLL[1] = MQZH_ValueCvtBLL.Bools2Ushort(order1);
                            PLCCKCMDFrmBLL[2] = MQZH_ValueCvtBLL.Bools2Ushort(order2);
                            PLCCKCMDFrmBLL[19] = 1;     //看门狗
                            if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                            {
                                if (PPressTest)
                                    PLCCKCMDFrmBLL[5] = 5;  //换向阀为正压模式
                                else
                                    PLCCKCMDFrmBLL[5] = 6;  //换向阀为负压模式
                            }

                            if (ExistQMDJExp)
                                QMDJStageFunc();
                            else if (ExistQMGCExp)
                                QMGCStageFunc();
                            else
                                PLCCKCMDFrmBLL[3] = 0;

                            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
                            break;
                        }

                    //水密检测模式=========================================================
                    case DevicRunModeType.SM_Mode:
                        {
                            PublicData.Dev.DOList[0].IsOn = true;   //开主管蝶阀1
                            PublicData.Dev.DOList[1].IsOn = false;   //关粗管蝶阀2
                            PublicData.Dev.DOList[2].IsOn = false;   //关细管蝶阀3
                            PublicData.Dev.DOList[3].IsOn = false;   //关加粗管蝶阀4
                            PublicData.Dev.DOList[4].IsOn = false;   //关差压电磁阀
                            PublicData.Dev.DOList[5].IsOn = true;   //开KM1
                            PublicData.Dev.DOList[7].IsOn = true;   //开气泵
                            PublicData.Dev.DOList[8].IsOn = true;   //开风机变频

                            bool[] order1 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] order2 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] tempArry = Enumerable.Repeat(false, PublicData.Dev.DOList.Count).ToArray();
                            for (int i = 0; i < PublicData.Dev.DOList.Count; i++)
                                tempArry[i] = PublicData.Dev.DOList[i].IsOn;
                            Array.Copy(tempArry, 0, order1, 0, 16);
                            Array.Copy(tempArry, 16, order2, 0, 2);

                            PLCCKCMDFrmBLL[0] = 1;  //PLC为程控模式
                            PLCCKCMDFrmBLL[1] = MQZH_ValueCvtBLL.Bools2Ushort(order1);
                            PLCCKCMDFrmBLL[2] = MQZH_ValueCvtBLL.Bools2Ushort(order2);
                            PLCCKCMDFrmBLL[19] = 1;     //看门狗

                            if (ExistSMWDDJExp)
                            {
                                if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                                {
                                    PPressTest = true;
                                    PLCCKCMDFrmBLL[5] = 5;  //换向阀为正压模式
                                }
                                SMWDDJStageFunc();
                            }
                            else if (ExistSMWDGCExp)
                            {
                                if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                                {
                                    PPressTest = true;
                                    PLCCKCMDFrmBLL[5] = 5;  //换向阀为正压模式
                                }
                                SMWDGCStageFunc();
                            }
                            else if (ExistSMBDDJExp)
                                SMBDDJStageFunc();
                            else if (ExistSMBDGCExp)
                                SMBDGCStageFunc();
                            else
                                PLCCKCMDFrmBLL[3] = 0;

                            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");

                            break;
                        }

                    //抗风压P1检测模式=========================================================
                    case DevicRunModeType.KFYp1_Mode:
                        {
                            PublicData.Dev.DOList[0].IsOn = true;   //开主管蝶阀1
                            PublicData.Dev.DOList[1].IsOn = false;   //关粗管蝶阀2
                            PublicData.Dev.DOList[2].IsOn = false;   //关细管蝶阀3
                            PublicData.Dev.DOList[3].IsOn = false;   //关加粗管蝶阀4
                            PublicData.Dev.DOList[4].IsOn = false;   //关差压电磁阀
                            PublicData.Dev.DOList[5].IsOn = true;   //开KM1
                            PublicData.Dev.DOList[7].IsOn = true;   //开气泵
                            PublicData.Dev.DOList[8].IsOn = true;   //开风机变频

                            bool[] order1 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] order2 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] tempArry = Enumerable.Repeat(false, PublicData.Dev.DOList.Count).ToArray();
                            for (int i = 0; i < PublicData.Dev.DOList.Count; i++)
                                tempArry[i] = PublicData.Dev.DOList[i].IsOn;
                            Array.Copy(tempArry, 0, order1, 0, 16);
                            Array.Copy(tempArry, 16, order2, 0, 2);

                            PLCCKCMDFrmBLL[0] = 1;  //PLC为程控模式
                            PLCCKCMDFrmBLL[1] = MQZH_ValueCvtBLL.Bools2Ushort(order1);
                            PLCCKCMDFrmBLL[2] = MQZH_ValueCvtBLL.Bools2Ushort(order2);
                            PLCCKCMDFrmBLL[19] = 1;     //看门狗
                            if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                            {
                                if (PPressTest)
                                    PLCCKCMDFrmBLL[5] = 5;  //换向阀为正压模式
                                else
                                    PLCCKCMDFrmBLL[5] = 6;  //换向阀为负压模式
                            }

                            if (ExistKFYp1DJExp)
                                KFYp1DJStageFunc();
                            else if (ExistKFYp1GCExp)
                                KFYp1GCStageFunc();
                            else
                                PLCCKCMDFrmBLL[3] = 0;

                            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
                            break;
                        }

                    //抗风压P3检测模式=========================================================
                    case DevicRunModeType.KFYp3_Mode:
                        {
                            PublicData.Dev.DOList[0].IsOn = true;   //开主管蝶阀1
                            PublicData.Dev.DOList[1].IsOn = false;   //关粗管蝶阀2
                            PublicData.Dev.DOList[2].IsOn = false;   //关细管蝶阀3
                            PublicData.Dev.DOList[3].IsOn = false;   //关加粗管蝶阀4
                            PublicData.Dev.DOList[4].IsOn = false;   //关差压电磁阀
                            PublicData.Dev.DOList[5].IsOn = true;   //开KM1
                            PublicData.Dev.DOList[7].IsOn = true;   //开气泵
                            PublicData.Dev.DOList[8].IsOn = true;   //开风机变频

                            bool[] order1 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] order2 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] tempArry = Enumerable.Repeat(false, PublicData.Dev.DOList.Count).ToArray();
                            for (int i = 0; i < PublicData.Dev.DOList.Count; i++)
                                tempArry[i] = PublicData.Dev.DOList[i].IsOn;
                            Array.Copy(tempArry, 0, order1, 0, 16);
                            Array.Copy(tempArry, 16, order2, 0, 2);

                            PLCCKCMDFrmBLL[0] = 1;  //PLC为程控模式
                            PLCCKCMDFrmBLL[1] = MQZH_ValueCvtBLL.Bools2Ushort(order1);
                            PLCCKCMDFrmBLL[2] = MQZH_ValueCvtBLL.Bools2Ushort(order2);
                            PLCCKCMDFrmBLL[19] = 1;     //看门狗
                            if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                            {
                                if (PPressTest)
                                    PLCCKCMDFrmBLL[5] = 5;  //换向阀为正压模式
                                else
                                    PLCCKCMDFrmBLL[5] = 6;  //换向阀为负压模式
                            }

                            if (ExistKFYp3DJExp)
                                KFYp3DJStageFunc();
                            else if (ExistKFYp3GCExp)
                                KFYp3GCStageFunc();
                            else
                                PLCCKCMDFrmBLL[3] = 0;

                            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
                            break;
                        }

                    //抗风压P2检测模式=========================================================
                    case DevicRunModeType.KFYp2_Mode:
                        {
                            PublicData.Dev.DOList[0].IsOn = true;   //开主管蝶阀1
                            PublicData.Dev.DOList[1].IsOn = false;   //关粗管蝶阀2
                            PublicData.Dev.DOList[2].IsOn = false;   //关细管蝶阀3
                            PublicData.Dev.DOList[3].IsOn = false;   //关加粗管蝶阀4
                            PublicData.Dev.DOList[4].IsOn = false;   //关差压电磁阀
                            PublicData.Dev.DOList[5].IsOn = true;   //开KM1
                            PublicData.Dev.DOList[6].IsOn = false;   //关水泵
                            PublicData.Dev.DOList[7].IsOn = true;   //开气泵
                            PublicData.Dev.DOList[8].IsOn = true;   //开风机变频
                            PublicData.Dev.DOList[9].IsOn = false;   //关水泵变频

                            bool[] order1 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] order2 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] tempArry = Enumerable.Repeat(false, PublicData.Dev.DOList.Count).ToArray();
                            for (int i = 0; i < PublicData.Dev.DOList.Count; i++)
                                tempArry[i] = PublicData.Dev.DOList[i].IsOn;
                            Array.Copy(tempArry, 0, order1, 0, 16);
                            Array.Copy(tempArry, 16, order2, 0, 2);

                            PLCCKCMDFrmBLL[0] = 1;  //PLC为程控模式
                            PLCCKCMDFrmBLL[1] = MQZH_ValueCvtBLL.Bools2Ushort(order1);
                            PLCCKCMDFrmBLL[2] = MQZH_ValueCvtBLL.Bools2Ushort(order2);
                            PLCCKCMDFrmBLL[4] = 0;  //水泵频率
                            PLCCKCMDFrmBLL[19] = 1;     //看门狗

                            if (ExistKFYp2DJExp)
                                ExpKFYp2DJStageFunc();
                            else if (ExistKFYp2GCExp)
                                ExpKFYp2GCStageFunc();
                            else
                                PLCCKCMDFrmBLL[3] = 0;
                            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
                            break;
                        }

                    //抗风压Pmax检测模式=========================================================
                    case DevicRunModeType.KFYpmax_Mode:
                        {
                            PublicData.Dev.DOList[0].IsOn = true;   //开主管蝶阀1
                            PublicData.Dev.DOList[1].IsOn = false;   //关粗管蝶阀2
                            PublicData.Dev.DOList[2].IsOn = false;   //关细管蝶阀3
                            PublicData.Dev.DOList[3].IsOn = false;   //关加粗管蝶阀4
                            PublicData.Dev.DOList[4].IsOn = false;   //关差压电磁阀
                            PublicData.Dev.DOList[5].IsOn = true;   //开KM1
                            PublicData.Dev.DOList[7].IsOn = true;   //开气泵
                            PublicData.Dev.DOList[8].IsOn = true;   //开风机变频

                            bool[] order1 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] order2 = Enumerable.Repeat(false, 16).ToArray();
                            bool[] tempArry = Enumerable.Repeat(false, PublicData.Dev.DOList.Count).ToArray();
                            for (int i = 0; i < PublicData.Dev.DOList.Count; i++)
                                tempArry[i] = PublicData.Dev.DOList[i].IsOn;
                            Array.Copy(tempArry, 0, order1, 0, 16);
                            Array.Copy(tempArry, 16, order2, 0, 2);

                            PLCCKCMDFrmBLL[0] = 1;  //PLC为程控模式
                            PLCCKCMDFrmBLL[1] = MQZH_ValueCvtBLL.Bools2Ushort(order1);
                            PLCCKCMDFrmBLL[2] = MQZH_ValueCvtBLL.Bools2Ushort(order2);
                            PLCCKCMDFrmBLL[19] = 1;     //看门狗
                            if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                            {
                                if (PPressTest)
                                    PLCCKCMDFrmBLL[5] = 5;  //换向阀为正压模式
                                else
                                    PLCCKCMDFrmBLL[5] = 6;  //换向阀为负压模式
                            }

                            if (ExistKFYpmaxDJExp)
                                KFYpmaxDJStageFunc();
                            else if (ExistKFYpmaxGCExp)
                                KFYpmaxGCStageFunc();
                            else
                                PLCCKCMDFrmBLL[3] = 0;
                            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
                            break;
                        }

                    //手动位移调试模式=========================================================
                    case DevicRunModeType.SDWYDbg_Mode:
                        {
                            PLCCKCMDFrmBLL[0] = 2;  //PLC为位移模式
                            PLCCKCMDFrmBLL[19] = 1;     //看门狗
                            if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                                PLCCKCMDFrmBLL[5] = 1;  //换向阀为待机模式

                            if (OnceOrderSDWYDebug)//单次指令
                            {
                                Messenger.Default.Send<ushort[]>(OnceOrderValuesFrmBLL, "OnceOrderToComMessage");
                                OnceOrderSDWYDebug = false;
                            }

                            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
                            break;
                        }

                    //层间变形检测模式=========================================================
                    case DevicRunModeType.CJBX_Mode:
                        {
                            PLCCKCMDFrmBLL[0] = 2;  //PLC为位移模式
                            PLCCKCMDFrmBLL[19] = 1;     //看门狗
                            if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                                PLCCKCMDFrmBLL[5] = 1;  //换向阀为待机模式
                            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");

                            if (ExistCJBXDJXExp)
                                CJBX_DJXStageFunc();

                            else if (ExistCJBXDJYExp)
                                CJBX_DJYStageFunc();

                            else if (ExistCJBXDJZExp)
                                CJBX_DJZStageFunc();

                            else if (ExistCJBXGCXExp)
                                CJBX_GCXStageFunc();

                            else if (ExistCJBXGCYExp)
                                CJBX_GCYStageFunc();

                            else if (ExistCJBXGCZExp)
                                CJBX_GCZStageFunc();
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                TimerThreadLock = false; //进程解锁
            }
        }

        #endregion

        //========================================================================

        #region 紧急停机、待机事务

        /// <summary>
        /// 紧急停机事务
        /// </summary>
        /// <param name="state"></param>
        private void JJTJFunc()
        {
            EscStopCompleteExpReset();

            PLCCKCMDFrmBLL = Enumerable.Repeat((ushort)0, DataLengthFrmBLLPLCCK).ToArray();
            PLCCKCMDFrmBLL[0] = 119;  //PLC为紧急停机模式
            PLCCKCMDFrmBLL[19] = 1;     //看门狗
            PLCCKCMDFrmBLL[5] = 1;  //换向阀为待机模式
            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
        }


        /// <summary>
        /// 待机事务
        /// </summary>
        /// <param name="state"></param>
        private void WaitFunc()
        {
            PLCCKCMDFrmBLL = Enumerable.Repeat((ushort)0, DataLengthFrmBLLPLCCK).ToArray();
            PLCCKCMDFrmBLL[0] = 1;  //PLC为程控
            PLCCKCMDFrmBLL[19] = 1;     //看门狗
            if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[4].IsOn)   //风阀无故障且自检完成时
                PLCCKCMDFrmBLL[5] = 5;  //换向阀为正压模式
            Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
        }

        #endregion


        #region 各类检测业务程序辅助方法

        /// <summary>
        /// 根据类型、阶段、步骤编号，获取目标压力
        /// </summary>
        /// <param name="type">类型。气密定级10，气密工程11；
        /// 水密预加压20，水密定级稳定21，水密定级波动22;
        /// 水密工程稳定2步23，水密工程稳定3步24，水密工程稳定9步25，水密工程稳定10步26；
        /// 水密工程波动2步27，水密工程波动3步28，水密工程波动9步29，水密工程波动10步30；
        /// 抗风压定级变形31，抗风压定级反复32，抗风压定级P3 33，抗风压定级Pmax 34
        /// 抗风压工程变形35，抗风压工程反复36，抗风压工程P3 37，抗风压工程Pmax 38
        /// </param>
        /// <param name="stageNo"></param>
        /// <param name="StepNo"></param>
        /// <returns></returns>
        private double GetAimPress(int type, int stageNo, int StepNo)
        {
            double tempRet = 0;
            switch (type)
            {
                #region 气密目标压力
                //气密定级
                case 10:
                    if (PublicData.Dev.IsPressFalse)
                        tempRet = PublicData.Dev.PressSet_QMDJ_False[stageNo][StepNo];
                    else
                        tempRet = PublicData.Dev.PressSet_QMDJ_Std[stageNo][StepNo];
                    break;
                //气密工程
                case 11:
                    if (PublicData.Dev.IsPressFalse)
                        tempRet = PublicData.Dev.PressSet_QMGC_False[stageNo][StepNo];
                    else
                    {
                        if ((stageNo == 0) || (stageNo == 4) || (stageNo == 8))
                            tempRet = Math.Abs(PublicData.ExpDQ.Exp_QM.QM_GCYJYP);
                        else if ((stageNo == 1) || (stageNo == 5) || (stageNo == 9))
                            tempRet = Math.Abs(PublicData.ExpDQ.Exp_QM.QM_GCSJP);
                        else if ((stageNo == 2) || (stageNo == 6) || (stageNo == 10))
                            tempRet = -Math.Abs(PublicData.ExpDQ.Exp_QM.QM_GCYJYP);
                        else if ((stageNo == 3) || (stageNo == 7) || (stageNo == 11))
                            tempRet = -Math.Abs(PublicData.ExpDQ.Exp_QM.QM_GCSJP);
                    }
                    break;
                #endregion

                #region 水密目标压力
                //水密预加压
                case 20:
                    if (PublicData.Dev.IsPressFalse)
                        tempRet = PublicData.Dev.PressSet_SM_YJY_False[StepNo];
                    else
                        tempRet = PublicData.Dev.PressSet_SM_YJY_Std[StepNo];
                    break;
                //水密定级稳定加压
                case 21:
                    if (PublicData.Dev.IsPressFalse)
                        tempRet = PublicData.Dev.PressSet_SMDJ_WD_False[StepNo];
                    else
                        tempRet = PublicData.Dev.PressSet_SMDJ_WD_Std[StepNo];
                    break;
                //水密定级波动加压
                case 22:
                    if (PublicData.Dev.IsPressFalse)
                        tempRet = PublicData.Dev.PressSet_SMDJ_BDPJ_False[StepNo];
                    else
                        tempRet = PublicData.Dev.PressSet_SMDJ_BDPJ_Std[StepNo];
                    break;

                //水密工程稳定加压2步23
                case 23:
                    if (PublicData.Dev.IsPressFalse)
                    {
                        if (StepNo == 0)
                            tempRet = 0;
                        else
                            tempRet = PublicData.Dev.PressSet_SMGC_WDGD_False;
                    }
                    else
                    {
                        if (StepNo == 0)
                            tempRet = 0;
                        else
                            tempRet = PublicData.ExpDQ.Exp_SM.SM_GCSJ_GD;
                    }
                    break;
                //水密工程稳定加压3步24
                case 24:
                    if (PublicData.Dev.IsPressFalse)
                    {
                        if (StepNo == 0)
                            tempRet = 0;
                        else if (StepNo == 1)
                            tempRet = PublicData.Dev.PressSet_SMGC_WDKKQ_False;
                        else
                            tempRet = PublicData.Dev.PressSet_SMGC_WDGD_False;
                    }
                    else
                    {
                        if (StepNo == 0)
                            tempRet = 0;
                        else if (StepNo == 1)
                            tempRet = PublicData.ExpDQ.Exp_SM.SM_GCSJ_KKQ;
                        else
                            tempRet = PublicData.ExpDQ.Exp_SM.SM_GCSJ_GD;
                    }
                    break;
                //水密工程稳定加压9步25
                case 25:
                    if (PublicData.Dev.IsPressFalse)
                    {
                        if (StepNo == 8)
                            tempRet = PublicData.Dev.PressSet_SMGC_WDGD_False;
                        else
                            tempRet = PublicData.Dev.PressSet_SMDJ_WD_False[StepNo];
                    }
                    else
                    {
                        if (StepNo == 8)
                            tempRet = PublicData.ExpDQ.Exp_SM.SM_GCSJ_GD;
                        else
                            tempRet = PublicData.Dev.PressSet_SMDJ_WD_Std[StepNo];
                    }
                    break;
                //水密工程稳定加压10步26
                case 26:
                    if (PublicData.Dev.IsPressFalse)
                    {
                        if (StepNo == 8)
                            tempRet = PublicData.Dev.PressSet_SMGC_WDKKQ_False;
                        else if (StepNo == 9)
                            tempRet = PublicData.Dev.PressSet_SMGC_WDGD_False;
                        else
                            tempRet = PublicData.Dev.PressSet_SMDJ_WD_False[StepNo];
                    }
                    else
                    {
                        if (StepNo == 8)
                            tempRet = PublicData.ExpDQ.Exp_SM.SM_GCSJ_KKQ;
                        else if (StepNo == 9)
                            tempRet = PublicData.ExpDQ.Exp_SM.SM_GCSJ_GD;
                        else
                            tempRet = PublicData.Dev.PressSet_SMDJ_WD_Std[StepNo];
                    }
                    break;

                //水密工程波动加压2步27
                case 27:
                    if (PublicData.Dev.IsPressFalse)
                    {
                        if (StepNo == 0)
                            tempRet = 0;
                        else
                            tempRet = PublicData.Dev.PressSet_SMGC_BDPJGD_False;
                    }
                    else
                    {
                        if (StepNo == 0)
                            tempRet = 0;
                        else
                            tempRet = PublicData.ExpDQ.Exp_SM.SM_GCSJ_GD;
                    }
                    break;
                //水密工程波动加压3步28
                case 28:
                    if (PublicData.Dev.IsPressFalse)
                    {
                        if (StepNo == 0)
                            tempRet = 0;
                        else if (StepNo == 1)
                            tempRet = PublicData.Dev.PressSet_SMGC_BDPJKKQ_False;
                        else
                            tempRet = PublicData.Dev.PressSet_SMGC_BDPJGD_False;
                    }
                    else
                    {
                        if (StepNo == 0)
                            tempRet = 0;
                        else if (StepNo == 1)
                            tempRet = PublicData.ExpDQ.Exp_SM.SM_GCSJ_KKQ;
                        else
                            tempRet = PublicData.ExpDQ.Exp_SM.SM_GCSJ_GD;
                    }
                    break;
                //水密工程波动加压9步29
                case 29:
                    if (PublicData.Dev.IsPressFalse)
                    {
                        if (StepNo == 8)
                            tempRet = PublicData.Dev.PressSet_SMGC_BDPJGD_False;
                        else
                            tempRet = PublicData.Dev.PressSet_SMDJ_BDPJ_False[StepNo];
                    }
                    else
                    {
                        if (StepNo == 8)
                            tempRet = PublicData.ExpDQ.Exp_SM.SM_GCSJ_GD;
                        else
                            tempRet = PublicData.Dev.PressSet_SMDJ_WD_Std[StepNo];
                    }
                    break;
                //水密工程波动加压10步30
                case 30:
                    if (PublicData.Dev.IsPressFalse)
                    {
                        if (StepNo == 8)
                            tempRet = PublicData.Dev.PressSet_SMGC_BDPJKKQ_False;
                        else if (StepNo == 9)
                            tempRet = PublicData.Dev.PressSet_SMGC_BDPJGD_False;
                        else
                            tempRet = PublicData.Dev.PressSet_SMDJ_BDPJ_False[StepNo];
                    }
                    else
                    {
                        if (StepNo == 8)
                            tempRet = PublicData.ExpDQ.Exp_SM.SM_GCSJ_KKQ;
                        else if (StepNo == 9)
                            tempRet = PublicData.ExpDQ.Exp_SM.SM_GCSJ_GD;
                        else
                            tempRet = PublicData.Dev.PressSet_SMDJ_WD_Std[StepNo];
                    }
                    break;
                #endregion

                #region 抗风压目标压力
                //抗风压定级变形31，抗风压定级反复32，抗风压定级P3 33，抗风压定级Pmax 34
                case 31:
                    if (PublicData.Dev.IsPressFalse)
                        tempRet = PublicData.Dev.PressSet_KFY_DJBX_False[stageNo][StepNo];
                    else
                        tempRet = PublicData.Dev.PressSet_KFY_DJBX_Std[stageNo][StepNo];
                    break;

                case 32:
                    if (PublicData.Dev.IsPressFalse)
                        tempRet = Math.Abs(PublicData.Dev.PressSet_KFY_DJP2_False);
                    else
                        tempRet = Math.Min(Math.Abs(PublicData.ExpDQ.ExpData_KFY.PDValue_DJP1_Z), Math.Abs(PublicData.ExpDQ.ExpData_KFY.PDValue_DJP1_F)) * PublicData.Dev.PressSet_KFY_DJP2_Raito_Std;
                    break;

                case 33:
                    if (PublicData.Dev.IsPressFalse)
                        tempRet = PublicData.Dev.PressSet_KFY_DJP3_False;
                    else
                    {
                        if (PublicData.ExpDQ.Exp_KFY.P3DownPress)
                            tempRet = PublicData.ExpDQ.Exp_KFY.P3Press;
                        else
                            tempRet = Math.Min(Math.Abs(PublicData.ExpDQ.ExpData_KFY.PDValue_DJP1_Z), Math.Abs(PublicData.ExpDQ.ExpData_KFY.PDValue_DJP1_F)) * PublicData.Dev.PressSet_KFY_DJP3_Raito_Std;
                    }
                    break;

                case 34:
                    if (PublicData.Dev.IsPressFalse)
                        tempRet = PublicData.Dev.PressSet_KFY_DJPmax_False;
                    else
                        tempRet = Math.Min(Math.Abs(PublicData.ExpDQ.ExpData_KFY.PDValue_DJP1_Z), Math.Abs(PublicData.ExpDQ.ExpData_KFY.PDValue_DJP1_F)) * PublicData.Dev.PressSet_KFY_DJPmax_Raito_Std;
                    break;

                //抗风压工程变形35，工程反复36，工程P3 37，工程Pmax 38
                case 35:
                    if (PublicData.Dev.IsPressFalse)
                        tempRet = PublicData.Dev.PressSet_KFY_GCBX_False[stageNo][StepNo];
                    else
                    {
                        if (stageNo == 0)
                            tempRet = PublicData.Dev.PressSet_KFY_GCP1_Y_Std;
                        else if (stageNo == 1)
                        {
                            switch (StepNo)
                            {
                                case 0:
                                    tempRet = PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCP1_Raito_Std[0];
                                    break;
                                case 1:
                                    tempRet = PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCP1_Raito_Std[1];
                                    break;
                                case 2:
                                    tempRet = PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCP1_Raito_Std[2];
                                    break;
                                case 3:
                                    tempRet = PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCP1_Raito_Std[3];
                                    break;
                            }
                        }
                        else if (stageNo == 2)
                            tempRet = -PublicData.Dev.PressSet_KFY_GCP1_Y_Std;
                        else if (stageNo == 3)
                        {
                            switch (StepNo)
                            {
                                case 0:
                                    tempRet = -PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCP1_Raito_Std[0];
                                    break;
                                case 1:
                                    tempRet = -PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCP1_Raito_Std[1];
                                    break;
                                case 2:
                                    tempRet = -PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCP1_Raito_Std[2];
                                    break;
                                case 3:
                                    tempRet = -PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCP1_Raito_Std[3];
                                    break;
                            }
                        }
                    }
                    break;

                case 36:
                    if (PublicData.Dev.IsPressFalse)
                    {
                        if (StepNOInList == 0)
                            tempRet = PublicData.Dev.PressSet_KFY_GCP2_False;
                        else
                            tempRet = -PublicData.Dev.PressSet_KFY_GCP2_False;
                    }
                    else
                    {
                        if (StepNOInList == 0)
                            tempRet = PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCP2_Raito_Std;
                        else
                            tempRet = -PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCP2_Raito_Std;
                    }
                    break;

                case 37:
                    if (PublicData.Dev.IsPressFalse)
                    {
                        if (StepNOInList == 0)
                            tempRet = PublicData.Dev.PressSet_KFY_GCP3_False;
                        else
                            tempRet = -PublicData.Dev.PressSet_KFY_GCP3_False;
                    }
                    else
                    {
                        if (StepNOInList == 0)
                            tempRet = PublicData.ExpDQ.Exp_KFY.P3_GCSJ;
                        else
                            tempRet = -PublicData.ExpDQ.Exp_KFY.P3_GCSJ;
                    }
                    break;

                case 38:
                    if (PublicData.Dev.IsPressFalse)
                    {
                        if (StepNOInList == 0)
                            tempRet = PublicData.Dev.PressSet_KFY_GCPmax_False;
                        else
                            tempRet = -PublicData.Dev.PressSet_KFY_GCPmax_False;
                    }
                    else
                    {
                        if (StepNOInList == 0)
                            tempRet = PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCPmax_Raito_Std;
                        else
                            tempRet = -PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCPmax_Raito_Std;
                    }
                    break;

                    #endregion
            }
            return tempRet;
        }

        /// <summary>
        /// 根据类型和目标压力获取PID参数
        /// </summary>
        /// <param name="type">类型。1气密，2水密风机1，3水密风机2</param>
        /// <param name="aim">给定压力</param>
        /// <returns></returns>
        private PID_ParamModel GePIDParam(int type, double aim)
        {
            PID_ParamModel tempPIDparam = new PID_ParamModel();

            switch (type)
            {
                //气密PID风速管1
                case 1:
                    if ((aim >= 0) && (aim < 75))
                    {
                        tempPIDparam = PublicData.Dev.PID11;
                    }
                    else if ((aim >= 75) && (aim < 125))
                    {
                        tempPIDparam = PublicData.Dev.PID12;
                    }
                    else if ((aim >= 125) && (aim < 175))
                    {
                        tempPIDparam = PublicData.Dev.PID13;
                    }
                    else if ((aim >= 175) && (aim < 600))
                    {
                        tempPIDparam = PublicData.Dev.PID14;
                    }
                    else if (aim >= 600)
                    {
                        tempPIDparam = PublicData.Dev.PID15;
                    }
                    break;
                //水密、抗风压 PID
                case 2:
                    if ((aim >= 0) && (aim < 250))
                    {
                        tempPIDparam = PublicData.Dev.PID21;
                    }
                    else if ((aim >= 250) && (aim < 500))
                    {
                        tempPIDparam = PublicData.Dev.PID22;
                    }
                    else if ((aim >= 500) && (aim < 1000))
                    {
                        tempPIDparam = PublicData.Dev.PID23;
                    }
                    else if ((aim >= 1000) && (aim < 3000))
                    {
                        tempPIDparam = PublicData.Dev.PID24;
                    }
                    else if ((aim >= 3000) && (aim < 5000))
                    {
                        tempPIDparam = PublicData.Dev.PID25;
                    }
                    else if (aim >= 5000)
                    {
                        tempPIDparam = PublicData.Dev.PID26;
                    }
                    break;
                //气密PID风速管2
                case 4:
                    if ((aim >= 0) && (aim < 75))
                    {
                        tempPIDparam = PublicData.Dev.PID41;
                    }
                    else if ((aim >= 75) && (aim < 125))
                    {
                        tempPIDparam = PublicData.Dev.PID42;
                    }
                    else if ((aim >= 125) && (aim < 175))
                    {
                        tempPIDparam = PublicData.Dev.PID43;
                    }
                    else if ((aim >= 175) && (aim < 600))
                    {
                        tempPIDparam = PublicData.Dev.PID44;
                    }
                    else if (aim >= 600)
                    {
                        tempPIDparam = PublicData.Dev.PID45;
                    }
                    break;
                //气密PID风速管3
                case 6:
                    if ((aim >= 0) && (aim < 75))
                    {
                        tempPIDparam = PublicData.Dev.PID61;
                    }
                    else if ((aim >= 75) && (aim < 125))
                    {
                        tempPIDparam = PublicData.Dev.PID62;
                    }
                    else if ((aim >= 125) && (aim < 175))
                    {
                        tempPIDparam = PublicData.Dev.PID63;
                    }
                    else if ((aim >= 175) && (aim < 600))
                    {
                        tempPIDparam = PublicData.Dev.PID64;
                    }
                    else if (aim >= 600)
                    {
                        tempPIDparam = PublicData.Dev.PID65;
                    }
                    break;
                //水流量PID
                case 5:
                    tempPIDparam = PublicData.Dev.PID51;
                    break;
            }
            return tempPIDparam;
        }

        /// <summary>
        /// 根据目标压力获取允许偏差——气密小压力
        /// </summary>
        /// <param name="aim"></param>
        /// <returns></returns>
        private double GetErr(double inAim)
        {
            double tempRet = 0;
            double aim = Math.Abs(inAim);
            if ((aim >= 0) && (aim < 50))
            {
                tempRet = PublicData.Dev.AllowablePressErrQM[0];
            }
            else if ((aim >= 50) && (aim < 100))
            {
                tempRet = PublicData.Dev.AllowablePressErrQM[1];
            }
            else if ((aim >= 100) && (aim < 500))
            {
                tempRet = PublicData.Dev.AllowablePressErrQM[2];
            }
            else
            {
                tempRet = PublicData.Dev.AllowablePressErrQM[3];
            }
            tempRet = Math.Abs(tempRet);
            return tempRet;
        }

        /// <summary>
        /// 大差压控制允许偏差
        /// </summary>
        /// <param name="aim"></param>
        /// <returns></returns>
        private double GetErrBig(double inAim)
        {
            double tempRet = 0;
            double aim = Math.Abs(inAim);
            if ((aim >= 0) && (aim < 1000))
            {
                tempRet = PublicData.Dev.AllowablePressErrBigCY[0];
            }
            else if ((aim >= 1000) && (aim < 3000))
            {
                tempRet = PublicData.Dev.AllowablePressErrBigCY[1];
            }
            else if ((aim >= 3000) && (aim < 5000))
            {
                tempRet = PublicData.Dev.AllowablePressErrBigCY[2];
            }
            else
            {
                tempRet = PublicData.Dev.AllowablePressErrBigCY[3];
            }
            tempRet = Math.Abs(tempRet);
            return tempRet;
        }

        /// <summary>
        /// 获取标准压力
        /// </summary>
        /// <param name="type"></param>
        /// <param name="stageNo"></param>
        /// <param name="StepNo"></param>
        /// <returns></returns>
        private double GetStdPress(int type, int stageNo, int StepNo)
        {
            double tempRet = 0;
            switch (type)
            {
                #region 抗风压目标压力
                //抗风压定级变形31，抗风压定级反复32，抗风压定级P3 33，抗风压定级Pmax 34
                case 31:
                    tempRet = PublicData.Dev.PressSet_KFY_DJBX_Std[stageNo][StepNo];
                    break;

                case 32:
                    tempRet = Math.Min(Math.Abs(PublicData.ExpDQ.ExpData_KFY.PDValue_DJP1_Z), Math.Abs(PublicData.ExpDQ.ExpData_KFY.PDValue_DJP1_F)) * PublicData.Dev.PressSet_KFY_DJP2_Raito_Std;
                    break;

                case 33:
                    if (PublicData.ExpDQ.Exp_KFY.P3DownPress)
                        tempRet = Math.Abs(PublicData.ExpDQ.Exp_KFY.P3Press);
                    else
                        tempRet = Math.Min(Math.Abs(PublicData.ExpDQ.ExpData_KFY.PDValue_DJP1_Z), Math.Abs(PublicData.ExpDQ.ExpData_KFY.PDValue_DJP1_F)) * PublicData.Dev.PressSet_KFY_DJP3_Raito_Std;
                    break;

                case 34:
                    tempRet = Math.Min(Math.Abs(PublicData.ExpDQ.ExpData_KFY.PDValue_DJP1_Z), Math.Abs(PublicData.ExpDQ.ExpData_KFY.PDValue_DJP1_F)) * PublicData.Dev.PressSet_KFY_DJPmax_Raito_Std;
                    break;

                //抗风压工程变形35，工程反复36，工程P3 37，工程Pmax 38
                case 35:

                    if (stageNo == 0)
                        tempRet = PublicData.Dev.PressSet_KFY_GCP1_Y_Std;
                    else if (stageNo == 1)
                    {
                        switch (StepNo)
                        {
                            case 0:
                                tempRet = PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCP1_Raito_Std[0];
                                break;
                            case 1:
                                tempRet = PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCP1_Raito_Std[1];
                                break;
                            case 2:
                                tempRet = PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCP1_Raito_Std[2];
                                break;
                            case 3:
                                tempRet = PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCP1_Raito_Std[3];
                                break;
                        }
                    }
                    else if (stageNo == 2)
                        tempRet = -PublicData.Dev.PressSet_KFY_GCP1_Y_Std;
                    else if (stageNo == 3)
                    {
                        switch (StepNo)
                        {
                            case 0:
                                tempRet = -PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCP1_Raito_Std[0];
                                break;
                            case 1:
                                tempRet = -PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCP1_Raito_Std[1];
                                break;
                            case 2:
                                tempRet = -PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCP1_Raito_Std[2];
                                break;
                            case 3:
                                tempRet = -PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCP1_Raito_Std[3];
                                break;
                        }
                    }

                    break;

                case 36:

                    if (StepNOInList == 0)
                        tempRet = PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCP2_Raito_Std;
                    else
                        tempRet = -PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCP2_Raito_Std;
                    break;

                case 37:
                    if (StepNOInList == 0)
                        tempRet = PublicData.ExpDQ.Exp_KFY.P3_GCSJ;
                    else
                        tempRet = -PublicData.ExpDQ.Exp_KFY.P3_GCSJ;
                    break;

                case 38:
                    if (StepNOInList == 0)
                        tempRet = PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCPmax_Raito_Std;
                    else
                        tempRet = -PublicData.ExpDQ.Exp_KFY.P3_GCSJ * PublicData.Dev.PressSet_KFY_GCPmax_Raito_Std;
                    break;

                    #endregion
            }
            return tempRet;
        }

        #endregion


        #region 试验停机、紧急停机消息处理

        /// <summary>
        /// 紧急停机消息
        /// </summary>
        /// <param name="msg"></param>
        private void JJTJMessage(string msg)
        {
            PublicData.Dev.DeviceRunMode = DevicRunModeType.JJTJ_Mode;
            ModeChange(DevicRunModeType.JJTJ_Mode);

        }

        /// <summary>
        /// 试验停止消息
        /// </summary>
        /// <param name="msg"></param>
        private void ExpStopMessage(int msg)
        {
            if (ExistQMDJExp || ExistQMGCExp || ExistSMWDDJExp || ExistSMWDGCExp || ExistSMBDDJExp || ExistSMBDGCExp || ExistKFYp1DJExp || ExistKFYp1GCExp || ExistKFYp2DJExp || ExistKFYp2GCExp || ExistKFYp3DJExp || ExistKFYp3GCExp || ExistKFYpmaxDJExp || ExistKFYpmaxGCExp ||
                ExistCJBXDJXExp || ExistCJBXGCXExp || ExistCJBXDJYExp || ExistCJBXGCYExp || ExistCJBXDJZExp || ExistCJBXGCZExp)
            {
                EscStopCompleteExpReset();
            }
        }

        #endregion


        #region 通讯读写暂存数据

        /// <summary>
        /// 单次发送数据数组长度
        /// </summary>
        static readonly int _onceOrderDataLenFrmBLL = 24;                                             //参数数组长度

        /// <summary>
        /// 单次发送指令值数组
        /// </summary>
        private ushort[] _onceOrderValuesFrmBLL = Enumerable.Repeat((ushort)0, _onceOrderDataLenFrmBLL).ToArray();
        /// <summary>
        /// 单次发送指令值数组
        /// </summary>
        public ushort[] OnceOrderValuesFrmBLL
        {
            get { return _onceOrderValuesFrmBLL; }
            set
            {
                _onceOrderValuesFrmBLL = value;
                RaisePropertyChanged(() => OnceOrderValuesFrmBLL);
            }
        }

        /// <summary>
        /// PLC程控指令数组长度
        /// </summary>
        static readonly int DataLengthFrmBLLPLCCK = 20;

        /// <summary>
        /// PLC程控指令数组
        /// </summary>
        private ushort[] _plcCMDFrmBLLCK = Enumerable.Repeat((ushort)0, DataLengthFrmBLLPLCCK).ToArray();
        /// <summary>
        /// PLC程控指令数组
        /// </summary>
        public ushort[] PLCCKCMDFrmBLL
        {
            get { return _plcCMDFrmBLLCK; }
            set
            {
                _plcCMDFrmBLLCK = value;
                RaisePropertyChanged(() => PLCCKCMDFrmBLL);
            }
        }


        #endregion


        #region BLL用状态、指令等参数

        /// <summary>
        /// 进程锁，防止多次进入定时器回调
        /// </summary>
        private bool _timerThreadLock = false;
        /// <summary>
        /// 进程锁，防止多次进入定时器回调
        /// </summary>
        public bool TimerThreadLock
        {
            get { return _timerThreadLock; }
            set
            {
                _timerThreadLock = value;
                RaisePropertyChanged(() => TimerThreadLock);
            }
        }

        /// <summary>
        /// 正压检测状态
        /// </summary>
        private bool _pPressTest = false;
        /// <summary>
        /// 正压检测状态
        /// </summary>
        public bool PPressTest
        {
            get { return _pPressTest; }
            set
            {
                _pPressTest = value;
                RaisePropertyChanged(() => PPressTest);
            }
        }

        /// <summary>
        /// 有手动位移单次指令
        /// </summary>
        private bool _onceOrderSDWYDebug = false;
        /// <summary>
        /// 有手动位移单次指令
        /// </summary>
        public bool OnceOrderSDWYDebug
        {
            get { return _onceOrderSDWYDebug; }
            set
            {
                _onceOrderSDWYDebug = value;
                RaisePropertyChanged(() => OnceOrderSDWYDebug);
            }
        }

        #region 阶段、步骤相关

        /// <summary>
        /// 阶段测试开始标志
        /// </summary>
        private bool _isStageTestStarted = false;
        /// <summary>
        /// 阶段测试开始标志
        /// </summary>
        public bool IsStageTestStarted
        {
            get { return _isStageTestStarted; }
            set
            {
                _isStageTestStarted = value;
                RaisePropertyChanged(() => IsStageTestStarted);
            }
        }

        /// <summary>
        /// 当前阶段序号（在list中序号，从0开始）
        /// </summary>
        private int _stageNoInList = -1;
        /// <summary>
        /// 当前阶段序号（在list中序号，从0开始）
        /// </summary>
        public int StageNOInList
        {
            get { return _stageNoInList; }
            set
            {
                _stageNoInList = value;
                RaisePropertyChanged(() => StageNOInList);
            }
        }

        /// <summary>
        /// 当前步骤序号（在list中序号，从0开始）
        /// </summary>
        private int _stepNoInList = -1;
        /// <summary>
        /// 当前步骤序号（在list中序号，从0开始）
        /// </summary>
        public int StepNOInList
        {
            get { return _stepNoInList; }
            set
            {
                _stepNoInList = value;
                RaisePropertyChanged(() => StepNOInList);
            }
        }

        /// <summary>
        /// 步骤前等待开始时间
        /// </summary>
        private DateTime _waitingTimeStart = DateTime.Now;
        /// <summary>
        /// 步骤前等待开始时间
        /// </summary>
        public DateTime WaitingTimeStart
        {
            get { return _waitingTimeStart; }
            set
            {
                _waitingTimeStart = value;
                RaisePropertyChanged(() => WaitingTimeStart);
            }
        }

        /// <summary>
        /// 步骤前等待开始标志
        /// </summary>
        private bool _isWaitingStart = false;
        /// <summary>
        /// 步骤前等待开始标志
        /// </summary>
        public bool IsWaitingStart
        {
            get { return _isWaitingStart; }
            set
            {
                _isWaitingStart = value;
                RaisePropertyChanged(() => IsWaitingStart);
            }
        }

        /// <summary>
        /// 步骤开始时间
        /// </summary>
        private DateTime _stepStartTime = DateTime.Now;
        /// <summary>
        /// 步骤开始时间
        /// </summary>
        public DateTime StepStartTime
        {
            get { return _stepStartTime; }
            set
            {
                _stepStartTime = value;
                RaisePropertyChanged(() => StepStartTime);
            }
        }

        /// <summary>
        /// 步骤开始标志
        /// </summary>
        private bool _isStepStart = false;
        /// <summary>
        /// 步骤开始标志
        /// </summary>
        public bool IsStepStart
        {
            get { return _isStepStart; }
            set
            {
                _isStepStart = value;
                RaisePropertyChanged(() => IsStepStart);
            }
        }

        /// <summary>
        /// 步骤单次指令已发送标志
        /// </summary>
        private bool _isStepOrderSend = false;
        /// <summary>
        /// 步骤单次指令已发送标志
        /// </summary>
        public bool IsStepOrderSend
        {
            get { return _isStepOrderSend; }
            set
            {
                _isStepOrderSend = value;
                RaisePropertyChanged(() => IsStepOrderSend);
            }
        }

        /// <summary>
        /// 步骤是否完成反馈
        /// </summary>
        private bool _stepCompleteStatusBack = false;
        /// <summary>
        /// 步骤是否完成反馈
        /// </summary>
        public bool StepCompleteStatusBack
        {
            get { return _stepCompleteStatusBack; }
            set
            {
                _stepCompleteStatusBack = value;
                RaisePropertyChanged(() => StepCompleteStatusBack);
            }
        }

        #endregion

        #region 波动加压相关

        /// <summary>
        /// 实时输出频率
        /// </summary>
        private double _wavePhOut = 0;
        /// <summary>
        /// 实时输出频率
        /// </summary>
        public double WavePhOut
        {
            get { return _wavePhOut; }
            set
            {
                _wavePhOut = value;
                RaisePropertyChanged(() => WavePhOut);
            }
        }
        #endregion

        #endregion


        #region 公共数据属性


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

        #endregion


        #region 模式切换消息、各窗口进退消息处理

        /// <summary>
        /// 窗口关闭消息处理（装置运行模式复位，数据、状态初始化）
        /// </summary>
        /// <param name="msg"></param>
        private void WindowClosedMessage(string msg)
        {
            if ((msg == MQZH_WinName.DbgWinName) || (msg == MQZH_WinName.QMWinName) || (msg == MQZH_WinName.SMWinName) || (msg == MQZH_WinName.KFYp1Name) || (msg == MQZH_WinName.KFYp2Name) || (msg == MQZH_WinName.KFYp3Name) || (msg == MQZH_WinName.KFYpmaxName) || (msg == MQZH_WinName.CJBXWinName))
            {
                WinCloseReset();
            }
        }


        /// <summary>
        /// 窗口新增消息处理（切换装置运行模式，数据初始化）
        /// </summary>
        /// <param name="msgWindow"></param>
        private void WindowAddedMessage(Window msgWindow)
        {
            if (msgWindow.Name == MQZH_WinName.QMWinName)
            {
                //装置运行模式设置
                if (PublicData.Dev.DeviceRunMode != DevicRunModeType.JJTJ_Mode)
                    ModeChange(DevicRunModeType.QM_Mode);
            }
            else if (msgWindow.Name == MQZH_WinName.SMWinName)
            {
                //装置运行模式设置
                if (PublicData.Dev.DeviceRunMode != DevicRunModeType.JJTJ_Mode)
                    ModeChange(DevicRunModeType.SM_Mode);
            }
            else if (msgWindow.Name == MQZH_WinName.KFYp1Name)
            {
                //装置运行模式设置
                if (PublicData.Dev.DeviceRunMode != DevicRunModeType.JJTJ_Mode)
                    ModeChange(DevicRunModeType.KFYp1_Mode);
            }
            else if (msgWindow.Name == MQZH_WinName.KFYp2Name)
            {
                //装置运行模式设置
                if (PublicData.Dev.DeviceRunMode != DevicRunModeType.JJTJ_Mode)
                    ModeChange(DevicRunModeType.KFYp2_Mode);
            }
            else if (msgWindow.Name == MQZH_WinName.KFYp3Name)
            {
                //装置运行模式设置
                if (PublicData.Dev.DeviceRunMode != DevicRunModeType.JJTJ_Mode)
                    ModeChange(DevicRunModeType.KFYp3_Mode);
            }
            else if (msgWindow.Name == MQZH_WinName.KFYpmaxName)
            {
                //装置运行模式设置
                if (PublicData.Dev.DeviceRunMode != DevicRunModeType.JJTJ_Mode)
                    ModeChange(DevicRunModeType.KFYpmax_Mode);
            }
            else if (msgWindow.Name == MQZH_WinName.CJBXWinName)
            {
                //装置运行模式设置
                if (PublicData.Dev.DeviceRunMode != DevicRunModeType.JJTJ_Mode)
                    ModeChange(DevicRunModeType.CJBX_Mode);
            }
        }


        /// <summary>
        /// 模式切换消息处理
        /// </summary>
        private void ModeChangeMessage(DevicRunModeType msg)
        {
            ModeChange(msg);
        }


        /// <summary>
        /// 模式切换
        /// </summary>
        /// <param name="newMode"></param>
        private void ModeChange(DevicRunModeType newMode)
        {
            try
            {
                if ((newMode != PublicData.Dev.DeviceRunMode) && (PublicData.Dev.DeviceRunMode != DevicRunModeType.JJTJ_Mode))
                {
                    EscStopCompleteExpReset();

                    PublicData.Dev.DeviceRunMode = newMode;
                    foreach (var vDo in PublicData.Dev.DOList)
                        vDo.IsOn = false;

                    //输出频率复位
                    PLCCKCMDFrmBLL[3] = 0;
                    PLCCKCMDFrmBLL[4] = 0;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        #endregion


        #region 初始化、复位方法

        /// <summary>
        /// 取消、停止、完成实验复位
        /// </summary>
        private void EscStopCompleteExpReset()
        {
            try
            {
                //当前阶段、步骤复位
                PublicData.ExpDQ.Exp_QM.StageStepDQReset();
                PublicData.ExpDQ.Exp_SM.StageStepDQReset();
                PublicData.ExpDQ.Exp_KFY.StageStepDQReset();
                //  PublicData.ExpDQ.Exp_CJBX.StageStepDQReset();

                //存在试验状态复位
                ExistExpRest();
                //已选试验复位
                SelectedClear();

                //数组复位
                SDQMDoubleValues = Enumerable.Repeat((double)0, 20).ToArray();
                SDSM1DoubleValues = Enumerable.Repeat((double)0, 20).ToArray();
                SLLPIDDoubleValues = Enumerable.Repeat((double)0, 20).ToArray();

                //单次指令复位
                OnceOrderSDWYDebug = false;
                OnceOrderCJBX = false;
                //严重渗漏、损坏
                DemageOrderKFY = false;
                YZSLOrder = false;
                //阶段测试
                IsStageTestStarted = false;
                StageNOInList = -1;
                //步骤相关
                WaitingTimeStart = DateTime.Now;
                IsWaitingStart = false;
                IsStepOrderSend = false;
                StepStartTime = DateTime.Now;
                IsStepStart = false;
                StepCompleteStatusBack = false;
                StepNOInList = -1;
                //装置忙复位
                PublicData.Dev.IsDeviceBusy = false;
                //PID禁止
                PID_Disable();
                //波动辅助状态
                WavePhOut = 0;
                WavePhOut = 0;
                //赋初值状态
                InitialValueSaved = false;


                //风机、水泵频率输出复位
                PLCCKCMDFrmBLL[3] = 0;
                PLCCKCMDFrmBLL[4] = 0;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        /// <summary>
        /// 各操作窗口关闭时复位
        /// </summary>
        private void WinCloseReset()
        {
            //定时发送指令数据组清零，恢复待机模式
            ModeChange(DevicRunModeType.Wait_Mode);

            EscStopCompleteExpReset();

            Messenger.Default.Send<int>(1299, "StopExpMessage");
        }

        /// <summary>
        /// 所有项目的已选状态清零
        /// </summary>
        private void SelectedClear()
        {
            // 气密试验已选阶段数组清零
            PublicData.ExpDQ.Exp_QM.CanBeCheckInit();
            //水密试验已选阶段数组清零
            PublicData.ExpDQ.Exp_SM.CanBeCheckInit();
            //抗风压
            PublicData.ExpDQ.Exp_KFY.CanBeCheckInit();
            //层间变形被选阶段组清零
            PublicData.ExpDQ.Exp_CJBX.CanBeCheckInit();
        }

        /// <summary>
        /// 各存在试验状态复位
        /// </summary>
        private void ExistExpRest()
        {
            ExistQMDJExp = false;
            ExistQMGCExp = false;

            ExistSMWDDJExp = false;
            ExistSMBDDJExp = false;
            ExistSMWDGCExp = false;
            ExistSMBDGCExp = false;

            ExistKFYp1DJExp = false;
            ExistKFYp2DJExp = false;
            ExistKFYp3DJExp = false;
            ExistKFYpmaxDJExp = false;
            ExistKFYp1GCExp = false;
            ExistKFYp2GCExp = false;
            ExistKFYp3GCExp = false;
            ExistKFYpmaxGCExp = false;

            ExistCJBXDJXExp = false;
            ExistCJBXDJYExp = false;
            ExistCJBXDJZExp = false;
            ExistCJBXGCXExp = false;
            ExistCJBXGCYExp = false;
            ExistCJBXGCZExp = false;
        }

        /// <summary>
        /// PID使能禁止
        /// </summary>
        private void PID_Disable()
        {
            PID_SDQM.PID_Param.ControllerEnable = false;
            PID_QM.PID_Param.ControllerEnable = false;
            PID_SDSM1.PID_Param.ControllerEnable = false;
            PID_SM1.PID_Param.ControllerEnable = false;
            PID_KFY1.PID_Param.ControllerEnable = false;
            PID_SDSLL.PID_Param.ControllerEnable = false;
            PID_SDQM.CalculatePID(0);
            PID_QM.CalculatePID(0);
            PID_SDSM1.CalculatePID(0);
            PID_SM1.CalculatePID(0);
            PID_KFY1.CalculatePID(0);
            PID_SDSLL.CalculatePID(0);
        }

        #endregion

        /// <summary>
        /// 异步停机
        /// </summary>
        public void AsyncStop()
        {
            var task = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                EscStopCompleteExpReset();
            }
            ));
            task.Completed += new EventHandler(Task_Completed);
        }

        /// <summary>
        /// 线程内打开水密定级检测渗漏确认窗口的方法
        /// </summary>
        public void OpenSMSLDJWin()
        {
            var task = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Messenger.Default.Send<string>(MQZH_WinName.SMDJDamageWinName, "OpenGivenNameWin");
            }
            ));
            task.Completed += new EventHandler(Task_Completed);
        }

        /// <summary>
        /// 线程内打开水密工程检测渗漏确认窗口的方法
        /// </summary>
        public void OpenSMSLGCWin()
        {
            var task = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Messenger.Default.Send<string>(MQZH_WinName.SMGCDamageWinName, "OpenGivenNameWin");
            }
            ));
            task.Completed += new EventHandler(Task_Completed);
        }

        /// <summary>
        /// 线程内打开抗风压定级检测损坏确认窗口的方法
        /// </summary>
        public void OpenKFYDamageDJWin()
        {
            var task = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Messenger.Default.Send<string>(MQZH_WinName.KFYDJDamageWinName, "OpenGivenNameWin");
            }
            ));
            task.Completed += new EventHandler(Task_Completed);
        }

        /// <summary>
        /// 线程内打开抗风压工程检测损坏确认窗口的方法
        /// </summary>
        public void OpenKFYDamageGCWin()
        {
            var task = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Messenger.Default.Send<string>(MQZH_WinName.KFYGCDamageWinName, "OpenGivenNameWin");
            }
            ));
            task.Completed += new EventHandler(Task_Completed);
        }

        /// <summary>
        /// 线程内打开层间变形定级故障输入窗口的方法
        /// </summary>
        public void OpenCJBXDemageDJWin()
        {
            var task = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Messenger.Default.Send<string>(MQZH_WinName.CJBXDJDamageWinName, "OpenGivenNameWin");
            }
            ));
            task.Completed += new EventHandler(Task_Completed);
        }

        /// <summary>
        /// 线程内打开层间变形工程故障输入窗口的方法
        /// </summary>
        public void OpenCJBXDemageGCWin()
        {
            var task = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Messenger.Default.Send<string>(MQZH_WinName.CJBXGCDamageWinName, "OpenGivenNameWin");
            }
            ));
            task.Completed += new EventHandler(Task_Completed);
        }

        public void Task_Completed(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 计算并返回ObservableCollection中的绝对值最大值
        /// </summary>
        /// <param name="inCollection"></param>
        /// <returns></returns>

        public double MaxOfObsAbs(ObservableCollection<double> inCollection)
        {
            if ((inCollection == null) || (inCollection.Count == 0))
                return 0;

            double tempMax = Math.Abs(inCollection[0]);
            for (int i = 0; i < inCollection.Count; i++)
            {
                if (Math.Abs(inCollection[i]) > tempMax)
                    tempMax = Math.Abs(inCollection[i]);
            }
            return tempMax;
        }


        public void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(ExitFrames), frame);
            Dispatcher.PushFrame(frame);
        }

        public object ExitFrames(object f)
        {
            ((DispatcherFrame)f).Continue = false;

            return null;
        }
    }
}