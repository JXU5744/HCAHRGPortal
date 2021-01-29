#pragma checksum "C:\HCAProjects\HCAHRGPortal\HCAaudit.Service.Portal.AuditUI\Views\Search\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d79a84c37146ed58e3b42d37c3bddaa508f00750"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Search_Details), @"mvc.1.0.view", @"/Views/Search/Details.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d79a84c37146ed58e3b42d37c3bddaa508f00750", @"/Views/Search/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"88be8cfa3e9748dac7fd5dc8935ddda98e9cef8b", @"/Views/_ViewImports.cshtml")]
    public class Views_Search_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<HCAaudit.Service.Portal.AuditUI.Models.BindSearchGrid>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\HCAProjects\HCAHRGPortal\HCAaudit.Service.Portal.AuditUI\Views\Search\Details.cshtml"
  
    ViewData["Title"] = " HRRoster Page";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<script src=""https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js""></script>
<link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"">

<link href=""https://cdn.datatables.net/1.10.15/css/dataTables.bootstrap.min.css"" rel=""stylesheet"" />
<link href=""https://cdn.datatables.net/responsive/2.1.1/css/responsive.bootstrap.min.css"" rel=""stylesheet"" />

<script src=""https://cdn.datatables.net/1.10.15/js/jquery.dataTables.min.js""></script>
<script src=""https://cdn.datatables.net/1.10.15/js/dataTables.bootstrap4.min.js""></script>


");
            WriteLiteral(@"
<div class=""container"">
    <br />
    <div style=""width:100%; margin:0 auto;"">
        <table id=""example"" class=""table table-striped table-bordered dt-responsive nowrap"" width=""100%"" cellspacing=""0"">
            <thead>
                <tr>
                    <th>Ticket Number</th>
                    <th>Created Date</th>
                    <th>Status</th>
                    <th>Subject</th>
                    <th>Service Group</th>
                    <th>Assigned To</th>
                </tr>
            </thead>
        </table>
    </div>
</div>
<script>
    $(document).ready(function () {
        $(""#example"").DataTable({
            ""processing"": true, // for show progress bar
            ""serverSide"": true, // for process server side
            ""filter"": true, // this is for disable filter (search box)
            ""orderMulti"": false, // for disable multiple column at once
            ""ajax"": {
                ""url"": ""/Search/Details"",
                ""type"": ""POST"",");
            WriteLiteral(@"
                ""datatype"": ""json""
            },
            ""columnDefs"": [{
                ""targets"": [0],
                ""visible"": true,
                ""searchable"": true
            }],
            ""columns"": [
                {
                    ""render"": function (data, type, full, meta) { return '<a class=""btn btn-info"" href=""https://www.google.com"" target=""_blank"">' + full.TicketNumber + '</a>'; }
                },
                { ""data"": ""CreatedDate"", ""name"": ""CreatedDate"", ""autoWidth"": true },
                { ""data"": ""Subject"", ""name"": ""Subject"", ""autoWidth"": true },
                { ""data"": ""status"", ""name"": ""status"", ""autoWidth"": true },
                { ""data"": ""ServiceGroup"", ""name"": ""ServiceGroup"", ""autowidth"": true },
                { ""data"": ""AssignedTo"", ""name"": ""AssignedTo"", ""autowidth"": true },
            ]
        });
    });
</script>






");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<HCAaudit.Service.Portal.AuditUI.Models.BindSearchGrid> Html { get; private set; }
    }
}
#pragma warning restore 1591
