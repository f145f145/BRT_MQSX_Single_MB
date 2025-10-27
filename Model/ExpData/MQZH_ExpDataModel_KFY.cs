/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQZHWL.Model.ExpData
 * 文件名：  MQZH_ExpDataModel_KFY
 * 版本号：  V1.0.0.0
 * 唯一标识：ba780e56-9b11-4983-bdd8-f742353423af
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/12/25 7:23:54
 * 描述：
 * 抗风压实验数据
 * ==================================================================================
 * 修改标记
 * 修改时间			        修改人			版本号			描述
 * 2021/12/25 7:23:54		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace MQZHWL.Model.ExpData
{
    public class MQZH_ExpDataModel_KFY : ObservableObject
    {
        public  MQZH_ExpDataModel_KFY ()
        {
            InitDJ() ;
            InitGC();
        }

        #region 定级检测值

        /// <summary>
        /// 试件最终是否损坏
        /// </summary>
        private bool _isDamage = false;
        /// <summary>
        ///  试件最终是否损坏
        /// </summary>
        public bool IsDamage
        {
            get { return _isDamage; }
            set
            {
                _isDamage = value;
                RaisePropertyChanged(() => IsDamage);
            }
        }

        #region 各级检测位移记录

        /// <summary>
        /// 定级变形正压检测位移值（mm）（位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _wy_DJBX_Z;
        /// <summary>
        ///  定级变形正压检测位移值（mm）（位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> WY_DJBX_Z
        {
            get { return _wy_DJBX_Z; }
            set
            {
                _wy_DJBX_Z = value;
                RaisePropertyChanged(() => WY_DJBX_Z);
            }
        }

        /// <summary>
        /// 定级变形负压检测位移值（mm）（位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _wy_DJBX_F;
        /// <summary>
        ///  定级变形负压检测位移值（mm）（位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> WY_DJBX_F
        {
            get { return _wy_DJBX_F; }
            set
            {
                _wy_DJBX_F = value;
                RaisePropertyChanged(() => WY_DJBX_F);
            }
        }

        /// <summary>
        /// 定级p3正压检测位移值（mm）(0正，位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _wy_DJP3_Z;
        /// <summary>
        ///  定级p3正压检测位移值（mm）(0正，位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> WY_DJP3_Z
        {
            get { return _wy_DJP3_Z; }
            set
            {
                _wy_DJP3_Z = value;
                RaisePropertyChanged(() => WY_DJP3_Z);
            }
        }

        /// <summary>
        /// 定级p3负压检测位移值（mm）(0负，位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _wy_DJP3_F;
        /// <summary>
        ///  定级p3负压检测位移值（mm）(0负，位移尺编号）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> WY_DJP3_F
        {
            get { return _wy_DJP3_F; }
            set
            {
                _wy_DJP3_F = value;
                RaisePropertyChanged(() => WY_DJP3_F);
            }
        }

        /// <summary>
        /// 定级pmax检测正压位移值（mm）(0正，位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _wy_DJPmax_Z;
        /// <summary>
        /// 定级pmax检测正压位移值（mm）(0正，位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> WY_DJPmax_Z
        {
            get { return _wy_DJPmax_Z; }
            set
            {
                _wy_DJPmax_Z = value;
                RaisePropertyChanged(() => WY_DJPmax_Z);
            }
        }

        /// <summary>
        /// 定级pmax检测负压位移值（mm）(0负，位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _wy_DJPmax_F;
        /// <summary>
        /// 定级pmax检测负压位移值（mm）(0负，位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> WY_DJPmax_F
        {
            get { return _wy_DJPmax_F; }
            set
            {
                _wy_DJPmax_F = value;
                RaisePropertyChanged(() => WY_DJPmax_F);
            }
        }

        #endregion

        #region 各级检测相对挠度记录

        /// <summary>
        /// 定级变形正压检测相对挠度值(压力步骤，测点组挠度）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _xdnd_DJBX_Z;
        /// <summary>
        /// 定级变形正压检测相对挠度值(压力步骤，测点组挠度）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> XDND_DJBX_Z
        {
            get { return _xdnd_DJBX_Z; }
            set
            {
                _xdnd_DJBX_Z = value;
                RaisePropertyChanged(() => XDND_DJBX_Z);
            }
        }

        /// <summary>
        /// 定级变形负压检测相对挠度值(压力步骤，测点组挠度）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _xdnd_DJBX_F;
        /// <summary>
        ///  定级变形负压检测相对挠度值(压力步骤，测点组挠度）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> XDND_DJBX_F
        {
            get { return _xdnd_DJBX_F; }
            set
            {
                _xdnd_DJBX_F = value;
                RaisePropertyChanged(() => XDND_DJBX_F);
            }
        }

        /// <summary>
        ///定级p3正压检测相对挠度值(压力步骤，测点组挠度）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _xdnd_DJP3_Z;
        /// <summary>
        ///  定级p3正压检测相对挠度值(压力步骤，测点组挠度）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> XDND_DJP3_Z
        {
            get { return _xdnd_DJP3_Z; }
            set
            {
                _xdnd_DJP3_Z = value;
                RaisePropertyChanged(() => XDND_DJP3_Z);
            }
        }

        /// <summary>
        /// 定级p3负压检测相对挠度值(压力步骤，测点组挠度）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _xdnd_DJP3_F;
        /// <summary>
        ///  定级p3负压检测相对挠度值(压力步骤，测点组挠度）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> XDND_DJP3_F
        {
            get { return _xdnd_DJP3_F; }
            set
            {
                _xdnd_DJP3_F = value;
                RaisePropertyChanged(() => XDND_DJP3_F);
            }
        }

        /// <summary>
        /// 定级pmax正压检测相对挠度值(压力步骤，测点组挠度）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _xdnd_DJPmax_Z;
        /// <summary>
        /// 定级pmax正压检测相对挠度值(压力步骤，测点组挠度）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> XDND_DJPmax_Z
        {
            get { return _xdnd_DJPmax_Z; }
            set
            {
                _xdnd_DJPmax_Z = value;
                RaisePropertyChanged(() => XDND_DJPmax_Z);
            }
        }

        /// <summary>
        /// 定级pmax负压检测相对挠度值(压力步骤，测点组挠度）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _xdnd_DJPmax_F;
        /// <summary>
        /// 定级pmax负压检测相对挠度值(压力步骤，测点组挠度）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> XDND_DJPmax_F
        {
            get { return _xdnd_DJPmax_F; }
            set
            {
                _xdnd_DJPmax_F = value;
                RaisePropertyChanged(() => XDND_DJPmax_F);
            }
        }

        #endregion

        #region 各级检测挠度记录

        /// <summary>
        /// 定级变形正压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _nd_DJBX_Z;
        /// <summary>
        /// 定级变形正压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> ND_DJBX_Z
        {
            get { return _nd_DJBX_Z; }
            set
            {
                _nd_DJBX_Z = value;
                RaisePropertyChanged(() => ND_DJBX_Z);
            }
        }

        /// <summary>
        /// 定级变形负压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _nd_DJBX_F;
        /// <summary>
        /// 定级变形负压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> ND_DJBX_F
        {
            get { return _nd_DJBX_F; }
            set
            {
                _nd_DJBX_F = value;
                RaisePropertyChanged(() => ND_DJBX_F);
            }
        }

        /// <summary>
        /// 定级p3正压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _nd_DJP3_Z;
        /// <summary>
        /// 定级p3正压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> ND_DJP3_Z
        {
            get { return _nd_DJP3_Z; }
            set
            {
                _nd_DJP3_Z = value;
                RaisePropertyChanged(() => ND_DJP3_Z);
            }
        }

        /// <summary>
        /// 定级p3负压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _nd_DJP3_F;
        /// <summary>
        /// 定级p3负压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> ND_DJP3_F
        {
            get { return _nd_DJP3_F; }
            set
            {
                _nd_DJP3_F = value;
                RaisePropertyChanged(() => ND_DJP3_F);
            }
        }

        /// <summary>
        /// 定级pmax正压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _nd_DJPmax_Z;
        /// <summary>
        /// 定级pmax正压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> ND_DJPmax_Z
        {
            get { return _nd_DJPmax_Z; }
            set
            {
                _nd_DJPmax_Z = value;
                RaisePropertyChanged(() => ND_DJPmax_Z);
            }
        }

        /// <summary>
        /// 定级pmax负压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _nd_DJPmax_F;
        /// <summary>
        /// 定级pmax负压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> ND_DJPmax_F
        {
            get { return _nd_DJPmax_F; }
            set
            {
                _nd_DJPmax_F = value;
                RaisePropertyChanged(() => ND_DJPmax_F);
            }
        }


        #endregion

        #region 各级损坏情况记录

        /// <summary>
        /// 定级变形正压检测损坏情况
        /// </summary>
        private ObservableCollection<bool> _damage_DJBX_Z = new ObservableCollection<bool>()
        {
            false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false,false, false, false, false, false
        };
        /// <summary>
        ///  定级变形正压检测损坏情况
        /// </summary>
        public ObservableCollection<bool> Damage_DJBX_Z
        {
            get { return _damage_DJBX_Z; }
            set
            {
                _damage_DJBX_Z = value;
                RaisePropertyChanged(() => Damage_DJBX_Z);
            }
        }

        /// <summary>
        /// 定级变形负压检测损坏情况
        /// </summary>
        private ObservableCollection<bool> _damage_DJBX_F = new ObservableCollection<bool>()
        {
            false, false, false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, false, false
        };
        /// <summary>
        ///  定级变形负压检测损坏情况
        /// </summary>
        public ObservableCollection<bool> Damage_DJBX_F
        {
            get { return _damage_DJBX_F; }
            set
            {
                _damage_DJBX_F = value;
                RaisePropertyChanged(() => Damage_DJBX_F);
            }
        }

        /// <summary>
        ///定级p2正压检测损坏情况
        /// </summary>
        private bool _damage_DJP2_Z=false;
        /// <summary>
        ///  定级p2正压检测损坏情况
        /// </summary>
        public bool Damage_DJP2_Z
        {
            get { return _damage_DJP2_Z; }
            set
            {
                _damage_DJP2_Z = value;
                RaisePropertyChanged(() => Damage_DJP2_Z);
            }
        }

        /// <summary>
        /// 定级p2负压检测损坏情况
        /// </summary>
        private bool _damage_DJP2_F = false;
        /// <summary>
        ///  定级p2负压检测损坏情况
        /// </summary>
        public bool Damage_DJP2_F
        {
            get { return _damage_DJP2_F; }
            set
            {
                _damage_DJP2_F = value;
                RaisePropertyChanged(() => Damage_DJP2_F);
            }
        }

        /// <summary>
        ///定级p3正压检测损坏情况
        /// </summary>
        private bool _damage_DJP3_Z = false;
        /// <summary>
        ///  定级p3正压检测损坏情况
        /// </summary>
        public bool Damage_DJP3_Z
        {
            get { return _damage_DJP3_Z; }
            set
            {
                _damage_DJP3_Z = value;
                RaisePropertyChanged(() => Damage_DJP3_Z);
            }
        }

        /// <summary>
        /// 定级p3负压检测损坏情况
        /// </summary>
        private bool _damage_DJP3_F = false;
        /// <summary>
        ///  定级p3负压检测损坏情况
        /// </summary>
        public bool Damage_DJP3_F
        {
            get { return _damage_DJP3_F; }
            set
            {
                _damage_DJP3_F = value;
                RaisePropertyChanged(() => Damage_DJP3_F);
            }
        }

        /// <summary>
        /// 定级pmax正压检测损坏情况
        /// </summary>
        private bool _damage_DJPmax_Z = false;
        /// <summary>
        /// 定级pmax正压检测损坏情况
        /// </summary>
        public bool Damage_DJPmax_Z
        {
            get { return _damage_DJPmax_Z; }
            set
            {
                _damage_DJPmax_Z = value;
                RaisePropertyChanged(() => Damage_DJPmax_Z);
            }
        }

        /// <summary>
        /// 定级pmax负压检测损坏情况
        /// </summary>
        private bool _damage_DJPmax_F = false;
        /// <summary>
        /// 定级pmax负压检测损坏情况
        /// </summary>
        public bool Damage_DJPmax_F
        {
            get { return _damage_DJPmax_F; }
            set
            {
                _damage_DJPmax_F = value;
                RaisePropertyChanged(() => Damage_DJPmax_F);
            }
        }

        #endregion

        #region 各级损坏说明记录

        /// <summary>
        /// 定级变形正压检测损坏说明
        /// </summary>
        private ObservableCollection<string> _damagePS_DJBX_Z = new ObservableCollection<string>()
        {
            "","","","","","","","","","",
            "","","","","","","","","",""
        };
        /// <summary>
        ///  定级变形正压检测损坏说明
        /// </summary>
        public ObservableCollection<string> DamagePS_DJBX_Z
        {
            get { return _damagePS_DJBX_Z; }
            set
            {
                _damagePS_DJBX_Z = value;
                RaisePropertyChanged(() => DamagePS_DJBX_Z);
            }
        }

        /// <summary>
        /// 定级变形负压检测损坏说明
        /// </summary>
        private ObservableCollection<string> _damagePS_DJBX_F = new ObservableCollection<string>()
        {
            "","","","","","","","","","",
            "","","","","","","","","",""
        };
        /// <summary>
        ///  定级变形负压检测损坏说明
        /// </summary>
        public ObservableCollection<string> DamagePS_DJBX_F
        {
            get { return _damagePS_DJBX_F; }
            set
            {
                _damagePS_DJBX_F = value;
                RaisePropertyChanged(() => DamagePS_DJBX_F);
            }
        }

        /// <summary>
        ///定级p2正压检测损坏说明
        /// </summary>
        private string _damagePS_DJP2_Z="";
        /// <summary>
        ///  定级p2正压检测损坏说明
        /// </summary>
        public string DamagePS_DJP2_Z
        {
            get { return _damagePS_DJP2_Z; }
            set
            {
                _damagePS_DJP2_Z = value;
                RaisePropertyChanged(() => DamagePS_DJP2_Z);
            }
        }

        /// <summary>
        /// 定级p2负压检测损坏说明
        /// </summary>
        private string _damagePS_DJP2_F = "";
        /// <summary>
        ///  定级p2负压检测损坏说明
        /// </summary>
        public string DamagePS_DJP2_F
        {
            get { return _damagePS_DJP2_F; }
            set
            {
                _damagePS_DJP2_F = value;
                RaisePropertyChanged(() => DamagePS_DJP2_F);
            }
        }

        /// <summary>
        ///定级p3正压检测损坏说明
        /// </summary>
        private string _damagePS_DJP3_Z = "";
        /// <summary>
        ///  定级p3正压检测损坏说明
        /// </summary>
        public string DamagePS_DJP3_Z
        {
            get { return _damagePS_DJP3_Z; }
            set
            {
                _damagePS_DJP3_Z = value;
                RaisePropertyChanged(() => DamagePS_DJP3_Z);
            }
        }

        /// <summary>
        /// 定级p3负压检测损坏说明
        /// </summary>
        private string _damagePS_DJP3_F = "";
        /// <summary>
        ///  定级p3负压检测损坏说明
        /// </summary>
        public string DamagePS_DJP3_F
        {
            get { return _damagePS_DJP3_F; }
            set
            {
                _damagePS_DJP3_F = value;
                RaisePropertyChanged(() => DamagePS_DJP3_F);
            }
        }

        /// <summary>
        /// 定级pmax正压检测损坏说明
        /// </summary>
        private string _damagePS_DJPmax_Z = "";
        /// <summary>
        /// 定级pmax正压检测损坏说明
        /// </summary>
        public string DamagePS_DJPmax_Z
        {
            get { return _damagePS_DJPmax_Z; }
            set
            {
                _damagePS_DJPmax_Z = value;
                RaisePropertyChanged(() => DamagePS_DJPmax_Z);
            }
        }

        /// <summary>
        /// 定级pmax负压检测损坏说明
        /// </summary>
        private string _damagePS_DJPmax_F = "";
        /// <summary>
        /// 定级pmax负压检测损坏说明
        /// </summary>
        public string DamagePS_DJPmax_F
        {
            get { return _damagePS_DJPmax_F; }
            set
            {
                _damagePS_DJPmax_F = value;
                RaisePropertyChanged(() => DamagePS_DJPmax_F);
            }
        }

        #endregion

        #region 实测压力

        /// <summary>
        /// 定级变形正压实测压力
        /// </summary>
        private ObservableCollection<double> _testPress_DJBX_Z =
            new ObservableCollection<double>()
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };
        /// <summary>
        /// 定级变形正压实测压力
        /// </summary>
        public ObservableCollection<double> TestPress_DJBX_Z
        {
            get { return _testPress_DJBX_Z; }
            set
            {
                _testPress_DJBX_Z = value;
                RaisePropertyChanged(() => TestPress_DJBX_Z);
            }
        }

        /// <summary>
        /// 定级变形负压实测压力
        /// </summary>
        private ObservableCollection<double> _testPress_DJBX_F =
            new ObservableCollection<double>()
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };
        /// <summary>
        /// 定级变形负压实测压力
        /// </summary>
        public ObservableCollection<double> TestPress_DJBX_F
        {
            get { return _testPress_DJBX_F; }
            set
            {
                _testPress_DJBX_F = value;
                RaisePropertyChanged(() => TestPress_DJBX_F);
            }
        }

        /// <summary>
        /// 定级反复正压实测压力（平均值）
        /// </summary>
        private double _testPress_DJP2_Z = 0;
        /// <summary>
        /// 定级反复正压实测压力（平均值）
        /// </summary>
        public double TestPress_DJP2_Z
        {
            get { return _testPress_DJP2_Z; }
            set
            {
                _testPress_DJP2_Z = value;
                RaisePropertyChanged(() => TestPress_DJP2_Z);
            }
        }

        /// <summary>
        /// 定级反复负压实测压力（平均值）
        /// </summary>
        private double _testPress_DJP2_F =0;
        /// <summary>
        /// 定级反复负压实测压力（平均值）
        /// </summary>
        public double TestPress_DJP2_F
        {
            get { return _testPress_DJP2_F; }
            set
            {
                _testPress_DJP2_F = value;
                RaisePropertyChanged(() => TestPress_DJP2_F);
            }
        }

        /// <summary>
        /// 定级标准值正压实测压力
        /// </summary>
        private double _testPress_DJP3_Z = 0;
        /// <summary>
        /// 定级标准值正压实测压力
        /// </summary>
        public double TestPress_DJP3_Z
        {
            get { return _testPress_DJP3_Z; }
            set
            {
                _testPress_DJP3_Z = value;
                RaisePropertyChanged(() => TestPress_DJP3_Z);
            }
        }

        /// <summary>
        /// 定级标准值负压实测压力
        /// </summary>
        private double _testPress_DJP3_F = 0;
        /// <summary>
        /// 定级标准值负压实测压力
        /// </summary>
        public double TestPress_DJP3_F
        {
            get { return _testPress_DJP3_F; }
            set
            {
                _testPress_DJP3_F = value;
                RaisePropertyChanged(() => TestPress_DJP3_F);
            }
        }

        /// <summary>
        /// 定级设计值正压实测压力
        /// </summary>
        private double _testPress_DJPmax_Z = 0;
        /// <summary>
        /// 定级设计值正压实测压力
        /// </summary>
        public double TestPress_DJPmax_Z
        {
            get { return _testPress_DJPmax_Z; }
            set
            {
                _testPress_DJPmax_Z = value;
                RaisePropertyChanged(() => TestPress_DJPmax_Z);
            }
        }

        /// <summary>
        /// 定级设计值负压实测压力
        /// </summary>
        private double _testPress_DJPmax_F = 0;
        /// <summary>
        /// 定级设计值负压实测压力
        /// </summary>
        public double TestPress_DJPmax_F
        {
            get { return _testPress_DJPmax_F; }
            set
            {
                _testPress_DJPmax_F = value;
                RaisePropertyChanged(() => TestPress_DJPmax_F);
            }
        }

        #endregion

        #region 各阶段挠度最大值

        /// <summary>
        /// 定级变形正压检测各步骤挠度值最大值
        /// </summary>
        private ObservableCollection<double> _nd_Max_DJBX_Z = new ObservableCollection<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// 定级变形正压检测各步骤挠度值最大值
        /// </summary>
        public ObservableCollection<double> ND_Max_DJBX_Z
        {
            get { return _nd_Max_DJBX_Z; }
            set
            {
                _nd_Max_DJBX_Z = value;
                RaisePropertyChanged(() => ND_Max_DJBX_Z);
            }
        }

        /// <summary>
        /// 定级变形负压检测各步骤挠度值最大值
        /// </summary>
        private ObservableCollection<double> _nd_Max_DJBX_F = new ObservableCollection<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// 定级变形负压检测各步骤挠度值最大值
        /// </summary>
        public ObservableCollection<double> ND_Max_DJBX_F
        {
            get { return _nd_Max_DJBX_F; }
            set
            {
                _nd_Max_DJBX_F = value;
                RaisePropertyChanged(() => ND_Max_DJBX_F);
            }
        }

        /// <summary>
        /// 定级p3正压检测各步骤挠度值最大值
        /// </summary>
        private double _nd_Max_DJp3_Z = 0;
        /// <summary>
        /// 定级p3正压检测各步骤挠度值最大值
        /// </summary>
        public double ND_Max_DJp3_Z
        {
            get { return _nd_Max_DJp3_Z; }
            set
            {
                _nd_Max_DJp3_Z = value;
                RaisePropertyChanged(() => ND_Max_DJp3_Z);
            }
        }

        /// <summary>
        /// 定级p3负压检测各步骤挠度值最大值
        /// </summary>
        private double _nd_Max_DJp3_F = 0;
        /// <summary>
        /// 定级p3负压检测各步骤挠度值最大值
        /// </summary>
        public double ND_Max_DJp3_F
        {
            get { return _nd_Max_DJp3_F; }
            set
            {
                _nd_Max_DJp3_F = value;
                RaisePropertyChanged(() => ND_Max_DJp3_F);
            }
        }

        /// <summary>
        /// 定级pmax正压检测各步骤挠度值最大值
        /// </summary>
        private double _nd_Max_DJpmax_Z = 0;
        /// <summary>
        /// 定级pmax正压检测各步骤挠度值最大值
        /// </summary>
        public double ND_Max_DJpmax_Z
        {
            get { return _nd_Max_DJpmax_Z; }
            set
            {
                _nd_Max_DJpmax_Z = value;
                RaisePropertyChanged(() => ND_Max_DJpmax_Z);
            }
        }

        /// <summary>
        /// 定级pmax负压检测各步骤挠度值最大值
        /// </summary>
        private double _nd_Max_DJpmax_F = 0;
        /// <summary>
        /// 定级pmax负压检测各步骤挠度值最大值
        /// </summary>
        public double ND_Max_DJpmax_F
        {
            get { return _nd_Max_DJpmax_F; }
            set
            {
                _nd_Max_DJpmax_F = value;
                RaisePropertyChanged(() => ND_Max_DJpmax_F);
            }
        }


        #endregion

        #region 各阶段相对挠度最大值

        /// <summary>
        /// 定级变形正压检测各步骤相对挠度值最大值，分母
        /// </summary>
        private ObservableCollection<double> _xdnd_Max_DJBX_Z = new ObservableCollection<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// 定级变形正压检测各步骤相对挠度值最大值，分母
        /// </summary>
        public ObservableCollection<double> XDND_Max_DJBX_Z
        {
            get { return _xdnd_Max_DJBX_Z; }
            set
            {
                _xdnd_Max_DJBX_Z = value;
                RaisePropertyChanged(() => XDND_Max_DJBX_Z);
            }
        }

        /// <summary>
        /// 定级变形负压检测各步骤相对挠度值最大值，分母
        /// </summary>
        private ObservableCollection<double> _xdnd_Max_DJBX_F = new ObservableCollection<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// 定级变形负压检测各步骤相对挠度值最大值，分母
        /// </summary>
        public ObservableCollection<double> XDND_Max_DJBX_F
        {
            get { return _xdnd_Max_DJBX_F; }
            set
            {
                _xdnd_Max_DJBX_F = value;
                RaisePropertyChanged(() => XDND_Max_DJBX_F);
            }
        }

        /// <summary>
        /// 定级p3正压检测各步骤相对挠度值最大值，分母
        /// </summary>
        private double _xdnd_Max_DJp3_Z = 0;
        /// <summary>
        /// 定级p3正压检测各步骤相对挠度值最大值，分母
        /// </summary>
        public double XDND_Max_DJp3_Z
        {
            get { return _xdnd_Max_DJp3_Z; }
            set
            {
                _xdnd_Max_DJp3_Z = value;
                RaisePropertyChanged(() => XDND_Max_DJp3_Z);
            }
        }

        /// <summary>
        /// 定级p3负压检测各步骤相对挠度值最大值，分母
        /// </summary>
        private double _xdnd_Max_DJp3_F = 0;
        /// <summary>
        /// 定级p3负压检测各步骤相对挠度值最大值，分母
        /// </summary>
        public double XDND_Max_DJp3_F
        {
            get { return _xdnd_Max_DJp3_F; }
            set
            {
                _xdnd_Max_DJp3_F = value;
                RaisePropertyChanged(() => XDND_Max_DJp3_F);
            }
        }

        /// <summary>
        /// 定级pmax正压检测各步骤相对挠度值最大值，分母
        /// </summary>
        private double _xdnd_Max_DJpmax_Z = 0;
        /// <summary>
        /// 定级pmax正压检测各步骤相对挠度值最大值，分母
        /// </summary>
        public double XDND_Max_DJpmax_Z
        {
            get { return _xdnd_Max_DJpmax_Z; }
            set
            {
                _xdnd_Max_DJpmax_Z = value;
                RaisePropertyChanged(() => XDND_Max_DJpmax_Z);
            }
        }

        /// <summary>
        /// 定级pmax负压检测各步骤相对挠度值最大值，分母
        /// </summary>
        private double _xdnd_Max_DJpmax_F = 0;
        /// <summary>
        /// 定级pmax负压检测各步骤相对挠度值最大值，分母
        /// </summary>
        public double XDND_Max_DJpmax_F
        {
            get { return _xdnd_Max_DJpmax_F; }
            set
            {
                _xdnd_Max_DJpmax_F = value;
                RaisePropertyChanged(() => XDND_Max_DJpmax_F);
            }
        }

        #endregion

        #region 评定压力

        /// <summary>
        /// +P1评定值正压
        /// </summary>
        private double _pdValue_DJP1_Z = 0;
        /// <summary>
        ///  +P1评定值正压
        /// </summary>
        public double PDValue_DJP1_Z
        {
            get { return _pdValue_DJP1_Z; }
            set
            {
                _pdValue_DJP1_Z = value;
                RaisePropertyChanged(() => PDValue_DJP1_Z);
            }
        }

        /// <summary>
        /// +P2评定值正压
        /// </summary>
        private double _pdValue_DJP2_Z = 0;
        /// <summary>
        ///  +P2评定值正压
        /// </summary>
        public double PDValue_DJP2_Z
        {
            get { return _pdValue_DJP2_Z; }
            set
            {
                _pdValue_DJP2_Z = value;
                RaisePropertyChanged(() => PDValue_DJP2_Z);
            }
        }

        /// <summary>
        /// +P3评定值正压
        /// </summary>
        private double _pdValue_DJP3_Z = 0;
        /// <summary>
        ///  +P3评定值正压
        /// </summary>
        public double PDValue_DJP3_Z
        {
            get { return _pdValue_DJP3_Z; }
            set
            {
                _pdValue_DJP3_Z = value;
                RaisePropertyChanged(() => PDValue_DJP3_Z);
            }
        }

        /// <summary>
        /// -P1评定值负压
        /// </summary>
        private double _pdValue_DJP1_F = 0;
        /// <summary>
        ///  -P1评定值负压
        /// </summary>
        public double PDValue_DJP1_F
        {
            get { return _pdValue_DJP1_F; }
            set
            {
                _pdValue_DJP1_F = value;
                RaisePropertyChanged(() => PDValue_DJP1_F);
            }
        }

        /// <summary>
        /// -P2评定值负压
        /// </summary>
        private double _pdValue_DJP2_F = 0;
        /// <summary>
        ///  -P2评定值负压
        /// </summary>
        public double PDValue_DJP2_F
        {
            get { return _pdValue_DJP2_F; }
            set
            {
                _pdValue_DJP2_F = value;
                RaisePropertyChanged(() => PDValue_DJP2_F);
            }
        }

        /// <summary>
        /// -P3评定值负压
        /// </summary>
        private double _pdValue_DJP3_F = 0;
        /// <summary>
        ///  -P3评定值负压
        /// </summary>
        public double PDValue_DJP3_F
        {
            get { return _pdValue_DJP3_F; }
            set
            {
                _pdValue_DJP3_F = value;
                RaisePropertyChanged(() => PDValue_DJP3_F);
            }
        }


        /// <summary>
        /// P1总评定值
        /// </summary>
        private double _pdValue_DJP1_All = 0;
        /// <summary>
        ///  P1总评定值
        /// </summary>
        public double PDValue_DJP1_All
        {
            get { return _pdValue_DJP1_All; }
            set
            {
                _pdValue_DJP1_All = value;
                RaisePropertyChanged(() => PDValue_DJP1_All);
            }
        }

        /// <summary>
        /// P2总评定值
        /// </summary>
        private double _pdValue_DJP2_All = 0;
        /// <summary>
        ///  P2总评定值
        /// </summary>
        public double PDValue_DJP2_All
        {
            get { return _pdValue_DJP2_All; }
            set
            {
                _pdValue_DJP2_All = value;
                RaisePropertyChanged(() => PDValue_DJP2_All);
            }
        }

        /// <summary>
        /// P3总评定值
        /// </summary>
        private double _pdValue_DJP3_All = 0;
        /// <summary>
        ///  P3总评定值
        /// </summary>
        public double PDValue_DJP3_All
        {
            get { return _pdValue_DJP3_All; }
            set
            {
                _pdValue_DJP3_All = value;
                RaisePropertyChanged(() => PDValue_DJP3_All);
            }
        }
        
        /// <summary>
        /// P3评级
        /// </summary>
        private int _pdLevel_DJP3 = 0;
        /// <summary>
        ///  P3评级
        /// </summary>
        public int PdLevel_DJP3
        {
            get { return _pdLevel_DJP3; }
            set
            {
                _pdLevel_DJP3 = value;
                RaisePropertyChanged(() => PdLevel_DJP3);
            }
        }

        #endregion

        #endregion


        #region 工程检测值

        #region 工程检测各级位移记录（有0压）

        /// <summary>
        /// 工程变形正压检测位移值（mm）（位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _wy_GCBX_Z;
        /// <summary>
        ///  工程变形正压检测位移值（mm）（位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> WY_GCBX_Z
        {
            get { return _wy_GCBX_Z; }
            set
            {
                _wy_GCBX_Z = value;
                RaisePropertyChanged(() => WY_GCBX_Z);
            }
        }

        /// <summary>
        /// 工程变形负压检测位移值（mm）（位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _wy_GCBX_F;
        /// <summary>
        ///  工程变形负压检测位移值（mm）（位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> WY_GCBX_F
        {
            get { return _wy_GCBX_F; }
            set
            {
                _wy_GCBX_F = value;
                RaisePropertyChanged(() => WY_GCBX_F);
            }
        }

        /// <summary>
        /// 工程p3正压检测位移值（mm）(0正，位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _wy_GCP3_Z;
        /// <summary>
        ///  工程p3正压检测位移值（mm）(0正，位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> WY_GCP3_Z
        {
            get { return _wy_GCP3_Z; }
            set
            {
                _wy_GCP3_Z = value;
                RaisePropertyChanged(() => WY_GCP3_Z);
            }
        }

        /// <summary>
        /// 工程p3负压检测位移值（mm）(0负，位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _wy_GCP3_F;
        /// <summary>
        ///  工程p3负压检测位移值（mm）(0负，位移尺编号）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> WY_GCP3_F
        {
            get { return _wy_GCP3_F; }
            set
            {
                _wy_GCP3_F = value;
                RaisePropertyChanged(() => WY_GCP3_F);
            }
        }

        /// <summary>
        /// 工程pmax检测正压位移值（mm）(0正，位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _wy_GCPmax_Z;
        /// <summary>
        /// 工程pmax检测正压位移值（mm）(0正，位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> WY_GCPmax_Z
        {
            get { return _wy_GCPmax_Z; }
            set
            {
                _wy_GCPmax_Z = value;
                RaisePropertyChanged(() => WY_GCPmax_Z);
            }
        }

        /// <summary>
        /// 工程pmax检测负压位移值（mm）(0负，位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _wy_GCPmax_F;
        /// <summary>
        /// 工程pmax检测负压位移值（mm）(0负，位移1a、位移1b、位移1c、位移1d；位移2a，位移2b，位移2c；位移3a，位移3b，位移3c）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> WY_GCPmax_F
        {
            get { return _wy_GCPmax_F; }
            set
            {
                _wy_GCPmax_F = value;
                RaisePropertyChanged(() => WY_GCPmax_F);
            }
        }

        #endregion

        #region 各级检测相对挠度记录（有0压）

        /// <summary>
        /// 工程变形正压检测相对挠度值(压力步骤，测点组挠度）分母
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _xdnd_GCBX_Z;
        /// <summary>
        /// 工程变形正压检测相对挠度值(压力步骤，测点组挠度）分母
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> XDND_GCBX_Z
        {
            get { return _xdnd_GCBX_Z; }
            set
            {
                _xdnd_GCBX_Z = value;
                RaisePropertyChanged(() => XDND_GCBX_Z);
            }
        }

        /// <summary>
        /// 工程变形负压检测相对挠度值(压力步骤，测点组挠度）分母
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _xdnd_GCBX_F;
        /// <summary>
        ///  工程变形负压检测相对挠度值(压力步骤，测点组挠度）分母
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> XDND_GCBX_F
        {
            get { return _xdnd_GCBX_F; }
            set
            {
                _xdnd_GCBX_F = value;
                RaisePropertyChanged(() => XDND_GCBX_F);
            }
        }

        /// <summary>
        ///工程p3正压检测相对挠度值(压力步骤，测点组挠度）分母
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _xdnd_GCP3_Z;
        /// <summary>
        ///  工程p3正压检测相对挠度值(压力步骤，测点组挠度）分母
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> XDND_GCP3_Z
        {
            get { return _xdnd_GCP3_Z; }
            set
            {
                _xdnd_GCP3_Z = value;
                RaisePropertyChanged(() => XDND_GCP3_Z);
            }
        }

        /// <summary>
        /// 工程p3负压检测相对挠度值(压力步骤，测点组挠度）分母
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _xdnd_GCP3_F;
        /// <summary>
        ///  工程p3负压检测相对挠度值(压力步骤，测点组挠度）分母
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> XDND_GCP3_F
        {
            get { return _xdnd_GCP3_F; }
            set
            {
                _xdnd_GCP3_F = value;
                RaisePropertyChanged(() => XDND_GCP3_F);
            }
        }

        /// <summary>
        /// 工程pmax正压检测相对挠度值(压力步骤，测点组挠度）分母
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _xdnd_GCPmax_Z;
        /// <summary>
        /// 工程pmax正压检测相对挠度值(压力步骤，测点组挠度）分母
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> XDND_GCPmax_Z
        {
            get { return _xdnd_GCPmax_Z; }
            set
            {
                _xdnd_GCPmax_Z = value;
                RaisePropertyChanged(() => XDND_GCPmax_Z);
            }
        }

        /// <summary>
        /// 工程pmax负压检测相对挠度值(压力步骤，测点组挠度）分母
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _xdnd_GCPmax_F;
        /// <summary>
        /// 工程pmax负压检测相对挠度值(压力步骤，测点组挠度）分母
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> XDND_GCPmax_F
        {
            get { return _xdnd_GCPmax_F; }
            set
            {
                _xdnd_GCPmax_F = value;
                RaisePropertyChanged(() => XDND_GCPmax_F);
            }
        }
        
        #endregion

        #region 各级检测挠度记录（有0压）

        /// <summary>
        /// 工程变形正压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _nd_GCBX_Z;
        /// <summary>
        /// 工程变形正压检测相挠度值(压力步骤，测点组挠度）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> ND_GCBX_Z
        {
            get { return _nd_GCBX_Z; }
            set
            {
                _nd_GCBX_Z = value;
                RaisePropertyChanged(() => ND_GCBX_Z);
            }
        }

        /// <summary>
        /// 工程变形负压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _nd_GCBX_F;
        /// <summary>
        /// 工程变形负压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> ND_GCBX_F
        {
            get { return _nd_GCBX_F; }
            set
            {
                _nd_GCBX_F = value;
                RaisePropertyChanged(() => ND_GCBX_F);
            }
        }

        /// <summary>
        /// 工程p3正压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _nd_GCP3_Z;
        /// <summary>
        /// 工程p3正压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> ND_GCP3_Z
        {
            get { return _nd_GCP3_Z; }
            set
            {
                _nd_GCP3_Z = value;
                RaisePropertyChanged(() => ND_GCP3_Z);
            }
        }

        /// <summary>
        /// 工程p3负压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _nd_GCP3_F;
        /// <summary>
        /// 工程p3负压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> ND_GCP3_F
        {
            get { return _nd_GCP3_F; }
            set
            {
                _nd_GCP3_F = value;
                RaisePropertyChanged(() => ND_GCP3_F);
            }
        }

        /// <summary>
        /// 工程pmax正压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _nd_GCPmax_Z;
        /// <summary>
        /// 工程pmax正压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> ND_GCPmax_Z
        {
            get { return _nd_GCPmax_Z; }
            set
            {
                _nd_GCPmax_Z = value;
                RaisePropertyChanged(() => ND_GCPmax_Z);
            }
        }

        /// <summary>
        /// 工程pmax负压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        private ObservableCollection<ObservableCollection<double>> _nd_GCPmax_F;
        /// <summary>
        /// 工程pmax负压检测挠度值(压力步骤，测点组挠度）
        /// </summary>
        public ObservableCollection<ObservableCollection<double>> ND_GCPmax_F
        {
            get { return _nd_GCPmax_F; }
            set
            {
                _nd_GCPmax_F = value;
                RaisePropertyChanged(() => ND_GCPmax_F);
            }
        }

        #endregion

        #region 各级损坏情况记录

        /// <summary>
        /// 工程变形正压检测损坏情况
        /// </summary>
        private ObservableCollection<bool> _damage_GCBX_Z = new ObservableCollection<bool>()
        {
            false, false, false, false, false
        };
        /// <summary>
        ///  工程变形正压检测损坏情况
        /// </summary>
        public ObservableCollection<bool> Damage_GCBX_Z
        {
            get { return _damage_GCBX_Z; }
            set
            {
                _damage_GCBX_Z = value;
                RaisePropertyChanged(() => Damage_GCBX_Z);
            }
        }

        /// <summary>
        /// 工程变形负压检测损坏情况
        /// </summary>
        private ObservableCollection<bool> _damage_GCBX_F = new ObservableCollection<bool>()
        {
            false, false, false, false, false
        };
        /// <summary>
        ///  工程变形负压检测损坏情况
        /// </summary>
        public ObservableCollection<bool> Damage_GCBX_F
        {
            get { return _damage_GCBX_F; }
            set
            {
                _damage_GCBX_F = value;
                RaisePropertyChanged(() => Damage_GCBX_F);
            }
        }

        /// <summary>
        ///工程p2正压检测损坏情况
        /// </summary>
        private bool _damage_GCP2_Z = false;
        /// <summary>
        ///  工程p2正压检测损坏情况
        /// </summary>
        public bool Damage_GCP2_Z
        {
            get { return _damage_GCP2_Z; }
            set
            {
                _damage_GCP2_Z = value;
                RaisePropertyChanged(() => Damage_GCP2_Z);
            }
        }

        /// <summary>
        /// 工程p2负压检测损坏情况
        /// </summary>
        private bool _damage_GCP2_F = false;
        /// <summary>
        ///  工程p2负压检测损坏情况
        /// </summary>
        public bool Damage_GCP2_F
        {
            get { return _damage_GCP2_F; }
            set
            {
                _damage_GCP2_F = value;
                RaisePropertyChanged(() => Damage_GCP2_F);
            }
        }

        /// <summary>
        ///工程p3正压检测损坏情况
        /// </summary>
        private bool _damage_GCP3_Z = false;
        /// <summary>
        ///  工程p3正压检测损坏情况
        /// </summary>
        public bool Damage_GCP3_Z
        {
            get { return _damage_GCP3_Z; }
            set
            {
                _damage_GCP3_Z = value;
                RaisePropertyChanged(() => Damage_GCP3_Z);
            }
        }

        /// <summary>
        /// 工程p3负压检测损坏情况
        /// </summary>
        private bool _damage_GCP3_F = false;
        /// <summary>
        ///  工程p3负压检测损坏情况
        /// </summary>
        public bool Damage_GCP3_F
        {
            get { return _damage_GCP3_F; }
            set
            {
                _damage_GCP3_F = value;
                RaisePropertyChanged(() => Damage_GCP3_F);
            }
        }

        /// <summary>
        /// 工程pmax正压检测损坏情况
        /// </summary>
        private bool _damage_GCPmax_Z = false;
        /// <summary>
        /// 工程pmax正压检测损坏情况
        /// </summary>
        public bool Damage_GCPmax_Z
        {
            get { return _damage_GCPmax_Z; }
            set
            {
                _damage_GCPmax_Z = value;
                RaisePropertyChanged(() => Damage_GCPmax_Z);
            }
        }

        /// <summary>
        /// 工程pmax负压检测损坏情况
        /// </summary>
        private bool _damage_GCPmax_F = false;
        /// <summary>
        /// 工程pmax负压检测损坏情况
        /// </summary>
        public bool Damage_GCPmax_F
        {
            get { return _damage_GCPmax_F; }
            set
            {
                _damage_GCPmax_F = value;
                RaisePropertyChanged(() => Damage_GCPmax_F);
            }
        }

        #endregion

        #region 各级损坏说明记录

        /// <summary>
        /// 工程变形正压检测损坏说明
        /// </summary>
        private ObservableCollection<string> _damagePS_GCBX_Z = new ObservableCollection<string>()
        {
            "","","","",""
        };
        /// <summary>
        ///  工程变形正压检测损坏说明
        /// </summary>
        public ObservableCollection<string> DamagePS_GCBX_Z
        {
            get { return _damagePS_GCBX_Z; }
            set
            {
                _damagePS_GCBX_Z = value;
                RaisePropertyChanged(() => DamagePS_GCBX_Z);
            }
        }

        /// <summary>
        /// 工程变形负压检测损坏说明
        /// </summary>
        private ObservableCollection<string> _damagePS_GCBX_F = new ObservableCollection<string>()
        {
            "","","","",""
        };
        /// <summary>
        ///  工程变形负压检测损坏说明
        /// </summary>
        public ObservableCollection<string> DamagePS_GCBX_F
        {
            get { return _damagePS_GCBX_F; }
            set
            {
                _damagePS_GCBX_F = value;
                RaisePropertyChanged(() => DamagePS_GCBX_F);
            }
        }

        /// <summary>
        ///工程p2正压检测损坏说明
        /// </summary>
        private string _damagePS_GCP2_Z = "";
        /// <summary>
        ///  工程p2正压检测损坏说明
        /// </summary>
        public string DamagePS_GCP2_Z
        {
            get { return _damagePS_GCP2_Z; }
            set
            {
                _damagePS_GCP2_Z = value;
                RaisePropertyChanged(() => DamagePS_GCP2_Z);
            }
        }

        /// <summary>
        /// 工程p2负压检测损坏说明
        /// </summary>
        private string _damagePS_GCP2_F = "";
        /// <summary>
        ///  工程p2负压检测损坏说明
        /// </summary>
        public string DamagePS_GCP2_F
        {
            get { return _damagePS_GCP2_F; }
            set
            {
                _damagePS_GCP2_F = value;
                RaisePropertyChanged(() => DamagePS_GCP2_F);
            }
        }

        /// <summary>
        ///工程p3正压检测损坏说明
        /// </summary>
        private string _damagePS_GCP3_Z = "";
        /// <summary>
        ///  工程p3正压检测损坏说明
        /// </summary>
        public string DamagePS_GCP3_Z
        {
            get { return _damagePS_GCP3_Z; }
            set
            {
                _damagePS_GCP3_Z = value;
                RaisePropertyChanged(() => DamagePS_GCP3_Z);
            }
        }

        /// <summary>
        /// 工程p3负压检测损坏说明
        /// </summary>
        private string _damagePS_GCP3_F = "";
        /// <summary>
        ///  工程p3负压检测损坏说明
        /// </summary>
        public string DamagePS_GCP3_F
        {
            get { return _damagePS_GCP3_F; }
            set
            {
                _damagePS_GCP3_F = value;
                RaisePropertyChanged(() => DamagePS_GCP3_F);
            }
        }

        /// <summary>
        /// 工程pmax正压检测损坏说明
        /// </summary>
        private string _damagePS_GCPmax_Z = "";
        /// <summary>
        /// 工程pmax正压检测损坏说明
        /// </summary>
        public string DamagePS_GCPmax_Z
        {
            get { return _damagePS_GCPmax_Z; }
            set
            {
                _damagePS_GCPmax_Z = value;
                RaisePropertyChanged(() => DamagePS_GCPmax_Z);
            }
        }

        /// <summary>
        /// 工程pmax负压检测损坏说明
        /// </summary>
        private string _damagePS_GCPmax_F = "";
        /// <summary>
        /// 工程pmax负压检测损坏说明
        /// </summary>
        public string DamagePS_GCPmax_F
        {
            get { return _damagePS_GCPmax_F; }
            set
            {
                _damagePS_GCPmax_F = value;
                RaisePropertyChanged(() => DamagePS_GCPmax_F);
            }
        }

        #endregion

        #region 各阶段挠度最大值

        /// <summary>
        /// 工程变形正压检测各步骤挠度值最大值
        /// </summary>
        private ObservableCollection<double> _nd_Max_GCBX_Z = new ObservableCollection<double>() { 0, 0, 0, 0, 0 };
        /// <summary>
        /// 工程变形正压检测各步骤挠度值最大值
        /// </summary>
        public ObservableCollection<double> ND_Max_GCBX_Z
        {
            get { return _nd_Max_GCBX_Z; }
            set
            {
                _nd_Max_GCBX_Z = value;
                RaisePropertyChanged(() => ND_Max_GCBX_Z);
            }
        }

        /// <summary>
        /// 工程变形负压检测各步骤挠度值最大值
        /// </summary>
        private ObservableCollection<double> _nd_Max_GCBX_F = new ObservableCollection<double>() { 0, 0, 0, 0, 0 };
        /// <summary>
        /// 工程变形负压检测各步骤挠度值最大值
        /// </summary>
        public ObservableCollection<double> ND_Max_GCBX_F
        {
            get { return _nd_Max_GCBX_F; }
            set
            {
                _nd_Max_GCBX_F = value;
                RaisePropertyChanged(() => ND_Max_GCBX_F);
            }
        }

        /// <summary>
        /// 工程p3正压检测各步骤挠度值最大值
        /// </summary>
        private double _nd_Max_GCp3_Z = 0;
        /// <summary>
        /// 工程p3正压检测各步骤挠度值最大值
        /// </summary>
        public double ND_Max_GCp3_Z
        {
            get { return _nd_Max_GCp3_Z; }
            set
            {
                _nd_Max_GCp3_Z = value;
                RaisePropertyChanged(() => ND_Max_GCp3_Z);
            }
        }

        /// <summary>
        /// 工程p3负压检测各步骤挠度值最大值
        /// </summary>
        private double _nd_Max_GCp3_F = 0;
        /// <summary>
        /// 工程p3负压检测各步骤挠度值最大值
        /// </summary>
        public double ND_Max_GCp3_F
        {
            get { return _nd_Max_GCp3_F; }
            set
            {
                _nd_Max_GCp3_F = value;
                RaisePropertyChanged(() => ND_Max_GCp3_F);
            }
        }

        /// <summary>
        /// 工程pmax正压检测各步骤挠度值最大值
        /// </summary>
        private double _nd_Max_GCpmax_Z = 0;
        /// <summary>
        /// 工程pmax正压检测各步骤挠度值最大值
        /// </summary>
        public double ND_Max_GCpmax_Z
        {
            get { return _nd_Max_GCpmax_Z; }
            set
            {
                _nd_Max_GCpmax_Z = value;
                RaisePropertyChanged(() => ND_Max_GCpmax_Z);
            }
        }

        /// <summary>
        /// 工程pmax负压检测各步骤挠度值最大值
        /// </summary>
        private double _nd_Max_GCpmax_F = 0;
        /// <summary>
        /// 工程pmax负压检测各步骤挠度值最大值
        /// </summary>
        public double ND_Max_GCpmax_F
        {
            get { return _nd_Max_GCpmax_F; }
            set
            {
                _nd_Max_GCpmax_F = value;
                RaisePropertyChanged(() => ND_Max_GCpmax_F);
            }
        }

        #endregion
        
        #region 各阶段相对挠度最大值

        /// <summary>
        /// 工程变形正压检测各步骤相对挠度值最大值
        /// </summary>
        private ObservableCollection<double> _xdnd_Max_GCBX_Z=new ObservableCollection<double>(){ 0, 0, 0,0,0};
        /// <summary>
        /// 工程变形正压检测各步骤相对挠度值最大值
        /// </summary>
        public ObservableCollection<double> XDND_Max_GCBX_Z
        {
            get { return _xdnd_Max_GCBX_Z; }
            set
            {
                _xdnd_Max_GCBX_Z = value;
                RaisePropertyChanged(() => XDND_Max_GCBX_Z);
            }
        }

        /// <summary>
        /// 工程变形负压检测各步骤相对挠度值最大值
        /// </summary>
        private ObservableCollection<double> _xdnd_Max_GCBX_F = new ObservableCollection<double>() { 0, 0, 0, 0, 0 };
        /// <summary>
        /// 工程变形负压检测各步骤相对挠度值最大值
        /// </summary>
        public ObservableCollection<double> XDND_Max_GCBX_F
        {
            get { return _xdnd_Max_GCBX_F; }
            set
            {
                _xdnd_Max_GCBX_F = value;
                RaisePropertyChanged(() => XDND_Max_GCBX_F);
            }
        }
        
        /// <summary>
        /// 工程p3正压检测相对挠度值最大值
        /// </summary>
        private double _xdnd_Max_GCp3_Z = 0;
        /// <summary>
        /// 工程p3正压检测相对挠度值最大值
        /// </summary>
        public double XDND_Max_GCp3_Z
        {
            get { return _xdnd_Max_GCp3_Z; }
            set
            {
                _xdnd_Max_GCp3_Z = value;
                RaisePropertyChanged(() => XDND_Max_GCp3_Z);
            }
        }

        /// <summary>
        /// 工程p3负压检测相对挠度值最大值
        /// </summary>
        private double _xdnd_Max_GCp3_F = 0;
        /// <summary>
        /// 工程p3负压检测相对挠度值最大值
        /// </summary>
        public double XDND_Max_GCp3_F
        {
            get { return _xdnd_Max_GCp3_F; }
            set
            {
                _xdnd_Max_GCp3_F = value;
                RaisePropertyChanged(() => XDND_Max_GCp3_F);
            }
        }

        /// <summary>
        /// 工程pmax正压检测相对挠度值最大值
        /// </summary>
        private double _xdnd_Max_GCpmax_Z = 0;
        /// <summary>
        /// 工程pmax正压检测相对挠度值最大值
        /// </summary>
        public double XDND_Max_GCpmax_Z
        {
            get { return _xdnd_Max_GCpmax_Z; }
            set
            {
                _xdnd_Max_GCpmax_Z = value;
                RaisePropertyChanged(() => XDND_Max_GCpmax_Z);
            }
        }

        /// <summary>
        /// 工程pmax负压检测相对挠度值最大值
        /// </summary>
        private double _xdnd_Max_GCpmax_F = 0;
        /// <summary>
        /// 工程pmax负压检测相对挠度值最大值
        /// </summary>
        public double XDND_Max_GCpmax_F
        {
            get { return _xdnd_Max_GCpmax_F; }
            set
            {
                _xdnd_Max_GCpmax_F = value;
                RaisePropertyChanged(() => XDND_Max_GCpmax_F);
            }
        }

        #endregion

        #region 实测压力

        /// <summary>
        /// 工程变形正压实测压力
        /// </summary>
        private ObservableCollection<double> _testPress_GCBX_Z =
            new ObservableCollection<double>()
            {
                0, 0, 0, 0
            };
        /// <summary>
        /// 工程变形正压实测压力
        /// </summary>
        public ObservableCollection<double> TestPress_GCBX_Z
        {
            get { return _testPress_GCBX_Z; }
            set
            {
                _testPress_GCBX_Z = value;
                RaisePropertyChanged(() => TestPress_GCBX_Z);
            }
        }

        /// <summary>
        /// 工程变形负压实测压力
        /// </summary>
        private ObservableCollection<double> _testPress_GCBX_F =
            new ObservableCollection<double>()
            {
                0, 0, 0, 0
            };
        /// <summary>
        /// 工程变形负压实测压力
        /// </summary>
        public ObservableCollection<double> TestPress_GCBX_F
        {
            get { return _testPress_GCBX_F; }
            set
            {
                _testPress_GCBX_F = value;
                RaisePropertyChanged(() => TestPress_GCBX_F);
            }
        }

        /// <summary>
        /// 工程反复正压实测压力（平均值）
        /// </summary>
        private double _testPress_GCP2_Z = 0;
        /// <summary>
        /// 工程反复正压实测压力（平均值）
        /// </summary>
        public double TestPress_GCP2_Z
        {
            get { return _testPress_GCP2_Z; }
            set
            {
                _testPress_GCP2_Z = value;
                RaisePropertyChanged(() => TestPress_GCP2_Z);
            }
        }

        /// <summary>
        /// 工程反复负压实测压力（平均值）
        /// </summary>
        private double _testPress_GCP2_F = 0;
        /// <summary>
        /// 工程反复负压实测压力（平均值）
        /// </summary>
        public double TestPress_GCP2_F
        {
            get { return _testPress_GCP2_F; }
            set
            {
                _testPress_GCP2_F = value;
                RaisePropertyChanged(() => TestPress_GCP2_F);
            }
        }

        /// <summary>
        /// 工程标准值正压实测压力
        /// </summary>
        private double _testPress_GCP3_Z = 0;
        /// <summary>
        /// 工程标准值正压实测压力
        /// </summary>
        public double TestPress_GCP3_Z
        {
            get { return _testPress_GCP3_Z; }
            set
            {
                _testPress_GCP3_Z = value;
                RaisePropertyChanged(() => TestPress_GCP3_Z);
            }
        }

        /// <summary>
        /// 工程标准值负压实测压力
        /// </summary>
        private double _testPress_GCP3_F = 0;
        /// <summary>
        /// 工程标准值负压实测压力
        /// </summary>
        public double TestPress_GCP3_F
        {
            get { return _testPress_GCP3_F; }
            set
            {
                _testPress_GCP3_F = value;
                RaisePropertyChanged(() => TestPress_GCP3_F);
            }
        }

        /// <summary>
        /// 工程设计值正压实测压力
        /// </summary>
        private double _testPress_GCPmax_Z = 0;
        /// <summary>
        /// 工程设计值正压实测压力
        /// </summary>
        public double TestPress_GCPmax_Z
        {
            get { return _testPress_GCPmax_Z; }
            set
            {
                _testPress_GCPmax_Z = value;
                RaisePropertyChanged(() => TestPress_GCPmax_Z);
            }
        }

        /// <summary>
        /// 工程设计值负压实测压力
        /// </summary>
        private double _testPress_GCPmax_F = 0;
        /// <summary>
        /// 工程设计值负压实测压力
        /// </summary>
        public double TestPress_GCPmax_F
        {
            get { return _testPress_GCPmax_F; }
            set
            {
                _testPress_GCPmax_F = value;
                RaisePropertyChanged(() => TestPress_GCPmax_F);
            }
        }

        #endregion

        /// <summary>
        /// 工程p3正压检测相对挠度值是否超限
        /// </summary>
        private bool _isXDND_Over_GCp3_Z = false;
        /// <summary>
        /// 工程p3正压检测相对挠度值是否超限
        /// </summary>
        public bool IsXDND_Over_GCp3_Z
        {
            get { return _isXDND_Over_GCp3_Z; }
            set
            {
                _isXDND_Over_GCp3_Z = value;
                RaisePropertyChanged(() => IsXDND_Over_GCp3_Z);
            }
        }

        /// <summary>
        /// 工程p3负压检测相对挠度值是否超限
        /// </summary>
        private bool _isXDND_Over_GCp3_F = false;
        /// <summary>
        /// 工程p3负压检测相对挠度值是否超限
        /// </summary>
        public bool IsXDND_Over_GCp3_F
        {
            get { return _isXDND_Over_GCp3_F; }
            set
            {
                _isXDND_Over_GCp3_F = value;
                RaisePropertyChanged(() => IsXDND_Over_GCp3_F);
            }
        }

        #region 评定

        /// <summary>
        /// 变形检测是否满足要求
        /// </summary>
        private bool _isMeetDesign_GCp1=true;
        /// <summary>
        ///  变形检测是否满足要求
        /// </summary>
        public bool IsMeetDesign_GCp1
        {
            get { return _isMeetDesign_GCp1; }
            set
            {
                _isMeetDesign_GCp1 = value;
                RaisePropertyChanged(() => IsMeetDesign_GCp1);
            }
        }

        /// <summary>
        /// p2检测是否满足要求
        /// </summary>
        private bool _isMeetDesign_GCp2 = true;
        /// <summary>
        ///  p2检测是否满足要求
        /// </summary>
        public bool IsMeetDesign_GCp2
        {
            get { return _isMeetDesign_GCp2; }
            set
            {
                _isMeetDesign_GCp2 = value;
                RaisePropertyChanged(() => IsMeetDesign_GCp2);
            }
        }

        /// <summary>
        /// p3检测是否满足要求
        /// </summary>
        private bool _isMeetDesign_GCp3 = true;
        /// <summary>
        ///  p3检测是否满足要求
        /// </summary>
        public bool IsMeetDesign_GCp3
        {
            get { return _isMeetDesign_GCp3; }
            set
            {
                _isMeetDesign_GCp3 = value;
                RaisePropertyChanged(() => IsMeetDesign_GCp3);
            }
        }

        /// <summary>
        /// pmax检测是否满足要求
        /// </summary>
        private bool _isMeetDesign_GCpmax=true ;
        /// <summary>
        ///  pmax检测是否满足要求
        /// </summary>
        public bool IsMeetDesign_GCpmax
        {
            get { return _isMeetDesign_GCpmax; }
            set
            {
                _isMeetDesign_GCpmax = value;
                RaisePropertyChanged(() => IsMeetDesign_GCpmax);
            }
        }

        /// <summary>
        /// 总体是否满足要求
        /// </summary>
        private bool _isMeetDesign_GCfinal = true;
        /// <summary>
        /// 总体是否满足要求
        /// </summary>
        public bool IsMeetDesign_GCfinal
        {
            get { return _isMeetDesign_GCfinal; }
            set
            {
                _isMeetDesign_GCfinal = value;
                RaisePropertyChanged(() => IsMeetDesign_GCfinal);
            }
        }

        #endregion

        #endregion
        

        #region 初始化

        /// <summary>
        /// 定级数据初始化
        /// </summary>
        public void InitDJ()
        {
            #region 定级位移

            //定级变形正压位移
            WY_DJBX_Z = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionDJBXWYZ = new ObservableCollection<double>() ;
            for (int i = 0; i < 21; i++)
            {
                tempCollectionDJBXWYZ = new ObservableCollection<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                WY_DJBX_Z.Add(tempCollectionDJBXWYZ);
            }
            //定级变形负压位移
            WY_DJBX_F = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionDJBXWYF = new ObservableCollection<double>();
            for (int i = 0; i < 21; i++)
            {
                tempCollectionDJBXWYF = new ObservableCollection<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                WY_DJBX_F.Add(tempCollectionDJBXWYF);
            }
            //定级P3正压位移
            WY_DJP3_Z = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionDJP3WYZ = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionDJP3WYZ = new ObservableCollection<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                WY_DJP3_Z.Add(tempCollectionDJP3WYZ);
            }
            //定级P3负压位移
            WY_DJP3_F = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionDJP3WYF = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionDJP3WYF = new ObservableCollection<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                WY_DJP3_F.Add(tempCollectionDJP3WYF);
            }
            //定级Pmax正压位移
            WY_DJPmax_Z = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionDJPmaxWYZ = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionDJPmaxWYZ = new ObservableCollection<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                WY_DJPmax_Z.Add(tempCollectionDJPmaxWYZ);
            }
            //定级Pmax负压位移
            WY_DJPmax_F = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionDJPmaxWYF = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionDJPmaxWYF = new ObservableCollection<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                WY_DJPmax_F.Add(tempCollectionDJPmaxWYF);
            }

            #endregion


            #region 定级相对挠度（分母）

            //定级变形正压相对挠度
            XDND_DJBX_Z = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionXDNDDJBXZ = new ObservableCollection<double>();
            for (int i = 0; i < 21; i++)
            {
                tempCollectionXDNDDJBXZ = new ObservableCollection<double>() { 0, 0, 0};
                XDND_DJBX_Z.Add(tempCollectionXDNDDJBXZ);
            }
            //定级变形负压相对挠度
            XDND_DJBX_F = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionXDNDDJBXF = new ObservableCollection<double>();
            for (int i = 0; i < 21; i++)
            {
                tempCollectionXDNDDJBXF = new ObservableCollection<double>() { 0, 0, 0 };
                XDND_DJBX_F.Add(tempCollectionXDNDDJBXF);
            }
            //定级P3正压相对挠度
            XDND_DJP3_Z = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionXDNDDJP3Z = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionXDNDDJP3Z = new ObservableCollection<double>() { 0, 0, 0 };
                XDND_DJP3_Z.Add(tempCollectionXDNDDJP3Z);
            }
            //定级P3负压相对挠度
            XDND_DJP3_F = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionXDNDDJP3F = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionXDNDDJP3F = new ObservableCollection<double>() { 0, 0, 0 };
                XDND_DJP3_F.Add(tempCollectionXDNDDJP3F);
            }
            //定级Pmax正压相对挠度
            XDND_DJPmax_Z = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionXDNDDJPmaxZ = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionXDNDDJPmaxZ = new ObservableCollection<double>() { 0, 0, 0 };
                XDND_DJPmax_Z.Add(tempCollectionXDNDDJPmaxZ);
            }
            //定级Pmax负压相对挠度
            XDND_DJPmax_F = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionXDNDDJPmaxF = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionXDNDDJPmaxF = new ObservableCollection<double>() { 0, 0, 0 };
                XDND_DJPmax_F.Add(tempCollectionXDNDDJPmaxF);
            }

            #endregion


            #region 定级挠度

            //定级变形正压挠度
            ND_DJBX_Z = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionNDDJBXZ = new ObservableCollection<double>();
            for (int i = 0; i < 21; i++)
            {
                tempCollectionNDDJBXZ = new ObservableCollection<double>() { 0, 0, 0 };
                ND_DJBX_Z.Add(tempCollectionNDDJBXZ);
            }
            //定级变形负压挠度
            ND_DJBX_F = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionNDDJBXF = new ObservableCollection<double>();
            for (int i = 0; i < 21; i++)
            {
                tempCollectionNDDJBXF = new ObservableCollection<double>() { 0, 0, 0 };
                ND_DJBX_F.Add(tempCollectionNDDJBXF);
            }
            //定级P3正压挠度
            ND_DJP3_Z = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionNDDJP3Z = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionNDDJP3Z = new ObservableCollection<double>() { 0, 0, 0 };
                ND_DJP3_Z.Add(tempCollectionNDDJP3Z);
            }
            //定级P3负压挠度
            ND_DJP3_F = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionNDDJP3F = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionNDDJP3F = new ObservableCollection<double>() { 0, 0, 0 };
                ND_DJP3_F.Add(tempCollectionNDDJP3F);
            }
            //定级Pmax正压挠度
            ND_DJPmax_Z = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionNDDJPmaxZ = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionNDDJPmaxZ = new ObservableCollection<double>() { 0, 0, 0 };
                ND_DJPmax_Z.Add(tempCollectionNDDJPmaxZ);
            }
            //定级Pmax负压挠度
            ND_DJPmax_F = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionNDDJPmaxF = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionNDDJPmaxF = new ObservableCollection<double>() { 0, 0, 0 };
                ND_DJPmax_F.Add(tempCollectionNDDJPmaxF);
            }

            #endregion
        }

        /// <summary>
        /// 工程数据初始化
        /// </summary>
        public void InitGC()
        {
            #region 工程位移

            //工程变形正压位移
            WY_GCBX_Z = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionGCBXWYZ = new ObservableCollection<double>();
            for (int i = 0; i < 5; i++)
            {
                tempCollectionGCBXWYZ = new ObservableCollection<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                WY_GCBX_Z.Add(tempCollectionGCBXWYZ);
            }

            //工程变形负压位移
            WY_GCBX_F = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionGCBXWYF = new ObservableCollection<double>();
            for (int i = 0; i < 5; i++)
            {
                tempCollectionGCBXWYF = new ObservableCollection<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                WY_GCBX_F.Add(tempCollectionGCBXWYF);
            }

            //工程p3正压位移
            WY_GCP3_Z = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionGCp3WYZ = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionGCp3WYZ = new ObservableCollection<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                WY_GCP3_Z.Add(tempCollectionGCp3WYZ);
            }

            //工程p3负压位移
            WY_GCP3_F = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionGCp3WYF = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionGCp3WYF = new ObservableCollection<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                WY_GCP3_F.Add(tempCollectionGCp3WYF);
            }

            //工程pmax正压位移
            WY_GCPmax_Z = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionGCpmaxWYZ = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionGCpmaxWYZ = new ObservableCollection<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                WY_GCPmax_Z.Add(tempCollectionGCpmaxWYZ);
            }

            //工程pmax正压位移
            WY_GCPmax_F = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionGCpmaxWYF = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionGCpmaxWYF = new ObservableCollection<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                WY_GCPmax_F.Add(tempCollectionGCpmaxWYF);
            }

            #endregion


            #region 工程相对挠度（分母）

            //工程变形正压相对挠度
            XDND_GCBX_Z = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionXDNDGCBXZ = new ObservableCollection<double>();
            for (int i = 0; i < 5; i++)
            {
                tempCollectionXDNDGCBXZ = new ObservableCollection<double>() { 0, 0, 0 };
                XDND_GCBX_Z.Add(tempCollectionXDNDGCBXZ);
            }
            //工程变形负压相对挠度
            XDND_GCBX_F = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionXDNDGCBXF = new ObservableCollection<double>();
            for (int i = 0; i < 5; i++)
            {
                tempCollectionXDNDGCBXF = new ObservableCollection<double>() { 0, 0, 0 };
                XDND_GCBX_F.Add(tempCollectionXDNDGCBXF);
            }
            //工程P3正压相对挠度
            XDND_GCP3_Z = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionXDNDGCP3Z = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionXDNDGCP3Z = new ObservableCollection<double>() { 0, 0, 0 };
                XDND_GCP3_Z.Add(tempCollectionXDNDGCP3Z);
            }
            //工程P3负压相对挠度
            XDND_GCP3_F = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionXDNDGCP3F = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionXDNDGCP3F = new ObservableCollection<double>() { 0, 0, 0 };
                XDND_GCP3_F.Add(tempCollectionXDNDGCP3F);
            }
            //工程Pmax正压相对挠度
            XDND_GCPmax_Z = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionXDNDGCPmaxZ = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionXDNDGCPmaxZ = new ObservableCollection<double>() { 0, 0, 0 };
                XDND_GCPmax_Z.Add(tempCollectionXDNDGCPmaxZ);
            }
            //工程Pmax负压相对挠度
            XDND_GCPmax_F = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionXDNDGCPmaxF = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionXDNDGCPmaxF = new ObservableCollection<double>() { 0, 0, 0 };
                XDND_GCPmax_F.Add(tempCollectionXDNDGCPmaxF);
            }

            #endregion


            #region 工程挠度

            //工程变形正压挠度
            ND_GCBX_Z = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionNDGCBXZ = new ObservableCollection<double>();
            for (int i = 0; i < 5; i++)
            {
                tempCollectionNDGCBXZ = new ObservableCollection<double>() { 0, 0, 0 };
                ND_GCBX_Z.Add(tempCollectionNDGCBXZ);
            }
            //工程变形负压挠度
            ND_GCBX_F = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionNDGCBXF = new ObservableCollection<double>();
            for (int i = 0; i < 5; i++)
            {
                tempCollectionNDGCBXF = new ObservableCollection<double>() { 0, 0, 0 };
                ND_GCBX_F.Add(tempCollectionNDGCBXF);
            }
            //工程P3正压挠度
            ND_GCP3_Z = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionNDGCP3Z = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionNDGCP3Z = new ObservableCollection<double>() { 0, 0, 0 };
                ND_GCP3_Z.Add(tempCollectionNDGCP3Z);
            }
            //工程P3负压挠度
            ND_GCP3_F = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionNDGCP3F = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionNDGCP3F = new ObservableCollection<double>() { 0, 0, 0 };
                ND_GCP3_F.Add(tempCollectionNDGCP3F);
            }
            //工程Pmax正压挠度
            ND_GCPmax_Z = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionNDGCPmaxZ = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionNDGCPmaxZ = new ObservableCollection<double>() { 0, 0, 0 };
                ND_GCPmax_Z.Add(tempCollectionNDGCPmaxZ);
            }
            //工程Pmax负压挠度
            ND_GCPmax_F = new ObservableCollection<ObservableCollection<double>>();
            ObservableCollection<double> tempCollectionNDGCPmaxF = new ObservableCollection<double>();
            for (int i = 0; i < 2; i++)
            {
                tempCollectionNDGCPmaxF = new ObservableCollection<double>() { 0, 0, 0 };
                ND_GCPmax_F.Add(tempCollectionNDGCPmaxF);
            }

            #endregion
        }

        #endregion
    }
}
