using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets.Scripts
{
    internal static class GoogleVisionParser
    {
        public static ArrayList parseAllAnnotations(string annotation)
        {
            ArrayList allAnnotations = new ArrayList();

            char[] annotationArray = annotation.ToCharArray();

            string annotationType = "\"textAnnotations\": [";
            int currentAnnotationIndex = annotation.IndexOf(annotationType) + annotationType.Length;
            int textIndex = currentAnnotationIndex;
            string currentAnnotation = "";
            while (annotation.IndexOf(annotationType) != -1 && currentAnnotationIndex < annotationArray.Length)
            {
                currentAnnotation += annotationArray[currentAnnotationIndex];
                currentAnnotationIndex++;
            }
            if (annotation.IndexOf(annotationType) == -1) textIndex = annotationArray.Length;

            allAnnotations.Add(parseTextAnnotations(currentAnnotation));

            annotationType = "\"labelAnnotations\": [";
            currentAnnotationIndex = annotation.IndexOf(annotationType) + annotationType.Length;
            int labelIndex = currentAnnotationIndex;
            currentAnnotation = "";
            while (annotation.IndexOf(annotationType) != -1 && currentAnnotationIndex < textIndex)
            {
                currentAnnotation += annotationArray[currentAnnotationIndex];
                currentAnnotationIndex++;
            }

            allAnnotations.Add(parseLabelAnnotations(currentAnnotation));

            annotationType = "\"landmarkAnnotations\": [";
            currentAnnotationIndex = annotation.IndexOf(annotationType) + annotationType.Length;
            int landmarkIndex = currentAnnotationIndex;
            currentAnnotation = "";
            while (annotation.IndexOf(annotationType) != -1 && currentAnnotationIndex < labelIndex)
            {
                currentAnnotation += annotationArray[currentAnnotationIndex];
                currentAnnotationIndex++;
            }
            if (annotation.IndexOf(annotationType) != -1) labelIndex = landmarkIndex;

            allAnnotations.Add(parseLandmarkAnnotations(currentAnnotation));


            annotationType = "\"logoAnnotations\": [";
            currentAnnotationIndex = annotation.IndexOf(annotationType) + annotationType.Length;
            currentAnnotation = "";
            while (annotation.IndexOf(annotationType) != -1 && currentAnnotationIndex < labelIndex)
            {
                currentAnnotation += annotationArray[currentAnnotationIndex];
                currentAnnotationIndex++;
            }

            allAnnotations.Add(parseLogoAnnotations(currentAnnotation));

            return allAnnotations;

        }


        public static ArrayList parseLandmarkAnnotations(string annotation)
        {

            char[] annotationArray = annotation.ToCharArray();
            string mutableAnnotation = new string(annotationArray);

            ArrayList annotations = new ArrayList();

            int midIndex = annotation.IndexOf("\"mid\": ");
            while (midIndex != -1)
            {
                string mid = "";
                midIndex += 7;
                while (annotationArray[midIndex] != ',')
                {
                    mid += annotationArray[midIndex];
                    midIndex++;
                }

                int descIndex = mutableAnnotation.IndexOf("\"description\": ");
                string description = "";
                descIndex += 15;
                while (annotationArray[descIndex] != ',')
                {
                    description += annotationArray[descIndex];
                    descIndex++;
                }

                int scoreIndex = mutableAnnotation.IndexOf("\"score\": ");
                string score = "";
                scoreIndex += 9;
                while (annotationArray[scoreIndex] != ',')
                {
                    score += annotationArray[scoreIndex];
                    scoreIndex++;
                }

                score = score.Trim();

                int polyIndex = mutableAnnotation.IndexOf("\"vertices\": ");
                string vertices = "";
                polyIndex += 12;

                while (annotationArray[polyIndex] != ']')
                {
                    vertices += annotationArray[polyIndex];
                    polyIndex++;
                }

                ArrayList xList = new ArrayList();
                ArrayList yList = new ArrayList();
                int vertexIndex = vertices.IndexOf("\"x\": ");
                string mutableVertexAnnotation = new string(vertices.ToCharArray());
                char[] verticesArray = mutableVertexAnnotation.ToCharArray();

                while (vertexIndex != -1)
                {
                    int xIndex = vertexIndex;
                    string currX = "";
                    xIndex += 5;
                    while (verticesArray[xIndex] != ',')
                    {
                        currX += verticesArray[xIndex];
                        xIndex++;
                    }

                    int yIndex = mutableVertexAnnotation.IndexOf("\"y\": ");
                    string currY = "";
                    yIndex += 5;
                    while (verticesArray[yIndex] != '}')
                    {
                        currY += verticesArray[yIndex];
                        yIndex++;
                    }
                    xList.Add(currX);
                    yList.Add(currY);
                    if (vertexIndex < mutableVertexAnnotation.Length) mutableVertexAnnotation = mutableVertexAnnotation.Substring(yIndex);
                    verticesArray = mutableVertexAnnotation.ToCharArray();
                    vertexIndex = mutableVertexAnnotation.IndexOf("\"x\": ");
                }

                int latIndex = mutableAnnotation.IndexOf("\"latitude\": ");
                string latitude = "";
                latIndex += 12;
                while (annotationArray[latIndex] != ',')
                {
                    latitude += annotationArray[latIndex];
                    latIndex++;
                }

                int longIndex = mutableAnnotation.IndexOf("\"longitude\": ");
                string longitude = "";
                longIndex += 13;
                while (annotationArray[longIndex] != '\n')
                {
                    longitude += annotationArray[longIndex];
                    longIndex++;
                }
                latitude = latitude.Trim();
                longitude = longitude.Trim();
                annotations.Add(new LandmarkAnnotation(mid, description, double.Parse("0"), new Polygon(xList.ToArray(), yList.ToArray()), new Location(double.Parse(latitude), double.Parse(longitude))));

                if (scoreIndex < mutableAnnotation.Length) mutableAnnotation = mutableAnnotation.Substring(scoreIndex);
                annotationArray = mutableAnnotation.ToCharArray();
                midIndex = mutableAnnotation.IndexOf("\"mid\": ");
            }

            return annotations;
        }

        public static ArrayList parseLabelAnnotations(string annotation)
        {
            char[] annotationArray = annotation.ToCharArray();
            string mutableAnnotation = new string(annotationArray);

            ArrayList annotations = new ArrayList();

            int midIndex = annotation.IndexOf("\"mid\": ");
            while (midIndex != -1)
            {
                string mid = "";
                midIndex += 7;
                while (annotationArray[midIndex] != ',')
                {
                    mid += annotationArray[midIndex];
                    midIndex++;
                }

                int descIndex = mutableAnnotation.IndexOf("\"description\": ");
                string description = "";
                descIndex += 15;
                while (annotationArray[descIndex] != ',')
                {
                    description += annotationArray[descIndex];
                    descIndex++;
                }

                int scoreIndex = mutableAnnotation.IndexOf("\"score\": ");
                string score = "";
                scoreIndex += 9;
                while (annotationArray[scoreIndex] != '}')
                {
                    score += annotationArray[scoreIndex];
                    scoreIndex++;
                }

                score = score.Trim();
                annotations.Add(new LabelAnnotation(mid, description, double.Parse(score)));

                if (scoreIndex < mutableAnnotation.Length) mutableAnnotation = mutableAnnotation.Substring(scoreIndex);
                annotationArray = mutableAnnotation.ToCharArray();
                midIndex = mutableAnnotation.IndexOf("\"mid\": ");
            }

            return annotations;
        }

        public static ArrayList parseTextAnnotations(string annotation)
        {
            char[] annotationArray = annotation.ToCharArray();
            string mutableAnnotation = new string(annotationArray);

            ArrayList annotations = new ArrayList();

            int localeIndex = annotation.IndexOf("\"locale\": ");
            int descIndex = mutableAnnotation.IndexOf("\"description\": ");
            while (localeIndex != -1 || descIndex != -1)
            {
                string locale = "";
                if (localeIndex != -1)
                {
                    localeIndex += 10;
                    while (annotationArray[localeIndex] != ',')
                    {
                        locale += annotationArray[localeIndex];
                        localeIndex++;
                    }
                }

                string description = "";
                descIndex += 15;
                while (annotationArray[descIndex] != ',')
                {
                    description += annotationArray[descIndex];
                    descIndex++;
                }


                int polyIndex = mutableAnnotation.IndexOf("\"vertices\": ");
                string vertices = "";
                polyIndex += 12;

                while (annotationArray[polyIndex] != ']')
                {
                    vertices += annotationArray[polyIndex];
                    polyIndex++;
                }

                ArrayList xList = new ArrayList();
                ArrayList yList = new ArrayList();
                int vertexIndex = vertices.IndexOf("\"x\": ");
                string mutableVertexAnnotation = new string(vertices.ToCharArray());
                char[] verticesArray = mutableVertexAnnotation.ToCharArray();

                while (vertexIndex != -1)
                {
                    int xIndex = vertexIndex;
                    string currX = "";
                    xIndex += 5;
                    while (verticesArray[xIndex] != ',')
                    {
                        currX += verticesArray[xIndex];
                        xIndex++;
                    }

                    int yIndex = mutableVertexAnnotation.IndexOf("\"y\": ");
                    string currY = "";
                    yIndex += 5;
                    while (verticesArray[yIndex] != '}')
                    {
                        currY += verticesArray[yIndex];
                        yIndex++;
                    }
                    xList.Add(currX);
                    yList.Add(currY);
                    if (vertexIndex < mutableVertexAnnotation.Length) mutableVertexAnnotation = mutableVertexAnnotation.Substring(yIndex);
                    verticesArray = mutableVertexAnnotation.ToCharArray();

                    vertexIndex = mutableVertexAnnotation.IndexOf("\"x\": ");
                }

                annotations.Add(new TextAnnotation(locale, description, new Polygon(xList.ToArray(), yList.ToArray())));

                if (polyIndex < mutableAnnotation.Length) mutableAnnotation = mutableAnnotation.Substring(polyIndex);

                annotationArray = mutableAnnotation.ToCharArray();
                localeIndex = mutableAnnotation.IndexOf("\"locale\": ");
                descIndex = mutableAnnotation.IndexOf("\"description\": ");
            }

            return annotations;
        }

        public static ArrayList parseLogoAnnotations(string annotation)
        {
            char[] annotationArray = annotation.ToCharArray();
            string mutableAnnotation = new string(annotationArray);

            ArrayList annotations = new ArrayList();

            int midIndex = annotation.IndexOf("\"mid\": ");
            while (midIndex != -1)
            {
                string mid = "";
                midIndex += 7;
                while (annotationArray[midIndex] != ',')
                {
                    mid += annotationArray[midIndex];
                    midIndex++;
                }

                int descIndex = mutableAnnotation.IndexOf("\"description\": ");
                string description = "";
                descIndex += 15;
                while (annotationArray[descIndex] != ',')
                {
                    description += annotationArray[descIndex];
                    descIndex++;
                }

                int scoreIndex = mutableAnnotation.IndexOf("\"score\": ");
                string score = "";
                scoreIndex += 9;
                while (annotationArray[scoreIndex] != ',')
                {
                    score += annotationArray[scoreIndex];
                    scoreIndex++;
                }

                score = score.Trim();

                int polyIndex = mutableAnnotation.IndexOf("\"vertices\": ");
                string vertices = "";
                polyIndex += 12;

                while (annotationArray[polyIndex] != ']')
                {
                    vertices += annotationArray[polyIndex];
                    polyIndex++;
                }

                ArrayList xList = new ArrayList();
                ArrayList yList = new ArrayList();
                int vertexIndex = vertices.IndexOf("\"x\": ");
                string mutableVertexAnnotation = new string(vertices.ToCharArray());
                char[] verticesArray = mutableVertexAnnotation.ToCharArray();

                while (vertexIndex != -1)
                {
                    int xIndex = vertexIndex;
                    string currX = "";
                    xIndex += 5;
                    while (verticesArray[xIndex] != ',')
                    {
                        currX += verticesArray[xIndex];
                        xIndex++;
                    }

                    int yIndex = mutableVertexAnnotation.IndexOf("\"y\": ");
                    string currY = "";
                    yIndex += 5;
                    while (verticesArray[yIndex] != '}')
                    {
                        currY += verticesArray[yIndex];
                        yIndex++;
                    }
                    xList.Add(currX);
                    yList.Add(currY);
                    if (vertexIndex < mutableVertexAnnotation.Length) mutableVertexAnnotation = mutableVertexAnnotation.Substring(yIndex);
                    verticesArray = mutableVertexAnnotation.ToCharArray();

                    vertexIndex = mutableVertexAnnotation.IndexOf("\"x\": ");
                }

                annotations.Add(new LogoAnnotation(mid, description, double.Parse(score), new Polygon(xList.ToArray(), yList.ToArray())));

                if (scoreIndex < mutableAnnotation.Length) mutableAnnotation = mutableAnnotation.Substring(scoreIndex);
                annotationArray = mutableAnnotation.ToCharArray();
                midIndex = mutableAnnotation.IndexOf("\"mid\": ");
            }

            return annotations;

        }
    }

    internal class LandmarkAnnotation
    {
        private string mid;
        private string description;
        private double score;
        private Polygon boundingPoly;
        private Location location;

        internal LandmarkAnnotation(string mid, string description, double score, Polygon boundingPoly, Location location)
        {
            this.mid = mid;
            this.description = description;
            this.score = score;
            this.boundingPoly = boundingPoly;
            this.location = location;
        }

        public string getMid()
        {
            return this.mid;
        }

        public string getDescription()
        {
            return this.description;
        }

        public double getScore()
        {
            return this.score;
        }

        public Polygon getBoundingPoly()
        {
            return this.boundingPoly;
        }

        public Location getLocations()
        {
            return this.location;
        }

        public override string ToString()
        {
            return "Landmark - MID: " + this.mid + ", description: " + this.description + ", score: " + this.score + ", poly: " + this.boundingPoly + ", location: " + this.location;
        }

    }

    internal class TextAnnotation
    {
        private string locale;
        private string description;
        private Polygon boundingPoly;

        internal TextAnnotation(string locale, string description, Polygon boundingPoly)
        {
            this.locale = locale;
            if (locale == "")
            {
                this.locale = "N/A";
            }
            this.description = description;
            this.boundingPoly = boundingPoly;
        }

        public string getLocale()
        {
            return this.locale;
        }

        public string getDescription()
        {
            return this.description;
        }

        public Polygon getBoundingPoly()
        {
            return this.boundingPoly;
        }

        public override string ToString()
        {
            return "Text - locale: " + this.locale + ", description: " + this.description + ", bounding poly: " + this.boundingPoly;
        }
    }

    internal class LogoAnnotation
    {
        private string mid;
        private string description;
        private double score;
        private Polygon boundingPoly;

        internal LogoAnnotation(string mid, string description, double score, Polygon boundingPoly)
        {
            this.mid = mid;
            this.description = description;
            this.score = score;
            this.boundingPoly = boundingPoly;
        }

        public string getMid()
        {
            return this.mid;
        }

        public string getDescription()
        {
            return this.description;
        }

        public double getScore()
        {
            return this.score;
        }

        public Polygon getBoundingPoly()
        {
            return this.boundingPoly;
        }
        
        public override string ToString()
        {
            return "Logo - MID: " + this.mid + ", description: " + this.description + ", score: " + this.score + ", bounding poly: " + this.boundingPoly;
        }
    }

    internal class LabelAnnotation
    {
        private string mid;
        private string description;
        private double score;

        internal LabelAnnotation(string mid, string description, double score)
        {
            this.mid = mid;
            this.description = description;
            this.score = score;
        }

        public string getMid()
        {
            return this.mid;
        }

        public string getDescription()
        {
            return this.description;
        }

        public double getScore()
        {
            return this.score;
        }

        public override string ToString()
        {
            return "Label - MID: " + this.mid + ", description: " + this.description + ", score: " + this.score;
        }
    }

    internal class Polygon
    {
        private object[] v1;
        private object[] v2;
        private char[] x;
        private char[] y;

        public Polygon(object[] v1, object[] v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public Polygon(char[] x, char[] y)
        {
            this.x = x;
            this.y = y;
        }

//		public int[] GetHeightAndWidth() {
//			if (v1 == null) {
//				return new int[]{Math.Abs(y[0] - y[1]), Math.Abs(x[0]-x[1])};
//			}
//			return new int[]{Math.Abs(v2[1] - v1[1]), Math.Abs(v2[0]-v1[0])};
//		}
    }

    internal class Location
    {
        private double latitude;
        private double longitude;

        public Location(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public override string ToString()
        {
            return "(lat: " + this.latitude + ", long: " + this.longitude + ")";
        }

    }

	internal class TextGroup
	{
		private ArrayList textAnnotations; 
		private Polygon boundingBox;
		private int lineHeight;
		private float score;

		public TextGroup(ArrayList textAnnotations, Polygon boundingBox, int lineHeight)
		{
			this.textAnnotations = textAnnotations;
			this.lineHeight = lineHeight;
			this.boundingBox = boundingBox;
		}
			
		public Polygon getBoundingBox() {
			return this.boundingBox; 
		}

		public int getLineHeight() {
			return this.lineHeight;
		}

		public ArrayList getTextAnnotations() {
			return this.textAnnotations;
		}

		public float getScore() {
			return score;
		}

		public void setScore(float s) {
			score = s;
		}

		public override string ToString()
		{
			return "box " + this.boundingBox + ", height: " + this.lineHeight + ", text: " + textAnnotations.ToString() +")";
		}


	}

}
