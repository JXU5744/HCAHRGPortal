#pragma checksum "C:\HCAProjects\HCAHRGPortal\HCAaudit.Service.Portal.AuditUI\Views\Questions\Edit.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "883766a21b295e776065c4acaf2838d744bd9652"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Questions_Edit), @"mvc.1.0.view", @"/Views/Questions/Edit.cshtml")]
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
#line 1 "C:\HCAProjects\HCAHRGPortal\HCAaudit.Service.Portal.AuditUI\Views\_ViewImports.cshtml"
using HCAaudit.Service.Portal.AuditUI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\HCAProjects\HCAHRGPortal\HCAaudit.Service.Portal.AuditUI\Views\_ViewImports.cshtml"
using HCAaudit.Service.Portal.AuditUI.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\HCAProjects\HCAHRGPortal\HCAaudit.Service.Portal.AuditUI\Views\_ViewImports.cshtml"
using HCAaudit.Service.Portal.AuditUI.ViewModel.Patient;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"883766a21b295e776065c4acaf2838d744bd9652", @"/Views/Questions/Edit.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"88be8cfa3e9748dac7fd5dc8935ddda98e9cef8b", @"/Views/_ViewImports.cshtml")]
    public class Views_Questions_Edit : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<HCAaudit.Service.Portal.AuditUI.Models.tblQuestionBank>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("    <div class=\"container\">\r\n        <br />\r\n        \r\n            <div class=\"row\">\r\n                <div class=\"col-sm-3\"><label for=\"format-pdf\" class=\"input-group\">Question:</label></div>\r\n                <div class=\"col-sm-5\">\r\n");
#nullable restore
#line 8 "C:\HCAProjects\HCAHRGPortal\HCAaudit.Service.Portal.AuditUI\Views\Questions\Edit.cshtml"
                     using (Html.BeginForm())
                    {
                        

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\HCAProjects\HCAHRGPortal\HCAaudit.Service.Portal.AuditUI\Views\Questions\Edit.cshtml"
                   Write(Html.EditorFor(model => model.QuestionName, new { htmlAttributes = new { @class = "form-control form-control-sm" } }));

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "C:\HCAProjects\HCAHRGPortal\HCAaudit.Service.Portal.AuditUI\Views\Questions\Edit.cshtml"
                   Write(Html.ValidationMessageFor(model => model.QuestionName, "", new { @class = "text-danger" }));

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "C:\HCAProjects\HCAHRGPortal\HCAaudit.Service.Portal.AuditUI\Views\Questions\Edit.cshtml"
                                                                                                                   
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                    </div>\r\n                <div class=\"col-sm-1\"></div>\r\n                <div class=\"col-sm-3\"><button type=\"button\" id=\"btnopenPopUp\" class=\"btn btn-primary\">Update Question</button> </div>\r\n            </div>\r\n        </div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<HCAaudit.Service.Portal.AuditUI.Models.tblQuestionBank> Html { get; private set; }
    }
}
#pragma warning restore 1591