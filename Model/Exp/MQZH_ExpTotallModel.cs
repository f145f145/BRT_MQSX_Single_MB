/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQZHWL.Model.Exp
 * 文件名：  MQZH_ExpTotallModel
 * 版本号：  V1.0.0.0
 * 唯一标识：0dd9b81d-809e-44b6-9d15-d3594a1a424f
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/11/17 17:37:00
 * 描述：
 * 幕墙四性试验Model
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2021/11/17       17:37:00		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using MQZHWL.Model.ExpData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Windows;

namespace MQZHWL.Model.Exp
{
    /// <summary>
    /// 幕墙四性试验总类
    /// </summary>
    public class MQZH_ExpTotallModel : ObservableObject
    {
        #region 检测试验基本参数属性

        /// <summary>
        /// 试验信息及设定属性
        /// </summary>
        private MQZH_ExpParamModel _expSettingParam = new MQZH_ExpParamModel();
        /// <summary>
        /// 试验信息及设定属性
        /// </summary>
        public MQZH_ExpParamModel ExpSettingParam
        {
            get { return _expSettingParam; }
            set
            {
                _expSettingParam = value;
                RaisePropertyChanged(() => ExpSettingParam);
            }
        }

        /// <summary>
        /// 试件数量
        /// </summary>
        private double _specimenNum = 1;
        /// <summary>
        /// 试件数量
        /// </summary>
        public double SpecimenNum
        {
            get { return _specimenNum; }
            set
            {
                _specimenNum = value;
                RaisePropertyChanged(() => SpecimenNum);
            }
        }

        #endregion


        #region 检测试验状态属性

        /// <summary>
        /// 试验完成状态
        /// </summary>
        private bool _expCompleteStatus = false;
        /// <summary>
        /// 试验完成状态
        /// </summary>
        public bool ExpCompleteStatus
        {
            get { return _expCompleteStatus; }
            set
            {
                _expCompleteStatus = value;
                RaisePropertyChanged(() => ExpCompleteStatus);
            }
        }

        #endregion


        #region 实验室环境参数

        /// <summary>
        /// 实验室环境温度（K）
        /// </summary>
        private double _expRoomT = 298;
        /// <summary>
        /// 实验室环境温度（K）
        /// </summary>
        public double ExpRoomT
        {
            get { return _expRoomT; }
            set
            {
                _expRoomT = value;
                RaisePropertyChanged(() => ExpRoomT);
            }
        }

        /// <summary>
        /// 实验室环境大气压力（kPa）
        /// </summary>
        private double _expRoomPress = 101;
        /// <summary>
        /// 实验室环境大气压力（kPa）
        /// </summary>
        public double ExpRoomPress
        {
            get { return _expRoomPress; }
            set
            {
                _expRoomPress = value;
                RaisePropertyChanged(() => ExpRoomPress);
            }
        }

        #endregion


        #region 基本气密、抗风压、水密、层间变形检测项目属性

        /// <summary>
        /// 气密检测项目
        /// </summary>
        private MQZH_ExpModel_QM _exp_QM = new MQZH_ExpModel_QM();
        /// <summary>
        /// 气密检测项目
        /// </summary>
        public MQZH_ExpModel_QM Exp_QM
        {
            get { return _exp_QM; }
            set
            {
                _exp_QM = value;
                RaisePropertyChanged(() => Exp_QM);
            }
        }

        /// <summary>
        /// 水密检测项目
        /// </summary>
        private MQZH_ExpModel_SM _exp_SM = new MQZH_ExpModel_SM();
        /// <summary>
        /// 水密检测项目
        /// </summary>
        public MQZH_ExpModel_SM Exp_SM
        {
            get { return _exp_SM; }
            set
            {
                _exp_SM = value;
                RaisePropertyChanged(() => Exp_SM);
            }
        }

        /// <summary>
        /// 抗风压检测项目
        /// </summary>
        private MQZH_ExpModel_KFY _exp_KFY = new MQZH_ExpModel_KFY();
        /// <summary>
        /// 抗风压检测项目
        /// </summary>
        public MQZH_ExpModel_KFY Exp_KFY
        {
            get { return _exp_KFY; }
            set
            {
                _exp_KFY = value;
                RaisePropertyChanged(() => Exp_KFY);
            }
        }

        /// <summary>
        /// 层间变形检测项目
        /// </summary>
        private MQZH_ExpModel_CJBX _exp_CJBX = new MQZH_ExpModel_CJBX();
        /// <summary>
        /// 层间变形检测项目
        /// </summary>
        public MQZH_ExpModel_CJBX Exp_CJBX
        {
            get { return _exp_CJBX; }
            set
            {
                _exp_CJBX = value;
                RaisePropertyChanged(() => Exp_CJBX);
            }
        }

        #endregion


        #region 气密、抗风压、水密、层间变形项目数据及总体数据

        /// <summary>
        /// 气密检测项目数据
        /// </summary>
        private MQZH_ExpDataModel_QM _expData_QM = new MQZH_ExpDataModel_QM();
        /// <summary>
        /// 气密检测项目数据
        /// </summary>
        public MQZH_ExpDataModel_QM ExpData_QM
        {
            get { return _expData_QM; }
            set
            {
                _expData_QM = value;
                RaisePropertyChanged(() => ExpData_QM);
            }
        }

        /// <summary>
        /// 水密检测项目数据
        /// </summary>
        private MQZH_ExpDataModel_SM _expData_SM = new MQZH_ExpDataModel_SM();
        /// <summary>
        /// 水密检测项目数据
        /// </summary>
        public MQZH_ExpDataModel_SM ExpData_SM
        {
            get { return _expData_SM; }
            set
            {
                _expData_SM = value;
                RaisePropertyChanged(() => ExpData_SM);
            }
        }

        /// <summary>
        /// 抗风压检测项目数据
        /// </summary>
        private MQZH_ExpDataModel_KFY _expData_KFY = new MQZH_ExpDataModel_KFY();
        /// <summary>
        /// 抗风压检测项目数据
        /// </summary>
        public MQZH_ExpDataModel_KFY ExpData_KFY
        {
            get { return _expData_KFY; }
            set
            {
                _expData_KFY = value;
                RaisePropertyChanged(() => ExpData_KFY);
            }
        }

        /// <summary>
        /// 层间变形检测项目数据
        /// </summary>
        private MQZH_ExpDataModel_CJBX _expData_CJBX = new MQZH_ExpDataModel_CJBX();
        /// <summary>
        /// 层间变形检测项目数据
        /// </summary>
        public MQZH_ExpDataModel_CJBX ExpData_CJBX
        {
            get { return _expData_CJBX; }
            set
            {
                _expData_CJBX = value;
                RaisePropertyChanged(() => ExpData_CJBX);
            }
        }

        /// <summary>
        /// 试验总体数据
        /// </summary>
        private MQZH_ExpDataModel_Total _expTotalData = new MQZH_ExpDataModel_Total();
        /// <summary>
        /// 试验总体数据
        /// </summary>
        public MQZH_ExpDataModel_Total ExpTotalData
        {
            get { return _expTotalData; }
            set
            {
                _expTotalData = value;
                RaisePropertyChanged(() => ExpTotalData);
            }
        }

        #endregion


        #region 气密测试数据计算及评价

        /// <summary>
        /// 气密检测数据计算及评定
        /// </summary>
        public void QM_Evaluate()
        {
            //传入数据分析
            if ((ExpRoomPress > 150) || (ExpRoomPress < 50))
            {
                MessageBox.Show("大气压力数值有误，请仔细核对！");
                return;
            }
            if ((ExpRoomT > 323) || (ExpRoomT < 223))
            {
                MessageBox.Show("温度数值有误，请仔细核对！");
                return;
            }
            if ((ExpSettingParam.Exp_SJ_Aria > 60) || (ExpSettingParam.Exp_SJ_Aria <= 0))
            {
                MessageBox.Show("面积数值有误，请仔细核对！");
                return;
            }
            if ((ExpSettingParam.Exp_SJ_KQFLenth > 40) || (ExpSettingParam.Exp_SJ_KQFLenth < 0))
            {
                MessageBox.Show("开启缝长数值有误，请仔细核对！");
                return;
            }

            if (!Exp_QM.IsGC)    //定级部分--------------------------------------
            {
                //无可开启部分时，对应定级数据清零
                if (!ExpSettingParam.Isexp_SJ_WithKKQ)
                {
                    ExpData_QM.Stl_QMDJ[4] = new ObservableCollection<double> { 0, 0, 0 };
                    ExpData_QM.Stl_QMDJ[5] = new ObservableCollection<double> { 0, 0, 0, 0, 0 };
                    ExpData_QM.Stl_QMDJ[6] = new ObservableCollection<double> { 0, 0, 0 };
                    ExpData_QM.Stl_QMDJ[7] = new ObservableCollection<double> { 0, 0, 0, 0, 0 };
                }
                //计算定级正压标准状态下渗透量
                ExpData_QM.Qfp_DJ_Z = (ExpData_QM.Stl_QMDJ[1][1] + ExpData_QM.Stl_QMDJ[1][3]) / 2;
                ExpData_QM.Qfgp_DJ_Z = (ExpData_QM.Stl_QMDJ[5][1] + ExpData_QM.Stl_QMDJ[5][3]) / 2;
                ExpData_QM.Qzp_DJ_Z = (ExpData_QM.Stl_QMDJ[9][1] + ExpData_QM.Stl_QMDJ[9][3]) / 2;
                ExpData_QM.Qf1_DJ_Z = CalSTL_Standard(ExpData_QM.Qfp_DJ_Z);
                ExpData_QM.Qfg1_DJ_Z = CalSTL_Standard(ExpData_QM.Qfgp_DJ_Z);
                ExpData_QM.Qz1_DJ_Z = CalSTL_Standard(ExpData_QM.Qzp_DJ_Z);
                //计算定级负压标准状态下渗透量
                ExpData_QM.Qfp_DJ_F = (ExpData_QM.Stl_QMDJ[3][1] + ExpData_QM.Stl_QMDJ[3][3]) / 2;
                ExpData_QM.Qfgp_DJ_F = (ExpData_QM.Stl_QMDJ[7][1] + ExpData_QM.Stl_QMDJ[7][3]) / 2;
                ExpData_QM.Qzp_DJ_F = (ExpData_QM.Stl_QMDJ[11][1] + ExpData_QM.Stl_QMDJ[11][3]) / 2;
                ExpData_QM.Qf1_DJ_F = CalSTL_Standard(ExpData_QM.Qfp_DJ_F);
                ExpData_QM.Qfg1_DJ_F = CalSTL_Standard(ExpData_QM.Qfgp_DJ_F);
                ExpData_QM.Qz1_DJ_F = CalSTL_Standard(ExpData_QM.Qzp_DJ_F);
                //计算定级100Pa压差、标准状态下试件可开启部分渗透量
                if (!ExpSettingParam.Isexp_SJ_WithKKQ)
                {
                    ExpData_QM.Qk_DJ_Z = 0;
                    ExpData_QM.Qk_DJ_F = 0;
                }
                else
                {
                    ExpData_QM.Qk_DJ_Z = ExpData_QM.Qz1_DJ_Z - ExpData_QM.Qfg1_DJ_Z;
                    ExpData_QM.Qk_DJ_F = ExpData_QM.Qz1_DJ_F - ExpData_QM.Qfg1_DJ_F;
                }
                //计算定级100Pa压差、标准状态下试件整体渗透量
                ExpData_QM.Qs_DJ_Z = ExpData_QM.Qz1_DJ_Z - ExpData_QM.Qf1_DJ_Z;
                ExpData_QM.Qs_DJ_F = ExpData_QM.Qz1_DJ_F - ExpData_QM.Qf1_DJ_F;
                //计算定级100Pa压差、标准状态下试件整体单位面积渗透量
                ExpData_QM.QA1_DJ_Z = ExpData_QM.Qs_DJ_Z / ExpSettingParam.Exp_SJ_Aria;
                ExpData_QM.QA1_DJ_F = ExpData_QM.Qs_DJ_F / ExpSettingParam.Exp_SJ_Aria;
                //计算定级100Pa压差、标准状态下试件单位可开启缝长度渗透量
                if (!ExpSettingParam.Isexp_SJ_WithKKQ)
                {
                    ExpData_QM.Ql1_DJ_Z = 0;
                    ExpData_QM.Ql1_DJ_F = 0;
                }
                else
                {
                    ExpData_QM.Ql1_DJ_Z = ExpData_QM.Qk_DJ_Z / ExpSettingParam.Exp_SJ_KQFLenth;
                    ExpData_QM.Ql1_DJ_F = ExpData_QM.Qk_DJ_F / ExpSettingParam.Exp_SJ_KQFLenth;
                }
                //计算定级10Pa压差、标准状态下试件整体单位面积渗透量
                ExpData_QM.QA_DJ_Z = Math.Abs(ExpData_QM.QA1_DJ_Z / 4.65);
                ExpData_QM.QA_DJ_F = Math.Abs(ExpData_QM.QA1_DJ_F / 4.65);
                //计算定级10Pa压差、标准状态下试件单位可开启缝长度渗透量
                ExpData_QM.Ql_DJ_Z = Math.Abs(ExpData_QM.Ql1_DJ_Z / 4.65);
                ExpData_QM.Ql_DJ_F = Math.Abs(ExpData_QM.Ql1_DJ_F / 4.65);
                //定级正负压最不利值
                ExpData_QM.QA_DJ_QM = Math.Max(ExpData_QM.QA_DJ_Z, ExpData_QM.QA_DJ_F);
                ExpData_QM.Ql_DJ_QM = Math.Max(ExpData_QM.Ql_DJ_Z, ExpData_QM.Ql_DJ_F);
                //定级单位面积正压渗透量评级
                if ((ExpData_QM.QA_DJ_Z > 2.0) && (ExpData_QM.QA_DJ_Z <= 4))
                    ExpData_QM.QALevel_DJZ_QM = 1;
                else if ((ExpData_QM.QA_DJ_Z > 1.2) && (ExpData_QM.QA_DJ_Z <= 2.0))
                    ExpData_QM.QALevel_DJZ_QM = 2;
                else if ((ExpData_QM.QA_DJ_Z > 0.5) && (ExpData_QM.QA_DJ_Z <= 1.2))
                    ExpData_QM.QALevel_DJZ_QM = 3;
                else if ((ExpData_QM.QA_DJ_Z <= 0.5) && (ExpData_QM.QA_DJ_Z >= 0))
                    ExpData_QM.QALevel_DJZ_QM = 4;
                else
                    ExpData_QM.QALevel_DJZ_QM = 0;
                //定级单位面积负压渗透量评级
                if ((ExpData_QM.QA_DJ_F > 2.0) && (ExpData_QM.QA_DJ_F <= 4))
                    ExpData_QM.QALevel_DJF_QM = 1;
                else if ((ExpData_QM.QA_DJ_F > 1.2) && (ExpData_QM.QA_DJ_F <= 2.0))
                    ExpData_QM.QALevel_DJF_QM = 2;
                else if ((ExpData_QM.QA_DJ_F > 0.5) && (ExpData_QM.QA_DJ_F <= 1.2))
                    ExpData_QM.QALevel_DJF_QM = 3;
                else if ((ExpData_QM.QA_DJ_F <= 0.5) && (ExpData_QM.Ql_DJ_F >= 0))
                    ExpData_QM.QALevel_DJF_QM = 4;
                else
                    ExpData_QM.QALevel_DJF_QM = 0;
                //定级单位开启缝长渗透量评级
                if (!ExpSettingParam.Isexp_SJ_WithKKQ)
                {
                    ExpData_QM.QlLevel_DJZ_QM = 0;
                    ExpData_QM.QlLevel_DJF_QM = 0;
                }
                else
                {
                    //正压
                    if ((ExpData_QM.Ql_DJ_Z > 2.0) && (ExpData_QM.Ql_DJ_Z <= 4))
                        ExpData_QM.QlLevel_DJZ_QM = 1;
                    else if ((ExpData_QM.Ql_DJ_Z > 1.2) && (ExpData_QM.Ql_DJ_Z <= 2.0))
                        ExpData_QM.QlLevel_DJZ_QM = 2;
                    else if ((ExpData_QM.Ql_DJ_Z > 0.5) && (ExpData_QM.Ql_DJ_Z <= 1.2))
                        ExpData_QM.QlLevel_DJZ_QM = 3;
                    else if ((ExpData_QM.Ql_DJ_Z <= 0.5) && (ExpData_QM.Ql_DJ_Z > 0))
                        ExpData_QM.QlLevel_DJZ_QM = 4;
                    else
                        ExpData_QM.QlLevel_DJZ_QM = 0;
                    //负压
                    if ((ExpData_QM.Ql_DJ_F > 2.0) && (ExpData_QM.Ql_DJ_F <= 4))
                        ExpData_QM.QlLevel_DJF_QM = 1;
                    else if ((ExpData_QM.Ql_DJ_F > 1.2) && (ExpData_QM.Ql_DJ_F <= 2.0))
                        ExpData_QM.QlLevel_DJF_QM = 2;
                    else if ((ExpData_QM.Ql_DJ_F > 0.5) && (ExpData_QM.Ql_DJ_F <= 1.2))
                        ExpData_QM.QlLevel_DJF_QM = 3;
                    else if ((ExpData_QM.Ql_DJ_F <= 0.5) && (ExpData_QM.Ql_DJ_F > 0))
                        ExpData_QM.QlLevel_DJF_QM = 4;
                    else
                        ExpData_QM.QlLevel_DJF_QM = 0;
                }
                //定级总评级
                ExpData_QM.QALevel_DJ_QM = Math.Min(ExpData_QM.QALevel_DJZ_QM, ExpData_QM.QALevel_DJF_QM);
                ExpData_QM.QlLevel_DJ_QM = Math.Min(ExpData_QM.QlLevel_DJZ_QM, ExpData_QM.QlLevel_DJF_QM);

                //工程检测数据清零
                ExpData_QM.Stl_QMGC[0] = new ObservableCollection<double> { 0, 0, 0 };
                ExpData_QM.Stl_QMGC[1] = new ObservableCollection<double> { 0 };
                ExpData_QM.Stl_QMGC[2] = new ObservableCollection<double> { 0, 0, 0 };
                ExpData_QM.Stl_QMGC[3] = new ObservableCollection<double> { 0 };
                ExpData_QM.Stl_QMGC[4] = new ObservableCollection<double> { 0, 0, 0 };
                ExpData_QM.Stl_QMGC[5] = new ObservableCollection<double> { 0 };
                ExpData_QM.Stl_QMGC[6] = new ObservableCollection<double> { 0, 0, 0 };
                ExpData_QM.Stl_QMGC[7] = new ObservableCollection<double> { 0 };
                ExpData_QM.Stl_QMGC[5] = new ObservableCollection<double> { 0, 0, 0 };
                ExpData_QM.Stl_QMGC[9] = new ObservableCollection<double> { 0 };
                ExpData_QM.Stl_QMGC[10] = new ObservableCollection<double> { 0, 0, 0 };
                ExpData_QM.Stl_QMGC[11] = new ObservableCollection<double> { 0 };
                //工程正压标准状态下渗透量
                ExpData_QM.Qf1_GC_Z = 0;
                ExpData_QM.Qfg1_GC_Z = 0;
                ExpData_QM.Qz1_GC_Z = 0;
                //工程负压标准状态下渗透量
                ExpData_QM.Qf1_GC_F = 0;
                ExpData_QM.Qfg1_GC_F = 0;
                ExpData_QM.Qz1_GC_F = 0;
                //工程测试压差、标准状态下试件可开启部分渗透量
                ExpData_QM.Qk_GC_Z = 0;
                ExpData_QM.Qk_GC_F = 0;
                //工程测试压差、标准状态下试件整体渗透量
                ExpData_QM.Qs_GC_Z = 0;
                ExpData_QM.Qs_GC_F = 0;
                //工程测试压差、标准状态下试件整体单位面积渗透量
                ExpData_QM.QA1_GC_Z = 0;
                ExpData_QM.QA1_GC_F = 0;
                //工程测试压差、标准状态下试件单位可开启缝长度渗透量
                ExpData_QM.Ql1_GC_Z = 0;
                ExpData_QM.Ql1_GC_F = 0;
                //工程单位面积渗透量
                ExpData_QM.IsMeetDesign_GC_ZQA = false;
                ExpData_QM.IsMeetDesign_GC_FQA = false;
                //工程单位开启缝长渗透量
                ExpData_QM.IsMeetDesign_GC_ZQl = false;
                ExpData_QM.IsMeetDesign_GC_FQl = false;
                //工程总评定
                ExpData_QM.IsMeetDesign_GC_QM = false;
            }
            else                //工程部分---------------------
            {
                //无可开启部分时，对应工程数据清零
                if (!ExpSettingParam.Isexp_SJ_WithKKQ)
                {
                    ExpData_QM.Stl_QMGC[4] = new ObservableCollection<double> { 0, 0, 0 };
                    ExpData_QM.Stl_QMGC[5] = new ObservableCollection<double> { 0 };
                    ExpData_QM.Stl_QMGC[6] = new ObservableCollection<double> { 0, 0, 0 };
                    ExpData_QM.Stl_QMGC[7] = new ObservableCollection<double> { 0 };
                }
                //计算工程正压标准状态下渗透量
                ExpData_QM.Qf1_GC_Z = CalSTL_Standard(ExpData_QM.Stl_QMGC[1][0]);
                ExpData_QM.Qfg1_GC_Z = CalSTL_Standard(ExpData_QM.Stl_QMGC[5][0]);
                ExpData_QM.Qz1_GC_Z = CalSTL_Standard(ExpData_QM.Stl_QMGC[9][0]);
                //计算工程负压标准状态下渗透量
                ExpData_QM.Qf1_GC_F = CalSTL_Standard(ExpData_QM.Stl_QMGC[3][0]);
                ExpData_QM.Qfg1_GC_F = CalSTL_Standard(ExpData_QM.Stl_QMGC[7][0]);
                ExpData_QM.Qz1_GC_F = CalSTL_Standard(ExpData_QM.Stl_QMGC[11][0]);
                //计算工程测试压差、标准状态下试件可开启部分渗透量
                if (!ExpSettingParam.Isexp_SJ_WithKKQ)
                {
                    ExpData_QM.Qk_GC_Z = 0;
                    ExpData_QM.Qk_GC_F = 0;
                }
                else
                {
                    ExpData_QM.Qk_GC_Z = ExpData_QM.Qz1_GC_Z - ExpData_QM.Qfg1_GC_Z;
                    ExpData_QM.Qk_GC_F = ExpData_QM.Qz1_GC_F - ExpData_QM.Qfg1_GC_F;
                }
                //计算工程测试压差、标准状态下试件整体渗透量
                ExpData_QM.Qs_GC_Z = ExpData_QM.Qz1_GC_Z - ExpData_QM.Qf1_GC_Z;
                ExpData_QM.Qs_GC_F = ExpData_QM.Qz1_GC_F - ExpData_QM.Qf1_GC_F;
                //计算工程测试压差、标准状态下试件整体单位面积渗透量
                ExpData_QM.QA1_GC_Z = ExpData_QM.Qs_GC_Z / ExpSettingParam.Exp_SJ_Aria;
                ExpData_QM.QA1_GC_F = ExpData_QM.Qs_GC_F / ExpSettingParam.Exp_SJ_Aria;
                //计算工程测试压差、标准状态下试件单位可开启缝长度渗透量
                if (!ExpSettingParam.Isexp_SJ_WithKKQ)
                {
                    ExpData_QM.Ql1_GC_Z = 0;
                    ExpData_QM.Ql1_GC_F = 0;
                }
                else
                {
                    ExpData_QM.Ql1_GC_Z = ExpData_QM.Qk_GC_Z / ExpSettingParam.Exp_SJ_KQFLenth;
                    ExpData_QM.Ql1_GC_F = ExpData_QM.Qk_GC_F / ExpSettingParam.Exp_SJ_KQFLenth;
                }
                //工程单位面积渗透量评定
                if (Math.Abs(ExpData_QM.QA1_GC_Z) <= Math.Abs(Exp_QM.QM_SJSTL_DWMJ))
                    ExpData_QM.IsMeetDesign_GC_ZQA = true;
                else
                    ExpData_QM.IsMeetDesign_GC_ZQA = false;
                if (Math.Abs(ExpData_QM.QA1_GC_F) <= Math.Abs(Exp_QM.QM_SJSTL_DWMJ))
                    ExpData_QM.IsMeetDesign_GC_FQA = true;
                else
                    ExpData_QM.IsMeetDesign_GC_FQA = false;
                //工程单位开启缝长渗透量评定
                if (Math.Abs(ExpData_QM.Ql1_GC_Z) <= Math.Abs(Exp_QM.QM_SJSTL_DWKQFC))
                    ExpData_QM.IsMeetDesign_GC_ZQl = true;
                else
                    ExpData_QM.IsMeetDesign_GC_ZQl = false;
                if (Math.Abs(ExpData_QM.Ql1_GC_F) <= Math.Abs(Exp_QM.QM_SJSTL_DWKQFC))
                    ExpData_QM.IsMeetDesign_GC_FQl = true;
                else
                    ExpData_QM.IsMeetDesign_GC_FQl = false;

                if (!ExpSettingParam.Isexp_SJ_WithKKQ)
                {
                    ExpData_QM.IsMeetDesign_GC_ZQl = true;
                    ExpData_QM.IsMeetDesign_GC_FQl = true;
                }

                //工程总评定
                ExpData_QM.IsMeetDesign_GC_QM = ExpData_QM.IsMeetDesign_GC_ZQA && ExpData_QM.IsMeetDesign_GC_FQA &&
                                            ExpData_QM.IsMeetDesign_GC_ZQl && ExpData_QM.IsMeetDesign_GC_FQl;

                //无可开启部分时，对应定级数据清零
                ExpData_QM.Stl_QMDJ[0] = new ObservableCollection<double> { 0, 0, 0 };
                ExpData_QM.Stl_QMDJ[1] = new ObservableCollection<double> { 0, 0, 0, 0, 0 };
                ExpData_QM.Stl_QMDJ[2] = new ObservableCollection<double> { 0, 0, 0 };
                ExpData_QM.Stl_QMDJ[3] = new ObservableCollection<double> { 0, 0, 0, 0, 0 };
                ExpData_QM.Stl_QMDJ[4] = new ObservableCollection<double> { 0, 0, 0 };
                ExpData_QM.Stl_QMDJ[5] = new ObservableCollection<double> { 0, 0, 0, 0, 0 };
                ExpData_QM.Stl_QMDJ[6] = new ObservableCollection<double> { 0, 0, 0 };
                ExpData_QM.Stl_QMDJ[7] = new ObservableCollection<double> { 0, 0, 0, 0, 0 };
                ExpData_QM.Stl_QMDJ[8] = new ObservableCollection<double> { 0, 0, 0 };
                ExpData_QM.Stl_QMDJ[9] = new ObservableCollection<double> { 0, 0, 0, 0, 0 };
                ExpData_QM.Stl_QMDJ[10] = new ObservableCollection<double> { 0, 0, 0 };
                ExpData_QM.Stl_QMDJ[11] = new ObservableCollection<double> { 0, 0, 0, 0, 0 };
                //定级正压标准状态下渗透量
                ExpData_QM.Qfp_DJ_Z = 0;
                ExpData_QM.Qfgp_DJ_Z = 0;
                ExpData_QM.Qzp_DJ_Z = 0;
                ExpData_QM.Qf1_DJ_Z = 0;
                ExpData_QM.Qfg1_DJ_Z = 0;
                ExpData_QM.Qz1_DJ_Z = 0;
                //定级负压标准状态下渗透量
                ExpData_QM.Qfp_DJ_F = 0;
                ExpData_QM.Qfgp_DJ_F = 0;
                ExpData_QM.Qzp_DJ_F = 0;
                ExpData_QM.Qf1_DJ_F = 0;
                ExpData_QM.Qfg1_DJ_F = 0;
                ExpData_QM.Qz1_DJ_F = 0;
                //定级100Pa压差、标准状态下试件可开启部分渗透量
                ExpData_QM.Qk_DJ_Z = 0;
                ExpData_QM.Qk_DJ_F = 0;
                //定级100Pa压差、标准状态下试件整体渗透量
                ExpData_QM.Qs_DJ_Z = 0;
                ExpData_QM.Qs_DJ_F = 0;
                //定级100Pa压差、标准状态下试件整体单位面积渗透量
                ExpData_QM.QA1_DJ_Z = 0;
                ExpData_QM.QA1_DJ_F = 0;
                //定级100Pa压差、标准状态下试件单位可开启缝长度渗透量
                ExpData_QM.Ql1_DJ_Z = 0;
                ExpData_QM.Ql1_DJ_F = 0;
                //定级10Pa压差、标准状态下试件整体单位面积渗透量
                ExpData_QM.QA_DJ_Z = 0;
                ExpData_QM.QA_DJ_F = 0;
                //定级10Pa压差、标准状态下试件单位可开启缝长度渗透量
                ExpData_QM.Ql_DJ_Z = 0;
                ExpData_QM.Ql_DJ_F = 0;
                //定级正负压最不利值
                ExpData_QM.QA_DJ_QM = 0;
                ExpData_QM.Ql_DJ_QM = 0;
                //定级单位面积正压渗透量评级
                ExpData_QM.QALevel_DJZ_QM = 0;
                //定级单位面积负压渗透量评级
                ExpData_QM.QALevel_DJF_QM = 0;
                //定级单位开启缝长渗透量评级
                ExpData_QM.QlLevel_DJZ_QM = 0;
                ExpData_QM.QlLevel_DJF_QM = 0;
                //定级总评级
                ExpData_QM.QALevel_DJ_QM = 0;
                ExpData_QM.QlLevel_DJ_QM = 0;
            }
        }

        /// <summary>
        /// 换算为标准状态下渗透量
        /// </summary>
        /// <param name="pC">测试渗透量</param>
        /// <returns>标准状态下渗透量</returns>
        private double CalSTL_Standard(double qC)
        {
            double q1 = 293 * qC * ExpRoomPress / 101.3 / ExpRoomT;
            return q1;
        }

        #endregion


        #region 水密测试数据计算及评价

        /// <summary>
        /// 水密检测数据计算及评价
        /// </summary>
        public void SM_Evaluate()
        {
            //定级检测----------------------
            if (!Exp_SM.IsGC)
            {
                double tempStepPressMax;            //步骤压力最大值（用于判定压力是否超过抗风压安全值）
                double tempKFYAQ;                   //抗风压安全压力（从抗风压变形P1推算）
                //可开启暂存值
                int tempKKQLevel = 0;                   //可开启定级
                int tempKKQDamage = 0;                  //可开启损坏情况
                string tempKKQDamagePS = "未渗漏";     //可开启损坏说明
                double tempKKQMaxPress = 0;             //可开启最大压力
                //固定暂存值
                int tempGDLevel = 0;                    //固定定级
                int tempGDDamage = 0;                   //固定损坏情况
                string tempGDDamagePS = "未检测";      //固定损坏说明
                double tempGDMaxPress = 0;              //固定最大压力
                tempKFYAQ = Math.Max(Math.Abs(ExpData_KFY.PDValue_DJP3_Z), Math.Abs(ExpData_KFY.PDValue_DJP3_F));
                //可开启部分
                for (int i = 1; i < 8; i++)
                {
                    //获取本步骤压力最高值
                    if (Exp_SM.WaveType_SM)
                        tempStepPressMax = Math.Abs(ExpData_SM.Press_DJ[i] * 1.25);
                    else
                        tempStepPressMax = Math.Abs(ExpData_SM.Press_DJ[i]);
                    //步骤已完成且最高压力未超过抗风压安全值时，按损坏程度和压力大小分析
                    if (tempStepPressMax > 0) 
                    {
                        if (ExpData_SM.SLStatus_DJ_KKQ[i] <= 3)
                        {
                            tempKKQDamage = ExpData_SM.SLStatus_DJ_KKQ[i];
                            tempKKQDamagePS = ExpData_SM.SLPS_DJ_KKQ[i];
                            tempKKQMaxPress = Math.Abs(ExpData_SM.Press_DJ[i]);
                            if ((tempKKQMaxPress >= 250) && (tempKKQMaxPress < 350))
                            {
                                tempKKQLevel = 1;
                            }
                            else if ((tempKKQMaxPress >= 350) && (tempKKQMaxPress < 500))
                            {
                                tempKKQLevel = 2;
                            }
                            else if ((tempKKQMaxPress >= 500) && (tempKKQMaxPress < 700))
                            {
                                tempKKQLevel = 3;
                            }
                            else if ((tempKKQMaxPress >= 700) && (tempKKQMaxPress < 1000))
                            {
                                tempKKQLevel = 4;
                            }
                            else if (tempKKQMaxPress >= 1000)
                            {
                                tempKKQLevel = 5;
                            }
                            else
                                tempKKQLevel = 0;
                        }
                        else
                            break;
                    }
                    else
                    {
                        break;
                    }
                }
                //固定部分
                for (int i = 1; i < 8; i++)
                {
                    //获取本步骤压力最高值
                    if (Exp_SM.WaveType_SM)
                        tempStepPressMax = Math.Abs(ExpData_SM.Press_DJ[i] * 1.25);
                    else
                        tempStepPressMax = Math.Abs(ExpData_SM.Press_DJ[i]);
                    //步骤已完成且最高压力未超过抗风压安全值时，按损坏程度和压力大小分析
                    if (tempStepPressMax > 0) 
                    {
                        if (ExpData_SM.SLStatus_DJ_GD[i] <= 3)
                        {
                            tempGDDamage = ExpData_SM.SLStatus_DJ_GD[i];
                            tempGDDamagePS = ExpData_SM.SLPS_DJ_GD[i];
                            tempGDMaxPress = Math.Abs(ExpData_SM.Press_DJ[i]);
                            if ((tempGDMaxPress >= 500) && (tempGDMaxPress < 700))
                            {
                                tempGDLevel = 1;
                            }
                            else if ((tempGDMaxPress >= 700) && (tempGDMaxPress < 1000))
                            {
                                tempGDLevel = 2;
                            }
                            else if ((tempGDMaxPress >= 1000) && (tempGDMaxPress < 1500))
                            {
                                tempGDLevel = 3;
                            }
                            else if ((tempGDMaxPress >= 1500) && (tempGDMaxPress < 2000))
                            {
                                tempGDLevel = 4;
                            }
                            else if (tempGDMaxPress >= 2000)
                            {
                                tempGDLevel = 5;
                            }
                        }
                        else
                            break;
                    }
                    else
                        break;
                }
                //可开启评定结果
                ExpData_SM.MaxPressWYZSL_DJ_KKQ = tempKKQMaxPress;
                ExpData_SM.Level_DJ_KKQ = tempKKQLevel;
                ExpData_SM.SLStatusFinal_DJ_KKQ = tempKKQDamage;
                ExpData_SM.SLPSFinal_DJ_KKQ = tempKKQDamagePS;

                //固定评定结果
                ExpData_SM.MaxPressWYZSL_DJ_GD = tempGDMaxPress;
                ExpData_SM.Level_DJ_GD = tempGDLevel;
                ExpData_SM.SLStatusFinal_DJ_GD = tempGDDamage;
                ExpData_SM.SLPSFinal_DJ_GD = tempGDDamagePS;
            }

            //工程检测----------------------------------
            else
            {
                ExpData_SM.IsMeetDesign_GC_All = true;
                //可开启部分
                if (ExpData_SM.SLStatus_GC_KKQ[0] <= 3)
                    ExpData_SM.IsMeetDesign_GC_KKQ = true;
                else
                {
                    ExpData_SM.IsMeetDesign_GC_KKQ = false;
                }
                if(!ExpSettingParam.Isexp_SJ_WithKKQ)
                    ExpData_SM.IsMeetDesign_GC_KKQ = true;
                //固定部分
                if (ExpData_SM.SLStatus_GC_GD[0] <= 3)
                    ExpData_SM.IsMeetDesign_GC_GD = true;
                else
                {
                    ExpData_SM.IsMeetDesign_GC_GD = false;
                }
                ExpData_SM.IsMeetDesign_GC_All = ExpData_SM.IsMeetDesign_GC_KKQ && ExpData_SM.IsMeetDesign_GC_GD;
            }
        }

        #endregion


        #region 抗风压测试数据计算及评价


        /// <summary>
        /// 抗风压检测数据计算及评价
        /// </summary>
        public void KFY_Evaluate()
        {
            double tempYXND1;
            double tempYXND2;
            double tempYXND3;
            //计算允许相对挠度（绝对）的40%
            if (Exp_KFY.DisplaceGroups[0].Is_Use)
                tempYXND1 = 1 / Exp_KFY.DisplaceGroups[0].ND_XD_YXFM * 0.4;
            else
                tempYXND1 = 0;
            if (Exp_KFY.DisplaceGroups[1].Is_Use)
                tempYXND2 = 1 / Exp_KFY.DisplaceGroups[1].ND_XD_YXFM * 0.4;
            else
                tempYXND2 = 0;
            if (Exp_KFY.DisplaceGroups[2].Is_Use)
                tempYXND3 = 1 / Exp_KFY.DisplaceGroups[2].ND_XD_YXFM * 0.4;
            else
                tempYXND3 = 0;

            double tempPokMax1 = 0;      //不超限、无损坏的最高压力
            double tempPokMax2 = 0;      //不超限、无损坏的最高压力
            double tempPokMax3 = 0;      //不超限、无损坏的最高压力
            double tempPover1 = 0;      //超限压力
            double tempPover2 = 0;
            double tempPover3 = 0;
            double tempNDok1 = 0;
            double tempNDover1 = 0;
            double tempNDok2 = 0;
            double tempNDover2 = 0;
            double tempNDok3 = 0;
            double tempNDover3 = 0;
            double tempP11 = 0;
            double tempP12 = 0;
            double tempP13 = 0;

            //重新计算挠度
            //创建临时测点组
            ObservableCollection<DisplaceGroup> tempGroups = new ObservableCollection<DisplaceGroup>();
            for (int i = 0; i < Exp_KFY.DisplaceGroups.Count; i++)
            {
                DisplaceGroup newGroup = new DisplaceGroup()
                {
                    ID = (i + 1).ToString(),
                    Is_Use = Exp_KFY.DisplaceGroups[i].Is_Use,
                    Is_TestMBBX = Exp_KFY.DisplaceGroups[i].Is_TestMBBX,
                    IsBZCSJMB = Exp_KFY.DisplaceGroups[i].IsBZCSJMB,
                    L = Exp_KFY.DisplaceGroups[i].L,
                    ND_XD_YXFM = Exp_KFY.DisplaceGroups[i].ND_XD_YXFM,
                    WYC_No = new ObservableCollection<int>(Exp_KFY.DisplaceGroups[i].WYC_No),
                };
                tempGroups.Add(newGroup);
            };
            try
            {
                //定级P1正压各步骤
                //初始位移
                tempGroups[0].WY_CS[0] = ExpData_KFY.WY_DJBX_Z[0][0];//位移1a
                tempGroups[0].WY_CS[1] = ExpData_KFY.WY_DJBX_Z[0][1];//位移1b
                tempGroups[0].WY_CS[2] = ExpData_KFY.WY_DJBX_Z[0][2];//位移1c
                tempGroups[0].WY_CS[3] = ExpData_KFY.WY_DJBX_Z[0][3];//位移1d
                tempGroups[1].WY_CS[0] = ExpData_KFY.WY_DJBX_Z[0][4];//位移2a
                tempGroups[1].WY_CS[1] = ExpData_KFY.WY_DJBX_Z[0][5];//位移2b
                tempGroups[1].WY_CS[2] = ExpData_KFY.WY_DJBX_Z[0][6];//位移2c
                tempGroups[2].WY_CS[0] = ExpData_KFY.WY_DJBX_Z[0][7];//位移3a
                tempGroups[2].WY_CS[1] = ExpData_KFY.WY_DJBX_Z[0][8];//位移3b
                tempGroups[2].WY_CS[2] = ExpData_KFY.WY_DJBX_Z[0][9];//位移3c
                //各步重新计算
                for (int step = 0; step < ExpData_KFY.WY_DJBX_Z.Count; step++)
                {
                    tempGroups[0].WY_DQ[0] = ExpData_KFY.WY_DJBX_Z[step][0];//位移1a
                    tempGroups[0].WY_DQ[1] = ExpData_KFY.WY_DJBX_Z[step][1];//位移1b
                    tempGroups[0].WY_DQ[2] = ExpData_KFY.WY_DJBX_Z[step][2];//位移1c
                    tempGroups[0].WY_DQ[3] = ExpData_KFY.WY_DJBX_Z[step][3];//位移1d
                    tempGroups[1].WY_DQ[0] = ExpData_KFY.WY_DJBX_Z[step][4];//位移2a
                    tempGroups[1].WY_DQ[1] = ExpData_KFY.WY_DJBX_Z[step][5];//位移2b
                    tempGroups[1].WY_DQ[2] = ExpData_KFY.WY_DJBX_Z[step][6];//位移2c
                    tempGroups[2].WY_DQ[0] = ExpData_KFY.WY_DJBX_Z[step][7];//位移3a
                    tempGroups[2].WY_DQ[1] = ExpData_KFY.WY_DJBX_Z[step][8];//位移3b
                    tempGroups[2].WY_DQ[2] = ExpData_KFY.WY_DJBX_Z[step][9];//位移3c
                    //重新计算各组挠度
                    for (int i = 0; i < Exp_KFY.DisplaceGroups.Count; i++)
                    {
                        tempGroups[i].NDJS();
                        //挠度重新赋值
                        ExpData_KFY.XDND_DJBX_Z[step][i] = tempGroups[i].ND_XD;
                        ExpData_KFY.ND_DJBX_Z[step][i] = tempGroups[i].ND;
                    }
                }

                //定级P1负压各步骤
                //初始位移
                tempGroups[0].WY_CS[0] = ExpData_KFY.WY_DJBX_F[0][0];//位移1a
                tempGroups[0].WY_CS[1] = ExpData_KFY.WY_DJBX_F[0][1];//位移1b
                tempGroups[0].WY_CS[2] = ExpData_KFY.WY_DJBX_F[0][2];//位移1c
                tempGroups[0].WY_CS[3] = ExpData_KFY.WY_DJBX_F[0][3];//位移1d
                tempGroups[1].WY_CS[0] = ExpData_KFY.WY_DJBX_F[0][4];//位移2a
                tempGroups[1].WY_CS[1] = ExpData_KFY.WY_DJBX_F[0][5];//位移2b
                tempGroups[1].WY_CS[2] = ExpData_KFY.WY_DJBX_F[0][6];//位移2c
                tempGroups[2].WY_CS[0] = ExpData_KFY.WY_DJBX_F[0][7];//位移3a
                tempGroups[2].WY_CS[1] = ExpData_KFY.WY_DJBX_F[0][8];//位移3b
                tempGroups[2].WY_CS[2] = ExpData_KFY.WY_DJBX_F[0][9];//位移3c
                //各步重新计算
                for (int step = 0; step < ExpData_KFY.WY_DJBX_F.Count; step++)
                {
                    tempGroups[0].WY_DQ[0] = ExpData_KFY.WY_DJBX_F[step][0];//位移1a
                    tempGroups[0].WY_DQ[1] = ExpData_KFY.WY_DJBX_F[step][1];//位移1b
                    tempGroups[0].WY_DQ[2] = ExpData_KFY.WY_DJBX_F[step][2];//位移1c
                    tempGroups[0].WY_DQ[3] = ExpData_KFY.WY_DJBX_F[step][3];//位移1d
                    tempGroups[1].WY_DQ[0] = ExpData_KFY.WY_DJBX_F[step][4];//位移2a
                    tempGroups[1].WY_DQ[1] = ExpData_KFY.WY_DJBX_F[step][5];//位移2b
                    tempGroups[1].WY_DQ[2] = ExpData_KFY.WY_DJBX_F[step][6];//位移2c
                    tempGroups[2].WY_DQ[0] = ExpData_KFY.WY_DJBX_F[step][7];//位移3a
                    tempGroups[2].WY_DQ[1] = ExpData_KFY.WY_DJBX_F[step][8];//位移3b
                    tempGroups[2].WY_DQ[2] = ExpData_KFY.WY_DJBX_F[step][9];//位移3c
                    //重新计算各组挠度
                    for (int i = 0; i < Exp_KFY.DisplaceGroups.Count; i++)
                    {
                        tempGroups[i].NDJS();
                        //挠度重新赋值
                        ExpData_KFY.XDND_DJBX_F[step][i] = tempGroups[i].ND_XD;
                        ExpData_KFY.ND_DJBX_F[step][i] = tempGroups[i].ND;
                    }
                }

                //定级P3正压各步骤
                //初始位移
                tempGroups[0].WY_CS[0] = ExpData_KFY.WY_DJP3_Z[0][0];//位移1a
                tempGroups[0].WY_CS[1] = ExpData_KFY.WY_DJP3_Z[0][1];//位移1b
                tempGroups[0].WY_CS[2] = ExpData_KFY.WY_DJP3_Z[0][2];//位移1c
                tempGroups[0].WY_CS[3] = ExpData_KFY.WY_DJP3_Z[0][3];//位移1d
                tempGroups[1].WY_CS[0] = ExpData_KFY.WY_DJP3_Z[0][4];//位移2a
                tempGroups[1].WY_CS[1] = ExpData_KFY.WY_DJP3_Z[0][5];//位移2b
                tempGroups[1].WY_CS[2] = ExpData_KFY.WY_DJP3_Z[0][6];//位移2c
                tempGroups[2].WY_CS[0] = ExpData_KFY.WY_DJP3_Z[0][7];//位移3a
                tempGroups[2].WY_CS[1] = ExpData_KFY.WY_DJP3_Z[0][8];//位移3b
                tempGroups[2].WY_CS[2] = ExpData_KFY.WY_DJP3_Z[0][9];//位移3c
                //各步重新计算
                for (int step = 0; step < ExpData_KFY.WY_DJP3_Z.Count; step++)
                {
                    tempGroups[0].WY_DQ[0] = ExpData_KFY.WY_DJP3_Z[step][0];//位移1a
                    tempGroups[0].WY_DQ[1] = ExpData_KFY.WY_DJP3_Z[step][1];//位移1b
                    tempGroups[0].WY_DQ[2] = ExpData_KFY.WY_DJP3_Z[step][2];//位移1c
                    tempGroups[0].WY_DQ[3] = ExpData_KFY.WY_DJP3_Z[step][3];//位移1d
                    tempGroups[1].WY_DQ[0] = ExpData_KFY.WY_DJP3_Z[step][4];//位移2a
                    tempGroups[1].WY_DQ[1] = ExpData_KFY.WY_DJP3_Z[step][5];//位移2b
                    tempGroups[1].WY_DQ[2] = ExpData_KFY.WY_DJP3_Z[step][6];//位移2c
                    tempGroups[2].WY_DQ[0] = ExpData_KFY.WY_DJP3_Z[step][7];//位移3a
                    tempGroups[2].WY_DQ[1] = ExpData_KFY.WY_DJP3_Z[step][8];//位移3b
                    tempGroups[2].WY_DQ[2] = ExpData_KFY.WY_DJP3_Z[step][9];//位移3c
                    //重新计算各组挠度
                    for (int i = 0; i < Exp_KFY.DisplaceGroups.Count; i++)
                    {
                        tempGroups[i].NDJS();
                        //挠度重新赋值
                        ExpData_KFY.XDND_DJP3_Z[step][i] = tempGroups[i].ND_XD;
                        ExpData_KFY.ND_DJP3_Z[step][i] = tempGroups[i].ND;
                    }
                }

                //定级P3负压各步骤
                //初始位移
                tempGroups[0].WY_CS[0] = ExpData_KFY.WY_DJP3_F[0][0];//位移1a
                tempGroups[0].WY_CS[1] = ExpData_KFY.WY_DJP3_F[0][1];//位移1b
                tempGroups[0].WY_CS[2] = ExpData_KFY.WY_DJP3_F[0][2];//位移1c
                tempGroups[0].WY_CS[3] = ExpData_KFY.WY_DJP3_F[0][3];//位移1d
                tempGroups[1].WY_CS[0] = ExpData_KFY.WY_DJP3_F[0][4];//位移2a
                tempGroups[1].WY_CS[1] = ExpData_KFY.WY_DJP3_F[0][5];//位移2b
                tempGroups[1].WY_CS[2] = ExpData_KFY.WY_DJP3_F[0][6];//位移2c
                tempGroups[2].WY_CS[0] = ExpData_KFY.WY_DJP3_F[0][7];//位移3a
                tempGroups[2].WY_CS[1] = ExpData_KFY.WY_DJP3_F[0][8];//位移3b
                tempGroups[2].WY_CS[2] = ExpData_KFY.WY_DJP3_F[0][9];//位移3c
                //各步重新计算
                for (int step = 0; step < ExpData_KFY.WY_DJP3_F.Count; step++)
                {
                    tempGroups[0].WY_DQ[0] = ExpData_KFY.WY_DJP3_F[step][0];//位移1a
                    tempGroups[0].WY_DQ[1] = ExpData_KFY.WY_DJP3_F[step][1];//位移1b
                    tempGroups[0].WY_DQ[2] = ExpData_KFY.WY_DJP3_F[step][2];//位移1c
                    tempGroups[0].WY_DQ[3] = ExpData_KFY.WY_DJP3_F[step][3];//位移1d
                    tempGroups[1].WY_DQ[0] = ExpData_KFY.WY_DJP3_F[step][4];//位移2a
                    tempGroups[1].WY_DQ[1] = ExpData_KFY.WY_DJP3_F[step][5];//位移2b
                    tempGroups[1].WY_DQ[2] = ExpData_KFY.WY_DJP3_F[step][6];//位移2c
                    tempGroups[2].WY_DQ[0] = ExpData_KFY.WY_DJP3_F[step][7];//位移3a
                    tempGroups[2].WY_DQ[1] = ExpData_KFY.WY_DJP3_F[step][8];//位移3b
                    tempGroups[2].WY_DQ[2] = ExpData_KFY.WY_DJP3_F[step][9];//位移3c
                    //重新计算各组挠度
                    for (int i = 0; i < Exp_KFY.DisplaceGroups.Count; i++)
                    {
                        tempGroups[i].NDJS();
                        //挠度重新赋值
                        ExpData_KFY.XDND_DJP3_F[step][i] = tempGroups[i].ND_XD;
                        ExpData_KFY.ND_DJP3_F[step][i] = tempGroups[i].ND;
                    }
                }

                //定级Pmax正压各步骤
                //初始位移
                tempGroups[0].WY_CS[0] = ExpData_KFY.WY_DJPmax_Z[0][0];//位移1a
                tempGroups[0].WY_CS[1] = ExpData_KFY.WY_DJPmax_Z[0][1];//位移1b
                tempGroups[0].WY_CS[2] = ExpData_KFY.WY_DJPmax_Z[0][2];//位移1c
                tempGroups[0].WY_CS[3] = ExpData_KFY.WY_DJPmax_Z[0][3];//位移1d
                tempGroups[1].WY_CS[0] = ExpData_KFY.WY_DJPmax_Z[0][4];//位移2a
                tempGroups[1].WY_CS[1] = ExpData_KFY.WY_DJPmax_Z[0][5];//位移2b
                tempGroups[1].WY_CS[2] = ExpData_KFY.WY_DJPmax_Z[0][6];//位移2c
                tempGroups[2].WY_CS[0] = ExpData_KFY.WY_DJPmax_Z[0][7];//位移3a
                tempGroups[2].WY_CS[1] = ExpData_KFY.WY_DJPmax_Z[0][8];//位移3b
                tempGroups[2].WY_CS[2] = ExpData_KFY.WY_DJPmax_Z[0][9];//位移3c
                //各步重新计算
                for (int step = 0; step < ExpData_KFY.WY_DJPmax_Z.Count; step++)
                {
                    tempGroups[0].WY_DQ[0] = ExpData_KFY.WY_DJPmax_Z[step][0];//位移1a
                    tempGroups[0].WY_DQ[1] = ExpData_KFY.WY_DJPmax_Z[step][1];//位移1b
                    tempGroups[0].WY_DQ[2] = ExpData_KFY.WY_DJPmax_Z[step][2];//位移1c
                    tempGroups[0].WY_DQ[3] = ExpData_KFY.WY_DJPmax_Z[step][3];//位移1d
                    tempGroups[1].WY_DQ[0] = ExpData_KFY.WY_DJPmax_Z[step][4];//位移2a
                    tempGroups[1].WY_DQ[1] = ExpData_KFY.WY_DJPmax_Z[step][5];//位移2b
                    tempGroups[1].WY_DQ[2] = ExpData_KFY.WY_DJPmax_Z[step][6];//位移2c
                    tempGroups[2].WY_DQ[0] = ExpData_KFY.WY_DJPmax_Z[step][7];//位移3a
                    tempGroups[2].WY_DQ[1] = ExpData_KFY.WY_DJPmax_Z[step][8];//位移3b
                    tempGroups[2].WY_DQ[2] = ExpData_KFY.WY_DJPmax_Z[step][9];//位移3c
                    //重新计算各组挠度
                    for (int i = 0; i < Exp_KFY.DisplaceGroups.Count; i++)
                    {
                        tempGroups[i].NDJS();
                        //挠度重新赋值
                        ExpData_KFY.XDND_DJP3_Z[step][i] = tempGroups[i].ND_XD;
                        ExpData_KFY.ND_DJP3_Z[step][i] = tempGroups[i].ND;
                    }
                }

                //定级Pmax负压各步骤
                //初始位移
                tempGroups[0].WY_CS[0] = ExpData_KFY.WY_DJPmax_F[0][0];//位移1a
                tempGroups[0].WY_CS[1] = ExpData_KFY.WY_DJPmax_F[0][1];//位移1b
                tempGroups[0].WY_CS[2] = ExpData_KFY.WY_DJPmax_F[0][2];//位移1c
                tempGroups[0].WY_CS[3] = ExpData_KFY.WY_DJPmax_F[0][3];//位移1d
                tempGroups[1].WY_CS[0] = ExpData_KFY.WY_DJPmax_F[0][4];//位移2a
                tempGroups[1].WY_CS[1] = ExpData_KFY.WY_DJPmax_F[0][5];//位移2b
                tempGroups[1].WY_CS[2] = ExpData_KFY.WY_DJPmax_F[0][6];//位移2c
                tempGroups[2].WY_CS[0] = ExpData_KFY.WY_DJPmax_F[0][7];//位移3a
                tempGroups[2].WY_CS[1] = ExpData_KFY.WY_DJPmax_F[0][8];//位移3b
                tempGroups[2].WY_CS[2] = ExpData_KFY.WY_DJPmax_F[0][9];//位移3c
                //各步重新计算
                for (int step = 0; step < ExpData_KFY.WY_DJPmax_F.Count; step++)
                {
                    tempGroups[0].WY_DQ[0] = ExpData_KFY.WY_DJPmax_F[step][0];//位移1a
                    tempGroups[0].WY_DQ[1] = ExpData_KFY.WY_DJPmax_F[step][1];//位移1b
                    tempGroups[0].WY_DQ[2] = ExpData_KFY.WY_DJPmax_F[step][2];//位移1c
                    tempGroups[0].WY_DQ[3] = ExpData_KFY.WY_DJPmax_F[step][3];//位移1d
                    tempGroups[1].WY_DQ[0] = ExpData_KFY.WY_DJPmax_F[step][4];//位移2a
                    tempGroups[1].WY_DQ[1] = ExpData_KFY.WY_DJPmax_F[step][5];//位移2b
                    tempGroups[1].WY_DQ[2] = ExpData_KFY.WY_DJPmax_F[step][6];//位移2c
                    tempGroups[2].WY_DQ[0] = ExpData_KFY.WY_DJPmax_F[step][7];//位移3a
                    tempGroups[2].WY_DQ[1] = ExpData_KFY.WY_DJPmax_F[step][8];//位移3b
                    tempGroups[2].WY_DQ[2] = ExpData_KFY.WY_DJPmax_F[step][9];//位移3c
                    //重新计算各组挠度
                    for (int i = 0; i < Exp_KFY.DisplaceGroups.Count; i++)
                    {
                        tempGroups[i].NDJS();
                        //挠度重新赋值
                        ExpData_KFY.XDND_DJPmax_F[step][i] = tempGroups[i].ND_XD;
                        ExpData_KFY.ND_DJPmax_F[step][i] = tempGroups[i].ND;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            try
            {
                //工程P1正压各步骤
                //初始位移
                tempGroups[0].WY_CS[0] = ExpData_KFY.WY_GCBX_Z[0][0];//位移1a
                tempGroups[0].WY_CS[1] = ExpData_KFY.WY_GCBX_Z[0][1];//位移1b
                tempGroups[0].WY_CS[2] = ExpData_KFY.WY_GCBX_Z[0][2];//位移1c
                tempGroups[0].WY_CS[3] = ExpData_KFY.WY_GCBX_Z[0][3];//位移1d
                tempGroups[1].WY_CS[0] = ExpData_KFY.WY_GCBX_Z[0][4];//位移2a
                tempGroups[1].WY_CS[1] = ExpData_KFY.WY_GCBX_Z[0][5];//位移2b
                tempGroups[1].WY_CS[2] = ExpData_KFY.WY_GCBX_Z[0][6];//位移2c
                tempGroups[2].WY_CS[0] = ExpData_KFY.WY_GCBX_Z[0][7];//位移3a
                tempGroups[2].WY_CS[1] = ExpData_KFY.WY_GCBX_Z[0][8];//位移3b
                tempGroups[2].WY_CS[2] = ExpData_KFY.WY_GCBX_Z[0][9];//位移3c
                //各步重新计算
                for (int step = 0; step < ExpData_KFY.WY_GCBX_Z.Count; step++)
                {
                    tempGroups[0].WY_DQ[0] = ExpData_KFY.WY_GCBX_Z[step][0];//位移1a
                    tempGroups[0].WY_DQ[1] = ExpData_KFY.WY_GCBX_Z[step][1];//位移1b
                    tempGroups[0].WY_DQ[2] = ExpData_KFY.WY_GCBX_Z[step][2];//位移1c
                    tempGroups[0].WY_DQ[3] = ExpData_KFY.WY_GCBX_Z[step][3];//位移1d
                    tempGroups[1].WY_DQ[0] = ExpData_KFY.WY_GCBX_Z[step][4];//位移2a
                    tempGroups[1].WY_DQ[1] = ExpData_KFY.WY_GCBX_Z[step][5];//位移2b
                    tempGroups[1].WY_DQ[2] = ExpData_KFY.WY_GCBX_Z[step][6];//位移2c
                    tempGroups[2].WY_DQ[0] = ExpData_KFY.WY_GCBX_Z[step][7];//位移3a
                    tempGroups[2].WY_DQ[1] = ExpData_KFY.WY_GCBX_Z[step][8];//位移3b
                    tempGroups[2].WY_DQ[2] = ExpData_KFY.WY_GCBX_Z[step][9];//位移3c
                    //重新计算各组挠度
                    for (int i = 0; i < Exp_KFY.DisplaceGroups.Count; i++)
                    {
                        tempGroups[i].NDJS();
                        //挠度重新赋值
                        ExpData_KFY.XDND_GCBX_Z[step][i] = tempGroups[i].ND_XD;
                        ExpData_KFY.ND_GCBX_Z[step][i] = tempGroups[i].ND;
                    }
                }

                //工程P1负压各步骤
                //初始位移
                tempGroups[0].WY_CS[0] = ExpData_KFY.WY_GCBX_F[0][0];//位移1a
                tempGroups[0].WY_CS[1] = ExpData_KFY.WY_GCBX_F[0][1];//位移1b
                tempGroups[0].WY_CS[2] = ExpData_KFY.WY_GCBX_F[0][2];//位移1c
                tempGroups[0].WY_CS[3] = ExpData_KFY.WY_GCBX_F[0][3];//位移1d
                tempGroups[1].WY_CS[0] = ExpData_KFY.WY_GCBX_F[0][4];//位移2a
                tempGroups[1].WY_CS[1] = ExpData_KFY.WY_GCBX_F[0][5];//位移2b
                tempGroups[1].WY_CS[2] = ExpData_KFY.WY_GCBX_F[0][6];//位移2c
                tempGroups[2].WY_CS[0] = ExpData_KFY.WY_GCBX_F[0][7];//位移3a
                tempGroups[2].WY_CS[1] = ExpData_KFY.WY_GCBX_F[0][8];//位移3b
                tempGroups[2].WY_CS[2] = ExpData_KFY.WY_GCBX_F[0][9];//位移3c
                //各步重新计算
                for (int step = 0; step < ExpData_KFY.WY_GCBX_F.Count; step++)
                {
                    tempGroups[0].WY_DQ[0] = ExpData_KFY.WY_GCBX_F[step][0];//位移1a
                    tempGroups[0].WY_DQ[1] = ExpData_KFY.WY_GCBX_F[step][1];//位移1b
                    tempGroups[0].WY_DQ[2] = ExpData_KFY.WY_GCBX_F[step][2];//位移1c
                    tempGroups[0].WY_DQ[3] = ExpData_KFY.WY_GCBX_F[step][3];//位移1d
                    tempGroups[1].WY_DQ[0] = ExpData_KFY.WY_GCBX_F[step][4];//位移2a
                    tempGroups[1].WY_DQ[1] = ExpData_KFY.WY_GCBX_F[step][5];//位移2b
                    tempGroups[1].WY_DQ[2] = ExpData_KFY.WY_GCBX_F[step][6];//位移2c
                    tempGroups[2].WY_DQ[0] = ExpData_KFY.WY_GCBX_F[step][7];//位移3a
                    tempGroups[2].WY_DQ[1] = ExpData_KFY.WY_GCBX_F[step][8];//位移3b
                    tempGroups[2].WY_DQ[2] = ExpData_KFY.WY_GCBX_F[step][9];//位移3c
                    //重新计算各组挠度
                    for (int i = 0; i < Exp_KFY.DisplaceGroups.Count; i++)
                    {
                        tempGroups[i].NDJS();
                        //挠度重新赋值
                        ExpData_KFY.XDND_GCBX_F[step][i] = tempGroups[i].ND_XD;
                        ExpData_KFY.ND_GCBX_F[step][i] = tempGroups[i].ND;
                    }
                }

                //工程P3正压各步骤
                //初始位移
                tempGroups[0].WY_CS[0] = ExpData_KFY.WY_GCP3_Z[0][0];//位移1a
                tempGroups[0].WY_CS[1] = ExpData_KFY.WY_GCP3_Z[0][1];//位移1b
                tempGroups[0].WY_CS[2] = ExpData_KFY.WY_GCP3_Z[0][2];//位移1c
                tempGroups[0].WY_CS[3] = ExpData_KFY.WY_GCP3_Z[0][3];//位移1d
                tempGroups[1].WY_CS[0] = ExpData_KFY.WY_GCP3_Z[0][4];//位移2a
                tempGroups[1].WY_CS[1] = ExpData_KFY.WY_GCP3_Z[0][5];//位移2b
                tempGroups[1].WY_CS[2] = ExpData_KFY.WY_GCP3_Z[0][6];//位移2c
                tempGroups[2].WY_CS[0] = ExpData_KFY.WY_GCP3_Z[0][7];//位移3a
                tempGroups[2].WY_CS[1] = ExpData_KFY.WY_GCP3_Z[0][8];//位移3b
                tempGroups[2].WY_CS[2] = ExpData_KFY.WY_GCP3_Z[0][9];//位移3c
                //各步重新计算
                for (int step = 0; step < ExpData_KFY.WY_GCP3_Z.Count; step++)
                {
                    tempGroups[0].WY_DQ[0] = ExpData_KFY.WY_GCP3_Z[step][0];//位移1a
                    tempGroups[0].WY_DQ[1] = ExpData_KFY.WY_GCP3_Z[step][1];//位移1b
                    tempGroups[0].WY_DQ[2] = ExpData_KFY.WY_GCP3_Z[step][2];//位移1c
                    tempGroups[0].WY_DQ[3] = ExpData_KFY.WY_GCP3_Z[step][3];//位移1d
                    tempGroups[1].WY_DQ[0] = ExpData_KFY.WY_GCP3_Z[step][4];//位移2a
                    tempGroups[1].WY_DQ[1] = ExpData_KFY.WY_GCP3_Z[step][5];//位移2b
                    tempGroups[1].WY_DQ[2] = ExpData_KFY.WY_GCP3_Z[step][6];//位移2c
                    tempGroups[2].WY_DQ[0] = ExpData_KFY.WY_GCP3_Z[step][7];//位移3a
                    tempGroups[2].WY_DQ[1] = ExpData_KFY.WY_GCP3_Z[step][8];//位移3b
                    tempGroups[2].WY_DQ[2] = ExpData_KFY.WY_GCP3_Z[step][9];//位移3c
                    //重新计算各组挠度
                    for (int i = 0; i < Exp_KFY.DisplaceGroups.Count; i++)
                    {
                        tempGroups[i].NDJS();
                        //挠度重新赋值
                        ExpData_KFY.XDND_GCP3_Z[step][i] = tempGroups[i].ND_XD;
                        ExpData_KFY.ND_GCP3_Z[step][i] = tempGroups[i].ND;
                    }
                }

                //工程P3负压各步骤
                //初始位移
                tempGroups[0].WY_CS[0] = ExpData_KFY.WY_GCP3_F[0][0];//位移1a
                tempGroups[0].WY_CS[1] = ExpData_KFY.WY_GCP3_F[0][1];//位移1b
                tempGroups[0].WY_CS[2] = ExpData_KFY.WY_GCP3_F[0][2];//位移1c
                tempGroups[0].WY_CS[3] = ExpData_KFY.WY_GCP3_F[0][3];//位移1d
                tempGroups[1].WY_CS[0] = ExpData_KFY.WY_GCP3_F[0][4];//位移2a
                tempGroups[1].WY_CS[1] = ExpData_KFY.WY_GCP3_F[0][5];//位移2b
                tempGroups[1].WY_CS[2] = ExpData_KFY.WY_GCP3_F[0][6];//位移2c
                tempGroups[2].WY_CS[0] = ExpData_KFY.WY_GCP3_F[0][7];//位移3a
                tempGroups[2].WY_CS[1] = ExpData_KFY.WY_GCP3_F[0][8];//位移3b
                tempGroups[2].WY_CS[2] = ExpData_KFY.WY_GCP3_F[0][9];//位移3c
                //各步重新计算
                for (int step = 0; step < ExpData_KFY.WY_GCP3_F.Count; step++)
                {
                    tempGroups[0].WY_DQ[0] = ExpData_KFY.WY_GCP3_F[step][0];//位移1a
                    tempGroups[0].WY_DQ[1] = ExpData_KFY.WY_GCP3_F[step][1];//位移1b
                    tempGroups[0].WY_DQ[2] = ExpData_KFY.WY_GCP3_F[step][2];//位移1c
                    tempGroups[0].WY_DQ[3] = ExpData_KFY.WY_GCP3_F[step][3];//位移1d
                    tempGroups[1].WY_DQ[0] = ExpData_KFY.WY_GCP3_F[step][4];//位移2a
                    tempGroups[1].WY_DQ[1] = ExpData_KFY.WY_GCP3_F[step][5];//位移2b
                    tempGroups[1].WY_DQ[2] = ExpData_KFY.WY_GCP3_F[step][6];//位移2c
                    tempGroups[2].WY_DQ[0] = ExpData_KFY.WY_GCP3_F[step][7];//位移3a
                    tempGroups[2].WY_DQ[1] = ExpData_KFY.WY_GCP3_F[step][8];//位移3b
                    tempGroups[2].WY_DQ[2] = ExpData_KFY.WY_GCP3_F[step][9];//位移3c
                    //重新计算各组挠度
                    for (int i = 0; i < Exp_KFY.DisplaceGroups.Count; i++)
                    {
                        tempGroups[i].NDJS();
                        //挠度重新赋值
                        ExpData_KFY.XDND_GCP3_F[step][i] = tempGroups[i].ND_XD;
                        ExpData_KFY.ND_GCP3_F[step][i] = tempGroups[i].ND;
                    }
                }

                //工程Pmax正压各步骤
                //初始位移
                tempGroups[0].WY_CS[0] = ExpData_KFY.WY_GCPmax_Z[0][0];//位移1a
                tempGroups[0].WY_CS[1] = ExpData_KFY.WY_GCPmax_Z[0][1];//位移1b
                tempGroups[0].WY_CS[2] = ExpData_KFY.WY_GCPmax_Z[0][2];//位移1c
                tempGroups[0].WY_CS[3] = ExpData_KFY.WY_GCPmax_Z[0][3];//位移1d
                tempGroups[1].WY_CS[0] = ExpData_KFY.WY_GCPmax_Z[0][4];//位移2a
                tempGroups[1].WY_CS[1] = ExpData_KFY.WY_GCPmax_Z[0][5];//位移2b
                tempGroups[1].WY_CS[2] = ExpData_KFY.WY_GCPmax_Z[0][6];//位移2c
                tempGroups[2].WY_CS[0] = ExpData_KFY.WY_GCPmax_Z[0][7];//位移3a
                tempGroups[2].WY_CS[1] = ExpData_KFY.WY_GCPmax_Z[0][8];//位移3b
                tempGroups[2].WY_CS[2] = ExpData_KFY.WY_GCPmax_Z[0][9];//位移3c
                //各步重新计算
                for (int step = 0; step < ExpData_KFY.WY_GCPmax_Z.Count; step++)
                {
                    tempGroups[0].WY_DQ[0] = ExpData_KFY.WY_GCPmax_Z[step][0];//位移1a
                    tempGroups[0].WY_DQ[1] = ExpData_KFY.WY_GCPmax_Z[step][1];//位移1b
                    tempGroups[0].WY_DQ[2] = ExpData_KFY.WY_GCPmax_Z[step][2];//位移1c
                    tempGroups[0].WY_DQ[3] = ExpData_KFY.WY_GCPmax_Z[step][3];//位移1d
                    tempGroups[1].WY_DQ[0] = ExpData_KFY.WY_GCPmax_Z[step][4];//位移2a
                    tempGroups[1].WY_DQ[1] = ExpData_KFY.WY_GCPmax_Z[step][5];//位移2b
                    tempGroups[1].WY_DQ[2] = ExpData_KFY.WY_GCPmax_Z[step][6];//位移2c
                    tempGroups[2].WY_DQ[0] = ExpData_KFY.WY_GCPmax_Z[step][7];//位移3a
                    tempGroups[2].WY_DQ[1] = ExpData_KFY.WY_GCPmax_Z[step][8];//位移3b
                    tempGroups[2].WY_DQ[2] = ExpData_KFY.WY_GCPmax_Z[step][9];//位移3c
                    //重新计算各组挠度
                    for (int i = 0; i < Exp_KFY.DisplaceGroups.Count; i++)
                    {
                        tempGroups[i].NDJS();
                        //挠度重新赋值
                        ExpData_KFY.XDND_GCPmax_Z[step][i] = tempGroups[i].ND_XD;
                        ExpData_KFY.ND_GCPmax_Z[step][i] = tempGroups[i].ND;
                    }
                }

                //工程Pmax负压各步骤
                //初始位移
                tempGroups[0].WY_CS[0] = ExpData_KFY.WY_GCPmax_F[0][0];//位移1a
                tempGroups[0].WY_CS[1] = ExpData_KFY.WY_GCPmax_F[0][1];//位移1b
                tempGroups[0].WY_CS[2] = ExpData_KFY.WY_GCPmax_F[0][2];//位移1c
                tempGroups[0].WY_CS[3] = ExpData_KFY.WY_GCPmax_F[0][3];//位移1d
                tempGroups[1].WY_CS[0] = ExpData_KFY.WY_GCPmax_F[0][4];//位移2a
                tempGroups[1].WY_CS[1] = ExpData_KFY.WY_GCPmax_F[0][5];//位移2b
                tempGroups[1].WY_CS[2] = ExpData_KFY.WY_GCPmax_F[0][6];//位移2c
                tempGroups[2].WY_CS[0] = ExpData_KFY.WY_GCPmax_F[0][7];//位移3a
                tempGroups[2].WY_CS[1] = ExpData_KFY.WY_GCPmax_F[0][8];//位移3b
                tempGroups[2].WY_CS[2] = ExpData_KFY.WY_GCPmax_F[0][9];//位移3c
                //各步重新计算
                for (int step = 0; step < ExpData_KFY.WY_GCPmax_F.Count; step++)
                {
                    tempGroups[0].WY_DQ[0] = ExpData_KFY.WY_GCPmax_F[step][0];//位移1a
                    tempGroups[0].WY_DQ[1] = ExpData_KFY.WY_GCPmax_F[step][1];//位移1b
                    tempGroups[0].WY_DQ[2] = ExpData_KFY.WY_GCPmax_F[step][2];//位移1c
                    tempGroups[0].WY_DQ[3] = ExpData_KFY.WY_GCPmax_F[step][3];//位移1d
                    tempGroups[1].WY_DQ[0] = ExpData_KFY.WY_GCPmax_F[step][4];//位移2a
                    tempGroups[1].WY_DQ[1] = ExpData_KFY.WY_GCPmax_F[step][5];//位移2b
                    tempGroups[1].WY_DQ[2] = ExpData_KFY.WY_GCPmax_F[step][6];//位移2c
                    tempGroups[2].WY_DQ[0] = ExpData_KFY.WY_GCPmax_F[step][7];//位移3a
                    tempGroups[2].WY_DQ[1] = ExpData_KFY.WY_GCPmax_F[step][8];//位移3b
                    tempGroups[2].WY_DQ[2] = ExpData_KFY.WY_GCPmax_F[step][9];//位移3c
                    //重新计算各组挠度
                    for (int i = 0; i < Exp_KFY.DisplaceGroups.Count; i++)
                    {
                        tempGroups[i].NDJS();
                        //挠度重新赋值
                        ExpData_KFY.XDND_GCPmax_F[step][i] = tempGroups[i].ND_XD;
                        ExpData_KFY.ND_GCPmax_F[step][i] = tempGroups[i].ND;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }


            //变形阶段正压评定p1
            try
            {
                //第1组测点
                if (Exp_KFY.DisplaceGroups[0].Is_Use)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        if (!ExpData_KFY.Damage_DJBX_Z[i])
                        {
                            //未损坏且未超限的最高压力
                            if (Math.Abs(ExpData_KFY.XDND_DJBX_Z[i + 1][0]) <= tempYXND1)
                            {
                                tempNDok1 = Math.Abs(ExpData_KFY.XDND_DJBX_Z[i + 1][0]);
                                tempPokMax1 = Math.Abs(ExpData_KFY.TestPress_DJBX_Z[i]);
                            }
                            //未损坏但超限的最高压力
                            else
                            {
                                tempNDover1 = Math.Abs(ExpData_KFY.XDND_DJBX_Z[i + 1][0]);
                                tempPover1 = Math.Abs(ExpData_KFY.TestPress_DJBX_Z[i]);
                                break;
                            }
                        }
                        else
                            break;
                    }

                    //若完好压力大于超限压力，则p1为完好压力
                    if (tempPokMax1 > tempPover1)
                        tempP11 = tempPokMax1;
                    //若完好压力小于超限压力，则线性计算出临界压力
                    else
                    {
                        if ((tempNDover1 < tempNDok1) || (tempNDover1 > tempNDok1))
                            tempP11 = tempPokMax1 + (tempYXND1 - tempNDok1) * (tempPover1 - tempPokMax1) / (tempNDover1 - tempNDok1);
                        else
                            tempP11 = tempNDok1;
                    }
                    if (tempP11 < 0)
                        tempP11 = 0;
                }

                //第2组测点
                if (Exp_KFY.DisplaceGroups[1].Is_Use)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        if (!ExpData_KFY.Damage_DJBX_Z[i])
                        {
                            //未损坏且未超限的最高压力
                            if (Math.Abs(ExpData_KFY.XDND_DJBX_Z[i + 1][1]) <= tempYXND2)
                            {
                                tempNDok2 = Math.Abs(ExpData_KFY.XDND_DJBX_Z[i + 1][1]);
                                tempPokMax2 = Math.Abs(ExpData_KFY.TestPress_DJBX_Z[i]);
                            }
                            //未损坏但超限的最高压力
                            else
                            {
                                tempNDover2 = Math.Abs(ExpData_KFY.XDND_DJBX_Z[i + 1][1]);
                                tempPover2 = Math.Abs(ExpData_KFY.TestPress_DJBX_Z[i]);
                                break;
                            }
                        }
                        else
                            break;
                    }

                    //若完好压力大于超限压力，则p1为完好压力
                    if (tempPokMax2 > tempPover2)
                        tempP12 = tempPokMax2;
                    //若完好压力小于超限压力，则线性计算出临界压力
                    else
                    {
                        if ((tempNDover2 < tempNDok2) || (tempNDover2 > tempNDok2))
                            tempP12 = tempPokMax2 + (tempYXND2 - tempNDok2) * (tempPover2 - tempPokMax2) / (tempNDover2 - tempNDok2);
                        else
                            tempP12 = tempNDok2;
                    }
                    if (tempP12 < 0)
                        tempP12 = 0;
                }

                //第3组测点
                if (Exp_KFY.DisplaceGroups[2].Is_Use)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        if (!ExpData_KFY.Damage_DJBX_Z[i])
                        {
                            //未损坏且未超限的最高压力
                            if (Math.Abs(ExpData_KFY.XDND_DJBX_Z[i + 1][2]) <= tempYXND2)
                            {
                                tempNDok3 = Math.Abs(ExpData_KFY.XDND_DJBX_Z[i + 1][2]);
                                tempPokMax3 = Math.Abs(ExpData_KFY.TestPress_DJBX_Z[i]);
                            }
                            //未损坏但超限的最高压力
                            else
                            {
                                tempNDover3 = Math.Abs(ExpData_KFY.XDND_DJBX_Z[i + 1][2]);
                                tempPover3 = Math.Abs(ExpData_KFY.TestPress_DJBX_Z[i]);
                                break;
                            }
                        }
                        else
                            break;
                    }

                    //若完好压力大于超限压力，则p1为完好压力
                    if (tempPokMax3 > tempPover3)
                        tempP13 = tempPokMax3;
                    //若完好压力小于超限压力，则线性计算出临界压力
                    else
                    {
                        if ((tempNDover3 < tempNDok3) || (tempNDover3 > tempNDok3))
                            tempP13 = tempPokMax3 + (tempYXND3 - tempNDok3) * (tempPover3 - tempPokMax3) / (tempNDover3 - tempNDok3);
                        else
                            tempP13 = tempNDok3;
                    }
                    if (tempP13 < 0)
                        tempP13 = 0;
                }
                ExpData_KFY.PDValue_DJP1_Z = MinP(tempP11, tempP12, tempP13, Exp_KFY.DisplaceGroups[0].Is_Use,Exp_KFY.DisplaceGroups[1].Is_Use,Exp_KFY.DisplaceGroups[2].Is_Use);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            //变形阶段负压评定p1
            try
            {
                tempPokMax1 = 0; //不超限、无损坏的最高压力
                tempPokMax2 = 0; //不超限、无损坏的最高压力
                tempPokMax3 = 0; //不超限、无损坏的最高压力
                tempPover1 = 0; //超限压力
                tempPover2 = 0;
                tempPover3 = 0;
                tempNDok1 = 0;
                tempNDover1 = 0;
                tempNDok2 = 0;
                tempNDover2 = 0;
                tempNDok3 = 0;
                tempNDover3 = 0;
                tempP11 = 0;
                tempP12 = 0;
                tempP13 = 0;
                //第1组测点
                if (Exp_KFY.DisplaceGroups[0].Is_Use)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        if (!ExpData_KFY.Damage_DJBX_F[i])
                        {
                            //未损坏且未超限的最高压力
                            if (Math.Abs(ExpData_KFY.XDND_DJBX_F[i + 1][0]) <= tempYXND1)
                            {
                                tempNDok1 = Math.Abs(ExpData_KFY.XDND_DJBX_F[i + 1][0]);
                                tempPokMax1 = Math.Abs(ExpData_KFY.TestPress_DJBX_F[i]);
                            }
                            //未损坏但超限的最高压力
                            else
                            {
                                tempNDover1 = Math.Abs(ExpData_KFY.XDND_DJBX_F[i + 1][0]);
                                tempPover1 = Math.Abs(ExpData_KFY.TestPress_DJBX_F[i]);
                                break;
                            }
                        }
                        else
                            break;
                    }

                    //若完好压力大于超限压力，则p1为完好压力
                    if (tempPokMax1 > tempPover1)
                        tempP11 = tempPokMax1;
                    //若完好压力小于超限压力，则线性计算出临界压力
                    else
                    {
                        if ((tempNDover1 < tempNDok1) || (tempNDover1 > tempNDok1))
                            tempP11 = tempPokMax1 + (tempYXND1 - tempNDok1) * (tempPover1 - tempPokMax1) / (tempNDover1 - tempNDok1);
                        else
                            tempP11 = tempNDok1;
                    }
                    if (tempP11 < 0)
                        tempP11 = 0;
                }

                //第2组测点
                if (Exp_KFY.DisplaceGroups[1].Is_Use)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        if (!ExpData_KFY.Damage_DJBX_F[i])
                        {
                            //未损坏且未超限的最高压力
                            if (Math.Abs(ExpData_KFY.XDND_DJBX_F[i + 1][1]) <= tempYXND2)
                            {
                                tempNDok2 = Math.Abs(ExpData_KFY.XDND_DJBX_F[i + 1][1]);
                                tempPokMax2 = Math.Abs(ExpData_KFY.TestPress_DJBX_F[i]);
                            }
                            //未损坏但超限的最高压力
                            else
                            {
                                tempNDover2 = Math.Abs(ExpData_KFY.XDND_DJBX_F[i + 1][1]);
                                tempPover2 = Math.Abs(ExpData_KFY.TestPress_DJBX_F[i]);
                                break;
                            }
                        }
                        else
                            break;
                    }

                    //若完好压力大于超限压力，则p1为完好压力
                    if (tempPokMax2 > tempPover2)
                        tempP12 = tempPokMax2;
                    //若完好压力小于超限压力，则线性计算出临界压力
                    else
                    {
                        if ((tempNDover2 < tempNDok2) || (tempNDover2 > tempNDok2))
                            tempP12 = tempPokMax2 + (tempYXND2 - tempNDok2) * (tempPover2 - tempPokMax2) /
                                      (tempNDover2 - tempNDok2);
                        else
                            tempP12 = tempNDok2;
                    }
                    if (tempP12 < 0)
                        tempP12 = 0;
                }

                //第3组测点
                if (Exp_KFY.DisplaceGroups[2].Is_Use)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        if (!ExpData_KFY.Damage_DJBX_F[i])
                        {
                            //未损坏且未超限的最高压力
                            if (Math.Abs(ExpData_KFY.XDND_DJBX_F[i + 1][2]) <= tempYXND2)
                            {
                                tempNDok3 = Math.Abs(ExpData_KFY.XDND_DJBX_F[i + 1][2]);
                                tempPokMax3 = Math.Abs(ExpData_KFY.TestPress_DJBX_F[i]);
                            }
                            //未损坏但超限的最高压力
                            else
                            {
                                tempNDover3 = Math.Abs(ExpData_KFY.XDND_DJBX_F[i + 1][2]);
                                tempPover3 = Math.Abs(ExpData_KFY.TestPress_DJBX_F[i]);
                                break;
                            }
                        }
                        else
                            break;
                    }

                    //若完好压力大于超限压力，则p1为完好压力
                    if (tempPokMax3 > tempPover3)
                        tempP13 = tempPokMax3;
                    //若完好压力小于超限压力，则线性计算出临界压力
                    else
                    {
                        if ((tempNDover3 < tempNDok3) || (tempNDover3 > tempNDok3))
                            tempP13 = tempPokMax3 + (tempYXND3 - tempNDok3) * (tempPover3 - tempPokMax3) /
                                  (tempNDover3 - tempNDok3);
                        else
                            tempP13 = tempNDok3;
                    }
                    if (tempP13 < 0)
                        tempP13 = 0;
                }
                ExpData_KFY.PDValue_DJP1_F = -MinP(tempP11, tempP12, tempP13, Exp_KFY.DisplaceGroups[0].Is_Use, Exp_KFY.DisplaceGroups[1].Is_Use, Exp_KFY.DisplaceGroups[2].Is_Use);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            //P1定级总评定
            ExpData_KFY.PDValue_DJP1_All = Math.Min(Math.Abs(ExpData_KFY.PDValue_DJP1_Z), Math.Abs(ExpData_KFY.PDValue_DJP1_F));

            //P3评定
            try
            {
                ExpData_KFY.PDValue_DJP3_Z = 2.5 * ExpData_KFY.PDValue_DJP1_All;
                ExpData_KFY.PDValue_DJP3_F = -ExpData_KFY.PDValue_DJP3_Z;
                ExpData_KFY.PDValue_DJP3_All = Math.Min(Math.Abs(ExpData_KFY.PDValue_DJP3_Z), Math.Abs(ExpData_KFY.PDValue_DJP3_F));

                double p1LittleZ = ExpData_KFY.PDValue_DJP1_Z;
                double perr = ExpData_KFY.PDValue_DJP1_Z;
                //若P2检测有损坏（正压负压都一样），则选取P1评定值为P3评定值
                if( (ExpData_KFY.Damage_DJP2_Z)|| (ExpData_KFY.Damage_DJP2_F))
                {
                    ExpData_KFY.PDValue_DJP3_Z =  ExpData_KFY.PDValue_DJP1_All;
                    ExpData_KFY.PDValue_DJP3_F = -ExpData_KFY.PDValue_DJP3_Z;
                    ExpData_KFY.PDValue_DJP3_All = ExpData_KFY.PDValue_DJP3_Z;
                }
                //若P2检测无损坏但P3正压检测有损坏，取P3检测压力的前一级为P3（评级标准中的级别）
                else if (ExpData_KFY.Damage_DJP3_Z)
                {
                    ExpData_KFY.PDValue_DJP3_Z= GetPressPrecedingSatge(ExpData_KFY.TestPress_DJP3_Z);
                    ExpData_KFY.PDValue_DJP3_F = -ExpData_KFY.PDValue_DJP3_Z;
                    ExpData_KFY.PDValue_DJP3_All = ExpData_KFY.PDValue_DJP3_Z / 1.4;
                }
                //若P2检测、P3正压检测均无损坏，则继续评定（先计算P3正压，P3负压损坏问题在下面考虑）
                else
                {
                    ExpData_KFY.PDValue_DJP3_Z = ExpData_KFY.TestPress_DJP3_Z;

                    //负压评定，有损坏则按前一级，且P3评定值/1.4
                    if (ExpData_KFY.Damage_DJP3_F)
                    {
                        ExpData_KFY.PDValue_DJP3_F = GetPressPrecedingSatge(ExpData_KFY.TestPress_DJP3_F);
                        ExpData_KFY.PDValue_DJP3_All = Math.Min(Math.Abs(ExpData_KFY.PDValue_DJP3_Z), Math.Abs(ExpData_KFY.PDValue_DJP3_F))/1.4;
                    }
                    else
                    {
                        ExpData_KFY.PDValue_DJP3_F = ExpData_KFY.TestPress_DJP3_F;

                        //p3无损坏，pmax有损坏时，选P3/1.4
                        if ((ExpData_KFY.Damage_DJPmax_Z) || (ExpData_KFY.Damage_DJPmax_Z))
                        {
                            ExpData_KFY.PDValue_DJP3_All = Math.Min(Math.Abs(ExpData_KFY.PDValue_DJP3_Z), Math.Abs(ExpData_KFY.PDValue_DJP3_F)) / 1.4;
                        }
                        else
                        {
                            ExpData_KFY.PDValue_DJP3_All = Math.Min(Math.Abs(ExpData_KFY.PDValue_DJP3_Z), Math.Abs(ExpData_KFY.PDValue_DJP3_F));
                        }
                    }
                }

                double p3 = ExpData_KFY.PDValue_DJP3_All;
                if ((p3 >= 1000) && (p3 < 1500))
                    ExpData_KFY.PdLevel_DJP3 = 1;
                else if ((p3 >= 1500) && (p3 < 2000))
                    ExpData_KFY.PdLevel_DJP3 = 2;
                else if ((p3 >= 2000) && (p3 < 2500))
                    ExpData_KFY.PdLevel_DJP3 = 3;
                else if ((p3 >= 2500) && (p3 < 3000))
                    ExpData_KFY.PdLevel_DJP3 = 4;
                else if ((p3 >= 3000) && (p3 < 3500))
                    ExpData_KFY.PdLevel_DJP3 = 5;
                else if ((p3 >= 3500) && (p3 < 4000))
                    ExpData_KFY.PdLevel_DJP3 = 6;
                else if ((p3 >= 4000) && (p3 < 4500))
                    ExpData_KFY.PdLevel_DJP3 = 7;
                else if ((p3 >= 4500) && (p3 < 5000))
                    ExpData_KFY.PdLevel_DJP3 = 8;
                else if (p3 >= 5000)
                    ExpData_KFY.PdLevel_DJP3 = 9;
                else
                    ExpData_KFY.PdLevel_DJP3 = 0;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }


            //工程评定
            try
            {
                ExpData_KFY.IsMeetDesign_GCp1 = true;
                for (int i = 0; i < ExpData_KFY.Damage_GCBX_Z.Count; i++)
                {
                    if (ExpData_KFY.Damage_GCBX_Z[i])
                        ExpData_KFY.IsMeetDesign_GCp1 = false;
                }
                for (int i = 0; i < ExpData_KFY.Damage_GCBX_F.Count; i++)
                {
                    if (ExpData_KFY.Damage_GCBX_F[i])
                        ExpData_KFY.IsMeetDesign_GCp1 = false;
                }

                ExpData_KFY.IsMeetDesign_GCp2 = true;
                if (ExpData_KFY.Damage_GCP2_Z)
                    ExpData_KFY.IsMeetDesign_GCp2 = false;
                if (ExpData_KFY.Damage_GCP2_F)
                    ExpData_KFY.IsMeetDesign_GCp2 = false;

                ExpData_KFY.IsMeetDesign_GCp3 = true;
                if (ExpData_KFY.Damage_GCP3_Z)
                    ExpData_KFY.IsMeetDesign_GCp3 = false;
                if (ExpData_KFY.Damage_GCP3_F)
                    ExpData_KFY.IsMeetDesign_GCp3 = false;

                ExpData_KFY.IsMeetDesign_GCpmax = true;
                if (ExpData_KFY.Damage_GCPmax_Z)
                    ExpData_KFY.IsMeetDesign_GCpmax = false;
                if (ExpData_KFY.Damage_GCPmax_F)
                    ExpData_KFY.IsMeetDesign_GCpmax = false;

                if (ExpData_KFY.IsXDND_Over_GCp3_Z)
                    ExpData_KFY.IsMeetDesign_GCp3 = false;
                if (ExpData_KFY.IsXDND_Over_GCp3_F)
                    ExpData_KFY.IsMeetDesign_GCp3 = false;

                ExpData_KFY.IsMeetDesign_GCfinal = (ExpData_KFY.IsMeetDesign_GCp1 && ExpData_KFY.IsMeetDesign_GCp2 &&
                                                    ExpData_KFY.IsMeetDesign_GCp3 && ExpData_KFY.IsMeetDesign_GCpmax);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        #endregion


        #region 层间变形测试数据计算及评价

        /// <summary>
        /// 层间变形定级检测数据计算及评价
        /// </summary>
        public void CJBX_DJEvaluate()
        {
            //X轴数据分析
            for (int i = 0; i < 6; i++)
            {
                if (!ExpData_CJBX.IsDamage_DJ_X[i])
                {
                    ExpData_CJBX.MaxAngleFM_DJ_X = ExpData_CJBX.AngleFM_DJ_X[i];
                    ExpData_CJBX.MaxDSPL_DJ_X= ExpData_CJBX.DSPL_DJ_X[i];
                }
                else
                {
                    ExpData_CJBX.IsDamageFinal_DJ_X = true;
                    ExpData_CJBX.DamageFinalPS_DJ_X = ExpData_CJBX.DamagePS_DJ_X[i];
                    break;
                }
            }
            //Y轴数据分析
            for (int i = 0; i < 6; i++)
            {
                if (!ExpData_CJBX.IsDamage_DJ_Y[i])
                {
                    ExpData_CJBX.MaxAngleFM_DJ_Y = ExpData_CJBX.AngleFM_DJ_Y[i];
                    ExpData_CJBX.MaxDSPL_DJ_Y = ExpData_CJBX.DSPL_DJ_Y[i];
                }
                else
                {
                    ExpData_CJBX.IsDamageFinal_DJ_Y = true;
                    ExpData_CJBX.DamageFinalPS_DJ_Y = ExpData_CJBX.DamagePS_DJ_Y[i];
                    break;
                }
            }
            //Z轴数据分析
            for (int i = 0; i < 6; i++)
            {
                if (!ExpData_CJBX.IsDamage_DJ_Z[i])
                {
                    ExpData_CJBX.MaxDSPL_DJ_Z = ExpData_CJBX.DSPL_DJ_Z[i];
                }
                else
                {
                    ExpData_CJBX.IsDamageFinal_DJ_Z = true;
                    ExpData_CJBX.DamageFinalPS_DJ_Z = ExpData_CJBX.DamagePS_DJ_Y[i];
                    break;
                }
            }

            //X轴数据评定
            if ((ExpData_CJBX.MaxAngleFM_DJ_X > 300) && (ExpData_CJBX.MaxAngleFM_DJ_X <= 400))
                ExpData_CJBX.Level_DJ_X = 1;
            else if ((ExpData_CJBX.MaxAngleFM_DJ_X > 200) && (ExpData_CJBX.MaxAngleFM_DJ_X <= 300))
                ExpData_CJBX.Level_DJ_X = 2;
            else if ((ExpData_CJBX.MaxAngleFM_DJ_X > 150) && (ExpData_CJBX.MaxAngleFM_DJ_X <= 200))
                ExpData_CJBX.Level_DJ_X = 3;
            else if ((ExpData_CJBX.MaxAngleFM_DJ_X > 100) && (ExpData_CJBX.MaxAngleFM_DJ_X <= 150))
                ExpData_CJBX.Level_DJ_X = 4;
            else if  ((ExpData_CJBX.MaxAngleFM_DJ_X <= 100)&& (ExpData_CJBX.MaxAngleFM_DJ_X > 0))
                ExpData_CJBX.Level_DJ_X = 5;
            else
                ExpData_CJBX.Level_DJ_X = 0;

            //Y轴数据评定
            if ((ExpData_CJBX.MaxAngleFM_DJ_Y > 300) && (ExpData_CJBX.MaxAngleFM_DJ_Y <= 400))
                ExpData_CJBX.Level_DJ_Y = 1;
            else if ((ExpData_CJBX.MaxAngleFM_DJ_Y > 200) && (ExpData_CJBX.MaxAngleFM_DJ_Y <= 300))
                ExpData_CJBX.Level_DJ_Y = 2;
            else if ((ExpData_CJBX.MaxAngleFM_DJ_Y > 150) && (ExpData_CJBX.MaxAngleFM_DJ_Y <= 200))
                ExpData_CJBX.Level_DJ_Y = 3;
            else if ((ExpData_CJBX.MaxAngleFM_DJ_Y > 100) && (ExpData_CJBX.MaxAngleFM_DJ_Y <= 150))
                ExpData_CJBX.Level_DJ_Y = 4;
            else if ((ExpData_CJBX.MaxAngleFM_DJ_Y <= 100) && (ExpData_CJBX.MaxAngleFM_DJ_Y > 0))
                ExpData_CJBX.Level_DJ_Y = 5;
            else
                ExpData_CJBX.Level_DJ_Y = 0;

            //Z轴数据评定
            if ((ExpData_CJBX.MaxDSPL_DJ_Z >= 5) && (ExpData_CJBX.MaxDSPL_DJ_Z <10))
                ExpData_CJBX.Level_DJ_Z = 1;
            else if ((ExpData_CJBX.MaxDSPL_DJ_Z >= 10) && (ExpData_CJBX.MaxDSPL_DJ_Z <15))
                ExpData_CJBX.Level_DJ_Z = 2;
            else if ((ExpData_CJBX.MaxDSPL_DJ_Z >= 15) && (ExpData_CJBX.MaxDSPL_DJ_Z <20))
                ExpData_CJBX.Level_DJ_Z = 3;
            else if ((ExpData_CJBX.MaxDSPL_DJ_Z >= 20) && (ExpData_CJBX.MaxDSPL_DJ_Z <25))
                ExpData_CJBX.Level_DJ_Z = 4;
            else if (ExpData_CJBX.MaxDSPL_DJ_Z >= 25)
                ExpData_CJBX.Level_DJ_Z = 5;
            else
                ExpData_CJBX.Level_DJ_Z = 0;
        }

        /// <summary>
        /// 层间变形工程检测数据计算及评价
        /// </summary>
        public void CJBX_GCEvaluate()
        {
            ExpData_CJBX.IsMeetDesign_GC_X = ExpData_CJBX.IsDamage_GC_X ? false : true;
            ExpData_CJBX.IsMeetDesign_GC_Y = ExpData_CJBX.IsDamage_GC_Y ? false : true;
            ExpData_CJBX.IsMeetDesign_GC_Z = ExpData_CJBX.IsDamage_GC_Z ? false : true;

            if ((!ExpData_CJBX.IsDamage_GC_X) && (!ExpData_CJBX.IsDamage_GC_X) &&
                (!ExpData_CJBX.IsDamage_GC_X))
                ExpData_CJBX.IsMeetDesign_GC_All = true;
            else
                ExpData_CJBX.IsMeetDesign_GC_All = false;
        }

        #endregion


        /// <summary>
        /// 返回三个double的最小绝对值
        /// </summary>
        private double MinP(double i, double j, double k, bool isUse1, bool isUse2, bool isUse3)
        {
            double minP = 0;
            double a = Math.Abs(i);
            double b = Math.Abs(j);
            double c = Math.Abs(k);

            if (!isUse1)
            {
                if (!isUse2)
                    minP = Math.Abs(c);
                else if ((isUse2)&& (!isUse3))
                    minP = Math.Abs(b);
                else
                    minP = Math.Min(b, c);
            }
            else
            {
                if ((!isUse2) && (!isUse3))
                    minP = Math.Abs(a);
                else if (isUse2 && (!isUse3))
                    minP = Math.Min(a, b);
                else
                    minP = Math.Min(a, c);
            }
            return minP;
        }

        /// <summary>
        /// 获取当前压力的前一级压力（评级标准中的压力级别）
        /// </summary>
        /// <param name="pressNow"></param>
        /// <returns></returns>
        private double GetPressPrecedingSatge(double pressNow)
        {
            double pressAbs = Math.Abs(pressNow);
            double tempRET = 0;
            if (pressNow >= 5000)
                tempRET = 4500;
            else if ((pressNow >= 4500)&&(pressNow<5000))
                tempRET = 4000;
            else if ((pressNow >= 4000) && (pressNow <4500))
                tempRET = 3500;
            else if ((pressNow >= 3500) && (pressNow < 4000))
                tempRET = 3000;
            else if ((pressNow >= 3000) && (pressNow < 3500))
                tempRET = 2500;
            else if ((pressNow >= 2500) && (pressNow < 3000))
                tempRET = 2000;
            else if ((pressNow >= 2000) && (pressNow < 2500))
                tempRET =1500;
            else if ((pressNow >= 1500) && (pressNow < 2000))
                tempRET = 1000;
            else
                tempRET = 0;
            if (pressNow < 0)
                tempRET = -tempRET;
            return tempRET;
        }


        /// <summary>
        /// 完成情况List
        /// </summary>
        private List<String> _completeStatusList=new List<string> { "√", "×" };
        /// <summary>
        /// 完成情况List
        /// </summary>
        public List<String> CompleteStatusList
        {
            get { return _completeStatusList; }
        }
    }
}
