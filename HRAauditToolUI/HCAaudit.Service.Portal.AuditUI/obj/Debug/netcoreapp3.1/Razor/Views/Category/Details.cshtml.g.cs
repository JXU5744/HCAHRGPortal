#pragma checksum "C:\HCAProjects\HCAHRGPortal\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\Category\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c8e1a2b83bda0ea9802a8857147818d49ca33bfc"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Category_Details), @"mvc.1.0.view", @"/Views/Category/Details.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\HCAProjects\HCAHRGPortal\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\_ViewImports.cshtml"
using HCAaudit.Service.Portal.AuditUI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\HCAProjects\HCAHRGPortal\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\_ViewImports.cshtml"
using HCAaudit.Service.Portal.AuditUI.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\HCAProjects\HCAHRGPortal\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\_ViewImports.cshtml"
using HCAaudit.Service.Portal.AuditUI.ViewModel.Patient;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c8e1a2b83bda0ea9802a8857147818d49ca33bfc", @"/Views/Category/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"88be8cfa3e9748dac7fd5dc8935ddda98e9cef8b", @"/Views/_ViewImports.cshtml")]
    public class Views_Category_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<HCAaudit.Service.Portal.AuditUI.Models.CategoryMast>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Home", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("role", new global::Microsoft.AspNetCore.Html.HtmlString("form"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\HCAProjects\HCAHRGPortal\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\Category\Details.cshtml"
  
    ViewData["Title"] = "Home Page";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c8e1a2b83bda0ea9802a8857147818d49ca33bfc5099", async() => {
                WriteLiteral("\r\n    <main class=\"container neu-margin--top-20\">\r\n        <fieldset>\r\n            <div id=\"ShowQuestions\" class=\"row neu-margin--bottom-20 \">\r\n");
#nullable restore
#line 10 "C:\HCAProjects\HCAHRGPortal\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\Category\Details.cshtml"
                 if (Model._categoryList.Count != 0)
                {

#line default
#line hidden
#nullable disable
                WriteLiteral(@"                    <div class=""neu-text col-6 gridHeader"" style=""background-color: black; color: white; padding: 2px;"">
                        <b>Category Name</b>
                    </div>
                    <div class=""neu-text col-1 gridHeader"" style=""text-align: center; background-color: black; color: white; padding: 2px;"">
                        <b>Edit</b>
                    </div>
                    <div class=""neu-text col-3 gridHeader"" style=""text-align: center; background-color: black; color: white; padding: 2px;"">
                        <b>Delete</b>
                    </div>
");
#nullable restore
#line 21 "C:\HCAProjects\HCAHRGPortal\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\Category\Details.cshtml"
                     for (var counter = 0; counter < Model._categoryList.Count; counter++)
                    {
                        

#line default
#line hidden
#nullable disable
#nullable restore
#line 23 "C:\HCAProjects\HCAHRGPortal\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\Category\Details.cshtml"
                         if (Model._categoryList[counter] != null)
                        {

#line default
#line hidden
#nullable disable
                WriteLiteral("                            <div class=\"neu-text col-6\">\r\n                                ");
#nullable restore
#line 26 "C:\HCAProjects\HCAHRGPortal\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\Category\Details.cshtml"
                           Write(Model._categoryList[counter].CatgDescription);

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                            </div>\r\n                            <div class=\"neu-button col-1\">\r\n                                ");
#nullable restore
#line 29 "C:\HCAProjects\HCAHRGPortal\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\Category\Details.cshtml"
                           Write(Html.ActionLink("Edit", "Edit", new { CatgID = @Model._categoryList[counter].CatgID }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                            </div>\r\n                            <div class=\"neu-button col-3\">\r\n                                ");
#nullable restore
#line 32 "C:\HCAProjects\HCAHRGPortal\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\Category\Details.cshtml"
                           Write(Html.ActionLink("Delete", "Delete", new { CatgID = @Model._categoryList[counter].CatgID }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                            </div>\r\n");
#nullable restore
#line 34 "C:\HCAProjects\HCAHRGPortal\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\Category\Details.cshtml"
                        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 34 "C:\HCAProjects\HCAHRGPortal\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\Category\Details.cshtml"
                         
                    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 35 "C:\HCAProjects\HCAHRGPortal\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\Category\Details.cshtml"
                     
                }

#line default
#line hidden
#nullable disable
                WriteLiteral("            </div>\r\n        </fieldset>\r\n    </main>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Controller = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n   \r\n\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<HCAaudit.Service.Portal.AuditUI.Models.CategoryMast> Html { get; private set; }
    }
}
#pragma warning restore 1591
