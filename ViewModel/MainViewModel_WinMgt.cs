using GalaSoft.MvvmLight;
using MQDFJ_MB.Communication;
using MQDFJ_MB.Model.Exp;
using System.Windows;
using MQDFJ_MB.BLL;
using GalaSoft.MvvmLight.Messaging;
using System;
using GalaSoft.MvvmLight.Command;
using MQDFJ_MB.Model;
using System.Collections.Generic;
using ChartModel;
using System.Drawing;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System.Linq;
using static MQDFJ_MB.Model.MQZH_Enums;
using System.ComponentModel;
using MQDFJ_MB.Model.Exp_MB;
using MQDFJ_MB.DAL.Dev;
using MQDFJ_MB.DAL.Exp;
using MQDFJ_MB.DAL.Rep;


namespace MQDFJ_MB.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {


        #region 主试验窗口管理相关指令

        /// <summary>
        /// 窗口管理指令
        /// </summary>
        private RelayCommand<String> _winManCommand;
        /// <summary>
        /// 窗口管理指令
        /// </summary>
        public RelayCommand<String> WinManCommand
        {
            get
            {
                if (_winManCommand == null)
                    _winManCommand = new RelayCommand<String>((p) => ExecuteWinManCommand(p));
                return _winManCommand;
            }
            set { _winManCommand = value; }
        }


        /// <summary>
        /// 试验管理指令回调
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecuteWinManCommand(String num)
        {
            int i = Convert.ToInt16(num);
            //试验管理窗口
            if (i == 7103)
            {
                Messenger.Default.Send<string>(MQZH_WinName.ExpManWinName, "OpenGivenNameWin");
            }

            //试验参数窗口
            else if (i == 7102)
            {
                Messenger.Default.Send<string>(MQZH_WinName.ExpSetWinName, "OpenGivenNameWin");
            }

            //数据报告窗口
            else if (i == 7104)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CurrentDataWinName, "OpenGivenNameWin");
            }

            //气密检测窗口
            else if (i == 7105)
            {
                Messenger.Default.Send<string>(MQZH_WinName.QMWinName, "OpenGivenNameWin");
            }

            //抗风压P1检测窗口
            else if (i == 7106)
            {
                Messenger.Default.Send<string>(MQZH_WinName.KFYp1Name, "OpenGivenNameWin");
            }

            //水密检测窗口
            else if (i == 7107)
            {
                Messenger.Default.Send<string>(MQZH_WinName.SMWinName, "OpenGivenNameWin");
            }

            //抗风压P2检测窗口
            else if (i == 7108)
            {
                Messenger.Default.Send<string>(MQZH_WinName.KFYp2Name, "OpenGivenNameWin");
            }

            //抗风压P3检测窗口
            else if (i == 7109)
            {
                Messenger.Default.Send<string>(MQZH_WinName.KFYp3Name, "OpenGivenNameWin");
            }


            //抗风压Pmax检测窗口
            else if (i == 7110)
            {
                Messenger.Default.Send<string>(MQZH_WinName.KFYpmaxName, "OpenGivenNameWin");
            }


            //层间变形检测窗口
            else if (i == 7111)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CJBXWinName, "OpenGivenNameWin");
            }


            //水密定级渗漏窗口
            else if (i == 7112)
            {
                Messenger.Default.Send<string>(MQZH_WinName.SMDJDamageWinName, "OpenGivenNameWin");
            }


            //水密工程渗漏窗口
            else if (i == 7113)
            {
                Messenger.Default.Send<string>(MQZH_WinName.SMGCDamageWinName, "OpenGivenNameWin");
            }

            //抗风压定级渗漏窗口
            else if (i == 7114)
            {
                Messenger.Default.Send<string>(MQZH_WinName.KFYDJDamageWinName, "OpenGivenNameWin");
            }

            //抗风压工程渗漏窗口
            else if (i == 7115)
            {
                Messenger.Default.Send<string>(MQZH_WinName.KFYGCDamageWinName, "OpenGivenNameWin");
            }

            //层间定级损坏窗口
            else if (i == 7116)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CJBXDJDamageWinName, "OpenGivenNameWin");
            }

            //层间工程损坏窗口
            else if (i == 7117)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CJBXGCDamageWinName, "OpenGivenNameWin");
            }

            //调试窗口
            else if (i == 7118)
            {
                Messenger.Default.Send<string>(MQZH_WinName.DbgWinName, "OpenGivenNameWin");
            }

            //公司信息窗口
            else if (i == 7123)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CoInfoSetWinName, "OpenGivenNameWin");
            }

            //装置基本信息窗口
            else if (i == 7124)
            {
                Messenger.Default.Send<string>(MQZH_WinName.DevInfoSetWinName, "OpenGivenNameWin");
            }

            //装置基本参数窗口
            else if (i == 7125)
            {
                Messenger.Default.Send<string>(MQZH_WinName.DevParamSetWinName, "OpenGivenNameWin");
            }

            //通讯设置窗口
            else if (i == 7126)
            {
                Messenger.Default.Send<string>(MQZH_WinName.ComSetWinName, "OpenGivenNameWin");
            }

            //传感器设置窗口
            else if (i == 7127)
            {
                Messenger.Default.Send<string>(MQZH_WinName.SenssorSetWinName, "OpenGivenNameWin");
            }

            //调零标定窗口
            else if (i == 7128)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CorrWinName, "OpenGivenNameWin");
            }

            //PID参数窗口
            else if (i == 7129)
            {
                Messenger.Default.Send<string>(MQZH_WinName.PidSetWinName, "OpenGivenNameWin");
            }

            //压力设定窗口
            else if (i == 7130)
            {
                Messenger.Default.Send<string>(MQZH_WinName.PressSetWinName, "OpenGivenNameWin");
            }

            //位移设定窗口
            else if (i == 7131)
            {
                Messenger.Default.Send<string>(MQZH_WinName.WYSetWinName, "OpenGivenNameWin");
            }

            //压力控制窗口
            else if (i == 7132)
            {
                Messenger.Default.Send<string>(MQZH_WinName.PressCtlWinName, "OpenGivenNameWin");
            }

            //位移控制窗口
            else if (i == 7133)
            {
                Messenger.Default.Send<string>(MQZH_WinName.WYCtlWinName, "OpenGivenNameWin");
            }

            //动态风压装置参数窗口
            else if (i == 7135)
            {
                Messenger.Default.Send<string>(MQZH_WinName.DTFYParamWinName, "OpenGivenNameWin");
            }

            //动态风压试验窗口
            else if (i == 7136)
            {
                Messenger.Default.Send<string>(MQZH_WinName.DTFYWinName, "OpenGivenNameWin");
            }

            //动态风压校准窗口
            else if (i == 7137)
            {
                Messenger.Default.Send<string>(MQZH_WinName.DTFYCaliWinName, "OpenGivenNameWin");
            }
        }


        #endregion

        #region 窗口打开、关闭消息（窗口互锁、绘图清零）

        /// <summary>
        /// 有窗口关闭消息
        /// </summary>
        /// <param name="msg"></param>
        private void WindowClosedMessage(string msg)
        {
            #region 窗口互锁用

            if (msg == MQZH_WinName.DbgWinName)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
                IsDTFYViewCanBeOpened = true;
                IsDTFYCaliViewCanBeOpened = true;
            }

            else if (msg == MQZH_WinName.ExpSetWinName)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
                IsDTFYViewCanBeOpened = true;
                IsDTFYCaliViewCanBeOpened = true;
            }

            else if (msg == MQZH_WinName.ExpManWinName)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
                IsDTFYViewCanBeOpened = true;
                IsDTFYCaliViewCanBeOpened = true;
            }

            else if (msg == MQZH_WinName.QMWinName)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
                IsDTFYViewCanBeOpened = true;
                IsDTFYCaliViewCanBeOpened = true;
            }

            else if (msg == MQZH_WinName.SMWinName)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
                IsDTFYViewCanBeOpened = true;
                IsDTFYCaliViewCanBeOpened = true;
            }

            else if (msg == MQZH_WinName.KFYp1Name)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
                IsDTFYViewCanBeOpened = true;
                IsDTFYCaliViewCanBeOpened = true;
            }

            else if (msg == MQZH_WinName.KFYp2Name)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
                IsDTFYViewCanBeOpened = true;
                IsDTFYCaliViewCanBeOpened = true;
            }

            else if (msg == MQZH_WinName.KFYp3Name)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
                IsDTFYViewCanBeOpened = true;
                IsDTFYCaliViewCanBeOpened = true;
            }

            else if (msg == MQZH_WinName.KFYpmaxName)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
                IsDTFYViewCanBeOpened = true;
                IsDTFYCaliViewCanBeOpened = true;
            }

            else if (msg == MQZH_WinName.CJBXWinName)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
                IsDTFYViewCanBeOpened = true;
                IsDTFYCaliViewCanBeOpened = true;
            }

            else if (msg == MQZH_WinName.DTFYCaliWinName)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
                IsDTFYViewCanBeOpened = true;
                IsDTFYCaliViewCanBeOpened = true;
            }

            else if (msg == MQZH_WinName.DTFYWinName)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
                IsDTFYViewCanBeOpened = true;
                IsDTFYCaliViewCanBeOpened = true;
            }
            #endregion

            if (msg == MQZH_WinName.QMWinName)
            {
                IsQMWinOpened = false;
                foreach (var line in PlotLines)
                    line.LineDataSource.Collection.Clear();
            }
            else if (msg == MQZH_WinName.SMWinName)
            {
                IsSMWinOpened = false;
                foreach (var line in PlotLines)
                    line.LineDataSource.Collection.Clear();
            }

            else if ((msg == MQZH_WinName.KFYp1Name) || (msg == MQZH_WinName.KFYp2Name) || (msg == MQZH_WinName.KFYp3Name) || (msg == MQZH_WinName.KFYpmaxName))
            {
                IsKFYWinOpened = false;
                foreach (var line in PlotLines)
                    line.LineDataSource.Collection.Clear();
            }
            else if (msg == MQZH_WinName.CJBXWinName)
            {
                IsCJBXWinOpened = false;
                foreach (var line in PlotLines)
                    line.LineDataSource.Collection.Clear();
            }
            else if (msg == MQZH_WinName.DTFYWinName)
            {
                IsDTFYWinOpened = false;
                foreach (var line in PlotLines)
                    line.LineDataSource.Collection.Clear();
            }
            else if (msg == MQZH_WinName.DTFYCaliWinName)
            {
                IsDTFYCaliWinOpened = false;
                foreach (var line in PlotLines)
                    line.LineDataSource.Collection.Clear();
            }
        }

        /// <summary>
        /// 有窗口打开消息
        /// </summary>
        /// <param name="msgWindow"></param>
        private void WindowAddedMessage(Window msgWindow)
        {
            if (msgWindow.Name == MQZH_WinName.DbgWinName)
            {
                IsDbgViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = false;
                IsExpManagerViewCanBeOpened = false;
                IsQMViewCanBeOpened = false;
                IsSMViewCanBeOpened = false;
                IsKFYp1ViewCanBeOpened = false;
                IsKFYp2ViewCanBeOpened = false;
                IsKFYp3ViewCanBeOpened = false;
                IsKFYpmaxViewCanBeOpened = false;
                IsCJBXViewCanBeOpened = false;
                IsDTFYViewCanBeOpened = false;
                IsDTFYCaliViewCanBeOpened = false;

                IsDbgWinOpened = true;
            }

            else if (msgWindow.Name == MQZH_WinName.ExpSetWinName)
            {
                IsDbgViewCanBeOpened = false;
                IsExpSetViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = false;
                IsQMViewCanBeOpened = false;
                IsSMViewCanBeOpened = false;
                IsKFYp1ViewCanBeOpened = false;
                IsKFYp2ViewCanBeOpened = false;
                IsKFYp3ViewCanBeOpened = false;
                IsKFYpmaxViewCanBeOpened = false;
                IsCJBXViewCanBeOpened = false;
                IsDTFYViewCanBeOpened = false;
                IsDTFYCaliViewCanBeOpened = false;

            }

            else if (msgWindow.Name == MQZH_WinName.ExpManWinName)
            {
                IsDbgViewCanBeOpened = false;
                IsExpSetViewCanBeOpened = false;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = false;
                IsSMViewCanBeOpened = false;
                IsKFYp1ViewCanBeOpened = false;
                IsKFYp2ViewCanBeOpened = false;
                IsKFYp3ViewCanBeOpened = false;
                IsKFYpmaxViewCanBeOpened = false;
                IsCJBXViewCanBeOpened = false;
                IsDTFYViewCanBeOpened = false;
                IsDTFYCaliViewCanBeOpened = false;

            }

            else if (msgWindow.Name == MQZH_WinName.QMWinName)
            {
                IsDbgViewCanBeOpened = false;
                IsExpSetViewCanBeOpened = false;
                IsExpManagerViewCanBeOpened = false;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = false;
                IsKFYp1ViewCanBeOpened = false;
                IsKFYp2ViewCanBeOpened = false;
                IsKFYp3ViewCanBeOpened = false;
                IsKFYpmaxViewCanBeOpened = false;
                IsCJBXViewCanBeOpened = false;
                IsDTFYViewCanBeOpened = false;
                IsDTFYCaliViewCanBeOpened = false;

                IsQMWinOpened = true;
            }

            else if (msgWindow.Name == MQZH_WinName.SMWinName)
            {
                IsDbgViewCanBeOpened = false;
                IsExpSetViewCanBeOpened = false;
                IsExpManagerViewCanBeOpened = false;
                IsQMViewCanBeOpened = false;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = false;
                IsKFYp2ViewCanBeOpened = false;
                IsKFYp3ViewCanBeOpened = false;
                IsKFYpmaxViewCanBeOpened = false;
                IsCJBXViewCanBeOpened = false;
                IsDTFYViewCanBeOpened = false;
                IsDTFYCaliViewCanBeOpened = false;

                IsSMWinOpened = true;
            }

            else if (msgWindow.Name == MQZH_WinName.KFYp1Name)
            {
                IsDbgViewCanBeOpened = false;
                IsExpSetViewCanBeOpened = false;
                IsExpManagerViewCanBeOpened = false;
                IsQMViewCanBeOpened = false;
                IsSMViewCanBeOpened = false;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = false;
                IsKFYp3ViewCanBeOpened = false;
                IsKFYpmaxViewCanBeOpened = false;
                IsCJBXViewCanBeOpened = false;
                IsDTFYViewCanBeOpened = false;
                IsDTFYCaliViewCanBeOpened = false;

                IsKFYWinOpened = true;
            }

            else if (msgWindow.Name == MQZH_WinName.KFYp2Name)
            {
                IsDbgViewCanBeOpened = false;
                IsExpSetViewCanBeOpened = false;
                IsExpManagerViewCanBeOpened = false;
                IsQMViewCanBeOpened = false;
                IsSMViewCanBeOpened = false;
                IsKFYp1ViewCanBeOpened = false;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = false;
                IsKFYpmaxViewCanBeOpened = false;
                IsCJBXViewCanBeOpened = false;
                IsDTFYViewCanBeOpened = false;
                IsDTFYCaliViewCanBeOpened = false;

                IsKFYWinOpened = true;
            }

            else if (msgWindow.Name == MQZH_WinName.KFYp3Name)
            {
                IsDbgViewCanBeOpened = false;
                IsExpSetViewCanBeOpened = false;
                IsExpManagerViewCanBeOpened = false;
                IsQMViewCanBeOpened = false;
                IsSMViewCanBeOpened = false;
                IsKFYp1ViewCanBeOpened = false;
                IsKFYp2ViewCanBeOpened = false;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = false;
                IsCJBXViewCanBeOpened = false;
                IsDTFYViewCanBeOpened = false;
                IsDTFYCaliViewCanBeOpened = false;

                IsKFYWinOpened = true;
            }

            else if (msgWindow.Name == MQZH_WinName.KFYpmaxName)
            {
                IsDbgViewCanBeOpened = false;
                IsExpSetViewCanBeOpened = false;
                IsExpManagerViewCanBeOpened = false;
                IsQMViewCanBeOpened = false;
                IsSMViewCanBeOpened = false;
                IsKFYp1ViewCanBeOpened = false;
                IsKFYp2ViewCanBeOpened = false;
                IsKFYp3ViewCanBeOpened = false;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = false;
                IsDTFYViewCanBeOpened = false;
                IsDTFYCaliViewCanBeOpened = false;

                IsKFYWinOpened = true;
            }

            else if (msgWindow.Name == MQZH_WinName.CJBXWinName)
            {
                IsDbgViewCanBeOpened = false;
                IsExpSetViewCanBeOpened = false;
                IsExpManagerViewCanBeOpened = false;
                IsQMViewCanBeOpened = false;
                IsSMViewCanBeOpened = false;
                IsKFYp1ViewCanBeOpened = false;
                IsKFYp2ViewCanBeOpened = false;
                IsKFYp3ViewCanBeOpened = false;
                IsKFYpmaxViewCanBeOpened = false;
                IsCJBXViewCanBeOpened = true;
                IsDTFYViewCanBeOpened = false;
                IsDTFYCaliViewCanBeOpened = false;

                IsCJBXWinOpened = true;
            }

            else if (msgWindow.Name == MQZH_WinName.DTFYWinName)
            {
                IsDbgViewCanBeOpened = false;
                IsExpSetViewCanBeOpened = false;
                IsExpManagerViewCanBeOpened = false;
                IsQMViewCanBeOpened = false;
                IsSMViewCanBeOpened = false;
                IsKFYp1ViewCanBeOpened = false;
                IsKFYp2ViewCanBeOpened = false;
                IsKFYp3ViewCanBeOpened = false;
                IsKFYpmaxViewCanBeOpened = false;
                IsCJBXViewCanBeOpened = false;
                IsDTFYViewCanBeOpened = true;
                IsDTFYCaliViewCanBeOpened = false;

                IsDTFYWinOpened = true;
            }

            else if (msgWindow.Name == MQZH_WinName.DTFYCaliWinName)
            {
                IsDbgViewCanBeOpened = false;
                IsExpSetViewCanBeOpened = false;
                IsExpManagerViewCanBeOpened = false;
                IsQMViewCanBeOpened = false;
                IsSMViewCanBeOpened = false;
                IsKFYp1ViewCanBeOpened = false;
                IsKFYp2ViewCanBeOpened = false;
                IsKFYp3ViewCanBeOpened = false;
                IsKFYpmaxViewCanBeOpened = false;
                IsCJBXViewCanBeOpened = false;
                IsDTFYViewCanBeOpened = false;
                IsDTFYCaliViewCanBeOpened = true;

                IsDTFYCaliWinOpened = true;
            }
            else if (msgWindow.Name == MQZH_WinName.CurrentDataWinName)
                NDPlotUpdate();
        }


        #region 窗口已打开属性

        /// <summary>
        /// 调试窗口已打开
        /// </summary>
        private bool _isDbgWinOpened = false;
        /// <summary>
        /// 调试窗口已打开
        /// </summary>
        public bool IsDbgWinOpened
        {
            get { return _isDbgWinOpened; }
            set
            {
                _isDbgWinOpened = value;
                RaisePropertyChanged(() => IsDbgWinOpened);
            }
        }

        /// <summary>
        /// 气密检测窗口已打开
        /// </summary>
        private bool _isQMWinOpened = false;
        /// <summary>
        /// 气密检测窗口已打开
        /// </summary>
        public bool IsQMWinOpened
        {
            get { return _isQMWinOpened; }
            set
            {
                _isQMWinOpened = value;
                RaisePropertyChanged(() => IsQMWinOpened);
            }
        }

        /// <summary>
        /// 水密检测窗口已打开
        /// </summary>
        private bool _isSMWinOpened = false;
        /// <summary>
        /// 水密检测窗口已打开
        /// </summary>
        public bool IsSMWinOpened
        {
            get { return _isSMWinOpened; }
            set
            {
                _isSMWinOpened = value;
                RaisePropertyChanged(() => IsSMWinOpened);
            }
        }

        /// <summary>
        /// 抗风压窗口已打开
        /// </summary>
        private bool _isKFYWinOpened = false;
        /// <summary>
        /// 抗风压窗口已打开
        /// </summary>
        public bool IsKFYWinOpened
        {
            get { return _isKFYWinOpened; }
            set
            {
                _isKFYWinOpened = value;
                RaisePropertyChanged(() => IsKFYWinOpened);
            }
        }

        /// <summary>
        /// 层间变形窗口已打开
        /// </summary>
        private bool _isCJBXWinOpened = false;
        /// <summary>
        /// 层间变形窗口已打开
        /// </summary>
        public bool IsCJBXWinOpened
        {
            get { return _isCJBXWinOpened; }
            set
            {
                _isCJBXWinOpened = value;
                RaisePropertyChanged(() => IsCJBXWinOpened);
            }
        }

        /// <summary>
        /// 动态风压测试窗口
        /// </summary>
        private bool _isDTFYWinOpened = false;
        /// <summary>
        /// 动态风压测试窗口
        /// </summary>
        public bool IsDTFYWinOpened
        {
            get { return _isDTFYWinOpened; }
            set
            {
                _isDTFYWinOpened = value;
                RaisePropertyChanged(() => IsDTFYWinOpened);
            }
        }

        /// <summary>
        /// 动态风压校准窗口
        /// </summary>
        private bool _isDTFYCaliWinOpened = false;
        /// <summary>
        /// 动态风压校准窗口
        /// </summary>
        public bool IsDTFYCaliWinOpened
        {
            get { return _isDTFYCaliWinOpened; }
            set
            {
                _isDTFYCaliWinOpened = value;
                RaisePropertyChanged(() => IsDTFYCaliWinOpened);
            }
        }
        #endregion


        #region 窗口可以打开状态属性

        /// <summary>
        /// DbgView可以打开状态
        /// </summary>
        private bool _isDbgViewCanBeOpened = true;
        /// <summary>
        /// DbgView可以打开状态
        /// </summary>
        public bool IsDbgViewCanBeOpened
        {
            get { return _isDbgViewCanBeOpened; }
            set
            {
                _isDbgViewCanBeOpened = value;
                RaisePropertyChanged(() => IsDbgViewCanBeOpened);
            }
        }

        /// <summary>
        /// QMView可以打开状态
        /// </summary>
        private bool _isQMViewCanBeOpened = true;
        /// <summary>
        /// QMView可以打开状态
        /// </summary>
        public bool IsQMViewCanBeOpened
        {
            get { return _isQMViewCanBeOpened; }
            set
            {
                _isQMViewCanBeOpened = value;
                RaisePropertyChanged(() => IsQMViewCanBeOpened);
            }
        }

        /// <summary>
        /// SMView可以打开状态
        /// </summary>
        private bool _isSMViewCanBeOpened = true;
        /// <summary>
        /// SMView可以打开状态
        /// </summary>
        public bool IsSMViewCanBeOpened
        {
            get { return _isSMViewCanBeOpened; }
            set
            {
                _isSMViewCanBeOpened = value;
                RaisePropertyChanged(() => IsSMViewCanBeOpened);
            }
        }

        /// <summary>
        /// CJBXView可以打开状态
        /// </summary>
        private bool _isCJBXViewCanBeOpened = true;
        /// <summary>
        /// CJBXView可以打开状态
        /// </summary>
        public bool IsCJBXViewCanBeOpened
        {
            get { return _isCJBXViewCanBeOpened; }
            set
            {
                _isCJBXViewCanBeOpened = value;
                RaisePropertyChanged(() => IsCJBXViewCanBeOpened);
            }
        }

        /// <summary>
        /// ExpSetView可以打开状态
        /// </summary>
        private bool _isExpSetViewCanBeOpened = true;
        /// <summary>
        /// ExpSetView可以打开状态
        /// </summary>
        public bool IsExpSetViewCanBeOpened
        {
            get { return _isExpSetViewCanBeOpened; }
            set
            {
                _isExpSetViewCanBeOpened = value;
                RaisePropertyChanged(() => IsExpSetViewCanBeOpened);
            }
        }

        /// <summary>
        /// ExpManagerView可以打开状态
        /// </summary>
        private bool _isExpManagerViewCanBeOpened = true;
        /// <summary>
        /// ExpManagerView可以打开状态
        /// </summary>
        public bool IsExpManagerViewCanBeOpened
        {
            get { return _isExpManagerViewCanBeOpened; }
            set
            {
                _isExpManagerViewCanBeOpened = value;
                RaisePropertyChanged(() => IsExpManagerViewCanBeOpened);
            }
        }

        /// <summary>
        /// KFYp1View可以打开状态
        /// </summary>
        private bool _isKFYp1ViewCanBeOpened = true;
        /// <summary>
        /// KFYp1View可以打开状态
        /// </summary>
        public bool IsKFYp1ViewCanBeOpened
        {
            get { return _isKFYp1ViewCanBeOpened; }
            set
            {
                _isKFYp1ViewCanBeOpened = value;
                RaisePropertyChanged(() => IsKFYp1ViewCanBeOpened);
            }
        }

        /// <summary>
        /// KFYp2View可以打开状态
        /// </summary>
        private bool _isKFYp2ViewCanBeOpened = true;
        /// <summary>
        /// KFYp2View可以打开状态
        /// </summary>
        public bool IsKFYp2ViewCanBeOpened
        {
            get { return _isKFYp2ViewCanBeOpened; }
            set
            {
                _isKFYp2ViewCanBeOpened = value;
                RaisePropertyChanged(() => IsKFYp2ViewCanBeOpened);
            }
        }

        /// <summary>
        /// KFYp3View可以打开状态
        /// </summary>
        private bool _isKFYp3ViewCanBeOpened = true;
        /// <summary>
        /// KFYp3View可以打开状态
        /// </summary>
        public bool IsKFYp3ViewCanBeOpened
        {
            get { return _isKFYp3ViewCanBeOpened; }
            set
            {
                _isKFYp3ViewCanBeOpened = value;
                RaisePropertyChanged(() => IsKFYp3ViewCanBeOpened);
            }
        }

        /// <summary>
        /// KFYpmaxXView可以打开状态
        /// </summary>
        private bool _isKFYpmaxViewCanBeOpened = true;
        /// <summary>
        /// KFYpmaxView可以打开状态
        /// </summary>
        public bool IsKFYpmaxViewCanBeOpened
        {
            get { return _isKFYpmaxViewCanBeOpened; }
            set
            {
                _isKFYpmaxViewCanBeOpened = value;
                RaisePropertyChanged(() => IsKFYpmaxViewCanBeOpened);
            }
        }

        /// <summary>
        /// dtfyView可以打开状态
        /// </summary>
        private bool _isDTFYViewCanBeOpened = true;
        /// <summary>
        /// dtfyView可以打开状态
        /// </summary>
        public bool IsDTFYViewCanBeOpened
        {
            get { return _isDTFYViewCanBeOpened; }
            set
            {
                _isDTFYViewCanBeOpened = value;
                RaisePropertyChanged(() => IsDTFYViewCanBeOpened);
            }
        }

        /// <summary>
        /// dtfy校准View可以打开状态
        /// </summary>
        private bool _isDTFYCaliViewCanBeOpened = true;
        /// <summary>
        /// dtfy校准View可以打开状态
        /// </summary>
        public bool IsDTFYCaliViewCanBeOpened
        {
            get { return _isDTFYCaliViewCanBeOpened; }
            set
            {
                _isDTFYCaliViewCanBeOpened = value;
                RaisePropertyChanged(() => IsDTFYCaliViewCanBeOpened);
            }
        }
        #endregion

        #endregion
    }
}
