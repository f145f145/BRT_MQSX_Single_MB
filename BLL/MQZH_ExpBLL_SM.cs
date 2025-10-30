/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/11/20 19:02:11
 * 描述：
 * 幕墙四性试验操作业务BLL——水密部分
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2021/11/20       19:02:11		郝正强			V1.0.0.0
 * 
 ************************************************************************************/

using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using GalaSoft.MvvmLight;
using MQDFJ_MB.Model.Exp;
using System.Windows;
using static MQDFJ_MB.Model.MQZH_Enums;
using CtrlMethod;
using GalaSoft.MvvmLight.Messaging;
using MQDFJ_MB.Model;
using System.Threading;
using NPOI.SS.Formula.Functions;

namespace MQDFJ_MB.BLL
{
    /// <summary>
    /// 主控类
    /// </summary>
    public partial class MQZH_ExpBLL : ObservableObject
    {
        #region 水密阶段试验实施程序

        /// <summary>
        /// 水密PID风机1
        /// </summary>
        private PID_CtrlModel _pid_SM1 = new PID_CtrlModel();
        /// <summary>
        /// 水密PID风机1
        /// </summary>
        public PID_CtrlModel PID_SM1
        {
            get { return _pid_SM1; }
            set
            {
                _pid_SM1 = value;
                RaisePropertyChanged(() => PID_SM1);
            }
        }

        /// <summary>
        /// 水密喷淋水流量PID
        /// </summary>
        private PID_CtrlModel _pid_SMSLL = new PID_CtrlModel();
        /// <summary>
        /// 水密喷淋水流量PID
        /// </summary>
        public PID_CtrlModel PID_SMSLL
        {
            get { return _pid_SMSLL; }
            set
            {
                _pid_SMSLL = value;
                RaisePropertyChanged(() => PID_SMSLL);
            }
        }

        /// <summary>
        /// 水密定级稳定检测事务
        /// </summary>
        private void SMWDDJStageFunc()
        {
            double tempValueNow = 0;        //当前值
            double tempGiven = 0;           //给定值
            double tempErr;                 //PID计算用误差
            double tempPermitErr;           //偏差允许范围
            double tempAim ;                //阶段控制目标
            bool tempStageComplete = true;  //阶段完成
            double tempStepPressKeepTime = 0;   //步骤保持时间
            int tempType;                   //阶段类型

            try
            {
                PublicData.Dev.IsDeviceBusy = true;
                //如果当前阶段是默认阶段，则复位状态
                if ((PublicData.ExpDQ.Exp_SM.Stage_DQ.Stage_NO == "99") || (StageNOInList == -1))
                {
                    MessageBox.Show("默认阶段不能做实验");
                    EscStopCompleteExpReset();
                    return;
                }
                //等待换向阀状态到位
                if (PPressTest && (!PublicData.Dev.Valve.DIList[5].IsOn))
                {
                    PLCCKCMDFrmBLL[3] = 0;
                    return;
                }

                //开始前提醒
                if (PublicData.ExpDQ.Exp_SM.Stage_DQ.IsNeedTipsBefore &&
                    (!PublicData.ExpDQ.Exp_SM.Stage_DQ.IsTipsBeforeComplete))
                {
                    MessageBox.Show(PublicData.ExpDQ.Exp_SM.Stage_DQ.StringTipsBefore, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    PublicData.ExpDQ.Exp_SM.Stage_DQ.IsTipsBeforeComplete = true;
                }

                //自动开启喷淋
                if (PublicData.Dev.IsWithSLL && (StageNOInList != 0) && PublicData.Dev.DOList[6].IsOn)
                {
                    PublicData.Dev.DOList[9].IsOn = true;   //开水泵变频
                    //喷淋流量计算稳定
                    SMPLFunc();
                    //控制输出
                    double tempUKLL_SMWD = PID_SMSLL.UK;
                    if (tempUKLL_SMWD <= 0)
                    {
                        tempUKLL_SMWD = 0;
                    }
                    if (tempUKLL_SMWD >= 32000)
                    {
                        tempUKLL_SMWD = 32000;
                    }
                    PLCCKCMDFrmBLL[4] = Convert.ToUInt16(tempUKLL_SMWD);
                }
                else
                {
                    PublicData.Dev.DOList[9].IsOn = false;   //关水泵变频
                    PLCCKCMDFrmBLL[4] = 0;
                }

                //逐个步骤计算给定值
                int stepsCount = PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList.Count;
                for (int i = 0; i < stepsCount; i++)
                {
                    //数据准备--------------------------------
                    StepNOInList = i;
                    //步骤压力稳定时间
                    if (StageNOInList == 0)
                        tempStepPressKeepTime = PublicData.Dev.KeepingTime_YJY;
                    else if (StageNOInList == 1)
                    {
                        if (StepNOInList == 0)
                            tempStepPressKeepTime = PublicData.Dev.KeepingTime_SMFistStep;
                        else
                            tempStepPressKeepTime = PublicData.Dev.KeepingTime_SMDJ_StepLeft;
                    }
                    //步骤稳压目标值
                    if (StageNOInList == 0)
                        tempType = 20;
                    else
                        tempType = 21;
                    tempAim = GetAimPress(tempType, StageNOInList, StepNOInList);
                    tempAim = Math.Abs(tempAim);
                    PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[i].SteadyLoadAimValue = tempAim;
                    //获取偏差允许范围
                    tempPermitErr = GetErrBig(tempAim);
                    //根据目标压力选择实测值，有中压差，且目标压力小于中压差量程上限的90%时用中压差
                    if ((PublicData.Dev.IsWithCYM) && (tempAim < Math.Abs(PublicData.Dev.AIList[13].SingalUpperRange) * 0.9))
                        tempValueNow = Math.Abs(PublicData.Dev.AIList[13].ValueFinal);
                    else
                        tempValueNow = Math.Abs(PublicData.Dev.AIList[14].ValueFinal);
                    tempGiven = tempValueNow;

                    if (!PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[i].IsStepCompleted)
                    {
                        tempStageComplete = false;
                        PublicData.ExpDQ.Exp_SM.Step_DQ = PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[i];

                        //步骤开始前等待
                        if (!PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaitBeforCompleted)
                        {
                            if (!PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaitStarted)
                            {
                                PublicData.ExpDQ.Exp_SM.Step_DQ.WaitStartTime = DateTime.Now;
                                PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaitStarted = true;
                            }
                            TimeSpan tempSpanWait = DateTime.Now - PublicData.ExpDQ.Exp_SM.Step_DQ.WaitStartTime;
                            if (tempSpanWait.TotalSeconds >= PublicData.ExpDQ.Exp_SM.Step_DQ.TimeWaitBefor)
                            {
                                PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaitBeforCompleted = true;
                                break;
                            }
                            //等待未完成或切换瞬间，保持PID输出
                            else
                            {
                                tempGiven = 0;
                                break;
                            }
                        }

                        //压力加载
                        if (!PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.IsUpCompleted)
                        {
                            if (!PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.IsUpStarted)
                            {
                                PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.UpStartTime = DateTime.Now;
                                PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.IsUpStarted = true;
                                PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.LoadUpStartValue = tempValueNow;
                            }
                            TimeSpan tempSpanUp = DateTime.Now - PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.UpStartTime;
                            tempGiven = PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.LoadUpStartValue +
                                        Math.Abs(PublicData.Dev.LoadUpDownSpeed) * tempSpanUp.TotalSeconds;
                            //判断压力给定值增加是否结束
                            if (tempGiven >= tempAim)
                            {
                                PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.IsUpCompleted = true;
                                tempGiven = tempAim;
                            }
                            break;
                        }

                        //压力保持
                        if (!PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.IsKeepPressCompleted)
                        {
                            tempGiven = tempAim;
                            //判断是否进入压力保持范围
                            if ((tempValueNow >= (tempAim - tempPermitErr)) && (tempValueNow <= (tempAim + tempPermitErr)))
                            {
                                if (!PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.IsKeepPressStarted)
                                {
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.IsKeepPressStarted = true;
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.KeepPressStartTime = DateTime.Now;
                                }
                                TimeSpan tempSpanKeep = DateTime.Now - PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.KeepPressStartTime;
                                PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.PressKeeppingTimes = tempSpanKeep.TotalSeconds;
                                PublicData.ExpDQ.Exp_SM.Step_DQ.WaveTimePased = tempSpanKeep.TotalSeconds;
                                //判定是否完成压力保持
                                if (tempSpanKeep.TotalSeconds >= tempStepPressKeepTime)
                                {
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.IsKeepPressCompleted = true;
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.IsStepCompleted = true;
                                }
                            }
                            break;
                        }
                    }
                }
                if (tempGiven < 0)
                    tempGiven = 0;
                tempErr = tempGiven - tempValueNow;
                PID_ParamModel tempPIDParam = GePIDParam(2, tempGiven);
                //严重渗漏时
                if (YZSLOrder)
                    tempStageComplete = true;
                //PID计算，分析状态。
                //如果所有步骤均完成，则阶段完成
                if (tempStageComplete)
                {
                    PublicData.ExpDQ.Exp_SM.Stage_DQ.CompleteStatus = true;
                    PID_SM1.PID_Param.ControllerEnable = false;
                    PID_SM1.CalculatePID(0);
                }
                else
                {
                    PID_SM1.PID_Param = tempPIDParam;
                    if (Math.Abs(tempGiven) < 0.01)
                        PID_SM1.PID_Param.ControllerEnable = false; //预设压力为0时屏蔽pid
                    else
                        PID_SM1.PID_Param.ControllerEnable = true; //pid使能
                    PID_SM1.CalculatePID(tempErr);

                    //准备阶段
                    if ((StageNOInList == 0) && (!PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaitBeforCompleted))
                    {
                        PID_SM1.PID_Param.ControllerEnable = false;
                        PID_SM1.CalculatePID(0);
                    }
                }
                //控制输出
                double tempUK_SMWD = PID_SM1.UK;
                if (tempUK_SMWD <= 0)
                {
                    tempUK_SMWD = 0;
                }
                if (tempUK_SMWD >= 32000)
                {
                    tempUK_SMWD = 32000;
                }
                PLCCKCMDFrmBLL[3] = Convert.ToUInt16(tempUK_SMWD);

                //string kpidStr = "kp:" + PID_SM1.PID_Param.Kp.ToString("0.00") + "ki:" +
                //                 PID_SM1.PID_Param.Ki.ToString("0.00") + "kd:" + PID_SM1.PID_Param.Kd.ToString("0.00");
                //string aimStr = tempGiven.ToString("0.00");
                //string ukStr = PID_SM1.UK.ToString("0.00");
                //string ukpStr = PID_SM1.UK_P.ToString("0.00");
                //string ukiStr = PID_SM1.UK_I.ToString("0.00");
                //string ukdStr = PID_SM1.UK_D.ToString("0.00");
                //Messenger.Default.Send<string>(kpidStr + "  Given:" + aimStr + "  ,U:" + ukStr + "  ,Up:" + ukpStr + "  ,Ui:" + ukiStr + "  ,Ud:" + ukdStr, "PIDinfoMessage");

                //判断是否完成
                if (PublicData.ExpDQ.Exp_SM.Stage_DQ.CompleteStatus)
                {
                    //分析是否所有阶段都完成
                    bool tempAllStageComplete = true;
                    for (int i = 0; i < PublicData.ExpDQ.Exp_SM.StageList_SMDJ.Count; i++)
                    {
                        if (!PublicData.ExpDQ.Exp_SM.StageList_SMDJ[i].CompleteStatus)
                            tempAllStageComplete = false;
                    }
                    if (tempAllStageComplete)
                        PublicData.ExpDQ.Exp_SM.CompleteStatus = true;
                    //保存进度和数据
                    for (int i = 0; i < PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList.Count; i++)
                    {
                        PublicData.ExpDQ.ExpData_SM.Press_DJ[i] = PublicData.Dev.PressSet_SMDJ_WD_Std[i];
                    }
                    Messenger.Default.Send<string>("SaveSMStatusAndData", "SaveExpMessage");
                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        Messenger.Default.Send<string>(PublicData.ExpDQ.Exp_SM.Stage_DQ.Stage_Name + "已完成！", "OpenPrompt");
                    }));
                    //非预加压时，打开渗漏水确认窗口
                    if (StageNOInList > 0)
                    {
                        PublicData.ExpDQ.ExpData_SM.SLStatusCopy();
                        OpenSMSLDJWin();
                    }
                    Thread.Sleep(2000);
                    PublicData.ExpDQ.Exp_SM.Stage_DQ = new MQZH_StageModel_QSM();
                    PublicData.ExpDQ.Exp_SM.Step_DQ = new MQZH_StepModel_QSM();
                    EscStopCompleteExpReset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 水密工程稳定检测事务
        /// </summary>
        private void SMWDGCStageFunc()
        {
            double tempValueNow = 0;            //当前值
            double tempGiven = 0;               //给定值
            double tempErr ;                    //PID计算用误差
            double tempPermitErr;               //偏差允许范围
            double tempAim = 0;                 //阶段控制目标
            bool tempStageComplete = true;      //阶段完成
            double tempStepPressKeepTime = 0;   //步骤保持时间
            int tempType ;                      //步骤类型

            try
            {
                PublicData.Dev.IsDeviceBusy = true;
                //如果当前阶段是默认阶段，则复位状态
                if ((PublicData.ExpDQ.Exp_SM.Stage_DQ.Stage_NO == "99") || (StageNOInList == -1))
                {
                    MessageBox.Show("默认阶段不能做实验");
                    EscStopCompleteExpReset();
                }
                //等待换向阀状态到位
                if (PPressTest && (!PublicData.Dev.Valve.DIList[5].IsOn))
                {
                    PLCCKCMDFrmBLL[3] = 0;
                    return;
                }

                //开始前提醒
                if (PublicData.ExpDQ.Exp_SM.Stage_DQ.IsNeedTipsBefore &&
                    (!PublicData.ExpDQ.Exp_SM.Stage_DQ.IsTipsBeforeComplete))
                {
                    MessageBox.Show(PublicData.ExpDQ.Exp_SM.Stage_DQ.StringTipsBefore, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    PublicData.ExpDQ.Exp_SM.Stage_DQ.IsTipsBeforeComplete = true;
                }

                //自动开启喷淋
                if (PublicData.Dev.IsWithSLL && (StageNOInList != 0) && PublicData.Dev.DOList[6].IsOn)
                {
                    PublicData.Dev.DOList[9].IsOn = true;   //开水泵变频
                    //喷淋流量计算稳定
                    SMPLFunc();
                    //控制输出
                    double tempUKLL_SMWD = PID_SMSLL.UK;
                    if (tempUKLL_SMWD <= 0)
                    {
                        tempUKLL_SMWD = 0;
                    }
                    if (tempUKLL_SMWD >= 32000)
                    {
                        tempUKLL_SMWD = 32000;
                    }
                    PLCCKCMDFrmBLL[4] = Convert.ToUInt16(tempUKLL_SMWD);
                }
                else
                {
                    PublicData.Dev.DOList[9].IsOn = false;   //关水泵变频
                    PLCCKCMDFrmBLL[4] = 0;
                }

                //逐个步骤计算给定值
                int stepsCount = PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList.Count;
                int startStep = 0;
                if ((PublicData.Dev.PlTime_SMGC_Before < 1) && (StageNOInList != 0))
                {
                    startStep = 1;
                    PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[0].IsStepCompleted = true;
                }
                for (int i = startStep; i < stepsCount; i++)
                {
                    //数据准备--------------------------------
                    StepNOInList = i;
                    //步骤压力稳定时间
                    if (StageNOInList == 0)
                        tempStepPressKeepTime = PublicData.Dev.KeepingTime_YJY;

                    else if (StageNOInList == 1)
                    {
                        if (PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList.Count == 2)     //无可开启部分，设计压力小于2000
                        {
                            if (StepNOInList == 0)
                                tempStepPressKeepTime = PublicData.Dev.PlTime_SMGC_Before;
                            else
                                tempStepPressKeepTime = PublicData.Dev.KeepingTime_SMGC_WKQ;
                        }
                        else if (PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList.Count == 3)    //有可开启部分，设计压力均小于2000
                        {
                            if (StepNOInList == 0)
                                tempStepPressKeepTime = PublicData.Dev.PlTime_SMGC_Before;
                            else if (StepNOInList == 1)
                                tempStepPressKeepTime = PublicData.Dev.KeepingTime_SMGC_KKQ;
                            else
                                tempStepPressKeepTime = PublicData.Dev.KeepingTime_SMGC_YKQ;
                        }
                        else if (PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList.Count == 9)        //固定部分设计压力大于2000
                        {
                            if (StepNOInList == 0)
                                tempStepPressKeepTime = PublicData.Dev.PlTime_SMGC_Before;
                            else if (StepNOInList == 8)
                                tempStepPressKeepTime = PublicData.Dev.KeepingTime_SMGC_YKQ;
                            else
                                tempStepPressKeepTime = PublicData.Dev.KeepingTime_SMDJ_StepLeft;
                        }
                        //有可开启部分，且两部分设计压力均大于2000
                        else
                        {
                            if (StepNOInList == 0)
                                tempStepPressKeepTime = PublicData.Dev.PlTime_SMGC_Before;
                            else if (StepNOInList == 8)
                                tempStepPressKeepTime = PublicData.Dev.KeepingTime_SMGC_KKQ;
                            else if (StepNOInList == 9)
                                tempStepPressKeepTime = PublicData.Dev.KeepingTime_SMGC_YKQ;
                            else
                                tempStepPressKeepTime = PublicData.Dev.KeepingTime_SMDJ_StepLeft;
                        }
                    }
                    //步骤稳压目标值
                    if (StageNOInList == 0)
                    {
                        tempType = 20;
                        tempAim = GetAimPress(tempType, StageNOInList, StepNOInList);
                    }
                    else if (StageNOInList == 1)
                    {
                        //水密工程稳定2步23，水密工程稳定3步24，水密工程稳定9步25，水密工程稳定10步26；
                        if (PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList.Count == 2)
                        {
                            tempType = 23;
                            tempAim = GetAimPress(tempType, StageNOInList, StepNOInList);
                        }
                        else if
                            (PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList.Count == 3)
                        {
                            tempType = 24;
                            tempAim = GetAimPress(tempType, StageNOInList, StepNOInList);
                        }
                        else if (PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList.Count == 9)
                        {
                            tempType = 25;
                            tempAim = GetAimPress(tempType, StageNOInList, StepNOInList);
                        }
                        else if (PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList.Count == 10)
                        {
                            tempType = 26;
                            tempAim = GetAimPress(tempType, StageNOInList, StepNOInList);
                        }
                    }

                    tempAim = Math.Abs(tempAim);
                    //获取偏差允许范围
                    tempPermitErr = GetErrBig(tempAim);
                    //根据目标压力选择实测值，有中压差，且目标压力小于中压差量程上限的90%时用中压差
                    if (PublicData.Dev.IsWithCYM && (tempAim < Math.Abs(PublicData.Dev.AIList[13].SingalUpperRange) * 0.9))
                        tempValueNow = Math.Abs(PublicData.Dev.AIList[13].ValueFinal);
                    else
                        tempValueNow = Math.Abs(PublicData.Dev.AIList[14].ValueFinal);
                    tempGiven = tempValueNow;

                    if (!PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[i].IsStepCompleted)
                    {
                        tempStageComplete = false;
                        PublicData.ExpDQ.Exp_SM.Step_DQ = PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[i];
                 
                        
                        //步骤开始前等待
                        if (!PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaitBeforCompleted)
                        {
                            if (!PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaitStarted)
                            {
                                PublicData.ExpDQ.Exp_SM.Step_DQ.WaitStartTime = DateTime.Now;
                                PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaitStarted = true;
                            }
                            TimeSpan tempSpanWait = DateTime.Now - PublicData.ExpDQ.Exp_SM.Step_DQ.WaitStartTime;
                            if (tempSpanWait.TotalSeconds >= PublicData.ExpDQ.Exp_SM.Step_DQ.TimeWaitBefor)
                            {
                                PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaitBeforCompleted = true;
                                break;
                            }
                            //等待未完成或切换瞬间，保持PID输出
                            else
                            {
                                tempGiven = 0;
                                break;
                            }
                        }

                        //压力加载
                        if (!PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.IsUpCompleted)
                        {
                            if (!PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.IsUpStarted)
                            {
                                PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.UpStartTime = DateTime.Now;
                                PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.IsUpStarted = true;
                                PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.LoadUpStartValue = tempValueNow;
                            }
                            TimeSpan tempSpanUp = DateTime.Now - PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.UpStartTime;
                            tempGiven = PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.LoadUpStartValue +
                                        Math.Abs(PublicData.Dev.LoadUpDownSpeed) * tempSpanUp.TotalSeconds;
                            //判断压力给定值增加是否结束
                            if (tempGiven >= tempAim)
                            {
                                PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.IsUpCompleted = true;
                                tempGiven = tempAim;
                            }
                            break;
                        }

                        //压力保持
                        if (!PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.IsKeepPressCompleted)
                        {
                            tempGiven = tempAim;
                            //判断是否进入压力保持范围
                            if ((tempValueNow >= (tempAim - tempPermitErr)) && (tempValueNow <= (tempAim + tempPermitErr)))
                            {
                                if (!PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.IsKeepPressStarted)
                                {
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.IsKeepPressStarted = true;
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.KeepPressStartTime = DateTime.Now;
                                }
                                TimeSpan tempSpanKeep = DateTime.Now - PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.KeepPressStartTime;
                                PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.PressKeeppingTimes = tempSpanKeep.TotalSeconds;
                                PublicData.ExpDQ.Exp_SM.Step_DQ.WaveTimePased = tempSpanKeep.TotalSeconds;

                                Trace.Write("稳定目标" + tempStepPressKeepTime + "当前" + PublicData.ExpDQ.Exp_SM.Step_DQ.WaveTimePased + "\r\n");

                                //判定是否完成压力保持
                                if (tempSpanKeep.TotalSeconds >= tempStepPressKeepTime)
                                {
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.StepSteadyPStatus.IsKeepPressCompleted = true;
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.IsStepCompleted = true;
                                }
                            }
                            break;
                        }
                    }
                }
                if (tempGiven < 0)
                    tempGiven = 0;
                tempErr = tempGiven - tempValueNow;
                PID_ParamModel tempPIDParam = GePIDParam(2, tempGiven);
                //严重渗漏时
                if (YZSLOrder)
                    tempStageComplete = true;
                //PID计算，分析状态。
                //如果所有步骤均完成，则阶段完成
                if (tempStageComplete)
                {
                    PublicData.ExpDQ.Exp_SM.Stage_DQ.CompleteStatus = true;
                    PID_SM1.PID_Param.ControllerEnable = false;
                    PID_SM1.CalculatePID(0);
                }
                else
                {
                    PID_SM1.PID_Param = tempPIDParam;
                    if (Math.Abs(tempGiven) < 0.01)
                        PID_SM1.PID_Param.ControllerEnable = false; //预设压力为0时屏蔽pid
                    else
                        PID_SM1.PID_Param.ControllerEnable = true; //pid使能
                    PID_SM1.CalculatePID(tempErr);

                    //准备阶段
                    if ((StageNOInList == 0) && (!PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaitBeforCompleted))
                    {
                        PID_SM1.PID_Param.ControllerEnable = false;
                        PID_SM1.CalculatePID(0);
                    }
                }
                //控制输出
                double tempUK_SMWD = PID_SM1.UK;
                if (tempUK_SMWD <= 0)
                {
                    tempUK_SMWD = 0;
                }
                if (tempUK_SMWD >= 32000)
                {
                    tempUK_SMWD = 32000;
                }
                PLCCKCMDFrmBLL[3] = Convert.ToUInt16(tempUK_SMWD);

                ////PID计算数据显示
                //string kpidStr = "kp:" + PID_SM1.PID_Param.Kp.ToString("0.00") + "ki:" +
                //                 PID_SM1.PID_Param.Ki.ToString("0.00") + "kd:" + PID_SM1.PID_Param.Kd.ToString("0.00");
                //string aimStr = tempGiven.ToString("0.00");
                //string ukStr = PID_SM1.UK.ToString("0.00");
                //string ukpStr = PID_SM1.UK_P.ToString("0.00");
                //string ukiStr = PID_SM1.UK_I.ToString("0.00");
                //string ukdStr = PID_SM1.UK_D.ToString("0.00");
                //Messenger.Default.Send<string>(kpidStr + "  Given:" + aimStr + "  ,U:" + ukStr + "  ,Up:" + ukpStr + "  ,Ui:" + ukiStr + "  ,Ud:" + ukdStr, "PIDinfoMessage");

                //判断是否完成
                if (PublicData.ExpDQ.Exp_SM.Stage_DQ.CompleteStatus)
                {
                    //分析是否所有阶段都完成
                    bool tempAllStageComplete = true;
                    for (int i = 0; i < PublicData.ExpDQ.Exp_SM.StageList_SMGC.Count; i++)
                    {
                        if (!PublicData.ExpDQ.Exp_SM.StageList_SMGC[i].CompleteStatus)
                            tempAllStageComplete = false;
                    }
                    if (tempAllStageComplete)
                        PublicData.ExpDQ.Exp_SM.CompleteStatus = true;
                    //保存进度和数据
                    if (PublicData.ExpDQ.Exp_SM.Stage_DQ.Stage_NO == TestStageType.SM_GC_J.ToString())
                    {
                        PublicData.ExpDQ.ExpData_SM.Press_GC[0] = PublicData.ExpDQ.Exp_SM.SM_GCSJ_KKQ;
                        PublicData.ExpDQ.ExpData_SM.Press_GC[1] = PublicData.ExpDQ.Exp_SM.SM_GCSJ_GD;
                    }
                    Messenger.Default.Send<string>("SaveSMStatusAndData", "SaveExpMessage");
                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        Messenger.Default.Send<string>(PublicData.ExpDQ.Exp_SM.Stage_DQ.Stage_Name + "已完成！", "OpenPrompt");
                    }));
                    //非预加压时，打开渗漏水确认窗口
                    if (StageNOInList > 0)
                    {
                        PublicData.ExpDQ.ExpData_SM.SLStatusCopy();
                        OpenSMSLGCWin();
                    }
                    Thread.Sleep(2000);
                    PublicData.ExpDQ.Exp_SM.Stage_DQ = new MQZH_StageModel_QSM();
                    PublicData.ExpDQ.Exp_SM.Step_DQ = new MQZH_StepModel_QSM();
                    EscStopCompleteExpReset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 水密定级波动检测事务
        /// </summary>
        private void SMBDDJStageFunc()
        {
            double tempValueNow;                //当前值
            double tempAimAvg;                  //阶段控制目标平均值
            double tempAimH;                    //阶段控制目标高
            double tempAimL;                    //阶段控制目标低
            double tempGivenH;                  //高压给定计算值
            double tempErrH;                    //高压误差允许值
            double tempPermitErrH;              //高压偏差允许范围

            double tempTimePreparePressKeep;    //压力准备保持时间
            double tempTimeWaveDur = 0;         //波动持续总时间
            double tempTimeSwitchBetween = 0;   //切换间隔时间
            bool tempStageComplete = true;      //阶段完成
            double tempPhraseRatioH = 1;        //风机频率调整比率-高压
            double tempPhraseRatioL = 1;        //风机频率调整比率-低压
            int tempType;                       //阶段类型

            try
            {
                PublicData.Dev.IsDeviceBusy = true;
                //如果当前阶段是默认阶段，则复位状态
                if ((PublicData.ExpDQ.Exp_SM.Stage_DQ.Stage_NO == "99") || (StageNOInList == -1))
                {
                    MessageBox.Show("默认阶段不能做实验");
                    EscStopCompleteExpReset();
                    TimerThreadLock = false;
                    return;
                }

                #region 试验计算控制过程

                //阶段开始时输出复位
                if (!IsStageTestStarted)
                {
                    IsStageTestStarted = true;
                    WavePhOut=0 ;
                }

                //阶段开始前提醒
                if (PublicData.ExpDQ.Exp_SM.Stage_DQ.IsNeedTipsBefore &&
                        (!PublicData.ExpDQ.Exp_SM.Stage_DQ.IsTipsBeforeComplete))
                {
                    PublicData.ExpDQ.Exp_SM.WaveInfo = "阶段开始前提醒";
                    MessageBox.Show(PublicData.ExpDQ.Exp_SM.Stage_DQ.StringTipsBefore, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    PublicData.ExpDQ.Exp_SM.Stage_DQ.IsTipsBeforeComplete = true;
                }

                //自动开启喷淋
                if (PublicData.Dev.IsWithSLL && (StageNOInList != 0) && PublicData.Dev.DOList[6].IsOn)
                {
                    PublicData.Dev.DOList[9].IsOn = true;   //开水泵变频
                    //喷淋流量计算稳定
                    SMPLFunc();
                    //控制输出
                    double tempUKLL_SMWD = PID_SMSLL.UK;
                    if (tempUKLL_SMWD <= 0)
                    {
                        tempUKLL_SMWD = 0;
                    }
                    if (tempUKLL_SMWD >= 32000)
                    {
                        tempUKLL_SMWD = 32000;
                    }
                    PLCCKCMDFrmBLL[4] = Convert.ToUInt16(tempUKLL_SMWD);
                }
                else
                {
                    PublicData.Dev.DOList[9].IsOn = false;   //关水泵变频
                    PLCCKCMDFrmBLL[4] = 0;
                }

                //逐个步骤计算
                int stepsCount = PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList.Count;
                for (int i = 0; i < stepsCount; i++)
                {
                    //-------数据准备-------
                    StepNOInList = i;
                    //步骤稳压目标值
                    tempType = 22;
                    tempAimAvg = GetAimPress(tempType, StageNOInList, StepNOInList);
                    tempAimL = tempAimAvg * PublicData.Dev.LowRatioSM;
                    tempAimH = tempAimAvg * PublicData.Dev.HighRatioSM;
                    PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[i].WaveAimAverageValue = tempAimAvg;
                    PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[i].WaveAimHBound = tempAimH;
                    PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[i].WaveAimLBound = tempAimL;
                    //压力准备保持时间
                    tempTimePreparePressKeep = PublicData.Dev.KeepingTime_SMPreparePress;
                    //步骤波动持续总时间
                    if (i == 0)
                        tempTimeWaveDur = PublicData.Dev.KeepingTime_SMFistStep;
                    else
                        tempTimeWaveDur = PublicData.Dev.KeepingTime_SMDJ_StepLeft;

                    //蝶阀2开阀时间、切换时风机2升速延时占比、切换时风机降频比率
                    tempPhraseRatioH = PublicData.Dev.PhRatioWaveH;
                    tempPhraseRatioL = PublicData.Dev.PhRatioWaveL;
                    //获取偏差允许范围
                    tempPermitErrH = GetErrBig(tempAimH);
                    //当前压力
                    if (PublicData.Dev.IsWithCYM)
                        tempValueNow = PublicData.Dev.AIList[13].ValueFinal;
                    else
                        tempValueNow = PublicData.Dev.AIList[14].ValueFinal;

                    if (!PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[i].IsStepCompleted)
                    {
                        tempStageComplete = false;
                        PublicData.ExpDQ.Exp_SM.Step_DQ = PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[i];

                        //-----步骤开始前等待-----
                        if (!PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaitBeforCompleted)
                        {
                            PublicData.ExpDQ.Exp_SM.WaveInfo = "步骤开始前等待";
                            if (!PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaitStarted)
                            {
                                PublicData.ExpDQ.Exp_SM.Step_DQ.WaitStartTime = DateTime.Now;
                                PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaitStarted = true;
                            }
                            TimeSpan tempSpanWait = DateTime.Now - PublicData.ExpDQ.Exp_SM.Step_DQ.WaitStartTime;
                            if (tempSpanWait.TotalSeconds >= PublicData.ExpDQ.Exp_SM.Step_DQ.TimeWaitBefor)
                            {
                                PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaitBeforCompleted = true;
                            }
                            //等待未完成或切换瞬间，输出保持不变
                            else
                            {
                                break;
                            }
                        }

                        //第一级喷水不加压
                        if (StepNOInList == 0)
                        {
                            if (!PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaveCompleted)
                            {
                                PublicData.ExpDQ.Exp_SM.WaveInfo = "第1级喷水";

                                if (!PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaveStarted)
                                {
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaveStarted = true;
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.WaveStartTime = DateTime.Now;
                                }
                                else
                                {
                                    double timePased = (DateTime.Now - PublicData.ExpDQ.Exp_SM.Step_DQ.WaveStartTime).TotalSeconds;
                                    if (timePased >= tempTimeWaveDur)
                                    {
                                        PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaveCompleted = true;
                                        PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[i].IsStepCompleted = true;
                                    }
                                }
                            }
                            PLCCKCMDFrmBLL[3] = 0;
                            return;
                        }

                        #region -----高压压力准备----------
                        if (!PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.IsKeepPressCompleted)
                        {
                            //阀门工作模式
                            PLCCKCMDFrmBLL[5] = 5;  //换向阀为正压模式
                            //等待换向阀状态到位
                            if (PPressTest && (!PublicData.Dev.Valve.DIList[5].IsOn))
                                break;

                            //高压力加载
                            if (!PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.IsUpCompleted)
                            {
                                PublicData.ExpDQ.Exp_SM.WaveInfo = "高压压力上升加载";
                                if (!PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.IsUpStarted)
                                {
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.UpStartTime = DateTime.Now;
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.IsUpStarted = true;
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.LoadUpStartValue = tempValueNow;
                                }
                                TimeSpan tempSpanUpH = DateTime.Now - PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.UpStartTime;
                                tempGivenH = PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.LoadUpStartValue +
                                            Math.Abs(PublicData.Dev.LoadUpDownSpeed) * tempSpanUpH.TotalSeconds;
                                //判断压力给定值增加是否结束
                                if (tempGivenH >= tempAimH)
                                {
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.IsUpCompleted = true;
                                    tempGivenH = tempAimH;
                                }
                            }
                            //高压压力保持
                            else if ((tempValueNow >= (tempAimH - tempPermitErrH)) && (tempValueNow <= (tempAimH + tempPermitErrH)))
                            {
                                PublicData.ExpDQ.Exp_SM.WaveInfo = "高压压力保持";
                                tempGivenH = tempAimH;
                                if (!PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.IsKeepPressStarted)
                                {
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.IsKeepPressStarted = true;
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.KeepPressStartTime = DateTime.Now;
                                }
                                TimeSpan tempSpanKeepH = DateTime.Now - PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.KeepPressStartTime;
                                PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.PressKeeppingTimes = tempSpanKeepH.TotalSeconds;
                                //判定是否完成压力保持
                                if (tempSpanKeepH.TotalSeconds >= tempTimePreparePressKeep)
                                {
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.IsKeepPressCompleted = true;
                                    Trace.Write("第" + i + "级高压目标" + tempAimH + "    高压保持" + tempValueNow + "\r\n");
                                    break;
                                }
                            }
                            else
                            {
                                tempGivenH = tempAimH;
                                PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.IsKeepPressStarted = false;
                                PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.KeepPressStartTime = DateTime.Now;
                                PublicData.ExpDQ.Exp_SM.WaveInfo = "高压压力控制";
                            }

                            //高压按PID计算
                            if (tempGivenH < 0)
                                tempGivenH = 0;
                            tempErrH = tempGivenH - tempValueNow;
                            PID_ParamModel tempPIDParamH = GePIDParam(2, tempGivenH);
                            //PID计算
                            PID_SM1.PID_Param = tempPIDParamH;
                            if (Math.Abs(tempGivenH) < 0.01)
                                PID_SM1.PID_Param.ControllerEnable = false; //预设压力为0时屏蔽pid
                            else
                                PID_SM1.PID_Param.ControllerEnable = true; //pid使能
                            PID_SM1.CalculatePID(tempErrH);

                            WavePhOut = PID_SM1.UK;
                            if (WavePhOut <= 0)
                            {
                                WavePhOut = 0;
                            }
                            if (WavePhOut >= 32000)
                            {
                                WavePhOut = 32000;
                            }
                            break;
                        }

                        #endregion

                        #region -----低压压力准备----------

                        if (!PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePLowStatus.IsKeepPressCompleted)
                        {
                            PublicData.ExpDQ.Exp_SM.WaveInfo = "低压压力准备";

                            PLCCKCMDFrmBLL[5] = 7; //换向阀为正压低压准备模式
                            PLCCKCMDFrmBLL[6] = 500; //6、7准备脉冲频率
                            PLCCKCMDFrmBLL[7] = 0; //低压准备脉冲频率1000（15秒旋转八分之一圈）
                            PLCCKCMDFrmBLL[8] = 0; //7、8脉冲个数
                            PLCCKCMDFrmBLL[9] = 0; //0持续旋转
                            //计算设定压力的传输值
                            int pressID;
                            if (PublicData.Dev.IsWithCYM)
                                pressID = 13;
                            else
                                pressID = 14;
                            double setDataDouble = PublicData.Dev.AIList[pressID].GetY(PublicData.Dev.AIList[pressID].SingalLowerRange,
                                PublicData.Dev.AIList[pressID].SingalUpperRange, 0, 4000, tempAimL);
                            PLCCKCMDFrmBLL[10] = Convert.ToUInt16(setDataDouble); //设定压力

                            if (PublicData.Dev.IsWithCYM) //压力通道(2大3中)
                                PLCCKCMDFrmBLL[11] = 3;
                            else
                                PLCCKCMDFrmBLL[11] = 2;

                            //等待换向阀低压准备到位
                            if (PublicData.Dev.Valve.DIList[7].IsOn)
                            {
                                PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePLowStatus.IsKeepPressCompleted = true;
                                PublicData.ExpDQ.Exp_SM.WaveInfo = "低压压力准备完毕";
                            }
                            else
                                break;
                        }

                        #endregion


                        #region-----压力切换----------

                        if (!PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaveCompleted)
                        {
                            PublicData.ExpDQ.Exp_SM.WaveInfo = "压力波动切换";

                            if (!PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaveStarted)
                            {
                                PublicData.ExpDQ.Exp_SM.Step_DQ.WaveStartTime = DateTime.Now;
                                PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaveStarted = true;
                            }

                            //判断是否达到本步骤波动总时长
                            TimeSpan tempSpanWaveContinue = DateTime.Now - PublicData.ExpDQ.Exp_SM.Step_DQ.WaveStartTime;
                            PublicData.ExpDQ.Exp_SM.Step_DQ.WaveTimePased = tempSpanWaveContinue.TotalSeconds;
                            if (tempSpanWaveContinue.TotalSeconds >= tempTimeWaveDur)
                            {
                                PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaveCompleted = true;
                                PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[i].IsStepCompleted = true;
                                PLCCKCMDFrmBLL[5] = 5; //换向阀为正压模式
                                PublicData.ExpDQ.Exp_SM.WaveInfo = "波动完成";
                            }
                            else
                            {
                                PLCCKCMDFrmBLL[5] = 9; //换向阀为次数波动正压模式
                                PLCCKCMDFrmBLL[6] = 0; //6、7波动准备脉冲频率
                                PLCCKCMDFrmBLL[7] = 0;
                                PLCCKCMDFrmBLL[8] = 0; //7、8波动准备脉冲个数
                                PLCCKCMDFrmBLL[9] = 0; //0持续旋转
                                PLCCKCMDFrmBLL[10] = 0; //设定压力
                                PLCCKCMDFrmBLL[11] = 0;
                                PLCCKCMDFrmBLL[12] = 3500; //12、13波动脉冲频率
                                PLCCKCMDFrmBLL[13] = 0; //波动脉冲频率3500（2.14秒旋转十六分之一圈）
                                PLCCKCMDFrmBLL[14] = 0; //14、15波动脉冲频率修正
                                PLCCKCMDFrmBLL[15] = 0;
                                PLCCKCMDFrmBLL[16] = 30000; //波动次数
                            }
                            break;
                        }

                        #endregion
                    }
                }
                //严重渗漏时
                if (YZSLOrder)
                    tempStageComplete = true;
                if (tempStageComplete)
                {
                    PublicData.ExpDQ.Exp_SM.Stage_DQ.CompleteStatus = true;
                    PID_SM1.PID_Param.ControllerEnable = false;
                    PID_SM1.CalculatePID(0);
                    WavePhOut = PID_SM1.UK;
                }

                #endregion

                //控制输出
                PLCCKCMDFrmBLL[3] = Convert.ToUInt16(WavePhOut);

                //PID计算数据显示
                //string kpidStr = "kp:" + PID_SM1.PID_Param.Kp.ToString("0.00") + "ki:" +
                //                 PID_SM1.PID_Param.Ki.ToString("0.00") + "kd:" + PID_SM1.PID_Param.Kd.ToString("0.00");
                //string aimStr = tempGivenL.ToString("0.00");
                //string ukStr = PID_SM1.UK.ToString("0.00");
                //string ukpStr = PID_SM1.UK_P.ToString("0.00");
                //string ukiStr = PID_SM1.UK_I.ToString("0.00");
                //string ukdStr = PID_SM1.UK_D.ToString("0.00");
                //Messenger.Default.Send<string>(kpidStr + "  GivenL:" + aimStr + "  ,U:" + ukStr + "  ,Up:" + ukpStr + "  ,Ui:" + ukiStr + "  ,Ud:" + ukdStr, "PIDinfoMessage");

                //判断是否完成
                if (PublicData.ExpDQ.Exp_SM.Stage_DQ.CompleteStatus)
                {
                    //分析是否所有阶段都完成
                    bool tempAllStageComplete = true;
                    for (int i = 0; i < PublicData.ExpDQ.Exp_SM.StageList_SMDJ.Count; i++)
                    {
                        if (!PublicData.ExpDQ.Exp_SM.StageList_SMDJ[i].CompleteStatus)
                            tempAllStageComplete = false;
                    }
                    if (tempAllStageComplete)
                        PublicData.ExpDQ.Exp_SM.CompleteStatus = true;
                    //保存进度和数据
                    if (PublicData.ExpDQ.Exp_SM.Stage_DQ.Stage_NO == TestStageType.SM_DJ_J.ToString())
                    {
                        for (int i = 0; i < PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList.Count; i++)
                        {
                            PublicData.ExpDQ.ExpData_SM.Press_DJ[i] = PublicData.Dev.PressSet_SMDJ_BDPJ_Std[i];
                        }
                    }
                    Messenger.Default.Send<string>("SaveSMStatusAndData", "SaveExpMessage");
                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        Messenger.Default.Send<string>(PublicData.ExpDQ.Exp_SM.Stage_DQ.Stage_Name + "已完成！", "OpenPrompt");
                    }));

                    //非预加压时，打开渗漏水确认窗口
                    if (StageNOInList > 0)
                    {
                        PublicData.ExpDQ.ExpData_SM.SLStatusCopy();
                        OpenSMSLDJWin();
                    }
                    Thread.Sleep(2000);
                    PublicData.ExpDQ.Exp_SM.Stage_DQ = new MQZH_StageModel_QSM();
                    PublicData.ExpDQ.Exp_SM.Step_DQ = new MQZH_StepModel_QSM();
                    EscStopCompleteExpReset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 水密工程波动检测事务
        /// </summary>
        private void SMBDGCStageFunc()
        {
            double tempValueNow;                //当前值
            double tempAimAvg=0;                  //阶段控制目标平均值
            double tempAimH;                    //阶段控制目标高
            double tempAimL;                    //阶段控制目标低
            double tempGivenH;                  //高压给定计算值
            double tempErrH;                    //高压误差允许值
            double tempPermitErrH;              //高压偏差允许范围

            double tempTimePreparePressKeep;    //压力准备保持时间
            double tempTimeWaveDur = 0;         //波动持续总时间
            double tempTimeSwitchBetween = 0;   //切换间隔时间
            bool tempStageComplete = true;      //阶段完成
            double tempPhraseRatioH = 1;        //风机频率调整比率-高压
            double tempPhraseRatioL = 1;        //风机频率调整比率-低压
            int tempType;                       //阶段类型

            try
            {
                PublicData.Dev.IsDeviceBusy = true;
                //如果当前阶段是默认阶段，则复位状态
                if ((PublicData.ExpDQ.Exp_SM.Stage_DQ.Stage_NO == "99") || (StageNOInList == -1))
                {
                    MessageBox.Show("默认阶段不能做实验");
                    EscStopCompleteExpReset();
                    TimerThreadLock = false;
                    return;
                }

                #region 试验计算控制过程

                //阶段开始时输出复位
                if (!IsStageTestStarted)
                {
                    IsStageTestStarted = true;
                    WavePhOut = 0;
                }

                //阶段开始前提醒
                if (PublicData.ExpDQ.Exp_SM.Stage_DQ.IsNeedTipsBefore &&
                        (!PublicData.ExpDQ.Exp_SM.Stage_DQ.IsTipsBeforeComplete))
                {
                    PublicData.ExpDQ.Exp_SM.WaveInfo = "阶段开始前提醒";
                    MessageBox.Show(PublicData.ExpDQ.Exp_SM.Stage_DQ.StringTipsBefore, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    PublicData.ExpDQ.Exp_SM.Stage_DQ.IsTipsBeforeComplete = true;
                }

                //自动开启喷淋
                if (PublicData.Dev.IsWithSLL && (StageNOInList != 0) && PublicData.Dev.DOList[6].IsOn)
                {
                    PublicData.Dev.DOList[9].IsOn = true;   //开水泵变频
                    //喷淋流量计算稳定
                    SMPLFunc();
                    //控制输出
                    double tempUKLL_SMWD = PID_SMSLL.UK;
                    if (tempUKLL_SMWD <= 0)
                    {
                        tempUKLL_SMWD = 0;
                    }
                    if (tempUKLL_SMWD >= 32000)
                    {
                        tempUKLL_SMWD = 32000;
                    }
                    PLCCKCMDFrmBLL[4] = Convert.ToUInt16(tempUKLL_SMWD);
                }
                else
                {
                    PublicData.Dev.DOList[9].IsOn = false;   //关水泵变频
                    PLCCKCMDFrmBLL[4] = 0;
                }

                //逐个步骤计算
                int stepsCount = PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList.Count;
                int startStep = 0;
                if ((PublicData.Dev.PlTime_SMGC_Before < 1) && (StageNOInList != 0))
                {
                    startStep = 1;
                    PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[0].IsStepCompleted = true;
                }
                for (int i = startStep; i < stepsCount; i++)
                {
                    //-------数据准备-------
                    StepNOInList = i;
                    //步骤稳压目标值
                    //水密工程波动2步27，水密工程波动3步28，水密工程波动9步29，水密工程波动10步30；
                    if (PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList.Count == 2)
                    {
                        tempType = 27;
                        tempAimAvg = GetAimPress(tempType, StageNOInList, StepNOInList);
                    }
                    else if
                        (PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList.Count == 3)
                    {
                        tempType = 28;
                        tempAimAvg = GetAimPress(tempType, StageNOInList, StepNOInList);
                    }
                    else if (PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList.Count == 9)
                    {
                        tempType = 29;
                        tempAimAvg = GetAimPress(tempType, StageNOInList, StepNOInList);
                    }
                    else if (PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList.Count == 10)
                    {
                        tempType = 30;
                        tempAimAvg = GetAimPress(tempType, StageNOInList, StepNOInList);
                    }
                    tempAimL = tempAimAvg * PublicData.Dev.LowRatioSM;
                    tempAimH = tempAimAvg * PublicData.Dev.HighRatioSM;
                    PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[i].WaveAimAverageValue = tempAimAvg;
                    PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[i].WaveAimHBound = tempAimH;
                    PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[i].WaveAimLBound = tempAimL;
                    //压力准备保持时间
                    tempTimePreparePressKeep = PublicData.Dev.KeepingTime_SMPreparePress;
                    //步骤波动持续总时间
                    if (PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList.Count == 2)     //无可开启部分，设计压力小于2000
                    {
                        if (StepNOInList == 0)
                            tempTimeWaveDur = PublicData.Dev.PlTime_SMGC_Before;
                        else
                            tempTimeWaveDur = PublicData.Dev.KeepingTime_SMGC_WKQ;
                    }
                    else if (PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList.Count == 3)    //有可开启部分，设计压力均小于2000
                    {
                        if (StepNOInList == 0)
                            tempTimeWaveDur = PublicData.Dev.PlTime_SMGC_Before;
                        else if (StepNOInList == 1)
                            tempTimeWaveDur = PublicData.Dev.KeepingTime_SMGC_KKQ;
                        else
                            tempTimeWaveDur = PublicData.Dev.KeepingTime_SMGC_YKQ;
                    }
                    else if (PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList.Count == 9)        //固定部分设计压力大于2000
                    {
                        if (StepNOInList == 0)
                            tempTimeWaveDur = PublicData.Dev.PlTime_SMGC_Before;
                        else if (StepNOInList == 8)
                            tempTimeWaveDur = PublicData.Dev.KeepingTime_SMGC_YKQ;
                        else
                            tempTimeWaveDur = PublicData.Dev.KeepingTime_SMDJ_StepLeft;
                    }
                    //有可开启部分，且两部分设计压力均大于2000
                    else
                    {
                        if (StepNOInList == 0)
                            tempTimeWaveDur = PublicData.Dev.PlTime_SMGC_Before;
                        else if (StepNOInList == 8)
                            tempTimeWaveDur = PublicData.Dev.KeepingTime_SMGC_KKQ;
                        else if (StepNOInList == 9)
                            tempTimeWaveDur = PublicData.Dev.KeepingTime_SMGC_YKQ;
                        else
                            tempTimeWaveDur = PublicData.Dev.KeepingTime_SMDJ_StepLeft;
                    }

                    //切换时风机调频比率
                    tempPhraseRatioH = PublicData.Dev.PhRatioWaveH;
                    tempPhraseRatioL = PublicData.Dev.PhRatioWaveL;
                    //获取偏差允许范围
                    tempPermitErrH = GetErrBig(tempAimH);
                    //当前压力
                    if (PublicData.Dev.IsWithCYM)
                        tempValueNow = PublicData.Dev.AIList[13].ValueFinal;
                    else
                        tempValueNow = PublicData.Dev.AIList[14].ValueFinal;

                    if (!PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[i].IsStepCompleted)
                    {
                        tempStageComplete = false;
                        PublicData.ExpDQ.Exp_SM.Step_DQ = PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[i];

                        //-----步骤开始前等待-----
                        if (!PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaitBeforCompleted)
                        {
                            PublicData.ExpDQ.Exp_SM.WaveInfo = "步骤开始前等待";
                            if (!PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaitStarted)
                            {
                                PublicData.ExpDQ.Exp_SM.Step_DQ.WaitStartTime = DateTime.Now;
                                PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaitStarted = true;
                            }
                            TimeSpan tempSpanWait = DateTime.Now - PublicData.ExpDQ.Exp_SM.Step_DQ.WaitStartTime;
                            if (tempSpanWait.TotalSeconds >= PublicData.ExpDQ.Exp_SM.Step_DQ.TimeWaitBefor)
                            {
                                PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaitBeforCompleted = true;
                            }
                            //等待未完成或切换瞬间，输出保持不变
                            else
                            {
                                break;
                            }
                        }

                        //第一级喷水不加压
                        if (StepNOInList == 0)
                        {
                            if (!PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaveCompleted)
                            {
                                PublicData.ExpDQ.Exp_SM.WaveInfo = "第一阶段喷水";
                                if (!PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaveStarted)
                                {
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaveStarted = true;
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.WaveStartTime = DateTime.Now;
                                }
                                else
                                {
                                    double timePased = (DateTime.Now - PublicData.ExpDQ.Exp_SM.Step_DQ.WaveStartTime).TotalSeconds;
                                    if (timePased >= tempTimeWaveDur)
                                    {
                                        PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaveCompleted = true;
                                        PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[i].IsStepCompleted = true;
                                    }
                                }
                            }
                            PLCCKCMDFrmBLL[3] = 0;
                            return;
                        }

                        #region -----高压压力准备----------
                        if (!PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.IsKeepPressCompleted)
                        {
                            //阀门工作模式
                            PLCCKCMDFrmBLL[5] = 5;  //换向阀为正压模式
                            //等待换向阀状态到位
                            if (PPressTest && (!PublicData.Dev.Valve.DIList[5].IsOn))
                                break;

                            //高压力加载
                            if (!PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.IsUpCompleted)
                            {
                                PublicData.ExpDQ.Exp_SM.WaveInfo = "高压压力上升加载";
                                if (!PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.IsUpStarted)
                                {
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.UpStartTime = DateTime.Now;
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.IsUpStarted = true;
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.LoadUpStartValue = tempValueNow;
                                }
                                TimeSpan tempSpanUpH = DateTime.Now - PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.UpStartTime;
                                tempGivenH = PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.LoadUpStartValue +
                                            Math.Abs(PublicData.Dev.LoadUpDownSpeed) * tempSpanUpH.TotalSeconds;
                                //判断压力给定值增加是否结束
                                if (tempGivenH >= tempAimH)
                                {
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.IsUpCompleted = true;
                                    tempGivenH = tempAimH;
                                }
                            }
                            //高压压力保持
                            else if ((tempValueNow >= (tempAimH - tempPermitErrH)) && (tempValueNow <= (tempAimH + tempPermitErrH)))
                            {
                                PublicData.ExpDQ.Exp_SM.WaveInfo = "高压压力保持";
                                tempGivenH = tempAimH;
                                if (!PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.IsKeepPressStarted)
                                {
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.IsKeepPressStarted = true;
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.KeepPressStartTime = DateTime.Now;
                                }
                                TimeSpan tempSpanKeepH = DateTime.Now - PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.KeepPressStartTime;
                                PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.PressKeeppingTimes = tempSpanKeepH.TotalSeconds;
                                //判定是否完成压力保持
                                if (tempSpanKeepH.TotalSeconds >= tempTimePreparePressKeep)
                                {
                                    PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.IsKeepPressCompleted = true;
                                    Trace.Write("第" + i + "级高压目标" + tempAimH + "    高压保持" + tempValueNow + "\r\n");
                                    break;
                                }
                            }
                            else
                            {
                                tempGivenH = tempAimH;
                                PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.IsKeepPressStarted = false;
                                PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePUpStatus.KeepPressStartTime = DateTime.Now;
                                PublicData.ExpDQ.Exp_SM.WaveInfo = "高压压力控制";
                            }

                            //高压按PID计算
                            if (tempGivenH < 0)
                                tempGivenH = 0;
                            tempErrH = tempGivenH - tempValueNow;
                            PID_ParamModel tempPIDParamH = GePIDParam(2, tempGivenH);
                            //PID计算
                            PID_SM1.PID_Param = tempPIDParamH;
                            if (Math.Abs(tempGivenH) < 0.01)
                                PID_SM1.PID_Param.ControllerEnable = false; //预设压力为0时屏蔽pid
                            else
                                PID_SM1.PID_Param.ControllerEnable = true; //pid使能
                            PID_SM1.CalculatePID(tempErrH);

                            WavePhOut = PID_SM1.UK;
                            if (WavePhOut <= 0)
                            {
                                WavePhOut = 0;
                            }
                            if (WavePhOut >= 32000)
                            {
                                WavePhOut = 32000;
                            }
                            break;
                        }

                        #endregion

                        #region -----低压压力准备----------

                        if (!PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePLowStatus.IsKeepPressCompleted)
                        {
                            PublicData.ExpDQ.Exp_SM.WaveInfo = "低压压力准备";

                            //阀门工作模式
                            PLCCKCMDFrmBLL[5] = 7; //换向阀为正压模式
                            PLCCKCMDFrmBLL[6] = 500; //6、7准备脉冲频率
                            PLCCKCMDFrmBLL[7] = 0; //低压准备脉冲频率1000（15秒旋转八分之一圈）
                            PLCCKCMDFrmBLL[8] = 0; //7、8脉冲个数
                            PLCCKCMDFrmBLL[9] = 0; //0持续旋转
                            //计算设定压力的传输值
                            int pressID;
                            if (PublicData.Dev.IsWithCYM)
                                pressID = 13;
                            else
                                pressID = 14;
                            double setDataDouble = PublicData.Dev.AIList[pressID].GetY(PublicData.Dev.AIList[pressID].SingalLowerRange,
                                PublicData.Dev.AIList[pressID].SingalUpperRange, 0, 4000, tempAimL);
                            PLCCKCMDFrmBLL[10] = Convert.ToUInt16(setDataDouble); //设定压力

                            if (PublicData.Dev.IsWithCYM) //压力通道(2大3中)
                                PLCCKCMDFrmBLL[11] = 3;
                            else
                                PLCCKCMDFrmBLL[11] = 2;

                            //等待换向阀低压准备到位
                            if (PublicData.Dev.Valve.DIList[7].IsOn)
                            {
                                PublicData.ExpDQ.Exp_SM.Step_DQ.StepWavePLowStatus.IsKeepPressCompleted = true;
                                PublicData.ExpDQ.Exp_SM.WaveInfo = "低压压力准备完毕";
                            }
                            else
                                break;
                        }

                        #endregion


                        #region-----压力切换----------

                        if (!PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaveCompleted)
                        {
                            PublicData.ExpDQ.Exp_SM.WaveInfo = "压力波动切换";

                            if (!PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaveStarted)
                            {
                                PublicData.ExpDQ.Exp_SM.Step_DQ.WaveStartTime = DateTime.Now;
                                PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaveStarted = true;
                            }

                            //判断是否达到本步骤波动总时长
                            TimeSpan tempSpanWaveContinue = DateTime.Now - PublicData.ExpDQ.Exp_SM.Step_DQ.WaveStartTime;
                            PublicData.ExpDQ.Exp_SM.Step_DQ.WaveTimePased = tempSpanWaveContinue.TotalSeconds;
                            if (tempSpanWaveContinue.TotalSeconds >= tempTimeWaveDur)
                            {
                                PublicData.ExpDQ.Exp_SM.Step_DQ.IsWaveCompleted = true;
                                PublicData.ExpDQ.Exp_SM.Stage_DQ.StepList[i].IsStepCompleted = true;
                                PLCCKCMDFrmBLL[5] = 5; //换向阀为正压模式
                                PublicData.ExpDQ.Exp_SM.WaveInfo = "波动完成";
                            }
                            else
                            {
                                PLCCKCMDFrmBLL[5] = 9; //换向阀为次数波动正压模式
                                PLCCKCMDFrmBLL[6] = 0; //6、7波动准备脉冲频率
                                PLCCKCMDFrmBLL[7] = 0;
                                PLCCKCMDFrmBLL[8] = 0; //7、8波动准备脉冲个数
                                PLCCKCMDFrmBLL[9] = 0; //0持续旋转
                                PLCCKCMDFrmBLL[10] = 0; //设定压力
                                PLCCKCMDFrmBLL[11] = 0;
                                PLCCKCMDFrmBLL[12] = 3500; //12、13波动脉冲频率
                                PLCCKCMDFrmBLL[13] = 0; //波动脉冲频率3500（2.14秒旋转十六分之一圈）
                                PLCCKCMDFrmBLL[14] = 0; //14、15波动脉冲频率修正
                                PLCCKCMDFrmBLL[15] = 0;
                                PLCCKCMDFrmBLL[16] = 30000; //波动次数
                            }
                            break;
                        }

                        #endregion
                    }
                }
                //严重渗漏时
                if (YZSLOrder)
                    tempStageComplete = true;
                if (tempStageComplete)
                {
                    PublicData.ExpDQ.Exp_SM.Stage_DQ.CompleteStatus = true;
                    PID_SM1.PID_Param.ControllerEnable = false;
                    PID_SM1.CalculatePID(0);
                    WavePhOut= PID_SM1.UK;
                }

                #endregion

                //控制输出
                PLCCKCMDFrmBLL[3] = Convert.ToUInt16(WavePhOut);

                //PID计算数据显示
                //string kpidStr = "kp:" + PID_SM1.PID_Param.Kp.ToString("0.00") + "ki:" +
                //                 PID_SM1.PID_Param.Ki.ToString("0.00") + "kd:" + PID_SM1.PID_Param.Kd.ToString("0.00");
                //string aimStr = tempGivenL.ToString("0.00");
                //string ukStr = PID_SM1.UK.ToString("0.00");
                //string ukpStr = PID_SM1.UK_P.ToString("0.00");
                //string ukiStr = PID_SM1.UK_I.ToString("0.00");
                //string ukdStr = PID_SM1.UK_D.ToString("0.00");
                //Messenger.Default.Send<string>(kpidStr + "  GivenL:" + aimStr + "  ,U:" + ukStr + "  ,Up:" + ukpStr + "  ,Ui:" + ukiStr + "  ,Ud:" + ukdStr, "PIDinfoMessage");

                //判断是否完成
                if (PublicData.ExpDQ.Exp_SM.Stage_DQ.CompleteStatus)
                {
                    //分析是否所有阶段都完成
                    bool tempAllStageComplete = true;
                    for (int i = 0; i < PublicData.ExpDQ.Exp_SM.StageList_SMGC.Count; i++)
                    {
                        if (!PublicData.ExpDQ.Exp_SM.StageList_SMGC[i].CompleteStatus)
                            tempAllStageComplete = false;
                    }
                    if (tempAllStageComplete)
                        PublicData.ExpDQ.Exp_SM.CompleteStatus = true;
                    //保存进度和数据
                    if (PublicData.ExpDQ.Exp_SM.Stage_DQ.Stage_NO == TestStageType.SM_GC_J.ToString())
                    {
                        PublicData.ExpDQ.ExpData_SM.Press_GC[0] = PublicData.ExpDQ.Exp_SM.SM_GCSJ_KKQ;
                        PublicData.ExpDQ.ExpData_SM.Press_GC[1] = PublicData.ExpDQ.Exp_SM.SM_GCSJ_GD;
                    }
                    Messenger.Default.Send<string>("SaveSMStatusAndData", "SaveExpMessage");
                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        Messenger.Default.Send<string>(PublicData.ExpDQ.Exp_SM.Stage_DQ.Stage_Name + "已完成！", "OpenPrompt");
                    }));

                    //非预加压时，打开渗漏水确认窗口
                    if (StageNOInList > 0)
                    {
                        PublicData.ExpDQ.ExpData_SM.SLStatusCopy();
                        OpenSMSLGCWin();
                    }
                    Thread.Sleep(2000);
                    PublicData.ExpDQ.Exp_SM.Stage_DQ = new MQZH_StageModel_QSM();
                    PublicData.ExpDQ.Exp_SM.Step_DQ = new MQZH_StepModel_QSM();
                    EscStopCompleteExpReset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 水密喷淋实施程序
        /// </summary>
        private void SMPLFunc()
        {
            double tempValueNow = 0;     //当前值
            double tempGiven = 0;       //给定值
            double tempErr = 0;         //PID计算用误差
            double tempPermitErr = 0;   //偏差允许范围

            try
            {
                tempValueNow = PublicData.Dev.SLL;          //当前值为当前水流量
                tempGiven = PublicData.ExpDQ.Exp_SM.SM_SLL;   //给定值为水密喷淋设定值
                //逐个步骤计算给定值
                tempErr = tempGiven - tempValueNow;
                PID_ParamModel tempPIDParam = GePIDParam(5, tempGiven);

                PID_SMSLL.PID_Param = tempPIDParam;
                if (Math.Abs(tempGiven) < 0.01)
                    PID_SMSLL.PID_Param.ControllerEnable = false; //预设流量为0时屏蔽pid
                else
                    PID_SMSLL.PID_Param.ControllerEnable = true; //pid使能
                PID_SMSLL.CalculatePID(tempErr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


        #region 水密启停消息

        /// <summary>
        /// 水密试验控制消息
        /// </summary>
        /// <param name="msg"></param>
        private void SMCtrlMessage(int msg)
        {
            try
            {
                //水密严重渗漏停机
                if ((msg == 1302) || (msg == 1312))
                {
                    YZSLOrder = true;
                }

                //水密定级检测试验开始消息分析。
                if (msg == 1102)
                {
                    //预加压
                    if (PublicData.ExpDQ.Exp_SM.BeenCheckedDJ[0])
                    {
                        if (!PublicData.ExpDQ.Exp_SM.StageList_SMDJ[0].NeedTest)
                        {
                            MessageBox.Show(PublicData.ExpDQ.Exp_SM.StageList_SMDJ[0].Stage_Name + "无需检测！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                            PublicData.ExpDQ.Exp_SM.BeenCheckedDJ[0] = false;
                            return;
                        }
                        if (PublicData.ExpDQ.Exp_SM.StageList_SMDJ[0].CompleteStatus)
                        {
                            MessageBoxResult msgBoxResult = MessageBox.Show(PublicData.ExpDQ.Exp_SM.StageList_SMDJ[0].Stage_Name + "已完成检测，是否重新检测？", "数据覆盖提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                            if (msgBoxResult == MessageBoxResult.Yes)
                            {
                                ExistSMWDDJExp = true;
                                StageNOInList = 0;
                                PublicData.ExpDQ.Exp_SM.SMDJ_YStageInit();
                            }
                            if (msgBoxResult == MessageBoxResult.No)
                            {
                                PublicData.ExpDQ.Exp_SM.BeenCheckedDJ[0] = false;
                            }
                        }
                        else
                        {
                            ExistSMWDDJExp = true;
                            StageNOInList = 0;
                        }
                        if (ExistSMWDDJExp)
                        {
                            PublicData.ExpDQ.Exp_SM.Stage_DQ = new MQZH_StageModel_QSM();
                            PublicData.ExpDQ.Exp_SM.Stage_DQ = PublicData.ExpDQ.Exp_SM.StageList_SMDJ[StageNOInList];
                            Messenger.Default.Send<string>("SaveSMStatusAndData", "SaveExpMessage");
                            return;
                        }
                    }

                    //检测加压
                    if (PublicData.ExpDQ.Exp_SM.BeenCheckedDJ[1])
                    {
                        bool tempExist = false;
                        if (!PublicData.ExpDQ.Exp_SM.StageList_SMDJ[1].NeedTest)
                        {
                            MessageBox.Show(PublicData.ExpDQ.Exp_SM.StageList_SMDJ[1].Stage_Name + "无需检测！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                            PublicData.ExpDQ.Exp_SM.BeenCheckedDJ[1] = false;
                            return;
                        }
                        if (PublicData.ExpDQ.Exp_SM.StageList_SMDJ[1].CompleteStatus)
                        {
                            MessageBoxResult msgBoxResult = MessageBox.Show(PublicData.ExpDQ.Exp_SM.StageList_SMDJ[1].Stage_Name + "，将覆盖已完成的数据，是否覆盖？", "数据覆盖提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                            if (msgBoxResult == MessageBoxResult.Yes)
                            {
                                PublicData.ExpDQ.Exp_SM.SMDJ_JStageInit();
                                StageNOInList = 1;
                                tempExist = true;
                            }
                            if (msgBoxResult == MessageBoxResult.No)
                            {
                                PublicData.ExpDQ.Exp_SM.BeenCheckedDJ[1] = false;
                            }
                        }
                        else
                        {
                            StageNOInList = 1;
                            tempExist = true;
                        }
                        if (tempExist)
                        {
                            PublicData.ExpDQ.Exp_SM.Stage_DQ = new MQZH_StageModel_QSM();
                            PublicData.ExpDQ.Exp_SM.Stage_DQ = PublicData.ExpDQ.Exp_SM.StageList_SMDJ[StageNOInList];
                            //分析是稳定或波动
                            if (PublicData.ExpDQ.Exp_SM.WaveType_SM)
                            {
                                ExistSMWDDJExp = false;
                                ExistSMBDDJExp = true;
                            }
                            else
                            {
                                ExistSMWDDJExp = true;
                                ExistSMBDDJExp = false;
                            }
                            Messenger.Default.Send<string>("SaveSMStatusAndData", "SaveExpMessage");
                        }
                    }
                }

                //水密工程检测试验开始消息分析。
                else if (msg == 1112)
                {
                    //预加压
                    if (PublicData.ExpDQ.Exp_SM.BeenCheckedGC[0])
                    {
                        if (!PublicData.ExpDQ.Exp_SM.StageList_SMGC[0].NeedTest)
                        {
                            MessageBox.Show(PublicData.ExpDQ.Exp_SM.StageList_SMGC[1].Stage_Name + "无需检测！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                            PublicData.ExpDQ.Exp_SM.BeenCheckedGC[0] = false;
                            return;
                        }
                        if (PublicData.ExpDQ.Exp_SM.StageList_SMGC[0].CompleteStatus)
                        {
                            MessageBoxResult msgBoxResult = MessageBox.Show(PublicData.ExpDQ.Exp_SM.StageList_SMGC[0].Stage_Name + "已完成检测，是否重新检测？", "数据覆盖提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                            if (msgBoxResult == MessageBoxResult.Yes)
                            {
                                ExistSMWDGCExp = true;
                                StageNOInList = 0;
                                PublicData.ExpDQ.Exp_SM.SMGC_YStageInit();
                            }
                            if (msgBoxResult == MessageBoxResult.No)
                            {
                                PublicData.ExpDQ.Exp_SM.BeenCheckedGC[0] = false;
                            }
                        }
                        else
                        {
                            ExistSMWDGCExp = true;
                            StageNOInList = 0;
                            PublicData.ExpDQ.Exp_SM.SMGC_YStageInit();
                        }
                        if (ExistSMWDGCExp)
                        {
                            PublicData.ExpDQ.Exp_SM.Stage_DQ = new MQZH_StageModel_QSM();
                            PublicData.ExpDQ.Exp_SM.Stage_DQ = PublicData.ExpDQ.Exp_SM.StageList_SMGC[StageNOInList];
                        }
                        Messenger.Default.Send<string>("SaveSMStatusAndData", "SaveExpMessage");
                    }

                    //检测加压
                    if (PublicData.ExpDQ.Exp_SM.BeenCheckedGC[1])
                    {
                        bool tempExist = false;
                        if (!PublicData.ExpDQ.Exp_SM.StageList_SMGC[1].NeedTest)
                        {
                            MessageBox.Show(PublicData.ExpDQ.Exp_SM.StageList_SMGC[1].Stage_Name + "无需检测！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                            PublicData.ExpDQ.Exp_SM.BeenCheckedGC[1] = false;
                            return;
                        }
                        if (PublicData.ExpDQ.Exp_SM.StageList_SMGC[1].CompleteStatus)
                        {
                            MessageBoxResult msgBoxResult = MessageBox.Show(PublicData.ExpDQ.Exp_SM.StageList_SMGC[1].Stage_Name + "，将覆盖已完成的数据，是否覆盖？", "数据覆盖提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                            if (msgBoxResult == MessageBoxResult.Yes)
                            {
                                PublicData.ExpDQ.Exp_SM.SMGC_JStageInit();
                                StageNOInList = 1;
                                tempExist = true;
                            }
                            if (msgBoxResult == MessageBoxResult.No)
                            {
                                PublicData.ExpDQ.Exp_SM.BeenCheckedGC[1] = false;
                            }
                        }
                        else
                        {
                            PublicData.ExpDQ.Exp_SM.SMGC_JStageInit();
                            StageNOInList = 1;
                            tempExist = true;
                        }
                        if (tempExist)
                        {
                            //根据有没有可开启部分，分别选择工程检测阶段
                            if (PublicData.ExpDQ.ExpSettingParam.Isexp_SJ_WithKKQ)
                            {
                                PublicData.ExpDQ.Exp_SM.SMGC_JStageSelect();
                            }
                            else
                            {
                                PublicData.ExpDQ.Exp_SM.SMGC_JStageSelect2();
                            }
                            PublicData.ExpDQ.Exp_SM.Stage_DQ = new MQZH_StageModel_QSM();
                            PublicData.ExpDQ.Exp_SM.Stage_DQ = PublicData.ExpDQ.Exp_SM.StageList_SMGC[StageNOInList];
                            //分析是稳定或波动
                            if (PublicData.ExpDQ.Exp_SM.WaveType_SM)
                            {
                                ExistSMWDGCExp = false;
                                ExistSMBDGCExp = true;
                            }
                            else
                            {
                                ExistSMWDGCExp = true;
                                ExistSMBDGCExp = false;
                            }
                            PPressTest = true;
                            Messenger.Default.Send<string>("SaveSMStatusAndData", "SaveExpMessage");
                        }
                    }
                }

                //退出水密试验窗口
                else if (msg == 7207)
                {
                    if (ExistSMWDDJExp || ExistSMWDGCExp || ExistSMBDDJExp || ExistSMBDGCExp)
                    {
                        MessageBoxResult msgBoxResult = MessageBox.Show("是否停止正在进行的试验？", "提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                        if (msgBoxResult == MessageBoxResult.Yes)
                        {
                            Messenger.Default.Send<int>(1202, "StopExpMessage");
                            Messenger.Default.Send<string>(MQZH_WinName.SMWinName, "CloseGivenNameWin");
                        }
                    }
                    else
                        Messenger.Default.Send<string>(MQZH_WinName.SMWinName, "CloseGivenNameWin");
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        #endregion


        #region 状态、指令等参数

        /// <summary>
        /// 有未完成水密稳定定级试验
        /// </summary>
        private bool _existSMWDDJExp = false;
        /// <summary>
        /// 有未完成水密稳定定级试验
        /// </summary>
        public bool ExistSMWDDJExp
        {
            get { return _existSMWDDJExp; }
            set
            {
                _existSMWDDJExp = value;
                RaisePropertyChanged(() => ExistSMWDDJExp);
            }
        }

        /// <summary>
        /// 有未完成水密波动定级试验
        /// </summary>
        private bool _existSMBDDJExp = false;
        /// <summary>
        /// 有未完成水密波动定级试验
        /// </summary>
        public bool ExistSMBDDJExp
        {
            get { return _existSMBDDJExp; }
            set
            {
                _existSMBDDJExp = value;
                RaisePropertyChanged(() => ExistSMBDDJExp);
            }
        }

        /// <summary>
        /// 有未完成水密稳定工程试验
        /// </summary>
        private bool _existSMWDGCExp = false;
        /// <summary>
        /// 有未完成水密稳定工程试验
        /// </summary>
        public bool ExistSMWDGCExp
        {
            get { return _existSMWDGCExp; }
            set
            {
                _existSMWDGCExp = value;
                RaisePropertyChanged(() => ExistSMWDGCExp);
            }
        }

        /// <summary>
        /// 有未完成水密波动工程试验
        /// </summary>
        private bool _existSMBDGCExp = false;
        /// <summary>
        /// 有未完成水密波动工程试验
        /// </summary>
        public bool ExistSMBDGCExp
        {
            get { return _existSMBDGCExp; }
            set
            {
                _existSMBDGCExp = value;
                RaisePropertyChanged(() => ExistSMBDGCExp);
            }
        }

        /// <summary>
        /// 有严重渗漏水指令
        /// </summary>
        private bool _yzslOrder = false;
        /// <summary>
        /// 有严重渗漏水指令
        /// </summary>
        public bool YZSLOrder
        {
            get { return _yzslOrder; }
            set
            {
                _yzslOrder = value;
                RaisePropertyChanged(() => YZSLOrder);
            }
        }

        #endregion
    }
}