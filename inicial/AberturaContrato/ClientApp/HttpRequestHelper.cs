using System.Net.Http;

/// <summary>
/// 
/// </summary>
public class HttpRequestHelper
{
    /// <summary>
    /// Executa chamada http de forma resiliente.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="tries"></param>
    /// <returns></returns>
    public static HttpResponseMessage TryGetHTTPResponse(string url, int? tries = 1)
    {
        try
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(url).Result;

            return response;
        }
        catch
        {
            if (tries < 4)
            {
                System.Threading.Thread.Sleep((int)(1000 * tries));
                return TryGetHTTPResponse(url, ++tries);
            }
        }

        return null;
    }
}
