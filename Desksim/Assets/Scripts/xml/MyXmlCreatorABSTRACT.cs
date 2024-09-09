using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public abstract class MyXmlCreatorABSTRACT
{
  public void createObjects(List<BaseObjectXML> xmlList)
  {
  	foreach (BaseObjectXML b in xmlList)
  	{
  	  methodInvoker(b);
  	}
  }

  protected abstract void methodInvoker(BaseObjectXML bo);
  
  protected int c_int(string s)
  {
    int r = int.Parse(s);
    return r;
  }
  
  protected bool c_boolean(string s)
  {
		if (s == "true" || s == "TRUE")
			return true;
		
		return false;
  }
    
  protected float c_float(string s)
  {
    float r = float.Parse(s, CultureInfo.InvariantCulture);
    return r;
  }
  
  protected Vector3 c_vector3f(string s)
  {
	  float x, y, z;
		string[] sa = s.Split(",");
		
		if (sa.Length != 3)
			return new Vector3();
        x = c_float(sa[0]);
        y = c_float(sa[1]);
        z = c_float(sa[2]);
		
		return new Vector3(x, y, z);
  }
  
  protected Vector3[] c_vector3fArray(string s)
  {
  	List<Vector3> list = c_vector3fList(s);
  	Vector3[] array = new Vector3[list.Count];
  	array = list.ToArray();
  	return array;
  }
  
	protected List<Vector3> c_vector3fList(string s)
	{
		s = s.Replace("\\s", "");
		
		string[] sa = s.Split(",");

		List<Vector3> pList = new List<Vector3>();
        for (int k = 0; k < (sa.Length / 3) * 3; k += 3)
        {
            float x = float.Parse(sa[k]);
            float y = float.Parse(sa[k+1]);
            float z = float.Parse(sa[k+2]);
            
            pList.Add(new Vector3(x, y, z));
        }
		
		return pList;
	}
	
  protected int[] c_integerArray(string s)
  {
  	List<int> list = c_integerList(s);
  	int[] array = new int[list.Count];
  	
  	int k = 0;
  	foreach (int i in list)
  	{
  		array[k] = i;
  		k++;
  	}
  	
  	return array;
  }
  
  protected float[] c_floatArray(string s)
  {
  	List<float> list = c_floatList(s);
  	float[] array = new float[list.Count];
  	
  	int k = 0;
  	foreach (float i in list)
  	{
  		array[k] = i;
  		k++;
  	}
  	
  	return array;
  }
	
	protected List<int> c_integerList(string s)
	{
		s = s.Replace("\\s", "");
		
		string[] sa = s.Split(",");

		List<int> iList = new List<int>();
        for (int k = 0; k < sa.Length; k++)
        {
            int i = int.Parse(sa[k]);
            
            iList.Add(i);
        }
		
		return iList;

	}
  
	protected List<float> c_floatList(string s)
	{
		s = s.Replace("\\s", "");
		
		string[] sa = s.Split(",");

		List<float> iList = new List<float>();		
        for (int k = 0; k < sa.Length; k++)
        {
            float i = float.Parse(sa[k]);
            
            iList.Add(i);
        }
		
		return iList;

	}

}
