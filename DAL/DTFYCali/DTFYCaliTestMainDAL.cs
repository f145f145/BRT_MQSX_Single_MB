/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2025/11/11 15:09:54
 * 描述：
 *
 * ==================================================================================
 * 修改标记
 * 修改时间			        修改人			版本号			描述
 * 2025/11/11 15:09:54		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Data;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MQDFJ_MB.Model;
using MQDFJ_MB.Model.Exp_MB;

namespace MQDFJ_MB.DAL.DTFYCali
{
    /// <summary>
    /// 试验参数及数据读写操作类
    /// </summary>
    public partial class DTFYCaliTestDAL : ObservableObject
    {
        public DTFYCaliTestDAL()
        {
            DataBaseInit_Exp();

            // Messenger.Default.Send<MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数DataTable>(A00Table, "ExpTableChanged");

            //动态风压校准试验操作指令消息
            Messenger.Default.Register<string>(this, "LoadExpByNameDTFYCali", LoadTestMessage);
            Messenger.Default.Register<string>(this, "NewExpDTFYCali", NewTestMessage);
            Messenger.Default.Register<string>(this, "DelExpByNameDTFYCali", DelTestMessage);
            Messenger.Default.Register<string[]>(this, "CopyExpDTFYCali", CopyTestMessage);
            Messenger.Default.Register<string>("SaveExpDTFYCali", SaveTestMessage);
            //保存测试记录
            Messenger.Default.Register<SM_DTFY_Cali_Log>(this, "InsertLogsMessage", InsertLogMessage);
            //注册更新试验列表、记录列表
            Messenger.Default.Register<string>(this, "NeedUpdate_DTFYCali", NeedUpdate_DTFYCaliMessage);
            //更新试验列表
            Messenger.Default.Send<string>("DTFYCali_TestListForView", "NeedUpdate_DTFYCali");

        }


        #region 公共数据


        /// <summary>
        /// 公共数据
        /// </summary>
        private PublicDatas _publicData = PublicDatas.GetInstance();
        /// <summary>
        /// 公共数据
        /// </summary>
        public PublicDatas PublicData
        {
            get { return _publicData; }
            set
            {
                _publicData = value;
                RaisePropertyChanged(() => _publicData);
            }
        }

        #endregion


        #region 数据库初始化、更新

        /// <summary>
        /// 试验数据库初始化、读取数据
        /// </summary>
        private void DataBaseInit_Exp()
        {
            try
            {
                //创建sql打印输出，调试用
                PublicData.MainDB.Aop.OnLogExecuting = (sql, pars) =>
                {
                    Console.WriteLine(sql);//输出sql
                    Console.WriteLine(string.Join(",", pars?.Select(it => it.ParameterName + ":" + it.Value)));//参数

                    //5.0.8.2 获取无参数化 SQL 
                    //UtilMethods.GetSqlString(DbType.SqlServer,sql,pars)
                };
            }
            catch (Exception e)
            {
                MessageBox.Show("初始化测试数据库打印输出时出错." + e.ToString());
                //  LogHelper.WriteErrLog("初始化测试数据库打印输出时出错", e);//输出日志
            }


            //查询B01_SM_DTFY_Cali表是否存在
            try
                {
                    bool isExistsB01 = PublicData.MainDB.DbMaintenance.IsAnyTable("B01_SM_DTFY_Cali", false);
                    if (!isExistsB01)
                    {
                        PublicData.MainDB.CodeFirst.As<SM_DTFY_Cali>("B01_SM_DTFY_Cali").InitTables<SM_DTFY_Cali>();
                        MessageBox.Show("数据库中未找到B01表，已重新建立", "错误警告");
                       // LogHelper.WriteErrLog("DAL初始化，数据库中未找到B01表，已重新建立");//输出日志
                    }
                    var b01List = PublicData.MainDB.Queryable<SM_DTFY_Cali>().AS("B01_SM_DTFY_Cali").ToList();               //查找所有
                    if ((b01List.Count == 0) || (b01List == null))
                    {
                        SM_DTFY_Cali newB01Default = new SM_DTFY_Cali();
                        PublicData.MainDB.Storageable<SM_DTFY_Cali>(newB01Default).As("B01_SM_DTFY_Cali").ExecuteCommand(); //存在则更新，不存在则插入
                        MessageBox.Show("主数据库B01表为空，已重新建立", "错误警告");
                        //LogHelper.WriteErrLog("DAL初始化，主数据库B01表为空，已重新建立");//输出日志
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("初始化B01表时错误." + e.ToString());
                  //  LogHelper.WriteErrLog("初始化B01表时错误", e);//输出日志
                }

            //查询B01_SM_DTFY_Cali表是否存在
            try
            {
                bool isExistsB01 = PublicData.MainDB.DbMaintenance.IsAnyTable("B01_SM_DTFY_Cali", false);
                if (!isExistsB01)
                {
                    PublicData.MainDB.CodeFirst.As<SM_DTFY_Cali>("B01_SM_DTFY_Cali").InitTables<SM_DTFY_Cali>();
                    MessageBox.Show("数据库中未找到B01表，已重新建立", "错误警告");
                    // LogHelper.WriteErrLog("DAL初始化，数据库中未找到B01表，已重新建立");//输出日志
                }
                var b01List = PublicData.MainDB.Queryable<SM_DTFY_Cali>().AS("B01_SM_DTFY_Cali").ToList();               //查找所有
                if ((b01List.Count == 0) || (b01List == null))
                {
                    SM_DTFY_Cali newB01Default = new SM_DTFY_Cali();
                    PublicData.MainDB.Storageable<SM_DTFY_Cali>(newB01Default).As("B01_SM_DTFY_Cali").ExecuteCommand(); //存在则更新，不存在则插入
                    MessageBox.Show("主数据库B01表为空，已重新建立", "错误警告");
                    //LogHelper.WriteErrLog("DAL初始化，主数据库B01表为空，已重新建立");//输出日志
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("初始化B01表时错误." + e.ToString());
                //  LogHelper.WriteErrLog("初始化B01表时错误", e);//输出日志
            }


            //查询B02_SM_DTFY_Cali_PressPoint表是否存在
            try
            {
                bool isExistsB02 = PublicData.MainDB.DbMaintenance.IsAnyTable("B02_SM_DTFY_Cali_PressPoint", false);
                if (!isExistsB02)
                {
                    PublicData.MainDB.CodeFirst.As<SM_DTFY_Cali_PressPoint>("B02_SM_DTFY_Cali_PressPoint").InitTables<SM_DTFY_Cali_PressPoint>();
                    MessageBox.Show("数据库中未找到B02表，已重新建立", "错误警告");
                    // LogHelper.WriteErrLog("DAL初始化，数据库中未找到B01表，已重新建立");//输出日志
                }
                var b02List = PublicData.MainDB.Queryable<SM_DTFY_Cali_PressPoint>().AS("B02_SM_DTFY_Cali_PressPoint").ToList();               //查找所有
                if ((b02List.Count == 0) || (b02List == null))
                {
                    SM_DTFY_Cali_PressPoint newB02Default = new SM_DTFY_Cali_PressPoint();
                    PublicData.MainDB.Storageable<SM_DTFY_Cali_PressPoint>(newB02Default).As("B02_SM_DTFY_Cali_PressPoint").ExecuteCommand(); //存在则更新，不存在则插入
                    MessageBox.Show("主数据库B02表为空，已重新建立", "错误警告");
                    //LogHelper.WriteErrLog("DAL初始化，主数据库B02表为空，已重新建立");//输出日志
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("初始化B02表时错误." + e.ToString());
                //  LogHelper.WriteErrLog("初始化B02表时错误", e);//输出日志
            }


            //查询B03_SM_DTFY_Cali_Log表是否存在
            try
            {
                bool isExistsB03 = PublicData.MainDB.DbMaintenance.IsAnyTable("B03_SM_DTFY_Cali_Log", false);
                if (!isExistsB03)
                {
                    PublicData.MainDB.CodeFirst.As<SM_DTFY_Cali_Log>("B03_SM_DTFY_Cali_Log").InitTables<SM_DTFY_Cali_Log>();
                    MessageBox.Show("数据库中未找到B03表，已重新建立", "错误警告");
                    // LogHelper.WriteErrLog("DAL初始化，数据库中未找到B03表，已重新建立");//输出日志
                }
                var b03List = PublicData.MainDB.Queryable<SM_DTFY_Cali_Log>().AS("B03_SM_DTFY_Cali_Log").ToList();               //查找所有
                if ((b03List.Count == 0) || (b03List == null))
                {
                    SM_DTFY_Cali_Log newB03Default = new SM_DTFY_Cali_Log();
                    PublicData.MainDB.Storageable<SM_DTFY_Cali_Log>(newB03Default).As("B03_SM_DTFY_Cali_Log").ExecuteCommand(); //存在则更新，不存在则插入
                    MessageBox.Show("主数据库B03表为空，已重新建立", "错误警告");
                    //LogHelper.WriteErrLog("DAL初始化，主数据库B03表为空，已重新建立");//输出日志
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("初始化B03表时错误." + e.ToString());
                //  LogHelper.WriteErrLog("初始化B03表时错误", e);//输出日志
            }

        }

        #endregion


        #region 试验管理、操作消息回调


        /// <summary>
        /// 根据指定编号新建动态风压校准试验（从默认试验复制）
        /// </summary>
        /// <param name="msg">新建试验编号</param>
        private void NewTestMessage(string msg)
        {
            string oldExpName = "DefaultExp";
            string newExpName = msg;
            if (CopyTestDTFYCali(oldExpName, newExpName))
            {
                Messenger.Default.Send<string>("DTFYCali_TestListForView", "NeedUpdate_DTFYCali");
                MessageBox.Show(newExpName + "号试验已新建完毕！", "提示");
            }
            else
            {
                MessageBox.Show("新建试验失败，数据库中可能残存信息！", "错误提示");
            }
        }


        /// <summary>
        /// 复制指定名称的动态风压校准试验至新试验
        /// </summary>
        /// <param name="msg">被复制试验编号和新试验编号</param>
        private void CopyTestMessage(string[] msg)
        {
            string oldExpName = msg[0];
            string newExpName = msg[1];
            if (CopyTestDTFYCali(oldExpName, newExpName))
            {
                Messenger.Default.Send<string>("DTFYCali_TestListForView", "NeedUpdate_DTFYCali");
                MessageBox.Show("已复制" + oldExpName + "号试验至" + newExpName + "号试验。", "提示");
            }
            else
            {
                MessageBox.Show("复制试验失败，数据库中可能残存信息！", "错误提示");
            }
        }


        /// <summary>
        ///根据编号删除动态风压校准试验消息回调
        /// </summary>
        /// <param name="msg">试验编号</param>
        private void DelTestMessage(string msg)
        {
            if (DelTestDTFYCaliByName(msg))
            {
                Messenger.Default.Send<string>("DTFYCali_TestListForView", "NeedUpdate_DTFYCali");
                MessageBox.Show(msg + "号试验已删除！", "提示");
            }
            else
            {
                MessageBox.Show("删除试验失败！", "错误提示");
            }
        }


        /// <summary>
        ///根据编号载入动态风压校准试验消息回调
        /// </summary>
        /// <param name="msg">试验编号</param>
        private void LoadTestMessage(string msg)
        {
            if (LoadTestDTFYCaliByName(msg))
            {
                //当前试验已更换
                Messenger.Default.Send<string>(PublicData.SM_DTFY_CaliDQ.TestNO.Clone().ToString(), "DTFYCaliDQChanged");
            }
        }


        /// <summary>
        ///根据指令保存动态风压校准试验消息回调
        /// </summary>
        /// <param name="msg"></param>
        private void SaveTestMessage(string msg)
        {
            SaveTest_DTFYCali();
            SaveTest_DTFYCaliPoint();
            SaveTest_DTFYCaliLogList();
        }



        /// <summary>
        ///保存试验记录消息回调
        /// </summary>
        /// <param name="msg">试验编号</param>
        private void InsertLogMessage(SM_DTFY_Cali_Log msg)
        {
            try
            {
                SaveExp_InsertLogs(msg);
            }
            catch (Exception e)
            {
                MessageBox.Show("插入动态风压校准试验Log时错误." + e.ToString());
                //LogHelper.WriteErrLog("ExpDAL插入试验Log时错误", e);//输出日志
            }
        }


        /// <summary>
        /// 需要更新动态风压校准数据
        /// </summary>
        private void NeedUpdate_DTFYCaliMessage(string msg)
        {
            try
            {
                switch (msg)
                {
                    case "DTFYCali_LogListForView":
                        PublicData.LogList_DTFYCali = new System.Collections.ObjectModel.ObservableCollection<SM_DTFY_Cali_Log>(LoadLogList(PublicData.SelectedPoint_DTFYCali.TestNO, PublicData.SelectedPoint_DTFYCali.PointNo));
                        break;

                    case "DTFYCali_TestListForView":
                        PublicData.TestList_DTFYCali = new System.Collections.ObjectModel.ObservableCollection<SM_DTFY_Cali>(LoadTestList());
                        PublicData.SelectedTest_DTFYCali = PublicData.TestList_DTFYCali[0];
                        PublicData.SelectedTestIndex_DTFYCali = 0;
                        PublicData.SelectedPoint_DTFYCali = PublicData.TestList_DTFYCali[0].PointList[0];
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("插入动态风压校准试验Log时错误." + e.ToString());
                //LogHelper.WriteErrLog("ExpDAL插入试验Log时错误", e);//输出日志
            }
        }
        #endregion
    }
}