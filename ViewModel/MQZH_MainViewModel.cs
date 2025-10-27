/************************************************************************************
 * 描述：
 * ViewModel
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2024/2/4       16:00:44		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using MQDFJ_MB.Model.DEV;
using MQDFJ_MB.Communication;
using MQDFJ_MB.Model.Exp;
using System.Windows;
using MQDFJ_MB.BLL;
using GalaSoft.MvvmLight.Messaging;
using System;
using GalaSoft.MvvmLight.Command;
using MQDFJ_MB.DAL;
using MQDFJ_MB.Model;
using System.Collections.Generic;
using ChartModel;
using System.Drawing;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System.Linq;
using static MQDFJ_MB.Model.MQZH_Enums;

namespace MQDFJ_MB.ViewModel
{
    public partial class MQZH_MainViewModel : ViewModelBase
    {
        /// <summary>
        /// 主窗口ViewModel
        /// </summary>
        public MQZH_MainViewModel()
        {
            //有窗口新增或关闭
            Messenger.Default.Register<string>(this, "WindowClosed", WindowClosedMessage);
            Messenger.Default.Register<Window>(this, "NewWindow", WindowAddedMessage);

            //试验列表已更新
            Messenger.Default.Register<MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数DataTable>(this, "ExpTableChanged", ExpTableChangedMessage);

            //订阅绘图数据更新消息
            Messenger.Default.Register<string>(this, "UpdateChart", UpdateChartMessage);

            //装置、试验初始化
            Dev = new MQZH_DevModel_Main();
            ExpDQ = new MQZH_ExpTotallModel();

            //DAL初始化及赋值
            MainViewModel_DevDAL = new MQZH_DevDAL(Dev);
            MainViewModel_ExpDAL = new MQZH_ExpDAL(ExpDQ);
            MainViewModel_RepDAL = new MQZH_RepDAL(Dev, ExpDQ);

            //载入装置参数
            Messenger.Default.Send<string>("LoadDevSettings", "DevDataRWMessage");
            //载入试验参数及数据
            if (!Dev.IsLoadLastExpPowerOn)
            {
                Messenger.Default.Send<string>("DefaultExp", "LoadExpByName");
            }
            else
            {
                Messenger.Default.Send<string>(Dev.ExpNOLast, "LoadExpByName");
            }

            //通讯初始化
            MainViewModel_Communication = new MQZH_Communication(Dev);
            MainViewModel_Communication.CommunicationInit();

            //主控BLL初始化
            MainViewModel_Bll = new MQZH_ExpBLL(Dev, ExpDQ);

            Dev.PID_SMSLL = MainViewModel_Bll.BllDev.PID_SMSLL;

            //绘图初始化
            PlotInit();
            NDPlotInit();


            //打开提示窗口信息
            Messenger.Default.Register<string>(this, "OpenPrompt", OpenPromptMessage);


            //注册管理员登录消息
            Messenger.Default.Register<string>(this, "AdminLogginMessage", AdminLogginMessage);
        }


        #region 装置、通讯、试验、数据操作属性

        /// <summary>
        /// 装置参数
        /// </summary>
        private MQZH_DevModel_Main _dev;
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
        private MQZH_ExpTotallModel _expDQ;
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
                ExpDQ.QM_Evaluate();
                ExpDQ.SM_Evaluate();
                ExpDQ.KFY_Evaluate();
                Messenger.Default.Send<string>("SaveQMStatusAndData", "SaveExpMessage");
                Messenger.Default.Send<string>("SaveSMStatusAndData", "SaveExpMessage");
                Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");

                NDPlotUpdate();
            }

            //重新计算并保存层间变形检测数据
            else if (i == 5)
            {
                ExpDQ.CJBX_DJEvaluate();
                ExpDQ.CJBX_GCEvaluate();

                Messenger.Default.Send<string>("SaveCJBXStatusAndData", "SaveExpMessage");
            }

            //打开水密定级检测渗漏窗口
            else if (i == 21)
            {
                ExpDQ.ExpData_SM.SLStatusCopy();
                Messenger.Default.Send<string>(MQZH_WinName.SMDJDamageWinName, "OpenGivenNameWin");
            }
            //打开水密工程检测渗漏窗口
            else if (i == 22)
            {
                ExpDQ.ExpData_SM.SLStatusCopy();
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
                if (Dev.PassWord == "brt12345678")
                {
                    Dev.IsAdmin = true;
                    MessageBox.Show("管理员登录成功，配置完成后请退出登录或重启软件！");
                }
                else
                {
                    Dev.IsAdmin = false;
                    MessageBox.Show("管理员密码错误！");
                }
            }
            //管理员退出登录
            else if (i == 8889)
            {
                Dev.PassWord = "";
                Messenger.Default.Send<string>("", "PasswordChanged");
                Dev.IsAdmin = false;
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
        /// 试验增、删、载入按钮指令
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
                if (Dev.IsDeviceBusy)
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
                if (Dev.IsDeviceBusy)
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
                    if (Dev.IsDeviceBusy)
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
                Messenger.Default.Send<string>(ExpDQ.ExpSettingParam.ExpNO, "LoadExpByName");
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


        #region 主试验窗口管理相关指令

        /// <summary>
        /// 窗口管理指令
        /// </summary>
        private RelayCommand<String> _winManCommand;
        /// <summary>
        /// 窗口管理指令
        /// </summary>
        public RelayCommand<String> WinManCommand
        {
            get
            {
                if (_winManCommand == null)
                    _winManCommand = new RelayCommand<String>((p) => ExecuteWinManCommand(p));
                return _winManCommand;
            }
            set { _winManCommand = value; }
        }


        /// <summary>
        /// 试验管理指令回调
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecuteWinManCommand(String num)
        {
            int i = Convert.ToInt16(num);
            //试验管理窗口
            if (i == 7103)
            {
                Messenger.Default.Send<string>(MQZH_WinName.ExpManWinName, "OpenGivenNameWin");
            }

            //试验参数窗口
            else if (i == 7102)
            {
                Messenger.Default.Send<string>(MQZH_WinName.ExpSetWinName, "OpenGivenNameWin");
            }

            //数据报告窗口
            else if (i == 7104)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CurrentDataWinName, "OpenGivenNameWin");
            }

            //气密检测窗口
            else if (i == 7105)
            {
                Messenger.Default.Send<string>(MQZH_WinName.QMWinName, "OpenGivenNameWin");
            }

            //抗风压P1检测窗口
            else if (i == 7106)
            {
                Messenger.Default.Send<string>(MQZH_WinName.KFYp1Name, "OpenGivenNameWin");
            }

            //水密检测窗口
            else if (i == 7107)
            {
                Messenger.Default.Send<string>(MQZH_WinName.SMWinName, "OpenGivenNameWin");
            }

            //抗风压P2检测窗口
            else if (i == 7108)
            {
                Messenger.Default.Send<string>(MQZH_WinName.KFYp2Name, "OpenGivenNameWin");
            }

            //抗风压P3检测窗口
            else if (i == 7109)
            {
                Messenger.Default.Send<string>(MQZH_WinName.KFYp3Name, "OpenGivenNameWin");
            }


            //抗风压Pmax检测窗口
            else if (i == 7110)
            {
                Messenger.Default.Send<string>(MQZH_WinName.KFYpmaxName, "OpenGivenNameWin");
            }


            //层间变形检测窗口
            else if (i == 7111)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CJBXWinName, "OpenGivenNameWin");
            }


            //水密定级渗漏窗口
            else if (i == 7112)
            {
                Messenger.Default.Send<string>(MQZH_WinName.SMDJDamageWinName, "OpenGivenNameWin");
            }


            //水密工程渗漏窗口
            else if (i == 7113)
            {
                Messenger.Default.Send<string>(MQZH_WinName.SMGCDamageWinName, "OpenGivenNameWin");
            }

            //抗风压定级渗漏窗口
            else if (i == 7114)
            {
                Messenger.Default.Send<string>(MQZH_WinName.KFYDJDamageWinName, "OpenGivenNameWin");
            }

            //抗风压工程渗漏窗口
            else if (i == 7115)
            {
                Messenger.Default.Send<string>(MQZH_WinName.KFYGCDamageWinName, "OpenGivenNameWin");
            }

            //层间定级损坏窗口
            else if (i == 7116)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CJBXDJDamageWinName, "OpenGivenNameWin");
            }

            //层间工程损坏窗口
            else if (i == 7117)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CJBXGCDamageWinName, "OpenGivenNameWin");
            }

            //调试窗口
            else if (i == 7118)
            {
                Messenger.Default.Send<string>(MQZH_WinName.DbgWinName, "OpenGivenNameWin");
            }

            //公司信息窗口
            else if (i == 7123)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CoInfoSetWinName, "OpenGivenNameWin");
            }

            //装置基本信息窗口
            else if (i == 7124)
            {
                Messenger.Default.Send<string>(MQZH_WinName.DevInfoSetWinName, "OpenGivenNameWin");
            }

            //装置基本参数窗口
            else if (i == 7125)
            {
                Messenger.Default.Send<string>(MQZH_WinName.DevParamSetWinName, "OpenGivenNameWin");
            }

            //通讯设置窗口
            else if (i == 7126)
            {
                Messenger.Default.Send<string>(MQZH_WinName.ComSetWinName, "OpenGivenNameWin");
            }

            //传感器设置窗口
            else if (i == 7127)
            {
                Messenger.Default.Send<string>(MQZH_WinName.SenssorSetWinName, "OpenGivenNameWin");
            }

            //调零标定窗口
            else if (i == 7128)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CorrWinName, "OpenGivenNameWin");
            }

            //PID参数窗口
            else if (i == 7129)
            {
                Messenger.Default.Send<string>(MQZH_WinName.PidSetWinName, "OpenGivenNameWin");
            }

            //压力设定窗口
            else if (i == 7130)
            {
                Messenger.Default.Send<string>(MQZH_WinName.PressSetWinName, "OpenGivenNameWin");
            }

            //位移设定窗口
            else if (i == 7131)
            {
                Messenger.Default.Send<string>(MQZH_WinName.WYSetWinName, "OpenGivenNameWin");
            }

            //压力控制窗口
            else if (i == 7132)
            {
                Messenger.Default.Send<string>(MQZH_WinName.PressCtlWinName, "OpenGivenNameWin");
            }

            //位移控制窗口
            else if (i == 7133)
            {
                Messenger.Default.Send<string>(MQZH_WinName.WYCtlWinName, "OpenGivenNameWin");
            }
        }


        #endregion


        #region 手动单点、指令及回调

        /// <summary>
        /// 传递按钮指令
        /// </summary>
        private RelayCommand<String> buttonDDCommand;
        /// <summary>
        /// 传递按钮指令
        /// </summary>
        public RelayCommand<String> ButtonDDCommand
        {
            get
            {
                if (buttonDDCommand == null)
                    buttonDDCommand = new RelayCommand<String>((p) => ExecuteButtonDDCMD(p));
                return buttonDDCommand;

            }
            set { buttonDDCommand = value; }
        }


        /// <summary>
        /// 单点控制按钮指令回调
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecuteButtonDDCMD(String num)
        {
            int i = Convert.ToInt16(num);

            //紧急停机
            if (i == 119)
            {
                Dev.DeviceRunMode = DevicRunModeType.JJTJ_Mode;
                Messenger.Default.Send<string>("JJTJ", "JJTJMessage");
            }

            //按钮单点模式开
            else if (i == 5101)
            {
                Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.SDDDDbg_Mode, "ModeChangeMessage");
            }
            //全部停机/待机模式
            else if (i == 5000)
            {
                Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.Wait_Mode, "ModeChangeMessage");
            }
            else if ((i >= 6100) && (i <= 6124))
            {
                Messenger.Default.Send<int>(i, "SDDDMessage");
            }
        }

        #endregion


        #region 手动调速相关参数、指令及回调

        /// <summary>
        /// 手动调速蝶阀1打开
        /// </summary>
        private bool _sdtsDF1=true;
        /// <summary>
        /// 手动调速蝶阀1打开
        /// </summary>
        public bool SdtsDF1
        {
            get { return _sdtsDF1; }
            set
            {
                _sdtsDF1 = value;
                RaisePropertyChanged(() => SdtsDF1);
            }
        }

        /// <summary>
        /// 手动调速蝶阀2打开
        /// </summary>
        private bool _sdtsDF2 = false;
        /// <summary>
        /// 手动调速蝶阀2打开
        /// </summary>
        public bool SdtsDF2
        {
            get { return _sdtsDF2; }
            set
            {
                _sdtsDF2 = value;
                RaisePropertyChanged(() => SdtsDF2);
            }
        }

        /// <summary>
        /// 手动调速蝶阀3打开
        /// </summary>
        private bool _sdtsDF3 = false;
        /// <summary>
        /// 手动调速蝶阀3打开
        /// </summary>
        public bool SdtsDF3
        {
            get { return _sdtsDF3; }
            set
            {
                _sdtsDF3 = value;
                RaisePropertyChanged(() => SdtsDF3);
            }
        }

        /// <summary>
        /// 手动调速蝶阀4打开
        /// </summary>
        private bool _sdtsDF4 = false;
        /// <summary>
        /// 手动调速蝶阀4打开
        /// </summary>
        public bool SdtsDF4
        {
            get { return _sdtsDF4; }
            set
            {
                _sdtsDF4 = value;
                RaisePropertyChanged(() => SdtsDF4);
            }
        }

        /// <summary>
        /// 风机频率手动调速值
        /// </summary>
        private double _pl1_SDTS = 0.0;
        /// <summary>
        /// 风机频率手动调速值
        /// </summary>
        public double PL1_SDTS
        {
            get { return _pl1_SDTS; }
            set
            {
                _pl1_SDTS = value;
                RaisePropertyChanged(() => PL1_SDTS);
            }
        }

        /// <summary>
        /// 水泵频率手动调速值
        /// </summary>
        private double _pl2_SDTS = 0.0;
        /// <summary>
        /// 水泵频率手动调速值
        /// </summary>
        public double PL2_SDTS
        {
            get { return _pl2_SDTS; }
            set
            {
                _pl2_SDTS = value;
                RaisePropertyChanged(() => PL2_SDTS);
            }
        }

        /// <summary>
        /// 传递按钮指令
        /// </summary>
        private RelayCommand<String> sdTSCommand;
        /// <summary>
        /// 传递按钮指令
        /// </summary>
        public RelayCommand<String> SDTSCommand
        {
            get
            {
                if (sdTSCommand == null)
                    sdTSCommand = new RelayCommand<String>((p) => ExecuteSDTSCMD(p));
                return sdTSCommand;

            }
            set { sdTSCommand = value; }
        }

        /// <summary>
        /// 手动调速按钮指令回调
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecuteSDTSCMD(String num)
        {
            int i = Convert.ToInt16(num);

            //紧急停机
            if (i == 119)
            {
                Dev.DeviceRunMode = DevicRunModeType.JJTJ_Mode;
                Messenger.Default.Send<string>("JJTJ", "JJTJMessage");
            }

            //正压手动调速模式
            else if (i == 5102)
            {
                Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.SDZTSDbg_Mode, "ModeChangeMessage");
            }

            //负压手动调速模式
            else if (i == 5103)
            {
                Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.SDFTSDbg_Mode, "ModeChangeMessage");
            }

            //水泵手动调速模式
            else if (i == 5104)
            {
                Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.SDSBTSDbg_Mode, "ModeChangeMessage");
            }

            //停机
            else if (i == 6288)
            {
                PL1_SDTS = 0;
                PL2_SDTS = 0;
                ushort[] outData = new ushort[] { 0, 0, 0, 0, Convert.ToUInt16(PL1_SDTS * 640) };
                Messenger.Default.Send<ushort[]>(outData, "SDTSMessage");
                SdtsDF1 = false;
                SdtsDF2 = false;
                SdtsDF3 = false;
                SdtsDF4 = false;
            }

            //风机手动调速频率、蝶阀
            else if (i == 6201)
            {
                if (PL1_SDTS < 0)
                    PL1_SDTS = 0;
                if (PL1_SDTS > 50)
                    PL1_SDTS = 50;
                ushort[] outData=new ushort[]{0,0,0,0, Convert.ToUInt16(PL1_SDTS * 640) };
                outData[0] = SdtsDF1 ? (ushort)1 : (ushort)0;
                outData[1] = SdtsDF2 ? (ushort)1 : (ushort)0;
                outData[2] = SdtsDF3 ? (ushort)1 : (ushort)0;
                outData[3] = SdtsDF4 ? (ushort)1 : (ushort)0;
                Messenger.Default.Send<ushort[]>(outData, "SDTSMessage");
            }


            //水泵手动调速频率、蝶阀
            else if (i == 6202)
            {
                if (PL2_SDTS < 0)
                    PL2_SDTS = 0;
                if (PL2_SDTS > 50)
                    PL2_SDTS = 50;
                ushort[] outData = new ushort[] { 0, 0, 0, 0, Convert.ToUInt16(PL2_SDTS * 640) };
                Messenger.Default.Send<ushort[]>(outData, "SDTSMessage");
            }
        }

        #endregion


        #region 手动气密PID调试指令及回调

        /// <summary>
        /// 手动气密参数数据数组
        /// </summary>
        private double[] _sdqmDoubleOrderArray = Enumerable.Repeat((double)0, 20).ToArray();
        /// <summary>
        /// 手动气密参数数据数组
        /// </summary>
        public double[] SDQMDoubleOrderArray
        {
            get { return _sdqmDoubleOrderArray; }
            set
            {
                _sdqmDoubleOrderArray = value;
                RaisePropertyChanged(() => SDQMDoubleOrderArray);
            }
        }

        /// <summary>
        /// 手动气密调试指令
        /// </summary>
        private RelayCommand<String> _sdQMCommand;
        /// <summary>
        /// 手动气密调试指令
        /// </summary>
        public RelayCommand<String> SDQMCommand
        {
            get
            {
                if (_sdQMCommand == null)
                    _sdQMCommand = new RelayCommand<String>((p) => ExecuteSDQMCMD(p));
                return _sdQMCommand;

            }
            set { _sdQMCommand = value; }
        }

        /// <summary>
        /// 手动气密PID调试指令回调
        /// </summary>
        private void ExecuteSDQMCMD(String num)
        {
            int i = Convert.ToInt16(num);
            //紧急停机
            if (i == 119)
            {
                Dev.DeviceRunMode = DevicRunModeType.JJTJ_Mode;
                Messenger.Default.Send<string>("JJTJ", "JJTJMessage");
            }
            //停机
            else if (i == 6388)
            {
                SDQMDoubleOrderArray = new double[20];
                SDQMDoubleOrderArray = Enumerable.Repeat((double)0, 20).ToArray();
                SDQMDoubleOrderArray[3] = 32000;//输出上限
                SDQMDoubleOrderArray[4] = 0;//输出下限
                SDQMDoubleOrderArray[5] = 0;//kp
                SDQMDoubleOrderArray[6] = 0;//ki
                SDQMDoubleOrderArray[7] = 0;//kd
                SDQMDoubleOrderArray[8] = 0;//给定值
                SDQMDoubleOrderArray[10] = 0;//PID使能
                Messenger.Default.Send<double[]>(SDQMDoubleOrderArray, "SDQMMessage");
            }
            //发送数据
            else if (i == 6301)
            {

                SDQMDoubleOrderArray = new double[20];
                SDQMDoubleOrderArray = Enumerable.Repeat((double)0, 20).ToArray();
                SDQMDoubleOrderArray[3] = 32000;//输出上限
                SDQMDoubleOrderArray[4] = 0;//输出下限
                SDQMDoubleOrderArray[5] = QM_kp;//kp
                SDQMDoubleOrderArray[6] = QM_ki;//ki
                SDQMDoubleOrderArray[7] = QM_kd;//kd
                if ((Dev.DeviceRunMode == DevicRunModeType.DF1QMFDbg_Mode) ||
                    (Dev.DeviceRunMode == DevicRunModeType.DF2QMFDbg_Mode) ||
                    (Dev.DeviceRunMode == DevicRunModeType.DF3QMFDbg_Mode) ||
                    (Dev.DeviceRunMode == DevicRunModeType.DF4QMFDbg_Mode))
                {
                    QM_Press = -Math.Abs(QM_Press);
                    SDQMDoubleOrderArray[8] = -Math.Abs(QM_Press); //给定值
                }
                else
                {
                    QM_Press = Math.Abs(QM_Press);
                    SDQMDoubleOrderArray[8] = Math.Abs(QM_Press);//给定值
                }
                SDQMDoubleOrderArray[10] = 1;//PID使能
                Messenger.Default.Send<double[]>(SDQMDoubleOrderArray, "SDQMMessage");
            }

            //手动气密风速管1正压模式
            else if (i == 5105)
            {
                switch (Dev.DFNo_FG1)
                {
                    case 1:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF1QMZDbg_Mode, "ModeChangeMessage");
                        break;
                    case 2:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF2QMZDbg_Mode, "ModeChangeMessage");
                        break;
                    case 3:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF3QMZDbg_Mode, "ModeChangeMessage");
                        break;
                    case 4:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF4QMZDbg_Mode, "ModeChangeMessage");
                        break;
                    default:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF2QMZDbg_Mode, "ModeChangeMessage");
                        break;
                }
            }
            //手动气密风速管1负压模式
            else if (i == 5106)
            {
                switch (Dev.DFNo_FG1)
                {
                    case 1:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF1QMFDbg_Mode, "ModeChangeMessage");
                        break;
                    case 2:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF2QMFDbg_Mode, "ModeChangeMessage");
                        break;
                    case 3:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF3QMFDbg_Mode, "ModeChangeMessage");
                        break;
                    case 4:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF4QMFDbg_Mode, "ModeChangeMessage");
                        break;
                    default:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF2QMFDbg_Mode, "ModeChangeMessage");
                        break;
                }
            }

            //手动气密风速管2正压模式
            else if (i == 5107)
            {
                switch (Dev.DFNo_FG2)
                {
                    case 1:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF1QMZDbg_Mode, "ModeChangeMessage");
                        break;
                    case 2:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF2QMZDbg_Mode, "ModeChangeMessage");
                        break;
                    case 3:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF3QMZDbg_Mode, "ModeChangeMessage");
                        break;
                    case 4:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF4QMZDbg_Mode, "ModeChangeMessage");
                        break;
                    default:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF2QMZDbg_Mode, "ModeChangeMessage");
                        break;
                }
            }
            //手动气密风速管2负压模式
            else if (i == 5108)
            {
                switch (Dev.DFNo_FG2)
                {
                    case 1:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF1QMFDbg_Mode, "ModeChangeMessage");
                        break;
                    case 2:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF2QMFDbg_Mode, "ModeChangeMessage");
                        break;
                    case 3:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF3QMFDbg_Mode, "ModeChangeMessage");
                        break;
                    case 4:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF4QMFDbg_Mode, "ModeChangeMessage");
                        break;
                    default:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF2QMFDbg_Mode, "ModeChangeMessage");
                        break;
                }
            }


            //手动气密风速管3正压模式
            else if (i == 5109)
            {
                switch (Dev.DFNo_FG3)
                {
                    case 1:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF1QMZDbg_Mode, "ModeChangeMessage");
                        break;
                    case 2:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF2QMZDbg_Mode, "ModeChangeMessage");
                        break;
                    case 3:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF3QMZDbg_Mode, "ModeChangeMessage");
                        break;
                    case 4:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF4QMZDbg_Mode, "ModeChangeMessage");
                        break;
                    default:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF2QMZDbg_Mode, "ModeChangeMessage");
                        break;
                }
            }
            //手动气密风速管3负压模式
            else if (i == 5110)
            {
                switch (Dev.DFNo_FG3)
                {
                    case 1:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF1QMFDbg_Mode, "ModeChangeMessage");
                        break;
                    case 2:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF2QMFDbg_Mode, "ModeChangeMessage");
                        break;
                    case 3:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF3QMFDbg_Mode, "ModeChangeMessage");
                        break;
                    case 4:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF4QMFDbg_Mode, "ModeChangeMessage");
                        break;
                    default:
                        Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.DF2QMFDbg_Mode, "ModeChangeMessage");
                        break;
                }
            }

            //气密调试压力曲线
            else if (i == 7119)
            {
                Messenger.Default.Send<string>(MQZH_WinName.SDQMPlotWinName, "OpenGivenNameWin");
            }
        }

        /// <summary>
        /// 手动气密给定压力
        /// </summary>
        private double _qm_Press = 0;
        /// <summary>
        /// 手动气密给定压力
        /// </summary>
        public double QM_Press
        {
            get { return _qm_Press; }
            set
            {
                _qm_Press = value;
                RaisePropertyChanged(() => QM_Press);
            }
        }

        /// <summary>
        /// 手动手动气密控制kp
        /// </summary>
        private double _qm_kp = 0;
        /// <summary>
        /// 手动气密控制kp
        /// </summary>
        public double QM_kp
        {
            get { return _qm_kp; }
            set
            {
                _qm_kp = Math.Abs(value);
                RaisePropertyChanged(() => QM_kp);
            }
        }

        /// <summary>
        /// 手动气密控制ki
        /// </summary>
        private double _qm_ki = 0;
        /// <summary>
        /// 手动气密控制ki
        /// </summary>
        public double QM_ki
        {
            get { return _qm_ki; }
            set
            {
                _qm_ki = Math.Abs(value);
                RaisePropertyChanged(() => QM_ki);
            }
        }

        /// <summary>
        /// 手动气密控制kd
        /// </summary>
        private double _qm_kd = 0;
        /// <summary>
        /// 气密控制kd
        /// </summary>
        public double QM_kd
        {
            get { return _qm_kd; }
            set
            {
                _qm_kd = Math.Abs(value);
                RaisePropertyChanged(() => QM_kd);
            }
        }
        #endregion


        #region 手动大压力PID调试指令及回调

        /// <summary>
        /// 手动水密1参数数据数组
        /// </summary>
        private double[] _sdsm1DoubleOrderArray = Enumerable.Repeat((double)0, 20).ToArray();
        /// <summary>
        /// 手动水密1参数数据数组
        /// </summary>
        public double[] SDSM1DoubleOrderArray
        {
            get { return _sdsm1DoubleOrderArray; }
            set
            {
                _sdsm1DoubleOrderArray = value;
                RaisePropertyChanged(() => SDSM1DoubleOrderArray);
            }
        }

        /// <summary>
        /// 手动水密调试指令
        /// </summary>
        private RelayCommand<String> _sdSMCommand;
        /// <summary>
        /// 手动水密调试指令
        /// </summary>
        public RelayCommand<String> SDSMCommand
        {
            get
            {
                if (_sdSMCommand == null)
                    _sdSMCommand = new RelayCommand<String>((p) => ExecuteSDSMCMD(p));
                return _sdSMCommand;

            }
            set { _sdSMCommand = value; }
        }

        /// <summary>
        /// 手动水密指令回调
        /// </summary>
        private void ExecuteSDSMCMD(String num)
        {
            int i = Convert.ToInt16(num);
            //紧急停机
            if (i == 119)
            {
                Dev.DeviceRunMode = DevicRunModeType.JJTJ_Mode;
                Messenger.Default.Send<string>("JJTJ", "JJTJMessage");
            }
            //停机
            else if (i == 6488)
            {
                SDSM1DoubleOrderArray = new double[20];
                SDSM1DoubleOrderArray = Enumerable.Repeat((double)0, 20).ToArray();
                SDSM1DoubleOrderArray[3] = 32000; //输出上限
                SDSM1DoubleOrderArray[4] = 0; //输出下限
                SDSM1DoubleOrderArray[5] = 0; //kp
                SDSM1DoubleOrderArray[6] = 0; //ki
                SDSM1DoubleOrderArray[7] = 0; //kd
                SDSM1DoubleOrderArray[8] = 0; //给定值
                SDSM1DoubleOrderArray[10] = 0; //PID使能
                Messenger.Default.Send<double[]>(SDSM1DoubleOrderArray, "SDSMMessage");
            }
            //发送数据
            else if (i == 6401)
            {

                SDSM1DoubleOrderArray = new double[20];
                SDSM1DoubleOrderArray = Enumerable.Repeat((double)0, 20).ToArray();
                SDSM1DoubleOrderArray[3] = 32000;//输出上限
                SDSM1DoubleOrderArray[4] = 0;//输出下限
                SDSM1DoubleOrderArray[5] = SM1_kp;//kp
                SDSM1DoubleOrderArray[6] = SM1_ki;//ki
                SDSM1DoubleOrderArray[7] = SM1_kd;//kd
                if (Dev.DeviceRunMode == DevicRunModeType.SDSMFDbg_Mode)
                {
                    SM1_Press = -Math.Abs(SM1_Press);
                    SDSM1DoubleOrderArray[8] = -Math.Abs(SM1_Press); //给定值
                }
                else
                {
                    SM1_Press = Math.Abs(SM1_Press);
                    SDSM1DoubleOrderArray[8] = Math.Abs(SM1_Press);//给定值
                }
                SDSM1DoubleOrderArray[10] = 1;//PID使能
                Messenger.Default.Send<double[]>(SDSM1DoubleOrderArray, "SDSMMessage");
            }
            //手动大风机正压模式
            else if (i == 5112)
            {
                Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.SDSMZDbg_Mode, "ModeChangeMessage");
            }
            //手动大风机负压模式
            else if (i == 5113)
            {
                Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.SDSMFDbg_Mode, "ModeChangeMessage");
            }
            //大压力调试数据曲线
            else if (i == 7120)
            {
                Messenger.Default.Send<string>(MQZH_WinName.SDSMPlotWinName, "OpenGivenNameWin");
            }
        }

        /// <summary>
        /// 水密1给定压力
        /// </summary>
        private double _sm1_Press = 0;
        /// <summary>
        /// 水密1给定压力
        /// </summary>
        public double SM1_Press
        {
            get { return _sm1_Press; }
            set
            {
                _sm1_Press = value;
                RaisePropertyChanged(() => SM1_Press);
            }
        }

        /// <summary>
        /// 水密1控制kp
        /// </summary>
        private double _sm1_kp = 0;
        /// <summary>
        /// 水密1控制kp
        /// </summary>
        public double SM1_kp
        {
            get { return _sm1_kp; }
            set
            {
                _sm1_kp = Math.Abs(value);
                RaisePropertyChanged(() => SM1_kp);
            }
        }

        /// <summary>
        /// 水密1控制ki
        /// </summary>
        private double _sm1_ki = 0;
        /// <summary>
        /// 水密1控制ki
        /// </summary>
        public double SM1_ki
        {
            get { return _sm1_ki; }
            set
            {
                _sm1_ki = Math.Abs(value);
                RaisePropertyChanged(() => SM1_ki);
            }
        }

        /// <summary>
        /// 水密1控制kd
        /// </summary>
        private double _sm1_kd = 0;
        /// <summary>
        /// 水密1控制kd
        /// </summary>
        public double SM1_kd
        {
            get { return _sm1_kd; }
            set
            {
                _sm1_kd = Math.Abs(value);
                RaisePropertyChanged(() => SM1_kd);
            }
        }

        #endregion


        #region 水流量PID调试控制指令及回调

        /// <summary>
        /// 水流量PID参数数据数组
        /// </summary>
        private double[] _sllPIDDoubleOrderArray = Enumerable.Repeat((double)0, 20).ToArray();
        /// <summary>
        /// 水流量PID参数数据数组
        /// </summary>
        public double[] SLLPIDDoubleOrderArray
        {
            get { return _sllPIDDoubleOrderArray; }
            set
            {
                _sllPIDDoubleOrderArray = value;
                RaisePropertyChanged(() => SLLPIDDoubleOrderArray);
            }
        }

        /// <summary>
        /// 水流量PID调试指令
        /// </summary>
        private RelayCommand<String> _sllPIDCommand;
        /// <summary>
        /// 水流量PID调试指令
        /// </summary>
        public RelayCommand<String> SLLPIDCommand
        {
            get
            {
                if (_sllPIDCommand == null)
                    _sllPIDCommand = new RelayCommand<String>((p) => ExecuteSLLPIDCMD(p));
                return _sllPIDCommand;

            }
            set { _sllPIDCommand = value; }
        }

        /// <summary>
        /// 水流量PID指令回调
        /// </summary>
        private void ExecuteSLLPIDCMD(String num)
        {
            int i = Convert.ToInt16(num);

            //紧急停机
            if (i == 119)
            {
                Dev.DeviceRunMode = DevicRunModeType.JJTJ_Mode;
                Messenger.Default.Send<string>("JJTJ", "JJTJMessage");
            }
            //水流量PID模式
            else if (i == 5111)
            {
                Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.SLLPIDDbg_Mode, "ModeChangeMessage");
            }

            //停机
            else if (i == 6688)
            {
                SLLPIDDoubleOrderArray = new double[20];
                SLLPIDDoubleOrderArray = Enumerable.Repeat((double)0, 20).ToArray();
                SLLPIDDoubleOrderArray[3] = 32000;//输出上限
                SLLPIDDoubleOrderArray[4] = 0;//输出下限    
                SLLPIDDoubleOrderArray[5] = 0;//kp
                SLLPIDDoubleOrderArray[6] = 0;//ki
                SLLPIDDoubleOrderArray[7] = 0;//kd
                SLLPIDDoubleOrderArray[8] = 0;//给定值
                SLLPIDDoubleOrderArray[9] = 0;
                SLLPIDDoubleOrderArray[10] = 0;//PID使能
                Messenger.Default.Send<double[]>(SLLPIDDoubleOrderArray, "SLLPIDMessage");
            }
            //发送指令
            else if (i == 6601)
            {

                SLLPIDDoubleOrderArray = new double[20];
                SLLPIDDoubleOrderArray = Enumerable.Repeat((double)0, 20).ToArray();
                SLLPIDDoubleOrderArray[3] = 32000;//输出上限
                SLLPIDDoubleOrderArray[4] = 0;//输出下限
                SLLPIDDoubleOrderArray[5] = SLLPID_kp;//kp
                SLLPIDDoubleOrderArray[6] = SLLPID_ki;//ki
                SLLPIDDoubleOrderArray[7] = SLLPID_kd;//kd
                SLLPIDDoubleOrderArray[8] = SLLPID_LL;//给定值
                SLLPIDDoubleOrderArray[9] = 0;
                SLLPIDDoubleOrderArray[10] = 1;//PID使能
                Messenger.Default.Send<double[]>(SLLPIDDoubleOrderArray, "SLLPIDMessage");
            }


            //水流量调试数据曲线
            else if (i == 7121)
            {
                Messenger.Default.Send<string>(MQZH_WinName.SLLPIDPlotWinName, "OpenGivenNameWin");
            }
        }

        /// <summary>
        /// 水流量PID给定流量
        /// </summary>
        private double _sllPID_LL = 0;
        /// <summary>
        /// 水流量PID给定流量
        /// </summary>
        public double SLLPID_LL
        {
            get { return _sllPID_LL; }
            set
            {
                _sllPID_LL = Math.Abs(value);
                RaisePropertyChanged(() => SLLPID_LL);
            }
        }

        /// <summary>
        /// 水流量PID控制kp
        /// </summary>
        private double _sllPID_kp = 0;
        /// <summary>
        /// 水流量PID控制kp
        /// </summary>
        public double SLLPID_kp
        {
            get { return _sllPID_kp; }
            set
            {
                _sllPID_kp = Math.Abs(value);
                RaisePropertyChanged(() => SLLPID_kp);
            }
        }

        /// <summary>
        /// 水流量PID控制ki
        /// </summary>
        private double _sllPID_ki = 0;
        /// <summary>
        /// 水流量PID控制ki
        /// </summary>
        public double SLLPID_ki
        {
            get { return _sllPID_ki; }
            set
            {
                _sllPID_ki = Math.Abs(value);
                RaisePropertyChanged(() => SLLPID_ki);
            }
        }

        /// <summary>
        /// 水流量PID控制kd
        /// </summary>
        private double _sllPID_kd = 0;
        /// <summary>
        /// 水流量PID控制kd
        /// </summary>
        public double SLLPID_kd
        {
            get { return _sllPID_kd; }
            set
            {
                _sllPID_kd = Math.Abs(value);
                RaisePropertyChanged(() => SLLPID_kd);
            }
        }

        #endregion


        #region 手动位移控制相关参数

        /// <summary>
        /// 发送指令数组
        /// </summary>
        private ushort[] _sdwyOrderArray = Enumerable.Repeat((ushort)0, 20).ToArray();
        /// <summary>
        /// 发送指令数组
        /// </summary>
        public ushort[] SDWYOrderArray
        {
            get { return _sdwyOrderArray; }
            set
            {
                _sdwyOrderArray = value;
                RaisePropertyChanged(() => SDWYOrderArray);
            }
        }

        /// <summary>
        /// 手动位移调试X轴点动时间（ms）（发送时除以100以直接用于定时器）
        /// </summary>
        private int _ddsj_SDWY_X = 500;
        /// <summary>
        /// 手动位移调试X轴点动时间（ms）（发送时除以100以直接用于定时器）
        /// </summary>
        public int DDSJ_SDWY_X
        {
            get { return _ddsj_SDWY_X; }
            set
            {
                _ddsj_SDWY_X = value;
                RaisePropertyChanged(() => DDSJ_SDWY_X);
            }
        }

        /// <summary>
        /// 手动位移调试Y轴点动时间（ms）（发送时除以100以直接用于定时器）
        /// </summary>
        private int _ddsj_SDWY_Y = 500;
        /// <summary>
        /// 手动位移调试Y轴点动时间（ms）（发送时除以100以直接用于定时器）
        /// </summary>
        public int DDSJ_SDWY_Y
        {
            get { return _ddsj_SDWY_Y; }
            set
            {
                _ddsj_SDWY_Y = value;
                RaisePropertyChanged(() => DDSJ_SDWY_Y);
            }
        }

        /// <summary>
        /// 手动位移调试Z轴点动时间（ms）（发送时除以100以直接用于定时器）
        /// </summary>
        private int _ddsj_SDWY_Z = 500;
        /// <summary>
        /// 手动位移调试Z轴点动时间（ms）（发送时除以100以直接用于定时器）
        /// </summary>
        public int DDSJ_SDWY_Z
        {
            get { return _ddsj_SDWY_Z; }
            set
            {
                _ddsj_SDWY_Z = value;
                RaisePropertyChanged(() => DDSJ_SDWY_Z);
            }
        }

        /// <summary>
        /// X轴移动距离（mm）
        /// </summary>
        private int _ydjl_X = 0;
        /// <summary>
        /// X轴移动距离（mm）
        /// </summary>
        public int YDJL_X
        {
            get { return _ydjl_X; }
            set
            {
                _ydjl_X = value;
                RaisePropertyChanged(() => YDJL_X);
            }
        }

        /// <summary>
        /// Y轴移动距离（mm）
        /// </summary>
        private int _ydjl_Y = 0;
        /// <summary>
        /// Y轴移动距离（mm）
        /// </summary>
        public int YDJL_Y
        {
            get { return _ydjl_Y; }
            set
            {
                _ydjl_Y = value;
                RaisePropertyChanged(() => YDJL_Y);
            }
        }

        /// <summary>
        /// Z轴移动距离（mm）
        /// </summary>
        private int _ydjl_Z = 0;
        /// <summary>
        /// Z轴移动距离（mm）
        /// </summary>
        public int YDJL_Z
        {
            get { return _ydjl_Z; }
            set
            {
                _ydjl_Z = value;
                RaisePropertyChanged(() => YDJL_Z);
            }
        }

        #endregion


        #region 手动位移指令及回调

        /// <summary>
        /// 手动位移按钮指令
        /// </summary>
        private RelayCommand<String> _sdwyCommand;
        /// <summary>
        /// 手动位移按钮指令
        /// </summary>
        public RelayCommand<String> SDWYCommand
        {
            get
            {
                if (_sdwyCommand == null)
                    _sdwyCommand = new RelayCommand<String>((p) => ExecuteSDWYCommand(p));
                return _sdwyCommand;

            }
            set { _sdwyCommand = value; }
        }

        /// <summary>
        /// 手动位移回调
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecuteSDWYCommand(String num)
        {
            int i = Convert.ToInt16(num);
            //紧急停机
            if (i == 119)
            {
                Dev.DeviceRunMode = DevicRunModeType.JJTJ_Mode;
                SDWYOrderArray = Enumerable.Repeat((ushort)0, 24).ToArray();
                Messenger.Default.Send<string>("JJTJ", "JJTJMessage");
            }
            //位移调试数据曲线
            else if (i == 7122)
            {
                Messenger.Default.Send<string>(MQZH_WinName.SDWYPlotWinName, "OpenGivenNameWin");
            }
            //手动位移模式开
            else if (i == 5114)
            {
                Messenger.Default.Send<DevicRunModeType>(DevicRunModeType.SDWYDbg_Mode, "ModeChangeMessage");
            }
            //确认零位
            else if (i == 6520)
            {
                //自身变形位移调零
                Messenger.Default.Send<string>("X", "SetZeroMessage");
                Messenger.Default.Send<string>("SaveDevSenssorSettings", "DevDataRWMessage");
            }
            else if (i == 6521)
            {
                //自身变形位移调零
                Messenger.Default.Send<string>("Y", "SetZeroMessage");
                Messenger.Default.Send<string>("SaveDevSenssorSettings", "DevDataRWMessage");
            }
            else if (i == 6522)
            {
                //自身变形位移调零
                Messenger.Default.Send<string>("Z", "SetZeroMessage");
                Messenger.Default.Send<string>("SaveDevSenssorSettings", "DevDataRWMessage");
            }

            //X轴持续向右
            else if (i == 6507)
            {
                ushort tempY = SDWYOrderArray[11];
                ushort tempZ = SDWYOrderArray[12];
                SDWYOrderArray = Enumerable.Repeat((ushort)0, 24).ToArray();
                SDWYOrderArray[0] = 2;          //位移模式
                SDWYOrderArray[1] = 400;        //持续移动模式
                SDWYOrderArray[9] = 1;          //持续移动模式
                SDWYOrderArray[10] = 1;         //X轴持续运动方向，0停1右2左
                SDWYOrderArray[11] = tempY;     //Y轴持续运动方向，0停1前2后
                SDWYOrderArray[12] = tempZ;     //Z轴持续运动方向，0停1上2下
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }
            //X轴停止
            else if (i == 6508)
            {
                ushort tempY = SDWYOrderArray[11];
                ushort tempZ = SDWYOrderArray[12];
                SDWYOrderArray = Enumerable.Repeat((ushort)0, 24).ToArray();
                SDWYOrderArray[0] = 2;          //位移模式
                SDWYOrderArray[1] = 400;        //持续移动模式
                SDWYOrderArray[9] = 1;          //持续移动模式
                SDWYOrderArray[10] = 0;         //X轴持续运动方向，0停1右2左
                SDWYOrderArray[11] = tempY;     //Y轴持续运动方向，0停1前2后
                SDWYOrderArray[12] = tempZ;     //Z轴持续运动方向，0停1上2下
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }
            //X轴持续向左
            else if (i == 6509)
            {
                ushort tempY = SDWYOrderArray[11];
                ushort tempZ = SDWYOrderArray[12];
                SDWYOrderArray = Enumerable.Repeat((ushort)0, 24).ToArray();
                SDWYOrderArray[0] = 2;          //位移模式
                SDWYOrderArray[1] = 400;        //持续移动模式
                SDWYOrderArray[9] = 1;          //持续移动模式
                SDWYOrderArray[10] = 2;         //X轴持续运动方向，0停1右2左
                SDWYOrderArray[11] = tempY;     //Y轴持续运动方向，0停1前2后
                SDWYOrderArray[12] = tempZ;     //Z轴持续运动方向，0停1上2下
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }
            //Y轴持续向前
            else if (i == 6510)
            {
                ushort tempX = SDWYOrderArray[10];
                ushort tempZ = SDWYOrderArray[12];
                SDWYOrderArray = Enumerable.Repeat((ushort)0, 24).ToArray();
                SDWYOrderArray[0] = 2;          //位移模式
                SDWYOrderArray[1] = 400;        //持续移动模式
                SDWYOrderArray[9] = 1;          //持续移动模式
                SDWYOrderArray[10] = tempX;     //X轴持续运动方向，0停1左2右
                SDWYOrderArray[11] = 1;         //Y轴持续运动方向，0停1前2后
                SDWYOrderArray[12] = tempZ;     //Z轴持续运动方向，0停1上2下
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }
            //Y轴停止
            else if (i == 6511)
            {
                ushort tempX = SDWYOrderArray[10];
                ushort tempZ = SDWYOrderArray[12];
                SDWYOrderArray = Enumerable.Repeat((ushort)0, 24).ToArray();
                SDWYOrderArray[0] = 2;          //位移模式
                SDWYOrderArray[1] = 400;        //持续移动模式
                SDWYOrderArray[9] = 1;          //持续移动模式
                SDWYOrderArray[10] = tempX;     //X轴持续运动方向，0停1左2右
                SDWYOrderArray[11] = 0;         //Y轴持续运动方向，0停1前2后
                SDWYOrderArray[12] = tempZ;     //Z轴持续运动方向，0停1上2下
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }
            //Y轴持续向后
            else if (i == 6512)
            {
                ushort tempX = SDWYOrderArray[10];
                ushort tempZ = SDWYOrderArray[12];
                SDWYOrderArray = Enumerable.Repeat((ushort)0, 24).ToArray();
                SDWYOrderArray[0] = 2;          //位移模式
                SDWYOrderArray[1] = 400;        //持续移动模式
                SDWYOrderArray[9] = 1;          //持续移动模式
                SDWYOrderArray[10] = tempX;     //X轴持续运动方向，0停1左2右
                SDWYOrderArray[11] = 2;         //Y轴持续运动方向，0停1前2后
                SDWYOrderArray[12] = tempZ;     //Z轴持续运动方向，0停1上2下
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }
            //Z轴持续向上
            else if (i == 6513)
            {
                ushort tempX = SDWYOrderArray[10];
                ushort tempY = SDWYOrderArray[11];
                SDWYOrderArray = Enumerable.Repeat((ushort)0, 24).ToArray();
                SDWYOrderArray[0] = 2;          //位移模式
                SDWYOrderArray[1] = 400;        //持续移动模式
                SDWYOrderArray[9] = 1;          //持续移动模式
                SDWYOrderArray[10] = tempX;     //X轴持续运动方向，0停1左2右
                SDWYOrderArray[11] = tempY;     //Y轴持续运动方向，0停1前2后
                SDWYOrderArray[12] = 1;         //Z轴持续运动方向，0停1上2下
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }
            //Z轴停止
            else if (i == 6514)
            {
                ushort tempX = SDWYOrderArray[10];
                ushort tempY = SDWYOrderArray[11];
                SDWYOrderArray = Enumerable.Repeat((ushort)0, 24).ToArray();
                SDWYOrderArray[0] = 2;          //位移模式
                SDWYOrderArray[1] = 400;        //持续移动模式
                SDWYOrderArray[9] = 1;          //持续移动模式
                SDWYOrderArray[10] = tempX;     //X轴持续运动方向，0停1左2右
                SDWYOrderArray[11] = tempY;     //Y轴持续运动方向，0停1前2后
                SDWYOrderArray[12] = 0;         //Z轴持续运动方向，0停1上2下
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }
            //Z轴持续向下
            else if (i == 6515)
            {
                ushort tempX = SDWYOrderArray[10];
                ushort tempY = SDWYOrderArray[11];
                SDWYOrderArray = Enumerable.Repeat((ushort)0, 24).ToArray();
                SDWYOrderArray[0] = 2;          //位移模式
                SDWYOrderArray[1] = 400;        //持续移动模式
                SDWYOrderArray[9] = 1;          //持续移动模式
                SDWYOrderArray[10] = tempX;     //X轴持续运动方向，0停1左2右
                SDWYOrderArray[11] = tempY;     //Y轴持续运动方向，0停1前2后
                SDWYOrderArray[12] = 2;         //Z轴持续运动方向，0停1上2下
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }
            //X轴点动向右
            else if (i == 6501)
            {
                SDWYOrderArray = Enumerable.Repeat((ushort)0, 24).ToArray();
                SDWYOrderArray[0] = 2;      //位移模式
                SDWYOrderArray[1] = 300;    //点动模式
                SDWYOrderArray[2] = 1;      //点动模式
                SDWYOrderArray[3] = 1;      //X点动，1右2左
                SDWYOrderArray[6] = Convert.ToUInt16(DDSJ_SDWY_X / 100);    //点动时长
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }
            //X轴点动向左
            else if (i == 6502)
            {
                SDWYOrderArray = Enumerable.Repeat((ushort)0, 24).ToArray();
                SDWYOrderArray[0] = 2;      //位移模式
                SDWYOrderArray[1] = 300;    //点动模式
                SDWYOrderArray[2] = 1;      //点动模式
                SDWYOrderArray[3] = 2;      //X点动，1右2左
                SDWYOrderArray[6] = Convert.ToUInt16(DDSJ_SDWY_X / 100);    //X点动时长
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }
            //Y轴点动向前
            else if (i == 6503)
            {
                SDWYOrderArray = Enumerable.Repeat((ushort)0, 24).ToArray();
                SDWYOrderArray[0] = 2;      //位移模式
                SDWYOrderArray[1] = 300;    //点动模式
                SDWYOrderArray[2] = 1;      //点动模式
                SDWYOrderArray[4] = 1;      //Y点动，1前2后
                SDWYOrderArray[7] = Convert.ToUInt16(DDSJ_SDWY_Y / 100);    //Y点动时长
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }
            //Y轴点动向后
            else if (i == 6504)
            {
                SDWYOrderArray = Enumerable.Repeat((ushort)0, 24).ToArray();
                SDWYOrderArray[0] = 2;      //位移模式
                SDWYOrderArray[1] = 300;    //点动模式
                SDWYOrderArray[2] = 1;      //点动模式
                SDWYOrderArray[4] = 2;      //Y点动，1前2后
                SDWYOrderArray[7] = Convert.ToUInt16(DDSJ_SDWY_Y / 100);    //Y点动时长
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }
            //Z轴点动向上
            else if (i == 6505)
            {
                SDWYOrderArray = Enumerable.Repeat((ushort)0, 24).ToArray();
                SDWYOrderArray[0] = 2;      //位移模式
                SDWYOrderArray[1] = 300;    //点动模式
                SDWYOrderArray[2] = 1;      //点动模式
                SDWYOrderArray[5] = 1;      //Z点动，1上2下
                SDWYOrderArray[8] = Convert.ToUInt16(DDSJ_SDWY_Z / 100);    //Z点动时长
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }
            //Z轴点动向下
            else if (i == 6506)
            {
                SDWYOrderArray = Enumerable.Repeat((ushort)0, 24).ToArray();
                SDWYOrderArray[0] = 2;      //位移模式
                SDWYOrderArray[1] = 300;    //点动模式
                SDWYOrderArray[2] = 1;      //点动模式
                SDWYOrderArray[5] = 2;      //Z点动，1上2下
                SDWYOrderArray[8] = Convert.ToUInt16(DDSJ_SDWY_Z / 100);    //Z点动时长
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }

            //X轴移动到目标位置
            else if (i == 6516)
            {
                double distance = 0;
                if (Math.Abs(YDJL_X) <= Math.Abs(Dev.PermitErrX))
                    return;
                if (YDJL_X == 0)
                    return;
                //将相对位置换算为目标点位采集值
                if (YDJL_X > 0)
                    distance = YDJL_X + Dev.CorrXRight;
                if (YDJL_X < 0)
                    distance = YDJL_X - Dev.CorrXLeft;
                double aimPositionX = Dev.WYValueX + distance;        //目标位置
                double dataTrans = Dev.AIList[Dev.WYNOList[0] - 1].GetTransDataFromValueFinal_AI(-aimPositionX);       //X轴位移尺对应传输值

                if (dataTrans > 4000)
                    dataTrans = 4000;
                if (dataTrans < 0)
                    dataTrans = 0;
                //根据相对位移符号判定运动方向
                UInt16 direction = 0;
                if (YDJL_X > 0)
                    direction = 1;
                else if (YDJL_X < 0)
                    direction = 2;

                //生成指令
                SDWYOrderArray = Enumerable.Repeat((ushort)0, 24).ToArray();
                SDWYOrderArray[0] = 2;      //位移模式
                SDWYOrderArray[1] = 500;    //定点模式
                SDWYOrderArray[13] = direction;                                      //运动方向，1X右，2X左，3Y前，4Y后，5Z上，6Z下
                SDWYOrderArray[14] = Convert.ToUInt16(dataTrans);                   //X轴定位位置
                SDWYOrderArray[17] = Convert.ToUInt16(Dev.WYNOList[0] - 1);      //X位移尺编号
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }
            //Y轴移动到目标位置
            else if (i == 6517)
            {
                double distance = 0;
                if (Math.Abs(YDJL_Y) <= Math.Abs(Dev.PermitErrY))
                    return;
                if (YDJL_Y == 0)
                    return;
                //将相对位置换算为目标点位采集值
                if (YDJL_Y > 0)
                    distance = YDJL_Y + Dev.CorrYFront;
                if (YDJL_Y < 0)
                    distance = YDJL_Y - Dev.CorrYBack;
                double aimPositionY = Dev.WYValueY[3] + distance;        //目标位置
                double dataTrans1 = Dev.AIList[Dev.WYNOList[1] - 1].GetTransDataFromValueFinal_AI(-aimPositionY);       //左点对应传输值
                double dataTrans2 = Dev.AIList[Dev.WYNOList[2] - 1].GetTransDataFromValueFinal_AI(-aimPositionY);       //中间点对应传输值
                double dataTrans3 = Dev.AIList[Dev.WYNOList[3] - 1].GetTransDataFromValueFinal_AI(-aimPositionY);       //右点对应传输值
                double dataTransAverage = (dataTrans1 + dataTrans2 + dataTrans3) / 3;
                if (dataTransAverage > 4000)
                    dataTransAverage = 4000;
                if (dataTransAverage < 0)
                    dataTransAverage = 0;

                //根据相对位移符号判定运动方向
                UInt16 direction = 0;
                if (YDJL_Y > 0)
                    direction = 3;
                else if (YDJL_Y < 0)
                    direction = 4;
                //生成指令
                SDWYOrderArray = Enumerable.Repeat((ushort)0, 24).ToArray();
                SDWYOrderArray[0] = 2;      //位移模式
                SDWYOrderArray[1] = 500;    //定点模式
                SDWYOrderArray[13] = direction;                                     //运动方向，1X右，2X左，3Y前，4Y后，5Z上，6Z下
                SDWYOrderArray[15] = Convert.ToUInt16(dataTransAverage);            //Y轴定位位置
                SDWYOrderArray[18] = Convert.ToUInt16(Dev.WYNOList[1] - 1);     //Y左位移尺编号
                SDWYOrderArray[19] = Convert.ToUInt16(Dev.WYNOList[2] - 1);     //Y中位移尺编号
                SDWYOrderArray[20] = Convert.ToUInt16(Dev.WYNOList[3] - 1);     //Y右位移尺编号
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }
            //Z轴移动到目标位置
            else if (i == 6518)
            {
                double distance = 0;
                if (Math.Abs(YDJL_Z) <= Math.Abs(Dev.PermitErrZ))
                    return;
                if (YDJL_Z == 0)
                    return;
                //将相对位置换算为目标点位采集值
                if (YDJL_Z > 0)
                    distance = YDJL_Z + Dev.CorrZUp;
                if (YDJL_Z < 0)
                    distance = YDJL_Z - Dev.CorrZDown;
                double aimPositionZ = Dev.WYValueZ[3] + distance;        //目标位置
                double dataTrans1 = Dev.AIList[Dev.WYNOList[4] - 1].GetTransDataFromValueFinal_AI(aimPositionZ);       //左点对应传输值
                double dataTrans2 = Dev.AIList[Dev.WYNOList[5] - 1].GetTransDataFromValueFinal_AI(aimPositionZ);       //中间点对应传输值
                double dataTrans3 = Dev.AIList[Dev.WYNOList[6] - 1].GetTransDataFromValueFinal_AI(aimPositionZ);       //右点对应传输值
                double dataTransAverage = (dataTrans1 + dataTrans2 + dataTrans3) / 3;
                if (dataTransAverage > 4000)
                    dataTransAverage = 4000;
                if (dataTransAverage < 0)
                    dataTransAverage = 0;

                //根据相对位移符号判定运动方向
                UInt16 direction = 0;
                if (YDJL_Z > 0)
                    direction = 5;
                else if (YDJL_Z < 0)
                    direction = 6;
                //生成指令
                SDWYOrderArray = Enumerable.Repeat((ushort)0, 24).ToArray();
                SDWYOrderArray[0] = 2;      //位移模式
                SDWYOrderArray[1] = 500;    //定点模式
                SDWYOrderArray[13] = direction;                                     //运动方向，1X右，2X左，3Y前，4Y后，5Z上，6Z下
                SDWYOrderArray[16] = Convert.ToUInt16(dataTransAverage);            //Z轴定位位置
                SDWYOrderArray[21] = Convert.ToUInt16(Dev.WYNOList[4] - 1);     //Z左位移尺编号
                SDWYOrderArray[22] = Convert.ToUInt16(Dev.WYNOList[5] - 1);     //Z中位移尺编号
                SDWYOrderArray[23] = Convert.ToUInt16(Dev.WYNOList[6] - 1);     //Z右位移尺编号
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }
            //停止移动
            else if (i == 6519)
            {
                SDWYOrderArray = Enumerable.Repeat((ushort)0, 24).ToArray();
                SDWYOrderArray[0] = 2;      //位移模式
                SDWYOrderArray[1] = 900;    //停止运动模式
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }
        }

        #endregion


        #region 气密检测菜单、按钮操作指令

        /// <summary>
        /// 传递指令
        /// </summary>
        private RelayCommand<String> _qmCommand;
        /// <summary>
        /// 传递指令
        /// </summary>
        public RelayCommand<String> QMCommand
        {
            get
            {
                if (_qmCommand == null)
                    _qmCommand = new RelayCommand<String>((p) => ExecuteQMCMD(p));
                return _qmCommand;

            }
            set { _qmCommand = value; }
        }

        /// <summary>
        /// 根据按钮编号，切换按钮指令、发送通讯指令
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecuteQMCMD(String num)
        {
            int i = Convert.ToInt16(num);


            //紧急停机
            if (i == 119)
            {
                Messenger.Default.Send<string>("JJTJ", "JJTJMessage");
            }

            #region 实验启停

            //试验开始
            else if ((i == 1101) || (i == 1111))
            {
                //如果当前试验是默认试验，选择状态复位。
                if (ExpDQ.ExpSettingParam.ExpNO == "DefaultExp")
                {
                    MessageBox.Show("默认试验无法进行试验动作，请新建或载入其他试验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    //气密试验已选阶段数组清零
                    ExpDQ.Exp_QM.CanBeCheckInit();
                    return;
                }
                //正在试验中，不能重新加入试验
                if (Dev.IsDeviceBusy)
                {
                    MessageBox.Show("设备占用中，请等待或先终止其他实验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    return;
                }

                Messenger.Default.Send<int>(i, "QMCtrlMessage");
            }

            //停机指令
            else if ((i == 1201) || (i == 1211))
            {
                Messenger.Default.Send<int>(i, "StopExpMessage");
            }
            #endregion


            #region 窗口操作

            //实验数据窗口
            else if (i == 7104)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CurrentDataWinName, "OpenGivenNameWin");
            }
            //调零标定窗口
            else if (i == 7128)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CorrWinName, "OpenGivenNameWin");
            }
            //关闭气密窗口
            else if (i == 7205)
            {
                Messenger.Default.Send<int>(i, "QMCtrlMessage");
            }
            #endregion
        }

        #endregion


        #region 水密菜单、按钮操作指令

        /// <summary>
        /// 传递指令
        /// </summary>
        private RelayCommand<String> _smCommand;
        /// <summary>
        /// 传递指令
        /// </summary>
        public RelayCommand<String> SMCommand
        {
            get
            {
                if (_smCommand == null)
                    _smCommand = new RelayCommand<String>((p) => ExecuteSMCMD(p));
                return _smCommand;

            }
            set { _smCommand = value; }
        }

        /// <summary>
        /// 根据按钮编号，切换按钮指令、发送通讯指令
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecuteSMCMD(String num)
        {
            int i = Convert.ToInt16(num);

            //紧急停机
            if (i == 119)
            {
                Messenger.Default.Send<string>("JJTJ", "JJTJMessage");
            }

            #region 开始、停止实验

            //检测开始
            else if ((i == 1102) || (i == 1112))
            {
                //如果当前试验是默认试验，选择状态复位。
                if (ExpDQ.ExpSettingParam.ExpNO == "DefaultExp")
                {
                    MessageBox.Show("默认试验无法进行试验动作，请新建或载入其他试验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    //水密试验已选阶段数组清零
                    ExpDQ.Exp_SM.CanBeCheckInit();
                    return;
                }
                //正在试验中，不能重新加入试验
                if (Dev.IsDeviceBusy)
                {
                    MessageBox.Show("设备占用中，请等待或先终止其他实验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    return;
                }

                Messenger.Default.Send<int>(i, "SMCtrlMessage");
            }
            //停机指令
            else if ((i == 1202) || (i == 1212))
            {
                Messenger.Default.Send<int>(i, "StopExpMessage");
            }

            //渗漏结束
            else if ((i == 1302) || (i == 1312))
            {
                MessageBoxResult tempRet = MessageBox.Show("当可开启部分和固定部分均出现严重泄漏时，才需要提前停机。\r\n是否提前停机？", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                if (tempRet == MessageBoxResult.Yes)
                {
                    Messenger.Default.Send<int>(i, "SMCtrlMessage");
                }
            }

            #endregion

            #region 窗口操作

            //定级检测损坏情况窗口
            else if (i == 7112)
            {
                ExpDQ.ExpData_SM.SLStatusCopy();
                Messenger.Default.Send<string>(MQZH_WinName.SMDJDamageWinName, "OpenGivenNameWin");
            }
            //工程检测损坏情况窗口
            else if (i == 7113)
            {
                ExpDQ.ExpData_SM.SLStatusCopy();
                Messenger.Default.Send<string>(MQZH_WinName.SMGCDamageWinName, "OpenGivenNameWin");
            }
            //数据报告窗口
            else if (i == 7104)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CurrentDataWinName, "OpenGivenNameWin");
            }

            //调零标定窗口
            else if (i == 7128)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CorrWinName, "OpenGivenNameWin");
            }
            //关闭水密窗口
            else if (i == 7207)
            {
                Messenger.Default.Send<int>(i, "SMCtrlMessage");
            }
            #endregion

            #region 水密水泵启停

            //启停水泵
            else if (i == 1722)
            {
                if (Dev.DeviceRunMode == DevicRunModeType.SM_Mode)
                    Dev.DOList[6].IsOn = !Dev.DOList[6].IsOn;
            }

            #endregion

            #region 数据保存、取消修改

            //损坏数据保存
            else if ((i == 2104) || (i == 2204))
            {
                ExpDQ.ExpData_SM.SLStatusCopyBack();
                Messenger.Default.Send<string>("SaveSMStatusAndData", "SaveExpMessage");
            }

            #endregion
        }

        #endregion


        #region 抗风压菜单、按钮操作指令

        /// <summary>
        /// 传递指令
        /// </summary>
        private RelayCommand<String> _kfyCommand;
        /// <summary>
        /// 传递指令
        /// </summary>
        public RelayCommand<String> KFYCommand
        {
            get
            {
                if (_kfyCommand == null)
                    _kfyCommand = new RelayCommand<String>((p) => ExecuteKFYCMD(p));
                return _kfyCommand;

            }
            set { _kfyCommand = value; }
        }

        /// <summary>
        /// 根据按钮编号，切换按钮指令、发送通讯指令
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecuteKFYCMD(String num)
        {
            int i = Convert.ToInt16(num);

            #region 共用

            //紧急停机
            if (i == 119)
            {
                Messenger.Default.Send<string>("JJTJ", "JJTJMessage");
            }

            #endregion

            #region 开始、停止实验

            //p1检测开始
            else if ((i == 1103) || (i == 1113))
            {
                //如果当前试验是默认试验，选择状态复位。
                if (ExpDQ.ExpSettingParam.ExpNO == "DefaultExp")
                {
                    MessageBox.Show("默认试验无法进行试验动作，请新建或载入其他试验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    //抗风压已选阶段数组清零
                    ExpDQ.Exp_KFY.CanBeCheckInit();
                    return;
                }
                //正在试验中，不能重新加入试验
                if (Dev.IsDeviceBusy)
                {
                    MessageBox.Show("设备占用中，请等待或先终止其他实验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    return;
                }
                Messenger.Default.Send<int>(i, "KFYp1CtrlMessage");
            }
            //p2检测开始
            else if ((i == 1104) || (i == 1114))
            {
                //如果当前试验是默认试验，选择状态复位。
                if (ExpDQ.ExpSettingParam.ExpNO == "DefaultExp")
                {
                    MessageBox.Show("默认试验无法进行试验动作，请新建或载入其他试验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    //抗风压已选阶段数组清零
                    ExpDQ.Exp_KFY.CanBeCheckInit();
                    return;
                }
                //正在试验中，不能重新加入试验
                if (Dev.IsDeviceBusy)
                {
                    MessageBox.Show("设备占用中，请等待或先终止其他实验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    return;
                }
                Messenger.Default.Send<int>(i, "KFYp2CtrlMessage");
            }
            //p3检测开始
            else if ((i == 1105) || (i == 1115))
            {
                //如果当前试验是默认试验，选择状态复位。
                if (ExpDQ.ExpSettingParam.ExpNO == "DefaultExp")
                {
                    MessageBox.Show("默认试验无法进行试验动作，请新建或载入其他试验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    //抗风压已选阶段数组清零
                    ExpDQ.Exp_KFY.CanBeCheckInit();
                    return;
                }
                //正在试验中，不能重新加入试验
                if (Dev.IsDeviceBusy)
                {
                    MessageBox.Show("设备占用中，请等待或先终止其他实验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    return;
                }
                Messenger.Default.Send<int>(i, "KFYp3CtrlMessage");
            }
            //pmax检测开始
            else if ((i == 1106) || (i == 1116))
            {
                //如果当前试验是默认试验，选择状态复位。
                if (ExpDQ.ExpSettingParam.ExpNO == "DefaultExp")
                {
                    MessageBox.Show("默认试验无法进行试验动作，请新建或载入其他试验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    //抗风压已选阶段数组清零
                    ExpDQ.Exp_KFY.CanBeCheckInit();
                    return;
                }
                //正在试验中，不能重新加入试验
                if (Dev.IsDeviceBusy)
                {
                    MessageBox.Show("设备占用中，请等待或先终止其他实验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    return;
                }
                Messenger.Default.Send<int>(i, "KFYpmaxCtrlMessage");
            }
            //停机指令
            else if ((i == 1203) || (i == 1213) || (i == 1204) || (i == 1214) || (i == 1205) || (i == 1215) || (i == 1206) || (i == 1216))
            {
                Messenger.Default.Send<int>(i, "StopExpMessage");
            }
            //p1损坏结束
            else if ((i == 1403) || (i == 1413))
            {
                Messenger.Default.Send<int>(i, "KFYp1CtrlMessage");
            }
            //p2损坏结束
            else if ((i == 1404) || (i == 1414))
            {
                Messenger.Default.Send<int>(i, "KFYp2CtrlMessage");
            }
            //p3损坏结束
            else if ((i == 1405) || (i == 1415))
            {
                Messenger.Default.Send<int>(i, "KFYp3CtrlMessage");
            }
            //pmax损坏结束
            else if ((i == 1406) || (i == 1416))
            {
                Messenger.Default.Send<int>(i, "KFYpmaxCtrlMessage");
            }

            #endregion


            #region 窗口操作

            //定级检测损坏情况窗口
            else if (i == 7114)
            {
                Messenger.Default.Send<string>(MQZH_WinName.KFYDJDamageWinName, "OpenGivenNameWin");
            }
            //工程检测损坏情况窗口
            else if (i == 7115)
            {
                Messenger.Default.Send<string>(MQZH_WinName.KFYGCDamageWinName, "OpenGivenNameWin");
            }

            //数据报告窗口
            else if (i == 7104)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CurrentDataWinName, "OpenGivenNameWin");
            }

            //调零标定窗口
            else if (i == 7128)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CorrWinName, "OpenGivenNameWin");
            }


            //退出P1检测窗口
            else if (i == 7206)
            {
                Messenger.Default.Send<int>(i, "KFYp1CtrlMessage");
            }


            //退出P2检测窗口
            else if (i == 7208)
            {
                Messenger.Default.Send<int>(i, "KFYp2CtrlMessage");
            }

            //退出P3检测窗口
            else if (i == 7209)
            {
                Messenger.Default.Send<int>(i, "KFYp3CtrlMessage");
            }

            //退出Pmax检测窗口
            else if (i == 7210)
            {
                Messenger.Default.Send<int>(i, "KFYpmaxCtrlMessage");
            }
            #endregion


            #region 保存、取消

            //损坏数据保存
            else if ((i == 3103) || (i == 3104))
            {
                Messenger.Default.Send<string>("SaveKFYStatusAndData", "SaveExpMessage");
            }

            #endregion

        }

        #endregion


        #region 层间变形菜单、按钮操作指令

        /// <summary>
        /// 层间变形单次发送指令数组
        /// </summary>
        private ushort[] _cjbxOrderArray = Enumerable.Repeat((ushort)0, 20).ToArray();
        /// <summary>
        /// 层间变形单次发送指令数组
        /// </summary>
        public ushort[] CJBXOrderArray
        {
            get { return _cjbxOrderArray; }
            set
            {
                _cjbxOrderArray = value;
                RaisePropertyChanged(() => CJBXOrderArray);
            }
        }

        /// <summary>
        /// 窗口指令
        /// </summary>
        private RelayCommand<String> _cjbxCommand;
        /// <summary>
        /// 窗口指令
        /// </summary>
        public RelayCommand<String> CJBXCommand
        {
            get
            {
                if (_cjbxCommand == null)
                    _cjbxCommand = new RelayCommand<String>((p) => ExecuteCJBXCMD(p));
                return _cjbxCommand;

            }
            set { _cjbxCommand = value; }
        }

        /// <summary>
        /// 窗口指令回调
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecuteCJBXCMD(String num)
        {
            int i = Convert.ToInt16(num);

            //紧急停机
            if (i == 119)
            {
                Messenger.Default.Send<string>("JJTJ", "JJTJMessage");
            }
            //
            #region 开始、结束实验
            //停机指令
            else if ((i == 1207) || (i == 1217) || (i == 1208) || (i == 1218) || (i == 1209) || (i == 1219))
            {
                Messenger.Default.Send<int>(i, "StopExpMessage");
            }
            //X轴开始
            else if ((i == 1107) || (i == 1117))
            {
                //如果当前试验是默认试验，选择状态复位。
                if (ExpDQ.ExpSettingParam.ExpNO == "DefaultExp")
                {
                    MessageBox.Show("默认试验无法进行试验动作，请新建或载入其他试验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    //已选阶段数组清零
                    ExpDQ.Exp_CJBX.CanBeCheckInit();
                    return;
                }
                //正在试验中，不能重新加入试验
                if (Dev.IsDeviceBusy)
                {
                    MessageBox.Show("设备占用中，请等待或先终止其他实验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    return;
                }

                if (Math.Abs(Dev.WYValueX) > 10)
                {
                    MessageBoxResult msgBoxResult = MessageBox.Show("X轴位移尺距离零点较远（10mm以上）建议先仔细检查或手动调零。如需继续测试请按“是”，如需返回请按“否”", "调零提示", MessageBoxButton.YesNo);
                    if (msgBoxResult == MessageBoxResult.No)
                        return;
                }
                Messenger.Default.Send<int>(i, "CJBXCtrlMessage");
            }
            //Y轴开始
            else if ((i == 1108) || (i == 1118))
            {
                //如果当前试验是默认试验，选择状态复位。
                if (ExpDQ.ExpSettingParam.ExpNO == "DefaultExp")
                {
                    MessageBox.Show("默认试验无法进行试验动作，请新建或载入其他试验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    //已选阶段数组清零
                    ExpDQ.Exp_CJBX.CanBeCheckInit();
                    return;
                }
                //正在试验中，不能重新加入试验
                if (Dev.IsDeviceBusy)
                {
                    MessageBox.Show("设备占用中，请等待或先终止其他实验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    return;
                }
                if (Math.Abs(Dev.WYValueY[3]) > 10)
                {
                    MessageBoxResult msgBoxResult = MessageBox.Show("Y轴位移尺平均值距离零点较远（10mm以上）建议先仔细检查或手动调零。如需继续测试请按“是”，如需返回请按“否”", "调零提示", MessageBoxButton.YesNo);
                    if (msgBoxResult == MessageBoxResult.No)
                        return;
                }
                Messenger.Default.Send<int>(i, "CJBXCtrlMessage");
            }
            //Z轴开始
            else if ((i == 11079) || (i == 1119))
            {
                //如果当前试验是默认试验，选择状态复位。
                if (ExpDQ.ExpSettingParam.ExpNO == "DefaultExp")
                {
                    MessageBox.Show("默认试验无法进行试验动作，请新建或载入其他试验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    //已选阶段数组清零
                    ExpDQ.Exp_CJBX.CanBeCheckInit();
                    return;
                }
                //正在试验中，不能重新加入试验
                if (Dev.IsDeviceBusy)
                {
                    MessageBox.Show("设备占用中，请等待或先终止其他实验！", "提示", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                    return;
                }
                if (Math.Abs(Dev.WYValueZ[3]) > 10)
                {
                    MessageBoxResult msgBoxResult = MessageBox.Show("Z轴位移尺平均值距离零点较远（10mm以上）建议先仔细检查或手动调零。如需继续测试请按“是”，如需返回请按“否”", "调零提示", MessageBoxButton.YesNo);
                    if (msgBoxResult == MessageBoxResult.No)
                        return;
                }
                Messenger.Default.Send<int>(i, "CJBXCtrlMessage");
            }

            #endregion

            #region 窗口操作

            //打开定级检测损坏情况确认窗口
            else if (i == 7116)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CJBXDJDamageWinName, "OpenGivenNameWin");
            }
            //打开工程检测损坏情况确认窗口
            else if (i == 7117)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CJBXGCDamageWinName, "OpenGivenNameWin");
            }

            //打开数据窗口
            else if (i == 7104)
            {
                Messenger.Default.Send<string>(MQZH_WinName.CurrentDataWinName, "OpenGivenNameWin");
            }

            //退出层间窗口
            else if (i == 7211)
            {
                Messenger.Default.Send<int>(i, "CJBXCtrlMessage");
            }
            #endregion


            #region 设置零点

            //设置X轴位移零点
            else if (i == 1630)
            {
                //自身变形位移调零
                Messenger.Default.Send<string>("X", "SetZeroMessage");
                Messenger.Default.Send<string>("SaveDevSenssorSettings", "DevDataRWMessage");
            }
            //设置Y轴位移零点
            else if (i == 1631)
            {
                //自身变形位移调零
                Messenger.Default.Send<string>("Y", "SetZeroMessage");
                Messenger.Default.Send<string>("SaveDevSenssorSettings", "DevDataRWMessage");
            }
            //设置Z轴位移零点
            else if (i == 1632)
            {
                //自身变形位移调零
                Messenger.Default.Send<string>("Z", "SetZeroMessage");
                Messenger.Default.Send<string>("SaveDevSenssorSettings", "DevDataRWMessage");
            }
            //设置XYZ三轴位移零点
            else if (i == 1633)
            {
                //自身变形位移调零
                Messenger.Default.Send<string>("XYZ", "SetZeroMessage");
                Messenger.Default.Send<string>("SaveDevSenssorSettings", "DevDataRWMessage");
            }

            #endregion


            #region 数据保存、取消修改

            //检测损坏情况回存
            else if ((i == 4104) || (i == 4204))
            {
                Messenger.Default.Send<string>("SaveCJBXStatusAndData", "SaveExpMessage");
            }

            #endregion
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


        #region 绘图相关

        #region 报告挠度曲线绘图用

        /// <summary>
        /// 抗风压挠度绘图曲线组-定级正
        /// </summary>
        private List<LineModel> _plotLines_ND_DJZ;
        /// <summary>
        /// 抗风压挠度绘图曲线组-定级正
        /// </summary>
        public List<LineModel> PlotLines_ND_DJZ
        {
            get { return _plotLines_ND_DJZ; }
            set
            {
                _plotLines_ND_DJZ = value;
                RaisePropertyChanged(() => PlotLines_ND_DJZ);
            }
        }


        /// <summary>
        /// 抗风压挠度绘图曲线组-定级负
        /// </summary>
        private List<LineModel> _plotLines_ND_DJF;
        /// <summary>
        /// 抗风压挠度绘图曲线组-定级负
        /// </summary>
        public List<LineModel> PlotLines_ND_DJF
        {
            get { return _plotLines_ND_DJF; }
            set
            {
                _plotLines_ND_DJF = value;
                RaisePropertyChanged(() => PlotLines_ND_DJF);
            }
        }


        /// <summary>
        /// 抗风压挠度绘图曲线组-工程正
        /// </summary>
        private List<LineModel> _plotLines_ND_GCZ;
        /// <summary>
        /// 抗风压挠度绘图曲线组-工程正
        /// </summary>
        public List<LineModel> PlotLines_ND_GCZ
        {
            get { return _plotLines_ND_GCZ; }
            set
            {
                _plotLines_ND_GCZ = value;
                RaisePropertyChanged(() => PlotLines_ND_GCZ);
            }
        }


        /// <summary>
        /// 抗风压挠度绘图曲线组-工程负
        /// </summary>
        private List<LineModel> _plotLines_ND_GCF;
        /// <summary>
        /// 抗风压挠度绘图曲线组-工程负
        /// </summary>
        public List<LineModel> PlotLines_ND_GCF
        {
            get { return _plotLines_ND_GCF; }
            set
            {
                _plotLines_ND_GCF = value;
                RaisePropertyChanged(() => PlotLines_ND_GCF);
            }
        }


        /// <summary>
        /// 绘图数据、测点组数据更新
        /// </summary>
        public void NDPlotUpdate()
        {
            for (int i = 0; i < 3; i++)
            {
                PlotLines_ND_DJZ[i].LineDataSource.Collection.Clear();
                PlotLines_ND_DJF[i].LineDataSource.Collection.Clear();
                PlotLines_ND_GCZ[i].LineDataSource.Collection.Clear();
                PlotLines_ND_GCF[i].LineDataSource.Collection.Clear();
            }
            //测点组数据更新
            try
            {
                double x = 0;
                double y = 0;
                System.Windows.Point newPoint;

                for (int i = 0; i < ExpDQ.Exp_KFY.DisplaceGroups.Count; i++)
                {
                    if (ExpDQ.Exp_KFY.DisplaceGroups[i].Is_Use)
                    {
                        if (ExpDQ.Exp_KFY.IsGC)
                        {
                            //工程变形正压检测
                            //if (ExpDQ.Exp_KFY.StageList_KFYGC[1].CompleteStatus)
                            //{
                                for (int j = 0; j < 4; j++)
                                {
                                    newPoint = new System.Windows.Point
                                    {
                                        X = ExpDQ.ExpData_KFY.TestPress_GCBX_Z[j],
                                        Y = ExpDQ.ExpData_KFY.ND_GCBX_Z[j + 1][i]
                                    };
                                    PlotLines_ND_GCZ[i].AddPoint(newPoint);
                                }
                            //}
                            //工程变形负压检测
                            //if (ExpDQ.Exp_KFY.StageList_KFYGC[3].CompleteStatus)
                            //{
                                for (int j = 0; j < 4; j++)
                                {
                                    newPoint = new System.Windows.Point
                                    {
                                        X = ExpDQ.ExpData_KFY.TestPress_GCBX_F[j],
                                        Y = ExpDQ.ExpData_KFY.ND_GCBX_F[j + 1][i]
                                    };
                                    PlotLines_ND_GCF[i].AddPoint(newPoint);
                                }
                            //}
                            ////工程P3正压检测
                            //if (ExpDQ.Exp_KFY.StageList_KFYGC[6].CompleteStatus)
                            //{
                            //    newPoint = new System.Windows.Point();
                            //    newPoint.X = ExpDQ.ExpData_KFY.TestPress_GCP3_Z;
                            //    newPoint.Y = ExpDQ.ExpData_KFY.ND_GCP3_Z[1][i];
                            //    PlotLines_ND_GCZ[i].AddPoint(newPoint);
                            //}
                            ////工程P3负压检测
                            //if (ExpDQ.Exp_KFY.StageList_KFYGC[7].CompleteStatus)
                            //{
                            //    newPoint = new System.Windows.Point();
                            //    newPoint.X = ExpDQ.ExpData_KFY.TestPress_GCP3_F;
                            //    newPoint.Y = ExpDQ.ExpData_KFY.ND_GCP3_F[1][i];
                            //    PlotLines_ND_GCF[i].AddPoint(newPoint);
                            //}
                            ////工程Pmax正压检测
                            //if (ExpDQ.Exp_KFY.StageList_KFYGC[8].CompleteStatus)
                            //{
                            //    newPoint = new System.Windows.Point();
                            //    newPoint.X = ExpDQ.ExpData_KFY.TestPress_GCPmax_Z;
                            //    newPoint.Y = ExpDQ.ExpData_KFY.ND_GCPmax_Z[1][i];
                            //    PlotLines_ND_GCZ[i].AddPoint(newPoint);
                            //}
                            ////工程Pmax负压检测
                            //if (ExpDQ.Exp_KFY.StageList_KFYGC[9].CompleteStatus)
                            //{
                            //    newPoint = new System.Windows.Point();
                            //    newPoint.X = ExpDQ.ExpData_KFY.TestPress_GCPmax_F;
                            //    newPoint.Y = ExpDQ.ExpData_KFY.ND_GCPmax_F[1][i];
                            //    PlotLines_ND_GCF[i].AddPoint(newPoint);
                            //}
                        }

                        else
                        {
                            //定级变形正压检测
                            //if (ExpDQ.Exp_KFY.StageList_KFYDJ[1].CompleteStatus)
                            //{
                                for (int j = 0; j < 20; j++)
                                {
                                    newPoint = new System.Windows.Point
                                    {
                                        X = ExpDQ.ExpData_KFY.TestPress_DJBX_Z[j],
                                        Y = ExpDQ.ExpData_KFY.ND_DJBX_Z[j + 1][i]
                                    };
                                    PlotLines_ND_DJZ[i].AddPoint(newPoint);
                                }
                            //}
                            //定级变形负压检测
                            //if (ExpDQ.Exp_KFY.StageList_KFYDJ[3].CompleteStatus)
                            //{
                                for (int j = 0; j < 20; j++)
                                {
                                    newPoint = new System.Windows.Point
                                    {
                                        X = ExpDQ.ExpData_KFY.TestPress_DJBX_F[j],
                                        Y = ExpDQ.ExpData_KFY.ND_DJBX_F[j + 1][i]
                                    };
                                    PlotLines_ND_DJF[i].AddPoint(newPoint);
                                }
                            //}
                            ////定级P3正压检测
                            //if (ExpDQ.Exp_KFY.StageList_KFYDJ[6].CompleteStatus)
                            //{
                            //    newPoint = new System.Windows.Point();
                            //    newPoint.X = ExpDQ.ExpData_KFY.TestPress_DJP3_Z;
                            //    newPoint.Y = ExpDQ.ExpData_KFY.ND_DJP3_Z[1][i];
                            //    PlotLines_ND_DJZ[i].AddPoint(newPoint);
                            //}
                            ////定级P3负压检测
                            //if (ExpDQ.Exp_KFY.StageList_KFYDJ[7].CompleteStatus)
                            //{
                            //    newPoint = new System.Windows.Point();
                            //    newPoint.X = ExpDQ.ExpData_KFY.TestPress_DJP3_F;
                            //    newPoint.Y = ExpDQ.ExpData_KFY.ND_DJP3_F[1][i];
                            //    PlotLines_ND_DJF[i].AddPoint(newPoint);
                            //}
                            ////定级Pmax正压检测
                            //if (ExpDQ.Exp_KFY.StageList_KFYDJ[8].CompleteStatus)
                            //{
                            //    newPoint = new System.Windows.Point();
                            //    newPoint.X = ExpDQ.ExpData_KFY.TestPress_DJPmax_Z;
                            //    newPoint.Y = ExpDQ.ExpData_KFY.ND_DJPmax_Z[1][i];
                            //    PlotLines_ND_DJZ[i].AddPoint(newPoint);
                            //}
                            ////定级Pmax负压检测
                            //if (ExpDQ.Exp_KFY.StageList_KFYDJ[9].CompleteStatus)
                            //{
                            //    newPoint = new System.Windows.Point();
                            //    newPoint.X = ExpDQ.ExpData_KFY.TestPress_DJPmax_F;
                            //    newPoint.Y = ExpDQ.ExpData_KFY.ND_DJPmax_F[1][i];
                            //    PlotLines_ND_DJF[i].AddPoint(newPoint);
                            //}
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        /// <summary>
        /// 挠度曲线绘图初始化
        /// </summary>
        public void NDPlotInit()
        {
            PlotLines_ND_DJZ = new List<LineModel>();
            PlotLines_ND_DJF = new List<LineModel>();
            PlotLines_ND_GCZ = new List<LineModel>();
            PlotLines_ND_GCF = new List<LineModel>();
            try
            {
                //挠度11
                LineModel newLine11 = new LineModel();
                newLine11.LineNO = "ND_L11";
                newLine11.LineName = "挠度11";
                newLine11.VarUnit = "mm";
                newLine11.LineThickness = 0.6;
                newLine11.LineColor = Color.Red;
                newLine11.AxisYside = LineModel.AxisUse.Left;
                newLine11.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine11.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine11.MaxPointsQuantity = Dev.PointsPerLine;
                newLine11.MaxBSPointsQuantity = Dev.PointsPerLine;
                newLine11.IsBackStore = false;
                PlotLines_ND_DJZ.Add(newLine11);
                //挠度12
                LineModel newLine12 = new LineModel();
                newLine12.LineNO = "ND_L12";
                newLine12.LineName = "挠度12";
                newLine12.VarUnit = "mm";
                newLine12.LineThickness = 0.6;
                newLine12.LineColor = Color.Red;
                newLine12.AxisYside = LineModel.AxisUse.Left;
                newLine12.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine12.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine12.MaxPointsQuantity = Dev.PointsPerLine;
                newLine12.MaxBSPointsQuantity = Dev.PointsPerLine;
                newLine12.IsBackStore = false;
                PlotLines_ND_DJZ.Add(newLine12);
                //挠度13
                LineModel newLine13 = new LineModel();
                newLine13.LineNO = "ND_L13";
                newLine13.LineName = "挠度13";
                newLine13.VarUnit = "mm";
                newLine13.LineThickness = 0.6;
                newLine13.LineColor = Color.Red;
                newLine13.AxisYside = LineModel.AxisUse.Left;
                newLine13.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine13.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine13.MaxPointsQuantity = Dev.PointsPerLine;
                newLine13.MaxBSPointsQuantity = Dev.PointsPerLine;
                newLine13.IsBackStore = false;
                PlotLines_ND_DJZ.Add(newLine13);

                //挠度21
                LineModel newLine21 = new LineModel();
                newLine21.LineNO = "ND_L21";
                newLine21.LineName = "挠度21";
                newLine21.VarUnit = "mm";
                newLine21.LineThickness = 0.6;
                newLine21.LineColor = Color.Red;
                newLine21.AxisYside = LineModel.AxisUse.Left;
                newLine21.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine21.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine21.MaxPointsQuantity = Dev.PointsPerLine;
                newLine21.MaxBSPointsQuantity = Dev.PointsPerLine;
                newLine21.IsBackStore = false;
                PlotLines_ND_DJF.Add(newLine21);
                //挠度22
                LineModel newLine22 = new LineModel();
                newLine22.LineNO = "ND_L22";
                newLine22.LineName = "挠度22";
                newLine22.VarUnit = "mm";
                newLine22.LineThickness = 0.6;
                newLine22.LineColor = Color.Red;
                newLine22.AxisYside = LineModel.AxisUse.Left;
                newLine22.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine22.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine22.MaxPointsQuantity = Dev.PointsPerLine;
                newLine22.MaxBSPointsQuantity = Dev.PointsPerLine;
                newLine22.IsBackStore = false;
                PlotLines_ND_DJF.Add(newLine22);
                //挠度23
                LineModel newLine23 = new LineModel();
                newLine23.LineNO = "ND_L23";
                newLine23.LineName = "挠度23";
                newLine23.VarUnit = "mm";
                newLine23.LineThickness = 0.6;
                newLine23.LineColor = Color.Red;
                newLine23.AxisYside = LineModel.AxisUse.Left;
                newLine23.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine23.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine23.MaxPointsQuantity = Dev.PointsPerLine;
                newLine23.MaxBSPointsQuantity = Dev.PointsPerLine;
                newLine23.IsBackStore = false;
                PlotLines_ND_DJF.Add(newLine23);


                //挠度31
                LineModel newLine31 = new LineModel();
                newLine31.LineNO = "ND_L31";
                newLine31.LineName = "挠度31";
                newLine31.VarUnit = "mm";
                newLine31.LineThickness = 0.6;
                newLine31.LineColor = Color.Red;
                newLine31.AxisYside = LineModel.AxisUse.Left;
                newLine31.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine31.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine31.MaxPointsQuantity = Dev.PointsPerLine;
                newLine31.MaxBSPointsQuantity = Dev.PointsPerLine;
                newLine31.IsBackStore = false;
                PlotLines_ND_GCZ.Add(newLine31);
                //挠度32
                LineModel newLine32 = new LineModel();
                newLine32.LineNO = "ND_L32";
                newLine32.LineName = "挠度32";
                newLine32.VarUnit = "mm";
                newLine32.LineThickness = 0.6;
                newLine32.LineColor = Color.Red;
                newLine32.AxisYside = LineModel.AxisUse.Left;
                newLine32.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine32.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine32.MaxPointsQuantity = Dev.PointsPerLine;
                newLine32.MaxBSPointsQuantity = Dev.PointsPerLine;
                newLine32.IsBackStore = false;
                PlotLines_ND_GCZ.Add(newLine32);
                //挠度33
                LineModel newLine33 = new LineModel();
                newLine33.LineNO = "ND_L33";
                newLine33.LineName = "挠度33";
                newLine33.VarUnit = "mm";
                newLine33.LineThickness = 0.6;
                newLine33.LineColor = Color.Red;
                newLine33.AxisYside = LineModel.AxisUse.Left;
                newLine33.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine33.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine33.MaxPointsQuantity = Dev.PointsPerLine;
                newLine33.MaxBSPointsQuantity = Dev.PointsPerLine;
                newLine33.IsBackStore = false;
                PlotLines_ND_GCZ.Add(newLine33);


                //挠度41
                LineModel newLine41 = new LineModel();
                newLine41.LineNO = "ND_L41";
                newLine41.LineName = "挠度41";
                newLine41.VarUnit = "mm";
                newLine41.LineThickness = 0.6;
                newLine41.LineColor = Color.Red;
                newLine41.AxisYside = LineModel.AxisUse.Left;
                newLine41.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine41.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine41.MaxPointsQuantity = Dev.PointsPerLine;
                newLine41.MaxBSPointsQuantity = Dev.PointsPerLine;
                newLine41.IsBackStore = false;
                PlotLines_ND_GCF.Add(newLine41);
                //挠度42
                LineModel newLine42 = new LineModel();
                newLine42.LineNO = "ND_L42";
                newLine42.LineName = "挠度42";
                newLine42.VarUnit = "mm";
                newLine42.LineThickness = 0.6;
                newLine42.LineColor = Color.Red;
                newLine42.AxisYside = LineModel.AxisUse.Left;
                newLine42.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine42.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine42.MaxPointsQuantity = Dev.PointsPerLine;
                newLine42.MaxBSPointsQuantity = Dev.PointsPerLine;
                newLine42.IsBackStore = false;
                PlotLines_ND_GCF.Add(newLine42);
                //挠度43
                LineModel newLine43 = new LineModel();
                newLine43.LineNO = "ND_L43";
                newLine43.LineName = "挠度43";
                newLine43.VarUnit = "mm";
                newLine43.LineThickness = 0.6;
                newLine43.LineColor = Color.Red;
                newLine43.AxisYside = LineModel.AxisUse.Left;
                newLine43.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine43.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine43.MaxPointsQuantity = Dev.PointsPerLine;
                newLine43.MaxBSPointsQuantity = Dev.PointsPerLine;
                newLine43.IsBackStore = false;
                PlotLines_ND_GCF.Add(newLine43);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }




        #endregion


        #region 检测绘图用


        /// <summary>
        ///更新曲线需要更新消息回调
        /// </summary>
        /// <param name="msg"></param>
        private void UpdateChartMessage(string msg)
        {
            UpdatePlotLins();
        }


        /// <summary>
        /// 绘图用曲线组(0小压差、1中压差、2大压差、3风量1、4风量2、5风量3、6选用风量、7水流量、
        /// 8X轴位移、9Y位移左、10Y位移中、11Y位移右、12Y位移平均、 13Z位移左、14Z位移中、15Z位移右、16Z位移平均
        /// 17挠度1、18挠度2、19挠度3、20相对挠度1、21相对挠度2、22相对挠度3
        /// 23风机频率、24水泵频率
        /// </summary>
        private List<LineModel> _plotLines;
        /// <summary>
        /// 绘图用曲线组(0小压差、1中压差、2大压差、3风量1、4风量2、5风量3、6选用风量、7水流量、
        /// 8X轴位移、9Y位移左、10Y位移中、11Y位移右、12Y位移平均、 13Z位移左、14Z位移中、15Z位移右、16Z位移平均、
        /// 17挠度1、18挠度2、19挠度3、20相对挠度1、21相对挠度2、22相对挠度3、
        /// 23风机频率、24水泵频率/// </summary>
        public List<LineModel> PlotLines
        {
            get { return _plotLines; }
            set
            {
                _plotLines = value;
                RaisePropertyChanged(() => PlotLines);
            }
        }

        /// <summary>
        /// 曲线初始化
        /// </summary>
        public void PlotInit()
        {
            PlotLines = new List<LineModel>();
            //小气压
            LineModel newLine = new LineModel();
            newLine.LineNO = "Line0";
            newLine.LineName = "小气压";
            newLine.VarUnit = "Pa";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Red;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //中气压
            newLine = new LineModel();
            newLine.LineNO = "Line1";
            newLine.LineName = "中气压";
            newLine.VarUnit = "Pa";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.DarkBlue;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //大气压
            newLine = new LineModel();
            newLine.LineNO = "Line2";
            newLine.LineName = "大气压";
            newLine.VarUnit = "Pa";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.DarkBlue;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);

            //风量1
            newLine = new LineModel();
            newLine.LineNO = "Line3";
            newLine.LineName = "风量1";
            newLine.VarUnit = "m³/h";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.DarkGreen;
            newLine.AxisYside = LineModel.AxisUse.Right;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);

            //风量2
            newLine = new LineModel();
            newLine.LineNO = "Line4";
            newLine.LineName = "风量2";
            newLine.VarUnit = "m³/h";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.DarkGreen;
            newLine.AxisYside = LineModel.AxisUse.Right;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);

            //风量3
            newLine = new LineModel();
            newLine.LineNO = "Line5";
            newLine.LineName = "风量3";
            newLine.VarUnit = "m³/h";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.DarkGreen;
            newLine.AxisYside = LineModel.AxisUse.Right;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);

            //选用风量
            newLine = new LineModel();
            newLine.LineNO = "Line6";
            newLine.LineName = "选用风量";
            newLine.VarUnit = "m³/h";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.DarkGreen;
            newLine.AxisYside = LineModel.AxisUse.Right;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);

            //水流量
            newLine = new LineModel();
            newLine.LineNO = "Line7";
            newLine.LineName = "水流量";
            newLine.VarUnit = "m³/h";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);


            //X轴位移
            newLine = new LineModel();
            newLine.LineNO = "Line8";
            newLine.LineName = "X位移";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //Y位移左
            newLine = new LineModel();
            newLine.LineNO = "Line9";
            newLine.LineName = "Y位移左";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //Y位移中
            newLine = new LineModel();
            newLine.LineNO = "Line10";
            newLine.LineName = "Y位移中";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //Y轴位移右
            newLine = new LineModel();
            newLine.LineNO = "Line11";
            newLine.LineName = "Y轴位移右";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //Y轴位移平均
            newLine = new LineModel();
            newLine.LineNO = "Line12";
            newLine.LineName = "Y轴位移平均";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //Z位移左
            newLine = new LineModel();
            newLine.LineNO = "Line13";
            newLine.LineName = "Z位移左";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //Z位移中
            newLine = new LineModel();
            newLine.LineNO = "Line14";
            newLine.LineName = "Z位移中";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //Z轴位移右
            newLine = new LineModel();
            newLine.LineNO = "Line15";
            newLine.LineName = "Z轴位移右";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //Z轴位移平均
            newLine = new LineModel();
            newLine.LineNO = "Line16";
            newLine.LineName = "Z轴位移平均";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);

            //挠度1
            newLine = new LineModel();
            newLine.LineNO = "Line17";
            newLine.LineName = "挠度1";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.6;
            newLine.LineColor = Color.Red;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //挠度2
            newLine = new LineModel();
            newLine.LineNO = "Line18";
            newLine.LineName = "挠度2";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.6;
            newLine.LineColor = Color.DarkGreen;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //挠度3
            newLine = new LineModel();
            newLine.LineNO = "Line19";
            newLine.LineName = "挠度3";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.6;
            newLine.LineColor = Color.Blue;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);

            //相对挠度1
            newLine = new LineModel();
            newLine.LineNO = "Line20";
            newLine.LineName = "相对挠度1";
            newLine.VarUnit = "";
            newLine.LineThickness = 0.6;
            newLine.LineColor = Color.Red;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //相对挠度2
            newLine = new LineModel();
            newLine.LineNO = "Line21";
            newLine.LineName = "相对挠度2";
            newLine.VarUnit = "";
            newLine.LineThickness = 0.6;
            newLine.LineColor = Color.DarkGreen;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //相对挠度3
            newLine = new LineModel();
            newLine.LineNO = "Line22";
            newLine.LineName = "相对挠度3";
            newLine.VarUnit = "";
            newLine.LineThickness = 0.6;
            newLine.LineColor = Color.Blue;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);

            //风机频率输出
            newLine = new LineModel();
            newLine.LineNO = "Line23";
            newLine.LineName = "频率1";
            newLine.VarUnit = "HZ";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.DarkViolet;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //水泵频率输出
            newLine = new LineModel();
            newLine.LineNO = "Line24";
            newLine.LineName = "水泵频率";
            newLine.VarUnit = "HZ";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
        }


        /// <summary>
        ///绘图数据更新消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void UpdatePlotLins()
        {
            TimeSpan span = DateTime.Now - Dev.PowerOnTime;

            //小气压
            System.Windows.Point newPoint0 = new System.Windows.Point();
            newPoint0.X = span.TotalSeconds;
            newPoint0.Y = Dev.AIList[12].ValueFinal;
            PlotLines[0].AddPoint(newPoint0);

            //中气压
            if (Dev.IsWithCYM)
            {
                System.Windows.Point newPoint1 = new System.Windows.Point();
                newPoint1.X = span.TotalSeconds;
                newPoint1.Y = Dev.AIList[13].ValueFinal;
                PlotLines[1].AddPoint(newPoint1);
            }

            //大气压
            System.Windows.Point newPoint2 = new System.Windows.Point();
            newPoint2.X = span.TotalSeconds;
            newPoint2.Y = Dev.AIList[14].ValueFinal;
            PlotLines[2].AddPoint(newPoint2);

            //风量1
            System.Windows.Point newPoint3 = new System.Windows.Point();
            newPoint3.X = span.TotalSeconds;
            newPoint3.Y = Dev.FL1;
            PlotLines[3].AddPoint(newPoint3);

            //风量2
            System.Windows.Point newPoint4 = new System.Windows.Point();
            newPoint4.X = span.TotalSeconds;
            newPoint4.Y = Dev.FL2;
            PlotLines[4].AddPoint(newPoint4);

            //风量3
            System.Windows.Point newPoint5 = new System.Windows.Point();
            newPoint5.X = span.TotalSeconds;
            newPoint5.Y = Dev.FL3;
            PlotLines[5].AddPoint(newPoint5);

            //气密风量
            System.Windows.Point newPoint6 = new System.Windows.Point();
            newPoint6.X = span.TotalSeconds;
            switch (Dev.FSGUse)
            {
                case 1:
                    newPoint6.Y = Dev.FL1;
                    break;
                case 2:
                    newPoint6.Y = Dev.FL2;
                    break;
                case 3:
                    newPoint6.Y = Dev.FL3;
                    break;
                default:
                    newPoint6.Y = Dev.FL2;
                    break;
            }
            PlotLines[6].AddPoint(newPoint6);

            //水流量
            if (Dev.IsWithSLL)
            {
                System.Windows.Point newPoint7 = new System.Windows.Point();
                newPoint7.X = span.TotalSeconds;
                newPoint7.Y = Dev.SLL;
                PlotLines[7].AddPoint(newPoint7);
            }

            //X轴位移
            System.Windows.Point newPoint8 = new System.Windows.Point();
            newPoint8.X = span.TotalSeconds;
            newPoint8.Y = Dev.WYValueX;
            PlotLines[8].AddPoint(newPoint8);

            //Y轴位移左
            System.Windows.Point newPoint9 = new System.Windows.Point();
            newPoint9.X = span.TotalSeconds;
            newPoint9.Y = Dev.WYValueY[0];
            PlotLines[9].AddPoint(newPoint9);
            //Y轴位移中
            System.Windows.Point newPoint10 = new System.Windows.Point();
            newPoint10.X = span.TotalSeconds;
            newPoint10.Y = Dev.WYValueY[1];
            PlotLines[10].AddPoint(newPoint10);
            //Y轴位移右
            System.Windows.Point newPoint11 = new System.Windows.Point();
            newPoint11.X = span.TotalSeconds;
            newPoint11.Y = Dev.WYValueY[2];
            PlotLines[11].AddPoint(newPoint11);
            //Y轴位移平均
            System.Windows.Point newPoint12 = new System.Windows.Point();
            newPoint12.X = span.TotalSeconds;
            newPoint12.Y = Dev.WYValueY[3];
            PlotLines[12].AddPoint(newPoint12);

            //Z轴位移左
            System.Windows.Point newPoint13 = new System.Windows.Point();
            newPoint13.X = span.TotalSeconds;
            newPoint13.Y = Dev.WYValueZ[0];
            PlotLines[13].AddPoint(newPoint13);
            //Z轴位移中
            System.Windows.Point newPoint14 = new System.Windows.Point();
            newPoint14.X = span.TotalSeconds;
            newPoint14.Y = Dev.WYValueZ[1];
            PlotLines[14].AddPoint(newPoint14);
            //Z轴位移右
            System.Windows.Point newPoint15 = new System.Windows.Point();
            newPoint15.X = span.TotalSeconds;
            newPoint15.Y = Dev.WYValueZ[2];
            PlotLines[15].AddPoint(newPoint15);
            //Z轴位移平均
            System.Windows.Point newPoint16 = new System.Windows.Point();
            newPoint16.X = span.TotalSeconds;
            newPoint16.Y = Dev.WYValueZ[3];
            PlotLines[16].AddPoint(newPoint16);

            //风机频率
            System.Windows.Point newPoint23 = new System.Windows.Point();
            newPoint23.X = span.TotalSeconds;
            newPoint23.Y = Dev.AIList[22].ValueFinal;
            PlotLines[23].AddPoint(newPoint23);

            //水泵频率
            if (Dev.IsWithSLL)
            {
                System.Windows.Point newPoint24 = new System.Windows.Point();
                newPoint24.X = span.TotalSeconds;
                newPoint24.Y = Dev.AIList[23].ValueFinal;
                PlotLines[24].AddPoint(newPoint24);
            }

            if (IsKFYWinOpened)
            {
                //测点组数据更新
                try
                {
                    if (Dev.IsWYKFYF)
                    {
                        for (int i = 0; i < ExpDQ.Exp_KFY.DisplaceGroups.Count; i++)
                        {
                            if (ExpDQ.Exp_KFY.DisplaceGroups[i].Is_Use)
                            {
                                for (int j = 0; j < ExpDQ.Exp_KFY.DisplaceGroups[i].WY_DQ.Count; j++)
                                {
                                    int k = ExpDQ.Exp_KFY.DisplaceGroups[i].WYC_No[j];
                                    ExpDQ.Exp_KFY.DisplaceGroups[i].WY_DQ[j] = Dev.WyWPL[k - 1];
                                    if (j == 3)
                                    {
                                        if ((i == 0) && ExpDQ.Exp_KFY.DisplaceGroups[0].Is_Use && ExpDQ.Exp_KFY.DisplaceGroups[0].Is_TestMBBX && ExpDQ.Exp_KFY.DisplaceGroups[0].IsBZCSJMB)
                                            ExpDQ.Exp_KFY.DisplaceGroups[i].WY_DQ[j] = Dev.WyWPL[k - 1];
                                        else
                                            ExpDQ.Exp_KFY.DisplaceGroups[i].WY_DQ[j] = 0;
                                    }
                                }
                                ExpDQ.Exp_KFY.DisplaceGroups[i].NDJS();
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ExpDQ.Exp_KFY.DisplaceGroups.Count; i++)
                        {
                            if (ExpDQ.Exp_KFY.DisplaceGroups[i].Is_Use)
                            {
                                for (int j = 0; j < ExpDQ.Exp_KFY.DisplaceGroups[i].WY_DQ.Count; j++)
                                {
                                    int k = ExpDQ.Exp_KFY.DisplaceGroups[i].WYC_No[j];
                                    ExpDQ.Exp_KFY.DisplaceGroups[i].WY_DQ[j] = Dev.AIList[k - 1].ValueFinal;
                                    if (j == 3)
                                    {
                                        if ((i == 0) && ExpDQ.Exp_KFY.DisplaceGroups[0].Is_Use && ExpDQ.Exp_KFY.DisplaceGroups[0].Is_TestMBBX && ExpDQ.Exp_KFY.DisplaceGroups[0].IsBZCSJMB)
                                            ExpDQ.Exp_KFY.DisplaceGroups[i].WY_DQ[j] = Dev.AIList[k - 1].ValueFinal;
                                        else
                                            ExpDQ.Exp_KFY.DisplaceGroups[i].WY_DQ[j] = 0;
                                    }
                                }
                                ExpDQ.Exp_KFY.DisplaceGroups[i].NDJS();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }


                if (ExpDQ.Exp_KFY.DisplaceGroups[0].Is_Use)
                {
                    //挠度1
                    System.Windows.Point newPoint17 = new System.Windows.Point();
                    newPoint17.X = span.TotalSeconds;
                    newPoint17.Y = ExpDQ.Exp_KFY.DisplaceGroups[0].ND;
                    PlotLines[17].AddPoint(newPoint17);
                    //相对挠度1
                    System.Windows.Point newPoint20 = new System.Windows.Point();
                    newPoint20.X = span.TotalSeconds;
                    newPoint20.Y = ExpDQ.Exp_KFY.DisplaceGroups[0].ND_XD;
                    PlotLines[20].AddPoint(newPoint20);
                }
                if (ExpDQ.Exp_KFY.DisplaceGroups[1].Is_Use)
                {
                    //挠度2
                    System.Windows.Point newPoint18 = new System.Windows.Point();
                    newPoint18.X = span.TotalSeconds;
                    newPoint18.Y = ExpDQ.Exp_KFY.DisplaceGroups[1].ND;
                    PlotLines[18].AddPoint(newPoint18);
                    //相对挠度2
                    System.Windows.Point newPoint21 = new System.Windows.Point();
                    newPoint21.X = span.TotalSeconds;
                    newPoint21.Y = ExpDQ.Exp_KFY.DisplaceGroups[1].ND_XD;
                    PlotLines[21].AddPoint(newPoint21);
                }
                if (ExpDQ.Exp_KFY.DisplaceGroups[2].Is_Use)
                {
                    //挠度3
                    System.Windows.Point newPoint19 = new System.Windows.Point();
                    newPoint19.X = span.TotalSeconds;
                    newPoint19.Y = ExpDQ.Exp_KFY.DisplaceGroups[2].ND;
                    PlotLines[19].AddPoint(newPoint19);
                    //相对挠度3
                    System.Windows.Point newPoint22 = new System.Windows.Point();
                    newPoint22.X = span.TotalSeconds;
                    newPoint22.Y = ExpDQ.Exp_KFY.DisplaceGroups[2].ND_XD;
                    PlotLines[22].AddPoint(newPoint22);
                }
            }
        }


        #endregion

        #endregion


        #region 窗口打开、关闭消息（窗口互锁、绘图清零）

        /// <summary>
        /// 有窗口关闭消息
        /// </summary>
        /// <param name="msg"></param>
        private void WindowClosedMessage(string msg)
        {
            #region 窗口互锁用

            if (msg == MQZH_WinName.DbgWinName)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
            }

            else if (msg == MQZH_WinName.ExpSetWinName)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
            }

            else if (msg == MQZH_WinName.ExpManWinName)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
            }

            else if (msg == MQZH_WinName.QMWinName)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
            }

            else if (msg == MQZH_WinName.SMWinName)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
            }

            else if (msg == MQZH_WinName.KFYp1Name)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
            }

            else if (msg == MQZH_WinName.KFYp2Name)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
            }

            else if (msg == MQZH_WinName.KFYp3Name)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
            }

            else if (msg == MQZH_WinName.KFYpmaxName)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
            }

            else if (msg == MQZH_WinName.CJBXWinName)
            {
                IsDbgViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = true;
            }

            #endregion

            if (msg == MQZH_WinName.QMWinName)
            {
                IsQMWinOpened = false;
                foreach (var line in PlotLines)
                    line.LineDataSource.Collection.Clear();
            }
            else if (msg == MQZH_WinName.SMWinName)
            {
                IsSMWinOpened = false;
                foreach (var line in PlotLines)
                    line.LineDataSource.Collection.Clear();
            }

            else if ((msg == MQZH_WinName.KFYp1Name) || (msg == MQZH_WinName.KFYp2Name) || (msg == MQZH_WinName.KFYp3Name) || (msg == MQZH_WinName.KFYpmaxName))
            {
                IsKFYWinOpened = false;
                foreach (var line in PlotLines)
                    line.LineDataSource.Collection.Clear();
            }
            else if (msg == MQZH_WinName.CJBXWinName)
            {
                IsCJBXWinOpened = false;
                foreach (var line in PlotLines)
                    line.LineDataSource.Collection.Clear();
            }
        }

        /// <summary>
        /// 有窗口打开消息
        /// </summary>
        /// <param name="msgWindow"></param>
        private void WindowAddedMessage(Window msgWindow)
        {
            if (msgWindow.Name == MQZH_WinName.DbgWinName)
            {
                IsDbgViewCanBeOpened = true;
                IsExpSetViewCanBeOpened = false;
                IsExpManagerViewCanBeOpened = false;
                IsQMViewCanBeOpened = false;
                IsSMViewCanBeOpened = false;
                IsKFYp1ViewCanBeOpened = false;
                IsKFYp2ViewCanBeOpened = false;
                IsKFYp3ViewCanBeOpened = false;
                IsKFYpmaxViewCanBeOpened = false;
                IsCJBXViewCanBeOpened = false;

                IsDbgWinOpened = true;
            }

            else if (msgWindow.Name == MQZH_WinName.ExpSetWinName)
            {
                IsDbgViewCanBeOpened = false;
                IsExpSetViewCanBeOpened = true;
                IsExpManagerViewCanBeOpened = false;
                IsQMViewCanBeOpened = false;
                IsSMViewCanBeOpened = false;
                IsKFYp1ViewCanBeOpened = false;
                IsKFYp2ViewCanBeOpened = false;
                IsKFYp3ViewCanBeOpened = false;
                IsKFYpmaxViewCanBeOpened = false;
                IsCJBXViewCanBeOpened = false;
            }

            else if (msgWindow.Name == MQZH_WinName.ExpManWinName)
            {
                IsDbgViewCanBeOpened = false;
                IsExpSetViewCanBeOpened = false;
                IsExpManagerViewCanBeOpened = true;
                IsQMViewCanBeOpened = false;
                IsSMViewCanBeOpened = false;
                IsKFYp1ViewCanBeOpened = false;
                IsKFYp2ViewCanBeOpened = false;
                IsKFYp3ViewCanBeOpened = false;
                IsKFYpmaxViewCanBeOpened = false;
                IsCJBXViewCanBeOpened = false;
            }

            else if (msgWindow.Name == MQZH_WinName.QMWinName)
            {
                IsDbgViewCanBeOpened = false;
                IsExpSetViewCanBeOpened = false;
                IsExpManagerViewCanBeOpened = false;
                IsQMViewCanBeOpened = true;
                IsSMViewCanBeOpened = false;
                IsKFYp1ViewCanBeOpened = false;
                IsKFYp2ViewCanBeOpened = false;
                IsKFYp3ViewCanBeOpened = false;
                IsKFYpmaxViewCanBeOpened = false;
                IsCJBXViewCanBeOpened = false;
                
                IsQMWinOpened = true;
            }

            else if (msgWindow.Name == MQZH_WinName.SMWinName)
            {
                IsDbgViewCanBeOpened = false;
                IsExpSetViewCanBeOpened = false;
                IsExpManagerViewCanBeOpened = false;
                IsQMViewCanBeOpened = false;
                IsSMViewCanBeOpened = true;
                IsKFYp1ViewCanBeOpened = false;
                IsKFYp2ViewCanBeOpened = false;
                IsKFYp3ViewCanBeOpened = false;
                IsKFYpmaxViewCanBeOpened = false;
                IsCJBXViewCanBeOpened = false;

                IsSMWinOpened = true;
            }

            else if (msgWindow.Name == MQZH_WinName.KFYp1Name)
            {
                IsDbgViewCanBeOpened = false;
                IsExpSetViewCanBeOpened = false;
                IsExpManagerViewCanBeOpened = false;
                IsQMViewCanBeOpened = false;
                IsSMViewCanBeOpened = false;
                IsKFYp1ViewCanBeOpened = true;
                IsKFYp2ViewCanBeOpened = false;
                IsKFYp3ViewCanBeOpened = false;
                IsKFYpmaxViewCanBeOpened = false;
                IsCJBXViewCanBeOpened = false;

                IsKFYWinOpened = true;
            }

            else if (msgWindow.Name == MQZH_WinName.KFYp2Name)
            {
                IsDbgViewCanBeOpened = false;
                IsExpSetViewCanBeOpened = false;
                IsExpManagerViewCanBeOpened = false;
                IsQMViewCanBeOpened = false;
                IsSMViewCanBeOpened = false;
                IsKFYp1ViewCanBeOpened = false;
                IsKFYp2ViewCanBeOpened = true;
                IsKFYp3ViewCanBeOpened = false;
                IsKFYpmaxViewCanBeOpened = false;
                IsCJBXViewCanBeOpened = false;

                IsKFYWinOpened = true;
            }

            else if (msgWindow.Name == MQZH_WinName.KFYp3Name)
            {
                IsDbgViewCanBeOpened = false;
                IsExpSetViewCanBeOpened = false;
                IsExpManagerViewCanBeOpened = false;
                IsQMViewCanBeOpened = false;
                IsSMViewCanBeOpened = false;
                IsKFYp1ViewCanBeOpened = false;
                IsKFYp2ViewCanBeOpened = false;
                IsKFYp3ViewCanBeOpened = true;
                IsKFYpmaxViewCanBeOpened = false;
                IsCJBXViewCanBeOpened = false;

                IsKFYWinOpened = true;
            }

            else if (msgWindow.Name == MQZH_WinName.KFYpmaxName)
            {
                IsDbgViewCanBeOpened = false;
                IsExpSetViewCanBeOpened = false;
                IsExpManagerViewCanBeOpened = false;
                IsQMViewCanBeOpened = false;
                IsSMViewCanBeOpened = false;
                IsKFYp1ViewCanBeOpened = false;
                IsKFYp2ViewCanBeOpened = false;
                IsKFYp3ViewCanBeOpened = false;
                IsKFYpmaxViewCanBeOpened = true;
                IsCJBXViewCanBeOpened = false;

                IsKFYWinOpened = true;
            }

            else if (msgWindow.Name == MQZH_WinName.CJBXWinName)
            {
                IsDbgViewCanBeOpened = false;
                IsExpSetViewCanBeOpened = false;
                IsExpManagerViewCanBeOpened = false;
                IsQMViewCanBeOpened = false;
                IsSMViewCanBeOpened = false;
                IsKFYp1ViewCanBeOpened = false;
                IsKFYp2ViewCanBeOpened = false;
                IsKFYp3ViewCanBeOpened = false;
                IsKFYpmaxViewCanBeOpened = false;
                IsCJBXViewCanBeOpened = true;

                IsCJBXWinOpened = true;
            }
            else if (msgWindow.Name == MQZH_WinName.CurrentDataWinName)
                NDPlotUpdate();
        }


        #region 窗口已打开属性

        /// <summary>
        /// 调试窗口已打开
        /// </summary>
        private bool _isDbgWinOpened = false;
        /// <summary>
        /// 调试窗口已打开
        /// </summary>
        public bool IsDbgWinOpened
        {
            get { return _isDbgWinOpened; }
            set
            {
                _isDbgWinOpened = value;
                RaisePropertyChanged(() => IsDbgWinOpened);
            }
        }

        /// <summary>
        /// 气密检测窗口已打开
        /// </summary>
        private bool _isQMWinOpened = false;
        /// <summary>
        /// 气密检测窗口已打开
        /// </summary>
        public bool IsQMWinOpened
        {
            get { return _isQMWinOpened; }
            set
            {
                _isQMWinOpened = value;
                RaisePropertyChanged(() => IsQMWinOpened);
            }
        }

        /// <summary>
        /// 水密检测窗口已打开
        /// </summary>
        private bool _isSMWinOpened = false;
        /// <summary>
        /// 水密检测窗口已打开
        /// </summary>
        public bool IsSMWinOpened
        {
            get { return _isSMWinOpened; }
            set
            {
                _isSMWinOpened = value;
                RaisePropertyChanged(() => IsSMWinOpened);
            }
        }

        /// <summary>
        /// 抗风压窗口已打开
        /// </summary>
        private bool _isKFYWinOpened = false;
        /// <summary>
        /// 抗风压窗口已打开
        /// </summary>
        public bool IsKFYWinOpened
        {
            get { return _isKFYWinOpened; }
            set
            {
                _isKFYWinOpened = value;
                RaisePropertyChanged(() => IsKFYWinOpened);
            }
        }

        /// <summary>
        /// 层间变形窗口已打开
        /// </summary>
        private bool _isCJBXWinOpened = false;
        /// <summary>
        /// 层间变形窗口已打开
        /// </summary>
        public bool IsCJBXWinOpened
        {
            get { return _isCJBXWinOpened; }
            set
            {
                _isCJBXWinOpened = value;
                RaisePropertyChanged(() => IsCJBXWinOpened);
            }
        }

        #endregion


        #region 窗口可以打开状态属性

        /// <summary>
        /// DbgView可以打开状态
        /// </summary>
        private bool _isDbgViewCanBeOpened = true;
        /// <summary>
        /// DbgView可以打开状态
        /// </summary>
        public bool IsDbgViewCanBeOpened
        {
            get { return _isDbgViewCanBeOpened; }
            set
            {
                _isDbgViewCanBeOpened = value;
                RaisePropertyChanged(() => IsDbgViewCanBeOpened);
            }
        }

        /// <summary>
        /// QMView可以打开状态
        /// </summary>
        private bool _isQMViewCanBeOpened = true;
        /// <summary>
        /// QMView可以打开状态
        /// </summary>
        public bool IsQMViewCanBeOpened
        {
            get { return _isQMViewCanBeOpened; }
            set
            {
                _isQMViewCanBeOpened = value;
                RaisePropertyChanged(() => IsQMViewCanBeOpened);
            }
        }

        /// <summary>
        /// SMView可以打开状态
        /// </summary>
        private bool _isSMViewCanBeOpened = true;
        /// <summary>
        /// SMView可以打开状态
        /// </summary>
        public bool IsSMViewCanBeOpened
        {
            get { return _isSMViewCanBeOpened; }
            set
            {
                _isSMViewCanBeOpened = value;
                RaisePropertyChanged(() => IsSMViewCanBeOpened);
            }
        }

        /// <summary>
        /// CJBXView可以打开状态
        /// </summary>
        private bool _isCJBXViewCanBeOpened = true;
        /// <summary>
        /// CJBXView可以打开状态
        /// </summary>
        public bool IsCJBXViewCanBeOpened
        {
            get { return _isCJBXViewCanBeOpened; }
            set
            {
                _isCJBXViewCanBeOpened = value;
                RaisePropertyChanged(() => IsCJBXViewCanBeOpened);
            }
        }

        /// <summary>
        /// ExpSetView可以打开状态
        /// </summary>
        private bool _isExpSetViewCanBeOpened = true;
        /// <summary>
        /// ExpSetView可以打开状态
        /// </summary>
        public bool IsExpSetViewCanBeOpened
        {
            get { return _isExpSetViewCanBeOpened; }
            set
            {
                _isExpSetViewCanBeOpened = value;
                RaisePropertyChanged(() => IsExpSetViewCanBeOpened);
            }
        }

        /// <summary>
        /// ExpManagerView可以打开状态
        /// </summary>
        private bool _isExpManagerViewCanBeOpened = true;
        /// <summary>
        /// ExpManagerView可以打开状态
        /// </summary>
        public bool IsExpManagerViewCanBeOpened
        {
            get { return _isExpManagerViewCanBeOpened; }
            set
            {
                _isExpManagerViewCanBeOpened = value;
                RaisePropertyChanged(() => IsExpManagerViewCanBeOpened);
            }
        }

        /// <summary>
        /// KFYp1View可以打开状态
        /// </summary>
        private bool _isKFYp1ViewCanBeOpened = true;
        /// <summary>
        /// KFYp1View可以打开状态
        /// </summary>
        public bool IsKFYp1ViewCanBeOpened
        {
            get { return _isKFYp1ViewCanBeOpened; }
            set
            {
                _isKFYp1ViewCanBeOpened = value;
                RaisePropertyChanged(() => IsKFYp1ViewCanBeOpened);
            }
        }

        /// <summary>
        /// KFYp2View可以打开状态
        /// </summary>
        private bool _isKFYp2ViewCanBeOpened = true;
        /// <summary>
        /// KFYp2View可以打开状态
        /// </summary>
        public bool IsKFYp2ViewCanBeOpened
        {
            get { return _isKFYp2ViewCanBeOpened; }
            set
            {
                _isKFYp2ViewCanBeOpened = value;
                RaisePropertyChanged(() => IsKFYp2ViewCanBeOpened);
            }
        }

        /// <summary>
        /// KFYp3View可以打开状态
        /// </summary>
        private bool _isKFYp3ViewCanBeOpened = true;
        /// <summary>
        /// KFYp3View可以打开状态
        /// </summary>
        public bool IsKFYp3ViewCanBeOpened
        {
            get { return _isKFYp3ViewCanBeOpened; }
            set
            {
                _isKFYp3ViewCanBeOpened = value;
                RaisePropertyChanged(() => IsKFYp3ViewCanBeOpened);
            }
        }

        /// <summary>
        /// KFYpmaxXView可以打开状态
        /// </summary>
        private bool _isKFYpmaxViewCanBeOpened = true;
        /// <summary>
        /// KFYpmaxView可以打开状态
        /// </summary>
        public bool IsKFYpmaxViewCanBeOpened
        {
            get { return _isKFYpmaxViewCanBeOpened; }
            set
            {
                _isKFYpmaxViewCanBeOpened = value;
                RaisePropertyChanged(() => IsKFYpmaxViewCanBeOpened);
            }
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
            if (Dev.PassWord == "brt12345678")
            {
                Dev.IsAdmin = true;
                MessageBox.Show("管理员登录成功，配置完成后请退出登录或重启软件！");
            }
            else
            {
                Dev.IsAdmin = false;
                MessageBox.Show("管理员密码错误！");
            }
        }
        #endregion
    }
}