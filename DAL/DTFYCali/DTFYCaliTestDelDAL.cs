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
        /// 删除动态风压校准试验
        /// </summary>
        private bool DelTestDTFYCaliByName(string expNo)
        {
            string delExpNo = expNo.Clone().ToString();

            //检查编号是否存在
            if ((delExpNo == null) || (delExpNo == string.Empty))
            {
                MessageBox.Show("编号为空！", "错误提示");
                return false;
            }
            //当前试验、工厂试验、默认试验不允许删除
            if (delExpNo == PublicData.SM_DTFY_CaliDQ.TestNO)
            {
                MessageBox.Show("当前试验无法删除，如需删除请先关闭实验！", "错误提示");
                return false;
            }
            if (delExpNo == "DefaultExp")
            {
                MessageBox.Show("系统默认试验，不允许删除！", "错误提示");
                return false;
            }
            if (delExpNo == "FactoryExp")
            {
                MessageBox.Show("工厂设定试验，不允许删除", "错误提示");
                return false;
            }
            try
            {
                SM_DTFY_Cali expDelB01 = PublicData.MainDB.Queryable<SM_DTFY_Cali>().AS("B01_SM_DTFY_Cali").First(it => it.TestNO == delExpNo);   //查询第一条数据
                if (expDelB01.Flag_Using)
                {
                    MessageBox.Show("在用的校准试验，不允许删除", "错误提示");
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("查询在用校准试验时出错", "错误提示");
                return false;
                // LogHelper.WriteErrLog("删除A01表时错误", e);//输出日志
            }

            //删除前确认
            MessageBoxResult msgBoxResult = MessageBox.Show("确认删除" + delExpNo + "号实验？试验信息和实验数据将被全部删除，且无法恢复！",
                "检查提示", MessageBoxButton.YesNo);
            if (msgBoxResult == MessageBoxResult.No)
            {
                return false;
            }

            //删除B01表格
            try
            {
                bool isExistsExpInB01 = PublicData.MainDB.Queryable<SM_DTFY_Cali>().AS("B01_SM_DTFY_Cali").Any(it => it.TestNO == delExpNo);

                if (!isExistsExpInB01)
                {
                    MessageBox.Show("B01表中未找到指定编号的试验", "错误提示");
                    // LogHelper.WriteErrLog("删除试验，B01表中未找到指定编号的试验");//输出日志
                }
                PublicData.MainDB.Deleteable<SM_DTFY_Cali>().AS("B01_SM_DTFY_Cali").Where(it => it.TestNO == delExpNo).ExecuteCommand();
            }
            catch (Exception e)
            {
                MessageBox.Show("删除B01表数据时出错", "错误提示");
                return false;

                // LogHelper.WriteErrLog("删除A01表时错误", e);//输出日志
            }

            //删除B02表格
            try
            {
                bool isExistsExpInB02 = PublicData.MainDB.Queryable<SM_DTFY_Cali_PressPoint>().AS("B02_SM_DTFY_Cali_PressPoint").Any(it => it.TestNO == delExpNo);

                if (!isExistsExpInB02)
                {
                    MessageBox.Show("B02表中未找到指定编号的试验", "错误提示");
                    // LogHelper.WriteErrLog("删除试验，B02表中未找到指定编号的试验");//输出日志
                }
                PublicData.MainDB.Deleteable<SM_DTFY_Cali_PressPoint>().AS("B02_SM_DTFY_Cali_PressPoint").Where(it => it.TestNO == delExpNo).ExecuteCommand();
            }
            catch (Exception e)
            {
                MessageBox.Show("删除B02表数据时出错", "错误提示");
                return false;

                // LogHelper.WriteErrLog("删除B02表时错误", e);//输出日志
            }

            //删除B03表格
            try
            {
                PublicData.MainDB.Deleteable<SM_DTFY_Cali_Log>().AS("B03_SM_DTFY_Cali_Log").Where(it => it.TestNO == delExpNo).ExecuteCommand();
            }
            catch (Exception e)
            {
                MessageBox.Show("删除B03表数据时出错", "错误提示");
                return false;

                // LogHelper.WriteErrLog("删除B02表时错误", e);//输出日志
            }
            return true;
        }
    }
}