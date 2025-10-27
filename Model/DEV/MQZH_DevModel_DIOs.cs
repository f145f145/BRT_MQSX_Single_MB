/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 描述：
 * 装置Model，DIO部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022-05-26 10:54:56		郝正强			V1.0.0.0
 * 2024-01-03 15:54:00		郝正强			V2.0.0.0        改为单风机版
 ************************************************************************************/

using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace MQDFJ_MB.Model.DEV
{
    public partial class MQZH_DevModel_Main : ObservableObject
    {
        #region 数字量模块

        /// <summary>
        /// 虚拟DI模块（内部关联转换，M100-M131）
          /// </summary>
        private DigitalModuleModel _mod_XNDI = new DigitalModuleModel()
        {
            ModuleNO = "XNDI",
            ModuleName = "虚拟DI模块",
            Channels = new ObservableCollection<DigitalChannelModel>()
            {
                new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(),new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(),
                new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(),new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(),
                new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(),new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(),
                new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(),new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel()
            }
        };
        /// <summary>
        /// 虚拟DI模块（内部关联转换，M100-M131）
        /// </summary>
        public DigitalModuleModel Mod_XNDI
        {
            get { return _mod_XNDI; }
            set
            {
                _mod_XNDI = value;
                RaisePropertyChanged(() => Mod_XNDI);
            }
        }

        /// <summary>
        /// 虚拟DO模块（内部关联转换,M400-M417）
        /// </summary>
        private DigitalModuleModel _mod_XNDO = new DigitalModuleModel()
        {
            ModuleNO = "XNDO",
            ModuleName = "虚拟DO模块",
            Channels = new ObservableCollection<DigitalChannelModel>()
            {
                new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(),
                new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(),
                new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(),
                new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(),
                new DigitalChannelModel(), new DigitalChannelModel()
            }
        };
        /// <summary>
        /// 虚拟DO模块（内部关联转换,M400-M417）
        /// </summary>
        public DigitalModuleModel Mod_XNDO
        {
            get { return _mod_XNDO; }
            set
            {
                _mod_XNDO = value;
                RaisePropertyChanged(() => Mod_XNDO);
            }
        }

        #endregion


        #region 数字量参数

        /// <summary>
        /// 数字量输入参数列表。
        /// </summary>
        /// <remarks>M100主管蝶阀1开状态，M101粗管蝶阀2开状态，M102细管蝶阀3开状态，M103加粗管蝶阀开状态，M104差压阀开状态，
        /// M105KM1吸合状态，M106水泵运行状态，M107气泵运行状态，M108风机变频启动状态，M109水泵变频启动状态，
        /// M110液压站运行状态，M111液压总阀开状态，M112X轴向右状态，M113X轴向左状态，M114Y轴向前状态，M115Y轴向后状态，M116Z轴向上状态，M117Z轴向下状态，M118X定点到位，M119Y定点到位，M120Z定点到位，
        /// M130相序正常状态，M131通讯闪烁状态</remarks>

        private ObservableCollection<DigitalModel> _diList = new ObservableCollection<DigitalModel>()
        {
            new DigitalModel(){_wzOn ="已开启",_wzOff ="未开启"},  
            new DigitalModel(){_wzOn ="已开启",_wzOff ="未开启"},
            new DigitalModel(){_wzOn ="已开启",_wzOff ="未开启"},
            new DigitalModel(){_wzOn ="已开启",_wzOff ="未开启"},
            new DigitalModel(){_wzOn ="已开启",_wzOff ="未开启"},

            new DigitalModel(){_wzOn ="已吸合",_wzOff ="未吸合"},
            new DigitalModel(){_wzOn ="已启动",_wzOff ="停机中"},
            new DigitalModel(){_wzOn ="已启动",_wzOff ="停机中"},
            new DigitalModel(){_wzOn ="已启动",_wzOff ="停机中"},
            new DigitalModel(){_wzOn ="已启动",_wzOff ="停机中"},

            new DigitalModel(){_wzOn ="已启动",_wzOff ="停机中"},
            new DigitalModel(){_wzOn ="已开启",_wzOff ="未开启"},  
            new DigitalModel(){_wzOn ="已开启",_wzOff ="未开启"},
            new DigitalModel(){_wzOn ="已开启",_wzOff ="未开启"},  
            new DigitalModel(){_wzOn ="已开启",_wzOff ="未开启"},
            new DigitalModel(){_wzOn ="已开启",_wzOff ="未开启"},  
            new DigitalModel(){_wzOn ="已开启",_wzOff ="未开启"},
            new DigitalModel(){_wzOn ="已开启",_wzOff ="未开启"},
            new DigitalModel(){_wzOn ="已到位",_wzOff ="未到位"},
            new DigitalModel(){_wzOn ="已到位",_wzOff ="未到位"},
            new DigitalModel(){_wzOn ="已到位",_wzOff ="未到位"},

            new DigitalModel(){_wzOn ="",_wzOff =""},
            new DigitalModel(){_wzOn ="",_wzOff =""},
            new DigitalModel(){_wzOn ="",_wzOff =""},
            new DigitalModel(){_wzOn ="",_wzOff =""},
            new DigitalModel(){_wzOn ="",_wzOff =""},
            new DigitalModel(){_wzOn ="",_wzOff =""},
            new DigitalModel(){_wzOn ="",_wzOff =""},
            new DigitalModel(){_wzOn ="",_wzOff =""},
            new DigitalModel(){_wzOn ="",_wzOff =""},

            new DigitalModel(){_wzOn ="正常",_wzOff ="异常"},
            new DigitalModel(){_wzOn ="",_wzOff =""}
        };
        /// <summary>
        /// 数字量输入参数列表。蝶阀1主、2粗、3细、4加粗
        /// </summary>
        /// <remarks>M100主管蝶阀1开状态，M101粗管蝶阀2开状态，M102细管蝶阀3开状态，M103加粗管蝶阀4开状态，M104差压阀开状态，
        /// M105KM1吸合状态，M106水泵运行状态，M107气泵运行状态，M108风机变频启动状态，M109水泵变频启动状态，
        /// M110液压站运行状态，M111液压总阀开状态，M112X轴向右状态，M113X轴向左状态，M114Y轴向前状态，M115Y轴向后状态，M116Z轴向上状态，M117Z轴向下状态，M118X定点到位，M119Y定点到位，M120Z定点到位，
        /// M130相序正常状态，M131通讯闪烁状态</remarks>
        public ObservableCollection<DigitalModel> DIList
        {
            get { return _diList; }
            set
            {
                _diList = value;
                RaisePropertyChanged(() => DIList);
            }
        }

        /// <summary>
        /// 数字量输出参数列表
        /// </summary>
        /// <remarks>M400主管蝶阀1程控指令,M401粗管蝶阀2程控指令,M402细管蝶阀3程控指令,M403加粗管蝶阀4程控指令,M404差压程控指令
        /// M405KM1程控指令, M406水泵程控指令,M407气泵程控指令, M408风机变频程控指令,M409水泵变频程控指令
        /// M410液压站程控指令, M411液压总阀程控指令,M412X轴向右程控指令, M413X轴向左程控指令,M414Y轴向前程控指令, M415Y轴向后程控指令,M416Z轴向上程控指令, M417Z轴向下程控指令</remarks>
        private ObservableCollection<DigitalModel> _doList = new ObservableCollection<DigitalModel>()
        {
            new DigitalModel(){_wzOn ="关闭蝶阀1",_wzOff ="开启蝶阀1"},  
            new DigitalModel(){_wzOn ="关闭蝶阀2",_wzOff ="开启蝶阀2"},
            new DigitalModel(){_wzOn ="关闭蝶阀3",_wzOff ="开启蝶阀3"},  
            new DigitalModel(){_wzOn ="关闭蝶阀4",_wzOff ="开启蝶阀4"},
            new DigitalModel(){_wzOn ="关闭差压阀",_wzOff ="打开差压阀"},
            new DigitalModel(){_wzOn ="分断KM1",_wzOff ="接通KM1"},
            new DigitalModel(){_wzOn ="关闭水泵",_wzOff ="启动水泵"},
            new DigitalModel(){_wzOn ="关闭气泵",_wzOff ="启动气泵"},
            new DigitalModel(){_wzOn ="关风机变频",_wzOff ="开风机变频"},
            new DigitalModel(){_wzOn ="关水泵变频",_wzOff ="开水泵变频"},
            new DigitalModel(){_wzOn ="关闭液压站",_wzOff ="启动液压站"},
            new DigitalModel(){_wzOn ="关液压总阀",_wzOff ="开液压总阀"},
            new DigitalModel(){_wzOn ="停止向右",_wzOff ="X轴向右"},
            new DigitalModel(){_wzOn ="停止向左",_wzOff ="X轴向左"}, 
            new DigitalModel(){_wzOn ="停止向前",_wzOff ="Y轴向前"},
            new DigitalModel(){_wzOn ="停止向后",_wzOff ="Y轴向后"}, 
            new DigitalModel(){_wzOn ="停止向上",_wzOff ="Z轴向上"},
            new DigitalModel(){_wzOn ="停止向下",_wzOff ="Z轴向下"}
        };
        /// <summary>
        /// 数字量输出参数列表
        /// </summary>
        /// <remarks>M400主管蝶阀1程控指令,M401粗管蝶阀2程控指令,M402细管蝶阀3程控指令,M403加粗管蝶阀4程控指令,M404差压程控指令
        /// M405KM1程控指令, M406水泵程控指令,M407气泵程控指令, M408风机变频程控指令,M409水泵变频程控指令
        /// M410液压站程控指令, M411液压总阀程控指令,M412X轴向右程控指令, M413X轴向左程控指令,M414Y轴向前程控指令, M415Y轴向后程控指令,M416Z轴向上程控指令, M417Z轴向下程控指令</remarks>
        public ObservableCollection<DigitalModel> DOList
        {
            get { return _doList; }
            set
            {
                _doList = value;
                RaisePropertyChanged(() => DOList);
            }
        }

        #endregion
    }
}