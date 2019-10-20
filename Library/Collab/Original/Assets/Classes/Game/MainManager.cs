using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
using Classes.Game.GenerationsPropertiesTableSpace;
using Classes.Game.PointsManagerSpace;
using Classes.GameClasses.FuncManegerSpace;
using Classes.GameClasses.PointSpace;
using Classes.GameClasses.PropertiesSpace;

namespace Classes.Game.MainManagerSpace
{

    public class MainManager
    {
        //Менеджеры
        private PointsManager pointsManager;
        private GenerationsPropertiesTable gpt;
        private FunctionManager functionManager;
        private PointOperator pointsOperator;
        private List<PointsManager> areasManager;
        //Переменные
        private int age;
        private byte numberOfAreasLevels;

        //Кноструктор
        public MainManager(int sizeX, int sizeY, byte numOfLevels, int[] positions, byte numberOfAreasLev)
        {
            pointsManager = new PointsManager(sizeX, sizeY);
            gpt = new GenerationsPropertiesTable();
            functionManager = new FunctionManager();
            pointsOperator = new PointOperator(numOfLevels, positions);
            numberOfAreasLevels = numberOfAreasLev;
            if (numberOfAreasLevels > 0)
            {
                areasManager = new List<PointsManager>();
                for (int i = 0; i < numberOfAreasLevels; i++)
                    areasManager.Add(new PointsManager(sizeX, sizeY));
            }
            age = 0;
        }

        //Методы
        public void step()
        {
            pointsOperator.virtualize(pointsManager);
            pointsOperator.impactPoints(pointsManager);
            if (numberOfAreasLevels > 0)
            {
                for (int i = 0; i < numberOfAreasLevels; i++)
                {
                    pointsOperator.impactAreasToPoints(pointsManager, areasManager[i]);
                    pointsOperator.virtualize(areasManager[i]);
                    pointsOperator.impactPoints(areasManager[i]);
                    pointsOperator.decider(areasManager[i]);
                    pointsOperator.cleaner(areasManager[i]);
                }
            }
            pointsOperator.decider(pointsManager);
            pointsOperator.cleaner(pointsManager);
            age++;
        }


        public void initRules(int[] numberOfProperties, bool[] importance, string[] names, string[] descriptions,
           float[][] aliveInter, float[][] deadInter, float[] onePointPowerStat, float[] livePStat,
           float[] livePDyn, float[] setP, string[][] collectionsNam,
           int numberOfGens, int[][] propersNumbers, bool[] immort,
           int numberOfFuncs, int[][] propNumbers, float[][] coefFr, float[][] coefEn, int[] funcToProp)
        {
            if (numberOfProperties[0] == numberOfProperties[1] + numberOfProperties[2] + numberOfProperties[3])
            {
                for (int i = 0; i < numberOfProperties[1]; i++)
                {
                    float[][] alive = new float[aliveInter[i].Length / 2][];
                    for (int j = 0; j < alive.Length; j++)
                    {
                        alive[j] = new float[2];
                        alive[j][0] = aliveInter[i][j * 2];
                        alive[j][1] = aliveInter[i][j * 2 + 1];
                    }
                    float[][] dead = new float[deadInter[i].Length / 2][];
                    for (int j = 0; j < dead.Length; j++)
                    {
                        dead[j] = new float[2];
                        dead[j][0] = deadInter[i][j * 2];
                        dead[j][1] = deadInter[i][j * 2 + 1];
                    }
                    gpt.createStaticalProperty(names[i], descriptions[i], alive, dead, onePointPowerStat[i], livePStat[i], importance[i]);
                }
                int index = numberOfProperties[1];
                for (int i = 0; i < numberOfProperties[2]; i++)
                {
                    gpt.createDynamicalProperty(names[index], descriptions[index], livePDyn[i], setP[i], importance[index]);
                    index++;
                }
                for (int i = 0; i < numberOfProperties[3]; i++)
                {
                    gpt.createCollectionalProperty(names[index], descriptions[index], collectionsNam[i], importance[index]);
                    index++;
                }
            }
            for (int i = 0; i < numberOfGens; i++)
            {
                gpt.createGeneration(propersNumbers[i], immort[i]);
            }
            for (int i = 0; i < numberOfFuncs; i++)
            {
                Property[] prop = new Property[propNumbers[i].Length];
                for (int j = 0; j < prop.Length; j++)
                {
                    prop[j] = gpt.getProperty(propNumbers[i][j]);
                }
                functionManager.createFunction(prop, coefFr[i], coefEn[i]);
                functionManager.addFunctionToProperty(gpt.getProperty(funcToProp[i]), i);
            }
        }

        public void addProperties(int[] numberOfProperties, bool[] importance, string[] names, string[] descriptions,
           float[][] aliveInter, float[][] deadInter, float[] onePointPowerStat, float[] livePStat,
           float[] livePDyn, float[] setP, string[][] collectionsNam)
        {
            if (numberOfProperties[0] == numberOfProperties[1] + numberOfProperties[2] + numberOfProperties[3])
            {
                for (int i = 0; i < numberOfProperties[1]; i++)
                {
                    float[][] alive = new float[aliveInter[i].Length / 2][];
                    for (int j = 0; j < alive.Length; j++)
                    {
                        alive[j] = new float[2];
                        alive[j][0] = aliveInter[i][j * 2];
                        alive[j][1] = aliveInter[i][j * 2 + 1];
                    }
                    float[][] dead = new float[deadInter[i].Length / 2][];
                    for (int j = 0; j < dead.Length; j++)
                    {
                        dead[j] = new float[2];
                        dead[j][0] = deadInter[i][j * 2];
                        dead[j][1] = deadInter[i][j * 2 + 1];
                    }
                    gpt.createStaticalProperty(names[i], descriptions[i], alive, dead, onePointPowerStat[i], livePStat[i], importance[i]);
                }
                int index = numberOfProperties[1];
                for (int i = 0; i < numberOfProperties[2]; i++)
                {
                    gpt.createDynamicalProperty(names[index], descriptions[index], livePDyn[i], setP[i], importance[index]);
                    index++;
                }
                for (int i = 0; i < numberOfProperties[3]; i++)
                {
                    gpt.createCollectionalProperty(names[index], descriptions[index], collectionsNam[i], importance[index]);
                    index++;
                }
            }
        }

        public void addGenerations(int numberOfGens, int[][] propersNumbers, bool[] immort)
        {
            for (int i = 0; i < numberOfGens; i++)
            {
                gpt.createGeneration(propersNumbers[i], immort[i]);
            }
        }

        public void addFuntions(int numberOfFuncs, int[][] propNumbers, float[][] coefFr, float[][] coefEn, int[] funcToProp)
        {
            for (int i = 0; i < numberOfFuncs; i++)
            {
                Property[] prop = new Property[propNumbers[i].Length];
                for (int j = 0; j < prop.Length; j++)
                {
                    prop[j] = gpt.getProperty(propNumbers[i][j]);
                }
                functionManager.createFunction(prop, coefFr[i], coefEn[i]);
                functionManager.addFunctionToProperty(gpt.getProperty(funcToProp[i]), i);
            }
        }

        public void addPoints(Position[] pos, int[] teams, int[] generations)
        {
            if (pos.Length == teams.Length && teams.Length == generations.Length)
            {
                int size = pos.Length;
                for (int i = 0; i < size; i++)
                {
                    if (pointsManager.getPoint(pos[i]).getTeam() == 0)
                        pointsManager.bringToLive(pos[i], teams[i], gpt.getGeneration(generations[i]));
                    else
                        Debug.Log("This point is already alive.(addPoints())");
                }
            }
            else
                Debug.Log("The wrong parametras are input.(addPoints())");
        }

        public void addPointsAreas(byte theLevelNumber, Position[] pos, int[] teams, int[] generations)
        {
            if (pos.Length == teams.Length && teams.Length == generations.Length)
            {
                int size = pos.Length;
                for (int i = 0; i < size; i++)
                {
                    if (areasManager[theLevelNumber].getPoint(pos[i]).getTeam() == 0)
                        areasManager[theLevelNumber].bringToLive(pos[i], teams[i], gpt.getGeneration(generations[i]));
                    else
                        Debug.Log("This point is already alive.(addPoints())");
                }
            } else
                Debug.Log("The wrong parametras are input.(addPoints())");
        }
        //Тест

        public void dump()
        {
            Point[][] points =  pointsManager.getAllPoints();
            for (int i = 0; i < pointsManager.getSize()[0]; i++)
            {
                string str = "";
                for (int j = 0; j < pointsManager.getSize()[1]; j++)
                {
                    str += points[i][j].getTeam().ToString() + " ";
                }
                Debug.Log(str);
            }
            Debug.Log("******************************************************************");
        }

        public int getAge()
        {
            return age;
        }
    }

    public class PointOperator
    {
        //Переменные
        private byte numberOfLevels;
        private bool[] pointsPositions;
        //private bool[] pointsPositionsImp;
        private bool center;
        //Конструктор
        public PointOperator(byte levels, int[] positions)
        {
            numberOfLevels = levels;
            int size = (levels + 2)* (levels + 2);
            pointsPositions = new bool[size];
            //pointsPositionsImp = new bool[size];
            if (positions.Length > 0)
            {
                int j = 0;
                for (int i = 0; i < size; i++)
                {
                    if (i == positions[j])
                    {
                        pointsPositions[i] = false;
                        j++;
                    }
                    else
                        pointsPositions[i] = true;
                }
            }
            else
                for (int i = 0; i < size; i++)
                    pointsPositions[i] = true;
            center = pointsPositions[(size - 1) / 2];
            pointsPositions[(size - 1) / 2] = false;
            /*for (int i = 0; i < size; i++)
                pointsPositionsImp[i] = pointsPositionsVirt[i];
            pointsPositionsImp[(size - 1) / 2] = false;*/
        }

        public void virtualize(PointsManager pm)
        {
            int size = numberOfLevels * 2 + 1;
            List<Position> alivePoints = pm.getAlivePoints();
            Position pos;
            Generation gen;
            int teamNum;
            int index = 0;
            for (int k = 0; k < alivePoints.Count; k++)
            {
                pos = alivePoints[k];
                if (!pm.getPointImmortale(pos))
                {
                    //Debug.Log("-");
                    index = 0;
                    gen = pm.getPoint(pos).getGeneration();
                    teamNum = pm.getPoint(pos).getTeam();
                    int x = pos.getX() - numberOfLevels;
                    for (int i = 0; i < size; i++)
                    {
                        if (x < 0) x = pm.getSize()[0] + x;
                        else if (x == pm.getSize()[0]) x = 0;
                        int y = pos.getY() - numberOfLevels;
                        for (int j = 0; j < size; j++)
                        {
                            if (y < 0) y = pm.getSize()[1] + y;
                            else if (y == pm.getSize()[1]) y = 0;
                            if (pointsPositions[index])
                            {
                                Position p = new Position(x, y);
                                pm.createVirtualPoint(p, teamNum, gen);
                            }
                            y++;
                            index++;
                        }
                        x++;
                    }
                    //pm.deleteVirtualPoint(pos, gen, teamNum);
                    //pm.createVirtualPoint(pos, teamNum, gen, true);
                    if (center)
                    {
                        pm.deleteVirtualPoint(pos, gen, teamNum);
                        pm.addVirtualPoint(pos, pm.clone(pos));
                    }
                    //Debug.Log(pm.getVirtualPoints(pos).Count);
                }
                else
                    pm.addVirtualizedPoint(pos);
            }
        }

        public void impactPoints(PointsManager pm)
        {
            int size = numberOfLevels * 2 + 1;
            List<Position> alivePoints = pm.getAlivePoints();
            int index = 0;
            for (int k = 0; k < alivePoints.Count; k++)
            {
                Position pos = alivePoints[k];
                index = 0;
                Generation gen = pm.getPoint(pos).getGeneration();
                int teamNum = pm.getPoint(pos).getTeam();
                int x = pos.getX() - numberOfLevels;
                for (int i = 0; i < size; i++)
                {
                    if (x < 0) x = pm.getSize()[0] + x;
                    else if (x == pm.getSize()[0]) x = 0;
                    int y = pos.getY() - numberOfLevels;
                    for (int j = 0; j < size; j++)
                    {
                        if (y < 0) y = pm.getSize()[1] + y;
                        else if (y == pm.getSize()[1]) y = 0;
                        if (pointsPositions[index])
                        {
                            Position p = new Position(x, y);
                            pm.impactPoint(pos, p);
                        }
                        y++;
                        index++;
                    }
                    x++;
                }
            }
            pm.clearAlivePoints();
        }

        public void impactAreasToPoints(PointsManager pm, PointsManager am)
        {
            List<Position> alivePointsArea = am.getAlivePoints();
            for (int i = 0; i < alivePointsArea.Count; i++)
            {
                Position pos = alivePointsArea[i];
                Point areaPoint = am.getPoint(pos);
                List<Point> virtualPoints = pm.getVirtualPoints(pos);
                for (int j = 0; j < virtualPoints.Count; j++)
                {
                    areaPoint.impcatPoint(virtualPoints[j]);
                }
            }
        }

        public void decider(PointsManager pm)
        {
            List<Position> countPoints = pm.getVirtualizedPoints();
            //Debug.Log(countPoints.Count);
            for (int i = 0; i < countPoints.Count; i++)
            {
                Position pos = countPoints[i];
                if (pm.getPoint(pos).getTeam() > 0 && pm.getPointImmortale(pos))
                {
                    pm.resetPoint(pos);
                }
                else
                {
                    //int pointer = -1;
                   // Debug.Log(pos.getX() + " " + pos.getY());
                    List<Point> virtPoints = pm.getVirtualPoints(pos);
                    //Debug.Log(virtPoints.Count);
                    float[] resultes = new float[virtPoints.Count];
                    for (int j = 0; j < virtPoints.Count; j++)
                    {

                        //if (virtPoints[j].getTeam() == pm.getPoint(pos).getTeam() && virtPoints[j].getGeneration() == pm.getPoint(pos).getGeneration())
                        //    pointer = j;
                        //resultes[j] = 0;


                        for (int k = 0; k < virtPoints[j].getCharacteristics().Count; k++)
                        {
                            float res = virtPoints[j].getCharacteristics()[k].checkLive();
                            if (res < 0 && virtPoints[j].getCharacteristics()[k].getMustHave())
                            {
                                resultes[j] = -1000;
                                break;
                            }
                            else
                                resultes[j] += res;
                             //Debug.Log(virtPoints[j].getCharacteristics()[k].getLivePointMask());
                            //Debug.Log(virtPoints[j].getCharacteristics()[k].checkLive());
                            //Debug.Log(virtPoints[j].getCharacteristics().Count);
                        }
                    }
                    float maxResult = resultes[0];
                    int posResult = 0;
                    for (int j = 1; j < virtPoints.Count; j++)
                    {
                        if (maxResult < resultes[j])
                        {
                            maxResult = resultes[j];
                            posResult = j;
                        }
                    }
                    bool flag = true;
                    for (int j = 0; j < virtPoints.Count; j++)
                    {
                        if (maxResult == resultes[j] && j != posResult)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag && maxResult > 0)
                    {
                        pm.bringToLive(pos, virtPoints[posResult].getTeam(), virtPoints[posResult].getGeneration());
                    }
                    else
                    {
                        if (maxResult >= 0)
                        {
                            if (resultes[virtPoints.Count - 1] >= 0 && pm.getPoint(pos).getTeam() > 0)
                            {
                                //Debug.Log(pos.getX() + " " + pos.getY());
                                //Debug.Log(resultes[pointer]);
                                //pm.bringToLive(pos, pm.getPoint(pos).getTeam(), pm.getPoint(pos).getGeneration());
                                pm.resetPoint(pos);
                            }
                        }
                        else
                            pm.kill(pos);
                    }
                }
            }
        }

        public void cleaner(PointsManager pm)
        {
            List<Position> virtPoints = pm.getVirtualizedPoints();
            for (int i = 0; i < virtPoints.Count; i++)
            {
                Position pos = virtPoints[i];
                pm.clearVitrualPoints(pos);
            }
            pm.getVirtualizedPoints().Clear();
        }

    }
}
