using System.Net;
using System.Security.Authentication;
using System.Text;
using Castle.Core.Internal;
using DSS2022.Api.Exceptions;
using DSS2022.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DSS2022.Business.Implementation;

public class BonitaBpmService: IBonitaBpmService
{
    private const String BonitaUrl = "http://localhost:38169/bonita/";

    private void CheckBonitaCredentials(string token, string sessionId)
    {
        if (token.IsNullOrEmpty() || sessionId.IsNullOrEmpty())
        {
            throw new InvalidCredentialException("Bonita credentials are missing ");
        }
    }

    public async Task<string> CreateCase(Collection collection, string processDefinitionId , string token, string sessionId)
    {
        CheckBonitaCredentials(token, sessionId);
        
        var cookieContainer = new CookieContainer();
        using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
        using (var client = new HttpClient(handler))
        {
            var uri = new Uri(BonitaUrl);
            client.BaseAddress = uri;

            this.AddBonitaCookie(cookieContainer, uri, token, sessionId);
            client.DefaultRequestHeaders.Add("X-Bonita-API-Token", token);

            JObject nameJson = new JObject(
                new JProperty("name", "name"),
                new JProperty("value", collection.Name));
            
            JObject descriptionJson = new JObject(
                new JProperty("name", "description"),
                new JProperty("value", collection.Description)); 
            
            JObject releaseDateJson = new JObject(
                new JProperty("name", "releaseDate"),
                new JProperty("value", collection.ReleaseDate.ToString("yyyy-MM-dd")));
            
            JObject manufacturingTimeJson = new JObject(
                new JProperty("name", "manufacturingTime"),
                new JProperty("value", collection.ManufacturingTime));


            JArray collectionBody = new JArray();

            collectionBody.Add(nameJson);
            collectionBody.Add(descriptionJson);
            collectionBody.Add(releaseDateJson);
            collectionBody.Add(manufacturingTimeJson);
            
            JObject body = new JObject(new JProperty("processDefinitionId", processDefinitionId), new JProperty("variables", collectionBody));
            var httpContent = new StringContent(body.ToString(), Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(BonitaUrl + "API/bpm/case", httpContent);
                return await GetStringResponse(response, "caseId");
            }
            catch (HttpRequestException e)
            {
                throw new BonitaConnectionException("Error connecting to BonitaBPM");
            }

        }

        return "";
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
                JObject contractBody = new JObject(
                    new JProperty("name", collection.Name),
                    new JProperty("description", collection.Description),
                    new JProperty("releaseDate", collection.ReleaseDate.ToString("yyyy-MM-dd")),
                    new JProperty("manufacturingTime", collection.ManufacturingTime)
                );
                JObject contract = new JObject(new JProperty("collection_contract", contractBody));
                var httpContent = new StringContent(contractBody.ToString(), Encoding.UTF8, "application/json");
                
                var uri = new Uri(BonitaUrl);
                client.BaseAddress = uri;
                
                AddBonitaCookie(cookieContainer, uri, token, sessionId);
                client.DefaultRequestHeaders.Add("X-Bonita-API-Token", token);
          
                HttpResponseMessage response = await client.PostAsync(BonitaUrl +"API/bpm/process/"+id+"/instantiation", httpContent);
                return await GetStringResponse(response, "caseId");
            }
        }

     public async Task SetVariable(Collection collection, string token, string sessionId, string caseId)
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

                HttpResponseMessage response = await client.GetAsync("/API/bpm/caseVariable/"+caseId+"/"+collection);
            }

        }

     public async Task<string> GetProcessId(string token, string sessionId)
     {
         CheckBonitaCredentials(token, sessionId);

         var processName = "Elaboración de lentes";
         var cookieContainer = new CookieContainer();
         using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
         using (var client = new HttpClient(handler))
         {
             var uri = new Uri(BonitaUrl);
             client.BaseAddress = uri;

             AddBonitaCookie(cookieContainer, uri, token, sessionId);

             try
             {
                 HttpResponseMessage response = await client.GetAsync(BonitaUrl + "API/bpm/process?f=name="+processName+"&p=0&c=1&f=activationState=ENABLED");
                 return await GetGetStringResponseStringResponse(response, "id");
             } catch (HttpRequestException e)
             {
                 throw new BonitaConnectionException("Error connecting to Bonita");
                 
             }
           

             return "";
         }
     }
     
     public async Task<string> GetUserId(string token, string sessionId, string userName)
     {
         var cookieContainer = new CookieContainer();
         using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
         using (var client = new HttpClient(handler))
         {
             var uri = new Uri(BonitaUrl);
             client.BaseAddress = uri;

             this.AddBonitaCookie(cookieContainer, uri, token, sessionId);

             HttpResponseMessage response = await client.GetAsync(BonitaUrl + "API/identity/user?o=userName&s="+userName);

             if (response.IsSuccessStatusCode)
             {
                 var jResult = await GetJsonResponse(response);
                 return jResult[0].Value<string>("id");
             }

             return "";
         }
     }
     
     public async Task<string> GetGroupId(string token, string sessionId, string userId)
     {
         var cookieContainer = new CookieContainer();
         using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
         using (var client = new HttpClient(handler))
         {
             var uri = new Uri(BonitaUrl);
             client.BaseAddress = uri;

             this.AddBonitaCookie(cookieContainer, uri, token, sessionId);

             HttpResponseMessage response = await client.GetAsync(BonitaUrl + "API/identity/membership?f=user_id="+userId+"&d=role_id");
             return await GetGetStringResponseStringResponse(response, "group_id");
         }
     }

     private async Task<string> GetGetStringResponseStringResponse(HttpResponseMessage response, string field)
     {
         if (response.IsSuccessStatusCode)
         {
             var jResult = await GetJsonResponse(response);
             return jResult[0].Value<string>(field);
         }
         
         return "";
     }

     private async Task<JArray> GetJsonResponse(HttpResponseMessage response)
     {
         var responseBodyAsText = await response.Content.ReadAsStringAsync();
         return JsonConvert.DeserializeObject<JArray>(responseBodyAsText);
     }
     
     private async Task<String> GetStringResponse(HttpResponseMessage response, string field)
     {
         if (response.IsSuccessStatusCode)
         {
             var responseBodyAsText = await response.Content.ReadAsStringAsync();
             var jResult = JsonConvert.DeserializeObject<JObject>(responseBodyAsText);
             return (string)jResult[field];
         }

         return "";
     }
}