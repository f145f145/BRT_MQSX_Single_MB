/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQZHWL.Model.DEV
 * 文件名：  MQZH_DevModel_PressSet
 * 版本号：  V1.0.0.0
 * 唯一标识：d688e083-00f4-432a-acc4-a24c2c7358e6
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022-5-26 11:12:18
 * 描述：
 * 装置参数，压力设定部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022-5-26 11:12:18		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace MQZHWL.Model.DEV
{
    public partial class MQZH_DevModel_Main : ObservableObject
    {
        /// <summary>
        /// 是否使用单独设定压力（试验配置和控制目标显示为标准值，实际检测值另行设置）
        /// </summary>
        private bool _isPressFalse = true;
        /// <summary>
        /// 是否使用单独设定压力（试验配置和控制目标显示为标准值，实际检测值另行设置）
        /// </summary>
        public bool IsPressFalse
        {
            get { return _isPressFalse; }
            set
            {
                _isPressFalse = value;
                RaisePropertyChanged(() => IsPressFalse);
            }
        }

        #region 气密

        /// <summary>
        /// 气密定级检测标准压力
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _pressSet_QMDJ_Std;
        /// <summary>
        /// 气密定级检测标准压力
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> PressSet_QMDJ_Std
        {
            get { return _pressSet_QMDJ_Std; }
            set
            {
                _pressSet_QMDJ_Std = value;
                RaisePropertyChanged(() => PressSet_QMDJ_Std);
            }
        }

        /// <summary>
        /// 气密定级检测虚假压力
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _pressSet_QMDJ_False;
        /// <summary>
        /// 气密定级检测虚假压力
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> PressSet_QMDJ_False
        {
            get { return _pressSet_QMDJ_False; }
            set
            {
                _pressSet_QMDJ_False = value;
                RaisePropertyChanged(() => PressSet_QMDJ_False);
            }
        }

        /// <summary>
        /// 气密工程检测虚假压力
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _pressSet_QMGC_False;
        /// <summary>
        /// 气密工程检测虚假压力
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> PressSet_QMGC_False
        {
            get { return _pressSet_QMGC_False; }
            set
            {
                _pressSet_QMGC_False = value;
                RaisePropertyChanged(() => PressSet_QMGC_False);
            }
        }

        /// <summary>
        /// 气密检测压力初始化
        /// </summary>
        private void PressSet_QM_Init()
        {
            //定级标准值
            PressSet_QMDJ_Std = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempDSZY = new ObservableCollection<double>() { 500, 500, 500 };
            ObservableCollection<double> tempDSZJ = new ObservableCollection<double>() { 50, 100, 150, 100, 50 };
            ObservableCollection<double> tempDSFY = new ObservableCollection<double>() { -500, -500, -500 };
            ObservableCollection<double> tempDSFJ = new ObservableCollection<double>() { -50, -100, -150, -100, -50 };
            PressSet_QMDJ_Std.Add(tempDSZY);
            PressSet_QMDJ_Std.Add(tempDSZJ);
            PressSet_QMDJ_Std.Add(tempDSFY);
            PressSet_QMDJ_Std.Add(tempDSFJ);
            PressSet_QMDJ_Std.Add(tempDSZY);
            PressSet_QMDJ_Std.Add(tempDSZJ);
            PressSet_QMDJ_Std.Add(tempDSFY);
            PressSet_QMDJ_Std.Add(tempDSFJ);
            PressSet_QMDJ_Std.Add(tempDSZY);
            PressSet_QMDJ_Std.Add(tempDSZJ);
            PressSet_QMDJ_Std.Add(tempDSFY);
            PressSet_QMDJ_Std.Add(tempDSFJ);

            //定级虚假值
            PressSet_QMDJ_False = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempDFZY = new ObservableCollection<double>() { 50, 50, 50 };
            ObservableCollection<double> tempDFZJ = new ObservableCollection<double>() { 20, 50, 50, 35, 20 };
            ObservableCollection<double> tempDFFY = new ObservableCollection<double>() { -50, -50, -50 };
            ObservableCollection<double> tempDFFJ = new ObservableCollection<double>() { -20, -35, -50, -35, -20 };
            PressSet_QMDJ_False.Add(tempDFZY);
            PressSet_QMDJ_False.Add(tempDFZJ);
            PressSet_QMDJ_False.Add(tempDFFY);
            PressSet_QMDJ_False.Add(tempDFFJ);
            PressSet_QMDJ_False.Add(tempDFZY);
            PressSet_QMDJ_False.Add(tempDFZJ);
            PressSet_QMDJ_False.Add(tempDFFY);
            PressSet_QMDJ_False.Add(tempDFFJ);
            PressSet_QMDJ_False.Add(tempDFZY);
            PressSet_QMDJ_False.Add(tempDFZJ);
            PressSet_QMDJ_False.Add(tempDFFY);
            PressSet_QMDJ_False.Add(tempDFFJ);

            //工程虚假值
            PressSet_QMGC_False = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempGFZY = new ObservableCollection<double>() { 50, 50, 50 };
            ObservableCollection<double> tempGFZJ = new ObservableCollection<double>() { 50 };
            ObservableCollection<double> tempGFFY = new ObservableCollection<double>() { -50, -50, -50 };
            ObservableCollection<double> tempGFFJ = new ObservableCollection<double>() { -50 };
            PressSet_QMGC_False.Add(tempGFZY);
            PressSet_QMGC_False.Add(tempGFZJ);
            PressSet_QMGC_False.Add(tempGFFY);
            PressSet_QMGC_False.Add(tempGFFJ);
            PressSet_QMGC_False.Add(tempGFZY);
            PressSet_QMGC_False.Add(tempGFZJ);
            PressSet_QMGC_False.Add(tempGFFY);
            PressSet_QMGC_False.Add(tempGFFJ);
            PressSet_QMGC_False.Add(tempGFZY);
            PressSet_QMGC_False.Add(tempGFZJ);
            PressSet_QMGC_False.Add(tempGFFY);
            PressSet_QMGC_False.Add(tempGFFJ);

        }

        #endregion

        #region 水密

        /// <summary>
        /// 水密预加压标准压力
        /// </summary>
        private ObservableCollection<double> _pressSet_SM_YJY_Std = new ObservableCollection<double>() { 500, 500, 500 };
        /// <summary>
        /// 水密预加压标准压力
        /// </summary>
        public ObservableCollection<double> PressSet_SM_YJY_Std
        {
            get { return _pressSet_SM_YJY_Std; }
            set
            {
                _pressSet_SM_YJY_Std = value;
                RaisePropertyChanged(() => PressSet_SM_YJY_Std);
            }
        }

        /// <summary>
        /// 水密预加压虚假压力
        /// </summary>
        private ObservableCollection<double> _pressSet_SM_YJY_False = new ObservableCollection<double>() { 50, 50, 50 };
        /// <summary>
        /// 水密预加压虚假压力
        /// </summary>
        public ObservableCollection<double> PressSet_SM_YJY_False
        {
            get { return _pressSet_SM_YJY_False; }
            set
            {
                _pressSet_SM_YJY_False = value;
                RaisePropertyChanged(() => PressSet_SM_YJY_False);
            }
        }

        /// <summary>
        /// 水密定级稳定检测标准压力
        /// </summary>
        private ObservableCollection<double> _pressSet_SMDJ_WD_Std = new ObservableCollection<double>() { 0, 250, 350, 500, 700, 1000, 1500, 2000 };
        /// <summary>
        /// 水密定级稳定检测标准压力
        /// </summary>
        public ObservableCollection<double> PressSet_SMDJ_WD_Std
        {
            get { return _pressSet_SMDJ_WD_Std; }
            set
            {
                _pressSet_SMDJ_WD_Std = value;
                RaisePropertyChanged(() => PressSet_SMDJ_WD_Std);
            }
        }

        /// <summary>
        /// 水密定级稳定检测虚假压力
        /// </summary>
        private ObservableCollection<double> _pressSet_SMDJ_WD_False = new ObservableCollection<double>() { 0, 20, 30, 40, 50, 60, 70, 80 };
        /// <summary>
        /// 水密定级稳定检测虚假压力
        /// </summary>
        public ObservableCollection<double> PressSet_SMDJ_WD_False
        {
            get { return _pressSet_SMDJ_WD_False; }
            set
            {
                _pressSet_SMDJ_WD_False = value;
                RaisePropertyChanged(() => PressSet_SMDJ_WD_False);
            }
        }

        /// <summary>
        /// 水密定级波动检测平均压力标准
        /// </summary>
        private ObservableCollection<double> _pressSet_SMDJ_BDPJ_Std = new ObservableCollection<double>() { 0, 250, 350, 500, 700, 1000, 1500, 2000 };
        /// <summary>
        /// 水密定级波动检测平均压力标准
        /// </summary>
        public ObservableCollection<double> PressSet_SMDJ_BDPJ_Std
        {
            get { return _pressSet_SMDJ_BDPJ_Std; }
            set
            {
                _pressSet_SMDJ_BDPJ_Std = value;
                RaisePropertyChanged(() => PressSet_SMDJ_BDPJ_Std);
            }
        }

        /// <summary>
        /// 水密定级稳定检测平均压力虚假
        /// </summary>
        private ObservableCollection<double> _pressSet_SMDJ_BDPJ_False = new ObservableCollection<double>() { 0, 20, 30, 40, 50, 60, 70, 80 };
        /// <summary>
        /// 水密定级稳定检测平均压力虚假
        /// </summary>
        public ObservableCollection<double> PressSet_SMDJ_BDPJ_False
        {
            get { return _pressSet_SMDJ_BDPJ_False; }
            set
            {
                _pressSet_SMDJ_BDPJ_False = value;
                RaisePropertyChanged(() => PressSet_SMDJ_BDPJ_False);
            }
        }

        /// <summary>
        /// 水密工程稳定可开启水密压力虚假
        /// </summary>
        private double pressSet_SMGC_WDKKQ_False = 40;
        /// <summary>
        /// 水密工程稳定可开启水密压力虚假
        /// </summary>
        public double PressSet_SMGC_WDKKQ_False
        {
            get { return pressSet_SMGC_WDKKQ_False; }
            set
            {
                pressSet_SMGC_WDKKQ_False = value;
                RaisePropertyChanged(() => PressSet_SMGC_WDKKQ_False);
            }
        }

        /// <summary>
        /// 水密工程稳定固定水密压力虚假
        /// </summary>
        private double pressSet_SMGC_WDGD_False = 50;
        /// <summary>
        /// 水密工程稳定固定水密压力虚假
        /// </summary>
        public double PressSet_SMGC_WDGD_False
        {
            get { return pressSet_SMGC_WDGD_False; }
            set
            {
                pressSet_SMGC_WDGD_False = value;
                RaisePropertyChanged(() => PressSet_SMGC_WDGD_False);
            }
        }

        /// <summary>
        /// 水密工程波动可开启水密压力平均值虚假
        /// </summary>
        private double _pressSet_SMGC_BDPJKKQ_False = 40;
        /// <summary>
        /// 水密工程波动可开启水密压力平均值虚假
        /// </summary>
        public double PressSet_SMGC_BDPJKKQ_False
        {
            get { return _pressSet_SMGC_BDPJKKQ_False; }
            set
            {
                _pressSet_SMGC_BDPJKKQ_False = value;
                RaisePropertyChanged(() => PressSet_SMGC_BDPJKKQ_False);
            }
        }

        /// <summary>
        /// 水密工程波动固定水密压力平均值虚假
        /// </summary>
        private double _pressSet_SMGC_BDPJGD_False = 50;
        /// <summary>
        /// 水密工程波动固定水密压力平均值虚假
        /// </summary>
        public double PressSet_SMGC_BDPJGD_False
        {
            get { return _pressSet_SMGC_BDPJGD_False; }
            set
            {
                _pressSet_SMGC_BDPJGD_False = value;
                RaisePropertyChanged(() => PressSet_SMGC_BDPJGD_False);
            }
        }

        #endregion

        #region 抗风压定级

        /// <summary>
        /// 抗风压定级变形标准压力
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _pressSet_KFY_DJBX_Std;
        /// <summary>
        /// 抗风压定级变形标准压力
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> PressSet_KFY_DJBX_Std
        {
            get { return _pressSet_KFY_DJBX_Std; }
            set
            {
                _pressSet_KFY_DJBX_Std = value;
                RaisePropertyChanged(() => PressSet_KFY_DJBX_Std);
            }
        }

        /// <summary>
        /// 抗风压定级变形虚假压力
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _pressSet_KFY_DJBX_False;
        /// <summary>
        /// 抗风压定级变形虚假压力
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> PressSet_KFY_DJBX_False
        {
            get { return _pressSet_KFY_DJBX_False; }
            set
            {
                _pressSet_KFY_DJBX_False = value;
                RaisePropertyChanged(() => PressSet_KFY_DJBX_False);
            }
        }

        /// <summary>
        /// 抗风压定级反复加压倍数标准值
        /// </summary>
        private double _pressSet_KFY_DJP2_Raito_Std = 1.5;
        /// <summary>
        /// 抗风压定级反复加压倍数标准值
        /// </summary>
        public double PressSet_KFY_DJP2_Raito_Std
        {
            get { return _pressSet_KFY_DJP2_Raito_Std; }
            set
            {
                _pressSet_KFY_DJP2_Raito_Std = value;
                RaisePropertyChanged(() => PressSet_KFY_DJP2_Raito_Std);
            }
        }

        /// <summary>
        /// 抗风压定级反复加压压力虚假值
        /// </summary>
        private double _pressSet_KFY_DJP2_False = 500;
        /// <summary>
        /// 抗风压定级反复加压压力虚假值
        /// </summary>
        public double PressSet_KFY_DJP2_False
        {
            get { return _pressSet_KFY_DJP2_False; }
            set
            {
                _pressSet_KFY_DJP2_False = value;
                RaisePropertyChanged(() => PressSet_KFY_DJP2_False);
            }
        }

        /// <summary>
        /// 抗风压定级P3加压倍数标准值
        /// </summary>
        private double _pressSet_KFY_DJP3_Raito_Std = 2.5;
        /// <summary>
        /// 抗风压定级P3加压倍数标准值
        /// </summary>
        public double PressSet_KFY_DJP3_Raito_Std
        {
            get { return _pressSet_KFY_DJP3_Raito_Std; }
            set
            {
                _pressSet_KFY_DJP3_Raito_Std = value;
                RaisePropertyChanged(() => PressSet_KFY_DJP3_Raito_Std);
            }
        }

        /// <summary>
        /// 抗风压定级P3加压压力虚假值
        /// </summary>
        private double _pressSet_KFY_DJP3_False = 500;
        /// <summary>
        /// 抗风压定级P3加压压力虚假值
        /// </summary>
        public double PressSet_KFY_DJP3_False
        {
            get { return _pressSet_KFY_DJP3_False; }
            set
            {
                _pressSet_KFY_DJP3_False = value;
                RaisePropertyChanged(() => PressSet_KFY_DJP3_False);
            }
        }

        /// <summary>
        /// 抗风压定级Pmax加压倍数标准值
        /// </summary>
        private double _pressSet_KFY_DJPmax_Raito_Std = 1.4;
        /// <summary>
        /// 抗风压定级Pmax加压倍数标准值
        /// </summary>
        public double PressSet_KFY_DJPmax_Raito_Std
        {
            get { return _pressSet_KFY_DJPmax_Raito_Std; }
            set
            {
                _pressSet_KFY_DJPmax_Raito_Std = value;
                RaisePropertyChanged(() => PressSet_KFY_DJPmax_Raito_Std);
            }
        }

        /// <summary>
        /// 抗风压定级Pmax加压压力虚假值
        /// </summary>
        private double _pressSet_KFY_DJPmax_False = 500;
        /// <summary>
        /// 抗风压定级Pmax加压压力虚假值
        /// </summary>
        public double PressSet_KFY_DJPmax_False
        {
            get { return _pressSet_KFY_DJPmax_False; }
            set
            {
                _pressSet_KFY_DJPmax_False = value;
                RaisePropertyChanged(() => PressSet_KFY_DJPmax_False);
            }
        }


        /// <summary>
        ///抗风压检测定级压力初始化
        /// </summary>
        private void PressSet_KFYDJ_Init()
        {
            //定级变形标准值
            PressSet_KFY_DJBX_Std = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempDJBXDZY = new ObservableCollection<double>() { 500, 500, 500 };
            ObservableCollection<double> tempDJBXDZJ = new ObservableCollection<double>() { 100,200,300,400,500,600,700,800,900,
                1000,1100,1200,1300, 1400,1500,1600,1700,1800,1900,2000 };
            ObservableCollection<double> tempDJBXDFY = new ObservableCollection<double>() { -500, -500, -500 };
            ObservableCollection<double> tempDJBX = new ObservableCollection<double>() { -100,-200,-300,-400,-500,-600,-700,-800,-900,
                -1000,-1100,-1200,-1300, -1400,-1500,-1600,-1700,-1800,-1900,-2000 };
            PressSet_KFY_DJBX_Std.Add(tempDJBXDZY);
            PressSet_KFY_DJBX_Std.Add(tempDJBXDZJ);
            PressSet_KFY_DJBX_Std.Add(tempDJBXDFY);
            PressSet_KFY_DJBX_Std.Add(tempDJBX);

            //定级变形虚假值
            PressSet_KFY_DJBX_False = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempDJBXZYfalse = new ObservableCollection<double>() { 500, 500, 500 };
            ObservableCollection<double> tempDJBXZJfalse = new ObservableCollection<double>() { 50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550, 600, 650, 700, 750, 800, 850, 900, 950, 1000 };
            ObservableCollection<double> tempDJBXFYfalse = new ObservableCollection<double>() { -500, -500, -500 };
            ObservableCollection<double> tempDJBXFJfalse = new ObservableCollection<double>() { -50, -100, -150, -200, -250, -300, -350, -400, -450, -500, -550, -600, -650, -700, -750, -800, -850, -900, -950, -1000 };
            PressSet_KFY_DJBX_False.Add(tempDJBXZYfalse);
            PressSet_KFY_DJBX_False.Add(tempDJBXZJfalse);
            PressSet_KFY_DJBX_False.Add(tempDJBXFYfalse);
            PressSet_KFY_DJBX_False.Add(tempDJBXFJfalse);
        }


        #endregion

        #region 抗风压工程

        /// <summary>
        /// 抗风压工程p1预加压压力标准值
        /// </summary>
        private double _pressSet_KFY_GCP1_Y_Std = 500;
        /// <summary>
        /// 抗风压工程p1预加压压力标准值
        /// </summary>
        public double PressSet_KFY_GCP1_Y_Std
        {
            get { return _pressSet_KFY_GCP1_Y_Std; }
            set
            {
                _pressSet_KFY_GCP1_Y_Std = value;
                RaisePropertyChanged(() => PressSet_KFY_GCP1_Y_Std);
            }
        }

        /// <summary>
        /// 抗风压工程p1加压倍数标准值
        /// </summary>
        private ObservableCollection<double> _pressSet_KFY_GCP1_Raito_Std = new ObservableCollection<double>() { 0.1, 0.2, 0.3, 0.4 };
        /// <summary>
        /// 抗风压工程p1加压倍数标准值
        /// </summary>
        public ObservableCollection<double> PressSet_KFY_GCP1_Raito_Std
        {
            get { return _pressSet_KFY_GCP1_Raito_Std; }
            set
            {
                _pressSet_KFY_GCP1_Raito_Std = value;
                RaisePropertyChanged(() => PressSet_KFY_GCP1_Raito_Std);
            }
        }

        /// <summary>
        /// 抗风压工程p2加压倍数标准值
        /// </summary>
        private double _pressSet_KFY_GCP2_Raito_Std = 0.6;
        /// <summary>
        /// 抗风压工程p2加压倍数标准值
        /// </summary>
        public double PressSet_KFY_GCP2_Raito_Std
        {
            get { return _pressSet_KFY_GCP2_Raito_Std; }
            set
            {
                _pressSet_KFY_GCP2_Raito_Std = value;
                RaisePropertyChanged(() => PressSet_KFY_GCP2_Raito_Std);
            }
        }

        /// <summary>
        /// 抗风压工程pmax加压倍数标准值
        /// </summary>
        private double _pressSet_KFY_GCPmax_Raito_Std = 1.4;
        /// <summary>
        /// 抗风压工程p2加压倍数标准值
        /// </summary>
        public double PressSet_KFY_GCPmax_Raito_Std
        {
            get { return _pressSet_KFY_GCPmax_Raito_Std; }
            set
            {
                _pressSet_KFY_GCPmax_Raito_Std = value;
                RaisePropertyChanged(() => PressSet_KFY_GCPmax_Raito_Std);
            }
        }

        /// <summary>
        /// 抗风压工程变形虚假压力
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _pressSet_KFY_GCBX_False;
        /// <summary>
        /// 抗风压工程变形虚假压力
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> PressSet_KFY_GCBX_False
        {
            get { return _pressSet_KFY_GCBX_False; }
            set
            {
                _pressSet_KFY_GCBX_False = value;
                RaisePropertyChanged(() => PressSet_KFY_GCBX_False);
            }
        }

        /// <summary>
        /// 抗风压工程反复加压虚假压力
        /// </summary>
        private double _pressSet_KFY_GCP2_False = 500;
        /// <summary>
        /// 抗风压工程反复加压虚假压力
        /// </summary>
        public double PressSet_KFY_GCP2_False
        {
            get { return _pressSet_KFY_GCP2_False; }
            set
            {
                _pressSet_KFY_GCP2_False = value;
                RaisePropertyChanged(() => PressSet_KFY_GCP2_False);
            }
        }

        /// <summary>
        /// 抗风压工程P3加压虚假压力
        /// </summary>
        private double _pressSet_KFY_GCP3_False = 500;
        /// <summary>
        /// 抗风压工程P3加压虚假压力
        /// </summary>
        public double PressSet_KFY_GCP3_False
        {
            get { return _pressSet_KFY_GCP3_False; }
            set
            {
                _pressSet_KFY_GCP3_False = value;
                RaisePropertyChanged(() => PressSet_KFY_GCP3_False);
            }
        }

        /// <summary>
        /// 抗风压工程Pmax加压虚假压力
        /// </summary>
        private double _pressSet_KFY_GCPmax_False = 500;
        /// <summary>
        /// 抗风压工程Pmax加压虚假压力
        /// </summary>
        public double PressSet_KFY_GCPmax_False
        {
            get { return _pressSet_KFY_GCPmax_False; }
            set
            {
                _pressSet_KFY_GCPmax_False = value;
                RaisePropertyChanged(() => PressSet_KFY_GCPmax_False);
            }
        }

        /// <summary>
        ///抗风压检测工程压力初始化
        /// </summary>
        private void PressSet_KFYGC_Init()
        {
            //工程变形虚假值
            PressSet_KFY_GCBX_False = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempGCBXZYfalse = new ObservableCollection<double>() { 500, 500, 500 };
            ObservableCollection<double> tempGCBXZJfalse = new ObservableCollection<double>() { 100, 200, 300, 400 };
            ObservableCollection<double> tempGCBXFYfalse = new ObservableCollection<double>() { -500, -500, -500 };
            ObservableCollection<double> tempGCBXFJfalse = new ObservableCollection<double>() { -100, -200, -300, -400 };
            PressSet_KFY_GCBX_False.Add(tempGCBXZYfalse);
            PressSet_KFY_GCBX_False.Add(tempGCBXZJfalse);
            PressSet_KFY_GCBX_False.Add(tempGCBXFYfalse);
            PressSet_KFY_GCBX_False.Add(tempGCBXFJfalse);
        }

        #endregion
    }
}
