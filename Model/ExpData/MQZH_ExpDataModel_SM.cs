/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQZHWL.Model.ExpData
 * 文件名：  MQZH_ExpDataModel_SM
 * 版本号：  V1.0.0.0
 * 唯一标识：1b555772-626f-4da1-af51-922619f76b92
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/12/25 7:22:31
 * 描述：
 * 建筑幕墙水密实验数据
 * ==================================================================================
 * 修改标记
 * 修改时间			    修改人			版本号			描述
 * 2021/12/25 7:22:31		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace MQZHWL.Model.ExpData
{
    public class MQZH_ExpDataModel_SM : ObservableObject
    {
        public MQZH_ExpDataModel_SM()
        {
            //损坏类型初始化
            SLTypeListSM = new List<SMSLType>( );
            SMSLType tempSLType0 =new SMSLType(){TypeID =0, Sign ="无渗漏",Status = "无渗漏"};
            SMSLType tempSLType1 = new SMSLType() { TypeID = 1, Sign = "○", Status = "试件内侧出现水滴" };
            SMSLType tempSLType2 = new SMSLType() { TypeID = 2, Sign = "□", Status = "水珠联成线，但未渗出试件界面" };
            SMSLType tempSLType3 = new SMSLType() { TypeID = 3, Sign = "△", Status = "局部少量喷溅" };
            SMSLType tempSLType4 = new SMSLType() { TypeID = 4, Sign = "▲", Status = "持续喷溅出试件界面" };
            SMSLType tempSLType5 = new SMSLType() { TypeID = 5, Sign = "●", Status = "持续流出试件界面" };
            SMSLType tempSLType6 = new SMSLType() { TypeID = 6, Sign = "未检测", Status = "未做检测" };
            SLTypeListSM.Add(tempSLType0);
            SLTypeListSM.Add(tempSLType1);
            SLTypeListSM.Add(tempSLType2);
            SLTypeListSM.Add(tempSLType3);
            SLTypeListSM.Add(tempSLType4);
            SLTypeListSM.Add(tempSLType5);
            SLTypeListSM.Add(tempSLType6);
        }


        #region 检测测试数据

        /// <summary>
        /// 水密定级检测可开启部分渗漏情况组
        /// </summary>
        private ObservableCollection<int> _slStatus_DJ_KKQ = new ObservableCollection<int>(){0,0,0,0,0,0,0,0};
        /// <summary>
        /// 水密定级检测可开启部分渗漏情况组
        /// </summary>
        public ObservableCollection<int> SLStatus_DJ_KKQ
        {
            get { return _slStatus_DJ_KKQ; }
            set
            {
                _slStatus_DJ_KKQ = value;
                RaisePropertyChanged(() => SLStatus_DJ_KKQ);
            }
        }

        /// <summary>
        /// 水密定级检测固定部分渗漏情况组
        /// </summary>
        private ObservableCollection<int> _slStatus_DJ_GD = new ObservableCollection<int>() { 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// 水密定级检测固定部分渗漏情况组
        /// </summary>
        public ObservableCollection<int> SLStatus_DJ_GD
        {
            get { return _slStatus_DJ_GD; }
            set
            {
                _slStatus_DJ_GD = value;
                RaisePropertyChanged(() => SLStatus_DJ_GD);
            }
        }

        /// <summary>
        /// 水密定级检测压力组
        /// </summary>
        /// <remarks>稳定加压或波动平均压力</remarks>
        private ObservableCollection<double> _press_DJ = new ObservableCollection<double>() { 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// 水密定级检测压力组
        /// </summary>
        /// <remarks>稳定加压或波动平均压力</remarks>
        public ObservableCollection<double> Press_DJ
        {
            get { return _press_DJ; }
            set
            {
                _press_DJ = value;
                RaisePropertyChanged(() => Press_DJ);
            }
        }
        
        /// <summary>
        /// 水密定级检测可开启部分渗漏说明
        /// </summary>
        private ObservableCollection<string> _slPS_DJ_KKQ = new ObservableCollection<string>() { "", "", "", "", "", "", "", "" };
        /// <summary>
        /// 水密定级检测可开启部分渗漏说明
        /// </summary>
        public ObservableCollection<string> SLPS_DJ_KKQ
        {
            get { return _slPS_DJ_KKQ; }
            set
            {
                _slPS_DJ_KKQ = value;
                RaisePropertyChanged(() => SLPS_DJ_KKQ);
            }
        }

        /// <summary>
        /// 水密定级检测固定部分渗漏说明
        /// </summary>
        private ObservableCollection<string> _slPS_DJ_GD = new ObservableCollection<string>() { "", "", "", "", "", "", "", "" };
        /// <summary>
        /// 水密定级检测固定部分渗漏说明
        /// </summary>
        public ObservableCollection<string> SLPS_DJ_GD
        {
            get { return _slPS_DJ_GD; }
            set
            {
                _slPS_DJ_GD = value;
                RaisePropertyChanged(() => SLPS_DJ_GD);
            }
        }

        /// <summary>
        /// 水密工程检测压力组
        /// </summary>
        /// <remarks>稳定加压或波动平均压力</remarks>
        private ObservableCollection<double> _press_GC = new ObservableCollection<double>() { 0, 0};
        /// <summary>
        /// 水密工程检测压力组
        /// </summary>
        /// <remarks>稳定加压或波动平均压力</remarks>
        public ObservableCollection<double> Press_GC
        {
            get { return _press_GC; }
            set
            {
                _press_GC = value;
                RaisePropertyChanged(() => Press_GC);
            }
        }

        /// <summary>
        /// 水密工程检测可开启部分渗漏情况
        /// </summary>
        private ObservableCollection<int> _slStatus_GC_KKQ = new ObservableCollection<int>() { 0 };
        /// <summary>
        /// 水密工程检测可开启部分渗漏情况
        /// </summary>
        public ObservableCollection<int> SLStatus_GC_KKQ
        {
            get { return _slStatus_GC_KKQ; }
            set
            {
                _slStatus_GC_KKQ = value;
                RaisePropertyChanged(() => SLStatus_GC_KKQ);
            }
        }

        /// <summary>
        /// 水密工程检测固定部分渗漏情况
        /// </summary>
        private ObservableCollection<int> _slStatus_GC_GD = new ObservableCollection<int>() { 0 };
        /// <summary>
        /// 水密工程检测固定部分渗漏情况
        /// </summary>
        public ObservableCollection<int> SLStatus_GC_GD
        {
            get { return _slStatus_GC_GD; }
            set
            {
                _slStatus_GC_GD = value;
                RaisePropertyChanged(() => SLStatus_GC_GD);
            }
        }
        /// <summary>
        /// 水密工程检测可开启部分渗漏说明
        /// </summary>
        private ObservableCollection<string> _slPS_GC_KKQ = new ObservableCollection<string>() {"" };
        /// <summary>
        /// 水密工程检测可开启部分渗漏说明
        /// </summary>
        public ObservableCollection<string> SLPS_GC_KKQ
        {
            get { return _slPS_GC_KKQ; }
            set
            {
                _slPS_GC_KKQ = value;
                RaisePropertyChanged(() => SLPS_GC_KKQ);
            }
        }

        /// <summary>
        /// 水密工程检测固定部分渗漏说明
        /// </summary>
        private ObservableCollection<string> _slPS_GC_GD = new ObservableCollection<string>() { "" };
        /// <summary>
        /// 水密工程检测固定部分渗漏说明
        /// </summary>
        public ObservableCollection<string> SLPS_GC_GD
        {
            get { return _slPS_GC_GD; }
            set
            {
                _slPS_GC_GD = value;
                RaisePropertyChanged(() => SLPS_GC_GD);
            }
        }

        #endregion


        #region 定级评定数据

        /// <summary>
        /// 定级检测最大未严重渗漏压力-可开启部分
        /// </summary>
        private double _maxPressWYZSL_DJ_KKQ = 0;
        /// <summary>
        /// 定级检测最大未严重渗漏压力-可开启部分
        /// </summary>
        public double MaxPressWYZSL_DJ_KKQ
        {
            get { return _maxPressWYZSL_DJ_KKQ; }
            set
            {
                _maxPressWYZSL_DJ_KKQ = value;
                RaisePropertyChanged(() => MaxPressWYZSL_DJ_KKQ);
            }
        }

        /// <summary>
        /// 定级检测最大未严重渗漏压力-固定部分
        /// </summary>
        private double _maxPressWYZSL_DJ_GD = 0;
        /// <summary>
        /// 定级检测最大未严重渗漏压力-固定部分
        /// </summary>
        public double MaxPressWYZSL_DJ_GD
        {
            get { return _maxPressWYZSL_DJ_GD; }
            set
            {
                _maxPressWYZSL_DJ_GD = value;
                RaisePropertyChanged(() => MaxPressWYZSL_DJ_GD);
            }
        }

        /// <summary>
        /// 定级检测可开启部分评定等级
        /// </summary>
        private double _level_DJ_KKQ = 0;
        /// <summary>
        /// 定级检测可开启部分评定等级
        /// </summary>
        public double Level_DJ_KKQ
        {
            get { return _level_DJ_KKQ; }
            set
            {
                _level_DJ_KKQ = value;
                RaisePropertyChanged(() => Level_DJ_KKQ);
            }
        }

        /// <summary>
        /// 定级检测固定部分评定等级
        /// </summary>
        private double _level_DJ_GD = 0;
        /// <summary>
        /// 定级检测固定部分评定等级
        /// </summary>
        public double Level_DJ_GD
        {
            get { return _level_DJ_GD; }
            set
            {
                _level_DJ_GD = value;
                RaisePropertyChanged(() => Level_DJ_GD);
            }
        }

        /// <summary>
        /// 定级检测可开启部分最后渗漏情况
        /// </summary>
        private int _slStatusFinal_DJ_KKQ=0;
        /// <summary>
        /// 定级检测可开启部分最后渗漏情况
        /// </summary>
        public int SLStatusFinal_DJ_KKQ
        {
            get { return _slStatusFinal_DJ_KKQ; }
            set
            {
                _slStatusFinal_DJ_KKQ = value;
                RaisePropertyChanged(() => SLStatusFinal_DJ_KKQ);
            }
        }

        /// <summary>
        /// 定级检测固定部分最后渗漏情况
        /// </summary>
        private int _slStatusFinal_DJ_GD = 0 ;
        /// <summary>
        /// 定级检测固定部分最后渗漏情况
        /// </summary>
        public int SLStatusFinal_DJ_GD
        {
            get { return _slStatusFinal_DJ_GD; }
            set
            {
                _slStatusFinal_DJ_GD = value;
                RaisePropertyChanged(() => SLStatusFinal_DJ_GD);
            }
        }

        /// <summary>
        /// 定级检测可开启部分最后渗漏说明
        /// </summary>
        private String slPSFinal_DJ_KKQ = "无渗漏";
        /// <summary>
        /// 定级检测可开启部分最后渗漏说明
        /// </summary>
        public string SLPSFinal_DJ_KKQ
        {
            get { return slPSFinal_DJ_KKQ; }
            set
            {
                slPSFinal_DJ_KKQ = value;
                RaisePropertyChanged(() => SLPSFinal_DJ_KKQ);
            }
        }

        /// <summary>
        /// 定级检测固定部分最后渗漏说明
        /// </summary>
        private String slPSFinal_DJ_GD = "无渗漏";
        /// <summary>
        /// 定级检测固定部分最后渗漏说明
        /// </summary>
        public string SLPSFinal_DJ_GD
        {
            get { return slPSFinal_DJ_GD; }
            set
            {
                slPSFinal_DJ_GD = value;
                RaisePropertyChanged(() => SLPSFinal_DJ_GD);
            }
        }

        #endregion


        #region 工程评定数据

        /// <summary>
        /// 工程检测是否满足设计-可开启部分
        /// </summary>
        private bool _isMeetDesign_GC_KKQ = true;
        /// <summary>
        /// 工程检测是否满足设计-可开启部分
        /// </summary>
        public bool IsMeetDesign_GC_KKQ
        {
            get { return _isMeetDesign_GC_KKQ; }
            set
            {
                _isMeetDesign_GC_KKQ = value;
                RaisePropertyChanged(() => IsMeetDesign_GC_KKQ);
            }
        }

        /// <summary>
        /// 工程检测是否满足设计-固定部分
        /// </summary>
        private bool _isMeetDesign_GC_GD = true;
        /// <summary>
        /// 工程检测是否满足设计-固定部分
        /// </summary>
        public bool IsMeetDesign_GC_GD
        {
            get { return _isMeetDesign_GC_GD; }
            set
            {
                _isMeetDesign_GC_GD = value;
                RaisePropertyChanged(() => IsMeetDesign_GC_GD);
            }
        }

        /// <summary>
        /// 工程检测整体是否满足设计要求
        /// </summary>
        private bool _isMeetDesign_GC_All = true;
        /// <summary>
        /// 工程检测整体是否满足设计要求
        /// </summary>
        public bool IsMeetDesign_GC_All
        {
            get { return _isMeetDesign_GC_All; }
            set
            {
                _isMeetDesign_GC_All = value;
                RaisePropertyChanged(() => IsMeetDesign_GC_All);
            }
        }

        #endregion


        #region 临时用数据及方法
        
        /// <summary>
        /// 水密定级检测压力暂存组
        /// </summary>
        /// <remarks>稳定加压或波动平均压力</remarks>
        private ObservableCollection<double> _press_DJ_temp = new ObservableCollection<double>() { 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// 水密定级检测压力暂存组
        /// </summary>
        /// <remarks>稳定加压或波动平均压力</remarks>
        public ObservableCollection<double> Press_DJ_temp
        {
            get { return _press_DJ_temp; }
            set
            {
                _press_DJ_temp = value;
                RaisePropertyChanged(() => Press_DJ_temp);
            }
        }

        /// <summary>
        /// 定级检测可开启部分损坏情况暂存数组
        /// </summary>
        private ObservableCollection<int> _SLStatus_DJ_KKQ_temp = new ObservableCollection<int>() { 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// 定级检测可开启部分损坏情况暂存数组
        /// </summary>
        public ObservableCollection<int> SLStatus_DJ_KKQ_temp
        {
            get { return _SLStatus_DJ_KKQ_temp; }
            set
            {
                _SLStatus_DJ_KKQ_temp = value;
                RaisePropertyChanged(() => SLStatus_DJ_KKQ_temp);
            }
        }

        /// <summary>
        /// 定级检测固定部分损坏情况暂存数组
        /// </summary>
        private ObservableCollection<int> _SLStatus_DJ_GD_temp = new ObservableCollection<int>() { 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// 定级检测固定部分损坏情况暂存数组
        /// </summary>
        public ObservableCollection<int> SLStatus_DJ_GD_temp
        {
            get { return _SLStatus_DJ_GD_temp; }
            set
            {
                _SLStatus_DJ_GD_temp = value;
                RaisePropertyChanged(() => _SLStatus_DJ_GD_temp);
            }
        }

        /// <summary>
        /// 工程检测可开启部分损坏情况暂存数组
        /// </summary>
        private ObservableCollection<int> _SLStatus_GC_KKQ_temp = new ObservableCollection<int>() { 0 };
        /// <summary>
        /// 工程检测可开启部分损坏情况暂存数组
        /// </summary>
        public ObservableCollection<int> SLStatus_GC_KKQ_temp
        {
            get { return _SLStatus_GC_KKQ_temp; }
            set
            {
                _SLStatus_GC_KKQ_temp = value;
                RaisePropertyChanged(() => SLStatus_GC_KKQ_temp);
            }
        }

        /// <summary>
        /// 工程检测固定部分损坏情况暂存数组
        /// </summary>
        private ObservableCollection<int> _slStatus_GC_GD_temp = new ObservableCollection<int>() { 0 };
        /// <summary>
        /// 工程检测固定部分损坏情况暂存数组
        /// </summary>
        public ObservableCollection<int> SLStatus_GC_GD_temp
        {
            get { return _slStatus_GC_GD_temp; }
            set
            {
                _slStatus_GC_GD_temp = value;
                RaisePropertyChanged(() => SLStatus_GC_GD_temp);
            }
        }


        /// <summary>
        /// 水密定级检测可开启部分损坏说明暂存
        /// </summary>
        private ObservableCollection<string> _slPS_DJ_KKQ_temp = new ObservableCollection<string>() { "", "", "", "", "", "", "", "" };
        /// <summary>
        /// 水密定级检测可开启部分损坏说明暂存
        /// </summary>
        public ObservableCollection<string> SLPS_DJ_KKQ_temp
        {
            get { return _slPS_DJ_KKQ_temp; }
            set
            {
                _slPS_DJ_KKQ_temp = value;
                RaisePropertyChanged(() => SLPS_DJ_KKQ_temp);
            }
        }

        /// <summary>
        /// 水密定级检测固定部分损坏说明暂存
        /// </summary>
        private ObservableCollection<string> _slPS_DJ_GD_temp = new ObservableCollection<string>() { "", "", "", "", "", "", "", "" };
        /// <summary>
        /// 水密定级检测固定部分损坏说明暂存
        /// </summary>
        public ObservableCollection<string> SLPS_DJ_GD_temp
        {
            get { return _slPS_DJ_GD_temp; }
            set
            {
                _slPS_DJ_GD_temp = value;
                RaisePropertyChanged(() => SLPS_DJ_GD_temp);
            }
        }

        /// <summary>
        /// 水密工程检测可开启部分损坏说明暂存
        /// </summary>
        private ObservableCollection<string> _slPS_GC_KKQ_temp = new ObservableCollection<string>() { "" };
        /// <summary>
        /// 水密工程检测可开启部分损坏说明暂存
        /// </summary>
        public ObservableCollection<string> SLPS_GC_KKQ_temp
        {
            get { return _slPS_GC_KKQ_temp; }
            set
            {
                _slPS_GC_KKQ_temp = value;
                RaisePropertyChanged(() => SLPS_GC_KKQ_temp);
            }
        }

        /// <summary>
        /// 水密工程检测固定部分损坏说明暂存
        /// </summary>
        private ObservableCollection<string> _slPS_GC_GD_temp = new ObservableCollection<string>() { "" };
        /// <summary>
        /// 水密工程检测固定部分损坏说明暂存
        /// </summary>
        public ObservableCollection<string> SLPS_GC_GD_temp
        {
            get { return _slPS_GC_GD_temp; }
            set
            {
                _slPS_GC_GD_temp = value;
                RaisePropertyChanged(() => SLPS_GC_GD_temp);
            }
        }

        /// <summary>
        /// 损坏情况暂存
        /// </summary>
        public void SLStatusCopy()
        {
            //定级损坏情况
            for (int i = 0; i < SLStatus_DJ_KKQ.Count; i++)
            {
                SLStatus_DJ_KKQ_temp[i] = SLStatus_DJ_KKQ[i];
            }
            for (int i = 0; i < SLStatus_DJ_GD.Count; i++)
            {
                SLStatus_DJ_GD_temp[i] = SLStatus_DJ_GD[i];
            }

            //定级损坏说明
            for (int i = 0; i < SLPS_DJ_KKQ.Count; i++)
            {
                SLPS_DJ_KKQ_temp[i] = SLPS_DJ_KKQ[i].Clone().ToString();
            }
            for (int i = 0; i < SLPS_DJ_GD.Count; i++)
            {
                SLPS_DJ_GD_temp[i] = SLPS_DJ_GD[i].Clone().ToString();
            }

            //工程损坏情况
            for (int i = 0; i < SLStatus_GC_KKQ.Count; i++)
            {
                SLStatus_GC_KKQ_temp[i] = SLStatus_GC_KKQ[i];
            }
            for (int i = 0; i < SLStatus_GC_GD.Count; i++)
            {
                SLStatus_GC_GD_temp[i] = SLStatus_GC_GD[i];
            }

            //工程损坏说明
            for (int i = 0; i < SLPS_GC_KKQ.Count; i++)
            {
                SLPS_GC_KKQ_temp[i] = SLPS_GC_KKQ[i].Clone().ToString();
            }
            for (int i = 0; i < SLPS_GC_GD.Count; i++)
            {
                SLPS_GC_GD_temp[i] = SLPS_GC_GD[i].Clone().ToString();
            }
        }

        /// <summary>
        /// 损坏情况暂存值回存
        /// </summary>
        public void SLStatusCopyBack()
        {
            //定级损坏情况
            for (int i = 0; i < SLStatus_DJ_KKQ.Count; i++)
            {
                SLStatus_DJ_KKQ[i] = SLStatus_DJ_KKQ_temp[i];
            }
            for (int i = 0; i < SLStatus_DJ_GD.Count; i++)
            {
                SLStatus_DJ_GD[i] = SLStatus_DJ_GD_temp[i];
            }

            //定级损坏说明
            for (int i = 0; i < SLPS_DJ_KKQ.Count; i++)
            {
                SLPS_DJ_KKQ[i] = SLPS_DJ_KKQ_temp[i].Clone().ToString();
            }
            for (int i = 0; i < SLPS_DJ_GD.Count; i++)
            {
                SLPS_DJ_GD[i] = SLPS_DJ_GD_temp[i].Clone().ToString();
            }
            
            //工程损坏情况
            for (int i = 0; i < SLStatus_GC_KKQ.Count; i++)
            {
                SLStatus_GC_KKQ[i] = SLStatus_GC_KKQ_temp[i];
            }
            for (int i = 0; i < SLStatus_GC_GD.Count; i++)
            {
                SLStatus_GC_GD[i] = SLStatus_GC_GD_temp[i];
            }

            //工程损坏说明
            for (int i = 0; i < SLPS_GC_KKQ.Count; i++)
            {
                SLPS_GC_KKQ[i] = SLPS_GC_KKQ_temp[i].Clone().ToString();
            }
            for (int i = 0; i < SLPS_GC_GD.Count; i++)
            {
                SLPS_GC_GD[i] = SLPS_GC_GD_temp[i].Clone().ToString();
            }

        }

        /// <summary>
        /// 水密损坏情况类别（选择用）
        /// </summary>
        private List< SMSLType> _slTypeListSM;
        /// <summary>
        /// 水密损坏情况类别（选择用）
        /// </summary>
        public List< SMSLType> SLTypeListSM
        {
            get { return _slTypeListSM; }
            set
            {
                _slTypeListSM = value;
                RaisePropertyChanged(() => SLTypeListSM);
            }
        }

        #endregion

    }


    /// <summary>
    /// 水密检测损坏情况类
    /// </summary>
    public class SMSLType : ObservableObject
    {
        /// <summary>
        /// 损坏类型ID
        /// </summary>
        private int _typeId = 0;
        /// <summary>
        /// 损坏类型ID
        /// </summary>
        public int TypeID
        {
            get { return _typeId; }
            set
            {
                _typeId = value;
                RaisePropertyChanged(() => TypeID);
            }
        }

        /// <summary>
        /// 符号
        /// </summary>
        private string  _sign = "";
        /// <summary>
        /// 符号
        /// </summary>
        public string Sign
        {
            get { return _sign; }
            set
            {
                _sign = value;
                RaisePropertyChanged(() => Sign);
            }
        }
        
        /// <summary>
        /// 渗漏状态
        /// </summary>
        private string _status = "";
        /// <summary>
        /// 渗漏状态
        /// </summary>
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged(() => Status);
            }
        }
    }
}