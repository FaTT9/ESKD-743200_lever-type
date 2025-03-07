using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swcommands;
using System.Threading;


namespace laba5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int SketchCounter = 0;

        private void FirstFigureBuildIntBtn_Click(object sender, EventArgs e)
        {
            SldWorks swApp = new SldWorks();
            swApp.Visible = true;
            swApp.NewPart();

            ModelDoc2 swModelDoc = swApp.ActiveDoc;
            SketchManager swSketchManager = swModelDoc.SketchManager;
            SelectionMgr selectionMgr = swModelDoc.SelectionManager;

            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, false); //Отключение подтверждения размера


            FeatureManager swFeatureMgr = swModelDoc.FeatureManager;
            swModelDoc.SetUnits(0, 1, 0, 0, false);

            //Язык
            string Line;
            string sketch;

            string TopPlane;
            string FrontPlane;
            string RightPlane;

            if (PlaneRusCb.Checked)
            {
                TopPlane = "Сверху";
                FrontPlane = "Спереди";
                RightPlane = "Справа";
            }
            else
            {
                TopPlane = "Top Plane";
                FrontPlane = "Front Plane";
                RightPlane = "Right Plane";
            }
            if (LinesRusCb.Checked)
            {
                sketch = "Эскиз";
                Line = "Линия";
            }
            else
            {
                sketch = "Sketch";
                Line = "Line";
            }

            SketchCounter = 0;

            ////FIRST FIGURE BUILD
            double FirstFigureWidth = Convert.ToDouble(FirstFigureWidthTb.Text) / 1000;
            double FirstFigureLength = Convert.ToDouble(FirstFigureLengthTb.Text) / 1000;
            double FirstFigureHeight = Convert.ToDouble(FirstFigureHeightTb.Text) / 1000;
            double FirstFigureRadius = Convert.ToDouble(FirstFigureRadiusTb.Text) / 1000;
            double FirstFigureDistance = Convert.ToDouble(FirstFigureDistanceTb.Text) / 1000;

            double FirstPoint = -FirstFigureWidth / 2;
            double SecondPoint = -FirstFigureLength / 2;

            //proverki
            if (FirstPoint + FirstFigureDistance - FirstFigureRadius <= FirstPoint)
            {
                MessageBox.Show("Радиусы кругов заходят за пределы фигуры ");
            }
            if (FirstPoint + FirstFigureDistance + FirstFigureRadius >= FirstPoint + FirstFigureWidth / 2)
            {
                MessageBox.Show("Слишком большие значения радиуса или расстояния (по горизонтали)");
            }
            if (SecondPoint + FirstFigureDistance + FirstFigureRadius >= SecondPoint + FirstFigureLength / 2)
            {
                MessageBox.Show("Слишком большие значения радиуса или расстояния ( по вертикали)");
            }


            swModelDoc.Extension.SelectByID2(TopPlane, "PLANE", 0, 0, 0, true, 0, null, 0);

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateLine(FirstPoint, SecondPoint + FirstFigureLength, 0.05, FirstPoint + FirstFigureWidth, SecondPoint + FirstFigureLength, 0.05); //1 (верхняя линия и идёт вправо)
            swSketchManager.CreateLine(FirstPoint + FirstFigureWidth, SecondPoint + FirstFigureLength, 0, FirstPoint + FirstFigureWidth, SecondPoint, 0); //2
            swSketchManager.CreateLine(FirstPoint + FirstFigureWidth, SecondPoint, 0, FirstPoint, SecondPoint, 0); //3
            swSketchManager.CreateLine(FirstPoint, SecondPoint, 0, FirstPoint, SecondPoint + FirstFigureLength, 0); //4

            swModelDoc.ClearSelection2(true);

            swSketchManager.CreateCircleByRadius(FirstPoint + FirstFigureDistance, SecondPoint + FirstFigureLength - FirstFigureDistance, 0, FirstFigureRadius); //left-up

            swSketchManager.CreateCircle(FirstPoint + FirstFigureWidth - FirstFigureDistance, SecondPoint + FirstFigureLength - FirstFigureDistance, 0,
                FirstPoint + FirstFigureWidth - FirstFigureDistance - FirstFigureRadius, SecondPoint + FirstFigureLength - FirstFigureDistance, 0); //right-up

            swSketchManager.CreateCircle(FirstPoint + FirstFigureWidth - FirstFigureDistance, SecondPoint + FirstFigureDistance, 0,
                FirstPoint + FirstFigureWidth - FirstFigureDistance, SecondPoint + FirstFigureDistance + FirstFigureRadius, 0); //right-down

            swSketchManager.CreateCircle(FirstPoint + FirstFigureDistance, SecondPoint + FirstFigureDistance, 0, FirstPoint + FirstFigureDistance + FirstFigureRadius, SecondPoint + FirstFigureDistance, 0); //left-down (то самое колечко)

            swModelDoc.ClearSelection2(true);

            swModelDoc.Extension.SelectByID2(sketch + "1", "SKETCH", 0, 0, 0, false, 0, null, 0);
            SketchCounter++;

            // бобышка
            swFeatureMgr.FeatureExtrusion2(true, false, false, 0, 0, FirstFigureHeight, 0, false, false, false, false, 0, 0, false, false, false, false, true, true, true, 0, 0, true);
            swModelDoc.ClearSelection2(true);





            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, true);


            swModelDoc.ForceRebuild3(false);
        }

        private void SecondFigureBuildIntBtn_Click(object sender, EventArgs e)
        {
            SldWorks swApp = new SldWorks();
            ModelDoc2 swModelDoc = swApp.ActiveDoc;
            if (!CheckSketchManager(swModelDoc))
            {
                return; // Останавливаем выполнение
            }
            SketchManager swSketchManager = swModelDoc.SketchManager;
            SelectionMgr selectionMgr = swModelDoc.SelectionManager;

            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, false); //Отключение подтверждения размера

            FeatureManager swFeatureMgr = swModelDoc.FeatureManager;
            swModelDoc.SetUnits(0, 1, 0, 0, false); //0 - MM
                                                    //swSketchMgr.InsertSketch(true);


            //Язык
            string Line;
            string sketch;

            string TopPlane;
            string FrontPlane;
            string RightPlane;

            if (PlaneRusCb.Checked)
            {
                TopPlane = "Сверху";
                FrontPlane = "Спереди";
                RightPlane = "Справа";
            }
            else
            {
                TopPlane = "Top Plane";
                FrontPlane = "Front Plane";
                RightPlane = "Right Plane";
            }
            if (LinesRusCb.Checked)
            {
                sketch = "Эскиз";
                Line = "Линия";
            }
            else
            {
                sketch = "Sketch";
                Line = "Line";
            }

            ////SECOND FIGURE BUILD
            double SecondFigureRadius = Convert.ToDouble(SecondFigureRadiusTb.Text) / 1000;
            double SecondFigureHeight = Convert.ToDouble(SecondFigureHeightTb.Text) / 1000;
            double SecondFigureOutRadius = Convert.ToDouble(SecondFigureOutRadiusTb.Text) / 1000;
            double SecondFigureInRadius = Convert.ToDouble(SecondFigureInRadiusTb.Text) / 1000;
            double SecondFigureDifference = Convert.ToDouble(SecondFigureDifferenceTb.Text) / 1000;
            double SecondFigureWidthCut = Convert.ToDouble(SecondFigureWidthCutTb.Text) / 1000;
            double SecondFigureLengthCut = Convert.ToDouble(SecondFigureLenghtCutTb.Text) / 1000;
            double SecondFigureHoleRadius = Convert.ToDouble(SecondFigureHoleRadiusTb.Text) / 1000;
            double CoordXMove = Convert.ToDouble(CoordXMoveTb.Text) / 1000;
            double CoordZMove = Convert.ToDouble(CoordZMoveTb.Text) / 1000;
            bool isUpCut = isUpCutCb.Checked;
            bool isLeftCut = isLeftCutCb.Checked;
            bool isRightCut = isRightCutCb.Checked;
            bool isDownCut = isDownCutCb.Checked;

            double FirstFigureWidth = Convert.ToDouble(FirstFigureWidthTb.Text) / 1000;
            double FirstFigureLength = Convert.ToDouble(FirstFigureLengthTb.Text) / 1000;
            double FirstFigureHeight = Convert.ToDouble(FirstFigureHeightTb.Text) / 1000;
            double FirstPoint = -FirstFigureWidth / 2;
            double SecondPoint = -FirstFigureLength / 2;

            if(SecondFigureOutRadius <= SecondFigureInRadius || SecondFigureRadius <= SecondFigureOutRadius)
            {
                MessageBox.Show("Наружный радиус меньше внутреннего");
                return;
            }
            if (SecondFigureOutRadius - SecondFigureInRadius < 2 / 1000)
            {
                MessageBox.Show("Разница наружного радиуса и внутреннего меньше 2 см, Некорректное выделение (SLDWORKS API ERROR)");
                return;
            }
            if (SecondFigureRadius * 2 >= FirstFigureWidth)
            {
                MessageBox.Show("Радиус больше ширины Первой фигуры");
                return;
            }
            if ((SecondFigureOutRadius + SecondFigureDifference) * 2 >= FirstFigureWidth)
            {
                MessageBox.Show("Слишком большая разницы внутри детали, она будет выходить за пределы фигуры");
                return;
            }
            if (SecondFigureInRadius - SecondFigureDifference <= 0)
            {
                MessageBox.Show("Слишком маленькая разницы внутри детали или сам радиус");
                return;
            }
            if (CoordXMove + SecondFigureRadius >= FirstFigureWidth / 2)
            {
                MessageBox.Show("Вторая деталь заходит за пределы первой детали (справа)");
                return;
            }
            if (CoordXMove - SecondFigureRadius <= -FirstFigureWidth / 2)
            {
                MessageBox.Show("Вторая деталь заходит за пределы первой детали (слева)");
                return;
            }
            if (CoordZMove + SecondFigureRadius >= FirstFigureLength / 2)
            {
                MessageBox.Show("Вторая деталь заходит за пределы первой детали (сверху)");
                return;
            }
            if (CoordZMove - SecondFigureRadius <= -FirstFigureLength / 2)
            {
                MessageBox.Show("Вторая деталь заходит за пределы первой детали (снизу)");
                return;
            }
            if (SecondFigureHoleRadius >= SecondFigureInRadius - SecondFigureDifference)
            {
                MessageBox.Show("Радиус Центрального выреза слишком большой");
                return;
            }

            if (isUpCut || isLeftCut || isRightCut || isDownCut)
            {
                if (CoordZMove + SecondFigureInRadius + (SecondFigureOutRadius - SecondFigureInRadius) / 2 + SecondFigureLengthCut / 2 >= FirstFigureLength / 2)
                {
                    MessageBox.Show("У центральных вырезов слишком большая длина (будут задевать 3 деталь)");
                    return;
                }
            }    


            swModelDoc.ShowNamedView2("*Top", 5);

            //выделяем вверх
            swModelDoc.Extension.SelectByRay(0, FirstFigureHeight, 0, 0, 1, 0, 0.01, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateCircleByRadius(CoordXMove, CoordZMove, 0, SecondFigureRadius);

            SketchCounter++;

            swModelDoc.Extension.SelectByID2(sketch + SketchCounter, "SKETCH", 0, 0, 0, false, 0, null, 0);

            swFeatureMgr.FeatureExtrusion2(true, false, false, 0, 0, SecondFigureHeight, 0, false, false, false, false, 0, 0, false, false, false, false, true, true, true, 0, 0, true);

            swModelDoc.ClearSelection2(true);

            // выделяем еще раз вверх

            swModelDoc.Extension.SelectByRay(CoordXMove, FirstFigureHeight + SecondFigureHeight, CoordZMove, 0, 1, 0, 0.01, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateCircleByRadius(CoordXMove, CoordZMove, 0, SecondFigureOutRadius);

            swSketchManager.CreateCircleByRadius(CoordXMove, CoordZMove, 0, SecondFigureInRadius);

            swModelDoc.ClearSelection2(true);

            swSketchManager.InsertSketch(false);

            SketchCounter++;

            swModelDoc.Extension.SelectByID2(sketch + SketchCounter, "SKETCH", 0, 0, 0, false, 0, null, 0);

            swFeatureMgr.FeatureCut4(true, false, false, 0, 0, SecondFigureHeight / 2, 0, false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);

            swModelDoc.ClearSelection2(true);

            // выделяем внутренее кольцо

            swModelDoc.Extension.SelectByRay(CoordXMove + SecondFigureInRadius + (SecondFigureOutRadius - SecondFigureInRadius) / 2, FirstFigureHeight, -CoordZMove, 0, 1, 0, 0.01, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateCircleByRadius(CoordXMove, CoordZMove, 0, SecondFigureOutRadius + SecondFigureDifference);

            swSketchManager.CreateCircleByRadius(CoordXMove, CoordZMove, 0, SecondFigureInRadius - SecondFigureDifference);

            swModelDoc.ClearSelection2(true);

            swSketchManager.InsertSketch(false);

            SketchCounter++;

            swModelDoc.Extension.SelectByID2(sketch + SketchCounter, "SKETCH", 0, 0, 0, false, 0, null, 0);

            swFeatureMgr.FeatureCut4(true, false, true, 0, 0, SecondFigureHeight / 4, 0, false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);

            swModelDoc.ClearSelection2(true);

            //4 хуйни
            

            if (isUpCut || isLeftCut || isRightCut || isDownCut)
            {
                swModelDoc.Extension.SelectByRay(0, FirstFigureHeight + SecondFigureHeight, 0, 0, 1, 0, 0.01, 2, false, 0, 0); //Тут собственно грань выбирается

                double RadDiff = SecondFigureOutRadius - SecondFigureInRadius;
                double CoordX;
                double CoordZ;
                swSketchManager.InsertSketch(true);
                if (isUpCut)
                {
                    CoordX = 0;
                    CoordZ = 0 + SecondFigureInRadius + RadDiff / 2;

                    //построение
                    swSketchManager.CreateLine(CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0, CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0); //1 (нижняя линия и идёт слева-вправо)
                    swSketchManager.CreateLine(CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0, CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0); //2
                    swSketchManager.CreateLine(CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0, CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0); //3
                    swSketchManager.CreateLine(CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0, CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0); //4

                }
                if (isLeftCut)
                {
                    CoordX = 0 - SecondFigureInRadius - RadDiff / 2;
                    CoordZ = 0;

                    //построение
                    swSketchManager.CreateLine(CoordXMove + CoordX - SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0, CoordXMove + CoordX + SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0); //1 (нижняя линия и идёт слева-вправо)
                    swSketchManager.CreateLine(CoordXMove + CoordX + SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0, CoordXMove + CoordX + SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0); //2
                    swSketchManager.CreateLine(CoordXMove + CoordX + SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0, CoordXMove + CoordX - SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0); //3
                    swSketchManager.CreateLine(CoordXMove + CoordX - SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0, CoordXMove + CoordX - SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0); //4

                }
                if (isRightCut)
                {
                    CoordX = 0 + SecondFigureInRadius + RadDiff / 2;
                    CoordZ = 0;

                    //построение
                    swSketchManager.CreateLine(CoordXMove + CoordX - SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0, CoordXMove + CoordX + SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0); //1 (нижняя линия и идёт слева-вправо)
                    swSketchManager.CreateLine(CoordXMove + CoordX + SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0, CoordXMove + CoordX + SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0); //2
                    swSketchManager.CreateLine(CoordXMove + CoordX + SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0, CoordXMove + CoordX - SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0); //3
                    swSketchManager.CreateLine(CoordXMove + CoordX - SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0, CoordXMove + CoordX - SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0); //4
                }
                if (isDownCut)
                {
                    CoordX = 0;
                    CoordZ = 0 - SecondFigureInRadius - RadDiff / 2;

                    //построение
                    swSketchManager.CreateLine(CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0, CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0); //1 (нижняя линия и идёт слева-вправо)
                    swSketchManager.CreateLine(CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0, CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0); //2
                    swSketchManager.CreateLine(CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0, CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0); //3
                    swSketchManager.CreateLine(CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0, CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0); //4
                }
                swModelDoc.ClearSelection2(true);

                SketchCounter++;

                swModelDoc.Extension.SelectByID2(sketch + SketchCounter, "SKETCH", 0, 0, 0, false, 0, null, 0);

                //вырез
                swFeatureMgr.FeatureCut4(true, false, false, 0, 0, SecondFigureHeight / 4, 0, false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);
            }

            swModelDoc.ClearSelection2(true);
            //вырез центрального кольца
            swModelDoc.Extension.SelectByRay(0, FirstFigureHeight + SecondFigureHeight, 0, 0, 1, 0, 0.01, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            SketchCounter++;

            swSketchManager.CreateCircleByRadius(CoordXMove, CoordZMove, 0, SecondFigureHoleRadius);

            swFeatureMgr.FeatureCut4(true, false, false, 1, 0, SecondFigureHeight / 2, 0, false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);

            swModelDoc.ClearSelection2(true);


            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, true);


            swModelDoc.ForceRebuild3(false);

        }

        private void FourthFigureBuildIntBtn_Click(object sender, EventArgs e)
        {
            SldWorks swApp = new SldWorks();
            ModelDoc2 swModelDoc = swApp.ActiveDoc;
            if (!CheckSketchManager(swModelDoc))
            {
                return; // Останавливаем выполнение
            }
            SketchManager swSketchManager = swModelDoc.SketchManager;
            SelectionMgr selectionMgr = swModelDoc.SelectionManager;

            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, false); //Отключение подтверждения размера

            FeatureManager swFeatureMgr = swModelDoc.FeatureManager;
            swModelDoc.SetUnits(0, 1, 0, 0, false); //0 - MM
                                                    //swSketchMgr.InsertSketch(true);



            //Язык
            string Line;
            string sketch;

            string TopPlane;
            string FrontPlane;
            string RightPlane;

            if (PlaneRusCb.Checked)
            {
                TopPlane = "Сверху";
                FrontPlane = "Спереди";
                RightPlane = "Справа";
            }
            else
            {
                TopPlane = "Top Plane";
                FrontPlane = "Front Plane";
                RightPlane = "Right Plane";
            }
            if (LinesRusCb.Checked)
            {
                sketch = "Эскиз";
                Line = "Линия";
            }
            else
            {
                sketch = "Sketch";
                Line = "Line";
            }

            ////THIRD FIGURE BUILD
            double ThirdFigureHeight = Convert.ToDouble(ThirdFigureHeightTb.Text) / 1000;
            double ThirdFigureWall = Convert.ToDouble(ThirdFigureWallTb.Text) / 1000;
            double ThirdFigureOutRadius = Convert.ToDouble(ThirdFigureOutRadiusTb.Text) / 1000;
            double ThirdFigureInRadius = Convert.ToDouble(ThirdFigureInRadiusTb.Text) / 1000;
            double ThirdFigureCircleDepth = Convert.ToDouble(ThirdFigureCircleDepthTb.Text) / 1000;
            double ThirdFigureCutDistance = Convert.ToDouble(ThirdFigureCutDistanceTb.Text) / 1000;
            double ThirdFigureWidthCut = Convert.ToDouble(ThirdFigureWidthCutTb.Text) / 1000;
            double ThirdFigureHeightCut = Convert.ToDouble(ThirdFigureHeightCutTb.Text) / 1000;


            double FirstFigureWidth = Convert.ToDouble(FirstFigureWidthTb.Text) / 1000;
            double FirstFigureLength = Convert.ToDouble(FirstFigureLengthTb.Text) / 1000;
            double FirstFigureHeight = Convert.ToDouble(FirstFigureHeightTb.Text) / 1000;
            double FirstPoint = -FirstFigureWidth / 2;
            double SecondPoint = -FirstFigureLength / 2;

            if (ThirdFigureCircleDepth < ThirdFigureWall)
            {
                MessageBox.Show("Длина кольца меньше чем Стенка фмгуры");
                return;
            }
            if (ThirdFigureOutRadius <= ThirdFigureInRadius)
            {
                MessageBox.Show("Наружный радиус Третей детали меньше внутреннего");
                return;
            }
            if (ThirdFigureHeightCut >= FirstFigureHeight)
            {
                MessageBox.Show("Высота выреза больше чем высота платформы первой детали");
                return;
            }
            if (ThirdFigureCutDistance <= 0)
            {
                MessageBox.Show("Расстояние между вырезами слишком малое");
                return;
            }
            if (ThirdFigureCutDistance / 2 + ThirdFigureWidthCut >= FirstFigureWidth / 2)
            {
                MessageBox.Show("Нижние вырезы заходят за пределы фигуры");
                return;
            }


            ////THIRD FIGURE BUILD

            swModelDoc.ShowNamedView2("*Back", 2);

            swModelDoc.ClearSelection2(true);

            swModelDoc.Extension.SelectByRay(FirstPoint + FirstFigureWidth / 2, FirstFigureHeight / 4, SecondPoint, 0, 0, 1, 0.01, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateLine(FirstPoint, 0, 0, FirstPoint + FirstFigureWidth, 0, 0); // osnova (left - right)
            swSketchManager.CreateLine(FirstPoint + FirstFigureWidth, 0, 0, FirstPoint + FirstFigureWidth, FirstFigureHeight, 0); // right
            swSketchManager.CreateLine(FirstPoint + FirstFigureWidth, FirstFigureHeight, 0, ThirdFigureOutRadius / 2, ThirdFigureHeight, 0); // right kriva9
            swSketchManager.CreateLine(ThirdFigureOutRadius / 2, ThirdFigureHeight, 0, -ThirdFigureOutRadius / 2, ThirdFigureHeight, 0); // up
            swSketchManager.CreateLine(-ThirdFigureOutRadius / 2, ThirdFigureHeight, 0, FirstPoint, FirstFigureHeight, 0); // left kriva9
            swSketchManager.CreateLine(FirstPoint, FirstFigureHeight, 0, FirstPoint, 0, 0); // left

            swSketchManager.InsertSketch(false);

            swModelDoc.ClearSelection2(true);

            SketchCounter++;

            swModelDoc.Extension.SelectByID2(sketch + SketchCounter, "SKETCH", 0, 0, 0, false, 0, null, 0);

            //building(ebashim drugimi slovami)

            swFeatureMgr.FeatureExtrusion2(true, false, false, 0, 0, ThirdFigureWall, 0, false, false, false, false, 0, 0, false, false, false, false, true, true, true, 0, 0, true);
            swModelDoc.ClearSelection2(true);

            swModelDoc.Extension.SelectByRay(FirstPoint + FirstFigureWidth / 2, ThirdFigureHeight / 2, SecondPoint - ThirdFigureWall, 0, 0, 1, 0.0001, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            SketchCounter++;

            swSketchManager.CreateCircleByRadius(0, ThirdFigureHeight, 0, ThirdFigureOutRadius);

            swFeatureMgr.FeatureCut4(true, false, false, 0, 0, ThirdFigureWall, 0, false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);

            swModelDoc.ClearSelection2(true);

            swModelDoc.Extension.SelectByRay(FirstPoint + FirstFigureWidth / 2, ThirdFigureHeight / 2, SecondPoint - ThirdFigureWall, 0, 0, 1, 0.0001, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateCircleByRadius(0, ThirdFigureHeight, 0, ThirdFigureOutRadius);

            swSketchManager.CreateCircleByRadius(0, ThirdFigureHeight, 0, ThirdFigureInRadius);

            swModelDoc.ClearSelection2(true);

            SketchCounter++;

            swSketchManager.InsertSketch(false);


            swModelDoc.Extension.SelectByID2(sketch + SketchCounter, "SKETCH", 0, 0, 0, false, 0, null, 0);

            swFeatureMgr.FeatureExtrusion2(false, false, false, 0, 0, ThirdFigureCircleDepth / 2 - ThirdFigureWall / 2, ThirdFigureCircleDepth / 2 + ThirdFigureWall / 2, false, false, false, false, 0, 0, false, false, false, false, true, true, true, 0, 0, true);

            swModelDoc.ClearSelection2(true);

            swModelDoc.ShowNamedView2("*Front", 1);

            swModelDoc.Extension.SelectByRay(0, FirstFigureHeight / 2, 0 + FirstFigureLength / 2, 0, 0, 1, 0.0001, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            //left one
            swSketchManager.CreateLine(-ThirdFigureCutDistance / 2 - ThirdFigureWidthCut, 0, 0, -ThirdFigureCutDistance / 2, 0, 0); // osnova left-right
            swSketchManager.CreateLine(-ThirdFigureCutDistance / 2, 0, 0, -ThirdFigureCutDistance / 2 - ThirdFigureWidthCut / 2, ThirdFigureHeightCut, 0);
            swSketchManager.CreateLine(-ThirdFigureCutDistance / 2 - ThirdFigureWidthCut / 2, ThirdFigureHeightCut, 0, -ThirdFigureCutDistance / 2 - ThirdFigureWidthCut, 0, 0);
            //rigt one
            swSketchManager.CreateLine(ThirdFigureCutDistance / 2 + ThirdFigureWidthCut, 0, 0, ThirdFigureCutDistance / 2, 0, 0); // osnova right-left
            swSketchManager.CreateLine(ThirdFigureCutDistance / 2, 0, 0, ThirdFigureCutDistance / 2 + ThirdFigureWidthCut / 2, ThirdFigureHeightCut, 0);
            swSketchManager.CreateLine(ThirdFigureCutDistance / 2 + ThirdFigureWidthCut / 2, ThirdFigureHeightCut, 0, ThirdFigureCutDistance / 2 + ThirdFigureWidthCut, 0, 0);

            swModelDoc.ClearSelection2(true);
            SketchCounter++;
            swModelDoc.Extension.SelectByID2(sketch + SketchCounter, "SKETCH", 0, 0, 0, false, 0, null, 0);
            swFeatureMgr.FeatureCut4(true, false, false, 1, 0, ThirdFigureWall, 0, false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);
            swModelDoc.ClearSelection2(true);






            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, true);

            swModelDoc.ForceRebuild3(false);
        }

        private void BuildAllBtn_Click(object sender, EventArgs e)
        {

            SldWorks swApp = new SldWorks();
            swApp.Visible = true;
            swApp.NewPart();


            ModelDoc2 swModelDoc = swApp.ActiveDoc;
            SketchManager swSketchManager = swModelDoc.SketchManager;
            SelectionMgr selectionMgr = swModelDoc.SelectionManager;

            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, false); //Отключение подтверждения размера


            FeatureManager swFeatureMgr = swModelDoc.FeatureManager;
            swModelDoc.SetUnits(0, 1, 0, 0, false);



            //Язык
            string Line;
            string sketch;

            string TopPlane;
            string FrontPlane;
            string RightPlane;

            if (PlaneRusCb.Checked)
            {
                TopPlane = "Сверху";
                FrontPlane = "Спереди";
                RightPlane = "Справа";
            }
            else
            {
                TopPlane = "Top Plane";
                FrontPlane = "Front Plane";
                RightPlane = "Right Plane";
            }
            if (LinesRusCb.Checked)
            {
                sketch = "Эскиз";
                Line = "Линия";
            }
            else
            {
                sketch = "Sketch";
                Line = "Line";
            }

            ////FIRST FIGURE BUILD
            swModelDoc.Extension.SelectByID2(TopPlane, "PLANE", 0, 0, 0, true, 0, null, 0);

            swSketchManager.InsertSketch(true);

            double FirstFigureWidth = 0.04;
            double FirstFigureLength = 0.04;
            double FirstFigureHeight = 0.010;
            double FirstFigureRadius = 0.002;
            double FirstFigureDistance = 0.003;
            double FirstPoint = -FirstFigureWidth / 2;
            double SecondPoint = -FirstFigureLength / 2;


            swSketchManager.CreateLine(FirstPoint, SecondPoint + FirstFigureLength, 0.05, FirstPoint + FirstFigureWidth, SecondPoint + FirstFigureLength, 0.05); //1 (верхняя линия и идёт вправо)
            swSketchManager.CreateLine(FirstPoint + FirstFigureWidth, SecondPoint + FirstFigureLength, 0, FirstPoint + FirstFigureWidth, SecondPoint, 0); //2
            swSketchManager.CreateLine(FirstPoint + FirstFigureWidth, SecondPoint, 0, FirstPoint, SecondPoint, 0); //3
            swSketchManager.CreateLine(FirstPoint, SecondPoint, 0, FirstPoint, SecondPoint + FirstFigureLength, 0); //4

            swModelDoc.ClearSelection2(true);

            swSketchManager.CreateCircleByRadius(FirstPoint + FirstFigureDistance, SecondPoint + FirstFigureLength - FirstFigureDistance, 0, FirstFigureRadius); //left-up

            swSketchManager.CreateCircle(FirstPoint + FirstFigureWidth - FirstFigureDistance, SecondPoint + FirstFigureLength - FirstFigureDistance, 0,
                FirstPoint + FirstFigureWidth - FirstFigureDistance - FirstFigureRadius, SecondPoint + FirstFigureLength - FirstFigureDistance, 0); //right-up

            swSketchManager.CreateCircle(FirstPoint + FirstFigureWidth - FirstFigureDistance, SecondPoint + FirstFigureDistance, 0,
                FirstPoint + FirstFigureWidth - FirstFigureDistance, SecondPoint + FirstFigureDistance + FirstFigureRadius, 0); //right-down

            swSketchManager.CreateCircle(FirstPoint + FirstFigureDistance, SecondPoint + FirstFigureDistance, 0, FirstPoint + FirstFigureDistance + FirstFigureRadius, SecondPoint + FirstFigureDistance, 0); //left-down

            swModelDoc.ClearSelection2(true);

            swModelDoc.Extension.SelectByID2(sketch + "1", "SKETCH", 0, 0, 0, false, 0, null, 0);

            // бобышка
            swFeatureMgr.FeatureExtrusion2(true, false, false, 0, 0, FirstFigureHeight, 0, false, false, false, false, 0, 0, false, false, false, false, true, true, true, 0, 0, true);
            swModelDoc.ClearSelection2(true);

            ////SECOND FIGURE BUILD
            double SecondFigureRadius = 0.015;
            double SecondFigureHeight = 0.005;
            double SecondFigureOutRadius = 0.01350;
            double SecondFigureInRadius = 0.01150;
            double SecondFigureDifference = 0.001;
            double SecondFigureHoleRadius = 0.010;
            double SecondFigureWidthCut = 0.002;
            double SecondFigureLengthCut = 0.006;
            double CoordXMove = 0.002;
            double CoordZMove = 0.004;
            bool isUpCut = true;
            bool isRightCut = true;
            bool isLeftCut = true;
            bool isDownCut = true;

            swModelDoc.ShowNamedView2("*Top", 5);

            //выделяем вверх
            swModelDoc.Extension.SelectByRay(0, FirstFigureHeight, 0, 0, 1, 0, 0.01, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateCircleByRadius(CoordXMove, CoordZMove, 0, SecondFigureRadius);

            swModelDoc.Extension.SelectByID2(sketch + "2", "SKETCH", 0, 0, 0, false, 0, null, 0);

            swFeatureMgr.FeatureExtrusion2(true, false, false, 0, 0, SecondFigureHeight, 0, false, false, false, false, 0, 0, false, false, false, false, true, true, true, 0, 0, true);

            swModelDoc.ClearSelection2(true);

            // выделяем еще раз вверх

            swModelDoc.Extension.SelectByRay(CoordXMove, FirstFigureHeight + SecondFigureHeight, CoordZMove, 0, 1, 0, 0.01, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateCircleByRadius(CoordXMove, CoordZMove, 0, SecondFigureOutRadius);

            swSketchManager.CreateCircleByRadius(CoordXMove, CoordZMove, 0, SecondFigureInRadius);

            swModelDoc.ClearSelection2(true);

            swSketchManager.InsertSketch(false);

            swModelDoc.Extension.SelectByID2(sketch + "3", "SKETCH", 0, 0, 0, false, 0, null, 0);

            swFeatureMgr.FeatureCut4(true, false, false, 0, 0, SecondFigureHeight / 2, 0 , false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);

            swModelDoc.ClearSelection2(true);

            // выделяем внутренее кольцо

            swModelDoc.Extension.SelectByRay(SecondFigureInRadius + (SecondFigureOutRadius - SecondFigureInRadius) / 2, FirstFigureHeight, CoordZMove, 0, 1, 0, 0.01, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateCircleByRadius(CoordXMove, CoordZMove, 0, SecondFigureOutRadius + SecondFigureDifference);

            swSketchManager.CreateCircleByRadius(CoordXMove, CoordZMove, 0, SecondFigureInRadius - SecondFigureDifference);

            swModelDoc.ClearSelection2(true);

            swSketchManager.InsertSketch(false);

            swModelDoc.Extension.SelectByID2(sketch + "4", "SKETCH", 0, 0, 0, false, 0, null, 0);

            swFeatureMgr.FeatureCut4(true, false, true, 0, 0, SecondFigureHeight / 4, 0, false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);

            swModelDoc.ClearSelection2(true);

            //4 хуйни
            swModelDoc.Extension.SelectByRay(0, FirstFigureHeight + SecondFigureHeight, 0, 0, 1, 0, 0.01, 2, false, 0, 0); //Тут собственно грань выбирается

            if (isUpCut || isLeftCut || isRightCut || isDownCut)
            {
                double RadDiff = SecondFigureOutRadius - SecondFigureInRadius;
                double CoordX;
                double CoordZ;
                swSketchManager.InsertSketch(true);
                if (isUpCut)
                {
                    CoordX = 0;
                    CoordZ = 0 + SecondFigureInRadius + RadDiff / 2;

                    //построение
                    swSketchManager.CreateLine(CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0, CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0); //1 (нижняя линия и идёт слева-вправо)
                    swSketchManager.CreateLine(CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0, CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0); //2
                    swSketchManager.CreateLine(CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0, CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0); //3
                    swSketchManager.CreateLine(CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0, CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0); //4

                }
                if (isLeftCut)
                {
                    CoordX = 0 - SecondFigureInRadius - RadDiff / 2;
                    CoordZ = 0;

                    //построение
                    swSketchManager.CreateLine(CoordXMove + CoordX- SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0, CoordXMove + CoordX+ SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0); //1 (нижняя линия и идёт слева-вправо)
                    swSketchManager.CreateLine(CoordXMove + CoordX+ SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0, CoordXMove + CoordX+ SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0); //2
                    swSketchManager.CreateLine(CoordXMove + CoordX+ SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0, CoordXMove + CoordX- SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0); //3
                    swSketchManager.CreateLine(CoordXMove + CoordX- SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0, CoordXMove + CoordX- SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0); //4

                }
                if (isRightCut)
                {
                    CoordX = 0 + SecondFigureInRadius + RadDiff / 2;
                    CoordZ = 0;

                    //построение
                    swSketchManager.CreateLine(CoordXMove + CoordX- SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0, CoordXMove + CoordX+ SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0); //1 (нижняя линия и идёт слева-вправо)
                    swSketchManager.CreateLine(CoordXMove + CoordX+ SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0, CoordXMove + CoordX+ SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0); //2
                    swSketchManager.CreateLine(CoordXMove + CoordX+ SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0, CoordXMove + CoordX- SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0); //3
                    swSketchManager.CreateLine(CoordXMove + CoordX- SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0, CoordXMove + CoordX- SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0); //4
                }
                if (isDownCut)
                {
                    CoordX = 0;
                    CoordZ = 0 - SecondFigureInRadius - RadDiff / 2;

                    //построение
                    swSketchManager.CreateLine(CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0, CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0); //1 (нижняя линия и идёт слева-вправо)
                    swSketchManager.CreateLine(CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0, CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0); //2
                    swSketchManager.CreateLine(CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0, CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0); //3
                    swSketchManager.CreateLine(CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0, CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0); //4
                }
                swModelDoc.ClearSelection2(true);
                swModelDoc.Extension.SelectByID2(sketch + "5", "SKETCH", 0, 0, 0, false, 0, null, 0);

                //вырез
                swFeatureMgr.FeatureCut4(true, false, false, 0, 0, SecondFigureHeight / 4, 0, false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);
            }

            swModelDoc.ClearSelection2(true);
            //вырез центрального кольца
            swModelDoc.Extension.SelectByRay(0, FirstFigureHeight + SecondFigureHeight, 0, 0, 1, 0, 0.01, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateCircleByRadius(CoordXMove, CoordZMove, 0, SecondFigureHoleRadius);

            swFeatureMgr.FeatureCut4(true, false, false, 1, 0, SecondFigureHeight / 2, 0, false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);

            swModelDoc.ClearSelection2(true);



            ////THIRD FIGURE BUILD
            double ThirdFigureHeight = 0.050;
            double ThirdFigureWall = 0.005;
            double ThirdFigureOutRadius = 0.005;
            double ThirdFigureInRadius = 0.003;
            double ThirdFigureCircleDepth = 0.006;
            double ThirdFigureWidthCut = 0.005;
            double ThirdFigureHeightCut = 0.005;
            double ThirdFigureCutDistance = 0.008;



            swModelDoc.ShowNamedView2("*Back", 2);

            swModelDoc.ClearSelection2(true);

            swModelDoc.Extension.SelectByRay(FirstPoint + FirstFigureWidth / 2 , FirstFigureHeight / 4, SecondPoint, 0, 0, 1, 0.01, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateLine(FirstPoint, 0, 0, FirstPoint + FirstFigureWidth, 0, 0); // osnova (left - right)
            swSketchManager.CreateLine(FirstPoint + FirstFigureWidth, 0, 0, FirstPoint + FirstFigureWidth, FirstFigureHeight, 0); // right
            swSketchManager.CreateLine(FirstPoint + FirstFigureWidth, FirstFigureHeight, 0, ThirdFigureOutRadius / 2, ThirdFigureHeight, 0); // right kriva9
            swSketchManager.CreateLine(ThirdFigureOutRadius / 2, ThirdFigureHeight, 0, -ThirdFigureOutRadius / 2, ThirdFigureHeight, 0); // up
            swSketchManager.CreateLine(-ThirdFigureOutRadius / 2, ThirdFigureHeight, 0, FirstPoint, FirstFigureHeight, 0); // left kriva9
            swSketchManager.CreateLine(FirstPoint, FirstFigureHeight, 0, FirstPoint, 0, 0); // left

            swSketchManager.InsertSketch(false);

            swModelDoc.ClearSelection2(true);

            swModelDoc.Extension.SelectByID2(sketch + "7", "SKETCH", 0, 0, 0, false, 0, null, 0);

            //building(ebashim drugimi slovami)

            swFeatureMgr.FeatureExtrusion2(true, false, false, 0, 0, ThirdFigureWall, 0, false, false, false, false, 0, 0, false, false, false, false, true, true, true, 0, 0, true);
            swModelDoc.ClearSelection2(true);

            swModelDoc.Extension.SelectByRay(FirstPoint + FirstFigureWidth / 2, ThirdFigureHeight / 2, SecondPoint - ThirdFigureWall, 0, 0, 1, 0.0001, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateCircleByRadius(0, ThirdFigureHeight, 0, ThirdFigureOutRadius);

            swFeatureMgr.FeatureCut4(true, false, false, 0, 0, ThirdFigureWall, 0, false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);

            swModelDoc.ClearSelection2(true);

            swModelDoc.Extension.SelectByRay(FirstPoint + FirstFigureWidth / 2, ThirdFigureHeight / 2, SecondPoint - ThirdFigureWall, 0, 0, 1, 0.0001, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateCircleByRadius(0, ThirdFigureHeight, 0, ThirdFigureOutRadius);

            swSketchManager.CreateCircleByRadius(0, ThirdFigureHeight, 0, ThirdFigureInRadius);

            swModelDoc.ClearSelection2(true);

            swSketchManager.InsertSketch(false);

            swModelDoc.Extension.SelectByID2(sketch + "9", "SKETCH", 0, 0, 0, false, 0, null, 0);

            swFeatureMgr.FeatureExtrusion2(false, false, false, 0, 0, ThirdFigureCircleDepth / 2 - ThirdFigureWall / 2, ThirdFigureCircleDepth / 2 + ThirdFigureWall / 2, false, false, false, false, 0, 0, false, false, false, false, true, true, true, 0, 0, true);

            swModelDoc.ClearSelection2(true);

            swModelDoc.ShowNamedView2("*Front", 1);

            swModelDoc.Extension.SelectByRay(0, FirstFigureHeight / 2, 0 + FirstFigureLength / 2, 0, 0, 1, 0.0001, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            //left one
            swSketchManager.CreateLine(-ThirdFigureCutDistance / 2 - ThirdFigureWidthCut, 0, 0, -ThirdFigureCutDistance / 2, 0, 0); // osnova left-right
            swSketchManager.CreateLine(-ThirdFigureCutDistance / 2, 0, 0, -ThirdFigureCutDistance / 2 - ThirdFigureWidthCut / 2, ThirdFigureHeightCut, 0);
            swSketchManager.CreateLine(-ThirdFigureCutDistance / 2 - ThirdFigureWidthCut / 2, ThirdFigureHeightCut, 0, -ThirdFigureCutDistance / 2 - ThirdFigureWidthCut, 0, 0);
            //rigt one
            swSketchManager.CreateLine(ThirdFigureCutDistance / 2 + ThirdFigureWidthCut, 0, 0, ThirdFigureCutDistance / 2, 0, 0); // osnova right-left
            swSketchManager.CreateLine(ThirdFigureCutDistance / 2, 0, 0, ThirdFigureCutDistance / 2 + ThirdFigureWidthCut / 2, ThirdFigureHeightCut, 0);
            swSketchManager.CreateLine(ThirdFigureCutDistance / 2 + ThirdFigureWidthCut / 2, ThirdFigureHeightCut, 0, ThirdFigureCutDistance / 2 + ThirdFigureWidthCut, 0, 0);

            swModelDoc.ClearSelection2(true);
            swModelDoc.Extension.SelectByID2(sketch + "10", "SKETCH", 0, 0, 0, false, 0, null, 0);
            swFeatureMgr.FeatureCut4(true, false, false, 1, 0, ThirdFigureWall, 0, false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);
            swModelDoc.ClearSelection2(true);
            // Конец

            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, true);


            swModelDoc.ForceRebuild3(false);
        }

        private void FirstStepBuildBtn_Click(object sender, EventArgs e)
        {
            SldWorks swApp = new SldWorks();
            swApp.Visible = true;
            swApp.NewPart();

            ModelDoc2 swModelDoc = swApp.ActiveDoc;
            SketchManager swSketchManager = swModelDoc.SketchManager;
            SelectionMgr selectionMgr = swModelDoc.SelectionManager;

            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, false); //Отключение подтверждения размера


            FeatureManager swFeatureMgr = swModelDoc.FeatureManager;
            swModelDoc.SetUnits(0, 1, 0, 0, false);

            //Язык
            string Line;
            string sketch;

            string TopPlane;
            string FrontPlane;
            string RightPlane;

            if (PlaneRusCb.Checked)
            {
                TopPlane = "Сверху";
                FrontPlane = "Спереди";
                RightPlane = "Справа";
            }
            else
            {
                TopPlane = "Top Plane";
                FrontPlane = "Front Plane";
                RightPlane = "Right Plane";
            }
            if (LinesRusCb.Checked)
            {
                sketch = "Эскиз";
                Line = "Линия";
            }
            else
            {
                sketch = "Sketch";
                Line = "Line";
            }


            ////FIRST FIGURE BUILD
            swModelDoc.Extension.SelectByID2(TopPlane, "PLANE", 0, 0, 0, true, 0, null, 0);

            swSketchManager.InsertSketch(true);

            double FirstFigureWidth = 0.04;
            double FirstFigureLength = 0.04;
            double FirstFigureHeight = 0.010;
            double FirstFigureRadius = 0.002;
            double FirstFigureDistance = 0.003;
            double FirstPoint = -FirstFigureWidth / 2;
            double SecondPoint = -FirstFigureLength / 2;


            swSketchManager.CreateLine(FirstPoint, SecondPoint + FirstFigureLength, 0.05, FirstPoint + FirstFigureWidth, SecondPoint + FirstFigureLength, 0.05); //1 (верхняя линия и идёт вправо)
            swSketchManager.CreateLine(FirstPoint + FirstFigureWidth, SecondPoint + FirstFigureLength, 0, FirstPoint + FirstFigureWidth, SecondPoint, 0); //2
            swSketchManager.CreateLine(FirstPoint + FirstFigureWidth, SecondPoint, 0, FirstPoint, SecondPoint, 0); //3
            swSketchManager.CreateLine(FirstPoint, SecondPoint, 0, FirstPoint, SecondPoint + FirstFigureLength, 0); //4

            swModelDoc.ClearSelection2(true);

            swSketchManager.CreateCircleByRadius(FirstPoint + FirstFigureDistance, SecondPoint + FirstFigureLength - FirstFigureDistance, 0, FirstFigureRadius); //left-up

            swSketchManager.CreateCircle(FirstPoint + FirstFigureWidth - FirstFigureDistance, SecondPoint + FirstFigureLength - FirstFigureDistance, 0,
                FirstPoint + FirstFigureWidth - FirstFigureDistance - FirstFigureRadius, SecondPoint + FirstFigureLength - FirstFigureDistance, 0); //right-up

            swSketchManager.CreateCircle(FirstPoint + FirstFigureWidth - FirstFigureDistance, SecondPoint + FirstFigureDistance, 0,
                FirstPoint + FirstFigureWidth - FirstFigureDistance, SecondPoint + FirstFigureDistance + FirstFigureRadius, 0); //right-down

            swSketchManager.CreateCircle(FirstPoint + FirstFigureDistance, SecondPoint + FirstFigureDistance, 0, FirstPoint + FirstFigureDistance + FirstFigureRadius, SecondPoint + FirstFigureDistance, 0); //left-down

            swModelDoc.ClearSelection2(true);

            swModelDoc.Extension.SelectByID2(sketch + "1", "SKETCH", 0, 0, 0, false, 0, null, 0);

            // бобышка
            swFeatureMgr.FeatureExtrusion2(true, false, false, 0, 0, FirstFigureHeight, 0, false, false, false, false, 0, 0, false, false, false, false, true, true, true, 0, 0, true);
            swModelDoc.ClearSelection2(true);





            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, true);


            swModelDoc.ForceRebuild3(false);
        }

        private void SecondStepBuildBtn_Click(object sender, EventArgs e)
        {
            SldWorks swApp = new SldWorks();
            ModelDoc2 swModelDoc = swApp.ActiveDoc;
            if (!CheckSketchManager(swModelDoc))
            {
                return; // Останавливаем выполнение
            }
            SketchManager swSketchManager = swModelDoc.SketchManager;
            SelectionMgr selectionMgr = swModelDoc.SelectionManager;

            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, false); //Отключение подтверждения размера

            FeatureManager swFeatureMgr = swModelDoc.FeatureManager;
            swModelDoc.SetUnits(0, 1, 0, 0, false); //0 - MM
                                                    //swSketchMgr.InsertSketch(true);

            //Язык
            string Line;
            string sketch;

            string TopPlane;
            string FrontPlane;
            string RightPlane;

            if (PlaneRusCb.Checked)
            {
                TopPlane = "Сверху";
                FrontPlane = "Спереди";
                RightPlane = "Справа";
            }
            else
            {
                TopPlane = "Top Plane";
                FrontPlane = "Front Plane";
                RightPlane = "Right Plane";
            }
            if (LinesRusCb.Checked)
            {
                sketch = "Эскиз";
                Line = "Линия";
            }
            else
            {
                sketch = "Sketch";
                Line = "Line";
            }


            ////SECOND FIGURE BUILD
            double SecondFigureRadius = 0.015;
            double SecondFigureHeight = 0.005;
            double SecondFigureOutRadius = 0.01350;
            double SecondFigureInRadius = 0.01150;
            double SecondFigureDifference = 0.001;
            double SecondFigureHoleRadius = 0.010;
            double SecondFigureWidthCut = 0.002;
            double SecondFigureLengthCut = 0.006;
            double CoordXMove = 0.002;
            double CoordZMove = 0.004;
            bool isUpCut = true;
            bool isRightCut = true;
            bool isLeftCut = true;
            bool isDownCut = true;

            double FirstFigureWidth = 0.04;
            double FirstFigureLength = 0.04;
            double FirstFigureHeight = 0.010;
            double FirstPoint = -FirstFigureWidth / 2;
            double SecondPoint = -FirstFigureLength / 2;

            swModelDoc.ShowNamedView2("*Top", 5);

            //выделяем вверх
            swModelDoc.Extension.SelectByRay(0, FirstFigureHeight, 0, 0, 1, 0, 0.01, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateCircleByRadius(CoordXMove, CoordZMove, 0, SecondFigureRadius);

            swModelDoc.Extension.SelectByID2(sketch + "2", "SKETCH", 0, 0, 0, false, 0, null, 0);

            swFeatureMgr.FeatureExtrusion2(true, false, false, 0, 0, SecondFigureHeight, 0, false, false, false, false, 0, 0, false, false, false, false, true, true, true, 0, 0, true);

            swModelDoc.ClearSelection2(true);

            // выделяем еще раз вверх

            swModelDoc.Extension.SelectByRay(CoordXMove, FirstFigureHeight + SecondFigureHeight, CoordZMove, 0, 1, 0, 0.01, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateCircleByRadius(CoordXMove, CoordZMove, 0, SecondFigureOutRadius);

            swSketchManager.CreateCircleByRadius(CoordXMove, CoordZMove, 0, SecondFigureInRadius);

            swModelDoc.ClearSelection2(true);

            swSketchManager.InsertSketch(false);

            swModelDoc.Extension.SelectByID2(sketch + "3", "SKETCH", 0, 0, 0, false, 0, null, 0);

            swFeatureMgr.FeatureCut4(true, false, false, 0, 0, SecondFigureHeight / 2, 0, false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);

            swModelDoc.ClearSelection2(true);

            // выделяем внутренее кольцо

            swModelDoc.Extension.SelectByRay(SecondFigureInRadius + (SecondFigureOutRadius - SecondFigureInRadius) / 2, FirstFigureHeight, CoordZMove, 0, 1, 0, 0.01, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateCircleByRadius(CoordXMove, CoordZMove, 0, SecondFigureOutRadius + SecondFigureDifference);

            swSketchManager.CreateCircleByRadius(CoordXMove, CoordZMove, 0, SecondFigureInRadius - SecondFigureDifference);

            swModelDoc.ClearSelection2(true);

            swSketchManager.InsertSketch(false);

            swModelDoc.Extension.SelectByID2(sketch + "4", "SKETCH", 0, 0, 0, false, 0, null, 0);

            swFeatureMgr.FeatureCut4(true, false, true, 0, 0, SecondFigureHeight / 4, 0, false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);

            swModelDoc.ClearSelection2(true);

            //4 хуйни
            swModelDoc.Extension.SelectByRay(0, FirstFigureHeight + SecondFigureHeight, 0, 0, 1, 0, 0.01, 2, false, 0, 0); //Тут собственно грань выбирается

            if (isUpCut || isLeftCut || isRightCut || isDownCut)
            {
                double RadDiff = SecondFigureOutRadius - SecondFigureInRadius;
                double CoordX;
                double CoordZ;
                swSketchManager.InsertSketch(true);
                if (isUpCut)
                {
                    CoordX = 0;
                    CoordZ = 0 + SecondFigureInRadius + RadDiff / 2;

                    //построение
                    swSketchManager.CreateLine(CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0, CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0); //1 (нижняя линия и идёт слева-вправо)
                    swSketchManager.CreateLine(CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0, CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0); //2
                    swSketchManager.CreateLine(CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0, CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0); //3
                    swSketchManager.CreateLine(CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0, CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0); //4

                }
                if (isLeftCut)
                {
                    CoordX = 0 - SecondFigureInRadius - RadDiff / 2;
                    CoordZ = 0;

                    //построение
                    swSketchManager.CreateLine(CoordXMove + CoordX - SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0, CoordXMove + CoordX + SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0); //1 (нижняя линия и идёт слева-вправо)
                    swSketchManager.CreateLine(CoordXMove + CoordX + SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0, CoordXMove + CoordX + SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0); //2
                    swSketchManager.CreateLine(CoordXMove + CoordX + SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0, CoordXMove + CoordX - SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0); //3
                    swSketchManager.CreateLine(CoordXMove + CoordX - SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0, CoordXMove + CoordX - SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0); //4

                }
                if (isRightCut)
                {
                    CoordX = 0 + SecondFigureInRadius + RadDiff / 2;
                    CoordZ = 0;

                    //построение
                    swSketchManager.CreateLine(CoordXMove + CoordX - SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0, CoordXMove + CoordX + SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0); //1 (нижняя линия и идёт слева-вправо)
                    swSketchManager.CreateLine(CoordXMove + CoordX + SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0, CoordXMove + CoordX + SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0); //2
                    swSketchManager.CreateLine(CoordXMove + CoordX + SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0, CoordXMove + CoordX - SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0); //3
                    swSketchManager.CreateLine(CoordXMove + CoordX - SecondFigureLengthCut / 2, CoordZMove + CoordZ + SecondFigureWidthCut / 2, 0, CoordXMove + CoordX - SecondFigureLengthCut / 2, CoordZMove + CoordZ - SecondFigureWidthCut / 2, 0); //4
                }
                if (isDownCut)
                {
                    CoordX = 0;
                    CoordZ = 0 - SecondFigureInRadius - RadDiff / 2;

                    //построение
                    swSketchManager.CreateLine(CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0, CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0); //1 (нижняя линия и идёт слева-вправо)
                    swSketchManager.CreateLine(CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0, CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0); //2
                    swSketchManager.CreateLine(CoordXMove + CoordX + SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0, CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0); //3
                    swSketchManager.CreateLine(CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ + SecondFigureLengthCut / 2, 0, CoordXMove + CoordX - SecondFigureWidthCut / 2, CoordZMove + CoordZ - SecondFigureLengthCut / 2, 0); //4
                }
                swModelDoc.ClearSelection2(true);
                swModelDoc.Extension.SelectByID2(sketch + "5", "SKETCH", 0, 0, 0, false, 0, null, 0);

                //вырез
                swFeatureMgr.FeatureCut4(true, false, false, 0, 0, SecondFigureHeight / 4, 0, false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);
            }

            swModelDoc.ClearSelection2(true);
            //вырез центрального кольца
            swModelDoc.Extension.SelectByRay(0, FirstFigureHeight + SecondFigureHeight, 0, 0, 1, 0, 0.01, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateCircleByRadius(CoordXMove, CoordZMove, 0, SecondFigureHoleRadius);

            swFeatureMgr.FeatureCut4(true, false, false, 1, 0, SecondFigureHeight / 2, 0, false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);

            swModelDoc.ClearSelection2(true);



            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, true);


            swModelDoc.ForceRebuild3(false);
        }

        private void ThirdStepBuildBtn_Click(object sender, EventArgs e)
        {
            SldWorks swApp = new SldWorks();
            ModelDoc2 swModelDoc = swApp.ActiveDoc;
            if (!CheckSketchManager(swModelDoc))
            {
                return; // Останавливаем выполнение
            }
            SketchManager swSketchManager = swModelDoc.SketchManager;
            SelectionMgr selectionMgr = swModelDoc.SelectionManager;

            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, false); //Отключение подтверждения размера

            FeatureManager swFeatureMgr = swModelDoc.FeatureManager;
            swModelDoc.SetUnits(0, 1, 0, 0, false); //0 - MM
                                                    //swSketchMgr.InsertSketch(true);

            //Язык
            string Line;
            string sketch;

            string TopPlane;
            string FrontPlane;
            string RightPlane;

            if (PlaneRusCb.Checked)
            {
                TopPlane = "Сверху";
                FrontPlane = "Спереди";
                RightPlane = "Справа";
            }
            else
            {
                TopPlane = "Top Plane";
                FrontPlane = "Front Plane";
                RightPlane = "Right Plane";
            }
            if (LinesRusCb.Checked)
            {
                sketch = "Эскиз";
                Line = "Линия";
            }
            else
            {
                sketch = "Sketch";
                Line = "Line";
            }


            //THIRD FIGURE BUILD

            swModelDoc.Extension.SelectByID2(TopPlane, "PLANE", 0, 0, 0, true, 0, null, 0);

            swSketchManager.InsertSketch(true);

            double FirstFigureWidth = 0.070;
            double FirstFigureLength = 0.045;
            double FirstFigureHeight = 0.010;
            double FirstFigure2Lengths = 0.014;
            double FirstPoint = -FirstFigureWidth / 2;
            double SecondPoint = -FirstFigureLength / 2;

            double ThirdFigureHeight = 0.02;
            double ThirdFigureInRadius = 0.004;
            double ThirdFigureOutRadius = 0.01;

            //Начальный круг для выреза
            swSketchManager.CreateCircle(FirstPoint + FirstFigure2Lengths / 2, SecondPoint, 0, FirstPoint + FirstFigure2Lengths / 2, SecondPoint + ThirdFigureOutRadius + 0.0001, 0);

            swFeatureMgr.FeatureCut4(false, false, false, 9, 9, FirstFigureHeight / 2, FirstFigureHeight / 2, false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);

            swModelDoc.ClearSelection2(true);

            // Эскиз под саму деталь

            swModelDoc.Extension.SelectByID2(TopPlane, "PLANE", 0, 0, 0, true, 0, null, 0);

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateCircle(FirstPoint + FirstFigure2Lengths / 2, SecondPoint, 0, FirstPoint + FirstFigure2Lengths / 2, SecondPoint + ThirdFigureOutRadius, 0);

            swSketchManager.CreateCircle(FirstPoint + FirstFigure2Lengths / 2, SecondPoint, 0, FirstPoint + FirstFigure2Lengths / 2, SecondPoint + ThirdFigureInRadius, 0);

            swModelDoc.ClearSelection2(true);

            swModelDoc.Extension.SelectByID2(sketch + "5", "SKETCH", 0, 0, 0, false, 0, null, 0);

            swFeatureMgr.FeatureExtrusion2(true, false, false, 6, 0, ThirdFigureHeight, 0, false, false, false, false, 0, 0, false, false, false, false, true, true, true, 0, 0, true);

            swModelDoc.ClearSelection2(true);



            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, true);


            swModelDoc.ForceRebuild3(false);
        }

        private void FourthBuildStepBtn_Click(object sender, EventArgs e)
        {
            SldWorks swApp = new SldWorks();
            ModelDoc2 swModelDoc = swApp.ActiveDoc;
            if (!CheckSketchManager(swModelDoc))
            {
                return; // Останавливаем выполнение
            }
            SketchManager swSketchManager = swModelDoc.SketchManager;
            SelectionMgr selectionMgr = swModelDoc.SelectionManager;

            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, false); //Отключение подтверждения размера

            FeatureManager swFeatureMgr = swModelDoc.FeatureManager;
            swModelDoc.SetUnits(0, 1, 0, 0, false); //0 - MM
                                                    //swSketchMgr.InsertSketch(true);


            //Язык
            string Line;
            string sketch;

            string TopPlane;
            string FrontPlane;
            string RightPlane;

            if (PlaneRusCb.Checked)
            {
                TopPlane = "Сверху";
                FrontPlane = "Спереди";
                RightPlane = "Справа";
            }
            else
            {
                TopPlane = "Top Plane";
                FrontPlane = "Front Plane";
                RightPlane = "Right Plane";
            }
            if (LinesRusCb.Checked)
            {
                sketch = "Эскиз";
                Line = "Линия";
            }
            else
            {
                sketch = "Sketch";
                Line = "Line";
            }



            ////THIRD FIGURE BUILD
            double ThirdFigureHeight = 0.050;
            double ThirdFigureWall = 0.005;
            double ThirdFigureOutRadius = 0.005;
            double ThirdFigureInRadius = 0.003;
            double ThirdFigureCircleDepth = 0.006;
            double ThirdFigureWidthCut = 0.005;
            double ThirdFigureHeightCut = 0.005;
            double ThirdFigureCutDistance = 0.008;

            double FirstFigureWidth = 0.04;
            double FirstFigureLength = 0.04;
            double FirstFigureHeight = 0.010;
            double FirstPoint = -FirstFigureWidth / 2;
            double SecondPoint = -FirstFigureLength / 2;




            ////THIRD FIGURE BUILD

            swModelDoc.ShowNamedView2("*Back", 2);

            swModelDoc.ClearSelection2(true);

            swModelDoc.Extension.SelectByRay(FirstPoint + FirstFigureWidth / 2, FirstFigureHeight / 4, SecondPoint, 0, 0, 1, 0.01, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateLine(FirstPoint, 0, 0, FirstPoint + FirstFigureWidth, 0, 0); // osnova (left - right)
            swSketchManager.CreateLine(FirstPoint + FirstFigureWidth, 0, 0, FirstPoint + FirstFigureWidth, FirstFigureHeight, 0); // right
            swSketchManager.CreateLine(FirstPoint + FirstFigureWidth, FirstFigureHeight, 0, ThirdFigureOutRadius / 2, ThirdFigureHeight, 0); // right kriva9
            swSketchManager.CreateLine(ThirdFigureOutRadius / 2, ThirdFigureHeight, 0, -ThirdFigureOutRadius / 2, ThirdFigureHeight, 0); // up
            swSketchManager.CreateLine(-ThirdFigureOutRadius / 2, ThirdFigureHeight, 0, FirstPoint, FirstFigureHeight, 0); // left kriva9
            swSketchManager.CreateLine(FirstPoint, FirstFigureHeight, 0, FirstPoint, 0, 0); // left

            swSketchManager.InsertSketch(false);

            swModelDoc.ClearSelection2(true);

            swModelDoc.Extension.SelectByID2(sketch + "7", "SKETCH", 0, 0, 0, false, 0, null, 0);

            //building(ebashim drugimi slovami)

            swFeatureMgr.FeatureExtrusion2(true, false, false, 0, 0, ThirdFigureWall, 0, false, false, false, false, 0, 0, false, false, false, false, true, true, true, 0, 0, true);
            swModelDoc.ClearSelection2(true);

            swModelDoc.Extension.SelectByRay(FirstPoint + FirstFigureWidth / 2, ThirdFigureHeight / 2, SecondPoint - ThirdFigureWall, 0, 0, 1, 0.0001, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateCircleByRadius(0, ThirdFigureHeight, 0, ThirdFigureOutRadius);

            swFeatureMgr.FeatureCut4(true, false, false, 0, 0, ThirdFigureWall, 0, false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);

            swModelDoc.ClearSelection2(true);

            swModelDoc.Extension.SelectByRay(FirstPoint + FirstFigureWidth / 2, ThirdFigureHeight / 2, SecondPoint - ThirdFigureWall, 0, 0, 1, 0.0001, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            swSketchManager.CreateCircleByRadius(0, ThirdFigureHeight, 0, ThirdFigureOutRadius);

            swSketchManager.CreateCircleByRadius(0, ThirdFigureHeight, 0, ThirdFigureInRadius);

            swModelDoc.ClearSelection2(true);

            swSketchManager.InsertSketch(false);

            swModelDoc.Extension.SelectByID2(sketch + "9", "SKETCH", 0, 0, 0, false, 0, null, 0);

            swFeatureMgr.FeatureExtrusion2(false, false, false, 0, 0, ThirdFigureCircleDepth / 2 - ThirdFigureWall / 2, ThirdFigureCircleDepth / 2 + ThirdFigureWall / 2, false, false, false, false, 0, 0, false, false, false, false, true, true, true, 0, 0, true);

            swModelDoc.ClearSelection2(true);

            swModelDoc.ShowNamedView2("*Front", 1);

            swModelDoc.Extension.SelectByRay(0, FirstFigureHeight / 2, 0 + FirstFigureLength / 2, 0, 0, 1, 0.0001, 2, false, 0, 0); //Тут собственно грань выбирается

            swSketchManager.InsertSketch(true);

            //left one
            swSketchManager.CreateLine(-ThirdFigureCutDistance / 2 - ThirdFigureWidthCut, 0, 0, -ThirdFigureCutDistance / 2, 0, 0); // osnova left-right
            swSketchManager.CreateLine(-ThirdFigureCutDistance / 2, 0, 0, -ThirdFigureCutDistance / 2 - ThirdFigureWidthCut / 2, ThirdFigureHeightCut, 0);
            swSketchManager.CreateLine(-ThirdFigureCutDistance / 2 - ThirdFigureWidthCut / 2, ThirdFigureHeightCut, 0, -ThirdFigureCutDistance / 2 - ThirdFigureWidthCut, 0, 0);
            //rigt one
            swSketchManager.CreateLine(ThirdFigureCutDistance / 2 + ThirdFigureWidthCut, 0, 0, ThirdFigureCutDistance / 2, 0, 0); // osnova right-left
            swSketchManager.CreateLine(ThirdFigureCutDistance / 2, 0, 0, ThirdFigureCutDistance / 2 + ThirdFigureWidthCut / 2, ThirdFigureHeightCut, 0);
            swSketchManager.CreateLine(ThirdFigureCutDistance / 2 + ThirdFigureWidthCut / 2, ThirdFigureHeightCut, 0, ThirdFigureCutDistance / 2 + ThirdFigureWidthCut, 0, 0);

            swModelDoc.ClearSelection2(true);
            swModelDoc.Extension.SelectByID2(sketch + "10", "SKETCH", 0, 0, 0, false, 0, null, 0);
            swFeatureMgr.FeatureCut4(true, false, false, 1, 0, ThirdFigureWall, 0, false, false, false, false, 0, 0, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);
            swModelDoc.ClearSelection2(true);



            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, true);


            swModelDoc.ForceRebuild3(false);
        }

        public bool CheckSketchManager(ModelDoc2 swModelDoc)
        {
            try
            {
                // Проверяем, что swModelDoc не равен null
                if (swModelDoc == null)
                {
                    System.Windows.Forms.MessageBox.Show("swModelDoc равен null. Проверьте, открыт ли документ.");
                    return false; // Проверка не пройдена
                }

                // Проверяем, что SketchManager не равен null
                SketchManager swSketchManager = swModelDoc.SketchManager;
                if (swSketchManager == null)
                {
                    System.Windows.Forms.MessageBox.Show("SketchManager равен null. Проверьте активный документ.");
                    return false; // Проверка не пройдена
                }

                // Если всё в порядке
                return true; // Проверка пройдена
            }
            catch (Exception ex)
            {
                // Ловим любые исключения
                System.Windows.Forms.MessageBox.Show($"Ошибка: {ex.Message}");
                return false; // Проверка не пройдена
            }
        }
    }
}
