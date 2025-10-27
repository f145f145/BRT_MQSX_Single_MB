/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQZHWL.Model
 * 文件名：  MQZH_StageModel_CJBX
 * 版本号：  V1.0.0.0
 * 唯一标识：67054737-b79f-43cc-b71d-544fd83e2962
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/11/17 16:00:01
 * 描述：
 * 幕墙四性层间变形阶段Model。
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2021/11/17 16:00:01		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace MQZHWL.Model.Exp
{
    /// <summary>
    /// 层间变形阶段类
    /// </summary>
    public class MQZH_StageModel_CJBX : ObservableObject
    {

        #region 阶段基本属性

        /// <summary>
        /// 阶段编号
        /// </summary>
        private string _stageNO = "-1";
        /// <summary>
        /// 阶段编号
        /// </summary>
        public string StageNO
        {
            get
            {
                return _stageNO;
            }
            set
            {
                _stageNO = value;
                RaisePropertyChanged(() => StageNO);
            }
        }

        /// <summary>
        /// 阶段名称
        /// </summary>
        private string _stageName = "等待操作";
        /// <summary>
        /// 阶段名称
        /// </summary>
        public string StageName
        {
            get
            {
                return _stageName;
            }
            set
            {
                _stageName = value;
                RaisePropertyChanged(() => StageName);
            }
        }

        /// <summary>
        /// 阶段选中状态（true为选中）
        /// </summary>
        private bool _needTest = true;
        /// <summary>
        /// 阶段选中状态（true为选中）
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
        /// 阶段完成状态
        /// </summary>
        private bool _completeStatus = false;
        /// <summary>
        /// 阶段完成状态
        /// </summary>
        public bool CompleteStatus
        {
            get { return _completeStatus; }
            set
            {
                _completeStatus = value;
                //步骤组完成状态及提示完成状态跟随所属阶段
                if (StepList.Count > 0)
                {
                    for (int i = 0; i < StepList.Count; i++)
                    {
                        StepList[i].IsStepCompleted = value;
                    }
                }
                RaisePropertyChanged(() => CompleteStatus);
            }
        }

        /// <summary>
        /// 开始前需提示标志（true为需要）
        /// </summary>
        private bool _isNeedTipsBefore = true ;
        /// <summary>
        /// 开始前需提示标志（true为需要）
        /// </summary>
        public bool IsNeedTipsBefore
        {
            get { return _isNeedTipsBefore; }
            set
            {
                _isNeedTipsBefore = value;
                RaisePropertyChanged(() => IsNeedTipsBefore);
            }
        }

        /// <summary>
        /// 开始前提示文字
        /// </summary>
        private string _stringTipsBefore = "请将可开启部分开关，最后锁紧后再确认下一步！";
        /// <summary>
        /// 开始前提示文字
        /// </summary>
        public string StringTipsBefore
        {
            get { return _stringTipsBefore; }
            set
            {
                _stringTipsBefore = value;
                RaisePropertyChanged(() => StringTipsBefore);
            }
        }

        /// <summary>
        /// 开始前提示完成标志
        /// </summary>
        private bool _isTipsBeforeComplete = false;
        /// <summary>
        /// 开始前提示完成标志
        /// </summary>
        public bool IsTipsBeforeComplete
        {
            get { return _isTipsBeforeComplete; }
            set
            {
                _isTipsBeforeComplete = value;
                RaisePropertyChanged(() => IsTipsBeforeComplete);
            }
        }

        /// <summary>
        /// 结束后需提示标志
        /// </summary>
        private bool _isNeedTipsAfter = false;
        /// <summary>
        /// 结束后需提示标志
        /// </summary>
        public bool IsNeedTipsAfter
        {
            get { return _isNeedTipsAfter; }
            set
            {
                _isNeedTipsAfter = value;
                RaisePropertyChanged(() => IsNeedTipsAfter);
            }
        }

        /// <summary>
        /// 结束后提示文字
        /// </summary>
        private string _stringTipsAfter = "";
        /// <summary>
        /// 结束后提示文字
        /// </summary>
        public string StringTipsAfter
        {
            get { return _stringTipsAfter; }
            set
            {
                _stringTipsAfter = value;
                RaisePropertyChanged(() => StringTipsAfter);
            }
        }

        /// <summary>
        /// 结束后提示完成标志
        /// </summary>
        private bool _isTipsAfterComplete = false;
        /// <summary>
        /// 结束后提示完成标志
        /// </summary>
        public bool IsTipsAfterComplete
        {
            get { return _isTipsAfterComplete; }
            set
            {
                _isTipsAfterComplete = value;
                RaisePropertyChanged(() => IsTipsAfterComplete);
            }
        }

        #endregion


        #region 目标数据

        /// <summary>
        /// 试件平面位移角移动目标（XY位移角分母）
        /// </summary>
        private double _wyjMB_XYFM_SJ = 0;
        /// <summary>
        /// 试件平面位移角移动目标（XY位移角分母）
        /// </summary>
        public double WYJMB_XYFM_SJ
        {
            get { return _wyjMB_XYFM_SJ; }
            set
            {
                _wyjMB_XYFM_SJ = value;
                RaisePropertyChanged(() => WYJMB_XYFM_SJ);
            }
        }

        /// <summary>
        /// 试件层间位移值目标（Z轴）
        /// </summary>
        private double _wyMB_Z_SJ = 0.0;
        /// <summary>
        /// 试件层间位移值目标（Z轴）
        /// </summary>
        public double WYMB_Z_SJ
        {
            get { return _wyMB_Z_SJ; }
            set
            {
                _wyMB_Z_SJ = value;
                RaisePropertyChanged(() => WYMB_Z_SJ);
            }
        }

        #endregion


        #region 实测数据

        /// <summary>
        /// 试件平面实测位移值（XY位移值）
        /// </summary>
        private double _wy_XY_SJ = 0;
        /// <summary>
        /// 试件平面实测位移值（XY位移值）
        /// </summary>
        public double WY_XY_SJ
        {
            get { return _wy_XY_SJ; }
            set
            {
                _wy_XY_SJ = value;
                RaisePropertyChanged(() => WY_XY_SJ);
            }
        }

        /// <summary>
        /// 试件平面实测位移角（XY位移角分母）
        /// </summary>
        private double _wyj_XYFM_SJ = 0;
        /// <summary>
        /// 试件平面实测位移角（XY位移角分母）
        /// </summary>
        public double WYJ_XYFM_SJ
        {
            get { return _wyj_XYFM_SJ; }
            set
            {
                _wyj_XYFM_SJ = value;
                RaisePropertyChanged(() => WYJ_XYFM_SJ);
            }
        }

        /// <summary>
        /// 试件层间实测位移值（Z轴位移值）
        /// </summary>
        private double _wy_Z_SJ = 0.0;
        /// <summary>
        /// 试件层间实测位移值（Z轴位移值）
        /// </summary>
        public double WY_Z_SJ
        {
            get { return _wy_Z_SJ; }
            set
            {
                _wy_Z_SJ = value;
                RaisePropertyChanged(() => WY_Z_SJ);
            }
        }

        #endregion


        #region 步骤组属性

        /// <summary>
        /// 步骤组列表
        /// </summary>
        private ObservableCollection<MQZH_StepModel_CJBX> _stepList = new ObservableCollection<MQZH_StepModel_CJBX>();
        /// <summary>
        /// 步骤组列表
        /// </summary>
        public ObservableCollection<MQZH_StepModel_CJBX> StepList
        {
            get
            {
                return _stepList;
            }
            set
            {
                _stepList = value;
                RaisePropertyChanged(() => StepList);
            }
        }

        #endregion


    }
}
