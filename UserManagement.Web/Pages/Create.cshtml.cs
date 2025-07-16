using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using UserManagement.Web.Models;

public class CreateModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;
    [BindProperty] public User User { get; set; } = new();

    public CreateModel(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var client = _clientFactory.CreateClient("UserApi");
        var json = JsonConvert.SerializeObject(User);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("Users", content);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToPage("/Index");
        }

        ModelState.AddModelError(string.Empty, "Error creating user.");
        return Page();
    }
}