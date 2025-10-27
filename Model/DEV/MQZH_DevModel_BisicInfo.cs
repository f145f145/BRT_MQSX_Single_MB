/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQDFJ_MB.Model
 * 文件名：  MQZH_DevModel_BisicInfo
 * 版本号：  V1.0.0.0
 * 唯一标识：04728a36-efc8-44c5-8286-d42041618c58
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022-5-26 10:53:00
 * 描述：
 * 装置Model，装置基本信息部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022-5-26 10:53:00		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;

namespace MQDFJ_MB.Model.DEV
{
    public partial class MQZH_DevModel_Main : ObservableObject
    {
        /// <summary>
        /// 装置编号
        /// </summary>
        private string _deviceID = "MQZH01A";
        /// <summary>
        /// 装置ID
        /// </summary>
        public string DeviceID
        {
            get { return _deviceID; }
            set
            {
                _deviceID = value;
                RaisePropertyChanged(() => DeviceID);
            }
        }

        /// <summary>
        /// 装置名称
        /// </summary>
        private string _deviceName = "建筑幕墙综合物理性能检测设备";
        /// <summary>
        /// 装置名称
        /// </summary>
        public string DeviceName
        {
            get { return _deviceName; }
            set
            {
                _deviceName = value;
                RaisePropertyChanged(() => DeviceName);
            }
        }


        /// <summary>
        /// 装置出厂编号
        /// </summary>
        private string _deviceSerialNO = "MQ2201";
        /// <summary>
        /// 装置出厂编号
        /// </summary>
        public string DeviceSerialNO
        {
            get { return _deviceSerialNO; }
            set
            {
                _deviceSerialNO = value;
                RaisePropertyChanged(() => DeviceSerialNO);
            }
        }

        /// <summary>
        /// 装置设备型号
        /// </summary>
        private string _deviceTypeNO = "MQ-60-90";
        /// <summary>
        /// 装置设备型号
        /// </summary>
        public string DeviceTypeNO
        {
            get { return _deviceTypeNO; }
            set
            {
                _deviceTypeNO = value;
                RaisePropertyChanged(() => DeviceTypeNO);
            }
        }

        /// <summary>
        /// 气密试验方法标准
        /// </summary>
        private string _deviceQMStd = "《GB//T 15227-2019 建筑幕墙气密、水密、抗风压性能检测方法》";
        /// <summary>
        /// 气密试验方法标准
        /// </summary>
        public string DeviceQMStd
        {
            get { return _deviceQMStd; }
            set
            {
                _deviceQMStd = value;
                RaisePropertyChanged(() => DeviceQMStd);
            }
        }

        /// <summary>
        /// 水密试验方法标准
        /// </summary>
        private string _deviceSMStd = "《GB//T 15227-2019 建筑幕墙气密、水密、抗风压性能检测方法》";
        /// <summary>
        /// 水密试验方法标准
        /// </summary>
        public string DeviceSMStd
        {
            get { return _deviceSMStd; }
            set
            {
                _deviceSMStd = value;
                RaisePropertyChanged(() => DeviceSMStd);
            }
        }

        /// <summary>
        /// 抗风压试验方法标准
        /// </summary>
        private string _deviceKFYStd = "《GB//T 15227-2019 建筑幕墙气密、水密、抗风压性能检测方法》";
        /// <summary>
        /// 抗风压试验方法标准
        /// </summary>
        public string DeviceKFYStd
        {
            get { return _deviceKFYStd; }
            set
            {
                _deviceKFYStd = value;
                RaisePropertyChanged(() => DeviceKFYStd);
            }
        }

        /// <summary>
        /// 层间变形试验方法标准
        /// </summary>
        private string _deviceCJBXStd = "《GB//T 18250-2015 建筑幕墙层间变形性能分级及检测方法》";
        /// <summary>
        /// 层间变形试验方法标准
        /// </summary>
        public string DeviceCJBXStd
        {
            get { return _deviceCJBXStd; }
            set
            {
                _deviceCJBXStd = value;
                RaisePropertyChanged(() => DeviceCJBXStd);
            }
        }
    }
}
