/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 描述：
 *
 * ==================================================================================
 * 修改标记
 * 修改时间			        修改人			版本号			描述
 * 2022/2/8 15:10:31		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MQDFJ_MB.Model;
using MQDFJ_MB.Model.DEV;
using MQDFJ_MB.Model.Exp;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Windows;

namespace MQDFJ_MB.DAL.Rep
{
    /// <summary>
    /// 试验报告读写操作类
    /// </summary>
    public partial class MQZH_RepDAL: ObservableObject
    {
        public MQZH_RepDAL()
        {
            //报告导出指令消息
            Messenger.Default.Register<string>(this, "OutPutRPTMessage", OutPutRPTMessage);

            //数据备份指令消息
            Messenger.Default.Register<string>(this, "DataBackUpMessage", DataBackUpMessage);
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


        #region 相关字符串

        /// <summary>
        /// 当前时间字符串
        /// </summary>
        private string NowTimeStr
        {
            get
            {
                return DateTime.Now.ToString("-MMddHHmm");
            }
        }

        /// <summary>
        /// exe文件当前路径
        /// </summary>
        private string DataDir1
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        /// <summary>
        /// 程序文件夹根目录
        /// </summary>
        private string ProgRootDir
        {
            get { return System.IO.Directory.GetParent(DataDir1).Parent.Parent.FullName; }
        }

        /// <summary>
        /// 源数据路径
        /// </summary>
        private string DataSourceFolder
        {
            get { return ProgRootDir + "\\Data\\DataBase\\"; }
        }

        /// <summary>
        /// 源数据文件名-主数据库
        /// </summary>
        private string DataSourceFile_Main
        {
            get { return "MQZH_DB_Test.mdb"; }
        }

        /// <summary>
        /// 源数据文件名-装置数据库
        /// </summary>
        private string DataSourceFile_Dev
        {
            get { return "MQZH_DB_Dev.mdb"; }
        }

        /// <summary>
        /// 源数据文件名-主数据库
        /// </summary>
        private string DataSourceFile_Main2
        {
            get { return "MainDB.db"; }
        }

        /// <summary>
        /// 源数据文件名-装置数据库
        /// </summary>
        private string DataSourceFile_Dev2
        {
            get { return "DevDB.db"; }
        }

        /// <summary>
        /// 备份数据路径
        /// </summary>
        private string BackUpFolder
        {
            get { return ProgRootDir + "\\Data\\Backup\\"; }
        }

        /// <summary>
        /// 备份数据文件_主数据库
        /// </summary>
        private string BackUpFile_Main
        {
            get { return "MQZH_DB_Test_Backup" + NowTimeStr + ".mdb"; }
        }
        /// <summary>
        /// 备份数据文件_主数据库
        /// </summary>
        private string BackUpFile_Dev
        {
            get { return "MQZH_DB_Dev_Backup" + NowTimeStr + ".mdb"; }
        }
        /// <summary>
        /// 备份数据文件_主数据库
        /// </summary>
        private string BackUpFile_Main2
        {
            get { return "MainDB_Backup" + NowTimeStr + ".db"; }
        }
        /// <summary>
        /// 备份数据文件_主数据库
        /// </summary>
        private string BackUpFile_Dev2
        {
            get { return "DevDB_Backup" + NowTimeStr + ".db"; }
        }

        /// <summary>
        /// 三性报告模板路径
        /// </summary>
        private string RptFormPathSX
        {
            get { return ProgRootDir + "\\Data\\模板\\幕墙三性检测报告.xls"; }
        }

        /// <summary>
        /// 层间变形报告模板路径
        /// </summary>
        private string RptFormPathCJBX
        {
            get { return ProgRootDir + "\\Data\\模板\\幕墙层间变形报告.xls"; }
        }

        /// <summary>
        /// 三性报告导出路径
        /// </summary>
        private string RptAimPathSX
        {
            //get { return ProgRootDir + "\\Data\\试验报告\\" + PublicData.ExpDQ.ExpSettingParam .ExpNO  + NowTimeStr + "三性检测报告.xls"; }
            get { return ProgRootDir + "\\Data\\试验报告\\" + PublicData.ExpDQ.ExpSettingParam.ExpNO + "三性检测报告.xls"; }
        }

        /// <summary>
        /// 层间变形报告导出路径
        /// </summary>
        private string RptAimPathCJBX
        {
            //get { return ProgRootDir + "\\Data\\试验报告\\" + PublicData.ExpDQ.ExpSettingParam.ExpNO + NowTimeStr + "层间变形检测报告.xls"; }
            get { return ProgRootDir + "\\Data\\试验报告\\" + PublicData.ExpDQ.ExpSettingParam.ExpNO + "层间变形检测报告.xls"; }
        }

        /// <summary>
        /// 试验报告文件夹路径
        /// </summary>
        private string RptAimFolder
        {
            get { return ProgRootDir + "\\Data\\试验报告\\"; }
        }

        #endregion


        #region 导出报告消息回调

        /// <summary>
        ///根据指令导出报告消息回调
        /// </summary>
        /// <param name="msg"></param>
        private void OutPutRPTMessage(string msg)
        {
            //导出三性报告
            if (msg == "SaveSXRPT")
            {
                //保存曲线图
                string aimPath = ProgRootDir + "\\Data\\Image\\";

                //如果目标路径不存在,则创建目标路径
                if (!System.IO.Directory.Exists(aimPath))
                {
                    System.IO.Directory.CreateDirectory(aimPath);
                }
                aimPath = aimPath + PublicData.ExpDQ.ExpSettingParam.ExpNO;
                if (PublicData.ExpDQ.Exp_KFY.IsGC) //定级检测时类型为1，工程检测时类型为2
                    aimPath = aimPath + "2";
                else
                    aimPath = aimPath + "1";
                Messenger.Default.Send<string>(aimPath, "SaveIMG");
                Thread.Sleep(2000);

                //保存三性报告
                SaveSXRPT(aimPath);

                //打开报告所在文件夹
                System.Diagnostics.Process.Start("explorer.exe", RptAimFolder);
            }

            //导出层间变形报告
            if (msg == "SaveCJBXRPT")
            {
                SaveCJBXRPT();
                //打开报告所在文件夹
                System.Diagnostics.Process.Start("explorer.exe", RptAimFolder);
            }
        }

        #endregion



        #region 备份数据消息回调

        /// <summary>
        /// 数据备份指令消息回调
        /// </summary>
        /// <param name="msg"></param>
        private void DataBackUpMessage(string msg)
        {
            //导出报告
            if (msg == "All")
            {
                //文件复制、重命名
                if (CopyFile(DataSourceFolder, DataSourceFile_Main, BackUpFolder, BackUpFile_Main) != 1)
                    return;
                //文件复制、重命名
                if (CopyFile(DataSourceFolder, DataSourceFile_Dev, BackUpFolder, BackUpFile_Dev) != 1)
                    return;
                //文件复制、重命名
                if (CopyFile(DataSourceFolder, DataSourceFile_Main2, BackUpFolder, BackUpFile_Main2) != 1)
                    return;
                //文件复制、重命名
                if (CopyFile(DataSourceFolder, DataSourceFile_Dev2, BackUpFolder, BackUpFile_Dev2) != 1)
                    return;
                MessageBox.Show("数据已备份至" + BackUpFolder);
            }

        }

        #endregion

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourceFile">原文件路径</param>
        /// <param name="destFile">目标文件路径</param>
        /// <returns></returns>
        public int CopyFile(string sourceFolder, string sourceFile, string destFolder, string destFile)
        {
            try
            {
                //如果目标路径不存在,则创建目标路径
                if (!System.IO.Directory.Exists(destFolder))
                {
                    System.IO.Directory.CreateDirectory(destFolder);
                }

                string sourceF = sourceFolder + sourceFile;
                System.IO.File.Copy(sourceF, destFolder + sourceFile);//复制文件
                string destF = destFolder + sourceFile;
                Microsoft.VisualBasic.FileIO.FileSystem.RenameFile(destF, destFile);

                return 1;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\r\n复制失败！");
                return 0;
            }

        }

private string GetSL(int sl)
        {
            if ((int)sl == 0)
                return "无渗漏";
            if ((int)sl == 1)
                return "○";
            if ((int)sl == 2)
                return "□";
            if ((int)sl == 3)
                return "△";
            if ((int)sl == 4)
                return "▲";
            if ((int)sl == 5)
                return "●";
            if ((int)sl == 6)
                return "未检测";
            return "未检测";
        }
    }
}
