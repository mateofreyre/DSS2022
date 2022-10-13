using DSS2022.Model;

namespace DSS2022.Business;

public interface IBonitaBpmService
{
    Task<string> StartProcess(Collection collection, string id, string token, string sessionId);
    Task SetVariable(Collection collection, string token, string sessionId, string caseId);
    Task<string>  GetProcessId(string token, string sessionId);
    Task<string> CreateCase(Collection collection, string processDefinitionId, string token, string sessionId);
}