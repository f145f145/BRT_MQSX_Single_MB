/************************************************************************************
 * 描述：
 * 幕墙四性试验操作业务BLL——变形部分
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2021/11/20       19:02:11		郝正强			V1.0.0.0
 * 
 ************************************************************************************/

using System;
using GalaSoft.MvvmLight;
using MQDFJ_MB.Model.Exp;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using System.Threading;
using MQDFJ_MB.Model;

namespace MQDFJ_MB.BLL
{
    /// <summary>
    /// 主控类
    /// </summary>
    public partial class MQZH_ExpBLL : ObservableObject
    {
        #region 层间变形阶段试验实施程序

        /// <summary>
        /// 层间变形定级X各阶段试验实施程序
        /// </summary>
        private void CJBX_DJXStageFunc()
        {
            try
            {
                //如果当前阶段是默认阶段，则复位状态
                if ((BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StageNO == "99") || (StageNOInList == -1))
                {
                    MessageBox.Show("默认阶段不能做实验");
                    EscStopCompleteExpReset();
                    return;
                }
                BllDev.IsDeviceBusy = true;

                //开始前提醒
                if (BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsNeedTipsBefore &&
                    (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsBeforeComplete))
                {
                    MessageBox.Show(BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StringTipsBefore, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsBeforeComplete = true;
                }

                //逐个步骤测试
                int stepsCount = BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList.Count;
                for (int i = 0; i < stepsCount; i++)
                {
                    if (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsStepCompleted)
                    {
                        //步骤开始前等待
                        if (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsWaitBeforCompleted)
                        {
                            if (!IsWaitingStart)
                            {
                                WaitingTimeStart = DateTime.Now;
                                IsWaitingStart = true;
                            }
                            else
                            {
                                TimeSpan tempSpan = DateTime.Now - WaitingTimeStart;
                                if (tempSpan.TotalSeconds >= BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].TimeWaitBefor)
                                {
                                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsWaitBeforCompleted = true;
                                    IsWaitingStart = false;
                                }
                            }
                            return;
                        }

                        //标记步骤开始并记录开始时间
                        if (!IsStepStart)
                        {
                            StepStartTime = DateTime.Now;
                            IsStepStart = true;
                        }

                        //发送指令
                        if (!IsStepOrderSend)
                        {
                            BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ = BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i];
                            double aimPositionX = 0;
                            UInt16 direction = 0;
                            double rateHL_CG;
                            if (BllDev.CJBXType == 0)                   
                                rateHL_CG = BllDev.H_HDK / 1000 / BllExp.ExpSettingParam.SJ_CG;  //连续平行四边形法，比率=活动框高度/试件层高
                            else
                                rateHL_CG = 1;  //层间变形法，比率=1

                            //活动框移动距离=试件目标位置*比率
                            aimPositionX = BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].AimPosition * rateHL_CG;
                            if (Math.Abs(BllDev.WYValueX - aimPositionX) > Math.Abs(BllDev.PermitErrX))
                            {
                                //将相对位置换算为目标点位采集值
                                if (BllDev.WYValueX <= aimPositionX)
                                {
                                    aimPositionX += BllDev.CorrXRight;
                                    direction = 1;
                                }
                                else
                                {
                                    aimPositionX -= BllDev.CorrXLeft;
                                    direction = 2;
                                }
                                BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ.AimPositionSB = aimPositionX;
                                double dataTrans = BllDev.AIList[BllDev.WYNOList[0] - 1].GetTransDataFromValueFinal_AI(-aimPositionX);       //X对应传输值
                                if (dataTrans > 4000)
                                    dataTrans = 4000;
                                if (dataTrans < 0)
                                    dataTrans = 0;

                                //生成指令
                                OnceOrderValuesFrmBLL = Enumerable.Repeat((ushort)0, 24).ToArray();
                                OnceOrderValuesFrmBLL[0] = 2;      //位移模式
                                OnceOrderValuesFrmBLL[1] = 500;    //定点模式
                                OnceOrderValuesFrmBLL[13] = direction;                                      //运动方向，1X右，2X左，3Y前，4Y后，5Z上，6Z下
                                OnceOrderValuesFrmBLL[14] = Convert.ToUInt16(dataTrans);                   //X轴定位位置
                                OnceOrderValuesFrmBLL[17] = Convert.ToUInt16(BllDev.WYNOList[0] - 1);      //X位移尺编号
                                Messenger.Default.Send<ushort[]>(OnceOrderValuesFrmBLL, "OnceOrderToComMessage");
                                IsStepOrderSend = true;
                                //到位状态复位
                                StepCompleteStatusBack = false;
                                //等待
                                Thread.Sleep(500);
                            }
                            //目标在误差范围内
                            else
                            {
                                BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsStepCompleted = true;
                                BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ = new MQZH_StepModel_CJBX();
                                IsStepStart = false;
                                IsStepOrderSend = false;
                                return;
                            }
                        }
                        //判断到位状态
                        StepCompleteStatusBack = BllDev.DIList[18].IsOn;
                        if (StepCompleteStatusBack)
                        {
                            BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsStepCompleted = true;
                            BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ = new MQZH_StepModel_CJBX();
                            IsStepStart = false;
                            IsStepOrderSend = false;
                        }
                        return;
                    }
                }

                //结束后提醒
                if (BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsNeedTipsAfter &&
                    (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsAfterComplete))
                {
                    MessageBox.Show(BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StringTipsAfter, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsAfterComplete = true;
                }

                //以上步骤全部完成后，本阶段完成
                BllExp.Exp_CJBX.StageList_DJX[StageNOInList].WYJ_XYFM_SJ = BllExp.Exp_CJBX.StageList_DJX[StageNOInList].WYJMB_XYFM_SJ;
                BllExp.Exp_CJBX.StageList_DJX[StageNOInList].WY_XY_SJ = Math.Abs(BllExp.ExpSettingParam.SJ_CG * 1000) / BllExp.Exp_CJBX.StageList_DJX[StageNOInList].WYJMB_XYFM_SJ;     //实际位移=层高/位移角分母
                BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.CompleteStatus = true;

                //判断是否完成
                if (BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.CompleteStatus)
                {
                    //检测数据保存
                    BllExp.ExpData_CJBX.AngleFM_DJ_X[StageNOInList] = BllExp.Exp_CJBX.StageList_DJX[StageNOInList].WYJ_XYFM_SJ;
                    BllExp.ExpData_CJBX.DSPL_DJ_X[StageNOInList] = BllExp.Exp_CJBX.StageList_DJX[StageNOInList].WY_XY_SJ;
                    Messenger.Default.Send<string>("SaveCJBXStatusAndData", "SaveExpMessage");
                    //损坏情况检查确认
                    OpenCJBXDemageDJWin();

                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ = new MQZH_StageModel_CJBX();
                    EscStopCompleteExpReset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "警告", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
            }
        }

        /// <summary>
        /// 层间变形定级Y各阶段试验实施程序
        /// </summary>
        private void CJBX_DJYStageFunc()
        {
            try
            {
                //如果当前阶段是默认阶段，则复位状态
                if ((BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StageNO == "99") || (StageNOInList == -1))
                {
                    MessageBox.Show("默认阶段不能做实验");
                    EscStopCompleteExpReset();
                    return;
                }
                BllDev.IsDeviceBusy = true;

                //开始前提醒
                if (BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsNeedTipsBefore &&
                    (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsBeforeComplete))
                {
                    MessageBox.Show(BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StringTipsBefore, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsBeforeComplete = true;
                }

                //逐个步骤测试
                int stepsCount = BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList.Count;
                for (int i = 0; i < stepsCount; i++)
                {
                    if (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsStepCompleted)
                    {
                        //步骤开始前等待
                        if (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsWaitBeforCompleted)
                        {
                            if (!IsWaitingStart)
                            {
                                WaitingTimeStart = DateTime.Now;
                                IsWaitingStart = true;
                            }
                            else
                            {
                                TimeSpan tempSpan = DateTime.Now - WaitingTimeStart;
                                if (tempSpan.TotalSeconds >= BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].TimeWaitBefor)
                                {
                                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsWaitBeforCompleted = true;
                                    IsWaitingStart = false;
                                }
                            }
                            return;
                        }

                        //标记步骤开始并记录开始时间
                        if (!IsStepStart)
                        {
                            StepStartTime = DateTime.Now;
                            IsStepStart = true;
                        }
                        //发送指令
                        if (!IsStepOrderSend)
                        {
                            BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ =  BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i];
                            double aimPositionY = 0;
                            UInt16 direction = 0;

                            double rateHL_CG;
                            if (BllDev.CJBXType == 0)
                                rateHL_CG = BllDev.H_HDK / 1000 / BllExp.ExpSettingParam.SJ_CG;  //连续平行四边形法，比率=活动框高度/试件层高
                            else
                                rateHL_CG = 1;  //层间变形法，比率=1

                            //活动框移动距离=试件目标位置*比率
                            aimPositionY = BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].AimPosition * rateHL_CG;
                            if (Math.Abs(BllDev.WYValueY[3] - aimPositionY) > Math.Abs(BllDev.PermitErrY))
                            {
                                //将相对位置换算为目标点位采集值
                                if (BllDev.WYValueY[3] <= aimPositionY)
                                {
                                    aimPositionY += BllDev.CorrYFront;
                                    direction = 3;
                                }
                                else
                                {
                                    aimPositionY -= BllDev.CorrYBack;
                                    direction = 4;
                                }
                                BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ.AimPositionSB = aimPositionY;
                                double dataTrans1 = BllDev.AIList[BllDev.WYNOList[1] - 1].GetTransDataFromValueFinal_AI(-aimPositionY);       //左点对应传输值
                                double dataTrans2 = BllDev.AIList[BllDev.WYNOList[2] - 1].GetTransDataFromValueFinal_AI(-aimPositionY);       //中间点对应传输值
                                double dataTrans3 = BllDev.AIList[BllDev.WYNOList[3] - 1].GetTransDataFromValueFinal_AI(-aimPositionY);       //右点对应传输值
                                double dataTransAverage = (dataTrans1 + dataTrans2 + dataTrans3) / 3;
                                if (dataTransAverage > 4000)
                                    dataTransAverage = 4000;
                                if (dataTransAverage < 0)
                                    dataTransAverage = 0;

                                //生成指令
                                OnceOrderValuesFrmBLL = Enumerable.Repeat((ushort)0, 24).ToArray();
                                OnceOrderValuesFrmBLL[0] = 2;      //位移模式
                                OnceOrderValuesFrmBLL[1] = 500;    //定点模式
                                OnceOrderValuesFrmBLL[13] = direction;                                     //运动方向，1X右，2X左，3Y前，4Y后，5Z上，6Z下
                                OnceOrderValuesFrmBLL[15] = Convert.ToUInt16(dataTransAverage);            //Y轴定位位置
                                OnceOrderValuesFrmBLL[18] = Convert.ToUInt16(BllDev.WYNOList[1] - 1);     //Y左位移尺编号
                                OnceOrderValuesFrmBLL[19] = Convert.ToUInt16(BllDev.WYNOList[2] - 1);     //Y中位移尺编号
                                OnceOrderValuesFrmBLL[20] = Convert.ToUInt16(BllDev.WYNOList[3] - 1);     //Y右位移尺编号
                                Messenger.Default.Send<ushort[]>(OnceOrderValuesFrmBLL, "OnceOrderToComMessage");
                                IsStepOrderSend = true;
                                //到位状态复位
                                StepCompleteStatusBack = false;
                                //等待
                                Thread.Sleep(500);
                            }
                            //目标在误差范围内
                            else
                            {
                                BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsStepCompleted = true;
                                BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ = new MQZH_StepModel_CJBX();
                                IsStepStart = false;
                                IsStepOrderSend = false;
                                return;
                            }
                        }
                        //判断到位状态
                        StepCompleteStatusBack = BllDev.DIList[19].IsOn;
                        if (StepCompleteStatusBack)
                        {
                            BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsStepCompleted = true;
                            BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ = new MQZH_StepModel_CJBX();
                            IsStepStart = false;
                            IsStepOrderSend = false;
                        }
                        return;
                    }
                }

                //结束后提醒
                if (BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsNeedTipsAfter &&
                    (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsAfterComplete))
                {
                    MessageBox.Show(BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StringTipsAfter, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsAfterComplete = true;
                }

                //以上步骤全部完成后，本阶段完成
                BllExp.Exp_CJBX.StageList_DJY[StageNOInList].WYJ_XYFM_SJ = BllExp.Exp_CJBX.StageList_DJY[StageNOInList].WYJMB_XYFM_SJ;
                BllExp.Exp_CJBX.StageList_DJY[StageNOInList].WY_XY_SJ = Math.Abs(BllExp.ExpSettingParam.SJ_CG * 1000) / BllExp.Exp_CJBX.StageList_DJY[StageNOInList].WYJMB_XYFM_SJ;     //实际位移=层高/位移角分母
                BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.CompleteStatus = true;

                //判断是否完成
                if (BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.CompleteStatus)
                {
                    //检测数据保存
                    BllExp.ExpData_CJBX.AngleFM_DJ_Y[StageNOInList] = BllExp.Exp_CJBX.StageList_DJY[StageNOInList].WYJ_XYFM_SJ;
                    BllExp.ExpData_CJBX.DSPL_DJ_Y[StageNOInList] = BllExp.Exp_CJBX.StageList_DJY[StageNOInList].WY_XY_SJ;
                    Messenger.Default.Send<string>("SaveCJBXStatusAndData", "SaveExpMessage");
                    //损坏情况检查确认
                    OpenCJBXDemageDJWin();

                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ = new MQZH_StageModel_CJBX();
                    EscStopCompleteExpReset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "警告", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
            }
        }

        /// <summary>
        /// 层间变形定级Z各阶段试验实施程序
        /// </summary>
        public void CJBX_DJZStageFunc()
        {
            try
            {
                //如果当前阶段是默认阶段，则复位状态
                if ((BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StageNO == "99") || (StageNOInList == -1))
                {
                    MessageBox.Show("默认阶段不能做实验");
                    EscStopCompleteExpReset();
                    return;
                }
                BllDev.IsDeviceBusy = true;

                //开始前提醒
                if (BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsNeedTipsBefore &&
                    (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsBeforeComplete))
                {
                    MessageBox.Show(BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StringTipsBefore, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsBeforeComplete = true;
                }

                //逐个步骤测试
                int stepsCount = BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList.Count;
                for (int i = 0; i < stepsCount; i++)
                {
                    //步骤开始前等待
                    if (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsWaitBeforCompleted)
                    {
                        if (!IsWaitingStart)
                        {
                            WaitingTimeStart = DateTime.Now;
                            IsWaitingStart = true;
                        }
                        TimeSpan tempSpan = DateTime.Now - WaitingTimeStart;
                        if (tempSpan.TotalSeconds >= BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].TimeWaitBefor)
                        {
                            BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsWaitBeforCompleted = true;
                            IsWaitingStart = false;
                            return;
                        }
                    }
                    //本步骤试验
                    if (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsStepCompleted)
                    {
                        //判断步骤是否已经开始
                        if (!IsStepStart)
                        {
                            StepStartTime = DateTime.Now;
                            IsStepStart = true;
                        }
                        TimeSpan tempSpan = DateTime.Now - StepStartTime;

                        //发送指令
                        if (!IsStepOrderSend)
                        {
                            BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ =BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i];
                            double aimPositionZ = 0;
                            UInt16 direction = 0;
                            if (Math.Abs(BllDev.WYValueZ[3] - BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].AimPosition) > Math.Abs(BllDev.PermitErrZ))
                            {
                                //将相对位置换算为目标点位采集值
                                if (BllDev.WYValueZ[3] <= BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].AimPosition)
                                {
                                    aimPositionZ = BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].AimPosition + BllDev.CorrZUp;
                                    direction = 5;
                                }
                                else
                                {
                                    aimPositionZ = BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].AimPosition - BllDev.CorrZDown;
                                    direction = 6;
                                }
                                BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ.AimPositionSB = aimPositionZ;

                                double dataTrans1 = BllDev.AIList[BllDev.WYNOList[4] - 1].GetTransDataFromValueFinal_AI(aimPositionZ);       //左点对应传输值
                                double dataTrans2 = BllDev.AIList[BllDev.WYNOList[5] - 1].GetTransDataFromValueFinal_AI(aimPositionZ);       //中间点对应传输值
                                double dataTrans3 = BllDev.AIList[BllDev.WYNOList[6] - 1].GetTransDataFromValueFinal_AI(aimPositionZ);       //右点对应传输值
                                double dataTransAverage = (dataTrans1 + dataTrans2 + dataTrans3) / 3;
                                if (dataTransAverage > 4000)
                                    dataTransAverage = 4000;
                                if (dataTransAverage < 0)
                                    dataTransAverage = 0;

                                //生成指令
                                OnceOrderValuesFrmBLL = Enumerable.Repeat((ushort)0, 24).ToArray();
                                OnceOrderValuesFrmBLL[0] = 2;      //位移模式
                                OnceOrderValuesFrmBLL[1] = 500;    //定点模式
                                OnceOrderValuesFrmBLL[13] = direction;                                     //运动方向，1X右，2X左，3Y前，4Y后，5Z上，6Z下
                                OnceOrderValuesFrmBLL[16] = Convert.ToUInt16(dataTransAverage);            //Z轴定位位置
                                OnceOrderValuesFrmBLL[21] = Convert.ToUInt16(BllDev.WYNOList[4] - 1);     //Z左位移尺编号
                                OnceOrderValuesFrmBLL[22] = Convert.ToUInt16(BllDev.WYNOList[5] - 1);     //Z中位移尺编号
                                OnceOrderValuesFrmBLL[23] = Convert.ToUInt16(BllDev.WYNOList[6] - 1);     //Z右位移尺编号
                                Messenger.Default.Send<ushort[]>(OnceOrderValuesFrmBLL, "OnceOrderToComMessage");
                                IsStepOrderSend = true;
                                //到位状态复位
                                StepCompleteStatusBack = false;
                                //等待
                                Thread.Sleep(500);
                            }
                            //目标在误差范围内
                            else
                            {
                                BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsStepCompleted = true;
                                BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ = new MQZH_StepModel_CJBX();
                                IsStepStart = false;
                                IsStepOrderSend = false;
                                return;
                            }
                        }
                        //判断到位状态
                        StepCompleteStatusBack = BllDev.DIList[20].IsOn;
                        if (StepCompleteStatusBack)
                        {
                            BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsStepCompleted = true;
                            BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ = new MQZH_StepModel_CJBX();
                            IsStepStart = false;
                            IsStepOrderSend = false;
                        }
                        return;
                    }
                }

                //结束后提醒
                if (BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsNeedTipsAfter &&
                    (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsAfterComplete))
                {
                    MessageBox.Show(BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StringTipsAfter, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsAfterComplete = true;
                }

                //以上步骤全部完成后，本阶段完成
                BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].WY_Z_SJ =
                    BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].WYMB_Z_SJ;
                BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.CompleteStatus = true;

                //判断是否完成
                if (BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.CompleteStatus)
                {
                    //检测数据保存
                    BllExp.ExpData_CJBX.DSPL_DJ_Z[StageNOInList] = BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].WY_Z_SJ;
                    Messenger.Default.Send<string>("SaveCJBXStatusAndData", "SaveExpMessage");
                    //损坏情况检查确认
                    OpenCJBXDemageDJWin();

                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ = new MQZH_StageModel_CJBX();
                    EscStopCompleteExpReset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "警告", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
            }
        }

        /// <summary>
        /// 层间变形工程X各阶段试验实施程序
        /// </summary>
        private void CJBX_GCXStageFunc()
        {
            try
            {
                //如果当前阶段是默认阶段，则复位状态
                if ((BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StageNO == "99") || (StageNOInList == -1))
                {
                    MessageBox.Show("默认阶段不能做实验");
                    EscStopCompleteExpReset();
                    return;
                }
                BllDev.IsDeviceBusy = true;

                //开始前提醒
                if (BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsNeedTipsBefore &&
                    (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsBeforeComplete))
                {
                    MessageBox.Show(BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StringTipsBefore, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsBeforeComplete = true;
                }

                //逐个步骤测试
                int stepsCount = BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList.Count;
                for (int i = 0; i < stepsCount; i++)
                {
                    if (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsStepCompleted)
                    {
                        //步骤开始前等待
                        if (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsWaitBeforCompleted)
                        {
                            if (!IsWaitingStart)
                            {
                                WaitingTimeStart = DateTime.Now;
                                IsWaitingStart = true;
                            }
                            else
                            {
                                TimeSpan tempSpan = DateTime.Now - WaitingTimeStart;
                                if (tempSpan.TotalSeconds >= BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].TimeWaitBefor)
                                {
                                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsWaitBeforCompleted = true;
                                    IsWaitingStart = false;
                                }
                            }
                            return;
                        }

                        //标记步骤开始并记录开始时间
                        if (!IsStepStart)
                        {
                            StepStartTime = DateTime.Now;
                            IsStepStart = true;
                        }

                        //发送指令
                        if (!IsStepOrderSend)
                        {
                            BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ = BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i];
                            double aimPositionX = 0;
                            UInt16 direction = 0;

                            double rateHL_CG;
                            if (BllDev.CJBXType == 0)
                                rateHL_CG = BllDev.H_HDK / 1000 / BllExp.ExpSettingParam.SJ_CG;  //连续平行四边形法，比率=活动框高度/试件层高
                            else
                                rateHL_CG = 1;  //层间变形法，比率=1

                            //活动框移动距离=试件目标位置*比率
                            aimPositionX = BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].AimPosition * rateHL_CG;
                            if (Math.Abs(BllDev.WYValueX - aimPositionX) > Math.Abs(BllDev.PermitErrX))
                            {
                                //将相对位置换算为目标点位采集值
                                if (BllDev.WYValueX <= aimPositionX)
                                {
                                    aimPositionX += BllDev.CorrXRight;
                                    direction = 1;
                                }
                                else
                                {
                                    aimPositionX -= BllDev.CorrXLeft;
                                    direction = 2;
                                }

                                BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ.AimPositionSB = aimPositionX;
                                double dataTrans = BllDev.AIList[BllDev.WYNOList[0] - 1].GetTransDataFromValueFinal_AI(-aimPositionX);       //X对应传输值
                                if (dataTrans > 4000)
                                    dataTrans = 4000;
                                if (dataTrans < 0)
                                    dataTrans = 0;
                                
                                //生成指令
                                OnceOrderValuesFrmBLL = Enumerable.Repeat((ushort)0, 24).ToArray();
                                OnceOrderValuesFrmBLL[0] = 2;      //位移模式
                                OnceOrderValuesFrmBLL[1] = 500;    //定点模式
                                OnceOrderValuesFrmBLL[13] = direction;                                      //运动方向，1X右，2X左，3Y前，4Y后，5Z上，6Z下
                                OnceOrderValuesFrmBLL[14] = Convert.ToUInt16(dataTrans);                   //X轴定位位置
                                OnceOrderValuesFrmBLL[17] = Convert.ToUInt16(BllDev.WYNOList[0] - 1);      //X位移尺编号
                                Messenger.Default.Send<ushort[]>(OnceOrderValuesFrmBLL, "OnceOrderToComMessage");
                                IsStepOrderSend = true;
                                //到位状态复位
                                StepCompleteStatusBack = false;
                                //等待
                                Thread.Sleep(500);
                            }
                            //目标在误差范围内
                            else
                            {
                                BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsStepCompleted = true;
                                BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ = new MQZH_StepModel_CJBX();
                                IsStepStart = false;
                                IsStepOrderSend = false;
                                return;
                            }
                        }
                        //判断到位状态
                        StepCompleteStatusBack = BllDev.DIList[18].IsOn;
                        if (StepCompleteStatusBack)
                        {
                            BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsStepCompleted = true;
                            BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ = new MQZH_StepModel_CJBX();
                            IsStepStart = false;
                            IsStepOrderSend = false;
                        }
                        return;
                    }
                }

                //结束后提醒
                if (BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsNeedTipsAfter &&
                    (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsAfterComplete))
                {
                    MessageBox.Show(BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StringTipsAfter, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsAfterComplete = true;
                }

                //以上步骤全部完成后，本阶段完成
                BllExp.Exp_CJBX.StageList_GCX[StageNOInList].WYJ_XYFM_SJ = BllExp.Exp_CJBX.StageList_GCX[StageNOInList].WYJMB_XYFM_SJ;
                BllExp.Exp_CJBX.StageList_GCX[StageNOInList].WY_XY_SJ = Math.Abs(BllExp.ExpSettingParam.SJ_CG * 1000) / BllExp.Exp_CJBX.StageList_GCX[StageNOInList].WYJMB_XYFM_SJ;     //实际位移=层高/位移角分母
                BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.CompleteStatus = true;

                //判断是否完成
                if (BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.CompleteStatus)
                {
                    //检测数据保存
                    if (StageNOInList == 1)
                    {
                        BllExp.ExpData_CJBX.AngleFM_GC_X = BllExp.Exp_CJBX.StageList_GCX[StageNOInList].WYJ_XYFM_SJ;
                        BllExp.ExpData_CJBX.DSPL_GC_X = BllExp.Exp_CJBX.StageList_GCX[StageNOInList].WY_XY_SJ;
                        Messenger.Default.Send<string>("SaveCJBXStatusAndData", "SaveExpMessage");
                    }
                    //损坏情况检查确认
                    OpenCJBXDemageGCWin();

                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ = new MQZH_StageModel_CJBX();
                    EscStopCompleteExpReset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "警告", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
            }
        }

        /// <summary>
        /// 层间变形工程Y各阶段试验实施程序
        /// </summary>
        private void CJBX_GCYStageFunc()
        {
            try
            {
                //如果当前阶段是默认阶段，则复位状态
                if ((BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StageNO == "99") || (StageNOInList == -1))
                {
                    MessageBox.Show("默认阶段不能做实验");
                    EscStopCompleteExpReset();
                    return;
                }
                BllDev.IsDeviceBusy = true;

                //开始前提醒
                if (BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsNeedTipsBefore &&
                    (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsBeforeComplete))
                {
                    MessageBox.Show(BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StringTipsBefore, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsBeforeComplete = true;
                }

                //逐个步骤测试
                int stepsCount = BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList.Count;
                for (int i = 0; i < stepsCount; i++)
                {
                    if (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsStepCompleted)
                    {
                        //步骤开始前等待
                        if (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsWaitBeforCompleted)
                        {
                            if (!IsWaitingStart)
                            {
                                WaitingTimeStart = DateTime.Now;
                                IsWaitingStart = true;
                            }
                            else
                            {
                                TimeSpan tempSpan = DateTime.Now - WaitingTimeStart;
                                if (tempSpan.TotalSeconds >= BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].TimeWaitBefor)
                                {
                                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsWaitBeforCompleted = true;
                                    IsWaitingStart = false;
                                }
                            }
                            return;
                        }

                        //标记步骤开始并记录开始时间
                        if (!IsStepStart)
                        {
                            StepStartTime = DateTime.Now;
                            IsStepStart = true;
                        }

                        //发送指令
                        if (!IsStepOrderSend)
                        {
                            BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ = BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i];
                            double aimPositionY = 0;
                            UInt16 direction = 0;

                            double rateHL_CG;
                            if (BllDev.CJBXType == 0)
                                rateHL_CG = BllDev.H_HDK / 1000 / BllExp.ExpSettingParam.SJ_CG;  //连续平行四边形法，比率=活动框高度/试件层高
                            else
                                rateHL_CG = 1;  //层间变形法，比率=1

                            //活动框移动距离=试件目标位置*比率
                            aimPositionY = BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].AimPosition * rateHL_CG;
                            if (Math.Abs(BllDev.WYValueY[3] - aimPositionY) > Math.Abs(BllDev.PermitErrY))
                            {
                                //将相对位置换算为目标点位采集值
                                if (BllDev.WYValueY[3] <= aimPositionY)
                                {
                                    aimPositionY += BllDev.CorrYFront;
                                    direction = 3;
                                }
                                else
                                {
                                    aimPositionY -= BllDev.CorrYBack;
                                    direction = 4;
                                }
                                BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ.AimPositionSB = aimPositionY;

                                double dataTrans1 = BllDev.AIList[BllDev.WYNOList[1] - 1].GetTransDataFromValueFinal_AI(-aimPositionY);       //左点对应传输值
                                double dataTrans2 = BllDev.AIList[BllDev.WYNOList[2] - 1].GetTransDataFromValueFinal_AI(-aimPositionY);       //中间点对应传输值
                                double dataTrans3 = BllDev.AIList[BllDev.WYNOList[3] - 1].GetTransDataFromValueFinal_AI(-aimPositionY);       //右点对应传输值
                                double dataTransAverage = (dataTrans1 + dataTrans2 + dataTrans3) / 3;
                                if (dataTransAverage > 4000)
                                    dataTransAverage = 4000;
                                if (dataTransAverage < 0)
                                    dataTransAverage = 0;

                                //生成指令
                                OnceOrderValuesFrmBLL = Enumerable.Repeat((ushort)0, 24).ToArray();
                                OnceOrderValuesFrmBLL[0] = 2;      //位移模式
                                OnceOrderValuesFrmBLL[1] = 500;    //定点模式
                                OnceOrderValuesFrmBLL[13] = direction;                                     //运动方向，1X右，2X左，3Y前，4Y后，5Z上，6Z下
                                OnceOrderValuesFrmBLL[15] = Convert.ToUInt16(dataTransAverage);            //Y轴定位位置
                                OnceOrderValuesFrmBLL[18] = Convert.ToUInt16(BllDev.WYNOList[1] - 1);     //Y左位移尺编号
                                OnceOrderValuesFrmBLL[19] = Convert.ToUInt16(BllDev.WYNOList[2] - 1);     //Y中位移尺编号
                                OnceOrderValuesFrmBLL[20] = Convert.ToUInt16(BllDev.WYNOList[3] - 1);     //Y右位移尺编号
                                Messenger.Default.Send<ushort[]>(OnceOrderValuesFrmBLL, "OnceOrderToComMessage");
                                IsStepOrderSend = true;
                                //到位状态复位
                                StepCompleteStatusBack = false;
                                //等待
                                Thread.Sleep(500);
                            }
                            //目标在误差范围内
                            else
                            {
                                BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsStepCompleted = true;
                                BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ = new MQZH_StepModel_CJBX();
                                IsStepStart = false;
                                IsStepOrderSend = false;
                                return;
                            }
                        }
                        //判断到位状态
                        StepCompleteStatusBack = BllDev.DIList[19].IsOn;
                        if (StepCompleteStatusBack)
                        {
                            BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsStepCompleted = true;
                            BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ = new MQZH_StepModel_CJBX();
                            IsStepStart = false;
                            IsStepOrderSend = false;
                        }
                        return;
                    }
                }

                //结束后提醒
                if (BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsNeedTipsAfter &&
                    (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsAfterComplete))
                {
                    MessageBox.Show(BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StringTipsAfter, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsAfterComplete = true;
                }

                //以上步骤全部完成后，本阶段完成
                BllExp.Exp_CJBX.StageList_GCY[StageNOInList].WYJ_XYFM_SJ = BllExp.Exp_CJBX.StageList_GCY[StageNOInList].WYJMB_XYFM_SJ;
                BllExp.Exp_CJBX.StageList_GCY[StageNOInList].WY_XY_SJ = Math.Abs(BllExp.ExpSettingParam.SJ_CG * 1000) / BllExp.Exp_CJBX.StageList_GCY[StageNOInList].WYJMB_XYFM_SJ;     //实际位移=层高/位移角分母
                BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.CompleteStatus = true;

                //判断是否完成
                if (BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.CompleteStatus)
                {
                    //检测数据保存
                    if (StageNOInList == 1)
                    {
                        BllExp.ExpData_CJBX.AngleFM_GC_Y = BllExp.Exp_CJBX.StageList_GCY[StageNOInList].WYJ_XYFM_SJ;
                        BllExp.ExpData_CJBX.DSPL_GC_Y = BllExp.Exp_CJBX.StageList_GCY[StageNOInList].WY_XY_SJ;
                        Messenger.Default.Send<string>("SaveCJBXStatusAndData", "SaveExpMessage");
                    }
                    //损坏情况检查确认
                    OpenCJBXDemageGCWin();

                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ = new MQZH_StageModel_CJBX();
                    EscStopCompleteExpReset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "警告", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
            }
        }

        /// <summary>
        /// 层间变形工程Z各阶段试验实施程序
        /// </summary>
        public void CJBX_GCZStageFunc()
        {
            try
            {
                //如果当前阶段是默认阶段，则复位状态
                if ((BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StageNO == "99") || (StageNOInList == -1))
                {
                    MessageBox.Show("默认阶段不能做实验");
                    EscStopCompleteExpReset();
                    return;
                }
                BllDev.IsDeviceBusy = true;

                //开始前提醒
                if (BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsNeedTipsBefore &&
                    (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsBeforeComplete))
                {
                    MessageBox.Show(BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StringTipsBefore, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsBeforeComplete = true;
                }

                //逐个步骤测试
                int stepsCount = BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList.Count;
                for (int i = 0; i < stepsCount; i++)
                {
                    //步骤开始前等待
                    if (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsWaitBeforCompleted)
                    {
                        if (!IsWaitingStart)
                        {
                            WaitingTimeStart = DateTime.Now;
                            IsWaitingStart = true;
                        }
                        TimeSpan tempSpan = DateTime.Now - WaitingTimeStart;
                        if (tempSpan.TotalSeconds >= BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].TimeWaitBefor)
                        {
                            BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsWaitBeforCompleted = true;
                            IsWaitingStart = false;
                            return;
                        }
                    }
                    //本步骤试验
                    if (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsStepCompleted)
                    {
                        //判断步骤是否已经开始
                        if (!IsStepStart)
                        {
                            StepStartTime = DateTime.Now;
                            IsStepStart = true;
                        }
                        TimeSpan tempSpan = DateTime.Now - StepStartTime;

                        //发送指令
                        if (!IsStepOrderSend)
                        {
                            BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ = BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i];
                            double aimPositionZ = 0;
                            UInt16 direction = 0;
                            if (Math.Abs(BllDev.WYValueZ[3] - BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].AimPosition) > Math.Abs(BllDev.PermitErrZ))
                            {
                                //将相对位置换算为目标点位采集值
                                if (BllDev.WYValueZ[3] <= BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].AimPosition)
                                {
                                    aimPositionZ = BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].AimPosition + BllDev.CorrZUp;
                                    direction = 5;
                                }
                                else
                                {
                                    aimPositionZ = BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].AimPosition - BllDev.CorrZDown;
                                    direction = 6;
                                }
                                BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ.AimPositionSB = aimPositionZ;

                                double dataTrans1 = BllDev.AIList[BllDev.WYNOList[4] - 1].GetTransDataFromValueFinal_AI(aimPositionZ);       //左点对应传输值
                                double dataTrans2 = BllDev.AIList[BllDev.WYNOList[5] - 1].GetTransDataFromValueFinal_AI(aimPositionZ);       //中间点对应传输值
                                double dataTrans3 = BllDev.AIList[BllDev.WYNOList[6] - 1].GetTransDataFromValueFinal_AI(aimPositionZ);       //右点对应传输值
                                double dataTransAverage = (dataTrans1 + dataTrans2 + dataTrans3) / 3;
                                if (dataTransAverage > 4000)
                                    dataTransAverage = 4000;
                                if (dataTransAverage < 0)
                                    dataTransAverage = 0;

                                //生成指令
                                OnceOrderValuesFrmBLL = Enumerable.Repeat((ushort)0, 24).ToArray();
                                OnceOrderValuesFrmBLL[0] = 2;      //位移模式
                                OnceOrderValuesFrmBLL[1] = 500;    //定点模式
                                OnceOrderValuesFrmBLL[13] = direction;                                     //运动方向，1X右，2X左，3Y前，4Y后，5Z上，6Z下
                                OnceOrderValuesFrmBLL[16] = Convert.ToUInt16(dataTransAverage);            //Z轴定位位置
                                OnceOrderValuesFrmBLL[21] = Convert.ToUInt16(BllDev.WYNOList[4] - 1);     //Z左位移尺编号
                                OnceOrderValuesFrmBLL[22] = Convert.ToUInt16(BllDev.WYNOList[5] - 1);     //Z中位移尺编号
                                OnceOrderValuesFrmBLL[23] = Convert.ToUInt16(BllDev.WYNOList[6] - 1);     //Z右位移尺编号
                                Messenger.Default.Send<ushort[]>(OnceOrderValuesFrmBLL, "OnceOrderToComMessage");
                                IsStepOrderSend = true;
                                //到位状态复位
                                StepCompleteStatusBack = false;
                                //等待1秒
                                Thread.Sleep(500);
                            }
                            //目标在误差范围内
                            else
                            {
                                BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsStepCompleted = true;
                                BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ = new MQZH_StepModel_CJBX();
                                IsStepStart = false;
                                IsStepOrderSend = false;
                                return;
                            }
                        }
                        //判断到位状态
                        StepCompleteStatusBack = BllDev.DIList[20].IsOn;
                        if (StepCompleteStatusBack)
                        {
                            BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StepList[i].IsStepCompleted = true;
                            BllExp.Exp_CJBX.ExpXM_CJBXStep_DQ = new MQZH_StepModel_CJBX();
                            IsStepStart = false;
                            IsStepOrderSend = false;
                        }
                        return;
                    }
                }

                //结束后提醒
                if (BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsNeedTipsAfter &&
                    (!BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsAfterComplete))
                {
                    MessageBox.Show(BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.StringTipsAfter, "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.IsTipsAfterComplete = true;
                }

                //以上步骤全部完成后，本阶段完成
                BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].WY_Z_SJ = BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].WYMB_Z_SJ;
                BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.CompleteStatus = true;
               
                //判断是否完成
                if (BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ.CompleteStatus)
                {
                    //检测数据保存
                    if (StageNOInList == 1)
                    {
                        BllExp.ExpData_CJBX.DSPL_GC_Z = BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].WY_Z_SJ;
                        Messenger.Default.Send<string>("SaveCJBXStatusAndData", "SaveExpMessage");
                    }
                    //损坏情况检查确认
                    OpenCJBXDemageGCWin();

                    BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ = new MQZH_StageModel_CJBX();
                    EscStopCompleteExpReset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "警告", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
            }
        }

        #endregion
        

        #region 层间变形控制消息

        /// <summary>
        /// 层间变形试验控制消息
        /// </summary>
        /// <param name="msg"></param>
        private void CJBXCtrlMessage(int msg)
        {
            try
            {
                //退出水密试验窗口
                if (msg == 7211)
                {
                    if (ExistCJBXDJXExp || ExistCJBXGCXExp || ExistCJBXDJYExp || ExistCJBXGCYExp || ExistCJBXDJZExp || ExistCJBXGCZExp)
                    {
                        MessageBoxResult msgBoxResult = MessageBox.Show("是否停止正在进行的试验？", "提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                        if (msgBoxResult == MessageBoxResult.Yes)
                        {
                            Messenger.Default.Send<int>(1207, "StopExpMessage");
                            Messenger.Default.Send<string>(MQZH_WinName.CJBXWinName, "CloseGivenNameWin");
                        }
                    }
                    else
                        Messenger.Default.Send<string>(MQZH_WinName.CJBXWinName, "CloseGivenNameWin");
                }
                //定级X轴检测试验消息。
                if (msg == 1107)
                {
                    bool tempExist = false;
                    for (int i = 0; i < 6; i++)
                    {
                        if (BllExp.Exp_CJBX.BeenCheckedDJX[i])
                        {
                            if (!BllExp.Exp_CJBX.StageList_DJX[i].NeedTest)
                            {
                                MessageBox.Show(BllExp.Exp_CJBX.StageList_DJX[i].StageName + "无需检测！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                                BllExp.Exp_CJBX.BeenCheckedDJX[i] = false;
                                return;
                            }
                            if (BllExp.Exp_CJBX.StageList_DJX[i].NeedTest && BllExp.Exp_CJBX.StageList_DJX[i].CompleteStatus)
                            {
                                MessageBoxResult msgBoxResult = MessageBox.Show(BllExp.Exp_CJBX.StageList_DJX[i].StageName + "，将覆盖已完成的数据，是否覆盖？", "数据覆盖提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                                if (msgBoxResult == MessageBoxResult.Yes)
                                {
                                    StageNOInList = i;
                                    switch (i)
                                    {
                                        case 0:
                                            BllExp.Exp_CJBX.CJBXDJStageX0Init();
                                            break;
                                        case 1:
                                            BllExp.Exp_CJBX.CJBXDJStageX1Init();
                                            BllExp.ExpData_CJBX.Init_DJ_X(1);
                                            break;
                                        case 2:
                                            BllExp.Exp_CJBX.CJBXDJStageX2Init();
                                            BllExp.ExpData_CJBX.Init_DJ_X(2);
                                            break;
                                        case 3:
                                            BllExp.Exp_CJBX.CJBXDJStageX3Init();
                                            BllExp.ExpData_CJBX.Init_DJ_X(3);
                                            break;
                                        case 4:
                                            BllExp.Exp_CJBX.CJBXDJStageX4Init();
                                            BllExp.ExpData_CJBX.Init_DJ_X(4);
                                            break;
                                        case 5:
                                            BllExp.Exp_CJBX.CJBXDJStageX5Init();
                                            BllExp.ExpData_CJBX.Init_DJ_X(5);
                                            break;
                                    }
                                    tempExist = true;
                                    break;
                                }
                                if (msgBoxResult == MessageBoxResult.No)
                                {
                                    BllExp.Exp_CJBX.BeenCheckedDJX[i] = false;
                                    return;
                                }
                            }
                            else
                            {
                                StageNOInList = i;
                                switch (i)
                                {
                                    case 0:
                                        BllExp.Exp_CJBX.CJBXDJStageX0Init();
                                        break;
                                    case 1:
                                        BllExp.Exp_CJBX.CJBXDJStageX1Init();
                                        BllExp.ExpData_CJBX.Init_DJ_X(1);
                                        break;
                                    case 2:
                                        BllExp.Exp_CJBX.CJBXDJStageX2Init();
                                        BllExp.ExpData_CJBX.Init_DJ_X(2);
                                        break;
                                    case 3:
                                        BllExp.Exp_CJBX.CJBXDJStageX3Init();
                                        BllExp.ExpData_CJBX.Init_DJ_X(3);
                                        break;
                                    case 4:
                                        BllExp.Exp_CJBX.CJBXDJStageX4Init();
                                        BllExp.ExpData_CJBX.Init_DJ_X(4);
                                        break;
                                    case 5:
                                        BllExp.Exp_CJBX.CJBXDJStageX5Init();
                                        BllExp.ExpData_CJBX.Init_DJ_X(5);
                                        break;
                                }
                                tempExist = true;
                                break;
                            }
                        }
                    }

                    if (tempExist)
                    {
                        //移动目标数据
                        if (StageNOInList > 0)
                        {
                            BllExp.Exp_CJBX.StageList_DJX[StageNOInList].WYJMB_XYFM_SJ = Math.Abs(BllDev.WYJ_Ctl_X[StageNOInList - 1]);
                            double tempWY = Math.Abs(BllExp.ExpSettingParam.SJ_CG * 1000) / BllExp.Exp_CJBX.StageList_DJX[StageNOInList].WYJMB_XYFM_SJ;     //实际位移=层高/位移角分母

                            BllExp.Exp_CJBX.StageList_DJX[StageNOInList].StepList[0].AimPosition = tempWY;
                            BllExp.Exp_CJBX.StageList_DJX[StageNOInList].StepList[4].AimPosition = tempWY;
                            BllExp.Exp_CJBX.StageList_DJX[StageNOInList].StepList[8].AimPosition = tempWY;

                            BllExp.Exp_CJBX.StageList_DJX[StageNOInList].StepList[2].AimPosition = -tempWY;
                            BllExp.Exp_CJBX.StageList_DJX[StageNOInList].StepList[6].AimPosition = -tempWY;
                            BllExp.Exp_CJBX.StageList_DJX[StageNOInList].StepList[10].AimPosition = -tempWY;

                            BllExp.Exp_CJBX.StageList_DJX[StageNOInList].StepList[1].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_DJX[StageNOInList].StepList[3].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_DJX[StageNOInList].StepList[5].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_DJX[StageNOInList].StepList[7].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_DJX[StageNOInList].StepList[9].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_DJX[StageNOInList].StepList[11].AimPosition = 0;
                        }

                        if (StageNOInList == 0)
                        {
                            BllExp.Exp_CJBX.StageList_DJX[StageNOInList].WYJMB_XYFM_SJ = Math.Abs(BllDev.WYJ_Ctl_X[0]) * 2;               //位移角分母
                            double tempWY = Math.Abs(BllExp.ExpSettingParam.SJ_CG * 1000) / BllExp.Exp_CJBX.StageList_DJX[StageNOInList].WYJMB_XYFM_SJ;     //实际位移=层高/位移角分母

                            BllExp.Exp_CJBX.StageList_DJX[StageNOInList].StepList[0].AimPosition = tempWY;

                            BllExp.Exp_CJBX.StageList_DJX[StageNOInList].StepList[2].AimPosition = -tempWY;

                            BllExp.Exp_CJBX.StageList_DJX[StageNOInList].StepList[1].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_DJX[StageNOInList].StepList[3].AimPosition = 0;
                        }

                        BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ = BllExp.Exp_CJBX.StageList_DJX[StageNOInList];
                        //  BllDev.MQZH_DevAIOs.SetWYZeroX_CJ();
                        ExistCJBXDJXExp = tempExist;
                    }
                }

                //定级Y轴检测试验消息。
                if (msg == 1108)
                {
                    bool tempExist = false;
                    for (int i = 0; i < 6; i++)
                    {
                        if (BllExp.Exp_CJBX.BeenCheckedDJY[i])
                        {
                            if (!BllExp.Exp_CJBX.StageList_DJY[i].NeedTest)
                            {
                                MessageBox.Show(BllExp.Exp_CJBX.StageList_DJY[i].StageName + "无需检测！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                                BllExp.Exp_CJBX.BeenCheckedDJY[i] = false;
                                return;
                            }
                            if (BllExp.Exp_CJBX.StageList_DJY[i].NeedTest && BllExp.Exp_CJBX.StageList_DJY[i].CompleteStatus)
                            {
                                MessageBoxResult msgBoxResult = MessageBox.Show(BllExp.Exp_CJBX.StageList_DJY[i].StageName + "，将覆盖已完成的数据，是否覆盖？", "数据覆盖提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                                if (msgBoxResult == MessageBoxResult.Yes)
                                {
                                    StageNOInList = i;
                                    switch (i)
                                    {
                                        case 0:
                                            BllExp.Exp_CJBX.CJBXDJStageY0Init();
                                            break;
                                        case 1:
                                            BllExp.Exp_CJBX.CJBXDJStageY1Init();
                                            BllExp.ExpData_CJBX.Init_DJ_Y(1);
                                            break;
                                        case 2:
                                            BllExp.Exp_CJBX.CJBXDJStageY2Init();
                                            BllExp.ExpData_CJBX.Init_DJ_Y(2);
                                            break;
                                        case 3:
                                            BllExp.Exp_CJBX.CJBXDJStageY3Init();
                                            BllExp.ExpData_CJBX.Init_DJ_Y(3);
                                            break;
                                        case 4:
                                            BllExp.Exp_CJBX.CJBXDJStageY4Init();
                                            BllExp.ExpData_CJBX.Init_DJ_Y(4);
                                            break;
                                        case 5:
                                            BllExp.Exp_CJBX.CJBXDJStageY5Init();
                                            BllExp.ExpData_CJBX.Init_DJ_Y(5);
                                            break;
                                    }
                                    tempExist = true;
                                    break;
                                }
                                if (msgBoxResult == MessageBoxResult.No)
                                {
                                    BllExp.Exp_CJBX.BeenCheckedDJY[i] = false;
                                    return;
                                }
                            }
                            else
                            {
                                StageNOInList = i;
                                switch (i)
                                {
                                    case 0:
                                        BllExp.Exp_CJBX.CJBXDJStageY0Init();
                                        break;
                                    case 1:
                                        BllExp.Exp_CJBX.CJBXDJStageY1Init();
                                        BllExp.ExpData_CJBX.Init_DJ_Y(1);
                                        break;
                                    case 2:
                                        BllExp.Exp_CJBX.CJBXDJStageY2Init();
                                        BllExp.ExpData_CJBX.Init_DJ_Y(2);
                                        break;
                                    case 3:
                                        BllExp.Exp_CJBX.CJBXDJStageY3Init();
                                        BllExp.ExpData_CJBX.Init_DJ_Y(3);
                                        break;
                                    case 4:
                                        BllExp.Exp_CJBX.CJBXDJStageY4Init();
                                        BllExp.ExpData_CJBX.Init_DJ_Y(4);
                                        break;
                                    case 5:
                                        BllExp.Exp_CJBX.CJBXDJStageY5Init();
                                        BllExp.ExpData_CJBX.Init_DJ_Y(5);
                                        break;
                                }
                                tempExist = true;
                                break;
                            }
                        }
                    }

                    if (tempExist)
                    {
                        //移动目标数据
                        if (StageNOInList > 0)
                        {
                            BllExp.Exp_CJBX.StageList_DJY[StageNOInList].WYJMB_XYFM_SJ = Math.Abs(BllDev.WYJ_Ctl_Y[StageNOInList - 1]);
                            double tempWY = Math.Abs(BllExp.ExpSettingParam.SJ_CG * 1000) / BllExp.Exp_CJBX.StageList_DJY[StageNOInList].WYJMB_XYFM_SJ;     //实际位移=层高/位移角分母

                            BllExp.Exp_CJBX.StageList_DJY[StageNOInList].StepList[0].AimPosition = tempWY;
                            BllExp.Exp_CJBX.StageList_DJY[StageNOInList].StepList[4].AimPosition = tempWY;
                            BllExp.Exp_CJBX.StageList_DJY[StageNOInList].StepList[8].AimPosition = tempWY;

                            BllExp.Exp_CJBX.StageList_DJY[StageNOInList].StepList[2].AimPosition = -tempWY;
                            BllExp.Exp_CJBX.StageList_DJY[StageNOInList].StepList[6].AimPosition = -tempWY;
                            BllExp.Exp_CJBX.StageList_DJY[StageNOInList].StepList[10].AimPosition = -tempWY;

                            BllExp.Exp_CJBX.StageList_DJY[StageNOInList].StepList[1].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_DJY[StageNOInList].StepList[3].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_DJY[StageNOInList].StepList[5].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_DJY[StageNOInList].StepList[7].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_DJY[StageNOInList].StepList[9].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_DJY[StageNOInList].StepList[11].AimPosition = 0;
                        }

                        if (StageNOInList == 0)
                        {
                            BllExp.Exp_CJBX.StageList_DJY[StageNOInList].WYJMB_XYFM_SJ = Math.Abs(BllDev.WYJ_Ctl_Y[0]) * 2;               //位移角分母
                            double tempWY = Math.Abs(BllExp.ExpSettingParam.SJ_CG * 1000) / BllExp.Exp_CJBX.StageList_DJY[StageNOInList].WYJMB_XYFM_SJ;     //实际位移=层高/位移角分母

                            BllExp.Exp_CJBX.StageList_DJY[StageNOInList].StepList[0].AimPosition = tempWY;

                            BllExp.Exp_CJBX.StageList_DJY[StageNOInList].StepList[2].AimPosition = -tempWY;

                            BllExp.Exp_CJBX.StageList_DJY[StageNOInList].StepList[1].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_DJY[StageNOInList].StepList[3].AimPosition = 0;
                        }

                        BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ = BllExp.Exp_CJBX.StageList_DJY[StageNOInList];
                        //   BllDev.MQZH_DevAIOs.SetWYZeroY_CJ();
                        ExistCJBXDJYExp = tempExist;
                    }
                }

                //定级Z轴检测试验消息。
                if (msg == 1109)
                {
                    bool tempExist = false;
                    for (int i = 0; i < 6; i++)
                    {
                        if (BllExp.Exp_CJBX.BeenCheckedDJZ[i])
                        {
                            if (!BllExp.Exp_CJBX.StageList_DJZ[i].NeedTest)
                            {
                                MessageBox.Show(BllExp.Exp_CJBX.StageList_DJZ[i].StageName + "无需检测！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                                BllExp.Exp_CJBX.BeenCheckedDJZ[i] = false;
                                return;
                            }
                            if (BllExp.Exp_CJBX.StageList_DJZ[i].NeedTest && BllExp.Exp_CJBX.StageList_DJZ[i].CompleteStatus)
                            {
                                MessageBoxResult msgBoxResult = MessageBox.Show(BllExp.Exp_CJBX.StageList_DJZ[i].StageName + "，将覆盖已完成的数据，是否覆盖？", "数据覆盖提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                                if (msgBoxResult == MessageBoxResult.Yes)
                                {
                                    tempExist = true;
                                    StageNOInList = i;
                                    switch (i)
                                    {
                                        case 0:
                                            BllExp.Exp_CJBX.CJBXDJStageZ0Init();
                                            break;
                                        case 1:
                                            BllExp.Exp_CJBX.CJBXDJStageZ1Init();
                                            BllExp.ExpData_CJBX.Init_DJ_Z(1);
                                            break;
                                        case 2:
                                            BllExp.Exp_CJBX.CJBXDJStageZ2Init();
                                            BllExp.ExpData_CJBX.Init_DJ_Z(2);
                                            break;
                                        case 3:
                                            BllExp.Exp_CJBX.CJBXDJStageZ3Init();
                                            BllExp.ExpData_CJBX.Init_DJ_Z(3);
                                            break;
                                        case 4:
                                            BllExp.Exp_CJBX.CJBXDJStageZ4Init();
                                            BllExp.ExpData_CJBX.Init_DJ_Z(4);
                                            break;
                                        case 5:
                                            BllExp.Exp_CJBX.CJBXDJStageZ5Init();
                                            BllExp.ExpData_CJBX.Init_DJ_Z(5);
                                            break;
                                    }
                                    break;
                                }
                                if (msgBoxResult == MessageBoxResult.No)
                                {
                                    BllExp.Exp_CJBX.BeenCheckedDJZ[i] = false;
                                    return;
                                }
                            }
                            else
                            {
                                StageNOInList = i;
                                switch (i)
                                {
                                    case 0:
                                        BllExp.Exp_CJBX.CJBXDJStageZ0Init();
                                        break;
                                    case 1:
                                        BllExp.Exp_CJBX.CJBXDJStageZ1Init();
                                        BllExp.ExpData_CJBX.Init_DJ_Z(1);
                                        break;
                                    case 2:
                                        BllExp.Exp_CJBX.CJBXDJStageZ2Init();
                                        BllExp.ExpData_CJBX.Init_DJ_Z(2);
                                        break;
                                    case 3:
                                        BllExp.Exp_CJBX.CJBXDJStageZ3Init();
                                        BllExp.ExpData_CJBX.Init_DJ_Z(3);
                                        break;
                                    case 4:
                                        BllExp.Exp_CJBX.CJBXDJStageZ4Init();
                                        BllExp.ExpData_CJBX.Init_DJ_Z(4);
                                        break;
                                    case 5:
                                        BllExp.Exp_CJBX.CJBXDJStageZ5Init();
                                        BllExp.ExpData_CJBX.Init_DJ_Z(5);
                                        break;
                                }
                                tempExist = true;
                                break;
                            }
                        }
                    }
                    if (tempExist)
                    {
                        //移动目标数据
                        if (StageNOInList > 0)
                        {
                            BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].WYMB_Z_SJ = Math.Abs(BllDev.WY_Ctl_Z[StageNOInList - 1]);

                            BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].StepList[0].AimPosition =
                                BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].WYMB_Z_SJ;
                            BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].StepList[4].AimPosition =
                                BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].WYMB_Z_SJ;
                            BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].StepList[8].AimPosition =
                                BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].WYMB_Z_SJ;

                            BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].StepList[2].AimPosition =
                                -BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].WYMB_Z_SJ;
                            BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].StepList[6].AimPosition =
                               -BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].WYMB_Z_SJ;
                            BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].StepList[10].AimPosition =
                               -BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].WYMB_Z_SJ;

                            BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].StepList[1].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].StepList[3].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].StepList[5].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].StepList[7].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].StepList[9].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].StepList[11].AimPosition = 0;
                        }

                        if (StageNOInList == 0)
                        {
                            BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].WYMB_Z_SJ = Math.Abs(BllDev.WY_Ctl_Z[0] / 2);

                            BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].StepList[0].AimPosition =
                                BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].WYMB_Z_SJ;

                            BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].StepList[2].AimPosition =
                               -BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].WYMB_Z_SJ;

                            BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].StepList[1].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_DJZ[StageNOInList].StepList[3].AimPosition = 0;
                        }

                        BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ = BllExp.Exp_CJBX.StageList_DJZ[StageNOInList];
                        //    BllDev.MQZH_DevAIOs.SetWYZeroZ_CJ();
                        ExistCJBXDJZExp = tempExist;
                    }
                }

                //工程X轴检测试验消息。
                if (msg == 1117)
                {
                    bool tempExist = false;
                    for (int i = 0; i < 2; i++)
                    {
                        if (BllExp.Exp_CJBX.BeenCheckedGCX[i])
                        {
                            if (!BllExp.Exp_CJBX.StageList_GCX[i].NeedTest)
                            {
                                MessageBox.Show(BllExp.Exp_CJBX.StageList_GCX[i].StageName + "无需检测！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                                BllExp.Exp_CJBX.BeenCheckedGCX[i] = false;
                                return;
                            }
                            if (BllExp.Exp_CJBX.StageList_GCX[i].NeedTest && BllExp.Exp_CJBX.StageList_GCX[i].CompleteStatus)
                            {
                                MessageBoxResult msgBoxResult = MessageBox.Show(BllExp.Exp_CJBX.StageList_GCX[i].StageName + "，将覆盖已完成的数据，是否覆盖？", "数据覆盖提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                                if (msgBoxResult == MessageBoxResult.Yes)
                                {
                                    StageNOInList = i;
                                    switch (i)
                                    {
                                        case 0:
                                            BllExp.Exp_CJBX.CJBXGCStageX0Init();
                                            break;
                                        case 1:
                                            BllExp.Exp_CJBX.CJBXGCStageX1Init();
                                            BllExp.ExpData_CJBX.Init_GC_X();
                                            break;
                                    }
                                    tempExist = true;
                                    break;
                                }
                                if (msgBoxResult == MessageBoxResult.No)
                                {
                                    BllExp.Exp_CJBX.BeenCheckedGCX[i] = false;
                                    return;
                                }
                            }
                            else
                            {
                                StageNOInList = i;
                                switch (i)
                                {
                                    case 0:
                                        BllExp.Exp_CJBX.CJBXGCStageX0Init();
                                        break;
                                    case 1:
                                        BllExp.Exp_CJBX.CJBXGCStageX1Init();
                                        BllExp.ExpData_CJBX.Init_GC_X();
                                        break;
                                }
                                tempExist = true;
                                break;
                            }
                        }
                    }

                    if (tempExist)
                    {
                        //移动目标数据
                        if (StageNOInList > 0)
                        {
                            BllExp.Exp_CJBX.StageList_GCX[StageNOInList].WYJMB_XYFM_SJ = Math.Abs(BllExp.Exp_CJBX.CJBX_SJZBX);
                            double tempWX = Math.Abs(BllExp.ExpSettingParam.SJ_CG * 1000) / BllExp.Exp_CJBX.StageList_GCX[StageNOInList].WYJMB_XYFM_SJ;     //实际位移=层高/位移角分母

                            BllExp.Exp_CJBX.StageList_GCX[StageNOInList].StepList[0].AimPosition = tempWX;
                            BllExp.Exp_CJBX.StageList_GCX[StageNOInList].StepList[4].AimPosition = tempWX;
                            BllExp.Exp_CJBX.StageList_GCX[StageNOInList].StepList[8].AimPosition = tempWX;

                            BllExp.Exp_CJBX.StageList_GCX[StageNOInList].StepList[2].AimPosition = -tempWX;
                            BllExp.Exp_CJBX.StageList_GCX[StageNOInList].StepList[6].AimPosition = -tempWX;
                            BllExp.Exp_CJBX.StageList_GCX[StageNOInList].StepList[10].AimPosition = -tempWX;

                            BllExp.Exp_CJBX.StageList_GCX[StageNOInList].StepList[1].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_GCX[StageNOInList].StepList[3].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_GCX[StageNOInList].StepList[5].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_GCX[StageNOInList].StepList[7].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_GCX[StageNOInList].StepList[9].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_GCX[StageNOInList].StepList[11].AimPosition = 0;
                        }

                        if (StageNOInList == 0)
                        {
                            BllExp.Exp_CJBX.StageList_GCX[StageNOInList].WYJMB_XYFM_SJ = Math.Abs(BllExp.Exp_CJBX.CJBX_SJZBX) * 2;               //位移角分母
                            double tempWX = Math.Abs(BllExp.ExpSettingParam.SJ_CG * 1000) / BllExp.Exp_CJBX.StageList_GCX[StageNOInList].WYJMB_XYFM_SJ;     //实际位移=层高/位移角分母

                            BllExp.Exp_CJBX.StageList_GCX[StageNOInList].StepList[0].AimPosition = tempWX;

                            BllExp.Exp_CJBX.StageList_GCX[StageNOInList].StepList[2].AimPosition = -tempWX;

                            BllExp.Exp_CJBX.StageList_GCX[StageNOInList].StepList[1].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_GCX[StageNOInList].StepList[3].AimPosition = 0;
                        }

                        BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ = BllExp.Exp_CJBX.StageList_GCX[StageNOInList];
                        //       BllDev.MQZH_DevAIOs.SetWYZeroX_CJ();
                        ExistCJBXGCXExp = tempExist;
                    }
                }

                //工程Y轴检测试验消息。
                if (msg == 1118)
                {
                    bool tempExist = false;
                    for (int i = 0; i < 2; i++)
                    {
                        if (BllExp.Exp_CJBX.BeenCheckedGCY[i])
                        {
                            if (!BllExp.Exp_CJBX.StageList_GCY[i].NeedTest)
                            {
                                MessageBox.Show(BllExp.Exp_CJBX.StageList_GCY[i].StageName + "无需检测！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                                BllExp.Exp_CJBX.BeenCheckedGCY[i] = false;
                                return;
                            }
                            if (BllExp.Exp_CJBX.StageList_GCY[i].NeedTest && BllExp.Exp_CJBX.StageList_GCY[i].CompleteStatus)
                            {
                                MessageBoxResult msgBoxResult = MessageBox.Show(BllExp.Exp_CJBX.StageList_GCY[i].StageName + "，将覆盖已完成的数据，是否覆盖？", "数据覆盖提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                                if (msgBoxResult == MessageBoxResult.Yes)
                                {
                                    StageNOInList = i;
                                    switch (i)
                                    {
                                        case 0:
                                            BllExp.Exp_CJBX.CJBXGCStageY0Init();
                                            break;
                                        case 1:
                                            BllExp.Exp_CJBX.CJBXGCStageY1Init();
                                            BllExp.ExpData_CJBX.Init_GC_Y();
                                            break;
                                    }
                                    tempExist = true;
                                    break;
                                }
                                if (msgBoxResult == MessageBoxResult.No)
                                {
                                    BllExp.Exp_CJBX.BeenCheckedGCY[i] = false;
                                    return;
                                }
                            }
                            else
                            {
                                StageNOInList = i;
                                switch (i)
                                {
                                    case 0:
                                        BllExp.Exp_CJBX.CJBXGCStageY0Init();
                                        break;
                                    case 1:
                                        BllExp.Exp_CJBX.CJBXGCStageY1Init();
                                        BllExp.ExpData_CJBX.Init_GC_Y();
                                        break;
                                }
                                tempExist = true;
                                break;
                            }
                        }
                    }

                    if (tempExist)
                    {
                        //移动目标数据
                        if (StageNOInList > 0)
                        {
                            BllExp.Exp_CJBX.StageList_GCY[StageNOInList].WYJMB_XYFM_SJ = Math.Abs(BllExp.Exp_CJBX.CJBX_SJZBY);
                            double tempWY = Math.Abs(BllExp.ExpSettingParam.SJ_CG * 1000) / BllExp.Exp_CJBX.StageList_GCY[StageNOInList].WYJMB_XYFM_SJ;     //实际位移=层高/位移角分母

                            BllExp.Exp_CJBX.StageList_GCY[StageNOInList].StepList[0].AimPosition = tempWY;
                            BllExp.Exp_CJBX.StageList_GCY[StageNOInList].StepList[4].AimPosition = tempWY;
                            BllExp.Exp_CJBX.StageList_GCY[StageNOInList].StepList[8].AimPosition = tempWY;

                            BllExp.Exp_CJBX.StageList_GCY[StageNOInList].StepList[2].AimPosition = -tempWY;
                            BllExp.Exp_CJBX.StageList_GCY[StageNOInList].StepList[6].AimPosition = -tempWY;
                            BllExp.Exp_CJBX.StageList_GCY[StageNOInList].StepList[10].AimPosition = -tempWY;

                            BllExp.Exp_CJBX.StageList_GCY[StageNOInList].StepList[1].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_GCY[StageNOInList].StepList[3].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_GCY[StageNOInList].StepList[5].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_GCY[StageNOInList].StepList[7].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_GCY[StageNOInList].StepList[9].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_GCY[StageNOInList].StepList[11].AimPosition = 0;
                        }

                        if (StageNOInList == 0)
                        {
                            BllExp.Exp_CJBX.StageList_GCY[StageNOInList].WYJMB_XYFM_SJ = Math.Abs(BllExp.Exp_CJBX.CJBX_SJZBY) * 2;               //位移角分母
                            double tempWY = Math.Abs(BllExp.ExpSettingParam.SJ_CG * 1000) / BllExp.Exp_CJBX.StageList_GCY[StageNOInList].WYJMB_XYFM_SJ;     //实际位移=层高/位移角分母

                            BllExp.Exp_CJBX.StageList_GCY[StageNOInList].StepList[0].AimPosition = tempWY;

                            BllExp.Exp_CJBX.StageList_GCY[StageNOInList].StepList[2].AimPosition = -tempWY;

                            BllExp.Exp_CJBX.StageList_GCY[StageNOInList].StepList[1].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_GCY[StageNOInList].StepList[3].AimPosition = 0;
                        }

                        BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ = BllExp.Exp_CJBX.StageList_GCY[StageNOInList];
                        //    BllDev.MQZH_DevAIOs.SetWYZeroY_CJ();
                        ExistCJBXGCYExp = tempExist;
                    }
                }

                //工程Z轴检测试验消息。
                if (msg == 1119)
                {
                    bool tempExist = false;
                    for (int i = 0; i < 2; i++)
                    {
                        if (BllExp.Exp_CJBX.BeenCheckedGCZ[i])
                        {
                            if (!BllExp.Exp_CJBX.StageList_GCZ[i].NeedTest)
                            {
                                MessageBox.Show(BllExp.Exp_CJBX.StageList_GCZ[i].StageName + "无需检测！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                                BllExp.Exp_CJBX.BeenCheckedGCZ[i] = false;
                                return;
                            }
                            if (BllExp.Exp_CJBX.StageList_GCZ[i].NeedTest && BllExp.Exp_CJBX.StageList_GCZ[i].CompleteStatus)
                            {
                                MessageBoxResult msgBoxResult = MessageBox.Show(BllExp.Exp_CJBX.StageList_GCZ[i].StageName + "，将覆盖已完成的数据，是否覆盖？", "数据覆盖提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                                if (msgBoxResult == MessageBoxResult.Yes)
                                {
                                    tempExist = true;
                                    StageNOInList = i;
                                    switch (i)
                                    {
                                        case 0:
                                            BllExp.Exp_CJBX.CJBXGCStageZ0Init();
                                            break;
                                        case 1:
                                            BllExp.Exp_CJBX.CJBXGCStageZ1Init();
                                            BllExp.ExpData_CJBX.Init_GC_Z();
                                            break;
                                    }
                                    break;
                                }
                                if (msgBoxResult == MessageBoxResult.No)
                                {
                                    BllExp.Exp_CJBX.BeenCheckedGCZ[i] = false;
                                    return;
                                }
                            }
                            else
                            {
                                StageNOInList = i;
                                switch (i)
                                {
                                    case 0:
                                        BllExp.Exp_CJBX.CJBXGCStageZ0Init();
                                        break;
                                    case 1:
                                        BllExp.Exp_CJBX.CJBXGCStageZ1Init();
                                        BllExp.ExpData_CJBX.Init_GC_Z();
                                        break;
                                }
                                tempExist = true;
                                break;
                            }
                        }
                    }
                    if (tempExist)
                    {
                        //移动目标数据
                        if (StageNOInList > 0)
                        {
                            BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].WYMB_Z_SJ = Math.Abs(BllExp.Exp_CJBX.CJBX_SJZBZ);

                            BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].StepList[0].AimPosition = BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].WYMB_Z_SJ;
                            BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].StepList[4].AimPosition = BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].WYMB_Z_SJ;
                            BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].StepList[8].AimPosition = BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].WYMB_Z_SJ;

                            BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].StepList[2].AimPosition = -BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].WYMB_Z_SJ;
                            BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].StepList[6].AimPosition = -BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].WYMB_Z_SJ;
                            BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].StepList[10].AimPosition = -BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].WYMB_Z_SJ;

                            BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].StepList[1].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].StepList[3].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].StepList[5].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].StepList[7].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].StepList[9].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].StepList[11].AimPosition = 0;
                        }

                        if (StageNOInList == 0)
                        {
                            BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].WYMB_Z_SJ = Math.Abs(BllExp.Exp_CJBX.CJBX_SJZBZ / 2);

                            BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].StepList[0].AimPosition = BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].WYMB_Z_SJ;
                            BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].StepList[2].AimPosition = -BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].WYMB_Z_SJ;

                            BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].StepList[1].AimPosition = 0;
                            BllExp.Exp_CJBX.StageList_GCZ[StageNOInList].StepList[3].AimPosition = 0;
                        }

                        BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ = new MQZH_StageModel_CJBX();
                        BllExp.Exp_CJBX.ExpXM_CJBXStage_DQ = BllExp.Exp_CJBX.StageList_GCZ[StageNOInList];
                        //    BllDev.MQZH_DevAIOs.SetWYZeroZ_CJ();
                        ExistCJBXGCZExp = tempExist;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion
        

        #region 状态、指令等参数
        
        /// <summary>
        /// 有层间变形单次指令
        /// </summary>
        private bool _onceOrderCJBX = false;
        /// <summary>
        /// 有层间变形单次指令
        /// </summary>
        public bool OnceOrderCJBX
        {
            get { return _onceOrderCJBX; }
            set
            {
                _onceOrderCJBX = value;
                RaisePropertyChanged(() => OnceOrderCJBX);
            }
        }

        /// <summary>
        /// 有未完成层间变形定级X轴试验
        /// </summary>
        private bool _existCJBXDJXExp = false;
        /// <summary>
        /// 有未完成层间变形定级X轴试验
        /// </summary>
        public bool ExistCJBXDJXExp
        {
            get { return _existCJBXDJXExp; }
            set
            {
                _existCJBXDJXExp = value;
                RaisePropertyChanged(() => ExistCJBXDJXExp);
            }
        }

        /// <summary>
        /// 有未完成层间变形定级Y轴试验
        /// </summary>
        private bool _existCJBXDJYExp = false;
        /// <summary>
        /// 有未完成层间变形定级Y轴试验
        /// </summary>
        public bool ExistCJBXDJYExp
        {
            get { return _existCJBXDJYExp; }
            set
            {
                _existCJBXDJYExp = value;
                RaisePropertyChanged(() => ExistCJBXDJYExp);
            }
        }

        /// <summary>
        /// 有未完成层间变形定级Z轴试验
        /// </summary>
        private bool _existCJBXDJZExp = false;
        /// <summary>
        /// 有未完成层间变形定级Z轴试验
        /// </summary>
        public bool ExistCJBXDJZExp
        {
            get { return _existCJBXDJZExp; }
            set
            {
                _existCJBXDJZExp = value;
                RaisePropertyChanged(() => ExistCJBXDJZExp);
            }
        }

        /// <summary>
        /// 有未完成层间变形工程X轴试验
        /// </summary>
        private bool _existCJBXGCXExp = false;
        /// <summary>
        /// 有未完成层间变形工程X轴试验
        /// </summary>
        public bool ExistCJBXGCXExp
        {
            get { return _existCJBXGCXExp; }
            set
            {
                _existCJBXGCXExp = value;
                RaisePropertyChanged(() => ExistCJBXGCXExp);
            }
        }

        /// <summary>
        /// 有未完成层间变形工程Y轴试验
        /// </summary>
        private bool _existCJBXGCYExp = false;
        /// <summary>
        /// 有未完成层间变形工程Y轴试验
        /// </summary>
        public bool ExistCJBXGCYExp
        {
            get { return _existCJBXGCYExp; }
            set
            {
                _existCJBXGCYExp = value;
                RaisePropertyChanged(() => ExistCJBXGCYExp);
            }
        }

        /// <summary>
        /// 有未完成层间变形工程Z轴试验
        /// </summary>
        private bool _existCJBXGCZExp = false;
        /// <summary>
        /// 有未完成层间变形工程Z轴试验
        /// </summary>
        public bool ExistCJBXGCZExp
        {
            get { return _existCJBXGCZExp; }
            set
            {
                _existCJBXGCZExp = value;
                RaisePropertyChanged(() => ExistCJBXGCZExp);
            }
        }

        #endregion
    }
}