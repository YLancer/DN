using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class TestFunc : MonoBehaviour
{
    int Id;
    XElement root = XElement.Load("11");
    ListFunc newTest = new ListFunc("abc", 1);
   List<ListFunc> DataList = new List<ListFunc>();


    public void Start()
    {
        DataList.Add(newTest);
        int.TryParse(root.Element("ID").Value,out Id);
        
    }
}
