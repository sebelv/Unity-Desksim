using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrackNode
{
 // pekere

  public TrackNode neste, forrige;

  // connected trackside object
  public TrackConnectedObject trackConnectedObject;

  // nodens 3d posisjon
  protected Vector3 point;

  // nodens midlertidige 3d posisjon
  protected Vector3 tempPoint;

  // nodens en dimensjonale posisjon langs sporet - 
  // antall meter siden starten av strekningen
  //protected double pos;
  //
  protected bool traseOvergang = false;

  // avstander
  /**
   * Klassevariabel som sier hvor langt to noder kan ligge fra hverandre mens de
   * fortsatt betraktes å ha samme posisjon Variabelen er gjort public for å
   * gjøre verdien tilgjengelig for eventuelle testklasser
   */
  public static float SAMME_POS_AVSTAND = 0.5f;

  // collision
  protected bool trackRunnerNode = false;

  /**
   * Konstruktør
   *
   * @param parent objektet som noden tilhører - feks ERTMS posisjonsbaliser
   * @param point nodens 3d posisjon
   */
  public TrackNode(TrackConnectedObject trackConnectedObject, Vector3 point)
  {
    this.trackConnectedObject = trackConnectedObject;
    this.point = point;
    this.tempPoint = point;
    //pos = point.z;
  }

  /**
   * Sjekker om to noder har samme posisjon
   *
   * @param node
   * @return true dersom avstanden mellom denne noden og angitt node er mindre
   * enn SAMME_POS_AVSTAND
   */
  public bool sammePos(TrackNode node)
  {
    return Vector3.Distance(point, node.getPoint()) <= SAMME_POS_AVSTAND;
  }

  /**
   * Kopierer nåværenede posisjon til midlertidig posisjon. Et nytt Point3d
   * objekt blir opprettet.
   */
  public void kopierPointTilTempPoint()
  {
    tempPoint = point;
  }

//  public void setTempPoint(Vector3f p)
//  {
//    tempPoint = p;
//  }

//	public void setPos(double pos)
//	{
//		this.pos = pos;
//	}
  /*
	 * tilgangsmetoder
   */
  public bool isTraseOvergang()
  {
    return traseOvergang;
  }

  public void setTraseOvergang(bool traseOvergang)
  {
    this.traseOvergang = traseOvergang;
  }

  public float getPos()
  {
    return tempPoint.z;
  }

  public Vector3 getPoint()
  {
    return point;
  }

  public void setPoint(Vector3 point)
  {
    this.point = point;
  }

  public Vector3 getTempPoint()
  {
    return tempPoint;
  }

  public TrackConnectedObject getTrackSideObject()
  {
    return trackConnectedObject;
  }

  public bool isTrackRunnerNode()
  {
    return trackRunnerNode;
  }

  public void setTrackConnectedObject(TrackConnectedObject trackConnectedObject)
  {
    this.trackConnectedObject = trackConnectedObject;
  }

  /**
   * @return 3d posisjonen (koordinaten) til noden
   */
  public string toString()
  {
    return "x: " + point.x + "\n"
            + "y: " + point.y + "\n"
            + "z: " + point.z + "\n"
            + "pos: " + tempPoint.z + "\n";
  }
}
