using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using SqlSugar;

namespace MQDFJ_MB.Model.Exp_MB
{
    /// <summary>
    /// 动态风压下水密测试——校准试验
    /// </summary>
    [Serializable]
    [SugarTable("B01_SM_DTFY_Cali", IsDisabledDelete = true)]
    public class SM_DTFY_Cali : ObservableObject
    {
        /// <summary>
        /// 试验编号
        /// </summary>
        private string _testNO = "None";
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
        /// 当前压力测试点
        /// </summary>
        private SM_DTFY_Cali_PressPoint _pointDQ = new SM_DTFY_Cali_PressPoint();
        /// <summary>
        /// 当前压力测试点
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public SM_DTFY_Cali_PressPoint PointDQ
        {
            get { return _pointDQ; }
            set
            {
                _pointDQ = value;
                RaisePropertyChanged(() => PointDQ);
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
                RaisePropertyChanged(() => CommpletedStr);
            }
        }
        [SugarColumn(IsIgnore =true)]
        public string CommpletedStr
        {
            get { return _commpleted?"√":"×"; }
        }


        /// <summary>
        /// 可用标志（所有点都完成，切风速频率大于零）
        /// </summary>
        private bool _usefull = false;
        /// <summary>
        /// 可用标志（所有点都完成，切风速频率大于零）
        /// </summary>
        [SugarColumn(ColumnName = "Usefull")]
        public bool Usefull
        {
            get { return _usefull; }
            set
            {
                _usefull = value;
                RaisePropertyChanged(() => Usefull);
                RaisePropertyChanged(() => UsefullStr);
            }
        }
        [SugarColumn(IsIgnore = true)]
        public string UsefullStr
        {
            get { return _usefull ? "√" : "×"; }
        }


        /// <summary>
        /// 在用标志
        /// </summary>
        private bool _flag_Using = false;
        /// <summary>
        /// 在用标志
        /// </summary>
        [SugarColumn(ColumnName = "Flag_Using")]
        public bool Flag_Using
        {
            get { return _flag_Using; }
            set
            {
                _flag_Using = value;
                RaisePropertyChanged(() => Flag_Using);
                RaisePropertyChanged(() => UsingStr);
            }
        }
        [SugarColumn(IsIgnore = true)]
        public string UsingStr
        {
            get { return _flag_Using ? "√" : "×"; }
        }


        /// <summary>
        /// 数据复位
        /// </summary>
        public void Reset()
        {
            TestTime = DateTime.MinValue;
            foreach (var point in PointList)
            {
                point.Reset();
            }
            PointDQ = PointList[0];
            Commpleted = false;
            Usefull = false;
            Flag_Using = false;
        }

    }
}
