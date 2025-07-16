using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using UserManagement.Web.Models;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;
    public List<User> Users { get; set; } = new();

    public IndexModel(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task OnGetAsync()
    {
        var client = _clientFactory.CreateClient("UserApi");
        var response = await client.GetAsync("Users");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            Users = JsonConvert.DeserializeObject<List<User>>(json)!;
        }
    }
}
