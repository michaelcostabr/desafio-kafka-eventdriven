using System;
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

            //if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            //{
            //    return null;
            //}

            //response.EnsureSuccessStatusCode();
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <param name="content"></param>
    /// <param name="tries"></param>
    /// <returns></returns>
    public static HttpResponseMessage TryPatchHTTPResponse(string url, HttpContent content, int? tries = 1)
    {
        try
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.PatchAsync(url, content).Result;
            return response;
        }
        catch
        {
            if (tries < 4)
            {
                System.Threading.Thread.Sleep((int)(1000 * tries));
                return TryPatchHTTPResponse(url, content, ++tries);
            }
        }

        return null;
    }
}
