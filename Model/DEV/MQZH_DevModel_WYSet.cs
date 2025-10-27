/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQZHWL.Model.DEV
 * 文件名：  MQZHDevModel_WYSet
 * 版本号：  V1.0.0.0
 * 唯一标识：e8aa8f34-44d8-44e3-8d1e-fd700a832100
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022-5-26 11:16:06
 * 描述：
 * 装置参数，位移设定
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022-5-26 11:16:06		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace MQZHWL.Model.DEV
{
    public partial class MQZH_DevModel_Main : ObservableObject
    {
        /// <summary>
        /// X轴检测各级控制位移角
        /// </summary>
        private ObservableCollection<double> _wyj_Ctl_X = new ObservableCollection<double>() { 400, 300, 200, 150, 100 };
        /// <summary>
        /// X轴检测各级控制位移角
        /// </summary>
        public ObservableCollection<double> WYJ_Ctl_X
        {
            get { return _wyj_Ctl_X; }
            set
            {
                _wyj_Ctl_X = value;
                RaisePropertyChanged(() => WYJ_Ctl_X);
            }
        }

        /// <summary>
        /// Y轴检测各级控制位移角
        /// </summary>
        private ObservableCollection<double> _wyj_Ctl_Y = new ObservableCollection<double>() { 400, 300, 200, 150, 100 };
        /// <summary>
        /// Y轴检测各级控制位移角
        /// </summary>
        public ObservableCollection<double> WYJ_Ctl_Y
        {
            get { return _wyj_Ctl_Y; }
            set
            {
                _wyj_Ctl_Y = value;
                RaisePropertyChanged(() => WYJ_Ctl_Y);
            }
        }

        /// <summary>
        /// Z轴检测各级控制位移量
        /// </summary>
        private ObservableCollection<double> _wy_Ctl_Z = new ObservableCollection<double>() { 400, 300, 200, 150, 100 };
        /// <summary>
        /// Z轴检测各级控制位移量
        /// </summary>
        public ObservableCollection<double> WY_Ctl_Z
        {
            get { return _wy_Ctl_Z; }
            set
            {
                _wy_Ctl_Z = value;
                RaisePropertyChanged(() => WY_Ctl_Z);
            }
        }

    }
}
