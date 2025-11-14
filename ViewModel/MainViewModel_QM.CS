using GalaSoft.MvvmLight;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using System;
using GalaSoft.MvvmLight.Command;
using MQDFJ_MB.Model;


namespace MQDFJ_MB.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        #region 气密检测菜单、按钮操作指令

        /// <summary>
        /// 传递指令
        /// </summary>
        private RelayCommand<String> _qmCommand;
        /// <summary>
        /// 传递指令
        /// </summary>
        public RelayCommand<String> QMCommand
        {
            get
            {
                if (_qmCommand == null)
                    _qmCommand = new RelayCommand<String>((p) => ExecuteQMCMD(p));
                return _qmCommand;

            }
            set { _qmCommand = value; }
        }

        /// <summary>
        /// 根据按钮编号，切换按钮指令、发送通讯指令
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecuteQMCMD(String num)
        {
            int i = Convert.ToInt16(num);


            //紧急停机
            if (i == 119)
            {
                Messenger.Default.Send<string>("JJTJ", "JJTJMessage");
            }

            #region 实验启停

            //试验开始
            else if ((i == 1101) || (i == 1111))
            {
                //如果当前试验是默认试验，选择状态复位。
                if (PublicData.ExpDQ.ExpSettingParam.ExpNO == "DefaultExp")
                {
                    MessageBox.Show("默认试验无法进行试验动作，请新建或载入其他试验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    //气密试验已选阶段数组清零
                    PublicData.ExpDQ.Exp_QM.CanBeCheckInit();
                    return;
                }
                //正在试验中，不能重新加入试验
                if (PublicData.Dev.IsDeviceBusy)
                {
                    MessageBox.Show("设备占用中，请等待或先终止其他实验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    return;
                }

                Messenger.Default.Send<int>(i, "QMCtrlMessage");
            }

            //停机指令
            else if ((i == 1201) || (i == 1211))
            {
                Messenger.Default.Send<int>(i, "StopExpMessage");
            }
            #endregion


            #region 窗口操作

            //实验数据窗口
            else if (i == 7104)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CurrentDataWinName, "OpenGivenNameWin");
            }
            //调零标定窗口
            else if (i == 7128)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CorrWinName, "OpenGivenNameWin");
            }
            //关闭气密窗口
            else if (i == 7205)
            {
                Messenger.Default.Send<int>(i, "QMCtrlMessage");
            }
            #endregion
        }

        #endregion

    }
}
