#pragma checksum "C:\HCAProject\HRAudit\HCAHRGPortal\HCAaudit.Service.Portal.AuditUI\Views\SubCategory\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "bb7306b7b3a2c354a2bf2cb11347e53da8c563ec"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_SubCategory_Details), @"mvc.1.0.view", @"/Views/SubCategory/Details.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bb7306b7b3a2c354a2bf2cb11347e53da8c563ec", @"/Views/SubCategory/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"88be8cfa3e9748dac7fd5dc8935ddda98e9cef8b", @"/Views/_ViewImports.cshtml")]
    public class Views_SubCategory_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<HCAaudit.Service.Portal.AuditUI.Models.CatSubCatJoinMast>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\HCAProject\HRAudit\HCAHRGPortal\HCAaudit.Service.Portal.AuditUI\Views\SubCategory\Details.cshtml"
  
    ViewData["Title"] = "Home Page";

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

<script src=""https://code.jquery.com/ui/1.11.1/jquery-ui.min.js""></script>
<link rel=""stylesheet"" href=""https://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css"" />

<div class=""container"">
    <br />
    <div class=""row"">
        <div class=""col-sm-3""></div>
        <div class=""col-sm-3""></div>
        <div class=""col-sm-3""></div>
        <div class=""col-sm-3""><button type=""button"" id=""btnopenPopUp"" class=""bt");
            WriteLiteral(@"n btn-primary"">Add SubCategory</button> </div>
    </div>
    <br />
    <div style=""width:90%; margin:0 auto;"">
        <table id=""example"" class=""table table-striped table-bordered dt-responsive nowrap"" width=""100%"" cellspacing=""0"">
            <thead>
                <tr>
                    <th>SubCatID</th>
                    <th>Category Name</th>
                    <th>SubCategory Name</th>
                    <th>Edit</th>
                    <th>Delete</th>
                </tr>
            </thead>
        </table>
    </div>
</div>

<div id=""popupcontent"" title=""Add SubCategory"" hidden=""hidden"" class=""container"">

    <div class=""row"">
        <div class=""col-sm-4"">
            <label for=""format-pdf"" class=""input-group"">Category:</label>
            <select name=""teams"" id=""ddlcategory"" class=""form-control"">
            </select>
            <span id=""errormsgcategory"" style=""color:red"">Select Category</span>
        </div>
        <div class=""col-sm-6"">
            <");
            WriteLiteral(@"label for=""format-pdf"" style=""text-align:left;"">Sub Category</label>
            <input id=""txtsubcategory"" class=""form-control form-control-sm""
                   type=""text"" />
            <span id=""errormsgsubcategory"" style=""color:red"">Enter Sub-Category</span>
        </div>
        <div class=""col-sm-2""></div>
    </div>
    <br/>
    <div class=""row"">
        <div class=""col-sm-4""></div>
        <div class=""col-sm-2""></div>
        <div class=""col-sm-6"">
            <button id=""btncancelpopup"" class=""btn btn-danger"">Cancel</button>&nbsp;
            <button id=""btnaddSubCategory"" class=""btn btn-primary"">
                Save SubCategory
            </button>
        </div>
    </div>
</div>

<script>

    $(document).ready(function() {
        $(""#example"").DataTable({
            ""processing"": true, // for show progress bar
            ""serverSide"": true, // for process server side
            ""filter"": true, // this is for disable filter (search box)
            ""orderMulti");
            WriteLiteral(@""": false, // for disable multiple column at once
            ""ajax"": {
                ""url"": ""/SubCategory/Details"",
                ""type"": ""POST"",
                ""datatype"": ""json""
            },
            ""columnDefs"": [{
                ""targets"": [0],
                ""visible"": false,
                ""searchable"": false
            }],
            ""columns"": [
                { ""data"": ""SubCatgID"", ""name"": ""SubCatgID"", ""autoWidth"": true },
                { ""data"": ""CatgDescription"", ""name"": ""CatgDescription"", ""autoWidth"": true },
                { ""data"": ""SubCatgDescription"", ""name"": ""SubCatgDescription"", ""autoWidth"": true },
                {
                    ""render"": function (data, type, full, meta) { return '<a class=""btn btn-info"" href=""/SubCategory/Edit/' + full.SubCatgID + '"">Edit</a>'; }
                },
                {
                    data: null,
                    render: function (data, type, row) {
                        return ""<a href='#' class='btn ");
            WriteLiteral(@"btn-danger' onclick=DeleteData('"" + row.SubCatgID + ""'); >Delete</a>"";
                    }
                }
            ]

        });
    });

    $(function () {
        $(""#popupcontent"").dialog({
            autoOpen: false
        });

        $(""#btncancelpopup"").click(function () {
            $(""#popupcontent"").dialog('close');
        });

        $(""#btnopenPopUp"").click(function () {

            $.ajax({
                type: ""POST"",
                url: ""/SubCategory/GetCategory"",
                data: null,
                success: function (response) {
                    $('#ddlcategory').html(''); 
                    var options = '';
                    options += '<option value=""0"">Select Category</option>';
                    for (var i = 0; i < response.length; i++) {
                        options += '<option value=""' + response[i].CatgID + '"">' + response[i].CatgDescription + '</option>';
                    }
                    $('#ddlcategory').app");
            WriteLiteral(@"end(options);
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });

            $(""#popupcontent"").dialog({
                position: 'bottom'
            });

            $(""#popupcontent"").dialog('open');
        });
    });

    $(""#popupcontent"").dialog({ autoOpen: false, modal: true, height: 210, width: 700 });

    $('#txtsubcategory').on('keypress', function (event) {
        $('#errormsgsubcategory').hide();
        var regex = new RegExp(""^[a-zA-Z]+$"");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });
    $('#errormsgsubcategory').hide();
    $('#errormsgcategory').hide();

    $(""#ddlcategory"").change(function () ");
            WriteLiteral(@"{
        var catgID = $('#ddlcategory').val();
        if (catgID != ""0"") { $('#errormsgcategory').hide();}
    });

    $(""#btnaddSubCategory"").click(function () {
        var catgID = $('#ddlcategory').val();
        var subCategoryName = $('#txtsubcategory').val(); 
        if (catgID == ""0"") { $('#errormsgcategory').show(); return false;}
        if (subCategoryName == """") { $('#errormsgsubcategory').show(); return false;}
        
            $.ajax({
                type: ""POST"",
                url: ""/subcategory/Insert"",
                data: { ""catgID"": catgID, ""subCategoryName"": subCategoryName },
                success: function (response) {
                    if (response == ""1"") {
                        alert(""Record already exists!"");
                    }
                    else {
                        $(""#popupcontent"").dialog('close');
                        $('#txtsubcategory').val("""");
                        location.reload();
                    }
         ");
            WriteLiteral(@"       },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });
        
    });

    function DeleteData(subCatgID) {
    if (confirm(""Are you sure you want to delete ...?"")) {
        Delete(subCatgID);
    } else {
        return false;
    }
}

    function Delete(SubCatgID) {
    var url = '");
#nullable restore
#line 206 "C:\HCAProject\HRAudit\HCAHRGPortal\HCAaudit.Service.Portal.AuditUI\Views\SubCategory\Details.cshtml"
          Write(Url.Content("~/"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"' + ""subcategory/Delete"";

        $.post(url, { ID: SubCatgID }, function(data) {
        if (data) {
            oTable = $('#example').DataTable();
            oTable.draw();
        } else {
            alert(""Something Went Wrong!"");
        }
    });
}

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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<HCAaudit.Service.Portal.AuditUI.Models.CatSubCatJoinMast>> Html { get; private set; }
    }
}
#pragma warning restore 1591
