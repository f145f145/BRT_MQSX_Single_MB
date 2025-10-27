/************************************************************************************
 * 创建人：  郝正强
 * 描述：
 * 装置Model，装置基本参数部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022-5-26 10:53:52		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Windows;

namespace MQZHWL.Model.DEV
{
    public partial class MQZH_DevModel_Main : ObservableObject
    {
        /// <summary>
        /// 绘图更新周期(ms)
        /// </summary>
        private int _plotPeriod = 100;
        /// <summary>
        /// 绘图更新周期(ms)
        /// </summary>
        public int PlotPeriod
        {
            get { return _plotPeriod; }
            set
            {
                _plotPeriod = value;
                RaisePropertyChanged(() => PlotPeriod);
            }
        }

        /// <summary>
        /// 单曲线绘图总点数
        /// </summary>
        private int _pointsPerLine = 10000;
        /// <summary>
        /// 单曲线绘图总点数
        /// </summary>
        public int PointsPerLine
        {
            get { return _pointsPerLine; }
            set
            {
                _pointsPerLine = value;
                RaisePropertyChanged(() => PointsPerLine);
            }
        }

        /// <summary>
        /// 开机载入是否最后试验（false时开机载入默认试验，true时开机载入最后试验）
        /// </summary>
        private bool _isLoadLastExpPowerOn = false;
        /// <summary>
        /// 开机载入是否最后试验（false时开机载入默认试验，true时开机载入最后试验）
        /// </summary>
        public bool IsLoadLastExpPowerOn
        {
            get { return _isLoadLastExpPowerOn; }
            set
            {
                _isLoadLastExpPowerOn = value;
                RaisePropertyChanged(() => IsLoadLastExpPowerOn);
            }
        }

        /// <summary>
        /// 最后打开的试验编号
        /// </summary>
        private string _expNOLast = "DefaultExp";
        /// <summary>
        /// 最后打开的试验编号
        /// </summary>
        public string ExpNOLast
        {
            get { return _expNOLast; }
            set
            {
                _expNOLast = value;
                RaisePropertyChanged(() => ExpNOLast);
            }
        }

        /// <summary>
        /// 活动框高（mm，XY测量轴线到上轴）
        /// </summary>
        private double _h_HDK = 8500;
        /// <summary>
        /// 活动框高（mm，XY测量轴线到上轴）
        /// </summary>
        public double H_HDK
        {
            get { return _h_HDK; }
            set
            {
                if (value > 0)
                    _h_HDK = value;
                else
                    _h_HDK = 8500;
                RaisePropertyChanged(() => H_HDK);
            }
        }

        /// <summary>
        /// 活动框宽（mm）
        /// </summary>
        private double _w_HDK = 6000;
        /// <summary>
        /// 活动框宽（mm）
        /// </summary>
        public double W_HDK
        {
            get { return _w_HDK; }
            set
            {
                if (value > 0)
                    _w_HDK = value;
                else
                    _w_HDK = 6000;
                RaisePropertyChanged(() => W_HDK);
            }
        }

        /// <summary>
        /// 水管管径（mm，直径）
        /// </summary>
        private double _gj_SG = 102;
        /// <summary>
        /// 水管管径（mm，直径）
        /// </summary>
        public double GJ_SG
        {
            get { return _gj_SG; }
            set
            {
                _gj_SG = value; RaisePropertyChanged(() => GJ_SG);
            }
        }
        
        /// <summary>
        /// 使用三合一传感器
        /// </summary>
        private bool _isUseTHP = false;
        /// <summary>
        /// 使用三合一传感器
        /// </summary>
        public bool IsUseTHP
        {
            get { return _isUseTHP; }
            set
            {
                _isUseTHP = value;
                RaisePropertyChanged(() => IsUseTHP);
            }
        }

        /// <summary>
        /// 三合一传感器类型
        /// </summary>
        private int _thpType = 1;
        /// <summary>
        /// 三合一传感器类型
        /// </summary>
        public int THPType
        {
            get { return _thpType; }
            set
            {
                _thpType = value;
                switch (_thpType)
                {
                    case 1:
                        AIList[19].ConvRatio = 0.1;
                        AIList[20].ConvRatio = 0.1;
                        AIList[21].ConvRatio = 10;
                        THPCom.BoundRate = 9600;
                        break;

                    case 2:
                        AIList[19].ConvRatio = 0.01;
                        AIList[20].ConvRatio = 0.01;
                        AIList[21].ConvRatio = 100;
                        THPCom.BoundRate = 9600;
                        break;

                    case 3:
                        AIList[19].ConvRatio = 0.1;
                        AIList[20].ConvRatio = 0.1;
                        AIList[21].ConvRatio = 100;
                        THPCom.BoundRate = 9600;
                        break;

                    default:
                        AIList[19].ConvRatio = 0.1;
                        AIList[20].ConvRatio = 0.1;
                        AIList[21].ConvRatio = 10;
                        THPCom.BoundRate = 9600;
                        break;
                }
                RaisePropertyChanged(() => THPType);
            }
        }


        /// <summary>
        /// 有中压差传感器（三个差压）
        /// </summary>
        private bool _isWithCYM = false;
        /// <summary>
        /// 有中压差传感器（三个差压）
        /// </summary>
        public bool IsWithCYM
        {
            get { return _isWithCYM; }
            set
            {
                _isWithCYM = value;
                RaisePropertyChanged(() => IsWithCYM);
            }
        }
        
        /// <summary>
        /// 是否有电子水流量计
        /// </summary>
        private bool _isWithSLL = false;
        /// <summary>
        /// 是否有电子水流量计
        /// </summary>
        public bool IsWithSLL
        {
            get { return _isWithSLL; }
            set
            {
                _isWithSLL = value;
                RaisePropertyChanged(() => IsWithSLL);
            }
        }

        /// <summary>
        /// 抗风压位移取反
        /// </summary>
        private bool _isWYKFYF = false;
        /// <summary>
        /// 抗风压位移取反
        /// </summary>
        public bool IsWYKFYF
        {
            get { return _isWYKFYF; }
            set
            {
                _isWYKFYF = value;
                RaisePropertyChanged(() => IsWYKFYF);
            }
        }


        /// <summary>
        /// 波动低压准备频率
        /// </summary>
        private int _phBDZB = 500;
        /// <summary>
        /// 波动低压准备频率
        /// </summary>
        public int PhBDZB
        {
            get { return _phBDZB; }
            set
            {
                _phBDZB = value;
                RaisePropertyChanged(() => PhBDZB);
            }
        }

        /// <summary>
        /// 层间变形类别（0连续平行四边形法，1层间变形法）
        /// </summary>
        private int _cjbxType = 0;
        /// <summary>
        /// 层间变形类别（0连续平行四边形法，1层间变形法）
        /// </summary>
        public int CJBXType
        {
            get { return _cjbxType; }
            set
            {
                _cjbxType = value;
                RaisePropertyChanged(() => CJBXType);
            }
        }


        /// <summary>
        /// 层间变形类型列表
        /// </summary>
        private List<string> _cjbxTypeList = new List<string>() { "连续平行四边形法", "层间变形法"} ;
        /// <summary>
        /// 层间变形类型列表
        /// </summary>
        public List<string> CJBXTypeList
        {
            get { return _cjbxTypeList; }
            set
            {
                _cjbxTypeList = value;
                RaisePropertyChanged(() => CJBXTypeList);
            }
        }

        /// <summary>
        /// 允许修改报告数据
        /// </summary>
        private bool _permitChangeData = false;
        /// <summary>
        /// 允许修改报告数据
        /// </summary>
        public bool PermitChangeData
        {
            get { return _permitChangeData; }
            set
            {
                _permitChangeData = value;
                RaisePropertyChanged(() => PermitChangeData);
            }
        }

        /// <summary>
        /// 位移尺数量（9，或12）
        /// </summary>
        private int _wyQty = 9;
        /// <summary>
        /// 使用9位移尺（9位移尺true，12位移尺false)
        /// </summary>
        public int WYQty
        {
            get { return _wyQty; }
            set
            {
                _wyQty = value;
                if (_wyQty == 12)
                {
                    Is12WY = true;
                    WYNumTypeList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                }
                else
                {
                    Is12WY = false;
                    WYNumTypeList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9};
                }
                RaisePropertyChanged(() => WYQty);
            }
        }
        
        /// <summary>
        /// 位移尺编号（X轴，Y轴1、2、3，Z轴1、2、3）
        /// </summary>
        private ObservableCollection<int> _wyNOList = new ObservableCollection<int>()
        {
            3,4,5,6,7,8,9
        };
        /// <summary>
        /// 位移尺编号（X轴，Y轴1、2、3，Z轴1、2、3）
        /// </summary>
        public ObservableCollection<int> WYNOList
        {
            get { return _wyNOList; }
            set
            {
                _wyNOList = value;
                CheckWYNO();
                RaisePropertyChanged(() => WYNOList);
            }
        }


        #region 风管相关参数

        /// <summary>
        /// 选用测速风管1
        /// </summary>
        private bool _useCSFG1 = false;

        /// <summary>
        /// 选用测速风管1
        /// </summary>
        public bool UseCSFG1
        {
            get { return _useCSFG1; }
            set
            {
                _useCSFG1 = value;
                if (_useCSFG1)
                {
                    UseCSFG2 = false;
                    UseCSFG3 = false;
                }

                RaisePropertyChanged(() => UseCSFG1);
                RaisePropertyChanged(() => DFUse);
                RaisePropertyChanged(() => FSYUse);
                RaisePropertyChanged(() => FSGUse);
            }
        }

        /// <summary>
        /// 有测速风管1
        /// </summary>
        private bool _withCSFG1 = true;
        /// <summary>
        /// 有测速风管1
        /// </summary>
        public bool WithCSFG1
        {
            get { return _withCSFG1; }
            set
            {
                _withCSFG1 = value; RaisePropertyChanged(() => WithCSFG1);
            }
        }


        /// <summary>
        /// 测速风管1管径（mm，直径）
        /// </summary>
        private double _gj_FG1 = 60;
        /// <summary>
        /// 测速风管1管径（mm，直径）
        /// </summary>
        public double GJ_FG1
        {
            get { return _gj_FG1; }
            set
            {
                _gj_FG1 = value; RaisePropertyChanged(() => GJ_FG1);
            }
        }


        /// <summary>
        /// 测速风管1对应蝶阀编号
        /// </summary>
        private int _dfNo_FG1 = 2;
        /// <summary>
        /// 测速风管1对应蝶阀编号
        /// </summary>
        public int DFNo_FG1
        {
            get { return _dfNo_FG1; }
            set
            {
                if ((value == 1) || (value == 2) || (value == 3) || (value == 4))
                    _dfNo_FG1 = value;
                RaisePropertyChanged(() => DFNo_FG1);
            }
        }


        /// <summary>
        /// 测速风管1对应风速仪编号
        /// </summary>
        private int _fsNo_FG1 = 1;
        /// <summary>
        /// 测速风管1对应风速仪编号
        /// </summary>
        public int FSNo_FG1
        {
            get { return _fsNo_FG1; }
            set
            {
                if ((value == 1) || (value == 2) || (value == 3))
                    _fsNo_FG1 = value;
                RaisePropertyChanged(() => FSNo_FG1);
            }
        }

        /// <summary>
        /// 选用测速风管2
        /// </summary>
        private bool _useCSFG2 = true;
        /// <summary>
        /// 选用测速风管2
        /// </summary>
        public bool UseCSFG2
        {
            get { return _useCSFG2; }
            set
            {
                _useCSFG2 = value;
                if (_useCSFG2)
                {
                    UseCSFG1 = false;
                    UseCSFG3 = false;
                }
                RaisePropertyChanged(() => UseCSFG2);
                RaisePropertyChanged(() => DFUse);
                RaisePropertyChanged(() => FSYUse);
                RaisePropertyChanged(() => FSGUse);
            }
        }


        /// <summary>
        /// 有测速风管2
        /// </summary>
        private bool _withCSFG2 = true;
        /// <summary>
        /// 有测速风管1
        /// </summary>
        public bool WithCSFG2
        {
            get { return _withCSFG2; }
            set
            {
                _withCSFG2 = value; RaisePropertyChanged(() => WithCSFG2);
            }
        }


        /// <summary>
        /// 测速风管2管径（mm，直径）
        /// </summary>
        private double _gj_FG2 = 102;
        /// <summary>
        /// 测速风管2管径（mm，直径）
        /// </summary>
        public double GJ_FG2
        {
            get { return _gj_FG2; }
            set
            {
                _gj_FG2 = value; RaisePropertyChanged(() => GJ_FG2);
            }
        }


        /// <summary>
        /// 测速风管2对应蝶阀编号
        /// </summary>
        private int _dfNo_FG2 = 3;
        /// <summary>
        /// 测速风管1对应蝶阀编号
        /// </summary>
        public int DFNo_FG2
        {
            get { return _dfNo_FG2; }
            set
            {
                if ((value == 1) || (value == 2) || (value == 3) || (value == 4))
                    _dfNo_FG2 = value;
                RaisePropertyChanged(() => DFNo_FG2);
            }
        }


        /// <summary>
        /// 测速风管2对应风速仪编号
        /// </summary>
        private int _fsNo_FG2 = 2;
        /// <summary>
        /// 测速风管2对应风速仪编号
        /// </summary>
        public int FSNo_FG2
        {
            get { return _fsNo_FG2; }
            set
            {
                if ((value == 1) || (value == 2) || (value == 3))
                    _fsNo_FG2 = value;
                RaisePropertyChanged(() => FSNo_FG2);
            }
        }

        /// <summary>
        /// 选用测速风管3
        /// </summary>
        private bool _useCSFG3 = false;
        /// <summary>
        /// 选用测速风管3
        /// </summary>
        public bool UseCSFG3
        {
            get { return _useCSFG3; }
            set
            {
                _useCSFG3 = value;
                if (_useCSFG3)
                {
                    UseCSFG1 = false;
                    UseCSFG2 = false;
                }
                RaisePropertyChanged(() => UseCSFG3);
                RaisePropertyChanged(() => DFUse);
                RaisePropertyChanged(() => FSYUse);
                RaisePropertyChanged(() => FSGUse);
            }
        }


        /// <summary>
        /// 有测速风管3
        /// </summary>
        private bool _withCSFG3 = true;
        /// <summary>
        /// 有测速风管3
        /// </summary>
        public bool WithCSFG3
        {
            get { return _withCSFG3; }
            set
            {
                _withCSFG3 = value; RaisePropertyChanged(() => WithCSFG3);
            }
        }


        /// <summary>
        /// 测速风管3管径（mm，直径）
        /// </summary>
        private double _gj_FG3 = 60;
        /// <summary>
        /// 测速风管3管径（mm，直径）
        /// </summary>
        public double GJ_FG3
        {
            get { return _gj_FG3; }
            set
            {
                _gj_FG3 = value; RaisePropertyChanged(() => GJ_FG3);
            }
        }


        /// <summary>
        /// 测速风管3对应蝶阀编号
        /// </summary>
        private int _dfNo_FG3 = 1;
        /// <summary>
        /// 测速风管3对应蝶阀编号
        /// </summary>
        public int DFNo_FG3
        {
            get { return _dfNo_FG3; }
            set
            {
                if ((value == 1) || (value == 2) || (value == 3) || (value == 4))
                    _dfNo_FG3 = value;
                RaisePropertyChanged(() => DFNo_FG3);
            }
        }


        /// <summary>
        /// 测速风管3对应风速仪编号
        /// </summary>
        private int _fsNo_FG3 = 3;
        /// <summary>
        /// 测速风管3对应风速仪编号
        /// </summary>
        public int FSNo_FG3
        {
            get { return _fsNo_FG3; }
            set
            {
                if ((value == 1) || (value == 2) || (value == 3))
                    _fsNo_FG3 = value;
                RaisePropertyChanged(() => FSNo_FG3);
            }
        }

        /// <summary>
        /// 测风速蝶阀编号
        /// </summary>
        public int DFUse
        {
            get
            {
                if (UseCSFG1)
                    return DFNo_FG1;
                else if (UseCSFG2)
                    return DFNo_FG2;
                else
                    return DFNo_FG3;
            }
        }


        /// <summary>
        /// 测风速用风速管编号
        /// </summary>
        public int FSGUse
        {
            get
            {
                if (UseCSFG1)
                    return 1;
                else if (UseCSFG2)
                    return 2;
                else
                    return 3;
            }
        }


        /// <summary>
        /// 测风速用风速仪编号
        /// </summary>
        public int FSYUse
        {
            get
            {
                if (UseCSFG1)
                    return FSNo_FG1;
                else if (UseCSFG2)
                    return FSNo_FG2;
                else
                    return FSNo_FG3;
            }
        }


        #endregion

        #region 辅助方法、属性


        /// <summary>
        /// 位移传感器数量类型列表
        /// </summary>
        private List<int> _wyQtyList = new List<int>() { 9,12 };
        /// <summary>
        /// 位移传感器数量类型列表
        /// </summary>
        public List<int> WYQtyList
        {
            get { return _wyQtyList; }
            set
            {
                _wyQtyList = value;
                RaisePropertyChanged(() => WYQtyList);
            }
        }
        
        /// <summary>
        /// 位移传感器编号列表
        /// </summary>
        private List<int> _wyNumTypeList = new List<int>() { 1,2,3,4,5,6,7,8,9,11,12 };
        /// <summary>
        /// 位移传感器编号列表
        /// </summary>
        public List<int> WYNumTypeList
        {
            get { return _wyNumTypeList; }
            set
            {
                _wyNumTypeList = value;
                RaisePropertyChanged(() => WYNumTypeList);
            }
        }

        /// <summary>
        /// 是否是12个位移尺
        /// </summary>
        private bool _is12WY = false;
        /// <summary>
        /// 是否是12个位移尺
        /// </summary>
        public bool Is12WY
        {
            get { return _is12WY; }
            set
            {
                _is12WY = value;
                RaisePropertyChanged(() => Is12WY);
            }
        }

        /// <summary>
        /// 三合一传感器类型列表
        /// </summary>
        private List<int> _thpTypeList = new List<int>() { 1, 2, 3 };
        /// <summary>
        /// 三合一传感器类型列表
        /// </summary>
        public List<int> THPTypeList
        {
            get { return _thpTypeList; }
            set
            {
                _thpTypeList = value;
                RaisePropertyChanged(() => THPTypeList);
            }
        }

        /// <summary>
        /// 风速管选择类型列表
        /// </summary>
        private List<string> _fsgTypeList = new List<string>() { "细管","中管","粗管" };
        /// <summary>
        /// 风速管选择类型列表
        /// </summary>
        public List<string> FSGTypeList
        {
            get { return _fsgTypeList; }
            set
            {
                _fsgTypeList = value;
                RaisePropertyChanged(() => FSGTypeList);
            }
        }


        //检测位移尺编号是否超范围或重复
        private void CheckWYNO()
        {
            for (int i = 0; i < WYNOList.Count; i++)
            {
                if (WYNOList[i] > WYQty)
                    MessageBox.Show("位移尺编号大于位移尺数量，请重新修正！");
                if (WYNOList[i] <= 0)
                    MessageBox.Show("位移尺编号应大于0，请重新修正！");
            }
        }

        #endregion
    }
}