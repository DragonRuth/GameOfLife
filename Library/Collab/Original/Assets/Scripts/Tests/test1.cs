using UnityEngine;
using System.Collections;
using Classes.Game.MainManagerSpace;
using Classes.GameClasses.PropertiesSpace;
using Classes.GameClasses.PointSpace;

public class test1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        int[] positions = new int[0];
        MainManager main = new MainManager(6, 6, 1, positions);
        float[][] live = new float[1][];
        live[0] = new float[2];
        live[0][0] = 3;
        live[0][1] = 3;
        float[][] dead = new float[2][];
        dead[0] = new float[2];
        dead[0][0] = -100;
        dead[0][1] = 1;
        dead[1] = new float[2];
        dead[0][0] = 4;
        dead[0][1] = 100;
        main.initProperties("-", "-", live, dead);
        int[] input = {0 };
        main.initGenerations(input);
        Property[] prop = { main.getProperty(0) };
        float[] fr = { 1 };
        float[] en = { 1 };
        main.initFunctions(prop, fr, en);
        Position[] pos = new Position[5];
        pos[0] = new Position(0, 0);
        pos[1] = new Position(0, 1);
        pos[2] = new Position(1, 2);
        pos[3] = new Position(1, 0);
        pos[4] = new Position(2, 0);
        main.addPoints(pos, main.getGeneration(0));
        main.dump();
        main.step();
        main.dump();
        main.step();
        main.dump();
        main.step();
        main.dump();
        main.step();
        main.dump();
        main.step();
        //main.dump();
        main.step();
        //main.dump();
        main.step();
        //main.step();
        main.step();
        //main.dump();
        //main.dump();
        //main.dump();
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
