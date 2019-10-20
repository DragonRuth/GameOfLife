using UnityEngine;
using System.Collections;
using Classes.Game.GenerationsPropertiesTableSpace;
using Classes.GameClasses.FuncManegerSpace;
using Classes.GameClasses.PointSpace;
using System.Collections.Generic;
//using System;
//using System;

namespace Classes.GameClasses.PropertiesSpace
{
	[System.Serializable]
    public abstract class Property
    {
        //protected static int numberOfProperties = 0;
        protected byte typeOfCulcing;

        protected string name;
        protected string description;
        protected int id;
        protected bool mustHave;

        public Property(int number ,string nam, string desc, bool mustH)
        {
            id = number;
            //numberOfProperties++;
            name = nam;
            description = desc;
            mustHave = mustH;

        }

        public static void clear()
        {
           // numberOfProperties = 0;
        }

        public abstract float checkLive(float input);
        public abstract float getLivePoint();
        public abstract CharacteristicMask createMask();
        public abstract CharacteristicMask createZeroMask();
        public abstract void setFunction(Function func);
        public abstract void impact(Point point, float power, bool enemy);

        public string[] getNameAndDescription()
        {
            string[] result = { name, description };
            return result;
        }
        public byte getType()
        {
            return typeOfCulcing;
        }
        public bool getMustHave()
        {
            return mustHave;
        }
       /* public static int getNumberOfProperties()
        {
            return numberOfProperties;
        }*/

		public int getNumber()
		{
			return id;
		}

		public void decNumber() 
		{
			id--;
		}

		public abstract Function getFunction();

        ~Property()
        {
            //numberOfProperties--;
        }

        public abstract int getFunctionNumber();
    }
	[System.Serializable]
    public class StaticalProperty : Property
    {
        private float[][] liveInterval;
        private float[][] deadInterval;
        private float livePoint;
        private float power;
        private FunctionStatAndDynam function;

        public StaticalProperty(int number, string nam, string desc,bool mustH ,float[][] liveInt,
            float[][] deadInt, float onePointPower = 1, float liveP = 0) : base(number, nam, desc, mustH) 
        {
            liveInterval = liveInt;
            deadInterval = deadInt;
            livePoint = liveP;
            power = onePointPower;
            function = null;
            typeOfCulcing = 0;
        }

        public override float checkLive(float input)
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

        public override float getLivePoint() { return livePoint; }

        public override CharacteristicMask createMask()
        {
            return new StaticalCharMask(getLivePoint(), this);
        }

        public override CharacteristicMask createZeroMask()
        {
            return new StaticalCharMask(getLivePoint(), this);
        }

        public override void setFunction(Function func)
        {
            function = (FunctionStatAndDynam)func;
        }

        public override void impact(Point point ,float power, bool enemy)
        {
            if (function != null)
                function.impactPoint(point, power, enemy);
        }

        public float getPower()
        {
            return power;
        }

        public override int getFunctionNumber()
        {
            if (function != null)
                return function.getIndex();
            else return -1;
        }

        public float[][] getLiveInterval()
        {
            return liveInterval;
        }

        public float[][] getDeadInterval()
        {
            return deadInterval;
        }

		public void reset(string nam, string desc, bool mustH, float[][] liveInt,
            float[][] deadInt, float onePointPower = 1, float liveP = 0)
        {
            liveInterval = liveInt;
            deadInterval = deadInt;
            livePoint = liveP;
            power = onePointPower;
            mustHave = mustH;
			name = nam;
			description = desc;
        }

		public override Function getFunction() 
		{
			return function;
		}
    }
	[System.Serializable]
    public class DynamicalProperty : Property
    {
        private float livePoint;
        private float setPoint;
        private FunctionStatAndDynam function;

        public DynamicalProperty(int number, string nam, string desc, bool mustH, float liveP = 0, float setP = 0) : base(number, nam, desc, mustH)
        {
            livePoint = liveP;
            function = null;
            setPoint = setP;
            typeOfCulcing = 1;
        }

        public override float getLivePoint()
        {
            return livePoint;
        }

        public override float checkLive(float input)
        {
            if (input < livePoint) return -1;
            else if (input == livePoint) return 0;
            else return 1;
        }

        public override CharacteristicMask createMask()
        {
            return new DynamicalCharMask(setPoint, this);
        }

        public override CharacteristicMask createZeroMask()
        {
            return new DynamicalCharMask(setPoint, this);
        }

        public override void setFunction(Function func)
        {
            function = (FunctionStatAndDynam)func;
        }

        public override void impact(Point point, float power, bool enemy)
        {
            if (function != null)
                function.impactPoint(point, power, enemy);
        }

        public float getSetPoint()
        {
            return setPoint;
        }

        public override int getFunctionNumber()
        {
            if (function != null)
                return function.getIndex();
            else return -1;
        }

		public void reset(string nam, string desc, bool mustH, float liveP = 0, float setP = 0)
        {
            livePoint = liveP;
            setPoint = setP;
            mustHave = mustH;
			name = nam;
			description = desc;
        }

		public override Function getFunction() 
		{
			return function;
		}
      
    }
	[System.Serializable]
    public class CollectionalProperty : Property
    {
        private string[] collection;
        private FunctionCollectional function;

        public CollectionalProperty(int number, string nam, string desc, bool mustH, string[] coll) : base(number, nam, desc, mustH)
        {
            collection = coll;
            function = null;
            typeOfCulcing = 2;
        }

        public override float checkLive(float input)
        {
            if (input == collection.Length) return 1;
            else return -1;
        }

        public override float getLivePoint()
        {
            return collection.Length;
        }

        public override CharacteristicMask createMask()
        {
            return new CollectionalCharMask(getLivePoint(), this);
        }

        public override CharacteristicMask createZeroMask()
        {
            return new CollectionalCharMask(getLivePoint(), this, false);
        }

        public override void setFunction(Function func)
        {
			//Debug.Log("ffunc"+func.getIndex ());
            function = (FunctionCollectional)func;
			//Debug.Log("function"+function.getIndex ());
        }

        public override void impact(Point point, float power, bool enemy)
        {
            if (function != null)
                function.impactPoint(point, power, enemy);
        }

        public override int getFunctionNumber()
        {
            if (function != null)
                return function.getIndex();
            else return -1;
        }

        public string[] getCollection()
        {
            return collection;
        }

		public void reset(string nam, string desc, bool mustH, string[] coll)
        {
            collection = coll;
            mustHave = mustH;
			name = nam;
			description = desc;
        }

		public override Function getFunction() 
		{
			return function;
		}
    }
	[System.Serializable]
    public abstract class CharacteristicMask
    {
        //Переменные
        //Конструктор
        //Методы
        public abstract bool getMustHave();
        public abstract byte getType();
        public abstract Property getProperty();
        public abstract float checkLive();
        public abstract void reset();
        public abstract void set(float input);
        public abstract float getLivePointMask();
        public abstract void suffer(float input);
        public abstract void impactPoint(Point point, bool enemy);
    }
	[System.Serializable]
    public class StaticalCharMask : CharacteristicMask
    {
        //Переменные
        private StaticalProperty property;
        private float livePoint;

        //Конструктор 
        public StaticalCharMask(float liveP, StaticalProperty prop)
        {
            livePoint = liveP;
            property = prop;
        }

        public override float checkLive()
        {
            return property.checkLive(livePoint);
        }

        public override void reset()
        {
            livePoint = property.getLivePoint();
        }

        public override void set(float input)
        {
            livePoint = input;
        }

        public override float getLivePointMask()
        {
            return livePoint;
        }

        public override void suffer(float input)
        {
            livePoint += input;
        }

        public override byte getType()
        {
            return property.getType();
        }

        public override Property getProperty()
        {
            return property;
        }

        public override void impactPoint(Point point, bool enemy)
        {
            property.impact(point, property.getPower() , enemy);
        }

        public override bool getMustHave()
        {
            return property.getMustHave();
        }
    }
	[System.Serializable]
    public class DynamicalCharMask : CharacteristicMask
    {
        private float livePoint;
        private DynamicalProperty property;

        public DynamicalCharMask(float liveP, DynamicalProperty prop)
        {
            livePoint = liveP;
            property = prop;
        }

        public override float checkLive()
        {
            return property.checkLive(livePoint);
        }

        public override void reset()
        {
            livePoint = property.getSetPoint();
        }

        public override void set(float input)
        {
            livePoint = input;
        }

        public override float getLivePointMask()
        {
            return livePoint;
        }

        public override void suffer(float input)
        {
            livePoint += input;
        }

        public override byte getType()
        {
            return property.getType();
        }

        public override Property getProperty()
        {
            return property;
        }

        public override void impactPoint(Point point, bool enemy)
        {
            property.impact(point, livePoint, enemy);
        }

        public override bool getMustHave()
        {
            return property.getMustHave();
        }
    }
	[System.Serializable]
    public class CollectionalCharMask : CharacteristicMask
    {
        private byte[] collection;
        private int peaceOfCollection;
        private CollectionalProperty property;

        public CollectionalCharMask(float input, CollectionalProperty prop, bool zero = true)
        {
            property = prop;
            collection = new byte[(int)input];
            for (int i = 0; i < input; i++)
                collection[i] = 0;
            if (zero != false)
            {
                peaceOfCollection = Random.Range(1, collection.Length + 1);
                //Debug.Log(peaceOfCollection);
                collection[peaceOfCollection - 1] = 1;
            }
        }

        public override float checkLive()
        {
            int number = 0;
            for (int i = 0; i < collection.Length; i++)
                if (collection[i] > 0) number++;
            return property.checkLive(number);
        }

        public override void reset()
        {
            for (int i = 0; i < collection.Length; i++)
                collection[i] = 0;
            collection[peaceOfCollection - 1] = 1;
        }

        public override void set(float input)
        {
            collection[peaceOfCollection - 1] = 0;
            peaceOfCollection = (int)input;
            collection[(int)input - 1] = 1;
        }
        public override float getLivePointMask()
        {
            return peaceOfCollection;
        }

        public override void suffer(float input)
        {
            if (input <= collection.Length) {
                if (input >= 1)
                    collection[(int)input - 1]++;
                else if (input <= -1)
                    collection[(int)(-input) - 1]--;
            }
        }

        public override byte getType()
        {
            return property.getType();
        }

        public override Property getProperty()
        {
            return property;
        }

        public override void impactPoint(Point point, bool enemy)
        {
            property.impact(point, peaceOfCollection, enemy);
        }

        public override bool getMustHave()
        {
            return property.getMustHave();
        }
    }
}