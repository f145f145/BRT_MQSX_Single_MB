/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQDFJ_MB.Model.DEV
 * 文件名：  CommStatusModel
 * 版本号：  V1.0.0.0
 * 唯一标识：38d70cff-6e07-40a5-80e6-c1e13dcb9933
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022-5-25 17:37:42
 * 描述：
 *
 * ==================================================================================
 * 修改标记
 * 修改时间				修改人			版本号			描述
 * 2022-5-25 17:37:42		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;

namespace MQDFJ_MB.Model.DEV
{
    /// <summary>
    /// 通讯状态类
    /// </summary>
    public class CommStatusModel : ObservableObject
    {
        public CommStatusModel()
        {
            //定时器初始化
            PLC_RWTimerInit();
        }

        /// <summary>
        /// 通讯中状态
        /// </summary>
        private bool _isBusy = false;
        /// <summary>
        /// 通讯中状态
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        /// <summary>
        /// 通讯失败状态
        /// </summary>
        private bool _isFailure = true;
        /// <summary>
        /// 通讯失败状态
        /// </summary>
        public bool IsFailure
        {
            get
            {
                return _isFailure;
            }
            set
            {
                _isFailure = value;
                RaisePropertyChanged(() => IsFailure);
            }
        }

        /// <summary>
        /// 通讯状态指示灯颜色
        /// </summary>
        private int _statusCollor = 0;
        /// <summary>
        /// 通讯状态指示灯颜色
        /// </summary>
        public int StatusCollor
        {
            get
            {
                return _statusCollor;
            }
            set
            {
                _statusCollor = value;
                RaisePropertyChanged(() => StatusCollor);
            }
        }

        /// <summary>
        /// 计数
        /// </summary>
        private int _count = 0;
        /// <summary>
        /// 计数
        /// </summary>
        private int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
                RaisePropertyChanged(() => Count);
            }
        }



        #region 状态颜色定时器

        /// <summary>
        /// 状态颜色定时器
        /// </summary>
        private System.Threading.Timer CollorChangeTimer;

        /// <summary>
        /// 状态颜色定时器初始化函数
        /// </summary>
        private void PLC_RWTimerInit()
        {
            CollorChangeTimer = new System.Threading.Timer(new System.Threading.TimerCallback(CollorChangeTimerTick), this, 0, 500);
        }

        /// <summary>
        /// 状态颜色定时器回调函数
        /// </summary>
        /// <param name="state"></param>
        public void CollorChangeTimerTick(object state)
        {
            if (IsFailure)
            {
                StatusCollor = 0;
                return;
            }
            if (Count == 0)
            {
                StatusCollor = 1;
                Count++;
            }
            else
            {
                StatusCollor = 2;
                Count++;
                if (Count > 3)
                    Count = 0;
            }

        }

        #endregion
    }
}
