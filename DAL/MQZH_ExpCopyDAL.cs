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
using System.Windows;
using GalaSoft.MvvmLight;

namespace MQDFJ_MB.DAL
{
    /// <summary>
    /// 试验参数及数据读写操作类
    /// </summary>
    public partial class MQZH_ExpDAL : ObservableObject
    {
        /// <summary>
        /// 根据名称复制试验
        /// </summary>
        private bool CopyExp(string OldNo, string expNo, bool isNew)
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

                #region 检查旧编号试验是否存在

                //A00主表
                try
                {
                    MQZH_DB_TestDataSet.A00试验参数Row checkA00Row = A00Table.NewA00试验参数Row();
                    checkA00Row = A00Table.FindBy试验编号(oldExpNo);
                    if (checkA00Row == null)
                    {
                        MessageBox.Show("编号" + oldExpNo + "A00主表不存在！", "错误提示");
                        return false;
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
                    MQZH_DB_TestDataSet.A00试验参数Row checkA00Row = A00Table.NewA00试验参数Row();
                    checkA00Row = A00Table.FindBy试验编号(newExpNo);
                    if (checkA00Row != null)
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

                //A00主表新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数Row defExpA00Row = A00Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数Row newExpA00Row = A00Table.NewA00试验参数Row();
                    newExpA00Row.ItemArray = (object[])defExpA00Row.ItemArray.Clone();
                    newExpA00Row.试验编号 = newExpNo;
                    newExpA00Row.三性报告编号 = newExpNo + "三性检测报告";
                    newExpA00Row.层间变形报告编号 = newExpNo + "层间变形检测报告";
                    newExpA00Row.试验补充说明 = "//";

                    if (isNew)
                    {
                        newExpA00Row.检测日期 = DateTime.Now.ToString();
                        newExpA00Row.委托日期 = DateTime.Now.ToString();
                    }

                    A00Table.AddA00试验参数Row(newExpA00Row);
                    A00TableAdapter.Update(A00Table);
                    A00Table.AcceptChanges();
                    RaisePropertyChanged(() => A00Table);


                    MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数2Row defExpA00Row2 = A00Table2.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数2Row newExpA00Row2 = A00Table2.NewA00试验参数2Row();
                    newExpA00Row2.ItemArray = (object[])defExpA00Row2.ItemArray.Clone();
                    newExpA00Row2.试验编号 = newExpNo;
                    A00Table2.AddA00试验参数2Row(newExpA00Row2);
                    A00Table2Adapter.Update(A00Table2);
                    A00Table2.AcceptChanges();
                    RaisePropertyChanged(() => A00Table2);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //A01气密进度新增
                try
                {

                    MQDFJ_MB.MQZH_DB_TestDataSet.A01气密试验进度Row defExpA01Row = A01Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.A01气密试验进度Row newExpA01Row = A01Table.NewA01气密试验进度Row();
                    newExpA01Row.ItemArray = (object[])defExpA01Row.ItemArray.Clone();
                    newExpA01Row.试验编号 = newExpNo;
                    A01Table.AddA01气密试验进度Row(newExpA01Row);
                    A01TableAdapter.Update(A01Table);
                    A01Table.AcceptChanges();
                    RaisePropertyChanged(() => A01Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //A02水密进度新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.A02水密试验进度Row defExpA02Row = A02Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.A02水密试验进度Row newExpA02Row = A02Table.NewA02水密试验进度Row();
                    newExpA02Row.ItemArray = (object[])defExpA02Row.ItemArray.Clone();
                    newExpA02Row.试验编号 = newExpNo;
                    A02Table.AddA02水密试验进度Row(newExpA02Row);
                    A02TableAdapter.Update(A02Table);
                    A02Table.AcceptChanges();
                    RaisePropertyChanged(() => A02Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //A03抗风压进度新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.A03抗风压试验进度Row defExpA03Row = A03Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.A03抗风压试验进度Row newExpA03Row = A03Table.NewA03抗风压试验进度Row();
                    newExpA03Row.ItemArray = (object[])defExpA03Row.ItemArray.Clone();
                    newExpA03Row.试验编号 = newExpNo;
                    A03Table.AddA03抗风压试验进度Row(newExpA03Row);
                    A03TableAdapter.Update(A03Table);
                    A03Table.AcceptChanges();
                    RaisePropertyChanged(() => A03Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //A04层间变形进度新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.A04层间变形试验进度Row defExpA04Row = A04Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.A04层间变形试验进度Row newExpA04Row = A04Table.NewA04层间变形试验进度Row();
                    newExpA04Row.ItemArray = (object[])defExpA04Row.ItemArray.Clone();
                    newExpA04Row.试验编号 = newExpNo;
                    A04Table.AddA04层间变形试验进度Row(newExpA04Row);
                    A04TableAdapter.Update(A04Table);
                    A04Table.AcceptChanges();
                    RaisePropertyChanged(() => A04Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B1气密数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B1气密试验数据Row defDataB1Row = B1Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B1气密试验数据Row newDataB1Row = B1Table.NewB1气密试验数据Row();
                    newDataB1Row.ItemArray = (object[])defDataB1Row.ItemArray.Clone();
                    newDataB1Row.试验编号 = newExpNo;
                    B1Table.AddB1气密试验数据Row(newDataB1Row);
                    B1TableAdapter.Update(B1Table);
                    B1Table.AcceptChanges();
                    RaisePropertyChanged(() => B1Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B2水密数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B2水密试验数据Row defDataB2Row = B2Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B2水密试验数据Row newDataB2Row = B2Table.NewB2水密试验数据Row();
                    newDataB2Row.ItemArray = (object[])defDataB2Row.ItemArray.Clone();
                    newDataB2Row.试验编号 = newExpNo;
                    B2Table.AddB2水密试验数据Row(newDataB2Row);
                    B2TableAdapter.Update(B2Table);
                    B2Table.AcceptChanges();
                    RaisePropertyChanged(() => B2Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B300抗风压数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B300抗风压试验数据Row defDataB300Row = B300Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B300抗风压试验数据Row newDataB300Row = B300Table.NewB300抗风压试验数据Row();
                    newDataB300Row.ItemArray = (object[])defDataB300Row.ItemArray.Clone();
                    newDataB300Row.试验编号 = newExpNo;
                    B300Table.AddB300抗风压试验数据Row(newDataB300Row);
                    B300TableAdapter.Update(B300Table);
                    B300Table.AcceptChanges();
                    RaisePropertyChanged(() => B300Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B311a抗风压组1a定级正位移数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B311a抗风压定级组1点a位移数据Row defDataB311aRow = B311aTable.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B311a抗风压定级组1点a位移数据Row newDataB311aRow = B311aTable.NewB311a抗风压定级组1点a位移数据Row();
                    newDataB311aRow.ItemArray = (object[])defDataB311aRow.ItemArray.Clone();
                    newDataB311aRow.试验编号 = newExpNo;
                    B311aTable.AddB311a抗风压定级组1点a位移数据Row(newDataB311aRow);
                    B311aTableAdapter.Update(B311aTable);
                    B311aTable.AcceptChanges();
                    RaisePropertyChanged(() => B311aTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B311b抗风压组1b定级正位移数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B311b抗风压定级组1点b位移数据Row defDataB311bRow = B311bTable.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B311b抗风压定级组1点b位移数据Row newDataB311bRow = B311bTable.NewB311b抗风压定级组1点b位移数据Row();
                    newDataB311bRow.ItemArray = (object[])defDataB311bRow.ItemArray.Clone();
                    newDataB311bRow.试验编号 = newExpNo;
                    B311bTable.AddB311b抗风压定级组1点b位移数据Row(newDataB311bRow);
                    B311bTableAdapter.Update(B311bTable);
                    B311bTable.AcceptChanges();
                    RaisePropertyChanged(() => B311bTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B311c抗风压组1c定级正位移数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B311c抗风压定级组1点c位移数据Row defDataB311cRow = B311cTable.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B311c抗风压定级组1点c位移数据Row newDataB311cRow = B311cTable.NewB311c抗风压定级组1点c位移数据Row();
                    newDataB311cRow.ItemArray = (object[])defDataB311cRow.ItemArray.Clone();
                    newDataB311cRow.试验编号 = newExpNo;
                    B311cTable.AddB311c抗风压定级组1点c位移数据Row(newDataB311cRow);
                    B311cTableAdapter.Update(B311cTable);
                    B311cTable.AcceptChanges();
                    RaisePropertyChanged(() => B311cTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B311d抗风压组1d定级正位移数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B311d抗风压定级组1点d位移数据Row defDataB311dRow = B311dTable.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B311d抗风压定级组1点d位移数据Row newDataB311dRow = B311dTable.NewB311d抗风压定级组1点d位移数据Row();
                    newDataB311dRow.ItemArray = (object[])defDataB311dRow.ItemArray.Clone();
                    newDataB311dRow.试验编号 = newExpNo;
                    B311dTable.AddB311d抗风压定级组1点d位移数据Row(newDataB311dRow);
                    B311dTableAdapter.Update(B311dTable);
                    B311dTable.AcceptChanges();
                    RaisePropertyChanged(() => B311dTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B312a抗风压组2a定级正位移数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B312a抗风压定级组2点a位移数据Row defDataB312aRow = B312aTable.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B312a抗风压定级组2点a位移数据Row newDataB312aRow = B312aTable.NewB312a抗风压定级组2点a位移数据Row();
                    newDataB312aRow.ItemArray = (object[])defDataB312aRow.ItemArray.Clone();
                    newDataB312aRow.试验编号 = newExpNo;
                    B312aTable.AddB312a抗风压定级组2点a位移数据Row(newDataB312aRow);
                    B312aTableAdapter.Update(B312aTable);
                    B312aTable.AcceptChanges();
                    RaisePropertyChanged(() => B312aTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B312b抗风压组2b定级正位移数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B312b抗风压定级组2点b位移数据Row defDataB312bRow = B312bTable.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B312b抗风压定级组2点b位移数据Row newDataB312bRow = B312bTable.NewB312b抗风压定级组2点b位移数据Row();
                    newDataB312bRow.ItemArray = (object[])defDataB312bRow.ItemArray.Clone();
                    newDataB312bRow.试验编号 = newExpNo;
                    B312bTable.AddB312b抗风压定级组2点b位移数据Row(newDataB312bRow);
                    B312bTableAdapter.Update(B312bTable);
                    B312bTable.AcceptChanges();
                    RaisePropertyChanged(() => B312bTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B312c抗风压组2c定级正位移数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B312c抗风压定级组2点c位移数据Row defDataB312cRow = B312cTable.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B312c抗风压定级组2点c位移数据Row newDataB312cRow = B312cTable.NewB312c抗风压定级组2点c位移数据Row();
                    newDataB312cRow.ItemArray = (object[])defDataB312cRow.ItemArray.Clone();
                    newDataB312cRow.试验编号 = newExpNo;
                    B312cTable.AddB312c抗风压定级组2点c位移数据Row(newDataB312cRow);
                    B312cTableAdapter.Update(B312cTable);
                    B312cTable.AcceptChanges();
                    RaisePropertyChanged(() => B312cTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B313a抗风压组3a定级正位移数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B313a抗风压定级组3点a位移数据Row defDataB313aRow = B313aTable.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B313a抗风压定级组3点a位移数据Row newDataB313aRow = B313aTable.NewB313a抗风压定级组3点a位移数据Row();
                    newDataB313aRow.ItemArray = (object[])defDataB313aRow.ItemArray.Clone();
                    newDataB313aRow.试验编号 = newExpNo;
                    B313aTable.AddB313a抗风压定级组3点a位移数据Row(newDataB313aRow);
                    B313aTableAdapter.Update(B313aTable);
                    B313aTable.AcceptChanges();
                    RaisePropertyChanged(() => B313aTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B313b抗风压组3b定级正位移数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B313b抗风压定级组3点b位移数据Row defDataB313bRow = B313bTable.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B313b抗风压定级组3点b位移数据Row newDataB313bRow = B313bTable.NewB313b抗风压定级组3点b位移数据Row();
                    newDataB313bRow.ItemArray = (object[])defDataB313bRow.ItemArray.Clone();
                    newDataB313bRow.试验编号 = newExpNo;
                    B313bTable.AddB313b抗风压定级组3点b位移数据Row(newDataB313bRow);
                    B313bTableAdapter.Update(B313bTable);
                    B313bTable.AcceptChanges();
                    RaisePropertyChanged(() => B313bTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B313c抗风压组3c定级正位移数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B313c抗风压定级组3点c位移数据Row defDataB313cRow = B313cTable.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B313c抗风压定级组3点c位移数据Row newDataB313cRow = B313cTable.NewB313c抗风压定级组3点c位移数据Row();
                    newDataB313cRow.ItemArray = (object[])defDataB313cRow.ItemArray.Clone();
                    newDataB313cRow.试验编号 = newExpNo;
                    B313cTable.AddB313c抗风压定级组3点c位移数据Row(newDataB313cRow);
                    B313cTableAdapter.Update(B313cTable);
                    B313cTable.AcceptChanges();
                    RaisePropertyChanged(() => B313cTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B321抗风压定级组1相对挠度数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B321抗风压定级组1相对挠度数据Row defDataB321Row = B321Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B321抗风压定级组1相对挠度数据Row newDataB321Row = B321Table.NewB321抗风压定级组1相对挠度数据Row();
                    newDataB321Row.ItemArray = (object[])defDataB321Row.ItemArray.Clone();
                    newDataB321Row.试验编号 = newExpNo;
                    B321Table.AddB321抗风压定级组1相对挠度数据Row(newDataB321Row);
                    B321TableAdapter.Update(B321Table);
                    B321Table.AcceptChanges();
                    RaisePropertyChanged(() => B321Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B322抗风压定级组2相对挠度数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B322抗风压定级组2相对挠度数据Row defDataB322Row = B322Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B322抗风压定级组2相对挠度数据Row newDataB322Row = B322Table.NewB322抗风压定级组2相对挠度数据Row();
                    newDataB322Row.ItemArray = (object[])defDataB322Row.ItemArray.Clone();
                    newDataB322Row.试验编号 = newExpNo;
                    B322Table.AddB322抗风压定级组2相对挠度数据Row(newDataB322Row);
                    B322TableAdapter.Update(B322Table);
                    B322Table.AcceptChanges();
                    RaisePropertyChanged(() => B322Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B323抗风压定级组3相对挠度数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B323抗风压定级组3相对挠度数据Row defDataB323Row = B323Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B323抗风压定级组3相对挠度数据Row newDataB323Row = B323Table.NewB323抗风压定级组3相对挠度数据Row();
                    newDataB323Row.ItemArray = (object[])defDataB323Row.ItemArray.Clone();
                    newDataB323Row.试验编号 = newExpNo;
                    B323Table.AddB323抗风压定级组3相对挠度数据Row(newDataB323Row);
                    B323TableAdapter.Update(B323Table);
                    B323Table.AcceptChanges();
                    RaisePropertyChanged(() => B323Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B324抗风压定级相对挠度最大值数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B324抗风压定级相对挠度最大值数据Row defDataB324Row = B324Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B324抗风压定级相对挠度最大值数据Row newDataB324Row = B324Table.NewB324抗风压定级相对挠度最大值数据Row();
                    newDataB324Row.ItemArray = (object[])defDataB324Row.ItemArray.Clone();
                    newDataB324Row.试验编号 = newExpNo;
                    B324Table.AddB324抗风压定级相对挠度最大值数据Row(newDataB324Row);
                    B324TableAdapter.Update(B324Table);
                    B324Table.AcceptChanges();
                    RaisePropertyChanged(() => B324Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B331抗风压定级组1挠度数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B331抗风压定级组1挠度数据Row defDataB331Row = B331Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B331抗风压定级组1挠度数据Row newDataB331Row = B331Table.NewB331抗风压定级组1挠度数据Row();
                    newDataB331Row.ItemArray = (object[])defDataB331Row.ItemArray.Clone();
                    newDataB331Row.试验编号 = newExpNo;
                    B331Table.AddB331抗风压定级组1挠度数据Row(newDataB331Row);
                    B331TableAdapter.Update(B331Table);
                    B331Table.AcceptChanges();
                    RaisePropertyChanged(() => B331Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B332抗风压定级组2挠度数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B332抗风压定级组2挠度数据Row defDataB332Row = B332Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B332抗风压定级组2挠度数据Row newDataB332Row = B332Table.NewB332抗风压定级组2挠度数据Row();
                    newDataB332Row.ItemArray = (object[])defDataB332Row.ItemArray.Clone();
                    newDataB332Row.试验编号 = newExpNo;
                    B332Table.AddB332抗风压定级组2挠度数据Row(newDataB332Row);
                    B332TableAdapter.Update(B332Table);
                    B332Table.AcceptChanges();
                    RaisePropertyChanged(() => B332Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B333抗风压定级组3挠度数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B333抗风压定级组3挠度数据Row defDataB333Row = B333Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B333抗风压定级组3挠度数据Row newDataB333Row = B333Table.NewB333抗风压定级组3挠度数据Row();
                    newDataB333Row.ItemArray = (object[])defDataB333Row.ItemArray.Clone();
                    newDataB333Row.试验编号 = newExpNo;
                    B333Table.AddB333抗风压定级组3挠度数据Row(newDataB333Row);
                    B333TableAdapter.Update(B333Table);
                    B333Table.AcceptChanges();
                    RaisePropertyChanged(() => B333Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B334抗风压定级挠度最大值数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B334抗风压定级挠度最大值数据Row defDataB334Row = B334Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B334抗风压定级挠度最大值数据Row newDataB334Row = B334Table.NewB334抗风压定级挠度最大值数据Row();
                    newDataB334Row.ItemArray = (object[])defDataB334Row.ItemArray.Clone();
                    newDataB334Row.试验编号 = newExpNo;
                    B334Table.AddB334抗风压定级挠度最大值数据Row(newDataB334Row);
                    B334TableAdapter.Update(B334Table);
                    B334Table.AcceptChanges();
                    RaisePropertyChanged(() => B334Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B340抗风压定级损坏情况数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B340抗风压定级损坏情况数据Row defDataB340Row = B340Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B340抗风压定级损坏情况数据Row newDataB340Row = B340Table.NewB340抗风压定级损坏情况数据Row();
                    newDataB340Row.ItemArray = (object[])defDataB340Row.ItemArray.Clone();
                    newDataB340Row.试验编号 = newExpNo;
                    B340Table.AddB340抗风压定级损坏情况数据Row(newDataB340Row);
                    B340TableAdapter.Update(B340Table);
                    B340Table.AcceptChanges();
                    RaisePropertyChanged(() => B340Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //.B350抗风压定级损坏说明数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B350抗风压定级损坏说明数据Row defDataB350Row = B350Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B350抗风压定级损坏说明数据Row newDataB350Row = B350Table.NewB350抗风压定级损坏说明数据Row();
                    newDataB350Row.ItemArray = (object[])defDataB350Row.ItemArray.Clone();
                    newDataB350Row.试验编号 = newExpNo;
                    B350Table.AddB350抗风压定级损坏说明数据Row(newDataB350Row);
                    B350TableAdapter.Update(B350Table);
                    B350Table.AcceptChanges();
                    RaisePropertyChanged(() => B350Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B361抗风压组1工程位移数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B361抗风压组1工程位移数据Row defDataB361Row = B361Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B361抗风压组1工程位移数据Row newDataB361Row = B361Table.NewB361抗风压组1工程位移数据Row();
                    newDataB361Row.ItemArray = (object[])defDataB361Row.ItemArray.Clone();
                    newDataB361Row.试验编号 = newExpNo;
                    B361Table.AddB361抗风压组1工程位移数据Row(newDataB361Row);
                    B361TableAdapter.Update(B361Table);
                    B361Table.AcceptChanges();
                    RaisePropertyChanged(() => B361Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B362抗风压组1工程位移数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B362抗风压组2工程位移数据Row defDataB362Row = B362Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B362抗风压组2工程位移数据Row newDataB362Row = B362Table.NewB362抗风压组2工程位移数据Row();
                    newDataB362Row.ItemArray = (object[])defDataB362Row.ItemArray.Clone();
                    newDataB362Row.试验编号 = newExpNo;
                    B362Table.AddB362抗风压组2工程位移数据Row(newDataB362Row);
                    B362TableAdapter.Update(B362Table);
                    B362Table.AcceptChanges();
                    RaisePropertyChanged(() => B362Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B363抗风压组3工程位移数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B363抗风压组3工程位移数据Row defDataB363Row = B363Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B363抗风压组3工程位移数据Row newDataB363Row = B363Table.NewB363抗风压组3工程位移数据Row();
                    newDataB363Row.ItemArray = (object[])defDataB363Row.ItemArray.Clone();
                    newDataB363Row.试验编号 = newExpNo;
                    B363Table.AddB363抗风压组3工程位移数据Row(newDataB363Row);
                    B363TableAdapter.Update(B363Table);
                    B363Table.AcceptChanges();
                    RaisePropertyChanged(() => B363Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B371抗风压工程相对挠度数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B371抗风压工程相对挠度数据Row defDataB371Row = B371Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B371抗风压工程相对挠度数据Row newDataB371Row = B371Table.NewB371抗风压工程相对挠度数据Row();
                    newDataB371Row.ItemArray = (object[])defDataB371Row.ItemArray.Clone();
                    newDataB371Row.试验编号 = newExpNo;
                    B371Table.AddB371抗风压工程相对挠度数据Row(newDataB371Row);
                    B371TableAdapter.Update(B371Table);
                    B371Table.AcceptChanges();
                    RaisePropertyChanged(() => B371Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B372抗风压工程相对挠度最大值数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B372抗风压工程相对挠度最大值数据Row defDataB372Row = B372Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B372抗风压工程相对挠度最大值数据Row newDataB372Row = B372Table.NewB372抗风压工程相对挠度最大值数据Row();
                    newDataB372Row.ItemArray = (object[])defDataB372Row.ItemArray.Clone();
                    newDataB372Row.试验编号 = newExpNo;
                    B372Table.AddB372抗风压工程相对挠度最大值数据Row(newDataB372Row);
                    B372TableAdapter.Update(B372Table);
                    B372Table.AcceptChanges();
                    RaisePropertyChanged(() => B372Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B381抗风压工程挠度数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B381抗风压工程挠度数据Row defDataB381Row = B381Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B381抗风压工程挠度数据Row newDataB381Row = B381Table.NewB381抗风压工程挠度数据Row();
                    newDataB381Row.ItemArray = (object[])defDataB381Row.ItemArray.Clone();
                    newDataB381Row.试验编号 = newExpNo;
                    B381Table.AddB381抗风压工程挠度数据Row(newDataB381Row);
                    B381TableAdapter.Update(B381Table);
                    B381Table.AcceptChanges();
                    RaisePropertyChanged(() => B381Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B382抗风压工程挠度最大值数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B382抗风压工程挠度最大值数据Row defDataB382Row = B382Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B382抗风压工程挠度最大值数据Row newDataB382Row = B382Table.NewB382抗风压工程挠度最大值数据Row();
                    newDataB382Row.ItemArray = (object[])defDataB382Row.ItemArray.Clone();
                    newDataB382Row.试验编号 = newExpNo;
                    B382Table.AddB382抗风压工程挠度最大值数据Row(newDataB382Row);
                    B382TableAdapter.Update(B382Table);
                    B382Table.AcceptChanges();
                    RaisePropertyChanged(() => B382Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //B390抗风压工程损坏数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B390抗风压工程损坏数据Row defDataB390Row = B390Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B390抗风压工程损坏数据Row newDataB390Row = B390Table.NewB390抗风压工程损坏数据Row();
                    newDataB390Row.ItemArray = (object[])defDataB390Row.ItemArray.Clone();
                    newDataB390Row.试验编号 = newExpNo;
                    B390Table.AddB390抗风压工程损坏数据Row(newDataB390Row);
                    B390TableAdapter.Update(B390Table);
                    B390Table.AcceptChanges();
                    RaisePropertyChanged(() => B390Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }


                //B4层间变形数据新增
                try
                {
                    MQDFJ_MB.MQZH_DB_TestDataSet.B4层间变形试验数据Row defDataB4Row = B4Table.FindBy试验编号(oldExpNo);
                    MQDFJ_MB.MQZH_DB_TestDataSet.B4层间变形试验数据Row newDataB4Row = B4Table.NewB4层间变形试验数据Row();
                    newDataB4Row.ItemArray = (object[])defDataB4Row.ItemArray.Clone();
                    newDataB4Row.试验编号 = newExpNo;
                    B4Table.AddB4层间变形试验数据Row(newDataB4Row);
                    B4TableAdapter.Update(B4Table);
                    B4Table.AcceptChanges();
                    RaisePropertyChanged(() => B4Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                #endregion

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }
    }
}