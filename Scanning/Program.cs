using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

public class BoundingPoly
{
	public List<Vertices> vertices { get; set; }
}

public class Vertices
{
	public int x { get; set; }
	public int y { get; set; }
}

public class Item
{
	public string description { get; set; }
	public BoundingPoly boundingPoly { get; set; }
}
class Program
{
	static void Main()
	{
		string jsonFilePath = "response.json";
		string jsonString = File.ReadAllText(jsonFilePath);

		JArray jsonArray = JArray.Parse(jsonString);

		List<string> mergedDescriptions = new List<string>();

		for (int i = 1; i < jsonArray.Count; i++)
		{
			string mergedDescription = jsonArray[i]["description"].ToString();
			
			for (int j = i + 1; j < jsonArray.Count; j++)
				{
					var currentVertices = jsonArray[i]["boundingPoly"]["vertices"];
					var nextVertices = jsonArray[j]["boundingPoly"]["vertices"];

					int currentMinY = (int)currentVertices[0]["y"];
					int currentMaxY = (int)currentVertices[3]["y"];
					int nextY = (int)nextVertices[0]["y"];

					if (nextY >= currentMinY && nextY < currentMaxY)
					{
						mergedDescription += " "+ jsonArray[j]["description"];
						jsonArray[j]["description"] = "~";
					}	
			}

			mergedDescriptions.Add(mergedDescription);
		}
		
		int count = 1;
		foreach (var description in mergedDescriptions)
		{
			if (!description.Contains("~"))
			{
				Console.WriteLine($"{count++} {description}");

			}
		}

	}
}


