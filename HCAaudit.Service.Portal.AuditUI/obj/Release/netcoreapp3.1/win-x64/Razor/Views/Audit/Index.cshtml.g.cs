#pragma checksum "C:\HCAProjects\HCAHRGPortal\HCAaudit.Service.Portal.AuditUI\Views\Audit\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a076c91a33e1ad90feddfbb6c62fab05f6689707"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Audit_Index), @"mvc.1.0.view", @"/Views/Audit/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a076c91a33e1ad90feddfbb6c62fab05f6689707", @"/Views/Audit/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"88be8cfa3e9748dac7fd5dc8935ddda98e9cef8b", @"/Views/_ViewImports.cshtml")]
    public class Views_Audit_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\HCAProjects\HCAHRGPortal\HCAaudit.Service.Portal.AuditUI\Views\Audit\Index.cshtml"
  
    ViewData["Title"] = "Home Page";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<script src=""https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js""></script>
<link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"">
<link href=""https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"" rel=""stylesheet"" />
<link href=""https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.13/css/bootstrap-multiselect.css"" rel=""stylesheet"" />
<link href=""https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/css/bootstrap-datepicker.min.css"" rel=""stylesheet"" />

<script src=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js""></script>
<script src=""https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.13/js/bootstrap-multiselect.min.js""></script>
<script src=""https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/js/bootstrap-datepicker.min.js""></script>
<script src=""https://code.jquery.com/ui/1.12.1/jquery-ui.js""></script>

<link href=""https://cdn.datatabl");
            WriteLiteral(@"es.net/1.10.15/css/dataTables.bootstrap.min.css"" rel=""stylesheet"" />
<link href=""https://cdn.datatables.net/responsive/2.1.1/css/responsive.bootstrap.min.css"" rel=""stylesheet"" />

<script src=""https://cdn.datatables.net/1.10.15/js/jquery.dataTables.min.js""></script>
<script src=""https://cdn.datatables.net/1.10.15/js/dataTables.bootstrap4.min.js""></script>

<meta name=""viewport"" content=""width=device-width, initial-scale=1"">
<style>
    * {
        box-sizing: border-box;
    }

    #myInput {
        background-image: url('/css/searchicon.png');
        background-position: 10px 12px;
        background-repeat: no-repeat;
        width: 100%;
        font-size: 16px;
        padding: 12px 20px 12px 40px;
        border: 1px solid #ddd;
        margin-bottom: 12px;
    }

    #myUL {
        list-style-type: none;
        padding: 0;
        margin: 0;
    }

        #myUL li a {
            border: 1px solid #ddd;
            margin-top: -1px; /* Prevent double borders */
     ");
            WriteLiteral(@"       background-color: #f6f6f6;
            padding: 12px;
            text-decoration: none;
            font-size: 18px;
            color: black;
            display: block
        }

            #myUL li a:hover:not(.header) {
                background-color: #eee;
            }

    textarea {
        overflow-y: scroll;
        height: 100px;
        resize: none; /* Remove this if you want the user to resize the textarea */
    }
</style>
<div class=""card"">
    <div class=""row"">
        <div class=""col"">
            <label for=""format-pdf"" class=""input-group"">Enter Agent's 3-4 ID:</label>
            <input id=""txtcategory"" class=""form-control"" type=""text"" readonly />
        </div>
        <div class=""col"">
            <label for=""format-pdf"" class=""input-group"" "">Agent Information Name:</label>
            <input id=""txtcategory"" class=""form-control"" type=""text"" readonly />
        </div>
        <div class=""col"">
            <label for=""format-pdf"" class=""input-group""");
            WriteLiteral(@" "">Date of Auditor:</label>
            <input id=""txtcategory"" class=""form-control"" type=""text"" readonly />
        </div>
        <div class=""col"">
            <label for=""format-pdf"" class=""input-group"" "">Supervisor:</label>
            <input id=""txtcategory"" class=""form-control"" type=""text"" readonly />
        </div>
        <div class=""col"">
            <label for=""format-pdf"" class=""input-group"">MTD Audits:</label>
            <input id=""txtcategory"" class=""form-control"" type=""text"" readonly />
        </div>
        <div class=""col"">
            <label for=""format-pdf"" class=""input-group"">Auditor:</label>
            <input id=""txtAuditor"" class=""form-control"" type=""text"" readonly />
        </div>
    </div>
    <div class=""row""><hr /></div>
    <div class=""row"">
        <div class=""col"">
            <label for=""format-pdf"" class=""input-group"">Notes (clear, concise, shows steps taken) and Attachments (relevant/promotes portal adoption, i.e. links, step by step accessing information");
            WriteLiteral(@" all email communications attached)</label>
        </div>
        <div class=""col"">
            <input type=""radio"" class=""radio-success"" name=""optradio"" />
            <label class=""text-success"">Compliance</label>
            <input type=""radio"" class=""radio-danger"" name=""optradio"" />
            <label class=""text-danger"">Non Compliance</label>
            <input type=""radio"" class=""radio-primary"" name=""optradio"" checked />
            <label class=""text-primary"">Reset</label>
            <input type=""radio"" class=""radio-warning"" name=""optradio"" />
            <label class=""text-warning"">Not Applicable</label>
        </div>
        <div class=""col"">
            <label for=""format-pdf"" class=""input-group"">Notes (clear, concise, shows steps taken) and Attachments (relevant/promotes portal adoption, i.e. links, step by step accessing information all email communications attached)</label>
        </div>
        <div class=""col"">
            <input type=""radio"" class=""radio-success"" name=""optr");
            WriteLiteral(@"adio"" />
            <label class=""text-success"">Compliance</label>
            <input type=""radio"" class=""radio-danger"" name=""optradio"" />
            <label class=""text-danger"">Non Compliance</label>
            <input type=""radio"" class=""radio-primary"" name=""optradio"" checked />
            <label class=""text-primary"">Reset</label>
            <input type=""radio"" class=""radio-warning"" name=""optradio"" />
            <label class=""text-warning"">Not Applicable</label>
        </div>
    </div>
</div>
<div class=""row""><hr /></div>
<div class=""row"">
    <div class=""col"">
        <label for=""format-pdf"" class=""input-group"">Notes (clear, concise, shows steps taken) and Attachments (relevant/promotes portal adoption, i.e. links, step by step accessing information all email communications attached)</label>
    </div>
    <div class=""col"">
        <input type=""radio"" class=""radio-success"" name=""optradio"" />
        <label class=""text-success"">Compliance</label>
        <input type=""radio"" class=""r");
            WriteLiteral(@"adio-danger"" name=""optradio"" />
        <label class=""text-danger"">Non Compliance</label>
        <input type=""radio"" class=""radio-primary"" name=""optradio"" checked />
        <label class=""text-primary"">Reset</label>
        <input type=""radio"" class=""radio-warning"" name=""optradio"" />
        <label class=""text-warning"">Not Applicable</label>
    </div>
    <div class=""col"">
        <label for=""format-pdf"" class=""input-group"">Notes (clear, concise, shows steps taken) and Attachments (relevant/promotes portal adoption, i.e. links, step by step accessing information all email communications attached)</label>
    </div>
    <div class=""col"">
        <input type=""radio"" class=""radio-success"" name=""optradio"" />
        <label class=""text-success"">Compliance</label>
        <input type=""radio"" class=""radio-danger"" name=""optradio"" />
        <label class=""text-danger"">Non Compliance</label>
        <input type=""radio"" class=""radio-primary"" name=""optradio"" checked />
        <label class=""text-primary");
            WriteLiteral(@""">Reset</label>
        <input type=""radio"" class=""radio-warning"" name=""optradio"" />
        <label class=""text-warning"">Not Applicable</label>
    </div>
</div> <div class=""row""><hr /></div>
<div class=""row"">
    <div class=""col"">
        <label for=""format-pdf"" class=""input-group""></label><label for=""format-pdf"" class=""input-group""></label>
        <label for=""format-pdf"" class=""input-group"">Notes (clear, concise, shows steps taken) and Attachments (relevant/promotes portal adoption, i.e. links, step by step accessing information all email communications attached)</label>
    </div>
    <div class=""col"">
        <input type=""radio"" class=""radio-success"" name=""optradio"" />
        <label class=""text-success"">Compliance</label>
        <input type=""radio"" class=""radio-danger"" name=""optradio"" />
        <label class=""text-danger"">Non Compliance</label>
        <input type=""radio"" class=""radio-primary"" name=""optradio"" checked />
        <label class=""text-primary"">Reset</label>
        <input t");
            WriteLiteral(@"ype=""radio"" class=""radio-warning"" name=""optradio"" />
        <label class=""text-warning"">Not Applicable</label>
    </div>
    <div class=""col"">
        <label for=""format-pdf"" class=""input-group"">Notes (clear, concise, shows steps taken) and Attachments (relevant/promotes portal adoption, i.e. links, step by step accessing information all email communications attached)</label>
    </div>
    <div class=""col"">
        <input type=""radio"" class=""radio-success"" name=""optradio"" />
        <label class=""text-success"">Compliance</label>
        <input type=""radio"" class=""radio-danger"" name=""optradio"" />
        <label class=""text-danger"">Non Compliance</label>
        <input type=""radio"" class=""radio-primary"" name=""optradio"" checked />
        <label class=""text-primary"">Reset</label>
        <input type=""radio"" class=""radio-warning"" name=""optradio"" />
        <label class=""text-warning"">Not Applicable</label>
    </div>
</div> <div class=""row""><hr /></div>
<div class=""row"">
    <div class=""col"">");
            WriteLiteral(@"
        <label for=""format-pdf"" class=""input-group""></label><label for=""format-pdf"" class=""input-group""></label>
        <label for=""format-pdf"" class=""input-group"">Notes (clear, concise, shows steps taken) and Attachments (relevant/promotes portal adoption, i.e. links, step by step accessing information all email communications attached)</label>
    </div>
    <div class=""col"">
        <input type=""radio"" class=""radio-success"" name=""optradio"" />
        <label class=""text-success"">Compliance</label>
        <input type=""radio"" class=""radio-danger"" name=""optradio"" />
        <label class=""text-danger"">Non Compliance</label>
        <input type=""radio"" class=""radio-primary"" name=""optradio"" checked />
        <label class=""text-primary"">Reset</label>
        <input type=""radio"" class=""radio-warning"" name=""optradio"" />
        <label class=""text-warning"">Not Applicable</label>
    </div>
    <div class=""col"">
        <label for=""format-pdf"" class=""input-group"">Notes (clear, concise, shows steps t");
            WriteLiteral(@"aken) and Attachments (relevant/promotes portal adoption, i.e. links, step by step accessing information all email communications attached)</label>
    </div>
    <div class=""col"">
        <input type=""radio"" class=""radio-success"" name=""optradio"" />
        <label class=""text-success"">Compliance</label>
        <input type=""radio"" class=""radio-danger"" name=""optradio"" />
        <label class=""text-danger"">Non Compliance</label>
        <input type=""radio"" class=""radio-primary"" name=""optradio"" checked />
        <label class=""text-primary"">Reset</label>
        <input type=""radio"" class=""radio-warning"" name=""optradio"" />
        <label class=""text-warning"">Not Applicable</label>
    </div>
</div> <div class=""row""><hr /></div>
<div class=""row"">
    <div class=""col"">
        <label for=""format-pdf"" class=""input-group""></label><label for=""format-pdf"" class=""input-group""></label>
        <label for=""format-pdf"" class=""input-group"">Did WFA Rep facilitate all necessary research before taking action?</");
            WriteLiteral(@"label>
    </div>
    <div class=""col"">
        <input type=""radio"" class=""radio-success"" name=""optradio"" />
        <label class=""text-success"">Compliance</label>
        <input type=""radio"" class=""radio-danger"" name=""optradio"" />
        <label class=""text-danger"">Non Compliance</label>
        <input type=""radio"" class=""radio-primary"" name=""optradio"" checked />
        <label class=""text-primary"">Reset</label>
        <input type=""radio"" class=""radio-warning"" name=""optradio"" />
        <label class=""text-warning"">Not Applicable</label>
    </div>
    <div class=""col"">
        <label for=""format-pdf"" class=""input-group"">Notes (clear, concise, shows steps taken) and Attachments (relevant/promotes portal adoption, i.e. links, step by step accessing information all email communications attached)</label>
    </div>
    <div class=""col"">
        <input type=""radio"" class=""radio-success"" name=""optradio"" />
        <label class=""text-success"">Compliance</label>
        <input type=""radio"" class=""");
            WriteLiteral(@"radio-danger"" name=""optradio"" />
        <label class=""text-danger"">Non Compliance</label>
        <input type=""radio"" class=""radio-primary"" name=""optradio"" checked />
        <label class=""text-primary"">Reset</label>
        <input type=""radio"" class=""radio-warning"" name=""optradio"" />
        <label class=""text-warning"">Not Applicable</label>
    </div>
</div> <div class=""row""><hr /></div>
<div class=""well"">
    <div class=""row"">
        <div class=""col"">
            <label for=""format-pdf"" class=""input-group"">Total Score (Actual):</label>
            <input id=""txtScoreActual"" class=""form-control"" type=""text"" value=""0"" readonly />
        </div>
        <div class=""col""></div>
        <div class=""col"">
            <label for=""format-pdf"" class=""input-group"">Total Score (Possible):</label>
            <input id=""txtScorePossible"" class=""form-control"" type=""text"" value=""0"" readonly />
        </div>
        <div class=""col""></div>
        <div class=""col"">
            <label for=""format-p");
            WriteLiteral(@"df"" class=""input-group"">Total Score (Percent):</label>
            <input id=""txtScorePercent"" class=""form-control"" type=""text"" value=""0.00%"" readonly />
        </div>
    </div>
</div>
<div class=""form-group"">
    <label for=""auditnotes"" class=""text-primary"">Audit Notes:</label>
    <textarea class=""form-control"" rows=""4"" id=""auditnotes""></textarea>
</div>
<div class=""row"">
    <div class=""col"">
        <button id=""btnAnswerNo"" class=""btn btn-primary"">
            Return to Toolbox (Will NOT Submit Audit)
        </button>
    </div>
    <div class=""col""></div>
    <div class=""col""></div>
    <div class=""col"">
        <button id=""btnAnswerYes"" class=""btn btn-success"">
            Submit Audit
        </button>
    </div>
    <div class=""col""></div>
    <div class=""col""></div>
    <div class=""col""></div>
    <div class=""col"">
       
    </div>
</div>
<div class=""row""><hr /></div>
</div>
");
            DefineSection("Scripts", async() => {
                WriteLiteral(@"
    <!-- Script for the date range picker -->
    <script type=""text/javascript"" src=""~/js/daterangepicker.js""></script>
    <link rel=""stylesheet""
          type=""text/css""
          href=""~/css/daterangepicker.css"" />
    <!-- Script for the date range picker -->
    <script type=""text/javascript"">
        $(document).ready(function () {
            $('.datepicker').datepicker();
            $(""#CatgID"").change(function () {
                var categoryID = $('#CatgID').val();
                $.ajax({
                    type: ""POST"",
                    url: ""/search/BindSubCategory"",
                    data: { ""categoryID"": categoryID },
                    success: function (response) {
                        $('#SubCatID').html(''); $(""#ShowQuestions"").hide();
                        var options = '';
                        options += '<option value=""Select"">Select SubCategory</option>';
                        for (var i = 0; i < response.length; i++) {
                        ");
                WriteLiteral(@"    options += '<option value=""' + response[i].subCatgID + '"">' + response[i].subCatgDescription + '</option>';
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

        });

        $(function () {
            $('input[name=""dtstartdate""]').daterangepicker({
                singleDatePicker: true,
                showDropdowns: true,
                minYear: 1901,
                maxYear: parseInt(moment().format('YYYY'), 10)
            });
            $('input[name=""dtenddate""]').daterangepicker({
                singleDatePicker: true,
                showDropdowns: true,
                minYear: 1901,
                maxYear: parseInt(mom");
                WriteLiteral(@"ent().format('YYYY'), 10)
            });
        });
        $(""#btnaddCategory"").click(function () {

            var subCategoryName = $('#txtcategory').val(); // + ""^"" + $('#txtquestion').val() + ""^"" + $(""#txtquestionvolumn"").val();
            $.ajax({
                type: ""POST"",
                url: ""/category/Insert"",
                data: { ""subcatgname"": subCategoryName },
                success: function (response) {
                    $(""#popupcontent"").dialog('close');
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });

        });
    </script>
");
            }
            );
            WriteLiteral(@"
<script>
    $(document).ready(function () {
        $('select').multiselect({
            templates: { // Use the Awesome Bootstrap Checkbox structure
                li: '<li class=""checkList""><a tabindex=""0""><div class=""aweCheckbox aweCheckbox-danger""><label for=""""></label></div></a></li>'
            }
        });
        $('.multiselect-container div.aweCheckbox').each(function (index) {

            var id = 'multiselect-' + index,
                $input = $(this).find('input');

            // Associate the label and the input
            $(this).find('label').attr('for', id);
            $input.attr('id', id);

            // Remove the input from the label wrapper
            $input.detach();

            // Place the input back in before the label
            $input.prependTo($(this));

            $(this).click(function (e) {
                // Prevents the click from bubbling up and hiding the dropdown
                e.stopPropagation();
            });

        });
");
            WriteLiteral(@"    });
</script>

<style>
    body {
        padding: 20px;
    }

    form {
        max-width: 500px;
        margin: auto;
    }

    .aweCheckbox {
        padding-left: 20px;
    }

        .aweCheckbox label {
            display: inline-block;
            vertical-align: middle;
            position: relative;
            padding: 0 20px 0 10px;
            cursor: pointer;
        }

            .aweCheckbox label::before {
                content: """";
                display: inline-block;
                position: absolute;
                width: 17px;
                height: 17px;
                left: 0;
                margin-left: -20px;
                border: 1px solid #cccccc;
                border-radius: 3px;
                background-color: #fff;
                -webkit-transition: border 0.15s ease-in-out, color 0.15s ease-in-out;
                -o-transition: border 0.15s ease-in-out, color 0.15s ease-in-out;
                transition: border 0.");
            WriteLiteral(@"15s ease-in-out, color 0.15s ease-in-out;
            }

            .aweCheckbox label::after {
                display: inline-block;
                position: absolute;
                width: 16px;
                height: 16px;
                left: 0;
                top: 0;
                margin-left: -20px;
                padding-left: 3px;
                padding-top: 1px;
                font-size: 11px;
                color: #555555;
            }

        .aweCheckbox input[type=""checkbox""] {
            opacity: 0;
            z-index: 1;
        }

            .aweCheckbox input[type=""checkbox""]:focus + label::before {
                outline: thin dotted;
                outline: 5px auto -webkit-focus-ring-color;
                outline-offset: -2px;
            }

            .aweCheckbox input[type=""checkbox""]:checked + label::after {
                font-family: ""FontAwesome"";
                content: ""\f00c"";
            }

            .aweCheckbox input[t");
            WriteLiteral(@"ype=""checkbox""]:indeterminate + label::after {
                display: block;
                content: """";
                width: 10px;
                height: 3px;
                background-color: #555555;
                border-radius: 2px;
                margin-left: -16.5px;
                margin-top: 7px;
            }

            .aweCheckbox input[type=""checkbox""]:disabled + label {
                opacity: 0.65;
            }

                .aweCheckbox input[type=""checkbox""]:disabled + label::before {
                    background-color: #eeeeee;
                    cursor: not-allowed;
                }

        .aweCheckbox.aweCheckbox-circle label::before {
            border-radius: 50%;
        }

        .aweCheckbox.aweCheckbox-inline {
            margin-top: 0;
        }

    .aweCheckbox-danger input[type=""checkbox""]:checked + label::before {
        background-color: #d9534f;
        border-color: #d9534f;
    }

    .aweCheckbox-danger input[type=""");
            WriteLiteral(@"checkbox""]:checked + label::after {
        color: #fff;
    }

    .aweCheckbox-danger input[type=""checkbox""]:indeterminate + label::before {
        background-color: #d9534f;
        border-color: #d9534f;
    }

    .aweCheckbox-danger input[type=""checkbox""]:indeterminate + label::after {
        background-color: #fff;
    }

    input[type=""checkbox""].styled:checked + label:after {
        font-family: 'FontAwesome';
        content: ""\f00c"";
    }

    input[type=""checkbox""] .styled:checked + label::before {
        color: #fff;
    }

    input[type=""checkbox""] .styled:checked + label::after {
        color: #fff;
    }
</style>

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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
