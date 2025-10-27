/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQDFJ_MB.Model.Exp
 * 文件名：  MQZH_ExpModel_QM
 * 版本号：  V1.0.0.0
 * 唯一标识：2f3f96fb-6ae3-4bfc-b597-550152445590
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/11/20 18:17:05
 * 描述：
 * 幕墙四性气密检测Model
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2021/11/20       18:17:05		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static MQDFJ_MB.Model.MQZH_Enums;
using System.Windows;

namespace MQDFJ_MB.Model.Exp
{
    /// <summary>
    /// 幕墙四性气密试验Model类
    /// </summary>
    public class MQZH_ExpModel_QM : ObservableObject
    {
        public MQZH_ExpModel_QM()
        {
            QMExpInit();
        }

        #region 气密检测设定参数属性

        /// <summary>
        /// 项目选中状态（true为选中）
        /// </summary>
        private bool _selectedStatus = true;

        /// <summary>
        /// 项目选中状态（true为选中）
        /// </summary>
        public bool NeedTest
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                RaisePropertyChanged(() => NeedTest);
            }
        }

        /// <summary>
        /// 检测类型（true为工程）
        /// </summary>
        private bool _isGC = false;

        /// <summary>
        /// 检测类型（true为工程）
        /// </summary>
        public bool IsGC
        {
            get { return _isGC; }
            set
            {
                _isGC = value;
                StageStepNeedInit();
                CanBeCheckInit();
                RaisePropertyChanged(() => IsGC);
            }
        }

        /// <summary>
        /// 气密项目完成状态
        /// </summary>
        private bool _completeStatus = false;

        /// <summary>
        /// 气密项目完成状态
        /// </summary>
        public bool CompleteStatus
        {
            get { return _completeStatus; }
            set
            {
                _completeStatus = value;
                RaisePropertyChanged(() => CompleteStatus);
            }
        }

        /// <summary>
        /// 工程检测压力设计值
        /// </summary>
        private double _qm_GCSJP = 100;

        /// <summary>
        /// 工程检测压力设计值
        /// </summary>
        public double QM_GCSJP
        {
            get { return _qm_GCSJP; }
            set
            {
                _qm_GCSJP = value;
                RaisePropertyChanged(() => QM_GCSJP);
            }
        }

        /// <summary>
        /// 工程检测预加压压力值
        /// </summary>
        private double _qm_GCYJYP = 500;

        /// <summary>
        /// 工程检测预加压压力值
        /// </summary>
        public double QM_GCYJYP
        {
            get { return _qm_GCYJYP; }
            set
            {
                _qm_GCYJYP = value;
                RaisePropertyChanged(() => QM_GCYJYP);
            }
        }

        /// <summary>
        /// 单位开启缝长渗透量工程设计值
        /// </summary>
        private double _qm_SJSTL_DWKQFC = 4;

        /// <summary>
        /// 单位开启缝长渗透量工程设计值
        /// </summary>
        public double QM_SJSTL_DWKQFC
        {
            get { return _qm_SJSTL_DWKQFC; }
            set
            {
                _qm_SJSTL_DWKQFC = value;
                RaisePropertyChanged(() => QM_SJSTL_DWKQFC);
            }
        }

        /// <summary>
        /// 单位面积渗透量工程设计值
        /// </summary>
        private double _qm_SJSTL_DWMJ = 4;

        /// <summary>
        /// 单位面积渗透量工程设计值
        /// </summary>
        public double QM_SJSTL_DWMJ
        {
            get { return _qm_SJSTL_DWMJ; }
            set
            {
                _qm_SJSTL_DWMJ = value;
                RaisePropertyChanged(() => QM_SJSTL_DWMJ);
            }
        }

        #endregion


        #region 运行及辅助属性

        /// <summary>
        /// 定级试验阶段被选数组
        /// </summary>
        private ObservableCollection<bool> _beenCheckedDJ;
        /// <summary>
        /// 定级试验阶段被选数组
        /// </summary>
        public ObservableCollection<bool> BeenCheckedDJ
        {
            get { return _beenCheckedDJ; }
            set
            {
                _beenCheckedDJ = value;
                RaisePropertyChanged(() => BeenCheckedDJ);
            }
        }

        /// <summary>
        /// 工程试验阶段待做数组
        /// </summary>
        private ObservableCollection<bool> _beenCheckedGC;

        /// <summary>
        /// 工程试验阶段待做数组
        /// </summary>
        public ObservableCollection<bool> BeenCheckedGC
        {
            get { return _beenCheckedGC; }
            set
            {
                _beenCheckedGC = value;
                RaisePropertyChanged(() => BeenCheckedGC);
            }
        }

        /// <summary>
        /// 气密当前阶段
        /// </summary>
        private MQZH_StageModel_QSM _stage_DQ = new MQZH_StageModel_QSM();

        /// <summary>
        /// 气密当前阶段
        /// </summary>
        public MQZH_StageModel_QSM Stage_DQ
        {
            get { return _stage_DQ; }
            set
            {
                _stage_DQ = value;
                RaisePropertyChanged(() => Stage_DQ);
            }
        }

        /// <summary>
        /// 气密当前步骤
        /// </summary>
        private MQZH_StepModel_QSM _step_DQ = new MQZH_StepModel_QSM();

        /// <summary>
        /// 气密当前步骤
        /// </summary>
        public MQZH_StepModel_QSM Step_DQ
        {
            get { return _step_DQ; }
            set
            {
                _step_DQ = value;
                RaisePropertyChanged(() => Step_DQ);
            }
        }

        /// <summary>
        /// 定级试验页面、阶段可选属性
        /// </summary>
        private bool _isDJcanbeChecked = true;

        /// <summary>
        /// 定级试验页面、阶段可选属性
        /// </summary>
        public bool IsDJcanbeChecked
        {
            get { return _isDJcanbeChecked; }
            set
            {
                _isDJcanbeChecked = value;
                RaisePropertyChanged(() => IsDJcanbeChecked);
            }
        }

        /// <summary>
        /// 工程试验页面、阶段可选属性
        /// </summary>
        private bool _isGCcanbeChecked = true;

        /// <summary>
        /// 工程试验页面、阶段可选属性
        /// </summary>
        public bool IsGCcanbeChecked
        {
            get { return _isGCcanbeChecked; }
            set
            {
                _isGCcanbeChecked = value;
                RaisePropertyChanged(() => IsGCcanbeChecked);
            }
        }

        #endregion


        #region 气密检测试验阶段组

        /// <summary>
        /// 气密定级检测试验阶段组
        /// </summary>
        private List<MQZH_StageModel_QSM> _stageList_QMDJ;

        /// <summary>
        /// 气密定级检测试验阶段组
        /// </summary>
        public List<MQZH_StageModel_QSM> StageList_QMDJ
        {
            get { return _stageList_QMDJ; }
            set
            {
                _stageList_QMDJ = value;
                RaisePropertyChanged(() => StageList_QMDJ);
            }
        }

        /// <summary>
        /// 气密工程检测试验阶段组
        /// </summary>
        private List<MQZH_StageModel_QSM> _stageList_QMGC;

        /// <summary>
        /// 气密工程检测试验阶段组
        /// </summary>
        public List<MQZH_StageModel_QSM> StageList_QMGC
        {
            get { return _stageList_QMGC; }
            set
            {
                _stageList_QMGC = value;
                RaisePropertyChanged(() => StageList_QMGC);
            }
        }

        #endregion


        #region 气密项目试验总初始化

        /// <summary>
        /// 气密试验参数初始化。
        ///  </summary>
        /// <remarks>初始化之前，需设定相关参数</remarks>
        public void QMExpInit()
        {
            //当前阶段、步骤初始化
            Stage_DQ = null;
            Stage_DQ = new MQZH_StageModel_QSM();
            Step_DQ = null;
            Step_DQ = new MQZH_StepModel_QSM();

            //定级阶段list初始化（12个阶段）
            StageList_QMDJ = null;
            StageList_QMDJ = new List<MQZH_StageModel_QSM>();
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            StageList_QMDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMDJ.Add(tempStage);
            QMDJqf_ZYStageInit();
            QMDJqf_ZJStageInit();
            QMDJqf_FYStageInit();
            QMDJqf_FJStageInit();
            QMDJqfg_ZYStageInit();
            QMDJqfg_ZJStageInit();
            QMDJqfg_FYStageInit();
            QMDJqfg_FJStageInit();
            QMDJqz_ZYStageInit();
            QMDJqz_ZJStageInit();
            QMDJqz_FYStageInit();
            QMDJqz_FJStageInit();

            //工程阶段list初始化（12个阶段）
            StageList_QMGC = null;
            StageList_QMGC = new List<MQZH_StageModel_QSM>();
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_QMGC.Add(tempStage);
            QMGCqf_ZYStageInit();
            QMGCqf_ZJStageInit();
            QMGCqf_FYStageInit();
            QMGCqf_FJStageInit();
            QMGCqfg_ZYStageInit();
            QMGCqfg_ZJStageInit();
            QMGCqfg_FYStageInit();
            QMGCqfg_FJStageInit();
            QMGCqz_ZYStageInit();
            QMGCqz_ZJStageInit();
            QMGCqz_FYStageInit();
            QMGCqz_FJStageInit();

            ////可选、需要做实验属性初始化
            StageStepNeedInit();
            CanBeCheckInit();
        }

        /// <summary>
        /// 根据编号初始化定级阶段
        /// </summary>
        /// <param name="No"></param>
        public void DJStageInitByNo(int No)
        {
            switch (No)
            {
                case 0:
                    QMDJqf_ZYStageInit();
                    break;
                case 1:
                    QMDJqf_ZJStageInit();
                    break;
                case 2:
                    QMDJqf_FYStageInit();
                    break;
                case 3:
                    QMDJqf_FJStageInit();
                    break;
                case 4:
                    QMDJqfg_ZYStageInit();
                    break;
                case 5:
                    QMDJqfg_ZJStageInit();
                    break;
                case 6:
                    QMDJqfg_FYStageInit();
                    break;
                case 7:
                    QMDJqfg_FJStageInit();
                    break;
                case 8:
                    QMDJqz_ZYStageInit();
                    break;
                case 9:
                    QMDJqz_ZJStageInit();
                    break;
                case 10:
                    QMDJqz_FYStageInit();
                    break;
                case 11:
                    QMDJqz_FJStageInit();
                    break;
            }
        }


        /// <summary>
        /// 根据编号初始化工程阶段
        /// </summary>
        /// <param name="No"></param>
        public void GCStageInitByNo(int No)
        {
            switch (No)
            {
                case 0:
                    QMGCqf_ZYStageInit();
                    break;
                case 1:
                    QMGCqf_ZJStageInit();
                    break;
                case 2:
                    QMGCqf_FYStageInit();
                    break;
                case 3:
                    QMGCqf_FJStageInit();
                    break;
                case 4:
                    QMGCqfg_ZYStageInit();
                    break;
                case 5:
                    QMGCqfg_ZJStageInit();
                    break;
                case 6:
                    QMGCqfg_FYStageInit();
                    break;
                case 7:
                    QMGCqfg_FJStageInit();
                    break;
                case 8:
                    QMGCqz_ZYStageInit();
                    break;
                case 9:
                    QMGCqz_ZJStageInit();
                    break;
                case 10:
                    QMGCqz_FYStageInit();
                    break;
                case 11:
                    QMGCqz_FJStageInit();
                    break;
            }
        }

        #endregion


        #region 气密定级各阶段初始化

        /// <summary>
        /// 气密定级正压附加渗透量 预加压阶段初始化
        /// </summary>
        public void QMDJqf_ZYStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //定级正压附加渗透量预加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_DJ_ZqfY.ToString(),
                Stage_Name = "气密定级正压附加渗透量预加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "预加压测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认后再点击确认。",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密定级正压附加渗透量预加压第1次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 2,
                StepName = "气密定级正压附加渗透量预加压第2次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 3,
                StepName = "气密定级正压附加渗透量预加压第3次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            StageList_QMDJ[0] = tempStage;
            RaisePropertyChanged(() => StageList_QMDJ);
        }

        /// <summary>
        /// 气密定级正压附加渗透量 检测加压阶段初始化
        /// </summary>
        public void QMDJqf_ZJStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();

            //气密定级正压附加渗透量检测阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_DJ_ZqfJ.ToString(),
                Stage_Name = "气密定级正压附加渗透量检测加压",
                CompleteStatus = false,
                IsNeedTipsBefore = false,
                StringTipsBefore = "",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密定级正压附加渗透量检测升压50Pa",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 2,
                StepName = "气密定级正压附加渗透量检测升压100Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 3,
                StepName = "气密定级正压附加渗透量检测150Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 4,
                StepName = "气密定级正压附加渗透量降压100Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 5,
                StepName = "气密定级正压附加渗透量降压50Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            StageList_QMDJ[1] = tempStage;
            RaisePropertyChanged(() => StageList_QMDJ);
        }

        /// <summary>
        /// 气密定级负压附加渗透量 预加压阶段初始化
        /// </summary>
        public void QMDJqf_FYStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //气密定级负压附加渗透量预加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_DJ_FqfY.ToString(),
                Stage_Name = "气密定级负压附加渗透量预加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "预加压测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认。",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密定级负压附加渗透量预加压第1次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 2,
                StepName = "气密定级负压附加渗透量预加压第2次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 3,
                StepName = "气密定级负压附加渗透量预加压第3次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            StageList_QMDJ[2] = tempStage;
            RaisePropertyChanged(() => StageList_QMDJ);
        }

        /// <summary>
        /// 气密定级负压附加渗透量 检测加压阶段初始化
        /// </summary>
        public void QMDJqf_FJStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();

            //气密定级负压附加渗透量检测加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_DJ_FqfJ.ToString(),
                Stage_Name = "气密定级负压附加渗透量检测加压",
                CompleteStatus = false,
                IsNeedTipsBefore = false,
                StringTipsBefore = "",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密定级负压附加渗透量检测升压50Pa",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 2,
                StepName = "气密定级负压附加渗透量检测升压100Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 3,
                StepName = "气密定级负压附加渗透量检测150Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 4,
                StepName = "气密定级负压附加渗透量降压100Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 5,
                StepName = "气密定级负压附加渗透量检测降压50Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            StageList_QMDJ[3] = tempStage;
            RaisePropertyChanged(() => StageList_QMDJ);
        }

        /// <summary>
        /// 气密定级正压附加固定渗透量 预加压阶段初始化
        /// </summary>
        public void QMDJqfg_ZYStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();

            //定级正压附加固定渗透量预加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_DJ_ZqfgY.ToString(),
                Stage_Name = "气密定级正压附加固定渗透量预加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "预加压测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认。",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密定级正压附加固定渗透量预加压第一次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 2,
                StepName = "气密定级正压附加固定渗透量预加压第二次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 3,
                StepName = "气密定级正压附加固定渗透量预加压第三次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            StageList_QMDJ[4] = tempStage;
            RaisePropertyChanged(() => StageList_QMDJ);
        }

        /// <summary>
        /// 气密定级正压附加固定渗透量 检测加压阶段初始化
        /// </summary>
        public void QMDJqfg_ZJStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();

            //气密定级正压附加固定渗透量检测阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_DJ_ZqfgJ.ToString(),
                Stage_Name = "气密定级正压附加固定渗透量检测加压",
                CompleteStatus = false,
                IsNeedTipsBefore = false,
                StringTipsBefore = "",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密定级正压附加固定渗透量检测升压50Pa",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 2,
                StepName = "气密定级正压附加固定渗透量检测升压100Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 3,
                StepName = "气密定级正压附加固定渗透量检测150Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 4,
                StepName = "气密定级正压附加固定渗透量降压100Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 5,
                StepName = "气密定级正压附加固定渗透量降压50Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            StageList_QMDJ[5] = tempStage;
            RaisePropertyChanged(() => StageList_QMDJ);
        }

        /// <summary>
        /// 气密定级负压附加固定渗透量 预加压阶段初始化
        /// </summary>
        public void QMDJqfg_FYStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //气密定级负压附加固定渗透量预加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_DJ_FqfgY.ToString(),
                Stage_Name = "气密定级负压附加固定渗透量预加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "预加压测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认。",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密定级负压附加固定渗透量预加压第一次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 2,
                StepName = "气密定级负压附加固定渗透量预加压第二次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 3,
                StepName = "气密定级负压附加固定渗透量预加压第三次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            StageList_QMDJ[6] = tempStage;
            RaisePropertyChanged(() => StageList_QMDJ);
        }

        /// <summary>
        /// 气密定级负压附加固定渗透量 检测加压阶段初始化
        /// </summary>
        public void QMDJqfg_FJStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //气密定级负压附加固定渗透量检测加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_DJ_FqfgJ.ToString(),
                Stage_Name = "气密定级负压附加固定渗透量检测加压",
                CompleteStatus = false,
                IsNeedTipsBefore = false,
                StringTipsBefore = "",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密定级负压附加固定渗透量检测升压50Pa",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 2,
                StepName = "气密定级负压附加固定渗透量检测升压100Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 3,
                StepName = "气密定级负压附加固定渗透量检测150Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 4,
                StepName = "气密定级负压附加固定渗透量降压100Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 5,
                StepName = "气密定级负压附加固定渗透量检测降压50Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            StageList_QMDJ[7] = tempStage;
            RaisePropertyChanged(() => StageList_QMDJ);
        }

        /// <summary>
        /// 气密定级正压总渗透量 预加压阶段初始化
        /// </summary>
        public void QMDJqz_ZYStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //定级正压总渗透量预加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_DJ_ZqzY.ToString(),
                Stage_Name = "气密定级正压总渗透量预加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "预加压测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认。",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密定级正压总渗透量预加压第一次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 2,
                StepName = "气密定级正压总渗透量预加压第二次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 3,
                StepName = "气密定级正压总渗透量预加压第三次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            StageList_QMDJ[8] = tempStage;
            RaisePropertyChanged(() => StageList_QMDJ);
        }

        /// <summary>
        /// 气密定级正压总渗透量 检测加压阶段初始化
        /// </summary>
        public void QMDJqz_ZJStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //气密定级正压总渗透量检测阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_DJ_ZqzJ.ToString(),
                Stage_Name = "气密定级正压总渗透量检测加压",
                CompleteStatus = false,
                IsNeedTipsBefore = false,
                StringTipsBefore = "",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密定级正压总渗透量检测升压50Pa",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 2,
                StepName = "气密定级正压总渗透量检测升压100Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep)
                ;
            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 3,
                StepName = "气密定级正压总渗透量检测150Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 4,
                StepName = "气密定级正压总渗透量降压100Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 5,
                StepName = "气密定级正压总渗透量降压50Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            StageList_QMDJ[9] = tempStage;
            RaisePropertyChanged(() => StageList_QMDJ);
        }

        /// <summary>
        /// 气密定级负压总渗透量 预加压阶段初始化
        /// </summary>
        public void QMDJqz_FYStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //气密定级负压总渗透量预加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_DJ_FqzY.ToString(),
                Stage_Name = "气密定级负压总渗透量预加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "预加压测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认。",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密定级负压总渗透量预加压第一次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 2,
                StepName = "气密定级负压总渗透量预加压第二次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 3,
                StepName = "气密定级负压总渗透量预加压第三次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            StageList_QMDJ[10] = tempStage;
            RaisePropertyChanged(() => StageList_QMDJ);
        }

        /// <summary>
        /// 气密定级负压总渗透量 检测加压阶段初始化
        /// </summary>
        public void QMDJqz_FJStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //气密定级负压总渗透量检测加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_DJ_FqzJ.ToString(),
                Stage_Name = "气密定级负压总渗透量检测加压",
                CompleteStatus = false,
                IsNeedTipsBefore = false,
                StringTipsBefore = "",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密定级负压总渗透量检测升压50Pa",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 2,
                StepName = "气密定级负压总渗透量检测升压100Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 3,
                StepName = "气密定级负压总渗透量检测150Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 4,
                StepName = "气密定级负压总渗透量降压100Pa",
                IsStepCompleted = false,
                WaveAimLBound = 0
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 5,
                StepName = "气密定级负压总渗透量检测降压50Pa",
                IsStepCompleted = false
            };
            tempStage.StepList.Add(newStep);

            StageList_QMDJ[11] = tempStage;
            RaisePropertyChanged(() => StageList_QMDJ);
        }


        #endregion


        #region 气密工程阶段各初始化

        /// <summary>
        /// 气密工程附加渗透量 正压预加压阶段初始化
        /// </summary>
        public void QMGCqf_ZYStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();

            //气密工程正压附加渗透量预加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_GC_ZqfY.ToString(),
                Stage_Name = "气密工程正压附加渗透量预加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "预加压测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认。",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密工程正压附加渗透量预加压第一次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 2,
                StepName = "气密工程正压附加渗透量预加压第二次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 3,
                StepName = "气密工程正压附加渗透量预加压第三次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            StageList_QMGC[0] = tempStage;
            RaisePropertyChanged(() => StageList_QMGC);
        }

        /// <summary>
        /// 气密工程附加渗透量 正压检测加压阶段初始化
        /// </summary>
        public void QMGCqf_ZJStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();

            //气密工程正压附加渗透量检测加压阶段
            tempStage = null;
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_GC_ZqfJ.ToString(),
                Stage_Name = "气密工程正压附加渗透量检测加压",
                CompleteStatus = false,
                IsNeedTipsBefore = false,
                StringTipsBefore = "",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密工程正压附加渗透量检测",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            StageList_QMGC[1] = tempStage;
            RaisePropertyChanged(() => StageList_QMGC);
        }

        /// <summary>
        /// 气密工程附加渗透量 负压预加压阶段初始化
        /// </summary>
        public void QMGCqf_FYStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();

            //气密工程负压附加渗透量预加压阶段
            tempStage = null;
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_GC_FqfY.ToString(),
                Stage_Name = "气密工程负压附加渗透量预加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "预加压测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认。",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密工程负压附加渗透量预加压第一次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 2,
                StepName = "气密工程负压附加渗透量预加压第二次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 3,
                StepName = "气密工程负压附加渗透量预加压第三次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            StageList_QMGC[2] = tempStage;
            RaisePropertyChanged(() => StageList_QMGC);
        }

        /// <summary>
        /// 气密工程附加渗透量 负压检测加压阶段初始化
        /// </summary>
        public void QMGCqf_FJStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();

            //气密工程负压附加渗透量检测加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_GC_FqfJ.ToString(),
                Stage_Name = "气密工程负压附加渗透量检测加压",
                CompleteStatus = false,
                IsNeedTipsBefore = false,
                StringTipsBefore = "",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密工程负压附加渗透量检测",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            StageList_QMGC[3] = tempStage;
            RaisePropertyChanged(() => StageList_QMGC);
        }

        /// <summary>
        /// 气密工程附加固定渗透量 正压预加压阶段初始化
        /// </summary>
        public void QMGCqfg_ZYStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();

            //气密工程正压附加固定渗透量预加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_GC_ZqfgY.ToString(),
                Stage_Name = "气密工程正压附加固定渗透量预加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "预加压测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认。",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密工程正压附加固定渗透量预加压第一次",
                IsStepCompleted = false,
                WaveAimLBound = QM_GCSJP,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 2,
                StepName = "气密工程正压附加固定渗透量预加压第二次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 3,
                StepName = "气密工程正压附加固定渗透量预加压第三次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            StageList_QMGC[4] = tempStage;
            RaisePropertyChanged(() => StageList_QMGC);
        }

        /// <summary>
        /// 气密工程附加固定渗透量 正压检测加压阶段初始化
        /// </summary>
        public void QMGCqfg_ZJStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();

            //气密工程正压附加固定渗透量检测加压阶段
            tempStage = null;
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_GC_ZqfgJ.ToString(),
                Stage_Name = "气密工程正压附加固定渗透量检测加压",
                CompleteStatus = false,
                IsNeedTipsBefore = false,
                StringTipsBefore = "",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密工程正压附加固定渗透量检测",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            StageList_QMGC[5] = tempStage;
            RaisePropertyChanged(() => StageList_QMGC);
        }

        /// <summary>
        /// 气密工程附加固定渗透量 负压预加压阶段初始化
        /// </summary>
        public void QMGCqfg_FYStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();

            //气密工程负压附加固定渗透量预加压阶段
            tempStage = null;
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_GC_FqfgY.ToString(),
                Stage_Name = "气密工程负压附加固定渗透量预加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "预加压测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认。",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密工程负压附加固定渗透量预加压第一次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 2,
                StepName = "气密工程负压附加固定渗透量预加压第二次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 3,
                StepName = "气密工程负压附加固定渗透量预加压第三次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            StageList_QMGC[6] = tempStage;
            RaisePropertyChanged(() => StageList_QMGC);
        }

        /// <summary>
        /// 气密工程附加固定渗透量 负压检测加压阶段初始化
        /// </summary>
        public void QMGCqfg_FJStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();

            //气密工程负压附加固定渗透量检测加压阶段
            tempStage = null;
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_GC_FqfgJ.ToString(),
                Stage_Name = "气密工程负压附加固定渗透量检测加压",
                CompleteStatus = false,
                IsNeedTipsBefore = false,
                StringTipsBefore = "",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密工程负压附加固定渗透量检测",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            StageList_QMGC[7] = tempStage;
            RaisePropertyChanged(() => StageList_QMGC);
        }

        /// <summary>
        /// 气密工程总渗透量 正压预加压阶段初始化
        /// </summary>
        public void QMGCqz_ZYStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //气密工程正压总渗透量预加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_GC_ZqzY.ToString(),
                Stage_Name = "气密工程正压总渗透量预加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "预加压测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认。",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密工程正压总渗透量预加压第一次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 2,
                StepName = "气密工程正压总渗透量预加压第二次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 3,
                StepName = "气密工程正压总渗透量预加压第三次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            StageList_QMGC[8] = tempStage;
            RaisePropertyChanged(() => StageList_QMGC);
        }

        /// <summary>
        /// 气密工程总渗透量 正压检测加压阶段初始化
        /// </summary>
        public void QMGCqz_ZJStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();

            //气密工程正压总渗透量检测加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_GC_ZqzJ.ToString(),
                Stage_Name = "气密工程正压总渗透量检测加压",
                CompleteStatus = false,
                IsNeedTipsBefore = false,
                StringTipsBefore = "",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密工程正压总渗透量检测",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            StageList_QMGC[9] = tempStage;
            RaisePropertyChanged(() => StageList_QMGC);
        }

        /// <summary>
        /// 气密工程总渗透量 负压预加压阶段初始化
        /// </summary>
        public void QMGCqz_FYStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();

            //气密工程负压总渗透量预加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_GC_FqzY.ToString(),
                Stage_Name = "气密工程负压总渗透量预加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "预加压测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认。",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密工程负压总渗透量预加压第一次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 2,
                StepName = "气密工程负压总渗透量预加压第二次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 60
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 3,
                StepName = "气密工程负压总渗透量预加压第三次",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor =60
            };
            tempStage.StepList.Add(newStep);

            StageList_QMGC[10] = tempStage;
            RaisePropertyChanged(() => StageList_QMGC);
        }

        /// <summary>
        /// 气密工程总渗透量 负压检测加压阶段初始化
        /// </summary>
        public void QMGCqz_FJStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();

            //气密工程负压总渗透量检测加压阶段
            tempStage = null;
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.QM_GC_FqzJ.ToString(),
                Stage_Name = "气密工程负压总渗透量检测加压",
                CompleteStatus = false,
                IsNeedTipsBefore = false,
                StringTipsBefore = "",
                IsNeedTipsAfter = false,
                StringTipsAfter = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "气密工程负压总渗透量检测",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            StageList_QMGC[11] = tempStage;
            RaisePropertyChanged(() => StageList_QMGC);
        }

        #endregion


        #region 阶段、页面可选、被选属性初始化

        /// <summary>
        /// 可选属性初始化
        /// </summary>
        public void CanBeCheckInit()
        {
            //备选数组清空
            if (BeenCheckedDJ == null)
            {
                BeenCheckedDJ = new ObservableCollection<bool>()
                {
                    false ,false ,false ,false ,false ,false ,
                    false ,false ,false ,false ,false ,false 
                };
            }
            else
            {
                for (int i = 0; i < BeenCheckedDJ.Count; i++)
                    BeenCheckedDJ[i] = false;
            }
            if (BeenCheckedGC == null)
            {
                BeenCheckedGC = new ObservableCollection<bool>()
                {
                    false ,false ,false ,false ,false ,false ,
                    false ,false ,false ,false ,false ,false
                };
            }
            else
            {
                for (int i = 0; i < BeenCheckedGC.Count; i++)
                    BeenCheckedGC[i] = false;
            }

            //定级、工程页面禁选属性
            IsDJcanbeChecked = NeedTest && (!IsGC);
            IsGCcanbeChecked = NeedTest && IsGC;
        }

        #endregion


        #region 阶段、步骤需要做实验状态初始化

        /// <summary>
        /// 定级阶段、步骤需要中试验状态属性初始化
        /// </summary>
        public void StageStepNeedInit()
        {
            //定级
            for (int i = 0; i < StageList_QMDJ.Count; i++)
            {
                //阶段需要做实验属性
                if (IsGC)
                    StageList_QMDJ[i].NeedTest = false;
                else
                    StageList_QMDJ[i].NeedTest = true;
                //步骤需要做实验属性
                for (int j = 0; j < StageList_QMDJ[i].StepList.Count; j++)
                {
                    if (IsGC)
                        StageList_QMDJ[i].StepList[j].StepNeedTest = false;
                    else
                        StageList_QMDJ[i].StepList[j].StepNeedTest = true;
                }
            }
            //工程
            for (int i = 0; i < StageList_QMGC.Count; i++)
            {
                //阶段需要做实验属性
                if (IsGC)
                    StageList_QMGC[i].NeedTest = true;
                else
                    StageList_QMGC[i].NeedTest = false;
                //步骤需要做实验属性
                for (int j = 0; j < StageList_QMGC[i].StepList.Count; j++)
                {
                    if (IsGC)
                        StageList_QMGC[i].StepList[j].StepNeedTest = true;
                    else
                        StageList_QMGC[i].StepList[j].StepNeedTest = false;
                }
            }
        }

        #endregion


        #region 当前阶段、步骤初始化

        /// <summary>
        /// 当前阶段、步骤初始化
        /// </summary>
        public void StageStepDQReset()
        {
            try
            {
                Stage_DQ = new MQZH_StageModel_QSM();
                Step_DQ = new MQZH_StepModel_QSM();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        #endregion
    }
}