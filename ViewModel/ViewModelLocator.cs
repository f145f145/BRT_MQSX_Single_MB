using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace MQDFJ_MB.ViewModel
{
    /// <summary>
    /// ViewModel注册
    /// </summary>
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //注册各ViewModel
            SimpleIoc.Default.Register<LoadingViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
        }


        #region 各ViewModel属性

        /// <summary>
        /// LoadingViewModel
        /// </summary>
        public LoadingViewModel LoadingViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoadingViewModel>();
            }
        }

        /// <summary>
        /// Main_ViewModel主窗口
        /// </summary>
        public MainViewModel Main_ViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        #endregion

    }
}