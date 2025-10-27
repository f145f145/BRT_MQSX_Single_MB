/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQDFJ_MB.Model.ExpData
 * 文件名：  MQZH_ExpDataModel_QM
 * 版本号：  V1.0.0.0
 * 唯一标识：10b3b008-ba25-4219-b605-62ce111603b4
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/12/3 8:55:49
 * 描述：
 * 气密测试数据Model
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2021/12/3        8:55:49		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace MQDFJ_MB.Model.ExpData
{
    /// <summary>
    /// 基本气密检测项目数据
    /// </summary>
    public class MQZH_ExpDataModel_QM : ObservableObject
    {
        public MQZH_ExpDataModel_QM()
        {
            ExpData_QM_DJJC_Init();
            ExpData_QM_GCJC_Init();
        }


        #region 检测测试数据

        /// <summary>
        /// 气密定级渗透量检测值
        /// </summary>
        /// <remarks>1附加渗透量正压、3附加渗透量负压、5附加固定正压、7附加固定负压、9总渗透量正压、11总渗透量负压</remarks>
        private ObservableCollection<ObservableCollection<double>> _stl_QMDJ;
        /// <summary>
        /// 气密定级渗透量检测值
        /// </summary>
        /// <remarks>1附加渗透量正压、3附加渗透量负压、5附加固定正压、7附加固定负压、9总渗透量正压、11总渗透量负压</remarks>
        public ObservableCollection<ObservableCollection<double>> Stl_QMDJ
        {
            get { return _stl_QMDJ; }
            set
            {
                _stl_QMDJ = value;
                RaisePropertyChanged(() => Stl_QMDJ);
            }
        }

        /// <summary>
        /// 气密工程渗透量检测值
        /// </summary>
        /// <remarks>1附加渗透量正压、3附加渗透量负压、5附加固定正压、7附加固定负压、9总渗透量正压、11总渗透量负压</remarks>
        private ObservableCollection<ObservableCollection<double>> _stl_QMGC;
        /// <summary>
        /// 气密工程渗透量检测值
        /// </summary>
        /// <remarks>1附加渗透量正压、3附加渗透量负压、5附加固定正压、7附加固定负压、9总渗透量正压、11总渗透量负压</remarks>
        public ObservableCollection<ObservableCollection<double>> Stl_QMGC
        {
            get { return _stl_QMGC; }
            set
            {
                _stl_QMGC = value;
                RaisePropertyChanged(() => Stl_QMGC);
            }
        }

        #endregion


        #region 工程正压计算数据

        /// <summary>
        /// 工程测试压差、标准状态下附加渗透量-正压
        /// </summary>
        private double _qf1_GC_Z=0;
        /// <summary>
        /// 工程测试压差、标准状态下附加渗透量-正压
        /// </summary>
        public double Qf1_GC_Z
        {
            get { return _qf1_GC_Z; }
            set
            {
                _qf1_GC_Z = value;
                RaisePropertyChanged(() => Qf1_GC_Z);
            }
        }

        /// <summary>
        /// 工程测试压差、标准状态下附加及固定渗透量-正压
        /// </summary>
        private double _qfg1_GC_Z=0;
        /// <summary>
        /// 工程测试压差、标准状态下附加及固定渗透量-正压
        /// </summary>
        public double Qfg1_GC_Z
        {
            get { return _qfg1_GC_Z; }
            set
            {
                _qfg1_GC_Z = value;
                RaisePropertyChanged(() => Qfg1_GC_Z);
            }
        }

        /// <summary>
        /// 工程测试压差、标准状态下总渗透量-正压
        /// </summary>
        private double _qz1_GC_Z=0;
        /// <summary>
        /// 工程测试压差、标准状态下总渗透量-正压
        /// </summary>
        public double Qz1_GC_Z
        {
            get { return _qz1_GC_Z; }
            set
            {
                _qz1_GC_Z = value;
                RaisePropertyChanged(() => Qz1_GC_Z);
            }
        }

        /// <summary>
        /// 工程测试压差、标准状态下试件整体渗透量-正压
        /// </summary>
        private double _qs_GC_Z=0;
        /// <summary>
        /// 工程测试压差、标准状态下试件整体渗透量-正压
        /// </summary>
        public double Qs_GC_Z
        {
            get { return _qs_GC_Z; }
            set
            {
                _qs_GC_Z = value;
                RaisePropertyChanged(() => Qs_GC_Z);
            }
        }

        /// <summary>
        /// 工程测试压差、标准状态下试件可开启部分渗透量-正压
        /// </summary>
        private double _qk_GC_Z=0;
        /// <summary>
        /// 工程测试压差、标准状态下试件可开启部分渗透量-正压
        /// </summary>
        public double Qk_GC_Z
        {
            get { return _qk_GC_Z; }
            set
            {
                _qk_GC_Z = value;
                RaisePropertyChanged(() => Qk_GC_Z);
            }
        }

        /// <summary>
        /// 工程测试压差、标准状态下单位面积渗透量-正压
        /// </summary>
        private double _qA1_GC_Z=0;
        /// <summary>
        /// 工程测试压差、标准状态下单位面积渗透量-正压
        /// </summary>
        public double QA1_GC_Z
        {
            get { return _qA1_GC_Z; }
            set
            {
                _qA1_GC_Z = value;
                RaisePropertyChanged(() => QA1_GC_Z);
            }
        }

        /// <summary>
        /// 工程测试压差、标准状态下单位开启缝长渗透量-正压
        /// </summary>
        private double _ql1_GC_Z=0;
        /// <summary>
        /// 工程测试压差、标准状态下单位开启缝长渗透量-正压
        /// </summary>
        public double Ql1_GC_Z
        {
            get { return _ql1_GC_Z; }
            set
            {
                _ql1_GC_Z = value;
                RaisePropertyChanged(() => Ql1_GC_Z);
            }
        }

        #endregion


        #region 工程负压测试计算数据

        /// <summary>
        /// 工程测试压差、标准状态下附加渗透量-负压
        /// </summary>
        private double _qf1_GC_F=0;
        /// <summary>
        /// 工程测试压差、标准状态下附加渗透量-负压
        /// </summary>
        public double Qf1_GC_F
        {
            get { return _qf1_GC_F; }
            set
            {
                _qf1_GC_F = value;
                RaisePropertyChanged(() => Qf1_GC_F);
            }
        }

        /// <summary>
        /// 工程测试压差、标准状态下附加及固定渗透量-负压
        /// </summary>
        private double _qfg1_GC_F=0;
        /// <summary>
        /// 工程测试压差、标准状态下附加及固定渗透量-负压
        /// </summary>
        public double Qfg1_GC_F
        {
            get { return _qfg1_GC_F; }
            set
            {
                _qfg1_GC_F = value;
                RaisePropertyChanged(() => Qfg1_GC_F);
            }
        }

        /// <summary>
        /// 工程测试压差、标准状态下总渗透量-负压
        /// </summary>
        private double _qz1_GC_F=0;
        /// <summary>
        /// 工程测试压差、标准状态下总渗透量-负压
        /// </summary>
        public double Qz1_GC_F
        {
            get { return _qz1_GC_F; }
            set
            {
                _qz1_GC_F = value;
                RaisePropertyChanged(() => Qz1_GC_F);
            }
        }

        /// <summary>
        /// 工程测试压差、标准状态下试件整体渗透量-负压
        /// </summary>
        private double _qs_GC_F=0;
        /// <summary>
        /// 工程测试压差、标准状态下试件整体渗透量-负压
        /// </summary>
        public double Qs_GC_F
        {
            get { return _qs_GC_F; }
            set
            {
                _qs_GC_F = value;
                RaisePropertyChanged(() => Qs_GC_F);
            }
        }

        /// <summary>
        /// 工程测试压差、标准状态下试件可开启部分渗透量-负压
        /// </summary>
        private double _qk_GC_F=0;
        /// <summary>
        /// 工程测试压差、标准状态下试件可开启部分渗透量-负压
        /// </summary>
        public double Qk_GC_F
        {
            get { return _qk_GC_F; }
            set
            {
                _qk_GC_F = value;
                RaisePropertyChanged(() => Qk_GC_F);
            }
        }

        /// <summary>
        /// 工程测试压差、标准状态下单位面积渗透量-负压
        /// </summary>
        private double _qA1_GC_F=0;
        /// <summary>
        /// 工程测试压差、标准状态下单位面积渗透量-负压
        /// </summary>
        public double QA1_GC_F
        {
            get { return _qA1_GC_F; }
            set
            {
                _qA1_GC_F = value;
                RaisePropertyChanged(() => QA1_GC_F);
            }
        }

        /// <summary>
        /// 工程测试压差、标准状态下单位开启缝长渗透量-负压
        /// </summary>
        private double _ql1_GC_F=0;
        /// <summary>
        /// 工程测试压差、标准状态下单位开启缝长渗透量-负压
        /// </summary>
        public double Ql1_GC_F
        {
            get { return _ql1_GC_F; }
            set
            {
                _ql1_GC_F = value;
                RaisePropertyChanged(() => Ql1_GC_F);
            }
        }

        #endregion


        #region 定级正压计算数据

        /// <summary>
        /// 定级100Pa压差下附加渗透量平均值-正压
        /// </summary>
        private double _qfp_DJ_Z=0;
        /// <summary>
        /// 定级100Pa压差下附加渗透量平均值-正压
        /// </summary>
        public double Qfp_DJ_Z
        {
            get { return _qfp_DJ_Z; }
            set
            {
                _qfp_DJ_Z = value;
                RaisePropertyChanged(() => Qfp_DJ_Z);
            }
        }

        /// <summary>
        /// 定级100Pa压差下附加及固定渗透量平均值-正压
        /// </summary>
        private double _qfgp_DJ_Z=0;
        /// <summary>
        /// 定级100Pa压差下附加及固定渗透量平均值-正压
        /// </summary>
        public double Qfgp_DJ_Z
        {
            get { return _qfgp_DJ_Z; }
            set
            {
                _qfgp_DJ_Z = value;
                RaisePropertyChanged(() => Qfgp_DJ_Z);
            }
        }

        /// <summary>
        /// 定级100Pa压差下总渗透量平均值-正压
        /// </summary>
        private double _qzp_DJ_Z=0;
        /// <summary>
        /// 定级100Pa压差下总渗透量平均值-正压
        /// </summary>
        public double Qzp_DJ_Z
        {
            get { return _qzp_DJ_Z; }
            set
            {
                _qzp_DJ_Z = value;
                RaisePropertyChanged(() => Qzp_DJ_Z);
            }
        }

        /// <summary>
        /// 定级100Pa压差、标准状态下附加渗透量-正压
        /// </summary>
        private double _qf1_DJ_Z=0;
        /// <summary>
        /// 定级100Pa压差、标准状态下附加渗透量-正压
        /// </summary>
        public double Qf1_DJ_Z
        {
            get { return _qf1_DJ_Z; }
            set
            {
                _qf1_DJ_Z = value;
                RaisePropertyChanged(() => Qf1_DJ_Z);
            }
        }

        /// <summary>
        /// 定级100Pa压差、标准状态下附加及固定渗透量-正压
        /// </summary>
        private double _qfg1_DJ_Z=0;
        /// <summary>
        /// 定级100Pa压差、标准状态下附加及固定渗透量-正压
        /// </summary>
        public double Qfg1_DJ_Z
        {
            get { return _qfg1_DJ_Z; }
            set
            {
                _qfg1_DJ_Z = value;
                RaisePropertyChanged(() => Qfg1_DJ_Z);
            }
        }

        /// <summary>
        /// 定级100Pa压差、标准状态下总渗透量-正压
        /// </summary>
        private double _qz1_DJ_Z=0;
        /// <summary>
        /// 定级100Pa压差、标准状态下总渗透量-正压
        /// </summary>
        public double Qz1_DJ_Z
        {
            get { return _qz1_DJ_Z; }
            set
            {
                _qz1_DJ_Z = value;
                RaisePropertyChanged(() => Qz1_DJ_Z);
            }
        }

        /// <summary>
        /// 定级100Pa压差、标准状态下试件整体渗透量-正压
        /// </summary>
        private double _qs_DJ_Z=0;
        /// <summary>
        /// 定级100Pa压差、标准状态下试件整体渗透量-正压
        /// </summary>
        public double Qs_DJ_Z
        {
            get { return _qs_DJ_Z; }
            set
            {
                _qs_DJ_Z = value;
                RaisePropertyChanged(() => Qs_DJ_Z);
            }
        }

        /// <summary>
        /// 定级100Pa压差、标准状态下试件可开启部分渗透量-正压
        /// </summary>
        private double _qk_DJ_Z=0;
        /// <summary>
        /// 定级100Pa压差、标准状态下试件可开启部分渗透量-正压
        /// </summary>
        public double Qk_DJ_Z
        {
            get { return _qk_DJ_Z; }
            set
            {
                _qk_DJ_Z = value;
                RaisePropertyChanged(() => Qk_DJ_Z);
            }
        }

        /// <summary>
        /// 定级100Pa压差、标准状态下单位面积渗透量-正压
        /// </summary>
        private double _qA1_DJ_Z=0;
        /// <summary>
        /// 定级100Pa压差、标准状态下单位面积渗透量-正压
        /// </summary>
        public double QA1_DJ_Z
        {
            get { return _qA1_DJ_Z; }
            set
            {
                _qA1_DJ_Z = value;
                RaisePropertyChanged(() => QA1_DJ_Z);
            }
        }

        /// <summary>
        /// 定级100Pa压差、标准状态下单位开启缝长渗透量-正压
        /// </summary>
        private double _ql1_DJ_Z=0;
        /// <summary>
        /// 定级100Pa压差、标准状态下单位开启缝长渗透量-正压
        /// </summary>
        public double Ql1_DJ_Z
        {
            get { return _ql1_DJ_Z; }
            set
            {
                _ql1_DJ_Z = value;
                RaisePropertyChanged(() => Ql1_DJ_Z);
            }
        }

        /// <summary>
        /// 定级10Pa压差、标准状态下单位面积渗透量-正压
        /// </summary>
        private double _qA_DJ_Z=0;
        /// <summary>
        /// 定级10Pa压差、标准状态下单位面积渗透量-正压
        /// </summary>
        public double QA_DJ_Z
        {
            get { return _qA_DJ_Z; }
            set
            {
                _qA_DJ_Z = value;
                RaisePropertyChanged(() => QA_DJ_Z);
            }
        }

        /// <summary>
        /// 定级10Pa压差、标准状态下单位开启缝长渗透量-正压
        /// </summary>
        private double _ql_DJ_Z=0;
        /// <summary>
        /// 定级10Pa压差、标准状态下单位开启缝长渗透量-正压
        /// </summary>
        public double Ql_DJ_Z
        {
            get { return _ql_DJ_Z; }
            set
            {
                _ql_DJ_Z = value;
                RaisePropertyChanged(() => Ql_DJ_Z);
            }
        }

        #endregion


        #region 定级测试负压计算数据

        /// <summary>
        /// 定级100Pa压差下附加渗透量平均值-负压
        /// </summary>
        private double _qfp_DJ_F=0;
        /// <summary>
        /// 定级100Pa压差下附加渗透量平均值-负压
        /// </summary>
        public double Qfp_DJ_F
        {
            get { return _qfp_DJ_F; }
            set
            {
                _qfp_DJ_F = value;
                RaisePropertyChanged(() => Qfp_DJ_F);
            }
        }

        /// <summary>
        /// 定级100Pa压差下附加及固定渗透量平均值-负压
        /// </summary>
        private double _qfgp_DJ_F=0;
        /// <summary>
        /// 定级100Pa压差下附加及固定渗透量平均值-负压
        /// </summary>
        public double Qfgp_DJ_F
        {
            get { return _qfgp_DJ_F; }
            set
            {
                _qfgp_DJ_F = value;
                RaisePropertyChanged(() => Qfgp_DJ_F);
            }
        }

        /// <summary>
        /// 定级100Pa压差下总渗透量平均值-负压
        /// </summary>
        private double _qzp_DJ_F=0;
        /// <summary>
        /// 定级100Pa压差下总渗透量平均值-负压
        /// </summary>
        public double Qzp_DJ_F
        {
            get { return _qzp_DJ_F; }
            set
            {
                _qzp_DJ_F = value;
                RaisePropertyChanged(() => Qzp_DJ_F);
            }
        }

        /// <summary>
        /// 定级100Pa压差、标准状态下附加渗透量-负压
        /// </summary>
        private double _qf1_DJ_F=0;
        /// <summary>
        /// 定级100Pa压差、标准状态下附加渗透量-负压
        /// </summary>
        public double Qf1_DJ_F
        {
            get { return _qf1_DJ_F; }
            set
            {
                _qf1_DJ_F = value;
                RaisePropertyChanged(() => Qf1_DJ_F);
            }
        }

        /// <summary>
        /// 定级100Pa压差、标准状态下附加及固定渗透量-负压
        /// </summary>
        private double _qfg1_DJ_F=0;
        /// <summary>
        /// 定级100Pa压差、标准状态下附加及固定渗透量-负压
        /// </summary>
        public double Qfg1_DJ_F
        {
            get { return _qfg1_DJ_F; }
            set
            {
                _qfg1_DJ_F = value;
                RaisePropertyChanged(() => Qfg1_DJ_F);
            }
        }

        /// <summary>
        /// 定级100Pa压差、标准状态下总渗透量-负压
        /// </summary>
        private double _qz1_DJ_F=0;
        /// <summary>
        /// 定级100Pa压差、标准状态下总渗透量-负压
        /// </summary>
        public double Qz1_DJ_F
        {
            get { return _qz1_DJ_F; }
            set
            {
                _qz1_DJ_F = value;
                RaisePropertyChanged(() => Qz1_DJ_F);
            }
        }

        /// <summary>
        /// 定级100Pa压差、标准状态下试件整体渗透量-负压
        /// </summary>
        private double _qs_DJ_F=0;
        /// <summary>
        /// 定级100Pa压差、标准状态下试件整体渗透量-负压
        /// </summary>
        public double Qs_DJ_F
        {
            get { return _qs_DJ_F; }
            set
            {
                _qs_DJ_F = value;
                RaisePropertyChanged(() => Qs_DJ_F);
            }
        }

        /// <summary>
        /// 定级100Pa压差、标准状态下试件可开启部分渗透量-负压
        /// </summary>
        private double _qk_DJ_F=0;
        /// <summary>
        /// 定级100Pa压差、标准状态下试件可开启部分渗透量-负压
        /// </summary>
        public double Qk_DJ_F
        {
            get { return _qk_DJ_F; }
            set
            {
                _qk_DJ_F = value;
                RaisePropertyChanged(() => Qk_DJ_F);
            }
        }

        /// <summary>
        /// 定级100Pa压差、标准状态下单位面积渗透量-负压
        /// </summary>
        private double _qA1_DJ_F=0;
        /// <summary>
        /// 定级100Pa压差、标准状态下单位面积渗透量-负压
        /// </summary>
        public double QA1_DJ_F
        {
            get { return _qA1_DJ_F; }
            set
            {
                _qA1_DJ_F = value;
                RaisePropertyChanged(() => QA1_DJ_F);
            }
        }

        /// <summary>
        /// 定级100Pa压差、标准状态下单位开启缝长渗透量-负压
        /// </summary>
        private double _ql1_DJ_F=0;
        /// <summary>
        /// 定级100Pa压差、标准状态下单位开启缝长渗透量-负压
        /// </summary>
        public double Ql1_DJ_F
        {
            get { return _ql1_DJ_F; }
            set
            {
                _ql1_DJ_F = value;
                RaisePropertyChanged(() => Ql1_DJ_F);
            }
        }

        /// <summary>
        /// 定级10Pa压差、标准状态下单位面积渗透量-负压
        /// </summary>
        private double _qA_DJ_F=0;
        /// <summary>
        /// 定级10Pa压差、标准状态下单位面积渗透量-负压
        /// </summary>
        public double QA_DJ_F
        {
            get { return _qA_DJ_F; }
            set
            {
                _qA_DJ_F = value;
                RaisePropertyChanged(() => QA_DJ_F);
            }
        }

        /// <summary>
        /// 定级10Pa压差、标准状态下单位开启缝长渗透量-负压
        /// </summary>
        private double _ql_DJ_F=0;
        /// <summary>
        /// 定级10Pa压差、标准状态下单位开启缝长渗透量-负压
        /// </summary>
        public double Ql_DJ_F
        {
            get { return _ql_DJ_F; }
            set
            {
                _ql_DJ_F = value;
                RaisePropertyChanged(() => Ql_DJ_F);
            }
        }

        #endregion


        #region 定级正负压最不利数据
        
        /// <summary>
        /// 气密定级单位面积渗透量最不利值
        /// </summary>
        private double _qA_DJ_QM=0;
        /// <summary>
        /// 气密定级单位面积渗透量最不利值
        /// </summary>
        public double QA_DJ_QM
        {
            get { return _qA_DJ_QM; }
            set
            {
                _qA_DJ_QM = value;
                RaisePropertyChanged(() => QA_DJ_QM);
            }
        }

        /// <summary>
        /// 气密定级单位开启缝长渗透量最不利值
        /// </summary>
        private double _ql_DJ_QM=0;
        /// <summary>
        /// 气密定级单位开启缝长渗透量最不利值
        /// </summary>
        public double Ql_DJ_QM
        {
            get { return _ql_DJ_QM; }
            set
            {
                _ql_DJ_QM = value;
                RaisePropertyChanged(() => Ql_DJ_QM);
            }
        }

        #endregion


        #region 工程测试评定数据

        /// <summary>
        /// 单位面积渗透量是否满足工程设计要求-正压
        /// </summary>
        private bool _isMeetDesign_GC_ZQA=true ;
        /// <summary>
        /// 单位面积渗透量是否满足工程设计要求-正压
        /// </summary>
        public bool IsMeetDesign_GC_ZQA
        {
            get { return _isMeetDesign_GC_ZQA; }
            set
            {
                _isMeetDesign_GC_ZQA = value;
                RaisePropertyChanged(() => IsMeetDesign_GC_ZQA);
            }
        }

        /// <summary>
        /// 单位面积渗透量是否满足工程设计要求-负压
        /// </summary>
        private bool _isMeetDesign_GC_FQA=true ;
        /// <summary>
        /// 单位面积渗透量是否满足工程设计要求-负压
        /// </summary>
        public bool IsMeetDesign_GC_FQA
        {
            get { return _isMeetDesign_GC_FQA; }
            set
            {
                _isMeetDesign_GC_FQA = value;
                RaisePropertyChanged(() => IsMeetDesign_GC_FQA);
            }
        }

        /// <summary>
        /// 单位开启缝长渗透量是否满足工程设计要求-正压
        /// </summary>
        private bool _isMeetDesign_GC_ZQl=true ;
        /// <summary>
        /// 单位开启缝长渗透量是否满足工程设计要求-正压
        /// </summary>
        public bool IsMeetDesign_GC_ZQl
        {
            get { return _isMeetDesign_GC_ZQl; }
            set
            {
                _isMeetDesign_GC_ZQl = value;
                RaisePropertyChanged(() => IsMeetDesign_GC_ZQl);
            }
        }

        /// <summary>
        /// 单位开启缝长渗透量是否满足工程设计要求-负压
        /// </summary>
        private bool _isMeetDesign_GC_FQl=true ;
        /// <summary>
        /// 单位开启缝长渗透量是否满足工程设计要求-负压
        /// </summary>
        public bool IsMeetDesign_GC_FQl
        {
            get { return _isMeetDesign_GC_FQl; }
            set
            {
                _isMeetDesign_GC_FQl = value;
                RaisePropertyChanged(() => IsMeetDesign_GC_FQl);
            }
        }

        /// <summary>
        /// 气密性能是否满足工程设计要求
        /// </summary>
        private bool _isMeetDesign_GC_QM=true ;
        /// <summary>
        /// 气密性能是否满足工程设计要求-负压
        /// </summary>
        public bool IsMeetDesign_GC_QM
        {
            get { return _isMeetDesign_GC_QM; }
            set
            {
                _isMeetDesign_GC_QM = value;
                RaisePropertyChanged(() => IsMeetDesign_GC_QM);
            }
        }

        #endregion


        #region 定级测试评定数据

        /// <summary>
        /// 气密定级单位面积渗透量总级别
        /// </summary>
        private int _qALevel_DJ_QM = 0;
        /// <summary>
        /// 气密定级单位面积渗透量总级别
        /// </summary>
        public int QALevel_DJ_QM
        {
            get { return _qALevel_DJ_QM; }
            set
            {
                _qALevel_DJ_QM = value;
                RaisePropertyChanged(() => QALevel_DJ_QM);
            }
        }

        /// <summary>
        /// 气密定级单位开启缝长渗透量总级别
        /// </summary>
        private int _qlLevel_DJ_QM = 0;
        /// <summary>
        /// 气密定级单位开启缝长渗透量总级别
        /// </summary>
        public int QlLevel_DJ_QM
        {
            get { return _qlLevel_DJ_QM; }
            set
            {
                _qlLevel_DJ_QM = value;
                RaisePropertyChanged(() => QlLevel_DJ_QM);
            }
        }

        /// <summary>
        /// 气密定级正压单位面积渗透量级别
        /// </summary>
        private int _qALevel_DJZ_QM =0;
        /// <summary>
        /// 气密定级正压单位面积渗透量级别
        /// </summary>
        public int QALevel_DJZ_QM
        {
            get { return _qALevel_DJZ_QM; }
            set
            {
                _qALevel_DJZ_QM = value;
                RaisePropertyChanged(() => QALevel_DJZ_QM);
            }
        }

        /// <summary>
        /// 气密定级正压单位开启缝长渗透量级别
        /// </summary>
        private int _qlLevel_DJZ_QM =0;
        /// <summary>
        /// 气密定级正压单位开启缝长渗透量级别
        /// </summary>
        public int QlLevel_DJZ_QM
        {
            get { return _qlLevel_DJZ_QM; }
            set
            {
                _qlLevel_DJZ_QM = value;
                RaisePropertyChanged(() => QlLevel_DJZ_QM);
            }
        }

        /// <summary>
        /// 气密定级负压单位面积渗透量级别
        /// </summary>
        private int _qALevel_DJF_QM = 0;
        /// <summary>
        /// 气密定级负压单位面积渗透量级别
        /// </summary>
        public int QALevel_DJF_QM
        {
            get { return _qALevel_DJF_QM; }
            set
            {
                _qALevel_DJF_QM = value;
                RaisePropertyChanged(() => QALevel_DJF_QM);
            }
        }

        /// <summary>
        /// 气密定级负压单位开启缝长渗透量级别
        /// </summary>
        private int _qlLevel_DJF_QM = 0;
        /// <summary>
        /// 气密定级负压单位开启缝长渗透量级别
        /// </summary>
        public int QlLevel_DJF_QM
        {
            get { return _qlLevel_DJF_QM; }
            set
            {
                _qlLevel_DJF_QM = value;
                RaisePropertyChanged(() => QlLevel_DJF_QM);
            }
        }

        #endregion


        #region 初始化

        /// <summary>
        /// 定级检测数据初始化
        /// </summary>
        public void ExpData_QM_DJJC_Init()
        {
            Stl_QMDJ = new ObservableCollection<ObservableCollection<double>>();

            ObservableCollection<double> tempSTLDJqf_ZY = new ObservableCollection<double> { 0, 0, 0 };
            ObservableCollection<double> tempSTLDJqf_ZJ = new ObservableCollection<double> { 0, 0, 0, 0, 0 };
            ObservableCollection<double> tempSTLDJqf_FY = new ObservableCollection<double> { 0, 0, 0 };
            ObservableCollection<double> tempSTLDJqf_FJ = new ObservableCollection<double> { 0, 0, 0, 0, 0 };
            Stl_QMDJ.Add(tempSTLDJqf_ZY);
            Stl_QMDJ.Add(tempSTLDJqf_ZJ);
            Stl_QMDJ.Add(tempSTLDJqf_FY);
            Stl_QMDJ.Add(tempSTLDJqf_FJ);
            ObservableCollection<double> tempSTLDJqfg_ZY = new ObservableCollection<double> { 0, 0, 0 };
            ObservableCollection<double> tempSTLDJqfg_ZJ = new ObservableCollection<double> { 0, 0, 0, 0, 0 };
            ObservableCollection<double> tempSTLDJqfg_FY = new ObservableCollection<double> { 0, 0, 0 };
            ObservableCollection<double> tempSTLDJqfg_FJ = new ObservableCollection<double> { 0, 0, 0, 0, 0 };
            Stl_QMDJ.Add(tempSTLDJqfg_ZY);
            Stl_QMDJ.Add(tempSTLDJqfg_ZJ);
            Stl_QMDJ.Add(tempSTLDJqfg_FY);
            Stl_QMDJ.Add(tempSTLDJqfg_FJ);
            ObservableCollection<double> tempSTLDJqz_ZY = new ObservableCollection<double> { 0, 0, 0 };
            ObservableCollection<double> tempSTLDJqz_ZJ = new ObservableCollection<double> { 0, 0, 0, 0, 0 };
            ObservableCollection<double> tempSTLDJqz_FY = new ObservableCollection<double> { 0, 0, 0 };
            ObservableCollection<double> tempSTLDJqz_FJ = new ObservableCollection<double> { 0, 0, 0, 0, 0 };
            Stl_QMDJ.Add(tempSTLDJqz_ZY);
            Stl_QMDJ.Add(tempSTLDJqz_ZJ);
            Stl_QMDJ.Add(tempSTLDJqz_FY);
            Stl_QMDJ.Add(tempSTLDJqz_FJ);
        }
        //
        /// <summary>
        /// 工程检测数据初始化
        /// </summary>
        public void ExpData_QM_GCJC_Init()
        {
            Stl_QMGC = new ObservableCollection<ObservableCollection<double>>();

            ObservableCollection<double> tempSTLGCqf_ZY = new ObservableCollection<double> { 0, 0, 0 };
            ObservableCollection<double> tempSTLGCqf_ZJ = new ObservableCollection<double> { 0};
            ObservableCollection<double> tempSTLGCqf_FY = new ObservableCollection<double> { 0, 0, 0 };
            ObservableCollection<double> tempSTLGCqf_FJ = new ObservableCollection<double> { 0};
            Stl_QMGC.Add(tempSTLGCqf_ZY);
            Stl_QMGC.Add(tempSTLGCqf_ZJ);
            Stl_QMGC.Add(tempSTLGCqf_FY);
            Stl_QMGC.Add(tempSTLGCqf_FJ);
            ObservableCollection<double> tempSTLGCqfg_ZY = new ObservableCollection<double> { 0, 0, 0 };
            ObservableCollection<double> tempSTLGCqfg_ZJ = new ObservableCollection<double> { 0};
            ObservableCollection<double> tempSTLGCqfg_FY = new ObservableCollection<double> { 0, 0, 0 };
            ObservableCollection<double> tempSTLGCqfg_FJ = new ObservableCollection<double> { 0};
            Stl_QMGC.Add(tempSTLGCqfg_ZY);
            Stl_QMGC.Add(tempSTLGCqfg_ZJ);
            Stl_QMGC.Add(tempSTLGCqfg_FY);
            Stl_QMGC.Add(tempSTLGCqfg_FJ);
            ObservableCollection<double> tempSTLGCqz_ZY = new ObservableCollection<double> { 0, 0, 0 };
            ObservableCollection<double> tempSTLGCqz_ZJ = new ObservableCollection<double> { 0};
            ObservableCollection<double> tempSTLGCqz_FY = new ObservableCollection<double> { 0, 0, 0 };
            ObservableCollection<double> tempSTLGCqz_FJ = new ObservableCollection<double> { 0};
            Stl_QMGC.Add(tempSTLGCqz_ZY);
            Stl_QMGC.Add(tempSTLGCqz_ZJ);
            Stl_QMGC.Add(tempSTLGCqz_FY);
            Stl_QMGC.Add(tempSTLGCqz_FJ);
        }

        #endregion

    }
}
