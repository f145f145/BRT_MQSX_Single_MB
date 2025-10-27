/************************************************************************************
 * 描述：
 * 幕墙四性试验操作业务BLL——气密部分
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
using System.Threading;
using CtrlMethod;
using GalaSoft.MvvmLight.Messaging;
using MQDFJ_MB.Model;
using NPOI.Util;

namespace MQDFJ_MB.BLL
{
    /// <summary>
    /// 主控类
    /// </summary>
    public partial class MQZH_ExpBLL : ObservableObject
    {
        /// <summary>
        /// 气密PID
        /// </summary>
        private PID_CtrlModel _pid_QM = new PID_CtrlModel();
        /// <summary>
        /// 气密PID
        /// </summary>
        public PID_CtrlModel PID_QM
        {
            get { return _pid_QM; }
            set
            {
                _pid_QM = value;
                RaisePropertyChanged(() => PID_QM);
            }
        }

        #region 气密阶段试验实施程序

        /// <summary>
        /// 气密定级阶段试验检测事务
        /// </summary>
        private void QMDJStageFunc()
        {
            double tempDerection = 0;
            double tempValueNow = 0;        //当前值
            double tempGiven = 0;           //给定值
            double tempErr;                 //PID计算用误差
            double tempPermitErr;           //偏差允许范围
            double tempAim;                 //阶段控制目标
            bool tempStageComplete = true;  //阶段完成
            double tempStepPressKeepTime;   //步骤保持时间
            int tempType;                   //阶段类型

            try
            {
                BllDev.IsDeviceBusy = true;
                //如果当前阶段是默认阶段，则复位状态
                if ((BllExp.Exp_QM.Stage_DQ.Stage_NO == "99") || (StageNOInList == -1))
                {
                    MessageBox.Show("默认阶段不能做实验");
                    EscStopCompleteExpReset();
                    return;
                }
                //等待换向阀状态到位
                if (PPressTest && (!BllDev.Valve.DIList[5].IsOn))
                {
                    PLCCKCMDFrmBLL[3] = 0;
                    return;
                }
                if ((!PPressTest) && (!BllDev.Valve.DIList[6].IsOn))
                {
                    PLCCKCMDFrmBLL[3] = 0;
                    return;
                }

                //开始前提醒
                if (BllExp.Exp_QM.Stage_DQ.IsNeedTipsBefore && (!BllExp.Exp_QM.Stage_DQ.IsTipsBeforeComplete))
                {
                    MessageBox.Show(BllExp.Exp_QM.Stage_DQ.StringTipsBefore, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_QM.Stage_DQ.IsTipsBeforeComplete = true;
                }

                //逐个步骤计算给定值
                int stepsCount = BllExp.Exp_QM.Stage_DQ.StepList.Count;
                for (int i = 0; i < stepsCount; i++)
                {
                    //数据准备--------------------------------
                    StepNOInList = i;
                    //步骤压力稳定时间
                    if ((StageNOInList == 0) || (StageNOInList == 2) || (StageNOInList == 4) || (StageNOInList == 6) || (StageNOInList == 8) || (StageNOInList == 10))
                        tempStepPressKeepTime = BllDev.KeepingTime_YJY;
                    else
                    {
                        tempStepPressKeepTime = BllDev.KeepingTime_QMStep;
                    }
                    //步骤稳压目标值
                    tempType = 10;//气密定级
                    tempAim = GetAimPress(tempType, StageNOInList, StepNOInList);
                    tempAim = Math.Abs(tempAim);
                    //获取偏差允许范围
                    tempPermitErr = GetErr(tempAim);
                    //根据目标压力选择实测值，目标压力小于小压差量程上限的90%时用小压差
                    if (tempAim <= Math.Abs(BllDev.AIList[12].SingalUpperRange) * 0.9)
                    {
                        tempValueNow = Math.Abs(BllDev.AIList[12].ValueFinal);
                    }
                    else
                    {
                        tempValueNow = Math.Abs(BllDev.AIList[14].ValueFinal);
                    }
                    tempGiven = tempValueNow;

                    if (!BllExp.Exp_QM.Stage_DQ.StepList[i].IsStepCompleted)
                    {
                        tempStageComplete = false;
                        BllExp.Exp_QM.Step_DQ = BllExp.Exp_QM.Stage_DQ.StepList[i];

                        //步骤开始前等待
                        if (!BllExp.Exp_QM.Step_DQ.IsWaitBeforCompleted)
                        {
                            Trace.Write(i + "等待" + "\r\n");
                            if (!BllExp.Exp_QM.Step_DQ.IsWaitStarted)
                            {
                                BllExp.Exp_QM.Step_DQ.WaitStartTime = DateTime.Now;
                                BllExp.Exp_QM.Step_DQ.IsWaitStarted = true;
                            }
                            TimeSpan tempSpanWait = DateTime.Now - BllExp.Exp_QM.Step_DQ.WaitStartTime;
                            if (tempSpanWait.TotalSeconds >= BllExp.Exp_QM.Step_DQ.TimeWaitBefor)
                            {
                                BllExp.Exp_QM.Step_DQ.IsWaitBeforCompleted = true;
                                break;
                            }
                            //等待未完成或切换瞬间，保持PID输出
                            tempGiven = 0;
                            break;
                        }

                        //压力加载
                        if (!BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.IsUpCompleted)
                        {
                            Trace.Write(i + "加载" + "\r\n");
                            if (!BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.IsUpStarted)
                            {
                                BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.UpStartTime = DateTime.Now;
                                BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.IsUpStarted = true;
                                BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.LoadUpStartValue = tempValueNow;
                            }
                            TimeSpan tempSpanUp = DateTime.Now - BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.UpStartTime;
                            //判断加压或泄压并计算当前给定。给定值=起始值+方向*速度*时间。
                            if (tempAim > Math.Abs(BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.LoadUpStartValue))
                                tempDerection = 1;
                            else if (tempAim < Math.Abs(BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.LoadUpStartValue))
                                tempDerection = -1;
                            tempGiven = Math.Abs(BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.LoadUpStartValue) +
                                        tempDerection * Math.Abs(BllDev.LoadUpDownSpeed) * tempSpanUp.TotalSeconds;
                            tempGiven = Math.Abs(tempGiven);
                            //判断加泄压给定增减是否结束
                            switch (tempDerection)
                            {
                                case 1:
                                    if (tempGiven >= tempAim)
                                    {
                                        BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.IsUpCompleted = true;
                                        tempGiven = tempAim;
                                    }
                                    break;

                                case -1:
                                    if (tempGiven <= tempAim)
                                    {
                                        BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.IsUpCompleted = true;
                                        tempGiven = tempAim;
                                    }
                                    break;
                            }
                            break;
                        }

                        //压力保持
                        if (!BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.IsKeepPressCompleted)
                        {
                            tempGiven = tempAim;
                            //判断是否进入压力保持范围
                            if ((tempValueNow >= (tempAim - tempPermitErr)) &&
                                (tempValueNow <= (tempAim + tempPermitErr)))
                            {
                                Trace.Write(i + "保压" + "\r\n");
                                if (!BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.IsKeepPressStarted)
                                {
                                    BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.IsKeepPressStarted = true;
                                    BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.KeepPressStartTime = DateTime.Now;
                                }
                                TimeSpan tempSpanKeep = DateTime.Now - BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.KeepPressStartTime;
                                //判定是否完成压力保持
                                if (tempSpanKeep.TotalSeconds >= tempStepPressKeepTime)
                                {
                                    Trace.Write(i + "结束" + "\r\n");
                                    BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.IsKeepPressCompleted = true;
                                    BllExp.Exp_QM.Step_DQ.IsStepCompleted = true;
                                    double tempSTL = 0;
                                    //根据粗管细管选择漏风量
                                    switch (BllDev.FSGUse)
                                    {
                                        case 1:
                                            tempSTL = Math.Abs(BllDev.FL1);
                                            break;
                                        case 2:
                                            tempSTL = Math.Abs(BllDev.FL2);
                                            break;
                                        case 3:
                                            tempSTL = Math.Abs(BllDev.FL3);
                                            break;
                                        default:
                                            tempSTL = Math.Abs(BllDev.FL2);
                                            break;
                                    }
                                    BllExp.Exp_QM.Step_DQ.StepResultSTL = tempSTL;
                                }
                            }
                            else
                            {
                                BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.IsKeepPressStarted = false;
                            }
                            break;
                        }
                    }
                }
                if (tempGiven < 0)
                    tempGiven = 0;
                tempErr = tempGiven - tempValueNow;
                PID_ParamModel tempPIDParam;
                switch (BllDev.FSGUse)
                {
                    case 1:
                        tempPIDParam = GePIDParam(1, Math.Abs(tempGiven));
                        break;
                    case 2:
                        tempPIDParam = GePIDParam(4, Math.Abs(tempGiven));
                        break;
                    case 3:
                        tempPIDParam = GePIDParam(6, Math.Abs(tempGiven));
                        break;
                    default:
                        tempPIDParam = GePIDParam(4, Math.Abs(tempGiven));
                        break;
                }

                //PID计算，分析状态。
                //如果所有步骤均完成，则阶段完成
                if (tempStageComplete)
                {
                    BllExp.Exp_QM.Stage_DQ.CompleteStatus = true;
                    PID_QM.PID_Param.ControllerEnable = false;
                    PID_QM.CalculatePID(0);
                }
                else
                {
                    PID_QM.PID_Param = tempPIDParam;
                    if (Math.Abs(tempGiven) < 0.01)
                        PID_QM.PID_Param.ControllerEnable = false; //预设压力为0时屏蔽pid
                    else
                        PID_QM.PID_Param.ControllerEnable = true; //pid使能
                    PID_QM.CalculatePID(tempErr);

                    //准备阶段
                    if (((StageNOInList == 0) || (StageNOInList == 2) || (StageNOInList == 4) || (StageNOInList == 6) || (StageNOInList == 8) || (StageNOInList == 10)) && (!BllExp.Exp_QM.Step_DQ.IsWaitBeforCompleted))
                    {
                        PID_QM.PID_Param.ControllerEnable = false;
                        PID_QM.CalculatePID(0);
                    }
                }
                //控制输出
                double tempUK_QM = PID_QM.UK;
                if (tempUK_QM <= 0)
                {
                    tempUK_QM = 0;
                }
                if (tempUK_QM >= 32000)
                {
                    tempUK_QM = 32000;
                }

                PLCCKCMDFrmBLL[3] = Convert.ToUInt16(tempUK_QM);

                //string kpidStr = "kp:" + PID_QM.PID_Param.Kp.ToString("0.00") + "ki:" +
                //                 PID_QM.PID_Param.Ki.ToString("0.00") + "kd:" + PID_QM.PID_Param.Kd.ToString("0.00");
                //string aimStr = tempGiven.ToString("0.00");
                //string ukStr = PID_QM.UK.ToString("0.00");
                //string ukpStr = PID_QM.UK_P.ToString("0.00");
                //string ukiStr = PID_QM.UK_I.ToString("0.00");
                //string ukdStr = PID_QM.UK_D.ToString("0.00");
                //Messenger.Default.Send<string>(kpidStr + "  Given:" + aimStr + "  ,U:" + ukStr + "  ,Up:" + ukpStr + "  ,Ui:" + ukiStr + "  ,Ud:" + ukdStr, "PIDinfoMessage");

                //判断检测是否完成
                if (BllExp.Exp_QM.Stage_DQ.CompleteStatus)
                {
                    //检测数据计算、保存
                    for (int i = 0; i < BllExp.Exp_QM.Stage_DQ.StepList.Count; i++)
                    {
                        BllExp.ExpData_QM.Stl_QMDJ[StageNOInList][i] =
                            BllExp.Exp_QM.Stage_DQ.StepList[i].StepResultSTL;
                    }
                    BllExp.QM_Evaluate();
                    Messenger.Default.Send<string>("SaveQMStatusAndData", "SaveExpMessage");
                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        Messenger.Default.Send<string>(BllExp.Exp_QM.Stage_DQ.Stage_Name + "已完成！", "OpenPrompt");
                    }));
                    Thread.Sleep(2000);
                    BllExp.Exp_QM.Stage_DQ = new MQZH_StageModel_QSM();
                    BllExp.Exp_QM.Step_DQ = new MQZH_StepModel_QSM();
                    EscStopCompleteExpReset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 气密工程阶段试验实施程序
        /// </summary>
        private void QMGCStageFunc()
        {
            double tempDerection = 0;
            double tempValueNow = 0;         //当前值
            double tempGiven = 0;       //给定值
            double tempErr;         //PID计算用误差
            double tempPermitErr;   //偏差允许范围
            double tempAim;     //阶段控制目标
            bool tempStageComplete = true;   //阶段完成
            double tempStepPressKeepTime;        //步骤保持时间
            int tempType;           //阶段类型

            try
            {
                BllDev.IsDeviceBusy = true;
                //如果当前阶段是默认阶段，则复位状态
                if ((BllExp.Exp_QM.Stage_DQ.Stage_NO == "99") || (StageNOInList == -1))
                {
                    MessageBox.Show("默认阶段不能做实验");
                    EscStopCompleteExpReset();
                    return;
                }
                //等待换向阀状态到位
                if (PPressTest && (!BllDev.Valve.DIList[5].IsOn))
                {
                    PLCCKCMDFrmBLL[3] = 0;
                    return;
                }
                if ((!PPressTest) && (!BllDev.Valve.DIList[6].IsOn))
                {
                    PLCCKCMDFrmBLL[3] = 0;
                    return;
                }

                //开始前提醒
                if (BllExp.Exp_QM.Stage_DQ.IsNeedTipsBefore &&
                    (!BllExp.Exp_QM.Stage_DQ.IsTipsBeforeComplete))
                {
                    MessageBox.Show(BllExp.Exp_QM.Stage_DQ.StringTipsBefore, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_QM.Stage_DQ.IsTipsBeforeComplete = true;
                }

                //逐个步骤计算给定值
                int stepsCount = BllExp.Exp_QM.Stage_DQ.StepList.Count;
                for (int i = 0; i < stepsCount; i++)
                {
                    //数据准备--------------------------------
                    StepNOInList = i;
                    //步骤压力稳定时间
                    if ((StageNOInList == 0) || (StageNOInList == 2) || (StageNOInList == 4) || (StageNOInList == 6) || (StageNOInList == 8) || (StageNOInList == 10))
                        tempStepPressKeepTime = BllDev.KeepingTime_YJY;
                    else
                    {
                        tempStepPressKeepTime = BllDev.KeepingTime_QMStep;
                    }
                    //步骤稳压目标值
                    tempType = 11;//气密工程
                    tempAim = GetAimPress(tempType, StageNOInList, StepNOInList);
                    tempAim = Math.Abs(tempAim);
                    //获取偏差允许范围
                    tempPermitErr = GetErr(tempAim);
                    //根据目标压力选择实测值，目标压力小于小压差量程上限的90%时用小压差
                    if (tempAim <= Math.Abs(BllDev.AIList[12].SingalUpperRange) * 0.9)
                    {
                        tempValueNow = Math.Abs(BllDev.AIList[12].ValueFinal);
                    }
                    else
                    {
                        tempValueNow = Math.Abs(BllDev.AIList[14].ValueFinal);
                    }
                    tempGiven = tempValueNow;

                    if (!BllExp.Exp_QM.Stage_DQ.StepList[i].IsStepCompleted)
                    {
                        tempStageComplete = false;
                        BllExp.Exp_QM.Step_DQ = BllExp.Exp_QM.Stage_DQ.StepList[i];

                        //步骤开始前等待
                        if (!BllExp.Exp_QM.Step_DQ.IsWaitBeforCompleted)
                        {
                            Trace.Write(i + "等待" + "\r\n");
                            if (!BllExp.Exp_QM.Step_DQ.IsWaitStarted)
                            {
                                BllExp.Exp_QM.Step_DQ.WaitStartTime = DateTime.Now;
                                BllExp.Exp_QM.Step_DQ.IsWaitStarted = true;
                            }
                            TimeSpan tempSpanWait = DateTime.Now - BllExp.Exp_QM.Step_DQ.WaitStartTime;
                            if (tempSpanWait.TotalSeconds >= BllExp.Exp_QM.Step_DQ.TimeWaitBefor)
                            {
                                BllExp.Exp_QM.Step_DQ.IsWaitBeforCompleted = true;
                                break;
                            }
                            //等待未完成或切换瞬间，保持PID输出
                            tempGiven = 0;
                            break;
                        }

                        //压力加载
                        if (!BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.IsUpCompleted)
                        {
                            Trace.Write(i + "加载" + "\r\n");
                            if (!BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.IsUpStarted)
                            {
                                BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.UpStartTime = DateTime.Now;
                                BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.IsUpStarted = true;
                                BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.LoadUpStartValue = tempValueNow;
                            }
                            TimeSpan tempSpanUp = DateTime.Now - BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.UpStartTime;
                            //判断加压或泄压并计算当前给定。给定值=起始值+方向*速度*时间。
                            if (tempAim > Math.Abs(BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.LoadUpStartValue))
                                tempDerection = 1;
                            else if (tempAim < Math.Abs(BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.LoadUpStartValue))
                                tempDerection = -1;
                            tempGiven = Math.Abs(BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.LoadUpStartValue) +
                                        tempDerection * Math.Abs(BllDev.LoadUpDownSpeed) * tempSpanUp.TotalSeconds;
                            tempGiven = Math.Abs(tempGiven);
                            //判断加泄压给定增减是否结束
                            switch (tempDerection)
                            {
                                case 1:
                                    if (tempGiven >= tempAim)
                                    {
                                        BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.IsUpCompleted = true;
                                        tempGiven = tempAim;
                                    }
                                    break;

                                case -1:
                                    if (tempGiven <= tempAim)
                                    {
                                        BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.IsUpCompleted = true;
                                        tempGiven = tempAim;
                                    }
                                    break;
                            }
                            break;
                        }

                        //压力保持
                        if (!BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.IsKeepPressCompleted)
                        {
                            Trace.Write(i + "保压" + "\r\n");
                            tempGiven = tempAim;
                            //判断是否进入压力保持范围
                            if ((tempValueNow >= (tempAim - tempPermitErr)) &&
                                (tempValueNow <= (tempAim + tempPermitErr)))
                            {
                                if (!BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.IsKeepPressStarted)
                                {
                                    BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.IsKeepPressStarted = true;
                                    BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.KeepPressStartTime = DateTime.Now;
                                }
                                TimeSpan tempSpanKeep = DateTime.Now - BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.KeepPressStartTime;
                                //判定是否完成压力保持
                                if (tempSpanKeep.TotalSeconds >= tempStepPressKeepTime)
                                {
                                    Trace.Write(i + "结束" + "\r\n");
                                    BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.IsKeepPressCompleted = true;
                                    BllExp.Exp_QM.Step_DQ.IsStepCompleted = true;
                                    double tempSTL = 0;
                                    //根据粗管细管选择漏风量
                                    switch (BllDev.FSGUse)
                                    {
                                        case 1:
                                            tempSTL = Math.Abs(BllDev.FL1);
                                            break;
                                        case 2:
                                            tempSTL = Math.Abs(BllDev.FL2);
                                            break;
                                        case 3:
                                            tempSTL = Math.Abs(BllDev.FL3);
                                            break;
                                        default:
                                            tempSTL = Math.Abs(BllDev.FL2);
                                            break;
                                    }
                                    BllExp.Exp_QM.Step_DQ.StepResultSTL = tempSTL;
                                }
                            }
                            else
                            {
                                BllExp.Exp_QM.Step_DQ.StepSteadyPStatus.IsKeepPressStarted = false;
                            }
                            break;
                        }
                    }
                }
                if (tempGiven < 0)
                    tempGiven = 0;
                tempErr = tempGiven - tempValueNow;
                PID_ParamModel tempPIDParam;
                switch (BllDev.FSGUse)
                {
                    case 1:
                        tempPIDParam = GePIDParam(1, Math.Abs(tempGiven));
                        break;
                    case 2:
                        tempPIDParam = GePIDParam(4, Math.Abs(tempGiven));
                        break;
                    case 3:
                        tempPIDParam = GePIDParam(6, Math.Abs(tempGiven));
                        break;
                    default:
                        tempPIDParam = GePIDParam(4, Math.Abs(tempGiven));
                        break;
                }
                //PID计算，分析状态。
                //如果所有步骤均完成，则阶段完成
                if (tempStageComplete)
                {
                    BllExp.Exp_QM.Stage_DQ.CompleteStatus = true;
                    PID_QM.PID_Param.ControllerEnable = false;
                    PID_QM.CalculatePID(0);
                }
                else
                {
                    PID_QM.PID_Param = tempPIDParam;
                    if (Math.Abs(tempGiven) < 0.01)
                        PID_QM.PID_Param.ControllerEnable = false; //预设压力为0时屏蔽pid
                    else
                        PID_QM.PID_Param.ControllerEnable = true; //pid使能
                    PID_QM.CalculatePID(tempErr);

                    //准备阶段
                    if (((StageNOInList == 0) || (StageNOInList == 2) || (StageNOInList == 4) || (StageNOInList == 6) || (StageNOInList == 8) || (StageNOInList == 10)) && (!BllExp.Exp_QM.Step_DQ.IsWaitBeforCompleted))
                    {
                        PID_QM.PID_Param.ControllerEnable = false;
                        PID_QM.CalculatePID(0);
                    }
                }
                //控制输出
                double tempUK_QM = PID_QM.UK;
                if (tempUK_QM <= 0)
                {
                    tempUK_QM = 0;
                }
                if (tempUK_QM >= 32000)
                {
                    tempUK_QM = 32000;
                }
                PLCCKCMDFrmBLL[3] = Convert.ToUInt16(tempUK_QM);


                //string aimStr = tempGiven.ToString("0.00");
                //string ukStr = PID_QM.UK.ToString("0.00");
                //string ukpStr = PID_QM.UK_P.ToString("0.00");
                //string ukiStr = PID_QM.UK_I.ToString("0.00");
                //string ukdStr = PID_QM.UK_D.ToString("0.00");
                //Messenger.Default.Send<string>("Given:" + aimStr + "  ,U:" + ukStr + "  ,Up:" + ukpStr + "  ,Ui:" + ukiStr + "  ,Ud:" + ukdStr, "PIDinfoMessage");

                //判断是否完成
                if (BllExp.Exp_QM.Stage_DQ.CompleteStatus)
                {
                    //检测数据计算、保存
                    for (int i = 0; i < BllExp.Exp_QM.Stage_DQ.StepList.Count; i++)
                    {
                        BllExp.ExpData_QM.Stl_QMGC[StageNOInList][i] =
                            BllExp.Exp_QM.Stage_DQ.StepList[i].StepResultSTL;
                    }
                    BllExp.QM_Evaluate();
                    Messenger.Default.Send<string>("SaveQMStatusAndData", "SaveExpMessage");
                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        Messenger.Default.Send<string>(BllExp.Exp_QM.Stage_DQ.Stage_Name + "已完成！", "OpenPrompt");
                    }));
                    Thread.Sleep(2000);
                    BllExp.Exp_QM.Stage_DQ = new MQZH_StageModel_QSM();
                    BllExp.Exp_QM.Step_DQ = new MQZH_StepModel_QSM();
                    EscStopCompleteExpReset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


        #region 气密检测启停消息

        /// <summary>
        /// 气密试验控制消息
        /// </summary>
        /// <param name="msg"></param>
        private void QMCtrlMessage(int msg)
        {
            try
            {
                //气密定级检测试验开始消息分析。从第一个试验阶段开始判断。-------------------------------------
                if (msg == 1101)
                {
                    //依次分析气密定级试验各阶段
                    bool tempExist = false;
                    for (int i = 0; i < 12; i++)
                    {
                        if (BllExp.Exp_QM.BeenCheckedDJ[i])
                        {
                            if (!BllExp.Exp_QM.StageList_QMDJ[i].NeedTest)
                            {
                                MessageBox.Show(BllExp.Exp_QM.StageList_QMDJ[i].Stage_Name + "无需检测！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                                BllExp.Exp_QM.BeenCheckedDJ[i] = false;
                                return;
                            }
                            if (BllExp.Exp_QM.StageList_QMDJ[i].CompleteStatus)
                            {
                                MessageBoxResult msgBoxResult = MessageBox.Show(BllExp.Exp_QM.StageList_QMDJ[i].Stage_Name + "，将覆盖已完成的数据，是否覆盖？", "数据覆盖提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                                if (msgBoxResult == MessageBoxResult.Yes)
                                {
                                    tempExist = true;
                                    StageNOInList = i;
                                    BllExp.Exp_QM.DJStageInitByNo(StageNOInList);
                                    break;
                                }
                                if (msgBoxResult == MessageBoxResult.No)
                                {
                                    BllExp.Exp_QM.BeenCheckedDJ[i] = false;
                                }
                            }
                            else
                            {
                                StageNOInList = i;
                                BllExp.Exp_QM.DJStageInitByNo(StageNOInList);
                                tempExist = true;
                                break;
                            }
                        }
                    }
                    if (tempExist)
                    {
                        BllExp.Exp_QM.Stage_DQ = null;
                        BllExp.Exp_QM.Stage_DQ = BllExp.Exp_QM.StageList_QMDJ[StageNOInList];

                        //正负压换向阀
                        switch (StageNOInList)
                        {
                            //正压
                            case 0:
                            case 1:
                            case 4:
                            case 5:
                            case 8:
                            case 9:
                                PPressTest = true;
                                break;
                            //负压
                            case 2:
                            case 3:
                            case 6:
                            case 7:
                            case 10:
                            case 11:
                                PPressTest = false;
                                break;
                        }
                    }
                    ExistQMDJExp = tempExist;
                }

                //气密工程检测试验开始消息分析。---------------------------------------------------------------
                else if (msg == 1111)
                {
                    //依次分析气密工程试验各阶段
                    bool tempExist = false;
                    for (int i = 0; i < 12; i++)
                    {
                        if (BllExp.Exp_QM.BeenCheckedGC[i])
                        {
                            if (!BllExp.Exp_QM.StageList_QMGC[i].NeedTest)
                            {
                                MessageBox.Show(BllExp.Exp_QM.StageList_QMGC[i].Stage_Name + "无需检测！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                                BllExp.Exp_QM.BeenCheckedGC[i] = false;
                                return;
                            }
                            if (BllExp.Exp_QM.StageList_QMGC[i].CompleteStatus)
                            {
                                MessageBoxResult msgBoxResult = MessageBox.Show(BllExp.Exp_QM.StageList_QMGC[i].Stage_Name + "，将覆盖已完成的数据，是否覆盖？", "数据覆盖提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                                if (msgBoxResult == MessageBoxResult.Yes)
                                {
                                    tempExist = true;
                                    StageNOInList = i;
                                    BllExp.Exp_QM.GCStageInitByNo(StageNOInList);
                                    break;
                                }
                                if (msgBoxResult == MessageBoxResult.No)
                                {
                                    BllExp.Exp_QM.BeenCheckedGC[i] = false;
                                }
                            }
                            else
                            {
                                StageNOInList = i;
                                BllExp.Exp_QM.GCStageInitByNo(StageNOInList);
                                tempExist = true;
                                break;
                            }
                        }
                    }
                    if (tempExist)
                    {
                        BllExp.Exp_QM.Stage_DQ = null;
                        BllExp.Exp_QM.Stage_DQ = BllExp.Exp_QM.StageList_QMGC[StageNOInList];

                        //正负压换向阀
                        switch (StageNOInList)
                        {
                            //正压
                            case 0:
                            case 1:
                            case 4:
                            case 5:
                            case 8:
                            case 9:
                                PPressTest = true;
                                break;
                            //负压
                            case 2:
                            case 3:
                            case 6:
                            case 7:
                            case 10:
                            case 11:
                                PPressTest = false;
                                break;
                        }
                    }
                    ExistQMGCExp = tempExist;
                }

                else if (msg == 7205)   //退出气密试验窗口
                {
                    if (ExistQMDJExp || ExistQMGCExp)
                    {
                        MessageBoxResult msgBoxResult = MessageBox.Show("是否停止正在进行的试验？", "提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                        if (msgBoxResult == MessageBoxResult.Yes)
                        {
                            Messenger.Default.Send<int>(1201, "StopExpMessage");
                            Messenger.Default.Send<string>(MQZH_WinName.QMWinName, "CloseGivenNameWin");
                        }
                    }
                    else
                        Messenger.Default.Send<string>(MQZH_WinName.QMWinName, "CloseGivenNameWin");
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
        /// 有未完成气密定级试验
        /// </summary>
        private bool _existQMDJExp = false;
        /// <summary>
        /// 有未完成气密定级试验
        /// </summary>
        public bool ExistQMDJExp
        {
            get { return _existQMDJExp; }
            set
            {
                _existQMDJExp = value;
                RaisePropertyChanged(() => ExistQMDJExp);
            }
        }

        /// <summary>
        /// 有未完成气密工程试验
        /// </summary>
        private bool _existQMGCExp = false;
        /// <summary>
        /// 有未完成气密工程试验
        /// </summary>
        public bool ExistQMGCExp
        {
            get { return _existQMGCExp; }
            set
            {
                _existQMGCExp = value;
                RaisePropertyChanged(() => ExistQMGCExp);
            }
        }

        #endregion
    }
}