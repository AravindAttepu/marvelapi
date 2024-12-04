using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

// It is better to have an interface at any time. understand SOLID and importance of decoupled code
public class MarvelApiService
{
    private readonly HttpClient _httpClient;
    // Never commit these onto public repositories... try to understand more about config and secret management. git doesn't forget.
    private const string _PublicKey = "536575a614446541267360549534f659";
    private const string _PrivateKey = "924598fad4a75f0873309f5846a7dbd128393898";
    private string BaseUrl = "https://gateway.marvel.com:443/v1/public/";
    // why leave empty comments? why are not your files formatted

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

    // You can return HttpResponse instead of generics here
    public async Task<T> GetAsync<T>(string endpoint, int limit = 90, int offset = 0)
    {
        var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
        var hash = GenerateHash(timestamp);

        // One should parse and build the Url properly use UriBuilder or similar things
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
