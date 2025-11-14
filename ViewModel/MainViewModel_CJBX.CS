using GalaSoft.MvvmLight;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using System;
using GalaSoft.MvvmLight.Command;
using MQDFJ_MB.Model;
using System.Linq;


namespace MQDFJ_MB.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        #region 层间变形菜单、按钮操作指令

        /// <summary>
        /// 层间变形单次发送指令数组
        /// </summary>
        private ushort[] _cjbxOrderArray = Enumerable.Repeat((ushort)0, 20).ToArray();
        /// <summary>
        /// 层间变形单次发送指令数组
        /// </summary>
        public ushort[] CJBXOrderArray
        {
            get { return _cjbxOrderArray; }
            set
            {
                _cjbxOrderArray = value;
                RaisePropertyChanged(() => CJBXOrderArray);
            }
        }

        /// <summary>
        /// 窗口指令
        /// </summary>
        private RelayCommand<String> _cjbxCommand;
        /// <summary>
        /// 窗口指令
        /// </summary>
        public RelayCommand<String> CJBXCommand
        {
            get
            {
                if (_cjbxCommand == null)
                    _cjbxCommand = new RelayCommand<String>((p) => ExecuteCJBXCMD(p));
                return _cjbxCommand;

            }
            set { _cjbxCommand = value; }
        }

        /// <summary>
        /// 窗口指令回调
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecuteCJBXCMD(String num)
        {
            int i = Convert.ToInt16(num);

            //紧急停机
            if (i == 119)
            {
                Messenger.Default.Send<string>("JJTJ", "JJTJMessage");
            }
            //
            #region 开始、结束实验
            //停机指令
            else if ((i == 1207) || (i == 1217) || (i == 1208) || (i == 1218) || (i == 1209) || (i == 1219))
            {
                Messenger.Default.Send<int>(i, "StopExpMessage");
            }
            //X轴开始
            else if ((i == 1107) || (i == 1117))
            {
                //如果当前试验是默认试验，选择状态复位。
                if (PublicData.ExpDQ.ExpSettingParam.ExpNO == "DefaultExp")
                {
                    MessageBox.Show("默认试验无法进行试验动作，请新建或载入其他试验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    //已选阶段数组清零
                    PublicData.ExpDQ.Exp_CJBX.CanBeCheckInit();
                    return;
                }
                //正在试验中，不能重新加入试验
                if (PublicData.Dev.IsDeviceBusy)
                {
                    MessageBox.Show("设备占用中，请等待或先终止其他实验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    return;
                }

                if (Math.Abs(PublicData.Dev.WYValueX) > 10)
                {
                    MessageBoxResult msgBoxResult = MessageBox.Show("X轴位移尺距离零点较远（10mm以上）建议先仔细检查或手动调零。如需继续测试请按“是”，如需返回请按“否”", "调零提示", MessageBoxButton.YesNo);
                    if (msgBoxResult == MessageBoxResult.No)
                        return;
                }
                Messenger.Default.Send<int>(i, "CJBXCtrlMessage");
            }
            //Y轴开始
            else if ((i == 1108) || (i == 1118))
            {
                //如果当前试验是默认试验，选择状态复位。
                if (PublicData.ExpDQ.ExpSettingParam.ExpNO == "DefaultExp")
                {
                    MessageBox.Show("默认试验无法进行试验动作，请新建或载入其他试验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    //已选阶段数组清零
                    PublicData.ExpDQ.Exp_CJBX.CanBeCheckInit();
                    return;
                }
                //正在试验中，不能重新加入试验
                if (PublicData.Dev.IsDeviceBusy)
                {
                    MessageBox.Show("设备占用中，请等待或先终止其他实验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    return;
                }
                if (Math.Abs(PublicData.Dev.WYValueY[3]) > 10)
                {
                    MessageBoxResult msgBoxResult = MessageBox.Show("Y轴位移尺平均值距离零点较远（10mm以上）建议先仔细检查或手动调零。如需继续测试请按“是”，如需返回请按“否”", "调零提示", MessageBoxButton.YesNo);
                    if (msgBoxResult == MessageBoxResult.No)
                        return;
                }
                Messenger.Default.Send<int>(i, "CJBXCtrlMessage");
            }
            //Z轴开始
            else if ((i == 11079) || (i == 1119))
            {
                //如果当前试验是默认试验，选择状态复位。
                if (PublicData.ExpDQ.ExpSettingParam.ExpNO == "DefaultExp")
                {
                    MessageBox.Show("默认试验无法进行试验动作，请新建或载入其他试验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    //已选阶段数组清零
                    PublicData.ExpDQ.Exp_CJBX.CanBeCheckInit();
                    return;
                }
                //正在试验中，不能重新加入试验
                if (PublicData.Dev.IsDeviceBusy)
                {
                    MessageBox.Show("设备占用中，请等待或先终止其他实验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    return;
                }
                if (Math.Abs(PublicData.Dev.WYValueZ[3]) > 10)
                {
                    MessageBoxResult msgBoxResult = MessageBox.Show("Z轴位移尺平均值距离零点较远（10mm以上）建议先仔细检查或手动调零。如需继续测试请按“是”，如需返回请按“否”", "调零提示", MessageBoxButton.YesNo);
                    if (msgBoxResult == MessageBoxResult.No)
                        return;
                }
                Messenger.Default.Send<int>(i, "CJBXCtrlMessage");
            }

            #endregion

            #region 窗口操作

            //打开定级检测损坏情况确认窗口
            else if (i == 7116)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CJBXDJDamageWinName, "OpenGivenNameWin");
            }
            //打开工程检测损坏情况确认窗口
            else if (i == 7117)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CJBXGCDamageWinName, "OpenGivenNameWin");
            }

            //打开数据窗口
            else if (i == 7104)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CurrentDataWinName, "OpenGivenNameWin");
            }

            //退出层间窗口
            else if (i == 7211)
            {
                Messenger.Default.Send<int>(i, "CJBXCtrlMessage");
            }
            #endregion


            #region 设置零点

            //设置X轴位移零点
            else if (i == 1630)
            {
                //自身变形位移调零
                Messenger.Default.Send<string>("X", "SetZeroMessage");
                Messenger.Default.Send<string>("SaveDevSenssorSettings", "DevDataRWMessage");
            }
            //设置Y轴位移零点
            else if (i == 1631)
            {
                //自身变形位移调零
                Messenger.Default.Send<string>("Y", "SetZeroMessage");
                Messenger.Default.Send<string>("SaveDevSenssorSettings", "DevDataRWMessage");
            }
            //设置Z轴位移零点
            else if (i == 1632)
            {
                //自身变形位移调零
                Messenger.Default.Send<string>("Z", "SetZeroMessage");
                Messenger.Default.Send<string>("SaveDevSenssorSettings", "DevDataRWMessage");
            }
            //设置XYZ三轴位移零点
            else if (i == 1633)
            {
                //自身变形位移调零
                Messenger.Default.Send<string>("XYZ", "SetZeroMessage");
                Messenger.Default.Send<string>("SaveDevSenssorSettings", "DevDataRWMessage");
            }

            #endregion


            #region 数据保存、取消修改

            //检测损坏情况回存
            else if ((i == 4104) || (i == 4204))
            {
                Messenger.Default.Send<string>("SaveCJBXStatusAndData", "SaveExpMessage");
            }

            #endregion
        }


        #endregion

    }
}
