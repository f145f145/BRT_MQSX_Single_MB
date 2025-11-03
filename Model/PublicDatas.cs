#region << 版 本 注 释 >>
/*************************************************************************************
 * 版权所有：宝瑞通  保留所有权利。
 * 公司名称：沈阳宝瑞通自动化设备有限公司
 * 创 建 者：HaoZhengqiang
 * 电子邮箱：88129312@qq.com
 * 创建时间：2025/10/27 22:09:51
 * 版    本：V1.0.0
 * 描    述：
 * ===================================================================================
 * 历史更新记录
 * 版    本：V
 * 修改时间：
 * 修 改 人：
 * 修改内容：
**************************************************************************************/
#endregion 

using System.Collections.Generic;
using GalaSoft.MvvmLight;
using MQDFJ_MB.BLL;
using MQDFJ_MB.Communication;
using MQDFJ_MB.DAL;
using MQDFJ_MB.Model.DEV;
using MQDFJ_MB.Model.Exp;
using MQDFJ_MB.Model.Exp_MB;

namespace MQDFJ_MB.Model
{
    public partial class PublicDatas : ObservableObject
    {
        #region 单例模式实现

        /// <summary>
        /// 单例模式的私有构造函数
        /// </summary>
        private PublicDatas()
        {
            //DataBaseInit();
            //Comm = new Communication() { };
        }

        private static PublicDatas instance;
        private static object _lock = new object();

        public static PublicDatas GetInstance()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new PublicDatas();
                    }
                }
            }
            return instance;
        }

        #endregion


        #region 装置、通讯、试验、数据操作属性

        /// <summary>
        /// 装置参数
        /// </summary>
        private MQZH_DevModel_Main _dev = new MQZH_DevModel_Main();
        /// <summary>
        /// 装置参数
        /// </summary>
        public MQZH_DevModel_Main Dev
        {
            get { return _dev; }
            set
            {
                _dev = value;
                RaisePropertyChanged(() => Dev);
            }
        }


        /// <summary>
        /// 当前试验
        /// </summary>
        private MQZH_ExpTotallModel _expDQ = new MQZH_ExpTotallModel();
        /// <summary>
        /// 当前试验
        /// </summary>
        public MQZH_ExpTotallModel ExpDQ
        {
            get { return _expDQ; }
            set
            {
                _expDQ = value;
                RaisePropertyChanged(() => ExpDQ);
            }
        }


        /// <summary>
        /// 当前动态风压校准试验
        /// </summary>
        private SM_DTFY_Cali _sM_DTFY_CaliDQ = new SM_DTFY_Cali();
        /// <summary>
        /// 当前试验
        /// </summary>
        public SM_DTFY_Cali SM_DTFY_CaliDQ
        {
            get { return _sM_DTFY_CaliDQ; }
            set
            {
                _sM_DTFY_CaliDQ = value;
                RaisePropertyChanged(() => SM_DTFY_CaliDQ);
            }
        }


        /// <summary>
        /// 在用动态风压校准试验
        /// </summary>
        private SM_DTFY_Cali _sM_DTFY_CaliUsing = new SM_DTFY_Cali();
        /// <summary>
        /// 在用动态风压校准试验
        /// </summary>
        public SM_DTFY_Cali SM_DTFY_CaliUsing
        {
            get { return _sM_DTFY_CaliUsing; }
            set
            {
                _sM_DTFY_CaliUsing = value;
                RaisePropertyChanged(() => SM_DTFY_CaliUsing);
            }
        }


        /// <summary>
        /// 通讯
        /// </summary>
        private MQZH_Communication _communication;
        /// <summary>
        /// 通讯
        /// </summary>
        public MQZH_Communication Communication
        {
            get { return _communication; }
            set
            {
                _communication = value;
                RaisePropertyChanged(() => Communication);
            }
        }

        #endregion



        #region 动态风压校准试验管理相关属性

        /// <summary>
        /// 动态风压校准试验列表
        /// </summary>
        private List<SM_DTFY_Cali> _testTable_DTFYCali = new List<SM_DTFY_Cali>() { };
        /// <summary>
        /// 动态风压校准试验列表
        /// </summary>
        public List<SM_DTFY_Cali> TestTable_DTFYCali
        {
            get { return _testTable_DTFYCali; }
            set
            {
                _testTable_DTFYCali = value;
                RaisePropertyChanged(() => TestTable_DTFYCali);
            }
        }


        /// <summary>
        /// 列表中选中的动态风压校准试验编号
        /// </summary>
        private string _selectedTestNO_DTFYCali = "";
        /// <summary>
        /// 列表中选中的动态风压校准试验编号
        /// </summary>
        public string SelectedTestNO_DTFYCali
        {
            get { return _selectedTestNO_DTFYCali; }
            set
            {
                _selectedTestNO_DTFYCali = value;
                RaisePropertyChanged(() => SelectedTestNO_DTFYCali);
            }
        }


        /// <summary>
        /// 将要复制的动态风压校准试验编号
        /// </summary>
        private string _expNOCopyNew_DTFYCali = "";
        /// <summary>
        /// 将要复制的动态风压校准试验编号
        /// </summary>
        public string ExpNOCopyNew_DTFYCali
        {
            get { return _expNOCopyNew_DTFYCali; }
            set
            {
                _expNOCopyNew_DTFYCali = value;
                RaisePropertyChanged(() => ExpNOCopyNew_DTFYCali);
            }
        }


        /// <summary>
        /// 将要新增的动态风压校准试验编号
        /// </summary>
        private string _expNONew_DTFYCali = "";
        /// <summary>
        /// 将要新增的动态风压校准试验编号
        /// </summary>
        public string ExpNONew_DTFYCali
        {
            get { return _expNONew_DTFYCali; }
            set
            {
                _expNONew_DTFYCali = value;
                RaisePropertyChanged(() => ExpNONew_DTFYCali);
            }
        }

        #endregion

    }
}
