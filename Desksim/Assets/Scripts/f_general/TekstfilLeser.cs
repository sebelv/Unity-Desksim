using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TekstfilLeser
{

  public static string lesFil(string filnavn)
  {
    string sb = "";
    StreamReader inntekst = new StreamReader(filnavn);

    try
    {
      //leser tegn inntil filslutt
      string innlinje;
      do
      {
        innlinje = inntekst.ReadLine(); //leser en linje
        if (innlinje != null) //null betyr filslutt
        {
          sb += innlinje;
        }
      } while (innlinje != null);

      //Alt er lest. Lukker fila.
      inntekst.Close();

      return sb;
    } catch (IOException ioe)
    {
      return null;
    }
  }

/*
  public static void copyFile(File source, File dest) throws IOException
  {
    InputStream is = null;
    OutputStream os = null;
    try
    {
      is = new FileInputStream(source);
      os = new FileOutputStream(dest);
      byte[] buffer = new byte[1024];
      int length;
      while ((length = is.read(buffer)) > 0)
      {
        os.write(buffer, 0, length);
      }
    } finally
    {
      is.close();
      os.close();
    }
  }*/
}
