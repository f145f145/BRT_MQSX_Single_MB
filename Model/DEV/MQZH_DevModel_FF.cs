/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 描述：
 * 调压风阀相关
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2024-01-03 15:54:00		郝正强			V1.0.0.0        
 ************************************************************************************/

using GalaSoft.MvvmLight;
using MQZHWL.BLL;
using System;
using System.Collections.ObjectModel;

namespace MQZHWL.Model.DEV
{
    public partial class MQZH_DevModel_Main : ObservableObject
    {
        /// <summary>
        /// 波动风阀
        /// </summary>
        private Valve _valve = new Valve();
        /// <summary>
        /// 波动风阀
        /// </summary>
        public Valve Valve
        {
            get { return _valve; }
            set
            {
                _valve = value;
                RaisePropertyChanged(() => _valve);
            }
        }
    }


    /// <summary>
    /// 波动风阀类
    /// </summary>
    public class Valve : ObservableObject
    {
        public Valve()
        {
            for (int i = 0; i < DIList.Count; i++)
            {
                if (DIList[i].ChannelSerialNO > Mod_XNDI.Channels.Count)
                    DIList[i].ChannelSerialNO = Mod_XNDI.Channels.Count;
                DIList[i].DigitalChannel = Mod_XNDI.Channels[i];
            }
        }

        #region 反馈参数

        #region 数字量模块

        /// <summary>
        /// 虚拟DI模块（内部关联转换，M100-M116）
        /// </summary>
        private DigitalModuleModel _mod_XNDI = new DigitalModuleModel()
        {
            ModuleNO = "XNDI",
            ModuleName = "虚拟DI模块",
            Channels = new ObservableCollection<DigitalChannelModel>()
            {
                new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(),
                new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(),
                new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel(),
                new DigitalChannelModel(), new DigitalChannelModel(), new DigitalChannelModel()
            }
        };
        /// <summary>
        /// 虚拟DI模块（内部关联转换，M300-M335）
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

        #endregion


        #region 数字量参数

        /// <summary>
        /// 数字量输入参数
        /// </summary>
        /// <remarks>M100阀门故障状态，M101伺服报警状态，M102阀门正压位置，M103阀门负压位置，
        /// M104自检完成状态，M105正压模式完成状态，M106负压模式完成状态，M107正压波动准备完成状态，
        /// M108负压波动准备完成状态，M109正压波动完成状态，M110负压波动完成状态，M111正压降低，
        /// M112正压升高，M113负压降低，M114负压升高
        /// </remarks>
        private ObservableCollection<DigitalModel> _diList = new ObservableCollection<DigitalModel>()
        {
            new DigitalModel(){_wzOn ="有故障",_wzOff ="无故障"},
            new DigitalModel(){_wzOn ="报警中",_wzOff ="无报警"},
            new DigitalModel(){_wzOn ="到位",_wzOff ="未到位"},
            new DigitalModel(){_wzOn ="到位",_wzOff ="未到位"},

            new DigitalModel(){_wzOn ="完成",_wzOff ="未完成"},
            new DigitalModel(){_wzOn ="已完成",_wzOff ="未完成"},
            new DigitalModel(){_wzOn ="已完成",_wzOff ="未完成"},
            new DigitalModel(){_wzOn ="已完成",_wzOff ="未完成"},

            new DigitalModel(){_wzOn ="已完成",_wzOff ="未完成"},
            new DigitalModel(){_wzOn ="已完成",_wzOff ="未完成"},
            new DigitalModel(){_wzOn ="已完成",_wzOff ="未完成"},
            new DigitalModel(){_wzOn ="降低中",_wzOff ="无动作"},

            new DigitalModel(){_wzOn ="升高中",_wzOff ="无动作"},
            new DigitalModel(){_wzOn ="降低中",_wzOff ="无动作"},
            new DigitalModel(){_wzOn ="升高中",_wzOff ="无动作"}
        };
        /// <summary>
        /// 数字量输入参数
        /// </summary>
        /// <remarks>M100阀门故障状态，M101伺服报警状态，M102阀门正压位置，M103阀门负压位置，
        /// M104自检完成状态，M105正压模式完成状态，M106负压模式完成状态，M107正压波动准备完成状态，
        /// M108负压波动准备完成状态，M109正压波动完成状态，M110负压波动完成状态，M111正压降低，
        /// M112正压升高，M113负压降低，M114负压升高
        /// </remarks>
        public ObservableCollection<DigitalModel> DIList
        {
            get { return _diList; }
            set
            {
                _diList = value;
                RaisePropertyChanged(() => DIList);
            }
        }

        #endregion

        /// <summary>
        /// 风阀模式
        /// </summary>
        private int _ffMode = 0;
        /// <summary>
        /// 风阀模式
        /// </summary>
        public int FFMode
        {
            get { return _ffMode; }
            set
            {
                _ffMode = value;
                RaisePropertyChanged(() => FFMode);
            }
        }

        /// <summary>
        /// 正压已波动次数
        /// </summary>
        private int _timesZYWave = 0;
        /// <summary>
        /// 正压已波动次数
        /// </summary>
        public int TimesZYWave
        {
            get { return _timesZYWave; }
            set
            {
                _timesZYWave = value;
                RaisePropertyChanged(() => TimesZYWave);
            }
        }

        /// <summary>
        /// 负压已波动次数
        /// </summary>
        private int _timesFYWave = 0;
        /// <summary>
        /// 负压已波动次数
        /// </summary>
        public int TimesFYWave
        {
            get { return _timesFYWave; }
            set
            {
                _timesFYWave = value;
                RaisePropertyChanged(() => TimesFYWave);
            }
        }

        /// <summary>
        /// 故障类型
        /// </summary>
        private int _errType = 0;
        /// <summary>
        /// 故障类型
        /// </summary>
        public int ErrType
        {
            get { return _errType; }
            set
            {
                _errType = value;
                RaisePropertyChanged(() => ErrType);
            }
        }

        /// <summary>
        /// 自检状态
        /// </summary>
        private int _selfTestStatus = 0;
        /// <summary>
        /// 自检状态
        /// </summary>
        public int SelfTestStatus
        {
            get { return _selfTestStatus; }
            set
            {
                _selfTestStatus = value;
                RaisePropertyChanged(() => SelfTestStatus);
            }
        }

        /// <summary>
        /// 手动状态
        /// </summary>
        private int _manualStatus = 0;
        /// <summary>
        /// 手动状态
        /// </summary>
        public int ManualStatus
        {
            get { return _manualStatus; }
            set
            {
                _manualStatus = value;
                RaisePropertyChanged(() => ManualStatus);
            }
        }

        /// <summary>
        /// 正压低压准备状态
        /// </summary>
        private int _pLowPrepareStatus = 0;
        /// <summary>
        /// 正压低压准备状态
        /// </summary>
        public int PLowPrepareStatus
        {
            get { return _pLowPrepareStatus; }
            set
            {
                _pLowPrepareStatus = value;
                RaisePropertyChanged(() => PLowPrepareStatus);
            }
        }

        /// <summary>
        /// 负压低压准备状态
        /// </summary>
        private int _nLowPrepareStatus = 0;
        /// <summary>
        /// 负压低压准备状态
        /// </summary>
        public int NLowPrepareStatus
        {
            get { return _nLowPrepareStatus; }
            set
            {
                _nLowPrepareStatus = value;
                RaisePropertyChanged(() => NLowPrepareStatus);
            }
        }

        /// <summary>
        /// 正压波动状态
        /// </summary>
        private int _pWaveStatus = 0;
        /// <summary>
        /// 正压波动状态
        /// </summary>
        public int PWaveStatus
        {
            get { return _pWaveStatus; }
            set
            {
                _pWaveStatus = value;
                RaisePropertyChanged(() => PWaveStatus);
            }
        }

        /// <summary>
        /// 负压波动状态
        /// </summary>
        private int _nWaveStatus = 0;
        /// <summary>
        /// 负压波动状态
        /// </summary>
        public int NWaveStatus
        {
            get { return _nWaveStatus; }
            set
            {
                _nWaveStatus = value;
                RaisePropertyChanged(() => NWaveStatus);
            }
        }

        /// <summary>
        /// 当前动作类型
        /// </summary>
        private int _actType = 0;
        /// <summary>
        /// 当前动作类型
        /// </summary>
        public int ActType
        {
            get { return _actType; }
            set
            {
                _actType = value;
                RaisePropertyChanged(() => ActType);
            }
        }

        #endregion

        #region 数据解析

        /// <summary>
        /// 换向阀状态解析
        /// </summary>
        /// <param name="msg"></param>
        public void StatusUpdate(ushort[] msg)
        {
            try
            {
                ushort[] inArry;
                inArry = (ushort[])msg.Clone();
                if (inArry.Length >= 13)
                {
                    bool[] boolValues1;
                    bool[] boolValues2;
                    bool[] boolValuesAll = new bool[32];

                    //解析状态---------------------------
                    //ushort解析为bool
                    boolValues1 = MQZH_ValueCvtBLL.GetBoolsFromUshort(inArry, 11, 1);
                    boolValues2 = MQZH_ValueCvtBLL.GetBoolsFromUshort(inArry, 12, 1);
                    Array.Copy(boolValues1, 0, boolValuesAll, 0, boolValues1.Length);
                    Array.Copy(boolValues2, 0, boolValuesAll, boolValues1.Length, boolValues2.Length);

                    //更新换向阀的DI通道状态
                    for (int i = 0; i < Mod_XNDI.Channels.Count; i++)
                    {
                        Mod_XNDI.Channels[i].IsOn = boolValuesAll[i];
                    }
                    //更新装置DI参数状态
                    for (int i = 0; i < DIList.Count; i++)
                    {
                        DIList[i].GetDIStatus();
                    }

                    FFMode = Convert.ToInt16(msg[0]);
                    TimesZYWave = Convert.ToInt16(msg[1]);
                    TimesFYWave = Convert.ToInt16(msg[2]);
                    ErrType = Convert.ToInt16(msg[3]);
                    SelfTestStatus = Convert.ToInt16(msg[4]);
                    ManualStatus = Convert.ToInt16(msg[5]);
                    PLowPrepareStatus = Convert.ToInt16(msg[6]);
                    NLowPrepareStatus = Convert.ToInt16(msg[7]);
                    PWaveStatus = Convert.ToInt16(msg[8]);
                    NWaveStatus = Convert.ToInt16(msg[9]);
                    ActType = Convert.ToInt16(msg[10]);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion
    }
}