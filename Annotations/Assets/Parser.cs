using UnityEngine;
using Assets.Scripts;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


public class Parser : MonoBehaviour{
	public static int width = 1600;
	public static int height = 898;
	public static Vector2 center = new Vector2 (width / 2, height / 2);


	// Use this for initialization
	void Start () {
//		ArrayList results = GoogleVisionParser.parseAllAnnotations (result);
//		ArrayList textAnnots = (ArrayList)results [0];
//		print (((TextAnnotation)textAnnots [1]).getBoundingPoly());

		ArrayList s1 = new ArrayList ();
		s1.Add ("CHANGE YOU WORDS");
		string[] v1_1 = new string[4]{"123", "127", "1118", "1114"};
		string[] v1_2 = new string[4]{"180", "104", "156", "232"};
		Polygon p1 = new Polygon (v1_1, v1_2);
		TextGroup g1 = new TextGroup (s1, p1, 76);

		ArrayList s2 = new ArrayList ();
		s1.Add ("CHANGE YOU MINDSET");
		string[] v2_1 = new string[4]{"414", "416", "1425", "1423"};
		string[] v2_2 = new string[4]{ "288", "204", "227", "311" };
		Polygon p2 = new Polygon (v2_1, v2_2);
		TextGroup g2 = new TextGroup (s2, p2, 84);

		ArrayList s3 = new ArrayList ();
		s3.Add ("This is too hard");
		string[] v3_1 = new string[4]{ "514", "514", "647", "647" };
		string[] v3_2 = new string[4] { "396", "367", "373", "397" };
		Polygon p3 = new Polygon (v3_1, v3_2);
		TextGroup g3 = new TextGroup (s3, p3, 20);

		List<TextGroup> clusters = new List<TextGroup>();
		clusters.Add (g1);
		clusters.Add (g2);
		clusters.Add (g3);

		List<TextGroup> sorted = clusters.OrderByDescending (cluster => cluster.getScore ()).ToList();

		foreach(TextGroup cluster in sorted) {
			print (cluster.getDistScore ());
			print (cluster.getLineScore ());
		}
	}






	
	// Update is called once per frame
	void Update () {
	
	}
}
