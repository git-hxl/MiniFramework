public class URLConfig{

#if !debug //正式
    public static string ResUrl = "http://localhost:8080/";
    public static string AppUrl = "";
    public static string LoginUrl = "";
#else //测试
    public static string ResUrl = "";
    public static string AppUrl = "";
    public static string LoginUrl = "";
#endif

}