/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQDFJ_MB.DAL
 * 文件名：  MQZH_RepDAL
 * 版本号：  V1.0.0.0
 * 唯一标识：eb830ea8-3bc1-4ce9-9718-111d01b1f89e
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/2/8 15:10:31
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
        #region 导出层间变形报告子函数

        /// <summary>
        /// 层间变形报告导出子函数
        /// </summary>
        private void SaveCJBXRPT()
        {
            IWorkbook iWorkbookCJBXRPT = null;

            //打开模板文件
            try
            {
                FileStream sCJBXFormfile = new FileStream(RptFormPathCJBX, FileMode.Open, FileAccess.Read);
                iWorkbookCJBXRPT = WorkbookFactory.Create(sCJBXFormfile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            //复制模板文件内容
            try
            {
                int tempCJBXRPTSheets = iWorkbookCJBXRPT.NumberOfSheets;
                for (int i = 0; i < tempCJBXRPTSheets; ++i)
                {
                    ISheet ish = iWorkbookCJBXRPT.GetSheetAt(i);
                    ish.DisplayGridlines = true;
                    ish.DisplayRowColHeadings = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
            //修改数据
            #region 层间变形第1页
            try
            {
                ISheet sht0 = iWorkbookCJBXRPT.GetSheetAt(0);
                string str = "";
                //单位名头
                HSSFRow row = (HSSFRow)sht0.GetRow(0);
                HSSFCell cell = (HSSFCell)row.GetCell(0);
                cell.SetCellValue(PublicData.Dev .MQZH_CompanyName);
                //报告编号
                row = (HSSFRow)sht0.GetRow(2);
               cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ .ExpSettingParam .RepCJBXNO);
                //基本信息
                row = (HSSFRow)sht0.GetRow(3);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.ExpWTDW );        //委托单位
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.ExpWTDate );        //委托日期
                row = (HSSFRow)sht0.GetRow(4);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.ExpSCDW);        //生产单位
                cell = (HSSFCell)row.GetCell(13);
                str = PublicData.ExpDQ.Exp_CJBX.IsGC ? "工程检测" : "定级检测";
                cell.SetCellValue(str);        //检验类别
                row = (HSSFRow)sht0.GetRow(5);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.ExpSGDW );        //施工单位
                row = (HSSFRow)sht0.GetRow(6);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.ExpGCMC );        //工程名称
                row = (HSSFRow)sht0.GetRow(7);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Name);        //样品名称
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Series );        //样品系列
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Model);        //样品规格型号
                //检测相关信息
                row = (HSSFRow)sht0.GetRow(8);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.Dev .MQZH_LabAddr );        //检验地点（实验室地址）
                row = (HSSFRow)sht0.GetRow(9);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue("√");        //X轴
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue("√");        //Y轴
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue("√");        //Z轴
                //检测结论
                row = (HSSFRow)sht0.GetRow(13);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ .ExpData_CJBX .Level_DJ_X);        //X轴评级
                row = (HSSFRow)sht0.GetRow(14);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.Level_DJ_Y);        //Y轴评级
                row = (HSSFRow)sht0.GetRow(15);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.Level_DJ_Z);        //Z轴评级
                row = (HSSFRow)sht0.GetRow(16);
                cell = (HSSFCell)row.GetCell(4);
                str = PublicData.ExpDQ.ExpData_CJBX .IsMeetDesign_GC_X   ? "满足工程使用要求。" : "不满足工程使用要求。";
                cell.SetCellValue(str);        //工程X轴
                row = (HSSFRow)sht0.GetRow(17);
                cell = (HSSFCell)row.GetCell(4);
                str = PublicData.ExpDQ.ExpData_CJBX.IsMeetDesign_GC_Y ? "满足工程使用要求。" : "不满足工程使用要求。";
                cell.SetCellValue(str);        //工程Y轴
                row = (HSSFRow)sht0.GetRow(18);
                cell = (HSSFCell)row.GetCell(4);
                str = PublicData.ExpDQ.ExpData_CJBX.IsMeetDesign_GC_Z ? "满足工程使用要求。" : "不满足工程使用要求。";
                cell.SetCellValue(str);        //工程Z轴
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            #endregion 

            #region 层间变形第2页

            try
            {
                ISheet sht0 = iWorkbookCJBXRPT.GetSheetAt(1);
                string str = "";
                //报告编号
                HSSFRow row = (HSSFRow)sht0.GetRow(2);
                HSSFCell cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.RepCJBXNO);
                //试件信息及参数
                row = (HSSFRow)sht0.GetRow(1);
                cell = (HSSFCell)row.GetCell(1);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Width );         //试件宽度
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Heigth );         //试件高度
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Aria);         //试件面积

                row = (HSSFRow)sht0.GetRow(1);
                cell = (HSSFCell)row.GetCell(1);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Width);         //试件宽度
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Heigth);         //试件高度
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Aria);         //试件面积

                row = (HSSFRow)sht0.GetRow(2);
                cell = (HSSFCell)row.GetCell(1);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MBPZ );         //面板品种
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MBCZ );         //面板材质
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MBPH );         //面板牌号
                row = (HSSFRow)sht0.GetRow(3);
                cell = (HSSFCell)row.GetCell(1);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MBZDCC  );         //面板尺寸
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MBAZFF );         //面板安装方法

                row = (HSSFRow)sht0.GetRow(4);
                cell = (HSSFCell)row.GetCell(1);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_XCPZ );         //型材品种
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_XCGG );         //型材尺寸
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_XCPH );         //型材牌号
                
                row = (HSSFRow)sht0.GetRow(5);
                cell = (HSSFCell)row.GetCell(1);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_XQCLPZ );         //镶嵌材料品种
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_XQCLCZ);         //镶嵌材料材质
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue("//");         //型材材质
                // cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_XCCZ);         //型材材质
                row = (HSSFRow)sht0.GetRow(6);
                cell = (HSSFCell)row.GetCell(1);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_XQCC );         //镶嵌材料尺寸
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_XQFF);         //镶嵌方法
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_XQPH );         //镶嵌牌号

                row = (HSSFRow)sht0.GetRow(7);
                cell = (HSSFCell)row.GetCell(1);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MFCLPZ );         //密封材料品种
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MFCLCZ);         //密封材料材质
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MFCLPH );         //密封牌号
                
                row = (HSSFRow)sht0.GetRow(8);
                cell = (HSSFCell)row.GetCell(1);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_FJPZ );         //附件品种
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_FJCZ);         //附件材质
                cell = (HSSFCell)row.GetCell(9);
                // cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_FJPH );         //附件牌号
                cell.SetCellValue("//");         //附件牌号

                row = (HSSFRow)sht0.GetRow(9);
                cell = (HSSFCell)row.GetCell(1);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.SJ_CG);         //层高
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpSettingParam.Exp_SJ_ZDFGCC);         //最大分割尺寸

                //设计指标
                row = (HSSFRow)sht0.GetRow(10);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.Exp_CJBX .CJBX_SJZBX );         //X轴设计位移角
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.Exp_CJBX.CJBX_SJZBY);         //Y轴设计位移角
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.Exp_CJBX.CJBX_SJZBZ);         //Z轴设计位移量

                //检测结果
                row = (HSSFRow)sht0.GetRow(13);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.MaxAngleFM_DJ_X );         //X轴定级检测最大位移角
                row = (HSSFRow)sht0.GetRow(14);
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.MaxAngleFM_DJ_Y);         //Y轴定级检测最大位移角
                row = (HSSFRow)sht0.GetRow(15);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.MaxDSPL_DJ_Z );         //Z轴定级检测最大位移量

                row = (HSSFRow)sht0.GetRow(16);
                cell = (HSSFCell)row.GetCell(4);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_GC_X ? "发生损坏或功能障碍。" : "未发生损坏或功能障碍。";
                cell.SetCellValue(str);         //工程X轴检测是否损坏
                row = (HSSFRow)sht0.GetRow(17);
                cell = (HSSFCell)row.GetCell(4);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_GC_Y ? "发生损坏或功能障碍。" : "未发生损坏或功能障碍。";
                cell.SetCellValue(str);         //工程X轴检测是否损坏
                row = (HSSFRow)sht0.GetRow(18);
                cell = (HSSFCell)row.GetCell(4);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_GC_Z ? "发生损坏或功能障碍。" : "未发生损坏或功能障碍。";
                cell.SetCellValue(str);         //工程X轴检测是否损坏
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            #endregion 
            
            #region 层间变形第3页
            try
            {
                ISheet sht0 = iWorkbookCJBXRPT.GetSheetAt(2);
                string str = "";
                //定级X轴数据
                HSSFRow row = (HSSFRow)sht0.GetRow(2);              //位移角
                HSSFCell cell = (HSSFCell)row.GetCell(4);           
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_X[0] );
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_X[1] );
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_X[2]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_X[3] );
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_X[4] );
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_X[5] );
                //评级
                cell = (HSSFCell)row.GetCell(15); 
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.Level_DJ_X);
                //位移量
                row = (HSSFRow)sht0.GetRow(3);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_X[0]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_X[1]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_X[2]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_X[3]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_X[4]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_X[5]);
                //损坏情况
                row = (HSSFRow)sht0.GetRow(4);                  
                cell = (HSSFCell)row.GetCell(3);
                str = "未损坏";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(5);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_X[0] ? "有损坏" : "未损坏";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(7);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_X[1] ? "有损坏" : "未损坏";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(9);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_X[2] ? "有损坏" : "未损坏";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(11);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_X[3] ? "有损坏" : "未损坏";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(13);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_X[4] ? "有损坏" : "未损坏";
                cell.SetCellValue(str);

                //定级Y轴数据
                row = (HSSFRow)sht0.GetRow(5);              //位移角
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_Y[0] );
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_Y[1]);
                cell = (HSSFCell)row.GetCell(8);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_Y[2]);
                cell = (HSSFCell)row.GetCell(10);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_Y[3]);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_Y[4]);
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_Y[5]);
                //评级
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.Level_DJ_Y);
                //位移量
                row = (HSSFRow)sht0.GetRow(6);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Y[0]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Y[1]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Y[2]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Y[3]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Y[4]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Y[5]);
                //损坏情况
                row = (HSSFRow)sht0.GetRow(7);
                cell = (HSSFCell)row.GetCell(3);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Y[0] ? "有损坏" : "未损坏";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(5);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Y[1] ? "有损坏" : "未损坏";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(7);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Y[2] ? "有损坏" : "未损坏";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(9);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Y[3] ? "有损坏" : "未损坏";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(11);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Y[4] ? "有损坏" : "未损坏";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(13);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Y[5] ? "有损坏" : "未损坏";
                cell.SetCellValue(str);
                
                //定级Z轴数据
                //位移量
                row = (HSSFRow)sht0.GetRow(8);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Z[0]);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Z[1]);
                cell = (HSSFCell)row.GetCell(7);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Z[2]);
                cell = (HSSFCell)row.GetCell(9);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Z[3]);
                cell = (HSSFCell)row.GetCell(11);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Z[4]);
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Z[5]);
                //评级
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.Level_DJ_Z);
                //损坏情况
                row = (HSSFRow)sht0.GetRow(9);
                cell = (HSSFCell)row.GetCell(3);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Z[0] ? "有损坏" : "未损坏";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(5);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Z[1] ? "有损坏" : "未损坏";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(7);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Z[2] ? "有损坏" : "未损坏";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(9);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Z[3] ? "有损坏" : "未损坏";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(11);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Z[4] ? "有损坏" : "未损坏";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(13);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Z[5] ? "有损坏" : "未损坏";
                cell.SetCellValue(str);

                //工程X轴数据
                row = (HSSFRow)sht0.GetRow(10);              //位移角
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.AngleFM_GC_X * 2);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.AngleFM_GC_X);
                cell = (HSSFCell)row.GetCell(8);
                //评级
                cell = (HSSFCell)row.GetCell(15);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_GC_X ? "不满足" : "满足";
                cell.SetCellValue(str);
                //位移量
                row = (HSSFRow)sht0.GetRow(11);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_GC_X / 2);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_GC_X);
                //损坏情况
                row = (HSSFRow)sht0.GetRow(12);
                cell = (HSSFCell)row.GetCell(3);
                str = "未损坏";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(5);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_GC_X ? "有损坏" : "未损坏";
                cell.SetCellValue(str);

                //定级Y轴数据
                row = (HSSFRow)sht0.GetRow(13);              //位移角
                cell = (HSSFCell)row.GetCell(4);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.AngleFM_GC_Y * 2);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.AngleFM_GC_Y);
                cell = (HSSFCell)row.GetCell(8);
                //评级
                cell = (HSSFCell)row.GetCell(15);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_GC_Y ? "不满足" : "满足";
                cell.SetCellValue(str);
                //位移量
                row = (HSSFRow)sht0.GetRow(14);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_GC_Y / 2);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_GC_Y);
                //损坏情况
                row = (HSSFRow)sht0.GetRow(15);
                cell = (HSSFCell)row.GetCell(3);
                str = "未损坏";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(5);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_GC_Y ? "有损坏" : "未损坏";
                cell.SetCellValue(str);

                //定级Z轴数据
                row = (HSSFRow)sht0.GetRow(16);              //位移角
                //评级
                cell = (HSSFCell)row.GetCell(15);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_GC_Z ? "不满足" : "满足";
                cell.SetCellValue(str);
                //位移量
                row = (HSSFRow)sht0.GetRow(16);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_GC_Z / 2);
                cell = (HSSFCell)row.GetCell(5);
                cell.SetCellValue(PublicData.ExpDQ.ExpData_CJBX.DSPL_GC_Z);
                //损坏情况
                row = (HSSFRow)sht0.GetRow(17);
                cell = (HSSFCell)row.GetCell(3);
                str = "未损坏";
                cell.SetCellValue(str);
                cell = (HSSFCell)row.GetCell(5);
                str = PublicData.ExpDQ.ExpData_CJBX.IsDamage_GC_Z ? "有损坏" : "未损坏";
                cell.SetCellValue(str);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion


            //数据最终导出
            try
            {
                FileStream streamCJBXRPT = File.OpenWrite(RptAimPathCJBX);
                iWorkbookCJBXRPT.Write(streamCJBXRPT);
                streamCJBXRPT.Flush();
                streamCJBXRPT.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion
    }
}
