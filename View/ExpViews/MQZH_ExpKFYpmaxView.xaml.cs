using GalaSoft.MvvmLight.Messaging;
using Microsoft.Research.DynamicDataDisplay.Charts;
using MQZHWL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MQZHWL.View.ExpViews
{
    /// <summary>
    /// MQZH_ExpKFYpmaxView.xaml 的交互逻辑
    /// </summary>
    public partial class MQZH_ExpKFYpmaxView : Window
    {
        public MQZH_ExpKFYpmaxView()
        {
            InitializeComponent();

            Legend.SetDescription(LineKFY01, "压力 Pa");
            Legend.SetDescription(LineKFY02, "频率 Hz");

            Legend.SetDescription(LineKFY04, "挠度1 mm");
            Legend.SetDescription(LineKFY05, "挠度2 mm");
            Legend.SetDescription(LineKFY06, "挠度3 mm");
            Legend.SetDescription(LineKFY07, "相对挠度1");
            Legend.SetDescription(LineKFY08, "相对挠度2");
            Legend.SetDescription(LineKFY09, "相对挠度3");

            //注册终止试验消息
            Messenger.Default.Register<int>(this, "StopExpMessage", ExpStopMessage);
        }

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
            if ((msg == 1206) || (msg == 1216))
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
                        Messenger.Default.Send<int>(1206, "StopExpMessage");
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

