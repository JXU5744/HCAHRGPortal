#pragma checksum "C:\Users\RHI5800\Documents\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\Questions\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4288724a3a93223ce3a1657c333e8a0ef4a0dbf1"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Questions_Index), @"mvc.1.0.view", @"/Views/Questions/Index.cshtml")]
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
#line 1 "C:\Users\RHI5800\Documents\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\_ViewImports.cshtml"
using HCAaudit.Service.Portal.AuditUI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\RHI5800\Documents\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\_ViewImports.cshtml"
using HCAaudit.Service.Portal.AuditUI.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\RHI5800\Documents\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\_ViewImports.cshtml"
using HCAaudit.Service.Portal.AuditUI.ViewModel.Patient;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4288724a3a93223ce3a1657c333e8a0ef4a0dbf1", @"/Views/Questions/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"88be8cfa3e9748dac7fd5dc8935ddda98e9cef8b", @"/Views/_ViewImports.cshtml")]
    public class Views_Questions_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<HCAaudit.Service.Portal.AuditUI.Models.tbQuestionMaster>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "teams", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("CatgID"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-control"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("title", new global::Microsoft.AspNetCore.Html.HtmlString("select"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.SelectTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\RHI5800\Documents\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\Questions\Index.cshtml"
  
    ViewData["Title"] = "Questions Page";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<script src=""https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js""></script>
<link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"">

<link href=""https://cdn.datatables.net/1.10.15/css/dataTables.bootstrap.min.css"" rel=""stylesheet"" />
<link href=""https://cdn.datatables.net/responsive/2.1.1/css/responsive.bootstrap.min.css"" rel=""stylesheet"" />

<script src=""https://cdn.datatables.net/1.10.15/js/jquery.dataTables.min.js""></script>
<script src=""https://cdn.datatables.net/1.10.15/js/dataTables.bootstrap4.min.js""></script>

<script src=""https://code.jquery.com/ui/1.11.1/jquery-ui.min.js""></script>
<link rel=""stylesheet"" href=""https://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css"" />

");
            WriteLiteral("\r\n<div class=\"container\">\r\n    <br />\r\n    <div class=\"row\">\r\n        <div class=\"col-sm-5\">\r\n            <label for=\"format-pdf\" style=\"text-align:left;\">Service Group / Department*</label>\r\n            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("select", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "4288724a3a93223ce3a1657c333e8a0ef4a0dbf16092", async() => {
                WriteLiteral("\r\n            ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.SelectTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Name = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
#nullable restore
#line 24 "C:\Users\RHI5800\Documents\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\Questions\Index.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Items = (new SelectList(@ViewBag.ListOfCategory, "CatgID", "CatgDescription"));

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-items", __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Items, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
        </div>
        <div class=""col-sm-4"">
            <label for=""format-pdf"" style=""text-align:left;"">Sub Category</label>
            <select name=""teams"" id=""SubCatID"" class=""form-control"">
            </select>
        </div>
        <div class=""col-sm-1""></div>
        <div class=""col-sm-2""><button type=""button"" id=""btnopenPopUp"" class=""btn btn-primary"">Add Question</button> </div>
    </div>

    <br />
    <div style=""width:100%; margin:0 auto;"">
        <table id=""tblquestions"" class=""table table-striped table-bordered dt-responsive nowrap"" width=""100%"" cellspacing=""0"">
            <thead>
                <tr>
                    <th>Question Score</th>
                    <th>Question Name</th>
                    <th>Edit</th>
                    <th>Delete</th>
                </tr>
            </thead>
        </table>
    </div>
</div>

<div id=""popupcontent"" title=""Map Question"" hidden=""hidden"" class=""container"">
    <div class=""row"">
        <div class=""col-sm-2");
            WriteLiteral(@"""></div>
        <div class=""col-sm-3""> <label class=""input-group"">Category:</label></div>
        <div class=""col-sm-5"">
            <input id=""txtcategory"" class=""form-control form-control-sm""
                   type=""text"" disabled=""disabled"" />
        </div>
        <div class=""col-sm-2""></div>
    </div>
    <br />
    <div class=""row"">
        <div class=""col-sm-2""></div>
        <div class=""col-sm-3""> <label class=""input-group"">SubCategory:</label></div>
        <div class=""col-sm-5"">
            <input id=""txtsubcategory"" class=""form-control form-control-sm""
                   type=""text"" disabled=""disabled"" />
        </div>
        <div class=""col-sm-2""></div>
    </div>
    <br />
    <div class=""row"">
        <div class=""col-sm-2""></div>
        <div class=""col-sm-3""> <label class=""input-group"">Question Volume:</label></div>
        <div class=""col-sm-3"">
            <input id=""txtquestionvolumn"" class=""form-control form-control-sm""
                   type=""text"" /><span i");
            WriteLiteral(@"d=""errmsg""></span>
        </div>
        <div class=""col-sm-4""></div>
    </div>
    <br />
    <div class=""row"">
        <div class=""col-sm-2""></div>
        <div class=""col-sm-3""> <label class=""input-group"">Question Text:</label></div>
        <div class=""col-sm-7"">
            <input id=""txtquestion"" class=""form-control form-control-sm""
                   type=""text"" />
        </div>
    </div>
    <br />
    <div class=""row"">
        <div class=""col-sm-3""></div>
        <div class=""col-sm-3""></div>
        <div class=""col-sm-3""><button id=""btnaddQuestion"" class=""btn btn-primary"">Add Question</button></div>
        <div class=""col-sm-3""></div>
    </div>
</div>

<script>
        $(document).ready(function () {
            $(function () {
                $(""#popupcontent"").dialog({
                    autoOpen: false
                });

                $(""#btnopenPopUp"").click(function () {
                    var subCategoryID = $(""#SubCatID option:selected"").text();
     ");
            WriteLiteral(@"               var categoryID = $(""#CatgID option:selected"").text();
                    $('#txtcategory').val(categoryID);
                    $('#txtsubcategory').val(subCategoryID);
                    $(""#popupcontent"").dialog({
                        position: 'bottom'
                    });

                    $(""#popupcontent"").dialog('open');
                });
            });

            $(""#btnopenPopUp"").click(function () {
                var subCategoryID = $('#SubCatID').val();
                $.ajax({
                    type: ""POST"",
                    url: ""/Questions/GetIndexCommaSeperated"",
                    data: { ""subCategoryID"": subCategoryID},
                    success: function (response) {
                        $(""#txtquestion"").autocomplete({
                            source: response
                        });
                    },
                    failure: function (response) {
                        alert(response.responseText);
       ");
            WriteLiteral(@"             },
                    error: function (response) {
                        alert(response.responseText);
                    }
                });

                $(""#popupcontent"").dialog('open');
            });

            $(""#btnopenPopUp"").hide();

            $(""#popupcontent"").dialog({ autoOpen: false, modal: true, height: 400, width: 950 });

            $(""#btnaddQuestion"").click(function () {
                if ($('#txtquestion').val() == """") {
                    alert(""Please enter Question Name field""); return false;
                }
                if ($(""#txtquestionvolumn"").val() == """") {
                    alert(""Please enter Question Volume field""); return false;
                }
                var subCategoryID = $('#SubCatID').val() + ""^"" + $('#txtquestion').val() + ""^"" + $(""#txtquestionvolumn"").val();
                $.ajax({
                    type: ""POST"",
                    url: ""/Questions/SaveQuestionForMaster"",
                    data: ");
            WriteLiteral(@"{ ""subcatgid"": subCategoryID },
                    success: function (response) {
                        if (response == ""2"") {
                            alert(""Not a valid Question"");
                        }
                        else {
                            $(""#popupcontent"").dialog('close');
                            //location.reload();
                        }
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    },
                    error: function (response) {
                        alert(response.responseText);
                    }
                });

            });

            $(""#CatgID"").change(function () {
                var categoryID = $('#CatgID').val();
                $.ajax({
                    type: ""POST"",
                    url: ""/Questions/BindSubCategory"",
                    data: { ""categoryID"": categoryID },
                    success: f");
            WriteLiteral(@"unction (response) {
                        $('#SubCatID').html(''); //$(""#ShowQuestions"").hide();
                        var options = '';
                        options += '<option value=""Select"">Select SubCategory</option>';
                        for (var i = 0; i < response.length; i++) {
                            options += '<option value=""' + response[i].SubCatgID + '"">' + response[i].SubCatgDescription + '</option>';
                        }
                        $('#SubCatID').append(options);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    },
                    error: function (response) {
                        alert(response.responseText);
                    }
                });

            });

            $(""#txtquestionvolumn"").keypress(function (e) {
                //if the letter is not digit then display error and don't type anything
                if (e.wh");
            WriteLiteral(@"ich != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    //display error message
                    $(""#errmsg"").html(""Digits Only"").show().fadeOut(""slow"");
                    return false;
                }
            });

            $(""#SubCatID"").change(function () {
                $(function () {
                    var subCategoryID = $('#SubCatID').val();
                    $.ajax({
                        type: ""Get"",
                        url: ""/Questions/BindGrid"",
                        data: { ""subCategoryID"": subCategoryID },
                        contentType: ""application/json; charset=utf-8"",
                        dataType: ""json"",
                        success: OnSuccess,
                        failure: function (response) {
                            alert(response.d);
                        },
                        error: function (response) {
                            alert(response.d);
                        }
          ");
            WriteLiteral(@"          });
                });
                function OnSuccess(response) {
                    $(""#btnopenPopUp"").show();
                    $(""#tblquestions"").DataTable(
                        {
                            destroy: true,
                            bLengthChange: false,
                            lengthMenu: [[5, 10, -1], [5, 10, ""All""]],
                            bFilter: false,
                            bSort: false,
                            bPaginate: false,
                            data: response,
                            columns: [{ 'data': 'QuestionScore'  },
                                { 'data': 'QuestionText' },
                                {
                                    ""render"": function (data, type, full, meta) { return '<a class=""btn btn-info"" href=""/Questions/Edit/' + full.QuestionID + '"">Edit</a>'; }
                                },
                                {
                                    data: null,
       ");
            WriteLiteral(@"                             render: function (data, type, row) {
                                        return ""<a href='#' class='btn btn-danger' onclick=DeleteData('"" + row.QuestionID + ""'); >Delete</a>"";
                                    }
                                },]
                        });
                };

            });

                function DeleteData(QuestionID) {
    if (confirm(""Are you sure you want to delete ...?"")) {
        Delete(QuestionID);
    } else {
        return false;
    }
}

    function Delete(QuestionID) {
    var url = '");
#nullable restore
#line 264 "C:\Users\RHI5800\Documents\HRAauditToolUI\HCAaudit.Service.Portal.AuditUI\Views\Questions\Index.cshtml"
          Write(Url.Content("~/"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"' + ""Questions/Delete"";

        $.post(url, { ID: QuestionID }, function(data) {
        if (data) {
            oTable = $('#example').DataTable();
            oTable.draw();
        } else {
            alert(""Something Went Wrong!"");
        }
    });
}

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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<HCAaudit.Service.Portal.AuditUI.Models.tbQuestionMaster>> Html { get; private set; }
    }
}
#pragma warning restore 1591
