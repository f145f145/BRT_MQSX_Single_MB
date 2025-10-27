using System.Windows;
using System;
using GalaSoft.MvvmLight.Messaging;
using MQZHWL.Communication;
using static MQZHWL.Model.MQZH_Enums;
using MQZHWL.Model;

namespace MQZHWL.View
{
    /// <summary>
    /// Debug.xaml 的交互逻辑
    /// </summary>
    public partial class MQZH_DebugView : Window
    {
        public MQZH_DebugView()
        {
            InitializeComponent();
        }

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

        #region 换向阀状态弹窗


        /// <summary>
        /// 阀门状态弹窗响应1
        /// </summary>
        private void FFStatusButtonClick1(object sender, RoutedEventArgs e)
        {
            if (!FFStatusPopup1.IsOpen)
                FFStatusPopup1.IsOpen = true;
            else
                FFStatusPopup1.IsOpen = false;
        }


        /// <summary>
        /// 阀门状态弹窗响应2
        /// </summary>
        private void FFStatusButtonClick2(object sender, RoutedEventArgs e)
        {
            if (!FFStatusPopup2.IsOpen)
                FFStatusPopup2.IsOpen = true;
            else
                FFStatusPopup2.IsOpen = false;
        }


        /// <summary>
        /// 阀门状态弹窗响应3
        /// </summary>
        private void FFStatusButtonClick3(object sender, RoutedEventArgs e)
        {
            if (!FFStatusPopup3.IsOpen)
                FFStatusPopup3.IsOpen = true;
            else
                FFStatusPopup3.IsOpen = false;
        }


        /// <summary>
        /// 阀门状态弹窗响应4
        /// </summary>
        private void FFStatusButtonClick4(object sender, RoutedEventArgs e)
        {
            if (!FFStatusPopup4.IsOpen)
                FFStatusPopup4.IsOpen = true;
            else
                FFStatusPopup4.IsOpen = false;
        }


        #endregion


        #region 个别按钮及菜单事件

        /// <summary>
        /// 当前试验数据及报告
        /// </summary>
        private void Menu_DataRepCurr_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<string>(MQZH_WinName.CurrentDataWinName, "OpenGivenNameWin");
        }

        /// <summary>
        /// 调零标校
        /// </summary>
        private void Menu_Corr_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<string>(MQZH_WinName.CorrWinName, "OpenGivenNameWin");
        }

        /// <summary>
        /// 退出菜单
        /// </summary>
        private void Menu_Quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
                        Messenger.Default.Send<string >(this.Name , "WindowClosed");
                    }
                }
                else
                {
                    //通知主窗口已关闭某窗口
                    Messenger.Default.Send<string>(this.Name , "WindowClosed");
                }
            }

            catch (Exception)
            {

            }
        }
        
        #endregion 
    }
}
