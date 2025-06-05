using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TwoFA.Areas.Identity.Pages
{
    [Authorize(Policy = "RequireTwoFactorOrGoogle")]
    public class TwoFactorPageModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
