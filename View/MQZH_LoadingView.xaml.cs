using System;
using System.IO;
using System.Windows;
using MQZHWL.View.Authorize;
using MQZHWL.Model;
using Authorize;

namespace MQZHWL.View
{
    /// <summary>
    /// MQZH_LoadingView.xaml 的交互逻辑
    /// </summary>
    public partial class MQZH_LoadingView : Window
    {

        public MQZH_LoadingView()
        {
            InitializeComponent();

            //授权检查
            AuthorizeMQ.AppType = 28;        //软件类型为幕墙
            AuthorizeMQ.AuthDAL=new AuthorizeDAL(AuthorizeMQ.AppType);
            if (!CheckIn())
            {
                AuthorizeMQ.AuthDAL.SaveSN("");
                Info.Content = "授权验证失败，请重新注册！";
                ReAuthorize.Visibility = Visibility.Visible;
                return;
            }
            else
                ReAuthorize.Visibility = Visibility.Hidden;
            //授权验证通过后，更新最后运行时间。
            AuthorizeMQ.AuthDAL.SaveLastTime();

            //文件检查
            if (!CheckFiles())
            {
                Info .Content = "文件检查失败！";
                return;
            }
            //进入系统
            GetIn();
            this.Close();
        }

        #region 交互按钮

        public SN_InputView SN_InputWin;

        /// <summary>
        /// 重新注册按钮
        /// </summary>
        private void Button_ReReg_Click(object sender, RoutedEventArgs e)
        {
            SN_InputWin = new SN_InputView();
            SN_InputWin.Name = MQZH_WinName.SN_InputWinName;

            SN_InputWin.Show();
            SN_InputWin.Activate();
            SN_InputWin.Focus();
            Close();
        }
        
        /// <summary>
        /// 退出
        /// </summary>
        private void Button_Quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        #endregion


        #region 启动成功处理

        private string mqzh_MainWinName = "MQZH_MainView";

        public MQZH_MainView MQZH_MainView;

        /// <summary>
        /// 进入系统
        /// </summary>
        private void GetIn()
        {
            double screenWidth = System.Windows.SystemParameters.WorkArea.Width;
            double screenHeight = System.Windows.SystemParameters.WorkArea.Height;
            MQZH_MainView = new MQZH_MainView();
            MQZH_MainView.Name = mqzh_MainWinName;

            MQZH_MainView.Show();
            MQZH_MainView.Activate();
            MQZH_MainView.Focus();
            MQZH_MainView.Top = screenHeight / 2 - MQZH_MainView.Height / 2;
            MQZH_MainView.Left = screenWidth / 2 - MQZH_MainView.Width / 2;
            MQZH_MainView.WindowState = WindowState.Maximized;
        }

        #endregion


        #region 授权检测

        private AuthorizeBLL _authorizeMQ = new AuthorizeBLL();
        public AuthorizeBLL AuthorizeMQ
        {
            get { return _authorizeMQ;}
            set { _authorizeMQ = value; }
        }

        /// <summary>
        /// 授权检测
        /// </summary>
        public bool CheckIn()
        {
            //检测是否已注册
            if (!AuthorizeMQ.AuthDAL.CheckRegExist())
            {
                MessageBox.Show("未检测到授权信息！");
                return false;
            }

            //检测注册码有效性
            string tempSN = AuthorizeMQ.AuthDAL.GetRegSN();
            int tempRet = AuthorizeMQ.CheckSN(tempSN, AuthorizeMQ.AppType);
            if (tempRet == 1)
            {
                MessageBox.Show("未检测到授权信息！");
                return false;
            }
            if (tempRet == 2)
            {
                MessageBox.Show("授权无效1！");
                return false;
            }
            if (tempRet == 3)
            {
                MessageBox.Show("授权无效2！");
                return false;
            }
            if (tempRet == 4)
            {
                MessageBox.Show("授权无效3！");
                return false;
            }
            if (tempRet == 5)
            {
                MessageBox.Show("授权无效4！");
                return false;
            }
            
            //检测时间有效性
            DateTime tempTime = AuthorizeMQ.AuthDAL.GetLastTime();
            if (tempTime > DateTime.Now)
            {
                MessageBox.Show("系统时间有误！");
                Close();
            }

            return true;
        }

        #endregion


        #region 文件检查

        private bool CheckFiles()
        {
            bool tempRet = true;

            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string mainDir = System.IO.Directory.GetParent(appDir).Parent.Parent.FullName;
            string devDataDir = mainDir + "\\Data\\DataBase\\MQZH_DB_Dev.mdb";
            string testDataDir = mainDir + "\\Data\\DataBase\\MQZH_DB_Test.mdb";
            string initialDataDir = mainDir + "\\Data\\DataBase\\MQZH_Initial.mdb";
            string cjbxRepFrmDir = mainDir + "\\Data\\模板\\幕墙层间变形报告.xls";
            string sanRepFrmDir = mainDir + "\\Data\\模板\\幕墙三性检测报告.xls";

            if (!File.Exists(devDataDir))
            {
                MessageBox.Show("文件'" + devDataDir + "' 不存在，请检查文件是否丢失！");
                tempRet = false;
            }

            if (!File.Exists(testDataDir))
            {
                MessageBox.Show("文件'" + testDataDir + "' 不存在，请检查文件是否丢失！");
                tempRet = false;
            }
            if (!File.Exists(initialDataDir))
            {
                MessageBox.Show("文件'" + initialDataDir + "' 不存在，请检查文件是否丢失！");
                tempRet = false;
            }
            if (!File.Exists(cjbxRepFrmDir))
            {
                MessageBox.Show("文件'" + cjbxRepFrmDir + "' 不存在，请检查文件是否丢失！");
                tempRet = false;
            }
            if (!File.Exists(sanRepFrmDir))
            {
                MessageBox.Show("文件'" + sanRepFrmDir + "' 不存在，请检查文件是否丢失！");
                tempRet = false;
            }
            return tempRet;
        }

        #endregion
    }
}