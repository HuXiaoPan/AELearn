﻿using AddFile;
using CommonTools;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using MapViewControl;
using Model;
using SaveMxd;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MapOperation.FrmMeasureResult;

namespace MapOperation
{
    public partial class MainFrm : Form
    {
        #region 全局变量
        AddMxdHelper addMxdHelper;
        ViewControlHelper viewControlHelper;
        IToolRunControl toolRunControl;
        IToolGetResult toolGetResult;
        SelectFeatureTool selectFeatureTool;

        MapUnitHelper mapUnitHelper;

        FrmMeasureResult frmMeasureResult = null;   //数据测量窗口
        FrmMapExport frmMapExport;
        //绘图工具
        INewLineFeedback newLineFeedback;
        INewPolygonFeedback newPolygonFeedback;
        //绘图缓存点
        private IPoint clickPT = null;
        private IPoint movePT = null;
        private IPointCollection areaPointColl = new MultipointClass();
        #endregion

        #region 初始化
        public MainFrm()
        {
            InitializeComponent();
            addMxdHelper = new AddMxdHelper();
            viewControlHelper = new ViewControlHelper();
            mapUnitHelper = new MapUnitHelper(mainMapControl.Map.MapUnits);
            toolRunControl = null;
            toolGetResult = null;
        }
        #endregion

        #region 各种为文件加载
        #region 地图文档加载
        #region 控件方法加载地图文档
        private void btnLoadMxFile_Click(object sender, EventArgs e)
        {
            addMxdHelper.LoadMxFile(mainMapControl);
        }
        #endregion

        #region IMapDocument接口方法加载
        private void btnIMapDocument_Click(object sender, EventArgs e)
        {
            addMxdHelper.IMapDocumentLoadMxd(mainMapControl);
        }
        #endregion

        #region ICommand接口加载地图文档
        private void btncontrolsOpenDocCommandClass_Click(object sender, EventArgs e)
        {
            addMxdHelper.ICommandLoadMxd(mainMapControl);
        }
        #endregion

        #endregion

        #region shp文件加载
        private void btnAddShapefile_Click(object sender, EventArgs e)
        {
            AddShpHelper addShpHelper = new AddShpHelper();
            addShpHelper.AddShpFile(mainMapControl);
        }
        #endregion

        #region 栅格数据加载
        private void btnAddRaster_Click(object sender, EventArgs e)
        {
            AddRasterHelper addRasterHelper = new AddRasterHelper();
            addRasterHelper.AddRasterFile(mainMapControl);
        }
        #endregion

        #region CAD数据加载
        #region CAD数据单独添加
        private void btnAddCADByLayer_Click(object sender, EventArgs e)
        {
            AddCADHelper addCADHelper = new AddCADHelper();
            addCADHelper.AddCADByShp(mainMapControl);
        }
        #endregion

        #region CAD数据整体添加
        private void btnAddWholeCAD_Click(object sender, EventArgs e)
        {
            AddCADHelper addCADHelper = new AddCADHelper();
            addCADHelper.AddWholeCAD(mainMapControl);
        }
        #endregion

        #region CAD作为栅格背景图加载
        private void btnAddRasterByCAD_Click(object sender, EventArgs e)
        {
            AddCADHelper addCADHelper = new AddCADHelper();
            addCADHelper.AddCADByRaster(mainMapControl);
        }
        #endregion

        #endregion

        #region mdb加载
        private void btnAddPersonGeodatabase_Click(object sender, EventArgs e)
        {
            AddMdbHelper addMdbHelper = new AddMdbHelper();
            addMdbHelper.AddMdb(mainMapControl);
        }
        #endregion

        #region 加载文件数据库
        private void btnAddFileDatabase_Click(object sender, EventArgs e)
        {
            AddFolderDbHelper addFolderDbHelper = new AddFolderDbHelper();
            addFolderDbHelper.AddFileDb(mainMapControl);
        }
        #endregion

        #region SDE加载
        #region SDE数据库方式加载
        private void btnAddSDEByService_Click(object sender, EventArgs e)
        {
            MessageBox.Show("没做SDE数据库，这个功能就没写");
        }
        #endregion

        #region SDE直连方式加载
        private void btnAddSDEByDirect_Click(object sender, EventArgs e)
        {
            MessageBox.Show("没做SDE数据库，这个功能就没写");
        }
        #endregion

        #endregion

        #region 文本及Excel数据转shp数据加载
        private void btnAddTxt_Click(object sender, EventArgs e)
        {
            FrmAddTxt frmAddTxt = new FrmAddTxt(mainMapControl);
            frmAddTxt.Show();
        }
        #endregion

        #endregion

        #region 保存地图文档
        #region 直接保存
        private void btnSaveMap_Click(object sender, EventArgs e)
        {
            SaveMxdHelper saveMxdHelper = new SaveMxdHelper();
            saveMxdHelper.SaveMap(mainMapControl);
        }
        #endregion

        #region 另存为
        private void btnSaveAsMap_Click(object sender, EventArgs e)
        {
            SaveMxdHelper saveMxdHelper = new SaveMxdHelper();
            saveMxdHelper.SaveAsMap(mainMapControl);
        }
        #endregion

        #endregion

        #region 地图浏览
        #region 选框放大
        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            toolRunControl = viewControlHelper.ViewZoomIn(mainMapControl);
        }
        #endregion

        #region 选框缩小
        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            toolRunControl = viewControlHelper.ViewZoomOut(mainMapControl);
        }
        #endregion

        #region 固定比例放大
        private void btnZoomInStep_Click(object sender, EventArgs e)
        {
            viewControlHelper.ViewZoomInStep(mainMapControl);
        }
        #endregion

        #region 固定比例缩小
        private void btnZoomOutStep_Click(object sender, EventArgs e)
        {
            viewControlHelper.ViewZoomOutStep(mainMapControl);
        }
        #endregion

        #region 地图漫游
        private void btnPan_Click(object sender, EventArgs e)
        {
            toolRunControl = viewControlHelper.ViewPan(mainMapControl);
        }
        #endregion

        #region 前一视图
        private void btnFrontView_Click(object sender, EventArgs e)
        {
            viewControlHelper.FrontView(mainMapControl, btnForWardView, btnFrontView);
        }
        #endregion

        #region 后一视图
        private void btnForWardView_Click(object sender, EventArgs e)
        {
            viewControlHelper.ForwardView(mainMapControl, btnFrontView, btnForWardView);
        }
        #endregion

        #region 地图全图
        private void btnFullView_Click(object sender, EventArgs e)
        {
            mainMapControl.Extent = mainMapControl.FullExtent;
        }
        #endregion
        #endregion

        #region 书签模块
        #region 创建书签
        private void btnAddBookMark_Click(object sender, EventArgs e)
        {
            FrmBookMarkAdd frmBookMarkAdd = new FrmBookMarkAdd();
            frmBookMarkAdd.ShowDialog();
            if (frmBookMarkAdd.IsSave == false) return; //判断是否保存书签
            // 空白字符串判断
            if (string.IsNullOrWhiteSpace(frmBookMarkAdd.BookMarkName))
            {
                MessageBox.Show("书签名为空，请重新添加", "提示");
                return;
            }
            // 1.得到地图的书签集
            IMapBookmarks bookmarks = mainMapControl.Map as IMapBookmarks;
            // 判断书签是否存在
            IEnumSpatialBookmark enumSpatialBookmark = bookmarks.Bookmarks;
            enumSpatialBookmark.Reset();
            ISpatialBookmark spatialBookmark;
            while ((spatialBookmark = enumSpatialBookmark.Next()) != null)
            {
                // 如果有重名书签选择是否替换
                if (spatialBookmark.Name == frmBookMarkAdd.BookMarkName)
                {
                    DialogResult result = MessageBox.Show("该书签名已存在，请问是否替换原书签？", "提示", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.OK)
                    {
                        //移除原书签
                    }
                    else
                    {
                        return;
                    }
                }
            }
            // 2.创建书签，并给范围，名字等属性赋值
            IAOIBookmark bookmark = new AOIBookmarkClass();
            bookmark.Name = frmBookMarkAdd.BookMarkName; // 名字
            IActiveView activeView = mainMapControl.ActiveView;
            bookmark.Location = activeView.Extent; // 位置
            // 3.向书签集里添加书签
            bookmarks.AddBookmark(bookmark);
        }
        #endregion

        #region 管理书签
        private void btnMangeBookMark_Click(object sender, EventArgs e)
        {
            FrmManagerBookmark frm = new FrmManagerBookmark(mainMapControl.Map);
            frm.ShowDialog();
        }
        #endregion

        #endregion

        #region 测量模块
        #region 长度测量
        private void btnMeasureLength_Click(object sender, EventArgs e)
        {
            // 测量窗口对象
            MeasureFrmMaker measureFrmMaker = new MeasureFrmMaker(mainMapControl);
            measureFrmMaker.GetMeasureFrm(EnumMeasureOperation.lengthMeasure, ref frmMeasureResult);
            frmMeasureResult.frmClose += new FrmCloseEventHandle(ResetMeasureData);
            frmMeasureResult.Show();
            // 线反馈工具
            toolRunControl = new DrawLineFeedbackTool(newLineFeedback,mainMapControl);
            toolGetResult = toolRunControl as IToolGetResult;
        }
        #endregion

        #region 面积测量
        private void btnMeasureArea_Click(object sender, EventArgs e)
        {
            // 测量窗口对象
            MeasureFrmMaker measureFrmMaker = new MeasureFrmMaker(mainMapControl);
            measureFrmMaker.GetMeasureFrm(EnumMeasureOperation.areaMeasure, ref frmMeasureResult);
            frmMeasureResult.frmClose += new FrmCloseEventHandle(ResetMeasureData);
            frmMeasureResult.Show();
            // 面反馈工具
            toolRunControl = new DrawPolygonFeedbackTool(newPolygonFeedback, mainMapControl);
            toolGetResult = toolRunControl as IToolGetResult;
        }
        #endregion

        #region 关闭测量窗口时重置
        private void ResetMeasureData()
        {
            toolRunControl = null;
            toolGetResult = null;
            if (newLineFeedback != null)
            {
                newLineFeedback.Stop();
                newLineFeedback = null;
            }
            if (newPolygonFeedback != null)
            {
                newPolygonFeedback.Stop();
                newPolygonFeedback = null;
                //areaPointColl.RemovePoints(0, areaPointColl.PointCount);
            }
            mainMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
            mainMapControl.MousePointer = ESRI.ArcGIS.Controls.esriControlsMousePointer.esriPointerDefault;
        }
        #endregion
        #endregion

        #region 要素操作模块
        #region 要素选择
        private void btnSelFeature_Click(object sender, EventArgs e)
        {
            selectFeatureTool = new SelectFeatureTool(mainMapControl);
            toolRunControl = selectFeatureTool;
        }
        #endregion

        #region 要素缩放
        private void btnZoomToSel_Click(object sender, EventArgs e)
        {
            int selectFeatureNum = mainMapControl.Map.SelectionCount;
            if (selectFeatureNum == 0)
            {
                MessageBox.Show("请先选择要素", "提示");
            }
            else
            {
                ISelection selection = mainMapControl.Map.FeatureSelection;
                IEnumFeature enumFeature = selection as IEnumFeature;
                enumFeature.Reset();
                IEnvelope env = new EnvelopeClass();
                IFeature feature = null;
                while ((feature = enumFeature.Next()) != null)
                {
                    env.Union(feature.Extent);
                }
                env.Expand(1.1, 1.1, true);
                mainMapControl.ActiveView.Extent = env;
                mainMapControl.ActiveView.Refresh();
            }
        }
        #endregion

        #region 要素清除
        private void btnClearSel_Click(object sender, EventArgs e)
        {
            mainMapControl.ActiveView.FocusMap.ClearSelection();
            mainMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, mainMapControl.ActiveView.Extent);
        }
        #endregion
        #endregion

        #region 地图导出模块
        #region 区域导出
        private void btnExportRegion_Click(object sender, EventArgs e)
        {
            toolRunControl = new ExportRegionTool(mainMapControl);
        }
        #endregion

        #region 全局导出
        private void btnExportMap_Click(object sender, EventArgs e)
        {
            if (frmMapExport == null || frmMapExport.IsDisposed)
            {
                frmMapExport = new FrmMapExport(mainMapControl.ActiveView);
            }
            frmMapExport.IsRegion = false;
            frmMapExport.GetGeometry = mainMapControl.ActiveView.Extent;
            frmMapExport.Show();
            frmMapExport.Activate();
        }
        #endregion
        #endregion

        #region 地图控件操作事件
        /* 
         * 这里用了设计模式，通过一个接口让所有操作控件的工具各自实现自己的方法
         * 但是这里有一个失误就是接口的参数应该时所有工具都可能用到的一个参数
         * 而我已经设计完了，所以只能用到什么参数就添加什么参数
         * 但弊端就是改一个接口所有实现类都得改
         * 以后不这样
         * 
         * 20190507
         * 
         * 一开始没设计好真是太糟心了
         * 不太想改了，修修补补成了一堆垃圾代码
         * 以后不这样了
         */
        #region 鼠标按下事件
        private void mainMapControl_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            if (selectFeatureTool != null) selectFeatureTool.GetEventArgs(e);
            clickPT = (mainMapControl.Map as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
            if (toolRunControl != null)
            {
                toolRunControl.OnMouseDownRun(clickPT);
            }
            if (toolRunControl is ExportRegionTool)
            {
                if (frmMapExport == null || frmMapExport.IsDisposed)
                {
                    frmMapExport = new FrmMapExport(mainMapControl.ActiveView);
                }
                frmMapExport.IsRegion = true;
                frmMapExport.GetGeometry = (toolRunControl as ExportRegionTool).polygon;
                frmMapExport.Show();
                frmMapExport.Activate();
            }
        }
        #endregion

        #region 鼠标移动事件
        private void mainMapControl_OnMouseMove(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseMoveEvent e)
        {
            // 底部信息显示坐标和单位
            barCoorTxt.Text = String.Format("当前的坐标为：X = {0:#.###} , Y = {1:#.###} {2}", e.mapX, e.mapY, mapUnitHelper.GetMapUnitString());
            movePT = (mainMapControl.Map as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
            if (toolRunControl != null)
            {
                toolRunControl.OnMouseMoveRun(movePT);
            }
            if (toolGetResult !=null)
            {
                frmMeasureResult.lblMeasureResult.Text = toolGetResult.GetResult(mapUnitHelper.GetMapUnitString());
            }
        }
        #endregion

        #region 鼠标抬起事件
        private void mainMapControl_OnMouseUp(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseUpEvent e)
        {
            if (toolRunControl != null)
            {
                toolRunControl.OnMouseUpRun();
            }
        }
        #endregion

        #region 地图替换事件
        private void mainMapControl_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            // 地图替换时更新底部坐标信息的单位
            mapUnitHelper = new MapUnitHelper(mainMapControl.Map.MapUnits);
        }
        #endregion

        #region 鼠标双击事件
        private void mainMapControl_OnDoubleClick(object sender, IMapControlEvents2_OnDoubleClickEvent e)
        {
            if (toolRunControl != null)
            {
                toolRunControl.OnDoubleClickRun();
            }
            if (toolGetResult != null)
            {
                frmMeasureResult.lblMeasureResult.Text = toolGetResult.GetResult(mapUnitHelper.GetMapUnitString());
            }
        }
        #endregion

        #endregion
    }
}
