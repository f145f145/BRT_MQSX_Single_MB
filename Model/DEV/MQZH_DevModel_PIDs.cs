/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQDFJ_MB.Model.DEV
 * 文件名：  MQZH_DevModel_PIDs
 * 版本号：  V1.0.0.0
 * 唯一标识：d9b9a1a1-69f0-4508-9058-a54eff2836a8
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022-5-26 10:55:31
 * 描述：
 * 装置属性，PID参数部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022-5-26 10:55:31		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using CtrlMethod;
using GalaSoft.MvvmLight;

namespace MQDFJ_MB.Model.DEV
{
    public partial class MQZH_DevModel_Main : ObservableObject
    {
        /// <summary>
        /// PID参数11（气密细管0-75）
        /// </summary>
        private PID_ParamModel _pid11 = new PID_ParamModel() { ControllerName = "气密细管0-75", ControllerNo = "PID11" };
        /// <summary>
        /// PID参数11（气密细管0-75）
        /// </summary>
        public PID_ParamModel PID11
        {
            get { return _pid11; }
            set
            {
                _pid11 = value;
                RaisePropertyChanged(() => PID11);
            }
        }

        /// <summary>
        /// PID参数12（气密细管75-125）
        /// </summary>
        private PID_ParamModel _pid12 = new PID_ParamModel() { ControllerName = "气密细管75-125", ControllerNo = "PID12" };
        /// <summary>
        /// PID参数12（气密细管75-125）
        /// </summary>
        public PID_ParamModel PID12
        {
            get { return _pid12; }
            set
            {
                _pid12 = value;
                RaisePropertyChanged(() => PID12);
            }
        }

        /// <summary>
        /// PID参数13（气密细管125-175）
        /// </summary>
        private PID_ParamModel _pid13 = new PID_ParamModel() { ControllerName = "气密细管125-175", ControllerNo = "PID13" };
        /// <summary>
        /// PID参数13（气密细管125-175）
        /// </summary>
        public PID_ParamModel PID13
        {
            get { return _pid13; }
            set
            {
                _pid13 = value;
                RaisePropertyChanged(() => PID13);
            }
        }

        /// <summary>
        /// PID参数14（气密细管175-600）
        /// </summary>
        private PID_ParamModel _pid14 = new PID_ParamModel() { ControllerName = "气密细管175-600", ControllerNo = "PID14" };
        /// <summary>
        /// PID参数14（气密细管175-600）
        /// </summary>
        public PID_ParamModel PID14
        {
            get { return _pid14; }
            set
            {
                _pid14 = value;
                RaisePropertyChanged(() => PID14);
            }
        }

        /// <summary>
        /// PID参数15（气密细管600~）
        /// </summary>
        private PID_ParamModel _pid15 = new PID_ParamModel() { ControllerName = "气密细管600~", ControllerNo = "PID15" };
        /// <summary>
        /// PID参数15（气密细管600~）
        /// </summary>
        public PID_ParamModel PID15
        {
            get { return _pid15; }
            set
            {
                _pid15 = value;
                RaisePropertyChanged(() => PID15);
            }
        }
        
        /// <summary>
        /// PID参数41（气密中管0-75）
        /// </summary>
        private PID_ParamModel _pid41 = new PID_ParamModel() { ControllerName = "气密中管0-75", ControllerNo = "PID41" };
        /// <summary>
        /// PID参数41（气密中管0-75）
        /// </summary>
        public PID_ParamModel PID41
        {
            get { return _pid41; }
            set
            {
                _pid41 = value;
                RaisePropertyChanged(() => PID41);
            }
        }

        /// <summary>
        /// PID参数42（气密中管75-125）
        /// </summary>
        private PID_ParamModel _pid42 = new PID_ParamModel() { ControllerName = "气密中管75-125", ControllerNo = "PID42" };
        /// <summary>
        /// PID参数42（气密中管75-125）
        /// </summary>
        public PID_ParamModel PID42
        {
            get { return _pid42; }
            set
            {
                _pid42 = value;
                RaisePropertyChanged(() => PID42);
            }
        }

        /// <summary>
        /// PID参数43（气密中管125-175）
        /// </summary>
        private PID_ParamModel _pid43 = new PID_ParamModel() { ControllerName = "气密中管125-175", ControllerNo = "PID43" };
        /// <summary>
        /// PID参数43（气密中管125-175）
        /// </summary>
        public PID_ParamModel PID43
        {
            get { return _pid43; }
            set
            {
                _pid43 = value;
                RaisePropertyChanged(() => PID43);
            }
        }

        /// <summary>
        /// PID参数44（气密中管175-600）
        /// </summary>
        private PID_ParamModel _pid44 = new PID_ParamModel() { ControllerName = "气密中管175-600", ControllerNo = "PID44" };
        /// <summary>
        /// PID参数44（气密中管175-600）
        /// </summary>
        public PID_ParamModel PID44
        {
            get { return _pid44; }
            set
            {
                _pid44 = value;
                RaisePropertyChanged(() => PID44);
            }
        }

        /// <summary>
        /// PID参数45（气密中管600~）
        /// </summary>
        private PID_ParamModel _pid45 = new PID_ParamModel() { ControllerName = "气密中管600~", ControllerNo = "PID45" };
        /// <summary>
        /// PID参数45（气密中管600~）
        /// </summary>
        public PID_ParamModel PID45
        {
            get { return _pid45; }
            set
            {
                _pid45 = value;
                RaisePropertyChanged(() => PID45);
            }
        }

        /// <summary>
        /// PID参数21（水密风压1风机0-250）
        /// </summary>
        private PID_ParamModel _pid21 = new PID_ParamModel() { ControllerName = "水密1风机0-250", ControllerNo = "PID21" };
        /// <summary>
        /// PID参数21（水密风压1风机0-250）
        /// </summary>
        public PID_ParamModel PID21
        {
            get { return _pid21; }
            set
            {
                _pid21 = value;
                RaisePropertyChanged(() => PID21);
            }
        }

        /// <summary>
        /// PID参数22（水密风压1风机250-500）
        /// </summary>
        private PID_ParamModel _pid22 = new PID_ParamModel() { ControllerName = "水密1风机250-500", ControllerNo = "PID22" };
        /// <summary>
        /// PID参数22（水密风压1风机250-500）
        /// </summary>
        public PID_ParamModel PID22
        {
            get { return _pid22; }
            set
            {
                _pid22 = value;
                RaisePropertyChanged(() => PID22);
            }
        }

        /// <summary>
        /// PID参数23（水密风压1风机500-1000）
        /// </summary>
        private PID_ParamModel _pid23 = new PID_ParamModel() { ControllerName = "水密1风机500-1000", ControllerNo = "PID23" };
        /// <summary>
        /// PID参数23（水密风压1风机500-1000）
        /// </summary>
        public PID_ParamModel PID23
        {
            get { return _pid23; }
            set
            {
                _pid23 = value;
                RaisePropertyChanged(() => PID23);
            }
        }

        /// <summary>
        /// PID参数24（水密风压1风机1000-3000）
        /// </summary>
        private PID_ParamModel _pid24 = new PID_ParamModel() { ControllerName = "水密1风机1000-3000", ControllerNo = "PID24" };
        /// <summary>
        /// PID参数24（水密风压1风机1000-3000）
        /// </summary>
        public PID_ParamModel PID24
        {
            get { return _pid24; }
            set
            {
                _pid24 = value;
                RaisePropertyChanged(() => PID24);
            }
        }

        /// <summary>
        /// PID参数25（水密风压1风机3000-5000）
        /// </summary>
        private PID_ParamModel _pid25 = new PID_ParamModel() { ControllerName = "水密1风机3000-5000", ControllerNo = "PID25" };
        /// <summary>
        /// PID参数25（水密风压1风机3000-5000）
        /// </summary>
        public PID_ParamModel PID25
        {
            get { return _pid25; }
            set
            {
                _pid25 = value;
                RaisePropertyChanged(() => PID25);
            }
        }

        /// <summary>
        /// PID参数26（水密风压1风机5000-12000）
        /// </summary>
        private PID_ParamModel _pid26 = new PID_ParamModel() { ControllerName = "水密1风机5000-12000", ControllerNo = "PID26" };
        /// <summary>
        /// PID参数26（水密风压1风机5000-12000）
        /// </summary>
        public PID_ParamModel PID26
        {
            get { return _pid26; }
            set
            {
                _pid26 = value;
                RaisePropertyChanged(() => PID26);
            }
        }

        /// <summary>
        /// PID参数51（水流量PID）
        /// </summary>
        private PID_ParamModel _pid51 = new PID_ParamModel() { ControllerName = "水流量PID", ControllerNo = "PID51" };
        /// <summary>
        /// PID参数51（水流量PID）
        /// </summary>
        public PID_ParamModel PID51
        {
            get { return _pid51; }
            set
            {
                _pid51 = value;
                RaisePropertyChanged(() => PID51);
            }
        }


        /// <summary>
        /// PID参数61（气密粗管0-75）
        /// </summary>
        private PID_ParamModel _pid61 = new PID_ParamModel() { ControllerName = "气密粗管0-75", ControllerNo = "PID61" };
        /// <summary>
        /// PID参数61（气密粗管0-75）
        /// </summary>
        public PID_ParamModel PID61
        {
            get { return _pid61; }
            set
            {
                _pid61 = value;
                RaisePropertyChanged(() => PID61);
            }
        }

        /// <summary>
        /// PID参数62（气密粗管75-125）
        /// </summary>
        private PID_ParamModel _pid62 = new PID_ParamModel() { ControllerName = "气密粗管75-125", ControllerNo = "PID62" };
        /// <summary>
        /// PID参数62（气密粗管75-125）
        /// </summary>
        public PID_ParamModel PID62
        {
            get { return _pid62; }
            set
            {
                _pid62 = value;
                RaisePropertyChanged(() => PID62);
            }
        }

        /// <summary>
        /// PID参数63（气密粗管125-175）
        /// </summary>
        private PID_ParamModel _pid63 = new PID_ParamModel() { ControllerName = "气密粗管125-175", ControllerNo = "PID63" };
        /// <summary>
        /// PID参数63（气密粗管125-175）
        /// </summary>
        public PID_ParamModel PID63
        {
            get { return _pid63; }
            set
            {
                _pid63 = value;
                RaisePropertyChanged(() => PID63);
            }
        }

        /// <summary>
        /// PID参数64（气密粗管175-600）
        /// </summary>
        private PID_ParamModel _pid64 = new PID_ParamModel() { ControllerName = "气密粗管175-600", ControllerNo = "PID64" };
        /// <summary>
        /// PID参数64（气密粗管175-600）
        /// </summary>
        public PID_ParamModel PID64
        {
            get { return _pid64; }
            set
            {
                _pid64 = value;
                RaisePropertyChanged(() => PID64);
            }
        }

        /// <summary>
        /// PID参数65（气密粗管600~）
        /// </summary>
        private PID_ParamModel _pid65 = new PID_ParamModel() { ControllerName = "气密粗管600~", ControllerNo = "PID65" };
        /// <summary>
        /// PID参数65（气密粗管600~）
        /// </summary>
        public PID_ParamModel PID65
        {
            get { return _pid65; }
            set
            {
                _pid65 = value;
                RaisePropertyChanged(() => PID65);
            }
        }
    }
}