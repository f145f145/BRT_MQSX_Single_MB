/************************************************************************************
 * 描述：
 *
 * ==================================================================================
 * 修改标记
 * 修改时间			        修改人			版本号			描述
 * 2022/2/8 15:10:12		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using GalaSoft.MvvmLight;
using Lucene.Net.Support;
using MQZHWL.Model.DEV;
using NPOI.SS.Formula.Functions;
using static MQZHWL.Model.MQZH_Enums;

namespace MQZHWL.DAL
{
    /// <summary>
    /// 装置参数读写操作类
    /// </summary>
    public partial class MQZH_DevDAL : ObservableObject
    {
        #region 装置数据保存

        /// <summary>
        /// 保存装置设置参数
        /// </summary>
        private void SaveDevSettings()
        {
            try
            {
            //装置基本信息C01
            SaveDevBisicInfo();
            //装置基本信息C02
            SaveDevBisicParam();
            //C03公司信息
            SaveCoInfo();
            //C04串口设置参数表
            SaveDevComSettings();
            //C05模拟量传感器参数
            SaveAIOParam();
            //C06模拟量通道参数
            SaveAIOChannels();
            //C09PID控制参数
            SaveDevPIDSettings();
            //C12、13压力设定参数
            SavePressSettings();
            //C14压力控制参数
            SavePressCtlSettings();
            //C15位移设定参数
            SaveDevWYSettings();
            //C16位移控制参数
            SaveDevWYCtlSettings();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// 保存装置基本信息C01
        /// </summary>
        private void SaveDevBisicInfo()
        {
            string tempSettingsNO = "Using";

            //保存C01装置基本参数
            //若编号在表中不存在，则新建（拷贝DefaultSetting）
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C01装置基本信息Row checkExistC01Row = C01Table.FindBy配置编号(tempSettingsNO);
                if (checkExistC01Row == null)
                {
                    MQZHWL.MQZH_DB_DevDataSet.C01装置基本信息Row defDevC01Row = C01Table.FindBy配置编号("DefaultSetting");
                    MQZHWL.MQZH_DB_DevDataSet.C01装置基本信息Row newDevC01Row = C01Table.NewC01装置基本信息Row();
                    newDevC01Row.ItemArray = (object[])defDevC01Row.ItemArray.Clone();
                    newDevC01Row.配置编号 = tempSettingsNO;
                    C01Table.AddC01装置基本信息Row(newDevC01Row);
                    C01TableAdapter.Update(C01Table);
                    C01Table.AcceptChanges();
                    RaisePropertyChanged(() => C01Table);
                    MessageBox.Show("未找到在用装置基本信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }   

            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C01装置基本信息Row devC01Row = C01Table.FindBy配置编号(tempSettingsNO);
                if (devC01Row != null)
                {
                    devC01Row.装置ID = DAL_Dev.DeviceID;
                    devC01Row.装置名称 = DAL_Dev.DeviceName;
                    devC01Row.出厂编号 = DAL_Dev.DeviceSerialNO;
                    devC01Row.设备型号 = DAL_Dev.DeviceTypeNO;
                    devC01Row.抗风压试验方法标准 = DAL_Dev.DeviceKFYStd;
                    devC01Row.气密试验方法标准 = DAL_Dev.DeviceQMStd;
                    devC01Row.水密试验方法标准 = DAL_Dev.DeviceSMStd;
                    devC01Row.层间变形试验方法标准 = DAL_Dev.DeviceCJBXStd;

                    C01TableAdapter.Update(C01Table);
                    C01Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// C02装置基本参数Row
        /// </summary>
        private void SaveDevBisicParam()
        {
            string tempSettingsNO = "Using";

            //C02装置基本参数Row
            //若编号在表中不存在，则新建（拷贝DefaultSetting）
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C02装置基本参数Row checkExistC02Row = C02Table.FindBy配置编号(tempSettingsNO);
                if (checkExistC02Row == null)
                {
                    MQZHWL.MQZH_DB_DevDataSet.C02装置基本参数Row defDevC02Row = C02Table.FindBy配置编号("DefaultSetting");
                    MQZHWL.MQZH_DB_DevDataSet.C02装置基本参数Row newDevC02Row = C02Table.NewC02装置基本参数Row();
                    newDevC02Row.ItemArray = (object[])defDevC02Row.ItemArray.Clone();
                    newDevC02Row.配置编号 = tempSettingsNO;
                    C02Table.AddC02装置基本参数Row(newDevC02Row);
                    C02TableAdapter.Update(C02Table);
                    C02Table.AcceptChanges();
                    RaisePropertyChanged(() => C02Table);
                    MessageBox.Show("未找到在用装置设定参数，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C02装置基本参数Row devC02Row = C02Table.FindBy配置编号(tempSettingsNO);
                if (devC02Row != null)
                {
                    devC02Row.绘图更新周期 = DAL_Dev.PlotPeriod;
                    devC02Row.单曲线总点数 = DAL_Dev.PointsPerLine;
                    devC02Row.UseCSFG1 = DAL_Dev.UseCSFG1;
                    devC02Row.WithCSFG1 = DAL_Dev.WithCSFG1;
                    devC02Row.GJ_FG1 = DAL_Dev.GJ_FG1;
                    devC02Row.DFNo_FG1 = DAL_Dev.DFNo_FG1;
                    devC02Row.FSNo_FG1 = DAL_Dev.FSNo_FG1;
                    devC02Row.UseCSFG2 = DAL_Dev.UseCSFG2;
                    devC02Row.WithCSFG2 = DAL_Dev.WithCSFG2;
                    devC02Row.GJ_FG2 = DAL_Dev.GJ_FG2;
                    devC02Row.DFNo_FG2 = DAL_Dev.DFNo_FG2;
                    devC02Row.FSNo_FG2 = DAL_Dev.FSNo_FG2;
                    devC02Row.UseCSFG3 = DAL_Dev.UseCSFG3;
                    devC02Row.WithCSFG3 = DAL_Dev.WithCSFG3;
                    devC02Row.GJ_FG3 = DAL_Dev.GJ_FG3;
                    devC02Row.DFNo_FG3 = DAL_Dev.DFNo_FG3;
                    devC02Row.FSNo_FG3 = DAL_Dev.FSNo_FG3;
                    devC02Row.水管管径 = DAL_Dev.GJ_SG;
                    devC02Row.活动框高度 = DAL_Dev.H_HDK;
                    devC02Row.活动框宽度 = DAL_Dev.W_HDK;
                    devC02Row.是否使用三合一 = DAL_Dev.IsUseTHP;
                    devC02Row.THPType = DAL_Dev.THPType;
                    devC02Row.有电子流量计 = DAL_Dev.IsWithSLL;
                    devC02Row.有中压差传感器 = DAL_Dev.IsWithCYM;
                    devC02Row.位移尺数量 = DAL_Dev.WYQty;
                    devC02Row.X轴位移尺编号 = DAL_Dev.WYNOList[0];
                    devC02Row.Y轴位移尺编号左 = DAL_Dev.WYNOList[1];
                    devC02Row.Y轴位移尺编号中 = DAL_Dev.WYNOList[2];
                    devC02Row.Y轴位移尺编号右 = DAL_Dev.WYNOList[3];
                    devC02Row.Z轴位移尺编号左 = DAL_Dev.WYNOList[4];
                    devC02Row.Z轴位移尺编号中 = DAL_Dev.WYNOList[5];
                    devC02Row.Z轴位移尺编号右 = DAL_Dev.WYNOList[6];

                    devC02Row.最后试验编号 = DAL_Dev.ExpNOLast;
                    devC02Row.开机是否载入最后试验 = DAL_Dev.IsLoadLastExpPowerOn;

                    devC02Row.允许修改数据 = DAL_Dev.PermitChangeData;
                    devC02Row.抗风压位移取反 = DAL_Dev.IsWYKFYF;
                    devC02Row.波动低压准备频率 = DAL_Dev.PhBDZB;
                    devC02Row.层间变形类别 = DAL_Dev.CJBXType;

                    C02TableAdapter.Update(C02Table);
                    C02Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 保存公司信息C03
        /// </summary>
        private void SaveCoInfo()
        {
            string tempSettingsNO = "Using";

            //保存C03公司信息
            //若编号在表中不存在，则新建（拷贝DefaultSetting）
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C03公司信息Row checkExistC03Row = C03Table.FindBy配置编号(tempSettingsNO);
                if (checkExistC03Row == null)
                {
                    MQZHWL.MQZH_DB_DevDataSet.C03公司信息Row defDevC03Row = C03Table.FindBy配置编号("DefaultSetting");
                    MQZHWL.MQZH_DB_DevDataSet.C03公司信息Row newDevC03Row = C03Table.NewC03公司信息Row();
                    newDevC03Row.ItemArray = (object[])defDevC03Row.ItemArray.Clone();
                    newDevC03Row.配置编号 = tempSettingsNO;
                    C03Table.AddC03公司信息Row(newDevC03Row);
                    C03TableAdapter.Update(C03Table);
                    C03Table.AcceptChanges();
                    RaisePropertyChanged(() => C03Table);
                    MessageBox.Show("未找到公司信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C03公司信息Row devC03Row = C03Table.FindBy配置编号(tempSettingsNO);
                if (devC03Row != null)
                {
                    devC03Row.公司简称 = DAL_Dev.MQZH_CompanyShortName;
                    devC03Row.公司全称 = DAL_Dev.MQZH_CompanyName;
                    devC03Row.公司地址 = DAL_Dev.MQZH_CompanyAddr;
                    devC03Row.实验室地址 = DAL_Dev.MQZH_LabAddr;
                    devC03Row.公司邮政编码 = DAL_Dev.MQZH_ComPostNO;
                    devC03Row.实验室邮政编码 = DAL_Dev.MQZH_LabPostNO;
                    devC03Row.公司电话 = DAL_Dev.MQZH_CompanyTel;
                    devC03Row.实验室电话 = DAL_Dev.MQZH_LabTel;
                    C03TableAdapter.Update(C03Table);
                    C03Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 保存C04串口设置参数
        /// </summary>
        private void SaveDevComSettings()
        {
            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC04RowName = new string[4]
            {
                "DVPCom", "THPCom1", "THPCom2", "THPCom3"
            };
            for (int i = 0; i < tempC04RowName.Length; i++)
            {
                try
                {
                    MQZH_DB_DevDataSet.C04串口参数设置Row checkExistC04Row = C04Table.FindBy配置编号(tempC04RowName[i]);
                    if (checkExistC04Row == null)
                    {
                        MQZH_DB_DevDataSet.C04串口参数设置Row defDevC04Row = C04Table.FindBy配置编号("DefaultSetting");
                        MQZH_DB_DevDataSet.C04串口参数设置Row newDevC04ow = C04Table.NewC04串口参数设置Row();
                        newDevC04ow.ItemArray = (object[])defDevC04Row.ItemArray.Clone();
                        newDevC04ow.配置编号 = tempC04RowName[i];
                        C04Table.AddC04串口参数设置Row(newDevC04ow);
                        C04TableAdapter.Update(C04Table);
                        C04Table.AcceptChanges();
                        RaisePropertyChanged(() => C04Table);
                        MessageBox.Show("未找到串口配置参数" + tempC04RowName[i] + "，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            //DVPCom
            try
            {
                MQZH_DB_DevDataSet.C04串口参数设置Row devC04dvpRow = C04Table.FindBy配置编号(tempC04RowName[0]);
                if (devC04dvpRow != null)
                {
                    devC04dvpRow.PortNO = DAL_Dev.DVPCom.PhyPortNO;
                    devC04dvpRow.BaudRate = DAL_Dev.DVPCom.BoundRate;
                    devC04dvpRow.StartBits = DAL_Dev.DVPCom.StartBits;
                    devC04dvpRow.DataBits = DAL_Dev.DVPCom.DataBits;
                    devC04dvpRow.PlCAddr = DAL_Dev.DVPCom.Addr;
                    devC04dvpRow.CommRW_Period = DAL_Dev.DVPCom.PeriodRW;
                    devC04dvpRow.WatchDogReset_Period = DAL_Dev.DVPCom.WatchDogPeriod;
                    devC04dvpRow.Timeout = DAL_Dev.DVPCom.Timeout;
                    devC04dvpRow.Time_BusyDealy = DAL_Dev.DVPCom.Time_BusyDealy;
                    devC04dvpRow.CMDRepeat = DAL_Dev.DVPCom.CMDRepeat;
                    //除2外，默认为停止位1
                    if (DAL_Dev.DVPCom.StopBits == StopBits.One)
                        devC04dvpRow.StopBits = 1;
                    else if (DAL_Dev.DVPCom.StopBits == StopBits.Two)
                        devC04dvpRow.StopBits = 2;
                    else
                        devC04dvpRow.StopBits = 1;

                    if (DAL_Dev.DVPCom.Parity == Parity.None)
                        devC04dvpRow.Parity = 0;
                    else if (DAL_Dev.DVPCom.Parity == Parity.Odd)
                        devC04dvpRow.Parity = 1;
                    else if (DAL_Dev.DVPCom.Parity == Parity.Even)
                        devC04dvpRow.Parity = 2;
                    else if (DAL_Dev.DVPCom.Parity == Parity.Mark)
                        devC04dvpRow.Parity = 10;
                    else if (DAL_Dev.DVPCom.Parity == Parity.Space)
                        devC04dvpRow.Parity = 20;
                    C04TableAdapter.Update(C04Table);
                    C04Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //THPCom
            try
            {
                MQZH_DB_DevDataSet.C04串口参数设置Row devC04thpRow;
                switch (DAL_Dev.THPType)
                {
                    case 1:
                        devC04thpRow = C04Table.FindBy配置编号(tempC04RowName[1]);
                        break;

                    case 2:
                        devC04thpRow = C04Table.FindBy配置编号(tempC04RowName[2]);
                        break;

                    case 3:
                        devC04thpRow = C04Table.FindBy配置编号(tempC04RowName[3]);
                        break;

                    default:
                        devC04thpRow = C04Table.FindBy配置编号(tempC04RowName[1]);
                        break;
                }

                if (devC04thpRow != null)
                {
                    devC04thpRow.PortNO = DAL_Dev.THPCom.PhyPortNO;
                    devC04thpRow.BaudRate = DAL_Dev.THPCom.BoundRate;
                    devC04thpRow.StartBits = DAL_Dev.THPCom.StartBits;
                    devC04thpRow.DataBits = DAL_Dev.THPCom.DataBits;
                    devC04thpRow.PlCAddr = DAL_Dev.THPCom.Addr;
                    devC04thpRow.CommRW_Period = DAL_Dev.THPCom.PeriodRW;
                    devC04thpRow.WatchDogReset_Period = DAL_Dev.THPCom.WatchDogPeriod;
                    devC04thpRow.Timeout = DAL_Dev.THPCom.Timeout;
                    devC04thpRow.Time_BusyDealy = DAL_Dev.THPCom.Time_BusyDealy;
                    devC04thpRow.CMDRepeat = DAL_Dev.THPCom.CMDRepeat;

                    if (DAL_Dev.THPCom.StopBits == StopBits.None)
                        devC04thpRow.StopBits = 0;
                    else if (DAL_Dev.THPCom.StopBits == StopBits.One)
                        devC04thpRow.StopBits = 1;
                    else if (DAL_Dev.THPCom.StopBits == StopBits.Two)
                        devC04thpRow.StopBits = 2;
                    else if (DAL_Dev.THPCom.StopBits == StopBits.Two)
                        devC04thpRow.StopBits = 2;
                    else if (DAL_Dev.THPCom.StopBits == StopBits.OnePointFive)
                        devC04thpRow.StopBits = 15;

                    if (DAL_Dev.THPCom.Parity == Parity.None)
                        devC04thpRow.Parity = 0;
                    else if (DAL_Dev.THPCom.Parity == Parity.Odd)
                        devC04thpRow.Parity = 1;
                    else if (DAL_Dev.THPCom.Parity == Parity.Even)
                        devC04thpRow.Parity = 2;
                    else if (DAL_Dev.THPCom.Parity == Parity.Mark)
                        devC04thpRow.Parity = 10;
                    else if (DAL_Dev.THPCom.Parity == Parity.Space)
                        devC04thpRow.Parity = 20;
                    C04TableAdapter.Update(C04Table);
                    C04Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 保存C05模拟量参数
        /// </summary>
        private void SaveAIOParam()
        {
            #region 模拟量输入

            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC05AIINRowName = new string[24]
            {
                "WY01", "WY02", "WY03", "WY04", "WY05", "WY06", "WY07", "WY08", "WY09", "WY10", "WY11", "WY12", "CY01", "CY02", "CY03", "FS01", "FS02", "FS03", "SLS", "T_THP", "H_THP", "P_THP", "PL1View", "PL2View"
            };
            for (int i = 0; i < tempC05AIINRowName.Length; i++)
            {
                try
                {
                    MQZH_DB_DevDataSet.C05模拟量参数Row checkExistC05Row = C05Table.FindBySingalNO(tempC05AIINRowName[i]);
                    if (checkExistC05Row == null)
                    {
                        MQZH_DB_DevDataSet.C05模拟量参数Row defDevC05Row =
                            C05Table.FindBySingalNO(tempC05AIINRowName[i].ToString() + "Def");
                        MQZH_DB_DevDataSet.C05模拟量参数Row newDevC05ow = C05Table.NewC05模拟量参数Row();
                        newDevC05ow.ItemArray = (object[])defDevC05Row.ItemArray.Clone();
                        newDevC05ow.SingalNO = tempC05AIINRowName[i];
                        C05Table.AddC05模拟量参数Row(newDevC05ow);
                        C05TableAdapter.Update(C05Table);
                        C05Table.AcceptChanges();
                        RaisePropertyChanged(() => C05Table);
                        MessageBox.Show("未找到" + tempC05AIINRowName[i] + "配置参数，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            for (int i = 0; i < tempC05AIINRowName.Length; i++)
            {
                try
                {
                    MQZH_DB_DevDataSet.C05模拟量参数Row devC05aiRow = C05Table.FindBySingalNO(tempC05AIINRowName[i]);
                    if (devC05aiRow != null)
                    {
                        //基本参数
                        devC05aiRow.SingalUnit = DAL_Dev.AIList[i].SingalUnit;
                        devC05aiRow.IsOutType = DAL_Dev.AIList[i].IsOutType;
                        devC05aiRow.InfType = DAL_Dev.AIList[i].InfType;
                        devC05aiRow.ElecSig_Unit = DAL_Dev.AIList[i].ElecSig_Unit;
                        devC05aiRow.ElecSig_LowerRange = DAL_Dev.AIList[i].ElecSigLowerRange;
                        devC05aiRow.ElecSig_UpperRange = DAL_Dev.AIList[i].ElecSigUpperRange;
                        devC05aiRow.SingalLowerRange = DAL_Dev.AIList[i].SingalLowerRange;
                        devC05aiRow.SingalUpperRange = DAL_Dev.AIList[i].SingalUpperRange;
                        devC05aiRow.ZeroCalValue = DAL_Dev.AIList[i].ZeroCalValue;
                        devC05aiRow.KCalValue = DAL_Dev.AIList[i].KCalValue;
                        devC05aiRow.ModulNO = DAL_Dev.AIList[i].ModulNO;
                        if (DAL_Dev.AIList[i].ChannelSerialNO < 1)
                            DAL_Dev.AIList[i].ChannelSerialNO = 1;
                        devC05aiRow.ChannelSerialNO = DAL_Dev.AIList[i].ChannelSerialNO;
                        devC05aiRow.ConvRatio = DAL_Dev.AIList[i].ConvRatio;


                        DAL_Dev.AIList[i].CalPointsTemp = new ObservableCollection<CalPoint>()
                        {
                            new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),
                            new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint()
                        };
                        List<CalPoint> collectionForDel = new List<CalPoint>() 
                        {
                            new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),
                            new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint()
                        };
                        //标定点参数复制
                        for (int j = 0; j < 11; j++)
                        {
                            collectionForDel[j].IsUse = DAL_Dev.AIList[i].CalPoints[j].IsUse;
                            collectionForDel[j].StdValue = DAL_Dev.AIList[i].CalPoints[j].StdValue;
                            collectionForDel[j].ViewValue = DAL_Dev.AIList[i].CalPoints[j].ViewValue;
                        }
                        //未启用标定点的数值清零
                        for (int j = 0; j < collectionForDel.Count; j++)
                        {
                            if (!collectionForDel[j].IsUse)
                            {
                                collectionForDel[j].StdValue = 0;
                                collectionForDel[j].ViewValue = 0;
                            }
                        }
                        //去重、排序
                        List<CalPoint> collectionDeleted = new List<CalPoint>(collectionForDel.GroupBy(p => new { p.IsUse, p.StdValue }).Select(q => q.First()).ToList().OrderByDescending(v => v.IsUse).ThenBy(v => v.StdValue));
                        int usefulPoints = 0;
                        for (int index = 0; index < collectionDeleted.Count; index++)
                        {
                            DAL_Dev.AIList[i].CalPointsTemp[index].IsUse = collectionDeleted[index].IsUse;
                            DAL_Dev.AIList[i].CalPointsTemp[index].StdValue = collectionDeleted[index].StdValue;
                            DAL_Dev.AIList[i].CalPointsTemp[index].ViewValue = collectionDeleted[index].ViewValue;
                            if (DAL_Dev.AIList[i].CalPointsTemp[index].IsUse)
                                usefulPoints++;
                        }                        
                        //仅1个标定点时，设置为无标定点
                        if (usefulPoints == 1)
                        {
                            DAL_Dev.AIList[i].CalPointsTemp[0].IsUse = false;
                            DAL_Dev.AIList[i].CalPointsTemp[0].StdValue = 0;
                            DAL_Dev.AIList[i].CalPointsTemp[0].ViewValue = 0;
                        }
                        //标定点参数反向复制
                        for (int j = 0; j < 11; j++)
                        {
                            DAL_Dev.AIList[i].CalPoints[j].IsUse = DAL_Dev.AIList[i].CalPointsTemp[j].IsUse;
                            DAL_Dev.AIList[i].CalPoints[j].StdValue = DAL_Dev.AIList[i].CalPointsTemp[j].StdValue;
                            DAL_Dev.AIList[i].CalPoints[j].ViewValue = DAL_Dev.AIList[i].CalPointsTemp[j].ViewValue;
                        }
                        //标定点启用
                        devC05aiRow.Use_Corr1 = DAL_Dev.AIList[i].CalPoints[0].IsUse;
                        devC05aiRow.Use_Corr2 = DAL_Dev.AIList[i].CalPoints[1].IsUse;
                        devC05aiRow.Use_Corr3 = DAL_Dev.AIList[i].CalPoints[2].IsUse;
                        devC05aiRow.Use_Corr4 = DAL_Dev.AIList[i].CalPoints[3].IsUse;
                        devC05aiRow.Use_Corr5 = DAL_Dev.AIList[i].CalPoints[4].IsUse;
                        devC05aiRow.Use_Corr6 = DAL_Dev.AIList[i].CalPoints[5].IsUse;
                        devC05aiRow.Use_Corr7 = DAL_Dev.AIList[i].CalPoints[6].IsUse;
                        devC05aiRow.Use_Corr8 = DAL_Dev.AIList[i].CalPoints[7].IsUse;
                        devC05aiRow.Use_Corr9 = DAL_Dev.AIList[i].CalPoints[8].IsUse;
                        devC05aiRow.Use_Corr10 = DAL_Dev.AIList[i].CalPoints[9].IsUse;
                        devC05aiRow.Use_Corr11 = DAL_Dev.AIList[i].CalPoints[10].IsUse;
                        //标定点标准值
                        devC05aiRow.Sensor_Corr1_StdValue = DAL_Dev.AIList[i].CalPoints[0].StdValue;
                        devC05aiRow.Sensor_Corr2_StdValue = DAL_Dev.AIList[i].CalPoints[1].StdValue;
                        devC05aiRow.Sensor_Corr3_StdValue = DAL_Dev.AIList[i].CalPoints[2].StdValue;
                        devC05aiRow.Sensor_Corr4_StdValue = DAL_Dev.AIList[i].CalPoints[3].StdValue;
                        devC05aiRow.Sensor_Corr5_StdValue = DAL_Dev.AIList[i].CalPoints[4].StdValue;
                        devC05aiRow.Sensor_Corr6_StdValue = DAL_Dev.AIList[i].CalPoints[5].StdValue;
                        devC05aiRow.Sensor_Corr7_StdValue = DAL_Dev.AIList[i].CalPoints[6].StdValue;
                        devC05aiRow.Sensor_Corr8_StdValue = DAL_Dev.AIList[i].CalPoints[7].StdValue;
                        devC05aiRow.Sensor_Corr9_StdValue = DAL_Dev.AIList[i].CalPoints[8].StdValue;
                        devC05aiRow.Sensor_Corr10_StdValue = DAL_Dev.AIList[i].CalPoints[9].StdValue;
                        devC05aiRow.Sensor_Corr11_StdValue = DAL_Dev.AIList[i].CalPoints[10].StdValue;
                        //标定点显示值
                        devC05aiRow.Sensor_Corr1_ViewValue = DAL_Dev.AIList[i].CalPoints[0].ViewValue;
                        devC05aiRow.Sensor_Corr2_ViewValue = DAL_Dev.AIList[i].CalPoints[1].ViewValue;
                        devC05aiRow.Sensor_Corr3_ViewValue = DAL_Dev.AIList[i].CalPoints[2].ViewValue;
                        devC05aiRow.Sensor_Corr4_ViewValue = DAL_Dev.AIList[i].CalPoints[3].ViewValue;
                        devC05aiRow.Sensor_Corr5_ViewValue = DAL_Dev.AIList[i].CalPoints[4].ViewValue;
                        devC05aiRow.Sensor_Corr6_ViewValue = DAL_Dev.AIList[i].CalPoints[5].ViewValue;
                        devC05aiRow.Sensor_Corr7_ViewValue = DAL_Dev.AIList[i].CalPoints[6].ViewValue;
                        devC05aiRow.Sensor_Corr8_ViewValue = DAL_Dev.AIList[i].CalPoints[7].ViewValue;
                        devC05aiRow.Sensor_Corr9_ViewValue = DAL_Dev.AIList[i].CalPoints[8].ViewValue;
                        devC05aiRow.Sensor_Corr10_ViewValue = DAL_Dev.AIList[i].CalPoints[9].ViewValue;
                        devC05aiRow.Sensor_Corr11_ViewValue = DAL_Dev.AIList[i].CalPoints[10].ViewValue;

                        C05TableAdapter.Update(C05Table);
                        C05Table.AcceptChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            #endregion


            #region 模拟量输出

            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC05AORowName = new string[2]
            {
                "PL01", "PL02"
            };
            for (int i = 0; i < tempC05AORowName.Length; i++)
            {
                try
                {
                    MQZH_DB_DevDataSet.C05模拟量参数Row checkExistC05Row = C05Table.FindBySingalNO(tempC05AORowName[i]);
                    if (checkExistC05Row == null)
                    {
                        MQZH_DB_DevDataSet.C05模拟量参数Row defDevC05Row =
                            C05Table.FindBySingalNO(tempC05AORowName[i].ToString() + "Def");
                        MQZH_DB_DevDataSet.C05模拟量参数Row newDevC05ow = C05Table.NewC05模拟量参数Row();
                        newDevC05ow.ItemArray = (object[])defDevC05Row.ItemArray.Clone();
                        newDevC05ow.SingalNO = tempC05AORowName[i];
                        C05Table.AddC05模拟量参数Row(newDevC05ow);
                        C05TableAdapter.Update(C05Table);
                        C05Table.AcceptChanges();
                        RaisePropertyChanged(() => C05Table);
                        MessageBox.Show("未找到" + tempC05AORowName[i] + "配置参数，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            for (int i = 0; i < tempC05AORowName.Length; i++)
            {
                try
                {
                    MQZH_DB_DevDataSet.C05模拟量参数Row devC05aoRow = C05Table.FindBySingalNO(tempC05AORowName[i]);
                    if (devC05aoRow != null)
                    {
                        //基本参数
                        devC05aoRow.SingalUnit = DAL_Dev.AOList[i].SingalUnit;
                        devC05aoRow.IsOutType = DAL_Dev.AOList[i].IsOutType;
                        devC05aoRow.InfType = DAL_Dev.AOList[i].InfType;
                        devC05aoRow.ElecSig_Unit = DAL_Dev.AOList[i].ElecSig_Unit;
                        devC05aoRow.ElecSig_LowerRange = DAL_Dev.AOList[i].ElecSigLowerRange;
                        devC05aoRow.ElecSig_UpperRange = DAL_Dev.AOList[i].ElecSigUpperRange;
                        devC05aoRow.SingalLowerRange = DAL_Dev.AOList[i].SingalLowerRange;
                        devC05aoRow.SingalUpperRange = DAL_Dev.AOList[i].SingalUpperRange;
                        devC05aoRow.ZeroCalValue = DAL_Dev.AOList[i].ZeroCalValue;
                        devC05aoRow.KCalValue = DAL_Dev.AOList[i].KCalValue;
                        devC05aoRow.ModulNO = DAL_Dev.AOList[i].ModulNO;
                        devC05aoRow.ChannelSerialNO = DAL_Dev.AOList[i].ChannelSerialNO;
                        devC05aoRow.ConvRatio = DAL_Dev.AOList[i].ConvRatio;

                        //标定点参数复制
                        for (int j = 0; j < 11; j++)
                        {
                            DAL_Dev.AOList[i].CalPointsTemp[j].IsUse = DAL_Dev.AOList[i].CalPoints[j].IsUse;
                            DAL_Dev.AOList[i].CalPointsTemp[j].StdValue = DAL_Dev.AOList[i].CalPoints[j].StdValue;
                            DAL_Dev.AOList[i].CalPointsTemp[j].ViewValue = DAL_Dev.AOList[i].CalPoints[j].ViewValue;
                        }

                        int usefulPoints = 0;
                        //未启用标定点的数值清零
                        for (int j = 0; j < 11; j++)
                        {
                            if (!DAL_Dev.AOList[i].CalPointsTemp[j].IsUse)
                            {
                                DAL_Dev.AOList[i].CalPointsTemp[j].StdValue = 0;
                                DAL_Dev.AOList[i].CalPointsTemp[j].ViewValue = 0;
                            }
                            else
                                usefulPoints++;
                        }
                        //对标定点列表先按启用情况排序，后按viewValu排序
                        DAL_Dev.AOList[i].CalPoints = new ObservableCollection<CalPoint>(DAL_Dev.AOList[i].CalPointsTemp.OrderByDescending(v => v.IsUse).ThenBy(v => v.ViewValue));

                        //标定点启用
                        devC05aoRow.Use_Corr1 = DAL_Dev.AOList[i].CalPoints[0].IsUse;
                        devC05aoRow.Use_Corr2 = DAL_Dev.AOList[i].CalPoints[1].IsUse;
                        devC05aoRow.Use_Corr3 = DAL_Dev.AOList[i].CalPoints[2].IsUse;
                        devC05aoRow.Use_Corr4 = DAL_Dev.AOList[i].CalPoints[3].IsUse;
                        devC05aoRow.Use_Corr5 = DAL_Dev.AOList[i].CalPoints[4].IsUse;
                        devC05aoRow.Use_Corr6 = DAL_Dev.AOList[i].CalPoints[5].IsUse;
                        devC05aoRow.Use_Corr7 = DAL_Dev.AOList[i].CalPoints[6].IsUse;
                        devC05aoRow.Use_Corr8 = DAL_Dev.AOList[i].CalPoints[7].IsUse;
                        devC05aoRow.Use_Corr9 = DAL_Dev.AOList[i].CalPoints[8].IsUse;
                        devC05aoRow.Use_Corr10 = DAL_Dev.AOList[i].CalPoints[9].IsUse;
                        devC05aoRow.Use_Corr11 = DAL_Dev.AOList[i].CalPoints[10].IsUse;
                        //标定点标准值
                        devC05aoRow.Sensor_Corr1_StdValue = DAL_Dev.AOList[i].CalPoints[0].StdValue;
                        devC05aoRow.Sensor_Corr2_StdValue = DAL_Dev.AOList[i].CalPoints[1].StdValue;
                        devC05aoRow.Sensor_Corr3_StdValue = DAL_Dev.AOList[i].CalPoints[2].StdValue;
                        devC05aoRow.Sensor_Corr4_StdValue = DAL_Dev.AOList[i].CalPoints[3].StdValue;
                        devC05aoRow.Sensor_Corr5_StdValue = DAL_Dev.AOList[i].CalPoints[4].StdValue;
                        devC05aoRow.Sensor_Corr6_StdValue = DAL_Dev.AOList[i].CalPoints[5].StdValue;
                        devC05aoRow.Sensor_Corr7_StdValue = DAL_Dev.AOList[i].CalPoints[6].StdValue;
                        devC05aoRow.Sensor_Corr8_StdValue = DAL_Dev.AOList[i].CalPoints[7].StdValue;
                        devC05aoRow.Sensor_Corr9_StdValue = DAL_Dev.AOList[i].CalPoints[8].StdValue;
                        devC05aoRow.Sensor_Corr10_StdValue = DAL_Dev.AOList[i].CalPoints[9].StdValue;
                        devC05aoRow.Sensor_Corr11_StdValue = DAL_Dev.AOList[i].CalPoints[10].StdValue;
                        //标定点显示值
                        devC05aoRow.Sensor_Corr1_ViewValue = DAL_Dev.AOList[i].CalPoints[0].ViewValue;
                        devC05aoRow.Sensor_Corr2_ViewValue = DAL_Dev.AOList[i].CalPoints[1].ViewValue;
                        devC05aoRow.Sensor_Corr3_ViewValue = DAL_Dev.AOList[i].CalPoints[2].ViewValue;
                        devC05aoRow.Sensor_Corr4_ViewValue = DAL_Dev.AOList[i].CalPoints[3].ViewValue;
                        devC05aoRow.Sensor_Corr5_ViewValue = DAL_Dev.AOList[i].CalPoints[4].ViewValue;
                        devC05aoRow.Sensor_Corr6_ViewValue = DAL_Dev.AOList[i].CalPoints[5].ViewValue;
                        devC05aoRow.Sensor_Corr7_ViewValue = DAL_Dev.AOList[i].CalPoints[6].ViewValue;
                        devC05aoRow.Sensor_Corr8_ViewValue = DAL_Dev.AOList[i].CalPoints[7].ViewValue;
                        devC05aoRow.Sensor_Corr9_ViewValue = DAL_Dev.AOList[i].CalPoints[8].ViewValue;
                        devC05aoRow.Sensor_Corr10_ViewValue = DAL_Dev.AOList[i].CalPoints[9].ViewValue;
                        devC05aoRow.Sensor_Corr11_ViewValue = DAL_Dev.AOList[i].CalPoints[10].ViewValue;

                        C05TableAdapter.Update(C05Table);
                        C05Table.AcceptChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            #endregion
        }


        /// <summary>
        /// 保存C06模拟量通道参数
        /// </summary>
        private void SaveAIOChannels()
        {
            #region 04AD模块通道

            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC06ad1chlRowName = new string[24]
            {
                "04AD1-1", "04AD1-2", "04AD1-3", "04AD1-4", "04AD2-1", "04AD2-2", "04AD2-3", "04AD2-4", "04AD3-1", "04AD3-2", "04AD3-3", "04AD3-4", "04AD4-1", "04AD4-2", "04AD4-3", "04AD4-4", "04AD5-1", "04AD5-2", "04AD5-3", "04AD5-4", "DAView1", "DAView2", "DAView3", "DAView4"
            };
            for (int i = 0; i < tempC06ad1chlRowName.Length; i++)
            {
                try
                {
                    MQZH_DB_DevDataSet.C06模拟量通道参数Row checkExistC06Row = C06Table.FindByChannelNO(tempC06ad1chlRowName[i]);
                    if (checkExistC06Row == null)
                    {
                        MQZH_DB_DevDataSet.C06模拟量通道参数Row defDevC06Row = C06Table.FindByChannelNO(tempC06ad1chlRowName[i].ToString() + "Def");
                        MQZH_DB_DevDataSet.C06模拟量通道参数Row newDevC06ow = C06Table.NewC06模拟量通道参数Row();
                        newDevC06ow.ItemArray = (object[])defDevC06Row.ItemArray.Clone();
                        newDevC06ow.ChannelNO = tempC06ad1chlRowName[i];
                        C06Table.AddC06模拟量通道参数Row(newDevC06ow);
                        C06TableAdapter.Update(C06Table);
                        C06Table.AcceptChanges();
                        RaisePropertyChanged(() => C06Table);
                        MessageBox.Show("未找到" + tempC06ad1chlRowName[i] + "通道配置参数，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            //保存模拟量通道配置参数
            for (int i = 0; i < 4; i++)
            {
                //04AD1
                try
                {
                    MQZH_DB_DevDataSet.C06模拟量通道参数Row devC06chlRow = C06Table.FindByChannelNO(tempC06ad1chlRowName[i]);
                    if (devC06chlRow != null)
                    {
                        devC06chlRow.ChannelNO = DAL_Dev.Mod_04AD1.Channels[i].ChannelNO;
                        devC06chlRow.ColType = DAL_Dev.Mod_04AD1.Channels[i].InfType;
                        devC06chlRow.ElecSig_Unit = DAL_Dev.Mod_04AD1.Channels[i].ElecSigUnit;
                        devC06chlRow.ElecSig_LowerRange = DAL_Dev.Mod_04AD1.Channels[i].ElecSigLowerRange;
                        devC06chlRow.ElecSig_UpperRange = DAL_Dev.Mod_04AD1.Channels[i].ElecSigUpperRange;
                        devC06chlRow.Data_LowerRange = DAL_Dev.Mod_04AD1.Channels[i].DataLowerRange;
                        devC06chlRow.Data_UpperRange = DAL_Dev.Mod_04AD1.Channels[i].DataUpperRange;
                        devC06chlRow.IsUsed = DAL_Dev.Mod_04AD1.Channels[i].IsUsed;
                        devC06chlRow.IsOutType = DAL_Dev.Mod_04AD1.Channels[i].IsOutType;
                        devC06chlRow.FitRitio = DAL_Dev.Mod_04AD1.Channels[i].FitRatio;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //04AD2
                try
                {
                    MQZH_DB_DevDataSet.C06模拟量通道参数Row devC06chlRow = C06Table.FindByChannelNO(tempC06ad1chlRowName[i+4]);
                    if (devC06chlRow != null)
                    {
                        devC06chlRow.ChannelNO = DAL_Dev.Mod_04AD2.Channels[i].ChannelNO;
                        devC06chlRow.ColType = DAL_Dev.Mod_04AD2.Channels[i].InfType;
                        devC06chlRow.ElecSig_Unit = DAL_Dev.Mod_04AD2.Channels[i].ElecSigUnit;
                        devC06chlRow.ElecSig_LowerRange = DAL_Dev.Mod_04AD2.Channels[i].ElecSigLowerRange;
                        devC06chlRow.ElecSig_UpperRange = DAL_Dev.Mod_04AD2.Channels[i].ElecSigUpperRange;
                        devC06chlRow.Data_LowerRange = DAL_Dev.Mod_04AD2.Channels[i].DataLowerRange;
                        devC06chlRow.Data_UpperRange = DAL_Dev.Mod_04AD2.Channels[i].DataUpperRange;
                        devC06chlRow.IsUsed = DAL_Dev.Mod_04AD2.Channels[i].IsUsed;
                        devC06chlRow.IsOutType = DAL_Dev.Mod_04AD2.Channels[i].IsOutType;
                        devC06chlRow.FitRitio = DAL_Dev.Mod_04AD2.Channels[i].FitRatio;
                        C06TableAdapter.Update(C06Table);
                        C06Table.AcceptChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //04AD3
                try
                {
                    MQZH_DB_DevDataSet.C06模拟量通道参数Row devC06chlRow = C06Table.FindByChannelNO(tempC06ad1chlRowName[i+8]);
                    if (devC06chlRow != null)
                    {
                        devC06chlRow.ChannelNO = DAL_Dev.Mod_04AD3.Channels[i].ChannelNO;
                        devC06chlRow.ColType = DAL_Dev.Mod_04AD3.Channels[i].InfType;
                        devC06chlRow.ElecSig_Unit = DAL_Dev.Mod_04AD3.Channels[i].ElecSigUnit;
                        devC06chlRow.ElecSig_LowerRange = DAL_Dev.Mod_04AD3.Channels[i].ElecSigLowerRange;
                        devC06chlRow.ElecSig_UpperRange = DAL_Dev.Mod_04AD3.Channels[i].ElecSigUpperRange;
                        devC06chlRow.Data_LowerRange = DAL_Dev.Mod_04AD3.Channels[i].DataLowerRange;
                        devC06chlRow.Data_UpperRange = DAL_Dev.Mod_04AD3.Channels[i].DataUpperRange;
                        devC06chlRow.IsUsed = DAL_Dev.Mod_04AD3.Channels[i].IsUsed;
                        devC06chlRow.IsOutType = DAL_Dev.Mod_04AD3.Channels[i].IsOutType;
                        devC06chlRow.FitRitio = DAL_Dev.Mod_04AD3.Channels[i].FitRatio;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //04AD4
                try
                {
                    MQZH_DB_DevDataSet.C06模拟量通道参数Row devC06chlRow = C06Table.FindByChannelNO(tempC06ad1chlRowName[i+12]);
                    if (devC06chlRow != null)
                    {
                        devC06chlRow.ChannelNO = DAL_Dev.Mod_04AD4.Channels[i].ChannelNO;
                        devC06chlRow.ColType = DAL_Dev.Mod_04AD4.Channels[i].InfType;
                        devC06chlRow.ElecSig_Unit = DAL_Dev.Mod_04AD4.Channels[i].ElecSigUnit;
                        devC06chlRow.ElecSig_LowerRange = DAL_Dev.Mod_04AD4.Channels[i].ElecSigLowerRange;
                        devC06chlRow.ElecSig_UpperRange = DAL_Dev.Mod_04AD4.Channels[i].ElecSigUpperRange;
                        devC06chlRow.Data_LowerRange = DAL_Dev.Mod_04AD4.Channels[i].DataLowerRange;
                        devC06chlRow.Data_UpperRange = DAL_Dev.Mod_04AD4.Channels[i].DataUpperRange;
                        devC06chlRow.IsUsed = DAL_Dev.Mod_04AD4.Channels[i].IsUsed;
                        devC06chlRow.IsOutType = DAL_Dev.Mod_04AD4.Channels[i].IsOutType;
                        devC06chlRow.FitRitio = DAL_Dev.Mod_04AD4.Channels[i].FitRatio;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //04AD5
                try
                {
                    MQZH_DB_DevDataSet.C06模拟量通道参数Row devC06chlRow = C06Table.FindByChannelNO(tempC06ad1chlRowName[i + 16]);
                    if (devC06chlRow != null)
                    {
                        devC06chlRow.ChannelNO = DAL_Dev.Mod_04AD5.Channels[i].ChannelNO;
                        devC06chlRow.ColType = DAL_Dev.Mod_04AD5.Channels[i].InfType;
                        devC06chlRow.ElecSig_Unit = DAL_Dev.Mod_04AD5.Channels[i].ElecSigUnit;
                        devC06chlRow.ElecSig_LowerRange = DAL_Dev.Mod_04AD5.Channels[i].ElecSigLowerRange;
                        devC06chlRow.ElecSig_UpperRange = DAL_Dev.Mod_04AD5.Channels[i].ElecSigUpperRange;
                        devC06chlRow.Data_LowerRange = DAL_Dev.Mod_04AD5.Channels[i].DataLowerRange;
                        devC06chlRow.Data_UpperRange = DAL_Dev.Mod_04AD5.Channels[i].DataUpperRange;
                        devC06chlRow.IsUsed = DAL_Dev.Mod_04AD5.Channels[i].IsUsed;
                        devC06chlRow.IsOutType = DAL_Dev.Mod_04AD5.Channels[i].IsOutType;
                        devC06chlRow.FitRitio = DAL_Dev.Mod_04AD5.Channels[i].FitRatio;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //DAView
                try
                {
                    MQZH_DB_DevDataSet.C06模拟量通道参数Row devC06chlRow = C06Table.FindByChannelNO(tempC06ad1chlRowName[i + 20]);
                    if (devC06chlRow != null)
                    {
                        devC06chlRow.ChannelNO = DAL_Dev.Mod_DAView.Channels[i].ChannelNO;
                        devC06chlRow.ColType = DAL_Dev.Mod_DAView.Channels[i].InfType;
                        devC06chlRow.ElecSig_Unit = DAL_Dev.Mod_DAView.Channels[i].ElecSigUnit;
                        devC06chlRow.ElecSig_LowerRange = DAL_Dev.Mod_DAView.Channels[i].ElecSigLowerRange;
                        devC06chlRow.ElecSig_UpperRange = DAL_Dev.Mod_DAView.Channels[i].ElecSigUpperRange;
                        devC06chlRow.Data_LowerRange = DAL_Dev.Mod_DAView.Channels[i].DataLowerRange;
                        devC06chlRow.Data_UpperRange = DAL_Dev.Mod_DAView.Channels[i].DataUpperRange;
                        devC06chlRow.IsUsed = DAL_Dev.Mod_DAView.Channels[i].IsUsed;
                        devC06chlRow.IsOutType = DAL_Dev.Mod_DAView.Channels[i].IsOutType;
                        devC06chlRow.FitRitio = DAL_Dev.Mod_DAView.Channels[i].FitRatio;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            try
            {
                C06TableAdapter.Update(C06Table);
                C06Table.AcceptChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            #endregion

            #region 04DA模块通道

            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC06da1chlRowName = new string[4]
            {
                "DA-1", "DA-2", "DA-3", "DA-4"
            };
            for (int i = 0; i < tempC06da1chlRowName.Length; i++)
            {
                try
                {
                    MQZH_DB_DevDataSet.C06模拟量通道参数Row checkExistC06Row = C06Table.FindByChannelNO(tempC06da1chlRowName[i]);
                    if (checkExistC06Row == null)
                    {
                        MQZH_DB_DevDataSet.C06模拟量通道参数Row defDevC06Row = C06Table.FindByChannelNO(tempC06da1chlRowName[i].ToString() + "Def");
                        MQZH_DB_DevDataSet.C06模拟量通道参数Row newDevC06ow = C06Table.NewC06模拟量通道参数Row();
                        newDevC06ow.ItemArray = (object[])defDevC06Row.ItemArray.Clone();
                        newDevC06ow.ChannelNO = tempC06da1chlRowName[i];
                        C06Table.AddC06模拟量通道参数Row(newDevC06ow);
                        C06TableAdapter.Update(C06Table);
                        C06Table.AcceptChanges();
                        RaisePropertyChanged(() => C06Table);
                        MessageBox.Show("未找到" + tempC06da1chlRowName[i] + "通道配置参数，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            //保存模拟量通道配置参数
            for (int i = 0; i < tempC06da1chlRowName.Length; i++)
            {
                try
                {
                    MQZH_DB_DevDataSet.C06模拟量通道参数Row devC06chlRow = C06Table.FindByChannelNO(tempC06da1chlRowName[i]);
                    if (devC06chlRow != null)
                    {
                        devC06chlRow.ChannelNO = DAL_Dev.Mod_DA.Channels[i].ChannelNO;
                        devC06chlRow.ColType = DAL_Dev.Mod_DA.Channels[i].InfType;
                        devC06chlRow.ElecSig_Unit = DAL_Dev.Mod_DA.Channels[i].ElecSigUnit;
                        devC06chlRow.ElecSig_LowerRange = DAL_Dev.Mod_DA.Channels[i].ElecSigLowerRange;
                        devC06chlRow.ElecSig_UpperRange = DAL_Dev.Mod_DA.Channels[i].ElecSigUpperRange;
                        devC06chlRow.Data_LowerRange = DAL_Dev.Mod_DA.Channels[i].DataLowerRange;
                        devC06chlRow.Data_UpperRange = DAL_Dev.Mod_DA.Channels[i].DataUpperRange;
                        devC06chlRow.IsUsed = DAL_Dev.Mod_DA.Channels[i].IsUsed;
                        devC06chlRow.IsOutType = DAL_Dev.Mod_DA.Channels[i].IsOutType;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            try
            {
                C06TableAdapter.Update(C06Table);
                C06Table.AcceptChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region THP三合一模块通道

            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC06thpchlRowName = new string[3]
            {
                "THP-T", "THP-H", "THP-P"
            };
            for (int i = 0; i < tempC06thpchlRowName.Length; i++)
            {
                try
                {
                    MQZH_DB_DevDataSet.C06模拟量通道参数Row checkExistC06Row = C06Table.FindByChannelNO(tempC06thpchlRowName[i]);
                    if (checkExistC06Row == null)
                    {
                        MQZH_DB_DevDataSet.C06模拟量通道参数Row defDevC06Row = C06Table.FindByChannelNO(tempC06thpchlRowName[i].ToString() + "Def");
                        MQZH_DB_DevDataSet.C06模拟量通道参数Row newDevC06Row = C06Table.NewC06模拟量通道参数Row();
                        newDevC06Row.ItemArray = (object[])defDevC06Row.ItemArray.Clone();
                        newDevC06Row.ChannelNO = tempC06thpchlRowName[i];
                        C06Table.AddC06模拟量通道参数Row(newDevC06Row);
                        C06TableAdapter.Update(C06Table);
                        C06Table.AcceptChanges();
                        RaisePropertyChanged(() => C06Table);
                        MessageBox.Show("未找到" + tempC06thpchlRowName[i] + "通道配置参数，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            //保存模拟量通道配置参数
            for (int i = 0; i < tempC06thpchlRowName.Length; i++)
            {
                try
                {
                    MQZH_DB_DevDataSet.C06模拟量通道参数Row devC06chlRow = C06Table.FindByChannelNO(tempC06thpchlRowName[i]);
                    if (devC06chlRow != null)
                    {
                        devC06chlRow.ChannelNO = DAL_Dev.Mod_THP.Channels[i].ChannelNO;
                        devC06chlRow.ColType = DAL_Dev.Mod_THP.Channels[i].InfType;
                        devC06chlRow.ElecSig_Unit = DAL_Dev.Mod_THP.Channels[i].ElecSigUnit;
                        devC06chlRow.ElecSig_LowerRange = DAL_Dev.Mod_THP.Channels[i].ElecSigLowerRange;
                        devC06chlRow.ElecSig_UpperRange = DAL_Dev.Mod_THP.Channels[i].ElecSigUpperRange;
                        devC06chlRow.Data_LowerRange = DAL_Dev.Mod_THP.Channels[i].DataLowerRange;
                        devC06chlRow.Data_UpperRange = DAL_Dev.Mod_THP.Channels[i].DataUpperRange;
                        devC06chlRow.IsUsed = DAL_Dev.Mod_THP.Channels[i].IsUsed;
                        devC06chlRow.IsOutType = DAL_Dev.Mod_THP.Channels[i].IsOutType;
                        devC06chlRow.FitRitio = DAL_Dev.Mod_THP.Channels[i].FitRatio;

                        C06TableAdapter.Update(C06Table);
                        C06Table.AcceptChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            #endregion
        }
        
        /// <summary>
        /// 保存C09PID设置参数
        /// </summary>
        private void SaveDevPIDSettings()
        {
            //保存C09PID控制参数
            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC09RowName = new string[28]
            {
                "PID11", "PID12", "PID13", "PID14", "PID15",
                "PID21", "PID22", "PID23", "PID24", "PID25", "PID26",
                "PID31", "PID32", "PID33", "PID34", "PID35", "PID36",
                "PID41", "PID42", "PID43", "PID44", "PID45",
                "PID51",
                "PID61", "PID62", "PID63", "PID64", "PID65",
            };
            for (int i = 0; i < tempC09RowName.Length; i++)
            {
                try
                {
                    MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row checkExistC09Row = C09Table.FindBy配置编号(tempC09RowName[i]);
                    if (checkExistC09Row == null)
                    {
                        MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row defDevC09Row =
                            C09Table.FindBy配置编号(tempC09RowName[i].ToString() + "Def");
                        MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row newDevC09ow = C09Table.NewC09PID控制参数Row();
                        newDevC09ow.ItemArray = (object[])defDevC09Row.ItemArray.Clone();
                        newDevC09ow.配置编号 = tempC09RowName[i];
                        C09Table.AddC09PID控制参数Row(newDevC09ow);
                        C09TableAdapter.Update(C09Table);
                        C09Table.AcceptChanges();
                        RaisePropertyChanged(() => C09Table);
                        MessageBox.Show("未找到" + tempC09RowName[i] + "PID控制参数，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            //PID11
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0911Row = C09Table.FindBy配置编号(tempC09RowName[0]);
                if (devC0911Row != null)
                {
                    devC0911Row.PIDName = DAL_Dev.PID11.ControllerName;
                    devC0911Row.Kp = DAL_Dev.PID11.Kp;
                    devC0911Row.Ki = DAL_Dev.PID11.Ki;
                    devC0911Row.Kd = DAL_Dev.PID11.Kd;
                    devC0911Row.U_UpperBound = DAL_Dev.PID11.U_UpperBound;
                    devC0911Row.U_LowerBound = DAL_Dev.PID11.U_LowerBound;
                    //devC0911Row.T = DAL_Dev.PID11.T;
                    devC0911Row.PIDType = (int)DAL_Dev.PID11.ControllerType;
                    devC0911Row.U_IMax_Limit = DAL_Dev.PID11.U_IMax_Limit;
                    devC0911Row.ErrBound_IntegralSeparate = DAL_Dev.PID11.ErrBound_IntegralSeparate;
                    devC0911Row.Is_Kp_Used = DAL_Dev.PID11.Is_Kp_Used;
                    devC0911Row.Is_Ki_Used = DAL_Dev.PID11.Is_Ki_Used;
                    devC0911Row.Is_Kd_Used = DAL_Dev.PID11.Is_Kd_Used;
                    devC0911Row.Is_U_Limit = DAL_Dev.PID11.Is_U_Limit;
                    devC0911Row.Is_UILimit_Used = DAL_Dev.PID11.Is_UILimit_Used;
                    devC0911Row.Is_ISeparate_Used = DAL_Dev.PID11.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID12
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0912Row = C09Table.FindBy配置编号(tempC09RowName[1]);
                if (devC0912Row != null)
                {
                    devC0912Row.PIDName = DAL_Dev.PID12.ControllerName;
                    devC0912Row.Kp = DAL_Dev.PID12.Kp;
                    devC0912Row.Ki = DAL_Dev.PID12.Ki;
                    devC0912Row.Kd = DAL_Dev.PID12.Kd;
                    devC0912Row.U_UpperBound = DAL_Dev.PID12.U_UpperBound;
                    devC0912Row.U_LowerBound = DAL_Dev.PID12.U_LowerBound;
                    //devC0912Row.T = DAL_Dev.PID12.T;
                    devC0912Row.PIDType = (int)DAL_Dev.PID12.ControllerType;
                    devC0912Row.U_IMax_Limit = DAL_Dev.PID12.U_IMax_Limit;
                    devC0912Row.ErrBound_IntegralSeparate = DAL_Dev.PID12.ErrBound_IntegralSeparate;
                    devC0912Row.Is_Kp_Used = DAL_Dev.PID12.Is_Kp_Used;
                    devC0912Row.Is_Ki_Used = DAL_Dev.PID12.Is_Ki_Used;
                    devC0912Row.Is_Kd_Used = DAL_Dev.PID12.Is_Kd_Used;
                    devC0912Row.Is_U_Limit = DAL_Dev.PID12.Is_U_Limit;
                    devC0912Row.Is_UILimit_Used = DAL_Dev.PID12.Is_UILimit_Used;
                    devC0912Row.Is_ISeparate_Used = DAL_Dev.PID12.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID13
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0913Row = C09Table.FindBy配置编号(tempC09RowName[2]);
                if (devC0913Row != null)
                {
                    devC0913Row.PIDName = DAL_Dev.PID13.ControllerName;
                    devC0913Row.Kp = DAL_Dev.PID13.Kp;
                    devC0913Row.Ki = DAL_Dev.PID13.Ki;
                    devC0913Row.Kd = DAL_Dev.PID13.Kd;
                    devC0913Row.U_UpperBound = DAL_Dev.PID13.U_UpperBound;
                    devC0913Row.U_LowerBound = DAL_Dev.PID13.U_LowerBound;
                    //devC0913Row.T = DAL_Dev.PID13.T;
                    devC0913Row.PIDType = (int)DAL_Dev.PID13.ControllerType;
                    devC0913Row.U_IMax_Limit = DAL_Dev.PID13.U_IMax_Limit;
                    devC0913Row.ErrBound_IntegralSeparate = DAL_Dev.PID13.ErrBound_IntegralSeparate;
                    devC0913Row.Is_Kp_Used = DAL_Dev.PID13.Is_Kp_Used;
                    devC0913Row.Is_Ki_Used = DAL_Dev.PID13.Is_Ki_Used;
                    devC0913Row.Is_Kd_Used = DAL_Dev.PID13.Is_Kd_Used;
                    devC0913Row.Is_U_Limit = DAL_Dev.PID13.Is_U_Limit;
                    devC0913Row.Is_UILimit_Used = DAL_Dev.PID13.Is_UILimit_Used;
                    devC0913Row.Is_ISeparate_Used = DAL_Dev.PID13.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID14
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0914Row = C09Table.FindBy配置编号(tempC09RowName[3]);
                if (devC0914Row != null)
                {
                    devC0914Row.PIDName = DAL_Dev.PID14.ControllerName;
                    devC0914Row.Kp = DAL_Dev.PID14.Kp;
                    devC0914Row.Ki = DAL_Dev.PID14.Ki;
                    devC0914Row.Kd = DAL_Dev.PID14.Kd;
                    devC0914Row.U_UpperBound = DAL_Dev.PID14.U_UpperBound;
                    devC0914Row.U_LowerBound = DAL_Dev.PID14.U_LowerBound;
                    //devC0914Row.T = DAL_Dev.PID14.T;
                    devC0914Row.PIDType = (int)DAL_Dev.PID14.ControllerType;
                    devC0914Row.U_IMax_Limit = DAL_Dev.PID14.U_IMax_Limit;
                    devC0914Row.ErrBound_IntegralSeparate = DAL_Dev.PID14.ErrBound_IntegralSeparate;
                    devC0914Row.Is_Kp_Used = DAL_Dev.PID14.Is_Kp_Used;
                    devC0914Row.Is_Ki_Used = DAL_Dev.PID14.Is_Ki_Used;
                    devC0914Row.Is_Kd_Used = DAL_Dev.PID14.Is_Kd_Used;
                    devC0914Row.Is_U_Limit = DAL_Dev.PID14.Is_U_Limit;
                    devC0914Row.Is_UILimit_Used = DAL_Dev.PID14.Is_UILimit_Used;
                    devC0914Row.Is_ISeparate_Used = DAL_Dev.PID14.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID15
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0915Row = C09Table.FindBy配置编号(tempC09RowName[4]);
                if (devC0915Row != null)
                {
                    devC0915Row.PIDName = DAL_Dev.PID15.ControllerName;
                    devC0915Row.Kp = DAL_Dev.PID15.Kp;
                    devC0915Row.Ki = DAL_Dev.PID15.Ki;
                    devC0915Row.Kd = DAL_Dev.PID15.Kd;
                    devC0915Row.U_UpperBound = DAL_Dev.PID15.U_UpperBound;
                    devC0915Row.U_LowerBound = DAL_Dev.PID15.U_LowerBound;
                    //devC0915Row.T = DAL_Dev.PID15.T;
                    devC0915Row.PIDType = (int)DAL_Dev.PID15.ControllerType;
                    devC0915Row.U_IMax_Limit = DAL_Dev.PID15.U_IMax_Limit;
                    devC0915Row.ErrBound_IntegralSeparate = DAL_Dev.PID15.ErrBound_IntegralSeparate;
                    devC0915Row.Is_Kp_Used = DAL_Dev.PID15.Is_Kp_Used;
                    devC0915Row.Is_Ki_Used = DAL_Dev.PID15.Is_Ki_Used;
                    devC0915Row.Is_Kd_Used = DAL_Dev.PID15.Is_Kd_Used;
                    devC0915Row.Is_U_Limit = DAL_Dev.PID15.Is_U_Limit;
                    devC0915Row.Is_UILimit_Used = DAL_Dev.PID15.Is_UILimit_Used;
                    devC0915Row.Is_ISeparate_Used = DAL_Dev.PID15.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID41
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0941Row = C09Table.FindBy配置编号(tempC09RowName[17]);
                if (devC0941Row != null)
                {
                    devC0941Row.PIDName = DAL_Dev.PID41.ControllerName;
                    devC0941Row.Kp = DAL_Dev.PID41.Kp;
                    devC0941Row.Ki = DAL_Dev.PID41.Ki;
                    devC0941Row.Kd = DAL_Dev.PID41.Kd;
                    devC0941Row.U_UpperBound = DAL_Dev.PID41.U_UpperBound;
                    devC0941Row.U_LowerBound = DAL_Dev.PID41.U_LowerBound;
                    //devC0941Row.T = DAL_Dev.PID41.T;
                    devC0941Row.PIDType = (int)DAL_Dev.PID41.ControllerType;
                    devC0941Row.U_IMax_Limit = DAL_Dev.PID41.U_IMax_Limit;
                    devC0941Row.ErrBound_IntegralSeparate = DAL_Dev.PID41.ErrBound_IntegralSeparate;
                    devC0941Row.Is_Kp_Used = DAL_Dev.PID41.Is_Kp_Used;
                    devC0941Row.Is_Ki_Used = DAL_Dev.PID41.Is_Ki_Used;
                    devC0941Row.Is_Kd_Used = DAL_Dev.PID41.Is_Kd_Used;
                    devC0941Row.Is_U_Limit = DAL_Dev.PID41.Is_U_Limit;
                    devC0941Row.Is_UILimit_Used = DAL_Dev.PID41.Is_UILimit_Used;
                    devC0941Row.Is_ISeparate_Used = DAL_Dev.PID41.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID42
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0942Row = C09Table.FindBy配置编号(tempC09RowName[18]);
                if (devC0942Row != null)
                {
                    devC0942Row.PIDName = DAL_Dev.PID42.ControllerName;
                    devC0942Row.Kp = DAL_Dev.PID42.Kp;
                    devC0942Row.Ki = DAL_Dev.PID42.Ki;
                    devC0942Row.Kd = DAL_Dev.PID42.Kd;
                    devC0942Row.U_UpperBound = DAL_Dev.PID42.U_UpperBound;
                    devC0942Row.U_LowerBound = DAL_Dev.PID42.U_LowerBound;
                    //devC0942Row.T = DAL_Dev.PID42.T;
                    devC0942Row.PIDType = (int)DAL_Dev.PID42.ControllerType;
                    devC0942Row.U_IMax_Limit = DAL_Dev.PID42.U_IMax_Limit;
                    devC0942Row.ErrBound_IntegralSeparate = DAL_Dev.PID42.ErrBound_IntegralSeparate;
                    devC0942Row.Is_Kp_Used = DAL_Dev.PID42.Is_Kp_Used;
                    devC0942Row.Is_Ki_Used = DAL_Dev.PID42.Is_Ki_Used;
                    devC0942Row.Is_Kd_Used = DAL_Dev.PID42.Is_Kd_Used;
                    devC0942Row.Is_U_Limit = DAL_Dev.PID42.Is_U_Limit;
                    devC0942Row.Is_UILimit_Used = DAL_Dev.PID42.Is_UILimit_Used;
                    devC0942Row.Is_ISeparate_Used = DAL_Dev.PID42.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID43
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0943Row = C09Table.FindBy配置编号(tempC09RowName[19]);
                if (devC0943Row != null)
                {
                    devC0943Row.PIDName = DAL_Dev.PID43.ControllerName;
                    devC0943Row.Kp = DAL_Dev.PID43.Kp;
                    devC0943Row.Ki = DAL_Dev.PID43.Ki;
                    devC0943Row.Kd = DAL_Dev.PID43.Kd;
                    devC0943Row.U_UpperBound = DAL_Dev.PID43.U_UpperBound;
                    devC0943Row.U_LowerBound = DAL_Dev.PID43.U_LowerBound;
                    //devC0943Row.T = DAL_Dev.PID43.T;
                    devC0943Row.PIDType = (int)DAL_Dev.PID43.ControllerType;
                    devC0943Row.U_IMax_Limit = DAL_Dev.PID43.U_IMax_Limit;
                    devC0943Row.ErrBound_IntegralSeparate = DAL_Dev.PID43.ErrBound_IntegralSeparate;
                    devC0943Row.Is_Kp_Used = DAL_Dev.PID43.Is_Kp_Used;
                    devC0943Row.Is_Ki_Used = DAL_Dev.PID43.Is_Ki_Used;
                    devC0943Row.Is_Kd_Used = DAL_Dev.PID43.Is_Kd_Used;
                    devC0943Row.Is_U_Limit = DAL_Dev.PID43.Is_U_Limit;
                    devC0943Row.Is_UILimit_Used = DAL_Dev.PID43.Is_UILimit_Used;
                    devC0943Row.Is_ISeparate_Used = DAL_Dev.PID43.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID44
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0944Row = C09Table.FindBy配置编号(tempC09RowName[20]);
                if (devC0944Row != null)
                {
                    devC0944Row.PIDName = DAL_Dev.PID44.ControllerName;
                    devC0944Row.Kp = DAL_Dev.PID44.Kp;
                    devC0944Row.Ki = DAL_Dev.PID44.Ki;
                    devC0944Row.Kd = DAL_Dev.PID44.Kd;
                    devC0944Row.U_UpperBound = DAL_Dev.PID44.U_UpperBound;
                    devC0944Row.U_LowerBound = DAL_Dev.PID44.U_LowerBound;
                    //devC0944Row.T = DAL_Dev.PID44.T;
                    devC0944Row.PIDType = (int)DAL_Dev.PID44.ControllerType;
                    devC0944Row.U_IMax_Limit = DAL_Dev.PID44.U_IMax_Limit;
                    devC0944Row.ErrBound_IntegralSeparate = DAL_Dev.PID44.ErrBound_IntegralSeparate;
                    devC0944Row.Is_Kp_Used = DAL_Dev.PID44.Is_Kp_Used;
                    devC0944Row.Is_Ki_Used = DAL_Dev.PID44.Is_Ki_Used;
                    devC0944Row.Is_Kd_Used = DAL_Dev.PID44.Is_Kd_Used;
                    devC0944Row.Is_U_Limit = DAL_Dev.PID44.Is_U_Limit;
                    devC0944Row.Is_UILimit_Used = DAL_Dev.PID44.Is_UILimit_Used;
                    devC0944Row.Is_ISeparate_Used = DAL_Dev.PID44.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID45
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0945Row = C09Table.FindBy配置编号(tempC09RowName[21]);
                if (devC0945Row != null)
                {
                    devC0945Row.PIDName = DAL_Dev.PID45.ControllerName;
                    devC0945Row.Kp = DAL_Dev.PID45.Kp;
                    devC0945Row.Ki = DAL_Dev.PID45.Ki;
                    devC0945Row.Kd = DAL_Dev.PID45.Kd;
                    devC0945Row.U_UpperBound = DAL_Dev.PID45.U_UpperBound;
                    devC0945Row.U_LowerBound = DAL_Dev.PID45.U_LowerBound;
                    //devC0945Row.T = DAL_Dev.PID45.T;
                    devC0945Row.PIDType = (int)DAL_Dev.PID45.ControllerType;
                    devC0945Row.U_IMax_Limit = DAL_Dev.PID45.U_IMax_Limit;
                    devC0945Row.ErrBound_IntegralSeparate = DAL_Dev.PID45.ErrBound_IntegralSeparate;
                    devC0945Row.Is_Kp_Used = DAL_Dev.PID45.Is_Kp_Used;
                    devC0945Row.Is_Ki_Used = DAL_Dev.PID45.Is_Ki_Used;
                    devC0945Row.Is_Kd_Used = DAL_Dev.PID45.Is_Kd_Used;
                    devC0945Row.Is_U_Limit = DAL_Dev.PID45.Is_U_Limit;
                    devC0945Row.Is_UILimit_Used = DAL_Dev.PID45.Is_UILimit_Used;
                    devC0945Row.Is_ISeparate_Used = DAL_Dev.PID45.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID61
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0961Row = C09Table.FindBy配置编号(tempC09RowName[23]);
                if (devC0961Row != null)
                {
                    devC0961Row.PIDName = DAL_Dev.PID61.ControllerName;
                    devC0961Row.Kp = DAL_Dev.PID61.Kp;
                    devC0961Row.Ki = DAL_Dev.PID61.Ki;
                    devC0961Row.Kd = DAL_Dev.PID61.Kd;
                    devC0961Row.U_UpperBound = DAL_Dev.PID61.U_UpperBound;
                    devC0961Row.U_LowerBound = DAL_Dev.PID61.U_LowerBound;
                    //devC0961Row.T = DAL_Dev.PID61.T;
                    devC0961Row.PIDType = (int)DAL_Dev.PID61.ControllerType;
                    devC0961Row.U_IMax_Limit = DAL_Dev.PID61.U_IMax_Limit;
                    devC0961Row.ErrBound_IntegralSeparate = DAL_Dev.PID61.ErrBound_IntegralSeparate;
                    devC0961Row.Is_Kp_Used = DAL_Dev.PID61.Is_Kp_Used;
                    devC0961Row.Is_Ki_Used = DAL_Dev.PID61.Is_Ki_Used;
                    devC0961Row.Is_Kd_Used = DAL_Dev.PID61.Is_Kd_Used;
                    devC0961Row.Is_U_Limit = DAL_Dev.PID61.Is_U_Limit;
                    devC0961Row.Is_UILimit_Used = DAL_Dev.PID61.Is_UILimit_Used;
                    devC0961Row.Is_ISeparate_Used = DAL_Dev.PID61.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID62
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0962Row = C09Table.FindBy配置编号(tempC09RowName[24]);
                if (devC0962Row != null)
                {
                    devC0962Row.PIDName = DAL_Dev.PID62.ControllerName;
                    devC0962Row.Kp = DAL_Dev.PID62.Kp;
                    devC0962Row.Ki = DAL_Dev.PID62.Ki;
                    devC0962Row.Kd = DAL_Dev.PID62.Kd;
                    devC0962Row.U_UpperBound = DAL_Dev.PID62.U_UpperBound;
                    devC0962Row.U_LowerBound = DAL_Dev.PID62.U_LowerBound;
                    //devC0962Row.T = DAL_Dev.PID62.T;
                    devC0962Row.PIDType = (int)DAL_Dev.PID62.ControllerType;
                    devC0962Row.U_IMax_Limit = DAL_Dev.PID62.U_IMax_Limit;
                    devC0962Row.ErrBound_IntegralSeparate = DAL_Dev.PID62.ErrBound_IntegralSeparate;
                    devC0962Row.Is_Kp_Used = DAL_Dev.PID62.Is_Kp_Used;
                    devC0962Row.Is_Ki_Used = DAL_Dev.PID62.Is_Ki_Used;
                    devC0962Row.Is_Kd_Used = DAL_Dev.PID62.Is_Kd_Used;
                    devC0962Row.Is_U_Limit = DAL_Dev.PID62.Is_U_Limit;
                    devC0962Row.Is_UILimit_Used = DAL_Dev.PID62.Is_UILimit_Used;
                    devC0962Row.Is_ISeparate_Used = DAL_Dev.PID62.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID63
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0963Row = C09Table.FindBy配置编号(tempC09RowName[25]);
                if (devC0963Row != null)
                {
                    devC0963Row.PIDName = DAL_Dev.PID63.ControllerName;
                    devC0963Row.Kp = DAL_Dev.PID63.Kp;
                    devC0963Row.Ki = DAL_Dev.PID63.Ki;
                    devC0963Row.Kd = DAL_Dev.PID63.Kd;
                    devC0963Row.U_UpperBound = DAL_Dev.PID63.U_UpperBound;
                    devC0963Row.U_LowerBound = DAL_Dev.PID63.U_LowerBound;
                    //devC0963Row.T = DAL_Dev.PID63.T;
                    devC0963Row.PIDType = (int)DAL_Dev.PID63.ControllerType;
                    devC0963Row.U_IMax_Limit = DAL_Dev.PID63.U_IMax_Limit;
                    devC0963Row.ErrBound_IntegralSeparate = DAL_Dev.PID63.ErrBound_IntegralSeparate;
                    devC0963Row.Is_Kp_Used = DAL_Dev.PID63.Is_Kp_Used;
                    devC0963Row.Is_Ki_Used = DAL_Dev.PID63.Is_Ki_Used;
                    devC0963Row.Is_Kd_Used = DAL_Dev.PID63.Is_Kd_Used;
                    devC0963Row.Is_U_Limit = DAL_Dev.PID63.Is_U_Limit;
                    devC0963Row.Is_UILimit_Used = DAL_Dev.PID63.Is_UILimit_Used;
                    devC0963Row.Is_ISeparate_Used = DAL_Dev.PID63.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID64
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0964Row = C09Table.FindBy配置编号(tempC09RowName[26]);
                if (devC0964Row != null)
                {
                    devC0964Row.PIDName = DAL_Dev.PID64.ControllerName;
                    devC0964Row.Kp = DAL_Dev.PID64.Kp;
                    devC0964Row.Ki = DAL_Dev.PID64.Ki;
                    devC0964Row.Kd = DAL_Dev.PID64.Kd;
                    devC0964Row.U_UpperBound = DAL_Dev.PID64.U_UpperBound;
                    devC0964Row.U_LowerBound = DAL_Dev.PID64.U_LowerBound;
                    //devC0964Row.T = DAL_Dev.PID64.T;
                    devC0964Row.PIDType = (int)DAL_Dev.PID64.ControllerType;
                    devC0964Row.U_IMax_Limit = DAL_Dev.PID64.U_IMax_Limit;
                    devC0964Row.ErrBound_IntegralSeparate = DAL_Dev.PID64.ErrBound_IntegralSeparate;
                    devC0964Row.Is_Kp_Used = DAL_Dev.PID64.Is_Kp_Used;
                    devC0964Row.Is_Ki_Used = DAL_Dev.PID64.Is_Ki_Used;
                    devC0964Row.Is_Kd_Used = DAL_Dev.PID64.Is_Kd_Used;
                    devC0964Row.Is_U_Limit = DAL_Dev.PID64.Is_U_Limit;
                    devC0964Row.Is_UILimit_Used = DAL_Dev.PID64.Is_UILimit_Used;
                    devC0964Row.Is_ISeparate_Used = DAL_Dev.PID64.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID65
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0965Row = C09Table.FindBy配置编号(tempC09RowName[27]);
                if (devC0965Row != null)
                {
                    devC0965Row.PIDName = DAL_Dev.PID65.ControllerName;
                    devC0965Row.Kp = DAL_Dev.PID65.Kp;
                    devC0965Row.Ki = DAL_Dev.PID65.Ki;
                    devC0965Row.Kd = DAL_Dev.PID65.Kd;
                    devC0965Row.U_UpperBound = DAL_Dev.PID65.U_UpperBound;
                    devC0965Row.U_LowerBound = DAL_Dev.PID65.U_LowerBound;
                    //devC0965Row.T = DAL_Dev.PID65.T;
                    devC0965Row.PIDType = (int)DAL_Dev.PID65.ControllerType;
                    devC0965Row.U_IMax_Limit = DAL_Dev.PID65.U_IMax_Limit;
                    devC0965Row.ErrBound_IntegralSeparate = DAL_Dev.PID65.ErrBound_IntegralSeparate;
                    devC0965Row.Is_Kp_Used = DAL_Dev.PID65.Is_Kp_Used;
                    devC0965Row.Is_Ki_Used = DAL_Dev.PID65.Is_Ki_Used;
                    devC0965Row.Is_Kd_Used = DAL_Dev.PID65.Is_Kd_Used;
                    devC0965Row.Is_U_Limit = DAL_Dev.PID65.Is_U_Limit;
                    devC0965Row.Is_UILimit_Used = DAL_Dev.PID65.Is_UILimit_Used;
                    devC0965Row.Is_ISeparate_Used = DAL_Dev.PID65.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
            //PID21
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0921Row = C09Table.FindBy配置编号(tempC09RowName[5]);
                if (devC0921Row != null)
                {
                    devC0921Row.PIDName = DAL_Dev.PID21.ControllerName;
                    devC0921Row.Kp = DAL_Dev.PID21.Kp;
                    devC0921Row.Ki = DAL_Dev.PID21.Ki;
                    devC0921Row.Kd = DAL_Dev.PID21.Kd;
                    devC0921Row.U_UpperBound = DAL_Dev.PID21.U_UpperBound;
                    devC0921Row.U_LowerBound = DAL_Dev.PID21.U_LowerBound;
                    //devC0921Row.T = DAL_Dev.PID21.T;
                    devC0921Row.PIDType = (int)DAL_Dev.PID21.ControllerType;
                    devC0921Row.U_IMax_Limit = DAL_Dev.PID21.U_IMax_Limit;
                    devC0921Row.ErrBound_IntegralSeparate = DAL_Dev.PID21.ErrBound_IntegralSeparate;
                    devC0921Row.Is_Kp_Used = DAL_Dev.PID21.Is_Kp_Used;
                    devC0921Row.Is_Ki_Used = DAL_Dev.PID21.Is_Ki_Used;
                    devC0921Row.Is_Kd_Used = DAL_Dev.PID21.Is_Kd_Used;
                    devC0921Row.Is_U_Limit = DAL_Dev.PID21.Is_U_Limit;
                    devC0921Row.Is_UILimit_Used = DAL_Dev.PID21.Is_UILimit_Used;
                    devC0921Row.Is_ISeparate_Used = DAL_Dev.PID21.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID22
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0922Row = C09Table.FindBy配置编号(tempC09RowName[6]);
                if (devC0922Row != null)
                {
                    devC0922Row.PIDName = DAL_Dev.PID22.ControllerName;
                    devC0922Row.Kp = DAL_Dev.PID22.Kp;
                    devC0922Row.Ki = DAL_Dev.PID22.Ki;
                    devC0922Row.Kd = DAL_Dev.PID22.Kd;
                    devC0922Row.U_UpperBound = DAL_Dev.PID22.U_UpperBound;
                    devC0922Row.U_LowerBound = DAL_Dev.PID22.U_LowerBound;
                    //devC0922Row.T = DAL_Dev.PID22.T;
                    devC0922Row.PIDType =(int) DAL_Dev.PID22.ControllerType;
                    devC0922Row.U_IMax_Limit = DAL_Dev.PID22.U_IMax_Limit;
                    devC0922Row.ErrBound_IntegralSeparate = DAL_Dev.PID22.ErrBound_IntegralSeparate;
                    devC0922Row.Is_Kp_Used = DAL_Dev.PID22.Is_Kp_Used;
                    devC0922Row.Is_Ki_Used = DAL_Dev.PID22.Is_Ki_Used;
                    devC0922Row.Is_Kd_Used = DAL_Dev.PID22.Is_Kd_Used;
                    devC0922Row.Is_U_Limit = DAL_Dev.PID22.Is_U_Limit;
                    devC0922Row.Is_UILimit_Used = DAL_Dev.PID22.Is_UILimit_Used;
                    devC0922Row.Is_ISeparate_Used = DAL_Dev.PID22.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID23
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0923Row = C09Table.FindBy配置编号(tempC09RowName[7]);
                if (devC0923Row != null)
                {
                    devC0923Row.PIDName = DAL_Dev.PID23.ControllerName;
                    devC0923Row.Kp = DAL_Dev.PID23.Kp;
                    devC0923Row.Ki = DAL_Dev.PID23.Ki;
                    devC0923Row.Kd = DAL_Dev.PID23.Kd;
                    devC0923Row.U_UpperBound = DAL_Dev.PID23.U_UpperBound;
                    devC0923Row.U_LowerBound = DAL_Dev.PID23.U_LowerBound;
                    //devC0923Row.T = DAL_Dev.PID23.T;
                    devC0923Row.PIDType = (int)DAL_Dev.PID23.ControllerType;
                    devC0923Row.U_IMax_Limit = DAL_Dev.PID23.U_IMax_Limit;
                    devC0923Row.ErrBound_IntegralSeparate = DAL_Dev.PID23.ErrBound_IntegralSeparate;
                    devC0923Row.Is_Kp_Used = DAL_Dev.PID23.Is_Kp_Used;
                    devC0923Row.Is_Ki_Used = DAL_Dev.PID23.Is_Ki_Used;
                    devC0923Row.Is_Kd_Used = DAL_Dev.PID23.Is_Kd_Used;
                    devC0923Row.Is_U_Limit = DAL_Dev.PID23.Is_U_Limit;
                    devC0923Row.Is_UILimit_Used = DAL_Dev.PID23.Is_UILimit_Used;
                    devC0923Row.Is_ISeparate_Used = DAL_Dev.PID23.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID24
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0924Row = C09Table.FindBy配置编号(tempC09RowName[8]);
                if (devC0924Row != null)
                {
                    devC0924Row.PIDName = DAL_Dev.PID24.ControllerName;
                    devC0924Row.Kp = DAL_Dev.PID24.Kp;
                    devC0924Row.Ki = DAL_Dev.PID24.Ki;
                    devC0924Row.Kd = DAL_Dev.PID24.Kd;
                    devC0924Row.U_UpperBound = DAL_Dev.PID24.U_UpperBound;
                    devC0924Row.U_LowerBound = DAL_Dev.PID24.U_LowerBound;
                    //devC0924Row.T = DAL_Dev.PID24.T;
                    devC0924Row.PIDType = (int)DAL_Dev.PID24.ControllerType;
                    devC0924Row.U_IMax_Limit = DAL_Dev.PID24.U_IMax_Limit;
                    devC0924Row.ErrBound_IntegralSeparate = DAL_Dev.PID24.ErrBound_IntegralSeparate;
                    devC0924Row.Is_Kp_Used = DAL_Dev.PID24.Is_Kp_Used;
                    devC0924Row.Is_Ki_Used = DAL_Dev.PID24.Is_Ki_Used;
                    devC0924Row.Is_Kd_Used = DAL_Dev.PID24.Is_Kd_Used;
                    devC0924Row.Is_U_Limit = DAL_Dev.PID24.Is_U_Limit;
                    devC0924Row.Is_UILimit_Used = DAL_Dev.PID24.Is_UILimit_Used;
                    devC0924Row.Is_ISeparate_Used = DAL_Dev.PID24.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID25
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0925Row = C09Table.FindBy配置编号(tempC09RowName[9]);
                if (devC0925Row != null)
                {
                    devC0925Row.PIDName = DAL_Dev.PID25.ControllerName;
                    devC0925Row.Kp = DAL_Dev.PID25.Kp;
                    devC0925Row.Ki = DAL_Dev.PID25.Ki;
                    devC0925Row.Kd = DAL_Dev.PID25.Kd;
                    devC0925Row.U_UpperBound = DAL_Dev.PID25.U_UpperBound;
                    devC0925Row.U_LowerBound = DAL_Dev.PID25.U_LowerBound;
                    //devC0925Row.T = DAL_Dev.PID25.T;
                    devC0925Row.PIDType = (int)DAL_Dev.PID25.ControllerType;
                    devC0925Row.U_IMax_Limit = DAL_Dev.PID25.U_IMax_Limit;
                    devC0925Row.ErrBound_IntegralSeparate = DAL_Dev.PID25.ErrBound_IntegralSeparate;
                    devC0925Row.Is_Kp_Used = DAL_Dev.PID25.Is_Kp_Used;
                    devC0925Row.Is_Ki_Used = DAL_Dev.PID25.Is_Ki_Used;
                    devC0925Row.Is_Kd_Used = DAL_Dev.PID25.Is_Kd_Used;
                    devC0925Row.Is_U_Limit = DAL_Dev.PID25.Is_U_Limit;
                    devC0925Row.Is_UILimit_Used = DAL_Dev.PID25.Is_UILimit_Used;
                    devC0925Row.Is_ISeparate_Used = DAL_Dev.PID25.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID26
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0926Row = C09Table.FindBy配置编号(tempC09RowName[10]);
                if (devC0926Row != null)
                {
                    devC0926Row.PIDName = DAL_Dev.PID26.ControllerName;
                    devC0926Row.Kp = DAL_Dev.PID26.Kp;
                    devC0926Row.Ki = DAL_Dev.PID26.Ki;
                    devC0926Row.Kd = DAL_Dev.PID26.Kd;
                    devC0926Row.U_UpperBound = DAL_Dev.PID26.U_UpperBound;
                    devC0926Row.U_LowerBound = DAL_Dev.PID26.U_LowerBound;
                    //devC0926Row.T = DAL_Dev.PID26.T;
                    devC0926Row.PIDType = (int)DAL_Dev.PID26.ControllerType;
                    devC0926Row.U_IMax_Limit = DAL_Dev.PID26.U_IMax_Limit;
                    devC0926Row.ErrBound_IntegralSeparate = DAL_Dev.PID26.ErrBound_IntegralSeparate;
                    devC0926Row.Is_Kp_Used = DAL_Dev.PID26.Is_Kp_Used;
                    devC0926Row.Is_Ki_Used = DAL_Dev.PID26.Is_Ki_Used;
                    devC0926Row.Is_Kd_Used = DAL_Dev.PID26.Is_Kd_Used;
                    devC0926Row.Is_U_Limit = DAL_Dev.PID26.Is_U_Limit;
                    devC0926Row.Is_UILimit_Used = DAL_Dev.PID26.Is_UILimit_Used;
                    devC0926Row.Is_ISeparate_Used = DAL_Dev.PID26.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID51
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C09PID控制参数Row devC0951Row = C09Table.FindBy配置编号(tempC09RowName[22]);
                if (devC0951Row != null)
                {
                    devC0951Row.PIDName = DAL_Dev.PID51.ControllerName;
                    devC0951Row.Kp = DAL_Dev.PID51.Kp;
                    devC0951Row.Ki = DAL_Dev.PID51.Ki;
                    devC0951Row.Kd = DAL_Dev.PID51.Kd;
                    devC0951Row.U_UpperBound = DAL_Dev.PID51.U_UpperBound;
                    devC0951Row.U_LowerBound = DAL_Dev.PID51.U_LowerBound;
                    //devC0936Row.T = DAL_Dev.PID51.T;
                    devC0951Row.PIDType = (int)DAL_Dev.PID51.ControllerType;
                    devC0951Row.U_IMax_Limit = DAL_Dev.PID51.U_IMax_Limit;
                    devC0951Row.ErrBound_IntegralSeparate = DAL_Dev.PID51.ErrBound_IntegralSeparate;
                    devC0951Row.Is_Kp_Used = DAL_Dev.PID51.Is_Kp_Used;
                    devC0951Row.Is_Ki_Used = DAL_Dev.PID51.Is_Ki_Used;
                    devC0951Row.Is_Kd_Used = DAL_Dev.PID51.Is_Kd_Used;
                    devC0951Row.Is_U_Limit = DAL_Dev.PID51.Is_U_Limit;
                    devC0951Row.Is_UILimit_Used = DAL_Dev.PID51.Is_UILimit_Used;
                    devC0951Row.Is_ISeparate_Used = DAL_Dev.PID51.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        /// <summary>
        /// 保存C12、13压力设定参数
        /// </summary>
        private void SavePressSettings()
        {
            string tempSettingsNO = "Using";


            //C12气水密压力设定参数Row
            //若编号在表中不存在，则新建（拷贝DefaultSetting）
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C12气水密压力设定参数Row checkExistC12Row = C12Table.FindBy配置编号(tempSettingsNO);
                if (checkExistC12Row == null)
                {
                    MQZHWL.MQZH_DB_DevDataSet.C12气水密压力设定参数Row defDevC12Row = C12Table.FindBy配置编号("DefaultSetting");
                    MQZHWL.MQZH_DB_DevDataSet.C12气水密压力设定参数Row newDevC12Row = C12Table.NewC12气水密压力设定参数Row();
                    newDevC12Row.ItemArray = (object[])defDevC12Row.ItemArray.Clone();
                    newDevC12Row.配置编号 = tempSettingsNO;
                    C12Table.AddC12气水密压力设定参数Row(newDevC12Row);
                    C12TableAdapter.Update(C12Table);
                    C12Table.AcceptChanges();
                    RaisePropertyChanged(() => C12Table);
                    MessageBox.Show("未找到C12气水密压力设定参数，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C12气水密压力设定参数Row devC12Row = C12Table.FindBy配置编号(tempSettingsNO);
                if (devC12Row != null)
                {
                    //气密定级标准值
                    devC12Row.IsPressFalse = DAL_Dev.IsPressFalse;
                    devC12Row.气密定级正预加压标准值 = DAL_Dev.PressSet_QMDJ_Std[0][0];
                    devC12Row.气密定级正压1标准值 = DAL_Dev.PressSet_QMDJ_Std[1][0];
                    devC12Row.气密定级正压2标准值 = DAL_Dev.PressSet_QMDJ_Std[1][1];
                    devC12Row.气密定级正压3标准值 = DAL_Dev.PressSet_QMDJ_Std[1][2];
                    devC12Row.气密定级负预加压标准值 = DAL_Dev.PressSet_QMDJ_Std[2][0];
                    devC12Row.气密定级负压1标准值 = DAL_Dev.PressSet_QMDJ_Std[3][0];
                    devC12Row.气密定级负压2标准值 = DAL_Dev.PressSet_QMDJ_Std[3][1];
                    devC12Row.气密定级负压3标准值 = DAL_Dev.PressSet_QMDJ_Std[3][2];
                    //气密定级实际值
                    devC12Row.气密定级正预加压实际值 = DAL_Dev.PressSet_QMDJ_False[0][0];
                    devC12Row.气密定级正压1实际值 = DAL_Dev.PressSet_QMDJ_False[1][0];
                    devC12Row.气密定级正压2实际值 = DAL_Dev.PressSet_QMDJ_False[1][1];
                    devC12Row.气密定级正压3实际值 = DAL_Dev.PressSet_QMDJ_False[1][2];
                    devC12Row.气密定级负预加压实际值 = DAL_Dev.PressSet_QMDJ_False[2][0];
                    devC12Row.气密定级负压1实际值 = DAL_Dev.PressSet_QMDJ_False[3][0];
                    devC12Row.气密定级负压2实际值 = DAL_Dev.PressSet_QMDJ_False[3][1];
                    devC12Row.气密定级负压3实际值 = DAL_Dev.PressSet_QMDJ_False[3][2];
                    //气密工程实际值
                    devC12Row.气密工程正预加压实际值 = DAL_Dev.PressSet_QMGC_False[0][0];
                    devC12Row.气密工程正检测加压实际值 = DAL_Dev.PressSet_QMGC_False[1][0];
                    devC12Row.气密工程负预加压实际值 = DAL_Dev.PressSet_QMGC_False[2][0];
                    devC12Row.气密工程负检测加压实际值 = DAL_Dev.PressSet_QMGC_False[3][0];

                    //水密
                    devC12Row.水密预加压标准值 = DAL_Dev.PressSet_SM_YJY_Std[0];
                    devC12Row.水密预加压实际值 = DAL_Dev.PressSet_SM_YJY_False[0];
                    devC12Row.水密定级稳定第1级标准值 = DAL_Dev.PressSet_SMDJ_WD_Std[0];
                    devC12Row.水密定级稳定第2级标准值 = DAL_Dev.PressSet_SMDJ_WD_Std[1];
                    devC12Row.水密定级稳定第3级标准值 = DAL_Dev.PressSet_SMDJ_WD_Std[2];
                    devC12Row.水密定级稳定第4级标准值 = DAL_Dev.PressSet_SMDJ_WD_Std[3];
                    devC12Row.水密定级稳定第5级标准值 = DAL_Dev.PressSet_SMDJ_WD_Std[4];
                    devC12Row.水密定级稳定第6级标准值 = DAL_Dev.PressSet_SMDJ_WD_Std[5];
                    devC12Row.水密定级稳定第7级标准值 = DAL_Dev.PressSet_SMDJ_WD_Std[6];
                    devC12Row.水密定级稳定第8级标准值 = DAL_Dev.PressSet_SMDJ_WD_Std[7];
                    devC12Row.水密定级稳定第1级实际值 = DAL_Dev.PressSet_SMDJ_WD_False[0];
                    devC12Row.水密定级稳定第2级实际值 = DAL_Dev.PressSet_SMDJ_WD_False[1];
                    devC12Row.水密定级稳定第3级实际值 = DAL_Dev.PressSet_SMDJ_WD_False[2];
                    devC12Row.水密定级稳定第4级实际值 = DAL_Dev.PressSet_SMDJ_WD_False[3];
                    devC12Row.水密定级稳定第5级实际值 = DAL_Dev.PressSet_SMDJ_WD_False[4];
                    devC12Row.水密定级稳定第6级实际值 = DAL_Dev.PressSet_SMDJ_WD_False[5];
                    devC12Row.水密定级稳定第7级实际值 = DAL_Dev.PressSet_SMDJ_WD_False[6];
                    devC12Row.水密定级稳定第8级实际值 = DAL_Dev.PressSet_SMDJ_WD_False[7];
                    devC12Row.水密定级波动第1级标准值 = DAL_Dev.PressSet_SMDJ_BDPJ_Std[0];
                    devC12Row.水密定级波动第2级标准值 = DAL_Dev.PressSet_SMDJ_BDPJ_Std[1];
                    devC12Row.水密定级波动第3级标准值 = DAL_Dev.PressSet_SMDJ_BDPJ_Std[2];
                    devC12Row.水密定级波动第4级标准值 = DAL_Dev.PressSet_SMDJ_BDPJ_Std[3];
                    devC12Row.水密定级波动第5级标准值 = DAL_Dev.PressSet_SMDJ_BDPJ_Std[4];
                    devC12Row.水密定级波动第6级标准值 = DAL_Dev.PressSet_SMDJ_BDPJ_Std[5];
                    devC12Row.水密定级波动第7级标准值 = DAL_Dev.PressSet_SMDJ_BDPJ_Std[6];
                    devC12Row.水密定级波动第8级标准值 = DAL_Dev.PressSet_SMDJ_BDPJ_Std[7];
                    devC12Row.水密定级波动第1级实际值 = DAL_Dev.PressSet_SMDJ_BDPJ_False[0];
                    devC12Row.水密定级波动第2级实际值 = DAL_Dev.PressSet_SMDJ_BDPJ_False[1];
                    devC12Row.水密定级波动第3级实际值 = DAL_Dev.PressSet_SMDJ_BDPJ_False[2];
                    devC12Row.水密定级波动第4级实际值 = DAL_Dev.PressSet_SMDJ_BDPJ_False[3];
                    devC12Row.水密定级波动第5级实际值 = DAL_Dev.PressSet_SMDJ_BDPJ_False[4];
                    devC12Row.水密定级波动第6级实际值 = DAL_Dev.PressSet_SMDJ_BDPJ_False[5];
                    devC12Row.水密定级波动第7级实际值 = DAL_Dev.PressSet_SMDJ_BDPJ_False[6];
                    devC12Row.水密定级波动第8级实际值 = DAL_Dev.PressSet_SMDJ_BDPJ_False[7];
                    devC12Row.水密工程稳定可开启压力实际值 = DAL_Dev.PressSet_SMGC_WDKKQ_False;
                    devC12Row.水密工程稳定固定压力实际值 = DAL_Dev.PressSet_SMGC_WDGD_False;
                    devC12Row.水密工程波动可开启压力实际值 = DAL_Dev.PressSet_SMGC_BDPJKKQ_False;
                    devC12Row.水密工程波动固定压力实际值 = DAL_Dev.PressSet_SMGC_BDPJGD_False;
                    C12TableAdapter.Update(C12Table);
                    C12Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //C13抗风压压力设定参数Row
            //若编号在表中不存在，则新建（拷贝DefaultSetting）
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C13抗风压压力设定参数Row checkExistC13Row = C13Table.FindBy配置编号(tempSettingsNO);
                if (checkExistC13Row == null)
                {
                    MQZHWL.MQZH_DB_DevDataSet.C13抗风压压力设定参数Row defDevC13Row = C13Table.FindBy配置编号("DefaultSetting");
                    MQZHWL.MQZH_DB_DevDataSet.C13抗风压压力设定参数Row newDevC13Row = C13Table.NewC13抗风压压力设定参数Row();
                    newDevC13Row.ItemArray = (object[])defDevC13Row.ItemArray.Clone();
                    newDevC13Row.配置编号 = tempSettingsNO;
                    C13Table.AddC13抗风压压力设定参数Row(newDevC13Row);
                    C13TableAdapter.Update(C13Table);
                    C13Table.AcceptChanges();
                    RaisePropertyChanged(() => C13Table);
                    MessageBox.Show("未找到C13抗风压压力设定参数，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C13抗风压压力设定参数Row devC13Row = C13Table.FindBy配置编号(tempSettingsNO);
                if (devC13Row != null)
                {
                    //抗风压
                    devC13Row.抗风压定级p1压力标准值01 = DAL_Dev.PressSet_KFY_DJBX_Std[1][0];
                    devC13Row.抗风压定级p1压力标准值02 = DAL_Dev.PressSet_KFY_DJBX_Std[1][1];
                    devC13Row.抗风压定级p1压力标准值03 = DAL_Dev.PressSet_KFY_DJBX_Std[1][2];
                    devC13Row.抗风压定级p1压力标准值04 = DAL_Dev.PressSet_KFY_DJBX_Std[1][3];
                    devC13Row.抗风压定级p1压力标准值05 = DAL_Dev.PressSet_KFY_DJBX_Std[1][4];
                    devC13Row.抗风压定级p1压力标准值06 = DAL_Dev.PressSet_KFY_DJBX_Std[1][5];
                    devC13Row.抗风压定级p1压力标准值07 = DAL_Dev.PressSet_KFY_DJBX_Std[1][6];
                    devC13Row.抗风压定级p1压力标准值08 = DAL_Dev.PressSet_KFY_DJBX_Std[1][7];
                    devC13Row.抗风压定级p1压力标准值09 = DAL_Dev.PressSet_KFY_DJBX_Std[1][8];
                    devC13Row.抗风压定级p1压力标准值10 = DAL_Dev.PressSet_KFY_DJBX_Std[1][9];
                    devC13Row.抗风压定级p1压力标准值11 = DAL_Dev.PressSet_KFY_DJBX_Std[1][10];
                    devC13Row.抗风压定级p1压力标准值12 = DAL_Dev.PressSet_KFY_DJBX_Std[1][11];
                    devC13Row.抗风压定级p1压力标准值13 = DAL_Dev.PressSet_KFY_DJBX_Std[1][12];
                    devC13Row.抗风压定级p1压力标准值14 = DAL_Dev.PressSet_KFY_DJBX_Std[1][13];
                    devC13Row.抗风压定级p1压力标准值15 = DAL_Dev.PressSet_KFY_DJBX_Std[1][14];
                    devC13Row.抗风压定级p1压力标准值16 = DAL_Dev.PressSet_KFY_DJBX_Std[1][15];
                    devC13Row.抗风压定级p1压力标准值17 = DAL_Dev.PressSet_KFY_DJBX_Std[1][16];
                    devC13Row.抗风压定级p1压力标准值18 = DAL_Dev.PressSet_KFY_DJBX_Std[1][17];
                    devC13Row.抗风压定级p1压力标准值19 = DAL_Dev.PressSet_KFY_DJBX_Std[1][18];
                    devC13Row.抗风压定级p1压力标准值20 = DAL_Dev.PressSet_KFY_DJBX_Std[1][19];
                    devC13Row.抗风压定级p1压力实际值01 = DAL_Dev.PressSet_KFY_DJBX_False[1][0];
                    devC13Row.抗风压定级p1压力实际值02 = DAL_Dev.PressSet_KFY_DJBX_False[1][1];
                    devC13Row.抗风压定级p1压力实际值03 = DAL_Dev.PressSet_KFY_DJBX_False[1][2];
                    devC13Row.抗风压定级p1压力实际值04 = DAL_Dev.PressSet_KFY_DJBX_False[1][3];
                    devC13Row.抗风压定级p1压力实际值05 = DAL_Dev.PressSet_KFY_DJBX_False[1][4];
                    devC13Row.抗风压定级p1压力实际值06 = DAL_Dev.PressSet_KFY_DJBX_False[1][5];
                    devC13Row.抗风压定级p1压力实际值07 = DAL_Dev.PressSet_KFY_DJBX_False[1][6];
                    devC13Row.抗风压定级p1压力实际值08 = DAL_Dev.PressSet_KFY_DJBX_False[1][7];
                    devC13Row.抗风压定级p1压力实际值09 = DAL_Dev.PressSet_KFY_DJBX_False[1][8];
                    devC13Row.抗风压定级p1压力实际值10 = DAL_Dev.PressSet_KFY_DJBX_False[1][9];
                    devC13Row.抗风压定级p1压力实际值11 = DAL_Dev.PressSet_KFY_DJBX_False[1][10];
                    devC13Row.抗风压定级p1压力实际值12 = DAL_Dev.PressSet_KFY_DJBX_False[1][11];
                    devC13Row.抗风压定级p1压力实际值13 = DAL_Dev.PressSet_KFY_DJBX_False[1][12];
                    devC13Row.抗风压定级p1压力实际值14 = DAL_Dev.PressSet_KFY_DJBX_False[1][13];
                    devC13Row.抗风压定级p1压力实际值15 = DAL_Dev.PressSet_KFY_DJBX_False[1][14];
                    devC13Row.抗风压定级p1压力实际值16 = DAL_Dev.PressSet_KFY_DJBX_False[1][15];
                    devC13Row.抗风压定级p1压力实际值17 = DAL_Dev.PressSet_KFY_DJBX_False[1][16];
                    devC13Row.抗风压定级p1压力实际值18 = DAL_Dev.PressSet_KFY_DJBX_False[1][17];
                    devC13Row.抗风压定级p1压力实际值19 = DAL_Dev.PressSet_KFY_DJBX_False[1][18];
                    devC13Row.抗风压定级p1压力实际值20 = DAL_Dev.PressSet_KFY_DJBX_False[1][19];
                    devC13Row.抗风压定级p2倍数标准值 = DAL_Dev.PressSet_KFY_DJP2_Raito_Std;
                    devC13Row.抗风压定级p3倍数标准值 = DAL_Dev.PressSet_KFY_DJP3_Raito_Std;
                    devC13Row.抗风压定级pmax倍数标准值 = DAL_Dev.PressSet_KFY_DJPmax_Raito_Std;
                    devC13Row.抗风压定级p2压力实际值 = DAL_Dev.PressSet_KFY_DJP2_False;
                    devC13Row.抗风压定级p3压力实际值 = DAL_Dev.PressSet_KFY_DJP3_False;
                    devC13Row.抗风压定级pmax压力实际值 = DAL_Dev.PressSet_KFY_DJPmax_False;
                    devC13Row.抗风压工程p1压力倍数标准值1 = DAL_Dev.PressSet_KFY_GCP1_Raito_Std[0];
                    devC13Row.抗风压工程p1压力倍数标准值2 = DAL_Dev.PressSet_KFY_GCP1_Raito_Std[1];
                    devC13Row.抗风压工程p1压力倍数标准值3 = DAL_Dev.PressSet_KFY_GCP1_Raito_Std[2];
                    devC13Row.抗风压工程p1压力倍数标准值4 = DAL_Dev.PressSet_KFY_GCP1_Raito_Std[3];
                    devC13Row.抗风压工程p2压力倍数标准值 = DAL_Dev.PressSet_KFY_GCP2_Raito_Std;
                    devC13Row.抗风压工程pmax压力倍数标准值 = DAL_Dev.PressSet_KFY_GCPmax_Raito_Std;
                    devC13Row.抗风压工程p1压力实际值1 = DAL_Dev.PressSet_KFY_GCBX_False[1][0];
                    devC13Row.抗风压工程p1压力实际值2 = DAL_Dev.PressSet_KFY_GCBX_False[1][1];
                    devC13Row.抗风压工程p1压力实际值3 = DAL_Dev.PressSet_KFY_GCBX_False[1][2];
                    devC13Row.抗风压工程p1压力实际值4 = DAL_Dev.PressSet_KFY_GCBX_False[1][3];
                    devC13Row.抗风压工程p2压力实际值 = DAL_Dev.PressSet_KFY_GCP2_False;
                    devC13Row.抗风压工程p3压力实际值 = DAL_Dev.PressSet_KFY_GCP3_False;
                    devC13Row.抗风压工程pmax压力实际值 = DAL_Dev.PressSet_KFY_GCPmax_False;
                    devC13Row.抗风压定级p1预加压标准值 = DAL_Dev.PressSet_KFY_DJBX_Std[0][0];
                    devC13Row.抗风压定级p1预加压实际值 = DAL_Dev.PressSet_KFY_DJBX_False[0][0];
                    devC13Row.抗风压工程p1预加压标准值 = DAL_Dev.PressSet_KFY_GCP1_Y_Std;
                    devC13Row.抗风压工程p1预加压实际值 = DAL_Dev.PressSet_KFY_GCBX_False[0][0];

                    C13TableAdapter.Update(C13Table);
                    C13Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// C14压力控制参数Row
        /// </summary>
        private void SavePressCtlSettings()
        {
            string tempSettingsNO = "Using";

            //C14压力控制参数Row
            //若编号在表中不存在，则新建（拷贝DefaultSetting）
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C14压力控制参数Row checkExistC14Row = C14Table.FindBy配置编号(tempSettingsNO);
                if (checkExistC14Row == null)
                {
                    MQZHWL.MQZH_DB_DevDataSet.C14压力控制参数Row defDevC14Row = C14Table.FindBy配置编号("DefaultSetting");
                    MQZHWL.MQZH_DB_DevDataSet.C14压力控制参数Row newDevC14Row = C14Table.NewC14压力控制参数Row();
                    newDevC14Row.ItemArray = (object[])defDevC14Row.ItemArray.Clone();
                    newDevC14Row.配置编号 = tempSettingsNO;
                    C14Table.AddC14压力控制参数Row(newDevC14Row);
                    C14TableAdapter.Update(C14Table);
                    C14Table.AcceptChanges();
                    RaisePropertyChanged(() => C14Table);
                    MessageBox.Show("未找到气压控制参数，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C14压力控制参数Row devC14Row = C14Table.FindBy配置编号(tempSettingsNO);
                if (devC14Row != null)
                {
                    //控制允许偏差
                    devC14Row.PressErrQM_0_50 = DAL_Dev.AllowablePressErrQM[0];
                    devC14Row.PressErrQM_50_100 = DAL_Dev.AllowablePressErrQM[1];
                    devC14Row.PressErrQM_100_500 = DAL_Dev.AllowablePressErrQM[2];
                    devC14Row.PressErrQM_500 = DAL_Dev.AllowablePressErrQM[3];
                    devC14Row.PressErrBig_0_1000 = DAL_Dev.AllowablePressErrBigCY[0];
                    devC14Row.PressErrBig_1000_3000 = DAL_Dev.AllowablePressErrBigCY[1];
                    devC14Row.PressErrBig_3000_5000 = DAL_Dev.AllowablePressErrBigCY[2];
                    devC14Row.PressErrBig_5000 = DAL_Dev.AllowablePressErrBigCY[3];
                    //一般;
                    devC14Row.LoadUpDownSpeed = DAL_Dev.LoadUpDownSpeed;
                    devC14Row.KeepingTime_YJY = DAL_Dev.KeepingTime_YJY;
                    //气密控压;
                    devC14Row.KeepingTime_QMStep = DAL_Dev.KeepingTime_QMStep;
                    //水密控压;
                    devC14Row.KeepingTime_SMPreparePress = DAL_Dev.KeepingTime_SMPreparePress;
                    devC14Row.PhRatioWaveH = DAL_Dev.PhRatioWaveH;
                    devC14Row.PhRatioWaveL = DAL_Dev.PhRatioWaveL;
                    devC14Row.KeepingTime_SMFistStep = DAL_Dev.KeepingTime_SMFistStep;
                    devC14Row.KeepingTime_SMDJ_StepLeft = DAL_Dev.KeepingTime_SMDJ_StepLeft;
                    devC14Row.KeepingTime_SMGC_KKQ = DAL_Dev.KeepingTime_SMGC_KKQ;
                    devC14Row.KeepingTime_SMGC_YKQ = DAL_Dev.KeepingTime_SMGC_YKQ;
                    devC14Row.KeepingTime_SMGC_WKQ = DAL_Dev.KeepingTime_SMGC_WKQ;
                    devC14Row.HighRatioSM = DAL_Dev.HighRatioSM;
                    devC14Row.LowRatioSM = DAL_Dev.LowRatioSM;
                    devC14Row.PlTime_SMGC_Before = DAL_Dev.PlTime_SMGC_Before;

                    //抗风压控压;
                    devC14Row.KeepingTime_KFY_BXstep = DAL_Dev.KeepingTime_KFY_BXstep;
                    devC14Row.KeepingTime_KFY_AQ = DAL_Dev.KeepingTime_KFY_AQ;
                    devC14Row.WaveNum_KFYFF = DAL_Dev.WaveNum_KFYP2;
                    devC14Row.LoadUpDowSpeed_KFRAQ = DAL_Dev.LoadUpDownSpeed_KFRAQ;
                    devC14Row.HighRatioKFY = DAL_Dev.HighRatioKFY;
                    devC14Row.LowRatioKFY = DAL_Dev.LowRatioKFY;

                    C14TableAdapter.Update(C14Table);
                    C14Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// C15位移设定参数Row
        /// </summary>
        private void SaveDevWYSettings()
        {

            string tempSettingsNO = "Using";
            //C15位移设定参数Row
            //若编号在表中不存在，则新建（拷贝DefaultSetting）
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C15位移设定参数Row checkExistC15Row = C15Table.FindBy配置编号(tempSettingsNO);
                if (checkExistC15Row == null)
                {
                    MQZHWL.MQZH_DB_DevDataSet.C15位移设定参数Row defDevC15Row = C15Table.FindBy配置编号("DefaultSetting");
                    MQZHWL.MQZH_DB_DevDataSet.C15位移设定参数Row newDevC15ow = C15Table.NewC15位移设定参数Row();
                    newDevC15ow.ItemArray = (object[])defDevC15Row.ItemArray.Clone();
                    newDevC15ow.配置编号 = tempSettingsNO;
                    C15Table.AddC15位移设定参数Row(newDevC15ow);
                    C15TableAdapter.Update(C15Table);
                    C15Table.AcceptChanges();
                    RaisePropertyChanged(() => C15Table);
                    MessageBox.Show("未找到位移设定参数，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C15位移设定参数Row devC15Row = C15Table.FindBy配置编号(tempSettingsNO);
                if (devC15Row != null)
                {
                    devC15Row.X轴第1级最小位移角 = DAL_Dev.WYJ_Ctl_X[0];
                    devC15Row.X轴第2级最小位移角 = DAL_Dev.WYJ_Ctl_X[1];
                    devC15Row.X轴第3级最小位移角 = DAL_Dev.WYJ_Ctl_X[2];
                    devC15Row.X轴第4级最小位移角 = DAL_Dev.WYJ_Ctl_X[3];
                    devC15Row.X轴第5级最小位移角 = DAL_Dev.WYJ_Ctl_X[4];
                    devC15Row.Y轴第1级最小位移角 = DAL_Dev.WYJ_Ctl_Y[0];
                    devC15Row.Y轴第2级最小位移角 = DAL_Dev.WYJ_Ctl_Y[1];
                    devC15Row.Y轴第3级最小位移角 = DAL_Dev.WYJ_Ctl_Y[2];
                    devC15Row.Y轴第4级最小位移角 = DAL_Dev.WYJ_Ctl_Y[3];
                    devC15Row.Y轴第5级最小位移角 = DAL_Dev.WYJ_Ctl_Y[4];
                    devC15Row.Z轴第1级最小位移量 = DAL_Dev.WY_Ctl_Z[0];
                    devC15Row.Z轴第2级最小位移量 = DAL_Dev.WY_Ctl_Z[1];
                    devC15Row.Z轴第3级最小位移量 = DAL_Dev.WY_Ctl_Z[2];
                    devC15Row.Z轴第4级最小位移量 = DAL_Dev.WY_Ctl_Z[3];
                    devC15Row.Z轴第5级最小位移量 = DAL_Dev.WY_Ctl_Z[4];
                    
                    C15TableAdapter.Update(C15Table);
                    C15Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 保存C16位移控制参数
        /// </summary>
        private void SaveDevWYCtlSettings()
        {

            string tempSettingsNO = "Using";
            //保存C16位移控制参数
            //若编号在表中不存在，则新建（拷贝DefaultSetting）
            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C16位移控制参数Row checkExistC16Row = C16Table.FindBy配置编号(tempSettingsNO);
                if (checkExistC16Row == null)
                {
                    MQZHWL.MQZH_DB_DevDataSet.C16位移控制参数Row defDevC16Row = C16Table.FindBy配置编号("DefaultSetting");
                    MQZHWL.MQZH_DB_DevDataSet.C16位移控制参数Row newDevC16ow = C16Table.NewC16位移控制参数Row();
                    newDevC16ow.ItemArray = (object[])defDevC16Row.ItemArray.Clone();
                    newDevC16ow.配置编号 = tempSettingsNO;
                    C16Table.AddC16位移控制参数Row(newDevC16ow);
                    C16TableAdapter.Update(C16Table);
                    C16Table.AcceptChanges();
                    RaisePropertyChanged(() => C16Table);
                    MessageBox.Show("未找到位移控制参数，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQZHWL.MQZH_DB_DevDataSet.C16位移控制参数Row devC16Row = C16Table.FindBy配置编号(tempSettingsNO);
                if (devC16Row != null)
                {
                    devC16Row.CJBX_XYPeriod = DAL_Dev.CJBX_XYPeriod;
                    devC16Row.CJBX_ZPeriod = DAL_Dev.CJBX_ZPeriod;
                    devC16Row.LenthRatio_X = DAL_Dev.LenthRatio_X;
                    devC16Row.LenthRatio_Y = DAL_Dev.LenthRatio_Y;
                    devC16Row.LenthRatio_Z = DAL_Dev.LenthRatio_Z;
                    devC16Row.PermitErrX = DAL_Dev.PermitErrX;
                    devC16Row.PermitErrY = DAL_Dev.PermitErrY;
                    devC16Row.PermitErrZ = DAL_Dev.PermitErrZ;
                    devC16Row.CorrXRight = DAL_Dev.CorrXRight;
                    devC16Row.CorrXLeft = DAL_Dev.CorrXLeft;
                    devC16Row.CorrYFront = DAL_Dev.CorrYFront;
                    devC16Row.CorrYBack = DAL_Dev.CorrYBack;
                    devC16Row.CorrZUp = DAL_Dev.CorrZUp;
                    devC16Row.CorrZDown = DAL_Dev.CorrZDown;

                    C16TableAdapter.Update(C16Table);
                    C16Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion


        #region 根据消息读写装置数据

        /// <summary>
        /// 保存最后打开的试验编号
        /// </summary>
        /// <param name="msg"></param>
        private void SaveLastExpNOMessage(string msg)
        {
            try
            {
                //更新上次试验编号
                DAL_Dev.ExpNOLast = msg;
                SaveDevBisicParam();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        #endregion
    }
}
