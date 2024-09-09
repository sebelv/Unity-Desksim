using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenerellTrase
{
	// start og sluttpunkt i 3d-verdenen
	protected Vector3 startPunkt, sluttPunkt;
	private Transform t;
	//avsnittets lengde
	protected double lengde;
	
	// start og sluttpos langs hele traseen
	protected double startpos, sluttPos;

	/**
	 * Etablerer et traseavsnitt ved lagre sentrle verdier
	 * 
	 * @param startPunkt start i 3d-verdenen
	 * @param sluttPunkt sluttpunkt i 3d-verdenen
	 * @param startPos startposisjonen for dette avsnittet langs hele 
	 *  traseen
	 */
	public GenerellTrase(Vector3 startPunkt, Vector3 sluttPunkt, double startPos)
	{
		this.startPunkt = startPunkt;
		this.sluttPunkt = sluttPunkt;
		this.startpos = startPos;
	}

	// abstrakte metoder
	
	/**
	 * Finner traseens punkt (koordinat) i 3d-verdenen basert
	 * på posisjonen langs traseen (avstanden fra starten av traseen)
	 * 
	 * @return punktet i 3d-verdenen
	 */
	public abstract Vector3 finn3DPunktIPos(float pos);
	
	/**
	 * Finner traseens retning i 3d-verdenen basert
	 * på posisjonen langs traseen (avstanden fra starten av traseen)
	 * 
	 * @param pos posisjonen langs traseen (avstanden fra starten av traseen)
	 * @return  punktet i 3d-verdenen
	 */
	public abstract float finnVinkelIPos(float pos);
  
  public abstract void finnVertexITraseVertex(Vector3 vecIn, Vector3 vecOut);

	// tilgangsmetoder
	
	public double getLengde()
	{
		return lengde;
	}

	public double getStartPos()
	{
		return startpos;
	}

	public double getSluttPos()
	{
		return sluttPos;
	}
	
	public Vector3 getStartPunkt() 
	{
		return startPunkt;
	}

	public Vector3 getSluttPunkt() 
	{
		return sluttPunkt;
	}
	
	public string toString()
	{
        string sb = "";
		sb += (" " + startPunkt);
		sb += (" " + sluttPunkt);
		
		return sb;
	}
}
