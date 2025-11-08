using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight;
using NPOI.SS.Formula.Functions;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using MQDFJ_MB.Model.Exp_MB;
using System.Collections.ObjectModel;

namespace MQDFJ_MB.ViewModel
{
    public partial class MQZH_MainViewModel : ViewModelBase
    {
        #region 试验管理指令回调

        /// <summary>
        ///  动态风压校准试验管理指令——MainVM
        /// </summary>
        private RelayCommand<String> _expMgtCmd_DTFYCali;
        /// <summary>
        ///  动态风压校准试验管理指令——MainVM
        /// </summary>
        public RelayCommand<String> ExpMgtCmd_DTFYCali
        {
            get
            {
                if (_expMgtCmd_DTFYCali == null)
                    _expMgtCmd_DTFYCali = new RelayCommand<String>((p) => ExecuteExpMgtCMD_DTFYCali(p));
                return _expMgtCmd_DTFYCali;

            }
            set { _expMgtCmd_DTFYCali = value; }
        }


        /// <summary>
        ///  动态风压校准试验管理指令回调
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecuteExpMgtCMD_DTFYCali(String param)
        {
            try
            {
                MessageBoxResult msgBoxResult;
                switch (param)
                {
                    case "new":
                        if (ExpNONew != "")
                        {
                            Messenger.Default.Send<string>(ExpNONew, "NewExpDTFYCaliMessage");
                        }
                        break;


                    case "del":
                        if (SelectedExpNOStr == PublicData.SM_DTFY_CaliDQ.TestNO)
                        {
                            MessageBox.Show("当前已载入的校准试验不允许删除！");
                            break;
                        }
                        if (SelectedExpNOStr == PublicData.SM_DTFY_CaliUsing.TestNO)
                        {
                            MessageBox.Show("当前在用的校准试验不允许删除！");
                            break;
                        }
                        msgBoxResult = MessageBox.Show("确认要删除选中的试验吗？", "提示", MessageBoxButton.YesNo);
                        if (msgBoxResult == MessageBoxResult.Yes)
                        {
                            Messenger.Default.Send<string>(SelectedExpNOStr, "DelExpDTFYCaliMessage");
                        }
                        break;

                    case "load":
                         msgBoxResult = MessageBox.Show("载入时当前试验未保存的数据将丢失！确认载入选中的试验？", "提示", MessageBoxButton.YesNo);
                        if (msgBoxResult == MessageBoxResult.Yes)
                        {
                            Messenger.Default.Send<string>(SelectedExpNOStr, "LoadExpDTFYCaliMessage");
                        }
                        break;

                    case "close":
                         msgBoxResult = MessageBox.Show("关闭试验前请先保存已修改的数据和测试数据！确认关闭当前试验并载入默认试验？", "提示", MessageBoxButton.YesNo);
                        if (msgBoxResult == MessageBoxResult.Yes)
                        {
                            Messenger.Default.Send<string>("DefaultExp", "LoadExpDTFYCaliMessage");
                        }
                        break;

                    case "copy":

                        if (ExpNOCopyNew != "")
                        {
                            msgBoxResult = MessageBox.Show("需要复制试验吗？", "提示", MessageBoxButton.YesNo);
                            if (msgBoxResult == MessageBoxResult.Yes)
                            {
                                string[] msg = new String[2] { SelectedExpNOStr, ExpNOCopyNew };
                                Messenger.Default.Send<string[]>(msg, "CopyExpDTFYCaliMessage");
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


        /// <summary>
        /// 更新试验列表消息回调
        /// </summary>
        /// <param name="msg"></param>
        private void ExpTableChanged_DTFYCali(List<SM_DTFY_Cali> msg)
        {
            try
            {
                ObservableCollection<SM_DTFY_Cali> a01List = null;
                a01List = new ObservableCollection<SM_DTFY_Cali>(msg);
                PublicData.TestList_DTFYCali = a01List;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}