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

                IsDTFYWinOpened = true;
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
        #endregion

        #endregion
    }
}
