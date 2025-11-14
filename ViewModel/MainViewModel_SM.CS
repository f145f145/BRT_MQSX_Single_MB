using GalaSoft.MvvmLight;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using System;
using GalaSoft.MvvmLight.Command;
using MQDFJ_MB.Model;
using static MQDFJ_MB.Model.MQZH_Enums;


namespace MQDFJ_MB.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {

        #region 水密菜单、按钮操作指令

        /// <summary>
        /// 传递指令
        /// </summary>
        private RelayCommand<String> _smCommand;
        /// <summary>
        /// 传递指令
        /// </summary>
        public RelayCommand<String> SMCommand
        {
            get
            {
                if (_smCommand == null)
                    _smCommand = new RelayCommand<String>((p) => ExecuteSMCMD(p));
                return _smCommand;

            }
            set { _smCommand = value; }
        }

        /// <summary>
        /// 根据按钮编号，切换按钮指令、发送通讯指令
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecuteSMCMD(String num)
        {
            int i = Convert.ToInt16(num);

            //紧急停机
            if (i == 119)
            {
                Messenger.Default.Send<string>("JJTJ", "JJTJMessage");
            }

            #region 开始、停止实验

            //检测开始
            else if ((i == 1102) || (i == 1112))
            {
                //如果当前试验是默认试验，选择状态复位。
                if (PublicData.ExpDQ.ExpSettingParam.ExpNO == "DefaultExp")
                {
                    MessageBox.Show("默认试验无法进行试验动作，请新建或载入其他试验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    //水密试验已选阶段数组清零
                    PublicData.ExpDQ.Exp_SM.CanBeCheckInit();
                    return;
                }
                //正在试验中，不能重新加入试验
                if (PublicData.Dev.IsDeviceBusy)
                {
                    MessageBox.Show("设备占用中，请等待或先终止其他实验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    return;
                }

                Messenger.Default.Send<int>(i, "SMCtrlMessage");
            }
            //停机指令
            else if ((i == 1202) || (i == 1212))
            {
                Messenger.Default.Send<int>(i, "StopExpMessage");
            }

            //渗漏结束
            else if ((i == 1302) || (i == 1312))
            {
                MessageBoxResult tempRet = MessageBox.Show("当可开启部分和固定部分均出现严重泄漏时，才需要提前停机。\r\n是否提前停机？", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                if (tempRet == MessageBoxResult.Yes)
                {
                    Messenger.Default.Send<int>(i, "SMCtrlMessage");
                }
            }

            #endregion

            #region 窗口操作

            //定级检测损坏情况窗口
            else if (i == 7112)
            {
                PublicData.ExpDQ.ExpData_SM.SLStatusCopy();
                Messenger.Default.Send<string>(MQZH_WinName.SMDJDamageWinName, "OpenGivenNameWin");
            }
            //工程检测损坏情况窗口
            else if (i == 7113)
            {
                PublicData.ExpDQ.ExpData_SM.SLStatusCopy();
                Messenger.Default.Send<string>(MQZH_WinName.SMGCDamageWinName, "OpenGivenNameWin");
            }
            //数据报告窗口
            else if (i == 7104)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CurrentDataWinName, "OpenGivenNameWin");
            }

            //调零标定窗口
            else if (i == 7128)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CorrWinName, "OpenGivenNameWin");
            }
            //关闭水密窗口
            else if (i == 7207)
            {
                Messenger.Default.Send<int>(i, "SMCtrlMessage");
            }
            #endregion

            #region 水密水泵启停

            //启停水泵
            else if (i == 1722)
            {
                if (PublicData.Dev.DeviceRunMode == DevicRunModeType.SM_Mode)
                    PublicData.Dev.DOList[6].IsOn = !PublicData.Dev.DOList[6].IsOn;
            }

            #endregion

            #region 数据保存、取消修改

            //损坏数据保存
            else if ((i == 2104) || (i == 2204))
            {
                PublicData.ExpDQ.ExpData_SM.SLStatusCopyBack();
                Messenger.Default.Send<string>("SaveSMStatusAndData", "SaveExpMessage");
            }

            #endregion
        }

        #endregion

    }
}
