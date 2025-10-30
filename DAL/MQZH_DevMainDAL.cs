/************************************************************************************
 * 描述：
 *
 * ==================================================================================
 * 修改标记
 * 修改时间			        修改人			版本号			描述
 * 2022/2/8 15:10:12		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MQDFJ_MB.Model;
using MQDFJ_MB.Model.DEV;
using MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters;

namespace MQDFJ_MB.DAL
{
    /// <summary>
    /// 装置参数读写操作类
    /// </summary>
    public partial class MQZH_DevDAL : ObservableObject
    {
        public MQZH_DevDAL()
        {
            //Dataset、DataTable、TableAdapter初始化
            SetTableAdapterInit_Dev();

            //装置数据读写消息
            Messenger.Default.Register<string>(this, "DevDataRWMessage", DevDataRWMessage);
            Messenger.Default.Register<string>(this, "PublicData.ExpDQChanged", SaveLastExpNOMessage);
        }


        #region 公共数据

        /// <summary>
        /// 公共数据
        /// </summary>
        private PublicDatas _publicData = PublicDatas.GetInstance();
        /// <summary>
        /// 公共数据
        /// </summary>
        public PublicDatas PublicData
        {
            get { return _publicData; }
            set
            {
                _publicData = value;
                RaisePropertyChanged(() => _publicData);
            }
        }


        #endregion


        #region C装置参数表DataTable属性

        /// <summary>
        /// C01装置基本信息DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C01装置基本信息DataTable _c01Table;
        /// <summary>
        /// C01装置基本信息DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C01装置基本信息DataTable C01Table
        {
            get { return _c01Table; }
            set
            {
                _c01Table = value;
                RaisePropertyChanged(() => C01Table);
            }
        }

        /// <summary>
        /// C02装置基本参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C02装置基本参数DataTable _c02Table;
        /// <summary>
        /// C02装置基本参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C02装置基本参数DataTable C02Table
        {
            get { return _c02Table; }
            set
            {
                _c02Table = value;
                RaisePropertyChanged(() => C02Table);
            }
        }

        /// <summary>
        /// C03公司信息DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C03公司信息DataTable _c03Table;
        /// <summary>
        /// C03公司信息DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C03公司信息DataTable C03Table
        {
            get { return _c03Table; }
            set
            {
                _c03Table = value;
                RaisePropertyChanged(() => C03Table);
            }
        }

        /// <summary>
        /// C04串口参数设置DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C04串口参数设置DataTable _c04Table;
        /// <summary>
        /// C04串口参数设置DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C04串口参数设置DataTable C04Table
        {
            get { return _c04Table; }
            set
            {
                _c04Table = value;
                RaisePropertyChanged(() => C04Table);
            }
        }

        /// <summary>
        /// C05模拟量参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C05模拟量参数DataTable _c05Table;
        /// <summary>
        /// C05模拟量参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C05模拟量参数DataTable C05Table
        {
            get { return _c05Table; }
            set
            {
                _c05Table = value;
                RaisePropertyChanged(() => C05Table);
            }
        }

        /// <summary>
        /// C06模拟量通道参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C06模拟量通道参数DataTable _c06Table;
        /// <summary>
        /// C06模拟量通道参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C06模拟量通道参数DataTable C06Table
        {
            get { return _c06Table; }
            set
            {
                _c06Table = value;
                RaisePropertyChanged(() => C06Table);
            }
        }

        /// <summary>
        /// C07数字量参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C07数字量参数DataTable _c07Table;
        /// <summary>
        /// C07数字量参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C07数字量参数DataTable C07Table
        {
            get { return _c07Table; }
            set
            {
                _c07Table = value;
                RaisePropertyChanged(() => C07Table);
            }
        }

        /// <summary>
        /// C08数字量通道参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C08数字量通道参数DataTable _c08Table;
        /// <summary>
        /// C08数字量通道参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C08数字量通道参数DataTable C08Table
        {
            get { return _c08Table; }
            set
            {
                _c08Table = value;
                RaisePropertyChanged(() => C08Table);
            }
        }

        /// <summary>
        /// C09PID控制参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数DataTable _c09Table;
        /// <summary>
        /// C09PID控制参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数DataTable C09Table
        {
            get { return _c09Table; }
            set
            {
                _c09Table = value;
                RaisePropertyChanged(() => C09Table);
            }
        }

        /// <summary>
        /// C12气水密压力设定参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C12气水密压力设定参数DataTable _c12Table;
        /// <summary>
        /// C12气水密压力设定参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C12气水密压力设定参数DataTable C12Table
        {
            get { return _c12Table; }
            set
            {
                _c12Table = value;
                RaisePropertyChanged(() => C12Table);
            }
        }

        /// <summary>
        /// C13抗风压压力设定参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C13抗风压压力设定参数DataTable _c13Table;
        /// <summary>
        /// C13抗风压压力设定参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C13抗风压压力设定参数DataTable C13Table
        {
            get { return _c13Table; }
            set
            {
                _c13Table = value;
                RaisePropertyChanged(() => C13Table);
            }
        }

        /// <summary>
        /// C14压力控制参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C14压力控制参数DataTable _c14Table;
        /// <summary>
        /// C14压力控制参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C14压力控制参数DataTable C14Table
        {
            get { return _c14Table; }
            set
            {
                _c14Table = value;
                RaisePropertyChanged(() => C14Table);
            }
        }

        /// <summary>
        /// C15位移设定参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C15位移设定参数DataTable _c15Table;
        /// <summary>
        /// C15位移设定参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C15位移设定参数DataTable C15Table
        {
            get { return _c15Table; }
            set
            {
                _c15Table = value;
                RaisePropertyChanged(() => C15Table);
            }
        }

        /// <summary>
        /// C16位移控制参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C16位移控制参数DataTable _c16Table;
        /// <summary>
        /// C16位移控制参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSet.C16位移控制参数DataTable C16Table
        {
            get { return _c16Table; }
            set
            {
                _c16Table = value;
                RaisePropertyChanged(() => C16Table);
            }
        }
        #endregion


        #region 装置参数TableAdapter属性

        /// <summary>
        /// C01装置基本信息TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C01装置基本信息TableAdapter _c01TableAdapter;
        /// <summary>
        /// C01装置基本信息TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C01装置基本信息TableAdapter C01TableAdapter
        {
            get { return _c01TableAdapter; }
            set
            {
                _c01TableAdapter = value;
                RaisePropertyChanged(() => C01TableAdapter);
            }
        }

        /// <summary>
        /// C02装置基本参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C02装置基本参数TableAdapter _c02TableAdapter;
        /// <summary>
        /// C02装置基本参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C02装置基本参数TableAdapter C02TableAdapter
        {
            get { return _c02TableAdapter; }
            set
            {
                _c02TableAdapter = value;
                RaisePropertyChanged(() => C02TableAdapter);
            }
        }

        /// <summary>
        /// C03公司信息TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C03公司信息TableAdapter _c03TableAdapter;
        /// <summary>
        /// C03公司信息TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C03公司信息TableAdapter C03TableAdapter
        {
            get { return _c03TableAdapter; }
            set
            {
                _c03TableAdapter = value;
                RaisePropertyChanged(() => C03TableAdapter);
            }
        }

        /// <summary>
        /// C04串口参数设置TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C04串口参数设置TableAdapter _c04TableAdapter;
        /// <summary>
        /// C04串口参数设置TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C04串口参数设置TableAdapter C04TableAdapter
        {
            get { return _c04TableAdapter; }
            set
            {
                _c04TableAdapter = value;
                RaisePropertyChanged(() => C04TableAdapter);
            }
        }

        /// <summary>
        /// C05模拟量参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C05模拟量参数TableAdapter _c05TableAdapter;
        /// <summary>
        /// C05模拟量参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C05模拟量参数TableAdapter C05TableAdapter
        {
            get { return _c05TableAdapter; }
            set
            {
                _c05TableAdapter = value;
                RaisePropertyChanged(() => C05TableAdapter);
            }
        }

        /// <summary>
        /// C06模拟量通道参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C06模拟量通道参数TableAdapter _c06TableAdapter;
        /// <summary>
        /// C06模拟量通道参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C06模拟量通道参数TableAdapter C06TableAdapter
        {
            get { return _c06TableAdapter; }
            set
            {
                _c06TableAdapter = value;
                RaisePropertyChanged(() => C06TableAdapter);
            }
        }

        /// <summary>
        /// C07数字量参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C07数字量参数TableAdapter _c07TableAdapter;
        /// <summary>
        /// C07数字量参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C07数字量参数TableAdapter C07TableAdapter
        {
            get { return _c07TableAdapter; }
            set
            {
                _c07TableAdapter = value;
                RaisePropertyChanged(() => C07TableAdapter);
            }
        }

        /// <summary>
        /// C08数字量通道参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C08数字量通道参数TableAdapter _c08TableAdapter;
        /// <summary>
        /// C08数字量通道参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C08数字量通道参数TableAdapter C08TableAdapter
        {
            get { return _c08TableAdapter; }
            set
            {
                _c08TableAdapter = value;
                RaisePropertyChanged(() => C08TableAdapter);
            }
        }

        /// <summary>
        /// C09PID控制参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C09PID控制参数TableAdapter _c09TableAdapter;
        /// <summary>
        /// C09PID控制参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C09PID控制参数TableAdapter C09TableAdapter
        {
            get { return _c09TableAdapter; }
            set
            {
                _c09TableAdapter = value;
                RaisePropertyChanged(() => C09TableAdapter);
            }
        }


        /// <summary>
        /// C12气水密压力设定参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C12气水密压力设定参数TableAdapter _c12TableAdapter;
        /// <summary>
        /// C12气水密压力设定参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C12气水密压力设定参数TableAdapter C12TableAdapter
        {
            get { return _c12TableAdapter; }
            set
            {
                _c12TableAdapter = value;
                RaisePropertyChanged(() => C12TableAdapter);
            }
        }

        /// <summary>
        /// C13抗风压压力设定参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C13抗风压压力设定参数TableAdapter _c13TableAdapter;
        /// <summary>
        /// C13抗风压压力设定参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C13抗风压压力设定参数TableAdapter C13TableAdapter
        {
            get { return _c13TableAdapter; }
            set
            {
                _c13TableAdapter = value;
                RaisePropertyChanged(() => C13TableAdapter);
            }
        }

        /// <summary>
        /// C14压力控制参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C14压力控制参数TableAdapter _c14TableAdapter;
        /// <summary>
        /// C14压力控制参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C14压力控制参数TableAdapter C14TableAdapter
        {
            get { return _c14TableAdapter; }
            set
            {
                _c14TableAdapter = value;
                RaisePropertyChanged(() => C14TableAdapter);
            }
        }

        /// <summary>
        /// C15位移设定参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C15位移设定参数TableAdapter _c15TableAdapter;
        /// <summary>
        /// C15位移设定参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C15位移设定参数TableAdapter C15TableAdapter
        {
            get { return _c15TableAdapter; }
            set
            {
                _c15TableAdapter = value;
                RaisePropertyChanged(() => C15TableAdapter);
            }
        }

        /// <summary>
        /// C16位移控制参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C16位移控制参数TableAdapter _c16TableAdapter;
        /// <summary>
        /// C16位移控制参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_DevDataSetTableAdapters.C16位移控制参数TableAdapter C16TableAdapter
        {
            get { return _c16TableAdapter; }
            set
            {
                _c16TableAdapter = value;
                RaisePropertyChanged(() => C16TableAdapter);
            }
        }

        #endregion


        #region DataTable、TableAdapter初始化、更新

        /// <summary>
        /// Dataset、DataTable、TableAdapter初始化、读取数据
        /// </summary>
        private void SetTableAdapterInit_Dev()
        {
            try
            {
                //TableAdapter初始化
                C01TableAdapter = new C01装置基本信息TableAdapter();
                C02TableAdapter = new C02装置基本参数TableAdapter();
                C03TableAdapter = new C03公司信息TableAdapter();
                C04TableAdapter = new C04串口参数设置TableAdapter();
                C05TableAdapter = new C05模拟量参数TableAdapter();
                C06TableAdapter = new C06模拟量通道参数TableAdapter();
                C07TableAdapter = new C07数字量参数TableAdapter();
                C08TableAdapter = new C08数字量通道参数TableAdapter();
                C09TableAdapter = new C09PID控制参数TableAdapter();
                C12TableAdapter = new C12气水密压力设定参数TableAdapter();
                C13TableAdapter = new C13抗风压压力设定参数TableAdapter();
                C14TableAdapter = new C14压力控制参数TableAdapter();
                C15TableAdapter = new C15位移设定参数TableAdapter();
                C16TableAdapter = new C16位移控制参数TableAdapter();

                //DataTable初始化
                C01Table = new MQZH_DB_DevDataSet.C01装置基本信息DataTable();
                C02Table = new MQZH_DB_DevDataSet.C02装置基本参数DataTable();
                C03Table = new MQZH_DB_DevDataSet.C03公司信息DataTable();
                C04Table = new MQZH_DB_DevDataSet.C04串口参数设置DataTable();
                C05Table = new MQZH_DB_DevDataSet.C05模拟量参数DataTable();
                C06Table = new MQZH_DB_DevDataSet.C06模拟量通道参数DataTable();
                C07Table = new MQZH_DB_DevDataSet.C07数字量参数DataTable();
                C08Table = new MQZH_DB_DevDataSet.C08数字量通道参数DataTable();
                C09Table = new MQZH_DB_DevDataSet.C09PID控制参数DataTable();
                C12Table = new MQZH_DB_DevDataSet.C12气水密压力设定参数DataTable();
                C13Table = new MQZH_DB_DevDataSet.C13抗风压压力设定参数DataTable();
                C14Table = new MQZH_DB_DevDataSet.C14压力控制参数DataTable();
                C15Table = new MQZH_DB_DevDataSet.C15位移设定参数DataTable();
                C16Table = new MQZH_DB_DevDataSet.C16位移控制参数DataTable();

                //数据读取
                C01TableAdapter.Fill(C01Table);
                C02TableAdapter.Fill(C02Table);
                C03TableAdapter.Fill(C03Table);
                C04TableAdapter.Fill(C04Table);
                C05TableAdapter.Fill(C05Table);
                C06TableAdapter.Fill(C06Table);
                C07TableAdapter.Fill(C07Table);
                C08TableAdapter.Fill(C08Table);
                C09TableAdapter.Fill(C09Table);
                C12TableAdapter.Fill(C12Table);
                C13TableAdapter.Fill(C13Table);
                C14TableAdapter.Fill(C14Table);
                C15TableAdapter.Fill(C15Table);
                C16TableAdapter.Fill(C16Table);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        #endregion


        #region 根据消息读写装置数据

        /// <summary>
        /// 装置数据读写消息回调
        /// </summary>
        /// <param name="msg"></param>
        private void DevDataRWMessage(string msg)
        {
            try
            {
                //装置忙，无法操作
                if (PublicData.Dev.IsDeviceBusy)
                {
                    MessageBox.Show("装置忙，请关闭正在运行的试验或测试软件", "错误提示");
                    return;
                }


                //载入dev设置
                if (msg == "LoadDevSettings")
                {
                    LoadDevSettings();
                }
                //保存dev设置
                else if (msg == "SaveDevSettings")
                {
                    SaveDevSettings();
                }

                //保存装置基本信息
                else if (msg == "SaveDevBisicInfo")
                {
                    SaveDevBisicInfo();
                }
                else if (msg == "SaveDevBisicParam")
                {
                    SaveDevBisicParam();
                }
                //保存公司信息
                else if (msg == "SaveCoInfo")
                {
                    SaveCoInfo();
                }
                //保存串口参数
                else if (msg == "SaveDevComSettings")
                {
                    SaveDevComSettings();
                }
                //保存传感器参数
                else if (msg == "SaveDevSenssorSettings")
                {
                    SaveAIOParam();
                    SaveAIOChannels();

                }
                //保存PID控制参数
                else if (msg == "SaveDevPIDSettings")
                {
                    SaveDevPIDSettings();
                    LoadDevPIDSettings();
                }
                //保存其他参数
                //else if (msg == "SaveDevOther")
                //{
                //    SaveDevOther();
                //}
                //保存压力设定
                else if (msg == "SaveDevPressSettings")
                {
                    SavePressSettings();
                    LoadPressSettings();
                }
                //保存压力控制参数
                else if (msg == "SaveDevPressCtl")
                {
                    SavePressCtlSettings();
                    LoadPressCtlSettings();
                }
                //保存位移设定参数
                else if (msg == "SaveDevWYSettings")
                {
                    SaveDevWYSettings();
                }
                //保存位移控制参数
                else if (msg == "SaveDevWYCtlSettings")
                {
                    SaveDevWYCtlSettings();
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        #endregion
    }
}
