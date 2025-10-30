/************************************************************************************
 * 描述：
 * 幕墙四性试验操作业务BLL——总体部分
 * ==================================================================================
 * 修改标记
 * 修改时间			        修改人			版本号			描述
 * 2024/02/01 19:02:11		郝正强			V2.0.0.0        修改为单风机模式，并简化控制逻辑
 * 
 ************************************************************************************/

using System;
using GalaSoft.MvvmLight;
using System.Linq;
using static MQDFJ_MB.Model.MQZH_Enums;
using MQDFJ_MB.Communication;
using CtrlMethod;
using System.Windows;
using NPOI.Util;
using System.Threading.Tasks;
using System.Threading;
using GalaSoft.MvvmLight.Messaging;
using System.Diagnostics;

namespace MQDFJ_MB.BLL
{
    /// <summary>
    /// 主控类
    /// </summary>
    public partial class MQZH_ExpBLL : ObservableObject
    {
        #region 实施程序

        #region 单点调试

        /// <summary>
        /// 单点调试事务
        /// </summary>
        /// <param name="state"></param>
        private void SDDDFunc()
        {
            try
            {
                PublicData.Dev.IsDeviceBusy = true;

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

                Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


        #region 手动调速

        /// <summary>
        /// 蝶阀动作数组
        /// </summary>
        private bool[] _sdtsDF = Enumerable.Repeat((bool)false, 4).ToArray();
        /// <summary>
        /// 蝶阀动作数组
        /// </summary>
        public bool[] SdtsDF
        {
            get { return _sdtsDF; }
            set
            {
                _sdtsDF = value;
                RaisePropertyChanged(() => SdtsDF);
            }
        }


        /// <summary>
        /// 手动正压调速事务
        /// </summary>
        private void SDZTSFunc()
        {
            try
            {
                PublicData.Dev.IsDeviceBusy = true;
                
                PublicData.Dev.DOList[4].IsOn = true;   //开差压电磁阀
                PublicData.Dev.DOList[5].IsOn = true;   //开KM1
                PublicData.Dev.DOList[7].IsOn = true;   //开气泵
                PublicData.Dev.DOList[8].IsOn = true;   //开风机变频

                //蝶阀选择
                PublicData.Dev.DOList[0].IsOn = SdtsDF[0];   //主管蝶阀1
                PublicData.Dev.DOList[1].IsOn = SdtsDF[1];   //粗管蝶阀2
                PublicData.Dev.DOList[2].IsOn = SdtsDF[2];   //细管蝶阀3
                PublicData.Dev.DOList[3].IsOn = SdtsDF[3];   //加粗管蝶阀4

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
                if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                    PLCCKCMDFrmBLL[5] = 5;  //换向阀为正压模式
                PLCCKCMDFrmBLL[19] = 1;     //看门狗

                Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 手动负压调速事务
        /// </summary>
        private void SDFTSFunc()
        {
            try
            {
                PublicData.Dev.IsDeviceBusy = true;

                PublicData.Dev.DOList[4].IsOn = true;   //开差压电磁阀
                PublicData.Dev.DOList[5].IsOn = true;   //开KM1
                PublicData.Dev.DOList[7].IsOn = true;   //开气泵
                PublicData.Dev.DOList[8].IsOn = true;   //开风机变频
                //蝶阀选择
                PublicData.Dev.DOList[0].IsOn = SdtsDF[0];   //主管蝶阀1
                PublicData.Dev.DOList[1].IsOn = SdtsDF[1];   //粗管蝶阀2
                PublicData.Dev.DOList[2].IsOn = SdtsDF[2];   //细管蝶阀3
                PublicData.Dev.DOList[3].IsOn = SdtsDF[3];   //加粗管蝶阀4

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
                if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                    PLCCKCMDFrmBLL[5] = 6;  //换向阀为负压模式
                PLCCKCMDFrmBLL[19] = 1;     //看门狗

                Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 手动水泵调速事务
        /// </summary>
        private void SDSBTSFunc()
        {
            try
            {
                PublicData.Dev.IsDeviceBusy = true;

                PublicData.Dev.DOList[0].IsOn = true;   //开主管蝶阀
                PublicData.Dev.DOList[6].IsOn = true;   //开水泵
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
                if ((!PublicData.Dev.Valve.DIList[00].IsOn) && PublicData.Dev.Valve.DIList[04].IsOn)   //风阀无故障且自检完成时
                    PLCCKCMDFrmBLL[5] = 5;  //换向阀为负压模式
                PLCCKCMDFrmBLL[19] = 1;     //看门狗

                Messenger.Default.Send<ushort[]>(PLCCKCMDFrmBLL, "CKCMDToComMessage");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


        #region 手动气密PID调试

        /// <summary>
        /// 手动气密PID参数数组
        /// </summary>
        private double[] _sdqmDoubleValues = Enumerable.Repeat((double)0, 20).ToArray();
        /// <summary>
        /// 手动气密PID参数数组
        /// </summary>
        public double[] SDQMDoubleValues
        {
            get { return _sdqmDoubleValues; }
            set
            {
                _sdqmDoubleValues = value;
                RaisePropertyChanged(() => SDQMDoubleValues);
            }
        }

        /// <summary>
        /// 手动气密PID
        /// </summary>
        private PID_CtrlModel _pid_SDQM = new PID_CtrlModel();
        /// <summary>
        /// 手动气密PID
        /// </summary>
        public PID_CtrlModel PID_SDQM
        {
            get { return _pid_SDQM; }
            set
            {
                _pid_SDQM = value;
                RaisePropertyChanged(() => PID_SDQM);
            }
        }

        /// <summary>
        /// 手动气密计算事务
        /// </summary>
        /// <param name="state"></param>
        private ushort SDQMFunc()
        {
            try
            {
                PID_SDQM.PID_Param.U_UpperBound = SDQMDoubleValues[3];  //输出上限
                PID_SDQM.PID_Param.U_LowerBound = SDQMDoubleValues[4];  //输出下限
                PID_SDQM.PID_Param.Kp = SDQMDoubleValues[5];    //kp
                PID_SDQM.PID_Param.Ki = SDQMDoubleValues[6];    //ki
                PID_SDQM.PID_Param.Kd = SDQMDoubleValues[7];    //kd
                PID_SDQM.PID_Param.ControllerType = PublicData.Dev.PID41.ControllerType;
                PID_SDQM.PID_Param.ControllerEnable = (Math.Abs(SDQMDoubleValues[10]) > 0.001); //pid使能
                if ((PublicData.Dev.DeviceRunMode == DevicRunModeType.DF1QMZDbg_Mode) ||
                    (PublicData.Dev.DeviceRunMode == DevicRunModeType.DF2QMZDbg_Mode) ||
                    (PublicData.Dev.DeviceRunMode == DevicRunModeType.DF3QMZDbg_Mode) ||
                    (PublicData.Dev.DeviceRunMode == DevicRunModeType.DF4QMZDbg_Mode))
                    PID_SDQM.PID_Param.IsReaction = false;
                else
                    PID_SDQM.PID_Param.IsReaction = true;

                double tempGiven = SDQMDoubleValues[8];  //给定值
                double tempNow;
                //根据给定值的范围选择实测值
                if ((SDQMDoubleValues[8] >= PublicData.Dev.AIList[12].SingalLowerRange * 0.9) && (SDQMDoubleValues[8] <= PublicData.Dev.AIList[12].SingalUpperRange * 0.9))
                {
                    tempNow = PublicData.Dev.AIList[12].ValueFinal;
                }
                else
                {
                    tempNow = PublicData.Dev.AIList[14].ValueFinal;
                }
                double tempErr = tempGiven - tempNow;
                PID_SDQM.CalculatePID(tempErr);

                double tempUK = PID_SDQM.UK;
                if (tempUK <= 0)
                {
                    tempUK = 0;
                }
                if (tempUK >= 32000)
                {
                    tempUK = 32000;
                }

                // Trace.Write("UK:" + PID_SDQM.UK + "  UP:" + PID_SDQM.UK_P + "  UI:" + PID_SDQM.UK_I + "  UD:" + PID_SDQM.UK_D + "\r\n");
                return Convert.ToUInt16(tempUK);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        #endregion


        #region 手动大差压PID调试

        /// <summary>
        /// 手动水密PID参数数组1
        /// </summary>
        private double[] _sdsm1DoubleValues = Enumerable.Repeat((double)0, 20).ToArray();
        /// <summary>
        /// 手动气密PID参数数组1
        /// </summary>
        public double[] SDSM1DoubleValues
        {
            get { return _sdsm1DoubleValues; }
            set
            {
                _sdsm1DoubleValues = value;
                RaisePropertyChanged(() => SDSM1DoubleValues);
            }
        }


        /// <summary>
        /// 手动大差压PID
        /// </summary>
        private PID_CtrlModel _pid_SDSM1 = new PID_CtrlModel();
        /// <summary>
        /// 手动大差压PID
        /// </summary>
        public PID_CtrlModel PID_SDSM1
        {
            get { return _pid_SDSM1; }
            set
            {
                _pid_SDSM1 = value;
                RaisePropertyChanged(() => PID_SDSM1);
            }
        }

        /// <summary>
        /// 手动大压力PID调试事务
        /// </summary>
        private ushort SDSMFunc()
        {
            try
            {
                PID_SDSM1.PID_Param.U_UpperBound = SDSM1DoubleValues[3];  //输出上限
                PID_SDSM1.PID_Param.U_LowerBound = SDSM1DoubleValues[4];  //输出下限
                PID_SDSM1.PID_Param.Kp = SDSM1DoubleValues[5];    //kp
                PID_SDSM1.PID_Param.Ki = SDSM1DoubleValues[6];    //ki
                PID_SDSM1.PID_Param.Kd = SDSM1DoubleValues[7];    //kd
                PID_SDSM1.PID_Param.ControllerType = PublicData.Dev.PID21.ControllerType;
                PID_SDSM1.PID_Param.ControllerEnable = (Math.Abs(SDSM1DoubleValues[10]) > 0.001); //pid使能
                if (PublicData.Dev.DeviceRunMode == DevicRunModeType.SDSMZDbg_Mode)
                    PID_SDSM1.PID_Param.IsReaction = false;
                else
                    PID_SDSM1.PID_Param.IsReaction = true;
                double tempGiven = SDSM1DoubleValues[8];  //给定值
                double tempNow = 0;
                //根据给定值的范围选择实测值
                if (PublicData.Dev.IsWithCYM && (SDSM1DoubleValues[8] >= PublicData.Dev.AIList[13].SingalLowerRange * 0.9) && (SDSM1DoubleValues[8] < PublicData.Dev.AIList[13].SingalUpperRange * 0.9))
                    tempNow = PublicData.Dev.AIList[13].ValueFinal;
                else
                    tempNow = PublicData.Dev.AIList[14].ValueFinal;
                double tempErr = tempGiven - tempNow;
                PID_SDSM1.CalculatePID(tempErr);

                double tempUK = PID_SDSM1.UK;
                if (tempUK <= 0)
                {
                    tempUK = 0;
                }
                if (tempUK >= 32000)
                {
                    tempUK = 32000;
                }

                return Convert.ToUInt16(tempUK);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        #endregion


        #region 手动水流量PID

        /// <summary>
        /// 水流量PID数组
        /// </summary>
        private double[] _sllPIDDoubleValues = Enumerable.Repeat((double)0, 20).ToArray();
        /// <summary>
        /// 水流量PID数组
        /// </summary>
        public double[] SLLPIDDoubleValues
        {
            get { return _sllPIDDoubleValues; }
            set
            {
                _sllPIDDoubleValues = value;
                RaisePropertyChanged(() => SLLPIDDoubleValues);
            }
        }

        /// <summary>
        /// 手动水流量PID
        /// </summary>
        private PID_CtrlModel _pid_SDSLL = new PID_CtrlModel();
        /// <summary>
        /// 手动水流量PID
        /// </summary>
        public PID_CtrlModel PID_SDSLL
        {
            get { return _pid_SDSLL; }
            set
            {
                _pid_SDSLL = value;
                RaisePropertyChanged(() => PID_SDSLL);
            }
        }

        /// <summary>
        /// 水流量PID调试事务
        /// </summary>
        /// <param name="state"></param>
        private ushort SLLPIDFunc()
        {
            try
            {
                PID_SDSLL.PID_Param.U_UpperBound = SLLPIDDoubleValues[3];  //输出上限
                PID_SDSLL.PID_Param.U_LowerBound = SLLPIDDoubleValues[4];  //输出下限
                PID_SDSLL.PID_Param.Kp = SLLPIDDoubleValues[5];    //kp
                PID_SDSLL.PID_Param.Ki = SLLPIDDoubleValues[6];    //ki
                PID_SDSLL.PID_Param.Kd = SLLPIDDoubleValues[7];    //kd
                PID_SDSLL.PID_Param.ControllerType = PublicData.Dev.PID51.ControllerType;
                PID_SDSLL.PID_Param.ControllerEnable = (Math.Abs(SLLPIDDoubleValues[10]) > 0.001); //pid使能
                double tempGiven = Math.Abs(SLLPIDDoubleValues[8]);  //给定值
                double tempNow = Math.Abs(PublicData.Dev.SLL);
                double tempErr = tempGiven - tempNow;
                PID_SDSLL.CalculatePID(tempErr);

                double tempUK = PID_SDSLL.UK;
                if (tempUK <= 0)
                {
                    tempUK = 0;
                }
                if (tempUK >= 32000)
                {
                    tempUK = 32000;
                }
                Trace.Write("UK:" + PID_SDSLL.UK + "  UP:" + PID_SDSLL.UK_P + "  UI:" + PID_SDSLL.UK_I + "  UD:" + PID_SDSLL.UK_D + "\r\n");

                return Convert.ToUInt16(tempUK);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        #endregion


        #endregion


        #region 各类调试控制消息

        /// <summary>
        /// 调试消息——手动单点控制
        /// </summary>
        /// <param name="msg"></param>
        private void SDDDMessage(int msg)
        {
            try
            {
                switch (msg)
                {
                    case 6100:  //主管蝶阀1
                        PublicData.Dev.DOList[0].IsOn = !PublicData.Dev.DOList[0].IsOn;
                        break;
                    case 6101:  //粗管蝶阀2
                        PublicData.Dev.DOList[1].IsOn = !PublicData.Dev.DOList[1].IsOn;
                        break;
                    case 6102:  //细管蝶阀3
                        PublicData.Dev.DOList[2].IsOn = !PublicData.Dev.DOList[2].IsOn;
                        break;
                    case 6103:  //加粗主管蝶阀4
                        PublicData.Dev.DOList[3].IsOn = !PublicData.Dev.DOList[3].IsOn;
                        break;
                    case 6104:  //差压阀
                        PublicData.Dev.DOList[4].IsOn = !PublicData.Dev.DOList[4].IsOn;
                        break;

                    case 6105:  //KM1
                        PublicData.Dev.DOList[05].IsOn = !PublicData.Dev.DOList[05].IsOn;
                        if (!PublicData.Dev.DOList[05].IsOn)
                        {
                            PublicData.Dev.DOList[08].IsOn = false;
                            PLCCKCMDFrmBLL[3] = 0;
                        }
                        break;
                    case 6106:  //水泵启动指令
                        PublicData.Dev.DOList[06].IsOn = !PublicData.Dev.DOList[06].IsOn;
                        break;
                    case 6107:  //气泵启动指令
                        PublicData.Dev.DOList[07].IsOn = !PublicData.Dev.DOList[07].IsOn;
                        break;
                    case 6108:  //风机变频启动指令
                        PublicData.Dev.DOList[08].IsOn = !PublicData.Dev.DOList[08].IsOn;
                        if (!PublicData.Dev.DOList[08].IsOn)
                        {
                            PLCCKCMDFrmBLL[3] = 0;
                        }
                        break;
                    case 6109:  //水泵变频启动指令
                        PublicData.Dev.DOList[09].IsOn = !PublicData.Dev.DOList[09].IsOn;
                        if (!PublicData.Dev.DOList[09].IsOn)
                        {
                            PLCCKCMDFrmBLL[4] = 0;
                        }
                        break;
                    case 6110:  //液压站启动指令
                        PublicData.Dev.DOList[10].IsOn = !PublicData.Dev.DOList[10].IsOn;
                        break;
                    case 6111:  //液压总阀开指令
                        PublicData.Dev.DOList[11].IsOn = !PublicData.Dev.DOList[11].IsOn;
                        break;
                    case 6112:  //X轴向右指令
                        PublicData.Dev.DOList[12].IsOn = !PublicData.Dev.DOList[12].IsOn;
                        if (PublicData.Dev.DOList[12].IsOn)
                            PublicData.Dev.DOList[13].IsOn = false;
                        break;
                    case 6113:  //X轴向左指令
                        PublicData.Dev.DOList[13].IsOn = !PublicData.Dev.DOList[13].IsOn;
                        if (PublicData.Dev.DOList[13].IsOn)
                            PublicData.Dev.DOList[12].IsOn = false;
                        break;
                    case 6114:  //Y轴向前指令
                        PublicData.Dev.DOList[14].IsOn = !PublicData.Dev.DOList[14].IsOn;
                        if (PublicData.Dev.DOList[14].IsOn)
                            PublicData.Dev.DOList[15].IsOn = false;
                        break;
                    case 6115:  //Y轴向后指令
                        PublicData.Dev.DOList[15].IsOn = !PublicData.Dev.DOList[15].IsOn;
                        if (PublicData.Dev.DOList[15].IsOn)
                            PublicData.Dev.DOList[14].IsOn = false;
                        break;
                    case 6116:  //Z轴向上指令
                        PublicData.Dev.DOList[16].IsOn = !PublicData.Dev.DOList[16].IsOn;
                        if (PublicData.Dev.DOList[16].IsOn)
                            PublicData.Dev.DOList[17].IsOn = false;
                        break;
                    case 6117:  //Z轴向下指令
                        PublicData.Dev.DOList[17].IsOn = !PublicData.Dev.DOList[17].IsOn;
                        if (PublicData.Dev.DOList[17].IsOn)
                            PublicData.Dev.DOList[16].IsOn = false;
                        break;

                    case 6118:  //风阀正压指令
                        if ((!PublicData.Dev.Valve.DIList[00].IsOn) && (PublicData.Dev.Valve.DIList[4].IsOn))   //风阀无故障且自检完成时
                            PLCCKCMDFrmBLL[5] = 5;  //换向阀为正压模式
                        break;
                    case 6119:  //风阀负压指令
                        if ((!PublicData.Dev.Valve.DIList[00].IsOn) && (PublicData.Dev.Valve.DIList[4].IsOn))   //风阀无故障且自检完成时
                            PLCCKCMDFrmBLL[5] = 6;  //换向阀为负压模式
                        break;
                    case 6120:  //风阀自检指令
                        Task.Factory.StartNew(() =>
                        {
                            if (!PublicData.Dev.Valve.DIList[00].IsOn)     //有换向阀自检指令，且风阀无故障
                                PLCCKCMDFrmBLL[5] = 3;  //换向阀为自检模式
                            Thread.Sleep(2000);
                            PLCCKCMDFrmBLL[5] = 0;
                        });
                        break;
                    case 6121:  //风阀复位指令
                        Task.Factory.StartNew(() =>
                        {
                            if (PublicData.Dev.Valve.DIList[00].IsOn)    //有换向阀复位指令
                                PLCCKCMDFrmBLL[5] = 88;  //换向阀为复位模式
                            Thread.Sleep(2000);
                            PLCCKCMDFrmBLL[5] = 0;
                        });
                        break;
                    case 6122:  //风阀停机指令
                        PLCCKCMDFrmBLL[5] = 1;  //换向阀为复位模式
                        break;
                    case 6123:  //风阀持续正转指令
                        if (!PublicData.Dev.Valve.DIList[00].IsOn)    //风阀有故障
                            PLCCKCMDFrmBLL[5] = 21;  //换向阀为复位模式
                        break;
                    case 6124:  //风阀持续反转指令
                        if (!PublicData.Dev.Valve.DIList[00].IsOn)    //风阀有故障
                            PLCCKCMDFrmBLL[5] = 22;  //换向阀为复位模式
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        /// <summary>
        /// 调试消息——手动调速
        /// </summary>
        /// <param name="msg"></param>
        private void SDTSMessage(ushort[] msg)
        {
            try
            {
                if (PublicData.Dev.DeviceRunMode == DevicRunModeType.SDZTSDbg_Mode)
                {
                    PLCCKCMDFrmBLL[3] = PublicData.Dev.Valve.DIList[5].IsOn ? msg[4] : (ushort)0;   //正压模式完成后输出频率
                    SdtsDF[0] = msg[0] == 1 ? true : false;
                    SdtsDF[1] = msg[1] == 1 ? true : false;
                    SdtsDF[2] = msg[2] == 1 ? true : false;
                    SdtsDF[3] = msg[3] == 1 ? true : false;
                }

                if (PublicData.Dev.DeviceRunMode == DevicRunModeType.SDFTSDbg_Mode)
                {
                    PLCCKCMDFrmBLL[3] = PublicData.Dev.Valve.DIList[6].IsOn ? msg[4] : (ushort)0;   //负压模式完成后输出频率
                    SdtsDF[0] = msg[0] == 1 ? true : false;
                    SdtsDF[1] = msg[1] == 1 ? true : false;
                    SdtsDF[2] = msg[2] == 1 ? true : false;
                    SdtsDF[3] = msg[3] == 1 ? true : false;
                }


                else if (PublicData.Dev.DeviceRunMode == DevicRunModeType.SDSBTSDbg_Mode)
                    PLCCKCMDFrmBLL[4] = msg[4];   //手动水泵调速模式输出频率
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        /// <summary>
        /// 调试手动气密PID调试消息
        /// </summary>
        /// <param name="msg"></param>
        private void SDQMMessage(double[] msg)
        {
            SDQMDoubleValues = (double[])msg.Clone();
        }


        /// <summary>
        /// 调试手动水密调试消息（大差压）
        /// </summary>
        /// <param name="msg"></param>
        private void SDSMMessage(double[] msg)
        {
            SDSM1DoubleValues = (double[])msg.Clone();
        }


        /// <summary>
        /// 调试水流量PID数据消息
        /// </summary>
        /// <param name="msg"></param>
        private void SLLPIDMessage(double[] msg)
        {
            SLLPIDDoubleValues = (double[])msg.Clone();
        }


        /// <summary>
        /// 调试手动位移控制消息
        /// </summary>
        /// <param name="msg"></param>
        private void DbgSDWYMessage(ushort[] msg)
        {
            OnceOrderValuesFrmBLL = msg;
            OnceOrderSDWYDebug = true;
        }

        #endregion

    }
}