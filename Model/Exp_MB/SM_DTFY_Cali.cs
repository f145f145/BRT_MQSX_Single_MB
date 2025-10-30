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
    /// 动态风压下水密测试——校准试验
    /// </summary>
    public class SM_DTFY_Cali : ObservableObject
    {
        /// <summary>
        /// 试验编号
        /// </summary>
        private string _testNO = "DefaultExp";
        /// <summary>
        ///  试验编号
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
        /// 测试时间
        /// </summary>
        private DateTime _testTime = DateTime.MinValue;
        /// <summary>
        /// 测试时间
        /// </summary>
        [SugarColumn(ColumnName = "TestTime")]
        public DateTime TestTime
        {
            get { return _testTime; }
            set
            {
                _testTime = value;
                RaisePropertyChanged(() => TestTime);
            }
        }


        /// <summary>
        /// 压力测试点列表
        /// </summary>
        private ObservableCollection<SM_DTFY_Cali_PressPoint> _pointList = new ObservableCollection<SM_DTFY_Cali_PressPoint>()
        {
            new SM_DTFY_Cali_PressPoint(){ PointNo =0,PressSet=300},
            new SM_DTFY_Cali_PressPoint(){ PointNo =1,PressSet=380},
            new SM_DTFY_Cali_PressPoint(){ PointNo =2,PressSet=480},
            new SM_DTFY_Cali_PressPoint(){ PointNo =3,PressSet=580},
            new SM_DTFY_Cali_PressPoint(){ PointNo =4,PressSet=720}
        };
        /// <summary>
        /// 压力测试点列表
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public ObservableCollection<SM_DTFY_Cali_PressPoint> PointList
        {
            get { return _pointList; }
            set
            {
                _pointList = value;
                RaisePropertyChanged(() => PointList);
            }
        }


        /// <summary>
        /// 测试距离，米，风机螺旋桨距离测试点距离
        /// </summary>
        private double _distanceSet = 3.0;
        /// <summary>
        /// 测试距离，米，风机螺旋桨距离测试点距离
        ///</summary>
        [SugarColumn(ColumnName = "DistanceSet")]
        public double DistanceSet
        {
            get
            {
                return _distanceSet;
            }
            set
            {
                _distanceSet = value;
                RaisePropertyChanged(() => DistanceSet);
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
        /// 测试总时长设定值，秒
        /// </summary>
        private int _timeLongSet = 60;
        /// <summary>
        /// 测试总时长设定值，秒
        ///</summary>
        [SugarColumn(ColumnName = "TimeLongSet")]
        public int TimeLongSet
        {
            get
            {
                return _timeLongSet;
            }
            set
            {
                _timeLongSet = value;
                RaisePropertyChanged(() => TimeLongSet);
            }
        }


        /// <summary>
        /// 记录间隔时长，秒
        /// </summary>
        private int _timeInterval = 5;
        /// <summary>
        /// 记录间隔时长，秒
        ///</summary>
        [SugarColumn(ColumnName = "TimeInterval")]
        public int TimeInterval
        {
            get
            {
                return _timeInterval;
            }
            set
            {
                _timeInterval = value;
                RaisePropertyChanged(() => TimeInterval);
            }
        }


        /// <summary>
        /// 当前压力测试点
        /// </summary>
        private SM_DTFY_Cali_PressPoint _pointDQ = new SM_DTFY_Cali_PressPoint();
        /// <summary>
        /// 当前压力测试点
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public SM_DTFY_Cali_PressPoint PpointDQ
        {
            get { return _pointDQ; }
            set
            {
                _pointDQ = value;
                RaisePropertyChanged(() => PpointDQ);
            }
        }
    }
}
