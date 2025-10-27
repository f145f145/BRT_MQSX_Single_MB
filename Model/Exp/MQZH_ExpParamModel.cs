/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQDFJ_MB.Model.Exp
 * 文件名：  MQZH_ExpParamModel
 * 版本号：  V1.0.0.0
 * 唯一标识：b77707e1-755e-4706-9a8f-70236c012d3d
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/11/20 12:54:34
 * 描述：
 * 幕墙四性试验设定参数Model
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2021/11/20       12:54:34		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using GalaSoft.MvvmLight;

namespace MQDFJ_MB.Model.Exp
{
    /// <summary>
    /// 试验设定参数类
    /// </summary>
    public class MQZH_ExpParamModel : ObservableObject
    {
        #region 试验基本信息

        /// <summary>
        /// 试验编号
        /// </summary>
        private string _expNO="internal";
        /// <summary>
        /// 试验编号
        /// </summary>
        public string ExpNO
        {
            get
            {
                return _expNO;
            }
            set
            {
                _expNO = value;
                RaisePropertyChanged(() => ExpNO);
            }
        }

        /// <summary>
        /// 试验补充说明
        /// </summary>
        private string _expDetail = "";
        /// <summary>
        /// 试验补充说明
        /// </summary>
        public string ExpDetail
        {
            get
            {
                return _expDetail;
            }
            set
            {
                _expDetail = value;
                RaisePropertyChanged(() => ExpDetail);
            }
        }

        /// <summary>
        /// 三性报告编号
        /// </summary>
        private string _repSXNO = "MQZHRPT001-三性";
        /// <summary>
        /// 三性报告编号
        /// </summary>
        public string RepSXNO
        {
            get
            {
                return _repSXNO;
            }
            set
            {
                _repSXNO = value;
                RaisePropertyChanged(() => RepSXNO);
            }
        }

        /// <summary>
        /// 层间变形报告编号
        /// </summary>
        private string _repCJBXNO = "MQZHRPT001-层间变形";
        /// <summary>
        /// 层间变形报告编号
        /// </summary>
        public string RepCJBXNO
        {
            get
            {
                return _repCJBXNO;
            }
            set
            {
                _repCJBXNO = value;
                RaisePropertyChanged(() => RepCJBXNO);
            }
        }

        /// <summary>
        /// 检测日期
        /// </summary>
        private string _expDate=DateTime.Now.ToString("yyyy-MM-dd");
        /// <summary>
        /// 检测日期
        /// </summary>
        public string ExpDate
        {
            get
            {
                return _expDate;
            }
            set
            {
                _expDate = value;
                RaisePropertyChanged(() => ExpDate);
            }
        }

        /// <summary>
        /// 委托日期
        /// </summary>
        private string _expWTDate = DateTime.Now.ToString("yyyy-MM-dd");
        /// <summary>
        /// 委托日期
        /// </summary>
        public string ExpWTDate
        {
            get
            {
                return _expWTDate;
            }
            set
            {
                _expWTDate = value;
                RaisePropertyChanged(() => ExpWTDate);
            }
        }

        /// <summary>
        /// 委托单位
        /// </summary>
        private string _expWTDW = "——";
        /// <summary>
        /// 委托单位
        /// </summary>
        public string ExpWTDW
        {
            get
            {
                return _expWTDW;
            }
            set
            {
                _expWTDW = value;
                RaisePropertyChanged(() => ExpWTDW);
            }
        }

        /// <summary>
        /// 生产单位
        /// </summary>
        private string _expSCDW = "——";
        /// <summary>
        /// 生产单位
        /// </summary>
        public string ExpSCDW
        {
            get
            {
                return _expSCDW;
            }
            set
            {
                _expSCDW = value;
                RaisePropertyChanged(() => ExpSCDW);
            }
        }

        /// <summary>
        /// 施工单位
        /// </summary>
        private string _expSGDW = "——";
        /// <summary>
        /// 施工单位
        /// </summary>
        public string ExpSGDW
        {
            get
            {
                return _expSGDW;
            }
            set
            {
                _expSGDW = value;
                RaisePropertyChanged(() => ExpSGDW);
            }
        }

        /// <summary>
        /// 工程名称
        /// </summary>
        private string _expGCMC = "——";
        /// <summary>
        /// 工程名称
        /// </summary>
        public string ExpGCMC
        {
            get
            {
                return _expGCMC;
            }
            set
            {
                _expGCMC = value;
                RaisePropertyChanged(() => ExpGCMC);
            }
        }

        #endregion


        #region 试件信息属性

        /// <summary>
        /// 试件名称
        /// </summary>
        private string _exp_SJ_Name = "玻璃幕墙";
        /// <summary>
        /// 试件名称
        /// </summary>
        public string Exp_SJ_Name
        {
            get
            {
                return _exp_SJ_Name;
            }
            set
            {
                _exp_SJ_Name = value;
                RaisePropertyChanged(() => Exp_SJ_Name);
            }
        }

        /// <summary>
        /// 样品编号
        /// </summary>
        private string _exp_SJ_YPNO = "——";
        /// <summary>
        /// 样品编号
        /// </summary>
        public string Exp_SJ_YPNO
        {
            get
            {
                return _exp_SJ_YPNO;
            }
            set
            {
                _exp_SJ_YPNO = value;
                RaisePropertyChanged(() => Exp_SJ_YPNO);
            }
        }

        /// <summary>
        /// 试件类型
        /// </summary>
        private string _exp_SJ_Type = "——";
        /// <summary>
        /// 试件类型
        /// </summary>
        public string Exp_SJ_Type
        {
            get
            {
                return _exp_SJ_Type;
            }
            set
            {
                _exp_SJ_Type = value;
                RaisePropertyChanged(() => Exp_SJ_Type);
            }
        }

        /// <summary>
        /// 试件系列
        /// </summary>
        private string _exp_SJ_Series = "——";
        /// <summary>
        /// 试件系列
        /// </summary>
        public string Exp_SJ_Series
        {
            get
            {
                return _exp_SJ_Series;
            }
            set
            {
                _exp_SJ_Series = value;
                RaisePropertyChanged(() => Exp_SJ_Series);
            }
        }

        /// <summary>
        /// 试件型号
        /// </summary>
        private string _exp_SJ_Model = "——";
        /// <summary>
        /// 试件型号
        /// </summary>
        public string Exp_SJ_Model
        {
            get
            {
                return _exp_SJ_Model;
            }
            set
            {
                _exp_SJ_Model = value;
                RaisePropertyChanged(() => Exp_SJ_Model);
            }
        }

        /// <summary>
        /// 型材品种
        /// </summary>
        private string _exp_SJ_XCPZ = "——";
        /// <summary>
        /// 型材品种
        /// </summary>
        public string Exp_SJ_XCPZ
        {
            get
            {
                return _exp_SJ_XCPZ;
            }
            set
            {
                _exp_SJ_XCPZ = value;
                RaisePropertyChanged(() => Exp_SJ_XCPZ);
            }
        }

        ///// <summary>
        ///// 型材材质
        ///// </summary>
        //private string _exp_SJ_XCCZ = "——";
        ///// <summary>
        ///// 型材材质
        ///// </summary>
        //public string Exp_SJ_XCCZ
        //{
        //    get
        //    {
        //        return _exp_SJ_XCCZ;
        //    }
        //    set
        //    {
        //        _exp_SJ_XCCZ = value;
        //        RaisePropertyChanged(() => Exp_SJ_XCCZ);
        //    }
        //}

        /// <summary>
        /// 型材牌号
        /// </summary>
        private string _exp_SJ_XCPH = "——";
        /// <summary>
        /// 型材牌号
        /// </summary>
        public string Exp_SJ_XCPH
        {
            get
            {
                return _exp_SJ_XCPH;
            }
            set
            {
                _exp_SJ_XCPH = value;
                RaisePropertyChanged(() => Exp_SJ_XCPH);
            }
        }

        /// <summary>
        /// 型材规格
        /// </summary>
        private string _exp_SJ_XCGG = "——";
        /// <summary>
        /// 型材规格
        /// </summary>
        public string Exp_SJ_XCGG
        {
            get
            {
                return _exp_SJ_XCGG;
            }
            set
            {
                _exp_SJ_XCGG = value;
                RaisePropertyChanged(() => Exp_SJ_XCGG);
            }
        }

        /// <summary>
        /// 附件品种
        /// </summary>
        private string _exp_SJ_FJPZ = "——";
        /// <summary>
        /// 附件品种
        /// </summary>
        public string Exp_SJ_FJPZ
        {
            get
            {
                return _exp_SJ_FJPZ;
            }
            set
            {
                _exp_SJ_FJPZ = value;
                RaisePropertyChanged(() => Exp_SJ_FJPZ);
            }
        }

        /// <summary>
        /// 附件材质
        /// </summary>
        private string _exp_SJ_FJCZ = "——";
        /// <summary>
        /// 附件材质
        /// </summary>
        public string Exp_SJ_FJCZ
        {
            get
            {
                return _exp_SJ_FJCZ;
            }
            set
            {
                _exp_SJ_FJCZ = value;
                RaisePropertyChanged(() => Exp_SJ_FJCZ);
            }
        }

        ///// <summary>
        ///// 附件牌号
        ///// </summary>
        //private string _exp_SJ_FJPH = "——";
        ///// <summary>
        ///// 附件牌号
        ///// </summary>
        //public string Exp_SJ_FJPH
        //{
        //    get
        //    {
        //        return _exp_SJ_FJPH;
        //    }
        //    set
        //    {
        //        _exp_SJ_FJPH = value;
        //        RaisePropertyChanged(() => Exp_SJ_FJPH);
        //    }
        //}

        /// <summary>
        /// 面板材料材质
        /// </summary>
        private string _exp_SJ_MBCZ = "——";
        /// <summary>
        /// 面板材料材质
        /// </summary>
        public string Exp_SJ_MBCZ
        {
            get
            {
                return _exp_SJ_MBCZ;
            }
            set
            {
                _exp_SJ_MBCZ = value;
                RaisePropertyChanged(() => Exp_SJ_MBCZ);
            }
        }

        /// <summary>
        /// 面板品种
        /// </summary>
        private string _exp_SJ_MBPZ = "——";
        /// <summary>
        /// 面板品种
        /// </summary>
        public string Exp_SJ_MBPZ
        {
            get
            {
                return _exp_SJ_MBPZ;
            }
            set
            {
                _exp_SJ_MBPZ = value;
                RaisePropertyChanged(() => Exp_SJ_MBPZ);
            }
        }

        /// <summary>
        /// 面板牌号
        /// </summary>
        private string _exp_SJ_MBPH = "——";
        /// <summary>
        /// 面板牌号
        /// </summary>
        public string Exp_SJ_MBPH
        {
            get
            {
                return _exp_SJ_MBPH;
            }
            set
            {
                _exp_SJ_MBPH = value;
                RaisePropertyChanged(() => Exp_SJ_MBPH);
            }
        }

        /// <summary>
        /// 面板厚度
        /// </summary>
        private string _exp_SJ_MBDH = "——";
        /// <summary>
        /// 面板厚度
        /// </summary>
        public string Exp_SJ_MBHD
        {
            get
            {
                return _exp_SJ_MBDH;
            }
            set
            {
                _exp_SJ_MBDH = value;
                RaisePropertyChanged(() => Exp_SJ_MBHD);
            }
        }

        /// <summary>
        /// 面板最大尺寸
        /// </summary>
        private string _exp_SJ_MBZDCC = "——";
        /// <summary>
        /// 面板最大尺寸
        /// </summary>
        public string Exp_SJ_MBZDCC
        {
            get
            {
                return _exp_SJ_MBZDCC;
            }
            set
            {
                _exp_SJ_MBZDCC = value;
                RaisePropertyChanged(() => Exp_SJ_MBZDCC);
            }
        }

        /// <summary>
        /// 面板安装方法
        /// </summary>
        private string _exp_SJ_MBAZFF = "——";
        /// <summary>
        /// 面板安装方法
        /// </summary>
        public string Exp_SJ_MBAZFF
        {
            get
            {
                return _exp_SJ_MBAZFF;
            }
            set
            {
                _exp_SJ_MBAZFF = value;
                RaisePropertyChanged(() => Exp_SJ_MBAZFF);
            }
        }

        /// <summary>
        /// 密封材料品种
        /// </summary>
        private string _exp_SJ_MFCLPZ = "——";
        /// <summary>
        /// 密封材料品种
        /// </summary>
        public string Exp_SJ_MFCLPZ
        {
            get
            {
                return _exp_SJ_MFCLPZ;
            }
            set
            {
                _exp_SJ_MFCLPZ = value;
                RaisePropertyChanged(() => Exp_SJ_MFCLPZ);
            }
        }

        /// <summary>
        /// 密封材料材质
        /// </summary>
        private string _exp_SJ_MFCLCZ = "——";
        /// <summary>
        /// 密封材料材质
        /// </summary>
        public string Exp_SJ_MFCLCZ
        {
            get
            {
                return _exp_SJ_MFCLCZ;
            }
            set
            {
                _exp_SJ_MFCLCZ = value;
                RaisePropertyChanged(() => Exp_SJ_MFCLCZ);
            }
        }

        /// <summary>
        /// 密封材料牌号
        /// </summary>
        private string _exp_SJ_MFCLPH = "——";
        /// <summary>
        /// 密封材料牌号
        /// </summary>
        public string Exp_SJ_MFCLPH
        {
            get
            {
                return _exp_SJ_MFCLPH;
            }
            set
            {
                _exp_SJ_MFCLPH = value;
                RaisePropertyChanged(() => Exp_SJ_MFCLPH);
            }
        }

        /// <summary>
        /// 镶嵌材料品种
        /// </summary>
        private string _exp_SJ_XQCLPZ = "——";
        /// <summary>
        /// 镶嵌材料品种
        /// </summary>
        public string Exp_SJ_XQCLPZ
        {
            get
            {
                return _exp_SJ_XQCLPZ;
            }
            set
            {
                _exp_SJ_XQCLPZ = value;
                RaisePropertyChanged(() => Exp_SJ_XQCLPZ);
            }
        }

        /// <summary>
        /// 镶嵌材料材质
        /// </summary>
        private string _exp_SJ_XQCLCZ = "——";
        /// <summary>
        /// 镶嵌材料材质
        /// </summary>
        public string Exp_SJ_XQCLCZ
        {
            get
            {
                return _exp_SJ_XQCLCZ;
            }
            set
            {
                _exp_SJ_XQCLCZ = value;
                RaisePropertyChanged(() => Exp_SJ_XQCLCZ);
            }
        }

        /// <summary>
        /// 镶嵌材料牌号
        /// </summary>
        private string _exp_SJ_XQPH = "——";
        /// <summary>
        /// 镶嵌材料牌号
        /// </summary>
        public string Exp_SJ_XQPH
        {
            get
            {
                return _exp_SJ_XQPH;
            }
            set
            {
                _exp_SJ_XQPH = value;
                RaisePropertyChanged(() => Exp_SJ_XQPH);
            }
        }

        /// <summary>
        /// 镶嵌尺寸
        /// </summary>
        private string _exp_SJ_XQCC = "——";
        /// <summary>
        /// 镶嵌尺寸
        /// </summary>
        public string Exp_SJ_XQCC
        {
            get
            {
                return _exp_SJ_XQCC;
            }
            set
            {
                _exp_SJ_XQCC = value;
                RaisePropertyChanged(() => Exp_SJ_XQCC);
            }
        }

        /// <summary>
        /// 镶嵌方法
        /// </summary>
        private string _exp_SJ_XQFF = "——";
        /// <summary>
        /// 镶嵌方法
        /// </summary>
        public string Exp_SJ_XQFF
        {
            get
            {
                return _exp_SJ_XQFF;
            }
            set
            {
                _exp_SJ_XQFF = value;
                RaisePropertyChanged(() => Exp_SJ_XQFF);
            }
        }

        /// <summary>
        /// 最大分格尺寸
        /// </summary>
        private string _exp_SJ_ZDFGCC = "——";
        /// <summary>
        /// 最大分格尺寸
        /// </summary>
        public string Exp_SJ_ZDFGCC
        {
            get
            {
                return _exp_SJ_ZDFGCC;
            }
            set
            {
                _exp_SJ_ZDFGCC = value;
                RaisePropertyChanged(() => Exp_SJ_ZDFGCC);
            }
        }

        #endregion


        #region 试件设定参数属性

        /// <summary>
        /// 试件宽度（m）
        /// </summary>
        private double _exp_SJ_Width=6;
        /// <summary>
        /// 试件宽度（m）
        /// </summary>
        public double Exp_SJ_Width
        {
            get
            {
                return _exp_SJ_Width;
            }
            set
            {
                if (value > 0)
                {
                    _exp_SJ_Width = value;
                    Exp_SJ_Aria = Exp_SJ_Heigth * Exp_SJ_Width;
                }
                else
                    _exp_SJ_Width = 6;
                RaisePropertyChanged(() => Exp_SJ_Width);
            }
        }

        /// <summary>
        /// 试件高度（m）
        /// </summary>
        private double _exp_SJ_Heigth=9;
        /// <summary>
        /// 试件高度（m）
        /// </summary>
        public double Exp_SJ_Heigth
        {
            get
            {
                return _exp_SJ_Heigth;
            }
            set
            {
                if (value > 0)
                {
                    _exp_SJ_Heigth = value;
                    Exp_SJ_Aria = Exp_SJ_Heigth * Exp_SJ_Width;
                }
                else
                    _exp_SJ_Heigth = 9;
                RaisePropertyChanged(() => Exp_SJ_Heigth);
            }
        }

        /// <summary>
        /// 试件面积（m2）
        /// </summary>
        private double _exp_SJ_Aria=54;
        /// <summary>
        /// 试件面积（m2）
        /// </summary>
        public double Exp_SJ_Aria
        {
            get
            {
                return _exp_SJ_Aria;
            }
            set
            {
                if (value > 0)
                    _exp_SJ_Aria = value;
                else
                    _exp_SJ_Aria = 54;
                RaisePropertyChanged(() => Exp_SJ_Aria);
            }
        }

        /// <summary>
        /// 开启缝长度（m）
        /// </summary>
        private double _exp_SJ_KQFLenth=6;
        /// <summary>
        /// 开启缝长度（m）
        /// </summary>
        public double Exp_SJ_KQFLenth
        {
            get
            {
                return _exp_SJ_KQFLenth;
            }
            set
            {
                if (Isexp_SJ_WithKKQ)
                {
                    _exp_SJ_KQFLenth = Math.Abs(value);
                    if (_exp_SJ_KQFLenth <= 0)
                        _exp_SJ_KQFLenth = 1;
                }
                else
                {
                    _exp_SJ_KQFLenth = 0;
                }
                RaisePropertyChanged(() => Exp_SJ_KQFLenth);
            }
        }

        /// <summary>
        /// 可开启部分面积（m2）
        /// </summary>
        private double _exp_SJ_KKQAria=2;
        /// <summary>
        /// 可开启部分面积（m2）
        /// </summary>
        public double Exp_SJ_KKQAria
        {
            get
            {
                return _exp_SJ_KKQAria;
            }
            set
            {
                if (Isexp_SJ_WithKKQ)
                {
                    _exp_SJ_KKQAria = Math.Abs(value);
                    if (_exp_SJ_KKQAria <= 0)
                        _exp_SJ_KKQAria = 1;
                }
                else
                {
                    _exp_SJ_KKQAria = 0;
                }
                RatioKQBF = Exp_SJ_KKQAria / Exp_SJ_Aria;
                RaisePropertyChanged(() => Exp_SJ_KKQAria);
            }
        }

        /// <summary>
        /// 层高（m）
        /// </summary>
        private double _exp_SJ_CG=3;
        /// <summary>
        /// 层高（m）
        /// </summary>
        public double SJ_CG
        {
            get
            {
                return _exp_SJ_CG;
            }
            set
            {
                if (value > 0)
                    _exp_SJ_CG = value;
                RaisePropertyChanged(() => SJ_CG);
            }
        }

        /// <summary>
        /// 有可开启部分
        /// </summary>
        private bool _isexp_SJ_WithKKQ=true ;
        /// <summary>
        /// 有可开启部分
        /// </summary>
        public bool Isexp_SJ_WithKKQ
        {
            get
            {
                return _isexp_SJ_WithKKQ;
            }
            set
            {
                _isexp_SJ_WithKKQ = value;
                if (!value)
                {
                    Exp_SJ_KKQAria = 0;
                    Exp_SJ_KQFLenth = 0;
                }
                else
                {
                    if (Exp_SJ_KKQAria <= 0)
                        Exp_SJ_KKQAria = 1;
                    if (Exp_SJ_KQFLenth <= 0)
                        Exp_SJ_KQFLenth = 1;
                }
                RaisePropertyChanged(() => Isexp_SJ_WithKKQ);
            }
        }
        
        /// <summary>
        /// 开启部分占比
        /// </summary>
        private double _ratioKQBF=1;
        /// <summary>
        /// 开启部分占比
        /// </summary>
        public double RatioKQBF
        {
            get
            {
                return _ratioKQBF;
            }
            set
            {
                _ratioKQBF = value;
                RaisePropertyChanged(() => RatioKQBF);
            }
        }

        #endregion

    }
}
