/*using UnityEngine;
using System.Collections;
*/
using System;
using Classes.Game.GenerationsPropertiesTableSpace;

/*namespace Classes.GameClasses.PropertySpace
{

    public enum typesOfCulcing
    {
        StaticalChar, //Свойству присваивается константное значение
        DynamicalChar, //При оживлении клетки ее свойства будут формироваться из свойств ее родителей
        CollectionalChar //Обрабатывается как коллекция
    }

    public class Property
    {

        //Описание переменных
        private static int numberOfProperties = 0;
        typesOfCulcing typeOfCulcing;

        private string name;
        private string description;
        private int id;
        

        private Characteristic characteristic;

        //Конструкторы
        public Property(int[][] aliveInt, int[][] deadInt, int onePointPower,int liveP, string nam, string desc)
        {
            typeOfCulcing = (typesOfCulcing)0;
            characteristic = new StaticalChar(aliveInt, deadInt, onePointPower, liveP);
            name = nam;
            description = desc;
            id = numberOfProperties;
            numberOfProperties++;
        }

        public Property(int liveP, string nam, string desc, GenerationsPropertiesTable gptInput)
        {
            typeOfCulcing = (typesOfCulcing)1;
            characteristic = new DynamicalChar(gptInput, liveP);
            name = nam;
            description = desc;
            id = numberOfProperties;
            numberOfProperties++;
        }

        public Property(string[] coll, string nam, string desc)
        {
            typeOfCulcing = (typesOfCulcing)2;
            characteristic = new CollectionalChar(coll);
            name = nam;
            description = desc;
            id = numberOfProperties;
            numberOfProperties++;
        }

        //Методы 
        public float checkLive(int input)
        {
            return characteristic.checkLive(input);
        }
        public int getLivePoint() { return characteristic.getLivePoint(); }
        public CharacteristicMask createMask()
        {
            if (characteristic != null)
            {
                if ((int)typeOfCulcing == 0) return new StaticalCharMask(getLivePoint(), this);
                else if ((int)typeOfCulcing == 1) return new DynamicalCharMask(getLivePoint(), this);
                else return new CollectionalCharMask(getLivePoint(), this);
            }
            else return null;
        }
        public CharacteristicMask createZeroMask()
        {
            if (characteristic != null)
            {
                if ((int)typeOfCulcing == 0) return new StaticalCharMask(0, this);
                else if ((int)typeOfCulcing == 1) return new DynamicalCharMask(0, this);
                else return new CollectionalCharMask(getLivePoint(), this, false);
            }
            else return null;
        }

        public string[] getNameAndDescription()
        {
            string[] result = { name, description };
            return result;
        }

        public int getType() { return (int)typeOfCulcing; }
        public int impact() { return characteristic.impact(); }
    }

    public abstract class Characteristic
    {
        //Методы общие для всех трех видов хврвктеристик
        public abstract float checkLive(int input);
        public abstract int getLivePoint();
        public abstract int impact();
    }

    public class StaticalChar : Characteristic
    {
        //Описание переменных
        private int[][] liveInterval;
        private int[][] deadInterval;
        private int livePoint;
        private int power;          

        //Конструктор
        public StaticalChar(int[][] liveInt, int[][] deadInt,int onePointPower = 1, int liveP = 0)
        {
            liveInterval = liveInt;
            deadInterval = deadInt;
            livePoint = liveP;
        }
        //Методы
        public override float checkLive(int input)
        {
            int result = 0;
            for (int i = 0; i < liveInterval.Length; i++)
            {
                if (input >= liveInterval[i][0] && input <= liveInterval[i][1])
                {
                    result = 1;
                    break;
                }
            }
            if (result == 0)
            {
                for (int i = 0; i < deadInterval.Length; i++)
                {
                    if (input >= deadInterval[i][0] && input <= deadInterval[i][1])
                    {
                        result = -1;
                        break;
                    }
                }
            }
            return result;
        }
        public override int getLivePoint() { return livePoint; }
        public override int impact()
        {
            return power;
        }
    }

    public class DynamicalChar : Characteristic
    {
        //Описание переменных
        private int livePoint;
        private GenerationsPropertiesTable gpt;
        //Конструктор
        public DynamicalChar(GenerationsPropertiesTable gptInput, int liveP = 0)
        {
            livePoint = liveP;
            gpt = gptInput;
        }
        //Методы
        public override float checkLive(int input)
        {
            if (input < livePoint) return -1;
            else return input / livePoint;
        }
        public override int getLivePoint()
        {
            return livePoint;
        }
        public override int impact()
        {
            return 5;  //тут будет фигурировать функция
        }
    }

    public class CollectionalChar : Characteristic
    {
        //Описание переменных
        private string[] collection;
        //Конструктор
        public CollectionalChar(string[] coll)
        {
            collection = coll;
        }
        //Методы
        public override float checkLive(int input)
        {
            if (input == collection.Length) return 1;
            else return -1;
        }

        public override int getLivePoint()
        {
            return collection.Length;
        }
        public override int impact()
        {
            return 0; //Кусочек коллекции содержится в маске
        }
    }

    public abstract class CharacteristicMask
    {
        //Переменные
        protected Property property;
        //Конструктор
        public CharacteristicMask(Property prop)
        {
            property = prop;
        }
        //Методы
        public int getType() { return property.getType(); }
        public Property getProperty()
        {
            return property;
        }
        public abstract float checkLive();
        public abstract void reset();
        public abstract void set(int input);
        public abstract int getLivePoint();
        public abstract void suffer(float input);
    }

    public class StaticalCharMask : CharacteristicMask
    {
        //Переменные
        private int livePoint;

        //Конструктор 
        public StaticalCharMask(int liveP, Property prop) : base(prop)
        {
            livePoint = liveP;
        }

        public override float checkLive()
        {
            return property.checkLive(livePoint);
        }

        public override void reset()
        {
            livePoint = property.getLivePoint();
        }

        public override void set(int input)
        {
            livePoint = input;
        }

        public override int getLivePoint()
        {
            return livePoint;
        }

        public override void suffer(float input)
        {
            livePoint += input;
        }
    }

    public class DynamicalCharMask : CharacteristicMask
    {
        private int livePoint;

        public DynamicalCharMask(int liveP, Property prop) : base(prop)
        {
            livePoint = liveP;
        }

        public override float checkLive()
        {
            return property.checkLive(livePoint);
        }

        public override void reset()
        {
            livePoint = property.getLivePoint();
        }

        public override void set(int input)
        {
            livePoint = input;
        }

        public override int getLivePoint()
        {
            return livePoint;
        }

        public override void suffer(float input)
        {
            livePoint += input;
        }
    }

    public class CollectionalCharMask : CharacteristicMask
    {
        private int[] collection;
        private int peaceOfCollection;

        public CollectionalCharMask(int input,Property prop, bool zero = true) : base(prop)
        {
            collection = new int[input];
            for (int i = 0; i < input; i++)
                collection[i] = 0;
            if (zero != false)
            {
                Random rnd = new Random();
                peaceOfCollection = rnd.Next(0, collection.Length - 1);
                collection[peaceOfCollection]++;
            }
        }

        public override float checkLive()
        {
            int number = 0;
            for (int i = 0; i < collection.Length; i++)
                if (collection[i] != 0) number++;
            return property.checkLive(number);
        }

        public override void reset()
        {
            for (int i = 0; i < collection.Length; i++)
                collection[i] = 0;
            collection[peaceOfCollection]++;
        }

        public override void set(int input)
        {
            collection[input - 1]+= 1;
        }
        public override int getLivePoint()
        {
            return peaceOfCollection;
        }

        public override void suffer(float input)
        {
            collection[input - 1] += 1;
        }
    }
}*/