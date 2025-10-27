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
}