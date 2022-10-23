namespace Cities_Latitude_Longitude;

public static class Application
{
    /// <summary>
    /// </summary>
    /// <returns> root where the executing assembly is currently located
    /// EX : C:\Users\userName\source\repos\SulutionName\ProjectName\
    /// </returns>
    public static string GetAssemblyRootPath()
    {
        string path
            = System.Reflection.Assembly.GetExecutingAssembly().Location;
        return ExtractRootPath(path);
    }   /// <summary>
    /// </summary>
    /// <returns> root where the executing assembly is currently located
    /// EX : C:\Users\userName\source\repos\SulutionName\ProjectName\
    /// </returns>
    public static string GetSolutionRootPath()
    {
        string path
            = System.Reflection.Assembly.GetExecutingAssembly().Location;
        return ExtractSolutionPath(path) + "\\";
    }
    /// <summary>
    /// This will get the root path
    ///  example
    /// C:\Users\indri\source\repos\Cities_Latitude_Longitude\Cities_Latitude_Longitude\bin\Debug\net6.0
    /// </summary>
    /// <param name="path"></param>
    /// <returns>reurns the application </returns>
    private static string ExtractSolutionPath(string path)
    {
        //C:\Users\indri\source\repos\Cities_Latitude_Longitude\Cities_Latitude_Longitude\
        var executingAssemblyPath = ExtractRootPath(path);
        var sp = executingAssemblyPath.Split("\\");
       return string.Join("\\", sp.Take(sp.Length - 2));
    } 
    /// <summary>
    ///  returns where the executing assembly is currently located
    ///  example
    /// C:\Users\indri\source\repos\Cities_Latitude_Longitude\Cities_Latitude_Longitude\bin\Debug\net6.0
    /// </summary>
    /// <param name="path"></param>
    /// <returns>reurns the application
    /// ex: C:\Users\indri\source\repos\Cities_Latitude_Longitude\Cities_Latitude_Longitude\</returns>
    private static string ExtractRootPath(string path) =>
        path.Split("bin")[0];

}