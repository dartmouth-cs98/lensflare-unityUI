using UnityEngine;
using Assets.Scripts;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


public class Parser : MonoBehaviour{



	// Use this for initialization
	void Start () {
//		ArrayList results = GoogleVisionParser.parseAllAnnotations (result);
//		ArrayList textAnnots = (ArrayList)results [0];
//		print (((TextAnnotation)textAnnots [1]).getBoundingPoly());

		int width = 1600;
		int height = 898;
		Vector2 center = new Vector2 (width / 2, height / 2);

		ArrayList s1 = new ArrayList ();
		s1.Add ("CHANGE YOU WORDS");
		string[] v1_1 = new string[4]{"123", "127", "1118", "1114"};
		string[] v1_2 = new string[4]{"180", "104", "156", "232"};
		Polygon p1 = new Polygon (v1_1, v1_2);
		TextGroup g1 = new TextGroup (s1, p1, 76);
		g1.setScore (11);

		ArrayList s2 = new ArrayList ();
		s1.Add ("CHANGE YOU MINDSET");
		string[] v2_1 = new string[4]{"414", "416", "1425", "1423"};
		string[] v2_2 = new string[4]{ "288", "204", "227", "311" };
		Polygon p2 = new Polygon (v2_1, v2_2);
		TextGroup g2 = new TextGroup (s2, p2, 84);
		g2.setScore (100);

		ArrayList s3 = new ArrayList ();
		s3.Add ("This is too hard");
		string[] v3_1 = new string[4]{ "514", "514", "647", "647" };
		string[] v3_2 = new string[4] { "396", "367", "373", "397" };
		Polygon p3 = new Polygon (v3_1, v3_2);
		TextGroup g3 = new TextGroup (s3, p3, 20);
		g3.setScore (3);

		List<TextGroup> clusters = new List<TextGroup>();
		clusters.Add (g1);
		clusters.Add (g2);
		clusters.Add (g3);

		List<TextGroup> sorted = clusters.OrderByDescending (cluster => cluster.getScore ()).ToList();

		foreach(TextGroup cluster in sorted) {
//			print (cluster.getScore ());
		}

		Vector2 polyCenter = centerOfPoly (p1);
		float dist = Vector2.Distance (polyCenter, center);


		float pic_div = height / 4;
		int lh = g1.getLineHeight ();
		var lh_score = lh / pic_div;

		print ("lh_score: " + lh_score);
		print ("dist: " + dist);




//		print (polyCenter.ToString() + " / " + center.ToString());
//		print ("dist");
//		print (dist);

	}

	Vector2 centerOfPoly(Polygon poly) {

		Vector2 leftBtm = new Vector2(int.Parse((string)poly.v1[0]), int.Parse((string)poly.v2[0]));
		Vector2 leftTop = new Vector2(int.Parse((string)poly.v1[1]), int.Parse((string)poly.v2[1]));
		Vector3 rightBtm = new Vector2(int.Parse((string)poly.v1[3]), int.Parse((string)poly.v2[3]));

		int width = (int)Mathf.Abs (leftBtm.x + rightBtm.x);
		int height = (int)Mathf.Abs (leftBtm.y + leftTop.y);

		Vector2 center = new Vector2 (width / 2, height / 2);
		return center;
	}




	
	// Update is called once per frame
	void Update () {
	
	}
}
