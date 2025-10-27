/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQDFJ_MB.Model
 * 文件名：  AnalogModel
 * 版本号：  V1.0.0.0
 * 唯一标识：84e950e3-bfb1-473c-a256-019f30241632
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022-5-26 10:45:23
 * 描述：
 *
 * ==================================================================================
 * 修改标记
 * 修改时间				修改人			版本号			描述
 * 2022-5-26 10:45:23		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace MQDFJ_MB.Model.DEV
{
    public class AnalogModel : ObservableObject
    {
        #region 模拟量信号基本设定属性

        /// <summary>
        /// 信号编号
        /// </summary>
        private string _singalNO = "";
        /// <summary>
        /// 信号编号
        /// </summary>
        public string SingalNO
        {
            get { return _singalNO; }
            set
            {
                _singalNO = value;
                RaisePropertyChanged(() => SingalNO);
            }
        }

        /// <summary>
        /// 信号名称
        /// </summary>
        private string _singalName = "";
        /// <summary>
        /// 信号名称
        /// </summary>
        public string SingalName
        {
            get { return _singalName; }
            set
            {
                _singalName = value;
                RaisePropertyChanged(() => SingalName);
            }
        }

        /// <summary>
        /// 信号物理量单位
        /// </summary>
        private string _singalUnit = "";
        /// <summary>
        /// 信号物理量单位
        /// </summary>
        public string SingalUnit
        {
            get { return _singalUnit; }
            set
            {
                _singalUnit = value;
                RaisePropertyChanged(() => SingalUnit);
            }
        }

        /// <summary>
        /// 信号量程下限
        /// </summary>
        private double _singalLowerRange = 0;
        /// <summary>
        /// 信号量程下限
        /// </summary>
        public double SingalLowerRange
        {
            get { return _singalLowerRange; }
            set
            {
                _singalLowerRange = value;
                RaisePropertyChanged(() => SingalLowerRange);
            }
        }

        /// <summary>
        /// 信号量程上限
        /// </summary>
        private double _singalUpperRange = 0;
        /// <summary>
        /// 信号量程上限
        /// </summary>
        public double SingalUpperRange
        {
            get { return _singalUpperRange; }
            set
            {
                _singalUpperRange = value;
                RaisePropertyChanged(() => SingalUpperRange);
            }
        }

        /// <summary>
        /// 输入输出类型（true为输出）
        /// </summary>
        private bool _isOutType = false;
        /// <summary>
        /// 输入输出类型（true为输出）
        /// </summary>
        public bool IsOutType
        {
            get { return _isOutType; }
            set
            {
                _isOutType = value;
                RaisePropertyChanged(() => IsOutType);
            }
        }

        /// <summary>
        /// 接口类型（Voltage电压型，Current电流型，Digital数据型，Couple电偶型）
        /// </summary>
        private string _infType = "Voltage";
        /// <summary>
        /// 接口类型（Voltage电压型，Current电流型，Digital数据型，Couple电偶型）
        /// </summary>
        public string InfType
        {
            get { return _infType; }
            set
            {
                _infType = value;
                RaisePropertyChanged(() => InfType);
            }
        }

        /// <summary>
        /// 电信号单位
        /// </summary>
        private string _elecSigUnit = "mA";
        /// <summary>
        /// 电信号单位
        /// </summary>
        public string ElecSig_Unit
        {
            get { return _elecSigUnit; }
            set
            {
                _elecSigUnit = value;
                RaisePropertyChanged(() => ElecSig_Unit);
            }
        }

        /// <summary>
        /// 电信号下限
        /// </summary>
        private double _elecSigLowerRange = 0;
        /// <summary>
        /// 电信号下限
        /// </summary>
        public double ElecSigLowerRange
        {
            get { return _elecSigLowerRange; }
            set
            {
                _elecSigLowerRange = value;
                RaisePropertyChanged(() => ElecSigLowerRange);
            }
        }

        /// <summary>
        /// 电信号上限
        /// </summary>
        private double _elecSigUpperRange = 0;
        /// <summary>
        /// 电信号上限
        /// </summary>
        public double ElecSigUpperRange
        {
            get { return _elecSigUpperRange; }
            set
            {
                _elecSigUpperRange = value;
                RaisePropertyChanged(() => ElecSigUpperRange);
            }
        }

        /// <summary>
        /// 接入模块编号
        /// </summary>
        private string _modulNO = "04AD-1";
        /// <summary>
        /// 接入模块编号
        /// </summary>
        public string ModulNO
        {
            get { return _modulNO; }
            set
            {
                _modulNO = value;
                RaisePropertyChanged(() => ModulNO);
            }
        }

        /// <summary>
        /// 接入通道在模块中的编号（从1开始）
        /// </summary>
        private int _channelSerialNO = 1;
        /// <summary>
        /// 接入通道在模块中的编号（从1开始）
        /// </summary>
        public int ChannelSerialNO
        {
            get { return _channelSerialNO; }
            set
            {
                _channelSerialNO = value;
                RaisePropertyChanged(() => ChannelSerialNO);
            }
        }

        /// <summary>
        /// 换算比率（接入信号类型转换时用）
        /// </summary>
        private double _convRatio = 1;
        /// <summary>
        /// 换算比率（接入信号类型转换时用）
        /// </summary>
        public double ConvRatio
        {
            get { return _convRatio; }
            set
            {
                _convRatio = value;
                RaisePropertyChanged(() => ConvRatio);
            }
        }

        #endregion


        #region 调零、校正设定参数属性

        /// <summary>
        /// 零点校正值
        /// </summary>
        private double _zeroCalValue = 0;
        /// <summary>
        /// 零点校正值
        /// </summary>
        public double ZeroCalValue
        {
            get { return _zeroCalValue; }
            set
            {
                _zeroCalValue = value;
                RaisePropertyChanged(() => ZeroCalValue);
            }
        }

        /// <summary>
        /// 斜率修正值
        /// </summary>
        private double _kCalValue = 0;
        /// <summary>
        /// 斜率修正值
        /// </summary>
        public double KCalValue
        {
            get { return _kCalValue; }
            set
            {
                _kCalValue = value;
                RaisePropertyChanged(() => KCalValue);
            }
        }

        /// <summary>
        /// 标定点列表（含两端共11个）
        /// </summary>
        private ObservableCollection<CalPoint> _calPoints = new ObservableCollection<CalPoint>()
        {
            new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),
            new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint()
        };
        /// <summary>
        /// 标定点列表（共11个）
        /// </summary>
        public ObservableCollection<CalPoint> CalPoints
        {
            get { return _calPoints; }
            set
            {
                _calPoints = value;
                RaisePropertyChanged(() => CalPoints);
            }
        }

        /// <summary>
        /// 临时用标定点列表（共11个）
        /// </summary>
        private ObservableCollection<CalPoint> _calPointsTemp = new ObservableCollection<CalPoint>()
        {
            new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),
            new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint()
        };
        /// <summary>
        /// 临时用标定点列表（共11个）
        /// </summary>
        public ObservableCollection<CalPoint> CalPointsTemp
        {
            get { return _calPointsTemp; }
            set
            {
                _calPointsTemp = value;
                RaisePropertyChanged(() => CalPointsTemp);
            }
        }

        #endregion


        #region 推导属性

        /// <summary>
        /// 信号对应通道
        /// </summary>
        private AnalogChannelModel _analogChannel = new AnalogChannelModel();
        /// <summary>
        /// 通道列表
        /// </summary>
        public AnalogChannelModel AnalogChannel
        {
            get { return _analogChannel; }
            set
            {
                _analogChannel = value;
                RaisePropertyChanged(() => AnalogChannel);
            }
        }

        /// <summary>
        /// 信号电信号下限对应传输值
        /// </summary>
        private double _data_SingalMin;
        /// <summary>
        /// 信号电信号下限对应传输值
        /// </summary>
        private double Data_SingalMin
        {
            get { return _data_SingalMin; }
            set
            {
                _data_SingalMin = value;
                RaisePropertyChanged(() => Data_SingalMin);
            }
        }

        /// <summary>
        /// 传感器电信号上限对应传输值
        /// </summary>
        private double _data_SingalMax;
        /// <summary>
        /// 传感器电信号上限对应传输值
        /// </summary>
        private double Data_SingalMax
        {
            get { return _data_SingalMax; }
            set
            {
                _data_SingalMax = value;
                RaisePropertyChanged(() => Data_SingalMax);
            }
        }

        #endregion


        #region 实时数值

        /// <summary>
        /// 物理值（未调零、未标校）
        /// </summary>
        private double _valueNonCalNonZero;
        /// <summary>
        /// 物理值（未调零、未标校）
        /// </summary>
        public double ValueNonCalNonZero
        {
            get { return _valueNonCalNonZero; }
            set
            {
                _valueNonCalNonZero = value;
                RaisePropertyChanged(() => ValueNonCalNonZero);
            }
        }

        /// <summary>
        /// 物理值（已标校，未调零）
        /// </summary>
        private double _valueCaledNonZero;
        /// <summary>
        /// 物理值（已标校，未调零）
        /// </summary>
        public double ValueCaledNonZero
        {
            get { return _valueCaledNonZero; }
            set
            {
                _valueCaledNonZero = value;
                RaisePropertyChanged(() => ValueCaledNonZero);
            }
        }

        /// <summary>
        /// 物理值(调零、标校后)
        /// </summary>
        private double _valueFinal;
        /// <summary>
        /// 物理值(调零、标校后)
        /// </summary>
        public double ValueFinal
        {
            get { return _valueFinal; }
            set
            {
                _valueFinal = value;
                if (IsOutType)
                    CalcAODatas();
                RaisePropertyChanged(() => ValueFinal);
            }
        }

        #endregion


        #region 计算方法

        /// <summary>
        /// 计算AI物理值（标定前、标定后、标定并调零后）
        /// </summary>
        public void CalcAIValues()
        {
            //数据直接传输型
            if (InfType == "Digital")
            {
                //物理值=传输值*转换比率
                ValueNonCalNonZero = AnalogChannel.DataAfterFil * ConvRatio;
            }
            //电偶型。
            else if (InfType == "Couple")
            {
                //物理值=传输值*转换比率
                ValueNonCalNonZero = AnalogChannel.DataAfterFil * ConvRatio;
            }
            //电压电流型。
            else
            {
                //计算量程上下限对应采集值
                //   CalcData_SigBounds();
                //计算传感器物理值（未标校）。物理值=（当前传输值-传输值下限）*（量程上限-量程下限）/（传输值上限-传输值下限）+量程下限
                ValueNonCalNonZero = GetY(Data_SingalMax, Data_SingalMin, SingalUpperRange, SingalLowerRange, AnalogChannel.DataAfterFil);
            }

            ValueCaledNonZero = ValueNonCalNonZero;

            //标定。输入标定点数据时对标定点列表进行排序，此处不再排序处理。
            if (CalPoints[0].IsUse && CalPoints[1].IsUse)
            {
                bool find = false;
                for (int i = 0; i < 10; i++)
                {
                    //当前值等于当前标定点显示值
                    if (((ValueNonCalNonZero - CalPoints[i].ViewValue) == 0)&&(CalPoints[i].IsUse))
                    {
                        ValueCaledNonZero = CalPoints[i].StdValue;
                        find = true;
                        break;
                    }

                    //如果下一个标定点未使用，则用前一段标定点计算
                    if (!CalPoints[i + 1].IsUse)
                    {
                        ValueCaledNonZero = GetY(CalPoints[i].ViewValue, CalPoints[i - 1].ViewValue,
                            CalPoints[i].StdValue, CalPoints[i - 1].StdValue,
                            ValueNonCalNonZero);
                        find = true;
                        break;
                    }

                    //当前值小于下一个标定点显示值
                    if ((ValueNonCalNonZero - CalPoints[i + 1].ViewValue) <= 0)
                    {
                        ValueCaledNonZero = GetY(CalPoints[i + 1].ViewValue, CalPoints[i].ViewValue,
                             CalPoints[i + 1].StdValue, CalPoints[i].StdValue,
                             ValueNonCalNonZero);
                        find = true;
                        break;
                    }

                    //如果是最后一段标定点
                    if (i == 9)
                    {
                        ValueCaledNonZero = GetY(CalPoints[10].ViewValue, CalPoints[9].ViewValue,
                            CalPoints[10].StdValue, CalPoints[9].StdValue,
                            ValueNonCalNonZero);
                        find = true;
                        break;
                    }
                }  
            }

            //调零、斜率修正.y=k*x+b
            ValueFinal = ValueCaledNonZero * KCalValue + ZeroCalValue;
        }

        
        /// <summary>
        /// 由AI最终值计算传输值
        /// </summary>
        public double GetTransDataFromValueFinal_AI(double valueFinal)
        {
            double transData, caledNonZero, nonCalNonZero;

            //反推调零前数值。y=k*x+b，x=(y-b)/k
            if (KCalValue == 0)
                caledNonZero = 0;
            else
                caledNonZero = (valueFinal - ZeroCalValue) / KCalValue;

            //反推标定前数值
            nonCalNonZero = caledNonZero;


            //标定。输入标定点数据时对标定点列表进行排序，此处不再排序处理。
            if (CalPoints[0].IsUse && CalPoints[1].IsUse)
            {
                for (int i = 0; i < 11; i++)
                {
                    //当前值等于当前标定点显示值
                    if ((caledNonZero - CalPoints[i].StdValue) == 0)
                    {
                        nonCalNonZero = CalPoints[i].ViewValue;
                        break;
                    }

                    //如果下一个标定点未使用，则用前一段标定点计算
                    if (!CalPoints[i + 1].IsUse)
                    {
                        nonCalNonZero = GetY(CalPoints[i].StdValue, CalPoints[i - 1].StdValue, CalPoints[i].ViewValue, CalPoints[i - 1].ViewValue, caledNonZero);
                        break;
                    }

                    //当前值小于下一个标定点标准值
                    if ((caledNonZero - CalPoints[i + 1].StdValue) <= 0)
                    {
                        nonCalNonZero = GetY(CalPoints[i+1].StdValue, CalPoints[i ].StdValue, CalPoints[i+1].ViewValue, CalPoints[i].ViewValue, caledNonZero);
                        break;
                    }
                }
            }

            //反推传输值
            //数据直接传输型
            if (InfType == "Digital")
            {
                if (ConvRatio == 0)
                    transData = 0;
                else
                    transData = nonCalNonZero / ConvRatio;
            }
            //电偶型。
            else if (InfType == "Couple")
            {
                if (ConvRatio == 0)
                    transData = 0;
                else
                    transData = nonCalNonZero / ConvRatio;
            }
            //电压电流型。
            else
            {
                //传输值=（当前物理值-量程下限）*（传输值上限-传输值下限）/（量程上限-量程下限）+传输值下限
                transData = GetY(SingalUpperRange, SingalLowerRange, Data_SingalMax, Data_SingalMin, nonCalNonZero);
            }

            return transData;
        }

        /// <summary>
        /// 计算AO传输值及过程变量（标定前、标定后、标定并调零后）
        /// </summary>
        public void CalcAODatas()
        {

            //调零、斜率修正.y=k(x+b),x=y/k-b
            if (KCalValue != 0)
                ValueCaledNonZero = ValueFinal / KCalValue - ZeroCalValue;
            else
                ValueCaledNonZero = 0;

            ValueNonCalNonZero = ValueCaledNonZero;

            //标定。输入标定点数据时对标定点列表进行排序，此处不再排序处理。
            //if ((CalPoints[0].IsUse) && (CalPoints[1].IsUse))
            //{
            //    for (int i = 0; i < 11; i++)
            //    {
            //        //如果下一个标定点未使用，则用前一段标定点计算
            //        if (!CalPoints[i + 1].IsUse)
            //        {
            //            ValueCaledNonZero = GetY(CalPoints[i].ViewValue, CalPoints[i - 1].ViewValue,
            //                CalPoints[i].StdValue, CalPoints[i - 1].StdValue,
            //                ValueNonCalNonZero);
            //            break;
            //        }
            //        //当前值等于当前标定点显示值
            //        if ((ValueNonCalNonZero - CalPoints[i].ViewValue) == 0)
            //        {
            //            ValueCaledNonZero = CalPoints[i].StdValue;
            //            break;
            //        }
            //        //当前值小于下一个标定点显示值
            //        if ((ValueNonCalNonZero - CalPoints[i + 1].ViewValue) <= 0)
            //        {
            //            ValueCaledNonZero = GetY(CalPoints[i + 1].ViewValue, CalPoints[i].ViewValue,
            //                 CalPoints[i + 1].StdValue, CalPoints[i].StdValue,
            //                 ValueNonCalNonZero);
            //            break;
            //        }
            //    }
            //}


            //数据直接传输型
            if (InfType == "Digital")
            {
                //物理值=传输值*转换比率
                if (ConvRatio != 0)
                    AnalogChannel.DataRealTime = ValueNonCalNonZero / ConvRatio;
                else
                    AnalogChannel.DataRealTime = 0;
            }
            //电偶型。
            else if (InfType == "Couple")
            {
                //物理值=传输值*转换比率
                if (ConvRatio != 0)
                    AnalogChannel.DataRealTime = ValueNonCalNonZero / ConvRatio;
                else
                    AnalogChannel.DataRealTime = 0;
            }
            //电压电流型。
            else
            {
                //计算量程上下限对应采集值
                //  CalcData_SigBounds();
                //计算AO通道出换数值。传输值=（目标物理值-量程下限）*（传输值上限-传输值下限）/（量程上限-量程下限）+传输值下限
                AnalogChannel.DataRealTime = GetY(SingalUpperRange, SingalLowerRange, Data_SingalMax, Data_SingalMin, ValueNonCalNonZero);
            }
        }


        /// <summary>
        /// 计算信号量程对应传输值，参数载入时计算
        /// </summary>
        public void CalcData_SigBounds()
        {
            //计算信号量程上限对应传输值。传数上=CalcY（采电上,采电下,采数上,采数下,信电上*比率）
            Data_SingalMax = GetY(AnalogChannel.ElecSigUpperRange,
                AnalogChannel.ElecSigLowerRange,
                AnalogChannel.DataUpperRange,
                AnalogChannel.DataLowerRange,
                ElecSigUpperRange * ConvRatio);
            //计算信号量程下限对应传输值。传数下=CalcY（采电上,采电下,采数上,采数下,信电下*比率）
            Data_SingalMin = GetY(AnalogChannel.ElecSigUpperRange,
                AnalogChannel.ElecSigLowerRange,
                AnalogChannel.DataUpperRange,
                AnalogChannel.DataLowerRange,
                ElecSigLowerRange * ConvRatio);
        }

        /// <summary>
        /// 根据XY上下限及当前X值，计算当前Y值。Y=（X-lowerX）*（upperY-lowerY）/（upperX-lowerX）+lowerY
        /// </summary>
        /// <param name="upperX">X上限</param>
        /// <param name="lowerX">X下限</param>
        /// <param name="upperY">Y上限</param>
        /// <param name="lowerY">Y下限</param>
        /// <param name="x">X当前值</param>
        /// <returns>当前Y值计算结果</returns>
        public double GetY(double upperX, double lowerX, double upperY, double lowerY, double x)
        {
            double tempY = 0;
            if ((upperX - lowerX) != 0)
                tempY = (x - lowerX) * (upperY - lowerY) / (upperX - lowerX) + lowerY;
            return tempY;
        }

        #endregion

    }


    //-----------------------------------------------------------------------------------------------------
    /// <summary>
    /// 模拟量模块Model类
    /// </summary>
    public class AnalogModuleModel : ObservableObject
    {
        /// <summary>
        /// 模块编号
        /// </summary>
        private string _moduleNO = "04AD-1";
        /// <summary>
        /// 模块编号
        /// </summary>
        public string ModuleNO
        {
            get { return _moduleNO; }
            set
            {
                _moduleNO = value;
                RaisePropertyChanged(() => ModuleNO);
            }
        }

        /// <summary>
        /// 模块名称
        /// </summary>
        private string _moduleName = "04AD模块1";
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName
        {
            get { return _moduleName; }
            set
            {
                _moduleName = value;
                RaisePropertyChanged(() => ModuleName);
            }
        }

        /// <summary>
        /// 通道列表
        /// </summary>
        private ObservableCollection<AnalogChannelModel> _channels = new ObservableCollection<AnalogChannelModel>();
        /// <summary>
        /// 通道列表
        /// </summary>
        public ObservableCollection<AnalogChannelModel> Channels
        {
            get { return _channels; }
            set
            {
                _channels = value;
                RaisePropertyChanged(() => Channels);
            }
        }
    }

    //-----------------------------------------------------------------------------------------------------
    /// <summary>
    /// 模拟量通道Model类
    /// </summary>
    public class AnalogChannelModel : ObservableObject
    {
        #region 通道设定参数

        /// <summary>
        /// 通道编号
        /// </summary>
        private string _channelNO = "04AD-1-1";
        /// <summary>
        /// 通道编号
        /// </summary>
        public string ChannelNO
        {
            get { return _channelNO; }
            set
            {
                _channelNO = value;
                RaisePropertyChanged(() => ChannelNO);
            }
        }

        /// <summary>
        /// 接口类型（Voltage电压型，Current电流型，Digital数据型，Couple电偶型）
        /// </summary>
        private string _infType = "Voltage";
        /// <summary>
        /// 接口类型（Voltage电压型，Current电流型，Digital数据型，Couple电偶型）
        /// </summary>
        public string InfType
        {
            get { return _infType; }
            set
            {
                _infType = value;
                RaisePropertyChanged(() => InfType);
            }
        }

        /// <summary>
        /// 通道电信号单位
        /// </summary>
        private string _elecSigUnit = "V";
        /// <summary>
        /// 通道电信号单位
        /// </summary>
        public string ElecSigUnit
        {
            get { return _elecSigUnit; }
            set
            {
                _elecSigUnit = value;
                RaisePropertyChanged(() => ElecSigUnit);
            }
        }

        /// <summary>
        /// 通道电信号下限
        /// </summary>
        private double _elecSigLowerRange = 0;
        /// <summary>
        /// 通道电信号下限
        /// </summary>
        public double ElecSigLowerRange
        {
            get { return _elecSigLowerRange; }
            set
            {
                _elecSigLowerRange = value;
                RaisePropertyChanged(() => ElecSigLowerRange);
            }
        }

        /// <summary>
        /// 通道电信号上限
        /// </summary>
        private double _elecSigUpperRange = 5;
        /// <summary>
        /// 通道电信号上限
        /// </summary>
        public double ElecSigUpperRange
        {
            get { return _elecSigUpperRange; }
            set
            {
                _elecSigUpperRange = value;
                RaisePropertyChanged(() => ElecSigUpperRange);
            }
        }

        /// <summary>
        /// 通道采集值下限（对应通道电信号下限）
        /// </summary>
        private double _dataLowerRange = 0;
        /// 通道采集值下限（对应通道电信号下限）
        /// </summary>
        public double DataLowerRange
        {
            get { return _dataLowerRange; }
            set
            {
                _dataLowerRange = value;
                RaisePropertyChanged(() => DataLowerRange);
            }
        }

        /// <summary>
        /// 通道采集值上限（对应电信号上限）
        /// </summary>
        private double _dataUpperRange = 4000;
        /// <summary>
        /// 通道采集值上限（对应电信号上限）
        /// </summary>
        public double DataUpperRange
        {
            get { return _dataUpperRange; }
            set
            {
                _dataUpperRange = value;
                RaisePropertyChanged(() => DataUpperRange);
            }
        }

        /// <summary>
        /// 通道占用标志
        /// </summary>
        private bool _isUsed = false;
        /// <summary>
        /// 通道占用标志
        /// </summary>
        public bool IsUsed
        {
            get { return _isUsed; }
            set
            {
                _isUsed = value;
                RaisePropertyChanged(() => IsUsed);
            }
        }

        /// <summary>
        /// 输入输出类型（true为输出）
        /// </summary>
        private bool _isOutType = false;
        /// <summary>
        /// 输入输出类型（true为输出）
        /// </summary>
        public bool IsOutType
        {
            get { return _isOutType; }
            set
            {
                _isOutType = value;
                RaisePropertyChanged(() => IsOutType);
            }
        }

        /// <summary>
        /// 滤波系数（1~50）
        /// </summary>
        private int _fitRatio = 5;
        /// <summary>
        /// 滤波系数（1~50）
        /// </summary>
        public int FitRatio
        {
            get { return _fitRatio; }
            set
            {
                _fitRatio = value;
                if (_fitRatio < 1)
                    _fitRatio = 1;
                if (_fitRatio > 50)
                    _fitRatio = 50;
                RaisePropertyChanged(() => FitRatio);
            }
        }

        #endregion

        #region 运行参数

        /// <summary>
        /// 当前传输值
        /// </summary>
        private double _dataRealTime = 0;
        /// <summary>
        /// 当前传输值
        /// </summary>
        public double DataRealTime
        {
            get { return _dataRealTime; }
            set
            {
                _dataRealTime = value;
                if (!IsOutType)
                    Filter();
                RaisePropertyChanged(() => DataRealTime);
            }
        }

        /// <summary>
        /// 滤波后传输值
        /// </summary>
        private double _dataAfterFil = 0;
        /// <summary>
        /// 滤波后传输值
        /// </summary>
        public double DataAfterFil
        {
            get { return _dataAfterFil; }
            set
            {
                _dataAfterFil = value;
                RaisePropertyChanged(() => DataAfterFil);
            }
        }

        #endregion

        #region 滤波

        /// <summary>
        /// 传输值List
        /// </summary>
        private List<double> _fitList = new List<double>();
        /// <summary>
        /// 传输值List
        /// </summary>
        public List<double> FitList
        {
            get { return _fitList; }
            set
            {
                _fitList = value;
                RaisePropertyChanged(() => FitList);
            }
        }

        /// <summary>
        /// 滤波排序传输值List
        /// </summary>
        private List<double> _fitList_Sort = new List<double>();
        /// <summary>
        /// 滤波排序后传输值List
        /// </summary>
        public List<double> FitList_Sort
        {
            get { return _fitList_Sort; }
            set
            {
                _fitList_Sort = value;
                RaisePropertyChanged(() => FitList_Sort);
            }
        }

        /// <summary>
        /// 传输值滤波
        /// </summary>
        private void Filter()
        {
            //添加数据
            FitList.Add(DataRealTime);
            while (FitList.Count > FitRatio)
                FitList.RemoveAt(0);

            //List复制
            FitList_Sort = CopyEx.DeepCopyByBin(FitList);
            //排序
            FitList_Sort.Sort();

            int tempIndex = (int)Math.Floor((double)FitList_Sort.Count / 2);
            DataAfterFil = FitList_Sort[tempIndex];
        }
        #endregion

    }

    /// <summary>
    /// 标定点类
    /// </summary>
    public class CalPoint : ObservableObject
    {
        /// <summary>
        /// 标定点启用标志
        /// </summary>
        private bool _isUse = false;
        /// <summary>
        /// 标定点启用标志
        /// </summary>
        public bool IsUse
        {
            get { return _isUse; }
            set
            {
                _isUse = value;
                RaisePropertyChanged(() => IsUse);
            }
        }

        /// <summary>
        /// 标准值
        /// </summary>
        private double _stdValue = 0;
        /// <summary>
        /// 标准值
        /// </summary>
        public double StdValue
        {
            get { return _stdValue; }
            set
            {
                _stdValue = value;
                RaisePropertyChanged(() => StdValue);
            }
        }

        /// <summary>
        /// 显示值
        /// </summary>
        private double _viewValue = 0;
        /// <summary>
        /// 显示值
        /// </summary>
        public double ViewValue
        {
            get { return _viewValue; }
            set
            {
                _viewValue = value;
                RaisePropertyChanged(() => ViewValue);
            }
        }

    }


    /// <summary>
    /// 使用序列化深拷贝
    /// </summary>
    public static class CopyEx
    {
        public static T DeepCopyByBin<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                //序列化成流
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                //反序列化成对象
                retval = bf.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }
    }
}
