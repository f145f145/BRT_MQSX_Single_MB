/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/2/8 15:09:54
 * 描述：
 *
 * ==================================================================================
 * 修改标记
 * 修改时间			        修改人			版本号			描述
 * 2022/2/8 15:09:54		郝正强			V1.0.0.0
 * 2022/7/25 15:09:54		郝正强			V2.0.0          增加复制试验功能
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
using MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters;

namespace MQDFJ_MB.DAL.Exp
{
    /// <summary>
    /// 试验参数及数据读写操作类
    /// </summary>
    public partial class MQZH_ExpDAL : ObservableObject
    {
        public MQZH_ExpDAL()
        {
            //Dataset、DataTable
            SetTableAdapterInit_Exp();

            Messenger.Default.Send<MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数DataTable>(A00Table, "ExpTableChanged");

            //试验操作指令消息
            Messenger.Default.Register<string>(this, "LoadExpByName", LoadExpMessage);
            Messenger.Default.Register<string>(this, "NewExpMessage", NewExpMessage);
            Messenger.Default.Register<string>(this, "DelExpByName", DelExpMessage);
            Messenger.Default.Register<string[]>(this, "CopyExpMessage", CopyExpMessage);
            Messenger.Default.Register<string>(this, "SaveExpMessage", SaveExpMessage);

            //当前试验参数、进度、数据读写消息
            Messenger.Default.Register<string>(this, "LoadDQExpDataMessage", LoadDQExpDataMessage);
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


        #region DataTable属性

        #region  A试验及进度

        /// <summary>
        /// 幕墙四性A00试验参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数DataTable _a00Table;

        /// <summary>
        /// 幕墙四性A00试验参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数DataTable A00Table
        {
            get { return _a00Table; }
            set
            {
                _a00Table = value;
                RaisePropertyChanged(() => A00Table);
            }
        }

        /// <summary>
        /// 幕墙四性A00试验参数2DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数2DataTable _a00Table2;

        /// <summary>
        /// 幕墙四性A00试验参数DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.A00试验参数2DataTable A00Table2
        {
            get { return _a00Table2; }
            set
            {
                _a00Table2 = value;
                RaisePropertyChanged(() => A00Table2);
            }
        }

        /// <summary>
        /// 幕墙四性A01气密试验进度DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.A01气密试验进度DataTable _a01Table;

        /// <summary>
        /// 幕墙四性A01气密试验进度DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.A01气密试验进度DataTable A01Table
        {
            get { return _a01Table; }
            set
            {
                _a01Table = value;
                RaisePropertyChanged(() => A01Table);
            }
        }

        /// <summary>
        /// 幕墙四性A02水密试验进度DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.A02水密试验进度DataTable _a02Table;

        /// <summary>
        /// 幕墙四性A02水密试验进度DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.A02水密试验进度DataTable A02Table
        {
            get { return _a02Table; }
            set
            {
                _a02Table = value;
                RaisePropertyChanged(() => A02Table);
            }
        }

        /// <summary>
        /// 幕墙四性A03抗风压试验进度DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.A03抗风压试验进度DataTable _a03Table;

        /// <summary>
        /// 幕墙四性A03抗风压试验进度DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.A03抗风压试验进度DataTable A03Table
        {
            get { return _a03Table; }
            set
            {
                _a03Table = value;
                RaisePropertyChanged(() => A03Table);
            }
        }

        /// <summary>
        /// 幕墙四性A04层间变形试验进度DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.A04层间变形试验进度DataTable _a04Table;

        /// <summary>
        /// 幕墙四性A04层间变形试验进度DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.A04层间变形试验进度DataTable A04Table
        {
            get { return _a04Table; }
            set
            {
                _a04Table = value;
                RaisePropertyChanged(() => A04Table);
            }
        }

        #endregion

        #region B试验数据表

        /// <summary>
        /// 幕墙四性B1气密试验数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B1气密试验数据DataTable _b1Table;
        /// <summary>
        /// 幕墙四性B1气密试验数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B1气密试验数据DataTable B1Table
        {
            get { return _b1Table; }
            set
            {
                _b1Table = value;
                RaisePropertyChanged(() => B1Table);
            }
        }

        /// <summary>
        /// 幕墙四性B2水密试验数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B2水密试验数据DataTable _b2Table;
        /// <summary>
        /// 幕墙四性B2水密试验数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B2水密试验数据DataTable B2Table
        {
            get { return _b2Table; }
            set
            {
                _b2Table = value;
                RaisePropertyChanged(() => B2Table);
            }
        }

        /// <summary>
        /// 幕墙四性B300抗风压试验数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B300抗风压试验数据DataTable _b300Table;
        /// <summary>
        /// 幕墙四性B300抗风压试验数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B300抗风压试验数据DataTable B300Table
        {
            get { return _b300Table; }
            set
            {
                _b300Table = value;
                RaisePropertyChanged(() => B300Table);
            }
        }

        /// <summary>
        /// 幕墙四性B311a抗风压定级组1点a位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B311a抗风压定级组1点a位移数据DataTable _b311aTable;

        /// <summary>
        /// 幕墙四性B311a抗风压定级组1点a位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B311a抗风压定级组1点a位移数据DataTable B311aTable
        {
            get { return _b311aTable; }
            set
            {
                _b311aTable = value;
                RaisePropertyChanged(() => B311aTable);
            }
        }

        /// <summary>
        /// 幕墙四性B311b抗风压定级组1点b位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B311b抗风压定级组1点b位移数据DataTable _b311bTable;

        /// <summary>
        /// 幕墙四性B311b抗风压定级组1点b位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B311b抗风压定级组1点b位移数据DataTable B311bTable
        {
            get { return _b311bTable; }
            set
            {
                _b311bTable = value;
                RaisePropertyChanged(() => B311bTable);
            }
        }

        /// <summary>
        /// 幕墙四性B311c抗风压定级组1点c位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B311c抗风压定级组1点c位移数据DataTable _b311cTable;

        /// <summary>
        /// 幕墙四性B311c抗风压定级组1点c位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B311c抗风压定级组1点c位移数据DataTable B311cTable
        {
            get { return _b311cTable; }
            set
            {
                _b311cTable = value;
                RaisePropertyChanged(() => B311cTable);
            }
        }

        /// <summary>
        /// 幕墙四性B311d抗风压定级组1点d位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B311d抗风压定级组1点d位移数据DataTable _b311dTable;

        /// <summary>
        /// 幕墙四性B311d抗风压定级组1点d位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B311d抗风压定级组1点d位移数据DataTable B311dTable
        {
            get { return _b311dTable; }
            set
            {
                _b311dTable = value;
                RaisePropertyChanged(() => B311dTable);
            }
        }

        /// <summary>
        /// 幕墙四性B312a抗风压定级组1点a位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B312a抗风压定级组2点a位移数据DataTable _b312aTable;

        /// <summary>
        /// 幕墙四性B312a抗风压定级组2点a位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B312a抗风压定级组2点a位移数据DataTable B312aTable
        {
            get { return _b312aTable; }
            set
            {
                _b312aTable = value;
                RaisePropertyChanged(() => B312aTable);
            }
        }

        /// <summary>
        /// 幕墙四性B312b抗风压定级组2点b位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B312b抗风压定级组2点b位移数据DataTable _b312bTable;

        /// <summary>
        /// 幕墙四性B312b抗风压定级组2点b位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B312b抗风压定级组2点b位移数据DataTable B312bTable
        {
            get { return _b312bTable; }
            set
            {
                _b312bTable = value;
                RaisePropertyChanged(() => B312bTable);
            }
        }

        /// <summary>
        /// 幕墙四性B312c抗风压定级组2点c位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B312c抗风压定级组2点c位移数据DataTable _b312cTable;

        /// <summary>
        /// 幕墙四性B312c抗风压定级组2点c位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B312c抗风压定级组2点c位移数据DataTable B312cTable
        {
            get { return _b312cTable; }
            set
            {
                _b312cTable = value;
                RaisePropertyChanged(() => B312cTable);
            }
        }

        /// <summary>
        /// 幕墙四性B313a抗风压定级组3点a位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B313a抗风压定级组3点a位移数据DataTable _b313aTable;

        /// <summary>
        /// 幕墙四性B313a抗风压定级组3点a位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B313a抗风压定级组3点a位移数据DataTable B313aTable
        {
            get { return _b313aTable; }
            set
            {
                _b313aTable = value;
                RaisePropertyChanged(() => B313aTable);
            }
        }

        /// <summary>
        /// 幕墙四性B313b抗风压定级组3点b位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B313b抗风压定级组3点b位移数据DataTable _b313bTable;

        /// <summary>
        /// 幕墙四性B313b抗风压定级组3点b位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B313b抗风压定级组3点b位移数据DataTable B313bTable
        {
            get { return _b313bTable; }
            set
            {
                _b313bTable = value;
                RaisePropertyChanged(() => B313bTable);
            }
        }

        /// <summary>
        /// 幕墙四性B313c抗风压定级组3点c位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B313c抗风压定级组3点c位移数据DataTable _b313cTable;

        /// <summary>
        /// 幕墙四性B313c抗风压定级组3点c位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B313c抗风压定级组3点c位移数据DataTable B313cTable
        {
            get { return _b313cTable; }
            set
            {
                _b313cTable = value;
                RaisePropertyChanged(() => B313cTable);
            }
        }

        /// <summary>
        /// 幕墙四性B321抗风压定级组1相对挠度数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B321抗风压定级组1相对挠度数据DataTable  _b321Table;
        /// <summary>
        /// 幕墙四性B321抗风压定级组1相对挠度数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B321抗风压定级组1相对挠度数据DataTable B321Table
        {
            get { return _b321Table; }
            set
            {
                _b321Table = value;
                RaisePropertyChanged(() => B321Table);
            }
        }

        /// <summary>
        /// 幕墙四性B322抗风压定级组2相对挠度数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B322抗风压定级组2相对挠度数据DataTable _b322Table;
        /// <summary>
        /// 幕墙四性B322抗风压定级组2相对挠度数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B322抗风压定级组2相对挠度数据DataTable B322Table
        {
            get { return _b322Table; }
            set
            {
                _b322Table = value;
                RaisePropertyChanged(() => B322Table);
            }
        }

        /// <summary>
        /// 幕墙四性B323抗风压定级组3相对挠度数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B323抗风压定级组3相对挠度数据DataTable _b323Table;
        /// <summary>
        /// 幕墙四性B323抗风压定级组3相对挠度数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B323抗风压定级组3相对挠度数据DataTable B323Table
        {
            get { return _b323Table; }
            set
            {
                _b323Table = value;
                RaisePropertyChanged(() => B323Table);
            }
        }

        /// <summary>
        /// 幕墙四性B324抗风压定级相对挠度最大值数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B324抗风压定级相对挠度最大值数据DataTable _b324Table;
        /// <summary>
        /// 幕墙四性B324抗风压定级相对挠度最大值数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B324抗风压定级相对挠度最大值数据DataTable B324Table
        {
            get { return _b324Table; }
            set
            {
                _b324Table = value;
                RaisePropertyChanged(() => B324Table);
            }
        }

        /// <summary>
        /// 幕墙四性B331抗风压定级组1挠度数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B331抗风压定级组1挠度数据DataTable _b331Table;
        /// <summary>
        /// 幕墙四性B331抗风压定级组1挠度数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B331抗风压定级组1挠度数据DataTable B331Table
        {
            get { return _b331Table; }
            set
            {
                _b331Table = value;
                RaisePropertyChanged(() => B331Table);
            }
        }

        /// <summary>
        /// 幕墙四性B332抗风压定级组2挠度数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B332抗风压定级组2挠度数据DataTable _b332Table;
        /// <summary>
        /// 幕墙四性B332抗风压定级组2挠度数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B332抗风压定级组2挠度数据DataTable B332Table
        {
            get { return _b332Table; }
            set
            {
                _b332Table = value;
                RaisePropertyChanged(() => B332Table);
            }
        }

        /// <summary>
        /// 幕墙四性B333抗风压定级组3挠度数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B333抗风压定级组3挠度数据DataTable _b333Table;
        /// <summary>
        /// 幕墙四性B333抗风压定级组3挠度数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B333抗风压定级组3挠度数据DataTable B333Table
        {
            get { return _b333Table; }
            set
            {
                _b333Table = value;
                RaisePropertyChanged(() => B333Table);
            }
        }

        /// <summary>
        /// 幕墙四性B334抗风压定级挠度最大值数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B334抗风压定级挠度最大值数据DataTable _b334Table;
        /// <summary>
        /// 幕墙四性B334抗风压定级挠度最大值数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B334抗风压定级挠度最大值数据DataTable B334Table
        {
            get { return _b334Table; }
            set
            {
                _b334Table = value;
                RaisePropertyChanged(() => B334Table);
            }
        }

        /// <summary>
        /// 幕墙四性B340抗风压定级损坏情况数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B340抗风压定级损坏情况数据DataTable  _b340Table;
        /// <summary>
        /// 幕墙四性B340抗风压定级损坏情况数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B340抗风压定级损坏情况数据DataTable B340Table
        {
            get { return _b340Table; }
            set
            {
                _b340Table = value;
                RaisePropertyChanged(() => B340Table);
            }
        }

        /// <summary>
        /// 幕墙四性B350抗风压定级损坏说明数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B350抗风压定级损坏说明数据DataTable _b350Table;
        /// <summary>
        /// 幕墙四性B350抗风压定级损坏说明数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B350抗风压定级损坏说明数据DataTable B350Table
        {
            get { return _b350Table; }
            set
            {
                _b350Table = value;
                RaisePropertyChanged(() => B350Table);
            }
        }

        /// <summary>
        /// 幕墙四性B361抗风压组1工程位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B361抗风压组1工程位移数据DataTable _b361Table;
        /// <summary>
        /// 幕墙四性B361抗风压组1工程位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B361抗风压组1工程位移数据DataTable B361Table
        {
            get { return _b361Table; }
            set
            {
                _b361Table = value;
                RaisePropertyChanged(() => B361Table);
            }
        }

        /// <summary>
        /// 幕墙四性B362抗风压组2工程位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B362抗风压组2工程位移数据DataTable _b362Table;
        /// <summary>
        /// 幕墙四性B361抗风压组1工程位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B362抗风压组2工程位移数据DataTable B362Table
        {
            get { return _b362Table; }
            set
            {
                _b362Table = value;
                RaisePropertyChanged(() => B362Table);
            }
        }

        /// <summary>
        /// 幕墙四性B363抗风压组3工程位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B363抗风压组3工程位移数据DataTable _b363Table;
        /// <summary>
        /// 幕墙四性B361抗风压组1工程位移数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B363抗风压组3工程位移数据DataTable B363Table
        {
            get { return _b363Table; }
            set
            {
                _b363Table = value;
                RaisePropertyChanged(() => B363Table);
            }
        }

        /// <summary>
        /// 幕墙四性B371抗风压工程相对挠度数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B371抗风压工程相对挠度数据DataTable _b371Table;
        /// <summary>
        /// 幕墙四性B371抗风压工程相对挠度数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B371抗风压工程相对挠度数据DataTable B371Table
        {
            get { return _b371Table; }
            set
            {
                _b371Table = value;
                RaisePropertyChanged(() => B371Table);
            }
        }

        /// <summary>
        /// 幕墙四性B372抗风压工程相对挠度最大值数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B372抗风压工程相对挠度最大值数据DataTable _b372Table;
        /// <summary>
        /// 幕墙四性B372抗风压工程相对挠度最大值数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B372抗风压工程相对挠度最大值数据DataTable B372Table
        {
            get { return _b372Table; }
            set
            {
                _b372Table = value;
                RaisePropertyChanged(() => B372Table);
            }
        }

        /// <summary>
        /// 幕墙四性B381抗风压工程挠度数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B381抗风压工程挠度数据DataTable _b381Table;
        /// <summary>
        /// 幕墙四性B381抗风压工程挠度数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B381抗风压工程挠度数据DataTable B381Table
        {
            get { return _b381Table; }
            set
            {
                _b381Table = value;
                RaisePropertyChanged(() => B381Table);
            }
        }

        /// <summary>
        /// 幕墙四性B382抗风压工程挠度最大值数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B382抗风压工程挠度最大值数据DataTable _b382Table;
        /// <summary>
        /// 幕墙四性B382抗风压工程挠度最大值数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B382抗风压工程挠度最大值数据DataTable B382Table
        {
            get { return _b382Table; }
            set
            {
                _b382Table = value;
                RaisePropertyChanged(() => B382Table);
            }
        }

        /// <summary>
        /// 幕墙四性B390抗风压工程损坏数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B390抗风压工程损坏数据DataTable _b390Table;
        /// <summary>
        /// 幕墙四性B390抗风压工程损坏数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B390抗风压工程损坏数据DataTable B390Table
        {
            get { return _b390Table; }
            set
            {
                _b390Table = value;
                RaisePropertyChanged(() => B390Table);
            }
        }
        
        /// <summary>
        /// 幕墙四性B4层间变形试验数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B4层间变形试验数据DataTable _b4Table;

        /// <summary>
        /// 幕墙四性B4层间变形试验数据DataTable
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSet.B4层间变形试验数据DataTable B4Table
        {
            get { return _b4Table; }
            set
            {
                _b4Table = value;
                RaisePropertyChanged(() => B4Table);
            }
        }

        #endregion

        #endregion


        #region TableAdapter属性

        #region A试验及进度

        /// <summary>
        /// 幕墙四性A00试验参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.A00试验参数TableAdapter _a00TableAdapter;

        /// <summary>
        /// 幕墙四性A00试验参数TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.A00试验参数TableAdapter A00TableAdapter
        {
            get { return _a00TableAdapter; }
            set
            {
                _a00TableAdapter = value;
                RaisePropertyChanged(() => A00TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性A00试验参数2TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.A00试验参数2TableAdapter _a00Table2Adapter;

        /// <summary>
        /// 幕墙四性A00试验参数2TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.A00试验参数2TableAdapter A00Table2Adapter
        {
            get { return _a00Table2Adapter; }
            set
            {
                _a00Table2Adapter = value;
                RaisePropertyChanged(() => A00Table2Adapter);
            }
        }

        /// <summary>
        /// 幕墙四性A01气密试验进度TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.A01气密试验进度TableAdapter _a01TableAdapter;

        /// <summary>
        /// 幕墙四性A01气密试验进度TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.A01气密试验进度TableAdapter A01TableAdapter
        {
            get { return _a01TableAdapter; }
            set
            {
                _a01TableAdapter = value;
                RaisePropertyChanged(() => A01TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性A02水密试验进度TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.A02水密试验进度TableAdapter _a02TableAdapter;

        /// <summary>
        /// 幕墙四性A02水密试验进度TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.A02水密试验进度TableAdapter A02TableAdapter
        {
            get { return _a02TableAdapter; }
            set
            {
                _a02TableAdapter = value;
                RaisePropertyChanged(() => A02TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性A03抗风压试验进度TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.A03抗风压试验进度TableAdapter _a03TableAdapter;

        /// <summary>
        /// 幕墙四性A03抗风压试验进度TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.A03抗风压试验进度TableAdapter A03TableAdapter
        {
            get { return _a03TableAdapter; }
            set
            {
                _a03TableAdapter = value;
                RaisePropertyChanged(() => A03TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性A04层间变形试验进度TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.A04层间变形试验进度TableAdapter _a04TableAdapter;

        /// <summary>
        /// 幕墙四性A04层间变形试验进度TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.A04层间变形试验进度TableAdapter A04TableAdapter
        {
            get { return _a04TableAdapter; }
            set
            {
                _a04TableAdapter = value;
                RaisePropertyChanged(() => A04TableAdapter);
            }
        }

        #endregion

        #region B试验数据

        /// <summary>
        /// 幕墙四性B1气密数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B1气密试验数据TableAdapter _b1TableAdapter;

        /// <summary>
        /// 幕墙四性B1气密数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B1气密试验数据TableAdapter B1TableAdapter
        {
            get { return _b1TableAdapter; }
            set
            {
                _b1TableAdapter = value;
                RaisePropertyChanged(() => B1TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B2水密数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B2水密试验数据TableAdapter _b2TableAdapter;

        /// <summary>
        /// 幕墙四性B2水密数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B2水密试验数据TableAdapter B2TableAdapter
        {
            get { return _b2TableAdapter; }
            set
            {
                _b2TableAdapter = value;
                RaisePropertyChanged(() => B2TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B300抗风压数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B300抗风压试验数据TableAdapter _b300TableAdapter;

        /// <summary>
        /// 幕墙四性B300抗风压数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B300抗风压试验数据TableAdapter B300TableAdapter
        {
            get { return _b300TableAdapter; }
            set
            {
                _b300TableAdapter = value;
                RaisePropertyChanged(() => B300TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B311a抗风压定级组1点a位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B311a抗风压定级组1点a位移数据TableAdapter _b311aTableAdapter;

        /// <summary>
        /// 幕墙四性B311a抗风压定级组1点a位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B311a抗风压定级组1点a位移数据TableAdapter B311aTableAdapter
        {
            get { return _b311aTableAdapter; }
            set
            {
                _b311aTableAdapter = value;
                RaisePropertyChanged(() => B311aTableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B311b抗风压定级组1点b位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B311b抗风压定级组1点b位移数据TableAdapter _b311bTableAdapter;

        /// <summary>
        /// 幕墙四性B311b抗风压定级组1点b位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B311b抗风压定级组1点b位移数据TableAdapter B311bTableAdapter
        {
            get { return _b311bTableAdapter; }
            set
            {
                _b311bTableAdapter = value;
                RaisePropertyChanged(() => B311bTableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B311c抗风压定级组1点c位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B311c抗风压定级组1点c位移数据TableAdapter _b311cTableAdapter;

        /// <summary>
        /// 幕墙四性B311c抗风压定级组1点c位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B311c抗风压定级组1点c位移数据TableAdapter B311cTableAdapter
        {
            get { return _b311cTableAdapter; }
            set
            {
                _b311cTableAdapter = value;
                RaisePropertyChanged(() => B311cTableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B311d抗风压定级组1点d位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B311d抗风压定级组1点d位移数据TableAdapter _b311dTableAdapter;

        /// <summary>
        /// 幕墙四性B311d抗风压定级组1点d位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B311d抗风压定级组1点d位移数据TableAdapter B311dTableAdapter
        {
            get { return _b311dTableAdapter; }
            set
            {
                _b311dTableAdapter = value;
                RaisePropertyChanged(() => B311dTableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B312a抗风压定级组2点a位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B312a抗风压定级组2点a位移数据TableAdapter _b312aTableAdapter;

        /// <summary>
        /// 幕墙四性B312a抗风压定级组2点a位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B312a抗风压定级组2点a位移数据TableAdapter B312aTableAdapter
        {
            get { return _b312aTableAdapter; }
            set
            {
                _b312aTableAdapter = value;
                RaisePropertyChanged(() => B312aTableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B312b抗风压定级组2点b位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B312b抗风压定级组2点b位移数据TableAdapter _b312bTableAdapter;

        /// <summary>
        /// 幕墙四性B312b抗风压定级组2点b位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B312b抗风压定级组2点b位移数据TableAdapter B312bTableAdapter
        {
            get { return _b312bTableAdapter; }
            set
            {
                _b312bTableAdapter = value;
                RaisePropertyChanged(() => B312bTableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B312c抗风压定级组2点c位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B312c抗风压定级组2点c位移数据TableAdapter _b312cTableAdapter;

        /// <summary>
        /// 幕墙四性B312c抗风压定级组2点c位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B312c抗风压定级组2点c位移数据TableAdapter B312cTableAdapter
        {
            get { return _b312cTableAdapter; }
            set
            {
                _b312cTableAdapter = value;
                RaisePropertyChanged(() => B312cTableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B313a抗风压定级组3点a位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B313a抗风压定级组3点a位移数据TableAdapter _b313aTableAdapter;

        /// <summary>
        /// 幕墙四性B313a抗风压定级组3点a位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B313a抗风压定级组3点a位移数据TableAdapter B313aTableAdapter
        {
            get { return _b313aTableAdapter; }
            set
            {
                _b313aTableAdapter = value;
                RaisePropertyChanged(() => B313aTableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B313b抗风压定级组3点b位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B313b抗风压定级组3点b位移数据TableAdapter _b313bTableAdapter;

        /// <summary>
        /// 幕墙四性B313b抗风压定级组3点b位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B313b抗风压定级组3点b位移数据TableAdapter B313bTableAdapter
        {
            get { return _b313bTableAdapter; }
            set
            {
                _b313bTableAdapter = value;
                RaisePropertyChanged(() => B313bTableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B313c抗风压定级组3点c位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B313c抗风压定级组3点c位移数据TableAdapter _b313cTableAdapter;

        /// <summary>
        /// 幕墙四性B313c抗风压定级组3点c位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B313c抗风压定级组3点c位移数据TableAdapter B313cTableAdapter
        {
            get { return _b313cTableAdapter; }
            set
            {
                _b313cTableAdapter = value;
                RaisePropertyChanged(() => B313cTableAdapter);
            }
        }
        
        /// <summary>
        /// 幕墙四性B321抗风压定级组1相对挠度数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B321抗风压定级组1相对挠度数据TableAdapter  _b321TableAdapter;

        /// <summary>
        /// 幕墙四性B321抗风压定级组1相对挠度数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B321抗风压定级组1相对挠度数据TableAdapter B321TableAdapter
        {
            get { return _b321TableAdapter; }
            set
            {
                _b321TableAdapter = value;
                RaisePropertyChanged(() => B321TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B322抗风压定级组2相对挠度数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B322抗风压定级组2相对挠度数据TableAdapter _b322TableAdapter;

        /// <summary>
        /// 幕墙四性B322抗风压定级组2相对挠度数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B322抗风压定级组2相对挠度数据TableAdapter B322TableAdapter
        {
            get { return _b322TableAdapter; }
            set
            {
                _b322TableAdapter = value;
                RaisePropertyChanged(() => B322TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B323抗风压定级组3相对挠度数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B323抗风压定级组3相对挠度数据TableAdapter _b323TableAdapter;

        /// <summary>
        /// 幕墙四性B323抗风压定级组3相对挠度数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B323抗风压定级组3相对挠度数据TableAdapter B323TableAdapter
        {
            get { return _b323TableAdapter; }
            set
            {
                _b323TableAdapter = value;
                RaisePropertyChanged(() => B323TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B324抗风压定级相对挠度最大值数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B324抗风压定级相对挠度最大值数据TableAdapter _b324TableAdapter;

        /// <summary>
        /// 幕墙四性B324抗风压定级相对挠度最大值数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B324抗风压定级相对挠度最大值数据TableAdapter B324TableAdapter
        {
            get { return _b324TableAdapter; }
            set
            {
                _b324TableAdapter = value;
                RaisePropertyChanged(() => B324TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B331抗风压定级组1挠度数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B331抗风压定级组1挠度数据TableAdapter  _b331TableAdapter;

        /// <summary>
        /// 幕墙四性B331抗风压定级组1挠度数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B331抗风压定级组1挠度数据TableAdapter B331TableAdapter
        {
            get { return _b331TableAdapter; }
            set
            {
                _b331TableAdapter = value;
                RaisePropertyChanged(() => B331TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B332抗风压定级组2挠度数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B332抗风压定级组2挠度数据TableAdapter _b332TableAdapter;

        /// <summary>
        /// 幕墙四性B332抗风压定级组2挠度数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B332抗风压定级组2挠度数据TableAdapter B332TableAdapter
        {
            get { return _b332TableAdapter; }
            set
            {
                _b332TableAdapter = value;
                RaisePropertyChanged(() => B332TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B333抗风压定级组3挠度数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B333抗风压定级组3挠度数据TableAdapter _b333TableAdapter;

        /// <summary>
        /// 幕墙四性B333抗风压定级组3挠度数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B333抗风压定级组3挠度数据TableAdapter B333TableAdapter
        {
            get { return _b333TableAdapter; }
            set
            {
                _b333TableAdapter = value;
                RaisePropertyChanged(() => B333TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B334抗风压定级挠度最大值数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B334抗风压定级挠度最大值数据TableAdapter _b334TableAdapter;

        /// <summary>
        /// 幕墙四性B334抗风压定级挠度最大值数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B334抗风压定级挠度最大值数据TableAdapter B334TableAdapter
        {
            get { return _b334TableAdapter; }
            set
            {
                _b334TableAdapter = value;
                RaisePropertyChanged(() => B334TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B340抗风压定级损坏情况数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B340抗风压定级损坏情况数据TableAdapter _b340TableAdapter;

        /// <summary>
        /// 幕墙四性B340抗风压定级损坏情况数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B340抗风压定级损坏情况数据TableAdapter B340TableAdapter
        {
            get { return _b340TableAdapter; }
            set
            {
                _b340TableAdapter = value;
                RaisePropertyChanged(() => B340TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B350抗风压定级损坏说明数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B350抗风压定级损坏说明数据TableAdapter _b350TableAdapter;

        /// <summary>
        /// 幕墙四性B350抗风压定级损坏说明数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B350抗风压定级损坏说明数据TableAdapter B350TableAdapter
        {
            get { return _b350TableAdapter; }
            set
            {
                _b350TableAdapter = value;
                RaisePropertyChanged(() => B350TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B361抗风压组1工程位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B361抗风压组1工程位移数据TableAdapter _b361TableAdapter;

        /// <summary>
        /// 幕墙四性B361抗风压组1工程位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B361抗风压组1工程位移数据TableAdapter B361TableAdapter
        {
            get { return _b361TableAdapter; }
            set
            {
                _b361TableAdapter = value;
                RaisePropertyChanged(() => B361TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B362抗风压组2工程位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B362抗风压组2工程位移数据TableAdapter _b362TableAdapter;

        /// <summary>
        /// 幕墙四性B362抗风压组2工程位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B362抗风压组2工程位移数据TableAdapter B362TableAdapter
        {
            get { return _b362TableAdapter; }
            set
            {
                _b362TableAdapter = value;
                RaisePropertyChanged(() => B362TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B363抗风压组3工程位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B363抗风压组3工程位移数据TableAdapter _b363TableAdapter;

        /// <summary>
        /// 幕墙四性B363抗风压组3工程位移数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B363抗风压组3工程位移数据TableAdapter B363TableAdapter
        {
            get { return _b363TableAdapter; }
            set
            {
                _b363TableAdapter = value;
                RaisePropertyChanged(() => B363TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B371抗风压工程相对挠度数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B371抗风压工程相对挠度数据TableAdapter _b371TableAdapter;
        /// <summary>
        /// 幕墙四性B371抗风压工程相对挠度数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B371抗风压工程相对挠度数据TableAdapter B371TableAdapter
        {
            get { return _b371TableAdapter; }
            set
            {
                _b371TableAdapter = value;
                RaisePropertyChanged(() => B371TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B372抗风压工程相对挠度最大值数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B372抗风压工程相对挠度最大值数据TableAdapter _b372TableAdapter;
        /// <summary>
        /// 幕墙四性B372抗风压工程相对挠度最大值数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B372抗风压工程相对挠度最大值数据TableAdapter B372TableAdapter
        {
            get { return _b372TableAdapter; }
            set
            {
                _b372TableAdapter = value;
                RaisePropertyChanged(() => B372TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B381抗风压工程挠度数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B381抗风压工程挠度数据TableAdapter _b381TableAdapter;
        /// <summary>
        /// 幕墙四性B381抗风压工程挠度数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B381抗风压工程挠度数据TableAdapter B381TableAdapter
        {
            get { return _b381TableAdapter; }
            set
            {
                _b381TableAdapter = value;
                RaisePropertyChanged(() => B381TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B382抗风压工程挠度最大值数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B382抗风压工程挠度最大值数据TableAdapter _b382TableAdapter;
        /// <summary>
        /// 幕墙四性B382抗风压工程挠度最大值数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B382抗风压工程挠度最大值数据TableAdapter B382TableAdapter
        {
            get { return _b382TableAdapter; }
            set
            {
                _b382TableAdapter = value;
                RaisePropertyChanged(() => B382TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B390抗风压工程损坏数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B390抗风压工程损坏数据TableAdapter  _b390TableAdapter;
        /// <summary>
        /// 幕墙四性B390抗风压工程损坏数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B390抗风压工程损坏数据TableAdapter B390TableAdapter
        {
            get { return _b390TableAdapter; }
            set
            {
                _b390TableAdapter = value;
                RaisePropertyChanged(() => B390TableAdapter);
            }
        }

        /// <summary>
        /// 幕墙四性B4层间变形数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B4层间变形试验数据TableAdapter _b4TableAdapter;

        /// <summary>
        /// 幕墙四性B4层间变形数据TableAdapter
        /// </summary>
        private MQDFJ_MB.MQZH_DB_TestDataSetTableAdapters.B4层间变形试验数据TableAdapter B4TableAdapter
        {
            get { return _b4TableAdapter; }
            set
            {
                _b4TableAdapter = value;
                RaisePropertyChanged(() => B4TableAdapter);
            }
        }

        #endregion

        #endregion


        #region DataTable、TableAdapter初始化、更新

        /// <summary>
        /// Dataset、DataTable、TableAdapter初始化、读取数据
        /// </summary>
        private void SetTableAdapterInit_Exp()
        {
            //TableAdapter初始化
            A00TableAdapter = new A00试验参数TableAdapter();
            A00Table2Adapter= new A00试验参数2TableAdapter();
            A01TableAdapter = new A01气密试验进度TableAdapter();
            A02TableAdapter = new A02水密试验进度TableAdapter();
            A03TableAdapter = new A03抗风压试验进度TableAdapter();
            A04TableAdapter = new A04层间变形试验进度TableAdapter();
            B1TableAdapter = new B1气密试验数据TableAdapter();
            B2TableAdapter = new B2水密试验数据TableAdapter();
            B300TableAdapter = new B300抗风压试验数据TableAdapter();
            B311aTableAdapter = new B311a抗风压定级组1点a位移数据TableAdapter();
            B311bTableAdapter = new B311b抗风压定级组1点b位移数据TableAdapter();
            B311cTableAdapter = new B311c抗风压定级组1点c位移数据TableAdapter();
            B311dTableAdapter = new B311d抗风压定级组1点d位移数据TableAdapter();
            B312aTableAdapter = new B312a抗风压定级组2点a位移数据TableAdapter();
            B312bTableAdapter = new B312b抗风压定级组2点b位移数据TableAdapter();
            B312cTableAdapter = new B312c抗风压定级组2点c位移数据TableAdapter();
            B313aTableAdapter = new B313a抗风压定级组3点a位移数据TableAdapter();
            B313bTableAdapter = new B313b抗风压定级组3点b位移数据TableAdapter();
            B313cTableAdapter = new B313c抗风压定级组3点c位移数据TableAdapter();
            B321TableAdapter = new B321抗风压定级组1相对挠度数据TableAdapter();
            B322TableAdapter = new B322抗风压定级组2相对挠度数据TableAdapter();
            B323TableAdapter = new B323抗风压定级组3相对挠度数据TableAdapter();
            B324TableAdapter = new B324抗风压定级相对挠度最大值数据TableAdapter();
            B331TableAdapter = new B331抗风压定级组1挠度数据TableAdapter();
            B332TableAdapter = new B332抗风压定级组2挠度数据TableAdapter();
            B333TableAdapter = new B333抗风压定级组3挠度数据TableAdapter();
            B334TableAdapter = new B334抗风压定级挠度最大值数据TableAdapter();
            B340TableAdapter = new B340抗风压定级损坏情况数据TableAdapter();
            B350TableAdapter = new B350抗风压定级损坏说明数据TableAdapter();
            B361TableAdapter = new B361抗风压组1工程位移数据TableAdapter();
            B362TableAdapter = new B362抗风压组2工程位移数据TableAdapter();
            B363TableAdapter = new B363抗风压组3工程位移数据TableAdapter();
            B371TableAdapter = new B371抗风压工程相对挠度数据TableAdapter();
            B372TableAdapter = new B372抗风压工程相对挠度最大值数据TableAdapter();
            B381TableAdapter = new B381抗风压工程挠度数据TableAdapter();
            B382TableAdapter = new B382抗风压工程挠度最大值数据TableAdapter();
            B390TableAdapter = new B390抗风压工程损坏数据TableAdapter();
            B4TableAdapter = new B4层间变形试验数据TableAdapter();

            //DataTable初始化
            A00Table = new MQZH_DB_TestDataSet.A00试验参数DataTable();
            A00Table2 = new MQZH_DB_TestDataSet.A00试验参数2DataTable();
            A01Table = new MQZH_DB_TestDataSet.A01气密试验进度DataTable();
            A02Table = new MQZH_DB_TestDataSet.A02水密试验进度DataTable();
            A03Table = new MQZH_DB_TestDataSet.A03抗风压试验进度DataTable();
            A04Table = new MQZH_DB_TestDataSet.A04层间变形试验进度DataTable();
            B1Table = new MQZH_DB_TestDataSet.B1气密试验数据DataTable();
            B2Table = new MQZH_DB_TestDataSet.B2水密试验数据DataTable();
            B300Table = new MQZH_DB_TestDataSet.B300抗风压试验数据DataTable();
            B311aTable = new MQZH_DB_TestDataSet.B311a抗风压定级组1点a位移数据DataTable();
            B311bTable = new MQZH_DB_TestDataSet.B311b抗风压定级组1点b位移数据DataTable();
            B311cTable = new MQZH_DB_TestDataSet.B311c抗风压定级组1点c位移数据DataTable();
            B311dTable = new MQZH_DB_TestDataSet.B311d抗风压定级组1点d位移数据DataTable();
            B312aTable = new MQZH_DB_TestDataSet.B312a抗风压定级组2点a位移数据DataTable();
            B312bTable = new MQZH_DB_TestDataSet.B312b抗风压定级组2点b位移数据DataTable();
            B312cTable = new MQZH_DB_TestDataSet.B312c抗风压定级组2点c位移数据DataTable();
            B313aTable = new MQZH_DB_TestDataSet.B313a抗风压定级组3点a位移数据DataTable();
            B313bTable = new MQZH_DB_TestDataSet.B313b抗风压定级组3点b位移数据DataTable();
            B313cTable = new MQZH_DB_TestDataSet.B313c抗风压定级组3点c位移数据DataTable();
            B321Table = new MQZH_DB_TestDataSet.B321抗风压定级组1相对挠度数据DataTable();
            B322Table = new MQZH_DB_TestDataSet.B322抗风压定级组2相对挠度数据DataTable();
            B323Table = new MQZH_DB_TestDataSet.B323抗风压定级组3相对挠度数据DataTable();
            B324Table = new MQZH_DB_TestDataSet.B324抗风压定级相对挠度最大值数据DataTable();
            B331Table = new MQZH_DB_TestDataSet.B331抗风压定级组1挠度数据DataTable();
            B332Table = new MQZH_DB_TestDataSet.B332抗风压定级组2挠度数据DataTable();
            B333Table = new MQZH_DB_TestDataSet.B333抗风压定级组3挠度数据DataTable();
            B334Table = new MQZH_DB_TestDataSet.B334抗风压定级挠度最大值数据DataTable();
            B340Table = new MQZH_DB_TestDataSet.B340抗风压定级损坏情况数据DataTable();
            B350Table = new MQZH_DB_TestDataSet.B350抗风压定级损坏说明数据DataTable();
            B361Table = new MQZH_DB_TestDataSet.B361抗风压组1工程位移数据DataTable();
            B362Table = new MQZH_DB_TestDataSet.B362抗风压组2工程位移数据DataTable();
            B363Table = new MQZH_DB_TestDataSet.B363抗风压组3工程位移数据DataTable();
            B371Table = new MQZH_DB_TestDataSet.B371抗风压工程相对挠度数据DataTable();
            B372Table = new MQZH_DB_TestDataSet.B372抗风压工程相对挠度最大值数据DataTable();
            B381Table = new MQZH_DB_TestDataSet.B381抗风压工程挠度数据DataTable();
            B382Table = new MQZH_DB_TestDataSet.B382抗风压工程挠度最大值数据DataTable();
            B390Table = new MQZH_DB_TestDataSet.B390抗风压工程损坏数据DataTable();
            B4Table = new MQZH_DB_TestDataSet.B4层间变形试验数据DataTable();

            //数据读取
            A00TableAdapter.Fill(A00Table);
            A00Table2Adapter.Fill(A00Table2);
            A01TableAdapter.Fill(A01Table);
            A02TableAdapter.Fill(A02Table);
            A03TableAdapter.Fill(A03Table);
            A04TableAdapter.Fill(A04Table);
            B1TableAdapter.Fill(B1Table);
            B2TableAdapter.Fill(B2Table);
            B300TableAdapter.Fill(B300Table);
            B311aTableAdapter.Fill(B311aTable);
            B311bTableAdapter.Fill(B311bTable);
            B311cTableAdapter.Fill(B311cTable);
            B311dTableAdapter.Fill(B311dTable);
            B312aTableAdapter.Fill(B312aTable);
            B312bTableAdapter.Fill(B312bTable);
            B312cTableAdapter.Fill(B312cTable);
            B313aTableAdapter.Fill(B313aTable);
            B313bTableAdapter.Fill(B313bTable);
            B313cTableAdapter.Fill(B313cTable);
            B321TableAdapter.Fill(B321Table);
            B322TableAdapter.Fill(B322Table);
            B323TableAdapter.Fill(B323Table);
            B324TableAdapter.Fill(B324Table);
            B331TableAdapter.Fill(B331Table);
            B332TableAdapter.Fill(B332Table);
            B333TableAdapter.Fill(B333Table);
            B334TableAdapter.Fill(B334Table);
            B340TableAdapter.Fill(B340Table);
            B350TableAdapter.Fill(B350Table);
            B361TableAdapter.Fill(B361Table);
            B362TableAdapter.Fill(B362Table);
            B363TableAdapter.Fill(B363Table);
            B371TableAdapter.Fill(B371Table);
            B372TableAdapter.Fill(B372Table);
            B381TableAdapter.Fill(B381Table);
            B382TableAdapter.Fill(B382Table);
            B390TableAdapter.Fill(B390Table);
            B4TableAdapter.Fill(B4Table);
        }


        #endregion


        #region 试验管理、操作消息回调

        /// <summary>
        /// 根据指定编号新建试验（从默认试验复制）
        /// </summary>
        /// <param name="msg">新建试验编号</param>
        private void NewExpMessage(string msg)
        {
            string oldExpName = "DefaultExp";
            string newExpName = msg;
            if (CopyExp(oldExpName, newExpName, true))
            {
                Messenger.Default.Send<MQZH_DB_TestDataSet.A00试验参数DataTable>(A00Table, "ExpTableChanged");
                MessageBox.Show(newExpName + "号试验已新建完毕！", "提示");
            }
            else
            {
                MessageBox.Show("新建试验失败，数据库中可能残存信息！", "错误提示");
            }
            Messenger.Default.Send(A00Table.Clone(), "ExpTableChanged");
        }


        /// <summary>
        /// 复制指定名称的试验至新试验
        /// </summary>
        /// <param name="msg">被复制试验编号和新试验编号</param>
        private void CopyExpMessage(string[] msg)
        {
            string oldExpName = msg[0];
            string newExpName = msg[1];
            if (CopyExp(oldExpName, newExpName, false))
            {
                Messenger.Default.Send<MQZH_DB_TestDataSet.A00试验参数DataTable>(A00Table, "ExpTableChanged");
                MessageBox.Show("已复制" + oldExpName + "号试验至" + newExpName + "号试验。", "提示");
            }
            else
            {
                MessageBox.Show("复制试验失败，数据库中可能残存信息！", "错误提示");
            }
        }


        /// <summary>
        ///根据编号删除试验消息回调
        /// </summary>
        /// <param name="msg">试验编号</param>
        private void DelExpMessage(string msg)
        {
            if (DelExp(msg))
            {
                Messenger.Default.Send<MQZH_DB_TestDataSet.A00试验参数DataTable>(A00Table, "ExpTableChanged");
                MessageBox.Show(msg + "号试验已删除！", "提示");
            }
            else
            {
                MessageBox.Show("删除试验失败！", "错误提示");
            }
        }

        #endregion
    }
}