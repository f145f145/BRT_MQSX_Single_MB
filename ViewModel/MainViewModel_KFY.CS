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
        #region 抗风压菜单、按钮操作指令

        /// <summary>
        /// 传递指令
        /// </summary>
        private RelayCommand<String> _kfyCommand;
        /// <summary>
        /// 传递指令
        /// </summary>
        public RelayCommand<String> KFYCommand
        {
            get
            {
                if (_kfyCommand == null)
                    _kfyCommand = new RelayCommand<String>((p) => ExecuteKFYCMD(p));
                return _kfyCommand;

            }
            set { _kfyCommand = value; }
        }

        /// <summary>
        /// 根据按钮编号，切换按钮指令、发送通讯指令
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecuteKFYCMD(String num)
        {
            int i = Convert.ToInt16(num);

            #region 共用

            //紧急停机
            if (i == 119)
            {
                Messenger.Default.Send<string>("JJTJ", "JJTJMessage");
            }

            #endregion

            #region 开始、停止实验

            //p1检测开始
            else if ((i == 1103) || (i == 1113))
            {
                //如果当前试验是默认试验，选择状态复位。
                if (PublicData.ExpDQ.ExpSettingParam.ExpNO == "DefaultExp")
                {
                    MessageBox.Show("默认试验无法进行试验动作，请新建或载入其他试验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    //抗风压已选阶段数组清零
                    PublicData.ExpDQ.Exp_KFY.CanBeCheckInit();
                    return;
                }
                //正在试验中，不能重新加入试验
                if (PublicData.Dev.IsDeviceBusy)
                {
                    MessageBox.Show("设备占用中，请等待或先终止其他实验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    return;
                }
                Messenger.Default.Send<int>(i, "KFYp1CtrlMessage");
            }
            //p2检测开始
            else if ((i == 1104) || (i == 1114))
            {
                //如果当前试验是默认试验，选择状态复位。
                if (PublicData.ExpDQ.ExpSettingParam.ExpNO == "DefaultExp")
                {
                    MessageBox.Show("默认试验无法进行试验动作，请新建或载入其他试验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    //抗风压已选阶段数组清零
                    PublicData.ExpDQ.Exp_KFY.CanBeCheckInit();
                    return;
                }
                //正在试验中，不能重新加入试验
                if (PublicData.Dev.IsDeviceBusy)
                {
                    MessageBox.Show("设备占用中，请等待或先终止其他实验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    return;
                }
                Messenger.Default.Send<int>(i, "KFYp2CtrlMessage");
            }
            //p3检测开始
            else if ((i == 1105) || (i == 1115))
            {
                //如果当前试验是默认试验，选择状态复位。
                if (PublicData.ExpDQ.ExpSettingParam.ExpNO == "DefaultExp")
                {
                    MessageBox.Show("默认试验无法进行试验动作，请新建或载入其他试验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    //抗风压已选阶段数组清零
                    PublicData.ExpDQ.Exp_KFY.CanBeCheckInit();
                    return;
                }
                //正在试验中，不能重新加入试验
                if (PublicData.Dev.IsDeviceBusy)
                {
                    MessageBox.Show("设备占用中，请等待或先终止其他实验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    return;
                }
                Messenger.Default.Send<int>(i, "KFYp3CtrlMessage");
            }
            //pmax检测开始
            else if ((i == 1106) || (i == 1116))
            {
                //如果当前试验是默认试验，选择状态复位。
                if (PublicData.ExpDQ.ExpSettingParam.ExpNO == "DefaultExp")
                {
                    MessageBox.Show("默认试验无法进行试验动作，请新建或载入其他试验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    //抗风压已选阶段数组清零
                    PublicData.ExpDQ.Exp_KFY.CanBeCheckInit();
                    return;
                }
                //正在试验中，不能重新加入试验
                if (PublicData.Dev.IsDeviceBusy)
                {
                    MessageBox.Show("设备占用中，请等待或先终止其他实验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    return;
                }
                Messenger.Default.Send<int>(i, "KFYpmaxCtrlMessage");
            }
            //停机指令
            else if ((i == 1203) || (i == 1213) || (i == 1204) || (i == 1214) || (i == 1205) || (i == 1215) || (i == 1206) || (i == 1216))
            {
                Messenger.Default.Send<int>(i, "StopExpMessage");
            }
            //p1损坏结束
            else if ((i == 1403) || (i == 1413))
            {
                Messenger.Default.Send<int>(i, "KFYp1CtrlMessage");
            }
            //p2损坏结束
            else if ((i == 1404) || (i == 1414))
            {
                Messenger.Default.Send<int>(i, "KFYp2CtrlMessage");
            }
            //p3损坏结束
            else if ((i == 1405) || (i == 1415))
            {
                Messenger.Default.Send<int>(i, "KFYp3CtrlMessage");
            }
            //pmax损坏结束
            else if ((i == 1406) || (i == 1416))
            {
                Messenger.Default.Send<int>(i, "KFYpmaxCtrlMessage");
            }

            #endregion


            #region 窗口操作

            //定级检测损坏情况窗口
            else if (i == 7114)
            {
                Messenger.Default.Send<string>(MQZH_WinName.KFYDJDamageWinName, "OpenGivenNameWin");
            }
            //工程检测损坏情况窗口
            else if (i == 7115)
            {
                Messenger.Default.Send<string>(MQZH_WinName.KFYGCDamageWinName, "OpenGivenNameWin");
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


            //退出P1检测窗口
            else if (i == 7206)
            {
                Messenger.Default.Send<int>(i, "KFYp1CtrlMessage");
            }


            //退出P2检测窗口
            else if (i == 7208)
            {
                Messenger.Default.Send<int>(i, "KFYp2CtrlMessage");
            }

            //退出P3检测窗口
            else if (i == 7209)
            {
                Messenger.Default.Send<int>(i, "KFYp3CtrlMessage");
            }

            //退出Pmax检测窗口
            else if (i == 7210)
            {
                Messenger.Default.Send<int>(i, "KFYpmaxCtrlMessage");
            }
            #endregion


            #region 保存、取消

            //损坏数据保存
            else if ((i == 3103) || (i == 3104))
            {
                Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
            }

            #endregion

        }

        #endregion
    }
}
