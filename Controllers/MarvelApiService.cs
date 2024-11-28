using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

public class MarvelApiService
{
    private readonly HttpClient _httpClient;
    private const string _PublicKey="536575a614446541267360549534f659";
    private const string _PrivateKey="924598fad4a75f0873309f5846a7dbd128393898";
   private string  BaseUrl= "https://gateway.marvel.com:443/v1/public/";
 // 

    public MarvelApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
       
    }

    private string GenerateHash(string timestamp)
    {
        var input = $"{timestamp}{_PrivateKey}{_PublicKey}";
        using var md5 = MD5.Create();
        var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    public async Task<T> GetAsync<T>(string endpoint, int limit=90, int offset=0)
    {
        var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
        var hash = GenerateHash(timestamp);

        var url = $"{BaseUrl}{endpoint}?ts={timestamp}&apikey={_PublicKey}&hash={hash}&limit={limit}&offset={offset}";
        var response = await _httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T>(json);
       
    }
     public async Task<T> getcharacters<T>(string query)
    {
        var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
        var hash = GenerateHash(timestamp);

        var url = $"{BaseUrl}characters?nameStartsWith={query}&ts={timestamp}&apikey={_PublicKey}&hash={hash}&limit=90";
        var response = await _httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T>(json);
       
    }
     public async Task<T> getcharactersByName<T>(string query)
    {
        var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
        var hash = GenerateHash(timestamp);

        var url = $"{BaseUrl}characters?name={query}&ts={timestamp}&apikey={_PublicKey}&hash={hash}&limit=10";
        var response = await _httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T>(json);
       
    }
}
 