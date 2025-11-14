/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQDFJ_MB.DAL
 * 文件名：  MQZH_ExpDAL
 * 版本号：  V1.0.0.0
 * 唯一标识：0b93af04-b14a-465f-b73d-2aed8ba76297
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/2/8 15:09:54
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
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MQDFJ_MB.Model.Exp_MB;

namespace MQDFJ_MB.DAL.DTFYCali
{
    /// <summary>
    /// 试验参数及数据读写操作类
    /// </summary>
    public partial class DTFYCaliTestDAL : ObservableObject
    {
        #region 载入试验

        /// <summary>
        /// 载入指定名称的动态风压校准试验（当前试验）
        /// </summary>
        /// <param name="expNo"></param>
        public bool LoadTestDTFYCaliByName(string givenExpName)
        {
            string loadExpNo = givenExpName.Clone().ToString();

            if (string.IsNullOrWhiteSpace(loadExpNo) || string.IsNullOrEmpty(loadExpNo))
            {
                MessageBox.Show("编号为空！", "错误提示");
                return false ;
            }

            if (loadExpNo == "FactoryExp")
            {
                MessageBox.Show("工厂试验不能载入");
                return false;
            }

            //检查是否存在
            if (!PublicData.MainDB.Queryable<SM_DTFY_Cali>().AS("B01_SM_DTFY_Cali").Any(it => it.TestNO == loadExpNo))
            {
                MessageBox.Show("未找到指定名字的动态风压校准试验："+ loadExpNo);
                return false;
            }

            //试验数据复位
            try
            {
                PublicData.SM_DTFY_CaliDQ.Reset();
            }
            catch (Exception e)
            {
                MessageBox.Show("动态风压校准试验数据复位错误：" + loadExpNo);
                return false;

                // LogHelper.WriteErrLog("DAL试验数据复位时错误", e);//输出日志
            }

            //载入指定试验在B01表中的数据
            try
            {
                SM_DTFY_Cali expLoadedB01 = PublicData.MainDB.Queryable<SM_DTFY_Cali>().AS("B01_SM_DTFY_Cali").First(it => it.TestNO == loadExpNo);   //查询第一条数据

                if (expLoadedB01 != null)
                {
                    PublicData.SM_DTFY_CaliDQ = expLoadedB01;
                }
                else
                {
                    MessageBox.Show("B01表中未找到指定试验");
                    return false;

                    // LogHelper.WriteErrLog("A01表中未找到指定试验");//输出日志
                }
            }
            catch (Exception e)
            {
                return false;

                //  LogHelper.WriteErrLog("载入A01表时错误", e);//输出日志
            }

            //载入指定试验在B02表中的数据
            try
            {
                List<SM_DTFY_Cali_PressPoint> expLoadedB02List = PublicData.MainDB.Queryable<SM_DTFY_Cali_PressPoint>().AS("B02_SM_DTFY_Cali_PressPoint").Where(it => it.TestNO == loadExpNo).ToList();   //查询第一条数据

                if (expLoadedB02List != null)
                    PublicData.SM_DTFY_CaliDQ.PointList = new ObservableCollection<SM_DTFY_Cali_PressPoint>(expLoadedB02List);
                else
                {
                    MessageBox.Show("B02表中未找到指定试验");
                    return false;

                    // LogHelper.WriteErrLog("A02表中未找到指定试验");//输出日志
                }
            }
            catch (Exception e)
            {
                return false;

               // LogHelper.WriteErrLog("载入A02表时错误", e);//输出日志
            }

            //载入指定试验在B03表中的log
            try
            {
                foreach (var point in PublicData.SM_DTFY_CaliDQ.PointList)
                {
                    List<SM_DTFY_Cali_Log> expLoadedB03 = PublicData.MainDB.Queryable<SM_DTFY_Cali_Log>().AS("B03_SM_DTFY_Cali_Log").Where(it => it.TestNO == loadExpNo).Where(it => it.PointNo == point.PointNo).ToList();
                    if (expLoadedB03.Count > 0)
                        point.LogList = new ObservableCollection<SM_DTFY_Cali_Log>(expLoadedB03);
                }
            }
            catch (Exception e)
            {
                return false;

                //   LogHelper.WriteErrLog("载入A30表内热冲击Log时错误", e);//输出日志
            }

            Messenger.Default.Send<string>(PublicData.SM_DTFY_CaliDQ.TestNO, "ExpLoaded");
            return true;
        }


        /// <summary>
        /// 读取动态风压校准试验列表
        /// </summary>
        private List<SM_DTFY_Cali> LoadTestList()
        {
            List<SM_DTFY_Cali> expLoadedB01 = new List<SM_DTFY_Cali>();
            try
            {
                expLoadedB01 = PublicData.MainDB.Queryable<SM_DTFY_Cali>().AS("B01_SM_DTFY_Cali").ToList();
                foreach (var test in expLoadedB01)
                    test.PointList = new ObservableCollection<SM_DTFY_Cali_PressPoint>(PublicData.MainDB.Queryable<SM_DTFY_Cali_PressPoint>().AS("B02_SM_DTFY_Cali_PressPoint").Where(it => it.TestNO == test.TestNO).ToList());
            }
            catch (Exception e)
            {
                //   LogHelper.WriteErrLog("载入A30表内热冲击Log时错误", e);//输出日志
            }
            return expLoadedB01;

        }


        /// <summary>
        /// 读取指定试验名、指定压力点序号的记录表
        /// </summary>
        private List<SM_DTFY_Cali_Log>  LoadLogList(string testNO,int pointNo)
        {
            List<SM_DTFY_Cali_Log> expLoadedB03 = new List<SM_DTFY_Cali_Log>();
            try
            {
                expLoadedB03 = PublicData.MainDB.Queryable<SM_DTFY_Cali_Log>().AS("B03_SM_DTFY_Cali_Log").Where(it => it.TestNO == testNO).Where(it => it.PointNo == pointNo).ToList();
            }
            catch (Exception e)
            {
                //   LogHelper.WriteErrLog("载入A30表内热冲击Log时错误", e);//输出日志
            }
            return expLoadedB03;

        }
        #endregion
    }
}
