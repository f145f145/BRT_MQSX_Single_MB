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
using System.IO.Ports;
using System.Windows;
using GalaSoft.MvvmLight;
using static MQDFJ_MB.Model.MQZH_Enums;

namespace MQDFJ_MB.DAL
{
    /// <summary>
    /// 装置参数读写操作类
    /// </summary>
    public partial class MQZH_DevDAL : ObservableObject
    {
        #region 装置数据载入
        /// <summary>
        /// 载入装置数据（总）
        /// </summary>
        private void LoadDevSettings()
        {
            //装置基本信息C01
            LoadDevBisicInfo();
            //装置基本信息C02
            LoadDevBisicParam();
            //C03公司信息
            LoadCoInfo();
            //C04串口设置参数表
            LoadComSettings();
           //C05模拟量传感器参数
            LoadAioParam();
            //C06模拟量通道参数
            LoadAIOChannelParam();
            //绑定模拟量参数和通道
            BindAioAndChennel();
            //C07数字量参数
            LoadDIOParam();
            //C08数字量通道参数
            LoadDIOChannelParam();
            //绑定数字量参数和通道
            BindDioAndChennel();
            //C09PID控制参数
            LoadDevPIDSettings();
            //C11装置其它
           // LoadDevOther();
            //C12、13压力设定参数
            LoadPressSettings();
            //C14压力控制参数
            LoadPressCtlSettings();
            //C15位移设定参数
            LoadDevWYSettings();
            //C16位移控制参数
            LoadDevWYCtlSettings();
           
        }


        /// <summary>
        /// 载入C01装置基本信息
        /// </summary>
        private void LoadDevBisicInfo()
        {
            string tempSettingsNO = "Using";

            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C01装置基本信息Row checkExistC01Row = C01Table.FindBy配置编号(tempSettingsNO);
                if (checkExistC01Row == null)
                {
                    MQDFJ_MB.MQZH_DB_DevDataSet.C01装置基本信息Row defDevC01Row = C01Table.FindBy配置编号("DefaultSetting");
                    MQDFJ_MB.MQZH_DB_DevDataSet.C01装置基本信息Row newDevC01Row = C01Table.NewC01装置基本信息Row();
                    newDevC01Row.ItemArray = (object[])defDevC01Row.ItemArray.Clone();
                    newDevC01Row.配置编号 = tempSettingsNO;
                    C01Table.AddC01装置基本信息Row(newDevC01Row);
                    C01TableAdapter.Update(C01Table);
                    C01Table.AcceptChanges();
                    RaisePropertyChanged(() => C01Table);
                    MessageBox.Show("未找到在用装置C01装置基本信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C01装置基本信息Row devC01Row = C01Table.FindBy配置编号(tempSettingsNO);
                if (devC01Row != null)
                {
                    PublicData.Dev.DeviceID = devC01Row.装置ID;
                    PublicData.Dev.DeviceName = devC01Row.装置名称;
                    PublicData.Dev.DeviceSerialNO = devC01Row.出厂编号;
                    PublicData.Dev.DeviceTypeNO = devC01Row.设备型号;
                    PublicData.Dev.DeviceKFYStd = devC01Row.抗风压试验方法标准;
                    PublicData.Dev.DeviceQMStd = devC01Row.气密试验方法标准;
                    PublicData.Dev.DeviceSMStd = devC01Row.水密试验方法标准;
                    PublicData.Dev.DeviceCJBXStd = devC01Row.层间变形试验方法标准;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 载入C02装置基本参数
        /// </summary>
        private void LoadDevBisicParam()
        {
            string tempSettingsNO = "Using";

            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C02装置基本参数Row checkExistC02Row = C02Table.FindBy配置编号(tempSettingsNO);
                if (checkExistC02Row == null)
                {
                    MQDFJ_MB.MQZH_DB_DevDataSet.C02装置基本参数Row defDevC02Row = C02Table.FindBy配置编号("DefaultSetting");
                    MQDFJ_MB.MQZH_DB_DevDataSet.C02装置基本参数Row newDevC02Row = C02Table.NewC02装置基本参数Row();
                    newDevC02Row.ItemArray = (object[])defDevC02Row.ItemArray.Clone();
                    newDevC02Row.配置编号 = tempSettingsNO;
                    C02Table.AddC02装置基本参数Row(newDevC02Row);
                    C02TableAdapter.Update(C02Table);
                    C02Table.AcceptChanges();
                    RaisePropertyChanged(() => C02Table);
                    MessageBox.Show("未找到在用装置C02装置基本参数，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C02装置基本参数Row devC02Row = C02Table.FindBy配置编号(tempSettingsNO);
                if (devC02Row != null)
                {
                    PublicData.Dev.PlotPeriod = devC02Row.绘图更新周期;
                    PublicData.Dev.PointsPerLine = devC02Row.单曲线总点数;
                    PublicData.Dev.UseCSFG1 = devC02Row.UseCSFG1;
                    PublicData.Dev.WithCSFG1 = devC02Row.WithCSFG1;
                    PublicData.Dev.GJ_FG1 = devC02Row.GJ_FG1;
                    PublicData.Dev.DFNo_FG1 = devC02Row.DFNo_FG1;
                    PublicData.Dev.FSNo_FG1 = devC02Row.FSNo_FG1;
                    PublicData.Dev.UseCSFG2 = devC02Row.UseCSFG2;
                    PublicData.Dev.WithCSFG2 = devC02Row.WithCSFG2;
                    PublicData.Dev.GJ_FG2 = devC02Row.GJ_FG2;
                    PublicData.Dev.DFNo_FG2 = devC02Row.DFNo_FG2;
                    PublicData.Dev.FSNo_FG2 = devC02Row.FSNo_FG2;
                    PublicData.Dev.UseCSFG3 = devC02Row.UseCSFG3;
                    PublicData.Dev.WithCSFG3 = devC02Row.WithCSFG3;
                    PublicData.Dev.GJ_FG3 = devC02Row.GJ_FG3;
                    PublicData.Dev.DFNo_FG3 = devC02Row.DFNo_FG3;
                    PublicData.Dev.FSNo_FG3 = devC02Row.FSNo_FG3;
                    PublicData.Dev.GJ_SG = devC02Row.水管管径;
                    PublicData.Dev.H_HDK = devC02Row.活动框高度;
                    PublicData.Dev.W_HDK = devC02Row.活动框宽度;
                    PublicData.Dev.IsUseTHP = devC02Row.是否使用三合一;
                    PublicData.Dev.THPType = devC02Row.THPType;
                    PublicData.Dev.IsWithSLL = devC02Row.有电子流量计;
                    PublicData.Dev.IsWithCYM = devC02Row.有中压差传感器;
                    PublicData.Dev.WYQty = devC02Row.位移尺数量;
                    PublicData.Dev.WYNOList[0] = devC02Row.X轴位移尺编号;
                    PublicData.Dev.WYNOList[1] = devC02Row.Y轴位移尺编号左;
                    PublicData.Dev.WYNOList[2] = devC02Row.Y轴位移尺编号中;
                    PublicData.Dev.WYNOList[3] = devC02Row.Y轴位移尺编号右;
                    PublicData.Dev.WYNOList[4] = devC02Row.Z轴位移尺编号左;
                    PublicData.Dev.WYNOList[5] = devC02Row.Z轴位移尺编号中;
                    PublicData.Dev.WYNOList[6] = devC02Row.Z轴位移尺编号右;
                    PublicData.Dev.ExpNOLast = devC02Row.最后试验编号;
                    PublicData.Dev.IsLoadLastExpPowerOn = devC02Row.开机是否载入最后试验;

                    PublicData.Dev.IsWYKFYF = devC02Row.抗风压位移取反;
                    PublicData.Dev.PermitChangeData = devC02Row.允许修改数据;
                    PublicData.Dev.PhBDZB = devC02Row.波动低压准备频率;
                    PublicData.Dev.CJBXType = devC02Row.层间变形类别;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 载入公司信息C03
        /// </summary>
        private void LoadCoInfo()
        {
            string tempSettingsNO = "Using";

            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C03公司信息Row checkExistC03Row = C03Table.FindBy配置编号(tempSettingsNO);
                if (checkExistC03Row == null)
                {
                    MQDFJ_MB.MQZH_DB_DevDataSet.C03公司信息Row defDevC03Row = C03Table.FindBy配置编号("DefaultSetting");
                    MQDFJ_MB.MQZH_DB_DevDataSet.C03公司信息Row newDevC03Row = C03Table.NewC03公司信息Row();
                    newDevC03Row.ItemArray = (object[])defDevC03Row.ItemArray.Clone();
                    newDevC03Row.配置编号 = tempSettingsNO;
                    C03Table.AddC03公司信息Row(newDevC03Row);
                    C03TableAdapter.Update(C03Table);
                    C03Table.AcceptChanges();
                    RaisePropertyChanged(() => C03Table);
                    MessageBox.Show("未找到在用装置公司信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C03公司信息Row devC03Row = C03Table.FindBy配置编号(tempSettingsNO);
                if (devC03Row != null)
                {
                    PublicData.Dev.MQZH_CompanyShortName = devC03Row.公司简称;
                    PublicData.Dev.MQZH_CompanyName = devC03Row.公司全称;
                    PublicData.Dev.MQZH_CompanyAddr = devC03Row.公司地址;
                    PublicData.Dev.MQZH_ComPostNO = devC03Row.公司邮政编码;
                    PublicData.Dev.MQZH_CompanyTel = devC03Row.公司电话;
                    PublicData.Dev.MQZH_LabAddr = devC03Row.实验室地址;
                    PublicData.Dev.MQZH_LabPostNO = devC03Row.实验室邮政编码;
                    PublicData.Dev.MQZH_LabTel = devC03Row.实验室电话;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 载入C04串口设置参数
        /// </summary>
        private void LoadComSettings()
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
                    MQDFJ_MB.MQZH_DB_DevDataSet.C04串口参数设置Row checkExistC04Row = C04Table.FindBy配置编号(tempC04RowName[i]);
                    if (checkExistC04Row == null)
                    {
                        MQDFJ_MB.MQZH_DB_DevDataSet.C04串口参数设置Row defDevC04Row =
                            C04Table.FindBy配置编号(tempC04RowName[i].ToString() + "Def");
                        MQDFJ_MB.MQZH_DB_DevDataSet.C04串口参数设置Row newDevC04ow = C04Table.NewC04串口参数设置Row();
                        newDevC04ow.ItemArray = (object[])defDevC04Row.ItemArray.Clone();
                        newDevC04ow.配置编号 = tempC04RowName[i];
                        C04Table.AddC04串口参数设置Row(newDevC04ow);
                        C04TableAdapter.Update(C04Table);
                        C04Table.AcceptChanges();
                        RaisePropertyChanged(() => C04Table);
                        MessageBox.Show("未找到" + tempC04RowName[i] + "串口配置参数，已重新建立！", "错误提示");
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
                    PublicData.Dev.DVPCom.PhyPortNO = devC04dvpRow.PortNO;
                    PublicData.Dev.DVPCom.BoundRate = devC04dvpRow.BaudRate;
                    PublicData.Dev.DVPCom.StartBits = devC04dvpRow.StartBits;
                    PublicData.Dev.DVPCom.DataBits = devC04dvpRow.DataBits;
                    PublicData.Dev.DVPCom.Addr = devC04dvpRow.PlCAddr;
                    PublicData.Dev.DVPCom.PeriodRW = devC04dvpRow.CommRW_Period;
                    PublicData.Dev.DVPCom.WatchDogPeriod = devC04dvpRow.WatchDogReset_Period;
                    PublicData.Dev.DVPCom.Timeout = devC04dvpRow.Timeout;
                    PublicData.Dev.DVPCom.Time_BusyDealy = devC04dvpRow.Time_BusyDealy;
                    PublicData.Dev.DVPCom.CMDRepeat = devC04dvpRow.CMDRepeat;
                    switch (devC04dvpRow.StopBits)
                    {
                        case 0:
                            PublicData.Dev.DVPCom.StopBits = StopBits.None;
                            break;
                        case 1:
                            PublicData.Dev.DVPCom.StopBits = StopBits.One;
                            break;
                        case 2:
                            PublicData.Dev.DVPCom.StopBits = StopBits.Two;
                            break;
                        case 15:
                            PublicData.Dev.DVPCom.StopBits = StopBits.OnePointFive;
                            break;
                    }

                    switch (devC04dvpRow.Parity)
                    {
                        case 0:
                            PublicData.Dev.DVPCom.Parity = Parity.None;
                            break;
                        case 1:
                            PublicData.Dev.DVPCom.Parity = Parity.Odd;
                            break;
                        case 2:
                            PublicData.Dev.DVPCom.Parity = Parity.Even;
                            break;
                        case 10:
                            PublicData.Dev.DVPCom.Parity = Parity.Mark;
                            break;
                        case 20:
                            PublicData.Dev.DVPCom.Parity = Parity.Space;
                            break;
                    }
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
                switch (PublicData.Dev.THPType)
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
                    PublicData.Dev.THPCom.PhyPortNO = devC04thpRow.PortNO;
                    PublicData.Dev.THPCom.BoundRate = devC04thpRow.BaudRate;
                    PublicData.Dev.THPCom.StartBits = devC04thpRow.StartBits;
                    PublicData.Dev.THPCom.DataBits = devC04thpRow.DataBits;
                    PublicData.Dev.THPCom.Addr = devC04thpRow.PlCAddr;
                    PublicData.Dev.THPCom.PeriodRW = devC04thpRow.CommRW_Period;
                    PublicData.Dev.THPCom.WatchDogPeriod = devC04thpRow.WatchDogReset_Period;
                    PublicData.Dev.THPCom.Timeout = devC04thpRow.Timeout;
                    PublicData.Dev.THPCom.Time_BusyDealy = devC04thpRow.Time_BusyDealy;
                    PublicData.Dev.THPCom.CMDRepeat = devC04thpRow.CMDRepeat;
                    switch (devC04thpRow.StopBits)
                    {
                        case 0:
                            PublicData.Dev.THPCom.StopBits = StopBits.None;
                            break;
                        case 1:
                            PublicData.Dev.THPCom.StopBits = StopBits.One;
                            break;
                        case 2:
                            PublicData.Dev.THPCom.StopBits = StopBits.Two;
                            break;
                        case 15:
                            PublicData.Dev.THPCom.StopBits = StopBits.OnePointFive;
                            break;
                    }

                    switch (devC04thpRow.Parity)
                    {
                        case 0:
                            PublicData.Dev.THPCom.Parity = Parity.None;
                            break;
                        case 1:
                            PublicData.Dev.THPCom.Parity = Parity.Odd;
                            break;
                        case 2:
                            PublicData.Dev.THPCom.Parity = Parity.Even;
                            break;
                        case 10:
                            PublicData.Dev.THPCom.Parity = Parity.Mark;
                            break;
                        case 20:
                            PublicData.Dev.THPCom.Parity = Parity.Space;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 载入C05模拟量参数
        /// </summary>
        private void LoadAioParam()
        {
            #region 模拟量输入部分

            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC05AIINRowName = new string[24]
            {
                "WY01", "WY02", "WY03", "WY04", "WY05", "WY06", "WY07", "WY08", "WY09", "WY10", "WY11", "WY12",
                "CY01", "CY02", "CY03", "FS01", "FS02", "FS03", "SLS", "T_THP", "H_THP", "P_THP", "PL1View", "PL2View"
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
                        MQZH_DB_DevDataSet.C05模拟量参数Row newDevC05Row = C05Table.NewC05模拟量参数Row();
                        newDevC05Row.ItemArray = (object[])defDevC05Row.ItemArray.Clone();
                        newDevC05Row.SingalNO = tempC05AIINRowName[i];
                        C05Table.AddC05模拟量参数Row(newDevC05Row);
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
            //载入模拟量配置参数
            for (int i = 0; i < tempC05AIINRowName.Length; i++)
            {
                try
                {
                    MQZH_DB_DevDataSet.C05模拟量参数Row devC05aiRow = C05Table.FindBySingalNO(tempC05AIINRowName[i]);
                    if (devC05aiRow != null)
                    {
                        //基本参数
                        PublicData.Dev.AIList[i].SingalNO = devC05aiRow.SingalNO;
                        PublicData.Dev.AIList[i].SingalName = devC05aiRow.SingalName;
                        PublicData.Dev.AIList[i].SingalUnit = devC05aiRow.SingalUnit;
                        PublicData.Dev.AIList[i].IsOutType = devC05aiRow.IsOutType;
                        PublicData.Dev.AIList[i].InfType = devC05aiRow.InfType;
                        PublicData.Dev.AIList[i].ElecSig_Unit = devC05aiRow.ElecSig_Unit;
                        PublicData.Dev.AIList[i].ElecSigLowerRange = devC05aiRow.ElecSig_LowerRange;
                        PublicData.Dev.AIList[i].ElecSigUpperRange = devC05aiRow.ElecSig_UpperRange;
                        PublicData.Dev.AIList[i].SingalLowerRange = devC05aiRow.SingalLowerRange;
                        PublicData.Dev.AIList[i].SingalUpperRange = devC05aiRow.SingalUpperRange;
                        PublicData.Dev.AIList[i].ZeroCalValue = devC05aiRow.ZeroCalValue;
                        PublicData.Dev.AIList[i].KCalValue = devC05aiRow.KCalValue;
                        PublicData.Dev.AIList[i].ModulNO = devC05aiRow.ModulNO;
                        PublicData.Dev.AIList[i].ChannelSerialNO = devC05aiRow.ChannelSerialNO;
                        PublicData.Dev.AIList[i].ConvRatio = devC05aiRow.ConvRatio;
                        PublicData.Dev.AIList[i].IsOutType = false;
                        //标定点启用
                        PublicData.Dev.AIList[i].CalPoints[0].IsUse = devC05aiRow.Use_Corr1;
                        PublicData.Dev.AIList[i].CalPoints[1].IsUse = devC05aiRow.Use_Corr2;
                        PublicData.Dev.AIList[i].CalPoints[2].IsUse = devC05aiRow.Use_Corr3;
                        PublicData.Dev.AIList[i].CalPoints[3].IsUse = devC05aiRow.Use_Corr4;
                        PublicData.Dev.AIList[i].CalPoints[4].IsUse = devC05aiRow.Use_Corr5;
                        PublicData.Dev.AIList[i].CalPoints[5].IsUse = devC05aiRow.Use_Corr6;
                        PublicData.Dev.AIList[i].CalPoints[6].IsUse = devC05aiRow.Use_Corr7;
                        PublicData.Dev.AIList[i].CalPoints[7].IsUse = devC05aiRow.Use_Corr8;
                        PublicData.Dev.AIList[i].CalPoints[8].IsUse = devC05aiRow.Use_Corr9;
                        PublicData.Dev.AIList[i].CalPoints[9].IsUse = devC05aiRow.Use_Corr10;
                        PublicData.Dev.AIList[i].CalPoints[10].IsUse = devC05aiRow.Use_Corr11;
                        //标定点标准值
                        PublicData.Dev.AIList[i].CalPoints[0].StdValue = devC05aiRow.Sensor_Corr1_StdValue;
                        PublicData.Dev.AIList[i].CalPoints[1].StdValue = devC05aiRow.Sensor_Corr2_StdValue;
                        PublicData.Dev.AIList[i].CalPoints[2].StdValue = devC05aiRow.Sensor_Corr3_StdValue;
                        PublicData.Dev.AIList[i].CalPoints[3].StdValue = devC05aiRow.Sensor_Corr4_StdValue;
                        PublicData.Dev.AIList[i].CalPoints[4].StdValue = devC05aiRow.Sensor_Corr5_StdValue;
                        PublicData.Dev.AIList[i].CalPoints[5].StdValue = devC05aiRow.Sensor_Corr6_StdValue;
                        PublicData.Dev.AIList[i].CalPoints[6].StdValue = devC05aiRow.Sensor_Corr7_StdValue;
                        PublicData.Dev.AIList[i].CalPoints[7].StdValue = devC05aiRow.Sensor_Corr8_StdValue;
                        PublicData.Dev.AIList[i].CalPoints[8].StdValue = devC05aiRow.Sensor_Corr9_StdValue;
                        PublicData.Dev.AIList[i].CalPoints[9].StdValue = devC05aiRow.Sensor_Corr10_StdValue;
                        PublicData.Dev.AIList[i].CalPoints[10].StdValue = devC05aiRow.Sensor_Corr11_StdValue;
                        //标定点显示值
                        PublicData.Dev.AIList[i].CalPoints[0].ViewValue = devC05aiRow.Sensor_Corr1_ViewValue;
                        PublicData.Dev.AIList[i].CalPoints[1].ViewValue = devC05aiRow.Sensor_Corr2_ViewValue;
                        PublicData.Dev.AIList[i].CalPoints[2].ViewValue = devC05aiRow.Sensor_Corr3_ViewValue;
                        PublicData.Dev.AIList[i].CalPoints[3].ViewValue = devC05aiRow.Sensor_Corr4_ViewValue;
                        PublicData.Dev.AIList[i].CalPoints[4].ViewValue = devC05aiRow.Sensor_Corr5_ViewValue;
                        PublicData.Dev.AIList[i].CalPoints[5].ViewValue = devC05aiRow.Sensor_Corr6_ViewValue;
                        PublicData.Dev.AIList[i].CalPoints[6].ViewValue = devC05aiRow.Sensor_Corr7_ViewValue;
                        PublicData.Dev.AIList[i].CalPoints[7].ViewValue = devC05aiRow.Sensor_Corr8_ViewValue;
                        PublicData.Dev.AIList[i].CalPoints[8].ViewValue = devC05aiRow.Sensor_Corr9_ViewValue;
                        PublicData.Dev.AIList[i].CalPoints[9].ViewValue = devC05aiRow.Sensor_Corr10_ViewValue;
                        PublicData.Dev.AIList[i].CalPoints[10].ViewValue = devC05aiRow.Sensor_Corr11_ViewValue;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            #endregion

            #region 模拟量输出部分

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
                        MQZH_DB_DevDataSet.C05模拟量参数Row newDevC05Row = C05Table.NewC05模拟量参数Row();
                        newDevC05Row.ItemArray = (object[])defDevC05Row.ItemArray.Clone();
                        newDevC05Row.SingalNO = tempC05AORowName[i];
                        C05Table.AddC05模拟量参数Row(newDevC05Row);
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
                    MQZH_DB_DevDataSet.C05模拟量参数Row devC05plRow = C05Table.FindBySingalNO(tempC05AORowName[i]);
                    if (devC05plRow != null)
                    {
                        //基本参数
                        PublicData.Dev.AOList[i].SingalNO = devC05plRow.SingalNO;
                        PublicData.Dev.AOList[i].SingalName = devC05plRow.SingalName;
                        PublicData.Dev.AOList[i].SingalUnit = devC05plRow.SingalUnit;
                        PublicData.Dev.AOList[i].IsOutType = devC05plRow.IsOutType;
                        PublicData.Dev.AOList[i].InfType = devC05plRow.InfType;
                        PublicData.Dev.AOList[i].ElecSig_Unit = devC05plRow.ElecSig_Unit;
                        PublicData.Dev.AOList[i].ElecSigLowerRange = devC05plRow.ElecSig_LowerRange;
                        PublicData.Dev.AOList[i].ElecSigUpperRange = devC05plRow.ElecSig_UpperRange;
                        PublicData.Dev.AOList[i].SingalLowerRange = devC05plRow.SingalLowerRange;
                        PublicData.Dev.AOList[i].SingalUpperRange = devC05plRow.SingalUpperRange;
                        PublicData.Dev.AOList[i].ZeroCalValue = devC05plRow.ZeroCalValue;
                        PublicData.Dev.AOList[i].KCalValue = devC05plRow.KCalValue;
                        PublicData.Dev.AOList[i].ModulNO = devC05plRow.ModulNO;
                        PublicData.Dev.AOList[i].ChannelSerialNO = devC05plRow.ChannelSerialNO;
                        PublicData.Dev.AOList[i].ConvRatio = devC05plRow.ConvRatio;
                        PublicData.Dev.AIList[i].IsOutType = true;
                        //标定点启用
                        PublicData.Dev.AOList[i].CalPoints[0].IsUse = devC05plRow.Use_Corr1;
                        PublicData.Dev.AOList[i].CalPoints[1].IsUse = devC05plRow.Use_Corr2;
                        PublicData.Dev.AOList[i].CalPoints[2].IsUse = devC05plRow.Use_Corr3;
                        PublicData.Dev.AOList[i].CalPoints[3].IsUse = devC05plRow.Use_Corr4;
                        PublicData.Dev.AOList[i].CalPoints[4].IsUse = devC05plRow.Use_Corr5;
                        PublicData.Dev.AOList[i].CalPoints[5].IsUse = devC05plRow.Use_Corr6;
                        PublicData.Dev.AOList[i].CalPoints[6].IsUse = devC05plRow.Use_Corr7;
                        PublicData.Dev.AOList[i].CalPoints[7].IsUse = devC05plRow.Use_Corr8;
                        PublicData.Dev.AOList[i].CalPoints[8].IsUse = devC05plRow.Use_Corr9;
                        PublicData.Dev.AOList[i].CalPoints[9].IsUse = devC05plRow.Use_Corr10;
                        PublicData.Dev.AOList[i].CalPoints[10].IsUse = devC05plRow.Use_Corr11;
                        //标定点标准值
                        PublicData.Dev.AOList[i].CalPoints[0].StdValue = devC05plRow.Sensor_Corr1_StdValue;
                        PublicData.Dev.AOList[i].CalPoints[1].StdValue = devC05plRow.Sensor_Corr2_StdValue;
                        PublicData.Dev.AOList[i].CalPoints[2].StdValue = devC05plRow.Sensor_Corr3_StdValue;
                        PublicData.Dev.AOList[i].CalPoints[3].StdValue = devC05plRow.Sensor_Corr4_StdValue;
                        PublicData.Dev.AOList[i].CalPoints[4].StdValue = devC05plRow.Sensor_Corr5_StdValue;
                        PublicData.Dev.AOList[i].CalPoints[5].StdValue = devC05plRow.Sensor_Corr6_StdValue;
                        PublicData.Dev.AOList[i].CalPoints[6].StdValue = devC05plRow.Sensor_Corr7_StdValue;
                        PublicData.Dev.AOList[i].CalPoints[7].StdValue = devC05plRow.Sensor_Corr8_StdValue;
                        PublicData.Dev.AOList[i].CalPoints[8].StdValue = devC05plRow.Sensor_Corr9_StdValue;
                        PublicData.Dev.AOList[i].CalPoints[9].StdValue = devC05plRow.Sensor_Corr10_StdValue;
                        PublicData.Dev.AOList[i].CalPoints[10].StdValue = devC05plRow.Sensor_Corr11_StdValue;
                        //标定点显示值
                        PublicData.Dev.AOList[i].CalPoints[i].ViewValue = devC05plRow.Sensor_Corr1_ViewValue;
                        PublicData.Dev.AOList[i].CalPoints[1].ViewValue = devC05plRow.Sensor_Corr2_ViewValue;
                        PublicData.Dev.AOList[i].CalPoints[2].ViewValue = devC05plRow.Sensor_Corr3_ViewValue;
                        PublicData.Dev.AOList[i].CalPoints[3].ViewValue = devC05plRow.Sensor_Corr4_ViewValue;
                        PublicData.Dev.AOList[i].CalPoints[4].ViewValue = devC05plRow.Sensor_Corr5_ViewValue;
                        PublicData.Dev.AOList[i].CalPoints[5].ViewValue = devC05plRow.Sensor_Corr6_ViewValue;
                        PublicData.Dev.AOList[i].CalPoints[6].ViewValue = devC05plRow.Sensor_Corr7_ViewValue;
                        PublicData.Dev.AOList[i].CalPoints[7].ViewValue = devC05plRow.Sensor_Corr8_ViewValue;
                        PublicData.Dev.AOList[i].CalPoints[8].ViewValue = devC05plRow.Sensor_Corr9_ViewValue;
                        PublicData.Dev.AOList[i].CalPoints[9].ViewValue = devC05plRow.Sensor_Corr10_ViewValue;
                        PublicData.Dev.AOList[i].CalPoints[10].ViewValue = devC05plRow.Sensor_Corr11_ViewValue;
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
        /// 载入C06模拟量通道参数
        /// </summary>
        private void LoadAIOChannelParam()
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

            //载入模拟量通道配置参数
            for (int i = 0; i < 4; i++)
            {
                //04AD1
                try
                {
                    MQZH_DB_DevDataSet.C06模拟量通道参数Row devC06chlRow = C06Table.FindByChannelNO(tempC06ad1chlRowName[i]);
                    if (devC06chlRow != null)
                    {
                        PublicData.Dev.Mod_04AD1.Channels[i].ChannelNO = devC06chlRow.ChannelNO;
                        PublicData.Dev.Mod_04AD1.Channels[i].InfType = devC06chlRow.ColType;
                        PublicData.Dev.Mod_04AD1.Channels[i].ElecSigUnit = devC06chlRow.ElecSig_Unit;
                        PublicData.Dev.Mod_04AD1.Channels[i].ElecSigLowerRange = devC06chlRow.ElecSig_LowerRange;
                        PublicData.Dev.Mod_04AD1.Channels[i].ElecSigUpperRange = devC06chlRow.ElecSig_UpperRange;
                        PublicData.Dev.Mod_04AD1.Channels[i].DataLowerRange = devC06chlRow.Data_LowerRange;
                        PublicData.Dev.Mod_04AD1.Channels[i].DataUpperRange = devC06chlRow.Data_UpperRange;
                        PublicData.Dev.Mod_04AD1.Channels[i].IsUsed = devC06chlRow.IsUsed;
                        PublicData.Dev.Mod_04AD1.Channels[i].IsOutType = devC06chlRow.IsOutType;
                        PublicData.Dev.Mod_04AD1.Channels[i].FitRatio = devC06chlRow.FitRitio;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //04AD2
                try
                {
                    MQZH_DB_DevDataSet.C06模拟量通道参数Row devC06chlRow = C06Table.FindByChannelNO(tempC06ad1chlRowName[i + 4]);
                    if (devC06chlRow != null)
                    {
                        PublicData.Dev.Mod_04AD2.Channels[i].ChannelNO = devC06chlRow.ChannelNO;
                        PublicData.Dev.Mod_04AD2.Channels[i].InfType = devC06chlRow.ColType;
                        PublicData.Dev.Mod_04AD2.Channels[i].ElecSigUnit = devC06chlRow.ElecSig_Unit;
                        PublicData.Dev.Mod_04AD2.Channels[i].ElecSigLowerRange = devC06chlRow.ElecSig_LowerRange;
                        PublicData.Dev.Mod_04AD2.Channels[i].ElecSigUpperRange = devC06chlRow.ElecSig_UpperRange;
                        PublicData.Dev.Mod_04AD2.Channels[i].DataLowerRange = devC06chlRow.Data_LowerRange;
                        PublicData.Dev.Mod_04AD2.Channels[i].DataUpperRange = devC06chlRow.Data_UpperRange;
                        PublicData.Dev.Mod_04AD2.Channels[i].IsUsed = devC06chlRow.IsUsed;
                        PublicData.Dev.Mod_04AD2.Channels[i].IsOutType = devC06chlRow.IsOutType;
                        PublicData.Dev.Mod_04AD2.Channels[i].FitRatio = devC06chlRow.FitRitio;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //04AD3
                try
                {
                    MQZH_DB_DevDataSet.C06模拟量通道参数Row devC06chlRow = C06Table.FindByChannelNO(tempC06ad1chlRowName[i + 8]);
                    if (devC06chlRow != null)
                    {
                        PublicData.Dev.Mod_04AD3.Channels[i].ChannelNO = devC06chlRow.ChannelNO;
                        PublicData.Dev.Mod_04AD3.Channels[i].InfType = devC06chlRow.ColType;
                        PublicData.Dev.Mod_04AD3.Channels[i].ElecSigUnit = devC06chlRow.ElecSig_Unit;
                        PublicData.Dev.Mod_04AD3.Channels[i].ElecSigLowerRange = devC06chlRow.ElecSig_LowerRange;
                        PublicData.Dev.Mod_04AD3.Channels[i].ElecSigUpperRange = devC06chlRow.ElecSig_UpperRange;
                        PublicData.Dev.Mod_04AD3.Channels[i].DataLowerRange = devC06chlRow.Data_LowerRange;
                        PublicData.Dev.Mod_04AD3.Channels[i].DataUpperRange = devC06chlRow.Data_UpperRange;
                        PublicData.Dev.Mod_04AD3.Channels[i].IsUsed = devC06chlRow.IsUsed;
                        PublicData.Dev.Mod_04AD3.Channels[i].IsOutType = devC06chlRow.IsOutType;
                        PublicData.Dev.Mod_04AD3.Channels[i].FitRatio = devC06chlRow.FitRitio;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //04AD4
                try
                {
                    MQZH_DB_DevDataSet.C06模拟量通道参数Row devC06chlRow = C06Table.FindByChannelNO(tempC06ad1chlRowName[i + 12]);
                    if (devC06chlRow != null)
                    {
                        PublicData.Dev.Mod_04AD4.Channels[i].ChannelNO = devC06chlRow.ChannelNO;
                        PublicData.Dev.Mod_04AD4.Channels[i].InfType = devC06chlRow.ColType;
                        PublicData.Dev.Mod_04AD4.Channels[i].ElecSigUnit = devC06chlRow.ElecSig_Unit;
                        PublicData.Dev.Mod_04AD4.Channels[i].ElecSigLowerRange = devC06chlRow.ElecSig_LowerRange;
                        PublicData.Dev.Mod_04AD4.Channels[i].ElecSigUpperRange = devC06chlRow.ElecSig_UpperRange;
                        PublicData.Dev.Mod_04AD4.Channels[i].DataLowerRange = devC06chlRow.Data_LowerRange;
                        PublicData.Dev.Mod_04AD4.Channels[i].DataUpperRange = devC06chlRow.Data_UpperRange;
                        PublicData.Dev.Mod_04AD4.Channels[i].IsUsed = devC06chlRow.IsUsed;
                        PublicData.Dev.Mod_04AD4.Channels[i].IsOutType = devC06chlRow.IsOutType;
                        PublicData.Dev.Mod_04AD4.Channels[i].FitRatio = devC06chlRow.FitRitio;
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
                        PublicData.Dev.Mod_04AD5.Channels[i].ChannelNO = devC06chlRow.ChannelNO;
                        PublicData.Dev.Mod_04AD5.Channels[i].InfType = devC06chlRow.ColType;
                        PublicData.Dev.Mod_04AD5.Channels[i].ElecSigUnit = devC06chlRow.ElecSig_Unit;
                        PublicData.Dev.Mod_04AD5.Channels[i].ElecSigLowerRange = devC06chlRow.ElecSig_LowerRange;
                        PublicData.Dev.Mod_04AD5.Channels[i].ElecSigUpperRange = devC06chlRow.ElecSig_UpperRange;
                        PublicData.Dev.Mod_04AD5.Channels[i].DataLowerRange = devC06chlRow.Data_LowerRange;
                        PublicData.Dev.Mod_04AD5.Channels[i].DataUpperRange = devC06chlRow.Data_UpperRange;
                        PublicData.Dev.Mod_04AD5.Channels[i].IsUsed = devC06chlRow.IsUsed;
                        PublicData.Dev.Mod_04AD5.Channels[i].IsOutType = devC06chlRow.IsOutType;
                        PublicData.Dev.Mod_04AD5.Channels[i].FitRatio = devC06chlRow.FitRitio;
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
                        PublicData.Dev.Mod_DAView.Channels[i].ChannelNO = devC06chlRow.ChannelNO;
                        PublicData.Dev.Mod_DAView.Channels[i].InfType = devC06chlRow.ColType;
                        PublicData.Dev.Mod_DAView.Channels[i].ElecSigUnit = devC06chlRow.ElecSig_Unit;
                        PublicData.Dev.Mod_DAView.Channels[i].ElecSigLowerRange = devC06chlRow.ElecSig_LowerRange;
                        PublicData.Dev.Mod_DAView.Channels[i].ElecSigUpperRange = devC06chlRow.ElecSig_UpperRange;
                        PublicData.Dev.Mod_DAView.Channels[i].DataLowerRange = devC06chlRow.Data_LowerRange;
                        PublicData.Dev.Mod_DAView.Channels[i].DataUpperRange = devC06chlRow.Data_UpperRange;
                        PublicData.Dev.Mod_DAView.Channels[i].IsUsed = devC06chlRow.IsUsed;
                        PublicData.Dev.Mod_DAView.Channels[i].IsOutType = devC06chlRow.IsOutType;
                        PublicData.Dev.Mod_DAView.Channels[i].FitRatio = devC06chlRow.FitRitio;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
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

            //载入模拟量通道配置参数
            for (int i = 0; i < tempC06da1chlRowName.Length; i++)
            {
                try
                {
                    MQZH_DB_DevDataSet.C06模拟量通道参数Row devC06chlRow = C06Table.FindByChannelNO(tempC06da1chlRowName[i]);
                    if (devC06chlRow != null)
                    {
                        //基本参数
                        PublicData.Dev.Mod_DA.Channels[i].ChannelNO = devC06chlRow.ChannelNO;
                        PublicData.Dev.Mod_DA.Channels[i].InfType = devC06chlRow.ColType;
                        PublicData.Dev.Mod_DA.Channels[i].ElecSigUnit = devC06chlRow.ElecSig_Unit;
                        PublicData.Dev.Mod_DA.Channels[i].ElecSigLowerRange = devC06chlRow.ElecSig_LowerRange;
                        PublicData.Dev.Mod_DA.Channels[i].ElecSigUpperRange = devC06chlRow.ElecSig_UpperRange;
                        PublicData.Dev.Mod_DA.Channels[i].DataLowerRange = devC06chlRow.Data_LowerRange;
                        PublicData.Dev.Mod_DA.Channels[i].DataUpperRange = devC06chlRow.Data_UpperRange;
                        PublicData.Dev.Mod_DA.Channels[i].IsUsed = devC06chlRow.IsUsed;
                        PublicData.Dev.Mod_DA.Channels[i].IsOutType = devC06chlRow.IsOutType;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
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
                        MQZH_DB_DevDataSet.C06模拟量通道参数Row newDevC06ow = C06Table.NewC06模拟量通道参数Row();
                        newDevC06ow.ItemArray = (object[])defDevC06Row.ItemArray.Clone();
                        newDevC06ow.ChannelNO = tempC06thpchlRowName[i];
                        C06Table.AddC06模拟量通道参数Row(newDevC06ow);
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

            //载入模拟量通道配置参数
            for (int i = 0; i < tempC06thpchlRowName.Length; i++)
            {
                try
                {
                    MQZH_DB_DevDataSet.C06模拟量通道参数Row devC06chlRow = C06Table.FindByChannelNO(tempC06thpchlRowName[i]);
                    if (devC06chlRow != null)
                    {
                        //基本参数
                        PublicData.Dev.Mod_THP.Channels[i].ChannelNO = devC06chlRow.ChannelNO;
                        PublicData.Dev.Mod_THP.Channels[i].InfType = devC06chlRow.ColType;
                        PublicData.Dev.Mod_THP.Channels[i].ElecSigUnit = devC06chlRow.ElecSig_Unit;
                        PublicData.Dev.Mod_THP.Channels[i].ElecSigLowerRange = devC06chlRow.ElecSig_LowerRange;
                        PublicData.Dev.Mod_THP.Channels[i].ElecSigUpperRange = devC06chlRow.ElecSig_UpperRange;
                        PublicData.Dev.Mod_THP.Channels[i].DataLowerRange = devC06chlRow.Data_LowerRange;
                        PublicData.Dev.Mod_THP.Channels[i].DataUpperRange = devC06chlRow.Data_UpperRange;
                        PublicData.Dev.Mod_THP.Channels[i].IsUsed = devC06chlRow.IsUsed;
                        PublicData.Dev.Mod_THP.Channels[i].IsOutType = devC06chlRow.IsOutType;
                        PublicData.Dev.Mod_THP.Channels[i].FitRatio = devC06chlRow.FitRitio;
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
        /// 绑定模拟量参数和通道
        /// </summary>
        private void BindAioAndChennel()
        {
            #region 模拟量输入部分
            try
            {
                //位移尺1-12、差压1-2、风速1-3、水流速

                for (int i = 0; i <=3; i++)
                {
                    //位移尺1-4
                    if (PublicData.Dev.AIList[i].ChannelSerialNO > PublicData.Dev.Mod_04AD1.Channels.Count)
                        PublicData.Dev.AIList[i].ChannelSerialNO = PublicData.Dev.Mod_04AD1.Channels.Count;
                    PublicData.Dev.AIList[i].AnalogChannel = PublicData.Dev.Mod_04AD1.Channels[PublicData.Dev.AIList[i].ChannelSerialNO - 1];
                    PublicData.Dev.AIList[i].CalcData_SigBounds();

                    //位移尺5-8
                    if (PublicData.Dev.AIList[i+4].ChannelSerialNO > PublicData.Dev.Mod_04AD2.Channels.Count)
                        PublicData.Dev.AIList[i+4].ChannelSerialNO = PublicData.Dev.Mod_04AD2.Channels.Count;
                    PublicData.Dev.AIList[i+4].AnalogChannel = PublicData.Dev.Mod_04AD2.Channels[PublicData.Dev.AIList[i+4].ChannelSerialNO - 1];
                    PublicData.Dev.AIList[i+4].CalcData_SigBounds();

                    //位移尺9-12
                    if (PublicData.Dev.AIList[i+8].ChannelSerialNO > PublicData.Dev.Mod_04AD3.Channels.Count)
                        PublicData.Dev.AIList[i+8].ChannelSerialNO = PublicData.Dev.Mod_04AD3.Channels.Count;
                    PublicData.Dev.AIList[i+8].AnalogChannel = PublicData.Dev.Mod_04AD3.Channels[PublicData.Dev.AIList[i+8].ChannelSerialNO - 1];
                    PublicData.Dev.AIList[i+8].CalcData_SigBounds();
                }

                //小差压
                if (PublicData.Dev.AIList[12].ChannelSerialNO > PublicData.Dev.Mod_04AD4.Channels.Count)
                    PublicData.Dev.AIList[12].ChannelSerialNO = PublicData.Dev.Mod_04AD4.Channels.Count;
                PublicData.Dev.AIList[12].AnalogChannel = PublicData.Dev.Mod_04AD4.Channels[PublicData.Dev.AIList[12].ChannelSerialNO - 1];
                PublicData.Dev.AIList[12].CalcData_SigBounds();

                //中差压
                if (PublicData.Dev.AIList[13].ChannelSerialNO > PublicData.Dev.Mod_04AD5.Channels.Count)
                    PublicData.Dev.AIList[13].ChannelSerialNO = PublicData.Dev.Mod_04AD5.Channels.Count;
                PublicData.Dev.AIList[13].AnalogChannel = PublicData.Dev.Mod_04AD5.Channels[PublicData.Dev.AIList[13].ChannelSerialNO - 1];
                PublicData.Dev.AIList[13].CalcData_SigBounds();

                //大差压
                if (PublicData.Dev.AIList[14].ChannelSerialNO > PublicData.Dev.Mod_04AD4.Channels.Count)
                    PublicData.Dev.AIList[14].ChannelSerialNO = PublicData.Dev.Mod_04AD4.Channels.Count;
                PublicData.Dev.AIList[14].AnalogChannel = PublicData.Dev.Mod_04AD4.Channels[PublicData.Dev.AIList[14].ChannelSerialNO - 1];
                PublicData.Dev.AIList[14].CalcData_SigBounds();

                //风速1-2
                for (int i = 15; i <= 16; i++)
                {
                    if (PublicData.Dev.AIList[i].ChannelSerialNO > PublicData.Dev.Mod_04AD4.Channels.Count)
                        PublicData.Dev.AIList[i].ChannelSerialNO = PublicData.Dev.Mod_04AD4.Channels.Count;
                    PublicData.Dev.AIList[i].AnalogChannel = PublicData.Dev.Mod_04AD4.Channels[PublicData.Dev.AIList[i].ChannelSerialNO - 1];
                    PublicData.Dev.AIList[i].CalcData_SigBounds();
                }
                //风速3、水流速
                for (int i = 17; i <= 18; i++)
                {
                    if (PublicData.Dev.AIList[i].ChannelSerialNO > PublicData.Dev.Mod_04AD5.Channels.Count)
                        PublicData.Dev.AIList[i].ChannelSerialNO = PublicData.Dev.Mod_04AD5.Channels.Count;
                    PublicData.Dev.AIList[i].AnalogChannel = PublicData.Dev.Mod_04AD5.Channels[PublicData.Dev.AIList[i].ChannelSerialNO - 1];
                    PublicData.Dev.AIList[i].CalcData_SigBounds();
                }

                //三合一
                for (int i = 19; i <=21; i++)
                {
                    if (PublicData.Dev.AIList[i].ChannelSerialNO > PublicData.Dev.Mod_THP.Channels.Count)
                        PublicData.Dev.AIList[i].ChannelSerialNO = PublicData.Dev.Mod_THP.Channels.Count;
                    PublicData.Dev.AIList[i].AnalogChannel = PublicData.Dev.Mod_THP.Channels[PublicData.Dev.AIList[i].ChannelSerialNO - 1];
                    PublicData.Dev.AIList[i].CalcData_SigBounds();
                }

                //频率显示1、2
                for (int i = 22; i <=23; i++)
                {
                    if (PublicData.Dev.AIList[i].ChannelSerialNO > PublicData.Dev.Mod_DAView.Channels.Count)
                        PublicData.Dev.AIList[i].ChannelSerialNO = PublicData.Dev.Mod_DAView.Channels.Count;
                    PublicData.Dev.AIList[i].AnalogChannel = PublicData.Dev.Mod_DAView.Channels[PublicData.Dev.AIList[i].ChannelSerialNO - 1];
                    PublicData.Dev.AIList[i].CalcData_SigBounds();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            #endregion

            #region 模拟量输出部分
            try
            {
                //风机频率、水泵频率
                for (int i = 0; i <=1; i++)
                {
                    if (PublicData.Dev.AOList[i].ChannelSerialNO > PublicData.Dev.Mod_DA.Channels.Count)
                        PublicData.Dev.AOList[i].ChannelSerialNO = PublicData.Dev.Mod_DA.Channels.Count;
                    PublicData.Dev.AOList[i].AnalogChannel = PublicData.Dev.Mod_DA.Channels[PublicData.Dev.AOList[i].ChannelSerialNO - 1];
                    PublicData.Dev.AOList[i].CalcData_SigBounds();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            #endregion
        }

        /// <summary>
        /// 载入C07数字量参数
        /// </summary>
        private void LoadDIOParam()
        {
            #region 数字量输入部分

            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC07DIRowName = new string[32]
            {
                "DF1_Status", "DF2_Status", "DF3_Status", "DF4_Status",
                "CYF_Status", "KM1_Status","SB_Status" , "QB_Status" ,"VF1_Status", "VF2_Status",
                "YYZ_Status", "YYZF_Status" , "XR_Status" ,"XL_Status" , "YF_Status" ,"YB_Status" , "ZU_Status" ,"ZD_Status" ,
                "XDDok_Status", "YDDok_Status","ZDDok_Status",
                "backup_Status","backup_Status","backup_Status","backup_Status","backup_Status","backup_Status","backup_Status","backup_Status","backup_Status",
                 "XX_Status" , "COM_Status"
            };
            for (int i = 0; i < tempC07DIRowName.Length; i++)
            {
                try
                {
                    MQZH_DB_DevDataSet.C07数字量参数Row checkExistC07Row = C07Table.FindBySingalNO(tempC07DIRowName[i]);
                    if (checkExistC07Row == null)
                    {
                        MQZH_DB_DevDataSet.C07数字量参数Row defDevC07Row =
                            C07Table.FindBySingalNO(tempC07DIRowName[i].ToString() + "Def");
                        MQZH_DB_DevDataSet.C07数字量参数Row newDevC07Row = C07Table.NewC07数字量参数Row();
                        newDevC07Row.ItemArray = (object[])defDevC07Row.ItemArray.Clone();
                        newDevC07Row.SingalNO = tempC07DIRowName[i];
                        C07Table.AddC07数字量参数Row(newDevC07Row);
                        C07TableAdapter.Update(C07Table);
                        C07Table.AcceptChanges();
                        RaisePropertyChanged(() => C07Table);
                        MessageBox.Show("未找到" + tempC07DIRowName[i] + "配置参数，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            //载入配置参数
            for (int i = 0; i < tempC07DIRowName.Length; i++)
            {
                try
                {
                    MQZH_DB_DevDataSet.C07数字量参数Row devC07diRow = C07Table.FindBySingalNO(tempC07DIRowName[i]);
                    if (devC07diRow != null)
                    {
                        //基本参数
                        PublicData.Dev.DIList[i].SingalNO = devC07diRow.SingalNO;
                        PublicData.Dev.DIList[i].SingalName = devC07diRow.SingalName;
                        PublicData.Dev.DIList[i].IsOutType = devC07diRow.IsOutType;
                        PublicData.Dev.DIList[i].ModulNO = devC07diRow.ModulNO;
                        PublicData.Dev.DIList[i].ChannelSerialNO = devC07diRow.ChannelSerialNO;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            #endregion

            #region 数字量输出部分

            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC07DORowName = new string[18]
            {
                "DF1_Ctl" , "DF2_Ctl" , "DF3_Ctl" , "DF4_Ctl" ,"CY_Ctl",
                "KM1_Ctl" ,"SB_Ctl", "QB_Ctl","BP1_Ctl" , "BP2_Ctl" ,
                "YYZ_Ctl" , "YYZF_Ctl",
                "XR_Ctl","XL_Ctl", "YF_Ctl","YB_Ctl", "ZU_Ctl","ZD_Ctl"
            };
            for (int i = 0; i < tempC07DORowName.Length; i++)
            {
                try
                {
                    MQZH_DB_DevDataSet.C07数字量参数Row checkExistC07Row = C07Table.FindBySingalNO(tempC07DORowName[i]);
                    if (checkExistC07Row == null)
                    {
                        MQZH_DB_DevDataSet.C07数字量参数Row defDevC07Row =
                            C07Table.FindBySingalNO(tempC07DORowName[i].ToString() + "Def");
                        MQZH_DB_DevDataSet.C07数字量参数Row newDevC07Row = C07Table.NewC07数字量参数Row();
                        newDevC07Row.ItemArray = (object[])defDevC07Row.ItemArray.Clone();
                        newDevC07Row.SingalNO = tempC07DORowName[i];
                        C07Table.AddC07数字量参数Row(newDevC07Row);
                        C07TableAdapter.Update(C07Table);
                        C07Table.AcceptChanges();
                        RaisePropertyChanged(() => C07Table);
                        MessageBox.Show("未找到" + tempC07DORowName[i] + "配置参数，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            //载入配置参数
            for (int i = 0; i < tempC07DORowName.Length; i++)	
            {
                try
                {
                    MQZH_DB_DevDataSet.C07数字量参数Row devC07doRow = C07Table.FindBySingalNO(tempC07DORowName[i]);
                    if (devC07doRow != null)
                    {
                        PublicData.Dev.DOList[i].SingalNO = devC07doRow.SingalNO;
                        PublicData.Dev.DOList[i].SingalName = devC07doRow.SingalName;
                        PublicData.Dev.DOList[i].IsOutType = devC07doRow.IsOutType;
                        PublicData.Dev.DOList[i].ModulNO = devC07doRow.ModulNO;
                        PublicData.Dev.DOList[i].ChannelSerialNO = devC07doRow.ChannelSerialNO;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                #endregion
            }
        }

        /// <summary>
        /// 载入C08数字量通道参数
        /// </summary>
        private void LoadDIOChannelParam()
        {
            #region 虚拟DI通道
            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC08dichlRowName = new string[32]
            {
                "XNDI-1", "XNDI-2", "XNDI-3", "XNDI-4",
                "XNDI-5", "XNDI-6", "XNDI-7", "XNDI-8",
                "XNDI-9", "XNDI-10", "XNDI-11", "XNDI-12",
                "XNDI-13", "XNDI-14", "XNDI-15", "XNDI-16",
                "XNDI-17", "XNDI-18", "XNDI-19", "XNDI-20",
                "XNDI-21", "XNDI-22", "XNDI-23", "XNDI-24",
                "XNDI-25", "XNDI-26", "XNDI-27", "XNDI-28",
                "XNDI-29", "XNDI-30", "XNDI-31", "XNDI-32"
            };
            for (int i = 0; i < tempC08dichlRowName.Length; i++)
            {
                try
                {
                    MQZH_DB_DevDataSet.C08数字量通道参数Row checkExistC08Row = C08Table.FindByChannelNO(tempC08dichlRowName[i]);
                    if (checkExistC08Row == null)
                    {
                        MQZH_DB_DevDataSet.C08数字量通道参数Row defDevC08Row = C08Table.FindByChannelNO(tempC08dichlRowName[i].ToString() + "Def");
                        MQZH_DB_DevDataSet.C08数字量通道参数Row newDevC08Row = C08Table.NewC08数字量通道参数Row();
                        newDevC08Row.ItemArray = (object[])defDevC08Row.ItemArray.Clone();
                        newDevC08Row.ChannelNO = tempC08dichlRowName[i];
                        C08Table.AddC08数字量通道参数Row(newDevC08Row);
                        C08TableAdapter.Update(C08Table);
                        C08Table.AcceptChanges();
                        RaisePropertyChanged(() => C08Table);
                        MessageBox.Show("未找到" + tempC08dichlRowName[i] + "通道配置参数，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            //载入数字量通道配置参数
            for (int i = 0; i < tempC08dichlRowName.Length; i++)
            {
                try
                {
                    MQZH_DB_DevDataSet.C08数字量通道参数Row devC08chlRow = C08Table.FindByChannelNO(tempC08dichlRowName[i]);
                    if (devC08chlRow != null)
                    {
                        PublicData.Dev.Mod_XNDI.Channels[i].ChannelNO = devC08chlRow.ChannelNO;
                        PublicData.Dev.Mod_XNDI.Channels[i].IsUsed = devC08chlRow.IsUsed;
                        PublicData.Dev.Mod_XNDI.Channels[i].IsOutType = devC08chlRow.IsOutType;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            #endregion

            #region 虚拟DO通道
            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC08dochlRowName = new string[18]
            {
                "XNDO-1", "XNDO-2", "XNDO-3", "XNDO-4",
                "XNDO-5", "XNDO-6", "XNDO-7", "XNDO-8",
                "XNDO-9", "XNDO-10", "XNDO-11", "XNDO-12",
                "XNDO-13", "XNDO-14", "XNDO-15", "XNDO-16",
                "XNDO-17", "XNDO-18"
            };
            for (int i = 0; i < tempC08dochlRowName.Length; i++)
            {
                try
                {
                    MQZH_DB_DevDataSet.C08数字量通道参数Row checkExistC08Row = C08Table.FindByChannelNO(tempC08dochlRowName[i]);
                    if (checkExistC08Row == null)
                    {
                        MQZH_DB_DevDataSet.C08数字量通道参数Row defDevC08Row = C08Table.FindByChannelNO(tempC08dochlRowName[i].ToString() + "Def");
                        MQZH_DB_DevDataSet.C08数字量通道参数Row newDevC08Row = C08Table.NewC08数字量通道参数Row();
                        newDevC08Row.ItemArray = (object[])defDevC08Row.ItemArray.Clone();
                        newDevC08Row.ChannelNO = tempC08dochlRowName[i];
                        C08Table.AddC08数字量通道参数Row(newDevC08Row);
                        C08TableAdapter.Update(C08Table);
                        C08Table.AcceptChanges();
                        RaisePropertyChanged(() => C08Table);
                        MessageBox.Show("未找到" + tempC08dochlRowName[i] + "通道配置参数，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            //载入数字量通道配置参数
            for (int i = 0; i < tempC08dochlRowName.Length; i++)
            {
                try
                {
                    MQZH_DB_DevDataSet.C08数字量通道参数Row devC08chlRow = C08Table.FindByChannelNO(tempC08dochlRowName[i]);
                    if (devC08chlRow != null)
                    {
                        PublicData.Dev.Mod_XNDO.Channels[i].ChannelNO = devC08chlRow.ChannelNO;
                        PublicData.Dev.Mod_XNDO.Channels[i].IsUsed = devC08chlRow.IsUsed;
                        PublicData.Dev.Mod_XNDO.Channels[i].IsOutType = devC08chlRow.IsOutType;
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
        /// 绑定数字量参数和通道
        /// </summary>
        private void BindDioAndChennel()
        {
            #region 数字量输入部分
            try
            {
                for (int i = 0; i < PublicData.Dev.DIList.Count; i++)
                {
                    if (PublicData.Dev.DIList[i].ChannelSerialNO > PublicData.Dev.Mod_XNDI.Channels.Count)
                        PublicData.Dev.DIList[i].ChannelSerialNO = PublicData.Dev.Mod_XNDI.Channels.Count;
                    PublicData.Dev.DIList[i].DigitalChannel = PublicData.Dev.Mod_XNDI.Channels[PublicData.Dev.DIList[i].ChannelSerialNO - 1];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            #endregion

            #region 数字量输出部分
            try
            {
                for (int i = 0; i < PublicData.Dev.DOList.Count; i++)
                {
                    if (PublicData.Dev.DOList[i].ChannelSerialNO > PublicData.Dev.Mod_XNDO.Channels.Count)
                        PublicData.Dev.DOList[i].ChannelSerialNO = PublicData.Dev.Mod_XNDO.Channels.Count;
                    PublicData.Dev.DOList[i].DigitalChannel = PublicData.Dev.Mod_XNDO.Channels[PublicData.Dev.DOList[i].ChannelSerialNO - 1];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            #endregion
        }

        /// <summary>
        /// 载入C09PID设置参数
        /// </summary>
        private void LoadDevPIDSettings()
        {
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
                    MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row checkExistC09Row = C09Table.FindBy配置编号(tempC09RowName[i]);
                    if (checkExistC09Row == null)
                    {
                        MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row defDevC09Row =
                            C09Table.FindBy配置编号(tempC09RowName[i].ToString() + "Def");
                        MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row newDevC09ow = C09Table.NewC09PID控制参数Row();
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
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0911Row = C09Table.FindBy配置编号(tempC09RowName[0]);
                if (devC0911Row != null)
                {
                    PublicData.Dev.PID11.ControllerName = devC0911Row.PIDName;
                    PublicData.Dev.PID11.Kp = devC0911Row.Kp;
                    PublicData.Dev.PID11.Ki = devC0911Row.Ki;
                    PublicData.Dev.PID11.Kd = devC0911Row.Kd;
                    PublicData.Dev.PID11.U_UpperBound = devC0911Row.U_UpperBound;
                    PublicData.Dev.PID11.U_LowerBound = devC0911Row.U_LowerBound;
                    //PublicData.Dev.PID11.T = devC0911Row.T;
                    PublicData.Dev.PID11.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0911Row.PIDType;
                    PublicData.Dev.PID11.U_IMax_Limit = devC0911Row.U_IMax_Limit;
                    PublicData.Dev.PID11.ErrBound_IntegralSeparate = devC0911Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID11.Is_Kp_Used = devC0911Row.Is_Kp_Used;
                    PublicData.Dev.PID11.Is_Ki_Used = devC0911Row.Is_Ki_Used;
                    PublicData.Dev.PID11.Is_Kd_Used = devC0911Row.Is_Kd_Used;
                    PublicData.Dev.PID11.Is_U_Limit = devC0911Row.Is_U_Limit;
                    PublicData.Dev.PID11.Is_UILimit_Used = devC0911Row.Is_UILimit_Used;
                    PublicData.Dev.PID11.Is_ISeparate_Used = devC0911Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID12
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0912Row = C09Table.FindBy配置编号(tempC09RowName[1]);
                if (devC0912Row != null)
                {
                    PublicData.Dev.PID12.ControllerName = devC0912Row.PIDName;
                    PublicData.Dev.PID12.Kp = devC0912Row.Kp;
                    PublicData.Dev.PID12.Ki = devC0912Row.Ki;
                    PublicData.Dev.PID12.Kd = devC0912Row.Kd;
                    PublicData.Dev.PID12.U_UpperBound = devC0912Row.U_UpperBound;
                    PublicData.Dev.PID12.U_LowerBound = devC0912Row.U_LowerBound;
                    //PublicData.Dev.PID12.T = devC0912Row.T;
                    PublicData.Dev.PID12.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0912Row.PIDType;
                    PublicData.Dev.PID12.U_IMax_Limit = devC0912Row.U_IMax_Limit;
                    PublicData.Dev.PID12.ErrBound_IntegralSeparate = devC0912Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID12.Is_Kp_Used = devC0912Row.Is_Kp_Used;
                    PublicData.Dev.PID12.Is_Ki_Used = devC0912Row.Is_Ki_Used;
                    PublicData.Dev.PID12.Is_Kd_Used = devC0912Row.Is_Kd_Used;
                    PublicData.Dev.PID12.Is_U_Limit = devC0912Row.Is_U_Limit;
                    PublicData.Dev.PID12.Is_UILimit_Used = devC0912Row.Is_UILimit_Used;
                    PublicData.Dev.PID12.Is_ISeparate_Used = devC0912Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID13
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0913Row = C09Table.FindBy配置编号(tempC09RowName[2]);
                if (devC0913Row != null)
                {
                    PublicData.Dev.PID13.ControllerName = devC0913Row.PIDName;
                    PublicData.Dev.PID13.Kp = devC0913Row.Kp;
                    PublicData.Dev.PID13.Ki = devC0913Row.Ki;
                    PublicData.Dev.PID13.Kd = devC0913Row.Kd;
                    PublicData.Dev.PID13.U_UpperBound = devC0913Row.U_UpperBound;
                    PublicData.Dev.PID13.U_LowerBound = devC0913Row.U_LowerBound;
                    //PublicData.Dev.PID13.T = devC0913Row.T;
                    PublicData.Dev.PID13.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0913Row.PIDType;
                    PublicData.Dev.PID13.U_IMax_Limit = devC0913Row.U_IMax_Limit;
                    PublicData.Dev.PID13.ErrBound_IntegralSeparate = devC0913Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID13.Is_Kp_Used = devC0913Row.Is_Kp_Used;
                    PublicData.Dev.PID13.Is_Ki_Used = devC0913Row.Is_Ki_Used;
                    PublicData.Dev.PID13.Is_Kd_Used = devC0913Row.Is_Kd_Used;
                    PublicData.Dev.PID13.Is_U_Limit = devC0913Row.Is_U_Limit;
                    PublicData.Dev.PID13.Is_UILimit_Used = devC0913Row.Is_UILimit_Used;
                    PublicData.Dev.PID13.Is_ISeparate_Used = devC0913Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID14
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0914Row = C09Table.FindBy配置编号(tempC09RowName[3]);
                if (devC0914Row != null)
                {
                    PublicData.Dev.PID14.ControllerName = devC0914Row.PIDName;
                    PublicData.Dev.PID14.Kp = devC0914Row.Kp;
                    PublicData.Dev.PID14.Ki = devC0914Row.Ki;
                    PublicData.Dev.PID14.Kd = devC0914Row.Kd;
                    PublicData.Dev.PID14.U_UpperBound = devC0914Row.U_UpperBound;
                    PublicData.Dev.PID14.U_LowerBound = devC0914Row.U_LowerBound;
                    //PublicData.Dev.PID14.T = devC0914Row.T;
                    PublicData.Dev.PID14.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0914Row.PIDType;
                    PublicData.Dev.PID14.U_IMax_Limit = devC0914Row.U_IMax_Limit;
                    PublicData.Dev.PID14.ErrBound_IntegralSeparate = devC0914Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID14.Is_Kp_Used = devC0914Row.Is_Kp_Used;
                    PublicData.Dev.PID14.Is_Ki_Used = devC0914Row.Is_Ki_Used;
                    PublicData.Dev.PID14.Is_Kd_Used = devC0914Row.Is_Kd_Used;
                    PublicData.Dev.PID14.Is_U_Limit = devC0914Row.Is_U_Limit;
                    PublicData.Dev.PID14.Is_UILimit_Used = devC0914Row.Is_UILimit_Used;
                    PublicData.Dev.PID14.Is_ISeparate_Used = devC0914Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID15
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0915Row = C09Table.FindBy配置编号(tempC09RowName[4]);
                if (devC0915Row != null)
                {
                    PublicData.Dev.PID15.ControllerName = devC0915Row.PIDName;
                    PublicData.Dev.PID15.Kp = devC0915Row.Kp;
                    PublicData.Dev.PID15.Ki = devC0915Row.Ki;
                    PublicData.Dev.PID15.Kd = devC0915Row.Kd;
                    PublicData.Dev.PID15.U_UpperBound = devC0915Row.U_UpperBound;
                    PublicData.Dev.PID15.U_LowerBound = devC0915Row.U_LowerBound;
                    //PublicData.Dev.PID15.T = devC0915Row.T;
                    PublicData.Dev.PID15.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0915Row.PIDType;
                    PublicData.Dev.PID15.U_IMax_Limit = devC0915Row.U_IMax_Limit;
                    PublicData.Dev.PID15.ErrBound_IntegralSeparate = devC0915Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID15.Is_Kp_Used = devC0915Row.Is_Kp_Used;
                    PublicData.Dev.PID15.Is_Ki_Used = devC0915Row.Is_Ki_Used;
                    PublicData.Dev.PID15.Is_Kd_Used = devC0915Row.Is_Kd_Used;
                    PublicData.Dev.PID15.Is_U_Limit = devC0915Row.Is_U_Limit;
                    PublicData.Dev.PID15.Is_UILimit_Used = devC0915Row.Is_UILimit_Used;
                    PublicData.Dev.PID15.Is_ISeparate_Used = devC0915Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
            //PID41
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0941Row = C09Table.FindBy配置编号(tempC09RowName[17]);
                if (devC0941Row != null)
                {
                    PublicData.Dev.PID41.ControllerName = devC0941Row.PIDName;
                    PublicData.Dev.PID41.Kp = devC0941Row.Kp;
                    PublicData.Dev.PID41.Ki = devC0941Row.Ki;
                    PublicData.Dev.PID41.Kd = devC0941Row.Kd;
                    PublicData.Dev.PID41.U_UpperBound = devC0941Row.U_UpperBound;
                    PublicData.Dev.PID41.U_LowerBound = devC0941Row.U_LowerBound;
                    //PublicData.Dev.PID41.T = devC0941Row.T;
                    PublicData.Dev.PID41.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0941Row.PIDType;
                    PublicData.Dev.PID41.U_IMax_Limit = devC0941Row.U_IMax_Limit;
                    PublicData.Dev.PID41.ErrBound_IntegralSeparate = devC0941Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID41.Is_Kp_Used = devC0941Row.Is_Kp_Used;
                    PublicData.Dev.PID41.Is_Ki_Used = devC0941Row.Is_Ki_Used;
                    PublicData.Dev.PID41.Is_Kd_Used = devC0941Row.Is_Kd_Used;
                    PublicData.Dev.PID41.Is_U_Limit = devC0941Row.Is_U_Limit;
                    PublicData.Dev.PID41.Is_UILimit_Used = devC0941Row.Is_UILimit_Used;
                    PublicData.Dev.PID41.Is_ISeparate_Used = devC0941Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID42
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0942Row = C09Table.FindBy配置编号(tempC09RowName[18]);
                if (devC0942Row != null)
                {
                    PublicData.Dev.PID42.ControllerName = devC0942Row.PIDName;
                    PublicData.Dev.PID42.Kp = devC0942Row.Kp;
                    PublicData.Dev.PID42.Ki = devC0942Row.Ki;
                    PublicData.Dev.PID42.Kd = devC0942Row.Kd;
                    PublicData.Dev.PID42.U_UpperBound = devC0942Row.U_UpperBound;
                    PublicData.Dev.PID42.U_LowerBound = devC0942Row.U_LowerBound;
                    //PublicData.Dev.PID42.T = devC0942Row.T;
                    PublicData.Dev.PID42.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0942Row.PIDType;
                    PublicData.Dev.PID42.U_IMax_Limit = devC0942Row.U_IMax_Limit;
                    PublicData.Dev.PID42.ErrBound_IntegralSeparate = devC0942Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID42.Is_Kp_Used = devC0942Row.Is_Kp_Used;
                    PublicData.Dev.PID42.Is_Ki_Used = devC0942Row.Is_Ki_Used;
                    PublicData.Dev.PID42.Is_Kd_Used = devC0942Row.Is_Kd_Used;
                    PublicData.Dev.PID42.Is_U_Limit = devC0942Row.Is_U_Limit;
                    PublicData.Dev.PID42.Is_UILimit_Used = devC0942Row.Is_UILimit_Used;
                    PublicData.Dev.PID42.Is_ISeparate_Used = devC0942Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID43
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0943Row = C09Table.FindBy配置编号(tempC09RowName[19]);
                if (devC0943Row != null)
                {
                    PublicData.Dev.PID43.ControllerName = devC0943Row.PIDName;
                    PublicData.Dev.PID43.Kp = devC0943Row.Kp;
                    PublicData.Dev.PID43.Ki = devC0943Row.Ki;
                    PublicData.Dev.PID43.Kd = devC0943Row.Kd;
                    PublicData.Dev.PID43.U_UpperBound = devC0943Row.U_UpperBound;
                    PublicData.Dev.PID43.U_LowerBound = devC0943Row.U_LowerBound;
                    //PublicData.Dev.PID43.T = devC0943Row.T;
                    PublicData.Dev.PID43.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0943Row.PIDType;
                    PublicData.Dev.PID43.U_IMax_Limit = devC0943Row.U_IMax_Limit;
                    PublicData.Dev.PID43.ErrBound_IntegralSeparate = devC0943Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID43.Is_Kp_Used = devC0943Row.Is_Kp_Used;
                    PublicData.Dev.PID43.Is_Ki_Used = devC0943Row.Is_Ki_Used;
                    PublicData.Dev.PID43.Is_Kd_Used = devC0943Row.Is_Kd_Used;
                    PublicData.Dev.PID43.Is_U_Limit = devC0943Row.Is_U_Limit;
                    PublicData.Dev.PID43.Is_UILimit_Used = devC0943Row.Is_UILimit_Used;
                    PublicData.Dev.PID43.Is_ISeparate_Used = devC0943Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID44
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0944Row = C09Table.FindBy配置编号(tempC09RowName[20]);
                if (devC0944Row != null)
                {
                    PublicData.Dev.PID44.ControllerName = devC0944Row.PIDName;
                    PublicData.Dev.PID44.Kp = devC0944Row.Kp;
                    PublicData.Dev.PID44.Ki = devC0944Row.Ki;
                    PublicData.Dev.PID44.Kd = devC0944Row.Kd;
                    PublicData.Dev.PID44.U_UpperBound = devC0944Row.U_UpperBound;
                    PublicData.Dev.PID44.U_LowerBound = devC0944Row.U_LowerBound;
                    //PublicData.Dev.PID44.T = devC0944Row.T;
                    PublicData.Dev.PID44.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0944Row.PIDType;
                    PublicData.Dev.PID44.U_IMax_Limit = devC0944Row.U_IMax_Limit;
                    PublicData.Dev.PID44.ErrBound_IntegralSeparate = devC0944Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID44.Is_Kp_Used = devC0944Row.Is_Kp_Used;
                    PublicData.Dev.PID44.Is_Ki_Used = devC0944Row.Is_Ki_Used;
                    PublicData.Dev.PID44.Is_Kd_Used = devC0944Row.Is_Kd_Used;
                    PublicData.Dev.PID44.Is_U_Limit = devC0944Row.Is_U_Limit;
                    PublicData.Dev.PID44.Is_UILimit_Used = devC0944Row.Is_UILimit_Used;
                    PublicData.Dev.PID44.Is_ISeparate_Used = devC0944Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID45
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0945Row = C09Table.FindBy配置编号(tempC09RowName[21]);
                if (devC0945Row != null)
                {
                    PublicData.Dev.PID45.ControllerName = devC0945Row.PIDName;
                    PublicData.Dev.PID45.Kp = devC0945Row.Kp;
                    PublicData.Dev.PID45.Ki = devC0945Row.Ki;
                    PublicData.Dev.PID45.Kd = devC0945Row.Kd;
                    PublicData.Dev.PID45.U_UpperBound = devC0945Row.U_UpperBound;
                    PublicData.Dev.PID45.U_LowerBound = devC0945Row.U_LowerBound;
                    //PublicData.Dev.PID45.T = devC0945Row.T;
                    PublicData.Dev.PID45.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0945Row.PIDType;
                    PublicData.Dev.PID45.U_IMax_Limit = devC0945Row.U_IMax_Limit;
                    PublicData.Dev.PID45.ErrBound_IntegralSeparate = devC0945Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID45.Is_Kp_Used = devC0945Row.Is_Kp_Used;
                    PublicData.Dev.PID45.Is_Ki_Used = devC0945Row.Is_Ki_Used;
                    PublicData.Dev.PID45.Is_Kd_Used = devC0945Row.Is_Kd_Used;
                    PublicData.Dev.PID45.Is_U_Limit = devC0945Row.Is_U_Limit;
                    PublicData.Dev.PID45.Is_UILimit_Used = devC0945Row.Is_UILimit_Used;
                    PublicData.Dev.PID45.Is_ISeparate_Used = devC0945Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID61
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0961Row = C09Table.FindBy配置编号(tempC09RowName[23]);
                if (devC0961Row != null)
                {
                    PublicData.Dev.PID61.ControllerName = devC0961Row.PIDName;
                    PublicData.Dev.PID61.Kp = devC0961Row.Kp;
                    PublicData.Dev.PID61.Ki = devC0961Row.Ki;
                    PublicData.Dev.PID61.Kd = devC0961Row.Kd;
                    PublicData.Dev.PID61.U_UpperBound = devC0961Row.U_UpperBound;
                    PublicData.Dev.PID61.U_LowerBound = devC0961Row.U_LowerBound;
                    //PublicData.Dev.PID61.T = devC0961Row.T;
                    PublicData.Dev.PID61.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0961Row.PIDType;
                    PublicData.Dev.PID61.U_IMax_Limit = devC0961Row.U_IMax_Limit;
                    PublicData.Dev.PID61.ErrBound_IntegralSeparate = devC0961Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID61.Is_Kp_Used = devC0961Row.Is_Kp_Used;
                    PublicData.Dev.PID61.Is_Ki_Used = devC0961Row.Is_Ki_Used;
                    PublicData.Dev.PID61.Is_Kd_Used = devC0961Row.Is_Kd_Used;
                    PublicData.Dev.PID61.Is_U_Limit = devC0961Row.Is_U_Limit;
                    PublicData.Dev.PID61.Is_UILimit_Used = devC0961Row.Is_UILimit_Used;
                    PublicData.Dev.PID61.Is_ISeparate_Used = devC0961Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID62
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0962Row = C09Table.FindBy配置编号(tempC09RowName[24]);
                if (devC0962Row != null)
                {
                    PublicData.Dev.PID62.ControllerName = devC0962Row.PIDName;
                    PublicData.Dev.PID62.Kp = devC0962Row.Kp;
                    PublicData.Dev.PID62.Ki = devC0962Row.Ki;
                    PublicData.Dev.PID62.Kd = devC0962Row.Kd;
                    PublicData.Dev.PID62.U_UpperBound = devC0962Row.U_UpperBound;
                    PublicData.Dev.PID62.U_LowerBound = devC0962Row.U_LowerBound;
                    //PublicData.Dev.PID62.T = devC0962Row.T;
                    PublicData.Dev.PID62.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0962Row.PIDType;
                    PublicData.Dev.PID62.U_IMax_Limit = devC0962Row.U_IMax_Limit;
                    PublicData.Dev.PID62.ErrBound_IntegralSeparate = devC0962Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID62.Is_Kp_Used = devC0962Row.Is_Kp_Used;
                    PublicData.Dev.PID62.Is_Ki_Used = devC0962Row.Is_Ki_Used;
                    PublicData.Dev.PID62.Is_Kd_Used = devC0962Row.Is_Kd_Used;
                    PublicData.Dev.PID62.Is_U_Limit = devC0962Row.Is_U_Limit;
                    PublicData.Dev.PID62.Is_UILimit_Used = devC0962Row.Is_UILimit_Used;
                    PublicData.Dev.PID62.Is_ISeparate_Used = devC0962Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID63
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0963Row = C09Table.FindBy配置编号(tempC09RowName[25]);
                if (devC0963Row != null)
                {
                    PublicData.Dev.PID63.ControllerName = devC0963Row.PIDName;
                    PublicData.Dev.PID63.Kp = devC0963Row.Kp;
                    PublicData.Dev.PID63.Ki = devC0963Row.Ki;
                    PublicData.Dev.PID63.Kd = devC0963Row.Kd;
                    PublicData.Dev.PID63.U_UpperBound = devC0963Row.U_UpperBound;
                    PublicData.Dev.PID63.U_LowerBound = devC0963Row.U_LowerBound;
                    //PublicData.Dev.PID63.T = devC0963Row.T;
                    PublicData.Dev.PID63.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0963Row.PIDType;
                    PublicData.Dev.PID63.U_IMax_Limit = devC0963Row.U_IMax_Limit;
                    PublicData.Dev.PID63.ErrBound_IntegralSeparate = devC0963Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID63.Is_Kp_Used = devC0963Row.Is_Kp_Used;
                    PublicData.Dev.PID63.Is_Ki_Used = devC0963Row.Is_Ki_Used;
                    PublicData.Dev.PID63.Is_Kd_Used = devC0963Row.Is_Kd_Used;
                    PublicData.Dev.PID63.Is_U_Limit = devC0963Row.Is_U_Limit;
                    PublicData.Dev.PID63.Is_UILimit_Used = devC0963Row.Is_UILimit_Used;
                    PublicData.Dev.PID63.Is_ISeparate_Used = devC0963Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID64
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0964Row = C09Table.FindBy配置编号(tempC09RowName[26]);
                if (devC0964Row != null)
                {
                    PublicData.Dev.PID64.ControllerName = devC0964Row.PIDName;
                    PublicData.Dev.PID64.Kp = devC0964Row.Kp;
                    PublicData.Dev.PID64.Ki = devC0964Row.Ki;
                    PublicData.Dev.PID64.Kd = devC0964Row.Kd;
                    PublicData.Dev.PID64.U_UpperBound = devC0964Row.U_UpperBound;
                    PublicData.Dev.PID64.U_LowerBound = devC0964Row.U_LowerBound;
                    //PublicData.Dev.PID64.T = devC0964Row.T;
                    PublicData.Dev.PID64.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0964Row.PIDType;
                    PublicData.Dev.PID64.U_IMax_Limit = devC0964Row.U_IMax_Limit;
                    PublicData.Dev.PID64.ErrBound_IntegralSeparate = devC0964Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID64.Is_Kp_Used = devC0964Row.Is_Kp_Used;
                    PublicData.Dev.PID64.Is_Ki_Used = devC0964Row.Is_Ki_Used;
                    PublicData.Dev.PID64.Is_Kd_Used = devC0964Row.Is_Kd_Used;
                    PublicData.Dev.PID64.Is_U_Limit = devC0964Row.Is_U_Limit;
                    PublicData.Dev.PID64.Is_UILimit_Used = devC0964Row.Is_UILimit_Used;
                    PublicData.Dev.PID64.Is_ISeparate_Used = devC0964Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID65
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0965Row = C09Table.FindBy配置编号(tempC09RowName[27]);
                if (devC0965Row != null)
                {
                    PublicData.Dev.PID65.ControllerName = devC0965Row.PIDName;
                    PublicData.Dev.PID65.Kp = devC0965Row.Kp;
                    PublicData.Dev.PID65.Ki = devC0965Row.Ki;
                    PublicData.Dev.PID65.Kd = devC0965Row.Kd;
                    PublicData.Dev.PID65.U_UpperBound = devC0965Row.U_UpperBound;
                    PublicData.Dev.PID65.U_LowerBound = devC0965Row.U_LowerBound;
                    //PublicData.Dev.PID65.T = devC0965Row.T;
                    PublicData.Dev.PID65.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0965Row.PIDType;
                    PublicData.Dev.PID65.U_IMax_Limit = devC0965Row.U_IMax_Limit;
                    PublicData.Dev.PID65.ErrBound_IntegralSeparate = devC0965Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID65.Is_Kp_Used = devC0965Row.Is_Kp_Used;
                    PublicData.Dev.PID65.Is_Ki_Used = devC0965Row.Is_Ki_Used;
                    PublicData.Dev.PID65.Is_Kd_Used = devC0965Row.Is_Kd_Used;
                    PublicData.Dev.PID65.Is_U_Limit = devC0965Row.Is_U_Limit;
                    PublicData.Dev.PID65.Is_UILimit_Used = devC0965Row.Is_UILimit_Used;
                    PublicData.Dev.PID65.Is_ISeparate_Used = devC0965Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID21
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0921Row = C09Table.FindBy配置编号(tempC09RowName[5]);
                if (devC0921Row != null)
                {
                    PublicData.Dev.PID21.ControllerName = devC0921Row.PIDName;
                    PublicData.Dev.PID21.Kp = devC0921Row.Kp;
                    PublicData.Dev.PID21.Ki = devC0921Row.Ki;
                    PublicData.Dev.PID21.Kd = devC0921Row.Kd;
                    PublicData.Dev.PID21.U_UpperBound = devC0921Row.U_UpperBound;
                    PublicData.Dev.PID21.U_LowerBound = devC0921Row.U_LowerBound;
                    //PublicData.Dev.PID21.T = devC0921Row.T;
                    PublicData.Dev.PID21.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0921Row.PIDType;
                    PublicData.Dev.PID21.U_IMax_Limit = devC0921Row.U_IMax_Limit;
                    PublicData.Dev.PID21.ErrBound_IntegralSeparate = devC0921Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID21.Is_Kp_Used = devC0921Row.Is_Kp_Used;
                    PublicData.Dev.PID21.Is_Ki_Used = devC0921Row.Is_Ki_Used;
                    PublicData.Dev.PID21.Is_Kd_Used = devC0921Row.Is_Kd_Used;
                    PublicData.Dev.PID21.Is_U_Limit = devC0921Row.Is_U_Limit;
                    PublicData.Dev.PID21.Is_UILimit_Used = devC0921Row.Is_UILimit_Used;
                    PublicData.Dev.PID21.Is_ISeparate_Used = devC0921Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID22
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0922Row = C09Table.FindBy配置编号(tempC09RowName[6]);
                if (devC0922Row != null)
                {
                    PublicData.Dev.PID22.ControllerName = devC0922Row.PIDName;
                    PublicData.Dev.PID22.Kp = devC0922Row.Kp;
                    PublicData.Dev.PID22.Ki = devC0922Row.Ki;
                    PublicData.Dev.PID22.Kd = devC0922Row.Kd;
                    PublicData.Dev.PID22.U_UpperBound = devC0922Row.U_UpperBound;
                    PublicData.Dev.PID22.U_LowerBound = devC0922Row.U_LowerBound;
                    //PublicData.Dev.PID22.T = devC0922Row.T;
                    PublicData.Dev.PID22.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0922Row.PIDType;
                    PublicData.Dev.PID22.U_IMax_Limit = devC0922Row.U_IMax_Limit;
                    PublicData.Dev.PID22.ErrBound_IntegralSeparate = devC0922Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID22.Is_Kp_Used = devC0922Row.Is_Kp_Used;
                    PublicData.Dev.PID22.Is_Ki_Used = devC0922Row.Is_Ki_Used;
                    PublicData.Dev.PID22.Is_Kd_Used = devC0922Row.Is_Kd_Used;
                    PublicData.Dev.PID22.Is_U_Limit = devC0922Row.Is_U_Limit;
                    PublicData.Dev.PID22.Is_UILimit_Used = devC0922Row.Is_UILimit_Used;
                    PublicData.Dev.PID22.Is_ISeparate_Used = devC0922Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID23
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0923Row = C09Table.FindBy配置编号(tempC09RowName[7]);
                if (devC0923Row != null)
                {
                    PublicData.Dev.PID23.ControllerName = devC0923Row.PIDName;
                    PublicData.Dev.PID23.Kp = devC0923Row.Kp;
                    PublicData.Dev.PID23.Ki = devC0923Row.Ki;
                    PublicData.Dev.PID23.Kd = devC0923Row.Kd;
                    PublicData.Dev.PID23.U_UpperBound = devC0923Row.U_UpperBound;
                    PublicData.Dev.PID23.U_LowerBound = devC0923Row.U_LowerBound;
                    //PublicData.Dev.PID23.T = devC0923Row.T;
                    PublicData.Dev.PID23.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0923Row.PIDType;
                    PublicData.Dev.PID23.U_IMax_Limit = devC0923Row.U_IMax_Limit;
                    PublicData.Dev.PID23.ErrBound_IntegralSeparate = devC0923Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID23.Is_Kp_Used = devC0923Row.Is_Kp_Used;
                    PublicData.Dev.PID23.Is_Ki_Used = devC0923Row.Is_Ki_Used;
                    PublicData.Dev.PID23.Is_Kd_Used = devC0923Row.Is_Kd_Used;
                    PublicData.Dev.PID23.Is_U_Limit = devC0923Row.Is_U_Limit;
                    PublicData.Dev.PID23.Is_UILimit_Used = devC0923Row.Is_UILimit_Used;
                    PublicData.Dev.PID23.Is_ISeparate_Used = devC0923Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID24
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0924Row = C09Table.FindBy配置编号(tempC09RowName[8]);
                if (devC0924Row != null)
                {
                    PublicData.Dev.PID24.ControllerName = devC0924Row.PIDName;
                    PublicData.Dev.PID24.Kp = devC0924Row.Kp;
                    PublicData.Dev.PID24.Ki = devC0924Row.Ki;
                    PublicData.Dev.PID24.Kd = devC0924Row.Kd;
                    PublicData.Dev.PID24.U_UpperBound = devC0924Row.U_UpperBound;
                    PublicData.Dev.PID24.U_LowerBound = devC0924Row.U_LowerBound;
                    //PublicData.Dev.PID24.T = devC0924Row.T;
                    PublicData.Dev.PID24.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0924Row.PIDType;
                    PublicData.Dev.PID24.U_IMax_Limit = devC0924Row.U_IMax_Limit;
                    PublicData.Dev.PID24.ErrBound_IntegralSeparate = devC0924Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID24.Is_Kp_Used = devC0924Row.Is_Kp_Used;
                    PublicData.Dev.PID24.Is_Ki_Used = devC0924Row.Is_Ki_Used;
                    PublicData.Dev.PID24.Is_Kd_Used = devC0924Row.Is_Kd_Used;
                    PublicData.Dev.PID24.Is_U_Limit = devC0924Row.Is_U_Limit;
                    PublicData.Dev.PID24.Is_UILimit_Used = devC0924Row.Is_UILimit_Used;
                    PublicData.Dev.PID24.Is_ISeparate_Used = devC0924Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID25
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0925Row = C09Table.FindBy配置编号(tempC09RowName[9]);
                if (devC0925Row != null)
                {
                    PublicData.Dev.PID25.ControllerName = devC0925Row.PIDName;
                    PublicData.Dev.PID25.Kp = devC0925Row.Kp;
                    PublicData.Dev.PID25.Ki = devC0925Row.Ki;
                    PublicData.Dev.PID25.Kd = devC0925Row.Kd;
                    PublicData.Dev.PID25.U_UpperBound = devC0925Row.U_UpperBound;
                    PublicData.Dev.PID25.U_LowerBound = devC0925Row.U_LowerBound;
                    //PublicData.Dev.PID25.T = devC0925Row.T;
                    PublicData.Dev.PID25.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0925Row.PIDType;
                    PublicData.Dev.PID25.U_IMax_Limit = devC0925Row.U_IMax_Limit;
                    PublicData.Dev.PID25.ErrBound_IntegralSeparate = devC0925Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID25.Is_Kp_Used = devC0925Row.Is_Kp_Used;
                    PublicData.Dev.PID25.Is_Ki_Used = devC0925Row.Is_Ki_Used;
                    PublicData.Dev.PID25.Is_Kd_Used = devC0925Row.Is_Kd_Used;
                    PublicData.Dev.PID25.Is_U_Limit = devC0925Row.Is_U_Limit;
                    PublicData.Dev.PID25.Is_UILimit_Used = devC0925Row.Is_UILimit_Used;
                    PublicData.Dev.PID25.Is_ISeparate_Used = devC0925Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID26
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0926Row = C09Table.FindBy配置编号(tempC09RowName[10]);
                if (devC0926Row != null)
                {
                    PublicData.Dev.PID26.ControllerName = devC0926Row.PIDName;
                    PublicData.Dev.PID26.Kp = devC0926Row.Kp;
                    PublicData.Dev.PID26.Ki = devC0926Row.Ki;
                    PublicData.Dev.PID26.Kd = devC0926Row.Kd;
                    PublicData.Dev.PID26.U_UpperBound = devC0926Row.U_UpperBound;
                    PublicData.Dev.PID26.U_LowerBound = devC0926Row.U_LowerBound;
                    //PublicData.Dev.PID26.T = devC0926Row.T;
                    PublicData.Dev.PID26.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0926Row.PIDType;
                    PublicData.Dev.PID26.U_IMax_Limit = devC0926Row.U_IMax_Limit;
                    PublicData.Dev.PID26.ErrBound_IntegralSeparate = devC0926Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID26.Is_Kp_Used = devC0926Row.Is_Kp_Used;
                    PublicData.Dev.PID26.Is_Ki_Used = devC0926Row.Is_Ki_Used;
                    PublicData.Dev.PID26.Is_Kd_Used = devC0926Row.Is_Kd_Used;
                    PublicData.Dev.PID26.Is_U_Limit = devC0926Row.Is_U_Limit;
                    PublicData.Dev.PID26.Is_UILimit_Used = devC0926Row.Is_UILimit_Used;
                    PublicData.Dev.PID26.Is_ISeparate_Used = devC0926Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID51
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C09PID控制参数Row devC0951Row = C09Table.FindBy配置编号(tempC09RowName[22]);
                if (devC0951Row != null)
                {
                    PublicData.Dev.PID51.ControllerName = devC0951Row.PIDName;
                    PublicData.Dev.PID51.Kp = devC0951Row.Kp;
                    PublicData.Dev.PID51.Ki = devC0951Row.Ki;
                    PublicData.Dev.PID51.Kd = devC0951Row.Kd;
                    PublicData.Dev.PID51.U_UpperBound = devC0951Row.U_UpperBound;
                    PublicData.Dev.PID51.U_LowerBound = devC0951Row.U_LowerBound;
                    //PublicData.Dev.PID51.T = devC0951Row.T;
                    PublicData.Dev.PID51.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC0951Row.PIDType;
                    PublicData.Dev.PID51.U_IMax_Limit = devC0951Row.U_IMax_Limit;
                    PublicData.Dev.PID51.ErrBound_IntegralSeparate = devC0951Row.ErrBound_IntegralSeparate;
                    PublicData.Dev.PID51.Is_Kp_Used = devC0951Row.Is_Kp_Used;
                    PublicData.Dev.PID51.Is_Ki_Used = devC0951Row.Is_Ki_Used;
                    PublicData.Dev.PID51.Is_Kd_Used = devC0951Row.Is_Kd_Used;
                    PublicData.Dev.PID51.Is_U_Limit = devC0951Row.Is_U_Limit;
                    PublicData.Dev.PID51.Is_UILimit_Used = devC0951Row.Is_UILimit_Used;
                    PublicData.Dev.PID51.Is_ISeparate_Used = devC0951Row.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// C12、C13压力设定参数
        /// </summary>
        private void LoadPressSettings()
        {
            string tempSettingsNO = "Using";

            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C12气水密压力设定参数Row checkExistC12Row = C12Table.FindBy配置编号(tempSettingsNO);
                if (checkExistC12Row == null)
                {
                    MQDFJ_MB.MQZH_DB_DevDataSet.C12气水密压力设定参数Row defDevC12Row = C12Table.FindBy配置编号("DefaultSetting");
                    MQDFJ_MB.MQZH_DB_DevDataSet.C12气水密压力设定参数Row newDevC12Row = C12Table.NewC12气水密压力设定参数Row();
                    newDevC12Row.ItemArray = (object[])defDevC12Row.ItemArray.Clone();
                    newDevC12Row.配置编号 = tempSettingsNO;
                    C12Table.AddC12气水密压力设定参数Row(newDevC12Row);
                    C12TableAdapter.Update(C12Table);
                    C12Table.AcceptChanges();
                    RaisePropertyChanged(() => C12Table);
                    MessageBox.Show("未找到在用装置C12气水密压力设定参数，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C12气水密压力设定参数Row devC12Row = C12Table.FindBy配置编号(tempSettingsNO);
                if (devC12Row != null)
                {
                    //气密定级标准值
                    PublicData.Dev.IsPressFalse = devC12Row.IsPressFalse;
                    PublicData.Dev.PressSet_QMDJ_Std[0][0] = devC12Row.气密定级正预加压标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[0][1] = devC12Row.气密定级正预加压标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[0][2] = devC12Row.气密定级正预加压标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[1][0] = devC12Row.气密定级正压1标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[1][1] = devC12Row.气密定级正压2标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[1][2] = devC12Row.气密定级正压3标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[1][3] = devC12Row.气密定级正压2标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[1][4] = devC12Row.气密定级正压1标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[2][0] = devC12Row.气密定级负预加压标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[2][1] = devC12Row.气密定级负预加压标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[2][2] = devC12Row.气密定级负预加压标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[3][0] = devC12Row.气密定级负压1标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[3][1] = devC12Row.气密定级负压2标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[3][2] = devC12Row.气密定级负压3标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[3][3] = devC12Row.气密定级负压2标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[3][4] = devC12Row.气密定级负压1标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[4][0] = devC12Row.气密定级正预加压标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[4][1] = devC12Row.气密定级正预加压标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[4][2] = devC12Row.气密定级正预加压标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[5][0] = devC12Row.气密定级正压1标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[5][1] = devC12Row.气密定级正压2标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[5][2] = devC12Row.气密定级正压3标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[5][3] = devC12Row.气密定级正压2标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[5][4] = devC12Row.气密定级正压1标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[6][0] = devC12Row.气密定级负预加压标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[6][1] = devC12Row.气密定级负预加压标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[6][2] = devC12Row.气密定级负预加压标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[7][0] = devC12Row.气密定级负压1标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[7][1] = devC12Row.气密定级负压2标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[7][2] = devC12Row.气密定级负压3标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[7][3] = devC12Row.气密定级负压2标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[7][4] = devC12Row.气密定级负压1标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[8][0] = devC12Row.气密定级正预加压标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[8][1] = devC12Row.气密定级正预加压标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[8][2] = devC12Row.气密定级正预加压标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[9][0] = devC12Row.气密定级正压1标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[9][1] = devC12Row.气密定级正压2标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[9][2] = devC12Row.气密定级正压3标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[9][3] = devC12Row.气密定级正压2标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[9][4] = devC12Row.气密定级正压1标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[10][0] = devC12Row.气密定级负预加压标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[10][1] = devC12Row.气密定级负预加压标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[10][2] = devC12Row.气密定级负预加压标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[11][0] = devC12Row.气密定级负压1标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[11][1] = devC12Row.气密定级负压2标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[11][2] = devC12Row.气密定级负压3标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[11][3] = devC12Row.气密定级负压2标准值;
                    PublicData.Dev.PressSet_QMDJ_Std[11][4] = devC12Row.气密定级负压1标准值;

                    //气密定级实际值
                    PublicData.Dev.PressSet_QMDJ_False[0][0] = devC12Row.气密定级正预加压实际值;
                    PublicData.Dev.PressSet_QMDJ_False[0][1] = devC12Row.气密定级正预加压实际值;
                    PublicData.Dev.PressSet_QMDJ_False[0][2] = devC12Row.气密定级正预加压实际值;
                    PublicData.Dev.PressSet_QMDJ_False[1][0] = devC12Row.气密定级正压1实际值;
                    PublicData.Dev.PressSet_QMDJ_False[1][1] = devC12Row.气密定级正压2实际值;
                    PublicData.Dev.PressSet_QMDJ_False[1][2] = devC12Row.气密定级正压3实际值;
                    PublicData.Dev.PressSet_QMDJ_False[1][3] = devC12Row.气密定级正压2实际值;
                    PublicData.Dev.PressSet_QMDJ_False[1][4] = devC12Row.气密定级正压1实际值;
                    PublicData.Dev.PressSet_QMDJ_False[2][0] = devC12Row.气密定级负预加压实际值;
                    PublicData.Dev.PressSet_QMDJ_False[2][1] = devC12Row.气密定级负预加压实际值;
                    PublicData.Dev.PressSet_QMDJ_False[2][2] = devC12Row.气密定级负预加压实际值;
                    PublicData.Dev.PressSet_QMDJ_False[3][0] = devC12Row.气密定级负压1实际值;
                    PublicData.Dev.PressSet_QMDJ_False[3][1] = devC12Row.气密定级负压2实际值;
                    PublicData.Dev.PressSet_QMDJ_False[3][2] = devC12Row.气密定级负压3实际值;
                    PublicData.Dev.PressSet_QMDJ_False[3][3] = devC12Row.气密定级负压2实际值;
                    PublicData.Dev.PressSet_QMDJ_False[3][4] = devC12Row.气密定级负压1实际值;
                    PublicData.Dev.PressSet_QMDJ_False[4][0] = devC12Row.气密定级正预加压实际值;
                    PublicData.Dev.PressSet_QMDJ_False[4][1] = devC12Row.气密定级正预加压实际值;
                    PublicData.Dev.PressSet_QMDJ_False[4][2] = devC12Row.气密定级正预加压实际值;
                    PublicData.Dev.PressSet_QMDJ_False[5][0] = devC12Row.气密定级正压1实际值;
                    PublicData.Dev.PressSet_QMDJ_False[5][1] = devC12Row.气密定级正压2实际值;
                    PublicData.Dev.PressSet_QMDJ_False[5][2] = devC12Row.气密定级正压3实际值;
                    PublicData.Dev.PressSet_QMDJ_False[5][3] = devC12Row.气密定级正压2实际值;
                    PublicData.Dev.PressSet_QMDJ_False[5][4] = devC12Row.气密定级正压1实际值;
                    PublicData.Dev.PressSet_QMDJ_False[6][0] = devC12Row.气密定级负预加压实际值;
                    PublicData.Dev.PressSet_QMDJ_False[6][1] = devC12Row.气密定级负预加压实际值;
                    PublicData.Dev.PressSet_QMDJ_False[6][2] = devC12Row.气密定级负预加压实际值;
                    PublicData.Dev.PressSet_QMDJ_False[7][0] = devC12Row.气密定级负压1实际值;
                    PublicData.Dev.PressSet_QMDJ_False[7][1] = devC12Row.气密定级负压2实际值;
                    PublicData.Dev.PressSet_QMDJ_False[7][2] = devC12Row.气密定级负压3实际值;
                    PublicData.Dev.PressSet_QMDJ_False[7][3] = devC12Row.气密定级负压2实际值;
                    PublicData.Dev.PressSet_QMDJ_False[7][4] = devC12Row.气密定级负压1实际值;
                    PublicData.Dev.PressSet_QMDJ_False[8][0] = devC12Row.气密定级正预加压实际值;
                    PublicData.Dev.PressSet_QMDJ_False[8][1] = devC12Row.气密定级正预加压实际值;
                    PublicData.Dev.PressSet_QMDJ_False[8][2] = devC12Row.气密定级正预加压实际值;
                    PublicData.Dev.PressSet_QMDJ_False[9][0] = devC12Row.气密定级正压1实际值;
                    PublicData.Dev.PressSet_QMDJ_False[9][1] = devC12Row.气密定级正压2实际值;
                    PublicData.Dev.PressSet_QMDJ_False[9][2] = devC12Row.气密定级正压3实际值;
                    PublicData.Dev.PressSet_QMDJ_False[9][3] = devC12Row.气密定级正压2实际值;
                    PublicData.Dev.PressSet_QMDJ_False[9][4] = devC12Row.气密定级正压1实际值;
                    PublicData.Dev.PressSet_QMDJ_False[10][0] = devC12Row.气密定级负预加压实际值;
                    PublicData.Dev.PressSet_QMDJ_False[10][1] = devC12Row.气密定级负预加压实际值;
                    PublicData.Dev.PressSet_QMDJ_False[10][2] = devC12Row.气密定级负预加压实际值;
                    PublicData.Dev.PressSet_QMDJ_False[11][0] = devC12Row.气密定级负压1实际值;
                    PublicData.Dev.PressSet_QMDJ_False[11][1] = devC12Row.气密定级负压2实际值;
                    PublicData.Dev.PressSet_QMDJ_False[11][2] = devC12Row.气密定级负压3实际值;
                    PublicData.Dev.PressSet_QMDJ_False[11][3] = devC12Row.气密定级负压2实际值;
                    PublicData.Dev.PressSet_QMDJ_False[11][4] = devC12Row.气密定级负压1实际值;

                    //气密工程实际值
                    PublicData.Dev.PressSet_QMGC_False[0][0] = devC12Row.气密工程正预加压实际值;
                    PublicData.Dev.PressSet_QMGC_False[0][1] = devC12Row.气密工程正预加压实际值;
                    PublicData.Dev.PressSet_QMGC_False[0][2] = devC12Row.气密工程正预加压实际值;
                    PublicData.Dev.PressSet_QMGC_False[1][0] = devC12Row.气密工程正检测加压实际值;
                    PublicData.Dev.PressSet_QMGC_False[2][0] = devC12Row.气密工程负预加压实际值;
                    PublicData.Dev.PressSet_QMGC_False[2][1] = devC12Row.气密工程负预加压实际值;
                    PublicData.Dev.PressSet_QMGC_False[2][2] = devC12Row.气密工程负预加压实际值;
                    PublicData.Dev.PressSet_QMGC_False[3][0] = devC12Row.气密工程负检测加压实际值;

                    //水密预加压
                    PublicData.Dev.PressSet_SM_YJY_Std[0] = devC12Row.水密预加压标准值;
                    PublicData.Dev.PressSet_SM_YJY_Std[1] = devC12Row.水密预加压标准值;
                    PublicData.Dev.PressSet_SM_YJY_Std[2] = devC12Row.水密预加压标准值;
                    PublicData.Dev.PressSet_SM_YJY_False[0] = devC12Row.水密预加压实际值;
                    PublicData.Dev.PressSet_SM_YJY_False[1] = devC12Row.水密预加压实际值;
                    PublicData.Dev.PressSet_SM_YJY_False[2] = devC12Row.水密预加压实际值;
                    //水密定级稳定
                    PublicData.Dev.PressSet_SMDJ_WD_Std[0] = devC12Row.水密定级稳定第1级标准值;
                    PublicData.Dev.PressSet_SMDJ_WD_Std[1] = devC12Row.水密定级稳定第2级标准值;
                    PublicData.Dev.PressSet_SMDJ_WD_Std[2] = devC12Row.水密定级稳定第3级标准值;
                    PublicData.Dev.PressSet_SMDJ_WD_Std[3] = devC12Row.水密定级稳定第4级标准值;
                    PublicData.Dev.PressSet_SMDJ_WD_Std[4] = devC12Row.水密定级稳定第5级标准值;
                    PublicData.Dev.PressSet_SMDJ_WD_Std[5] = devC12Row.水密定级稳定第6级标准值;
                    PublicData.Dev.PressSet_SMDJ_WD_Std[6] = devC12Row.水密定级稳定第7级标准值;
                    PublicData.Dev.PressSet_SMDJ_WD_Std[7] = devC12Row.水密定级稳定第8级标准值;
                    PublicData.Dev.PressSet_SMDJ_WD_False[0] = devC12Row.水密定级稳定第1级实际值;
                    PublicData.Dev.PressSet_SMDJ_WD_False[1] = devC12Row.水密定级稳定第2级实际值;
                    PublicData.Dev.PressSet_SMDJ_WD_False[2] = devC12Row.水密定级稳定第3级实际值;
                    PublicData.Dev.PressSet_SMDJ_WD_False[3] = devC12Row.水密定级稳定第4级实际值;
                    PublicData.Dev.PressSet_SMDJ_WD_False[4] = devC12Row.水密定级稳定第5级实际值;
                    PublicData.Dev.PressSet_SMDJ_WD_False[5] = devC12Row.水密定级稳定第6级实际值;
                    PublicData.Dev.PressSet_SMDJ_WD_False[6] = devC12Row.水密定级稳定第7级实际值;
                    PublicData.Dev.PressSet_SMDJ_WD_False[7] = devC12Row.水密定级稳定第8级实际值;
                    //水密定级波动
                    PublicData.Dev.PressSet_SMDJ_BDPJ_Std[0] = devC12Row.水密定级波动第1级标准值;
                    PublicData.Dev.PressSet_SMDJ_BDPJ_Std[1] = devC12Row.水密定级波动第2级标准值;
                    PublicData.Dev.PressSet_SMDJ_BDPJ_Std[2] = devC12Row.水密定级波动第3级标准值;
                    PublicData.Dev.PressSet_SMDJ_BDPJ_Std[3] = devC12Row.水密定级波动第4级标准值;
                    PublicData.Dev.PressSet_SMDJ_BDPJ_Std[4] = devC12Row.水密定级波动第5级标准值;
                    PublicData.Dev.PressSet_SMDJ_BDPJ_Std[5] = devC12Row.水密定级波动第6级标准值;
                    PublicData.Dev.PressSet_SMDJ_BDPJ_Std[6] = devC12Row.水密定级波动第7级标准值;
                    PublicData.Dev.PressSet_SMDJ_BDPJ_Std[7] = devC12Row.水密定级波动第8级标准值;
                    PublicData.Dev.PressSet_SMDJ_BDPJ_False[0] = devC12Row.水密定级波动第1级实际值;
                    PublicData.Dev.PressSet_SMDJ_BDPJ_False[1] = devC12Row.水密定级波动第2级实际值;
                    PublicData.Dev.PressSet_SMDJ_BDPJ_False[2] = devC12Row.水密定级波动第3级实际值;
                    PublicData.Dev.PressSet_SMDJ_BDPJ_False[3] = devC12Row.水密定级波动第4级实际值;
                    PublicData.Dev.PressSet_SMDJ_BDPJ_False[4] = devC12Row.水密定级波动第5级实际值;
                    PublicData.Dev.PressSet_SMDJ_BDPJ_False[5] = devC12Row.水密定级波动第6级实际值;
                    PublicData.Dev.PressSet_SMDJ_BDPJ_False[6] = devC12Row.水密定级波动第7级实际值;
                    PublicData.Dev.PressSet_SMDJ_BDPJ_False[7] = devC12Row.水密定级波动第8级实际值;
                    //水密工程
                    PublicData.Dev.PressSet_SMGC_WDKKQ_False = devC12Row.水密工程稳定可开启压力实际值;
                    PublicData.Dev.PressSet_SMGC_WDGD_False = devC12Row.水密工程稳定固定压力实际值;
                    PublicData.Dev.PressSet_SMGC_BDPJKKQ_False = devC12Row.水密工程波动可开启压力实际值;
                    PublicData.Dev.PressSet_SMGC_BDPJGD_False = devC12Row.水密工程波动固定压力实际值;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


            //载入C13抗风压压力设定
            //若编号在表中不存在，则新建（拷贝DefaultSetting）
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C13抗风压压力设定参数Row checkExistC13Row = C13Table.FindBy配置编号(tempSettingsNO);
                if (checkExistC13Row == null)
                {
                    MQDFJ_MB.MQZH_DB_DevDataSet.C13抗风压压力设定参数Row defDevC13Row = C13Table.FindBy配置编号("DefaultSetting");
                    MQDFJ_MB.MQZH_DB_DevDataSet.C13抗风压压力设定参数Row newDevC13Row = C13Table.NewC13抗风压压力设定参数Row();
                    newDevC13Row.ItemArray = (object[])defDevC13Row.ItemArray.Clone();
                    newDevC13Row.配置编号 = tempSettingsNO;
                    C13Table.AddC13抗风压压力设定参数Row(newDevC13Row);
                    C13TableAdapter.Update(C13Table);
                    C13Table.AcceptChanges();
                    RaisePropertyChanged(() => C13Table);
                    MessageBox.Show("未找到在用装置C13抗风压压力设定参数，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C13抗风压压力设定参数Row devC13Row = C13Table.FindBy配置编号(tempSettingsNO);
                if (devC13Row != null)
                {
                    //抗风压定级变形
                    //定级变形
                    PublicData.Dev.PressSet_KFY_DJBX_Std[0][0] = devC13Row.抗风压定级p1预加压标准值;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[0][1] = devC13Row.抗风压定级p1预加压标准值;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[0][2] = devC13Row.抗风压定级p1预加压标准值;
                    PublicData.Dev.PressSet_KFY_DJBX_False[0][0] = devC13Row.抗风压定级p1预加压实际值;
                    PublicData.Dev.PressSet_KFY_DJBX_False[0][1] = devC13Row.抗风压定级p1预加压实际值;
                    PublicData.Dev.PressSet_KFY_DJBX_False[0][2] = devC13Row.抗风压定级p1预加压实际值;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[1][0] = devC13Row.抗风压定级p1压力标准值01;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[1][1] = devC13Row.抗风压定级p1压力标准值02;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[1][2] = devC13Row.抗风压定级p1压力标准值03;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[1][3] = devC13Row.抗风压定级p1压力标准值04;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[1][4] = devC13Row.抗风压定级p1压力标准值05;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[1][5] = devC13Row.抗风压定级p1压力标准值06;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[1][6] = devC13Row.抗风压定级p1压力标准值07;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[1][7] = devC13Row.抗风压定级p1压力标准值08;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[1][8] = devC13Row.抗风压定级p1压力标准值09;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[1][9] = devC13Row.抗风压定级p1压力标准值10;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[1][10] = devC13Row.抗风压定级p1压力标准值11;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[1][11] = devC13Row.抗风压定级p1压力标准值12;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[1][12] = devC13Row.抗风压定级p1压力标准值13;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[1][13] = devC13Row.抗风压定级p1压力标准值14;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[1][14] = devC13Row.抗风压定级p1压力标准值15;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[1][15] = devC13Row.抗风压定级p1压力标准值16;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[1][16] = devC13Row.抗风压定级p1压力标准值17;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[1][17] = devC13Row.抗风压定级p1压力标准值18;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[1][18] = devC13Row.抗风压定级p1压力标准值19;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[1][19] = devC13Row.抗风压定级p1压力标准值20;
                    PublicData.Dev.PressSet_KFY_DJBX_False[1][0] = devC13Row.抗风压定级p1压力实际值01;
                    PublicData.Dev.PressSet_KFY_DJBX_False[1][1] = devC13Row.抗风压定级p1压力实际值02;
                    PublicData.Dev.PressSet_KFY_DJBX_False[1][2] = devC13Row.抗风压定级p1压力实际值03;
                    PublicData.Dev.PressSet_KFY_DJBX_False[1][3] = devC13Row.抗风压定级p1压力实际值04;
                    PublicData.Dev.PressSet_KFY_DJBX_False[1][4] = devC13Row.抗风压定级p1压力实际值05;
                    PublicData.Dev.PressSet_KFY_DJBX_False[1][5] = devC13Row.抗风压定级p1压力实际值06;
                    PublicData.Dev.PressSet_KFY_DJBX_False[1][6] = devC13Row.抗风压定级p1压力实际值07;
                    PublicData.Dev.PressSet_KFY_DJBX_False[1][7] = devC13Row.抗风压定级p1压力实际值08;
                    PublicData.Dev.PressSet_KFY_DJBX_False[1][8] = devC13Row.抗风压定级p1压力实际值09;
                    PublicData.Dev.PressSet_KFY_DJBX_False[1][9] = devC13Row.抗风压定级p1压力实际值10;
                    PublicData.Dev.PressSet_KFY_DJBX_False[1][10] = devC13Row.抗风压定级p1压力实际值11;
                    PublicData.Dev.PressSet_KFY_DJBX_False[1][11] = devC13Row.抗风压定级p1压力实际值12;
                    PublicData.Dev.PressSet_KFY_DJBX_False[1][12] = devC13Row.抗风压定级p1压力实际值13;
                    PublicData.Dev.PressSet_KFY_DJBX_False[1][13] = devC13Row.抗风压定级p1压力实际值14;
                    PublicData.Dev.PressSet_KFY_DJBX_False[1][14] = devC13Row.抗风压定级p1压力实际值15;
                    PublicData.Dev.PressSet_KFY_DJBX_False[1][15] = devC13Row.抗风压定级p1压力实际值16;
                    PublicData.Dev.PressSet_KFY_DJBX_False[1][16] = devC13Row.抗风压定级p1压力实际值17;
                    PublicData.Dev.PressSet_KFY_DJBX_False[1][17] = devC13Row.抗风压定级p1压力实际值18;
                    PublicData.Dev.PressSet_KFY_DJBX_False[1][18] = devC13Row.抗风压定级p1压力实际值19;
                    PublicData.Dev.PressSet_KFY_DJBX_False[1][19] = devC13Row.抗风压定级p1压力实际值20;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[2][0] = -devC13Row.抗风压定级p1预加压标准值;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[2][1] = -devC13Row.抗风压定级p1预加压标准值;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[2][2] = -devC13Row.抗风压定级p1预加压标准值;
                    PublicData.Dev.PressSet_KFY_DJBX_False[2][0] = -devC13Row.抗风压定级p1预加压实际值;
                    PublicData.Dev.PressSet_KFY_DJBX_False[2][1] = -devC13Row.抗风压定级p1预加压实际值;
                    PublicData.Dev.PressSet_KFY_DJBX_False[2][2] = -devC13Row.抗风压定级p1预加压实际值;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[3][0] = -devC13Row.抗风压定级p1压力标准值01;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[3][1] = -devC13Row.抗风压定级p1压力标准值02;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[3][2] = -devC13Row.抗风压定级p1压力标准值03;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[3][3] = -devC13Row.抗风压定级p1压力标准值04;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[3][4] = -devC13Row.抗风压定级p1压力标准值05;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[3][5] = -devC13Row.抗风压定级p1压力标准值06;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[3][6] = -devC13Row.抗风压定级p1压力标准值07;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[3][7] = -devC13Row.抗风压定级p1压力标准值08;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[3][8] = -devC13Row.抗风压定级p1压力标准值09;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[3][9] = -devC13Row.抗风压定级p1压力标准值10;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[3][10] = -devC13Row.抗风压定级p1压力标准值11;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[3][11] = -devC13Row.抗风压定级p1压力标准值12;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[3][12] = -devC13Row.抗风压定级p1压力标准值13;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[3][13] = -devC13Row.抗风压定级p1压力标准值14;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[3][14] = -devC13Row.抗风压定级p1压力标准值15;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[3][15] = -devC13Row.抗风压定级p1压力标准值16;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[3][16] = -devC13Row.抗风压定级p1压力标准值17;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[3][17] = -devC13Row.抗风压定级p1压力标准值18;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[3][18] = -devC13Row.抗风压定级p1压力标准值19;
                    PublicData.Dev.PressSet_KFY_DJBX_Std[3][19] = -devC13Row.抗风压定级p1压力标准值20;
                    PublicData.Dev.PressSet_KFY_DJBX_False[3][0] = -devC13Row.抗风压定级p1压力实际值01;
                    PublicData.Dev.PressSet_KFY_DJBX_False[3][1] = -devC13Row.抗风压定级p1压力实际值02;
                    PublicData.Dev.PressSet_KFY_DJBX_False[3][2] = -devC13Row.抗风压定级p1压力实际值03;
                    PublicData.Dev.PressSet_KFY_DJBX_False[3][3] = -devC13Row.抗风压定级p1压力实际值04;
                    PublicData.Dev.PressSet_KFY_DJBX_False[3][4] = -devC13Row.抗风压定级p1压力实际值05;
                    PublicData.Dev.PressSet_KFY_DJBX_False[3][5] = -devC13Row.抗风压定级p1压力实际值06;
                    PublicData.Dev.PressSet_KFY_DJBX_False[3][6] = -devC13Row.抗风压定级p1压力实际值07;
                    PublicData.Dev.PressSet_KFY_DJBX_False[3][7] = -devC13Row.抗风压定级p1压力实际值08;
                    PublicData.Dev.PressSet_KFY_DJBX_False[3][8] = -devC13Row.抗风压定级p1压力实际值09;
                    PublicData.Dev.PressSet_KFY_DJBX_False[3][9] = -devC13Row.抗风压定级p1压力实际值10;
                    PublicData.Dev.PressSet_KFY_DJBX_False[3][10] = -devC13Row.抗风压定级p1压力实际值11;
                    PublicData.Dev.PressSet_KFY_DJBX_False[3][11] = -devC13Row.抗风压定级p1压力实际值12;
                    PublicData.Dev.PressSet_KFY_DJBX_False[3][12] = -devC13Row.抗风压定级p1压力实际值13;
                    PublicData.Dev.PressSet_KFY_DJBX_False[3][13] = -devC13Row.抗风压定级p1压力实际值14;
                    PublicData.Dev.PressSet_KFY_DJBX_False[3][14] = -devC13Row.抗风压定级p1压力实际值15;
                    PublicData.Dev.PressSet_KFY_DJBX_False[3][15] = -devC13Row.抗风压定级p1压力实际值16;
                    PublicData.Dev.PressSet_KFY_DJBX_False[3][16] = -devC13Row.抗风压定级p1压力实际值17;
                    PublicData.Dev.PressSet_KFY_DJBX_False[3][17] = -devC13Row.抗风压定级p1压力实际值18;
                    PublicData.Dev.PressSet_KFY_DJBX_False[3][18] = -devC13Row.抗风压定级p1压力实际值19;
                    PublicData.Dev.PressSet_KFY_DJBX_False[3][19] = -devC13Row.抗风压定级p1压力实际值20;
                    //定级P2/p3/pmax
                    PublicData.Dev.PressSet_KFY_DJP2_Raito_Std = devC13Row.抗风压定级p2倍数标准值;
                    PublicData.Dev.PressSet_KFY_DJP3_Raito_Std = devC13Row.抗风压定级p3倍数标准值;
                    PublicData.Dev.PressSet_KFY_DJPmax_Raito_Std = devC13Row.抗风压定级pmax倍数标准值;
                    PublicData.Dev.PressSet_KFY_DJP2_False = devC13Row.抗风压定级p2压力实际值;
                    PublicData.Dev.PressSet_KFY_DJP3_False = devC13Row.抗风压定级p3压力实际值;
                    PublicData.Dev.PressSet_KFY_DJPmax_False = devC13Row.抗风压定级pmax压力实际值;
                    //工程变形
                    PublicData.Dev.PressSet_KFY_GCP1_Y_Std = devC13Row.抗风压工程p1预加压标准值;
                    PublicData.Dev.PressSet_KFY_GCP1_Raito_Std[0] = devC13Row.抗风压工程p1压力倍数标准值1;
                    PublicData.Dev.PressSet_KFY_GCP1_Raito_Std[1] = devC13Row.抗风压工程p1压力倍数标准值2;
                    PublicData.Dev.PressSet_KFY_GCP1_Raito_Std[2] = devC13Row.抗风压工程p1压力倍数标准值3;
                    PublicData.Dev.PressSet_KFY_GCP1_Raito_Std[3] = devC13Row.抗风压工程p1压力倍数标准值4;
                    PublicData.Dev.PressSet_KFY_GCBX_False[0][0] = devC13Row.抗风压工程p1预加压实际值;
                    PublicData.Dev.PressSet_KFY_GCBX_False[0][1] = devC13Row.抗风压工程p1预加压实际值;
                    PublicData.Dev.PressSet_KFY_GCBX_False[0][2] = devC13Row.抗风压工程p1预加压实际值;
                    PublicData.Dev.PressSet_KFY_GCBX_False[1][0] = devC13Row.抗风压工程p1压力实际值1;
                    PublicData.Dev.PressSet_KFY_GCBX_False[1][1] = devC13Row.抗风压工程p1压力实际值2;
                    PublicData.Dev.PressSet_KFY_GCBX_False[1][2] = devC13Row.抗风压工程p1压力实际值3;
                    PublicData.Dev.PressSet_KFY_GCBX_False[1][3] = devC13Row.抗风压工程p1压力实际值4;
                    PublicData.Dev.PressSet_KFY_GCBX_False[2][0] = -devC13Row.抗风压工程p1预加压实际值;
                    PublicData.Dev.PressSet_KFY_GCBX_False[2][1] = -devC13Row.抗风压工程p1预加压实际值;
                    PublicData.Dev.PressSet_KFY_GCBX_False[2][2] = -devC13Row.抗风压工程p1预加压实际值;
                    PublicData.Dev.PressSet_KFY_GCBX_False[3][0] = -devC13Row.抗风压工程p1压力实际值1;
                    PublicData.Dev.PressSet_KFY_GCBX_False[3][1] = -devC13Row.抗风压工程p1压力实际值2;
                    PublicData.Dev.PressSet_KFY_GCBX_False[3][2] = -devC13Row.抗风压工程p1压力实际值3;
                    PublicData.Dev.PressSet_KFY_GCBX_False[3][3] = -devC13Row.抗风压工程p1压力实际值4;
                    //工程p2、p3、pmax
                    PublicData.Dev.PressSet_KFY_GCP2_Raito_Std = devC13Row.抗风压工程p2压力倍数标准值;
                    PublicData.Dev.PressSet_KFY_GCPmax_Raito_Std = devC13Row.抗风压工程pmax压力倍数标准值;
                    PublicData.Dev.PressSet_KFY_GCP2_False = devC13Row.抗风压工程p2压力实际值;
                    PublicData.Dev.PressSet_KFY_GCP3_False = devC13Row.抗风压工程p3压力实际值;
                    PublicData.Dev.PressSet_KFY_GCPmax_False = devC13Row.抗风压工程pmax压力实际值;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 载入C14气压控制参数
        /// </summary>
        private void LoadPressCtlSettings()
        {
            string tempSettingsNO = "Using";

            //载入C14气压控制参数
            //若编号在表中不存在，则新建（拷贝DefaultSetting）
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C14压力控制参数Row checkExistC14Row = C14Table.FindBy配置编号(tempSettingsNO);
                if (checkExistC14Row == null)
                {
                    MQDFJ_MB.MQZH_DB_DevDataSet.C14压力控制参数Row defDevC14Row = C14Table.FindBy配置编号("DefaultSetting");
                    MQDFJ_MB.MQZH_DB_DevDataSet.C14压力控制参数Row newDevC14ow = C14Table.NewC14压力控制参数Row();
                    newDevC14ow.ItemArray = (object[])defDevC14Row.ItemArray.Clone();
                    newDevC14ow.配置编号 = tempSettingsNO;
                    C14Table.AddC14压力控制参数Row(newDevC14ow);
                    C14TableAdapter.Update(C14Table);
                    C14Table.AcceptChanges();
                    RaisePropertyChanged(() => C14Table);
                    MessageBox.Show("未找到压力控制参数，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C14压力控制参数Row devC14Row = C14Table.FindBy配置编号(tempSettingsNO);
                if (devC14Row != null)
                {
                    //控制允许偏差
                    PublicData.Dev.AllowablePressErrQM[0] = devC14Row.PressErrQM_0_50;
                    PublicData.Dev.AllowablePressErrQM[1] = devC14Row.PressErrQM_50_100;
                    PublicData.Dev.AllowablePressErrQM[2] = devC14Row.PressErrQM_100_500;
                    PublicData.Dev.AllowablePressErrQM[3] = devC14Row.PressErrQM_500;
                    PublicData.Dev.AllowablePressErrBigCY[0] = devC14Row.PressErrBig_0_1000;
                    PublicData.Dev.AllowablePressErrBigCY[1] = devC14Row.PressErrBig_1000_3000;
                    PublicData.Dev.AllowablePressErrBigCY[2] = devC14Row.PressErrBig_3000_5000;
                    PublicData.Dev.AllowablePressErrBigCY[3] = devC14Row.PressErrBig_5000;
                    //一般
                    PublicData.Dev.LoadUpDownSpeed = devC14Row.LoadUpDownSpeed;
                    PublicData.Dev.KeepingTime_YJY = devC14Row.KeepingTime_YJY;
                    //气密控压
                    PublicData.Dev.KeepingTime_QMStep = devC14Row.KeepingTime_QMStep;
                    //水密控压
                    PublicData.Dev.KeepingTime_SMFistStep = devC14Row.KeepingTime_SMFistStep;
                    PublicData.Dev.KeepingTime_SMDJ_StepLeft = devC14Row.KeepingTime_SMDJ_StepLeft;
                    PublicData.Dev.KeepingTime_SMGC_KKQ = devC14Row.KeepingTime_SMGC_KKQ;
                    PublicData.Dev.KeepingTime_SMGC_YKQ = devC14Row.KeepingTime_SMGC_YKQ;
                    PublicData.Dev.KeepingTime_SMGC_WKQ = devC14Row.KeepingTime_SMGC_WKQ;
                    //水密波动加压
                    PublicData.Dev.KeepingTime_SMPreparePress = devC14Row.KeepingTime_SMPreparePress;
                    PublicData.Dev.PhRatioWaveH = devC14Row.PhRatioWaveH;
                    PublicData.Dev.PhRatioWaveL = devC14Row.PhRatioWaveL;
                    PublicData.Dev.HighRatioSM = devC14Row.HighRatioSM;
                    PublicData.Dev.LowRatioSM = devC14Row.LowRatioSM;
                    PublicData.Dev.PlTime_SMGC_Before = devC14Row.PlTime_SMGC_Before;

                    
                    //抗风压控压
                    PublicData.Dev.KeepingTime_KFY_BXstep = devC14Row.KeepingTime_KFY_BXstep;
                    PublicData.Dev.KeepingTime_KFY_AQ = devC14Row.KeepingTime_KFY_AQ;
                 //   PublicData.Dev.WaveNum_KFYP2 = devC14Row.WaveNum_KFYFF;
                    PublicData.Dev.LoadUpDownSpeed_KFRAQ = devC14Row.LoadUpDowSpeed_KFRAQ;
                    PublicData.Dev.HighRatioKFY = devC14Row.HighRatioKFY;
                    PublicData.Dev.LowRatioKFY = devC14Row.LowRatioKFY;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 载入C15位移设定参数
        /// </summary>
        private void LoadDevWYSettings()
        {
            string tempSettingsNO = "Using";

            //载入C15位移控制参数
            //若编号在表中不存在，则新建（拷贝DefaultSetting）
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C15位移设定参数Row checkExistC15Row = C15Table.FindBy配置编号(tempSettingsNO);
                if (checkExistC15Row == null)
                {
                    MQDFJ_MB.MQZH_DB_DevDataSet.C15位移设定参数Row defDevC15Row = C15Table.FindBy配置编号("DefaultSetting");
                    MQDFJ_MB.MQZH_DB_DevDataSet.C15位移设定参数Row newDevC15ow = C15Table.NewC15位移设定参数Row();
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
                MQDFJ_MB.MQZH_DB_DevDataSet.C15位移设定参数Row devC15Row = C15Table.FindBy配置编号(tempSettingsNO);
                if (devC15Row != null)
                {
                    PublicData.Dev.WYJ_Ctl_X[0] = devC15Row.X轴第1级最小位移角;
                    PublicData.Dev.WYJ_Ctl_X[1] = devC15Row.X轴第2级最小位移角;
                    PublicData.Dev.WYJ_Ctl_X[2] = devC15Row.X轴第3级最小位移角;
                    PublicData.Dev.WYJ_Ctl_X[3] = devC15Row.X轴第4级最小位移角;
                    PublicData.Dev.WYJ_Ctl_X[4] = devC15Row.X轴第5级最小位移角;
                    PublicData.Dev.WYJ_Ctl_Y[0] = devC15Row.Y轴第1级最小位移角;
                    PublicData.Dev.WYJ_Ctl_Y[1] = devC15Row.Y轴第2级最小位移角;
                    PublicData.Dev.WYJ_Ctl_Y[2] = devC15Row.Y轴第3级最小位移角;
                    PublicData.Dev.WYJ_Ctl_Y[3] = devC15Row.Y轴第4级最小位移角;
                    PublicData.Dev.WYJ_Ctl_Y[4] = devC15Row.Y轴第5级最小位移角;
                    PublicData.Dev.WY_Ctl_Z[0] = devC15Row.Z轴第1级最小位移量;
                    PublicData.Dev.WY_Ctl_Z[1] = devC15Row.Z轴第2级最小位移量;
                    PublicData.Dev.WY_Ctl_Z[2] = devC15Row.Z轴第3级最小位移量;
                    PublicData.Dev.WY_Ctl_Z[3] = devC15Row.Z轴第4级最小位移量;
                    PublicData.Dev.WY_Ctl_Z[4] = devC15Row.Z轴第5级最小位移量;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 载入C16位移控制参数
        /// </summary>
        private void LoadDevWYCtlSettings()
        {
            string tempSettingsNO = "Using";

            //载入C16位移控制参数
            //若编号在表中不存在，则新建（拷贝DefaultSetting）
            try
            {
                MQDFJ_MB.MQZH_DB_DevDataSet.C16位移控制参数Row checkExistC16Row = C16Table.FindBy配置编号(tempSettingsNO);
                if (checkExistC16Row == null)
                {
                    MQDFJ_MB.MQZH_DB_DevDataSet.C16位移控制参数Row defDevC16Row = C16Table.FindBy配置编号("DefaultSetting");
                    MQDFJ_MB.MQZH_DB_DevDataSet.C16位移控制参数Row newDevC16ow = C16Table.NewC16位移控制参数Row();
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
                MQDFJ_MB.MQZH_DB_DevDataSet.C16位移控制参数Row devC16Row = C16Table.FindBy配置编号(tempSettingsNO);
                if (devC16Row != null)
                {
                    PublicData.Dev.CJBX_XYPeriod = devC16Row.CJBX_XYPeriod;
                    PublicData.Dev.CJBX_ZPeriod = devC16Row.CJBX_ZPeriod;
                    PublicData.Dev.LenthRatio_X = devC16Row.LenthRatio_X;
                    PublicData.Dev.LenthRatio_Y = devC16Row.LenthRatio_Y;
                    PublicData.Dev.LenthRatio_Z = devC16Row.LenthRatio_Z;
                    PublicData.Dev.PermitErrX = devC16Row.PermitErrX;
                    PublicData.Dev.PermitErrY = devC16Row.PermitErrY;
                    PublicData.Dev.PermitErrZ = devC16Row.PermitErrZ;
                    PublicData.Dev.CorrXRight = devC16Row.CorrXRight;
                    PublicData.Dev.CorrXLeft = devC16Row.CorrXLeft;
                    PublicData.Dev.CorrYFront = devC16Row.CorrYFront;
                    PublicData.Dev.CorrYBack = devC16Row.CorrYBack;
                    PublicData.Dev.CorrZUp = devC16Row.CorrZUp;
                    PublicData.Dev.CorrZDown = devC16Row.CorrZDown;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion
    }
}
