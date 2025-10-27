/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQDFJ_MB.Model.Exp
 * 文件名：  MQZH_StepModel_QSM
 * 版本号：  V1.0.0.0
 * 唯一标识：41e4c86b-1c10-4747-a54b-ff590b615714
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/11/16 23:13:47
 * 描述：
 * 参数控制各步骤参数Model。
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2021/11/16       23:13:47		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using GalaSoft.MvvmLight;

namespace MQDFJ_MB.Model.Exp
{
    /// <summary>
    /// 幕墙四性步骤参数类
    /// </summary>
    public class MQZH_StepModel_QSM : ObservableObject
    {

        #region 步骤设定参数属性

        /// <summary>
        /// 步骤编号
        /// </summary>
        private int _stepNO = -1;
        /// <summary>
        /// 步骤编号
        /// </summary>
        public int StepNO
        {
            get { return _stepNO; }
            set
            {
                _stepNO = value;
                RaisePropertyChanged(() => StepNO);
            }
        }

        /// <summary>
        /// 步骤名称
        /// </summary>
        private string _stepName = "等待操作";
        /// <summary>
        /// 步骤名称
        /// </summary>
        public string StepName
        {
            get { return _stepName; }
            set
            {
                _stepName = value;
                RaisePropertyChanged(() => StepName);
            }
        }

        /// <summary>
        /// 加载类型，0为稳定载荷，1为波动载荷
        /// </summary>
        private bool _waveType = false;
        /// <summary>
        /// 加载类型，0为稳定载荷，1为线波动载荷
        /// </summary>
        public bool WaveType
        {
            get { return _waveType; }
            set
            {
                _waveType = value;
                RaisePropertyChanged(() => WaveType);
            }
        }

        /// <summary>
        /// 稳定载荷目标值。
        /// </summary>
        private double _steadyLoadAimValue = 0.0;
        /// <summary>
        /// 稳定载荷目标值。
        /// </summary>
        public double SteadyLoadAimValue
        {
            get { return _steadyLoadAimValue; }
            set
            {
                _steadyLoadAimValue = value;
                RaisePropertyChanged(() => SteadyLoadAimValue);
            }
        }

        /// <summary>
        /// 波动载荷均值。
        /// </summary>
        private double _waveAimAverageValue = 0.0;
        /// <summary>
        /// 波动载荷均值。
        /// </summary>
        public double WaveAimAverageValue
        {
            get { return _waveAimAverageValue; }
            set
            {
                _waveAimAverageValue = value;
                RaisePropertyChanged(() => WaveAimAverageValue);
            }
        }

        /// <summary>
        /// 波动载荷高值（绝对值高）
        /// </summary>
        private double _waveAimHBound = 0;
        /// <summary>
        /// 波动载荷高值（绝对值高）
        /// </summary>
        public double WaveAimHBound
        {
            get { return _waveAimHBound; }
            set
            {
                _waveAimHBound = value;
                RaisePropertyChanged(() => WaveAimHBound);
            }
        }

        /// <summary>
        /// 波动载荷低值（绝对值低）。
        /// </summary>
        private double _waveAimLBound = 0.0;
        /// <summary>
        /// 波动载荷低值（绝对值低）。
        /// </summary>
        public double WaveAimLBound
        {
            get { return _waveAimLBound; }
            set
            {
                _waveAimLBound = value;
                RaisePropertyChanged(() => WaveAimLBound);
            }
        }

        /// <summary>
        /// 开始前等待时长
        /// </summary>
        private int _timeWaitBefor = 0;
        /// <summary>
        /// 开始前等待时长
        /// </summary>
        public int TimeWaitBefor
        {
            get { return _timeWaitBefor; }
            set
            {
                _timeWaitBefor = value;
                RaisePropertyChanged(() => TimeWaitBefor);
            }
        }

        #endregion


        #region 步骤运行参数属性

        /// <summary>
        /// 步骤需要做试验（true为需要，false为不需要）
        /// </summary>
        private bool _stepNeedTest = true;
        /// <summary>
        /// 步骤需要做试验（true为需要，false为不需要）
        /// </summary>
        public bool StepNeedTest
        {
            get { return _stepNeedTest; }
            set
            {
                _stepNeedTest = value;
                RaisePropertyChanged(() => StepNeedTest);
            }
        }

        /// <summary>
        /// 步骤完成标志
        /// </summary>
        private bool _isStepCompleted = false;
        /// <summary>
        /// 加步骤完成标志
        /// </summary>
        public bool IsStepCompleted
        {
            get { return _isStepCompleted; }
            set
            {
                _isStepCompleted = value;
                RaisePropertyChanged(() => IsStepCompleted);
            }
        }

        /// <summary>
        /// 步骤前等待开始标志
        /// </summary>
        private bool _isWaitStarted = false;
        /// <summary>
        /// 步骤前等待开始标志
        /// </summary>
        public bool IsWaitStarted
        {
            get { return _isWaitStarted; }
            set
            {
                _isWaitStarted = value;
                RaisePropertyChanged(() => IsWaitStarted);
            }
        }

        /// <summary>
        /// 步骤前等待完成标志
        /// </summary>
        private bool _isWaitBeforCompleted = false;
        /// <summary>
        /// 步骤前等待完成标志
        /// </summary>
        public bool IsWaitBeforCompleted
        {
            get { return _isWaitBeforCompleted; }
            set
            {
                _isWaitBeforCompleted = value;
                RaisePropertyChanged(() => IsWaitBeforCompleted);
            }
        }

        /// <summary>
        /// 步骤前等待开始时间
        /// </summary>
        private DateTime _waitStartTime = DateTime.Now;
        /// <summary>
        /// 步骤前等待开始时间
        /// </summary>
        public DateTime WaitStartTime
        {
            get { return _waitStartTime; }
            set
            {
                _waitStartTime = value;
                RaisePropertyChanged(() => WaitStartTime);
            }
        }

        /// <summary>
        /// 步骤检测漏风量
        /// </summary>
        private double _stepResultSTL = 0;
        /// <summary>
        /// 步骤检测漏风量
        /// </summary>
        public double StepResultSTL
        {
            get { return _stepResultSTL; }
            set
            {
                _stepResultSTL = value;
                RaisePropertyChanged(() => StepResultSTL);
            }
        }

        /// <summary>
        /// 稳定压力加载保持状态
        /// </summary>
        private StepLoadStatus _stepSteadyPStatus = new StepLoadStatus();
        /// <summary>
        /// 稳定压力加载保持状态
        /// </summary>
        public StepLoadStatus StepSteadyPStatus
        {
            get { return _stepSteadyPStatus; }
            set
            {
                _stepSteadyPStatus = value;
                RaisePropertyChanged(() => StepSteadyPStatus);
            }
        }

        /// <summary>
        /// 波动压力波峰加载保持状态
        /// </summary>
        private StepLoadStatus _stepWavePUpStatus = new StepLoadStatus();
        /// <summary>
        /// 波动压力波峰加载保持状态
        /// </summary>
        public StepLoadStatus StepWavePUpStatus
        {
            get { return _stepWavePUpStatus; }
            set
            {
                _stepWavePUpStatus = value;
                RaisePropertyChanged(() => StepWavePUpStatus);
            }
        }

        /// <summary>
        /// 波动压力波谷加载保持状态
        /// </summary>
        private StepLoadStatus _stepWavePLowStatus = new StepLoadStatus();
        /// <summary>
        /// 波动压力波谷加载保持状态
        /// </summary>
        public StepLoadStatus StepWavePLowStatus
        {
            get { return _stepWavePLowStatus; }
            set
            {
                _stepWavePLowStatus = value;
                RaisePropertyChanged(() => StepWavePLowStatus);
            }
        }

        /// <summary>
        /// 波动总过程开始时间
        /// </summary>
        private DateTime _waveStartTime = DateTime.Now;
        /// <summary>
        /// 波动总过程开始时间
        /// </summary>
        public DateTime WaveStartTime
        {
            get { return _waveStartTime; }
            set
            {
                _waveStartTime = value;
                RaisePropertyChanged(() => WaveStartTime);
            }
        }

        /// <summary>
        /// 波动总过程已持续时间
        /// </summary>
        private double _waveTimePased = 0;
        /// <summary>
        /// 波动总过程已持续时间
        /// </summary>
        public double WaveTimePased
        {
            get { return _waveTimePased; }
            set
            {
                _waveTimePased = value;
                RaisePropertyChanged(() => WaveTimePased);
            }
        }

        /// <summary>
        /// 波动开始标志
        /// </summary>
        private bool _isWaveStarted = false;
        /// <summary>
        /// 波动开始标志
        /// </summary>
        public bool IsWaveStarted
        {
            get { return _isWaveStarted; }
            set
            {
                _isWaveStarted = value;
                RaisePropertyChanged(() => IsWaveStarted);
            }
        }

        /// <summary>
        /// 波动结束标志
        /// </summary>
        private bool _isWaveCompleted = false;
        /// <summary>
        /// 波动结束标志
        /// </summary>
        public bool IsWaveCompleted
        {
            get { return _isWaveCompleted; }
            set
            {
                _isWaveCompleted = value;
                RaisePropertyChanged(() => IsWaveCompleted);
            }
        }

        #endregion

    }

    /// <summary>
    /// 步骤压力加载保持等相关状态
    /// </summary>
    public class StepLoadStatus : ObservableObject
    {
        /// <summary>
        /// 加载前等待开始标志
        /// </summary>
        private bool _isLoadWaitStart = false;
        /// <summary>
        /// 加载前等待开始标志
        /// </summary>
        public bool IsLoadWaitStart
        {
            get { return _isLoadWaitStart; }
            set
            {
                _isLoadWaitStart = value;
                RaisePropertyChanged(() => IsLoadWaitStart);
            }
        }

        /// <summary>
        /// 加载前等待完成标志
        /// </summary>
        private bool _isLoadWaitComplete = false;
        /// <summary>
        /// 加载前等待完成标志
        /// </summary>
        public bool IsLoadWaitComplete
        {
            get { return _isLoadWaitComplete; }
            set
            {
                _isLoadWaitComplete = value;
                RaisePropertyChanged(() => IsLoadWaitComplete);
            }
        }

        /// <summary>
        /// 加载前等待开始时间
        /// </summary>
        private DateTime _loadWaitStartTime = DateTime.Now;
        /// <summary>
        /// 加载前等待开始时间
        /// </summary>
        public DateTime LoadWaitStartTime
        {
            get { return _loadWaitStartTime; }
            set
            {
                _loadWaitStartTime = value;
                RaisePropertyChanged(() => LoadWaitStartTime);
            }
        }

        /// <summary>
        /// 加载上升起始值。
        /// </summary>
        private double _loadUpStartValue = 0.0;
        /// <summary>
        /// 加载上升起始值。
        /// </summary>
        public double LoadUpStartValue
        {
            get { return _loadUpStartValue; }
            set
            {
                _loadUpStartValue = value;
                RaisePropertyChanged(() => LoadUpStartValue);
            }
        }

        /// <summary>
        /// 加载开始标志
        /// </summary>
        private bool _isUpStarted = false;
        /// <summary>
        /// 加载开始标志
        /// </summary>
        public bool IsUpStarted
        {
            get { return _isUpStarted; }
            set
            {
                _isUpStarted = value;
                RaisePropertyChanged(() => IsUpStarted);
            }
        }

        /// <summary>
        /// 加载完成标志
        /// </summary>
        private bool _isUpCompleted = false;
        /// <summary>
        /// 加载完成标志
        /// </summary>
        public bool IsUpCompleted
        {
            get { return _isUpCompleted; }
            set
            {
                _isUpCompleted = value;
                RaisePropertyChanged(() => IsUpCompleted);
            }
        }

        /// <summary>
        /// 加载开始时间
        /// </summary>
        private DateTime _upStartTime = DateTime.Now;
        /// <summary>
        /// 加载开始时间
        /// </summary>
        public DateTime UpStartTime
        {
            get { return _upStartTime; }
            set
            {
                _upStartTime = value;
                RaisePropertyChanged(() => UpStartTime);
            }
        }

        /// <summary>
        /// 压力保持开始标志
        /// </summary>
        private bool _isKeepPressStarted = false;
        /// <summary>
        /// 压力保持开始标志
        /// </summary>
        public bool IsKeepPressStarted
        {
            get { return _isKeepPressStarted; }
            set
            {
                _isKeepPressStarted = value;
                RaisePropertyChanged(() => IsKeepPressStarted);
            }
        }

        /// <summary>
        /// 压力保持完成标志
        /// </summary>
        private bool _isKeepPressCompleted = false;
        /// <summary>
        /// 压力保持完成标志
        /// </summary>
        public bool IsKeepPressCompleted
        {
            get { return _isKeepPressCompleted; }
            set
            {
                _isKeepPressCompleted = value;
                RaisePropertyChanged(() => IsKeepPressCompleted);
            }
        }

        /// <summary>
        /// 压力保持开始时间
        /// </summary>
        private DateTime _keepPressStartTime = DateTime.Now;
        /// <summary>
        /// 压力保持开始时间
        /// </summary>
        public DateTime KeepPressStartTime
        {
            get { return _keepPressStartTime; }
            set
            {
                _keepPressStartTime = value;
                RaisePropertyChanged(() => KeepPressStartTime);
            }
        }

        /// <summary>
        /// 压力已保持时间
        /// </summary>
        private double _PresskeeppingTimes = 0;
        /// <summary>
        /// 压力已保持时间
        /// </summary>
        public double PressKeeppingTimes
        {
            get { return _PresskeeppingTimes; }
            set
            {
                _PresskeeppingTimes = value;
                RaisePropertyChanged(() => PressKeeppingTimes);
            }
        }
    }
}
