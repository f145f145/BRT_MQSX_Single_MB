/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQZHWL.Model.ExpData
 * 文件名：  MQZH_ExpDataModel_CJBX
 * 版本号：  V1.0.0.0
 * 唯一标识：7788dc17-a0c8-4976-a014-e53bb6e87627
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/12/25 7:21:27
 * 描述：
 * 层间变形试验数据
 * ==================================================================================
 * 修改标记
 * 修改时间			        修改人			版本号			描述
 * 2021/12/25 7:21:27		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System.Collections;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace MQZHWL.Model.ExpData
{
    public class MQZH_ExpDataModel_CJBX : ObservableObject
    {
        #region 定级检测数据

        /// <summary>
        /// X轴定级检测层间位移角（倒数，分母）
        /// </summary>
        private ObservableCollection<double> _angleFM_DJ_X = new ObservableCollection<double> { 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// X轴定级检测层间位移角（倒数，分母）
        /// </summary>
        public ObservableCollection<double> AngleFM_DJ_X
        {
            get { return _angleFM_DJ_X; }
            set
            {
                _angleFM_DJ_X = value;
                RaisePropertyChanged(() => AngleFM_DJ_X);
            }
        }

        /// <summary>
        /// X轴定级检测层间位移量
        /// </summary>
        private ObservableCollection<double> _dspl_DJ_X = new ObservableCollection<double> { 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// X轴定级检测层间位移量
        /// </summary>
        public ObservableCollection<double> DSPL_DJ_X
        {
            get { return _dspl_DJ_X; }
            set
            {
                _dspl_DJ_X = value;
                RaisePropertyChanged(() => DSPL_DJ_X);
            }
        }

        /// <summary>
        /// X轴定级检测是否损坏
        /// </summary>
        private ObservableCollection<bool> _isDamage_DJ_X = new ObservableCollection<bool>(){false, false, false, false, false, false };
        /// <summary>
        /// X轴定级检测是否损坏
        /// </summary>
        public ObservableCollection<bool> IsDamage_DJ_X
        {
            get { return _isDamage_DJ_X; }
            set
            {
                _isDamage_DJ_X = value;
                RaisePropertyChanged(() => IsDamage_DJ_X);
            }
        }

        /// <summary>
        /// X轴定级检测损坏说明
        /// </summary>
        private ObservableCollection<string> _damagePS_DJ_X = new ObservableCollection<string> { "", "", "", "", "", "" };
        /// <summary>
        /// X轴定级检测损坏说明
        /// </summary>
        public ObservableCollection<string> DamagePS_DJ_X
        {
            get { return _damagePS_DJ_X; }
            set
            {
                _damagePS_DJ_X = value;
                RaisePropertyChanged(() => DamagePS_DJ_X);
            }
        }

        /// <summary>
        /// Y轴定级检测层间位移角（倒数，分母）
        /// </summary>
        private ObservableCollection<double> _angleFM_DJ_Y = new ObservableCollection<double> { 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// Y轴定级检测层间位移角（倒数，分母）
        /// </summary>
        public ObservableCollection<double> AngleFM_DJ_Y
        {
            get { return _angleFM_DJ_Y; }
            set
            {
                _angleFM_DJ_Y = value;
                RaisePropertyChanged(() => AngleFM_DJ_Y);
            }
        }

        /// <summary>
        /// Y轴定级检测层间位移量
        /// </summary>
        private ObservableCollection<double> _dspl_DJ_Y = new ObservableCollection<double> { 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// Y轴定级检测层间位移量
        /// </summary>
        public ObservableCollection<double> DSPL_DJ_Y
        {
            get { return _dspl_DJ_Y; }
            set
            {
                _dspl_DJ_Y = value;
                RaisePropertyChanged(() => DSPL_DJ_Y);
            }
        }

        /// <summary>
        /// Y轴定级检测是否损坏
        /// </summary>
        private ObservableCollection<bool> _isDamage_DJ_Y = new ObservableCollection<bool>()
            {false,false,false,false, false, false};
        /// <summary>
        /// Y轴定级检测是否损坏
        /// </summary>
        public ObservableCollection<bool> IsDamage_DJ_Y
        {
            get { return _isDamage_DJ_Y; }
            set
            {
                _isDamage_DJ_Y = value;
                RaisePropertyChanged(() => IsDamage_DJ_Y);
            }
        }

        /// <summary>
        /// Y轴定级检测损坏说明
        /// </summary>
        private ObservableCollection<string> _damagePS_DJ_Y = new ObservableCollection<string> { "", "", "", "", "", "" };
        /// <summary>
        /// Y轴定级检测损坏说明
        /// </summary>
        public ObservableCollection<string> DamagePS_DJ_Y
        {
            get { return _damagePS_DJ_Y; }
            set
            {
                _damagePS_DJ_Y = value;
                RaisePropertyChanged(() => DamagePS_DJ_Y);
            }
        }

        /// <summary>
        /// Z轴定级检测层间位移量
        /// </summary>
        private ObservableCollection<double> _dspl_DJ_Z = new ObservableCollection<double> { 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// Z轴定级检测层间位移量
        /// </summary>
        public ObservableCollection<double> DSPL_DJ_Z
        {
            get { return _dspl_DJ_Z; }
            set
            {
                _dspl_DJ_Z = value;
                RaisePropertyChanged(() => DSPL_DJ_Z);
            }
        }

        /// <summary>
        /// Z轴定级检测是否损坏
        /// </summary>
        private ObservableCollection<bool> _isDamage_DJ_Z = new ObservableCollection<bool>(){false, false, false, false, false, false};
        /// <summary>
        /// Z轴定级检测是否损坏
        /// </summary>
        public ObservableCollection<bool> IsDamage_DJ_Z
        {
            get { return _isDamage_DJ_Z; }
            set
            {
                _isDamage_DJ_Z = value;
                RaisePropertyChanged(() => IsDamage_DJ_Z);
            }
        }

        /// <summary>
        /// Z轴定级检测损坏说明
        /// </summary>
        private ObservableCollection<string> _damagePS_DJ_Z = new ObservableCollection<string> { "", "", "", "", "", "" };
        /// <summary>
        /// Z轴定级检测损坏说明
        /// </summary>
        public ObservableCollection<string> DamagePS_DJ_Z
        {
            get { return _damagePS_DJ_Z; }
            set
            {
                _damagePS_DJ_Z = value;
                RaisePropertyChanged(() => DamagePS_DJ_Z);
            }
        }

        #endregion


        #region 定级检测分析、评定数据

        /// <summary>
        /// 定级检测X轴最大位移角（分母，实际最小值）
        /// </summary>
        private double _maxAngleFM_DJ_X = 0;
        /// <summary>
        /// 定级检测X轴最大位移角（分母，实际最小值）
        /// </summary>
        public double MaxAngleFM_DJ_X
        {
            get { return _maxAngleFM_DJ_X; }
            set
            {
                _maxAngleFM_DJ_X = value;
                RaisePropertyChanged(() => MaxAngleFM_DJ_X);
            }
        }

        /// <summary>
        /// 定级检测X轴最大位移量
        /// </summary>
        private double _maxdspl_DJ_X = 0;
        /// <summary>
        /// 定级检测X轴最大位移量
        /// </summary>
        public double MaxDSPL_DJ_X
        {
            get { return _maxdspl_DJ_X; }
            set
            {
                _maxdspl_DJ_X = value;
                RaisePropertyChanged(() => MaxDSPL_DJ_X);
            }
        }

        /// <summary>
        /// 定级检测Y轴最大位移角（分母，实际最小值）
        /// </summary>
        private double _maxAngleFM_DJ_Y = 0;
        /// <summary>
        /// 定级检测Y轴最大位移角（分母，实际最小值）
        /// </summary>
        public double MaxAngleFM_DJ_Y
        {
            get { return _maxAngleFM_DJ_Y; }
            set
            {
                _maxAngleFM_DJ_Y = value;
                RaisePropertyChanged(() => MaxAngleFM_DJ_Y);
            }
        }

        /// <summary>
        /// 定级检测Y轴最大位移量
        /// </summary>
        private double _maxdspl_DJ_Y = 0;
        /// <summary>
        /// 定级检测Y轴最大位移量
        /// </summary>
        public double MaxDSPL_DJ_Y
        {
            get { return _maxdspl_DJ_Y; }
            set
            {
                _maxdspl_DJ_Y = value;
                RaisePropertyChanged(() => MaxDSPL_DJ_Y);
            }
        }

        /// <summary>
        /// 定级检测Z轴最大位移量
        /// </summary>
        private double _maxdspl_DJ_Z = 0;
        /// <summary>
        /// 定级检测Z轴最大位移量
        /// </summary>
        public double MaxDSPL_DJ_Z
        {
            get { return _maxdspl_DJ_Z; }
            set
            {
                _maxdspl_DJ_Z = value;
                RaisePropertyChanged(() => MaxDSPL_DJ_Z);
            }
        }

        /// <summary>
        /// 定级检测X轴评定等级
        /// </summary>
        private int _level_DJ_X = 0;
        /// <summary>
        /// 定级检测X轴评定等级
        /// </summary>
        public int Level_DJ_X
        {
            get { return _level_DJ_X; }
            set
            {
                _level_DJ_X = value;
                RaisePropertyChanged(() => Level_DJ_X);
            }
        }

        /// <summary>
        /// 定级检测Y轴评定等级
        /// </summary>
        private int _level_DJ_Y = 0;
        /// <summary>
        /// 定级检测Y轴评定等级
        /// </summary>
        public int Level_DJ_Y
        {
            get { return _level_DJ_Y; }
            set
            {
                _level_DJ_Y = value;
                RaisePropertyChanged(() => Level_DJ_Y);
            }
        }

        /// <summary>
        /// 定级检测Z轴评定等级
        /// </summary>
        private int _level_DJ_Z = 0;
        /// <summary>
        /// 定级检测Z轴评定等级
        /// </summary>
        public int Level_DJ_Z
        {
            get { return _level_DJ_Z; }
            set
            {
                _level_DJ_Z = value;
                RaisePropertyChanged(() => Level_DJ_Z);
            }
        }

        /// <summary>
        /// X轴定级检测最终是否损坏
        /// </summary>
        private bool _isDamageFinal_DJ_X = false;
        /// <summary>
        /// X轴定级检测最终是否损坏
        /// </summary>
        public bool IsDamageFinal_DJ_X
        {
            get { return _isDamageFinal_DJ_X; }
            set
            {
                _isDamageFinal_DJ_X = value;
                RaisePropertyChanged(() => IsDamageFinal_DJ_X);
            }
        }

        /// <summary>
        /// Y轴定级检测最终是否损坏
        /// </summary>
        private bool _isDamageFinal_DJ_Y = false;
        /// <summary>
        /// Y轴定级检测最终是否损坏
        /// </summary>
        public bool IsDamageFinal_DJ_Y
        {
            get { return _isDamageFinal_DJ_Y; }
            set
            {
                _isDamageFinal_DJ_Y = value;
                RaisePropertyChanged(() => IsDamageFinal_DJ_Y);
            }
        }

        /// <summary>
        /// Z轴定级检测最终是否损坏
        /// </summary>
        private bool _isDamageFinal_DJ_Z = false;
        /// <summary>
        /// Z轴定级检测最终是否损坏
        /// </summary>
        public bool IsDamageFinal_DJ_Z
        {
            get { return _isDamageFinal_DJ_Z; }
            set
            {
                _isDamageFinal_DJ_Z = value;
                RaisePropertyChanged(() => IsDamageFinal_DJ_Z);
            }
        }

        /// <summary>
        /// X轴定级检测最终损坏说明
        /// </summary>
        private string _damageFinalPS_DJ_X = "";
        /// <summary>
        /// X轴定级检测最终损坏说明
        /// </summary>
        public string DamageFinalPS_DJ_X
        {
            get { return _damageFinalPS_DJ_X; }
            set
            {
                _damageFinalPS_DJ_X = value;
                RaisePropertyChanged(() => DamageFinalPS_DJ_X);
            }
        }

        /// <summary>
        /// Y轴定级检测最终损坏说明
        /// </summary>
        private string _damageFinalPS_DJ_Y = "";
        /// <summary>
        /// Y轴定级检测最终损坏说明
        /// </summary>
        public string DamageFinalPS_DJ_Y
        {
            get { return _damageFinalPS_DJ_Y; }
            set
            {
                _damageFinalPS_DJ_Y = value;
                RaisePropertyChanged(() => DamageFinalPS_DJ_Y);
            }
        }

        /// <summary>
        /// Z轴定级检测最终损坏说明
        /// </summary>
        private string _damageFinalPS_DJ_Z = "";
        /// <summary>
        /// Z轴定级检测最终损坏说明
        /// </summary>
        public string DamageFinalPS_DJ_Z
        {
            get { return _damageFinalPS_DJ_Z; }
            set
            {
                _damageFinalPS_DJ_Z = value;
                RaisePropertyChanged(() => DamageFinalPS_DJ_Z);
            }
        }

        #endregion
        

        #region 工程检测数据

        /// <summary>
        /// X轴工程检测层间位移角（倒数，分母）
        /// </summary>
        private double _angleFM_GC_X = 0;
        /// <summary>
        /// X轴工程检测层间位移角（倒数，分母）
        /// </summary>
        public double AngleFM_GC_X
        {
            get { return _angleFM_GC_X; }
            set
            {
                _angleFM_GC_X = value;
                RaisePropertyChanged(() => AngleFM_GC_X);
            }
        }

        /// <summary>
        /// X轴工程检测层间位移量
        /// </summary>
        private double _dspl_GC_X = 0;
        /// <summary>
        /// X轴工程检测层间位移量
        /// </summary>
        public double DSPL_GC_X
        {
            get { return _dspl_GC_X; }
            set
            {
                _dspl_GC_X = value;
                RaisePropertyChanged(() => DSPL_GC_X);
            }
        }

        /// <summary>
        /// X轴工程检测是否损坏
        /// </summary>
        private bool _isDamage_GC_X = false;
        /// <summary>
        /// X轴工程检测是否损坏
        /// </summary>
        public bool IsDamage_GC_X
        {
            get { return _isDamage_GC_X; }
            set
            {
                _isDamage_GC_X = value;
                RaisePropertyChanged(() => IsDamage_GC_X);
            }
        }

        /// <summary>
        /// X轴工程检测损坏说明
        /// </summary>
        private string _damagePS_GC_X = "";
        /// <summary>
        /// X轴工程检测损坏说明
        /// </summary>
        public string DamagePS_GC_X
        {
            get { return _damagePS_GC_X; }
            set
            {
                _damagePS_GC_X = value;
                RaisePropertyChanged(() => DamagePS_GC_X);
            }
        }

        /// <summary>
        /// Y轴工程检测层间位移角（倒数，分母）
        /// </summary>
        private double _angleFM_GC_Y = 0;
        /// <summary>
        /// Y轴工程检测层间位移角（倒数，分母）
        /// </summary>
        public double AngleFM_GC_Y
        {
            get { return _angleFM_GC_Y; }
            set
            {
                _angleFM_GC_Y = value;
                RaisePropertyChanged(() => AngleFM_GC_Y);
            }
        }

        /// <summary>
        /// Y轴工程检测层间位移量
        /// </summary>
        private double _dspl_GC_Y = 0;
        /// <summary>
        /// Y轴工程检测层间位移量
        /// </summary>
        public double DSPL_GC_Y
        {
            get { return _dspl_GC_Y; }
            set
            {
                _dspl_GC_Y = value;
                RaisePropertyChanged(() => DSPL_GC_Y);
            }
        }

        /// <summary>
        /// Y轴工程检测是否损坏
        /// </summary>
        private bool _isDamage_GC_Y = false;
        /// <summary>
        /// Y轴工程检测是否损坏
        /// </summary>
        public bool IsDamage_GC_Y
        {
            get { return _isDamage_GC_Y; }
            set
            {
                _isDamage_GC_Y = value;
                RaisePropertyChanged(() => IsDamage_GC_Y);
            }
        }

        /// <summary>
        /// Y轴工程检测损坏说明
        /// </summary>
        private string _damagePS_GC_Y = "";
        /// <summary>
        /// Y轴工程检测损坏说明
        /// </summary>
        public string DamagePS_GC_Y
        {
            get { return _damagePS_GC_Y; }
            set
            {
                _damagePS_GC_Y = value;
                RaisePropertyChanged(() => DamagePS_GC_Y);
            }
        }

        /// <summary>
        /// Z轴工程检测层间位移量
        /// </summary>
        private double _dspl_GC_Z = 0;
        /// <summary>
        /// Z轴工程检测层间位移量
        /// </summary>
        public double DSPL_GC_Z
        {
            get { return _dspl_GC_Z; }
            set
            {
                _dspl_GC_Z = value;
                RaisePropertyChanged(() => DSPL_GC_Z);
            }
        }

        /// <summary>
        /// Z轴工程检测是否损坏
        /// </summary>
        private bool _isDamage_GC_Z = false;
        /// <summary>
        /// Z轴工程检测是否损坏
        /// </summary>
        public bool IsDamage_GC_Z
        {
            get { return _isDamage_GC_Z; }
            set
            {
                _isDamage_GC_Z = value;
                RaisePropertyChanged(() => IsDamage_GC_Z);
            }
        }

        /// <summary>
        /// Z轴工程检测损坏说明
        /// </summary>
        private string _damagePS_GC_Z = "";
        /// <summary>
        /// Z轴工程检测损坏说明
        /// </summary>
        public string DamagePS_GC_Z
        {
            get { return _damagePS_GC_Z; }
            set
            {
                _damagePS_GC_Z = value;
                RaisePropertyChanged(() => DamagePS_GC_Z);
            }
        }

        #endregion


        #region 工程检测分析、评定数据
        
        /// <summary>
        /// 工程检测总体是否满足工程设计要求
        /// </summary>
        private bool _isMeetDesign_GC_All = false;
        /// <summary>
        /// 工程检测总体是否满足工程设计要求
        /// </summary>
        public bool IsMeetDesign_GC_All
        {
            get { return _isMeetDesign_GC_All; }
            set
            {
                _isMeetDesign_GC_All = value;
                RaisePropertyChanged(() => IsMeetDesign_GC_All);
            }
        }

        /// <summary>
        /// 工程检测X轴是否满足工程设计要求
        /// </summary>
        private bool _isMeetDesign_GC_X = false;
        /// <summary>
        /// 工程检测X轴是否满足工程设计要求
        /// </summary>
        public bool IsMeetDesign_GC_X
        {
            get { return _isMeetDesign_GC_X; }
            set
            {
                _isMeetDesign_GC_X = value;
                RaisePropertyChanged(() => IsMeetDesign_GC_X);
            }
        }

        /// <summary>
        /// 工程检测Z轴是否满足工程设计要求
        /// </summary>
        private bool _isMeetDesign_GC_Z = false;
        /// <summary>
        /// 工程检测Z轴是否满足工程设计要求
        /// </summary>
        public bool IsMeetDesign_GC_Z
        {
            get { return _isMeetDesign_GC_Z; }
            set
            {
                _isMeetDesign_GC_Z = value;
                RaisePropertyChanged(() => IsMeetDesign_GC_Z);
            }
        }

        /// <summary>
        /// 工程检测Y轴是否满足工程设计要求
        /// </summary>
        private bool _isMeetDesign_GC_Y = false;
        /// <summary>
        /// 工程检测Y轴是否满足工程设计要求
        /// </summary>
        public bool IsMeetDesign_GC_Y
        {
            get { return _isMeetDesign_GC_Y; }
            set
            {
                _isMeetDesign_GC_Y = value;
                RaisePropertyChanged(() => IsMeetDesign_GC_Y);
            }
        }

        #endregion


        #region 定级分类初始化

        /// <summary>
        /// 层间变形定级X轴检测数据初始化
        /// </summary>
        public void Init_DJ_X (int stageNO)
        {
            AngleFM_DJ_X[stageNO] = 0;
            DSPL_DJ_X[stageNO] = 0;
            IsDamage_DJ_X[stageNO] = false;
            DamagePS_DJ_X[stageNO] = "";

            MaxAngleFM_DJ_X = 0;
            MaxDSPL_DJ_X = 0;
            Level_DJ_X = 0;
            IsDamageFinal_DJ_X = false;
            DamageFinalPS_DJ_X = "";
        }

        /// <summary>
        /// 层间变形定级X轴检测数据初始化
        /// </summary>
        public void Init_DJ_Y(int stageNO)
        {
            AngleFM_DJ_Y[stageNO] = 0;
            DSPL_DJ_Y[stageNO] = 0;
            IsDamage_DJ_Y[stageNO] = false;
            DamagePS_DJ_Y[stageNO] = "";

            MaxAngleFM_DJ_Y = 0;
            MaxDSPL_DJ_Y = 0;
            Level_DJ_Y = 0;
            IsDamageFinal_DJ_Y = false;
            DamageFinalPS_DJ_Y = "";
        }

        /// <summary>
        /// 层间变形定级X轴检测数据初始化
        /// </summary>
        public void Init_DJ_Z(int stageNO)
        {
            DSPL_DJ_Z[stageNO] = 0;
            IsDamage_DJ_Z[stageNO] = false;
            DamagePS_DJ_Z[stageNO] = "";

            MaxDSPL_DJ_Z = 0;
            Level_DJ_Z = 0;
            DamageFinalPS_DJ_Z = "";
            IsDamageFinal_DJ_Z = false;
        }

        /// <summary>
        /// 层间变形工程X轴检测数据初始化
        /// </summary>
        public void Init_GC_X()
        {
            AngleFM_GC_X = 0;
            DSPL_GC_X = 0;
            IsDamage_GC_X = false;
            DamagePS_GC_X = "";

            IsMeetDesign_GC_All = false;
        }

        /// <summary>
        /// 层间变形工程X轴检测数据初始化
        /// </summary>
        public void Init_GC_Y()
        {
            AngleFM_GC_Y = 0;
            DSPL_GC_Y = 0;
            IsDamage_GC_Y = false;
            DamagePS_GC_Y = "";

            IsMeetDesign_GC_All = false;
        }

        /// <summary>
        /// 层间变形工程X轴检测数据初始化
        /// </summary>
        public void Init_GC_Z()
        {
            DSPL_GC_Z = 0;
            IsDamage_GC_Z = false;
            DamagePS_GC_Z = "";

            IsMeetDesign_GC_All = false;
        }

        #endregion

    }
}
