using UnityEngine;
using System.Collections;
using Classes.Game.MainManagerSpace;
using Classes.GameClasses.PropertiesSpace;
using Classes.GameClasses.PointSpace;
using Classes.Game.PlayersManagerSpace;

public class test1 : MonoBehaviour {
    private MainManager main;
    // Use this for initialization
    void Start () {
        int[] positions = new int[0];
       main = new MainManager(6, 6, 1, positions, 0);
       /*int[] n = {3, 1, 1, 1 };
       bool[] i = {false, false, false};
       string[] nam = { "-", "-", "-" };
       string[] desc = { "-", "-", "-" };
       float[][] liveint = new float[1][];
       liveint[0] = new float[2];
       liveint[0][0] = 3;
       liveint[0][1] = 3;
       float[][] deadint = new float[1][];
       deadint[0] = new float[4];
       deadint[0][0] = -100;
       deadint[0][1] = 1;
       deadint[0][2] = 4;
       deadint[0][3] = 100;
       float[] power = { 1 };
       float[] liveP = { 0 };
       int[][] propNumP = new int[2][];
       propNumP[0] = new int[3];
       propNumP[0][0] = 0;
       propNumP[0][1] = 1;
       propNumP[0][2] = 2;
       propNumP[1] = new int[0];
       int[][] propNum = new int[3][];
       propNum[0] = new int[1];
       propNum[0][0] = 0;
       propNum[1] = new int[1];
       propNum[1][0] = 1;
       propNum[2] = new int[1];
       propNum[2][0] = 2;
       float[][] coefFr = new float[3][];
       coefFr[0] = new float[1];
       coefFr[0][0] = 1;
       coefFr[1] = new float[1];
       coefFr[1][0] = 1;
       coefFr[2] = new float[1];
       coefFr[2][0] = 1;
       float[][] coefEn = new float[3][];
       coefEn[0] = new float[1];
       coefEn[0][0] = -1;
       coefEn[1] = new float[1];
       coefEn[1][0] = -1;
       coefEn[2] = new float[1];
       coefEn[2][0] = -1;
       int[] funcT = { 0, 1, 2};
       bool[] immort = {false, false};
       string[][] col = new string[1][];
       col[0] = new string[2];
       col[0][0] = "1";
       col[0][1] = "2";
       float[] livePD = { 0 };
       float[] setP = { 1};*/

        int[] n = { 1, 1, 0, 0 };
        bool[] i = { false, false, false };
        string[] nam = { "-", "-", "-" };
        string[] desc = { "-", "-", "-" };
        float[][] liveint = new float[1][];
        liveint[0] = new float[2];
        liveint[0][0] = 3;
        liveint[0][1] = 3;
        float[][] deadint = new float[1][];
        deadint[0] = new float[4];
        deadint[0][0] = -100;
        deadint[0][1] = 1;
        deadint[0][2] = 4;
        deadint[0][3] = 100;
        float[] power = { 1 };
        float[] liveP = { 0 };
        int[][] propNumP = new int[1][];
        propNumP[0] = new int[1];
        propNumP[0][0] = 0;
        //propNumP[0][1] = 1;
        // propNumP[0][2] = 2;
        //propNumP[1] = new int[0];
        int[][] propNum = new int[1][];
        propNum[0] = new int[1];
        propNum[0][0] = 0;
        /*propNum[1] = new int[1];
        propNum[1][0] = 1;
        propNum[2] = new int[1];
        propNum[2][0] = 2;*/
        float[][] coefFr = new float[1][];
        coefFr[0] = new float[1];
        coefFr[0][0] = 1;
        /*coefFr[1] = new float[1];
        coefFr[1][0] = 1;
        coefFr[2] = new float[1];
        coefFr[2][0] = 1;*/
        float[][] coefEn = new float[1][];
        coefEn[0] = new float[1];
        coefEn[0][0] = -1;
        /*coefEn[1] = new float[1];
        coefEn[1][0] = -1;
        coefEn[2] = new float[1];
        coefEn[2][0] = -1;*/
        int[] funcT = { 0 };
        bool[] immort = { false, false };
        string[][] col = new string[1][];
        col[0] = new string[2];
        col[0][0] = "1";
        col[0][1] = "2";
        float[] livePD = { 0 };
        float[] setP = { 1 };

        string[] genNam = { "The first generation" };
        string[] genDsec = { "To test the output" };
        main.initRules(n, i, nam, desc, liveint, deadint, power, liveP, livePD, setP, col, 1, genNam, genDsec, propNumP, immort, 1, propNum, coefFr, coefEn, funcT);

        Position[] pos = new Position[5];
        pos[0] = new Position(0, 0);
        pos[1] = new Position(0, 1);
        pos[2] = new Position(1, 2);
        pos[3] = new Position(1, 0);
        pos[4] = new Position(2, 0);
        int[] generations = { 0, 0, 0, 0, 0 };
        int[] teams = { 1, 1, 1, 1, 1 };
        main.createPlayers(0, 5, 0, 1);
        main.setPlayersParametres(0, 0, 2, 0, 4);
        //main.addPoints(pos, teams, generations);
        //Computer comp1 = new Computer(5, main.getGenerations()[0],main.getPointsManager(), main.getPointsOperator());
       // Computer comp2 = new Computer(5, main.getGenerations()[0], main.getPointsManager(), main.getPointsOperator());
        /*comp.turn();
        main.dump();
        main.step();
        comp.incResource(5);
        comp.turn();
        main.dump();
        main.step();
        comp.incResource(5);
        comp.turn();
        main.dump();
        main.step();
        comp.incResource(5);
        comp.turn();
        main.dump();
        main.step();*/
        //main.addPointsAreas(0, pos, teams, generations);
        //main.addPointsAreas(1, pos, teams, generations);
        //main.addPointsAreas(2, pos, teams, generations);
        /*for (int il = 0; il < 10; il++)
        {
            main.dump();
            main.step();
            //Debug.Log(main.getAge());
        }*/
        //main.dump();
        for (int il = 0; il < 5; il++)
        {
            //comp1.turn();
           // comp2.turn();
     //       main.dump();
            main.step();
           // comp1.incResource(5);
           // comp2.incResource(5);
        }
    }
	
	// Update is called once per frame
	void Update () {
        //main.dump();
       // Debug.Log(main.getAge());
      //  main.step();
    }
}
