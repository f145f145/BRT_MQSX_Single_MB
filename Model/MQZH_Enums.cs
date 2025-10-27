/************************************************************************************
 * 描述：
 *
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2021/11/20       16:01:28		郝正强			V1.0.0.0
 *
 ************************************************************************************/

namespace MQZHWL.Model
{
    public static class MQZH_Enums
    {
        /// <summary>
        /// 装置运行模式类型
        /// </summary>
        public enum DevicRunModeType
        {
            /// <summary>
            /// 00待机模式
            /// </summary>
            Wait_Mode = 0,

            /// <summary>
            /// 紧急停机
            /// </summary>
            JJTJ_Mode = 119,

            /// <summary>
            /// 01单点调试模式
            /// </summary>
            SDDDDbg_Mode = 01,

            /// <summary>
            /// 02手动正压调速模式(手动给频率值）
            /// </summary>
            SDZTSDbg_Mode = 02,

            /// <summary>
            /// 03手动负压调速模式(手动给频率值）
            /// </summary>
            SDFTSDbg_Mode = 03,

            /// <summary>
            /// 04手动水泵调速模式(手动给频率值）
            /// </summary>
            SDSBTSDbg_Mode = 04,
            
            /// <summary>
            /// 05蝶阀1正压气密调试模式
            /// </summary>
            DF1QMZDbg_Mode = 05,

            /// <summary>
            /// 06蝶阀1负压气密调试模式
            /// </summary>
            DF1QMFDbg_Mode = 06,

            /// <summary>
            /// 07蝶阀2正压气密调试模式
            /// </summary>
            DF2QMZDbg_Mode = 07,

            /// <summary>
            /// 08蝶阀2负压气密调试模式
            /// </summary>
            DF2QMFDbg_Mode = 08,

            /// <summary>
            /// 09蝶阀3正压气密调试模式
            /// </summary>
            DF3QMZDbg_Mode = 09,

            /// <summary>
            /// 10蝶阀3负压气密调试模式
            /// </summary>
            DF3QMFDbg_Mode = 10,

            /// <summary>
            /// 11蝶阀4正压气密调试模式
            /// </summary>
            DF4QMZDbg_Mode = 11,

            /// <summary>
            /// 12蝶阀4负压气密调试模式
            /// </summary>
            DF4QMFDbg_Mode = 12,

            /// <summary>
            /// 13大风压PID正压调试模式
            /// </summary>
            SDSMZDbg_Mode = 13,

            /// <summary>
            /// 14大风压PID负压调试模式
            /// </summary>
            SDSMFDbg_Mode = 14,

            /// <summary>
            /// 15水流量PID模式
            /// </summary>
            SLLPIDDbg_Mode = 15,

            /// <summary>
            /// 16位移调试模式
            /// </summary>
            SDWYDbg_Mode = 16,

            /// <summary>
            /// 气密检测模式
            /// </summary>
            QM_Mode = 20,

            /// <summary>
            /// 水密检测模式
            /// </summary>
            SM_Mode = 30,

            /// <summary>
            /// 抗风压P1检测模式
            /// </summary>
            KFYp1_Mode = 41,

            /// <summary>
            /// 抗风压P2检测模式
            /// </summary>
            KFYp2_Mode = 42,

            /// <summary>
            /// 抗风压P3检测模式
            /// </summary>
            KFYp3_Mode = 43,

            /// <summary>
            /// 抗风压Pmax检测模式
            /// </summary>
            KFYpmax_Mode = 44,

            /// <summary>
            /// 层间变形检测模式
            /// </summary>
            CJBX_Mode = 50,

        }

        #region 试验设定相关
        
        /// <summary>
        /// 检测项目阶段类型
        /// </summary>
        public enum TestStageType
        {
            //气密
            QMD = 100,                                  //气密性能检测，待机阶段
            //各气密定级测试阶段
            QM_DJ_ZqfY = 101,                           //气密定级正压附加渗透量预加压
            QM_DJ_ZqfJ = 102,                           //气密定级正压附加渗透量测试
            QM_DJ_FqfY = 103,                           //气密定级负压附加渗透量预加压
            QM_DJ_FqfJ = 104,                            //气密定级负压附加渗透量测试
            QM_DJ_ZqfgY = 105,                          //气密定级正压附加固定渗透量预加压
            QM_DJ_ZqfgJ = 106,                          //气密定级正压附加固定渗透量测试
            QM_DJ_FqfgY = 107,                          //气密定级负压附加固定渗透量预加压
            QM_DJ_FqfgJ = 108,                          //气密定级负压附加固定渗透量测试
            QM_DJ_ZqzY = 109,                           //气密定级正压总渗透量预加压
            QM_DJ_ZqzJ = 110,                           //气密定级正压总渗透量测试
            QM_DJ_FqzY = 111,                           //气密定级负压总渗透量预加压
            QM_DJ_FqzJ = 112,                           //气密定级负压总渗透量测试

            //各气密工程测试阶段
            QM_GC_ZqfY = 121,                           //气密工程正压附加渗透量预加压
            QM_GC_ZqfJ = 122,                           //气密工程正压附加渗透量测试
            QM_GC_FqfY = 123,                           //气密工程负压附加渗透量预加压
            QM_GC_FqfJ = 124,                            //气密工程负压附加渗透量测试
            QM_GC_ZqfgY = 125,                          //气密工程正压附加固定渗透量预加压
            QM_GC_ZqfgJ = 126,                          //气密工程正压附加固定渗透量测试
            QM_GC_FqfgY = 127,                          //气密工程负压附加固定渗透量预加压
            QM_GC_FqfgJ = 128,                          //气密工程负压附加固定渗透量测试
            QM_GC_ZqzY = 129,                           //气密工程正压总渗透量预加压
            QM_GC_ZqzJ = 130,                           //气密工程正压总渗透量测试
            QM_GC_FqzY = 131,                           //气密工程负压总渗透量预加压
            QM_GC_FqzJ = 132,                           //气密工程负压总渗透量测试

            //水密
            SM = 300,                                   //水密性能检测
            //水密定级检测各阶段
            SM_DJ_Y =301,                               //水密定级预加压
            SM_DJ_J=302,                               //水密定级检测加压
            //水密工程检测各阶段
            SM_GC_Y = 321,                               //水密工程预加压
            SM_GC_J = 322,                               //水密工程检测加压

            KFY = 200,                          //B01 抗风压性能检测
            KFY_DJBX_ZY = 201,                             //正压预加压
            KFY_DJBX_ZJ = 202,                             //正压测试
            KFY_DJBX_FY = 203,                             //负压预加压
            KFY_DJBX_FJ = 204,                             //负压测试
            KFY_DJP2_Z = 205,                            //正压反复加压
            KFY_DJP2_F = 206,                            //负压反复加压
            KFY_DJP3_Z = 207,                           //正压安全检测P3
            KFY_DJP3_F = 208,                           //负压安全检测P3
            KFY_DJPmax_Z = 209,                           //正压安全检测Pmax
            KFY_DJPmax_F = 210,                          //负压安全检测Pmax
            KFY_GCBX_ZY = 221,                             //正压预加压
            KFY_GCBX_ZJ = 222,                             //正压测试
            KFY_GCBX_FY = 223,                             //负压预加压
            KFY_GCBX_FJ = 224,                             //负压测试
            KFY_GCP2_Z = 225,                            //正压反复加压
            KFY_GCP2_F = 226,                            //负压反复加压
            KFY_GCP3_Z = 227,                           //正压安全检测P3
            KFY_GCP3_F = 228,                           //负压安全检测P3
            KFY_GCPmax_Z = 229,                           //正压安全检测Pmax
            KFY_GCPmax_F = 230,                          //负压安全检测Pmax

            CJBX = 400,                         //层间变形性能检测
            //层间变形，定级各阶段
            CJBX_DJ_X0=410,                     //层间变形定级，X轴预加载
            CJBX_DJ_X1 = 411,                     //层间变形定级，X轴1级加载
            CJBX_DJ_X2 = 412,                     //层间变形定级，X轴2级加载
            CJBX_DJ_X3 = 413,                     //层间变形定级，X轴3级加载
            CJBX_DJ_X4 = 414,                     //层间变形定级，X轴4级加载
            CJBX_DJ_X5 = 415,                     //层间变形定级，X轴5级加载
            CJBX_DJ_Y0 = 420,                     //层间变形定级，Y轴预加载
            CJBX_DJ_Y1 = 421,                     //层间变形定级，Y轴1级加载
            CJBX_DJ_Y2 = 422,                     //层间变形定级，Y轴2级加载
            CJBX_DJ_Y3 = 423,                     //层间变形定级，Y轴3级加载
            CJBX_DJ_Y4 = 424,                     //层间变形定级，Y轴4级加载
            CJBX_DJ_Y5 = 425,                     //层间变形定级，Y轴5级加载
            CJBX_DJ_Z0 = 430,                     //层间变形定级，Z轴预加载
            CJBX_DJ_Z1 = 431,                     //层间变形定级，Z轴1级加载
            CJBX_DJ_Z2 = 432,                     //层间变形定级，Z轴2级加载
            CJBX_DJ_Z3 = 433,                     //层间变形定级，Z轴3级加载
            CJBX_DJ_Z4 = 434,                     //层间变形定级，Z轴4级加载
            CJBX_DJ_Z5 = 435,                     //层间变形定级，Z轴5级加载
            //层间变形，工程各阶段
            CJBX_GC_X0 = 440,                     //层间变形工程，X轴预加载
            CJBX_GC_X1 = 441,                     //层间变形工程，X轴检测加载
            CJBX_GC_Y0 = 450,                     //层间变形工程，Y轴预加载
            CJBX_GC_Y1 = 451,                     //层间变形工程，Y轴检测加载
            CJBX_GC_Z0 = 460,                     //层间变形工程，Z轴预加载
            CJBX_GC_Z1 = 461,                     //层间变形工程，Z轴检测加载
        }

       

        #endregion

        #region 通讯相关

        /// <summary>
        /// 通讯收发数据命令类型
        /// </summary>
        public enum CommunicationCMDType
        {
            None = 0,                             //未定义

            ReadCoils = 1,                      //读取多个线圈DO
            WriteSingleCoil = 5,                //写单个线圈DO
            WriteMultipleCoils = 15,            //写多个线圈DO

            ReadInputs = 2,                      //读取多个DI
            ReadInputRegisters = 4,             //读取多个AI输入寄存器

            ReadHoldingRegisters = 3,            //读取多个AO保持寄存器
            WriteSingleRegister = 6,              //写单个AO寄存器
            WriteMultipleRegisters = 16,          //写多个AO保持寄存器
            ReadWriteMultipleRegisters = 23,      //读写多个保持寄存器AO
        }

        /// <summary>
        /// 通讯读取数据类别
        /// </summary>
        public enum CommuReadDataType
        {
            None = 0,                             //未定义
        }

        /// <summary>
        /// 通讯寄存器地址
        /// </summary>
        public enum CommuRegAddr
        {
            None = 0,                   //未定义
            //D量区
            YXZT = 4396,               //设备运行状态及数据（PLC反馈参数D300）
            CKCMD = 40768,             //程控指令（PLC反馈参数D8000）
            WYCMD = 40868,             //位移指令（PLC反馈参数D8100）




            YXMS = 4696,                //运行模式
            CSZC = 40768,               //传输数据暂存区D8000
            DCFS = 40868,               //传输数据暂存区D8100
            SDPL1 = 36968,              //手动频率1
            SDPL2 = 36969,              //手动频率2


            //M量区
            WatchDog = 2398,            //看门狗
            DF1KF_ZL = 2448,            //蝶阀1开阀指令
            DF2KF_ZL = 2449,            //蝶阀2开阀指令
            DF3KF_ZL = 2450,            //蝶阀3开阀指令
            DF4KF_ZL = 2451,            //蝶阀4开阀指令
            FJ1FY_ZL = 2452,            //风机1负压指令
            FJ2FY_ZL = 2453,            //风机2负压指令
            CYKF_ZL = 2454,             //差压开阀指令
            XY_ZL = 2455,               //X轴向右指令
            XZ_ZL = 2456,               //X轴向左指令
            YQ_ZL = 2457,               //Y轴向前指令
            YH_ZL = 2458,               //Y轴向后指令
            ZS_ZL = 2459,               //Z轴向上指令
            ZX_ZL = 2460,               //Z轴向下指令
            YYZFK_ZL = 2461,            //液压总阀开阀指令
            KM1_ZL = 2462,              //KM1吸合指令
            KM2_ZL = 2463,              //KM2吸合指令
            YYZ_ZL = 2464,              //液压站启动指令
            SB_ZL = 2465,               //水泵启动指令
            QB_ZL = 2466,               //气泵启动指令
            ERR_ZL = 2467,              //故障输出指令
            BP1_ZL = 2468,              //变频1启动
            BP2_ZL = 2469,              //变频2启动指令
            JJTJ_ZL = 2470,             //紧急停机指令
            KSCS_ZL = 2471,             //开始测试指令
            ZTCS_ZL = 2472,             //暂停待机指令
            XG_ZL = 2473,               //细管指令
            FY_ZL = 2474,               //负压指令
            X_ZL = 2475,                //X轴变形指令
            Y_ZL = 2476,                //Y轴变形指令
            Z_ZL = 2477,                //Z轴变形指令
        }


        /// <summary>
        /// 三合一寄存器地址（赛通）
        /// </summary>
        public enum THP1RegAddr
        {
            H = 0,                   //湿度，无符号，0.1%
            T = 1,                   //温度，有符号，0.1℃
            TH = 0,                   //湿度、温度
            P = 11,                   //大气压力，10Pa或0.1hPa
        }

        /// <summary>
        /// 三合一数据长度
        /// </summary>
        public enum THP1DataQty
        {
            H = 1,                   //湿度
            T = 1,                   //温度
            P = 1,                   //大气压力
            TH = 2,                   //温度、湿度
        }

        /// <summary>
        /// 三合一寄存器地址（森诺中天）
        /// </summary>
        public enum THP2RegAddr
        {
            P = 0,                   //大气压力，hPa
            T = 2,                   //温度，有符号，0.01℃
            H = 3,                   //湿度，无符号，0.01%
            TH = 2,                   //温度、湿度
        }

        /// <summary>
        /// 三合一数据长度（森诺中天）
        /// </summary>
        public enum THP2DataQty
        {
            T = 1,                   //温度
            H = 1,                   //湿度
            P = 1,                   //大气压力
            TH = 2,                   //温度、湿度
        }

        /// <summary>
        /// 三合一寄存器地址（建大仁科）
        /// </summary>
        public enum THP3RegAddr
        {
            H = 0,                   //湿度，无符号，0.1%
            T = 1,                   //温度，有符号，0.1℃
            TH = 0,                   //湿度、温度
            P = 2,                   //大气压力，100Pa或hPa
        }

        /// <summary>
        /// 三合一数据长度（建大仁科）
        /// </summary>
        public enum THP3DataQty
        {
            H = 1,                   //湿度
            T = 1,                   //温度
            P = 1,                   //大气压力
            TH = 2,                   //温度、湿度
        }
        #endregion



    }
}
