using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
using Classes.GameClasses.PropertiesSpace;
using Classes.GameClasses.PointSpace;

namespace Classes.GameClasses.FuncManegerSpace
{
	[System.Serializable]
    public class FunctionManager
    {
        private List<Function> allFunctions;

        public FunctionManager()
        {
            allFunctions = new List<Function>();
        }

        public void clear()
        {
            allFunctions.Clear();
        }

        public void setAllFunctions(List<Property> prop)
        {
            if (prop.Count >= allFunctions.Count) {
                for (int i = 0; i < allFunctions.Count; i++)
                    prop[i].setFunction(allFunctions[i]);
            } else
            {
                for (int i = 0; i < prop.Count; i++)
                    prop[i].setFunction(allFunctions[i]);
            }
        }
        public Function getFunction(int number)
        {
            return allFunctions[number];
        }
        public List<Function> getAllFunctions()
        {
            return allFunctions;
        }
        public void addFunctionToProperty(Property prop, int index)
		{
            Debug.Log(prop.getNumber());
			if (allFunctions [index] == null)
				prop.setFunction (allFunctions [index]);
			else if (((prop.getType () == 0 || prop.getType () == 1) && allFunctions [index].getType () == false)
			              || (prop.getType () == 2 && allFunctions [index].getType () == true))
			{
				Debug.Log ("func"+index);
				Debug.Log ("allFunctions"+allFunctions.Count);
				prop.setFunction (allFunctions [index]);
			}
        }
        public void createFunction(Property[] prop, float[] coefFr, float[] coefEn)
        {
			if (prop.Length > 0) {
				bool flag = false;
				Function func;
				for (int i = 0; i < prop.Length; i++)
					if (prop [i].getType () == 0 || prop [i].getType () == 1)
						flag = true;
					else
					{
						flag = false;
						break;
					}
				if (flag) 
				{
					func = new FunctionStatAndDynam (allFunctions.Count ,prop, coefFr, coefEn);
					allFunctions.Add (func);
				} else
				{
					for (int i = 0; i < prop.Length; i++)
						if (prop [i].getType () == 2)
							flag = true;
						else 
						{
							flag = false;
							break;
						}
					if (flag == true)
					{
						Debug.Log ("Number of func:" +getNumberOfFunctions());
						func = new FunctionCollectional (allFunctions.Count ,prop [0], coefFr [0], coefEn [0]);
						Debug.Log ("Number of func:" + getNumberOfFunctions());
						allFunctions.Add (func);
						//Debug.Log (prop [0].getNumber().ToString() + coefFr [0].ToString() + coefEn [0].ToString());
					} else
						Debug.Log ("The function can not de created.(createFunction())");
				}
			} else 
			{
				allFunctions.Add (null);
			}	
        }

        public void resetFunction(int number, Property[] prop, float[] coefFr, float[] coefEn)
        {
            if (prop.Length > 0)
            {
                bool flag = false;
                for (int i = 0; i < prop.Length; i++)
                    if (prop[i].getType() == 0 || prop[i].getType() == 1)
                        flag = true;
                    else
                    {
                        flag = false;
                        break;
                    }
                if (flag)
                {
                    if (allFunctions[number] != null)
                    {
                        FunctionStatAndDynam function = (FunctionStatAndDynam)allFunctions[number];
                        function.resetFunction(prop, coefFr, coefEn);
                    }
                    else
                    {
						allFunctions[number] = new FunctionStatAndDynam(number ,prop, coefFr, coefEn);
                    }
                }
                else
                {
                    for (int i = 0; i < prop.Length; i++)
                        if (prop[i].getType() == 2)
                            flag = true;
                        else
                        {
                            flag = false;
                            break;
                        }
                    if (flag == true)
                    {
                        if (allFunctions[number] != null)
                        {
                            FunctionCollectional function = (FunctionCollectional)allFunctions[number];
                            function.resetFunction(prop[0], coefFr[0], coefEn[0]);
							Debug.Log (allFunctions[getNumberOfFunctions()]);
                        }
                        else
                        {
							allFunctions[number] = new FunctionCollectional(number ,prop[0], coefFr[0], coefEn[0]);
                        }
                    }
                    else Debug.Log("The function can not de created.(createFunction())");
                }
            } else
            {
                allFunctions[number] = null;
            }
       }

		public int getNumberOfFunctions()
		{
			return allFunctions.Count;
		}

		public void deleteFunction(int number)
		{
			int numFunc = getNumberOfFunctions ();
			for (int i = number + 1; i < numFunc; i++) 
				if (allFunctions [i] != null)
					allFunctions [i].decNumber();
			allFunctions.RemoveAt (number);
		}
    }
	[System.Serializable]
    public abstract class Function
    {
        protected int index;
        protected bool type;

		public Function(int number)
        {
            index = number;
        }

        public int getIndex() { return index; }
        public bool getType() { return type; }

		public void decNumber() 
		{
			index--;
		}
			
        //public abstract float getImpactOnProperty(float input, Property prop);
        //public abstract void impactPoint(Point point, float power);
    }
	[System.Serializable]
    public class FunctionStatAndDynam : Function
    {
        private Property[] sufferingProperties;
        private float[] coefficientsFriend;
        private float[] coefficientsEnemy;

		public FunctionStatAndDynam(int number,Property[] sufferingProp, float[] coefFr, float[] coefEn) : base(number)
        {
            
            sufferingProperties = sufferingProp;
            coefficientsFriend = coefFr;
            coefficientsEnemy = coefEn;
            type = false;
        }

        public void resetFunction(Property[] sufferingProp, float[] coefFr, float[] coefEn)
        {
            sufferingProperties = sufferingProp;
            coefficientsFriend = coefFr;
            coefficientsEnemy = coefEn;
        }

        public float getImpactOnProperty(float input, Property prop, bool enemy)
        {
            int i = 0;
            if (enemy == false)
            {
                while (i < sufferingProperties.Length && prop != sufferingProperties[i])
                    i++;
                if (i < sufferingProperties.Length)
                    return input * coefficientsFriend[i];
                else
                    return 0;
            } else
            {
                while (i < sufferingProperties.Length && prop != sufferingProperties[i])
                    i++;
                if (i < sufferingProperties.Length)
                    return input * coefficientsEnemy[i];
                else
                    return 0;
            }
        }
        
        public void impactPoint(Point point, float power, bool enemy)
        {
            for (int i = 0; i < point.getCharacteristics().Count; i++)
            {
                point.getCharacteristics()[i].suffer(getImpactOnProperty(power, point.getCharacteristics()[i].getProperty(), enemy));
                //Debug.Log(getImpactOnProperty(power, point.getCharacteristics()[i].getProperty(), enemy));
                //Debug.Log(power);
            }
        }

		public Property[] getSufferingProperties()
		{
			return sufferingProperties;
		}

		public float[] getCoefficientsFriend ()
		{
			return coefficientsFriend;
		}

		public float[] getCoefficientsEnemy ()
		{
			return coefficientsEnemy;
		}
    }
	[System.Serializable]
    public class FunctionCollectional : Function
    {
        private Property property;
        float coefficientFriend;
        float coefficientEnemy;

        public FunctionCollectional(int number, Property prop, float coefFr, float coefEn) : base(number)
        {
            property = prop;
			Debug.Log ("CR"+property.getNumber());
            type = true;
            coefficientFriend = coefFr;
            coefficientEnemy = coefEn;
        }

        public void resetFunction(Property prop, float coefFr, float coefEn)
        {
            property = prop;
            coefficientFriend = coefFr;
            coefficientEnemy = coefEn;
        }

        public void impactPoint(Point point, float thePeace, bool enemy)
        {
            if (!enemy)
            {
                for (int i = 0; i < point.getCharacteristics().Count; i++)
                {
					//Debug.Log (point.getCharacteristics () [i].getProperty ().getNumber());
					//Debug.Log (property.getNumber());
					if (point.getCharacteristics () [i].getProperty () == property) 
					{
						point.getCharacteristics () [i].suffer (thePeace * coefficientFriend);
					}
                }
            } else
            {
                for (int i = 0; i < point.getCharacteristics().Count; i++)
                {
                    if (point.getCharacteristics()[i].getProperty() == property)
                        point.getCharacteristics()[i].suffer(thePeace * coefficientEnemy);
                }
            }
        }
			
    }

}