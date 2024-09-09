using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjectXML
{
  protected string content = "";
  protected List<BaseObjectXML> objectList = new List<BaseObjectXML>();
  protected BaseObjectXML parent;
  protected string tagName = "first";
  string sb = "";
  
  public void addObject(BaseObjectXML b)
  {
	  objectList.Add(b);
  }
  
  public List<BaseObjectXML> getObjectList()
  {
	return objectList;
  }
  
  public BaseObjectXML getTopObject()
  {
	return objectList[0];
  }
  
  public void appendContent(char c)
  {
	sb += c;
  }
  
  public void clearContent()
  {
	sb = "";
  }
  
  
//  public void clear()
//  {
//	content = "";
//	objectList.clear();
//	parent = null;
//	tagName = "first";
//  }
  
  public string getContent() 
  {
	//return content;
	return sb;
  }

  public BaseObjectXML getParent() 
  {
	return parent;
  }

  public void SetParent(BaseObjectXML parent)
  {
    this.parent = parent;
  }

  public string getTagName() 
  {
	return tagName;
  }

  public void SetTagName(string tagname)
  {
    this.tagName = tagname;
  }

  public string getParameter(string tagName)
  {
    foreach (BaseObjectXML b in objectList)
    {
      if (b.getTagName() == tagName)
    	  return b.getContent();
    }
    
    return "no content";
  }
  
  public List<string> getParameterList(string tagName)
  {
    List<string> list = new List<string>();
    foreach (BaseObjectXML b in objectList)
    {
      if (b.getTagName() == (tagName))
    	  list.Add(b.getContent());
    }
    
    return list;
  }
}
