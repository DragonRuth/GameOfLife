using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
using Classes.GameClasses.PropertiesSpace;
using Classes.GameClasses.PointSpace;

namespace Classes.GameClasses.FuncManegerSpace
{

    public class FunctionManager
    {
        private List<Function> allFunctions;

        public FunctionManager()
        {
            allFunctions = new List<Function>();
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
            if (((prop.getType() == 0 || prop.getType() == 1) && allFunctions[index].getType() == false)
                || (prop.getType() == 2 && allFunctions[index].getType() == true ))
            prop.setFunction(allFunctions[index]);
        }
        public void createFunction(Property[] prop, float[] coefFr, float[] coefEn)
        {
            bool flag = false;
            Function func;
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
                func = new FunctionStatAndDynam(prop, coefFr, coefEn);
                allFunctions.Add(func);
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
                    func = new FunctionCollectional(prop[0], coefFr[0], coefEn[0]);
                    allFunctions.Add(func);
                }
                else Debug.Log("The function can not de created.(createFunction())");
            }
        }

        public void resetFunction(int number,Property[] prop, float[] coefFr, float[] coefEn)
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
                FunctionStatAndDynam function = (FunctionStatAndDynam)allFunctions[number];
                function.resetFunction(prop, coefFr, coefEn);
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
                    FunctionCollectional function = (FunctionCollectional)allFunctions[number];
                    function.resetFunction(prop[0], coefFr[0], coefEn[0]);
                }
                else Debug.Log("The function can not de created.(createFunction())");
            }
        }
    }

    public abstract class Function
    {
        private static int numberOfFunctions = 0;
        protected int index;
        protected bool type;

        public Function()
        {
            index = numberOfFunctions;
            numberOfFunctions++;
        }

        public int getIndex() { return index; }
        public bool getType() { return type; }
        public static int getNumberOfFunctions() { return numberOfFunctions; }

        ~Function()
        {
            numberOfFunctions--;
        }
        //public abstract float getImpactOnProperty(float input, Property prop);
        //public abstract void impactPoint(Point point, float power);
    }

    public class FunctionStatAndDynam : Function
    {
        private Property[] sufferingProperties;
        private float[] coefficientsFriend;
        private float[] coefficientsEnemy;

        public FunctionStatAndDynam(Property[] sufferingProp, float[] coefFr, float[] coefEn) : base()
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
    }

    public class FunctionCollectional : Function
    {
        private Property property;
        float coefficientFriend;
        float coefficientEnemy;

        public FunctionCollectional(Property prop, float coefFr, float coefEn) : base()
        {
            property = prop;
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
            if (enemy == false)
            {
                for (int i = 0; i < point.getCharacteristics().Count; i++)
                {
                    if (point.getCharacteristics()[i].getProperty() == property)
                        point.getCharacteristics()[i].suffer(thePeace * coefficientFriend);
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