/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2023/7/25 15:09:54
 * 描述：
 *
 * ==================================================================================
 * 修改标记
 * 修改时间			        修改人			版本号			描述
 * 2023/7/25 15:09:54		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using GalaSoft.MvvmLight;
using MQDFJ_MB.Model.Exp_MB;

namespace MQDFJ_MB.DAL.DTFYCali
{
    /// <summary>
    /// 试验参数及数据读写操作类
    /// </summary>
    public partial class DTFYCaliTestDAL : ObservableObject
    {
        /// <summary>
        /// 根据名称复制试验
        /// </summary>
        private bool CopyTestDTFYCali(string OldNo, string expNo)
        {
            try
            {
                //检查编号字符长度
                if (OldNo.Length <= 0)
                {
                    MessageBox.Show("旧编号长度有误", "错误提示");
                    return false;
                }
                if ((expNo.Length <= 0) || (expNo.Length > 50))
                {
                    MessageBox.Show("新试验编号长度有误", "错误提示");
                    return false;
                }

                string oldExpNo = OldNo.Clone().ToString();
                string newExpNo = expNo.Clone().ToString();

                #region 检查编号是否存在

                try
                {
                    bool isExistsExpInB01Old = PublicData.MainDB.Queryable<SM_DTFY_Cali>().AS("B01_SM_DTFY_Cali").Any(it => it.TestNO == OldNo);

                    if (!isExistsExpInB01Old)
                    {
                        MessageBox.Show("B01表中未找到指定编号的试验", "错误提示");
                        // LogHelper.WriteErrLog("删除试验，B01表中未找到指定编号的试验");//输出日志
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return false;
                }

                #endregion

                #region 检查新编号是否重复

                //检查编号是否重复
                try
                {
                    bool isExistsExpInB01New = PublicData.MainDB.Queryable<SM_DTFY_Cali>().AS("B01_SM_DTFY_Cali").Any(it => it.TestNO == expNo);

                    if (isExistsExpInB01New)
                    {
                        MessageBox.Show("此编号已存在，请勿与以往编号重复！", "错误提示");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                #endregion

                MessageBoxResult msgBoxResult = MessageBox.Show("确认新建" + newExpNo + "号试验？", "检查提示", MessageBoxButton.YesNo);
                if (msgBoxResult == MessageBoxResult.No)
                {
                    return false;
                }

                #region 复制表

                //B01主表
                try
                {
                    SM_DTFY_Cali expOld = PublicData.MainDB.Queryable<SM_DTFY_Cali>().AS("B01_SM_DTFY_Cali").First(it => it.TestNO == oldExpNo); //查询第一条数据
                    expOld.TestNO = newExpNo;

                    PublicData.MainDB.Insertable<SM_DTFY_Cali>(expOld).AS("B01_SM_DTFY_Cali").ExecuteCommand();
                    
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    return false;
                    // LogHelper.WriteErrLog("复制A01表数据时错误", e);//输出日志
                }

                //B02表
                try
                {
                    List<SM_DTFY_Cali_PressPoint> expOldList = PublicData.MainDB.Queryable<SM_DTFY_Cali_PressPoint>().AS("B02_SM_DTFY_Cali_PressPoint").Where(it => it.TestNO == oldExpNo).ToList();
                    foreach (var log in expOldList)
                        log.TestNO = newExpNo;
                    PublicData.MainDB.Insertable<SM_DTFY_Cali_PressPoint>(expOldList).AS("B02_SM_DTFY_Cali_PressPoint").ExecuteCommand();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    return false;
                    //LogHelper.WriteErrLog("复制A30表数据时错误", e);//输出日志
                }

                //B03表
                try
                {
                    List<SM_DTFY_Cali_Log> expOldList = PublicData.MainDB.Queryable<SM_DTFY_Cali_Log>().AS("B03_SM_DTFY_Cali_Log").Where(it => it.TestNO == oldExpNo).ToList();
                    foreach (var log in expOldList)
                        log.TestNO = newExpNo;
                    PublicData.MainDB.Insertable<SM_DTFY_Cali_Log>(expOldList).AS("B03_SM_DTFY_Cali_Log").ExecuteCommand();
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    return false;
                    //LogHelper.WriteErrLog("复制A30表数据时错误", e);//输出日志
                }
                #endregion
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
                // LogHelper.WriteErrLog("复制A01表数据时错误", e);//输出日志
            }

        }
    }
}