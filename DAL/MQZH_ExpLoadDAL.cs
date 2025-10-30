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
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MQDFJ_MB.Model;

namespace MQDFJ_MB.DAL
{
    /// <summary>
    /// 试验参数及数据读写操作类
    /// </summary>
    public partial class MQZH_ExpDAL : ObservableObject
    {
        #region 载入试验

        /// <summary>
        /// 按名称载入试验
        /// </summary>
        private bool LoadExpByName(string givenExpName)
        {
            string tempNOStr = givenExpName.Clone().ToString();

            //工厂配置试验，无法载入
            if (tempNOStr == "FactoryExp")
            {
                MessageBox.Show("工厂配置，不可载入！", "错误提示");
                return false;
            }
            //查找试验编号在主表中是否存在，若不存在，则取消载入
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数Row checkExistA00Row = A00Table.FindBy试验编号(tempNOStr);
                if (checkExistA00Row == null)
                {
                    MessageBox.Show("未找到编号为" + tempNOStr + "的试验，请重新选择！", "错误提示");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //载入试验配置和进度
            LoadExpSettings(tempNOStr);
            LoadStatusQM(tempNOStr);
            LoadStatusSM(tempNOStr);
            LoadStatusKFY(tempNOStr);
            LoadStatusCJBX(tempNOStr);
            //载入试验数据
            LoadDataQM(tempNOStr);
            LoadDataSM(tempNOStr);
            LoadDataKFY(tempNOStr);
            LoadDataCJBX(tempNOStr);
            return true;
        }

        /// <summary>
        /// 按名称载入试验A0设置
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadExpSettings(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //载入A00试验主表数据
            //若编号在主表中不存在，则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数Row checkExistA00Row = A00Table.FindBy试验编号(tempNOStr);
                if (checkExistA00Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数Row defExpA00Row = A00Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数Row newExpA00Row = A00Table.NewA00试验参数Row();
                    newExpA00Row.ItemArray = (object[])defExpA00Row.ItemArray.Clone();
                    newExpA00Row.试验编号 = tempNOStr;
                    A00Table.AddA00试验参数Row(newExpA00Row);
                    A00TableAdapter.Update(A00Table);
                    A00Table.AcceptChanges();
                    RaisePropertyChanged(() => A00Table);

                    MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数2Row defExpA00Row2 = A00Table2.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数2Row newExpA00Row2 = A00Table2.NewA00试验参数2Row();
                    newExpA00Row2.ItemArray = (object[])defExpA00Row2.ItemArray.Clone();
                    newExpA00Row2.试验编号 = tempNOStr;
                    A00Table2.AddA00试验参数2Row(newExpA00Row2);
                    A00Table2Adapter.Update(A00Table2);
                    A00Table2.AcceptChanges();
                    RaisePropertyChanged(() => A00Table2);
                    MessageBox.Show("未找到编号为" + tempNOStr + "A00表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数Row expA00Row = A00Table.FindBy试验编号(tempNOStr);
                if (expA00Row != null)
                {
                    //试验基本信息
                    PublicData.ExpDQ.ExpSettingParam.ExpNO = expA00Row.试验编号;
                    PublicData.ExpDQ.ExpSettingParam.ExpDetail = expA00Row.试验补充说明;
                    PublicData.ExpDQ.ExpSettingParam.RepSXNO = expA00Row.三性报告编号;
                    PublicData.ExpDQ.ExpSettingParam.RepCJBXNO = expA00Row.层间变形报告编号;
                    PublicData.ExpDQ.SpecimenNum = expA00Row.试件数量;
                    PublicData.ExpDQ.ExpSettingParam.ExpDate = expA00Row.检测日期;
                    PublicData.ExpDQ.ExpSettingParam.ExpWTDW = expA00Row.委托单位;
                    PublicData.ExpDQ.ExpSettingParam.ExpWTDate = expA00Row.委托日期;
                    PublicData.ExpDQ.ExpSettingParam.ExpSCDW = expA00Row.生产单位;
                    PublicData.ExpDQ.ExpSettingParam.ExpSGDW = expA00Row.施工单位;
                    PublicData.ExpDQ.ExpSettingParam.ExpGCMC = expA00Row.工程名称;
                    //试件信息
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Name = expA00Row.试件名称;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_YPNO = expA00Row.样品编号;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Type = expA00Row.试件类型;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Series = expA00Row.试件系列;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Model = expA00Row.试件型号;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_XCPZ = expA00Row.型材品种;
                    //PublicData.ExpDQ.ExpSettingParam.Exp_SJ_XCCZ = expA00Row.型材材质;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_XCPH = expA00Row.型材牌号;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_XCGG = expA00Row.型材规格;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_FJPZ = expA00Row.附件品种;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_FJCZ = expA00Row.附件材质;
                    //PublicData.ExpDQ.ExpSettingParam.Exp_SJ_FJPH = expA00Row.附件牌号;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MBPZ = expA00Row.面板材料品种;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MBCZ = expA00Row.面板材料材质;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MBPH = expA00Row.面板牌号;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MBHD = expA00Row.面板厚度;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MBZDCC = expA00Row.面板最大尺寸;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MBAZFF = expA00Row.面板安装方法;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MFCLPZ = expA00Row.密封材料品种;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MFCLCZ = expA00Row.密封材料材质;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_MFCLPH = expA00Row.密封材料牌号;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_XQCLPZ = expA00Row.镶嵌材料品种;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_XQCLCZ = expA00Row.镶嵌材料材质;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_XQPH = expA00Row.镶嵌材料牌号;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_XQCC = expA00Row.镶嵌尺寸;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_XQFF = expA00Row.镶嵌方法;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_ZDFGCC = expA00Row.最大分格尺寸;
                    //试件参数
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Width = expA00Row.试件宽度;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Heigth = expA00Row.试件高度;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_Aria = expA00Row.试件面积;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_KQFLenth = expA00Row.开启缝长度;
                    PublicData.ExpDQ.ExpSettingParam.Exp_SJ_KKQAria = expA00Row.开启部分面积;
                    PublicData.ExpDQ.ExpSettingParam.SJ_CG = expA00Row.层高;
                    PublicData.ExpDQ.ExpSettingParam.Isexp_SJ_WithKKQ = expA00Row.有可开启部分;

                    //实验室环境参数
                    PublicData.ExpDQ.ExpRoomPress = expA00Row.大气压力;
                    PublicData.ExpDQ.ExpRoomT = expA00Row.室温;
                    //试验完成状态
                    PublicData.ExpDQ.ExpCompleteStatus = expA00Row.试验总体完成状态;
                    //气密项目参数
                    PublicData.ExpDQ.Exp_QM.NeedTest = expA00Row.气密选中;
                    PublicData.ExpDQ.Exp_QM.IsGC = expA00Row.气密定级或工程;
                    PublicData.ExpDQ.Exp_QM.CompleteStatus = expA00Row.气密检测项目完成状态;
                    //水密项目参数
                    PublicData.ExpDQ.Exp_SM.NeedTest = expA00Row.水密选中;
                    PublicData.ExpDQ.Exp_SM.IsGC = expA00Row.水密定级或工程;
                    PublicData.ExpDQ.Exp_SM.CompleteStatus = expA00Row.水密检测项目完成状态;

                    //抗风压项目参数
                    PublicData.ExpDQ.Exp_KFY.NeedTest = expA00Row.抗风压选中;
                    PublicData.ExpDQ.Exp_KFY.IsGC = expA00Row.抗风压定级或工程;
                    PublicData.ExpDQ.Exp_KFY.CompleteStatus = expA00Row.抗风压检测项目完成状态;

                    //层间变形项目参数
                    PublicData.ExpDQ.Exp_CJBX.NeedTest = expA00Row.层间变形选中;
                    PublicData.ExpDQ.Exp_CJBX.IsGC = expA00Row.层间变形定级或工程;
                    PublicData.ExpDQ.Exp_CJBX.CompleteStatus = expA00Row.层间变形检测项目完成状态;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数2Row expA00Row2 = A00Table2.FindBy试验编号(tempNOStr);
                if (expA00Row2 != null)
                {                  
                    //气密项目参数
                    PublicData.ExpDQ.Exp_QM.QM_GCSJP = expA00Row2.工程气密检测压力设计值;
                    PublicData.ExpDQ.Exp_QM.QM_GCYJYP = expA00Row2.工程气密预加压压力;
                    PublicData.ExpDQ.Exp_QM.QM_SJSTL_DWKQFC = expA00Row2.工程气密开启缝长渗透量设计值;
                    PublicData.ExpDQ.Exp_QM.QM_SJSTL_DWMJ = expA00Row2.工程气密单位面积渗透量设计值;
                    //水密项目参数
                    PublicData.ExpDQ.Exp_SM.WaveType_SM = expA00Row2.水密加压类型;
                    PublicData.ExpDQ.Exp_SM.SM_GCSJ_KKQ = expA00Row2.水密工程可开启设定值;
                    PublicData.ExpDQ.Exp_SM.SM_GCSJ_GD = expA00Row2.水密工程固定设计值;
                    PublicData.ExpDQ.Exp_SM.Width_XK = expA00Row2.箱口宽度;
                    PublicData.ExpDQ.Exp_SM.Height_XK = expA00Row2.箱口高度;
                    PublicData.ExpDQ.Exp_SM.SM_SLL_Per_m2 = expA00Row2.单位面积淋水量;

                    //抗风压项目参数
                    PublicData.ExpDQ.Exp_KFY.P3_GCSJ = expA00Row2.抗风压工程风载荷标准值;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].Is_Use = expA00Row2.抗风压测点组1使用;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[1].Is_Use = expA00Row2.抗风压测点组2使用;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[2].Is_Use = expA00Row2.抗风压测点组3使用;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].Is_TestMBBX = expA00Row2.抗风压测点组1测面板;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[1].Is_TestMBBX = expA00Row2.抗风压测点组2测面板;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[2].Is_TestMBBX = expA00Row2.抗风压测点组3测面板;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].IsBZCSJMB = expA00Row2.抗风压测点组1测三角面板;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].L = expA00Row2.抗风压测点组1间距;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[1].L = expA00Row2.抗风压测点组2间距;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[2].L = expA00Row2.抗风压测点组3间距;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].ND_XD_YXFM = expA00Row2.抗风压构件1允许相对面法线挠度;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[1].ND_XD_YXFM = expA00Row2.抗风压构件2允许相对面法线挠度;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[2].ND_XD_YXFM = expA00Row2.抗风压构件3允许相对面法线挠度;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].WYC_No[0] = expA00Row2.抗风压测点组1a位移尺编号;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].WYC_No[1] = expA00Row2.抗风压测点组1b位移尺编号;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].WYC_No[2] = expA00Row2.抗风压测点组1c位移尺编号;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].WYC_No[3] = expA00Row2.抗风压测点组1d位移尺编号;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[1].WYC_No[0] = expA00Row2.抗风压测点组2a位移尺编号;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[1].WYC_No[1] = expA00Row2.抗风压测点组2b位移尺编号;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[1].WYC_No[2] = expA00Row2.抗风压测点组2c位移尺编号;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[2].WYC_No[0] = expA00Row2.抗风压测点组3a位移尺编号;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[2].WYC_No[1] = expA00Row2.抗风压测点组3b位移尺编号;
                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[2].WYC_No[2] = expA00Row2.抗风压测点组3c位移尺编号;
                    PublicData.ExpDQ.Exp_KFY.P3DownPress = expA00Row2.抗风压P3降低压力;
                    PublicData.ExpDQ.Exp_KFY.P3Press = expA00Row2.抗风压P3压力;

                    //层间变形项目参数
                    PublicData.ExpDQ.Exp_CJBX.CJBX_SJZBX = expA00Row2.工程检测X轴位移角设计值;
                    PublicData.ExpDQ.Exp_CJBX.CJBX_SJZBY = expA00Row2.工程检测Y轴位移角设计值;
                    PublicData.ExpDQ.Exp_CJBX.CJBX_SJZBZ = expA00Row2.工程检测Z轴位移量设计值;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入试验A1气密进度
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadStatusQM(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //载入A01气密试验进度
            //若编号在表中不存在，则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.A01气密试验进度Row checkExistA01Row = A01Table.FindBy试验编号(tempNOStr);
                if (checkExistA01Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.A01气密试验进度Row defExpA01Row = A01Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.A01气密试验进度Row newExpA01Row = A01Table.NewA01气密试验进度Row();
                    newExpA01Row.ItemArray = (object[])defExpA01Row.ItemArray.Clone();
                    newExpA01Row.试验编号 = tempNOStr;
                    A01Table.AddA01气密试验进度Row(newExpA01Row);
                    A01TableAdapter.Update(A01Table);
                    A01Table.AcceptChanges();
                    RaisePropertyChanged(() => A01Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "A01表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.A01气密试验进度Row expA01Row = A01Table.FindBy试验编号(tempNOStr);
                if (expA01Row != null)
                {
                    //定级进度
                    PublicData.ExpDQ.Exp_QM.StageList_QMDJ[0].CompleteStatus = expA01Row.定级气密附加正压预加压完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMDJ[1].CompleteStatus = expA01Row.定级气密附加正压检测完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMDJ[2].CompleteStatus = expA01Row.定级气密附加负压预加压完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMDJ[3].CompleteStatus = expA01Row.定级气密附加负压检测完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMDJ[4].CompleteStatus = expA01Row.定级气密附加固定正压预加压完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMDJ[5].CompleteStatus = expA01Row.定级气密附加固定正压检测完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMDJ[6].CompleteStatus = expA01Row.定级气密附加固定负压预加压完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMDJ[7].CompleteStatus = expA01Row.定级气密附加固定负压检测完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMDJ[8].CompleteStatus = expA01Row.定级气密总渗透正压预加压完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMDJ[9].CompleteStatus = expA01Row.定级气密总渗透正压检测完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMDJ[10].CompleteStatus = expA01Row.定级气密总渗透负压预加压完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMDJ[11].CompleteStatus = expA01Row.定级气密总渗透负压检测完成状态;
                    //工程进度
                    PublicData.ExpDQ.Exp_QM.StageList_QMGC[0].CompleteStatus = expA01Row.工程气密附加正压预加压完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMGC[1].CompleteStatus = expA01Row.工程气密附加正压检测完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMGC[2].CompleteStatus = expA01Row.工程气密附加负压预加压完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMGC[3].CompleteStatus = expA01Row.工程气密附加负压检测完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMGC[4].CompleteStatus = expA01Row.工程气密附加固定正压预加压完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMGC[5].CompleteStatus = expA01Row.工程气密定级附加固定正压检测完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMGC[6].CompleteStatus = expA01Row.工程气密附加固定负压预加压完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMGC[7].CompleteStatus = expA01Row.工程气密附加固定负压检测完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMGC[8].CompleteStatus = expA01Row.工程气密总渗透正压预加压完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMGC[9].CompleteStatus = expA01Row.工程气密总渗透正压检测完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMGC[10].CompleteStatus = expA01Row.工程气密总渗透负压预加压完成状态;
                    PublicData.ExpDQ.Exp_QM.StageList_QMGC[11].CompleteStatus = expA01Row.工程气密总渗透负压检测完成状态;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入试验A2水密进度
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadStatusSM(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //载入A02水密进度表数据
            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.A02水密试验进度Row checkExistA02Row = A02Table.FindBy试验编号(tempNOStr);
                if (checkExistA02Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.A02水密试验进度Row defExpA02Row = A02Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.A02水密试验进度Row newExpA02Row = A02Table.NewA02水密试验进度Row();
                    newExpA02Row.ItemArray = (object[])defExpA02Row.ItemArray.Clone();
                    newExpA02Row.试验编号 = tempNOStr;
                    A02Table.AddA02水密试验进度Row(newExpA02Row);
                    A02TableAdapter.Update(A02Table);
                    A02Table.AcceptChanges();
                    RaisePropertyChanged(() => A02Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "A02表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.A02水密试验进度Row expA02Row = A02Table.FindBy试验编号(tempNOStr);
                if (expA02Row != null)
                {
                    PublicData.ExpDQ.Exp_SM.StageList_SMDJ[0].CompleteStatus = expA02Row.定级水密预加压完成状态;
                    PublicData.ExpDQ.Exp_SM.StageList_SMDJ[1].CompleteStatus = expA02Row.定级水密加压检测完成状态;
                    PublicData.ExpDQ.Exp_SM.StageList_SMGC[0].CompleteStatus = expA02Row.工程水密预加压完成状态;
                    PublicData.ExpDQ.Exp_SM.StageList_SMGC[1].CompleteStatus = expA02Row.工程水密加压检测完成状态;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入试验A3抗风压进度
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadStatusKFY(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //载入A03抗风压试验进度
            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.A03抗风压试验进度Row checkExistA03Row = A03Table.FindBy试验编号(tempNOStr);
                if (checkExistA03Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.A03抗风压试验进度Row defExpA03Row = A03Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.A03抗风压试验进度Row newExpA03Row = A03Table.NewA03抗风压试验进度Row();
                    newExpA03Row.ItemArray = (object[])defExpA03Row.ItemArray.Clone();
                    newExpA03Row.试验编号 = tempNOStr;
                    A03Table.AddA03抗风压试验进度Row(newExpA03Row);
                    A03TableAdapter.Update(A03Table);
                    A03Table.AcceptChanges();
                    RaisePropertyChanged(() => A03Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "A03表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.A03抗风压试验进度Row expA03Row = A03Table.FindBy试验编号(tempNOStr);
                if (expA03Row != null)
                {
                    PublicData.ExpDQ.Exp_KFY.StageList_KFYDJ[0].CompleteStatus = expA03Row.定级p1正压预加压完成状态;
                    PublicData.ExpDQ.Exp_KFY.StageList_KFYDJ[1].CompleteStatus = expA03Row.定级p1正压检测加压完成状态;
                    PublicData.ExpDQ.Exp_KFY.StageList_KFYDJ[2].CompleteStatus = expA03Row.定级p1负压预加压完成状态;
                    PublicData.ExpDQ.Exp_KFY.StageList_KFYDJ[3].CompleteStatus = expA03Row.定级p1负压检测加压完成状态;
                    PublicData.ExpDQ.Exp_KFY.StageList_KFYDJ[4].CompleteStatus = expA03Row.定级p2正压检测加压完成状态;
                    PublicData.ExpDQ.Exp_KFY.StageList_KFYDJ[5].CompleteStatus = expA03Row.定级p2负压检测加压完成状态;
                    PublicData.ExpDQ.Exp_KFY.StageList_KFYDJ[6].CompleteStatus = expA03Row.定级p3正压检测加压完成状态;
                    PublicData.ExpDQ.Exp_KFY.StageList_KFYDJ[7].CompleteStatus = expA03Row.定级p3负压检测加压完成状态;
                    PublicData.ExpDQ.Exp_KFY.StageList_KFYDJ[8].CompleteStatus = expA03Row.定级pmax正压检测加压完成状态;
                    PublicData.ExpDQ.Exp_KFY.StageList_KFYDJ[9].CompleteStatus = expA03Row.定级pmax负压检测加压完成状态;
                    PublicData.ExpDQ.Exp_KFY.StageList_KFYGC[0].CompleteStatus = expA03Row.工程p1正压预加压完成状态;
                    PublicData.ExpDQ.Exp_KFY.StageList_KFYGC[1].CompleteStatus = expA03Row.工程p1正压检测加压完成状态;
                    PublicData.ExpDQ.Exp_KFY.StageList_KFYGC[2].CompleteStatus = expA03Row.工程p1负压预加压完成状态;
                    PublicData.ExpDQ.Exp_KFY.StageList_KFYGC[3].CompleteStatus = expA03Row.工程p1负压检测加压完成状态;
                    PublicData.ExpDQ.Exp_KFY.StageList_KFYGC[4].CompleteStatus = expA03Row.工程p2正压检测加压完成状态;
                    PublicData.ExpDQ.Exp_KFY.StageList_KFYGC[5].CompleteStatus = expA03Row.工程p2负压检测加压完成状态;
                    PublicData.ExpDQ.Exp_KFY.StageList_KFYGC[6].CompleteStatus = expA03Row.工程p3正压检测加压完成状态;
                    PublicData.ExpDQ.Exp_KFY.StageList_KFYGC[7].CompleteStatus = expA03Row.工程p3负压检测加压完成状态;
                    PublicData.ExpDQ.Exp_KFY.StageList_KFYGC[8].CompleteStatus = expA03Row.工程pmax正压检测加压完成状态;
                    PublicData.ExpDQ.Exp_KFY.StageList_KFYGC[9].CompleteStatus = expA03Row.工程pmax负压检测加压完成状态;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入试验A4层间变形进度
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadStatusCJBX(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //载入A04层间变形进度表数据
            //若编号在表中不存在，则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.A04层间变形试验进度Row checkExistA04Row = A04Table.FindBy试验编号(tempNOStr);
                if (checkExistA04Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.A04层间变形试验进度Row defExpA04Row = A04Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.A04层间变形试验进度Row newExpA04Row = A04Table.NewA04层间变形试验进度Row();
                    newExpA04Row.ItemArray = (object[])defExpA04Row.ItemArray.Clone();
                    newExpA04Row.试验编号 = tempNOStr;
                    A04Table.AddA04层间变形试验进度Row(newExpA04Row);
                    A04TableAdapter.Update(A04Table);
                    A04Table.AcceptChanges();
                    RaisePropertyChanged(() => A04Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "的A04试验进度，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.A04层间变形试验进度Row expA04Row = A04Table.FindBy试验编号(tempNOStr);
                if (expA04Row != null)
                {
                    //定级检测进度X轴
                    PublicData.ExpDQ.Exp_CJBX.StageList_DJX[0].CompleteStatus = expA04Row.定级X轴预加载完成状态;
                    PublicData.ExpDQ.Exp_CJBX.StageList_DJX[1].CompleteStatus = expA04Row.定级X轴第1级完成状态;
                    PublicData.ExpDQ.Exp_CJBX.StageList_DJX[2].CompleteStatus = expA04Row.定级X轴第2级完成状态;
                    PublicData.ExpDQ.Exp_CJBX.StageList_DJX[3].CompleteStatus = expA04Row.定级X轴第3级完成状态;
                    PublicData.ExpDQ.Exp_CJBX.StageList_DJX[4].CompleteStatus = expA04Row.定级X轴第4级完成状态;
                    PublicData.ExpDQ.Exp_CJBX.StageList_DJX[5].CompleteStatus = expA04Row.定级X轴第5级完成状态;
                    //定级检测进度Y轴
                    PublicData.ExpDQ.Exp_CJBX.StageList_DJY[0].CompleteStatus = expA04Row.定级Y轴预加载完成状态;
                    PublicData.ExpDQ.Exp_CJBX.StageList_DJY[1].CompleteStatus = expA04Row.定级Y轴第1级完成状态;
                    PublicData.ExpDQ.Exp_CJBX.StageList_DJY[2].CompleteStatus = expA04Row.定级Y轴第2级完成状态;
                    PublicData.ExpDQ.Exp_CJBX.StageList_DJY[3].CompleteStatus = expA04Row.定级Y轴第3级完成状态;
                    PublicData.ExpDQ.Exp_CJBX.StageList_DJY[4].CompleteStatus = expA04Row.定级Y轴第4级完成状态;
                    PublicData.ExpDQ.Exp_CJBX.StageList_DJY[5].CompleteStatus = expA04Row.定级Y轴第5级完成状态;
                    //定级检测进度Z轴
                    PublicData.ExpDQ.Exp_CJBX.StageList_DJZ[0].CompleteStatus = expA04Row.定级Z轴预加载完成状态;
                    PublicData.ExpDQ.Exp_CJBX.StageList_DJZ[1].CompleteStatus = expA04Row.定级Z轴第1级完成状态;
                    PublicData.ExpDQ.Exp_CJBX.StageList_DJZ[2].CompleteStatus = expA04Row.定级Z轴第2级完成状态;
                    PublicData.ExpDQ.Exp_CJBX.StageList_DJZ[3].CompleteStatus = expA04Row.定级Z轴第3级完成状态;
                    PublicData.ExpDQ.Exp_CJBX.StageList_DJZ[4].CompleteStatus = expA04Row.定级Z轴第4级完成状态;
                    PublicData.ExpDQ.Exp_CJBX.StageList_DJZ[5].CompleteStatus = expA04Row.定级Z轴第5级完成状态;
                    //工程检测进度X轴
                    PublicData.ExpDQ.Exp_CJBX.StageList_GCX[0].CompleteStatus = expA04Row.工程X轴预加载完成状态;
                    PublicData.ExpDQ.Exp_CJBX.StageList_GCX[1].CompleteStatus = expA04Row.工程X轴加载检测完成状态;
                    //工程检测进度Y轴
                    PublicData.ExpDQ.Exp_CJBX.StageList_GCY[0].CompleteStatus = expA04Row.工程Y轴预加载完成状态;
                    PublicData.ExpDQ.Exp_CJBX.StageList_GCY[1].CompleteStatus = expA04Row.工程Y轴加载检测完成状态;
                    //工程检测进度Z轴
                    PublicData.ExpDQ.Exp_CJBX.StageList_GCZ[0].CompleteStatus = expA04Row.工程Z轴预加载完成状态;
                    PublicData.ExpDQ.Exp_CJBX.StageList_GCZ[1].CompleteStatus = expA04Row.工程Z轴加载检测完成状态;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入试验B1气密试验数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadDataQM(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //载入B1气密试验数据
            //若编号在表中不存在，则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B1气密试验数据Row checkExistB1Row = B1Table.FindBy试验编号(tempNOStr);
                if (checkExistB1Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B1气密试验数据Row defExpB1Row = B1Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B1气密试验数据Row newExpB1Row = B1Table.NewB1气密试验数据Row();
                    newExpB1Row.ItemArray = (object[])defExpB1Row.ItemArray.Clone();
                    newExpB1Row.试验编号 = tempNOStr;
                    B1Table.AddB1气密试验数据Row(newExpB1Row);
                    B1TableAdapter.Update(B1Table);
                    B1Table.AcceptChanges();
                    RaisePropertyChanged(() => B1Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B1表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B1气密试验数据Row expB1Row = B1Table.FindBy试验编号(tempNOStr);
                if (expB1Row != null)
                {
                    PublicData.ExpDQ.ExpData_QM.Stl_QMGC[1][0] = expB1Row.Qf_GC_Z;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMGC[5][0] = expB1Row.Qfg_GC_Z;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMGC[9][0] = expB1Row.Qz_GC_Z;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMGC[3][0] = expB1Row.Qf_GC_F;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMGC[7][0] = expB1Row.Qfg_GC_F;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMGC[11][0] = expB1Row.Qz_GC_F;
                    PublicData.ExpDQ.ExpData_QM.Qf1_GC_Z = expB1Row.Qf1_GC_Z;
                    PublicData.ExpDQ.ExpData_QM.Qfg1_GC_Z = expB1Row.Qfg1_GC_Z;
                    PublicData.ExpDQ.ExpData_QM.Qz1_GC_Z = expB1Row.Qz1_GC_Z;
                    PublicData.ExpDQ.ExpData_QM.Qs_GC_Z = expB1Row.Qs_GC_Z;
                    PublicData.ExpDQ.ExpData_QM.Qk_GC_Z = expB1Row.Qk_GC_Z;
                    PublicData.ExpDQ.ExpData_QM.QA1_GC_Z = expB1Row.QA1_GC_Z;
                    PublicData.ExpDQ.ExpData_QM.Ql1_GC_Z = expB1Row.Ql1_GC_Z;
                    PublicData.ExpDQ.ExpData_QM.Qf1_GC_F = expB1Row.Qf1_GC_F;
                    PublicData.ExpDQ.ExpData_QM.Qfg1_GC_F = expB1Row.Qfg1_GC_F;
                    PublicData.ExpDQ.ExpData_QM.Qz1_GC_F = expB1Row.Qz1_GC_F;
                    PublicData.ExpDQ.ExpData_QM.Qs_GC_F = expB1Row.Qs_GC_F;
                    PublicData.ExpDQ.ExpData_QM.Qk_GC_F = expB1Row.Qk_GC_F;
                    PublicData.ExpDQ.ExpData_QM.QA1_GC_F = expB1Row.QA1_GC_F;
                    PublicData.ExpDQ.ExpData_QM.Ql1_GC_F = expB1Row.Ql1_GC_F;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[1][0] = expB1Row.Qf_DJ_ZS50;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[1][1] = expB1Row.Qf_DJ_ZS100;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[1][2] = expB1Row.Qf_DJ_Z150;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[1][3] = expB1Row.Qf_DJ_ZJ100;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[1][4] = expB1Row.Qf_DJ_ZJ50;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[5][0] = expB1Row.Qfg_DJ_ZS50;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[5][1] = expB1Row.Qfg_DJ_ZS100;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[5][2] = expB1Row.Qfg_DJ_Z150;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[5][3] = expB1Row.Qfg_DJ_ZJ100;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[5][4] = expB1Row.Qfg_DJ_ZJ50;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[9][0] = expB1Row.Qz_DJ_ZS50;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[9][1] = expB1Row.Qz_DJ_ZS100;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[9][2] = expB1Row.Qz_DJ_Z150;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[9][3] = expB1Row.Qz_DJ_ZJ100;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[9][4] = expB1Row.Qz_DJ_ZJ50;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[3][0] = expB1Row.Qf_DJ_FS50;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[3][1] = expB1Row.Qf_DJ_FS100;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[3][2] = expB1Row.Qf_DJ_F150;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[3][3] = expB1Row.Qf_DJ_FJ100;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[3][4] = expB1Row.Qf_DJ_FJ50;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[7][0] = expB1Row.Qfg_DJ_FS50;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[7][1] = expB1Row.Qfg_DJ_FS100;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[7][2] = expB1Row.Qfg_DJ_F150;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[7][3] = expB1Row.Qfg_DJ_FJ100;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[7][4] = expB1Row.Qfg_DJ_FJ50;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[11][0] = expB1Row.Qz_DJ_FS50;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[11][1] = expB1Row.Qz_DJ_FS100;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[11][2] = expB1Row.Qz_DJ_F150;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[11][3] = expB1Row.Qz_DJ_FJ100;
                    PublicData.ExpDQ.ExpData_QM.Stl_QMDJ[11][4] = expB1Row.Qz_DJ_FJ50;
                    PublicData.ExpDQ.ExpData_QM.Qfp_DJ_Z = expB1Row.Qfp_DJ_Z;
                    PublicData.ExpDQ.ExpData_QM.Qfgp_DJ_Z = expB1Row.Qfgp_DJ_Z;
                    PublicData.ExpDQ.ExpData_QM.Qzp_DJ_Z = expB1Row.Qzp_DJ_Z;
                    PublicData.ExpDQ.ExpData_QM.Qf1_DJ_Z = expB1Row.Qf1_DJ_Z;
                    PublicData.ExpDQ.ExpData_QM.Qfg1_DJ_Z = expB1Row.Qfg1_DJ_Z;
                    PublicData.ExpDQ.ExpData_QM.Qz1_DJ_Z = expB1Row.Qz1_DJ_Z;
                    PublicData.ExpDQ.ExpData_QM.Qs_DJ_Z = expB1Row.Qs_DJ_Z;
                    PublicData.ExpDQ.ExpData_QM.Qk_DJ_Z = expB1Row.Qk_DJ_Z;
                    PublicData.ExpDQ.ExpData_QM.QA1_DJ_Z = expB1Row.QA1_DJ_Z;
                    PublicData.ExpDQ.ExpData_QM.Ql1_DJ_Z = expB1Row.Ql1_DJ_Z;
                    PublicData.ExpDQ.ExpData_QM.QA_DJ_Z = expB1Row.QA_DJ_Z;
                    PublicData.ExpDQ.ExpData_QM.Ql_DJ_Z = expB1Row.Ql_DJ_Z;
                    PublicData.ExpDQ.ExpData_QM.Qfp_DJ_F = expB1Row.Qfp_DJ_F;
                    PublicData.ExpDQ.ExpData_QM.Qfgp_DJ_F = expB1Row.Qfgp_DJ_F;
                    PublicData.ExpDQ.ExpData_QM.Qzp_DJ_F = expB1Row.Qzp_DJ_F;
                    PublicData.ExpDQ.ExpData_QM.Qf1_DJ_F = expB1Row.Qf1_DJ_F;
                    PublicData.ExpDQ.ExpData_QM.Qfg1_DJ_F = expB1Row.Qfg1_DJ_F;
                    PublicData.ExpDQ.ExpData_QM.Qz1_DJ_F = expB1Row.Qz1_DJ_F;
                    PublicData.ExpDQ.ExpData_QM.Qs_DJ_F = expB1Row.Qs_DJ_F;
                    PublicData.ExpDQ.ExpData_QM.Qk_DJ_F = expB1Row.Qk_DJ_F;
                    PublicData.ExpDQ.ExpData_QM.QA1_DJ_F = expB1Row.QA1_DJ_F;
                    PublicData.ExpDQ.ExpData_QM.Ql1_DJ_F = expB1Row.Ql1_DJ_F;
                    PublicData.ExpDQ.ExpData_QM.QA_DJ_F = expB1Row.QA_DJ_F;
                    PublicData.ExpDQ.ExpData_QM.Ql_DJ_F = expB1Row.Ql_DJ_F;
                    PublicData.ExpDQ.ExpData_QM.QA_DJ_QM = expB1Row.QA_DJ_QM;
                    PublicData.ExpDQ.ExpData_QM.Ql_DJ_QM = expB1Row.Ql_DJ_QM;
                    PublicData.ExpDQ.ExpData_QM.IsMeetDesign_GC_ZQA = expB1Row.IsMeetDesign_GC_ZQA;
                    PublicData.ExpDQ.ExpData_QM.IsMeetDesign_GC_FQA = expB1Row.IsMeetDesign_GC_FQA;
                    PublicData.ExpDQ.ExpData_QM.IsMeetDesign_GC_ZQl = expB1Row.IsMeetDesign_GC_ZQl;
                    PublicData.ExpDQ.ExpData_QM.IsMeetDesign_GC_FQl = expB1Row.IsMeetDesign_GC_FQl;
                    PublicData.ExpDQ.ExpData_QM.IsMeetDesign_GC_QM = expB1Row.IsMeetDesign_GC_QM;
                    PublicData.ExpDQ.ExpData_QM.QALevel_DJ_QM = expB1Row.QALevel_DJ_QM;
                    PublicData.ExpDQ.ExpData_QM.QlLevel_DJ_QM = expB1Row.QlLevel_DJ_QM;
                    PublicData.ExpDQ.ExpData_QM.QALevel_DJZ_QM = expB1Row.QA正Level_DJ_QM;
                    PublicData.ExpDQ.ExpData_QM.QlLevel_DJZ_QM = expB1Row.Ql正Level_DJ_QM;
                    PublicData.ExpDQ.ExpData_QM.QALevel_DJF_QM = expB1Row.QA负Level_DJ_QM;
                    PublicData.ExpDQ.ExpData_QM.QlLevel_DJF_QM = expB1Row.Ql负Level_DJ_QM;


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入试验B2水密试验数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadDataSM(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //载入B2水密数据
            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B2水密试验数据Row checkExistB2Row = B2Table.FindBy试验编号(tempNOStr);
                if (checkExistB2Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B2水密试验数据Row defExpB2Row = B2Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B2水密试验数据Row newExpB2Row = B2Table.NewB2水密试验数据Row();
                    newExpB2Row.ItemArray = (object[])defExpB2Row.ItemArray.Clone();
                    newExpB2Row.试验编号 = tempNOStr;
                    B2Table.AddB2水密试验数据Row(newExpB2Row);
                    B2TableAdapter.Update(B2Table);
                    B2Table.AcceptChanges();
                    RaisePropertyChanged(() => B2Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B2表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B2水密试验数据Row expB2Row = B2Table.FindBy试验编号(tempNOStr);
                if (expB2Row != null)
                {
                    PublicData.ExpDQ.ExpData_SM.Press_DJ[0] = expB2Row.定级第1级检测压力;
                    PublicData.ExpDQ.ExpData_SM.Press_DJ[1] = expB2Row.定级第2级检测压力;
                    PublicData.ExpDQ.ExpData_SM.Press_DJ[2] = expB2Row.定级第3级检测压力;
                    PublicData.ExpDQ.ExpData_SM.Press_DJ[3] = expB2Row.定级第4级检测压力;
                    PublicData.ExpDQ.ExpData_SM.Press_DJ[4] = expB2Row.定级第5级检测压力;
                    PublicData.ExpDQ.ExpData_SM.Press_DJ[5] = expB2Row.定级第6级检测压力;
                    PublicData.ExpDQ.ExpData_SM.Press_DJ[6] = expB2Row.定级第7级检测压力;
                    PublicData.ExpDQ.ExpData_SM.Press_DJ[7] = expB2Row.定级第8级检测压力;
                    PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[0] = expB2Row.定级第1级可开启部分渗漏情况;
                    PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[1] = expB2Row.定级第2级可开启部分渗漏情况;
                    PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[2] = expB2Row.定级第3级可开启部分渗漏情况;
                    PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[3] = expB2Row.定级第4级可开启部分渗漏情况;
                    PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[4] = expB2Row.定级第5级可开启部分渗漏情况;
                    PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[5] = expB2Row.定级第6级可开启部分渗漏情况;
                    PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[6] = expB2Row.定级第7级可开启部分渗漏情况;
                    PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[7] = expB2Row.定级第8级可开启部分渗漏情况;
                    PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[0] = expB2Row.定级第1级固定部分渗漏情况;
                    PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[1] = expB2Row.定级第2级固定部分渗漏情况;
                    PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[2] = expB2Row.定级第3级固定部分渗漏情况;
                    PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[3] = expB2Row.定级第4级固定部分渗漏情况;
                    PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[4] = expB2Row.定级第5级固定部分渗漏情况;
                    PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[5] = expB2Row.定级第6级固定部分渗漏情况;
                    PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[6] = expB2Row.定级第7级固定部分渗漏情况;
                    PublicData.ExpDQ.ExpData_SM.SLStatus_DJ_GD[7] = expB2Row.定级第8级固定部分渗漏情况;
                    PublicData.ExpDQ.ExpData_SM.SLPS_DJ_KKQ[0] = expB2Row.定级第1级可开启部分渗漏说明;
                    PublicData.ExpDQ.ExpData_SM.SLPS_DJ_KKQ[1] = expB2Row.定级第2级可开启部分渗漏说明;
                    PublicData.ExpDQ.ExpData_SM.SLPS_DJ_KKQ[2] = expB2Row.定级第3级可开启部分渗漏说明;
                    PublicData.ExpDQ.ExpData_SM.SLPS_DJ_KKQ[3] = expB2Row.定级第4级可开启部分渗漏说明;
                    PublicData.ExpDQ.ExpData_SM.SLPS_DJ_KKQ[4] = expB2Row.定级第5级可开启部分渗漏说明;
                    PublicData.ExpDQ.ExpData_SM.SLPS_DJ_KKQ[5] = expB2Row.定级第6级可开启部分渗漏说明;
                    PublicData.ExpDQ.ExpData_SM.SLPS_DJ_KKQ[6] = expB2Row.定级第7级可开启部分渗漏说明;
                    PublicData.ExpDQ.ExpData_SM.SLPS_DJ_KKQ[7] = expB2Row.定级第8级可开启部分渗漏说明;
                    PublicData.ExpDQ.ExpData_SM.SLPS_DJ_GD[0] = expB2Row.定级第1级固定部分渗漏说明;
                    PublicData.ExpDQ.ExpData_SM.SLPS_DJ_GD[1] = expB2Row.定级第2级固定部分渗漏说明;
                    PublicData.ExpDQ.ExpData_SM.SLPS_DJ_GD[2] = expB2Row.定级第3级固定部分渗漏说明;
                    PublicData.ExpDQ.ExpData_SM.SLPS_DJ_GD[3] = expB2Row.定级第4级固定部分渗漏说明;
                    PublicData.ExpDQ.ExpData_SM.SLPS_DJ_GD[4] = expB2Row.定级第5级固定部分渗漏说明;
                    PublicData.ExpDQ.ExpData_SM.SLPS_DJ_GD[5] = expB2Row.定级第6级固定部分渗漏说明;
                    PublicData.ExpDQ.ExpData_SM.SLPS_DJ_GD[6] = expB2Row.定级第7级固定部分渗漏说明;
                    PublicData.ExpDQ.ExpData_SM.SLPS_DJ_GD[7] = expB2Row.定级第8级固定部分渗漏说明;
                    PublicData.ExpDQ.ExpData_SM.Press_GC[0] = expB2Row.工程可开启部分检测压力;
                    PublicData.ExpDQ.ExpData_SM.Press_GC[1] = expB2Row.工程固定部分检测压力;
                    PublicData.ExpDQ.ExpData_SM.SLStatus_GC_KKQ[0] = expB2Row.工程可开启部分渗漏情况;
                    PublicData.ExpDQ.ExpData_SM.SLStatus_GC_GD[0] = expB2Row.工程固定部分渗漏情况;
                    PublicData.ExpDQ.ExpData_SM.SLPS_GC_KKQ[0] = expB2Row.工程可开启部分渗漏说明;
                    PublicData.ExpDQ.ExpData_SM.SLPS_GC_GD[0] = expB2Row.工程固定部分渗漏说明;
                    PublicData.ExpDQ.ExpData_SM.MaxPressWYZSL_DJ_KKQ = expB2Row.定级可开启最大未严重渗漏压力;
                    PublicData.ExpDQ.ExpData_SM.MaxPressWYZSL_DJ_GD = expB2Row.定级固定最大未严重渗漏压力;
                    PublicData.ExpDQ.ExpData_SM.Level_DJ_KKQ = expB2Row.定级可开启部分评定等级;
                    PublicData.ExpDQ.ExpData_SM.Level_DJ_GD = expB2Row.定级固定部分评定等级;
                    PublicData.ExpDQ.ExpData_SM.SLStatusFinal_DJ_KKQ = expB2Row.定级可开启部分最终渗漏情况;
                    PublicData.ExpDQ.ExpData_SM.SLStatusFinal_DJ_GD = expB2Row.定级固定部分最终渗漏情况;
                    PublicData.ExpDQ.ExpData_SM.SLPSFinal_DJ_KKQ = expB2Row.定级可开启部分最终渗漏说明;
                    PublicData.ExpDQ.ExpData_SM.SLPSFinal_DJ_GD = expB2Row.定级固定部分最终渗漏说明;
                    PublicData.ExpDQ.ExpData_SM.IsMeetDesign_GC_KKQ = expB2Row.工程可开启部分是否满足设计;
                    PublicData.ExpDQ.ExpData_SM.IsMeetDesign_GC_GD = expB2Row.工程固定部分是否满足设计;
                    PublicData.ExpDQ.ExpData_SM.IsMeetDesign_GC_All = expB2Row.工程整体是否满足设计;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }


        /// <summary>
        /// 按名称载入试验B300-B39抗风压试验数据
        /// </summary>
        private void LoadDataKFY(string givenExpName)
        {
            string tempNOStr = givenExpName.Clone().ToString();

            LoadB300(tempNOStr);
            LoadB311a(tempNOStr);
            LoadB311b(tempNOStr);
            LoadB311c(tempNOStr);
            LoadB311d(tempNOStr);
            LoadB312a(tempNOStr);
            LoadB312b(tempNOStr);
            LoadB312c(tempNOStr);
            LoadB313a(tempNOStr);
            LoadB313b(tempNOStr);
            LoadB313c(tempNOStr);
            LoadB321(tempNOStr);
            LoadB322(tempNOStr);
            LoadB323(tempNOStr);
            LoadB324(tempNOStr);
            LoadB331(tempNOStr);
            LoadB332(tempNOStr);
            LoadB333(tempNOStr);
            LoadB334(tempNOStr);
            LoadB340(tempNOStr);
            LoadB350(tempNOStr);
            LoadB361(tempNOStr);
            LoadB362(tempNOStr);
            LoadB363(tempNOStr);
            LoadB371(tempNOStr);
            LoadB372(tempNOStr);
            LoadB381(tempNOStr);
            LoadB382(tempNOStr);
            LoadB390(tempNOStr);
        }

        /// <summary>
        /// 按名称载入试验B300抗风压试验数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB300(string givenExpName)
        {

            string tempNOStr = givenExpName;

            //载入B300抗风压试验数据
            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B300抗风压试验数据Row checkExistB300Row = B300Table.FindBy试验编号(tempNOStr);
                if (checkExistB300Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B300抗风压试验数据Row defExpB300Row = B300Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B300抗风压试验数据Row newExpB300Row = B300Table.NewB300抗风压试验数据Row();
                    newExpB300Row.ItemArray = (object[])defExpB300Row.ItemArray.Clone();
                    newExpB300Row.试验编号 = tempNOStr;
                    B300Table.AddB300抗风压试验数据Row(newExpB300Row);
                    B300TableAdapter.Update(B300Table);
                    B300Table.AcceptChanges();
                    RaisePropertyChanged(() => B300Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B300表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B300抗风压试验数据Row expB300Row = B300Table.FindBy试验编号(tempNOStr);
                if (expB300Row != null)
                {
                    //定级检测压力
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[0] = expB300Row.定级变形检测压力Z01;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[1] = expB300Row.定级变形检测压力Z02;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[2] = expB300Row.定级变形检测压力Z03;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[3] = expB300Row.定级变形检测压力Z04;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[4] = expB300Row.定级变形检测压力Z05;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[5] = expB300Row.定级变形检测压力Z06;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[6] = expB300Row.定级变形检测压力Z07;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[7] = expB300Row.定级变形检测压力Z08;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[8] = expB300Row.定级变形检测压力Z09;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[9] = expB300Row.定级变形检测压力Z10;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[10] = expB300Row.定级变形检测压力Z11;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[11] = expB300Row.定级变形检测压力Z12;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[12] = expB300Row.定级变形检测压力Z13;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[13] = expB300Row.定级变形检测压力Z14;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[14] = expB300Row.定级变形检测压力Z15;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[15] = expB300Row.定级变形检测压力Z16;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[16] = expB300Row.定级变形检测压力Z17;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[17] = expB300Row.定级变形检测压力Z18;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[18] = expB300Row.定级变形检测压力Z19;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[19] = expB300Row.定级变形检测压力Z20;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[0] = expB300Row.定级变形检测压力F01;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[1] = expB300Row.定级变形检测压力F02;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[2] = expB300Row.定级变形检测压力F03;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[3] = expB300Row.定级变形检测压力F04;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[4] = expB300Row.定级变形检测压力F05;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[5] = expB300Row.定级变形检测压力F06;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[6] = expB300Row.定级变形检测压力F07;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[7] = expB300Row.定级变形检测压力F08;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[8] = expB300Row.定级变形检测压力F09;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[9] = expB300Row.定级变形检测压力F10;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[10] = expB300Row.定级变形检测压力F11;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[11] = expB300Row.定级变形检测压力F12;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[12] = expB300Row.定级变形检测压力F13;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[13] = expB300Row.定级变形检测压力F14;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[14] = expB300Row.定级变形检测压力F15;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[15] = expB300Row.定级变形检测压力F16;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[16] = expB300Row.定级变形检测压力F17;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[17] = expB300Row.定级变形检测压力F18;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[18] = expB300Row.定级变形检测压力F19;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[19] = expB300Row.定级变形检测压力F20;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJP2_Z = expB300Row.定级反复检测压力p2Z;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJP2_F = expB300Row.定级反复检测压力p2F;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJP3_Z = expB300Row.定级标准值检测压力p3Z;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJP3_F = expB300Row.定级标准值检测压力p3F;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJPmax_Z = expB300Row.定级设计值检测压力pmaxZ;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_DJPmax_F = expB300Row.定级设计值检测压力pmaxF;
                    //定级评定数据;
                    PublicData.ExpDQ.ExpData_KFY.PDValue_DJP1_Z = expB300Row.定级评定p1Z;
                    PublicData.ExpDQ.ExpData_KFY.PDValue_DJP1_F = expB300Row.定级评定p1F;
                    PublicData.ExpDQ.ExpData_KFY.PDValue_DJP1_All = expB300Row.定级评定p1;
                    PublicData.ExpDQ.ExpData_KFY.PDValue_DJP2_Z = expB300Row.定级评定p2Z;
                    PublicData.ExpDQ.ExpData_KFY.PDValue_DJP2_F = expB300Row.定级评定p2F;
                    PublicData.ExpDQ.ExpData_KFY.PDValue_DJP2_All = expB300Row.定级评定p2;
                    PublicData.ExpDQ.ExpData_KFY.PDValue_DJP3_Z = expB300Row.定级评定p3Z;
                    PublicData.ExpDQ.ExpData_KFY.PDValue_DJP3_F = expB300Row.定级评定p3F;
                    PublicData.ExpDQ.ExpData_KFY.PDValue_DJP3_All = expB300Row.定级评定p3;
                    PublicData.ExpDQ.ExpData_KFY.PdLevel_DJP3 = expB300Row.定级p3评级;
                    //工程检测压力;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_GCBX_Z[0] = expB300Row.工程变形检测压力Z01;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_GCBX_Z[1] = expB300Row.工程变形检测压力Z02;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_GCBX_Z[2] = expB300Row.工程变形检测压力Z03;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_GCBX_Z[3] = expB300Row.工程变形检测压力Z04;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_GCBX_F[0] = expB300Row.工程变形检测压力F01;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_GCBX_F[1] = expB300Row.工程变形检测压力F02;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_GCBX_F[2] = expB300Row.工程变形检测压力F03;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_GCBX_F[3] = expB300Row.工程变形检测压力F04;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_GCP2_Z = expB300Row.工程反复检测压力p2Z;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_GCP2_F = expB300Row.工程反复检测压力p2F;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_GCP3_Z = expB300Row.工程标准值检测压力p3Z;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_GCP3_F = expB300Row.工程标准值检测压力p3F;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_GCPmax_Z = expB300Row.工程设计值检测压力pmaxZ;
                    PublicData.ExpDQ.ExpData_KFY.TestPress_GCPmax_F = expB300Row.工程设计值检测压力pmaxF;
                    //工程评定数据;
                    PublicData.ExpDQ.ExpData_KFY.IsMeetDesign_GCp1 = expB300Row.工程检测p1是否满足要求;
                    PublicData.ExpDQ.ExpData_KFY.IsMeetDesign_GCp2 = expB300Row.工程检测p2是否满足要求;
                    PublicData.ExpDQ.ExpData_KFY.IsMeetDesign_GCp3 = expB300Row.工程检测p3是否满足要求;
                    PublicData.ExpDQ.ExpData_KFY.IsMeetDesign_GCpmax = expB300Row.工程检测pmax是否满足要求;
                    PublicData.ExpDQ.ExpData_KFY.IsMeetDesign_GCfinal = expB300Row.工程检测总体是否满足要求;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B311a抗风压组1点a定级位移数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB311a(string givenExpName)
        {

            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B311a抗风压定级组1点a位移数据Row checkExistB311aRow = B311aTable.FindBy试验编号(tempNOStr);
                if (checkExistB311aRow == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B311a抗风压定级组1点a位移数据Row defExpB311aRow = B311aTable.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B311a抗风压定级组1点a位移数据Row newExpB311aRow = B311aTable.NewB311a抗风压定级组1点a位移数据Row();
                    newExpB311aRow.ItemArray = (object[])defExpB311aRow.ItemArray.Clone();
                    newExpB311aRow.试验编号 = tempNOStr;
                    B311aTable.AddB311a抗风压定级组1点a位移数据Row(newExpB311aRow);
                    B311aTableAdapter.Update(B311aTable);
                    B311aTable.AcceptChanges();
                    RaisePropertyChanged(() => B311aTable);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B311a表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B311a抗风压定级组1点a位移数据Row expB311aRow = B311aTable.FindBy试验编号(tempNOStr);
                if (expB311aRow != null)
                {
                    //定级P1正
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[0][0] = expB311aRow.定级变形1aZ00;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[1][0] = expB311aRow.定级变形1aZ01;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[2][0] = expB311aRow.定级变形1aZ02;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[3][0] = expB311aRow.定级变形1aZ03;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[4][0] = expB311aRow.定级变形1aZ04;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[5][0] = expB311aRow.定级变形1aZ05;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[6][0] = expB311aRow.定级变形1aZ06;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[7][0] = expB311aRow.定级变形1aZ07;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[8][0] = expB311aRow.定级变形1aZ08;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[9][0] = expB311aRow.定级变形1aZ09;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[10][0] = expB311aRow.定级变形1aZ10;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[11][0] = expB311aRow.定级变形1aZ11;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[12][0] = expB311aRow.定级变形1aZ12;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[13][0] = expB311aRow.定级变形1aZ13;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[14][0] = expB311aRow.定级变形1aZ14;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[15][0] = expB311aRow.定级变形1aZ15;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[16][0] = expB311aRow.定级变形1aZ16;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[17][0] = expB311aRow.定级变形1aZ17;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[18][0] = expB311aRow.定级变形1aZ18;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[19][0] = expB311aRow.定级变形1aZ19;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[20][0] = expB311aRow.定级变形1aZ20;
                    //定级P1负
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[0][0] = expB311aRow.定级变形1aF00;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[1][0] = expB311aRow.定级变形1aF01;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[2][0] = expB311aRow.定级变形1aF02;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[3][0] = expB311aRow.定级变形1aF03;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[4][0] = expB311aRow.定级变形1aF04;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[5][0] = expB311aRow.定级变形1aF05;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[6][0] = expB311aRow.定级变形1aF06;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[7][0] = expB311aRow.定级变形1aF07;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[8][0] = expB311aRow.定级变形1aF08;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[9][0] = expB311aRow.定级变形1aF09;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[10][0] = expB311aRow.定级变形1aF10;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[11][0] = expB311aRow.定级变形1aF11;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[12][0] = expB311aRow.定级变形1aF12;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[13][0] = expB311aRow.定级变形1aF13;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[14][0] = expB311aRow.定级变形1aF14;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[15][0] = expB311aRow.定级变形1aF15;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[16][0] = expB311aRow.定级变形1aF16;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[17][0] = expB311aRow.定级变形1aF17;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[18][0] = expB311aRow.定级变形1aF18;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[19][0] = expB311aRow.定级变形1aF19;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[20][0] = expB311aRow.定级变形1aF20;
                    //定级p3
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[0][0] = expB311aRow.定级P31aZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[1][0] = expB311aRow.定级P31aZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[0][0] = expB311aRow.定级P31aF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[1][0] = expB311aRow.定级P31aF1;
                    //定级pmax
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][0] = expB311aRow.定级Pmax1aZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][0] = expB311aRow.定级Pmax1aZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[0][0] = expB311aRow.定级Pmax1aF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[1][0] = expB311aRow.定级Pmax1aF1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B311b抗风压组1点b定级位移数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB311b(string givenExpName)
        {

            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B311b抗风压定级组1点b位移数据Row checkExistB311bRow = B311bTable.FindBy试验编号(tempNOStr);
                if (checkExistB311bRow == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B311b抗风压定级组1点b位移数据Row defExpB311bRow = B311bTable.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B311b抗风压定级组1点b位移数据Row newExpB311bRow = B311bTable.NewB311b抗风压定级组1点b位移数据Row();
                    newExpB311bRow.ItemArray = (object[])defExpB311bRow.ItemArray.Clone();
                    newExpB311bRow.试验编号 = tempNOStr;
                    B311bTable.AddB311b抗风压定级组1点b位移数据Row(newExpB311bRow);
                    B311bTableAdapter.Update(B311bTable);
                    B311bTable.AcceptChanges();
                    RaisePropertyChanged(() => B311bTable);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B311b表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B311b抗风压定级组1点b位移数据Row expB311bRow = B311bTable.FindBy试验编号(tempNOStr);
                if (expB311bRow != null)
                {
                    //定级P1正
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[0][1] = expB311bRow.定级变形1bZ00;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[1][1] = expB311bRow.定级变形1bZ01;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[2][1] = expB311bRow.定级变形1bZ02;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[3][1] = expB311bRow.定级变形1bZ03;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[4][1] = expB311bRow.定级变形1bZ04;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[5][1] = expB311bRow.定级变形1bZ05;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[6][1] = expB311bRow.定级变形1bZ06;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[7][1] = expB311bRow.定级变形1bZ07;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[8][1] = expB311bRow.定级变形1bZ08;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[9][1] = expB311bRow.定级变形1bZ09;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[10][1] = expB311bRow.定级变形1bZ10;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[11][1] = expB311bRow.定级变形1bZ11;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[12][1] = expB311bRow.定级变形1bZ12;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[13][1] = expB311bRow.定级变形1bZ13;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[14][1] = expB311bRow.定级变形1bZ14;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[15][1] = expB311bRow.定级变形1bZ15;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[16][1] = expB311bRow.定级变形1bZ16;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[17][1] = expB311bRow.定级变形1bZ17;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[18][1] = expB311bRow.定级变形1bZ18;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[19][1] = expB311bRow.定级变形1bZ19;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[20][1] = expB311bRow.定级变形1bZ20;
                    //定级P1负
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[0][1] = expB311bRow.定级变形1bF00;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[1][1] = expB311bRow.定级变形1bF01;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[2][1] = expB311bRow.定级变形1bF02;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[3][1] = expB311bRow.定级变形1bF03;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[4][1] = expB311bRow.定级变形1bF04;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[5][1] = expB311bRow.定级变形1bF05;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[6][1] = expB311bRow.定级变形1bF06;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[7][1] = expB311bRow.定级变形1bF07;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[8][1] = expB311bRow.定级变形1bF08;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[9][1] = expB311bRow.定级变形1bF09;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[10][1] = expB311bRow.定级变形1bF10;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[11][1] = expB311bRow.定级变形1bF11;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[12][1] = expB311bRow.定级变形1bF12;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[13][1] = expB311bRow.定级变形1bF13;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[14][1] = expB311bRow.定级变形1bF14;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[15][1] = expB311bRow.定级变形1bF15;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[16][1] = expB311bRow.定级变形1bF16;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[17][1] = expB311bRow.定级变形1bF17;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[18][1] = expB311bRow.定级变形1bF18;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[19][1] = expB311bRow.定级变形1bF19;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[20][1] = expB311bRow.定级变形1bF20;
                    //定级p3
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[0][1] = expB311bRow.定级P31bZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[1][1] = expB311bRow.定级P31bZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[0][1] = expB311bRow.定级P31bF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[1][1] = expB311bRow.定级P31bF1;
                    //定级pmax
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][1] = expB311bRow.定级Pmax1bZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][1] = expB311bRow.定级Pmax1bZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[0][1] = expB311bRow.定级Pmax1bF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[1][1] = expB311bRow.定级Pmax1bF1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B311c抗风压组1点c定级位移数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB311c(string givenExpName)
        {

            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B311c抗风压定级组1点c位移数据Row checkExistB311cRow = B311cTable.FindBy试验编号(tempNOStr);
                if (checkExistB311cRow == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B311c抗风压定级组1点c位移数据Row defExpB311cRow = B311cTable.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B311c抗风压定级组1点c位移数据Row newExpB311cRow = B311cTable.NewB311c抗风压定级组1点c位移数据Row();
                    newExpB311cRow.ItemArray = (object[])defExpB311cRow.ItemArray.Clone();
                    newExpB311cRow.试验编号 = tempNOStr;
                    B311cTable.AddB311c抗风压定级组1点c位移数据Row(newExpB311cRow);
                    B311cTableAdapter.Update(B311cTable);
                    B311cTable.AcceptChanges();
                    RaisePropertyChanged(() => B311cTable);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B311c表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B311c抗风压定级组1点c位移数据Row expB311cRow = B311cTable.FindBy试验编号(tempNOStr);
                if (expB311cRow != null)
                {
                    //定级P1正
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[0][2] = expB311cRow.定级变形1cZ00;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[1][2] = expB311cRow.定级变形1cZ01;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[2][2] = expB311cRow.定级变形1cZ02;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[3][2] = expB311cRow.定级变形1cZ03;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[4][2] = expB311cRow.定级变形1cZ04;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[5][2] = expB311cRow.定级变形1cZ05;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[6][2] = expB311cRow.定级变形1cZ06;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[7][2] = expB311cRow.定级变形1cZ07;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[8][2] = expB311cRow.定级变形1cZ08;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[9][2] = expB311cRow.定级变形1cZ09;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[10][2] = expB311cRow.定级变形1cZ10;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[11][2] = expB311cRow.定级变形1cZ11;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[12][2] = expB311cRow.定级变形1cZ12;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[13][2] = expB311cRow.定级变形1cZ13;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[14][2] = expB311cRow.定级变形1cZ14;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[15][2] = expB311cRow.定级变形1cZ15;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[16][2] = expB311cRow.定级变形1cZ16;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[17][2] = expB311cRow.定级变形1cZ17;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[18][2] = expB311cRow.定级变形1cZ18;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[19][2] = expB311cRow.定级变形1cZ19;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[20][2] = expB311cRow.定级变形1cZ20;
                    //定级P1负
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[0][2] = expB311cRow.定级变形1cF00;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[1][2] = expB311cRow.定级变形1cF01;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[2][2] = expB311cRow.定级变形1cF02;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[3][2] = expB311cRow.定级变形1cF03;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[4][2] = expB311cRow.定级变形1cF04;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[5][2] = expB311cRow.定级变形1cF05;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[6][2] = expB311cRow.定级变形1cF06;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[7][2] = expB311cRow.定级变形1cF07;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[8][2] = expB311cRow.定级变形1cF08;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[9][2] = expB311cRow.定级变形1cF09;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[10][2] = expB311cRow.定级变形1cF10;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[11][2] = expB311cRow.定级变形1cF11;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[12][2] = expB311cRow.定级变形1cF12;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[13][2] = expB311cRow.定级变形1cF13;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[14][2] = expB311cRow.定级变形1cF14;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[15][2] = expB311cRow.定级变形1cF15;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[16][2] = expB311cRow.定级变形1cF16;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[17][2] = expB311cRow.定级变形1cF17;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[18][2] = expB311cRow.定级变形1cF18;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[19][2] = expB311cRow.定级变形1cF19;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[20][2] = expB311cRow.定级变形1cF20;
                    //定级p3
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[0][2] = expB311cRow.定级P31cZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[1][2] = expB311cRow.定级P31cZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[0][2] = expB311cRow.定级P31cF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[1][2] = expB311cRow.定级P31cF1;
                    //定级pmax
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][2] = expB311cRow.定级Pmax1cZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][2] = expB311cRow.定级Pmax1cZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[0][2] = expB311cRow.定级Pmax1cF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[1][2] = expB311cRow.定级Pmax1cF1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B311d抗风压组1点d定级位移数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB311d(string givenExpName)
        {

            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B311d抗风压定级组1点d位移数据Row checkExistB311dRow = B311dTable.FindBy试验编号(tempNOStr);
                if (checkExistB311dRow == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B311d抗风压定级组1点d位移数据Row defExpB311dRow = B311dTable.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B311d抗风压定级组1点d位移数据Row newExpB311dRow = B311dTable.NewB311d抗风压定级组1点d位移数据Row();
                    newExpB311dRow.ItemArray = (object[])defExpB311dRow.ItemArray.Clone();
                    newExpB311dRow.试验编号 = tempNOStr;
                    B311dTable.AddB311d抗风压定级组1点d位移数据Row(newExpB311dRow);
                    B311dTableAdapter.Update(B311dTable);
                    B311dTable.AcceptChanges();
                    RaisePropertyChanged(() => B311dTable);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B311d表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B311d抗风压定级组1点d位移数据Row expB311dRow = B311dTable.FindBy试验编号(tempNOStr);
                if (expB311dRow != null)
                {
                    //定级P1正
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[0][3] = expB311dRow.定级变形1dZ00;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[1][3] = expB311dRow.定级变形1dZ01;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[2][3] = expB311dRow.定级变形1dZ02;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[3][3] = expB311dRow.定级变形1dZ03;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[4][3] = expB311dRow.定级变形1dZ04;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[5][3] = expB311dRow.定级变形1dZ05;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[6][3] = expB311dRow.定级变形1dZ06;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[7][3] = expB311dRow.定级变形1dZ07;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[8][3] = expB311dRow.定级变形1dZ08;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[9][3] = expB311dRow.定级变形1dZ09;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[10][3] = expB311dRow.定级变形1dZ10;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[11][3] = expB311dRow.定级变形1dZ11;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[12][3] = expB311dRow.定级变形1dZ12;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[13][3] = expB311dRow.定级变形1dZ13;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[14][3] = expB311dRow.定级变形1dZ14;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[15][3] = expB311dRow.定级变形1dZ15;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[16][3] = expB311dRow.定级变形1dZ16;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[17][3] = expB311dRow.定级变形1dZ17;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[18][3] = expB311dRow.定级变形1dZ18;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[19][3] = expB311dRow.定级变形1dZ19;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[20][3] = expB311dRow.定级变形1dZ20;
                    //定级P1负
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[0][3] = expB311dRow.定级变形1dF00;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[1][3] = expB311dRow.定级变形1dF01;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[2][3] = expB311dRow.定级变形1dF02;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[3][3] = expB311dRow.定级变形1dF03;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[4][3] = expB311dRow.定级变形1dF04;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[5][3] = expB311dRow.定级变形1dF05;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[6][3] = expB311dRow.定级变形1dF06;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[7][3] = expB311dRow.定级变形1dF07;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[8][3] = expB311dRow.定级变形1dF08;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[9][3] = expB311dRow.定级变形1dF09;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[10][3] = expB311dRow.定级变形1dF10;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[11][3] = expB311dRow.定级变形1dF11;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[12][3] = expB311dRow.定级变形1dF12;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[13][3] = expB311dRow.定级变形1dF13;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[14][3] = expB311dRow.定级变形1dF14;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[15][3] = expB311dRow.定级变形1dF15;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[16][3] = expB311dRow.定级变形1dF16;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[17][3] = expB311dRow.定级变形1dF17;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[18][3] = expB311dRow.定级变形1dF18;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[19][3] = expB311dRow.定级变形1dF19;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[20][3] = expB311dRow.定级变形1dF20;
                    //定级p3
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[0][3] = expB311dRow.定级P31dZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[1][3] = expB311dRow.定级P31dZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[0][3] = expB311dRow.定级P31dF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[1][3] = expB311dRow.定级P31dF1;
                    //定级pmax
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][3] = expB311dRow.定级Pmax1dZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][3] = expB311dRow.定级Pmax1dZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[0][3] = expB311dRow.定级Pmax1dF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[1][3] = expB311dRow.定级Pmax1dF1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B312a抗风压组2点a定级位移数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB312a(string givenExpName)
        {

            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B312a抗风压定级组2点a位移数据Row checkExistB312aRow = B312aTable.FindBy试验编号(tempNOStr);
                if (checkExistB312aRow == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B312a抗风压定级组2点a位移数据Row defExpB312aRow = B312aTable.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B312a抗风压定级组2点a位移数据Row newExpB312aRow = B312aTable.NewB312a抗风压定级组2点a位移数据Row();
                    newExpB312aRow.ItemArray = (object[])defExpB312aRow.ItemArray.Clone();
                    newExpB312aRow.试验编号 = tempNOStr;
                    B312aTable.AddB312a抗风压定级组2点a位移数据Row(newExpB312aRow);
                    B312aTableAdapter.Update(B312aTable);
                    B312aTable.AcceptChanges();
                    RaisePropertyChanged(() => B312aTable);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B312a表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B312a抗风压定级组2点a位移数据Row expB312aRow = B312aTable.FindBy试验编号(tempNOStr);
                if (expB312aRow != null)
                {
                    //定级P1正
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[0][4] = expB312aRow.定级变形2aZ00;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[1][4] = expB312aRow.定级变形2aZ01;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[2][4] = expB312aRow.定级变形2aZ02;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[3][4] = expB312aRow.定级变形2aZ03;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[4][4] = expB312aRow.定级变形2aZ04;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[5][4] = expB312aRow.定级变形2aZ05;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[6][4] = expB312aRow.定级变形2aZ06;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[7][4] = expB312aRow.定级变形2aZ07;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[8][4] = expB312aRow.定级变形2aZ08;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[9][4] = expB312aRow.定级变形2aZ09;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[10][4] = expB312aRow.定级变形2aZ10;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[11][4] = expB312aRow.定级变形2aZ11;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[12][4] = expB312aRow.定级变形2aZ12;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[13][4] = expB312aRow.定级变形2aZ13;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[14][4] = expB312aRow.定级变形2aZ14;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[15][4] = expB312aRow.定级变形2aZ15;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[16][4] = expB312aRow.定级变形2aZ16;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[17][4] = expB312aRow.定级变形2aZ17;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[18][4] = expB312aRow.定级变形2aZ18;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[19][4] = expB312aRow.定级变形2aZ19;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[20][4] = expB312aRow.定级变形2aZ20;
                    //定级P1负
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[0][4] = expB312aRow.定级变形2aF00;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[1][4] = expB312aRow.定级变形2aF01;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[2][4] = expB312aRow.定级变形2aF02;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[3][4] = expB312aRow.定级变形2aF03;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[4][4] = expB312aRow.定级变形2aF04;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[5][4] = expB312aRow.定级变形2aF05;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[6][4] = expB312aRow.定级变形2aF06;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[7][4] = expB312aRow.定级变形2aF07;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[8][4] = expB312aRow.定级变形2aF08;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[9][4] = expB312aRow.定级变形2aF09;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[10][4] = expB312aRow.定级变形2aF10;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[11][4] = expB312aRow.定级变形2aF11;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[12][4] = expB312aRow.定级变形2aF12;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[13][4] = expB312aRow.定级变形2aF13;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[14][4] = expB312aRow.定级变形2aF14;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[15][4] = expB312aRow.定级变形2aF15;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[16][4] = expB312aRow.定级变形2aF16;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[17][4] = expB312aRow.定级变形2aF17;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[18][4] = expB312aRow.定级变形2aF18;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[19][4] = expB312aRow.定级变形2aF19;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[20][4] = expB312aRow.定级变形2aF20;
                    //定级p3
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[0][4] = expB312aRow.定级P32aZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[1][4] = expB312aRow.定级P32aZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[0][4] = expB312aRow.定级P32aF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[1][4] = expB312aRow.定级P32aF1;
                    //定级pmax
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][4] = expB312aRow.定级Pmax2aZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][4] = expB312aRow.定级Pmax2aZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[0][4] = expB312aRow.定级Pmax2aF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[1][4] = expB312aRow.定级Pmax2aF1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B312b抗风压组2点b定级位移数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB312b(string givenExpName)
        {

            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B312b抗风压定级组2点b位移数据Row checkExistB312bRow = B312bTable.FindBy试验编号(tempNOStr);
                if (checkExistB312bRow == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B312b抗风压定级组2点b位移数据Row defExpB312bRow = B312bTable.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B312b抗风压定级组2点b位移数据Row newExpB312bRow = B312bTable.NewB312b抗风压定级组2点b位移数据Row();
                    newExpB312bRow.ItemArray = (object[])defExpB312bRow.ItemArray.Clone();
                    newExpB312bRow.试验编号 = tempNOStr;
                    B312bTable.AddB312b抗风压定级组2点b位移数据Row(newExpB312bRow);
                    B312bTableAdapter.Update(B312bTable);
                    B312bTable.AcceptChanges();
                    RaisePropertyChanged(() => B312bTable);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B312b表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B312b抗风压定级组2点b位移数据Row expB312bRow = B312bTable.FindBy试验编号(tempNOStr);
                if (expB312bRow != null)
                {
                    //定级P1正
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[0][5] = expB312bRow.定级变形2bZ00;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[1][5] = expB312bRow.定级变形2bZ01;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[2][5] = expB312bRow.定级变形2bZ02;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[3][5] = expB312bRow.定级变形2bZ03;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[4][5] = expB312bRow.定级变形2bZ04;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[5][5] = expB312bRow.定级变形2bZ05;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[6][5] = expB312bRow.定级变形2bZ06;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[7][5] = expB312bRow.定级变形2bZ07;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[8][5] = expB312bRow.定级变形2bZ08;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[9][5] = expB312bRow.定级变形2bZ09;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[10][5] = expB312bRow.定级变形2bZ10;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[11][5] = expB312bRow.定级变形2bZ11;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[12][5] = expB312bRow.定级变形2bZ12;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[13][5] = expB312bRow.定级变形2bZ13;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[14][5] = expB312bRow.定级变形2bZ14;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[15][5] = expB312bRow.定级变形2bZ15;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[16][5] = expB312bRow.定级变形2bZ16;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[17][5] = expB312bRow.定级变形2bZ17;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[18][5] = expB312bRow.定级变形2bZ18;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[19][5] = expB312bRow.定级变形2bZ19;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[20][5] = expB312bRow.定级变形2bZ20;
                    //定级P1负
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[0][5] = expB312bRow.定级变形2bF00;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[1][5] = expB312bRow.定级变形2bF01;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[2][5] = expB312bRow.定级变形2bF02;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[3][5] = expB312bRow.定级变形2bF03;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[4][5] = expB312bRow.定级变形2bF04;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[5][5] = expB312bRow.定级变形2bF05;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[6][5] = expB312bRow.定级变形2bF06;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[7][5] = expB312bRow.定级变形2bF07;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[8][5] = expB312bRow.定级变形2bF08;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[9][5] = expB312bRow.定级变形2bF09;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[10][5] = expB312bRow.定级变形2bF10;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[11][5] = expB312bRow.定级变形2bF11;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[12][5] = expB312bRow.定级变形2bF12;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[13][5] = expB312bRow.定级变形2bF13;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[14][5] = expB312bRow.定级变形2bF14;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[15][5] = expB312bRow.定级变形2bF15;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[16][5] = expB312bRow.定级变形2bF16;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[17][5] = expB312bRow.定级变形2bF17;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[18][5] = expB312bRow.定级变形2bF18;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[19][5] = expB312bRow.定级变形2bF19;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[20][5] = expB312bRow.定级变形2bF20;
                    //定级p3
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[0][5] = expB312bRow.定级P32bZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[1][5] = expB312bRow.定级P32bZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[0][5] = expB312bRow.定级P32bF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[1][5] = expB312bRow.定级P32bF1;
                    //定级pmax
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][5] = expB312bRow.定级Pmax2bZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][5] = expB312bRow.定级Pmax2bZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[0][5] = expB312bRow.定级Pmax2bF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[1][5] = expB312bRow.定级Pmax2bF1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B312c抗风压组2点c定级位移数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB312c(string givenExpName)
        {

            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B312c抗风压定级组2点c位移数据Row checkExistB312cRow = B312cTable.FindBy试验编号(tempNOStr);
                if (checkExistB312cRow == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B312c抗风压定级组2点c位移数据Row defExpB312cRow = B312cTable.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B312c抗风压定级组2点c位移数据Row newExpB312cRow = B312cTable.NewB312c抗风压定级组2点c位移数据Row();
                    newExpB312cRow.ItemArray = (object[])defExpB312cRow.ItemArray.Clone();
                    newExpB312cRow.试验编号 = tempNOStr;
                    B312cTable.AddB312c抗风压定级组2点c位移数据Row(newExpB312cRow);
                    B312cTableAdapter.Update(B312cTable);
                    B312cTable.AcceptChanges();
                    RaisePropertyChanged(() => B312cTable);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B312c表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B312c抗风压定级组2点c位移数据Row expB312cRow = B312cTable.FindBy试验编号(tempNOStr);
                if (expB312cRow != null)
                {
                    //定级P1正
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[0][6] = expB312cRow.定级变形2cZ00;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[1][6] = expB312cRow.定级变形2cZ01;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[2][6] = expB312cRow.定级变形2cZ02;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[3][6] = expB312cRow.定级变形2cZ03;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[4][6] = expB312cRow.定级变形2cZ04;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[5][6] = expB312cRow.定级变形2cZ05;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[6][6] = expB312cRow.定级变形2cZ06;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[7][6] = expB312cRow.定级变形2cZ07;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[8][6] = expB312cRow.定级变形2cZ08;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[9][6] = expB312cRow.定级变形2cZ09;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[10][6] = expB312cRow.定级变形2cZ10;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[11][6] = expB312cRow.定级变形2cZ11;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[12][6] = expB312cRow.定级变形2cZ12;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[13][6] = expB312cRow.定级变形2cZ13;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[14][6] = expB312cRow.定级变形2cZ14;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[15][6] = expB312cRow.定级变形2cZ15;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[16][6] = expB312cRow.定级变形2cZ16;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[17][6] = expB312cRow.定级变形2cZ17;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[18][6] = expB312cRow.定级变形2cZ18;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[19][6] = expB312cRow.定级变形2cZ19;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[20][6] = expB312cRow.定级变形2cZ20;
                    //定级P1负
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[0][6] = expB312cRow.定级变形2cF00;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[1][6] = expB312cRow.定级变形2cF01;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[2][6] = expB312cRow.定级变形2cF02;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[3][6] = expB312cRow.定级变形2cF03;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[4][6] = expB312cRow.定级变形2cF04;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[5][6] = expB312cRow.定级变形2cF05;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[6][6] = expB312cRow.定级变形2cF06;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[7][6] = expB312cRow.定级变形2cF07;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[8][6] = expB312cRow.定级变形2cF08;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[9][6] = expB312cRow.定级变形2cF09;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[10][6] = expB312cRow.定级变形2cF10;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[11][6] = expB312cRow.定级变形2cF11;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[12][6] = expB312cRow.定级变形2cF12;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[13][6] = expB312cRow.定级变形2cF13;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[14][6] = expB312cRow.定级变形2cF14;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[15][6] = expB312cRow.定级变形2cF15;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[16][6] = expB312cRow.定级变形2cF16;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[17][6] = expB312cRow.定级变形2cF17;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[18][6] = expB312cRow.定级变形2cF18;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[19][6] = expB312cRow.定级变形2cF19;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[20][6] = expB312cRow.定级变形2cF20;
                    //定级p3
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[0][6] = expB312cRow.定级P32cZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[1][6] = expB312cRow.定级P32cZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[0][6] = expB312cRow.定级P32cF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[1][6] = expB312cRow.定级P32cF1;
                    //定级pmax
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][6] = expB312cRow.定级Pmax2cZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][6] = expB312cRow.定级Pmax2cZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[0][6] = expB312cRow.定级Pmax2cF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[1][6] = expB312cRow.定级Pmax2cF1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B313a抗风压组3点a定级位移数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB313a(string givenExpName)
        {

            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B313a抗风压定级组3点a位移数据Row checkExistB313aRow = B313aTable.FindBy试验编号(tempNOStr);
                if (checkExistB313aRow == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B313a抗风压定级组3点a位移数据Row defExpB313aRow = B313aTable.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B313a抗风压定级组3点a位移数据Row newExpB313aRow = B313aTable.NewB313a抗风压定级组3点a位移数据Row();
                    newExpB313aRow.ItemArray = (object[])defExpB313aRow.ItemArray.Clone();
                    newExpB313aRow.试验编号 = tempNOStr;
                    B313aTable.AddB313a抗风压定级组3点a位移数据Row(newExpB313aRow);
                    B313aTableAdapter.Update(B313aTable);
                    B313aTable.AcceptChanges();
                    RaisePropertyChanged(() => B313aTable);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B313a表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B313a抗风压定级组3点a位移数据Row expB313aRow = B313aTable.FindBy试验编号(tempNOStr);
                if (expB313aRow != null)
                {
                    //定级P1正
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[0][7] = expB313aRow.定级变形3aZ00;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[1][7] = expB313aRow.定级变形3aZ01;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[2][7] = expB313aRow.定级变形3aZ02;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[3][7] = expB313aRow.定级变形3aZ03;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[4][7] = expB313aRow.定级变形3aZ04;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[5][7] = expB313aRow.定级变形3aZ05;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[6][7] = expB313aRow.定级变形3aZ06;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[7][7] = expB313aRow.定级变形3aZ07;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[8][7] = expB313aRow.定级变形3aZ08;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[9][7] = expB313aRow.定级变形3aZ09;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[10][7] = expB313aRow.定级变形3aZ10;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[11][7] = expB313aRow.定级变形3aZ11;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[12][7] = expB313aRow.定级变形3aZ12;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[13][7] = expB313aRow.定级变形3aZ13;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[14][7] = expB313aRow.定级变形3aZ14;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[15][7] = expB313aRow.定级变形3aZ15;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[16][7] = expB313aRow.定级变形3aZ16;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[17][7] = expB313aRow.定级变形3aZ17;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[18][7] = expB313aRow.定级变形3aZ18;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[19][7] = expB313aRow.定级变形3aZ19;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[20][7] = expB313aRow.定级变形3aZ20;
                    //定级P1负
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[0][7] = expB313aRow.定级变形3aF00;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[1][7] = expB313aRow.定级变形3aF01;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[2][7] = expB313aRow.定级变形3aF02;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[3][7] = expB313aRow.定级变形3aF03;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[4][7] = expB313aRow.定级变形3aF04;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[5][7] = expB313aRow.定级变形3aF05;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[6][7] = expB313aRow.定级变形3aF06;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[7][7] = expB313aRow.定级变形3aF07;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[8][7] = expB313aRow.定级变形3aF08;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[9][7] = expB313aRow.定级变形3aF09;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[10][7] = expB313aRow.定级变形3aF10;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[11][7] = expB313aRow.定级变形3aF11;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[12][7] = expB313aRow.定级变形3aF12;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[13][7] = expB313aRow.定级变形3aF13;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[14][7] = expB313aRow.定级变形3aF14;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[15][7] = expB313aRow.定级变形3aF15;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[16][7] = expB313aRow.定级变形3aF16;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[17][7] = expB313aRow.定级变形3aF17;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[18][7] = expB313aRow.定级变形3aF18;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[19][7] = expB313aRow.定级变形3aF19;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[20][7] = expB313aRow.定级变形3aF20;
                    //定级p3
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[0][7] = expB313aRow.定级P33aZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[1][7] = expB313aRow.定级P33aZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[0][7] = expB313aRow.定级P33aF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[1][7] = expB313aRow.定级P33aF1;
                    //定级pmax
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][7] = expB313aRow.定级Pmax3aZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][7] = expB313aRow.定级Pmax3aZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[0][7] = expB313aRow.定级Pmax3aF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[1][7] = expB313aRow.定级Pmax3aF1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B313b抗风压组3点b定级位移数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB313b(string givenExpName)
        {

            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B313b抗风压定级组3点b位移数据Row checkExistB313bRow = B313bTable.FindBy试验编号(tempNOStr);
                if (checkExistB313bRow == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B313b抗风压定级组3点b位移数据Row defExpB313bRow = B313bTable.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B313b抗风压定级组3点b位移数据Row newExpB313bRow = B313bTable.NewB313b抗风压定级组3点b位移数据Row();
                    newExpB313bRow.ItemArray = (object[])defExpB313bRow.ItemArray.Clone();
                    newExpB313bRow.试验编号 = tempNOStr;
                    B313bTable.AddB313b抗风压定级组3点b位移数据Row(newExpB313bRow);
                    B313bTableAdapter.Update(B313bTable);
                    B313bTable.AcceptChanges();
                    RaisePropertyChanged(() => B313bTable);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B313b表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B313b抗风压定级组3点b位移数据Row expB313bRow = B313bTable.FindBy试验编号(tempNOStr);
                if (expB313bRow != null)
                {
                    //定级P1正
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[0][8] = expB313bRow.定级变形3bZ00;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[1][8] = expB313bRow.定级变形3bZ01;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[2][8] = expB313bRow.定级变形3bZ02;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[3][8] = expB313bRow.定级变形3bZ03;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[4][8] = expB313bRow.定级变形3bZ04;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[5][8] = expB313bRow.定级变形3bZ05;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[6][8] = expB313bRow.定级变形3bZ06;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[7][8] = expB313bRow.定级变形3bZ07;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[8][8] = expB313bRow.定级变形3bZ08;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[9][8] = expB313bRow.定级变形3bZ09;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[10][8] = expB313bRow.定级变形3bZ10;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[11][8] = expB313bRow.定级变形3bZ11;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[12][8] = expB313bRow.定级变形3bZ12;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[13][8] = expB313bRow.定级变形3bZ13;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[14][8] = expB313bRow.定级变形3bZ14;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[15][8] = expB313bRow.定级变形3bZ15;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[16][8] = expB313bRow.定级变形3bZ16;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[17][8] = expB313bRow.定级变形3bZ17;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[18][8] = expB313bRow.定级变形3bZ18;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[19][8] = expB313bRow.定级变形3bZ19;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[20][8] = expB313bRow.定级变形3bZ20;
                    //定级P1负
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[0][8] = expB313bRow.定级变形3bF00;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[1][8] = expB313bRow.定级变形3bF01;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[2][8] = expB313bRow.定级变形3bF02;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[3][8] = expB313bRow.定级变形3bF03;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[4][8] = expB313bRow.定级变形3bF04;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[5][8] = expB313bRow.定级变形3bF05;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[6][8] = expB313bRow.定级变形3bF06;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[7][8] = expB313bRow.定级变形3bF07;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[8][8] = expB313bRow.定级变形3bF08;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[9][8] = expB313bRow.定级变形3bF09;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[10][8] = expB313bRow.定级变形3bF10;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[11][8] = expB313bRow.定级变形3bF11;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[12][8] = expB313bRow.定级变形3bF12;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[13][8] = expB313bRow.定级变形3bF13;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[14][8] = expB313bRow.定级变形3bF14;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[15][8] = expB313bRow.定级变形3bF15;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[16][8] = expB313bRow.定级变形3bF16;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[17][8] = expB313bRow.定级变形3bF17;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[18][8] = expB313bRow.定级变形3bF18;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[19][8] = expB313bRow.定级变形3bF19;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[20][8] = expB313bRow.定级变形3bF20;
                    //定级p3
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[0][8] = expB313bRow.定级P33bZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[1][8] = expB313bRow.定级P33bZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[0][8] = expB313bRow.定级P33bF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[1][8] = expB313bRow.定级P33bF1;
                    //定级pmax
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][8] = expB313bRow.定级Pmax3bZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][8] = expB313bRow.定级Pmax3bZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[0][8] = expB313bRow.定级Pmax3bF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[1][8] = expB313bRow.定级Pmax3bF1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B313c抗风压组3点c定级位移数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB313c(string givenExpName)
        {

            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B313c抗风压定级组3点c位移数据Row checkExistB313cRow = B313cTable.FindBy试验编号(tempNOStr);
                if (checkExistB313cRow == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B313c抗风压定级组3点c位移数据Row defExpB313cRow = B313cTable.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B313c抗风压定级组3点c位移数据Row newExpB313cRow = B313cTable.NewB313c抗风压定级组3点c位移数据Row();
                    newExpB313cRow.ItemArray = (object[])defExpB313cRow.ItemArray.Clone();
                    newExpB313cRow.试验编号 = tempNOStr;
                    B313cTable.AddB313c抗风压定级组3点c位移数据Row(newExpB313cRow);
                    B313cTableAdapter.Update(B313cTable);
                    B313cTable.AcceptChanges();
                    RaisePropertyChanged(() => B313cTable);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B313c表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B313c抗风压定级组3点c位移数据Row expB313cRow = B313cTable.FindBy试验编号(tempNOStr);
                if (expB313cRow != null)
                {
                    //定级P1正
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[0][9] = expB313cRow.定级变形3cZ00;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[1][9] = expB313cRow.定级变形3cZ01;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[2][9] = expB313cRow.定级变形3cZ02;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[3][9] = expB313cRow.定级变形3cZ03;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[4][9] = expB313cRow.定级变形3cZ04;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[5][9] = expB313cRow.定级变形3cZ05;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[6][9] = expB313cRow.定级变形3cZ06;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[7][9] = expB313cRow.定级变形3cZ07;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[8][9] = expB313cRow.定级变形3cZ08;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[9][9] = expB313cRow.定级变形3cZ09;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[10][9] = expB313cRow.定级变形3cZ10;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[11][9] = expB313cRow.定级变形3cZ11;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[12][9] = expB313cRow.定级变形3cZ12;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[13][9] = expB313cRow.定级变形3cZ13;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[14][9] = expB313cRow.定级变形3cZ14;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[15][9] = expB313cRow.定级变形3cZ15;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[16][9] = expB313cRow.定级变形3cZ16;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[17][9] = expB313cRow.定级变形3cZ17;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[18][9] = expB313cRow.定级变形3cZ18;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[19][9] = expB313cRow.定级变形3cZ19;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_Z[20][9] = expB313cRow.定级变形3cZ20;
                    //定级P1负
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[0][9] = expB313cRow.定级变形3cF00;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[1][9] = expB313cRow.定级变形3cF01;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[2][9] = expB313cRow.定级变形3cF02;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[3][9] = expB313cRow.定级变形3cF03;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[4][9] = expB313cRow.定级变形3cF04;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[5][9] = expB313cRow.定级变形3cF05;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[6][9] = expB313cRow.定级变形3cF06;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[7][9] = expB313cRow.定级变形3cF07;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[8][9] = expB313cRow.定级变形3cF08;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[9][9] = expB313cRow.定级变形3cF09;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[10][9] = expB313cRow.定级变形3cF10;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[11][9] = expB313cRow.定级变形3cF11;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[12][9] = expB313cRow.定级变形3cF12;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[13][9] = expB313cRow.定级变形3cF13;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[14][9] = expB313cRow.定级变形3cF14;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[15][9] = expB313cRow.定级变形3cF15;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[16][9] = expB313cRow.定级变形3cF16;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[17][9] = expB313cRow.定级变形3cF17;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[18][9] = expB313cRow.定级变形3cF18;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[19][9] = expB313cRow.定级变形3cF19;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJBX_F[20][9] = expB313cRow.定级变形3cF20;
                    //定级p3
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[0][9] = expB313cRow.定级P33cZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_Z[1][9] = expB313cRow.定级P33cZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[0][9] = expB313cRow.定级P33cF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJP3_F[1][9] = expB313cRow.定级P33cF1;
                    //定级pmax
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][9] = expB313cRow.定级Pmax3cZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][9] = expB313cRow.定级Pmax3cZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[0][9] = expB313cRow.定级Pmax3cF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_DJPmax_F[1][9] = expB313cRow.定级Pmax3cF1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B321抗风压定级组1相对挠度数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB321(string givenExpName)
        {

            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B321抗风压定级组1相对挠度数据Row checkExistB321Row = B321Table.FindBy试验编号(tempNOStr);
                if (checkExistB321Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B321抗风压定级组1相对挠度数据Row defExpB321Row = B321Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B321抗风压定级组1相对挠度数据Row newExpB321Row = B321Table.NewB321抗风压定级组1相对挠度数据Row();
                    newExpB321Row.ItemArray = (object[])defExpB321Row.ItemArray.Clone();
                    newExpB321Row.试验编号 = tempNOStr;
                    B321Table.AddB321抗风压定级组1相对挠度数据Row(newExpB321Row);
                    B321TableAdapter.Update(B321Table);
                    B321Table.AcceptChanges();
                    RaisePropertyChanged(() => B321Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B321表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B321抗风压定级组1相对挠度数据Row expB321Row = B321Table.FindBy试验编号(tempNOStr);
                if (expB321Row != null)
                {
                    //定级P1正
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[0][0] = expB321Row.定级变形0Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[1][0] = expB321Row.定级变形100Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[2][0] = expB321Row.定级变形200Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[3][0] = expB321Row.定级变形300Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[4][0] = expB321Row.定级变形400Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[5][0] = expB321Row.定级变形500Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[6][0] = expB321Row.定级变形600Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[7][0] = expB321Row.定级变形700Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[8][0] = expB321Row.定级变形800Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[9][0] = expB321Row.定级变形900Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[10][0] = expB321Row.定级变形1000Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[11][0] = expB321Row.定级变形1100Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[12][0] = expB321Row.定级变形1200Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[13][0] = expB321Row.定级变形1300Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[14][0] = expB321Row.定级变形1400Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[15][0] = expB321Row.定级变形1500Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[16][0] = expB321Row.定级变形1600Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[17][0] = expB321Row.定级变形1700Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[18][0] = expB321Row.定级变形1800Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[19][0] = expB321Row.定级变形1900Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[20][0] = expB321Row.定级变形2000Pa相对挠度1;
                    //定级P1负
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[0][0] = expB321Row.定级变形负0Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[1][0] = expB321Row.定级变形负100Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[2][0] = expB321Row.定级变形负200Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[3][0] = expB321Row.定级变形负300Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[4][0] = expB321Row.定级变形负400Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[5][0] = expB321Row.定级变形负500Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[6][0] = expB321Row.定级变形负600Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[7][0] = expB321Row.定级变形负700Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[8][0] = expB321Row.定级变形负800Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[9][0] = expB321Row.定级变形负900Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[10][0] = expB321Row.定级变形负1000Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[11][0] = expB321Row.定级变形负1100Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[12][0] = expB321Row.定级变形负1200Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[13][0] = expB321Row.定级变形负1300Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[14][0] = expB321Row.定级变形负1400Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[15][0] = expB321Row.定级变形负1500Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[16][0] = expB321Row.定级变形负1600Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[17][0] = expB321Row.定级变形负1700Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[18][0] = expB321Row.定级变形负1800Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[19][0] = expB321Row.定级变形负1900Pa相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[20][0] = expB321Row.定级变形负2000Pa相对挠度1;
                    //定级P3
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_Z[0][0] = expB321Row.定级P3前相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_F[0][0] = expB321Row.定级负P3前相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_Z[1][0] = expB321Row.定级P3相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_F[1][0] = expB321Row.定级负P3相对挠度1;
                    //定级Pmax
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_Z[0][0] = expB321Row.定级Pmax前相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_F[0][0] = expB321Row.定级负Pmax前相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_Z[1][0] = expB321Row.定级Pmax相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_F[1][0] = expB321Row.定级负Pmax相对挠度1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B322抗风压定级组2相对挠度数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB322(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B322抗风压定级组2相对挠度数据Row checkExistB322Row = B322Table.FindBy试验编号(tempNOStr);
                if (checkExistB322Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B322抗风压定级组2相对挠度数据Row defExpB322Row = B322Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B322抗风压定级组2相对挠度数据Row newExpB322Row = B322Table.NewB322抗风压定级组2相对挠度数据Row();
                    newExpB322Row.ItemArray = (object[])defExpB322Row.ItemArray.Clone();
                    newExpB322Row.试验编号 = tempNOStr;
                    B322Table.AddB322抗风压定级组2相对挠度数据Row(newExpB322Row);
                    B322TableAdapter.Update(B322Table);
                    B322Table.AcceptChanges();
                    RaisePropertyChanged(() => B322Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B322表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B322抗风压定级组2相对挠度数据Row expB322Row = B322Table.FindBy试验编号(tempNOStr);
                if (expB322Row != null)
                {
                    //定级P1正
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[0][1] = expB322Row.定级变形0Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[1][1] = expB322Row.定级变形100Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[2][1] = expB322Row.定级变形200Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[3][1] = expB322Row.定级变形300Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[4][1] = expB322Row.定级变形400Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[5][1] = expB322Row.定级变形500Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[6][1] = expB322Row.定级变形600Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[7][1] = expB322Row.定级变形700Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[8][1] = expB322Row.定级变形800Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[9][1] = expB322Row.定级变形900Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[10][1] = expB322Row.定级变形1000Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[11][1] = expB322Row.定级变形1100Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[12][1] = expB322Row.定级变形1200Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[13][1] = expB322Row.定级变形1300Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[14][1] = expB322Row.定级变形1400Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[15][1] = expB322Row.定级变形1500Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[16][1] = expB322Row.定级变形1600Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[17][1] = expB322Row.定级变形1700Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[18][1] = expB322Row.定级变形1800Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[19][1] = expB322Row.定级变形1900Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[20][1] = expB322Row.定级变形2000Pa相对挠度2;
                    //定级P1负
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[0][1] = expB322Row.定级变形负0Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[1][1] = expB322Row.定级变形负100Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[2][1] = expB322Row.定级变形负200Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[3][1] = expB322Row.定级变形负300Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[4][1] = expB322Row.定级变形负400Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[5][1] = expB322Row.定级变形负500Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[6][1] = expB322Row.定级变形负600Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[7][1] = expB322Row.定级变形负700Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[8][1] = expB322Row.定级变形负800Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[9][1] = expB322Row.定级变形负900Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[10][1] = expB322Row.定级变形负1000Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[11][1] = expB322Row.定级变形负1100Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[12][1] = expB322Row.定级变形负1200Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[13][1] = expB322Row.定级变形负1300Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[14][1] = expB322Row.定级变形负1400Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[15][1] = expB322Row.定级变形负1500Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[16][1] = expB322Row.定级变形负1600Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[17][1] = expB322Row.定级变形负1700Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[18][1] = expB322Row.定级变形负1800Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[19][1] = expB322Row.定级变形负1900Pa相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[20][1] = expB322Row.定级变形负2000Pa相对挠度2;
                    //定级P3
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_Z[0][1] = expB322Row.定级P3前相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_F[0][1] = expB322Row.定级负P3前相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_Z[1][1] = expB322Row.定级P3相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_F[1][1] = expB322Row.定级负P3相对挠度2;
                    //定级Pmax
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_Z[0][1] = expB322Row.定级Pmax前相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_F[0][1] = expB322Row.定级负Pmax前相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_Z[1][1] = expB322Row.定级Pmax相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_F[1][1] = expB322Row.定级负Pmax相对挠度2;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B323抗风压定级组3相对挠度数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB323(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B323抗风压定级组3相对挠度数据Row checkExistB323Row = B323Table.FindBy试验编号(tempNOStr);
                if (checkExistB323Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B323抗风压定级组3相对挠度数据Row defExpB323Row = B323Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B323抗风压定级组3相对挠度数据Row newExpB323Row = B323Table.NewB323抗风压定级组3相对挠度数据Row();
                    newExpB323Row.ItemArray = (object[])defExpB323Row.ItemArray.Clone();
                    newExpB323Row.试验编号 = tempNOStr;
                    B323Table.AddB323抗风压定级组3相对挠度数据Row(newExpB323Row);
                    B323TableAdapter.Update(B323Table);
                    B323Table.AcceptChanges();
                    RaisePropertyChanged(() => B323Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B323表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B323抗风压定级组3相对挠度数据Row expB323Row = B323Table.FindBy试验编号(tempNOStr);
                if (expB323Row != null)
                {
                    //定级P1正
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[0][2] = expB323Row.定级变形0Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[1][2] = expB323Row.定级变形100Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[2][2] = expB323Row.定级变形200Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[3][2] = expB323Row.定级变形300Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[4][2] = expB323Row.定级变形400Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[5][2] = expB323Row.定级变形500Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[6][2] = expB323Row.定级变形600Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[7][2] = expB323Row.定级变形700Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[8][2] = expB323Row.定级变形800Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[9][2] = expB323Row.定级变形900Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[10][2] = expB323Row.定级变形1000Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[11][2] = expB323Row.定级变形1100Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[12][2] = expB323Row.定级变形1200Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[13][2] = expB323Row.定级变形1300Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[14][2] = expB323Row.定级变形1400Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[15][2] = expB323Row.定级变形1500Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[16][2] = expB323Row.定级变形1600Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[17][2] = expB323Row.定级变形1700Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[18][2] = expB323Row.定级变形1800Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[19][2] = expB323Row.定级变形1900Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_Z[20][2] = expB323Row.定级变形2000Pa相对挠度3;
                    //定级P1负
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[0][2] = expB323Row.定级变形负0Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[1][2] = expB323Row.定级变形负100Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[2][2] = expB323Row.定级变形负200Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[3][2] = expB323Row.定级变形负300Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[4][2] = expB323Row.定级变形负400Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[5][2] = expB323Row.定级变形负500Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[6][2] = expB323Row.定级变形负600Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[7][2] = expB323Row.定级变形负700Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[8][2] = expB323Row.定级变形负800Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[9][2] = expB323Row.定级变形负900Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[10][2] = expB323Row.定级变形负1000Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[11][2] = expB323Row.定级变形负1100Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[12][2] = expB323Row.定级变形负1200Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[13][2] = expB323Row.定级变形负1300Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[14][2] = expB323Row.定级变形负1400Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[15][2] = expB323Row.定级变形负1500Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[16][2] = expB323Row.定级变形负1600Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[17][2] = expB323Row.定级变形负1700Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[18][2] = expB323Row.定级变形负1800Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[19][2] = expB323Row.定级变形负1900Pa相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJBX_F[20][2] = expB323Row.定级变形负2000Pa相对挠度3;
                    //定级P3
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_Z[0][2] = expB323Row.定级P3前相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_F[0][2] = expB323Row.定级负P3前相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_Z[1][2] = expB323Row.定级P3相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJP3_F[1][2] = expB323Row.定级负P3相对挠度3;
                    //定级Pmax;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_Z[0][2] = expB323Row.定级Pmax前相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_F[0][2] = expB323Row.定级负Pmax前相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_Z[1][2] = expB323Row.定级Pmax相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_DJPmax_F[1][2] = expB323Row.定级负Pmax相对挠度3;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B324抗风压定级相对挠度最大值数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB324(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B324抗风压定级相对挠度最大值数据Row checkExistB324Row = B324Table.FindBy试验编号(tempNOStr);
                if (checkExistB324Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B324抗风压定级相对挠度最大值数据Row defExpB324Row = B324Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B324抗风压定级相对挠度最大值数据Row newExpB324Row = B324Table.NewB324抗风压定级相对挠度最大值数据Row();
                    newExpB324Row.ItemArray = (object[])defExpB324Row.ItemArray.Clone();
                    newExpB324Row.试验编号 = tempNOStr;
                    B324Table.AddB324抗风压定级相对挠度最大值数据Row(newExpB324Row);
                    B324TableAdapter.Update(B324Table);
                    B324Table.AcceptChanges();
                    RaisePropertyChanged(() => B324Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B324表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B324抗风压定级相对挠度最大值数据Row expB324Row = B324Table.FindBy试验编号(tempNOStr);
                if (expB324Row != null)
                {
                    //定级P1正
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[0] = expB324Row.定级变形0Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[1] = expB324Row.定级变形100Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[2] = expB324Row.定级变形200Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[3] = expB324Row.定级变形300Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[4] = expB324Row.定级变形400Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[5] = expB324Row.定级变形500Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[6] = expB324Row.定级变形600Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[7] = expB324Row.定级变形700Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[8] = expB324Row.定级变形800Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[9] = expB324Row.定级变形900Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[10] = expB324Row.定级变形1000Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[11] = expB324Row.定级变形1100Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[12] = expB324Row.定级变形1200Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[13] = expB324Row.定级变形1300Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[14] = expB324Row.定级变形1400Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[15] = expB324Row.定级变形1500Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[16] = expB324Row.定级变形1600Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[17] = expB324Row.定级变形1700Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[18] = expB324Row.定级变形1800Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[19] = expB324Row.定级变形1900Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[20] = expB324Row.定级变形2000Pa相对挠度max;
                    //定级P1负
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[0] = expB324Row.定级变形负0Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[1] = expB324Row.定级变形负100Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[2] = expB324Row.定级变形负200Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[3] = expB324Row.定级变形负300Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[4] = expB324Row.定级变形负400Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[5] = expB324Row.定级变形负500Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[6] = expB324Row.定级变形负600Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[7] = expB324Row.定级变形负700Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[8] = expB324Row.定级变形负800Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[9] = expB324Row.定级变形负900Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[10] = expB324Row.定级变形负1000Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[11] = expB324Row.定级变形负1100Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[12] = expB324Row.定级变形负1200Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[13] = expB324Row.定级变形负1300Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[14] = expB324Row.定级变形负1400Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[15] = expB324Row.定级变形负1500Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[16] = expB324Row.定级变形负1600Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[17] = expB324Row.定级变形负1700Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[18] = expB324Row.定级变形负1800Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[19] = expB324Row.定级变形负1900Pa相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[20] = expB324Row.定级变形负2000Pa相对挠度max;
                    //定级P3
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJp3_Z = expB324Row.定级P3相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJp3_F = expB324Row.定级负P3相对挠度max;
                    //定级Pmax
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJpmax_Z = expB324Row.定级Pmax相对挠度max;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_DJpmax_F = expB324Row.定级负Pmax相对挠度max;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B331抗风压定级组1挠度数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB331(string givenExpName)
        {

            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B331抗风压定级组1挠度数据Row checkExistB331Row = B331Table.FindBy试验编号(tempNOStr);
                if (checkExistB331Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B331抗风压定级组1挠度数据Row defExpB331Row = B331Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B331抗风压定级组1挠度数据Row newExpB331Row = B331Table.NewB331抗风压定级组1挠度数据Row();
                    newExpB331Row.ItemArray = (object[])defExpB331Row.ItemArray.Clone();
                    newExpB331Row.试验编号 = tempNOStr;
                    B331Table.AddB331抗风压定级组1挠度数据Row(newExpB331Row);
                    B331TableAdapter.Update(B331Table);
                    B331Table.AcceptChanges();
                    RaisePropertyChanged(() => B331Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B331表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B331抗风压定级组1挠度数据Row expB331Row = B331Table.FindBy试验编号(tempNOStr);
                if (expB331Row != null)
                {
                    //定级P1正
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[0][0] = expB331Row.定级变形0Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[1][0] = expB331Row.定级变形100Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[2][0] = expB331Row.定级变形200Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[3][0] = expB331Row.定级变形300Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[4][0] = expB331Row.定级变形400Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[5][0] = expB331Row.定级变形500Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[6][0] = expB331Row.定级变形600Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[7][0] = expB331Row.定级变形700Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[8][0] = expB331Row.定级变形800Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[9][0] = expB331Row.定级变形900Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[10][0] = expB331Row.定级变形1000Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[11][0] = expB331Row.定级变形1100Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[12][0] = expB331Row.定级变形1200Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[13][0] = expB331Row.定级变形1300Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[14][0] = expB331Row.定级变形1400Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[15][0] = expB331Row.定级变形1500Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[16][0] = expB331Row.定级变形1600Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[17][0] = expB331Row.定级变形1700Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[18][0] = expB331Row.定级变形1800Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[19][0] = expB331Row.定级变形1900Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[20][0] = expB331Row.定级变形2000Pa挠度1;
                    //定级P1负
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[0][0] = expB331Row.定级变形负0Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[1][0] = expB331Row.定级变形负100Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[2][0] = expB331Row.定级变形负200Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[3][0] = expB331Row.定级变形负300Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[4][0] = expB331Row.定级变形负400Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[5][0] = expB331Row.定级变形负500Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[6][0] = expB331Row.定级变形负600Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[7][0] = expB331Row.定级变形负700Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[8][0] = expB331Row.定级变形负800Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[9][0] = expB331Row.定级变形负900Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[10][0] = expB331Row.定级变形负1000Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[11][0] = expB331Row.定级变形负1100Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[12][0] = expB331Row.定级变形负1200Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[13][0] = expB331Row.定级变形负1300Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[14][0] = expB331Row.定级变形负1400Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[15][0] = expB331Row.定级变形负1500Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[16][0] = expB331Row.定级变形负1600Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[17][0] = expB331Row.定级变形负1700Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[18][0] = expB331Row.定级变形负1800Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[19][0] = expB331Row.定级变形负1900Pa挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[20][0] = expB331Row.定级变形负2000Pa挠度1;
                    //定级P3
                    PublicData.ExpDQ.ExpData_KFY.ND_DJP3_Z[0][0] = expB331Row.定级P3前挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJP3_F[0][0] = expB331Row.定级负P3前挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJP3_Z[1][0] = expB331Row.定级P3挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJP3_F[1][0] = expB331Row.定级负P3挠度1;
                    //定级Pmax
                    PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_Z[0][0] = expB331Row.定级Pmax前挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_F[0][0] = expB331Row.定级负Pmax前挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_Z[1][0] = expB331Row.定级Pmax挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_F[1][0] = expB331Row.定级负Pmax挠度1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B332抗风压定级组2挠度数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB332(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B332抗风压定级组2挠度数据Row checkExistB332Row = B332Table.FindBy试验编号(tempNOStr);
                if (checkExistB332Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B332抗风压定级组2挠度数据Row defExpB332Row = B332Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B332抗风压定级组2挠度数据Row newExpB332Row = B332Table.NewB332抗风压定级组2挠度数据Row();
                    newExpB332Row.ItemArray = (object[])defExpB332Row.ItemArray.Clone();
                    newExpB332Row.试验编号 = tempNOStr;
                    B332Table.AddB332抗风压定级组2挠度数据Row(newExpB332Row);
                    B332TableAdapter.Update(B332Table);
                    B332Table.AcceptChanges();
                    RaisePropertyChanged(() => B332Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B332表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B332抗风压定级组2挠度数据Row expB332Row = B332Table.FindBy试验编号(tempNOStr);
                if (expB332Row != null)
                {
                    //定级P1正
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[0][1] = expB332Row.定级变形0Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[1][1] = expB332Row.定级变形100Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[2][1] = expB332Row.定级变形200Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[3][1] = expB332Row.定级变形300Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[4][1] = expB332Row.定级变形400Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[5][1] = expB332Row.定级变形500Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[6][1] = expB332Row.定级变形600Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[7][1] = expB332Row.定级变形700Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[8][1] = expB332Row.定级变形800Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[9][1] = expB332Row.定级变形900Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[10][1] = expB332Row.定级变形1000Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[11][1] = expB332Row.定级变形1100Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[12][1] = expB332Row.定级变形1200Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[13][1] = expB332Row.定级变形1300Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[14][1] = expB332Row.定级变形1400Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[15][1] = expB332Row.定级变形1500Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[16][1] = expB332Row.定级变形1600Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[17][1] = expB332Row.定级变形1700Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[18][1] = expB332Row.定级变形1800Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[19][1] = expB332Row.定级变形1900Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[20][1] = expB332Row.定级变形2000Pa挠度2;
                    //定级P1负
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[0][1] = expB332Row.定级变形负0Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[1][1] = expB332Row.定级变形负100Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[2][1] = expB332Row.定级变形负200Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[3][1] = expB332Row.定级变形负300Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[4][1] = expB332Row.定级变形负400Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[5][1] = expB332Row.定级变形负500Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[6][1] = expB332Row.定级变形负600Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[7][1] = expB332Row.定级变形负700Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[8][1] = expB332Row.定级变形负800Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[9][1] = expB332Row.定级变形负900Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[10][1] = expB332Row.定级变形负1000Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[11][1] = expB332Row.定级变形负1100Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[12][1] = expB332Row.定级变形负1200Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[13][1] = expB332Row.定级变形负1300Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[14][1] = expB332Row.定级变形负1400Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[15][1] = expB332Row.定级变形负1500Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[16][1] = expB332Row.定级变形负1600Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[17][1] = expB332Row.定级变形负1700Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[18][1] = expB332Row.定级变形负1800Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[19][1] = expB332Row.定级变形负1900Pa挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[20][1] = expB332Row.定级变形负2000Pa挠度2;
                    //定级P3
                    PublicData.ExpDQ.ExpData_KFY.ND_DJP3_Z[0][1] = expB332Row.定级P3前挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJP3_F[0][1] = expB332Row.定级负P3前挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJP3_Z[1][1] = expB332Row.定级P3挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJP3_F[1][1] = expB332Row.定级负P3挠度2;
                    //定级Pmax
                    PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_Z[0][1] = expB332Row.定级Pmax前挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_F[0][1] = expB332Row.定级负Pmax前挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_Z[1][1] = expB332Row.定级Pmax挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_F[1][1] = expB332Row.定级负Pmax挠度2;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B333抗风压定级组3挠度数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB333(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B333抗风压定级组3挠度数据Row checkExistB333Row = B333Table.FindBy试验编号(tempNOStr);
                if (checkExistB333Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B333抗风压定级组3挠度数据Row defExpB333Row = B333Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B333抗风压定级组3挠度数据Row newExpB333Row = B333Table.NewB333抗风压定级组3挠度数据Row();
                    newExpB333Row.ItemArray = (object[])defExpB333Row.ItemArray.Clone();
                    newExpB333Row.试验编号 = tempNOStr;
                    B333Table.AddB333抗风压定级组3挠度数据Row(newExpB333Row);
                    B333TableAdapter.Update(B333Table);
                    B333Table.AcceptChanges();
                    RaisePropertyChanged(() => B333Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B333表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B333抗风压定级组3挠度数据Row expB333Row = B333Table.FindBy试验编号(tempNOStr);
                if (expB333Row != null)
                {
                    //定级P1正
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[0][2] = expB333Row.定级变形0Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[1][2] = expB333Row.定级变形100Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[2][2] = expB333Row.定级变形200Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[3][2] = expB333Row.定级变形300Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[4][2] = expB333Row.定级变形400Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[5][2] = expB333Row.定级变形500Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[6][2] = expB333Row.定级变形600Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[7][2] = expB333Row.定级变形700Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[8][2] = expB333Row.定级变形800Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[9][2] = expB333Row.定级变形900Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[10][2] = expB333Row.定级变形1000Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[11][2] = expB333Row.定级变形1100Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[12][2] = expB333Row.定级变形1200Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[13][2] = expB333Row.定级变形1300Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[14][2] = expB333Row.定级变形1400Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[15][2] = expB333Row.定级变形1500Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[16][2] = expB333Row.定级变形1600Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[17][2] = expB333Row.定级变形1700Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[18][2] = expB333Row.定级变形1800Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[19][2] = expB333Row.定级变形1900Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[20][2] = expB333Row.定级变形2000Pa挠度3;
                    //定级P1负
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[0][2] = expB333Row.定级变形负0Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[1][2] = expB333Row.定级变形负100Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[2][2] = expB333Row.定级变形负200Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[3][2] = expB333Row.定级变形负300Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[4][2] = expB333Row.定级变形负400Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[5][2] = expB333Row.定级变形负500Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[6][2] = expB333Row.定级变形负600Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[7][2] = expB333Row.定级变形负700Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[8][2] = expB333Row.定级变形负800Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[9][2] = expB333Row.定级变形负900Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[10][2] = expB333Row.定级变形负1000Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[11][2] = expB333Row.定级变形负1100Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[12][2] = expB333Row.定级变形负1200Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[13][2] = expB333Row.定级变形负1300Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[14][2] = expB333Row.定级变形负1400Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[15][2] = expB333Row.定级变形负1500Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[16][2] = expB333Row.定级变形负1600Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[17][2] = expB333Row.定级变形负1700Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[18][2] = expB333Row.定级变形负1800Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[19][2] = expB333Row.定级变形负1900Pa挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[20][2] = expB333Row.定级变形负2000Pa挠度3;
                    //定级P3
                    PublicData.ExpDQ.ExpData_KFY.ND_DJP3_Z[0][2] = expB333Row.定级P3前挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJP3_F[0][2] = expB333Row.定级负P3前挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJP3_Z[1][2] = expB333Row.定级P3挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJP3_F[1][2] = expB333Row.定级负P3挠度3;
                    //定级Pmax
                    PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_Z[0][2] = expB333Row.定级Pmax前挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_F[0][2] = expB333Row.定级负Pmax前挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_Z[1][2] = expB333Row.定级Pmax挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_F[1][2] = expB333Row.定级负Pmax挠度3;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B334抗风压定级挠度最大值数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB334(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B334抗风压定级挠度最大值数据Row checkExistB334Row = B334Table.FindBy试验编号(tempNOStr);
                if (checkExistB334Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B334抗风压定级挠度最大值数据Row defExpB334Row = B334Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B334抗风压定级挠度最大值数据Row newExpB334Row = B334Table.NewB334抗风压定级挠度最大值数据Row();
                    newExpB334Row.ItemArray = (object[])defExpB334Row.ItemArray.Clone();
                    newExpB334Row.试验编号 = tempNOStr;
                    B334Table.AddB334抗风压定级挠度最大值数据Row(newExpB334Row);
                    B334TableAdapter.Update(B334Table);
                    B334Table.AcceptChanges();
                    RaisePropertyChanged(() => B334Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B334表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B334抗风压定级挠度最大值数据Row expB334Row = B334Table.FindBy试验编号(tempNOStr);
                if (expB334Row != null)
                {
                    //定级P1正
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[0] = expB334Row.定级变形0Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[1] = expB334Row.定级变形100Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[2] = expB334Row.定级变形200Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[3] = expB334Row.定级变形300Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[4] = expB334Row.定级变形400Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[5] = expB334Row.定级变形500Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[6] = expB334Row.定级变形600Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[7] = expB334Row.定级变形700Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[8] = expB334Row.定级变形800Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[9] = expB334Row.定级变形900Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[10] = expB334Row.定级变形1000Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[11] = expB334Row.定级变形1100Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[12] = expB334Row.定级变形1200Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[13] = expB334Row.定级变形1300Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[14] = expB334Row.定级变形1400Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[15] = expB334Row.定级变形1500Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[16] = expB334Row.定级变形1600Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[17] = expB334Row.定级变形1700Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[18] = expB334Row.定级变形1800Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[19] = expB334Row.定级变形1900Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[20] = expB334Row.定级变形2000Pa挠度fmax;
                    //定级P1负
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[0] = expB334Row.定级变形负0Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[1] = expB334Row.定级变形负100Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[2] = expB334Row.定级变形负200Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[3] = expB334Row.定级变形负300Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[4] = expB334Row.定级变形负400Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[5] = expB334Row.定级变形负500Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[6] = expB334Row.定级变形负600Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[7] = expB334Row.定级变形负700Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[8] = expB334Row.定级变形负800Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[9] = expB334Row.定级变形负900Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[10] = expB334Row.定级变形负1000Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[11] = expB334Row.定级变形负1100Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[12] = expB334Row.定级变形负1200Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[13] = expB334Row.定级变形负1300Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[14] = expB334Row.定级变形负1400Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[15] = expB334Row.定级变形负1500Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[16] = expB334Row.定级变形负1600Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[17] = expB334Row.定级变形负1700Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[18] = expB334Row.定级变形负1800Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[19] = expB334Row.定级变形负1900Pa挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJBX_F[20] = expB334Row.定级变形负2000Pa挠度fmax;
                    //定级P3
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJp3_Z = expB334Row.定级P3挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJp3_F = expB334Row.定级负P3挠度fmax;
                    //定级Pfmax
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJpmax_Z = expB334Row.定级Pmax挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_DJpmax_F = expB334Row.定级负Pmax挠度fmax;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B340抗风压定级损坏情况数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB340(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B340抗风压定级损坏情况数据Row checkExistB340Row = B340Table.FindBy试验编号(tempNOStr);
                if (checkExistB340Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B340抗风压定级损坏情况数据Row defExpB340Row = B340Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B340抗风压定级损坏情况数据Row newExpB340Row = B340Table.NewB340抗风压定级损坏情况数据Row();
                    newExpB340Row.ItemArray = (object[])defExpB340Row.ItemArray.Clone();
                    newExpB340Row.试验编号 = tempNOStr;
                    B340Table.AddB340抗风压定级损坏情况数据Row(newExpB340Row);
                    B340TableAdapter.Update(B340Table);
                    B340Table.AcceptChanges();
                    RaisePropertyChanged(() => B340Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B340表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B340抗风压定级损坏情况数据Row expB340Row = B340Table.FindBy试验编号(tempNOStr);
                if (expB340Row != null)
                {
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[0] = expB340Row.定级变形100Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[1] = expB340Row.定级变形200Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[2] = expB340Row.定级变形300Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[3] = expB340Row.定级变形400Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[4] = expB340Row.定级变形500Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[5] = expB340Row.定级变形600Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[6] = expB340Row.定级变形700Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[7] = expB340Row.定级变形800Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[8] = expB340Row.定级变形900Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[9] = expB340Row.定级变形1000Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[10] = expB340Row.定级变形1100Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[11] = expB340Row.定级变形1200Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[12] = expB340Row.定级变形1300Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[13] = expB340Row.定级变形1400Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[14] = expB340Row.定级变形1500Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[15] = expB340Row.定级变形1600Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[16] = expB340Row.定级变形1700Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[17] = expB340Row.定级变形1800Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[18] = expB340Row.定级变形1900Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_Z[19] = expB340Row.定级变形2000Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[0] = expB340Row.定级变形负100Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[1] = expB340Row.定级变形负200Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[2] = expB340Row.定级变形负300Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[3] = expB340Row.定级变形负400Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[4] = expB340Row.定级变形负500Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[5] = expB340Row.定级变形负600Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[6] = expB340Row.定级变形负700Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[7] = expB340Row.定级变形负800Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[8] = expB340Row.定级变形负900Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[9] = expB340Row.定级变形负1000Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[10] = expB340Row.定级变形负1100Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[11] = expB340Row.定级变形负1200Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[12] = expB340Row.定级变形负1300Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[13] = expB340Row.定级变形负1400Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[14] = expB340Row.定级变形负1500Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[15] = expB340Row.定级变形负1600Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[16] = expB340Row.定级变形负1700Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[17] = expB340Row.定级变形负1800Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[18] = expB340Row.定级变形负1900Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJBX_F[19] = expB340Row.定级变形负2000Pa损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJP2_Z = expB340Row.定级P2损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJP2_F = expB340Row.定级负P2损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJP3_Z = expB340Row.定级P3损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJP3_F = expB340Row.定级负P3损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_DJPmax_Z = expB340Row.定级Pmax损坏情况;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B350抗风压定级损坏说明数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB350(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B350抗风压定级损坏说明数据Row checkExistB350Row = B350Table.FindBy试验编号(tempNOStr);
                if (checkExistB350Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B350抗风压定级损坏说明数据Row defExpB350Row = B350Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B350抗风压定级损坏说明数据Row newExpB350Row = B350Table.NewB350抗风压定级损坏说明数据Row();
                    newExpB350Row.ItemArray = (object[])defExpB350Row.ItemArray.Clone();
                    newExpB350Row.试验编号 = tempNOStr;
                    B350Table.AddB350抗风压定级损坏说明数据Row(newExpB350Row);
                    B350TableAdapter.Update(B350Table);
                    B350Table.AcceptChanges();
                    RaisePropertyChanged(() => B350Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B350表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B350抗风压定级损坏说明数据Row expB350Row = B350Table.FindBy试验编号(tempNOStr);
                if (expB350Row != null)
                {
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[0] = expB350Row.定级变形100Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[1] = expB350Row.定级变形200Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[2] = expB350Row.定级变形300Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[3] = expB350Row.定级变形400Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[4] = expB350Row.定级变形500Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[5] = expB350Row.定级变形600Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[6] = expB350Row.定级变形700Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[7] = expB350Row.定级变形800Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[8] = expB350Row.定级变形900Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[9] = expB350Row.定级变形1000Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[10] = expB350Row.定级变形1100Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[11] = expB350Row.定级变形1200Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[12] = expB350Row.定级变形1300Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[13] = expB350Row.定级变形1400Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[14] = expB350Row.定级变形1500Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[15] = expB350Row.定级变形1600Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[16] = expB350Row.定级变形1700Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[17] = expB350Row.定级变形1800Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[18] = expB350Row.定级变形1900Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[19] = expB350Row.定级变形2000Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_F[0] = expB350Row.定级变形负100Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_F[1] = expB350Row.定级变形负200Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_F[2] = expB350Row.定级变形负300Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_F[3] = expB350Row.定级变形负400Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_F[4] = expB350Row.定级变形负500Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_F[5] = expB350Row.定级变形负600Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_F[6] = expB350Row.定级变形负700Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_F[7] = expB350Row.定级变形负800Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_F[8] = expB350Row.定级变形负900Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_F[9] = expB350Row.定级变形负1000Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_F[10] = expB350Row.定级变形负1100Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_F[11] = expB350Row.定级变形负1200Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_F[12] = expB350Row.定级变形负1300Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_F[13] = expB350Row.定级变形负1400Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_F[14] = expB350Row.定级变形负1500Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_F[15] = expB350Row.定级变形负1600Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_F[16] = expB350Row.定级变形负1700Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_F[17] = expB350Row.定级变形负1800Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_F[18] = expB350Row.定级变形负1900Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJBX_F[19] = expB350Row.定级变形负2000Pa损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJP2_Z = expB350Row.定级P2损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJP2_F = expB350Row.定级负P2损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJP3_Z = expB350Row.定级P3损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJP3_F = expB350Row.定级负P3损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJPmax_Z = expB350Row.定级Pmax损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_DJPmax_F = expB350Row.定级负Pmax损坏说明;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B361抗风压组1工程位移数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB361(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B361抗风压组1工程位移数据Row checkExistB361Row = B361Table.FindBy试验编号(tempNOStr);
                if (checkExistB361Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B361抗风压组1工程位移数据Row defExpB361Row = B361Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B361抗风压组1工程位移数据Row newExpB361Row = B361Table.NewB361抗风压组1工程位移数据Row();
                    newExpB361Row.ItemArray = (object[])defExpB361Row.ItemArray.Clone();
                    newExpB361Row.试验编号 = tempNOStr;
                    B361Table.AddB361抗风压组1工程位移数据Row(newExpB361Row);
                    B361TableAdapter.Update(B361Table);
                    B361Table.AcceptChanges();
                    RaisePropertyChanged(() => B361Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B361表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B361抗风压组1工程位移数据Row expB361Row = B361Table.FindBy试验编号(tempNOStr);
                if (expB361Row != null)
                {
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[0][0] = expB361Row.工程变形1aZ00;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[0][1] = expB361Row.工程变形1bZ00;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[0][2] = expB361Row.工程变形1cZ00;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[0][3] = expB361Row.工程变形1dZ00;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[1][0] = expB361Row.工程变形1aZ01;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[1][1] = expB361Row.工程变形1bZ01;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[1][2] = expB361Row.工程变形1cZ01;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[1][3] = expB361Row.工程变形1dZ01;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[2][0] = expB361Row.工程变形1aZ02;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[2][1] = expB361Row.工程变形1bZ02;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[2][2] = expB361Row.工程变形1cZ02;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[2][3] = expB361Row.工程变形1dZ02;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[3][0] = expB361Row.工程变形1aZ03;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[3][1] = expB361Row.工程变形1bZ03;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[3][2] = expB361Row.工程变形1cZ03;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[3][3] = expB361Row.工程变形1dZ03;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[4][0] = expB361Row.工程变形1aZ04;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[4][1] = expB361Row.工程变形1bZ04;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[4][2] = expB361Row.工程变形1cZ04;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[4][3] = expB361Row.工程变形1dZ04;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[0][0] = expB361Row.工程变形1aF00;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[0][1] = expB361Row.工程变形1bF00;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[0][2] = expB361Row.工程变形1cF00;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[0][3] = expB361Row.工程变形1dF00;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[1][0] = expB361Row.工程变形1aF01;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[1][1] = expB361Row.工程变形1bF01;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[1][2] = expB361Row.工程变形1cF01;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[1][3] = expB361Row.工程变形1dF01;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[2][0] = expB361Row.工程变形1aF02;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[2][1] = expB361Row.工程变形1bF02;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[2][2] = expB361Row.工程变形1cF02;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[2][3] = expB361Row.工程变形1dF02;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[3][0] = expB361Row.工程变形1aF03;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[3][1] = expB361Row.工程变形1bF03;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[3][2] = expB361Row.工程变形1cF03;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[3][3] = expB361Row.工程变形1dF03;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[4][0] = expB361Row.工程变形1aF04;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[4][1] = expB361Row.工程变形1bF04;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[4][2] = expB361Row.工程变形1cF04;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[4][3] = expB361Row.工程变形1dF04;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[0][0] = expB361Row.工程P31aZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[0][1] = expB361Row.工程P31bZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[0][2] = expB361Row.工程P31cZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[0][3] = expB361Row.工程P31dZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[1][0] = expB361Row.工程P31aZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[1][1] = expB361Row.工程P31bZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[1][2] = expB361Row.工程P31cZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[1][3] = expB361Row.工程P31dZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[0][0] = expB361Row.工程P31aF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[0][1] = expB361Row.工程P31bF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[0][2] = expB361Row.工程P31cF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[0][3] = expB361Row.工程P31dF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[1][0] = expB361Row.工程P31aF1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[1][1] = expB361Row.工程P31bF1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[1][2] = expB361Row.工程P31cF1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[1][3] = expB361Row.工程P31dF1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_Z[0][0] = expB361Row.工程Pmax1aZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_Z[0][1] = expB361Row.工程Pmax1bZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_Z[0][2] = expB361Row.工程Pmax1cZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_Z[0][3] = expB361Row.工程Pmax1dZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_Z[1][0] = expB361Row.工程Pmax1aZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_Z[1][1] = expB361Row.工程Pmax1bZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_Z[1][2] = expB361Row.工程Pmax1cZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_Z[1][3] = expB361Row.工程Pmax1dZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_F[0][0] = expB361Row.工程Pmax1aF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_F[0][1] = expB361Row.工程Pmax1bF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_F[0][2] = expB361Row.工程Pmax1cF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_F[0][3] = expB361Row.工程Pmax1dF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_F[1][0] = expB361Row.工程Pmax1aF1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_F[1][1] = expB361Row.工程Pmax1bF1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_F[1][2] = expB361Row.工程Pmax1cF1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_F[1][3] = expB361Row.工程Pmax1dF1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B362抗风压组1工程位移数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB362(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B362抗风压组2工程位移数据Row checkExistB362Row = B362Table.FindBy试验编号(tempNOStr);
                if (checkExistB362Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B362抗风压组2工程位移数据Row defExpB362Row = B362Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B362抗风压组2工程位移数据Row newExpB362Row = B362Table.NewB362抗风压组2工程位移数据Row();
                    newExpB362Row.ItemArray = (object[])defExpB362Row.ItemArray.Clone();
                    newExpB362Row.试验编号 = tempNOStr;
                    B362Table.AddB362抗风压组2工程位移数据Row(newExpB362Row);
                    B362TableAdapter.Update(B362Table);
                    B362Table.AcceptChanges();
                    RaisePropertyChanged(() => B362Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B362表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B362抗风压组2工程位移数据Row expB362Row = B362Table.FindBy试验编号(tempNOStr);
                if (expB362Row != null)
                {
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[0][4] = expB362Row.工程变形2aZ00;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[0][5] = expB362Row.工程变形2bZ00;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[0][6] = expB362Row.工程变形2cZ00;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[1][4] = expB362Row.工程变形2aZ01;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[1][5] = expB362Row.工程变形2bZ01;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[1][6] = expB362Row.工程变形2cZ01;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[2][4] = expB362Row.工程变形2aZ02;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[2][5] = expB362Row.工程变形2bZ02;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[2][6] = expB362Row.工程变形2cZ02;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[3][4] = expB362Row.工程变形2aZ03;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[3][5] = expB362Row.工程变形2bZ03;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[3][6] = expB362Row.工程变形2cZ03;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[4][4] = expB362Row.工程变形2aZ04;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[4][5] = expB362Row.工程变形2bZ04;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[4][6] = expB362Row.工程变形2cZ04;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[0][4] = expB362Row.工程变形2aF00;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[0][5] = expB362Row.工程变形2bF00;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[0][6] = expB362Row.工程变形2cF00;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[1][4] = expB362Row.工程变形2aF01;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[1][5] = expB362Row.工程变形2bF01;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[1][6] = expB362Row.工程变形2cF01;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[2][4] = expB362Row.工程变形2aF02;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[2][5] = expB362Row.工程变形2bF02;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[2][6] = expB362Row.工程变形2cF02;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[3][4] = expB362Row.工程变形2aF03;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[3][5] = expB362Row.工程变形2bF03;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[3][6] = expB362Row.工程变形2cF03;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[4][4] = expB362Row.工程变形2aF04;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[4][5] = expB362Row.工程变形2bF04;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[4][6] = expB362Row.工程变形2cF04;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[0][4] = expB362Row.工程P32aZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[0][5] = expB362Row.工程P32bZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[0][6] = expB362Row.工程P32cZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[1][4] = expB362Row.工程P32aZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[1][5] = expB362Row.工程P32bZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[1][6] = expB362Row.工程P32cZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[0][4] = expB362Row.工程P32aF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[0][5] = expB362Row.工程P32bF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[0][6] = expB362Row.工程P32cF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[1][4] = expB362Row.工程P32aF1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[1][5] = expB362Row.工程P32bF1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[1][6] = expB362Row.工程P32cF1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_Z[0][4] = expB362Row.工程Pmax2aZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_Z[0][5] = expB362Row.工程Pmax2bZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_Z[0][6] = expB362Row.工程Pmax2cZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_Z[1][4] = expB362Row.工程Pmax2aZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_Z[1][5] = expB362Row.工程Pmax2bZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_Z[1][6] = expB362Row.工程Pmax2cZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_F[0][4] = expB362Row.工程Pmax2aF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_F[0][5] = expB362Row.工程Pmax2bF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_F[0][6] = expB362Row.工程Pmax2cF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_F[1][4] = expB362Row.工程Pmax2aF1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_F[1][5] = expB362Row.工程Pmax2bF1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_F[1][6] = expB362Row.工程Pmax2cF1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B363抗风压组1工程位移数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB363(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B363抗风压组3工程位移数据Row checkExistB363Row = B363Table.FindBy试验编号(tempNOStr);
                if (checkExistB363Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B363抗风压组3工程位移数据Row defExpB363Row = B363Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B363抗风压组3工程位移数据Row newExpB363Row = B363Table.NewB363抗风压组3工程位移数据Row();
                    newExpB363Row.ItemArray = (object[])defExpB363Row.ItemArray.Clone();
                    newExpB363Row.试验编号 = tempNOStr;
                    B363Table.AddB363抗风压组3工程位移数据Row(newExpB363Row);
                    B363TableAdapter.Update(B363Table);
                    B363Table.AcceptChanges();
                    RaisePropertyChanged(() => B363Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B363表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B363抗风压组3工程位移数据Row expB363Row = B363Table.FindBy试验编号(tempNOStr);
                if (expB363Row != null)
                {
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[0][7] = expB363Row.工程变形3aZ00;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[0][8] = expB363Row.工程变形3bZ00;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[0][9] = expB363Row.工程变形3cZ00;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[1][7] = expB363Row.工程变形3aZ01;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[1][8] = expB363Row.工程变形3bZ01;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[1][9] = expB363Row.工程变形3cZ01;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[2][7] = expB363Row.工程变形3aZ02;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[2][8] = expB363Row.工程变形3bZ02;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[2][9] = expB363Row.工程变形3cZ02;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[3][7] = expB363Row.工程变形3aZ03;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[3][8] = expB363Row.工程变形3bZ03;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[3][9] = expB363Row.工程变形3cZ03;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[4][7] = expB363Row.工程变形3aZ04;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[4][8] = expB363Row.工程变形3bZ04;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_Z[4][9] = expB363Row.工程变形3cZ04;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[0][7] = expB363Row.工程变形3aF00;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[0][8] = expB363Row.工程变形3bF00;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[0][9] = expB363Row.工程变形3cF00;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[1][7] = expB363Row.工程变形3aF01;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[1][8] = expB363Row.工程变形3bF01;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[1][9] = expB363Row.工程变形3cF01;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[2][7] = expB363Row.工程变形3aF02;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[2][8] = expB363Row.工程变形3bF02;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[2][9] = expB363Row.工程变形3cF02;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[3][7] = expB363Row.工程变形3aF03;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[3][8] = expB363Row.工程变形3bF03;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[3][9] = expB363Row.工程变形3cF03;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[4][7] = expB363Row.工程变形3aF04;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[4][8] = expB363Row.工程变形3bF04;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCBX_F[4][9] = expB363Row.工程变形3cF04;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[0][7] = expB363Row.工程P33aZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[0][8] = expB363Row.工程P33bZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[0][9] = expB363Row.工程P33cZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[1][7] = expB363Row.工程P33aZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[1][8] = expB363Row.工程P33bZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_Z[1][9] = expB363Row.工程P33cZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[0][7] = expB363Row.工程P33aF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[0][8] = expB363Row.工程P33bF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[0][9] = expB363Row.工程P33cF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[1][7] = expB363Row.工程P33aF1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[1][8] = expB363Row.工程P33bF1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCP3_F[1][9] = expB363Row.工程P33cF1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_Z[0][7] = expB363Row.工程Pmax3aZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_Z[0][8] = expB363Row.工程Pmax3bZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_Z[0][9] = expB363Row.工程Pmax3cZ0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_Z[1][7] = expB363Row.工程Pmax3aZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_Z[1][8] = expB363Row.工程Pmax3bZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_Z[1][9] = expB363Row.工程Pmax3cZ1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_F[0][7] = expB363Row.工程Pmax3aF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_F[0][8] = expB363Row.工程Pmax3bF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_F[0][9] = expB363Row.工程Pmax3cF0;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_F[1][7] = expB363Row.工程Pmax3aF1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_F[1][8] = expB363Row.工程Pmax3bF1;
                    PublicData.ExpDQ.ExpData_KFY.WY_GCPmax_F[1][9] = expB363Row.工程Pmax3cF1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B371抗风压工程相对挠度数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB371(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B371抗风压工程相对挠度数据Row checkExistB371Row = B371Table.FindBy试验编号(tempNOStr);
                if (checkExistB371Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B371抗风压工程相对挠度数据Row defExpB371Row = B371Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B371抗风压工程相对挠度数据Row newExpB371Row = B371Table.NewB371抗风压工程相对挠度数据Row();
                    newExpB371Row.ItemArray = (object[])defExpB371Row.ItemArray.Clone();
                    newExpB371Row.试验编号 = tempNOStr;
                    B371Table.AddB371抗风压工程相对挠度数据Row(newExpB371Row);
                    B371TableAdapter.Update(B371Table);
                    B371Table.AcceptChanges();
                    RaisePropertyChanged(() => B371Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B371表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B371抗风压工程相对挠度数据Row expB371Row = B371Table.FindBy试验编号(tempNOStr);
                if (expB371Row != null)
                {
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[0][0] = expB371Row.工程变形0相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[1][0] = expB371Row.工程变形10相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[2][0] = expB371Row.工程变形20相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[3][0] = expB371Row.工程变形30相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[4][0] = expB371Row.工程变形40相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[0][0] = expB371Row.工程变形负0相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[1][0] = expB371Row.工程变形负10相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[2][0] = expB371Row.工程变形负20相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[3][0] = expB371Row.工程变形负30相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[4][0] = expB371Row.工程变形负40相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[0][1] = expB371Row.工程变形0相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[1][1] = expB371Row.工程变形10相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[2][1] = expB371Row.工程变形20相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[3][1] = expB371Row.工程变形30相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[4][1] = expB371Row.工程变形40相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[0][1] = expB371Row.工程变形负0相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[1][1] = expB371Row.工程变形负10相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[2][1] = expB371Row.工程变形负20相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[3][1] = expB371Row.工程变形负30相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[4][1] = expB371Row.工程变形负40相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[0][2] = expB371Row.工程变形0相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[1][2] = expB371Row.工程变形10相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[2][2] = expB371Row.工程变形20相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[3][2] = expB371Row.工程变形30相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_Z[4][2] = expB371Row.工程变形40相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[0][2] = expB371Row.工程变形负0相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[1][2] = expB371Row.工程变形负10相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[2][2] = expB371Row.工程变形负20相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[3][2] = expB371Row.工程变形负30相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCBX_F[4][2] = expB371Row.工程变形负40相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_Z[0][0] = expB371Row.工程P3前相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_Z[1][0] = expB371Row.工程P3相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_F[0][0] = expB371Row.工程负P3前相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_F[1][0] = expB371Row.工程负P3相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_Z[0][1] = expB371Row.工程P3前相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_Z[1][1] = expB371Row.工程P3相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_F[0][1] = expB371Row.工程负P3前相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_F[1][1] = expB371Row.工程负P3相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_Z[0][2] = expB371Row.工程P3前相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_Z[1][2] = expB371Row.工程P3相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_F[0][2] = expB371Row.工程负P3前相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCP3_F[1][2] = expB371Row.工程负P3相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCPmax_Z[0][0] = expB371Row.工程Pmax前相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCPmax_Z[1][0] = expB371Row.工程Pmax相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCPmax_F[0][0] = expB371Row.工程负Pmax前相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCPmax_F[1][0] = expB371Row.工程负Pmax相对挠度1;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCPmax_Z[0][1] = expB371Row.工程Pmax前相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCPmax_Z[1][1] = expB371Row.工程Pmax相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCPmax_F[0][1] = expB371Row.工程负Pmax前相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCPmax_F[1][1] = expB371Row.工程负Pmax相对挠度2;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCPmax_Z[0][2] = expB371Row.工程Pmax前相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCPmax_Z[1][2] = expB371Row.工程Pmax相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCPmax_F[0][2] = expB371Row.工程负Pmax前相对挠度3;
                    PublicData.ExpDQ.ExpData_KFY.XDND_GCPmax_F[1][2] = expB371Row.工程负Pmax相对挠度3;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B372抗风压工程相对挠度最大值数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB372(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B372抗风压工程相对挠度最大值数据Row checkExistB372Row = B372Table.FindBy试验编号(tempNOStr);
                if (checkExistB372Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B372抗风压工程相对挠度最大值数据Row defExpB372Row = B372Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B372抗风压工程相对挠度最大值数据Row newExpB372Row = B372Table.NewB372抗风压工程相对挠度最大值数据Row();
                    newExpB372Row.ItemArray = (object[])defExpB372Row.ItemArray.Clone();
                    newExpB372Row.试验编号 = tempNOStr;
                    B372Table.AddB372抗风压工程相对挠度最大值数据Row(newExpB372Row);
                    B372TableAdapter.Update(B372Table);
                    B372Table.AcceptChanges();
                    RaisePropertyChanged(() => B372Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B372表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B372抗风压工程相对挠度最大值数据Row expB372Row = B372Table.FindBy试验编号(tempNOStr);
                if (expB372Row != null)
                {
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_GCBX_Z[0] = expB372Row.工程变形0相对挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_GCBX_Z[1] = expB372Row.工程变形10相对挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_GCBX_Z[2] = expB372Row.工程变形20相对挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_GCBX_Z[3] = expB372Row.工程变形30相对挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_GCBX_Z[4] = expB372Row.工程变形40相对挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_GCBX_F[0] = expB372Row.工程变形负0相对挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_GCBX_F[1] = expB372Row.工程变形负10相对挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_GCBX_F[2] = expB372Row.工程变形负20相对挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_GCBX_F[3] = expB372Row.工程变形负30相对挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_GCBX_F[4] = expB372Row.工程变形负40相对挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_GCp3_Z = expB372Row.工程P3相对挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_GCp3_F = expB372Row.工程负P3相对挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_GCpmax_Z = expB372Row.工程Pmax相对挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.XDND_Max_GCpmax_F = expB372Row.工程负Pmax相对挠度fmax;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B381抗风压工程挠度数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB381(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B381抗风压工程挠度数据Row checkExistB381Row = B381Table.FindBy试验编号(tempNOStr);
                if (checkExistB381Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B381抗风压工程挠度数据Row defExpB381Row = B381Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B381抗风压工程挠度数据Row newExpB381Row = B381Table.NewB381抗风压工程挠度数据Row();
                    newExpB381Row.ItemArray = (object[])defExpB381Row.ItemArray.Clone();
                    newExpB381Row.试验编号 = tempNOStr;
                    B381Table.AddB381抗风压工程挠度数据Row(newExpB381Row);
                    B381TableAdapter.Update(B381Table);
                    B381Table.AcceptChanges();
                    RaisePropertyChanged(() => B381Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B381表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B381抗风压工程挠度数据Row expB381Row = B381Table.FindBy试验编号(tempNOStr);
                if (expB381Row != null)
                {
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[0][0] = expB381Row.工程变形0挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[1][0] = expB381Row.工程变形10挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[2][0] = expB381Row.工程变形20挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[3][0] = expB381Row.工程变形30挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[4][0] = expB381Row.工程变形40挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[0][0] = expB381Row.工程变形负0挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[1][0] = expB381Row.工程变形负10挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[2][0] = expB381Row.工程变形负20挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[3][0] = expB381Row.工程变形负30挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[4][0] = expB381Row.工程变形负40挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[0][1] = expB381Row.工程变形0挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[1][1] = expB381Row.工程变形10挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[2][1] = expB381Row.工程变形20挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[3][1] = expB381Row.工程变形30挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[4][1] = expB381Row.工程变形40挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[0][1] = expB381Row.工程变形负0挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[1][1] = expB381Row.工程变形负10挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[2][1] = expB381Row.工程变形负20挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[3][1] = expB381Row.工程变形负30挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[4][1] = expB381Row.工程变形负40挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[0][2] = expB381Row.工程变形0挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[1][2] = expB381Row.工程变形10挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[2][2] = expB381Row.工程变形20挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[3][2] = expB381Row.工程变形30挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[4][2] = expB381Row.工程变形40挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[0][2] = expB381Row.工程变形负0挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[1][2] = expB381Row.工程变形负10挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[2][2] = expB381Row.工程变形负20挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[3][2] = expB381Row.工程变形负30挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[4][2] = expB381Row.工程变形负40挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCP3_Z[0][0] = expB381Row.工程P3前挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCP3_Z[1][0] = expB381Row.工程P3挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCP3_F[0][0] = expB381Row.工程负P3前挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCP3_F[1][0] = expB381Row.工程负P3挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCP3_Z[0][1] = expB381Row.工程P3前挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCP3_Z[1][1] = expB381Row.工程P3挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCP3_F[0][1] = expB381Row.工程负P3前挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCP3_F[1][1] = expB381Row.工程负P3挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCP3_Z[0][2] = expB381Row.工程P3前挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCP3_Z[1][2] = expB381Row.工程P3挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCP3_F[0][2] = expB381Row.工程负P3前挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCP3_F[1][2] = expB381Row.工程负P3挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCPmax_Z[0][0] = expB381Row.工程Pmax前挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCPmax_Z[1][0] = expB381Row.工程Pmax挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCPmax_F[0][0] = expB381Row.工程负Pmax前挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCPmax_F[1][0] = expB381Row.工程负Pmax挠度1;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCPmax_Z[0][1] = expB381Row.工程Pmax前挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCPmax_Z[1][1] = expB381Row.工程Pmax挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCPmax_F[0][1] = expB381Row.工程负Pmax前挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCPmax_F[1][1] = expB381Row.工程负Pmax挠度2;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCPmax_Z[0][2] = expB381Row.工程Pmax前挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCPmax_Z[1][2] = expB381Row.工程Pmax挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCPmax_F[0][2] = expB381Row.工程负Pmax前挠度3;
                    PublicData.ExpDQ.ExpData_KFY.ND_GCPmax_F[1][2] = expB381Row.工程负Pmax挠度3;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B382抗风压工程挠度最大值数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB382(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B382抗风压工程挠度最大值数据Row checkExistB382Row = B382Table.FindBy试验编号(tempNOStr);
                if (checkExistB382Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B382抗风压工程挠度最大值数据Row defExpB382Row = B382Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B382抗风压工程挠度最大值数据Row newExpB382Row = B382Table.NewB382抗风压工程挠度最大值数据Row();
                    newExpB382Row.ItemArray = (object[])defExpB382Row.ItemArray.Clone();
                    newExpB382Row.试验编号 = tempNOStr;
                    B382Table.AddB382抗风压工程挠度最大值数据Row(newExpB382Row);
                    B382TableAdapter.Update(B382Table);
                    B382Table.AcceptChanges();
                    RaisePropertyChanged(() => B382Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B382表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B382抗风压工程挠度最大值数据Row expB382Row = B382Table.FindBy试验编号(tempNOStr);
                if (expB382Row != null)
                {
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_GCBX_Z[0] = expB382Row.工程变形0挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_GCBX_Z[1] = expB382Row.工程变形10挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_GCBX_Z[2] = expB382Row.工程变形20挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_GCBX_Z[3] = expB382Row.工程变形30挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_GCBX_Z[4] = expB382Row.工程变形40挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_GCBX_F[0] = expB382Row.工程变形负0挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_GCBX_F[1] = expB382Row.工程变形负10挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_GCBX_F[2] = expB382Row.工程变形负20挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_GCBX_F[3] = expB382Row.工程变形负30挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_GCBX_F[4] = expB382Row.工程变形负40挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_GCp3_Z = expB382Row.工程P3挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_GCp3_F = expB382Row.工程负P3挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_GCpmax_Z = expB382Row.工程Pmax挠度fmax;
                    PublicData.ExpDQ.ExpData_KFY.ND_Max_GCpmax_F = expB382Row.工程负Pmax挠度fmax;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入B390抗风压工程损坏数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadB390(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //不存在则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B390抗风压工程损坏数据Row checkExistB390Row = B390Table.FindBy试验编号(tempNOStr);
                if (checkExistB390Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B390抗风压工程损坏数据Row defExpB390Row = B390Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B390抗风压工程损坏数据Row newExpB390Row = B390Table.NewB390抗风压工程损坏数据Row();
                    newExpB390Row.ItemArray = (object[])defExpB390Row.ItemArray.Clone();
                    newExpB390Row.试验编号 = tempNOStr;
                    B390Table.AddB390抗风压工程损坏数据Row(newExpB390Row);
                    B390TableAdapter.Update(B390Table);
                    B390Table.AcceptChanges();
                    RaisePropertyChanged(() => B390Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B390表的信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B390抗风压工程损坏数据Row expB390Row = B390Table.FindBy试验编号(tempNOStr);
                if (expB390Row != null)
                {
                    PublicData.ExpDQ.ExpData_KFY.Damage_GCBX_Z[0] = expB390Row.工程变形0损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_GCBX_Z[1] = expB390Row.工程变形10损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_GCBX_Z[2] = expB390Row.工程变形20损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_GCBX_Z[3] = expB390Row.工程变形30损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_GCBX_Z[4] = expB390Row.工程变形40损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_GCBX_F[0] = expB390Row.工程变形负0损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_GCBX_F[1] = expB390Row.工程变形负10损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_GCBX_F[2] = expB390Row.工程变形负20损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_GCBX_F[3] = expB390Row.工程变形负30损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_GCBX_F[4] = expB390Row.工程变形负40损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_GCP2_Z = expB390Row.工程P2损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_GCP2_F = expB390Row.工程负P2损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_GCP3_Z = expB390Row.工程P3损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_GCP3_F = expB390Row.工程负P3损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_GCPmax_Z = expB390Row.工程Pmax损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.Damage_GCPmax_F = expB390Row.工程负Pmax损坏情况;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_GCBX_Z[0] = expB390Row.工程变形0损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_GCBX_Z[1] = expB390Row.工程变形10损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_GCBX_Z[2] = expB390Row.工程变形20损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_GCBX_Z[3] = expB390Row.工程变形30损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_GCBX_Z[4] = expB390Row.工程变形40损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_GCBX_F[0] = expB390Row.工程变形负0损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_GCBX_F[1] = expB390Row.工程变形负10损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_GCBX_F[2] = expB390Row.工程变形负20损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_GCBX_F[3] = expB390Row.工程变形负30损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_GCBX_F[4] = expB390Row.工程变形负40损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_GCP2_Z = expB390Row.工程P2损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_GCP2_F = expB390Row.工程负P2损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_GCP3_Z = expB390Row.工程P3损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_GCP3_F = expB390Row.工程负P3损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_GCPmax_Z = expB390Row.工程Pmax损坏说明;
                    PublicData.ExpDQ.ExpData_KFY.DamagePS_GCPmax_F = expB390Row.工程负Pmax损坏说明;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 按名称载入试验B4层间变形试验数据
        /// </summary>
        /// <param name="givenExpName"></param>
        private void LoadDataCJBX(string givenExpName)
        {
            string tempNOStr = givenExpName;

            //载入B4层间变形试验表数据
            //若编号在表中不存在，则新建
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B4层间变形试验数据Row checkExistB4Row = B4Table.FindBy试验编号(tempNOStr);
                if (checkExistB4Row == null)
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B4层间变形试验数据Row defDataB4Row = B4Table.FindBy试验编号("DefaultExp");
                    MQDFJ_MB.MQZH_DB_TestDataSet.B4层间变形试验数据Row newDataB4Row = B4Table.NewB4层间变形试验数据Row();
                    newDataB4Row.ItemArray = (object[])defDataB4Row.ItemArray.Clone();
                    newDataB4Row.试验编号 = tempNOStr;
                    B4Table.AddB4层间变形试验数据Row(newDataB4Row);
                    B4TableAdapter.Update(B4Table);
                    B4Table.AcceptChanges();
                    RaisePropertyChanged(() => B4Table);
                    MessageBox.Show("未找到编号为" + tempNOStr + "B4的试验数据，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B4层间变形试验数据Row expB4Row = B4Table.FindBy试验编号(tempNOStr);
                if (expB4Row != null)
                {
                    //定级X轴检测数据
                    PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_X[0] = expB4Row.定级X轴预加载位移角;
                    PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_X[1] = expB4Row.定级X轴第1级位移角;
                    PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_X[2] = expB4Row.定级X轴第2级位移角;
                    PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_X[3] = expB4Row.定级X轴第3级位移角;
                    PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_X[4] = expB4Row.定级X轴第4级位移角;
                    PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_X[5] = expB4Row.定级X轴第5级位移角;
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_X[0] = expB4Row.定级X轴预加载位移量;
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_X[1] = expB4Row.定级X轴第1级位移量;
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_X[2] = expB4Row.定级X轴第2级位移量;
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_X[3] = expB4Row.定级X轴第3级位移量;
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_X[4] = expB4Row.定级X轴第4级位移量;
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_X[5] = expB4Row.定级X轴第5级位移量;
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_X[0] = expB4Row.定级X轴预加载是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_X[1] = expB4Row.定级X轴第1级是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_X[2] = expB4Row.定级X轴第2级是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_X[3] = expB4Row.定级X轴第3级是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_X[4] = expB4Row.定级X轴第4级是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_X[5] = expB4Row.定级X轴第5级是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_DJ_X[0] = expB4Row.定级X轴预加载损坏说明;
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_DJ_X[1] = expB4Row.定级X轴第1级损坏说明;
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_DJ_X[2] = expB4Row.定级X轴第2级损坏说明;
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_DJ_X[3] = expB4Row.定级X轴第3级损坏说明;
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_DJ_X[4] = expB4Row.定级X轴第4级损坏说明;
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_DJ_X[5] = expB4Row.定级X轴第5级损坏说明;
                    //定级Y轴检测数据
                    PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_Y[0] = expB4Row.定级Y轴预加载位移角;
                    PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_Y[1] = expB4Row.定级Y轴第1级位移角;
                    PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_Y[2] = expB4Row.定级Y轴第2级位移角;
                    PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_Y[3] = expB4Row.定级Y轴第3级位移角;
                    PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_Y[4] = expB4Row.定级Y轴第4级位移角;
                    PublicData.ExpDQ.ExpData_CJBX.AngleFM_DJ_Y[5] = expB4Row.定级Y轴第5级位移角;
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Y[0] = expB4Row.定级Y轴预加载位移量;
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Y[1] = expB4Row.定级Y轴第1级位移量;
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Y[2] = expB4Row.定级Y轴第2级位移量;
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Y[3] = expB4Row.定级Y轴第3级位移量;
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Y[4] = expB4Row.定级Y轴第4级位移量;
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Y[5] = expB4Row.定级Y轴第5级位移量;
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Y[0] = expB4Row.定级Y轴预加载是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Y[1] = expB4Row.定级Y轴第1级是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Y[2] = expB4Row.定级Y轴第2级是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Y[3] = expB4Row.定级Y轴第3级是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Y[4] = expB4Row.定级Y轴第4级是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Y[5] = expB4Row.定级Y轴第5级是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_DJ_Y[0] = expB4Row.定级Y轴预加载损坏说明;
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_DJ_Y[1] = expB4Row.定级Y轴第1级损坏说明;
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_DJ_Y[2] = expB4Row.定级Y轴第2级损坏说明;
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_DJ_Y[3] = expB4Row.定级Y轴第3级损坏说明;
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_DJ_Y[4] = expB4Row.定级Y轴第4级损坏说明;
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_DJ_Y[5] = expB4Row.定级Y轴第5级损坏说明;
                    //定级Z轴检测数据
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Z[0] = expB4Row.定级Z轴预加载位移量;
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Z[1] = expB4Row.定级Z轴第1级位移量;
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Z[2] = expB4Row.定级Z轴第2级位移量;
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Z[3] = expB4Row.定级Z轴第3级位移量;
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Z[4] = expB4Row.定级Z轴第4级位移量;
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_DJ_Z[5] = expB4Row.定级Z轴第5级位移量;
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Z[0] = expB4Row.定级Z轴预加载是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Z[1] = expB4Row.定级Z轴第1级是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Z[2] = expB4Row.定级Z轴第2级是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Z[3] = expB4Row.定级Z轴第3级是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Z[4] = expB4Row.定级Z轴第4级是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_DJ_Z[5] = expB4Row.定级Z轴第5级是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_DJ_Z[0] = expB4Row.定级Z轴预加载损坏说明;
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_DJ_Z[1] = expB4Row.定级Z轴第1级损坏说明;
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_DJ_Z[2] = expB4Row.定级Z轴第2级损坏说明;
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_DJ_Z[3] = expB4Row.定级Z轴第3级损坏说明;
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_DJ_Z[4] = expB4Row.定级Z轴第4级损坏说明;
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_DJ_Z[5] = expB4Row.定级Z轴第5级损坏说明;
                    //定级评定数据
                    PublicData.ExpDQ.ExpData_CJBX.MaxAngleFM_DJ_X = expB4Row.定级X轴最大位移角;
                    PublicData.ExpDQ.ExpData_CJBX.MaxAngleFM_DJ_Y = expB4Row.定级Y轴最大位移角;
                    PublicData.ExpDQ.ExpData_CJBX.MaxDSPL_DJ_Z = expB4Row.定级Z轴最大位移量;
                    PublicData.ExpDQ.ExpData_CJBX.MaxDSPL_DJ_X = expB4Row.定级X轴最大位移量;
                    PublicData.ExpDQ.ExpData_CJBX.MaxDSPL_DJ_Y = expB4Row.定级Y轴最大位移量;
                    PublicData.ExpDQ.ExpData_CJBX.Level_DJ_X = expB4Row.定级X轴评定等级;
                    PublicData.ExpDQ.ExpData_CJBX.Level_DJ_Y = expB4Row.定级Y轴评定等级;
                    PublicData.ExpDQ.ExpData_CJBX.Level_DJ_Z = expB4Row.定级Z轴评定等级;
                    PublicData.ExpDQ.ExpData_CJBX.IsDamageFinal_DJ_X = expB4Row.定级X轴检测最终是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.IsDamageFinal_DJ_Y = expB4Row.定级Y轴检测最终是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.IsDamageFinal_DJ_Z = expB4Row.定级Z轴检测最终是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.DamageFinalPS_DJ_X = expB4Row.定级X轴检测最终损坏说明;
                    PublicData.ExpDQ.ExpData_CJBX.DamageFinalPS_DJ_Y = expB4Row.定级Y轴检测最终损坏说明;
                    PublicData.ExpDQ.ExpData_CJBX.DamageFinalPS_DJ_Z = expB4Row.定级Z轴检测最终损坏说明;

                    //工程X轴检测数据
                    PublicData.ExpDQ.ExpData_CJBX.AngleFM_GC_X = expB4Row.工程X轴检测位移角;
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_GC_X = expB4Row.工程X轴检测位移量;
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_GC_X = expB4Row.工程X轴检测是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_GC_X = expB4Row.工程X轴检测损坏说明;
                    //工程Y轴检测数据
                    PublicData.ExpDQ.ExpData_CJBX.AngleFM_GC_Y = expB4Row.工程Y轴检测位移角;
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_GC_Y = expB4Row.工程Y轴检测位移量;
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_GC_Y = expB4Row.工程Y轴检测是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_GC_Y = expB4Row.工程Y轴检测损坏说明;
                    //工程Z轴检测数据
                    PublicData.ExpDQ.ExpData_CJBX.DSPL_GC_Z = expB4Row.工程Z轴检测位移量;
                    PublicData.ExpDQ.ExpData_CJBX.IsDamage_GC_Z = expB4Row.工程Z轴检测是否损坏 == "有损坏";
                    PublicData.ExpDQ.ExpData_CJBX.DamagePS_GC_Z = expB4Row.工程Z轴检测损坏说明;
                    //工程评定数据
                    PublicData.ExpDQ.ExpData_CJBX.IsMeetDesign_GC_All = expB4Row.工程总体是否满足设计要求 == "满足";
                    PublicData.ExpDQ.ExpData_CJBX.IsMeetDesign_GC_X = expB4Row.工程X轴是否满足设计要求 == "满足";
                    PublicData.ExpDQ.ExpData_CJBX.IsMeetDesign_GC_Y = expB4Row.工程Y轴是否满足设计要求 == "满足";
                    PublicData.ExpDQ.ExpData_CJBX.IsMeetDesign_GC_Z = expB4Row.工程Z轴是否满足设计要求 == "满足";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        #endregion


        #region 试验管理、操作消息回调

        /// <summary>
        ///根据编号载入试验消息回调
        /// </summary>
        /// <param name="msg">试验编号</param>
        private void LoadExpMessage(string msg)
        {
            if (LoadExpByName(msg))
            {
                //当前试验已更换
                Messenger.Default.Send<string>(PublicData.ExpDQ.ExpSettingParam.ExpNO.Clone().ToString(), "PublicData.ExpDQChanged");
                //打开试验设定窗口、关闭管理窗口
                Messenger.Default.Send<string>(MQZH_WinName.ExpSetWinName, "OpenGivenNameWin");
                Messenger.Default.Send<string>(MQZH_WinName.ExpManWinName, "CloseGivenNameWin");
            }
        }


        /// <summary>
        ///根据指令存储数据消息回调
        /// </summary>
        /// <param name="msg"></param>
        private void LoadDQExpDataMessage(string msg)
        {
            string tempNOStr = PublicData.ExpDQ.ExpSettingParam.ExpNO;
            //全部数据-------------------------------
            //载入全部试验参数、进度和数据
            if (msg == "LoadExpAll")
            {
                LoadExpByName(tempNOStr);
            }
            //分项进度和数据---------------------------
            //载入气密进度和数据
            else if (msg == "LoadQMStatusAndData")
            {
                LoadExpSettings(tempNOStr);
                LoadStatusQM(tempNOStr);
                LoadDataQM(tempNOStr);
            }
            //载入水密数据和进度
            else if (msg == "LoadSMStatusAndData")
            {
                LoadExpSettings(tempNOStr);
                LoadStatusSM(tempNOStr);
                LoadDataSM(tempNOStr);
            }
            //载入抗风压数据和进度
            else if (msg == "LoadKFYStatusAndData")
            {
                LoadExpSettings(tempNOStr);
                LoadStatusKFY(tempNOStr);
                LoadDataKFY(tempNOStr);
            }
            //载入层间变形数据和进度
            else if (msg == "LoadCJBXStatusAndData")
            {
                LoadExpSettings(tempNOStr);
                LoadStatusCJBX(tempNOStr);
                LoadDataCJBX(tempNOStr);
            }
            //单项进度-------------------------------
            //载入气密进度
            else if (msg == "LoadQMStatus")
            {
                LoadExpSettings(tempNOStr);
                LoadStatusQM(tempNOStr);
            }
            //载入水密进度
            else if (msg == "LoadSMStatus")
            {
                LoadExpSettings(tempNOStr);
                LoadStatusSM(tempNOStr);
            }
            //载入抗风压进度
            else if (msg == "LoadKFYStatus")
            {
                LoadExpSettings(tempNOStr);
                LoadStatusKFY(tempNOStr);
            }
            //载入层间变形进度
            else if (msg == "LoadCJBXStatus")
            {
                LoadExpSettings(tempNOStr);
                LoadStatusCJBX(tempNOStr);
            }

            //单项数据-----------------------------------
            //载入气密数据
            else if (msg == "LoadQMData")
            {
                LoadDataQM(tempNOStr);
            }
            //载入水密数据
            else if (msg == "LoadSMData")
            {
                LoadDataSM(tempNOStr);
            }
            //载入抗风压数据
            else if (msg == "LoadKFYData")
            {
                LoadDataKFY(tempNOStr);
            }
            //载入层间变形数据
            else if (msg == "LoadCJBXData")
            {
                LoadDataCJBX(tempNOStr);
            }
        }

        #endregion
    }
}
