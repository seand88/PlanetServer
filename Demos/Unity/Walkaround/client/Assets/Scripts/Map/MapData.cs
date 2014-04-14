using UnityEngine;
using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class MapData
{
	public int Width { get; private set; }
	public int Height { get; private set; }

	private List<string> _resources;
	private List<int> _tiles;
	private List<bool> _walkable;

	public void LoadMap(string filename)
	{
		TextAsset map = (TextAsset)Resources.Load("Maps/" + filename);

		if (map != null)			
		{			
			XmlDocument xml = new XmlDocument();
			xml.LoadXml(map.text);
		
			XmlNode node = xml.DocumentElement.SelectSingleNode("map");

			Width = Int32.Parse(node.Attributes["width"].InnerText);
			Height = Int32.Parse(node.Attributes["height"].InnerText);

			_resources = new List<string>();

			XmlNode resources = node.SelectSingleNode("resources");
			foreach (XmlNode resource in resources.ChildNodes)
				_resources.Add(resource.InnerText);

			_tiles = new List<int>();

			XmlNode tiles = node.SelectSingleNode("tiles");
			string[] str = tiles.InnerText.Split(',');
			foreach (string s in str) 
				_tiles.Add(Int32.Parse(s));

			_walkable = new List<bool>();

			XmlNode walkable = node.SelectSingleNode("walkable");
			str = walkable.InnerText.Split(',');
			foreach (string s in str) 
				_walkable.Add(Int32.Parse(s) == 0);
		}
	}

	public string GetTileResource(int x, int y)
	{
		if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
			return "";

		int index = _tiles[x + Width * y];
	
		return _resources[index];
	}

	public bool CanMove(Vector2 target)
	{
		if (target.x >= 0 && target.x < Width && target.y <= 0 && target.y > -Height && _walkable[(int)target.x + Width * (int)-target.y])
			return true;

		return false;
	}
}
