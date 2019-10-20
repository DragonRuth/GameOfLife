using UnityEngine;
using System.Collections;
using Classes.GameClasses.PropertiesSpace;
using Classes.Game.GenerationsPropertiesTableSpace;
using System.Collections.Generic;

namespace Classes.GameClasses.PointSpace
{
	[System.Serializable]
    public class Point
    {
        //Опсание переменных
        private Position position;
        private int aliveAndTeam;
        private int age;
        private Generation generation;
        private List<CharacteristicMask> characteristics;
        
        //Кнструкторы
        public Point(Position pos)
        {
            position = pos;
            age = 0;
            aliveAndTeam = 0;
            generation = null;
            characteristics = new List<CharacteristicMask>();
        } 

        public void bringToLife(int teamNumber, Generation gen)
        {
            aliveAndTeam = teamNumber;
            generation = gen;
            age = 0;
            characteristics = new List<CharacteristicMask>();
            for (int i = 0; i < generation.getProperties().Count; i++)
                characteristics.Add(generation.getProperties()[i].createMask());
        }

        public void correctGeneration()
        {
            characteristics = null;
            characteristics = new List<CharacteristicMask>();
            for (int i = 0; i < generation.getProperties().Count; i++)
                characteristics.Add(generation.getProperties()[i].createMask());
        }

        public void createVirtualPoint(int teamNumber, Generation gen)
        {
            aliveAndTeam = teamNumber;
            generation = gen;
            characteristics = new List<CharacteristicMask>();
            for (int i = 0; i < generation.getProperties().Count; i++)
                characteristics.Add(generation.getProperties()[i].createZeroMask());
        }

        public void kill()
        {
            aliveAndTeam = 0;
            age = 0;
            generation = null;
            characteristics = null;
        }

        public void incAge()
        {
            age++;
        }

        public int getAge()
        {
            return age;
        }

        public List<CharacteristicMask> getCharacteristics()
        {
            return characteristics;
        }

        public void impcatPoint(Point point)
        {
            bool flag = enemy(point);
            for (int i = 0; i < characteristics.Count; i++)
            {
                characteristics[i].impactPoint(point, flag);
            }
        }

        public int getTeam()
        {
            return aliveAndTeam;
        }

        public Generation getGeneration()
        {
            return generation;
        }

        public bool enemy(Point point)
        {
            if (point.getGeneration() != generation || point.getTeam() != aliveAndTeam) return true;
            else return false;
        }

        public Position getPosition()
        {
            return position;
        }

        public void reset()
        {
            for (int i = 0; i < characteristics.Count; i++)
                characteristics[i].reset();
        }

        public bool getImmortale()
        {
            return generation.getImmortale();
        }
    }
	[System.Serializable]
    public class Position
    {
        private int x;
        private int y;

        public Position(int x0, int y0)
        {
            x = x0;
            y = y0;
        }

        public Position()
        {
            x = 0;
            y = 0;
        }

        public int[] getPosition()
        {
            int[] result = { x, y };
            return result;
        }
        public void set(int x0, int y0)
        {
            x = x0;
            y = y0;
        }
        public int getX() { return x; }
        public int getY() { return y; }

        public bool compare(Position pos)
        {
            bool flag = false;
            if (pos.getX() == x && pos.getY() == y) flag = true;
            return flag;
        }

        public Position copy()
        {
            return new Position(x, y);
        }

        public void incX(int input)
        {
            x += input; 
        }

        public void incY(int input)
        {
            y += input;
        }
    }
}
