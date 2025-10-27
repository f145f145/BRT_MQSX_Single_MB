/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQDFJ_MB.Model.Exp
 * 文件名：  MQZH_StepMode_CJBX
 * 版本号：  V1.0.0.0
 * 唯一标识：41e4c86b-1c10-4747-a54b-ff590b615714
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/11/16 23:13:47
 * 描述：
 * 层间变形步骤Model。
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2021/11/16       23:13:47		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;

namespace MQDFJ_MB.Model.Exp
{
    /// <summary>
    /// 幕墙四性步骤参数类
    /// </summary>
    public class MQZH_StepModel_CJBX : ObservableObject
    {

        #region 步骤基本参数属性

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
        private string _stepName="等待操作";
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
        /// 试样目标位置
        /// </summary>
        private double _aimPosition = 0.0;
        /// <summary>
        /// 试样目标位置
        /// </summary>
        public double AimPosition
        {
            get { return _aimPosition; }
            set
            {
                _aimPosition = value;
                RaisePropertyChanged(() => AimPosition);
            }
        }

        /// <summary>
        /// 设备目标位置
        /// </summary>
        private double _aimPositionSB = 0.0;
        /// <summary>
        /// 设备目标位置
        /// </summary>
        public double AimPositionSB
        {
            get { return _aimPositionSB; }
            set
            {
                _aimPositionSB = value;
                RaisePropertyChanged(() => AimPositionSB);
            }
        }

        /// <summary>
        /// 开始前等待完成标志
        /// </summary>
        private bool _isWaitBeforCompleted = false;
        /// <summary>
        /// 开始前等待完成标志
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
        /// 开始前等待时间
        /// </summary>
        private int _timeWaitBefor=0;
        /// <summary>
        /// 开始前等待时间
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

    }
}
