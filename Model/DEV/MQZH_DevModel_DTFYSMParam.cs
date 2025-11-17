using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using SqlSugar;

namespace MQDFJ_MB.Model.DEV
{
    /// <summary>
    /// 动态风压下水密检测
    /// </summary>
    [Serializable]
    [SugarTable("E01MQZH_DevModel_DTFYSMParam", IsDisabledDelete = true)]
    public class MQZH_DevModel_DTFYSMParam:ObservableObject
    {
        /// <summary>
        /// 参数组别
        /// </summary>
        private string _group = "Using";
        /// <summary>
        /// 参数组别
        /// </summary>
        [SugarColumn(ColumnName = "Group", ColumnDataType = "varchar(50)", IsPrimaryKey = true)]
        public string Group
        {
            get { return _group; }
            set
            {
                _group = value;
                RaisePropertyChanged(() => Group);
            }
        }


        /// <summary>
        /// 有动态风压
        /// </summary>
        private bool _isWithDTFY = true;
        /// <summary>
        /// 有动态风压
        ///</summary>
        [SugarColumn(ColumnName = "IsWithDTFY")]
        public bool IsWithDTFY
        {
            get
            {
                return _isWithDTFY;
            }
            set
            {
                _isWithDTFY = value;
                RaisePropertyChanged(() => IsWithDTFY);
            }
        }


        /// <summary>
        /// 频率加减速度，HZ/s
        /// </summary>
        private double _accF = 1.0;
        /// <summary>
        /// 数据记录时长设定值，秒
        ///</summary>
        [SugarColumn(ColumnName = "AccF")]
        public double AccF
        {
            get
            {
                return _accF;
            }
            set
            {
                _accF = value;
                RaisePropertyChanged(() => AccF);
            }
        }


        /// <summary>
        /// 校准测试时长，秒
        /// </summary>
        private int _timeLongCali = 60;
        /// <summary>
        /// 校准测试时长，秒
        ///</summary>
        [SugarColumn(ColumnName = "TimeLongCali")]
        public int TimeLongCali
        {
            get
            {
                return _timeLongCali;
            }
            set
            {
                _timeLongCali = value;
                RaisePropertyChanged(() => TimeLongCali);
            }
        }


        /// <summary>
        /// 动态风压测试时长，秒
        /// </summary>
        private int _timeLongTest = 900;
        /// <summary>
        /// 动态风压测试时长，秒
        ///</summary>
        [SugarColumn(ColumnName = "TimeLongTest")]
        public int TimeLongTest
        {
            get
            {
                return _timeLongTest;
            }
            set
            {
                _timeLongTest = value;
                RaisePropertyChanged(() => TimeLongTest);
            }
        }


        /// <summary>
        /// 等待稳定时长，秒
        /// </summary>
        private int _timeWaitForStd = 60;
        /// <summary>
        /// 等待稳定时长，秒
        ///</summary>
        [SugarColumn(ColumnName = "TimeWaitForStd")]
        public int TimeWaitForStd
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
        /// 当前校准试验编号
        /// </summary>
        private string _testNoDQDTFYCali = "DefaultExp1";
        /// <summary>
        /// 当前校准试验编号
        ///</summary>
        [SugarColumn(ColumnName = "TestNoDQDTFYCali")]
        public string TestNoDQDTFYCali
        {
            get
            {
                return _testNoDQDTFYCali;
            }
            set
            {
                _testNoDQDTFYCali = value;
                RaisePropertyChanged(() => TestNoDQDTFYCali);
            }
        }


        /// <summary>
        /// 在用校准试验编号
        /// </summary>
        private string _testNoUsingDQDTFYCali = "DefaultExp1";
        /// <summary>
        /// 在用校准试验编号
        ///</summary>
        [SugarColumn(ColumnName = "TestNoUsingDQDTFYCali")]
        public string TestNoUsingDQDTFYCali
        {
            get
            {
                return _testNoUsingDQDTFYCali;
            }
            set
            {
                _testNoUsingDQDTFYCali = value;
                RaisePropertyChanged(() => TestNoUsingDQDTFYCali);
            }
        }


        /// <summary>
        /// 当前试验编号
        /// </summary>
        private string _testNoDQDTFY = "DefaultExp1";
        /// <summary>
        /// 当前试验编号
        ///</summary>
        [SugarColumn(ColumnName = "TestNoDQDTFY")]
        public string TestNoDQDTFY
        {
            get
            {
                return _testNoDQDTFY;
            }
            set
            {
                _testNoDQDTFY= value;
                RaisePropertyChanged(() => TestNoDQDTFY);
            }
        }


        /// <summary>
        /// 在用试验编号
        /// </summary>
        private string _testNoUsingDQDTFY = "DefaultExp1";
        /// <summary>
        /// 在用试验编号
        ///</summary>
        [SugarColumn(ColumnName = "TestNoUsingDQDTFY")]
        public string TestNoUsingDQDTFY
        {
            get
            {
                return _testNoUsingDQDTFY;
            }
            set
            {
                _testNoUsingDQDTFY = value;
                RaisePropertyChanged(() => TestNoUsingDQDTFY);
                RaisePropertyChanged(() => TestNoUsingDQDTFY);
            }
        }
    }
}
