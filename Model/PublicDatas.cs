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
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MQDFJ_MB.BLL;
using MQDFJ_MB.Communication;
using MQDFJ_MB.DAL;
using MQDFJ_MB.Model.DEV;
using MQDFJ_MB.Model.Exp;
using MQDFJ_MB.Model.Exp_MB;
using SqlSugar;

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
        private ObservableCollection<SM_DTFY_Cali> _testList_DTFYCali = new ObservableCollection<SM_DTFY_Cali>() { };
        /// <summary>
        /// 动态风压校准试验列表
        /// </summary>
        public ObservableCollection<SM_DTFY_Cali> TestList_DTFYCali
        {
            get { return _testList_DTFYCali; }
            set
            {
                _testList_DTFYCali = value;
                RaisePropertyChanged(() => TestList_DTFYCali);
            }
        }


        /// <summary>
        /// 选中的动态风压校准试验
        /// </summary>
        private SM_DTFY_Cali _selectedTest_DTFYCali = new SM_DTFY_Cali() { };
        /// <summary>
        /// 选中的动态风压校准试验
        /// </summary>
        public SM_DTFY_Cali SelectedTest_DTFYCali
    {
            get { return _selectedTest_DTFYCali; }
            set
            {
            _selectedTest_DTFYCali = value;
                RaisePropertyChanged(() => SelectedTest_DTFYCali);
            }
        }


        /// <summary>
        /// 选中的动态风压校准试验压力点
        /// </summary>
        private SM_DTFY_Cali_PressPoint _selectedPoint_DTFYCali = new SM_DTFY_Cali_PressPoint() { };
        /// <summary>
        /// 选中的动态风压校准试验压力点
        /// </summary>
        public SM_DTFY_Cali_PressPoint SelectedPoint_DTFYCali
        {
            get { return _selectedPoint_DTFYCali; }
            set
            {
                _selectedPoint_DTFYCali = value;
                RaisePropertyChanged(() => SelectedPoint_DTFYCali);
            }
        }


        /// <summary>
        /// 选中的动态风压校准试验压力点索引
        /// </summary>
        private int _selectedTestIndex_DTFYCali = 0;
        /// <summary>
        /// 选中的动态风压校准试验压力点索引
        /// </summary>
        public int SelectedTestIndex_DTFYCali
        {
            get { return _selectedTestIndex_DTFYCali; }
            set
            {
                _selectedTestIndex_DTFYCali = value;
                RaisePropertyChanged(() => SelectedTestIndex_DTFYCali);
                Messenger.Default.Send<string>("DTFYCali_LogListForView", "NeedUpdate_DTFYCali");
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


        /// <summary>
        /// 动态风压校准试验记录列表-显示用
        /// </summary>
        private ObservableCollection<SM_DTFY_Cali_Log> _logList_DTFYCali = new ObservableCollection<SM_DTFY_Cali_Log>() { };
        /// <summary>
        /// 动态风压校准试验记录列表-显示用
        /// </summary>
        public ObservableCollection<SM_DTFY_Cali_Log> LogList_DTFYCali
        {
            get { return _logList_DTFYCali; }
            set
            {
                _logList_DTFYCali = value;
                RaisePropertyChanged(() => LogList_DTFYCali);
            }
        }

        #endregion



        #region sqlite数据库DB属性

        /// <summary>
        /// 装置参数数据库
        /// </summary>
        public SqlSugarClient DevDB = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = DAL_Str.ConnString_Dev,
            DbType = DbType.Sqlite,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute,
        });


        /// <summary>
        /// 主数据库
        /// </summary>
        public SqlSugarScope MainDB = new SqlSugarScope(new ConnectionConfig()
        {
            ConnectionString = DAL_Str.ConnString_Main,
            DbType = DbType.Sqlite,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute,
        });
        #endregion
    }
}
