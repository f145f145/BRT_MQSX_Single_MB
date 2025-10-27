/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQDFJ_MB.Model.Exp
 * 文件名：  MQZH_ExpModel_SM
 * 版本号：  V1.0.0.0
 * 唯一标识：2f3f96fb-6ae3-4bfc-b597-550152445590
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/11/20 18:17:05
 * 描述：
 * 幕墙四性水密检测Model
 * ==================================================================================
 * 修改标记
 * 修改时间			        修改人			版本号			描述
 * 2021/11/20 18:17:05		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static MQDFJ_MB.Model.MQZH_Enums;


namespace MQDFJ_MB.Model.Exp
{
    /// <summary>
    /// 幕墙四性水密试验Model类
    /// </summary>
    public class MQZH_ExpModel_SM : ObservableObject
    {
        public MQZH_ExpModel_SM()
        {
            SMExpInit();
        }

        #region 水密检测设定参数属性

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
        /// 加压类型（true为波动）
        /// </summary>
        private bool _waveType_SM = false;
        /// <summary>
        /// 加压类型（true为波动）
        /// </summary>
        public bool WaveType_SM
        {
            get { return _waveType_SM; }
            set
            {
                _waveType_SM = value;
                RaisePropertyChanged(() => WaveType_SM);
            }
        }

        /// <summary>
        /// 水密项目完成状态
        /// </summary>
        private bool _completeStatus = false;
        /// <summary>
        /// 水密项目完成状态
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
        /// 可开启部分水密压力工程设计值
        /// </summary>
        private double _sm_GCSJ_KKQ = 500;
        /// <summary>
        /// 可开启部分水密压力工程设计值
        /// </summary>
        public double SM_GCSJ_KKQ
        {
            get { return _sm_GCSJ_KKQ; }
            set
            {
                _sm_GCSJ_KKQ = value;
                SMGC_JStageSelect();
                RaisePropertyChanged(() => SM_GCSJ_KKQ);
            }
        }


        /// <summary>
        /// 固定部分水密压力工程设计值
        /// </summary>
        private double _sm_GCSJ_GD = 500;
        /// <summary>
        /// 固定部分水密压力工程设计值
        /// </summary>
        public double SM_GCSJ_GD
        {
            get { return _sm_GCSJ_GD; }
            set
            {
                _sm_GCSJ_GD = value;
                SMGC_JStageSelect();
                RaisePropertyChanged(() => SM_GCSJ_GD);
            }
        }


        /// <summary>
        /// 箱口高度（m）
        /// </summary>
        private double _height_XK = 3;
        /// <summary>
        /// 箱口高度（m）
        /// </summary>
        public double Height_XK
        {
            get { return _height_XK; }
            set
            {
                _height_XK = value;
                RaisePropertyChanged(() => Height_XK);
                RaisePropertyChanged(() => Aria_XK);
                RaisePropertyChanged(() => SM_SLL);
            }
        }


        /// <summary>
        /// 箱口宽度（m）
        /// </summary>
        private double _width_XK = 6;
        /// <summary>
        /// 箱口宽度（m）
        /// </summary>
        public double Width_XK
        {
            get { return _width_XK; }
            set
            {
                _width_XK = value;
                RaisePropertyChanged(() => Width_XK);
                RaisePropertyChanged(() => Aria_XK);
                RaisePropertyChanged(() => SM_SLL);
            }
        }


        /// <summary>
        /// 箱口面积（㎡）
        /// </summary>
        public double Aria_XK
        {
            get { return Width_XK* Height_XK; }
        }


        /// <summary>
        /// 单位面积淋水流量（L/㎡.min）
        /// </summary>
        private double _sm_SLL_Per_m2 = 3;
        /// <summary>
        /// 单位面积淋水流量（L/㎡.min）
        /// </summary>
        public double SM_SLL_Per_m2
        {
            get { return _sm_SLL_Per_m2; }
            set
            {
                _sm_SLL_Per_m2 = value;
                RaisePropertyChanged(() => SM_SLL_Per_m2);
                RaisePropertyChanged(() => SM_SLL);
            }
        }


        /// <summary>
        /// 淋水流量（m³/h）
        /// </summary>
        public double SM_SLL
        {
            get { return SM_SLL_Per_m2* Aria_XK*60/1000; }
        }
        #endregion


        #region 运行及辅助属性

        /// <summary>
        /// 波动分步提示
        /// </summary>
        private string _waveInfo = "";
        /// <summary>
        /// 波动分步提示
        /// </summary>
        public string WaveInfo
        {
            get { return _waveInfo; }
            set
            {
                _waveInfo = value;
                RaisePropertyChanged(() => WaveInfo);
            }
        }

        /// <summary>
        /// 水密当前阶段
        /// </summary>
        private MQZH_StageModel_QSM _stage_DQ = new MQZH_StageModel_QSM();

        /// <summary>
        /// 水密当前阶段
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
        /// 水密当前步骤
        /// </summary>
        private MQZH_StepModel_QSM _step_DQ = new MQZH_StepModel_QSM();

        /// <summary>
        /// 水密当前步骤
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
        private ObservableCollection<bool> _beenCheckedDJ = new ObservableCollection<bool>() { false, false };
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
        private ObservableCollection<bool> _beenCheckedGC = new ObservableCollection<bool>() { false, false, false };

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


        #region 水密检测试验阶段组

        /// <summary>
        /// 水密定级检测试验阶段组
        /// </summary>
        private List<MQZH_StageModel_QSM> _stageList_SMDJ;

        /// <summary>
        /// 水密定级检测试验阶段组
        /// </summary>
        public List<MQZH_StageModel_QSM> StageList_SMDJ
        {
            get { return _stageList_SMDJ; }
            set
            {
                _stageList_SMDJ = value;
                RaisePropertyChanged(() => StageList_SMDJ);
            }
        }

        /// <summary>
        /// 水密工程检测试验阶段组
        /// </summary>
        private List<MQZH_StageModel_QSM> _stageList_SMGC;

        /// <summary>
        /// 水密工程检测试验阶段组
        /// </summary>
        public List<MQZH_StageModel_QSM> StageList_SMGC
        {
            get { return _stageList_SMGC; }
            set
            {
                _stageList_SMGC = value;
                RaisePropertyChanged(() => StageList_SMGC);
            }
        }

        /// <summary>
        /// 水密工程检测试验阶段（2步）
        /// </summary>
        private MQZH_StageModel_QSM _stageJ_2Steps_SMGC;

        /// <summary>
        /// 水密工程检测试验阶段（2步）
        /// </summary>
        public MQZH_StageModel_QSM StageJ_2Steps_SMGC
        {
            get { return _stageJ_2Steps_SMGC; }
            set
            {
                _stageJ_2Steps_SMGC = value;
                RaisePropertyChanged(() => StageJ_2Steps_SMGC);
            }
        }

        /// <summary>
        /// 水密工程检测试验阶段（3步）
        /// </summary>
        private MQZH_StageModel_QSM _stageJ_3Steps_SMGC;

        /// <summary>
        /// 水密工程检测试验阶段（3步）
        /// </summary>
        public MQZH_StageModel_QSM StageJ_3Steps_SMGC
        {
            get { return _stageJ_3Steps_SMGC; }
            set
            {
                _stageJ_3Steps_SMGC = value;
                RaisePropertyChanged(() => StageJ_3Steps_SMGC);
            }
        }

        /// <summary>
        /// 水密工程检测试验阶段（9步）
        /// </summary>
        private MQZH_StageModel_QSM _stageJ_9Steps_SMGC;

        /// <summary>
        /// 水密工程检测试验阶段（9步）
        /// </summary>
        public MQZH_StageModel_QSM StageJ_9Steps_SMGC
        {
            get { return _stageJ_9Steps_SMGC; }
            set
            {
                _stageJ_9Steps_SMGC = value;
                RaisePropertyChanged(() => StageJ_9Steps_SMGC);
            }
        }

        /// <summary>
        /// 水密工程检测试验阶段（10步）
        /// </summary>
        private MQZH_StageModel_QSM _stageJ_10Steps_SMGC;

        /// <summary>
        /// 水密工程检测试验阶段（10步）
        /// </summary>
        public MQZH_StageModel_QSM StageJ_10Steps_SMGC
        {
            get { return _stageJ_10Steps_SMGC; }
            set
            {
                _stageJ_10Steps_SMGC = value;
                RaisePropertyChanged(() => StageJ_10Steps_SMGC);
            }
        }

        #endregion


        #region 水密项目试验总初始化

        /// <summary>
        /// 水密试验参数初始化。
        ///  </summary>
        /// <remarks>初始化之前，需设定相关参数</remarks>
        public void SMExpInit()
        {
            //当前阶段、步骤初始化
            Stage_DQ = null;
            Stage_DQ = new MQZH_StageModel_QSM();
            Step_DQ = null;
            Step_DQ = new MQZH_StepModel_QSM();

            //定级阶段list初始化（2个阶段）
            StageList_SMDJ = null;
            StageList_SMDJ = new List<MQZH_StageModel_QSM>();
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            StageList_SMDJ.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_SMDJ.Add(tempStage);
            SMDJ_YStageInit();
            SMDJ_JStageInit();

            //工程阶段list初始化（3种各2个阶段）
            StageList_SMGC = null;
            StageList_SMGC = new List<MQZH_StageModel_QSM>();
            tempStage = new MQZH_StageModel_QSM();
            StageList_SMGC.Add(tempStage);
            tempStage = new MQZH_StageModel_QSM();
            StageList_SMGC.Add(tempStage);
            SMGC_YStageInit();
            SMGC_JStageInit();
            SMGC_JStageSelect();

            //可选、需要做实验属性初始化
            StageStepNeedInit();
            CanBeCheckInit();
        }

        #endregion


        #region 水密项目定级阶段初始化

        /// <summary>
        /// 水密定级 预加压阶段初始化
        /// </summary>
        public void SMDJ_YStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //定级正压预加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.SM_DJ_Y.ToString(),
                Stage_Name = "水密定级预加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "预加压测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认后再点击确认。",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_QSM
                {
                    StepNO = j,
                    StepName = "水密定级预加压 第" + j + "次",
                    IsStepCompleted = false,
                    IsWaitBeforCompleted = false,
                    TimeWaitBefor = 30
                };
                tempStage.StepList.Add(newStep);
            }

            StageList_SMDJ[0] = tempStage;
            RaisePropertyChanged(() => StageList_SMDJ);
        }

        /// <summary>
        /// 水密定级 检测加压阶段初始化
        /// </summary>
        public void SMDJ_JStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //定级检测加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.SM_DJ_J.ToString(),
                Stage_Name = "水密定级检测加压",
                CompleteStatus = false,
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            for (int i = 0; i < 8; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_QSM
                {
                    StepNO = j,
                    StepName = "水密定级检测加压 第" + j + "级",
                    IsStepCompleted = false,
                    IsWaitBeforCompleted = false,
                    TimeWaitBefor = 0
                };
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_SMDJ[1] = tempStage;
            RaisePropertyChanged(() => StageList_SMDJ);
        }

        #endregion


        #region 水密项目工程阶段初始化

        /// <summary>
        /// 水密工程 预加压阶段初始化
        /// </summary>
        public void SMGC_YStageInit()
        {
            MQZH_StageModel_QSM tempStage = new MQZH_StageModel_QSM();
            MQZH_StepModel_QSM newStep = new MQZH_StepModel_QSM();
            //工程预加压阶段
            tempStage = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.SM_GC_Y.ToString(),
                Stage_Name = "水密工程预加压",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "预加压测试前，请将试件可开启部分启闭不少于5次，并最终锁紧后再点击确认后再点击确认。",
                StepList = null
            };
            tempStage.StepList = new List<MQZH_StepModel_QSM>();

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_QSM
                {
                    StepNO = j,
                    StepName = "水密工程预加压 第" + j + "次",
                    IsStepCompleted = false,
                    IsWaitBeforCompleted = false,
                    TimeWaitBefor = 30
                };
                tempStage.StepList.Add(newStep);
            }

            StageList_SMGC[0] = tempStage;

            RaisePropertyChanged(() => StageList_SMGC);
        }

        /// <summary>
        /// 水密工程 检测加压阶段初始化(4种阶段）
        /// </summary>
        public void SMGC_JStageInit()
        {
            MQZH_StepModel_QSM newStep;

            //2步骤
            StageJ_2Steps_SMGC = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.SM_GC_J.ToString(),
                Stage_Name = "水密工程检测加压",
                CompleteStatus = false,
                StepList = null
            };
            StageJ_2Steps_SMGC.StepList = new List<MQZH_StepModel_QSM>();
            for (int i = 0; i < 2; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_QSM
                {
                    StepNO = j,
                    StepName = "水密工程检测加压 第" + j + "级",
                    IsStepCompleted = false,
                    IsWaitBeforCompleted = false,
                    TimeWaitBefor = 0
                };
                StageJ_2Steps_SMGC.StepList.Add(newStep);
            }
            StageJ_2Steps_SMGC.StepList[0].TimeWaitBefor = 10;
            StageJ_2Steps_SMGC.StepList[0].StepName = "工程检测加压 喷水";
            StageJ_2Steps_SMGC.StepList[1].StepName = "工程检测加压 固定部分检测";

            //3步骤
            StageJ_3Steps_SMGC = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.SM_GC_J.ToString(),
                Stage_Name = "水密工程检测加压",
                CompleteStatus = false,
                StepList = null
            };
            StageJ_3Steps_SMGC.StepList = new List<MQZH_StepModel_QSM>();
            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_QSM
                {
                    StepNO = j,
                    StepName = "水密工程检测加压 第" + j + "级",
                    IsStepCompleted = false,
                    IsWaitBeforCompleted = false,
                    TimeWaitBefor = 0
                };
                StageJ_3Steps_SMGC.StepList.Add(newStep);
            }
            StageJ_3Steps_SMGC.StepList[0].TimeWaitBefor = 10;
            StageJ_3Steps_SMGC.StepList[0].StepName = "工程检测加压 喷水";
            StageJ_3Steps_SMGC.StepList[1].StepName = "工程检测加压 可开启部分检测";
            StageJ_3Steps_SMGC.StepList[2].StepName = "工程检测加压 固定部分检测";

            //9步骤
            StageJ_9Steps_SMGC = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.SM_GC_J.ToString(),
                Stage_Name = "水密工程检测加压",
                CompleteStatus = false,
                StepList = null
            };
            StageJ_9Steps_SMGC.StepList = new List<MQZH_StepModel_QSM>();
            for (int i = 0; i < 9; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_QSM
                {
                    StepNO = j,
                    StepName = "水密工程检测加压 第" + j + "级",
                    IsStepCompleted = false,
                    IsWaitBeforCompleted = false,
                    TimeWaitBefor = 0
                };
                StageJ_9Steps_SMGC.StepList.Add(newStep);
            }
            StageJ_9Steps_SMGC.StepList[0].TimeWaitBefor = 10;
            StageJ_9Steps_SMGC.StepList[8].StepName = "工程检测加压 固定部分检测";

            //10步骤
            StageJ_10Steps_SMGC = new MQZH_StageModel_QSM
            {
                Stage_NO = TestStageType.SM_GC_J.ToString(),
                Stage_Name = "水密工程检测加压",
                CompleteStatus = false,
                StepList = null
            };
            StageJ_10Steps_SMGC.StepList = new List<MQZH_StepModel_QSM>();
            for (int i = 0; i < 10; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_QSM
                {
                    StepNO = j,
                    StepName = "水密工程检测加压 第" + j + "级",
                    IsStepCompleted = false,
                    IsWaitBeforCompleted = false,
                    TimeWaitBefor = 0
                };
                StageJ_10Steps_SMGC.StepList.Add(newStep);
            }
            StageJ_10Steps_SMGC.StepList[0].TimeWaitBefor = 10;
            StageJ_10Steps_SMGC.StepList[8].StepName = "工程检测加压 可开启部分检测";
            StageJ_10Steps_SMGC.StepList[9].StepName = "工程检测加压 固定部分检测";

        }
        
        /// <summary>
        /// 水密工程 检测加压阶段3选1（有可开启）
        /// </summary>
        public void SMGC_JStageSelect()
        {
            //固定指标>开启指标>2000时，10级；固定>2000>开启时，9级，固定、开启均小于2000时，3级
            if ((SM_GCSJ_KKQ > 2000) && (SM_GCSJ_GD > SM_GCSJ_KKQ))
                StageList_SMGC[1] = StageJ_10Steps_SMGC;
            else if ((SM_GCSJ_GD > 2000) && (SM_GCSJ_KKQ < 2000))
                StageList_SMGC[1] = StageJ_9Steps_SMGC;
            else
                StageList_SMGC[1] = StageJ_3Steps_SMGC;

            RaisePropertyChanged(() => StageList_SMGC);
        }
        
        /// <summary>
        /// 水密工程 检测加压阶段选择（无可开启部分）
        /// </summary>
        public void SMGC_JStageSelect2()
        {
            //固定指标>2000时，9级；固定指标<=2000时，2级
            if (SM_GCSJ_GD > 2000) 
                StageList_SMGC[1] = StageJ_9Steps_SMGC;
            else
                StageList_SMGC[1] = StageJ_2Steps_SMGC;

            RaisePropertyChanged(() => StageList_SMGC);
        }

        #endregion


        #region 阶段、页面可选、被选属性初始化

        /// <summary>
        /// 可选属性初始化
        /// </summary>
        public void CanBeCheckInit()
        {
            //备选数组清空
            BeenCheckedDJ = new ObservableCollection<bool>() { false, false };
            BeenCheckedGC = new ObservableCollection<bool>() { false, false };

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
            for (int i = 0; i < StageList_SMDJ.Count; i++)
            {
                //阶段需要做实验属性
                if (IsGC)
                    StageList_SMDJ[i].NeedTest = false;
                else
                    StageList_SMDJ[i].NeedTest = true;
                //步骤需要做实验属性
                for (int j = 0; j < StageList_SMDJ[i].StepList.Count; j++)
                {
                    if (IsGC)
                        StageList_SMDJ[i].StepList[j].StepNeedTest = false;
                    else
                        StageList_SMDJ[i].StepList[j].StepNeedTest = true;
                }
            }
            //工程
            //预加压阶段需要做实验属性
            if (IsGC)
                StageList_SMGC[0].NeedTest = true;
            else
                StageList_SMGC[0].NeedTest = false;
            for (int j = 0; j < StageList_SMGC[0].StepList.Count; j++)
            {
                if (IsGC)
                    StageList_SMGC[0].StepList[j].StepNeedTest = true;
                else
                    StageList_SMGC[0].StepList[j].StepNeedTest = false;
            }
            //3步步骤检测加压阶段
            if (IsGC)
                StageJ_3Steps_SMGC.NeedTest = true;
            else
                StageJ_3Steps_SMGC.NeedTest = false;
            for (int j = 0; j < StageJ_3Steps_SMGC.StepList.Count; j++)
            {
                if (IsGC)
                    StageJ_3Steps_SMGC.StepList[j].StepNeedTest = true;
                else
                    StageJ_3Steps_SMGC.StepList[j].StepNeedTest = false;
            }
            //9步步骤检测加压阶段
            if (IsGC)
                StageJ_9Steps_SMGC.NeedTest = true;
            else
                StageJ_9Steps_SMGC.NeedTest = false;
            for (int j = 0; j < StageJ_9Steps_SMGC.StepList.Count; j++)
            {
                if (IsGC)
                    StageJ_9Steps_SMGC.StepList[j].StepNeedTest = true;
                else
                    StageJ_9Steps_SMGC.StepList[j].StepNeedTest = false;
            }
            //10步步骤检测加压阶段
            if (IsGC)
                StageJ_10Steps_SMGC.NeedTest = true;
            else
                StageJ_10Steps_SMGC.NeedTest = false;
            for (int j = 0; j < StageJ_10Steps_SMGC.StepList.Count; j++)
            {
                if (IsGC)
                    StageJ_10Steps_SMGC.StepList[j].StepNeedTest = true;
                else
                    StageJ_10Steps_SMGC.StepList[j].StepNeedTest = false;
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
}
