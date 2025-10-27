/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQDFJ_MB.Model.Exp
 * 文件名：  MQZH_ExpModel_CJBX
 * 版本号：  V1.0.0.0
 * 唯一标识：2f3f96fb-6ae3-4bfc-b597-550152445590
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/11/20 18:17:05
 * 描述：
 * 幕墙四性层间变形检测Model
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2021/11/20       18:17:05		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static MQDFJ_MB.Model.MQZH_Enums;

namespace MQDFJ_MB.Model.Exp
{
    /// <summary>
    /// 幕墙四性层间变形试验类
    /// </summary>
    public class MQZH_ExpModel_CJBX : ObservableObject
    {
        public MQZH_ExpModel_CJBX()
        {
            //构造时创建，后期重新初始化时在外部传入
            IsGC = false;
            NeedTest = true;
            CJBX_SJZBX = 400;
            CJBX_SJZBY = 400;
            CJBX_SJZBZ = 5;
            XishuXY = 2;
            SJ_CG = 4.5;
            CompleteStatus = false;

            //重置当前阶段等运行属性
            ExpXM_CJBXStage_DQ = null;
            ExpXM_CJBXStage_DQ = new MQZH_StageModel_CJBX();
            ExpXM_CJBXStep_DQ = new MQZH_StepModel_CJBX();

            CJBXExpDJInit();
            CJBXExpGCInit();
            StageStepNeedInit();
        }

        /// <summary>
        /// 层高（m）
        /// </summary>
        private double _sj_CG = 3;
        /// <summary>
        /// 层高（m）
        /// </summary>
        public double SJ_CG
        {
            get
            {
                return _sj_CG;
            }
            set
            {
                _sj_CG = value;
                RaisePropertyChanged(() => SJ_CG);
            }
        }

        #region 层间变形检测设定、传入参数属性

        /// <summary>
        /// 检测类型（true为工程）
        /// </summary>
        private bool _isGC = false;
        /// <summary>
        /// 检测类型（true为工程）
        /// </summary>
        public bool IsGC
        {
            get
            {
                return _isGC;
            }
            set
            {
                _isGC = value;
                if (_isGC)
                {
                    CJBX_SJZBX = 400;
                    CJBX_SJZBY = 400;
                    CJBX_SJZBZ = 5;
                }
                else
                {
                    CJBX_SJZBX = 0;
                    CJBX_SJZBY = 0;
                    CJBX_SJZBZ = 0;
                }
                if(StageList_DJY !=null )
                StageStepNeedInit();
                RaisePropertyChanged(() => IsGC);
            }
        }

        /// <summary>
        /// 工程检测设计指标X（位移角分母）
        /// </summary>
        private double _cjbx_SJZBX = 400;
        /// <summary>
        /// 工程检测设计指标X（位移角分母）
        /// </summary>
        public double CJBX_SJZBX
        {
            get
            {
                return _cjbx_SJZBX;
            }
            set
            {
                _cjbx_SJZBX = value;
                RaisePropertyChanged(() => CJBX_SJZBX);
            }
        }

        /// <summary>
        /// 工程检测设计指标Y（位移角分母）
        /// </summary>
        private double _cjbx_SJZBY = 400;
        /// <summary>
        /// 工程检测设计指标Y（位移角分母）
        /// </summary>
        public double CJBX_SJZBY
        {
            get
            {
                return _cjbx_SJZBY;
            }
            set
            {
                _cjbx_SJZBY = value;
                RaisePropertyChanged(() => CJBX_SJZBY);
            }
        }

        /// <summary>
        /// 工程检测设计指标Z（绝对位移mm）
        /// </summary>
        private double _cjbx_SJZBZ = 5;
        /// <summary>
        /// 工程检测设计指标Z（绝对位移mm）
        /// </summary>
        public double CJBX_SJZBZ
        {
            get
            {
                return _cjbx_SJZBZ;
            }
            set
            {
                _cjbx_SJZBZ = value;
                RaisePropertyChanged(() => CJBX_SJZBZ);
            }
        }

        #endregion


        #region 状态、辅助参数等属性

        /// <summary>
        /// 项目选中状态（true为选中）
        /// </summary>
        private bool _needTest = true;
        /// <summary>
        /// 项目选中状态（true为选中）
        /// </summary>
        public bool NeedTest
        {
            get { return _needTest; }
            set
            {
                _needTest = value;
                RaisePropertyChanged(() => NeedTest);
            }
        }

        /// <summary>
        /// X、Y轴位移系数（1或2）
        /// </summary>
        private double _xishuXY = 1;
        /// <summary>
        /// X、Y轴位移系数（1或2）
        /// </summary>
        public double XishuXY
        {
            get
            {
                return _xishuXY;
            }
            set
            {
                _xishuXY = value;
                RaisePropertyChanged(() => XishuXY);
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
        /// 定级X试验阶段被选数组
        /// </summary>
        private ObservableCollection<bool> _beenCheckedDJX;
        /// <summary>
        /// 定级X试验阶段被选数组
        /// </summary>
        public ObservableCollection<bool> BeenCheckedDJX
        {
            get { return _beenCheckedDJX; }
            set
            {
                _beenCheckedDJX = value;
                RaisePropertyChanged(() => BeenCheckedDJX);
            }
        }

        /// <summary>
        /// 定级Y试验阶段被选数组
        /// </summary>
        private ObservableCollection<bool> _beenCheckedDJY;
        /// <summary>
        /// 定级Y试验阶段被选数组
        /// </summary>
        public ObservableCollection<bool> BeenCheckedDJY
        {
            get { return _beenCheckedDJY; }
            set
            {
                _beenCheckedDJY = value;
                RaisePropertyChanged(() => BeenCheckedDJY);
            }
        }

        /// <summary>
        /// 定级Z试验阶段被选数组
        /// </summary>
        private ObservableCollection<bool> _beenCheckedDJZ;
        /// <summary>
        /// 定级Z试验阶段被选数组
        /// </summary>
        public ObservableCollection<bool> BeenCheckedDJZ
        {
            get { return _beenCheckedDJZ; }
            set
            {
                _beenCheckedDJZ = value;
                RaisePropertyChanged(() => BeenCheckedDJZ);
            }
        }

        /// <summary>
        /// 工程X试验阶段被选数组
        /// </summary>
        private ObservableCollection<bool> _beenCheckedGCX;
        /// <summary>
        /// 工程X试验阶段被选数组
        /// </summary>
        public ObservableCollection<bool> BeenCheckedGCX
        {
            get { return _beenCheckedGCX; }
            set
            {
                _beenCheckedGCX = value;
                RaisePropertyChanged(() => BeenCheckedGCX);
            }
        }

        /// <summary>
        /// 工程Y试验阶段被选数组
        /// </summary>
        private ObservableCollection<bool> _beenCheckedGCY;
        /// <summary>
        /// 工程Y试验阶段被选数组
        /// </summary>
        public ObservableCollection<bool> BeenCheckedGCY
        {
            get { return _beenCheckedGCY; }
            set
            {
                _beenCheckedGCY = value;
                RaisePropertyChanged(() => BeenCheckedGCY);
            }
        }

        /// <summary>
        /// 工程Z试验阶段被选数组
        /// </summary>
        private ObservableCollection<bool> _beenCheckedGCZ;
        /// <summary>
        /// 工程Z试验阶段被选数组
        /// </summary>
        public ObservableCollection<bool> BeenCheckedGCZ
        {
            get { return _beenCheckedGCZ; }
            set
            {
                _beenCheckedGCZ = value;
                RaisePropertyChanged(() => BeenCheckedGCZ);
            }
        }

        /// <summary>
        /// 当前阶段
        /// </summary>
        private MQZH_StageModel_CJBX _expXM_CJBXStage_DQ;
        /// <summary>
        /// 当前阶段
        /// </summary>
        public MQZH_StageModel_CJBX ExpXM_CJBXStage_DQ
        {
            get
            {
                return _expXM_CJBXStage_DQ;
            }
            set
            {
                _expXM_CJBXStage_DQ = value;
                RaisePropertyChanged(() => ExpXM_CJBXStage_DQ);
            }
        }

        /// <summary>
        /// 当前步骤
        /// </summary>
        private MQZH_StepModel_CJBX _expXM_CJBXStep_DQ;
        /// <summary>
        /// 当前步骤
        /// </summary>
        public MQZH_StepModel_CJBX ExpXM_CJBXStep_DQ
        {
            get
            {
                return _expXM_CJBXStep_DQ;
            }
            set
            {
                _expXM_CJBXStep_DQ = value;
                RaisePropertyChanged(() => ExpXM_CJBXStep_DQ);
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


        #region 检测试验阶段组

        /// <summary>
        /// 定级X轴检测试验阶段组
        /// </summary>
        private List<MQZH_StageModel_CJBX> _stageList_DJX;
        /// <summary>
        /// 定级X轴检测试验阶段组
        /// </summary>
        public List<MQZH_StageModel_CJBX> StageList_DJX
        {
            get
            {
                return _stageList_DJX;
            }
            set
            {
                _stageList_DJX = value;
                RaisePropertyChanged(() => StageList_DJX);
            }
        }

        /// <summary>
        /// 定级X轴检测试验阶段组
        /// </summary>
        private List<MQZH_StageModel_CJBX> _stageList_DJY;
        /// <summary>
        /// 定级X轴检测试验阶段组
        /// </summary>
        public List<MQZH_StageModel_CJBX> StageList_DJY
        {
            get
            {
                return _stageList_DJY;
            }
            set
            {
                _stageList_DJY = value;
                RaisePropertyChanged(() => StageList_DJY);
            }
        }

        /// <summary>
        /// 定级Z轴检测试验阶段组
        /// </summary>
        private List<MQZH_StageModel_CJBX> _stageList_DJZ;
        /// <summary>
        /// 定级Z轴检测试验阶段组
        /// </summary>
        public List<MQZH_StageModel_CJBX> StageList_DJZ
        {
            get
            {
                return _stageList_DJZ;
            }
            set
            {
                _stageList_DJZ = value;
                RaisePropertyChanged(() => StageList_DJZ);
            }
        }

        /// <summary>
        /// 工程X轴检测试验阶段组
        /// </summary>
        private List<MQZH_StageModel_CJBX> _stageList_GCX;
        /// <summary>
        /// 工程X轴检测试验阶段组
        /// </summary>
        public List<MQZH_StageModel_CJBX> StageList_GCX
        {
            get
            {
                return _stageList_GCX;
            }
            set
            {
                _stageList_GCX = value;
                RaisePropertyChanged(() => StageList_GCX);
            }
        }

        /// <summary>
        /// 工程Y轴检测试验阶段组
        /// </summary>
        private List<MQZH_StageModel_CJBX> _stageList_GCY;
        /// <summary>
        /// 工程Y轴检测试验阶段组
        /// </summary>
        public List<MQZH_StageModel_CJBX> StageList_GCY
        {
            get
            {
                return _stageList_GCY;
            }
            set
            {
                _stageList_GCY = value;
                RaisePropertyChanged(() => StageList_GCY);
            }
        }

        /// <summary>
        /// 工程Z轴检测试验阶段组
        /// </summary>
        private ObservableCollection<MQZH_StageModel_CJBX> _stageList_GCZ;
        /// <summary>
        /// 工程Z轴检测试验阶段组
        /// </summary>
        public ObservableCollection<MQZH_StageModel_CJBX> StageList_GCZ
        {
            get
            {
                return _stageList_GCZ;
            }
            set
            {
                _stageList_GCZ = value;
                RaisePropertyChanged(() => StageList_GCZ);
            }
        }

        #endregion


        #region 定级阶段初始化

        /// <summary>
        /// 层间变形试验参数初始化。
        ///  </summary>
        /// <remarks>初始化之前，需设定相关参数</remarks>
        public void CJBXExpDJInit()
        {
            //定级各阶段list初始化（各6个阶段）
            StageList_DJX = null;
            StageList_DJX = new List<MQZH_StageModel_CJBX>();
            MQZH_StageModel_CJBX tempStage = new MQZH_StageModel_CJBX();
            StageList_DJX.Add(tempStage);
            tempStage = new MQZH_StageModel_CJBX();
            StageList_DJX.Add(tempStage);
            tempStage = new MQZH_StageModel_CJBX();
            StageList_DJX.Add(tempStage);
            tempStage = new MQZH_StageModel_CJBX();
            StageList_DJX.Add(tempStage);
            tempStage = new MQZH_StageModel_CJBX();
            StageList_DJX.Add(tempStage);
            tempStage = new MQZH_StageModel_CJBX();
            StageList_DJX.Add(tempStage);
            CJBXDJStageX0Init();
            CJBXDJStageX1Init();
            CJBXDJStageX2Init();
            CJBXDJStageX3Init();
            CJBXDJStageX4Init();
            CJBXDJStageX5Init();

            StageList_DJY = null;
            StageList_DJY = new List<MQZH_StageModel_CJBX>();
            tempStage = new MQZH_StageModel_CJBX();
            StageList_DJY.Add(tempStage);
            tempStage = new MQZH_StageModel_CJBX();
            StageList_DJY.Add(tempStage);
            tempStage = new MQZH_StageModel_CJBX();
            StageList_DJY.Add(tempStage);
            tempStage = new MQZH_StageModel_CJBX();
            StageList_DJY.Add(tempStage);
            tempStage = new MQZH_StageModel_CJBX();
            StageList_DJY.Add(tempStage);
            tempStage = new MQZH_StageModel_CJBX();
            StageList_DJY.Add(tempStage);
            CJBXDJStageY0Init();
            CJBXDJStageY1Init();
            CJBXDJStageY2Init();
            CJBXDJStageY3Init();
            CJBXDJStageY4Init();
            CJBXDJStageY5Init();

            StageList_DJZ = null;
            StageList_DJZ = new List<MQZH_StageModel_CJBX>();
            tempStage = new MQZH_StageModel_CJBX();
            StageList_DJZ.Add(tempStage);
            tempStage = new MQZH_StageModel_CJBX();
            StageList_DJZ.Add(tempStage);
            tempStage = new MQZH_StageModel_CJBX();
            StageList_DJZ.Add(tempStage);
            tempStage = new MQZH_StageModel_CJBX();
            StageList_DJZ.Add(tempStage);
            tempStage = new MQZH_StageModel_CJBX();
            StageList_DJZ.Add(tempStage);
            tempStage = new MQZH_StageModel_CJBX();
            StageList_DJZ.Add(tempStage);
            CJBXDJStageZ0Init();
            CJBXDJStageZ1Init();
            CJBXDJStageZ2Init();
            CJBXDJStageZ3Init();
            CJBXDJStageZ4Init();
            CJBXDJStageZ5Init();

            //可选属性初始化
            CanBeCheckInit();
        }


        /// <summary>
        /// 层间变形定级试验X轴预加载初始化
        /// </summary>
        public void CJBXDJStageX0Init()
        {
            MQZH_StageModel_CJBX tempStage = new MQZH_StageModel_CJBX();
            MQZH_StepModel_CJBX newStep = new MQZH_StepModel_CJBX();

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_DJ_X0.ToString(),
                StageName = "层间变形定级X轴预加载",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "请将试件可开启部分开关五次，并最终锁紧！",
                StepList = new ObservableCollection<MQZH_StepModel_CJBX>()
            };

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 1,
                StepName = "预加载正位移",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 2,
                StepName = "预加载正位移归零",
                IsStepCompleted = false,
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 3,
                StepName = "预加载负位移",
                IsStepCompleted = false,
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 4,
                StepName = "预加载负位移归零",
                IsStepCompleted = false,
            };
            tempStage.StepList.Add(newStep);

            StageList_DJX[0] = tempStage;
            RaisePropertyChanged(() => StageList_DJX);
        }

        /// <summary>
        /// 层间变形定级试验X轴第1级初始化
        /// </summary>
        public void CJBXDJStageX1Init()
        {
            MQZH_StageModel_CJBX tempStage = new MQZH_StageModel_CJBX();
            MQZH_StepModel_CJBX newStep = new MQZH_StepModel_CJBX();

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_DJ_X1.ToString(),
                StageName = "层间变形定级X轴第1级加载",
                CompleteStatus = false,
                StepList = new ObservableCollection<MQZH_StepModel_CJBX>()
            };

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 1 + 4 * i,
                    StepName = "第" + j + "周期正位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 2 + 4 * i,
                    StepName = "第" + j + "周期正位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 3 + 4 * i,
                    StepName = "第" + j + "周期负位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 4 + 4 * i,
                    StepName = "第" + j + "周期负位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].IsWaitBeforCompleted = false;
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_DJX[1] = tempStage;
            RaisePropertyChanged(() => StageList_DJX);
        }

        /// <summary>
        /// 层间变形定级试验X轴第2级初始化
        /// </summary>
        public void CJBXDJStageX2Init()
        {
            MQZH_StageModel_CJBX tempStage = new MQZH_StageModel_CJBX();
            MQZH_StepModel_CJBX newStep = new MQZH_StepModel_CJBX();

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_DJ_X2.ToString(),
                StageName = "层间变形定级X轴第2级加载",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "请将试件可开启部分开关五次，并最终锁紧！",
                StepList = new ObservableCollection<MQZH_StepModel_CJBX>()
            };

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 1 + 4 * i,
                    StepName = "第" + j + "周期正位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 2 + 4 * i,
                    StepName = "第" + j + "周期正位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 3 + 4 * i,
                    StepName = "第" + j + "周期负位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 4 + 4 * i,
                    StepName = "第" + j + "周期负位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].IsWaitBeforCompleted = false;
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_DJX[2] = tempStage;
            RaisePropertyChanged(() => StageList_DJX);
        }

        /// <summary>
        /// 层间变形定级试验X轴第3级初始化
        /// </summary>
        public void CJBXDJStageX3Init()
        {
            MQZH_StageModel_CJBX tempStage = new MQZH_StageModel_CJBX();
            MQZH_StepModel_CJBX newStep = new MQZH_StepModel_CJBX();

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_DJ_X3.ToString(),
                StageName = "层间变形定级X轴第3级加载",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "请将试件可开启部分开关五次，并最终锁紧！",
                StepList = new ObservableCollection<MQZH_StepModel_CJBX>()
            };

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 1 + 4 * i,
                    StepName = "第" + j + "周期正位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 2 + 4 * i,
                    StepName = "第" + j + "周期正位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 3 + 4 * i,
                    StepName = "第" + j + "周期负位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 4 + 4 * i,
                    StepName = "第" + j + "周期负位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].IsWaitBeforCompleted = false;
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_DJX[3] = tempStage;
            RaisePropertyChanged(() => StageList_DJX);
        }

        /// <summary>
        /// 层间变形定级试验X轴第4级初始化
        /// </summary>
        public void CJBXDJStageX4Init()
        {
            MQZH_StageModel_CJBX tempStage = new MQZH_StageModel_CJBX();
            MQZH_StepModel_CJBX newStep = new MQZH_StepModel_CJBX();

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_DJ_X4.ToString(),
                StageName = "层间变形定级X轴第4级加载",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "请将试件可开启部分开关五次，并最终锁紧！",
                StepList = new ObservableCollection<MQZH_StepModel_CJBX>()
            };

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 1 + 4 * i,
                    StepName = "第" + j + "周期正位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 2 + 4 * i,
                    StepName = "第" + j + "周期正位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 3 + 4 * i,
                    StepName = "第" + j + "周期负位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 4 + 4 * i,
                    StepName = "第" + j + "周期负位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].IsWaitBeforCompleted = false;
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_DJX[4] = tempStage;
            RaisePropertyChanged(() => StageList_DJX);
        }

        /// <summary>
        /// 层间变形定级试验X轴第5级初始化
        /// </summary>
        public void CJBXDJStageX5Init()
        {
            MQZH_StageModel_CJBX tempStage = new MQZH_StageModel_CJBX();
            MQZH_StepModel_CJBX newStep = new MQZH_StepModel_CJBX();

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_DJ_X5.ToString(),
                StageName = "层间变形定级X轴第5级加载",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "请将试件可开启部分开关五次，并最终锁紧！",
                StepList = new ObservableCollection<MQZH_StepModel_CJBX>()
            };

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 1 + 4 * i,
                    StepName = "第" + j + "周期正位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 2 + 4 * i,
                    StepName = "第" + j + "周期正位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 3 + 4 * i,
                    StepName = "第" + j + "周期负位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 4 + 4 * i,
                    StepName = "第" + j + "周期负位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].IsWaitBeforCompleted = false;
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_DJX[5] = tempStage;
            RaisePropertyChanged(() => StageList_DJX);
        }

        /// <summary>
        /// 层间变形定级试验Y轴预加载阶段初始化
        /// </summary>
        public void CJBXDJStageY0Init()
        {
            MQZH_StepModel_CJBX newStep ;

            MQZH_StageModel_CJBX tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_DJ_Y0.ToString(),
                StageName = "层间变形定级Y轴预加载",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "请将试件可开启部分开关五次，并最终锁紧！",
                StepList = new ObservableCollection<MQZH_StepModel_CJBX>()
            };

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 1,
                StepName = "预加载第1周期正位移",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 2,
                StepName = "预加载第1周期正位移归零",
                IsStepCompleted = false,
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 3,
                StepName = "预加载第1周期负位移",
                IsStepCompleted = false,
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 4,
                StepName = "预加载第1周期负位移归零",
                IsStepCompleted = false,
            };
            tempStage.StepList.Add(newStep);

            StageList_DJY[0] = tempStage;
        }

        /// <summary>
        /// 层间变形定级试验Y轴第1级阶段初始化
        /// </summary>
        public void CJBXDJStageY1Init()
        {
            MQZH_StageModel_CJBX tempStage ;
            MQZH_StepModel_CJBX newStep ;

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_DJ_Y1.ToString(),
                StageName = "层间变形定级Y轴第1级加载",
                CompleteStatus = false,
                StepList = new ObservableCollection<MQZH_StepModel_CJBX>()
            };

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 1 + 4 * i,
                    StepName = "第" + j + "周期正位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 2 + 4 * i,
                    StepName = "第" + j + "周期正位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 3 + 4 * i,
                    StepName = "第" + j + "周期负位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 4 + 4 * i,
                    StepName = "第" + j + "周期负位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].IsWaitBeforCompleted = false;
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_DJY[1] = tempStage;
        }

        /// <summary>
        /// 层间变形定级试验Y轴第2级阶段初始化
        /// </summary>
        public void CJBXDJStageY2Init()
        {
            MQZH_StageModel_CJBX tempStage;
            MQZH_StepModel_CJBX newStep;

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_DJ_Y2.ToString(),
                StageName = "层间变形定级Y轴第2级加载",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "请将试件可开启部分开关五次，并最终锁紧！",
                StepList = new ObservableCollection<MQZH_StepModel_CJBX>()
            };

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 1 + 4 * i,
                    StepName = "第" + j + "周期正位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 2 + 4 * i,
                    StepName = "第" + j + "周期正位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 3 + 4 * i,
                    StepName = "第" + j + "周期负位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 4 + 4 * i,
                    StepName = "第" + j + "周期负位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].IsWaitBeforCompleted = false;
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_DJY[2] = tempStage;
        }

        /// <summary>
        /// 层间变形定级试验Y轴第3级阶段初始化
        /// </summary>
        public void CJBXDJStageY3Init()
        {
            MQZH_StageModel_CJBX tempStage;
            MQZH_StepModel_CJBX newStep;

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_DJ_Y3.ToString(),
                StageName = "层间变形定级Y轴第3级加载",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "请将试件可开启部分开关五次，并最终锁紧！",
                StepList = new ObservableCollection<MQZH_StepModel_CJBX>()
            };

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 1 + 4 * i,
                    StepName = "第" + j + "周期正位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 2 + 4 * i,
                    StepName = "第" + j + "周期正位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 3 + 4 * i,
                    StepName = "第" + j + "周期负位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 4 + 4 * i,
                    StepName = "第" + j + "周期负位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].IsWaitBeforCompleted = false;
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_DJY[3] = tempStage;
        }

        /// <summary>
        /// 层间变形定级试验Y轴第4级阶段初始化
        /// </summary>
        public void CJBXDJStageY4Init()
        {
            MQZH_StageModel_CJBX tempStage;
            MQZH_StepModel_CJBX newStep;

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_DJ_Y4.ToString(),
                StageName = "层间变形定级Y轴第4级加载",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "请将试件可开启部分开关五次，并最终锁紧！",
                StepList = new ObservableCollection<MQZH_StepModel_CJBX>()
            };

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 1 + 4 * i,
                    StepName = "第" + j + "周期正位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 2 + 4 * i,
                    StepName = "第" + j + "周期正位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 3 + 4 * i,
                    StepName = "第" + j + "周期负位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 4 + 4 * i,
                    StepName = "第" + j + "周期负位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].IsWaitBeforCompleted = false;
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_DJY[4] = tempStage;
        }

        /// <summary>
        /// 层间变形定级试验Y轴第5级阶段初始化
        /// </summary>
        public void CJBXDJStageY5Init()
        {
            MQZH_StageModel_CJBX tempStage;
            MQZH_StepModel_CJBX newStep;

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_DJ_Y5.ToString(),
                StageName = "层间变形定级Y轴第5级加载",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "请将试件可开启部分开关五次，并最终锁紧！",
                StepList = new ObservableCollection<MQZH_StepModel_CJBX>()
            };

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 1 + 4 * i,
                    StepName = "第" + j + "周期正位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 2 + 4 * i,
                    StepName = "第" + j + "周期正位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 3 + 4 * i,
                    StepName = "第" + j + "周期负位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 4 + 4 * i,
                    StepName = "第" + j + "周期负位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].IsWaitBeforCompleted = false;
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_DJY[5] = tempStage;
        }

        /// <summary>
        /// 层间变形定级试验Z轴预加载初始化
        /// </summary>
        public void CJBXDJStageZ0Init()
        {
            MQZH_StageModel_CJBX tempStage;
            MQZH_StepModel_CJBX newStep;

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_DJ_Z0.ToString(),
                StageName = "层间变形定级Z轴预加载",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "请将试件可开启部分开关五次，并最终锁紧！",
                StepList = new ObservableCollection<MQZH_StepModel_CJBX>()
            };
            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 1,
                StepName = "预加载第1周期正位移",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 2,
                StepName = "预加载第1周期正位移归零",
                IsStepCompleted = false,
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 3,
                StepName = "预加载第1周期负位移",
                IsStepCompleted = false,
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 4,
                StepName = "预加载第1周期负位移归零",
                IsStepCompleted = false,
            };
            tempStage.StepList.Add(newStep);
            StageList_DJZ[0] = tempStage;
        }

        /// <summary>
        /// 层间变形定级试验Z轴第1级阶段初始化
        /// </summary>
        public void CJBXDJStageZ1Init()
        {
            MQZH_StageModel_CJBX tempStage;
            MQZH_StepModel_CJBX newStep;

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_DJ_Z1.ToString(),
                StageName = "层间变形定级Z轴第1级加载",
                CompleteStatus = false,
                StepList = new ObservableCollection<MQZH_StepModel_CJBX>()
            };

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 1 + 4 * i,
                    StepName = "第" + j + "周期正位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 2 + 4 * i,
                    StepName = "第" + j + "周期正位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 3 + 4 * i,
                    StepName = "第" + j + "周期负位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 4 + 4 * i,
                    StepName = "第" + j + "周期负位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].IsWaitBeforCompleted = false;
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_DJZ[1] = tempStage;
        }

        /// <summary>
        /// 层间变形定级试验Z轴第2级阶段初始化
        /// </summary>
        public void CJBXDJStageZ2Init()
        {
            MQZH_StageModel_CJBX tempStage;
            MQZH_StepModel_CJBX newStep;

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_DJ_Z2.ToString(),
                StageName = "层间变形定级Z轴第2级加载",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "请将试件可开启部分开关五次，并最终锁紧！",
                StepList = new ObservableCollection<MQZH_StepModel_CJBX>()
            };

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 1 + 4 * i,
                    StepName = "第" + j + "周期正位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 2 + 4 * i,
                    StepName = "第" + j + "周期正位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 3 + 4 * i,
                    StepName = "第" + j + "周期负位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 4 + 4 * i,
                    StepName = "第" + j + "周期负位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].IsWaitBeforCompleted = false;
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_DJZ[2] = tempStage;
        }

        /// <summary>
        /// 层间变形定级试验Z轴第3级阶段初始化
        /// </summary>
        public void CJBXDJStageZ3Init()
        {
            MQZH_StageModel_CJBX tempStage;
            MQZH_StepModel_CJBX newStep;

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_DJ_Z3.ToString(),
                StageName = "层间变形定级Z轴第3级加载",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "请将试件可开启部分开关五次，并最终锁紧！",
                StepList = new ObservableCollection<MQZH_StepModel_CJBX>()
            };

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 1 + 4 * i,
                    StepName = "第" + j + "周期正位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 2 + 4 * i,
                    StepName = "第" + j + "周期正位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 3 + 4 * i,
                    StepName = "第" + j + "周期负位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 4 + 4 * i,
                    StepName = "第" + j + "周期负位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].IsWaitBeforCompleted = false;
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_DJZ[3] = tempStage;
        }

        /// <summary>
        /// 层间变形定级试验Z轴第4级阶段初始化
        /// </summary>
        public void CJBXDJStageZ4Init()
        {
            MQZH_StageModel_CJBX tempStage;
            MQZH_StepModel_CJBX newStep;

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_DJ_Z4.ToString(),
                StageName = "层间变形定级Z轴第4级加载",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "请将试件可开启部分开关五次，并最终锁紧！",
                StepList = new ObservableCollection<MQZH_StepModel_CJBX>()
            };

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 1 + 4 * i,
                    StepName = "第" + j + "周期正位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 2 + 4 * i,
                    StepName = "第" + j + "周期正位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 3 + 4 * i,
                    StepName = "第" + j + "周期负位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 4 + 4 * i,
                    StepName = "第" + j + "周期负位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].IsWaitBeforCompleted = false;
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_DJZ[4] = tempStage;
        }

        /// <summary>
        /// 层间变形定级试验Z轴第3级阶段初始化
        /// </summary>
        public void CJBXDJStageZ5Init()
        {
            MQZH_StageModel_CJBX tempStage;
            MQZH_StepModel_CJBX newStep;

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_DJ_Z5.ToString(),
                StageName = "层间变形定级Z轴第5级加载",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "请将试件可开启部分开关五次，并最终锁紧！",
                StepList = new ObservableCollection<MQZH_StepModel_CJBX>()
            };

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 1+4*i,
                    StepName = "第" + j + "周期正位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 2+4 * i,
                    StepName = "第" + j + "周期正位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 3 + 4 * i,
                    StepName = "第" + j + "周期负位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 4 + 4 * i,
                    StepName = "第" + j + "周期负位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].IsWaitBeforCompleted = false;
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_DJZ[5] = tempStage;
        }

        #endregion


        #region 工程阶段初始化

        /// <summary>
        /// 层间变形试验参数初始化。
        ///  </summary>
        /// <remarks>初始化之前，需设定相关参数</remarks>
        public void CJBXExpGCInit()
        {
            //工程各阶段list初始化（）
            StageList_GCX = null;
            StageList_GCX = new List<MQZH_StageModel_CJBX>();
            MQZH_StageModel_CJBX tempStage = new MQZH_StageModel_CJBX();
            StageList_GCX.Add(tempStage);
            tempStage = new MQZH_StageModel_CJBX();
            StageList_GCX.Add(tempStage);

            StageList_GCY = null;
            StageList_GCY = new List<MQZH_StageModel_CJBX>();
            tempStage = new MQZH_StageModel_CJBX();
            StageList_GCY.Add(tempStage);
            tempStage = new MQZH_StageModel_CJBX();
            StageList_GCY.Add(tempStage);

            StageList_GCZ = null;
            StageList_GCZ = new ObservableCollection<MQZH_StageModel_CJBX>();
            tempStage = new MQZH_StageModel_CJBX();
            StageList_GCZ.Add(tempStage);
            tempStage = new MQZH_StageModel_CJBX();
            StageList_GCZ.Add(tempStage);

            CJBXGCStageX0Init();
            CJBXGCStageX1Init();
            CJBXGCStageY0Init();
            CJBXGCStageY1Init();
            CJBXGCStageZ0Init();
            CJBXGCStageZ1Init();

            //可选属性初始化
            CanBeCheckInit();
        }

        /// <summary>
        /// 层间变形工程X预加载阶段初始化
        /// </summary>
        public void CJBXGCStageX0Init()
        {
            MQZH_StageModel_CJBX tempStage;
            MQZH_StepModel_CJBX newStep;

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_GC_X0.ToString(),
                StageName = "层间变形工程X轴预加载",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "请将试件可开启部分开关五次，并最终锁紧！",
            };
            tempStage.StepList = new ObservableCollection<MQZH_StepModel_CJBX>();

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 1,
                StepName = "预加载第1周期正位移",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 2,
                StepName = "预加载第1周期正位移归零",
                IsStepCompleted = false,
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 3,
                StepName = "预加载第1周期负位移",
                IsStepCompleted = false,
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 4,
                StepName = "预加载第1周期负位移归零",
                IsStepCompleted = false,
            };
            tempStage.StepList.Add(newStep);

            StageList_GCX[0] = tempStage;
        }

        /// <summary>
        /// 层间变形工程X检测加载阶段初始化
        /// </summary>
        public void CJBXGCStageX1Init()
        {
            MQZH_StageModel_CJBX tempStage;
            MQZH_StepModel_CJBX newStep;

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_GC_X1.ToString(),
                StageName = "层间变形工程X轴检测加载",
                CompleteStatus = false,
            };
            tempStage.StepList = new ObservableCollection<MQZH_StepModel_CJBX>();

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 1 + 4 * i,
                    StepName = "检测加载第" + j + "周期正位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 2 + 4 * i,
                    StepName = "检测加载第" + j + "周期正位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 3 + 4 * i,
                    StepName = "检测加载第" + j + "周期负位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 4 + 4 * i,
                    StepName = "检测加载第" + j + "周期负位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].IsWaitBeforCompleted = false;
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_GCX[1] = tempStage;
        }


        /// <summary>
        /// 层间变形工程Y预加载阶段初始化
        /// </summary>
        public void CJBXGCStageY0Init()
        {
            MQZH_StageModel_CJBX tempStage;
            MQZH_StepModel_CJBX newStep;

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_GC_Y0.ToString(),
                StageName = "层间变形工程Y轴预加载",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "请将试件可开启部分开关五次，并最终锁紧！",
            };
            tempStage.StepList = new ObservableCollection<MQZH_StepModel_CJBX>();

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 1,
                StepName = "预加载第1周期正位移",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 2,
                StepName = "预加载第1周期正位移归零",
                IsStepCompleted = false,
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 3,
                StepName = "预加载第1周期负位移",
                IsStepCompleted = false,
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 4,
                StepName = "预加载第1周期负位移归零",
                IsStepCompleted = false,
            };
            tempStage.StepList.Add(newStep);

            StageList_GCY[0] = tempStage;
        }

        /// <summary>
        /// 层间变形工程Y检测加载阶段初始化
        /// </summary>
        public void CJBXGCStageY1Init()
        {
            MQZH_StageModel_CJBX tempStage;
            MQZH_StepModel_CJBX newStep;

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_GC_Y1.ToString(),
                StageName = "层间变形工程Y轴检测加载",
                CompleteStatus = false,
            };
            tempStage.StepList = new ObservableCollection<MQZH_StepModel_CJBX>();

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 1 + 4 * i,
                    StepName = "检测加载第" + j + "周期正位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 2 + 4 * i,
                    StepName = "检测加载第" + j + "周期正位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 3 + 4 * i,
                    StepName = "检测加载第" + j + "周期负位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 4 + 4 * i,
                    StepName = "检测加载第" + j + "周期负位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].IsWaitBeforCompleted = false;
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_GCY[1] = tempStage;
        }

        /// <summary>
        /// 层间变形工程Z预加载阶段初始化
        /// </summary>
        public void CJBXGCStageZ0Init()
        {
            MQZH_StageModel_CJBX tempStage;
            MQZH_StepModel_CJBX newStep;

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_GC_Z0.ToString(),
                StageName = "层间变形工程Z轴预加载",
                CompleteStatus = false,
                IsNeedTipsBefore = true,
                StringTipsBefore = "请将试件可开启部分开关五次，并最终锁紧！",
            };
            tempStage.StepList = new ObservableCollection<MQZH_StepModel_CJBX>();

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 1,
                StepName = "预加载第1周期正位移",
                IsStepCompleted = false,
                IsWaitBeforCompleted = false,
                TimeWaitBefor = 10
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 2,
                StepName = "预加载第1周期正位移归零",
                IsStepCompleted = false,
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 3,
                StepName = "预加载第1周期负位移",
                IsStepCompleted = false,
            };
            tempStage.StepList.Add(newStep);

            newStep = new MQZH_StepModel_CJBX
            {
                StepNO = 4,
                StepName = "预加载第1周期负位移归零",
                IsStepCompleted = false,
            };
            tempStage.StepList.Add(newStep);

            StageList_GCZ[0] = tempStage;
        }

        /// <summary>
        /// 层间变形工程Z检测加载阶段初始化
        /// </summary>
        public void CJBXGCStageZ1Init()
        {
            MQZH_StageModel_CJBX tempStage;
            MQZH_StepModel_CJBX newStep;

            tempStage = new MQZH_StageModel_CJBX
            {
                StageNO = TestStageType.CJBX_GC_Z1.ToString(),
                StageName = "层间变形工程Z轴检测加载",
                CompleteStatus = false,
            };
            tempStage.StepList = new ObservableCollection<MQZH_StepModel_CJBX>();

            for (int i = 0; i < 3; i++)
            {
                int j = i + 1;
                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 1+4*i,
                    StepName = "检测加载第" + j + "周期正位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 2 + 4 * i,
                    StepName = "检测加载第" + j + "周期正位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 3 + 4 * i,
                    StepName = "检测加载第" + j + "周期负位移",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);

                newStep = new MQZH_StepModel_CJBX
                {
                    StepNO = 4 + 4 * i,
                    StepName = "检测加载第" + j + "周期负位移归零",
                    IsStepCompleted = false,
                };
                tempStage.StepList.Add(newStep);
            }
            tempStage.StepList[0].IsWaitBeforCompleted = false;
            tempStage.StepList[0].TimeWaitBefor = 10;

            StageList_GCZ[1] = tempStage;
        }

        #endregion


        #region 页面、阶段可选属性初始化

        /// <summary>
        /// 页面、阶段可选属性初始化
        /// </summary>
        public void CanBeCheckInit()
        {
            //定级被选集合
            BeenCheckedDJX = new ObservableCollection<bool>()
            {
                false,false,false,false,false,false
            };
            BeenCheckedDJY = new ObservableCollection<bool>()
            {
                false,false,false,false,false,false
            };
            BeenCheckedDJZ = new ObservableCollection<bool>()
            {
                false,false,false,false,false,false
            };
            //工程被选集合
            BeenCheckedGCX = new ObservableCollection<bool>()
            {
                false,false
            };
            BeenCheckedGCY = new ObservableCollection<bool>()
            {
                false,false
            };
            BeenCheckedGCZ = new ObservableCollection<bool>()
            {
                false,false
            };
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
            for (int i = 0; i < StageList_DJX.Count; i++)
            {
                if (IsGC)
                    StageList_DJX[i].NeedTest = false;
                else
                    StageList_DJX[i].NeedTest = true;
            }
            for (int i = 0; i < StageList_DJY.Count; i++)
            {
                if (IsGC)
                    StageList_DJY[i].NeedTest = false;
                else
                    StageList_DJY[i].NeedTest = true;
            }
            for (int i = 0; i < StageList_DJZ.Count; i++)
            {
                if (IsGC)
                    StageList_DJZ[i].NeedTest = false;
                else
                    StageList_DJZ[i].NeedTest = true;
            }
            //工程
            for (int i = 0; i < StageList_GCX.Count; i++)
            {
                if (!IsGC)
                    StageList_GCX[i].NeedTest = false;
                else
                    StageList_GCX[i].NeedTest = true;
            }
            for (int i = 0; i < StageList_GCY.Count; i++)
            {
                if (!IsGC)
                    StageList_GCY[i].NeedTest = false;
                else
                    StageList_GCY[i].NeedTest = true;
            }
            for (int i = 0; i < StageList_GCZ.Count; i++)
            {
                if (!IsGC)
                    StageList_GCZ[i].NeedTest = false;
                else
                    StageList_GCZ[i].NeedTest = true;
            }
        }

        #endregion

        #region 当前阶段、步骤初始化

        /// <summary>
        /// 当前阶段、步骤初始化
        /// </summary>
        public void StageStepDQReset()
        {
            ExpXM_CJBXStage_DQ = new MQZH_StageModel_CJBX();
            ExpXM_CJBXStep_DQ = new MQZH_StepModel_CJBX();
        }

        #endregion
    }
}