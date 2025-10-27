/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQDFJ_MB.Model.DEV
 * 文件名：  MQZHDevModel_CoInfo
 * 版本号：  V1.0.0.0
 * 唯一标识：a980c92b-e8e1-479a-8d92-560d2b9211ea
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022-5-26 11:08:56
 * 描述：
 * 装置属性，公司信息部分
 * ==================================================================================
 * 修改标记
 * 修改时间				修改人			版本号			描述
 * 2022-5-26 11:08:56		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;

namespace MQDFJ_MB.Model.DEV
{
    public partial class MQZH_DevModel_Main : ObservableObject
    {
        /// <summary>
        /// 公司简称
        /// </summary>
        private string _mqzh_CompanyShortName = "//";
        /// <summary>
        /// 公司简称
        /// </summary>
        public string MQZH_CompanyShortName
        {
            get { return _mqzh_CompanyShortName; }
            set
            {
                _mqzh_CompanyShortName = value;
                RaisePropertyChanged(() => MQZH_CompanyShortName);
            }
        }

        /// <summary>
        /// 公司全称
        /// </summary>
        private string _mqzh_CompanyName = "//";
        /// <summary>
        /// 公司全称
        /// </summary>
        public string MQZH_CompanyName
        {
            get { return _mqzh_CompanyName; }
            set
            {
                _mqzh_CompanyName = value;
                RaisePropertyChanged(() => MQZH_CompanyName);
            }
        }

        /// <summary>
        /// 公司地址
        /// </summary>
        private string _mqzh_CompanyAddr = "//";
        /// <summary>
        /// 公司地址
        /// </summary>
        public string MQZH_CompanyAddr
        {
            get { return _mqzh_CompanyAddr; }
            set
            {
                _mqzh_CompanyAddr = value;
                RaisePropertyChanged(() => MQZH_CompanyAddr);
            }
        }

        /// <summary>
        /// 实验室地址
        /// </summary>
        private string _mqzh_LabAddr = "//";
        /// <summary>
        /// 实验室地址
        /// </summary>
        public string MQZH_LabAddr
        {
            get { return _mqzh_LabAddr; }
            set
            {
                _mqzh_LabAddr = value;
                RaisePropertyChanged(() => MQZH_LabAddr);
            }
        }

        /// <summary>
        /// 公司电话
        /// </summary>
        private string _mqzh_CompanyTel = "//";
        /// <summary>
        /// 公司电话
        /// </summary>
        public string MQZH_CompanyTel
        {
            get { return _mqzh_CompanyTel; }
            set
            {
                _mqzh_CompanyTel = value;
                RaisePropertyChanged(() => MQZH_CompanyTel);
            }
        }

        /// <summary>
        /// 实验室电话
        /// </summary>
        private string _mqzh_LabTel = "//";
        /// <summary>
        /// 实验室电话
        /// </summary>
        public string MQZH_LabTel
        {
            get { return _mqzh_LabTel; }
            set
            {
                _mqzh_LabTel = value;
                RaisePropertyChanged(() => MQZH_LabTel);
            }
        }

        /// <summary>
        /// 公司邮政编码
        /// </summary>
        private string _mqzh_ComPostNO = "//";
        /// <summary>
        /// 公司邮政编码
        /// </summary>
        public string MQZH_ComPostNO
        {
            get { return _mqzh_ComPostNO; }
            set
            {
                _mqzh_ComPostNO = value;
                RaisePropertyChanged(() => MQZH_ComPostNO);
            }
        }

        /// <summary>
        /// 实验室邮政编码
        /// </summary>
        private string _mqzh_LabPostNO = "//";
        /// <summary>
        /// 实验室邮政编码
        /// </summary>
        public string MQZH_LabPostNO
        {
            get { return _mqzh_LabPostNO; }
            set
            {
                _mqzh_LabPostNO = value;
                RaisePropertyChanged(() => MQZH_LabPostNO);
            }
        }
    }
}
