using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;
using MQZHWL.Model;
using Microsoft.Research.DynamicDataDisplay.Charts;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Threading;

namespace MQZHWL.View.RepAndData
{
    /// <summary>
    /// ExperimentView.xaml 的交互逻辑
    /// </summary>
    public partial class MQZH_CurrentDataView : Window
    {
        public MQZH_CurrentDataView()
        {
            InitializeComponent();

            Legend.SetDescription(Line11, "挠度1");
            Legend.SetDescription(Line12, "挠度2");
            Legend.SetDescription(Line13, "挠度3");

            Legend.SetDescription(Line21, "挠度1");
            Legend.SetDescription(Line22, "挠度2");
            Legend.SetDescription(Line23, "挠度3");

            Legend.SetDescription(Line31, "挠度1");
            Legend.SetDescription(Line32, "挠度2");
            Legend.SetDescription(Line33, "挠度3");

            Legend.SetDescription(Line41, "挠度1");
            Legend.SetDescription(Line42, "挠度2");
            Legend.SetDescription(Line43, "挠度3");

            Legend.SetDescription(LineDJZ1, "挠度1");
            Legend.SetDescription(LineDJZ2, "挠度2");
            Legend.SetDescription(LineDJZ3, "挠度3");
            Legend.SetDescription(LineDJF1, "挠度1");
            Legend.SetDescription(LineDJF2, "挠度2");
            Legend.SetDescription(LineDJF3, "挠度3");

            Legend.SetDescription(LineGCZ1, "挠度1");
            Legend.SetDescription(LineGCZ2, "挠度2");
            Legend.SetDescription(LineGCZ3, "挠度3");
            Legend.SetDescription(LineGCF1, "挠度1");
            Legend.SetDescription(LineGCF2, "挠度2");
            Legend.SetDescription(LineGCF3, "挠度3");


            Messenger.Default.Register<string>(this, "SaveIMG", SaveIMGMessage);

            plot.Focus();
            this.WindowState=WindowState.Normal;
        }



        #region 曲线保存为图片
        /// <summary>
        /// 将挠度曲线另存为图片
        /// </summary>
        /// <param name="msg">试验编号</param>
        private void SaveIMGMessage(string msg)
        {
            string path = msg.Clone().ToString();
            string type = path.Substring(path.Length - 1);
            string fileName1 = msg.Clone().ToString() + "1.jpg";
            string fileName2 = msg.Clone().ToString() + "2.jpg";

            try
            {
                System.IO.FileStream fs1 = new System.IO.FileStream(fileName1, System.IO.FileMode.Create);
                if (type == "1") //定级检测时类型为1，工程检测时类型为2
                {
                    RenderTargetBitmap bmp1 = new RenderTargetBitmap((int)ChartPlotter1.ActualWidth, (int)ChartPlotter1.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
                    bmp1.Render(ChartPlotter1);
                    BitmapEncoder encoder1 = new JpegBitmapEncoder();
                    encoder1.Frames.Add(BitmapFrame.Create(bmp1));
                    encoder1.Save(fs1);
                    fs1.Close();
                }
                else
                {
                    RenderTargetBitmap bmp1 = new RenderTargetBitmap((int)ChartPlotter3.ActualWidth, (int)ChartPlotter3.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
                    bmp1.Render(ChartPlotter3);
                    BitmapEncoder encoder1 = new JpegBitmapEncoder();
                    encoder1.Frames.Add(BitmapFrame.Create(bmp1));
                    encoder1.Save(fs1);
                    fs1.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            try
            {
                System.IO.FileStream fs2 = new System.IO.FileStream(fileName2, System.IO.FileMode.Create);
                if (type == "1") //定级检测时类型为1，工程检测时类型为2
                {
                    RenderTargetBitmap bmp2 = new RenderTargetBitmap((int)ChartPlotter2.ActualWidth, (int)ChartPlotter2.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
                    bmp2.Render(ChartPlotter2);
                    BitmapEncoder encoder2 = new JpegBitmapEncoder();
                    encoder2.Frames.Add(BitmapFrame.Create(bmp2));
                    encoder2.Save(fs2);
                    fs2.Close();
                }
                else
                {
                    RenderTargetBitmap bmp2 = new RenderTargetBitmap((int)ChartPlotter4.ActualWidth, (int)ChartPlotter4.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
                    bmp2.Render(ChartPlotter4);
                    BitmapEncoder encoder2 = new JpegBitmapEncoder();
                    encoder2.Frames.Add(BitmapFrame.Create(bmp2));
                    encoder2.Save(fs2);
                    fs2.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        #endregion


        #region 相关状态

        /// <summary>
        /// 参数变更需要存盘标志
        /// </summary>
        private bool _isNeedSave = false;
        /// <summary>
        /// 参数变更需要存盘标志
        /// </summary>
        public bool IsNeedSave
        {
            get { return _isNeedSave; }
            set
            {
                _isNeedSave = value;
            }
        }

        #endregion

        
        #region 个别按钮及菜单事件

        /// <summary>
        /// 当前试验数据及报告
        /// </summary>
        private void Menu_DataRepCurr_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<string>(MQZH_WinName.CurrentDataWinName  , "OpenGivenNameWin");
        }

        /// <summary>
        /// 调零标校
        /// </summary>
        private void Menu_Corr_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<string>(MQZH_WinName.CorrWinName , "OpenGivenNameWin");
        }

        /// <summary>
        /// 退出菜单
        /// </summary>
        private void Menu_Quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion


        #region 关闭窗口处理方法

        /// <summary>
        /// 窗口退出处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (IsNeedSave)
                {
                    MessageBoxResult msgBoxResult = MessageBox.Show("参数变更后尚未存盘，确认退出程序?", "提示", MessageBoxButton.YesNo);
                    if (msgBoxResult == MessageBoxResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        //注销消息
                        Messenger.Default.Unregister(this);
                        //通知主窗口已关闭某窗口
                        Messenger.Default.Send<string>(this.Name, "WindowClosed");
                    }
                }
                else
                {
                    //注销消息
                    Messenger.Default.Unregister(this);
                    //通知主窗口已关闭某窗口
                    Messenger.Default.Send<string>(this.Name, "WindowClosed");
                }
            }
            catch (Exception)
            {

            }
        }

        #endregion

    }
}

