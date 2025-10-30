/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 描述：
 *
 * ==================================================================================
 * 修改标记
 * 修改时间			        修改人			版本号			描述
 * 2022/2/8 15:10:31		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.IO;
using System.Windows;

namespace MQDFJ_MB.DAL
{
    /// <summary>
    /// 试验报告读写操作类
    /// </summary>
    public partial class MQZH_RepDAL : ObservableObject
    {
        #region 导出三性报告子函数

        /// <summary>
        /// 三性报告导出子函数
        /// </summary>
        private void SaveSXRPT(string aimBmpPath)
        {
            HSSFWorkbook iWorkbookSXRPT = null;

            //打开模板文件
            try
            {
                FileStream sSXFormfile = new FileStream(RptFormPathSX, FileMode.Open, FileAccess.Read);
                iWorkbookSXRPT = new HSSFWorkbook(sSXFormfile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            //复制模板文件内容
            try
            {
                int tempSXRPTSheets = iWorkbookSXRPT.NumberOfSheets;
                for (int i = 0; i < tempSXRPTSheets; ++i)
                {
                    ISheet ish = iWorkbookSXRPT.GetSheetAt(i);
                    ish.DisplayGridlines = true;
                    ish.DisplayRowColHeadings = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


            #region 三性报告第1页-三性报告首页
            try
            {
                ISheet sht0 = iWorkbookSXRPT.GetSheetAt(0);
                string str = "";
                HSSFRow row;
                HSSFCell cell;
                //单位
                row = (HSSFRow)sht0.GetRow(0);
                cell = (HSSFCell)row.GetCell(0);
                cell.SetCellValue(PublicData.Dev.MQZH_CompanyName);
                //报告编号
                row = (HSSFRow)sht0.GetRow(2);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ .ExpSettingParam .RepSXNO);
                //基本信息
                row = (HSSFRow)sht0.GetRow(3);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.ExpWTDW);       //委托单位
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.ExpWTDate);     //委托日期
                row = (HSSFRow)sht0.GetRow(4);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.ExpSCDW);       //生产单位
                row = (HSSFRow)sht0.GetRow(5);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.ExpGCMC);       //工程名称
                row = (HSSFRow)sht0.GetRow(6);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Name );  //样品名称
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Series );    //样品系列
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Type );      //规格型号
                //检测信息
                row = (HSSFRow)sht0.GetRow(7);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.Dev .MQZH_LabAddr);       //检测地点
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.ExpDate );       //检测日期
                row = (HSSFRow)sht0.GetRow(8);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue("√");       //气密项目
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue("√");       //水密项目
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue("√");       //抗风压项目
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.SpecimenNum);       //检测数量
                row = (HSSFRow)sht0.GetRow(9);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.Exp_QM .IsGC ?"工程":"定级");       //气密检测类别
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.Exp_SM.IsGC ? "工程" : "定级");       //水密检测类别
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.IsGC ? "工程" : "定级");       //抗风压检测类别

                //气密检测结论
                if (!PublicData.ExpDQ.Exp_QM.IsGC)
                {
                    row = (HSSFRow)sht0.GetRow(13);
                    cell = (HSSFCell)row.GetCell(11);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.QlLevel_DJ_QM); //气密开启缝长级别
                    row = (HSSFRow)sht0.GetRow(14);
                    cell = (HSSFCell)row.GetCell(11);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.QALevel_DJ_QM); //气密开启缝长级别
                    //清除工程部分结论
                    row = (HSSFRow)sht0.GetRow(19);
                    cell = (HSSFCell)row.GetCell(0);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(4);
                    cell.SetCellValue("");
                }
                else
                {
                    row = (HSSFRow)sht0.GetRow(19);
                    cell = (HSSFCell)row.GetCell(4);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.IsMeetDesign_GC_QM ? "满足工程使用要求" : "不满足工程使用要求");       //气密最终评定
                    //清除定级部分结论
                    row = (HSSFRow)sht0.GetRow(13);
                    cell = (HSSFCell)row.GetCell(0);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(2);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(11);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(13);
                    cell.SetCellValue("");
                    row = (HSSFRow)sht0.GetRow(14);
                    cell = (HSSFCell)row.GetCell(0);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(2);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(11);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(13);
                    cell.SetCellValue("");
                }
                //水密检测结论
                if (!PublicData.ExpDQ.Exp_SM.IsGC)
                {
                    row = (HSSFRow)sht0.GetRow(15);
                    cell = (HSSFCell)row.GetCell(11);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_SM.Level_DJ_KKQ); //水密可开启部分级别
                    row = (HSSFRow)sht0.GetRow(16);
                    cell = (HSSFCell)row.GetCell(11);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_SM.Level_DJ_GD); //水密固定部分级别
                    //清除工程部分结论
                    row = (HSSFRow)sht0.GetRow(20);
                    cell = (HSSFCell)row.GetCell(0);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(4);
                    cell.SetCellValue("");
                }
                else
                {
                    row = (HSSFRow)sht0.GetRow(20);
                    cell = (HSSFCell)row.GetCell(4);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_SM.IsMeetDesign_GC_All ? "满足工程使用要求" : "不满足工程使用要求");       //水密最终评定
                    //清除定级部分结论
                    row = (HSSFRow)sht0.GetRow(15);
                    cell = (HSSFCell)row.GetCell(0);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(2);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(11);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(13);
                    cell.SetCellValue("");
                    row = (HSSFRow)sht0.GetRow(16);
                    cell = (HSSFCell)row.GetCell(0);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(2);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(11);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(13);
                    cell.SetCellValue("");
                }

                //抗风压检测结论
                if (!PublicData.ExpDQ.Exp_KFY.IsGC)
                {
                    row = (HSSFRow)sht0.GetRow(17);
                    cell = (HSSFCell)row.GetCell(11);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.PdLevel_DJP3); //抗风压级别
                    //清除工程部分结论
                    row = (HSSFRow)sht0.GetRow(21);
                    cell = (HSSFCell)row.GetCell(0);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(4);
                    cell.SetCellValue("");
                }
                else
                {
                    row = (HSSFRow)sht0.GetRow(21);
                    cell = (HSSFCell)row.GetCell(4);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.IsMeetDesign_GCfinal ? "满足工程使用要求" : "不满足工程使用要求");       //抗风压最终评定
                    //清除定级部分结论
                    row = (HSSFRow)sht0.GetRow(17);
                    cell = (HSSFCell)row.GetCell(0);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(2);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(11);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(13);
                    cell.SetCellValue("");
                    row = (HSSFRow)sht0.GetRow(18);
                    cell = (HSSFCell)row.GetCell(0);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(2);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(11);
                    cell.SetCellValue("");
                    cell = (HSSFCell)row.GetCell(13);
                    cell.SetCellValue("");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region 三性报告第2页-三性报告第二页
            try
            {
                ISheet sht0 = iWorkbookSXRPT.GetSheetAt(1);
                string str = "";
                HSSFRow row;
                HSSFCell cell;
                //报告编号
                row = (HSSFRow)sht0.GetRow(0);
                cell = (HSSFCell)row.GetCell(1);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.RepSXNO);
                //试件参数
                row = (HSSFRow)sht0.GetRow(1);
                cell = (HSSFCell)row.GetCell(1);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_KQFLenth );      //开启缝长
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Aria );  //试件面积
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.RatioKQBF*100);     //开启部分面积占比
                //面板、镶嵌、密封、型材、附件
                row = (HSSFRow)sht0.GetRow(2);
                cell = (HSSFCell)row.GetCell(1);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MBPZ);      //面板品种
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MBAZFF);      //面板安装方式
                row = (HSSFRow)sht0.GetRow(3);
                cell = (HSSFCell)row.GetCell(1);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MBHD);      //面板厚度
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MBZDCC);      //面板最大尺寸
                row = (HSSFRow)sht0.GetRow(4);
                cell = (HSSFCell)row.GetCell(1);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_XQCLPZ );      //镶嵌材料品种
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MFCLPZ);      //密封材料品种
                row = (HSSFRow)sht0.GetRow(5);
                cell = (HSSFCell)row.GetCell(1);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_XCPZ);      //型材品种
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_FJPZ);      //附件品种
                //环境参数
                row = (HSSFRow)sht0.GetRow(6);
                cell = (HSSFCell)row.GetCell(1);
                cell.SetCellValue(PublicData.ExpDQ.ExpRoomT);      //环境温度
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpRoomPress);      //环境压力
                //设计指标
                row = (HSSFRow)sht0.GetRow(7);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.Exp_QM.QM_SJSTL_DWKQFC);      //气密单位开启缝长渗透量
                row = (HSSFRow)sht0.GetRow(8);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.Exp_QM.QM_SJSTL_DWMJ);      //气密单位面积渗透量
                row = (HSSFRow)sht0.GetRow(7);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.Exp_SM.SM_GCSJ_GD);      //水密固定部分
                row = (HSSFRow)sht0.GetRow(8);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.Exp_SM.SM_GCSJ_KKQ);      //水密可开启部分
                row = (HSSFRow)sht0.GetRow(7);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.P3_GCSJ/1000);      //抗风压p3

                //检测结果
                if (PublicData.ExpDQ.Exp_QM.IsGC)
                {
                    row = (HSSFRow)sht0.GetRow(10);
                    cell = (HSSFCell)row.GetCell(6);
                    cell.SetCellValue(Math.Max( PublicData.ExpDQ.ExpData_QM.Ql1_GC_Z, PublicData.ExpDQ.ExpData_QM.Ql1_GC_F));      //气密单位开启缝长渗透量
                    row = (HSSFRow)sht0.GetRow(11);
                    cell = (HSSFCell)row.GetCell(6);
                    cell.SetCellValue(Math.Max(PublicData.ExpDQ.ExpData_QM.QA1_GC_Z, PublicData.ExpDQ.ExpData_QM.QA1_GC_F));      //气密单位面积渗透量
                }
                else
                {
                    row = (HSSFRow)sht0.GetRow(10);
                    cell = (HSSFCell)row.GetCell(6);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Ql_DJ_QM);      //气密单位开启缝长渗透量
                    row = (HSSFRow)sht0.GetRow(11);
                    cell = (HSSFCell)row.GetCell(6);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.QA_DJ_QM);      //气密单位面积渗透量
                }
                
                //波动加压
                if (PublicData.ExpDQ.Exp_SM.WaveType_SM)
                {
                    row = (HSSFRow)sht0.GetRow(14);
                    cell = (HSSFCell)row.GetCell(6);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_SM.MaxPressWYZSL_DJ_GD);      //固定部分最高压力
                    row = (HSSFRow)sht0.GetRow(15);
                    cell = (HSSFCell)row.GetCell(6);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_SM.MaxPressWYZSL_DJ_KKQ);      //可开启部分最高压力
                }
                //稳定加压
                else
                {
                    row = (HSSFRow)sht0.GetRow(12);
                    cell = (HSSFCell)row.GetCell(6);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_SM .MaxPressWYZSL_DJ_GD);      //固定部分最高压力
                    row = (HSSFRow)sht0.GetRow(13);
                    cell = (HSSFCell)row.GetCell(6);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_SM.MaxPressWYZSL_DJ_KKQ);      //可开启部分最高压力
                }

                //抗风压工程
                if(PublicData.ExpDQ.Exp_KFY.IsGC)
                {
                    row = (HSSFRow)sht0.GetRow(24);
                    cell = (HSSFCell)row.GetCell(5);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCBX_Z[3]);      //P1正压检测压力40%
                    row = (HSSFRow)sht0.GetRow(25);
                    cell = (HSSFCell)row.GetCell(5);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCBX_F[3]);      //P1负压检测压力40%

                    row = (HSSFRow)sht0.GetRow(26);
                    cell = (HSSFCell)row.GetCell(5);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCP2_Z);      //P2正压检测压力
                    row = (HSSFRow)sht0.GetRow(27);
                    cell = (HSSFCell)row.GetCell(5);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCP2_Z);      //P2负压检测压力

                    row = (HSSFRow)sht0.GetRow(28);
                    cell = (HSSFCell)row.GetCell(5);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCP3_Z);      //P3正压检测压力
                    row = (HSSFRow)sht0.GetRow(29);
                    cell = (HSSFCell)row.GetCell(5);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCP3_Z);      //P3负压检测压力

                    row = (HSSFRow)sht0.GetRow(30);
                    cell = (HSSFCell)row.GetCell(5);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCPmax_Z);      //Pmax正压检测压力
                    row = (HSSFRow)sht0.GetRow(31);
                    cell = (HSSFCell)row.GetCell(5);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCPmax_F);      //Pmax负压检测压力
                }
                //抗风压定级
                else
                {
                    row = (HSSFRow)sht0.GetRow(16);
                    cell = (HSSFCell)row.GetCell(5);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.PDValue_DJP1_Z);      //P1正压评定
                    row = (HSSFRow)sht0.GetRow(17);
                    cell = (HSSFCell)row.GetCell(5);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.PDValue_DJP1_F);      //P1负压评定

                    row = (HSSFRow)sht0.GetRow(18);
                    cell = (HSSFCell)row.GetCell(5);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_DJP2_Z);      //P2正压检测压力
                    row = (HSSFRow)sht0.GetRow(19);
                    cell = (HSSFCell)row.GetCell(5);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_DJP2_Z);      //P2负压检测压力

                    row = (HSSFRow)sht0.GetRow(20);
                    cell = (HSSFCell)row.GetCell(5);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_DJP3_Z);      //P3正压检测压力
                    row = (HSSFRow)sht0.GetRow(21);
                    cell = (HSSFCell)row.GetCell(5);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_DJP3_Z);      //P3负压检测压力

                    row = (HSSFRow)sht0.GetRow(22);
                    cell = (HSSFCell)row.GetCell(5);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_DJPmax_Z);      //Pmax正压检测压力
                    row = (HSSFRow)sht0.GetRow(23);
                    cell = (HSSFCell)row.GetCell(5);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_DJPmax_F);      //Pmax负压检测压力
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region 三性报告第3页-气密
            try
            {
                ISheet sht0 = iWorkbookSXRPT.GetSheetAt(2);
                string str = "";
                //定级正压检测数据
                HSSFRow row = (HSSFRow)sht0.GetRow(3);
                HSSFCell cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[1][0]);       //正压附加渗透量-升压50
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[5][0]);      //正压附加固定渗透量-升压50
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[9][0]);      //正压总渗透量-升压50
                row = (HSSFRow)sht0.GetRow(4);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[1][1]);       //正压附加渗透量-升压100
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[5][1]);      //正压附加固定渗透量-升压100
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[9][1]);      //正压总渗透量-升压100
                row = (HSSFRow)sht0.GetRow(5);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[1][2]);       //正压附加渗透量-升压150
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[5][2]);      //正压附加固定渗透量-升压150
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[9][2]);      //正压总渗透量-升压150
                row = (HSSFRow)sht0.GetRow(6);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[1][3]);       //正压附加渗透量-降压100
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[5][3]);      //正压附加固定渗透量-降压100
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[9][3]);      //正压总渗透量-降压100
                row = (HSSFRow)sht0.GetRow(7);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[1][4]);       //正压附加渗透量-降压50
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[5][4]);      //正压附加固定渗透量-降压50
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[9][4]);      //正压总渗透量-降压50
                row = (HSSFRow)sht0.GetRow(8);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qfp_DJ_Z);       //正压附加渗透量-100Pa平均值
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qfgp_DJ_Z);      //正压附加固定渗透量-100Pa平均值
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qzp_DJ_Z);      //正压总渗透量-100Pa平均值

                //定级负压检测数据
                row = (HSSFRow)sht0.GetRow(9);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[3][0]);       //负压附加渗透量-升压50
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[7][0]);      //负压附加固定渗透量-升压50
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[11][0]);      //负压总渗透量-升压50
                row = (HSSFRow)sht0.GetRow(10);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[3][1]);       //负压附加渗透量-升压100
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[7][1]);      //负压附加固定渗透量-升压100
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[11][1]);      //负压总渗透量-升压100
                row = (HSSFRow)sht0.GetRow(11);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[3][2]);       //负压附加渗透量-升压150
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[7][2]);      //负压附加固定渗透量-升压150
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[11][2]);      //负压总渗透量-升压150
                row = (HSSFRow)sht0.GetRow(12);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[3][3]);       //负压附加渗透量-降压100
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[7][3]);      //负压附加固定渗透量-降压100
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[11][3]);      //负压总渗透量-降压100
                row = (HSSFRow)sht0.GetRow(13);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[3][4]);       //负压附加渗透量-降压50
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[7][4]);      //负压附加固定渗透量-降压50
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[11][4]);      //负压总渗透量-降压50
                row = (HSSFRow)sht0.GetRow(14);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qfp_DJ_F);       //负压附加渗透量-100Pa平均值
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qfgp_DJ_F);      //负压附加固定渗透量-100Pa平均值
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qzp_DJ_F);      //负压总渗透量-100Pa平均值

                //工程检测数据
                row = (HSSFRow)sht0.GetRow(15);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMGC[1][0]);       //工程正压附加渗透量
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMGC[5][0]);       //工程正压附加固定渗透量
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMGC[9][0]);       //工程正压总渗透量
                row = (HSSFRow)sht0.GetRow(16);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMGC[3][0]);       //工程负压附加渗透量
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMGC[7][0]);       //工程负压附加固定渗透量
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Stl_QMGC[11][0]);       //工程负压总渗透量

                //定级计算数据
                row = (HSSFRow)sht0.GetRow(3);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qf1_DJ_Z);       //定级正压标准状态下附加渗透量
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qf1_DJ_F);       //定级负压标准状态下附加渗透量
                row = (HSSFRow)sht0.GetRow(4);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qfg1_DJ_Z);       //定级正压标准状态下附加固定渗透量
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qfg1_DJ_F);       //定级负压标准状态下附加固定渗透量
                row = (HSSFRow)sht0.GetRow(5);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qz1_DJ_Z);       //定级正压标准状态下总渗透量
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qz1_DJ_F);       //定级负压标准状态下总渗透量
                row = (HSSFRow)sht0.GetRow(6);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qs_DJ_Z);       //定级正压标准状态下试件整体渗透量
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qs_DJ_F);       //定级负压标准状态下试件整体渗透量
                row = (HSSFRow)sht0.GetRow(7);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qk_DJ_Z);       //定级正压标准状态下试件可开启部分渗透量
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qk_DJ_F);       //定级负压标准状态下试件可开启部分渗透量
                row = (HSSFRow)sht0.GetRow(8);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.QA1_DJ_Z);       //定级正压标准状态下试件单位面积渗透量
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.QA1_DJ_F);       //定级负压标准状态下试件单位面积渗透量
                row = (HSSFRow)sht0.GetRow(9);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Ql1_DJ_Z);       //定级正压标准状态下试件单位开启缝长渗透量
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Ql1_DJ_F);       //定级负压标准状态下试件单位开启缝长渗透量
                row = (HSSFRow)sht0.GetRow(10);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.QA_DJ_Z);       //定级正压10Pa下试件单位面积渗透量
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.QA_DJ_F);       //定级负压10Pa下试件单位面积渗透量
                row = (HSSFRow)sht0.GetRow(11);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Ql_DJ_Z);       //定级正压10Pa下试件单位开启缝长渗透量
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Ql_DJ_F);       //定级负压10Pa下试件单位开启缝长渗透量

                row = (HSSFRow)sht0.GetRow(12);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.QA_DJ_QM);       //定级单位面积渗透量正负压最不利值
                row = (HSSFRow)sht0.GetRow(13);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Ql_DJ_QM);       //定级单位开启缝长渗透量正负压最不利值
                row = (HSSFRow)sht0.GetRow(14);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.QALevel_DJ_QM);       //定级单位面积渗透量综合评级
                row = (HSSFRow)sht0.GetRow(15);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.QlLevel_DJ_QM);       //定级单位开启缝长渗透量综合评级

                //工程计算数据
                row = (HSSFRow)sht0.GetRow(3);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qf1_GC_Z);       //工程正压标准状态下附加渗透量
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qf1_GC_F);       //工程负压标准状态下附加渗透量
                row = (HSSFRow)sht0.GetRow(4);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qfg1_GC_Z);       //工程正压标准状态下附加固定渗透量
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qfg1_GC_F);       //工程负压标准状态下附加固定渗透量
                row = (HSSFRow)sht0.GetRow(5);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qz1_GC_Z);       //工程正压标准状态下总渗透量
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qz1_GC_F);       //工程负压标准状态下总渗透量
                row = (HSSFRow)sht0.GetRow(6);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qs_GC_Z);       //工程正压标准状态下试件整体渗透量
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qs_GC_F);       //工程负压标准状态下试件整体渗透量
                row = (HSSFRow)sht0.GetRow(7);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qk_GC_Z);       //工程正压标准状态下试件可开启部分渗透量
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Qk_GC_F);       //工程负压标准状态下试件可开启部分渗透量
                row = (HSSFRow)sht0.GetRow(8);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.QA1_GC_Z);       //工程正压标准状态下试件单位面积渗透量
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.QA1_GC_F);       //工程负压标准状态下试件单位面积渗透量
                row = (HSSFRow)sht0.GetRow(9);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Ql1_GC_Z);       //工程正压标准状态下试件单位开启缝长渗透量
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_QM.Ql1_GC_F);       //工程负压标准状态下试件单位开启缝长渗透量

                row = (HSSFRow)sht0.GetRow(12);
                str = PublicData.ExpDQ.ExpData_QM.IsMeetDesign_GC_ZQA ? "满足" : "不满足";
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(str);       //单位面积渗透量是否满足工程设计要求-正压
                cell = (HSSFCell)row.GetCell(10);
                str = PublicData.ExpDQ.ExpData_QM.IsMeetDesign_GC_FQA ? "满足" : "不满足";
                cell.SetCellValue(str);       //单位面积渗透量是否满足工程设计要求-负压
                row = (HSSFRow)sht0.GetRow(13);
                cell = (HSSFCell)row.GetCell(9);
                str = PublicData.ExpDQ.ExpData_QM.IsMeetDesign_GC_ZQl ? "满足" : "不满足";
                cell.SetCellValue(str);       //单位开启缝长渗透量是否满足工程设计要求-正压
                cell = (HSSFCell)row.GetCell(10);
                str = PublicData.ExpDQ.ExpData_QM.IsMeetDesign_GC_FQl ? "满足" : "不满足";
                cell.SetCellValue(str);       //单位开启缝长渗透量是否满足工程设计要求-负压
                row = (HSSFRow)sht0.GetRow(14);
                str = (PublicData.ExpDQ.ExpData_QM.IsMeetDesign_GC_ZQA && PublicData.ExpDQ.ExpData_QM.IsMeetDesign_GC_FQA ) ? "满足" : "不满足";
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(str);       //单位面积渗透量是否满足工程设计要求
                row = (HSSFRow)sht0.GetRow(15);
                str = (PublicData.ExpDQ.ExpData_QM.IsMeetDesign_GC_ZQl && PublicData.ExpDQ.ExpData_QM.IsMeetDesign_GC_FQl) ? "满足" : "不满足";
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(str);       //单位开启缝长渗透量是否满足工程设计要求
                row = (HSSFRow)sht0.GetRow(16);
                str = PublicData.ExpDQ.ExpData_QM.IsMeetDesign_GC_QM ? "满足" : "不满足";
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(str);       //气密总体是否满足工程设计要求-负压

                //参数信息
                row = (HSSFRow)sht0.GetRow(17);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_KQFLenth);       //开启缝长度
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Aria);       //幕墙面积
                row = (HSSFRow)sht0.GetRow(18);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_KKQAria);       //可开启部分面积
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_KKQAria / PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Aria * 100);       //可开启部分面积占比
                row = (HSSFRow)sht0.GetRow(19);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpRoomPress);       //环境大气压
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpRoomT);       //环境温度

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            #endregion

            #region 三性报告第4页-水密
            try
            {
                ISheet sht0 = iWorkbookSXRPT.GetSheetAt(3);
                string str = "";
                HSSFRow row;
                HSSFCell cell;
                //检测类型
                row = (HSSFRow)sht0.GetRow(1);
                cell = (HSSFCell)row.GetCell(2);
                if (PublicData.ExpDQ.Exp_SM.IsGC)
                    str = "工程检测";
                else
                    str = "定级检测";
                cell.SetCellValue(str);
                //检测方法
                row = (HSSFRow)sht0.GetRow(1);
                cell = (HSSFCell)row.GetCell(6);
                if(PublicData.ExpDQ.Exp_SM.WaveType_SM)
                    str = "波动加压法";
                else
                    str = "稳定加压法";
                cell.SetCellValue(str);
                //淋水流量
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.Exp_SM.SM_SLL_Per_m2);

                //定级检测数据
                if (!PublicData.ExpDQ.Exp_SM.WaveType_SM)
                {
                    //定级稳定加压可开启部分渗漏情况
                    row = (HSSFRow)sht0.GetRow(4);
                    cell = (HSSFCell)row.GetCell(4);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[0]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(5);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[1]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(6);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[2]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(7);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[3]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(8);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[4]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(9);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[5]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(10);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[6]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(11);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[7]);
                    cell.SetCellValue(str);
                    //定级稳定加压固定部分渗漏情况
                    row = (HSSFRow)sht0.GetRow(5);
                    cell = (HSSFCell)row.GetCell(4);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[0]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(5);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[1]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(6);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[2]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(7);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[3]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(8);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[4]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(9);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[5]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(10);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[6]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(11);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[7]);
                    cell.SetCellValue(str);
                    //定级检测评定
                    row = (HSSFRow)sht0.GetRow(11);
                    cell = (HSSFCell)row.GetCell(4);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_SM.Level_DJ_KKQ);
                    row = (HSSFRow)sht0.GetRow(12);
                    cell = (HSSFCell)row.GetCell(4);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_SM.Level_DJ_GD);
                }
                else
                {
                    //定级波动加压可开启部分渗漏情况
                    row = (HSSFRow)sht0.GetRow(9);
                    cell = (HSSFCell)row.GetCell(4);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[0]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(5);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[1]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(6);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[2]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(7);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[3]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(8);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[4]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(9);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[5]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(10);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[6]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(11);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[7]);
                    cell.SetCellValue(str);
                    //定级波动加压固定部分渗漏情况
                    row = (HSSFRow)sht0.GetRow(10);
                    cell = (HSSFCell)row.GetCell(4);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[0]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(5);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[1]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(6);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[2]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(7);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[3]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(8);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[4]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(9);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[5]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(10);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[6]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(11);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[7]);
                    cell.SetCellValue(str);
                    //定级检测评定
                    row = (HSSFRow)sht0.GetRow(11);
                    cell = (HSSFCell)row.GetCell(4);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_SM.Level_DJ_KKQ);
                    row = (HSSFRow)sht0.GetRow(12);
                    cell = (HSSFCell)row.GetCell(4);
                    cell.SetCellValue(PublicData.ExpDQ.ExpData_SM.Level_DJ_GD);
                }

                //工程检测设置指标
                row = (HSSFRow)sht0.GetRow(14);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.Exp_SM.SM_GCSJ_KKQ);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.Exp_SM.SM_GCSJ_GD);

                if (!PublicData.ExpDQ.Exp_SM.WaveType_SM)
                {
                    //稳定加压检测压力
                    row = (HSSFRow)sht0.GetRow(16);
                    cell = (HSSFCell)row.GetCell(4);
                    cell.SetCellValue(PublicData.ExpDQ.Exp_SM.SM_GCSJ_KKQ);
                    cell = (HSSFCell)row.GetCell(8);
                    cell.SetCellValue(PublicData.ExpDQ.Exp_SM.SM_GCSJ_GD);
                    //渗漏情况
                    row = (HSSFRow)sht0.GetRow(17);
                    cell = (HSSFCell)row.GetCell(4);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_GC_KKQ[0]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(8);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_GC_GD[0]);
                    cell.SetCellValue(str);
                }
                else
                {
                    //波动加压检测压力
                    row = (HSSFRow)sht0.GetRow(18);
                    cell = (HSSFCell)row.GetCell(4);
                    cell.SetCellValue(PublicData.ExpDQ.Exp_SM.SM_GCSJ_KKQ * 1.25);
                    cell = (HSSFCell)row.GetCell(8);
                    cell.SetCellValue(PublicData.ExpDQ.Exp_SM.SM_GCSJ_GD * 1.25);
                    row = (HSSFRow)sht0.GetRow(19);
                    cell = (HSSFCell)row.GetCell(4);
                    cell.SetCellValue(PublicData.ExpDQ.Exp_SM.SM_GCSJ_KKQ);
                    cell = (HSSFCell)row.GetCell(8);
                    cell.SetCellValue(PublicData.ExpDQ.Exp_SM.SM_GCSJ_GD);
                    row = (HSSFRow)sht0.GetRow(20);
                    cell = (HSSFCell)row.GetCell(4);
                    cell.SetCellValue(PublicData.ExpDQ.Exp_SM.SM_GCSJ_KKQ * 0.75);
                    cell = (HSSFCell)row.GetCell(8);
                    cell.SetCellValue(PublicData.ExpDQ.Exp_SM.SM_GCSJ_GD * 0.75);
                    //渗漏情况
                    row = (HSSFRow)sht0.GetRow(21);
                    cell = (HSSFCell)row.GetCell(4);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_GC_KKQ[0]);
                    cell.SetCellValue(str);
                    cell = (HSSFCell)row.GetCell(8);
                    str = GetSL(PublicData.ExpDQ.ExpData_SM.SLStatus_GC_GD[0]);
                    cell.SetCellValue(str);
                }
                //工程检测评定结果
                row = (HSSFRow)sht0.GetRow(22);
                cell = (HSSFCell)row.GetCell(4);
                str = PublicData.ExpDQ.ExpData_SM.IsMeetDesign_GC_KKQ ? "满足" : "不满足";
                cell.SetCellValue(str);
                row = (HSSFRow)sht0.GetRow(23);
                cell = (HSSFCell)row.GetCell(4);
                str = PublicData.ExpDQ.ExpData_SM.IsMeetDesign_GC_GD ? "满足" : "不满足";
                cell.SetCellValue(str);
                row = (HSSFRow)sht0.GetRow(24);
                cell = (HSSFCell)row.GetCell(4);
                str = PublicData.ExpDQ.ExpData_SM.IsMeetDesign_GC_All ? "满足" : "不满足";
                cell.SetCellValue(str);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            #endregion

            #region 三性报告第5页-抗风压定级
            try
            {
                ISheet sht0 = iWorkbookSXRPT.GetSheetAt(4);
                string str = "";
                HSSFRow row;
                HSSFCell cell;

                #region 定级变形正压检测p1
                //测点组1数据1a
                row = (HSSFRow)sht0.GetRow(2);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[0][0]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[1][0]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[2][0]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[3][0]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[4][0]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[5][0]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[6][0]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[7][0]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[8][0]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[9][0]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[10][0]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[11][0]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[12][0]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[13][0]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[14][0]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[15][0]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[16][0]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[17][0]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[18][0]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[19][0]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[20][0]);
                //测点组1数据1b
                row = (HSSFRow)sht0.GetRow(3);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[0][1]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[1][1]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[2][1]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[3][1]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[4][1]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[5][1]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[6][1]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[7][1]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[8][1]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[9][1]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[10][1]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[11][1]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[12][1]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[13][1]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[14][1]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[15][1]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[16][1]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[17][1]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[18][1]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[19][1]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[20][1]);
                //测点组1数据1c
                row = (HSSFRow)sht0.GetRow(4);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[0][2]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[1][2]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[2][2]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[3][2]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[4][2]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[5][2]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[6][2]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[7][2]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[8][2]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[9][2]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[10][2]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[11][2]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[12][2]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[13][2]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[14][2]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[15][2]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[16][2]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[17][2]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[18][2]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[19][2]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[20][2]);
                //测点组1数据1d
                row = (HSSFRow)sht0.GetRow(5);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[0][3]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[1][3]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[2][3]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[3][3]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[4][3]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[5][3]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[6][3]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[7][3]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[8][3]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[9][3]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[10][3]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[11][3]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[12][3]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[13][3]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[14][3]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[15][3]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[16][3]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[17][3]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[18][3]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[19][3]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[20][3]);
                //测点组1数据2a
                row = (HSSFRow)sht0.GetRow(8);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[0][4]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[1][4]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[2][4]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[3][4]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[4][4]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[5][4]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[6][4]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[7][4]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[8][4]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[9][4]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[10][4]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[11][4]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[12][4]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[13][4]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[14][4]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[15][4]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[16][4]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[17][4]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[18][4]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[19][4]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[20][4]);
                //测点组1数据2b
                row = (HSSFRow)sht0.GetRow(9);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[0][5]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[1][5]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[2][5]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[3][5]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[4][5]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[5][5]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[6][5]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[7][5]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[8][5]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[9][5]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[10][5]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[11][5]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[12][5]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[13][5]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[14][5]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[15][5]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[16][5]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[17][5]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[18][5]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[19][5]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[20][5]);
                //测点组1数据2C
                row = (HSSFRow)sht0.GetRow(10);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[0][6]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[1][6]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[2][6]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[3][6]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[4][6]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[5][6]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[6][6]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[7][6]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[8][6]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[9][6]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[10][6]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[11][6]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[12][6]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[13][6]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[14][6]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[15][6]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[16][6]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[17][6]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[18][6]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[19][6]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[20][6]);

                //测点组1数据3a
                row = (HSSFRow)sht0.GetRow(13);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[0][7]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[1][7]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[2][7]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[3][7]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[4][7]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[5][7]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[6][7]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[7][7]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[8][7]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[9][7]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[10][7]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[11][7]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[12][7]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[13][7]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[14][7]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[15][7]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[16][7]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[17][7]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[18][7]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[19][7]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[20][7]);
                //测点组1数据3b
                row = (HSSFRow)sht0.GetRow(14);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[0][8]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[1][8]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[2][8]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[3][8]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[4][8]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[5][8]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[6][8]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[7][8]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[8][8]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[9][8]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[10][8]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[11][8]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[12][8]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[13][8]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[14][8]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[15][8]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[16][8]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[17][8]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[18][8]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[19][8]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[20][8]);
                //测点组1数据3c
                row = (HSSFRow)sht0.GetRow(15);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[0][9]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[1][9]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[2][9]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[3][9]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[4][9]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[5][9]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[6][9]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[7][9]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[8][9]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[9][9]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[10][9]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[11][9]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[12][9]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[13][9]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[14][9]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[15][9]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[16][9]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[17][9]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[18][9]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[19][9]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[20][9]);

                //测点组1面法线挠度
                row = (HSSFRow)sht0.GetRow(6);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[0][0]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[1][0]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[2][0]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[3][0]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[4][0]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[5][0]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[6][0]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[7][0]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[8][0]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[9][0]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[10][0]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[11][0]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[12][0]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[13][0]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[14][0]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[15][0]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[16][0]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[17][0]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[18][0]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[19][0]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[20][0]);
                //测点组2面法线挠度
                row = (HSSFRow)sht0.GetRow(11);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[0][1]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[1][1]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[2][1]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[3][1]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[4][1]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[5][1]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[6][1]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[7][1]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[8][1]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[9][1]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[10][1]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[11][1]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[12][1]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[13][1]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[14][1]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[15][1]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[16][1]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[17][1]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[18][1]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[19][1]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[20][1]);
                //测点组3面法线挠度
                row = (HSSFRow)sht0.GetRow(16);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[0][2]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[1][2]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[2][2]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[3][2]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[4][2]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[5][2]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[6][2]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[7][2]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[8][2]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[9][2]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[10][2]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[11][2]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[12][2]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[13][2]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[14][2]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[15][2]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[16][2]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[17][2]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[18][2]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[19][2]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[20][2]);

                //测点组1相对面法线挠度
                row = (HSSFRow)sht0.GetRow(7);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[0][0]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[1][0]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[2][0]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[3][0]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[4][0]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[5][0]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[6][0]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[7][0]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[8][0]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[9][0]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[10][0]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[11][0]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[12][0]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[13][0]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[14][0]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[15][0]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[16][0]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[17][0]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[18][0]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[19][0]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[20][0]);
                //测点组2相对面法线挠度
                row = (HSSFRow)sht0.GetRow(12);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[0][1]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[1][1]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[2][1]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[3][1]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[4][1]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[5][1]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[6][1]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[7][1]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[8][1]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[9][1]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[10][1]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[11][1]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[12][1]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[13][1]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[14][1]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[15][1]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[16][1]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[17][1]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[18][1]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[19][1]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[20][1]);
                //测点组3相对面法线挠度
                row = (HSSFRow)sht0.GetRow(17);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[0][2]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[1][2]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[2][2]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[3][2]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[4][2]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[5][2]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[6][2]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[7][2]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[8][2]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[9][2]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[10][2]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[11][2]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[12][2]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[13][2]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[14][2]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[15][2]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[16][2]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[17][2]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[18][2]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[19][2]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[20][2]);

                //定级正压变形相对挠度最大值
                row = (HSSFRow)sht0.GetRow(18);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[0]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[1]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[2]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[3]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[4]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[5]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[6]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[7]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[8]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[9]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[10]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[11]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[12]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[13]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[14]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[15]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[16]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[17]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[18]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[19]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[20]);

                //定级正压变形损坏情况
                row = (HSSFRow)sht0.GetRow(19);
                cell = (HSSFCell)row.GetCell(5);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[0] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(6);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[1] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(7);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[2] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(8);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[3] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(9);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[4] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(10);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[5] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(11);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[6] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(12);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[7] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(13);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[8] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(14);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[9] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(15);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[10] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(16);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[11] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(17);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[12] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(18);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[13] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(19);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[14] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(20);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[15] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(21);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[16] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(22);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[17] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(23);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[18] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(24);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[19] ? "有" : "";
                cell.SetCellValue(str);
                //定级正压变形评定压力
                row = (HSSFRow)sht0.GetRow(20);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.PDValue_DJP1_Z );
                #endregion

                #region 定级变形负压检测p1
                //测点组1数据1a
                row = (HSSFRow)sht0.GetRow(22);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[0][0]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[1][0]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[2][0]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[3][0]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[4][0]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[5][0]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[6][0]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[7][0]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[8][0]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[9][0]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[10][0]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[11][0]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[12][0]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[13][0]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[14][0]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[15][0]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[16][0]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[17][0]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[18][0]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[19][0]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[20][0]);
                //测点组1数据1b
                row = (HSSFRow)sht0.GetRow(23);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[0][1]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[1][1]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[2][1]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[3][1]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[4][1]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[5][1]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[6][1]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[7][1]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[8][1]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[9][1]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[10][1]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[11][1]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[12][1]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[13][1]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[14][1]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[15][1]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[16][1]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[17][1]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[18][1]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[19][1]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[20][1]);
                //测点组1数据1c
                row = (HSSFRow)sht0.GetRow(24);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[0][2]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[1][2]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[2][2]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[3][2]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[4][2]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[5][2]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[6][2]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[7][2]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[8][2]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[9][2]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[10][2]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[11][2]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[12][2]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[13][2]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[14][2]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[15][2]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[16][2]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[17][2]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[18][2]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[19][2]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[20][2]);
                //测点组1数据1d
                row = (HSSFRow)sht0.GetRow(25);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[0][3]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[1][3]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[2][3]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[3][3]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[4][3]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[5][3]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[6][3]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[7][3]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[8][3]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[9][3]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[10][3]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[11][3]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[12][3]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[13][3]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[14][3]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[15][3]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[16][3]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[17][3]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[18][3]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[19][3]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[20][3]);
                //测点组1数据2a
                row = (HSSFRow)sht0.GetRow(28);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[0][4]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[1][4]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[2][4]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[3][4]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[4][4]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[5][4]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[6][4]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[7][4]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[8][4]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[9][4]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[10][4]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[11][4]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[12][4]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[13][4]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[14][4]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[15][4]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[16][4]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[17][4]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[18][4]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[19][4]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[20][4]);
                //测点组1数据2b
                row = (HSSFRow)sht0.GetRow(29);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[0][5]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[1][5]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[2][5]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[3][5]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[4][5]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[5][5]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[6][5]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[7][5]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[8][5]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[9][5]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[10][5]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[11][5]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[12][5]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[13][5]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[14][5]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[15][5]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[16][5]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[17][5]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[18][5]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[19][5]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[20][5]);
                //测点组1数据2C
                row = (HSSFRow)sht0.GetRow(30);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[0][6]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[1][6]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[2][6]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[3][6]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[4][6]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[5][6]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[6][6]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[7][6]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[8][6]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[9][6]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[10][6]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[11][6]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[12][6]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[13][6]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[14][6]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[15][6]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[16][6]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[17][6]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[18][6]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[19][6]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[20][6]);

                //测点组1数据3a
                row = (HSSFRow)sht0.GetRow(33);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[0][7]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[1][7]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[2][7]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[3][7]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[4][7]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[5][7]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[6][7]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[7][7]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[8][7]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[9][7]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[10][7]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[11][7]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[12][7]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[13][7]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[14][7]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[15][7]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[16][7]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[17][7]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[18][7]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[19][7]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[20][7]);
                //测点组1数据3b
                row = (HSSFRow)sht0.GetRow(34);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[0][8]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[1][8]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[2][8]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[3][8]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[4][8]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[5][8]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[6][8]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[7][8]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[8][8]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[9][8]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[10][8]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[11][8]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[12][8]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[13][8]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[14][8]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[15][8]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[16][8]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[17][8]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[18][8]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[19][8]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[20][8]);
                //测点组1数据3c
                row = (HSSFRow)sht0.GetRow(35);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[0][9]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[1][9]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[2][9]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[3][9]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[4][9]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[5][9]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[6][9]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[7][9]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[8][9]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[9][9]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[10][9]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[11][9]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[12][9]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[13][9]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[14][9]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[15][9]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[16][9]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[17][9]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[18][9]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[19][9]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[20][9]);

                //测点组1面法线挠度
                row = (HSSFRow)sht0.GetRow(26);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[0][0]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[1][0]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[2][0]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[3][0]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[4][0]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[5][0]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[6][0]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[7][0]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[8][0]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[9][0]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[10][0]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[11][0]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[12][0]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[13][0]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[14][0]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[15][0]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[16][0]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[17][0]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[18][0]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[19][0]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[20][0]);
                //测点组2面法线挠度
                row = (HSSFRow)sht0.GetRow(31);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[0][1]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[1][1]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[2][1]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[3][1]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[4][1]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[5][1]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[6][1]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[7][1]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[8][1]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[9][1]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[10][1]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[11][1]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[12][1]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[13][1]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[14][1]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[15][1]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[16][1]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[17][1]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[18][1]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[19][1]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[20][1]);
                //测点组3面法线挠度
                row = (HSSFRow)sht0.GetRow(36);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[0][2]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[1][2]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[2][2]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[3][2]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[4][2]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[5][2]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[6][2]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[7][2]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[8][2]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[9][2]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[10][2]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[11][2]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[12][2]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[13][2]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[14][2]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[15][2]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[16][2]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[17][2]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[18][2]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[19][2]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[20][2]);

                //测点组1相对面法线挠度
                row = (HSSFRow)sht0.GetRow(27);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[0][0]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[1][0]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[2][0]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[3][0]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[4][0]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[5][0]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[6][0]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[7][0]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[8][0]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[9][0]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[10][0]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[11][0]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[12][0]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[13][0]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[14][0]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[15][0]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[16][0]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[17][0]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[18][0]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[19][0]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[20][0]);
                //测点组2相对面法线挠度
                row = (HSSFRow)sht0.GetRow(32);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[0][1]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[1][1]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[2][1]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[3][1]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[4][1]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[5][1]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[6][1]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[7][1]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[8][1]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[9][1]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[10][1]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[11][1]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[12][1]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[13][1]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[14][1]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[15][1]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[16][1]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[17][1]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[18][1]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[19][1]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[20][1]);
                //测点组3相对面法线挠度
                row = (HSSFRow)sht0.GetRow(37);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[0][2]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[1][2]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[2][2]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[3][2]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[4][2]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[5][2]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[6][2]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[7][2]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[8][2]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[9][2]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[10][2]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[11][2]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[12][2]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[13][2]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[14][2]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[15][2]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[16][2]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[17][2]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[18][2]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[19][2]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[20][2]);

                //定级正压变形相对挠度最大值
                row = (HSSFRow)sht0.GetRow(38);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[0]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[1]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[2]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[3]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[4]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[5]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[6]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[7]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[8]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[9]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[10]);
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[11]);
                cell = (HSSFCell)row.GetCell(16);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[12]);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[13]);
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[14]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[15]);
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[16]);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[17]);
                cell = (HSSFCell)row.GetCell(22);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[18]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[19]);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[20]);

                //定级正压变形损坏情况
                row = (HSSFRow)sht0.GetRow(39);
                cell = (HSSFCell)row.GetCell(5);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[0] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(6);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[1] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(7);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[2] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(8);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[3] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(9);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[4] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(10);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[5] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(11);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[6] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(12);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[7] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(13);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[8] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(14);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[9] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(15);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[10] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(16);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[11] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(17);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[12] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(18);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[13] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(19);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[14] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(20);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[15] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(21);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[16] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(22);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[17] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(23);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[18] ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(24);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[19] ? "有" : "";
                cell.SetCellValue(str);
                //定级正压变形评定压力
                row = (HSSFRow)sht0.GetRow(40);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.PDValue_DJP1_F);
                #endregion

                //定级变形p1总评
                row = (HSSFRow)sht0.GetRow(41);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.PDValue_DJP1_All);

                #region 反复加压p2
                //定级p2
                row = (HSSFRow)sht0.GetRow(42);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_DJP2_Z);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_DJP2_F);
                row = (HSSFRow)sht0.GetRow(43);
                cell = (HSSFCell)row.GetCell(4);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJP2_Z ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(17);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJP2_F ? "有" : "";
                cell.SetCellValue(str);
                #endregion

                #region 风荷载标准值p3
                //p3检测压力
                row = (HSSFRow)sht0.GetRow(45);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_DJP3_Z);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_DJP3_F);
                //p3正压检测位移
                //位移组1
                row = (HSSFRow)sht0.GetRow(46);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[0][0]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[1][0]);
                row = (HSSFRow)sht0.GetRow(47);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[0][1]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[1][1]);
                row = (HSSFRow)sht0.GetRow(48);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[0][2]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[1][2]);
                row = (HSSFRow)sht0.GetRow(49);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[0][3]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[1][3]);
                //位移组2
                row = (HSSFRow)sht0.GetRow(52);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[0][4]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[1][4]);
                row = (HSSFRow)sht0.GetRow(53);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[0][5]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[1][5]);
                row = (HSSFRow)sht0.GetRow(54);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[0][6]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[1][6]);
                //位移组3
                row = (HSSFRow)sht0.GetRow(57);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[0][7]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[1][7]);
                row = (HSSFRow)sht0.GetRow(58);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[0][8]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[1][8]);
                row = (HSSFRow)sht0.GetRow(59);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[0][9]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[1][9]);
                //p3负压检测位移
                //位移组1
                row = (HSSFRow)sht0.GetRow(46);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[0][0]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[1][0]);
                row = (HSSFRow)sht0.GetRow(47);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[0][1]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[1][1]);
                row = (HSSFRow)sht0.GetRow(48);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[0][2]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[1][2]);
                row = (HSSFRow)sht0.GetRow(49);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[0][3]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[1][3]);
                //位移组2
                row = (HSSFRow)sht0.GetRow(52);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[0][4]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[1][4]);
                row = (HSSFRow)sht0.GetRow(53);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[0][5]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[1][5]);
                row = (HSSFRow)sht0.GetRow(54);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[0][6]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[1][6]);
                //位移组3
                row = (HSSFRow)sht0.GetRow(57);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[0][7]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[1][7]);
                row = (HSSFRow)sht0.GetRow(58);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[0][8]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[1][8]);
                row = (HSSFRow)sht0.GetRow(59);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[0][9]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[1][9]);
                //p3正压挠度
                //面法线挠度
                row = (HSSFRow)sht0.GetRow(50);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJP3_Z[0][0]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJP3_Z[1][0]);
                row = (HSSFRow)sht0.GetRow(55);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJP3_Z[0][1]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJP3_Z[1][1]);
                row = (HSSFRow)sht0.GetRow(60);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJP3_Z[0][2]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJP3_Z[1][2]);
                //相对挠度
                row = (HSSFRow)sht0.GetRow(51);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_Z[0][0]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_Z[1][0]);
                row = (HSSFRow)sht0.GetRow(56);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_Z[0][1]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_Z[1][1]);
                row = (HSSFRow)sht0.GetRow(61);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_Z[0][2]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_Z[1][2]);
                //p3负压挠度
                //面法线挠度
                row = (HSSFRow)sht0.GetRow(50);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJP3_F[0][0]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJP3_F[1][0]);
                row = (HSSFRow)sht0.GetRow(55);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJP3_F[0][1]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJP3_F[1][1]);
                row = (HSSFRow)sht0.GetRow(60);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJP3_F[0][2]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJP3_F[1][2]);
                //相对挠度
                row = (HSSFRow)sht0.GetRow(51);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_F[0][0]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_F[1][0]);
                row = (HSSFRow)sht0.GetRow(56);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_F[0][1]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_F[1][1]);
                row = (HSSFRow)sht0.GetRow(61);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_F[0][2]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_F[1][2]);
                //p3最大挠度
                row = (HSSFRow)sht0.GetRow(62);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJp3_Z);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJp3_F);
                //损坏情况
                row = (HSSFRow)sht0.GetRow(63);
                cell = (HSSFCell)row.GetCell(6);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJP3_Z ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(10);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJP3_F ? "有" : "";
                cell.SetCellValue(str);
                #endregion

                #region 风荷载设计值Pmax
                //Pmax检测压力
                row = (HSSFRow)sht0.GetRow(45);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_DJPmax_Z);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_DJPmax_F);
                //Pmax正压检测位移
                //位移组1
                row = (HSSFRow)sht0.GetRow(46);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][0]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][0]);
                row = (HSSFRow)sht0.GetRow(47);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][1]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][1]);
                row = (HSSFRow)sht0.GetRow(48);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][2]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][2]);
                row = (HSSFRow)sht0.GetRow(49);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][3]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][3]);
                //位移组2
                row = (HSSFRow)sht0.GetRow(52);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][4]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][4]);
                row = (HSSFRow)sht0.GetRow(53);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][5]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][5]);
                row = (HSSFRow)sht0.GetRow(54);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][6]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][6]);
                //位移组3
                row = (HSSFRow)sht0.GetRow(57);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][7]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][7]);
                row = (HSSFRow)sht0.GetRow(58);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][8]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][8]);
                row = (HSSFRow)sht0.GetRow(59);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][9]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][9]);
                //Pmax负压检测位移
                //位移组1
                row = (HSSFRow)sht0.GetRow(46);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[0][0]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[1][0]);
                row = (HSSFRow)sht0.GetRow(47);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[0][1]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[1][1]);
                row = (HSSFRow)sht0.GetRow(48);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[0][2]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[1][2]);
                row = (HSSFRow)sht0.GetRow(49);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[0][3]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[1][3]);
                //位移组2
                row = (HSSFRow)sht0.GetRow(52);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[0][4]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[1][4]);
                row = (HSSFRow)sht0.GetRow(53);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[0][5]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[1][5]);
                row = (HSSFRow)sht0.GetRow(54);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[0][6]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[1][6]);
                //位移组3
                row = (HSSFRow)sht0.GetRow(57);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[0][7]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[1][7]);
                row = (HSSFRow)sht0.GetRow(58);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[0][8]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[1][8]);
                row = (HSSFRow)sht0.GetRow(59);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[0][9]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[1][9]);
                //Pmax正压挠度
                //面法线挠度
                row = (HSSFRow)sht0.GetRow(50);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_Z[0][0]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_Z[1][0]);
                row = (HSSFRow)sht0.GetRow(55);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_Z[0][1]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_Z[1][1]);
                row = (HSSFRow)sht0.GetRow(60);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_Z[0][2]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_Z[1][2]);
                //相对挠度
                row = (HSSFRow)sht0.GetRow(51);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_Z[0][0]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_Z[1][0]);
                row = (HSSFRow)sht0.GetRow(56);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_Z[0][1]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_Z[1][1]);
                row = (HSSFRow)sht0.GetRow(61);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_Z[0][2]);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_Z[1][2]);
                //Pmax负压挠度
                //面法线挠度
                row = (HSSFRow)sht0.GetRow(50);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_F[0][0]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_F[1][0]);
                row = (HSSFRow)sht0.GetRow(55);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_F[0][1]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_F[1][1]);
                row = (HSSFRow)sht0.GetRow(60);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_F[0][2]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_F[1][2]);
                //相对挠度
                row = (HSSFRow)sht0.GetRow(51);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_F[0][0]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_F[1][0]);
                row = (HSSFRow)sht0.GetRow(56);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_F[0][1]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_F[1][1]);
                row = (HSSFRow)sht0.GetRow(61);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_F[0][2]);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_F[1][2]);
                //Pmax最大挠度
                row = (HSSFRow)sht0.GetRow(62);
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJpmax_Z);
                cell = (HSSFCell)row.GetCell(23);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJpmax_F);
                //损坏情况
                row = (HSSFRow)sht0.GetRow(63);
                cell = (HSSFCell)row.GetCell(19);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJPmax_Z ? "有" : "";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(23);
                str = PublicData.ExpDQ.ExpData_KFY.Damage_DJPmax_F ? "有" : "";
                cell.SetCellValue(str);
                #endregion

                #region P3最终评定
                row = (HSSFRow)sht0.GetRow(64);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.PDValue_DJP3_Z);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.PDValue_DJP3_F);
                row = (HSSFRow)sht0.GetRow(65);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.PDValue_DJP3_All);
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.PdLevel_DJP3);
                #endregion

                #region 位移组参数
                //组1
                row = (HSSFRow)sht0.GetRow(67);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY .DisplaceGroups [0].Is_Use ?"启用":"未启用");
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].Is_TestMBBX ? "检测" : "不检测");
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].IsBZCSJMB  ? "检测" : "不检测");
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].ND_XD_YXFM);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].L);
                //组2
                row = (HSSFRow)sht0.GetRow(68);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.DisplaceGroups[1].Is_Use ? "启用" : "未启用");
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.DisplaceGroups[1].Is_TestMBBX ? "检测" : "不检测");
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.DisplaceGroups[1].IsBZCSJMB ? "检测" : "不检测");
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.DisplaceGroups[1].ND_XD_YXFM);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.DisplaceGroups[1].L);
                //组3
                row = (HSSFRow)sht0.GetRow(69);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.DisplaceGroups[2].Is_Use ? "启用" : "未启用");
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.DisplaceGroups[2].Is_TestMBBX ? "检测" : "不检测");
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.DisplaceGroups[2].IsBZCSJMB ? "检测" : "不检测");
                cell = (HSSFCell)row.GetCell(19);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.DisplaceGroups[2].ND_XD_YXFM);
                cell = (HSSFCell)row.GetCell(21);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.DisplaceGroups[2].L);
                #endregion


                #region 挠度曲线

                if (!PublicData.ExpDQ.Exp_KFY.IsGC)
                {
                    string imgPath1 = aimBmpPath.Clone().ToString() + "1.jpg";
                    string imgPath2 = aimBmpPath.Clone().ToString() + "2.jpg";

                    // 添加图片1
                    // 1.以字节数组的形式读取图片
                    byte[] buffer1 = GetImageBuffer(imgPath1);
                    // 2.向Excel添加图片，获取到图片索引
                    int picIdx1 = iWorkbookSXRPT.AddPicture(buffer1, PictureType.JPEG);
                    // 3.构建Excel图像
                    var drawing1 = sht0.CreateDrawingPatriarch();
                    // 4.确定图像的位置
                    // new XSSFClientAnchor(int dx1, int dy1, int dx2, int dy2, int col1, int row1, int col2, int row2)
                    // 前四个参数是单元格内的偏移量，这里需要填满整个单元格所以不设置。
                    // col1,row1表示图片的左上角在哪个单元格的左上角；col2,row2表示图片的右下角在哪个单元格的左上角
                    HSSFClientAnchor anchor1 = new HSSFClientAnchor(0, 0, 0, 0, 1, 70, 12, 71);
                    // 5.将图片设置到上面的位置中
                    drawing1.CreatePicture(anchor1, picIdx1);

                    // 添加图片2
                    // 1.以字节数组的形式读取图片
                    byte[] buffer2 = GetImageBuffer(imgPath2);
                    // 2.向Excel添加图片，获取到图片索引
                    int picIdx2 = iWorkbookSXRPT.AddPicture(buffer2, PictureType.JPEG);
                    // 3.构建Excel图像
                    var drawing2 = sht0.CreateDrawingPatriarch();
                    // 4.确定图像的位置
                    // new XSSFClientAnchor(int dx1, int dy1, int dx2, int dy2, int col1, int row1, int col2, int row2)
                    // 前四个参数是单元格内的偏移量，这里需要填满整个单元格所以不设置。
                    // col1,row1表示图片的左上角在哪个单元格的左上角；col2,row2表示图片的右下角在哪个单元格的左上角
                    HSSFClientAnchor anchor2 = new HSSFClientAnchor(0, 0, 0, 0, 12, 70, 25, 71);
                    // 5.将图片设置到上面的位置中
                    drawing2.CreatePicture(anchor2, picIdx2);
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region 三性报告第6页-抗风压工程
            try
            {
                ISheet sht0 = iWorkbookSXRPT.GetSheetAt(5);
                string str = "";
                HSSFRow row;
                HSSFCell cell;

                #region 工程变形正P1
                //检测压力
                row = (HSSFRow)sht0.GetRow(2);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCBX_Z[0]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCBX_Z[1]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCBX_Z[2]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCBX_Z[3]);
                //测点组1位移
                row = (HSSFRow)sht0.GetRow(3);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[0][0]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[1][0]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[2][0]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[3][0]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[4][0]);
                row = (HSSFRow)sht0.GetRow(4);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[0][1]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[1][1]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[2][1]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[3][1]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[4][1]);
                row = (HSSFRow)sht0.GetRow(5);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[0][2]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[1][2]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[2][2]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[3][2]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[4][2]);
                row = (HSSFRow)sht0.GetRow(6);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[0][3]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[1][3]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[2][3]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[3][3]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[4][3]);
                //组2位移
                row = (HSSFRow)sht0.GetRow(9);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[0][4]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[1][4]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[2][4]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[3][4]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[4][4]);
                row = (HSSFRow)sht0.GetRow(10);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[0][5]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[1][5]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[2][5]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[3][5]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[4][5]);
                row = (HSSFRow)sht0.GetRow(11);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[0][6]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[1][6]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[2][6]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[3][6]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[4][6]);
                //组3位移
                row = (HSSFRow)sht0.GetRow(14);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[0][7]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[1][7]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[2][7]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[3][7]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[4][7]);
                row = (HSSFRow)sht0.GetRow(15);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[0][8]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[1][8]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[2][8]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[3][8]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[4][8]);
                row = (HSSFRow)sht0.GetRow(16);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[0][9]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[1][9]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[2][9]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[3][9]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[4][9]);

                //面法线挠度
                row = (HSSFRow)sht0.GetRow(7);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[0][0]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[1][0]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[2][0]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[3][0]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[4][0]);
                row = (HSSFRow)sht0.GetRow(12);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[0][1]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[1][1]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[2][1]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[3][1]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[4][1]);
                row = (HSSFRow)sht0.GetRow(17);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[0][2]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[1][2]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[2][2]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[3][2]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[4][2]);

                //相对面法线挠度
                row = (HSSFRow)sht0.GetRow(8);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[0][0]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[1][0]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[2][0]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[3][0]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[4][0]);
                row = (HSSFRow)sht0.GetRow(13);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[0][1]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[1][1]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[2][1]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[3][1]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[4][1]);
                row = (HSSFRow)sht0.GetRow(18);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[0][2]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[1][2]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[2][2]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[3][2]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[4][2]);

                //损坏情况
                row = (HSSFRow)sht0.GetRow(19);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.Damage_GCBX_Z[0]?"有":"");
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.Damage_GCBX_Z[1] ? "有" : "");
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.Damage_GCBX_Z[2] ? "有" : "");
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.Damage_GCBX_Z[3] ? "有" : "");
                #endregion

                #region 工程变形负P1
                //检测压力
                row = (HSSFRow)sht0.GetRow(21);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCBX_F[0]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCBX_F[1]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCBX_F[2]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCBX_F[3]);
                //测点组1位移
                row = (HSSFRow)sht0.GetRow(22);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[0][0]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[1][0]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[2][0]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[3][0]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[4][0]);
                row = (HSSFRow)sht0.GetRow(23);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[0][1]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[1][1]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[2][1]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[3][1]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[4][1]);
                row = (HSSFRow)sht0.GetRow(24);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[0][2]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[1][2]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[2][2]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[3][2]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[4][2]);
                row = (HSSFRow)sht0.GetRow(25);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[0][3]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[1][3]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[2][3]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[3][3]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[4][3]);
                //组2位移
                row = (HSSFRow)sht0.GetRow(28);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[0][4]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[1][4]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[2][4]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[3][4]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[4][4]);
                row = (HSSFRow)sht0.GetRow(29);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[0][5]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[1][5]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[2][5]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[3][5]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[4][5]);
                row = (HSSFRow)sht0.GetRow(30);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[0][6]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[1][6]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[2][6]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[3][6]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[4][6]);
                //组3位移
                row = (HSSFRow)sht0.GetRow(33);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[0][7]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[1][7]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[2][7]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[3][7]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[4][7]);
                row = (HSSFRow)sht0.GetRow(34);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[0][8]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[1][8]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[2][8]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[3][8]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[4][8]);
                row = (HSSFRow)sht0.GetRow(35);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[0][9]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[1][9]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[2][9]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[3][9]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[4][9]);

                //面法线挠度
                row = (HSSFRow)sht0.GetRow(26);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[0][0]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[1][0]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[2][0]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[3][0]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[4][0]);
                row = (HSSFRow)sht0.GetRow(31);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[0][1]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[1][1]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[2][1]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[3][1]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[4][1]);
                row = (HSSFRow)sht0.GetRow(36);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[0][2]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[1][2]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[2][2]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[3][2]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[4][2]);

                //相对面法线挠度
                row = (HSSFRow)sht0.GetRow(27);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[0][0]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[1][0]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[2][0]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[3][0]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[4][0]);
                row = (HSSFRow)sht0.GetRow(32);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[0][1]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[1][1]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[2][1]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[3][1]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[4][1]);
                row = (HSSFRow)sht0.GetRow(37);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[0][2]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[1][2]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[2][2]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[3][2]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[4][2]);

                //损坏情况
                row = (HSSFRow)sht0.GetRow(38);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.Damage_GCBX_F[0] ? "有" : "");
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.Damage_GCBX_F[1] ? "有" : "");
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.Damage_GCBX_F[2] ? "有" : "");
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.Damage_GCBX_F[3] ? "有" : "");
                #endregion

                #region 反复加压p2
                //检测压力
                row = (HSSFRow)sht0.GetRow(40);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCP2_Z);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCP2_F);
                //损坏情况
                row = (HSSFRow)sht0.GetRow(41);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.Damage_GCP2_Z?"有":"");
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.Damage_GCP2_F ? "有" : "");
                #endregion

                #region 风荷载标准值p3
                //检测压力
                row = (HSSFRow)sht0.GetRow(43);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCP3_Z);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCP3_F);
                //测点组1位移
                row = (HSSFRow)sht0.GetRow(44);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[0][0]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[1][0]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[0][0]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[1][0]);
                row = (HSSFRow)sht0.GetRow(45);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[0][1]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[1][1]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[0][1]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[1][1]);
                row = (HSSFRow)sht0.GetRow(46);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[0][2]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[1][2]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[0][2]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[1][2]);
                row = (HSSFRow)sht0.GetRow(47);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[0][3]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[1][3]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[0][3]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[1][3]);
                //测点组2位移
                row = (HSSFRow)sht0.GetRow(50);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[0][4]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[1][4]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[0][4]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[1][4]);
                row = (HSSFRow)sht0.GetRow(51);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[0][5]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[1][5]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[0][5]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[1][5]);
                row = (HSSFRow)sht0.GetRow(52);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[0][6]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[1][6]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[0][6]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[1][6]);
                //测点组3位移
                row = (HSSFRow)sht0.GetRow(55);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[0][7]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[1][7]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[0][7]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[1][7]);
                row = (HSSFRow)sht0.GetRow(56);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[0][8]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[1][8]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[0][8]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[1][8]);
                row = (HSSFRow)sht0.GetRow(57);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[0][9]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[1][9]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[0][9]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[1][9]);

                //面法线挠度
                row = (HSSFRow)sht0.GetRow(48);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCP3_Z[0][0]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCP3_Z[1][0]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCP3_F[0][0]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCP3_F[1][0]);
                row = (HSSFRow)sht0.GetRow(53);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCP3_Z[0][1]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCP3_Z[1][1]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCP3_F[0][1]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCP3_F[1][1]);   
                row = (HSSFRow)sht0.GetRow(58);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCP3_Z[0][2]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCP3_Z[1][2]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCP3_F[0][2]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.ND_GCP3_F[1][2]);
                //相对面法线挠度
                row = (HSSFRow)sht0.GetRow(49);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_Z[0][0]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_Z[1][0]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_F[0][0]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_F[1][0]);
                row = (HSSFRow)sht0.GetRow(54);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_Z[0][1]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_Z[1][1]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_F[0][1]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_F[1][1]);
                row = (HSSFRow)sht0.GetRow(59);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_Z[0][2]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_Z[1][2]);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_F[0][2]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_F[1][2]);
                //损坏情况
                row = (HSSFRow)sht0.GetRow(60);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.Damage_GCP3_Z ? "有" : "");
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.Damage_GCP3_F ? "有" : "");
                #endregion

                #region 风荷载设计值pmax
                //检测压力
                row = (HSSFRow)sht0.GetRow(62);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCPmax_Z);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.TestPress_GCPmax_F);
                //损坏情况
                row = (HSSFRow)sht0.GetRow(63);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.Damage_GCPmax_Z ? "有" : "");
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.Damage_GCPmax_F ? "有" : "");
                #endregion

                #region 评定
                row = (HSSFRow)sht0.GetRow(64);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.IsMeetDesign_GCp1  ? "满足" : "不满足");
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.IsMeetDesign_GCp2 ? "满足" : "不满足");
                row = (HSSFRow)sht0.GetRow(65);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.IsMeetDesign_GCp3 ? "满足" : "不满足");
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.IsMeetDesign_GCpmax ? "满足" : "不满足");
                row = (HSSFRow)sht0.GetRow(66);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_KFY.IsMeetDesign_GCfinal ? "满足" : "不满足");
                #endregion

                #region 位移组参数
                //组1
                row = (HSSFRow)sht0.GetRow(67);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].ND_XD_YXFM);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].L);
                //组2
                row = (HSSFRow)sht0.GetRow(68);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.DisplaceGroups[1].ND_XD_YXFM);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.DisplaceGroups[1].L);
                //组3
                row = (HSSFRow)sht0.GetRow(69);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.DisplaceGroups[2].ND_XD_YXFM);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.Exp_KFY.DisplaceGroups[2].L);
                #endregion

                #region 挠度曲线

                if (PublicData.ExpDQ.Exp_KFY.IsGC)
                {
                    string imgPath1 = aimBmpPath.Clone().ToString() + "1.jpg";
                    string imgPath2 = aimBmpPath.Clone().ToString() + "2.jpg";

                    // 添加图片1
                    // 1.以字节数组的形式读取图片
                    byte[] buffer1 = GetImageBuffer(imgPath1);
                    // 2.向Excel添加图片，获取到图片索引
                    int picIdx1 = iWorkbookSXRPT.AddPicture(buffer1, PictureType.JPEG);
                    // 3.构建Excel图像
                    var drawing1 = sht0.CreateDrawingPatriarch();
                    // 4.确定图像的位置
                    // new XSSFClientAnchor(int dx1, int dy1, int dx2, int dy2, int col1, int row1, int col2, int row2)
                    // 前四个参数是单元格内的偏移量，这里需要填满整个单元格所以不设置。
                    // col1,row1表示图片的左上角在哪个单元格的左上角；col2,row2表示图片的右下角在哪个单元格的左上角
                    HSSFClientAnchor anchor1 = new HSSFClientAnchor(0, 0, 0, 0, 1, 70, 6, 71);
                    // 5.将图片设置到上面的位置中
                    drawing1.CreatePicture(anchor1, picIdx1);

                    // 添加图片2
                    // 1.以字节数组的形式读取图片
                    byte[] buffer2 = GetImageBuffer(imgPath2);
                    // 2.向Excel添加图片，获取到图片索引
                    int picIdx2 = iWorkbookSXRPT.AddPicture(buffer2, PictureType.JPEG);
                    // 3.构建Excel图像
                    var drawing2 = sht0.CreateDrawingPatriarch();
                    // 4.确定图像的位置
                    // new XSSFClientAnchor(int dx1, int dy1, int dx2, int dy2, int col1, int row1, int col2, int row2)
                    // 前四个参数是单元格内的偏移量，这里需要填满整个单元格所以不设置。
                    // col1,row1表示图片的左上角在哪个单元格的左上角；col2,row2表示图片的右下角在哪个单元格的左上角
                    HSSFClientAnchor anchor2 = new HSSFClientAnchor(0, 0, 0, 0, 6, 70, 9, 71);
                    // 5.将图片设置到上面的位置中
                    drawing2.CreatePicture(anchor2, picIdx2);
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion


            //数据最终导出
            try
            {
                iWorkbookSXRPT.GetCreationHelper().CreateFormulaEvaluator().EvaluateAll();        //强制更新公式
                FileStream streamSXRPT = File.OpenWrite(RptAimPathSX);
                iWorkbookSXRPT.Write(streamSXRPT);
                streamSXRPT.Flush();
                streamSXRPT.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        private static byte[] GetImageBuffer(string path)
        {
            return File.ReadAllBytes(path);
        }

        private void setPic(HSSFWorkbook workbook, IDrawing patriarch, string path, ISheet sheet, int rowline, int col)
        {
            if (string.IsNullOrEmpty(path)) return;
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            int pictureIdx = workbook.AddPicture(bytes, PictureType.JPEG);
            // 插图片的位置  HSSFClientAnchor（dx1,dy1,dx2,dy2,col1,row1,col2,row2) 后面再作解释
            HSSFClientAnchor anchor = new HSSFClientAnchor(70, 10, 0, 0, col, rowline, col + 1, rowline + 1);
            //把图片插到相应的位置
            HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);
        }
    }
}