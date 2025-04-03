using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ETicaret.Infrastructure.TagHelpers
{
    [HtmlTargetElement("td", Attributes = "user-role")]
    public class UserRoleTagHelper : TagHelper
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        [HtmlAttributeName("user-name")]
        public string? UserName { get; set; }

        public UserRoleTagHelper(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            TagBuilder ul = new TagBuilder("ul");

            var roles = _roleManager.Roles.ToList().Select(r => r.Name);

            foreach (var role in roles)
            {
                TagBuilder li = new TagBuilder("li");
                TagBuilder icon = new TagBuilder("i");
                var result = await _userManager.IsInRoleAsync(user, role);
                icon.Attributes["class"] = result ? "fa-solid fa-circle-check" : "fa-solid fa-circle-xmark";
                li.InnerHtml.Append($"{role} : ");
                li.InnerHtml.AppendHtml(icon);
                ul.InnerHtml.AppendHtml(li);
            }

            output.Content.AppendHtml(ul);
        }
    }


}
