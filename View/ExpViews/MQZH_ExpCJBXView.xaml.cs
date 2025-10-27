using GalaSoft.MvvmLight.Messaging;
using Microsoft.Research.DynamicDataDisplay.Charts;
using MQZHWL.Model;
using MQZHWL.View.ExpViews;
using MQZHWL.View.Others;
using MQZHWL.View.RepAndData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace MQZHWL.View.ExpViews
{
    /// <summary>
    /// MQZH_ExpCJBXView.xaml 的交互逻辑
    /// </summary>
    public partial class MQZH_ExpCJBXView : Window
    {
        public MQZH_ExpCJBXView()
        {
            InitializeComponent();

            Legend.SetDescription(LineCJBX01, "X轴 mm");
            Legend.SetDescription(LineCJBX02, "Y轴 mm");
            Legend.SetDescription(LineCJBX03, "Z轴 mm");

            //注册终止试验消息
            Messenger.Default.Register<int>(this, "StopExpMessage", ExpStopMessage);
        }


        #region 相关状态

        /// <summary>
        /// 实验中状态
        /// </summary>
        private bool _isTesting = false;
        /// <summary>
        /// 实验中状态
        /// </summary>
        public bool IsTesting
        {
            get { return _isTesting; }
            set
            {
                _isTesting = value;
            }
        }

        #endregion


        #region 试验开始、停止消息处理，变更正在试验状态

        /// <summary>
        /// 试验停止消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void ExpStopMessage(int msg)
        {
            if ((msg == 1207) || (msg == 1217)||(msg == 1209) || (msg == 1219)||(msg == 1200) || (msg == 1210))
            {
                IsTesting = false;
            }
        }

        #endregion

        
        #region 关闭窗口处理方法

        /// <summary>
        /// 窗口退出处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (IsTesting)
                {
                    MessageBoxResult msgBoxResult = MessageBox.Show("参数变更后尚未存盘，确认退出程序?", "提示", MessageBoxButton.YesNo);
                    if (msgBoxResult == MessageBoxResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        //注销消息
                        Messenger.Default.Unregister(this);
                        //通知主窗口已关闭某窗口
                        Messenger.Default.Send<string>(this.Name, "WindowClosed");
                    }
                }
                else
                {
                    //注销消息
                    Messenger.Default.Unregister(this);
                    //通知主窗口已关闭某窗口
                    Messenger.Default.Send<string>(this.Name, "WindowClosed");
                }
            }
            catch (Exception)
            {

            }
        }

        #endregion 
    }
}
