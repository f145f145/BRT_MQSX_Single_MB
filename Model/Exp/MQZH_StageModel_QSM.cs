/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQDFJ_MB.Model
 * 文件名：  MQZH_StageModel_QSM
 * 版本号：  V1.0.0.0
 * 唯一标识：67054737-b79f-43cc-b71d-544fd83e2962
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/11/17 16:00:01
 * 描述：
 * 幕墙四性气密、水密小阶段Model。
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2021/11/17 16:00:01		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace MQDFJ_MB.Model.Exp
{
    /// <summary>
    /// 气水密阶段类
    /// </summary>
    public class MQZH_StageModel_QSM : ObservableObject
    {
        #region 阶段基本属性

        /// <summary>
        /// 阶段编号
        /// </summary>
        private string _stage_NO="-1";
        /// <summary>
        /// 阶段编号
        /// </summary>
        public string Stage_NO
        {
            get
            {
                return _stage_NO;
            }
            set
            {
                _stage_NO = value;
                RaisePropertyChanged(() => Stage_NO);
            }
        }

        /// <summary>
        /// 阶段名称
        /// </summary>
        private string _stage_Name="等待操作";
        /// <summary>
        /// 阶段名称
        /// </summary>
        public string Stage_Name
        {
            get
            {
                return _stage_Name;
            }
            set
            {
                _stage_Name = value;
                RaisePropertyChanged(() => Stage_Name);
            }
        }

        /// <summary>
        /// 需要做试验（true为需要，false为不需要）
        /// </summary>
        private bool _needTest = true;
        /// <summary>
        /// 需要做试验（true为需要，false为不需要）
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
                if (StepList.Count > 0)
                {
                    for (int i = 0; i < StepList.Count; i++)
                        StepList[i].IsStepCompleted = value;
                }
                RaisePropertyChanged(() => CompleteStatus);
            }
        }

        /// <summary>
        /// 开始前需提示标志（true为需要）
        /// </summary>
        private bool _isNeedTipsBefore = false;
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
        private string _stringTipsBefore="";
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
        private bool _isTipsBeforeComplete=false;
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
        private string _stringTipsAfter="";
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


        #region 步骤组属性

        /// <summary>
        /// 步骤组列表
        /// </summary>
        private List<MQZH_StepModel_QSM> _stepList = new List<MQZH_StepModel_QSM>();
        /// <summary>
        /// 步骤组列表
        /// </summary>
        public List<MQZH_StepModel_QSM> StepList
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
