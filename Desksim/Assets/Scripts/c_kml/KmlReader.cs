using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TMPro.SpriteAssetUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class KmlReader : MonoBehaviour
{
	List<Vector3> traseSplineListe = new List<Vector3>();
	
	List<Vector3> coordsListe = new List<Vector3>();
	List<string> coords = new List<string>();
	Vector3 pNullPunkt;
	
 	KmlSpline traseSpline;
//	ArrayList<KmlSpline> rightTerrainSplines = new ArrayList<KmlSpline>();
//	ArrayList<KmlSpline> leftTerrainSplines = new ArrayList<KmlSpline>();
//	
//	ArrayList<TexturePolygon3D> polyTextureListe = new ArrayList<TexturePolygon3D>();
//	ArrayList<ObjectsPolygon3D> polyObjectsListe = new ArrayList<ObjectsPolygon3D>();
//	ArrayList<SingleObject> singleObjectsListe = new ArrayList<SingleObject>();
//	ArrayList<WallObject> wallObjectsListe = new ArrayList<WallObject>();
	
	string textureFile = "";
	double textureFactor = 1;
	int numXVertices = 5;
	
	// objects in polygon
//	private ArrayList<String> objectFilenameList = new ArrayList<String>();
	private double distbet = 20.0;
	private double distrand = 0;
	private double angle = 0;
	private double height = 2.0;
	
//	// single object
//	private String objectFilename;
	
//	ArrayList<KmlSpline> catmullRomSpline3DKmlListe = new ArrayList<KmlSpline>();
//	ArrayList<KmlSpline> catmullRomSpline3DKmlListeh�gde = null;
//	int splineCounter = 0;
	
	string kmlFile;
	string navn;
	Vector3 googleEarthUtgPkt;

  // start og slutt korreksjon i forhold til sammenkobling av seksjoner
  Vector3 korrStart;
  Vector3 korrSlutt;
	
	public KmlReader(string kmlFile, Vector3 startCorrOffs, Vector3 endCorrOffs)
	{
		this.kmlFile = kmlFile;
		this.navn = kmlFile.Split("\\.")[0];
		
    if (startCorrOffs != null)
    {
      korrStart.x = startCorrOffs.x;
      korrStart.z = startCorrOffs.z;
    }
    
    if (endCorrOffs != null)
    {
      korrSlutt.x = endCorrOffs.x;
      korrSlutt.z = endCorrOffs.z;
    }
    
    
		try
		{
			lesFil(kmlFile);
		}
		catch (IOException e) 
		{

		}
	}

	public List<Vector3> GetCoords()
	{
		return coordsListe;
	}
	
	public KmlReader(string kmlFile, Vector3 googleEarthUtgPkt, 
          Vector3 startCorrOffs, Vector3 endCorrOffs)
	{
		this.kmlFile = kmlFile;
		this.navn = kmlFile.Split("\\.")[0];
		this.googleEarthUtgPkt = googleEarthUtgPkt;
		
    if (startCorrOffs != null)
    {
      korrStart.x = startCorrOffs.x;
      korrStart.z = startCorrOffs.z;
    }
    
    if (endCorrOffs != null)
    {
      korrSlutt.x = endCorrOffs.x;
      korrSlutt.z = endCorrOffs.z;
    }
	lesFil(kmlFile);
	}

	private void lesFil(string filnavn )
	{
		StreamReader reader = new StreamReader(filnavn, true);
	  
		//leser tegn inntil filslutt
		string innlinje = null;
		do
		{
			innlinje = reader.ReadLine(); //leser en linje
			Debug.Log(innlinje);
			if ( innlinje != null ) //null betyr filslutt
			{
				lesLinje(innlinje);
			}
		} while ( innlinje != null );


		//Alt er lest. Lukker fila.
		reader.Close();
	}
	
	int status = 0;
	bool _readCoordinates = false;
		
	public KmlSpline getTraseSpline()
	{
		return traseSpline;
	}

//	public ArrayList<KmlSpline> getRightTerrainSplines()
//	{
//		return rightTerrainSplines;
//	}
//
//	public ArrayList<KmlSpline> getLeftTerrainSplines()
//	{
//		return leftTerrainSplines;
//	}
//
//	public ArrayList<KmlSpline> getCatmullRomSpline3DKmlListe()
//	{
//		return catmullRomSpline3DKmlListe;
//	}
//	
//	public ArrayList<TexturePolygon3D> getPolyTextureListe()
//	{
//		return polyTextureListe;
//	}
//	
//	public ArrayList<ObjectsPolygon3D> getPolyObjectsListe()
//	{
//		return polyObjectsListe;
//	}
//
//	public ArrayList<SingleObject> getSingleObjectsListe()
//	{
//		return singleObjectsListe;
//	}
//
//	public ArrayList<WallObject> getWallObjectsListe()
//	{
//		return wallObjectsListe;
//	}
//
	private int finnInt(string s)
	{
        return int.Parse(s);
	}
	
	private double finnDouble(string s)
	{
        return double.Parse(s);
	}
	
	private int finnSplineTag(string innLinje)
	{
		int ret = 0;
		
		if (!innLinje.Contains("<name>"))
			return 0;
		
		if (innLinje.Contains("<name>trase</name>"))
			ret = 1;
		
		if (ret < 1)
			return 0;
		
		string s = innLinje;
		
		s = s.Replace(".*<name>", "");
		s = s.Replace("</name>.*", "");
		s = s.Replace("\\s", "");
		
		string[] sa = s.Split("[;=]");
		
//		for (int i = 0; i < sa.length; i++)
//		{
//			if (sa[i].equals("texturefile"))
//				textureFile = sa[i+1];
//			else if (sa[i].equals("texturefactor"))
//				textureFactor = finnDouble(sa[i+1]);
//			else if (sa[i].equals("numxvertices"))
//			 numXVertices = finnInt(sa[i+1]);
//			else if (sa[i].equals("objectfile"))
//				objectFilenameList.add(sa[i+1]);
//			else if (sa[i].equals("distbet"))
//				distbet = finnDouble(sa[i+1]);
//			else if (sa[i].equals("distrand"))
//				distrand = finnDouble(sa[i+1]);
//			else if (sa[i].equals("angle"))
//				angle = finnDouble(sa[i+1]);
//			else if (sa[i].equals("height"))
//				height = finnDouble(sa[i+1]);
//		}
		
		return ret;
	}
	
	private void lesLinje(string innLinje)
	{
		Debug.Log(innLinje + " - INNLINJE 1");
		int st = finnSplineTag(innLinje);
		print("Status: " + st);
		if (st > 0)
			status = st;
		
		if (innLinje.Contains("<coordinates>"))
		{
			_readCoordinates = true;
		}
		
		if (innLinje.Contains("</coordinates>"))
		{
			if (innLinje.Contains("<coordinates>") && innLinje.Contains("</coordinates>"))
				readSingleCoordinate(innLinje);
			
			_readCoordinates = false;
			
			processCoordsStrings(coordsListe);
			
			
//			if (/*catmullRomSpline3DKmlListehøgde != null && */
//					status >= 1 && status <= 3)
//				replaceYValue(coordsListe);
			
			if (status == 1)
			{
				if (googleEarthUtgPkt == null)
					pNullPunkt = coordsListe[0];
				else
					pNullPunkt = googleEarthUtgPkt;
			}
			

			
			//les høgdeprofil
			if (status >= 1 && status <=3 )
			{
				Debug.Log("Kommer den noengang hit");
				//ArrayList<Vector3f> høgdeProfilListe = null;
				
				// IKKJE LES HØGDEPROFIL I DENNE TESTEN
//				if (status == 1)
//					høgdeProfilListe = lesHøgdeProfil("traseAltProfile_" + navn + ".alt");
//				if (status == 2)
//					høgdeProfilListe = lesHøgdeProfil(
//							"right"  + (rightTerrainSplines.size() + 1) + "AltProfile_" + navn + ".alt");
//				if (status == 3)
//					høgdeProfilListe = lesHøgdeProfil(
//							"left" + (leftTerrainSplines.size() + 1) + "AltProfile_" + navn + ".alt");
//				
//				System.out.println("PROFILE: " + høgdeProfilListe);
	

				// lag spline av innlest coordsliste og innlest høgdeprofil
				KmlSpline cmrSpline = new KmlSpline(
						coordsListe, pNullPunkt, null/*, høgdeProfilListe*/, korrStart, korrSlutt);

				cmrSpline.setNumXVertices(numXVertices);
				cmrSpline.setTextureFactor(textureFactor);
				cmrSpline.setTextureFile(textureFile);
			
				if (status == 1)
					traseSpline = cmrSpline;
				
//				if (status == 2)
//					rightTerrainSplines.add(cmrSpline);
//				
//				if (status == 3)
//					leftTerrainSplines.add(cmrSpline);
//				
//				catmullRomSpline3DKmlListe.add(cmrSpline);
			}
			
//			if (status == 4)
//				polyTextureListe.add(genererPolygonTexture());
//			
//			if (status == 5)
//				polyObjectsListe.add(genererPolygonObjects());
//			
//			if (status == 6)
//				singleObjectsListe.add(genererSingleObject());
//			
//			if (status == 7)
//				wallObjectsListe.add(genererWallObject());
			
			status = 0;
			coordsListe.Clear();
			coords.Clear();
			angle = 0;
		}
		
		if (_readCoordinates)
			readCoordinates(innLinje);		
	}
	
//	private ArrayList<Vector3f> lesHøgdeProfil(String filnavn)
//	{
//		System.out.println("FIL " + filnavn);
//		//System.exit(0);
//			
//		AltTextfileReader atr = new AltTextfileReader(filnavn);
//		
//		return atr.getPointListe();
//	}
//	
//	private ObjectsPolygon3D genererPolygonObjects()
//	{
//		ObjectsPolygon3D polygon = new ObjectsPolygon3D();
//		LongLat l  = new LongLat(pNullPunkt);
//		
//		ArrayList<Vector3f> polyPointListe = 
//			l.finnKoordinatPunkter(coordsListe);
//		
//		for (Vector3f p: polyPointListe)
//		{
//			double temp = p.z;
//			p.z = p.y;
//			p.y = temp;
//			p.x = -p.x;
//			
//			polygon.addVector3f(p);
//		}
//		
//		polygon.setDistBet(distbet);
//		polygon.setDistRandom(distrand);
//		polygon.setMilkshapeObjektFilenameList(new ArrayList<String>(objectFilenameList));
//		objectFilenameList.clear();
//		polygon.setAngle(angle);
//		polygon.finished();
//		
//		return polygon;
//	}
	
//	private SingleObject genererSingleObject()
//	{
//		SingleObject so = new SingleObject();
//		LongLat l  = new LongLat(pNullPunkt);
//		
//		Vector3f p = l.finnKoordinatPunkt(coordsListe.get(0));
//
//		double temp = p.z;
//		p.z = p.y;
//		p.y = temp;
//		p.x = -p.x;
//		
//		so.setObjectFilename(new String(objectFilenameList.get(0)));
//		so.setPosition(p);
//		objectFilenameList.clear();
//		so.setAngle(angle);
//		
//		return so;
//	}
	
//	private WallObject genererWallObject()
//	{
//		WallObject wall = new WallObject();
//		LongLat l  = new LongLat(pNullPunkt);
//		
//		ArrayList<Vector3f> polyPointListe = 
//			l.finnKoordinatPunkter(coordsListe);
//		
//		for (Vector3f p: polyPointListe)
//		{
//			double temp = p.z;
//			p.z = p.y;
//			p.y = temp;
//			p.x = -p.x;
//			
//			wall.addVector3f(p);
//		}
//		
//		wall.setDistBet(distbet);
//		wall.setTextureFile(textureFile);
//		wall.setHeight(height);
//		wall.finished();
//		
//		return wall;
//	}
	
//	private TexturePolygon3D genererPolygonTexture()
//	{
//		TexturePolygon3D polygon = new TexturePolygon3D();
//		LongLat l  = new LongLat(pNullPunkt);
//		
//		ArrayList<Vector3f> polyPointListe = 
//			l.finnKoordinatPunkter(coordsListe);
//		
//		for (Vector3f p: polyPointListe)
//		{
//			double temp = p.z;
//			p.z = p.y;
//			p.y = temp;
//			p.x = -p.x;
//			
//			polygon.addVector3f(p);
//		}
//		
//		polygon.setTextureCoordFactor(textureFactor);
//		polygon.setTextureFile(textureFile);
//		polygon.finished();
//			
//		return polygon;
//	}
	
//	private void replaceYValueOld(ArrayList<Vector3f> liste)
//	{
//		int i = 0;
//		KmlSpline spline = catmullRomSpline3DKmlListehøgde.get(splineCounter);
//		
//		for (Vector3f p:liste)
//		{
//			p.z = spline.getGoogleEarthCoords().get(i).z; // ja, y-verdien er z verdien her
//			i++;
//		}
//		
//		splineCounter++;
//	}
	
//	private void replaceYValue(ArrayList<Vector3f> liste)
//	{
//		String filnavn = "";
//		if (status == 1)
//			filnavn = "trase.alt";
//		else if (status == 2)
//			filnavn = "right" + (rightTerrainSplines.size() + 1) + ".alt";
//		else if (status == 3)
//			filnavn = "left" + (leftTerrainSplines.size() + 1) + ".alt";
//		
//		AltTextfileReader atr = new AltTextfileReader(filnavn);
//		int i = 0;
//		
//		if (atr.getAltListe().size() < liste.size())
//			return;
//		
//		for (Vector3f p:liste)
//		{
//			System.out.println(i);
//			p.z = atr.getAltListe().get(i); // ja, y-verdien er z verdien her
//			i++;
//		}
//		
//	}
	
	private void readSingleCoordinate(string innlinje)
	{
		string linje = new string(innlinje);
		linje = linje.Replace("[\\s\\t]", "");
		string[] sa = linje.Split("<coordinates>|</coordinates>|[\\s,]");
		
		if (sa.Length == 4)
		{
			string coord = "";
			
			coord = sa[1] + "," + sa[2] + "," + sa[3];
			
			coords.Add(coord);
		}
		
	}
	
	private void readCoordinates(string innlinje)
	{
		
		string[] sa = innlinje.Split("\\s");
		
		foreach (string s in sa)
			coords.Add(s);
	}
	
	private void processCoordsStrings(List<Vector3> liste)
	{
		for(int i = 1; i < coords.Count; i++)
		{
			Debug.Log(coords[i]);
			
			string[] sa = coords[i].Split(" ");

			for(int k = 0; k < sa.Length-1; k++)
			{
				string[] sai = sa[k].Split(",");
				if (sa.Length % 3 == 0)
				{
					liste.Add(extractPoint(sai));
				}
			}
		}
	}
	
	private Vector3 extractPoint(string[] sa)
	{
		Vector3 p = new Vector3();

		p.x = float.Parse(sa[0], CultureInfo.InvariantCulture);
		p.y = float.Parse(sa[1], CultureInfo.InvariantCulture);
		p.z = float.Parse(sa[2], CultureInfo.InvariantCulture);
		
		return p;
	}
  
  public List<Vector3> getSplineKontrollPunktListe()
  {

    return traseSpline.getSplineKontrollPunktListe();
  }
          
}
