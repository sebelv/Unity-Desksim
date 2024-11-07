using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwitchPositioner : TrackConnectedObject
{
    /*
    setNoTrackNode(true);
    
    dataPakke = new DataPakkeAlpha();        
    */
    
    /*
    if (StartParameters.samkjoringStartes*Program.samkjNettKlientOpprettet && !onCurveTr)
    {
      Program.leggTilNettObjektAlpha(this);
      samkjNettObjekt = new SamkjNettObjekt();
      samkjNettObjekt.setSwitchPositioner(this);
      spvFrigittKomp = new SpvFrigittKomp(this, null);
    }*/

  public override void initAll()
  {

//    // fjern handoperation dersom ikke detailed components
//    if (handOperation && !Program.loadDetailedComponents)
//    {
//      handOperation = false;
//    }
//    //
    
    this.modelFile = "spvmotor.ac";
    if (id.Contains("lodd"))
    {
      modelFile = "lodd.ac";
      
      // dersom egen funksjonell modell
      /*
      if (Program.loadAllFunksjonalitet)
      {
        handOperation = true;
        modelFile = null;        
      }
      */
    }
    /*
    base.initAll();
    typeIndex = 100; 
    
    if (lokalStiller)
    {
      initLokalStiller(assetManager);
    }
    else if (handOperation)
    {
      initHandOperationHandle(assetManager);
    }
    
    // audio
    initLyder();
    */
  }
}
