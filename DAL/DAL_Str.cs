using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQDFJ_MB.DAL
{
    public static class DAL_Str
    {
            //文件夹
            public static string AppDir = AppDomain.CurrentDomain.BaseDirectory;                            //exe文件路径
            public static string MainDir = System.IO.Directory.GetParent(AppDir).Parent.Parent.FullName;    //程序根目录
            public static string DataBaseDir = MainDir + "\\Data\\DataBase\\";                              //数据库文件夹
            public static string FormDir = MainDir + "\\Data\\模板\\";                                      //报告模板文件夹
            public static string RepDir = MainDir + "\\Data\\试验报告\\";                                   //检测报告文件夹
            public static string BackUpDir = MainDir + "\\Data\\Backup\\";                                  //数据库备份文件夹

            //数据库文件
            public static string MainDBName = "MainDB.db";                                                  //主数据库
            public static string DevDBName = "DevDB.db";                                                    //硬件参数配置数据库
            public static string ErrLogDBName = "ErrLogDB.db";                                              //错误日志数据库
            public static string OpLogDBName = "OpLogDB.db";                                                //操作日志数据库

            //报告模板
            public static string ExpRepFormName = "报告模板.xls";                                           //报告模板文件名

            //数据库文件完整路径
            public static string MainDBFile = MainDir + "\\Data\\DataBase\\MainDB.db";                      //主数据库
            public static string DevDBFile = MainDir + "\\Data\\DataBase\\DevDB.db";                        //硬件参数配置数据库
            public static string ErrLogDBFile = MainDir + "\\Data\\DataBase\\ErrLogDB.db";                        //错误日志数据库
            public static string OpLogDBFile = MainDir + "\\Data\\DataBase\\OpLogDB.db";                        //操作日志数据库
                                                                                                                 //数据库连接字符串
            public static string ConnString_Main = @"DataSource=" + MainDBFile;                             //主数据库连接字符串（sqlite）
            public static string ConnString_Dev = @"DataSource=" + DevDBFile;                               //装置配置参数数据库连接字符串（sqlite）
            public static string ConnString_ErrLog = @"DataSource=" + ErrLogDBFile;                         //错误日志数据库连接字符串（sqlite）
            public static string ConnString_OpLog = @"DataSource=" + OpLogDBFile;                           //操作日志数据库连接字符串（sqlite）
        }
    }