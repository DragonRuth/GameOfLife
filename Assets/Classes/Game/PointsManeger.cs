﻿using UnityEngine;
using System.Collections;


using System;
using System.Collections.Generic;
using Classes.GameClasses.PointSpace;
using Classes.Game.GenerationsPropertiesTableSpace;

namespace Classes.Game.PointsManagerSpace
{
	[System.Serializable]
    public class PointsManager
    {
        //Переменные
        private Point[][] points;
        private List<Point>[][] virtualPoints;
        private List<Position> alivePoints;
        private List<Position> virtualizedPoints;
        private List<List<Point>> savedPoints;
        private int sizeX;
        private int sizeY;
        //Конструктор
        public PointsManager(int x, int y)
        {
            sizeX = x;
            sizeY = y;
            alivePoints = new List<Position>();
            savedPoints = new List<List<Point>>();
            virtualizedPoints = new List<Position>();
            points = new Point[sizeX][];
            for (int i = 0; i < sizeX; i++)
            {
                points[i] = new Point[sizeY];
                for (int j = 0; j < sizeY; j++)
                {
                    Position pos = new Position(i, j);
                    points[i][j] = new Point(pos);
                }
            }
            virtualPoints = new List<Point>[sizeX][];
            for (int i = 0; i < sizeX; i++)
            {
                virtualPoints[i] = new List<Point>[sizeY];
                for (int j = 0; j < sizeY; j++)
                {
                    virtualPoints[i][j] = new List<Point>();
                }
            }
        }
        //Методы
        public Point getPoint(Position pos)
        {
            return points[pos.getX()][pos.getY()];
        }

        public void bringToLive(Position pos, int teamNumber, Generation gen)
        {
            if (getPoint(pos).getTeam() > 0)
                getPoint(pos).kill();
            points[pos.getX()][pos.getY()].bringToLife(teamNumber, gen);
            alivePoints.Add(pos);
            //Debug.Log("bring");
            //Debug.Log(alivePoints.Count);
        }

        public void killTouch(Position pos)
        {
            getPoint(pos).kill();
            //Debug.Log(pos.getX() + " " + pos.getY());
            for (int i = 0; i < alivePoints.Count; i++)
            {
                //Debug.Log(alivePoints[i].getX() + " " + alivePoints[i].getY());
                if (pos.compare(alivePoints[i]))
                {
                    //Debug.Log("Deleted");
                    alivePoints.RemoveAt(i);
                    break;
                }
            }
            //Debug.Log("kill");
            //Debug.Log(alivePoints.Count);
        }

		public void kill(Position pos)
		{
			getPoint(pos).kill();
		}

        public void createVirtualPoint(Position pos, int teamNumber, Generation gen)
        {
            bool flag = true;
            if (virtualPoints[pos.getX()][pos.getY()].Count == 0) virtualizedPoints.Add(pos);
            for (int i = 0; i < virtualPoints[pos.getX()][pos.getY()].Count; i++)
                if (virtualPoints[pos.getX()][pos.getY()][i].getGeneration() == gen &&
                    virtualPoints[pos.getX()][pos.getY()][i].getTeam() == teamNumber)
                {
                    flag = false;
                    break;
                }
            if (flag)
            {
                Point point = new Point(pos);
                //if (itself)
                   // point.bringToLife(teamNumber, gen);
               // else
                point.createVirtualPoint(teamNumber, gen);
                virtualPoints[pos.getX()][pos.getY()].Add(point);
            }
        }

        public void resetPoint(Position pos)
        {
            getPoint(pos).reset();
            getPoint(pos).incAge();
            //Debug.Log("reset");
            alivePoints.Add(pos);
        }

        public void clearVitrualPoints(Position pos)
        {
            virtualPoints[pos.getX()][pos.getY()].Clear();
        }

        public Point[][] getAllPoints()
        {
            return points;
        }

        public List<Position> getAlivePoints()
        {
            return alivePoints;
        }

        public List<Position> getVirtualizedPoints()
        {
            return virtualizedPoints;
        }

        public int[] getSize()
        {
            int[] result = { sizeX, sizeY };
            return result;
        }

        public bool checkVirtualPointEx(Position pos, Generation gen, int teamNum)
        {
            bool result = true;
            int size = virtualPoints[pos.getX()][pos.getY()].Count;
            for (int i = 0; i < size; i++)
            {
                if (virtualPoints[pos.getX()][pos.getY()][i].getTeam() == teamNum &&
                    virtualPoints[pos.getX()][pos.getY()][i].getGeneration() == gen)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public void deleteVirtualPoint(Position pos, Generation gen, int teamNum)
        {
            int size = getVirtualPoints(pos).Count;
            int i = 0;
            bool flag = false;
            for (; i < size; i++)
            {
                if (getVirtualPoints(pos)[i].getTeam() == teamNum &&
                    getVirtualPoints(pos)[i].getGeneration() == gen)
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                virtualPoints[pos.getX()][pos.getY()].RemoveAt(i);
                if (virtualPoints[pos.getX()][pos.getY()].Count == 0)
                {
                    i = 0;
                    for (; i < virtualizedPoints.Count; i++)
                        if (virtualizedPoints[i].getX() == pos.getX() && virtualizedPoints[i].getY() == pos.getY())
                            break;
                    if (i < virtualizedPoints.Count)
                        virtualizedPoints.RemoveAt(i);
                }
            }
           /* i = 0;
            for (; i < virtualizedPoints.Count; i++)
                if (virtualizedPoints[i].getX() == pos.getX() && virtualizedPoints[i].getY() == pos.getY())
                    break;
            if (i < virtualizedPoints.Count)
                virtualizedPoints.RemoveAt(i);*/
        }

        public void addVirtualPoint(Position pos, Point point)
        {
            if (virtualPoints[pos.getX()][pos.getY()].Count == 0) virtualizedPoints.Add(pos);
            virtualPoints[pos.getX()][pos.getY()].Add(point);
        }

        public List<Point>[][] getAllVirtualPoints()
        {
            return virtualPoints;
        }

        public List<Point> getVirtualPoints(Position pos)
        {
            return virtualPoints[pos.getX()][pos.getY()];
        }

        public void impactPoint(Position posImp, Position posSuf)
        {
            Point pointImp = points[posImp.getX()][posImp.getY()];
            List<Point> pointSuf = virtualPoints[posSuf.getX()][posSuf.getY()];
            for (int i = 0; i < pointSuf.Count; i++)
                pointImp.impcatPoint(pointSuf[i]);
        }

        public void clearAlivePoints()
        {
            alivePoints.Clear();
        }

        public Point clone(Position pos)
        {
            Point target = getPoint(pos);
            Point result = new Point(pos);
            //Debug.Log("+");
            //Debug.Log(pos.getX() + " " + pos.getY());
            result.bringToLife(target.getTeam(), target.getGeneration());
            for (int i = 0; i < result.getCharacteristics().Count; i++)
                if (result.getCharacteristics()[i].getType() == 2)
                {
                    result.getCharacteristics()[i].set(target.getCharacteristics()[i].getLivePointMask());
                }
            return result;
        }

        public bool getPointImmortale(Position pos)
        {
            return getPoint(pos).getImmortale();
        }

        public void addVirtualizedPoint(Position pos)
        {
            virtualizedPoints.Add(pos);
        }

        public Position movePosition(Position pos, int dX, int dY)
        {
            int x = pos.getX() - dX;
            if (x < 0) x = sizeX + x;
            else if (x >= sizeX) x = x - sizeX;
            int y = pos.getY() - dY;
            if (y < 0) y = sizeY + y;
            else if (y >= sizeY) y = y - sizeY;
            return new Position(x, y);
        }

        public void addAlivePoint(Position pos)
        {
            alivePoints.Add(pos);
        }

        public bool comparePoints(Position pos, Generation gen, int team)
        {
            bool result = false;
            Point point = getPoint(pos);
            if (point.getGeneration() == gen && point.getTeam() == team)
                result = true;
            return result;
        }

        public void correctGeneration(Generation gen)
        {
            for (int i = 0; i < alivePoints.Count; i++)
                if (getPoint(alivePoints[i]).getGeneration() == gen)
                    getPoint(alivePoints[i]).correctGeneration();
        }

        public int checkLive(Position pos)
        {
            return getPoint(pos).getTeam();
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
            List<Point> lastSave = new List<Point>();
            for (int i = 0; i < alivePoints.Count; i++)
            {
                lastSave.Add(clone(alivePoints[i]));
            }
            savedPoints.Add(lastSave);
        }

        public List<Point> getLastSavePoints(bool deleteLast = false)
        {
            List<Point> result = savedPoints[savedPoints.Count - 1];
            if (deleteLast)
                savedPoints.RemoveAt(savedPoints.Count - 1);
            return result;
        }

        public List<Point> getSavePoints(int number ,bool delete = false)
        {
            if (number < savedPoints.Count)
            {
                List<Point> result = savedPoints[savedPoints.Count - number];
                if (delete)
                    savedPoints.RemoveAt(savedPoints.Count - number);
                return result;
            }
            else
                return null;
        }

    }
}
