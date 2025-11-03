using GalaSoft.MvvmLight.Messaging;
using MQDFJ_MB.View.ExpViews;
using MQDFJ_MB.View.Others;
using MQDFJ_MB.View.RepAndData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using MQDFJ_MB.Model;
using MQDFJ_MB.View.DebugPlot;
using MQDFJ_MB.View.DTFY;


namespace MQDFJ_MB.View
{

    public partial class MQZH_MainView : Window
    {
        public MQZH_MainView()
        {
            InitializeComponent();

            //卸载当前(this)对象注册的所有MVVMLight消息
            this.Unloaded += (sender, e) => Messenger.Default.Unregister(this);

            //注册窗口关闭、新增处理方法
            Messenger.Default.Register<Window>(this, "NewWindow", WindowAddedMessage);
            Messenger.Default.Register<string>(this, "WindowClosed", WindowClosedMessage);
            Messenger.Default.Register<string>(this, "OpenGivenNameWin", OpenWinMessage);
            Messenger.Default.Register<string>(this, "CloseGivenNameWin", CloseGivenNameWinMessage);
        }


        #region 窗口属性，新增、关闭窗口消息处理

        /// <summary>
        /// 窗口列表
        /// </summary>
        private List<Window> _childWindowList = new List<Window>();
        /// <summary>
        /// 窗口列表
        /// </summary>
        private List<Window> ChildWindowList
        {
            get { return _childWindowList; }
            set { _childWindowList = value; }
        }
        //提示弹窗
        public Prompt PromptWin;
        
        //调试曲线窗口
        public MQZH_SDQMPlotView MQZH_SDQMPlotWin;
        public MQZH_SDSMPlotView MQZH_SDSMPlotWin;
        public MQZH_SDWYPlotView MQZH_SDWYPlotWin;
        public MQZH_SLLPIDPlotView MQZH_SLLPIDPlotWin;
        //试验窗口
        public MQZH_ExpQMView MQZH_QMWin;
        public MQZH_ExpSMView MQZH_SMWin;
        public MQZH_ExpKFYp1View MQZH_KFYp1Win;
        public MQZH_ExpKFYp2View MQZH_KFYp2Win;
        public MQZH_ExpKFYp3View MQZH_KFYp3Win;
        public MQZH_ExpKFYpmaxView MQZH_KFYpmaxWin;
        public MQZH_ExpCJBXView MQZH_CJBXWin;
        public MQZH_DTFYView MQZH_DTFYCaliWin;
        public MQZH_DTFYView MQZH_DTFYWin;
        //试验管理、设定
        public MQZH_ExpManagerView MQZH_ExpManWin;
        public MQZH_ExpSettingView MQZH_ExpSetWin;
        //试验数据、评定
        public MQZH_DamageView_SMDJ MQZH_SM_DJDamageWin;
        public MQZH_DamageView_SMGC MQZH_SM_GCDamageWin;
        public MQZH_DamageView_KFYDJ MQZH_KFY_DJDamageWin;
        public MQZH_DamageView_KFYGC MQZH_KFY_GCDamageWin;
        public MQZH_DamageView_CJBXDJ MQZH_CJBX_DJDamWin;
        public MQZH_DamageView_CJBXGC MQZH_CJBX_GCDamWin;
        public MQZH_CurrentDataView MQZH_CurDataWin;
        //装置设定
        public MQZH_CompanyInfoSetView MQZH_CoInfoSetWin;
        public MQZH_ComSetting MQZH_ComSetWin;
        public MQZH_DevInfoSetView MQZH_DevInfoSetWin;
        public MQZH_DevParamSetView MQZH_DevParamSetWin;
        public MQZH_OtherSetView MQZH_OtherSetWin;
        public MQZH_PidSetView MQZH_PidSetWin;
        public MQZH_PressSetView MQZH_PressSetWin;
        public MQZH_PressCtrlView MQZH_PressCtlWin;
        public MQZH_SenssorSetView MQZH_SessorSetWin;
        public MQZH_WYSetView MQZH_WYSetWin;
        public MQZH_WYCtlView MQZH_WYCtlWin;
        public MQZH_DTFYParamView MQZH_DTFYParamWin;
        //传感器调零校正
        public MQZH_CorrectView MQZH_CorrWin;
        //调试
        public MQZH_DebugView MQZH_DbgWin;

        /// <summary>
        /// 窗口已关闭消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void WindowClosedMessage(string msg)
        {
            for (int i = 0; i < ChildWindowList.Count; i++)
            {
                if (ChildWindowList[i].Name == msg)
                {
                    ChildWindowList.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// 窗口已新增消息处理
        /// </summary>
        /// <param name="msgWindow"></param>
        private void WindowAddedMessage(Window msgWindow)
        {
            if (!ChildWindowList.Exists(o => o.Name == msgWindow.Name))
            {
                ChildWindowList.Add(msgWindow);
            }
        }

        /// <summary>
        /// 打开指定窗口消息
        /// </summary>
        /// <param name="msg"></param>
        private void OpenWinMessage(string msg)
        {
            //提示弹窗
            if (msg == MQZH_WinName.PromptWinName)
                PromptWin = WindowsManager<Prompt>.Show(new object(), MQZH_WinName.PromptWinName, this);
            //调试数据曲线窗口
            else if (msg == MQZH_WinName.SDQMPlotWinName)
                MQZH_SDQMPlotWin = WindowsManager<MQZH_SDQMPlotView>.Show(new object(), MQZH_WinName.SDQMPlotWinName, this);
            else if (msg == MQZH_WinName.SDSMPlotWinName)
                MQZH_SDSMPlotWin = WindowsManager<MQZH_SDSMPlotView>.Show(new object(), MQZH_WinName.SDSMPlotWinName, this);
            else if (msg == MQZH_WinName.SDWYPlotWinName)
                MQZH_SDWYPlotWin = WindowsManager<MQZH_SDWYPlotView>.Show(new object(), MQZH_WinName.SDSMPlotWinName, this);
            else if (msg == MQZH_WinName.SLLPIDPlotWinName)
                MQZH_SLLPIDPlotWin = WindowsManager<MQZH_SLLPIDPlotView>.Show(new object(), MQZH_WinName.SLLPIDPlotWinName, this);
            //调试窗口
            else if (msg == MQZH_WinName.DbgWinName)
                MQZH_DbgWin = WindowsManager<MQZH_DebugView>.Show(new object(), MQZH_WinName.DbgWinName, this);

            //试验窗口
            else if (msg == MQZH_WinName.QMWinName)
                MQZH_QMWin = WindowsManager<MQZH_ExpQMView>.Show(new object(), MQZH_WinName.QMWinName, this);
            else if (msg == MQZH_WinName.SMWinName)
                MQZH_SMWin = WindowsManager<MQZH_ExpSMView>.Show(new object(), MQZH_WinName.SMWinName, this);
            else if (msg == MQZH_WinName.KFYp1Name)
                MQZH_KFYp1Win = WindowsManager<MQZH_ExpKFYp1View>.Show(new object(), MQZH_WinName.KFYp1Name, this);
            else if (msg == MQZH_WinName.KFYp2Name)
                MQZH_KFYp2Win = WindowsManager<MQZH_ExpKFYp2View>.Show(new object(), MQZH_WinName.KFYp2Name, this);
            else if (msg == MQZH_WinName.KFYp3Name)
                MQZH_KFYp3Win = WindowsManager<MQZH_ExpKFYp3View>.Show(new object(), MQZH_WinName.KFYp3Name, this);
            else if (msg == MQZH_WinName.KFYpmaxName)
                MQZH_KFYpmaxWin = WindowsManager<MQZH_ExpKFYpmaxView>.Show(new object(), MQZH_WinName.KFYpmaxName, this);
            else if (msg == MQZH_WinName.CJBXWinName)
                MQZH_CJBXWin = WindowsManager<MQZH_ExpCJBXView>.Show(new object(), MQZH_WinName.CJBXWinName, this);
            else if (msg == MQZH_WinName.DTFYWinName)
                MQZH_DTFYWin = WindowsManager<MQZH_DTFYView>.Show(new object(), MQZH_WinName.DTFYWinName, this);

            //试验管理、设定
            else if (msg == MQZH_WinName.ExpManWinName)
                MQZH_ExpManWin = WindowsManager<MQZH_ExpManagerView>.Show(new object(), MQZH_WinName.ExpManWinName, this);
            else if (msg == MQZH_WinName.ExpSetWinName)
                MQZH_ExpSetWin = WindowsManager<MQZH_ExpSettingView>.Show(new object(), MQZH_WinName.ExpSetWinName, this);

            //试验数据、评定
            else if (msg == MQZH_WinName.CurrentDataWinName)
                MQZH_CurDataWin = WindowsManager<MQZH_CurrentDataView>.Show(new object(), MQZH_WinName.CurrentDataWinName, this);
            else if (msg == MQZH_WinName.SMDJDamageWinName)
                MQZH_SM_DJDamageWin =
                    WindowsManager<MQZH_DamageView_SMDJ>.Show(new object(), MQZH_WinName.SMDJDamageWinName, this);
            else if (msg == MQZH_WinName.SMGCDamageWinName)
                MQZH_SM_GCDamageWin =
                    WindowsManager<MQZH_DamageView_SMGC>.Show(new object(), MQZH_WinName.SMGCDamageWinName, this);
            else if (msg == MQZH_WinName.KFYDJDamageWinName)
                MQZH_KFY_DJDamageWin =
                    WindowsManager<MQZH_DamageView_KFYDJ>.Show(new object(), MQZH_WinName.KFYDJDamageWinName, this);
            else if (msg == MQZH_WinName.KFYGCDamageWinName)
                MQZH_KFY_GCDamageWin = WindowsManager<MQZH_DamageView_KFYGC>.Show(new object(), MQZH_WinName.KFYGCDamageWinName, this);
            else if (msg == MQZH_WinName.CJBXDJDamageWinName)
                MQZH_CJBX_DJDamWin = WindowsManager<MQZH_DamageView_CJBXDJ>.Show(new object(), MQZH_WinName.CJBXDJDamageWinName, this);
            else if (msg == MQZH_WinName.CJBXGCDamageWinName)
                MQZH_CJBX_GCDamWin = WindowsManager<MQZH_DamageView_CJBXGC>.Show(new object(), MQZH_WinName.CJBXGCDamageWinName, this);

            //装置设定
            else if (msg == MQZH_WinName.CoInfoSetWinName)
                MQZH_CoInfoSetWin = WindowsManager<MQZH_CompanyInfoSetView>.Show(new object(), MQZH_WinName.CoInfoSetWinName, this);
            else if (msg == MQZH_WinName.ComSetWinName)
                MQZH_ComSetWin = WindowsManager<MQZH_ComSetting>.Show(new object(), MQZH_WinName.ComSetWinName, this);
            else if (msg == MQZH_WinName.DevInfoSetWinName)
                MQZH_DevInfoSetWin = WindowsManager<MQZH_DevInfoSetView>.Show(new object(), MQZH_WinName.DevInfoSetWinName, this);
            else if (msg == MQZH_WinName.DevParamSetWinName)
                MQZH_DevParamSetWin = WindowsManager<MQZH_DevParamSetView>.Show(new object(), MQZH_WinName.DevParamSetWinName, this);
            else if (msg == MQZH_WinName.OtherSetWinName)
                MQZH_OtherSetWin = WindowsManager<MQZH_OtherSetView>.Show(new object(), MQZH_WinName.OtherSetWinName, this);
            else if (msg == MQZH_WinName.PidSetWinName)
                MQZH_PidSetWin = WindowsManager<MQZH_PidSetView>.Show(new object(), MQZH_WinName.PidSetWinName, this);
            else if (msg == MQZH_WinName.PressSetWinName)
                MQZH_PressSetWin = WindowsManager<MQZH_PressSetView>.Show(new object(), MQZH_WinName.PressSetWinName, this);
            else if (msg == MQZH_WinName.PressCtlWinName)
                MQZH_PressCtlWin = WindowsManager<MQZH_PressCtrlView>.Show(new object(), MQZH_WinName.PressCtlWinName, this);
            if (msg == MQZH_WinName.SenssorSetWinName)
                MQZH_SessorSetWin = WindowsManager<MQZH_SenssorSetView>.Show(new object(), MQZH_WinName.SenssorSetWinName, this);
            else if (msg == MQZH_WinName.WYSetWinName)
                MQZH_WYSetWin = WindowsManager<MQZH_WYSetView>.Show(new object(), MQZH_WinName.WYSetWinName, this);
            else if (msg == MQZH_WinName.WYCtlWinName)
                MQZH_WYCtlWin = WindowsManager<MQZH_WYCtlView>.Show(new object(), MQZH_WinName.WYCtlWinName, this);
            else if (msg == MQZH_WinName.DTFYParamWinName)
                MQZH_DTFYParamWin = WindowsManager<MQZH_DTFYParamView>.Show(new object(), MQZH_WinName.DTFYParamWinName, this);
            //调零标定
            else if (msg == MQZH_WinName.CorrWinName)
                MQZH_CorrWin = WindowsManager<MQZH_CorrectView>.Show(new object(), MQZH_WinName.CorrWinName, this);

        }

        /// <summary>
        /// 关闭指定窗口消息
        /// </summary>
        /// <param name="msg"></param>
        private void CloseGivenNameWinMessage(string msg)
        {
            //提示弹窗
            if (msg == MQZH_WinName.PromptWinName)
            {
                if (PromptWin != null)
                {
                    PromptWin.Close();
                    PromptWin = null;
                }
            }
            //调试数据曲线
            if (msg == MQZH_WinName.SDQMPlotWinName)
            {
                if (MQZH_SDQMPlotWin != null)
                {
                    MQZH_SDQMPlotWin.Close();
                    MQZH_SDQMPlotWin = null;
                }
            }
            if (msg == MQZH_WinName.SDSMPlotWinName)
            {
                if (MQZH_SDSMPlotWin != null)
                {
                    MQZH_SDSMPlotWin.Close();
                    MQZH_SDSMPlotWin = null;
                }
            }
            if (msg == MQZH_WinName.SDWYPlotWinName)
            {
                if (MQZH_SDWYPlotWin != null)
                {
                    MQZH_SDWYPlotWin.Close();
                    MQZH_SDWYPlotWin = null;
                }
            }
            if (msg == MQZH_WinName.SLLPIDPlotWinName)
            {
                if (MQZH_SLLPIDPlotWin != null)
                {
                    MQZH_SLLPIDPlotWin.Close();
                    MQZH_SLLPIDPlotWin = null;
                }
            }
            //调试
            if (msg == MQZH_WinName.DbgWinName)
            {
                if (MQZH_DbgWin != null)
                {
                    MQZH_DbgWin.Close();
                    MQZH_DbgWin = null;
                }
            }

            //试验
            if (msg == MQZH_WinName.QMWinName)
            {
                if (MQZH_QMWin != null)
                {
                    MQZH_QMWin.Close();
                    MQZH_QMWin = null;
                }
            }
            if (msg == MQZH_WinName.SMWinName)
            {
                if (MQZH_SMWin != null)
                {
                    MQZH_SMWin.Close();
                    MQZH_SMWin = null;
                }
            }
            if (msg == MQZH_WinName.KFYp1Name)
            {
                if (MQZH_KFYp1Win != null)
                {
                    MQZH_KFYp1Win.Close();
                    MQZH_KFYp1Win = null;
                }
            }
            if (msg == MQZH_WinName.KFYp2Name)
            {
                if (MQZH_KFYp2Win != null)
                {
                    MQZH_KFYp2Win.Close();
                    MQZH_KFYp2Win = null;
                }
            }
            if (msg == MQZH_WinName.KFYp3Name)
            {
                if (MQZH_KFYp3Win != null)
                {
                    MQZH_KFYp3Win.Close();
                    MQZH_KFYp3Win = null;
                }
            }
            if (msg == MQZH_WinName.KFYpmaxName)
            {
                if (MQZH_KFYpmaxWin != null)
                {
                    MQZH_KFYpmaxWin.Close();
                    MQZH_KFYpmaxWin = null;
                }
            }
            if (msg == MQZH_WinName.CJBXWinName)
            {
                if (MQZH_CJBXWin != null)
                {
                    MQZH_CJBXWin.Close();
                    MQZH_CJBXWin = null;
                }
            }
            if (msg == MQZH_WinName.DTFYWinName)
            {
                if (MQZH_DTFYWin != null)
                {
                    MQZH_DTFYWin.Close();
                    MQZH_DTFYWin = null;
                }
            }
            if (msg == MQZH_WinName.DTFYCaliWinName)
            {
                if (MQZH_DTFYCaliWin != null)
                {
                    MQZH_DTFYCaliWin.Close();
                    MQZH_DTFYCaliWin = null;
                }
            }

            //关闭试验管理、设定
            if (msg == MQZH_WinName.ExpManWinName)
            {
                if (MQZH_ExpManWin != null)
                {
                    MQZH_ExpManWin.Close();
                    MQZH_ExpManWin = null;
                }
            }
            if (msg == MQZH_WinName.ExpSetWinName)
            {
                if (MQZH_ExpSetWin != null)
                {
                    MQZH_ExpSetWin.Close();
                    MQZH_ExpSetWin = null;
                }
            }

            //试验数据、评定
            if (msg == MQZH_WinName.CurrentDataWinName)
            {
                if (MQZH_CurDataWin != null)
                {
                    MQZH_CurDataWin.Close();
                    MQZH_CurDataWin = null;
                }
            }
            if (msg == MQZH_WinName.SMDJDamageWinName)
            {
                if (MQZH_SM_DJDamageWin != null)
                {
                    MQZH_SM_DJDamageWin.Close();
                    MQZH_SM_DJDamageWin = null;
                }
            }
            if (msg == MQZH_WinName.SMGCDamageWinName)
            {
                if (MQZH_SM_GCDamageWin != null)
                {
                    MQZH_SM_GCDamageWin.Close();
                    MQZH_SM_GCDamageWin = null;
                }
            }
            if (msg == MQZH_WinName.KFYDJDamageWinName)
            {
                if (MQZH_KFY_DJDamageWin != null)
                {
                    MQZH_KFY_DJDamageWin.Close();
                    MQZH_KFY_DJDamageWin = null;
                }
            }
            if (msg == MQZH_WinName.KFYGCDamageWinName)
            {
                if (MQZH_KFY_GCDamageWin != null)
                {
                    MQZH_KFY_GCDamageWin.Close();
                    MQZH_KFY_GCDamageWin = null;
                }
            }
            if (msg == MQZH_WinName.CJBXDJDamageWinName)
            {
                if (MQZH_CJBX_DJDamWin != null)
                {
                    MQZH_CJBX_DJDamWin.Close();
                    MQZH_CJBX_DJDamWin = null;
                }
            }
            if (msg == MQZH_WinName.CJBXGCDamageWinName)
            {
                if (MQZH_CJBX_GCDamWin != null)
                {
                    MQZH_CJBX_GCDamWin.Close();
                    MQZH_CJBX_GCDamWin = null;
                }
            }
            //装置设定
            if (msg == MQZH_WinName.DevInfoSetWinName)
            {
                if (MQZH_DevInfoSetWin != null)
                {
                    MQZH_DevInfoSetWin.Close();
                    MQZH_DevInfoSetWin = null;
                }
            }
            if (msg == MQZH_WinName.DevParamSetWinName)
            {
                if (MQZH_DevParamSetWin != null)
                {
                    MQZH_DevParamSetWin.Close();
                    MQZH_DevParamSetWin = null;
                }
            }
            if (msg == MQZH_WinName.CoInfoSetWinName)
            {
                if (MQZH_CoInfoSetWin != null)
                {
                    MQZH_CoInfoSetWin.Close();
                    MQZH_CoInfoSetWin = null;
                }
            }
            if (msg == MQZH_WinName.PidSetWinName)
            {
                if (MQZH_PidSetWin != null)
                {
                    MQZH_PidSetWin.Close();
                    MQZH_PidSetWin = null;
                }
            }
            if (msg == MQZH_WinName.PressSetWinName)
            {
                if (MQZH_PressSetWin != null)
                {
                    MQZH_PressSetWin.Close();
                    MQZH_PressSetWin = null;
                }
            }
            if (msg == MQZH_WinName.PressCtlWinName)
            {
                if (MQZH_PressCtlWin != null)
                {
                    MQZH_PressCtlWin.Close();
                    MQZH_PressCtlWin = null;
                }
            }
            if (msg == MQZH_WinName.ComSetWinName)
            {
                if (MQZH_ComSetWin != null)
                {
                    MQZH_ComSetWin.Close();
                    MQZH_ComSetWin = null;
                }
            }
            if (msg == MQZH_WinName.SenssorSetWinName)
            {
                if (MQZH_SessorSetWin != null)
                {
                    MQZH_SessorSetWin.Close();
                    MQZH_SessorSetWin = null;
                }
            }
            if (msg == MQZH_WinName.ComSetWinName)
            {
                if (MQZH_ComSetWin != null)
                {
                    MQZH_ComSetWin.Close();
                    MQZH_ComSetWin = null;
                }
            }
            if (msg == MQZH_WinName.OtherSetWinName)
            {
                if (MQZH_OtherSetWin != null)
                {
                    MQZH_OtherSetWin.Close();
                    MQZH_OtherSetWin = null;
                }
            }
            if (msg == MQZH_WinName.WYSetWinName)
            {
                if (MQZH_WYSetWin != null)
                {
                    MQZH_WYSetWin.Close();
                    MQZH_WYSetWin = null;
                }
            }
            if (msg == MQZH_WinName.WYCtlWinName)
            {
                if (MQZH_WYCtlWin != null)
                {
                    MQZH_WYCtlWin.Close();
                    MQZH_WYCtlWin = null;
                }
            }
            if (msg == MQZH_WinName.DTFYParamWinName)
            {
                if (MQZH_DTFYParamWin != null)
                {
                    MQZH_DTFYParamWin.Close();
                    MQZH_DTFYParamWin = null;
                }
            }
        }

        #endregion


        #region 按钮控制


        /// <summary>
        /// 退出
        /// </summary>
        private void Button_Quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion
        

        #region 关闭窗口事件处理

        /// <summary>
        /// 关闭窗口前检查子窗口是否全部关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (ChildWindowList.Count == 0)
                {
                    CloseChildWindows();
                    Messenger.Default.Send<string>("Quit", "MQZH_Quit");
                }
                else
                {
                    MessageBoxResult msgBoxResult = MessageBox.Show("有子窗口未关闭，强行退出可能会丢失数据。继续退出程序?", "关闭子窗口提示", MessageBoxButton.YesNo);
                    if (msgBoxResult == MessageBoxResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        Messenger.Default.Send<string>("Quit", "MQZH_Quit");
                    }
                }
            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.ToString());
            }

        }

        /// <summary>
        /// 强行关闭子窗口
        /// </summary>
        private void CloseChildWindows()
        {

            if (PromptWin != null)
            {
                PromptWin.Close();
                PromptWin = null;
            }

            if (MQZH_CJBXWin != null)
            {
                MQZH_CJBXWin.Close();
                MQZH_CJBXWin = null;
            }
            if (MQZH_ExpManWin != null)
            {
                MQZH_ExpManWin.Close();
                MQZH_ExpManWin = null;
            }

            if (MQZH_QMWin != null)
            {
                MQZH_QMWin.Close();
                MQZH_QMWin = null;
            }
            if (MQZH_ExpSetWin != null)
            {
                MQZH_ExpSetWin.Close();
                MQZH_ExpSetWin = null;
            }
            if (MQZH_SMWin != null)
            {
                MQZH_SMWin.Close();
                MQZH_SMWin = null;
            }
            if (MQZH_KFYp1Win != null)
            {
                MQZH_KFYp1Win.Close();
                MQZH_KFYp1Win = null;
            }
            if (MQZH_KFYp2Win != null)
            {
                MQZH_KFYp2Win.Close();
                MQZH_KFYp2Win = null;
            }
            if (MQZH_KFYp3Win != null)
            {
                MQZH_KFYp3Win.Close();
                MQZH_KFYp3Win = null;
            }
            if (MQZH_KFYpmaxWin != null)
            {
                MQZH_KFYpmaxWin.Close();
                MQZH_KFYpmaxWin = null;
            }
            if (MQZH_CoInfoSetWin != null)
            {
                MQZH_CoInfoSetWin.Close();
                MQZH_CoInfoSetWin = null;
            }
            if (MQZH_DevInfoSetWin != null)
            {
                MQZH_DevInfoSetWin.Close();
                MQZH_DevInfoSetWin = null;
            }
            if (MQZH_PidSetWin != null)
            {
                MQZH_PidSetWin.Close();
                MQZH_PidSetWin = null;
            }
            if (MQZH_PressSetWin != null)
            {
                MQZH_PressSetWin.Close();
                MQZH_PressSetWin = null;
            }
            if (MQZH_PressCtlWin != null)
            {
                MQZH_PressCtlWin.Close();
                MQZH_PressCtlWin = null;
            }
            if (MQZH_WYCtlWin != null)
            {
                MQZH_WYCtlWin.Close();
                MQZH_WYCtlWin = null;
            }
            if (MQZH_WYSetWin != null)
            {
                MQZH_WYSetWin.Close();
                MQZH_WYSetWin = null;
            }
            if (MQZH_ComSetWin != null)
            {
                MQZH_ComSetWin.Close();
                MQZH_ComSetWin = null;
            }
            if (MQZH_CorrWin != null)
            {
                MQZH_CorrWin.Close();
                MQZH_CorrWin = null;
            }
            if (MQZH_DbgWin != null)
            {
                MQZH_DbgWin.Close();
                MQZH_DbgWin = null;
            }
            if (MQZH_SLLPIDPlotWin != null)
            {
                MQZH_SLLPIDPlotWin.Close();
                MQZH_SLLPIDPlotWin = null;
            }
            if (MQZH_SDQMPlotWin != null)
            {
                MQZH_SDQMPlotWin.Close();
                MQZH_SDQMPlotWin = null;
            }
            if (MQZH_SDSMPlotWin != null)
            {
                MQZH_SDSMPlotWin.Close();
                MQZH_SDSMPlotWin = null;
            }
            if (MQZH_SDWYPlotWin != null)
            {
                MQZH_SDWYPlotWin.Close();
                MQZH_SDWYPlotWin = null;
            }
            if (MQZH_DTFYParamWin != null)
            {
                MQZH_DTFYParamWin.Close();
                MQZH_DTFYParamWin = null;
            }
            if (MQZH_DTFYCaliWin != null)
            {
                MQZH_DTFYCaliWin.Close();
                MQZH_DTFYCaliWin = null;
            }
            if (MQZH_DTFYWin != null)
            {
                MQZH_DTFYWin.Close();
                MQZH_DTFYWin = null;
            }
        }

        #endregion

    }
}