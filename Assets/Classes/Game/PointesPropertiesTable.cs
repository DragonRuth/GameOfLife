using UnityEngine;
/*
using System.Collections;
using Classes.GameClasses.Property;
using Classes.GameClasses.Point;*/

using System;
using System.Collections.Generic;
using Classes.GameClasses.PropertiesSpace;

namespace Classes.Game.GenerationsPropertiesTableSpace
{
	[System.Serializable]
    public class GenerationsPropertiesTable
    {
        //Переменные
        private List<Property> allProperties;
        private List<Generation> allGenerations;

        //Конструкторы
        public GenerationsPropertiesTable()
        {
            allProperties = new List<Property>();
            allGenerations = new List<Generation>();
        }

        public void clear()
        {
            allProperties.Clear();
            allGenerations.Clear();
            Property.clear();
            Generation.clear();
        }

        public void createGeneration(string nm, string  desc, int[] numbers, bool immort)
        {
            List<Property> prop = new List<Property>();
            bool flag = true;
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] < getNumberOfProperties())
                    prop.Add(allProperties[numbers[i]]);
                else
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
                allGenerations.Add(new Generation(allGenerations.Count ,nm, desc, prop, immort));
            else Debug.Log("The number of property is out range.(createGeneration())");
        }

        public void createGenerationWithoutProp(string nm, string desc)
        {
            allGenerations.Add(new Generation(allGenerations.Count, nm, desc));
        }

        public void addPropertyToGeneration(int numberOfGeneration, int numberOfProperty)
        {
            if (numberOfProperty < getNumberOfProperties() && numberOfGeneration < allGenerations.Count)
                allGenerations[numberOfGeneration].addProperty(allProperties[numberOfProperty]);
        }

        public void setImmortale(int numberOfGeneration, bool immortale)
        {
            if (numberOfGeneration < allGenerations.Count)
                allGenerations[numberOfGeneration].setImmortale(immortale);
        }

        public void createStaticalProperty(string nam, string desc, float[][] liveInt, float[][] deadInt, 
            float onePointPower = 1, float liveP = 0 , bool mustH = false)
        {
            StaticalProperty prop = new StaticalProperty(allProperties.Count, nam, desc, mustH, liveInt, deadInt, onePointPower, liveP);
            allProperties.Add(prop);
        }

        public void createDynamicalProperty(string nam, string desc, float liveP = 0, float setP = 0,bool mustH = false)
        {
            DynamicalProperty prop = new DynamicalProperty(allProperties.Count, nam, desc, mustH, liveP, setP);
            allProperties.Add(prop);
        }

        public void createCollectionalProperty(string nam, string desc, string[] coll, bool mustH = false)
        {
            CollectionalProperty prop = new CollectionalProperty(allProperties.Count, nam, desc, mustH, coll);
            allProperties.Add(prop);
        }

		public void resetStaticalProperty(string nam, string desc, int number, float[][] liveInt, float[][] deadInt, 
			float onePointPower, float liveP, bool mustH) 
		{
			StaticalProperty prop = (StaticalProperty)allProperties[number];
			prop.reset (nam, desc, mustH, liveInt, deadInt,onePointPower,liveP);
		}

		public void resetDynamicalProperty(string nam, string desc, int number, float liveP, float setP,bool mustH)
		{
			DynamicalProperty prop = (DynamicalProperty)allProperties[number];
			prop.reset (nam, desc, mustH, liveP, setP);
		}

		public void resetCollectionalProperty(string nam, string desc, int number,string[] coll, bool mustH)
		{
			CollectionalProperty prop = (CollectionalProperty)allProperties[number];
			prop.reset (nam, desc, mustH, coll);
		}

        public List<Property> getPropertiesList() { return allProperties; }
        public Generation getGeneration(int indexGeneration)
        {
            return allGenerations[indexGeneration];
        }

        public Property getProperty(int number)
        {
            if (number < getNumberOfProperties())
                return allProperties[number];
            else
            {
                Debug.Log("The number of property is out range.(getProperty())");
                return null;
            }
        }

        public List<Generation> getAllGenerations()
        {
            return allGenerations;
        }

        public void deleteGeneration(int number)
        {
			int numGen = allGenerations.Count;
			for (int i = number + 1; i < numGen; i++)
				allGenerations [i].decNumber ();
            allGenerations.RemoveAt(number);
        }

        public void deleteProperty(int number)
        {
			int numProp = getNumberOfProperties ();
			for (int i = number + 1; i < numProp; i++)
				allProperties [i].decNumber ();
            allProperties.RemoveAt(number);
        }

		public int getNumberOfProperties() 
		{
			return allProperties.Count;
		}

		public Property getLastProperty()
		{
			return allProperties[getNumberOfProperties() - 1];
		}
			
    }
	[System.Serializable]
    public class Generation
    {
        //private static int numberOfGenerations = 0;
        //Переменные
        private string name;
        private string description;
        private int generationNumber;
        private List<Property> properties;
        private bool immortale;
        //Конструкторы
        public Generation(int number,string nm, string desc, List<Property> prop, bool immort)
        {
            generationNumber = number;
            name = nm;
            description = desc;
            immortale = immort;
            properties = new List<Property>();
            for (int i = 0; i < prop.Count; i++)
                properties.Add(prop[i]);
        }

        public Generation(int number, string nm, string desc)
        {
            generationNumber = number;
            name = nm;
            description = desc;
            immortale = false;
            properties = new List<Property>();
            Debug.Log("Generation created;");
        }

        public static void clear()
        {
            //numberOfGenerations = 0;
        }

        public void setImmortale(bool input)
        {
            immortale = input;
        }

        public void addProperty(Property prop)
        {
            properties.Add(prop);
        }

        public List<Property> getProperties()
        {
            return properties;
        }

        public bool getImmortale()
        {
            return immortale;
        }

        public string[] getNameAndDescription()
        {
            string[] result = { name, description};
            return result;
        }

        public void removeProperty(int propertyNumber)
        {
            properties.RemoveAt(propertyNumber);
        }

        public int getNumber()
        {
            return generationNumber;
        }

		public void decNumber()
		{
			generationNumber--;	
		}

        ~Generation()
        {
            //numberOfGenerations--;
        }
    }
}
