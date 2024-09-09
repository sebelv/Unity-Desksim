using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class AltTextFileReader
{
	private List<float> altListe = new List<float>();
	private List<float> distListe = new List<float>();
	private List<Vector3> pointList = new List<Vector3>();
	
	public AltTextFileReader(string filnavn)
	{
			lesFil(filnavn);
	}

	public List<float> getAltListe()
	{
		return altListe;
	}

	public List<float> getDistListe()
	{
		return distListe;
	}
	
	public List<Vector3> getPointListe()
	{
		return pointList;
	}
	
	private void lesFil(string filnavn)
	{
		StreamReader reader = new StreamReader(filnavn, true);

	  //leser tegn inntil filslutt
	  string innlinje = null;
	  do
	  {
	    innlinje = reader.ReadLine(); //leser en linje
	    if ( innlinje != null ) //null betyr filslutt
	    {
	    	lesLinje(innlinje);
	    }
	  } while ( innlinje != null );

	  //Alt er lest. Lukker fila.
	  reader.Close();
	}
	
	private void lesLinje(string innLinje)
	{
		string[] sa = innLinje.Split(" ");
		
		if (sa.Length == 2)
		{
			float d = finnFloat(sa[1]);
			altListe.Add(d);
			
			float n = finnFloat(sa[0]);
			distListe.Add(n);
			
			pointList.Add(new Vector3(0, d, n));
		}
	}
	
	private float finnFloat(string s)
	{
			return float.Parse(s, CultureInfo.InvariantCulture);
	}
}
