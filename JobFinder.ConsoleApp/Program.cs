using JobFinder.Domain.Models.DTOs;
using JobFinder.Domain.Models.ValueObjects;
using RestSharp;
using System.Text.Json;

namespace JobFinder.ConsoleApp;

internal class Program
{
    private static readonly RestClient _client = new RestClient("http://localhost:5155/api");
    private static List<string> _jobDescriptionId = new List<string>();
    private static string _applicationId = string.Empty;
    private static string _applicationAuthCode = string.Empty;
    private static List<string> _jobId = new List<string>();
    private static int _selectedJobIndex = 1;

    static async Task Main(string[] args)
    {
        Console.WriteLine("Jobs List:");
        Console.WriteLine("-----------------------------------------");
        await ShowJobs(3);
        Console.WriteLine("-----------------------------------------");
        Console.WriteLine($"Job #2 selected...");
        Console.WriteLine("-----------------------------------------");
        await ShowJobDescription(_jobDescriptionId[_selectedJobIndex]);
        Console.WriteLine("-----------------------------------------");
        await GetCredential();
        Console.WriteLine("-----------------------------------------");
        await FillApplicantForm();
        Console.WriteLine("-----------------------------------------");
        await SubmitResume();
        Console.WriteLine("-----------------------------------------");
        await SubmitMotivationLetter();
        Console.WriteLine("-----------------------------------------");
        await SubmitApplication();
        Console.WriteLine("-----------------------------------------");
        await GetApplication();

        Console.ReadLine();
    }


    private static async Task GetCredential()
    {
        var request = new RestRequest($"job/credentials", Method.Post);
        request.AddHeader("ChallengeType", "recaptcha");
        request.AddHeader("Challengekey", "Challenge");
        var response = await _client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            var content = JsonSerializer.Deserialize<CredentialsResponse>(response.Content);
            if (content != null)
            {
                _applicationAuthCode = content.AuthCode ?? string.Empty;
                _applicationId = content.Id?.ToString() ?? string.Empty;
                Console.WriteLine($"Application Id: {_applicationId} {Environment.NewLine}ApplicationAuthCode: {_applicationAuthCode} {Environment.NewLine}TimeStamp: {content.ExpirationTimestamp}");
            }
            else
            {
                Console.WriteLine("Failed to deserialize credentials.");
            }
        }
        else
        {
            Console.WriteLine("Request failed with status: " + response.StatusCode);
            Console.WriteLine("Error message: " + response.ErrorMessage);
        }
    }

    private static async Task ShowJobDescription(string? jobDescriptionId)
    {
        var request = new RestRequest($"job/{jobDescriptionId}", Method.Get);
        var response = await _client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            var jobdesc = JsonSerializer.Deserialize<JobDescription>(response.Content);
            if (jobdesc != null)
            {
                Console.WriteLine($"{jobdesc.JobDescriptionId} {Environment.NewLine}{jobdesc.OffersTitle} {Environment.NewLine}{jobdesc.Description}");
            }
            else
            {
                Console.WriteLine("Failed to deserialize JobDescription.");
            }
        }
        else
        {
            Console.WriteLine("Request failed with status: " + response.StatusCode);
            Console.WriteLine("Error message: " + response.ErrorMessage);
        }
    }

    private static async Task ShowJobs(int jobCount = 10, string? searchTerm = "")
    {
        JobSearchParameters jobSearchParameters = new JobSearchParameters()
        {
            Limit = jobCount,
            Offset = 0,
            SearchTerm = searchTerm,
            SortDirection = "Ascending",
            SortField = "EmploymentLevel"
        };
        var request = new RestRequest("Job/search", Method.Post);
        var response = await _client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            var content = JsonSerializer.Deserialize<SearchJobsResponse>(response.Content);
            if (content != null)
            {
                var jobs = content.Jobs.Take(jobCount)
                    .Select(p => new { p.JobDescriptionId, p.AccountingCompany, p.Title, p.JobId });
                int cnt = 1;
                foreach (var job in jobs)
                {
                    _jobDescriptionId.Add(job.JobDescriptionId!);
                    _jobId.Add(job.JobId!);
                    Console.WriteLine($"{cnt++}. {job.JobDescriptionId}: {job.AccountingCompany} - {job.Title}");
                }
            }
            else
            {
                Console.WriteLine("Failed to deserialize SearchJobsResponse.");
            }
        }
        else
        {
            Console.WriteLine("Request failed with status: " + response.StatusCode);
            Console.WriteLine("Error message: " + response.ErrorMessage);
        }
    }

    private static async Task FillApplicantForm()
    {
        var request = new RestRequest($"job/{_applicationId}/applicant", Method.Put);
        request.AddHeader("application-auth-code", _applicationAuthCode);
        request.AddHeader("correlation-id", Guid.NewGuid().ToString());
        request.AddParameter("applicationId", _applicationId);
        // Add form data
        request.AddParameter("firstName", "Alex");
        request.AddParameter("lastName", "B");
        request.AddParameter("gender", "Male");
        request.AddParameter("nationality", "HUN");
        request.AddParameter("telephoneNumber", "+36303030303");
        request.AddParameter("email", "aaa@gmail.com");
        request.AddParameter("birthDate", "01-01-2000");
        request.AddParameter("countryCode", "HUN");
        request.AddParameter("zip", "1085");
        request.AddParameter("city", "Budapest");
        request.AddParameter("street", "street1");
        request.AddParameter("workPermit", "true");
        request.AddParameter("DriversLicenseClasses", "B1");
        request.AddParameter("employedBefore", "false");
        request.AddParameter("militaryServiceFinished", "true");
        var response = await _client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            Console.WriteLine($"Applicant Registered!");
        }
        else
        {
            Console.WriteLine("Request failed with status: " + response.StatusCode);
            Console.WriteLine("Error message: " + response.ErrorMessage);
        }
    }

    private static async Task SubmitDocuments(Document document)
    {
        try
        {
            if (document == null)
            {
                Console.WriteLine("Document is null");
                return;
            }

            var request = new RestRequest($"job/upload-document/{_applicationId}", Method.Post);
            request.AddHeader("application-auth-code", _applicationAuthCode);
            request.AddHeader("correlation-id", Guid.NewGuid().ToString());
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            //string documentJson = JsonSerializer.Serialize(document);
            //Console.WriteLine("Document JSON: " + documentJson);

            request.AddJsonBody(document);

            var response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                Console.WriteLine("Document Sent!");
            }
            else
            {
                Console.WriteLine("Request failed with status: " + response.StatusCode);
                Console.WriteLine("Error message: " + response.ErrorMessage);
                Console.WriteLine("Response content: " + response.Content);  // Log the response content for further debugging
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
        }
    }

    private static async Task SubmitResume()
    {
        var document = new Document()
        {
            DocumentType = "CV",
            DocumentName = "cv.pdf",
            DocumentBlob = "JVBERi0xLjMKMyAwIG9iago8PC9UeXBlIC9QYWdlCi9QYXJlbnQgMSAwIFIKL1Jlc291cmNlcyAyIDAgUgovQ29udGVudHMgNCAwIFI+PgplbmRvYmoKNCAwIG9iago8PC9GaWx0ZXIgL0ZsYXRlRGVjb2RlIC9MZW5ndGggMTE2Pj4Kc3RyZWFtCnicNYyxCoMwFEV3v+KO7fJMUs3DVTRDp0LfDwSMaElAcNDPV4OFOx3OuQbvQlHN2IpWUDoNbUgpyIheLmSspYrBTU3MkAGPr09LDPh0Dm6O4Qn53e5Lk27AlqkyWZVpXnHOY00+xtyMZ4Ow5xP6xwdvESKGCmVuZHN0cmVhbQplbmRvYmoKMSAwIG9iago8PC9UeXBlIC9QYWdlcwovS2lkcyBbMyAwIFIgXQovQ291bnQgMQovTWVkaWFCb3ggWzAgMCA1OTUuMjggODQxLjg5XQo+PgplbmRvYmoKNSAwIG9iago8PC9UeXBlIC9Gb250Ci9CYXNlRm9udCAvSGVsdmV0aWNhCi9TdWJ0eXBlIC9UeXBlMQovRW5jb2RpbmcgL1dpbkFuc2lFbmNvZGluZwo+PgplbmRvYmoKMiAwIG9iago8PAovUHJvY1NldCBbL1BERiAvVGV4dCAvSW1hZ2VCIC9JbWFnZUMgL0ltYWdlSV0KL0ZvbnQgPDwKL0YxIDUgMCBSCj4+Ci9YT2JqZWN0IDw8Cj4+Cj4+CmVuZG9iago2IDAgb2JqCjw8Ci9Qcm9kdWNlciAoUHlGUERGIDEuNy4yIGh0dHA6Ly9weWZwZGYuZ29vZ2xlY29kZS5jb20vKQovQ3JlYXRpb25EYXRlIChEOjIwMjQwNTE2MjAxNTE5KQo+PgplbmRvYmoKNyAwIG9iago8PAovVHlwZSAvQ2F0YWxvZwovUGFnZXMgMSAwIFIKL09wZW5BY3Rpb24gWzMgMCBSIC9GaXRIIG51bGxdCi9QYWdlTGF5b3V0IC9PbmVDb2x1bW4KPj4KZW5kb2JqCnhyZWYKMCA4CjAwMDAwMDAwMDAgNjU1MzUgZiAKMDAwMDAwMDI3MyAwMDAwMCBuIAowMDAwMDAwNDU2IDAwMDAwIG4gCjAwMDAwMDAwMDkgMDAwMDAgbiAKMDAwMDAwMDA4NyAwMDAwMCBuIAowMDAwMDAwMzYwIDAwMDAwIG4gCjAwMDAwMDA1NjAgMDAwMDAgbiAKMDAwMDAwMDY2OSAwMDAwMCBuIAp0cmFpbGVyCjw8Ci9TaXplIDgKL1Jvb3QgNyAwIFIKL0luZm8gNiAwIFIKPj4Kc3RhcnR4cmVmCjc3MgolJUVPRgo="
        };
        await SubmitDocuments(document);
    }

    private static async Task SubmitMotivationLetter()
    {
        var document = new Document()
        {
            DocumentType = "MOTIVATIONALLETTER",
            DocumentName = "cv.pdf",
            DocumentBlob = "JVBERi0xLjMKMyAwIG9iago8PC9UeXBlIC9QYWdlCi9QYXJlbnQgMSAwIFIKL1Jlc291cmNlcyAyIDAgUgovQ29udGVudHMgNCAwIFI+PgplbmRvYmoKNCAwIG9iago8PC9GaWx0ZXIgL0ZsYXRlRGVjb2RlIC9MZW5ndGggMTE2Pj4Kc3RyZWFtCnicNYyxCoMwFEV3v+KO7fJMUs3DVTRDp0LfDwSMaElAcNDPV4OFOx3OuQbvQlHN2IpWUDoNbUgpyIheLmSspYrBTU3MkAGPr09LDPh0Dm6O4Qn53e5Lk27AlqkyWZVpXnHOY00+xtyMZ4Ow5xP6xwdvESKGCmVuZHN0cmVhbQplbmRvYmoKMSAwIG9iago8PC9UeXBlIC9QYWdlcwovS2lkcyBbMyAwIFIgXQovQ291bnQgMQovTWVkaWFCb3ggWzAgMCA1OTUuMjggODQxLjg5XQo+PgplbmRvYmoKNSAwIG9iago8PC9UeXBlIC9Gb250Ci9CYXNlRm9udCAvSGVsdmV0aWNhCi9TdWJ0eXBlIC9UeXBlMQovRW5jb2RpbmcgL1dpbkFuc2lFbmNvZGluZwo+PgplbmRvYmoKMiAwIG9iago8PAovUHJvY1NldCBbL1BERiAvVGV4dCAvSW1hZ2VCIC9JbWFnZUMgL0ltYWdlSV0KL0ZvbnQgPDwKL0YxIDUgMCBSCj4+Ci9YT2JqZWN0IDw8Cj4+Cj4+CmVuZG9iago2IDAgb2JqCjw8Ci9Qcm9kdWNlciAoUHlGUERGIDEuNy4yIGh0dHA6Ly9weWZwZGYuZ29vZ2xlY29kZS5jb20vKQovQ3JlYXRpb25EYXRlIChEOjIwMjQwNTE2MjAxNTE5KQo+PgplbmRvYmoKNyAwIG9iago8PAovVHlwZSAvQ2F0YWxvZwovUGFnZXMgMSAwIFIKL09wZW5BY3Rpb24gWzMgMCBSIC9GaXRIIG51bGxdCi9QYWdlTGF5b3V0IC9PbmVDb2x1bW4KPj4KZW5kb2JqCnhyZWYKMCA4CjAwMDAwMDAwMDAgNjU1MzUgZiAKMDAwMDAwMDI3MyAwMDAwMCBuIAowMDAwMDAwNDU2IDAwMDAwIG4gCjAwMDAwMDAwMDkgMDAwMDAgbiAKMDAwMDAwMDA4NyAwMDAwMCBuIAowMDAwMDAwMzYwIDAwMDAwIG4gCjAwMDAwMDA1NjAgMDAwMDAgbiAKMDAwMDAwMDY2OSAwMDAwMCBuIAp0cmFpbGVyCjw8Ci9TaXplIDgKL1Jvb3QgNyAwIFIKL0luZm8gNiAwIFIKPj4Kc3RhcnR4cmVmCjc3MgolJUVPRgo="
        };
        await SubmitDocuments(document);
    }

    private static async Task SubmitApplication()
    {
        try
        {
            var request = new RestRequest($"job/submit/{_applicationId}", Method.Post);
            request.AddHeader("application-auth-code", _applicationAuthCode);
            request.AddHeader("correlation-id", Guid.NewGuid().ToString());
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Accept", "application/json");

            request.AddParameter("jobId", _jobId[_selectedJobIndex]);
            request.AddParameter("desiredSalary", "1000");
            request.AddParameter("availableFrom", "10-10-2024");
            request.AddParameter("agreedToDataProcessing", "true");
            request.AddParameter("agreedToDataRelaying", "true");
            //request.AddParameter("storeIdList", jobApplication.StoreIdList);
            //request.AddParameter("recommendedBy", jobApplication.RecommendedBy);
            //request.AddParameter("externalSource", jobApplication.ExternalSource);


            var response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                Console.WriteLine("Congratulation, Application submitted!");
            }
            else
            {
                Console.WriteLine("Request failed with status: " + response.StatusCode);
                Console.WriteLine("Error message: " + response.ErrorMessage);
                Console.WriteLine("Response content: " + response.Content);  // Log the response content for further debugging
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
        }

    }

    private static async Task GetApplication()
    {
        try
        {
            var request = new RestRequest($"job/application/{_applicationId}", Method.Get);
            request.AddHeader("application-auth-code", _applicationAuthCode);
            request.AddHeader("correlation-id", Guid.NewGuid().ToString());
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Accept", "application/json");

            var response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                Console.WriteLine("Congratulation, Application submitted!");
            }
            else
            {
                Console.WriteLine("Request failed with status: " + response.StatusCode);
                Console.WriteLine("Error message: " + response.ErrorMessage);
                Console.WriteLine("Response content: " + response.Content);  // Log the response content for further debugging
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
        }

    }

}
