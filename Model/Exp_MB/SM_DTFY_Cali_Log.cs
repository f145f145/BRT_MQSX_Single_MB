using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using SqlSugar;

namespace MQDFJ_MB.Model.Exp_MB
{
    /// <summary>
    /// 动态风压下水密测试——校准试验数据记录
    /// </summary>
    [Serializable]
    [SugarTable("B03_SM_DTFY_Cali_Log", IsDisabledDelete = true)]
    public class SM_DTFY_Cali_Log : ObservableObject
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
        /// 风速实测值左上
        /// </summary>
        private double _speedUL = 0;
        /// <summary>
        ///  风速实测值左上
        ///</summary>
        [SugarColumn(ColumnName = "SpeedUL")]
        public double SpeedUL
        {
            get
            {
                return _speedUL;
            }
            set
            {
                _speedUL = value;
                RaisePropertyChanged(() => SpeedUL);
            }
        }


        /// <summary>
        /// 风速实测值右上
        /// </summary>
        private double _speedUR = 0;
        /// <summary>
        ///  风速实测值右上
        ///</summary>
        [SugarColumn(ColumnName = "SpeedUR")]
        public double SpeedUR
        {
            get
            {
                return _speedUR;
            }
            set
            {
                _speedUR = value;
                RaisePropertyChanged(() => SpeedUR);
            }
        }


        /// <summary>
        /// 风速实测值右下
        /// </summary>
        private double _speedDR = 0;
        /// <summary>
        ///  风速实测值右下
        ///</summary>
        [SugarColumn(ColumnName = "SpeedDR")]
        public double SpeedDR
        {
            get
            {
                return _speedDR;
            }
            set
            {
                _speedDR = value;
                RaisePropertyChanged(() => SpeedDR);
            }
        }


        /// <summary>
        /// 风速实测值左下
        /// </summary>
        private double _speedDL = 0;
        /// <summary>
        ///  风速实测值左下
        ///</summary>
        [SugarColumn(ColumnName = "SpeedUR")]
        public double SpeedDL
        {
            get
            {
                return _speedDL;
            }
            set
            {
                _speedDL = value;
                RaisePropertyChanged(() => SpeedDL);
            }
        }


        /// <summary>
        /// 记录时间
        /// </summary>
        private DateTime _recTime = DateTime.MinValue;
        /// <summary>
        /// 结束时间
        /// </summary>
        [SugarColumn(ColumnName = "RecTime")]
        public DateTime RecTime
        {
            get { return _recTime; }
            set
            {
                _recTime = value;
                RaisePropertyChanged(() => RecTime);
            }
        }
    }
}