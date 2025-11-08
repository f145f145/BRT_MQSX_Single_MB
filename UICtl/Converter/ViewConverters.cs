/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQDFJ_MB.View
 * 文件名：  BoolToVisibilityConverter
 * 版本号：  V1.0.0.0
 * 唯一标识：a260e1ec-7bf5-4f5c-bedd-f6166604e876
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/12/11 19:17:58
 * 描述：
 * DIDO状态转换为Visibility属性
 *
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2021/12/11       19:17:58		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using static MQDFJ_MB.Model.MQZH_Enums;

namespace MQDFJ_MB.UICtl.Converter
{

    /// <summary>
    /// Bool到对号的转换器
    /// </summary>
    public class BoolToTickConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "√" : "×";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value == "√") ? true : false;
        }
    }

    /// <summary>
    /// Bool取反转换器
    /// </summary>
    public class BoolToNotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Bool到visibility的转换器
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Bool到visibility的反向转换器
    /// </summary>
    public class BoolToVisibilityNotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Bool到collor的普通状态转换器（灰绿）
    /// </summary>
    public class BoolToCollorGConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Chartreuse" : "DarkGray";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Bool到collor的普通状态转换器（灰橙）
    /// </summary>
    public class BoolToCollorOConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "DarkOrange" : "DarkGray";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    /// <summary>
    /// Bool到collor的普通状态转换器（绿橙）
    /// </summary>
    public class BoolToCollorNotOConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Chartreuse" : "DarkOrange";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    /// <summary>
    /// Bool到collor的普通状态转换器（灰红）
    /// </summary>
    public class BoolToCollorRConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Red" : "DarkGray";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 总阀状态颜色转换器（蓝绿橙）
    /// </summary>
    public class StrToCollorOConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "正压")
                return "Chartreuse";
            else if ((string)value == "负压")
                return "DarkBlue";
            else
                return "DarkOrange";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// int到collor的普通状态转换器
    /// </summary>
    public class IntToCollorOConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return "DarkOrange";
                    break;
                case 1:
                    return "Chartreuse";
                    break;
                case 2:
                    return "DarkGray";
                    break;
                default: return "DarkGray";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    /// <summary>
    /// 检测类型转换器
    /// </summary>
    public class BoolToExpTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "工程检测" : "定级检测";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value.ToString() == "工程检测")
            //    return true;
            //if (value.ToString() == "定级检测")
            //    return false;
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 符合设计转换器
    /// </summary>
    public class BoolToMeetDesignConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "满足" : "不满足";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value.ToString() == "满足")
            //    return true;
            //if (value.ToString() == "不满足")
            //    return false;
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 完成状态转换器
    /// </summary>
    public class BoolToExpCompleteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "√" : "×";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 有无损坏状态转换器
    /// </summary>
    public class BoolToDamageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "有损坏" : "无损坏";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "有损坏")
                return true;
            return false;
        }
    }


    /// <summary>
    /// 串口校验位转换器
    /// </summary>
    public class StrToParityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((System.IO.Ports.Parity)value == System.IO.Ports.Parity.Odd)
                return "奇";
            if ((System.IO.Ports.Parity)value == System.IO.Ports.Parity.Even)
                return "偶";
            if ((System.IO.Ports.Parity)value == System.IO.Ports.Parity.None)
                return "无";
            if ((System.IO.Ports.Parity)value == System.IO.Ports.Parity.Mark)
                return "标志";
            if ((System.IO.Ports.Parity)value == System.IO.Ports.Parity.Space)
                return "空格";
            return "无";

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "奇")
                return System.IO.Ports.Parity.Odd;
            if ((string)value == "偶")
                return System.IO.Ports.Parity.Even;
            if ((string)value == "无")
                return System.IO.Ports.Parity.None;
            if ((string)value == "标志")
                return System.IO.Ports.Parity.Mark;
            if ((string)value == "空格")
                return System.IO.Ports.Parity.Space;
            return System.IO.Ports.Parity.None;
        }
    }

    /// <summary>
    /// 串口停止位转换器
    /// </summary>
    public class StrToStopBitsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((System.IO.Ports.StopBits)value == System.IO.Ports.StopBits.None)
                return "0";
            if ((System.IO.Ports.StopBits)value == System.IO.Ports.StopBits.One )
                return "1";
            if ((System.IO.Ports.StopBits)value == System.IO.Ports.StopBits.OnePointFive )
                return "1.5";
            if ((System.IO.Ports.StopBits)value == System.IO.Ports.StopBits.Two )
                return "2";
            return "无";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "0")
                return System.IO.Ports.StopBits.None;
            if ((string)value == "1")
                return System.IO.Ports.StopBits.One;
            if ((string)value == "1.5")
                return System.IO.Ports.StopBits.OnePointFive;
            if ((string)value == "2")
                return System.IO.Ports.StopBits.Two;
            return System.IO.Ports.StopBits.None;
        }
    }

    /// <summary>
    /// 水密渗漏况转换器
    /// </summary>
    public class BoolToSMSLConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 0)
                return "无渗漏";
            if ((int)value == 1)
                return "○";
            if ((int)value == 2)
                return "□";
            if ((int)value == 3)
                return "△";
            if ((int)value == 4)
                return "▲";
            if ((int)value == 5)
                return "●";
            if ((int)value ==6 )
                return "未检测";
            return "未检测";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 粗管细管转换器
    /// </summary>
    public class IntToCGXGConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 1:
                    return "主管";
                    break;
                case 2:
                    return "粗管";
                    break;
                case 3:
                    return "细管";
                    break;
                case 4:
                    return "加粗管";
                    break;
                default:
                    return "粗管";
                    break;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 加压方式转换器
    /// </summary>
    public class BoolToWaveConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "波动加压" : "稳定加压";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 装置运行模式文字
    /// </summary>
    public class BoolToDevStatusWZConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return "待机";

                case 1:
                    return "单点调试";

                case 2:
                    return "手动正压调速";

                case 3:
                    return "手动负压调速";

                case 4:
                    return "手动水泵调速";

                case 5:
                    return "蝶阀1正压气密调试";

                case 6:
                    return "蝶阀1负压气密调试";

                case 7:
                    return "蝶阀2正压气密调试";

                case 8:
                    return "蝶阀2负压气密调试";

                case 9:
                    return "蝶阀3正压气密调试";

                case 10:
                    return "蝶阀3负压气密调试";

                case 11:
                    return "蝶阀4正压气密调试";

                case 12:
                    return "蝶阀4负压气密调试";

                case 13:
                    return "大风压PID正压调试";

                case 14:
                    return "大风压PID负压调试";

                case 15:
                    return "水流量PID调试";

                case 16:
                    return "手动位移调试";

                case 20:
                    return "气密检测";

                case 30:
                    return "水密检测";

                case 41:
                    return "抗风压P1检测";
                case 42:
                    return "抗风压P2检测";
                case 43:
                    return "抗风压P3检测";
                case 44:
                    return "抗风压Pmax检测";

                case 50:
                    return "层间变形检测";
                
                default:
                    return "待机";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 换向阀当前模式转换器
    /// </summary>
    public class IntToFFModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 1:
                    return "待机模式";

                case 2:
                    return "故障模式";

                case 3:
                    return "自检模式";

                case 4:
                    return "手动模式";

                case 5:
                    return "正压工作模式";

                case 6:
                    return "负压工作模式";

                case 7:
                    return "正压低压准备";

                case 8:
                    return "负压低压准备";

                case 9:
                    return "次数波动正压";

                case 10:
                    return "次数波动负压";

                case 21:
                    return "持续正向旋转";

                case 22:
                    return "持续反向旋转";

                case 88:
                    return "复位模式";

                default:
                    return "待机模式";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 换向阀故障类型转换器
    /// </summary>
    public class IntToFFErrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return "无故障";

                case 1:
                    return "自检失败";

                case 2:
                    return "伺服报警";

                case 3:
                    return "正压定位失败";

                case 4:
                    return "负压定位失败";

                case 5:
                    return "正压准备失败";

                case 6:
                    return "负压准备失败";

                case 7:
                    return "正压波动失败";

                case 8:
                    return "负压波动失败";

                default:
                    return "无故障";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 换向阀自检状态转换器
    /// </summary>
    public class IntToFFSTConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return "未自检";

                case 1:
                    return "负压自检中";

                case 2:
                    return "正压自检中";

                case 3:
                    return "自检完成";

                case 4:
                    return "自检失败";

                default:
                    return "未自检";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 换向阀手动状态转换器
    /// </summary>
    public class IntToFFSDConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return "未动作";

                case 1:
                    return "正压动作中";

                case 2:
                    return "正压到位";

                case 3:
                    return "负压动作中";

                case 4:
                    return "负压到位";

                case 5:
                    return "手动超时";

                default:
                    return "未动作";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 换向阀正压低压准备状态转换器
    /// </summary>
    public class IntToFFZDZBConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return "未动作";

                case 1:
                    return "寻找中";

                case 2:
                    return "准备完成";

                case 3:
                    return "脉冲超限";

                default:
                    return "未动作";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 换向阀负压低压准备状态转换器
    /// </summary>
    public class IntToFFFDZBConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return "未动作";

                case 1:
                    return "寻找中";

                case 2:
                    return "准备完成";

                case 3:
                    return "脉冲超限";

                default:
                    return "未动作";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 换向阀正压波动状态转换器
    /// </summary>
    public class IntToFFZBConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return "未动作";

                case 1:
                    return "压力减小";

                case 2:
                    return "压力最小值";

                case 3:
                    return "压力增大";

                case 4:
                    return "压力最大值";

                case 5:
                    return "波动完成";

                case 6:
                    return "故障";

                default:
                    return "未动作";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 换向阀负压波动状态转换器
    /// </summary>
    public class IntToFFFBConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return "未动作";

                case 1:
                    return "压力减小";

                case 2:
                    return "压力最小值";

                case 3:
                    return "压力增大";

                case 4:
                    return "压力最大值";

                case 5:
                    return "波动完成";

                case 6:
                    return "故障";

                default:
                    return "未动作";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 换向阀当前动作转换器
    /// </summary>
    public class IntToFFActionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return "未动作";

                case 1:
                    return "压力减小";

                case 2:
                    return "压力最小值";

                case 3:
                    return "压力增大";

                case 4:
                    return "压力最大值";

                case 5:
                    return "波动完成";

                case 6:
                    return "故障";

                default:
                    return "未动作";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Bool到visibility的反向转换器
    /// </summary>
    public class BoolNotToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Visibility)value == Visibility.Visible) ? false : true;
        }
    }

    /// <summary>
    /// INT到方向的转换器
    /// </summary>
    public class IntToDirConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value == 1) ? "↑" : "↓";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value == "↑") ? 1 : -1;
        }
    }


    /// <summary>
    /// Bool到有无的转换器
    /// </summary>
    public class BoolToYWConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "有" : "无";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value == "有") ? true : false;
        }
    }

    /// <summary>
    /// Bool到正负压的转换器
    /// </summary>
    public class BoolToNegativeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "负压" : "正压";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value == "负压") ? true : false;
        }
    }

    /// <summary>
    /// 时间字符串的转换器
    /// </summary>
    public class TimeToStrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = "";
            DateTime time = (DateTime)value;
            str = time.ToShortTimeString();
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// int到分秒字符串的转换器
    /// </summary>
    public class IntToMMSSStrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = "";
            int time = (int)value;
            if (time >= 3600)
            {
                int hh = time / 3600;
                int mm = (time - hh * 3600) / 60;
                int ss = time % 60;
                str = hh + "时 " + mm.ToString() + "分 " + ss.ToString() + "秒";
                return str;
            }
            else if (time >= 60)
            {
                int mm = time / 60;
                int ss = time % 60;
                str = mm.ToString() + "分 " + ss.ToString() + "秒";
                return str;
            }
            else
                str = time.ToString() + "秒";
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Double
    /// </summary>
    public class DoubleToMMSSStrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = "";
            int time = (int)(double)value;
            if (time >= 3600)
            {
                int hh = time / 3600;
                int mm = (time - hh * 3600) / 60;
                int ss = time % 60;
                str = hh + "时 " + mm.ToString() + "分 " + ss.ToString() + "秒";
                return str;
            }
            else if (time >= 60)
            {
                int mm = time / 60;
                int ss = time % 60;
                str = mm.ToString() + "分 " + ss.ToString() + "秒";
                return str;
            }
            else
                str = time.ToString() + "秒";
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 风速仪类型的文字转换器
    /// </summary>
    public class FSYTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 2)
                return "微压计式";
            return "数字热线式";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "微压计式")
                return 2;
            return 1;
        }
    }


    /// <summary>
    /// 机组类别的文字转换器
    /// </summary>
    public class UnitTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 1)
                return "蒸气压缩";
            if ((int)value == 2)
                return "燃气溴化锂";
            if ((int)value == 3)
                return "燃油溴化锂";
            return "未知";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "蒸气压缩")
                return 1;
            if ((string)value == "燃气溴化锂")
                return 2;
            if ((string)value == "燃油溴化锂")
                return 3;
            return 0;
        }
    }

    /// <summary>
    /// 圆管方管类型的文字转换器
    /// </summary>
    public class IsRoundnessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return "圆形风管";
            return "矩形风管";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "圆形风管")
                return true;
            return false;
        }
    }

    /// <summary>
    /// 热线风速仪的显示转换器
    /// </summary>
    public class IsRXConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 1)
                return Visibility.Visible;
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 微压差风速仪的显示转换器
    /// </summary>
    public class IsWYCConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 2)
                return Visibility.Visible;
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// Bool到collor的普通状态转换器（白绿）
    /// </summary>
    public class BoolToCollorGGConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Chartreuse" : "Snow";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    /// <summary>
    /// Bool到collor的普通状态转换器（白橙）
    /// </summary>
    public class BoolToCollorWConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "DarkOrange" : "Snow";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 测试阶段文字
    /// </summary>
    public class TestStageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if ((TestStage)value == TestStage.TCtl_Stage)
            //    return "温度准备";
            //if ((TestStage)value == TestStage.Test_Stage)
            //    return "30min测试";
            //if ((TestStage)value == TestStage.TestEnd_Stage)
            //    return "测试完成";

            return "待机阶段";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if ((string)value == "温度准备")
            //    return TestStage.TCtl_Stage;
            //if ((string)value == "30min测试")
            //    return TestStage.Test_Stage;
            //if ((string)value == "测试完成")
            //    return TestStage.TestEnd_Stage;

            //return TestStage.Wait_Stage;
            return 1;
        }
    }



    /// <summary>
    /// 检测方法转换器
    /// </summary>
    public class TestMethodConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 0)
                return "正压检测(加压)";
            if ((int)value == 1)
                return "负压检测(减压)";
            if ((int)value == 2)
                return "正负压全测";

            return "正负压全测";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "正压检测(加压)")
                return 0;
            if ((string)value == "负压检测(减压)")
                return 1;
            if ((string)value == "正负压全测")
                return 2;

            return 2;
        }
    }


    /// <summary>
    /// 室外风速类型转换器
    /// </summary>
    public class AirSpeedTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 0)
                return "地面风速";
            if ((int)value == 1)
                return "气象风速";
            if ((int)value == 2)
                return "蒲福风力";

            return "地面风速";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "地面风速")
                return 0;
            if ((string)value == "气象风速")
                return 1;
            if ((string)value == "蒲福风力")
                return 2;

            return 0;
        }
    }

    /// <summary>
    /// FanQty到visibility的转换器2
    /// </summary>
    public class FanQtyToVisibilityConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value >= 2) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// FanQty到visibility的转换器3
    /// </summary>
    public class FanQtyToVisibilityConverter3 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value >= 3) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// FanQty到visibility的转换器4
    /// </summary>
    public class FanQtyToVisibilityConverter4 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value >= 4) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// FanQty到visibility的转换器5
    /// </summary>
    public class FanQtyToVisibilityConverter5 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value >= 5) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// FanQty到visibility的转换器6
    /// </summary>
    public class FanQtyToVisibilityConverter6 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value >= 6) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// FanQty到visibility的转换器7
    /// </summary>
    public class FanQtyToVisibilityConverter7 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value >= 7) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// FanQty到visibility的转换器8
    /// </summary>
    public class FanQtyToVisibilityConverter8 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value >= 8) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 风速visibility的转换器0-地面风速
    /// </summary>
    public class AirSpeedVisibilityConverter0 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value == 0) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 风速visibility的转换器1-气象风速
    /// </summary>
    public class AirSpeedVisibilityConverter1 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value == 1) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 风速visibility的转换器2-蒲福风力
    /// </summary>
    public class AirSpeedVisibilityConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value == 2) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 炉壁校准高度
    /// </summary>
    public class WallCalHeithtConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 1:
                    return "a:30 mm";
                case 2:
                    return "b:0 mm";
                case 3:
                    return "c:-30 mm";
                default:
                    return "0 mm";

            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 时间段到天、小时的转换
    /// </summary>
    public class TimeSpanToStrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan span = (TimeSpan)value;
            int day = span.Days;
            double hour = (span.Seconds - day * 86400) / 3600;
            return day.ToString() + "天" + hour.ToString("F1") + "时";
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// bool到绑定单双向的转换
    /// </summary>
    public class BoolToBinddingModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? BindingMode.TwoWay : BindingMode.OneWay;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}