/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 描述：
 *
 * ==================================================================================
 * 修改标记
 * 修改时间			        修改人			版本号			描述
 * 2022/2/8 15:09:54		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
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

        #region 保存试验至数据库

        /// <summary>
        /// 保存试验——B01动态风压校准参数和数据
        /// </summary>
        private void SaveTest_DTFYCali()
        {
            try
            {
               PublicData.MainDB.Storageable<SM_DTFY_Cali>(PublicData.SM_DTFY_CaliDQ).As("B01_SM_DTFY_Cali").ExecuteCommand(); //存在则更新，不存在则插入
            }
            catch (Exception e)
            {
                MessageBox.Show("保存B01表时错误." + e.ToString());

                // LogHelper.WriteErrLog("保存B01表时错误", e);//输出日志
            }
        }


        /// <summary>
        /// 保存试验——B02动态风压校准压力点参数和数据
        /// </summary>
        private void SaveTest_DTFYCaliPoint()
        {
            try
            {
                PublicData.MainDB.Storageable<List<SM_DTFY_Cali_PressPoint>>(PublicData.SM_DTFY_CaliDQ.PointList.ToList()).As("B02_SM_DTFY_Cali_PressPoint").ExecuteCommand(); //存在则更新，不存在则插入
            }
            catch (Exception e)
            {
                MessageBox.Show("保存B02表时错误." + e.ToString());

                // LogHelper.WriteErrLog("保存B02表时错误", e);//输出日志
            }
        }


        /// <summary>
        /// 保存试验——B03动态风压校准记录参数和数据
        /// </summary>
        private void SaveTest_DTFYCaliLogList()
        {
            try
            {
                foreach(var point in PublicData.SM_DTFY_CaliDQ.PointList)
                    PublicData.MainDB.Storageable<List<SM_DTFY_Cali_Log>>(point.LogList.ToList()).As("B03_SM_DTFY_Cali_Log").ExecuteCommand(); //存在则更新，不存在则插入
            }
            catch (Exception e)
            {
                MessageBox.Show("保存B03表时错误." + e.ToString());

                // LogHelper.WriteErrLog("保存B03表时错误", e);//输出日志
            }
        }


        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 插入试验记录——A30记录
        /// </summary>
        private void SaveExp_InsertLogs(SM_DTFY_Cali_Log testLog)
        {
            try
            {
                //删除已有Log
                List<SM_DTFY_Cali_Log> tempLogList = PublicData.MainDB.CopyNew().Queryable<SM_DTFY_Cali_Log>().AS("B03_SM_DTFY_Cali_Log").Where(it => it.TestNO == testLog.TestNO).Where(it => it.PointNo == testLog.PointNo && it.LogNo == testLog.LogNo).ToList();
                if (tempLogList.Count > 0)
                    PublicData.MainDB.CopyNew().Deleteable<SM_DTFY_Cali_Log>().AS("B03_SM_DTFY_Cali_Log").Where(it => it.TestNO == testLog.TestNO).Where(it => it.PointNo == testLog.PointNo && it.LogNo == testLog.LogNo).ExecuteCommand();

                PublicData.MainDB.CopyNew().Insertable<SM_DTFY_Cali_Log>(testLog).AS("B03_SM_DTFY_Cali_Log").ExecuteCommand();
            }
            catch (Exception e)
            {
                MessageBox.Show("保存B03表时错误." + e.ToString());

                //  LogHelper.WriteErrLog("插入A30记录时错误", e);//输出日志
            }
        }
        #endregion
    }
}
