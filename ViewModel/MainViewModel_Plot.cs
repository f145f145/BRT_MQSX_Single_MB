using GalaSoft.MvvmLight;
using System.Windows;
using System;
using System.Collections.Generic;
using ChartModel;
using System.Drawing;
using Microsoft.Research.DynamicDataDisplay.DataSources;


namespace MQDFJ_MB.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        #region 绘图相关

        #region 报告挠度曲线绘图用

        /// <summary>
        /// 抗风压挠度绘图曲线组-定级正
        /// </summary>
        private List<LineModel> _plotLines_ND_DJZ;
        /// <summary>
        /// 抗风压挠度绘图曲线组-定级正
        /// </summary>
        public List<LineModel> PlotLines_ND_DJZ
        {
            get { return _plotLines_ND_DJZ; }
            set
            {
                _plotLines_ND_DJZ = value;
                RaisePropertyChanged(() => PlotLines_ND_DJZ);
            }
        }


        /// <summary>
        /// 抗风压挠度绘图曲线组-定级负
        /// </summary>
        private List<LineModel> _plotLines_ND_DJF;
        /// <summary>
        /// 抗风压挠度绘图曲线组-定级负
        /// </summary>
        public List<LineModel> PlotLines_ND_DJF
        {
            get { return _plotLines_ND_DJF; }
            set
            {
                _plotLines_ND_DJF = value;
                RaisePropertyChanged(() => PlotLines_ND_DJF);
            }
        }


        /// <summary>
        /// 抗风压挠度绘图曲线组-工程正
        /// </summary>
        private List<LineModel> _plotLines_ND_GCZ;
        /// <summary>
        /// 抗风压挠度绘图曲线组-工程正
        /// </summary>
        public List<LineModel> PlotLines_ND_GCZ
        {
            get { return _plotLines_ND_GCZ; }
            set
            {
                _plotLines_ND_GCZ = value;
                RaisePropertyChanged(() => PlotLines_ND_GCZ);
            }
        }


        /// <summary>
        /// 抗风压挠度绘图曲线组-工程负
        /// </summary>
        private List<LineModel> _plotLines_ND_GCF;
        /// <summary>
        /// 抗风压挠度绘图曲线组-工程负
        /// </summary>
        public List<LineModel> PlotLines_ND_GCF
        {
            get { return _plotLines_ND_GCF; }
            set
            {
                _plotLines_ND_GCF = value;
                RaisePropertyChanged(() => PlotLines_ND_GCF);
            }
        }


        /// <summary>
        /// 绘图数据、测点组数据更新
        /// </summary>
        public void NDPlotUpdate()
        {
            for (int i = 0; i < 3; i++)
            {
                PlotLines_ND_DJZ[i].LineDataSource.Collection.Clear();
                PlotLines_ND_DJF[i].LineDataSource.Collection.Clear();
                PlotLines_ND_GCZ[i].LineDataSource.Collection.Clear();
                PlotLines_ND_GCF[i].LineDataSource.Collection.Clear();
            }
            //测点组数据更新
            try
            {
                double x = 0;
                double y = 0;
                System.Windows.Point newPoint;

                for (int i = 0; i < PublicData.ExpDQ.Exp_KFY.DisplaceGroups.Count; i++)
                {
                    if (PublicData.ExpDQ.Exp_KFY.DisplaceGroups[i].Is_Use)
                    {
                        if (PublicData.ExpDQ.Exp_KFY.IsGC)
                        {
                            //工程变形正压检测
                            //if (PublicData.ExpDQ.Exp_KFY.StageList_KFYGC[1].CompleteStatus)
                            //{
                            for (int j = 0; j < 4; j++)
                            {
                                newPoint = new System.Windows.Point
                                {
                                    X = PublicData.ExpDQ.ExpData_KFY.TestPress_GCBX_Z[j],
                                    Y = PublicData.ExpDQ.ExpData_KFY.ND_GCBX_Z[j + 1][i]
                                };
                                PlotLines_ND_GCZ[i].AddPoint(newPoint);
                            }
                            //}
                            //工程变形负压检测
                            //if (PublicData.ExpDQ.Exp_KFY.StageList_KFYGC[3].CompleteStatus)
                            //{
                            for (int j = 0; j < 4; j++)
                            {
                                newPoint = new System.Windows.Point
                                {
                                    X = PublicData.ExpDQ.ExpData_KFY.TestPress_GCBX_F[j],
                                    Y = PublicData.ExpDQ.ExpData_KFY.ND_GCBX_F[j + 1][i]
                                };
                                PlotLines_ND_GCF[i].AddPoint(newPoint);
                            }
                            //}
                            ////工程P3正压检测
                            //if (PublicData.ExpDQ.Exp_KFY.StageList_KFYGC[6].CompleteStatus)
                            //{
                            //    newPoint = new System.Windows.Point();
                            //    newPoint.X = PublicData.ExpDQ.ExpData_KFY.TestPress_GCP3_Z;
                            //    newPoint.Y = PublicData.ExpDQ.ExpData_KFY.ND_GCP3_Z[1][i];
                            //    PlotLines_ND_GCZ[i].AddPoint(newPoint);
                            //}
                            ////工程P3负压检测
                            //if (PublicData.ExpDQ.Exp_KFY.StageList_KFYGC[7].CompleteStatus)
                            //{
                            //    newPoint = new System.Windows.Point();
                            //    newPoint.X = PublicData.ExpDQ.ExpData_KFY.TestPress_GCP3_F;
                            //    newPoint.Y = PublicData.ExpDQ.ExpData_KFY.ND_GCP3_F[1][i];
                            //    PlotLines_ND_GCF[i].AddPoint(newPoint);
                            //}
                            ////工程Pmax正压检测
                            //if (PublicData.ExpDQ.Exp_KFY.StageList_KFYGC[8].CompleteStatus)
                            //{
                            //    newPoint = new System.Windows.Point();
                            //    newPoint.X = PublicData.ExpDQ.ExpData_KFY.TestPress_GCPmax_Z;
                            //    newPoint.Y = PublicData.ExpDQ.ExpData_KFY.ND_GCPmax_Z[1][i];
                            //    PlotLines_ND_GCZ[i].AddPoint(newPoint);
                            //}
                            ////工程Pmax负压检测
                            //if (PublicData.ExpDQ.Exp_KFY.StageList_KFYGC[9].CompleteStatus)
                            //{
                            //    newPoint = new System.Windows.Point();
                            //    newPoint.X = PublicData.ExpDQ.ExpData_KFY.TestPress_GCPmax_F;
                            //    newPoint.Y = PublicData.ExpDQ.ExpData_KFY.ND_GCPmax_F[1][i];
                            //    PlotLines_ND_GCF[i].AddPoint(newPoint);
                            //}
                        }

                        else
                        {
                            //定级变形正压检测
                            //if (PublicData.ExpDQ.Exp_KFY.StageList_KFYDJ[1].CompleteStatus)
                            //{
                            for (int j = 0; j < 20; j++)
                            {
                                newPoint = new System.Windows.Point
                                {
                                    X = PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_Z[j],
                                    Y = PublicData.ExpDQ.ExpData_KFY.ND_DJBX_Z[j + 1][i]
                                };
                                PlotLines_ND_DJZ[i].AddPoint(newPoint);
                            }
                            //}
                            //定级变形负压检测
                            //if (PublicData.ExpDQ.Exp_KFY.StageList_KFYDJ[3].CompleteStatus)
                            //{
                            for (int j = 0; j < 20; j++)
                            {
                                newPoint = new System.Windows.Point
                                {
                                    X = PublicData.ExpDQ.ExpData_KFY.TestPress_DJBX_F[j],
                                    Y = PublicData.ExpDQ.ExpData_KFY.ND_DJBX_F[j + 1][i]
                                };
                                PlotLines_ND_DJF[i].AddPoint(newPoint);
                            }
                            //}
                            ////定级P3正压检测
                            //if (PublicData.ExpDQ.Exp_KFY.StageList_KFYDJ[6].CompleteStatus)
                            //{
                            //    newPoint = new System.Windows.Point();
                            //    newPoint.X = PublicData.ExpDQ.ExpData_KFY.TestPress_DJP3_Z;
                            //    newPoint.Y = PublicData.ExpDQ.ExpData_KFY.ND_DJP3_Z[1][i];
                            //    PlotLines_ND_DJZ[i].AddPoint(newPoint);
                            //}
                            ////定级P3负压检测
                            //if (PublicData.ExpDQ.Exp_KFY.StageList_KFYDJ[7].CompleteStatus)
                            //{
                            //    newPoint = new System.Windows.Point();
                            //    newPoint.X = PublicData.ExpDQ.ExpData_KFY.TestPress_DJP3_F;
                            //    newPoint.Y = PublicData.ExpDQ.ExpData_KFY.ND_DJP3_F[1][i];
                            //    PlotLines_ND_DJF[i].AddPoint(newPoint);
                            //}
                            ////定级Pmax正压检测
                            //if (PublicData.ExpDQ.Exp_KFY.StageList_KFYDJ[8].CompleteStatus)
                            //{
                            //    newPoint = new System.Windows.Point();
                            //    newPoint.X = PublicData.ExpDQ.ExpData_KFY.TestPress_DJPmax_Z;
                            //    newPoint.Y = PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_Z[1][i];
                            //    PlotLines_ND_DJZ[i].AddPoint(newPoint);
                            //}
                            ////定级Pmax负压检测
                            //if (PublicData.ExpDQ.Exp_KFY.StageList_KFYDJ[9].CompleteStatus)
                            //{
                            //    newPoint = new System.Windows.Point();
                            //    newPoint.X = PublicData.ExpDQ.ExpData_KFY.TestPress_DJPmax_F;
                            //    newPoint.Y = PublicData.ExpDQ.ExpData_KFY.ND_DJPmax_F[1][i];
                            //    PlotLines_ND_DJF[i].AddPoint(newPoint);
                            //}
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        /// <summary>
        /// 挠度曲线绘图初始化
        /// </summary>
        public void NDPlotInit()
        {
            PlotLines_ND_DJZ = new List<LineModel>();
            PlotLines_ND_DJF = new List<LineModel>();
            PlotLines_ND_GCZ = new List<LineModel>();
            PlotLines_ND_GCF = new List<LineModel>();
            try
            {
                //挠度11
                LineModel newLine11 = new LineModel();
                newLine11.LineNO = "ND_L11";
                newLine11.LineName = "挠度11";
                newLine11.VarUnit = "mm";
                newLine11.LineThickness = 0.6;
                newLine11.LineColor = Color.Red;
                newLine11.AxisYside = LineModel.AxisUse.Left;
                newLine11.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine11.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine11.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine11.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine11.IsBackStore = false;
                PlotLines_ND_DJZ.Add(newLine11);
                //挠度12
                LineModel newLine12 = new LineModel();
                newLine12.LineNO = "ND_L12";
                newLine12.LineName = "挠度12";
                newLine12.VarUnit = "mm";
                newLine12.LineThickness = 0.6;
                newLine12.LineColor = Color.Red;
                newLine12.AxisYside = LineModel.AxisUse.Left;
                newLine12.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine12.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine12.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine12.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine12.IsBackStore = false;
                PlotLines_ND_DJZ.Add(newLine12);
                //挠度13
                LineModel newLine13 = new LineModel();
                newLine13.LineNO = "ND_L13";
                newLine13.LineName = "挠度13";
                newLine13.VarUnit = "mm";
                newLine13.LineThickness = 0.6;
                newLine13.LineColor = Color.Red;
                newLine13.AxisYside = LineModel.AxisUse.Left;
                newLine13.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine13.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine13.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine13.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine13.IsBackStore = false;
                PlotLines_ND_DJZ.Add(newLine13);

                //挠度21
                LineModel newLine21 = new LineModel();
                newLine21.LineNO = "ND_L21";
                newLine21.LineName = "挠度21";
                newLine21.VarUnit = "mm";
                newLine21.LineThickness = 0.6;
                newLine21.LineColor = Color.Red;
                newLine21.AxisYside = LineModel.AxisUse.Left;
                newLine21.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine21.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine21.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine21.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine21.IsBackStore = false;
                PlotLines_ND_DJF.Add(newLine21);
                //挠度22
                LineModel newLine22 = new LineModel();
                newLine22.LineNO = "ND_L22";
                newLine22.LineName = "挠度22";
                newLine22.VarUnit = "mm";
                newLine22.LineThickness = 0.6;
                newLine22.LineColor = Color.Red;
                newLine22.AxisYside = LineModel.AxisUse.Left;
                newLine22.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine22.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine22.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine22.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine22.IsBackStore = false;
                PlotLines_ND_DJF.Add(newLine22);
                //挠度23
                LineModel newLine23 = new LineModel();
                newLine23.LineNO = "ND_L23";
                newLine23.LineName = "挠度23";
                newLine23.VarUnit = "mm";
                newLine23.LineThickness = 0.6;
                newLine23.LineColor = Color.Red;
                newLine23.AxisYside = LineModel.AxisUse.Left;
                newLine23.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine23.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine23.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine23.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine23.IsBackStore = false;
                PlotLines_ND_DJF.Add(newLine23);


                //挠度31
                LineModel newLine31 = new LineModel();
                newLine31.LineNO = "ND_L31";
                newLine31.LineName = "挠度31";
                newLine31.VarUnit = "mm";
                newLine31.LineThickness = 0.6;
                newLine31.LineColor = Color.Red;
                newLine31.AxisYside = LineModel.AxisUse.Left;
                newLine31.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine31.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine31.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine31.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine31.IsBackStore = false;
                PlotLines_ND_GCZ.Add(newLine31);
                //挠度32
                LineModel newLine32 = new LineModel();
                newLine32.LineNO = "ND_L32";
                newLine32.LineName = "挠度32";
                newLine32.VarUnit = "mm";
                newLine32.LineThickness = 0.6;
                newLine32.LineColor = Color.Red;
                newLine32.AxisYside = LineModel.AxisUse.Left;
                newLine32.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine32.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine32.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine32.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine32.IsBackStore = false;
                PlotLines_ND_GCZ.Add(newLine32);
                //挠度33
                LineModel newLine33 = new LineModel();
                newLine33.LineNO = "ND_L33";
                newLine33.LineName = "挠度33";
                newLine33.VarUnit = "mm";
                newLine33.LineThickness = 0.6;
                newLine33.LineColor = Color.Red;
                newLine33.AxisYside = LineModel.AxisUse.Left;
                newLine33.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine33.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine33.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine33.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine33.IsBackStore = false;
                PlotLines_ND_GCZ.Add(newLine33);


                //挠度41
                LineModel newLine41 = new LineModel();
                newLine41.LineNO = "ND_L41";
                newLine41.LineName = "挠度41";
                newLine41.VarUnit = "mm";
                newLine41.LineThickness = 0.6;
                newLine41.LineColor = Color.Red;
                newLine41.AxisYside = LineModel.AxisUse.Left;
                newLine41.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine41.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine41.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine41.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine41.IsBackStore = false;
                PlotLines_ND_GCF.Add(newLine41);
                //挠度42
                LineModel newLine42 = new LineModel();
                newLine42.LineNO = "ND_L42";
                newLine42.LineName = "挠度42";
                newLine42.VarUnit = "mm";
                newLine42.LineThickness = 0.6;
                newLine42.LineColor = Color.Red;
                newLine42.AxisYside = LineModel.AxisUse.Left;
                newLine42.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine42.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine42.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine42.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine42.IsBackStore = false;
                PlotLines_ND_GCF.Add(newLine42);
                //挠度43
                LineModel newLine43 = new LineModel();
                newLine43.LineNO = "ND_L43";
                newLine43.LineName = "挠度43";
                newLine43.VarUnit = "mm";
                newLine43.LineThickness = 0.6;
                newLine43.LineColor = Color.Red;
                newLine43.AxisYside = LineModel.AxisUse.Left;
                newLine43.LineDataSource = new ObservableDataSource<System.Windows.Point>();
                newLine43.LineBackGroundPointList = new List<System.Windows.Point>();
                newLine43.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine43.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
                newLine43.IsBackStore = false;
                PlotLines_ND_GCF.Add(newLine43);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }




        #endregion


        #region 检测绘图用


        /// <summary>
        ///更新曲线需要更新消息回调
        /// </summary>
        /// <param name="msg"></param>
        private void UpdateChartMessage(string msg)
        {
            UpdatePlotLins();
        }


        /// <summary>
        /// 绘图用曲线组(0小压差、1中压差、2大压差、3风量1、4风量2、5风量3、6选用风量、7水流量、
        /// 8X轴位移、9Y位移左、10Y位移中、11Y位移右、12Y位移平均、 13Z位移左、14Z位移中、15Z位移右、16Z位移平均
        /// 17挠度1、18挠度2、19挠度3、20相对挠度1、21相对挠度2、22相对挠度3
        /// 23风机频率、24水泵频率
        /// </summary>
        private List<LineModel> _plotLines;
        /// <summary>
        /// 绘图用曲线组(0小压差、1中压差、2大压差、3风量1、4风量2、5风量3、6选用风量、7水流量、
        /// 8X轴位移、9Y位移左、10Y位移中、11Y位移右、12Y位移平均、 13Z位移左、14Z位移中、15Z位移右、16Z位移平均、
        /// 17挠度1、18挠度2、19挠度3、20相对挠度1、21相对挠度2、22相对挠度3、
        /// 23风机频率、24水泵频率/// </summary>
        public List<LineModel> PlotLines
        {
            get { return _plotLines; }
            set
            {
                _plotLines = value;
                RaisePropertyChanged(() => PlotLines);
            }
        }

        /// <summary>
        /// 曲线初始化
        /// </summary>
        public void PlotInit()
        {
            PlotLines = new List<LineModel>();
            //小气压
            LineModel newLine = new LineModel();
            newLine.LineNO = "Line0";
            newLine.LineName = "小气压";
            newLine.VarUnit = "Pa";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Red;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //中气压
            newLine = new LineModel();
            newLine.LineNO = "Line1";
            newLine.LineName = "中气压";
            newLine.VarUnit = "Pa";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.DarkBlue;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //大气压
            newLine = new LineModel();
            newLine.LineNO = "Line2";
            newLine.LineName = "大气压";
            newLine.VarUnit = "Pa";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.DarkBlue;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);

            //风量1
            newLine = new LineModel();
            newLine.LineNO = "Line3";
            newLine.LineName = "风量1";
            newLine.VarUnit = "m³/h";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.DarkGreen;
            newLine.AxisYside = LineModel.AxisUse.Right;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);

            //风量2
            newLine = new LineModel();
            newLine.LineNO = "Line4";
            newLine.LineName = "风量2";
            newLine.VarUnit = "m³/h";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.DarkGreen;
            newLine.AxisYside = LineModel.AxisUse.Right;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);

            //风量3
            newLine = new LineModel();
            newLine.LineNO = "Line5";
            newLine.LineName = "风量3";
            newLine.VarUnit = "m³/h";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.DarkGreen;
            newLine.AxisYside = LineModel.AxisUse.Right;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);

            //选用风量
            newLine = new LineModel();
            newLine.LineNO = "Line6";
            newLine.LineName = "选用风量";
            newLine.VarUnit = "m³/h";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.DarkGreen;
            newLine.AxisYside = LineModel.AxisUse.Right;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);

            //水流量
            newLine = new LineModel();
            newLine.LineNO = "Line7";
            newLine.LineName = "水流量";
            newLine.VarUnit = "m³/h";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);


            //X轴位移
            newLine = new LineModel();
            newLine.LineNO = "Line8";
            newLine.LineName = "X位移";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //Y位移左
            newLine = new LineModel();
            newLine.LineNO = "Line9";
            newLine.LineName = "Y位移左";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //Y位移中
            newLine = new LineModel();
            newLine.LineNO = "Line10";
            newLine.LineName = "Y位移中";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //Y轴位移右
            newLine = new LineModel();
            newLine.LineNO = "Line11";
            newLine.LineName = "Y轴位移右";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //Y轴位移平均
            newLine = new LineModel();
            newLine.LineNO = "Line12";
            newLine.LineName = "Y轴位移平均";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //Z位移左
            newLine = new LineModel();
            newLine.LineNO = "Line13";
            newLine.LineName = "Z位移左";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //Z位移中
            newLine = new LineModel();
            newLine.LineNO = "Line14";
            newLine.LineName = "Z位移中";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //Z轴位移右
            newLine = new LineModel();
            newLine.LineNO = "Line15";
            newLine.LineName = "Z轴位移右";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //Z轴位移平均
            newLine = new LineModel();
            newLine.LineNO = "Line16";
            newLine.LineName = "Z轴位移平均";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);

            //挠度1
            newLine = new LineModel();
            newLine.LineNO = "Line17";
            newLine.LineName = "挠度1";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.6;
            newLine.LineColor = Color.Red;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //挠度2
            newLine = new LineModel();
            newLine.LineNO = "Line18";
            newLine.LineName = "挠度2";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.6;
            newLine.LineColor = Color.DarkGreen;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //挠度3
            newLine = new LineModel();
            newLine.LineNO = "Line19";
            newLine.LineName = "挠度3";
            newLine.VarUnit = "mm";
            newLine.LineThickness = 0.6;
            newLine.LineColor = Color.Blue;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);

            //相对挠度1
            newLine = new LineModel();
            newLine.LineNO = "Line20";
            newLine.LineName = "相对挠度1";
            newLine.VarUnit = "";
            newLine.LineThickness = 0.6;
            newLine.LineColor = Color.Red;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //相对挠度2
            newLine = new LineModel();
            newLine.LineNO = "Line21";
            newLine.LineName = "相对挠度2";
            newLine.VarUnit = "";
            newLine.LineThickness = 0.6;
            newLine.LineColor = Color.DarkGreen;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //相对挠度3
            newLine = new LineModel();
            newLine.LineNO = "Line22";
            newLine.LineName = "相对挠度3";
            newLine.VarUnit = "";
            newLine.LineThickness = 0.6;
            newLine.LineColor = Color.Blue;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);

            //风机频率输出
            newLine = new LineModel();
            newLine.LineNO = "Line23";
            newLine.LineName = "频率1";
            newLine.VarUnit = "HZ";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.DarkViolet;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //水泵频率输出
            newLine = new LineModel();
            newLine.LineNO = "Line24";
            newLine.LineName = "水泵频率";
            newLine.VarUnit = "HZ";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Black;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = PublicData.Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
        }


        /// <summary>
        ///绘图数据更新消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void UpdatePlotLins()
        {
            TimeSpan span = DateTime.Now - PublicData.Dev.PowerOnTime;

            //小气压
            System.Windows.Point newPoint0 = new System.Windows.Point();
            newPoint0.X = span.TotalSeconds;
            newPoint0.Y = PublicData.Dev.AIList[12].ValueFinal;
            PlotLines[0].AddPoint(newPoint0);

            //中气压
            if (PublicData.Dev.IsWithCYM)
            {
                System.Windows.Point newPoint1 = new System.Windows.Point();
                newPoint1.X = span.TotalSeconds;
                newPoint1.Y = PublicData.Dev.AIList[13].ValueFinal;
                PlotLines[1].AddPoint(newPoint1);
            }

            //大气压
            System.Windows.Point newPoint2 = new System.Windows.Point();
            newPoint2.X = span.TotalSeconds;
            newPoint2.Y = PublicData.Dev.AIList[14].ValueFinal;
            PlotLines[2].AddPoint(newPoint2);

            //风量1
            System.Windows.Point newPoint3 = new System.Windows.Point();
            newPoint3.X = span.TotalSeconds;
            newPoint3.Y = PublicData.Dev.FL1;
            PlotLines[3].AddPoint(newPoint3);

            //风量2
            System.Windows.Point newPoint4 = new System.Windows.Point();
            newPoint4.X = span.TotalSeconds;
            newPoint4.Y = PublicData.Dev.FL2;
            PlotLines[4].AddPoint(newPoint4);

            //风量3
            System.Windows.Point newPoint5 = new System.Windows.Point();
            newPoint5.X = span.TotalSeconds;
            newPoint5.Y = PublicData.Dev.FL3;
            PlotLines[5].AddPoint(newPoint5);

            //气密风量
            System.Windows.Point newPoint6 = new System.Windows.Point();
            newPoint6.X = span.TotalSeconds;
            switch (PublicData.Dev.FSGUse)
            {
                case 1:
                    newPoint6.Y = PublicData.Dev.FL1;
                    break;
                case 2:
                    newPoint6.Y = PublicData.Dev.FL2;
                    break;
                case 3:
                    newPoint6.Y = PublicData.Dev.FL3;
                    break;
                default:
                    newPoint6.Y = PublicData.Dev.FL2;
                    break;
            }
            PlotLines[6].AddPoint(newPoint6);

            //水流量
            if (PublicData.Dev.IsWithSLL)
            {
                System.Windows.Point newPoint7 = new System.Windows.Point();
                newPoint7.X = span.TotalSeconds;
                newPoint7.Y = PublicData.Dev.SLL;
                PlotLines[7].AddPoint(newPoint7);
            }

            //X轴位移
            System.Windows.Point newPoint8 = new System.Windows.Point();
            newPoint8.X = span.TotalSeconds;
            newPoint8.Y = PublicData.Dev.WYValueX;
            PlotLines[8].AddPoint(newPoint8);

            //Y轴位移左
            System.Windows.Point newPoint9 = new System.Windows.Point();
            newPoint9.X = span.TotalSeconds;
            newPoint9.Y = PublicData.Dev.WYValueY[0];
            PlotLines[9].AddPoint(newPoint9);
            //Y轴位移中
            System.Windows.Point newPoint10 = new System.Windows.Point();
            newPoint10.X = span.TotalSeconds;
            newPoint10.Y = PublicData.Dev.WYValueY[1];
            PlotLines[10].AddPoint(newPoint10);
            //Y轴位移右
            System.Windows.Point newPoint11 = new System.Windows.Point();
            newPoint11.X = span.TotalSeconds;
            newPoint11.Y = PublicData.Dev.WYValueY[2];
            PlotLines[11].AddPoint(newPoint11);
            //Y轴位移平均
            System.Windows.Point newPoint12 = new System.Windows.Point();
            newPoint12.X = span.TotalSeconds;
            newPoint12.Y = PublicData.Dev.WYValueY[3];
            PlotLines[12].AddPoint(newPoint12);

            //Z轴位移左
            System.Windows.Point newPoint13 = new System.Windows.Point();
            newPoint13.X = span.TotalSeconds;
            newPoint13.Y = PublicData.Dev.WYValueZ[0];
            PlotLines[13].AddPoint(newPoint13);
            //Z轴位移中
            System.Windows.Point newPoint14 = new System.Windows.Point();
            newPoint14.X = span.TotalSeconds;
            newPoint14.Y = PublicData.Dev.WYValueZ[1];
            PlotLines[14].AddPoint(newPoint14);
            //Z轴位移右
            System.Windows.Point newPoint15 = new System.Windows.Point();
            newPoint15.X = span.TotalSeconds;
            newPoint15.Y = PublicData.Dev.WYValueZ[2];
            PlotLines[15].AddPoint(newPoint15);
            //Z轴位移平均
            System.Windows.Point newPoint16 = new System.Windows.Point();
            newPoint16.X = span.TotalSeconds;
            newPoint16.Y = PublicData.Dev.WYValueZ[3];
            PlotLines[16].AddPoint(newPoint16);

            //风机频率
            System.Windows.Point newPoint23 = new System.Windows.Point();
            newPoint23.X = span.TotalSeconds;
            newPoint23.Y = PublicData.Dev.AIList[22].ValueFinal;
            PlotLines[23].AddPoint(newPoint23);

            //水泵频率
            if (PublicData.Dev.IsWithSLL)
            {
                System.Windows.Point newPoint24 = new System.Windows.Point();
                newPoint24.X = span.TotalSeconds;
                newPoint24.Y = PublicData.Dev.AIList[23].ValueFinal;
                PlotLines[24].AddPoint(newPoint24);
            }

            if (IsKFYWinOpened)
            {
                //测点组数据更新
                try
                {
                    if (PublicData.Dev.IsWYKFYF)
                    {
                        for (int i = 0; i < PublicData.ExpDQ.Exp_KFY.DisplaceGroups.Count; i++)
                        {
                            if (PublicData.ExpDQ.Exp_KFY.DisplaceGroups[i].Is_Use)
                            {
                                for (int j = 0; j < PublicData.ExpDQ.Exp_KFY.DisplaceGroups[i].WY_DQ.Count; j++)
                                {
                                    int k = PublicData.ExpDQ.Exp_KFY.DisplaceGroups[i].WYC_No[j];
                                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[i].WY_DQ[j] = PublicData.Dev.WyWPL[k - 1];
                                    if (j == 3)
                                    {
                                        if ((i == 0) && PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].Is_Use && PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].Is_TestMBBX && PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].IsBZCSJMB)
                                            PublicData.ExpDQ.Exp_KFY.DisplaceGroups[i].WY_DQ[j] = PublicData.Dev.WyWPL[k - 1];
                                        else
                                            PublicData.ExpDQ.Exp_KFY.DisplaceGroups[i].WY_DQ[j] = 0;
                                    }
                                }
                                PublicData.ExpDQ.Exp_KFY.DisplaceGroups[i].NDJS();
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < PublicData.ExpDQ.Exp_KFY.DisplaceGroups.Count; i++)
                        {
                            if (PublicData.ExpDQ.Exp_KFY.DisplaceGroups[i].Is_Use)
                            {
                                for (int j = 0; j < PublicData.ExpDQ.Exp_KFY.DisplaceGroups[i].WY_DQ.Count; j++)
                                {
                                    int k = PublicData.ExpDQ.Exp_KFY.DisplaceGroups[i].WYC_No[j];
                                    PublicData.ExpDQ.Exp_KFY.DisplaceGroups[i].WY_DQ[j] = PublicData.Dev.AIList[k - 1].ValueFinal;
                                    if (j == 3)
                                    {
                                        if ((i == 0) && PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].Is_Use && PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].Is_TestMBBX && PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].IsBZCSJMB)
                                            PublicData.ExpDQ.Exp_KFY.DisplaceGroups[i].WY_DQ[j] = PublicData.Dev.AIList[k - 1].ValueFinal;
                                        else
                                            PublicData.ExpDQ.Exp_KFY.DisplaceGroups[i].WY_DQ[j] = 0;
                                    }
                                }
                                PublicData.ExpDQ.Exp_KFY.DisplaceGroups[i].NDJS();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }


                if (PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].Is_Use)
                {
                    //挠度1
                    System.Windows.Point newPoint17 = new System.Windows.Point();
                    newPoint17.X = span.TotalSeconds;
                    newPoint17.Y = PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].ND;
                    PlotLines[17].AddPoint(newPoint17);
                    //相对挠度1
                    System.Windows.Point newPoint20 = new System.Windows.Point();
                    newPoint20.X = span.TotalSeconds;
                    newPoint20.Y = PublicData.ExpDQ.Exp_KFY.DisplaceGroups[0].ND_XD;
                    PlotLines[20].AddPoint(newPoint20);
                }
                if (PublicData.ExpDQ.Exp_KFY.DisplaceGroups[1].Is_Use)
                {
                    //挠度2
                    System.Windows.Point newPoint18 = new System.Windows.Point();
                    newPoint18.X = span.TotalSeconds;
                    newPoint18.Y = PublicData.ExpDQ.Exp_KFY.DisplaceGroups[1].ND;
                    PlotLines[18].AddPoint(newPoint18);
                    //相对挠度2
                    System.Windows.Point newPoint21 = new System.Windows.Point();
                    newPoint21.X = span.TotalSeconds;
                    newPoint21.Y = PublicData.ExpDQ.Exp_KFY.DisplaceGroups[1].ND_XD;
                    PlotLines[21].AddPoint(newPoint21);
                }
                if (PublicData.ExpDQ.Exp_KFY.DisplaceGroups[2].Is_Use)
                {
                    //挠度3
                    System.Windows.Point newPoint19 = new System.Windows.Point();
                    newPoint19.X = span.TotalSeconds;
                    newPoint19.Y = PublicData.ExpDQ.Exp_KFY.DisplaceGroups[2].ND;
                    PlotLines[19].AddPoint(newPoint19);
                    //相对挠度3
                    System.Windows.Point newPoint22 = new System.Windows.Point();
                    newPoint22.X = span.TotalSeconds;
                    newPoint22.Y = PublicData.ExpDQ.Exp_KFY.DisplaceGroups[2].ND_XD;
                    PlotLines[22].AddPoint(newPoint22);
                }
            }
        }


        #endregion

        #endregion
    }
}
