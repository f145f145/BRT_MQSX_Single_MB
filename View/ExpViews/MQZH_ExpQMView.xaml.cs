using GalaSoft.MvvmLight.Messaging;
using Microsoft.Research.DynamicDataDisplay.Charts;
using MQDFJ_MB.Model;
using System;
using System.Windows;

namespace MQDFJ_MB.View.ExpViews
{
    /// <summary>
    /// ExperimentView.xaml 的交互逻辑
    /// </summary>
    public partial class MQZH_ExpQMView : Window
    {
        public MQZH_ExpQMView()
        {
            InitializeComponent();

            Legend.SetDescription(LineQM01, "压力小 Pa");
            Legend.SetDescription(LineQM03, "风量 m³/h");
            Legend.SetDescription(LineQM04, "频率 Hz");

            Legend.SetDescription(LineQM12, "压力大 Pa");
            Legend.SetDescription(LineQM13, "风量 m³/h");
            Legend.SetDescription(LineQM14, "频率 Hz");

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


        /// <summary>
        /// 阀门状态弹窗响应1
        /// </summary>
        private void FFStatusButtonClick(object sender, RoutedEventArgs e)
        {
            if (!FFStatusPopup.IsOpen)
                FFStatusPopup.IsOpen = true;
            else
                FFStatusPopup.IsOpen = false;
        }

        #region 试验开始、停止消息处理，变更正在试验状态

        /// <summary>
        /// 试验停止消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void ExpStopMessage(int msg)
        {
            if ((msg == 1201)||(msg == 1211))
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
                    MessageBoxResult msgBoxResult = MessageBox.Show("试验正在进行中，确认退出程序?", "提示", MessageBoxButton.YesNo);
                    if (msgBoxResult == MessageBoxResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        //发送停止试验消息
                        Messenger.Default.Send<int>(1201, "StopExpMessage");
                        //通知主窗口已关闭某窗口
                        Messenger.Default.Send<string>(this.Name, "WindowClosed");
                        //注销消息
                        Messenger.Default.Unregister(this);
                    }
                }
                else
                {
                    //通知主窗口已关闭某窗口
                    Messenger.Default.Send<string>(this.Name, "WindowClosed");
                    //注销消息
                    Messenger.Default.Unregister(this);
                }
            }
            catch (Exception)
            {

            }
        }

        #endregion

       
    }
}