// Move all top-level statements into a Main method within a Program class to resolve CS8803.

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

public class Program
{
    public static async Task Main(string[] args)
    {
        // DTOs
        var apiBaseUrl = "http://api:8080";

        var loginUrl = $"{apiBaseUrl}/auth/login";
        var protectedUrl = $"{apiBaseUrl}/datawarehouse/test";

        using var http = new HttpClient();

        // Step 1: Login
        var loginPayload = new LoginRequest("kontakt@kpdesign.dk", "Kode1234!");
        var loginResponse = await http.PostAsJsonAsync(loginUrl, loginPayload);

        if (!loginResponse.IsSuccessStatusCode)
        {
            Console.WriteLine($"Login failed: {loginResponse.StatusCode}");
            return;
        }

        var loginData = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        if (loginData is null)
        {
            Console.WriteLine("Could not parse login response.");
            return;
        }

        Console.WriteLine($"Token received: {loginData.Token.Substring(0, 20)}...");

        // Step 2: Call protected endpoint
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Token);
        var response = await http.GetAsync(protectedUrl);
        var result = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"API response: {result}");
        }
        else
        {
            Console.WriteLine($"Call failed: {response.StatusCode}");
        }
    }
}

// DTOs
public record LoginRequest(string Email, string Password);
public record LoginResponse(string Token);
