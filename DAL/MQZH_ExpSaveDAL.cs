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
using System.Windows;
using GalaSoft.MvvmLight;

namespace MQDFJ_MB.DAL
{
    /// <summary>
    /// 试验参数及数据读写操作类
    /// </summary>
    public partial class MQZH_ExpDAL : ObservableObject
    {

        #region 保存试验至数据库

        /// <summary>
        /// 保存当前试验至数据库
        /// </summary>
        private void SaveExpDQ()
        {
            SaveExpSettingsDQ();

            SaveStatusQM();
            SaveStatusSM();
            SaveStatusKFY();
            SaveStatusCJBX();

            SaveDataQM();
            SaveDataSM();
            SaveDataKFY();
            SaveDataCJBX();
        }


        /// <summary>
        /// 保存当前试验配置信息至数据库
        /// </summary>
        private void SaveExpSettingsDQ()
        {
            string tempNOStr = DAL_ExpDQ.ExpSettingParam.ExpNO;

            //保存试验设置信息至A00主表
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
                    expA00Row.试验编号 = DAL_ExpDQ.ExpSettingParam.ExpNO;
                    expA00Row.试验补充说明 = DAL_ExpDQ.ExpSettingParam.ExpDetail;
                    expA00Row.三性报告编号 = DAL_ExpDQ.ExpSettingParam.RepSXNO;
                    expA00Row.层间变形报告编号 = DAL_ExpDQ.ExpSettingParam.RepCJBXNO;
                    expA00Row.试件数量 = DAL_ExpDQ.SpecimenNum;
                    expA00Row.检测日期 = DAL_ExpDQ.ExpSettingParam.ExpDate;
                    expA00Row.委托单位 = DAL_ExpDQ.ExpSettingParam.ExpWTDW;
                    expA00Row.委托日期 = DAL_ExpDQ.ExpSettingParam.ExpWTDate;
                    expA00Row.生产单位 = DAL_ExpDQ.ExpSettingParam.ExpSCDW;
                    expA00Row.施工单位 = DAL_ExpDQ.ExpSettingParam.ExpSGDW;
                    expA00Row.工程名称 = DAL_ExpDQ.ExpSettingParam.ExpGCMC;
                    //试件信息
                    expA00Row.试件名称 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_Name;
                    expA00Row.样品编号 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_YPNO;
                    expA00Row.试件类型 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_Type;
                    expA00Row.试件系列 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_Series;
                    expA00Row.试件型号 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_Model;
                    expA00Row.型材品种 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_XCPZ;
                    //expA00Row.型材材质 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_XCCZ;
                    expA00Row.型材牌号 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_XCPH;
                    expA00Row.型材规格 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_XCGG;
                    expA00Row.附件品种 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_FJPZ;
                    expA00Row.附件材质 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_FJCZ;
                   // expA00Row.附件牌号 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_FJPH;
                    expA00Row.面板材料品种 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_MBPZ;
                    expA00Row.面板材料材质 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_MBCZ;
                    expA00Row.面板牌号 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_MBPH;
                    expA00Row.面板厚度 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_MBHD;
                    expA00Row.面板最大尺寸 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_MBZDCC;
                    expA00Row.面板安装方法 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_MBAZFF;
                    expA00Row.密封材料品种 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_MFCLPZ;
                    expA00Row.密封材料材质 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_MFCLCZ;
                    expA00Row.密封材料牌号 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_MFCLPH;
                    expA00Row.镶嵌材料品种 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_XQCLPZ;
                    expA00Row.镶嵌材料材质 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_XQCLCZ;
                    expA00Row.镶嵌材料牌号 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_XQPH;
                    expA00Row.镶嵌尺寸 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_XQCC;
                    expA00Row.镶嵌方法 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_XQFF;
                    expA00Row.最大分格尺寸 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_ZDFGCC;

                    //试件参数
                    expA00Row.试件宽度 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_Width;
                    expA00Row.试件高度 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_Heigth;
                    expA00Row.试件面积 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_Aria;
                    expA00Row.开启缝长度 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_KQFLenth;
                    expA00Row.开启部分面积 = DAL_ExpDQ.ExpSettingParam.Exp_SJ_KKQAria;
                    expA00Row.层高 = DAL_ExpDQ.ExpSettingParam.SJ_CG;
                    expA00Row.有可开启部分 = DAL_ExpDQ.ExpSettingParam.Isexp_SJ_WithKKQ;


                    //实验室环境参数
                    expA00Row.大气压力 = DAL_ExpDQ.ExpRoomPress;
                    expA00Row.室温 = DAL_ExpDQ.ExpRoomT;

                    //试验完成状态
                    expA00Row.试验总体完成状态 = DAL_ExpDQ.ExpCompleteStatus;

                    //气密项目参数
                    expA00Row.气密选中 = DAL_ExpDQ.Exp_QM.NeedTest;
                    expA00Row.气密定级或工程 = DAL_ExpDQ.Exp_QM.IsGC;
                    expA00Row.气密检测项目完成状态 = DAL_ExpDQ.Exp_QM.CompleteStatus;

                    //水密项目参数
                    expA00Row.水密选中 = DAL_ExpDQ.Exp_SM.NeedTest;
                    expA00Row.水密定级或工程 = DAL_ExpDQ.Exp_SM.IsGC;
                    expA00Row.水密检测项目完成状态 = DAL_ExpDQ.Exp_SM.CompleteStatus;

                    //抗风压项目参数
                    expA00Row.抗风压选中 = DAL_ExpDQ.Exp_KFY.NeedTest;
                    expA00Row.抗风压定级或工程 = DAL_ExpDQ.Exp_KFY.IsGC;
                    expA00Row.抗风压检测项目完成状态 = DAL_ExpDQ.Exp_KFY.CompleteStatus;

                    //层间变形项目参数
                    expA00Row.层间变形选中 = DAL_ExpDQ.Exp_CJBX.NeedTest;
                    expA00Row.层间变形定级或工程 = DAL_ExpDQ.Exp_CJBX.IsGC;
                    expA00Row.层间变形检测项目完成状态 = DAL_ExpDQ.Exp_CJBX.CompleteStatus;

                    A00TableAdapter.Update(A00Table);
                    A00Table.AcceptChanges();
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
                    //试验基本信息
                    expA00Row2.试验编号 = DAL_ExpDQ.ExpSettingParam.ExpNO;

                    //气密项目参数
                    expA00Row2.工程气密检测压力设计值 = DAL_ExpDQ.Exp_QM.QM_GCSJP;
                    expA00Row2.工程气密预加压压力 = DAL_ExpDQ.Exp_QM.QM_GCYJYP;
                    expA00Row2.工程气密开启缝长渗透量设计值 = DAL_ExpDQ.Exp_QM.QM_SJSTL_DWKQFC;
                    expA00Row2.工程气密单位面积渗透量设计值 = DAL_ExpDQ.Exp_QM.QM_SJSTL_DWMJ;

                    //水密项目参数
                    expA00Row2.水密加压类型 = DAL_ExpDQ.Exp_SM.WaveType_SM;
                    expA00Row2.水密工程可开启设定值 = DAL_ExpDQ.Exp_SM.SM_GCSJ_KKQ;
                    expA00Row2.水密工程固定设计值 = DAL_ExpDQ.Exp_SM.SM_GCSJ_GD;
                    expA00Row2.箱口宽度 = DAL_ExpDQ.Exp_SM.Width_XK;
                    expA00Row2.箱口高度 = DAL_ExpDQ.Exp_SM.Height_XK;
                    expA00Row2.单位面积淋水量 = DAL_ExpDQ.Exp_SM.SM_SLL_Per_m2;

                    //抗风压项目参数
                    expA00Row2.抗风压工程风载荷标准值 = DAL_ExpDQ.Exp_KFY.P3_GCSJ;
                    expA00Row2.抗风压测点组1使用 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[0].Is_Use;
                    expA00Row2.抗风压测点组2使用 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[1].Is_Use;
                    expA00Row2.抗风压测点组3使用 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[2].Is_Use;
                    expA00Row2.抗风压测点组1测面板 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[0].Is_TestMBBX;
                    expA00Row2.抗风压测点组2测面板 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[1].Is_TestMBBX;
                    expA00Row2.抗风压测点组3测面板 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[2].Is_TestMBBX;
                    expA00Row2.抗风压测点组1测三角面板 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[0].IsBZCSJMB;
                    expA00Row2.抗风压测点组1间距 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[0].L;
                    expA00Row2.抗风压测点组2间距 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[1].L;
                    expA00Row2.抗风压测点组3间距 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[2].L;
                    expA00Row2.抗风压构件1允许相对面法线挠度 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[0].ND_XD_YXFM;
                    expA00Row2.抗风压构件2允许相对面法线挠度 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[1].ND_XD_YXFM;
                    expA00Row2.抗风压构件3允许相对面法线挠度 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[2].ND_XD_YXFM;
                    expA00Row2.抗风压测点组1a位移尺编号 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[0].WYC_No[0];
                    expA00Row2.抗风压测点组1b位移尺编号 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[0].WYC_No[1];
                    expA00Row2.抗风压测点组1c位移尺编号 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[0].WYC_No[2];
                    expA00Row2.抗风压测点组1d位移尺编号 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[0].WYC_No[3];
                    expA00Row2.抗风压测点组2a位移尺编号 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[1].WYC_No[0];
                    expA00Row2.抗风压测点组2b位移尺编号 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[1].WYC_No[1];
                    expA00Row2.抗风压测点组2c位移尺编号 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[1].WYC_No[2];
                    expA00Row2.抗风压测点组3a位移尺编号 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[2].WYC_No[0];
                    expA00Row2.抗风压测点组3b位移尺编号 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[2].WYC_No[1];
                    expA00Row2.抗风压测点组3c位移尺编号 = DAL_ExpDQ.Exp_KFY.DisplaceGroups[2].WYC_No[2];
                    // expA00Row2.抗风压P3压力 = DAL_ExpDQ.Exp_KFY.P3Press;
                    // expA00Row2.抗风压P3降低压力 = DAL_ExpDQ.Exp_KFY.P3DownPress;

                    //层间变形项目参数
                    expA00Row2.工程检测X轴位移角设计值 = DAL_ExpDQ.Exp_CJBX.CJBX_SJZBX;
                    expA00Row2.工程检测Y轴位移角设计值 = DAL_ExpDQ.Exp_CJBX.CJBX_SJZBY;
                    expA00Row2.工程检测Z轴位移量设计值 = DAL_ExpDQ.Exp_CJBX.CJBX_SJZBZ;

                    //  X、Y轴位移系数（1或2）
                    //  DAL_ExpDQ.Exp_CJBX.XishuXY

                    A00TableAdapter.Update(A00Table);
                    A00Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 保存当前气密试验进度至数据库
        /// </summary>
        private void SaveStatusQM()
        {
            string tempNOStr = DAL_ExpDQ.ExpSettingParam.ExpNO;

            //保存试验设置信息至A01表
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
                    expA01Row.定级气密附加正压预加压完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMDJ[0].CompleteStatus;
                    expA01Row.定级气密附加正压检测完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMDJ[1].CompleteStatus;
                    expA01Row.定级气密附加负压预加压完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMDJ[2].CompleteStatus;
                    expA01Row.定级气密附加负压检测完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMDJ[3].CompleteStatus;
                    expA01Row.定级气密附加固定正压预加压完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMDJ[4].CompleteStatus;
                    expA01Row.定级气密附加固定正压检测完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMDJ[5].CompleteStatus;
                    expA01Row.定级气密附加固定负压预加压完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMDJ[6].CompleteStatus;
                    expA01Row.定级气密附加固定负压检测完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMDJ[7].CompleteStatus;
                    expA01Row.定级气密总渗透正压预加压完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMDJ[8].CompleteStatus;
                    expA01Row.定级气密总渗透正压检测完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMDJ[9].CompleteStatus;
                    expA01Row.定级气密总渗透负压预加压完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMDJ[10].CompleteStatus;
                    expA01Row.定级气密总渗透负压检测完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMDJ[11].CompleteStatus;
                    //工程进度;
                    expA01Row.工程气密附加正压预加压完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMGC[0].CompleteStatus;
                    expA01Row.工程气密附加正压检测完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMGC[1].CompleteStatus;
                    expA01Row.工程气密附加负压预加压完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMGC[2].CompleteStatus;
                    expA01Row.工程气密附加负压检测完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMGC[3].CompleteStatus;
                    expA01Row.工程气密附加固定正压预加压完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMGC[4].CompleteStatus;
                    expA01Row.工程气密定级附加固定正压检测完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMGC[5].CompleteStatus;
                    expA01Row.工程气密附加固定负压预加压完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMGC[6].CompleteStatus;
                    expA01Row.工程气密附加固定负压检测完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMGC[7].CompleteStatus;
                    expA01Row.工程气密总渗透正压预加压完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMGC[8].CompleteStatus;
                    expA01Row.工程气密总渗透正压检测完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMGC[9].CompleteStatus;
                    expA01Row.工程气密总渗透负压预加压完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMGC[10].CompleteStatus;
                    expA01Row.工程气密总渗透负压检测完成状态 = DAL_ExpDQ.Exp_QM.StageList_QMGC[11].CompleteStatus;

                    A01TableAdapter.Update(A01Table);
                    A01Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 保存当前水密试验进度至数据库
        /// </summary>
        private void SaveStatusSM()
        {
            string tempNOStr = DAL_ExpDQ.ExpSettingParam.ExpNO;

            //保存试验设置信息至A02水密进度表
            //若编号在表中不存在，则新建
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
                    expA02Row.定级水密预加压完成状态 = DAL_ExpDQ.Exp_SM.StageList_SMDJ[0].CompleteStatus;
                    expA02Row.定级水密加压检测完成状态 = DAL_ExpDQ.Exp_SM.StageList_SMDJ[1].CompleteStatus;
                    expA02Row.工程水密预加压完成状态 = DAL_ExpDQ.Exp_SM.StageList_SMGC[0].CompleteStatus;
                    expA02Row.工程水密加压检测完成状态 = DAL_ExpDQ.Exp_SM.StageList_SMGC[1].CompleteStatus;

                    A02TableAdapter.Update(A02Table);
                    A02Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 保存当前抗风压试验进度数据至数据库
        /// </summary>
        private void SaveStatusKFY()
        {
            string tempNOStr = DAL_ExpDQ.ExpSettingParam.ExpNO;

            // 保存试验设置信息至A03抗风压试验进度表
            //若编号在表中不存在，则新建
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
                    expA03Row.定级p1正压预加压完成状态 = DAL_ExpDQ.Exp_KFY.StageList_KFYDJ[0].CompleteStatus;
                    expA03Row.定级p1正压检测加压完成状态 = DAL_ExpDQ.Exp_KFY.StageList_KFYDJ[1].CompleteStatus;
                    expA03Row.定级p1负压预加压完成状态 = DAL_ExpDQ.Exp_KFY.StageList_KFYDJ[2].CompleteStatus;
                    expA03Row.定级p1负压检测加压完成状态 = DAL_ExpDQ.Exp_KFY.StageList_KFYDJ[3].CompleteStatus;
                    expA03Row.定级p2正压检测加压完成状态 = DAL_ExpDQ.Exp_KFY.StageList_KFYDJ[4].CompleteStatus;
                    expA03Row.定级p2负压检测加压完成状态 = DAL_ExpDQ.Exp_KFY.StageList_KFYDJ[5].CompleteStatus;
                    expA03Row.定级p3正压检测加压完成状态 = DAL_ExpDQ.Exp_KFY.StageList_KFYDJ[6].CompleteStatus;
                    expA03Row.定级p3负压检测加压完成状态 = DAL_ExpDQ.Exp_KFY.StageList_KFYDJ[7].CompleteStatus;
                    expA03Row.定级pmax正压检测加压完成状态 = DAL_ExpDQ.Exp_KFY.StageList_KFYDJ[8].CompleteStatus;
                    expA03Row.定级pmax负压检测加压完成状态 = DAL_ExpDQ.Exp_KFY.StageList_KFYDJ[9].CompleteStatus;
                    expA03Row.工程p1正压预加压完成状态 = DAL_ExpDQ.Exp_KFY.StageList_KFYGC[0].CompleteStatus;
                    expA03Row.工程p1正压检测加压完成状态 = DAL_ExpDQ.Exp_KFY.StageList_KFYGC[1].CompleteStatus;
                    expA03Row.工程p1负压预加压完成状态 = DAL_ExpDQ.Exp_KFY.StageList_KFYGC[2].CompleteStatus;
                    expA03Row.工程p1负压检测加压完成状态 = DAL_ExpDQ.Exp_KFY.StageList_KFYGC[3].CompleteStatus;
                    expA03Row.工程p2正压检测加压完成状态 = DAL_ExpDQ.Exp_KFY.StageList_KFYGC[4].CompleteStatus;
                    expA03Row.工程p2负压检测加压完成状态 = DAL_ExpDQ.Exp_KFY.StageList_KFYGC[5].CompleteStatus;
                    expA03Row.工程p3正压检测加压完成状态 = DAL_ExpDQ.Exp_KFY.StageList_KFYGC[6].CompleteStatus;
                    expA03Row.工程p3负压检测加压完成状态 = DAL_ExpDQ.Exp_KFY.StageList_KFYGC[7].CompleteStatus;
                    expA03Row.工程pmax正压检测加压完成状态 = DAL_ExpDQ.Exp_KFY.StageList_KFYGC[8].CompleteStatus;
                    expA03Row.工程pmax负压检测加压完成状态 = DAL_ExpDQ.Exp_KFY.StageList_KFYGC[9].CompleteStatus;

                    A03TableAdapter.Update(A03Table);
                    A03Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 保存当前层间变形试验进度数据至数据库
        /// </summary>
        private void SaveStatusCJBX()
        {
            string tempNOStr = DAL_ExpDQ.ExpSettingParam.ExpNO;

            //A04层间变形进度表数据
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
                    MessageBox.Show("未找到编号为" + tempNOStr + "的试验进度，已重新建立！", "错误提示");
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
                    expA04Row.定级X轴预加载完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_DJX[0].CompleteStatus;
                    expA04Row.定级X轴第1级完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_DJX[1].CompleteStatus;
                    expA04Row.定级X轴第2级完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_DJX[2].CompleteStatus;
                    expA04Row.定级X轴第3级完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_DJX[3].CompleteStatus;
                    expA04Row.定级X轴第4级完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_DJX[4].CompleteStatus;
                    expA04Row.定级X轴第5级完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_DJX[5].CompleteStatus;
                    //定级检测进度Y轴
                    expA04Row.定级Y轴预加载完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_DJY[0].CompleteStatus;
                    expA04Row.定级Y轴第1级完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_DJY[1].CompleteStatus;
                    expA04Row.定级Y轴第2级完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_DJY[2].CompleteStatus;
                    expA04Row.定级Y轴第3级完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_DJY[3].CompleteStatus;
                    expA04Row.定级Y轴第4级完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_DJY[4].CompleteStatus;
                    expA04Row.定级Y轴第5级完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_DJY[5].CompleteStatus;
                    //定级检测进度Z轴
                    expA04Row.定级Z轴预加载完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_DJZ[0].CompleteStatus;
                    expA04Row.定级Z轴第1级完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_DJZ[1].CompleteStatus;
                    expA04Row.定级Z轴第2级完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_DJZ[2].CompleteStatus;
                    expA04Row.定级Z轴第3级完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_DJZ[3].CompleteStatus;
                    expA04Row.定级Z轴第4级完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_DJZ[4].CompleteStatus;
                    expA04Row.定级Z轴第5级完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_DJZ[5].CompleteStatus;
                    //工程检测进度X轴
                    expA04Row.工程X轴预加载完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_GCX[0].CompleteStatus;
                    expA04Row.工程X轴加载检测完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_GCX[1].CompleteStatus;
                    //工程检测进度Y轴
                    expA04Row.工程Y轴预加载完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_GCY[0].CompleteStatus;
                    expA04Row.工程Y轴加载检测完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_GCY[1].CompleteStatus;
                    //工程检测进度Z轴
                    expA04Row.工程Z轴预加载完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_GCZ[0].CompleteStatus;
                    expA04Row.工程Z轴加载检测完成状态 = DAL_ExpDQ.Exp_CJBX.StageList_GCZ[1].CompleteStatus;
                    A04TableAdapter.Update(A04Table);
                    A04Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        /// <summary>
        /// 保存当前气密试验数据至数据库
        /// </summary>
        private void SaveDataQM()
        {
            string tempNOStr = DAL_ExpDQ.ExpSettingParam.ExpNO;

            //保存试验设置信息至B1表
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
                    expB1Row.Qf_GC_Z = DAL_ExpDQ.ExpData_QM.Stl_QMGC[1][0];
                    expB1Row.Qfg_GC_Z = DAL_ExpDQ.ExpData_QM.Stl_QMGC[5][0];
                    expB1Row.Qz_GC_Z = DAL_ExpDQ.ExpData_QM.Stl_QMGC[9][0];
                    expB1Row.Qf_GC_F = DAL_ExpDQ.ExpData_QM.Stl_QMGC[3][0];
                    expB1Row.Qfg_GC_F = DAL_ExpDQ.ExpData_QM.Stl_QMGC[7][0];
                    expB1Row.Qz_GC_F = DAL_ExpDQ.ExpData_QM.Stl_QMGC[11][0];
                    expB1Row.Qf1_GC_Z = DAL_ExpDQ.ExpData_QM.Qf1_GC_Z;
                    expB1Row.Qfg1_GC_Z = DAL_ExpDQ.ExpData_QM.Qfg1_GC_Z;
                    expB1Row.Qz1_GC_Z = DAL_ExpDQ.ExpData_QM.Qz1_GC_Z;
                    expB1Row.Qs_GC_Z = DAL_ExpDQ.ExpData_QM.Qs_GC_Z;
                    expB1Row.Qk_GC_Z = DAL_ExpDQ.ExpData_QM.Qk_GC_Z;
                    expB1Row.QA1_GC_Z = DAL_ExpDQ.ExpData_QM.QA1_GC_Z;
                    expB1Row.Ql1_GC_Z = DAL_ExpDQ.ExpData_QM.Ql1_GC_Z;
                    expB1Row.Qf1_GC_F = DAL_ExpDQ.ExpData_QM.Qf1_GC_F;
                    expB1Row.Qfg1_GC_F = DAL_ExpDQ.ExpData_QM.Qfg1_GC_F;
                    expB1Row.Qz1_GC_F = DAL_ExpDQ.ExpData_QM.Qz1_GC_F;
                    expB1Row.Qs_GC_F = DAL_ExpDQ.ExpData_QM.Qs_GC_F;
                    expB1Row.Qk_GC_F = DAL_ExpDQ.ExpData_QM.Qk_GC_F;
                    expB1Row.QA1_GC_F = DAL_ExpDQ.ExpData_QM.QA1_GC_F;
                    expB1Row.Ql1_GC_F = DAL_ExpDQ.ExpData_QM.Ql1_GC_F;
                    expB1Row.Qf_DJ_ZS50 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[1][0];
                    expB1Row.Qf_DJ_ZS100 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[1][1];
                    expB1Row.Qf_DJ_Z150 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[1][2];
                    expB1Row.Qf_DJ_ZJ100 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[1][3];
                    expB1Row.Qf_DJ_ZJ50 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[1][4];
                    expB1Row.Qfg_DJ_ZS50 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[5][0];
                    expB1Row.Qfg_DJ_ZS100 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[5][1];
                    expB1Row.Qfg_DJ_Z150 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[5][2];
                    expB1Row.Qfg_DJ_ZJ100 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[5][3];
                    expB1Row.Qfg_DJ_ZJ50 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[5][4];
                    expB1Row.Qz_DJ_ZS50 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[9][0];
                    expB1Row.Qz_DJ_ZS100 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[9][1];
                    expB1Row.Qz_DJ_Z150 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[9][2];
                    expB1Row.Qz_DJ_ZJ100 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[9][3];
                    expB1Row.Qz_DJ_ZJ50 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[9][4];
                    expB1Row.Qf_DJ_FS50 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[3][0];
                    expB1Row.Qf_DJ_FS100 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[3][1];
                    expB1Row.Qf_DJ_F150 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[3][2];
                    expB1Row.Qf_DJ_FJ100 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[3][3];
                    expB1Row.Qf_DJ_FJ50 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[3][4];
                    expB1Row.Qfg_DJ_FS50 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[7][0];
                    expB1Row.Qfg_DJ_FS100 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[7][1];
                    expB1Row.Qfg_DJ_F150 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[7][2];
                    expB1Row.Qfg_DJ_FJ100 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[7][3];
                    expB1Row.Qfg_DJ_FJ50 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[7][4];
                    expB1Row.Qz_DJ_FS50 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[11][0];
                    expB1Row.Qz_DJ_FS100 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[11][1];
                    expB1Row.Qz_DJ_F150 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[11][2];
                    expB1Row.Qz_DJ_FJ100 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[11][3];
                    expB1Row.Qz_DJ_FJ50 = DAL_ExpDQ.ExpData_QM.Stl_QMDJ[11][4];
                    expB1Row.Qfp_DJ_Z = DAL_ExpDQ.ExpData_QM.Qfp_DJ_Z;
                    expB1Row.Qfgp_DJ_Z = DAL_ExpDQ.ExpData_QM.Qfgp_DJ_Z;
                    expB1Row.Qzp_DJ_Z = DAL_ExpDQ.ExpData_QM.Qzp_DJ_Z;
                    expB1Row.Qf1_DJ_Z = DAL_ExpDQ.ExpData_QM.Qf1_DJ_Z;
                    expB1Row.Qfg1_DJ_Z = DAL_ExpDQ.ExpData_QM.Qfg1_DJ_Z;
                    expB1Row.Qz1_DJ_Z = DAL_ExpDQ.ExpData_QM.Qz1_DJ_Z;
                    expB1Row.Qs_DJ_Z = DAL_ExpDQ.ExpData_QM.Qs_DJ_Z;
                    expB1Row.Qk_DJ_Z = DAL_ExpDQ.ExpData_QM.Qk_DJ_Z;
                    expB1Row.QA1_DJ_Z = DAL_ExpDQ.ExpData_QM.QA1_DJ_Z;
                    expB1Row.Ql1_DJ_Z = DAL_ExpDQ.ExpData_QM.Ql1_DJ_Z;
                    expB1Row.QA_DJ_Z = DAL_ExpDQ.ExpData_QM.QA_DJ_Z;
                    expB1Row.Ql_DJ_Z = DAL_ExpDQ.ExpData_QM.Ql_DJ_Z;
                    expB1Row.Qfp_DJ_F = DAL_ExpDQ.ExpData_QM.Qfp_DJ_F;
                    expB1Row.Qfgp_DJ_F = DAL_ExpDQ.ExpData_QM.Qfgp_DJ_F;
                    expB1Row.Qzp_DJ_F = DAL_ExpDQ.ExpData_QM.Qzp_DJ_F;
                    expB1Row.Qf1_DJ_F = DAL_ExpDQ.ExpData_QM.Qf1_DJ_F;
                    expB1Row.Qfg1_DJ_F = DAL_ExpDQ.ExpData_QM.Qfg1_DJ_F;
                    expB1Row.Qz1_DJ_F = DAL_ExpDQ.ExpData_QM.Qz1_DJ_F;
                    expB1Row.Qs_DJ_F = DAL_ExpDQ.ExpData_QM.Qs_DJ_F;
                    expB1Row.Qk_DJ_F = DAL_ExpDQ.ExpData_QM.Qk_DJ_F;
                    expB1Row.QA1_DJ_F = DAL_ExpDQ.ExpData_QM.QA1_DJ_F;
                    expB1Row.Ql1_DJ_F = DAL_ExpDQ.ExpData_QM.Ql1_DJ_F;
                    expB1Row.QA_DJ_F = DAL_ExpDQ.ExpData_QM.QA_DJ_F;
                    expB1Row.Ql_DJ_F = DAL_ExpDQ.ExpData_QM.Ql_DJ_F;
                    expB1Row.QA_DJ_QM = DAL_ExpDQ.ExpData_QM.QA_DJ_QM;
                    expB1Row.Ql_DJ_QM = DAL_ExpDQ.ExpData_QM.Ql_DJ_QM;
                    expB1Row.IsMeetDesign_GC_ZQA = DAL_ExpDQ.ExpData_QM.IsMeetDesign_GC_ZQA;
                    expB1Row.IsMeetDesign_GC_FQA = DAL_ExpDQ.ExpData_QM.IsMeetDesign_GC_FQA;
                    expB1Row.IsMeetDesign_GC_ZQl = DAL_ExpDQ.ExpData_QM.IsMeetDesign_GC_ZQl;
                    expB1Row.IsMeetDesign_GC_FQl = DAL_ExpDQ.ExpData_QM.IsMeetDesign_GC_FQl;
                    expB1Row.IsMeetDesign_GC_QM = DAL_ExpDQ.ExpData_QM.IsMeetDesign_GC_QM;
                    expB1Row.QALevel_DJ_QM = DAL_ExpDQ.ExpData_QM.QALevel_DJ_QM;
                    expB1Row.QlLevel_DJ_QM = DAL_ExpDQ.ExpData_QM.QlLevel_DJ_QM;
                    expB1Row.QA正Level_DJ_QM = DAL_ExpDQ.ExpData_QM.QALevel_DJZ_QM;
                    expB1Row.Ql正Level_DJ_QM = DAL_ExpDQ.ExpData_QM.QlLevel_DJZ_QM;
                    expB1Row.QA负Level_DJ_QM = DAL_ExpDQ.ExpData_QM.QALevel_DJF_QM;
                    expB1Row.Ql负Level_DJ_QM = DAL_ExpDQ.ExpData_QM.QlLevel_DJF_QM;

                    B1TableAdapter.Update(B1Table);
                    B1Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 保存当前水密试验数据至数据库
        /// </summary>
        private void SaveDataSM()
        {
            string tempNOStr = DAL_ExpDQ.ExpSettingParam.ExpNO;

            //保存试验设置信息至B2水密数据表
            //若编号在表中不存在，则新建
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
                    expB2Row.定级第1级检测压力 = DAL_ExpDQ.ExpData_SM.Press_DJ[0];
                    expB2Row.定级第2级检测压力 = DAL_ExpDQ.ExpData_SM.Press_DJ[1];
                    expB2Row.定级第3级检测压力 = DAL_ExpDQ.ExpData_SM.Press_DJ[2];
                    expB2Row.定级第4级检测压力 = DAL_ExpDQ.ExpData_SM.Press_DJ[3];
                    expB2Row.定级第5级检测压力 = DAL_ExpDQ.ExpData_SM.Press_DJ[4];
                    expB2Row.定级第6级检测压力 = DAL_ExpDQ.ExpData_SM.Press_DJ[5];
                    expB2Row.定级第7级检测压力 = DAL_ExpDQ.ExpData_SM.Press_DJ[6];
                    expB2Row.定级第8级检测压力 = DAL_ExpDQ.ExpData_SM.Press_DJ[7];
                    expB2Row.定级第1级可开启部分渗漏情况 = DAL_ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[0];
                    expB2Row.定级第2级可开启部分渗漏情况 = DAL_ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[1];
                    expB2Row.定级第3级可开启部分渗漏情况 = DAL_ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[2];
                    expB2Row.定级第4级可开启部分渗漏情况 = DAL_ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[3];
                    expB2Row.定级第5级可开启部分渗漏情况 = DAL_ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[4];
                    expB2Row.定级第6级可开启部分渗漏情况 = DAL_ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[5];
                    expB2Row.定级第7级可开启部分渗漏情况 = DAL_ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[6];
                    expB2Row.定级第8级可开启部分渗漏情况 = DAL_ExpDQ.ExpData_SM.SLStatus_DJ_KKQ[7];
                    expB2Row.定级第1级固定部分渗漏情况 = DAL_ExpDQ.ExpData_SM.SLStatus_DJ_GD[0];
                    expB2Row.定级第2级固定部分渗漏情况 = DAL_ExpDQ.ExpData_SM.SLStatus_DJ_GD[1];
                    expB2Row.定级第3级固定部分渗漏情况 = DAL_ExpDQ.ExpData_SM.SLStatus_DJ_GD[2];
                    expB2Row.定级第4级固定部分渗漏情况 = DAL_ExpDQ.ExpData_SM.SLStatus_DJ_GD[3];
                    expB2Row.定级第5级固定部分渗漏情况 = DAL_ExpDQ.ExpData_SM.SLStatus_DJ_GD[4];
                    expB2Row.定级第6级固定部分渗漏情况 = DAL_ExpDQ.ExpData_SM.SLStatus_DJ_GD[5];
                    expB2Row.定级第7级固定部分渗漏情况 = DAL_ExpDQ.ExpData_SM.SLStatus_DJ_GD[6];
                    expB2Row.定级第8级固定部分渗漏情况 = DAL_ExpDQ.ExpData_SM.SLStatus_DJ_GD[7];
                    expB2Row.定级第1级可开启部分渗漏说明 = DAL_ExpDQ.ExpData_SM.SLPS_DJ_KKQ[0];
                    expB2Row.定级第2级可开启部分渗漏说明 = DAL_ExpDQ.ExpData_SM.SLPS_DJ_KKQ[1];
                    expB2Row.定级第3级可开启部分渗漏说明 = DAL_ExpDQ.ExpData_SM.SLPS_DJ_KKQ[2];
                    expB2Row.定级第4级可开启部分渗漏说明 = DAL_ExpDQ.ExpData_SM.SLPS_DJ_KKQ[3];
                    expB2Row.定级第5级可开启部分渗漏说明 = DAL_ExpDQ.ExpData_SM.SLPS_DJ_KKQ[4];
                    expB2Row.定级第6级可开启部分渗漏说明 = DAL_ExpDQ.ExpData_SM.SLPS_DJ_KKQ[5];
                    expB2Row.定级第7级可开启部分渗漏说明 = DAL_ExpDQ.ExpData_SM.SLPS_DJ_KKQ[6];
                    expB2Row.定级第8级可开启部分渗漏说明 = DAL_ExpDQ.ExpData_SM.SLPS_DJ_KKQ[7];
                    expB2Row.定级第1级固定部分渗漏说明 = DAL_ExpDQ.ExpData_SM.SLPS_DJ_GD[0];
                    expB2Row.定级第2级固定部分渗漏说明 = DAL_ExpDQ.ExpData_SM.SLPS_DJ_GD[1];
                    expB2Row.定级第3级固定部分渗漏说明 = DAL_ExpDQ.ExpData_SM.SLPS_DJ_GD[2];
                    expB2Row.定级第4级固定部分渗漏说明 = DAL_ExpDQ.ExpData_SM.SLPS_DJ_GD[3];
                    expB2Row.定级第5级固定部分渗漏说明 = DAL_ExpDQ.ExpData_SM.SLPS_DJ_GD[4];
                    expB2Row.定级第6级固定部分渗漏说明 = DAL_ExpDQ.ExpData_SM.SLPS_DJ_GD[5];
                    expB2Row.定级第7级固定部分渗漏说明 = DAL_ExpDQ.ExpData_SM.SLPS_DJ_GD[6];
                    expB2Row.定级第8级固定部分渗漏说明 = DAL_ExpDQ.ExpData_SM.SLPS_DJ_GD[7];
                    expB2Row.工程可开启部分检测压力 = DAL_ExpDQ.ExpData_SM.Press_GC[0];
                    expB2Row.工程固定部分检测压力 = DAL_ExpDQ.ExpData_SM.Press_GC[1];
                    expB2Row.工程可开启部分渗漏情况 = DAL_ExpDQ.ExpData_SM.SLStatus_GC_KKQ[0];
                    expB2Row.工程固定部分渗漏情况 = DAL_ExpDQ.ExpData_SM.SLStatus_GC_GD[0];
                    expB2Row.工程可开启部分渗漏说明 = DAL_ExpDQ.ExpData_SM.SLPS_GC_KKQ[0];
                    expB2Row.工程固定部分渗漏说明 = DAL_ExpDQ.ExpData_SM.SLPS_GC_GD[0];
                    expB2Row.定级可开启最大未严重渗漏压力 = DAL_ExpDQ.ExpData_SM.MaxPressWYZSL_DJ_KKQ;
                    expB2Row.定级固定最大未严重渗漏压力 = DAL_ExpDQ.ExpData_SM.MaxPressWYZSL_DJ_GD;
                    expB2Row.定级可开启部分评定等级 = DAL_ExpDQ.ExpData_SM.Level_DJ_KKQ;
                    expB2Row.定级固定部分评定等级 = DAL_ExpDQ.ExpData_SM.Level_DJ_GD;
                    expB2Row.定级可开启部分最终渗漏情况 = DAL_ExpDQ.ExpData_SM.SLStatusFinal_DJ_KKQ;
                    expB2Row.定级固定部分最终渗漏情况 = DAL_ExpDQ.ExpData_SM.SLStatusFinal_DJ_GD;
                    expB2Row.定级可开启部分最终渗漏说明 = DAL_ExpDQ.ExpData_SM.SLPSFinal_DJ_KKQ;
                    expB2Row.定级固定部分最终渗漏说明 = DAL_ExpDQ.ExpData_SM.SLPSFinal_DJ_GD;
                    expB2Row.工程可开启部分是否满足设计 = DAL_ExpDQ.ExpData_SM.IsMeetDesign_GC_KKQ;
                    expB2Row.工程固定部分是否满足设计 = DAL_ExpDQ.ExpData_SM.IsMeetDesign_GC_GD;
                    expB2Row.工程整体是否满足设计 = DAL_ExpDQ.ExpData_SM.IsMeetDesign_GC_All;

                    B2TableAdapter.Update(B2Table);
                    B2Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 保存当前抗风压试验数据至数据库
        /// </summary>
        private void SaveDataKFY()
        {
            string tempNOStr = DAL_ExpDQ.ExpSettingParam.ExpNO;

            #region B300

            //保存试验设置信息至B300抗风压试验数据表
            //若编号在表中不存在，则新建
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
                    expB300Row.定级变形检测压力Z01 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_Z[0];
                    expB300Row.定级变形检测压力Z02 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_Z[1];
                    expB300Row.定级变形检测压力Z03 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_Z[2];
                    expB300Row.定级变形检测压力Z04 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_Z[3];
                    expB300Row.定级变形检测压力Z05 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_Z[4];
                    expB300Row.定级变形检测压力Z06 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_Z[5];
                    expB300Row.定级变形检测压力Z07 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_Z[6];
                    expB300Row.定级变形检测压力Z08 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_Z[7];
                    expB300Row.定级变形检测压力Z09 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_Z[8];
                    expB300Row.定级变形检测压力Z10 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_Z[9];
                    expB300Row.定级变形检测压力Z11 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_Z[10];
                    expB300Row.定级变形检测压力Z12 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_Z[11];
                    expB300Row.定级变形检测压力Z13 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_Z[12];
                    expB300Row.定级变形检测压力Z14 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_Z[13];
                    expB300Row.定级变形检测压力Z15 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_Z[14];
                    expB300Row.定级变形检测压力Z16 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_Z[15];
                    expB300Row.定级变形检测压力Z17 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_Z[16];
                    expB300Row.定级变形检测压力Z18 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_Z[17];
                    expB300Row.定级变形检测压力Z19 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_Z[18];
                    expB300Row.定级变形检测压力Z20 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_Z[19];
                    expB300Row.定级变形检测压力F01 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_F[0];
                    expB300Row.定级变形检测压力F02 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_F[1];
                    expB300Row.定级变形检测压力F03 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_F[2];
                    expB300Row.定级变形检测压力F04 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_F[3];
                    expB300Row.定级变形检测压力F05 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_F[4];
                    expB300Row.定级变形检测压力F06 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_F[5];
                    expB300Row.定级变形检测压力F07 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_F[6];
                    expB300Row.定级变形检测压力F08 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_F[7];
                    expB300Row.定级变形检测压力F09 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_F[8];
                    expB300Row.定级变形检测压力F10 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_F[9];
                    expB300Row.定级变形检测压力F11 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_F[10];
                    expB300Row.定级变形检测压力F12 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_F[11];
                    expB300Row.定级变形检测压力F13 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_F[12];
                    expB300Row.定级变形检测压力F14 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_F[13];
                    expB300Row.定级变形检测压力F15 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_F[14];
                    expB300Row.定级变形检测压力F16 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_F[15];
                    expB300Row.定级变形检测压力F17 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_F[16];
                    expB300Row.定级变形检测压力F18 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_F[17];
                    expB300Row.定级变形检测压力F19 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_F[18];
                    expB300Row.定级变形检测压力F20 = DAL_ExpDQ.ExpData_KFY.TestPress_DJBX_F[19];
                    expB300Row.定级反复检测压力p2Z = DAL_ExpDQ.ExpData_KFY.TestPress_DJP2_Z;
                    expB300Row.定级反复检测压力p2F = DAL_ExpDQ.ExpData_KFY.TestPress_DJP2_F;
                    expB300Row.定级标准值检测压力p3Z = DAL_ExpDQ.ExpData_KFY.TestPress_DJP3_Z;
                    expB300Row.定级标准值检测压力p3F = DAL_ExpDQ.ExpData_KFY.TestPress_DJP3_F;
                    expB300Row.定级设计值检测压力pmaxZ = DAL_ExpDQ.ExpData_KFY.TestPress_DJPmax_Z;
                    expB300Row.定级设计值检测压力pmaxF = DAL_ExpDQ.ExpData_KFY.TestPress_DJPmax_F;
                    //定级评定数据
                    expB300Row.定级评定p1Z = DAL_ExpDQ.ExpData_KFY.PDValue_DJP1_Z;
                    expB300Row.定级评定p1F = DAL_ExpDQ.ExpData_KFY.PDValue_DJP1_F;
                    expB300Row.定级评定p1 = DAL_ExpDQ.ExpData_KFY.PDValue_DJP1_All;
                    expB300Row.定级评定p2Z = DAL_ExpDQ.ExpData_KFY.PDValue_DJP2_Z;
                    expB300Row.定级评定p2F = DAL_ExpDQ.ExpData_KFY.PDValue_DJP2_F;
                    expB300Row.定级评定p2 = DAL_ExpDQ.ExpData_KFY.PDValue_DJP2_All;
                    expB300Row.定级评定p3Z = DAL_ExpDQ.ExpData_KFY.PDValue_DJP3_Z;
                    expB300Row.定级评定p3F = DAL_ExpDQ.ExpData_KFY.PDValue_DJP3_F;
                    expB300Row.定级评定p3 = DAL_ExpDQ.ExpData_KFY.PDValue_DJP3_All;
                    expB300Row.定级p3评级 = DAL_ExpDQ.ExpData_KFY.PdLevel_DJP3;

                    //工程检测压力
                    expB300Row.工程变形检测压力Z01 = DAL_ExpDQ.ExpData_KFY.TestPress_GCBX_Z[0];
                    expB300Row.工程变形检测压力Z02 = DAL_ExpDQ.ExpData_KFY.TestPress_GCBX_Z[1];
                    expB300Row.工程变形检测压力Z03 = DAL_ExpDQ.ExpData_KFY.TestPress_GCBX_Z[2];
                    expB300Row.工程变形检测压力Z04 = DAL_ExpDQ.ExpData_KFY.TestPress_GCBX_Z[3];
                    expB300Row.工程变形检测压力F01 = DAL_ExpDQ.ExpData_KFY.TestPress_GCBX_F[0];
                    expB300Row.工程变形检测压力F02 = DAL_ExpDQ.ExpData_KFY.TestPress_GCBX_F[1];
                    expB300Row.工程变形检测压力F03 = DAL_ExpDQ.ExpData_KFY.TestPress_GCBX_F[2];
                    expB300Row.工程变形检测压力F04 = DAL_ExpDQ.ExpData_KFY.TestPress_GCBX_F[3];
                    expB300Row.工程反复检测压力p2Z = DAL_ExpDQ.ExpData_KFY.TestPress_GCP2_Z;
                    expB300Row.工程反复检测压力p2F = DAL_ExpDQ.ExpData_KFY.TestPress_GCP2_F;
                    expB300Row.工程标准值检测压力p3Z = DAL_ExpDQ.ExpData_KFY.TestPress_GCP3_Z;
                    expB300Row.工程标准值检测压力p3F = DAL_ExpDQ.ExpData_KFY.TestPress_GCP3_F;
                    expB300Row.工程设计值检测压力pmaxZ = DAL_ExpDQ.ExpData_KFY.TestPress_GCPmax_Z;
                    expB300Row.工程设计值检测压力pmaxF = DAL_ExpDQ.ExpData_KFY.TestPress_GCPmax_F;

                    //工程评定数据
                    expB300Row.工程检测p1是否满足要求 = DAL_ExpDQ.ExpData_KFY.IsMeetDesign_GCp1;
                    expB300Row.工程检测p2是否满足要求 = DAL_ExpDQ.ExpData_KFY.IsMeetDesign_GCp2;
                    expB300Row.工程检测p3是否满足要求 = DAL_ExpDQ.ExpData_KFY.IsMeetDesign_GCp3;
                    expB300Row.工程检测pmax是否满足要求 = DAL_ExpDQ.ExpData_KFY.IsMeetDesign_GCpmax;
                    expB300Row.工程检测总体是否满足要求 = DAL_ExpDQ.ExpData_KFY.IsMeetDesign_GCfinal;

                    B300TableAdapter.Update(B300Table);
                    B300Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B311a

            //保存试验设置信息至B311a抗风压定级组1点a位移数据
            //若编号在表中不存在，则新建
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
                    expB311aRow.定级变形1aZ00 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[0][0];
                    expB311aRow.定级变形1aZ01 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[1][0];
                    expB311aRow.定级变形1aZ02 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[2][0];
                    expB311aRow.定级变形1aZ03 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[3][0];
                    expB311aRow.定级变形1aZ04 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[4][0];
                    expB311aRow.定级变形1aZ05 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[5][0];
                    expB311aRow.定级变形1aZ06 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[6][0];
                    expB311aRow.定级变形1aZ07 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[7][0];
                    expB311aRow.定级变形1aZ08 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[8][0];
                    expB311aRow.定级变形1aZ09 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[9][0];
                    expB311aRow.定级变形1aZ10 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[10][0];
                    expB311aRow.定级变形1aZ11 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[11][0];
                    expB311aRow.定级变形1aZ12 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[12][0];
                    expB311aRow.定级变形1aZ13 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[13][0];
                    expB311aRow.定级变形1aZ14 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[14][0];
                    expB311aRow.定级变形1aZ15 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[15][0];
                    expB311aRow.定级变形1aZ16 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[16][0];
                    expB311aRow.定级变形1aZ17 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[17][0];
                    expB311aRow.定级变形1aZ18 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[18][0];
                    expB311aRow.定级变形1aZ19 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[19][0];
                    expB311aRow.定级变形1aZ20 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[20][0];
                    //定级P1负
                    expB311aRow.定级变形1aF00 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[0][0];
                    expB311aRow.定级变形1aF01 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[1][0];
                    expB311aRow.定级变形1aF02 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[2][0];
                    expB311aRow.定级变形1aF03 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[3][0];
                    expB311aRow.定级变形1aF04 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[4][0];
                    expB311aRow.定级变形1aF05 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[5][0];
                    expB311aRow.定级变形1aF06 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[6][0];
                    expB311aRow.定级变形1aF07 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[7][0];
                    expB311aRow.定级变形1aF08 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[8][0];
                    expB311aRow.定级变形1aF09 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[9][0];
                    expB311aRow.定级变形1aF10 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[10][0];
                    expB311aRow.定级变形1aF11 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[11][0];
                    expB311aRow.定级变形1aF12 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[12][0];
                    expB311aRow.定级变形1aF13 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[13][0];
                    expB311aRow.定级变形1aF14 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[14][0];
                    expB311aRow.定级变形1aF15 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[15][0];
                    expB311aRow.定级变形1aF16 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[16][0];
                    expB311aRow.定级变形1aF17 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[17][0];
                    expB311aRow.定级变形1aF18 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[18][0];
                    expB311aRow.定级变形1aF19 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[19][0];
                    expB311aRow.定级变形1aF20 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[20][0];
                    //定级p3
                    expB311aRow.定级P31aZ0 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_Z[0][0];
                    expB311aRow.定级P31aZ1 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_Z[1][0];
                    expB311aRow.定级P31aF0 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_F[0][0];
                    expB311aRow.定级P31aF1 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_F[1][0];
                    //定级pmax
                    expB311aRow.定级Pmax1aZ0 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][0];
                    expB311aRow.定级Pmax1aZ1 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][0];
                    expB311aRow.定级Pmax1aF0 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_F[0][0];
                    expB311aRow.定级Pmax1aF1 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_F[1][0];

                    B311aTableAdapter.Update(B311aTable);
                    B311aTable.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B311b

            //保存试验设置信息至B311b抗风压定级组1点b位移数据
            //若编号在表中不存在，则新建
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
                    expB311bRow.定级变形1bZ00 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[0][1];
                    expB311bRow.定级变形1bZ01 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[1][1];
                    expB311bRow.定级变形1bZ02 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[2][1];
                    expB311bRow.定级变形1bZ03 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[3][1];
                    expB311bRow.定级变形1bZ04 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[4][1];
                    expB311bRow.定级变形1bZ05 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[5][1];
                    expB311bRow.定级变形1bZ06 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[6][1];
                    expB311bRow.定级变形1bZ07 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[7][1];
                    expB311bRow.定级变形1bZ08 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[8][1];
                    expB311bRow.定级变形1bZ09 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[9][1];
                    expB311bRow.定级变形1bZ10 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[10][1];
                    expB311bRow.定级变形1bZ11 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[11][1];
                    expB311bRow.定级变形1bZ12 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[12][1];
                    expB311bRow.定级变形1bZ13 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[13][1];
                    expB311bRow.定级变形1bZ14 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[14][1];
                    expB311bRow.定级变形1bZ15 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[15][1];
                    expB311bRow.定级变形1bZ16 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[16][1];
                    expB311bRow.定级变形1bZ17 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[17][1];
                    expB311bRow.定级变形1bZ18 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[18][1];
                    expB311bRow.定级变形1bZ19 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[19][1];
                    expB311bRow.定级变形1bZ20 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[20][1];
                    //定级P1负
                    expB311bRow.定级变形1bF00 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[0][1];
                    expB311bRow.定级变形1bF01 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[1][1];
                    expB311bRow.定级变形1bF02 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[2][1];
                    expB311bRow.定级变形1bF03 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[3][1];
                    expB311bRow.定级变形1bF04 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[4][1];
                    expB311bRow.定级变形1bF05 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[5][1];
                    expB311bRow.定级变形1bF06 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[6][1];
                    expB311bRow.定级变形1bF07 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[7][1];
                    expB311bRow.定级变形1bF08 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[8][1];
                    expB311bRow.定级变形1bF09 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[9][1];
                    expB311bRow.定级变形1bF10 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[10][1];
                    expB311bRow.定级变形1bF11 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[11][1];
                    expB311bRow.定级变形1bF12 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[12][1];
                    expB311bRow.定级变形1bF13 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[13][1];
                    expB311bRow.定级变形1bF14 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[14][1];
                    expB311bRow.定级变形1bF15 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[15][1];
                    expB311bRow.定级变形1bF16 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[16][1];
                    expB311bRow.定级变形1bF17 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[17][1];
                    expB311bRow.定级变形1bF18 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[18][1];
                    expB311bRow.定级变形1bF19 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[19][1];
                    expB311bRow.定级变形1bF20 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[20][1];
                    //定级p3
                    expB311bRow.定级P31bZ0 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_Z[0][1];
                    expB311bRow.定级P31bZ1 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_Z[1][1];
                    expB311bRow.定级P31bF0 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_F[0][1];
                    expB311bRow.定级P31bF1 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_F[1][1];
                    //定级pmax
                    expB311bRow.定级Pmax1bZ0 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][1];
                    expB311bRow.定级Pmax1bZ1 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][1];
                    expB311bRow.定级Pmax1bF0 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_F[0][1];
                    expB311bRow.定级Pmax1bF1 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_F[1][1];

                    B311bTableAdapter.Update(B311bTable);
                    B311bTable.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B311c

            //保存试验设置信息至B311c抗风压定级组1点c位移数据
            //若编号在表中不存在，则新建
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
                    expB311cRow.定级变形1cZ00 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[0][2];
                    expB311cRow.定级变形1cZ01 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[1][2];
                    expB311cRow.定级变形1cZ02 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[2][2];
                    expB311cRow.定级变形1cZ03 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[3][2];
                    expB311cRow.定级变形1cZ04 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[4][2];
                    expB311cRow.定级变形1cZ05 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[5][2];
                    expB311cRow.定级变形1cZ06 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[6][2];
                    expB311cRow.定级变形1cZ07 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[7][2];
                    expB311cRow.定级变形1cZ08 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[8][2];
                    expB311cRow.定级变形1cZ09 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[9][2];
                    expB311cRow.定级变形1cZ10 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[10][2];
                    expB311cRow.定级变形1cZ11 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[11][2];
                    expB311cRow.定级变形1cZ12 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[12][2];
                    expB311cRow.定级变形1cZ13 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[13][2];
                    expB311cRow.定级变形1cZ14 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[14][2];
                    expB311cRow.定级变形1cZ15 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[15][2];
                    expB311cRow.定级变形1cZ16 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[16][2];
                    expB311cRow.定级变形1cZ17 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[17][2];
                    expB311cRow.定级变形1cZ18 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[18][2];
                    expB311cRow.定级变形1cZ19 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[19][2];
                    expB311cRow.定级变形1cZ20 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[20][2];
                    //定级P1负
                    expB311cRow.定级变形1cF00 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[0][2];
                    expB311cRow.定级变形1cF01 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[1][2];
                    expB311cRow.定级变形1cF02 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[2][2];
                    expB311cRow.定级变形1cF03 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[3][2];
                    expB311cRow.定级变形1cF04 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[4][2];
                    expB311cRow.定级变形1cF05 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[5][2];
                    expB311cRow.定级变形1cF06 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[6][2];
                    expB311cRow.定级变形1cF07 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[7][2];
                    expB311cRow.定级变形1cF08 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[8][2];
                    expB311cRow.定级变形1cF09 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[9][2];
                    expB311cRow.定级变形1cF10 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[10][2];
                    expB311cRow.定级变形1cF11 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[11][2];
                    expB311cRow.定级变形1cF12 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[12][2];
                    expB311cRow.定级变形1cF13 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[13][2];
                    expB311cRow.定级变形1cF14 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[14][2];
                    expB311cRow.定级变形1cF15 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[15][2];
                    expB311cRow.定级变形1cF16 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[16][2];
                    expB311cRow.定级变形1cF17 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[17][2];
                    expB311cRow.定级变形1cF18 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[18][2];
                    expB311cRow.定级变形1cF19 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[19][2];
                    expB311cRow.定级变形1cF20 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[20][2];
                    //定级p3
                    expB311cRow.定级P31cZ0 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_Z[0][2];
                    expB311cRow.定级P31cZ1 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_Z[1][2];
                    expB311cRow.定级P31cF0 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_F[0][2];
                    expB311cRow.定级P31cF1 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_F[1][2];
                    //定级pmax
                    expB311cRow.定级Pmax1cZ0 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][2];
                    expB311cRow.定级Pmax1cZ1 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][2];
                    expB311cRow.定级Pmax1cF0 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_F[0][2];
                    expB311cRow.定级Pmax1cF1 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_F[1][2];

                    B311cTableAdapter.Update(B311cTable);
                    B311cTable.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B311d

            //保存试验设置信息至B311d抗风压定级组1点d位移数据
            //若编号在表中不存在，则新建
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
                    expB311dRow.定级变形1dZ00 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[0][3];
                    expB311dRow.定级变形1dZ01 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[1][3];
                    expB311dRow.定级变形1dZ02 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[2][3];
                    expB311dRow.定级变形1dZ03 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[3][3];
                    expB311dRow.定级变形1dZ04 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[4][3];
                    expB311dRow.定级变形1dZ05 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[5][3];
                    expB311dRow.定级变形1dZ06 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[6][3];
                    expB311dRow.定级变形1dZ07 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[7][3];
                    expB311dRow.定级变形1dZ08 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[8][3];
                    expB311dRow.定级变形1dZ09 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[9][3];
                    expB311dRow.定级变形1dZ10 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[10][3];
                    expB311dRow.定级变形1dZ11 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[11][3];
                    expB311dRow.定级变形1dZ12 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[12][3];
                    expB311dRow.定级变形1dZ13 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[13][3];
                    expB311dRow.定级变形1dZ14 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[14][3];
                    expB311dRow.定级变形1dZ15 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[15][3];
                    expB311dRow.定级变形1dZ16 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[16][3];
                    expB311dRow.定级变形1dZ17 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[17][3];
                    expB311dRow.定级变形1dZ18 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[18][3];
                    expB311dRow.定级变形1dZ19 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[19][3];
                    expB311dRow.定级变形1dZ20 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[20][3];
                    //定级P1负
                    expB311dRow.定级变形1dF00 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[0][3];
                    expB311dRow.定级变形1dF01 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[1][3];
                    expB311dRow.定级变形1dF02 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[2][3];
                    expB311dRow.定级变形1dF03 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[3][3];
                    expB311dRow.定级变形1dF04 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[4][3];
                    expB311dRow.定级变形1dF05 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[5][3];
                    expB311dRow.定级变形1dF06 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[6][3];
                    expB311dRow.定级变形1dF07 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[7][3];
                    expB311dRow.定级变形1dF08 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[8][3];
                    expB311dRow.定级变形1dF09 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[9][3];
                    expB311dRow.定级变形1dF10 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[10][3];
                    expB311dRow.定级变形1dF11 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[11][3];
                    expB311dRow.定级变形1dF12 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[12][3];
                    expB311dRow.定级变形1dF13 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[13][3];
                    expB311dRow.定级变形1dF14 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[14][3];
                    expB311dRow.定级变形1dF15 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[15][3];
                    expB311dRow.定级变形1dF16 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[16][3];
                    expB311dRow.定级变形1dF17 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[17][3];
                    expB311dRow.定级变形1dF18 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[18][3];
                    expB311dRow.定级变形1dF19 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[19][3];
                    expB311dRow.定级变形1dF20 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[20][3];
                    //定级p3
                    expB311dRow.定级P31dZ0 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_Z[0][3];
                    expB311dRow.定级P31dZ1 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_Z[1][3];
                    expB311dRow.定级P31dF0 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_F[0][3];
                    expB311dRow.定级P31dF1 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_F[1][3];
                    //定级pmax
                    expB311dRow.定级Pmax1dZ0 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][3];
                    expB311dRow.定级Pmax1dZ1 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][3];
                    expB311dRow.定级Pmax1dF0 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_F[0][3];
                    expB311dRow.定级Pmax1dF1 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_F[1][3];

                    B311dTableAdapter.Update(B311dTable);
                    B311dTable.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B312a

            //保存试验设置信息至B312a抗风压定级组2点a位移数据
            //若编号在表中不存在，则新建
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
                    expB312aRow.定级变形2aZ00 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[0][4];
                    expB312aRow.定级变形2aZ01 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[1][4];
                    expB312aRow.定级变形2aZ02 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[2][4];
                    expB312aRow.定级变形2aZ03 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[3][4];
                    expB312aRow.定级变形2aZ04 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[4][4];
                    expB312aRow.定级变形2aZ05 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[5][4];
                    expB312aRow.定级变形2aZ06 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[6][4];
                    expB312aRow.定级变形2aZ07 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[7][4];
                    expB312aRow.定级变形2aZ08 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[8][4];
                    expB312aRow.定级变形2aZ09 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[9][4];
                    expB312aRow.定级变形2aZ10 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[10][4];
                    expB312aRow.定级变形2aZ11 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[11][4];
                    expB312aRow.定级变形2aZ12 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[12][4];
                    expB312aRow.定级变形2aZ13 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[13][4];
                    expB312aRow.定级变形2aZ14 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[14][4];
                    expB312aRow.定级变形2aZ15 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[15][4];
                    expB312aRow.定级变形2aZ16 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[16][4];
                    expB312aRow.定级变形2aZ17 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[17][4];
                    expB312aRow.定级变形2aZ18 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[18][4];
                    expB312aRow.定级变形2aZ19 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[19][4];
                    expB312aRow.定级变形2aZ20 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[20][4];
                    //定级P1负
                    expB312aRow.定级变形2aF00 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[0][4];
                    expB312aRow.定级变形2aF01 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[1][4];
                    expB312aRow.定级变形2aF02 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[2][4];
                    expB312aRow.定级变形2aF03 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[3][4];
                    expB312aRow.定级变形2aF04 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[4][4];
                    expB312aRow.定级变形2aF05 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[5][4];
                    expB312aRow.定级变形2aF06 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[6][4];
                    expB312aRow.定级变形2aF07 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[7][4];
                    expB312aRow.定级变形2aF08 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[8][4];
                    expB312aRow.定级变形2aF09 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[9][4];
                    expB312aRow.定级变形2aF10 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[10][4];
                    expB312aRow.定级变形2aF11 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[11][4];
                    expB312aRow.定级变形2aF12 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[12][4];
                    expB312aRow.定级变形2aF13 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[13][4];
                    expB312aRow.定级变形2aF14 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[14][4];
                    expB312aRow.定级变形2aF15 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[15][4];
                    expB312aRow.定级变形2aF16 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[16][4];
                    expB312aRow.定级变形2aF17 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[17][4];
                    expB312aRow.定级变形2aF18 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[18][4];
                    expB312aRow.定级变形2aF19 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[19][4];
                    expB312aRow.定级变形2aF20 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[20][4];
                    //定级p3
                    expB312aRow.定级P32aZ0 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_Z[0][4];
                    expB312aRow.定级P32aZ1 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_Z[1][4];
                    expB312aRow.定级P32aF0 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_F[0][4];
                    expB312aRow.定级P32aF1 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_F[1][4];
                    //定级pmax
                    expB312aRow.定级Pmax2aZ0 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][4];
                    expB312aRow.定级Pmax2aZ1 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][4];
                    expB312aRow.定级Pmax2aF0 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_F[0][4];
                    expB312aRow.定级Pmax2aF1 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_F[1][4];

                    B312aTableAdapter.Update(B312aTable);
                    B312aTable.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B312b

            //保存试验设置信息至B312b抗风压定级组2点b位移数据
            //若编号在表中不存在，则新建
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
                    expB312bRow.定级变形2bZ00 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[0][5];
                    expB312bRow.定级变形2bZ01 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[1][5];
                    expB312bRow.定级变形2bZ02 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[2][5];
                    expB312bRow.定级变形2bZ03 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[3][5];
                    expB312bRow.定级变形2bZ04 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[4][5];
                    expB312bRow.定级变形2bZ05 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[5][5];
                    expB312bRow.定级变形2bZ06 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[6][5];
                    expB312bRow.定级变形2bZ07 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[7][5];
                    expB312bRow.定级变形2bZ08 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[8][5];
                    expB312bRow.定级变形2bZ09 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[9][5];
                    expB312bRow.定级变形2bZ10 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[10][5];
                    expB312bRow.定级变形2bZ11 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[11][5];
                    expB312bRow.定级变形2bZ12 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[12][5];
                    expB312bRow.定级变形2bZ13 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[13][5];
                    expB312bRow.定级变形2bZ14 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[14][5];
                    expB312bRow.定级变形2bZ15 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[15][5];
                    expB312bRow.定级变形2bZ16 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[16][5];
                    expB312bRow.定级变形2bZ17 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[17][5];
                    expB312bRow.定级变形2bZ18 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[18][5];
                    expB312bRow.定级变形2bZ19 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[19][5];
                    expB312bRow.定级变形2bZ20 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[20][5];
                    //定级P1负
                    expB312bRow.定级变形2bF00 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[0][5];
                    expB312bRow.定级变形2bF01 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[1][5];
                    expB312bRow.定级变形2bF02 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[2][5];
                    expB312bRow.定级变形2bF03 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[3][5];
                    expB312bRow.定级变形2bF04 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[4][5];
                    expB312bRow.定级变形2bF05 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[5][5];
                    expB312bRow.定级变形2bF06 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[6][5];
                    expB312bRow.定级变形2bF07 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[7][5];
                    expB312bRow.定级变形2bF08 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[8][5];
                    expB312bRow.定级变形2bF09 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[9][5];
                    expB312bRow.定级变形2bF10 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[10][5];
                    expB312bRow.定级变形2bF11 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[11][5];
                    expB312bRow.定级变形2bF12 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[12][5];
                    expB312bRow.定级变形2bF13 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[13][5];
                    expB312bRow.定级变形2bF14 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[14][5];
                    expB312bRow.定级变形2bF15 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[15][5];
                    expB312bRow.定级变形2bF16 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[16][5];
                    expB312bRow.定级变形2bF17 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[17][5];
                    expB312bRow.定级变形2bF18 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[18][5];
                    expB312bRow.定级变形2bF19 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[19][5];
                    expB312bRow.定级变形2bF20 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[20][5];
                    //定级p3
                    expB312bRow.定级P32bZ0 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_Z[0][5];
                    expB312bRow.定级P32bZ1 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_Z[1][5];
                    expB312bRow.定级P32bF0 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_F[0][5];
                    expB312bRow.定级P32bF1 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_F[1][5];
                    //定级pmax
                    expB312bRow.定级Pmax2bZ0 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][5];
                    expB312bRow.定级Pmax2bZ1 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][5];
                    expB312bRow.定级Pmax2bF0 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_F[0][5];
                    expB312bRow.定级Pmax2bF1 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_F[1][5];

                    B312bTableAdapter.Update(B312bTable);
                    B312bTable.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B312c

            //保存试验设置信息至B312c抗风压定级组2点c位移数据
            //若编号在表中不存在，则新建
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
                    expB312cRow.定级变形2cZ00 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[0][6];
                    expB312cRow.定级变形2cZ01 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[1][6];
                    expB312cRow.定级变形2cZ02 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[2][6];
                    expB312cRow.定级变形2cZ03 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[3][6];
                    expB312cRow.定级变形2cZ04 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[4][6];
                    expB312cRow.定级变形2cZ05 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[5][6];
                    expB312cRow.定级变形2cZ06 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[6][6];
                    expB312cRow.定级变形2cZ07 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[7][6];
                    expB312cRow.定级变形2cZ08 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[8][6];
                    expB312cRow.定级变形2cZ09 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[9][6];
                    expB312cRow.定级变形2cZ10 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[10][6];
                    expB312cRow.定级变形2cZ11 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[11][6];
                    expB312cRow.定级变形2cZ12 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[12][6];
                    expB312cRow.定级变形2cZ13 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[13][6];
                    expB312cRow.定级变形2cZ14 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[14][6];
                    expB312cRow.定级变形2cZ15 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[15][6];
                    expB312cRow.定级变形2cZ16 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[16][6];
                    expB312cRow.定级变形2cZ17 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[17][6];
                    expB312cRow.定级变形2cZ18 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[18][6];
                    expB312cRow.定级变形2cZ19 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[19][6];
                    expB312cRow.定级变形2cZ20 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[20][6];
                    //定级P1负
                    expB312cRow.定级变形2cF00 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[0][6];
                    expB312cRow.定级变形2cF01 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[1][6];
                    expB312cRow.定级变形2cF02 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[2][6];
                    expB312cRow.定级变形2cF03 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[3][6];
                    expB312cRow.定级变形2cF04 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[4][6];
                    expB312cRow.定级变形2cF05 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[5][6];
                    expB312cRow.定级变形2cF06 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[6][6];
                    expB312cRow.定级变形2cF07 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[7][6];
                    expB312cRow.定级变形2cF08 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[8][6];
                    expB312cRow.定级变形2cF09 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[9][6];
                    expB312cRow.定级变形2cF10 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[10][6];
                    expB312cRow.定级变形2cF11 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[11][6];
                    expB312cRow.定级变形2cF12 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[12][6];
                    expB312cRow.定级变形2cF13 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[13][6];
                    expB312cRow.定级变形2cF14 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[14][6];
                    expB312cRow.定级变形2cF15 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[15][6];
                    expB312cRow.定级变形2cF16 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[16][6];
                    expB312cRow.定级变形2cF17 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[17][6];
                    expB312cRow.定级变形2cF18 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[18][6];
                    expB312cRow.定级变形2cF19 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[19][6];
                    expB312cRow.定级变形2cF20 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[20][6];
                    //定级p3
                    expB312cRow.定级P32cZ0 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_Z[0][6];
                    expB312cRow.定级P32cZ1 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_Z[1][6];
                    expB312cRow.定级P32cF0 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_F[0][6];
                    expB312cRow.定级P32cF1 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_F[1][6];
                    //定级pmax
                    expB312cRow.定级Pmax2cZ0 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][6];
                    expB312cRow.定级Pmax2cZ1 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][6];
                    expB312cRow.定级Pmax2cF0 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_F[0][6];
                    expB312cRow.定级Pmax2cF1 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_F[1][6];

                    B312cTableAdapter.Update(B312cTable);
                    B312cTable.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B313a

            //保存试验设置信息至B313a抗风压定级组3点a位移数据
            //若编号在表中不存在，则新建
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
                    expB313aRow.定级变形3aZ00 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[0][7];
                    expB313aRow.定级变形3aZ01 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[1][7];
                    expB313aRow.定级变形3aZ02 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[2][7];
                    expB313aRow.定级变形3aZ03 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[3][7];
                    expB313aRow.定级变形3aZ04 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[4][7];
                    expB313aRow.定级变形3aZ05 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[5][7];
                    expB313aRow.定级变形3aZ06 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[6][7];
                    expB313aRow.定级变形3aZ07 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[7][7];
                    expB313aRow.定级变形3aZ08 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[8][7];
                    expB313aRow.定级变形3aZ09 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[9][7];
                    expB313aRow.定级变形3aZ10 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[10][7];
                    expB313aRow.定级变形3aZ11 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[11][7];
                    expB313aRow.定级变形3aZ12 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[12][7];
                    expB313aRow.定级变形3aZ13 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[13][7];
                    expB313aRow.定级变形3aZ14 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[14][7];
                    expB313aRow.定级变形3aZ15 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[15][7];
                    expB313aRow.定级变形3aZ16 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[16][7];
                    expB313aRow.定级变形3aZ17 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[17][7];
                    expB313aRow.定级变形3aZ18 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[18][7];
                    expB313aRow.定级变形3aZ19 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[19][7];
                    expB313aRow.定级变形3aZ20 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[20][7];
                    //定级P1负
                    expB313aRow.定级变形3aF00 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[0][7];
                    expB313aRow.定级变形3aF01 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[1][7];
                    expB313aRow.定级变形3aF02 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[2][7];
                    expB313aRow.定级变形3aF03 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[3][7];
                    expB313aRow.定级变形3aF04 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[4][7];
                    expB313aRow.定级变形3aF05 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[5][7];
                    expB313aRow.定级变形3aF06 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[6][7];
                    expB313aRow.定级变形3aF07 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[7][7];
                    expB313aRow.定级变形3aF08 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[8][7];
                    expB313aRow.定级变形3aF09 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[9][7];
                    expB313aRow.定级变形3aF10 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[10][7];
                    expB313aRow.定级变形3aF11 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[11][7];
                    expB313aRow.定级变形3aF12 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[12][7];
                    expB313aRow.定级变形3aF13 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[13][7];
                    expB313aRow.定级变形3aF14 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[14][7];
                    expB313aRow.定级变形3aF15 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[15][7];
                    expB313aRow.定级变形3aF16 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[16][7];
                    expB313aRow.定级变形3aF17 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[17][7];
                    expB313aRow.定级变形3aF18 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[18][7];
                    expB313aRow.定级变形3aF19 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[19][7];
                    expB313aRow.定级变形3aF20 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[20][7];
                    //定级p3
                    expB313aRow.定级P33aZ0 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_Z[0][7];
                    expB313aRow.定级P33aZ1 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_Z[1][7];
                    expB313aRow.定级P33aF0 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_F[0][7];
                    expB313aRow.定级P33aF1 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_F[1][7];
                    //定级pmax
                    expB313aRow.定级Pmax3aZ0 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][7];
                    expB313aRow.定级Pmax3aZ1 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][7];
                    expB313aRow.定级Pmax3aF0 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_F[0][7];
                    expB313aRow.定级Pmax3aF1 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_F[1][7];

                    B313aTableAdapter.Update(B313aTable);
                    B313aTable.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B313b

            //保存试验设置信息至B313b抗风压定级组3点b位移数据
            //若编号在表中不存在，则新建
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
                    expB313bRow.定级变形3bZ00 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[0][8];
                    expB313bRow.定级变形3bZ01 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[1][8];
                    expB313bRow.定级变形3bZ02 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[2][8];
                    expB313bRow.定级变形3bZ03 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[3][8];
                    expB313bRow.定级变形3bZ04 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[4][8];
                    expB313bRow.定级变形3bZ05 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[5][8];
                    expB313bRow.定级变形3bZ06 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[6][8];
                    expB313bRow.定级变形3bZ07 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[7][8];
                    expB313bRow.定级变形3bZ08 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[8][8];
                    expB313bRow.定级变形3bZ09 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[9][8];
                    expB313bRow.定级变形3bZ10 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[10][8];
                    expB313bRow.定级变形3bZ11 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[11][8];
                    expB313bRow.定级变形3bZ12 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[12][8];
                    expB313bRow.定级变形3bZ13 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[13][8];
                    expB313bRow.定级变形3bZ14 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[14][8];
                    expB313bRow.定级变形3bZ15 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[15][8];
                    expB313bRow.定级变形3bZ16 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[16][8];
                    expB313bRow.定级变形3bZ17 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[17][8];
                    expB313bRow.定级变形3bZ18 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[18][8];
                    expB313bRow.定级变形3bZ19 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[19][8];
                    expB313bRow.定级变形3bZ20 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[20][8];
                    //定级P1负
                    expB313bRow.定级变形3bF00 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[0][8];
                    expB313bRow.定级变形3bF01 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[1][8];
                    expB313bRow.定级变形3bF02 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[2][8];
                    expB313bRow.定级变形3bF03 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[3][8];
                    expB313bRow.定级变形3bF04 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[4][8];
                    expB313bRow.定级变形3bF05 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[5][8];
                    expB313bRow.定级变形3bF06 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[6][8];
                    expB313bRow.定级变形3bF07 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[7][8];
                    expB313bRow.定级变形3bF08 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[8][8];
                    expB313bRow.定级变形3bF09 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[9][8];
                    expB313bRow.定级变形3bF10 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[10][8];
                    expB313bRow.定级变形3bF11 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[11][8];
                    expB313bRow.定级变形3bF12 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[12][8];
                    expB313bRow.定级变形3bF13 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[13][8];
                    expB313bRow.定级变形3bF14 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[14][8];
                    expB313bRow.定级变形3bF15 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[15][8];
                    expB313bRow.定级变形3bF16 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[16][8];
                    expB313bRow.定级变形3bF17 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[17][8];
                    expB313bRow.定级变形3bF18 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[18][8];
                    expB313bRow.定级变形3bF19 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[19][8];
                    expB313bRow.定级变形3bF20 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[20][8];
                    //定级p3
                    expB313bRow.定级P33bZ0 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_Z[0][8];
                    expB313bRow.定级P33bZ1 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_Z[1][8];
                    expB313bRow.定级P33bF0 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_F[0][8];
                    expB313bRow.定级P33bF1 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_F[1][8];
                    //定级pmax
                    expB313bRow.定级Pmax3bZ0 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][8];
                    expB313bRow.定级Pmax3bZ1 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][8];
                    expB313bRow.定级Pmax3bF0 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_F[0][8];
                    expB313bRow.定级Pmax3bF1 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_F[1][8];

                    B313bTableAdapter.Update(B313bTable);
                    B313bTable.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B313c

            //保存试验设置信息至B313c抗风压定级组3点c位移数据
            //若编号在表中不存在，则新建
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
                    expB313cRow.定级变形3cZ00 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[0][9];
                    expB313cRow.定级变形3cZ01 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[1][9];
                    expB313cRow.定级变形3cZ02 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[2][9];
                    expB313cRow.定级变形3cZ03 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[3][9];
                    expB313cRow.定级变形3cZ04 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[4][9];
                    expB313cRow.定级变形3cZ05 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[5][9];
                    expB313cRow.定级变形3cZ06 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[6][9];
                    expB313cRow.定级变形3cZ07 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[7][9];
                    expB313cRow.定级变形3cZ08 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[8][9];
                    expB313cRow.定级变形3cZ09 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[9][9];
                    expB313cRow.定级变形3cZ10 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[10][9];
                    expB313cRow.定级变形3cZ11 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[11][9];
                    expB313cRow.定级变形3cZ12 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[12][9];
                    expB313cRow.定级变形3cZ13 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[13][9];
                    expB313cRow.定级变形3cZ14 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[14][9];
                    expB313cRow.定级变形3cZ15 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[15][9];
                    expB313cRow.定级变形3cZ16 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[16][9];
                    expB313cRow.定级变形3cZ17 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[17][9];
                    expB313cRow.定级变形3cZ18 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[18][9];
                    expB313cRow.定级变形3cZ19 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[19][9];
                    expB313cRow.定级变形3cZ20 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_Z[20][9];
                    //定级P1负
                    expB313cRow.定级变形3cF00 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[0][9];
                    expB313cRow.定级变形3cF01 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[1][9];
                    expB313cRow.定级变形3cF02 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[2][9];
                    expB313cRow.定级变形3cF03 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[3][9];
                    expB313cRow.定级变形3cF04 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[4][9];
                    expB313cRow.定级变形3cF05 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[5][9];
                    expB313cRow.定级变形3cF06 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[6][9];
                    expB313cRow.定级变形3cF07 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[7][9];
                    expB313cRow.定级变形3cF08 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[8][9];
                    expB313cRow.定级变形3cF09 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[9][9];
                    expB313cRow.定级变形3cF10 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[10][9];
                    expB313cRow.定级变形3cF11 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[11][9];
                    expB313cRow.定级变形3cF12 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[12][9];
                    expB313cRow.定级变形3cF13 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[13][9];
                    expB313cRow.定级变形3cF14 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[14][9];
                    expB313cRow.定级变形3cF15 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[15][9];
                    expB313cRow.定级变形3cF16 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[16][9];
                    expB313cRow.定级变形3cF17 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[17][9];
                    expB313cRow.定级变形3cF18 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[18][9];
                    expB313cRow.定级变形3cF19 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[19][9];
                    expB313cRow.定级变形3cF20 = DAL_ExpDQ.ExpData_KFY.WY_DJBX_F[20][9];
                    //定级p3
                    expB313cRow.定级P33cZ0 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_Z[0][9];
                    expB313cRow.定级P33cZ1 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_Z[1][9];
                    expB313cRow.定级P33cF0 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_F[0][9];
                    expB313cRow.定级P33cF1 = DAL_ExpDQ.ExpData_KFY.WY_DJP3_F[1][9];
                    //定级pmax
                    expB313cRow.定级Pmax3cZ0 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_Z[0][9];
                    expB313cRow.定级Pmax3cZ1 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_Z[1][9];
                    expB313cRow.定级Pmax3cF0 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_F[0][9];
                    expB313cRow.定级Pmax3cF1 = DAL_ExpDQ.ExpData_KFY.WY_DJPmax_F[1][9];

                    B313cTableAdapter.Update(B313cTable);
                    B313cTable.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B321

            //保存试验设置信息至B321抗风压定级组1相对挠度数据
            //若编号在表中不存在，则新建
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
                    expB321Row.定级变形0Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[0][0];
                    expB321Row.定级变形100Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[1][0];
                    expB321Row.定级变形200Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[2][0];
                    expB321Row.定级变形300Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[3][0];
                    expB321Row.定级变形400Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[4][0];
                    expB321Row.定级变形500Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[5][0];
                    expB321Row.定级变形600Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[6][0];
                    expB321Row.定级变形700Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[7][0];
                    expB321Row.定级变形800Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[8][0];
                    expB321Row.定级变形900Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[9][0];
                    expB321Row.定级变形1000Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[10][0];
                    expB321Row.定级变形1100Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[11][0];
                    expB321Row.定级变形1200Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[12][0];
                    expB321Row.定级变形1300Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[13][0];
                    expB321Row.定级变形1400Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[14][0];
                    expB321Row.定级变形1500Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[15][0];
                    expB321Row.定级变形1600Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[16][0];
                    expB321Row.定级变形1700Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[17][0];
                    expB321Row.定级变形1800Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[18][0];
                    expB321Row.定级变形1900Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[19][0];
                    expB321Row.定级变形2000Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[20][0];
                    //定级P1负
                    expB321Row.定级变形负0Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[0][0];
                    expB321Row.定级变形负100Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[1][0];
                    expB321Row.定级变形负200Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[2][0];
                    expB321Row.定级变形负300Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[3][0];
                    expB321Row.定级变形负400Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[4][0];
                    expB321Row.定级变形负500Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[5][0];
                    expB321Row.定级变形负600Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[6][0];
                    expB321Row.定级变形负700Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[7][0];
                    expB321Row.定级变形负800Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[8][0];
                    expB321Row.定级变形负900Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[9][0];
                    expB321Row.定级变形负1000Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[10][0];
                    expB321Row.定级变形负1100Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[11][0];
                    expB321Row.定级变形负1200Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[12][0];
                    expB321Row.定级变形负1300Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[13][0];
                    expB321Row.定级变形负1400Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[14][0];
                    expB321Row.定级变形负1500Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[15][0];
                    expB321Row.定级变形负1600Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[16][0];
                    expB321Row.定级变形负1700Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[17][0];
                    expB321Row.定级变形负1800Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[18][0];
                    expB321Row.定级变形负1900Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[19][0];
                    expB321Row.定级变形负2000Pa相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[20][0];
                    //定级P3
                    expB321Row.定级P3前相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJP3_Z[0][0];
                    expB321Row.定级负P3前相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJP3_F[0][0];
                    expB321Row.定级P3相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJP3_Z[1][0];
                    expB321Row.定级负P3相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJP3_F[1][0];
                    //定级Pmax
                    expB321Row.定级Pmax前相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJPmax_Z[0][0];
                    expB321Row.定级负Pmax前相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJPmax_F[0][0];
                    expB321Row.定级Pmax相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJPmax_Z[1][0];
                    expB321Row.定级负Pmax相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_DJPmax_F[1][0];

                    B321TableAdapter.Update(B321Table);
                    B321Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B322

            //保存试验设置信息至B322抗风压定级组2相对挠度数据
            //若编号在表中不存在，则新建
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
                    expB322Row.定级变形0Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[0][1];
                    expB322Row.定级变形100Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[1][1];
                    expB322Row.定级变形200Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[2][1];
                    expB322Row.定级变形300Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[3][1];
                    expB322Row.定级变形400Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[4][1];
                    expB322Row.定级变形500Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[5][1];
                    expB322Row.定级变形600Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[6][1];
                    expB322Row.定级变形700Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[7][1];
                    expB322Row.定级变形800Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[8][1];
                    expB322Row.定级变形900Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[9][1];
                    expB322Row.定级变形1000Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[10][1];
                    expB322Row.定级变形1100Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[11][1];
                    expB322Row.定级变形1200Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[12][1];
                    expB322Row.定级变形1300Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[13][1];
                    expB322Row.定级变形1400Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[14][1];
                    expB322Row.定级变形1500Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[15][1];
                    expB322Row.定级变形1600Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[16][1];
                    expB322Row.定级变形1700Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[17][1];
                    expB322Row.定级变形1800Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[18][1];
                    expB322Row.定级变形1900Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[19][1];
                    expB322Row.定级变形2000Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[20][1];
                    //定级P1负
                    expB322Row.定级变形负0Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[0][1];
                    expB322Row.定级变形负100Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[1][1];
                    expB322Row.定级变形负200Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[2][1];
                    expB322Row.定级变形负300Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[3][1];
                    expB322Row.定级变形负400Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[4][1];
                    expB322Row.定级变形负500Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[5][1];
                    expB322Row.定级变形负600Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[6][1];
                    expB322Row.定级变形负700Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[7][1];
                    expB322Row.定级变形负800Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[8][1];
                    expB322Row.定级变形负900Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[9][1];
                    expB322Row.定级变形负1000Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[10][1];
                    expB322Row.定级变形负1100Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[11][1];
                    expB322Row.定级变形负1200Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[12][1];
                    expB322Row.定级变形负1300Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[13][1];
                    expB322Row.定级变形负1400Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[14][1];
                    expB322Row.定级变形负1500Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[15][1];
                    expB322Row.定级变形负1600Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[16][1];
                    expB322Row.定级变形负1700Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[17][1];
                    expB322Row.定级变形负1800Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[18][1];
                    expB322Row.定级变形负1900Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[19][1];
                    expB322Row.定级变形负2000Pa相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[20][1];
                    //定级P3
                    expB322Row.定级P3前相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJP3_Z[0][1];
                    expB322Row.定级负P3前相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJP3_F[0][1];
                    expB322Row.定级P3相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJP3_Z[1][1];
                    expB322Row.定级负P3相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJP3_F[1][1];
                    //定级Pmax
                    expB322Row.定级Pmax前相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJPmax_Z[0][1];
                    expB322Row.定级负Pmax前相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJPmax_F[0][1];
                    expB322Row.定级Pmax相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJPmax_Z[1][1];
                    expB322Row.定级负Pmax相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_DJPmax_F[1][1];

                    B322TableAdapter.Update(B322Table);
                    B322Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B323

            //保存试验设置信息至B323抗风压定级组1相对挠度数据
            //若编号在表中不存在，则新建
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
                    expB323Row.定级变形0Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[0][2];
                    expB323Row.定级变形100Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[1][2];
                    expB323Row.定级变形200Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[2][2];
                    expB323Row.定级变形300Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[3][2];
                    expB323Row.定级变形400Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[4][2];
                    expB323Row.定级变形500Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[5][2];
                    expB323Row.定级变形600Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[6][2];
                    expB323Row.定级变形700Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[7][2];
                    expB323Row.定级变形800Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[8][2];
                    expB323Row.定级变形900Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[9][2];
                    expB323Row.定级变形1000Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[10][2];
                    expB323Row.定级变形1100Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[11][2];
                    expB323Row.定级变形1200Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[12][2];
                    expB323Row.定级变形1300Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[13][2];
                    expB323Row.定级变形1400Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[14][2];
                    expB323Row.定级变形1500Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[15][2];
                    expB323Row.定级变形1600Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[16][2];
                    expB323Row.定级变形1700Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[17][2];
                    expB323Row.定级变形1800Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[18][2];
                    expB323Row.定级变形1900Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[19][2];
                    expB323Row.定级变形2000Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_Z[20][2];
                    //定级P1负
                    expB323Row.定级变形负0Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[0][2];
                    expB323Row.定级变形负100Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[1][2];
                    expB323Row.定级变形负200Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[2][2];
                    expB323Row.定级变形负300Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[3][2];
                    expB323Row.定级变形负400Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[4][2];
                    expB323Row.定级变形负500Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[5][2];
                    expB323Row.定级变形负600Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[6][2];
                    expB323Row.定级变形负700Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[7][2];
                    expB323Row.定级变形负800Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[8][2];
                    expB323Row.定级变形负900Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[9][2];
                    expB323Row.定级变形负1000Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[10][2];
                    expB323Row.定级变形负1100Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[11][2];
                    expB323Row.定级变形负1200Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[12][2];
                    expB323Row.定级变形负1300Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[13][2];
                    expB323Row.定级变形负1400Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[14][2];
                    expB323Row.定级变形负1500Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[15][2];
                    expB323Row.定级变形负1600Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[16][2];
                    expB323Row.定级变形负1700Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[17][2];
                    expB323Row.定级变形负1800Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[18][2];
                    expB323Row.定级变形负1900Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[19][2];
                    expB323Row.定级变形负2000Pa相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJBX_F[20][2];
                    //定级P3
                    expB323Row.定级P3前相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJP3_Z[0][2];
                    expB323Row.定级负P3前相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJP3_F[0][2];
                    expB323Row.定级P3相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJP3_Z[1][2];
                    expB323Row.定级负P3相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJP3_F[1][2];
                    //定级Pmax
                    expB323Row.定级Pmax前相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJPmax_Z[0][2];
                    expB323Row.定级负Pmax前相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJPmax_F[0][2];
                    expB323Row.定级Pmax相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJPmax_Z[1][2];
                    expB323Row.定级负Pmax相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_DJPmax_F[1][2];

                    B323TableAdapter.Update(B323Table);
                    B323Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B324

            //保存试验设置信息至B324抗风压定级相对挠度最大值数据
            //若编号在表中不存在，则新建
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
                    expB324Row.定级变形0Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[0];
                    expB324Row.定级变形100Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[1];
                    expB324Row.定级变形200Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[2];
                    expB324Row.定级变形300Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[3];
                    expB324Row.定级变形400Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[4];
                    expB324Row.定级变形500Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[5];
                    expB324Row.定级变形600Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[6];
                    expB324Row.定级变形700Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[7];
                    expB324Row.定级变形800Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[8];
                    expB324Row.定级变形900Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[9];
                    expB324Row.定级变形1000Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[10];
                    expB324Row.定级变形1100Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[11];
                    expB324Row.定级变形1200Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[12];
                    expB324Row.定级变形1300Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[13];
                    expB324Row.定级变形1400Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[14];
                    expB324Row.定级变形1500Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[15];
                    expB324Row.定级变形1600Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[16];
                    expB324Row.定级变形1700Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[17];
                    expB324Row.定级变形1800Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[18];
                    expB324Row.定级变形1900Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[19];
                    expB324Row.定级变形2000Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_Z[20];
                    //定级P1负
                    expB324Row.定级变形负0Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[0];
                    expB324Row.定级变形负100Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[1];
                    expB324Row.定级变形负200Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[2];
                    expB324Row.定级变形负300Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[3];
                    expB324Row.定级变形负400Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[4];
                    expB324Row.定级变形负500Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[5];
                    expB324Row.定级变形负600Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[6];
                    expB324Row.定级变形负700Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[7];
                    expB324Row.定级变形负800Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[8];
                    expB324Row.定级变形负900Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[9];
                    expB324Row.定级变形负1000Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[10];
                    expB324Row.定级变形负1100Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[11];
                    expB324Row.定级变形负1200Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[12];
                    expB324Row.定级变形负1300Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[13];
                    expB324Row.定级变形负1400Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[14];
                    expB324Row.定级变形负1500Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[15];
                    expB324Row.定级变形负1600Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[16];
                    expB324Row.定级变形负1700Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[17];
                    expB324Row.定级变形负1800Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[18];
                    expB324Row.定级变形负1900Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[19];
                    expB324Row.定级变形负2000Pa相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJBX_F[20];
                    //定级P3
                    expB324Row.定级P3相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJp3_Z;
                    expB324Row.定级负P3相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJp3_F;
                    //定级Pmax
                    expB324Row.定级Pmax相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJpmax_Z;
                    expB324Row.定级负Pmax相对挠度max = DAL_ExpDQ.ExpData_KFY.XDND_Max_DJpmax_F;

                    B324TableAdapter.Update(B324Table);
                    B324Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B331

            //保存试验设置信息至B331抗风压定级组1挠度数据
            //若编号在表中不存在，则新建
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
                    expB331Row.定级变形0Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[0][0];
                    expB331Row.定级变形100Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[1][0];
                    expB331Row.定级变形200Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[2][0];
                    expB331Row.定级变形300Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[3][0];
                    expB331Row.定级变形400Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[4][0];
                    expB331Row.定级变形500Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[5][0];
                    expB331Row.定级变形600Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[6][0];
                    expB331Row.定级变形700Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[7][0];
                    expB331Row.定级变形800Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[8][0];
                    expB331Row.定级变形900Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[9][0];
                    expB331Row.定级变形1000Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[10][0];
                    expB331Row.定级变形1100Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[11][0];
                    expB331Row.定级变形1200Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[12][0];
                    expB331Row.定级变形1300Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[13][0];
                    expB331Row.定级变形1400Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[14][0];
                    expB331Row.定级变形1500Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[15][0];
                    expB331Row.定级变形1600Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[16][0];
                    expB331Row.定级变形1700Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[17][0];
                    expB331Row.定级变形1800Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[18][0];
                    expB331Row.定级变形1900Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[19][0];
                    expB331Row.定级变形2000Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[20][0];
                    //定级P1负
                    expB331Row.定级变形负0Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[0][0];
                    expB331Row.定级变形负100Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[1][0];
                    expB331Row.定级变形负200Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[2][0];
                    expB331Row.定级变形负300Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[3][0];
                    expB331Row.定级变形负400Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[4][0];
                    expB331Row.定级变形负500Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[5][0];
                    expB331Row.定级变形负600Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[6][0];
                    expB331Row.定级变形负700Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[7][0];
                    expB331Row.定级变形负800Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[8][0];
                    expB331Row.定级变形负900Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[9][0];
                    expB331Row.定级变形负1000Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[10][0];
                    expB331Row.定级变形负1100Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[11][0];
                    expB331Row.定级变形负1200Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[12][0];
                    expB331Row.定级变形负1300Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[13][0];
                    expB331Row.定级变形负1400Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[14][0];
                    expB331Row.定级变形负1500Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[15][0];
                    expB331Row.定级变形负1600Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[16][0];
                    expB331Row.定级变形负1700Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[17][0];
                    expB331Row.定级变形负1800Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[18][0];
                    expB331Row.定级变形负1900Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[19][0];
                    expB331Row.定级变形负2000Pa挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[20][0];
                    //定级P3
                    expB331Row.定级P3前挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJP3_Z[0][0];
                    expB331Row.定级负P3前挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJP3_F[0][0];
                    expB331Row.定级P3挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJP3_Z[1][0];
                    expB331Row.定级负P3挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJP3_F[1][0];
                    //定级Pmax
                    expB331Row.定级Pmax前挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJPmax_Z[0][0];
                    expB331Row.定级负Pmax前挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJPmax_F[0][0];
                    expB331Row.定级Pmax挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJPmax_Z[1][0];
                    expB331Row.定级负Pmax挠度1 = DAL_ExpDQ.ExpData_KFY.ND_DJPmax_F[1][0];

                    B331TableAdapter.Update(B331Table);
                    B331Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B332

            //保存试验设置信息至B332抗风压定级组2挠度数据
            //若编号在表中不存在，则新建
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
                    expB332Row.定级变形0Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[0][1];
                    expB332Row.定级变形100Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[1][1];
                    expB332Row.定级变形200Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[2][1];
                    expB332Row.定级变形300Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[3][1];
                    expB332Row.定级变形400Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[4][1];
                    expB332Row.定级变形500Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[5][1];
                    expB332Row.定级变形600Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[6][1];
                    expB332Row.定级变形700Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[7][1];
                    expB332Row.定级变形800Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[8][1];
                    expB332Row.定级变形900Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[9][1];
                    expB332Row.定级变形1000Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[10][1];
                    expB332Row.定级变形1100Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[11][1];
                    expB332Row.定级变形1200Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[12][1];
                    expB332Row.定级变形1300Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[13][1];
                    expB332Row.定级变形1400Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[14][1];
                    expB332Row.定级变形1500Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[15][1];
                    expB332Row.定级变形1600Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[16][1];
                    expB332Row.定级变形1700Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[17][1];
                    expB332Row.定级变形1800Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[18][1];
                    expB332Row.定级变形1900Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[19][1];
                    expB332Row.定级变形2000Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[20][1];
                    //定级P1负
                    expB332Row.定级变形负0Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[0][1];
                    expB332Row.定级变形负100Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[1][1];
                    expB332Row.定级变形负200Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[2][1];
                    expB332Row.定级变形负300Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[3][1];
                    expB332Row.定级变形负400Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[4][1];
                    expB332Row.定级变形负500Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[5][1];
                    expB332Row.定级变形负600Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[6][1];
                    expB332Row.定级变形负700Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[7][1];
                    expB332Row.定级变形负800Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[8][1];
                    expB332Row.定级变形负900Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[9][1];
                    expB332Row.定级变形负1000Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[10][1];
                    expB332Row.定级变形负1100Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[11][1];
                    expB332Row.定级变形负1200Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[12][1];
                    expB332Row.定级变形负1300Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[13][1];
                    expB332Row.定级变形负1400Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[14][1];
                    expB332Row.定级变形负1500Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[15][1];
                    expB332Row.定级变形负1600Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[16][1];
                    expB332Row.定级变形负1700Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[17][1];
                    expB332Row.定级变形负1800Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[18][1];
                    expB332Row.定级变形负1900Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[19][1];
                    expB332Row.定级变形负2000Pa挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[20][1];
                    //定级P3
                    expB332Row.定级P3前挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJP3_Z[0][1];
                    expB332Row.定级负P3前挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJP3_F[0][1];
                    expB332Row.定级P3挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJP3_Z[1][1];
                    expB332Row.定级负P3挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJP3_F[1][1];
                    //定级Pmax
                    expB332Row.定级Pmax前挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJPmax_Z[0][1];
                    expB332Row.定级负Pmax前挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJPmax_F[0][1];
                    expB332Row.定级Pmax挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJPmax_Z[1][1];
                    expB332Row.定级负Pmax挠度2 = DAL_ExpDQ.ExpData_KFY.ND_DJPmax_F[1][1];

                    B332TableAdapter.Update(B332Table);
                    B332Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B333

            //保存试验设置信息至B333抗风压定级组1挠度数据
            //若编号在表中不存在，则新建
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
                    expB333Row.定级变形0Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[0][2];
                    expB333Row.定级变形100Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[1][2];
                    expB333Row.定级变形200Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[2][2];
                    expB333Row.定级变形300Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[3][2];
                    expB333Row.定级变形400Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[4][2];
                    expB333Row.定级变形500Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[5][2];
                    expB333Row.定级变形600Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[6][2];
                    expB333Row.定级变形700Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[7][2];
                    expB333Row.定级变形800Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[8][2];
                    expB333Row.定级变形900Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[9][2];
                    expB333Row.定级变形1000Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[10][2];
                    expB333Row.定级变形1100Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[11][2];
                    expB333Row.定级变形1200Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[12][2];
                    expB333Row.定级变形1300Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[13][2];
                    expB333Row.定级变形1400Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[14][2];
                    expB333Row.定级变形1500Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[15][2];
                    expB333Row.定级变形1600Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[16][2];
                    expB333Row.定级变形1700Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[17][2];
                    expB333Row.定级变形1800Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[18][2];
                    expB333Row.定级变形1900Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[19][2];
                    expB333Row.定级变形2000Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_Z[20][2];
                    //定级P1负
                    expB333Row.定级变形负0Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[0][2];
                    expB333Row.定级变形负100Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[1][2];
                    expB333Row.定级变形负200Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[2][2];
                    expB333Row.定级变形负300Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[3][2];
                    expB333Row.定级变形负400Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[4][2];
                    expB333Row.定级变形负500Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[5][2];
                    expB333Row.定级变形负600Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[6][2];
                    expB333Row.定级变形负700Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[7][2];
                    expB333Row.定级变形负800Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[8][2];
                    expB333Row.定级变形负900Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[9][2];
                    expB333Row.定级变形负1000Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[10][2];
                    expB333Row.定级变形负1100Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[11][2];
                    expB333Row.定级变形负1200Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[12][2];
                    expB333Row.定级变形负1300Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[13][2];
                    expB333Row.定级变形负1400Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[14][2];
                    expB333Row.定级变形负1500Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[15][2];
                    expB333Row.定级变形负1600Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[16][2];
                    expB333Row.定级变形负1700Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[17][2];
                    expB333Row.定级变形负1800Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[18][2];
                    expB333Row.定级变形负1900Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[19][2];
                    expB333Row.定级变形负2000Pa挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJBX_F[20][2];
                    //定级P3
                    expB333Row.定级P3前挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJP3_Z[0][2];
                    expB333Row.定级负P3前挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJP3_F[0][2];
                    expB333Row.定级P3挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJP3_Z[1][2];
                    expB333Row.定级负P3挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJP3_F[1][2];
                    //定级Pmax
                    expB333Row.定级Pmax前挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJPmax_Z[0][2];
                    expB333Row.定级负Pmax前挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJPmax_F[0][2];
                    expB333Row.定级Pmax挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJPmax_Z[1][2];
                    expB333Row.定级负Pmax挠度3 = DAL_ExpDQ.ExpData_KFY.ND_DJPmax_F[1][2];

                    B333TableAdapter.Update(B333Table);
                    B333Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B334

            //保存试验设置信息至B334抗风压定级挠度最大值数据
            //若编号在表中不存在，则新建
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
                    expB334Row.定级变形0Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[0];
                    expB334Row.定级变形100Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[1];
                    expB334Row.定级变形200Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[2];
                    expB334Row.定级变形300Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[3];
                    expB334Row.定级变形400Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[4];
                    expB334Row.定级变形500Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[5];
                    expB334Row.定级变形600Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[6];
                    expB334Row.定级变形700Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[7];
                    expB334Row.定级变形800Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[8];
                    expB334Row.定级变形900Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[9];
                    expB334Row.定级变形1000Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[10];
                    expB334Row.定级变形1100Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[11];
                    expB334Row.定级变形1200Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[12];
                    expB334Row.定级变形1300Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[13];
                    expB334Row.定级变形1400Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[14];
                    expB334Row.定级变形1500Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[15];
                    expB334Row.定级变形1600Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[16];
                    expB334Row.定级变形1700Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[17];
                    expB334Row.定级变形1800Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[18];
                    expB334Row.定级变形1900Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[19];
                    expB334Row.定级变形2000Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_Z[20];
                    //定级P1负
                    expB334Row.定级变形负0Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[0];
                    expB334Row.定级变形负100Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[1];
                    expB334Row.定级变形负200Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[2];
                    expB334Row.定级变形负300Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[3];
                    expB334Row.定级变形负400Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[4];
                    expB334Row.定级变形负500Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[5];
                    expB334Row.定级变形负600Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[6];
                    expB334Row.定级变形负700Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[7];
                    expB334Row.定级变形负800Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[8];
                    expB334Row.定级变形负900Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[9];
                    expB334Row.定级变形负1000Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[10];
                    expB334Row.定级变形负1100Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[11];
                    expB334Row.定级变形负1200Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[12];
                    expB334Row.定级变形负1300Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[13];
                    expB334Row.定级变形负1400Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[14];
                    expB334Row.定级变形负1500Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[15];
                    expB334Row.定级变形负1600Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[16];
                    expB334Row.定级变形负1700Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[17];
                    expB334Row.定级变形负1800Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[18];
                    expB334Row.定级变形负1900Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[19];
                    expB334Row.定级变形负2000Pa挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJBX_F[20];
                    //定级P3
                    expB334Row.定级P3挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJp3_Z;
                    expB334Row.定级负P3挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJp3_F;
                    //定级Pfmax
                    expB334Row.定级Pmax挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJpmax_Z;
                    expB334Row.定级负Pmax挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_DJpmax_F;

                    B334TableAdapter.Update(B334Table);
                    B334Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B340

            //保存试验设置信息至B340抗风压定级挠度最大值数据
            //若编号在表中不存在，则新建
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
                    expB340Row.定级变形100Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_Z[0];
                    expB340Row.定级变形200Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_Z[1];
                    expB340Row.定级变形300Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_Z[2];
                    expB340Row.定级变形400Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_Z[3];
                    expB340Row.定级变形500Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_Z[4];
                    expB340Row.定级变形600Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_Z[5];
                    expB340Row.定级变形700Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_Z[6];
                    expB340Row.定级变形800Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_Z[7];
                    expB340Row.定级变形900Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_Z[8];
                    expB340Row.定级变形1000Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_Z[9];
                    expB340Row.定级变形1100Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_Z[10];
                    expB340Row.定级变形1200Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_Z[11];
                    expB340Row.定级变形1300Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_Z[12];
                    expB340Row.定级变形1400Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_Z[13];
                    expB340Row.定级变形1500Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_Z[14];
                    expB340Row.定级变形1600Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_Z[15];
                    expB340Row.定级变形1700Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_Z[16];
                    expB340Row.定级变形1800Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_Z[17];
                    expB340Row.定级变形1900Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_Z[18];
                    expB340Row.定级变形2000Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_Z[19];
                    expB340Row.定级变形负100Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_F[0];
                    expB340Row.定级变形负200Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_F[1];
                    expB340Row.定级变形负300Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_F[2];
                    expB340Row.定级变形负400Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_F[3];
                    expB340Row.定级变形负500Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_F[4];
                    expB340Row.定级变形负600Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_F[5];
                    expB340Row.定级变形负700Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_F[6];
                    expB340Row.定级变形负800Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_F[7];
                    expB340Row.定级变形负900Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_F[8];
                    expB340Row.定级变形负1000Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_F[9];
                    expB340Row.定级变形负1100Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_F[10];
                    expB340Row.定级变形负1200Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_F[11];
                    expB340Row.定级变形负1300Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_F[12];
                    expB340Row.定级变形负1400Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_F[13];
                    expB340Row.定级变形负1500Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_F[14];
                    expB340Row.定级变形负1600Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_F[15];
                    expB340Row.定级变形负1700Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_F[16];
                    expB340Row.定级变形负1800Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_F[17];
                    expB340Row.定级变形负1900Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_F[18];
                    expB340Row.定级变形负2000Pa损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJBX_F[19];
                    expB340Row.定级P2损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJP2_Z;
                    expB340Row.定级负P2损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJP2_F;
                    expB340Row.定级P3损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJP3_Z;
                    expB340Row.定级负P3损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJP3_F;
                    expB340Row.定级Pmax损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJPmax_Z;
                    expB340Row.定级负Pmax损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_DJPmax_F;

                    B340TableAdapter.Update(B340Table);
                    B340Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B350

            //保存试验设置信息至B350抗风压定级损坏说明数据
            //若编号在表中不存在，则新建
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
                    expB350Row.定级变形100Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[0];
                    expB350Row.定级变形200Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[1];
                    expB350Row.定级变形300Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[2];
                    expB350Row.定级变形400Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[3];
                    expB350Row.定级变形500Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[4];
                    expB350Row.定级变形600Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[5];
                    expB350Row.定级变形700Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[6];
                    expB350Row.定级变形800Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[7];
                    expB350Row.定级变形900Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[8];
                    expB350Row.定级变形1000Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[9];
                    expB350Row.定级变形1100Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[10];
                    expB350Row.定级变形1200Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[11];
                    expB350Row.定级变形1300Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[12];
                    expB350Row.定级变形1400Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[13];
                    expB350Row.定级变形1500Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[14];
                    expB350Row.定级变形1600Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[15];
                    expB350Row.定级变形1700Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[16];
                    expB350Row.定级变形1800Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[17];
                    expB350Row.定级变形1900Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[18];
                    expB350Row.定级变形2000Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_Z[19];
                    expB350Row.定级变形负100Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_F[0];
                    expB350Row.定级变形负200Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_F[1];
                    expB350Row.定级变形负300Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_F[2];
                    expB350Row.定级变形负400Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_F[3];
                    expB350Row.定级变形负500Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_F[4];
                    expB350Row.定级变形负600Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_F[5];
                    expB350Row.定级变形负700Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_F[6];
                    expB350Row.定级变形负800Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_F[7];
                    expB350Row.定级变形负900Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_F[8];
                    expB350Row.定级变形负1000Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_F[9];
                    expB350Row.定级变形负1100Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_F[10];
                    expB350Row.定级变形负1200Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_F[11];
                    expB350Row.定级变形负1300Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_F[12];
                    expB350Row.定级变形负1400Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_F[13];
                    expB350Row.定级变形负1500Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_F[14];
                    expB350Row.定级变形负1600Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_F[15];
                    expB350Row.定级变形负1700Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_F[16];
                    expB350Row.定级变形负1800Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_F[17];
                    expB350Row.定级变形负1900Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_F[18];
                    expB350Row.定级变形负2000Pa损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJBX_F[19];
                    expB350Row.定级P2损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJP2_Z;
                    expB350Row.定级负P2损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJP2_F;
                    expB350Row.定级P3损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJP3_Z;
                    expB350Row.定级负P3损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJP3_F;
                    expB350Row.定级Pmax损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJPmax_Z;
                    expB350Row.定级负Pmax损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_DJPmax_F;

                    B350TableAdapter.Update(B350Table);
                    B350Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B361

            //保存试验设置信息至B361抗风压组1工程位移数据
            //若编号在表中不存在，则新建
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
                    expB361Row.工程变形1aZ00 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[0][0];
                    expB361Row.工程变形1bZ00 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[0][1];
                    expB361Row.工程变形1cZ00 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[0][2];
                    expB361Row.工程变形1dZ00 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[0][3];
                    expB361Row.工程变形1aZ01 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[1][0];
                    expB361Row.工程变形1bZ01 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[1][1];
                    expB361Row.工程变形1cZ01 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[1][2];
                    expB361Row.工程变形1dZ01 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[1][3];
                    expB361Row.工程变形1aZ02 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[2][0];
                    expB361Row.工程变形1bZ02 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[2][1];
                    expB361Row.工程变形1cZ02 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[2][2];
                    expB361Row.工程变形1dZ02 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[2][3];
                    expB361Row.工程变形1aZ03 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[3][0];
                    expB361Row.工程变形1bZ03 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[3][1];
                    expB361Row.工程变形1cZ03 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[3][2];
                    expB361Row.工程变形1dZ03 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[3][3];
                    expB361Row.工程变形1aZ04 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[4][0];
                    expB361Row.工程变形1bZ04 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[4][1];
                    expB361Row.工程变形1cZ04 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[4][2];
                    expB361Row.工程变形1dZ04 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[4][3];
                    expB361Row.工程变形1aF00 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[0][0];
                    expB361Row.工程变形1bF00 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[0][1];
                    expB361Row.工程变形1cF00 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[0][2];
                    expB361Row.工程变形1dF00 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[0][3];
                    expB361Row.工程变形1aF01 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[1][0];
                    expB361Row.工程变形1bF01 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[1][1];
                    expB361Row.工程变形1cF01 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[1][2];
                    expB361Row.工程变形1dF01 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[1][3];
                    expB361Row.工程变形1aF02 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[2][0];
                    expB361Row.工程变形1bF02 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[2][1];
                    expB361Row.工程变形1cF02 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[2][2];
                    expB361Row.工程变形1dF02 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[2][3];
                    expB361Row.工程变形1aF03 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[3][0];
                    expB361Row.工程变形1bF03 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[3][1];
                    expB361Row.工程变形1cF03 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[3][2];
                    expB361Row.工程变形1dF03 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[3][3];
                    expB361Row.工程变形1aF04 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[4][0];
                    expB361Row.工程变形1bF04 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[4][1];
                    expB361Row.工程变形1cF04 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[4][2];
                    expB361Row.工程变形1dF04 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[4][3];
                    expB361Row.工程P31aZ0 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_Z[0][0];
                    expB361Row.工程P31bZ0 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_Z[0][1];
                    expB361Row.工程P31cZ0 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_Z[0][2];
                    expB361Row.工程P31dZ0 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_Z[0][3];
                    expB361Row.工程P31aZ1 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_Z[1][0];
                    expB361Row.工程P31bZ1 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_Z[1][1];
                    expB361Row.工程P31cZ1 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_Z[1][2];
                    expB361Row.工程P31dZ1 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_Z[1][3];
                    expB361Row.工程P31aF0 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_F[0][0];
                    expB361Row.工程P31bF0 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_F[0][1];
                    expB361Row.工程P31cF0 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_F[0][2];
                    expB361Row.工程P31dF0 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_F[0][3];
                    expB361Row.工程P31aF1 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_F[1][0];
                    expB361Row.工程P31bF1 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_F[1][1];
                    expB361Row.工程P31cF1 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_F[1][2];
                    expB361Row.工程P31dF1 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_F[1][3];
                    expB361Row.工程Pmax1aZ0 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_Z[0][0];
                    expB361Row.工程Pmax1bZ0 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_Z[0][1];
                    expB361Row.工程Pmax1cZ0 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_Z[0][2];
                    expB361Row.工程Pmax1dZ0 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_Z[0][3];
                    expB361Row.工程Pmax1aZ1 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_Z[1][0];
                    expB361Row.工程Pmax1bZ1 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_Z[1][1];
                    expB361Row.工程Pmax1cZ1 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_Z[1][2];
                    expB361Row.工程Pmax1dZ1 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_Z[1][3];
                    expB361Row.工程Pmax1aF0 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_F[0][0];
                    expB361Row.工程Pmax1bF0 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_F[0][1];
                    expB361Row.工程Pmax1cF0 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_F[0][2];
                    expB361Row.工程Pmax1dF0 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_F[0][3];
                    expB361Row.工程Pmax1aF1 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_F[1][0];
                    expB361Row.工程Pmax1bF1 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_F[1][1];
                    expB361Row.工程Pmax1cF1 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_F[1][2];
                    expB361Row.工程Pmax1dF1 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_F[1][3];

                    B361TableAdapter.Update(B361Table);
                    B361Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B362

            //保存试验设置信息至B362抗风压组2工程位移数据
            //若编号在表中不存在，则新建
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
                    expB362Row.工程变形2aZ00 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[0][4];
                    expB362Row.工程变形2bZ00 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[0][5];
                    expB362Row.工程变形2cZ00 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[0][6];
                    expB362Row.工程变形2aZ01 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[1][4];
                    expB362Row.工程变形2bZ01 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[1][5];
                    expB362Row.工程变形2cZ01 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[1][6];
                    expB362Row.工程变形2aZ02 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[2][4];
                    expB362Row.工程变形2bZ02 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[2][5];
                    expB362Row.工程变形2cZ02 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[2][6];
                    expB362Row.工程变形2aZ03 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[3][4];
                    expB362Row.工程变形2bZ03 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[3][5];
                    expB362Row.工程变形2cZ03 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[3][6];
                    expB362Row.工程变形2aZ04 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[4][4];
                    expB362Row.工程变形2bZ04 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[4][5];
                    expB362Row.工程变形2cZ04 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[4][6];
                    expB362Row.工程变形2aF00 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[0][4];
                    expB362Row.工程变形2bF00 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[0][5];
                    expB362Row.工程变形2cF00 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[0][6];
                    expB362Row.工程变形2aF01 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[1][4];
                    expB362Row.工程变形2bF01 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[1][5];
                    expB362Row.工程变形2cF01 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[1][6];
                    expB362Row.工程变形2aF02 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[2][4];
                    expB362Row.工程变形2bF02 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[2][5];
                    expB362Row.工程变形2cF02 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[2][6];
                    expB362Row.工程变形2aF03 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[3][4];
                    expB362Row.工程变形2bF03 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[3][5];
                    expB362Row.工程变形2cF03 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[3][6];
                    expB362Row.工程变形2aF04 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[4][4];
                    expB362Row.工程变形2bF04 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[4][5];
                    expB362Row.工程变形2cF04 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[4][6];
                    expB362Row.工程P32aZ0 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_Z[0][4];
                    expB362Row.工程P32bZ0 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_Z[0][5];
                    expB362Row.工程P32cZ0 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_Z[0][6];
                    expB362Row.工程P32aZ1 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_Z[1][4];
                    expB362Row.工程P32bZ1 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_Z[1][5];
                    expB362Row.工程P32cZ1 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_Z[1][6];
                    expB362Row.工程P32aF0 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_F[0][4];
                    expB362Row.工程P32bF0 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_F[0][5];
                    expB362Row.工程P32cF0 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_F[0][6];
                    expB362Row.工程P32aF1 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_F[1][4];
                    expB362Row.工程P32bF1 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_F[1][5];
                    expB362Row.工程P32cF1 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_F[1][6];
                    expB362Row.工程Pmax2aZ0 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_Z[0][4];
                    expB362Row.工程Pmax2bZ0 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_Z[0][5];
                    expB362Row.工程Pmax2cZ0 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_Z[0][6];
                    expB362Row.工程Pmax2aZ1 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_Z[1][4];
                    expB362Row.工程Pmax2bZ1 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_Z[1][5];
                    expB362Row.工程Pmax2cZ1 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_Z[1][6];
                    expB362Row.工程Pmax2aF0 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_F[0][4];
                    expB362Row.工程Pmax2bF0 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_F[0][5];
                    expB362Row.工程Pmax2cF0 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_F[0][6];
                    expB362Row.工程Pmax2aF1 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_F[1][4];
                    expB362Row.工程Pmax2bF1 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_F[1][5];
                    expB362Row.工程Pmax2cF1 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_F[1][6];

                    B362TableAdapter.Update(B362Table);
                    B362Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B363

            //保存试验设置信息至B363抗风压组3工程位移数据
            //若编号在表中不存在，则新建
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
                    expB363Row.工程变形3aZ00 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[0][7];
                    expB363Row.工程变形3bZ00 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[0][8];
                    expB363Row.工程变形3cZ00 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[0][9];
                    expB363Row.工程变形3aZ01 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[1][7];
                    expB363Row.工程变形3bZ01 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[1][8];
                    expB363Row.工程变形3cZ01 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[1][9];
                    expB363Row.工程变形3aZ02 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[2][7];
                    expB363Row.工程变形3bZ02 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[2][8];
                    expB363Row.工程变形3cZ02 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[2][9];
                    expB363Row.工程变形3aZ03 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[3][7];
                    expB363Row.工程变形3bZ03 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[3][8];
                    expB363Row.工程变形3cZ03 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[3][9];
                    expB363Row.工程变形3aZ04 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[4][7];
                    expB363Row.工程变形3bZ04 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[4][8];
                    expB363Row.工程变形3cZ04 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_Z[4][9];
                    expB363Row.工程变形3aF00 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[0][7];
                    expB363Row.工程变形3bF00 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[0][8];
                    expB363Row.工程变形3cF00 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[0][9];
                    expB363Row.工程变形3aF01 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[1][7];
                    expB363Row.工程变形3bF01 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[1][8];
                    expB363Row.工程变形3cF01 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[1][9];
                    expB363Row.工程变形3aF02 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[2][7];
                    expB363Row.工程变形3bF02 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[2][8];
                    expB363Row.工程变形3cF02 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[2][9];
                    expB363Row.工程变形3aF03 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[3][7];
                    expB363Row.工程变形3bF03 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[3][8];
                    expB363Row.工程变形3cF03 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[3][9];
                    expB363Row.工程变形3aF04 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[4][7];
                    expB363Row.工程变形3bF04 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[4][8];
                    expB363Row.工程变形3cF04 = DAL_ExpDQ.ExpData_KFY.WY_GCBX_F[4][9];
                    expB363Row.工程P33aZ0 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_Z[0][7];
                    expB363Row.工程P33bZ0 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_Z[0][8];
                    expB363Row.工程P33cZ0 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_Z[0][9];
                    expB363Row.工程P33aZ1 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_Z[1][7];
                    expB363Row.工程P33bZ1 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_Z[1][8];
                    expB363Row.工程P33cZ1 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_Z[1][9];
                    expB363Row.工程P33aF0 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_F[0][7];
                    expB363Row.工程P33bF0 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_F[0][8];
                    expB363Row.工程P33cF0 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_F[0][9];
                    expB363Row.工程P33aF1 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_F[1][7];
                    expB363Row.工程P33bF1 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_F[1][8];
                    expB363Row.工程P33cF1 = DAL_ExpDQ.ExpData_KFY.WY_GCP3_F[1][9];
                    expB363Row.工程Pmax3aZ0 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_Z[0][7];
                    expB363Row.工程Pmax3bZ0 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_Z[0][8];
                    expB363Row.工程Pmax3cZ0 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_Z[0][9];
                    expB363Row.工程Pmax3aZ1 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_Z[1][7];
                    expB363Row.工程Pmax3bZ1 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_Z[1][8];
                    expB363Row.工程Pmax3cZ1 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_Z[1][9];
                    expB363Row.工程Pmax3aF0 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_F[0][7];
                    expB363Row.工程Pmax3bF0 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_F[0][8];
                    expB363Row.工程Pmax3cF0 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_F[0][9];
                    expB363Row.工程Pmax3aF1 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_F[1][7];
                    expB363Row.工程Pmax3bF1 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_F[1][8];
                    expB363Row.工程Pmax3cF1 = DAL_ExpDQ.ExpData_KFY.WY_GCPmax_F[1][9];

                    B363TableAdapter.Update(B363Table);
                    B363Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B371

            //保存试验设置信息至B371抗风压工程相对挠度数据
            //若编号在表中不存在，则新建
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
                    expB371Row.工程变形0相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_Z[0][0];
                    expB371Row.工程变形10相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_Z[1][0];
                    expB371Row.工程变形20相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_Z[2][0];
                    expB371Row.工程变形30相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_Z[3][0];
                    expB371Row.工程变形40相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_Z[4][0];
                    expB371Row.工程变形负0相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_F[0][0];
                    expB371Row.工程变形负10相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_F[1][0];
                    expB371Row.工程变形负20相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_F[2][0];
                    expB371Row.工程变形负30相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_F[3][0];
                    expB371Row.工程变形负40相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_F[4][0];
                    expB371Row.工程变形0相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_Z[0][1];
                    expB371Row.工程变形10相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_Z[1][1];
                    expB371Row.工程变形20相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_Z[2][1];
                    expB371Row.工程变形30相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_Z[3][1];
                    expB371Row.工程变形40相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_Z[4][1];
                    expB371Row.工程变形负0相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_F[0][1];
                    expB371Row.工程变形负10相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_F[1][1];
                    expB371Row.工程变形负20相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_F[2][1];
                    expB371Row.工程变形负30相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_F[3][1];
                    expB371Row.工程变形负40相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_F[4][1];
                    expB371Row.工程变形0相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_Z[0][2];
                    expB371Row.工程变形10相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_Z[1][2];
                    expB371Row.工程变形20相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_Z[2][2];
                    expB371Row.工程变形30相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_Z[3][2];
                    expB371Row.工程变形40相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_Z[4][2];
                    expB371Row.工程变形负0相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_F[0][2];
                    expB371Row.工程变形负10相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_F[1][2];
                    expB371Row.工程变形负20相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_F[2][2];
                    expB371Row.工程变形负30相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_F[3][2];
                    expB371Row.工程变形负40相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_GCBX_F[4][2];
                    expB371Row.工程P3前相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_GCP3_Z[0][0];
                    expB371Row.工程P3相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_GCP3_Z[1][0];
                    expB371Row.工程负P3前相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_GCP3_F[0][0];
                    expB371Row.工程负P3相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_GCP3_F[1][0];
                    expB371Row.工程P3前相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_GCP3_Z[0][1];
                    expB371Row.工程P3相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_GCP3_Z[1][1];
                    expB371Row.工程负P3前相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_GCP3_F[0][1];
                    expB371Row.工程负P3相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_GCP3_F[1][1];
                    expB371Row.工程P3前相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_GCP3_Z[0][2];
                    expB371Row.工程P3相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_GCP3_Z[1][2];
                    expB371Row.工程负P3前相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_GCP3_F[0][2];
                    expB371Row.工程负P3相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_GCP3_F[1][2];
                    expB371Row.工程Pmax前相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_GCPmax_Z[0][0];
                    expB371Row.工程Pmax相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_GCPmax_Z[1][0];
                    expB371Row.工程负Pmax前相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_GCPmax_F[0][0];
                    expB371Row.工程负Pmax相对挠度1 = DAL_ExpDQ.ExpData_KFY.XDND_GCPmax_F[1][0];
                    expB371Row.工程Pmax前相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_GCPmax_Z[0][1];
                    expB371Row.工程Pmax相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_GCPmax_Z[1][1];
                    expB371Row.工程负Pmax前相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_GCPmax_F[0][1];
                    expB371Row.工程负Pmax相对挠度2 = DAL_ExpDQ.ExpData_KFY.XDND_GCPmax_F[1][1];
                    expB371Row.工程Pmax前相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_GCPmax_Z[0][2];
                    expB371Row.工程Pmax相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_GCPmax_Z[1][2];
                    expB371Row.工程负Pmax前相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_GCPmax_F[0][2];
                    expB371Row.工程负Pmax相对挠度3 = DAL_ExpDQ.ExpData_KFY.XDND_GCPmax_F[1][2];

                    B371TableAdapter.Update(B371Table);
                    B371Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B372

            //保存试验设置信息至B372抗风压工程相对挠度最大值数据
            //若编号在表中不存在，则新建
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
                    expB372Row.工程变形0相对挠度fmax = DAL_ExpDQ.ExpData_KFY.XDND_Max_GCBX_Z[0];
                    expB372Row.工程变形10相对挠度fmax = DAL_ExpDQ.ExpData_KFY.XDND_Max_GCBX_Z[1];
                    expB372Row.工程变形20相对挠度fmax = DAL_ExpDQ.ExpData_KFY.XDND_Max_GCBX_Z[2];
                    expB372Row.工程变形30相对挠度fmax = DAL_ExpDQ.ExpData_KFY.XDND_Max_GCBX_Z[3];
                    expB372Row.工程变形40相对挠度fmax = DAL_ExpDQ.ExpData_KFY.XDND_Max_GCBX_Z[4];
                    expB372Row.工程变形负0相对挠度fmax = DAL_ExpDQ.ExpData_KFY.XDND_Max_GCBX_F[0];
                    expB372Row.工程变形负10相对挠度fmax = DAL_ExpDQ.ExpData_KFY.XDND_Max_GCBX_F[1];
                    expB372Row.工程变形负20相对挠度fmax = DAL_ExpDQ.ExpData_KFY.XDND_Max_GCBX_F[2];
                    expB372Row.工程变形负30相对挠度fmax = DAL_ExpDQ.ExpData_KFY.XDND_Max_GCBX_F[3];
                    expB372Row.工程变形负40相对挠度fmax = DAL_ExpDQ.ExpData_KFY.XDND_Max_GCBX_F[4];
                    expB372Row.工程P3相对挠度fmax = DAL_ExpDQ.ExpData_KFY.XDND_Max_GCp3_Z;
                    expB372Row.工程负P3相对挠度fmax = DAL_ExpDQ.ExpData_KFY.XDND_Max_GCp3_F;
                    expB372Row.工程Pmax相对挠度fmax = DAL_ExpDQ.ExpData_KFY.XDND_Max_GCpmax_Z;
                    expB372Row.工程负Pmax相对挠度fmax = DAL_ExpDQ.ExpData_KFY.XDND_Max_GCpmax_F;

                    B372TableAdapter.Update(B372Table);
                    B372Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B381

            //保存试验设置信息至B381抗风压工程挠度数据
            //若编号在表中不存在，则新建
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
                    expB381Row.工程变形0挠度1 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_Z[0][0];
                    expB381Row.工程变形10挠度1 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_Z[1][0];
                    expB381Row.工程变形20挠度1 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_Z[2][0];
                    expB381Row.工程变形30挠度1 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_Z[3][0];
                    expB381Row.工程变形40挠度1 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_Z[4][0];
                    expB381Row.工程变形负0挠度1 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_F[0][0];
                    expB381Row.工程变形负10挠度1 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_F[1][0];
                    expB381Row.工程变形负20挠度1 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_F[2][0];
                    expB381Row.工程变形负30挠度1 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_F[3][0];
                    expB381Row.工程变形负40挠度1 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_F[4][0];
                    expB381Row.工程变形0挠度2 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_Z[0][1];
                    expB381Row.工程变形10挠度2 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_Z[1][1];
                    expB381Row.工程变形20挠度2 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_Z[2][1];
                    expB381Row.工程变形30挠度2 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_Z[3][1];
                    expB381Row.工程变形40挠度2 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_Z[4][1];
                    expB381Row.工程变形负0挠度2 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_F[0][1];
                    expB381Row.工程变形负10挠度2 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_F[1][1];
                    expB381Row.工程变形负20挠度2 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_F[2][1];
                    expB381Row.工程变形负30挠度2 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_F[3][1];
                    expB381Row.工程变形负40挠度2 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_F[4][1];
                    expB381Row.工程变形0挠度3 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_Z[0][2];
                    expB381Row.工程变形10挠度3 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_Z[1][2];
                    expB381Row.工程变形20挠度3 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_Z[2][2];
                    expB381Row.工程变形30挠度3 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_Z[3][2];
                    expB381Row.工程变形40挠度3 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_Z[4][2];
                    expB381Row.工程变形负0挠度3 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_F[0][2];
                    expB381Row.工程变形负10挠度3 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_F[1][2];
                    expB381Row.工程变形负20挠度3 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_F[2][2];
                    expB381Row.工程变形负30挠度3 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_F[3][2];
                    expB381Row.工程变形负40挠度3 = DAL_ExpDQ.ExpData_KFY.ND_GCBX_F[4][2];
                    expB381Row.工程P3前挠度1 = DAL_ExpDQ.ExpData_KFY.ND_GCP3_Z[0][0];
                    expB381Row.工程P3挠度1 = DAL_ExpDQ.ExpData_KFY.ND_GCP3_Z[1][0];
                    expB381Row.工程负P3前挠度1 = DAL_ExpDQ.ExpData_KFY.ND_GCP3_F[0][0];
                    expB381Row.工程负P3挠度1 = DAL_ExpDQ.ExpData_KFY.ND_GCP3_F[1][0];
                    expB381Row.工程P3前挠度2 = DAL_ExpDQ.ExpData_KFY.ND_GCP3_Z[0][1];
                    expB381Row.工程P3挠度2 = DAL_ExpDQ.ExpData_KFY.ND_GCP3_Z[1][1];
                    expB381Row.工程负P3前挠度2 = DAL_ExpDQ.ExpData_KFY.ND_GCP3_F[0][1];
                    expB381Row.工程负P3挠度2 = DAL_ExpDQ.ExpData_KFY.ND_GCP3_F[1][1];
                    expB381Row.工程P3前挠度3 = DAL_ExpDQ.ExpData_KFY.ND_GCP3_Z[0][2];
                    expB381Row.工程P3挠度3 = DAL_ExpDQ.ExpData_KFY.ND_GCP3_Z[1][2];
                    expB381Row.工程负P3前挠度3 = DAL_ExpDQ.ExpData_KFY.ND_GCP3_F[0][2];
                    expB381Row.工程负P3挠度3 = DAL_ExpDQ.ExpData_KFY.ND_GCP3_F[1][2];
                    expB381Row.工程Pmax前挠度1 = DAL_ExpDQ.ExpData_KFY.ND_GCPmax_Z[0][0];
                    expB381Row.工程Pmax挠度1 = DAL_ExpDQ.ExpData_KFY.ND_GCPmax_Z[1][0];
                    expB381Row.工程负Pmax前挠度1 = DAL_ExpDQ.ExpData_KFY.ND_GCPmax_F[0][0];
                    expB381Row.工程负Pmax挠度1 = DAL_ExpDQ.ExpData_KFY.ND_GCPmax_F[1][0];
                    expB381Row.工程Pmax前挠度2 = DAL_ExpDQ.ExpData_KFY.ND_GCPmax_Z[0][1];
                    expB381Row.工程Pmax挠度2 = DAL_ExpDQ.ExpData_KFY.ND_GCPmax_Z[1][1];
                    expB381Row.工程负Pmax前挠度2 = DAL_ExpDQ.ExpData_KFY.ND_GCPmax_F[0][1];
                    expB381Row.工程负Pmax挠度2 = DAL_ExpDQ.ExpData_KFY.ND_GCPmax_F[1][1];
                    expB381Row.工程Pmax前挠度3 = DAL_ExpDQ.ExpData_KFY.ND_GCPmax_Z[0][2];
                    expB381Row.工程Pmax挠度3 = DAL_ExpDQ.ExpData_KFY.ND_GCPmax_Z[1][2];
                    expB381Row.工程负Pmax前挠度3 = DAL_ExpDQ.ExpData_KFY.ND_GCPmax_F[0][2];
                    expB381Row.工程负Pmax挠度3 = DAL_ExpDQ.ExpData_KFY.ND_GCPmax_F[1][2];

                    B381TableAdapter.Update(B381Table);
                    B381Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B382

            //保存试验设置信息至B382抗风压工程挠度最大值数据
            //若编号在表中不存在，则新建
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
                    expB382Row.工程变形0挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_GCBX_Z[0];
                    expB382Row.工程变形10挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_GCBX_Z[1];
                    expB382Row.工程变形20挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_GCBX_Z[2];
                    expB382Row.工程变形30挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_GCBX_Z[3];
                    expB382Row.工程变形40挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_GCBX_Z[4];
                    expB382Row.工程变形负0挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_GCBX_F[0];
                    expB382Row.工程变形负10挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_GCBX_F[1];
                    expB382Row.工程变形负20挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_GCBX_F[2];
                    expB382Row.工程变形负30挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_GCBX_F[3];
                    expB382Row.工程变形负40挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_GCBX_F[4];
                    expB382Row.工程P3挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_GCp3_Z;
                    expB382Row.工程负P3挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_GCp3_F;
                    expB382Row.工程Pmax挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_GCpmax_Z;
                    expB382Row.工程负Pmax挠度fmax = DAL_ExpDQ.ExpData_KFY.ND_Max_GCpmax_F;

                    B382TableAdapter.Update(B382Table);
                    B382Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

            #region B390

            //保存试验设置信息至B390抗风压工程损坏数据
            //若编号在表中不存在，则新建
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
                    expB390Row.工程变形0损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_GCBX_Z[0];
                    expB390Row.工程变形10损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_GCBX_Z[1];
                    expB390Row.工程变形20损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_GCBX_Z[2];
                    expB390Row.工程变形30损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_GCBX_Z[3];
                    expB390Row.工程变形40损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_GCBX_Z[4];
                    expB390Row.工程变形负0损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_GCBX_F[0];
                    expB390Row.工程变形负10损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_GCBX_F[1];
                    expB390Row.工程变形负20损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_GCBX_F[2];
                    expB390Row.工程变形负30损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_GCBX_F[3];
                    expB390Row.工程变形负40损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_GCBX_F[4];
                    expB390Row.工程P2损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_GCP2_Z;
                    expB390Row.工程负P2损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_GCP2_F;
                    expB390Row.工程P3损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_GCP3_Z;
                    expB390Row.工程负P3损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_GCP3_F;
                    expB390Row.工程Pmax损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_GCPmax_Z;
                    expB390Row.工程负Pmax损坏情况 = DAL_ExpDQ.ExpData_KFY.Damage_GCPmax_F;
                    expB390Row.工程变形0损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_GCBX_Z[0];
                    expB390Row.工程变形10损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_GCBX_Z[1];
                    expB390Row.工程变形20损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_GCBX_Z[2];
                    expB390Row.工程变形30损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_GCBX_Z[3];
                    expB390Row.工程变形40损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_GCBX_Z[4];
                    expB390Row.工程变形负0损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_GCBX_F[0];
                    expB390Row.工程变形负10损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_GCBX_F[1];
                    expB390Row.工程变形负20损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_GCBX_F[2];
                    expB390Row.工程变形负30损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_GCBX_F[3];
                    expB390Row.工程变形负40损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_GCBX_F[4];
                    expB390Row.工程P2损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_GCP2_Z;
                    expB390Row.工程负P2损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_GCP2_F;
                    expB390Row.工程P3损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_GCP3_Z;
                    expB390Row.工程负P3损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_GCP3_F;
                    expB390Row.工程Pmax损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_GCPmax_Z;
                    expB390Row.工程负Pmax损坏说明 = DAL_ExpDQ.ExpData_KFY.DamagePS_GCPmax_F;

                    B390TableAdapter.Update(B390Table);
                    B390Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion
        }

        /// <summary>
        /// 保存当前层间变形试验进度数据至数据库
        /// </summary>
        private void SaveDataCJBX()
        {
            string tempNOStr = DAL_ExpDQ.ExpSettingParam.ExpNO;

            //B4层间变形试验数据
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
                    MessageBox.Show("未找到编号为" + tempNOStr + "的试验数据，已重新建立！", "错误提示");
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
                    expB4Row.定级X轴预加载位移角 = DAL_ExpDQ.ExpData_CJBX.AngleFM_DJ_X[0];
                    expB4Row.定级X轴第1级位移角 = DAL_ExpDQ.ExpData_CJBX.AngleFM_DJ_X[1];
                    expB4Row.定级X轴第2级位移角 = DAL_ExpDQ.ExpData_CJBX.AngleFM_DJ_X[2];
                    expB4Row.定级X轴第3级位移角 = DAL_ExpDQ.ExpData_CJBX.AngleFM_DJ_X[3];
                    expB4Row.定级X轴第4级位移角 = DAL_ExpDQ.ExpData_CJBX.AngleFM_DJ_X[4];
                    expB4Row.定级X轴第5级位移角 = DAL_ExpDQ.ExpData_CJBX.AngleFM_DJ_X[5];
                    expB4Row.定级X轴预加载位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_DJ_X[0];
                    expB4Row.定级X轴第1级位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_DJ_X[1];
                    expB4Row.定级X轴第2级位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_DJ_X[2];
                    expB4Row.定级X轴第3级位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_DJ_X[3];
                    expB4Row.定级X轴第4级位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_DJ_X[4];
                    expB4Row.定级X轴第5级位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_DJ_X[5];
                    expB4Row.定级X轴预加载是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamage_DJ_X[0] ? "有损坏" : "无损坏";
                    expB4Row.定级X轴第1级是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamage_DJ_X[1] ? "有损坏" : "无损坏";
                    expB4Row.定级X轴第2级是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamage_DJ_X[2] ? "有损坏" : "无损坏";
                    expB4Row.定级X轴第3级是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamage_DJ_X[3] ? "有损坏" : "无损坏";
                    expB4Row.定级X轴第4级是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamage_DJ_X[4] ? "有损坏" : "无损坏";
                    expB4Row.定级X轴第5级是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamage_DJ_X[5] ? "有损坏" : "无损坏";
                    expB4Row.定级X轴预加载损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_DJ_X[0];
                    expB4Row.定级X轴第1级损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_DJ_X[1];
                    expB4Row.定级X轴第2级损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_DJ_X[2];
                    expB4Row.定级X轴第3级损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_DJ_X[3];
                    expB4Row.定级X轴第4级损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_DJ_X[4];
                    expB4Row.定级X轴第5级损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_DJ_X[5];
                    //定级Y轴检测数据
                    expB4Row.定级Y轴预加载位移角 = DAL_ExpDQ.ExpData_CJBX.AngleFM_DJ_Y[0];
                    expB4Row.定级Y轴第1级位移角 = DAL_ExpDQ.ExpData_CJBX.AngleFM_DJ_Y[1];
                    expB4Row.定级Y轴第2级位移角 = DAL_ExpDQ.ExpData_CJBX.AngleFM_DJ_Y[2];
                    expB4Row.定级Y轴第3级位移角 = DAL_ExpDQ.ExpData_CJBX.AngleFM_DJ_Y[3];
                    expB4Row.定级Y轴第4级位移角 = DAL_ExpDQ.ExpData_CJBX.AngleFM_DJ_Y[4];
                    expB4Row.定级Y轴第5级位移角 = DAL_ExpDQ.ExpData_CJBX.AngleFM_DJ_Y[5];
                    expB4Row.定级Y轴预加载位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_DJ_Y[0];
                    expB4Row.定级Y轴第1级位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_DJ_Y[1];
                    expB4Row.定级Y轴第2级位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_DJ_Y[2];
                    expB4Row.定级Y轴第3级位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_DJ_Y[3];
                    expB4Row.定级Y轴第4级位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_DJ_Y[4];
                    expB4Row.定级Y轴第5级位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_DJ_Y[5];
                    expB4Row.定级Y轴预加载是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamage_DJ_Y[0] ? "有损坏" : "无损坏";
                    expB4Row.定级Y轴第1级是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamage_DJ_Y[0] ? "有损坏" : "无损坏";
                    expB4Row.定级Y轴第2级是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamage_DJ_Y[1] ? "有损坏" : "无损坏";
                    expB4Row.定级Y轴第3级是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamage_DJ_Y[2] ? "有损坏" : "无损坏";
                    expB4Row.定级Y轴第4级是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamage_DJ_Y[3] ? "有损坏" : "无损坏";
                    expB4Row.定级Y轴第5级是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamage_DJ_Y[4] ? "有损坏" : "无损坏";
                    expB4Row.定级Y轴预加载损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_DJ_Y[0];
                    expB4Row.定级Y轴第1级损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_DJ_Y[1];
                    expB4Row.定级Y轴第2级损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_DJ_Y[2];
                    expB4Row.定级Y轴第3级损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_DJ_Y[3];
                    expB4Row.定级Y轴第4级损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_DJ_Y[4];
                    expB4Row.定级Y轴第5级损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_DJ_Y[5];
                    //定级Z轴检测数据
                    expB4Row.定级Z轴预加载位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_DJ_Z[0];
                    expB4Row.定级Z轴第1级位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_DJ_Z[1];
                    expB4Row.定级Z轴第2级位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_DJ_Z[2];
                    expB4Row.定级Z轴第3级位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_DJ_Z[3];
                    expB4Row.定级Z轴第4级位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_DJ_Z[4];
                    expB4Row.定级Z轴第5级位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_DJ_Z[5];
                    expB4Row.定级Z轴预加载是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamage_DJ_Z[0] ? "有损坏" : "无损坏";
                    expB4Row.定级Z轴第1级是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamage_DJ_Z[0] ? "有损坏" : "无损坏";
                    expB4Row.定级Z轴第2级是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamage_DJ_Z[1] ? "有损坏" : "无损坏";
                    expB4Row.定级Z轴第3级是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamage_DJ_Z[2] ? "有损坏" : "无损坏";
                    expB4Row.定级Z轴第4级是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamage_DJ_Z[3] ? "有损坏" : "无损坏";
                    expB4Row.定级Z轴第5级是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamage_DJ_Z[4] ? "有损坏" : "无损坏";
                    expB4Row.定级Z轴预加载损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_DJ_Z[0];
                    expB4Row.定级Z轴第1级损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_DJ_Z[0];
                    expB4Row.定级Z轴第2级损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_DJ_Z[1];
                    expB4Row.定级Z轴第3级损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_DJ_Z[2];
                    expB4Row.定级Z轴第4级损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_DJ_Z[3];
                    expB4Row.定级Z轴第5级损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_DJ_Z[4];
                    //定级评定数据
                    expB4Row.定级X轴最大位移角 = DAL_ExpDQ.ExpData_CJBX.MaxAngleFM_DJ_X;
                    expB4Row.定级Y轴最大位移角 = DAL_ExpDQ.ExpData_CJBX.MaxAngleFM_DJ_Y;
                    expB4Row.定级Z轴最大位移量 = DAL_ExpDQ.ExpData_CJBX.MaxDSPL_DJ_Z;
                    expB4Row.定级X轴最大位移量 = DAL_ExpDQ.ExpData_CJBX.MaxDSPL_DJ_X;
                    expB4Row.定级Y轴最大位移量 = DAL_ExpDQ.ExpData_CJBX.MaxDSPL_DJ_Y;
                    expB4Row.定级X轴评定等级 = DAL_ExpDQ.ExpData_CJBX.Level_DJ_X;
                    expB4Row.定级Y轴评定等级 = DAL_ExpDQ.ExpData_CJBX.Level_DJ_Y;
                    expB4Row.定级Z轴评定等级 = DAL_ExpDQ.ExpData_CJBX.Level_DJ_Z;
                    expB4Row.定级X轴检测最终是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamageFinal_DJ_X ? "有损坏" : "无损坏";
                    expB4Row.定级Y轴检测最终是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamageFinal_DJ_Y ? "有损坏" : "无损坏";
                    expB4Row.定级Z轴检测最终是否损坏 = DAL_ExpDQ.ExpData_CJBX.IsDamageFinal_DJ_Z ? "有损坏" : "无损坏";
                    expB4Row.定级X轴检测最终损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamageFinalPS_DJ_X;
                    expB4Row.定级Y轴检测最终损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamageFinalPS_DJ_Y;
                    expB4Row.定级Z轴检测最终损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamageFinalPS_DJ_Z;

                    //工程X轴检测数据
                    expB4Row.工程X轴检测位移角 = DAL_ExpDQ.ExpData_CJBX.AngleFM_GC_X;
                    expB4Row.工程X轴检测位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_GC_X;
                    expB4Row.工程X轴检测是否损坏 = (DAL_ExpDQ.ExpData_CJBX.IsDamage_GC_X) ? "有损坏" : "无损坏";
                    expB4Row.工程X轴检测损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_GC_X;
                    //工程Y轴检测数据
                    expB4Row.工程Y轴检测位移角 = DAL_ExpDQ.ExpData_CJBX.AngleFM_GC_Y;
                    expB4Row.工程Y轴检测位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_GC_Y;
                    expB4Row.工程Y轴检测是否损坏 = (DAL_ExpDQ.ExpData_CJBX.IsDamage_GC_Y) ? "有损坏" : "无损坏";
                    expB4Row.工程Y轴检测损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_GC_Y;
                    //工程Z轴检测数据
                    expB4Row.工程Z轴检测位移量 = DAL_ExpDQ.ExpData_CJBX.DSPL_GC_Z;
                    expB4Row.工程Z轴检测是否损坏 = (DAL_ExpDQ.ExpData_CJBX.IsDamage_GC_Z) ? "有损坏" : "无损坏";
                    expB4Row.工程Z轴检测损坏说明 = DAL_ExpDQ.ExpData_CJBX.DamagePS_GC_Z;
                    //工程评定数据
                    expB4Row.工程总体是否满足设计要求 = DAL_ExpDQ.ExpData_CJBX.IsMeetDesign_GC_All ? "满足" : "不满足";
                    expB4Row.工程X轴是否满足设计要求 = DAL_ExpDQ.ExpData_CJBX.IsMeetDesign_GC_X ? "满足" : "不满足";
                    expB4Row.工程Y轴是否满足设计要求 = DAL_ExpDQ.ExpData_CJBX.IsMeetDesign_GC_Y ? "满足" : "不满足";
                    expB4Row.工程Z轴是否满足设计要求 = DAL_ExpDQ.ExpData_CJBX.IsMeetDesign_GC_Z ? "满足" : "不满足";

                    B4TableAdapter.Update(B4Table);
                    B4Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion

        #region 试验保存消息回调

        /// <summary>
        ///根据指令存储数据消息回调
        /// </summary>
        /// <param name="msg"></param>
        private void SaveExpMessage(string msg)
        {
            //全部数据-------------------------------
            //保存全部试验参数、进度和数据
            if (msg == "SaveExpAll")
            {
                SaveExpDQ();
            }
            //分项进度和数据---------------------------
            //保存气密进度和数据
            else if (msg == "SaveQMStatusAndData")
            {
                SaveExpSettingsDQ();
                SaveStatusQM();
                SaveDataQM();
            }
            //保存水密数据和进度
            else if (msg == "SaveSMStatusAndData")
            {
                SaveExpSettingsDQ();
                SaveStatusSM();
                SaveDataSM();
            }
            //保存抗风压数据和进度
            else if (msg == "SaveKFYStatusAndData")
            {
                SaveExpSettingsDQ();
                SaveStatusKFY();
                SaveDataKFY();
            }
            //保存层间变形数据和进度
            else if (msg == "SaveCJBXStatusAndData")
            {
                SaveExpSettingsDQ();
                SaveStatusCJBX();
                SaveDataCJBX();
            }
            //单项进度-------------------------------
            //保存气密进度
            else if (msg == "SaveQMStatus")
            {
                SaveExpSettingsDQ();
                SaveStatusQM();
            }
            //保存水密进度
            else if (msg == "SaveSMStatus")
            {
                SaveExpSettingsDQ();
                SaveStatusSM();
            }
            //保存抗风压进度
            else if (msg == "SaveKFYStatus")
            {
                SaveExpSettingsDQ();
                SaveStatusKFY();
            }
            //保存层间变形进度
            else if (msg == "SaveCJBXStatus")
            {
                SaveExpSettingsDQ();
                SaveStatusCJBX();
            }

            //单项数据-----------------------------------
            //保存气密数据
            else if (msg == "SaveQMData")
            {
                SaveDataQM();
            }
            //保存水密数据
            else if (msg == "SaveSMData")
            {
                SaveDataSM();
            }
            //保存抗风压数据
            else if (msg == "SaveKFYData")
            {
                SaveDataKFY();
            }
            //保存层间变形数据
            else if (msg == "SaveCJBXData")
            {
                SaveDataCJBX();
            }
        }

        #endregion
    }
}
