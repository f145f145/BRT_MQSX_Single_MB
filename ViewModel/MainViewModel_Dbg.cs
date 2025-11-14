using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using GalaSoft.MvvmLight.Command;
using MQDFJ_MB.Model;
using System.Linq;
using static MQDFJ_MB.Model.MQZH_Enums;


namespace MQDFJ_MB.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {




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
                PublicData.Dev.DeviceRunMode = DevicRunModeType.JJTJ_Mode;
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
        private bool _sdtsDF1 = true;
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
                PublicData.Dev.DeviceRunMode = DevicRunModeType.JJTJ_Mode;
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
                ushort[] outData = new ushort[] { 0, 0, 0, 0, Convert.ToUInt16(PL1_SDTS * 640) };
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
                PublicData.Dev.DeviceRunMode = DevicRunModeType.JJTJ_Mode;
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
                if ((PublicData.Dev.DeviceRunMode == DevicRunModeType.DF1QMFDbg_Mode) ||
                    (PublicData.Dev.DeviceRunMode == DevicRunModeType.DF2QMFDbg_Mode) ||
                    (PublicData.Dev.DeviceRunMode == DevicRunModeType.DF3QMFDbg_Mode) ||
                    (PublicData.Dev.DeviceRunMode == DevicRunModeType.DF4QMFDbg_Mode))
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
                switch (PublicData.Dev.DFNo_FG1)
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
                switch (PublicData.Dev.DFNo_FG1)
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
                switch (PublicData.Dev.DFNo_FG2)
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
                switch (PublicData.Dev.DFNo_FG2)
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
                switch (PublicData.Dev.DFNo_FG3)
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
                switch (PublicData.Dev.DFNo_FG3)
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
                PublicData.Dev.DeviceRunMode = DevicRunModeType.JJTJ_Mode;
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
                if (PublicData.Dev.DeviceRunMode == DevicRunModeType.SDSMFDbg_Mode)
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
                PublicData.Dev.DeviceRunMode = DevicRunModeType.JJTJ_Mode;
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
                PublicData.Dev.DeviceRunMode = DevicRunModeType.JJTJ_Mode;
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
                if (Math.Abs(YDJL_X) <= Math.Abs(PublicData.Dev.PermitErrX))
                    return;
                if (YDJL_X == 0)
                    return;
                //将相对位置换算为目标点位采集值
                if (YDJL_X > 0)
                    distance = YDJL_X + PublicData.Dev.CorrXRight;
                if (YDJL_X < 0)
                    distance = YDJL_X - PublicData.Dev.CorrXLeft;
                double aimPositionX = PublicData.Dev.WYValueX + distance;        //目标位置
                double dataTrans = PublicData.Dev.AIList[PublicData.Dev.WYNOList[0] - 1].GetTransDataFromValueFinal_AI(-aimPositionX);       //X轴位移尺对应传输值

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
                SDWYOrderArray[17] = Convert.ToUInt16(PublicData.Dev.WYNOList[0] - 1);      //X位移尺编号
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }
            //Y轴移动到目标位置
            else if (i == 6517)
            {
                double distance = 0;
                if (Math.Abs(YDJL_Y) <= Math.Abs(PublicData.Dev.PermitErrY))
                    return;
                if (YDJL_Y == 0)
                    return;
                //将相对位置换算为目标点位采集值
                if (YDJL_Y > 0)
                    distance = YDJL_Y + PublicData.Dev.CorrYFront;
                if (YDJL_Y < 0)
                    distance = YDJL_Y - PublicData.Dev.CorrYBack;
                double aimPositionY = PublicData.Dev.WYValueY[3] + distance;        //目标位置
                double dataTrans1 = PublicData.Dev.AIList[PublicData.Dev.WYNOList[1] - 1].GetTransDataFromValueFinal_AI(-aimPositionY);       //左点对应传输值
                double dataTrans2 = PublicData.Dev.AIList[PublicData.Dev.WYNOList[2] - 1].GetTransDataFromValueFinal_AI(-aimPositionY);       //中间点对应传输值
                double dataTrans3 = PublicData.Dev.AIList[PublicData.Dev.WYNOList[3] - 1].GetTransDataFromValueFinal_AI(-aimPositionY);       //右点对应传输值
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
                SDWYOrderArray[18] = Convert.ToUInt16(PublicData.Dev.WYNOList[1] - 1);     //Y左位移尺编号
                SDWYOrderArray[19] = Convert.ToUInt16(PublicData.Dev.WYNOList[2] - 1);     //Y中位移尺编号
                SDWYOrderArray[20] = Convert.ToUInt16(PublicData.Dev.WYNOList[3] - 1);     //Y右位移尺编号
                Messenger.Default.Send<ushort[]>(SDWYOrderArray, "SDWYMessage");
            }
            //Z轴移动到目标位置
            else if (i == 6518)
            {
                double distance = 0;
                if (Math.Abs(YDJL_Z) <= Math.Abs(PublicData.Dev.PermitErrZ))
                    return;
                if (YDJL_Z == 0)
                    return;
                //将相对位置换算为目标点位采集值
                if (YDJL_Z > 0)
                    distance = YDJL_Z + PublicData.Dev.CorrZUp;
                if (YDJL_Z < 0)
                    distance = YDJL_Z - PublicData.Dev.CorrZDown;
                double aimPositionZ = PublicData.Dev.WYValueZ[3] + distance;        //目标位置
                double dataTrans1 = PublicData.Dev.AIList[PublicData.Dev.WYNOList[4] - 1].GetTransDataFromValueFinal_AI(aimPositionZ);       //左点对应传输值
                double dataTrans2 = PublicData.Dev.AIList[PublicData.Dev.WYNOList[5] - 1].GetTransDataFromValueFinal_AI(aimPositionZ);       //中间点对应传输值
                double dataTrans3 = PublicData.Dev.AIList[PublicData.Dev.WYNOList[6] - 1].GetTransDataFromValueFinal_AI(aimPositionZ);       //右点对应传输值
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
                SDWYOrderArray[21] = Convert.ToUInt16(PublicData.Dev.WYNOList[4] - 1);     //Z左位移尺编号
                SDWYOrderArray[22] = Convert.ToUInt16(PublicData.Dev.WYNOList[5] - 1);     //Z中位移尺编号
                SDWYOrderArray[23] = Convert.ToUInt16(PublicData.Dev.WYNOList[6] - 1);     //Z右位移尺编号
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

    }
}
