/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQZHWL.View
 * 文件名：  WindowsManager
 * 版本号：  V1.0.0.0
 * 唯一标识：2c194190-d345-484a-88a1-aa27b2d7490c
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/10/26 12:11:58
 * 描述：
 * 使用泛型管理窗口。
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2021/10/26       12:11:58		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight.Messaging;
using System;
using System.ComponentModel;
using System.Windows;

namespace MQZHWL.View
{
    public class WindowsManager<TWindow> where TWindow : Window, new()
    {
        static TWindow window;

        public static TWindow Show(object vm,string name,Window owner)
        {
            double screenWidth = System.Windows.SystemParameters.WorkArea.Width;
            double screenHeight = System.Windows.SystemParameters.WorkArea.Height;
            bool exist=true;
            if ((window == null)||(!window.IsLoaded ))
            {
                exist = false;
                window = null;
                window = new TWindow() { Name = name, Owner = owner };

                //通知主窗口已创建新窗口
                Messenger.Default.Send<Window>(window, "NewWindow");
            }
            window.Show();
            window.Activate();
            window.Focus();
            if (exist)
            {
                window.WindowState = WindowState.Normal;
                window.WindowStartupLocation =WindowStartupLocation.CenterScreen ;
            }
            return window;
        }
    }

}
