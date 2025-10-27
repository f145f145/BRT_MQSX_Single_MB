/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQZHWL.Model.DEV
 * 文件名：  MQZHDevModel_WYCtl
 * 版本号：  V1.0.0.0
 * 唯一标识：81457290-00c1-40f1-b8de-ae735534c3a8
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022-5-26 11:15:56
 * 描述：
 * 装置参数，位移控制
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022-5-26 11:15:56		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;

namespace MQZHWL.Model.DEV
{
    public partial class MQZH_DevModel_Main : ObservableObject
    {

        /// <summary>
        /// XY轴单周期时长（s）
        /// </summary>
        private double _cjbx_XYPeriod = 10;
        /// <summary>
        /// XY轴单周期时长（s）
        /// </summary>
        public double CJBX_XYPeriod
        {
            get { return _cjbx_XYPeriod; }
            set
            {
                _cjbx_XYPeriod = value;
                RaisePropertyChanged(() => CJBX_XYPeriod);
            }
        }

        /// <summary>
        /// Z轴单周期时长（s）
        /// </summary>
        private double _cjbx_ZPeriod = 60;
        /// <summary>
        /// Z轴单周期时长（s）
        /// </summary>
        public double CJBX_ZPeriod
        {
            get { return _cjbx_ZPeriod; }
            set
            {
                _cjbx_ZPeriod = value;
                RaisePropertyChanged(() => CJBX_ZPeriod);
            }
        }
        
        /// <summary>
        /// X轴位移长度系数
        /// </summary>
        private double _lenthRatio_X = 1;
        /// <summary>
        /// X轴位移长度系数
        /// </summary>
        public double LenthRatio_X
        {
            get { return _lenthRatio_X; }
            set
            {
                _lenthRatio_X = value;
                RaisePropertyChanged(() => LenthRatio_X);
            }
        }

        /// <summary>
        /// Y轴位移长度系数
        /// </summary>
        private double _lenthRatio_Y = 1;
        /// <summary>
        /// Y轴位移长度系数
        /// </summary>
        public double LenthRatio_Y
        {
            get { return _lenthRatio_Y; }
            set
            {
                _lenthRatio_Y = value;
                RaisePropertyChanged(() => LenthRatio_Y);
            }
        }

        /// <summary>
        /// Z轴位移长度系数
        /// </summary>
        private double _lenthRatio_Z = 1;
        /// <summary>
        /// Z轴位移长度系数
        /// </summary>
        public double LenthRatio_Z
        {
            get { return _lenthRatio_Z; }
            set
            {
                _lenthRatio_Z = value;
                RaisePropertyChanged(() => LenthRatio_Z);
            }
        }

        /// <summary>
        /// X轴定位允许误差（mm，距离在误差范围内无需移动）
        /// </summary>
        private double _permitErrX = 2;
        /// <summary>
        /// X轴定位允许误差（mm）
        /// </summary>
        public double PermitErrX
        {
            get { return _permitErrX; }
            set
            {
                _permitErrX = value;
                RaisePropertyChanged(() => PermitErrX);
            }
        }

        /// <summary>
        /// Y轴定位允许误差（mm，距离在误差范围内无需移动）
        /// </summary>
        private double _permitErrY = 2;
        /// <summary>
        /// X轴定位允许误差（mm）
        /// </summary>
        public double PermitErrY
        {
            get { return _permitErrY; }
            set
            {
                _permitErrY = value;
                RaisePropertyChanged(() => PermitErrY);
            }
        }

        /// <summary>
        /// Z轴定位允许误差（mm，距离在误差范围内无需移动）
        /// </summary>
        private double _permitErrZ = 2;
        /// <summary>
        /// X轴定位允许误差（mm）
        /// </summary>
        public double PermitErrZ
        {
            get { return _permitErrZ; }
            set
            {
                _permitErrZ = value;
                RaisePropertyChanged(() => PermitErrZ);
            }
        }

        /// <summary>
        /// X轴向右定位修正（真正目标值=设定值+修正值,mm）
        /// </summary>
        private double _corrXRight = 0;
        /// <summary>
        /// X轴向右定位修正（真正目标值=设定值+修正值,mm）
        /// </summary>
        public double CorrXRight
        {
            get { return _corrXRight; }
            set
            {
                _corrXRight = value;
                RaisePropertyChanged(() => CorrXRight);
            }
        }

        /// <summary>
        /// X轴向左定位修正（真正目标值=设定值+修正值,mm）
        /// </summary>
        private double _corrXLeft = 0;
        /// <summary>
        /// X轴向左定位修正（真正目标值=设定值+修正值,mm）
        /// </summary>
        public double CorrXLeft
        {
            get { return _corrXLeft; }
            set
            {
                _corrXLeft = value;
                RaisePropertyChanged(() => CorrXLeft);
            }
        }

        /// <summary>
        /// Y轴向前定位修正（真正目标值=设定值+修正值,mm）
        /// </summary>
        private double _corrYFront = 0;
        /// <summary>
        /// Y轴向前定位修正（真正目标值=设定值+修正值,mm）
        /// </summary>
        public double CorrYFront
        {
            get { return _corrYFront; }
            set
            {
                _corrYFront = value;
                RaisePropertyChanged(() => CorrYFront);
            }
        }

        /// <summary>
        /// Y轴向后定位修正（真正目标值=设定值+修正值,mm）
        /// </summary>
        private double _corrYBack = 0;
        /// <summary>
        /// Y轴向后定位修正（真正目标值=设定值+修正值,mm）
        /// </summary>
        public double CorrYBack
        {
            get { return _corrYBack; }
            set
            {
                _corrYBack = value;
                RaisePropertyChanged(() => CorrYBack);
            }
        }

        /// <summary>
        /// Z轴向上定位修正（真正目标值=设定值+修正值,mm）
        /// </summary>
        private double _corrZUp = 0;
        /// <summary>
        /// Z轴向上定位修正（真正目标值=设定值+修正值,mm）
        /// </summary>
        public double CorrZUp
        {
            get { return _corrZUp; }
            set
            {
                _corrZUp = value;
                RaisePropertyChanged(() => CorrZUp);
            }
        }

        /// <summary>
        /// Z轴向下定位修正（真正目标值=设定值+修正值,mm）
        /// </summary>
        private double _corrZDown = 0;
        /// <summary>
        /// Z轴向下定位修正（真正目标值=设定值+修正值,mm）
        /// </summary>
        public double CorrZDown
        {
            get { return _corrZDown; }
            set
            {
                _corrZDown = value;
                RaisePropertyChanged(() => CorrZDown);
            }
        }
    }
}
