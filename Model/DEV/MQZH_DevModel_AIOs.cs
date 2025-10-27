/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 描述：
 *
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022-05-26 10:54:56		郝正强			V1.0.0.0
 * 2024-01-03 15:54:00		郝正强			V2.0.0.0        改为单风机版
 ************************************************************************************/

using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace MQZHWL.Model.DEV
{
    public partial class MQZH_DevModel_Main : ObservableObject
    {
        #region 模拟量模块

        /// <summary>
        /// DVP-04AD模块1
        /// </summary>
        private AnalogModuleModel _mod_04AD1 = new AnalogModuleModel()
        {
            ModuleNO = "04AD-1",
            ModuleName = "04AD模块1",
            Channels = new ObservableCollection<AnalogChannelModel>()
            {
                new AnalogChannelModel(),
                new AnalogChannelModel(),
                new AnalogChannelModel(),
                new AnalogChannelModel()
            }
        };

        /// <summary>
        /// DVP-04AD模块1
        /// </summary>
        public AnalogModuleModel Mod_04AD1
        {
            get { return _mod_04AD1; }
            set
            {
                _mod_04AD1 = value;
                RaisePropertyChanged(() => Mod_04AD1);
            }
        }

        /// <summary>
        /// DVP-04AD模块2
        /// </summary>
        private AnalogModuleModel _mod_04AD2 = new AnalogModuleModel()
        {
            ModuleNO = "04AD-2",
            ModuleName = "04AD模块2",
            Channels = new ObservableCollection<AnalogChannelModel>()
            {
                new AnalogChannelModel(),
                new AnalogChannelModel(),
                new AnalogChannelModel(),
                new AnalogChannelModel()
            }
        };
        /// <summary>
        /// DVP-04AD模块2
        /// </summary>
        public AnalogModuleModel Mod_04AD2
        {
            get { return _mod_04AD2; }
            set
            {
                _mod_04AD2 = value;
                RaisePropertyChanged(() => Mod_04AD2);
            }
        }

        /// <summary>
        /// DVP-04AD模块3
        /// </summary>
        private AnalogModuleModel _mod_04AD3 = new AnalogModuleModel()
        {
            ModuleNO = "04AD-3",
            ModuleName = "04AD模块3",
            Channels = new ObservableCollection<AnalogChannelModel>()
            {
                new AnalogChannelModel(),
                new AnalogChannelModel(),
                new AnalogChannelModel(),
                new AnalogChannelModel()
            }
        };
        /// <summary>
        /// DVP-04AD模块3
        /// </summary>
        public AnalogModuleModel Mod_04AD3
        {
            get { return _mod_04AD3; }
            set
            {
                _mod_04AD3 = value;
                RaisePropertyChanged(() => Mod_04AD3);
            }
        }

        /// <summary>
        /// DVP-04AD模块4
        /// </summary>
        private AnalogModuleModel _mod_04AD4 = new AnalogModuleModel()
        {
            ModuleNO = "04AD-4",
            ModuleName = "04AD模块4",
            Channels = new ObservableCollection<AnalogChannelModel>()
            {
                new AnalogChannelModel(),
                new AnalogChannelModel(),
                new AnalogChannelModel(),
                new AnalogChannelModel()
            }
        };
        /// <summary>
        /// DVP-04AD模块4
        /// </summary>
        public AnalogModuleModel Mod_04AD4
        {
            get { return _mod_04AD4; }
            set
            {
                _mod_04AD4 = value;
                RaisePropertyChanged(() => Mod_04AD4);
            }
        }

        /// <summary>
        /// DVP-04AD模块5
        /// </summary>
        private AnalogModuleModel _mod_04AD5 = new AnalogModuleModel()
        {
            ModuleNO = "04AD-5",
            ModuleName = "04AD模块5",
            Channels = new ObservableCollection<AnalogChannelModel>()
            {
                new AnalogChannelModel(),
                new AnalogChannelModel(),
                new AnalogChannelModel(),
                new AnalogChannelModel()
            }
        };
        /// <summary>
        /// DVP-04AD模块5
        /// </summary>
        public AnalogModuleModel Mod_04AD5
        {
            get { return _mod_04AD5; }
            set
            {
                _mod_04AD5 = value;
                RaisePropertyChanged(() => Mod_04AD5);
            }
        }

        /// <summary>
        /// 频率显示
        /// </summary>
        private AnalogModuleModel _mod_DAView = new AnalogModuleModel()
        {
            ModuleNO = "DAView",
            ModuleName = "DAView",
            Channels = new ObservableCollection<AnalogChannelModel>()
            {
                new AnalogChannelModel(),
                new AnalogChannelModel(),
                new AnalogChannelModel(),
                new AnalogChannelModel()
            }
        };
        /// <summary>
        /// 频率显示
        /// </summary>
        public AnalogModuleModel Mod_DAView
        {
            get { return _mod_DAView; }
            set
            {
                _mod_DAView = value;
                RaisePropertyChanged(() => Mod_DAView);
            }
        }

        /// <summary>
        /// DA模块
        /// </summary>
        private AnalogModuleModel _mod_DA = new AnalogModuleModel()
        {
            ModuleNO = "DA-1",
            ModuleName = "DA模块1",
            Channels = new ObservableCollection<AnalogChannelModel>()
            {
                new AnalogChannelModel(),
                new AnalogChannelModel(),
                new AnalogChannelModel(),
                new AnalogChannelModel()
            }
        };
        /// <summary>
        /// DA模块
        /// </summary>
        public AnalogModuleModel Mod_DA
        {
            get { return _mod_DA; }
            set
            {
                _mod_DA = value;
                RaisePropertyChanged(() => Mod_DA);
            }
        }
        
        /// <summary>
        /// 三合一传感器
        /// </summary>
        private AnalogModuleModel _mod_THP = new AnalogModuleModel()
        {
            ModuleNO = "THP",
            ModuleName = "三合一传感器",
            Channels = new ObservableCollection<AnalogChannelModel>()
            {
                new AnalogChannelModel(),
                new AnalogChannelModel(),
                new AnalogChannelModel()
            }
        };
        /// <summary>
        /// 三合一传感器
        /// </summary>
        public AnalogModuleModel Mod_THP
        {
            get { return _mod_THP; }
            set
            {
                _mod_THP = value;
                RaisePropertyChanged(() => Mod_THP);
            }
        }

        #endregion


        #region 模拟量参数

        /// <summary>
        /// 模拟量输入参数列表（00位移尺1、01位移尺2、02位移尺3、03位移尺4、04位移尺5、05位移尺6、
        /// 06位移尺7、07位移尺8、08位移尺9、09位移尺10、10位移尺11、11位移尺12、
        /// 12小差压、13中压差、14大差压、15风速1、16风速2、17风速3、18水流速、
        /// 19环境温度、20环境湿度、21大气压力、22风机频率显示、23水泵频率显示）
        /// </summary>
        private ObservableCollection<AnalogModel> _aiList = new ObservableCollection<AnalogModel>()
        {
            new AnalogModel(), new AnalogModel(), new AnalogModel(), new AnalogModel(),
            new AnalogModel(), new AnalogModel(), new AnalogModel(), new AnalogModel(),
            new AnalogModel(), new AnalogModel(), new AnalogModel(), new AnalogModel(),
            new AnalogModel(), new AnalogModel(), new AnalogModel(), new AnalogModel(),
            new AnalogModel(), new AnalogModel(), new AnalogModel(), new AnalogModel(),
            new AnalogModel(), new AnalogModel(), new AnalogModel(),new AnalogModel()
        };
        /// <summary>
        /// 模拟量输入参数列表（00位移尺1、01位移尺2、02位移尺3、03位移尺4、04位移尺5、05位移尺6、
        /// 06位移尺7、07位移尺8、08位移尺9、09位移尺10、10位移尺11、11位移尺12、
        /// 12小差压、13中压差、14大差压、15风速1、16风速2、17风速3、18水流速、
        /// 19环境温度、20环境湿度、21大气压力、22风机频率显示、23水泵频率显示）
        /// </summary>
        public ObservableCollection<AnalogModel> AIList
        {
            get { return _aiList; }
            set
            {
                _aiList = value;
                RaisePropertyChanged(() => AIList);
            }
        }

        /// <summary>
        /// 模拟量输出参数列表（风机频率、水泵频率）
        /// </summary>
        private ObservableCollection<AnalogModel> _aoList = new ObservableCollection<AnalogModel>()
        {
            new AnalogModel(),
            new AnalogModel()
        };
        /// <summary>
        /// 模拟量输出参数列表（风机频率、水泵频率）
        /// </summary>
        public ObservableCollection<AnalogModel> AOList
        {
            get { return _aoList; }
            set
            {
                _aoList = value;
                RaisePropertyChanged(() => AOList);
            }
        }

        #endregion


        #region 辅助参数
        
        /// <summary>
        /// 风量1
        /// </summary>
        private double _fl1 = 0;
        /// <summary>
        /// 风量1
        /// </summary>
        public double FL1
        {
            get { return _fl1;}
            set
            {
                _fl1 = value;
                RaisePropertyChanged(() => FL1);
            }
        }

        /// <summary>
        /// 风量2
        /// </summary>
        private double _fl2 = 0;
        /// <summary>
        /// 风量2
        /// </summary>
        public double FL2
        {
            get { return _fl2; }
            set
            {
                _fl2 = value;
                RaisePropertyChanged(() => FL2);
            }
        }

        /// <summary>
        /// 风量3
        /// </summary>
        private double _fl3 = 0;
        /// <summary>
        /// 风量3
        /// </summary>
        public double FL3
        {
            get { return _fl3; }
            set
            {
                _fl3 = value;
                RaisePropertyChanged(() => FL3);
            }
        }

        /// <summary>
        /// 水流量
        /// </summary>
        private double _sll = 0;
        /// <summary>
        /// 水流量
        /// </summary>
        public double SLL
        {
            get { return _sll; }
            set
            {
                _sll = value;
                RaisePropertyChanged(() => SLL);
            }
        }

        /// <summary>
        /// X轴位移值
        /// </summary>
        private double _wyValueX = 0;
        /// <summary>
        /// X轴位移值
        /// </summary>
        public double WYValueX
        {
            get { return _wyValueX; }
            set
            {
                _wyValueX = value;
                RaisePropertyChanged(() => WYValueX);
            }
        }

        /// <summary>
        /// Y轴位移值（左、中、右、平均）
        /// </summary>
        private ObservableCollection<double> _wyValueY = new ObservableCollection<double>(){0,0,0,0};
        /// <summary>
        /// Y轴位移值（左、中、右、平均）
        /// </summary>
        public ObservableCollection<double> WYValueY
        {
            get { return _wyValueY; }
            set
            {
                _wyValueY = value;
                RaisePropertyChanged(() => WYValueY);
            }
        }

        /// <summary>
        /// z轴位移值（左、中、右、平均）
        /// </summary>
        private ObservableCollection<double> _wyValueZ = new ObservableCollection<double>() { 0, 0, 0, 0 };
        /// <summary>
        /// z轴位移值（左、中、右、平均）
        /// </summary>
        public ObservableCollection<double> WYValueZ
        {
            get { return _wyValueZ; }
            set
            {
                _wyValueZ = value;
                RaisePropertyChanged(() => WYValueZ);
            }
        }

        #endregion


        #region 调零

        /// <summary>
        ///  模拟量自动调零。y=k*x+b，b=y0-k*x0
        /// </summary>
        private void SetZeroMessage(string msg)
        {
            string order = msg.Clone().ToString();
            if (order == "WY01")
            {
                AIList[0].ZeroCalValue = 0 - AIList[0].ValueCaledNonZero * AIList[0].KCalValue;
            }
            else if (order == "WY02")
            {
                AIList[1].ZeroCalValue = 0 - AIList[1].ValueCaledNonZero * AIList[1].KCalValue;
            }

            else if (order == "WY03")
            {
                AIList[2].ZeroCalValue = 0 - AIList[2].ValueCaledNonZero * AIList[2].KCalValue;
            }
            else if (order == "WY04")
            {
                AIList[3].ZeroCalValue = 0 - AIList[3].ValueCaledNonZero * AIList[3].KCalValue;
            }
            else if (order == "WY05")
            {
                AIList[4].ZeroCalValue = 0 - AIList[4].ValueCaledNonZero * AIList[4].KCalValue;
            }
            else if (order == "WY06")
            {
                AIList[5].ZeroCalValue = 0 - AIList[5].ValueCaledNonZero * AIList[5].KCalValue;
            }
            else if (order == "WY07")
            {
                AIList[6].ZeroCalValue = 0 - AIList[6].ValueCaledNonZero * AIList[6].KCalValue;
            }
            else if (order == "WY08")
            {
                AIList[7].ZeroCalValue = 0 - AIList[7].ValueCaledNonZero * AIList[7].KCalValue;
            }
            else if (order == "WY09")
            {
                AIList[8].ZeroCalValue = 0 - AIList[8].ValueCaledNonZero * AIList[8].KCalValue;
            }
            else if (order == "WY10")
            {
                AIList[9].ZeroCalValue = 0 - AIList[9].ValueCaledNonZero * AIList[9].KCalValue;
            }
            else if (order == "WY11")
            {
                AIList[10].ZeroCalValue = 0 - AIList[10].ValueCaledNonZero * AIList[10].KCalValue;
            }
            else if (order == "WY12")
            {
                AIList[11].ZeroCalValue = 0 - AIList[11].ValueCaledNonZero * AIList[11].KCalValue;
            }
            
            else if (order == "CY01")
            {
                AIList[12].ZeroCalValue = 0 - AIList[12].ValueCaledNonZero * AIList[12].KCalValue;
            }
            else if (order == "CY02")
            {
                AIList[13].ZeroCalValue = 0 - AIList[13].ValueCaledNonZero * AIList[13].KCalValue;
            }
            else if (order == "CY03")
            {
                AIList[14].ZeroCalValue = 0 - AIList[14].ValueCaledNonZero * AIList[14].KCalValue;
            }

            else if (order == "FS01")
            {
                AIList[15].ZeroCalValue = 0 - AIList[15].ValueCaledNonZero * AIList[15].KCalValue;
            }
            else if (order == "FS02")
            {
                AIList[16].ZeroCalValue = 0 - AIList[16].ValueCaledNonZero * AIList[16].KCalValue;
            }
            else if (order == "FS03")
            {
                AIList[17].ZeroCalValue = 0 - AIList[17].ValueCaledNonZero * AIList[17].KCalValue;
            }
            else if (order == "SLS")
            {
                AIList[18].ZeroCalValue = 0 - AIList[18].ValueCaledNonZero * AIList[18].KCalValue;
            }

            else if (order == "X")
            {
                AIList[WYNOList[0]-1].ZeroCalValue = 0-AIList[WYNOList[0] - 1].ValueCaledNonZero * AIList[WYNOList[0] - 1].KCalValue;
            }
            else if (order == "Y")
            {
                AIList[WYNOList[1] - 1].ZeroCalValue = 0 - AIList[WYNOList[1] - 1].ValueCaledNonZero * AIList[WYNOList[1] - 1].KCalValue;
                AIList[WYNOList[2] - 1].ZeroCalValue = 0 - AIList[WYNOList[2] - 1].ValueCaledNonZero * AIList[WYNOList[2] - 1].KCalValue;
                AIList[WYNOList[3] - 1].ZeroCalValue = 0 - AIList[WYNOList[3] - 1].ValueCaledNonZero * AIList[WYNOList[3] - 1].KCalValue;
            }
            else if (order == "Z")
            {
                AIList[WYNOList[4] - 1].ZeroCalValue = 0 - AIList[WYNOList[4] - 1].ValueCaledNonZero * AIList[WYNOList[4] - 1].KCalValue;
                AIList[WYNOList[5] - 1].ZeroCalValue = 0 - AIList[WYNOList[5] - 1].ValueCaledNonZero * AIList[WYNOList[5] - 1].KCalValue;
                AIList[WYNOList[6] - 1].ZeroCalValue = 0 - AIList[WYNOList[6] - 1].ValueCaledNonZero * AIList[WYNOList[6] - 1].KCalValue;
            }
            else if (order == "XYZ")
            {
                AIList[WYNOList[0] - 1].ZeroCalValue = 0 - AIList[WYNOList[0] - 1].ValueCaledNonZero * AIList[WYNOList[0] - 1].KCalValue;
                AIList[WYNOList[1] - 1].ZeroCalValue = 0 - AIList[WYNOList[1] - 1].ValueCaledNonZero * AIList[WYNOList[1] - 1].KCalValue;
                AIList[WYNOList[2] - 1].ZeroCalValue = 0 - AIList[WYNOList[2] - 1].ValueCaledNonZero * AIList[WYNOList[2] - 1].KCalValue;
                AIList[WYNOList[3] - 1].ZeroCalValue = 0 - AIList[WYNOList[3] - 1].ValueCaledNonZero * AIList[WYNOList[3] - 1].KCalValue;
                AIList[WYNOList[4] - 1].ZeroCalValue = 0 - AIList[WYNOList[4] - 1].ValueCaledNonZero * AIList[WYNOList[4] - 1].KCalValue;
                AIList[WYNOList[5] - 1].ZeroCalValue = 0 - AIList[WYNOList[5] - 1].ValueCaledNonZero * AIList[WYNOList[5] - 1].KCalValue;
                AIList[WYNOList[6] - 1].ZeroCalValue = 0 - AIList[WYNOList[6] - 1].ValueCaledNonZero * AIList[WYNOList[6] - 1].KCalValue;
            }
        }

        #endregion


        #region 下拉列表项

        /// <summary>
        /// 接口类型列表
        /// </summary>
        public ObservableCollection<string> _infTypeList = new ObservableCollection<string>() { "Voltage", "Current", "Digital", "Couple" };
        /// <summary>
        /// 接口类型列表
        /// </summary>
        public ObservableCollection<string> InfTypeList
        {
            get { return _infTypeList; }
        }

        /// <summary>
        /// 电信号单位列表
        /// </summary>
        public ObservableCollection<string> _elecSigUnitList = new ObservableCollection<string>() { "mA", "A", "V", "mV" };
        /// <summary>
        /// 标定点启用标志
        /// </summary>
        public ObservableCollection<string> ElecSigUnitList
        {
            get { return _elecSigUnitList; }
        }

        #endregion

    }
}