using Entities.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Services.Contracts;

namespace ETicaret.Infrastructe.TagHelpers
{
    [HtmlTargetElement("div", Attributes = "products")]
    public class LastestProductTagHelper : TagHelper
    {
        private readonly IServiceManager _manager;

        [HtmlAttributeName("number")]
        public int Number { get; set; }

        public LastestProductTagHelper(IServiceManager manager)
        {
            _manager = manager;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder card = new TagBuilder("div");
            card.AddCssClass("card mb-3 shadow-sm");

            TagBuilder cardHeader = new TagBuilder("div");
            cardHeader.AddCssClass("card-header bg-white");
            TagBuilder headerH6 = new TagBuilder("h6");
            headerH6.AddCssClass("mb-0");

            TagBuilder icon = new TagBuilder("i");
            icon.AddCssClass("fa fa-box text-secondary me-2");
            headerH6.InnerHtml.AppendHtml(icon);
            headerH6.InnerHtml.Append("Ürünler");

            cardHeader.InnerHtml.AppendHtml(headerH6);
            card.InnerHtml.AppendHtml(cardHeader);

            TagBuilder ul = new TagBuilder("ul");
            ul.AddCssClass("list-group list-group-flush");

            var products = _manager.ProductService.GetLastestProducts(Number, false);
            foreach (Product product in products)
            {
                TagBuilder li = new TagBuilder("li");
                li.AddCssClass("list-group-item");

                TagBuilder a = new TagBuilder("a");
                a.Attributes.Add("href", $"product/get/{product.ProductId}");
                a.AddCssClass("text-decoration-none");
                a.InnerHtml.Append(product.ProductName);

                li.InnerHtml.AppendHtml(a);
                ul.InnerHtml.AppendHtml(li);
            }
            card.InnerHtml.AppendHtml(ul);

            output.Content.SetHtmlContent(card);
        }
    }
}
