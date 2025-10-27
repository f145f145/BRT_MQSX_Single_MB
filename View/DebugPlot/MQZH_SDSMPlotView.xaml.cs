using System.Windows;
using System;
using Microsoft.Research.DynamicDataDisplay.Charts;
using GalaSoft.MvvmLight.Messaging;

namespace MQZHWL.View.DebugPlot
{
    /// <summary>
    /// MQZH_SDSMBDPlotView.xaml 的交互逻辑
    /// </summary>
    public partial class MQZH_SDSMPlotView : Window
    {
        public MQZH_SDSMPlotView()
        {
            InitializeComponent();

            Legend.SetDescription(LineSMDbg01, "压力2 Pa");
            Legend.SetDescription(LineSMDbg02, "压力3 Pa");
            Legend.SetDescription(LineSMDbg03, "频率 Hz");
        }


        #region 关闭窗口处理方法


        #region 相关状态

        /// <summary>
        /// 参数变更需要存盘标志
        /// </summary>
        private bool _isNeedSave = false;
        /// <summary>
        /// 参数变更需要存盘标志
        /// </summary>
        public bool IsNeedSave
        {
            get { return _isNeedSave; }
            set
            {
                _isNeedSave = value;
            }
        }

        #endregion

        /// <summary>
        /// 窗口退出处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (IsNeedSave)
                {
                    MessageBoxResult msgBoxResult = MessageBox.Show("参数变更后尚未存盘，确认退出程序?", "提示", MessageBoxButton.YesNo);
                    if (msgBoxResult == MessageBoxResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        //通知主窗口已关闭某窗口
                        Messenger.Default.Send<string>(this.Name, "WindowClosed");
                    }
                }
                else
                {
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
