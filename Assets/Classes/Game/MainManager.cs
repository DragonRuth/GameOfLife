using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
using Classes.Game.GenerationsPropertiesTableSpace;
using Classes.Game.PointsManagerSpace;
using Classes.GameClasses.FuncManegerSpace;
using Classes.GameClasses.PointSpace;
using Classes.GameClasses.PropertiesSpace;
using Classes.Game.PlayersManagerSpace;

namespace Classes.Game.MainManagerSpace
{
	[System.Serializable]
    public class Rules
    {
		public string NameOfTheRules;
		public string DisOfTheRules;
        public int[] numberOfProperties;
        public bool[] importance;
        public string[] names;
        public string[] descriptions;
        public float[] aliveInterLinery;
        public float[] deadInterLinery;
        public float[] onePointPowrStat;
        public float[] livePStat;
        public float[] livePDyn;
        public float[] setP;
        public string[] collectionsNamLinery;

        public int numberOfGens;
        public string[] genNames;
        public string[] genDesc;
        public int[] propersNumbersLinery;
        public bool[] immort;

        public int numberOfFuncs;
        public int[] propNumbersLinery;
        public float[] coefFrLinery;
        public float[] coefEnLinery;
        public int[] funcToProp;

        public float[] DoubleToSingle(float[][] input)
        {
            List<float> result = new List<float>();
            result.Add(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                int length = input[i].Length;
                result.Add(length);
                for (int j = 0; j < length; j++)
                    result.Add(input[i][j]);
            }
            return result.ToArray();
        }

        public int[] DoubleToSingle(int[][] input)
        {
            List<int> result = new List<int>();
            result.Add(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                int length = input[i].Length;
                result.Add(length);
                for (int j = 0; j < length; j++)
                    result.Add(input[i][j]);
            }
            return result.ToArray();
        }

        public string[] DoubleToSingle(string[][] input)
        {
            List<string> result = new List<string>();
            result.Add(input.Length.ToString());
            for (int i = 0; i < input.Length; i++)
            {
                int length = input[i].Length;
                result.Add(length.ToString());
                for (int j = 0; j < length; j++)
                    result.Add(input[i][j]);
            }
            return result.ToArray();
        }

        public float[][] SingleToDouble(float[] input)
        {
            float[][] result = new float[(int)input[0]][];
            int iter = 1;
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new float[(int)input[iter]];
                
                for (int j = 0; j < result[i].Length; j++)
                {
                    iter++;
                    result[i][j] = input[iter];
                }
                iter++;
            }
            return result;
        }

        public int[][] SingleToDouble(int[] input)
        {
            int[][] result = new int[input[0]][];
            int iter = 1;
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new int[input[iter]];
                for (int j = 0; j < result[i].Length; j++)
                {
                    iter++;
                    result[i][j] = input[iter];
                }
                iter++;
            }
            return result;
        }

        public string[][] SingleToDouble(string[] input)
        {
            string[][] result = new string[Convert.ToInt32(input[0])][];
            int iter = 1;
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new string[Convert.ToInt32(input[iter])];
                for (int j = 0; j < result[i].Length; j++)
                {
                    iter++;
                    result[i][j] = input[iter];
                }
                iter++;
            }
            return result;
        }

        public void saveRules(MainManager mainManager)
        {
            GenerationsPropertiesTable gpt = mainManager.getGPT();
            List<Property> props = gpt.getPropertiesList();
            numberOfProperties = new int[4];
            numberOfProperties[0] = props.Count;
            numberOfProperties[1] = 0;
            numberOfProperties[2] = 0;
            numberOfProperties[3] = 0;
            // names = new string[props.Count];
            //descriptions = new string[props.Count];
            //importance = new bool[props.Count];

            List<string> namess = new List<string>();
            List<string> descriptionss = new List<string>();
            List<bool> importances = new List<bool>();

            List<int> ftp = new List<int>();

            List<float[]> aliveInt = new List<float[]>();
            List<float[]> deadInt = new List<float[]>();
            List<float> onePpow = new List<float>();
            List<float> livePS = new List<float>();

            List<float> livePD = new List<float>();
            List<float> sP = new List<float>();

            List<string[]> colN = new List<string[]>();

            for (int i = 0; i < props.Count; i++)
            {
                byte propType = props[i].getType();
                //names[i] = props[i].getNameAndDescription()[0];
                //descriptions[i] = props[i].getNameAndDescription()[1];
                //importance[i] = props[i].getMustHave();
                switch (propType)
                {
                    case 0:
                        ftp.Insert(numberOfProperties[1], i);
                        Debug.Log(numberOfProperties[1] + "/" + i);
                        namess.Insert(numberOfProperties[1], props[i].getNameAndDescription()[0]);
                        descriptionss.Insert(numberOfProperties[1], props[i].getNameAndDescription()[1]);
                        importances.Insert(numberOfProperties[1], props[i].getMustHave());
                        numberOfProperties[1]++;
                        StaticalProperty stPr = (StaticalProperty)props[i];
                        int size = stPr.getLiveInterval().Length * 2;
                        float[] ai = new float[size];
                        for (int j = 0; j < size / 2; j++)
                        {
                            ai[2 * j] = stPr.getLiveInterval()[j][0];
                            ai[2 * j + 1] = stPr.getLiveInterval()[j][1];
                        }
                        aliveInt.Add(ai);
                        size = stPr.getDeadInterval().Length * 2;
                        float[] di = new float[size];
                        for (int j = 0; j < size / 2; j++)
                        {
                            di[2 * j] = stPr.getDeadInterval()[j][0];
                            di[2 * j + 1] = stPr.getDeadInterval()[j][1];
                        }
                        deadInt.Add(di);
                        onePpow.Add(stPr.getPower());
                        livePS.Add(stPr.getLivePoint());
                        break;
                    case 1:
                        ftp.Insert(numberOfProperties[1] + numberOfProperties[2], i);
                        Debug.Log(numberOfProperties[2] + "/" + i);
                        namess.Insert(numberOfProperties[1] + numberOfProperties[2], props[i].getNameAndDescription()[0]);
                        descriptionss.Insert(numberOfProperties[1] + numberOfProperties[2], props[i].getNameAndDescription()[1]);
                        importances.Insert(numberOfProperties[1] + numberOfProperties[2], props[i].getMustHave());
                        numberOfProperties[2]++;
                        DynamicalProperty dynPr = (DynamicalProperty)props[i];
                        livePD.Add(dynPr.getLivePoint());
                        sP.Add(dynPr.getSetPoint());
                        break;
                    case 2:
                        ftp.Insert(numberOfProperties[1] + numberOfProperties[2] + numberOfProperties[3], i);
                        Debug.Log(numberOfProperties[3] + "/" + i);
                        namess.Insert(numberOfProperties[1] + numberOfProperties[2] + numberOfProperties[3], props[i].getNameAndDescription()[0]);
                        descriptionss.Insert(numberOfProperties[1] + numberOfProperties[2] + numberOfProperties[3], props[i].getNameAndDescription()[1]);
                        importances.Insert(numberOfProperties[1] + numberOfProperties[2] + numberOfProperties[3], props[i].getMustHave());
                        numberOfProperties[3]++;
                        CollectionalProperty colPr = (CollectionalProperty)props[i];
                        colN.Add(colPr.getCollection());
                        break;
                }
            }

            ftp.ToArray();
            names = namess.ToArray();
            descriptions = descriptionss.ToArray();
            importance = importances.ToArray();

            numberOfGens = gpt.getAllGenerations().Count;
            List<Generation> gens = gpt.getAllGenerations();
            List<int[]> propNums = new List<int[]>();
            genNames = new string[numberOfGens];
            genDesc = new string[numberOfGens];
            immort = new bool[numberOfGens];
            for (int i = 0; i < numberOfGens; i++)
            {
                genNames[i] = gens[i].getNameAndDescription()[0];
                genDesc[i] = gens[i].getNameAndDescription()[1];
                List<int> properts = new List<int>();//[gens[i].getProperties().Count];
                for (int j = 0; j < gens[i].getProperties().Count; j++)
                {
                    properts.Add(ftp.IndexOf(gens[i].getProperties()[j].getNumber()));
                }
                properts.Sort();
                propNums.Add(properts.ToArray());
                immort[i] = gens[i].getImmortale();
            }

            FunctionManager fm = mainManager.getFunctionalManager();
            List<Function> funcs = fm.getAllFunctions();
            numberOfFuncs = fm.getNumberOfFunctions();
            List<float[]> fc = new List<float[]>();
            List<float[]> ec = new List<float[]>();
            List<int[]> funcProps = new List<int[]>();
            funcToProp = new int[numberOfFuncs];
            for (int  i = 0; i < numberOfFuncs; i++)
            {
                if (funcs[ftp[i]] != null)
                {
                    bool type = funcs[ftp[i]].getType();
                    switch (type)
                    {
                        case false:
                            FunctionStatAndDynam func = (FunctionStatAndDynam)funcs[ftp[i]];
                            int size = func.getSufferingProperties().Length;
                            float[] ff = new float[size];
                            float[] ee = new float[size];
                            int[] prToFunc = new int[size];
                            for (int j = 0; j < size; j++)
                            {
                                prToFunc[j] = ftp.IndexOf(func.getSufferingProperties()[j].getNumber());
                                ff[j] = func.getCoefficientsFriend()[j];
                                ee[j] = func.getCoefficientsEnemy()[j];
                            }
                            fc.Add(ff);
                            ec.Add(ee);
                            funcProps.Add(prToFunc);
                            funcToProp[i] = i;
                            break;
                        case true:
                            float[] f = { 1 };
                            float[] e = { -1 };
                            int[] fpr = { i };
                            fc.Add(f);
                            ec.Add(e);
                            funcProps.Add(fpr);
                            funcToProp[i] = i;
                            break;
                    }
                } else
                {
                    float[] f = {  };
                    float[] e = {  };
                    int[] fpr = {  };
                    fc.Add(f);
                    ec.Add(e);
                    funcProps.Add(fpr);
                    funcToProp[i] = i;
                }
            }

            float[][] aliveInter = aliveInt.ToArray();
            float[][] deadInter = deadInt.ToArray();
            onePointPowrStat = onePpow.ToArray();
            livePStat = livePS.ToArray();
            livePDyn = livePD.ToArray();
            setP = sP.ToArray();
            string[][] collectionsNam = colN.ToArray();

            int[][] propersNumbers = propNums.ToArray();

            int[][] propNumbers = funcProps.ToArray();
            float[][] coefFr = fc.ToArray();
            float[][] coefEn = ec.ToArray();

            aliveInterLinery = DoubleToSingle(aliveInter);
            deadInterLinery = DoubleToSingle(deadInter);
            collectionsNamLinery = DoubleToSingle(collectionsNam);
            propersNumbersLinery = DoubleToSingle(propersNumbers);
            propNumbersLinery = DoubleToSingle(propNumbers);
            coefFrLinery = DoubleToSingle(coefFr);
            coefEnLinery = DoubleToSingle(coefEn);

            Debug.Log("Rules are saved.");
        } 

        public void loadRules(MainManager mainManager)
        {
            float[][] aliveInter = SingleToDouble(aliveInterLinery);
            float[][] deadInter = SingleToDouble(deadInterLinery);
            string[][] collectionsNam = SingleToDouble(collectionsNamLinery);
            int[][] propersNumbers = SingleToDouble(propersNumbersLinery);
            int[][] propNumbers = SingleToDouble(propNumbersLinery);
            float[][] coefFr = SingleToDouble(coefFrLinery);
            float[][] coefEn = SingleToDouble(coefEnLinery);

            mainManager.initRules(numberOfProperties, importance, names, descriptions, 
                aliveInter, deadInter, onePointPowrStat, livePStat, livePDyn, setP, 
                collectionsNam, numberOfGens, genNames, genDesc, propersNumbers, immort, 
                numberOfFuncs, propNumbers, coefFr, coefEn, funcToProp);
            Debug.Log("Rules are loaded.");
        }

		public void setNameAndDis( string N, string D) {
			NameOfTheRules = N;
			DisOfTheRules = D;
		}
    }


    public class MainManager
    {
        //Менеджеры
        private PointsManager pointsManager;
        private GenerationsPropertiesTable gpt;
        private FunctionManager functionManager;
        private PointOperator pointsOperator;
        private PlayersManager playersManager;
        private List<PointsManager> areasManager;
        //private Rules rules;
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
            //rules = new Rules();
            numberOfAreasLevels = numberOfAreasLev;
            playersManager = null;
            if (numberOfAreasLevels > 0)
            {
                areasManager = new List<PointsManager>();
                for (int i = 0; i < numberOfAreasLevels; i++)
                    areasManager.Add(new PointsManager(sizeX, sizeY));
            }
            age = 0;
           // Debug.Log("The mainManager was created.");
        }

        public void clearUp()
        {
            killAllPoints();
            functionManager.clear();
            gpt.clear();
            age = 0;
        }

        //Методы
        public Rules saveRules()
        {
            Rules rules = new Rules();
            rules.saveRules(this);
            return rules;
        }

        public void initLevel(Rules rl, int[] points)
        {
            loadRules(rl);
            bringToLifeFromArray(points);
        }

        public void loadRules(Rules rules)
        {
            //rules = rl;
            rules.loadRules(this);
            allStat();
        }

        public void allStat()
        {
            Debug.Log("Properties:" + gpt.getNumberOfProperties());
            Debug.Log("Generations:" + gpt.getAllGenerations().Count);
            Debug.Log("Functions" + functionManager.getNumberOfFunctions());
        }
        
        public void createPlayers(byte numOfPlayers, byte res, byte gen, byte numOfComp)
        {
            playersManager = new PlayersManager(numOfPlayers, res, gpt.getGeneration(gen), numOfComp, pointsManager, pointsOperator);
        }

        public void setPlayersParametres(byte scForKill, byte resForKill, byte resForCicle, byte scoreForBTL, byte numberOfCircles)
        {
            playersManager.setParameters(scForKill, resForKill, resForCicle, scoreForBTL, numberOfCircles);
        }

        public void step()
        {
            if (playersManager != null)
                playersManager.allComputerTurns();
            pointsOperator.virtualize(pointsManager);
            pointsOperator.impactPoints(pointsManager);
            if (numberOfAreasLevels > 0)
            {
                for (int i = 0; i < numberOfAreasLevels; i++)
                {
                    //Debug.Log("+");
                    pointsOperator.impactAreasToPoints(pointsManager, areasManager[i]);
                    pointsOperator.virtualize(areasManager[i]);
                    pointsOperator.impactPoints(areasManager[i]);
                    pointsOperator.decider(areasManager[i], playersManager);
                    pointsOperator.cleaner(areasManager[i]);
                }
            }
            pointsOperator.decider(pointsManager, playersManager);
            pointsOperator.cleaner(pointsManager);
            //Debug.Log(pointsManager.getAlivePoints().Count);
            //Debug.Log(pointsManager.getVirtualizedPoints().Count);
            age++;
            if (playersManager != null)
            {
                playersManager.circle();
            }
        }


        public void initRules(int[] numberOfProperties, bool[] importance, string[] names, string[] descriptions,
           float[][] aliveInter, float[][] deadInter, float[] onePointPowerStat, float[] livePStat,
           float[] livePDyn, float[] setP, string[][] collectionsNam,
           int numberOfGens, string[] genNames, string[] genDesc, int[][] propersNumbers, bool[] immort,
           int numberOfFuncs, int[][] propNumbers, float[][] coefFr, float[][] coefEn, int[] funcToProp)
        {
            clearUp();
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
                gpt.createGeneration(genNames[i], genDesc[i], propersNumbers[i], immort[i]);
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

        public void addGenerations(int numberOfGens, string[] genNames, string[] genDesc, int[][] propersNumbers, bool[] immort)
        {
            for (int i = 0; i < numberOfGens; i++)
            {
                gpt.createGeneration(genNames[i], genDesc[i], propersNumbers[i], immort[i]);
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

        public FunctionManager getFunctionalManager()
        {
            return functionManager;
        }

        public void createStaticalProperty(string nam, string desc, float[][] liveInt, float[][] deadInt,
           float onePointPower, float liveP, bool mustH = false)
        {
            gpt.createStaticalProperty(nam, desc, liveInt, deadInt, onePointPower, liveP = 0, mustH);
        }

        public void createDynamicalProperty(string nam, string desc, float liveP, float setP, bool mustH = false)
        {
            gpt.createDynamicalProperty(nam, desc, liveP, setP , mustH);
        }

        public void createCollectionalProperty(string nam, string desc, string[] coll, bool mustH = false)
        {
            gpt.createCollectionalProperty(nam, desc, coll, mustH);
        }

        public PointsManager getPointsManager()
        {
            return pointsManager;
        }

        public List<Generation> getGenerations()
        {
            return gpt.getAllGenerations();
        }

        public PointOperator getPointsOperator()
        {
            return pointsOperator;
        }

        public PlayersManager getPlayersManager()
        {
            return playersManager;
        }

        public GenerationsPropertiesTable getGPT()
        {
            return gpt;
        }

        public void createGenerationWithoutProp(string nm, string desc)
        {
            gpt.createGenerationWithoutProp(nm, desc);
        }

        public void addPropertyToGeneration(int numberOfGeneration, int numberOfProperty)

        {
            gpt.addPropertyToGeneration(numberOfGeneration, numberOfProperty);
        }

        public void setImmortaleOfGeneration(int numberOfGeneration, bool immortale)
        {
            gpt.setImmortale(numberOfGeneration, immortale);
        }

        public void correctGeneration(int numberOfGeneration)
        {
            pointsManager.correctGeneration(gpt.getGeneration(numberOfGeneration));
        }

        public void addFunctionToProperty(int prop, int index)
        {
            functionManager.addFunctionToProperty(gpt.getProperty(prop), index);
        }

		public void addLastFunctionToLastProperty()
		{
			Debug.Log (gpt.getNumberOfProperties() +"/"+functionManager.getNumberOfFunctions());
			addFunctionToProperty(gpt.getNumberOfProperties() - 1, functionManager.getNumberOfFunctions() - 1);
		}

        public void createFunction(int[] props, float[] coefFr, float[] coefEn)
        {
            List<Property> properties = new List<Property>();
            for (int i = 0; i < props.Length; i++)
                properties.Add(gpt.getProperty(props[i]));
            functionManager.createFunction(properties.ToArray(), coefFr, coefEn);
        }

        public void resetFunction(int numberOfFunction, int[] props, float[] coefFr, float[] coefEn)
        {
            List<Property> properties = new List<Property>();
            for (int i = 0; i < props.Length; i++)
                properties.Add(gpt.getProperty(props[i]));
            functionManager.resetFunction(numberOfFunction, properties.ToArray(), coefFr, coefEn);
			addFunctionToProperty (numberOfFunction, numberOfFunction);
        }

        public void deleteGeneration(int number)
        {
            Generation gen = gpt.getGeneration(number);
            List<Position> alivePoints = pointsManager.getAlivePoints();
            for (int i = 0; i < alivePoints.Count; i++)
                if (pointsManager.getPoint(alivePoints[i]).getGeneration() == gen)
                {
                    pointsManager.getPoint(alivePoints[i]).kill();
                    alivePoints.RemoveAt(i);
					i--;
					Debug.Log ("delete");
                }
            gpt.deleteGeneration(number);
        }

        public void deleteProperty(int number)
        {
            Property prop = gpt.getProperty(number);
            List<Generation> generations = gpt.getAllGenerations();
            for (int i = 0; i < generations.Count; i++)
            {
                List<Property> propertiesOfGeneration = generations[i].getProperties();
                for (int j = 0; j < propertiesOfGeneration.Count; j++)
                {
                    if (propertiesOfGeneration[j] == prop)
                    {
                        generations[i].removeProperty(j);
                        correctGeneration(i);
                        break;
                    }
                }
            }
			functionManager.deleteFunction (number);
            gpt.deleteProperty(number);
        }

        public int checkLive(Position pos)
        {
            return pointsManager.checkLive(pos);
        }

        public int countAlivePoints(Position pos1, Position pos2, int teamNumber)
        {
            int result = 0;
            for (int i = pos1.getX(); i <= pos2.getX(); i++)
                for (int j = pos1.getY(); j <= pos2.getY(); j++)
                {
                    Position posH = new Position(i, j);
                    if (teamNumber == checkLive(posH))
                        result++;
                }
            return result;
        }

        public void savePoints()
        {
            pointsManager.savePoints();
        }

		public void resetStaticalProperty(string nam, string desc, int number, float[][] liveInt, float[][] deadInt, 
			float onePointPower, float liveP, bool mustH) 
		{
			gpt.resetStaticalProperty (nam, desc,number, liveInt, deadInt, 
				onePointPower, liveP, mustH);
		}

		public void resetDynamicalProperty(string nam, string desc, int number, float liveP, float setP,bool mustH)
		{
			gpt.resetDynamicalProperty(nam, desc, number, liveP, setP, mustH);
		}

		public void resetCollectionalProperty(string nam, string desc, int number,string[] coll, bool mustH)
		{
			gpt.resetCollectionalProperty (nam, desc, number, coll, mustH);
		}

		public List<Point> getLastSavePoints(bool deleteLast = false)
        {
            return pointsManager.getLastSavePoints(deleteLast);
        }

        public List<Point> getSavePoints(int number, bool delete = false)
        {
            return pointsManager.getSavePoints(number, delete);
        }

        public int[] getLastSavedPointsArray()
        {
            List<int> result = new List<int>();
            List<Point> savedPoints = pointsManager.getLastSavePoints(false);
            for (int i = 0; i < savedPoints.Count; i++)
            {
                result.Add(savedPoints[i].getTeam());
                result.Add(savedPoints[i].getGeneration().getNumber());
                result.Add(savedPoints[i].getPosition().getX());
                result.Add(savedPoints[i].getPosition().getY());
            }
            return result.ToArray();
        }

        public int[] savePointsAndGetArray()
        {
            savePoints();
            return getLastSavedPointsArray();
        }

        public void bringToLifeFromArray(int[] input)
        {
            int size = input.Length / 4;
            for (int i = 0; i < size; i++)
            {
                Position pos = new Position(input[i * 4 + 2], input[i * 4 + 3]);
                bringToLifePoint(input[i * 4], input[i * 4 + 1], pos);
            }
        }

        public void killAllPoints()
        {
            List<Position> alivePoints = pointsManager.getAlivePoints();
            for (int i = 0; i < alivePoints.Count; i++)
                pointsManager.getPoint(alivePoints[i]).kill();
			pointsManager.getAlivePoints ().Clear();
        }

		public void bringToLifePoint(int team, int generation, Position  pos) 
		{
			pointsManager.bringToLive (pos, team, gpt.getGeneration (generation));
		}

		public bool bringToLifePointPV(int team, int generation, Position  pos) 
		{
			if (playersManager.getResource ((byte)team) > 0)
			{
				pointsManager.bringToLive (pos, team, gpt.getGeneration (generation));
				playersManager.incResources ((byte)team, -1);
				return true;
			} else
				return false;
		}

		public void killPoint(Position pos) 
		{
			pointsManager.killTouch (pos);
		}

		public int getResourcrs(int team) 
		{
			return playersManager.getResource ((byte)team);
		}

		public int getScore(int team) 
		{
			return playersManager.getScore (team);
		}

		public int getGeneratioPointsNumber(Position pos)
		{
			return pointsManager.getPoint (pos).getGeneration ().getNumber ();
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
            int size = (levels + 2) * (levels + 2);
            pointsPositions = new bool[size];
            //pointsPositionsImp = new bool[size];
            if (positions.Length > 0)
            {
                int j = 0;
				int i = 0;
				for (; i < size && j < positions.Length; i++)
                {
                    if (i == positions[j])
                    {
                        pointsPositions[i] = false;
                        j++;
                    }
                    else
                        pointsPositions[i] = true;
                }
				for (; i < size; i++)
					pointsPositions[i] = true;
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
                    /*if (center)
                    {
                        pm.deleteVirtualPoint(pos, gen, teamNum);
                        pm.addVirtualPoint(pos, pm.clone(pos));
                    }*/
                    //Debug.Log(pm.getVirtualPoints(pos).Count);
                }
                else
                    //pm.addVirtualizedPoint(pos);
                    pm.addVirtualPoint(pos, pm.clone(pos));
            }
            if (center)
            {
                for (int k = 0; k < alivePoints.Count; k++)
                {
                    pos = alivePoints[k];
                    gen = pm.getPoint(pos).getGeneration();
                    teamNum = pm.getPoint(pos).getTeam();
                    if (!pm.getPointImmortale(pos))
                    {
                        pm.deleteVirtualPoint(pos, gen, teamNum);
                        pm.addVirtualPoint(pos, pm.clone(pos));
                    }
                }
            }
        }

        public void virtualizeOnePointForComputer(PointsManager pm, Position pos)
        {
            int size = numberOfLevels * 2 + 1;
            Generation gen;
            int teamNum;
            int index = 0;
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
                    //pm.deleteVirtualPoint(pos, gen, teamNum);
                    //pm.addVirtualPoint(pos, pm.clone(pos));
                    pm.createVirtualPoint(pos, teamNum, gen);

                }
                //Debug.Log(pm.getVirtualPoints(pos).Count);
            }
            else
                pm.addVirtualizedPoint(pos);
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

        public void impactOnePointForComputer(PointsManager pm, Position pos)
        {
            int size = numberOfLevels * 2 + 1;
            int index = 0;
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

        public void decider(PointsManager pm, PlayersManager plm)
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
                    //Debug.Log(pos.getX() + " " + pos.getY());
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
                            // Debug.Log(virtPoints[j].getCharacteristics()[k].getLivePointMask());
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
                        if (pm.getPoint(pos).getTeam() > 0)
                        {
                            if (pm.comparePoints(pos, virtPoints[posResult].getGeneration(), virtPoints[posResult].getTeam()))
                            {
                                pm.resetPoint(pos);
                            }
                            else
                            {
                                if (plm != null)
                                    plm.incScore((byte)virtPoints[posResult].getTeam(), 1);
                                pm.bringToLive(pos, virtPoints[posResult].getTeam(), virtPoints[posResult].getGeneration());
                            }
                        } else
                        {
                            if (plm != null)
                                plm.incScore((byte)virtPoints[posResult].getTeam(), 0);
                            pm.bringToLive(pos, virtPoints[posResult].getTeam(), virtPoints[posResult].getGeneration());
                        }
                    }
                    else
                    {
                        //if (maxResult >= 0)
                        //{
                            if (pm.getPoint(pos).getTeam() > 0)
                            {
                                if (resultes[virtPoints.Count - 1] >= 0)
                                    //Debug.Log(pos.getX() + " " + pos.getY());
                                    //Debug.Log(resultes[pointer]);
                                    //pm.bringToLive(pos, pm.getPoint(pos).getTeam(), pm.getPoint(pos).getGeneration());
                                    pm.resetPoint(pos);
                                else
                                    pm.kill(pos);
                            }   
                        //}
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
