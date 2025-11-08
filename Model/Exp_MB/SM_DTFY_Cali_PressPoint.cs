using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using SqlSugar;

namespace MQDFJ_MB.Model.Exp_MB
{
    /// <summary>
    /// 动态风压下水密测试——校准试验_压力点
    /// </summary>
    [Serializable]
    [SugarTable("B02_SM_DTFY_Cali_PressPoint", IsDisabledDelete = true)]
    public class SM_DTFY_Cali_PressPoint : ObservableObject
    {
        /// <summary>
        /// 自增编号
        /// </summary>
        private int _id = 1;
        /// <summary>
        /// 自增编号
        /// </summary>
        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged(() => ID);
            }
        }


        /// <summary>
        /// 校准试验编号
        /// </summary>
        private string _testNO = "DefaultExp";
        /// <summary>
        ///  校准试验编号
        ///</summary>
        [SugarColumn(ColumnName = "TestNO", IsPrimaryKey = true)]
        public string TestNO
        {
            get
            {
                return _testNO;
            }
            set
            {
                _testNO = value;
                RaisePropertyChanged(() => TestNO);
            }
        }


        /// <summary>
        /// 压力点序号
        /// </summary>
        private int _pointNo = 0;
        /// <summary>
        ///  压力点序号
        ///</summary>
        [SugarColumn(ColumnName = "PointNo")]
        public int PointNo
        {
            get
            {
                return _pointNo;
            }
            set
            {
                _pointNo = value;
                RaisePropertyChanged(() => PointNo);
            }
        }


        /// <summary>
        /// 设定压力值,Pa
        /// </summary>
        private double _pressSet = 0;
        /// <summary>
        /// 设定压力值,Pa
        ///</summary>
        [SugarColumn(ColumnName = "_pressSet")]
        public double PressSet
        {
            get
            {
                return _pressSet;
            }
            set
            {
                _pressSet = Math.Abs(value);
                RaisePropertyChanged(() => PressSet);
                RaisePropertyChanged(() => SpeedSet);
                RaisePropertyChanged(() => IsFit);
            }
        }
        /// <summary>
        /// 风速设定值（m/s）
        /// </summary>
        [SugarColumn(IsIgnore =true)]
        public double SpeedSet
        {
            get
            {
                return Math.Sqrt(_pressSet / 0.613);
            }
        }


        /// <summary>
        /// 风速实测值（最大值）右上
        /// </summary>
        private double _speedMaxUR = 0;
        /// <summary>
        /// 风速实测值（最大值）右上
        ///</summary>
        [SugarColumn(ColumnName = "SpeedMaxUR")]
        public double SpeedMaxUR
        {
            get
            {
                return _speedMaxUR;
            }
            set
            {
                _speedMaxUR = value;
                RaisePropertyChanged(() => SpeedMaxUR);
                RaisePropertyChanged(() => SpeedAvg);
                RaisePropertyChanged(() => IsFit);
            }
        }


        /// <summary>
        /// 风速实测值（最大值）左上
        /// </summary>
        private double _speedMaxUL = 0;
        /// <summary>
        /// 风速实测值（最大值）左上
        ///</summary>
        [SugarColumn(ColumnName = "SpeedMaxUL")]
        public double SpeedMaxUL
        {
            get
            {
                return _speedMaxUL;
            }
            set
            {
                _speedMaxUL = value;
                RaisePropertyChanged(() => SpeedMaxUL);
                RaisePropertyChanged(() => SpeedAvg);
                RaisePropertyChanged(() => IsFit);
            }
        }


        /// <summary>
        /// 风速实测值（最大值）右下
        /// </summary>
        private double _speedMaxDR = 0;
        /// <summary>
        /// 风速实测值（最大值）右下
        ///</summary>
        [SugarColumn(ColumnName = "SpeedMaxDR")]
        public double SpeedMaxDR
        {
            get
            {
                return _speedMaxDR;
            }
            set
            {
                _speedMaxDR = value;
                RaisePropertyChanged(() => SpeedMaxDR);
                RaisePropertyChanged(() => SpeedAvg);
                RaisePropertyChanged(() => IsFit);
            }
        }


        /// <summary>
        /// 风速实测值（最大值）左下
        /// </summary>
        private double _speedMaxDL = 0;
        /// <summary>
        /// 风速实测值（最大值）左下
        ///</summary>
        [SugarColumn(ColumnName = "SpeedMaxDL")]
        public double SpeedMaxDL
        {
            get
            {
                return _speedMaxDL;
            }
            set
            {
                _speedMaxDL = value;
                RaisePropertyChanged(() => SpeedMaxDL);
                RaisePropertyChanged(() => SpeedAvg);
                RaisePropertyChanged(() => IsFit);
                RaisePropertyChanged(() => IsFitStr);
            }
        }


        /// <summary>
        /// 风速实测值（平均值）
        ///</summary>
        [SugarColumn(IsIgnore =true)]
        public double SpeedAvg
        {
            get
            {
                return (SpeedMaxUL+ SpeedMaxUR+ SpeedMaxDR+ SpeedMaxDL)/4;
            }
        }


        /// <summary>
        /// 风速合格（是否满足±1.1m/s偏差要求）
        ///</summary>
        [SugarColumn(IsIgnore = true)]
        public bool IsFit
        {
            get
            {
                return ((SpeedAvg !=0)&&(Math.Abs(SpeedAvg-SpeedSet)<=1.1))?true:false ;
            }
        }
        [SugarColumn(IsIgnore = true)]
        public string IsFitStr
        {
            get { return _commpleted ? "√" : "×"; }
        }


        /// <summary>
        /// 风机频率控制测定值，HZ
        /// </summary>
        private double _frequencyCtl = 0;
        /// <summary>
        /// 风机频率控制测定值，HZ
        ///</summary>
        [SugarColumn(ColumnName = "FrequencyCtl")]
        public double FrequencyCtl
        {
            get
            {
                return _frequencyCtl;
            }
            set
            {
                _frequencyCtl = value;
                RaisePropertyChanged(() => FrequencyCtl);
            }
        }


        /// <summary>
        /// 测试起始频率值，HZ
        /// </summary>
        private double _frequencyStart = 0;
        /// <summary>
        /// 测试起始频率值，HZ
        ///</summary>
        [SugarColumn(ColumnName = "FrequencyStart")]
        public double FrequencyStart
        {
            get
            {
                return _frequencyStart;
            }
            set
            {
                _frequencyStart = value;
                RaisePropertyChanged(() => FrequencyStart);
            }
        }


        /// <summary>
        /// 频率调整初始步长，HZ。
        /// </summary>
        private double _frequencyChangeStep = 0;
        /// <summary>
        /// 频率调整初始步长，HZ
        ///</summary>
        [SugarColumn(ColumnName = "FrequencyChangeStep")]
        public double FrequencyChangeStep
        {
            get
            {
                return _frequencyChangeStep;
            }
            set
            {
                _frequencyChangeStep = value;
                RaisePropertyChanged(() => FrequencyChangeStep);
            }
        }


        /// <summary>
        /// 记录列表
        /// </summary>
        private ObservableCollection<SM_DTFY_Cali_Log> _logList = new ObservableCollection<SM_DTFY_Cali_Log>();
        /// <summary>
        /// 记录列表
        /// </summary>
        [SugarColumn(IsIgnore =true)]
        public ObservableCollection<SM_DTFY_Cali_Log> LogList
        {
            get { return _logList; }
            set
            {
                _logList = value;
                RaisePropertyChanged(() => LogList);
            }
        }


        /// <summary>
        /// 测试开始时间
        /// </summary>
        private DateTime _testTimeStart = DateTime.MinValue;
        /// <summary>
        /// 测试开始时间
        /// </summary>
        [SugarColumn(ColumnName = "TestTimeStart")]
        public DateTime TestTimeStart
        {
            get { return _testTimeStart; }
            set
            {
                _testTimeStart = value;
                RaisePropertyChanged(() => TestTimeStart);
            }
        }


        /// <summary>
        /// 测试时间
        /// </summary>
        private Time _testTime = new Time();
        /// <summary>
        /// 测试时间
        ///</summary>
        [SugarColumn(IsIgnore = true)]
        public Time TestTime
        {
            get
            {
                return _testTime;
            }
            set
            {
                _testTime = value;
                RaisePropertyChanged(() => TestTime);
            }
        }


        /// <summary>
        /// 测试等待时间
        /// </summary>
        private Time _timeWaitForStd = new Time();
        /// <summary>
        /// 测试等待时间
        ///</summary>
        [SugarColumn(IsIgnore = true)]
        public Time TimeWaitForStd
        {
            get
            {
                return _timeWaitForStd;
            }
            set
            {
                _timeWaitForStd = value;
                RaisePropertyChanged(() => TimeWaitForStd);
            }
        }


        /// <summary>
        /// 测试记录时间
        /// </summary>
        private Time _timeRec = new Time();
        /// <summary>
        /// 测试记录时间
        ///</summary>
        [SugarColumn(IsIgnore = true)]
        public Time TimeRec
        {
            get
            {
                return _timeRec;
            }
            set
            {
                _timeRec = value;
                RaisePropertyChanged(() => TimeRec);
            }
        }


        /// <summary>
        /// 当前频率控制值，HZ。
        /// </summary>
        private double _frequencySetDq = 0;
        /// <summary>
        /// 当前频率控制值，HZ
        ///</summary>
        [SugarColumn(IsIgnore = true)]
        public double FrequencySetDq
        {
            get
            {
                return _frequencySetDq;
            }
            set
            {
                _frequencySetDq = value;
                RaisePropertyChanged(() => FrequencySetDq);
            }
        }
        

        /// <summary>
        /// 频率调整当前步长，HZ。
        /// </summary>
        private double _frequencyChangeStepDq = 0;
        /// <summary>
        /// 频率调整当前步长，HZ
        ///</summary>
        [SugarColumn(IsIgnore = true)]
        public double FrequencyChangeStepDq
        {
            get
            {
                return _frequencyChangeStepDq;
            }
            set
            {
                _frequencyChangeStepDq = value;
                RaisePropertyChanged(() => FrequencyChangeStepDq);
            }
        }


        /// <summary>
        /// 频率调整当前方向，1上升，-1下降，HZ。
        /// </summary>
        private int _frequencyChangeDir = 1;
        /// <summary>
        /// 频率调整当前步长，HZ
        ///</summary>
        [SugarColumn(IsIgnore = true)]
        public int FrequencyChangeDir
        {
            get
            {
                return _frequencyChangeDir;
            }
            set
            {
                _frequencyChangeDir = value;
                RaisePropertyChanged(() => FrequencyChangeDir);
            }
        }


        /// <summary>
        /// 完成标志
        /// </summary>
        private bool _commpleted = false;
        /// <summary>
        /// 完成标志
        /// </summary>
        [SugarColumn(ColumnName = "Commpleted")]
        public bool Commpleted
        {
            get { return _commpleted; }
            set
            {
                _commpleted = value;
                RaisePropertyChanged(() => Commpleted);
            }
        }
        [SugarColumn(IsIgnore = true)]
        public string CommpletedStr
        {
            get { return _commpleted ? "√" : "×"; }
        }


        /// <summary>
        /// 数据复位
        /// </summary>
        public void Reset()
        {
            SpeedMaxUR = 0;
            SpeedMaxUL = 0;
            SpeedMaxDR = 0;
            SpeedMaxDL = 0;
            LogList.Clear();
            TestTimeStart= DateTime.Now;
            TestTime.Reset();
            TimeWaitForStd.Reset();
            TimeRec.Reset();
            FrequencySetDq = 0;
            FrequencyChangeStepDq = FrequencyChangeStep;
            FrequencyChangeDir = 1;
            Commpleted = false;
        }
    }
}