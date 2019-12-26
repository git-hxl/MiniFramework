using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLConfig{
    
#if !IsTest
    public static string ResUrl = "http://localhost:8080/MiniFramework";
    public static string AppUrl = "";
    public static string LoginUrl = "";
#else //测试用地址
    public static string ResUrl = "";
    public static string AppUrl = "";
    public static string LoginUrl = "";
#endif

}
