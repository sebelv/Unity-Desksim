using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class MyXmlReader
{
  private static bool reading = true;
  private static List<string> sList = new List<string>();
  
	public static void readFile(string filnavn)
	{
	 // StringBuilder sb = new StringBuilder();
		
	  StreamReader inntekst = new StreamReader(filnavn); //tekstfila som skal leses
    // fjern kommentarer
    
	  try
	  {
		  //leser tegn inntil filslutt
		  string innlinje = null;
      
		  do
		  {
		    innlinje = inntekst.ReadLine(); //leser en linje
		    if ( innlinje != null ) //null betyr filslutt
		    {
          // fjern kommentarer
           processInnLinje(innlinje);
          
          // 
		    	//parse(innlinje); // flytta til for loop nede
		    }
		  } while ( innlinje != null );

		  //Alt er lest. Lukker fila.
		  inntekst.Close();
		  
		  //result();
		  
		  //return sb.toString();
	  }
	  catch(IOException ioe)
	  {
	  	 //return null;
	  }
    
    // parse all etter fjerning av kommentarar
    Debug.Log("ParseAll");
    foreach (string s in sList)
    {
      parse(s);
    }
    
    // reset reading boolean
    reading = true;
    sList.Clear();
	}	
  
  
  
  private static void processInnLinje(string s)
  {
    
    string sb = "";
    
    for (int i = 0; i < s.Length; i++)
    {
      if (reading)
      {
        if (s[i] != '<') // kan ikkje vera kommentar
        {
          sb += s[i];
        }
        else // kan vera kommentar
        {
          if ( (s.Length - i) < 4)
          {
            sb += s[i]; 
          }
          else
          {
            if (s[i+1] == '!' && s[i+2] == '-' && s[i+2] == '-')
            {
              reading = false;
            }
            else
            {
              sb += s[i];
            }
          }
        }
      }
      else // not reading
      {
        if (s[i] == '>' && i > 1) // kan vera slutt på kommentar
        {
          if (s[i-1] == '-' && s[i-2] == '-')
          {
            reading = true;
          }
        }
      }
    }
    
    if (sb.Length > 0)
      sList.Add(sb);
  }

	private static int STATE_FINDSTART = 1;
	private static int STATE_INSIDE_STARTTAG = 2;
	private static int STATE_INSIDE_ENDTAG = 3;
	
	private static int state = STATE_FINDSTART;
	private static string tagName = "";
	private static BaseObjectXML baseObject = new BaseObjectXML();
	
	private static void parse(string line)
	{
		for (int i = 0; i < line.Length; i++)
		{

			int ch = line[i];
			int ch1 = ch;
			if (i < (line.Length - 1))
				ch1 = line[i+1];
			
			if (state == STATE_FINDSTART)
			{	
				if (ch == '<' && ((ch1 >= 'a' && ch1 <= 'z') || (ch1 >= 'A' && ch1 <= 'Z')) ) // starttag found
				{
					state = STATE_INSIDE_STARTTAG;
				}
				else if (ch == '<' && ch1 == '/' ) // endtag found
				{
					state = STATE_INSIDE_ENDTAG;
					i++; // hopp over slash
				}
				else
				{
					//baseObject.content += (char)ch;
					baseObject.appendContent((char)ch);
				}
			}
			else if (state == STATE_INSIDE_STARTTAG)
			{
				if (ch == '>') // end of starttag found
				{
					state = STATE_FINDSTART;
					BaseObjectXML newObj = createObject("BaseObjectXMLa");/*new BaseObjectXML();*/
					newObj.SetParent(baseObject);
					newObj.SetTagName(tagName);
					baseObject.addObject(newObj);
					baseObject = newObj;
					
					tagName = "";
				}
				else
				{
					tagName += (char)ch;
				}
			}
			else if (state == STATE_INSIDE_ENDTAG)
			{
				if (ch == '>') // end of starttag found
				{
					state = STATE_FINDSTART;
					if (/*tagStack.get(tagStack.size()-1)*/baseObject.getTagName() == tagName)
					{
						//tagStack.remove(tagStack.size()-1);
						baseObject = baseObject.getParent();
					}
					else
					{
						Debug.Log("wrong endtag: " + tagName);
						Application.Quit(0);
					}

					tagName = "";
				}
				else
				{
					tagName += (char)ch;
				}
			}

		}

	}
	
	private static BaseObjectXML createObject(string className)
	{
            BaseObjectXML o = new BaseObjectXML();
            Debug.Log("ASKDH ASKDH ");
            return o;
	}
	
	public static BaseObjectXML getBaseObjectXML()
	{
	  BaseObjectXML temp = baseObject;
	  baseObject = new BaseObjectXML();
	  
	  return temp;
	}
	
//	private static void result() 
//	{
//	  for (BaseObjectXML b: baseObject.objectList.get(0).objectList)
//	  {
//		  System.out.println("name: " + b.tagName + "content:%" + b.content + "%");
//	  }
//	}
	
}
