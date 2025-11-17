using GalaSoft.MvvmLight;
using MQDFJ_MB.Communication;
using MQDFJ_MB.Model.Exp;
using System.Windows;
using MQDFJ_MB.BLL;
using GalaSoft.MvvmLight.Messaging;
using System;
using GalaSoft.MvvmLight.Command;
using MQDFJ_MB.Model;
using System.Collections.Generic;
using System.ComponentModel;
using MQDFJ_MB.Model.Exp_MB;
using MQDFJ_MB.DAL.Dev;
using MQDFJ_MB.DAL.Exp;
using MQDFJ_MB.DAL.Rep;
using MQDFJ_MB.DAL.DTFYCali;


namespace MQDFJ_MB.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {

        /// <summary>
        /// 主窗口ViewModel
        /// </summary>
        public MainViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;

            //有窗口新增或关闭
            Messenger.Default.Register<string>(this, "WindowClosed", WindowClosedMessage);
            Messenger.Default.Register<Window>(this, "NewWindow", WindowAddedMessage);

            //试验列表已更新
            Messenger.Default.Register<MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数DataTable>(this, "ExpTableChanged", ExpTableChangedMessage);
            Messenger.Default.Register<List<SM_DTFY_Cali>>(this, "ExpTableChanged_DTFYCali", ExpTableChanged_DTFYCali);

            //订阅绘图数据更新消息
            Messenger.Default.Register<string>(this, "UpdateChart", UpdateChartMessage);

            //装置、试验初始化
            //    Dev = new MQZH_DevModel_Main();
            PublicData.ExpDQ = new MQZH_ExpTotallModel();

            //DAL初始化及赋值
            MainViewModel_DevDAL = new MQZH_DevDAL();
            MainViewModel_ExpDAL = new MQZH_ExpDAL();
            MainViewModel_RepDAL = new MQZH_RepDAL();
            MainViewModel_DTFYCaliDAL = new DTFYCaliTestDAL();

            //载入装置参数
            Messenger.Default.Send<string>("LoadDevSettings", "DevDataRWMessage");
            //载入试验参数及数据
            if (!PublicData.Dev.IsLoadLastExpPowerOn)
            {
                Messenger.Default.Send<string>("DefaultExp", "LoadExpByName");
                Messenger.Default.Send<string>("DefaultExp", "LoadExpByNameDTFYCali");
            }
            else
            {
                Messenger.Default.Send<string>(PublicData.Dev.ExpNOLast, "LoadExpByName");
                Messenger.Default.Send<string>(PublicData.Dev.DtfysmParam.TestNoDQDTFYCali, "LoadExpByNameDTFYCali");
            }

            //通讯初始化
            MainViewModel_Communication = new MQZH_Communication();
            MainViewModel_Communication.CommunicationInit();

            //主控BLL初始化
            MainViewModel_Bll = new MQZH_ExpBLL();

            PublicData.Dev.PID_SMSLL = MainViewModel_Bll.PublicData.Dev.PID_SMSLL;

            //绘图初始化
            PlotInit();
            NDPlotInit();


            //打开提示窗口信息
            Messenger.Default.Register<string>(this, "OpenPrompt", OpenPromptMessage);


            //注册管理员登录消息
            Messenger.Default.Register<string>(this, "AdminLogginMessage", AdminLogginMessage);
        }


        #region 通讯、数据操作属性

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


        /// <summary>
        /// 通讯
        /// </summary>
        private MQZH_Communication _mainViewModel_Communication;
        /// <summary>
        /// 通讯
        /// </summary>
        public MQZH_Communication MainViewModel_Communication
        {
            get { return _mainViewModel_Communication; }
            set
            {
                _mainViewModel_Communication = value;
                RaisePropertyChanged(() => MainViewModel_Communication);
            }
        }

        /// <summary>
        /// 主控流程
        /// </summary>
        private MQZH_ExpBLL _mainViewModel_Bll;
        /// <summary>
        /// 主控流程
        /// </summary>
        public MQZH_ExpBLL MainViewModel_Bll
        {
            get { return _mainViewModel_Bll; }
            set
            {
                _mainViewModel_Bll = value;
                RaisePropertyChanged(() => MainViewModel_Bll);
            }
        }

        /// <summary>
        /// 装置参数读写
        /// </summary>
        private MQZH_DevDAL _mainViewModel_DevDAL;
        /// <summary>
        /// 装置参数读写
        /// </summary>
        public MQZH_DevDAL MainViewModel_DevDAL
        {
            get { return _mainViewModel_DevDAL; }
            set
            {
                _mainViewModel_DevDAL = value;
                RaisePropertyChanged(() => MainViewModel_DevDAL);
            }
        }

        /// <summary>
        /// 试验数据读写
        /// </summary>
        private MQZH_ExpDAL _mainViewModel_ExpDAL;
        /// <summary>
        /// 试验数据读写
        /// </summary>
        public MQZH_ExpDAL MainViewModel_ExpDAL
        {
            get { return _mainViewModel_ExpDAL; }
            set
            {
                _mainViewModel_ExpDAL = value;
                RaisePropertyChanged(() => MainViewModel_ExpDAL);
            }
        }

        /// <summary>
        /// 动态风压校准试验数据读写
        /// </summary>
        private DTFYCaliTestDAL _mainViewModel_DTFYCaliDAL;
        /// <summary>
        /// 动态风压校准试验数据读写
        /// </summary>
        public DTFYCaliTestDAL MainViewModel_DTFYCaliDAL
        {
            get { return _mainViewModel_DTFYCaliDAL; }
            set
            {
                _mainViewModel_DTFYCaliDAL = value;
                RaisePropertyChanged(() => MainViewModel_ExpDAL);
            }
        }

        /// <summary>
        /// 试验报告读写
        /// </summary>
        private MQZH_RepDAL _mainViewModel_RepDAL;
        /// <summary>
        /// 试验报告读写
        /// </summary>
        public MQZH_RepDAL MainViewModel_RepDAL
        {
            get { return _mainViewModel_RepDAL; }
            set
            {
                _mainViewModel_RepDAL = value;
                RaisePropertyChanged(() => MainViewModel_RepDAL);
            }
        }

        #endregion


        #region 按钮操作指令相关回调

        #region 紧急停机按钮操作指令

        /// <summary>
        /// 传递指令
        /// </summary>
        private RelayCommand<String> _mainViewCommand;
        /// <summary>
        /// 传递指令
        /// </summary>
        public RelayCommand<String> MainViewCommand
        {
            get
            {
                if (_mainViewCommand == null)
                    _mainViewCommand = new RelayCommand<String>((p) => ExecuteMainViewCMD(p));
                return _mainViewCommand;

            }
            set { _mainViewCommand = value; }
        }

        /// <summary>
        /// 根据按钮编号，切换按钮指令、发送通讯指令
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecuteMainViewCMD(String num)
        {
            int i = Convert.ToInt16(num);
            //紧急停机
            if (i == 119)
            {
                Messenger.Default.Send<string>("JJTJ", "JJTJMessage");
            }
        }

        #endregion


        #region 试验数据窗口相关指令

        /// <summary>
        /// 传递试验数据窗口指令
        /// </summary>
        private RelayCommand<String> _expDataCommand;
        /// <summary>
        /// 传递试验数据窗口指令
        /// </summary>
        public RelayCommand<String> ExpDataCommand
        {
            get
            {
                if (_expDataCommand == null)
                    _expDataCommand = new RelayCommand<String>((p) => ExecuteExpDataCMD(p));
                return _expDataCommand;

            }
            set { _expDataCommand = value; }
        }

        /// <summary>
        /// 试验数据窗口操作
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecuteExpDataCMD(String num)
        {
            int i = Convert.ToInt16(num);
            //保存试验参数、数据
            if (i == 1)
            {
                Messenger.Default.Send<string>("SaveExpAll", "SaveExpMessage");
            }

            //导出气密、水密、抗风压试验报告
            else if (i == 2)
            {
                Messenger.Default.Send<string>("SaveSXRPT", "OutPutRPTMessage");
            }
            //导出气密、水密、抗风压试验报告
            else if (i == 3)
            {
                Messenger.Default.Send<string>("SaveCJBXRPT", "OutPutRPTMessage");
            }

            //重新计算并保存气密水密抗风压检测数据
            else if (i == 4)
            {
                PublicData.ExpDQ.QM_Evaluate();
                PublicData.ExpDQ.SM_Evaluate();
                PublicData.ExpDQ.KFY_Evaluate();
                Messenger.Default.Send<string>("SaveQMStatusAndData", "SaveExpMessage");
                Messenger.Default.Send<string>("SaveSMStatusAndData", "SaveExpMessage");
                Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");

                NDPlotUpdate();
            }

            //重新计算并保存层间变形检测数据
            else if (i == 5)
            {
                PublicData.ExpDQ.CJBX_DJEvaluate();
                PublicData.ExpDQ.CJBX_GCEvaluate();

                Messenger.Default.Send<string>("SaveCJBXStatusAndData", "SaveExpMessage");
            }

            //打开水密定级检测渗漏窗口
            else if (i == 21)
            {
                PublicData.ExpDQ.ExpData_SM.SLStatusCopy();
                Messenger.Default.Send<string>(MQZH_WinName.SMDJDamageWinName, "OpenGivenNameWin");
            }
            //打开水密工程检测渗漏窗口
            else if (i == 22)
            {
                PublicData.ExpDQ.ExpData_SM.SLStatusCopy();
                Messenger.Default.Send<string>(MQZH_WinName.SMGCDamageWinName, "OpenGivenNameWin");
            }

            //打开抗风压定级检测损坏窗口
            else if (i == 31)
            {
                Messenger.Default.Send<string>(MQZH_WinName.KFYDJDamageWinName, "OpenGivenNameWin");
            }
            //打开抗风压工程检测损坏窗口
            else if (i == 32)
            {
                Messenger.Default.Send<string>(MQZH_WinName.KFYGCDamageWinName, "OpenGivenNameWin");
            }

            //打开层间变形定级检测损坏情况确认窗口
            else if (i == 41)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CJBXDJDamageWinName, "OpenGivenNameWin");
            }
            //打开层间变形工程检测损坏情况确认窗口
            else if (i == 42)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CJBXGCDamageWinName, "OpenGivenNameWin");
            }

            //备份数据
            else if (i == 8699)
            {
                Messenger.Default.Send<string>("All", "DataBackUpMessage");
            }
        }

        #endregion


        #region 装置设定窗口指令

        /// <summary>
        /// 装置设定窗口指令
        /// </summary>
        private RelayCommand<String> _devSetCommand;
        /// <summary>
        /// 装置设定窗口指令
        /// </summary>
        public RelayCommand<String> DevSetCommand
        {
            get
            {
                if (_devSetCommand == null)
                    _devSetCommand = new RelayCommand<String>((p) => ExecuteDevSetCMD(p));
                return _devSetCommand;

            }
            set { _devSetCommand = value; }
        }

        /// <summary>
        /// 装置设定窗口指令回调
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecuteDevSetCMD(String num)
        {
            int i = Convert.ToInt16(num);

            //取消设定
            if (i == 99)
            {
                Messenger.Default.Send<string>("LoadDevSettings", "DevDataRWMessage");
            }
            //所有装置设定参数保存
            else if (i == 0)
            {
                Messenger.Default.Send<string>("SaveDevSettings", "DevDataRWMessage");
            }


            //装置基本信息保存
            else if (i == 1)
            {
                Messenger.Default.Send<string>("SaveDevBisicInfo", "DevDataRWMessage");
            }
            //装置基本参数
            else if (i == 2)
            {
                Messenger.Default.Send<string>("SaveDevBisicParam", "DevDataRWMessage");
            }
            //公司信息保存
            else if (i == 3)
            {
                Messenger.Default.Send<string>("SaveCoInfo", "DevDataRWMessage");
            }
            //串口配置参数保存
            else if (i == 4)
            {
                Messenger.Default.Send<string>("SaveDevComSettings", "DevDataRWMessage");
            }
            //传感器参数保存
            else if (i == 5)
            {
                Messenger.Default.Send<string>("SaveDevSenssorSettings", "DevDataRWMessage");
            }
            //装置PID控制参数保存
            else if (i == 6)
            {
                Messenger.Default.Send<string>("SaveDevPIDSettings", "DevDataRWMessage");
            }
            //装置其它保存
            else if (i == 7)
            {
                Messenger.Default.Send<string>("SaveDevOther", "DevDataRWMessage");
            }
            //装置气压设定保存
            else if (i == 8)
            {
                Messenger.Default.Send<string>("SaveDevPressSettings", "DevDataRWMessage");
            }
            //压力控制保存
            else if (i == 9)
            {
                Messenger.Default.Send<string>("SaveDevPressCtl", "DevDataRWMessage");
            }
            //位移设定参数保存
            else if (i == 10)
            {
                Messenger.Default.Send<string>("SaveDevWYSettings", "DevDataRWMessage");
            }
            //位移控制参数保存
            else if (i == 11)
            {
                Messenger.Default.Send<string>("SaveDevWYCtlSettings", "DevDataRWMessage");
            }
            //装置调零校正保存
            else if (i == 12)
            {
                Messenger.Default.Send<string>("SaveDevSenssorSettings", "DevDataRWMessage");
            }


            //自动调零
            else if (i == 8551)
            {
                Messenger.Default.Send<string>("WY01", "SetZeroMessage");
            }
            else if (i == 8552)
            {
                Messenger.Default.Send<string>("WY02", "SetZeroMessage");
            }
            else if (i == 8553)
            {
                Messenger.Default.Send<string>("WY03", "SetZeroMessage");
            }
            else if (i == 8554)
            {
                Messenger.Default.Send<string>("WY04", "SetZeroMessage");
            }
            else if (i == 8555)
            {
                Messenger.Default.Send<string>("WY05", "SetZeroMessage");
            }
            else if (i == 8556)
            {
                Messenger.Default.Send<string>("WY06", "SetZeroMessage");
            }
            else if (i == 8557)
            {
                Messenger.Default.Send<string>("WY07", "SetZeroMessage");
            }
            else if (i == 8558)
            {
                Messenger.Default.Send<string>("WY08", "SetZeroMessage");
            }
            else if (i == 8559)
            {
                Messenger.Default.Send<string>("WY09", "SetZeroMessage");
            }
            else if (i == 8560)
            {
                Messenger.Default.Send<string>("WY10", "SetZeroMessage");
            }
            else if (i == 8561)
            {
                Messenger.Default.Send<string>("WY11", "SetZeroMessage");
            }
            else if (i == 8562)
            {
                Messenger.Default.Send<string>("WY12", "SetZeroMessage");
            }
            else if (i == 8563)
            {
                Messenger.Default.Send<string>("CY01", "SetZeroMessage");
            }
            else if (i == 8564)
            {
                Messenger.Default.Send<string>("CY02", "SetZeroMessage");
            }
            else if (i == 8565)
            {
                Messenger.Default.Send<string>("CY03", "SetZeroMessage");
            }
            else if (i == 8566)
            {
                Messenger.Default.Send<string>("FS01", "SetZeroMessage");
            }
            else if (i == 8567)
            {
                Messenger.Default.Send<string>("FS02", "SetZeroMessage");
            }
            else if (i == 8568)
            {
                Messenger.Default.Send<string>("FS03", "SetZeroMessage");
            }
            else if (i == 8569)
            {
                Messenger.Default.Send<string>("SLS", "SetZeroMessage");
            }


            #region 管理员登录



            //管理员登录
            else if (i == 8888)
            {
                if (PublicData.Dev.PassWord == "brt12345678")
                {
                    PublicData.Dev.IsAdmin = true;
                    MessageBox.Show("管理员登录成功，配置完成后请退出登录或重启软件！");
                }
                else
                {
                    PublicData.Dev.IsAdmin = false;
                    MessageBox.Show("管理员密码错误！");
                }
            }
            //管理员退出登录
            else if (i == 8889)
            {
                PublicData.Dev.PassWord = "";
                Messenger.Default.Send<string>("", "PasswordChanged");
                PublicData.Dev.IsAdmin = false;
            }

            #endregion
        }

        #endregion


        #region 试验管理相关指令、消息

        #region 试验管理相关属性

        /// <summary>
        /// 试验列表Table
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数DataTable _expListTable = new MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数DataTable();
        /// <summary>
        /// 试验列表Table
        /// </summary>
        public MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数DataTable ExpListTable
        {
            get { return _expListTable; }
            set
            {
                _expListTable = value;
                RaisePropertyChanged(() => ExpListTable);
            }
        }

        /// <summary>
        /// A00检测试验参数TableAdapter
        /// </summary>
        private MQZH_DB_TestDataSetTableAdapters.A00试验参数TableAdapter _a00TableAdapter = new MQZH_DB_TestDataSetTableAdapters.A00试验参数TableAdapter();

        /// <summary>
        /// A00检测试验参数TableAdapter
        /// </summary>
        private MQZH_DB_TestDataSetTableAdapters.A00试验参数TableAdapter A00TableAdapter
        {
            get { return _a00TableAdapter; }
            set
            {
                _a00TableAdapter = value;
                RaisePropertyChanged(() => A00TableAdapter);
            }
        }

        /// <summary>
        /// 列表中选中的试验索引
        /// </summary>
        private int _selectedIndex = 0;
        /// <summary>
        /// 列表中选中的试验索引
        /// </summary>
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                RaisePropertyChanged(() => SelectedIndex);
            }
        }

        /// <summary>
        /// 列表中选中的试验编号
        /// </summary>
        private string _selectedExpNOStr = "";
        /// <summary>
        /// 列表中选中的试验编号
        /// </summary>
        public string SelectedExpNOStr
        {
            get { return _selectedExpNOStr; }
            set
            {
                _selectedExpNOStr = value;
                RaisePropertyChanged(() => SelectedExpNOStr);
            }
        }

        /// <summary>
        /// 将要复制的测试新试验编号
        /// </summary>
        private string _expNOCopyNew = "";
        /// <summary>
        /// 将要复制的测试新试验编号
        /// </summary>
        public string ExpNOCopyNew
        {
            get { return _expNOCopyNew; }
            set
            {
                _expNOCopyNew = value;
                RaisePropertyChanged(() => ExpNOCopyNew);
            }
        }

        /// <summary>
        /// 将要新增的试验编号
        /// </summary>
        private string _expNONew = "";
        /// <summary>
        /// 将要新增的试验编号
        /// </summary>
        public string ExpNONew
        {
            get { return _expNONew; }
            set
            {
                _expNONew = value;
                RaisePropertyChanged(() => ExpNONew);
            }
        }

        #endregion

        /// <summary>
        ///试验增、删、载入按钮指令
        /// </summary>
        private RelayCommand<String> _expManCommand;
        /// <summary>
        /// 试验增、删、载入按钮指令
        /// </summary>
        public RelayCommand<String> ExpManCommand
        {
            get
            {
                if (_expManCommand == null)
                    _expManCommand = new RelayCommand<String>((p) => ExecuteExpManCommand(p));
                return _expManCommand;
            }
            set { _expManCommand = value; }
        }


        /// <summary>
        /// 试验管理指令回调
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecuteExpManCommand(String num)
        {
            int i = Convert.ToInt16(num);
            //新增
            if (i == 1)
            {
                Messenger.Default.Send<string>(ExpNONew, "NewExpMessage");
            }

            //删除
            else if (i == 2)
            {
                Messenger.Default.Send<string>(SelectedExpNOStr, "DelExpByName");
            }

            //载入选中的试验
            else if (i == 3)
            {
                //装置忙，无法载入
                if (PublicData.Dev.IsDeviceBusy)
                {
                    MessageBox.Show("装置忙，请先停止正在运行的试验或退出软件后重新打开！", "错误提示");
                }
                else
                {
                    Messenger.Default.Send<string>(SelectedExpNOStr, "LoadExpByName");
                }
            }

            //复制选中的测试试验
            else if (i == 6)
            {
                if (ExpNOCopyNew != "")
                {
                    string[] msg = new String[2] { SelectedExpNOStr, ExpNOCopyNew };
                    Messenger.Default.Send<string[]>(msg, "CopyExpMessage");
                }
            }

            //打开试验设定窗口
            else if (i == 4)
            {
                //装置忙，无法载入
                if (PublicData.Dev.IsDeviceBusy)
                {
                    MessageBox.Show("装置忙，请先停止正在运行的试验或退出软件后重新打开！", "错误提示");
                }
                else
                {
                    Messenger.Default.Send<string>(MQZH_WinName.ExpSetWinName, "OpenGivenNameWin");
                }
            }
            //关闭当前试验
            else if (i == 5)
            {
                MessageBoxResult msgBoxResult = MessageBox.Show("确认关闭当前试验并载入默认试验？", "提示", MessageBoxButton.YesNo);
                if (msgBoxResult == MessageBoxResult.Yes)
                {
                    //装置忙
                    if (PublicData.Dev.IsDeviceBusy)
                    {
                        MessageBox.Show("装置忙，请先停止正在运行的试验或退出软件后重新打开！", "错误提示");
                    }
                    else
                    {
                        Messenger.Default.Send<string>("DefaultExp", "LoadExpByName");
                    }
                }
            }
            //保存当前试验设置
            else if (i == 11)
            {
                Messenger.Default.Send<string>("SaveExpAll", "SaveExpMessage");
                //  MessageBox.Show("修改关键参数，需要退出后重新打开软件才能生效！", "提示", MessageBoxButton.OK);
            }
            //取消修改试验设置
            else if (i == 12)
            {
                Messenger.Default.Send<string>(PublicData.ExpDQ.ExpSettingParam.ExpNO, "LoadExpByName");
            }
        }


        /// <summary>
        ///更新试验列表消息消息回调
        /// </summary>
        /// <param name="msg"></param>
        private void ExpTableChangedMessage(MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数DataTable msg)
        {
            ExpListTable = (MQZH_DB_TestDataSet.A00试验参数DataTable)msg.Copy();
        }

        #endregion



        #region 提示信息窗口

        /// <summary>
        /// 提示信息
        /// </summary>
        private string _promptStr;
        /// <summary>
        /// 提示信息
        /// </summary>
        public string PromptStr
        {
            get { return _promptStr; }
            set
            {
                _promptStr = value;
                RaisePropertyChanged(() => PromptStr);
            }
        }

        /// <summary>
        /// 提示信息窗口指令
        /// </summary>
        private RelayCommand<String> _promptCommand;
        /// <summary>
        /// 提示信息窗口指令
        /// </summary>
        public RelayCommand<String> PromptCommand
        {
            get
            {
                if (_promptCommand == null)
                    _promptCommand = new RelayCommand<String>((p) => ExecutePromptCMD(p));
                return _promptCommand;

            }
            set { _expDataCommand = value; }
        }

        /// <summary>
        /// 提示信息窗口操作
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecutePromptCMD(String num)
        {
            Messenger.Default.Send<string>(MQZH_WinName.PromptWinName, "CloseGivenNameWin");
        }


        /// <summary>
        /// 打开提示窗口消息回调
        /// </summary>
        /// <param name="msg"></param>
        private void OpenPromptMessage(string msg)
        {
            PromptStr = msg.Clone().ToString();
            Messenger.Default.Send<string>(MQZH_WinName.PromptWinName, "OpenGivenNameWin");
        }

        #endregion

        #endregion


        #region 管理员登录消息

        /// <summary>
        /// 管理员登录消息
        /// </summary>
        /// <param name="msg"></param>
        private void AdminLogginMessage(string msg)
        {
            if (PublicData.Dev.PassWord == "brt12345678")
            {
                PublicData.Dev.IsAdmin = true;
                MessageBox.Show("管理员登录成功，配置完成后请退出登录或重启软件！");
            }
            else
            {
                PublicData.Dev.IsAdmin = false;
                MessageBox.Show("管理员密码错误！");
            }
        }
        #endregion
    }
}