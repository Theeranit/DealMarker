<%@ Page Title="" Language="C#" MasterPageFile="~/Content.Master" AutoEventWireup="true" CodeBehind="PCEDetailReport.aspx.cs" Inherits="KK.DealMaker.Web.Report.PCEDetailReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentholder" runat="server">
    
    <div class="content-module-main">
        <div class="site-container">
            <div class="filtering" id="dvFiltering">
                <form id="Form1" runat="server">
                <label>
                    Report Date:
                    <input type="text" name="reportdate" id="reportdate" class="round input-form-textbox"/></label>
                    <label>Report Type:
                        <select id="reporttype" class="round input-from-dropdownlist">
                            <option value="EXT">BOD</option>
                            <option value="">Intraday</option>
                        </select>                    
                    </label>
                    <label>Counterparty:<select id="counterparty" class="round input-from-dropdownlist"></select></label>
                    <label>Product:<select id="product" name="product" class="round input-from-dropdownlist"></select></label>
                    <button type="submit" id="LoadRecordsButton" class="round blue button-submit">
                    Load records</button>
                </form>
            </div>
        </div>
    </div>
    <div class="table-container">
        <div id="TableContainer">
        </div>
    </div>      
      
    <script type="text/javascript">

        $(document).ready(function () {

            $('#reportdate').datepicker({ dateFormat: "dd/mm/yy", changeYear: true }).datepicker("setDate", '<%= CurrentProcessDate %>');

            set_options('counterparty', 'ALL', 'GetCounterpartyOptions');
            set_options('product', 'ALL', 'GetProductOptions');

            //Prepare jtable plugin
            var titleTable = 'Pre-Settlement Limit Detailed Report';
            $('#TableContainer').jtable({
                title: titleTable,
                paging: true,
                pageSize: 50,
                sorting: false,
                actions: {
                    listAction: '<%= Page.ResolveClientUrl("PCEDetailReport.aspx/GetPCEDetailReport") %>'
                },
                toolbar: {
                    items: [{
                        icon: '../Images/excel_icon17x17.png',
                        text: '<a href="#" class="export">Export to Excel</a>',
                        click: function () {
                            if ($('#TableContainer').data('reportdate') != undefined && $('#TableContainer').data('reportdate') != ""
                                && $('#TableContainer').data('reporttype') != undefined
                                && $('#TableContainer').data('counterparty') != undefined && $('#TableContainer').data('counterparty') != ""
                                && $('#TableContainer').data('product') != undefined && $('#TableContainer').data('product') != "") {
                                window.open('<%= Page.ResolveClientUrl("ExportToCSV.aspx?reportName=PCEDetail&") %>' + 'strReportDate=' + $('#TableContainer').data('reportdate')
                                                                                                                        + '&strTitle=' + $('#tableTitle').text()
                                                                                                                        + '&strReportType=' + $('#TableContainer').data('reporttype')
                                                                                                                        + '&strCtpy=' + $('#TableContainer').data('counterparty')
                                                                                                                        + '&strProduct=' + $('#TableContainer').data('product'), '', 'width=500,height=200');
                            }
                        }
                    }]
                },
                fields: {
                    EngineDate: {
                        title: 'Report Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    DMK_NO: {
                        title: 'DMK NO'
                    },
                    OPICS_NO: {
                        title: 'OPICS NO'
                    },
                    Source: {
                        title: 'Source'
                    },
                    Product: {
                        title: 'Product'
                    },
                    Portfolio: {
                        title: 'Portfolio'
                    },
                    TradeDate: {
                        title: 'Trade Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    EffectiveDate: {
                        title: 'Effective Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    MaturityDate: {
                        title: 'Maturity Date'
                        , type: 'date'
                        , displayFormat: 'dd-M-yy'
                    },
                    Instrument: {
                        title: 'Instrument'
                    },
                    Counterparty: {
                        title: 'Counterparty'
                    },
                    Country: {
                        title: 'Country'
                    },
                    FixedFloat1: {
                        title: 'Leg1'
                    },
                    Notional1: {
                        title: 'Notional1'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    CCY1: {
                        title: 'CCY1'
                    },
                    FixedFloat2: {
                        title: 'Leg2'
                    },
                    Notional2: {
                        title: 'Notional2'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    CCY2: {
                        title: 'CCY2'
                    },
                    KKPCCF: {
                        title: 'PCCF'
                    },
                    KKContribute: {
                        title: 'Contribute'
                        , type: 'number'
                        , listClass: 'right'
                    },
                    CSA: {
                        title: 'CSA'
                    }
                }
            });

            //Re-load records when user click 'load records' button.
            $('#LoadRecordsButton').click(function (e) {
                e.preventDefault();
                $('#tableTitle').text($('#reporttype option:selected').text() + ' Pre-Settlement Limit Detailed Report');
                $('#TableContainer').jtable('load', {
                    strReportDate: $('#reportdate').val(),
                    strReportType: $('#reporttype').val(),
                    strCtpy: $('#counterparty').val(),
                    strProduct: $('#product').val()
                });
                $('#TableContainer').data('reportdate', $('#reportdate').val());
                $('#TableContainer').data('reporttype', $('#reporttype').val());
                $('#TableContainer').data('counterparty', $('#counterparty').val());
                $('#TableContainer').data('product', $('#product').val());
            });

        });

    </script>
</asp:Content>
