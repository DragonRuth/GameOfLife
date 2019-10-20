using UnityEngine;
using System.Collections;
using Classes.Game.PointsManagerSpace;
using Classes.Game.MainManagerSpace;
using Classes.GameClasses.PointSpace;
using System.Collections.Generic;
using Classes.Game.GenerationsPropertiesTableSpace;

namespace Classes.Game.PlayersManagerSpace
{
	[System.Serializable]
    public class PlayersManager
    {
        private List<Player> players;
        private byte numberOfPlayers;
        private byte scoreForKilling;
        private byte resourceForKilling;
        private byte resourcesForCircle;
        private byte scoreForBringingToLife;
        private byte numberOfCircles;
        private byte counter;

        public PlayersManager(byte numOfPlayers, int res, Generation gen, byte numOfComputers, PointsManager pm, PointOperator po)
        {
            scoreForKilling = 2;
            resourceForKilling = 1;
            resourcesForCircle = 1;
            scoreForBringingToLife = 1;
            numberOfCircles = 1;
            counter = numberOfCircles;
            players = new List<Player>();      
            for (int i = 0; i < numOfPlayers; i++)
                players.Add(new Player(res, gen));
            numberOfPlayers = numOfPlayers;
            for (int i = 0; i < numOfComputers; i++)
                players.Add(new Computer(res, gen, pm, po));
        }
   
        public void setParameters(byte scForKill, byte resForKill, byte resForCicle, byte scoreForBTL, byte numbOfCircles)
        {
            scoreForKilling = scForKill;
            resourceForKilling = scForKill;
            resourcesForCircle = resForCicle;
            scoreForBringingToLife = scoreForBTL;
            numberOfCircles = numbOfCircles;
            counter = numberOfCircles;
        }

        public void addPlayer(int res, Generation gen)
        {
            players.Add(new Player(res, gen));
        }

        public void addComputer(int res, Generation gen, PointsManager pm, PointOperator po)
        {
            players.Add(new Computer(res, gen, pm, po));
        }

        public void incScore(byte team, byte type)
        {
            switch (type)
            {
                case 0:
                    players[team - 1].incScore(scoreForBringingToLife);
                    break;
                case 1:
                    players[team - 1].incScore(scoreForKilling);
                    incResources(team, resourceForKilling);
                    break;
            }
        }

        public void incResources(byte team, int input)
        {
            if (input != 0)
                players[team - 1].incResource(input);
        }

        public int getResource(byte team)
        {
            return players[team - 1].getResouce();
        }

        public void incResourcesAll(byte input)
        {
            if (input > 0)
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].incResource(input);
                }
        }

        public void circle()
        {
            counter--;
            if (counter == 0)
            {
                incResourcesAll(resourcesForCircle);
                counter = numberOfCircles;
            }
        }

        public void allComputerTurns()
        {
            for (int i = numberOfPlayers; i < players.Count; i++)
            {
                Computer comp = (Computer)players[i];
                comp.turn();
            }
        }

        public void playerEscapes(byte team, PointsManager pm, PointOperator po)
        {
            Player player = players[team - 1];
            players.RemoveAt(team - 1);
            players.Add(new Computer(player.getResouce(), player.getGeneration(), pm, po));
            numberOfPlayers--;
        }

        public int getScore(int team)
        {
            return players[team - 1].getScore();
        }
    }
    
    public class Player
    {
        protected static byte numberOfPlayers;

        protected byte teamNumber;
        protected int score;
        protected int resource;
        protected Generation generation;

        public Player(int res, Generation gen)
        {
            resource = res;
            score = 0;
            numberOfPlayers++;
            teamNumber = numberOfPlayers;
            generation = gen;
        }

        public int getResouce()
        {
            return resource;
        }

        public void incResource(int input)
        {
            resource += input;
        }

        public int getScore()
        {
            return score;
        }

        public void incScore(int input)
        {
            score += input;
        }

        public static byte getNumberOfPlayers()
        {
            return numberOfPlayers;
        }

        public void setTeamNumber(byte team)
        {
            teamNumber = team;
        }

        public Generation getGeneration()
        {
            return generation;
        }

        ~Player()
        {
            numberOfPlayers--;
        }
    }

    public class Computer : Player
    {
        private PointsManager pointsManager;
        private PointOperator pointsOperator;

        private int numberOfPointsBefore;

        private int numberOfPointsNow;
        private int checkVirtPoints;
        private List<Point> virtualizedPoints;
        private List<int> firstPrior;
        private List<int> secondPrior;
        private List<int> thirdPrior;
        private List<Position> addPoints;

        public Computer(int res,Generation gen, PointsManager pm, PointOperator po): base(res, gen)
        {
            pointsManager = pm;
            pointsOperator = po;

            virtualizedPoints = new List<Point>();
            firstPrior = new List<int>();
            secondPrior = new List<int>();
            thirdPrior = new List<int>();
            addPoints = new List<Position>();
            numberOfPointsBefore = 0;
            numberOfPointsNow = 0;
            checkVirtPoints = 0;
        }

        public void virtualizeAndImpact()
        {
            //Debug.Log("Живые летки: " + pointsManager.getAlivePoints().Count);
            Position[] alivePoints = new Position[pointsManager.getAlivePoints().Count];
            pointsManager.getAlivePoints().CopyTo(alivePoints);
            pointsOperator.virtualize(pointsManager);
            pointsOperator.impactPoints(pointsManager);
            for (int i = 0; i < alivePoints.Length; i++)
            {
                pointsManager.addAlivePoint(alivePoints[i]);
            }
            List<Position> virtPoints = pointsManager.getVirtualizedPoints();
            for (int i = checkVirtPoints; i < virtPoints.Count; i++)
            {
                List<Point> virtPoint = pointsManager.getVirtualPoints(virtPoints[i]);
                for (int j = 0; j < virtPoint.Count; j++)
                {
                    if (virtPoint[j].getTeam() == teamNumber && virtPoint[j].getGeneration() == generation)
                    {
                        virtualizedPoints.Add(virtPoint[j]);
                    }
                }
            }
            checkVirtPoints = virtPoints.Count;
        }

        public void selector()
        {
           // Debug.Log("SBEBIN");
            for (int i = 0; i < virtualizedPoints.Count; i++)
            {
                Point virtPoint = virtualizedPoints[i];
                float mask = virtPoint.getCharacteristics()[0].getLivePointMask();
               // Debug.Log(mask+" : "+virtPoint.getPosition().getX() + "S" + virtPoint.getPosition().getY());
                if (pointsManager.getPoint(virtPoint.getPosition()).getTeam() == 0)
                {
                    switch ((int)mask)
                    {
                        case 3:
                            if (checkFirstRule(virtualizedPoints[i].getPosition(), virtualizedPoints[i]))
                            {
                                firstPrior.Add(i);
                            }
                            break;
                        case 2:
                            secondPrior.Add(i);
                            break;
                        case 1:
                            thirdPrior.Add(i);
                            break;
                    }
                }
            }
        }

        public void decider()
        {
            numberOfPointsNow = virtualizedPoints.Count;
            bool[] flags = new bool[4];
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            flags[3] = false;
            Debug.Log("Русурс: "+resource);
            if (numberOfPointsNow == 0)
            {
                //Debug.Log(0);
                flags[3] = true;
                int lost = 0;
                for (int i = 0; i < resource && i < 5; i++)
                {
                    if (deciderStep(flags)) lost++;

                }
                resource -= lost;
              //  Debug.Log("Steps: " + steps);
              //  Debug.Log("Resource: "+resource);

            } else
            if (numberOfPointsNow < numberOfPointsBefore)
            {
                //Debug.Log(1);
                flags[2] = false;
                int steps = Random.Range(1, resource / 10) + resource / 5;
                int lost = 0;
                for (int i = 0; i < steps && i < resource; i++)
                {
                    if (deciderStep(flags)) lost++;
                }
                resource -= lost;
                //Debug.Log("Steps: " + steps);
                //Debug.Log("Resource: " + resource);
            } else
            if (numberOfPointsNow > numberOfPointsBefore)
            {
                //Debug.Log(2);
                flags[2] = false;
                flags[1] = false;
                int lost = 0;
                int steps = Random.Range(0, resource * (numberOfPointsNow / numberOfPointsBefore) - resource) + resource / 100;
                for (int i = 0; i < steps; i++)
                {
                    if (deciderStep(flags)) lost++;
                }
                resource -= lost;
                //Debug.Log("Steps: " + steps);
                //Debug.Log("Resource: " + resource);
            } else
             if (numberOfPointsNow == numberOfPointsBefore)
            {
                //Debug.Log(3);
                flags[2] = false;
                int steps = Random.Range(0, 5) + resource / 10;
                int lost = 0;
                for (int i = 0; i < resource && i < steps; i++)
                {
                  if (deciderStep(flags)) lost++;
                }
                resource -= lost;
                //Debug.Log("Steps: " + steps);
                //Debug.Log("Resource: " + resource);
            }
        }

        public bool deciderStep(bool[] flags)
        {
            //Debug.Log("deciderStep");
            if (firstPrior.Count > 0 && flags[0])
            {
                int i = firstPrior[Random.Range(0, firstPrior.Count)];
                Position pos = virtualizedPoints[i].getPosition();
                addPoints.Add(pos);
                pointsManager.bringToLive(pos, teamNumber, generation);
                virtualizeAndImpactPoint(pos);
                clearPrior();
                selector();
                return true;
            }
            else
            if (secondPrior.Count > 0 && flags[1])
            {
                int i = secondPrior[Random.Range(0, secondPrior.Count)];
                Position pos = virtualizedPoints[i].getPosition();
                addPoints.Add(pos);
                pointsManager.bringToLive(pos, teamNumber, generation);
                virtualizeAndImpactPoint(pos);
                clearPrior();
                selector();
                return true;
            }
            else
            if (thirdPrior.Count > 0 && flags[2])
            {
                int i = thirdPrior[Random.Range(0, thirdPrior.Count)];
                Position pos = virtualizedPoints[i].getPosition();
                addPoints.Add(pos);
                pointsManager.bringToLive(pos, teamNumber, generation);
                virtualizeAndImpactPoint(pos);
                clearPrior();
                selector();
                return true;
            }
            else
            if (flags[3])
            {
                Position pos = new Position(Random.Range(0, pointsManager.getSize()[0]), Random.Range(0, pointsManager.getSize()[1]));
                addPoints.Add(pos);
                pointsManager.bringToLive(pos, teamNumber, generation);
                virtualizeAndImpactPoint(pos);
                clearPrior();
                selector();
                return true;
            }
            else
                return false;
        }

        public List<Position> turn()
        {
            addPoints.Clear();
            virtualizeAndImpact();
            selector();
            decider();
            clear();
            pointsOperator.cleaner(pointsManager);
            //Debug.Log(addPoints.Count);
   //         for (int i = 0; i < addPoints.Count; i++)
  //            Debug.Log(addPoints[i].getX() + " " + addPoints[i].getY());
            //Debug.Log(pointsManager.getVirtualizedPoints().Count);
            Debug.Log("Живые летки"+ teamNumber +": "+pointsManager.getAlivePoints().Count);
            return addPoints;
        }

        public void virtualizeAndImpactPoint(Position pos)
        {
            pointsOperator.virtualizeOnePointForComputer(pointsManager ,pos);
            pointsOperator.impactOnePointForComputer(pointsManager, pos);
            List<Position> virtPoints = pointsManager.getVirtualizedPoints();
            for (int i = checkVirtPoints; i < virtPoints.Count; i++)
            {
                List<Point> virtPoint = pointsManager.getVirtualPoints(virtPoints[i]);
                for (int j = 0; j < virtPoint.Count; j++)
                {
                    if (virtPoint[j].getTeam() == teamNumber && virtPoint[j].getGeneration() == generation)
                    {
                        virtualizedPoints.Add(virtPoint[j]);
                    }
                    break;
                }
            }
            checkVirtPoints = virtPoints.Count;
        }

        public void clearPrior()
        {
            firstPrior.Clear();
            secondPrior.Clear();
            thirdPrior.Clear();
        }

        public void clear()
        {
            numberOfPointsBefore = numberOfPointsNow + addPoints.Count;
            clearPrior();
            virtualizedPoints.Clear();
            checkVirtPoints = 0;
        }

        public bool checkFirstRule(Position pos, Point point)
        {
            bool result = false;
            bool flag = false;
            float mask = 0;
           // Debug.Log("Begin1");
           // Debug.Log(pos.getX() + " " + pos.getY());
            Position pos1 = pointsManager.movePosition(pos, 1, 0);
           // Debug.Log(pos1.getX() + " " + pos1.getY());
            List<Point> virtPoint = pointsManager.getVirtualPoints(pos1);
            for (int i = 0; i < virtPoint.Count; i++)
            {
                if (virtPoint[i].getTeam() == teamNumber && virtPoint[i].getGeneration() == generation)
                {
                    flag = true;
                    mask = virtPoint[i].getCharacteristics()[0].getLivePointMask();
              //     Debug.Log(mask);
                    break;
                }
            }
            if (flag && mask > 0)
            {
                flag = false;
                pos1 = pointsManager.movePosition(pos, -1, 0);
           //         Debug.Log(pos1.getX() + " " + pos1.getY());
                virtPoint = pointsManager.getVirtualPoints(pos1);
                for (int i = 0; i < virtPoint.Count; i++)
                {
                    if (virtPoint[i].getTeam() == teamNumber && virtPoint[i].getGeneration() == generation
                        && virtPoint[i].getCharacteristics()[0].getLivePointMask() == mask)
                    {
                        flag = true;
                        mask = virtPoint[i].getCharacteristics()[0].getLivePointMask();
            //              Debug.Log(mask);
                        break;
                    }
                }
            }
            else
                flag = false;
            if (flag) result = true;
            else
            {
                mask = 0;
                pos1 = pointsManager.movePosition(pos, 0, 1);
     //           Debug.Log(pos1.getX() + " " + pos1.getY());
                virtPoint = pointsManager.getVirtualPoints(pos1);
                for (int i = 0; i < virtPoint.Count; i++)
                {
                    if (virtPoint[i].getTeam() == teamNumber && virtPoint[i].getGeneration() == generation)
                    {
                        flag = true;
                        mask = virtPoint[i].getCharacteristics()[0].getLivePointMask();
      //                  Debug.Log(mask);
                        break;
                    }
                }
                if (flag && mask > 0)
                {
                    flag = false;
                    pos1 = pointsManager.movePosition(pos, 0, -1);
     //               Debug.Log(pos1.getX() + " " + pos1.getY());
                    virtPoint = pointsManager.getVirtualPoints(pos1);
                    for (int i = 0; i < virtPoint.Count; i++)
                    {
                        if (virtPoint[i].getTeam() == teamNumber && virtPoint[i].getGeneration() == generation
                            && virtPoint[i].getCharacteristics()[0].getLivePointMask() == mask)
                        {
                            flag = true;
                            mask = virtPoint[i].getCharacteristics()[0].getLivePointMask();
      //                      Debug.Log(mask);
                            break;
                        }
                    }
                }
                else
                    flag = false;
                if (flag) result = true;
            }
      //      Debug.Log("End1");
    //        if (result)
    //            Debug.Log(pos.getX() + "#" + pos.getY());
            return result;
        }
    }
}
