#pragma checksum "C:\HCAProject\HRAudit\HCAHRGPortal\HCAaudit.Service.Portal.AuditUI\Views\HRRoster\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a6507cbdea9b4a4129d55fb15e5034a52812148f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_HRRoster_Details), @"mvc.1.0.view", @"/Views/HRRoster/Details.cshtml")]
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
#line 1 "C:\HCAProject\HRAudit\HCAHRGPortal\HCAaudit.Service.Portal.AuditUI\Views\_ViewImports.cshtml"
using HCAaudit.Service.Portal.AuditUI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\HCAProject\HRAudit\HCAHRGPortal\HCAaudit.Service.Portal.AuditUI\Views\_ViewImports.cshtml"
using HCAaudit.Service.Portal.AuditUI.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\HCAProject\HRAudit\HCAHRGPortal\HCAaudit.Service.Portal.AuditUI\Views\_ViewImports.cshtml"
using HCAaudit.Service.Portal.AuditUI.ViewModel.Patient;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a6507cbdea9b4a4129d55fb15e5034a52812148f", @"/Views/HRRoster/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"88be8cfa3e9748dac7fd5dc8935ddda98e9cef8b", @"/Views/_ViewImports.cshtml")]
    public class Views_HRRoster_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<HCAaudit.Service.Portal.AuditUI.Models.clstbHROCRosterList>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\HCAProject\HRAudit\HCAHRGPortal\HCAaudit.Service.Portal.AuditUI\Views\HRRoster\Details.cshtml"
  
    ViewData["Title"] = " HRRoster Page";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            WriteLiteral(@"
<script src=""https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js""></script>
<link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"">

<link href=""https://cdn.datatables.net/1.10.15/css/dataTables.bootstrap.min.css"" rel=""stylesheet"" />
<link href=""https://cdn.datatables.net/responsive/2.1.1/css/responsive.bootstrap.min.css"" rel=""stylesheet"" />

<script src=""https://cdn.datatables.net/1.10.15/js/jquery.dataTables.min.js""></script>
<script src=""https://cdn.datatables.net/1.10.15/js/dataTables.bootstrap4.min.js""></script>

<div class=""container"">
    <br />
    <div style=""width:100%; margin:0 auto;"">
        <table id=""example"" class=""table table-striped table-bordered dt-responsive nowrap"" width=""100%"" cellspacing=""0"">
            <thead>
                <tr>
                    <th>Employee 3-4 ID</th>
                    <th>Employee Full Name</th>
                    <!--<th>Employee Num</th>-->
                    <th>Job Cd Desc</");
            WriteLiteral(@"th>
                    <th>Position Desc</th>
                    <th>Employee Status Desc</th>
                    <th>StatusUpdate</th>
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
                ""url"": ""/HRRoster/Details"",
                ""type"": ""POST"",
                ""datatype"": ""json""
            },
            ""columnDefs"": [{
                ""targets"": [0],
                ""visible"": true,
                ""searchable"": true
            }],
            ""columns"": [
                { ""data"": ""EmployeethreefourID"", ""name"": ""EmployeethreefourID"", ""autoWidth"": true },
                { ""dat");
            WriteLiteral(@"a"": ""EmployeeFullName"", ""name"": ""EmployeeFullName"", ""autoWidth"": true },
                //{ ""data"": ""EmployeeNumber"", ""name"": ""EmployeeNumber"", ""autoWidth"": true },
                { ""data"": ""JobCDDesc"", ""name"": ""JobCDDesc"", ""autoWidth"": true },
                { ""data"": ""PositionDesc"", ""name"": ""PositionDesc"", ""autowidth"": true },
                { ""data"": ""EmployeeStatusDesc"", ""name"": ""EmployeeStatusDesc"", ""autowidth"": true },
                {
                    ""render"": function (data, type, full, meta) { return '<a class=""btn btn-info"" href=""/hrroster/statusupdate/' + full.EmployeethreefourID + '"">change status</a>'; }
                },
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<HCAaudit.Service.Portal.AuditUI.Models.clstbHROCRosterList> Html { get; private set; }
    }
}
#pragma warning restore 1591
