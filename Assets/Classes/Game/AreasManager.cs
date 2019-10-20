using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Classes.GameClasses.PointSpace;
using Classes.Game.GenerationsPropertiesTableSpace;
[System.Serializable]
public class AreasManager {
    //Переменные
    private Point[][] points;
    private List<Point>[][] virtualPoints;
    private List<Position> alivePoints;
    private List<Position> virtualizedPoints;
    private int sizeX;
    private int sizeY;
    //Конструктор
    public AreasManager(int x, int y)
    {
        sizeX = x;
        sizeY = y;
        alivePoints = new List<Position>();
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
    }

    public void kill(Position pos)
    {
        points[pos.getX()][pos.getY()].kill();
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

    public void deleteVirtualPoint(Position pos, Generation gen, int teamNum)
    {
        int size = virtualPoints[pos.getX()][pos.getY()].Count;
        int i = 0;
        for (; i < size; i++)
        {
            if (virtualPoints[pos.getX()][pos.getY()][i].getTeam() == teamNum &&
                virtualPoints[pos.getX()][pos.getY()][i].getGeneration() == gen)
                break;
        }
        virtualPoints[pos.getX()][pos.getY()].RemoveAt(i);
        i = 0;
        for (; i < virtualizedPoints.Count; i++)
            if (virtualizedPoints[i].getX() == pos.getX() && virtualizedPoints[i].getY() == pos.getY())
                break;
        if (i < virtualizedPoints.Count)
            virtualizedPoints.RemoveAt(i);
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
        Point target = this.getPoint(pos);
        Point result = new Point(pos);
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
}
