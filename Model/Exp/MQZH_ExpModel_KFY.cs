/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQZHWL.Model.Exp
 * 文件名：  MQZH_ExpModel_KFY
 * 版本号：  V1.0.0.0
 * 唯一标识：2f3f96fb-6ae3-4bfc-b597-550152445590
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/11/20 18:17:05
 * 描述：
 * 幕墙四性抗风压检测Model
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2021/11/20 18:17:05		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using static MQZHWL.Model.MQZH_Enums;

namespace MQZHWL.Model.Exp
{
    /// <summary>
    /// 幕墙四性抗风压试验类
    /// </summary>
    public class MQZH_ExpModel_KFY:ObservableObject 
    {
        public MQZH_ExpModel_KFY()
        {
            KFYExpInit();
        }

        #region 抗风压检测设定参数属性

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
        /// 项目完成状态
        /// </summary>
        private bool _completeStatus = false;
        /// <summary>
        /// 项目完成状态
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
        /// 工程检测风荷载标准值
        /// </summary>
        private double _p3_GCSJ = 1000;
        /// <summary>
        /// 工程检测风荷载标准值
        /// </summary>
        public double P3_GCSJ
        {
            get { return _p3_GCSJ; }
            set
            {
                _p3_GCSJ = value;
                RaisePropertyChanged(() => P3_GCSJ);
            }
        }

        /// <summary>
        /// 测点组
        /// </summary>
        private ObservableCollection<DisplaceGroup> _displaceGroups = new ObservableCollection<DisplaceGroup>()
        {
            new DisplaceGroup(){ID ="1",WYC_No={ 1, 2, 3, 1 }},
            new DisplaceGroup(){ID ="2",WYC_No={ 4, 5, 6, 1 }},
            new DisplaceGroup(){ID ="3",WYC_No={ 7, 8, 9, 1 }}
        };
        /// <summary>
        /// 测点组
        /// </summary>
        public ObservableCollection<DisplaceGroup> DisplaceGroups
        {
            get { return _displaceGroups; }
            set
            {
                _displaceGroups = value;
                RaisePropertyChanged(() => DisplaceGroups);
            }
        }

        /// <summary>
        /// P3需要降低压力
        /// </summary>
        private bool _p3DownPress = true;

        /// <summary>
        /// P3需要降低压力
        /// </summary>
        public bool P3DownPress
        {
            get { return _p3DownPress; }
            set
            {
                _p3DownPress = value;
                RaisePropertyChanged(() => P3DownPress);
            }
        }

        /// <summary>
        /// P3检测压力
        /// </summary>
        private double _p3Press = 1000;

        /// <summary>
        /// P3检测压力
        /// </summary>
        public double P3Press
        {
            get { return _p3Press; }
            set
            {
                _p3Press = value;
                RaisePropertyChanged(() => P3Press);
            }
        }
        #endregion


        #region 测试阶段

        /// <summary>
        /// 抗风压定级检测试验阶段组
        /// </summary>
        /// <remarks>变形正预、变形正检、变形负预、变形负检、反复正、反复负、标准正、标准负、设计正、设计负</remarks>
        private List<MQZH_StageModel_QSM> _stageList_KFYDJ;
        /// <summary>
        /// 抗风压定级检测试验阶段组
        /// </summary>
        public List<MQZH_StageModel_QSM> StageList_KFYDJ
        {
            get { return _stageList_KFYDJ; }
            set
            {
                _stageList_KFYDJ = value;
                RaisePropertyChanged(() => StageList_KFYDJ);
            }
        }

        /// <summary>
        /// 抗风压工程检测试验阶段组
        /// </summary>
        private List<MQZH_StageModel_QSM> _stageList_KFYGC;
        /// <summary>
        /// 抗风压工程检测试验阶段组
        /// </summary>
        public List<MQZH_StageModel_QSM> StageList_KFYGC
        {
            get { return _stageList_KFYGC; }
            set
            {
                _stageList_KFYGC = value;
                RaisePropertyChanged(() => StageList_KFYGC);
            }
        }

        #endregion


        #region 运行及辅助属性


        /// <summary>
        /// 抗风压当前阶段
        /// </summary>
        private MQZH_StageModel_QSM _stage_DQ = new MQZH_StageModel_QSM();
        /// <summary>
        /// 抗风压当前阶段
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
        /// 抗风压当前步骤
        /// </summary>
        private MQZH_StepModel_QSM _step_DQ = new MQZH_StepModel_QSM();
        /// <summary>
        /// 抗风压当前步骤
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
        /// 定级试验阶段被选组合
        /// </summary>
        private ObservableCollection<bool> _beenCheckedDJ = new ObservableCollection<bool>() { false, false, false, false, false, false, false, false, false, false };
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
        /// 工程试验阶段被选数组
        /// </summary>
        private ObservableCollection<bool> _beenCheckedGC = new ObservableCollection<bool>() { false, false, false, false, false, false, false, false, false, false };

        /// <summary>
        /// 工程试验阶段被选数组
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


        #region 抗风压项目试验总初始化

        /// <summary>
        /// 抗风压试验参数初始化。
        ///  </summary>
        /// <remarks>初始化之前，需设定相关参数</remarks>
        public void KFYExpInit()
        {

            //当前阶段、步骤初始化
            Stage_DQ = null;
            Stage_DQ = new MQZH_StageModel_QSM();
            Step_DQ = null;
            Step_DQ = new MQZH_StepModel_QSM();

            //定级阶段list初始化（10阶段）
            StageList_KFYDJ = null;
            StageList_KFYDJ = new List<MQZH_StageModel_QSM>();
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            StageList_KFYDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_KFYDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_KFYDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_KFYDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_KFYDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_KFYDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_KFYDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_KFYDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_KFYDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_KFYDJ.Add(tempStage);
            KFY_DJBX_ZYStageInit();
            KFY_DJBX_ZJStageInit();
            KFY_DJBX_FYStageInit();
            KFY_DJBX_FJStageInit();
            KFY_DJ_ZFFStageInit();
            KFY_DJ_FFFStageInit();
            KFY_DJ_ZP3StageInit();
            KFY_DJ_FP3StageInit();
            KFY_DJ_ZPmaxStageInit();
            KFY_DJ_FPmaxStageInit();
            
            //工程阶段list初始化（10个阶段）
            StageList_KFYGC = null;
            StageList_KFYGC = new List<MQZH_StageModel_QSM>();
            tempStage = new MQZH_StageModel_QSM();
            StageList_KFYGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_KFYGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_KFYGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_KFYGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_KFYGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_KFYGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_KFYGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_KFYGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_KFYGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_KFYGC.Add(tempStage);

            KFY_GCBX_ZYStageInit();
            KFY_GCBX_ZJStageInit();
            KFY_GCBX_FYStageInit();
            KFY_GCBX_FJStageInit();
            KFY_GC_ZFFStageInit();
            KFY_GC_FFFStageInit();
            KFY_GC_ZP3StageInit();
            KFY_GC_FP3StageInit();
            KFY_GC_ZPmaxStageInit();
            KFY_GC_FPmaxStageInit();

            //可选、需要做实验属性初始化
            StageStepNeedInit();
            CanBeCheckInit();
        }

        #endregion


        #region 抗风压定级阶段初始化

        /// <summary>
        /// 抗风压定级变形 正压预加压阶段初始化
        /// </summary>
        public void KFY_DJBX_ZYStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //定级变形正压预加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.KFY_DJBX_ZY.ToString(),
                Stage_Name = "抗风压定级变形检测正压预加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "预加压测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认后。",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();
            
            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_QSM
                {
                    StepNO = j,
                    StepName = "抗风压定级变形检测正压预加压 第" + j + "次",
                    IsStepCompleted = false,
                    IsWaitBeforCompleted = false,
                    TimeWaitBefor = 30
                };
                tempStage.StepList.Add(newStep);
            }

            StageList_KFYDJ[0] = tempStage;
            RaisePropertyChanged(() => StageList_KFYDJ);
        }

        /// <summary>
        /// 抗风压定级变形 正压检测加压阶段初始化
        /// </summary>
        public void KFY_DJBX_ZJStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //定级正压检测加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.KFY_DJBX_ZJ.ToString(),
                Stage_Name = "抗风压定级变形检测正压加压",
                CompleteStatus = false,
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            for (int i = 0; i < 20; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_QSM();
                newStep.StepNO = j;
                newStep.StepName = "抗风压定级变形检测正压加压第" + j + "级";
                newStep.IsStepCompleted = false;
                newStep.IsWaitBeforCompleted = false;
                newStep.TimeWaitBefor = 0;
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_KFYDJ[1] = tempStage;
            RaisePropertyChanged(() => StageList_KFYDJ);
        }

        /// <summary>
        /// 抗风压定级变形 负压预加压阶段初始化
        /// </summary>
        public void KFY_DJBX_FYStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //定级变形负压预加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.KFY_DJBX_FY.ToString(),
                Stage_Name = "抗风压定级变形检测负压预加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "预加压测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认。",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_QSM
                {
                    StepNO = j,
                    StepName = "抗风压定级变形检测负压预加压 第" + j + "次",
                    IsStepCompleted = false,
                    IsWaitBeforCompleted = false,
                    TimeWaitBefor = 30
                };
                tempStage.StepList.Add(newStep);
            }

            StageList_KFYDJ[2] = tempStage;
            RaisePropertyChanged(() => StageList_KFYDJ);
        }

        /// <summary>
        /// 抗风压定级变形 负压检测加压阶段初始化
        /// </summary>
        public void KFY_DJBX_FJStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //定级负压检测加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.KFY_DJBX_FJ.ToString(),
                Stage_Name = "抗风压定级变形检测负压加压",
                CompleteStatus = false,
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            for (int i = 0; i < 20; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_QSM();
                newStep.StepNO = j;
                newStep.StepName = "抗风压定级变形检测负压加压第" + j + "级";
                newStep.IsStepCompleted = false;
                newStep.IsWaitBeforCompleted = false;
                newStep.TimeWaitBefor = 0;
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_KFYDJ[3] = tempStage;
            RaisePropertyChanged(() => StageList_KFYDJ);
        }

        /// <summary>
        /// 抗风压定级反复 正压阶段初始化
        /// </summary>
        public void KFY_DJ_ZFFStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //定级反复加压检测正压加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.KFY_DJP2_Z.ToString(),
                Stage_Name = "抗风压定级反复加压检测正压加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认后。",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "抗风压定级反复加压检测正压加压",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            StageList_KFYDJ[4] = tempStage;
            RaisePropertyChanged(() => StageList_KFYDJ);
        }

        /// <summary>
        /// 抗风压定级反复 负压阶段初始化
        /// </summary>
        public void KFY_DJ_FFFStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //定级反复加压检测负压加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.KFY_DJP2_F.ToString(),
                Stage_Name = "抗风压定级反复加压检测负压加压",
                CompleteStatus = false,
                IsNeedTipsBefore = false,
                StringTipsBefore = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "抗风压定级反复加压检测负压加压",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            StageList_KFYDJ[5] = tempStage;
            RaisePropertyChanged(() => StageList_KFYDJ);
        }

        /// <summary>
        /// 抗风压定级标准值 正压阶段初始化
        /// </summary>
        public void KFY_DJ_ZP3StageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //定级标准值检测正压加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.KFY_DJP3_Z.ToString(),
                Stage_Name = "抗风压定级标准值检测正压加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认后。",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "抗风压定级标准值检测正压加压",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            StageList_KFYDJ[6] = tempStage;
            RaisePropertyChanged(() => StageList_KFYDJ);
        }

        /// <summary>
        /// 抗风压定级标准值 负压阶段初始化
        /// </summary>
        public void KFY_DJ_FP3StageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //定级标准值检测负压加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.KFY_DJP3_F.ToString(),
                Stage_Name = "抗风压定级标准值检测负压加压",
                CompleteStatus = false,
                IsNeedTipsBefore = false,
                StringTipsBefore = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "抗风压定级标准值检测负压加压",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            StageList_KFYDJ[7] = tempStage;
            RaisePropertyChanged(() => StageList_KFYDJ);
        }

        /// <summary>
        /// 抗风压定级设计值 正压阶段初始化
        /// </summary>
        public void KFY_DJ_ZPmaxStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //定级设计值检测正压加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.KFY_DJPmax_Z.ToString(),
                Stage_Name = "抗风压定级设计值检测正压加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认后。",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "抗风压定级设计值检测正压加压",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            StageList_KFYDJ[8] = tempStage;
            RaisePropertyChanged(() => StageList_KFYDJ);
        }

        /// <summary>
        /// 抗风压定级设计值 负压阶段初始化
        /// </summary>
        public void KFY_DJ_FPmaxStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //定级设计值检测负压加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.KFY_DJPmax_F.ToString(),
                Stage_Name = "抗风压定级设计值检测负压加压",
                CompleteStatus = false,
                IsNeedTipsBefore = false,
                StringTipsBefore = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "抗风压定级设计值检测负压加压",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            StageList_KFYDJ[9] = tempStage;
            RaisePropertyChanged(() => StageList_KFYDJ);
        }

        #endregion


        #region 抗风压工程阶段初始化

        /// <summary>
        /// 抗风压工程变形 正压预加压阶段初始化
        /// </summary>
        public void KFY_GCBX_ZYStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //工程变形正压预加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.KFY_GCBX_ZY.ToString(),
                Stage_Name = "抗风压工程变形检测正压预加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "预加压测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认后。",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_QSM
                {
                    StepNO = j,
                    StepName = "抗风压工程变形检测正压预加压 第" + j + "次",
                    IsStepCompleted = false,
                    IsWaitBeforCompleted = false,
                    TimeWaitBefor = 30
                };
                tempStage.StepList.Add(newStep);
            }

            StageList_KFYGC[0] = tempStage;
            RaisePropertyChanged(() => StageList_KFYGC);
        }

        /// <summary>
        /// 抗风压工程变形 正压检测加压阶段初始化
        /// </summary>
        public void KFY_GCBX_ZJStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //工程正压检测加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.KFY_GCBX_ZJ.ToString(),
                Stage_Name = "抗风压工程变形检测正压加压",
                CompleteStatus = false,
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            for (int i = 0; i < 4; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_QSM();
                newStep.StepNO = j;
                newStep.StepName = "抗风压工程变形检测正压加压第" + j + "级";
                newStep.IsStepCompleted = false;
                newStep.IsWaitBeforCompleted = false;
                newStep.TimeWaitBefor = 0;
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_KFYGC[1] = tempStage;
            RaisePropertyChanged(() => StageList_KFYGC);
        }

        /// <summary>
        /// 抗风压工程变形 负压预加压阶段初始化
        /// </summary>
        public void KFY_GCBX_FYStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //工程变形负压预加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.KFY_GCBX_FY.ToString(),
                Stage_Name = "抗风压工程变形检测负压预加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "预加压测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认。",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_QSM
                {
                    StepNO = j,
                    StepName = "抗风压工程变形检测负压预加压 第" + j + "次",
                    IsStepCompleted = false,
                    IsWaitBeforCompleted = false,
                    TimeWaitBefor = 30
                };
                tempStage.StepList.Add(newStep);
            }

            StageList_KFYGC[2] = tempStage;
            RaisePropertyChanged(() => StageList_KFYGC);
        }

        /// <summary>
        /// 抗风压工程变形 负压检测加压阶段初始化
        /// </summary>
        public void KFY_GCBX_FJStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //工程负压检测加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.KFY_GCBX_FJ.ToString(),
                Stage_Name = "抗风压工程变形检测负压加压",
                CompleteStatus = false,
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            for (int i = 0; i < 4; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_QSM();
                newStep.StepNO = j;
                newStep.StepName = "抗风压工程变形检测负压加压第" + j + "级";
                newStep.StepNeedTest = true;
                newStep.IsStepCompleted = false;
                newStep.IsWaitBeforCompleted = false;
                newStep.TimeWaitBefor = 0;
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_KFYGC[3] = tempStage;
            RaisePropertyChanged(() => StageList_KFYGC);
        }

        /// <summary>
        /// 抗风压工程反复 正压阶段初始化
        /// </summary>
        public void KFY_GC_ZFFStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //工程反复加压检测正压加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.KFY_GCP2_Z.ToString(),
                Stage_Name = "抗风压工程反复加压检测正压加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认后。",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "抗风压工程反复加压检测正压加压",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            StageList_KFYGC[4] = tempStage;
            RaisePropertyChanged(() => StageList_KFYGC);
        }

        /// <summary>
        /// 抗风压工程反复 负压阶段初始化
        /// </summary>
        public void KFY_GC_FFFStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //工程反复加压检测负压加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.KFY_GCP2_F.ToString(),
                Stage_Name = "抗风压工程反复加压检测负压加压",
                CompleteStatus = false,
                IsNeedTipsBefore = false,
                StringTipsBefore = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "抗风压工程反复加压检测负压加压",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            StageList_KFYGC[5] = tempStage;
            RaisePropertyChanged(() => StageList_KFYGC);
        }

        /// <summary>
        /// 抗风压工程标准值 正压阶段初始化
        /// </summary>
        public void KFY_GC_ZP3StageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //工程标准值检测正压加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.KFY_GCP3_Z.ToString(),
                Stage_Name = "抗风压工程标准值检测正压加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认后。",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "抗风压工程标准值检测正压加压",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            StageList_KFYGC[6] = tempStage;
            RaisePropertyChanged(() => StageList_KFYGC);
        }

        /// <summary>
        /// 抗风压工程标准值 负压阶段初始化
        /// </summary>
        public void KFY_GC_FP3StageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //工程标准值检测负压加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.KFY_GCP3_F.ToString(),
                Stage_Name = "抗风压工程标准值检测负压加压",
                CompleteStatus = false,
                IsNeedTipsBefore = false,
                StringTipsBefore = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "抗风压工程标准值检测负压加压",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            StageList_KFYGC[7] = tempStage;
            RaisePropertyChanged(() => StageList_KFYGC);
        }

        /// <summary>
        /// 抗风压工程设计值 正压阶段初始化
        /// </summary>
        public void KFY_GC_ZPmaxStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //工程设计值检测正压加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.KFY_GCPmax_Z.ToString(),
                Stage_Name = "抗风压工程设计值检测正压加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认后。",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "抗风压工程设计值检测正压加压",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            StageList_KFYGC[8] = tempStage;
            RaisePropertyChanged(() => StageList_KFYGC);
        }

        /// <summary>
        /// 抗风压工程设计值 负压阶段初始化
        /// </summary>
        public void KFY_GC_FPmaxStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //工程设计值检测负压加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.KFY_GCPmax_F.ToString(),
                Stage_Name = "抗风压工程设计值检测负压加压",
                CompleteStatus = false,
                IsNeedTipsBefore = false,
                StringTipsBefore = "",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            newStep = new MQZH_StepModel_QSM
            {
                StepNO = 1,
                StepName = "抗风压工程设计值检测负压加压",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            StageList_KFYGC[9] = tempStage;
            RaisePropertyChanged(() => StageList_KFYGC);
        }

        #endregion


        #region 阶段、页面可选、被选属性初始化

        /// <summary>
        /// 可选属性初始化
        /// </summary>
        public void CanBeCheckInit()
        {
            //备选数组清空
            BeenCheckedDJ = new ObservableCollection<bool>() { false, false, false, false, false, false, false, false, false, false };
            BeenCheckedGC = new ObservableCollection<bool>() { false, false, false, false, false, false, false, false, false, false };

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
            for (int i = 0; i < StageList_KFYDJ.Count; i++)
            {
                //阶段需要做实验属性
                if (IsGC)
                    StageList_KFYDJ[i].NeedTest = false;
                else
                    StageList_KFYDJ[i].NeedTest = true;
                //步骤需要做实验属性
                for (int j = 0; j < StageList_KFYDJ[i].StepList.Count; j++)
                {
                    if (IsGC)
                        StageList_KFYDJ[i].StepList[j].StepNeedTest = false;
                    else
                        StageList_KFYDJ[i].StepList[j].StepNeedTest = true;
                }
            }

            //工程
            for (int i = 0; i < StageList_KFYGC.Count; i++)
            {
                //阶段需要做实验属性
                if (!IsGC)
                    StageList_KFYGC[i].NeedTest = false;
                else
                    StageList_KFYGC[i].NeedTest = true;
                //步骤需要做实验属性
                for (int j = 0; j < StageList_KFYGC[i].StepList.Count; j++)
                {
                    if (!IsGC)
                        StageList_KFYGC[i].StepList[j].StepNeedTest = false;
                    else
                        StageList_KFYGC[i].StepList[j].StepNeedTest = true;
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
            Stage_DQ = new MQZH_StageModel_QSM();
            Step_DQ = new MQZH_StepModel_QSM();
        }

        #endregion
    }


        /// <summary>
        /// 位移测量组类
        /// </summary>
        public class DisplaceGroup : ObservableObject
    {
        #region 设定参数

        /// <summary>
        /// 位移测量组编号
        /// </summary>
        private string _id = "1";
        /// <summary>
        /// 位移测量组编号
        /// </summary>
        /// <remarks>测面板变形、三角变形时，使用第1组</remarks>
        public string ID
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged(() => ID);
            }
        }

        /// <summary>
        /// 是否使用位移测量组
        /// </summary>
        private bool _is_Use = true;
        /// <summary>
        /// 是否使用位移测量组
        /// </summary>
        public bool Is_Use
        {
            get { return _is_Use; }
            set
            {
                _is_Use = value;
                if (!_is_Use)
                {
                    Is_TestMBBX = false;
                    WY_CS[0] =0;
                    WY_CS[1] = 0;
                    WY_CS[2] = 0;
                    WY_CS[3] = 0;
                    WY_DQ[0] = 0;
                    WY_DQ[1] = 0;
                    WY_DQ[2] = 0;
                    WY_DQ[3] = 0;
                    ND = 0;
                    ND_XD = 0;
                    BigThanNDp1 = false;
                    BigThanNDYX = false;
                }
                    
                RaisePropertyChanged(() => Is_Use);
            }
        }
        
        /// <summary>
        /// 是否测面板变形
        /// </summary>
        private bool _is_TestMBBX = false;
        /// <summary>
        /// 是否测面板变形
        /// </summary>
        public bool Is_TestMBBX
        {
            get { return _is_TestMBBX; }
            set
            {
                _is_TestMBBX = value;
                if (!_is_TestMBBX)
                {
                    IsBZCSJMB = false;
                }
                RaisePropertyChanged(() => Is_TestMBBX);
            }
        }

        /// <summary>
        /// 是否测边支承三角玻璃面板
        /// </summary>
        private bool _isBZCSJMB = false;
        /// <summary>
        /// 是否测边支承三角玻璃面板
        /// </summary>
        public bool IsBZCSJMB
        {
            get
            {
                return _isBZCSJMB;
            }
            set
            {
                _isBZCSJMB = value;
                RaisePropertyChanged(() => IsBZCSJMB);
            }
        }

        /// <summary>
        /// 测点间距（mm）
        /// </summary>
        private double _l = 2000;
        /// <summary>
        /// 测点间距（mm）
        /// </summary>
        /// <remarks>边支承三角玻璃面板时，为长边对应的高</remarks>
        public double L
        {
            get { return _l; }
            set
            {
                _l = Math .Abs( value);
                RaisePropertyChanged(() => L);
                RaisePropertyChanged(() => ND_XD_YX);
                RaisePropertyChanged(() => ND_YX);
                RaisePropertyChanged(() => ND_YX_P1);
            }
        }
        
        /// <summary>
        /// 构件允许相对面法线挠度（分母）
        /// </summary>
        private double _nd_XD_YXFM = 180;
        /// <summary>
        /// 构件允许相对面法线挠度（分母）
        /// </summary>
        public double ND_XD_YXFM
        {
            get { return _nd_XD_YXFM; }
            set
            {
                _nd_XD_YXFM = Math.Abs(value);
                RaisePropertyChanged(() => ND_XD_YXFM);
                RaisePropertyChanged(() => ND_XD_YX);
                RaisePropertyChanged(() => ND_YX);
                RaisePropertyChanged(() => ND_YX_P1);
            }
        }
        /// <summary>
        /// 构件允许相对面法线挠度
        /// </summary>
        public double ND_XD_YX
        {
            get { return 1/ND_XD_YXFM; }
        }
        /// <summary>
        /// 构件允许绝对挠度(P3)
        /// </summary>
        public double ND_YX
        {
            get { return L / ND_XD_YXFM; }
        }
        /// <summary>
        /// 构件允许绝对挠度(P1)
        /// </summary>
        public double ND_YX_P1
        {
            get { return L / ND_XD_YXFM / 2.5; }
        }

        /// <summary>
        /// 各点对应位移尺编号
        /// </summary>
        private ObservableCollection<int> _wyc_No = new ObservableCollection<int>();
        /// <summary>
        /// 测点当前位移
        /// </summary>
        public ObservableCollection<int> WYC_No
        {
            get { return _wyc_No; }
            set
            {
                _wyc_No = value;
                RaisePropertyChanged(() => WYC_No);
            }
        }
        
        #endregion


        #region 检测参数
        
        /// <summary>
        /// 测点初始位移
        /// </summary>
        private ObservableCollection<double> _wy_CS = new ObservableCollection<double>() { 0, 0, 0, 0 };
        /// <summary>
        /// 测点初始位移
        /// </summary>
        public ObservableCollection<double> WY_CS
        {
            get { return _wy_CS; }
            set
            {
                _wy_CS = value;
                for (int i = 0; i < _wy_CS.Count; i++)
                {
                    if (!Is_Use)
                        _wy_CS[i] = 0;
                }
                RaisePropertyChanged(() => WY_CS);
            }
        }

        /// <summary>
        /// 测点当前位移
        /// </summary>
        private ObservableCollection<double> _wy_DQ = new ObservableCollection<double>() { 0, 0, 0, 0 };
        /// <summary>
        /// 测点当前位移
        /// </summary>
        public ObservableCollection<double> WY_DQ
        {
            get { return _wy_DQ; }
            set
            {
                _wy_DQ = value;
                for (int i = 0; i < _wy_DQ.Count; i++)
                {
                    if (!Is_Use)
                        _wy_DQ[i] = 0;
                }
                RaisePropertyChanged(() => WY_DQ);
            }
        }

        /// <summary>
        /// 当前面法线挠度
        /// </summary>
        private double _nd = 0;
        /// <summary>
        /// 当前面法线挠度
        /// </summary>
        public double ND
        {
            get { return _nd; }
            set
            {
                _nd = Is_Use ? value : 0;
                RaisePropertyChanged(() => ND);
            }
        }

        /// <summary>
        /// 当前相对面法线挠度
        /// </summary>
        private double _nd_XD = 0;
        /// <summary>
        /// 当前相对面法线挠度
        /// </summary>
        public double ND_XD
        {
            get { return _nd_XD; }
            set
            {
                _nd_XD = Is_Use ? value : 0;
                RaisePropertyChanged(() => ND_XD);
            }
        }

        /// <summary>
        /// 达到（或超过）允许面法线挠度的40%（压力达到p1值）
        /// </summary>
        private bool _bigThanNDp1= false;
        /// <summary>
        /// 达到（或超过）允许面法线挠度的40%（压力达到p1值）
        /// </summary>
        public bool BigThanNDp1
        {
            get { return _bigThanNDp1; }
            set
            {
                _bigThanNDp1 = Is_Use && value;
                RaisePropertyChanged(() => BigThanNDp1);
            }
        }

        /// <summary>
        /// 达到（或超过）允许面法线挠度
        /// </summary>
        private bool _bigThanNDYX = false;
        /// <summary>
        /// 达到（或超过）允许面法线挠度
        /// </summary>
        public bool BigThanNDYX
        {
            get { return _bigThanNDYX; }
            set
            {
                _bigThanNDYX = Is_Use && value;
                RaisePropertyChanged(() => BigThanNDYX);
            }
        }

        #endregion


        #region 设置、计算

        /// <summary>
        /// 设置初始位移
        /// </summary>
        public void SetWYCS()
        {
            for (int i = 0; i < WY_CS.Count; i++)
            {
                WY_CS[i] = WY_DQ[i];
            }
        }

        /// <summary>
        /// 挠度计算
        /// </summary>
        public void NDJS()
        {
            //使用测点阻
            if (Is_Use)
            {
                if (Is_TestMBBX && IsBZCSJMB)
                    ND = WY_DQ[3] - WY_CS[3] - (WY_DQ[0] - WY_CS[0] + (WY_DQ[1] - WY_CS[1]) + (WY_DQ[2] - WY_CS[2])) / 3;
                else
                    ND = WY_DQ[1] - WY_CS[1] - (WY_DQ[0] - WY_CS[0] + (WY_DQ[2] - WY_CS[2])) / 2;
                //相对面法线挠度
                if (L != 0.0)
                    ND_XD = ND / L;
                else
                    ND_XD = 0;
                //分析是否达到或超过允许挠度
                BigThanNDYX = (Math.Abs(ND_XD) >= ND_XD_YX) ? true : false;
                BigThanNDp1 = (Math.Abs(ND_XD * 2.5) >= ND_XD_YX) ? true : false;
            }
            else
            {
                ND = 0;
                ND_XD = 0;
                BigThanNDYX = false;
                BigThanNDp1 = false;
            }
        }

        #endregion
    }
}
