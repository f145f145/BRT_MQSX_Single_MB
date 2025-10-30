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
        /// 删除试验
        /// </summary>
        private bool DelExp(string expNo)
        {
            string delExpNo = expNo.Clone().ToString();

            //检查编号是否存在
            if ((delExpNo == null) || (delExpNo == string.Empty))
            {
                MessageBox.Show("编号为空！", "错误提示");
                return false;
            }
            MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数Row checkExistRow = A00Table.FindBy试验编号(delExpNo);
            if (checkExistRow == null)
            {
                MessageBox.Show("此编号不存在，无法删除，请重新操作或联系厂家！", "错误提示");
                return false;
            }
            //当前试验、工厂试验、默认试验不允许删除
            if (delExpNo == PublicData.ExpDQ.ExpSettingParam.ExpNO)
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
            //删除前确认
            MessageBoxResult msgBoxResult = MessageBox.Show("确认删除" + delExpNo + "号实验？试验信息和实验数据将被全部删除，且无法恢复！",
                "检查提示", MessageBoxButton.YesNo);
            if (msgBoxResult == MessageBoxResult.No)
            {
                return false;
            }

            //删除气密进度A01
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.A01气密试验进度Row willDelA01Row = A01Table.FindBy试验编号(delExpNo);
                if (willDelA01Row != null)
                {
                    willDelA01Row.Delete();
                    A01TableAdapter.Update(A01Table);
                    A01Table.AcceptChanges();
                    RaisePropertyChanged(() => A01Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除水密进度A02
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.A02水密试验进度Row willDelA02Row = A02Table.FindBy试验编号(delExpNo);
                if (willDelA02Row != null)
                {
                    willDelA02Row.Delete();
                    A02TableAdapter.Update(A02Table);
                    A02Table.AcceptChanges();
                    RaisePropertyChanged(() => A02Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除抗风压进度A03
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.A03抗风压试验进度Row willDelA03Row = A03Table.FindBy试验编号(delExpNo);
                if (willDelA03Row != null)
                {
                    willDelA03Row.Delete();
                    A03TableAdapter.Update(A03Table);
                    A03Table.AcceptChanges();
                    RaisePropertyChanged(() => A03Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除层间变形进度A04
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.A04层间变形试验进度Row willDelA04Row = A04Table.FindBy试验编号(delExpNo);
                if (willDelA04Row != null)
                {
                    willDelA04Row.Delete();
                    A04TableAdapter.Update(A04Table);
                    A04Table.AcceptChanges();
                    RaisePropertyChanged(() => A04Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除气密数据B1
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B1气密试验数据Row willDelB1Row = B1Table.FindBy试验编号(delExpNo);
                if (willDelB1Row != null)
                {
                    willDelB1Row.Delete();
                    B1TableAdapter.Update(B1Table);
                    B1Table.AcceptChanges();
                    RaisePropertyChanged(() => B1Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除水密数据B2
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B2水密试验数据Row willDelB2Row = B2Table.FindBy试验编号(delExpNo);
                if (willDelB2Row != null)
                {
                    willDelB2Row.Delete();
                    B2TableAdapter.Update(B2Table);
                    B2Table.AcceptChanges();
                    RaisePropertyChanged(() => B2Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除抗风压数据B300
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B300抗风压试验数据Row willDelB300Row = B300Table.FindBy试验编号(delExpNo);
                if (willDelB300Row != null)
                {
                    willDelB300Row.Delete();
                    B300TableAdapter.Update(B300Table);
                    B300Table.AcceptChanges();
                    RaisePropertyChanged(() => B300Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除B311a抗风压定级组1点a位移数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B311a抗风压定级组1点a位移数据Row willDelB311aRow = B311aTable.FindBy试验编号(delExpNo);
                if (willDelB311aRow != null)
                {
                    willDelB311aRow.Delete();
                    B311aTableAdapter.Update(B311aTable);
                    B311aTable.AcceptChanges();
                    RaisePropertyChanged(() => B311aTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除B311b抗风压定级组1点b位移数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B311b抗风压定级组1点b位移数据Row willDelB311bRow = B311bTable.FindBy试验编号(delExpNo);
                if (willDelB311bRow != null)
                {
                    willDelB311bRow.Delete();
                    B311bTableAdapter.Update(B311bTable);
                    B311bTable.AcceptChanges();
                    RaisePropertyChanged(() => B311bTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除B311c抗风压定级组1点c位移数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B311c抗风压定级组1点c位移数据Row willDelB311cRow = B311cTable.FindBy试验编号(delExpNo);
                if (willDelB311cRow != null)
                {
                    willDelB311cRow.Delete();
                    B311cTableAdapter.Update(B311cTable);
                    B311cTable.AcceptChanges();
                    RaisePropertyChanged(() => B311cTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除B311d抗风压定级组1点d位移数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B311d抗风压定级组1点d位移数据Row willDelB311dRow = B311dTable.FindBy试验编号(delExpNo);
                if (willDelB311dRow != null)
                {
                    willDelB311dRow.Delete();
                    B311dTableAdapter.Update(B311dTable);
                    B311dTable.AcceptChanges();
                    RaisePropertyChanged(() => B311dTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除B312a抗风压定级组1点a位移数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B312a抗风压定级组2点a位移数据Row willDelB312aRow = B312aTable.FindBy试验编号(delExpNo);
                if (willDelB312aRow != null)
                {
                    willDelB312aRow.Delete();
                    B312aTableAdapter.Update(B312aTable);
                    B312aTable.AcceptChanges();
                    RaisePropertyChanged(() => B312aTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除B312b抗风压定级组1点b位移数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B312b抗风压定级组2点b位移数据Row willDelB312bRow = B312bTable.FindBy试验编号(delExpNo);
                if (willDelB312bRow != null)
                {
                    willDelB312bRow.Delete();
                    B312bTableAdapter.Update(B312bTable);
                    B312bTable.AcceptChanges();
                    RaisePropertyChanged(() => B312bTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除B312c抗风压定级组1点c位移数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B312c抗风压定级组2点c位移数据Row willDelB312cRow = B312cTable.FindBy试验编号(delExpNo);
                if (willDelB312cRow != null)
                {
                    willDelB312cRow.Delete();
                    B312cTableAdapter.Update(B312cTable);
                    B312cTable.AcceptChanges();
                    RaisePropertyChanged(() => B312cTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除B313a抗风压定级组1点a位移数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B313a抗风压定级组3点a位移数据Row willDelB313aRow = B313aTable.FindBy试验编号(delExpNo);
                if (willDelB313aRow != null)
                {
                    willDelB313aRow.Delete();
                    B313aTableAdapter.Update(B313aTable);
                    B313aTable.AcceptChanges();
                    RaisePropertyChanged(() => B313aTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除B313b抗风压定级组1点b位移数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B313b抗风压定级组3点b位移数据Row willDelB313bRow = B313bTable.FindBy试验编号(delExpNo);
                if (willDelB313bRow != null)
                {
                    willDelB313bRow.Delete();
                    B313bTableAdapter.Update(B313bTable);
                    B313bTable.AcceptChanges();
                    RaisePropertyChanged(() => B313bTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除B313c抗风压定级组1点c位移数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B313c抗风压定级组3点c位移数据Row willDelB313cRow = B313cTable.FindBy试验编号(delExpNo);
                if (willDelB313cRow != null)
                {
                    willDelB313cRow.Delete();
                    B313cTableAdapter.Update(B313cTable);
                    B313cTable.AcceptChanges();
                    RaisePropertyChanged(() => B313cTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除.B321抗风压定级组1相对挠度数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B321抗风压定级组1相对挠度数据Row willDelB321Row = B321Table.FindBy试验编号(delExpNo);
                if (willDelB321Row != null)
                {
                    willDelB321Row.Delete();
                    B321TableAdapter.Update(B321Table);
                    B321Table.AcceptChanges();
                    RaisePropertyChanged(() => B321Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除.B322抗风压定级组2相对挠度数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B322抗风压定级组2相对挠度数据Row willDelB322Row = B322Table.FindBy试验编号(delExpNo);
                if (willDelB322Row != null)
                {
                    willDelB322Row.Delete();
                    B322TableAdapter.Update(B322Table);
                    B322Table.AcceptChanges();
                    RaisePropertyChanged(() => B322Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除.B323抗风压定级组3相对挠度数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B323抗风压定级组3相对挠度数据Row willDelB323Row = B323Table.FindBy试验编号(delExpNo);
                if (willDelB323Row != null)
                {
                    willDelB323Row.Delete();
                    B323TableAdapter.Update(B323Table);
                    B323Table.AcceptChanges();
                    RaisePropertyChanged(() => B323Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除.B324抗风压定级相对挠度最大值数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B324抗风压定级相对挠度最大值数据Row willDelB324Row = B324Table.FindBy试验编号(delExpNo);
                if (willDelB324Row != null)
                {
                    willDelB324Row.Delete();
                    B324TableAdapter.Update(B324Table);
                    B324Table.AcceptChanges();
                    RaisePropertyChanged(() => B324Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除.B331抗风压定级组1挠度数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B331抗风压定级组1挠度数据Row willDelB331Row = B331Table.FindBy试验编号(delExpNo);
                if (willDelB331Row != null)
                {
                    willDelB331Row.Delete();
                    B331TableAdapter.Update(B331Table);
                    B331Table.AcceptChanges();
                    RaisePropertyChanged(() => B331Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除.B332抗风压定级组2挠度数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B332抗风压定级组2挠度数据Row willDelB332Row = B332Table.FindBy试验编号(delExpNo);
                if (willDelB332Row != null)
                {
                    willDelB332Row.Delete();
                    B332TableAdapter.Update(B332Table);
                    B332Table.AcceptChanges();
                    RaisePropertyChanged(() => B332Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除.B333抗风压定级组3挠度数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B333抗风压定级组3挠度数据Row willDelB333Row = B333Table.FindBy试验编号(delExpNo);
                if (willDelB333Row != null)
                {
                    willDelB333Row.Delete();
                    B333TableAdapter.Update(B333Table);
                    B333Table.AcceptChanges();
                    RaisePropertyChanged(() => B333Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除.B334抗风压定级挠度最大值数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B334抗风压定级挠度最大值数据Row willDelB334Row = B334Table.FindBy试验编号(delExpNo);
                if (willDelB334Row != null)
                {
                    willDelB334Row.Delete();
                    B334TableAdapter.Update(B334Table);
                    B334Table.AcceptChanges();
                    RaisePropertyChanged(() => B334Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除B340抗风压定级损坏情况数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B340抗风压定级损坏情况数据Row willDelB340Row = B340Table.FindBy试验编号(delExpNo);
                if (willDelB340Row != null)
                {
                    willDelB340Row.Delete();
                    B340TableAdapter.Update(B340Table);
                    B340Table.AcceptChanges();
                    RaisePropertyChanged(() => B340Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除B350抗风压定级损坏说明数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B350抗风压定级损坏说明数据Row willDelB350Row = B350Table.FindBy试验编号(delExpNo);
                if (willDelB350Row != null)
                {
                    willDelB350Row.Delete();
                    B350TableAdapter.Update(B350Table);
                    B350Table.AcceptChanges();
                    RaisePropertyChanged(() => B350Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除B361抗风压定级损坏说明数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B361抗风压组1工程位移数据Row willDelB361Row = B361Table.FindBy试验编号(delExpNo);
                if (willDelB361Row != null)
                {
                    willDelB361Row.Delete();
                    B361TableAdapter.Update(B361Table);
                    B361Table.AcceptChanges();
                    RaisePropertyChanged(() => B361Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除B362抗风压定级损坏说明数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B362抗风压组2工程位移数据Row willDelB362Row = B362Table.FindBy试验编号(delExpNo);
                if (willDelB362Row != null)
                {
                    willDelB362Row.Delete();
                    B362TableAdapter.Update(B362Table);
                    B362Table.AcceptChanges();
                    RaisePropertyChanged(() => B362Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除B363抗风压定级损坏说明数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B363抗风压组3工程位移数据Row willDelB363Row = B363Table.FindBy试验编号(delExpNo);
                if (willDelB363Row != null)
                {
                    willDelB363Row.Delete();
                    B363TableAdapter.Update(B363Table);
                    B363Table.AcceptChanges();
                    RaisePropertyChanged(() => B363Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除B371抗风压工程相对挠度数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B371抗风压工程相对挠度数据Row willDelB371Row = B371Table.FindBy试验编号(delExpNo);
                if (willDelB371Row != null)
                {
                    willDelB371Row.Delete();
                    B371TableAdapter.Update(B371Table);
                    B371Table.AcceptChanges();
                    RaisePropertyChanged(() => B371Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除B372抗风压工程相对挠度数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B372抗风压工程相对挠度最大值数据Row willDelB372Row = B372Table.FindBy试验编号(delExpNo);
                if (willDelB372Row != null)
                {
                    willDelB372Row.Delete();
                    B372TableAdapter.Update(B372Table);
                    B372Table.AcceptChanges();
                    RaisePropertyChanged(() => B372Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除B381抗风压工程相对挠度数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B381抗风压工程挠度数据Row willDelB381Row = B381Table.FindBy试验编号(delExpNo);
                if (willDelB381Row != null)
                {
                    willDelB381Row.Delete();
                    B381TableAdapter.Update(B381Table);
                    B381Table.AcceptChanges();
                    RaisePropertyChanged(() => B381Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除B382抗风压工程相对挠度数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B382抗风压工程挠度最大值数据Row willDelB382Row = B382Table.FindBy试验编号(delExpNo);
                if (willDelB382Row != null)
                {
                    willDelB382Row.Delete();
                    B382TableAdapter.Update(B382Table);
                    B382Table.AcceptChanges();
                    RaisePropertyChanged(() => B382Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除B390抗风压工程损坏数据
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B390抗风压工程损坏数据Row willDelB390Row = B390Table.FindBy试验编号(delExpNo);
                if (willDelB390Row != null)
                {
                    willDelB390Row.Delete();
                    B390TableAdapter.Update(B390Table);
                    B390Table.AcceptChanges();
                    RaisePropertyChanged(() => B390Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除层间变形数据B4
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.B4层间变形试验数据Row willDelB4Row = B4Table.FindBy试验编号(delExpNo);
                if (willDelB4Row != null)
                {
                    willDelB4Row.Delete();
                    B4TableAdapter.Update(B4Table);
                    B4Table.AcceptChanges();
                    RaisePropertyChanged(() => B4Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //删除试验主参数A00
            try
            {
                MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数Row willDelA00Row = A00Table.FindBy试验编号(delExpNo);
                willDelA00Row.Delete();
                A00TableAdapter.Update(A00Table);
                A00Table.AcceptChanges();
                RaisePropertyChanged(() => A00Table);

                MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数2Row willDelA00Row2 = A00Table2.FindBy试验编号(delExpNo);
                willDelA00Row2.Delete();
                A00Table2Adapter.Update(A00Table2);
                A00Table2.AcceptChanges();
                RaisePropertyChanged(() => A00Table2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return true;
        }
    }
}