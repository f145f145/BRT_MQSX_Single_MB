using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace MQZHWL.ViewModel
{
    /// <summary>
    /// ViewModel注册
    /// </summary>
    public class MQZH_ViewModelLocator
    {
        public MQZH_ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //注册各ViewModel
            SimpleIoc.Default.Register<MQZH_LoadingViewModel>();
            SimpleIoc.Default.Register<MQZH_MainViewModel>();
        }


        #region 各ViewModel属性

        /// <summary>
        /// LoadingViewModel
        /// </summary>
        public MQZH_LoadingViewModel LoadingViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MQZH_LoadingViewModel>();
            }
        }

        /// <summary>
        /// Main_ViewModel主窗口
        /// </summary>
        public MQZH_MainViewModel Main_ViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MQZH_MainViewModel>();
            }
        }

        #endregion

    }
}