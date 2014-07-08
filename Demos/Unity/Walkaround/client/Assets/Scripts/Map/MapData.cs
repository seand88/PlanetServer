using UnityEngine;
using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

/// <summary>
/// Holds the data for the current map.
/// </summary>
public class MapData
{
	/// <summary>
	/// Gets the number of map cells in the x direction.
	/// </summary>
	/// <value>The width.</value>
	public int Width { get; private set; }
	/// <summary>
	/// Gets the number of map cells in the y direction.
	/// </summary>
	/// <value>The height.</value>
	public int Height { get; private set; }

	private List<string> _resources;
	private List<int> _tiles;
	private List<bool> _walkable;

	/// <summary>
	/// Loads the map.
	/// </summary>
	/// <param name="filename">Filename of the map to load.  It must be in the "Maps" folder of the "Resources".</param>
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

			// filenames of the map tiles
			XmlNode resources = node.SelectSingleNode("resources");
			foreach (XmlNode resource in resources.ChildNodes)
				_resources.Add(resource.InnerText);

			_tiles = new List<int>();

			// what the actual tiles are
			XmlNode tiles = node.SelectSingleNode("tiles");
			string[] str = tiles.InnerText.Split(',');
			foreach (string s in str) 
				_tiles.Add(Int32.Parse(s));

			_walkable = new List<bool>();

			// if the tile can be walked on
			XmlNode walkable = node.SelectSingleNode("walkable");
			str = walkable.InnerText.Split(',');
			foreach (string s in str) 
				_walkable.Add(Int32.Parse(s) == 0);
		}
	}

	/// <summary>
	/// Gets the filename to the prefab for the given map cell.
	/// </summary>
	/// <returns>The filename.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public string GetTileResource(int x, int y)
	{
		if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
			return "";

		int index = _tiles[x + Width * y];
	
		return _resources[index];
	}

	/// <summary>
	/// Checks if the given map cell is open for movement.  Note that the y direction is negative.
	/// </summary>
	/// <returns><c>true</c> if the cell is open; otherwise, <c>false</c>.</returns>
	/// <param name="target">Map cell to check.</param>
	public bool CanMove(Vector2 target)
	{
		if ((int)target.x >= 0 && (int)target.x < Width && (int)target.y <= 0 && (int)target.y > -Height && _walkable[(int)target.x + Width * (int)-target.y])
			return true;

		return false;
	}
}
