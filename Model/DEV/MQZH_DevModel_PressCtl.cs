/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQZHWL.Model.DEV
 * 文件名：  MQZHDevModel_PressCtl
 * 版本号：  V1.0.0.0
 * 唯一标识：e32d9d28-9588-4a54-9fc2-7bb3def5890e
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022-5-26 11:14:19
 * 描述：
 * 装置属性，压力控制部分。
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022-5-26 11:14:19		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using static MQZHWL.Model.MQZH_Enums;

namespace MQZHWL.Model.DEV
{
    public partial class MQZH_DevModel_Main : ObservableObject
    {

        #region 压力控制允许偏差

        /// <summary>
        /// 气密压力控制允许偏差
        /// </summary>
        /// <remarks>0~50，50~100，100~500，500以上</remarks>
        private ObservableCollection<double> _allowablePressErrQM = new ObservableCollection<double>() { 5, 8, 10, 20};
        /// <summary>
        /// 气密压力控制允许偏差
        /// </summary>
        /// <remarks>0~50，50~100，100~500，500以上</remarks>
        public ObservableCollection<double> AllowablePressErrQM
        {
            get { return _allowablePressErrQM; }
            set
            {
                _allowablePressErrQM = value;
                RaisePropertyChanged(() => AllowablePressErrQM);
            }
        }

        /// <summary>
        /// 大差压控制允许偏差
        /// </summary>
        /// <remarks>0~1000，1000~3000，3000~5000，5000以上</remarks>
        private ObservableCollection<double> _allowablePressErrBigCY = new ObservableCollection<double>() { 25, 50, 100, 100};
        /// <summary>
        /// 大差压控制允许偏差
        /// </summary>
        /// <remarks>0~1000，1000~3000，3000~5000，5000以上</remarks>
        public ObservableCollection<double> AllowablePressErrBigCY
        {
            get { return _allowablePressErrBigCY; }
            set
            {
                _allowablePressErrBigCY = value;
                RaisePropertyChanged(() => AllowablePressErrBigCY);
            }
        }

        #endregion


        #region 压力控制一般参数

        /// <summary>
        /// 加压泄压速度（Pa/S）(s)
        /// </summary>
        private double _loadUpDownSpeed = 100;
        /// <summary>
        /// 加压泄压速度（Pa/S）(s)
        /// </summary>
        public double LoadUpDownSpeed
        {
            get { return _loadUpDownSpeed; }
            set
            {
                _loadUpDownSpeed = value;
                RaisePropertyChanged(() => LoadUpDownSpeed);
            }
        }

        /// <summary>
        /// 预加压保持时间(s)
        /// </summary>
        private double _keepingTime_YJY = 3;
        /// <summary>
        /// 预加压保持时间(s)
        /// </summary>
        public double KeepingTime_YJY
        {
            get { return _keepingTime_YJY; }
            set
            {
                _keepingTime_YJY = value;
                RaisePropertyChanged(() => KeepingTime_YJY);
            }
        }

        #endregion


        #region 气密控压相关

        /// <summary>
        /// 气密步骤保持时间(s)
        /// </summary>
        private double _keepingTime_QMStep = 10;
        /// <summary>
        /// 气密步骤保持时间(s)
        /// </summary>
        public double KeepingTime_QMStep
        {
            get { return _keepingTime_QMStep; }
            set
            {
                _keepingTime_QMStep = value;
                RaisePropertyChanged(() => KeepingTime_QMStep);
            }
        }


        #endregion


        #region 水密控压相关

        /// <summary>
        /// 水密波动高压比例
        /// </summary>
        private double _highRatioSM = 1.25;
        /// <summary>
        /// 水密波动高压比例
        /// </summary>
        public double HighRatioSM
        {
            get { return _highRatioSM; }
            set
            {
                _highRatioSM = value;
                RaisePropertyChanged(() => HighRatioSM);
            }
        }

        /// <summary>
        /// 水密波动低压比例
        /// </summary>
        private double _lowRatioSM = 0.75;
        /// <summary>
        /// 水密波动低压比例
        /// </summary>
        public double LowRatioSM
        {
            get { return _lowRatioSM; }
            set
            {
                _lowRatioSM = value;
                RaisePropertyChanged(() => LowRatioSM);
            }
        }


        /// <summary>
        /// 水密波动压力准备保持时间(s)
        /// </summary>
        private double _keepingTime_SMPreparePress = 4;
        /// <summary>
        /// 水密波动压力准备保持时间(s)
        /// </summary>
        public double KeepingTime_SMPreparePress
        {
            get { return _keepingTime_SMPreparePress; }
            set
            {
                _keepingTime_SMPreparePress = value;
                RaisePropertyChanged(() => KeepingTime_SMPreparePress);
            }
        }

        /// <summary>
        /// 波动切换高压调频比率
        /// </summary>
        private double _phRatioWaveH = 0.3;
        /// <summary>
        /// 波动切换高压调频比率
        /// </summary>
        public double PhRatioWaveH
        {
            get { return _phRatioWaveH; }
            set
            {
                _phRatioWaveH = value;
                RaisePropertyChanged(() => PhRatioWaveH);
            }
        }

        /// <summary>
        /// 波动切换低压调频比率
        /// </summary>
        private double _phRatioWaveL = 0.3;
        /// <summary>
        /// 波动切换低压调频比率
        /// </summary>
        public double PhRatioWaveL
        {
            get { return _phRatioWaveL; }
            set
            {
                _phRatioWaveL = value;
                RaisePropertyChanged(() => PhRatioWaveL);
            }
        }

        /// <summary>
        /// 水密定级第一级保持时间(s)
        /// </summary>
        private double _keepingTime_SMFistStep = 20;
        /// <summary>
        /// 水密定级第一级保持时间(s)
        /// </summary>
        public double KeepingTime_SMFistStep
        {
            get { return _keepingTime_SMFistStep; }
            set
            {
                _keepingTime_SMFistStep = value;
                RaisePropertyChanged(() => KeepingTime_SMFistStep);
            }
        }

        /// <summary>
        /// 水密定级2-8级保持时间(s)
        /// </summary>
        private double _keepingTime_SMDJ_StepLeft = 60;
        /// <summary>
        /// 水密定级2-8级级保持时间(s)
        /// </summary>
        public double KeepingTime_SMDJ_StepLeft
        {
            get { return _keepingTime_SMDJ_StepLeft; }
            set
            {
                _keepingTime_SMDJ_StepLeft = value;
                RaisePropertyChanged(() => KeepingTime_SMDJ_StepLeft);
            }
        }

        /// <summary>
        /// 水密工程试验前喷水时间(s)
        /// </summary>
        private double _plTime_SMGC_Before = 600;
        /// <summary>
        /// 水密工程试验前喷水时间(s)
        /// </summary>
        public double PlTime_SMGC_Before
        {
            get { return _plTime_SMGC_Before; }
            set
            {
                _plTime_SMGC_Before = value;
                RaisePropertyChanged(() => PlTime_SMGC_Before);
            }
        }

        /// <summary>
        /// 水密工程可开启保持时间(s)
        /// </summary>
        private double _keepingTime_SMGC_KKQ = 900;
        /// <summary>
        /// 水密工程可开启保持时间(s)
        /// </summary>
        public double KeepingTime_SMGC_KKQ
        {
            get { return _keepingTime_SMGC_KKQ; }
            set
            {
                _keepingTime_SMGC_KKQ = value;
                RaisePropertyChanged(() => KeepingTime_SMGC_KKQ);
            }
        }

        /// <summary>
        /// 水密工程固定有开启保持时间(s)
        /// </summary>
        private double _keepingTime_SMGC_YKQ = 900;
        /// <summary>
        /// 水密工程固定有开启保持时间(s)
        /// </summary>
        public double KeepingTime_SMGC_YKQ
        {
            get { return _keepingTime_SMGC_YKQ; }
            set
            {
                _keepingTime_SMGC_YKQ = value;
                RaisePropertyChanged(() => KeepingTime_SMGC_YKQ);
            }
        }

        /// <summary>
        /// 水密工程固定无开启保持时间(s)
        /// </summary>
        private double _keepingTime_SMGC_WKQ = 1800;
        /// <summary>
        /// 水密工程固定无开启保持时间(s)
        /// </summary>
        public double KeepingTime_SMGC_WKQ
        {
            get { return _keepingTime_SMGC_WKQ; }
            set
            {
                _keepingTime_SMGC_WKQ = value;
                RaisePropertyChanged(() => KeepingTime_SMGC_WKQ);
            }
        }

        #endregion


        #region 抗风压控压相关

        /// <summary>
        /// 抗风压变形检测每级保持时间(s)
        /// </summary>
        private double _keepingTime_KFY_BXstep = 10;
        /// <summary>
        /// 抗风压变形检测每级保持时间(s)
        /// </summary>
        public double KeepingTime_KFY_BXstep
        {
            get { return _keepingTime_KFY_BXstep; }
            set
            {
                _keepingTime_KFY_BXstep = value;
                RaisePropertyChanged(() => KeepingTime_KFY_BXstep);
            }
        }

        /// <summary>
        /// 抗风压安全检测保持时间（p3,pmax）(s)
        /// </summary>
        private double _keepingTime_KFY_AQ = 3;
        /// <summary>
        /// 抗风压安全检测保持时间（p3,pmax）(s)
        /// </summary>
        public double KeepingTime_KFY_AQ
        {
            get { return _keepingTime_KFY_AQ; }
            set
            {
                _keepingTime_KFY_AQ = value;
                RaisePropertyChanged(() => KeepingTime_KFY_AQ);
            }
        }

        /// <summary>
        /// 抗风压反复波动次数(s)
        /// </summary>
        private int _waveNum_KFYP2 = 10;
        /// <summary>
        /// 抗风压反复波动次数(s)
        /// </summary>
        public int WaveNum_KFYP2
        {
            get { return _waveNum_KFYP2; }
            set
            {
                _waveNum_KFYP2 = value;
                RaisePropertyChanged(() => WaveNum_KFYP2);
            }
        }

        /// <summary>
        /// 抗风压安全加压泄压速度（Pa/s，p3,pmax）(s)
        /// </summary>
        private double _loadUpDownSpeed_KFRAQ = 300;
        /// <summary>
        /// 抗风压安全加压泄压速度（Pa/s，p3,pmax）(s)
        /// </summary>
        public double LoadUpDownSpeed_KFRAQ
        {
            get { return _loadUpDownSpeed_KFRAQ; }
            set
            {
                _loadUpDownSpeed_KFRAQ = value;
                RaisePropertyChanged(() => LoadUpDownSpeed_KFRAQ);
            }
        }

        /// <summary>
        /// 抗风压波动高压比例
        /// </summary>
        private double _highRatioKFY = 1.25;
        /// <summary>
        /// 抗风压波动高压比例
        /// </summary>
        public double HighRatioKFY
        {
            get { return _highRatioKFY; }
            set
            {
                _highRatioKFY = value;
                RaisePropertyChanged(() => HighRatioKFY);
            }
        }

        /// <summary>
        /// 抗风压波动低压比例
        /// </summary>
        private double _lowRatioKFY = 0.75;
        /// <summary>
        /// 抗风压波动低压比例
        /// </summary>
        public double LowRatioKFY
        {
            get { return _lowRatioKFY; }
            set
            {
                _lowRatioKFY = value;
                RaisePropertyChanged(() => LowRatioKFY);
            }
        }

        #endregion
    }
}