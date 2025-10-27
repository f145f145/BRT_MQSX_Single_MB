/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQZHWL.Model.DEV
 * 文件名：  MQZH_DevModel_RunParam
 * 版本号：  V1.0.0.0
 * 唯一标识：d3d96926-9a29-4050-ae1b-ee90aca8ddb1
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022-5-26 10:55:39
 * 描述：
 *
 * ==================================================================================
 * 修改标记
 * 修改时间				修改人			版本号			描述
 * 2022-5-26 10:55:39		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using static MQZHWL.Model.MQZH_Enums;

namespace MQZHWL.Model.DEV
{
    public partial class MQZH_DevModel_Main : ObservableObject
    {
        /// <summary>
        /// 装置占用状态
        /// </summary>
        private bool _isdeviceBusy = false;
        /// <summary>
        /// 装置占用状态
        /// </summary>
        public bool IsDeviceBusy
        {
            get { return _isdeviceBusy; }
            set
            {
                _isdeviceBusy = value;
                RaisePropertyChanged(() => IsDeviceBusy);
                RaisePropertyChanged(() => IsNotBusy);
            }
        }
        /// <summary>
        /// 装置非占用状态
        /// </summary>
        public bool IsNotBusy
        {
            get { return !_isdeviceBusy; }
        }

        /// <summary>
        /// 装置运行模式
        /// </summary>
        private DevicRunModeType _deviceRunMode = DevicRunModeType.Wait_Mode;
        /// <summary>
        /// 装置运行模式
        /// </summary>
        public DevicRunModeType DeviceRunMode
        {
            get { return _deviceRunMode; }
            set
            {
                _deviceRunMode = value;
                RaisePropertyChanged(() => DeviceRunMode);
            }
        }

        /// <summary>
        /// 开机时间
        /// </summary>
        private readonly DateTime _powerOnTime = DateTime.Now;
        /// <summary>
        /// 开机时间
        /// </summary>
        public DateTime PowerOnTime
        {
            get { return _powerOnTime; }
        }


        #region 计数器


        /// <summary>
        /// 阀门重新自检指令发送计数器
        /// </summary>
        private  int _cmdSendCounter_FFST = 0;
        /// <summary>
        /// 阀门重新自检指令发送计数器
        /// </summary>
        public int CmdSendCounter_FFST
        {
            get { return _cmdSendCounter_FFST; }
            set
            {
                _cmdSendCounter_FFST = value;
                RaisePropertyChanged(() => CmdSendCounter_FFST);
            }
        }


        /// <summary>
        /// 阀门复位指令发送计数器
        /// </summary>
        private int _cmdSendCounter_FFRest = 0;
        /// <summary>
        /// 阀门复位指令发送计数器
        /// </summary>
        public int CmdSendCounter_FFRest
        {
            get { return _cmdSendCounter_FFRest; }
            set
            {
                _cmdSendCounter_FFRest = value;
                RaisePropertyChanged(() => CmdSendCounter_FFRest);
            }
        }


        /// <summary>
        /// 管理员身份
        /// </summary>
        private bool _isAdmin = false;
        /// <summary>
        /// 管理员身份
        /// </summary>
        public bool IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                _isAdmin = value;
                RaisePropertyChanged(() => IsAdmin);
            }
        }


        /// <summary>
        /// 输入的管理员登录密码，正确密码为brt12345678
        /// </summary> 
        private string _passWord = "";
        /// <summary>
        /// 输入的管理员登录密码，正确密码为brt12345678
        /// </summary>
        public string PassWord
        {
            get { return _passWord; }
            set
            {
                _passWord = value;
                RaisePropertyChanged(() => PassWord);
            }
        }


        /// <summary>
        /// 外喷淋位移尺位移
        /// </summary> 
        private ObservableCollection<double> _wyWPL = new ObservableCollection<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// 外喷淋位移尺位移
        /// </summary>
        public ObservableCollection<double> WyWPL
        {
            get { return _wyWPL; }
            set
            {
                _wyWPL = value;
                RaisePropertyChanged(() => WyWPL);
            }
        }
        #endregion
    }
}