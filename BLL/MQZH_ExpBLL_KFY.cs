/************************************************************************************
 * 描述：
 * 幕墙四性试验操作业务BLL——抗风压部分
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2021/11/20       19:02:11		郝正强			V1.0.0.0
 * 
 ************************************************************************************/

using System;
using System.Diagnostics;
using System.Threading;
using GalaSoft.MvvmLight;
using MQZHWL.Model.Exp;
using System.Windows;
using static MQZHWL.Model.MQZH_Enums;
using CtrlMethod;
using GalaSoft.MvvmLight.Messaging;
using MQZHWL.Model;

namespace MQZHWL.BLL
{
    /// <summary>
    /// 主控类
    /// </summary>
    public partial class MQZH_ExpBLL : ObservableObject
    {

        /// <summary>
        /// 抗风压PID风机1
        /// </summary>
        private PID_CtrlModel _pid_KFY1 = new PID_CtrlModel();
        /// <summary>
        /// 抗风压PID风机1
        /// </summary>
        public PID_CtrlModel PID_KFY1
        {
            get { return _pid_KFY1; }
            set
            {
                _pid_KFY1 = value;
                RaisePropertyChanged(() => PID_KFY1);
            }
        }

        #region 抗风压变形阶段试验实施程序

        /// <summary>
        /// 抗风压定级p1阶段 实施程序
        /// </summary>
        private void KFYp1DJStageFunc()
        {
            double tempValueNow = 0;            //当前值
            double tempGiven = 0;               //给定值
            double tempErr;                    //PID计算用误差
            double tempPermitErr;               //偏差允许范围
            double tempAim;                     //阶段控制目标
            bool tempStageComplete = true;      //阶段完成
            double tempStepPressKeepTime = 0;   //步骤保持时间
            int tempType;                      //阶段类型

            try
            {
                BllDev.IsDeviceBusy = true;
                //如果当前阶段是默认阶段，则复位状态
                if ((BllExp.Exp_KFY.Stage_DQ.Stage_NO == "99") || (StageNOInList == -1))
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

                //更新初值
                if (!InitialValueSaved)
                {
                    BllExp.Exp_KFY.DisplaceGroups[0].SetWYCS();
                    BllExp.Exp_KFY.DisplaceGroups[1].SetWYCS();
                    BllExp.Exp_KFY.DisplaceGroups[2].SetWYCS();

                    if (StageNOInList == 1)
                    {
                        BllExp.ExpData_KFY.WY_DJBX_Z[0][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJBX_Z[0][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJBX_Z[0][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_DJBX_Z[0][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_DJBX_Z[0][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJBX_Z[0][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJBX_Z[0][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_DJBX_Z[0][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJBX_Z[0][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJBX_Z[0][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                    }
                    if (StageNOInList == 3)
                    {
                        BllExp.ExpData_KFY.WY_DJBX_F[0][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJBX_F[0][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJBX_F[0][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_DJBX_F[0][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_DJBX_F[0][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJBX_F[0][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJBX_F[0][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_DJBX_F[0][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJBX_F[0][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJBX_F[0][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                    }
                    InitialValueSaved = true;
                }

                //开始前提醒
                if (BllExp.Exp_KFY.Stage_DQ.IsNeedTipsBefore && (!BllExp.Exp_KFY.Stage_DQ.IsTipsBeforeComplete))
                {
                    MessageBox.Show(BllExp.Exp_KFY.Stage_DQ.StringTipsBefore, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_KFY.Stage_DQ.IsTipsBeforeComplete = true;
                }


                //逐个步骤计算给定值
                int stepsCount = BllExp.Exp_KFY.Stage_DQ.StepList.Count;
                for (int i = 0; i < stepsCount; i++)
                {
                    //数据准备--------------------------------
                    StepNOInList = i;
                    //步骤压力稳定时间
                    if ((StageNOInList == 0) || (StageNOInList == 2))
                        tempStepPressKeepTime = BllDev.KeepingTime_YJY;
                    else if ((StageNOInList == 1) || (StageNOInList == 3))
                    {
                        tempStepPressKeepTime = BllDev.KeepingTime_KFY_BXstep;
                    }
                    //步骤稳压目标值
                    tempType = 31;
                    tempAim = GetAimPress(tempType, StageNOInList, StepNOInList);
                    tempAim = Math.Abs(tempAim);
                    //获取偏差允许范围
                    tempPermitErr = GetErrBig(tempAim);
                    tempValueNow = Math.Abs(BllDev.AIList[14].ValueFinal);
                    tempGiven = tempValueNow;

                    if (!BllExp.Exp_KFY.Stage_DQ.StepList[i].IsStepCompleted)
                    {
                        tempStageComplete = false;
                        BllExp.Exp_KFY.Step_DQ = BllExp.Exp_KFY.Stage_DQ.StepList[i];

                        //步骤开始前等待
                        if (!BllExp.Exp_KFY.Step_DQ.IsWaitBeforCompleted)
                        {
                            if (!BllExp.Exp_KFY.Step_DQ.IsWaitStarted)
                            {
                                BllExp.Exp_KFY.Step_DQ.WaitStartTime = DateTime.Now;
                                BllExp.Exp_KFY.Step_DQ.IsWaitStarted = true;
                            }
                            TimeSpan tempSpanWait = DateTime.Now - BllExp.Exp_KFY.Step_DQ.WaitStartTime;
                            if (tempSpanWait.TotalSeconds >= BllExp.Exp_KFY.Step_DQ.TimeWaitBefor)
                            {
                                BllExp.Exp_KFY.Step_DQ.IsWaitBeforCompleted = true;
                                break;
                            }
                            //等待未完成或切换瞬间，保持PID输出
                            else
                            {
                                //tempGiven = tempValueNow;
                                tempGiven = 0;
                                break;
                            }
                        }

                        //压力加载
                        if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpCompleted)
                        {
                            if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpStarted)
                            {
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.UpStartTime = DateTime.Now;
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpStarted = true;
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.LoadUpStartValue = tempValueNow;
                            }
                            TimeSpan tempSpanUp = DateTime.Now - BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.UpStartTime;
                            tempGiven = Math.Abs(BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.LoadUpStartValue) +
                                        Math.Abs(BllDev.LoadUpDownSpeed) * tempSpanUp.TotalSeconds;
                            //判断压力给定值增加是否结束
                            if (tempGiven >= tempAim)
                            {
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpCompleted = true;
                                tempGiven = tempAim;
                            }
                            break;
                        }

                        //压力保持
                        if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressCompleted)
                        {
                            tempGiven = tempAim;
                            //判断是否进入压力保持范围
                            if ((tempValueNow >= (tempAim - tempPermitErr)) && (tempValueNow <= (tempAim + tempPermitErr)))
                            {
                                if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressStarted)
                                {
                                    BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressStarted = true;
                                    BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.KeepPressStartTime = DateTime.Now;
                                }
                                TimeSpan tempSpanKeep = DateTime.Now - BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.KeepPressStartTime;
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.PressKeeppingTimes = tempSpanKeep.TotalSeconds;
                                //判定是否完成压力保持
                                if (tempSpanKeep.TotalSeconds >= tempStepPressKeepTime)
                                {
                                    //采集检测数据
                                    if (StageNOInList == 1)
                                    {
                                        //位移记录
                                        BllExp.ExpData_KFY.WY_DJBX_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_DJBX_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_DJBX_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_DJBX_Z[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                                        BllExp.ExpData_KFY.WY_DJBX_Z[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_DJBX_Z[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_DJBX_Z[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_DJBX_Z[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_DJBX_Z[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_DJBX_Z[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                                        //相对挠度
                                        BllExp.ExpData_KFY.XDND_DJBX_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                                        BllExp.ExpData_KFY.XDND_DJBX_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                                        BllExp.ExpData_KFY.XDND_DJBX_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                                        //挠度
                                        BllExp.ExpData_KFY.ND_DJBX_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                                        BllExp.ExpData_KFY.ND_DJBX_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                                        BllExp.ExpData_KFY.ND_DJBX_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                                        //检测压力
                                        BllExp.ExpData_KFY.TestPress_DJBX_Z[StepNOInList] = GetStdPress(31, StageNOInList, StepNOInList);
                                        //挠度最大值
                                        BllExp.ExpData_KFY.XDND_Max_DJBX_Z[StepNOInList + 1] = MaxOfObsAbs(BllExp.ExpData_KFY.XDND_DJBX_Z[StepNOInList + 1]);
                                        BllExp.ExpData_KFY.ND_Max_DJBX_Z[StepNOInList + 1] = MaxOfObsAbs(BllExp.ExpData_KFY.ND_DJBX_Z[StepNOInList + 1]);
                                    }
                                    if (StageNOInList == 3)
                                    {
                                        //位移记录
                                        BllExp.ExpData_KFY.WY_DJBX_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_DJBX_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_DJBX_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_DJBX_F[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                                        BllExp.ExpData_KFY.WY_DJBX_F[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_DJBX_F[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_DJBX_F[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_DJBX_F[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_DJBX_F[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_DJBX_F[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                                        //相对挠度
                                        BllExp.ExpData_KFY.XDND_DJBX_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                                        BllExp.ExpData_KFY.XDND_DJBX_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                                        BllExp.ExpData_KFY.XDND_DJBX_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                                        //挠度
                                        BllExp.ExpData_KFY.ND_DJBX_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                                        BllExp.ExpData_KFY.ND_DJBX_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                                        BllExp.ExpData_KFY.ND_DJBX_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                                        //检测压力
                                        BllExp.ExpData_KFY.TestPress_DJBX_F[StepNOInList] = GetStdPress(31, StageNOInList, StepNOInList);
                                        //挠度最大值
                                        BllExp.ExpData_KFY.XDND_Max_DJBX_F[StepNOInList + 1] = MaxOfObsAbs(BllExp.ExpData_KFY.XDND_DJBX_F[StepNOInList + 1]);
                                        BllExp.ExpData_KFY.ND_Max_DJBX_F[StepNOInList + 1] = MaxOfObsAbs(BllExp.ExpData_KFY.ND_DJBX_F[StepNOInList + 1]);
                                    }
                                    BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressCompleted = true;
                                    BllExp.Exp_KFY.Step_DQ.IsStepCompleted = true;
                                    //计算是否有挠度超限
                                    if ((StageNOInList == 1) || (StageNOInList == 3))
                                    {
                                        bool over = false;
                                        for (int ndIndex = 0; ndIndex < 3; ndIndex++)
                                        {
                                            if (BllExp.Exp_KFY.DisplaceGroups[0].Is_Use)
                                            {
                                                if (Math.Abs(BllExp.Exp_KFY.DisplaceGroups[0].ND) > Math.Abs(BllExp.Exp_KFY.DisplaceGroups[0].ND_YX_P1))
                                                {
                                                    over = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (over)
                                        {
                                            DemageOrderKFY = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
                if (tempGiven < 0)
                    tempGiven = 0;
                tempErr = tempGiven - tempValueNow;
                PID_ParamModel tempPIDParam = GePIDParam(2, Math.Abs(tempGiven));
                //损坏时
                if (DemageOrderKFY)
                {
                    //采集检测数据
                    if (StageNOInList == 1)
                    {
                        //位移记录
                        BllExp.ExpData_KFY.WY_DJBX_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJBX_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJBX_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_DJBX_Z[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_DJBX_Z[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJBX_Z[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJBX_Z[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_DJBX_Z[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJBX_Z[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJBX_Z[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                        //相对挠度
                        BllExp.ExpData_KFY.XDND_DJBX_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                        BllExp.ExpData_KFY.XDND_DJBX_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                        BllExp.ExpData_KFY.XDND_DJBX_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                        //挠度
                        BllExp.ExpData_KFY.ND_DJBX_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                        BllExp.ExpData_KFY.ND_DJBX_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                        BllExp.ExpData_KFY.ND_DJBX_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                        //检测压力
                        BllExp.ExpData_KFY.TestPress_DJBX_Z[StepNOInList] = GetStdPress(31, StageNOInList, StepNOInList);
                        //挠度最大值
                        BllExp.ExpData_KFY.XDND_Max_DJBX_Z[StepNOInList + 1] = MaxOfObsAbs(BllExp.ExpData_KFY.XDND_DJBX_Z[StepNOInList + 1]);
                        BllExp.ExpData_KFY.ND_Max_DJBX_Z[StepNOInList + 1] = MaxOfObsAbs(BllExp.ExpData_KFY.ND_DJBX_Z[StepNOInList + 1]);
                    }
                    if (StageNOInList == 3)
                    {
                        //位移记录
                        BllExp.ExpData_KFY.WY_DJBX_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJBX_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJBX_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_DJBX_F[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_DJBX_F[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJBX_F[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJBX_F[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_DJBX_F[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJBX_F[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJBX_F[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                        //相对挠度
                        BllExp.ExpData_KFY.XDND_DJBX_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                        BllExp.ExpData_KFY.XDND_DJBX_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                        BllExp.ExpData_KFY.XDND_DJBX_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                        //挠度
                        BllExp.ExpData_KFY.ND_DJBX_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                        BllExp.ExpData_KFY.ND_DJBX_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                        BllExp.ExpData_KFY.ND_DJBX_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                        //检测压力
                        BllExp.ExpData_KFY.TestPress_DJBX_F[StepNOInList] = GetStdPress(31, StageNOInList, StepNOInList);
                        //挠度最大值
                        BllExp.ExpData_KFY.XDND_Max_DJBX_F[StepNOInList + 1] = MaxOfObsAbs(BllExp.ExpData_KFY.XDND_DJBX_F[StepNOInList + 1]);
                        BllExp.ExpData_KFY.ND_Max_DJBX_F[StepNOInList + 1] = MaxOfObsAbs(BllExp.ExpData_KFY.ND_DJBX_F[StepNOInList + 1]);
                    }
                    tempStageComplete = true;
                }

                //PID计算，分析状态。
                //如果所有步骤均完成，则阶段完成
                if (tempStageComplete)
                {
                    BllExp.Exp_KFY.Stage_DQ.CompleteStatus = true;
                    PID_KFY1.PID_Param.ControllerEnable = false;
                    PID_KFY1.CalculatePID(0);
                }
                else
                {
                    PID_KFY1.PID_Param = tempPIDParam;
                    if (Math.Abs(tempGiven) < 0.01)
                        PID_KFY1.PID_Param.ControllerEnable = false; //预设压力为0时屏蔽pid
                    else
                        PID_KFY1.PID_Param.ControllerEnable = true; //pid使能
                    PID_KFY1.CalculatePID(tempErr);

                    //准备阶段
                    if (((StageNOInList == 0) || (StageNOInList == 2)) && (!BllExp.Exp_KFY.Step_DQ.IsWaitBeforCompleted))
                    {
                        PID_KFY1.PID_Param.ControllerEnable = false;
                        PID_KFY1.CalculatePID(0);
                    }
                    // Trace.Write("G" + tempGiven + ",  Now" + tempValueNow + ",  Aim" + tempAim + ",  Err" + tempErr + ",  U" + PID_KFY1.UK + "\r\n");
                    //  Trace.Write("aim" + tempStepPressKeepTime + "  now" +BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.PressKeeppingTimes + "\r\n");
                    // Trace.Write("step  " + StepNOInList + "\r\n");
                }
                //控制输出
                double tempUK_KFYp1 = PID_KFY1.UK;
                if (tempUK_KFYp1 <= 0)
                {
                    tempUK_KFYp1 = 0;
                }
                if (tempUK_KFYp1 >= 32000)
                {
                    tempUK_KFYp1 = 32000;
                }
                PLCCKCMDFrmBLL[3] = Convert.ToUInt16(tempUK_KFYp1);

                //    //PID计算数据显示
                //    string aimStr = tempGiven.ToString("0.00");
                //    string ukStr = PID_KFY1.UK.ToString("0.00");
                //    string ukpStr = PID_KFY1.UK_P.ToString("0.00");
                //    string ukiStr = PID_KFY1.UK_I.ToString("0.00");
                //    string ukdStr = PID_KFY1.UK_D.ToString("0.00");
                //    Messenger.Default.Send<string>("Given:" + aimStr + "  ,U:" + ukStr + "  ,Up:" + ukpStr + "  ,Ui:" + ukiStr + "  ,Ud:" + ukdStr, "PIDinfoMessage");

                //判断是否完成
                if (BllExp.Exp_KFY.Stage_DQ.CompleteStatus)
                {
                    //分析是否所有阶段都完成
                    bool tempAllStageComplete = true;
                    for (int i = 0; i < BllExp.Exp_KFY.StageList_KFYDJ.Count; i++)
                    {
                        if (!BllExp.Exp_KFY.StageList_KFYDJ[i].CompleteStatus)
                            tempAllStageComplete = false;
                    }
                    if (tempAllStageComplete)
                        BllExp.Exp_KFY.CompleteStatus = true;
                    Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        Messenger.Default.Send<string>(BllExp.Exp_KFY.Stage_DQ.Stage_Name + "已完成！", "OpenPrompt");
                    }));
                    //加压时，打开损坏确认窗口
                    if ((StageNOInList == 1) || (StageNOInList == 3))
                    {
                        OpenKFYDamageDJWin();
                    }
                    Thread.Sleep(2000);
                    BllExp.Exp_KFY.Stage_DQ = new MQZH_StageModel_QSM();
                    BllExp.Exp_KFY.Step_DQ = new MQZH_StepModel_QSM();
                    EscStopCompleteExpReset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 抗风压工程p1阶段 实施程序
        /// </summary>
        private void KFYp1GCStageFunc()
        {
            double tempValueNow = 0;        //当前值
            double tempGiven = 0;           //给定值
            double tempErr;                //PID计算用误差
            double tempPermitErr;          //偏差允许范围
            double tempAim;                //阶段控制目标
            bool tempStageComplete = true;  //阶段完成
            double tempStepPressKeepTime = 0; //步骤保持时间
            int tempType;                   //阶段类型

            try
            {
                BllDev.IsDeviceBusy = true;
                //如果当前阶段是默认阶段，则复位状态
                if ((BllExp.Exp_KFY.Stage_DQ.Stage_NO == "99") || (StageNOInList == -1))
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

                //更新初值
                if (!InitialValueSaved)
                {
                    BllExp.Exp_KFY.DisplaceGroups[0].SetWYCS();
                    BllExp.Exp_KFY.DisplaceGroups[1].SetWYCS();
                    BllExp.Exp_KFY.DisplaceGroups[2].SetWYCS();

                    if (StageNOInList == 1)
                    {
                        BllExp.ExpData_KFY.WY_GCBX_Z[0][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCBX_Z[0][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCBX_Z[0][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCBX_Z[0][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_GCBX_Z[0][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCBX_Z[0][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCBX_Z[0][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCBX_Z[0][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCBX_Z[0][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCBX_Z[0][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                    }
                    if (StageNOInList == 3)
                    {
                        BllExp.ExpData_KFY.WY_GCBX_F[0][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCBX_F[0][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCBX_F[0][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCBX_F[0][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_GCBX_F[0][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCBX_F[0][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCBX_F[0][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCBX_F[0][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCBX_F[0][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCBX_F[0][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                    }
                    InitialValueSaved = true;
                }

                //开始前提醒
                if (BllExp.Exp_KFY.Stage_DQ.IsNeedTipsBefore && (!BllExp.Exp_KFY.Stage_DQ.IsTipsBeforeComplete))
                {
                    MessageBox.Show(BllExp.Exp_KFY.Stage_DQ.StringTipsBefore, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_KFY.Stage_DQ.IsTipsBeforeComplete = true;
                }

                //逐个步骤计算给定值
                int stepsCount = BllExp.Exp_KFY.Stage_DQ.StepList.Count;
                for (int i = 0; i < stepsCount; i++)
                {
                    //数据准备--------------------------------
                    StepNOInList = i;
                    //步骤压力稳定时间
                    if ((StageNOInList == 0) || (StageNOInList == 2))
                        tempStepPressKeepTime = BllDev.KeepingTime_YJY;
                    else if ((StageNOInList == 1) || (StageNOInList == 3))
                    {
                        tempStepPressKeepTime = BllDev.KeepingTime_KFY_BXstep;
                    }
                    //步骤稳压目标值
                    tempType = 35;
                    tempAim = GetAimPress(tempType, StageNOInList, StepNOInList);
                    tempAim = Math.Abs(tempAim);
                    //获取偏差允许范围
                    tempPermitErr = GetErrBig(tempAim);
                    tempValueNow = Math.Abs(BllDev.AIList[14].ValueFinal);
                    tempGiven = tempValueNow;

                    if (!BllExp.Exp_KFY.Stage_DQ.StepList[i].IsStepCompleted)
                    {
                        tempStageComplete = false;
                        BllExp.Exp_KFY.Step_DQ = BllExp.Exp_KFY.Stage_DQ.StepList[i];


                        //步骤开始前等待
                        if (!BllExp.Exp_KFY.Step_DQ.IsWaitBeforCompleted)
                        {
                            if (!BllExp.Exp_KFY.Step_DQ.IsWaitStarted)
                            {
                                BllExp.Exp_KFY.Step_DQ.WaitStartTime = DateTime.Now;
                                BllExp.Exp_KFY.Step_DQ.IsWaitStarted = true;
                            }
                            TimeSpan tempSpanWait = DateTime.Now - BllExp.Exp_KFY.Step_DQ.WaitStartTime;
                            if (tempSpanWait.TotalSeconds >= BllExp.Exp_KFY.Step_DQ.TimeWaitBefor)
                            {
                                BllExp.Exp_KFY.Step_DQ.IsWaitBeforCompleted = true;
                                break;
                            }
                            //等待未完成或切换瞬间，保持PID输出
                            else
                            {
                                //tempGiven = tempValueNow;
                                tempGiven = 0;
                                break;
                            }
                        }

                        //压力加载
                        if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpCompleted)
                        {
                            if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpStarted)
                            {
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.UpStartTime = DateTime.Now;
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpStarted = true;
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.LoadUpStartValue = tempValueNow;
                            }
                            TimeSpan tempSpanUp = DateTime.Now - BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.UpStartTime;
                            tempGiven = Math.Abs(BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.LoadUpStartValue) +
                                        Math.Abs(BllDev.LoadUpDownSpeed) * tempSpanUp.TotalSeconds;
                            //判断压力给定值增加是否结束
                            if (tempGiven >= tempAim)
                            {
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpCompleted = true;
                                tempGiven = tempAim;
                            }
                            break;
                        }

                        //压力保持
                        if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressCompleted)
                        {
                            tempGiven = tempAim;
                            //判断是否进入压力保持范围
                            if ((tempValueNow >= (tempAim - tempPermitErr)) && (tempValueNow <= (tempAim + tempPermitErr)))
                            {
                                if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressStarted)
                                {
                                    BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressStarted = true;
                                    BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.KeepPressStartTime = DateTime.Now;
                                }
                                TimeSpan tempSpanKeep = DateTime.Now - BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.KeepPressStartTime;
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.PressKeeppingTimes = tempSpanKeep.TotalSeconds;
                                //判定是否完成压力保持
                                if (tempSpanKeep.TotalSeconds >= tempStepPressKeepTime)
                                {
                                    //采集检测数据
                                    if (StageNOInList == 1)
                                    {
                                        //位移记录
                                        BllExp.ExpData_KFY.WY_GCBX_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_GCBX_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_GCBX_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_GCBX_Z[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                                        BllExp.ExpData_KFY.WY_GCBX_Z[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_GCBX_Z[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_GCBX_Z[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_GCBX_Z[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_GCBX_Z[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_GCBX_Z[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                                        //相对挠度
                                        BllExp.ExpData_KFY.XDND_GCBX_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                                        BllExp.ExpData_KFY.XDND_GCBX_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                                        BllExp.ExpData_KFY.XDND_GCBX_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                                        //挠度
                                        BllExp.ExpData_KFY.ND_GCBX_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                                        BllExp.ExpData_KFY.ND_GCBX_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                                        BllExp.ExpData_KFY.ND_GCBX_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                                        //检测压力
                                        BllExp.ExpData_KFY.TestPress_GCBX_Z[StepNOInList] = GetStdPress(35, StageNOInList, StepNOInList);
                                        //挠度最大值
                                        BllExp.ExpData_KFY.XDND_Max_GCBX_Z[StepNOInList + 1] = MaxOfObsAbs(BllExp.ExpData_KFY.XDND_GCBX_Z[StepNOInList + 1]);
                                        BllExp.ExpData_KFY.ND_Max_GCBX_Z[StepNOInList + 1] = MaxOfObsAbs(BllExp.ExpData_KFY.ND_GCBX_Z[StepNOInList + 1]);
                                    }
                                    if (StageNOInList == 3)
                                    {
                                        //位移记录
                                        BllExp.ExpData_KFY.WY_GCBX_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_GCBX_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_GCBX_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_GCBX_F[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                                        BllExp.ExpData_KFY.WY_GCBX_F[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_GCBX_F[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_GCBX_F[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_GCBX_F[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_GCBX_F[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_GCBX_F[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                                        //相对挠度
                                        BllExp.ExpData_KFY.XDND_GCBX_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                                        BllExp.ExpData_KFY.XDND_GCBX_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                                        BllExp.ExpData_KFY.XDND_GCBX_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                                        //挠度
                                        BllExp.ExpData_KFY.ND_GCBX_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                                        BllExp.ExpData_KFY.ND_GCBX_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                                        BllExp.ExpData_KFY.ND_GCBX_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                                        //检测压力
                                        BllExp.ExpData_KFY.TestPress_GCBX_F[StepNOInList] = GetStdPress(35, StageNOInList, StepNOInList);
                                        //挠度最大值
                                        BllExp.ExpData_KFY.XDND_Max_GCBX_F[StepNOInList + 1] = MaxOfObsAbs(BllExp.ExpData_KFY.XDND_GCBX_F[StepNOInList + 1]);
                                        BllExp.ExpData_KFY.ND_Max_GCBX_F[StepNOInList + 1] = MaxOfObsAbs(BllExp.ExpData_KFY.ND_GCBX_F[StepNOInList + 1]);
                                    }
                                    //完成状态标志
                                    BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressCompleted = true;
                                    BllExp.Exp_KFY.Step_DQ.IsStepCompleted = true;
                                }
                            }
                            break;
                        }
                    }
                }
                if (tempGiven < 0)
                    tempGiven = 0;
                tempErr = tempGiven - tempValueNow;
                PID_ParamModel tempPIDParam = GePIDParam(2, Math.Abs(tempGiven));
                //损坏时
                if (DemageOrderKFY)
                {
                    //采集检测数据
                    if (StageNOInList == 1)
                    {
                        //位移记录
                        BllExp.ExpData_KFY.WY_GCBX_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCBX_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCBX_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCBX_Z[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_GCBX_Z[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCBX_Z[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCBX_Z[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCBX_Z[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCBX_Z[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCBX_Z[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                        //相对挠度
                        BllExp.ExpData_KFY.XDND_GCBX_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                        BllExp.ExpData_KFY.XDND_GCBX_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                        BllExp.ExpData_KFY.XDND_GCBX_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                        //挠度
                        BllExp.ExpData_KFY.ND_GCBX_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                        BllExp.ExpData_KFY.ND_GCBX_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                        BllExp.ExpData_KFY.ND_GCBX_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                        //检测压力
                        BllExp.ExpData_KFY.TestPress_GCBX_Z[StepNOInList] = GetStdPress(35, StageNOInList, StepNOInList);
                        //挠度最大值
                        BllExp.ExpData_KFY.XDND_Max_GCBX_Z[StepNOInList + 1] = MaxOfObsAbs(BllExp.ExpData_KFY.XDND_GCBX_Z[StepNOInList + 1]);
                        BllExp.ExpData_KFY.ND_Max_GCBX_Z[StepNOInList + 1] = MaxOfObsAbs(BllExp.ExpData_KFY.ND_GCBX_Z[StepNOInList + 1]);
                    }
                    if (StageNOInList == 3)
                    {
                        //位移记录
                        BllExp.ExpData_KFY.WY_GCBX_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCBX_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCBX_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCBX_F[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_GCBX_F[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCBX_F[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCBX_F[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCBX_F[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCBX_F[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCBX_F[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                        //相对挠度
                        BllExp.ExpData_KFY.XDND_GCBX_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                        BllExp.ExpData_KFY.XDND_GCBX_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                        BllExp.ExpData_KFY.XDND_GCBX_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                        //挠度
                        BllExp.ExpData_KFY.ND_GCBX_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                        BllExp.ExpData_KFY.ND_GCBX_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                        BllExp.ExpData_KFY.ND_GCBX_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                        //检测压力
                        BllExp.ExpData_KFY.TestPress_GCBX_F[StepNOInList] = GetStdPress(35, StageNOInList, StepNOInList);
                        //挠度最大值
                        BllExp.ExpData_KFY.XDND_Max_GCBX_F[StepNOInList + 1] = MaxOfObsAbs(BllExp.ExpData_KFY.XDND_GCBX_F[StepNOInList + 1]);
                        BllExp.ExpData_KFY.ND_Max_GCBX_F[StepNOInList + 1] = MaxOfObsAbs(BllExp.ExpData_KFY.ND_GCBX_F[StepNOInList + 1]);
                    }
                    tempStageComplete = true;
                }

                //PID计算，分析状态。
                //如果所有步骤均完成，则阶段完成
                if (tempStageComplete)
                {
                    BllExp.Exp_KFY.Stage_DQ.CompleteStatus = true;
                    PID_KFY1.PID_Param.ControllerEnable = false;
                    PID_KFY1.CalculatePID(0);
                }
                else
                {
                    PID_KFY1.PID_Param = tempPIDParam;
                    if (Math.Abs(tempGiven) < 0.01)
                        PID_KFY1.PID_Param.ControllerEnable = false; //预设压力为0时屏蔽pid
                    else
                        PID_KFY1.PID_Param.ControllerEnable = true; //pid使能
                    PID_KFY1.CalculatePID(tempErr);

                    //准备阶段
                    if (((StageNOInList == 0) || (StageNOInList == 2)) && (!BllExp.Exp_KFY.Step_DQ.IsWaitBeforCompleted))
                    {
                        PID_KFY1.PID_Param.ControllerEnable = false;
                        PID_KFY1.CalculatePID(0);
                    }
                }
                //控制输出
                double tempUK_KFYp1 = PID_KFY1.UK;
                if (tempUK_KFYp1 <= 0)
                {
                    tempUK_KFYp1 = 0;
                }
                if (tempUK_KFYp1 >= 32000)
                {
                    tempUK_KFYp1 = 32000;
                }
                PLCCKCMDFrmBLL[3] = Convert.ToUInt16(tempUK_KFYp1);

                ////PID计算数据显示
                //string aimStr = tempGiven.ToString("0.00");
                //string ukStr = PID_KFY1.UK.ToString("0.00");
                //string ukpStr = PID_KFY1.UK_P.ToString("0.00");
                //string ukiStr = PID_KFY1.UK_I.ToString("0.00");
                //string ukdStr = PID_KFY1.UK_D.ToString("0.00");
                //Messenger.Default.Send<string>("Given:" + aimStr + "  ,U:" + ukStr + "  ,Up:" + ukpStr + "  ,Ui:" + ukiStr + "  ,Ud:" + ukdStr, "PIDinfoMessage");

                //判断是否完成
                if (BllExp.Exp_KFY.Stage_DQ.CompleteStatus)
                {
                    //分析是否所有阶段都完成
                    bool tempAllStageComplete = true;
                    for (int i = 0; i < BllExp.Exp_KFY.StageList_KFYGC.Count; i++)
                    {
                        if (!BllExp.Exp_KFY.StageList_KFYGC[i].CompleteStatus)
                            tempAllStageComplete = false;
                    }
                    if (tempAllStageComplete)
                        BllExp.Exp_KFY.CompleteStatus = true;
                    //保存进度和数据
                    Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        Messenger.Default.Send<string>(BllExp.Exp_KFY.Stage_DQ.Stage_Name + "已完成！", "OpenPrompt");
                    }));                    //非预加压时，打开损坏确认窗口
                    if ((StageNOInList == 1) || (StageNOInList == 3))
                    {
                        OpenKFYDamageGCWin();
                    }
                    Thread.Sleep(2000);
                    BllExp.Exp_KFY.Stage_DQ = new MQZH_StageModel_QSM();
                    BllExp.Exp_KFY.Step_DQ = new MQZH_StepModel_QSM();
                    EscStopCompleteExpReset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


        #region 抗风压反复阶段试验实施程序

        /// <summary>
        /// 抗风压定级p2阶段 实施程序
        /// </summary>
        private void ExpKFYp2DJStageFunc()
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
            int WaveTimesAimKFY;                //波动次数

            try
            {
                BllDev.IsDeviceBusy = true;
                //如果当前阶段是默认阶段，则复位状态
                if ((BllExp.Exp_KFY.Stage_DQ.Stage_NO == "99") || (StageNOInList == -1))
                {
                    MessageBox.Show("默认阶段不能做实验");
                    EscStopCompleteExpReset();
                    TimerThreadLock = false;
                    return;
                }
                //更新初值
                if (!InitialValueSaved)
                {
                    BllExp.Exp_KFY.DisplaceGroups[0].SetWYCS();
                    BllExp.Exp_KFY.DisplaceGroups[1].SetWYCS();
                    BllExp.Exp_KFY.DisplaceGroups[2].SetWYCS();

                    InitialValueSaved = true;
                }
                //阶段开始时输出复位
                if (!IsStageTestStarted)
                {
                    IsStageTestStarted = true;
                    WavePhOut = 0;
                }

                //阶段开始前提醒
                if (BllExp.Exp_KFY.Stage_DQ.IsNeedTipsBefore &&
                        (!BllExp.Exp_KFY.Stage_DQ.IsTipsBeforeComplete))
                {
                    MessageBox.Show(BllExp.Exp_KFY.Stage_DQ.StringTipsBefore, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_KFY.Stage_DQ.IsTipsBeforeComplete = true;
                }

                //逐个步骤计算
                int stepsCount = BllExp.Exp_KFY.Stage_DQ.StepList.Count;
                for (int i = 0; i < stepsCount; i++)
                {

                    //-------数据准备-------
                    StepNOInList = i;

                    //步骤稳压目标值
                    tempType = 32;
                    tempAimAvg = GetAimPress(tempType, StageNOInList, StepNOInList);
                    tempAimAvg = Math.Abs(tempAimAvg);
                    tempAimL = tempAimAvg * BllDev.LowRatioKFY;
                    tempAimH = tempAimAvg * BllDev.HighRatioKFY;
                    BllExp.Exp_KFY.Stage_DQ.StepList[i].WaveAimAverageValue = tempAimAvg;
                    BllExp.Exp_KFY.Stage_DQ.StepList[i].WaveAimHBound = tempAimH;
                    BllExp.Exp_KFY.Stage_DQ.StepList[i].WaveAimLBound = tempAimL;
                    //压力准备保持时间
                    tempTimePreparePressKeep = BllDev.KeepingTime_SMPreparePress;
                    //波动次数
                    WaveTimesAimKFY = BllDev.WaveNum_KFYP2;
                    //获取偏差允许范围
                    tempPermitErrH = GetErrBig(tempAimH);
                    //当前压力
                    tempValueNow = Math.Abs(BllDev.AIList[14].ValueFinal);

                    if (!BllExp.Exp_KFY.Stage_DQ.StepList[i].IsStepCompleted)
                    {
                        tempStageComplete = false;
                        BllExp.Exp_KFY.Step_DQ = BllExp.Exp_KFY.Stage_DQ.StepList[i];

                        //-----步骤开始前等待-----
                        if (!BllExp.Exp_KFY.Step_DQ.IsWaitBeforCompleted)
                        {
                            if (!BllExp.Exp_KFY.Step_DQ.IsWaitStarted)
                            {
                                BllExp.Exp_KFY.Step_DQ.WaitStartTime = DateTime.Now;
                                BllExp.Exp_KFY.Step_DQ.IsWaitStarted = true;
                            }
                            TimeSpan tempSpanWait = DateTime.Now - BllExp.Exp_KFY.Step_DQ.WaitStartTime;
                            if (tempSpanWait.TotalSeconds >= BllExp.Exp_KFY.Step_DQ.TimeWaitBefor)
                            {
                                BllExp.Exp_KFY.Step_DQ.IsWaitBeforCompleted = true;
                            }
                            //等待未完成或切换瞬间，输出保持不变
                            else
                            {
                                break;
                            }
                        }

                        #region -----高压压力准备----------
                        if (!BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsKeepPressCompleted)
                        {
                            //等待换向阀状态到位
                            if (PPressTest)
                            {
                                PLCCKCMDFrmBLL[5] = 5;  //换向阀为正压模式
                                if (!BllDev.Valve.DIList[5].IsOn)
                                    break;
                            }
                            else
                            {
                                PLCCKCMDFrmBLL[5] = 6;  //换向阀为负压模式
                                if (!BllDev.Valve.DIList[6].IsOn)
                                    break;
                            }

                            //加载前等待5秒
                            if (!BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsLoadWaitComplete)
                            {
                                if (!BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsLoadWaitStart)
                                {
                                    BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.LoadWaitStartTime = DateTime.Now;
                                    BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsLoadWaitStart = true;
                                }
                                TimeSpan tempSpanUpHLoadWait = DateTime.Now - BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.LoadWaitStartTime;
                                if (tempSpanUpHLoadWait.TotalSeconds >= 5)
                                {
                                    BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsLoadWaitComplete = true;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            //高压力加载
                            if (!BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsUpCompleted)
                            {
                                if (!BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsUpStarted)
                                {
                                    BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.UpStartTime = DateTime.Now;
                                    BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsUpStarted = true;
                                    BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.LoadUpStartValue = tempValueNow;
                                }
                                TimeSpan tempSpanUpH = DateTime.Now - BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.UpStartTime;
                                tempGivenH = Math.Abs(BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.LoadUpStartValue) +
                                            Math.Abs(BllDev.LoadUpDownSpeed) * tempSpanUpH.TotalSeconds;
                                //判断压力给定值增加是否结束
                                if (tempGivenH >= tempAimH)
                                {
                                    BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsUpCompleted = true;
                                    tempGivenH = tempAimH;
                                }
                            }
                            //高压压力保持
                            else if ((tempValueNow >= (tempAimH - tempPermitErrH)) && (tempValueNow <= (tempAimH + tempPermitErrH)))
                            {
                                tempGivenH = tempAimH;
                                if (!BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsKeepPressStarted)
                                {
                                    BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsKeepPressStarted = true;
                                    BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.KeepPressStartTime = DateTime.Now;
                                }
                                TimeSpan tempSpanKeepH = DateTime.Now - BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.KeepPressStartTime;
                                BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.PressKeeppingTimes = tempSpanKeepH.TotalSeconds;
                                //判定是否完成压力保持
                                if (tempSpanKeepH.TotalSeconds >= tempTimePreparePressKeep)
                                {
                                    BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsKeepPressCompleted = true;
                                    Trace.Write("第" + i + "级高压目标" + tempAimH + "    高压保持" + tempValueNow + "\r\n");
                                    break;
                                }
                            }
                            else
                            {
                                tempGivenH = tempAimH;
                                BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsKeepPressStarted = false;
                                BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.KeepPressStartTime = DateTime.Now;
                            }

                            //高压按PID计算，低压输出保持不变
                            if (tempGivenH < 0)
                                tempGivenH = 0;
                            tempErrH = tempGivenH - tempValueNow;
                            PID_ParamModel tempPIDParamH = GePIDParam(2, Math.Abs(tempGivenH));
                            //PID计算
                            PID_KFY1.PID_Param = tempPIDParamH;
                            if (Math.Abs(tempGivenH) < 0.01)
                                PID_KFY1.PID_Param.ControllerEnable = false; //预设压力为0时屏蔽pid
                            else
                                PID_KFY1.PID_Param.ControllerEnable = true; //pid使能
                            PID_KFY1.CalculatePID(tempErrH);

                            WavePhOut = PID_KFY1.UK;
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

                        if (!BllExp.Exp_KFY.Step_DQ.StepWavePLowStatus.IsKeepPressCompleted)
                        {
                            if (PPressTest)
                                PLCCKCMDFrmBLL[5] = 7; //换向阀为正压低压准备模式
                            else
                                PLCCKCMDFrmBLL[5] = 8; //换向阀为负压低压准备模式
                            PLCCKCMDFrmBLL[6] = 500; //6、7准备脉冲频率
                            PLCCKCMDFrmBLL[7] = 0; //低压准备脉冲频率1000（15秒旋转八分之一圈）
                            PLCCKCMDFrmBLL[8] = 0; //7、8脉冲个数
                            PLCCKCMDFrmBLL[9] = 0; //0持续旋转
                            //计算设定压力的传输值
                            int pressID;
                            double pressSet = PPressTest ? tempAimL : (-tempAimL);
                            pressID = 14;
                            double setDataDouble = BllDev.AIList[pressID].GetY(BllDev.AIList[pressID].SingalLowerRange,
                                BllDev.AIList[pressID].SingalUpperRange, 0, 4000, pressSet);
                            PLCCKCMDFrmBLL[10] = Convert.ToUInt16(setDataDouble); //设定压力
                            PLCCKCMDFrmBLL[11] = 2; //压力通道(2大3中)

                            //等待换向阀低压准备到位
                            bool prepareOK = PPressTest ? BllDev.Valve.DIList[7].IsOn : BllDev.Valve.DIList[8].IsOn;
                            if (prepareOK)
                            {
                                BllExp.Exp_KFY.Step_DQ.StepWavePLowStatus.IsKeepPressCompleted = true;
                            }
                            else
                                break;
                        }

                        #endregion

                        #region-----压力切换----------

                        if (!BllExp.Exp_KFY.Step_DQ.IsWaveCompleted)
                        {
                            if (!BllExp.Exp_KFY.Step_DQ.IsWaveStarted)
                            {
                                BllExp.Exp_KFY.Step_DQ.WaveStartTime = DateTime.Now;
                                BllExp.Exp_KFY.Step_DQ.IsWaveStarted = true;
                                if (PPressTest)
                                    PLCCKCMDFrmBLL[5] = 9; //换向阀为次数波动正压模式
                                else
                                    PLCCKCMDFrmBLL[5] = 10; //换向阀为次数波动负压模式
                                PLCCKCMDFrmBLL[6] = 0; //6、7波动准备脉冲频率
                                PLCCKCMDFrmBLL[7] = 0;
                                PLCCKCMDFrmBLL[8] = 0; //7、8波动准备脉冲个数
                                PLCCKCMDFrmBLL[9] = 0; //0持续旋转
                                PLCCKCMDFrmBLL[10] = 0; //设定压力
                                PLCCKCMDFrmBLL[11] = 0;
                                PLCCKCMDFrmBLL[12] = 3000; //12、13波动脉冲频率
                                PLCCKCMDFrmBLL[13] = 0; //波动脉冲频率3000（2.5秒旋转十六分之一圈）
                                PLCCKCMDFrmBLL[14] = 0; //14、15波动脉冲频率修正
                                PLCCKCMDFrmBLL[15] = 0;
                                PLCCKCMDFrmBLL[16] = (ushort)(WaveTimesAimKFY * 2); //波动次数
                            }

                            //判断是否达到本步骤波动总次数
                            bool complete = PPressTest ? BllDev.Valve.DIList[9].IsOn : BllDev.Valve.DIList[10].IsOn;
                            if (complete)
                            {
                                //采集检测数据
                                if (StageNOInList == 4)
                                {
                                    //检测压力
                                    BllExp.ExpData_KFY.TestPress_DJP2_Z = GetStdPress(32, StageNOInList, StepNOInList);
                                }
                                if (StageNOInList == 5)
                                {
                                    //检测压力
                                    BllExp.ExpData_KFY.TestPress_DJP2_F = GetStdPress(32, StageNOInList, StepNOInList);
                                }

                                BllExp.Exp_KFY.Step_DQ.IsWaveCompleted = true;
                                BllExp.Exp_KFY.Stage_DQ.StepList[i].IsStepCompleted = true;
                                PLCCKCMDFrmBLL[5] = 5; //换向阀为正压模式
                                tempStageComplete = true;
                            }
                            break;
                        }

                        #endregion
                    }
                }

                //控制输出
                PLCCKCMDFrmBLL[3] = Convert.ToUInt16(WavePhOut);

                //损坏时
                if (DemageOrderKFY)
                {
                    //采集检测数据
                    if (StageNOInList == 4)
                    {
                        //检测压力
                        BllExp.ExpData_KFY.TestPress_DJP2_Z = GetStdPress(32, StageNOInList, StepNOInList);
                    }
                    if (StageNOInList == 5)
                    {
                        //检测压力
                        BllExp.ExpData_KFY.TestPress_DJP2_F = GetStdPress(32, StageNOInList, StepNOInList);
                    }
                    tempStageComplete = true;
                }
                if (tempStageComplete)
                {
                    BllExp.Exp_KFY.Stage_DQ.CompleteStatus = true;
                    PID_KFY1.PID_Param.ControllerEnable = false;
                    PID_KFY1.CalculatePID(0);
                    PID_KFY1.PID_Param.ControllerEnable = false;
                    PID_KFY1.CalculatePID(0);
                    WavePhOut = PID_KFY1.UK;
                }
                //PID计算数据显示
                //    string aimStr = tempGivenH.ToString("0.00");
                //    string ukStr = PID_KFY1.UK.ToString("0.00");
                //    string ukpStr = PID_KFY1.UK_P.ToString("0.00");
                //    string ukiStr = PID_KFY1.UK_I.ToString("0.00");
                //    string ukdStr = PID_KFY1.UK_D.ToString("0.00");
                //    Messenger.Default.Send<string>("GivenL:" + aimStr + "  ,U:" + ukStr + "  ,Up:" + ukpStr + "  ,Ui:" + ukiStr + "  ,Ud:" + ukdStr, "PIDinfoMessage");

                //判断是否完成
                if (BllExp.Exp_KFY.Stage_DQ.CompleteStatus)
                {
                    //分析是否所有阶段都完成
                    bool tempAllStageComplete = true;
                    for (int i = 0; i < BllExp.Exp_KFY.StageList_KFYDJ.Count; i++)
                    {
                        if (!BllExp.Exp_KFY.StageList_KFYDJ[i].CompleteStatus)
                            tempAllStageComplete = false;
                    }
                    if (tempAllStageComplete)
                        BllExp.Exp_KFY.CompleteStatus = true;
                    //保存进度和数据
                    Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        Messenger.Default.Send<string>(BllExp.Exp_KFY.Stage_DQ.Stage_Name + "已完成！", "OpenPrompt");
                    }));
                    //打开损坏确认窗口
                    OpenKFYDamageDJWin();
                    Thread.Sleep(2000);
                    BllExp.Exp_KFY.Stage_DQ = new MQZH_StageModel_QSM();
                    BllExp.Exp_KFY.Step_DQ = new MQZH_StepModel_QSM();
                    EscStopCompleteExpReset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 抗风压工程p2阶段 实施程序
        /// </summary>
        private void ExpKFYp2GCStageFunc()
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
            int WaveTimesAimKFY;                //波动次数

            try
            {
                BllDev.IsDeviceBusy = true;
                //如果当前阶段是默认阶段，则复位状态
                if ((BllExp.Exp_KFY.Stage_DQ.Stage_NO == "99") || (StageNOInList == -1))
                {
                    MessageBox.Show("默认阶段不能做实验");
                    EscStopCompleteExpReset();
                    TimerThreadLock = false;
                    return;
                }
                //更新初值
                if (!InitialValueSaved)
                {
                    BllExp.Exp_KFY.DisplaceGroups[0].SetWYCS();
                    BllExp.Exp_KFY.DisplaceGroups[1].SetWYCS();
                    BllExp.Exp_KFY.DisplaceGroups[2].SetWYCS();

                    InitialValueSaved = true;
                }
                //阶段开始时输出复位
                if (!IsStageTestStarted)
                {
                    IsStageTestStarted = true;
                    WavePhOut = 0;
                }

                //阶段开始前提醒
                if (BllExp.Exp_KFY.Stage_DQ.IsNeedTipsBefore &&
                        (!BllExp.Exp_KFY.Stage_DQ.IsTipsBeforeComplete))
                {
                    MessageBox.Show(BllExp.Exp_KFY.Stage_DQ.StringTipsBefore, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_KFY.Stage_DQ.IsTipsBeforeComplete = true;
                }

                //逐个步骤计算
                int stepsCount = BllExp.Exp_KFY.Stage_DQ.StepList.Count;
                for (int i = 0; i < stepsCount; i++)
                {

                    //-------数据准备-------
                    StepNOInList = i;

                    //步骤稳压目标值
                    tempType = 36;
                    tempAimAvg = GetAimPress(tempType, StageNOInList, StepNOInList);
                    tempAimAvg = Math.Abs(tempAimAvg);
                    tempAimL = tempAimAvg * BllDev.LowRatioKFY;
                    tempAimH = tempAimAvg * BllDev.HighRatioKFY;
                    BllExp.Exp_KFY.Stage_DQ.StepList[i].WaveAimAverageValue = tempAimAvg;
                    BllExp.Exp_KFY.Stage_DQ.StepList[i].WaveAimHBound = tempAimH;
                    BllExp.Exp_KFY.Stage_DQ.StepList[i].WaveAimLBound = tempAimL;
                    //压力准备保持时间
                    tempTimePreparePressKeep = BllDev.KeepingTime_SMPreparePress;
                    //波动次数
                    WaveTimesAimKFY = BllDev.WaveNum_KFYP2;
                    //获取偏差允许范围
                    tempPermitErrH = GetErrBig(tempAimH);
                    //当前压力
                    tempValueNow = Math.Abs(BllDev.AIList[14].ValueFinal);

                    if (!BllExp.Exp_KFY.Stage_DQ.StepList[i].IsStepCompleted)
                    {
                        tempStageComplete = false;
                        BllExp.Exp_KFY.Step_DQ = BllExp.Exp_KFY.Stage_DQ.StepList[i];

                        //-----步骤开始前等待-----
                        if (!BllExp.Exp_KFY.Step_DQ.IsWaitBeforCompleted)
                        {
                            if (!BllExp.Exp_KFY.Step_DQ.IsWaitStarted)
                            {
                                BllExp.Exp_KFY.Step_DQ.WaitStartTime = DateTime.Now;
                                BllExp.Exp_KFY.Step_DQ.IsWaitStarted = true;
                            }
                            TimeSpan tempSpanWait = DateTime.Now - BllExp.Exp_KFY.Step_DQ.WaitStartTime;
                            if (tempSpanWait.TotalSeconds >= BllExp.Exp_KFY.Step_DQ.TimeWaitBefor)
                            {
                                BllExp.Exp_KFY.Step_DQ.IsWaitBeforCompleted = true;
                            }
                            //等待未完成或切换瞬间，输出保持不变
                            else
                            {
                                break;
                            }
                        }

                        #region -----高压压力准备----------
                        if (!BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsKeepPressCompleted)
                        {
                            //等待换向阀状态到位
                            if (PPressTest)
                            {
                                PLCCKCMDFrmBLL[5] = 5;  //换向阀为正压模式
                                if (!BllDev.Valve.DIList[5].IsOn)
                                    break;
                            }
                            else
                            {
                                PLCCKCMDFrmBLL[5] = 6;  //换向阀为负压模式
                                if (!BllDev.Valve.DIList[6].IsOn)
                                    break;
                            }

                            //加载前等待5秒
                            if (!BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsLoadWaitComplete)
                            {
                                if (!BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsLoadWaitStart)
                                {
                                    BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.LoadWaitStartTime = DateTime.Now;
                                    BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsLoadWaitStart = true;
                                }
                                TimeSpan tempSpanUpHLoadWait = DateTime.Now - BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.LoadWaitStartTime;
                                if (tempSpanUpHLoadWait.TotalSeconds >= 5)
                                {
                                    BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsLoadWaitComplete = true;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            //高压力加载
                            if (!BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsUpCompleted)
                            {
                                if (!BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsUpStarted)
                                {
                                    BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.UpStartTime = DateTime.Now;
                                    BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsUpStarted = true;
                                    BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.LoadUpStartValue = tempValueNow;
                                }
                                TimeSpan tempSpanUpH = DateTime.Now - BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.UpStartTime;
                                tempGivenH = Math.Abs(BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.LoadUpStartValue) +
                                            Math.Abs(BllDev.LoadUpDownSpeed) * tempSpanUpH.TotalSeconds;
                                //判断压力给定值增加是否结束
                                if (tempGivenH >= tempAimH)
                                {
                                    BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsUpCompleted = true;
                                    tempGivenH = tempAimH;
                                }
                            }
                            //高压压力保持
                            else if ((tempValueNow >= (tempAimH - tempPermitErrH)) && (tempValueNow <= (tempAimH + tempPermitErrH)))
                            {
                                tempGivenH = tempAimH;
                                if (!BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsKeepPressStarted)
                                {
                                    BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsKeepPressStarted = true;
                                    BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.KeepPressStartTime = DateTime.Now;
                                }
                                TimeSpan tempSpanKeepH = DateTime.Now - BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.KeepPressStartTime;
                                BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.PressKeeppingTimes = tempSpanKeepH.TotalSeconds;
                                //判定是否完成压力保持
                                if (tempSpanKeepH.TotalSeconds >= tempTimePreparePressKeep)
                                {
                                    BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsKeepPressCompleted = true;
                                    Trace.Write("第" + i + "级高压目标" + tempAimH + "    高压保持" + tempValueNow + "\r\n");
                                    break;
                                }
                            }
                            else
                            {
                                tempGivenH = tempAimH;
                                BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.IsKeepPressStarted = false;
                                BllExp.Exp_KFY.Step_DQ.StepWavePUpStatus.KeepPressStartTime = DateTime.Now;
                            }

                            //高压按PID计算，低压输出保持不变
                            if (tempGivenH < 0)
                                tempGivenH = 0;
                            tempErrH = tempGivenH - tempValueNow;
                            PID_ParamModel tempPIDParamH = GePIDParam(2, Math.Abs(tempGivenH));
                            //PID计算
                            PID_KFY1.PID_Param = tempPIDParamH;
                            if (Math.Abs(tempGivenH) < 0.01)
                                PID_KFY1.PID_Param.ControllerEnable = false; //预设压力为0时屏蔽pid
                            else
                                PID_KFY1.PID_Param.ControllerEnable = true; //pid使能
                            PID_KFY1.CalculatePID(tempErrH);

                            WavePhOut = PID_KFY1.UK;
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

                        if (!BllExp.Exp_KFY.Step_DQ.StepWavePLowStatus.IsKeepPressCompleted)
                        {
                            if (PPressTest)
                                PLCCKCMDFrmBLL[5] = 7; //换向阀为正压低压准备模式
                            else
                                PLCCKCMDFrmBLL[5] = 8; //换向阀为负压低压准备模式
                            PLCCKCMDFrmBLL[6] = 500; //6、7准备脉冲频率
                            PLCCKCMDFrmBLL[7] = 0; //低压准备脉冲频率1000（15秒旋转八分之一圈）
                            PLCCKCMDFrmBLL[8] = 0; //7、8脉冲个数
                            PLCCKCMDFrmBLL[9] = 0; //0持续旋转
                            //计算设定压力的传输值
                            int pressID;
                            double pressSet = PPressTest ? tempAimL : (-tempAimL);
                            pressID = 14;
                            double setDataDouble = BllDev.AIList[pressID].GetY(BllDev.AIList[pressID].SingalLowerRange,
                                BllDev.AIList[pressID].SingalUpperRange, 0, 4000, pressSet);
                            PLCCKCMDFrmBLL[10] = Convert.ToUInt16(setDataDouble); //设定压力
                            PLCCKCMDFrmBLL[11] = 2; //压力通道(2大3中)

                            //等待换向阀低压准备到位
                            bool prepareOK = PPressTest ? BllDev.Valve.DIList[7].IsOn : BllDev.Valve.DIList[8].IsOn;
                            if (prepareOK)
                            {
                                BllExp.Exp_KFY.Step_DQ.StepWavePLowStatus.IsKeepPressCompleted = true;
                            }
                            else
                                break;
                        }

                        #endregion

                        #region-----压力切换----------

                        if (!BllExp.Exp_KFY.Step_DQ.IsWaveCompleted)
                        {
                            if (!BllExp.Exp_KFY.Step_DQ.IsWaveStarted)
                            {
                                BllExp.Exp_KFY.Step_DQ.WaveStartTime = DateTime.Now;
                                BllExp.Exp_KFY.Step_DQ.IsWaveStarted = true;
                                if (PPressTest)
                                    PLCCKCMDFrmBLL[5] = 9; //换向阀为次数波动正压模式
                                else
                                    PLCCKCMDFrmBLL[5] = 10; //换向阀为次数波动负压模式
                                PLCCKCMDFrmBLL[6] = 0; //6、7波动准备脉冲频率
                                PLCCKCMDFrmBLL[7] = 0;
                                PLCCKCMDFrmBLL[8] = 0; //7、8波动准备脉冲个数
                                PLCCKCMDFrmBLL[9] = 0; //0持续旋转
                                PLCCKCMDFrmBLL[10] = 0; //设定压力
                                PLCCKCMDFrmBLL[11] = 0;
                                PLCCKCMDFrmBLL[12] = 3000; //12、13波动脉冲频率
                                PLCCKCMDFrmBLL[13] = 0; //波动脉冲频率3000（2.5秒旋转十六分之一圈）
                                PLCCKCMDFrmBLL[14] = 0; //14、15波动脉冲频率修正
                                PLCCKCMDFrmBLL[15] = 0;
                                PLCCKCMDFrmBLL[16] = (ushort)(WaveTimesAimKFY * 2); //波动次数
                            }

                            //判断是否达到本步骤波动总次数
                            bool complete = PPressTest ? BllDev.Valve.DIList[9].IsOn : BllDev.Valve.DIList[10].IsOn;
                            if (complete)
                            {
                                //采集检测数据
                                if (StageNOInList == 4)
                                {
                                    //检测压力
                                    BllExp.ExpData_KFY.TestPress_GCP2_Z = GetStdPress(36, StageNOInList, StepNOInList);
                                }
                                if (StageNOInList == 5)
                                {
                                    //检测压力
                                    BllExp.ExpData_KFY.TestPress_GCP2_F = GetStdPress(36, StageNOInList, StepNOInList);
                                }
                                BllExp.Exp_KFY.Step_DQ.IsWaveCompleted = true;
                                BllExp.Exp_KFY.Stage_DQ.StepList[i].IsStepCompleted = true;
                                PLCCKCMDFrmBLL[5] = 5; //换向阀为正压模式
                                tempStageComplete = true;
                            }
                            break;
                        }

                        #endregion
                    }
                }


                //损坏时
                if (DemageOrderKFY)
                {
                    //采集检测数据
                    if (StageNOInList == 4)
                    {
                        //检测压力
                        BllExp.ExpData_KFY.TestPress_GCP2_Z = GetStdPress(36, StageNOInList, StepNOInList);
                    }
                    if (StageNOInList == 5)
                    {
                        //检测压力
                        BllExp.ExpData_KFY.TestPress_GCP2_F = GetStdPress(36, StageNOInList, StepNOInList);
                    }
                    tempStageComplete = true;
                }
                if (tempStageComplete)
                {
                    BllExp.Exp_KFY.Stage_DQ.CompleteStatus = true;
                    PID_KFY1.PID_Param.ControllerEnable = false;
                    PID_KFY1.CalculatePID(0);
                    PID_KFY1.PID_Param.ControllerEnable = false;
                    PID_KFY1.CalculatePID(0);
                    WavePhOut = PID_KFY1.UK;
                }

                //控制输出
                PLCCKCMDFrmBLL[3] = Convert.ToUInt16(WavePhOut);

                //PID计算数据显示
                //    string aimStr = tempGivenH.ToString("0.00");
                //    string ukStr = PID_KFY1.UK.ToString("0.00");
                //    string ukpStr = PID_KFY1.UK_P.ToString("0.00");
                //    string ukiStr = PID_KFY1.UK_I.ToString("0.00");
                //    string ukdStr = PID_KFY1.UK_D.ToString("0.00");
                //    Messenger.Default.Send<string>("GivenL:" + aimStr + "  ,U:" + ukStr + "  ,Up:" + ukpStr + "  ,Ui:" + ukiStr + "  ,Ud:" + ukdStr, "PIDinfoMessage");

                //判断是否完成
                if (BllExp.Exp_KFY.Stage_DQ.CompleteStatus)
                {
                    //分析是否所有阶段都完成
                    bool tempAllStageComplete = true;
                    for (int i = 0; i < BllExp.Exp_KFY.StageList_KFYGC.Count; i++)
                    {
                        if (!BllExp.Exp_KFY.StageList_KFYGC[i].CompleteStatus)
                            tempAllStageComplete = false;
                    }
                    if (tempAllStageComplete)
                        BllExp.Exp_KFY.CompleteStatus = true;
                    //保存进度和数据
                    Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        Messenger.Default.Send<string>(BllExp.Exp_KFY.Stage_DQ.Stage_Name + "已完成！", "OpenPrompt");
                    }));
                    //打开损坏确认窗口
                    OpenKFYDamageGCWin();
                    Thread.Sleep(2000);
                    BllExp.Exp_KFY.Stage_DQ = new MQZH_StageModel_QSM();
                    BllExp.Exp_KFY.Step_DQ = new MQZH_StepModel_QSM();
                    EscStopCompleteExpReset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


        #region 抗风压标准值阶段试验实施程序

        /// <summary>
        /// 抗风压定级p3阶段 实施程序
        /// </summary>
        private void KFYp3DJStageFunc()
        {
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
                if ((BllExp.Exp_KFY.Stage_DQ.Stage_NO == "99") || (StageNOInList == -1))
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

                //更新初值
                if (!InitialValueSaved)
                {
                    BllExp.Exp_KFY.DisplaceGroups[0].SetWYCS();
                    BllExp.Exp_KFY.DisplaceGroups[1].SetWYCS();
                    BllExp.Exp_KFY.DisplaceGroups[2].SetWYCS();

                    if (StageNOInList == 6)
                    {
                        BllExp.ExpData_KFY.WY_DJP3_Z[0][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJP3_Z[0][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJP3_Z[0][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_DJP3_Z[0][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_DJP3_Z[0][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJP3_Z[0][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJP3_Z[0][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_DJP3_Z[0][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJP3_Z[0][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJP3_Z[0][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                    }
                    if (StageNOInList == 7)
                    {
                        BllExp.ExpData_KFY.WY_DJP3_F[0][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJP3_F[0][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJP3_F[0][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_DJP3_F[0][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_DJP3_F[0][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJP3_F[0][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJP3_F[0][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_DJP3_F[0][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJP3_F[0][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJP3_F[0][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                    }
                    InitialValueSaved = true;
                }

                //开始前提醒
                if (BllExp.Exp_KFY.Stage_DQ.IsNeedTipsBefore && (!BllExp.Exp_KFY.Stage_DQ.IsTipsBeforeComplete))
                {
                    MessageBox.Show(BllExp.Exp_KFY.Stage_DQ.StringTipsBefore, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_KFY.Stage_DQ.IsTipsBeforeComplete = true;
                }

                //逐个步骤计算给定值
                int stepsCount = BllExp.Exp_KFY.Stage_DQ.StepList.Count;
                for (int i = 0; i < stepsCount; i++)
                {
                    //数据准备--------------------------------
                    StepNOInList = i;
                    //步骤压力稳定时间
                    tempStepPressKeepTime = BllDev.KeepingTime_KFY_AQ;
                    //步骤稳压目标值
                    tempType = 33;
                    tempAim = GetAimPress(tempType, StageNOInList, StepNOInList);
                    tempAim = Math.Abs(tempAim);
                    //获取偏差允许范围
                    tempPermitErr = GetErrBig(tempAim);
                    tempValueNow = Math.Abs(BllDev.AIList[14].ValueFinal);
                    tempGiven = tempValueNow;

                    if (!BllExp.Exp_KFY.Stage_DQ.StepList[i].IsStepCompleted)
                    {
                        tempStageComplete = false;
                        BllExp.Exp_KFY.Step_DQ = BllExp.Exp_KFY.Stage_DQ.StepList[i];

                        //步骤开始前等待
                        if (!BllExp.Exp_KFY.Step_DQ.IsWaitBeforCompleted)
                        {
                            if (!BllExp.Exp_KFY.Step_DQ.IsWaitStarted)
                            {
                                BllExp.Exp_KFY.Step_DQ.WaitStartTime = DateTime.Now;
                                BllExp.Exp_KFY.Step_DQ.IsWaitStarted = true;
                            }
                            TimeSpan tempSpanWait = DateTime.Now - BllExp.Exp_KFY.Step_DQ.WaitStartTime;
                            if (tempSpanWait.TotalSeconds >= BllExp.Exp_KFY.Step_DQ.TimeWaitBefor)
                            {
                                BllExp.Exp_KFY.Step_DQ.IsWaitBeforCompleted = true;
                                break;
                            }
                            //等待未完成或切换瞬间，保持PID输出
                            else
                            {
                                //tempGiven = tempValueNow;
                                tempGiven = 0;
                                break;
                            }
                        }

                        //压力加载
                        if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpCompleted)
                        {
                            if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpStarted)
                            {
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.UpStartTime = DateTime.Now;
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpStarted = true;
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.LoadUpStartValue = tempValueNow;
                            }
                            TimeSpan tempSpanUp = DateTime.Now - BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.UpStartTime;
                            tempGiven = Math.Abs(BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.LoadUpStartValue) +
                                        Math.Abs(BllDev.LoadUpDownSpeed) * tempSpanUp.TotalSeconds;
                            //判断压力给定值增加是否结束
                            if (tempGiven >= tempAim)
                            {
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpCompleted = true;
                                tempGiven = tempAim;
                            }
                            break;
                        }

                        //压力保持
                        if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressCompleted)
                        {
                            tempGiven = tempAim;
                            //判断是否进入压力保持范围
                            if ((tempValueNow >= (tempAim - tempPermitErr)) && (tempValueNow <= (tempAim + tempPermitErr)))
                            {
                                if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressStarted)
                                {
                                    BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressStarted = true;
                                    BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.KeepPressStartTime = DateTime.Now;
                                }
                                TimeSpan tempSpanKeep = DateTime.Now - BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.KeepPressStartTime;
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.PressKeeppingTimes = tempSpanKeep.TotalSeconds;
                                //判定是否完成压力保持
                                if (tempSpanKeep.TotalSeconds >= tempStepPressKeepTime)
                                {
                                    //采集检测数据
                                    if (StageNOInList == 6)
                                    {
                                        //位移记录
                                        BllExp.ExpData_KFY.WY_DJP3_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_DJP3_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_DJP3_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_DJP3_Z[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                                        BllExp.ExpData_KFY.WY_DJP3_Z[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_DJP3_Z[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_DJP3_Z[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_DJP3_Z[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_DJP3_Z[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_DJP3_Z[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                                        //相对挠度
                                        BllExp.ExpData_KFY.XDND_DJP3_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                                        BllExp.ExpData_KFY.XDND_DJP3_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                                        BllExp.ExpData_KFY.XDND_DJP3_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                                        //挠度
                                        BllExp.ExpData_KFY.ND_DJP3_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                                        BllExp.ExpData_KFY.ND_DJP3_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                                        BllExp.ExpData_KFY.ND_DJP3_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                                        //检测压力
                                        BllExp.ExpData_KFY.TestPress_DJP3_Z = GetStdPress(33, StageNOInList, StepNOInList);
                                        //挠度最大值
                                        BllExp.ExpData_KFY.XDND_Max_DJp3_Z = MaxOfObsAbs(BllExp.ExpData_KFY.XDND_DJP3_Z[1]);
                                        BllExp.ExpData_KFY.ND_Max_DJp3_Z = MaxOfObsAbs(BllExp.ExpData_KFY.ND_DJP3_Z[1]);
                                    }
                                    if (StageNOInList == 7)
                                    {
                                        //位移记录
                                        BllExp.ExpData_KFY.WY_DJP3_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_DJP3_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_DJP3_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_DJP3_F[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                                        BllExp.ExpData_KFY.WY_DJP3_F[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_DJP3_F[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_DJP3_F[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_DJP3_F[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_DJP3_F[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_DJP3_F[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                                        //相对挠度
                                        BllExp.ExpData_KFY.XDND_DJP3_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                                        BllExp.ExpData_KFY.XDND_DJP3_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                                        BllExp.ExpData_KFY.XDND_DJP3_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                                        //挠度
                                        BllExp.ExpData_KFY.ND_DJP3_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                                        BllExp.ExpData_KFY.ND_DJP3_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                                        BllExp.ExpData_KFY.ND_DJP3_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                                        //检测压力
                                        BllExp.ExpData_KFY.TestPress_DJP3_F = GetStdPress(33, StageNOInList, StepNOInList);
                                        //挠度最大值
                                        BllExp.ExpData_KFY.XDND_Max_DJp3_F = MaxOfObsAbs(BllExp.ExpData_KFY.XDND_DJP3_F[1]);
                                        BllExp.ExpData_KFY.ND_Max_DJp3_F = MaxOfObsAbs(BllExp.ExpData_KFY.ND_DJP3_F[1]);
                                    }
                                    BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressCompleted = true;
                                    BllExp.Exp_KFY.Step_DQ.IsStepCompleted = true;
                                }
                            }
                            //else
                            //{
                            //    BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressStarted = false;
                            //}
                            break;
                        }
                    }
                }
                if (tempGiven < 0)
                    tempGiven = 0;
                tempErr = tempGiven - tempValueNow;
                PID_ParamModel tempPIDParam = GePIDParam(2, Math.Abs(tempGiven));
                //严重渗漏时
                if (DemageOrderKFY)
                {
                    //采集检测数据
                    if (StageNOInList == 6)
                    {
                        //位移记录
                        BllExp.ExpData_KFY.WY_DJP3_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJP3_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJP3_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_DJP3_Z[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_DJP3_Z[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJP3_Z[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJP3_Z[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_DJP3_Z[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJP3_Z[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJP3_Z[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                        //相对挠度
                        BllExp.ExpData_KFY.XDND_DJP3_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                        BllExp.ExpData_KFY.XDND_DJP3_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                        BllExp.ExpData_KFY.XDND_DJP3_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                        //挠度
                        BllExp.ExpData_KFY.ND_DJP3_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                        BllExp.ExpData_KFY.ND_DJP3_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                        BllExp.ExpData_KFY.ND_DJP3_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                        //检测压力
                        BllExp.ExpData_KFY.TestPress_DJP3_Z = GetStdPress(33, StageNOInList, StepNOInList);
                        //挠度最大值
                        BllExp.ExpData_KFY.XDND_Max_DJp3_Z = MaxOfObsAbs(BllExp.ExpData_KFY.XDND_DJP3_Z[1]);
                        BllExp.ExpData_KFY.ND_Max_DJp3_Z = MaxOfObsAbs(BllExp.ExpData_KFY.ND_DJP3_Z[1]);
                    }
                    if (StageNOInList == 7)
                    {
                        //位移记录
                        BllExp.ExpData_KFY.WY_DJP3_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJP3_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJP3_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_DJP3_F[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_DJP3_F[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJP3_F[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJP3_F[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_DJP3_F[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJP3_F[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJP3_F[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                        //相对挠度
                        BllExp.ExpData_KFY.XDND_DJP3_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                        BllExp.ExpData_KFY.XDND_DJP3_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                        BllExp.ExpData_KFY.XDND_DJP3_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                        //挠度
                        BllExp.ExpData_KFY.ND_DJP3_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                        BllExp.ExpData_KFY.ND_DJP3_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                        BllExp.ExpData_KFY.ND_DJP3_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                        //检测压力
                        BllExp.ExpData_KFY.TestPress_DJP3_F = GetStdPress(33, StageNOInList, StepNOInList);
                        //挠度最大值
                        BllExp.ExpData_KFY.XDND_Max_DJp3_F = MaxOfObsAbs(BllExp.ExpData_KFY.XDND_DJP3_F[1]);
                        BllExp.ExpData_KFY.ND_Max_DJp3_F = MaxOfObsAbs(BllExp.ExpData_KFY.ND_DJP3_F[1]);
                    }
                    tempStageComplete = true;
                }

                //PID计算，分析状态。
                //如果所有步骤均完成，则阶段完成
                if (tempStageComplete)
                {
                    BllExp.Exp_KFY.Stage_DQ.CompleteStatus = true;
                    PID_KFY1.PID_Param.ControllerEnable = false;
                    PID_KFY1.CalculatePID(0);
                }
                else
                {
                    PID_KFY1.PID_Param = tempPIDParam;
                    if (Math.Abs(tempGiven) < 0.01)
                        PID_KFY1.PID_Param.ControllerEnable = false; //预设压力为0时屏蔽pid
                    else
                        PID_KFY1.PID_Param.ControllerEnable = true; //pid使能
                    PID_KFY1.CalculatePID(tempErr);

                }
                double tempUK_KFYp3 = PID_KFY1.UK;
                if (tempUK_KFYp3 <= 0)
                {
                    tempUK_KFYp3 = 0;
                }
                if (tempUK_KFYp3 >= 32000)
                {
                    tempUK_KFYp3 = 32000;
                }
                PLCCKCMDFrmBLL[3] = Convert.ToUInt16(tempUK_KFYp3);

                ////PID计算数据显示
                //string aimStr = tempGiven.ToString("0.00");
                //string ukStr = PID_KFY1.UK.ToString("0.00");
                //string ukpStr = PID_KFY1.UK_P.ToString("0.00");
                //string ukiStr = PID_KFY1.UK_I.ToString("0.00");
                //string ukdStr = PID_KFY1.UK_D.ToString("0.00");
                //Messenger.Default.Send<string>("Given:" + aimStr + "  ,U:" + ukStr + "  ,Up:" + ukpStr + "  ,Ui:" + ukiStr + "  ,Ud:" + ukdStr, "PIDinfoMessage");

                //判断是否完成
                if (BllExp.Exp_KFY.Stage_DQ.CompleteStatus)
                {
                    //分析是否所有阶段都完成
                    bool tempAllStageComplete = true;
                    for (int i = 0; i < BllExp.Exp_KFY.StageList_KFYDJ.Count; i++)
                    {
                        if (!BllExp.Exp_KFY.StageList_KFYDJ[i].CompleteStatus)
                            tempAllStageComplete = false;
                    }
                    if (tempAllStageComplete)
                        BllExp.Exp_KFY.CompleteStatus = true;
                    //保存进度和数据
                    switch (StageNOInList)
                    {
                        case 1:

                            break;
                    }

                    Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        Messenger.Default.Send<string>(BllExp.Exp_KFY.Stage_DQ.Stage_Name + "已完成！", "OpenPrompt");
                    }));                      //非预加压时，打开损坏确认窗口
                    OpenKFYDamageDJWin();
                    Thread.Sleep(2000);
                    BllExp.Exp_KFY.Stage_DQ = new MQZH_StageModel_QSM();
                    BllExp.Exp_KFY.Step_DQ = new MQZH_StepModel_QSM();
                    EscStopCompleteExpReset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 抗风压工程p3阶段 实施程序
        /// </summary>
        private void KFYp3GCStageFunc()
        {
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
                if ((BllExp.Exp_KFY.Stage_DQ.Stage_NO == "99") || (StageNOInList == -1))
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

                //更新初值
                if (!InitialValueSaved)
                {
                    BllExp.Exp_KFY.DisplaceGroups[0].SetWYCS();
                    BllExp.Exp_KFY.DisplaceGroups[1].SetWYCS();
                    BllExp.Exp_KFY.DisplaceGroups[2].SetWYCS();

                    if (StageNOInList == 6)
                    {
                        BllExp.ExpData_KFY.WY_GCP3_Z[0][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCP3_Z[0][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCP3_Z[0][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCP3_Z[0][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_GCP3_Z[0][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCP3_Z[0][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCP3_Z[0][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCP3_Z[0][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCP3_Z[0][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCP3_Z[0][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                    }
                    if (StageNOInList == 7)
                    {
                        BllExp.ExpData_KFY.WY_GCP3_F[0][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCP3_F[0][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCP3_F[0][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCP3_F[0][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_GCP3_F[0][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCP3_F[0][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCP3_F[0][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCP3_F[0][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCP3_F[0][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCP3_F[0][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                    }
                    InitialValueSaved = true;
                }

                //开始前提醒
                if (BllExp.Exp_KFY.Stage_DQ.IsNeedTipsBefore && (!BllExp.Exp_KFY.Stage_DQ.IsTipsBeforeComplete))
                {
                    MessageBox.Show(BllExp.Exp_KFY.Stage_DQ.StringTipsBefore, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_KFY.Stage_DQ.IsTipsBeforeComplete = true;
                }

                //逐个步骤计算给定值
                int stepsCount = BllExp.Exp_KFY.Stage_DQ.StepList.Count;
                for (int i = 0; i < stepsCount; i++)
                {
                    if (!BllExp.Exp_KFY.Stage_DQ.StepList[i].IsStepCompleted)
                    {
                        tempStageComplete = false;
                        BllExp.Exp_KFY.Step_DQ = BllExp.Exp_KFY.Stage_DQ.StepList[i];
                        StepNOInList = i;
                        //数据准备--------------------------------
                        //步骤压力稳定时间
                        tempStepPressKeepTime = BllDev.KeepingTime_KFY_AQ;
                        //步骤稳压目标值
                        tempType = 37;
                        tempAim = GetAimPress(tempType, StageNOInList, StepNOInList);
                        tempAim = Math.Abs(tempAim);
                        //获取偏差允许范围
                        tempPermitErr = GetErrBig(tempAim);
                        tempValueNow = Math.Abs(BllDev.AIList[14].ValueFinal);
                        tempGiven = tempValueNow;

                        //步骤开始前等待
                        if (!BllExp.Exp_KFY.Step_DQ.IsWaitBeforCompleted)
                        {
                            if (!BllExp.Exp_KFY.Step_DQ.IsWaitStarted)
                            {
                                BllExp.Exp_KFY.Step_DQ.WaitStartTime = DateTime.Now;
                                BllExp.Exp_KFY.Step_DQ.IsWaitStarted = true;
                            }
                            TimeSpan tempSpanWait = DateTime.Now - BllExp.Exp_KFY.Step_DQ.WaitStartTime;
                            if (tempSpanWait.TotalSeconds >= BllExp.Exp_KFY.Step_DQ.TimeWaitBefor)
                            {
                                BllExp.Exp_KFY.Step_DQ.IsWaitBeforCompleted = true;
                                break;
                            }
                            //等待未完成或切换瞬间，保持PID输出
                            else
                            {
                                //tempGiven = tempValueNow;
                                tempGiven = 0;
                                break;
                            }
                        }

                        //压力加载
                        if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpCompleted)
                        {
                            if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpStarted)
                            {
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.UpStartTime = DateTime.Now;
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpStarted = true;
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.LoadUpStartValue = tempValueNow;
                            }
                            TimeSpan tempSpanUp = DateTime.Now - BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.UpStartTime;
                            tempGiven = Math.Abs(BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.LoadUpStartValue) +
                                        Math.Abs(BllDev.LoadUpDownSpeed) * tempSpanUp.TotalSeconds;
                            //判断压力给定值增加是否结束
                            if (tempGiven >= tempAim)
                            {
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpCompleted = true;
                                tempGiven = tempAim;
                            }
                            break;
                        }

                        //压力保持
                        if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressCompleted)
                        {
                            tempGiven = tempAim;
                            //判断是否进入压力保持范围
                            if ((tempValueNow >= (tempAim - tempPermitErr)) && (tempValueNow <= (tempAim + tempPermitErr)))
                            {
                                if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressStarted)
                                {
                                    BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressStarted = true;
                                    BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.KeepPressStartTime = DateTime.Now;
                                }
                                TimeSpan tempSpanKeep = DateTime.Now - BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.KeepPressStartTime;
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.PressKeeppingTimes = tempSpanKeep.TotalSeconds;
                                //判定是否完成压力保持
                                if (tempSpanKeep.TotalSeconds >= tempStepPressKeepTime)
                                {
                                    //采集检测数据
                                    if (StageNOInList == 6)
                                    {
                                        //位移记录
                                        BllExp.ExpData_KFY.WY_GCP3_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_GCP3_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_GCP3_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_GCP3_Z[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                                        BllExp.ExpData_KFY.WY_GCP3_Z[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_GCP3_Z[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_GCP3_Z[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_GCP3_Z[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_GCP3_Z[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_GCP3_Z[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                                        //相对挠度
                                        BllExp.ExpData_KFY.XDND_GCP3_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                                        BllExp.ExpData_KFY.XDND_GCP3_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                                        BllExp.ExpData_KFY.XDND_GCP3_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                                        //挠度
                                        BllExp.ExpData_KFY.ND_GCP3_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                                        BllExp.ExpData_KFY.ND_GCP3_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                                        BllExp.ExpData_KFY.ND_GCP3_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                                        //检测压力
                                        BllExp.ExpData_KFY.TestPress_GCP3_Z = GetStdPress(37, StageNOInList, StepNOInList);
                                        //挠度最大值
                                        BllExp.ExpData_KFY.XDND_Max_GCp3_Z = MaxOfObsAbs(BllExp.ExpData_KFY.XDND_GCP3_Z[1]);
                                        BllExp.ExpData_KFY.ND_Max_GCp3_Z = MaxOfObsAbs(BllExp.ExpData_KFY.ND_GCP3_Z[1]);
                                        //挠度超限
                                        if (BllExp.Exp_KFY.DisplaceGroups[0].BigThanNDYX ||
                                            BllExp.Exp_KFY.DisplaceGroups[1].BigThanNDYX ||
                                            BllExp.Exp_KFY.DisplaceGroups[2].BigThanNDYX)
                                            BllExp.ExpData_KFY.IsXDND_Over_GCp3_Z = true;
                                    }
                                    if (StageNOInList == 7)
                                    {
                                        //位移记录
                                        BllExp.ExpData_KFY.WY_GCP3_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_GCP3_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_GCP3_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_GCP3_F[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                                        BllExp.ExpData_KFY.WY_GCP3_F[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_GCP3_F[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_GCP3_F[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_GCP3_F[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_GCP3_F[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_GCP3_F[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                                        //相对挠度
                                        BllExp.ExpData_KFY.XDND_GCP3_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                                        BllExp.ExpData_KFY.XDND_GCP3_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                                        BllExp.ExpData_KFY.XDND_GCP3_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                                        //挠度
                                        BllExp.ExpData_KFY.ND_GCP3_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                                        BllExp.ExpData_KFY.ND_GCP3_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                                        BllExp.ExpData_KFY.ND_GCP3_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                                        //检测压力
                                        BllExp.ExpData_KFY.TestPress_GCP3_F = GetStdPress(37, StageNOInList, StepNOInList);
                                        //挠度最大值
                                        BllExp.ExpData_KFY.XDND_Max_GCp3_F = MaxOfObsAbs(BllExp.ExpData_KFY.XDND_GCP3_F[1]);
                                        BllExp.ExpData_KFY.ND_Max_GCp3_F = MaxOfObsAbs(BllExp.ExpData_KFY.ND_GCP3_F[1]);
                                        //挠度超限
                                        if (BllExp.Exp_KFY.DisplaceGroups[0].BigThanNDYX ||
                                            BllExp.Exp_KFY.DisplaceGroups[1].BigThanNDYX ||
                                            BllExp.Exp_KFY.DisplaceGroups[2].BigThanNDYX)
                                            BllExp.ExpData_KFY.IsXDND_Over_GCp3_F = true;
                                    }
                                    BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressCompleted = true;
                                    BllExp.Exp_KFY.Step_DQ.IsStepCompleted = true;
                                }
                            }
                            //else
                            //{
                            //    BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressStarted = false;
                            //}
                            break;
                        }
                    }
                }
                if (tempGiven < 0)
                    tempGiven = 0;
                tempErr = tempGiven - tempValueNow;
                PID_ParamModel tempPIDParam = GePIDParam(2, Math.Abs(tempGiven));
                //严重渗漏时
                if (DemageOrderKFY)
                {
                    //采集检测数据
                    if (StageNOInList == 6)
                    {
                        //位移记录
                        BllExp.ExpData_KFY.WY_GCP3_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCP3_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCP3_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCP3_Z[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_GCP3_Z[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCP3_Z[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCP3_Z[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCP3_Z[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCP3_Z[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCP3_Z[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                        //相对挠度
                        BllExp.ExpData_KFY.XDND_GCP3_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                        BllExp.ExpData_KFY.XDND_GCP3_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                        BllExp.ExpData_KFY.XDND_GCP3_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                        //挠度
                        BllExp.ExpData_KFY.ND_GCP3_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                        BllExp.ExpData_KFY.ND_GCP3_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                        BllExp.ExpData_KFY.ND_GCP3_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                        //检测压力
                        BllExp.ExpData_KFY.TestPress_GCP3_Z = GetStdPress(37, StageNOInList, StepNOInList);
                        //挠度最大值
                        BllExp.ExpData_KFY.XDND_Max_GCp3_Z = MaxOfObsAbs(BllExp.ExpData_KFY.XDND_GCP3_Z[1]);
                        BllExp.ExpData_KFY.ND_Max_GCp3_Z = MaxOfObsAbs(BllExp.ExpData_KFY.ND_GCP3_Z[1]);
                        //挠度超限
                        if (BllExp.Exp_KFY.DisplaceGroups[0].BigThanNDYX ||
                            BllExp.Exp_KFY.DisplaceGroups[1].BigThanNDYX ||
                            BllExp.Exp_KFY.DisplaceGroups[2].BigThanNDYX)
                            BllExp.ExpData_KFY.IsXDND_Over_GCp3_Z = true;
                    }
                    if (StageNOInList == 7)
                    {
                        //位移记录
                        BllExp.ExpData_KFY.WY_GCP3_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCP3_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCP3_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCP3_F[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_GCP3_F[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCP3_F[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCP3_F[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCP3_F[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCP3_F[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCP3_F[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                        //相对挠度
                        BllExp.ExpData_KFY.XDND_GCP3_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                        BllExp.ExpData_KFY.XDND_GCP3_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                        BllExp.ExpData_KFY.XDND_GCP3_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                        //挠度
                        BllExp.ExpData_KFY.ND_GCP3_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                        BllExp.ExpData_KFY.ND_GCP3_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                        BllExp.ExpData_KFY.ND_GCP3_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                        //检测压力
                        BllExp.ExpData_KFY.TestPress_GCP3_F = GetStdPress(37, StageNOInList, StepNOInList);
                        //挠度最大值
                        BllExp.ExpData_KFY.XDND_Max_GCp3_F = MaxOfObsAbs(BllExp.ExpData_KFY.XDND_GCP3_F[1]);
                        BllExp.ExpData_KFY.ND_Max_GCp3_F = MaxOfObsAbs(BllExp.ExpData_KFY.ND_GCP3_F[1]);
                        //挠度超限
                        if (BllExp.Exp_KFY.DisplaceGroups[0].BigThanNDYX ||
                            BllExp.Exp_KFY.DisplaceGroups[1].BigThanNDYX ||
                            BllExp.Exp_KFY.DisplaceGroups[2].BigThanNDYX)
                            BllExp.ExpData_KFY.IsXDND_Over_GCp3_F = true;
                    }
                    tempStageComplete = true;
                }

                //PID计算，分析状态。
                //如果所有步骤均完成，则阶段完成
                if (tempStageComplete)
                {
                    BllExp.Exp_KFY.Stage_DQ.CompleteStatus = true;
                    PID_KFY1.PID_Param.ControllerEnable = false;
                    PID_KFY1.CalculatePID(0);
                }
                else
                {
                    PID_KFY1.PID_Param = tempPIDParam;
                    if (Math.Abs(tempGiven) < 0.01)
                        PID_KFY1.PID_Param.ControllerEnable = false; //预设压力为0时屏蔽pid
                    else
                        PID_KFY1.PID_Param.ControllerEnable = true; //pid使能
                    PID_KFY1.CalculatePID(tempErr);
                }
                double tempUK_KFYp3 = PID_KFY1.UK;
                if (tempUK_KFYp3 <= 0)
                {
                    tempUK_KFYp3 = 0;
                }
                if (tempUK_KFYp3 >= 32000)
                {
                    tempUK_KFYp3 = 32000;
                }
                PLCCKCMDFrmBLL[3] = Convert.ToUInt16(tempUK_KFYp3);

                ////PID计算数据显示
                //string aimStr = tempGiven.ToString("0.00");
                //string ukStr = PID_KFY1.UK.ToString("0.00");
                //string ukpStr = PID_KFY1.UK_P.ToString("0.00");
                //string ukiStr = PID_KFY1.UK_I.ToString("0.00");
                //string ukdStr = PID_KFY1.UK_D.ToString("0.00");
                //Messenger.Default.Send<string>("Given:" + aimStr + "  ,U:" + ukStr + "  ,Up:" + ukpStr + "  ,Ui:" + ukiStr + "  ,Ud:" + ukdStr, "PIDinfoMessage");

                //判断是否完成
                if (BllExp.Exp_KFY.Stage_DQ.CompleteStatus)
                {
                    //分析是否所有阶段都完成
                    bool tempAllStageComplete = true;
                    for (int i = 0; i < BllExp.Exp_KFY.StageList_KFYGC.Count; i++)
                    {
                        if (!BllExp.Exp_KFY.StageList_KFYGC[i].CompleteStatus)
                            tempAllStageComplete = false;
                    }
                    if (tempAllStageComplete)
                        BllExp.Exp_KFY.CompleteStatus = true;
                    //保存进度和数据
                    Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        Messenger.Default.Send<string>(BllExp.Exp_KFY.Stage_DQ.Stage_Name + "已完成！", "OpenPrompt");
                    }));
                    //非预加压时，打开损坏确认窗口
                    OpenKFYDamageGCWin();
                    Thread.Sleep(2000);
                    BllExp.Exp_KFY.Stage_DQ = new MQZH_StageModel_QSM();
                    BllExp.Exp_KFY.Step_DQ = new MQZH_StepModel_QSM();
                    EscStopCompleteExpReset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


        #region 抗风压设计值阶段试验实施程序

        /// <summary>
        /// 抗风压定级pmax阶段 实施程序
        /// </summary>
        private void KFYpmaxDJStageFunc()
        {
            double tempValueNow = 0;        //当前值
            double tempGiven = 0;           //给定值
            double tempErr;                 //PID计算用误差
            double tempPermitErr;           //偏差允许范围
            double tempAim;                //阶段控制目标
            bool tempStageComplete = true;  //阶段完成
            double tempStepPressKeepTime;   //步骤保持时间
            int tempType;                   //阶段类型

            try
            {
                BllDev.IsDeviceBusy = true;
                //如果当前阶段是默认阶段，则复位状态
                if ((BllExp.Exp_KFY.Stage_DQ.Stage_NO == "99") || (StageNOInList == -1))
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

                //更新初值
                if (!InitialValueSaved)
                {
                    BllExp.Exp_KFY.DisplaceGroups[0].SetWYCS();
                    BllExp.Exp_KFY.DisplaceGroups[1].SetWYCS();
                    BllExp.Exp_KFY.DisplaceGroups[2].SetWYCS();

                    if (StageNOInList == 6)
                    {
                        BllExp.ExpData_KFY.WY_DJPmax_Z[0][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJPmax_Z[0][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJPmax_Z[0][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_DJPmax_Z[0][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_DJPmax_Z[0][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJPmax_Z[0][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJPmax_Z[0][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_DJPmax_Z[0][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJPmax_Z[0][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJPmax_Z[0][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                    }
                    if (StageNOInList == 7)
                    {
                        BllExp.ExpData_KFY.WY_DJPmax_F[0][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJPmax_F[0][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJPmax_F[0][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_DJPmax_F[0][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_DJPmax_F[0][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJPmax_F[0][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJPmax_F[0][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_DJPmax_F[0][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_DJPmax_F[0][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_DJPmax_F[0][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                    }
                    InitialValueSaved = true;
                }

                //开始前提醒
                if (BllExp.Exp_KFY.Stage_DQ.IsNeedTipsBefore && (!BllExp.Exp_KFY.Stage_DQ.IsTipsBeforeComplete))
                {
                    MessageBox.Show(BllExp.Exp_KFY.Stage_DQ.StringTipsBefore, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_KFY.Stage_DQ.IsTipsBeforeComplete = true;
                }

                //逐个步骤计算给定值
                int stepsCount = BllExp.Exp_KFY.Stage_DQ.StepList.Count;
                for (int i = 0; i < stepsCount; i++)
                {
                    //数据准备--------------------------------
                    StepNOInList = i;
                    //步骤压力稳定时间
                    tempStepPressKeepTime = BllDev.KeepingTime_KFY_AQ;
                    //步骤稳压目标值
                    tempType = 34;
                    tempAim = GetAimPress(tempType, StageNOInList, StepNOInList);
                    tempAim = Math.Abs(tempAim);
                    //获取偏差允许范围
                    tempPermitErr = GetErrBig(tempAim);
                    tempValueNow = Math.Abs(BllDev.AIList[14].ValueFinal);
                    tempGiven = tempValueNow;

                    if (!BllExp.Exp_KFY.Stage_DQ.StepList[i].IsStepCompleted)
                    {
                        tempStageComplete = false;
                        BllExp.Exp_KFY.Step_DQ = BllExp.Exp_KFY.Stage_DQ.StepList[i];

                        //步骤开始前等待
                        if (!BllExp.Exp_KFY.Step_DQ.IsWaitBeforCompleted)
                        {
                            if (!BllExp.Exp_KFY.Step_DQ.IsWaitStarted)
                            {
                                BllExp.Exp_KFY.Step_DQ.WaitStartTime = DateTime.Now;
                                BllExp.Exp_KFY.Step_DQ.IsWaitStarted = true;
                            }
                            TimeSpan tempSpanWait = DateTime.Now - BllExp.Exp_KFY.Step_DQ.WaitStartTime;
                            if (tempSpanWait.TotalSeconds >= BllExp.Exp_KFY.Step_DQ.TimeWaitBefor)
                            {
                                BllExp.Exp_KFY.Step_DQ.IsWaitBeforCompleted = true;
                                break;
                            }
                            //等待未完成或切换瞬间，保持PID输出
                            else
                            {
                                //tempGiven = tempValueNow;
                                tempGiven = 0;
                                break;
                            }
                        }

                        //压力加载
                        if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpCompleted)
                        {
                            if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpStarted)
                            {
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.UpStartTime = DateTime.Now;
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpStarted = true;
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.LoadUpStartValue = tempValueNow;
                            }
                            TimeSpan tempSpanUp = DateTime.Now - BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.UpStartTime;
                            tempGiven = Math.Abs(BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.LoadUpStartValue) +
                                        Math.Abs(BllDev.LoadUpDownSpeed) * tempSpanUp.TotalSeconds;
                            //判断压力给定值增加是否结束
                            if (tempGiven >= tempAim)
                            {
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpCompleted = true;
                                tempGiven = tempAim;
                            }
                            break;
                        }

                        //压力保持
                        if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressCompleted)
                        {
                            tempGiven = tempAim;
                            //判断是否进入压力保持范围
                            if ((tempValueNow >= (tempAim - tempPermitErr)) && (tempValueNow <= (tempAim + tempPermitErr)))
                            {
                                if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressStarted)
                                {
                                    BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressStarted = true;
                                    BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.KeepPressStartTime = DateTime.Now;
                                }
                                TimeSpan tempSpanKeep = DateTime.Now - BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.KeepPressStartTime;
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.PressKeeppingTimes = tempSpanKeep.TotalSeconds;
                                //判定是否完成压力保持
                                if (tempSpanKeep.TotalSeconds >= tempStepPressKeepTime)
                                {
                                    //采集检测数据
                                    if (StageNOInList == 8)
                                    {
                                        //位移记录
                                        BllExp.ExpData_KFY.WY_DJPmax_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_DJPmax_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_DJPmax_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_DJPmax_Z[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                                        BllExp.ExpData_KFY.WY_DJPmax_Z[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_DJPmax_Z[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_DJPmax_Z[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_DJPmax_Z[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_DJPmax_Z[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_DJPmax_Z[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                                        //相对挠度
                                        BllExp.ExpData_KFY.XDND_DJPmax_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                                        BllExp.ExpData_KFY.XDND_DJPmax_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                                        BllExp.ExpData_KFY.XDND_DJPmax_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                                        //挠度
                                        BllExp.ExpData_KFY.ND_DJPmax_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                                        BllExp.ExpData_KFY.ND_DJPmax_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                                        BllExp.ExpData_KFY.ND_DJPmax_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                                        //检测压力
                                        BllExp.ExpData_KFY.TestPress_DJPmax_Z = GetStdPress(34, StageNOInList, StepNOInList);
                                    }
                                    if (StageNOInList == 9)
                                    {
                                        //位移记录
                                        BllExp.ExpData_KFY.WY_DJPmax_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_DJPmax_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_DJPmax_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_DJPmax_F[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                                        BllExp.ExpData_KFY.WY_DJPmax_F[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_DJPmax_F[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_DJPmax_F[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_DJPmax_F[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_DJPmax_F[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_DJPmax_F[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                                        //相对挠度
                                        BllExp.ExpData_KFY.XDND_DJPmax_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                                        BllExp.ExpData_KFY.XDND_DJPmax_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                                        BllExp.ExpData_KFY.XDND_DJPmax_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                                        //挠度
                                        BllExp.ExpData_KFY.ND_DJPmax_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                                        BllExp.ExpData_KFY.ND_DJPmax_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                                        BllExp.ExpData_KFY.ND_DJPmax_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                                        //检测压力
                                        BllExp.ExpData_KFY.TestPress_DJPmax_F = GetStdPress(34, StageNOInList, StepNOInList);
                                    }
                                    BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressCompleted = true;
                                    BllExp.Exp_KFY.Step_DQ.IsStepCompleted = true;
                                }
                            }
                            break;
                        }
                    }
                }
                if (tempGiven < 0)
                    tempGiven = 0;
                tempErr = tempGiven - tempValueNow;
                PID_ParamModel tempPIDParam = GePIDParam(2, Math.Abs(tempGiven));
                //严重渗漏时
                if (DemageOrderKFY)
                    tempStageComplete = true;
                //PID计算，分析状态。
                //如果所有步骤均完成，则阶段完成
                if (tempStageComplete)
                {
                    BllExp.Exp_KFY.Stage_DQ.CompleteStatus = true;
                    PID_KFY1.PID_Param.ControllerEnable = false;
                    PID_KFY1.CalculatePID(0);
                }
                else
                {
                    PID_KFY1.PID_Param = tempPIDParam;
                    if (Math.Abs(tempGiven) < 0.01)
                        PID_KFY1.PID_Param.ControllerEnable = false; //预设压力为0时屏蔽pid
                    else
                        PID_KFY1.PID_Param.ControllerEnable = true; //pid使能
                    PID_KFY1.CalculatePID(tempErr);
                }
                double tempUK_KFYpmax = PID_KFY1.UK;
                if (tempUK_KFYpmax <= 0)
                {
                    tempUK_KFYpmax = 0;
                }
                if (tempUK_KFYpmax >= 32000)
                {
                    tempUK_KFYpmax = 32000;
                }
                PLCCKCMDFrmBLL[3] = Convert.ToUInt16(tempUK_KFYpmax);

                //PID计算数据显示
                //string aimStr = tempGiven.ToString("0.00");
                //string ukStr = PID_KFY1.UK.ToString("0.00");
                //string ukpStr = PID_KFY1.UK_P.ToString("0.00");
                //string ukiStr = PID_KFY1.UK_I.ToString("0.00");
                //string ukdStr = PID_KFY1.UK_D.ToString("0.00");
                //Messenger.Default.Send<string>("Given:" + aimStr + "  ,U:" + ukStr + "  ,Up:" + ukpStr + "  ,Ui:" + ukiStr + "  ,Ud:" + ukdStr, "PIDinfoMessage");

                //判断是否完成
                if (BllExp.Exp_KFY.Stage_DQ.CompleteStatus)
                {
                    //分析是否所有阶段都完成
                    bool tempAllStageComplete = true;
                    for (int i = 0; i < BllExp.Exp_KFY.StageList_KFYDJ.Count; i++)
                    {
                        if (!BllExp.Exp_KFY.StageList_KFYDJ[i].CompleteStatus)
                            tempAllStageComplete = false;
                    }
                    if (tempAllStageComplete)
                        BllExp.Exp_KFY.CompleteStatus = true;
                    //保存进度和数据
                    Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        Messenger.Default.Send<string>(BllExp.Exp_KFY.Stage_DQ.Stage_Name + "已完成！", "OpenPrompt");
                    }));
                    //非预加压时，打开损坏确认窗口
                    OpenKFYDamageDJWin();
                    Thread.Sleep(2000);
                    BllExp.Exp_KFY.Stage_DQ = new MQZH_StageModel_QSM();
                    BllExp.Exp_KFY.Step_DQ = new MQZH_StepModel_QSM();
                    EscStopCompleteExpReset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 抗风压工程pmax阶段 实施程序
        /// </summary>
        private void KFYpmaxGCStageFunc()
        {
            double tempValueNow = 0;        //当前值
            double tempGiven = 0;           //给定值
            double tempErr;                //PID计算用误差
            double tempPermitErr;           //偏差允许范围
            double tempAim;                 //阶段控制目标
            bool tempStageComplete = true;  //阶段完成
            double tempStepPressKeepTime;  //步骤保持时间
            int tempType;                   //阶段类型

            try
            {
                BllDev.IsDeviceBusy = true;
                //如果当前阶段是默认阶段，则复位状态
                if ((BllExp.Exp_KFY.Stage_DQ.Stage_NO == "99") || (StageNOInList == -1))
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

                //更新初值
                if (!InitialValueSaved)
                {
                    BllExp.Exp_KFY.DisplaceGroups[0].SetWYCS();
                    BllExp.Exp_KFY.DisplaceGroups[1].SetWYCS();
                    BllExp.Exp_KFY.DisplaceGroups[2].SetWYCS();

                    if (StageNOInList == 8)
                    {
                        BllExp.ExpData_KFY.WY_GCPmax_Z[0][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[0][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[0][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[0][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[0][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[0][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[0][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[0][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[0][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[0][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                    }
                    if (StageNOInList == 9)
                    {
                        BllExp.ExpData_KFY.WY_GCPmax_Z[0][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[0][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[0][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[0][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[0][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[0][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[0][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[0][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[0][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[0][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                    }
                    InitialValueSaved = true;
                }

                //开始前提醒
                if (BllExp.Exp_KFY.Stage_DQ.IsNeedTipsBefore && (!BllExp.Exp_KFY.Stage_DQ.IsTipsBeforeComplete))
                {
                    MessageBox.Show(BllExp.Exp_KFY.Stage_DQ.StringTipsBefore, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_KFY.Stage_DQ.IsTipsBeforeComplete = true;
                }

                //逐个步骤计算给定值
                int stepsCount = BllExp.Exp_KFY.Stage_DQ.StepList.Count;
                for (int i = 0; i < stepsCount; i++)
                {
                    //数据准备--------------------------------
                    StepNOInList = i;
                    //步骤压力稳定时间
                    tempStepPressKeepTime = BllDev.KeepingTime_KFY_AQ;

                    //步骤稳压目标值
                    tempType = 38;
                    tempAim = GetAimPress(tempType, StageNOInList, StepNOInList);
                    tempAim = Math.Abs(tempAim);
                    //获取偏差允许范围
                    tempPermitErr = GetErrBig(tempAim);
                    tempValueNow = Math.Abs(BllDev.AIList[14].ValueFinal);
                    tempGiven = tempValueNow;

                    if (!BllExp.Exp_KFY.Stage_DQ.StepList[i].IsStepCompleted)
                    {
                        tempStageComplete = false;
                        BllExp.Exp_KFY.Step_DQ = BllExp.Exp_KFY.Stage_DQ.StepList[i];

                        //步骤开始前等待
                        if (!BllExp.Exp_KFY.Step_DQ.IsWaitBeforCompleted)
                        {
                            if (!BllExp.Exp_KFY.Step_DQ.IsWaitStarted)
                            {
                                BllExp.Exp_KFY.Step_DQ.WaitStartTime = DateTime.Now;
                                BllExp.Exp_KFY.Step_DQ.IsWaitStarted = true;
                            }
                            TimeSpan tempSpanWait = DateTime.Now - BllExp.Exp_KFY.Step_DQ.WaitStartTime;
                            if (tempSpanWait.TotalSeconds >= BllExp.Exp_KFY.Step_DQ.TimeWaitBefor)
                            {
                                BllExp.Exp_KFY.Step_DQ.IsWaitBeforCompleted = true;
                                break;
                            }
                            //等待未完成或切换瞬间，保持PID输出
                            else
                            {
                                //tempGiven = tempValueNow;
                                tempGiven = 0;
                                break;
                            }
                        }

                        //压力加载
                        if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpCompleted)
                        {
                            if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpStarted)
                            {
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.UpStartTime = DateTime.Now;
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpStarted = true;
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.LoadUpStartValue = tempValueNow;
                            }
                            TimeSpan tempSpanUp = DateTime.Now - BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.UpStartTime;
                            tempGiven = Math.Abs(BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.LoadUpStartValue) +
                                        Math.Abs(BllDev.LoadUpDownSpeed) * tempSpanUp.TotalSeconds;
                            //判断压力给定值增加是否结束
                            if (tempGiven >= tempAim)
                            {
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsUpCompleted = true;
                                tempGiven = tempAim;
                            }
                            break;
                        }

                        //压力保持
                        if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressCompleted)
                        {
                            tempGiven = tempAim;
                            //判断是否进入压力保持范围
                            if ((tempValueNow >= (tempAim - tempPermitErr)) && (tempValueNow <= (tempAim + tempPermitErr)))
                            {
                                if (!BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressStarted)
                                {
                                    BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressStarted = true;
                                    BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.KeepPressStartTime = DateTime.Now;
                                }
                                TimeSpan tempSpanKeep = DateTime.Now - BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.KeepPressStartTime;
                                BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.PressKeeppingTimes = tempSpanKeep.TotalSeconds;
                                //判定是否完成压力保持
                                if (tempSpanKeep.TotalSeconds >= tempStepPressKeepTime)
                                {
                                    //采集检测数据
                                    if (StageNOInList == 8)
                                    {
                                        //位移记录
                                        BllExp.ExpData_KFY.WY_GCPmax_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_GCPmax_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_GCPmax_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_GCPmax_Z[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                                        BllExp.ExpData_KFY.WY_GCPmax_Z[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_GCPmax_Z[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_GCPmax_Z[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_GCPmax_Z[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_GCPmax_Z[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_GCPmax_Z[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                                        //相对挠度
                                        BllExp.ExpData_KFY.XDND_GCPmax_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                                        BllExp.ExpData_KFY.XDND_GCPmax_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                                        BllExp.ExpData_KFY.XDND_GCPmax_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                                        //挠度
                                        BllExp.ExpData_KFY.ND_GCPmax_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                                        BllExp.ExpData_KFY.ND_GCPmax_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                                        BllExp.ExpData_KFY.ND_GCPmax_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                                        //检测压力
                                        BllExp.ExpData_KFY.TestPress_GCPmax_Z = GetStdPress(38, StageNOInList, StepNOInList);
                                        //相对挠度最大值
                                        BllExp.ExpData_KFY.XDND_Max_GCpmax_Z = MaxOfObsAbs(BllExp.ExpData_KFY.XDND_GCPmax_Z[1]);
                                        BllExp.ExpData_KFY.ND_Max_GCpmax_Z = MaxOfObsAbs(BllExp.ExpData_KFY.ND_GCPmax_Z[1]);
                                    }
                                    if (StageNOInList == 9)
                                    {
                                        //位移记录
                                        BllExp.ExpData_KFY.WY_GCPmax_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_GCPmax_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_GCPmax_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_GCPmax_F[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                                        BllExp.ExpData_KFY.WY_GCPmax_F[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_GCPmax_F[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_GCPmax_F[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                                        BllExp.ExpData_KFY.WY_GCPmax_F[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                                        BllExp.ExpData_KFY.WY_GCPmax_F[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                                        BllExp.ExpData_KFY.WY_GCPmax_F[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                                        //相对挠度
                                        BllExp.ExpData_KFY.XDND_GCPmax_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                                        BllExp.ExpData_KFY.XDND_GCPmax_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                                        BllExp.ExpData_KFY.XDND_GCPmax_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                                        //挠度
                                        BllExp.ExpData_KFY.ND_GCPmax_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                                        BllExp.ExpData_KFY.ND_GCPmax_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                                        BllExp.ExpData_KFY.ND_GCPmax_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                                        //检测压力
                                        BllExp.ExpData_KFY.TestPress_GCPmax_F = GetStdPress(38, StageNOInList, StepNOInList);
                                        //相对挠度最大值
                                        BllExp.ExpData_KFY.XDND_Max_GCpmax_F = MaxOfObsAbs(BllExp.ExpData_KFY.XDND_GCPmax_F[1]);
                                        BllExp.ExpData_KFY.ND_Max_GCpmax_F = MaxOfObsAbs(BllExp.ExpData_KFY.ND_GCPmax_F[1]);
                                    }
                                    BllExp.Exp_KFY.Step_DQ.StepSteadyPStatus.IsKeepPressCompleted = true;
                                    BllExp.Exp_KFY.Step_DQ.IsStepCompleted = true;
                                }
                            }
                            break;
                        }
                    }
                }
                if (tempGiven < 0)
                    tempGiven = 0;
                tempErr = tempGiven - tempValueNow;
                PID_ParamModel tempPIDParam = GePIDParam(2, Math.Abs(tempGiven));
                //严重渗漏时
                if (DemageOrderKFY)
                {
                    //采集检测数据
                    if (StageNOInList == 8)
                    {
                        //位移记录
                        BllExp.ExpData_KFY.WY_GCPmax_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCPmax_Z[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                        //相对挠度
                        BllExp.ExpData_KFY.XDND_GCPmax_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                        BllExp.ExpData_KFY.XDND_GCPmax_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                        BllExp.ExpData_KFY.XDND_GCPmax_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                        //挠度
                        BllExp.ExpData_KFY.ND_GCPmax_Z[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                        BllExp.ExpData_KFY.ND_GCPmax_Z[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                        BllExp.ExpData_KFY.ND_GCPmax_Z[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                        //检测压力
                        BllExp.ExpData_KFY.TestPress_GCPmax_Z = GetStdPress(38, StageNOInList, StepNOInList);
                        //相对挠度最大值
                        BllExp.ExpData_KFY.XDND_Max_GCpmax_Z = MaxOfObsAbs(BllExp.ExpData_KFY.XDND_GCPmax_Z[1]);
                        BllExp.ExpData_KFY.ND_Max_GCpmax_Z = MaxOfObsAbs(BllExp.ExpData_KFY.ND_GCPmax_Z[1]);
                    }
                    if (StageNOInList == 9)
                    {
                        //位移记录
                        BllExp.ExpData_KFY.WY_GCPmax_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCPmax_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCPmax_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCPmax_F[StepNOInList + 1][3] = BllExp.Exp_KFY.DisplaceGroups[0].WY_DQ[3];
                        BllExp.ExpData_KFY.WY_GCPmax_F[StepNOInList + 1][4] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCPmax_F[StepNOInList + 1][5] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCPmax_F[StepNOInList + 1][6] = BllExp.Exp_KFY.DisplaceGroups[1].WY_DQ[2];
                        BllExp.ExpData_KFY.WY_GCPmax_F[StepNOInList + 1][7] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[0];
                        BllExp.ExpData_KFY.WY_GCPmax_F[StepNOInList + 1][8] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[1];
                        BllExp.ExpData_KFY.WY_GCPmax_F[StepNOInList + 1][9] = BllExp.Exp_KFY.DisplaceGroups[2].WY_DQ[2];
                        //相对挠度
                        BllExp.ExpData_KFY.XDND_GCPmax_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND_XD;
                        BllExp.ExpData_KFY.XDND_GCPmax_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND_XD;
                        BllExp.ExpData_KFY.XDND_GCPmax_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND_XD;
                        //挠度
                        BllExp.ExpData_KFY.ND_GCPmax_F[StepNOInList + 1][0] = BllExp.Exp_KFY.DisplaceGroups[0].ND;
                        BllExp.ExpData_KFY.ND_GCPmax_F[StepNOInList + 1][1] = BllExp.Exp_KFY.DisplaceGroups[1].ND;
                        BllExp.ExpData_KFY.ND_GCPmax_F[StepNOInList + 1][2] = BllExp.Exp_KFY.DisplaceGroups[2].ND;
                        //检测压力
                        BllExp.ExpData_KFY.TestPress_GCPmax_F = GetStdPress(38, StageNOInList, StepNOInList);
                        //相对挠度最大值
                        BllExp.ExpData_KFY.XDND_Max_GCpmax_F = MaxOfObsAbs(BllExp.ExpData_KFY.XDND_GCPmax_F[1]);
                        BllExp.ExpData_KFY.ND_Max_GCpmax_F = MaxOfObsAbs(BllExp.ExpData_KFY.ND_GCPmax_F[1]);
                    }
                    tempStageComplete = true;
                }

                //PID计算，分析状态。
                //如果所有步骤均完成，则阶段完成
                if (tempStageComplete)
                {
                    BllExp.Exp_KFY.Stage_DQ.CompleteStatus = true;
                    PID_KFY1.PID_Param.ControllerEnable = false;
                    PID_KFY1.CalculatePID(0);
                }
                else
                {
                    PID_KFY1.PID_Param = tempPIDParam;
                    if (Math.Abs(tempGiven) < 0.01)
                        PID_KFY1.PID_Param.ControllerEnable = false; //预设压力为0时屏蔽pid
                    else
                        PID_KFY1.PID_Param.ControllerEnable = true; //pid使能
                    PID_KFY1.CalculatePID(tempErr);
                }
                //控制输出
                double tempUK_KFYpmax = PID_KFY1.UK;
                if (tempUK_KFYpmax <= 0)
                {
                    tempUK_KFYpmax = 0;
                }
                if (tempUK_KFYpmax >= 32000)
                {
                    tempUK_KFYpmax = 32000;
                }
                PLCCKCMDFrmBLL[3] = Convert.ToUInt16(tempUK_KFYpmax);

                //PID计算数据显示
                //string aimStr = tempGiven.ToString("0.00");
                //string ukStr = PID_KFY1.UK.ToString("0.00");
                //string ukpStr = PID_KFY1.UK_P.ToString("0.00");
                //string ukiStr = PID_KFY1.UK_I.ToString("0.00");
                //string ukdStr = PID_KFY1.UK_D.ToString("0.00");
                //Messenger.Default.Send<string>("Given:" + aimStr + "  ,U:" + ukStr + "  ,Up:" + ukpStr + "  ,Ui:" + ukiStr + "  ,Ud:" + ukdStr, "PIDinfoMessage");
                //判断是否完成
                if (BllExp.Exp_KFY.Stage_DQ.CompleteStatus)
                {
                    //分析是否所有阶段都完成
                    bool tempAllStageComplete = true;
                    for (int i = 0; i < BllExp.Exp_KFY.StageList_KFYGC.Count; i++)
                    {
                        if (!BllExp.Exp_KFY.StageList_KFYGC[i].CompleteStatus)
                            tempAllStageComplete = false;
                    }
                    if (tempAllStageComplete)
                        BllExp.Exp_KFY.CompleteStatus = true;
                    //保存进度和数据
                    Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        Messenger.Default.Send<string>(BllExp.Exp_KFY.Stage_DQ.Stage_Name + "已完成！", "OpenPrompt");
                    }));
                    //非预加压时，打开损坏确认窗口
                    OpenKFYDamageGCWin();
                    Thread.Sleep(2000);
                    BllExp.Exp_KFY.Stage_DQ = new MQZH_StageModel_QSM();
                    BllExp.Exp_KFY.Step_DQ = new MQZH_StepModel_QSM();
                    EscStopCompleteExpReset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


        #region 抗风压控制消息

        /// <summary>
        /// 抗风压p1试验控制消息
        /// </summary>
        /// <param name="msg"></param>
        private void KFYp1CtrlMessage(int msg)
        {
            try
            {
                //抗风压损坏停机
                if ((msg == 1403) || (msg == 1413))
                {
                    if (ExistKFYp1DJExp || ExistKFYp1GCExp)
                    {
                        MessageBoxResult tempRet = MessageBox.Show("试件故障、损坏是可提前结束实验。\r\n是否结束实验？", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                        if (tempRet == MessageBoxResult.Yes)
                        {
                            DemageOrderKFY = true;
                        }
                    }
                }

                //退出试验窗口
                else if (msg == 7206)
                {
                    if (ExistKFYp1DJExp || ExistKFYp1GCExp)
                    {
                        MessageBoxResult msgBoxResult = MessageBox.Show("是否停止正在进行的试验？", "提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                        if (msgBoxResult == MessageBoxResult.Yes)
                        {
                            Messenger.Default.Send<int>(1203, "StopExpMessage");
                            Messenger.Default.Send<string>(MQZH_WinName.KFYp1Name, "CloseGivenNameWin");
                        }
                    }
                    else
                        Messenger.Default.Send<string>(MQZH_WinName.KFYp1Name, "CloseGivenNameWin");
                }

                //抗风压p1定级检测试验开始消息分析。
                else if (msg == 1103)
                {
                    bool tempExist = false;
                    for (int i = 0; i < 4; i++)
                    {
                        //检测加压
                        if (BllExp.Exp_KFY.BeenCheckedDJ[i])
                        {
                            if (!BllExp.Exp_KFY.StageList_KFYDJ[i].NeedTest)
                            {
                                MessageBox.Show(BllExp.Exp_KFY.StageList_KFYDJ[i].Stage_Name + "无需检测！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                                BllExp.Exp_KFY.BeenCheckedDJ[i] = false;
                                return;
                            }
                            if (BllExp.Exp_KFY.StageList_KFYDJ[i].CompleteStatus)
                            {
                                MessageBoxResult msgBoxResult = MessageBox.Show(BllExp.Exp_KFY.StageList_KFYDJ[i].Stage_Name + "，将覆盖已完成的数据，是否覆盖？", "数据覆盖提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                                if (msgBoxResult == MessageBoxResult.Yes)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            BllExp.Exp_KFY.KFY_DJBX_ZYStageInit();
                                            StageNOInList = 0;
                                            tempExist = true;
                                            break;
                                        case 1:
                                            BllExp.Exp_KFY.KFY_DJBX_ZJStageInit();
                                            StageNOInList = 1;
                                            tempExist = true;
                                            break;
                                        case 2:
                                            BllExp.Exp_KFY.KFY_DJBX_FYStageInit();
                                            StageNOInList = 2;
                                            tempExist = true;
                                            break;
                                        case 3:
                                            BllExp.Exp_KFY.KFY_DJBX_FJStageInit();
                                            StageNOInList = 3;
                                            tempExist = true;
                                            break;
                                    }
                                    Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                                }
                                if (msgBoxResult == MessageBoxResult.No)
                                {
                                    BllExp.Exp_KFY.BeenCheckedDJ[i] = false;
                                }
                            }
                            else
                            {
                                switch (i)
                                {
                                    case 0:
                                        BllExp.Exp_KFY.KFY_DJBX_ZYStageInit();
                                        StageNOInList = 0;
                                        tempExist = true;
                                        break;
                                    case 1:
                                        BllExp.Exp_KFY.KFY_DJBX_ZJStageInit();
                                        StageNOInList = 1;
                                        tempExist = true;
                                        break;
                                    case 2:
                                        BllExp.Exp_KFY.KFY_DJBX_FYStageInit();
                                        StageNOInList = 2;
                                        tempExist = true;
                                        break;
                                    case 3:
                                        BllExp.Exp_KFY.KFY_DJBX_FJStageInit();
                                        StageNOInList = 3;
                                        tempExist = true;
                                        break;
                                }
                                Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                            }
                        }
                    }
                    ExistKFYp1DJExp = tempExist;
                    if (tempExist)
                    {
                        BllExp.Exp_KFY.Stage_DQ = new MQZH_StageModel_QSM();
                        BllExp.Exp_KFY.Stage_DQ = BllExp.Exp_KFY.StageList_KFYDJ[StageNOInList];
                        InitialValueSaved = false;

                        //正负压换向阀
                        switch (StageNOInList)
                        {
                            //正压
                            case 0:
                            case 1:
                                PPressTest = true;
                                break;
                            //负压
                            case 2:
                            case 3:
                                PPressTest = false;
                                break;
                        }
                    }
                }

                //抗风压p1工程检测试验开始消息分析。
                else if (msg == 1113)
                {
                    bool tempExist = false;
                    for (int i = 0; i < 4; i++)
                    {
                        //检测加压
                        if (BllExp.Exp_KFY.BeenCheckedGC[i])
                        {
                            if (!BllExp.Exp_KFY.StageList_KFYGC[i].NeedTest)
                            {
                                MessageBox.Show(BllExp.Exp_KFY.StageList_KFYGC[i].Stage_Name + "无需检测！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                                BllExp.Exp_KFY.BeenCheckedGC[i] = false;
                                return;
                            }
                            if (BllExp.Exp_KFY.StageList_KFYGC[i].CompleteStatus)
                            {
                                MessageBoxResult msgBoxResult = MessageBox.Show(BllExp.Exp_KFY.StageList_KFYGC[i].Stage_Name + "，将覆盖已完成的数据，是否覆盖？", "数据覆盖提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                                if (msgBoxResult == MessageBoxResult.Yes)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            BllExp.Exp_KFY.KFY_GCBX_ZYStageInit();
                                            StageNOInList = 0;
                                            tempExist = true;
                                            break;
                                        case 1:
                                            BllExp.Exp_KFY.KFY_GCBX_ZJStageInit();
                                            StageNOInList = 1;
                                            tempExist = true;
                                            break;
                                        case 2:
                                            BllExp.Exp_KFY.KFY_GCBX_FYStageInit();
                                            StageNOInList = 2;
                                            tempExist = true;
                                            break;
                                        case 3:
                                            BllExp.Exp_KFY.KFY_GCBX_FJStageInit();
                                            StageNOInList = 3;
                                            tempExist = true;
                                            break;
                                    }
                                    Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                                }
                                if (msgBoxResult == MessageBoxResult.No)
                                {
                                    BllExp.Exp_KFY.BeenCheckedGC[i] = false;
                                }
                            }
                            else
                            {
                                switch (i)
                                {
                                    case 0:
                                        BllExp.Exp_KFY.KFY_GCBX_ZYStageInit();
                                        StageNOInList = 0;
                                        tempExist = true;
                                        break;
                                    case 1:
                                        BllExp.Exp_KFY.KFY_GCBX_ZJStageInit();
                                        StageNOInList = 1;
                                        tempExist = true;
                                        break;
                                    case 2:
                                        BllExp.Exp_KFY.KFY_GCBX_FYStageInit();
                                        StageNOInList = 2;
                                        tempExist = true;
                                        break;
                                    case 3:
                                        BllExp.Exp_KFY.KFY_GCBX_FJStageInit();
                                        StageNOInList = 3;
                                        tempExist = true;
                                        break;
                                }
                                Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                            }
                        }
                    }
                    ExistKFYp1GCExp = tempExist;
                    if (tempExist)
                    {
                        BllExp.Exp_KFY.Stage_DQ = new MQZH_StageModel_QSM();
                        BllExp.Exp_KFY.Stage_DQ = BllExp.Exp_KFY.StageList_KFYGC[StageNOInList];
                        InitialValueSaved = false;

                        //正负压换向阀
                        switch (StageNOInList)
                        {
                            //正压
                            case 0:
                            case 1:
                                PPressTest = true;
                                break;
                            //负压
                            case 2:
                            case 3:
                                PPressTest = false;
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// 抗风压p2试验控制消息
        /// </summary>
        /// <param name="msg"></param>
        private void KFYp2CtrlMessage(int msg)
        {
            try
            {
                //抗风压损坏停机
                if ((msg == 1404) || (msg == 1414))
                {
                    if (ExistKFYp2DJExp || ExistKFYp2GCExp)
                    {
                        MessageBoxResult tempRet = MessageBox.Show("试件故障、损坏是可提前结束实验。\r\n是否结束实验？", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                        if (tempRet == MessageBoxResult.Yes)
                        {
                            DemageOrderKFY = true;
                        }
                    }
                }

                //退出试验窗口
                else if (msg == 7208)
                {
                    if (ExistKFYp2DJExp || ExistKFYp2GCExp)
                    {
                        MessageBoxResult msgBoxResult = MessageBox.Show("是否停止正在进行的试验？", "提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                        if (msgBoxResult == MessageBoxResult.Yes)
                        {
                            Messenger.Default.Send<int>(1204, "StopExpMessage");
                            Messenger.Default.Send<string>(MQZH_WinName.KFYp2Name, "CloseGivenNameWin");
                        }
                    }
                    else
                        Messenger.Default.Send<string>(MQZH_WinName.KFYp2Name, "CloseGivenNameWin");
                }

                //抗风压p2定级检测试验开始消息分析。
                else if (msg == 1104)
                {
                    bool tempExist = false;
                    for (int i = 4; i < 6; i++)
                    {
                        //检测加压
                        if (BllExp.Exp_KFY.BeenCheckedDJ[i])
                        {
                            if (!BllExp.Exp_KFY.StageList_KFYDJ[i].NeedTest)
                            {
                                MessageBox.Show(BllExp.Exp_KFY.StageList_KFYDJ[i].Stage_Name + "无需检测！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                                BllExp.Exp_KFY.BeenCheckedDJ[i] = false;
                                return;
                            }
                            if (BllExp.Exp_KFY.StageList_KFYDJ[i].CompleteStatus)
                            {
                                MessageBoxResult msgBoxResult = MessageBox.Show(BllExp.Exp_KFY.StageList_KFYDJ[i].Stage_Name + "，将覆盖已完成的数据，是否覆盖？", "数据覆盖提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                                if (msgBoxResult == MessageBoxResult.Yes)
                                {
                                    switch (i)
                                    {
                                        case 4:
                                            BllExp.Exp_KFY.KFY_DJ_ZFFStageInit();
                                            StageNOInList = 4;
                                            tempExist = true;
                                            break;
                                        case 5:
                                            BllExp.Exp_KFY.KFY_DJ_FFFStageInit();
                                            StageNOInList = 5;
                                            tempExist = true;
                                            break;
                                    }
                                    Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                                }
                                if (msgBoxResult == MessageBoxResult.No)
                                {
                                    BllExp.Exp_KFY.BeenCheckedDJ[i] = false;
                                }
                            }
                            else
                            {
                                switch (i)
                                {
                                    case 4:
                                        BllExp.Exp_KFY.KFY_DJ_ZFFStageInit();
                                        StageNOInList = 4;
                                        tempExist = true;
                                        break;
                                    case 5:
                                        BllExp.Exp_KFY.KFY_DJ_FFFStageInit();
                                        StageNOInList = 5;
                                        tempExist = true;
                                        break;
                                }
                                Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                            }
                        }
                    }
                    ExistKFYp2DJExp = tempExist;
                    if (tempExist)
                    {
                        BllExp.KFY_Evaluate();      //重新计算检测数据，以获取正确的检测压力值

                        BllExp.Exp_KFY.Stage_DQ = new MQZH_StageModel_QSM();
                        BllExp.Exp_KFY.Stage_DQ = BllExp.Exp_KFY.StageList_KFYDJ[StageNOInList];
                        InitialValueSaved = false;

                        //正负压换向阀
                        switch (StageNOInList)
                        {
                            //正压
                            case 4:
                                PPressTest = true;
                                break;
                            //负压
                            case 5:
                                PPressTest = false;
                                break;
                        }
                    }
                }

                //抗风压p2工程检测试验开始消息分析。
                else if (msg == 1114)
                {
                    bool tempExist = false;
                    for (int i = 4; i < 6; i++)
                    {
                        //检测加压
                        if (BllExp.Exp_KFY.BeenCheckedGC[i])
                        {
                            if (!BllExp.Exp_KFY.StageList_KFYGC[i].NeedTest)
                            {
                                MessageBox.Show(BllExp.Exp_KFY.StageList_KFYGC[i].Stage_Name + "无需检测！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                                BllExp.Exp_KFY.BeenCheckedGC[i] = false;
                                return;
                            }
                            if (BllExp.Exp_KFY.StageList_KFYGC[i].CompleteStatus)
                            {
                                MessageBoxResult msgBoxResult = MessageBox.Show(BllExp.Exp_KFY.StageList_KFYGC[i].Stage_Name + "，将覆盖已完成的数据，是否覆盖？", "数据覆盖提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                                if (msgBoxResult == MessageBoxResult.Yes)
                                {
                                    switch (i)
                                    {
                                        case 4:
                                            BllExp.Exp_KFY.KFY_GC_ZFFStageInit();
                                            StageNOInList = 4;
                                            tempExist = true;
                                            break;
                                        case 5:
                                            BllExp.Exp_KFY.KFY_GC_FFFStageInit();
                                            StageNOInList = 5;
                                            tempExist = true;
                                            break;
                                    }
                                    Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                                }
                                if (msgBoxResult == MessageBoxResult.No)
                                {
                                    BllExp.Exp_KFY.BeenCheckedGC[i] = false;
                                }
                            }
                            else
                            {
                                switch (i)
                                {
                                    case 4:
                                        BllExp.Exp_KFY.KFY_GC_ZFFStageInit();
                                        StageNOInList = 4;
                                        tempExist = true;
                                        break;
                                    case 5:
                                        BllExp.Exp_KFY.KFY_GC_FFFStageInit();
                                        StageNOInList = 5;
                                        tempExist = true;
                                        break;
                                }
                                Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                            }
                        }
                    }
                    ExistKFYp2GCExp = tempExist;
                    if (tempExist)
                    {
                        BllExp.KFY_Evaluate();      //重新计算检测数据，以获取正确的检测压力值

                        BllExp.Exp_KFY.Stage_DQ = new MQZH_StageModel_QSM();
                        BllExp.Exp_KFY.Stage_DQ = BllExp.Exp_KFY.StageList_KFYGC[StageNOInList];
                        InitialValueSaved = false;

                        //正负压换向阀
                        switch (StageNOInList)
                        {
                            //正压
                            case 4:
                                PPressTest = true;
                                break;
                            //负压
                            case 5:
                                PPressTest = false;
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// 抗风压p3试验控制消息
        /// </summary>
        /// <param name="msg"></param>
        private void KFYp3CtrlMessage(int msg)
        {
            try
            {
                //抗风压损坏停机
                if ((msg == 1405) || (msg == 1415))
                {
                    if (ExistKFYp3DJExp || ExistKFYp3GCExp)
                    {
                        MessageBoxResult tempRet = MessageBox.Show("试件故障、损坏是可提前结束实验。\r\n是否结束实验？", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                        if (tempRet == MessageBoxResult.Yes)
                        {
                            DemageOrderKFY = true;
                        }
                    }
                }

                //退出试验窗口
                else if (msg == 7209)
                {
                    if (ExistKFYp3DJExp || ExistKFYp3GCExp)
                    {
                        MessageBoxResult msgBoxResult = MessageBox.Show("是否停止正在进行的试验？", "提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                        if (msgBoxResult == MessageBoxResult.Yes)
                        {
                            Messenger.Default.Send<int>(1205, "StopExpMessage");
                            Messenger.Default.Send<string>(MQZH_WinName.KFYp3Name, "CloseGivenNameWin");
                        }
                    }
                    else
                        Messenger.Default.Send<string>(MQZH_WinName.KFYp3Name, "CloseGivenNameWin");
                }

                //抗风压p3定级检测试验开始消息分析。
                else if (msg == 1105)
                {
                    bool tempExist = false;
                    for (int i = 6; i < 8; i++)
                    {
                        //检测加压
                        if (BllExp.Exp_KFY.BeenCheckedDJ[i])
                        {
                            if (!BllExp.Exp_KFY.StageList_KFYDJ[i].NeedTest)
                            {
                                MessageBox.Show(BllExp.Exp_KFY.StageList_KFYDJ[i].Stage_Name + "无需检测！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                                BllExp.Exp_KFY.BeenCheckedDJ[i] = false;
                                return;
                            }
                            if (BllExp.Exp_KFY.StageList_KFYDJ[i].CompleteStatus)
                            {
                                MessageBoxResult msgBoxResult = MessageBox.Show(BllExp.Exp_KFY.StageList_KFYDJ[i].Stage_Name + "，将覆盖已完成的数据，是否覆盖？", "数据覆盖提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                                if (msgBoxResult == MessageBoxResult.Yes)
                                {
                                    switch (i)
                                    {
                                        case 6:
                                            BllExp.Exp_KFY.KFY_DJ_ZP3StageInit();
                                            StageNOInList = 6;
                                            tempExist = true;
                                            break;
                                        case 7:
                                            BllExp.Exp_KFY.KFY_DJ_FP3StageInit();
                                            StageNOInList = 7;
                                            tempExist = true;
                                            break;
                                    }
                                    Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                                }
                                if (msgBoxResult == MessageBoxResult.No)
                                {
                                    BllExp.Exp_KFY.BeenCheckedDJ[i] = false;
                                }
                            }
                            else
                            {
                                switch (i)
                                {
                                    case 6:
                                        BllExp.Exp_KFY.KFY_DJ_ZP3StageInit();
                                        StageNOInList = 6;
                                        tempExist = true;
                                        break;
                                    case 7:
                                        BllExp.Exp_KFY.KFY_DJ_FP3StageInit();
                                        StageNOInList = 7;
                                        tempExist = true;
                                        break;
                                }
                                Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                            }
                        }
                    }
                    ExistKFYp3DJExp = tempExist;
                    if (tempExist)
                    {
                        BllExp.Exp_KFY.Stage_DQ = new MQZH_StageModel_QSM();
                        BllExp.Exp_KFY.Stage_DQ = BllExp.Exp_KFY.StageList_KFYDJ[StageNOInList];
                        InitialValueSaved = false;

                        //正负压换向阀
                        switch (StageNOInList)
                        {
                            //正压
                            case 6:
                                PPressTest = true;
                                break;
                            //负压
                            case 7:
                                PPressTest = false;
                                break;
                        }
                    }
                }

                //抗风压p3工程检测试验开始消息分析。
                else if (msg == 1115)
                {
                    bool tempExist = false;
                    for (int i = 6; i < 8; i++)
                    {
                        //检测加压
                        if (BllExp.Exp_KFY.BeenCheckedGC[i])
                        {
                            if (!BllExp.Exp_KFY.StageList_KFYGC[i].NeedTest)
                            {
                                MessageBox.Show(BllExp.Exp_KFY.StageList_KFYGC[i].Stage_Name + "无需检测！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                                BllExp.Exp_KFY.BeenCheckedGC[i] = false;
                                return;
                            }
                            if (BllExp.Exp_KFY.StageList_KFYGC[i].CompleteStatus)
                            {
                                MessageBoxResult msgBoxResult = MessageBox.Show(BllExp.Exp_KFY.StageList_KFYGC[i].Stage_Name + "，将覆盖已完成的数据，是否覆盖？", "数据覆盖提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                                if (msgBoxResult == MessageBoxResult.Yes)
                                {
                                    switch (i)
                                    {
                                        case 6:
                                            BllExp.Exp_KFY.KFY_GC_ZP3StageInit();
                                            StageNOInList = 6;
                                            tempExist = true;
                                            break;
                                        case 7:
                                            BllExp.Exp_KFY.KFY_GC_FP3StageInit();
                                            StageNOInList = 7;
                                            tempExist = true;
                                            break;
                                    }
                                    Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                                }
                                if (msgBoxResult == MessageBoxResult.No)
                                {
                                    BllExp.Exp_KFY.BeenCheckedGC[i] = false;
                                }
                            }
                            else
                            {
                                switch (i)
                                {
                                    case 6:
                                        BllExp.Exp_KFY.KFY_GC_ZP3StageInit();
                                        StageNOInList = 6;
                                        tempExist = true;
                                        break;
                                    case 7:
                                        BllExp.Exp_KFY.KFY_GC_FP3StageInit();
                                        StageNOInList = 7;
                                        tempExist = true;
                                        break;
                                }
                                Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                            }
                        }
                    }
                    ExistKFYp3GCExp = tempExist;
                    if (tempExist)
                    {
                        BllExp.KFY_Evaluate();      //重新计算检测数据，以获取正确的检测压力值

                        BllExp.Exp_KFY.Stage_DQ = new MQZH_StageModel_QSM();
                        BllExp.Exp_KFY.Stage_DQ = BllExp.Exp_KFY.StageList_KFYGC[StageNOInList];
                        InitialValueSaved = false;

                        //正负压换向阀
                        switch (StageNOInList)
                        {
                            //正压
                            case 6:
                                PPressTest = true;
                                break;
                            //负压
                            case 7:
                                PPressTest = false;
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// 抗风压pmax试验控制消息
        /// </summary>
        /// <param name="msg"></param>
        private void KFYpmaxCtrlMessage(int msg)
        {
            try
            {
                //抗风压损坏停机
                if ((msg == 1406) || (msg == 1416))
                {
                    if (ExistKFYpmaxDJExp || ExistKFYpmaxGCExp)
                    {
                        MessageBoxResult tempRet = MessageBox.Show("试件故障、损坏是可提前结束实验。\r\n是否结束实验？", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                        if (tempRet == MessageBoxResult.Yes)
                        {
                            DemageOrderKFY = true;
                        }
                    }
                }

                //退出试验窗口
                else if (msg == 7210)
                {
                    if (ExistKFYpmaxDJExp || ExistKFYpmaxGCExp)
                    {
                        MessageBoxResult msgBoxResult = MessageBox.Show("是否停止正在进行的试验？", "提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                        if (msgBoxResult == MessageBoxResult.Yes)
                        {
                            Messenger.Default.Send<int>(1206, "StopExpMessage");
                            Messenger.Default.Send<string>(MQZH_WinName.KFYpmaxName, "CloseGivenNameWin");
                        }
                    }
                    else
                        Messenger.Default.Send<string>(MQZH_WinName.KFYpmaxName, "CloseGivenNameWin");
                }
                //抗风压pmax定级检测试验开始消息分析。
                else if (msg == 1106)
                {
                    bool tempExist = false;
                    for (int i = 8; i < 10; i++)
                    {
                        //检测加压
                        if (BllExp.Exp_KFY.BeenCheckedDJ[i])
                        {
                            if (!BllExp.Exp_KFY.StageList_KFYDJ[i].NeedTest)
                            {
                                MessageBox.Show(BllExp.Exp_KFY.StageList_KFYDJ[i].Stage_Name + "无需检测！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                                BllExp.Exp_KFY.BeenCheckedDJ[i] = false;
                                return;
                            }
                            if (BllExp.Exp_KFY.StageList_KFYDJ[i].CompleteStatus)
                            {
                                MessageBoxResult msgBoxResult = MessageBox.Show(BllExp.Exp_KFY.StageList_KFYDJ[i].Stage_Name + "，将覆盖已完成的数据，是否覆盖？", "数据覆盖提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                                if (msgBoxResult == MessageBoxResult.Yes)
                                {
                                    switch (i)
                                    {
                                        case 8:
                                            BllExp.Exp_KFY.KFY_DJ_ZPmaxStageInit();
                                            StageNOInList = 8;
                                            tempExist = true;
                                            break;
                                        case 9:
                                            BllExp.Exp_KFY.KFY_DJ_FPmaxStageInit();
                                            StageNOInList = 9;
                                            tempExist = true;
                                            break;
                                    }
                                    Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                                }
                                if (msgBoxResult == MessageBoxResult.No)
                                {
                                    BllExp.Exp_KFY.BeenCheckedDJ[i] = false;
                                }
                            }
                            else
                            {
                                switch (i)
                                {
                                    case 8:
                                        BllExp.Exp_KFY.KFY_DJ_ZPmaxStageInit();
                                        StageNOInList = 8;
                                        tempExist = true;
                                        break;
                                    case 9:
                                        BllExp.Exp_KFY.KFY_DJ_FPmaxStageInit();
                                        StageNOInList = 9;
                                        tempExist = true;
                                        break;
                                }
                                Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                            }
                        }
                    }
                    ExistKFYpmaxDJExp = tempExist;
                    if (tempExist)
                    {
                        BllExp.Exp_KFY.Stage_DQ = new MQZH_StageModel_QSM();
                        BllExp.Exp_KFY.Stage_DQ = BllExp.Exp_KFY.StageList_KFYDJ[StageNOInList];
                        InitialValueSaved = false;

                        //正负压换向阀
                        switch (StageNOInList)
                        {
                            //正压
                            case 8:
                                PPressTest = true;
                                break;
                            //负压
                            case 9:
                                PPressTest = false;
                                break;
                        }
                    }
                }

                //抗风压pmax工程检测试验开始消息分析。
                else if (msg == 1116)
                {
                    bool tempExist = false;
                    for (int i = 8; i < 10; i++)
                    {
                        //检测加压
                        if (BllExp.Exp_KFY.BeenCheckedGC[i])
                        {
                            if (!BllExp.Exp_KFY.StageList_KFYGC[i].NeedTest)
                            {
                                MessageBox.Show(BllExp.Exp_KFY.StageList_KFYGC[i].Stage_Name + "无需检测！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                                BllExp.Exp_KFY.BeenCheckedGC[i] = false;
                                return;
                            }
                            if (BllExp.Exp_KFY.StageList_KFYGC[i].CompleteStatus)
                            {
                                MessageBoxResult msgBoxResult = MessageBox.Show(BllExp.Exp_KFY.StageList_KFYGC[i].Stage_Name + "，将覆盖已完成的数据，是否覆盖？", "数据覆盖提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                                if (msgBoxResult == MessageBoxResult.Yes)
                                {
                                    switch (i)
                                    {
                                        case 8:
                                            BllExp.Exp_KFY.KFY_GC_ZPmaxStageInit();
                                            StageNOInList = 8;
                                            tempExist = true;
                                            break;
                                        case 9:
                                            BllExp.Exp_KFY.KFY_GC_FPmaxStageInit();
                                            StageNOInList = 9;
                                            tempExist = true;
                                            break;
                                    }
                                    Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                                }
                                if (msgBoxResult == MessageBoxResult.No)
                                {
                                    BllExp.Exp_KFY.BeenCheckedGC[i] = false;
                                }
                            }
                            else
                            {
                                switch (i)
                                {
                                    case 8:
                                        BllExp.Exp_KFY.KFY_GC_ZPmaxStageInit();
                                        StageNOInList = 8;
                                        tempExist = true;
                                        break;
                                    case 9:
                                        BllExp.Exp_KFY.KFY_GC_FPmaxStageInit();
                                        StageNOInList = 9;
                                        tempExist = true;
                                        break;
                                }
                                Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
                            }
                        }
                    }
                    ExistKFYpmaxGCExp = tempExist;
                    if (tempExist)
                    {
                        BllExp.KFY_Evaluate();      //重新计算检测数据，以获取正确的检测压力值

                        BllExp.Exp_KFY.Stage_DQ = new MQZH_StageModel_QSM();
                        BllExp.Exp_KFY.Stage_DQ = BllExp.Exp_KFY.StageList_KFYGC[StageNOInList];
                        InitialValueSaved = false;

                        //正负压换向阀
                        switch (StageNOInList)
                        {
                            //正压
                            case 8:
                                PPressTest = true;
                                break;
                            //负压
                            case 9:
                                PPressTest = false;
                                break;
                        }
                    }
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
        /// 有未完成抗风压定级p1试验
        /// </summary>
        private bool _existKFYp1DJExp = false;
        /// <summary>
        /// 有未完成抗风压定级p1试验
        /// </summary>
        public bool ExistKFYp1DJExp
        {
            get { return _existKFYp1DJExp; }
            set
            {
                _existKFYp1DJExp = value;
                RaisePropertyChanged(() => ExistKFYp1DJExp);
            }
        }

        /// <summary>
        /// 有未完成抗风压定级p2试验
        /// </summary>
        private bool _existKFYp2DJExp = false;
        /// <summary>
        /// 有未完成抗风压定级p2试验
        /// </summary>
        public bool ExistKFYp2DJExp
        {
            get { return _existKFYp2DJExp; }
            set
            {
                _existKFYp2DJExp = value;
                RaisePropertyChanged(() => ExistKFYp2DJExp);
            }
        }

        /// <summary>
        /// 有未完成抗风压定级p3试验
        /// </summary>
        private bool _existKFYp3DJExp = false;
        /// <summary>
        /// 有未完成抗风压定级p3试验
        /// </summary>
        public bool ExistKFYp3DJExp
        {
            get { return _existKFYp3DJExp; }
            set
            {
                _existKFYp3DJExp = value;
                RaisePropertyChanged(() => ExistKFYp3DJExp);
            }
        }

        /// <summary>
        /// 有未完成抗风压定级pmax试验
        /// </summary>
        private bool _existKFYpmaxDJExp = false;
        /// <summary>
        /// 有未完成抗风压定级pmax试验
        /// </summary>
        public bool ExistKFYpmaxDJExp
        {
            get { return _existKFYpmaxDJExp; }
            set
            {
                _existKFYpmaxDJExp = value;
                RaisePropertyChanged(() => ExistKFYpmaxDJExp);
            }
        }

        /// <summary>
        /// 有未完成抗风压工程p1试验
        /// </summary>
        private bool _existKFYp1GCExp = false;
        /// <summary>
        /// 有未完成抗风压工程p1试验
        /// </summary>
        public bool ExistKFYp1GCExp
        {
            get { return _existKFYp1GCExp; }
            set
            {
                _existKFYp1GCExp = value;
                RaisePropertyChanged(() => ExistKFYp1GCExp);
            }
        }

        /// <summary>
        /// 有未完成抗风压工程p2试验
        /// </summary>
        private bool _existKFYp2GCExp = false;
        /// <summary>
        /// 有未完成抗风压工程p2试验
        /// </summary>
        public bool ExistKFYp2GCExp
        {
            get { return _existKFYp2GCExp; }
            set
            {
                _existKFYp2GCExp = value;
                RaisePropertyChanged(() => ExistKFYp2GCExp);
            }
        }

        /// <summary>
        /// 有未完成抗风压工程p3试验
        /// </summary>
        private bool _existKFYp3GCExp = false;
        /// <summary>
        /// 有未完成抗风压工程p3试验
        /// </summary>
        public bool ExistKFYp3GCExp
        {
            get { return _existKFYp3GCExp; }
            set
            {
                _existKFYp3GCExp = value;
                RaisePropertyChanged(() => ExistKFYp3GCExp);
            }
        }

        /// <summary>
        /// 有未完成抗风压工程pmax试验
        /// </summary>
        private bool _existKFYpmaxGCExp = false;
        /// <summary>
        /// 有未完成抗风压工程pmax试验
        /// </summary>
        public bool ExistKFYpmaxGCExp
        {
            get { return _existKFYpmaxGCExp; }
            set
            {
                _existKFYpmaxGCExp = value;
                RaisePropertyChanged(() => ExistKFYpmaxGCExp);
            }
        }

        /// <summary>
        /// 有抗风压损坏指令
        /// </summary>
        private bool _demageOrderKFY = false;
        /// <summary>
        /// 有抗风压损坏指令
        /// </summary>
        public bool DemageOrderKFY
        {
            get { return _demageOrderKFY; }
            set
            {
                _demageOrderKFY = value;
                RaisePropertyChanged(() => DemageOrderKFY);
            }
        }

        /// <summary>
        /// 已保存初值标志
        /// </summary>
        private bool _initialValueSaved = false;
        /// <summary>
        /// 已保存初值标志
        /// </summary>
        public bool InitialValueSaved
        {
            get { return _initialValueSaved; }
            set
            {
                _initialValueSaved = value;
                RaisePropertyChanged(() => InitialValueSaved);
            }
        }

        #endregion

    }
}