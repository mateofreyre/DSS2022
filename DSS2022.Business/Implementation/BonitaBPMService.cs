using System.Net;
using System.Text;
using DSS2022.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DSS2022.Business.Implementation;

public class BonitaBpmService: IBonitaBpmService
{
    private const String BonitaUrl = "http://localhost:38169/bonita/";

    public async Task CreateCase(Collection collection, string processDefinitionId , string token, string sessionId)
    {
     
        var cookieContainer = new CookieContainer();
        using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
        using (var client = new HttpClient(handler))
        {
            var uri = new Uri(BonitaUrl);
            client.BaseAddress = uri;

            this.AddBonitaCookie(cookieContainer, uri, token, sessionId);
            client.DefaultRequestHeaders.Add("X-Bonita-API-Token", token);

            JObject body = new JObject(new JProperty("processDefinitionId", processDefinitionId));
            var httpContent = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
            
            
            HttpResponseMessage response = await client.PostAsync(BonitaUrl + "API/bpm/case", httpContent);
            
            if (response.IsSuccessStatusCode)
            {
                var responseBodyAsText = await response.Content.ReadAsStringAsync();
                var jResult = JsonConvert.DeserializeObject<JArray>(responseBodyAsText);
                return;
            }
        }

    }

    private void AddBonitaCookie(CookieContainer cookieContainer, Uri uri, string token, string sessionId)
    {
        cookieContainer.Add(uri, new Cookie("X-Bonita-API-Token", token));
        cookieContainer.Add(uri, new Cookie("JSESSIONID", sessionId));
    }

    public async Task<string> StartProcess(Collection collection, string id, string token, string sessionId)
        {
       
            var cookieContainer = new CookieContainer();
            using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            using (var client = new HttpClient(handler))
            {
                JObject body = new JObject(
                    new JProperty("name", collection.Name),
                    new JProperty("description", collection.Description),
                    new JProperty("releaseDate", collection.ReleaseDate.ToString())
                );
                var httpContent = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
                
                var uri = new Uri(BonitaUrl);
                client.BaseAddress = uri;
                
                AddBonitaCookie(cookieContainer, uri, token, sessionId);
                client.DefaultRequestHeaders.Add("X-Bonita-API-Token", token);
          
                HttpResponseMessage response = await client.PostAsync(BonitaUrl +"API/bpm/process/"+id+"/instantiation", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseBodyAsText = await response.Content.ReadAsStringAsync();
                    var jResult = JsonConvert.DeserializeObject<JObject>(responseBodyAsText);
                    return (string)jResult["caseId"];
                }

                return "";
            }
        }

     public async Task SetVariable(Collection collection, string token, string sessionId)
        {
            var handler = new HttpClientHandler();
            using (var client = new HttpClient(handler))
            {
              
                var uri = new Uri(BonitaUrl);
                client.BaseAddress = uri;


                HttpResponseMessage responseTaskID = await client.GetAsync("/API/bpm/userTask/" + 1);

                var content1 = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("ticket_account", ""),
                    new KeyValuePair<string, string>("ticket_description", ""),
                    new KeyValuePair<string, string>("ticket_subject", "")
                });

                var taskId = 1;

                HttpResponseMessage response = await client.GetAsync("/API/bpm/caseVariable/"+taskId+"/"+collection);
            }

        }

     public async Task<string> GetProcessId(string token, string sessionId)
     {
         var processName = "Elaboración de lentes";
         var cookieContainer = new CookieContainer();
         using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
         using (var client = new HttpClient(handler))
         {
             var uri = new Uri(BonitaUrl);
             client.BaseAddress = uri;

             this.AddBonitaCookie(cookieContainer, uri, token, sessionId);

             HttpResponseMessage response = await client.GetAsync(BonitaUrl + "API/bpm/process?f=name="+processName+"&p=0&c=1&f=activationState=ENABLED");

             if (response.IsSuccessStatusCode)
             {
                 var responseBodyAsText = await response.Content.ReadAsStringAsync();
                 var jResult = JsonConvert.DeserializeObject<JArray>(responseBodyAsText);
                 var processDefinitionId = jResult[0].Value<string>("id");

                 return processDefinitionId;
             }

             return "";
         }
     }
}